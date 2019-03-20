using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;

public partial class CAT : System.Web.UI.Page
{

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection condegree = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection rankcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection myconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection newconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection funconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection readconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection rdnewconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection cona = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection cona1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection bind_con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_result = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection Totcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection Totcon2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection Totcon3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection rcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection Totcon4 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    SqlCommand cmd;
    SqlCommand cmd1a;
    SqlCommand cmd3a;
    SqlCommand cmd4a;

    Hashtable hat = new Hashtable();
    Hashtable htpass = new Hashtable();
    Hashtable htfail = new Hashtable();
    Hashtable htabsent = new Hashtable();
    Hashtable htpresent = new Hashtable();
    Hashtable htpassperc = new Hashtable();
    Hashtable htclsavg = new Hashtable();
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    static Hashtable ht_sphr = new Hashtable();

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
    DataSet ds_sphr = new DataSet();
    DataSet dsprint = new DataSet();

    DAccess2 d2 = new DAccess2();
    DAccess2 dacces2 = new DAccess2();

    string markglag = string.Empty;
    string rol_no = string.Empty;
    string courseid = string.Empty;
    string atten = string.Empty;
    string Master1 = string.Empty;
    string regularflag = string.Empty;
    string genderflag = string.Empty;
    string strdayflag = string.Empty;
    string fromdate = string.Empty;
    string todate = string.Empty;
    string str_day = string.Empty;
    string Atmonth = string.Empty;
    string Atyear = string.Empty;
    string roll = string.Empty;
    string dateformat1 = string.Empty;
    string dateformat2 = string.Empty;
    string dateconcat = string.Empty;
    string date1concat = string.Empty;
    string strorder = string.Empty;
    string strregorder = string.Empty;
    string frdate, todate1;
    string diff_date;
    string value, date;
    string tempvalue = "-1";
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string footer_text = string.Empty;
    string group_user = string.Empty;
    string collnamenew1 = string.Empty;
    string address1 = string.Empty;
    string address2 = string.Empty;
    string address = string.Empty;
    string Phoneno = string.Empty;
    string Faxno = string.Empty;
    string phnfax = string.Empty;
    string district = string.Empty;
    string email = string.Empty;
    string website = string.Empty;
    string form_heading_name = string.Empty;
    string batch_degree_branch = string.Empty;
    string new_header_string = string.Empty;
    string halforfull = "", mng = "", evng = "", holiday_sched_details = string.Empty;
    string value_holi_status = string.Empty;
    string split_holiday_status_1 = "", split_holiday_status_2 = string.Empty;
    string tempfromdate = string.Empty;
    string tempdegreesem = string.Empty;
    string chkdegreesem = string.Empty;
    string tempdegreesempresent = string.Empty;
    static string grouporusercode = string.Empty;

    int strdate = 0;
    int subno = 0;    
    int stucount;
    int categrycount = 0;
    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    int next = 0;
    int minpresII = 0;
    int i, rows_count;
    int ObtValue = -1;
    int cal_from_date;
    int cal_to_date;
    int moncount;
    int student = 0;
    int abs = 0, att = 0;
    int dum_diff_date, unmark;
    int per_dum_unmark, cum_dum_unmark, dum_unmark;
    int tot_per_hrs, per_tot_per_hrs, cum_per_tot_per, tot_wok_hrs;
    int countds = 0;
    int child_sub_count = 0;
    int final_print_col_cnt = 0;
    int temp_count = 0;
    int split_col_for_footer = 0;
    int footer_balanc_col = 0;
    int footer_count = 0;
    int totalcount = 0;
    int percentcount = 0;
    int resultcount = 0;
    int subjectcount = 0;
    int right_logo_clmn = 0;
    int acol = 0;
    int tempcallfromdate = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0;
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    int notconsider_value = 0;

    bool IsFlag = false;
    bool IsSetFlag = false;
    bool fg = false;
    bool chk_final_clm_flag = false;
    bool splhr_flag = false;
    bool datechk = false;
    static bool PrintMaster = false;

    DateTime per_from_date;
    DateTime per_to_date;
    DateTime dumm_from_date;    
    TimeSpan ts;   
    
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    double halfday = 0;
    double per_perhrs, per_abshrs, cum_perhrs, cum_abshrs;
    double per_ondu, per_leave, per_hhday, cum_ondu, cum_leave, cum_hhday;
    double dif_date = 0;
    double dif_date1 = 0;    
    double workingdays = 0;
    double per_workingdays = 0;
    double cum_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double cum_present_date, cum_ondu_date, cum_leave_date, cum_absent_date;
    double per_tage_date, cum_tage_date, per_tage_hrs, cum_tage_hrs;
    double cum_tot_point, per_holidate, cum_per_holidate;    
    double per_con_hrs, cum_con_hrs;
    double njhr, njdate, per_njhr, per_njdate, cum_njhr, cum_njdate;
    double per_per_hrs, cum_per_perhrs;
    double tot_ondu, per_tot_ondu, cum_tot_ondu;
    double tot_ml, per_tot_ml;
    double conduct_hour_new = 0;
    double spl_tot_condut = 0;

