using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;
using System.Text;

public partial class IndividualStudentCamReport : System.Web.UI.Page
{
    SqlConnection con_Getfunc = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_subcrd = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con_splhr_query_master = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlCommand cmd = new SqlCommand();
    //---------------------- Added By Rajesh
    FarPoint.Web.Spread.CheckBoxCellType chkboxsel_all = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
    //---------------------- Added By Rajesh
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
    string sqlpercmd, sqlsylcmd, sqlmarkcmd, sqlsubjcmd;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    string marks_per, marks_perfinal;
    string tnpsubno = string.Empty;
    string rollno;
    string rolnov, criteriain, subjectin;
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
    int countv = 0;
    string group_code = "", columnfield = string.Empty;
    int rocount = 0;
    string hcrollno = string.Empty;
    static Hashtable htb = new Hashtable();
    static Hashtable htcriteria = new Hashtable();
    string strorder = string.Empty;
    string strregorder = string.Empty;
    string studname = string.Empty;
    string latmode = string.Empty;
    string regn = string.Empty;
    static Hashtable htsubjcide = new Hashtable();
    Hashtable hat = new Hashtable();
    Hashtable htv = new Hashtable();
    Hashtable htv3 = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet dsbind = new DataSet();
    DataTable dt = new DataTable();
    DataSet dsmethodgoper = new DataSet();
    DataSet dsmethodgosubj = new DataSet();
    DataSet dsmethodgocriteria = new DataSet();
    DataSet dsmethodgomark = new DataSet();
    DataSet tempdssubj = new DataSet();
    DataSet dsprint = new DataSet();
    DataSet dsuni = new DataSet();
    string rnkv = string.Empty;
    string rankov3 = string.Empty;
    string syll_code = string.Empty;
    string examcodevalg = string.Empty;
    DataSet dggradetot = new DataSet();
    DataSet dggradegra = new DataSet();
    DataSet dssem = new DataSet();
    int gtempejval = 0;
    string strgradetempgrade = string.Empty;
    string strtotgrac = string.Empty;
    double strtot = 0;
    double strgradetempfrm = 0;
    double strgradetempto = 0;
    string tempexmonth = string.Empty;
    string tempexyear = string.Empty;
    string gtempexmonth = string.Empty;
    string gtempexyear = string.Empty;
    string sqlStr = string.Empty;
    string sections = string.Empty;
    string batch = string.Empty;
    string degreecode = string.Empty;
    string subno = string.Empty;
    string semester = string.Empty;
    int quota_count;
    string exam_code = string.Empty;
    string criteria_no = string.Empty;
    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    int countds = 0;
    int next = 0;
    int minpresII = 0;
    int gs_count, bs_count, eod_count, tot_stu, x1;
    int count = 0;
    string failv = string.Empty;
    int min_mark, per_sub_count;
    double per_mark;
    int passcount, failcount, maxcount, mincount, avg_50count, avg_65count, pre_count, ab_count;
    int pass = 0, fail = 0;
    int mmyycount;
    int count_has = 0;
    int sub_code = 0;
    double tot_marks;
    double per_marks;
    double percen;
    string pass_fail, per_tage;
    double sub_max_marks;
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;
    int notconsider_value = 0;
    double conduct_hour_new = 0;
    int cal_from_date;
    int cal_to_date, start_column = 0;
    DateTime per_from_date;
    DateTime per_to_date;
    DateTime dumm_from_date;
    DateTime Admission_date;
    int i, rows_count;
    string dd = string.Empty;
    int moncount;
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    string halforfull = "", mng = "", evng = "", holiday_sched_details = string.Empty;
    TimeSpan ts;
    Boolean splhr_flag = false;
    string diff_date;
    double dif_date1 = 0;
    string value_holi_status = string.Empty;
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = string.Empty;
    double dif_date = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    string value, date;
    double per_perhrs, per_abshrs, cum_perhrs, cum_abshrs;
    double per_ondu, per_leave, per_hhday, cum_ondu, cum_leave, cum_hhday;
    int tot_per_hrs, per_tot_per_hrs, cum_per_tot_per, tot_wok_hrs;
    double njhr, njdate, per_njhr, per_njdate, cum_njhr, cum_njdate;
    double workingdays = 0;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    double halfday = 0;
    string tempvalue = "-1";
    int ObtValue = -1;
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double cum_present_date, cum_ondu_date, cum_leave_date, cum_absent_date;
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0, tot_ml_spl_fals = 0;
    double per_leave_fals = 0;
    int per_abshrs_spl_true = 0, tot_per_hrs_spl_true = 0, tot_conduct_hr_spl_true = 0, tot_ondu_spl_true = 0, tot_ml_spl_true = 0;
    double per_leave_true = 0;
    int per_dum_unmark, cum_dum_unmark, dum_unmark;
    int dum_diff_date, unmark;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0, tot_ml_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int check;
    double tot_ondu, per_tot_ondu, cum_tot_ondu, cum_tot_ml, tot_ml, per_tot_ml;
    double per_per_hrs, cum_per_perhrs;
    double per_workingdays = 0;
    double cum_tot_point, per_holidate, cum_per_holidate;
    double per_con_hrs, cum_con_hrs;
    double per_tage_date, cum_tage_date, per_tage_hrs, cum_tage_hrs;
    string admdatev = "", strtdate = "", examdate = string.Empty;
    string dum_tage_date, dum_tage_hrs;
    string dum_cum_tage_date, dum_cum_tage_hrs;
    double cum_workingdays = 0;
    DataSet ds_attnd_pts = new DataSet();
    DataTable data = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        errmsg.Visible = false;
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
        collegecode = Session["InternalCollegeCode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        lblerr.Visible = false;
        string strlongdate = DateTime.Now.ToLongDateString();
        string strlongdate1 = DateTime.Now.ToShortDateString();
        if (!IsPostBack)
        {
            //--------Spread Design Format-----------
            cvl.Visible = false;
            Showgrid.Visible = false;

            //FpSpread2.Visible = false;
            btnxl.Visible = false;
            //Added By Srinath 28/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            btnprint.Visible = false;
            chkGridSelectAll.Visible = false;
            //btnprint1.Visible = false;
            norecordlbl.Visible = false;
            bindcollege(sender, e);
            if (ddlcollege.Items.Count >= 1)
            {
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                if (ddldegree.Items.Count > 0)
                {
                    BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                    BindSem(strbranch, strbatchyear, collegecode);
                    BindSectionDetail(strbatch, strbranch);
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Give degree rights to staff";
                }
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Give college rights to staff";
            }
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

    public void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlbranch.Text == "")
            {
                errmsg.Text = "Give degree rights to staff";
                errmsg.Visible = true;
                return;
            }
            if (ddlsection.Text.ToString() == "All" || ddlsection.Text.ToString() == string.Empty || ddlsection.Text.ToString() == "-1")
            {
                strsec = string.Empty;
                strsec1 = string.Empty;
                strsecmark = string.Empty;
            }
            else
            {
                strsec = " and registration.sections='" + ddlsection.Text.ToString() + "'";
                strsec1 = " and sections='" + ddlsection.Text.ToString() + "'";
                strsecmark = "and re.sections='" + ddlsection.Text.ToString() + "'";
            }
            errmsg.Visible = false;
            errmsg.Text = string.Empty;
            filteration();
            sqlpercmd = "select ROW_NUMBER() OVER (ORDER BY  Roll_no) As SrNo,roll_no,reg_no,registration.stud_name as studname,registration.stud_type as studtype,sio.start_date as startdate,registration.Adm_Date as admdate,registration.mode as Mode from seminfo sio,registration inner join applyn on applyn.app_no = registration.app_no where registration.degree_code=sio.degree_code and registration.batch_year=sio.batch_year and registration.current_semester=sio.semester  and registration.degree_code='" + ddlbranch.SelectedValue + "'   " + strsec + "  and  RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' and registration.batch_year='" + ddlbatch.SelectedValue + "' " + strregorder + "";
            methodgo(sqlpercmd);
        }
        catch
        {
        }
    }

    public void methodgo(string sqlfirstcmd)
    {
        Hashtable htpass = new Hashtable();
        Hashtable htfail = new Hashtable();

        DataRow drow;
        ArrayList arrColHdrNames1 = new ArrayList();
        try
        {

            arrColHdrNames1.Add("S.No");
            arrColHdrNames1.Add("RollNo");
            arrColHdrNames1.Add("RegNo");
            arrColHdrNames1.Add("Student Name");
            arrColHdrNames1.Add("Student Type");
            arrColHdrNames1.Add("StartDate");
            arrColHdrNames1.Add("AdmDate");
            arrColHdrNames1.Add("Mode");

            data.Columns.Add("Sno");
            data.Columns.Add("RollNo");
            data.Columns.Add("RegNo");
            data.Columns.Add("stdname");
            data.Columns.Add("stdtype");
            data.Columns.Add("StartDate");
            data.Columns.Add("AdmDate");
            data.Columns.Add("Mode");

            DataRow drHdr1 = data.NewRow();
            for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                drHdr1[grCol] = arrColHdrNames1[grCol];

            data.Rows.Add(drHdr1);

            if (chklstsubject.Items.Count > 0)
            {
                if (cbltest.Items.Count > 0)
                {
                    errmsg.Visible = false;
                    btnprint.Visible = false;
                    chkGridSelectAll.Visible = false;
                    //btnprint1.Visible = false;
                    norecordlbl.Visible = false;
                    hat.Clear();
                    dsmethodgoper.Dispose();
                    dsmethodgoper.Reset();
                    dsmethodgoper.Clear();
                    dsmethodgoper = d2.select_method(sqlfirstcmd, hat, "Text");
                    if (dsmethodgoper != null && dsmethodgoper.Tables[0] != null && dsmethodgoper.Tables[0].Rows.Count > 0)
                    {
                        rocount = dsmethodgoper.Tables[0].Rows.Count;

                        int cn = 0;
                        for (int bindval = 0; bindval < dsmethodgoper.Tables[0].Rows.Count; bindval++)
                        {
                            cn++;
                            drow = data.NewRow();
                            drow["Sno"] = cn.ToString();
                            drow["RollNo"] = dsmethodgoper.Tables[0].Rows[bindval]["Roll_No"].ToString();
                            drow["RegNo"] = dsmethodgoper.Tables[0].Rows[bindval]["Reg_No"].ToString();
                            drow["stdname"] = dsmethodgoper.Tables[0].Rows[bindval]["studname"].ToString();
                            drow["stdtype"] = dsmethodgoper.Tables[0].Rows[bindval]["studtype"].ToString();
                            drow["StartDate"] = dsmethodgoper.Tables[0].Rows[bindval]["startdate"].ToString();
                            drow["AdmDate"] = dsmethodgoper.Tables[0].Rows[bindval]["admdate"].ToString();
                            drow["Mode"] = dsmethodgoper.Tables[0].Rows[bindval]["Mode"].ToString();
                            data.Rows.Add(drow);

                        }

                        if (data.Columns.Count > 0 && data.Rows.Count > 1)
                        {

                            Showgrid.DataSource = data;
                            Showgrid.DataBind();
                            Showgrid.Visible = true;
                            btnprint.Visible = true;
                            chkGridSelectAll.Visible = true;


                            Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            Showgrid.Rows[0].Font.Bold = true;
                            Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                        }



                    }
                    else
                    {
                        norecordlbl.Text = "No Record Found";
                        norecordlbl.Visible = true;
                    }
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Choose Test";
                }
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Choose Subject";
            }
        }
        catch (Exception e)
        {
            errmsg.Visible = true;
            errmsg.Text = e.ToString();
        }
    }

    protected void Showgrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //((CheckBox)e.Row.FindControl("allchk")).Attributes.Add("onclick",
            //    "javascript:SelectAll('" +
            //    ((CheckBox)e.Row.FindControl("allchk")).ClientID + "')");
            for (int grCol = 0; grCol < Showgrid.Columns.Count; grCol++)
                e.Row.Cells[grCol].Visible = false;
            //e.Row.Cells[5].Visible = false;

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                //CheckBox cbsel = (CheckBox)e.Row.Cells[5].FindControl("selectchk");
                //cbsel.Visible = false;
                //cbsel.Text = "Select";

                e.Row.Cells[6].Text = "Select";
            }
        }

    }


    public void btnprint_Click(object sender, EventArgs e)
    {

        try
        {
            string faclty = string.Empty;
            contentDiv.InnerHtml = "";
            StringBuilder html = new StringBuilder();
            StringBuilder html1 = new StringBuilder();
            StringBuilder htmlfinal = new StringBuilder();
            if (chklstsubject.Items.Count > 0)
            {
                if (cbltest.Items.Count > 0)
                {
                    Showgrid.Visible = false;
                    int cnt = 0;
                    foreach (GridViewRow gvrow in Showgrid.Rows)
                    {
                        System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                        if (chk.Checked == true)
                        {
                            cnt++;
                        }
                    }
                    #region Cmd
                    //for (int res = 0; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                    //{
                    //    int isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 8].Value);
                    //    if (isval == 1)
                    //    {
                    //        cnt++;
                    //    }
                    //}

                    //if (cnt == 1)
                    //{
                    //    foreach (GridViewRow gvrow in Showgrid.Rows)
                    //    {
                    //        System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    //        if (chk.Checked == true)
                    //        {
                    //            cnt++;
                    //            errmsg.Visible = false;
                    //            btnprint.Visible = false;
                    //            //btnprint1.Visible = false;
                    //            norecordlbl.Visible = false;
                    //            Label rollno = (Label)gvrow.Cells[1].FindControl("RollNo");
                    //            hcrollno = rollno.Text;
                    //            Label regno = (Label)gvrow.Cells[1].FindControl("RegNo");
                    //            regn = regno.Text;
                    //            Label stdname = (Label)gvrow.Cells[1].FindControl("stdname");
                    //            studname = stdname.Text;
                    //            Label stdate = (Label)gvrow.Cells[1].FindControl("StartDate");
                    //            strtdate = stdate.Text;
                    //            Label admdate = (Label)gvrow.Cells[1].FindControl("AdmDate");
                    //            admdatev = admdate.Text;
                    //            Label mod = (Label)gvrow.Cells[1].FindControl("Mode");
                    //            latmode = mod.Text;


                    //            //hcrollno = Convert.ToString(FpSpread2.Sheets[0].Cells[res, 1].Text);
                    //            //regn = Convert.ToString(FpSpread2.Sheets[0].Cells[res, 2].Text);
                    //            //studname = Convert.ToString(FpSpread2.Sheets[0].Cells[res, 3].Text);
                    //            //strtdate = Convert.ToString(FpSpread2.Sheets[0].Cells[res, 5].Text);
                    //            //admdatev = Convert.ToString(FpSpread2.Sheets[0].Cells[res, 6].Text);
                    //            //latmode = Convert.ToString(FpSpread2.Sheets[0].Cells[res, 7].Text);
                    //            DateTime admdv = Convert.ToDateTime(admdatev);
                    //            int admday = admdv.Day;
                    //            int admmonth = admdv.Month;
                    //            int admyear = admdv.Year;
                    //            admdatev = admday + "/" + admmonth + "/" + admyear;
                    //            DateTime startdv = Convert.ToDateTime(strtdate);
                    //            int stday = startdv.Day;
                    //            int stmonth = startdv.Month;
                    //            int styear = startdv.Year;
                    //            strtdate = stday + "/" + stmonth + "/" + styear;
                    //            int ini_column = 0;
                    //            int no_column = 0;
                    //            int tcolcount = 0;
                    //            Hashtable criteriatothat = new Hashtable();
                    //            //and ss.class_advisor like '%'+sm.staff_code+'%' By Raja T
                    //            if (ddlsection.Enabled == true)
                    //            {
                    //                faclty = d2.GetFunctionv("SELECT STUFF((select distinct ','+staff_name from semester_schedule ss, staffmaster sm where batch_year='" + ddlbatch.SelectedItem.Text + "' and  degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlsemester.SelectedItem.Text + "' and sections='" + ddlsection.SelectedItem.Text + "' and ss.class_advisor like '%'+sm.staff_code+'%' FOR XML PATH('')),1,1,'') -- and sm.staff_code=ss.class_advisor");
                    //            }
                    //            else
                    //            {
                    //                faclty = d2.GetFunctionv("SELECT STUFF((select distinct ','+ staff_name from  semester_schedule ss, staffmaster sm where batch_year='" + ddlbatch.SelectedItem.Text + "' and  degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlsemester.SelectedItem.Text + "' and ss.class_advisor like '%'+sm.staff_code+'%' FOR XML PATH('')),1,1,'') -- and sm.staff_code=ss.class_advisor");
                    //            }

                    //            html.Append("<center> <div style='height: 990px; width: 100%; border: 0px solid black; margin-left: 5px; margin: 0px; page-break-after: always;'> <center><div style='border: 1px solid black'>  <center> <table style='width: 95%; margin-top: 0px; margin-bottom: 2px; font-size: medium;' cellpadding='5' cellspacing='0'> ");
                    //            html.Append("<tr style='font-size:18px;'><td style='align: center;'>S.No</td><td style='align: center;'>Subject Code</td><td style='align: center;'>Subject Name</td><td style='align: center;'>TH/PR</td>");



                    //            //html.Append("</table> </center></div>  </center> </div></center>");

                    //            if (ddlsection.Text.ToString().ToLower().Trim() == "all" || ddlsection.Text.ToString().Trim() == string.Empty || ddlsection.Text.ToString().Trim() == "-1")
                    //            {
                    //                strsec1 = string.Empty;
                    //            }
                    //            else
                    //            {
                    //                strsec1 = " and sections='" + ddlsection.Text.ToString().Trim() + "'";
                    //            }
                    //            ini_column = FpSpread1.Sheets[0].ColumnCount;
                    //            no_column = 0;
                    //            criteriain = string.Empty;
                    //            for (int test = 0; test < cbltest.Items.Count; test++)
                    //            {
                    //                if (cbltest.Items[test].Selected == true)
                    //                {
                    //                    criteriatot = criteriatot + 1;
                    //                    html.Append("<td>" + Convert.ToString(cbltest.Items[test].Text.ToString()) + "</td>");

                    //                    //  FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].Note = Convert.ToString(cbltest.Items[test].Value);

                    //                    no_column = no_column + 1;
                    //                    if (criteriain == null || criteriain == "")
                    //                        criteriain = "'" + FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].Note + "'";
                    //                    else
                    //                        criteriain = criteriain + "," + "'" + FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].Note + "'";
                    //                }
                    //            }
                    //            tcolcount = FpSpread1.Sheets[0].ColumnCount;
                    //            if (criteriain != "")
                    //            {
                    //                criteriain = " in(" + criteriain + ")";
                    //            }
                    //            criteriain = " and e.criteria_no  " + criteriain + "";
                    //            html.Append("<td>Session Marks</td>");
                    //            html.Append("<td>University Exam Marks / Grade</td>");
                    //            html.Append("<td>Total</td>");
                    //            html.Append("</tr>");
                    //            html.Append("<tr style='font-size:18px;'><td></td><td></td><td style='align: center;'>Max.Marks</td>");

                    //            FpSpread1.Sheets[0].Cells[9, FpSpread1.Sheets[0].ColumnCount - 1].Text = "20 %";
                    //            FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //            FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].Cells[9, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[9, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[9, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                    //            FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].Text = "University Exam Marks / Grade";
                    //            FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //            FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].Cells[9, FpSpread1.Sheets[0].ColumnCount - 1].Text = "80 %";
                    //            FpSpread1.Sheets[0].Cells[9, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[9, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[9, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                    //            FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                    //            FpSpread1.Sheets[0].Cells[9, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[9, FpSpread1.Sheets[0].ColumnCount - 1].Text = "100 ";
                    //            FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //            FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[8, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].Cells[9, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[9, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[9, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //            if (chklstsubject.Items.Count > 0)
                    //            {
                    //                for (int subj = 0; subj < chklstsubject.Items.Count; subj++)
                    //                {
                    //                    if (chklstsubject.Items[subj].Selected == true)
                    //                    {
                    //                        if (no_column != 0)
                    //                        {
                    //                            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = (subj + 1).ToString();
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    //                            string subjtypevel = Convert.ToString(GetCorrespondingKey(Convert.ToString(chklstsubject.Items[subj].Value), htsubjcide));
                    //                            string subjcodde = d2.GetFunction("select subject_code from subject where subject_no='" + chklstsubject.Items[subj].Value + "'");
                    //                            // string subjtypevel = Convert.ToString(GetCorrespondingKey(Convert.ToString(chklstsubject.Items[subj].Value), htsubjcide));
                    //                            subjtypevel = subjcodde;
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(subjtypevel);
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(chklstsubject.Items[subj].Text.ToString());
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    //                            string subjcode = Convert.ToString(chklstsubject.Items[subj].Value);
                    //                            string thpr = string.Empty;
                    //                            if (htb.Contains(Convert.ToString(subjcode)))
                    //                            {
                    //                                thpr = Convert.ToString(GetCorrespondingKey(Convert.ToString(subjcode), htb));
                    //                            }
                    //                            else
                    //                            {
                    //                                thpr = "-";
                    //                            }
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString((thpr));
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    //                            subjectctot = subjectctot + 1;
                    //                            sqlmarkcmd = "select distinct r.marks_obtained,isnull(e.min_mark,0) as min_mark,r.roll_no,e.exam_code,re.roll_no,criteria_no ,Len(r.roll_no),e.max_mark from result r,exam_type e,registration re where r.roll_no=re.roll_no and e.exam_code=r.exam_code and  r.roll_no='" + hcrollno + "'  and e.subject_no='" + subjcode + "' " + criteriain + " and  RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by Len(r.roll_no),e.criteria_no";
                    //                            dsmethodgomark = d2.select_method(sqlmarkcmd, hat, "Text");
                    //                            if (dsmethodgomark != null && dsmethodgomark.Tables[0] != null && dsmethodgomark.Tables[0].Rows.Count > 0)
                    //                            {
                    //                                int minmark = 0;
                    //                                double markob = 0;
                    //                                double markforad = 0;
                    //                                string criterianotemp = "", mcriteriano = string.Empty;
                    //                                foreach (DataRow dr in dsmethodgomark.Tables[0].Rows)
                    //                                {
                    //                                    criterianotemp = dr["criteria_no"].ToString();
                    //                                    for (int k = 4; k <= tcolcount - 1; k++)
                    //                                    {
                    //                                        mcriteriano = Convert.ToString(FpSpread1.Sheets[0].Cells[8, k].Note);
                    //                                        if (criterianotemp != "" && mcriteriano != "" && criterianotemp == mcriteriano)
                    //                                        {
                    //                                            FpSpread1.Sheets[0].Cells[9, k].HorizontalAlign = HorizontalAlign.Center;
                    //                                            FpSpread1.Sheets[0].Cells[9, k].Text = dr["max_mark"].ToString();
                    //                                            minmark = Convert.ToInt32(dr["min_mark"]);
                    //                                            markob = Convert.ToDouble(dr["marks_obtained"]);
                    //                                            if (markob < 0)
                    //                                            {
                    //                                                markforad = 0;
                    //                                            }
                    //                                            else
                    //                                            {
                    //                                                markforad = markob;
                    //                                            }
                    //                                            marks_per = Convert.ToString(dr["marks_obtained"]);
                    //                                            double markobta = 0;
                    //                                            if (markob >= minmark)
                    //                                            {
                    //                                            }
                    //                                            else
                    //                                            {
                    //                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, k].ForeColor = Color.Red;
                    //                                                if (htv3.Contains(Convert.ToString(mcriteriano)))
                    //                                                {
                    //                                                    string failvtr = Convert.ToString(GetCorrespondingKey(Convert.ToString(mcriteriano), htv3));
                    //                                                    failvtr = failvtr + 1;
                    //                                                    htv3[Convert.ToString(mcriteriano)] = failvtr;
                    //                                                }
                    //                                                else
                    //                                                {
                    //                                                    htv3.Add(Convert.ToString(mcriteriano), 1);
                    //                                                }
                    //                                            }
                    //                                            if (criteriatothat.Contains(Convert.ToString(mcriteriano)))
                    //                                            {
                    //                                                markobta = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(mcriteriano), criteriatothat));
                    //                                                markobta = markobta + markob;
                    //                                                criteriatothat[Convert.ToString(mcriteriano)] = markobta;
                    //                                            }
                    //                                            else
                    //                                            {
                    //                                                criteriatothat.Add(Convert.ToString(mcriteriano), markforad);
                    //                                            }
                    //                                            switch (marks_per)
                    //                                            {
                    //                                                case "-1":
                    //                                                    marks_perfinal = "AAA";
                    //                                                    break;
                    //                                                case "-2":
                    //                                                    marks_perfinal = "EL";
                    //                                                    break;
                    //                                                case "-3":
                    //                                                    marks_perfinal = "EOD";
                    //                                                    break;
                    //                                                case "-4":
                    //                                                    marks_perfinal = "ML";
                    //                                                    break;
                    //                                                case "-5":
                    //                                                    marks_perfinal = "SOD";
                    //                                                    break;
                    //                                                case "-6":
                    //                                                    marks_perfinal = "NSS";
                    //                                                    break;
                    //                                                case "-7":
                    //                                                    marks_perfinal = "NJ";
                    //                                                    break;
                    //                                                case "-8":
                    //                                                    marks_perfinal = "S";
                    //                                                    break;
                    //                                                case "-9":
                    //                                                    marks_perfinal = "L";
                    //                                                    break;
                    //                                                case "-10":
                    //                                                    marks_perfinal = "NCC";
                    //                                                    break;
                    //                                                case "-11":
                    //                                                    marks_perfinal = "HS";
                    //                                                    break;
                    //                                                case "-12":
                    //                                                    marks_perfinal = "PP";
                    //                                                    break;
                    //                                                case "-13":
                    //                                                    marks_perfinal = "SYOD";
                    //                                                    break;
                    //                                                case "-14":
                    //                                                    marks_perfinal = "COD";
                    //                                                    break;
                    //                                                case "-15":
                    //                                                    marks_perfinal = "OOD";
                    //                                                    break;
                    //                                                case "-16":
                    //                                                    marks_perfinal = "OD";
                    //                                                    break;
                    //                                                //*********Modified by Subburaj 21.08.2014**********//
                    //                                                case "-18":
                    //                                                    marks_perfinal = "RAA";
                    //                                                    break;
                    //                                                //**************End********************//
                    //                                                default:
                    //                                                    marks_perfinal = marks_per;
                    //                                                    break;
                    //                                            }
                    //                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, k].HorizontalAlign = HorizontalAlign.Center;
                    //                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, k].Text = marks_perfinal.ToString();
                    //                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, k].HorizontalAlign = HorizontalAlign.Center;
                    //                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, k].VerticalAlign = VerticalAlign.Middle;
                    //                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, k].Font.Size = FontUnit.Medium;
                    //                                        }
                    //                                        if (tcolcount >= k + 1 && tcolcount <= k + 1)
                    //                                        {
                    //                                            string sqlcmduni = "select internal_mark,external_mark,grade from mark_entry m,exam_details e where e.exam_code=m.exam_code and roll_no='" + hcrollno + "' and subject_no='" + subjcode + "' and degree_code='" + ddlbranch.SelectedValue + "' and batch_year='" + ddlbatch.SelectedValue + "' and current_semester='" + ddlsemester.SelectedValue + "'";
                    //                                            dsuni = d2.select_method(sqlcmduni, hat, "Text");
                    //                                            if (dsuni != null && dsuni.Tables[0] != null && dsuni.Tables[0].Rows.Count > 0)
                    //                                            {
                    //                                                int intermark = 0;
                    //                                                int extermark = 0;
                    //                                                string coeintmark = dsuni.Tables[0].Rows[0]["internal_mark"].ToString();
                    //                                                string coeexttmark = dsuni.Tables[0].Rows[0]["external_mark"].ToString();
                    //                                                if (coeintmark.Trim() != "" && coeintmark != null)
                    //                                                {
                    //                                                    intermark = Convert.ToInt32(dsuni.Tables[0].Rows[0]["internal_mark"].ToString());
                    //                                                }
                    //                                                if (coeexttmark.Trim() != "" && coeexttmark != null)
                    //                                                {
                    //                                                    extermark = Convert.ToInt32(dsuni.Tables[0].Rows[0]["external_mark"].ToString());
                    //                                                }
                    //                                                string strgrade = Convert.ToString(dsuni.Tables[0].Rows[0]["grade"].ToString());
                    //                                                if (coeintmark.Trim() != "" && coeintmark != null && coeexttmark.Trim() != "" && coeexttmark != null)
                    //                                                {
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].Text = dsuni.Tables[0].Rows[0]["internal_mark"].ToString();
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = dsuni.Tables[0].Rows[0]["external_mark"].ToString();
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = (intermark + extermark).ToString();
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //                                                }
                    //                                                else //if (strgrade != "" && strgrade != null && intermark != 0 && intermark != null)
                    //                                                {
                    //                                                    string strintsd = dsuni.Tables[0].Rows[0]["internal_mark"].ToString();
                    //                                                    if (coeintmark == "" || coeintmark == "0")
                    //                                                    {
                    //                                                        strintsd = d2.GetFunctionv("select total from camarks where roll_no='" + hcrollno + "' and subject_no='" + subjcode + "'");
                    //                                                        if (strintsd == "")
                    //                                                        {
                    //                                                            strintsd = "-";
                    //                                                        }
                    //                                                    }
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].Text = strintsd;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = dsuni.Tables[0].Rows[0]["grade"].ToString();
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = dsuni.Tables[0].Rows[0]["grade"].ToString();
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    //                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //                                                }
                    //                                            }
                    //                                        }
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                        else
                    //                        {
                    //                            errmsg.Visible = true;
                    //                            errmsg.Text = "Please Choose Test";
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //            else
                    //            {
                    //                errmsg.Visible = true;
                    //                errmsg.Text = "Please Choose Subject";
                    //            }
                    //            tottet = ((subjectctot * no_column));
                    //            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    //            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total Marks Obtained";
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    //            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Maximum Marks";
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    //            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ">60 %";
                    //            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 2, 1, 2);
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = " % of Marks";
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    //            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Class Rank";
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    //            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Class - Attended Hours";
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    //            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Attendance - Total Hours Conducted";
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    //            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ">85 %";
                    //            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 2, 1, 2);
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Attendance %";
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    //            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Progress Card Sent to the Parents on";
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    //            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Student's Signature";
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Height = 100 + (10 * Convert.ToInt32(FpSpread1.Sheets[0].RowCount));
                    //            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    //            for (int z = 4; z <= tcolcount - 1; z++)
                    //            {
                    //                double tot = 0;
                    //                string mcriteriano1 = Convert.ToString(FpSpread1.Sheets[0].Cells[8, z].Note);
                    //                if (mcriteriano1 != "" && mcriteriano1 != null)
                    //                {
                    //                    if (Convert.ToDouble(GetCorrespondingKey(Convert.ToString(mcriteriano1), criteriatothat)) != null && Convert.ToDouble(GetCorrespondingKey(Convert.ToString(mcriteriano1), criteriatothat)) != 0)
                    //                    {
                    //                        tot = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(mcriteriano1), criteriatothat));
                    //                    }
                    //                    else
                    //                    {
                    //                        tot = 0;
                    //                    }
                    //                    examdate = GetFunction("select min(exam_date) from exam_type where criteria_no='" + mcriteriano1 + "' " + strsec1 + "");
                    //                    DateTime examdv = Convert.ToDateTime(examdate);
                    //                    int examday = examdv.Day;
                    //                    int exammonth = examdv.Month;
                    //                    int examyear = examdv.Year;
                    //                    examdate = examday + "/" + exammonth + "/" + examyear;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 9, z].HorizontalAlign = HorizontalAlign.Center;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 9, z].Text = tot.ToString();
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 9, z].HorizontalAlign = HorizontalAlign.Center;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 9, z].VerticalAlign = VerticalAlign.Middle;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 9, z].Font.Size = FontUnit.Medium;
                    //                    int maxv = 0;
                    //                    if (FpSpread1.Sheets[0].Cells[9, z].Text != null && FpSpread1.Sheets[0].Cells[9, z].Text != "")
                    //                    {
                    //                        if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[9, z].Text) != null)
                    //                        {
                    //                            maxv = Convert.ToInt32(FpSpread1.Sheets[0].Cells[9, z].Text);
                    //                        }
                    //                        else
                    //                        {
                    //                            maxv = 0;
                    //                        }
                    //                    }
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 8, z].HorizontalAlign = HorizontalAlign.Center;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 8, z].VerticalAlign = VerticalAlign.Middle;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 8, z].Font.Size = FontUnit.Medium;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 8, z].Text = (subjectctot * maxv).ToString();
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 7, z].HorizontalAlign = HorizontalAlign.Center;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 7, z].VerticalAlign = VerticalAlign.Middle;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 7, z].Font.Size = FontUnit.Medium;
                    //                    decimal tempperce = Convert.ToDecimal(tot / subjectctot);
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 7, z].Text = Convert.ToString(Decimal.Parse(tempperce.ToString("0.00")));
                    //                    failv = Convert.ToString(GetCorrespondingKey(Convert.ToString(mcriteriano1), htv3));
                    //                    if (failv == "")
                    //                    {
                    //                        rankov3 = overallperformprint(mcriteriano1, hcrollno);
                    //                    }
                    //                    else
                    //                    {
                    //                        rankov3 = "-";
                    //                    }
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 6, z].VerticalAlign = VerticalAlign.Middle;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 6, z].Font.Size = FontUnit.Medium;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 6, z].HorizontalAlign = HorizontalAlign.Center;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 6, z].Text = rankov3.ToString();
                    //                    con.Close();
                    //                    con.Open();
                    //                    string attnd_points = "select *from leave_points";
                    //                    SqlDataAdapter da_attnd_pts;
                    //                    da_attnd_pts = new SqlDataAdapter(attnd_points, con);
                    //                    da_attnd_pts.Fill(ds_attnd_pts);
                    //                    if (ds_attnd_pts.Tables[0].Rows.Count > 0)
                    //                    {
                    //                        holi_leav = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["leave_bef_aft"].ToString());
                    //                        holi_absent = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["absent_bef_aft"].ToString());
                    //                        leav_pt = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["leave"].ToString());
                    //                        absent_pt = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["absent"].ToString());
                    //                    }
                    //                    ds1.Dispose();
                    //                    ds1.Reset();
                    //                    hat.Clear();
                    //                    hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
                    //                    hat.Add("sem_ester", int.Parse(ddlsemester.SelectedValue.ToString()));
                    //                    ds = d2.select_method("period_attnd_schedule", hat, "sp");
                    //                    if (ds.Tables[0].Rows.Count != 0)
                    //                    {
                    //                        NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                    //                        fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                    //                        anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                    //                        minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                    //                        minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                    //                    }
                    //                    hat.Clear();
                    //                    hat.Add("colege_code", Session["collegecode"].ToString());
                    //                    ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
                    //                    count = ds1.Tables[0].Rows.Count;
                    //                    persentmonthcal(examdate, strtdate);
                    //                    per_con_hrs = per_workingdays1;
                    //                    per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / per_con_hrs) * 100);
                    //                    if (per_per_hrs != null && tot_per_hrs_spl_fals != null)
                    //                    {
                    //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 5, z].HorizontalAlign = HorizontalAlign.Center;
                    //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 5, z].Text = (per_per_hrs + tot_per_hrs_spl_fals).ToString();
                    //                    }
                    //                    if (per_con_hrs != null && tot_conduct_hr_spl_fals != null)
                    //                    {
                    //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, z].HorizontalAlign = HorizontalAlign.Center;
                    //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, z].Text = (per_con_hrs + tot_conduct_hr_spl_fals).ToString(); //per_con_hrs.ToString();
                    //                    }
                    //                    if (per_tage_hrs != null && Convert.ToString(per_tage_hrs) != "" && Convert.ToString(per_tage_hrs) != "NaN")
                    //                    {
                    //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, z].HorizontalAlign = HorizontalAlign.Center;
                    //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, z].Text = Convert.ToString(Decimal.Parse(per_tage_hrs.ToString("0.00")));
                    //                    }
                    //                    else
                    //                    {
                    //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, z].HorizontalAlign = HorizontalAlign.Center;
                    //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, z].Text = Convert.ToString("-");
                    //                    }
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 5, z].VerticalAlign = VerticalAlign.Middle;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 5, z].Font.Size = FontUnit.Medium;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, z].VerticalAlign = VerticalAlign.Middle;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, z].Font.Size = FontUnit.Medium;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, z].VerticalAlign = VerticalAlign.Middle;
                    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, z].Font.Size = FontUnit.Medium;
                    //                }
                    //            }
                    //            semester = ddlsemester.SelectedItem.ToString();
                    //            degreecode = ddlbranch.SelectedValue.ToString();
                    //            batch = ddlbatch.SelectedItem.ToString();
                    //            string strsecl = string.Empty;
                    //            if (ddlsection.Enabled == true)
                    //            {
                    //                if (ddlsection.Items.Count > 0)
                    //                {
                    //                    strsecl = "Section :" + ddlsection.SelectedItem.ToString();
                    //                }
                    //            }
                    //            string year = getyear(Convert.ToInt32(ddlsemester.SelectedItem.ToString()));
                    //            string studdetail = "    Course :" + ddldegree.SelectedItem.Text.ToString() + "    " + "   Branch :" + ddlbranch.SelectedItem.ToString() + "    Batch :" + ddlbatch.SelectedItem.ToString() + "    " + year.ToString() + "    Semester :" + ddlsemester.SelectedItem.ToString() + "   " + strsecl;
                    //            string studdetail3 = "     Name :" + studname + "          Roll No :" + hcrollno + "     Reg.No :" + regn;
                    //            FpSpread1.Sheets[0].AddSpanCell(0, 0, 2, FpSpread1.Sheets[0].ColumnCount - 3);
                    //            FpSpread1.Sheets[0].Cells[0, 0].Text = studdetail.ToString();
                    //            FpSpread1.Sheets[0].Cells[0, 0].Font.Bold = true;
                    //            FpSpread1.Sheets[0].Cells[0, 0].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[0, 0].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].AddSpanCell(2, 0, 2, FpSpread1.Sheets[0].ColumnCount - 3);
                    //            FpSpread1.Sheets[0].Cells[2, 0].Text = studdetail3.ToString();
                    //            FpSpread1.Sheets[0].Cells[2, 0].Font.Bold = true;
                    //            FpSpread1.Sheets[0].Cells[2, 0].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[2, 0].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].AddSpanCell(4, 0, 1, FpSpread1.Sheets[0].ColumnCount - 3);
                    //            FpSpread1.Sheets[0].Cells[4, 0].Text = "    Faculty Adviser :" + faclty.ToString();
                    //            FpSpread1.Sheets[0].Cells[4, 0].Font.Bold = true;
                    //            FpSpread1.Sheets[0].Cells[4, 0].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[4, 0].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].AddSpanCell(5, 0, 1, FpSpread1.Sheets[0].ColumnCount - 3);
                    //            // FpSpread1.Sheets[0].Cells[5, 0].Text = "    Ward Counsellor :".ToString();
                    //            //FpSpread1.Sheets[0].Cells[5, 0].Font.Bold = true;
                    //            //FpSpread1.Sheets[0].Cells[5, 0].VerticalAlign = VerticalAlign.Middle;
                    //            // FpSpread1.Sheets[0].Cells[5, 0].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].AddSpanCell(0, FpSpread1.Sheets[0].ColumnCount - 3, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                    //            FpSpread1.Sheets[0].Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Text = "Previous History";
                    //            FpSpread1.Sheets[0].Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Font.Bold = true;
                    //            FpSpread1.Sheets[0].Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].AddSpanCell(1, FpSpread1.Sheets[0].ColumnCount - 3, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                    //            FpSpread1.Sheets[0].Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Text = "Before Coming Here...";
                    //            FpSpread1.Sheets[0].Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Font.Bold = true;
                    //            FpSpread1.Sheets[0].Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                    //            string hscpolystr = GetFunction("select percentage from stud_prev_details as s,textvaltable as t where app_no= (select app_no from registration where roll_no='" + hcrollno + "')  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%'))");
                    //            FpSpread1.Sheets[0].AddSpanCell(2, FpSpread1.Sheets[0].ColumnCount - 3, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                    //            FpSpread1.Sheets[0].Cells[2, FpSpread1.Sheets[0].ColumnCount - 3].Text = "XII/Poly Marks %   :" + hscpolystr.ToString();
                    //            FpSpread1.Sheets[0].Cells[2, FpSpread1.Sheets[0].ColumnCount - 3].Font.Bold = true;
                    //            FpSpread1.Sheets[0].Cells[2, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[2, FpSpread1.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[2, FpSpread1.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].AddSpanCell(3, FpSpread1.Sheets[0].ColumnCount - 3, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                    //            FpSpread1.Sheets[0].Cells[3, FpSpread1.Sheets[0].ColumnCount - 3].Text = "After Coming Here...";
                    //            FpSpread1.Sheets[0].Cells[3, FpSpread1.Sheets[0].ColumnCount - 3].Font.Bold = true;
                    //            FpSpread1.Sheets[0].Cells[3, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[3, FpSpread1.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[3, FpSpread1.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                    //            con.Close();
                    //            con.Open();
                    //            string attnd_points1 = "select *from leave_points";
                    //            SqlDataAdapter da_attnd_pts1;
                    //            da_attnd_pts1 = new SqlDataAdapter(attnd_points1, con);
                    //            da_attnd_pts1.Fill(ds_attnd_pts);
                    //            if (ds_attnd_pts.Tables[0].Rows.Count > 0)
                    //            {
                    //                holi_leav = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["leave_bef_aft"].ToString());
                    //                holi_absent = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["absent_bef_aft"].ToString());
                    //                leav_pt = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["leave"].ToString());
                    //                absent_pt = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["absent"].ToString());
                    //            }
                    //            ds1.Dispose();
                    //            ds1.Reset();
                    //            hat.Clear();
                    //            hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
                    //            hat.Add("sem_ester", int.Parse(ddlsemester.SelectedValue.ToString()));
                    //            ds = d2.select_method("period_attnd_schedule", hat, "sp");
                    //            if (ds.Tables[0].Rows.Count != 0)
                    //            {
                    //                NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                    //                fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                    //                anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                    //                minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                    //                minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                    //            }
                    //            hat.Clear();
                    //            hat.Add("colege_code", Session["collegecode"].ToString());
                    //            ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
                    //            count = ds1.Tables[0].Rows.Count;
                    //            DateTime dtexamdate = DateTime.Now.Date;
                    //            int day, month, yearvex;
                    //            day = dtexamdate.Day;
                    //            month = dtexamdate.Month;
                    //            yearvex = dtexamdate.Year;
                    //            examdate = day + "/" + month + "/" + yearvex;
                    //            persentmonthcal(examdate, strtdate);
                    //            per_con_hrs = per_workingdays1;
                    //            per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / per_con_hrs) * 100);
                    //            FpSpread1.Sheets[0].Cells[4, FpSpread1.Sheets[0].ColumnCount - 3].Text = "Attendance %";
                    //            FpSpread1.Sheets[0].Cells[4, FpSpread1.Sheets[0].ColumnCount - 3].Font.Bold = true;
                    //            FpSpread1.Sheets[0].Cells[4, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[4, FpSpread1.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[4, FpSpread1.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                    //            if (per_tage_hrs != null && Convert.ToString(per_tage_hrs) != "" && Convert.ToString(per_tage_hrs) != "NaN")
                    //            {
                    //                FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 3].Text = (Convert.ToString(Decimal.Parse(per_tage_hrs.ToString("0.00"))));
                    //                FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                    //                FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                    //                FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                    //                FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                    //            }
                    //            else
                    //            {
                    //                FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                    //                FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 3].Text = Convert.ToString("-");
                    //            }
                    //            FpSpread1.Sheets[0].Cells[4, FpSpread1.Sheets[0].ColumnCount - 2].Text = "Arrears ";
                    //            FpSpread1.Sheets[0].Cells[4, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                    //            FpSpread1.Sheets[0].Cells[4, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[4, FpSpread1.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[4, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                    //            string strarrcount = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester from subject,syllabus_master as smas where smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from mark_entry where subject_no not in (select distinct subject_no from mark_entry where passorfail=1 and result='Pass' and ltrim(rtrim(roll_no))='" + hcrollno + "') and roll_no ='" + hcrollno + "' and Semester >= '1' and Semester <= '" + ddlsemester.SelectedValue.ToString() + "')";
                    //            string arrcount = string.Empty;
                    //            DataSet dsarrcount = new DataSet();
                    //            dsarrcount = d2.select_method(strarrcount, hat, "Text");
                    //            if (dsarrcount.Tables[0].Rows.Count > 0)
                    //            {
                    //                arrcount = Convert.ToString(dsarrcount.Tables[0].Rows.Count);
                    //            }
                    //            else
                    //            {
                    //                arrcount = "-";
                    //            }
                    //            FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 2].Text = arrcount.ToString();
                    //            FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                    //            string cgpav = string.Empty;
                    //            if (Calculete_CGPA(hcrollno, semester, degreecode, batch, latmode) != "" && Calculete_CGPA(hcrollno, semester, degreecode, batch, latmode) != null)
                    //            {
                    //                cgpav = Calculete_CGPA(hcrollno, semester, degreecode, batch, latmode);
                    //            }
                    //            else
                    //            {
                    //                cgpav = " -";
                    //            }
                    //            FpSpread1.Sheets[0].Cells[4, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Cummulative Marks %";
                    //            FpSpread1.Sheets[0].Cells[4, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //            FpSpread1.Sheets[0].Cells[4, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[4, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[4, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 1].Text = cgpav.ToString();
                    //            FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //            FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    //            FpSpread1.Sheets[0].Cells[5, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //            FpSpread1.Sheets[0].AddSpanCell(6, 0, 2, FpSpread1.Sheets[0].ColumnCount);
                    //            btnxl.Visible = true;
                    //            txtexcelname.Visible = true;
                    //            lblrptname.Visible = true;
                    //            FpSpread2.Sheets[0].Cells[res, 8].Value = "0";//Added By Srinath 25/4/2013
                    //            FpSpread2.SaveChanges();
                    //            btnclose.Visible = true;
                    //            // ------------------- pdf Added by Rajesh 27-5-2015 start
                    //            //    System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
                    //            //    System.Drawing.Font Fontsmall8 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
                    //            //    FpSpread1.SaveChanges();
                    //            //    int rowcountspread = FpSpread1.Sheets[0].RowCount;
                    //            //    int columncountspread = FpSpread1.Sheets[0].ColumnCount;
                    //            //    Gios.Pdf.PdfTable table1 = mydoc.NewTable(Fontsmall1, rowcountspread, columncountspread, 1);
                    //            //    table1.VisibleHeaders = false;
                    //            //    table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    //            //    Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                    //            //    for (int ik = 0; ik < FpSpread1.Sheets[0].RowCount; ik++)
                    //            //    {
                    //            //        for (int jk = 0; jk < columncountspread; jk++)
                    //            //        {
                    //            //            string coldata = FpSpread1.Sheets[0].Cells[ik, jk].Text;
                    //            //            table1.Cell(ik, jk).SetContentAlignment(ContentAlignment.MiddleCenter);
                    //            //            table1.Cell(ik, jk).SetContent(coldata);
                    //            //            if (ik == 8)
                    //            //            {
                    //            //                if (jk > 4 && jk < 20)
                    //            //                {
                    //            //                    if (coldata.Length <= 10)
                    //            //                    {
                    //            //                        table1.Columns[jk].SetWidth(4);
                    //            //                    }
                    //            //                }
                    //            //            }
                    //            //            if (jk == 0)
                    //            //            {
                    //            //                if (coldata == "Total Marks Obtained")
                    //            //                {
                    //            //                    foreach (PdfCell pr in table1.CellRange(ik, 0, ik, 0).Cells)
                    //            //                    {
                    //            //                        pr.ColSpan = 4;
                    //            //                    }
                    //            //                    foreach (PdfCell pr in table1.CellRange(ik + 1, 0, ik + 1, 0).Cells)
                    //            //                    {
                    //            //                        pr.ColSpan = 4;
                    //            //                    }
                    //            //                    foreach (PdfCell pr in table1.CellRange(ik + 2, 0, ik + 2, 0).Cells)
                    //            //                    {
                    //            //                        pr.ColSpan = 2;
                    //            //                    }
                    //            //                    foreach (PdfCell pr in table1.CellRange(ik + 2, 2, ik + 2, 2).Cells)
                    //            //                    {
                    //            //                        pr.ColSpan = 2;
                    //            //                    }
                    //            //                    foreach (PdfCell pr in table1.CellRange(ik + 3, 0, ik + 3, 0).Cells)
                    //            //                    {
                    //            //                        pr.ColSpan = 4;
                    //            //                    }
                    //            //                    foreach (PdfCell pr in table1.CellRange(ik + 4, 0, ik + 4, 0).Cells)
                    //            //                    {
                    //            //                        pr.ColSpan = 4;
                    //            //                    }
                    //            //                    foreach (PdfCell pr in table1.CellRange(ik + 5, 0, ik + 5, 0).Cells)
                    //            //                    {
                    //            //                        pr.ColSpan = 4;
                    //            //                    }
                    //            //                    //foreach (PdfCell pr in table1.CellRange(ik + 6, 0, ik + 6, 0).Cells)
                    //            //                    //{
                    //            //                    //    pr.ColSpan = 4;
                    //            //                    //}
                    //            //                    foreach (PdfCell pr in table1.CellRange(ik + 6, 0, ik + 6, 0).Cells)
                    //            //                    {
                    //            //                        pr.ColSpan = 2;
                    //            //                    }
                    //            //                    foreach (PdfCell pr in table1.CellRange(ik + 6, 2, ik + 6, 2).Cells)
                    //            //                    {
                    //            //                        pr.ColSpan = 2;
                    //            //                    }
                    //            //                    foreach (PdfCell pr in table1.CellRange(ik + 7, 0, ik + 7, 0).Cells)
                    //            //                    {
                    //            //                        pr.ColSpan = 4;
                    //            //                    }
                    //            //                    foreach (PdfCell pr in table1.CellRange(ik + 8, 0, ik + 8, 0).Cells)
                    //            //                    {
                    //            //                        pr.ColSpan = 4;
                    //            //                    }
                    //            //                }
                    //            //            }
                    //            //        }
                    //            //    }
                    //            //    table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                    //            //    table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                    //            //    table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                    //            //    table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                    //            //    table1.Columns[0].SetWidth(2);
                    //            //    table1.Columns[1].SetWidth(4);
                    //            //    int colss = columncountspread - 3;
                    //            //    ////foreach (PdfCell pr in table1.CellRange(0, 0, 0, 0).Cells)
                    //            //    ////{
                    //            //    ////    pr.ColSpan = 17;
                    //            //    ////}
                    //            //    ////foreach (PdfCell pr in table1.CellRange(0, 0, 0, 0).Cells)
                    //            //    ////{
                    //            //    ////    pr.RowSpan = 2;
                    //            //    ////}
                    //            //    ////foreach (PdfCell pr in table1.CellRange(0, 17, 0, 18).Cells)
                    //            //    ////{
                    //            //    ////    pr.ColSpan = 2;
                    //            //    ////}
                    //            //    ////foreach (PdfCell pr in table1.CellRange(0, 17, 1, 18).Cells)
                    //            //    ////{
                    //            //    ////    pr.ColSpan = 2;
                    //            //    ////}
                    //            //    //foreach (PdfCell pr in table1.CellRange(1, 0, 1, 16).Cells)
                    //            //    //{
                    //            //    //    pr.ColSpan = 17;
                    //            //    //}
                    //            //    table1.Columns[3].SetWidth(4);
                    //            //    int lastfpcol = FpSpread1.Sheets[0].Columns.Count;
                    //            //    table1.Columns[lastfpcol - 1].SetWidth(8);
                    //            //    foreach (PdfCell pr in table1.CellRange(0, 0, 0, 0).Cells)
                    //            //    {
                    //            //        pr.ColSpan = lastfpcol - 3;
                    //            //    }
                    //            //    foreach (PdfCell pr in table1.CellRange(0, 0, 0, 0).Cells)
                    //            //    {
                    //            //        pr.RowSpan = 2;
                    //            //    }
                    //            //    foreach (PdfCell pr in table1.CellRange(2, 0, 2, 0).Cells)
                    //            //    {
                    //            //        pr.ColSpan = lastfpcol - 3;
                    //            //    }
                    //            //    foreach (PdfCell pr in table1.CellRange(2, 0, 2, 0).Cells)
                    //            //    {
                    //            //        pr.RowSpan = 2;
                    //            //    }
                    //            //    foreach (PdfCell pr in table1.CellRange(8, 0, 8, 0).Cells)
                    //            //    {
                    //            //        pr.RowSpan = 2;
                    //            //    }
                    //            //    foreach (PdfCell pr in table1.CellRange(8, 1, 8, 1).Cells)
                    //            //    {
                    //            //        pr.RowSpan = 2;
                    //            //    }
                    //            //    foreach (PdfCell pr in table1.CellRange(3, 0, 3, 0).Cells)
                    //            //    {
                    //            //        pr.ColSpan = lastfpcol - 3;
                    //            //    }
                    //            //    foreach (PdfCell pr in table1.CellRange(4, 0, 4, 0).Cells)
                    //            //    {
                    //            //        pr.ColSpan = lastfpcol - 3;
                    //            //    }
                    //            //    foreach (PdfCell pr in table1.CellRange(5, 0, 5, 0).Cells)
                    //            //    {
                    //            //        pr.ColSpan = lastfpcol - 3;
                    //            //    }
                    //            //    foreach (PdfCell pr in table1.CellRange(6, 0, 6, 0).Cells)
                    //            //    {
                    //            //        pr.ColSpan = lastfpcol;
                    //            //    }
                    //            //    foreach (PdfCell pr in table1.CellRange(7, 0, 7, 0).Cells)
                    //            //    {
                    //            //        pr.ColSpan = lastfpcol;
                    //            //    }
                    //            //    //foreach (PdfCell pr in table1.CellRange(6, 0, 6, 0).Cells)
                    //            //    //{
                    //            //    //    pr.RowSpan = 2;
                    //            //    //}
                    //            //    // table1.Cell(6, 0).SetContent("                       ");
                    //            //    foreach (PdfCell pr in table1.CellRange(0, lastfpcol - 3, 0, lastfpcol - 3).Cells)
                    //            //    {
                    //            //        pr.ColSpan = 3;
                    //            //    }
                    //            //    foreach (PdfCell pr in table1.CellRange(1, lastfpcol - 3, 1, lastfpcol - 3).Cells)
                    //            //    {
                    //            //        pr.ColSpan = 3;
                    //            //    }
                    //            //    foreach (PdfCell pr in table1.CellRange(2, lastfpcol - 3, 2, lastfpcol - 3).Cells)
                    //            //    {
                    //            //        pr.ColSpan = 3;
                    //            //    }
                    //            //    foreach (PdfCell pr in table1.CellRange(3, lastfpcol - 3, 3, lastfpcol - 3).Cells)
                    //            //    {
                    //            //        pr.ColSpan = 3;
                    //            //    }
                    //            //    //foreach (PdfCell pr in table1.CellRange(0, 17, 3, 18).Cells)
                    //            //    //{
                    //            //    //    pr.ColSpan = 2;
                    //            //    //}
                    //            //    //foreach (PdfCell pr in table1.CellRange(0, 17, 3, 18).Cells)
                    //            //    //{
                    //            //    //    pr.ColSpan = 2;
                    //            //    //}
                    //            //    //foreach (PdfCell pr in table1.CellRange(res, 0, 1, 0).Cells)
                    //            //    //{
                    //            //    //    pr.ColSpan = 17;
                    //            //    //}
                    //            //    //foreach (PdfCell pr in table1.CellRange(res, 0, 1, 0).Cells)
                    //            //    //{
                    //            //    //    pr.RowSpan = 2;
                    //            //    //}
                    //            //    //foreach (PdfCell pr in table1.CellRange(0, 17, 4, 18).Cells)
                    //            //    //{
                    //            //    //    pr.ColSpan = 2;
                    //            //    //}
                    //            //    //foreach (PdfCell pr in table1.CellRange(0, 17, 4, 18).Cells)
                    //            //    //{
                    //            //    //    pr.ColSpan = 2;
                    //            //    //}
                    //            //    Gios.Pdf.PdfTablePage addtabletopagenew = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 30, 1150, 10000));
                    //            //    mypdfpage.Add(addtabletopagenew);
                    //            //    mypdfpage.SaveToDocument();
                    //            //    mypdfpage = mydoc.NewPage();
                    //            //}
                    //            //string appPath = HttpContext.Current.Server.MapPath("~");
                    //            //if (appPath != "")
                    //            //{
                    //            //    string szPath = appPath + "/Report/";
                    //            //    string szFile = "Collection" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    //            //    Response.Buffer = true;
                    //            //    Response.Clear();
                    //            //    mydoc.SaveToFile(szPath + szFile);
                    //            //    Response.ClearHeaders();
                    //            //    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    //            //    Response.ContentType = "application/pdf";
                    //            //    Response.WriteFile(szPath + szFile);
                    //        }
                    //    }
                    //    // ------------------- pdf Added by Rajesh 27-5-2015 End
                    //}
                    //// --------------------------- added by rajesh 26-5-2015 ---------------------------

                    #endregion
                    if (cnt > 0)
                    {
                        foreach (GridViewRow gvrow in Showgrid.Rows)
                        {
                            if (gvrow.RowIndex != 0)
                            {
                                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                                if (chk.Checked == true)
                                {
                                    cnt++;
                                    html = new StringBuilder();
                                    html1 = new StringBuilder();
                                    Label rollno = (Label)gvrow.Cells[1].FindControl("lbl_rollno");
                                    hcrollno = rollno.Text;
                                    Label regno = (Label)gvrow.Cells[2].FindControl("lbl_regno");
                                    regn = regno.Text;
                                    Label stdname = (Label)gvrow.Cells[3].FindControl("lbl_stdname");
                                    studname = stdname.Text;
                                    Label stdate = (Label)gvrow.Cells[4].FindControl("lbl_StartDate");
                                    strtdate = stdate.Text;
                                    Label admdate = (Label)gvrow.Cells[4].FindControl("lbl_admDate");
                                    admdatev = admdate.Text;
                                    Label mod = (Label)gvrow.Cells[4].FindControl("lbl_mode");
                                    latmode = mod.Text;

                                    DateTime admdv = Convert.ToDateTime(admdatev);
                                    int admday = admdv.Day;
                                    int admmonth = admdv.Month;
                                    int admyear = admdv.Year;
                                    admdatev = admday + "/" + admmonth + "/" + admyear;
                                    DateTime startdv = Convert.ToDateTime(strtdate);
                                    int stday = startdv.Day;
                                    int stmonth = startdv.Month;
                                    int styear = startdv.Year;
                                    strtdate = stday + "/" + stmonth + "/" + styear;
                                    int ini_column = 0;
                                    int no_column = 0;
                                    int tcolcount = 0;
                                    Hashtable criteriatothat = new Hashtable();
                                    if (ddlsection.Enabled == true)
                                    {
                                        faclty = GetFunction("select staff_name from  semester_schedule ss, staffmaster sm where batch_year='" + ddlbatch.SelectedItem.Text + "' and  degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlsemester.SelectedItem.Text + "' and sections='" + ddlsection.SelectedItem.Text + "' and ss.class_advisor like '%'+sm.staff_code+'%' --and sm.staff_code=ss.class_advisor");
                                    }
                                    else
                                    {
                                        faclty = GetFunction("select staff_name from  semester_schedule ss, staffmaster sm where batch_year='" + ddlbatch.SelectedItem.Text + "' and  degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlsemester.SelectedItem.Text + " ' and ss.class_advisor like '%'+sm.staff_code+'%' --and sm.staff_code=ss.class_advisor");
                                    }


                                    html.Append("<center> <div style='height: 990px; width: 100%; border: 0px solid black; margin-left: 5px; margin: 0px; page-break-after: always;'> <center><div style='border: 0px solid black'>  <center> <table style='width: 95%; margin-top: 1px; margin-bottom: 2px; font-size: medium;'cellpadding='5' cellspacing='0'> ");
                                    html.Append("<tr><td rowspan='2'  style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>S.No</td><td rowspan='2'  style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>Subject Code</td><td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>Subject Name</td><td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>TH/PR</td>");




                                    if (ddlsection.Text.ToString() == "All" || ddlsection.Text.ToString() == string.Empty || ddlsection.Text.ToString() == "-1")
                                    {
                                        strsec1 = string.Empty;
                                    }
                                    else
                                    {
                                        strsec1 = " and sections='" + ddlsection.Text.ToString() + "'";
                                    }
                                    //ini_column = FpSpread1.Sheets[0].ColumnCount;
                                    no_column = 0;
                                    criteriain = string.Empty;
                                    int col = 3;

                                    Dictionary<int, string> dictestcode = new Dictionary<int, string>();
                                    Dictionary<int, string> dictestmaxmark = new Dictionary<int, string>();
                                    for (int test = 0; test < cbltest.Items.Count; test++)
                                    {
                                        if (cbltest.Items[test].Selected == true)
                                        {
                                            criteriatot = criteriatot + 1;
                                            col++;
                                            html.Append("<td  style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + Convert.ToString(cbltest.Items[test].Text.ToString()) + "</td>");
                                            dictestcode.Add(col, Convert.ToString(cbltest.Items[test].Value));


                                            no_column = no_column + 1;
                                            if (criteriain == null || criteriain == "")
                                                criteriain = "'" + Convert.ToString(cbltest.Items[test].Value) + "'";
                                            else
                                                criteriain = criteriain + "," + "'" + Convert.ToString(cbltest.Items[test].Value) + "'";
                                        }
                                    }
                                    tcolcount = col;
                                    if (criteriain != "")
                                    {
                                        criteriain = " in(" + criteriain + ")";
                                    }
                                    criteriain = " and e.criteria_no  " + criteriain + "";

                                    html.Append("<td  style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>Session Marks</td>");
                                    html.Append("<td  style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>University Exam Marks / Grade</td>");
                                    html.Append("<td  style='border: thin solid #000000; ' align='center'  class='style1'>Total</td>");
                                    html.Append("</tr>");

                                    html.Append("<tr><td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>Max.Marks</td><td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'></td>");

                                    if (chklstsubject.Items.Count > 0)
                                    {

                                        for (int subj = 0; subj < chklstsubject.Items.Count; subj++)
                                        {
                                            if (chklstsubject.Items[subj].Selected == true)
                                            {
                                                string subjcode = Convert.ToString(chklstsubject.Items[subj].Value);
                                                sqlmarkcmd = "select distinct r.marks_obtained,isnull(e.min_mark,0) as min_mark,r.roll_no,e.exam_code,re.roll_no,criteria_no ,Len(r.roll_no),e.max_mark from result r,exam_type e,registration re where r.roll_no=re.roll_no and e.exam_code=r.exam_code and  r.roll_no='" + hcrollno + "'  and e.subject_no='" + subjcode + "' " + criteriain + " and  RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by Len(r.roll_no),e.criteria_no";
                                                dsmethodgomark = d2.select_method(sqlmarkcmd, hat, "Text");
                                                if (dsmethodgomark != null && dsmethodgomark.Tables[0] != null && dsmethodgomark.Tables[0].Rows.Count > 0)
                                                {
                                                    int minmark = 0;
                                                    double markob = 0;
                                                    double markforad = 0;
                                                    string criterianotemp = "", mcriteriano = string.Empty;
                                                    foreach (DataRow dr in dsmethodgomark.Tables[0].Rows)
                                                    {
                                                        criterianotemp = dr["criteria_no"].ToString();
                                                        for (int k = 4; k <= tcolcount; k++)
                                                        {
                                                            mcriteriano = dictestcode[k];
                                                            if (criterianotemp != "" && mcriteriano != "" && criterianotemp == mcriteriano)
                                                            {


                                                                if (dictestmaxmark.ContainsKey(k))
                                                                {
                                                                    dictestmaxmark.Remove(k);
                                                                    dictestmaxmark.Add(k, dr["max_mark"].ToString());

                                                                }
                                                                else
                                                                {
                                                                    dictestmaxmark.Add(k, dr["max_mark"].ToString());
                                                                }


                                                                //dictestmaxmark.Add(colcnt, dr["max_mark"].ToString());
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        foreach (KeyValuePair<int, string> drval in dictestmaxmark)
                                        {
                                            string maxmark = drval.Value;
                                            html.Append("<td  style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + maxmark + "</td>");

                                        }
                                    }
                                    html.Append("<td  style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>20 %</td><td  style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>80 %</td><td  style='border: thin solid #000000; ' align='center'  class='style1'>100</td>");
                                    html.Append("</tr>");
                                    if (chklstsubject.Items.Count > 0)
                                    {
                                        for (int subj = 0; subj < chklstsubject.Items.Count; subj++)
                                        {
                                            if (chklstsubject.Items[subj].Selected == true)
                                            {
                                                if (no_column != 0)
                                                {

                                                    html.Append("<tr><td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + (subj + 1).ToString() + "</td>");


                                                    string subjtypevel = Convert.ToString(GetCorrespondingKey(Convert.ToString(chklstsubject.Items[subj].Value), htsubjcide));
                                                    string subjcodde = d2.GetFunction("select subject_code from subject where subject_no='" + chklstsubject.Items[subj].Value + "'");

                                                    subjtypevel = subjcodde;
                                                    html.Append("<td  style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + Convert.ToString(subjtypevel) + "</td>");
                                                    html.Append("<td  style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + Convert.ToString(chklstsubject.Items[subj].Text.ToString()) + "</td>");



                                                    string subjcode = Convert.ToString(chklstsubject.Items[subj].Value);
                                                    string thpr = string.Empty;
                                                    if (htb.Contains(Convert.ToString(subjcode)))
                                                    {
                                                        thpr = Convert.ToString(GetCorrespondingKey(Convert.ToString(subjcode), htb));
                                                    }
                                                    else
                                                    {
                                                        thpr = "-";
                                                    }
                                                    string querythpr = d2.GetFunction("select subject_type from sub_sem ss,subject s where ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and s.subject_code='" + subjcodde + "' group by subject_type");
                                                    if (querythpr == "0")
                                                    {
                                                        querythpr = "-";
                                                    }

                                                    html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + Convert.ToString((querythpr)) + "</td>");

                                                    subjectctot = subjectctot + 1;
                                                    sqlmarkcmd = "select distinct r.marks_obtained,isnull(e.min_mark,0) as min_mark,r.roll_no,e.exam_code,re.roll_no,criteria_no ,Len(r.roll_no),e.max_mark from result r,exam_type e,registration re where r.roll_no=re.roll_no and e.exam_code=r.exam_code and  r.roll_no='" + hcrollno + "'  and e.subject_no='" + subjcode + "' " + criteriain + " and  RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by Len(r.roll_no),e.criteria_no";
                                                    dsmethodgomark = d2.select_method(sqlmarkcmd, hat, "Text");
                                                    if (dsmethodgomark != null && dsmethodgomark.Tables[0] != null && dsmethodgomark.Tables[0].Rows.Count > 0)
                                                    {
                                                        int minmark = 0;
                                                        double markob = 0;
                                                        double markforad = 0;
                                                        string criterianotemp = "", mcriteriano = string.Empty;
                                                        Boolean columnvisiable = false;
                                                        int columncount1 = 0;
                                                        int columncnt = (tcolcount - 3) - dsmethodgomark.Tables[0].Rows.Count;
                                                        foreach (DataRow dr in dsmethodgomark.Tables[0].Rows)
                                                        {
                                                            criterianotemp = dr["criteria_no"].ToString();

                                                            for (int k = 4; k <= tcolcount; k++)
                                                            {
                                                                bool color = false;
                                                                mcriteriano = dictestcode[k];
                                                                if (criterianotemp != "" && mcriteriano != "" && criterianotemp == mcriteriano)
                                                                {

                                                                    minmark = Convert.ToInt32(dr["min_mark"]);
                                                                    markob = Convert.ToDouble(dr["marks_obtained"]);
                                                                    if (markob < 0)
                                                                    {
                                                                        markforad = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        markforad = markob;
                                                                    }
                                                                    marks_per = Convert.ToString(dr["marks_obtained"]);
                                                                    double markobta = 0;
                                                                    if (markob >= minmark)
                                                                    {
                                                                    }
                                                                    else
                                                                    {
                                                                        color = true;

                                                                        if (htv3.Contains(Convert.ToString(mcriteriano)))
                                                                        {
                                                                            string failvtr = Convert.ToString(GetCorrespondingKey(Convert.ToString(mcriteriano), htv3));
                                                                            failvtr = failvtr + 1;
                                                                            htv3[Convert.ToString(mcriteriano)] = failvtr;
                                                                        }
                                                                        else
                                                                        {
                                                                            htv3.Add(Convert.ToString(mcriteriano), 1);
                                                                        }
                                                                    }
                                                                    if (criteriatothat.Contains(Convert.ToString(mcriteriano)))
                                                                    {
                                                                        markobta = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(mcriteriano), criteriatothat));
                                                                        markobta = markobta + markob;
                                                                        criteriatothat[Convert.ToString(mcriteriano)] = markobta;
                                                                    }
                                                                    else
                                                                    {
                                                                        criteriatothat.Add(Convert.ToString(mcriteriano), markforad);
                                                                    }
                                                                    switch (marks_per)
                                                                    {
                                                                        case "-1":
                                                                            marks_perfinal = "AAA";
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
                                                                        //*********Modified by Subburaj 21.08.2014**********//
                                                                        case "-18":
                                                                            marks_perfinal = "RAA";
                                                                            break;
                                                                        //**************End********************//
                                                                        default:
                                                                            marks_perfinal = marks_per;
                                                                            break;
                                                                    }
                                                                    columncount1++;
                                                                    if (!color)
                                                                        html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + marks_perfinal.ToString() + "</td>");
                                                                    else

                                                                        html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'><font color='red'>" + marks_perfinal.ToString() + "</font></td>");
                                                                }

                                                                if (tcolcount >= k + 1 && tcolcount <= k + 1)
                                                                {
                                                                    string sqlcmduni = "select internal_mark,external_mark,grade from mark_entry m,exam_details e where e.exam_code=m.exam_code and roll_no='" + hcrollno + "' and subject_no='" + subjcode + "' and degree_code='" + ddlbranch.SelectedValue + "' and batch_year='" + ddlbatch.SelectedValue + "' and current_semester='" + ddlsemester.SelectedValue + "'";
                                                                    dsuni = d2.select_method(sqlcmduni, hat, "Text");
                                                                    if (dsuni != null && dsuni.Tables[0] != null && dsuni.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        string intermark = Convert.ToString(dsuni.Tables[0].Rows[0]["internal_mark"].ToString());
                                                                        string extermark = Convert.ToString(dsuni.Tables[0].Rows[0]["external_mark"].ToString());
                                                                        string strgrade = Convert.ToString(dsuni.Tables[0].Rows[0]["grade"].ToString());
                                                                        if (intermark != "" && intermark != null && extermark != "" && extermark != null)
                                                                        {
                                                                            columnvisiable = true;
                                                                            html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + dsuni.Tables[0].Rows[0]["internal_mark"].ToString() + "</td>");
                                                                            html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + dsuni.Tables[0].Rows[0]["external_mark"].ToString() + "</td>");
                                                                            html.Append("<td style='border: thin solid #000000; ' align='center'  class='style1'>" + (intermark + extermark).ToString() + "</td>");

                                                                        }
                                                                        else if (strgrade != "" && strgrade != null && intermark != "" && intermark != null)
                                                                        {
                                                                            columnvisiable = true;
                                                                            html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + dsuni.Tables[0].Rows[0]["internal_mark"].ToString() + "</td>");
                                                                            html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + dsuni.Tables[0].Rows[0]["grade"].ToString() + "</td>");
                                                                            html.Append("<td style='border: thin solid #000000; ' align='center'  class='style1'>" + dsuni.Tables[0].Rows[0]["grade"].ToString() + "</td>");

                                                                        }
                                                                    }


                                                                }

                                                            }

                                                        }
                                                        if (!columnvisiable)
                                                        {

                                                            if (columncnt == 0)
                                                            {

                                                                html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'></td>");
                                                                html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'></td>");
                                                                html.Append("<td style='border: thin solid #000000;  align='center'  class='style1'></td>");
                                                            }
                                                            else
                                                            {

                                                                for (int g = 0; g < columncnt; g++)
                                                                    html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'></td>");
                                                                html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'></td>");
                                                                html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'></td>");
                                                                html.Append("<td style='border: thin solid #000000;  align='center'  class='style1'></td>");

                                                            }
                                                        }
                                                    }
                                                    html.Append("</tr>");
                                                }

                                                else
                                                {
                                                    errmsg.Visible = true;
                                                    errmsg.Text = "Please Choose Test";
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        errmsg.Visible = true;
                                        errmsg.Text = "Please Choose Subject";
                                    }
                                    tottet = ((subjectctot * no_column));

                                    Dictionary<int, string> dicrowcommoncontent = new Dictionary<int, string>();
                                    dicrowcommoncontent.Add(1, "TotalMarksObtained,4");
                                    dicrowcommoncontent.Add(2, "Maximum Marks,4");
                                    dicrowcommoncontent.Add(3, ">60 %,% of Marks,2");
                                    dicrowcommoncontent.Add(4, "Class Rank,4");
                                    dicrowcommoncontent.Add(5, "Class - Attended Hours,4");
                                    dicrowcommoncontent.Add(6, "Attendance - Total Hours Conducted,4");
                                    dicrowcommoncontent.Add(7, ">85 %,Attendance %,2");
                                    dicrowcommoncontent.Add(8, "Progress Card Sent to the Parents on,4");
                                    dicrowcommoncontent.Add(9, ">Student's Signature,4");

                                    foreach (KeyValuePair<int, string> dr in dicrowcommoncontent)
                                    {
                                        int row = dr.Key;
                                        string value = dr.Value;
                                        string[] colspan = value.Split(',');
                                        if (colspan.Length == 2)
                                        {
                                            html.Append("<tr><td colspan='" + colspan[1] + "' style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + colspan[0] + "</td>");
                                        }
                                        else
                                        {
                                            html.Append("<tr><td   colspan='" + colspan[2] + "' style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + colspan[0] + "</td><td   colspan='" + colspan[2] + "' style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + colspan[1] + "</td>");

                                        }
                                        int mm = 1;
                                        for (int z = 4; z <= tcolcount; z++)
                                        {
                                            double tot = 0;
                                            string mcriteriano1 = dictestcode[z];
                                            if (mcriteriano1 != "" && mcriteriano1 != null)
                                            {
                                                if (Convert.ToDouble(GetCorrespondingKey(Convert.ToString(mcriteriano1), criteriatothat)) != null && Convert.ToDouble(GetCorrespondingKey(Convert.ToString(mcriteriano1), criteriatothat)) != 0)
                                                {
                                                    tot = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(mcriteriano1), criteriatothat));
                                                }
                                                else
                                                {
                                                    tot = 0;
                                                }
                                                examdate = GetFunction("select min(exam_date) from exam_type where criteria_no='" + mcriteriano1 + "' " + strsec1 + "");
                                                DateTime examdv = Convert.ToDateTime(examdate);
                                                int examday = examdv.Day;
                                                int exammonth = examdv.Month;
                                                int examyear = examdv.Year;
                                                examdate = examday + "/" + exammonth + "/" + examyear;
                                                if (row == 1)
                                                    html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + tot.ToString() + "</td>");


                                                string maxmark = dictestmaxmark[z];
                                                int maxv = 0;
                                                if (maxmark != null && maxmark != "")
                                                {
                                                    if (Convert.ToInt32(maxmark) != null)
                                                    {
                                                        maxv = Convert.ToInt32(maxmark);
                                                    }
                                                    else
                                                    {
                                                        maxv = 0;
                                                    }
                                                }
                                                mm++;
                                                if (row == 2)
                                                    html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + (subjectctot * maxv).ToString() + "</td>");




                                                decimal tempperce = Convert.ToDecimal(tot / subjectctot);
                                                if (row == 3)
                                                {
                                                    string mark = Convert.ToString(Decimal.Parse(tempperce.ToString("0.00")));
                                                    string[] spilt = mark.Split('.');
                                                    html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + Convert.ToString(spilt[0]) + "</td>");
                                                }

                                                failv = Convert.ToString(GetCorrespondingKey(Convert.ToString(mcriteriano1), htv3));
                                                if (failv == "")
                                                {
                                                    rankov3 = overallperformprint(mcriteriano1, hcrollno);
                                                }
                                                else
                                                {
                                                    rankov3 = "-";
                                                }
                                                if (row == 4)
                                                    html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + rankov3.ToString() + "</td>");



                                                con.Close();
                                                con.Open();
                                                string attnd_points = "select *from leave_points";
                                                SqlDataAdapter da_attnd_pts;
                                                da_attnd_pts = new SqlDataAdapter(attnd_points, con);
                                                da_attnd_pts.Fill(ds_attnd_pts);
                                                if (ds_attnd_pts.Tables[0].Rows.Count > 0)
                                                {
                                                    holi_leav = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["leave_bef_aft"].ToString());
                                                    holi_absent = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["absent_bef_aft"].ToString());
                                                    leav_pt = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["leave"].ToString());
                                                    absent_pt = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["absent"].ToString());
                                                }
                                                ds1.Dispose();
                                                ds1.Reset();
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
                                                persentmonthcal(examdate, strtdate);
                                                per_con_hrs = per_workingdays1;
                                                per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / per_con_hrs) * 100);
                                                if (per_per_hrs != null && tot_per_hrs_spl_fals != null)
                                                {
                                                    if (row == 5)
                                                        html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + (per_per_hrs + tot_per_hrs_spl_fals).ToString() + "</td>");

                                                }
                                                if (per_con_hrs != null && tot_conduct_hr_spl_fals != null)
                                                {
                                                    if (row == 6)
                                                        html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + (per_con_hrs + tot_conduct_hr_spl_fals).ToString() + "</td>");


                                                }
                                                if (per_tage_hrs != null && Convert.ToString(per_tage_hrs) != "" && Convert.ToString(per_tage_hrs) != "NaN")
                                                {
                                                    if (row == 7)
                                                    {
                                                        string mark1 = Convert.ToString(Decimal.Parse(per_tage_hrs.ToString("0.00")));
                                                        string[] spilt1 = mark1.Split('.');
                                                        html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + Convert.ToString(mark1) + "</td>");
                                                    }

                                                }
                                                else
                                                {
                                                    if (row == 7)
                                                        html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + Convert.ToString("-") + "</td>");


                                                }
                                                if (row == 8)
                                                    html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'></td>");
                                                if (row == 9)
                                                    html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'></td>");
                                            }
                                        }

                                        html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'></td>");
                                        html.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'></td>");
                                        html.Append("<td style='border: thin solid #000000;' align='center'  class='style1'></td>");

                                        html.Append("</tr>");

                                    }
                                    html.Append("</table></center></div></center></div></center>");
                                    //Next Html

                                    #region Saran
                                    int columncount = col;
                                    html1.Append("<center> <div style='width: 100%; border: 0px solid black; margin-left: 5px;  margin: 0px; page-break-after: always;'>         <center>");
                                    html1.Append("<table style='width: 95%; margin-top: 0px; margin-bottom: 2px; font-size: medium;'cellpadding='5' cellspacing='0'> ");
                                    html1.Append("<tr>");


                                    semester = ddlsemester.SelectedItem.ToString();
                                    degreecode = ddlbranch.SelectedValue.ToString();
                                    batch = ddlbatch.SelectedItem.ToString();
                                    string strsecl = string.Empty;
                                    if (ddlsection.Enabled == true)
                                    {
                                        if (ddlsection.Items.Count > 0)
                                        {
                                            strsecl = "Section :" + ddlsection.SelectedItem.ToString();
                                        }
                                    }
                                    string year = getyear(Convert.ToInt32(ddlsemester.SelectedItem.ToString()));
                                    string studdetail = "    Course :" + ddldegree.SelectedItem.Text.ToString() + "    " + "   Branch :" + ddlbranch.SelectedItem.ToString() + "    Batch :" + ddlbatch.SelectedItem.ToString() + "    " + year.ToString() + "    Semester :" + ddlsemester.SelectedItem.ToString() + "   " + strsecl;
                                    string studdetail3 = "     Name :" + studname + "          Roll No :" + hcrollno + "     Reg.No :" + regn;
                                    html1.Append("<td colspan='" + columncount + "' rowspan='2'  style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + studdetail.ToString() + "</td>");
                                    html1.Append("<td colspan='3'  style='border: thin solid #000000;' align='center'  class='style1'>Previous History</td></tr>");


                                    html1.Append("<tr>");
                                    html1.Append("<td colspan='3'  style='border: thin solid #000000;' align='center'  class='style1'>Before Coming Here...</td></tr>");

                                    html1.Append("<tr><td colspan='" + columncount + "' rowspan='2'  style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + studdetail3 + "</td>");


                                    string hscpolystr = GetFunction("select percentage from stud_prev_details as s,textvaltable as t where app_no= (select app_no from registration where roll_no='" + hcrollno + "')  and t.textcode=s.course_code and ((textval like '%XII%') or (textval like '%HSLC%') or (textval like '%Higher Secondary%') or (textval like '%12%') or (textval like '%Twelth%') or (textval like '%HSC%') or (textval like '%Diploma%') or (textval like '%H.SC(SB)%') or (textval like '%DIPLOMA%'))");
                                    html1.Append("<td colspan='3'  style='border: thin solid #000000;' align='center'  class='style1'>XII/Poly Marks %   :" + hscpolystr.ToString() + "</td></tr>");

                                    html1.Append("<tr>");
                                    html1.Append("<td colspan='3'  style='border: thin solid #000000;' align='center'  class='style1'>After Coming Here...</td></tr>");
                                    html1.Append("<tr><td colspan='" + columncount + "'  style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>Faculty Adviser :" + faclty.ToString() + "</td>");

                                    html1.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>Attendance %</td><td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>Arrears</td><td style='border: thin solid #000000;' align='center'  class='style1'>Cummulative Marks %</td></tr>");



                                    con.Close();
                                    con.Open();
                                    string attnd_points1 = "select *from leave_points";
                                    SqlDataAdapter da_attnd_pts1;
                                    da_attnd_pts1 = new SqlDataAdapter(attnd_points1, con);
                                    da_attnd_pts1.Fill(ds_attnd_pts);
                                    if (ds_attnd_pts.Tables[0].Rows.Count > 0)
                                    {
                                        holi_leav = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["leave_bef_aft"].ToString());
                                        holi_absent = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["absent_bef_aft"].ToString());
                                        leav_pt = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["leave"].ToString());
                                        absent_pt = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["absent"].ToString());
                                    }
                                    ds1.Dispose();
                                    ds1.Reset();
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
                                    DateTime dtexamdate = DateTime.Now.Date;
                                    int day, month, yearvex;
                                    day = dtexamdate.Day;
                                    month = dtexamdate.Month;
                                    yearvex = dtexamdate.Year;
                                    examdate = day + "/" + month + "/" + yearvex;
                                    persentmonthcal(examdate, strtdate);
                                    per_con_hrs = per_workingdays1;
                                    per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / per_con_hrs) * 100);
                                    html1.Append("<tr><td colspan='" + columncount + "'  style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>Ward Counsellor :</td>");
                                    if (per_tage_hrs != null && Convert.ToString(per_tage_hrs) != "" && Convert.ToString(per_tage_hrs) != "NaN")
                                    {
                                        html1.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + (Convert.ToString(Decimal.Parse(per_tage_hrs.ToString("0.00")))) + "</td>");

                                    }
                                    else
                                    {
                                        html1.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>-</td>");

                                    }

                                    string strarrcount = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester from subject,syllabus_master as smas where smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from mark_entry where subject_no not in (select distinct subject_no from mark_entry where passorfail=1 and result='Pass' and ltrim(rtrim(roll_no))='" + hcrollno + "') and roll_no ='" + hcrollno + "' and Semester >= '1' and Semester <= '" + ddlsemester.SelectedValue.ToString() + "')";
                                    string arrcount = string.Empty;
                                    DataSet dsarrcount = new DataSet();
                                    dsarrcount = d2.select_method(strarrcount, hat, "Text");
                                    if (dsarrcount.Tables[0].Rows.Count > 0)
                                    {
                                        arrcount = Convert.ToString(dsarrcount.Tables[0].Rows.Count);
                                    }
                                    else
                                    {
                                        arrcount = "-";
                                    }
                                    html1.Append("<td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>" + arrcount.ToString() + "</td>");

                                    string cgpav = string.Empty;
                                    string ccgpa = Calculete_CGPA(hcrollno, semester, degreecode, batch, latmode);
                                    if (Calculete_CGPA(hcrollno, semester, degreecode, batch, latmode) != "" && Calculete_CGPA(hcrollno, semester, degreecode, batch, latmode) != null)
                                    {
                                        cgpav = Calculete_CGPA(hcrollno, semester, degreecode, batch, latmode);
                                    }
                                    else
                                    {
                                        cgpav = " -";
                                    }
                                    html1.Append("<td style='border: thin solid #000000;' align='center'  class='style1'>" + cgpav.ToString() + "</td>");
                                    html1.Append("</tr></table> </center></div></center>");
                                    btnxl.Visible = true;
                                    txtexcelname.Visible = true;
                                    lblrptname.Visible = true;

                                    btnclose.Visible = true;
                                    // ------------------- pdf Added by Rajesh 27-5-2015 start


                                    #endregion
                                    html1.Append(html);
                                    htmlfinal.Append(html1);
                                }
                            }

                        }
                        contentDiv.InnerHtml = htmlfinal.ToString();
                        contentDiv.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "btnprint", "PrintDiv();", true);
                        Showgrid.Visible = true;
                    }
                    else
                    {
                        Showgrid.Visible = true;
                        errmsg.Visible = true;
                        errmsg.Text = "Please Select Atleast One Record";
                    }
                    //// --------------------------- added by rajesh 26-5-2015 ---------------------------
                    //else//Condition Added By Venkat 26/9/2014============================
                    //{
                    //    btnxl.Visible = false;
                    //    btnclose.Visible = false;
                    //    txtexcelname.Visible = false;
                    //    lblrptname.Visible = false;
                    //    btnprint.Visible = true;
                    //    btnprint1.Visible = true;
                    //    FpSpread1.Visible = false;
                    //    FpSpread2.Visible = true;
                    //    errmsg.Visible = true;
                    //    errmsg.Text = "You Can't Select Multiple Students";
                    //}
                    //main for loop
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Choose Test";
                }
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Choose Subject";
            }
        }
        catch (Exception exy)
        {
            errmsg.Visible = true;
            errmsg.Text = exy.ToString();
        }
    }

    public string getyear(int semester1)
    {
        string semyear = string.Empty;
        switch (semester1)
        {
            case 1:
                semyear = "I Year";
                break;
            case 2:
                semyear = "I Year";
                break;
            case 3:
                semyear = "II Year";
                break;
            case 4:
                semyear = "II Year";
                break;
            case 5:
                semyear = "III Year";
                break;
            case 6:
                semyear = "III Year";
                break;
            case 7:
                semyear = "IV Year";
                break;
            case 8:
                semyear = "IV Year";
                break;
            case 9:
                semyear = "IV Year";
                break;
            case 10:
                semyear = "IV Year";
                break;
        }
        return semyear;
    }

    private string Calculete_CGPA(string RollNo, string semval, string degree_code, string batch_year, string latmode)
    {
        string sqlcmdgraderstotal = string.Empty;
        sqlcmdgraderstotal = " select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + "";
        dggradetot = d2.select_method(sqlcmdgraderstotal, hat, "Text");
        int jvalue = 0;
        string gradestr = string.Empty;
        string ccva = string.Empty;
        string strgrade = string.Empty;
        double creditval = 0;
        double finalgpa1 = 0;
        double creditsum1 = 0;
        double gpacal1 = 0;
        int se = 0;
        string latsem = string.Empty;
        string latsemmax = string.Empty;
        string strsubcrd = string.Empty;
        string graders = string.Empty;
        for (jvalue = 1; jvalue <= Convert.ToInt32(semval); jvalue++)
        {
            gtempejval = jvalue;
            syll_code = GetFunction("select distinct syll_code from syllabus_master where degree_code=" + degree_code + " and semester =" + jvalue + " and batch_year=" + batch_year + "");
            if (syll_code != "")
            {
                if (jvalue == Convert.ToInt32(semval))
                {
                    examcodevalg = GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year=" + batch_year + " and current_semester='" + jvalue + "' ");
                    if (examcodevalg != null && examcodevalg != "")
                    {
                        strsubcrd = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcodevalg + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts=1";
                    }
                }
                else
                {
                    strsubcrd = "Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and syll_Code = " + syll_code + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass')and exam_code in (select distinct exam_code from exam_details where degree_code=" + degree_code + " and batch_year=" + batch_year + " and current_semester<=" + semval + ")";
                }
            }
            if (strsubcrd != null && strsubcrd != "")
            {
                SqlCommand cmd_subcrd = new SqlCommand(strsubcrd, con_subcrd);
                con_subcrd.Close();
                con_subcrd.Open();
                SqlDataReader dr_subcrd;
                dr_subcrd = cmd_subcrd.ExecuteReader();
                while (dr_subcrd.Read())
                {
                    if (dr_subcrd.HasRows)
                    {
                        if ((dr_subcrd["total"].ToString() != string.Empty))
                        {
                            if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                            {
                                strtot = Convert.ToDouble(dr_subcrd["total"].ToString());
                                foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                {
                                    if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                    {
                                        strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                        strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());
                                        if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                        {
                                            strgrade = gratemp["credit_points"].ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else if ((dr_subcrd["grade"].ToString() != string.Empty))
                        {
                            if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                            {
                                strtotgrac = Convert.ToString(dr_subcrd["grade"].ToString());
                                foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                {
                                    strgradetempgrade = Convert.ToString(gratemp["mark_grade"].ToString());
                                    if (strgradetempgrade == strtotgrac)
                                    {
                                        strgrade = gratemp["credit_points"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        creditval = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                        if (creditsum1 == 0)
                        {
                            creditsum1 = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                        }
                        else
                        {
                            creditsum1 = creditsum1 + Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                        }
                        if (gpacal1 == 0)
                        {
                            if (strgrade != "")
                            {
                                gpacal1 = Convert.ToDouble(strgrade) * creditval;
                            }
                        }
                        else
                        {
                            if (strgrade != "")
                            {
                                gpacal1 = gpacal1 + (Convert.ToDouble(strgrade) * creditval);
                            }
                        }
                    }
                }
            }
            if (creditsum1 != 0)
            {
                if (finalgpa1 == 0)
                {
                    finalgpa1 = Math.Round((gpacal1 / creditsum1), 2);
                }
                else
                {
                    finalgpa1 = finalgpa1 + Math.Round((gpacal1 / creditsum1), 2);
                }
            }
            creditsum1 = 0;
            gpacal1 = 0;
            creditval = 0;
            strgrade = string.Empty;
        }
        string strlatsem = "select min(semester) as semmin,max(semester) as semmax from subjectchooser where roll_no='" + RollNo + "'";
        dssem = d2.select_method(strlatsem, hat, "TEXT");
        if (dssem != null && dssem.Tables[0] != null && dssem.Tables[0].Rows.Count > 0)
        {
            latsem = Convert.ToString(dssem.Tables[0].Rows[0]["semmin"]);
            latsemmax = Convert.ToString(dssem.Tables[0].Rows[0]["semmax"]);
        }
        int latsemes = 0;
        string calculate = string.Empty;
        if (latsem == "")
        {
            latsem = "0";
        }
        if (latsemmax == "")
        {
            latsemmax = "0";
        }
        if (Convert.ToInt32(semval) >= Convert.ToInt32(latsem) && Convert.ToInt32(latsemmax) >= Convert.ToInt32(semval))
        {
            for (se = Convert.ToInt32(latsem); se <= Convert.ToInt32(semval); se++)
            {
                latsemes = latsemes + 1;
            }
        }
        else if (Convert.ToInt32(semval) >= Convert.ToInt32(latsemmax) && Convert.ToInt32(latsemmax) >= Convert.ToInt32(latsem))
        {
            for (se = Convert.ToInt32(latsem); se <= Convert.ToInt32(latsemmax); se++)
            {
                latsemes = latsemes + 1;
            }
        }
        if (Convert.ToInt32(latmode) == 1)
        {
            calculate = Math.Round((finalgpa1 / Convert.ToInt32(latsemes)), 2).ToString();
        }
        else
        {
            calculate = Math.Round((finalgpa1 / Convert.ToInt32(latsemes)), 2).ToString();
        }
        return calculate;
    }

    public string GetFunction(string sqlQuery)
    {
        string sqlstr;
        sqlstr = sqlQuery;
        con_Getfunc.Close();
        con_Getfunc.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con_Getfunc);
        SqlDataReader drnew;
        SqlCommand funcmd = new SqlCommand(sqlstr);
        funcmd.Connection = con_Getfunc;
        drnew = funcmd.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "0";
        }
    }

    protected string overallperformprint(string mcriteriano, string hcrollno)
    {
        int subjectcount = 0;
        string rankov = string.Empty;
        Session["rank1roll"] = string.Empty;
        string branch = "0";
        string degree = "0";
        if (ddlbranch.Items.Count > 0)
            branch = ddlbranch.SelectedItem.Text;
        if (ddldegree.Items.Count > 0)
            degree = ddldegree.SelectedItem.Text;
        string sem = ddlsemester.SelectedValue;
        string sec = ddlsection.SelectedValue;
        string test = mcriteriano;
        //'-------------------------------------------------------mythili start----------------------------------------'
        //'-------------------------------------------- Query for Get the subjectno,sub code,acronym ,examdate,minmrk,maxmrk,entrydate and examcode
        filteration();
        string filterwithsection = "a.app_no=r.app_no and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + mcriteriano.ToString() + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0   and r.sections='" + ddlsection.SelectedValue.ToString() + "' " + strorder + ",s.subject_no";
        string filterwithoutsection = "a.app_no=r.app_no and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + mcriteriano.ToString() + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0  " + strorder + ",s.subject_no";
        hat.Clear();
        hat.Add("batchyear", ddlbatch.SelectedValue.ToString());
        hat.Add("degreecode", ddlbranch.SelectedValue.ToString());
        hat.Add("criteria_no", mcriteriano.ToString());
        hat.Add("sections", ddlsection.SelectedValue.ToString());
        hat.Add("filterwithsection", filterwithsection.ToString());
        hat.Add("filterwithoutsection", filterwithoutsection.ToString());
        ds2.Clear();
        ds2.Reset();
        ds2 = d2.select_method("PROC_STUD_ALL_SUBMARK", hat, "sp");
        string sections = string.Empty;
        string strsec = string.Empty;
        double find_total = 0;
        int sum_max_mark = 0;
        double percent = 0;
        int fail_sub_cnt = 0;
        int ra_nk = 0;
        sections = ddlsection.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
        {
            strsec = string.Empty;
        }
        else
        {
            strsec = " and registration.sections='" + sections.ToString() + "'";
        }
        ds4.Clear();
        ds4.Reset();
        ds4 = d2.select_method_wo_parameter("Delete_Rank_Table", "sp");
        if (ds2.Tables[0].Rows.Count > 0)
        {
            ds1.Clear();
            ds1.Reset();
            sqlStr = "select distinct len(registration.Roll_No),registration.Roll_No as roll,registration.Reg_No as regno,registration.stud_name as studname,registration.stud_type as studtype,registration.App_No as ApplicationNumber from registration, applyn a,exam_type et,result rt where a.app_no=registration.app_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + "   and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0   " + strsec + " and  rt.exam_code=et.exam_code and registration.roll_no=rt.roll_no and et.criteria_no =" + mcriteriano + " " + strregorder + "";
            ds1 = d2.select_method(sqlStr, hat, "Text");
            int subrow = 0;
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds1.Tables[0].Rows.Count; row++)
                {
                    fail_sub_cnt = 0;
                    find_total = 0;
                    sum_max_mark = 0;
                    for (int j = 0; j < ds2.Tables[1].Rows.Count; j++)
                    {
                        if (subrow < Convert.ToInt32(ds2.Tables[0].Rows.Count))
                        {
                            if (ds1.Tables[0].Rows[row]["roll"].ToString() == ds2.Tables[0].Rows[subrow]["roll"].ToString())
                            {
                                if (ds2.Tables[1].Rows[j]["min_mark"].ToString() != "" && ds2.Tables[1].Rows[j]["min_mark"].ToString() != null)
                                {
                                    if (double.Parse(ds2.Tables[0].Rows[subrow]["mark"].ToString()) != -2 && double.Parse(ds2.Tables[0].Rows[subrow]["mark"].ToString()) != -1 && double.Parse(ds2.Tables[0].Rows[subrow]["mark"].ToString()) != -3 && (double.Parse(ds2.Tables[0].Rows[subrow]["mark"].ToString()) < double.Parse(ds2.Tables[1].Rows[j]["min_mark"].ToString())))
                                    {
                                        fail_sub_cnt++;
                                    }
                                    if (double.Parse(ds2.Tables[0].Rows[subrow]["mark"].ToString()) >= 0 && (double.Parse(ds2.Tables[0].Rows[subrow]["mark"].ToString()) >= Convert.ToDouble(ds2.Tables[1].Rows[j]["min_mark"].ToString())))
                                    {
                                        //'---------------total,percent,rank-------------------------------
                                        find_total = (Convert.ToDouble(find_total) + Convert.ToDouble(ds2.Tables[0].Rows[subrow]["mark"].ToString()));
                                        sum_max_mark = sum_max_mark + Convert.ToInt32(ds2.Tables[1].Rows[j]["max_mark"].ToString());
                                        percent = Convert.ToDouble((Convert.ToDouble(find_total) / sum_max_mark) * 100);
                                        if (Convert.ToString(percent) == "Infinity")
                                            percent = 0.0;
                                    }
                                }
                            }
                        }
                        subrow++;
                    }
                    if (fail_sub_cnt == 0)
                    {
                        hat.Clear();
                        hat.Add("RollNumber", ds1.Tables[0].Rows[row]["roll"].ToString());
                        hat.Add("criteria_no", mcriteriano.ToString());
                        hat.Add("Total", find_total.ToString());
                        hat.Add("avg", percent.ToString());
                        hat.Add("rank", "");
                        int o = d2.insert_method("INSERT_RANK", hat, "sp");
                    }
                }
                //'--------------------------------insert the rank---------------------------------
                ra_nk = 1;
                ds3.Clear();
                ds3.Reset();
                ds3 = d2.select_method_wo_parameter("SELECT_RANK", "sp");
                if (ds3.Tables[0].Rows.Count != 0)
                {
                    //---------------------new rank 030412
                    double temp_rank = 0;
                    int zx = 1;
                    for (int rank = 0; rank < ds3.Tables[0].Rows.Count; rank++)
                    {
                        if (temp_rank == 0)
                        {
                            ra_nk = 1;
                            hat.Clear();
                            hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                            hat.Add("criteria_no", ddlbranch.SelectedValue.ToString());
                            hat.Add("Total", Convert.ToString(find_total));
                            hat.Add("avg", Convert.ToString(percent));
                            hat.Add("rank", ra_nk.ToString());
                            int o = d2.insert_method("INSERT_RANK", hat, "sp");
                            temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                            if (hcrollno == ds3.Tables[0].Rows[rank]["Rollno"].ToString())
                            {
                                break;
                            }
                        }
                        else if (temp_rank != 0)
                        {
                            if (temp_rank > double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString()))
                            {
                                //   ra_nk += 1;
                                ra_nk = zx;
                                hat.Clear();
                                hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                                hat.Add("criteria_no", ddlbranch.SelectedValue.ToString());
                                hat.Add("Total", Convert.ToString(find_total));
                                hat.Add("avg", Convert.ToString(percent)); ;
                                hat.Add("rank", ra_nk.ToString());
                                int o = d2.insert_method("INSERT_RANK", hat, "sp");
                                temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                                if (hcrollno == ds3.Tables[0].Rows[rank]["Rollno"].ToString())
                                {
                                    break;
                                }
                            }
                            else if (temp_rank == double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString()))
                            {
                                hat.Clear();
                                hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                                hat.Add("criteria_no", ddlbranch.SelectedValue.ToString());
                                hat.Add("Total", Convert.ToString(find_total));
                                hat.Add("avg", Convert.ToString(percent));
                                hat.Add("rank", ra_nk.ToString());
                                int o = d2.insert_method("INSERT_RANK", hat, "sp");
                                temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                                if (hcrollno == ds3.Tables[0].Rows[rank]["Rollno"].ToString())
                                {
                                    break;
                                }
                            }
                        }
                        zx++;
                    }
                }
            }
            if (ds3.Tables[0].Rows.Count > 0)
            {
                if (ds3.Tables[0].Rows.Count != 0)
                {
                    ds3.Clear();
                    ds3.Reset();
                    ds3 = d2.select_method_wo_parameter("SELECT_RANK", "sp");
                    for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    {
                        if (hcrollno == ds3.Tables[0].Rows[i]["rollno"].ToString())
                        {
                            rnkv = ds3.Tables[0].Rows[i]["Rank"].ToString();
                            break;
                        }
                    }
                }
            }
        }
        rankov = rnkv;
        return rankov;
    }

    public void persentmonthcal(string examdate, string strtdate)
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
            int demfcal, demtcal;
            string monthcal;
            conduct_hour_new = 0;
            //  if (rows_count == 0)
            {
                string dt = strtdate;//"semstartdate"
                string[] dsplit = dt.Split(new Char[] { '/' });
                strtdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                demfcal = int.Parse(dsplit[2].ToString());
                demfcal = demfcal * 12;
                cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                monthcal = cal_from_date.ToString();
                dt = examdate;//examconductdate
                dsplit = dt.Split(new Char[] { '/' });
                examdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                demtcal = int.Parse(dsplit[2].ToString());
                demtcal = demtcal * 12;
                cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
                per_from_date = Convert.ToDateTime(strtdate);
                per_to_date = Convert.ToDateTime(examdate);
            }
            dumm_from_date = per_from_date;
            string admdate = admdatev.ToString();//rows_count===0 coz roll no fror loop
            string[] admdatesp = admdate.Split(new Char[] { '/' });
            admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
            Admission_date = Convert.ToDateTime(admdate);
            dd = hcrollno.ToString();
            hat.Clear();
            hat.Add("std_rollno", hcrollno);
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
                hat.Add("from_date", strtdate.ToString());
                hat.Add("to_date", examdate.ToString());
                hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));
                //------------------------------------------------------------------
                int iscount = 0;
                holidaycon.Close();
                holidaycon.Open();
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + strtdate.ToString() + "' and '" + examdate.ToString() + "' and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsemester.SelectedItem.ToString() + "";
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
                    int k = 0;
                lbl:
                    for (; k < ds3.Tables[2].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        string vholidaydate = string.Empty;
                        vholidaydate = dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0];
                        if (holiday_table31.Contains(Convert.ToString(vholidaydate)))
                        {
                            k = k + 1;
                            goto lbl;
                        }
                        else
                        {
                            holiday_table31.Add(vholidaydate, k);
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
                        holiday_table3.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                    }
                }
                //=====================================14/5/12 PRABHA
                con.Close();
                cmd.CommandText = "select rights from  special_hr_rights where usercode=" + Session["usercode"].ToString() + "";
                cmd.Connection = con;
                con.Open();
                SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
                if (dr_rights_spl_hr.HasRows)
                {
                    while (dr_rights_spl_hr.Read())
                    {
                        string spl_hr_rights = string.Empty;
                        Hashtable od_has = new Hashtable();
                        spl_hr_rights = dr_rights_spl_hr["rights"].ToString();
                        if (spl_hr_rights == "True" || spl_hr_rights == "true")
                        {
                            splhr_flag = true;
                        }
                    }
                }
            }
            //===================================
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
                            getspecial_hr();
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
        catch (Exception evh)
        {
            string evhstr = evh.ToString();
        }
    }

    public void getspecial_hr()
    {
        //  try
        {
            con_splhr_query_master.Close();
            con_splhr_query_master.Open();
            DataSet ds_splhr_query_master = new DataSet();
            //  no_stud_flag = false;
            string splhr_query_master = "select attendance from specialhr_attendance sa,registration r where hrdet_no in(select hrdet_no from specialhr_details where hrentry_no=(select hrentry_no from  specialhr_master where batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlsemester.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and date='" + dumm_from_date + "')  ) and r.roll_no=sa.roll_no and batch_year=" + ddlbatch.SelectedValue.ToString() + " and current_semester=" + ddlsemester.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + "  and (CC = 0)  AND (DelFlag = 0)  AND (Exam_Flag <> 'debar') and sa.roll_no='" + dd + "' ";
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
        //  catch
        {
        }
    }

    public void bindcollege(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Session["QueryString"] = string.Empty;
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
                dsprint = d2.select_method("bind_college", hat, "sp");
                ddlcollege.Items.Clear();
                if (dsprint.Tables[0].Rows.Count > 0)
                {
                    ddlcollege.DataSource = dsprint;
                    ddlcollege.DataTextField = "collname";
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataBind();
                    ddlcollege_SelectedIndexChanged(sender, e);
                }
                Pageload(sender, e);
            }
        }
        catch
        {
        }
    }

    public void Pageload(object sender, EventArgs e)
    {
        usercode = Session["usercode"].ToString();
        collegecode = Session["InternalCollegeCode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        //--------Spread Design Format-----------

        Showgrid.Visible = false;
        btnxl.Visible = false;
        //Added By Srinath 28/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        btnprint.Visible = false;
        chkGridSelectAll.Visible = false;
        //btnprint1.Visible = false;
        norecordlbl.Visible = false;
        if (ddlcollege.Items.Count >= 1)
        {
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            if (ddldegree.Items.Count > 0)
            {
                BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                BindSem(strbranch, strbatchyear, collegecode);
                BindSectionDetail(strbatch, strbranch);
            }
        }
    }

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

    public void BindSubjecttest(string strbatch, string strbranch, string strsem, string strsec)
    {
        try
        {
            txtsubject.Text = "---Select---";
            chksubject.Checked = false;
            chklstsubject.Items.Clear();
            if (ddlsection.Text.ToString() == "All" || ddlsection.Text.ToString() == string.Empty || ddlsection.Text.ToString() == "-1")
            {
                strsec = string.Empty;
                strsec1 = string.Empty;
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
                htb.Clear();
                htsubjcide.Clear();
                //chklstsubject.SelectedIndex = chklstsubject.Items.Count - 1;
                for (int i = 0; i < chklstsubject.Items.Count; i++)
                {
                    string subjno = "", subjtype = "", subjcode = string.Empty;
                    subjno = dsmethodgosubj.Tables[0].Rows[i]["subject_no"].ToString();
                    subjtype = dsmethodgosubj.Tables[0].Rows[i]["subject_type"].ToString();
                    subjcode = dsmethodgosubj.Tables[0].Rows[i]["subject_code"].ToString();
                    if (htb.Contains(Convert.ToString(subjno)))
                    {
                        string subjtypeve = Convert.ToString(GetCorrespondingKey(Convert.ToString(subjno), htb));
                        htb[Convert.ToString(subjno)] = subjtypeve;
                    }
                    else
                    {
                        htb.Add(Convert.ToString(subjno), subjtype);
                    }
                    if (htsubjcide.Contains(Convert.ToString(subjno)))
                    {
                        string subjcodeve = Convert.ToString(GetCorrespondingKey(Convert.ToString(subjno), htsubjcide));
                        htsubjcide[Convert.ToString(subjno)] = subjcodeve;
                    }
                    else
                    {
                        htsubjcide.Add(Convert.ToString(subjno), subjcode);
                    }
                    chklstsubject.Items[i].Selected = true;
                    if (chklstsubject.Items[i].Selected == true)
                    {
                        count4 += 1;
                    }
                }
                if (count4 > 0)
                {
                    txtsubject.Text = "Subject(" + count4 + ")";
                    if (chklstsubject.Items.Count == count4)
                    {
                        chksubject.Checked = true;
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

    public void Bindtest(string strbatch, string strbranch, string strsem, string strsec1)
    {
        try
        {
            chktest.Checked = false;
            txttest.Text = "---Select---";
            cbltest.Items.Clear();
            if (ddlsection.Text.ToString() == "All" || ddlsection.Text.ToString() == string.Empty || ddlsection.Text.ToString() == "-1")
            {
                strsec1 = string.Empty;
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
                //chklstsubject.SelectedIndex = chklstsubject.Items.Count - 1;
                for (int i = 0; i < cbltest.Items.Count; i++)
                {
                    cbltest.Items[i].Selected = true;
                    if (cbltest.Items[i].Selected == true)
                    {
                        countv += 1;
                    }
                }
                if (countv > 0)
                {
                    txttest.Text = "Test(" + countv + ")";
                    if (cbltest.Items.Count == countv)
                    {
                        chktest.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            norecordlbl.Visible = true;
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

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Showgrid.Visible = false;
            norecordlbl.Visible = false;
            btnxl.Visible = false;
            //Added By Srinath 28/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            BindDegree(singleuser, group_user, collegecode, usercode);
            if (ddldegree.Items.Count > 0)
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
            //Added By Srinath 28/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            if (ddldegree.Items.Count > 0)
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

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        norecordlbl.Visible = false;
        btnxl.Visible = false;
        //Added By Srinath 28/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        if (!Page.IsPostBack == false)
        {
            ddlsemester.Items.Clear();
        }
        try
        {
            if ((ddlbranch.SelectedIndex != 0) && (ddlbranch.SelectedIndex > 0))
            {
                BindSem(strbranch, strbatchyear, collegecode);
                BindSectionDetail(strbatch, strbranch);
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }

    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            norecordlbl.Visible = false;
            Showgrid.Visible = false;
            btnxl.Visible = false;
            //Added By Srinath 28/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
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
        //Added By Srinath 28/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        norecordlbl.Visible = false;
        Showgrid.Visible = false;
        BindSubjecttest(strbatch, strbranch, strsem, strsec);
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

    protected void chksubject_CheckedChanged(object sender, EventArgs e)
    {
        if (chksubject.Checked == true)
        {
            for (int i = 0; i < chklstsubject.Items.Count; i++)
            {
                chklstsubject.Items[i].Selected = true;
            }
            txtsubject.Text = "Subject(" + (chklstsubject.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklstsubject.Items.Count; i++)
            {
                chklstsubject.Items[i].Selected = false;
            }
            txtsubject.Text = "---Select---";
        }
    }

    protected void chklstsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        psubject.Focus();
        chksubject.Checked = false;
        txtsubject.Text = "---Select---";
        int subjectcount = 0;
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < chklstsubject.Items.Count; i++)
        {
            if (chklstsubject.Items[i].Selected == true)
            {
                value = chklstsubject.Items[i].Text;
                code = chklstsubject.Items[i].Value.ToString();
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

    protected void chktest_CheckedChanged(object sender, EventArgs e)
    {
        if (chktest.Checked == true)
        {
            for (int i = 0; i < cbltest.Items.Count; i++)
            {
                cbltest.Items[i].Selected = true;
            }
            txttest.Text = "Test(" + (cbltest.Items.Count) + ")";
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
        chktest.Checked = false;
        txttest.Text = "---Select---";
        psubject.Focus();
        int subjectcount = 0;
        string value = string.Empty;
        string code = string.Empty;
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
            txttest.Text = "Test(" + subjectcount.ToString() + ")";
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

    protected void btnxl_Click(object sender, EventArgs e)
    {
        string reportname = txtexcelname.Text.ToString().Trim();
        if (reportname != "")
        {
            d2.printexcelreportgrid(Showgrid, reportname);
            lblerr.Visible = false;
        }
        else
        {
            lblerr.Text = "Please Enter Your Report Name";
            lblerr.Visible = true;
        }
    }

    protected void btnclose_Click(object sender, EventArgs e)
    {
        Showgrid.Visible = true;
        panelv.Visible = false;
        btnprint.Visible = true;
        chkGridSelectAll.Visible = true;
        //btnprint1.Visible = true;
        //Added By Srinath 25/4/2013
        for (int res = 0; res <= Convert.ToInt32(Showgrid.Rows.Count) - 1; res++)
        {
            // FpSpread2.Sheets[0].Cells[res, 8].Value = "0";
        }

    }



}
