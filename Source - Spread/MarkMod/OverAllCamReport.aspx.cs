using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

public partial class OverAllCamReport : System.Web.UI.Page
{

    #region "loaddetails"

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds3 = new DataSet();
    static Boolean splhr_flag = false;
    static Hashtable ht_sphr = new Hashtable();
    Hashtable hat = new Hashtable();
    DataSet ds8 = new DataSet();
    DataSet ds_sphr = new DataSet();
    DataSet ds7 = new DataSet();
    Hashtable hvtb = new Hashtable();
    DataSet ds6 = new DataSet();
    string degree_codeparticularstudent = "";
    string collegecode = "";
    string value_holi_status = "";
    string group_user = "";
    double Present, Absent, hollydats, Leave, Onduty;
    double per_tage_date, cum_tage_date, per_tage_hrs, cum_tage_hrs;
    double per_workingdays = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;
    string frdate = "";
    string todate = "";
    string usercode = "";
    string strorder = "";
    double tot_ondu, per_tot_ondu, cum_tot_ondu, tot_ml, per_tot_ml;
    string singleuser = "";
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double per_ondu, per_leave, per_hhday, cum_ondu, cum_leave, cum_hhday;
    double per_con_hrs, cum_con_hrs;
    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0, tot_ml_spl_fals = 0;
    string strquery = "";
    double per_per_hrs, cum_per_perhrs;
    int mediumcount = 0;
    double cum_tot_point, per_holidate, cum_per_holidate;
    int per_dum_unmark, cum_dum_unmark, dum_unmark;
    int minpresII = 0;
    double workingdays = 0;
    int percount = 0;
    int grdcount = 0;
    string tempvalue = "-1";
    int NoHrs = 0;
    int tot_per_hrs, per_tot_per_hrs, cum_per_tot_per, tot_wok_hrs;
    int fnhrs = 0;
    int ObtValue = -1;
    int anhrs = 0;
    int dum_diff_date, unmark;
    int minpresI = 0;
    double leave_pointer, absent_pointer;

    double per_perhrs, per_abshrs, cum_perhrs, cum_abshrs;
    int cgpacount = 0;
    double njhr, njdate, per_njhr, per_njdate, cum_njhr, cum_njdate;
    int Dpasscount = 0;
    int countds = 0;
    int Hpasscount = 0;
    int Tpasscount = 0;
    string value, date;
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    int Epasscount = 0;
    int Nooffailcount = 0;
    int gendercount = 0;
    string[] split_holiday_status = new string[1000];

    int Gpasscount = 0;
    int Bpasscount = 0;
    DateTime per_from_gendate;
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    DateTime per_to_gendate;
    TimeSpan ts;
    int quotacount = 0;
    int moncount;
    int spancount = 0;
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    int girlpass = 0;

    string diff_date = "";
    DateTime per_from_date;
    DateTime per_to_date;
    DateTime dumm_from_date;
    int boypass = 0;
    int girl1fail = 0;
    int boy1fail = 0;
    string split_holiday_status_1 = "", split_holiday_status_2 = "";
    int passcnt = 0;
    int cal_from_date, cal_from_date_tmp, cal_from_cumdate_tmp;
    int cal_to_date, start_column = 0, cal_to_date_tmp, cal_to_cumdate_tmp;
    DateTime Admission_date;
    double dif_date1 = 0;
    double dif_date = 0;

    double leave_point, absent_point;
    int failcnt = 0;
    int passcnt1 = 0;
    int failcnt1 = 0;
    int notconsider_value = 0;
    double conduct_hour_new = 0;

    int gs_pass_count, bs_pass_count, gs_fail_count, bs_failcount, tot_gs_count, tot_bs_count;
    int gs_count, bs_count, eod_count, tot_stu, x1;
    int d_pass_count, h_pass_count, t_pass_count, e_pass_count;
    int d_fail_count, h_fail_count, t_fail_count, e_fail_count;
    int quota_count;
    string exam_code = "";
    string criteria_no = "";
    int iscount = 1;
    int holi_count;
    int min_mark, per_sub_count;
    double per_mark;
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;
    int passcount, failcount, maxcount, mincount, avg_50count, avg_65count, pre_count, ab_count, pperc_count, avg_count, avgg65count, opasscoun, ofailcount, opperc_count;
    int perc75, perc60to74, perc50to59, perc30to49, perc20to29, perc19, maxrollnum, minrollnum, exdate;
    int concolhours;
    int avg_60count;
    int avg_80count;
    //double per;
    int pass = 0, fail = 0;
    int mmyycount;
    string degreecode = "";
    string batchyear = "";
    string sem = "";
    string test = "";
    int subcou = 0;
    int noofabcout = 0;
    int rankcount = 0;
    int attendence = 0;
    int subjectfail = 0;
    int totalmarkclum = 0;
    int Percentageclum = 0;
    int Resultcolumn = 0;
    string rollno = "";
    string temproll = "";
    int sno = 0;
    int subsno = 0;
    int failcout = 0;
    int abse = 0;
    Double totmark = 0;
    int totstudcount = 0;
    Double maxmark = 0;
    string regularflag = "";
    string genderflag = "";
    string strdayflag = "";
    string result = "";
    Hashtable hatfailcount = new Hashtable();
    Dictionary<string, int> totalpercentage = new Dictionary<string, int>();
    Dictionary<string, double> totaltoppers = new Dictionary<string, double>();
    Dictionary<string, double> totaltopperstot = new Dictionary<string, double>();
    int stu = 0;
    int odcnt = 0;
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    Hashtable hatsubmarkavg = new Hashtable();
    int totstudent = 0;

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
            lblmsg.Visible = false;
            lblmessage1.Visible = false;
            errmsg.Visible = false;
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                hat.Clear();
                if ((group_user.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
                {
                    hat.Add("column_field", " and group_code='" + group_user + "'");
                }
                else
                {
                    hat.Add("column_field", " and user_code='" + Session["usercode"] + "'");
                }
                rbt.Items[2].Selected = true;
                ds.Dispose();
                ds.Reset();
                ds = d2.select_method("bind_college", hat, "sp");
                ddlcollege.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlcollege.DataSource = ds;
                    ddlcollege.DataTextField = "collname";
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataBind();
                    ddlcollege_SelectedIndexChanged(sender, e);


                    BindBatch();
                    BindDegree();
                    BindBranchMultiple();
                    bindsem();
                    bindsec();
                    GetTest();
                }
                else
                {
                    btnGo.Enabled = false;
                    ddlbatch.Enabled = false;
                    txtbranch.Enabled = false;
                    txtdegree.Enabled = false;
                    ddlsemester.Enabled = false;
                    ddlTest.Enabled = false;
                }
                clear();

                FpSpread1.Sheets[0].SheetName = " ";
                FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                rbtsubject.Items[0].Selected = true;
                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.HorizontalAlign = HorizontalAlign.Center;
                style1.ForeColor = Color.Black;
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].AllowTableCorner = true;
                FpSpread1.CommandBar.Visible = false;
                txtcriteria.Attributes.Add("readonly", "readonly");