    string[] new_header_string_split;
    string[] split_holiday_status = new string[1000];

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
            txtdropdownlist.Attributes.Add("Readonly", "Readonly");
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            lblerr.Visible = false;
            if (!IsPostBack)
            {
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                btnExcel.Visible = false;
                Button1.Visible = false;

                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                chkIncludeAbsent.Checked = false;

                string dt1 = DateTime.Today.ToShortDateString();
                string[] dsplit = dt1.Split(new Char[] { '/' });
                dateconcat = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
                txtFromDate.Text = dateconcat.ToString();
                string dt2 = DateTime.Today.ToShortDateString();
                string[] dt2split = dt2.Split(new Char[] { '/' });
                date1concat = dt2split[1].ToString() + "/" + dt2split[0].ToString() + "/" + dt2split[2].ToString();
                txtToDate.Text = date1concat.ToString();

                RadioHeader.Visible = false;
                Radiowithoutheader.Visible = false;
                FpEntry.Visible = false;
                FpEntry.Sheets[0].SheetName = "  ";
                FpEntry.Sheets[0].AutoPostBack = true;
                FpEntry.Enabled = false;
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = FontUnit.Medium;
                style.Font.Bold = true;
                style.Font.Name = "Book Antiqua";
                FpEntry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpEntry.Sheets[0].AllowTableCorner = true;
                FarPoint.Web.Spread.SheetView svsort = new FarPoint.Web.Spread.SheetView();
                svsort = FpEntry.ActiveSheetView;
                svsort.AllowSort = true;
                FpEntry.CommandBar.Visible = true;
                FpEntry.Sheets[0].ColumnCount = 6;
                FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                FpEntry.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;
                FpEntry.Sheets[0].SheetCornerSpanModel.Add(0, 0, 2, 1);
                FpEntry.Sheets[0].SheetCornerStyle.HorizontalAlign = HorizontalAlign.Left;
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
                FpEntry.Sheets[0].Columns[1].Width = 100;
                FpEntry.Sheets[0].Columns[0].Width = 50;
                FpEntry.Sheets[0].Columns[2].Width = 150;
                FpEntry.Sheets[0].Columns[3].Width = 150;
                FpEntry.Pager.PageCount = 5;
                FpEntry.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                setcon.Close();
                setcon.Open();
                SqlDataReader mtrdr;
                SqlCommand mtcmd = new SqlCommand(Master1, setcon);
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
                        string[] spl_pageload_val = get_pageload_value.Split(',');
                        string[] spl_load_val = spl_pageload_val[5].Split('$');

                        bindbatch();
                        ddlBatch.SelectedIndex = Convert.ToInt32(spl_pageload_val[0].ToString());

                        binddegree();
                        ddlDegree.SelectedIndex = Convert.ToInt32(spl_pageload_val[1].ToString());
                        if (ddlDegree.Text != "")
                        {

                            bindbranch();
                            ddlBranch.SelectedIndex = Convert.ToInt32(spl_pageload_val[2].ToString());

                            bindsem();
                            ddlSemYr.SelectedIndex = Convert.ToInt32(spl_pageload_val[3].ToString());
                            //bind section
                            bindsec();
                            ddlSec.SelectedIndex = Convert.ToInt32(spl_pageload_val[4].ToString());
                            //bing test
                            GetTest();
                            ddlTest.SelectedIndex = Convert.ToInt32(spl_load_val[0].ToString());
                            lblnorec.Visible = false;
                            string[] spl_criteria_val = spl_load_val[1].Split('-');
                            if (spl_criteria_val.GetUpperBound(0) > 0)
                            {
                                for (int crt = 0; crt < spl_criteria_val.GetUpperBound(0) + 1; crt++)
                                {
                                    chklist.Items[Convert.ToInt32(spl_criteria_val[crt])].Selected = true;
                                }
                            }
                            txtFromDate.Text = spl_load_val[2].ToString();
                            txtToDate.Text = spl_load_val[3].ToString();
                            btnGo_Click(sender, e);
                            func_Print_Master_Setting();
                            func_header();
                            FpEntry.Visible = true;
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
                    //'----------------------- to bind the batch_year 
                    bindbatch();
                    //'--------------------------------- to bind the course
                    binddegree();
                    if (ddlDegree.Text != "")
                    {
                        //'----------------------------------------------------------- to bind the branch
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
        catch (Exception ex) { }
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

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
        Control cntUpdateBtn = FpEntry.FindControl("Update");
        Control cntCancelBtn = FpEntry.FindControl("Cancel");
        Control cntCopyBtn = FpEntry.FindControl("Copy");
        Control cntCutBtn = FpEntry.FindControl("Clear");
        Control cntPasteBtn = FpEntry.FindControl("Paste");
        Control cntPageNextBtn = FpEntry.FindControl("Next");
        Control cntPagePreviousBtn = FpEntry.FindControl("Prev");
        //   Control cntPagePrintBtn = FpEntry.FindControl("Print");
        //  Control cntPrintPDFBtn = FpEntry.FindControl("PrintPDF");
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
            tc = (TableCell)cntPageNextBtn.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntPagePreviousBtn.Parent;
            tr.Cells.Remove(tc);
            //     tc = (TableCell)cntPagePrintBtn.Parent;
            //   tr.Cells.Remove(tc);
            //    tc = (TableCell)cntPrintPDFBtn.Parent;
            //  tr.Cells.Remove(tc);
        }
        base.Render(writer);
    }

    public void GetTest()
    {
        try
        {
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
        //Buttontotal.Visible = false;
        //lblrecord.Visible = false;
        //DropDownListpage.Visible = false;
        //TextBoxother.Visible = false;
        //lblpage.Visible = false;
        //TextBoxpage.Visible = false;
        FpEntry.Visible = false;
        LabelE.Visible = false;
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 28/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        ddlBranch.Items.Clear();
        con.Open();
        string course_id = ddlDegree.SelectedValue.ToString();
        collegecode = Session["collegecode"].ToString();
        usercode = Session["usercode"].ToString();
        bindbranch();
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        lblpages.Visible = false;
        ddlpage.Visible = false;
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Buttontotal.Visible = false;
        //lblrecord.Visible = false;
        //DropDownListpage.Visible = false;
        //TextBoxother.Visible = false;
        //lblpage.Visible = false;
        //TextBoxpage.Visible = false;
        FpEntry.Visible = false;
        //Label6.Visible = false;
        LabelE.Visible = false;
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 28/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        lblpages.Visible = false;
        ddlpage.Visible = false;
        if (!Page.IsPostBack == false)
        {
            ddlSemYr.Items.Clear();
        }
        try
        {
            if (ddlBranch.SelectedIndex == 0)
            {
                bindsem();
                GetTest();
            }
            if ((ddlBranch.SelectedIndex != 0) && (ddlBranch.SelectedIndex > 0))
            {
                //  Get_Semester();
                bindsem();
                GetTest();
            }
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
        // ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));
        SqlDataReader dr_sec;
        dr_sec = cmd.ExecuteReader();
        dr_sec.Read();
        if (dr_sec.HasRows == true)
        {
            if (dr_sec["sections"].ToString() == "")
            {
                ddlSec.Enabled = false;
            }
            else
            {
                ddlSec.Enabled = true;
                //  lblEsection.Visible = true;
            }
        }
        else
        {
            ddlSec.Enabled = false;
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
        //--------------------------
        DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());
        //  ddlSemYr.Items.Insert(0, new ListItem("--Select--", "-1"));
        if (ds.Tables[0].Rows.Count > 0)
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
    
    public void bindsem()
    {
        try
        {
            //--------------------semester load
            ddlSemYr.Items.Clear();
            bool first_year;
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
    
    public void func_header()
    {
        //collnamenew1 = string.Empty;
        //address1 = string.Empty;
        //address2 = string.Empty;
        //address = string.Empty;
        //Phoneno = string.Empty;
        //Faxno = string.Empty;
        //phnfax = string.Empty;
        //district = string.Empty;
        //email = string.Empty;
        ////'----------for header
        //if (dsprint.Tables[0].Rows.Count > 0)
        //{
        //    if (dsprint.Tables[0].Rows[0]["college_name"].ToString() != string.Empty)
        //    {
        //        collnamenew1 = dsprint.Tables[0].Rows[0]["college_name"].ToString();
        //    }
        //    if (dsprint.Tables[0].Rows[0]["address1"].ToString() != "")
        //    {
        //        address1 = dsprint.Tables[0].Rows[0]["address1"].ToString();
        //        address = address1;
        //    }
        //    if (dsprint.Tables[0].Rows[0]["address2"].ToString() != "")
        //    {
        //        address2 = dsprint.Tables[0].Rows[0]["address2"].ToString();
        //        address = address1 + "-" + address2;
        //    }
        //    if (dsprint.Tables[0].Rows[0]["address3"].ToString() != "")
        //    {
        //        district = dsprint.Tables[0].Rows[0]["address3"].ToString();
        //        address = address1 + "-" + address2 + "-" + district;
        //    }
        //    if (dsprint.Tables[0].Rows[0]["phoneno"].ToString() != "")
        //    {
        //        Phoneno = dsprint.Tables[0].Rows[0]["phoneno"].ToString();
        //        phnfax = "Phone :" + " " + Phoneno;
        //    }
        //    if (dsprint.Tables[0].Rows[0]["faxno"].ToString() != "")
        //    {
        //        Faxno = dsprint.Tables[0].Rows[0]["faxno"].ToString();
        //        phnfax = phnfax + "Fax  :" + " " + Faxno;
        //    }
        //    if ((dsprint.Tables[0].Rows[0]["email"].ToString() != ""))
        //    {
        //        email = "E-Mail:" + dsprint.Tables[0].Rows[0]["email"].ToString();
        //    }
        //    if (dsprint.Tables[0].Rows[0]["website"].ToString() != "")
        //    {
        //        email = email + " " + "Web Site:" + dsprint.Tables[0].Rows[0]["website"].ToString();
        //    }
        //    if (dsprint.Tables[0].Rows[0]["form_heading_name"].ToString() != "")
        //    {
        //        form_heading_name = dsprint.Tables[0].Rows[0]["form_heading_name"].ToString();
        //    }
        //    if (dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString() != "")
        //    {
        //        batch_degree_branch = dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString();
        //    }
        //    //-to set the left logo
        //    if (final_print_col_cnt > 1)
        //    {
        //        for (int hdr_col = 0; hdr_col < FpEntry.Sheets[0].ColumnCount; hdr_col++)
        //        {
        //            if (final_print_col_cnt < FpEntry.Sheets[0].ColumnCount)
        //            {
        //                if (FpEntry.Sheets[0].Columns[hdr_col].Visible == true)
        //                {
        //                    MyImg mi3 = new MyImg();
        //                    mi3.ImageUrl = "Handler/Handler2.ashx?";
        //                    FpEntry.Sheets[0].ColumnHeader.Cells[0, hdr_col].CellType = mi3;
        //                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, hdr_col, 8, 1);
        //                    FpEntry.Sheets[0].ColumnHeader.Cells[0, hdr_col].Border.BorderColorBottom = Color.White;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}
        //FpEntry.Sheets[0].ColumnHeader.Cells[0, 3].Text = collnamenew1;
        //FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Text = address;
        //FpEntry.Sheets[0].ColumnHeader.Cells[2, 3].Text = phnfax;
        //FpEntry.Sheets[0].ColumnHeader.Cells[3, 3].Text = email;
        //FpEntry.Sheets[0].ColumnHeader.Cells[4, 3].Text = form_heading_name;
        //FpEntry.Sheets[0].ColumnHeader.Cells[5, 3].Text = batch_degree_branch;
        //if ((final_print_col_cnt != 0) && (final_print_col_cnt > 4))
        //{
        //    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, final_print_col_cnt - 4);
        //    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 3, 1, final_print_col_cnt - 4);
        //    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(2, 3, 1, final_print_col_cnt - 4);
        //    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(3, 3, 1, final_print_col_cnt - 4);
        //    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(4, 3, 1, final_print_col_cnt - 4);
        //    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(5, 3, 1, final_print_col_cnt - 4);
        //    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(6, 3, 1, final_print_col_cnt - 4);
        //}
        //for (int hdr_col = 0; hdr_col < FpEntry.Sheets[0].ColumnCount; hdr_col++)
        //{
        //    if (final_print_col_cnt == 1)
        //    {
        //        if (FpEntry.Sheets[0].Columns[hdr_col].Visible == true)
        //        {
        //            FpEntry.Sheets[0].ColumnHeader.Cells[0, hdr_col].Text = collnamenew1;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[1, hdr_col].Text = address;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[2, hdr_col].Text = phnfax;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[3, hdr_col].Text = email;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[4, hdr_col].Text = form_heading_name;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[5, hdr_col].Text = batch_degree_branch;
        //            break;
        //        }
        //    }
        //    else if (final_print_col_cnt < FpEntry.Sheets[0].ColumnCount)
        //    {
        //        if (hdr_col != 0)
        //        {
        //            if (FpEntry.Sheets[0].Columns[hdr_col].Visible == true)
        //            {
        //                FpEntry.Sheets[0].ColumnHeader.Cells[0, hdr_col].Text = collnamenew1;
        //                FpEntry.Sheets[0].ColumnHeader.Cells[1, hdr_col].Text = address;
        //                FpEntry.Sheets[0].ColumnHeader.Cells[2, hdr_col].Text = phnfax;
        //                FpEntry.Sheets[0].ColumnHeader.Cells[3, hdr_col].Text = email;
        //                FpEntry.Sheets[0].ColumnHeader.Cells[4, hdr_col].Text = form_heading_name;
        //                FpEntry.Sheets[0].ColumnHeader.Cells[5, hdr_col].Text = batch_degree_branch;
        //                if ((final_print_col_cnt != 0) && (final_print_col_cnt > 4))
        //                {
        //                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, hdr_col, 1, final_print_col_cnt - 4);
        //                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, hdr_col, 1, final_print_col_cnt - 4);
        //                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(2, hdr_col, 1, final_print_col_cnt - 4);
        //                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(3, hdr_col, 1, final_print_col_cnt - 4);
        //                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(4, hdr_col, 1, final_print_col_cnt - 4);
        //                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(5, hdr_col, 1, final_print_col_cnt - 4);
        //                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(6, hdr_col, 1, final_print_col_cnt - 4);
        //                }
        //                break;
        //            }
        //        }
        //    }
        //    else if (final_print_col_cnt == FpEntry.Sheets[0].ColumnCount)
        //    {
        //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 3].Text = collnamenew1;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Text = address;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[2, 3].Text = phnfax;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[3, 3].Text = email;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[4, 3].Text = form_heading_name;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[5, 3].Text = batch_degree_branch;
        //        if ((final_print_col_cnt != 0) && (final_print_col_cnt > 4))
        //        {
        //            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, final_print_col_cnt - 4);
        //            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 3, 1, final_print_col_cnt - 4);
        //            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(2, 3, 1, final_print_col_cnt - 4);
        //            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(3, 3, 1, final_print_col_cnt - 4);
        //            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(4, 3, 1, final_print_col_cnt - 4);
        //            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(5, 3, 1, final_print_col_cnt - 4);
        //            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(6, 3, 1, final_print_col_cnt - 4);
        //        }
        //        break;
        //    }
        //}
        //if (final_print_col_cnt >= 3)
        //{
        //    for (int logo_col = 0; logo_col < FpEntry.Sheets[0].ColumnCount; logo_col++)
        //    {
        //        if (FpEntry.Sheets[0].Columns[logo_col].Visible == true)
        //        {
        //            right_logo_clmn = logo_col;
        //        }
        //    }
        //    FpEntry.Sheets[0].SheetCorner.Columns[0].Width = 100;
        //    MyImg mi4 = new MyImg();
        //    mi4.ImageUrl = "Handler/Handler5.ashx?";
        //    FpEntry.Sheets[0].ColumnHeader.Cells[0, right_logo_clmn].CellType = mi4;
        //    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, right_logo_clmn, 8, 1);
        //}
        //if (dsprint.Tables[0].Rows.Count > 0)
        //{
        //    if (dsprint.Tables[0].Rows[0]["column_fields"].ToString() == string.Empty)
        //    {
        //        lblnorec.Visible = true;
        //        lblnorec.Text = "Select Atleast One Column From The TreeView";
        //        FpEntry.Visible = false;
        //        Buttontotal.Visible = false;
        //        lblrecord.Visible = false;
        //        DropDownListpage.Visible = false;
        //        TextBoxother.Visible = false;
        //        lblpage.Visible = false;
        //        TextBoxpage.Visible = false;
        //    }
        //    else
        //    {
        //        lblnorec.Visible = false;
        //        lblnorec.Text = string.Empty;
        //        FpEntry.Visible = true;
        //        Buttontotal.Visible = true;
        //        lblrecord.Visible = true;
        //        DropDownListpage.Visible = true;
        //        TextBoxother.Visible = true;
        //        lblpage.Visible = true;
        //        TextBoxpage.Visible = true;
        //    }
        //}
    }
    
    public void func_Print_Master_Setting()
    {
        FpEntry.Sheets[0].ColumnHeader.Rows[8].Visible = false;
        FpEntry.Sheets[0].ColumnHeader.Rows[9].Visible = false;
        hat.Clear();
        hat.Add("college_code", Session["collegecode"].ToString());
        hat.Add("form_name", "CAT.aspx");
        dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        if (dsprint.Tables[0].Rows.Count > 0)
        {
            for (int newlp = 0; newlp <= FpEntry.Sheets[0].ColumnCount - 1; newlp++)
            {
                FpEntry.Sheets[0].Columns[newlp].Visible = false;
            }
            if ((dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != " ") && (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != ""))
            {
                FpEntry.Sheets[0].ColumnHeader.Rows[8].Visible = false;
                FpEntry.Sheets[0].ColumnHeader.Rows[9].Visible = false;
                new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                new_header_string_split = new_header_string.Split(',');
                FpEntry.Sheets[0].SheetCorner.RowCount = FpEntry.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
            }
            //@@@@@@@@@@@ to visible the clmn start
            string printvar = string.Empty;
            int span_sub_count = 0;
            bool sub_span_flag = false;
            printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();
            string[] split_printvar = printvar.Split(',');
            FpEntry.Sheets[0].SheetCorner.RowCount += 2;
            for (int newloop = 0; newloop <= FpEntry.Sheets[0].ColumnCount - 1; newloop++)//total cmn count
            {
                for (int tot_clm_index = 0; tot_clm_index <= split_printvar.GetUpperBound(0); tot_clm_index++)
                {
                    string[] spl_sub_clmn = split_printvar[tot_clm_index].Split('*');//splitting the parent and child text (marks and subj)
                    if (spl_sub_clmn.GetUpperBound(0) > 0)
                    {
                        string[] splt_child_clmn_text = spl_sub_clmn[1].Split('$');//splittin the child column (only subject)
                        if (chk_final_clm_flag == false)
                        {
                            for (int chld_chk_row = 6; chld_chk_row <= 6 + splt_child_clmn_text.GetUpperBound(0) - 1; chld_chk_row++)
                            {
                                for (int index = 0; index <= splt_child_clmn_text.GetUpperBound(0); index++)
                                {
                                    if (splt_child_clmn_text.GetUpperBound(0) > 0)
                                    {
                                        if (FpEntry.Sheets[0].ColumnHeader.Cells[9, chld_chk_row].Text == splt_child_clmn_text[index].ToString()) // chk the heading
                                        {
                                            span_sub_count++;
                                            final_print_col_cnt++;
                                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, chld_chk_row].Text = spl_sub_clmn[0].ToString();//to display the marks as headng
                                            if (sub_span_flag == false)
                                            {
                                                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, chld_chk_row, 1, splt_child_clmn_text.GetUpperBound(0) - 1);//splt_child_clmn_text.GetUpperBound(0) - 1 for clmn count
                                            }
                                            sub_span_flag = true;
                                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, chld_chk_row].Border.BorderColorBottom = Color.Black;
                                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, chld_chk_row].Border.BorderColorRight = Color.Black;
                                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, chld_chk_row].Text = splt_child_clmn_text[index].ToString();
                                            FpEntry.Sheets[0].Columns[chld_chk_row].Visible = true;
                                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, chld_chk_row].Border.BorderColorBottom = Color.Black;
                                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, chld_chk_row].Border.BorderColor = Color.Black;
                                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, chld_chk_row].HorizontalAlign = HorizontalAlign.Center;
                                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, chld_chk_row].HorizontalAlign = HorizontalAlign.Center;
                                            FpEntry.Sheets[0].ColumnHeader.Rows[FpEntry.Sheets[0].ColumnHeader.RowCount - 1].BackColor = Color.AliceBlue;
                                            FpEntry.Sheets[0].ColumnHeader.Rows[FpEntry.Sheets[0].ColumnHeader.RowCount - 2].BackColor = Color.AliceBlue;
                                        }
                                    }
                                }
                            }
                            //      FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 6, 1, span_sub_count);//splt_child_clmn_text.GetUpperBound(0) - 1 for clmn count
                            chk_final_clm_flag = true;
                        }
                    }
                    else
                    {
                        //for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)//total selectd clm value
                        //{
                        if (FpEntry.Sheets[0].ColumnHeader.Cells[8, newloop].Text == split_printvar[tot_clm_index].ToString())
                        {
                            final_print_col_cnt++;
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, newloop].Text = split_printvar[tot_clm_index].ToString();
                            FpEntry.Sheets[0].Columns[newloop].Visible = true;
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, newloop].Border.BorderColorBottom = Color.Black;
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, newloop].Border.BorderColor = Color.Black;
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, newloop].HorizontalAlign = HorizontalAlign.Center;
                            FpEntry.Sheets[0].ColumnHeader.Rows[FpEntry.Sheets[0].ColumnHeader.RowCount - 1].BackColor = Color.AliceBlue;
                            FpEntry.Sheets[0].ColumnHeader.Rows[FpEntry.Sheets[0].ColumnHeader.RowCount - 2].BackColor = Color.AliceBlue;
                        }
                        // }
                    }
                }
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 0, 2, 1);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 1, 2, 1);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 2, 2, 1);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 3, 2, 1);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 4, 2, 1);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 5, 2, 1);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, totalcount, 2, 1);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, percentcount, 2, 1);
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, resultcount, 2, 1);
            }
            //@@@@@@@@@@@ to visible the clmn end
            for (int hdr_col = 0; hdr_col < FpEntry.Sheets[0].ColumnCount; hdr_col++)
            {
                if (FpEntry.Sheets[0].Columns[hdr_col].Visible == true)
                {
                    acol = hdr_col;
                    break;
                }
            }
            //@@@ to add the new header name strat
            if ((dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != " ") && (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != ""))
            {
                FpEntry.Sheets[0].ColumnHeader.Rows[8].Visible = false;
                FpEntry.Sheets[0].ColumnHeader.Rows[9].Visible = false;
                int strwindexcnt = 1;
                for (int strw = Convert.ToInt32(Session["sheetcorner"]); strw < FpEntry.Sheets[0].SheetCorner.RowCount - 2; strw++)
                {
                    if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Left")
                    {
                        FpEntry.Sheets[0].ColumnHeader.Cells[strw, acol].Text = new_header_string_split[strwindexcnt - 1].ToString();
                        FpEntry.Sheets[0].ColumnHeader.Cells[strw, acol].HorizontalAlign = HorizontalAlign.Left;
                    }
                    else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Center")
                    {
                        FpEntry.Sheets[0].ColumnHeader.Cells[strw, acol].Text = new_header_string_split[strwindexcnt - 1].ToString();
                        FpEntry.Sheets[0].ColumnHeader.Cells[strw, acol].HorizontalAlign = HorizontalAlign.Center;
                    }
                    else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Right")
                    {
                        FpEntry.Sheets[0].ColumnHeader.Cells[strw, acol].Text = new_header_string_split[strwindexcnt - 1].ToString();
                        FpEntry.Sheets[0].ColumnHeader.Cells[strw, acol].HorizontalAlign = HorizontalAlign.Right;
                    }
                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(strw, acol, 1, FpEntry.Sheets[0].ColumnCount);
                    strwindexcnt++;
                    FpEntry.Sheets[0].ColumnHeader.Cells[strw, acol].Border.BorderColorBottom = Color.Black;
                }
            }
            //@@@ to add the footer name
            if (dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
            {
                footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
                FpEntry.Sheets[0].RowCount += 3;// FpEntry.Sheets[0].RowCount++;
                footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
                string[] footer_text_split = footer_text.Split(',');
                footer_text = string.Empty;
                if (final_print_col_cnt < footer_count)
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
                    for (int col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount; col_count++)
                    {
                        if (FpEntry.Sheets[0].Columns[col_count].Visible == true)
                        {
                            // FpEntry.Sheets[0].SpanModel.Add((FpEntry.Sheets[0].RowCount - 1), col_count, 1, final_print_col_cnt);
                            FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Text = footer_text;
                            break;
                        }
                    }
                }
                else if (final_print_col_cnt == footer_count)
                {
                    for (int col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount; col_count++)
                    {
                        if (FpEntry.Sheets[0].Columns[col_count].Visible == true)
                        {
                            FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
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
                    split_col_for_footer = final_print_col_cnt / footer_count;
                    footer_balanc_col = final_print_col_cnt % footer_count;
                    for (int col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount; col_count++)
                    {
                        if (FpEntry.Sheets[0].Columns[col_count].Visible == true)
                        {
                            if (temp_count == 0)
                            {
                                FpEntry.Sheets[0].SpanModel.Add((FpEntry.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer + footer_balanc_col);
                            }
                            else
                            {
                                FpEntry.Sheets[0].SpanModel.Add((FpEntry.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);
                            }
                            FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
                            FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
                            FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
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
            //@@@ to add the footer end
            //@@@@@@ with header and without header
            function_radioheader();
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
    
    public void SpreadBind()
    {
        try
        {
            DataView dvparstudmark = new DataView();
            btnExcel.Visible = true;
            Button1.Visible = true;
            //Added By Srinath 28/2/2013
            txtexcelname.Visible = true;
            lblrptname.Visible = true;
            //RadioHeader.Visible = true;
            //Radiowithoutheader.Visible = true;
            //ddlpage.Visible = true;
            //lblpages.Visible = true;
            int hashcount = 0;
            int hashcount1 = 0;
            int hasrow_count = 0;
            FpEntry.Visible = true;
            string strsec = string.Empty;
            string sections = string.Empty;
            string batch = string.Empty;
            string degreecode = string.Empty;
            string subno = string.Empty;
            string semester = string.Empty;
            string exam_code = string.Empty;
            string criteria_no = string.Empty;
            string resmaxmrk = string.Empty;
            string resminmrk = string.Empty;
            string resduration = string.Empty;
            string subject_code = string.Empty;
            string acronym = string.Empty;
            string examdate = string.Empty;
            string entrydate = string.Empty;
            int res = 0;
            int count = 0;
            batch = ddlBatch.SelectedValue.ToString();
            degreecode = ddlBranch.SelectedValue.ToString();
            sections = ddlSec.SelectedValue.ToString();
            semester = ddlSemYr.SelectedValue.ToString();
            criteria_no = ddlTest.SelectedValue.ToString();
            int StudentsAppeared = 0;
            Hashtable hat2 = new Hashtable();
            int StudentsAbsent = 0;
            int StudentsPassed = 0;
            int StudentsFailed = 0;
            int classminmark = 0;
            int classaverage = 0;
            int classmaxmark = 0;
            int Passpercent1 = 0;
            int signat = 0;
            int doe = 0;
            int dos = 0;
            string rol_no = string.Empty;
            string sqlStr = string.Empty;
            int subcols = 0;
            int subcole = 0;
            int sno = 0;
            string subjcode = string.Empty;
            int ra_nk = 0;
            ////----------------------------------------new myth 08.12
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
            //'----------------------------------
            if (chklist.Items[0].Selected == true)
            {
                categrycount = categrycount + 1;
            }
            if (chklist.Items[1].Selected == true)
            {
                categrycount = categrycount + 1;
            }
            if (chklist.Items[2].Selected == true)
            {
                categrycount = categrycount + 1;
            }
            if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and sections='" + sections.ToString() + "'";
            }
            FpEntry.Sheets[0].RowCount = 0;
            FpEntry.Sheets[0].ColumnCount = 6;
            FpEntry.Sheets[0].ColumnHeader.RowCount = 2;//clmn count 7- 02.03.12
            FpEntry.Sheets[0].RowHeader.Visible = false;
            //'-------------------------------------------- Query for Get the subjectno,sub code,acronym ,examdate,minmrk,maxmrk,entrydate and examcode
            FarPoint.Web.Spread.TextCellType tx = new FarPoint.Web.Spread.TextCellType();
            FpEntry.Sheets[0].Columns[1].CellType = tx;
            FpEntry.Sheets[0].Columns[2].CellType = tx;
            filteration();
            string filterwithsection = "a.app_no=r.app_no and ISNULL(r.sections,'')=ISNULL(et.sections,'') and r.degree_code='" + degreecode.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + batch.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + criteria_no.ToString() + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0   and ISNULL(r.sections,'')='" + sections.ToString() + "' " + strorder + ",s.subject_no";
            string filterwithoutsection = "a.app_no=r.app_no and  ISNULL(r.sections,'')=ISNULL(et.sections,'') and r.degree_code='" + degreecode.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + batch.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + criteria_no.ToString() + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0  " + strorder + ",s.subject_no";  //modified by Mullai
            hat.Clear();
            hat.Add("batchyear", batch.ToString());
            hat.Add("degreecode", degreecode.ToString());
            hat.Add("criteria_no", criteria_no.ToString());
            hat.Add("sections", sections.ToString());
            hat.Add("filterwithsection", filterwithsection.ToString());
            hat.Add("filterwithoutsection", filterwithoutsection.ToString());
            ds2 = dacces2.select_method("PROC_STUD_ALL_SUBMARK", hat, "sp");
            if (ds2.Tables[1].Rows.Count > 0)
            {
                //'------------------------------------------- Query for Displaying the STUDENT DETAILS
                sqlStr = "select distinct len(registration.Roll_No) as roll_len,registration.Roll_No as roll,registration.Reg_No as regno,registration.stud_name as studname,registration.stud_type as studtype,registration.App_No as ApplicationNumber,registration.adm_date from registration , applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + "   and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0  " + strsec + " " + Session["strvar"] + " " + strregorder + " ";
                con.Close();
                con.Open();
                if (sqlStr != "")
                {
                    SqlDataAdapter adaSyll1 = new SqlDataAdapter(sqlStr, con);
                    adaSyll1.Fill(ds1);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        for (int irow = 0; irow < ds1.Tables[0].Rows.Count; irow++)
                        {
                            sno++;
                            FpEntry.Sheets[0].RowCount++;
                            FpEntry.Sheets[0].Rows[irow].Border.BorderColor = Color.Black;
                            FpEntry.Sheets[0].Cells[irow, 2].Text = ds1.Tables[0].Rows[irow]["regno"].ToString();
                            FpEntry.Sheets[0].Cells[irow, 1].Text = ds1.Tables[0].Rows[irow]["roll"].ToString();
                            FpEntry.Sheets[0].Cells[irow, 3].Text = ds1.Tables[0].Rows[irow]["studname"].ToString();
                            FpEntry.Sheets[0].Cells[irow, 4].Text = ds1.Tables[0].Rows[irow]["studtype"].ToString();
                            FpEntry.Sheets[0].Cells[irow, 5].Text = ds1.Tables[0].Rows[irow]["ApplicationNumber"].ToString();
                            FpEntry.Sheets[0].Cells[irow, 0].Text = sno.ToString();
                        }
                    }
                    Session["rowcount"] = FpEntry.Sheets[0].RowCount;
                    FpEntry.SaveChanges();
                    if (Session["Rollflag"].ToString() == "0")
                    {
                        FpEntry.Sheets[0].ColumnHeader.Columns[1].Visible = false;
                    }
                    if (Session["Regflag"].ToString() == "0")
                    {
                        FpEntry.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                    }
                    if (Session["Studflag"].ToString() == "0")
                    {
                        fg = true;
                        FpEntry.Sheets[0].ColumnHeader.Columns[4].Visible = false;
                    }
                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                    //'------------------------------------load the clg information
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
                        con.Close();
                    }
                    //'---------------------------------------------------------------------------------------------------------
                    //FpEntry.Sheets[0].ColumnHeader.Rows[1].Border.BorderColor = Color.White;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorTop = Color.White;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[2].Border.BorderColorRight = Color.White;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[4].Border.BorderColorBottom = Color.White;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[7].Border.BorderColorBottom = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorTop = Color.Black;
                    ////FpEntry.Sheets[0].ColumnHeader.Rows[8].Border.BorderColorBottom = Color.Black;
                    ////FpEntry.Sheets[0].ColumnHeader.Rows[9].Border.BorderColorBottom = Color.Black;
                    //FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorTop = Color.Black;
                    ////FpEntry.Sheets[0].ColumnHeader.Cells[0, FpEntry.Sheets[0].ColumnCount - 1].Border.BorderColorBottom = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, FpEntry.Sheets[0].ColumnCount - 1].Border.BorderColorTop = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorBottom = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorBottom = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColorBottom = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 5].Border.BorderColorBottom = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorRight = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColorRight = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 5].Border.BorderColorRight = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorBottom = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorBottom = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Rows[0].Visible = true;
                    FpEntry.Sheets[0].ColumnHeader.Rows[1].Visible = true;
                    FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                    style.Font.Size = 10;
                    style.Font.Bold = true;
                    FpEntry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                    FpEntry.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                    FpEntry.Sheets[0].AllowTableCorner = true;
                    FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Text = "  ";
                    //'---------------------------------------------load theclg logo photo-------------------------------------
                    //MyImg mi3 = new MyImg();
                    //mi3.ImageUrl = "Handler/Handler2.ashx?";
                    //'------------------span the 3 rows to display the img----------------
                    //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 8, 1);
                    //FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi3;
                    //FpEntry.Sheets[0].ColumnHeader.Columns[0].Width = 150;
                    FpEntry.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.AliceBlue;
                    FpEntry.Sheets[0].ColumnHeader.Rows[1].BackColor = Color.AliceBlue;
                    //FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 5].Text = "AppNo";
                    FpEntry.Sheets[0].Columns[1].Width = 150;
                    FpEntry.Sheets[0].Columns[0].Width = 50;
                    FpEntry.Sheets[0].Columns[2].Width = 150;
                    FpEntry.Sheets[0].Columns[3].Width = 150;
                    FpEntry.Sheets[0].Columns[5].Visible = false;
                    FpEntry.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                    Session["sheetcorner"] = FpEntry.Sheets[0].SheetCorner.RowCount;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[2].HorizontalAlign = HorizontalAlign.Center;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[3].HorizontalAlign = HorizontalAlign.Center;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[4].HorizontalAlign = HorizontalAlign.Center;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[5].HorizontalAlign = HorizontalAlign.Center;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[6].HorizontalAlign = HorizontalAlign.Center;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[7].HorizontalAlign = HorizontalAlign.Center;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[1].Font.Size = FontUnit.Medium;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[2].Font.Size = FontUnit.Medium;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[3].Font.Size = FontUnit.Medium;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[4].Font.Size = FontUnit.Medium;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[5].Font.Size = FontUnit.Medium;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[6].Font.Size = FontUnit.Medium;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[7].Font.Size = FontUnit.Medium;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[1].Font.Bold = true;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[2].Font.Bold = true;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[3].Font.Bold = true;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[4].Font.Bold = true;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[5].Font.Bold = true;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[6].Font.Bold = true;
                    //FpEntry.Sheets[0].ColumnHeader.Rows[7].Font.Bold = true;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                }
                hasrow_count = hasrow_count + 1;
                subcols = FpEntry.Sheets[0].ColumnCount;
                for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                {
                    subno = ds2.Tables[1].Rows[i]["subject_no"].ToString();
                    subject_code = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                    acronym = ds2.Tables[1].Rows[i]["acronym"].ToString();
                    resmaxmrk = ds2.Tables[1].Rows[i]["max_mark"].ToString();
                    resminmrk = ds2.Tables[1].Rows[i]["min_mark"].ToString();
                    resduration = ds2.Tables[1].Rows[i]["duration"].ToString();
                    exam_code = ds2.Tables[1].Rows[i]["exam_code"].ToString();
                    examdate = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                    entrydate = ds2.Tables[1].Rows[i]["entry_date"].ToString();
                    int x1 = FpEntry.Sheets[0].ColumnCount;
                    FpEntry.Sheets[0].ColumnCount = Convert.ToInt32(FpEntry.Sheets[0].ColumnCount) + 1;
                    int incr = FpEntry.Sheets[0].ColumnCount - 1;
                    //FpEntry.Sheets[0].ColumnHeader.Cells[1, incr].Note = entrydate;
                    //FpEntry.Sheets[0].ColumnHeader.Cells[0, incr].Tag = examdate + "@" + exam_code;
                    //'----------------------------------------------------------to display the acronym as heading
                    FpEntry.Sheets[0].ColumnHeader.Cells[1, incr].Text = subject_code;//modif
                    FpEntry.Sheets[0].ColumnHeader.Cells[1, incr].Note = subno;
                    // FpEntry.Sheets[0].ColumnHeader.Cells[1, incr].Text = subject_code + "-" + acronym;//Modify By M.SakthiPriya 09-12-2014
                    FpEntry.Sheets[0].ColumnHeader.Cells[1, incr].HorizontalAlign = HorizontalAlign.Center;
                    //FpEntry.Sheets[0].ColumnHeader.Cells[0, incr].Note = subno;
                    //   FpEntry.Sheets[0].ColumnHeader.Cells[2, 2].Text = ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + ddlSemYr.SelectedItem.ToString() + "-" + ddlSec.SelectedItem.ToString() + "-" + ddlTest.SelectedItem.ToString();
                    //FpEntry.Sheets[0].Cells[2, 0].HorizontalAlign = HorizontalAlign.Center;
                    //'----------------------------------------------------------- count for getting the total no.of subject
                    count++;
                    if (txtMarkconversion.Text != "")
                    {
                        FpEntry.Sheets[0].ColumnHeader.Cells[0, incr].Text = "Marks Out(" + txtMarkconversion.Text + ")";
                    }
                    else
                    {
                        FpEntry.Sheets[0].ColumnHeader.Cells[0, incr].Text = "Marks";
                    }
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, incr].Border.BorderColorRight = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[1, incr].Border.BorderColorRight = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[1, incr].Border.BorderColorTop = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[1, incr].Border.BorderColorBottom = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, incr].Border.BorderColorBottom = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, incr].HorizontalAlign = HorizontalAlign.Center;
                    child_sub_count = count;
                }
                //--------------------- spaning for marks text in 3 clmn-----------------------
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, count);
            }
            subcole = FpEntry.Sheets[0].ColumnCount - 1;
            FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 3;
            totalcount = FpEntry.Sheets[0].ColumnCount - 3;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, totalcount, 2, 1);
            FpEntry.Sheets[0].ColumnHeader.Cells[0, totalcount].Text = "Total";
            FpEntry.Sheets[0].ColumnHeader.Cells[0, totalcount].HorizontalAlign = HorizontalAlign.Center;
            FpEntry.Sheets[0].ColumnHeader.Cells[0, totalcount].Border.BorderColorRight = Color.Black;
            FpEntry.Sheets[0].ColumnHeader.Cells[0, totalcount].Border.BorderColorBottom = Color.Black;
            FpEntry.Sheets[0].ColumnHeader.Cells[0, totalcount].Border.BorderColorLeft = Color.Black;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, totalcount].Border.BorderColorLeft = Color.Black;
            FpEntry.Sheets[0].ColumnHeader.Cells[1, totalcount].Border.BorderColor = Color.Black;
            FpEntry.Sheets[0].Columns[totalcount].Width = 100;
            FpEntry.Sheets[0].Columns[0].Width = 50;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, totalcount, 2, 1);
            percentcount = FpEntry.Sheets[0].ColumnCount - 2;
            //     FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, percentcount, 2, 1);
            FpEntry.Sheets[0].ColumnHeader.Cells[0, percentcount].Text = "Percentage";
            FpEntry.Sheets[0].ColumnHeader.Cells[0, percentcount].HorizontalAlign = HorizontalAlign.Center;
            FpEntry.Sheets[0].Columns[percentcount].Width = 100;
            FpEntry.Sheets[0].Columns[0].Width = 50;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, percentcount, 2, 1);
            FpEntry.Sheets[0].ColumnHeader.Cells[0, percentcount].Border.BorderColorBottom = Color.Black;
            FpEntry.Sheets[0].ColumnHeader.Cells[0, percentcount].Border.BorderColorTop = Color.Black;
            FpEntry.Sheets[0].ColumnHeader.Cells[0, percentcount].Border.BorderColorBottom = Color.Black;
            FpEntry.Sheets[0].ColumnHeader.Cells[0, percentcount].Border.BorderColorRight = Color.Black;
            resultcount = FpEntry.Sheets[0].ColumnCount - 1;
            //   FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, resultcount, 2, 1);
            FpEntry.Sheets[0].ColumnHeader.Cells[0, resultcount].Text = "Result";
            FpEntry.Sheets[0].ColumnHeader.Cells[0, resultcount].HorizontalAlign = HorizontalAlign.Center;
            FpEntry.Sheets[0].Columns[resultcount].Width = 100;
            FpEntry.Sheets[0].ColumnHeader.Cells[0, resultcount].Border.BorderColorBottom = Color.Black;
            FpEntry.Sheets[0].ColumnHeader.Cells[0, resultcount].Border.BorderColorTop = Color.Black;
            FpEntry.Sheets[0].ColumnHeader.Cells[0, resultcount].Border.BorderColorBottom = Color.Black;
            FpEntry.Sheets[0].ColumnHeader.Cells[0, resultcount].Border.BorderColorRight = Color.Black;
            FpEntry.Sheets[0].Columns[0].Width = 50;
            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, resultcount, 2, 1);
            //'-----------------------setting the clg name--------------------------------------
            //FpEntry.Sheets[0].ColumnHeader.Cells[0, 3].Text = collnamenew1;
            //FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].Text = address;
            //FpEntry.Sheets[0].ColumnHeader.Cells[2, 3].Text = phnfax;
            //FpEntry.Sheets[0].ColumnHeader.Cells[3, 3].Text = email;
            //FpEntry.Sheets[0].ColumnHeader.Cells[4, 3].Text = "CAT REPORT";
            //FpEntry.Sheets[0].ColumnHeader.Cells[5, 3].Text = "Batch Year:  " + ddlBatch.SelectedValue.ToString() + " " + "Course: " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString();
            //FpEntry.Sheets[0].ColumnHeader.Cells[6, 3].Text = "Semester: " + ddlSemYr.SelectedValue.ToString() + "Section: " + ddlSec.SelectedValue.ToString() + "  " + ddlTest.SelectedItem.ToString();
            //FpEntry.Sheets[0].ColumnHeader.Cells[7, 3].Text = "From Date: " + txtFromDate.Text.ToString() + "-" + "To Date: " + txtToDate.Text.ToString();
            //FpEntry.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            //FpEntry.Sheets[0].ColumnHeader.Cells[1, 3].HorizontalAlign = HorizontalAlign.Center;
            //FpEntry.Sheets[0].ColumnHeader.Cells[2, 3].HorizontalAlign = HorizontalAlign.Center;
            //FpEntry.Sheets[0].ColumnHeader.Cells[3, 3].HorizontalAlign = HorizontalAlign.Center;
            //FpEntry.Sheets[0].ColumnHeader.Cells[4, 3].HorizontalAlign = HorizontalAlign.Center;
            FpEntry.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpEntry.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpEntry.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(4, totalcount, 1, 2 + categrycount);
            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(
            int count23 = 0;
            int attendcount = 0;
            int attpercount = 0;
            int rankcount = 0;
            if (chklist.Items[0].Selected == true)
            {
                FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                rankcount = FpEntry.Sheets[0].ColumnCount - 1;
                FpEntry.Sheets[0].ColumnHeader.Cells[0, rankcount].Text = "Rank";
                FpEntry.Sheets[0].ColumnHeader.Cells[0, rankcount].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Columns[rankcount].Width = 100;
                FpEntry.Sheets[0].Columns[0].Width = 50;
                //-----------------span the 3 and 4 throw
                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, rankcount, 2, 1);
                FpEntry.Sheets[0].ColumnHeader.Cells[0, rankcount].Border.BorderColorRight = Color.Black;
                FpEntry.Sheets[0].ColumnHeader.Cells[0, rankcount].Border.BorderColorBottom = Color.Black;
                //----------------------------------------------to display the logo span
                // FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, rankcount);
                // FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 5, 1, rankcount);
                //  FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(2, 5, 1, rankcount);
                FpEntry.Width = 1200;
            }
            if (chklist.Items[1].Selected == true)
            {
                if (chklist.Items[1].Text == "NoofHrAttended")//modified on 31.05.12
                {
                    FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                    attendcount = FpEntry.Sheets[0].ColumnCount - 1;
                    //-----------------span the 3 and 4 throw
                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, attendcount, 2, 1);
                    if (Session["Hourwise"] == "1")
                    {
                        FpEntry.Sheets[0].ColumnHeader.Cells[0, attendcount].Text = "No.Of.Hrs.Attended";
                    }
                    else
                    {
                        FpEntry.Sheets[0].ColumnHeader.Cells[0, attendcount].Text = "No.Of.Days.Attended";
                    }
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, attendcount].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, attendcount].Border.BorderColorRight = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, attendcount].Border.BorderColorBottom = Color.Black;
                    FpEntry.Sheets[0].Columns[attendcount].Width = 100;
                    FpEntry.Sheets[0].Columns[0].Width = 50;
                    //----------------------------------------------to display the logo span
                    FpEntry.Width = 1200;
                }
            }
            if (chklist.Items[2].Selected == true)
            {
                if (chklist.Items[2].Text == "Attendance %")
                {
                    FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                    attpercount = FpEntry.Sheets[0].ColumnCount - 1;
                    //-----------------span the 3 and 4 throw
                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, attpercount, 2, 1);
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, attpercount].Text = "Attendance %";
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, attpercount].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, attpercount].Border.BorderColorRight = Color.Black;
                    FpEntry.Sheets[0].ColumnHeader.Cells[0, attpercount].Border.BorderColorBottom = Color.Black;
                    FpEntry.Sheets[0].Columns[attpercount].Width = 100;
                    FpEntry.Sheets[0].Columns[0].Width = 50;
                    //----------------------------------------------to display the logo span
                    FpEntry.Width = 1200;
                }
            }
            //'--------------------------------------------right logo-----------------------------------
            bool logo_flag = false;
            //MyImg1 mi4 = new MyImg1();
            //mi4.ImageUrl = "Handler/Handler5.ashx?";
            if ((chklist.Items[0].Selected != true) || (chklist.Items[1].Selected != true) || (chklist.Items[2].Selected != true))
            {
                //------------------------spaning the 3 rows
                //    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, FpEntry.Sheets[0].ColumnCount - 1, 5, 1);
                //  FpEntry.Sheets[0].ColumnHeader.Cells[0, FpEntry.Sheets[0].ColumnCount - 1].CellType = mi4;
                FpEntry.Sheets[0].ColumnHeader.Columns[FpEntry.Sheets[0].ColumnCount - 1].Width = 150;
                //-------------------------------without select criteria----------------
                //FpEntry.Sheets[0].ColumnHeader.Cells[0, FpEntry.Sheets[0].ColumnCount - 1].Border.BorderColorBottom = Color.Black;
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, FpEntry.Sheets[0].ColumnCount - 4);
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 3, 1, FpEntry.Sheets[0].ColumnCount - 4);
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(2, 3, 1, FpEntry.Sheets[0].ColumnCount - 4);
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(3, 3, 1, FpEntry.Sheets[0].ColumnCount - 4);
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(4, 3, 1, FpEntry.Sheets[0].ColumnCount - 4);
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(5, 3, 1, FpEntry.Sheets[0].ColumnCount - 4);
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(6, 3, 1, FpEntry.Sheets[0].ColumnCount - 4);
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(7, 3, 1, FpEntry.Sheets[0].ColumnCount - 4);
                FpEntry.Width = 1200;
                FpEntry.Sheets[0].Columns[0].Width = 50;
                FpEntry.Sheets[0].Columns[FpEntry.Sheets[0].ColumnCount - 1].Width = 150;
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, FpEntry.Sheets[0].ColumnCount - 1, 1, 1);
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, FpEntry.Sheets[0].ColumnCount - 1, 8, 1);
            }
            else if ((chklist.Items[0].Selected == true) || (chklist.Items[1].Selected == true) || (chklist.Items[2].Selected == true))
            {
                //FpEntry.Sheets[0].ColumnHeader.Columns[FpEntry.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, FpEntry.Sheets[0].ColumnCount - 4);
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 3, 1, FpEntry.Sheets[0].ColumnCount - 4);
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(2, 3, 1, FpEntry.Sheets[0].ColumnCount - 4);
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(3, 3, 1, FpEntry.Sheets[0].ColumnCount - 4);
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(4, 3, 1, FpEntry.Sheets[0].ColumnCount - 4);
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(5, 3, 1, FpEntry.Sheets[0].ColumnCount - 4);
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(6, 3, 1, FpEntry.Sheets[0].ColumnCount - 4);
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(7, 3, 1, FpEntry.Sheets[0].ColumnCount - 4);
                //FpEntry.Sheets[0].ColumnHeader.Cells[0, FpEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                //FpEntry.Sheets[0].ColumnHeader.Cells[0, FpEntry.Sheets[0].ColumnCount - 1].CellType = mi4;
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, FpEntry.Sheets[0].ColumnCount - 1, 1, 1);
                //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, FpEntry.Sheets[0].ColumnCount - 1, 8, 1);
                //FpEntry.Sheets[0].ColumnHeader.Cells[0, FpEntry.Sheets[0].ColumnCount - 1].Border.BorderColorBottom = Color.Black;
                //FpEntry.Width = 1700;
                //FpEntry.Sheets[0].Columns[0].Width = 150;
                //FpEntry.Sheets[0].ColumnHeader.Columns[FpEntry.Sheets[0].ColumnCount - 1].Width = 150;
            }
            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, FpEntry.Sheets[0].ColumnCount - 1, 1, 1);
            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, FpEntry.Sheets[0].ColumnCount - 1, 8, 1);
            ////'---------------spaning for roll and reg columns--------------------------
            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, 2);
            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, 2);
            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, 2);
            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, 2);
            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, 2);
            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(5, 1, 1, 2);
            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(6, 1, 1, 2);
            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(7, 1, 1, 2);
            //----------------------------------------------------------------------
            ds8 = dacces2.select_method_wo_parameter("Delete_Rank_Table", "sp");
            //'-----------------------------------------------------------------
            double outoften = 0;
            double hashcount2 = 0;
            double total = 0;
            double mark = 0;
            double percent = 0;
            int sub_max_mark = 0;
            int stud = 0;
            int failcount = 0;
            string min_mark = string.Empty;
            int EL = 0;
            string max_mark = string.Empty;
            double totoutoften = 0;
            if (Convert.ToInt32(FpEntry.Sheets[0].RowCount) != 0)
            {
                FpEntry.SaveChanges();
                //'----------------------------------------optimize 22.12---------------------
                for (res = 0; res < Convert.ToInt32(ds1.Tables[0].Rows.Count); res++)
                {
                    rol_no = FpEntry.Sheets[0].Cells[res, 1].Text.ToString();
                    sub_max_mark = 0;//------------max mark
                    failcount = 0;//--------individual fail cnt
                    for (int scol = subcols; scol <= subcole; scol++)
                    {
                        string subjectno1 = FpEntry.Sheets[0].ColumnHeader.Cells[1, scol].Note.ToString();
                        ds2.Tables[0].DefaultView.RowFilter = "roll='" + rol_no + "' and subject_no='" + subjectno1 + "'";
                        dvparstudmark = ds2.Tables[0].DefaultView;
                        int col = 0;
                        if (dvparstudmark.Count > 0)
                        {
                            col = 5;
                            col = scol;
                            for (int chcount = 0; chcount < dvparstudmark.Count; chcount++)
                            {
                                // col++;
                                string chkrolno = dvparstudmark[chcount]["roll"].ToString();
                                string subjectno = dvparstudmark[chcount]["subject_no"].ToString();
                                mark = Convert.ToDouble(dvparstudmark[chcount]["mark"].ToString());
                                atten = dvparstudmark[chcount]["mark"].ToString();
                                //for (int col = 6; col < count + 6; col++)
                                //{
                                hashcount1 = 0;//fail
                                hashcount = 0;//pass
                                hashcount2 = 0;//absent
                                if (stud < ds2.Tables[1].Rows.Count)
                                {
                                    entrydate = ds2.Tables[1].Rows[stud]["entry_date"].ToString();
                                    examdate = ds2.Tables[1].Rows[stud]["exam_date"].ToString();
                                    exam_code = ds2.Tables[1].Rows[stud]["exam_code"].ToString();
                                    min_mark = ds2.Tables[1].Rows[stud]["min_mark"].ToString();
                                    max_mark = ds2.Tables[1].Rows[stud]["max_mark"].ToString();
                                }
                                if ((FpEntry.Sheets[0].Cells[res, col].Text == string.Empty) || FpEntry.Sheets[0].Cells[res, col].Text == null)
                                {
                                    EL = 1;
                                    FpEntry.Sheets[0].Cells[res, col].Text = "EL";
                                    FpEntry.Sheets[0].Cells[res, col].HorizontalAlign = HorizontalAlign.Center;
                                }
                                if (chkrolno == rol_no)
                                {
                                    EL = 0;
                                    //'--------------------------optim-----------------------------------
                                    if ((mark.ToString() != "") || (mark != null))
                                    {
                                        //'-------------------convert the mark by given value---------------------------------
                                        if (txtMarkconversion.Text != "")
                                        {
                                            outoften = ((Convert.ToDouble(mark) / Convert.ToDouble(max_mark.ToString())) * (Convert.ToDouble(txtMarkconversion.Text)));
                                            FpEntry.Sheets[0].Cells[res, col].Text = Convert.ToString(Math.Round(outoften, 2));
                                            //gowthaman 24july2013====
                                            totoutoften = Convert.ToDouble(Math.Round(outoften, 2));
                                            //========================
                                            FpEntry.Sheets[0].Cells[res, col].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            FpEntry.Sheets[0].Cells[res, col].Text = mark.ToString();
                                            FpEntry.Sheets[0].Cells[res, col].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        if (((Convert.ToDouble(mark)) < (Convert.ToDouble(resminmrk))))//(Convert.ToDouble(mark) >= 0) &&
                                        {
                                            //  failcount++;
                                            FpEntry.Sheets[0].Cells[res, col].ForeColor = Color.Red;
                                            FpEntry.Sheets[0].Cells[res, col].Font.Underline = true;
                                            FpEntry.Sheets[0].Cells[res, col].Font.Name = "Book Antiqua";
                                            FpEntry.Sheets[0].Cells[res, col].Font.Size = FontUnit.Medium;
                                        }
                                        if ((((Convert.ToDouble(mark)) < (Convert.ToDouble(resminmrk)))) && Convert.ToString(mark) != "-2" && Convert.ToString(mark) != "-3")
                                        {
                                            failcount++;
                                        }
                                        //'----------------------------switch case----------------------
                                        switch (atten)
                                        {
                                            case "-1":
                                                atten = "AAA";
                                                break;
                                            case "-2":
                                                atten = "EL";
                                                break;
                                            case "-3":
                                                atten = "EOD";
                                                break;
                                            case "-4":
                                                atten = "ML";
                                                break;
                                            case "-5":
                                                atten = "SOD";
                                                break;
                                            case "-6":
                                                atten = "NSS";
                                                break;
                                            case "-7":
                                                atten = "NJ";
                                                break;
                                            case "-8":
                                                atten = "S";
                                                break;
                                            case "-9":
                                                atten = "L";
                                                break;
                                            case "-10":
                                                atten = "NCC";
                                                break;
                                            case "-11":
                                                atten = "HS";
                                                break;
                                            case "-12":
                                                atten = "PP";
                                                break;
                                            case "-13":
                                                atten = "SYOD";
                                                break;
                                            case "-14":
                                                atten = "COD";
                                                break;
                                            case "-15":
                                                atten = "OOD";
                                                break;
                                            case "-16":
                                                atten = "OD";
                                                break;
                                            case "-17":
                                                atten = "LA";
                                                break;
                                            //******Added By Subburaj 21.08.2014*************//
                                            case "-18":
                                                atten = "RAA";
                                                break;
                                        }
                                        if ((atten.ToString() == "AAA") || atten.ToString() == "EL" || atten.ToString() == "EOD" || atten.ToString() == "ML" || atten.ToString() == "SOD" || atten.ToString() == "NSS" || atten.ToString() == "NJ" || atten.ToString() == "S" || atten.ToString() == "L" || atten.ToString() == "NCC" || atten.ToString() == "HS" || atten.ToString() == "PP" || atten.ToString() == "SYOD" || atten.ToString() == "COD" || atten.ToString() == "OOD" || atten.ToString() == "OD" || atten.ToString() == "LA" || atten.ToString() == "RAA")
                                        {
                                            FpEntry.Sheets[0].Cells[res, col].Text = atten.ToString();
                                            FpEntry.Sheets[0].Cells[res, col].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        //***********End********************//
                                        if ((mark >= 0) && (Convert.ToString(mark) != string.Empty))
                                        {
                                            if (txtMarkconversion.Text != "")
                                            {
                                                total = total + totoutoften;
                                                sub_max_mark = sub_max_mark + Convert.ToInt32(resmaxmrk);
                                            }
                                            else
                                            {
                                                total = total + mark;
                                                sub_max_mark = sub_max_mark + Convert.ToInt32(resmaxmrk);
                                            }
                                        }
                                        //==========================================================
                                        FpEntry.Sheets[0].Cells[res, totalcount].HorizontalAlign = HorizontalAlign.Center;
                                        FpEntry.Sheets[0].Cells[res, totalcount].Text = total.ToString();
                                        //-----------------------putting percent------------------------------
                                        percent = Convert.ToDouble((Convert.ToDouble(total) / sub_max_mark) * 100);
                                        FpEntry.Sheets[0].Cells[res, percentcount].Text = Convert.ToDouble(Math.Round(percent, 2)).ToString();
                                        FpEntry.Sheets[0].Cells[res, percentcount].HorizontalAlign = HorizontalAlign.Center;
                                        if ((Convert.ToString(percent) == "NaN") || (Convert.ToString(percent) == "Infinity"))
                                        {
                                            FpEntry.Sheets[0].Cells[res, percentcount].Text = "0";
                                        }
                                    }  //--------- end if(mark)
                                    stud++;
                                }
                                if ((FpEntry.Sheets[0].Cells[res, col].Text == string.Empty) || FpEntry.Sheets[0].Cells[res, col].Text == null)
                                {
                                    FpEntry.Sheets[0].Cells[res, col].Text = "EL";
                                    FpEntry.Sheets[0].Cells[res, col].HorizontalAlign = HorizontalAlign.Center;
                                }
                                //'--------------------------display the criteria------------------------------------------------
                                if (IsFlag == false)
                                {
                                    hat.Clear();
                                    string secss = string.Empty;  // added by sridhar aug 2014
                                    if (ddlSec.Enabled == false)  // added by sridhar aug 2014
                                    {
                                        secss = string.Empty;
                                    }
                                    else
                                    {
                                        secss = ddlSec.SelectedItem.Text.ToString();
                                    }
                                    if (secss.ToString().Trim() == "-1" || secss.ToString().Trim() == "" || secss.ToString().Trim() == null || secss.ToString().Trim() == "All")
                                    {
                                        secss = string.Empty;  // added by sridhar aug 2014
                                    }
                                    else
                                    {
                                        secss = ddlSec.SelectedItem.Text.ToString(); // added by sridhar aug 2014
                                    }
                                    hat.Add("exam_code", exam_code.ToString());
                                    hat.Add("min_marks", min_mark.ToString());
                                    hat.Add("section", secss);
                                    ds9 = dacces2.select_method("Proc_All_Subject_Details", hat, "sp");
                                    count23++;
                                    string temp = string.Empty;
                                    if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
                                    {
                                        strsec = string.Empty;
                                    }
                                    else
                                    {
                                        strsec = " and sections='" + sections.ToString() + "'";
                                    }
                                    //  '---------------------------------------------pass------------------------------------
                                    if (chklist.Items[5].Selected == true)
                                    {
                                        if (IsSetFlag == false)
                                        {
                                            FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                                            StudentsPassed = FpEntry.Sheets[0].RowCount - 1;
                                            FpEntry.Sheets[0].SetText(FpEntry.Sheets[0].RowCount - 1, 3, "Students Passed");
                                            FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                                        }
                                        FpEntry.Sheets[0].Cells[StudentsPassed, col].Text = ds9.Tables[1].Rows[0]["PASS_COUNT"].ToString();
                                        FpEntry.Sheets[0].Cells[StudentsPassed, col].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    //'---------------------------------------------fail------------------------------------
                                    if (chklist.Items[6].Selected == true)
                                    {
                                        if (IsSetFlag == false)
                                        {
                                            FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                                            StudentsFailed = FpEntry.Sheets[0].RowCount - 1;
                                            FpEntry.Sheets[0].SetText(FpEntry.Sheets[0].RowCount - 1, 3, "Students Failed");
                                            FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                                        }
                                        FpEntry.Sheets[0].Cells[StudentsFailed, col].Text = ds9.Tables[11].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();
                                        FpEntry.Sheets[0].Cells[StudentsFailed, col].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    //'-----------------------class avg-------------------------------------------------
                                    int total_pass_fail = 0;
                                    if (chklist.Items[7].Selected == true)
                                    {
                                        if (IsSetFlag == false)
                                        {
                                            FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                                            classaverage = FpEntry.Sheets[0].RowCount - 1;
                                            FpEntry.Sheets[0].SetText(classaverage, 3, "Class Average");
                                            FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                                        }
                                        total_pass_fail = Convert.ToInt32(ds9.Tables[1].Rows[0]["PASS_COUNT"]) + Convert.ToInt32(ds9.Tables[2].Rows[0]["FAIL_COUNT"]);
                                        //double cal_avg = Convert.ToDouble(ds9.Tables[0].Rows[0]["SUM"]) / Convert.ToDouble(total_pass_fail);
                                        double cal_avg = (Convert.ToDouble(ds9.Tables[0].Rows[0]["SUM"]) / Convert.ToDouble(ds9.Tables[8].Rows[0]["PRESENT_COUNT"]));
                                        cal_avg = Math.Round(cal_avg, 2);
                                        FpEntry.Sheets[0].Cells[classaverage, col].Text = cal_avg.ToString();
                                        FpEntry.Sheets[0].Cells[classaverage, col].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    double absentCount = 0;
                                    if (chkIncludeAbsent.Checked)
                                    {
                                        double.TryParse(Convert.ToString(ds9.Tables[9].Rows[0]["ABSENT_COUNT"]).Trim(), out absentCount);
                                    }
                                    //'----------------------pass percen----------------------------------------
                                    if (chklist.Items[10].Selected == true)
                                    {
                                        if (IsSetFlag == false)
                                        {
                                            FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                                            Passpercent1 = FpEntry.Sheets[0].RowCount - 1;
                                            FpEntry.Sheets[0].SetText(FpEntry.Sheets[0].RowCount - 1, 3, "Percentage Of pass");
                                            FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                                        }
                                       
                                        //  double pass_perc = (Convert.ToDouble(ds9.Tables[1].Rows[0]["PASS_COUNT"]) / Convert.ToDouble(total_pass_fail)) * 100;
                                        double pass_perc = (Convert.ToDouble(ds9.Tables[1].Rows[0]["PASS_COUNT"]) / (Convert.ToDouble(ds9.Tables[8].Rows[0]["PRESENT_COUNT"]) + absentCount)) * 100;
                                        pass_perc = Math.Round(pass_perc, 2);
                                        FpEntry.Sheets[0].Cells[Passpercent1, col].Text = pass_perc.ToString();
                                        FpEntry.Sheets[0].Cells[Passpercent1, col].HorizontalAlign = HorizontalAlign.Center;
                                    }

                                    double pass_perc1 = (Convert.ToDouble(ds9.Tables[1].Rows[0]["PASS_COUNT"]) / (Convert.ToDouble(ds9.Tables[8].Rows[0]["PRESENT_COUNT"]) + absentCount)) * 100;
                                    pass_perc1 = Math.Round(pass_perc1, 2);
                                    if (!hat2.Contains(count23))
                                    {
                                        hat2.Add(count23, pass_perc1);
                                    }
                                    if (chklist.Items[8].Selected == true)
                                    {
                                        double MaxMark1 = 0;
                                        if (IsSetFlag == false)
                                        {
                                            FpEntry.Sheets[0].RowCount += 1;
                                            classmaxmark = FpEntry.Sheets[0].RowCount - 1;
                                            FpEntry.Sheets[0].SetText(classmaxmark, 3, "Highest Mark");
                                            FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                                        }
                                        if (txtMarkconversion.Text != "")
                                        {
                                            MaxMark1 = Convert.ToDouble(ds9.Tables[3].Rows[0]["MAX_MARK"].ToString()) / Convert.ToDouble(txtMarkconversion.Text);
                                        }
                                        else
                                        {
                                            MaxMark1 = Convert.ToDouble(ds9.Tables[3].Rows[0]["MAX_MARK"].ToString());
                                        }
                                        FpEntry.Sheets[0].Cells[classmaxmark, col].HorizontalAlign = HorizontalAlign.Center;
                                        FpEntry.Sheets[0].SetText(classmaxmark, col, Convert.ToString(MaxMark1));
                                    }
                                    if (chklist.Items[9].Selected == true)
                                    {
                                        double MinMark1 = 0;
                                        if (IsSetFlag == false)
                                        {
                                            FpEntry.Sheets[0].RowCount += 1;
                                            classminmark = FpEntry.Sheets[0].RowCount - 1;
                                            FpEntry.Sheets[0].SetText(classminmark, 3, "Lowest Mark");
                                            FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                                        }
                                        if (txtMarkconversion.Text != "")
                                        {
                                            MinMark1 = Convert.ToDouble(ds9.Tables[4].Rows[0]["MIN_MARK"].ToString()) / Convert.ToDouble(txtMarkconversion.Text);
                                        }
                                        else
                                        {
                                            MinMark1 = Convert.ToDouble(ds9.Tables[4].Rows[0]["MIN_MARK"].ToString());
                                        }
                                        FpEntry.Sheets[0].Cells[classminmark, col].HorizontalAlign = HorizontalAlign.Center;
                                        FpEntry.Sheets[0].SetText(classminmark, col, Convert.ToString(MinMark1));
                                    }
                                    if ((chklist.Items[3].Selected == true))
                                    {
                                        if (IsSetFlag == false)
                                        {
                                            FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                                            StudentsAppeared = FpEntry.Sheets[0].RowCount - 1;
                                            FpEntry.Sheets[0].SetText(StudentsAppeared, 3, "Students Present");
                                            FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                                        }
                                        FpEntry.Sheets[0].Cells[StudentsAppeared, col].Text = ds9.Tables[8].Rows[0]["PRESENT_COUNT"].ToString();
                                        FpEntry.Sheets[0].Cells[StudentsAppeared, col].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (chklist.Items[4].Selected == true)
                                    {
                                        if (IsSetFlag == false)
                                        {
                                            FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                                            StudentsAbsent = FpEntry.Sheets[0].RowCount - 1;
                                            FpEntry.Sheets[0].SetText(FpEntry.Sheets[0].RowCount - 1, 3, "Students Absent");
                                            FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                                        }
                                        FpEntry.Sheets[0].Cells[StudentsAbsent, col].Text = ds9.Tables[9].Rows[0]["ABSENT_COUNT"].ToString();
                                        FpEntry.Sheets[0].Cells[StudentsAbsent, col].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (chklist.Items[11].Selected == true)
                                    {
                                        if (IsSetFlag == false)
                                        {
                                            FpEntry.Sheets[0].RowCount += 1;
                                            signat = FpEntry.Sheets[0].RowCount - 1;
                                            FpEntry.Sheets[0].SetText(signat, 3, "Staff Signature");
                                            FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                                        }
                                        string staff = string.Empty;
                                        if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
                                        {
                                            strsec = string.Empty;
                                        }
                                        else
                                        {
                                            strsec = " and exam_type.sections='" + sections.ToString() + "'";
                                        }
                                        if ((subno != "") && (criteria_no != ""))
                                        {
                                            temp = GetFunction("Select distinct staff_code from result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + " and exam_type.subject_no =" + subjectno + " " + strsec + " and exam_type.criteria_no =" + criteria_no + "");
                                            if (temp != "")
                                            {
                                                staff = GetFunction("select nameacr from staff_appl_master where appl_no in(select distinct  appl_no from staffmaster where staff_code = '" + temp + "')");
                                            }
                                            if (staff == "" && (temp) != "")
                                            {
                                                staff = GetFunction("select staff_name from staffmaster where staff_code = '" + temp + "'");
                                            }
                                            FpEntry.Sheets[0].SetText(signat, col, staff);
                                        }
                                    }
                                    if (chklist.Items[12].Selected == true)
                                    {
                                        if (IsSetFlag == false)
                                        {
                                            FpEntry.Sheets[0].RowCount += 1;
                                            doe = FpEntry.Sheets[0].RowCount - 1;
                                            FpEntry.Sheets[0].SetText(doe, 3, "DateOfExamination");
                                            FpEntry.Sheets[0].Rows[doe].Border.BorderColor = Color.Black;
                                        }
                                        FpEntry.Sheets[0].Cells[doe, col].Text = Convert.ToString(examdate);
                                    }
                                    if (chklist.Items[13].Selected == true)
                                    {
                                        if (IsSetFlag == false)
                                        {
                                            FpEntry.Sheets[0].RowCount += 1;
                                            dos = FpEntry.Sheets[0].RowCount - 1;
                                            FpEntry.Sheets[0].SetText(dos, 3, "DateOfSubmission");
                                            FpEntry.Sheets[0].Rows[dos].Border.BorderColor = Color.Black;
                                        }
                                        FpEntry.Sheets[0].Cells[dos, col].Text = Convert.ToString(entrydate);
                                    }
                                }
                                IsSetFlag = true;
                            }
                            //if (stud < ds2.Tables[0].Rows.Count)
                            //{
                            //    string chkrolno = ds2.Tables[0].Rows[stud]["roll"].ToString();
                            //    string subjectno = ds2.Tables[0].Rows[stud]["subject_no"].ToString();
                            //    mark = Convert.ToDouble(ds2.Tables[0].Rows[stud]["mark"].ToString());
                            //    atten = ds2.Tables[0].Rows[stud]["mark"].ToString();
                            //    if (chkrolno == rol_no)
                            //    {
                            //        EL = 0;
                            //        //'--------------------------optim-----------------------------------
                            //        if ((mark.ToString() != "") || (mark != null))
                            //        {
                            //            //'-------------------convert the mark by given value---------------------------------
                            //            if (txtMarkconversion.Text != "")
                            //            {
                            //                outoften = ((Convert.ToDouble(mark) / Convert.ToDouble(max_mark.ToString())) * (Convert.ToDouble(txtMarkconversion.Text)));
                            //                FpEntry.Sheets[0].Cells[res, col].Text = Convert.ToString(Math.Round(outoften, 2));
                            //                //gowthaman 24july2013====
                            //                totoutoften = Convert.ToDouble(Math.Round(outoften, 2));
                            //                //========================
                            //                FpEntry.Sheets[0].Cells[res, col].HorizontalAlign = HorizontalAlign.Center;
                            //            }
                            //            else
                            //            {
                            //                FpEntry.Sheets[0].Cells[res, col].Text = mark.ToString();
                            //                FpEntry.Sheets[0].Cells[res, col].HorizontalAlign = HorizontalAlign.Center;
                            //            }
                            //            if ((Convert.ToDouble(mark) >= 0) && ((Convert.ToDouble(mark)) < (Convert.ToDouble(resminmrk))))
                            //            {
                            //                //  failcount++;
                            //                FpEntry.Sheets[0].Cells[res, col].ForeColor = Color.Red;
                            //                FpEntry.Sheets[0].Cells[res, col].Font.Underline = true;
                            //                FpEntry.Sheets[0].Cells[res, col].Font.Name = "Book Antiqua";
                            //                FpEntry.Sheets[0].Cells[res, col].Font.Size = FontUnit.Medium;
                            //            }
                            //            if ((((Convert.ToDouble(mark)) < (Convert.ToDouble(resminmrk)))) && Convert.ToString(mark) != "-2" && Convert.ToString(mark) != "-3")
                            //            {
                            //                failcount++;
                            //            }
                            //            //'----------------------------switch case----------------------
                            //            switch (atten)
                            //            {
                            //                case "-1":
                            //                    atten = "AAA";
                            //                    break;
                            //                case "-2":
                            //                    atten = "EL";
                            //                    break;
                            //                case "-3":
                            //                    atten = "EOD";
                            //                    break;
                            //                case "-4":
                            //                    atten = "ML";
                            //                    break;
                            //                case "-5":
                            //                    atten = "SOD";
                            //                    break;
                            //                case "-6":
                            //                    atten = "NSS";
                            //                    break;
                            //                case "-7":
                            //                    atten = "NJ";
                            //                    break;
                            //                case "-8":
                            //                    atten = "S";
                            //                    break;
                            //                case "-9":
                            //                    atten = "L";
                            //                    break;
                            //                case "-10":
                            //                    atten = "NCC";
                            //                    break;
                            //                case "-11":
                            //                    atten = "HS";
                            //                    break;
                            //                case "-12":
                            //                    atten = "PP";
                            //                    break;
                            //                case "-13":
                            //                    atten = "SYOD";
                            //                    break;
                            //                case "-14":
                            //                    atten = "COD";
                            //                    break;
                            //                case "-15":
                            //                    atten = "OOD";
                            //                    break;
                            //                case "-16":
                            //                    atten = "OD";
                            //                    break;
                            //                case "-17":
                            //                    atten = "LA";
                            //                    break;
                            //            }
                            //            if ((atten.ToString() == "AAA") || atten.ToString() == "EL" || atten.ToString() == "EOD" || atten.ToString() == "ML" || atten.ToString() == "SOD" || atten.ToString() == "NSS" || atten.ToString() == "NJ" || atten.ToString() == "S" || atten.ToString() == "L" || atten.ToString() == "NCC" || atten.ToString() == "HS" || atten.ToString() == "PP" || atten.ToString() == "SYOD" || atten.ToString() == "COD" || atten.ToString() == "OOD" || atten.ToString() == "OD" || atten.ToString() == "LA")
                            //            {
                            //                FpEntry.Sheets[0].Cells[res, col].Text = atten.ToString();
                            //                FpEntry.Sheets[0].Cells[res, col].HorizontalAlign = HorizontalAlign.Center;
                            //            }
                            //            //if (txtMarkconversion.Text != "")
                            //            //{
                            //            //    outoften = Convert.ToDouble(mark) / Convert.ToDouble(txtMarkconversion.Text);
                            //            //    FpEntry.Sheets[0].Cells[res, col].Text = Convert.ToString(Math.Round(outoften, 2));
                            //            //    FpEntry.Sheets[0].Cells[res, col].HorizontalAlign = HorizontalAlign.Center;
                            //            //}
                            //            //else
                            //            //{
                            //            //    FpEntry.Sheets[0].Cells[res, col].Text = mark.ToString();
                            //            //    FpEntry.Sheets[0].Cells[res, col].HorizontalAlign = HorizontalAlign.Center;
                            //            //}
                            //            //  FpEntry.Sheets[0].Cells[res, col].ForeColor = Color.Red;
                            //            //'-----------------putting total---------------------------------
                            //            //Gowthamn 24july2013====================================
                            //            //if ((mark >= 0) && (Convert.ToString(mark) != string.Empty))
                            //            //{
                            //            //    total = total + mark;
                            //            //    sub_max_mark = sub_max_mark + Convert.ToInt32(resmaxmrk);
                            //            //}
                            //            if ((mark >= 0) && (Convert.ToString(mark) != string.Empty))
                            //            {
                            //                if (txtMarkconversion.Text != "")
                            //                {
                            //                    total = total + totoutoften;
                            //                    sub_max_mark = sub_max_mark + Convert.ToInt32(resmaxmrk);
                            //                }
                            //                else
                            //                {
                            //                    total = total + mark;
                            //                    sub_max_mark = sub_max_mark + Convert.ToInt32(resmaxmrk);
                            //                }
                            //            }
                            //            //==========================================================
                            //            FpEntry.Sheets[0].Cells[res, totalcount].HorizontalAlign = HorizontalAlign.Center;
                            //            FpEntry.Sheets[0].Cells[res, totalcount].Text = total.ToString();
                            //            //-----------------------putting percent------------------------------
                            //            percent = Convert.ToDouble((Convert.ToDouble(total) / sub_max_mark) * 100);
                            //            FpEntry.Sheets[0].Cells[res, percentcount].Text = Convert.ToDouble(Math.Round(percent, 2)).ToString();
                            //            FpEntry.Sheets[0].Cells[res, percentcount].HorizontalAlign = HorizontalAlign.Center;
                            //            if ((Convert.ToString(percent) == "NaN") || (Convert.ToString(percent) == "Infinity"))
                            //            {
                            //                FpEntry.Sheets[0].Cells[res, percentcount].Text = "0";
                            //            }
                            //        }  //--------- end if(mark)
                            //        stud++;
                            //    }
                            //    if ((FpEntry.Sheets[0].Cells[res, col].Text == string.Empty) || FpEntry.Sheets[0].Cells[res, col].Text == null)
                            //    {
                            //        FpEntry.Sheets[0].Cells[res, col].Text = "EL";
                            //        FpEntry.Sheets[0].Cells[res, col].HorizontalAlign = HorizontalAlign.Center;
                            //    }
                            //    //'--------------------------display the criteria------------------------------------------------
                            //    if (IsFlag == false)
                            //    {
                            //        hat.Clear();
                            //        hat.Add("exam_code", exam_code.ToString());
                            //        hat.Add("min_marks", min_mark.ToString());
                            //        ds9 = dacces2.select_method("Proc_All_Subject_Details", hat, "sp");
                            //        string temp = string.Empty;
                            //        if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
                            //        {
                            //            strsec = string.Empty;
                            //        }
                            //        else
                            //        {
                            //            strsec = " and sections='" + sections.ToString() + "'";
                            //        }
                            //        //  '---------------------------------------------pass------------------------------------
                            //        if (chklist.Items[5].Selected == true)
                            //        {
                            //            if (IsSetFlag == false)
                            //            {
                            //                FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                            //                StudentsPassed = FpEntry.Sheets[0].RowCount - 1;
                            //                FpEntry.Sheets[0].SetText(FpEntry.Sheets[0].RowCount - 1, 3, "Students Passed");
                            //                FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                            //            }
                            //            FpEntry.Sheets[0].Cells[StudentsPassed, col].Text = ds9.Tables[1].Rows[0]["PASS_COUNT"].ToString();
                            //            FpEntry.Sheets[0].Cells[StudentsPassed, col].HorizontalAlign = HorizontalAlign.Center;
                            //        }
                            //        //'---------------------------------------------fail------------------------------------
                            //        if (chklist.Items[6].Selected == true)
                            //        {
                            //            if (IsSetFlag == false)
                            //            {
                            //                FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                            //                StudentsFailed = FpEntry.Sheets[0].RowCount - 1;
                            //                FpEntry.Sheets[0].SetText(FpEntry.Sheets[0].RowCount - 1, 3, "Students Failed");
                            //                FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                            //            }
                            //            FpEntry.Sheets[0].Cells[StudentsFailed, col].Text = ds9.Tables[11].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();
                            //            FpEntry.Sheets[0].Cells[StudentsFailed, col].HorizontalAlign = HorizontalAlign.Center;
                            //        }
                            //        //'-----------------------class avg-------------------------------------------------
                            //        int total_pass_fail = 0;
                            //        if (chklist.Items[7].Selected == true)
                            //        {
                            //            if (IsSetFlag == false)
                            //            {
                            //                FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                            //                classaverage = FpEntry.Sheets[0].RowCount - 1;
                            //                FpEntry.Sheets[0].SetText(classaverage, 3, "Class Average");
                            //                FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                            //            }
                            //            total_pass_fail = Convert.ToInt32(ds9.Tables[1].Rows[0]["PASS_COUNT"]) + Convert.ToInt32(ds9.Tables[2].Rows[0]["FAIL_COUNT"]);
                            //            //double cal_avg = Convert.ToDouble(ds9.Tables[0].Rows[0]["SUM"]) / Convert.ToDouble(total_pass_fail);
                            //            double cal_avg = (Convert.ToDouble(ds9.Tables[0].Rows[0]["SUM"]) / Convert.ToDouble(ds9.Tables[8].Rows[0]["PRESENT_COUNT"]));
                            //            cal_avg = Math.Round(cal_avg, 2);
                            //            FpEntry.Sheets[0].Cells[classaverage, col].Text = cal_avg.ToString();
                            //            FpEntry.Sheets[0].Cells[classaverage, col].HorizontalAlign = HorizontalAlign.Center;
                            //        }
                            //        //'----------------------pass percen----------------------------------------
                            //        if (chklist.Items[10].Selected == true)
                            //        {
                            //            if (IsSetFlag == false)
                            //            {
                            //                FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                            //                Passpercent1 = FpEntry.Sheets[0].RowCount - 1;
                            //                FpEntry.Sheets[0].SetText(FpEntry.Sheets[0].RowCount - 1, 3, "Percentage Of pass");
                            //                FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                            //            }
                            //            //  double pass_perc = (Convert.ToDouble(ds9.Tables[1].Rows[0]["PASS_COUNT"]) / Convert.ToDouble(total_pass_fail)) * 100;
                            //            double pass_perc = (Convert.ToDouble(ds9.Tables[1].Rows[0]["PASS_COUNT"]) / Convert.ToDouble(ds9.Tables[8].Rows[0]["PRESENT_COUNT"])) * 100;
                            //            pass_perc = Math.Round(pass_perc, 2);
                            //            FpEntry.Sheets[0].Cells[Passpercent1, col].Text = pass_perc.ToString();
                            //            FpEntry.Sheets[0].Cells[Passpercent1, col].HorizontalAlign = HorizontalAlign.Center;
                            //        }
                            //        if (chklist.Items[8].Selected == true)
                            //        {
                            //            double MaxMark1 = 0;
                            //            if (IsSetFlag == false)
                            //            {
                            //                FpEntry.Sheets[0].RowCount += 1;
                            //                classmaxmark = FpEntry.Sheets[0].RowCount - 1;
                            //                FpEntry.Sheets[0].SetText(classmaxmark, 3, "Highest Mark");
                            //                FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                            //            }
                            //            if (txtMarkconversion.Text != "")
                            //            {
                            //                MaxMark1 = Convert.ToDouble(ds9.Tables[3].Rows[0]["MAX_MARK"].ToString()) / Convert.ToDouble(txtMarkconversion.Text);
                            //            }
                            //            else
                            //            {
                            //                MaxMark1 = Convert.ToDouble(ds9.Tables[3].Rows[0]["MAX_MARK"].ToString());
                            //            }
                            //            FpEntry.Sheets[0].Cells[classmaxmark, col].HorizontalAlign = HorizontalAlign.Center;
                            //            FpEntry.Sheets[0].SetText(classmaxmark, col, Convert.ToString(MaxMark1));
                            //        }
                            //        if (chklist.Items[9].Selected == true)
                            //        {
                            //            double MinMark1 = 0;
                            //            if (IsSetFlag == false)
                            //            {
                            //                FpEntry.Sheets[0].RowCount += 1;
                            //                classminmark = FpEntry.Sheets[0].RowCount - 1;
                            //                FpEntry.Sheets[0].SetText(classminmark, 3, "Lowest Mark");
                            //                FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                            //            }
                            //            if (txtMarkconversion.Text != "")
                            //            {
                            //                MinMark1 = Convert.ToDouble(ds9.Tables[4].Rows[0]["MIN_MARK"].ToString()) / Convert.ToDouble(txtMarkconversion.Text);
                            //            }
                            //            else
                            //            {
                            //                MinMark1 = Convert.ToDouble(ds9.Tables[4].Rows[0]["MIN_MARK"].ToString());
                            //            }
                            //            FpEntry.Sheets[0].Cells[classminmark, col].HorizontalAlign = HorizontalAlign.Center;
                            //            FpEntry.Sheets[0].SetText(classminmark, col, Convert.ToString(MinMark1));
                            //        }
                            //        if ((chklist.Items[3].Selected == true))
                            //        {
                            //            if (IsSetFlag == false)
                            //            {
                            //                FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                            //                StudentsAppeared = FpEntry.Sheets[0].RowCount - 1;
                            //                FpEntry.Sheets[0].SetText(StudentsAppeared, 3, "Students Present");
                            //                FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                            //            }
                            //            FpEntry.Sheets[0].Cells[StudentsAppeared, col].Text = ds9.Tables[8].Rows[0]["PRESENT_COUNT"].ToString();
                            //            FpEntry.Sheets[0].Cells[StudentsAppeared, col].HorizontalAlign = HorizontalAlign.Center;
                            //        }
                            //        if (chklist.Items[4].Selected == true)
                            //        {
                            //            if (IsSetFlag == false)
                            //            {
                            //                FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                            //                StudentsAbsent = FpEntry.Sheets[0].RowCount - 1;
                            //                FpEntry.Sheets[0].SetText(FpEntry.Sheets[0].RowCount - 1, 3, "Students Absent");
                            //                FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                            //            }
                            //            FpEntry.Sheets[0].Cells[StudentsAbsent, col].Text = ds9.Tables[9].Rows[0]["ABSENT_COUNT"].ToString();
                            //            FpEntry.Sheets[0].Cells[StudentsAbsent, col].HorizontalAlign = HorizontalAlign.Center;
                            //        }
                            //        if (chklist.Items[11].Selected == true)
                            //        {
                            //            if (IsSetFlag == false)
                            //            {
                            //                FpEntry.Sheets[0].RowCount += 1;
                            //                signat = FpEntry.Sheets[0].RowCount - 1;
                            //                FpEntry.Sheets[0].SetText(signat, 3, "Staff Signature");
                            //                FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                            //            }
                            //            string staff = string.Empty;
                            //            if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
                            //            {
                            //                strsec = string.Empty;
                            //            }
                            //            else
                            //            {
                            //                strsec = " and exam_type.sections='" + sections.ToString() + "'";
                            //            }
                            //            if ((subno != "") && (criteria_no != ""))
                            //            {
                            //                temp = GetFunction("Select distinct staff_code from result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + " and exam_type.subject_no =" + subjectno + " " + strsec + " and exam_type.criteria_no =" + criteria_no + "");
                            //                if (temp != "")
                            //                {
                            //                    staff = GetFunction("select nameacr from staff_appl_master where appl_no in(select distinct  appl_no from staffmaster where staff_code = '" + temp + "')");
                            //                }
                            //                if (staff == "" && (temp) != "")
                            //                {
                            //                    staff = GetFunction("select staff_name from staffmaster where staff_code = '" + temp + "'");
                            //                }
                            //                FpEntry.Sheets[0].SetText(signat, col, staff);
                            //            }
                            //        }
                            //        if (chklist.Items[12].Selected == true)
                            //        {
                            //            if (IsSetFlag == false)
                            //            {
                            //                FpEntry.Sheets[0].RowCount += 1;
                            //                doe = FpEntry.Sheets[0].RowCount - 1;
                            //                FpEntry.Sheets[0].SetText(doe, 3, "DateOfExamination");
                            //                FpEntry.Sheets[0].Rows[doe].Border.BorderColor = Color.Black;
                            //            }
                            //            FpEntry.Sheets[0].Cells[doe, col].Text = Convert.ToString(examdate);
                            //        }
                            //        if (chklist.Items[13].Selected == true)
                            //        {
                            //            if (IsSetFlag == false)
                            //            {
                            //                FpEntry.Sheets[0].RowCount += 1;
                            //                dos = FpEntry.Sheets[0].RowCount - 1;
                            //                FpEntry.Sheets[0].SetText(dos, 3, "DateOfSubmission");
                            //                FpEntry.Sheets[0].Rows[dos].Border.BorderColor = Color.Black;
                            //            }
                            //            FpEntry.Sheets[0].Cells[dos, col].Text = Convert.ToString(entrydate);
                            //        }
                            //    }
                            //    IsSetFlag = true;
                            //}
                        }//----------------------end col loop
                        //'---------------------------------------------display the result----------------
                        if (percent.ToString() == "NaN")
                        {
                            percent = 0;
                        }
                        if (EL == 0)
                        {
                            if (failcount == 0)
                            {
                                FpEntry.Sheets[0].Cells[res, resultcount].Text = "PASS";
                                hat.Clear();
                                hat.Add("RollNumber", rol_no.ToString());
                                hat.Add("criteria_no", criteria_no.ToString());
                                hat.Add("Total", total.ToString());
                                hat.Add("avg", percent.ToString());
                                hat.Add("rank", "");
                                int o = dacces2.insert_method("INSERT_RANK", hat, "sp");
                            }
                            else
                            {
                                FpEntry.Sheets[0].Cells[res, resultcount].Text = "FAIL";
                            }
                        }
                        //IsFlag = true;
                        //if (chklist.Items[0].Selected == true)
                        //{
                        //    ra_nk = 1;
                        //    ds3 = dacces2.select_method_wo_parameter("SELECT_RANK", "sp");
                        //    if (ds3.Tables[0].Rows.Count != 0)
                        //    {
                        //        double top_no = double.Parse(ds3.Tables[0].Rows[0]["Total"].ToString());
                        //        for (int rank = 0; rank < ds3.Tables[0].Rows.Count; rank++)
                        //        {
                        //            if (top_no > double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString()))
                        //            {
                        //                ra_nk += 1;
                        //            }
                        //            else
                        //            {
                        //                ra_nk = ra_nk;
                        //            }
                        //            top_no = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                        //            hat.Clear();
                        //            hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                        //            hat.Add("criteria_no", criteria_no.ToString());
                        //            hat.Add("Total", Convert.ToString(total));
                        //            hat.Add("avg", Convert.ToString(percent));
                        //            hat.Add("rank", ra_nk.ToString());
                        //            int o = dacces2.insert_method("INSERT_RANK", hat, "sp");
                        //        }
                        //    }
                        //}
                        //--------------------attend function-------------------------
                        hat.Clear();
                        hat.Add("colege_code", Session["collegecode"].ToString());
                        ds5 = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
                        countds = ds5.Tables[0].Rows.Count;
                        if ((chklist.Items[1].Selected == true) || (chklist.Items[2].Selected == true))
                        {
                            string dum_tage_date = string.Empty;
                            string dum_tage_hrs = string.Empty;
                            //Added By Srinath 25/2/2013 =====Start
                            chkdegreesem = ddlBranch.SelectedValue.ToString() + '/' + ddlSemYr.SelectedValue.ToString();
                            if (tempdegreesem != chkdegreesem)
                            {
                                tempdegreesem = chkdegreesem;
                                //====End
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
                                ds5 = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
                                countds = ds5.Tables[0].Rows.Count;
                                //Added By Srinath 25/2/2013 ===Start
                                string[] fromdatespit = txtFromDate.Text.Split('/');
                                string[] todatespit = txtToDate.Text.Split('/');
                                DateTime spfromdate = Convert.ToDateTime(fromdatespit[1] + '/' + fromdatespit[0] + '/' + fromdatespit[2]);
                                DateTime sptodate = Convert.ToDateTime(todatespit[1] + '/' + todatespit[0] + '/' + todatespit[2]);
                                ht_sphr.Clear();
                                string hrdetno = string.Empty;
                                string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and date between '" + spfromdate.ToString() + "' and '" + sptodate.ToString() + "'";
                                ds_sphr = dacces2.select_method(getsphr, hat, "Text");
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
                                //============End
                            }//Added By Srinath 25/2/2013
                            persentmonthcal();
                            //'----------------------------------------new start----------------
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
                            if (chklist.Items[1].Selected == true)
                            {
                                if (Session["Hourwise"] == "1")
                                {
                                    FpEntry.Sheets[0].Cells[res, attendcount].Text = per_per_hrs.ToString();
                                    FpEntry.Sheets[0].Cells[res, attendcount].HorizontalAlign = HorizontalAlign.Center;
                                }
                                else
                                {
                                    FpEntry.Sheets[0].Cells[res, attendcount].Text = pre_present_date.ToString();
                                    FpEntry.Sheets[0].Cells[res, attendcount].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                            if (chklist.Items[2].Selected == true)
                            {
                                if (Session["Hourwise"] == "1")
                                {
                                    FpEntry.Sheets[0].Cells[res, attpercount].Text = dum_tage_hrs.ToString();
                                    FpEntry.Sheets[0].Cells[res, attpercount].HorizontalAlign = HorizontalAlign.Center;
                                }
                                else // if (Session["Daywise"] == "1")
                                {
                                    FpEntry.Sheets[0].Cells[res, attpercount].Text = dum_tage_date.ToString();
                                    FpEntry.Sheets[0].Cells[res, attpercount].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                    }
                    total = 0;
                    student++;
                    IsFlag = true;
                }//----------------------------------------end loop row
                //modified by annyutha//
                int no = 0;
                FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 1;
                FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Text = "S.No";
                if (fg == true)
                {
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Text = "Subject Code";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Text = "Subject Name";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 8].Text = "Staff Name";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 9].Text = "Percentage";
                }
                else if (fg == false)
                {
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Text = "Subject Code";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Text = "Subject Name";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Text = "Staff Name";
                    FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 8].Text = "Percentage";
                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 8, 1, 2);
                }
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 8].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 9].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                //-----------------------------End------------------------------By M.SakthiPriya 09-12-2014
                int row_span_start = FpEntry.Sheets[0].RowCount - 1;
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
                    int totrowcnt = FpEntry.Sheets[0].RowCount + ds2.Tables[1].Rows.Count;
                    for (subrow = FpEntry.Sheets[0].RowCount; subrow < totrowcnt; subrow++)
                    {
                        if (incrrowcnt <= ds2.Tables[1].Rows.Count)
                        {
                            FpEntry.Sheets[0].RowCount += 1;
                            FpEntry.Sheets[0].Rows[FpEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;
                            no++;
                            FarPoint.Web.Spread.TextCellType txt1 = new FarPoint.Web.Spread.TextCellType();
                            FpEntry.Sheets[0].Columns[3].CellType = txt1;
                            FpEntry.Sheets[0].Cells[subrow, 3].Text = Convert.ToString(no);
                            FpEntry.Sheets[0].Cells[subrow, 3].HorizontalAlign = HorizontalAlign.Center;
                            if (fg == true)
                            {
                                FpEntry.Sheets[0].Cells[subrow, 7].Text = ds2.Tables[1].Rows[incrrowcnt - 1]["subject_name"].ToString();
                                FpEntry.Sheets[0].Cells[subrow, 6].Text = ds2.Tables[1].Rows[incrrowcnt - 1]["subject_code"].ToString();
                            }
                            else
                            {
                                FpEntry.Sheets[0].Cells[subrow, 6].Text = ds2.Tables[1].Rows[incrrowcnt - 1]["subject_name"].ToString();
                                FpEntry.Sheets[0].Cells[subrow, 4].Text = ds2.Tables[1].Rows[incrrowcnt - 1]["subject_code"].ToString();
                            }
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
                            if (fg == true)
                            {
                                FpEntry.Sheets[0].SetText(subrow, 8, staff);// Modify By M.SakthiPriya 09-12-2014
                            }
                            else
                            {
                                FpEntry.Sheets[0].SetText(subrow, 7, staff);
                            }
                            if (hat2.Contains(incrrowcnt - 0))//Added by srinath 3/7/2014
                            {
                                string val = hat2[incrrowcnt - 0].ToString();
                                if (fg == true)
                                {
                                    FpEntry.Sheets[0].Cells[subrow, 9].Text = val;
                                    FpEntry.Sheets[0].Cells[subrow, 9].HorizontalAlign = HorizontalAlign.Center;
                                    //FpEntry.Sheets[0].Columns[subrow, 10].Border.BorderColor = Color.White;
                                }
                                else
                                {
                                    FpEntry.Sheets[0].Cells[subrow, 8].Text = val;
                                    FpEntry.Sheets[0].Cells[subrow, 8].HorizontalAlign = HorizontalAlign.Center;
                                    FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 8, 1, 2);
                                    // FpEntry.Sheets[0].Columns[subrow,9].Border.BorderColor = Color.White;
                                }
                            }
                            incrrowcnt++;
                        }
                    }
                }
                //-------------spaning the unwanted cell 030412
                FpEntry.Sheets[0].SpanModel.Add(row_span_start, 10, FpEntry.Sheets[0].RowCount - row_span_start, FpEntry.Sheets[0].ColumnCount - 9);//Modify By M.SakthiPriya 09-12-2014
                FpEntry.Sheets[0].SpanModel.Add(row_span_start, 0, FpEntry.Sheets[0].RowCount - ds5.Tables[0].Rows.Count, 3);
                //ended on 16thdec2014//
                bool flagrank = false;
                double temp_rank = 0;
                int zx = 1;
                //'---------------display the rank----------------------------
                ds8 = dacces2.select_method_wo_parameter("Delete_Rank_Table", "sp");
                if (chklist.Items[0].Selected == true)
                {
                    for (int ro = 0; ro < student; ro++)
                    {
                        string totalmar = FpEntry.Sheets[0].Cells[ro, totalcount].Text.ToString();
                        string resu = FpEntry.Sheets[0].Cells[ro, totalcount + 2].Text.ToString();
                        if (resu.Trim().ToLower() == "pass")
                        {
                            hat.Clear();
                            hat.Add("RollNumber", FpEntry.Sheets[0].Cells[ro, 1].Text.ToString());
                            hat.Add("criteria_no", criteria_no.ToString());
                            hat.Add("Total", Convert.ToString(FpEntry.Sheets[0].Cells[ro, totalcount].Text));
                            hat.Add("avg", Convert.ToString(FpEntry.Sheets[0].Cells[ro, totalcount + 1].Text));
                            hat.Add("rank", ra_nk.ToString());
                            int o = dacces2.insert_method("INSERT_RANK", hat, "sp");
                        }
                    }
                    ds3 = dacces2.select_method_wo_parameter("SELECT_RANK", "sp");
                    int rank_row_count = 1;
                    for (int rank = 0; rank < ds3.Tables[0].Rows.Count; rank++)
                    {
                        string rrol = ds3.Tables[0].Rows[rank]["Rollno"].ToString().Trim().ToLower(); ;
                        for (int ro = 0; ro < student; ro++)
                        {
                            string getrol = FpEntry.Sheets[0].Cells[ro, 1].Text.ToString().Trim().ToLower();
                            if (rrol == getrol)
                            {
                                FpEntry.Sheets[0].Cells[ro, rankcount].Text = rank_row_count.ToString();
                                FpEntry.Sheets[0].Cells[ro, rankcount].HorizontalAlign = HorizontalAlign.Center;
                                rank_row_count++;
                            }
                        }
                    }
                    ////ra_nk = 1;
                    //ds3 = dacces2.select_method_wo_parameter("SELECT_RANK", "sp");
                    //if (ds3.Tables[0].Rows.Count != 0)
                    //{
                    //    // double top_no = double.Parse(ds3.Tables[0].Rows[0]["Total"].ToString());
                    //    for (int rank = 0; rank < ds3.Tables[0].Rows.Count; rank++)
                    //    {
                    //        if (temp_rank == 0)
                    //        {
                    //            ra_nk = 1;
                    //            hat.Clear();
                    //            hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                    //            hat.Add("criteria_no", criteria_no.ToString());
                    //            hat.Add("Total", Convert.ToString(total));
                    //            hat.Add("avg", Convert.ToString(percent));
                    //            hat.Add("rank", ra_nk.ToString());
                    //            int o = dacces2.insert_method("INSERT_RANK", hat, "sp");
                    //            temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                    //        }
                    //        else if (temp_rank != 0)
                    //        {
                    //            if (temp_rank > double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString()))
                    //            {
                    //                //   ra_nk += 1;
                    //                ra_nk = zx;
                    //                hat.Clear();
                    //                hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                    //                hat.Add("criteria_no", criteria_no.ToString());
                    //                hat.Add("Total", Convert.ToString(total));
                    //                hat.Add("avg", Convert.ToString(percent));
                    //                hat.Add("rank", ra_nk.ToString());
                    //                int o = dacces2.insert_method("INSERT_RANK", hat, "sp");
                    //                temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                    //            }
                    //            else if (temp_rank == double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString()))
                    //            {
                    //                hat.Clear();
                    //                hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                    //                hat.Add("criteria_no", criteria_no.ToString());
                    //                hat.Add("Total", Convert.ToString(total));
                    //                hat.Add("avg", Convert.ToString(percent));
                    //                hat.Add("rank", ra_nk.ToString());
                    //                int o = dacces2.insert_method("INSERT_RANK", hat, "sp");
                    //                temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                    //            }
                    //        }
                    //        zx++;
                    //    }
                    //}
                }
                //if (chklist.Items[0].Selected == true)
                //{
                //    if (flagrank == false)
                //    {
                //        ds3 = dacces2.select_method_wo_parameter("SELECT_RANK", "sp");
                //        if (ds3.Tables[0].Rows.Count != 0)
                //        {
                //            int rank_row_count = 0;
                //            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                //            {
                //                if (rank_row_count < ds3.Tables[1].Rows.Count)
                //                {
                //                    if (ds3.Tables[1].Rows[rank_row_count]["Rollno"].ToString() == ds1.Tables[0].Rows[i]["roll"].ToString())
                //                    {
                //                        FpEntry.Sheets[0].Cells[i, rankcount].Text = ds3.Tables[1].Rows[rank_row_count]["Rank"].ToString();
                //                        FpEntry.Sheets[0].Cells[i, rankcount].HorizontalAlign = HorizontalAlign.Center;
                //                        rank_row_count++;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    flagrank = true;
                //}
                //Total(batch, ddlBranch.SelectedValue.ToString(), ddlSec.SelectedValue.ToString(),Convert.ToInt16(criteria_no));
            }
        }
        catch
        {
        }
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
        int count = ds1.Tables[0].Rows.Count;
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
            string RollNumber = Convert.ToString(ds1.Tables[0].Rows[i]["roll"]);
            Totcon2.Close();
            Totcon2.Open();
            string str = "select r.marks_obtained as marks,e.min_mark as minmark ,e.exam_code as examcode from exam_type e,subject s,result r where e.subject_no=s.subject_no and e.exam_code= r.exam_code and criteria_no='" + criteriano + "' and r.roll_no='" + RollNumber.ToString() + "'";
            SqlDataAdapter da1 = new SqlDataAdapter(str, Totcon2);
            DataSet dsss = new DataSet();
            da1.Fill(dsss);
            int count1;
            count1 = dsss.Tables[0].Rows.Count;
            for (int j = 0; j < count1; j++)
            {
                if ((dsss.Tables[0].Rows[j]["marks"].ToString()) == "-2")
                {
                    total = total + 0;
                }
                if (Convert.ToDouble(dsss.Tables[0].Rows[j]["marks"]) >= Convert.ToDouble(dsss.Tables[0].Rows[0]["minmark"]))
                {
                    total = total + Convert.ToDouble(dsss.Tables[0].Rows[j]["marks"]);
                }
                else if ((dsss.Tables[0].Rows[j]["marks"].ToString()) != "-2")
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
    
    public void persentmonthcal()
    {
        bool isadm = false;
        try
        {
            spl_tot_condut = 0;
            int demfcal, demtcal;
            string monthcal;
            int mmyycount = 0;
            DateTime Admission_date;
            int my_un_mark = 0;
            int njdate_mng = 0, njdate_evng = 0;
            int per_holidate_mng = 0, per_holidate_evng = 0;
            mng_conducted_half_days = 0;
            evng_conducted_half_days = 0;
            notconsider_value = 0;
            conduct_hour_new = 0;
            if (datechk != true)
            {
                datechk = true;
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
                tempfromdate = frdate;
                tempcallfromdate = cal_from_date;
            }
            frdate = tempfromdate;
            cal_from_date = tempcallfromdate;
            per_from_date = Convert.ToDateTime(frdate);
            per_to_date = Convert.ToDateTime(todate);
            dumm_from_date = per_from_date;
            string admdate = ds1.Tables[0].Rows[rows_count]["adm_date"].ToString();
            string[] admdatesp = admdate.Split(new Char[] { '/' });
            admdate = admdatesp[0].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[2].ToString();
            Admission_date = Convert.ToDateTime(admdate);
            hat.Clear();
            hat.Add("std_rollno", ds1.Tables[0].Rows[student]["roll"].ToString());
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds6 = dacces2.select_method("STUD_ATTENDANCE", hat, "sp");
            mmyycount = ds6.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
            chkdegreesem = ddlBranch.SelectedValue.ToString() + '/' + ddlSemYr.SelectedItem.ToString();
            if (chkdegreesem != tempdegreesempresent)
            {
                tempdegreesempresent = chkdegreesem;
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
                ds7 = dacces2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
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
                    //------------------------------------------------------------------
                    if (ds7.Tables[0].Rows.Count != 0)
                    {
                        ts = DateTime.Parse(ds7.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                        diff_date = Convert.ToString(ts.Days);
                        dif_date1 = double.Parse(diff_date.ToString());
                    }
                    string splhrrightquery = "select rights from  special_hr_rights where " + grouporusercode + "";
                    ds_sphr = dacces2.select_method(splhrrightquery, hat, "Text");
                    if (ds_sphr.Tables[0].Rows.Count > 0)
                    {
                        string spl_hr_rights = ds_sphr.Tables[0].Rows[0]["rights"].ToString();
                        if (spl_hr_rights == "True" || spl_hr_rights == "true")
                        {
                            splhr_flag = true;
                        }
                    }
                }
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
                                                        if (ds5.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                        {
                                                            ObtValue = int.Parse(ds5.Tables[0].Rows[j]["CalcFlag"].ToString());
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
                                                        if (ds5.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                        {
                                                            ObtValue = int.Parse(ds5.Tables[0].Rows[j]["CalcFlag"].ToString());
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
        catch
        {
        }
    }
    
    #region "Present Monthcal Old"

    /* public void persentmonthcal()
    {
        int demfcal, demtcal;
        string monthcal;
        int mmyycount = 0;
        //Added By Srinath 25/2/2013 ==Start
        if (datechk != true)
        {
            datechk = true;
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
            //Added By Srinath ==Start
            tempfromdate = frdate;
            tempcallfromdate = cal_from_date;
            //=====End
        }
        frdate = tempfromdate;
        cal_from_date = tempcallfromdate;
        per_from_date = Convert.ToDateTime(frdate);
        per_to_date = Convert.ToDateTime(todate);
        dumm_from_date = per_from_date;
        hat.Clear();
        hat.Add("std_rollno", ds1.Tables[0].Rows[student]["roll"].ToString());
        hat.Add("from_month", cal_from_date);
        hat.Add("to_month", cal_to_date);
        ds6 = dacces2.select_method("STUD_ATTENDANCE", hat, "sp");
        mmyycount = ds6.Tables[0].Rows.Count;
        moncount = mmyycount - 1;
        //Modified By Srinath 25/2/2013 ===Start
        if (tempdegreesempresent != chkdegreesem)
        {
            tempdegreesempresent = chkdegreesem;
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
            //Hidden By Srinath 25/2/2013 ====Start
            //mmyycount = ds6.Tables[0].Rows.Count;
            //moncount = mmyycount - 1;
            //======End
            ds7 = dacces2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
            //------------------------------------------------------------------
            if (ds7.Tables[0].Rows.Count != 0)
            {
                ts = DateTime.Parse(ds7.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                diff_date = Convert.ToString(ts.Days);
                dif_date1 = double.Parse(diff_date.ToString());
            }
            //added By srinath 25/2/2013
            string splhrquery = "select rights from  special_hr_rights where " + grouporusercode + "";
            DataSet dssplhrrisght = dacces2.select_method(splhrquery, hat, "Text");
            string spl_hr_rights = string.Empty;
            if (dssplhrrisght.Tables[0].Rows.Count > 0)
            {
                spl_hr_rights = dssplhrrisght.Tables[0].Rows[0]["rights"].ToString();
                if (spl_hr_rights == "True" || spl_hr_rights == "true")
                {
                    splhr_flag = true;
                }
            }
        }//==================Modified End
        next = 0;
        if (ds6.Tables[0].Rows.Count != 0)
        {
            int rowcount = 0;
            int ccount;
            ccount = ds7.Tables[1].Rows.Count;
            ccount = ccount - 1;
            //if ( == ds2.Tables [0].Rows [mmyycount].["Month_year"])
            if (ds7.Tables[1].Rows.Count > 0)
            {
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
                    //========End
                    // for (int i = 1; i <= mmyycount; i++)
                    // {
                    if (cal_from_date == int.Parse(ds6.Tables[0].Rows[next]["month_year"].ToString()))
                    {
                        if (dumm_from_date != DateTime.Parse(ds7.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()))
                        {
                            //ts = DateTime.Parse(ds7.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                            //diff_date = Convert.ToString(ts.Days);
                            //dif_date = double.Parse(diff_date.ToString());
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
                                            if (ds5.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                            {
                                                ObtValue = int.Parse(ds5.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                j = countds;
                                            }
                                        }
                                    }
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
                            if (per_perhrs >= minpresI)
                            {
                                Present += 0.5;
                            }
                            else if (njhr == fnhrs)
                            {
                                njdate += 0.5;
                            }
                            per_perhrs = 0;
                            // njhr = 0;
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
                                            if (ds5.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                            {
                                                ObtValue = int.Parse(ds5.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                j = countds;
                                            }
                                        }
                                    }
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
                            per_perhrs = 0;
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
                    //  }
                }//'----end while
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
    } */

    #endregion
    
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
            string splhr_query_master = "select attendance from specialhr_attendance where roll_no='" + ds1.Tables[0].Rows[student]["roll"].ToString() + "'  and hrdet_no in(" + hrdetno + ")";
            ds_splhr_query_master = dacces2.select_method(splhr_query_master, hat, "Text");
            if (ds_splhr_query_master.Tables[0].Rows.Count > 0)
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
                                if (ds5.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                {
                                    ObtValue = int.Parse(ds5.Tables[0].Rows[j]["CalcFlag"].ToString());
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
                    con2.Open();
                    string sqlhour;
                    string strcalflag = string.Empty;
                    sqlhour = "select d" + exam_date.Day + "d" + srtprd + "  from attendance where month_year='" + monthyear + "' and  roll_no='" + roll_no + "'";
                    SqlCommand cmdhour = new SqlCommand(sqlhour, con2);
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
                    con2.Close();
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

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Buttontotal.Visible = false;
        //lblrecord.Visible = false;
        //DropDownListpage.Visible = false;
        //TextBoxother.Visible = false;
        //lblpage.Visible = false;
        //TextBoxpage.Visible = false;
        FpEntry.Visible = false;
        lblnorec.Visible = false;
        ddlTest.Items.Clear();
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        lblpages.Visible = false;
        ddlpage.Visible = false;
        LabelE.Visible = false;
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 28/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }
        if (ddlSec.Enabled == true)
        {
            GetTest();
        }
        else
        {
            GetTest();
        }
        BindSectionDetail();
    }

    protected void ddlTest_SelectedIndexChanged1(object sender, EventArgs e)
    {
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
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
            if (ddlTest.Items.Count >= 0)
            {
                if (ddlTest.SelectedItem.Text == "--Select--" || ddlTest.SelectedItem.Text == "-1" || ddlTest.SelectedItem.Text == null || ddlTest.SelectedItem.Text == "")
                {
                    lblerroe.Text = "Please Select Any one Test";
                    lblerroe.Visible = true;
                    //lblnorec.Text = string.Empty;
                    lblnorec.Visible = false;
                    return;
                }
            }
            //added by sridhar 03 sep 2014 --------------* End  *-------------------------
            string valfromdate = string.Empty;
            string valtodate = string.Empty;
            string frmconcat = string.Empty;
            if ((txtFromDate.Text != string.Empty) && (txtToDate.Text != string.Empty))
            {
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
                    lblEmrkconversion.Text = "From Date Must Be Less Than To Date";
                    lblEmrkconversion.Visible = true;
                    lblnorec.Visible = false;
                    //Buttontotal.Visible = false;
                    //lblrecord.Visible = false;
                    //DropDownListpage.Visible = false;
                    //TextBoxother.Visible = false;
                    //lblpage.Visible = false;
                    //TextBoxpage.Visible = false;
                    FpEntry.Visible = false;
                    FpEntry.Sheets[0].RowCount = 0;
                }
                else
                {
                    lblEmrkconversion.Text = string.Empty;
                    lblEmrkconversion.Visible = false;
                    lblnorec.Visible = false;
                    //Buttontotal.Visible = false;
                    //lblrecord.Visible = false;
                    //DropDownListpage.Visible = false;
                    //TextBoxother.Visible = false;
                    //lblpage.Visible = false;
                    //TextBoxpage.Visible = false;
                    FpEntry.Visible = false;
                    SpreadBind();
                }
                //'--------------------display the no.of records
                if (Convert.ToInt32(FpEntry.Sheets[0].RowCount) == 0)
                {
                    btnExcel.Visible = false;
                    Button1.Visible = false;
                    //Added By Srinath 28/2/2013
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    if (lblEmrkconversion.Visible != true)
                    {
                        lblnorec.Visible = true;
                    }
                    //Buttontotal.Visible = false;
                    //lblrecord.Visible = false;
                    //DropDownListpage.Visible = false;
                    //TextBoxother.Visible = false;
                    //lblpage.Visible = false;
                    //TextBoxpage.Visible = false;
                    FpEntry.Visible = false;
                    lblEtest.Visible = false;
                    RadioHeader.Visible = false;
                    Radiowithoutheader.Visible = false;
                    lblpages.Visible = false;
                    ddlpage.Visible = false;
                }
                else
                {
                    btnExcel.Visible = true;
                    Button1.Visible = true;
                    //Added By Srinath 28/2/2013
                    txtexcelname.Visible = true;
                    lblrptname.Visible = true;
                    lblEtest.Visible = false;
                    //Buttontotal.Visible = true;
                    //lblrecord.Visible = true;
                    //DropDownListpage.Visible = true;
                    //TextBoxother.Visible = false;
                    //lblpage.Visible = true;
                    //TextBoxpage.Visible = true;
                    FpEntry.Visible = true;
                    Double totalRows = 0;
                    totalRows = Convert.ToInt32(FpEntry.Sheets[0].RowCount);
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / FpEntry.Sheets[0].PageSize);
                    //Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                    DropDownListpage.Items.Clear();
                    if (totalRows >= 10)
                    {
                        FpEntry.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                        {
                            DropDownListpage.Items.Add((k + 10).ToString());
                        }
                        DropDownListpage.Items.Add("Others");
                        DropDownListpage.SelectedIndex = DropDownListpage.Items.Count - 2;
                        FpEntry.Height = 300;
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
                    }
                    //FpEntry.Width = 1200;
                    FpEntry.Height = 100 + (10 * Convert.ToInt32(totalRows));
                }
            }
            else
            {
                lblnorec.Visible = false;
                lblEtest.Visible = true;
                lblEtest.Text = "Select Date";
                //lblMrkconversion.Visible = true;
                //Buttontotal.Visible = false;
                //lblrecord.Visible = false;
                //DropDownListpage.Visible = false;
                //TextBoxother.Visible = false;
                //lblpage.Visible = false;
                //TextBoxpage.Visible = false;
                FpEntry.Visible = false;
                RadioHeader.Visible = false;
                Radiowithoutheader.Visible = false;
                lblpages.Visible = false;
                ddlpage.Visible = false;
            }
            //    lblEtest.Visible = false;
            //   lblnorec.Visible = false;
            //    lblEsection.Visible = false;
            //   }
        }
        catch
        {
        }
        //catch
        //{
        //}
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblnorec.Visible = false;
        GetTest();
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        lblpages.Visible = false;
        ddlpage.Visible = false;
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 28/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        LabelE.Visible = false;
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if ((ddlDegree.SelectedIndex != 0) && (ddlBranch.SelectedIndex != 0))
        //{
        //    // Get_Semester();
        //    bindsem();
        //}
        //Buttontotal.Visible = false;
        //lblrecord.Visible = false;
        //DropDownListpage.Visible = false;
        //TextBoxother.Visible = false;
        //lblpage.Visible = false;
        //TextBoxpage.Visible = false;
        FpEntry.Visible = false;
        lblnorec.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        lblpages.Visible = false;
        ddlpage.Visible = false;
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 28/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        LabelE.Visible = false;
        //binddegree();
        if (ddlDegree.Text != "")
        {
            //bindbranch();
            //bindsem();
            //bindsec();
            GetTest();
            lblnorec.Visible = false;
            ddlDegree.SelectedIndex = 0;
            ddlBranch.SelectedIndex = 0;
        }
        else
        {
            lblnorec.Text = "Give degree rights to the staff";
            lblnorec.Visible = true;
        }
        //Added by Subburaj 04/09/2014************//
        bindbranch();
        bindsem();
        binddegree();
        bindsec();
        GetTest();
        //******************End*****************//
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBoxother.Text = string.Empty;
        if (DropDownListpage.Text == "Others")
        {
            TextBoxother.Visible = true;
            TextBoxother.Focus();
        }
        else
        {
            TextBoxother.Visible = false;
            FpEntry.Visible = true;
            FpEntry.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
        }
        FpEntry.CurrentPage = 0;
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
                    FpEntry.CurrentPage = Convert.ToInt16(TextBoxpage.Text) - 1;
                    FpEntry.Visible = true;
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
            if (TextBoxother.Text.Trim() != "")
            {
                FpEntry.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString().Trim());
            }
        }
        catch
        {
            TextBoxother.Text = string.Empty;
        }
    }

    protected void chkselectall_CheckedChanged(object sender, EventArgs e)
    {
        if (chkselectall.Checked == true)
        {
            foreach (System.Web.UI.WebControls.ListItem li in chklist.Items)
            {
                li.Selected = true;
                txtdropdownlist.Text = "Criteria(" + (chklist.Items.Count) + ")";
                txtdropdownlist.Font.Bold = true;
            }
        }
        else
        {
            foreach (System.Web.UI.WebControls.ListItem li in chklist.Items)
            {
                li.Selected = false;
                txtdropdownlist.Text = "--Select--";
                txtdropdownlist.Font.Bold = true;
            }
        }
        ddlTest.SelectedIndex = -1;
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 28/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
    }

    protected void chklist_SelectedIndexChanged1(object sender, EventArgs e)
    {
        string value = string.Empty;
        string code = string.Empty;
        ddlTest.SelectedIndex = -1;
        for (int i = 0; i < chklist.Items.Count; i++)
        {
            if (chklist.Items[i].Selected == true)
            {
                value = chklist.Items[i].Text;
                code = chklist.Items[i].Value.ToString();
                categrycount = categrycount + 1;
                txtdropdownlist.Text = "Category(" + categrycount.ToString() + ")";
            }
        }
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 28/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
    }

    protected void txtdropdownlist_TextChanged(object sender, EventArgs e)
    {
        //   ddlTest.SelectedIndex = 0;
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        //ddlTest.SelectedIndex = -1;
        GetTest();
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        ddlpage.Visible = false;
        lblpages.Visible = false;
        //Buttontotal.Visible = false;
        //lblrecord.Visible = false;
        //DropDownListpage.Visible = false;
        //TextBoxother.Visible = false;
        //lblpage.Visible = false;
        //TextBoxpage.Visible = false;
        FpEntry.Visible = false;
        lblrptname.Visible = false;
        btnExcel.Visible = false;
        txtexcelname.Visible = false;
        Button1.Visible = false;
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        //ddlTest.SelectedIndex = -1;
        GetTest();
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        ddlpage.Visible = false;
        lblpages.Visible = false;
        FpEntry.Visible = false;
        lblrptname.Visible = false;
        btnExcel.Visible = false;
        txtexcelname.Visible = false;
        Button1.Visible = false;
        //Buttontotal.Visible = false;
        //lblrecord.Visible = false;
        //DropDownListpage.Visible = false;
        //TextBoxother.Visible = false;
        //lblpage.Visible = false;
        //TextBoxpage.Visible = false;
        // FpEntry.Visible = false;
    }

    protected void RadioHeader_CheckedChanged(object sender, EventArgs e)
    {
        function_radioheader();
    }

    public void function_radioheader()
    {
        ddlpage.Items.Clear();
        int totrowcount = FpEntry.Sheets[0].RowCount;
        int pages = totrowcount / 19;
        int intialrow = 1;
        int remainrows = totrowcount % 19;
        if (FpEntry.Sheets[0].RowCount > 0)
        {
            int i5 = 0;
            ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
            for (int i = 1; i <= pages; i++)
            {
                i5 = i;
                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                intialrow = intialrow + 19;
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
            //Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
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
                //   FpEntry.Height = 100 + (10 * Convert.ToInt32(totalRows));
            }
            //Buttontotal.Visible = true;
            //lblrecord.Visible = true;
            //DropDownListpage.Visible = true;
            //TextBoxother.Visible = false;
            //lblpage.Visible = true;
            //TextBoxpage.Visible = true;
            //RadioHeader.Visible = true;
            //Radiowithoutheader.Visible = true;
            //ddlpage.Visible = true;
            //lblpages.Visible = true;
        }
        else
        {
            //RadioHeader.Visible = false;
            //Radiowithoutheader.Visible = false;
            //ddlpage.Visible = false;
            //lblpages.Visible = false;
            //Buttontotal.Visible = false;
            //lblrecord.Visible = false;
            //DropDownListpage.Visible = false;
            //TextBoxother.Visible = false;
            //lblpage.Visible = false;
            //TextBoxpage.Visible = false;
        }
    }

    protected void Radiowithoutheader_CheckedChanged(object sender, EventArgs e)
    {
        ddlpage.Items.Clear();
        int totrowcount = FpEntry.Sheets[0].RowCount;
        int pages = totrowcount / 19;
        int intialrow = 1;
        int remainrows = totrowcount % 19;
        if (FpEntry.Sheets[0].RowCount > 0)
        {
            int i5 = 0;
            ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
            for (int i = 1; i <= pages; i++)
            {
                i5 = i;
                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                intialrow = intialrow + 19;
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
            // Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
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
            }
            //Buttontotal.Visible = true;
            //lblrecord.Visible = true;
            //DropDownListpage.Visible = true;
            //TextBoxother.Visible = false;
            //lblpage.Visible = true;
            //TextBoxpage.Visible = true;
            //RadioHeader.Visible = true;
            //Radiowithoutheader.Visible = true;
            //ddlpage.Visible = true;
            //lblpages.Visible = true;
        }
        else
        {
            //RadioHeader.Visible = false;
            //Radiowithoutheader.Visible = false;
            //ddlpage.Visible = false;
            //lblpages.Visible = false;
            //Buttontotal.Visible = false;
            //lblrecord.Visible = false;
            //DropDownListpage.Visible = false;
            //TextBoxother.Visible = false;
            //lblpage.Visible = false;
            //TextBoxpage.Visible = false;
        }
    }

    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        hat.Clear();
        hat.Add("college_code", Session["collegecode"].ToString());
        hat.Add("form_name", "CAT.aspx");
        dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        if (dsprint.Tables[0].Rows[0]["header_flag_value"].ToString() == "0")
        {
            // SpreadBind();
            for (int i = 0; i < FpEntry.Sheets[0].RowCount - 3; i++)
            {
                FpEntry.Sheets[0].Rows[i].Visible = false;
            }
            int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
            int end = start + 14;
            if (end >= FpEntry.Sheets[0].RowCount)
            {
                end = FpEntry.Sheets[0].RowCount;
            }
            int rowstart = FpEntry.Sheets[0].RowCount - Convert.ToInt32(start);
            int rowend = FpEntry.Sheets[0].RowCount - Convert.ToInt32(end);
            for (int i = start - 1; i < end; i++)
            {
                FpEntry.Sheets[0].Rows[i].Visible = true;
            }
            for (int h = 0; h < FpEntry.Sheets[0].ColumnHeader.RowCount; h++)   //visible the clmn header rowcount
            {
                FpEntry.Sheets[0].ColumnHeader.Rows[h].Visible = true;
                FpEntry.Sheets[0].ColumnHeader.Rows[9].Visible = false;
                FpEntry.Sheets[0].ColumnHeader.Rows[8].Visible = false;
            }
        }
        else if (dsprint.Tables[0].Rows[0]["header_flag_value"].ToString() == "1")
        {
            //  SpreadBind();
            for (int i = 0; i < FpEntry.Sheets[0].RowCount; i++)
            {
                FpEntry.Sheets[0].Rows[i].Visible = false;
            }
            int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
            int end = start + 14;
            if (end >= FpEntry.Sheets[0].RowCount)
            {
                end = FpEntry.Sheets[0].RowCount;
            }
            int rowstart = FpEntry.Sheets[0].RowCount - Convert.ToInt32(start);
            int rowend = FpEntry.Sheets[0].RowCount - Convert.ToInt32(end);
            for (int i = start - 1; i < end; i++)
            {
                FpEntry.Sheets[0].Rows[i].Visible = true;
            }
            if (Convert.ToInt32(ddlpage.SelectedValue.ToString()) == 1)
            {
                for (int h = 0; h < FpEntry.Sheets[0].ColumnHeader.RowCount; h++)
                {
                    FpEntry.Sheets[0].ColumnHeader.Rows[h].Visible = true;
                    FpEntry.Sheets[0].ColumnHeader.Rows[9].Visible = true;
                    FpEntry.Sheets[0].ColumnHeader.Rows[8].Visible = true;
                }
            }
            else
            {
                for (int h = 0; h < FpEntry.Sheets[0].ColumnHeader.RowCount; h++)
                {
                    FpEntry.Sheets[0].ColumnHeader.Rows[h].Visible = false;
                    FpEntry.Sheets[0].ColumnHeader.Rows[9].Visible = false;
                    FpEntry.Sheets[0].ColumnHeader.Rows[8].Visible = false;
                }
            }
        }
        else if (dsprint.Tables[0].Rows[0]["header_flag_value"].ToString() == "2")
        {
            for (int i = 0; i < FpEntry.Sheets[0].RowCount; i++)
            {
                FpEntry.Sheets[0].Rows[i].Visible = false;
            }
            int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
            int end = start + 14;
            if (end >= FpEntry.Sheets[0].RowCount)
            {
                end = FpEntry.Sheets[0].RowCount;
            }
            int rowstart = FpEntry.Sheets[0].RowCount - Convert.ToInt32(start);
            int rowend = FpEntry.Sheets[0].RowCount - Convert.ToInt32(end);
            for (int i = start - 1; i < end; i++)
            {
                FpEntry.Sheets[0].Rows[i].Visible = true;
            }
            for (int h = 0; h < FpEntry.Sheets[0].ColumnHeader.RowCount; h++)
            {
                FpEntry.Sheets[0].ColumnHeader.Rows[h].Visible = false;
                FpEntry.Sheets[0].ColumnHeader.Rows[8].Visible = false;
                FpEntry.Sheets[0].ColumnHeader.Rows[9].Visible = false;
            }
        }
        //'-----------------------------------------------------------------------------
        if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        {
            for (int i = 0; i < FpEntry.Sheets[0].RowCount; i++)
            {
                FpEntry.Sheets[0].Rows[i].Visible = true;
            }
            Double totalRows = 0;
            totalRows = Convert.ToInt32(FpEntry.Sheets[0].RowCount);
            Session["totalPages"] = (int)Math.Ceiling(totalRows / FpEntry.Sheets[0].PageSize);
            //Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
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
            //Buttontotal.Visible = false;
            //lblrecord.Visible = false;
            //DropDownListpage.Visible = false;
            //TextBoxother.Visible = false;
            //lblpage.Visible = false;
            //TextBoxpage.Visible = false;
        }
        if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
        {
            FpEntry.Sheets[0].ColumnHeader.Rows[9].Visible = false;
            FpEntry.Sheets[0].ColumnHeader.Rows[8].Visible = false;
        }
        else
        {
            FpEntry.Sheets[0].ColumnHeader.Rows[9].Visible = true;
            FpEntry.Sheets[0].ColumnHeader.Rows[8].Visible = true;
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        //Modified by Srinath 27/2/2013
        string reportname = txtexcelname.Text;
        if (reportname.ToString() != "")
        {
            dacces2.printexcelreport(FpEntry, reportname);
            lblerr.Visible = false;
        }
        else
        {
            lblerr.Text = "Please Enter Your Report Name";
            lblerr.Visible = true;
        }
    }
    
    protected void btnPrintMaster_Click(object sender, EventArgs e)
    {
        //PrintMaster = true;
        string selected_criteria = string.Empty;
        //     Session["page_redirect_value"] = ddlBatch.SelectedIndex + "," + ddlDegree.SelectedIndex + "," + ddlBranch.SelectedIndex + "," + ddlSemYr.SelectedIndex + "," + ddlSec.SelectedIndex + "," + ddlTest.SelectedIndex;
        string select_frm_date = txtFromDate.Text;
        string select_to_date = txtToDate.Text;
        btnGo_Click(sender, e);
        string clmnheadrname = string.Empty;
        string subhdrtext = string.Empty;
        int srtcnt = 0;
        int subheadrname = 0;
        int total_clmn_count = FpEntry.Sheets[0].ColumnCount;
        for (srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
        {
            if (srtcnt != 6)
            {
                if (clmnheadrname == "")
                {
                    clmnheadrname = FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
                }
                else
                {
                    clmnheadrname = clmnheadrname + "," + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
                }
            }
            else
            {
                for (subheadrname = 6; subheadrname <= 6 + child_sub_count - 1; subheadrname++)
                {
                    if (subhdrtext == "")
                    {
                        subhdrtext = clmnheadrname + "," + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, subheadrname].Text + "* ($" + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, subheadrname].Text;
                    }
                    else
                    {
                        subhdrtext = subhdrtext + "$" + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, subheadrname].Text;
                    }
                }
                clmnheadrname = subhdrtext + "$)";//assign the child with parent name to hdrname
                srtcnt = subheadrname - 1;
            }
        }
        string hdr_date = "From Date- " + txtFromDate.Text + "  To Date- " + txtToDate.Text;
        if (chklist.Items.Count > 0)
        {
            for (int criteria = 0; criteria < chklist.Items.Count; criteria++)
            {
                if (chklist.Items[criteria].Selected == true)
                {
                    if (selected_criteria == "")
                    {
                        selected_criteria = chklist.Items[criteria].Value;
                    }
                    else
                    {
                        selected_criteria = selected_criteria + "-" + chklist.Items[criteria].Value;
                    }
                }
            }
        }
        Session["page_redirect_value"] = ddlBatch.SelectedIndex + "," + ddlDegree.SelectedIndex + "," + ddlBranch.SelectedIndex + "," + ddlSemYr.SelectedIndex + "," + ddlSec.SelectedIndex + "," + ddlTest.SelectedIndex + "$" + selected_criteria.ToString() + "$" + select_frm_date + "$" + select_to_date;
        Response.Redirect("Print_Master_Setting.aspx?ID=" + clmnheadrname.ToString() + ":" + "CAT.aspx" + ":" + ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + ddlSemYr.SelectedItem.ToString() + "-" + ddlSec.SelectedItem.ToString() + ":" + "CAT REPORT" + ":" + hdr_date);
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = 2;
        //string filt_details = ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + "Sem " + ddlSemYr.SelectedItem.ToString() + "-" + "Sec " + ddlSec.SelectedItem.ToString();
        string filt_details = string.Empty;
        if (ddlSec.Enabled == true)
        {
            filt_details = ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + "Sem " + ddlSemYr.SelectedItem.ToString() + "-" + "Sec " + ddlSec.SelectedItem.ToString();
        }
        else
        {
            filt_details = ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + "Sem " + ddlSemYr.SelectedItem.ToString();
        }
        string date_filt = "From :" + txtFromDate.Text + "-" + "To :" + txtToDate.Text;
        string test = "Test :" + ddlTest.SelectedItem.ToString();
        string degreedetails = string.Empty;
        degreedetails = "CAM R6-CAT Report" + "@" + filt_details + "@" + date_filt + "@" + test;
        string pagename = "CAT.aspx";
        Printcontrol.loadspreaddetails(FpEntry, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
}
