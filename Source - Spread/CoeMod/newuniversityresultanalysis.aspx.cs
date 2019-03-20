using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;
using InsproDataAccess;

public partial class newuniversityresultanalysis : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection newcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection newcon1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_convertgrade = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Grade = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_subcrd = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Hashtable hat = new Hashtable();
    DataSet ds_load = new DataSet();
    DAccess2 daccess = new DAccess2();

    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();

    SqlCommand cmd;
    string gsum = string.Empty;
    bool flagchknew = true;
    string attendancerollnum = string.Empty;
    string degree_code = string.Empty;
    string current_sem = string.Empty;
    string batch_year = string.Empty;
    string getgradeflag = string.Empty;
    string stgetgradeflag = string.Empty;
    string exam_month = string.Empty;
    string exam_year = string.Empty;
    string getsubno = string.Empty;
    string gchm = "0";
    Boolean InsFlag;
    int semdec = 0;
    string sections = string.Empty;
    string strsec = string.Empty;
    int IntExamCode = 0;
    int ExamCode = 0;
    string grade_setting = string.Empty;
    string strStudents = string.Empty;
    string funcsubno = string.Empty;
    string funcsubname = string.Empty;
    string funcsubcode = string.Empty;
    string funcresult = string.Empty;
    string funcsemester = string.Empty;
    string funccredit = string.Empty;
    string funcgrade = string.Empty;
    string previousgrade = string.Empty;
    string mark = string.Empty;
    string studadmdate = string.Empty;
    //attendance
    DateTime per_from_date;
    DateTime per_to_date;
    DateTime dumm_from_date;
    string frdate, todate1;
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
    int i, rows_count;
    string tempvalue = "-1";
    int ObtValue = -1;
    int cal_from_date;
    int cal_to_date;
    double per_perhrs, per_abshrs, cum_perhrs, cum_abshrs;
    double per_ondu, per_leave, per_hhday, cum_ondu, cum_leave, cum_hhday;
    int student = 0;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    double halfday = 0;
    int moncount;
    double dif_date = 0;
    double dif_date1 = 0;
    int abs = 0, att = 0;
    int dum_diff_date, unmark;
    double workingdays = 0;
    double per_workingdays = 0;
    double cum_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double cum_present_date, cum_ondu_date, cum_leave_date, cum_absent_date;
    double per_tage_date, cum_tage_date, per_tage_hrs, cum_tage_hrs;
    double cum_tot_point, per_holidate, cum_per_holidate;
    int per_dum_unmark, cum_dum_unmark, dum_unmark;
    int tot_per_hrs, per_tot_per_hrs, cum_per_tot_per, tot_wok_hrs;
    double per_con_hrs, cum_con_hrs;
    double njhr, njdate, per_njhr, per_njdate, cum_njhr, cum_njdate;
    double per_per_hrs, cum_per_perhrs;
    double tot_ondu, per_tot_ondu, cum_tot_ondu;
    int countds = 0;
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds5 = new DataSet();
    DataSet ds6 = new DataSet();
    DataSet ds7 = new DataSet();
    DataSet ds8 = new DataSet();
    DataSet ds9 = new DataSet();
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
    int strdate = 0;
    int subno = 0;
    string roll = string.Empty;
    string dateformat1 = string.Empty;
    string dateformat2 = string.Empty;
    string dateconcat = string.Empty;
    string date1concat = string.Empty;
    int stucount;
    int categrycount = 0;
    Boolean IsFlag = false;
    Boolean IsSetFlag = false;
    string strorder = string.Empty;
    string strregorder = string.Empty;
    DataSet ds_sphr = new DataSet();
    static Hashtable ht_sphr = new Hashtable();
    string tempdegreesem = string.Empty;
    string chkdegreesem = string.Empty;
    Boolean splhr_flag = false;
    Boolean datechk = false;
    int tempcallfromdate = 0;
    string tempfromdate = string.Empty;
    String tempdegreesempresent = string.Empty;
    static string grouporusercode = string.Empty;
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    string halforfull = "", mng = "", evng = "", holiday_sched_details = string.Empty;
    string value_holi_status = string.Empty;
    string split_holiday_status_1 = "", split_holiday_status_2 = string.Empty;
    string[] split_holiday_status = new string[1000];
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0;
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    int notconsider_value = 0;
    double tot_ml, per_tot_ml;
    double conduct_hour_new = 0;
    double spl_tot_condut = 0;
    string spsection = string.Empty;

    //attendance
    //adding datatable to check grade

    DataTable dgrades = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //FpExternal.Visible = true;
            lastdiv.Visible = true;
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("CoeHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/CoeMod/CoeHome.aspx");
                    return;
                }
            }
            //****************************************************//
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            collegecode = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            Session["Semester"] = Convert.ToString(ddlSemYr.SelectedValue);
            if (!IsPostBack)
            {
                chkonlyrevaluation.Enabled = false;
                FpExternal.CommandBar.Visible = false;
                txtfrm_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtend_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                FpExternal.Visible = false;
                btnExcel.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                btnprintmaster.Visible = false;
                collegecode = Session["collegecode"].ToString();
                usercode = Session["usercode"].ToString();
                singleuser = Session["single_user"].ToString();
                group_user = Session["group_code"].ToString();
                string Master1 = string.Empty;
                load_college();
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                bindsec();
                bindexammonth();
                bindexamyear();
                Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                con.Close();
                con.Open();
                SqlDataReader mtrdr;
                SqlCommand mtcmd = new SqlCommand(Master1, con);
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
                    }
                }
            }
        }
        catch(Exception ex)
        {
        }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
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
            //     ddlSemYr.Items.Clear();
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
        //    ddlSemYr.Items.Insert(0, new ListItem("--Select--", "-1"));
        con.Close();
    }

    public void bindexammonth()
    {
        ddlMonth.Items.Clear();
        SqlDataReader drexamyear;
        con.Close();
        con.Open();
        ddlYear.Items.Clear();
        string yearquery = "select distinct exam_month from exam_details where batch_year='" + ddlBatch.SelectedItem.Text + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and current_semester='" + ddlSemYr.SelectedItem.Text + "'";
        SqlCommand cmdyearquery = new SqlCommand(yearquery, con);
        drexamyear = cmdyearquery.ExecuteReader();
        while (drexamyear.Read())
        {
            int exammonth = Convert.ToInt16(drexamyear["exam_month"].ToString());
            string monthtext = bindmonthname(exammonth);
            ddlMonth.Items.Add(new System.Web.UI.WebControls.ListItem(monthtext.ToString(), exammonth.ToString()));
        }
        ddlMonth.Items.Add("Select");
    }

    public string bindmonthname(int mon)
    {
        int value = mon;
        string textvalue = string.Empty;
        switch (value)
        {
            case 1:
                textvalue = "Jan";
                break;
            case 2:
                textvalue = "Feb";
                break;
            case 3:
                textvalue = "Mar";
                break;
            case 4:
                textvalue = "Apr";
                break;
            case 5:
                textvalue = "May";
                break;
            case 6:
                textvalue = "Jun";
                break;
            case 7:
                textvalue = "Jul";
                break;
            case 8:
                textvalue = "Aug";
                break;
            case 9:
                textvalue = "Sep";
                break;
            case 10:
                textvalue = "Oct";
                break;
            case 11:
                textvalue = "Nov";
                break;
            case 12:
                textvalue = "Dec";
                break;
        }
        return textvalue;
    }

    public void bindexamyear()
    {
        ddlYear.Items.Clear();
        SqlDataReader drexamyear;
        con.Close();
        con.Open();
        ddlYear.Items.Clear();
        string yearquery = "select distinct exam_year from exam_details where batch_year='" + ddlBatch.SelectedItem.Text + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and current_semester='" + ddlSemYr.SelectedItem.Text + "'";
        SqlCommand cmdyearquery = new SqlCommand(yearquery, con);
        drexamyear = cmdyearquery.ExecuteReader();
        while (drexamyear.Read())
        {
            ddlYear.Items.Add(drexamyear["exam_year"].ToString());
        }
        ddlYear.Items.Add("Select");
    }

    public void bindbatch()
    {
        ddlBatch.Items.Clear();
        ds_load = daccess.select_method_wo_parameter("bind_batch", "sp");
        int count = ds_load.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlBatch.DataSource = ds_load;
            ddlBatch.DataTextField = "batch_year";
            ddlBatch.DataValueField = "batch_year";
            ddlBatch.DataBind();
            //ddlYear.DataSource = ds_load;
            //ddlYear.DataTextField = "batch_year";
            //ddlYear.DataValueField = "batch_year";
            //ddlYear.DataBind();
        }
        int count1 = ds_load.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds_load.Tables[1].Rows[0][0].ToString());
            ddlBatch.SelectedValue = max_bat.ToString();
            con.Close();
        }
    }

    void load_college()
    {
        con.Open();
        SqlDataAdapter da_college = new SqlDataAdapter("select distinct collname,college_code from collinfo", con);
        DataTable dt_college = new DataTable();
        da_college.Fill(dt_college);
        ddl_college.DataSource = dt_college;
        ddl_college.DataTextField = "collname";
        ddl_college.DataValueField = "college_code";
        ddl_college.DataBind();
        con.Close();
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
        ds_load = daccess.select_method("bind_branch", hat, "sp");
        int count2 = ds_load.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlBranch.DataSource = ds_load;
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
        ds_load = daccess.select_method("bind_degree", hat, "sp");
        int count1 = ds_load.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            ddlDegree.DataSource = ds_load;
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
        ds_load = daccess.select_method("bind_sec", hat, "sp");
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
        ddlSec.Items.Add("ALL");//@@@@@@ added on 29.06.12
    }

    public int LoadSubject(int intExamCode)
    {
         dgrades.Columns.Clear();
            dgrades.Rows.Clear();
            DataColumn dc;
            dc = new DataColumn();
            dc.ColumnName = "Sno";
            dgrades.Columns.Add(dc);
            dc = new DataColumn();
            dc.ColumnName = "subjectno";
            dgrades.Columns.Add(dc);
            dc = new DataColumn();
            dc.ColumnName = "grade";
            dgrades.Columns.Add(dc);
            dc = new DataColumn();
            dc.ColumnName = "count";
            dgrades.Columns.Add(dc);
            dc = new DataColumn();
            dc.ColumnName = "subtypenumber";
            dgrades.Columns.Add(dc);
            DataRow dr;
            int markstcount = 0;
            int i = 0;
            int IntSCount = 0;
            int Stno = 0;
            string Stype = string.Empty;
            string strsubject = string.Empty;
            string grade = string.Empty;
            string degree_code = string.Empty;
            string current_sem = string.Empty;
            string batch_year = string.Empty;
            int slno = 0;
            degree_code = ddlBranch.SelectedValue.ToString();
            current_sem = ddlSemYr.SelectedValue.ToString();
            batch_year = ddlBatch.SelectedValue.ToString();
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "S.No";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 2, 0].Text = "S.No";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 3, 0].Text = "S.No";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "RollNo";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 2, 1].Text = "RollNo";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 3, 1].Text = "RollNo";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "RegNo";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 2, 2].Text = "RegNo";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 3, 2].Text = "RegNo";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "Student Name";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 2, 3].Text = "Student Name";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 3, 3].Text = "Student Name";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "M/F";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 2, 4].Text = "M/F";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 3, 4].Text = "M/F";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "Student Type";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 2, 5].Text = "Student Type";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 3, 5].Text = "Student Type";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 6].Text = "Seat Type";//rrr
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 2, 6].Text = "Seat Type";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 3, 6].Text = "Seat Type";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 0].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 0].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 2].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 2].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 3].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 3].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 4].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 4].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 5].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 5].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 5].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 5].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 6].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 6].Font.Bold = true; //rrrr
            //get grade
            ArrayList albindgrade = new ArrayList();
            SqlDataReader sqldr;
            string fillgrade = "select Mark_Grade from Grade_Master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "'";
            con.Close();
            con.Open();
            SqlCommand sqlcm = new SqlCommand(fillgrade, con);
            sqldr = sqlcm.ExecuteReader();
            while (sqldr.Read())
            {
                albindgrade.Add(Convert.ToString(sqldr["Mark_Grade"]));
            }
            //end
            strsubject = "Select distinct subject.mintotal as mintot,subject.mintotal as mintot,subject.min_int_marks as mimark, subject.min_ext_marks as mxmark,subject.maxtotal as maxtot,subject.acronym as subacr,subject_name,subject_code as Subject_Code,mark_entry.subject_no as Subject_No,semester,subject_type as Subtype,credit_points,sub_sem.lab as chlab,subject.subtype_no as typeno from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = '" + intExamCode + "' and (attempts='1' or attempts='0') order by semester desc,subject.subtype_no  asc";
            con.Close();
            con.Open();
            SqlCommand cmd_loadSub = new SqlCommand(strsubject, con);
            SqlDataReader dr_loadSub;
            dr_loadSub = cmd_loadSub.ExecuteReader();
            int subcoltheory = 0;
            int subcolprac = 0;
            int chkcou = 0;
            int spch = 0;
            string presubjecttype = string.Empty;
            string nextsubjecttype = string.Empty;
            int startspan = 8;
            int numberofcols = 0;
            int sc = 0;
            int totcredits = 0;
            while (dr_loadSub.Read())
            {
                numberofcols++;
                markstcount++;
                IntSCount++;
                FpExternal.Sheets[0].ColumnCount += 1;
                FpExternal.Sheets[0].Columns[FpExternal.Sheets[0].ColumnCount - 1].Width = 10;
                if (spch == 0)
                {
                    startspan = FpExternal.Sheets[0].ColumnCount - 1;
                }
                spch++;
                string subtype = string.Empty;
                subtype = dr_loadSub["chlab"].ToString();
                nextsubjecttype = Convert.ToString(dr_loadSub["typeno"]);
                if (subtype == "0" || subtype == "0")
                {
                    subcoltheory++;
                }
                else
                {
                    subcolprac++;
                }
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].Text = dr_loadSub["Subtype"].ToString();
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].Tag = dr_loadSub["Subject_No"].ToString();
                if (albindgrade.Count > 0)
                {
                    for (int cn = 0; cn < albindgrade.Count; cn++)
                    {
                        slno++;
                        dr = dgrades.NewRow();
                        dr["Sno"] = slno.ToString();
                        dr["subjectno"] = Convert.ToString(dr_loadSub["Subject_No"]);
                        dr["grade"] = albindgrade[cn].ToString();
                        dr["subtypenumber"] = Convert.ToString(dr_loadSub["typeno"]);
                        dr["count"] = 0;
                        dgrades.Rows.Add(dr);
                    }
                }

                string getcredits = Convert.ToString(dr_loadSub["credit_points"]);//1245
                totcredits += Convert.ToInt32(getcredits);
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].Note = dr_loadSub["mintot"].ToString();
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 2, FpExternal.Sheets[0].ColumnCount - 1].Text = dr_loadSub["Subject_Code"].ToString() + "(" + getcredits+")";
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 2, FpExternal.Sheets[0].ColumnCount - 1].Tag = dr_loadSub["mintot"].ToString();
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 2, FpExternal.Sheets[0].ColumnCount - 1].Note = dr_loadSub["mimark"].ToString();
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Text = dr_loadSub["subacr"].ToString();
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Tag = dr_loadSub["maxtot"].ToString();
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Note = dr_loadSub["mxmark"].ToString();
                if (chkcou != 0 && nextsubjecttype != presubjecttype)
                {
                    if (sc == 0)
                    {
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, startspan, 1, numberofcols - 1);
                        startspan = 0;
                        numberofcols = 1;
                        subcoltheory = 0;
                        spch = 0;
                    }
                    else
                    {
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, startspan - 1, 1, numberofcols - 1);
                        startspan = 0;
                        numberofcols = 1;
                        subcoltheory = 0;
                        spch = 0;
                    }
                    sc++;
                }
                presubjecttype = nextsubjecttype;
                chkcou++;
            }
            if (numberofcols > 0 && startspan > 0)
            {
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, startspan - 1, 1, numberofcols);
            }
            if (markstcount == 0)
            {
                FpExternal.Visible = false;
                btnExcel.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                btnprintmaster.Visible = false;
                lblerrormsg.Text = "No Record Found";
                lblerrormsg.Visible = true;
                return 0;
            }
            else
            {
                FpExternal.Sheets[0].ColumnCount++;
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Text = "Current Arrears";
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 2, FpExternal.Sheets[0].ColumnCount - 1].Text = "Current Arrears";
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].Text = "Current Arrears";
                FpExternal.Sheets[0].ColumnCount++;
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Text = "P/F (" + Convert.ToString(totcredits)+")";
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 2, FpExternal.Sheets[0].ColumnCount - 1].Text = "P/F (" + Convert.ToString(totcredits) + ")";
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].Text = "P/F (" + Convert.ToString(totcredits) + ")";
                FpExternal.Sheets[0].ColumnCount++;
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Text = "% of Attendance";
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 2, FpExternal.Sheets[0].ColumnCount - 1].Text = "% of Attendance";
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].Text = "% of Attendance";
                FpExternal.Sheets[0].ColumnCount++;
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Text = "Total Arrears";
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 2, FpExternal.Sheets[0].ColumnCount - 1].Text = "Total Arrears";
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].Text = "Total Arrears";
                FpExternal.Sheets[0].ColumnCount++;
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Text = "SUM";
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 2, FpExternal.Sheets[0].ColumnCount - 1].Text = "SUM";
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].Text = "SUM";
                FpExternal.Sheets[0].ColumnCount++;
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Text = "GPA";
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 2, FpExternal.Sheets[0].ColumnCount - 1].Text = "GPA";
                FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].Text = "GPA";
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 3, 1);
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 3, 1);//rrrr
                // FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 3, 1);
                //FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, subcoltheory);
                //if (subcolprac != 0)
                //{
                //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, subcoltheory + 6, 1, subcolprac);
                //}
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 1, 3, 1);
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 2, 3, 1);
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 3, 3, 1);
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 4, 3, 1);
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 5, 3, 1);
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 6, 3, 1);
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 7, 3, 1);//rrrr
                //if (Session["Rollflag"].ToString() == "0")
                //{
                //    FpExternal.Sheets[0].ColumnHeader.Columns[1].Visible = false;
                //}
                //if (Session["Regflag"].ToString() == "0")
                //{
                //    FpExternal.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                //}
                //if (Session["Studflag"].ToString() == "0")
                //{
                //    FpExternal.Sheets[0].ColumnHeader.Columns[5].Visible = false;
                //}
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 11;
                style.Font.Bold = true;
                FpExternal.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpExternal.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpExternal.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                return IntSCount;
            }
        
      
    }

    public int Get_UnivExamCode(int DegreeCode, int Semester, int Batch, int exammonth, int examyear)
    {
        string GetUnivExamCode = string.Empty;
        string strExam_code = string.Empty;
        strExam_code = "Select Exam_Code from Exam_Details where Degree_Code ='" + DegreeCode.ToString() + "' and Current_Semester = '" + Semester.ToString() + "' and Batch_Year = '" + Batch.ToString() + "' and exam_month='" + exammonth + "' and exam_year='" + examyear + "'";
        con.Close();
        con.Open();
        SqlDataReader dr_examcode;
        SqlCommand cmd_examcode = new SqlCommand(strExam_code, con);
        dr_examcode = cmd_examcode.ExecuteReader();
        while (dr_examcode.Read())
        {
            if (dr_examcode.HasRows == true)
            {
                if (dr_examcode["Exam_Code"].ToString() != "")
                {
                    GetUnivExamCode = dr_examcode["Exam_Code"].ToString();
                }
            }
        }
        if (GetUnivExamCode != "")
        {
            return Convert.ToInt32(GetUnivExamCode);
        }
        else
        {
            return 0;
        }
    }

    public void clear()
    {
        ddlSemYr.Items.Clear();
        ddlSec.Items.Clear();
    }

    public void Get_Semester()
    {
        Boolean first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;
        string batch = ddlBatch.SelectedValue.ToString();
        collegecode = Session["collegecode"].ToString();
        string degree = ddlBranch.SelectedValue.ToString();
        batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();
        DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());
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

    public string GetFunction(string sqlQuery)
    {
        string sqlstr;
        sqlstr = sqlQuery;
        con.Close();
        con.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
        SqlDataReader drnew;
        SqlCommand funcmd = new SqlCommand(sqlstr);
        funcmd.Connection = con;
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

    public int GetSemester_AsNumber(int IpValue)
    {
        InsFlag = false;
        string strinssetting = string.Empty;
        string VarProcessValue = string.Empty;
        int GetSemesterAsNumber = 0;
        strinssetting = "select * from inssettings where LinkName='Semester Display'";
        con.Close();
        con.Open();
        SqlCommand cmd_ins = new SqlCommand(strinssetting, con);
        SqlDataReader dr_ins;
        dr_ins = cmd_ins.ExecuteReader();
        while (dr_ins.Read())
        {
            if (dr_ins.HasRows == true)
            {
                if (dr_ins["LinkName"].ToString() == "Semester Display")
                {
                    InsFlag = true;
                }
                if (Convert.ToInt32(dr_ins["LinkValue"]) == 0)
                {
                    GetSemesterAsNumber = IpValue;
                }
                else if (Convert.ToInt32(dr_ins["LinkValue"]) == 1)
                {
                    VarProcessValue = Convert.ToString(IpValue).Trim();
                }
            }
        }
        return IpValue;
    }

    public void persentmonthcal()
    {
        Boolean isadm = false;
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
                frdate = txtfrm_date.Text.ToString();
                todate = txtend_date.Text.ToString();
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
            string admdate = studadmdate.ToString();
            string[] admdatesp = admdate.Split(new Char[] { '/' });
            admdate = admdatesp[1].ToString() + "/" + admdatesp[0].ToString() + "/" + admdatesp[2].ToString();
            Admission_date = Convert.ToDateTime(admdate);
            hat.Clear();
            hat.Add("std_rollno", attendancerollnum.ToString());
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds6 = daccess.select_method("STUD_ATTENDANCE", hat, "sp");
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
                con.Close();
                con.Open();
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedItem.ToString() + "";
                SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, con);
                SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
                DataSet dsholiday = new DataSet();
                daholiday.Fill(dsholiday);
                if (dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                }
                hat.Add("iscount", iscount);
                ds7 = daccess.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
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
                    ds_sphr = daccess.select_method(splhrrightquery, hat, "Text");
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
                                dumm_from_date = dumm_from_date.AddDays(1);
                                if (dumm_from_date.Day == 1)
                                {
                                    cal_from_date++;
                                    if (moncount > next)
                                    {
                                        next++; //  next++;
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
            per_tot_ondu = tot_ondu;
            per_tot_ml = tot_ml;
            per_njdate = njdate;
            pre_present_date = Present - njdate;
            per_per_hrs = tot_per_hrs;
            per_absent_date = Absent;
            pre_ondu_date = Onduty;
            pre_leave_date = Leave;
            per_workingdays = workingdays - per_njdate;
            per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value - dum_unmark; //dum_unmark hided on 08.08.12 // ((Convert.ToInt16(njdate_mng) * fnhrs) + (Convert.ToInt16(njdate_evng) * (NoHrs - fnhrs)));
            per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value; //added on 08.08.12,notconsider value added for hr suspension,09.08.12 mythili
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
            ds_splhr_query_master = daccess.select_method(splhr_query_master, hat, "Text");
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

    protected void ddl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpExternal.Visible = false;
            lastdiv.Visible = false;
            lblnorec.Visible = false;
        }
        catch
        {
        }
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpExternal.Visible = false;
            lastdiv.Visible = false;
            lblnorec.Visible = false;
        }
        catch
        {
        }
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpExternal.Visible = false;
            lastdiv.Visible = false;
            lblnorec.Visible = false;
        }
        catch
        {
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        try
        {
            if ((ddlBranch.SelectedIndex != 0) || (ddlBranch.SelectedIndex > 0) || (ddlBranch.SelectedIndex == 0))
            {
                bindsem();
                bindsec();
                bindexammonth();
                bindexamyear();
            }
            FpExternal.Visible = false;
            lastdiv.Visible = false;
            lblnorec.Visible = false;
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((ddlDegree.SelectedIndex != 0) && (ddlBranch.SelectedIndex != 0))
        {
            ddlSemYr.Items.Clear();
            Get_Semester();
        }
        bindexammonth();
        bindexamyear();
        ddlSec.SelectedIndex = -1;
        FpExternal.Visible = false;
        lastdiv.Visible = false;
        lblnorec.Visible = false;
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlBranch.Items.Clear();
        string course_id = ddlDegree.SelectedValue.ToString();
        collegecode = Session["collegecode"].ToString();
        usercode = Session["UserCode"].ToString();
        DataSet ds = ClsAttendanceAccess.GetBranchDetail(course_id.ToString(), collegecode.ToString());
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlBranch.DataSource = ds;
            ddlBranch.DataTextField = "Dept_Name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();
        }
        if ((ddlBranch.SelectedIndex != 0) || (ddlBranch.SelectedIndex > 0) || (ddlBranch.SelectedIndex == 0))
        {
            bindsem();
            bindsec();
            bindexammonth();
            bindexamyear();
        }
        FpExternal.Visible = false;
        lastdiv.Visible = false;
        lblnorec.Visible = false;
    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }
        bindsec();
        bindexammonth();
        bindexamyear();
        FpExternal.Visible = false;
        lastdiv.Visible = false;
        lblnorec.Visible = false;
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void rbbeforeandafterrevaluation_selectedindexchanged(object sender, EventArgs e)
    {
        if (rbbeforeandafterrevaluation.SelectedValue == "1")
        {
            chkonlyrevaluation.Checked = false;
            chkonlyrevaluation.Enabled = false;
            //txtfrm_date.Enabled = true;
            //txtend_date.Enabled = true;
        }
        else
        {
            chkonlyrevaluation.Enabled = true;
            //txtfrm_date.Enabled = false;
            //txtend_date.Enabled = false;
        }
        FpExternal.Visible = false;
        lastdiv.Visible = false;
        lblnorec.Visible = false;
    }

    public void External_Students()
    {
        sections = ddlSec.SelectedValue.ToString();
        if (sections.ToString().ToLower() == "all" || sections.ToString() == "" || sections.ToString() == "-1")
        {
            strsec = string.Empty;
        }
        else
        {
            strsec = " and registration.sections='" + sections.ToString() + "'";
        }
        //'-------------------- select Exam_Code
        exam_month = ddlMonth.SelectedValue.ToString();
        //exam_year = ddlYear.SelectedValue.ToString();
        exam_year = ddlYear.SelectedItem.ToString();
        semdec = GetSemester_AsNumber(Convert.ToInt32(current_sem));
        ExamCode = Get_UnivExamCode(Convert.ToInt32(degree_code), GetSemester_AsNumber(Convert.ToInt32(current_sem)), Convert.ToInt32(batch_year), Convert.ToInt32(exam_month), Convert.ToInt32(exam_year));
        //SetHeader
        if ((exam_year != ""))
        {
            IntExamCode = Get_UnivExamCode(Convert.ToInt32(degree_code), Convert.ToInt32(semdec), Convert.ToInt32(batch_year), Convert.ToInt32(exam_month), Convert.ToInt32(exam_year));
            if (IntExamCode > 0)
            {
                if (LoadSubject(IntExamCode) > 0)
                {
                    string grade = string.Empty;
                    grade = "select grade_flag from grademaster where degree_code=" + degree_code + " and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year= " + exam_year + "";
                    newcon1.Close();
                    newcon1.Open();
                    cmd = new SqlCommand(grade, newcon1);
                    SqlDataReader drexgrade;
                    drexgrade = cmd.ExecuteReader();
                    int cs = 0;
                    while (drexgrade.Read())
                    {
                        cs++;
                        if (drexgrade.HasRows == true)
                        {
                            Load_Students(ExamCode);
                        }
                        else
                        {
                        }
                    }
                    if (cs == 0)
                    {
                        lblerrormsg.Text = "No Records Found";
                        lblerrormsg.Visible = true;
                        FpExternal.Visible = false;
                        btnExcel.Visible = false;
                        txtexcelname.Visible = false;
                        lblrptname.Visible = false;
                        btnprintmaster.Visible = false;
                    }
                }
                else
                {
                    lblerrormsg.Text = "No Records Found";
                    lblerrormsg.Visible = true;
                    FpExternal.Visible = false;
                    btnExcel.Visible = false;
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    btnprintmaster.Visible = false;
                }
            }
            else
            {
                lblerrormsg.Text = "No Records Found";
                lblerrormsg.Visible = true;
                FpExternal.Visible = false;
                btnExcel.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                btnprintmaster.Visible = false;
            }
        }
    }

    public void Load_Students(int ExamCode)
    {
        try
        {
            Hashtable habs = new Hashtable();
            int abscnt = 0;
            Hashtable htstaffdetails = new Hashtable();
            string arreargrade = string.Empty;
            double gminintmark = 0;
            double gmaxintmark = 0;
            int allpasscount = 0;
            int allappeared = 0;
            int gcount = 0;
            int gmintotal = 0;
            //   DataSet dstotalnumberofarrears = new DataSet();
            // DataSet dscurrentarrcount = new DataSet();
            // DataView dvtotalnumberofarrears = new DataView();
            Hashtable htfirstclass = new Hashtable();
            Hashtable hthighestmark = new Hashtable();
            DataSet dshighestmark = new DataSet();
            DataView dvhighestmark = new DataView();
            Hashtable htfailsubcount = new Hashtable();
            DataView dvtotarrear = new DataView();
            int firstsubbind = 0;
            Hashtable htoverall = new Hashtable();
            string dum_tage_date = string.Empty;
            string dum_tage_hrs = string.Empty;
            string result = string.Empty;
            Boolean chkflag = false;
            Boolean failflag = false;
            DataView dvmarkimp = new DataView();
            string section = string.Empty;
            string failgrade = string.Empty;
            int failcount = 0;
            int abs1count = 0;
            int nomarkcount = 0;
            int attept = 0, maxmrk = 0;
            string getattmaxmark = daccess.GetFunctionv("select convert(varchar,attempts)+'-'+convert(varchar,maxmark) as amm from coe_attmaxmark where collegecode='" + Session["collegecode"].ToString() + "'");
            string[] semecount = getattmaxmark.Split(new Char[] { '-' });
            if (semecount.GetUpperBound(0) == 1)
            {
                attept = Convert.ToInt32(semecount[0].ToString());
                maxmrk = Convert.ToInt32(semecount[1].ToString());
                flagchknew = true;
            }
            else
            {
                flagchknew = false;
            }
            string orderby_Setting = GetFunction("select value from master_Settings where settings='order_by'");
            if (orderby_Setting == "")
            {
                strorder = string.Empty;
            }
            else
            {
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY registration.Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY Registration.Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "ORDER BY Registration.Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "ORDER BY Registration.Roll_No,Registration.Reg_No,Registration.Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY Registration.Roll_No,Registration.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY Registration.Reg_No,Registration.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY Registration.Roll_No,Registration.Stud_Name";
                }
            }
            FarPoint.Web.Spread.LabelCellType lblcell = new FarPoint.Web.Spread.LabelCellType();
            if (ddlSec.Text != string.Empty)
            {
                section = ddlSec.SelectedItem.Text;
            }
            SqlDataReader dr_grade_val;
            con.Close();
            con.Open();
            cmd = new SqlCommand("select linkvalue from inssettings where linkname='corresponding grade' and college_code=" + Session["collegecode"] + "", con);
            dr_grade_val = cmd.ExecuteReader();
            while (dr_grade_val.Read())
            {
                if (dr_grade_val.HasRows == true)
                {
                    grade_setting = dr_grade_val[0].ToString();
                }
            }
            if (section.ToLower().Trim() == "all")
            {
                strStudents = "Select isnull(applyn.sex,'') as gender,isnull(registration.Roll_No,'') as RlNo,isnull(registration.Reg_No,'') as RgNo ,isnull(registration.Stud_Name,'') as SName,isnull(registration.stud_type,'') as type,roll_admit,registration.mode as mode,convert(varchar(30),adm_date,103) as addate,(select TextVal from TextValtable where TExtCode=isnull(seattype,0)) as [SeatType] from registration,applyn where registration.Degree_Code = " + degree_code + " and registration.Batch_Year = " + batch_year + " " + Session["strvar"] + " and registration.Current_Semester >= " + semdec + " and registration.app_no=applyn.app_no and cc=0 and delflag =0 and exam_flag <>'Debar' and RollNo_Flag=1 and Roll_No is not null and ltrim(rtrim(Roll_No)) <>'' " + strorder + " ";
            }
            else
            {
                strStudents = "Select isnull(applyn.sex,'') as gender,isnull(registration.Roll_No,'') as RlNo,isnull(registration.Reg_No,'') as RgNo ,isnull(registration.Stud_Name,'') as SName,isnull(registration.stud_type,'') as type,roll_admit,registration.mode as mode,convert(varchar(30),adm_date,103) as addate,(select TextVal from TextValtable where TExtCode=isnull(seattype,0)) as [SeatType] from registration,applyn where registration.Degree_Code = " + degree_code + " and registration.Batch_Year = " + batch_year + " " + Session["strvar"] + " and registration.Current_Semester >= " + semdec + " and registration.sections='" + section.ToString() + "' and registration.app_no=applyn.app_no and cc=0 and delflag =0 and exam_flag <>'Debar' and RollNo_Flag=1 and Roll_No is not null and ltrim(rtrim(Roll_No)) <>'' " + strorder + " ";
            }
            con.Close();
            con.Open();
            SqlCommand cmd_Subject = new SqlCommand(strStudents, con);
            SqlDataReader dr_Students;
            dr_Students = cmd_Subject.ExecuteReader();
            int sno = 0;
            while (dr_Students.Read())
            {
                sno++;
                FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 1;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Tag = dr_Students["mode"].ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Text = dr_Students["RlNo"].ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Tag = dr_Students["addate"].ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].CellType = lblcell;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Text = dr_Students["RgNo"].ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].CellType = lblcell;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = dr_Students["SName"].ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                string gen = string.Empty;
                if (dr_Students["gender"].ToString() == "1")
                {
                    gen = "F";
                }
                else if (dr_Students["gender"].ToString() == "0")
                {
                    gen = "M";
                }
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = gen.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = dr_Students["type"].ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Text = dr_Students["SeatType"].ToString();//rrr
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Font.Bold = true;
            }
            if (firstsubbind == 0)
            {
                int cn = 0;
                for (int col = 7; col <= FpExternal.Sheets[0].ColumnCount - 7; col++)//rrrr
                {
                    cn++;
                    htfailsubcount.Add(cn, "");
                }
            }
            string attempt = "";
            if (chk_subjectwisegrade.Checked)
                attempt = " and Attempts =1 ";
            string marksql = "Select mark_entry.*,mark_entry.subject_no as subn,maxtotal,Subject_type,subject.min_ext_marks,subject.min_int_marks,subject.mintotal from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = " + ExamCode + " " + attempt + "  order by subject_type desc,mark_entry.subject_no";
            DataSet dscheckresult = daccess.select_method(marksql, hat, "Text");
            // string markarrear = "select * from mark_entry";
            //DataSet dscoun = daccess.select_method(markarrear, hat, "Text");
            string grademaster = "select * from grade_master where degree_code=" + degree_code + " and batch_year='" + batch_year + "' ";
            DataSet dsgrademaster = daccess.select_method(grademaster, hat, "Text");
            DataSet dscheckgrademaster = daccess.select_method(grademaster, hat, "Text");
            DataView dvcheckgrademaster = new DataView();
            dshighestmark = daccess.select_method(grademaster, hat, "Text");
            DataView dvgrademaster = new DataView();
            string overallarrear = string.Empty;
            //  overallarrear = "select distinct m.subject_no, m.roll_no as rollno,sm.semester from mark_entry m,syllabus_master sm,subject s where sm.syll_code=s.syll_code  and sm.semester >=1 and sm.semester <='" + ddlSemYr.SelectedItem.Text + "' and s.subject_no =m.subject_no and m.result='fail' and m.passorfail=0";
            //overallarrear = "select distinct m.subject_no, m.roll_no as rollno,sm.semester ,internal_mark,external_mark,actual_internal_mark,actual_external_mark,grade,actual_grade from mark_entry m,syllabus_master sm,subject s where sm.syll_code=s.syll_code  and sm.semester >=1 and sm.semester <='" + ddlSemYr.SelectedItem.Text + "' and s.subject_no =m.subject_no  and m.exam_code='" + ExamCode + "' ";
            //dstotalnumberofarrears = daccess.select_method(overallarrear, hat, "Text");
            //dscurrentarrcount = dstotalnumberofarrears;
            string strgetarrecount = "select distinct r.roll_no,sc.semester,sc.subject_no from Registration r,subjectChooser sc,subject s,sub_sem ss where r.Roll_No=sc.Roll_No and sc.subject_no=s.subject_no and s.subType_no=ss.subType_no and ss.promote_count=1 and r.degree_code='" + degree_code + "' and r.Batch_Year='" + batch_year + "' and sc.semester<='" + ddlSemYr.SelectedItem.Text + "' and sc.subject_no not in(select m.subject_no from mark_entry m,exam_details ed where m.exam_code=ed.exam_code and ed.degree_code='" + degree_code + "' and ed.Batch_Year='" + batch_year + "'  and ed.current_semester<='" + ddlSemYr.SelectedItem.Text + "' and m.roll_no=r.Roll_No and m.subject_no=sc.subject_no and m.result='Pass') ";
            DataSet dsallarecount = daccess.select_method_wo_parameter(strgetarrecount, "Text");
            string grade = "select grade_flag from grademaster where degree_code=" + degree_code + " and batch_year='" + batch_year + "' and exam_month=" + exam_month + " and exam_year= " + exam_year + "";
            newcon.Close();
            newcon.Open();
            SqlDataReader drgrade;
            SqlCommand cmd_grade = new SqlCommand(grade, newcon);
            drgrade = cmd_grade.ExecuteReader();
            while (drgrade.Read())
            {
                getgradeflag = drgrade["grade_flag"].ToString();
                stgetgradeflag = getgradeflag;
                string gpa = string.Empty;
                int semarrearcount = 0;
                for (int rowcou = 0; rowcou <= FpExternal.Sheets[0].RowCount - 1; rowcou++)
                {
                    abscnt = 0;
                    semarrearcount = 0;
                    string rollnum = FpExternal.Sheets[0].Cells[rowcou, 1].Text;
                    studadmdate = FpExternal.Sheets[0].Cells[rowcou, 1].Tag.ToString();
                    attendancerollnum = rollnum;
                    string mod = FpExternal.Sheets[0].Cells[rowcou, 0].Tag.ToString();
                    //gpa = daccess.Calulat_GPA_Semwise(rollnum.ToString(), degree_code, batch_year, exam_month, exam_year, Session["collegecode"].ToString());
                    gpa = Calulat_GPA_Semwise(rollnum.ToString(), degree_code, batch_year, exam_month, exam_year, Session["collegecode"].ToString());  //modified by Mullai
                    FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 1].Text = gpa;
                    FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    string getgpa = Calulat_GPA_Semwise(rollnum.ToString(), degree_code, batch_year, exam_month, exam_year, Session["collegecode"].ToString());
                    FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 2].Text = gsum;
                    FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                    DataSet ds = new DataSet();
                    hat.Clear();
                    hat.Add("degree_code", degree_code.ToString());
                    hat.Add("sem_ester", int.Parse(ddlSemYr.SelectedItem.ToString()));
                    ds = daccess.select_method("period_attnd_schedule", hat, "sp");
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
                    ds5 = daccess.select_method("ATT_MASTER_SETTING", hat, "sp");
                    countds = ds5.Tables[0].Rows.Count;
                    //Added By Srinath 25/2/2013 ===Start
                    string[] fromdatespit = txtfrm_date.Text.Split('/');
                    string[] todatespit = txtend_date.Text.Split('/');
                    DateTime spfromdate = Convert.ToDateTime(fromdatespit[1] + '/' + fromdatespit[0] + '/' + fromdatespit[2]);
                    DateTime sptodate = Convert.ToDateTime(todatespit[1] + '/' + todatespit[0] + '/' + todatespit[2]);
                    ht_sphr.Clear();
                    string hrdetno = string.Empty;
                    string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and date between '" + spfromdate.ToString() + "' and '" + sptodate.ToString() + "'";
                    ds_sphr = daccess.select_method(getsphr, hat, "Text");
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
                    chkflag = false;
                    failcount = 0;
                    abs1count = 0;
                    nomarkcount = 0;
                    //atten
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
                    if (Session["Hourwise"] == "1")
                    {
                        FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 4].Text = dum_tage_hrs.ToString();
                        FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                    }
                    else
                    {
                        FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 4].Text = dum_tage_date.ToString();
                        FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                    }
                    //att
                    int vb = 0;
                    for (int col = 7; col <= FpExternal.Sheets[0].ColumnCount - 7; col++)//rrrr
                    {
                        string subnum = FpExternal.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();
                        getsubno = subnum;
                        string scode = FpExternal.Sheets[0].ColumnHeader.Cells[1, col].Text.ToString();
                        //OVERALL
                        if (firstsubbind == 0)
                        {
                            if (htfirstclass.Contains(scode) == false)
                            {
                                htfirstclass.Add(scode, "");
                                hthighestmark.Add(scode, "");
                            }
                        }
                        //OVERALL
                        dscheckresult.Tables[0].DefaultView.RowFilter = "roll_no='" + rollnum + "' and subn='" + subnum + "'";
                        dvmarkimp = dscheckresult.Tables[0].DefaultView;
                        double mintotal = Convert.ToDouble(FpExternal.Sheets[0].ColumnHeader.Cells[1, col].Tag.ToString());
                        gmintotal = Convert.ToInt32(mintotal);
                        double maxtotal = Convert.ToDouble(FpExternal.Sheets[0].ColumnHeader.Cells[2, col].Tag.ToString());
                        if (dvmarkimp.Count > 0)
                        {
                            vb++;
                            result = dvmarkimp[0]["result"].ToString();
                            if (rbbeforeandafterrevaluation.SelectedValue == "1")
                            {
                                #region before revaluation
                                #region Mark
                                //mark
                                double total = 0;
                                if (Convert.ToInt32(getgradeflag) == 3)
                                {
                                    gchm = "1";
                                    double internalmark = 0;
                                    double externalmark = 0;
                                    string stringintternal = string.Empty;
                                    string stringexternal = string.Empty;
                                    if (Convert.ToString(dvmarkimp[0]["actual_external_mark"]) != "")
                                    {
                                        stringexternal = dvmarkimp[0]["actual_external_mark"].ToString();
                                        externalmark = Convert.ToDouble(stringexternal);
                                    }
                                    if (Convert.ToString(dvmarkimp[0]["actual_internal_mark"]) != "")
                                    {
                                        stringintternal = dvmarkimp[0]["actual_internal_mark"].ToString();
                                        internalmark = Convert.ToDouble(stringintternal);
                                    }
                                    total = externalmark + internalmark;
                                    if ((total == 0) && (mod == "3"))
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = string.Empty;
                                        nomarkcount++;
                                    }
                                    else if ((result == "AAA") || (result == "-1"))
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "AAA";
                                        FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                        failcount++;
                                        abs1count++;
                                    }
                                    else if (result == "WHD")//Added by srinath 26/6/2014
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "WHD";
                                        FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                        failcount++;
                                    }
                                    else
                                    {
                                        if (grade_setting == "0")//if 0 means display only marks(in settings mark conversion unchecked)
                                        {
                                            if (total != 0)
                                            {
                                                if (Convert.ToDouble(dvmarkimp[0]["actual_internal_mark"].ToString()) >= Convert.ToDouble(dvmarkimp[0]["min_int_marks"].ToString()) && Convert.ToDouble(dvmarkimp[0]["actual_external_mark"].ToString()) >= Convert.ToDouble(dvmarkimp[0]["min_ext_marks"].ToString()))
                                                {
                                                    result = "Pass";
                                                }
                                                else
                                                {
                                                    result = "Fail";
                                                    failflag = true;
                                                    failcount++;
                                                    FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                                    FpExternal.Sheets[0].Cells[rowcou, col].BackColor = Color.LightGray;
                                                }
                                                FpExternal.Sheets[0].Cells[rowcou, col].Text = total.ToString();
                                                FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                            else
                                            {
                                                FpExternal.Sheets[0].Cells[rowcou, col].Text = string.Empty;
                                                //FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                        }
                                        else//grade_setting 1 means display corresponding grade for mark(in setting mark conversion checked)
                                        {
                                            //change new............
                                            if (flagchknew == true)
                                            {
                                                double inte = 0, exte = 0, realattpt = 0;
                                                if ((dvmarkimp[0]["actual_internal_mark"].ToString() != string.Empty) && (dvmarkimp[0]["actual_External_mark"].ToString() != string.Empty) && (dvmarkimp[0]["min_int_marks"].ToString() != string.Empty) && (dvmarkimp[0]["min_ext_marks"].ToString() != string.Empty) && (dvmarkimp[0]["mintotal"].ToString() != string.Empty))
                                                {
                                                    inte = Convert.ToDouble(dvmarkimp[0]["actual_internal_mark"].ToString());
                                                    exte = Convert.ToDouble(dvmarkimp[0]["actual_external_mark"].ToString());
                                                    realattpt = Convert.ToInt32(dvmarkimp[0]["attempts"].ToString());
                                                    if (attept > realattpt)
                                                    {
                                                        if (Convert.ToDouble(dvmarkimp[0]["actual_internal_mark"].ToString()) >= Convert.ToDouble(dvmarkimp[0]["min_int_marks"].ToString()) && Convert.ToDouble(dvmarkimp[0]["actual_External_mark"].ToString()) >= Convert.ToDouble(dvmarkimp[0]["min_ext_marks"].ToString()) && ((inte + exte) >= Convert.ToDouble((dvmarkimp[0]["mintotal"].ToString()))))
                                                        {
                                                            convertgradev(rollnum, getsubno, maxmrk, attept);
                                                            result = "Pass";
                                                            FpExternal.Sheets[0].Cells[rowcou, col].Text = funcgrade.ToString();
                                                            FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                        else
                                                        {
                                                            con.Close();
                                                            con.Open();
                                                            SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                                            SqlDataReader dr_failgrade;//= new SqlDataReader(cmd_failgrade);
                                                            dr_failgrade = cmd_failgrade.ExecuteReader();
                                                            if (dr_failgrade.HasRows == true)
                                                            {
                                                                if (dr_failgrade.Read())
                                                                {
                                                                    if (dr_failgrade["value"].ToString() != "")
                                                                    {
                                                                        failgrade = dr_failgrade["value"].ToString();
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                failgrade = "-";
                                                            }
                                                            if (chk_subjectwisegrade.Checked)
                                                            {
                                                                failgrade = "RA";
                                                            }
                                                            result = "Fail";
                                                            failflag = true;
                                                            FpExternal.Sheets[0].Cells[rowcou, col].Text = failgrade.ToString();
                                                            FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                                            FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                                            FpExternal.Sheets[0].Cells[rowcou, col].BackColor = Color.LightGray;
                                                            failcount++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (maxmrk <= exte)
                                                        {
                                                            convertgradev(rollnum, getsubno, maxmrk, attept);
                                                            result = "Pass";
                                                            FpExternal.Sheets[0].Cells[rowcou, col].Text = funcgrade.ToString();
                                                            FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                        else
                                                        {
                                                            con.Close();
                                                            con.Open();
                                                            SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                                            SqlDataReader dr_failgrade;//= new SqlDataReader(cmd_failgrade);
                                                            dr_failgrade = cmd_failgrade.ExecuteReader();
                                                            if (dr_failgrade.HasRows == true)
                                                            {
                                                                if (dr_failgrade.Read())
                                                                {
                                                                    if (dr_failgrade["value"].ToString() != "")
                                                                    {
                                                                        failgrade = dr_failgrade["value"].ToString();
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                failgrade = "-";
                                                            }
                                                            if (chk_subjectwisegrade.Checked)
                                                            {
                                                                failgrade = "RA";
                                                            }
                                                            result = "Fail";
                                                            failflag = true;
                                                            FpExternal.Sheets[0].Cells[rowcou, col].Text = failgrade.ToString();
                                                            FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                                            FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                                            FpExternal.Sheets[0].Cells[rowcou, col].BackColor = Color.LightGray;
                                                            failcount++;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                double inte = 0, exte = 0, realattpt = 0;

                                                double act_internal = 0.0;
                                                double min_internal = 0.0;
                                                double act_external = 0.0;
                                                double min_external = 0.0;
                                                double.TryParse(dvmarkimp[0]["actual_internal_mark"].ToString(), out act_internal);
                                                double.TryParse(dvmarkimp[0]["min_int_marks"].ToString(), out min_internal);
                                                double.TryParse(dvmarkimp[0]["actual_External_mark"].ToString(), out act_external);
                                                double.TryParse(dvmarkimp[0]["min_ext_marks"].ToString(), out min_external);
                                               // if (Convert.ToDouble(dvmarkimp[0]["actual_internal_mark"].ToString()) >= Convert.ToDouble(dvmarkimp[0]["min_int_marks"].ToString()) && Convert.ToDouble(dvmarkimp[0]["actual_External_mark"].ToString()) >= Convert.ToDouble(dvmarkimp[0]["min_ext_marks"].ToString()))
                                                if (act_internal >= min_internal && act_external >= min_external)
                                                {
                                                    inte = Convert.ToDouble(dvmarkimp[0]["actual_internal_mark"].ToString());
                                                    exte = Convert.ToDouble(dvmarkimp[0]["actual_external_mark"].ToString());
                                                    realattpt = Convert.ToInt32(dvmarkimp[0]["attempts"].ToString());
                                                    convertgrade(rollnum, getsubno);
                                                    result = "Pass";
                                                    FpExternal.Sheets[0].Cells[rowcou, col].Text = funcgrade.ToString();
                                                    FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                                else
                                                {
                                                    con.Close();
                                                    con.Open();
                                                    SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                                    SqlDataReader dr_failgrade;//= new SqlDataReader(cmd_failgrade);
                                                    dr_failgrade = cmd_failgrade.ExecuteReader();
                                                    if (dr_failgrade.HasRows == true)
                                                    {
                                                        if (dr_failgrade.Read())
                                                        {
                                                            if (dr_failgrade["value"].ToString() != "")
                                                            {
                                                                failgrade = dr_failgrade["value"].ToString();
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        failgrade = "-";
                                                    }
                                                    if (chk_subjectwisegrade.Checked)
                                                    {
                                                        failgrade = "RA";
                                                    }
                                                    result = "Fail";
                                                    failflag = true;
                                                    FpExternal.Sheets[0].Cells[rowcou, col].Text = failgrade.ToString();
                                                    FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                                    FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                                    FpExternal.Sheets[0].Cells[rowcou, col].BackColor = Color.LightGray;
                                                    failcount++;
                                                }
                                            }
                                            foreach (DataRow dr in dgrades.Rows)
                                            {
                                                if (result == "Pass")
                                                {
                                                    if (Convert.ToString(dr["subjectno"]) == subnum && Convert.ToString(dr["grade"]) == funcgrade)
                                                    {
                                                        int a = Convert.ToInt32(dr["count"].ToString());
                                                        dr["count"] = a + 1;
                                                    }
                                                }
                                                else if (result == "Fail")
                                                {
                                                    if (Convert.ToString(dr["subjectno"]) == subnum && Convert.ToString(dr["grade"]) == failgrade.ToUpper())
                                                    {
                                                        int a = Convert.ToInt32(dr["count"].ToString());
                                                        dr["count"] = a + 1;
                                                    }
                                                }
                                            }
                                            //
                                            int gfsdfgsdfg = dgrades.Columns.Count;
                                        }
                                        double va = total;
                                        if (result == "Pass")
                                        {
                                            if (va > 60)
                                            {
                                                if (htfirstclass.Contains(scode))
                                                {
                                                    int cnt = 0;
                                                    if (htfirstclass[scode] == "")
                                                    {
                                                        cnt = 1;
                                                    }
                                                    else
                                                    {
                                                        cnt = Convert.ToInt16(htfirstclass[scode]);
                                                        cnt = cnt + 1;
                                                    }
                                                    htfirstclass[scode] = cnt;
                                                }
                                            }
                                            if (hthighestmark.Contains(scode))
                                            {
                                                string value = Convert.ToString(hthighestmark[scode]);
                                                if (value == "")
                                                {
                                                    hthighestmark[scode] = total.ToString();
                                                }
                                                else
                                                {
                                                    double previousmark = Convert.ToDouble(hthighestmark[scode]);
                                                    if (va > previousmark)
                                                    {
                                                        hthighestmark[scode] = total.ToString();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (chkflag == false)
                                    {
                                        if (result == "Pass")
                                        {
                                            FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 5].Text = result.ToString();
                                        }
                                        //*****************************Modified By Subburaj 15.08.2014******//
                                        else
                                        {
                                            if ((total == 0.0) && (mod == "3"))
                                            {
                                                //  FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, "LE");
                                                FpExternal.Sheets[0].Cells[rowcou, col].Text = " ";
                                                //chkflag = true;
                                            }
                                            else if ((result == "AAA") || (result == "-1"))
                                            {
                                                FpExternal.Sheets[0].Cells[rowcou, col].Text = "AAA";
                                                FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                            }
                                            //***********************************end**************************//
                                            else if (result == "")
                                            {
                                                FpExternal.Sheets[0].Cells[rowcou, col].Text = " ";
                                                nomarkcount++;
                                            }
                                            else if (total == 0)
                                            {
                                                FpExternal.Sheets[0].Cells[rowcou, col].Text = " ";
                                            }
                                            else
                                            {
                                                //FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, "Fail");
                                                FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 5].Text = "Fail";
                                                FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 5].ForeColor = Color.Red;
                                                chkflag = true;
                                                failflag = true;
                                            }
                                        }
                                    }
                                }
                                //mark end
                                #endregion
                                #region Grade
                                //grade start
                                if (Convert.ToInt32(getgradeflag) == 2)
                                {
                                
                                    DataView dvcurrentresult = new DataView();
                                    if (col == 6)
                                    {
                                        double minimumtot = mintotal - 1;
                                        arreargrade = GetFunction("select mark_grade from grade_master where '" + minimumtot + "' between frange and trange and degree_code='" + degree_code + "' and batch_year='" + batch_year + "'");
                                        //dstotalnumberofarrears.Tables[0].DefaultView.RowFilter = "rollno='" + rollnum + "' and actual_grade = '" + arreargrade + "' and grade = ''";
                                        arreargrade = "and actual_grade in ('" + arreargrade + "','')";
                                        //dstotalnumberofarrears.Tables[0].DefaultView.RowFilter = "rollno='" + rollnum + "' " + arreargrade + "";
                                        //dvtotalnumberofarrears = dstotalnumberofarrears.Tables[0].DefaultView;
                                        //arreargrade = GetFunction("select mark_grade from grade_master where '" + minimumtot + "' between frange and trange and degree_code='" + degree_code + "' and batch_year='" + batch_year + "'");
                                        //if (dvtotalnumberofarrears.Count > 0)
                                        //{
                                        //    semarrearcount = dvtotalnumberofarrears.Count;
                                        //}
                                        //else
                                        //{
                                        //    semarrearcount = 0;
                                        //}
                                        //dscurrentarrcount.Tables[0].DefaultView.RowFilter = "rollno='" + rollnum + "' and actual_grade = '" + arreargrade + "' and grade <> '' and semester='" + ddlSemYr.SelectedItem.Text + "'";
                                        //dvcurrentresult = dscurrentarrcount.Tables[0].DefaultView;
                                        //if (dvcurrentresult.Count > 0)
                                        //{
                                        //    result = "Fail";
                                        //}
                                    }
                                    if (dvmarkimp[0]["actual_grade"].ToString() != "")
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = dvmarkimp[0]["actual_grade"].ToString();
                                    }
                                    else if (result == "AAA" && dvmarkimp[0]["actual_grade"].ToString() == "")
                                    {
                                        abscnt++;
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "AAA";
                                        FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                    }
                                    else if (result == "UA" && dvmarkimp[0]["actual_grade"].ToString() == "")
                                    {
                                        abscnt++;
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "UA";
                                        FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                    }
                                    else if (result == "WHD" && dvmarkimp[0]["actual_grade"].ToString() == "")
                                    {
                                        abscnt++;
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "WHD";
                                        FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                    }
                                    else if (result == "Fail" && dvmarkimp[0]["actual_grade"].ToString() == "")
                                    {
                                        abscnt++;
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "AAA";
                                        FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                    }
                                    else
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = " ";
                                        FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                    }
                                    //add grade
                                    foreach (DataRow dr in dgrades.Rows)
                                    {
                                        if (Convert.ToString(dr["subjectno"]) == subnum && Convert.ToString(dr["grade"]) == Convert.ToString(dvmarkimp[0]["actual_grade"]))
                                        {
                                            int a = Convert.ToInt32(dr["count"].ToString());
                                            dr["count"] = a + 1;
                                        }
                                    }
                                    //end
                                    if (dvmarkimp[0]["actual_grade"].ToString() == "AAA" || dvmarkimp[0]["actual_grade"].ToString() == "-1")
                                    {
                                        result = "Fail";
                                    }
                                    else if (dvmarkimp[0]["actual_grade"].ToString() == "UA")
                                    {
                                        result = "Fail";
                                    }
                                    else if (dvmarkimp[0]["actual_grade"].ToString() == "U")
                                    {
                                        result = "Fail";
                                    }
                                    else if (dvmarkimp[0]["actual_grade"].ToString() == arreargrade)
                                    {
                                        result = "Fail";
                                    }
                                    else if (dvmarkimp[0]["actual_grade"].ToString() != arreargrade && dvmarkimp[0]["actual_grade"].ToString().Trim() != "")
                                    {
                                        result = "Pass";
                                    }
                                    else
                                    {
                                        result = "Fail";
                                    }
                                    dshighestmark.Tables[0].DefaultView.RowFilter = "mark_grade='" + dvmarkimp[0]["actual_grade"].ToString() + "'";
                                    dvhighestmark = dshighestmark.Tables[0].DefaultView;
                                    if (dvhighestmark.Count > 0)
                                    {
                                        string gradevalue = dvhighestmark[0]["Mark_Grade"].ToString();
                                        double fromrange = Convert.ToDouble(dvhighestmark[0]["Trange"]);
                                        if (hthighestmark.Contains(scode))
                                        {
                                            string value = Convert.ToString(hthighestmark[scode]);
                                            if (value == "")
                                            {
                                                hthighestmark[scode] = gradevalue;
                                            }
                                            else
                                            {
                                                dscheckgrademaster.Tables[0].DefaultView.RowFilter = "Mark_Grade='" + value + "'";
                                                dvcheckgrademaster = dscheckgrademaster.Tables[0].DefaultView;
                                                if (dvcheckgrademaster.Count > 0)
                                                {
                                                    double previousmark = Convert.ToDouble(dvcheckgrademaster[0]["Trange"]);
                                                    if (fromrange > previousmark)
                                                    {
                                                        hthighestmark[scode] = gradevalue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    dsgrademaster.Tables[0].DefaultView.RowFilter = "mark_grade='" + dvmarkimp[0]["actual_grade"].ToString() + "'";
                                    dvgrademaster = dsgrademaster.Tables[0].DefaultView;
                                    double firstclasscnt = 0;
                                    if (dvgrademaster.Count > 0)
                                    {
                                        firstclasscnt = Convert.ToDouble(dvgrademaster[0]["Frange"]);
                                        if (firstclasscnt > 60)
                                        {
                                            if (htfirstclass.Contains(scode))
                                            {
                                                int cnt = 0;
                                                if (htfirstclass[scode] == "")
                                                {
                                                    cnt = 1;
                                                }
                                                else
                                                {
                                                    cnt = Convert.ToInt16(htfirstclass[scode]);
                                                    cnt = cnt + 1;
                                                }
                                                htfirstclass[scode] = cnt;
                                            }
                                        }
                                    }
                                    FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                    //FpExternal.Sheets[0].Cells[rowcou,FpExternal.Sheets[0].ColumnCount-4].Text=result.ToString();
                                    if (result == "Fail")
                                    {
                                        failcount++;
                                        FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                        FpExternal.Sheets[0].Cells[rowcou, col].BackColor = Color.LightGray;
                                    }
                                    if ((dvmarkimp[0]["actual_grade"].ToString() == "") && (mod == "3"))
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "AAA";
                                    }
                                    else if ((dvmarkimp[0]["actual_grade"].ToString() == "") && (result == "AAA") || (result == "-1"))
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "AAA";
                                        failcount++;
                                        abs1count++;
                                    }
                                    else if ((dvmarkimp[0]["actual_grade"].ToString() == "") && (result == "UA"))
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "AAA";
                                        failcount++;
                                        abs1count++;
                                    }
                                    else if ((result == "AAA") || (result == "-1"))
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "AAA";
                                        failcount++;
                                        abs1count++;
                                    }
                                    else if (result == "WHD")//Added by srinath 26/6/2014
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "WHD";
                                        FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                        failcount++;
                                    }
                                    if (chkflag == false)
                                    {
                                        if (result == "Pass")
                                        {
                                            FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 5].Text = result.ToString();
                                        }
                                        else
                                        {
                                            if ((dvmarkimp[0]["actual_grade"].ToString() == "") && (mod == "3"))
                                            {
                                                //  FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, "LE");
                                                FpExternal.Sheets[0].Cells[rowcou, col].Text = "AAA";
                                                //chkflag = true;
                                            }
                                            else if (result == "")
                                            {
                                                FpExternal.Sheets[0].Cells[rowcou, col].Text = " ";
                                                nomarkcount++;
                                            }
                                            else
                                            {
                                                //FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, "Fail");
                                                FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 5].Text = "Fail";
                                                FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 5].ForeColor = Color.Red;
                                                chkflag = true;
                                                failflag = true;
                                            }
                                        }
                                    }
                                }
                                //grade end
                                #endregion
                                #endregion
                            }
                            if (rbbeforeandafterrevaluation.SelectedValue == "2")
                            {
                                #region Mark
                                //mark
                                double total = 0;
                                if (Convert.ToInt32(getgradeflag) == 3)
                                {
                                    gchm = "1";
                                    double internalmark = 0;
                                    double externalmark = 0;
                                    string stringintternal = string.Empty;
                                    string stringexternal = string.Empty;
                                    if (Convert.ToString(dvmarkimp[0]["external_mark"]) != "")
                                    {
                                        stringexternal = dvmarkimp[0]["external_mark"].ToString();
                                        externalmark = Convert.ToDouble(stringexternal);
                                    }
                                    if (Convert.ToString(dvmarkimp[0]["internal_mark"]) != "")
                                    {
                                        stringintternal = dvmarkimp[0]["internal_mark"].ToString();
                                        internalmark = Convert.ToDouble(stringintternal);
                                    }
                                    total = externalmark + internalmark;
                                    if ((total == 0) && (mod == "3"))
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = string.Empty;
                                        nomarkcount++;
                                    }
                                    else if ((result == "AAA") || (result == "-1"))
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "AAA";
                                        FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                        failcount++;
                                        abs1count++;
                                    }
                                    else if (result == "WHD")//Added by srinath 26/6/2014
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "WHD";
                                        FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                        failcount++;
                                    }
                                    else
                                    {
                                        if (grade_setting == "0")//if 0 means display only marks(in settings mark conversion unchecked)
                                        {
                                            if (total != 0)
                                            {
                                                string strintmark = dvmarkimp[0]["internal_mark"].ToString();
                                                string streextmark = dvmarkimp[0]["external_mark"].ToString();
                                                if (strintmark.Trim() != "" && streextmark.Trim() != "")
                                                {
                                                    if (Convert.ToDouble(dvmarkimp[0]["internal_mark"].ToString()) >= Convert.ToDouble(dvmarkimp[0]["min_int_marks"].ToString()) && Convert.ToDouble(dvmarkimp[0]["external_mark"].ToString()) >= Convert.ToDouble(dvmarkimp[0]["min_ext_marks"].ToString()))
                                                    {
                                                        result = "Pass";
                                                    }
                                                    else
                                                    {
                                                        result = "Fail";
                                                        failflag = true;
                                                        failcount++;
                                                        FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                                        FpExternal.Sheets[0].Cells[rowcou, col].BackColor = Color.LightGray;
                                                    }
                                                }
                                                else
                                                {
                                                    result = "Fail";
                                                    failflag = true;
                                                    failcount++;
                                                    FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                                    FpExternal.Sheets[0].Cells[rowcou, col].BackColor = Color.LightGray;
                                                }
                                                FpExternal.Sheets[0].Cells[rowcou, col].Text = total.ToString();
                                                FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                            else
                                            {
                                                FpExternal.Sheets[0].Cells[rowcou, col].Text = string.Empty;
                                                //FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                        }
                                        else//grade_setting 1 means display corresponding grade for mark(in setting mark conversion checked)
                                        {
                                            //new 
                                            if (flagchknew == true)
                                            {
                                                double inte = 0, exte = 0, realattpt = 0;
                                                if ((dvmarkimp[0]["internal_mark"].ToString() != string.Empty) && (dvmarkimp[0]["External_mark"].ToString() != string.Empty) && (dvmarkimp[0]["min_int_marks"].ToString() != string.Empty) && (dvmarkimp[0]["min_ext_marks"].ToString() != string.Empty) && (dvmarkimp[0]["mintotal"].ToString() != string.Empty))
                                                {
                                                    inte = Convert.ToDouble(dvmarkimp[0]["internal_mark"].ToString());
                                                    exte = Convert.ToDouble(dvmarkimp[0]["external_mark"].ToString());
                                                    realattpt = Convert.ToInt32(dvmarkimp[0]["attempts"].ToString());
                                                    if (attept > realattpt)
                                                    {
                                                        if (Convert.ToDouble(dvmarkimp[0]["actual_internal_mark"].ToString()) >= Convert.ToDouble(dvmarkimp[0]["min_int_marks"].ToString()) && Convert.ToDouble(dvmarkimp[0]["External_mark"].ToString()) >= Convert.ToDouble(dvmarkimp[0]["min_ext_marks"].ToString()) && ((inte + exte) >= Convert.ToDouble((dvmarkimp[0]["mintotal"].ToString()))))
                                                        {
                                                            convertgradev(rollnum, getsubno, maxmrk, attept);
                                                            result = "Pass";
                                                            FpExternal.Sheets[0].Cells[rowcou, col].Text = funcgrade.ToString();
                                                            FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                        else
                                                        {
                                                            con.Close();
                                                            con.Open();
                                                            SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                                            SqlDataReader dr_failgrade;//= new SqlDataReader(cmd_failgrade);
                                                            dr_failgrade = cmd_failgrade.ExecuteReader();
                                                            if (dr_failgrade.HasRows == true)
                                                            {
                                                                if (dr_failgrade.Read())
                                                                {
                                                                    if (dr_failgrade["value"].ToString() != "")
                                                                    {
                                                                        failgrade = dr_failgrade["value"].ToString();
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                failgrade = "-";
                                                            }
                                                            if (chk_subjectwisegrade.Checked)
                                                            {
                                                                failgrade = "RA";
                                                            }
                                                            result = "Fail";
                                                            failflag = true;
                                                            FpExternal.Sheets[0].Cells[rowcou, col].Text = failgrade.ToString();
                                                            FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                                            FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                                            FpExternal.Sheets[0].Cells[rowcou, col].BackColor = Color.LightGray;
                                                            failcount++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (maxmrk <= exte)
                                                        {
                                                            convertgradev(rollnum, getsubno, maxmrk, attept);
                                                            result = "Pass";
                                                            FpExternal.Sheets[0].Cells[rowcou, col].Text = funcgrade.ToString();
                                                            FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                        else
                                                        {
                                                            con.Close();
                                                            con.Open();
                                                            SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                                            SqlDataReader dr_failgrade;//= new SqlDataReader(cmd_failgrade);
                                                            dr_failgrade = cmd_failgrade.ExecuteReader();
                                                            if (dr_failgrade.HasRows == true)
                                                            {
                                                                if (dr_failgrade.Read())
                                                                {
                                                                    if (dr_failgrade["value"].ToString() != "")
                                                                    {
                                                                        failgrade = dr_failgrade["value"].ToString();
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                failgrade = "-";
                                                            }
                                                            if (chk_subjectwisegrade.Checked)
                                                            {
                                                                failgrade = "RA";
                                                            }
                                                            result = "Fail";
                                                            failflag = true;
                                                            FpExternal.Sheets[0].Cells[rowcou, col].Text = failgrade.ToString();
                                                            FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                                            FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                                            FpExternal.Sheets[0].Cells[rowcou, col].BackColor = Color.LightGray;
                                                            failcount++;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                double inte = 0, exte = 0, realattpt = 0;
                                                if (Convert.ToDouble(dvmarkimp[0]["internal_mark"].ToString()) >= Convert.ToDouble(dvmarkimp[0]["min_int_marks"].ToString()) && Convert.ToDouble(dvmarkimp[0]["External_mark"].ToString()) >= Convert.ToDouble(dvmarkimp[0]["min_ext_marks"].ToString()))
                                                {
                                                    inte = Convert.ToDouble(dvmarkimp[0]["internal_mark"].ToString());
                                                    exte = Convert.ToDouble(dvmarkimp[0]["external_mark"].ToString());
                                                    realattpt = Convert.ToInt32(dvmarkimp[0]["attempts"].ToString());
                                                    convertgrade(rollnum, getsubno);
                                                    result = "Pass";
                                                    FpExternal.Sheets[0].Cells[rowcou, col].Text = funcgrade.ToString();
                                                    FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                                else
                                                {
                                                    con.Close();
                                                    con.Open();
                                                    SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                                    SqlDataReader dr_failgrade;//= new SqlDataReader(cmd_failgrade);
                                                    dr_failgrade = cmd_failgrade.ExecuteReader();
                                                    if (dr_failgrade.HasRows == true)
                                                    {
                                                        if (dr_failgrade.Read())
                                                        {
                                                            if (dr_failgrade["value"].ToString() != "")
                                                            {
                                                                failgrade = dr_failgrade["value"].ToString();
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        failgrade = "-";
                                                    }
                                                    if (chk_subjectwisegrade.Checked)
                                                    {
                                                        failgrade = "RA";
                                                    }
                                                    result = "Fail";
                                                    failflag = true;
                                                    FpExternal.Sheets[0].Cells[rowcou, col].Text = failgrade.ToString();
                                                    FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                                    FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                                    FpExternal.Sheets[0].Cells[rowcou, col].BackColor = Color.LightGray;
                                                    failcount++;
                                                }
                                            }
                                            foreach (DataRow dr in dgrades.Rows)
                                            {
                                                if (result == "Pass")
                                                {
                                                    if (Convert.ToString(dr["subjectno"]) == subnum && Convert.ToString(dr["grade"]) == funcgrade)
                                                    {
                                                        int a = Convert.ToInt32(dr["count"].ToString());
                                                        dr["count"] = a + 1;
                                                    }
                                                }
                                                else if (result == "Fail")
                                                {
                                                    if (Convert.ToString(dr["subjectno"]) == subnum && Convert.ToString(dr["grade"]) == failgrade.ToUpper())
                                                    {
                                                        int a = Convert.ToInt32(dr["count"].ToString());
                                                        dr["count"] = a + 1;
                                                    }
                                                }
                                            }
                                            ///
                                        }
                                        double va = total;
                                        if (result == "Pass")
                                        {
                                            if (va > 60)
                                            {
                                                if (htfirstclass.Contains(scode))
                                                {
                                                    int cnt = 0;
                                                    if (htfirstclass[scode] == "")
                                                    {
                                                        cnt = 1;
                                                    }
                                                    else
                                                    {
                                                        cnt = Convert.ToInt16(htfirstclass[scode]);
                                                        cnt = cnt + 1;
                                                    }
                                                    htfirstclass[scode] = cnt;
                                                }
                                            }
                                            if (hthighestmark.Contains(scode))
                                            {
                                                string value = Convert.ToString(hthighestmark[scode]);
                                                if (value == "")
                                                {
                                                    hthighestmark[scode] = total.ToString();
                                                }
                                                else
                                                {
                                                    double previousmark = Convert.ToDouble(hthighestmark[scode]);
                                                    if (va > previousmark)
                                                    {
                                                        hthighestmark[scode] = total.ToString();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (chkflag == false)
                                    {
                                        if (result == "Pass")
                                        {
                                            FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 5].Text = result.ToString();
                                        }
                                        else
                                        {
                                            if ((total == 0.0) && (mod == "3"))
                                            {
                                                //  FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, "LE");
                                                FpExternal.Sheets[0].Cells[rowcou, col].Text = " ";
                                                //chkflag = true;
                                            }
                                            else if (result == "")
                                            {
                                                FpExternal.Sheets[0].Cells[rowcou, col].Text = " ";
                                                nomarkcount++;
                                            }
                                            else if (total == 0)
                                            {
                                                FpExternal.Sheets[0].Cells[rowcou, col].Text = " ";
                                            }
                                            else
                                            {
                                                //FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, "Fail");
                                                FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 5].Text = "Fail";
                                                FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 5].ForeColor = Color.Red;
                                                chkflag = true;
                                                failflag = true;
                                            }
                                        }
                                    }
                                }
                                //end mark
                                #endregion
                                #region Grade
                                if (Convert.ToInt32(getgradeflag) == 2)
                                {
                                    string arregdarse = string.Empty;
                                    DataView dvcurrentresult = new DataView();
                                    if (col == 6)
                                    {
                                        double minimumtot = mintotal - 1;
                                        //arreargrade = GetFunction("select mark_grade from grade_master where '" + minimumtot + "' between frange and trange and degree_code='" + degree_code + "' and batch_year='" + batch_year + "'");
                                        //dstotalnumberofarrears.Tables[0].DefaultView.RowFilter = "rollno='" + rollnum + "' and grade in ('" + arreargrade + "') ";
                                        arreargrade = GetFunction("select mark_grade from grade_master where '" + minimumtot + "' between frange and trange and degree_code='" + degree_code + "' and batch_year='" + batch_year + "'");
                                        arregdarse = arreargrade;
                                        arreargrade = "and grade in ('" + arreargrade + "','')";
                                        //dstotalnumberofarrears.Tables[0].DefaultView.RowFilter = "rollno='" + rollnum + "' " + arreargrade + "";
                                        //dvtotalnumberofarrears = dstotalnumberofarrears.Tables[0].DefaultView;
                                        //arreargrade = GetFunction("select mark_grade from grade_master where '" + minimumtot + "' between frange and trange and degree_code='" + degree_code + "' and batch_year='" + batch_year + "'");
                                        //if (dvtotalnumberofarrears.Count > 0)
                                        //{
                                        //    semarrearcount = dvtotalnumberofarrears.Count;
                                        //}
                                        //else
                                        //{
                                        //    semarrearcount = 0;
                                        //}
                                        //dscurrentarrcount.Tables[0].DefaultView.RowFilter = "rollno='" + rollnum + "' and grade = '" + arreargrade + "' and grade <> '' and semester='" + ddlSemYr.SelectedItem.Text + "'";
                                        //dvcurrentresult = dscurrentarrcount.Tables[0].DefaultView;
                                        //if (dvcurrentresult.Count > 0)
                                        //{
                                        //    result = "Fail";
                                        //}
                                    }
                                    if (dvmarkimp[0]["grade"].ToString().Trim() == "" && result == "UA")
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "UA";
                                    }
                                    else if (dvmarkimp[0]["grade"].ToString().Trim() == "" && result == "WHD")
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "WHD";
                                    }
                                    else if (dvmarkimp[0]["grade"].ToString().Trim() == "")
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "AAA";
                                    }
                                    else
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = dvmarkimp[0]["grade"].ToString();
                                    }
                                    //add grade
                                    foreach (DataRow dr in dgrades.Rows)
                                    {
                                        if (Convert.ToString(dr["subjectno"]) == subnum && Convert.ToString(dr["grade"]) == Convert.ToString(dvmarkimp[0]["grade"]))
                                        {
                                            int a = Convert.ToInt32(dr["count"].ToString());
                                            dr["count"] = a + 1;
                                        }
                                    }
                                    //end
                                    if (dvmarkimp[0]["grade"].ToString().Trim() == "WHD")
                                    {
                                        result = "Fail";
                                    }
                                    else if (dvmarkimp[0]["grade"].ToString().Trim() == "")
                                    {
                                        result = "Fail";
                                    }
                                    else if (dvmarkimp[0]["grade"].ToString().Trim().ToLower() == arregdarse.Trim().ToLower())
                                    {
                                        result = "Fail";
                                    }
                                    else
                                    {
                                        result = "Pass";
                                    }
                                    //else if (dvmarkimp[0]["actual_grade"].ToString() != arreargrade && dvmarkimp[0]["actual_grade"].ToString().Trim() != "")
                                    //{
                                    //    result = "Pass";
                                    //}
                                    //else
                                    //{
                                    //    result = "Fail";
                                    //}
                                    result = dvmarkimp[0]["result"].ToString();
                                    dshighestmark.Tables[0].DefaultView.RowFilter = "mark_grade='" + dvmarkimp[0]["grade"].ToString() + "'";
                                    dvhighestmark = dshighestmark.Tables[0].DefaultView;
                                    if (dvhighestmark.Count > 0)
                                    {
                                        string gradevalue = dvhighestmark[0]["Mark_Grade"].ToString();
                                        double fromrange = Convert.ToDouble(dvhighestmark[0]["Trange"]);
                                        if (hthighestmark.Contains(scode))
                                        {
                                            string value = Convert.ToString(hthighestmark[scode]);
                                            if (value == "")
                                            {
                                                hthighestmark[scode] = gradevalue;
                                            }
                                            else
                                            {
                                                dscheckgrademaster.Tables[0].DefaultView.RowFilter = "Mark_Grade='" + value + "'";
                                                dvcheckgrademaster = dscheckgrademaster.Tables[0].DefaultView;
                                                if (dvcheckgrademaster.Count > 0)
                                                {
                                                    double previousmark = Convert.ToDouble(dvcheckgrademaster[0]["Trange"]);
                                                    if (fromrange > previousmark)
                                                    {
                                                        hthighestmark[scode] = gradevalue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    dsgrademaster.Tables[0].DefaultView.RowFilter = "mark_grade='" + dvmarkimp[0]["grade"].ToString() + "'";
                                    dvgrademaster = dsgrademaster.Tables[0].DefaultView;
                                    double firstclasscnt = 0;
                                    if (dvgrademaster.Count > 0)
                                    {
                                        firstclasscnt = Convert.ToDouble(dvgrademaster[0]["Frange"]);
                                        if (firstclasscnt > 60)
                                        {
                                            if (htfirstclass.Contains(scode))
                                            {
                                                int cnt = 0;
                                                if (htfirstclass[scode] == "")
                                                {
                                                    cnt = 1;
                                                }
                                                else
                                                {
                                                    cnt = Convert.ToInt16(htfirstclass[scode]);
                                                    cnt = cnt + 1;
                                                }
                                                htfirstclass[scode] = cnt;
                                            }
                                        }
                                    }
                                    FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                    //FpExternal.Sheets[0].Cells[rowcou,FpExternal.Sheets[0].ColumnCount-4].Text=result.ToString();
                                    if (result == "Fail")
                                    {
                                        failcount++;
                                        FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                        FpExternal.Sheets[0].Cells[rowcou, col].BackColor = Color.LightGray;
                                    }
                                    if ((dvmarkimp[0]["grade"].ToString() == "") && (mod == "3"))
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = " ";
                                    }
                                    else if ((dvmarkimp[0]["grade"].ToString() == "") && (result == "AAA") || (result == "-1"))
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "AAA";
                                        FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                        FpExternal.Sheets[0].Cells[rowcou, col].BackColor = Color.LightGray;
                                        failcount++;
                                        abs1count++;
                                    }
                                    else if ((dvmarkimp[0]["grade"].ToString() == "") && (result == "UA"))
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "UA";
                                        FpExternal.Sheets[0].Cells[rowcou, col].ForeColor = Color.Red;
                                        FpExternal.Sheets[0].Cells[rowcou, col].BackColor = Color.LightGray;
                                        failcount++;
                                        abs1count++;
                                    }
                                    else if ((result == "AAA") || (result == "-1"))
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "AAA";
                                        failcount++;
                                        abs1count++;
                                    }
                                    else if (result == "WHD")//Added by srinath 26/6/2014
                                    {
                                        FpExternal.Sheets[0].Cells[rowcou, col].Text = "WHD";
                                        FpExternal.Sheets[0].Cells[rowcou, col].HorizontalAlign = HorizontalAlign.Center;
                                        failcount++;
                                    }
                                    if (chkflag == false)
                                    {
                                        if (result == "Pass")
                                        {
                                            FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 5].Text = result.ToString();
                                            //FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 5].Tag = result.ToString();
                                        }
                                        else
                                        {
                                            if ((dvmarkimp[0]["grade"].ToString() == "") && (mod == "3"))
                                            {
                                                //  FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, "LE");
                                                FpExternal.Sheets[0].Cells[rowcou, col].Text = " ";
                                                //chkflag = true;
                                            }
                                            else if (result == "")
                                            {
                                                FpExternal.Sheets[0].Cells[rowcou, col].Text = " ";
                                                nomarkcount++;
                                            }
                                            else
                                            {
                                                //FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, "Fail");
                                                FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 5].Text = "Fail";
                                                FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 5].ForeColor = Color.Red;
                                                chkflag = true;
                                                failflag = true;
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        if (vb == 0)
                        {
                            nomarkcount++;
                        }
                    }
                    firstsubbind++;
                    FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 6].Text = failcount.ToString();
                    if (failcount == 0)
                    {
                        FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 6].BackColor = Color.LightPink;
                    }
                    else
                    {
                        if(!CheckBox1.Checked)
                            FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 1].Text = "0";
                    }
                    FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Center;
                    semarrearcount = 0;
                    int ttar = abs1count + semarrearcount;
                    string semval = string.Empty;
                    if (rbbeforeandafterrevaluation.SelectedValue == "1")
                    {
                        semval = " and semester>'" + ddlSemYr.SelectedValue.ToString() + "'";
                    }
                    dsallarecount.Tables[0].DefaultView.RowFilter = "roll_no='" + rollnum + "'" + semval;
                    DataView dvarrval = dsallarecount.Tables[0].DefaultView;
                    if (dvarrval.Count > 0)
                    {
                        semarrearcount = Convert.ToInt32(dvarrval.Count);
                    }
                    if (rbbeforeandafterrevaluation.SelectedValue == "1")
                    {
                        ttar = failcount + semarrearcount;
                    }
                    else
                    {
                        ttar = semarrearcount;
                    }
                    FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 3].Text = ttar.ToString();
                    if (ttar == 0)
                    {
                        FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 3].BackColor = Color.LightPink;
                    }
                    FpExternal.Sheets[0].Cells[rowcou, FpExternal.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                    if (failcount == 0 && nomarkcount == 0 && abscnt == 0)
                    {
                        allpasscount++;
                    }
                    if (nomarkcount == 0 && abscnt == 0)
                    {
                        allappeared++;
                    }
                    if (htfailsubcount.Contains(failcount))
                    {
                        int failcountval = 0;
                        if (htfailsubcount[failcount] == "")
                        {
                            failcountval = failcountval + 1;
                        }
                        else
                        {
                            int val = Convert.ToInt16(htfailsubcount[failcount]);
                            failcountval = val + 1;
                        }
                        htfailsubcount[failcount] = failcountval;
                    }
                }
                int f = FpExternal.Sheets[0].RowCount;
                FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 1;
                // FpExternal.Sheets[0].SpanModel.Add(f, 0, 2, FpExternal.Sheets[0].ColumnCount);
                int tempoverallscount = 0;
                for (int col = 7; col <= FpExternal.Sheets[0].ColumnCount - 7; col++)//rrrr
                {
                    tempoverallscount++;
                    string scod = FpExternal.Sheets[0].ColumnHeader.Cells[1, col].Text;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = scod.ToString();
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Font.Bold = true;
                }
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr
                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "TOTAL NO OF PASSES";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                // FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, FpExternal.Sheets[0].ColumnCount - 3].Text = " NO OF SUBJECTS FAILED";
                //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, FpExternal.Sheets[0].ColumnCount - 3].Font.Bold = true;
                // FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, FpExternal.Sheets[0].ColumnCount - 2].Text = " NO OF STUDENTS";
                // FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, FpExternal.Sheets[0].ColumnCount - 2].Font.Bold = true;
                int startcount = FpExternal.Sheets[0].RowCount - 1;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr
                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "TOTAL NO OF FAILURES";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr
                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "TOTAL NO APPEARED";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr
                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "TOTAL NO OF ABSENTEES";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr
                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "PERCENTAGE OF PASS";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr
                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "NO OF 1ST CLASS";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr
                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "PERCENTAGE OF 1ST CLASS";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr

                //added by rajasekar 11/08/2018
                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "AVERAGE MARK";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr

                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "AVERAGE GRADE";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr

                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "HIGHEST MARK";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr
               
                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "LOWEST MARK";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr

                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "SINGLE SUBJECT FAILURE";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr

                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS IN MANAGEMENT";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr

                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS PASS IN MANAGEMENT";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr

                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS IN GOVT";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr

                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS PASS IN GOVT";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr

                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "PASS PERCENTAGE MANAGEMENT";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr

                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "PASS PERCENTAGE GOVT";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr

                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS IN HOSTEL";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr

                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS PASS IN HOSTEL";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr

                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS IN DAYSCHOLAR";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr

                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS PASS IN DAYSCHOLAR";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr


                //=========================================//
                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "OVERALL PASS";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr
                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "OVERALL PASS %";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr
                int bindsubtypenumber = FpExternal.Sheets[0].RowCount - 1;
                string getsubtypequery = "select distinct subject.subtype_no,subject_type from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code='" + degree_code + "' and semester='" + ddlSemYr.SelectedItem.Text + "'  and batch_year ='" + batch_year + "') order by subject.subtype_no";
                DataSet dsubtype = daccess.select_method_wo_parameter(getsubtypequery, "Text");
                int colvsa = dsubtype.Tables[0].Rows.Count;
                for (int sm = 0; sm < dsubtype.Tables[0].Rows.Count; sm++)
                {
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - colvsa].Text = Convert.ToString(dsubtype.Tables[0].Rows[sm]["subject_type"]);
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - colvsa].Tag = Convert.ToString(dsubtype.Tables[0].Rows[sm]["subtype_no"]);
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - colvsa].Font.Bold = true;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - colvsa].HorizontalAlign = HorizontalAlign.Center;
                    colvsa--;
                }
                // int bindgraderownumber = FpExternal.Sheets[0].RowCount - 1;
                int bindgraderownumber = 0;
                ArrayList alv = new ArrayList();
                SqlDataReader sqldr;
                string fillgrade = "select Mark_Grade from Grade_Master where degree_code=" + degree_code + " and batch_year='" + batch_year + "'order by Frange desc";
                con.Close();
                con.Open();
                SqlCommand sqlcm = new SqlCommand(fillgrade, con);
                sqldr = sqlcm.ExecuteReader();
                int v = 0;
                while (sqldr.Read())
                {
                    FpExternal.Sheets[0].RowCount++;
                    if (v == 0)
                    {
                        bindgraderownumber = FpExternal.Sheets[0].RowCount - 1;
                    }
                    v++;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "Number of '" + Convert.ToString(sqldr["Mark_Grade"]) + "'  grade";
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(sqldr["Mark_Grade"]);
                    alv.Add(Convert.ToString(sqldr["Mark_Grade"]));
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);
                }
                ArrayList asubcode = new ArrayList();
                if (ddlSec.Enabled == false)
                {
                    spsection = string.Empty;
                }
                else
                {
                    if (ddlSec.SelectedItem.Text == "ALL")
                    {
                        spsection = string.Empty;
                    }
                    else
                    {
                        spsection = ddlSec.SelectedItem.Text;
                    }
                }
                for (int col = 7; col <= FpExternal.Sheets[0].ColumnCount - 7; col++)
                {
                    DataView dvbgrade = new DataView();
                    string subnum = FpExternal.Sheets[0].ColumnHeader.Cells[0, col].Tag.ToString();
                    string sco = FpExternal.Sheets[0].ColumnHeader.Cells[1, col].Text;
                    double mintot = Convert.ToDouble(FpExternal.Sheets[0].ColumnHeader.Cells[0, col].Note.ToString());
                    double minintmark = Convert.ToDouble(FpExternal.Sheets[0].ColumnHeader.Cells[1, col].Note.ToString());
                    double minextmark = Convert.ToDouble(FpExternal.Sheets[0].ColumnHeader.Cells[2, col].Note.ToString());
                    if (col == 7)//rrrr
                    {
                        gminintmark = minintmark;
                        gmaxintmark = minextmark;
                    }
                    string beforeorafter = string.Empty;
                    if (rbbeforeandafterrevaluation.SelectedValue == "1")
                    {
                        beforeorafter = "1";
                    }
                    else
                    {
                        beforeorafter = string.Empty;
                    }
                    string highestmark = string.Empty;
                    if (asubcode.Contains(subnum) == false)
                    {
                        con.Close();
                        con.Open();
                        asubcode.Add(subnum);
                        if (hthighestmark.Contains(sco))
                        {
                            highestmark = hthighestmark[sco].ToString();
                            if (gchm == "1")
                            {
                                highestmark = GetFunction("select mark_grade from grade_master where '" + highestmark + "' between frange and trange and degree_code='" + degree_code + "' and batch_year='" + batch_year + "'");
                            }
                        }
                        con.Close();
                        con.Open();
                        SqlCommand studinfo = new SqlCommand("universityprocbranchwiseresult", con);
                        studinfo.CommandType = CommandType.StoredProcedure;
                        studinfo.Parameters.AddWithValue("@degreecode", degree_code);
                        studinfo.Parameters.AddWithValue("@batchyear", batch_year);
                        studinfo.Parameters.AddWithValue("@semester", current_sem);
                        studinfo.Parameters.AddWithValue("@subject_no", subnum);
                        studinfo.Parameters.AddWithValue("@examcode", ExamCode);
                        studinfo.Parameters.AddWithValue("@gradetype", stgetgradeflag);
                        studinfo.Parameters.AddWithValue("@sections", spsection);
                        studinfo.Parameters.AddWithValue("@mintotal", mintot - 1);
                        studinfo.Parameters.AddWithValue("@minintmark", minintmark);
                        studinfo.Parameters.AddWithValue("@minextmark", minextmark);
                        studinfo.Parameters.AddWithValue("@beforeorafter", beforeorafter);
                        SqlDataAdapter studinfoada = new SqlDataAdapter(studinfo);
                        DataSet studinfoads = new DataSet();
                        studinfoada.Fill(studinfoads);
                        if (studinfoads.Tables[0].Rows.Count > 0)
                        {
                            string noofstudentmanagement = string.Empty;
                            string noofstudentpassmanagement = string.Empty;
                            string noofstudentgovt = string.Empty;
                            string noofstudentpassgovt = string.Empty;
                            string studentappeared = string.Empty;
                            string studentpassed = string.Empty;
                            string studenthostel = string.Empty;
                            string studentdayscholar = string.Empty;
                            string studentpassedhostel = string.Empty;
                            string studentpasseddayscholar = string.Empty;
                            string studentfail = string.Empty;
                            string totalstudents = string.Empty;
                            string registeredstud = string.Empty;
                            string absentstud = string.Empty;
                            string passpercent = string.Empty;
                            string firstclasspercent = string.Empty;
                            int firstclasscount = 0;
                            string allpassper = "0";
                            for (int studproci = 0; studproci < studinfoads.Tables[0].Rows.Count; studproci++)
                            {
                                DataSet ds = new DataSet();
                                string pss = string.Empty;
                             
                                totalstudents = studinfoads.Tables[0].Rows[studproci][0].ToString();
                                studentappeared = studinfoads.Tables[1].Rows[studproci][0].ToString();
                                //modified by srinath 30/3/2015
                                string actulgradeorgrade = string.Empty;
                                if (rbbeforeandafterrevaluation.SelectedValue == "1")
                                {
                                    actulgradeorgrade = "m.Actual_Grade   ";
                                }
                                else if (rbbeforeandafterrevaluation.SelectedValue == "2")
                                {
                                    actulgradeorgrade = "m.grade   ";
                                }

                                string seattypequery1 = string.Empty;
                                if (ddlSec.Enabled == true)
                                {
                                    
                                    seattypequery1 = "select distinct seattype,tv.textval as textval from applyn a,registration r,textvaltable tv where r.degree_code='" + degree_code + "' and r.batch_year='" + batch_year + "' and r.app_no=a.app_no and r.sections='" + ddlSec.SelectedItem.Text + "' and a.seattype=tv.textcode";
                                }
                                else
                                {
                                    seattypequery1 = "select distinct seattype,tv.textval as textval from applyn a,registration r,textvaltable tv where r.degree_code='" + degree_code + "' and r.batch_year='" + batch_year + "' and r.app_no=a.app_no  and a.seattype=tv.textcode";
                                }
                                DataSet dssd = new DataSet();
                                dssd = daccess.select_method_wo_parameter(seattypequery1, "text");
                                

                                if (spsection != "")
                                {
                                    // pss = "select count(result) as pass from mark_entry m,registration r where m.roll_no=r.roll_no   and m.attempts=1 and subject_no =  " + subnum + " and exam_code =" + ExamCode + " and m.actual_grade     not in (select mark_grade from grade_master where " + mintot + " between frange and trange and   degree_code=" + degree_code + "     and batch_year=" + batch_year + ")  and r.Sections='" + spsection + "' and r.Roll_No not in(  select r.roll_no from mark_entry m, registration r where m.roll_no=r.roll_no and result ='AAA' and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " and r.Sections='" + spsection + "')    ";
                                    pss = "select count(result) as pass from mark_entry m,registration r where m.roll_no=r.roll_no   and m.attempts>=1 and subject_no =  " + subnum + " and exam_code =" + ExamCode + " and  " + actulgradeorgrade + "     not in (select mark_grade from grade_master where  frange<" + mintot + " and degree_code=" + degree_code + "     and batch_year=" + batch_year + ")  and r.Sections='" + spsection + "' and r.Roll_No not in(  select r.roll_no from mark_entry m, registration r where m.roll_no=r.roll_no and result in ('AAA','UA','WHD') and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " and r.Sections='" + spsection + "') and r.delflag<>1   ";

                                    pss += "select count(result) as pass from mark_entry m,registration r where m.roll_no=r.roll_no and Stud_Type='hostler'  and m.attempts>=1 and subject_no =  " + subnum + " and exam_code =" + ExamCode + " and  " + actulgradeorgrade + "     not in (select mark_grade from grade_master where  frange<" + mintot + " and degree_code=" + degree_code + "     and batch_year=" + batch_year + ")  and r.Sections='" + spsection + "' and r.Roll_No not in(  select r.roll_no from mark_entry m, registration r where m.roll_no=r.roll_no and result in ('AAA','UA','WHD') and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " and r.Sections='" + spsection + "')  and r.delflag<>1  ";

                                    pss += "select count(result) as pass from mark_entry m,registration r where m.roll_no=r.roll_no and Stud_Type='day scholar'  and m.attempts>=1 and subject_no =  " + subnum + " and exam_code =" + ExamCode + " and  " + actulgradeorgrade + "     not in (select mark_grade from grade_master where  frange<" + mintot + " and degree_code=" + degree_code + "     and batch_year=" + batch_year + ")  and r.Sections='" + spsection + "' and r.Roll_No not in(  select r.roll_no from mark_entry m, registration r where m.roll_no=r.roll_no and result in ('AAA','UA','WHD') and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " and r.Sections='" + spsection + "')  and r.delflag<>1  ";

                                    pss += "select count(distinct m.roll_no) as pass from mark_entry m,registration r where m.roll_no=r.roll_no  and  Stud_Type='hostler'        and r.delflag<>1 and m.attempts = 1 and r.sections='" + spsection + "' and subject_no=" + subnum + "  and (result='pass' or result='fail' or result='S') and m.exam_code = " + ExamCode + "  and r.delflag<>1  ";

                                    pss += "select count(distinct m.roll_no) as pass from mark_entry m,registration r where m.roll_no=r.roll_no  and  Stud_Type='day scholar'        and r.delflag<>1 and m.attempts = 1 and r.sections='" + spsection + "' and subject_no=" + subnum + "  and (result='pass' or result='fail' or result='S') and m.exam_code = " + ExamCode + "   and r.delflag<>1 ";
                                    int a=2;
                                    for (int n = 0; n < dssd.Tables[0].Rows.Count; n++)
                                    {
                                        pss += "select count(result) as pass from mark_entry m,registration r,applyn a where m.roll_no=r.roll_no  and a.app_no=r.app_no  and m.attempts>=1 and subject_no =" + subnum + " and exam_code =" + ExamCode + " and a.seattype=" + dssd.Tables[0].Rows[n]["seattype"].ToString() + " and  m.grade        not in (select mark_grade from grade_master where  frange<" + mintot + " and degree_code=" + degree_code + "     and batch_year=" + batch_year + ")  and r.Sections='" + spsection + "' and r.Roll_No not in(  select r.roll_no from mark_entry m, registration r where m.roll_no=r.roll_no and result in ('AAA','UA','WHD') and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " and r.Sections='" + spsection + "') and r.delflag<>1";


                                        pss += "select count(distinct m.roll_no) as pass from mark_entry m,registration r,applyn a where m.roll_no=r.roll_no and a.app_no=r.app_no and a.seattype=" + dssd.Tables[0].Rows[n]["seattype"].ToString() + " and r.delflag<>1 and m.attempts = 1 and r.sections='" + spsection + "' and subject_no=" + subnum + "  and (result='pass' or result='fail' or result='S') and m.exam_code = " + ExamCode + " and r.delflag<>1";
                                    }
                                }
                                else
                                {
                                    // pss = "select count(result) as pass from mark_entry m,registration r where m.roll_no=r.roll_no   and m.attempts=1 and subject_no =  " + subnum + " and exam_code =" + ExamCode + " and m.actual_grade     not in (select mark_grade from grade_master where " + mintot + " between frange and trange and   degree_code=" + degree_code + "     and batch_year=" + batch_year + ")and r.Roll_No not in(  select r.roll_no from mark_entry m, registration r where m.roll_no=r.roll_no and result ='AAA' and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " )   ";
                                    pss = "select count(result) as pass from mark_entry m,registration r where m.roll_no=r.roll_no   and m.attempts>=1 and subject_no =  " + subnum + " and exam_code =" + ExamCode + " and " + actulgradeorgrade + "      not in (select mark_grade from grade_master where frange<" + mintot + " and   degree_code=" + degree_code + "     and batch_year=" + batch_year + ")and r.Roll_No not in(  select r.roll_no from mark_entry m, registration r where m.roll_no=r.roll_no and result in ('AAA','UA','WHD') and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " )  and r.delflag<>1 ";

                                    pss += "select count(result) as pass from mark_entry m,registration r where m.roll_no=r.roll_no and Stud_Type='hostler'  and m.attempts>=1 and subject_no =  " + subnum + " and exam_code =" + ExamCode + " and " + actulgradeorgrade + "      not in (select mark_grade from grade_master where frange<" + mintot + " and   degree_code=" + degree_code + "     and batch_year=" + batch_year + ")and r.Roll_No not in(  select r.roll_no from mark_entry m, registration r where m.roll_no=r.roll_no and result in ('AAA','UA','WHD') and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " ) and r.delflag<>1   ";

                                    pss += "select count(result) as pass from mark_entry m,registration r where m.roll_no=r.roll_no and Stud_Type='day scholar'  and m.attempts>=1 and subject_no =  " + subnum + " and exam_code =" + ExamCode + " and " + actulgradeorgrade + "      not in (select mark_grade from grade_master where frange<" + mintot + " and   degree_code=" + degree_code + "     and batch_year=" + batch_year + ")and r.Roll_No not in(  select r.roll_no from mark_entry m, registration r where m.roll_no=r.roll_no and result in ('AAA','UA','WHD') and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " )  and r.delflag<>1 ";

                                    pss += "select count(distinct m.roll_no) as pass from mark_entry m,registration r where m.roll_no=r.roll_no  and  Stud_Type='hostler'        and r.delflag<>1 and m.attempts = 1 and subject_no=" + subnum + "  and (result='pass' or result='fail' or result='S') and m.exam_code = " + ExamCode + "  and r.delflag<>1  ";

                                    pss += "select count(distinct m.roll_no) as pass from mark_entry m,registration r where m.roll_no=r.roll_no  and  Stud_Type='day scholar'        and r.delflag<>1 and m.attempts = 1 and subject_no=" + subnum + "  and (result='pass' or result='fail' or result='S') and m.exam_code = " + ExamCode + "  and r.delflag<>1  ";

                                    int a = 2;
                                    for (int n = 0; n < dssd.Tables[0].Rows.Count; n++)
                                    {
                                        pss += "select count(result) as pass from mark_entry m,registration r,applyn a where m.roll_no=r.roll_no  and a.app_no=r.app_no  and m.attempts>=1 and subject_no =" + subnum + " and exam_code =" + ExamCode + " and a.seattype=" + dssd.Tables[0].Rows[n]["seattype"].ToString() + " and  m.grade        not in (select mark_grade from grade_master where  frange<" + mintot + " and degree_code=" + degree_code + "     and batch_year=" + batch_year + ")   and r.Roll_No not in(  select r.roll_no from mark_entry m, registration r where m.roll_no=r.roll_no and result in ('AAA','UA','WHD') and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " ) and r.delflag<>1";


                                        pss += "select count(distinct m.roll_no) as pass from mark_entry m,registration r,applyn a where m.roll_no=r.roll_no and a.app_no=r.app_no and a.seattype=" + dssd.Tables[0].Rows[n]["seattype"].ToString() + " and r.delflag<>1 and m.attempts = 1  and subject_no=" + subnum + "  and (result='pass' or result='fail' or result='S') and m.exam_code = " + ExamCode + " and r.delflag<>1";
                                    }
                                }
                                ds.Clear();
                                ds = daccess.select_method_wo_parameter(pss, "text");
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    studentpassed = ds.Tables[0].Rows[0]["pass"].ToString();
                                }
                                else
                                {
                                    studentpassed = "0";
                                }
                                if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                                {
                                    studentpassedhostel = ds.Tables[1].Rows[0]["pass"].ToString();
                                }
                                else
                                {
                                    studentpassedhostel = "0";
                                }
                                if (ds.Tables.Count > 0 && ds.Tables[2].Rows.Count > 0)
                                {
                                    studentpasseddayscholar = ds.Tables[2].Rows[0]["pass"].ToString();
                                }
                                else
                                {
                                    studentpasseddayscholar = "0";
                                }

                                if (ds.Tables.Count > 0 && ds.Tables[3].Rows.Count > 0)
                                {
                                    studenthostel = ds.Tables[3].Rows[0]["pass"].ToString();
                                }
                                else
                                {
                                    studenthostel = "0";
                                }
                                if (ds.Tables.Count > 0 && ds.Tables[4].Rows.Count > 0)
                                {
                                    studentdayscholar = ds.Tables[4].Rows[0]["pass"].ToString();
                                }
                                else
                                {
                                    studentdayscholar = "0";
                                }
                                if (ds.Tables.Count > 5)
                                {
                                    if (ds.Tables.Count > 0 && ds.Tables[5].Rows.Count > 0)
                                    {
                                        noofstudentpassmanagement = ds.Tables[5].Rows[0]["pass"].ToString();

                                    }
                                }
                                else
                                {
                                    noofstudentpassmanagement = "0";
                                }

                                if (ds.Tables.Count > 6)
                                {
                                    if (ds.Tables.Count > 0 && ds.Tables[6].Rows.Count > 0)
                                    {
                                        noofstudentmanagement = ds.Tables[6].Rows[0]["pass"].ToString();

                                    }
                                }
                                else
                                {
                                    noofstudentmanagement = "0";
                                }
                                


                                if (ds.Tables.Count > 7)
                                {
                                    if (ds.Tables[7].Rows.Count > 0)
                                    noofstudentpassgovt = ds.Tables[7].Rows[0]["pass"].ToString();

                                }
                                else
                                {
                                    noofstudentpassgovt = "0";
                                }
                                if (ds.Tables.Count > 7)
                                {
                                    if (ds.Tables[8].Rows.Count > 0)
                                    noofstudentgovt = ds.Tables[8].Rows[0]["pass"].ToString();

                                }
                                else
                                {
                                    noofstudentgovt = "0";
                                }
                                // studentpassed = studinfoads.Tables[2].Rows[studproci][0].ToString();
                                // studentfail = studinfoads.Tables[3].Rows[studproci][0].ToString();
                                if (spsection != "")
                                {
                                    string sqlfailcount = "  select count(result) as failcount from mark_entry m, registration r where m.roll_no=r.roll_no and result in ('Fail','WHD') and r.Sections='" + spsection + "'  and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " and r.delflag<>1 ";
                                    ds.Clear();
                                    ds = daccess.select_method_wo_parameter(sqlfailcount, "text");
                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        studentfail = ds.Tables[0].Rows[0]["failcount"].ToString();
                                    }
                                    else
                                    {
                                        studentfail = "0";
                                    }
                                    //absentstud = studinfoads.Tables[5].Rows[studproci][0].ToString();
                                }
                                else
                                {
                                    string sqlfailcount = "  select count(result) as failcount from mark_entry m, registration r where m.roll_no=r.roll_no and result in ('Fail','WHD') and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " and r.delflag<>1";
                                    ds.Clear();
                                    ds = daccess.select_method_wo_parameter(sqlfailcount, "text");
                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        studentfail = ds.Tables[0].Rows[0]["failcount"].ToString();
                                    }
                                    else
                                    {
                                        studentfail = "0";
                                    }
                                }
                                registeredstud = studinfoads.Tables[4].Rows[studproci][0].ToString();
                                if (spsection != "")
                                {
                                    string abs = "  select count(result) as absc from mark_entry m, registration r where m.roll_no=r.roll_no and result in ('AAA','UA') and r.Sections='" + spsection + "'  and passorfail=0 and subject_no ='" + subnum + "' and  exam_code ='" + ExamCode + "' and r.delflag<>1";
                                    ds.Clear();
                                    ds = daccess.select_method_wo_parameter(abs, "text");
                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        absentstud = ds.Tables[0].Rows[0]["absc"].ToString();
                                    }
                                    else
                                    {
                                        absentstud = "0";
                                    }
                                    //absentstud = studinfoads.Tables[5].Rows[studproci][0].ToString();
                                }
                                else
                                {
                                    string abs = "  select count(result) as absc from mark_entry m, registration r where m.roll_no=r.roll_no and result in ('AAA','UA') and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " and r.delflag<>1";
                                    ds.Clear();
                                    ds = daccess.select_method_wo_parameter(abs, "text");
                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        absentstud = ds.Tables[0].Rows[0]["absc"].ToString();
                                    }
                                    else
                                    {
                                        absentstud = "0";
                                    }
                                }
                                int total = Convert.ToInt32(studentpassed) + Convert.ToInt32(studentfail);
                                string fclasscnt = string.Empty;
                                if (htfirstclass.Contains(sco))
                                {
                                    fclasscnt = htfirstclass[sco].ToString();
                                }
                                if (fclasscnt != "")
                                {
                                    firstclasscount = Convert.ToInt16(fclasscnt);
                                }
                                else
                                {
                                    firstclasscount = 0;
                                }
                                if (studentpassed != "0")
                                {
                                    double passpercent1 = 0;
                                    passpercent1 = Convert.ToDouble((Convert.ToDouble(studentpassed) / total) * 100);
                                    double passpercent2 = Math.Round(passpercent1, 2);
                                    string passpercent2_infi = String.Format("{0:0,0.00}", float.Parse(passpercent2.ToString()));
                                    if (passpercent2_infi == "NaN")
                                    {
                                        passpercent2 = 0;
                                    }
                                    else if (passpercent2_infi == "Infinity")
                                    {
                                        passpercent2 = 0;
                                    }
                                    passpercent = Convert.ToString(passpercent2);
                                }
                                if (firstclasscount != 0)
                                {
                                    double passpercent1 = 0;
                                    passpercent1 = Convert.ToDouble((Convert.ToDouble(firstclasscount) / total) * 100);
                                    double passpercent2 = Math.Round(passpercent1, 2);
                                    string passpercent2_infi = String.Format("{0:0,0.00}", float.Parse(passpercent2.ToString()));
                                    if (passpercent2_infi == "NaN")
                                    {
                                        passpercent2 = 0;
                                    }
                                    else if (passpercent2_infi == "Infinity")
                                    {
                                        passpercent2 = 0;
                                    }
                                    firstclasspercent = Convert.ToString(passpercent2);
                                }
                                if (allpasscount != 0)
                                {
                                    double passpercent1 = 0;
                                    passpercent1 = Convert.ToDouble((Convert.ToDouble(allpasscount) / allappeared) * 100);
                                    double passpercent2 = Math.Round(passpercent1, 2);
                                    string passpercent2_infi = String.Format("{0:0,0.00}", float.Parse(passpercent2.ToString()));
                                    if (passpercent2_infi == "NaN")
                                    {
                                        passpercent2 = 0;
                                    }
                                    else if (passpercent2_infi == "Infinity")
                                    {
                                        passpercent2 = 0;
                                    }
                                    allpassper = Convert.ToString(passpercent2);
                                }
                            }
                            FpExternal.Sheets[0].Cells[startcount, col].Text = studentpassed.ToString();
                            FpExternal.Sheets[0].Cells[startcount, col].HorizontalAlign = HorizontalAlign.Center;
                            FpExternal.Sheets[0].Cells[startcount + 1, col].Text = studentfail.ToString();
                            FpExternal.Sheets[0].Cells[startcount + 1, col].HorizontalAlign = HorizontalAlign.Center;
                            FpExternal.Sheets[0].Cells[startcount + 2, col].Text = studentappeared.ToString();
                            FpExternal.Sheets[0].Cells[startcount + 2, col].HorizontalAlign = HorizontalAlign.Center;
                            FpExternal.Sheets[0].Cells[startcount + 3, col].Text = absentstud.ToString();
                            FpExternal.Sheets[0].Cells[startcount + 3, col].HorizontalAlign = HorizontalAlign.Center;
                            FpExternal.Sheets[0].Cells[startcount + 4, col].Text = passpercent.ToString();
                            htstaffdetails.Add(subnum, passpercent);
                            FpExternal.Sheets[0].Cells[startcount + 4, col].HorizontalAlign = HorizontalAlign.Center;
                            FpExternal.Sheets[0].Cells[startcount + 5, col].Text = firstclasscount.ToString();
                            FpExternal.Sheets[0].Cells[startcount + 5, col].HorizontalAlign = HorizontalAlign.Center;
                            FpExternal.Sheets[0].Cells[startcount + 6, col].Text = firstclasspercent.ToString();
                            FpExternal.Sheets[0].Cells[startcount + 6, col].HorizontalAlign = HorizontalAlign.Center;


                            //Added by rajasekar 10/08/2018
                           

                            //string avegrade = GetFunction("select mark_grade from grade_master where '" + passpercent + "' between frange and trange and degree_code='" + degree_code + "' and batch_year='" + batch_year + "'");
                            string averagetrange = "0";
                            string averagegrade = "0";
                            DataSet trange = new DataSet();

                            if (spsection != "")
                            {
                                averagetrange = "select count(m.grade) as tot,grade from mark_entry m,Registration r where r.Batch_Year='" + ddlBatch.SelectedItem + "' and r.degree_code='" + ddlBranch.SelectedValue + "' and r.college_code='" + ddl_college.SelectedValue + "' and r.Sections='"+ spsection +"' and r.Roll_No=m.roll_no and  m.subject_no='" + subnum + "' and exam_code ='" + ExamCode + "' group by m.grade";
                                
                            }
                            else
                            {
                                averagetrange = "select count(m.grade) as tot,grade from mark_entry m,Registration r where r.Batch_Year='" + ddlBatch.SelectedItem + "' and r.degree_code='" + ddlBranch.SelectedValue + "' and r.college_code='" + ddl_college.SelectedValue + "'  and r.Roll_No=m.roll_no and  m.subject_no='" + subnum + "' and exam_code ='" + ExamCode + "' group by m.grade";
                                
                            }

                            
                            trange = daccess.select_method_wo_parameter(averagetrange, "text");
                            int count = 0;
                            string range = "0";
                            double totmark = 0;
                            double totmark1 = 0;
                            double avemark = 0;
                            if (trange.Tables[0].Rows.Count > 0 && trange.Tables.Count > 0)
                            {
                                for (int ss = 0; ss < trange.Tables[0].Rows.Count; ss++)
                                {
                                    range = daccess.GetFunction("select trange from grade_master where  mark_grade ='" + Convert.ToString(trange.Tables[0].Rows[ss]["grade"]).Trim() + "' and degree_code='" + ddlBranch.SelectedValue + "' and batch_year='" + ddlBatch.SelectedItem + "' and College_Code='" + ddl_college.SelectedValue + "'");
                                    totmark += (Convert.ToDouble(range) * Convert.ToDouble(trange.Tables[0].Rows[ss]["tot"]));
                                    count += Convert.ToInt32(trange.Tables[0].Rows[ss]["tot"]);
                                }

                                totmark1 = count * 100;

                                avemark = (totmark / totmark1);
                                if (avemark>0)
                              avemark = Convert.ToInt32(avemark * 100);

                            }


                            averagegrade = daccess.GetFunction("select mark_grade from grade_master where '" + Convert.ToString(avemark) + "' between frange and trange and degree_code='" + ddlBranch.SelectedValue + "' and batch_year='" + ddlBatch.SelectedItem + "' and College_Code='" + ddl_college.SelectedValue + "'");


                            FpExternal.Sheets[0].Cells[startcount + 7, col].Text = avemark.ToString();//rrrr
                            FpExternal.Sheets[0].Cells[startcount + 7, col].HorizontalAlign = HorizontalAlign.Center;

                            FpExternal.Sheets[0].Cells[startcount + 8, col].Text = averagegrade;//rrrr
                            FpExternal.Sheets[0].Cells[startcount + 8, col].HorizontalAlign = HorizontalAlign.Center;


                            string hightrange = "0";
                            string lowtrange = "0";
                            string highgrade = "0";
                            string lowgrade = "0";

                            hightrange = daccess.GetFunction("select MAX(Trange) from Grade_Master where Degree_Code='" + ddlBranch.SelectedValue + "' and batch_year='" + ddlBatch.SelectedItem + "' and College_Code='" + ddl_college.SelectedValue + "' and Mark_Grade in(select distinct m.grade from mark_entry m,Registration r where r.Roll_No=m.roll_no and m.subject_no='" + Convert.ToString(subnum).Trim() + "'and Frange>='50')");

                            highgrade = daccess.GetFunction("select mark_grade from grade_master where  trange ='" + hightrange + "' and degree_code='" + ddlBranch.SelectedValue + "' and batch_year='" + ddlBatch.SelectedItem + "' and College_Code='" + ddl_college.SelectedValue + "'");

                            lowtrange = daccess.GetFunction("select MIN(Trange) from Grade_Master where Degree_Code='" + ddlBranch.SelectedValue + "' and batch_year='" + ddlBatch.SelectedItem + "' and College_Code='" + ddl_college.SelectedValue + "' and Mark_Grade in(select distinct m.grade from mark_entry m,Registration r where r.Roll_No=m.roll_no and m.subject_no='" + Convert.ToString(subnum).Trim() + "' and Frange>='50')");

                            lowgrade = daccess.GetFunction("select mark_grade from grade_master where  trange ='" + lowtrange + "' and degree_code='" + ddlBranch.SelectedValue + "' and batch_year='" + ddlBatch.SelectedItem + "' and College_Code='" + ddl_college.SelectedValue + "'");

                           

                            FpExternal.Sheets[0].Cells[startcount + 9, col].Text = highestmark.ToString();
                            FpExternal.Sheets[0].Cells[startcount + 9, col].HorizontalAlign = HorizontalAlign.Center;




                            FpExternal.Sheets[0].Cells[startcount + 10, col].Text = lowgrade;//rrrr
                            FpExternal.Sheets[0].Cells[startcount + 10, col].HorizontalAlign = HorizontalAlign.Center;

                            int singlesubfailcount = 0;
                            string selquery ="";
                            if (spsection != "")
                            {
                                selquery = "select m.roll_no,m.subject_no from mark_entry m,Exam_Details ed,Registration r where m.exam_code=ed.exam_code and ed.batch_year='" + ddlBatch.SelectedValue + "' and ed.degree_code='" + ddlBranch.SelectedValue + "' and ed.current_semester='" + ddlSemYr.SelectedItem + "' and result<>'pass' and r.Roll_No=m.roll_no and r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and r.Sections='" + spsection + "' ";
                            }
                            else
                            {
                                selquery = "select m.roll_no,m.subject_no from mark_entry m,Exam_Details ed,Registration r where m.exam_code=ed.exam_code and ed.batch_year='" + ddlBatch.SelectedValue + "' and ed.degree_code='" + ddlBranch.SelectedValue + "' and ed.current_semester='" + ddlSemYr.SelectedItem + "' and result<>'pass' and r.Roll_No=m.roll_no and r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code";
                            }

                            DataSet single = new DataSet();
                            DataTable rollno = new DataTable();
                            Hashtable singlesubroll = new Hashtable();
                            single = daccess.select_method_wo_parameter(selquery, "text");

                            if (single.Tables.Count > 0 && single.Tables[0].Rows.Count > 0)
                            {
                                for (int cv = 0; cv < single.Tables[0].Rows.Count; cv++)
                                {
                                    string rollnum = single.Tables[0].Rows[cv]["roll_no"].ToString();
                                    if (!singlesubroll.Contains(rollnum))
                                    {
                                        singlesubroll.Add(rollnum, "");
                                        single.Tables[0].DefaultView.RowFilter = "roll_no='" + single.Tables[0].Rows[cv]["roll_no"].ToString() + "'";
                                        DataTable dtsingle = single.Tables[0].DefaultView.ToTable();
                                        if (dtsingle.Rows.Count == 1)
                                        {
                                            string subno=single.Tables[0].Rows[cv]["subject_no"].ToString();
                                            if (subno == subnum)
                                                singlesubfailcount++;
                                        }

                                    }
                                }
                            }


                            FpExternal.Sheets[0].Cells[startcount + 11, col].Text = singlesubfailcount.ToString();//rrrr
                            FpExternal.Sheets[0].Cells[startcount + 11, col].HorizontalAlign = HorizontalAlign.Center;

                            FpExternal.Sheets[0].Cells[startcount + 12, col].Text = noofstudentmanagement;//rrrr
                            FpExternal.Sheets[0].Cells[startcount + 12, col].HorizontalAlign = HorizontalAlign.Center;

                            FpExternal.Sheets[0].Cells[startcount + 13, col].Text = noofstudentpassmanagement;//rrrr
                            FpExternal.Sheets[0].Cells[startcount + 13, col].HorizontalAlign = HorizontalAlign.Center;

                            FpExternal.Sheets[0].Cells[startcount + 14, col].Text = noofstudentgovt;//rrrr
                            FpExternal.Sheets[0].Cells[startcount + 14, col].HorizontalAlign = HorizontalAlign.Center;

                            FpExternal.Sheets[0].Cells[startcount + 15, col].Text = noofstudentpassgovt;//rrrr
                            FpExternal.Sheets[0].Cells[startcount + 15, col].HorizontalAlign = HorizontalAlign.Center;

                            int totcount = Convert.ToInt16(noofstudentmanagement);
                            int totpasscount = Convert.ToInt16(noofstudentpassmanagement);
                            double managementpassper1 = 0;
                            managementpassper1 = Convert.ToDouble((Convert.ToDouble(totpasscount) / totcount) * 100);
                            double managementpassper2 = Math.Round(managementpassper1, 2);
                            string managementallpassper = Convert.ToString(managementpassper2);

                            FpExternal.Sheets[0].Cells[startcount + 16, col].Text = managementallpassper;//rrrr
                            FpExternal.Sheets[0].Cells[startcount + 16, col].HorizontalAlign = HorizontalAlign.Center;


                            int totcount1 = Convert.ToInt16(noofstudentgovt);
                            int totpasscount1 = Convert.ToInt16(noofstudentpassgovt);
                            double govtpassper1 = 0;
                            govtpassper1 = Convert.ToDouble((Convert.ToDouble(totpasscount1) / totcount1) * 100);
                            double govtpassper2 = Math.Round(govtpassper1, 2);
                            string govtallpassper = Convert.ToString(govtpassper2);

                            FpExternal.Sheets[0].Cells[startcount + 17, col].Text = govtallpassper;//rrrr
                            FpExternal.Sheets[0].Cells[startcount + 17, col].HorizontalAlign = HorizontalAlign.Center;

                            FpExternal.Sheets[0].Cells[startcount + 18, col].Text = studenthostel;//rrrr
                            FpExternal.Sheets[0].Cells[startcount + 18, col].HorizontalAlign = HorizontalAlign.Center;

                            FpExternal.Sheets[0].Cells[startcount + 19, col].Text = studentpassedhostel;//rrrr
                            FpExternal.Sheets[0].Cells[startcount + 19, col].HorizontalAlign = HorizontalAlign.Center;

                            FpExternal.Sheets[0].Cells[startcount + 20, col].Text = studentdayscholar;//rrrr
                            FpExternal.Sheets[0].Cells[startcount + 20, col].HorizontalAlign = HorizontalAlign.Center;

                            FpExternal.Sheets[0].Cells[startcount + 21, col].Text = studentpasseddayscholar;//rrrr
                            FpExternal.Sheets[0].Cells[startcount + 21, col].HorizontalAlign = HorizontalAlign.Center;

                            //============================//
                            if (gcount == 0)
                            {
                                FpExternal.Sheets[0].Cells[startcount + 22, col].Text = allpasscount.ToString();
                                FpExternal.Sheets[0].Cells[startcount + 22, col].HorizontalAlign = HorizontalAlign.Center;
                                FpExternal.Sheets[0].SpanModel.Add(startcount + 22, 7, 1, tempoverallscount);
                                FpExternal.Sheets[0].Cells[startcount + 23, col].Text = allpassper.ToString();
                                FpExternal.Sheets[0].Cells[startcount + 23, col].HorizontalAlign = HorizontalAlign.Center;
                                FpExternal.Sheets[0].SpanModel.Add(startcount + 23, 7, 1, tempoverallscount);
                            }
                            gcount++;
                        }
                        if (alv.Count > 0)
                        {
                            for (int cv = 0; cv < alv.Count; cv++)
                            {
                                dgrades.DefaultView.RowFilter = "subjectno='" + subnum + "' and grade='" + alv[cv].ToString() + "' ";
                                dvbgrade = dgrades.DefaultView;
                                if (dvbgrade.Count > 0)
                                {
                                    FpExternal.Sheets[0].Cells[bindgraderownumber + cv, col].Text = Convert.ToString(dvbgrade[0]["count"]);
                                    FpExternal.Sheets[0].Cells[bindgraderownumber + cv, col].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                    }
                }
                // int gggg=dgrades.Columns.cou;
                //for (int totalcv = FpExternal.Sheets[0].ColumnCount - 5; totalcv < FpExternal.Sheets[0].ColumnCount; totalcv++)
                //{
                //   for(int snum=6;snum < FpExternal.Sheets[0].ColumnCount - 6;snum++)
                //   {
                //       string subnu = FpExternal.Sheets[0].ColumnHeader.Cells[0, snum].Tag.ToString();
                //    string subnum =string.Empty;
                //    DataView dvsubtypegrade = new DataView();
                //    if (FpExternal.Sheets[0].Cells[bindsubtypenumber , totalcv].Text != "")
                //    {
                //        subnum = Convert.ToString(FpExternal.Sheets[0].Cells[bindsubtypenumber, totalcv].Tag);
                //        for (int cv = 0; cv < alv.Count; cv++)
                //        {
                //            dgrades.DefaultView.RowFilter = "subtypenumber='" + subnum + "' and grade='" + alv[cv].ToString() + "' and subjectno='"+subnu+"' ";
                //            dvsubtypegrade = dgrades.DefaultView;
                //            if (dvsubtypegrade.Count > 0)
                //            {
                //                FpExternal.Sheets[0].Cells[bindsubtypenumber + cv+1, totalcv].Text = Convert.ToString(dvsubtypegrade.Count);
                //            }
                //        }
                //    }
                //}
                //}
                int temprow = bindsubtypenumber + 1;
                for (int i = temprow; i < FpExternal.Sheets[0].RowCount; i++)
                {
                    for (int totalcv = FpExternal.Sheets[0].ColumnCount - 6; totalcv < FpExternal.Sheets[0].ColumnCount; totalcv++)
                    {
                        Hashtable ht = new Hashtable();
                        string subjecttype = string.Empty;
                        string previoussub = string.Empty;
                        double total = 0;
                        subjecttype = FpExternal.Sheets[0].Cells[bindsubtypenumber, totalcv].Text;
                        if (subjecttype != "")
                        {
                            //subjecttype = FpExternal.Sheets[0].ColumnHeader.Cells[0, snum].Text;
                            for (int snum = 7; snum <= FpExternal.Sheets[0].ColumnCount - 7; snum++)//rrrr
                            {
                                previoussub = FpExternal.Sheets[0].ColumnHeader.Cells[0, snum].Text;
                                if (previoussub == subjecttype)
                                {
                                    total = total + Convert.ToDouble(FpExternal.Sheets[0].Cells[i, snum].Text);
                                }
                            }
                            FpExternal.Sheets[0].Cells[i, totalcv].Text = Convert.ToString(total);
                            FpExternal.Sheets[0].Cells[i, totalcv].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
                //int count = 0;
                //for (int snum = 6; snum < FpExternal.Sheets[0].ColumnCount - 6; snum++)
                //{
                //    subjecttype = FpExternal.Sheets[0].ColumnHeader.Cells[0, snum].Text;
                //    if (previoussub != "" && previoussub == subjecttype)
                //    {
                //        count = count + Convert.ToInt32(FpExternal.Sheets[0].Cells[bindsubtypenumber, snum]);
                //    }
                //    else
                //    {
                //        ht.Add(previoussub, count);
                //        count = 0;
                //    }
                //    previoussub = subjecttype;
                //}
                // FpExternal.Sheets[0].Cells[bindsubtypenumber,totalcv]
                int rowc = startcount - 1;
                //for (int checkcnt = 1; checkcnt <= htfailsubcount.Count; checkcnt++)
                //{
                //    int tcount = 0;
                //    int rc = rowc;
                //    if (htfailsubcount.Contains(checkcnt))
                //    {
                //        if (FpExternal.Sheets[0].RowCount - 1 > rc)
                //        {
                //            rowc++;
                //            if (htfailsubcount[checkcnt].ToString() == "")
                //            {
                //                tcount = 0;
                //            }
                //            else
                //            {
                //                tcount = Convert.ToInt16(htfailsubcount[checkcnt].ToString());
                //            }
                //            FpExternal.Sheets[0].Cells[rowc, FpExternal.Sheets[0].ColumnCount - 3].Text = checkcnt.ToString();
                //            FpExternal.Sheets[0].Cells[rowc, FpExternal.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                //            FpExternal.Sheets[0].Cells[rowc, FpExternal.Sheets[0].ColumnCount - 2].Text = tcount.ToString();
                //            FpExternal.Sheets[0].Cells[rowc, FpExternal.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                //        }
                //        else
                //        {
                //            FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 1;
                //            rowc = FpExternal.Sheets[0].RowCount;
                //            if (htfailsubcount[checkcnt].ToString() == "")
                //            {
                //                tcount = 0;
                //            }
                //            else
                //            {
                //                tcount = Convert.ToInt16(htfailsubcount[checkcnt].ToString());
                //            }
                //            FpExternal.Sheets[0].Cells[rowc - 1, FpExternal.Sheets[0].ColumnCount - 3].Text = checkcnt.ToString();
                //            FpExternal.Sheets[0].Cells[rowc - 1, FpExternal.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                //            FpExternal.Sheets[0].Cells[rowc - 1, FpExternal.Sheets[0].ColumnCount - 2].Text = tcount.ToString();
                //            FpExternal.Sheets[0].Cells[rowc - 1, FpExternal.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                //        }
                //    }
                //}
                FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 2;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Text = " NO OF FAILED SUBJECT COUNT";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS FAILED";
                int colval = 7;//rrrr
                for (int checkcnt = 1; checkcnt <= htfailsubcount.Count; checkcnt++)
                {
                    int tcount = 0;
                    if (htfailsubcount.ContainsKey(checkcnt))
                    {
                        if (htfailsubcount[checkcnt].ToString() == "")
                        {
                            tcount = 0;
                        }
                        else
                        {
                            tcount = Convert.ToInt16(htfailsubcount[checkcnt].ToString());
                        }
                    }
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, colval].Text = checkcnt.ToString();
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, colval].HorizontalAlign = HorizontalAlign.Center;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colval].Text = tcount.ToString();
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colval].HorizontalAlign = HorizontalAlign.Center;
                    colval++;
                }
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 7);//rrrr
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Font.Bold = true;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 0, 1, 7);//rrrr
                int d = FpExternal.Sheets[0].RowCount;
                // FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 1;
                // FpExternal.Sheets[0].SpanModel.Add(d, 0,1, FpExternal.Sheets[0].ColumnCount);
                string bindstaffdetails = string.Empty;
                if (ddlSec.SelectedItem.Text.Trim().ToLower() == "all" || ddlSec.SelectedItem.Text.Trim().ToLower() == "" || ddlSec.Enabled == false)
                {
                    //bindstaffdetails = "select distinct st.staff_code,sm.staff_name, s.subject_no,s.acronym,s.subject_code,s.subject_name,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r,staffmaster sm where r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and st.sections=r.sections and semester in('" + ddlSemYr.SelectedItem.Text + "') and r.cc=0 and delflag=0 and exam_flag<>'Debar' and sy.degree_code='" + degree_code + "' and sm.staff_code=st.staff_code  order by st.batch_year,sy.degree_code ,s.subject_no,semester,st.sections";
                    bindstaffdetails = "select distinct st.staff_code,sm.staff_name, s.subject_no,s.acronym,s.subject_code,s.subject_name,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,staffmaster sm,sub_sem sb where s.subject_no=st.subject_no and sm.staff_code=st.staff_code and s.subType_no=sb.subType_no and sy.syll_code=sb.syll_code and sy.Batch_Year=st.batch_year and sy.Batch_Year='" + ddlBatch.SelectedItem.ToString() + "' and sy.degree_code='" + degree_code + "' and sy.semester in ('" + ddlSemYr.SelectedItem.Text + "') and sb.promote_count=1 order by s.subject_no";
                }
                else
                {
                    //bindstaffdetails = "select distinct st.staff_code,sm.staff_name, s.subject_no,s.acronym,s.subject_code,s.subject_name,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r,staffmaster sm where r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and st.sections=r.sections and semester in('" + ddlSemYr.SelectedItem.Text + "') and r.cc=0 and delflag=0 and exam_flag<>'Debar' and sy.degree_code='" + degree_code + "' and sm.staff_code=st.staff_code and st.sections='" + ddlSec.SelectedItem.Text + "'  order by st.batch_year,sy.degree_code ,s.subject_no,semester,st.sections";
                    bindstaffdetails = "select distinct st.staff_code,sm.staff_name, s.subject_no,s.acronym,s.subject_code,s.subject_name,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,staffmaster sm,sub_sem sb where s.subject_no=st.subject_no and sm.staff_code=st.staff_code and s.subType_no=sb.subType_no and sy.syll_code=sb.syll_code and sy.Batch_Year=st.batch_year and sy.Batch_Year='" + ddlBatch.SelectedItem.ToString() + "' and sy.degree_code='" + degree_code + "' and sy.semester in ('" + ddlSemYr.SelectedItem.Text + "') and st.sections='" + ddlSec.SelectedItem.Text + "' and sb.promote_count=1 order by s.subject_no";
                }
                DataSet dsstaff = daccess.select_method(bindstaffdetails, hat, "Text");
                int ntcount = 0;
                if (dsstaff.Tables[0].Rows.Count > 0)
                {
                    int sprcount = FpExternal.Sheets[0].RowCount;
                    FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 1;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7].Text = "SubCode";
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7].Font.Bold = true;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8].Text = "SubName";
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8].Font.Bold = true;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9].Text = "Staff Name";
                    FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 9, 1, 4);
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9].Font.Bold = true;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 13].Text = "%";
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 13].Font.Bold = true;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Center;
                    int spanstaffnamerow = FpExternal.Sheets[0].RowCount + 1;
                    for (int col = 0; col < dsstaff.Tables[0].Rows.Count; col++)
                    {
                        ntcount++;
                        FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 1;
                        string subcode = dsstaff.Tables[0].Rows[col]["subject_code"].ToString();
                        string subname = dsstaff.Tables[0].Rows[col]["acronym"].ToString();
                        string staffname = dsstaff.Tables[0].Rows[col]["staff_name"].ToString();
                        string subnumber = dsstaff.Tables[0].Rows[col]["subject_no"].ToString();
                        string passperc = string.Empty;
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7].Text = subcode.ToString();
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8].Text = subname.ToString();
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9].Text = staffname.ToString();
                        if (htstaffdetails.ContainsKey(subnumber))
                        {
                            passperc = Convert.ToString(htstaffdetails[subnumber]);
                        }
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(passperc);
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Center;
                        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 9, 1, 4);
                        //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1,9, 1, 3);
                    }
                    FpExternal.Sheets[0].SpanModel.Add(sprcount, 0, ntcount + 1, 7);//rrrr
                    FpExternal.Sheets[0].SpanModel.Add(sprcount, 14, ntcount + 1, FpExternal.Sheets[0].ColumnCount);
                }
                int g = FpExternal.Sheets[0].RowCount;
                FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 1;
                FpExternal.Sheets[0].SpanModel.Add(g, 0, 1, FpExternal.Sheets[0].ColumnCount);
                string seattypequery = string.Empty;
                if (ddlSec.Enabled == true)
                {
                    //seattypequery = "select distinct n.seattype as seattype,tv.textval as textval  from applyn n,textvaltable tv where degree_code='" + degree_code + "' and current_semester='" + current_sem + "' and batch_year='" + batch_year + "'and n.seattype=tv.textcode ";
                    seattypequery = "select distinct seattype,tv.textval as textval from applyn a,registration r,textvaltable tv where r.degree_code='" + degree_code + "' and r.batch_year='" + batch_year + "' and r.app_no=a.app_no and r.sections='" + ddlSec.SelectedItem.Text + "' and a.seattype=tv.textcode";
                }
                else
                {
                    seattypequery = "select distinct seattype,tv.textval as textval from applyn a,registration r,textvaltable tv where r.degree_code='" + degree_code + "' and r.batch_year='" + batch_year + "' and r.app_no=a.app_no  and a.seattype=tv.textcode";
                }
                SqlDataReader sqldrseat;
                con.Close();
                con.Open();
                SqlCommand cmdseat = new SqlCommand(seattypequery, con);
                sqldrseat = cmdseat.ExecuteReader();
                int colcount = 7;  //modified by Mullai
                int dc = colcount;
                string seatcode = string.Empty;
                string seatname = string.Empty;
                int ccountcheck = 0;
                int checkrcou = 0;
                checkrcou = FpExternal.Sheets[0].RowCount - 1;
                int cf = 0;
                while (sqldrseat.Read())
                {
                    ccountcheck++;
                    colcount++;
                    cf++;
                    // FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 1;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colcount].Tag = sqldrseat["seattype"].ToString();
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colcount].Text = sqldrseat["textval"].ToString();
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colcount].Font.Bold = true;
                }
                dc = dc + ccountcheck;
                colcount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colcount].Text = "Dayscholar";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colcount].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colcount + 1].Text = "Hostler";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colcount + 1].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colcount + 2].Text = "Boys";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colcount + 2].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colcount + 3].Text = "Girls";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colcount + 3].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                int mancolcount = colcount + 6;
                FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 1;
                int startrowcnt = FpExternal.Sheets[0].RowCount - 1;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7].Text = "No Of Students";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7].Font.Bold = true;
                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7].Text = "No Of Pass";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7].Font.Bold = true;
                FpExternal.Sheets[0].RowCount++;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7].Text = "% Of Pass";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7].Font.Bold = true;
                int ccs = ccountcheck;
                for (int man = 8; man < ccountcheck + 9; man++)  //modified by Mullai
                {
                    string sec = string.Empty;
                    string quotaval = string.Empty;
                    string dayscholar = "Day Scholar";
                    string hostler = "Hostler";
                    string girls = "1";
                    string boys = "0";
                    if (ddlSec.Enabled == true)
                    {
                        sec = ddlSec.SelectedItem.Text;
                    }
                    if (man <= dc)
                    {
                        quotaval = FpExternal.Sheets[0].Cells[checkrcou, man].Tag.ToString();
                    }
                    con.Close();
                    con.Open();
                    string beforeorafter = string.Empty;
                    if (rbbeforeandafterrevaluation.SelectedValue == "1")
                    {
                        beforeorafter = "1";
                    }
                    else
                    {
                        beforeorafter = string.Empty;
                    }
                    if (sec.Trim().ToLower() == "all")
                    {
                        sec = string.Empty;
                    }
                    SqlCommand studinfo = new SqlCommand("universitymarkresultanalysis", con);
                    studinfo.CommandType = CommandType.StoredProcedure;
                    studinfo.Parameters.AddWithValue("@degreecode", degree_code);
                    studinfo.Parameters.AddWithValue("@batchyear", batch_year);
                    studinfo.Parameters.AddWithValue("@semester", current_sem);
                    studinfo.Parameters.AddWithValue("@examcode", ExamCode);
                    studinfo.Parameters.AddWithValue("@sections", sec);
                    studinfo.Parameters.AddWithValue("@dayscholar", dayscholar);
                    studinfo.Parameters.AddWithValue("@hostler", hostler);
                    studinfo.Parameters.AddWithValue("@quota", quotaval);
                    studinfo.Parameters.AddWithValue("@girls", girls);
                    studinfo.Parameters.AddWithValue("@boys", boys);
                    //gmintotal
                    //@beforeorafter
                    studinfo.Parameters.AddWithValue("@mintot", gmintotal - 1);
                    studinfo.Parameters.AddWithValue("@beforeorafter", beforeorafter);
                    studinfo.Parameters.AddWithValue("@markorgrade", stgetgradeflag);
                    studinfo.Parameters.AddWithValue("@minintmark", gminintmark);
                    studinfo.Parameters.AddWithValue("@minextmark", gmaxintmark);
                    SqlDataAdapter studinfodata = new SqlDataAdapter(studinfo);
                    DataSet dsstudinfodata = new DataSet();
                    studinfodata.Fill(dsstudinfodata);
                    if (dsstudinfodata.Tables[0].Rows.Count > 0)
                    {
                        string allpassper = string.Empty;
                        if (quotaval != "")
                        {
                            int totcount = Convert.ToInt16(dsstudinfodata.Tables[0].Rows[0][0].ToString());
                            int totpasscount = Convert.ToInt16(dsstudinfodata.Tables[1].Rows[0][0].ToString());
                            double passpercent1 = 0;
                            passpercent1 = Convert.ToDouble((Convert.ToDouble(totpasscount) / totcount) * 100);
                            double passpercent2 = Math.Round(passpercent1, 2);
                            allpassper = Convert.ToString(passpercent2);
                            FpExternal.Sheets[0].Cells[checkrcou + 1, man].Text = totcount.ToString();
                            FpExternal.Sheets[0].Cells[checkrcou + 1, man].HorizontalAlign = HorizontalAlign.Center;
                            FpExternal.Sheets[0].Cells[checkrcou + 2, man].Text = totpasscount.ToString();
                            FpExternal.Sheets[0].Cells[checkrcou + 2, man].HorizontalAlign = HorizontalAlign.Center;
                            FpExternal.Sheets[0].Cells[checkrcou + 3, man].Text = allpassper.ToString();
                            FpExternal.Sheets[0].Cells[checkrcou + 3, man].HorizontalAlign = HorizontalAlign.Center;
                        }
                        else
                        {
                            int dayscount = 0;
                            int dayscholarpasscount = 0;
                            string dayscholarperc = string.Empty;
                            int hostlercount = 0;
                            int hostlerpasscount = 0;
                            string hostlerperc = string.Empty;
                            int girlscount = 0;
                            int girlspasscount = 0;
                            string girlsperc = string.Empty;
                            int boyscount = 0;
                            int boyspasscount = 0;
                            string boysperc = string.Empty;
                            dayscount = Convert.ToInt16(dsstudinfodata.Tables[0].Rows[0][0].ToString());
                            dayscholarpasscount = Convert.ToInt16(dsstudinfodata.Tables[1].Rows[0][0].ToString());
                            if (dayscount != 0)
                            {
                                double passpercent1 = 0;
                                passpercent1 = Convert.ToDouble((Convert.ToDouble(dayscholarpasscount) / dayscount) * 100);
                                double passpercent2 = Math.Round(passpercent1, 2);
                                dayscholarperc = Convert.ToString(passpercent2);
                            }
                            else
                            {
                                dayscholarperc = "0";
                            }
                            FpExternal.Sheets[0].Cells[checkrcou + 1, man].Text = dayscount.ToString();
                            FpExternal.Sheets[0].Cells[checkrcou + 1, man].HorizontalAlign = HorizontalAlign.Center;
                            FpExternal.Sheets[0].Cells[checkrcou + 2, man].Text = dayscholarpasscount.ToString();
                            FpExternal.Sheets[0].Cells[checkrcou + 2, man].HorizontalAlign = HorizontalAlign.Center;
                            FpExternal.Sheets[0].Cells[checkrcou + 3, man].Text = dayscholarperc.ToString();
                            FpExternal.Sheets[0].Cells[checkrcou + 3, man].HorizontalAlign = HorizontalAlign.Center;
                            hostlercount = Convert.ToInt16(dsstudinfodata.Tables[2].Rows[0][0].ToString());
                            hostlerpasscount = Convert.ToInt16(dsstudinfodata.Tables[3].Rows[0][0].ToString());
                            double passpercenthos = 0;
                            if (hostlercount != 0)
                            {
                                passpercenthos = Convert.ToDouble((Convert.ToDouble(hostlerpasscount) / hostlercount) * 100);
                                double passpercent2hos = Math.Round(passpercenthos, 2);
                                hostlerperc = Convert.ToString(passpercent2hos);
                            }
                            else
                            {
                                hostlerperc = "0";
                            }
                            FpExternal.Sheets[0].Cells[checkrcou + 1, man + 1].Text = hostlercount.ToString();
                            FpExternal.Sheets[0].Cells[checkrcou + 1, man + 1].HorizontalAlign = HorizontalAlign.Center;
                            FpExternal.Sheets[0].Cells[checkrcou + 2, man + 1].Text = hostlerpasscount.ToString();
                            FpExternal.Sheets[0].Cells[checkrcou + 2, man + 1].HorizontalAlign = HorizontalAlign.Center;
                            FpExternal.Sheets[0].Cells[checkrcou + 3, man + 1].Text = hostlerperc.ToString();
                            FpExternal.Sheets[0].Cells[checkrcou + 3, man + 1].HorizontalAlign = HorizontalAlign.Center;
                            girlscount = Convert.ToInt16(dsstudinfodata.Tables[4].Rows[0][0].ToString());
                            girlspasscount = Convert.ToInt16(dsstudinfodata.Tables[5].Rows[0][0].ToString());
                            if (girlscount != 0)
                            {
                                double passpercentgirls = 0;
                                passpercentgirls = Convert.ToDouble((Convert.ToDouble(girlspasscount) / girlscount) * 100);
                                double passpercent2gir = Math.Round(passpercentgirls, 2);
                                girlsperc = Convert.ToString(passpercent2gir);
                            }
                            else
                            {
                                girlsperc = "0";
                            }
                            FpExternal.Sheets[0].Cells[checkrcou + 1, man + 3].Text = girlscount.ToString();
                            FpExternal.Sheets[0].Cells[checkrcou + 1, man + 3].HorizontalAlign = HorizontalAlign.Center;
                            FpExternal.Sheets[0].Cells[checkrcou + 2, man + 3].Text = girlspasscount.ToString();
                            FpExternal.Sheets[0].Cells[checkrcou + 2, man + 3].HorizontalAlign = HorizontalAlign.Center;
                            FpExternal.Sheets[0].Cells[checkrcou + 3, man + 3].Text = girlsperc.ToString();
                            FpExternal.Sheets[0].Cells[checkrcou + 3, man + 3].HorizontalAlign = HorizontalAlign.Center;
                            boyscount = Convert.ToInt16(dsstudinfodata.Tables[6].Rows[0][0].ToString());
                            boyspasscount = Convert.ToInt16(dsstudinfodata.Tables[7].Rows[0][0].ToString());
                            if (boyscount != 0)
                            {
                                double passpercentboys = 0;
                                passpercentboys = Convert.ToDouble((Convert.ToDouble(boyspasscount) / boyscount) * 100);
                                double passpercent2boys = Math.Round(passpercentboys, 2);
                                boysperc = Convert.ToString(passpercent2boys);
                            }
                            else
                            {
                                boysperc = "0";
                            }
                            FpExternal.Sheets[0].Cells[checkrcou + 1, man + 2].Text = boyscount.ToString();
                            FpExternal.Sheets[0].Cells[checkrcou + 1, man + 2].HorizontalAlign = HorizontalAlign.Center;
                            FpExternal.Sheets[0].Cells[checkrcou + 2, man + 2].Text = boyspasscount.ToString();
                            FpExternal.Sheets[0].Cells[checkrcou + 2, man + 2].HorizontalAlign = HorizontalAlign.Center;
                            FpExternal.Sheets[0].Cells[checkrcou + 3, man + 2].Text = boysperc.ToString();
                            FpExternal.Sheets[0].Cells[checkrcou + 3, man + 2].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
                FpExternal.Sheets[0].SpanModel.Add(checkrcou, 0, 4, 7);
                FpExternal.Sheets[0].SpanModel.Add(checkrcou, cf + 12, 4, FpExternal.Sheets[0].ColumnCount);
                int h = FpExternal.Sheets[0].RowCount;
                //FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 3;
                //FpExternal.Sheets[0].SpanModel.Add(h, 0, 2, FpExternal.Sheets[0].ColumnCount);
                //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Text = "Class Advisor";
                //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9].Text = "HOD";
                //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9].Font.Bold = true;
                //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 6);
                //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 7, 1, 2);
                //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 10, 1, FpExternal.Sheets[0].ColumnCount);
            }
            if (FpExternal.Sheets[0].RowCount > 0)
            {
                lblerrormsg.Text = " ";
                lblerrormsg.Visible = false;
                FpExternal.Sheets[0].PageSize = FpExternal.Sheets[0].Rows.Count;
                FpExternal.Visible = true;
                btnExcel.Visible = true;
                txtexcelname.Visible = true;
                lblrptname.Visible = true;
                btnprintmaster.Visible = true;
            }
            else
            {
                lblerrormsg.Text = "No Records Found";
                lblerrormsg.Visible = true;
                FpExternal.Visible = false;
                btnExcel.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                btnprintmaster.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public string Calulat_GPA_Semwise(string RollNo, string degree_code, string batch_year, string exam_month, string exam_year, string collegecode)
    {
        connection connection = new connection();
        string ccva = string.Empty;
        string strgrade = string.Empty;
        double creditval = 0;
        double finalgpa1 = 0;
        double creditsum1 = 0;
        double gpacal1 = 0;
        string strsubcrd = string.Empty;
        string examcodeval = string.Empty;
        double strtot = 0;
        double strgradetempfrm = 0;
        double strgradetempto = 0;
        string strtotgrac = string.Empty;
        string strgradetempgrade = string.Empty;
        string syll_code = string.Empty;
        DataSet dggradetot = new DataSet();
        try
        {
            dggradetot.Dispose();
            DataSet daload = new DataSet();
            SqlDataAdapter adaload = new SqlDataAdapter();
            daload.Reset();
            string strsqlstaffname = "select distinct frange,trange,credit_points,mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + collegecode + "";
            cmd = new SqlCommand(strsqlstaffname);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(dggradetot);
        }
        catch (SqlException qle)
        {
            throw qle;
        }
        finally
        {
            connection.Close();
        }
        //dggradetot = GetFunctionv("select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + collegecode + "");
        syll_code = GetFunction("select distinct syll_code from exam_details e,syllabus_master s where e.degree_code=s.degree_code and e.batch_year=s.batch_year and e.current_semester=s.semester and e.degree_code='" + degree_code + "' and e.batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
        ccva = GetFunction("select cc from registration where roll_no='" + RollNo + "'");
        if (ccva == "False")
        {
            strsubcrd = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and Exam_Code = '" + ExamCode + "' ";  //modified by mullai
        }
        else if (ccva == "True")
        {
            strsubcrd = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and Exam_Code = '" + ExamCode + "' ";  //modified by mullai
        }
        if (strsubcrd != "" && strsubcrd != null)
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
                    if ((dr_subcrd["total"].ToString() != string.Empty) && (dr_subcrd["total"].ToString() != "0"))
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
                                if (strgradetempgrade.ToString().Trim() == strtotgrac.ToString().Trim())
                                {
                                    strgrade = gratemp["credit_points"].ToString();
                                    break;
                                }
                            }
                        }
                    }
                    if (strgrade != "" && strgrade != null)
                    {
                        if (dr_subcrd["credit_points"].ToString() != null && dr_subcrd["credit_points"].ToString() != "")
                        {
                            creditval = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                            if (creditsum1 == 0)
                            {
                                creditsum1 = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                            }
                            else
                            {
                                creditsum1 = creditsum1 + Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                            }
                        }
                        if (gpacal1 == 0)
                        {
                            gpacal1 = Convert.ToDouble(strgrade) * creditval;
                        }
                        else
                        {
                            gpacal1 = gpacal1 + (Convert.ToDouble(strgrade) * creditval);
                        }
                    }
                }
            }
        }
        if (creditsum1 != 0)
        {
            finalgpa1 = Math.Round((gpacal1 / creditsum1), 2, MidpointRounding.AwayFromZero);
        }
        gsum = Convert.ToString(gpacal1);
        return finalgpa1.ToString();
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = 3;
        string filt_details = string.Empty;
        string sec_details = string.Empty;
        string strsec = string.Empty;
        if (ddlSec.Enabled == true)
        {
            strsec = " Sec: " + ddlSec.SelectedItem.Text.ToString();
        }
        filt_details = "Batch: " + ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString();
        sec_details = "Sem :" + ddlSemYr.SelectedItem.ToString() + "-" + strsec;
        string date_filt = "From :" + txtfrm_date.Text + "-" + "To :" + txtend_date.Text;
        string degreedetails = string.Empty;
        if (chkonlyrevaluation.Checked == false)
        {
            degreedetails = "Result Analysis Report" + "@" + filt_details + "@" + sec_details + "@" + date_filt;
        }
        else if (chkonlyrevaluation.Checked == true)
        {
            date_filt = string.Empty;
            degreedetails = "Office of the Controller Of Examinations" + "@ Revaluation Results -" + ddlMonth.SelectedItem.Text + ddlYear.SelectedItem.Text + "@" + filt_details + "@" + sec_details + "@" + date_filt;
        }
        string pagename = "newuniversityresultanalysis.aspx";
        Printcontrol.loadspreaddetails(FpExternal, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string print = string.Empty;
            string appPath = HttpContext.Current.Server.MapPath("~");
            string strexcelname = string.Empty;
            if (appPath != "")
            {
                strexcelname = txtexcelname.Text;
                appPath = appPath.Replace("\\", "/");
                if (strexcelname != "")
                {
                    print = strexcelname;
                    string szPath = appPath + "/Report/";
                    string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")
                    FpExternal.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
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

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlMonth.SelectedItem.Text == "Select")
            {
                FpExternal.Visible = false;
                btnExcel.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                btnprintmaster.Visible = false;
                lblerrormsg.Text = "Kindly select Exam Month";
                lblerrormsg.Visible = true;
                return;
            }
            if (ddlYear.SelectedItem.Text == "Select")
            {
                FpExternal.Visible = false;
                btnExcel.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                btnprintmaster.Visible = false;
                lblerrormsg.Text = "Kindly select Exam year";
                lblerrormsg.Visible = true;
                return;
            }
            if (rbbeforeandafterrevaluation.SelectedValue == "2" && chkonlyrevaluation.Checked == true)
            {
                degree_code = ddlBranch.SelectedValue.ToString();
                current_sem = ddlSemYr.SelectedValue.ToString();
                batch_year = ddlBatch.SelectedValue.ToString();
                exam_month = ddlMonth.SelectedValue.ToString();
                exam_year = ddlYear.SelectedItem.ToString();
                bindindnewspread();
            }
            else
            {
                DateTime frmmdate;
                DateTime toodate;
                string[] split_fDate = txtfrm_date.Text.Split(new char[] { '/' });
                string set_date_from = split_fDate[1] + '/' + split_fDate[0] + '/' + split_fDate[2];
                string[] split_tDate = txtend_date.Text.Split(new char[] { '/' });
                string set_date_to = split_tDate[1] + '/' + split_tDate[0] + '/' + split_tDate[2];
                frmmdate = Convert.ToDateTime(set_date_from);
                toodate = Convert.ToDateTime(set_date_to);
                if (frmmdate > toodate)
                {
                    FpExternal.Visible = false;
                    btnExcel.Visible = false;
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    btnprintmaster.Visible = false;
                    lblerrormsg.Text = "To date must be greater then from date";
                    lblerrormsg.Visible = true;
                    return;
                }
                lblerrormsg.Visible = false;
                lblerrormsg.Text = string.Empty;
                degree_code = ddlBranch.SelectedValue.ToString();
                current_sem = ddlSemYr.SelectedValue.ToString();
                FpExternal.Sheets[0].ColumnCount = 0;
                batch_year = ddlBatch.SelectedValue.ToString();
                FpExternal.Sheets[0].ColumnCount = 7;//'------------------new
                FpExternal.Sheets[0].RowCount = 0;
                FpExternal.Sheets[0].Columns[0].Width = 50;//===changed from 50 to 100 29.06.12
                FpExternal.Sheets[0].Columns[3].Width = 150;
                FpExternal.Sheets[0].Columns[4].Width = 250;//=====changed 05.07.12
                FpExternal.Sheets[0].Columns[2].Width = 100;
                FpExternal.Sheets[0].Columns[2].Locked = true;
                FpExternal.Sheets[0].Columns[3].Locked = true;
                FpExternal.Sheets[0].Columns[4].Locked = true;
                FpExternal.Sheets[0].AutoPostBack = true;
                FpExternal.Sheets[0].ColumnHeader.RowCount = 3;
                FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                FpExternal.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.AliceBlue;
                FpExternal.Sheets[0].ColumnHeader.Rows[1].BackColor = Color.AliceBlue;
                FpExternal.Sheets[0].ColumnHeader.Rows[2].BackColor = Color.AliceBlue;
                FpExternal.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                FpExternal.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                FpExternal.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpExternal.Sheets[0].DefaultStyle.Font.Bold = false;
                FpExternal.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].RowHeader.Visible = false;
                FpExternal.Visible = true;
                btnExcel.Visible = true;
                txtexcelname.Visible = true;
                lblrptname.Visible = true;
                btnprintmaster.Visible = true;
                External_Students();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void convertaftergrade(string roll, string subj)
    {
        try
        {
            string strexam = string.Empty;
            strexam = "Select subject_name,subject_code,total,actual_total,result,cp,m.grade,m.subject_no,s.max_ext_marks,s.max_int_marks,s.maxtotal,s.mintotal,s.min_int_marks,s.min_ext_marks,ss.lab,m.Actual_Grade from Mark_Entry m,Subject s,sub_sem ss where m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.Exam_Code = '" + IntExamCode + "'  and m.roll_no='" + roll + "' and s.subject_no='" + subj + "'";
            SqlCommand cmd_exam1 = new SqlCommand(strexam, con);
            con.Close();
            con.Open();
            SqlDataReader dr_convert;
            dr_convert = cmd_exam1.ExecuteReader();
            if (dr_convert.Read())
            {
                funcsubname = dr_convert["subject_name"].ToString();
                funcsubno = dr_convert["subject_no"].ToString();
                funcsubcode = dr_convert["subject_code"].ToString();
                funcresult = dr_convert["result"].ToString();
                funccredit = dr_convert["cp"].ToString();
                mark = dr_convert["actual_total"].ToString();
                previousgrade = string.Empty;
                string strgrade = string.Empty;
                string lab = Convert.ToString(dr_convert["lab"]).Trim();
                string subjectGrade = Convert.ToString(dr_convert["Actual_Grade"]).Trim();
                double minextmark = 0;
                double totalMark = 0;
                double maxExternal = 0;
                double mintotal = 0;
                double maxtotal = 0;
                double minInternal = 0;
                double maxInternal = 0;

                double.TryParse(Convert.ToString(dr_convert["max_ext_marks"]), out maxExternal);
                double.TryParse(Convert.ToString(dr_convert["max_int_marks"]), out maxInternal);
                double.TryParse(Convert.ToString(dr_convert["maxtotal"]), out maxtotal);

                double.TryParse(Convert.ToString(dr_convert["min_ext_marks"]), out minextmark);
                double.TryParse(Convert.ToString(dr_convert["min_int_marks"]), out minInternal);
                double.TryParse(Convert.ToString(dr_convert["mintotal"]), out mintotal);
                double.TryParse(Convert.ToString(mark), out totalMark);
                //if (dr_convert["total"].ToString() != string.Empty)
                double checkmintotal = 0;
                if (chk_subjectwisegrade.Checked)
                {
                    bool failgrade = false;
                    int subjectAttempt = dirAcc.selectScalarInt(" select COUNT(distinct ed.exam_code) as attempts from mark_entry m,Exam_Details ed,subject s where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and s.subject_code in('" + funcsubcode + "') and m.roll_no='" + roll + "'");

                    string equalsub = string.Empty;
                    string strsuboquery = "select subject_no from tbl_equal_subject_Grade_System where Common_Subject_no in(select Common_Subject_no from tbl_equal_subject_Grade_System where Subject_no='" + funcsubno + "')";
                    DataSet dsequlsub = dirAcc.selectDataSet(strsuboquery);
                    if (dsequlsub.Tables.Count > 0 && dsequlsub.Tables[0].Rows.Count > 0)
                    {
                        for (int es = 0; es < dsequlsub.Tables[0].Rows.Count; es++)
                        {
                            string subjectNo = Convert.ToString(dsequlsub.Tables[0].Rows[es]["subject_no"]).Trim();
                            if (equalsub.Trim() != "")
                            {
                                equalsub = equalsub + ",'" + subjectNo + "'";
                            }
                            else
                            {
                                equalsub = "'" + subjectNo + "'";
                            }
                        }
                    }
                    if (equalsub.Trim() == "")
                    {
                        equalsub = "'" + funcsubno + "'";
                    }

                    string qry = " select * from SubWiseGrdeMaster where Exam_Year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' and Exam_Month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and SubjectCode in(" + equalsub + ") order by Frange desc";
                    DataTable dtSubWiseGrade = new DataTable();
                    dtSubWiseGrade = dirAcc.selectDataTable(qry);
                    if (dtSubWiseGrade.Rows.Count == 0)
                    {
                        if (subjectAttempt > 1 && dtSubWiseGrade.Rows.Count == 0)
                        {
                            qry = " select *,(Exam_Year*12+Exam_Month) exmonval from SubWiseGrdeMaster where SubjectName='" + funcsubname + "' and (Exam_Year*12+Exam_Month)<('" + Convert.ToString(ddlYear.SelectedValue).Trim() + "'*12+'" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "') order by exmonval desc,Frange desc";
                        }
                    }
                    dtSubWiseGrade = new DataTable();
                    dtSubWiseGrade = dirAcc.selectDataTable(qry);
                    dtSubWiseGrade.DefaultView.RowFilter = "grade='B'";
                    DataView dvSubWisegrade = dtSubWiseGrade.DefaultView;
                    if (dvSubWisegrade.Count > 0)
                    {
                        if (subjectAttempt > 1)
                        {
                            minextmark = 0;// Convert.ToDouble(dvgrade[0]["trange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out minextmark);
                            minextmark = (minextmark * maxExternal) / 100;
                            mintotal = 0;// Convert.ToDouble(dvgrade[0]["trange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out mintotal);
                        }
                        else
                        {
                            minextmark = 0;//Convert.ToDouble(dvgrade[0]["frange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out minextmark);
                            minextmark = (minextmark * maxExternal) / 100;
                            mintotal = 0;//Convert.ToDouble(dvgrade[0]["frange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out mintotal);
                        }
                    }
                    if (mintotal > 50)
                    {
                        mintotal = 50;
                    }
                    if (lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                    {
                        if (totalMark < 50)
                        {
                            failgrade = true;
                        }
                    }
                    else
                    {
                        if (totalMark < mintotal)
                        {
                            failgrade = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(subjectGrade))
                    {
                        previousgrade = subjectGrade;
                    }
                    else
                    {
                        if (failgrade == false)
                        {
                            dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + totalMark + "' and Trange >='" + totalMark + "'";// "Frange<='" + Reval_Tot + "'"; 
                            if (subjectAttempt > 1)
                            {
                                dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + totalMark + "' and Trange >='" + totalMark + "'";// "Frange<='" + Reval_Tot + "'";
                            }
                            dvSubWisegrade = dtSubWiseGrade.DefaultView;
                            dvSubWisegrade.Sort = "Frange asc";
                            if (dvSubWisegrade.Count > 0)
                            {
                                previousgrade = Convert.ToString(dvSubWisegrade[0]["Grade"]).Trim();
                            }
                            else
                            {
                                previousgrade = "B";
                            }
                        }
                        else
                        {
                            previousgrade = "RA";
                        }
                    }
                    //checkmintotal = mintotal - 1;
                    //failgrade = "Ra";
                    //if (failgrade.Trim().ToLower() == actualgrade.Trim().ToLower() && actualgrade.Trim().ToLower() != grades.Trim().ToLower())
                    //{
                    //    status = "PASS";
                    //}
                    //else
                    //{
                    //    status = "GC";
                    //}
                }
                else
                {
                    if (mark != string.Empty)
                    {
                        strgrade = "select mark_grade from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and college_code='" + Session["collegecode"] + "' and '" + mark + "' between frange and trange";
                    }
                    else
                    {
                        strgrade = "select mark_grade from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and college_code='" + Session["collegecode"] + "' and credit_points between frange and trange";
                    }
                    SqlCommand cmd_grade = new SqlCommand(strgrade, con);
                    con.Close();
                    con.Open();
                    SqlDataReader dr_grade;
                    dr_grade = cmd_grade.ExecuteReader();
                    if (dr_grade.HasRows == true)
                    {
                        while (dr_grade.Read())
                        {
                            previousgrade = dr_grade["mark_grade"].ToString();
                        }
                    }
                    else
                    {
                        previousgrade = string.Empty;
                    }
                }
            }
        }
        catch
        {
        }
    }

    public void convertgrade(string roll, string subj)
    {
        try
        {
            string strexam = string.Empty;
            strexam = "Select subject_name,subject_code,total,actual_total,result,cp,m.subject_no,s.max_ext_marks,s.max_int_marks,s.maxtotal,s.mintotal,s.min_int_marks,s.min_ext_marks,ss.lab,m.grade from Mark_Entry m,Subject s,sub_sem ss where m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and m.Exam_Code = '" + IntExamCode + "'  and m.roll_no='" + roll + "' and s.subject_no='" + subj + "'";
            //strexam = "Select subject_name,subject_code,total,actual_total,result,cp,m.subject_no,s.max_ext_marks,s.max_int_marks,s.maxtotal,s.mintotal,s.min_int_marks,s.min_ext_marks,ss.lab from Mark_Entry m,Subject ss,sub_sem ss where m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.Exam_Code = '" + IntExamCode + "'  and m.roll_no='" + roll + "' and s.subject_no='" + subj + "'";
            SqlCommand cmd_exam1 = new SqlCommand(strexam, con);
            con.Close();
            con.Open();
            SqlDataReader dr_convert;
            dr_convert = cmd_exam1.ExecuteReader();
            //while (dr_convert.Read())
            if (dr_convert.Read())
            {
                //   funcsemester = dr_convert["semester"].ToString();
                funcsubname = dr_convert["subject_name"].ToString();
                funcsubno = dr_convert["subject_no"].ToString();
                funcsubcode = dr_convert["subject_code"].ToString();
                funcresult = dr_convert["result"].ToString();
                funccredit = dr_convert["cp"].ToString();
                string lab = Convert.ToString(dr_convert["lab"]).Trim();
                string subjectGrade = Convert.ToString(dr_convert["grade"]).Trim();
                double minextmark = 0;
                double totalMark = 0;
                double maxExternal = 0;
                double mintotal = 0;
                double maxtotal = 0;
                double minInternal = 0;
                double maxInternal = 0;

                double.TryParse(Convert.ToString(dr_convert["max_ext_marks"]), out maxExternal);
                double.TryParse(Convert.ToString(dr_convert["max_int_marks"]), out maxInternal);
                double.TryParse(Convert.ToString(dr_convert["maxtotal"]), out maxtotal);

                double.TryParse(Convert.ToString(dr_convert["min_ext_marks"]), out minextmark);
                double.TryParse(Convert.ToString(dr_convert["min_int_marks"]), out minInternal);
                double.TryParse(Convert.ToString(dr_convert["mintotal"]), out mintotal);

                if (rbbeforeandafterrevaluation.SelectedValue == "1")
                {
                    mark = dr_convert["actual_total"].ToString();
                }
                else if (rbbeforeandafterrevaluation.SelectedValue == "2")
                {
                    mark = dr_convert["total"].ToString();
                }
                double.TryParse(Convert.ToString(mark), out totalMark);
                funcgrade = string.Empty;
                string strgrade = string.Empty;
                //if (dr_convert["total"].ToString() != string.Empty)
                double checkmintotal = 0;
                if (chk_subjectwisegrade.Checked)
                {
                    bool failgrade = false;
                    int subjectAttempt = dirAcc.selectScalarInt(" select COUNT(distinct ed.exam_code) as attempts from mark_entry m,Exam_Details ed,subject s where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and s.subject_code in('" + funcsubcode + "') and m.roll_no='" + roll + "'");

                    string equalsub = string.Empty;
                    string strsuboquery = "select subject_no from tbl_equal_subject_Grade_System where Common_Subject_no in(select Common_Subject_no from tbl_equal_subject_Grade_System where Subject_no='" + funcsubno + "')";
                    DataSet dsequlsub = dirAcc.selectDataSet(strsuboquery);
                    if (dsequlsub.Tables.Count > 0 && dsequlsub.Tables[0].Rows.Count > 0)
                    {
                        for (int es = 0; es < dsequlsub.Tables[0].Rows.Count; es++)
                        {
                            string subjectNo = Convert.ToString(dsequlsub.Tables[0].Rows[es]["subject_no"]).Trim();
                            if (equalsub.Trim() != "")
                            {
                                equalsub = equalsub + ",'" + subjectNo + "'";
                            }
                            else
                            {
                                equalsub = "'" + subjectNo + "'";
                            }
                        }
                    }
                    if (equalsub.Trim() == "")
                    {
                        equalsub = "'" + funcsubno + "'";
                    }

                    string qry = " select * from SubWiseGrdeMaster where Exam_Year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' and Exam_Month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and SubjectCode in(" + equalsub + ") order by Frange desc";
                    DataTable dtSubWiseGrade = new DataTable();
                    dtSubWiseGrade = dirAcc.selectDataTable(qry);
                    if (dtSubWiseGrade.Rows.Count == 0)
                    {
                        if (subjectAttempt > 1 && dtSubWiseGrade.Rows.Count == 0)
                        {
                            qry = " select *,(Exam_Year*12+Exam_Month) exmonval from SubWiseGrdeMaster where SubjectName='" + funcsubname + "' and (Exam_Year*12+Exam_Month)<('" + Convert.ToString(ddlYear.SelectedValue).Trim() + "'*12+'" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "') order by exmonval desc,Frange desc";
                        }
                    }
                    dtSubWiseGrade = new DataTable();
                    dtSubWiseGrade = dirAcc.selectDataTable(qry);
                    dtSubWiseGrade.DefaultView.RowFilter = "grade='B'";
                    DataView dvSubWisegrade = dtSubWiseGrade.DefaultView;
                    if (dvSubWisegrade.Count > 0)
                    {
                        if (subjectAttempt > 1)
                        {
                            minextmark = 0;// Convert.ToDouble(dvgrade[0]["trange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out minextmark);
                            minextmark = (minextmark * maxExternal) / 100;
                            mintotal = 0;// Convert.ToDouble(dvgrade[0]["trange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out mintotal);
                        }
                        else
                        {
                            minextmark = 0;//Convert.ToDouble(dvgrade[0]["frange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out minextmark);
                            minextmark = (minextmark * maxExternal) / 100;
                            mintotal = 0;//Convert.ToDouble(dvgrade[0]["frange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out mintotal);
                        }
                    }
                    if (mintotal > 50)
                    {
                        mintotal = 50;
                    }
                    if (lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                    {
                        if (totalMark < 50)
                        {
                            failgrade = true;
                        }
                    }
                    else
                    {
                        if (totalMark < mintotal)
                        {
                            failgrade = true;
                        }
                    }
                    if (failgrade == false)
                    {
                        dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + totalMark + "' and Trange >='" + totalMark + "'";// "Frange<='" + Reval_Tot + "'"; 
                        if (subjectAttempt > 1)
                        {
                            dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + totalMark + "' and Trange >='" + totalMark + "'";// "Frange<='" + Reval_Tot + "'";
                        }
                        dvSubWisegrade = dtSubWiseGrade.DefaultView;
                        dvSubWisegrade.Sort = "Frange asc";
                        if (dvSubWisegrade.Count > 0)
                        {
                            funcgrade = Convert.ToString(dvSubWisegrade[0]["Grade"]).Trim();
                        }
                        else
                        {
                            funcgrade = "B";
                        }
                    }
                    else
                    {
                        funcgrade = "RA";
                    }
                    if (!string.IsNullOrEmpty(subjectGrade))
                    {
                        funcgrade = subjectGrade;
                    }
                    //checkmintotal = mintotal - 1;
                    //failgrade = "Ra";
                    //if (failgrade.Trim().ToLower() == actualgrade.Trim().ToLower() && actualgrade.Trim().ToLower() != grades.Trim().ToLower())
                    //{
                    //    status = "PASS";
                    //}
                    //else
                    //{
                    //    status = "GC";
                    //}
                }
                else
                {
                    if (mark != string.Empty)
                    {
                        strgrade = "select mark_grade from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and college_code='" + Session["collegecode"] + "' and '" + mark + "' between frange and trange";
                    }
                    else
                    {
                        strgrade = "select mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code='" + Session["collegecode"] + "' and credit_points between frange and trange";
                    }
                    SqlCommand cmd_grade = new SqlCommand(strgrade, con);
                    con.Close();
                    con.Open();
                    SqlDataReader dr_grade;
                    dr_grade = cmd_grade.ExecuteReader();
                    if (dr_grade.HasRows == true)
                    {
                        while (dr_grade.Read())
                        {
                            funcgrade = dr_grade["mark_grade"].ToString();
                        }
                    }
                    else
                    {
                        funcgrade = string.Empty;
                    }
                }
            }
        }
        catch
        {
        }
    }

    public void convertaftergradev(string roll, string subj, int maxmarkve, int attmptreal)
    {
        try
        {
            SqlDataReader dr_convert;
            string strexam = "Select subject_name,subject_code,internal_mark,external_mark,actual_internal_mark,actual_external_mark,attempts,actual_total,total,result,cp,m.grade,m.subject_no,s.max_ext_marks,s.max_int_marks,s.maxtotal,s.mintotal,s.min_int_marks,s.min_ext_marks,ss.lab from Mark_Entry m,Subject s,sub_sem ss where m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and m.Exam_Code = '" + IntExamCode + "'  and m.roll_no='" + roll + "' and s.subject_no='" + subj + "'";

            double inte = 0, exte = 0;
            int attmpt = 0;
            SqlCommand cmd_exam1 = new SqlCommand(strexam, con_convertgrade);
            con_convertgrade.Close();
            con_convertgrade.Open();
            dr_convert = cmd_exam1.ExecuteReader();
            while (dr_convert.Read())
            {
                //   funcsemester = dr_convert["semester"].ToString();
                funcsubname = dr_convert["subject_name"].ToString();
                funcsubno = dr_convert["subject_no"].ToString();
                funcsubcode = dr_convert["subject_code"].ToString();
                funcresult = dr_convert["result"].ToString();
                funccredit = dr_convert["cp"].ToString();
                mark = dr_convert["actual_total"].ToString();
                inte = Convert.ToDouble(dr_convert["actual_internal_mark"].ToString());
                exte = Convert.ToDouble(dr_convert["actual_external_mark"].ToString());
                attmpt = Convert.ToInt32(dr_convert["attempts"].ToString());
                funcgrade = string.Empty;
                string strgrade = string.Empty;
                string lab = Convert.ToString(dr_convert["lab"]).Trim();
                string subjectGrade = Convert.ToString(dr_convert["grade"]).Trim();
                double minextmark = 0;
                double totalMark = 0;
                double maxExternal = 0;
                double mintotal = 0;
                double maxtotal = 0;
                double minInternal = 0;
                double maxInternal = 0;

                double.TryParse(Convert.ToString(dr_convert["max_ext_marks"]), out maxExternal);
                double.TryParse(Convert.ToString(dr_convert["max_int_marks"]), out maxInternal);
                double.TryParse(Convert.ToString(dr_convert["maxtotal"]), out maxtotal);

                double.TryParse(Convert.ToString(dr_convert["min_ext_marks"]), out minextmark);
                double.TryParse(Convert.ToString(dr_convert["min_int_marks"]), out minInternal);
                double.TryParse(Convert.ToString(dr_convert["mintotal"]), out mintotal);
                double.TryParse(Convert.ToString(mark), out totalMark);

                double checkmintotal = 0;
                if (chk_subjectwisegrade.Checked)
                {
                    bool failgrade = false;
                    int subjectAttempt = dirAcc.selectScalarInt(" select COUNT(distinct ed.exam_code) as attempts from mark_entry m,Exam_Details ed,subject s where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and s.subject_code in('" + funcsubcode + "') and m.roll_no='" + roll + "'");

                    string equalsub = string.Empty;
                    string strsuboquery = "select subject_no from tbl_equal_subject_Grade_System where Common_Subject_no in(select Common_Subject_no from tbl_equal_subject_Grade_System where Subject_no='" + funcsubno + "')";
                    DataSet dsequlsub = dirAcc.selectDataSet(strsuboquery);
                    if (dsequlsub.Tables.Count > 0 && dsequlsub.Tables[0].Rows.Count > 0)
                    {
                        for (int es = 0; es < dsequlsub.Tables[0].Rows.Count; es++)
                        {
                            string subjectNo = Convert.ToString(dsequlsub.Tables[0].Rows[es]["subject_no"]).Trim();
                            if (equalsub.Trim() != "")
                            {
                                equalsub = equalsub + ",'" + subjectNo + "'";
                            }
                            else
                            {
                                equalsub = "'" + subjectNo + "'";
                            }
                        }
                    }
                    if (equalsub.Trim() == "")
                    {
                        equalsub = "'" + funcsubno + "'";
                    }

                    string qry = " select * from SubWiseGrdeMaster where Exam_Year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' and Exam_Month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and SubjectCode in(" + equalsub + ") order by Frange desc";
                    DataTable dtSubWiseGrade = new DataTable();
                    dtSubWiseGrade = dirAcc.selectDataTable(qry);
                    if (dtSubWiseGrade.Rows.Count == 0)
                    {
                        if (subjectAttempt > 1 && dtSubWiseGrade.Rows.Count == 0)
                        {
                            qry = " select *,(Exam_Year*12+Exam_Month) exmonval from SubWiseGrdeMaster where SubjectName='" + funcsubname + "' and (Exam_Year*12+Exam_Month)<('" + Convert.ToString(ddlYear.SelectedValue).Trim() + "'*12+'" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "') order by exmonval desc,Frange desc";
                        }
                    }
                    dtSubWiseGrade = new DataTable();
                    dtSubWiseGrade = dirAcc.selectDataTable(qry);
                    dtSubWiseGrade.DefaultView.RowFilter = "grade='B'";
                    DataView dvSubWisegrade = dtSubWiseGrade.DefaultView;
                    if (dvSubWisegrade.Count > 0)
                    {
                        if (subjectAttempt > 1)
                        {
                            minextmark = 0;// Convert.ToDouble(dvgrade[0]["trange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out minextmark);
                            minextmark = (minextmark * maxExternal) / 100;
                            mintotal = 0;// Convert.ToDouble(dvgrade[0]["trange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out mintotal);
                        }
                        else
                        {
                            minextmark = 0;//Convert.ToDouble(dvgrade[0]["frange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out minextmark);
                            minextmark = (minextmark * maxExternal) / 100;
                            mintotal = 0;//Convert.ToDouble(dvgrade[0]["frange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out mintotal);
                        }
                    }
                    if (mintotal > 50)
                    {
                        mintotal = 50;
                    }
                    if (lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                    {
                        if (totalMark < 50)
                        {
                            failgrade = true;
                        }
                    }
                    else
                    {
                        if (totalMark < mintotal)
                        {
                            failgrade = true;
                        }
                    }
                    if (failgrade == false)
                    {
                        //dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + totalMark + "' and Trange >='" + totalMark + "'";// "Frange<='" + Reval_Tot + "'"; 
                        //if (subjectAttempt > 1)
                        //{
                        //    dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + totalMark + "' and Trange >='" + totalMark + "'";// "Frange<='" + Reval_Tot + "'";
                        //}
                        if (attmptreal > subjectAttempt)
                        {
                            dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + totalMark + "' and Trange >='" + totalMark + "'";// "Frange<='" + Reval_Tot + "'"; 
                        }
                        else
                        {
                            dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + exte + "' and Trange >='" + exte + "'";// "Frange<='" + Reval_Tot + "'"; 
                        }
                        dvSubWisegrade = dtSubWiseGrade.DefaultView;
                        dvSubWisegrade.Sort = "Frange asc";
                        if (dvSubWisegrade.Count > 0)
                        {
                            previousgrade = Convert.ToString(dvSubWisegrade[0]["Grade"]).Trim();
                        }
                        else
                        {
                            previousgrade = "B";
                        }
                    }
                    else
                    {
                        previousgrade = "RA";
                    }
                    if (!string.IsNullOrEmpty(subjectGrade))
                    {
                        previousgrade = subjectGrade;
                    }
                    //checkmintotal = mintotal - 1;
                    //failgrade = "Ra";
                    //if (failgrade.Trim().ToLower() == actualgrade.Trim().ToLower() && actualgrade.Trim().ToLower() != grades.Trim().ToLower())
                    //{
                    //    status = "PASS";
                    //}
                    //else
                    //{
                    //    status = "GC";
                    //}
                }
                else
                {
                    if (attmptreal > attmpt)
                    {
                        if (dr_convert["total"].ToString() != string.Empty)
                        {
                            strgrade = "select mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and " + dr_convert["actual_total"] + " between frange and trange";
                        }
                        else
                        {
                            strgrade = "select mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and credit_points between frange and trange";
                        }
                    }
                    else
                    {
                        if (dr_convert["total"].ToString() != string.Empty)
                        {
                            strgrade = "select mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and " + exte.ToString() + " between frange and trange";
                        }
                        else
                        {
                            strgrade = "select mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and credit_points between frange and trange";
                        }
                    }
                    SqlCommand cmd_grade = new SqlCommand(strgrade, con_Grade);
                    con_Grade.Close();
                    con_Grade.Open();
                    SqlDataReader dr_grade;
                    dr_grade = cmd_grade.ExecuteReader();
                    if (dr_grade.HasRows == true)
                    {
                        while (dr_grade.Read())
                        {
                            previousgrade = dr_grade["mark_grade"].ToString();
                        }
                    }
                    else
                    {
                        previousgrade = string.Empty;
                    }
                }
            }
        }
        catch
        {

        }
    }

    public void convertgradev(string roll, string subj, int maxmarkve, int attmptreal)
    {
        try
        {
            SqlDataReader dr_convert;
            string strexam = "Select s.subject_name,s.subject_code,m.internal_mark,m.external_mark,m.actual_internal_mark,m.actual_external_mark,m.attempts,m.actual_total,m.total,m.result,m.cp,m.subject_no,s.max_ext_marks,s.max_int_marks,s.maxtotal,s.mintotal,s.min_int_marks,s.min_ext_marks,ss.lab,m.grade from Mark_Entry m,Subject s,sub_sem ss where m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.Exam_Code = '" + IntExamCode + "'  and m.roll_no='" + roll + "' and s.subject_no='" + subj + "'";
            double inte = 0, exte = 0;
            int attmpt = 0;
            SqlCommand cmd_exam1 = new SqlCommand(strexam, con_convertgrade);
            con_convertgrade.Close();
            con_convertgrade.Open();
            dr_convert = cmd_exam1.ExecuteReader();
            while (dr_convert.Read())
            {
                //   funcsemester = dr_convert["semester"].ToString();
                funcsubname = dr_convert["subject_name"].ToString();
                funcsubno = dr_convert["subject_no"].ToString();
                funcsubcode = dr_convert["subject_code"].ToString();
                funcresult = dr_convert["result"].ToString();
                funccredit = dr_convert["cp"].ToString();
                if (rbbeforeandafterrevaluation.SelectedValue == "1")
                {
                    mark = dr_convert["actual_total"].ToString();
                    inte = Convert.ToDouble(dr_convert["actual_internal_mark"].ToString());
                    exte = Convert.ToDouble(dr_convert["actual_external_mark"].ToString());
                    attmpt = Convert.ToInt32(dr_convert["attempts"].ToString());
                }
                else if (rbbeforeandafterrevaluation.SelectedValue == "2")
                {
                    mark = dr_convert["total"].ToString();
                    inte = Convert.ToDouble(dr_convert["internal_mark"].ToString());
                    exte = Convert.ToDouble(dr_convert["external_mark"].ToString());
                    attmpt = Convert.ToInt32(dr_convert["attempts"].ToString());
                }
                funcgrade = string.Empty;
                string strgrade = string.Empty;
                string lab = Convert.ToString(dr_convert["lab"]).Trim();
                string subjectGrade = Convert.ToString(dr_convert["grade"]).Trim();
                double minextmark = 0;
                double totalMark = 0;
                double maxExternal = 0;
                double mintotal = 0;
                double maxtotal = 0;
                double minInternal = 0;
                double maxInternal = 0;

                double.TryParse(Convert.ToString(dr_convert["max_ext_marks"]), out maxExternal);
                double.TryParse(Convert.ToString(dr_convert["max_int_marks"]), out maxInternal);
                double.TryParse(Convert.ToString(dr_convert["maxtotal"]), out maxtotal);

                double.TryParse(Convert.ToString(dr_convert["min_ext_marks"]), out minextmark);
                double.TryParse(Convert.ToString(dr_convert["min_int_marks"]), out minInternal);
                double.TryParse(Convert.ToString(dr_convert["mintotal"]), out mintotal);
                double.TryParse(Convert.ToString(mark), out totalMark);
                double checkmintotal = 0;
                if (chk_subjectwisegrade.Checked)
                {
                    bool failgrade = false;
                    int subjectAttempt = dirAcc.selectScalarInt(" select COUNT(distinct ed.exam_code) as attempts from mark_entry m,Exam_Details ed,subject s where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and s.subject_code in('" + funcsubcode + "') and m.roll_no='" + roll + "'");

                    string equalsub = string.Empty;
                    string strsuboquery = "select subject_no from tbl_equal_subject_Grade_System where Common_Subject_no in(select Common_Subject_no from tbl_equal_subject_Grade_System where Subject_no='" + funcsubno + "')";
                    DataSet dsequlsub = dirAcc.selectDataSet(strsuboquery);
                    if (dsequlsub.Tables.Count > 0 && dsequlsub.Tables[0].Rows.Count > 0)
                    {
                        for (int es = 0; es < dsequlsub.Tables[0].Rows.Count; es++)
                        {
                            string subjectNo = Convert.ToString(dsequlsub.Tables[0].Rows[es]["subject_no"]).Trim();
                            if (equalsub.Trim() != "")
                            {
                                equalsub = equalsub + ",'" + subjectNo + "'";
                            }
                            else
                            {
                                equalsub = "'" + subjectNo + "'";
                            }
                        }
                    }
                    if (equalsub.Trim() == "")
                    {
                        equalsub = "'" + funcsubno + "'";
                    }
                    string qry = " select * from SubWiseGrdeMaster where Exam_Year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' and Exam_Month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and SubjectCode in(" + equalsub + ") order by Frange desc";
                    DataTable dtSubWiseGrade = new DataTable();
                    dtSubWiseGrade = dirAcc.selectDataTable(qry);
                    if (dtSubWiseGrade.Rows.Count == 0)
                    {
                        if (subjectAttempt > 1 && dtSubWiseGrade.Rows.Count == 0)
                        {
                            qry = " select *,(Exam_Year*12+Exam_Month) exmonval from SubWiseGrdeMaster where SubjectName='" + funcsubname + "' and (Exam_Year*12+Exam_Month)<('" + Convert.ToString(ddlYear.SelectedValue).Trim() + "'*12+'" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "') order by exmonval desc,Frange desc";
                        }
                    }
                    dtSubWiseGrade = new DataTable();
                    dtSubWiseGrade = dirAcc.selectDataTable(qry);
                    dtSubWiseGrade.DefaultView.RowFilter = "grade='B'";
                    DataView dvSubWisegrade = dtSubWiseGrade.DefaultView;
                    if (dvSubWisegrade.Count > 0)
                    {
                        if (subjectAttempt > 1)
                        {
                            minextmark = 0;// Convert.ToDouble(dvgrade[0]["trange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out minextmark);
                            minextmark = (minextmark * maxExternal) / 100;
                            mintotal = 0;// Convert.ToDouble(dvgrade[0]["trange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out mintotal);
                        }
                        else
                        {
                            minextmark = 0;//Convert.ToDouble(dvgrade[0]["frange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out minextmark);
                            minextmark = (minextmark * maxExternal) / 100;
                            mintotal = 0;//Convert.ToDouble(dvgrade[0]["frange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out mintotal);
                        }
                    }
                    if (mintotal > 50)
                    {
                        mintotal = 50;
                    }
                    if (lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                    {
                        if (totalMark < 50)
                        {
                            failgrade = true;
                        }
                    }
                    else
                    {
                        if (totalMark < mintotal)
                        {
                            failgrade = true;
                        }
                    }
                    if (failgrade == false)
                    {
                        dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + totalMark + "' and Trange >='" + totalMark + "'";// "Frange<='" + Reval_Tot + "'"; 
                        if (subjectAttempt > 1)
                        {
                            dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + totalMark + "' and Trange >='" + totalMark + "'";// "Frange<='" + Reval_Tot + "'";
                        }
                        if (attmptreal > subjectAttempt)
                        {
                            dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + totalMark + "' and Trange >='" + totalMark + "'";// "Frange<='" + Reval_Tot + "'"; 
                        }
                        else
                        {
                            dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + exte + "' and Trange >='" + exte + "'";// "Frange<='" + Reval_Tot + "'"; 
                        }
                        dvSubWisegrade = dtSubWiseGrade.DefaultView;
                        dvSubWisegrade.Sort = "Frange asc";
                        if (dvSubWisegrade.Count > 0)
                        {
                            funcgrade = Convert.ToString(dvSubWisegrade[0]["Grade"]).Trim();
                        }
                        else
                        {
                            funcgrade = "B";
                        }
                    }
                    else
                    {
                        funcgrade = "RA";
                    }
                    if (!string.IsNullOrEmpty(subjectGrade))
                    {
                        funcgrade = subjectGrade;
                    }
                    //checkmintotal = mintotal - 1;
                    //failgrade = "Ra";
                    //if (failgrade.Trim().ToLower() == actualgrade.Trim().ToLower() && actualgrade.Trim().ToLower() != grades.Trim().ToLower())
                    //{
                    //    status = "PASS";
                    //}
                    //else
                    //{
                    //    status = "GC";
                    //}
                }
                else
                {
                    if (attmptreal > attmpt)
                    {
                        if (dr_convert["total"].ToString() != string.Empty)
                        {
                            strgrade = "select mark_grade from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and college_code='" + Session["collegecode"] + "' and '" + dr_convert["total"] + "' between frange and trange";
                        }
                        else
                        {
                            strgrade = "select mark_grade from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and college_code='" + Session["collegecode"] + "' and credit_points between frange and trange";
                        }
                    }
                    else
                    {
                        if (dr_convert["total"].ToString() != string.Empty)
                        {
                            strgrade = "select mark_grade from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and college_code='" + Session["collegecode"] + "' and '" + exte.ToString() + "' between frange and trange";
                        }
                        else
                        {
                            strgrade = "select mark_grade from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and college_code='" + Session["collegecode"] + "' and credit_points between frange and trange";
                        }
                    }
                    SqlCommand cmd_grade = new SqlCommand(strgrade, con_Grade);
                    con_Grade.Close();
                    con_Grade.Open();
                    SqlDataReader dr_grade;
                    dr_grade = cmd_grade.ExecuteReader();
                    if (dr_grade.HasRows == true)
                    {
                        while (dr_grade.Read())
                        {
                            funcgrade = dr_grade["mark_grade"].ToString();
                        }
                    }
                    else
                    {
                        funcgrade = string.Empty;
                    }
                }
            }
        }
        catch
        {
        }
    }

    public void bindindnewspread()
    {
        FpExternal.Sheets[0].AutoPostBack = true;
        FpExternal.Sheets[0].ColumnHeader.RowCount = 1;
        FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        FpExternal.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.AliceBlue;
        FpExternal.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
        FpExternal.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
        FpExternal.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        FpExternal.Sheets[0].DefaultStyle.Font.Bold = true;
        FpExternal.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].RowHeader.Visible = false;
        FpExternal.Sheets[0].RowCount = 0;
        FpExternal.Sheets[0].ColumnCount = 7;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Sl.No";
        FpExternal.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
        FpExternal.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
        FpExternal.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject Code";
        FpExternal.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Grade";
        FpExternal.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Status";
        if (Session["Rollflag"].ToString() == "0")
        {
            FpExternal.Sheets[0].ColumnHeader.Columns[1].Visible = false;
        }
        if (Session["Regflag"].ToString() == "0")
        {
            FpExternal.Sheets[0].ColumnHeader.Columns[2].Visible = false;
        }
        if (exam_year != "Select")
        {
            ExamCode = Get_UnivExamCode(Convert.ToInt32(degree_code), GetSemester_AsNumber(Convert.ToInt32(current_sem)), Convert.ToInt32(batch_year), Convert.ToInt32(exam_month), Convert.ToInt32(exam_year));
            IntExamCode = ExamCode;
            bindrevalutionstudent(Convert.ToString(ExamCode));
        }
        FpExternal.SaveChanges();
    }

    public void bindrevalutionstudent(string exam_code)
    {
        try
        {
            string previoussgrade = string.Empty;
            string failgrade = string.Empty;
            DataSet dsgrade = new DataSet();
            DataView dvgrade = new DataView();
            DataSet dsallmark = new DataSet();
            DataView dvfiltermark = new DataView();
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
            SqlDataReader dr_grade_val;
            con.Close();
            con.Open();
            cmd = new SqlCommand("select linkvalue from inssettings where linkname='corresponding grade' and college_code='" + Session["collegecode"] + "'", con);
            dr_grade_val = cmd.ExecuteReader();
            while (dr_grade_val.Read())
            {
                if (dr_grade_val.HasRows == true)
                {
                    grade_setting = dr_grade_val[0].ToString();
                }
            }
            string grade = string.Empty;
            grade = GetFunction("select grade_flag from grademaster where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and exam_month='" + exam_month + "' and exam_year= '" + exam_year + "'");
            if (grade != "")
            {
                //con.Close();
                //con.Open();
                // string overallresult = "select * from mark_entry where exam_code='" + exam_code + "'";
                string overallresult = "select * from mark_entry where exam_code='" + exam_code + "'";
                //SqlDataAdapter sqldap = new SqlDataAdapter(overallresult, con);
                //sqldap.Fill(dsallmark);
                dsallmark = dirAcc.selectDataSet(overallresult);
                //con.Close();
                //con.Open();
                string gradequery = "select * from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "'";
                dsgrade = dirAcc.selectDataSet(gradequery);
                //SqlDataAdapter sqld = new SqlDataAdapter(gradequery, con);
                //sqld.Fill(dsgrade);

                string revstudentquery = "select ea.roll_no as revroll_no,ead.subject_no,r.reg_no,r.stud_name,r.roll_no,s.subject_code,s.subject_name,s.mintotal,maxtotal,s.min_ext_marks,s.min_int_marks,s.max_ext_marks,s.max_int_marks from exam_application ea,exam_appl_details ead, registration r,subject s where ea.appl_no=ead.appl_no and ea.exam_code='" + exam_code + "'  and r.roll_no=ea.roll_no and s.subject_no=ead.subject_no " + strorder + "";//and ea.exam_type=2 rajkumar 11/1/2018
                DataTable dr_students = new DataTable();
                dr_students = dirAcc.selectDataTable(revstudentquery);
                //con.Close();
                //con.Open();
                //SqlDataAdapter cmd_Subject = new SqlDataAdapter(revstudentquery, con);
                //cmd_Subject.Fill(dr_students);
                //dr_Students = cmd_Subject.ExecuteReader();
                int sno = 0;
                string temprollnumber = string.Empty;
                for (int stu = 0; stu < dr_students.Rows.Count; stu++)
                {
                    string roll_number = Convert.ToString(dr_students.Rows[stu]["revroll_no"]).Trim();
                    string stud = roll_number;
                    if (temprollnumber == "" || temprollnumber != roll_number)
                    {
                        sno++;
                    }
                    temprollnumber = roll_number;
                    string reg_no = Convert.ToString(dr_students.Rows[stu]["reg_no"]);
                    string Studentname = Convert.ToString(dr_students.Rows[stu]["stud_name"]);
                    string subject_code = Convert.ToString(dr_students.Rows[stu]["subject_code"]);
                    string subnumber = Convert.ToString(dr_students.Rows[stu]["subject_no"]);
                    double mintotal = Convert.ToDouble(dr_students.Rows[stu]["mintotal"]);
                    double minintmark = Convert.ToDouble(dr_students.Rows[stu]["min_int_marks"]);
                    double minextmark = Convert.ToDouble(dr_students.Rows[stu]["min_ext_marks"]);
                    double maxExternal = Convert.ToDouble(dr_students.Rows[stu]["max_ext_marks"]);
                    double maxInternal = Convert.ToDouble(dr_students.Rows[stu]["max_int_marks"]);
                    string subjectName = Convert.ToString(dr_students.Rows[stu]["subject_name"]);
                    getsubno = subnumber;
                    int attept = 0;double maxmrk = 0;
                    string getattmaxmark = daccess.GetFunctionv("select convert(varchar,attempts)+'-'+convert(varchar,maxmark) as amm from coe_attmaxmark where collegecode='" + Session["collegecode"].ToString() + "'");
                    string[] semecount = getattmaxmark.Split(new Char[] { '-' });
                    if (semecount.GetUpperBound(0) == 1)
                    {
                        attept = Convert.ToInt32(semecount[0].ToString());
                        maxmrk = Convert.ToDouble(semecount[1].ToString());
                        flagchknew = true;
                    }
                    else
                    {
                        flagchknew = false;
                    }
                    if (chk_subjectwisegrade.Checked)
                    {
                        int subjectAttempt = dirAcc.selectScalarInt(" select COUNT(distinct ed.exam_code) as attempts from mark_entry m,Exam_Details ed,subject s where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and s.subject_code in('" + subject_code + "') and m.roll_no='" + roll_number + "'");

                        string equalsub = string.Empty;
                        string strsuboquery = "select subject_no from tbl_equal_subject_Grade_System where Common_Subject_no in(select Common_Subject_no from tbl_equal_subject_Grade_System where Subject_no='" + subnumber + "')";
                        DataSet dsequlsub = dirAcc.selectDataSet(strsuboquery);
                        if (dsequlsub.Tables.Count > 0 && dsequlsub.Tables[0].Rows.Count > 0)
                        {
                            for (int es = 0; es < dsequlsub.Tables[0].Rows.Count; es++)
                            {
                                string subjectNo = Convert.ToString(dsequlsub.Tables[0].Rows[es]["subject_no"]).Trim();
                                if (equalsub.Trim() != "")
                                {
                                    equalsub = equalsub + ",'" + subjectNo + "'";
                                }
                                else
                                {
                                    equalsub = "'" + subjectNo + "'";
                                }
                            }
                        }
                        if (equalsub.Trim() == "")
                        {
                            equalsub = "'" + subnumber + "'";
                        }

                        string qry = " select * from SubWiseGrdeMaster where Exam_Year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' and Exam_Month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and SubjectCode in(" + equalsub + ") order by Frange desc";
                        DataTable dtSubWiseGrade = new DataTable();
                        dtSubWiseGrade = dirAcc.selectDataTable(qry);
                        if (dtSubWiseGrade.Rows.Count == 0)
                        {
                            if (subjectAttempt > 1 && dtSubWiseGrade.Rows.Count == 0)
                            {
                                qry = " select *,(Exam_Year*12+Exam_Month) exmonval from SubWiseGrdeMaster where SubjectName='" + subjectName + "' and (Exam_Year*12+Exam_Month)<('" + Convert.ToString(ddlYear.SelectedValue).Trim() + "'*12+'" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "') order by exmonval desc,Frange desc";
                            }
                        }
                        dtSubWiseGrade = new DataTable();
                        dtSubWiseGrade = dirAcc.selectDataTable(qry);
                        dtSubWiseGrade.DefaultView.RowFilter = "grade='B'";
                        DataView dvSubWisegrade = dtSubWiseGrade.DefaultView;
                        if (dvSubWisegrade.Count > 0)
                        {
                            if (subjectAttempt > 1)
                            {
                                minextmark = 0;// Convert.ToDouble(dvgrade[0]["trange"].ToString());
                                double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out minextmark);
                                minextmark = (minextmark * maxExternal) / 100;
                                mintotal = 0;// Convert.ToDouble(dvgrade[0]["trange"].ToString());
                                double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out mintotal);
                            }
                            else
                            {
                                minextmark = 0;//Convert.ToDouble(dvgrade[0]["frange"].ToString());
                                double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out minextmark);
                                minextmark = (minextmark * maxExternal) / 100;
                                mintotal = 0;//Convert.ToDouble(dvgrade[0]["frange"].ToString());
                                double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out mintotal);
                            }
                            maxmrk = minextmark;
                            if (mintotal > 50)
                            {
                                mintotal = 50;
                            }
                        }

                    }
                    if (grade == "2")
                    {
                        FpExternal.Sheets[0].RowCount++;
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Text = roll_number;
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Text = reg_no;
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = Studentname;
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = subject_code;
                        dsallmark.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_number + "' and subject_no='" + subnumber + "' and exam_code='" + exam_code + "'";
                        dvfiltermark = dsallmark.Tables[0].DefaultView;
                        if (dvfiltermark.Count > 0)
                        {
                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dvfiltermark[0]["grade"]).Trim();
                        }
                        else
                        {
                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = "-";
                        }
                        string actualgrade = Convert.ToString(dvfiltermark[0]["actual_grade"]).Trim();
                        string grades = Convert.ToString(dvfiltermark[0]["grade"]).Trim();
                        string result = Convert.ToString(dvfiltermark[0]["result"]);
                        string status = string.Empty;
                        if (actualgrade.Trim().ToLower() == grades.Trim().ToLower())
                        {
                            status = "NC";
                        }
                        else
                        {
                            double checkmintotal = 0;
                            if (chk_subjectwisegrade.Checked)
                            {                                
                                checkmintotal = mintotal - 1;
                                failgrade = "RA";
                                if (failgrade.Trim().ToLower() == actualgrade.Trim().ToLower() && actualgrade.Trim().ToLower() != grades.Trim().ToLower())
                                {
                                    status = "PASS";
                                }
                                else
                                {
                                    status = "GC";
                                }
                            }
                            else
                            {
                                checkmintotal = mintotal - 1;
                                failgrade = GetFunction("select mark_grade from grade_master where '" + checkmintotal + "' between frange and trange and degree_code='" + degree_code + "' and batch_year='" + batch_year + "'");
                                if (failgrade.Trim().ToLower() == actualgrade.Trim().ToLower() && actualgrade.Trim().ToLower() != grades.Trim().ToLower())
                                {
                                    status = "PASS";
                                }
                                else
                                {
                                    status = "GC";
                                }
                            }
                        }
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Text = status.ToString();
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        if (status.Trim().ToUpper() == "PASS")
                        {
                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].ForeColor = Color.Green;
                        }
                        con.Close();
                        con.Open();
                    }
                    else if (grade.Trim() == "3")
                    {
                        FpExternal.Sheets[0].RowCount++;
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Text = roll_number;
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Text = reg_no;
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = Studentname;
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = subject_code;
                        dsallmark.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_number + "' and subject_no='" + subnumber + "' and exam_code='" + exam_code + "'";
                        dvfiltermark = dsallmark.Tables[0].DefaultView;
                        if (grade_setting.Trim() == "0")
                        {
                            if (dvfiltermark.Count > 0)
                            {
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dvfiltermark[0]["actual_total"]).Trim();
                            }
                            else
                            {
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = "-";
                            }
                        }
                        else if (grade_setting.Trim() == "1")
                        {
                            if (flagchknew == true)
                            {
                                double inte = 0, exte = 0, realattpt = 0;
                                if ((dvfiltermark[0]["internal_mark"].ToString() != string.Empty) && (dvfiltermark[0]["External_mark"].ToString() != string.Empty)) ;
                                {
                                    //inte = Convert.ToDouble(dvfiltermark[0]["internal_mark"].ToString());
                                    //exte = Convert.ToDouble(dvfiltermark[0]["external_mark"].ToString());
                                    //realattpt = Convert.ToInt32(dvfiltermark[0]["attempts"].ToString());
                                    inte = 0;
                                    exte = 0;
                                    realattpt = 0;
                                    double.TryParse(Convert.ToString(dvfiltermark[0]["internal_mark"]).Trim(), out inte);
                                    double.TryParse(Convert.ToString(dvfiltermark[0]["external_mark"]).Trim(), out exte);
                                    double.TryParse(Convert.ToString(dvfiltermark[0]["attempts"]).Trim(), out realattpt);
                                    double convertMark = exte;
                                    if (chk_subjectwisegrade.Checked)
                                    {
                                        convertMark = Math.Round((exte / maxExternal) * 100, 1, MidpointRounding.AwayFromZero);
                                        //exte = convertMark;
                                    }
                                    if (attept > realattpt)
                                    {
                                        if (Convert.ToDouble(inte) >= Convert.ToDouble(minintmark) && convertMark >= Convert.ToDouble(minextmark) && ((inte + exte) >= Convert.ToDouble((mintotal))))
                                        {
                                            //convertgradev(stud, getsubno, maxmrk, attept);
                                            ConvertGradeNew(stud, getsubno, maxmrk, attept, 1, ref funcgrade);
                                            // result = "Pass";
                                            previoussgrade = funcgrade;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = previoussgrade.ToString();
                                        }
                                        else
                                        {
                                            //=====new 07.07.12
                                            con.Close();
                                            con.Open();
                                            SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                            SqlDataReader dr_failgrade;//= new SqlDataReader(cmd_failgrade);
                                            dr_failgrade = cmd_failgrade.ExecuteReader();
                                            if (dr_failgrade.HasRows == true)
                                            {
                                                if (dr_failgrade.Read())
                                                {
                                                    if (dr_failgrade["value"].ToString() != "")
                                                    {
                                                        failgrade = dr_failgrade["value"].ToString();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                failgrade = "-";
                                            }
                                            if (chk_subjectwisegrade.Checked)
                                            {
                                                failgrade = "RA";
                                            }
                                            //===============07.07.12
                                            //   funcgrade = "RA";//07.07.12
                                            previoussgrade = failgrade;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = previoussgrade.ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (minextmark <= convertMark)
                                        {
                                            //convertgradev(stud, getsubno, maxmrk, attept);
                                            ConvertGradeNew(stud, getsubno, maxmrk, attept, 1, ref funcgrade);
                                            //result = "Pass";
                                            previoussgrade = funcgrade;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = previoussgrade.ToString();
                                        }
                                        else
                                        {
                                            //=====new 07.07.12
                                            con.Close();
                                            con.Open();
                                            SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                            SqlDataReader dr_failgrade;//= new SqlDataReader(cmd_failgrade);
                                            dr_failgrade = cmd_failgrade.ExecuteReader();
                                            if (dr_failgrade.HasRows == true)
                                            {
                                                if (dr_failgrade.Read())
                                                {
                                                    if (dr_failgrade["value"].ToString() != "")
                                                    {
                                                        failgrade = dr_failgrade["value"].ToString();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                failgrade = "-";
                                            }
                                            if (chk_subjectwisegrade.Checked)
                                            {
                                                failgrade = "RA";
                                            }
                                            previoussgrade = failgrade;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = previoussgrade.ToString();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                double inte = 0, exte = 0, realattpt = 0;
                                if ((Convert.ToString(dvfiltermark[0]["internal_mark"]) != string.Empty) && (Convert.ToString(dvfiltermark[0]["External_mark"]) != string.Empty) && (Convert.ToString(minintmark) != string.Empty) && (Convert.ToString(minextmark) != string.Empty) && (Convert.ToString(mintotal) != string.Empty))
                                {
                                    //inte = Convert.ToDouble(dvfiltermark[0]["internal_mark"].ToString());
                                    //exte = Convert.ToDouble(dvfiltermark[0]["external_mark"].ToString());
                                    //realattpt = Convert.ToInt32(dvfiltermark[0]["attempts"].ToString());
                                    inte = 0;
                                    exte = 0;
                                    realattpt = 0;
                                    double.TryParse(Convert.ToString(dvfiltermark[0]["internal_mark"]).Trim(), out inte);
                                    double.TryParse(Convert.ToString(dvfiltermark[0]["external_mark"]).Trim(), out exte);
                                    double.TryParse(Convert.ToString(dvfiltermark[0]["attempts"]).Trim(), out realattpt);
                                    double convertMark = exte;
                                    if (chk_subjectwisegrade.Checked)
                                    {
                                        convertMark = Math.Round((exte / maxExternal) * 100, 1, MidpointRounding.AwayFromZero);
                                        //exte = convertMark;
                                    }
                                    if (Convert.ToDouble(inte) >= Convert.ToDouble(minintmark) && Convert.ToDouble(convertMark) >= Convert.ToDouble(minextmark) && ((inte + exte) >= Convert.ToDouble((mintotal))))
                                    {
                                        convertgrade(stud, getsubno);
                                        previoussgrade = funcgrade;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = previoussgrade.ToString();
                                    }
                                    else
                                    {
                                        con.Close();
                                        con.Open();
                                        SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                        SqlDataReader dr_failgrade;
                                        dr_failgrade = cmd_failgrade.ExecuteReader();
                                        if (dr_failgrade.HasRows == true)
                                        {
                                            if (dr_failgrade.Read())
                                            {
                                                if (dr_failgrade["value"].ToString() != "")
                                                {
                                                    failgrade = dr_failgrade["value"].ToString();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            failgrade = "-";
                                        }
                                        if (chk_subjectwisegrade.Checked)
                                        {
                                            failgrade = "RA";
                                        }
                                        previoussgrade = failgrade;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = failgrade.ToString();
                                    }
                                }
                            }
                        }
                        string status = string.Empty;
                        if (grade_setting == "0")
                        {
                            double actualtotal = Convert.ToDouble(dvfiltermark[0]["actual_total"]);
                            double total = Convert.ToDouble(dvfiltermark[0]["total"]);
                            if (actualtotal == total)
                            {
                                status = "NC";
                            }
                            else
                            {
                                if (total <= mintotal && actualtotal >= mintotal)
                                {
                                    status = "PASS";
                                }
                                else
                                {
                                    status = "GC";
                                }
                            }
                        }
                        else if (grade_setting.Trim() == "1")
                        {
                            if (flagchknew == true)
                            {
                                double inte = 0, exte = 0, realattpt = 0;
                                if ((dvfiltermark[0]["actual_internal_mark"].ToString() != string.Empty) && (dvfiltermark[0]["actual_External_mark"].ToString() != string.Empty)) ;
                                {
                                    //inte = Convert.ToDouble(dvfiltermark[0]["actual_internal_mark"].ToString());
                                    //exte = Convert.ToDouble(dvfiltermark[0]["actual_external_mark"].ToString());
                                    //realattpt = Convert.ToInt32(dvfiltermark[0]["attempts"].ToString());
                                    inte = 0;
                                    exte = 0;
                                    realattpt = 0;
                                    double.TryParse(Convert.ToString(dvfiltermark[0]["actual_internal_mark"]).Trim(), out inte);
                                    double.TryParse(Convert.ToString(dvfiltermark[0]["actual_external_mark"]).Trim(), out exte);
                                    double.TryParse(Convert.ToString(dvfiltermark[0]["attempts"]).Trim(), out realattpt);
                                    double convertMark = exte;
                                    if (chk_subjectwisegrade.Checked)
                                    {
                                        convertMark = Math.Round((exte / maxExternal) * 100, 1, MidpointRounding.AwayFromZero);
                                        //exte = convertMark;
                                    }
                                    if (attept > realattpt)
                                    {
                                        if (Convert.ToDouble(inte) >= Convert.ToDouble(minintmark) && Convert.ToDouble(convertMark) >= Convert.ToDouble(minextmark) && ((inte + exte) >= Convert.ToDouble((mintotal))))
                                        {
                                            //convertaftergradev(stud, getsubno, maxmrk, attept);
                                            ConvertGradeNew(stud, getsubno, maxmrk, attept, 0, ref funcgrade);
                                            // result = "Pass";
                                            previousgrade = funcgrade;
                                        }
                                        else
                                        {
                                            //=====new 07.07.12
                                            con.Close();
                                            con.Open();
                                            SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                            SqlDataReader dr_failgrade;//= new SqlDataReader(cmd_failgrade);
                                            dr_failgrade = cmd_failgrade.ExecuteReader();
                                            if (dr_failgrade.HasRows == true)
                                            {
                                                if (dr_failgrade.Read())
                                                {
                                                    if (dr_failgrade["value"].ToString().Trim() != "")
                                                    {
                                                        failgrade = dr_failgrade["value"].ToString().Trim();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                failgrade = "-";
                                            }
                                            if (chk_subjectwisegrade.Checked)
                                            {
                                                failgrade = "RA";
                                            }
                                            //===============07.07.12
                                            //   funcgrade = "RA";//07.07.12
                                            previousgrade = failgrade;
                                        }
                                    }
                                    else
                                    {
                                        if (minextmark <= convertMark)
                                        {
                                            //convertaftergradev(stud, getsubno, maxmrk, attept);
                                            ConvertGradeNew(stud, getsubno, maxmrk, attept, 0, ref funcgrade);
                                            //result = "Pass";
                                            previousgrade = failgrade;
                                        }
                                        else
                                        {
                                            //=====new 07.07.12
                                            con.Close();
                                            con.Open();
                                            SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                            SqlDataReader dr_failgrade;//= new SqlDataReader(cmd_failgrade);
                                            dr_failgrade = cmd_failgrade.ExecuteReader();
                                            if (dr_failgrade.HasRows == true)
                                            {
                                                if (dr_failgrade.Read())
                                                {
                                                    if (dr_failgrade["value"].ToString().Trim() != "")
                                                    {
                                                        failgrade = dr_failgrade["value"].ToString().Trim();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                failgrade = "-";
                                            }
                                            if (chk_subjectwisegrade.Checked)
                                            {
                                                failgrade = "RA";
                                            }
                                            previousgrade = failgrade;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                double inte = 0, exte = 0, realattpt = 0;
                                if ((Convert.ToString(dvfiltermark[0]["actual_internal_mark"]) != string.Empty) && (Convert.ToString(dvfiltermark[0]["actual_External_mark"]) != string.Empty) && (Convert.ToString(minintmark) != string.Empty) && (Convert.ToString(minextmark) != string.Empty) && (Convert.ToString(mintotal) != string.Empty))
                                {
                                    //inte = Convert.ToDouble(dvfiltermark[0]["actual_internal_mark"].ToString());
                                    //exte = Convert.ToDouble(dvfiltermark[0]["actual_external_mark"].ToString());
                                    //realattpt = Convert.ToInt32(dvfiltermark[0]["attempts"].ToString());
                                    inte = 0;
                                    exte = 0;
                                    realattpt = 0;
                                    double.TryParse(Convert.ToString(dvfiltermark[0]["actual_internal_mark"]).Trim(), out inte);
                                    double.TryParse(Convert.ToString(dvfiltermark[0]["actual_external_mark"]).Trim(), out exte);
                                    double.TryParse(Convert.ToString(dvfiltermark[0]["attempts"]).Trim(), out realattpt);
                                    double convertMark = exte;
                                    if (chk_subjectwisegrade.Checked)
                                    {
                                        convertMark = Math.Round((exte / maxExternal) * 100, 1, MidpointRounding.AwayFromZero);
                                    }
                                    if (Convert.ToDouble(inte) >= Convert.ToDouble(minintmark) && Convert.ToDouble(convertMark) >= Convert.ToDouble(minextmark) && ((inte + exte) >= Convert.ToDouble((mintotal))))
                                    {
                                        convertaftergrade(stud, getsubno);
                                        //ConvertGradeNew1(stud, getsubno);
                                        //previousgrade = failgrade;
                                    }
                                    else
                                    {
                                        con.Close();
                                        con.Open();
                                        SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                        SqlDataReader dr_failgrade;
                                        dr_failgrade = cmd_failgrade.ExecuteReader();
                                        if (dr_failgrade.HasRows == true)
                                        {
                                            if (dr_failgrade.Read())
                                            {
                                                if (dr_failgrade["value"].ToString().Trim() != "")
                                                {
                                                    failgrade = dr_failgrade["value"].ToString().Trim();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            failgrade = "-";
                                        }
                                        if (chk_subjectwisegrade.Checked)
                                        {
                                            failgrade = "RA";
                                        }
                                        previousgrade = failgrade;
                                    }
                                }
                            }
                            if (previousgrade.ToLower().Trim() == previoussgrade.ToLower().Trim())
                            {
                                status = "NC";
                            }
                            else
                            {
                                if (previousgrade.ToLower().Trim() == failgrade.ToLower().Trim() && previoussgrade.ToLower().Trim() != failgrade.ToLower().Trim())
                                {
                                    status = "PASS";
                                }
                                else
                                {
                                    status = "GC";
                                }
                            }
                        }
                        //-------------
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Text = status.ToString();
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        if (status.ToUpper().Trim() == "PASS")
                        {
                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].ForeColor = Color.Green;
                        }
                    }
                }
                if (FpExternal.Sheets[0].RowCount > 0)
                {
                    FpExternal.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpExternal.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpExternal.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpExternal.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                }
                //if (FpExternal.Sheets[0].RowCount > 0)
                //{
                //    FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 3;
                //    FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 0, 3, FpExternal.Sheets[0].ColumnCount);
                //    FpExternal.Sheets[0].SetRowMerge(FpExternal.Sheets[0].RowCount - 3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                //    FpExternal.Sheets[0].SetRowMerge(FpExternal.Sheets[0].RowCount - 2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                //    FpExternal.Sheets[0].SetRowMerge(FpExternal.Sheets[0].RowCount - 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                //    FpExternal.Sheets[0].RowCount++;
                //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = DateTime.Now.ToShortDateString();
                //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Text = "CONTROLLER OF EXAMINATIONS";
                //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                //    FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 1, 1,FpExternal.Sheets[0].ColumnCount-2);
                //    if (grade == "2")
                //    {
                //        DataTable dt = new DataTable();
                //        con.Close();
                //        con.Open();
                //        string gradedetails = "select * from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' order by frange";
                //        SqlDataAdapter sqldapp = new SqlDataAdapter(gradedetails, con);
                //        sqldapp.Fill(dt);
                //        if (dt.Rows.Count > 0)
                //        {
                //            string markrange = "Mark Range & Letter Grade:      ";
                //            string lettergrade = "Letter Grade & Grade Points:     ";
                //            for (int i = 0; i < dt.Rows.Count; i++)
                //            {
                //                string frange = Convert.ToString(dt.Rows[i]["frange"]);
                //                string trange = Convert.ToString(dt.Rows[i]["trange"]);
                //                string mark_grade = Convert.ToString(dt.Rows[i]["Mark_Grade"]);
                //                string credit=Convert.ToString(dt.Rows[i]["credit_points"]);
                //                if (frange == "0")
                //                {
                //                    markrange = markrange + "         <" + trange + "" + " : "+"  '' " + mark_grade+" ''"+"";
                //                    lettergrade = lettergrade + " " + mark_grade + ":" + credit;
                //                }
                //                else
                //                {
                //                    markrange = markrange + "     " + frange + " - " + trange + " : " +" ''"+ mark_grade+"'' "+"";
                //                    lettergrade = lettergrade + " " + mark_grade + ":" + credit;
                //                }
                //            }
                //            FpExternal.Sheets[0].RowCount++;
                //            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = markrange.ToString();
                //            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                //            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, FpExternal.Sheets[0].ColumnCount);
                //            FpExternal.Sheets[0].RowCount++;
                //            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = lettergrade.ToString();
                //            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                //            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, FpExternal.Sheets[0].ColumnCount);
                //            FpExternal.Sheets[0].RowCount++;
                //            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "NC-NO CHANGE    ,   GC-GRADE CHANGE";
                //            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                //            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, FpExternal.Sheets[0].ColumnCount);
                //        }
                //    }
                //}
                if (FpExternal.Sheets[0].RowCount > 0)
                {
                    lblerrormsg.Text = " ";
                    lblerrormsg.Visible = false;
                    FpExternal.Sheets[0].PageSize = FpExternal.Sheets[0].Rows.Count;
                    FpExternal.Visible = true;
                    btnExcel.Visible = true;
                    txtexcelname.Visible = true;
                    lblrptname.Visible = true;
                    btnprintmaster.Visible = true;
                }
                //else
                //{
                    //lblerrormsg.Text = "No Records Found";
                    //lblerrormsg.Visible = true;
                    //FpExternal.Visible = false;
                    //btnExcel.Visible = false;
                    //txtexcelname.Visible = false;
                    //lblrptname.Visible = false;
                    //btnprintmaster.Visible = false;
                //}
            }
            else
            {
                lblerrormsg.Text = "Kindly Set Grade";
                lblerrormsg.Visible = true;
                FpExternal.Visible = false;
                btnExcel.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                btnprintmaster.Visible = false;
            }
        }
        catch
        {

        }
    }

    //convertgradev
    public void ConvertGradeNew(string roll, string subj, double maxmarkve, int attmptreal, byte isBeforeOrAfter, ref string funcgrade)
    {
        try
        {
            string strexam = "Select s.subject_name,s.subject_code,m.internal_mark,m.external_mark,m.actual_internal_mark,m.actual_external_mark,m.attempts,m.actual_total,m.total,m.result,m.cp,m.subject_no,s.max_ext_marks,s.max_int_marks,s.maxtotal,s.mintotal,s.min_int_marks,s.min_ext_marks,ss.lab,m.grade,m.Actual_Grade  from Mark_Entry m,Subject s,sub_sem ss where m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.Exam_Code = '" + IntExamCode + "'  and m.roll_no='" + roll + "' and s.subject_no='" + subj + "'";
            DataTable dtStudentMarks = new DataTable();
            dtStudentMarks = dirAcc.selectDataTable(strexam);
            double inte = 0, exte = 0;
            int attmpt = 0;
            foreach (DataRow dr_convert in dtStudentMarks.Rows)
            {
                //   funcsemester = dr_convert["semester"].ToString().Trim();
                funcsubname = Convert.ToString(dr_convert["subject_name"]).Trim();
                funcsubno = Convert.ToString(dr_convert["subject_no"]).Trim();
                funcsubcode = Convert.ToString(dr_convert["subject_code"]).Trim();
                funcresult = Convert.ToString(dr_convert["result"]).Trim();
                funccredit = Convert.ToString(dr_convert["cp"]).Trim();
                string subjectGrade = string.Empty;// Convert.ToString(dr_convert["grade"]).Trim();
                if (isBeforeOrAfter == 0)
                {
                    mark = dr_convert["actual_total"].ToString();
                    inte = Convert.ToDouble(dr_convert["actual_internal_mark"].ToString());
                    exte = Convert.ToDouble(dr_convert["actual_external_mark"].ToString());
                    attmpt = Convert.ToInt32(dr_convert["attempts"].ToString());
                    subjectGrade = Convert.ToString(dr_convert["Actual_Grade"]).Trim();
                }
                else if (isBeforeOrAfter == 1)
                {
                    mark = dr_convert["total"].ToString();
                    inte = Convert.ToDouble(dr_convert["internal_mark"].ToString());
                    exte = Convert.ToDouble(dr_convert["external_mark"].ToString());
                    attmpt = Convert.ToInt32(dr_convert["attempts"].ToString());
                    subjectGrade = Convert.ToString(dr_convert["grade"]).Trim();
                }
                funcgrade = string.Empty;
                string strgrade = string.Empty;
                string lab = Convert.ToString(dr_convert["lab"]).Trim();

                double minextmark = 0;
                double totalMark = 0;
                double maxExternal = 0;
                double mintotal = 0;
                double maxtotal = 0;
                double minInternal = 0;
                double maxInternal = 0;

                double.TryParse(Convert.ToString(dr_convert["max_ext_marks"]), out maxExternal);
                double.TryParse(Convert.ToString(dr_convert["max_int_marks"]), out maxInternal);
                double.TryParse(Convert.ToString(dr_convert["maxtotal"]), out maxtotal);

                double.TryParse(Convert.ToString(dr_convert["min_ext_marks"]), out minextmark);
                double.TryParse(Convert.ToString(dr_convert["min_int_marks"]), out minInternal);
                double.TryParse(Convert.ToString(dr_convert["mintotal"]), out mintotal);
                double.TryParse(Convert.ToString(mark), out totalMark);
                double checkmintotal = 0;
                if (chk_subjectwisegrade.Checked)
                {
                    bool failgrade = false;
                    int subjectAttempt = dirAcc.selectScalarInt(" select COUNT(distinct ed.exam_code) as attempts from mark_entry m,Exam_Details ed,subject s where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and s.subject_code in('" + funcsubcode + "') and m.roll_no='" + roll + "'");

                    string equalsub = string.Empty;
                    string strsuboquery = "select subject_no from tbl_equal_subject_Grade_System where Common_Subject_no in(select Common_Subject_no from tbl_equal_subject_Grade_System where Subject_no='" + funcsubno + "')";
                    DataSet dsequlsub = dirAcc.selectDataSet(strsuboquery);
                    if (dsequlsub.Tables.Count > 0 && dsequlsub.Tables[0].Rows.Count > 0)
                    {
                        for (int es = 0; es < dsequlsub.Tables[0].Rows.Count; es++)
                        {
                            string subjectNo = Convert.ToString(dsequlsub.Tables[0].Rows[es]["subject_no"]).Trim();
                            if (equalsub.Trim() != "")
                            {
                                equalsub = equalsub + ",'" + subjectNo + "'";
                            }
                            else
                            {
                                equalsub = "'" + subjectNo + "'";
                            }
                        }
                    }
                    if (equalsub.Trim() == "")
                    {
                        equalsub = "'" + funcsubno + "'";
                    }
                    string qry = " select * from SubWiseGrdeMaster where Exam_Year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' and Exam_Month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and SubjectCode in(" + equalsub + ") order by Frange desc";
                    DataTable dtSubWiseGrade = new DataTable();
                    dtSubWiseGrade = dirAcc.selectDataTable(qry);
                    if (dtSubWiseGrade.Rows.Count == 0)
                    {
                        if (subjectAttempt > 1 && dtSubWiseGrade.Rows.Count == 0)
                        {
                            qry = " select *,(Exam_Year*12+Exam_Month) exmonval from SubWiseGrdeMaster where SubjectName='" + funcsubname + "' and (Exam_Year*12+Exam_Month)<('" + Convert.ToString(ddlYear.SelectedValue).Trim() + "'*12+'" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "') order by exmonval desc,Frange desc";
                        }
                    }
                    dtSubWiseGrade = new DataTable();
                    dtSubWiseGrade = dirAcc.selectDataTable(qry);
                    dtSubWiseGrade.DefaultView.RowFilter = "grade='B'";
                    DataView dvSubWisegrade = dtSubWiseGrade.DefaultView;
                    if (dvSubWisegrade.Count > 0)
                    {
                        if (subjectAttempt > 1)
                        {
                            minextmark = 0;// Convert.ToDouble(dvgrade[0]["trange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out minextmark);
                            minextmark = (minextmark * maxExternal) / 100;
                            mintotal = 0;// Convert.ToDouble(dvgrade[0]["trange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out mintotal);
                        }
                        else
                        {
                            minextmark = 0;//Convert.ToDouble(dvgrade[0]["frange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out minextmark);
                            minextmark = (minextmark * maxExternal) / 100;
                            mintotal = 0;//Convert.ToDouble(dvgrade[0]["frange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out mintotal);
                        }
                    }
                    if (mintotal > 50)
                    {
                        mintotal = 50;
                    }
                    if (lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                    {
                        if (totalMark < 50)
                        {
                            failgrade = true;
                        }
                    }
                    else
                    {
                        if (totalMark < mintotal)
                        {
                            failgrade = true;
                        }
                    }
                    if (failgrade == false)
                    {
                        dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + totalMark + "' and Trange >='" + totalMark + "'";// "Frange<='" + Reval_Tot + "'"; 
                        if (subjectAttempt > 1)
                        {
                            dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + totalMark + "' and Trange >='" + totalMark + "'";// "Frange<='" + Reval_Tot + "'";
                        }
                        if (attmptreal > subjectAttempt)
                        {
                            dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + totalMark + "' and Trange >='" + totalMark + "'";// "Frange<='" + Reval_Tot + "'"; 
                        }
                        else
                        {
                            dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + exte + "' and Trange >='" + exte + "'";// "Frange<='" + Reval_Tot + "'"; 
                        }
                        dvSubWisegrade = dtSubWiseGrade.DefaultView;
                        dvSubWisegrade.Sort = "Frange asc";
                        if (dvSubWisegrade.Count > 0)
                        {
                            funcgrade = Convert.ToString(dvSubWisegrade[0]["Grade"]).Trim();
                        }
                        else
                        {
                            funcgrade = "B";
                        }
                    }
                    else
                    {
                        funcgrade = "RA";
                    }
                    if (!string.IsNullOrEmpty(subjectGrade))
                    {
                        funcgrade = subjectGrade;
                    }
                    //checkmintotal = mintotal - 1;
                    //failgrade = "Ra";
                    //if (failgrade.Trim().ToLower() == actualgrade.Trim().ToLower() && actualgrade.Trim().ToLower() != grades.Trim().ToLower())
                    //{
                    //    status = "PASS";
                    //}
                    //else
                    //{
                    //    status = "GC";
                    //}
                }
                else
                {
                    if (attmptreal > attmpt)
                    {
                        if (dr_convert["total"].ToString() != string.Empty)
                        {
                            strgrade = "select mark_grade from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and college_code='" + Session["collegecode"] + "' and '" + dr_convert["total"] + "' between frange and trange";
                        }
                        else
                        {
                            strgrade = "select mark_grade from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and college_code='" + Session["collegecode"] + "' and credit_points between frange and trange";
                        }
                    }
                    else
                    {
                        if (dr_convert["total"].ToString() != string.Empty)
                        {
                            strgrade = "select mark_grade from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and college_code='" + Session["collegecode"] + "' and '" + exte.ToString() + "' between frange and trange";
                        }
                        else
                        {
                            strgrade = "select mark_grade from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and college_code='" + Session["collegecode"] + "' and credit_points between frange and trange";
                        }
                    }
                    SqlCommand cmd_grade = new SqlCommand(strgrade, con_Grade);
                    con_Grade.Close();
                    con_Grade.Open();
                    SqlDataReader dr_grade;
                    dr_grade = cmd_grade.ExecuteReader();
                    if (dr_grade.HasRows == true)
                    {
                        while (dr_grade.Read())
                        {
                            funcgrade = dr_grade["mark_grade"].ToString();
                        }
                    }
                    else
                    {
                        funcgrade = string.Empty;
                    }
                }
            }
        }
        catch
        {
        }
    }

    //convertaftergrade
    public void ConvertGradeNew1(string roll, string subj)
    {
        try
        {
            string strexam = string.Empty;
            strexam = "Select subject_name,subject_code,total,actual_total,result,cp,m.grade,m.subject_no,s.max_ext_marks,s.max_int_marks,s.maxtotal,s.mintotal,s.min_int_marks,s.min_ext_marks,ss.lab,m.Actual_Grade from Mark_Entry m,Subject s,sub_sem ss where m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and  m.Exam_Code = '" + IntExamCode + "'  and m.roll_no='" + roll + "' and s.subject_no='" + subj + "'";
            SqlCommand cmd_exam1 = new SqlCommand(strexam, con);
            con.Close();
            con.Open();
            SqlDataReader dr_convert;
            dr_convert = cmd_exam1.ExecuteReader();
            if (dr_convert.Read())
            {
                funcsubname = dr_convert["subject_name"].ToString();
                funcsubno = dr_convert["subject_no"].ToString();
                funcsubcode = dr_convert["subject_code"].ToString();
                funcresult = dr_convert["result"].ToString();
                funccredit = dr_convert["cp"].ToString();
                mark = dr_convert["actual_total"].ToString();
                previousgrade = string.Empty;
                string strgrade = string.Empty;
                string lab = Convert.ToString(dr_convert["lab"]).Trim();
                string subjectGrade = Convert.ToString(dr_convert["Actual_Grade"]).Trim();
                double minextmark = 0;
                double totalMark = 0;
                double maxExternal = 0;
                double mintotal = 0;
                double maxtotal = 0;
                double minInternal = 0;
                double maxInternal = 0;

                double.TryParse(Convert.ToString(dr_convert["max_ext_marks"]), out maxExternal);
                double.TryParse(Convert.ToString(dr_convert["max_int_marks"]), out maxInternal);
                double.TryParse(Convert.ToString(dr_convert["maxtotal"]), out maxtotal);

                double.TryParse(Convert.ToString(dr_convert["min_ext_marks"]), out minextmark);
                double.TryParse(Convert.ToString(dr_convert["min_int_marks"]), out minInternal);
                double.TryParse(Convert.ToString(dr_convert["mintotal"]), out mintotal);
                double.TryParse(Convert.ToString(mark), out totalMark);
                //if (dr_convert["total"].ToString() != string.Empty)
                double checkmintotal = 0;
                if (chk_subjectwisegrade.Checked)
                {
                    bool failgrade = false;
                    int subjectAttempt = dirAcc.selectScalarInt(" select COUNT(distinct ed.exam_code) as attempts from mark_entry m,Exam_Details ed,subject s where ed.exam_code=m.exam_code and s.subject_no=m.subject_no and s.subject_code in('" + funcsubcode + "') and m.roll_no='" + roll + "'");

                    string equalsub = string.Empty;
                    string strsuboquery = "select subject_no from tbl_equal_subject_Grade_System where Common_Subject_no in(select Common_Subject_no from tbl_equal_subject_Grade_System where Subject_no='" + funcsubno + "')";
                    DataSet dsequlsub = dirAcc.selectDataSet(strsuboquery);
                    if (dsequlsub.Tables.Count > 0 && dsequlsub.Tables[0].Rows.Count > 0)
                    {
                        for (int es = 0; es < dsequlsub.Tables[0].Rows.Count; es++)
                        {
                            string subjectNo = Convert.ToString(dsequlsub.Tables[0].Rows[es]["subject_no"]).Trim();
                            if (equalsub.Trim() != "")
                            {
                                equalsub = equalsub + ",'" + subjectNo + "'";
                            }
                            else
                            {
                                equalsub = "'" + subjectNo + "'";
                            }
                        }
                    }
                    if (equalsub.Trim() == "")
                    {
                        equalsub = "'" + funcsubno + "'";
                    }

                    string qry = " select * from SubWiseGrdeMaster where Exam_Year='" + Convert.ToString(ddlYear.SelectedValue).Trim() + "' and Exam_Month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and SubjectCode in(" + equalsub + ") order by Frange desc";
                    DataTable dtSubWiseGrade = new DataTable();
                    dtSubWiseGrade = dirAcc.selectDataTable(qry);
                    if (dtSubWiseGrade.Rows.Count == 0)
                    {
                        if (subjectAttempt > 1 && dtSubWiseGrade.Rows.Count == 0)
                        {
                            qry = " select *,(Exam_Year*12+Exam_Month) exmonval from SubWiseGrdeMaster where SubjectName='" + funcsubname + "' and (Exam_Year*12+Exam_Month)<('" + Convert.ToString(ddlYear.SelectedValue).Trim() + "'*12+'" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "') order by exmonval desc,Frange desc";
                        }
                    }
                    dtSubWiseGrade = new DataTable();
                    dtSubWiseGrade = dirAcc.selectDataTable(qry);
                    dtSubWiseGrade.DefaultView.RowFilter = "grade='B'";
                    DataView dvSubWisegrade = dtSubWiseGrade.DefaultView;
                    if (dvSubWisegrade.Count > 0)
                    {
                        if (subjectAttempt > 1)
                        {
                            minextmark = 0;// Convert.ToDouble(dvgrade[0]["trange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out minextmark);
                            minextmark = (minextmark * maxExternal) / 100;
                            mintotal = 0;// Convert.ToDouble(dvgrade[0]["trange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out mintotal);
                        }
                        else
                        {
                            minextmark = 0;//Convert.ToDouble(dvgrade[0]["frange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out minextmark);
                            minextmark = (minextmark * maxExternal) / 100;
                            mintotal = 0;//Convert.ToDouble(dvgrade[0]["frange"].ToString());
                            double.TryParse(Convert.ToString(dvSubWisegrade[0]["Frange"]).Trim(), out mintotal);
                        }
                    }
                    if (mintotal > 50)
                    {
                        mintotal = 50;
                    }
                    if (lab.Trim() == "1" || lab.Trim().ToLower() == "true")
                    {
                        if (totalMark < 50)
                        {
                            failgrade = true;
                        }
                    }
                    else
                    {
                        if (totalMark < mintotal)
                        {
                            failgrade = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(subjectGrade))
                    {
                        previousgrade = subjectGrade;
                    }
                    else
                    {
                        if (failgrade == false)
                        {
                            dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + totalMark + "' and Trange >='" + totalMark + "'";// "Frange<='" + Reval_Tot + "'"; 
                            if (subjectAttempt > 1)
                            {
                                dtSubWiseGrade.DefaultView.RowFilter = "Frange<'" + totalMark + "' and Trange >='" + totalMark + "'";// "Frange<='" + Reval_Tot + "'";
                            }
                            dvSubWisegrade = dtSubWiseGrade.DefaultView;
                            dvSubWisegrade.Sort = "Frange asc";
                            if (dvSubWisegrade.Count > 0)
                            {
                                previousgrade = Convert.ToString(dvSubWisegrade[0]["Grade"]).Trim();
                            }
                            else
                            {
                                previousgrade = "B";
                            }
                        }
                        else
                        {
                            previousgrade = "RA";
                        }
                    }
                    //checkmintotal = mintotal - 1;
                    //failgrade = "Ra";
                    //if (failgrade.Trim().ToLower() == actualgrade.Trim().ToLower() && actualgrade.Trim().ToLower() != grades.Trim().ToLower())
                    //{
                    //    status = "PASS";
                    //}
                    //else
                    //{
                    //    status = "GC";
                    //}
                }
                else
                {
                    if (mark != string.Empty)
                    {
                        strgrade = "select mark_grade from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and college_code='" + Session["collegecode"] + "' and '" + mark + "' between frange and trange";
                    }
                    else
                    {
                        strgrade = "select mark_grade from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and college_code='" + Session["collegecode"] + "' and credit_points between frange and trange";
                    }
                    SqlCommand cmd_grade = new SqlCommand(strgrade, con);
                    con.Close();
                    con.Open();
                    SqlDataReader dr_grade;
                    dr_grade = cmd_grade.ExecuteReader();
                    if (dr_grade.HasRows == true)
                    {
                        while (dr_grade.Read())
                        {
                            previousgrade = dr_grade["mark_grade"].ToString();
                        }
                    }
                    else
                    {
                        previousgrade = string.Empty;
                    }
                }
            }
        }
        catch
        {
        }
    }

}