                string grouporusercode = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }

                string Master1 = "select * from Master_Settings where " + grouporusercode + "";
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
            }
        }
        catch
        {
        }
    }

    public void BindBatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
                ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void BindDegree()
    {
        try
        {
            chkdegree.Checked = false;
            txtdegree.Text = "---Select---";
            chklsdegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, ddlcollege.SelectedValue.ToString(), usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsdegree.DataSource = ds;
                chklsdegree.DataTextField = "course_name";
                chklsdegree.DataValueField = "course_id";
                chklsdegree.DataBind();
                chklsdegree.Items[0].Selected = true;
                //if (Chgrade.Checked != true)
                //{
                for (int i = 0; i < chklsdegree.Items.Count; i++)
                {
                    chklsdegree.Items[i].Selected = true;
                }
                txtdegree.Text = "Degree (" + chklsdegree.Items.Count + ")";
                chkdegree.Checked = true;
                // }
                //else
                //{
                //    chklsdegree.Items[0].Selected = true;
                //    txtdegree.Text = "Degree (1)";
                //}
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void BindBranchMultiple()
    {
        try
        {
            chkbranch.Checked = false;
            txtbranch.Text = "---Select---";
            string course_id = "";
            for (int i = 0; i < chklsdegree.Items.Count; i++)
            {
                if (chklsdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklsdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklsdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            if (course_id != "")
            {
                ds = d2.BindBranchMultiple(singleuser, group_user, course_id, ddlcollege.SelectedValue.ToString(), usercode);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklsbranch.DataSource = ds;
                    chklsbranch.DataTextField = "dept_name";
                    chklsbranch.DataValueField = "degree_code";
                    chklsbranch.DataBind();
                    chklsbranch.Items[0].Selected = true;
                    //if (Chgrade.Checked != true)
                    //{
                    for (int i = 0; i < chklsbranch.Items.Count; i++)
                    {
                        chklsbranch.Items[i].Selected = true;
                    }
                    chkbranch.Checked = true;
                    txtbranch.Text = "Branch (" + chklsbranch.Items.Count + ")";
                    //}
                    //else
                    //{
                    //    chklsbranch.Items[0].Selected = true;
                    //    txtbranch.Text = "Branch (1)";
                    //}
                }
            }
        }
        catch (Exception ex)
        {
            //errmsg.Visible = true;
            //errmsg.Text = ex.ToString();
        }
    }

    public void bindsem()
    {
        try
        {
            ddlsemester.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;

            string strbranch = "";
            for (int b = 0; b < chklsbranch.Items.Count; b++)
            {
                if (chklsbranch.Items[b].Selected == true)
                {
                    if (strbranch.Trim() == "")
                    {
                        strbranch = chklsbranch.Items[b].Value;
                    }
                    else
                    {
                        strbranch = strbranch + ',' + chklsbranch.Items[b].Value;
                    }
                }
            }
            if (strbranch.Trim() != "")
            {
                strbranch = " and degree_code in(" + strbranch + ")";
            }

            strquery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + ddlcollege.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " " + strbranch + " order by NDurations desc";
            ds.Reset();
            ds.Dispose();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
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
            else
            {
                strquery = "select distinct duration,first_year_nonsemester  from degree where college_code=" + ddlcollege.SelectedValue.ToString() + " " + strbranch + " order by duration desc";
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

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
            }
        }
        catch (Exception ex)
        {
            //errmsg.Visible = true;
            //errmsg.Text = ex.ToString();
        }
    }

    public void bindsec()
    {
        //DataSet ds = new DataSet();
        string batch = "";
        string branch = "";
        ddlsec.Enabled = false;
        ddlsec.Items.Clear();
        ds.Reset();
        ds.Dispose();
        batch = ddlbatch.SelectedValue.ToString();
        for (int h = 0; h < chklsbranch.Items.Count; h++)
        {
            if (chklsbranch.Items[h].Selected == true)
            {
                if (branch == "")
                {
                    branch = chklsbranch.Items[h].Value;
                }
                else
                {
                    branch = branch + ',' + chklsbranch.Items[h].Value;
                }
            }
        }
        if (branch.Trim() != "")
        {
            ds.Reset();
            ds.Dispose();
            ds = d2.BindSectionDetail(batch, branch);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataValueField = "sections";
                ddlsec.DataBind();
                ddlsec.Items.Insert(0, "ALL");
                ddlsec.Enabled = true;
            }
            else
            {
                ddlsec.Enabled = false;
            }
        }
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    public void GetTest()
    {
        try
        {
            ddlTest.Items.Clear();
            collegecode = ddlcollege.SelectedValue.ToString();
            string strbranch = "";
            for (int b = 0; b < chklsbranch.Items.Count; b++)
            {
                if (chklsbranch.Items[b].Selected == true)
                {
                    if (strbranch.Trim() == "")
                    {
                        strbranch = chklsbranch.Items[b].Value;
                    }
                    else
                    {
                        strbranch = strbranch + ',' + chklsbranch.Items[b].Value;
                    }
                }
            }
            if (strbranch.Trim() != "")
            {
                strbranch = " and r.degree_code in(" + strbranch + ")";
            }

            strquery = "select distinct c.criteria from criteriaforinternal c,registration r,syllabus_master s where r.degree_code=s.degree_code and r.batch_year=s.batch_year and c.syll_code=s.syll_code and cc=0 and delflag=0 and r.exam_flag<>'debar'  and r.college_code='" + collegecode + "' and r.batch_year='" + ddlbatch.Text.ToString() + "' " + strbranch + " and s.semester='" + ddlsemester.SelectedItem.ToString() + "' order by criteria asc";
            ds.Reset();
            ds.Dispose();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlTest.Items.Clear();
                ddlTest.DataSource = ds;
                ddlTest.DataTextField = "Criteria";
                ddlTest.DataBind();
            }
            else
            {
                ddlTest.Items.Insert(0, "Select");
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void clear()
    {
        txtoptiminpassmark.Text = "";
        FpSpread1.Visible = false;
        rptprint.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        txtexcelname.Text = "";
        btnExcel.Visible = false;
        BtnPrint.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtoptiminpassmark.Text = "";
        clear();
        BindBatch();
        BindDegree();
        BindBranchMultiple();
        bindsem();
        bindsec();
        GetTest();
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        BindDegree();
        BindBranchMultiple();
        bindsem();
        bindsec();
        GetTest();
    }

    protected void chkdegree_ChekedChange(object sender, EventArgs e)
    {
        clear();
        if (chkdegree.Checked == true)
        {
            for (int i = 0; i < chklsdegree.Items.Count; i++)
            {
                chklsdegree.Items[i].Selected = true;
            }
            txtdegree.Text = "Degree (" + chklsdegree.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsdegree.Items.Count; i++)
            {
                chklsdegree.Items[i].Selected = false;
            }
            txtdegree.Text = "---Select---";
        }
        BindBranchMultiple();
        bindsem();
        bindsec();
        GetTest();
    }

    protected void chklsdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        int count = 0;
        txtdegree.Text = "---Select---";
        chkdegree.Checked = false;
        for (int i = 0; i < chklsdegree.Items.Count; i++)
        {
            if (chklsdegree.Items[i].Selected == true)
            {
                count++;
            }
        }
        if (count > 0)
        {
            txtdegree.Text = "Degree (" + count + ")";
            if (count == chklsdegree.Items.Count)
            {
                chkdegree.Checked = true;
            }
        }
        BindBranchMultiple();
        bindsem();
        bindsec();
        GetTest();
    }

    protected void chkbranch_ChekedChange(object sender, EventArgs e)
    {
        clear();
        if (chkbranch.Checked == true)
        {
            for (int i = 0; i < chklsbranch.Items.Count; i++)
            {
                chklsbranch.Items[i].Selected = true;
            }
            txtbranch.Text = "Branch (" + chklsbranch.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsbranch.Items.Count; i++)
            {
                chklsbranch.Items[i].Selected = false;
            }
            txtbranch.Text = "---Select---";
        }
        bindsem();
        bindsec();
        GetTest();
    }

    protected void chklsbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        int count = 0;
        txtbranch.Text = "---Select---";
        chkbranch.Checked = false;
        for (int i = 0; i < chklsbranch.Items.Count; i++)
        {
            if (chklsbranch.Items[i].Selected == true)
            {
                count++;
            }
        }
        if (count > 0)
        {
            txtbranch.Text = "Branch (" + count + ")";
            if (count == chklsbranch.Items.Count)
            {
                chkbranch.Checked = true;
            }
        }
        bindsem();
        bindsec();
        GetTest();
    }

    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        bindsec();
        GetTest();
    }

    protected void ddlTest_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void chkcriteria_CheckedChanged(object sender, EventArgs e)
    {
        //  clear();
        clear();
        rptprint.Visible = false;
        if (chkcriteria.Checked == true)
        {
            for (int i = 0; i < chklscriteria.Items.Count; i++)
            {
                chklscriteria.Items[i].Selected = true;
            }
            txtcriteria.Text = "Criteria (" + chklscriteria.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklscriteria.Items.Count; i++)
            {
                chklscriteria.Items[i].Selected = false;
            }
            txtcriteria.Text = "---Select---";
        }
    }

    protected void chklscriteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        //clear();
        clear();
        rptprint.Visible = false;
        int count = 0;
        txtcriteria.Text = "---Select---";
        chkcriteria.Checked = false;
        for (int i = 0; i < chklscriteria.Items.Count; i++)
        {
            if (chklscriteria.Items[i].Selected == true)
            {
                count++;
            }
        }
        if (count > 0)
        {
            txtcriteria.Text = "Criteria (" + count + ")";
            if (count == chklscriteria.Items.Count)
            {
                chkcriteria.Checked = true;
            }
        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                lblmsg.Visible = false;
                d2.printexcelreport(FpSpread1, reportname);
            }
            else
            {
                lblmsg.Text = "Please Enter Your Report Name";
                lblmsg.Visible = true;
                txtexcelname.Focus();
            }

        }
        catch
        {
        }
    }

    protected void ddlfail_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        if (DropDownList1.SelectedItem.Text != "Both")
        {
            txtcriteria.Enabled = false;
            txtoptiminpassmark.Enabled = false;
            txtoptiminpassmark.Text = "";
            chkcriteria.Checked = false;
            chklscriteria.ClearSelection();
            txtcriteria.Text = "Select All";
        }
        else
        {
            txtcriteria.Enabled = true;
            txtoptiminpassmark.Enabled = true;
            txtoptiminpassmark.Text = "";
        }
    }

    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        if (DropDownList2.SelectedItem.Text != "--Select--")
        {
            txtcriteria.Enabled = false;
            txtoptiminpassmark.Enabled = false;
            txtoptiminpassmark.Text = "";
            chkcriteria.Checked = false;
            chklscriteria.ClearSelection();
            txtcriteria.Text = " ";
        }
        else
        {
            txtcriteria.Enabled = true;
            txtoptiminpassmark.Enabled = true;
            txtoptiminpassmark.Text = "";
        }
    }

    protected void rbtselected(object sender, EventArgs e)
    {
        clear();
        if (rbt.Items[0].Selected == true)
        {
            DropDownList1.ClearSelection();
            DropDownList2.ClearSelection();
            DropDownList1.Enabled = false;
            DropDownList2.Enabled = false;
            txtcriteria.Enabled = false;
            chkcriteria.Checked = false;
            chklscriteria.ClearSelection();
            txtoptiminpassmark.Enabled = false;
            txtoptiminpassmark.Text = "";
            txtcriteria.Text = " ";
            Chgrade.Enabled = false;
            Chgrade.Checked = false;
        }
        else if (rbt.Items[1].Selected == true)
        {
            DropDownList1.Enabled = true;
            DropDownList2.Enabled = true;
            txtcriteria.Enabled = false;
            chkcriteria.Checked = false;
            chklscriteria.ClearSelection();
            txtoptiminpassmark.Enabled = false;
            txtoptiminpassmark.Text = "";
            txtcriteria.Text = " ";
            Chgrade.Enabled = false;
            Chgrade.Checked = false;
        }
        else if (rbt.Items[2].Selected == true)
        {
            DropDownList1.ClearSelection();
            DropDownList2.ClearSelection();
            DropDownList1.Enabled = true;
            DropDownList2.Enabled = true;
            txtcriteria.Enabled = true;
            txtoptiminpassmark.Enabled = true;
            txtoptiminpassmark.Text = "";
            Chgrade.Enabled = true;
        }
        //else if (rbt.Items[2].Selected == true)
        //{
        //    DropDownList1.Enabled = false;
        //    DropDownList2.Enabled = false;
        //}
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Printcontrol.loadspreaddetails(FpSpread1, "overallcamreport.aspx", "Over All Cam Report @ Date :" + DateTime.Now.ToString("dd/MM/yyyy") + "");
        Printcontrol.Visible = true;
    }

    #endregion

    public void spread()
    {
        try
        {
            int u = 0;
            FpSpread1.Visible = true;
            rptprint.Visible = true;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnExcel.Visible = true;
            BtnPrint.Visible = true;
            ArrayList arr_medium = new ArrayList();
            if (ds.Tables[1].Rows.Count > 0)
            {
                sno = 0;
                arr_medium.Clear();
                FpSpread1.Visible = true;
                lblrptname.Visible = true;
                rptprint.Visible = true;
                txtexcelname.Visible = true;
                btnExcel.Visible = true;
                BtnPrint.Visible = true;
                Dictionary<string, int> count = new Dictionary<string, int>();
                Dictionary<string, int> count1 = new Dictionary<string, int>();
                Dictionary<string, int> count2 = new Dictionary<string, int>();
                Dictionary<string, int> count3 = new Dictionary<string, int>();
                Dictionary<string, int> count4 = new Dictionary<string, int>();
                DataView dvw = new DataView();
                //  if (rbt.Items[2].Selected != true)
                // {
                //if (rbt.Items[0].Selected == true)
                //{
                //    ds.Tables[1].DefaultView.RowFilter = "min_mark>marks_obtained";
                //    dvw = ds.Tables[1].DefaultView;
                //}
                //else
                //{
                ds.Tables[1].DefaultView.RowFilter = "min_mark>marks_obtained ";
                dvw = ds.Tables[1].DefaultView;
                //   }
                //if (rbt.Items[0].Selected != true)
                //{
                //    for (int i = 0; i < dvw.Count; i++)
                //    {
                //        // if (DropDownList2.SelectedItem.Text != "--Select--")
                //        // {
                //        Double pn = Convert.ToInt32(dvw[i]["marks_obtained"].ToString());
                //        if (pn != -2.0 && pn != -3.0)
                //        {
                //            pn = Math.Round(pn, 2, MidpointRounding.AwayFromZero);

                //            string vlj = (dvw[i]["Roll_No"].ToString());
                //            if (!count.ContainsKey(vlj) && !count1.ContainsKey(vlj) && !count2.ContainsKey(vlj) && !count4.ContainsKey(vlj))
                //            {
                //                count.Add((dvw[i]["Roll_No"].ToString()), Convert.ToInt32(pn));
                //            }
                //            //}
                //            //else if (DropDownList2.SelectedItem.Text == "2Sub")
                //            //{
                //            else
                //            {
                //                if (!count1.ContainsKey((dvw[i]["Roll_No"].ToString())) && !count2.ContainsKey(vlj) && !count4.ContainsKey(vlj))
                //                {
                //                    count.Remove((dvw[i]["Roll_No"].ToString()));
                //                    count1.Add((dvw[i]["Roll_No"].ToString()), Convert.ToInt32(pn));
                //                }
                //                else
                //                {
                //                    if (!count2.ContainsKey((dvw[i]["Roll_No"].ToString())) && !count4.ContainsKey(vlj))
                //                    {
                //                        //  count.Remove((dvw[i]["Roll_No"].ToString()));
                //                        count1.Remove((dvw[i]["Roll_No"].ToString()));
                //                        count2.Add((dvw[i]["Roll_No"].ToString()), Convert.ToInt32(pn));
                //                    }
                //                    else
                //                    {
                //                        if (!count4.ContainsKey((dvw[i]["Roll_No"].ToString())))
                //                        {
                //                            // count.Remove((dvw[i]["Roll_No"].ToString()));
                //                            // count1.Remove((dvw[i]["Roll_No"].ToString()));
                //                            count2.Remove((dvw[i]["Roll_No"].ToString()));
                //                            count4.Add((dvw[i]["Roll_No"].ToString()), Convert.ToInt32(pn));
                //                        }
                //                    }
                //                }
                //            }
                //            //}
                //            //else if (DropDownList2.SelectedItem.Text == "3Sub")
                //            //{

                //        }
                //    }
                //}
                //else
                //{
                for (int i = 0; i < dvw.Count; i++)
                {
                    // if (DropDownList2.SelectedItem.Text != "--Select--")
                    // {
                    Double pn = Convert.ToDouble(dvw[i]["marks_obtained"].ToString());
                    if (pn != -2.0 && pn != -3.0)
                    {
                        pn = Math.Round(pn, 2, MidpointRounding.AwayFromZero);

                        string vlj = (dvw[i]["Roll_No"].ToString());
                        if (!count.ContainsKey(vlj) && !count1.ContainsKey(vlj) && !count2.ContainsKey(vlj) && !count4.ContainsKey(vlj))
                        {
                            count.Add((dvw[i]["Roll_No"].ToString()), Convert.ToInt32(pn));
                        }
                        //}
                        //else if (DropDownList2.SelectedItem.Text == "2Sub")
                        //{
                        else
                        {
                            if (!count1.ContainsKey((dvw[i]["Roll_No"].ToString())) && !count2.ContainsKey(vlj) && !count4.ContainsKey(vlj))
                            {
                                count.Remove((dvw[i]["Roll_No"].ToString()));
                                count1.Add((dvw[i]["Roll_No"].ToString()), Convert.ToInt32(pn));
                            }
                            else
                            {
                                if (!count2.ContainsKey((dvw[i]["Roll_No"].ToString())) && !count4.ContainsKey(vlj))
                                {
                                    //  count.Remove((dvw[i]["Roll_No"].ToString()));
                                    count1.Remove((dvw[i]["Roll_No"].ToString()));
                                    count2.Add((dvw[i]["Roll_No"].ToString()), Convert.ToInt32(pn));
                                }
                                else
                                {
                                    if (!count4.ContainsKey((dvw[i]["Roll_No"].ToString())))
                                    {
                                        // count.Remove((dvw[i]["Roll_No"].ToString()));
                                        // count1.Remove((dvw[i]["Roll_No"].ToString()));
                                        count2.Remove((dvw[i]["Roll_No"].ToString()));
                                        count4.Add((dvw[i]["Roll_No"].ToString()), Convert.ToInt32(pn));
                                    }
                                }
                            }
                        }
                        //}
                        //else if (DropDownList2.SelectedItem.Text == "3Sub")
                        //{

                    }
                }
                // }


                for (int stu = 0; stu < ds.Tables[1].Rows.Count; stu++)
                {
                    if (chtopper.Checked != true)
                    {
                        temproll = ds.Tables[1].Rows[stu]["Roll_No"].ToString();
                        if (DropDownList2.SelectedItem.Text == "1Sub")
                        {
                            if (count.Count > 0)
                            {
                                if (count.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                {
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            // Double perecentage = totmark / maxmark * 100;
                                            Double perecentage = 0;
                                            if (totmark != 0 && totstudcount != 0)
                                            {
                                                perecentage = totmark / totstudcount;
                                                perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                            }
                                            else
                                            {
                                                perecentage = 0;
                                            }
                                            sno++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                            if (ds.Tables[1].Rows[stu - 1]["stud_type"].ToString() == "Hostler")
                                            {
                                                if (result == "Pass")
                                                {
                                                    passcnt++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girlpass++;
                                                    }
                                                    else
                                                    {
                                                        boypass++;
                                                    }

                                                }
                                                else
                                                {
                                                    failcnt++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girl1fail++;
                                                    }
                                                    else
                                                    {
                                                        boy1fail++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (result == "Pass")
                                                {
                                                    passcnt1++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girlpass++;
                                                    }
                                                    else
                                                    {
                                                        boypass++;
                                                    }
                                                }
                                                else
                                                {
                                                    failcnt1++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girl1fail++;
                                                    }
                                                    else
                                                    {
                                                        boy1fail++;
                                                    }
                                                }
                                            }
                                            if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                            {
                                                totalpercentage.Add((rollno), Convert.ToInt32(perecentage));

                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;
                                            if (hatfailcount.Contains(failcout))
                                            {
                                                int fc = Convert.ToInt32(hatfailcount[failcout]);
                                                fc++;
                                                hatfailcount[failcout] = fc;
                                            }
                                            else
                                            {
                                                hatfailcount.Add(failcout, 1);
                                            }

                                        }
                                        string medium1 = "";
                                        //if (chklscriteria.Items[1].Selected == true)
                                        //{
                                        if (temproll != rollno)
                                        {
                                            if (rollno.Trim() != "")
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Left;
                                                medium1 = d2.GetFunctionv("select distinct TextVal from TextValTable  where TextCode in(select medium_ins from applyn where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + " and medium_ins is not NULL) and TextCriteria='medi'");
                                                if ((medium1 == "") || (medium1 == null))
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Text = "-";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Bold = false;

                                                }
                                                else
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Text = medium1.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Bold = false;


                                                }
                                            }
                                        }

                                        //}
                                        if (chklscriteria.Items[2].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    string s = "select  distinct c.course_id from degree as d, course as c where d.course_id=c.course_id and d.dept_code in(" + degreecode + ") "; //
                                                    DataSet ds4 = new DataSet();
                                                    ds4 = d2.select_method_wo_parameter(s, "text");
                                                    for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
                                                    {
                                                        string schoolgrd = d2.GetFunction("select edu_level from course where course_id= " + ds4.Tables[0].Rows[i]["course_id"].ToString() + "");
                                                        if (schoolgrd != string.Empty)
                                                        {
                                                            if (schoolgrd == "UG" || schoolgrd == "U.G")
                                                            {
                                                                string scholmrk = d2.GetFunction("select percentage from stud_prev_details as s,textvaltable as t where app_no= " + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%'))");

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Right;

                                                                if (scholmrk != string.Empty)
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = scholmrk.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }
                                                                else
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }

                                                                string scholmrk1 = d2.GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no= " + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%')))");

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                if (scholmrk1 != string.Empty)
                                                                {
                                                                    string sam = scholmrk1.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = scholmrk1.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                }
                                                                else
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                }

                                                            }

                                                        }
                                                        else if (schoolgrd == "PG" || schoolgrd == "P.G")
                                                        {
                                                            if (temproll != rollno)
                                                            {

                                                                string scholmrk2 = d2.GetFunction("select distinct percentage from stud_prev_details as s,textvaltable as t where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC'))and (textval not like '%H.SC(SB)') and (textval not like '%intermediate')");
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                if (scholmrk2 != string.Empty)
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = scholmrk2.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }
                                                                else
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }
                                                                string scholmrk3 = d2.GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and (textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC') and (textval not like '%H.SC(SB)') and (textval not like '%intermediate'))");
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                if (scholmrk3 != string.Empty)
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = scholmrk3.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;

                                                                }
                                                                else
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[3].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    degree_codeparticularstudent = ds.Tables[1].Rows[FpSpread1.Sheets[0].RowCount - 1]["degree_code"].ToString();
                                                    int sem1 = Convert.ToInt32(ddlsemester.SelectedValue.ToString());
                                                    double degcgpa = Math.Round(findgrade(ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["RollNumber"].ToString(), sem1), 2);
                                                    degcgpa = Math.Round(degcgpa, 2);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Text = degcgpa.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Bold = false;
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[3].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Text = failcout.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Bold = false;
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[8].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Day Scholar")
                                                        {

                                                            d_pass_count++;
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Bold = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Day Scholar")
                                                        {
                                                            d_fail_count++;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (chklscriteria.Items[9].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Hostler")
                                                        {
                                                            h_pass_count++;
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Bold = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Hostler")
                                                        {
                                                            h_fail_count++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[10].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                                                        {
                                                            t_pass_count++;
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Bold = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                                                        {
                                                            t_fail_count++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[11].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                                                        {
                                                            //if (!arr_medium.Contains(rollno))
                                                            //{
                                                            e_pass_count++;
                                                            //    arr_medium.Add(arr_medium);
                                                            //}
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                    }
                                                    else

                                                        if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                                                        {
                                                            e_fail_count++;
                                                        }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[20].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    int g = int.Parse(ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString());
                                                    string gender;
                                                    if (g == 1)
                                                    {
                                                        gender = "G";
                                                    }
                                                    else
                                                    {
                                                        gender = "B";
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Text = gender.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Bold = false;
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[13].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        gs_pass_count++;
                                                        tot_gs_count++;
                                                        gs_count = 1;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Text = gs_count.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Bold = false;
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[14].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "0")
                                                    {
                                                        bs_pass_count++;
                                                        tot_bs_count++;
                                                        bs_count = 1;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Text = bs_count.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Bold = false;
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[23].Selected == true)//modified on 01.08.12
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    string textval = "";
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["SeatType"].ToString() != "" && ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["SeatType"].ToString() != " ")
                                                    {
                                                        textval = d2.GetFunction("Select TextVal from textvaltable where textcode=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["seattype"].ToString() + "");
                                                    }
                                                    else
                                                    {
                                                        textval = "-";
                                                    }
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Text = textval.ToString();
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].HorizontalAlign = HorizontalAlign.Center;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Name = "Book Antiqua";
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Size = FontUnit.Medium;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Bold = false;
                                                    //quota_count = quotacount;
                                                }
                                            }
                                        }
                                        failcout = 0;

                                        result = "Pass";
                                        totmark = 0;
                                        totstudcount = 0;
                                        maxmark = 0;
                                        abse = 0;
                                        sno++;
                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = false;
                                        string degdetails = ds.Tables[1].Rows[stu]["Degreedetails"].ToString();
                                        string[] degsplit = degdetails.Split('-');
                                        string degree = "";
                                        if (degsplit.Length == 3)
                                        {
                                            if (degsplit[0] != "" && degsplit[0] != null)
                                            {
                                                degree = degsplit[0].ToString();
                                            }
                                            if (degsplit[1] != "" && degsplit[1] != null)
                                            {
                                                degree += " - " + degsplit[1].ToString();
                                            }
                                            if (degsplit[2] != "" && degsplit[2] != null)
                                            {
                                                degree += " - " + degsplit[2].ToString();
                                            }
                                        }
                                        else if (degsplit.Length == 2)
                                        {
                                            if (degsplit[0] != "" && degsplit[0] != null)
                                            {
                                                degree = degsplit[0].ToString();
                                            }
                                            if (degsplit[1] != "" && degsplit[1] != null)
                                            {
                                                degree += " - " + degsplit[1].ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (degsplit[0] != "" && degsplit[0] != null)
                                            {
                                                degree = degsplit[0].ToString();
                                            }
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = degree.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[1].Rows[stu]["Roll_No"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[1].Rows[stu]["Reg_no"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[1].Rows[stu]["Stud_Type"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[1].Rows[stu]["Stud_Name"].ToString();
                                        rollno = temproll;

                                    }
                                    string subcode = ds.Tables[1].Rows[stu]["Subject_Code"].ToString().Trim();
                                    if (hat.Contains(subcode))
                                    {
                                        int col = Convert.ToInt32(hat[subcode].ToString());
                                        string marks_per = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                        if (Convert.ToInt32(marks_per) < 0)
                                        {
                                            switch (marks_per)
                                            {
                                                case "-1":
                                                    marks_per = "AAA";
                                                    abse++;
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
                                                    odcnt++;
                                                    break;
                                                case "-17":
                                                    marks_per = "LA";

                                                    break;

                                                case "-18":
                                                    marks_per = "RAA";

                                                    break;

                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = marks_per;

                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                            subsno++;
                                        }
                                        string minmark = ds.Tables[1].Rows[stu]["min_mark"].ToString();
                                        if (minmark.Trim() != "" && minmark != null)
                                        {
                                            Double min = Convert.ToDouble(minmark);
                                            Double mark = Convert.ToDouble(ds.Tables[1].Rows[stu]["marks_obtained"].ToString());
                                            if (mark > 0)
                                            {
                                                totmark = totmark + mark;
                                                totstudcount = totstudcount + 1;
                                            }
                                            else
                                            {

                                            }
                                            maxmark = maxmark + Convert.ToDouble(ds.Tables[1].Rows[stu]["max_mark"].ToString());
                                            if (marks_per != "EL" && marks_per != "EOD")
                                            {
                                                if (mark < min)
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightPink;
                                                    failcout++;
                                                    result = "Fail";
                                                }
                                            }
                                            //   Double perecentage = totmark / maxmark * 100;

                                            double perecentage = 0;
                                            if (totmark != 0 && totstudcount != 0)
                                            {
                                                perecentage = totmark / totstudcount;
                                                perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                            }
                                            else
                                            {
                                                perecentage = 0;
                                            }


                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;

                                            //DataView dv = new DataView();
                                            //ds.Tables[1].DefaultView.RowFilter = "marks_obtained<0 and Roll_No='" + ds.Tables[1].Rows[stu]["Roll_No"].ToString() + "'";
                                            //dv = ds.Tables[1].DefaultView;
                                            //if (abse == 0)
                                            //{
                                            //    abse = dv.Count;
                                            //}
                                            //else
                                            //{
                                            //    abse = abse + dv.Count;
                                            //}
                                        }

                                    }

                                    if (stu == ds.Tables[1].Rows.Count - 1)
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();

                                        Double perecentage = 0;
                                        if (totmark != 0 && totstudcount != 0)
                                        {
                                            perecentage = totmark / totstudcount;
                                            perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                        }
                                        else
                                        {
                                            perecentage = 0;
                                        }

                                        //  Double perecentage = totmark / maxmark * 100;
                                        //Double perecentage = totmark / totstudcount;



                                        //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                        if (ds.Tables[1].Rows[stu - 1]["stud_type"].ToString() == "Hostler")
                                        {
                                            if (result == "Pass")
                                            {
                                                passcnt++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girlpass++;
                                                }
                                                else
                                                {
                                                    boypass++;
                                                }

                                            }
                                            else
                                            {
                                                failcnt++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girl1fail++;
                                                }
                                                else
                                                {
                                                    boy1fail++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (result == "Pass")
                                            {
                                                passcnt1++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girlpass++;
                                                }
                                                else
                                                {
                                                    boypass++;
                                                }
                                            }
                                            else
                                            {
                                                failcnt1++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girl1fail++;
                                                }
                                                else
                                                {
                                                    boy1fail++;
                                                }
                                            }
                                        }
                                        if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                        {
                                            totalpercentage.Add((ds.Tables[1].Rows[stu]["Roll_No"].ToString()), Convert.ToInt32(perecentage));

                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;
                                        if (hatfailcount.Contains(failcout))
                                        {
                                            int fc = Convert.ToInt32(hatfailcount[failcout]);
                                            fc++;
                                            hatfailcount[failcout] = fc;
                                        }
                                        else
                                        {
                                            hatfailcount.Add(failcout, 1);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                FpSpread1.Visible = false;
                                lblrptname.Visible = false;
                                rptprint.Visible = false;
                                txtexcelname.Visible = false;
                                btnExcel.Visible = false;
                                BtnPrint.Visible = false;
                                errmsg.Visible = true;
                                errmsg.Text = "No Records Found";
                                return;
                            }
                        }
                        if (DropDownList2.SelectedItem.Text == "2Sub")
                        {
                            if (count1.Count > 0)
                            {
                                if (count1.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                {
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            //  Double perecentage = totmark / maxmark * 100;
                                            //Double perecentage = totmark / totstudcount;

                                            //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                            Double perecentage = 0;
                                            if (totmark != 0 && totstudcount != 0)
                                            {
                                                perecentage = totmark / totstudcount;
                                                perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                            }
                                            else
                                            {
                                                perecentage = 0;
                                            }

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                            if (ds.Tables[1].Rows[stu - 1]["stud_type"].ToString() == "Hostler")
                                            {
                                                if (result == "Pass")
                                                {
                                                    passcnt++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girlpass++;
                                                    }
                                                    else
                                                    {
                                                        boypass++;
                                                    }

                                                }
                                                else
                                                {
                                                    failcnt++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girl1fail++;
                                                    }
                                                    else
                                                    {
                                                        boy1fail++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (result == "Pass")
                                                {
                                                    passcnt1++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girlpass++;
                                                    }
                                                    else
                                                    {
                                                        boypass++;
                                                    }
                                                }
                                                else
                                                {
                                                    failcnt1++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girl1fail++;
                                                    }
                                                    else
                                                    {
                                                        boy1fail++;
                                                    }
                                                }
                                            }
                                            if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                            {
                                                totalpercentage.Add((rollno), Convert.ToInt32(perecentage));

                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;
                                            if (hatfailcount.Contains(failcout))
                                            {
                                                int fc = Convert.ToInt32(hatfailcount[failcout]);
                                                fc++;
                                                hatfailcount[failcout] = fc;
                                            }
                                            else
                                            {
                                                hatfailcount.Add(failcout, 1);
                                            }

                                        }
                                        string medium1 = "";
                                        //if (chklscriteria.Items[1].Selected == true)
                                        //{
                                        if (temproll != rollno)
                                        {
                                            if (rollno.Trim() != "")
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Left;
                                                medium1 = d2.GetFunctionv("select distinct TextVal from TextValTable  where TextCode in(select medium_ins from applyn where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + " and medium_ins is not NULL) and TextCriteria='medi'");
                                                if ((medium1 == "") || (medium1 == null))
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Text = "-";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Bold = false;

                                                }
                                                else
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Text = medium1.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Bold = false;


                                                }
                                            }
                                        }

                                        //}
                                        if (chklscriteria.Items[2].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    string s = "select  distinct c.course_id from degree as d, course as c where d.course_id=c.course_id and d.dept_code in(" + degreecode + ") "; //
                                                    DataSet ds4 = new DataSet();
                                                    ds4 = d2.select_method_wo_parameter(s, "text");
                                                    for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
                                                    {
                                                        string schoolgrd = d2.GetFunction("select edu_level from course where course_id= " + ds4.Tables[0].Rows[i]["course_id"].ToString() + "");
                                                        if (schoolgrd != string.Empty)
                                                        {
                                                            if (schoolgrd == "UG" || schoolgrd == "U.G")
                                                            {
                                                                string scholmrk = d2.GetFunction("select percentage from stud_prev_details as s,textvaltable as t where app_no= " + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%'))");

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Right;

                                                                if (scholmrk != string.Empty)
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = scholmrk.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }
                                                                else
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }

                                                                string scholmrk1 = d2.GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no= " + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%')))");

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                if (scholmrk1 != string.Empty)
                                                                {
                                                                    string sam = scholmrk1.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = scholmrk1.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                }
                                                                else
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                }

                                                            }

                                                        }
                                                        else if (schoolgrd == "PG" || schoolgrd == "P.G")
                                                        {
                                                            if (temproll != rollno)
                                                            {

                                                                string scholmrk2 = d2.GetFunction("select distinct percentage from stud_prev_details as s,textvaltable as t where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC'))and (textval not like '%H.SC(SB)') and (textval not like '%intermediate')");
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                if (scholmrk2 != string.Empty)
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = scholmrk2.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }
                                                                else
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }
                                                                string scholmrk3 = d2.GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and (textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC') and (textval not like '%H.SC(SB)') and (textval not like '%intermediate'))");
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                if (scholmrk3 != string.Empty)
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = scholmrk3.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;

                                                                }
                                                                else
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[3].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    degree_codeparticularstudent = ds.Tables[1].Rows[FpSpread1.Sheets[0].RowCount - 1]["degree_code"].ToString();
                                                    int sem1 = Convert.ToInt32(ddlsemester.SelectedValue.ToString());
                                                    double degcgpa = Math.Round(findgrade(ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["RollNumber"].ToString(), sem1), 2);
                                                    degcgpa = Math.Round(degcgpa, 2);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Text = degcgpa.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Bold = false;
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[3].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Text = failcout.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Bold = false;
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[8].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Day Scholar")
                                                        {

                                                            d_pass_count++;
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Bold = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Day Scholar")
                                                        {
                                                            d_fail_count++;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (chklscriteria.Items[9].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Hostler")
                                                        {
                                                            h_pass_count++;
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Bold = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Hostler")
                                                        {
                                                            h_fail_count++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[10].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                                                        {
                                                            t_pass_count++;
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Bold = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                                                        {
                                                            t_fail_count++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[11].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                                                        {
                                                            //if (!arr_medium.Contains(rollno))
                                                            //{
                                                            e_pass_count++;
                                                            //    arr_medium.Add(arr_medium);
                                                            //}
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                    }
                                                    else

                                                        if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                                                        {
                                                            e_fail_count++;
                                                        }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[20].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    int g = int.Parse(ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString());
                                                    string gender;
                                                    if (g == 1)
                                                    {
                                                        gender = "G";
                                                    }
                                                    else
                                                    {
                                                        gender = "B";
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Text = gender.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Bold = false;
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[13].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        gs_pass_count++;
                                                        tot_gs_count++;
                                                        gs_count = 1;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Text = gs_count.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Bold = false;
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[14].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "0")
                                                    {
                                                        bs_pass_count++;
                                                        tot_bs_count++;
                                                        bs_count = 1;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Text = bs_count.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Bold = false;
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[23].Selected == true)//modified on 01.08.12
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    string textval = "";
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["SeatType"].ToString() != "" && ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["SeatType"].ToString() != " ")
                                                    {
                                                        textval = d2.GetFunction("Select TextVal from textvaltable where textcode=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["seattype"].ToString() + "");
                                                    }
                                                    else
                                                    {
                                                        textval = "-";
                                                    }
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Text = textval.ToString();
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].HorizontalAlign = HorizontalAlign.Center;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Name = "Book Antiqua";
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Size = FontUnit.Medium;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Bold = false;
                                                    //quota_count = quotacount;
                                                }
                                            }
                                        }
                                        failcout = 0;
                                        sno++;
                                        result = "Pass";
                                        totmark = 0;
                                        totstudcount = 0;
                                        maxmark = 0;
                                        abse = 0;
                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                        string degdetails = ds.Tables[1].Rows[stu]["Degreedetails"].ToString();
                                        string[] degsplit = degdetails.Split('-');
                                        string degree = "";
                                        if (degsplit.Length == 3)
                                        {
                                            if (degsplit[0] != "" && degsplit[0] != null)
                                            {
                                                degree = degsplit[0].ToString();
                                            }
                                            if (degsplit[1] != "" && degsplit[1] != null)
                                            {
                                                degree += " - " + degsplit[1].ToString();
                                            }
                                            if (degsplit[2] != "" && degsplit[2] != null)
                                            {
                                                degree += " - " + degsplit[2].ToString();
                                            }
                                        }
                                        else if (degsplit.Length == 2)
                                        {
                                            if (degsplit[0] != "" && degsplit[0] != null)
                                            {
                                                degree = degsplit[0].ToString();
                                            }
                                            if (degsplit[1] != "" && degsplit[1] != null)
                                            {
                                                degree += " - " + degsplit[1].ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (degsplit[0] != "" && degsplit[0] != null)
                                            {
                                                degree = degsplit[0].ToString();
                                            }
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = degree.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[1].Rows[stu]["Roll_No"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[1].Rows[stu]["Reg_no"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[1].Rows[stu]["Stud_Type"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[1].Rows[stu]["Stud_Name"].ToString();
                                        rollno = temproll;

                                    }
                                    string subcode = ds.Tables[1].Rows[stu]["Subject_Code"].ToString().Trim();
                                    if (hat.Contains(subcode))
                                    {
                                        int col = Convert.ToInt32(hat[subcode].ToString());
                                        string marks_per = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                        if (Convert.ToInt32(marks_per) < 0)
                                        {
                                            switch (marks_per)
                                            {
                                                case "-1":
                                                    marks_per = "AAA";
                                                    abse++;
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
                                                    odcnt++;
                                                    break;
                                                case "-17":
                                                    marks_per = "LA";
                                                    break;

                                                case "-18":
                                                    marks_per = "RAA";

                                                    break;

                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = marks_per;
                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                            subsno++;
                                        }
                                        string minmark = ds.Tables[1].Rows[stu]["min_mark"].ToString();
                                        if (minmark.Trim() != "" && minmark != null)
                                        {
                                            Double min = Convert.ToDouble(minmark);
                                            Double mark = Convert.ToDouble(ds.Tables[1].Rows[stu]["marks_obtained"].ToString());
                                            if (mark > 0)
                                            {
                                                totmark = totmark + mark;
                                                totstudcount = totstudcount + 1;
                                            }
                                            maxmark = maxmark + Convert.ToDouble(ds.Tables[1].Rows[stu]["max_mark"].ToString());
                                            if (marks_per != "EL" && marks_per != "EOD")
                                            {
                                                if (mark < min)
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightPink;
                                                    failcout++;
                                                    result = "Fail";
                                                }
                                            }
                                            // Double perecentage = totmark / maxmark * 100;
                                            //Double perecentage = totmark / totstudcount;


                                            //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                            Double perecentage = 0;
                                            if (totmark != 0 && totstudcount != 0)
                                            {
                                                perecentage = totmark / totstudcount;
                                                perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                            }
                                            else
                                            {
                                                perecentage = 0;
                                            }

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;

                                            //DataView dv = new DataView();
                                            //ds.Tables[1].DefaultView.RowFilter = "marks_obtained<0 and Roll_No='" + ds.Tables[1].Rows[stu]["Roll_No"].ToString() + "'";
                                            //dv = ds.Tables[1].DefaultView;
                                            //if (abse == 0)
                                            //{
                                            //    abse = dv.Count;
                                            //}
                                            //else
                                            //{
                                            //    abse = abse + dv.Count;
                                            //}
                                        }

                                    }

                                    if (stu == ds.Tables[1].Rows.Count - 1)
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                        // Double perecentage = totmark / maxmark * 100;
                                        //Double perecentage = totmark / totstudcount;


                                        //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                        Double perecentage = 0;
                                        if (totmark != 0 && totstudcount != 0)
                                        {
                                            perecentage = totmark / totstudcount;
                                            perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                        }
                                        else
                                        {
                                            perecentage = 0;
                                        }

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                        if (ds.Tables[1].Rows[stu - 1]["stud_type"].ToString() == "Hostler")
                                        {
                                            if (result == "Pass")
                                            {
                                                passcnt++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girlpass++;
                                                }
                                                else
                                                {
                                                    boypass++;
                                                }

                                            }
                                            else
                                            {
                                                failcnt++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girl1fail++;
                                                }
                                                else
                                                {
                                                    boy1fail++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (result == "Pass")
                                            {
                                                passcnt1++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girlpass++;
                                                }
                                                else
                                                {
                                                    boypass++;
                                                }
                                            }
                                            else
                                            {
                                                failcnt1++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girl1fail++;
                                                }
                                                else
                                                {
                                                    boy1fail++;
                                                }
                                            }
                                        }
                                        if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                        {
                                            totalpercentage.Add((ds.Tables[1].Rows[stu]["Roll_No"].ToString()), Convert.ToInt32(perecentage));

                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;
                                        if (hatfailcount.Contains(failcout))
                                        {
                                            int fc = Convert.ToInt32(hatfailcount[failcout]);
                                            fc++;
                                            hatfailcount[failcout] = fc;
                                        }
                                        else
                                        {
                                            hatfailcount.Add(failcout, 1);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                FpSpread1.Visible = false;
                                lblrptname.Visible = false;
                                rptprint.Visible = false;
                                txtexcelname.Visible = false;
                                btnExcel.Visible = false;
                                BtnPrint.Visible = false;
                                errmsg.Visible = true;
                                errmsg.Text = "No Records Found";
                            }
                        }
                        if (DropDownList2.SelectedItem.Text == "3Sub")
                        {
                            if (count2.Count > 0)
                            {
                                if (count2.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                {
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            //Double perecentage = totmark / maxmark * 100;
                                            //Double perecentage = totmark / totstudcount;


                                            //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                            Double perecentage = 0;
                                            if (totmark != 0 && totstudcount != 0)
                                            {
                                                perecentage = totmark / totstudcount;
                                                perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                            }
                                            else
                                            {
                                                perecentage = 0;
                                            }

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                            if (ds.Tables[1].Rows[stu - 1]["stud_type"].ToString() == "Hostler")
                                            {
                                                if (result == "Pass")
                                                {
                                                    passcnt++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girlpass++;
                                                    }
                                                    else
                                                    {
                                                        boypass++;
                                                    }

                                                }
                                                else
                                                {
                                                    failcnt++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girl1fail++;
                                                    }
                                                    else
                                                    {
                                                        boy1fail++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (result == "Pass")
                                                {
                                                    passcnt1++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girlpass++;
                                                    }
                                                    else
                                                    {
                                                        boypass++;
                                                    }
                                                }
                                                else
                                                {
                                                    failcnt1++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girl1fail++;
                                                    }
                                                    else
                                                    {
                                                        boy1fail++;
                                                    }
                                                }
                                            }
                                            if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                            {
                                                totalpercentage.Add((rollno), Convert.ToInt32(perecentage));

                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;
                                            if (hatfailcount.Contains(failcout))
                                            {
                                                int fc = Convert.ToInt32(hatfailcount[failcout]);
                                                fc++;
                                                hatfailcount[failcout] = fc;
                                            }
                                            else
                                            {
                                                hatfailcount.Add(failcout, 1);
                                            }

                                        }
                                        string medium1 = "";
                                        //if (chklscriteria.Items[1].Selected == true)
                                        //{
                                        if (temproll != rollno)
                                        {
                                            if (rollno.Trim() != "")
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Left;
                                                medium1 = d2.GetFunctionv("select distinct TextVal from TextValTable  where TextCode in(select medium_ins from applyn where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + " and medium_ins is not NULL) and TextCriteria='medi'");
                                                if ((medium1 == "") || (medium1 == null))
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Text = "-";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Bold = false;

                                                }
                                                else
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Text = medium1.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Bold = false;


                                                }
                                            }
                                        }

                                        //}
                                        if (chklscriteria.Items[2].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    string s = "select  distinct c.course_id from degree as d, course as c where d.course_id=c.course_id and d.dept_code in(" + degreecode + ") "; //
                                                    DataSet ds4 = new DataSet();
                                                    ds4 = d2.select_method_wo_parameter(s, "text");
                                                    for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
                                                    {
                                                        string schoolgrd = d2.GetFunction("select edu_level from course where course_id= " + ds4.Tables[0].Rows[i]["course_id"].ToString() + "");
                                                        if (schoolgrd != string.Empty)
                                                        {
                                                            if (schoolgrd == "UG" || schoolgrd == "U.G")
                                                            {
                                                                string scholmrk = d2.GetFunction("select percentage from stud_prev_details as s,textvaltable as t where app_no= " + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%'))");

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Right;

                                                                if (scholmrk != string.Empty)
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = scholmrk.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }
                                                                else
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }

                                                                string scholmrk1 = d2.GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no= " + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%')))");

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                if (scholmrk1 != string.Empty)
                                                                {
                                                                    string sam = scholmrk1.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = scholmrk1.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                }
                                                                else
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                }

                                                            }

                                                        }
                                                        else if (schoolgrd == "PG" || schoolgrd == "P.G")
                                                        {
                                                            if (temproll != rollno)
                                                            {

                                                                string scholmrk2 = d2.GetFunction("select distinct percentage from stud_prev_details as s,textvaltable as t where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC'))and (textval not like '%H.SC(SB)') and (textval not like '%intermediate')");
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                if (scholmrk2 != string.Empty)
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = scholmrk2.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }
                                                                else
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }
                                                                string scholmrk3 = d2.GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and (textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC') and (textval not like '%H.SC(SB)') and (textval not like '%intermediate'))");
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                if (scholmrk3 != string.Empty)
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = scholmrk3.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;

                                                                }
                                                                else
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[3].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    degree_codeparticularstudent = ds.Tables[1].Rows[FpSpread1.Sheets[0].RowCount - 1]["degree_code"].ToString();
                                                    int sem1 = Convert.ToInt32(ddlsemester.SelectedValue.ToString());
                                                    double degcgpa = Math.Round(findgrade(ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["RollNumber"].ToString(), sem1), 2);
                                                    degcgpa = Math.Round(degcgpa, 2);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Text = degcgpa.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Bold = false;
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[3].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Text = failcout.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Bold = false;
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[8].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Day Scholar")
                                                        {

                                                            d_pass_count++;
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Bold = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Day Scholar")
                                                        {
                                                            d_fail_count++;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (chklscriteria.Items[9].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Hostler")
                                                        {
                                                            h_pass_count++;
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Bold = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Hostler")
                                                        {
                                                            h_fail_count++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[10].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                                                        {
                                                            t_pass_count++;
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Bold = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                                                        {
                                                            t_fail_count++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[11].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                                                        {
                                                            //if (!arr_medium.Contains(rollno))
                                                            //{
                                                            e_pass_count++;
                                                            //    arr_medium.Add(arr_medium);
                                                            //}
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                    }
                                                    else

                                                        if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                                                        {
                                                            e_fail_count++;
                                                        }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[20].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    int g = int.Parse(ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString());
                                                    string gender;
                                                    if (g == 1)
                                                    {
                                                        gender = "G";
                                                    }
                                                    else
                                                    {
                                                        gender = "B";
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Text = gender.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Bold = false;
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[13].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        gs_pass_count++;
                                                        tot_gs_count++;
                                                        gs_count = 1;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Text = gs_count.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Bold = false;
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[14].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "0")
                                                    {
                                                        bs_pass_count++;
                                                        tot_bs_count++;
                                                        bs_count = 1;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Text = bs_count.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Bold = false;
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[23].Selected == true)//modified on 01.08.12
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    string textval = "";
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["SeatType"].ToString() != "" && ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["SeatType"].ToString() != " ")
                                                    {
                                                        textval = d2.GetFunction("Select TextVal from textvaltable where textcode=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["seattype"].ToString() + "");
                                                    }
                                                    else
                                                    {
                                                        textval = "-";
                                                    }
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Text = textval.ToString();
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].HorizontalAlign = HorizontalAlign.Center;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Name = "Book Antiqua";
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Size = FontUnit.Medium;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Bold = false;
                                                    //quota_count = quotacount;
                                                }
                                            }
                                        }
                                        failcout = 0;
                                        sno++;
                                        result = "Pass";
                                        totmark = 0;
                                        totstudcount = 0;
                                        maxmark = 0;
                                        abse = 0;
                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = false;
                                        string degdetails = ds.Tables[1].Rows[stu]["Degreedetails"].ToString();
                                        string[] degsplit = degdetails.Split('-');
                                        string degree = "";
                                        if (degsplit.Length == 3)
                                        {
                                            if (degsplit[0] != "" && degsplit[0] != null)
                                            {
                                                degree = degsplit[0].ToString();
                                            }
                                            if (degsplit[1] != "" && degsplit[1] != null)
                                            {
                                                degree += " - " + degsplit[1].ToString();
                                            }
                                            if (degsplit[2] != "" && degsplit[2] != null)
                                            {
                                                degree += " - " + degsplit[2].ToString();
                                            }
                                        }
                                        else if (degsplit.Length == 2)
                                        {
                                            if (degsplit[0] != "" && degsplit[0] != null)
                                            {
                                                degree = degsplit[0].ToString();
                                            }
                                            if (degsplit[1] != "" && degsplit[1] != null)
                                            {
                                                degree += " - " + degsplit[1].ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (degsplit[0] != "" && degsplit[0] != null)
                                            {
                                                degree = degsplit[0].ToString();
                                            }
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = degree.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[1].Rows[stu]["Roll_No"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[1].Rows[stu]["Reg_no"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[1].Rows[stu]["Stud_Type"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[1].Rows[stu]["Stud_Name"].ToString();
                                        rollno = temproll;

                                    }
                                    string subcode = ds.Tables[1].Rows[stu]["Subject_Code"].ToString().Trim();
                                    if (hat.Contains(subcode))
                                    {
                                        int col = Convert.ToInt32(hat[subcode].ToString());
                                        string marks_per = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                        if (Convert.ToInt32(marks_per) < 0)
                                        {
                                            switch (marks_per)
                                            {
                                                case "-1":
                                                    marks_per = "AAA";
                                                    abse++;
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
                                                    odcnt++;
                                                    break;
                                                case "-17":
                                                    marks_per = "LA";

                                                    break;

                                                case "-18":
                                                    marks_per = "RAA";

                                                    break;

                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = marks_per;
                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                            subsno++;
                                        }
                                        string minmark = ds.Tables[1].Rows[stu]["min_mark"].ToString();
                                        if (minmark.Trim() != "" && minmark != null)
                                        {
                                            Double min = Convert.ToDouble(minmark);
                                            Double mark = Convert.ToDouble(ds.Tables[1].Rows[stu]["marks_obtained"].ToString());
                                            if (mark > 0)
                                            {
                                                totmark = totmark + mark;
                                                totstudcount = totstudcount + 1;
                                            }
                                            maxmark = maxmark + Convert.ToDouble(ds.Tables[1].Rows[stu]["max_mark"].ToString());
                                            if (marks_per != "EL" && marks_per != "EOD")
                                            {
                                                if (mark < min)
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightPink;
                                                    failcout++;
                                                    result = "Fail";
                                                }
                                            }
                                            //Double perecentage = totmark / maxmark * 100;
                                            //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                            Double perecentage = 0;
                                            if (totmark != 0 && totstudcount != 0)
                                            {
                                                perecentage = totmark / maxmark * 100;
                                                perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                            }
                                            else
                                            {
                                                perecentage = 0;
                                            }

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;

                                            //DataView dv = new DataView();
                                            //ds.Tables[1].DefaultView.RowFilter = "marks_obtained<0 and Roll_No='" + ds.Tables[1].Rows[stu]["Roll_No"].ToString() + "'";
                                            //dv = ds.Tables[1].DefaultView;
                                            //if (abse == 0)
                                            //{
                                            //    abse = dv.Count;
                                            //}
                                            //else
                                            //{
                                            //    abse = abse + dv.Count;
                                            //}
                                        }

                                    }

                                    if (stu == ds.Tables[1].Rows.Count - 1)
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                        // Double perecentage = totmark / maxmark * 100;
                                        //Double perecentage = totmark / totstudcount;


                                        //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                        Double perecentage = 0;
                                        if (totmark != 0 && totstudcount != 0)
                                        {
                                            perecentage = totmark / totstudcount;
                                            perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                        }
                                        else
                                        {
                                            perecentage = 0;
                                        }


                                        //  totalpercentage.Add(Convert.ToInt32(ds.Tables[1].Rows[stu]["Roll_No"].ToString()),Convert.ToInt32(perecentage));
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                        if (ds.Tables[1].Rows[stu - 1]["stud_type"].ToString() == "Hostler")
                                        {
                                            if (result == "Pass")
                                            {
                                                passcnt++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girlpass++;
                                                }
                                                else
                                                {
                                                    boypass++;
                                                }

                                            }
                                            else
                                            {
                                                failcnt++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girl1fail++;
                                                }
                                                else
                                                {
                                                    boy1fail++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (result == "Pass")
                                            {
                                                passcnt1++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girlpass++;
                                                }
                                                else
                                                {
                                                    boypass++;
                                                }
                                            }
                                            else
                                            {
                                                failcnt1++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girl1fail++;
                                                }
                                                else
                                                {
                                                    boy1fail++;
                                                }
                                            }
                                        }
                                        if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                        {
                                            totalpercentage.Add((ds.Tables[1].Rows[stu]["Roll_No"].ToString()), Convert.ToInt32(perecentage));

                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;
                                        if (hatfailcount.Contains(failcout))
                                        {
                                            int fc = Convert.ToInt32(hatfailcount[failcout]);
                                            fc++;
                                            hatfailcount[failcout] = fc;
                                        }
                                        else
                                        {
                                            hatfailcount.Add(failcout, 1);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                FpSpread1.Visible = false;
                                lblrptname.Visible = false;
                                rptprint.Visible = false;
                                txtexcelname.Visible = false;
                                btnExcel.Visible = false;
                                BtnPrint.Visible = false;
                                errmsg.Visible = true;
                                errmsg.Text = "No Records Found";
                            }
                        }
                        if (DropDownList2.SelectedItem.Text == "Above 3Sub")
                        {
                            if (count4.Count > 0)
                            {
                                if (count4.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                {
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            //Double perecentage = totmark / maxmark * 100;
                                            //Double perecentage = totmark / totstudcount;

                                            //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                            Double perecentage = 0;
                                            if (totmark != 0 && totstudcount != 0)
                                            {
                                                perecentage = totmark / totstudcount;
                                                perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                            }
                                            else
                                            {
                                                perecentage = 0;
                                            }


                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                            if (ds.Tables[1].Rows[stu - 1]["stud_type"].ToString() == "Hostler")
                                            {
                                                if (result == "Pass")
                                                {
                                                    passcnt++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girlpass++;
                                                    }
                                                    else
                                                    {
                                                        boypass++;
                                                    }

                                                }
                                                else
                                                {
                                                    failcnt++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girl1fail++;
                                                    }
                                                    else
                                                    {
                                                        boy1fail++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (result == "Pass")
                                                {
                                                    passcnt1++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girlpass++;
                                                    }
                                                    else
                                                    {
                                                        boypass++;
                                                    }
                                                }
                                                else
                                                {
                                                    failcnt1++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girl1fail++;
                                                    }
                                                    else
                                                    {
                                                        boy1fail++;
                                                    }
                                                }
                                            }
                                            if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                            {
                                                totalpercentage.Add((rollno), Convert.ToInt32(perecentage));

                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;
                                            if (hatfailcount.Contains(failcout))
                                            {
                                                int fc = Convert.ToInt32(hatfailcount[failcout]);
                                                fc++;
                                                hatfailcount[failcout] = fc;
                                            }
                                            else
                                            {
                                                hatfailcount.Add(failcout, 1);
                                            }

                                        }
                                        string medium1 = "";
                                        //if (chklscriteria.Items[1].Selected == true)
                                        //{
                                        if (temproll != rollno)
                                        {
                                            if (rollno.Trim() != "")
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Left;
                                                medium1 = d2.GetFunctionv("select distinct TextVal from TextValTable  where TextCode in(select medium_ins from applyn where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + " and medium_ins is not NULL) and TextCriteria='medi'");
                                                if ((medium1 == "") || (medium1 == null))
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Text = "-";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Bold = false;

                                                }
                                                else
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Text = medium1.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Bold = false;

                                                }
                                            }
                                        }

                                        //}
                                        if (chklscriteria.Items[2].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    string s = "select  distinct c.course_id from degree as d, course as c where d.course_id=c.course_id and d.dept_code in(" + degreecode + ") "; //
                                                    DataSet ds4 = new DataSet();
                                                    ds4 = d2.select_method_wo_parameter(s, "text");
                                                    for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
                                                    {
                                                        string schoolgrd = d2.GetFunction("select edu_level from course where course_id= " + ds4.Tables[0].Rows[i]["course_id"].ToString() + "");
                                                        if (schoolgrd != string.Empty)
                                                        {
                                                            if (schoolgrd == "UG" || schoolgrd == "U.G")
                                                            {
                                                                string scholmrk = d2.GetFunction("select percentage from stud_prev_details as s,textvaltable as t where app_no= " + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%'))");

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Right;

                                                                if (scholmrk != string.Empty)
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = scholmrk.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }
                                                                else
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }

                                                                string scholmrk1 = d2.GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no= " + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%')))");

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                if (scholmrk1 != string.Empty)
                                                                {
                                                                    string sam = scholmrk1.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = scholmrk1.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                }
                                                                else
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                }

                                                            }

                                                        }
                                                        else if (schoolgrd == "PG" || schoolgrd == "P.G")
                                                        {
                                                            if (temproll != rollno)
                                                            {

                                                                string scholmrk2 = d2.GetFunction("select distinct percentage from stud_prev_details as s,textvaltable as t where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC'))and (textval not like '%H.SC(SB)') and (textval not like '%intermediate')");
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                if (scholmrk2 != string.Empty)
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = scholmrk2.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }
                                                                else
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }
                                                                string scholmrk3 = d2.GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and (textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC') and (textval not like '%H.SC(SB)') and (textval not like '%intermediate'))");
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                if (scholmrk3 != string.Empty)
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = scholmrk3.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;

                                                                }
                                                                else
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[3].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    degree_codeparticularstudent = ds.Tables[1].Rows[FpSpread1.Sheets[0].RowCount - 1]["degree_code"].ToString();
                                                    int sem1 = Convert.ToInt32(ddlsemester.SelectedValue.ToString());
                                                    double degcgpa = Math.Round(findgrade(ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["RollNumber"].ToString(), sem1), 2);
                                                    degcgpa = Math.Round(degcgpa, 2);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Text = degcgpa.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Bold = false;
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[3].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Text = failcout.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Bold = false;
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[8].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Day Scholar")
                                                        {

                                                            d_pass_count++;
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Bold = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Day Scholar")
                                                        {
                                                            d_fail_count++;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (chklscriteria.Items[9].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Hostler")
                                                        {
                                                            h_pass_count++;
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Bold = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Hostler")
                                                        {
                                                            h_fail_count++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[10].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                                                        {
                                                            t_pass_count++;
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Bold = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                                                        {
                                                            t_fail_count++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[11].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                                                        {
                                                            //if (!arr_medium.Contains(rollno))
                                                            //{
                                                            e_pass_count++;
                                                            //arr_medium.Add(arr_medium);
                                                            //}
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                    }
                                                    else

                                                        if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                                                        {
                                                            e_fail_count++;
                                                        }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[20].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    int g = int.Parse(ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString());
                                                    string gender;
                                                    if (g == 1)
                                                    {
                                                        gender = "G";
                                                    }
                                                    else
                                                    {
                                                        gender = "B";
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Text = gender.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Bold = false;
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[13].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        gs_pass_count++;
                                                        tot_gs_count++;
                                                        gs_count = 1;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Text = gs_count.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Bold = false;
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[14].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "0")
                                                    {
                                                        bs_pass_count++;
                                                        tot_bs_count++;
                                                        bs_count = 1;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Text = bs_count.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Bold = false;
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[23].Selected == true)//modified on 01.08.12
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    string textval = "";
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["SeatType"].ToString() != "" && ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["SeatType"].ToString() != " ")
                                                    {
                                                        textval = d2.GetFunction("Select TextVal from textvaltable where textcode=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["seattype"].ToString() + "");
                                                    }
                                                    else
                                                    {
                                                        textval = "-";
                                                    }
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Text = textval.ToString();
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].HorizontalAlign = HorizontalAlign.Center;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Name = "Book Antiqua";
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Size = FontUnit.Medium;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Bold = false;
                                                    //quota_count = quotacount;
                                                }
                                            }
                                        }
                                        failcout = 0;
                                        sno++;
                                        result = "Pass";
                                        totmark = 0;
                                        totstudcount = 0;

                                        maxmark = 0;
                                        abse = 0;
                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = false;
                                        string degdetails = ds.Tables[1].Rows[stu]["Degreedetails"].ToString();
                                        string[] degsplit = degdetails.Split('-');
                                        string degree = "";
                                        if (degsplit.Length == 3)
                                        {
                                            if (degsplit[0] != "" && degsplit[0] != null)
                                            {
                                                degree = degsplit[0].ToString();
                                            }
                                            if (degsplit[1] != "" && degsplit[1] != null)
                                            {
                                                degree += " - " + degsplit[1].ToString();
                                            }
                                            if (degsplit[2] != "" && degsplit[2] != null)
                                            {
                                                degree += " - " + degsplit[2].ToString();
                                            }
                                        }
                                        else if (degsplit.Length == 2)
                                        {
                                            if (degsplit[0] != "" && degsplit[0] != null)
                                            {
                                                degree = degsplit[0].ToString();
                                            }
                                            if (degsplit[1] != "" && degsplit[1] != null)
                                            {
                                                degree += " - " + degsplit[1].ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (degsplit[0] != "" && degsplit[0] != null)
                                            {
                                                degree = degsplit[0].ToString();
                                            }
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = degree.ToString();

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[1].Rows[stu]["Roll_No"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[1].Rows[stu]["Reg_no"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[1].Rows[stu]["Stud_Type"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[1].Rows[stu]["Stud_Name"].ToString();
                                        rollno = temproll;

                                    }
                                    string subcode = ds.Tables[1].Rows[stu]["Subject_Code"].ToString().Trim();
                                    if (hat.Contains(subcode))
                                    {
                                        int col = Convert.ToInt32(hat[subcode].ToString());
                                        string marks_per = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                        if (Convert.ToInt32(marks_per) < 0)
                                        {
                                            switch (marks_per)
                                            {
                                                case "-1":
                                                    marks_per = "AAA";
                                                    abse++;
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
                                                    odcnt++;
                                                    break;
                                                case "-17":
                                                    marks_per = "LA";

                                                    break;

                                                case "-18":
                                                    marks_per = "RAA";

                                                    break;

                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = marks_per;
                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                            subsno++;
                                        }
                                        string minmark = ds.Tables[1].Rows[stu]["min_mark"].ToString();
                                        if (minmark.Trim() != "" && minmark != null)
                                        {
                                            Double min = Convert.ToDouble(minmark);
                                            Double mark = Convert.ToDouble(ds.Tables[1].Rows[stu]["marks_obtained"].ToString());
                                            if (mark > 0)
                                            {
                                                totmark = totmark + mark;
                                                totstudcount = totstudcount + 1;


                                            }
                                            maxmark = maxmark + Convert.ToDouble(ds.Tables[1].Rows[stu]["max_mark"].ToString());
                                            if (marks_per != "EL" && marks_per != "EOD")
                                            {
                                                if (mark < min)
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightPink;
                                                    failcout++;
                                                    result = "Fail";
                                                }
                                            }
                                            //  Double perecentage = totmark / maxmark * 100;
                                            //Double perecentage = totmark / totstudcount;

                                            //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                            Double perecentage = 0;
                                            if (totmark != 0 && totstudcount != 0)
                                            {
                                                perecentage = totmark / totstudcount;
                                                perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                            }
                                            else
                                            {
                                                perecentage = 0;
                                            }

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;

                                            //DataView dv = new DataView();
                                            //ds.Tables[1].DefaultView.RowFilter = "marks_obtained<0 and Roll_No='" + ds.Tables[1].Rows[stu]["Roll_No"].ToString() + "'";
                                            //dv = ds.Tables[1].DefaultView;
                                            //if (abse == 0)
                                            //{
                                            //    abse = dv.Count;
                                            //}
                                            //else
                                            //{
                                            //    abse = abse + dv.Count;
                                            //}
                                        }

                                    }

                                    if (stu == ds.Tables[1].Rows.Count - 1)
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                        // Double perecentage = totmark / maxmark * 100;
                                        //Double perecentage = totmark / totstudcount;

                                        //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                        Double perecentage = 0;
                                        if (totmark != 0 && totstudcount != 0)
                                        {
                                            perecentage = totmark / totstudcount;
                                            perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                        }
                                        else
                                        {
                                            perecentage = 0;
                                        }

                                        //  totalpercentage.Add(Convert.ToInt32(ds.Tables[1].Rows[stu]["Roll_No"].ToString()),Convert.ToInt32(perecentage));
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                        if (ds.Tables[1].Rows[stu - 1]["stud_type"].ToString() == "Hostler")
                                        {
                                            if (result == "Pass")
                                            {
                                                passcnt++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girlpass++;
                                                }
                                                else
                                                {
                                                    boypass++;
                                                }

                                            }
                                            else
                                            {
                                                failcnt++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girl1fail++;
                                                }
                                                else
                                                {
                                                    boy1fail++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (result == "Pass")
                                            {
                                                passcnt1++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girlpass++;
                                                }
                                                else
                                                {
                                                    boypass++;
                                                }
                                            }
                                            else
                                            {
                                                failcnt1++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girl1fail++;
                                                }
                                                else
                                                {
                                                    boy1fail++;
                                                }
                                            }
                                        }
                                        if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                        {
                                            totalpercentage.Add((ds.Tables[1].Rows[stu]["Roll_No"].ToString()), Convert.ToInt32(perecentage));

                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;
                                        if (hatfailcount.Contains(failcout))
                                        {
                                            int fc = Convert.ToInt32(hatfailcount[failcout]);
                                            fc++;
                                            hatfailcount[failcout] = fc;
                                        }
                                        else
                                        {
                                            hatfailcount.Add(failcout, 1);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                FpSpread1.Visible = false;
                                lblrptname.Visible = false;
                                rptprint.Visible = false;
                                txtexcelname.Visible = false;
                                btnExcel.Visible = false;
                                BtnPrint.Visible = false;
                                errmsg.Visible = true;
                                errmsg.Text = "No Records Found";
                            }
                        }
                        if (rbt.Items[1].Selected != true)
                        {
                            if (rbt.Items[2].Selected != true)
                            {
                                if (DropDownList2.SelectedItem.Text == "--Select--")
                                {
                                    if (!count.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                    {
                                        if (!count1.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                        {
                                            if (!count2.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                            {
                                                if (!count4.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                                {
                                                    if (temproll != rollno)
                                                    {
                                                        if (rollno.Trim() != "")
                                                        {
                                                            // Double perecentage = totmark / maxmark * 100;
                                                            //Double perecentage = totmark / totstudcount;

                                                            //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                                            Double perecentage = 0;
                                                            if (totmark != 0 && totstudcount != 0)
                                                            {
                                                                perecentage = totmark / totstudcount;
                                                                perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                            else
                                                            {
                                                                perecentage = 0;
                                                            }

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                                            if (ds.Tables[1].Rows[stu - 1]["stud_type"].ToString() == "Hostler")
                                                            {
                                                                if (result == "Pass")
                                                                {
                                                                    passcnt++;
                                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                                    {
                                                                        girlpass++;
                                                                    }
                                                                    else
                                                                    {
                                                                        boypass++;
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    failcnt++;
                                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                                    {
                                                                        girl1fail++;
                                                                    }
                                                                    else
                                                                    {
                                                                        boy1fail++;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (result == "Pass")
                                                                {
                                                                    passcnt1++;
                                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                                    {
                                                                        girlpass++;
                                                                    }
                                                                    else
                                                                    {
                                                                        boypass++;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    failcnt1++;
                                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                                    {
                                                                        girl1fail++;
                                                                    }
                                                                    else
                                                                    {
                                                                        boy1fail++;
                                                                    }
                                                                }
                                                            }
                                                            if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                                            {
                                                                totalpercentage.Add((rollno), Convert.ToInt32(perecentage));

                                                            }
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;
                                                            if (hatfailcount.Contains(failcout))
                                                            {
                                                                int fc = Convert.ToInt32(hatfailcount[failcout]);
                                                                fc++;
                                                                hatfailcount[failcout] = fc;
                                                            }
                                                            else
                                                            {
                                                                hatfailcount.Add(failcout, 1);
                                                            }

                                                        }
                                                        string medium1 = "";
                                                        //if (chklscriteria.Items[1].Selected == true)
                                                        //{
                                                        if (temproll != rollno)
                                                        {
                                                            if (rollno.Trim() != "")
                                                            {
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Left;
                                                                medium1 = d2.GetFunctionv("select distinct TextVal from TextValTable  where TextCode in(select medium_ins from applyn where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + " and medium_ins is not NULL) and TextCriteria='medi'");
                                                                if ((medium1 == "") || (medium1 == null))
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Bold = false;

                                                                }
                                                                else
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Text = medium1.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Bold = false;


                                                                }
                                                            }
                                                        }

                                                        //}
                                                        if (chklscriteria.Items[2].Selected == true)
                                                        {
                                                            if (temproll != rollno)
                                                            {
                                                                if (rollno.Trim() != "")
                                                                {
                                                                    string s = "select  distinct c.course_id from degree as d, course as c where d.course_id=c.course_id and d.dept_code in(" + degreecode + ") "; //
                                                                    DataSet ds4 = new DataSet();
                                                                    ds4 = d2.select_method_wo_parameter(s, "text");
                                                                    for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
                                                                    {
                                                                        string schoolgrd = d2.GetFunction("select edu_level from course where course_id= " + ds4.Tables[0].Rows[i]["course_id"].ToString() + "");
                                                                        if (schoolgrd != string.Empty)
                                                                        {
                                                                            if (schoolgrd == "UG" || schoolgrd == "U.G")
                                                                            {
                                                                                string scholmrk = d2.GetFunction("select percentage from stud_prev_details as s,textvaltable as t where app_no= " + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%'))");

                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Right;

                                                                                if (scholmrk != string.Empty)
                                                                                {
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = scholmrk.ToString();
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                                }
                                                                                else
                                                                                {
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = "-";
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                                }

                                                                                string scholmrk1 = d2.GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no= " + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%')))");

                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                                if (scholmrk1 != string.Empty)
                                                                                {
                                                                                    string sam = scholmrk1.ToString();
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = scholmrk1.ToString();
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                                }
                                                                                else
                                                                                {

                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = "-";
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                                }

                                                                            }

                                                                        }
                                                                        else if (schoolgrd == "PG" || schoolgrd == "P.G")
                                                                        {
                                                                            if (temproll != rollno)
                                                                            {

                                                                                string scholmrk2 = d2.GetFunction("select distinct percentage from stud_prev_details as s,textvaltable as t where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC'))and (textval not like '%H.SC(SB)') and (textval not like '%intermediate')");
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                                if (scholmrk2 != string.Empty)
                                                                                {

                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = scholmrk2.ToString();
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                                }
                                                                                else
                                                                                {
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = "-";
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                                }
                                                                                string scholmrk3 = d2.GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and (textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC') and (textval not like '%H.SC(SB)') and (textval not like '%intermediate'))");
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                                if (scholmrk3 != string.Empty)
                                                                                {

                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = scholmrk3.ToString();
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;

                                                                                }
                                                                                else
                                                                                {

                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = "-";
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (chklscriteria.Items[3].Selected == true)
                                                        {
                                                            if (temproll != rollno)
                                                            {
                                                                if (rollno.Trim() != "")
                                                                {
                                                                    degree_codeparticularstudent = ds.Tables[1].Rows[FpSpread1.Sheets[0].RowCount - 1]["degree_code"].ToString();
                                                                    int sem1 = Convert.ToInt32(ddlsemester.SelectedValue.ToString());
                                                                    double degcgpa = Math.Round(findgrade(ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["RollNumber"].ToString(), sem1), 2);
                                                                    degcgpa = Math.Round(degcgpa, 2);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Text = degcgpa.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Bold = false;
                                                                }
                                                            }
                                                        }
                                                        if (chklscriteria.Items[3].Selected == true)
                                                        {
                                                            if (temproll != rollno)
                                                            {
                                                                if (rollno.Trim() != "")
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Text = failcout.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Bold = false;
                                                                }
                                                            }
                                                        }
                                                        if (chklscriteria.Items[8].Selected == true)
                                                        {
                                                            if (temproll != rollno)
                                                            {
                                                                if (rollno.Trim() != "")
                                                                {
                                                                    if (failcout == 0)
                                                                    {
                                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Day Scholar")
                                                                        {
                                                                            d_pass_count++;
                                                                            bs_count = 1;
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Text = bs_count.ToString();
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Name = "Book Antiqua";
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Size = FontUnit.Medium;
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Bold = false;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Day Scholar")
                                                                        {
                                                                            d_fail_count++;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (chklscriteria.Items[9].Selected == true)
                                                        {
                                                            if (temproll != rollno)
                                                            {
                                                                if (rollno.Trim() != "")
                                                                {
                                                                    if (failcout == 0)
                                                                    {
                                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Hostler")
                                                                        {
                                                                            h_pass_count++;
                                                                            bs_count = 1;
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Text = bs_count.ToString();
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Name = "Book Antiqua";
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Size = FontUnit.Medium;
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Bold = false;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Hostler")
                                                                        {
                                                                            h_fail_count++;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (chklscriteria.Items[10].Selected == true)
                                                        {
                                                            if (temproll != rollno)
                                                            {
                                                                if (rollno.Trim() != "")
                                                                {
                                                                    if (failcout == 0)
                                                                    {
                                                                        if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                                                                        {
                                                                            t_pass_count++;
                                                                            bs_count = 1;
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Text = bs_count.ToString();
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Name = "Book Antiqua";
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Size = FontUnit.Medium;
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Bold = false;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                                                                        {
                                                                            t_fail_count++;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (chklscriteria.Items[11].Selected == true)
                                                        {
                                                            if (temproll != rollno)
                                                            {
                                                                if (rollno.Trim() != "")
                                                                {
                                                                    if (failcout == 0)
                                                                    {
                                                                        if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                                                                        {
                                                                            //if (!arr_medium.Contains(rollno))
                                                                            //{
                                                                            e_pass_count++;
                                                                            //    arr_medium.Add(arr_medium);
                                                                            //}
                                                                            bs_count = 1;
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Text = bs_count.ToString();
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Name = "Book Antiqua";
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Size = FontUnit.Medium;
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Bold = false;
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].HorizontalAlign = HorizontalAlign.Center;
                                                                        }
                                                                    }
                                                                    else

                                                                        if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                                                                        {
                                                                            e_fail_count++;
                                                                        }
                                                                }
                                                            }
                                                        }
                                                        if (chklscriteria.Items[20].Selected == true)
                                                        {
                                                            if (temproll != rollno)
                                                            {
                                                                if (rollno.Trim() != "")
                                                                {
                                                                    int g = int.Parse(ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString());
                                                                    string gender;
                                                                    if (g == 1)
                                                                    {
                                                                        gender = "G";
                                                                    }
                                                                    else
                                                                    {
                                                                        gender = "B";
                                                                    }
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Text = gender.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Bold = false;
                                                                }
                                                            }
                                                        }
                                                        if (chklscriteria.Items[13].Selected == true)
                                                        {
                                                            if (temproll != rollno)
                                                            {
                                                                if (rollno.Trim() != "")
                                                                {
                                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                                    {
                                                                        gs_pass_count++;
                                                                        tot_gs_count++;
                                                                        gs_count = 1;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Text = gs_count.ToString();
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Name = "Book Antiqua";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Bold = false;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (chklscriteria.Items[14].Selected == true)
                                                        {
                                                            if (temproll != rollno)
                                                            {
                                                                if (rollno.Trim() != "")
                                                                {
                                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "0")
                                                                    {
                                                                        bs_pass_count++;
                                                                        tot_bs_count++;
                                                                        bs_count = 1;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Text = bs_count.ToString();
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Name = "Book Antiqua";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Bold = false;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (chklscriteria.Items[23].Selected == true)//modified on 01.08.12
                                                        {
                                                            if (temproll != rollno)
                                                            {
                                                                if (rollno.Trim() != "")
                                                                {
                                                                    string textval = "";
                                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["SeatType"].ToString() != "" && ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["SeatType"].ToString() != " ")
                                                                    {
                                                                        textval = d2.GetFunction("Select TextVal from textvaltable where textcode=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["seattype"].ToString() + "");
                                                                    }
                                                                    else
                                                                    {
                                                                        textval = "-";
                                                                    }
                                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Text = textval.ToString();
                                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].HorizontalAlign = HorizontalAlign.Center;
                                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Name = "Book Antiqua";
                                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Size = FontUnit.Medium;
                                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Bold = false;
                                                                    //quota_count = quotacount;
                                                                }
                                                            }
                                                        }
                                                        failcout = 0;
                                                        sno++;
                                                        abse = 0;
                                                        result = "Pass";
                                                        totmark = 0;
                                                        totstudcount = 0;

                                                        maxmark = 0;
                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                        string degdetails = ds.Tables[1].Rows[stu]["Degreedetails"].ToString();
                                                        string[] degsplit = degdetails.Split('-');
                                                        string degree = "";
                                                        if (degsplit.Length == 3)
                                                        {
                                                            if (degsplit[0] != "" && degsplit[0] != null)
                                                            {
                                                                degree = degsplit[0].ToString();
                                                            }
                                                            if (degsplit[1] != "" && degsplit[1] != null)
                                                            {
                                                                degree += " - " + degsplit[1].ToString();
                                                            }
                                                            if (degsplit[2] != "" && degsplit[2] != null)
                                                            {
                                                                degree += " - " + degsplit[2].ToString();
                                                            }
                                                        }
                                                        else if (degsplit.Length == 2)
                                                        {
                                                            if (degsplit[0] != "" && degsplit[0] != null)
                                                            {
                                                                degree = degsplit[0].ToString();
                                                            }
                                                            if (degsplit[1] != "" && degsplit[1] != null)
                                                            {
                                                                degree += " - " + degsplit[1].ToString();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (degsplit[0] != "" && degsplit[0] != null)
                                                            {
                                                                degree = degsplit[0].ToString();
                                                            }
                                                        }
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = degree.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = false;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = false;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = false;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = false;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = false;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = false;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[1].Rows[stu]["Roll_No"].ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[1].Rows[stu]["Reg_no"].ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[1].Rows[stu]["Stud_Type"].ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[1].Rows[stu]["Stud_Name"].ToString();
                                                        rollno = temproll;

                                                    }
                                                    string subcode = ds.Tables[1].Rows[stu]["Subject_Code"].ToString().Trim();
                                                    if (hat.Contains(subcode))
                                                    {
                                                        int col = Convert.ToInt32(hat[subcode].ToString());
                                                        string marks_per = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                                        if (Convert.ToInt32(marks_per) < 0)
                                                        {
                                                            switch (marks_per)
                                                            {
                                                                case "-1":
                                                                    marks_per = "AAA";
                                                                    abse++;
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
                                                                    odcnt++;
                                                                    break;
                                                                case "-17":
                                                                    marks_per = "LA";

                                                                    break;

                                                                case "-18":
                                                                    marks_per = "RAA";

                                                                    break;

                                                            }
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = marks_per;
                                                        }
                                                        else
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                                            subsno++;
                                                        }
                                                        string minmark = ds.Tables[1].Rows[stu]["min_mark"].ToString();
                                                        if (minmark.Trim() != "" && minmark != null)
                                                        {
                                                            Double min = Convert.ToDouble(minmark);
                                                            Double mark = Convert.ToDouble(ds.Tables[1].Rows[stu]["marks_obtained"].ToString());
                                                            if (mark > 0)
                                                            {
                                                                totmark = totmark + mark;
                                                                totstudcount++;

                                                            }
                                                            maxmark = maxmark + Convert.ToDouble(ds.Tables[1].Rows[stu]["max_mark"].ToString());
                                                            if (marks_per != "EL" && marks_per != "EOD")
                                                            {
                                                                if (mark < min)
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightPink;
                                                                    failcout++;
                                                                    result = "Fail";
                                                                }
                                                            }
                                                            //  Double perecentage = totmark / maxmark * 100;

                                                            Double perecentage = 0;
                                                            if (totmark != 0 && totstudcount != 0)
                                                            {
                                                                perecentage = totmark / totstudcount;
                                                                perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                            else
                                                            {
                                                                perecentage = 0;
                                                            }



                                                            //Double perecentage = totmark / totstudcount;

                                                            //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;

                                                            //DataView dv = new DataView();
                                                            //ds.Tables[1].DefaultView.RowFilter = "marks_obtained<0 and Roll_No='" + ds.Tables[1].Rows[stu]["Roll_No"].ToString() + "'";
                                                            //dv = ds.Tables[1].DefaultView;
                                                            //if (abse == 0)
                                                            //{
                                                            //    abse = dv.Count;
                                                            //}
                                                            //else
                                                            //{
                                                            //    abse = abse + dv.Count;
                                                            //}
                                                        }

                                                    }

                                                    if (stu == ds.Tables[1].Rows.Count - 1)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                                        //Double perecentage = totmark / maxmark * 100;
                                                        //Double perecentage = totmark / totstudcount;

                                                        //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                                        Double perecentage = 0;
                                                        if (totmark != 0 && totstudcount != 0)
                                                        {
                                                            perecentage = totmark / totstudcount;
                                                            perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                                        }
                                                        else
                                                        {
                                                            perecentage = 0;
                                                        }



                                                        //  totalpercentage.Add(Convert.ToInt32(ds.Tables[1].Rows[stu]["Roll_No"].ToString()),Convert.ToInt32(perecentage));
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                                        if (ds.Tables[1].Rows[stu - 1]["stud_type"].ToString() == "Hostler")
                                                        {
                                                            if (result == "Pass")
                                                            {
                                                                passcnt++;
                                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                                {
                                                                    girlpass++;
                                                                }
                                                                else
                                                                {
                                                                    boypass++;
                                                                }

                                                            }
                                                            else
                                                            {
                                                                failcnt++;
                                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                                {
                                                                    girl1fail++;
                                                                }
                                                                else
                                                                {
                                                                    boy1fail++;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (result == "Pass")
                                                            {
                                                                passcnt1++;
                                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                                {
                                                                    girlpass++;
                                                                }
                                                                else
                                                                {
                                                                    boypass++;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                failcnt1++;
                                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                                {
                                                                    girl1fail++;
                                                                }
                                                                else
                                                                {
                                                                    boy1fail++;
                                                                }
                                                            }
                                                        }
                                                        if (!count.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                                        {
                                                            if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                                            {
                                                                totalpercentage.Add((ds.Tables[1].Rows[stu]["Roll_No"].ToString()), Convert.ToInt32(perecentage));

                                                            }
                                                        }
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;
                                                        if (hatfailcount.Contains(failcout))
                                                        {
                                                            int fc = Convert.ToInt32(hatfailcount[failcout]);
                                                            fc++;
                                                            hatfailcount[failcout] = fc;
                                                        }
                                                        else
                                                        {
                                                            hatfailcount.Add(failcout, 1);
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (DropDownList2.SelectedItem.Text == "--Select--")
                                {
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            // Double perecentage = totmark / maxmark * 100;
                                            //Double perecentage = totmark / totstudcount;

                                            //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                            Double perecentage = 0;
                                            if (totmark != 0 && totstudcount != 0)
                                            {
                                                perecentage = totmark / totstudcount;
                                                perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                            }
                                            else
                                            {
                                                perecentage = 0;
                                            }


                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                            string type = ds.Tables[1].Rows[stu - 1]["stud_type"].ToString();
                                            if (ds.Tables[1].Rows[stu - 1]["stud_type"].ToString() == "Hostler")
                                            {
                                                if (result == "Pass")
                                                {
                                                    passcnt++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girlpass++;
                                                    }
                                                    else
                                                    {
                                                        boypass++;
                                                    }

                                                }
                                                else
                                                {
                                                    failcnt++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girl1fail++;
                                                    }
                                                    else
                                                    {
                                                        boy1fail++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (result == "Pass")
                                                {
                                                    passcnt1++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girlpass++;
                                                    }
                                                    else
                                                    {
                                                        boypass++;
                                                    }
                                                }
                                                else
                                                {
                                                    failcnt1++;
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        girl1fail++;
                                                    }
                                                    else
                                                    {
                                                        boy1fail++;
                                                    }
                                                }
                                            }
                                            if (!count.ContainsKey((rollno)))
                                            {
                                                if (!count1.ContainsKey((rollno)))
                                                {
                                                    if (!count2.ContainsKey((rollno)))
                                                    {
                                                        if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                                        {
                                                            totalpercentage.Add((rollno), Convert.ToInt32(perecentage));

                                                        }

                                                    }
                                                }
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;
                                            if (hatfailcount.Contains(failcout))
                                            {
                                                int fc = Convert.ToInt32(hatfailcount[failcout]);
                                                fc++;
                                                hatfailcount[failcout] = fc;
                                            }
                                            else
                                            {
                                                hatfailcount.Add(failcout, 1);
                                            }

                                        }
                                        string medium1 = "";
                                        //if (chklscriteria.Items[1].Selected == true)
                                        //{
                                        if (temproll != rollno)
                                        {
                                            if (rollno.Trim() != "")
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Left;
                                                medium1 = d2.GetFunctionv("select distinct TextVal from TextValTable  where TextCode in(select medium_ins from applyn where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + " and medium_ins is not NULL) and TextCriteria='medi'");
                                                if ((medium1 == "") || (medium1 == null))
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Text = "-";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Bold = false;

                                                }
                                                else
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Text = medium1.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Bold = false;


                                                }
                                            }
                                        }

                                        //}
                                        if (chklscriteria.Items[2].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    string s = "select  distinct c.course_id from degree as d, course as c where d.course_id=c.course_id and d.dept_code in(" + degreecode + ") "; //
                                                    DataSet ds4 = new DataSet();
                                                    ds4 = d2.select_method_wo_parameter(s, "text");
                                                    for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
                                                    {
                                                        string schoolgrd = d2.GetFunction("select edu_level from course where course_id= " + ds4.Tables[0].Rows[i]["course_id"].ToString() + "");
                                                        if (schoolgrd != string.Empty)
                                                        {
                                                            if (schoolgrd == "UG" || schoolgrd == "U.G")
                                                            {
                                                                string scholmrk = d2.GetFunction("select percentage from stud_prev_details as s,textvaltable as t where app_no= " + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%'))");

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Right;

                                                                if (scholmrk != string.Empty)
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = scholmrk.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }
                                                                else
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }

                                                                string scholmrk1 = d2.GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no= " + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%')))");

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                if (scholmrk1 != string.Empty)
                                                                {
                                                                    string sam = scholmrk1.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = scholmrk1.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                }
                                                                else
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                }

                                                            }

                                                        }
                                                        else if (schoolgrd == "PG" || schoolgrd == "P.G")
                                                        {
                                                            if (temproll != rollno)
                                                            {

                                                                string scholmrk2 = d2.GetFunction("select distinct percentage from stud_prev_details as s,textvaltable as t where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC'))and (textval not like '%H.SC(SB)') and (textval not like '%intermediate')");
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                if (scholmrk2 != string.Empty)
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = scholmrk2.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }
                                                                else
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                                }
                                                                string scholmrk3 = d2.GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and (textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC') and (textval not like '%H.SC(SB)') and (textval not like '%intermediate'))");
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                if (scholmrk3 != string.Empty)
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = scholmrk3.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;

                                                                }
                                                                else
                                                                {

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[3].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    degree_codeparticularstudent = ds.Tables[1].Rows[FpSpread1.Sheets[0].RowCount - 1]["degree_code"].ToString();
                                                    int sem1 = Convert.ToInt32(ddlsemester.SelectedValue.ToString());
                                                    double degcgpa = Math.Round(findgrade(ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["RollNumber"].ToString(), sem1), 2);
                                                    degcgpa = Math.Round(degcgpa, 2);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Text = degcgpa.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Bold = false;
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[3].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Text = failcout.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Bold = false;
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[8].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Day Scholar")
                                                        {

                                                            d_pass_count++;
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Bold = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Day Scholar")
                                                        {
                                                            d_fail_count++;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (chklscriteria.Items[9].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Hostler")
                                                        {
                                                            h_pass_count++;
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Bold = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Hostler")
                                                        {
                                                            h_fail_count++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[10].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                                                        {
                                                            t_pass_count++;
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Bold = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                                                        {
                                                            t_fail_count++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[11].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (failcout == 0)
                                                    {
                                                        if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                                                        {
                                                            //if (!arr_medium.Contains(rollno))
                                                            //{
                                                            e_pass_count++;
                                                            //    arr_medium.Add(arr_medium);
                                                            //}
                                                            bs_count = 1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Text = bs_count.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Bold = false;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                    }
                                                    else

                                                        if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                                                        {
                                                            e_fail_count++;
                                                        }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[20].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    int g = int.Parse(ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString());
                                                    string gender;
                                                    if (g == 1)
                                                    {
                                                        gender = "G";
                                                    }
                                                    else
                                                    {
                                                        gender = "B";
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Text = gender.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Bold = false;
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[13].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                    {
                                                        gs_pass_count++;
                                                        tot_gs_count++;
                                                        gs_count = 1;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Text = gs_count.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Bold = false;
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[14].Selected == true)
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "0")
                                                    {
                                                        bs_pass_count++;
                                                        tot_bs_count++;
                                                        bs_count = 1;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Text = bs_count.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Bold = false;
                                                    }
                                                }
                                            }
                                        }
                                        if (chklscriteria.Items[23].Selected == true)//modified on 01.08.12
                                        {
                                            if (temproll != rollno)
                                            {
                                                if (rollno.Trim() != "")
                                                {
                                                    string textval = "";
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["SeatType"].ToString() != "" && ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["SeatType"].ToString() != " ")
                                                    {
                                                        textval = d2.GetFunction("Select TextVal from textvaltable where textcode=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["seattype"].ToString() + "");
                                                    }
                                                    else
                                                    {
                                                        textval = "-";
                                                    }
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Text = textval.ToString();
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].HorizontalAlign = HorizontalAlign.Center;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Name = "Book Antiqua";
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Size = FontUnit.Medium;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Bold = false;
                                                    //quota_count = quotacount;
                                                }
                                            }
                                        }
                                        failcout = 0;
                                        sno++;
                                        result = "Pass";
                                        totmark = 0;
                                        totstudcount = 0; ;

                                        maxmark = 0;
                                        abse = 0;
                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = false;
                                        string degdetails = ds.Tables[1].Rows[stu]["Degreedetails"].ToString();
                                        string[] degsplit = degdetails.Split('-');
                                        string degree = "";
                                        if (degsplit.Length == 3)
                                        {
                                            if (degsplit[0] != "" && degsplit[0] != null)
                                            {
                                                degree = degsplit[0].ToString();
                                            }
                                            if (degsplit[1] != "" && degsplit[1] != null)
                                            {
                                                degree += " - " + degsplit[1].ToString();
                                            }
                                            if (degsplit[2] != "" && degsplit[2] != null)
                                            {
                                                degree += " - " + degsplit[2].ToString();
                                            }
                                        }
                                        else if (degsplit.Length == 2)
                                        {
                                            if (degsplit[0] != "" && degsplit[0] != null)
                                            {
                                                degree = degsplit[0].ToString();
                                            }
                                            if (degsplit[1] != "" && degsplit[1] != null)
                                            {
                                                degree += " - " + degsplit[1].ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (degsplit[0] != "" && degsplit[0] != null)
                                            {
                                                degree = degsplit[0].ToString();
                                            }
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = degree.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[1].Rows[stu]["Roll_No"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[1].Rows[stu]["Reg_no"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[1].Rows[stu]["Stud_Type"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[1].Rows[stu]["Stud_Name"].ToString();
                                        rollno = temproll;

                                        //if (!count.ContainsKey(Convert.ToInt32(ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                        //{
                                        //    if (!count1.ContainsKey(Convert.ToInt32(ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                        //    {
                                        //        if (!count2.ContainsKey(Convert.ToInt32(ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                        //        {
                                        //            if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                        //            {
                                        //                totalpercentage.Add(Convert.ToInt32(temproll), Convert.ToInt32(temproll));
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                    }
                                    string subcode = ds.Tables[1].Rows[stu]["Subject_Code"].ToString().Trim();
                                    if (hat.Contains(subcode))
                                    {
                                        int col = Convert.ToInt32(hat[subcode].ToString());
                                        string marks_per = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                        if (Convert.ToInt32(marks_per) < 0)
                                        {
                                            switch (marks_per)
                                            {
                                                case "-1":
                                                    marks_per = "AAA";
                                                    abse++;
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
                                                    odcnt++;
                                                    break;
                                                case "-17":
                                                    marks_per = "LA";

                                                    break;

                                                case "-18":
                                                    marks_per = "RAA";
                                                    abse = 0;
                                                    break;

                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = marks_per;
                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                            subsno++;

                                            Double markster = 0;
                                            if (ds.Tables[1].Rows[stu]["marks_obtained"].ToString().Trim() != "")
                                            {
                                                markster = Convert.ToDouble(ds.Tables[1].Rows[stu]["marks_obtained"].ToString());
                                                string subcodeh = ds.Tables[1].Rows[stu]["Subject_Code"].ToString().Trim().ToLower();
                                                if (hatsubmarkavg.Contains(subcodeh))
                                                {
                                                    markster = markster + Convert.ToDouble(hatsubmarkavg[subcodeh].ToString());
                                                    hatsubmarkavg[subcodeh] = markster;
                                                }
                                                else
                                                {
                                                    hatsubmarkavg.Add(subcodeh, markster);
                                                }
                                            }
                                        }
                                        string minmark = ds.Tables[1].Rows[stu]["min_mark"].ToString();
                                        if (minmark.Trim() != "" && minmark != null)
                                        {
                                            Double min = Convert.ToDouble(minmark);
                                            Double mark = Convert.ToDouble(ds.Tables[1].Rows[stu]["marks_obtained"].ToString());
                                            if (mark > 0)
                                            {
                                                totmark = totmark + mark;
                                                totstudcount++;

                                            }
                                            maxmark = maxmark + Convert.ToDouble(ds.Tables[1].Rows[stu]["max_mark"].ToString());
                                            if (marks_per != "EL" && marks_per != "EOD")
                                            {
                                                if (mark < min)
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightPink;
                                                    failcout++;
                                                    result = "Fail";
                                                }
                                            }
                                            //Double perecentage = totmark / maxmark * 100;
                                            //Double perecentage = totmark / totstudcount;

                                            //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                            Double perecentage = 0;
                                            if (totmark != 0 && totstudcount != 0)
                                            {
                                                perecentage = totmark / totstudcount;
                                                perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                            }
                                            else
                                            {
                                                perecentage = 0;
                                            }


                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;

                                            //DataView dv = new DataView();
                                            //ds.Tables[1].DefaultView.RowFilter = "marks_obtained<0 and Roll_No='" + ds.Tables[1].Rows[stu]["Roll_No"].ToString() + "'";
                                            //dv = ds.Tables[1].DefaultView;
                                            //if (abse == 0)
                                            //{
                                            //    abse = dv.Count;
                                            //}
                                            //else
                                            //{
                                            //    abse = abse + dv.Count;
                                            //}
                                        }

                                    }

                                    if (stu == ds.Tables[1].Rows.Count - 1)
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                        // Double perecentage = totmark / maxmark * 100;

                                        //Double perecentage = totmark / totstudcount;

                                        //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                        Double perecentage = 0;
                                        if (totmark != 0 && totstudcount != 0)
                                        {
                                            perecentage = totmark / totstudcount;
                                            perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                        }
                                        else
                                        {
                                            perecentage = 0;
                                        }


                                        //  totalpercentage.Add(Convert.ToInt32(ds.Tables[1].Rows[stu]["Roll_No"].ToString()),Convert.ToInt32(perecentage));
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                        if (ds.Tables[1].Rows[stu - 1]["stud_type"].ToString() == "Hostler")
                                        {
                                            if (result == "Pass")
                                            {
                                                passcnt++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girlpass++;
                                                }
                                                else
                                                {
                                                    boypass++;
                                                }

                                            }
                                            else
                                            {
                                                failcnt++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girl1fail++;
                                                }
                                                else
                                                {
                                                    boy1fail++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (result == "Pass")
                                            {
                                                passcnt1++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girlpass++;
                                                }
                                                else
                                                {
                                                    boypass++;
                                                }
                                            }
                                            else
                                            {
                                                failcnt1++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girl1fail++;
                                                }
                                                else
                                                {
                                                    boy1fail++;
                                                }
                                            }
                                        }
                                        if (!count.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                        {
                                            if (!count1.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                            {
                                                if (!count2.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                                {
                                                    if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                                    {
                                                        totalpercentage.Add((ds.Tables[1].Rows[stu]["Roll_No"].ToString()), Convert.ToInt32(perecentage));

                                                    }
                                                }
                                            }
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;
                                        if (hatfailcount.Contains(failcout))
                                        {
                                            int fc = Convert.ToInt32(hatfailcount[failcout]);
                                            fc++;
                                            hatfailcount[failcout] = fc;
                                        }
                                        else
                                        {
                                            hatfailcount.Add(failcout, 1);
                                        }
                                    }

                                }
                            }

                        }
                        else
                        {

                            if (DropDownList2.SelectedItem.Text == "--Select--")
                            {
                                //if (!count.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                //{
                                //    if (!count1.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                //    {
                                //        if (!count2.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                //        {
                                if (temproll != rollno)
                                {
                                    if (rollno.Trim() != "")
                                    {
                                        // Double perecentage = totmark / maxmark * 100;
                                        //Double perecentage = totmark / totstudcount;

                                        //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                        Double perecentage = 0;
                                        if (totmark != 0 && totstudcount != 0)
                                        {
                                            perecentage = totmark / totstudcount;
                                            perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                        }
                                        else
                                        {
                                            perecentage = 0;
                                        }

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                        if (ds.Tables[1].Rows[stu - 1]["stud_type"].ToString() == "Hostler")
                                        {
                                            if (result == "Pass")
                                            {
                                                passcnt++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girlpass++;
                                                }
                                                else
                                                {
                                                    boypass++;
                                                }

                                            }
                                            else
                                            {
                                                failcnt++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girl1fail++;
                                                }
                                                else
                                                {
                                                    boy1fail++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (result == "Pass")
                                            {
                                                passcnt1++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girlpass++;
                                                }
                                                else
                                                {
                                                    boypass++;
                                                }
                                            }
                                            else
                                            {
                                                failcnt1++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girl1fail++;
                                                }
                                                else
                                                {
                                                    boy1fail++;
                                                }
                                            }
                                        }

                                        if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                        {
                                            totalpercentage.Add((rollno), Convert.ToInt32(perecentage));

                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;
                                        if (hatfailcount.Contains(failcout))
                                        {
                                            int fc = Convert.ToInt32(hatfailcount[failcout]);
                                            fc++;
                                            hatfailcount[failcout] = fc;
                                        }
                                        else
                                        {
                                            hatfailcount.Add(failcout, 1);
                                        }

                                    }
                                    string medium1 = "";
                                    //if (chklscriteria.Items[1].Selected == true)
                                    //{
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Left;
                                            medium1 = d2.GetFunctionv("select distinct TextVal from TextValTable  where TextCode in(select medium_ins from applyn where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + " and medium_ins is not NULL) and TextCriteria='medi'");
                                            if ((medium1 == "") || (medium1 == null))
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Text = "-";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Bold = false;

                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Text = medium1.ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Bold = false;


                                            }
                                        }
                                    }

                                    //}
                                    if (chklscriteria.Items[2].Selected == true)
                                    {
                                        if (temproll != rollno)
                                        {
                                            if (rollno.Trim() != "")
                                            {
                                                string s = "select  distinct c.course_id from degree as d, course as c where d.course_id=c.course_id and d.dept_code in(" + degreecode + ") "; //
                                                DataSet ds4 = new DataSet();
                                                ds4 = d2.select_method_wo_parameter(s, "text");
                                                for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
                                                {
                                                    string schoolgrd = d2.GetFunction("select edu_level from course where course_id= " + ds4.Tables[0].Rows[i]["course_id"].ToString() + "");
                                                    if (schoolgrd != string.Empty)
                                                    {
                                                        if (schoolgrd == "UG" || schoolgrd == "U.G")
                                                        {
                                                            string scholmrk = d2.GetFunction("select percentage from stud_prev_details as s,textvaltable as t where app_no= " + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%'))");

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Right;

                                                            if (scholmrk != string.Empty)
                                                            {
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = scholmrk.ToString();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                            }
                                                            else
                                                            {
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = "-";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                            }

                                                            string scholmrk1 = d2.GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no= " + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%')))");

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                            if (scholmrk1 != string.Empty)
                                                            {
                                                                string sam = scholmrk1.ToString();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = scholmrk1.ToString();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                            }
                                                            else
                                                            {

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = "-";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                            }

                                                        }

                                                    }
                                                    else if (schoolgrd == "PG" || schoolgrd == "P.G")
                                                    {
                                                        if (temproll != rollno)
                                                        {

                                                            string scholmrk2 = d2.GetFunction("select distinct percentage from stud_prev_details as s,textvaltable as t where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC'))and (textval not like '%H.SC(SB)') and (textval not like '%intermediate')");
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                            if (scholmrk2 != string.Empty)
                                                            {

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = scholmrk2.ToString();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                            }
                                                            else
                                                            {
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = "-";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                            }
                                                            string scholmrk3 = d2.GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and (textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC') and (textval not like '%H.SC(SB)') and (textval not like '%intermediate'))");
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                            if (scholmrk3 != string.Empty)
                                                            {

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = scholmrk3.ToString();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;

                                                            }
                                                            else
                                                            {

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = "-";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (chklscriteria.Items[3].Selected == true)
                                    {
                                        if (temproll != rollno)
                                        {
                                            if (rollno.Trim() != "")
                                            {
                                                degree_codeparticularstudent = ds.Tables[1].Rows[FpSpread1.Sheets[0].RowCount - 1]["degree_code"].ToString();
                                                int sem1 = Convert.ToInt32(ddlsemester.SelectedValue.ToString());
                                                double degcgpa = Math.Round(findgrade(ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["RollNumber"].ToString(), sem1), 2);
                                                degcgpa = Math.Round(degcgpa, 2);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Text = degcgpa.ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Bold = false;
                                            }
                                        }
                                    }
                                    if (chklscriteria.Items[3].Selected == true)
                                    {
                                        if (temproll != rollno)
                                        {
                                            if (rollno.Trim() != "")
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Text = failcout.ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Bold = false;
                                            }
                                        }
                                    }
                                    if (chklscriteria.Items[8].Selected == true)
                                    {
                                        if (temproll != rollno)
                                        {
                                            if (rollno.Trim() != "")
                                            {
                                                if (failcout == 0)
                                                {
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Day Scholar")
                                                    {

                                                        d_pass_count++;
                                                        bs_count = 1;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Text = bs_count.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Bold = false;
                                                    }
                                                }
                                                else
                                                {
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Day Scholar")
                                                    {
                                                        d_fail_count++;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (chklscriteria.Items[9].Selected == true)
                                    {
                                        if (temproll != rollno)
                                        {
                                            if (rollno.Trim() != "")
                                            {
                                                if (failcout == 0)
                                                {
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Hostler")
                                                    {
                                                        h_pass_count++;
                                                        bs_count = 1;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Text = bs_count.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Bold = false;
                                                    }
                                                }
                                                else
                                                {
                                                    if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Hostler")
                                                    {
                                                        h_fail_count++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (chklscriteria.Items[10].Selected == true)
                                    {
                                        if (temproll != rollno)
                                        {
                                            if (rollno.Trim() != "")
                                            {
                                                if (failcout == 0)
                                                {
                                                    if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                                                    {
                                                        t_pass_count++;
                                                        bs_count = 1;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Text = bs_count.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Bold = false;
                                                    }
                                                }
                                                else
                                                {
                                                    if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                                                    {
                                                        t_fail_count++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (chklscriteria.Items[11].Selected == true)
                                    {
                                        if (temproll != rollno)
                                        {
                                            if (rollno.Trim() != "")
                                            {
                                                if (failcout == 0)
                                                {
                                                    if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                                                    {
                                                        //if (!arr_medium.Contains(rollno))
                                                        //{
                                                        e_pass_count++;
                                                        //    arr_medium.Add(arr_medium);
                                                        //}
                                                        bs_count = 1;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Text = bs_count.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Bold = false;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].HorizontalAlign = HorizontalAlign.Center;
                                                    }
                                                }
                                                else

                                                    if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                                                    {
                                                        e_fail_count++;
                                                    }
                                            }
                                        }
                                    }
                                    if (chklscriteria.Items[20].Selected == true)
                                    {
                                        if (temproll != rollno)
                                        {
                                            if (rollno.Trim() != "")
                                            {
                                                int g = int.Parse(ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString());
                                                string gender;
                                                if (g == 1)
                                                {
                                                    gender = "G";
                                                }
                                                else
                                                {
                                                    gender = "B";
                                                }
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Text = gender.ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Bold = false;
                                            }
                                        }
                                    }
                                    if (chklscriteria.Items[13].Selected == true)
                                    {
                                        if (temproll != rollno)
                                        {
                                            if (rollno.Trim() != "")
                                            {
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    gs_pass_count++;
                                                    tot_gs_count++;
                                                    gs_count = 1;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Text = gs_count.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Bold = false;
                                                }
                                            }
                                        }
                                    }
                                    if (chklscriteria.Items[14].Selected == true)
                                    {
                                        if (temproll != rollno)
                                        {
                                            if (rollno.Trim() != "")
                                            {
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "0")
                                                {
                                                    bs_pass_count++;
                                                    tot_bs_count++;
                                                    bs_count = 1;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Text = bs_count.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Bold = false;
                                                }
                                            }
                                        }
                                    }
                                    if (chklscriteria.Items[23].Selected == true)//modified on 01.08.12
                                    {
                                        if (temproll != rollno)
                                        {
                                            if (rollno.Trim() != "")
                                            {
                                                string textval = "";
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["SeatType"].ToString() != "" && ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["SeatType"].ToString() != " ")
                                                {
                                                    textval = d2.GetFunction("Select TextVal from textvaltable where textcode=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["seattype"].ToString() + "");
                                                }
                                                else
                                                {
                                                    textval = "-";
                                                }
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Text = textval.ToString();
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].HorizontalAlign = HorizontalAlign.Center;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Size = FontUnit.Medium;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Bold = false;
                                                //quota_count = quotacount;
                                            }
                                        }
                                    }
                                    failcout = 0;
                                    sno++;
                                    result = "Pass";
                                    totmark = 0;
                                    totstudcount = 0;

                                    maxmark = 0;
                                    abse = 0;
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = false;
                                    string degdetails = ds.Tables[1].Rows[stu]["Degreedetails"].ToString();
                                    string[] degsplit = degdetails.Split('-');
                                    string degree = "";
                                    if (degsplit.Length == 3)
                                    {
                                        if (degsplit[0] != "" && degsplit[0] != null)
                                        {
                                            degree = degsplit[0].ToString();
                                        }
                                        if (degsplit[1] != "" && degsplit[1] != null)
                                        {
                                            degree += " - " + degsplit[1].ToString();
                                        }
                                        if (degsplit[2] != "" && degsplit[2] != null)
                                        {
                                            degree += " - " + degsplit[2].ToString();
                                        }
                                    }
                                    else if (degsplit.Length == 2)
                                    {
                                        if (degsplit[0] != "" && degsplit[0] != null)
                                        {
                                            degree = degsplit[0].ToString();
                                        }
                                        if (degsplit[1] != "" && degsplit[1] != null)
                                        {
                                            degree += " - " + degsplit[1].ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (degsplit[0] != "" && degsplit[0] != null)
                                        {
                                            degree = degsplit[0].ToString();
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = degree.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[1].Rows[stu]["Roll_No"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[1].Rows[stu]["Reg_no"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[1].Rows[stu]["Stud_Type"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[1].Rows[stu]["Stud_Name"].ToString();

                                    rollno = temproll;

                                }
                                string subcode = ds.Tables[1].Rows[stu]["Subject_Code"].ToString().Trim();
                                if (hat.Contains(subcode))
                                {
                                    int col = Convert.ToInt32(hat[subcode].ToString());
                                    string marks_per = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                    if (Convert.ToInt32(marks_per) < 0)
                                    {
                                        switch (marks_per)
                                        {
                                            case "-1":
                                                marks_per = "AAA";
                                                abse++;
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
                                                odcnt++;
                                                break;
                                            case "-17":
                                                marks_per = "LA";

                                                break;

                                            case "-18":
                                                marks_per = "RAA";

                                                break;

                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = marks_per;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                        subsno++;
                                    }
                                    string minmark = ds.Tables[1].Rows[stu]["min_mark"].ToString();
                                    if (minmark.Trim() != "" && minmark != null)
                                    {
                                        Double min = Convert.ToDouble(minmark);
                                        Double mark = Convert.ToDouble(ds.Tables[1].Rows[stu]["marks_obtained"].ToString());
                                        if (mark > 0)
                                        {
                                            totmark = totmark + mark;
                                            totstudcount++;

                                        }
                                        maxmark = maxmark + Convert.ToDouble(ds.Tables[1].Rows[stu]["max_mark"].ToString());
                                        if (marks_per != "EL" && marks_per != "EOD")
                                        {
                                            if (mark < min)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightPink;
                                                failcout++;
                                                result = "Fail";
                                            }
                                        }
                                        //Double perecentage = totmark / maxmark * 100;
                                        //Double perecentage = totmark / totstudcount;

                                        //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                        Double perecentage = 0;
                                        if (totmark != 0 && totstudcount != 0)
                                        {
                                            perecentage = totmark / totstudcount;
                                            perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                        }
                                        else
                                        {
                                            perecentage = 0;
                                        }

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;

                                        //DataView dv = new DataView();
                                        //ds.Tables[1].DefaultView.RowFilter = "marks_obtained<0 and Roll_No='" + ds.Tables[1].Rows[stu]["Roll_No"].ToString() + "'";
                                        //dv = ds.Tables[1].DefaultView;
                                        //if (abse == 0)
                                        //{
                                        //    abse = dv.Count;
                                        //}
                                        //else
                                        //{
                                        //    abse = abse + dv.Count;
                                        //}
                                    }

                                }

                                if (stu == ds.Tables[1].Rows.Count - 1)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                    // Double perecentage = totmark / maxmark * 100;
                                    //Double perecentage = totmark / totstudcount;

                                    //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                    Double perecentage = 0;
                                    if (totmark != 0 && totstudcount != 0)
                                    {
                                        perecentage = totmark / totstudcount;
                                        perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                    }
                                    else
                                    {
                                        perecentage = 0;
                                    }

                                    //  totalpercentage.Add(Convert.ToInt32(ds.Tables[1].Rows[stu]["Roll_No"].ToString()),Convert.ToInt32(perecentage));
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                    if (ds.Tables[1].Rows[stu - 1]["stud_type"].ToString() == "Hostler")
                                    {
                                        if (result == "Pass")
                                        {
                                            passcnt++;
                                            if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                            {
                                                girlpass++;
                                            }
                                            else
                                            {
                                                boypass++;
                                            }

                                        }
                                        else
                                        {
                                            failcnt++;
                                            if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                            {
                                                girl1fail++;
                                            }
                                            else
                                            {
                                                boy1fail++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (result == "Pass")
                                        {
                                            passcnt1++;
                                            if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                            {
                                                girlpass++;
                                            }
                                            else
                                            {
                                                boypass++;
                                            }
                                        }
                                        else
                                        {
                                            failcnt1++;
                                            if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                            {
                                                girl1fail++;
                                            }
                                            else
                                            {
                                                boy1fail++;
                                            }
                                        }
                                    }
                                    if (!count.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                    {
                                        if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                        {
                                            totalpercentage.Add((ds.Tables[1].Rows[stu]["Roll_No"].ToString()), Convert.ToInt32(perecentage));

                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;
                                    if (hatfailcount.Contains(failcout))
                                    {
                                        int fc = Convert.ToInt32(hatfailcount[failcout]);
                                        fc++;
                                        hatfailcount[failcout] = fc;
                                    }
                                    else
                                    {
                                        hatfailcount.Add(failcout, 1);
                                    }
                                }

                            }
                            //        }
                            //    }
                            //}

                        }
                    }
                    else
                    {
                        temproll = ds.Tables[1].Rows[stu]["Roll_No"].ToString();
                        if (!count.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                        {
                            if (!count1.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                            {
                                if (!count2.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                {
                                    if (!count4.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                    {
                                        if (!count3.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                        {
                                            count3.Add((ds.Tables[1].Rows[stu]["Roll_No"].ToString()), Convert.ToInt32(stu));
                                            if (temproll != rollno)
                                            {

                                                if (rollno.Trim() != "")
                                                {
                                                    // Double perecentage = totmark / maxmark * 100;
                                                    Double perecentage = totmark / totstudcount;

                                                    perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                                    u++;
                                                    FpSpread1.Visible = true;
                                                    lblrptname.Visible = true;
                                                    txtexcelname.Visible = true;
                                                    rptprint.Visible = true;
                                                    btnExcel.Visible = true;
                                                    BtnPrint.Visible = true;
                                                    //if (FpSpread1.Sheets[0].RowCount > 0)
                                                    //{
                                                    //    if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                                    //    {
                                                    //        count3.Add((rollno), Convert.ToInt32(perecentage));
                                                    //    }
                                                    //}
                                                    //else
                                                    //{

                                                    //    count3.Add((rollno), Convert.ToInt32(perecentage));

                                                    //}
                                                }
                                                string medium1 = "";
                                                totmark = 0;
                                                totstudcount = 0;

                                                maxmark = 0;
                                                rollno = temproll;
                                            }
                                            string subcode = ds.Tables[1].Rows[stu]["Subject_Code"].ToString().Trim();
                                            if (hat.Contains(subcode))
                                            {
                                                int col = Convert.ToInt32(hat[subcode].ToString());
                                                //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                                string minmark = ds.Tables[1].Rows[stu]["min_mark"].ToString();
                                                if (minmark.Trim() != "" && minmark != null)
                                                {
                                                    Double min = Convert.ToDouble(minmark);
                                                    Double mark = Convert.ToDouble(ds.Tables[1].Rows[stu]["marks_obtained"].ToString());
                                                    if (mark > 0)
                                                    {
                                                        totmark = totmark + mark;
                                                        totstudcount++;
                                                    }
                                                    maxmark = maxmark + Convert.ToDouble(ds.Tables[1].Rows[stu]["max_mark"].ToString());
                                                    // if (marks_per != "EL" && marks_per != "EOD")
                                                    {
                                                        if (mark < min)
                                                        {
                                                            int cn = FpSpread1.Sheets[0].RowCount;
                                                            if (cn > 0)
                                                            {
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightPink;
                                                            }
                                                            else
                                                            {
                                                                FpSpread1.Sheets[0].RowCount++;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightPink;
                                                            }
                                                            failcout++;
                                                            result = "Fail";
                                                        }
                                                    }
                                                    DataView dv = new DataView();
                                                    ds.Tables[1].DefaultView.RowFilter = "marks_obtained<0 and Roll_No='" + ds.Tables[1].Rows[stu]["Roll_No"].ToString() + "'";
                                                    dv = ds.Tables[1].DefaultView;
                                                    if (abse == 0)
                                                    {
                                                        abse = dv.Count;
                                                    }
                                                    else
                                                    {
                                                        abse = abse + dv.Count;
                                                    }
                                                }

                                            }

                                            if (stu == ds.Tables[1].Rows.Count)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                                // Double perecentage = totmark / maxmark * 100;
                                                Double perecentage = totmark / totstudcount;

                                                perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                                //  totalpercentage.Add(Convert.ToInt32(ds.Tables[1].Rows[stu]["Roll_No"].ToString()),Convert.ToInt32(perecentage));

                                                if (!count.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                                {
                                                    //  if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                                    {
                                                        count3.Add((ds.Tables[1].Rows[stu]["Roll_No"].ToString()), Convert.ToInt32(perecentage));
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
                if (chtopper.Checked == true)
                {
                    if (u == 0)
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "No Records Found";
                        FpSpread1.Visible = false;
                        rptprint.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnExcel.Visible = false;
                        BtnPrint.Visible = false;
                        return;

                    }
                }
                rollno = "";
                count3 = count3.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                foreach (var kvp in count3)
                {
                    string setval = kvp.Key.ToString();
                    string setvalk = kvp.Value.ToString();
                    for (int stu = 0; stu < ds.Tables[1].Rows.Count; stu++)
                    {
                        temproll = ds.Tables[1].Rows[stu]["Roll_No"].ToString();
                        if (setval == ds.Tables[1].Rows[stu]["Roll_No"].ToString())
                        {

                            if (temproll != rollno)
                            {
                                if (rollno.Trim() != "")
                                {
                                    // Double perecentage = totmark / maxmark * 100;
                                    //Double perecentage = totmark / totstudcount;

                                    //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                    Double perecentage = 0;
                                    if (totmark != 0 && totstudcount != 0)
                                    {
                                        perecentage = totmark / totstudcount;
                                        perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                    }
                                    else
                                    {
                                        perecentage = 0;
                                    }


                                    if (chtopper.Checked != true)
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                    }
                                    else
                                    {
                                        if (result != "Fail")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, totalmarkclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, Percentageclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, Resultcolumn].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, totalmarkclum].Text = totmark.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, Percentageclum].Text = perecentage.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, Resultcolumn].Text = result.ToString();
                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 2].Visible = false;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 2].Visible = false;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 2].Visible = false;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 2].Visible = false;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 2].Visible = false;
                                        }
                                    }
                                    if (chtopper.Checked != true)
                                    {
                                        if (ds.Tables[1].Rows[stu - 1]["stud_type"].ToString() == "Hostler")
                                        {
                                            if (result == "Pass")
                                            {
                                                passcnt++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girlpass++;
                                                }
                                                else
                                                {
                                                    boypass++;
                                                }
                                            }
                                            else
                                            {
                                                failcnt++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girl1fail++;
                                                }
                                                else
                                                {
                                                    boy1fail++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (result == "Pass")
                                            {
                                                passcnt1++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girlpass++;
                                                }
                                                else
                                                {
                                                    boypass++;
                                                }
                                            }
                                            else
                                            {
                                                failcnt1++;
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                                {
                                                    girl1fail++;
                                                }
                                                else
                                                {
                                                    boy1fail++;
                                                }
                                            }
                                        }
                                        if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                        {
                                            totalpercentage.Add((rollno), Convert.ToInt32(perecentage));

                                        }
                                    }

                                }
                                string medium1 = "";
                                //if (chklscriteria.Items[1].Selected == true)
                                //{
                                if (temproll != rollno)
                                {
                                    if (rollno.Trim() != "")
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Left;
                                        //select distinct TextVal from TextValTable  where TextCode in(select medium_ins from applyn where app_no=8463) and TextCriteria='medi'
                                        medium1 = d2.GetFunctionv("select distinct TextVal from TextValTable  where TextCode in(select medium_ins from applyn where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + " and medium_ins is not NULL) and TextCriteria='medi'");
                                        if ((medium1 == "") || (medium1 == null))
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Text = "-";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Bold = false;

                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Text = medium1.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, mediumcount].Font.Bold = false;


                                        }
                                    }
                                }

                                //}
                                if (chklscriteria.Items[2].Selected == true)
                                {
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            string s = "select  distinct c.course_id from degree as d, course as c where d.course_id=c.course_id and d.dept_code in(" + degreecode + ") "; //
                                            DataSet ds4 = new DataSet();
                                            ds4 = d2.select_method_wo_parameter(s, "text");
                                            for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
                                            {
                                                string schoolgrd = d2.GetFunction("select edu_level from course where course_id= " + ds4.Tables[0].Rows[i]["course_id"].ToString() + "");
                                                if (schoolgrd != string.Empty)
                                                {
                                                    if (schoolgrd == "UG" || schoolgrd == "U.G")
                                                    {
                                                        string scholmrk = d2.GetFunction("select percentage from stud_prev_details as s,textvaltable as t where app_no= " + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%'))");

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Right;

                                                        if (scholmrk != string.Empty)
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = scholmrk.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                        }
                                                        else
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = "-";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                        }

                                                        string scholmrk1 = d2.GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no= " + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%')))");

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                        if (scholmrk1 != string.Empty)
                                                        {
                                                            string sam = scholmrk1.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = scholmrk1.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                        }
                                                        else
                                                        {

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = "-";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;
                                                        }

                                                    }

                                                }
                                                else if (schoolgrd == "PG" || schoolgrd == "P.G")
                                                {
                                                    if (temproll != rollno)
                                                    {

                                                        string scholmrk2 = d2.GetFunction("select distinct percentage from stud_prev_details as s,textvaltable as t where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and ((textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC'))and (textval not like '%H.SC(SB)') and (textval not like '%intermediate')");
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                        if (scholmrk2 != string.Empty)
                                                        {

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = scholmrk2.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                        }
                                                        else
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Text = "-";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, grdcount].Font.Bold = false;
                                                        }
                                                        string scholmrk3 = d2.GetFunction("select t.textval as textval from textvaltable t where t.textcode=(select top 1 branch_code from stud_prev_details s,textvaltable t where app_no=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["ApplicationNumber"].ToString() + "  and t.textcode=s.course_code and (textval not like '%X') and (textval not like '%SSLC') and (textval not like '%XII') and (textval not like '%HSLC') and (textval not like '%Diploma') and (textval not like '%DIPLOMA') and (textval not like 'XII') and (textval not like 'X') and (textval not like '%10') and (textval not like '%Tenth') and (textval not like '%Higher Secondary') and (textval not like '%12') and (textval not like '%Twelth')  and (textval not like '%HSC') and (textval not like '%H.SC(SB)') and (textval not like '%intermediate'))");
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                        if (scholmrk3 != string.Empty)
                                                        {

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = scholmrk3.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = false;

                                                        }
                                                        else
                                                        {

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Text = "-";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percount].Font.Bold = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (chklscriteria.Items[3].Selected == true)
                                {
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            degree_codeparticularstudent = ds.Tables[1].Rows[FpSpread1.Sheets[0].RowCount - 1]["degree_code"].ToString();
                                            int sem1 = Convert.ToInt32(ddlsemester.SelectedValue.ToString());
                                            double degcgpa = Math.Round(findgrade(ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["RollNumber"].ToString(), sem1), 2);
                                            degcgpa = Math.Round(degcgpa, 2);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Text = degcgpa.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cgpacount].Font.Bold = false;
                                        }
                                    }
                                }
                                if (chklscriteria.Items[3].Selected == true)
                                {
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Text = failcout.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Nooffailcount].Font.Bold = true;
                                        }
                                    }
                                }
                                if (chklscriteria.Items[8].Selected == true)
                                {
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            if (failcout == 0)
                                            {
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Day Scholar")
                                                {
                                                    d_pass_count++;
                                                    bs_count = 1;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Text = bs_count.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Dpasscount].Font.Bold = false;
                                                }
                                            }
                                            else
                                            {
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Day Scholar")
                                                {
                                                    d_fail_count++;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (chklscriteria.Items[9].Selected == true)
                                {
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            if (failcout == 0)
                                            {
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Hostler")
                                                {
                                                    h_pass_count++;
                                                    bs_count = 1;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Text = bs_count.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Hpasscount].Font.Bold = false;
                                                }
                                            }
                                            else
                                            {
                                                if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["StudentType"].ToString() == "Hostler")
                                                {
                                                    h_fail_count++;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (chklscriteria.Items[10].Selected == true)
                                {
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            if (failcout == 0)
                                            {
                                                if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                                                {
                                                    t_pass_count++;
                                                    bs_count = 1;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Text = bs_count.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Tpasscount].Font.Bold = false;
                                                }
                                            }
                                            else
                                            {
                                                if ((medium1 == "Tamil") || (medium1 == "TAMIL") || (medium1 == "tamil"))
                                                {
                                                    t_fail_count++;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (chklscriteria.Items[11].Selected == true)
                                {
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            if (failcout == 0)
                                            {
                                                if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                                                {
                                                    //if (!arr_medium.Contains(rollno))
                                                    //{
                                                    e_pass_count++;
                                                    //    arr_medium.Add(arr_medium);
                                                    //}
                                                    bs_count = 1;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Text = bs_count.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].Font.Bold = false;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Epasscount].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                            }
                                            else

                                                if ((medium1 == "ENGLISH") || (medium1 == "English") || (medium1 == "english"))
                                                {
                                                    e_fail_count++;
                                                }
                                        }
                                    }
                                }
                                if (chklscriteria.Items[13].Selected == true)
                                {
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            int g = int.Parse(ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString());
                                            string gender;
                                            if (g == 1)
                                            {
                                                gender = "G";
                                            }
                                            else
                                            {
                                                gender = "B";
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Text = gender.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gendercount].Font.Bold = false;
                                        }
                                    }
                                }
                                if (chklscriteria.Items[21].Selected == true)
                                {
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                            {
                                                gs_pass_count++;
                                                tot_gs_count++;
                                                gs_count = 1;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Text = gs_count.ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Gpasscount].Font.Bold = false;
                                            }
                                        }
                                    }
                                }
                                if (chklscriteria.Items[22].Selected == true)
                                {
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "0")
                                            {
                                                bs_pass_count++;
                                                tot_bs_count++;
                                                bs_count = 1;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Text = bs_count.ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Bpasscount].Font.Bold = false;
                                            }
                                        }
                                    }
                                }
                                if (chklscriteria.Items[23].Selected == true)//modified on 01.08.12
                                {
                                    if (temproll != rollno)
                                    {
                                        if (rollno.Trim() != "")
                                        {
                                            string textval = "";
                                            if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["SeatType"].ToString() != "" && ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["SeatType"].ToString() != " ")
                                            {
                                                textval = d2.GetFunction("Select TextVal from textvaltable where textcode=" + ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["seattype"].ToString() + "");
                                            }
                                            else
                                            {
                                                textval = "-";
                                            }
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Text = textval.ToString();
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Size = FontUnit.Medium;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, quotacount].Font.Bold = false;
                                            //quota_count = quotacount;
                                        }
                                    }
                                }
                                failcout = 0;
                                sno++;
                                result = "Pass";
                                totmark = 0;
                                totstudcount = 0;

                                maxmark = 0;
                                abse = 0;
                                if (chtopper.Checked != true)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    // FpSpread1.Sheets[0].Cells[sno - 1, 0].Text = (FpSpread1.Sheets[0].RowCount ).ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = (FpSpread1.Sheets[0].RowCount).ToString();
                                    string degdetails = ds.Tables[1].Rows[stu]["Degreedetails"].ToString();
                                    string[] degsplit = degdetails.Split('-');
                                    string degree = "";
                                    if (degsplit.Length == 3)
                                    {
                                        if (degsplit[0] != "" && degsplit[0] != null)
                                        {
                                            degree = degsplit[0].ToString();
                                        }
                                        if (degsplit[1] != "" && degsplit[1] != null)
                                        {
                                            degree += " - " + degsplit[1].ToString();
                                        }
                                        if (degsplit[2] != "" && degsplit[2] != null)
                                        {
                                            degree += " - " + degsplit[2].ToString();
                                        }
                                    }
                                    else if (degsplit.Length == 2)
                                    {
                                        if (degsplit[0] != "" && degsplit[0] != null)
                                        {
                                            degree = degsplit[0].ToString();
                                        }
                                        if (degsplit[1] != "" && degsplit[1] != null)
                                        {
                                            degree += " - " + degsplit[1].ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (degsplit[0] != "" && degsplit[0] != null)
                                        {
                                            degree = degsplit[0].ToString();
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = degree.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[1].Rows[stu]["Roll_No"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[1].Rows[stu]["Reg_no"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[1].Rows[stu]["Stud_Type"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[1].Rows[stu]["Stud_Name"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].BackColor = Color.Transparent;
                                }
                                else
                                {
                                    if (FpSpread1.Sheets[0].RowCount == 0)
                                    {
                                        FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 2;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].RowCount++;
                                    }
                                    // FpSpread1.Sheets[0].Cells[sno - 1, 0].Text = (FpSpread1.Sheets[0].RowCount ).ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 3].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 4].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 5].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = (FpSpread1.Sheets[0].RowCount - 1).ToString();
                                    string degdetails = ds.Tables[1].Rows[stu]["Degreedetails"].ToString();
                                    string[] degsplit = degdetails.Split('-');
                                    string degree = "";
                                    if (degsplit.Length == 3)
                                    {
                                        if (degsplit[0] != "" && degsplit[0] != null)
                                        {
                                            degree = degsplit[0].ToString();
                                        }
                                        if (degsplit[1] != "" && degsplit[1] != null)
                                        {
                                            degree += " - " + degsplit[1].ToString();
                                        }
                                        if (degsplit[2] != "" && degsplit[2] != null)
                                        {
                                            degree += " - " + degsplit[2].ToString();
                                        }
                                    }
                                    else if (degsplit.Length == 2)
                                    {
                                        if (degsplit[0] != "" && degsplit[0] != null)
                                        {
                                            degree = degsplit[0].ToString();
                                        }
                                        if (degsplit[1] != "" && degsplit[1] != null)
                                        {
                                            degree += " - " + degsplit[1].ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (degsplit[0] != "" && degsplit[0] != null)
                                        {
                                            degree = degsplit[0].ToString();
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].Text = degree.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].Text = ds.Tables[1].Rows[stu]["Roll_No"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 3].Text = ds.Tables[1].Rows[stu]["Reg_no"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 4].Text = ds.Tables[1].Rows[stu]["Stud_Type"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 5].Text = ds.Tables[1].Rows[stu]["Stud_Name"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 5].BackColor = Color.Transparent;
                                }
                                rollno = temproll;
                                if (chtopper.Checked == true)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, rankcount].Text = (FpSpread1.Sheets[0].RowCount - 1).ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, rankcount].Font.Size = FontUnit.Medium;
                                }
                            }
                            string subcode = ds.Tables[1].Rows[stu]["Subject_Code"].ToString().Trim();
                            if (hat.Contains(subcode))
                            {
                                int col = Convert.ToInt32(hat[subcode].ToString());
                                string marks_per = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                if (Convert.ToInt32(marks_per) < 0)
                                {
                                    switch (marks_per)
                                    {
                                        case "-1":
                                            marks_per = "AAA";
                                            abse++;
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
                                            odcnt++;
                                            break;
                                        case "-17":
                                            marks_per = "LA";

                                            break;

                                        case "-18":
                                            marks_per = "RAA";

                                            break;

                                    }
                                    if (chtopper.Checked != true)
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].Text = marks_per;
                                    }
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col + 1].Text = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                    subsno++;
                                }
                                //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = ds.Tables[1].Rows[stu]["marks_obtained"].ToString();
                                string minmark = ds.Tables[1].Rows[stu]["min_mark"].ToString();
                                if (minmark.Trim() != "" && minmark != null)
                                {
                                    Double min = Convert.ToDouble(minmark);
                                    Double mark = Convert.ToDouble(ds.Tables[1].Rows[stu]["marks_obtained"].ToString());
                                    if (mark > 0)
                                    {
                                        totmark = totmark + mark;
                                        totstudcount++;
                                    }
                                    maxmark = maxmark + Convert.ToDouble(ds.Tables[1].Rows[stu]["max_mark"].ToString());
                                    if (marks_per != "EL" && marks_per != "EOD")
                                    {
                                        if (mark < min)
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightPink;
                                            failcout++;
                                            result = "Fail";
                                        }
                                    }
                                    if (chtopper.Checked == true)
                                    {
                                        if (result != "Fail")
                                        {
                                            // Double perecentage = totmark / maxmark * 100;
                                            //Double perecentage = totmark / totstudcount;
                                            //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                            Double perecentage = 0;
                                            if (totmark != 0 && totstudcount != 0)
                                            {
                                                perecentage = totmark / totstudcount;
                                                perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                            }
                                            else
                                            {
                                                perecentage = 0;
                                            }

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, totalmarkclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, Percentageclum].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, Resultcolumn].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, totalmarkclum].Text = totmark.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, Percentageclum].Text = perecentage.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, Resultcolumn].Text = result.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, subjectfail].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, subjectfail].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, subjectfail].Text = failcout.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, noofabcout].Text = abse.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, noofabcout].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, noofabcout].Font.Size = FontUnit.Medium;
                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 2].Visible = false;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 2].Visible = false;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 2].Visible = false;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 2].Visible = false;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 2].Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        //Double perecentage = totmark / maxmark * 100;
                                        //Double perecentage = totmark / totstudcount;

                                        //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                        Double perecentage = 0;
                                        if (totmark != 0 && totstudcount != 0)
                                        {
                                            perecentage = totmark / totstudcount;
                                            perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                        }
                                        else
                                        {
                                            perecentage = 0;
                                        }


                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, subjectfail].Text = failcout.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Text = abse.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Bold = false;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, noofabcout].Font.Size = FontUnit.Medium;
                                    }
                                    //DataView dv = new DataView();
                                    //ds.Tables[1].DefaultView.RowFilter = "marks_obtained<0 and Roll_No='" + ds.Tables[1].Rows[stu]["Roll_No"].ToString() + "'";
                                    //dv = ds.Tables[1].DefaultView;
                                    //if (abse == 0)
                                    //{
                                    //    abse = dv.Count;
                                    //}
                                    //else
                                    //{
                                    //    abse = abse + dv.Count;
                                    //}
                                }
                                if (chtopper.Checked != true)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                }
                                //Double perecentage1 = totmark / maxmark * 100;
                                //perecentage = Math.Round(perecentage1, 2, MidpointRounding.AwayFromZero);
                                ////  totalpercentage.Add(Convert.ToInt32(ds.Tables[1].Rows[stu]["Roll_No"].ToString()),Convert.ToInt32(perecentage));
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = perecentage.ToString();
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = result.ToString();
                            }

                            if (stu == ds.Tables[1].Rows.Count - 1)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Font.Bold = false;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Font.Bold = false;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Font.Bold = false;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = totmark.ToString();
                                // Double perecentage = totmark / maxmark * 100;
                                //Double perecentage = totmark / totstudcount;

                                //perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);

                                Double perecentage = 0;
                                if (totmark != 0 && totstudcount != 0)
                                {
                                    perecentage = totmark / totstudcount;
                                    perecentage = Math.Round(perecentage, 2, MidpointRounding.AwayFromZero);
                                }
                                else
                                {
                                    perecentage = 0;
                                }

                                //  totalpercentage.Add(Convert.ToInt32(ds.Tables[1].Rows[stu]["Roll_No"].ToString()),Convert.ToInt32(perecentage));
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, Percentageclum].Text = perecentage.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, Resultcolumn].Text = result.ToString();
                                if (ds.Tables[1].Rows[stu - 1]["stud_type"].ToString() == "Hostler")
                                {
                                    if (result == "Pass")
                                    {
                                        passcnt++;
                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                        {
                                            girlpass++;
                                        }
                                        else
                                        {
                                            boypass++;
                                        }

                                    }
                                    else
                                    {
                                        failcnt++;
                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                        {
                                            girl1fail++;
                                        }
                                        else
                                        {
                                            boy1fail++;
                                        }
                                    }
                                }
                                else
                                {
                                    if (result == "Pass")
                                    {
                                        passcnt1++;
                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                        {
                                            girlpass++;
                                        }
                                        else
                                        {
                                            boypass++;
                                        }
                                    }
                                    else
                                    {
                                        failcnt1++;
                                        if (ds.Tables[2].Rows[FpSpread1.Sheets[0].RowCount - 1]["Gen"].ToString() == "1")
                                        {
                                            girl1fail++;
                                        }
                                        else
                                        {
                                            boy1fail++;
                                        }
                                    }
                                }
                                if (!count.ContainsKey((ds.Tables[1].Rows[stu]["Roll_No"].ToString())))
                                {
                                    if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text != "Fail")
                                    {
                                        totalpercentage.Add((ds.Tables[1].Rows[stu]["Roll_No"].ToString()), Convert.ToInt32(perecentage));

                                    }
                                }

                            }

                        }
                    }
                }
                if (chtopper.Checked == true)
                {
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Visible = false;
                }

            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
                FpSpread1.Visible = false;
                lblrptname.Visible = false;
                rptprint.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                BtnPrint.Visible = false;
            }


        }
        catch (Exception ex)
        {
            //    errmsg.Visible = true;
            //    errmsg.Text = ex.ToString();
        }
    }

    protected void Chgradechecked(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (Chgrade.Checked == true)
            {
                //BindDegree();
                //BindBranchMultiple();
                //txtdegree.Enabled = false;
                //txtbranch.Enabled = false;
            }
            else
            {
                //BindDegree();
                //BindBranchMultiple();
                //txtdegree.Enabled = true;
                //txtbranch.Enabled = true;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void chtopperchecked(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chtopper.Checked == true)
            {
                BindDegree();
                BindBranchMultiple();
                DropDownList1.Enabled = false;
                DropDownList2.Enabled = false;
                rbt.Items[0].Selected = false;
                rbt.Items[1].Selected = false;
                rbt.Items[2].Selected = false;
                rbt.Items[0].Enabled = false;
                rbt.Items[1].Enabled = false;
                rbt.Items[2].Enabled = false;
                txtcriteria.Enabled = false;
                chkcriteria.Checked = false;
                chklscriteria.ClearSelection();
                txtoptiminpassmark.Enabled = false;
                txtcriteria.Text = " ";
                txtdegree.Enabled = false;
                txtbranch.Enabled = false;
                rbtsubject.Items[0].Selected = false;
                rbtsubject.Items[1].Selected = false;
                rbtsubject.Items[0].Enabled = false;
                rbtsubject.Items[1].Enabled = false;
                Chgrade.Enabled = false;
                Chgrade.Checked = false;
            }
            else
            {
                DropDownList1.Enabled = true;
                DropDownList2.Enabled = true;
                rbt.Items[0].Selected = false;
                rbt.Items[1].Selected = false;
                rbt.Items[2].Selected = true;
                rbt.Items[0].Enabled = true;
                rbt.Items[1].Enabled = true;
                rbt.Items[2].Enabled = true;
                txtcriteria.Enabled = true;
                txtoptiminpassmark.Enabled = true;
                txtdegree.Enabled = true;
                txtbranch.Enabled = true;
                rbtsubject.Items[0].Selected = true;
                Chgrade.Enabled = true;
                rbtsubject.Items[0].Enabled = true;
                rbtsubject.Items[1].Enabled = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    // public void getspecial_hr()
    //{
    //    //  try
    //    {
    //        string hrdetno = "";
    //        if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
    //        {
    //            hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(dumm_from_date), ht_sphr));

    //        }
    //        if (hrdetno != "")
    //        {
    //            con_splhr_query_master.Close();
    //            con_splhr_query_master.Open();
    //            DataSet ds_splhr_query_master = new DataSet();

    //            string splhr_query_master = "select attendance from specialhr_attendance where roll_no='" + dd + "'  and hrdet_no in(" + hrdetno + ")";

    //            SqlDataReader dr_splhr_query_master;
    //            cmd = new SqlCommand(splhr_query_master, con_splhr_query_master);
    //            dr_splhr_query_master = cmd.ExecuteReader();

    //            while (dr_splhr_query_master.Read())
    //            {
    //                if (dr_splhr_query_master.HasRows)
    //                {
    //                    value = dr_splhr_query_master[0].ToString();

    //                    if (value != null && value != "0" && value != "7" && value != "")
    //                    {
    //                        if (tempvalue != value)
    //                        {
    //                            tempvalue = value;
    //                            for (int j = 0; j < count; j++)
    //                            {

    //                                if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
    //                                {
    //                                    ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
    //                                    j = count;
    //                                }
    //                            }
    //                        }
    //                        if (ObtValue == 1)
    //                        {
    //                            per_abshrs_spl += 1;
    //                        }
    //                        else if (ObtValue == 2)
    //                        {
    //                            notconsider_value += 1;
    //                            njhr += 1;
    //                        }
    //                        else if (ObtValue == 0)
    //                        {
    //                            tot_per_hrs_spl += 1;
    //                        }
    //                        if (value == "3")
    //                        {
    //                            tot_ondu_spl += 1;
    //                        }
    //                        else if (value == "10")
    //                        {
    //                            per_leave += 1;
    //                        }
    //                        if (value == "4")
    //                        {
    //                            tot_ml_spl += 1;
    //                        }
    //                        tot_conduct_hr_spl++;
    //                    }
    //                    else if (value == "7")
    //                    {
    //                        per_hhday_spl += 1;
    //                        tot_conduct_hr_spl--;
    //                    }
    //                    else
    //                    {
    //                        unmark_spl += 1;
    //                        tot_conduct_hr_spl--;
    //                    }
    //                }
    //            }


    //                per_abshrs_spl_fals = per_abshrs_spl;
    //                tot_per_hrs_spl_fals = tot_per_hrs_spl;
    //                per_leave_fals = per_leave;
    //                tot_conduct_hr_spl_fals = tot_conduct_hr_spl;
    //                tot_ondu_spl_fals = tot_ondu_spl;
    //                tot_ml_spl_fals = tot_ml_spl;

    //        }
    //    }
    //    //  catch
    //    {
    //    }
    //}

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

    public void binddate()
    {
        con.Close();
        con.Open();
        string from_date = "";
        string to_date = "";
        string final_from = "";
        string final_to = "";
        SqlDataReader dr_dateset;
        cmd = new SqlCommand("select start_date,end_date from seminfo where degree_code in (" + degreecode + ") and semester=" + ddlsemester.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " ", con);
        dr_dateset = cmd.ExecuteReader();
        dr_dateset.Read();
        if (dr_dateset.HasRows == true)
        {

            //------------get from date
            from_date = dr_dateset[0].ToString();
            string[] from_split = from_date.Split(' ');
            string[] date_split_from = from_split[0].Split('/');
            final_from = date_split_from[2] + "/" + date_split_from[0] + "/" + date_split_from[1];

            string sem_start = final_from;
            frdate = final_from;

            //------------get to date
            to_date = dr_dateset[1].ToString();
            string[] to_split = to_date.Split(' ');
            string[] date_split_to = to_split[0].Split('/');
            final_to = date_split_to[2] + "/" + date_split_to[0] + "/" + date_split_to[1];
            todate = final_to;


        }
    }

    public void persentmonthcal(int ival)
    {

        Boolean isadm = false;
        try
        {
            binddate();

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

            string admdate = ds.Tables[1].Rows[ival]["adm_date"].ToString();
            //string[] adate = admdate.Split(new Char[] { ' ' });
            //string[] admdatesp = adate[0].Split(new Char[] { '/' });
            //admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
            Admission_date = Convert.ToDateTime(admdate);

            //dd = ds5.Tables[0].Rows[student]["RollNumber"].ToString();
            hvtb.Clear();
            hvtb.Add("std_rollno", ds.Tables[1].Rows[ival]["roll_no"].ToString());
            hvtb.Add("from_month", cal_from_date);
            hvtb.Add("to_month", cal_to_date);

            ds6 = d2.select_method("STUD_ATTENDANCE", hvtb, "sp");
            mmyycount = ds6.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
            if (ival == 0)
            {
                hvtb.Clear();
                hvtb.Add("degree_code", int.Parse(ds.Tables[1].Rows[ival]["degree_code"].ToString()));
                hvtb.Add("sem", int.Parse(ddlsemester.SelectedItem.ToString()));
                hvtb.Add("from_date", frdate.ToString());
                hvtb.Add("to_date", todate.ToString());
                hvtb.Add("coll_code", int.Parse(Session["collegecode"].ToString()));


                //------------------------------------------------------------------
                int iscount = 0;
                holidaycon.Close();
                holidaycon.Open();
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code ='" + ds.Tables[1].Rows[ival]["degree_code"].ToString() + "' and semester=" + ddlsemester.SelectedItem.ToString() + "";
                SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
                SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
                DataSet dsholiday = new DataSet();
                daholiday.Fill(dsholiday);
                if (dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                }
                hvtb.Add("iscount", iscount);

                ds7 = d2.select_method("ALL_HOLIDATE_DETAILS", hvtb, "sp");

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
            int next = 0;

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
                                //  getspecial_hr();
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
        catch (Exception ex)
        {

        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            int totsub = 0;
            int branchcnt = 0;
            int seldegree = 0;
            int selbranch = 0;
            errmsg.Text = "";
            DataView dvpass = new DataView();
            if (ddlcollege.Items.Count == 0)
            {
                FpSpread1.Visible = false;
                lblrptname.Visible = false;
                rptprint.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                BtnPrint.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "There is no college were Found!";
                return;
            }

            if (ddlbatch.Items.Count == 0)
            {
                FpSpread1.Visible = false;
                lblrptname.Visible = false;
                rptprint.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                BtnPrint.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "There is no Batch were Found!";
                return;
            }
            if (chklsdegree.Items.Count == 0)
            {
                FpSpread1.Visible = false;
                lblrptname.Visible = false;
                rptprint.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                BtnPrint.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "There is no Degree were Found!";
                return;
            }
            else
            {

                for (int i = 0; i < chklsdegree.Items.Count; i++)
                {
                    if (chklsdegree.Items[i].Selected == true)
                    {
                        seldegree++;

                    }
                }
                if (seldegree == 0 && chklsdegree.Items.Count > 0)
                {
                    FpSpread1.Visible = false;
                    lblrptname.Visible = false;
                    rptprint.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    BtnPrint.Visible = false;
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select Atleast Any One Degree!!!";
                    return;
                }
            }



            if (chklsbranch.Items.Count == 0)
            {
                FpSpread1.Visible = false;
                lblrptname.Visible = false;
                rptprint.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                BtnPrint.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "There is no Branch were Found!";
                return;
            }
            else
            {
                for (int i = 0; i < chklsbranch.Items.Count; i++)
                {
                    if (chklsbranch.Items[i].Selected == true)
                    {
                        selbranch++;

                    }
                }
                if (selbranch == 0 && chklsbranch.Items.Count > 0)
                {
                    FpSpread1.Visible = false;
                    lblrptname.Visible = false;
                    rptprint.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    BtnPrint.Visible = false;
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select Atleast Any One Branch!!!";
                    return;
                }
            }
            if (Chgrade.Checked == true)
            {
                //if (txtdegree.Text == "Degree (1)" && txtbranch.Text == "Branch (1)" && DropDownList2.SelectedItem.Text == "--Select--" && DropDownList1.SelectedItem.Text == "Both")
                //{ }
                if (selbranch != 1 && seldegree != 1 && DropDownList2.SelectedItem.Text == "--Select--" && DropDownList1.SelectedItem.Text == "Both")
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select Only One Degree & Branch And Then Proceed Grade";
                    FpSpread1.Visible = false;
                    rptprint.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    BtnPrint.Visible = false;
                    return;
                }
            }

            if (ddlsemester.Items.Count == 0)
            {
                FpSpread1.Visible = false;
                lblrptname.Visible = false;
                rptprint.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                BtnPrint.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "There is no Semester were Found!";
                return;
                //ddlTest
            }

            if (ddlTest.Items.Count <= 1)
            {
                if (Convert.ToString(ddlTest.SelectedItem) == "Select")
                {
                    FpSpread1.Visible = false;
                    lblrptname.Visible = false;
                    rptprint.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    BtnPrint.Visible = false;
                    errmsg.Visible = true;
                    errmsg.Text = "There is no Test were Found!";
                    return;
                }
                //
            }

            FpSpread1.Visible = true;
            lblrptname.Visible = true;
            rptprint.Visible = true;
            txtexcelname.Visible = true;
            txtexcelname.Text = "";
            btnExcel.Visible = true;
            BtnPrint.Visible = true;
            Printcontrol.Visible = false;

            string sec = "";
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            degreecode = "";
            batchyear = ddlbatch.SelectedItem.ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            sem = ddlsemester.SelectedItem.ToString();
            test = Convert.ToString(ddlTest.SelectedItem.Text);
            filteration();



            if (Chgrade.Checked == true)
            {
                if (DropDownList2.SelectedItem.Text != "--Select--")
                {
                    errmsg.Visible = true;
                    errmsg.Text = "No Grade Visible In Fail List";
                    FpSpread1.Visible = false;
                    lblrptname.Visible = false;
                    rptprint.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    BtnPrint.Visible = false;

                    if (txtdegree.Text != "Degree (1)" && txtbranch.Text != "Branch (1)")
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Please Select Only One Degree & Branch And Then Proceed Grade";
                        FpSpread1.Visible = false;
                        rptprint.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnExcel.Visible = false;
                        BtnPrint.Visible = false;
                        return;
                    }
                    return;
                }
            }
            for (int i = 0; i < chklsbranch.Items.Count; i++)
            {
                if (chklsbranch.Items[i].Selected == true)
                {
                    branchcnt++;
                    if (degreecode.Trim() == "")
                    {
                        degreecode = chklsbranch.Items[i].Value.ToString();
                    }
                    else
                    {
                        degreecode = degreecode + ',' + chklsbranch.Items[i].Value.ToString();
                    }
                }
            }

            if (Session["group_code"] == null || Session["collegecode"] == null || Session["usercode"] == null || Session["Regflag"] == null || Session["Rollflag"] == null || Session["Studflag"] == null)
            {
                Response.Redirect("OverAllCamReport.aspx");
            }
            string degreequery = "";
            if (degreecode.Trim() != "")
            {
                degreequery = " and sy.degree_code in (" + degreecode + ")";
            }
            if (ddlsec.Enabled == true)
            {
                if (ddlsec.Items.Count > 0)
                {
                    sec = ddlsec.SelectedItem.ToString();
                    if (sec.ToUpper().Trim() == "ALL")
                    {
                        sec = "";
                    }
                }
                else
                {
                    sec = "";
                }
            }
            else
            {
                sec = "";
            }

            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            string semlico = d2.GetFunction("select value from Master_Settings where settings='previous sem subject allotment' " + grouporusercode + "");
            int stusemester = Convert.ToInt32(d2.GetFunction("select distinct isnull(Current_Semester,'0') sem from Registration r where Batch_Year='" + batchyear + "' and degree_code in(" + degreecode + ") " + ((sec != "") ? sec : "") + " and cc=0 and DelFlag=0 and Exam_Flag<>'debar' order by sem"));

            string strorder = "ORDER BY Roll_No";
            string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
            if (serialno.Trim() == "1")
            {
                strorder = "ORDER BY r.batch_year,r.degree_code,r.sections,r.serialno";
            }
            else
            {
                string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY r.batch_year,r.degree_code,r.sections,r.Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY r.batch_year,r.degree_code,r.sections,r.Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "ORDER BY r.batch_year,r.degree_code,r.sections,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "ORDER BY r.batch_year,r.degree_code,r.sections,r.Roll_No,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY r.batch_year,r.degree_code,r.sections,r.Roll_No,r.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY r.batch_year,r.degree_code,r.sections,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY r.batch_year,r.degree_code,r.sections,r.Roll_No,r.Stud_Name";
                }
            }
            int merRo = 0;
            if (test != "Select")
            {
                FpSpread1.Visible = true;
                lblrptname.Visible = true;
                rptprint.Visible = true;
                txtexcelname.Visible = true;
                btnExcel.Visible = true;
                BtnPrint.Visible = true;
                string strsec = "";
                if (sec == "")
                {
                    strsec = "";
                }
                else
                {
                    if (sec.ToUpper().Trim() != "ALL")
                    {
                        strsec = "and r.sections in('" + sec + "')";
                    }
                    else
                    {
                        strsec = "";
                    }
                }
                strquery = "select distinct s.subject_name,s.acronym,s.subject_code,ss.lab from subject s,sub_sem ss,syllabus_master sy where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.subType_no=s.subType_no and ss.promote_count=1 and sy.Batch_Year='" + batchyear + "' and sy.semester='" + ddlsemester.SelectedValue.ToString() + "' " + degreequery + " order by ss.lab,s.subject_code";
                // strquery = strquery + " ;select r.Roll_No,r.degree_code,r.Reg_No,r.Stud_Type,r.Stud_Name,e.exam_code,e.subject_no,s.subject_code,re.marks_obtained,e.min_mark,e.max_mark from syllabus_master sy,Exam_type e,Registration r,Result re,CriteriaForInternal c,sub_sem ss,subject s where sy.Batch_Year=e.batch_year and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and e.batch_year=r.Batch_Year and e.sections=r.Sections and e.exam_code=re.exam_code and c.syll_code=sy.syll_code and c.Criteria_no=e.criteria_no and e.subject_no=s.subject_no and s.syll_code=sy.syll_code and ss.syll_code=sy.syll_code and c.syll_code=s.syll_code and ss.syll_code=c.syll_code and s.syll_code=ss.syll_code and r.Roll_No=re.roll_no and s.subType_no=ss.subType_no and sy.syll_code=c.syll_code and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and ss.promote_count=1 and c.criteria='" + ddlTest.SelectedItem.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.ToString() + "' " + degreequery + "  ";

                strquery = strquery + " ;select distinct c.criteria_no,s.subject_name,c.criteria,r.Adm_Date,r.Roll_No,r.serialno,(crs.Course_Name +'-'+dt.dept_acronym+'-'+r.Sections) as Degreedetails,e.exam_date,r.Sections,s.acronym,r.Batch_Year,r.degree_code,r.Reg_No,r.Stud_Type,r.Stud_Name,e.exam_code,e.subject_no,s.subject_code,rs.marks_obtained,e.min_mark,e.max_mark from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result rs,Degree d,Department dt,Course crs where r.Batch_Year =sy.Batch_Year and r.degree_code =sy.degree_code and r.Current_Semester =sy.semester and r.degree_code=d.Degree_Code and d.Dept_Code=dt.Dept_Code and crs.Course_Id=d.Course_Id and r.college_code=dt.college_code and dt.college_code=d.college_code and crs.college_code=dt.college_code and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and s.syll_code =sy.syll_code and c.Criteria_no =e.criteria_no  and s.subject_no =e.subject_no and e.exam_code =rs.exam_code  and r.roll_no =rs.roll_no and r.college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' and r.Current_Semester ='" + ddlsemester.SelectedItem.ToString() + "' and sy.syll_code =c.syll_code and c.criteria ='" + ddlTest.SelectedItem.ToString() + "'" + strsec + " and r.Batch_Year ='" + ddlbatch.SelectedItem.ToString() + "'  " + degreequery + " ";
                if (chtopper.Checked != true)
                {
                    if (DropDownList1.SelectedItem.Text != "Both")
                    {
                        if (DropDownList1.SelectedItem.Text == "Dayscholar")
                        {
                            strquery = strquery + " and e.min_mark>marks_obtained and r.Stud_Type='Day Scholar' ";
                        }
                        else if (DropDownList1.SelectedItem.Text == "Hostler")
                        {
                            strquery = strquery + "and e.min_mark>marks_obtained and r.Stud_Type='Hostler' ";
                        }
                    }
                    else
                    {
                        if (DropDownList2.SelectedItem.Text != "--Select--")
                        {
                            strquery = strquery + "and e.min_mark>marks_obtained ";
                        }
                    }


                    if (rbt.Items[0].Selected == true)
                    {
                        //  strquery = strquery + "and e.min_mark>marks_obtained ";
                    }
                    else if (rbt.Items[1].Selected == true)
                    {
                        strquery = strquery + "and e.min_mark>marks_obtained and marks_obtained not in(-2,-3) ";
                    }
                }
                else
                {
                    //  strquery = strquery + "and e.min_mark>marks_obtained ";
                }
                strquery = strquery + " " + strorder + ",s.subject_code";
                strquery = strquery + ";select distinct len( r.Roll_No) as RollNum_len,(c.Course_Name +'-'+dt.dept_acronym+'-'+r.Sections) as Degreedetails,r.batch_year,r.degree_code,r.sections,r.Roll_No as RollNumber,r.serialno, r.Reg_No as RegistrationNumber, r.stud_name as Student_Name,r.stud_type as StudentType,r.App_No as ApplicationNumber,a.seattype,a.sex as Gen,c.course_name,c.Edu_Level as EDU_LEVEl,(select textval from textvaltable where textcode=seattype) as textval,convert(varchar(15),adm_date,103) as adm_date from registration r, applyn a ,degree d, course c,Department dt where a.app_no=r.app_no and r.degree_code in (" + degreecode + ") and r.college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' and r.batch_year='" + ddlbatch.SelectedItem.ToString() + "'" + strsec + " and RollNo_Flag<>0 and cc=0 and  r.degree_code=d.Degree_Code and d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and r.college_code=dt.college_code and dt.college_code=d.college_code and c.college_code=dt.college_code and exam_flag <> 'DEBAR' and delflag=0 and r.degree_code=a.degree_code  and ((r.mode=1) or (r.mode=3) or (r.mode=2))and ((r.mode=1) or (r.mode=3) or (r.mode=2)) " + strorder;
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(strquery, "Text");
                FpSpread1.Sheets[0].ColumnCount = 6;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;

                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Class";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;

                if (Session["Regflag"].ToString() == "0")
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[3].Visible = false;
                }
                Session["rowcount"] = FpSpread1.Sheets[0].RowCount;
                if (Session["Rollflag"].ToString() == "0")
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                }
                if (Session["Studflag"].ToString() == "0")
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[4].Visible = false;
                }

                FpSpread1.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[0].Font.Bold = false;
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].Width = 50;
                FpSpread1.Sheets[0].Columns[0].CellType = txt;

                FpSpread1.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[1].Font.Bold = false;
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[1].Width = 200;
                FpSpread1.Sheets[0].Columns[1].CellType = txt;

                FpSpread1.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[2].Font.Bold = false;
                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[2].Width = 150;
                FpSpread1.Sheets[0].Columns[2].CellType = txt;

                FpSpread1.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[3].Font.Bold = false;
                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[3].Width = 150;
                FpSpread1.Sheets[0].Columns[3].CellType = txt;

                FpSpread1.Sheets[0].Columns[4].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[4].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[4].Font.Bold = false;
                FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[4].Width = 200;
                FpSpread1.Sheets[0].Columns[4].CellType = txt;

                FpSpread1.Sheets[0].Columns[5].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[5].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[5].Font.Bold = false;
                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[5].Width = 250;
                FpSpread1.Sheets[0].Columns[5].CellType = txt;
                hat.Clear();
                subcou = 0;

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    //if (ds.Tables[0].Rows[i]["lab"].ToString() != "True")
                    //{
                    if (!hat.Contains(ds.Tables[1].Rows[i]["subject_code"].ToString().Trim()))
                    {
                        if (chtopper.Checked != true)
                        {
                            FpSpread1.Sheets[0].ColumnCount++;
                            if (rbtsubject.Items[0].Selected == true)
                            {
                                subcou++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = ds.Tables[1].Rows[i]["acronym"].ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = ds.Tables[1].Rows[i]["subject_no"].ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = ds.Tables[1].Rows[i]["subject_code"].ToString().Trim();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = ds.Tables[1].Rows[i]["subject_code"].ToString().Trim();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            }
                            else
                            {
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = ds.Tables[1].Rows[i]["subject_name"].ToString();
                                subcou++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = ds.Tables[1].Rows[i]["subject_code"].ToString().Trim();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = ds.Tables[1].Rows[i]["subject_no"].ToString();
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            }
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = false;
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 150;
                        }
                        hat.Add(ds.Tables[1].Rows[i]["Subject_Code"].ToString().Trim(), FpSpread1.Sheets[0].ColumnCount - 1);
                    }
                    //}
                }
                totsub = FpSpread1.Sheets[0].ColumnCount;
                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 4;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Text = "Total Marks";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 4, 2, 1);

                totalmarkclum = FpSpread1.Sheets[0].ColumnCount - 4;
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 4].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 4].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 4].Font.Bold = false;
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;


                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Text = "AVG.";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 3, 2, 1);

                Percentageclum = FpSpread1.Sheets[0].ColumnCount - 3;
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 3].Font.Bold = false;
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Text = "Result";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 2, 2, 1);

                Resultcolumn = FpSpread1.Sheets[0].ColumnCount - 2;
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = false;
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Left;

                if (chtopper.Checked != true)
                {
                    if (chklscriteria.Items[22].Selected == true)
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "No of Subject Failed";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);

                        subjectfail = FpSpread1.Sheets[0].ColumnCount - 1;
                        FpSpread1.Sheets[0].Columns[subjectfail].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Columns[subjectfail].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Columns[subjectfail].Font.Bold = false;
                        FpSpread1.Sheets[0].Columns[subjectfail].HorizontalAlign = HorizontalAlign.Center;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    }
                    if (chklscriteria.Items[28].Selected == true)
                    {
                        FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                        noofabcout = FpSpread1.Sheets[0].ColumnCount - 1;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, noofabcout].Text = "No of Subject Absent";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, noofabcout, 2, 1);

                        FpSpread1.Sheets[0].Columns[noofabcout].HorizontalAlign = HorizontalAlign.Center;

                    }
                    //if (chklscriteria.Items[0].Selected == true)
                    //{
                    if (rbt.Items[0].Selected == true)
                    {
                        if (chklscriteria.Items[0].Selected == true)
                        {
                            FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                            rankcount = FpSpread1.Sheets[0].ColumnCount - 1;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, rankcount].Text = "Rank";
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, rankcount, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, rankcount].HorizontalAlign = HorizontalAlign.Center;
                            spancount++;
                        }
                    }

                    else if (rbt.Items[2].Selected == true)
                    {
                        if (chklscriteria.Items[0].Selected == true)
                        {
                            FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                            rankcount = FpSpread1.Sheets[0].ColumnCount - 1;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, rankcount].Text = "Rank";
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, rankcount, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, rankcount].HorizontalAlign = HorizontalAlign.Center;
                            spancount++;
                        }
                    }
                }
                else
                {
                    //if (chklscriteria.Items[0].Selected == true)
                    //{
                    rankcount = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, rankcount].Text = "Rank";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, rankcount, 2, 1);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, rankcount].HorizontalAlign = HorizontalAlign.Center;
                    spancount++;
                    //}
                }
                if (chklscriteria.Items[15].Selected == true)
                {
                    FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                    attendence = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, attendence].Text = "Attendance %";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, attendence, 2, 1);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, attendence].HorizontalAlign = HorizontalAlign.Center;
                    spancount++;
                }

                spread();
                for (int sno1 = 0; sno1 < FpSpread1.Sheets[0].RowCount; sno1++)
                {
                    FpSpread1.Sheets[0].Cells[sno1, 0].Text = Convert.ToString(sno1 + 1);
                    if (chtopper.Checked == true)
                    {
                        if (FpSpread1.Sheets[0].Cells[sno1, Resultcolumn].Text != "Fail")//FpSpread1.Sheets[0].Cells[sno1, Resultcolumn].Text!=""
                        {
                            //Percentageclum 
                            double perc = 0;
                            double total = 0;
                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[sno1, totalmarkclum].Text), out total);
                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[sno1, Percentageclum].Text), out perc);
                            totaltoppers.Add(Convert.ToString(FpSpread1.Sheets[0].Cells[sno1, 2].Text), perc);
                            totaltopperstot.Add(Convert.ToString(FpSpread1.Sheets[0].Cells[sno1, 2].Text), total);

                        }
                    }
                }

                totalpercentage = totalpercentage.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);



                if (chklscriteria.Items[0].Selected == true)
                {

                }
                if (rbt.Items[0].Selected == true)
                {
                    Hashtable htt = new Hashtable();
                    int rcnt = 0;
                    int rcntv = 0;
                    for (int stu = 0; stu < ds.Tables[1].Rows.Count; stu++)
                    {
                        int cnt = 0;
                        if (!htt.ContainsKey(ds.Tables[1].Rows[stu]["Roll_No"].ToString()))
                        {
                            rcntv++;
                            foreach (var kvp in totalpercentage)
                            {
                                string setval = kvp.Key.ToString();
                                string setvalk = kvp.Value.ToString();

                                cnt++;

                                if (ds.Tables[1].Rows[stu]["Roll_No"].ToString() == setval)
                                {
                                    if (chklscriteria.Items[0].Selected == true)
                                    {
                                        if (rbt.Items[0].Selected == true)
                                        {
                                            rcnt++;
                                            FpSpread1.Sheets[0].Cells[rcnt - 1, rankcount].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[rcnt - 1, rankcount].VerticalAlign = VerticalAlign.Middle;
                                            if (FpSpread1.Sheets[0].Cells[rcnt - 1, Resultcolumn].Text != "Fail")
                                            {
                                                //if (!htt.ContainsKey(ds.Tables[1].Rows[stu]["marks_obtained"].ToString()))
                                                //{
                                                FpSpread1.Sheets[0].Cells[rcnt - 1, rankcount].Text = cnt.ToString();
                                                FpSpread1.Sheets[0].Cells[rcnt - 1, rankcount].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[rcnt - 1, rankcount].VerticalAlign = VerticalAlign.Middle;
                                                //  htt.Add(setval, setvalk);
                                            }
                                            FpSpread1.Sheets[0].Cells[rcnt - 1, rankcount].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[rcnt - 1, rankcount].Font.Size = FontUnit.Medium;
                                        }
                                        else if (rbt.Items[2].Selected == true)
                                        {
                                            //rcnt++;

                                            if (FpSpread1.Sheets[0].Cells[rcntv - 1, Resultcolumn].Text != "Fail")
                                            {
                                                FpSpread1.Sheets[0].Cells[rcntv - 1, rankcount].Text = cnt.ToString();
                                                FpSpread1.Sheets[0].Cells[rcntv - 1, rankcount].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[rcntv - 1, rankcount].VerticalAlign = VerticalAlign.Middle;
                                                //  htt.Add(setval, setvalk);
                                            }
                                            FpSpread1.Sheets[0].Cells[rcntv - 1, rankcount].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[rcntv - 1, rankcount].VerticalAlign = VerticalAlign.Middle;
                                            FpSpread1.Sheets[0].Cells[rcntv - 1, rankcount].Font.Bold = false;
                                            FpSpread1.Sheets[0].Cells[rcntv - 1, rankcount].Font.Size = FontUnit.Medium;
                                        }
                                    }
                                }
                            }
                            htt.Add(ds.Tables[1].Rows[stu]["Roll_No"].ToString(), ds.Tables[1].Rows[stu]["Roll_No"].ToString());
                        }
                    }
                }
                if (chtopper.Checked == true || (rbt.Items[2].Selected == true && DropDownList1.SelectedItem.Text == "Both" && DropDownList2.SelectedItem.Text == "--Select--"))
                {
                    if (txtcriteria.Text != "" || txtcriteria.Text != "Select All")
                    {
                        Hashtable hv1 = new Hashtable();
                        int cot = 0;
                        for (int stu = 0; stu < ds.Tables[1].Rows.Count; stu++)
                        {
                            string subcode1 = ds.Tables[1].Rows[stu]["Roll_No"].ToString();
                            if (!hv1.ContainsKey(subcode1))
                            {
                                cot++;
                                if (FpSpread1.Sheets[0].RowCount >= cot)
                                {
                                    // int col = Convert.ToInt32(hat[subcode1].ToString());
                                    FpSpread1.Sheets[0].Cells[cot - 1, 0].Text = cot.ToString();
                                    hv1.Add(ds.Tables[1].Rows[stu]["Roll_No"].ToString(), ds.Tables[1].Rows[stu]["Roll_No"].ToString());
                                }
                            }
                        }
                        if (chtopper.Checked == true)
                        {
                            FpSpread1.Sheets[0].Columns[8].Visible = false;
                            if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Text == "")
                            {
                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].Visible = false;
                            }
                        }
                    }
                }
                if (rbt.Items[2].Selected == true && DropDownList1.SelectedItem.Text == "Both" && DropDownList2.SelectedItem.Text == "--Select--")
                {
                    if (chtopper.Checked != true)
                    {
                        if (chklscriteria.Items[33].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "TOTAL NO OF STUDENTS";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                            totstudent = Convert.ToInt32(FpSpread1.Sheets[0].RowCount - 1);
                            totstudent = sno;
                        }


                        Hashtable htt = new Hashtable();
                        int rcnt = 0;
                        int rcntv = 0;
                        binddate();
                        for (int stu = 0; stu < ds.Tables[1].Rows.Count; stu++)
                        {
                            int cnt = 0;

                            if (!htt.ContainsKey(ds.Tables[1].Rows[stu]["Roll_No"].ToString()))
                            {
                                if (chklscriteria.Items[15].Selected == true)
                                {
                                    if (rcntv == 0)
                                    {

                                        string dt = frdate;
                                        string[] dsplit = dt.Split(new Char[] { '/' });
                                        frdate = dsplit[0].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[2].ToString();
                                        int demfcal = int.Parse(dsplit[0].ToString());
                                        demfcal = demfcal * 12;
                                        cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                                        cal_from_date_tmp = demfcal + int.Parse(dsplit[1].ToString());

                                        string monthcal = cal_from_date.ToString();
                                        dt = todate;
                                        dsplit = dt.Split(new Char[] { '/' });
                                        todate = dsplit[0].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[2].ToString();
                                        int demtcal = int.Parse(dsplit[0].ToString());
                                        demtcal = demtcal * 12;
                                        cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
                                        cal_to_date_tmp = demtcal + int.Parse(dsplit[1].ToString());

                                        per_from_gendate = Convert.ToDateTime(frdate);
                                        per_to_gendate = Convert.ToDateTime(todate);

                                        //if(
                                        ht_sphr.Clear();
                                        string hrdetno = "";
                                        string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and sections='" + sec + "' and degree_code ='" + ds.Tables[1].Rows[stu]["degree_code"].ToString() + "' and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlsemester.SelectedValue.ToString() + " and date between '" + per_from_gendate.ToString() + "' and '" + per_to_gendate.ToString() + "'";
                                        ds_sphr = d2.select_method_wo_parameter(getsphr, "Text");
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


                                    hvtb.Clear();
                                    hvtb.Add("degree_code", ds.Tables[1].Rows[stu]["degree_code"].ToString());
                                    hvtb.Add("sem_ester", int.Parse(ddlsemester.SelectedValue.ToString()));
                                    DataSet dsg = new DataSet();
                                    dsg = d2.select_method("period_attnd_schedule", hvtb, "sp");
                                    if (dsg.Tables[0].Rows.Count != 0)
                                    {
                                        NoHrs = int.Parse(dsg.Tables[0].Rows[0]["PER DAY"].ToString());
                                        fnhrs = int.Parse(dsg.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                                        anhrs = int.Parse(dsg.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                                        minpresI = int.Parse(dsg.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                                        minpresII = int.Parse(dsg.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                                    }
                                    hvtb.Clear();
                                    hvtb.Add("colege_code", Session["collegecode"].ToString());

                                    ds8 = d2.select_method("ATT_MASTER_SETTING", hvtb, "sp");
                                    countds = ds8.Tables[0].Rows.Count;
                                    persentmonthcal(stu);
                                    //'----------------------------------------new start----------------

                                    per_con_hrs = per_workingdays1;

                                    if (per_con_hrs != 0 && per_per_hrs != 0)
                                    {
                                        per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / (per_con_hrs + tot_conduct_hr_spl_fals)) * 100);
                                    }

                                    if (per_tage_hrs > 100)
                                    {
                                        per_tage_hrs = 100;
                                    }

                                    double dum_tage_date = 0;
                                    double dum_tage_hrs = 0;



                                    dum_tage_date = Math.Round(per_tage_date, 2);
                                    dum_tage_hrs = Math.Round(per_tage_hrs, 2);

                                    if (chklscriteria.Items[15].Selected == true)
                                    {
                                        if (FpSpread1.Sheets[0].RowCount > rcntv)
                                        {
                                            if (Session["Hourwise"] == "1")
                                            {
                                                if (dum_tage_hrs != null)
                                                {
                                                    FpSpread1.Sheets[0].Cells[rcntv, attendence].Text = dum_tage_hrs.ToString();
                                                }
                                                FpSpread1.Sheets[0].Cells[rcntv, attendence].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[rcntv, attendence].Font.Size = FontUnit.Medium;
                                                //  FpSpread1.Sheets[0].Cells[rcntv, attendence].Font.Name = "";
                                            }
                                            else
                                            {
                                                if (dum_tage_date != null)
                                                {
                                                    FpSpread1.Sheets[0].Cells[rcntv, attendence].Text = dum_tage_date.ToString();
                                                }
                                                FpSpread1.Sheets[0].Cells[rcntv, attendence].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[rcntv, attendence].Font.Size = FontUnit.Medium;
                                            }
                                        }
                                    }
                                }
                                rcntv++;
                                foreach (var kvp in totalpercentage)
                                {
                                    string setval = kvp.Key.ToString();
                                    string setvalk = kvp.Value.ToString();
                                    cnt++;
                                    if (ds.Tables[1].Rows[stu]["Roll_No"].ToString() == setval)
                                    {
                                        if (chklscriteria.Items[0].Selected == true)
                                        {
                                            if (rbt.Items[0].Selected == true)
                                            {
                                                rcnt++;

                                                if (FpSpread1.Sheets[0].Cells[rcnt - 1, Resultcolumn].Text != "Fail")
                                                {
                                                    //if (!htt.ContainsKey(ds.Tables[1].Rows[stu]["marks_obtained"].ToString()))
                                                    //{
                                                    FpSpread1.Sheets[0].Cells[rcnt - 1, rankcount].Text = cnt.ToString();

                                                    //  htt.Add(setval, setvalk);
                                                }
                                                //}s
                                                //else
                                                //{
                                                //    FpSpread1.Sheets[0].Cells[rcnt - 1, rankcount].Text = htt[ds.Tables[1].Rows[stu]["marks_obtained"].ToString()].ToString();
                                                //}
                                                FpSpread1.Sheets[0].Cells[rcnt - 1, rankcount].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[rcnt - 1, rankcount].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[rcnt - 1, rankcount].Font.Bold = false;
                                                FpSpread1.Sheets[0].Cells[rcnt - 1, rankcount].Font.Size = FontUnit.Medium;
                                            }
                                            else if (rbt.Items[2].Selected == true)
                                            {
                                                //rcnt++;
                                                if (FpSpread1.Sheets[0].Cells[rcntv - 1, Resultcolumn].Text != "Fail")
                                                {
                                                    FpSpread1.Sheets[0].Cells[rcntv - 1, rankcount].Text = cnt.ToString();

                                                    //  htt.Add(setval, setvalk);
                                                }
                                                FpSpread1.Sheets[0].Cells[rcntv - 1, rankcount].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[rcntv - 1, rankcount].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[rcntv - 1, rankcount].Font.Bold = false;
                                                FpSpread1.Sheets[0].Cells[rcntv - 1, rankcount].Font.Size = FontUnit.Medium;
                                            }
                                        }
                                    }
                                }
                                htt.Add(ds.Tables[1].Rows[stu]["Roll_No"].ToString(), ds.Tables[1].Rows[stu]["Roll_No"].ToString());
                            }
                        }
                    }
                }

                if (chtopper.Checked != true)
                {
                    if (rbt.Items[2].Selected == true && DropDownList1.SelectedItem.Text == "Both" && DropDownList2.SelectedItem.Text == "--Select--")
                    {
                        if (chklscriteria.Items[33].Selected == true)
                        {
                            for (int stu = 0; stu < ds.Tables[1].Rows.Count; stu++)
                            {
                                string subcode1 = ds.Tables[1].Rows[stu]["Subject_Code"].ToString().Trim();
                                if (hat.Contains(subcode1))
                                {
                                    int col = Convert.ToInt32(hat[subcode1].ToString());
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = sno.ToString();
                                }
                            }
                        }
                        if (chklscriteria.Items[8].Selected == true || chklscriteria.Items[9].Selected == true || chklscriteria.Items[10].Selected == true || chklscriteria.Items[16].Selected == true)
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = "Pass";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = "Fail";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = "Total";
                        }
                    }

                }

                if (chklscriteria.Items[8].Selected == true)
                {
                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = " DAYS SCHOLAR TOTAL:";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = (failcnt1 + passcnt1).ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = failcnt1.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = passcnt1.ToString();

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (chklscriteria.Items[9].Selected == true)
                {
                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "HOSTLER TOTAL:";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = (failcnt + passcnt).ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = failcnt.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = passcnt.ToString();

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }

                if (chklscriteria.Items[10].Selected == true)
                {
                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "TAMIL:";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = t_pass_count.ToString(); //(t_pass_count + t_fail_count).ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = t_fail_count.ToString(); //t_pass_count.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = (t_pass_count + t_fail_count).ToString(); //t_fail_count.ToString();

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }

                if (chklscriteria.Items[11].Selected == true)
                {
                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "ENGLISH:";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = e_pass_count.ToString();// (e_pass_count + e_fail_count).ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = e_fail_count.ToString();// e_pass_count.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = (e_pass_count + e_fail_count).ToString();// e_fail_count.ToString();

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);//Modified by srinath 23/5/2014
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }

                if (chklscriteria.Items[13].Selected == true)
                {
                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "GIRLS:";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = girlpass.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = girl1fail.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = (girl1fail + girlpass).ToString();

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (chklscriteria.Items[14].Selected == true)
                {
                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "BOYS:";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = boypass.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = boy1fail.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = (boy1fail + boypass).ToString();

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                }
                if (rbt.Items[2].Selected == true && DropDownList1.SelectedItem.Text == "Both" && DropDownList2.SelectedItem.Text == "--Select--")
                {
                    DataSet ds4 = new DataSet();
                    if (chtopper.Checked != true)
                    {
                        int avg75abv = 0;
                        int avg60to74 = 0;
                        int avg50to59 = 0;
                        int avg30to49 = 0;
                        int avg20to29 = 0;
                        int avg19 = 0;
                        int avg50 = 0;
                        int avg50to60 = 0;
                        int avg65 = 0;
                        int avg60 = 0;
                        int avg80 = 0;
                        int nostupres = 0;
                        int nostuabs = 0;
                        int nopass = 0;
                        int nofail = 0;
                        int opasscounvl = 0;
                        int opasscounvl1 = 0;
                        int opasscounvl12 = 0;
                        int opasscounvl2 = 0;
                        int passpercent = 0;
                        int graderow = 0;
                        int exam = 0;
                        int subavg = 0;
                        int staffname = 0;
                        if (chklscriteria.Items[31].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            nostupres = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS PRESENT";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }
                        if (chklscriteria.Items[1].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            nostuabs = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS ABSENT";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }
                        //if (chklscriteria.Items[2].Selected == true)
                        //{
                        //    FpSpread1.Sheets[0].RowCount++;
                        //    nopass = FpSpread1.Sheets[0].RowCount - 1;
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS PASSED";
                        //    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                        //}
                        //if (chklscriteria.Items[3].Selected == true)
                        //{
                        //    FpSpread1.Sheets[0].RowCount++;
                        //    nofail = FpSpread1.Sheets[0].RowCount - 1;
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS FAILED";
                        //    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                        //}
                        if (chklscriteria.Items[2].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            opasscounvl = FpSpread1.Sheets[0].RowCount - 1;
                            string min_mark = (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1].Rows[0]["min_mark"].ToString() : "";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS PASSED " + min_mark + "% ";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.LightBlue;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }
                        if (chklscriteria.Items[3].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            opasscounvl2 = FpSpread1.Sheets[0].RowCount - 1;
                            string min_mark = (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1].Rows[0]["min_mark"].ToString() : "";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS FAILED " + min_mark + "% ";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }
                        if (chklscriteria.Items[7].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            opasscounvl1 = FpSpread1.Sheets[0].RowCount - 1;
                            string min_mark1 = (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1].Rows[0]["min_mark"].ToString() : "";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "PASS PERCENTAGE (FOR " + min_mark1 + "% " + ")";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.LightBlue;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }

                        if (txtoptiminpassmark.Text != "")
                        {
                            if (Convert.ToInt32(txtoptiminpassmark.Text) > 0)
                            {
                                if (chklscriteria.Items[2].Selected == true)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    opasscoun = FpSpread1.Sheets[0].RowCount - 1;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS PASSED FOR " + txtoptiminpassmark.Text + "%:";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.LightBlue;
                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#0CA6CA");

                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                }
                                if (chklscriteria.Items[3].Selected == true)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    ofailcount = FpSpread1.Sheets[0].RowCount - 1;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS FAILED FOR " + txtoptiminpassmark.Text + "%:";
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                }

                                if (chklscriteria.Items[7].Selected == true)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    opasscounvl12 = FpSpread1.Sheets[0].RowCount - 1;
                                    //string min_mark1 = ds.Tables[1].Rows[0]["min_mark"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "PASS PERCENTAGE (FOR " + txtoptiminpassmark.Text + "% " + ")";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.LightBlue;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                }
                            }
                        }
                        //else
                        //{
                        //    if (chklscriteria.Items[7].Selected == true)
                        //    {
                        //        FpSpread1.Sheets[0].RowCount++;
                        //        passpercent = FpSpread1.Sheets[0].RowCount - 1;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "PASS PERCENTAGE";
                        //        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                        //    }
                        //}
                        if (chklscriteria.Items[16].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            avg75abv = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Average >= 75";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }
                        if (chklscriteria.Items[17].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            avg60to74 = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "AVERAGE >= 60 and <=74";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }
                        if (chklscriteria.Items[18].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;

                            avg50to59 = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "AVERAGE >= 50 and <=59";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }
                        if (chklscriteria.Items[19].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            avg30to49 = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "AVERAGE >= 30 and <=49";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }
                        if (chklscriteria.Items[20].Selected == true)
                        {

                            FpSpread1.Sheets[0].RowCount++;
                            avg20to29 = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "AVERAGE >= 20 and <=29";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }
                        if (chklscriteria.Items[21].Selected == true)
                        {

                            FpSpread1.Sheets[0].RowCount++;
                            avg19 = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "AVERAGE<=19";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }
                        if (chklscriteria.Items[4].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            avg50 = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "AVG<50 MARKS";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }
                        if (chklscriteria.Items[5].Selected == true)
                        {

                            FpSpread1.Sheets[0].RowCount++;
                            avg50to60 = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "AVG 50 To 60 MARKS";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }
                        if (chklscriteria.Items[23].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            avg60 = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "AVG>60 MARKS";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }
                        if (chklscriteria.Items[6].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            avg65 = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "AVG>65 MARKS";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }

                        if (chklscriteria.Items[24].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            avg80 = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "AVG>80 MARKS";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }

                        if (chklscriteria.Items[29].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            subavg = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "SUBJECT AVERAGE";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }
                        if (chklscriteria.Items[12].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            exam = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Exam Date";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }
                        if (branchcnt == 1)
                        {
                            if (chklscriteria.Items[30].Selected == true)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                staffname = FpSpread1.Sheets[0].RowCount - 1;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "STAFF NAME";
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                            }
                        }
                        else
                        {
                            if (chklscriteria.Items[30].Selected == true)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                staffname = FpSpread1.Sheets[0].RowCount - 1;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "STAFF NAME";
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                FpSpread1.Sheets[0].Cells[staffname, 6].Text = "Staff Names are Showed When Only One Branch is Selected";
                                FpSpread1.Sheets[0].Cells[staffname, 6].ForeColor = Color.Red;
                                merRo = staffname;
                                //FpSpread1.Sheets[0].Cells[staffname, col]
                                FpSpread1.Sheets[0].SpanModel.Add(staffname, 6, 1, totsub - 6);
                            }
                        }

                        if ((Chgrade.Checked == true))
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            graderow = FpSpread1.Sheets[0].RowCount - 1;
                        }

                        FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
                        Hashtable hv = new Hashtable();
                        Hashtable HTV = new Hashtable();
                        for (int stu = 0; stu < ds.Tables[1].Rows.Count; stu++)
                        {
                            string subcode1 = ds.Tables[1].Rows[stu]["Subject_Code"].ToString().Trim();
                            if (hat.Contains(subcode1))
                            {
                                if (!HTV.ContainsKey(subcode1))
                                {
                                    int col = Convert.ToInt32(hat[subcode1].ToString());
                                    Hashtable ht = new Hashtable();
                                    HTV.Add(subcode1, subcode1);
                                    //ds.Tables[1].DefaultView.RowFilter = "marks_obtained >-3 and subject_code='" + subcode1 + "'";
                                    //dvpass = ds.Tables[1].DefaultView;
                                    //if (dvpass.Count > 0)
                                    //{
                                    //    passcnt = Convert.ToInt32(dvpass.Count.ToString());
                                    //}
                                    ht.Clear();
                                    ht.Add("subject_code", ds.Tables[1].Rows[stu]["subject_code"].ToString().Trim());
                                    ht.Add("min_marks", ds.Tables[1].Rows[stu]["min_mark"].ToString());
                                    ht.Add("criteria", ds.Tables[1].Rows[stu]["criteria_no"].ToString());
                                    ht.Add("degree", degreecode);
                                    string date = ds.Tables[1].Rows[stu]["exam_date"].ToString();
                                    string sgcode = ds.Tables[1].Rows[stu]["subject_code"].ToString().Trim();
                                    //string cv = "select sum(marks_obtained) as 'SUM' from result r,exam_type ex,subjectchooser su,subject s,registration rt,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained>=0 and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code  IN(" + degreecode + ") select count(distinct r.roll_no) as 'PASS_COUNT' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and (r.marks_obtained>='" + ds.Tables[1].Rows[stu]["min_mark"].ToString() + "' or r.marks_obtained='-3' or r.marks_obtained='-2')and r.marks_obtained<>'-1'  and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN(" + degreecode + ") select count(distinct r.roll_no) as 'FAIL_COUNT' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and (r.marks_obtained<'" + ds.Tables[1].Rows[stu]["min_mark"].ToString() + "' and r.marks_obtained<>'-3' and r.marks_obtained<>'-2' and r.marks_obtained<>'-1') and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN(" + degreecode + ") select max(marks_obtained) as 'MAX_MARK' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN(" + degreecode + ") select min(marks_obtained) as 'MIN_MARK' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  (marks_obtained>=0) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code  IN(" + degreecode + ") select count(distinct rt.roll_no) as 'AVG<50' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and marks_obtained between 0  and 49  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and  c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN(" + degreecode + ") select count(distinct rt.roll_no) as 'AVG_50to65' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained  Between 50 And 65 and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and  c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN(" + degreecode + ") select count(distinct rt.roll_no) as 'AVG>65' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained >65  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN(" + degreecode + ") select count(distinct rt.roll_no) as 'PRESENT_COUNT' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no  and r.marks_obtained<>'-1' and  r.exam_code=ex.exam_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0   and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code  IN (" + degreecode + ")select count(distinct r.roll_no) as 'ABSENT_COUNT' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  r.marks_obtained<0 and (r.marks_obtained<>'-2' and r.marks_obtained<>'-3' and   r.marks_obtained<>'-7' )and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and  r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code  IN (" + degreecode + ")    select distinct r.roll_no as 'ROLL_NO',rt.stud_name as 'STUD_NAME' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained='" + ds.Tables[1].Rows[stu]["max_mark"].ToString() + "'     and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN (" + degreecode + ") select count(distinct r.roll_no) as 'FAIL_COUNT_WITHOUT_AB' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and (marks_obtained< '" + ds.Tables[1].Rows[stu]["min_mark"].ToString() + "' and marks_obtained<>'-3' and marks_obtained<>'-2' and marks_obtained<>'-1' and    marks_obtained<>'-4' and marks_obtained<>'-5' and marks_obtained<>'-6' and marks_obtained<>'-7' and    marks_obtained<>'-8' and marks_obtained<>'-9' and marks_obtained<>'-10' and marks_obtained<>'-11' and    marks_obtained<>'-12' and marks_obtained<>'-13' and marks_obtained<>'-14' and marks_obtained<>'-15' and    marks_obtained<>'-16' and marks_obtained<>'-17') and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN (" + degreecode + ") select count(marks_obtained) as 'AVG>=75' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and marks_obtained >=75 and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "'  and rt.degree_code  IN (" + degreecode + ") select count(distinct rt.roll_no) as 'AVG60to74' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and   marks_obtained Between 60    And 74 and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN (" + degreecode + ") select count(distinct rt.roll_no) as 'AVG50to59' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and   marks_obtained Between 50    And 59    and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code in (" + degreecode + ") select count(distinct rt.roll_no) as 'AVG30to49' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and   marks_obtained Between 30    And 49    and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN ( " + degreecode + ") select count(distinct rt.roll_no) as 'AVG20to29' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained Between 20    And 29   and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN (" + degreecode + ") select count(distinct rt.roll_no) as 'AVG<=19' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and marks_obtained <=19  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code  IN (" + degreecode + ") select marks_obtained ,rt.roll_no from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained =    (select max(marks_obtained) as 'MAX_MARK' from result r,exam_type ex,subjectchooser su,registration rt where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code ) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "'  and rt.degree_code   IN (" + degreecode + ") select marks_obtained ,rt.roll_no from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained = (    select min(marks_obtained) as 'MIN_MARK' from result r,exam_type ex,subjectchooser su,registration rt where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and  (marks_obtained>=0) ) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "'  and rt.degree_code   IN (" + degreecode + ") select count(distinct rt.roll_no) as 'AVG>=60' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and marks_obtained >=60 and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "'  and rt.degree_code   IN (" + degreecode + ")select count(distinct rt.roll_no) as 'AVG>=80' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and marks_obtained >=80 and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "'  and rt.degree_code   IN (" + degreecode + ") ";
                                    //string cv = "select sum(marks_obtained) as 'SUM' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and re.marks_obtained>=0 and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "';select count(distinct re.roll_no) as 'PASS_COUNT' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no  and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained>=e.min_mark or re.marks_obtained=-2 or re.marks_obtained=-3);select count(distinct r.roll_no) as 'FAIL_COUNT' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and (r.marks_obtained<'" + ds.Tables[1].Rows[stu]["min_mark"].ToString() + "' and r.marks_obtained<>'-3' and r.marks_obtained<>'-2' and r.marks_obtained<>'-1') and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN(" + degreecode + ");select max(marks_obtained) as 'MAX_MARK' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN(" + degreecode + ");select min(marks_obtained) as 'MIN_MARK' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  (marks_obtained>=0) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code  IN(" + degreecode + ");select count(distinct r.roll_no) as 'AVG<50' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 0 and 49;select count(distinct r.roll_no) as 'AVG_50to65' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 50 and 65;select count(distinct r.roll_no) as 'AVG>65' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>65;select count(distinct r.roll_no) as 'PRESENT_COUNT' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3);select count(distinct re.roll_no) as 'ABSENT_COUNT' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained='-1';select distinct r.roll_no as 'ROLL_NO',rt.stud_name as 'STUD_NAME' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained='" + ds.Tables[1].Rows[stu]["max_mark"].ToString() + "'     and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN (" + degreecode + ");select count(distinct re.roll_no) as 'FAIL_COUNT_WITHOUT_AB' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained<e.min_mark and re.marks_obtained<>'-2' and re.marks_obtained<>'-3' and re.marks_obtained<>'-1' and re.marks_obtained<>'-4' and re.marks_obtained<>'-5' and re.marks_obtained<>'-6' and re.marks_obtained<>'-7' and re.marks_obtained<>'-8' and re.marks_obtained<>'-9' and re.marks_obtained<>'-10' and re.marks_obtained<>'-11' and re.marks_obtained<>'-12' and re.marks_obtained<>'-13' and re.marks_obtained<>'-14' and re.marks_obtained<>'-15' and re.marks_obtained<>'-16' and re.marks_obtained<>'-17');select count(marks_obtained) as 'AVG>=75' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>=75;select count(distinct r.roll_no) as 'AVG60to74' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 60 and 74;select count(distinct r.roll_no) as 'AVG50to59' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 50 and 59;select count(distinct r.roll_no) as 'AVG30to49' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 30 and 49;select count(distinct r.roll_no) as 'AVG20to29' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and  c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 20 and 29;select count(distinct r.roll_no) as 'AVG<=19' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and  c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 0 and 19;select marks_obtained ,rt.roll_no from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained =    (select max(marks_obtained) as 'MAX_MARK' from result r,exam_type ex,subjectchooser su,registration rt where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code ) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "'  and rt.degree_code   IN (" + degreecode + ");select marks_obtained ,rt.roll_no from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained = (    select min(marks_obtained) as 'MIN_MARK' from result r,exam_type ex,subjectchooser su,registration rt where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and  (marks_obtained>=0) ) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "'  and rt.degree_code   IN (" + degreecode + ");select count(distinct r.roll_no) as 'AVG>=60' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>=60;select count(distinct r.roll_no) as 'AVG>=80' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained >=80;";
                                    string cv = "";
                                    if (sec == "")
                                    {
                                        cv = "select sum(marks_obtained) as 'SUM' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and re.marks_obtained>=0 and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "';select count(distinct re.roll_no) as 'PASS_COUNT' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no  and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained>=e.min_mark or re.marks_obtained=-2 or re.marks_obtained=-3);select count(distinct r.roll_no) as 'FAIL_COUNT' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and (r.marks_obtained<ex.min_mark and r.marks_obtained<>'-3' and r.marks_obtained<>'-2' and r.marks_obtained<>'-1') and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN(" + degreecode + ") and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select max(marks_obtained) as 'MAX_MARK' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and r.exam_code=ex.exam_code and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN(" + degreecode + ") and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select min(marks_obtained) as 'MIN_MARK' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  (marks_obtained>=0) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code  IN(" + degreecode + ") and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select count(distinct r.roll_no) as 'AVG<50' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 0 and 49;select count(distinct r.roll_no) as 'AVG_50to65' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 50 and 65;select count(distinct r.roll_no) as 'AVG>65' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>65;select count(distinct r.roll_no) as 'PRESENT_COUNT' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where  c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3);select count(distinct re.roll_no) as 'ABSENT_COUNT' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where cc=0 and DelFlag=0 and Exam_Flag<>'debar' and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained='-1';select distinct r.roll_no as 'ROLL_NO',rt.stud_name as 'STUD_NAME' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained='100' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN (" + degreecode + ") and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select count(distinct re.roll_no) as 'FAIL_COUNT_WITHOUT_AB' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained<e.min_mark and re.marks_obtained<>'-2' and re.marks_obtained<>'-3' and re.marks_obtained<>'-1' and re.marks_obtained<>'-4' and re.marks_obtained<>'-5' and re.marks_obtained<>'-6' and re.marks_obtained<>'-7' and re.marks_obtained<>'-8' and re.marks_obtained<>'-9' and re.marks_obtained<>'-10' and re.marks_obtained<>'-11' and re.marks_obtained<>'-12' and re.marks_obtained<>'-13' and re.marks_obtained<>'-14' and re.marks_obtained<>'-15' and re.marks_obtained<>'-16' and re.marks_obtained<>'-17');select count(marks_obtained) as 'AVG>=75' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>=75;select count(distinct r.roll_no) as 'AVG60to74' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 60 and 74;select count(distinct r.roll_no) as 'AVG50to59' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 50 and 59;select count(distinct r.roll_no) as 'AVG30to49' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where  c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 30 and 49;select count(distinct r.roll_no) as 'AVG20to29' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and  c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 20 and 29;select count(distinct r.roll_no) as 'AVG<=19' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where  c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and  c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 0 and 19;select marks_obtained ,rt.roll_no from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained =    (select max(marks_obtained) as 'MAX_MARK' from result r,exam_type ex,subjectchooser su,registration rt where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code ) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN (" + degreecode + ") and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select marks_obtained ,rt.roll_no from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained = (    select min(marks_obtained) as 'MIN_MARK' from result r,exam_type ex,subjectchooser su,registration rt where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and  (marks_obtained>=0) ) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN (" + degreecode + ") and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select count(distinct r.roll_no) as 'AVG>=60' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>=60;select count(distinct r.roll_no) as 'AVG>=80' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained >=80;";
                                    }
                                    else
                                    {
                                        cv = "select sum(marks_obtained) as 'SUM' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.sections in('" + sec + "')  and re.marks_obtained>=0 and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "';select count(distinct re.roll_no) as 'PASS_COUNT' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no  and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained>=e.min_mark or re.marks_obtained=-2 or re.marks_obtained=-3);select count(distinct r.roll_no) as 'FAIL_COUNT' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and (r.marks_obtained<ex.min_mark and r.marks_obtained<>'-3' and r.marks_obtained<>'-2' and r.marks_obtained<>'-1') and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN(" + degreecode + ") and rt.sections in('" + sec + "') and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select max(marks_obtained) as 'MAX_MARK' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and r.exam_code=ex.exam_code and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN(" + degreecode + ") and rt.sections in('" + sec + "') and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select min(marks_obtained) as 'MIN_MARK' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  (marks_obtained>=0) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code  IN(" + degreecode + ") and rt.sections in('" + sec + "') and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select count(distinct r.roll_no) as 'AVG<50' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 0 and 49;select count(distinct r.roll_no) as 'AVG_50to65' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 50 and 65;select count(distinct r.roll_no) as 'AVG>65' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>65;select count(distinct r.roll_no) as 'PRESENT_COUNT' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where  c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.sections in('" + sec + "')  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3);select count(distinct re.roll_no) as 'ABSENT_COUNT' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where cc=0 and DelFlag=0 and Exam_Flag<>'debar' and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained='-1';select distinct r.roll_no as 'ROLL_NO',rt.stud_name as 'STUD_NAME' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained='100' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN (" + degreecode + ") and rt.sections in('" + sec + "') and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select count(distinct re.roll_no) as 'FAIL_COUNT_WITHOUT_AB' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained<e.min_mark and re.marks_obtained<>'-2' and re.marks_obtained<>'-3' and re.marks_obtained<>'-1' and re.marks_obtained<>'-4' and re.marks_obtained<>'-5' and re.marks_obtained<>'-6' and re.marks_obtained<>'-7' and re.marks_obtained<>'-8' and re.marks_obtained<>'-9' and re.marks_obtained<>'-10' and re.marks_obtained<>'-11' and re.marks_obtained<>'-12' and re.marks_obtained<>'-13' and re.marks_obtained<>'-14' and re.marks_obtained<>'-15' and re.marks_obtained<>'-16' and re.marks_obtained<>'-17');select count(marks_obtained) as 'AVG>=75' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>=75;select count(distinct r.roll_no) as 'AVG60to74' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 60 and 74;select count(distinct r.roll_no) as 'AVG50to59' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 50 and 59;select count(distinct r.roll_no) as 'AVG30to49' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where  c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 30 and 49;select count(distinct r.roll_no) as 'AVG20to29' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.sections in('" + sec + "') and  c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 20 and 29;select count(distinct r.roll_no) as 'AVG<=19' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where  c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.sections in('" + sec + "') and  c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 0 and 19;select marks_obtained ,rt.roll_no from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained =    (select max(marks_obtained) as 'MAX_MARK' from result r,exam_type ex,subjectchooser su,registration rt where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code ) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN (" + degreecode + ") and rt.sections in('" + sec + "') and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select marks_obtained ,rt.roll_no from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained = (    select min(marks_obtained) as 'MIN_MARK' from result r,exam_type ex,subjectchooser su,registration rt where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and  (marks_obtained>=0) ) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN (" + degreecode + ") and rt.sections in('" + sec + "') and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select count(distinct r.roll_no) as 'AVG>=60' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>=60;select count(distinct r.roll_no) as 'AVG>=80' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained >=80;";
                                    }
                                    ds4 = d2.select_method_wo_parameter(cv, "text");
                                    string[] dat = date.Split(' ');
                                    string[] datee = dat[0].Split('/');
                                    date = datee[1].ToString() + "/" + datee[0].ToString() + "/" + datee[2].ToString();
                                    if (chklscriteria.Items[31].Selected == true)
                                    {
                                        FpSpread1.Sheets[0].Cells[nostupres, col].Text = ds4.Tables[8].Rows[0]["PRESENT_COUNT"].ToString();
                                    }
                                    if (chklscriteria.Items[1].Selected == true)
                                    {
                                        FpSpread1.Sheets[0].Cells[nostuabs, col].Text = ds4.Tables[9].Rows[0]["ABSENT_COUNT"].ToString();
                                    }
                                    if (chklscriteria.Items[2].Selected == true)
                                    {
                                        FpSpread1.Sheets[0].Cells[opasscounvl, col].Text = ds4.Tables[1].Rows[0]["PASS_COUNT"].ToString();
                                        FpSpread1.Sheets[0].Cells[opasscounvl, col].BackColor = Color.LightBlue;
                                        //FpSpread1.Sheets[0].Cells[nopass, col].Text = ds4.Tables[1].Rows[0]["PASS_COUNT"].ToString();
                                    }
                                    if (chklscriteria.Items[3].Selected == true)
                                    {
                                        FpSpread1.Sheets[0].Cells[opasscounvl2, col].Text = ds4.Tables[11].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();
                                        //    FpSpread1.Sheets[0].Cells[nofail, col].Text = ds4.Tables[11].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();
                                    }
                                    if (chklscriteria.Items[7].Selected == true)
                                    {
                                        double final_pperc1 = 0;
                                        //calculate pass perc by present
                                        double absent_Count = 0;
                                        if (chkIncludeAbsent.Checked)
                                            Double.TryParse(Convert.ToString(ds4.Tables[9].Rows[0]["ABSENT_COUNT"]), out absent_Count);
                                        final_pperc1 = (Convert.ToDouble(ds4.Tables[1].Rows[0]["PASS_COUNT"]) / (Convert.ToDouble(ds4.Tables[8].Rows[0]["PRESENT_COUNT"]) + absent_Count)) * 100;
                                        final_pperc1 = Convert.ToDouble(Math.Round(final_pperc1, 2));
                                        FpSpread1.Sheets[0].Cells[opasscounvl1, col].Text = final_pperc1.ToString();
                                        FpSpread1.Sheets[0].Cells[opasscounvl1, col].BackColor = Color.LightBlue;
                                    }

                                    if (chklscriteria.Items[12].Selected == true)
                                    {
                                        FpSpread1.Sheets[0].Cells[exam, col].CellType = tt;
                                        FpSpread1.Sheets[0].Cells[exam, col].Text = date.ToString();
                                    }
                                    if (branchcnt == 1)
                                    {
                                        if (chklscriteria.Items[30].Selected == true)
                                        {
                                            DataSet dsstaff = new DataSet();
                                            String subno = null;
                                            StringBuilder spStaff = new StringBuilder();
                                            subno = FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Note.ToString();
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Tag //Subcode
                                            //subno = ds.Tables[1].Rows[stu]["subject_no"].ToString();
                                            string sqlstaffname = "";
                                            if (sec == "")
                                            {
                                                sqlstaffname = "select ss.staff_code,s.staff_name,ss.subject_no,su.subject_code,sections from staff_selector ss,staffmaster s,subject su where ss.staff_code =s.staff_code and ss.subject_no =su.subject_no and ss.subject_no in ('" + subno + "') order by sections";
                                            }
                                            else
                                            {
                                                sqlstaffname = "select ss.staff_code,s.staff_name,ss.subject_no,su.subject_code,sections from staff_selector ss,staffmaster s,subject su where ss.staff_code =s.staff_code and ss.subject_no =su.subject_no and ss.subject_no in ('" + subno + "') and sections in('" + sec + "')   order by sections";
                                            }
                                            dsstaff = d2.select_method_wo_parameter(sqlstaffname, "Text");
                                            if (dsstaff.Tables[0].Rows.Count > 0)
                                            {
                                                for (int ss = 0; ss < dsstaff.Tables[0].Rows.Count; ss++)
                                                {
                                                    string section = Convert.ToString(dsstaff.Tables[0].Rows[ss]["sections"]);
                                                    spStaff.Append(dsstaff.Tables[0].Rows[ss]["staff_name"].ToString() + ((section != "" && section != null) ? (" - " + section) : "") + " , ");
                                                    //FpSpread1.Sheets[0].Cells[staffname, col].Text += dsstaff.Tables[0].Rows[ss]["staff_name"].ToString() + ((section!="" && section!=null)?(" - " +section):"")+" , ";
                                                }
                                                FpSpread1.Sheets[0].Cells[staffname, col].Text = Convert.ToString(spStaff.Remove(spStaff.Length - 3, 3));
                                                //FpSpread1.Sheets[0].Cells[staffname, col].Text.Length-1;
                                            }
                                        }
                                    }


                                    double subjavg = 0;
                                    //if (hatsubmarkavg.Contains(ds.Tables[1].Rows[stu]["subject_code"].ToString().Trim().ToLower()))
                                    //{
                                    //    string getval = hatsubmarkavg[ds.Tables[1].Rows[stu]["subject_code"].ToString().Trim().ToLower()].ToString();
                                    //    if (getval.Trim() != "")
                                    //    {
                                    //        Double getavg = Convert.ToDouble(getval) / Convert.ToDouble(subsno);
                                    //        // getavg = Math.Round(getavg, 0, MidpointRounding.AwayFromZero);
                                    //        getavg = Math.Round(getavg, 2);
                                    //        subjavg = getavg;
                                    //    }
                                    //    //if (ds.Tables[0].Rows.Count > 0)
                                    //    //{
                                    //    //    subjavg = Convert.ToDouble(ds4.Tables[8].Rows[0]["SUM"].ToString()) / Convert.ToDouble(ds4.Tables[8].Rows[0]["PRESENT_COUNT"].ToString());
                                    //    //}
                                    //}

                                    if (ds4.Tables[0].Rows.Count > 0)
                                    {
                                        subjavg = Convert.ToDouble(ds4.Tables[0].Rows[0]["SUM"].ToString()) / Convert.ToDouble(ds4.Tables[8].Rows[0]["PRESENT_COUNT"].ToString());
                                    }

                                    if (chklscriteria.Items[29].Selected == true)
                                    {
                                        //FpSpread1.Sheets[0].Cells[subavg, col].Text = ds4.Tables[21].Rows[0]["SUBJECT AVERAGE"].ToString();
                                        FpSpread1.Sheets[0].Cells[subavg, col].Text = Convert.ToString(Math.Round(subjavg, 2));
                                    }

                                    if (chtopper.Checked != true)
                                    {
                                        if (txtoptiminpassmark.Text != "")
                                        {
                                            Hashtable hatt = new Hashtable();
                                            hatt.Clear();
                                            hatt.Add("subject_code", ds.Tables[1].Rows[stu]["subject_code"].ToString());
                                            hatt.Add("min_marks", txtoptiminpassmark.Text);
                                            hatt.Add("criteria", ds.Tables[1].Rows[stu]["criteria"].ToString());
                                            hatt.Add("degree", degreecode);
                                            //string cv1 = "select sum(marks_obtained) as 'SUM' from result r,exam_type ex,subjectchooser su,subject s,registration rt,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained>=0 and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code  IN(" + degreecode + ") select count(distinct r.roll_no) as 'PASS_COUNT' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and (r.marks_obtained>='" + txtoptiminpassmark.Text.ToString() + "' or r.marks_obtained='-3' or r.marks_obtained='-2')and r.marks_obtained<>'-1'  and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN(" + degreecode + ") select count(distinct r.roll_no) as 'FAIL_COUNT' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and (r.marks_obtained<'" + txtoptiminpassmark.Text.ToString() + "' and r.marks_obtained<>'-3' and r.marks_obtained<>'-2' and r.marks_obtained<>'-1') and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN(" + degreecode + ") select max(marks_obtained) as 'MAX_MARK' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN(" + degreecode + ") select min(marks_obtained) as 'MIN_MARK' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  (marks_obtained>=0) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code  IN(" + degreecode + ") select count(distinct rt.roll_no) as 'AVG<50' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and marks_obtained between 0  and 49  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and  c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN(" + degreecode + ") select count(distinct rt.roll_no) as 'AVG_50to65' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained  Between 50 And 65 and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and  c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN(" + degreecode + ") select count(distinct rt.roll_no) as 'AVG>65' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained >65  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN(" + degreecode + ") select count(distinct rt.roll_no) as 'PRESENT_COUNT' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no  and r.marks_obtained<>'-1' and  r.exam_code=ex.exam_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0   and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code  IN (" + degreecode + ")select count(distinct r.roll_no) as 'ABSENT_COUNT' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  r.marks_obtained<0 and (r.marks_obtained<>'-2' and r.marks_obtained<>'-3' and   r.marks_obtained<>'-7' )and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and  r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code  IN (" + degreecode + ")   select distinct r.roll_no as 'ROLL_NO',rt.stud_name as 'STUD_NAME' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained='" + ds.Tables[1].Rows[stu]["max_mark"].ToString() + "'     and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN (" + degreecode + ") select count(distinct r.roll_no) as 'FAIL_COUNT_WITHOUT_AB' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and (marks_obtained< '" + txtoptiminpassmark.Text.ToString() + "' and marks_obtained<>'-3' and marks_obtained<>'-2' and marks_obtained<>'-1' and    marks_obtained<>'-4' and marks_obtained<>'-5' and marks_obtained<>'-6' and marks_obtained<>'-7' and    marks_obtained<>'-8' and marks_obtained<>'-9' and marks_obtained<>'-10' and marks_obtained<>'-11' and    marks_obtained<>'-12' and marks_obtained<>'-13' and marks_obtained<>'-14' and marks_obtained<>'-15' and    marks_obtained<>'-16' and marks_obtained<>'-17') and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN (" + degreecode + ") select count(marks_obtained) as 'AVG>=75' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and marks_obtained >=75 and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "'  and rt.degree_code  IN (" + degreecode + ") select count(distinct rt.roll_no) as 'AVG60to74' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and   marks_obtained Between 60    And 74 and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN (" + degreecode + ") select count(distinct rt.roll_no) as 'AVG50to59' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and   marks_obtained Between 50    And 59    and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code in (" + degreecode + ") select count(distinct rt.roll_no) as 'AVG30to49' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and   marks_obtained Between 30    And 49    and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN ( " + degreecode + ") select count(distinct rt.roll_no) as 'AVG20to29' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained Between 20    And 29   and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN (" + degreecode + ") select count(distinct rt.roll_no) as 'AVG<=19' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and marks_obtained <=19  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code  IN (" + degreecode + ") select marks_obtained ,rt.roll_no from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained =    (select max(marks_obtained) as 'MAX_MARK' from result r,exam_type ex,subjectchooser su,registration rt where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code ) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "'  and rt.degree_code   IN (" + degreecode + ") select marks_obtained ,rt.roll_no from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained = (    select min(marks_obtained) as 'MIN_MARK' from result r,exam_type ex,subjectchooser su,registration rt where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and  (marks_obtained>=0) ) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "'  and rt.degree_code   IN (" + degreecode + ") select count(distinct rt.roll_no) as 'AVG>=60' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and marks_obtained >=60 and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "'  and rt.degree_code   IN (" + degreecode + ")select count(distinct rt.roll_no) as 'AVG>=80' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and marks_obtained >=80 and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "'  and rt.degree_code   IN (" + degreecode + ") ";
                                            //string cv1 = "select sum(marks_obtained) as 'SUM' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and re.marks_obtained>=0 and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "';select count(distinct re.roll_no) as 'PASS_COUNT' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no  and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained>='" + txtoptiminpassmark.Text.ToString() + "' or re.marks_obtained=-2 or re.marks_obtained=-3);select count(distinct r.roll_no) as 'FAIL_COUNT' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and (r.marks_obtained<'" + txtoptiminpassmark.Text.ToString() + "' and r.marks_obtained<>'-3' and r.marks_obtained<>'-2' and r.marks_obtained<>'-1') and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN(" + degreecode + ");select max(marks_obtained) as 'MAX_MARK' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN(" + degreecode + ");select min(marks_obtained) as 'MIN_MARK' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  (marks_obtained>=0) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code  IN(" + degreecode + ");select count(distinct r.roll_no) as 'AVG<50' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 0 and 49;select count(distinct r.roll_no) as 'AVG_50to65' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 50 and 65;select count(distinct r.roll_no) as 'AVG>65' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>65;select count(distinct r.roll_no) as 'PRESENT_COUNT' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3);select count(distinct re.roll_no) as 'ABSENT_COUNT' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained='-1';select distinct r.roll_no as 'ROLL_NO',rt.stud_name as 'STUD_NAME' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained='" + ds.Tables[1].Rows[stu]["max_mark"].ToString() + "'     and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code   IN (" + degreecode + "); select count(distinct re.roll_no) as 'FAIL_COUNT_WITHOUT_AB' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained<'" + txtoptiminpassmark.Text.ToString() + "' and re.marks_obtained<>'-2' and re.marks_obtained<>'-3' and re.marks_obtained<>'-1' and re.marks_obtained<>'-4' and re.marks_obtained<>'-5' and re.marks_obtained<>'-6' and re.marks_obtained<>'-7' and re.marks_obtained<>'-8' and re.marks_obtained<>'-9' and re.marks_obtained<>'-10' and re.marks_obtained<>'-11' and re.marks_obtained<>'-12' and re.marks_obtained<>'-13' and re.marks_obtained<>'-14' and re.marks_obtained<>'-15' and re.marks_obtained<>'-16' and re.marks_obtained<>'-17');select count(marks_obtained) as 'AVG>=75' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>=75;select count(distinct r.roll_no) as 'AVG60to74' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 60 and 74;select count(distinct r.roll_no) as 'AVG50to59' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 50 and 59;select count(distinct r.roll_no) as 'AVG30to49' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 30 and 49;select count(distinct r.roll_no) as 'AVG20to29' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and  c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 20 and 29;select count(distinct r.roll_no) as 'AVG<=19' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and  c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 0 and 19;select marks_obtained ,rt.roll_no from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained =    (select max(marks_obtained) as 'MAX_MARK' from result r,exam_type ex,subjectchooser su,registration rt where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code ) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "'  and rt.degree_code   IN (" + degreecode + ");select marks_obtained ,rt.roll_no from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained = (    select min(marks_obtained) as 'MIN_MARK' from result r,exam_type ex,subjectchooser su,registration rt where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and  (marks_obtained>=0) ) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "'  and rt.degree_code   IN (" + degreecode + ");select count(distinct r.roll_no) as 'AVG>=60' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>=60;select count(distinct r.roll_no) as 'AVG>=80' from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and sy.semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained >=80;";
                                            string cv1 = "";
                                            if (sec == "")
                                            {
                                                cv1 = "select sum(marks_obtained) as 'SUM' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and re.marks_obtained>=0 and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "';select count(distinct re.roll_no) as 'PASS_COUNT' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no  and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained>='" + txtoptiminpassmark.Text.ToString() + "' or re.marks_obtained=-2 or re.marks_obtained=-3);select count(distinct r.roll_no) as 'FAIL_COUNT' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and (r.marks_obtained<'" + txtoptiminpassmark.Text.ToString() + "' and r.marks_obtained<>'-3' and r.marks_obtained<>'-2' and r.marks_obtained<>'-1') and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN(" + degreecode + ") and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select max(marks_obtained) as 'MAX_MARK' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and r.exam_code=ex.exam_code and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN(" + degreecode + ") and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select min(marks_obtained) as 'MIN_MARK' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  (marks_obtained>=0) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code  IN(" + degreecode + ") and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select count(distinct r.roll_no) as 'AVG<50' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 0 and 49;select count(distinct r.roll_no) as 'AVG_50to65' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 50 and 65;select count(distinct r.roll_no) as 'AVG>65' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>65;select count(distinct r.roll_no) as 'PRESENT_COUNT' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where  c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3);select count(distinct re.roll_no) as 'ABSENT_COUNT' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where cc=0 and DelFlag=0 and Exam_Flag<>'debar' and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained='-1';select distinct r.roll_no as 'ROLL_NO',rt.stud_name as 'STUD_NAME' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained='100' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN (" + degreecode + ") and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select count(distinct re.roll_no) as 'FAIL_COUNT_WITHOUT_AB' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained<'" + txtoptiminpassmark.Text.ToString() + "' and re.marks_obtained<>'-2' and re.marks_obtained<>'-3' and re.marks_obtained<>'-1' and re.marks_obtained<>'-4' and re.marks_obtained<>'-5' and re.marks_obtained<>'-6' and re.marks_obtained<>'-7' and re.marks_obtained<>'-8' and re.marks_obtained<>'-9' and re.marks_obtained<>'-10' and re.marks_obtained<>'-11' and re.marks_obtained<>'-12' and re.marks_obtained<>'-13' and re.marks_obtained<>'-14' and re.marks_obtained<>'-15' and re.marks_obtained<>'-16' and re.marks_obtained<>'-17');select count(marks_obtained) as 'AVG>=75' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>=75;select count(distinct r.roll_no) as 'AVG60to74' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 60 and 74;select count(distinct r.roll_no) as 'AVG50to59' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ")  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 50 and 59;select count(distinct r.roll_no) as 'AVG30to49' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where  c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 30 and 49;select count(distinct r.roll_no) as 'AVG20to29' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and  c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 20 and 29;select count(distinct r.roll_no) as 'AVG<=19' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where  c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and  c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 0 and 19;select marks_obtained ,rt.roll_no from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained =    (select max(marks_obtained) as 'MAX_MARK' from result r,exam_type ex,subjectchooser su,registration rt where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code ) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN (" + degreecode + ") and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select marks_obtained ,rt.roll_no from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained = (    select min(marks_obtained) as 'MIN_MARK' from result r,exam_type ex,subjectchooser su,registration rt where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and  (marks_obtained>=0) ) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN (" + degreecode + ") and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select count(distinct r.roll_no) as 'AVG>=60' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>=60;select count(distinct r.roll_no) as 'AVG>=80' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained >=80;";
                                            }
                                            else
                                            {
                                                cv1 = "select sum(marks_obtained) as 'SUM' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.Sections in('" + sec + "') and re.marks_obtained>=0 and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "';select count(distinct re.roll_no) as 'PASS_COUNT' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no  and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.Sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained>='" + txtoptiminpassmark.Text.ToString() + "' or re.marks_obtained=-2 or re.marks_obtained=-3);select count(distinct r.roll_no) as 'FAIL_COUNT' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and (r.marks_obtained<'" + txtoptiminpassmark.Text.ToString() + "' and r.marks_obtained<>'-3' and r.marks_obtained<>'-2' and r.marks_obtained<>'-1') and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN(" + degreecode + ") and rt.Sections in('" + sec + "') and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select max(marks_obtained) as 'MAX_MARK' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and r.exam_code=ex.exam_code and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN(" + degreecode + ") and rt.Sections in('" + sec + "') and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select min(marks_obtained) as 'MIN_MARK' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  (marks_obtained>=0) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code  IN(" + degreecode + ") and rt.Sections in('" + sec + "') and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select count(distinct r.roll_no) as 'AVG<50' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.Sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 0 and 49;select count(distinct r.roll_no) as 'AVG_50to65' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.Sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 50 and 65;select count(distinct r.roll_no) as 'AVG>65' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.Sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>65;select count(distinct r.roll_no) as 'PRESENT_COUNT' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where  c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.Sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3);select count(distinct re.roll_no) as 'ABSENT_COUNT' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where cc=0 and DelFlag=0 and Exam_Flag<>'debar' and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.Sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained='-1';select distinct r.roll_no as 'ROLL_NO',rt.stud_name as 'STUD_NAME' from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained='100' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN (" + degreecode + ") and rt.Sections in('" + sec + "') and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select count(distinct re.roll_no) as 'FAIL_COUNT_WITHOUT_AB' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.Sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and (re.marks_obtained<'" + txtoptiminpassmark.Text.ToString() + "' and re.marks_obtained<>'-2' and re.marks_obtained<>'-3' and re.marks_obtained<>'-1' and re.marks_obtained<>'-4' and re.marks_obtained<>'-5' and re.marks_obtained<>'-6' and re.marks_obtained<>'-7' and re.marks_obtained<>'-8' and re.marks_obtained<>'-9' and re.marks_obtained<>'-10' and re.marks_obtained<>'-11' and re.marks_obtained<>'-12' and re.marks_obtained<>'-13' and re.marks_obtained<>'-14' and re.marks_obtained<>'-15' and re.marks_obtained<>'-16' and re.marks_obtained<>'-17');select count(marks_obtained) as 'AVG>=75' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.Sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>=75;select count(distinct r.roll_no) as 'AVG60to74' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.Sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 60 and 74;select count(distinct r.roll_no) as 'AVG50to59' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.Sections in('" + sec + "')  and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 50 and 59;select count(distinct r.roll_no) as 'AVG30to49' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where  c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.Sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 30 and 49;select count(distinct r.roll_no) as 'AVG20to29' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.Sections in('" + sec + "') and  c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 20 and 29;select count(distinct r.roll_no) as 'AVG<=19' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where  c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.Sections in('" + sec + "') and  c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained between 0 and 19;select marks_obtained ,rt.roll_no from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained =    (select max(marks_obtained) as 'MAX_MARK' from result r,exam_type ex,subjectchooser su,registration rt where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code ) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN (" + degreecode + ") and rt.Sections in('" + sec + "') and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select marks_obtained ,rt.roll_no from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and  marks_obtained = (    select min(marks_obtained) as 'MIN_MARK' from result r,exam_type ex,subjectchooser su,registration rt where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and  (marks_obtained>=0) ) and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and rt.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and c.criteria='" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "' and rt.degree_code IN (" + degreecode + ") and rt.Sections in('" + sec + "') and rt.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "';select count(distinct r.roll_no) as 'AVG>=60' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.Sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained>=60;select count(distinct r.roll_no) as 'AVG>=80' from Registration r,subject s,CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ds.Tables[1].Rows[stu]["subject_code"].ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Text.ToString() + "' and r.Current_Semester='" + ddlsemester.SelectedItem.Text.ToString() + "' and r.degree_code in(" + degreecode + ") and r.Sections in('" + sec + "') and c.criteria in('" + ds.Tables[1].Rows[stu]["criteria"].ToString() + "') and re.marks_obtained >=80;";
                                            }
                                            DataSet dsopt = d2.select_method_wo_parameter(cv1, "text");
                                            //DataSet dsopt = d2.select_method("Proc_All_Subject_Detailsoverallcam", hatt, "sp");
                                            if (dsopt.Tables[0].Rows.Count > 0)
                                            {
                                                if (chklscriteria.Items[2].Selected == true)
                                                {
                                                    FpSpread1.Sheets[0].Cells[opasscoun, col].Text = dsopt.Tables[1].Rows[0]["PASS_COUNT"].ToString();
                                                    FpSpread1.Sheets[0].Cells[opasscoun, col].BackColor = Color.LightBlue;
                                                }
                                                if (chklscriteria.Items[3].Selected == true)
                                                {
                                                    FpSpread1.Sheets[0].Cells[ofailcount, col].Text = dsopt.Tables[11].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();
                                                }
                                                // opasscounvl1 = opasscounvl1 + 1;
                                                if (chklscriteria.Items[7].Selected == true)
                                                {
                                                    double final_pperc = 0;
                                                    final_pperc = (Convert.ToDouble(dsopt.Tables[1].Rows[0]["PASS_COUNT"]) / Convert.ToDouble(dsopt.Tables[8].Rows[0]["PRESENT_COUNT"])) * 100;
                                                    final_pperc = Math.Round(final_pperc, 2);
                                                    FpSpread1.Sheets[0].Cells[opasscounvl12, col].Text = final_pperc.ToString();
                                                    FpSpread1.Sheets[0].Cells[opasscounvl12, col].BackColor = Color.LightBlue;
                                                }

                                            }
                                        }
                                        //else
                                        //{                                            
                                        //    double final_pperc = 0;
                                        //    //calculate pass perc by present
                                        //    final_pperc = (Convert.ToDouble(ds4.Tables[1].Rows[0]["PASS_COUNT"]) / Convert.ToDouble(ds4.Tables[8].Rows[0]["PRESENT_COUNT"])) * 100;
                                        //    final_pperc = Math.Round(final_pperc, 2);
                                        //    FpSpread1.Sheets[0].Cells[passpercent, col].Text = final_pperc.ToString();                                           
                                        //}
                                        if (chklscriteria.Items[16].Selected == true)
                                        {
                                            FpSpread1.Sheets[0].Cells[avg75abv, col].Text = ds4.Tables[12].Rows[0]["AVG>=75"].ToString();
                                            //    }
                                            //}

                                        }
                                        if (chklscriteria.Items[17].Selected == true)
                                        {
                                            FpSpread1.Sheets[0].Cells[avg60to74, col].Text = ds4.Tables[13].Rows[0]["AVG60to74"].ToString();

                                        }
                                        if (chklscriteria.Items[18].Selected == true)
                                        {
                                            //FpSpread1.Sheets[0].RowCount++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "AVERAGE >= 50 and <=59";
                                            //FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                                            //for (int stu = 0; stu < ds.Tables[0].Rows.Count; stu++)
                                            //{
                                            //    string subcode1 = ds.Tables[0].Rows[stu]["Subject_Code"].ToString();
                                            //    if (hat.Contains(subcode1))
                                            //    {
                                            //        int col = Convert.ToInt32(hat[subcode1].ToString());
                                            //        DataView dv = new DataView();
                                            //        ds.Tables[1].DefaultView.RowFilter = "(marks_obtained>=50 and marks_obtained<=59)  and subject_code='" + subcode1 + "'";
                                            //        dv = ds.Tables[1].DefaultView;
                                            FpSpread1.Sheets[0].Cells[avg50to59, col].Text = ds4.Tables[14].Rows[0]["AVG50to59"].ToString();
                                            //    }
                                            //}

                                        }
                                        if (chklscriteria.Items[19].Selected == true)
                                        {

                                            FpSpread1.Sheets[0].Cells[avg30to49, col].Text = ds4.Tables[15].Rows[0]["AVG30to49"].ToString();
                                            //    }
                                            //}
                                        }
                                        if (chklscriteria.Items[20].Selected == true)
                                        {


                                            FpSpread1.Sheets[0].Cells[avg20to29, col].Text = ds4.Tables[16].Rows[0]["AVG20to29"].ToString();
                                            //    }
                                            //}
                                        }
                                        if (chklscriteria.Items[21].Selected == true)
                                        {


                                            FpSpread1.Sheets[0].Cells[avg19, col].Text = ds4.Tables[17].Rows[0]["AVG<=19"].ToString();
                                            //    }
                                            //}
                                        }
                                        if (chklscriteria.Items[4].Selected == true)
                                        {

                                            FpSpread1.Sheets[0].Cells[avg50, col].Text = ds4.Tables[5].Rows[0]["AVG<50"].ToString();
                                            //    }
                                            //}
                                        }
                                        if (chklscriteria.Items[5].Selected == true)
                                        {


                                            FpSpread1.Sheets[0].Cells[avg50to60, col].Text = ds4.Tables[6].Rows[0]["AVG_50to65"].ToString();

                                        }
                                        if (chklscriteria.Items[6].Selected == true)
                                        {


                                            FpSpread1.Sheets[0].Cells[avg65, col].Text = ds4.Tables[7].Rows[0]["AVG>65"].ToString();
                                            //    }
                                            //}
                                        }
                                        if (chklscriteria.Items[23].Selected == true)
                                        {

                                            FpSpread1.Sheets[0].Cells[avg60, col].Text = ds4.Tables[20].Rows[0]["AVG>=60"].ToString();

                                        }
                                        if (chklscriteria.Items[24].Selected == true)
                                        {


                                            FpSpread1.Sheets[0].Cells[avg80, col].Text = ds4.Tables[21].Rows[0]["AVG>=80"].ToString();
                                            //    }
                                            //}
                                        }



                                    }
                                    if (Chgrade.Checked == true)
                                    {
                                        if (txtdegree.Text == "Degree (1)" && txtbranch.Text == "Branch (1)" && DropDownList2.SelectedItem.Text == "--Select--" && DropDownList1.SelectedItem.Text == "Both")
                                        {
                                            DataSet dss = new DataSet();
                                            DataView vb = new DataView();

                                            //  if (chklscriteria.Items[27].Selected == true)
                                            {
                                                int grsem = 0;
                                                string strgradesem = d2.GetFunction("select distinct Semester from grade_master where batch_year=" + ddlbatch.SelectedItem.Text + " and Degree_Code in (" + degreecode + ") and Semester='" + ddlsemester.SelectedItem.Text + "'");
                                                if (strgradesem.Trim() != "" && strgradesem != null)
                                                {
                                                    grsem = Convert.ToInt32(strgradesem);
                                                }
                                                string gerdaeste = "select g.Mark_Grade,g.Trange,(select  count(r.roll_no)  from Result r,Exam_type e where r.exam_code=e.exam_code and sections='" + sec + "' and subject_no='" + ds.Tables[1].Rows[stu]["subject_no"].ToString() + "' and criteria_no= '" + ds.Tables[1].Rows[stu]["criteria_no"].ToString() + "' and (r.marks_obtained/e.max_mark *100) between g.Frange and g.Trange) as studcoun from grade_master g where  batch_year=" + ddlbatch.SelectedItem.Text + " and Degree_Code in (" + degreecode + ") and Semester='" + grsem + "' and g.Credit_Points>0 order by g.Mark_Grade desc";
                                                dss.Reset();
                                                dss.Dispose();
                                                dss = d2.select_method_wo_parameter(gerdaeste, "Text");
                                                if (dss.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int dg = 0; dg < dss.Tables[0].Rows.Count; dg++)
                                                    {
                                                        //if (dg == 0)
                                                        //{
                                                        //    FpSpread1.Sheets[0].RowCount++;
                                                        //    graderow = FpSpread1.Sheets[0].RowCount - 1;
                                                        //}
                                                        if (!hv.ContainsKey("NO OF STUDENT SECURED" + " " + dss.Tables[0].Rows[dg]["Mark_Grade"].ToString() + " " + "GRADE"))
                                                        {
                                                            dss.Tables[0].DefaultView.RowFilter = "Mark_Grade='" + dss.Tables[0].Rows[dg]["Mark_Grade"].ToString() + "'";
                                                            vb = dss.Tables[0].DefaultView;
                                                            if (vb.Count > 0)
                                                            {
                                                                if (dg != 0)
                                                                {
                                                                    FpSpread1.Sheets[0].RowCount++;
                                                                }
                                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENT SECURED" + " " + dss.Tables[0].Rows[dg]["Mark_Grade"].ToString() + " " + "GRADE";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = dss.Tables[0].Rows[dg]["studcoun"].ToString();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                                hv.Add(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text, dss.Tables[0].Rows[dg]["studcoun"].ToString());

                                                            }
                                                        }
                                                        else
                                                        {
                                                            FpSpread1.Sheets[0].Cells[graderow + dg, col].Text = dss.Tables[0].Rows[dg]["studcoun"].ToString();
                                                            FpSpread1.Sheets[0].Cells[graderow + dg, col].HorizontalAlign = HorizontalAlign.Center;

                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (graderow == FpSpread1.Sheets[0].RowCount - 1)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[graderow, 0].Text = "The Grades Are Not Entered for the Selected Degree and Branch in the Grade Master!!! So , The Grade's Count Not Showed.";
                                                        FpSpread1.Sheets[0].Cells[graderow, 0].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[graderow, 0].ForeColor = Color.Red;
                                                        FpSpread1.Sheets[0].Cells[graderow, 0].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[graderow, 0].Font.Bold = true;
                                                        FpSpread1.Sheets[0].SpanModel.Add(graderow, 0, 1, hat.Count + 6);
                                                    }
                                                    //FpSpread1.Sheets[0].RowCount--;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            errmsg.Visible = true;
                                            errmsg.Text = "Please Select Only One Degree & Branch And Then Proceed Grade";
                                            FpSpread1.Visible = false;
                                            lblrptname.Visible = false;
                                            txtexcelname.Visible = false;
                                            btnExcel.Visible = false;
                                            BtnPrint.Visible = false;
                                            rptprint.Visible = false;
                                            return;
                                        }
                                    }
                                }
                            }
                        }

                        int fstu = 0;
                        if (chklscriteria.Items[25].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "NO OF ALL PASS";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                            fstu = 0;
                            if (hatfailcount.Contains(0))
                            {
                                fstu = Convert.ToInt32(hatfailcount[0]);
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = fstu.ToString();
                        }
                        if (chklscriteria.Items[26].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "OVERALL PASS PERCENTAGE";
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                            Double passpercen = Convert.ToDouble(fstu) / Convert.ToDouble(sno) * 100;
                            passpercen = Math.Round(passpercen, 2, MidpointRounding.AwayFromZero);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = passpercen.ToString();
                        }

                        if (chklscriteria.Items[32].Selected == true)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS TAKEN OD";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(odcnt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        }
                        if (chklscriteria.Items[27].Selected == true)
                        {
                            double total = 0;
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "NO OF FAILURES";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                            for (int subf = 1; subf <= subcou; subf++)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = subf.ToString() + " Subject Failure";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                fstu = 0;
                                if (hatfailcount.Contains(subf))
                                {
                                    fstu = Convert.ToInt32(hatfailcount[subf]);
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = fstu.ToString();
                                total += fstu;
                            }
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total Failures ";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(total);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        }

                    }
                    if (chtopper.Checked == true)
                    {
                        FpSpread1.Sheets[0].Columns[8].Visible = true;
                        //  FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count].Visible = false;
                    }
                }
                if (chtopper.Checked == true)
                {
                    //FpSpread1.SaveChanges();
                    FpSpread1.Sheets[0].Columns[8].Visible = true;
                    totaltoppers = totaltoppers.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                    rearrangebydecenting(totaltoppers, ref FpSpread1);
                    FpSpread1.SaveChanges();
                }
                if (Chgrade.Checked == true)
                {
                    if (DropDownList2.SelectedItem.Text != "--Select--")
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "No Grade Visible In Fail List";
                        FpSpread1.Visible = false;
                        lblrptname.Visible = false;
                        rptprint.Visible = false;
                        txtexcelname.Visible = false;
                        btnExcel.Visible = false;
                        BtnPrint.Visible = false;

                        if (txtdegree.Text != "Degree (1)" && txtbranch.Text != "Branch (1)")
                        {
                            errmsg.Visible = true;
                            errmsg.Text = "Please Select Only One Degree & Branch And Then Proceed Grade";
                            FpSpread1.Visible = false;
                            rptprint.Visible = false;
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnExcel.Visible = false;
                            BtnPrint.Visible = false;
                            return;
                        }
                        return;
                    }
                }
            }
            else
            {
                errmsg.Visible = true;
                rptprint.Visible = false;
                errmsg.Text = "No Test Alloted";
                FpSpread1.Visible = false;
                rptprint.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                BtnPrint.Visible = false;
            }
            FpSpread1.Width = 1000;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.SaveChanges();
            ds.Dispose();
            ds = null;
            hat.Clear();
            hat = null;
            hatfailcount.Clear();
            hatfailcount = null;
        }
        catch (Exception ex)
        {
            //errmsg.Visible = true;
            //errmsg.Text = ex.ToString();
        }
    }

    public double findgrade(string rol_no, int semval)
    {
        //try
        //{
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
        string sql = "select exam_system,first_year_nonsemester from ndegree where degree_code = " + degree_codeparticularstudent.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + "";
        DataSet ds1 = new DataSet();
        DataSet ds3 = new DataSet();
        Hashtable ht1 = new Hashtable();
        ds1 = d2.select_method_wo_parameter(sql, "text");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            examsys = 0;
            for (int jh = 0; jh < ds1.Tables[0].Rows.Count; jh++)
            {
                if (examsys == 0.0)
                {
                    //for (int i = 1; i <= sem; i++)
                    //{
                    gpa = 0.0;
                    grpoints = 0.0;
                    grcredit = 0.0;
                    gpa1 = 0.0;
                    grcredit1 = 0.0;
                    int examcode = getunivcode(Convert.ToInt32(degree_codeparticularstudent), sem, Convert.ToInt32(ddlbatch.SelectedValue.ToString()));
                    if (!ht1.ContainsValue(examcode.ToString()))
                    {
                        //string sql1 = "select mark_entry.*,maxtotal from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " +examcode+ " and ltrim(rtrim(type))='' and  Attempts =1 and roll_no='" +rol_no+ "'";
                        ht1.Add("exam_code", examcode.ToString());
                        ht1.Add("rol_no", rol_no.ToString());
                        ds3 = d2.select_method("Proc_Field_MarkEntry", ht1, "sp");


                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            for (int g = 0; g < ds3.Tables[0].Rows.Count; g++)
                            {
                                string mgrade = ds3.Tables[0].Rows[g]["grade"].ToString();
                                if (mgrade != "")
                                {
                                    // mgrade = "-";
                                    //   string sql2 = "select top 1 credit_points from grade_master where mark_grade= '" + mgrade + "' and degree_code= " + ddlBranch.SelectedValue.ToString() + "";
                                    ds3.Clear();
                                    ht1.Clear();
                                    ht1.Add("mgrade", mgrade.ToString());
                                    ht1.Add("degcode", degree_codeparticularstudent);
                                    ds3 = d2.select_method("Proc_Credit_Points", ht1, "sp");
                                    if (ds3.Tables[0].Rows.Count > 0)
                                    {
                                        for (int h = 0; h < ds3.Tables[0].Rows.Count; h++)
                                        {
                                            grpoints = Convert.ToDouble(ds3.Tables[0].Rows[h]["credit_points"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        grpoints = 0.0;
                                    }

                                }
                                int subno = Convert.ToInt32(ds3.Tables[0].Rows[g]["subject_no"].ToString());
                                string sql3 = "select isnull(credit_points,' ') subject from subject where subject_no= " + subno + "";
                                DataSet ds6 = new DataSet();
                                ds6 = d2.select_method_wo_parameter(sql3, "text");
                                if (ds6.Tables[0].Rows.Count > 0)
                                {

                                    for (int g1 = 0; g1 < ds6.Tables[0].Rows.Count; g1++)
                                    {
                                        grcredit = Convert.ToDouble(ds6.Tables[0].Rows[0]["subject"].ToString());
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

                        cgpa1 = gpacal2 / sem;
                    }
                    //  }

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
                        int examcode = getunivcode(Convert.ToInt32(degree_codeparticularstudent), j, Convert.ToInt32(ddlbatch.SelectedValue.ToString()));

                        //string sql1 = "select mark_entry.*,maxtotal from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " +examcode+ " and ltrim(rtrim(type))='' and  Attempts =1 and roll_no='" +rol_no+ "'";
                        ht1.Add("exam_code", examcode.ToString());
                        ht1.Add("rol_no", rol_no.ToString());
                        ds3 = d2.select_method("Proc_Field_MarkEntry", ht1, "sp");

                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            for (int g = 0; g < ds3.Tables[0].Rows.Count; g++)
                            {
                                string mgrade = ds3.Tables[0].Rows[g]["grade"].ToString();
                                if (mgrade == "")
                                {
                                    mgrade = "-";
                                    ds3.Clear();
                                    ht1.Clear();
                                    ht1.Add("mgrade", mgrade.ToString());
                                    ht1.Add("degcode", degree_codeparticularstudent);
                                    ds3 = d2.select_method("Proc_Credit_Points", ht1, "sp");


                                    if (ds3.Tables[0].Rows.Count > 0)
                                    {
                                        for (int h = 0; h < ds3.Tables[0].Rows.Count; h++)
                                        {
                                            grpoints = Convert.ToDouble(ds3.Tables[0].Rows[h]["credit_points"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        grpoints = 0.0;
                                    }

                                }
                                int subno = Convert.ToInt32(ds3.Tables[0].Rows[g]["subject_no"].ToString());
                                string sql3 = "select isnull(credit_points,' ') subject from subject where subject_no= " + subno + "";
                                DataSet ds6 = new DataSet();
                                ds6 = d2.select_method_wo_parameter(sql3, "text");
                                if (ds6.Tables[0].Rows.Count > 0)
                                {


                                    for (int g1 = 0; g1 < ds6.Tables[0].Rows.Count; g1++)
                                    {
                                        grcredit = Convert.ToDouble(ds6.Tables[0].Rows[0]["Subject"].ToString());
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
        //}
        //catch (Exception ex)
        //{
        //}
    }

    public int getunivcode(int degreecode, int sem, int batch)
    {

        int x = -1;
        string sqlcode = "Select Exam_Code from Exam_Details where Degree_Code = " + degreecode + " and Current_Semester = " + sem + " and Batch_Year = " + batch + "";
        DataSet dds = new DataSet();
        dds = d2.select_method_wo_parameter(sqlcode, "text");
        if (dds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dds.Tables[0].Rows.Count; i++)
            {

                x = Convert.ToInt32(dds.Tables[0].Rows[i]["exam_code"].ToString());
            }
        }

        return x;

    }

    public string filteration()
    {
        string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");

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

    public void rearrangebydecenting(Dictionary<string, double> totalpercentage, ref FarPoint.Web.Spread.FpSpread fp)
    {
        DataView dvStud = new DataView();
        int cnt = 0;

        FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

        FpSpread1.Sheets[0].RowCount = 0;
        if (ds.Tables[1].Rows.Count > 0)
        {
            totaltoppers = totaltoppers.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            int rank = 1;
            string prevavg = "";
            foreach (KeyValuePair<string, double> kvp in totaltoppers)
            {
                string setval = kvp.Key.ToString();
                string setvalk = kvp.Value.ToString();
                ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + setval + "'";
                dvStud = ds.Tables[1].DefaultView;
                if (dvStud.Count > 0)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cnt + 1);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = false;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = false;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = false;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = false;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = false;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = false;
                    string degdetails = Convert.ToString(dvStud[0]["Degreedetails"]);
                    string[] degsplit = degdetails.Split('-');
                    string degree = "";
                    if (degsplit.Length == 3)
                    {
                        if (degsplit[0] != "" && degsplit[0] != null)
                        {
                            degree = degsplit[0].ToString();
                        }
                        if (degsplit[1] != "" && degsplit[1] != null)
                        {
                            degree += " - " + degsplit[1].ToString();
                        }
                        if (degsplit[2] != "" && degsplit[2] != null)
                        {
                            degree += " - " + degsplit[2].ToString();
                        }
                    }
                    else if (degsplit.Length == 2)
                    {
                        if (degsplit[0] != "" && degsplit[0] != null)
                        {
                            degree = degsplit[0].ToString();
                        }
                        if (degsplit[1] != "" && degsplit[1] != null)
                        {
                            degree += " - " + degsplit[1].ToString();
                        }
                    }
                    else
                    {
                        if (degsplit[0] != "" && degsplit[0] != null)
                        {
                            degree = degsplit[0].ToString();
                        }
                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = degree.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dvStud[0]["Roll_No"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dvStud[0]["Reg_no"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = dvStud[0]["Stud_Type"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = dvStud[0]["Stud_Name"].ToString();
                    if (totaltoppers.ContainsKey(setval))
                    {
                        double value = totaltoppers[setval];

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text = Convert.ToString(value);
                    }
                    if (totaltopperstot.ContainsKey(setval))
                    {

                        double value = totaltopperstot[setval];
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalmarkclum].Text = Convert.ToString(value);
                    }
                    if (prevavg != "" && prevavg != Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text))
                    {
                        rank++;
                    }
                    prevavg = Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Percentageclum].Text);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cnt + 1);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].Text = "PASS";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, Resultcolumn].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, rankcount].Text = Convert.ToString(rank);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, rankcount].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, rankcount].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, rankcount].Font.Bold = false;

                }
                cnt++;
            }
        }
    }

}