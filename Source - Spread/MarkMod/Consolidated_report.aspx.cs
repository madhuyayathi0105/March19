using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using BalAccess;
using System.Text;
using System.Collections.Generic;

public partial class Consolidated_report : System.Web.UI.Page
{

    string classs = "";
    Hashtable hatdclab = new Hashtable();
    int split_holiday_status_11 = 0, split_holiday_status_21 = 0;
    DataSet ds4 = new DataSet();
    DataSet dsalldetails = new DataSet();
    double tolal = 0;
    Hashtable hatdc12 = new Hashtable();
    System.Text.StringBuilder textpass = new System.Text.StringBuilder();
    Dictionary<int, string> subno1 = new Dictionary<int, string>();
    Dictionary<int, string> colno1 = new Dictionary<int, string>();
    DataSet dsonduty = new DataSet();
    Hashtable hatodtot = new Hashtable();
    int cal_to_date, cal_to_date_tmp;
    SqlCommand cmd;
    string splhrsec = "";
    DateTime spfromdate;
    DateTime sptodate;
    string dd = "";
    string markorder = "";
    double subcont = 0;
    double subtolcont = 0;
    DateTime dumm_from_date;
    DateTime Admission_date;
    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0, tot_ml_spl_fals = 0;
    double per_leave_fals = 0;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int notconsider_value = 0;
    int split_holiday_status_1 = 0, split_holiday_status_2 = 0;
    Boolean spl_hr_flag = false;
    Hashtable has_total_onduty_hour = new Hashtable();
    DataSet ds_alter = new DataSet();
    string semstartdate123 = "";
    int count_master = 0;
    string present_calcflag = "";
    static Hashtable has_subtype = new Hashtable();
    Boolean no_stud_flag = false;
    Boolean loadtolss = false;
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    string subj_type = "";
    string group_user = "", singleuser = "", usercode = "", collegecode = "";
    int mng_hrs = 0, evng_hrs = 0;
    DateTime dt1 = new DateTime();
    DateTime dt2 = new DateTime();
    string Academicyears = "";
    string strsec = "";
    int subtolcont1 = 0;
    int subtolcont1233 = 0;
    string criteriaassment = "";
    int no_of_hrs = 0;
    string order = "";
    Hashtable stud_perccnt = new Hashtable();
    Hashtable hatabsentvalues = new Hashtable();
    Hashtable has_hs = new Hashtable();
    DataSet ds_attndmaster = new DataSet();
    string roll_no = "";
    string sem_start_date = "";
    string strDay = "", dummy_date = "", temp_hr_field = "", subject_no = "";
    string full_hour = "";
    string single_hour = "";
    Boolean recflag = false;
    Boolean holiflag = false;
    DateTime temp_date = new DateTime();
    int stud_count = 0;
    string Att_mark;
    Boolean check_alter = false;
    int span_count = 0;
    string date_temp_field = "", month_year = "";
    int present_count = 0;
    int roll_count = 0;
    DataSet dsprint = new DataSet();
    Dictionary<int, string> hval = new Dictionary<int, string>();
    Dictionary<int, string> nohoval = new Dictionary<int, string>();
    Dictionary<int, string> snoco = new Dictionary<int, string>();
    string regularflag = "";
    DAccess2 d2 = new DAccess2();
    string section_lab = "";
    static string grouporusercode = "";
    DataSet ds_sphr = new DataSet();
    static Hashtable ht_sphr = new Hashtable();
    Boolean chkflag = false;
    Hashtable has_attnd_masterset_notconsider = new Hashtable();
    Boolean splhr_flag = false;
    DAccess2 dacces2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet ds_holi = new DataSet();
    DataSet ds_optim = new DataSet();
    string atten = "";
    string Master1 = "";
    int Atday = 0, endk = 0;
    string genderflag = "";
    string strdayflag = "";
    string staff = "";
    double perofpass = 0;
    string stud_roll = "";
    DateTime date_today;
    string strorder = "";
    string strregorder = "";
    int ic = 0;
    int i;
    static int cook = 0;
    string semstartdate = "";
    string noofdays = "";
    string startday = "";
    string dateconcat = "";
    Dictionary<int, string> yeartest = new Dictionary<int, string>();
    string group_code = "", columnfield = "";
    Hashtable has_load_rollno = new Hashtable();
    Hashtable has_total_attnd_hour = new Hashtable();
    Hashtable has_total_absent_hour = new Hashtable();
    Hashtable result_has = new Hashtable();
    Hashtable hat_holy = new Hashtable();
    Hashtable has_attnd_masterset = new Hashtable();
    Hashtable temp_has_subj_code = new Hashtable();
    Hashtable over_per = new Hashtable();
    Hashtable over_per1 = new Hashtable();

    DateTime per_from_date;
    DateTime per_to_date;
    string frdate, todate;
    string value, date;
    string tempvalue = "-1";
    int ObtValue = -1;
    double per_perhrs;
    double per_leave;
    double cum_tot_point, per_holidate;
    double njhr;
    int count = 0;
    int min_mark, per_sub_count;
    double per_mark;
    int pass = 0, fail = 0;
    int mmyycount;

    DataSet ds = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds5 = new DataSet();

    Hashtable hat = new Hashtable();
    Hashtable has = new Hashtable();
    Hashtable attmaster = new Hashtable();
    DataSet attnew = new DataSet();
    DAccess2 dacc = new DAccess2();

    double tot_marks;
    double percen;
    double sub_max_marks;

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
    DataTable consolidate = new DataTable();
    DataRow drconsreport;
    string affliated = "";
    string category = "";
    int subjectcount = 0;
    int demfcal, demtcal;
    string monthcal;
    static int prevs_endrow = 0;
    Hashtable hatexamdate = new Hashtable();
    Hashtable hatcritgal = new Hashtable();
    Hashtable hatcritgal1 = new Hashtable();
    Hashtable hatsubjectd = new Hashtable();
    ArrayList a1 = new ArrayList();
    ArrayList a2 = new ArrayList();
    ArrayList a3 = new ArrayList();
    Dictionary<int, string> dicadmdate = new Dictionary<int, string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        
        lblnorecc.Visible = false;
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
            lblnorecc.Visible = false;
            if (!IsPostBack)
            {
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                lblnorecc.Visible = false;


                prevs_endrow = 0;
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Session["Sex"] = "0";
                Session["flag"] = "-1";

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
                dsprint = da.select_method("bind_college", hat, "sp");
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
                    gridview1.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    return;
                }

                Radiowithoutheader.Visible = false;
                RadioHeader.Visible = false;
                ddlpage.Visible = false;
                lblpages.Visible = false;
                // FpEntry.Sheets[0].SheetName = " ";
                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;
                gridview1.Visible = false;

                gridview1.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                lblnorecc.Visible = false;
                //  FpEntry.Sheets[0].PageSize = 10;

                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 12;
                style.Font.Bold = true;




                //FpEntry.Sheets[0].Columns[1].Width = 100;


                //FpEntry.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                //FpEntry.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                //FpEntry.Pager.Align = HorizontalAlign.Right;
                //FpEntry.Pager.Font.Bold = true;
                //FpEntry.Pager.Font.Name = "Book Antiqua";
                //FpEntry.Pager.ForeColor = Color.DarkGreen;
                //FpEntry.Pager.BackColor = Color.Beige;
                //FpEntry.Pager.BackColor = Color.AliceBlue;
                //FpEntry.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                //FpEntry.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                //FpEntry.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                //FpEntry.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                //FpEntry.Sheets[0].DefaultStyle.Font.Bold = false;
                //FpEntry.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                //FpEntry.Sheets[0].FrozenColumnCount = 4;
                //FpEntry.Sheets[0].Columns[0].Width = 70;

                //FpEntry.Pager.PageCount = 5;
                //FpEntry.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                //  FpEntry.Sheets[0].AutoPostBack = true;
                RadioButtonList3.SelectedValue = "4";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    Master1 = "select * from Master_Settings where group_code=" + Session["group_code"] + "";
                }
                else
                {
                    Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                }

                DataSet dsmaseter = dacces2.select_method(Master1, hat, "Text");
                string regularflag = "";
                if (dsmaseter.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsmaseter.Tables[0].Rows.Count; i++)
                    {
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Register No" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "sex" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Sex"] = "1";
                        }

                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "General" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {

                            Session["flag"] = 0;

                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "As Per Lesson" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {

                            Session["flag"] = 1;

                        }

                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Male" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {

                            genderflag = " and (a.sex='0'";
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Female" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            if (genderflag != "" && genderflag != "\0")
                            {
                                genderflag = genderflag + " or a.sex='1'";
                            }
                            else
                            {
                                genderflag = " and (a.sex='1'";
                            }
                        }

                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Days Scholor" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            if (strdayflag != null && strdayflag != "\0")
                            {
                                strdayflag = strdayflag + " or registration.Stud_Type='Day Scholar'";

                            }
                            else
                            {
                                strdayflag = " and (registration.Stud_Type='Day Scholar'";
                            }
                        }

                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Hostel" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            if (strdayflag != null && strdayflag != "\0")
                            {
                                strdayflag = strdayflag + " or registration.Stud_Type='Hostler'";
                            }
                            else
                            {
                                strdayflag = " and (registration.Stud_Type='Hostler'";
                            }
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Regular")
                        {
                            regularflag = "and ((registration.mode=1)";
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Lateral")
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
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Transfer")
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
                    }
                }

                if (strdayflag != null && strdayflag != "")
                {
                    strdayflag = strdayflag + ")";
                }
                Session["strvar"] = strdayflag;

                if (regularflag != "")
                {
                    regularflag = regularflag + ")";
                }
                if (genderflag != "")
                {
                    genderflag = genderflag + ")";
                }
                Session["strvar"] = Session["strvar"] + regularflag + genderflag;

                singleuser = Session["single_user"].ToString();
                group_user = Session["group_code"].ToString();
                collegecode = Session["College_Code"].ToString();
                usercode = Session["usercode"].ToString();

                bindbatch();
                binddegree();

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

            SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
            splhr_flag = false;
            setcon.Close();
            cmd.CommandText = "select rights from  special_hr_rights where usercode=" + Session["usercode"].ToString() + "";
            cmd.Connection = setcon;
            setcon.Open();
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
        ds = da.select_method("bind_sec", hat, "sp");
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
        ds = da.select_method_wo_parameter("bind_batch", "sp");
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
        }
    }

    public void binddegree()
    {
        ddlDegree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = ddlcollege.SelectedValue.ToString();
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
        ds = da.select_method("bind_degree", hat, "sp");
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
        collegecode = ddlcollege.SelectedValue.ToString();
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

        ds = da.select_method("bind_branch", hat, "sp");
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

        Control cntUpdateBtn = gridview1.FindControl("Update");
        Control cntCancelBtn = gridview1.FindControl("Cancel");
        Control cntCopyBtn = gridview1.FindControl("Copy");
        Control cntCutBtn = gridview1.FindControl("Clear");
        Control cntPasteBtn = gridview1.FindControl("Paste");
        Control cntPagePrintBtn = gridview1.FindControl("Print");

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
            string SyllabusYr;
            string SyllabusQry;
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
            SyllabusYr = da.GetFunction(SyllabusQry.ToString());
            collegecode = ddlcollege.SelectedValue.ToString();
            string Sqlstr43;
            Sqlstr43 = "";
            Sqlstr43 = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by criteria";
            ds2 = d2.select_method_wo_parameter(Sqlstr43, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlTest.Items.Clear();
                ddlTest.DataSource = ds2;
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

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblnorec.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        gridview1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        lblnorecc.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        ddlpage.Visible = false;
        lblpages.Visible = false;
        Button2.Visible = false;
        ddlTest.Items.Clear();
        ddlBranch.Items.Clear();

        string collegecode = ddlcollege.SelectedValue.ToString();
        string usercode = Session["usercode"].ToString();
        string course_id = ddlDegree.SelectedValue.ToString();

        bindbranch();
        bindsem();
        bindsec();
        GetTest();
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblnorec.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        gridview1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        lblnorecc.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        ddlpage.Visible = false;
        lblpages.Visible = false;
        Button2.Visible = false;
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
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }

    public void BindSectionDetail()
    {
        try
        {
            string branch = ddlBranch.SelectedValue.ToString();
            string batch = ddlBatch.SelectedValue.ToString();
            string getdeteails = "select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";
            DataSet dssem = d2.select_method_wo_parameter(getdeteails, "Text");
            int count5 = dssem.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                ddlSec.DataSource = dssem;
                ddlSec.DataTextField = "sections";
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
            }
            else
            {
                ddlSec.Enabled = false;
            }
        }
        catch
        {

        }
    }
    public void bindsem()
    {
        ddlSemYr.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        string getdeteails = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.Text.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + ddlcollege.SelectedValue.ToString() + "";
        DataSet dssem = d2.select_method_wo_parameter(getdeteails, "Text");
        if (dssem.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(dssem.Tables[0].Rows[0]["first_year_nonsemester"].ToString());
            duration = Convert.ToInt16(dssem.Tables[0].Rows[0]["ndurations"].ToString());
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
            ddlSemYr.Items.Clear();
            string getdeteails1 = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + ddlcollege.SelectedValue.ToString() + "";
            DataSet dssem11 = d2.select_method_wo_parameter(getdeteails1, "Text");
            if (dssem11.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(dssem11.Tables[0].Rows[0]["first_year_nonsemester"].ToString());
                duration = Convert.ToInt16(dssem11.Tables[0].Rows[0]["duration"].ToString());

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

        }
    }
    public void Get_Semester()
    {
        Boolean first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;
        string batch = ddlBatch.SelectedValue.ToString();
        string collegecode = ddlcollege.SelectedValue.ToString();
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

    public void findholy()
    {
        hat.Clear();
        hat.Add("date_val", date_today);
        hat.Add("degree_code", ddlBranch.SelectedValue.ToString());
        hat.Add("sem_val", ddlSemYr.SelectedValue.ToString());
        ds_holi = da.select_method("holiday_sp", hat, "sp");
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

    private string Splitter(string p, string p_2)
    {
        throw new NotImplementedException();
    }
    private static void AddTableColumn(DataTable resultsTable, StringBuilder ColumnName)
    {
        try
        {
            DataColumn tableCol = new DataColumn(ColumnName.ToString());
            resultsTable.Columns.Add(tableCol);
        }
        catch (System.Data.DuplicateNameException)
        {
            ColumnName.Append(" ");
            AddTableColumn(resultsTable, ColumnName);
        }
    }
    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblnorec.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        gridview1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        lblnorecc.Visible = false;
        lblnorec.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        ddlpage.Visible = false;
        lblpages.Visible = false;
        Button2.Visible = false;
        ddlTest.Items.Clear();
        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }
        BindSectionDetail();
        GetTest();
    }
    public void filteration()
    {

        string orderby_Setting = da.GetFunction("select value from master_Settings where settings='order_by'");

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
        gridview1.Visible = false;
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
            if (ddlTest.Items.Count > 0 && ddlTest.SelectedItem.ToString() != "--Select--")
            {
                buttonGo();
            }
            else
            {
                gridview1.Visible = false;
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select Test";
            }

        }


    }
    protected void buttonGo()
    {
        try
        {

            btnExcel.Visible = false;
            btnPrintMaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            Button2.Visible = false;

            gridview1.Visible = true;
            consolidate.Columns.Add("SNo", typeof(string));
            consolidate.Columns.Add("Roll No", typeof(string));
            consolidate.Columns.Add("Name", typeof(string));

            a1.Add("SNo");
            a1.Add("Roll No");
            a1.Add("Name");

            a2.Add("SNo");
            a2.Add("Roll No");
            a2.Add("Name");

            a3.Add("SNo");
            a3.Add("Roll No");
            a3.Add("Name");



            string yrsemm = "1";
            int count6 = 0;
            int K = 0;
            DataSet dsbatch = new DataSet();
            DataSet dsbatch6 = new DataSet();
            int cnt1 = 0;
            Boolean headflag = false;
            string year = "";
            string yr = "";

            //string strorder12 = "ORDER BY registration.Reg_No";
            querystring = "select distinct p.No_of_hrs_per_day as 'PER DAY',schorder as 'order',convert(nvarchar(15),start_date,101) AS start_date1,start_date,no_of_hrs_I_half_day,no_of_hrs_II_half_day,len(a.roll_no), convert(varchar(15),adm_date,103) as adm_date,registration.serialno  FROM attendance a , registration , Department d ,  PeriodAttndSchedule p  ,seminfo s,applyn app WHERE a.roll_no=registration.roll_no and   registration.degree_code=p.degree_code and  registration.Batch_Year=" + ddlBatch.SelectedValue.ToString() + "  and  s.batch_Year=" + ddlBatch.SelectedValue.ToString() + "  and registration.degree_code= " + ddlBranch.SelectedValue.ToString() + " and s.degree_code= " + ddlBranch.SelectedValue.ToString() + " and  s.semester=" + ddlSemYr.SelectedValue.ToString() + " and p.semester=" + ddlSemYr.SelectedValue.ToString() + "  and (registration.CC = 0)  AND (registration.DelFlag = 0)  AND (registration.Exam_Flag <> 'debar') AND (registration.Current_Semester IS NOT NULL) and  registration.app_no=app.app_no " + strsec + "  ";
            ds_student = da.select_method(querystring, hat, "Text");
            stud_count = ds_student.Tables[0].Rows.Count;

            if (stud_count > 0)
            {
                no_of_hrs = int.Parse(ds_student.Tables[0].Rows[0]["PER DAY"].ToString());
                mng_hrs = int.Parse(ds_student.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString());
                evng_hrs = int.Parse(ds_student.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString());
                order = ds_student.Tables[0].Rows[0]["order"].ToString();
                sem_start_date = ds_student.Tables[0].Rows[0]["start_date1"].ToString();
            }




            DataView dv_data = new DataView();
            DataSet ds456 = new DataSet();
            DataSet dsroll = new DataSet();
            DataSet dssem = new DataSet();
            string deptcode1 = ddlBranch.SelectedValue.ToString();
            string batch1 = ddlBatch.SelectedItem.Text;
            string sections = "";
            if (ddlSec.Enabled == true)
            {
                sections = ddlSec.SelectedItem.Text;
            }
            string tess = ddlTest.SelectedValue.ToString();
            string collegecode1 = ddlcollege.SelectedValue.ToString();
            string collegecode = ddlcollege.SelectedValue.ToString();
            string str_sec = "";
            string filterwithoutsection = "";
            string filterwithsection = "";
            if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1" || sections.ToString() == "")
            {
                strsec = "";
                str_sec = "";
            }
            else
            {
                strsec = sections.ToString();
                str_sec = " and sections='" + sections.ToString() + "'";
            }
            string examstartdate = sem_start_date;
            string strquery = "select distinct convert(nvarchar(15),MAX(e.exam_date),101) AS exam_date,c.criteria,c.criteria_no from    criteriaforinternal c,syllabus_master sm,exam_type e where  c.syll_code=sm.syll_code and e.criteria_no = c.Criteria_no and sm.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and sm.semester='" + ddlSemYr.SelectedValue.ToString() + "' and    sm.batch_year='" + ddlBatch.SelectedValue.ToString() + "' " + str_sec + "  GROUP BY C.criteria_no,C.criteria  order by exam_date ;   select distinct convert(nvarchar(15),e.exam_date,101) AS exam_date,c.criteria,c.criteria_no from    criteriaforinternal c,syllabus_master sm,exam_type e where  c.syll_code=sm.syll_code and e.criteria_no = c.Criteria_no and c.Criteria_no='" + ddlTest.SelectedValue.ToString() + "' and sm.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and sm.semester='" + ddlSemYr.SelectedValue.ToString() + "' and    sm.batch_year='" + ddlBatch.SelectedValue.ToString() + "' " + str_sec + "   order by exam_date";
            DataSet dsexam = da.select_method_wo_parameter(strquery, "text");
            for (int e = 0; e < dsexam.Tables[0].Rows.Count; e++)
            {
                string criterai = dsexam.Tables[0].Rows[e]["criteria_no"].ToString();
                string examendate = dsexam.Tables[0].Rows[e]["exam_date"].ToString();
                hatexamdate.Add(criterai, examstartdate + ';' + examendate);
                hatcritgal.Add(criterai, e);
                hatcritgal1.Add(e, criterai);
                examstartdate = examendate;
            }
            string strquery1 = " select distinct convert(nvarchar(15),e.exam_date,101) AS exam_date,c.criteria,c.criteria_no,e.subject_no  from    criteriaforinternal c,syllabus_master sm,exam_type e where  c.syll_code=sm.syll_code and e.criteria_no = c.Criteria_no and c.Criteria_no='" + ddlTest.SelectedValue.ToString() + "' and sm.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and sm.semester='" + ddlSemYr.SelectedValue.ToString() + "' and    sm.batch_year='" + ddlBatch.SelectedValue.ToString() + "' " + str_sec + "   order by exam_date";
            DataSet dsexam1 = da.select_method_wo_parameter(strquery1, "text");
            for (int e = 0; e < dsexam1.Tables[0].Rows.Count; e++)
            {
                string subjectf = dsexam1.Tables[0].Rows[e]["subject_no"].ToString();
                string examendate = dsexam1.Tables[0].Rows[e]["exam_date"].ToString();
                hatsubjectd.Add(subjectf, examendate);
            }

            filteration();
            filterwithsection = "a.app_no=r.app_no and r.degree_code='" + deptcode1.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + batch1.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + tess.ToString() + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0   and r.sections='" + strsec.ToString() + "' ORDER BY r.Roll_No";
            filterwithoutsection = "a.app_no=r.app_no and r.degree_code='" + deptcode1.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + batch1.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + tess.ToString() + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0   ORDER BY r.Roll_No";

            hat.Clear();
            hat.Add("batchyear", batch1.ToString());
            hat.Add("degreecode", deptcode1.ToString());
            hat.Add("criteria_no", tess.ToString());
            hat.Add("sections", strsec.ToString());
            hat.Add("filterwithsection", filterwithsection.ToString());
            hat.Add("filterwithoutsection", filterwithoutsection.ToString());

            ds2 = d2.select_method("PROC_STUD_ALLSUBMARK", hat, "sp");

            count6 = ds2.Tables[1].Rows.Count;
            if (ds2.Tables[1].Rows.Count > 0)
            {
                //filteration();
                //string filterwithsectionsub = "a.app_no=r.app_no and r.degree_code='" + deptcode1.ToString() + "' and r.batch_year='" + batch1.ToString() + "'   and RollNo_Flag<>0 and cc=0 and  d.degree_code=r.degree_code and c.course_id=d.course_id and exam_flag <> 'DEBAR' and delflag=0   and sections='" + strsec.ToString() + "' and r.degree_code=a.degree_code  and ((r.mode=1) or (r.mode=3) or (r.mode=2))and ((r.mode=1) or (r.mode=3) or (r.mode=2)) " + strorder.ToString() + "  ";
                //string filterwithoutsectionsub = "a.app_no=r.app_no and r.degree_code='" + deptcode1.ToString() + "' and r.batch_year='" + batch1.ToString() + "'   and RollNo_Flag<>0 and cc=0 and  d.degree_code=r.degree_code and c.course_id=d.course_id and exam_flag <> 'DEBAR' and delflag=0 and r.degree_code=a.degree_code  and ((r.mode=1) or (r.mode=3) or (r.mode=2))and ((r.mode=1) or (r.mode=3) or (r.mode=2)) " + strorder.ToString() + "  ";
                //hat.Clear();
                //hat.Add("bath_year", batch1.ToString());
                //hat.Add("degree_code", deptcode1.ToString());
                //hat.Add("sec", strsec.ToString());
                //hat.Add("filterwithsectionsub", filterwithsectionsub.ToString());
                //hat.Add("filterwithoutsectionsub", filterwithoutsectionsub.ToString());
                //ds5 = d2.select_method("SELECT _ALL_STUDENT_CAM_REPORTS_DETAILS", hat, "sp");
                int columncnt = 3;
                for (int n = 0; n < ds2.Tables[1].Rows.Count; n++)
                {
                    subcont = 0;

                
                    subno1.Add(columncnt, ds2.Tables[1].Rows[n]["subject_code"].ToString());
                    colno1.Add(columncnt, ds2.Tables[1].Rows[n]["subject_no"].ToString());
                    int val = consolidate.Columns.Count;


                    a1.Add("TOTAL NO OF HOURS CONDUCTED(H) & SUBJECTS - WISE MARK(M)");

                    a2.Add(ds2.Tables[1].Rows[n]["subject_code"].ToString());


                    a3.Add("H (" + subcont + ")");

                    textpass = new System.Text.StringBuilder("H (" + subcont + ")");

                    AddTableColumn(consolidate, textpass);

                    a1.Add("TOTAL NO OF HOURS CONDUCTED(H) & SUBJECTS - WISE MARK(M)");

                    a2.Add(ds2.Tables[1].Rows[n]["subject_code"].ToString());


                   
                    string minmark = ds2.Tables[1].Rows[n]["min_mark"].ToString();
                    textpass = new System.Text.StringBuilder("M (" + minmark + ")");

                    AddTableColumn(consolidate, textpass);
                    a3.Add("M (" + minmark + ")");
                    if (chkretest.Checked == true)
                    {
                        a1.Add("TOTAL NO OF HOURS CONDUCTED(H) & SUBJECTS - WISE MARK(M)");

                        a2.Add(ds2.Tables[1].Rows[n]["subject_code"].ToString());


                        a3.Add("Re - M (" + minmark + ")");
                        textpass = new System.Text.StringBuilder("Re - M (" + minmark + ")");
                        string col = textpass.ToString();
                        AddTableColumn(consolidate, textpass);
                        columncnt++;
                        cnt1++;
                    }

                    columncnt = columncnt + 2;

                    cnt1 = cnt1 + 2;
                }
                DataRow drHdr1 = consolidate.NewRow();
                DataRow drHdr2 = consolidate.NewRow();
                DataRow drHdr3 = consolidate.NewRow();
                for (int grCol = 0; grCol < consolidate.Columns.Count; grCol++)
                {
                    drHdr1[grCol] = a1[grCol];
                    drHdr2[grCol] = a2[grCol];
                    drHdr3[grCol] = a3[grCol];
                }
                consolidate.Rows.Add(drHdr1);
                consolidate.Rows.Add(drHdr2);
                consolidate.Rows.Add(drHdr3);
            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "No Records Found";
                Button2.Visible = false;
                gridview1.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                btnPrintMaster.Visible = false;
                return;
            }


            if (ds2.Tables[0].Rows.Count > 0)
            {
                int SNo = 0;
                int c = 0;
                string temp = "";
                int rowcnt = -1;
                for (int irow = 0; irow < ds2.Tables[0].Rows.Count; irow++)
                {

                    if (temp != ds2.Tables[0].Rows[irow]["RollNumber"].ToString().Trim().ToLower())
                    {
                        drconsreport = consolidate.NewRow();
                        SNo++;
                        drconsreport["SNo"] = Convert.ToString(SNo);
                        if ((SNo % 2) == 0)
                        {
                            int row=consolidate.Rows.Count;
                            snoco.Add(row, "sno");
                        }
                        temp = ds2.Tables[0].Rows[irow]["RollNumber"].ToString().Trim().ToLower();

                        rowcnt++;
                        drconsreport["Roll No"] = ds2.Tables[0].Rows[irow]["RollNumber"].ToString();
                        dicadmdate.Add(rowcnt, ds2.Tables[0].Rows[irow]["adm_date"].ToString());
                        // drconsreport["adm_date"] = ds2.Tables[0].Rows[irow]["adm_date"].ToString();
                        drconsreport["Name"] = ds2.Tables[0].Rows[irow]["Student_Name"].ToString();

                        consolidate.Rows.Add(drconsreport);

                        if (!has_load_rollno.Contains(temp.ToString()))
                        {
                            has_load_rollno.Add(ds2.Tables[0].Rows[irow]["RollNumber"].ToString().ToLower(), 0);
                            has_total_attnd_hour.Add(ds2.Tables[0].Rows[irow]["RollNumber"].ToString().ToLower(), 0);
                            has_total_absent_hour.Add(ds2.Tables[0].Rows[irow]["RollNumber"].ToString().ToLower(), 0);
                        }
                    }
                }
                string splhrsec = "";
                string rstrsec = "";
                if (ddlSec.SelectedValue.ToString() == "" || ddlSec.SelectedValue.ToString() == "-1")
                {
                    strsec = "";

                    splhrsec = "";
                }
                else
                {
                    strsec = " and sections='" + ddlSec.SelectedItem.ToString() + "'";
                    rstrsec = " and r.sections='" + ddlSec.SelectedItem.ToString() + "'";
                    splhrsec = "and sections='" + ddlSec.SelectedItem.ToString() + "'";
                }
                string stralldetaisquery = "select r.roll_no,s.subject_no,s.batch,r.adm_date from registration r,subjectchooser s where s.roll_no=r.roll_no and r.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "' " + rstrsec + "";
                stralldetaisquery = stralldetaisquery + " ;select r.roll_no,s.subject_no,s.batch,r.adm_date from registration r,subjectchooser_new s where s.roll_no=r.roll_no and r.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "'  " + rstrsec + "";
                stralldetaisquery = stralldetaisquery + " ;select day_value,hour_value,stu_batch,subject_no,timetablename from laballoc where batch_year='" + ddlBatch.SelectedValue.ToString() + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "'  " + strsec + "";
                stralldetaisquery = stralldetaisquery + " ;select day_value,hour_value,stu_batch,subject_no,fdate from laballoc_new where batch_year='" + ddlBatch.SelectedValue.ToString() + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' " + strsec + "";
                stralldetaisquery = stralldetaisquery + " ;select a.* from attendance a,registration r where a.roll_no=r.roll_no and r.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "' " + rstrsec + "";
                stralldetaisquery = stralldetaisquery + " ;select a.* from attendance_withreason a,registration r where a.roll_no=r.roll_no and r.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "' " + rstrsec + "";
                stralldetaisquery = stralldetaisquery + " ;select * from Semester_Schedule where batch_year='" + ddlBatch.SelectedValue.ToString() + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and semester='" + ddlSemYr.SelectedItem.ToString() + "'  " + strsec + " order by FromDate desc";
                stralldetaisquery = stralldetaisquery + " ;select * from Alternate_Schedule where batch_year='" + ddlBatch.SelectedValue.ToString() + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and semester='" + ddlSemYr.SelectedItem.ToString() + "'  " + strsec + "  order by FromDate desc";
                dsalldetails = d2.select_method_wo_parameter(stralldetaisquery, "Text");
                loadtolss = false;
                load_attendance();

                if (loadtolss != true)
                {
                    loadmarkss();

                    int notext = 0;
                    if (hatcritgal.Contains(ddlTest.SelectedValue.ToString()))
                    {
                        notext = Convert.ToInt16(GetCorrespondingKey(ddlTest.SelectedValue.ToString(), hatcritgal));
                    }
                    // string getdeteails1 = " select  convert(nvarchar(15),MAX(e.exam_date),101) AS exam_date,c.criteria,c.criteria_no from   criteriaforinternal c,syllabus_master sm,exam_type e where    c.syll_code=sm.syll_code and    e.criteria_no = c.Criteria_no   and sm.degree_code='" + ddlBranch.SelectedValue.ToString() + "'    and sm.semester='" + ddlSemYr.SelectedItem.ToString() + "'   and  e.exam_date between '" + semstartdate + "' and '" + semstartdate123 + "' and  sm.batch_year='" + ddlBatch.Text.ToString() + "'  GROUP BY C.criteria_no,C.criteria order by e.exam_date asc";
                    string getdeteails1 = " select  convert(nvarchar(15),MAX(e.exam_date),101) AS exam_date,c.criteria,c.criteria_no from   criteriaforinternal c,syllabus_master sm,exam_type e where    c.syll_code=sm.syll_code and    e.criteria_no = c.Criteria_no   and sm.degree_code='" + ddlBranch.SelectedValue.ToString() + "'    and sm.semester='" + ddlSemYr.SelectedItem.ToString() + "'  and  sm.batch_year='" + ddlBatch.Text.ToString() + "'  GROUP BY C.criteria_no,C.criteria order by exam_date asc";
                    DataSet dssem25 = d2.select_method_wo_parameter(getdeteails1, "Text");
                    int rowpedcont = 0;
                    int rowpedcont1 = 0;
                    if (notext > 0)
                    {
                        rowpedcont1 = notext;
                        rowpedcont = notext;
                        rowpedcont = rowpedcont + 2;
                        subtolcont1 = Convert.ToInt16(subtolcont);
                        consolidate.Columns.Add("NO OF HOURS OUT OF (" + subtolcont + ")", typeof(string));
                        consolidate.Rows[0][consolidate.Columns.Count - 1] = "NO OF HOURS OUT OF (" + subtolcont + ")";
                        consolidate.Rows[1][consolidate.Columns.Count - 1] = "NO OF HOURS OUT OF (" + subtolcont + ")";
                        consolidate.Rows[2][consolidate.Columns.Count - 1] = "NO OF HOURS OUT OF (" + subtolcont + ")";
                        consolidate.Columns.Add("%", typeof(string));
                        consolidate.Rows[0][consolidate.Columns.Count - 1] = "%";
                        consolidate.Rows[1][consolidate.Columns.Count - 1] = "%";
                        consolidate.Rows[2][consolidate.Columns.Count - 1] = "%";
                        int PRODS = 0;

                        criteriaassment = ddlTest.SelectedItem.Text;
                        for (int proid = 0; proid < rowpedcont1; proid++)
                        {
                            subtolcont1 = 0;
                            PRODS++;
                            if (criteriaassment != "")
                            {
                                criteriaassment = criteriaassment + ", PERIOD - " + PRODS + "(" + dssem25.Tables[0].Rows[proid]["criteria"].ToString() + ")";
                            }
                            else
                            {
                                criteriaassment = dssem25.Tables[0].Rows[proid]["criteria"].ToString();
                            }
                            Session["perioftest"] = "ASSESMENT PERIOD  : " + criteriaassment;

                            fdate = "";
                            tdate = "";

                            int rr = proid;
                            notext = Convert.ToInt16(GetCorrespondingKey(rr, hatcritgal1));

                            string datesp = Convert.ToString(GetCorrespondingKey(notext, hatexamdate));
                            string[] spdate = datesp.Split(';');
                            if (spdate.GetUpperBound(0) >= 1)
                            {
                                fdate = spdate[0];
                                tdate = spdate[1];
                            }

                            string[] dm_splt_new = fdate.ToString().Split('/');
                            string[] date_increment_splt_new = tdate.ToString().Split('/');
                            DateTime alt;
                            from_date = Convert.ToDateTime(dm_splt_new[0].ToString() + "/" + dm_splt_new[1].ToString() + "/" + dm_splt_new[2].ToString());
                            to_date = Convert.ToDateTime(date_increment_splt_new[0].ToString() + "/" + date_increment_splt_new[1].ToString() + "/" + date_increment_splt_new[2].ToString());
                            t_date = to_date;
                            f_date = from_date;
                            dt1 = from_date;
                            dt2 = to_date;

                            has.Clear();
                            has.Add("from_date", f_date);
                            has.Add("to_date", t_date);
                            has.Add("degree_code", ddlBranch.SelectedValue.ToString());
                            has.Add("sem", ddlSemYr.SelectedValue.ToString());
                            has.Add("coll_code", Session["collegecode"].ToString());

                            int iscount = 0;

                            string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + dt1.ToString() + "' and '" + dt2.ToString() + "' and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedItem.ToString() + "";

                            DataSet dsholiday = new DataSet();
                            dsholiday = da.select_method(sqlstr_holiday, hat, "Text");

                            if (dsholiday.Tables[0].Rows.Count > 0)
                            {
                                iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                            }
                            has.Add("iscount", iscount);
                            ds_holi = dacc.select_method("HOLIDATE_DETAILS_FINE", has, "sp");

                            if (ds_holi.Tables[0].Rows.Count > 0)
                            {
                                for (int holi = 0; holi < ds_holi.Tables[0].Rows.Count; holi++)
                                {

                                    if (ds_holi.Tables[0].Rows[holi]["halforfull"].ToString() == "False")
                                    {
                                        halforfull = "0";
                                    }
                                    else
                                    {
                                        halforfull = "1";
                                    }
                                    if (ds_holi.Tables[0].Rows[holi]["morning"].ToString() == "False")
                                    {
                                        mng = "0";
                                    }
                                    else
                                    {
                                        mng = "1";
                                    }
                                    if (ds_holi.Tables[0].Rows[holi]["evening"].ToString() == "False")
                                    {
                                        evng = "0";
                                    }
                                    else
                                    {
                                        evng = "1";
                                    }

                                    holiday_sched_details = halforfull + "*" + mng + "*" + evng;
                                    if (!hat_holy.ContainsKey(Convert.ToDateTime(ds_holi.Tables[0].Rows[holi]["HOLI_DATE"].ToString())))
                                    {
                                        hat_holy.Add(Convert.ToDateTime(ds_holi.Tables[0].Rows[holi]["HOLI_DATE"].ToString()), holiday_sched_details);
                                    }
                                }
                            }
                            Hashtable hatattendance = new Hashtable();

                            ht_sphr.Clear();

                            if (ddlSec.Enabled == true)
                            {
                                if (ddlSec.SelectedValue.ToString() != "" && ddlSec.SelectedValue.ToString() != "-1")
                                {
                                    splhrsec = "and sections='" + ddlSec.SelectedItem.ToString() + "'";
                                }
                            }
                            else
                            {
                                splhrsec = "";
                            }
                            string hrdetno = "";
                            string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " " + splhrsec + " and date between '" + dt1.ToString() + "' and '" + dt2.ToString() + "'";
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
                            has_load_rollno.Clear();
                            has_total_attnd_hour.Clear();
                            has_total_onduty_hour.Clear();
                            string subno_val = "";
                            int less70perc = 0;
                            double sub_prc = 0;
                            int rcnt = 0;
                            if (consolidate.Columns.Count > 4)
                            {
                                int comy = 0;

                                for (int colcnt = 3; colcnt <= consolidate.Columns.Count - 3; colcnt++)
                                {
                                    rcnt = 0;
                                    sub_prc = 0;
                                    less70perc = 0;
                                    roll_count = 0;
                                    present_count = 0;
                                    temp_hr_field = "";
                                    temp_date = dt1;
                                    dt2 = dt2;
                                    onduty = 0;

                                    if (colno1.ContainsKey(colcnt))//ko
                                    {
                                        subno_val = colno1[colcnt];

                                        if (subno_val.Trim().ToString() != "")
                                        {

                                            subject_no = subno_val;
                                            while (temp_date <= dt2)
                                            {
                                                if (!hatdc12.Contains(temp_date))
                                                {
                                                    if (splhr_flag == true)
                                                    {
                                                        if (ht_sphr.Contains(Convert.ToString(temp_date)))
                                                        {
                                                            getspecial_hr();
                                                        }
                                                    }

                                                    span_count = 0;

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
                                                        //temp_date = temp_date.AddDays(1);
                                                    }
                                                    else
                                                    {

                                                        holiflag = true;

                                                        dsalldetails.Tables[7].DefaultView.RowFilter = "degree_code = " + ddlBranch.SelectedValue.ToString() + " and semester = " + ddlSemYr.SelectedItem.ToString() + " and batch_year = " + ddlBatch.SelectedValue.ToString() + " and FromDate ='" + temp_date + "' " + strsec + "";
                                                        DataView dvaltersech = dsalldetails.Tables[7].DefaultView;

                                                        dsalldetails.Tables[6].DefaultView.RowFilter = "degree_code = " + ddlBranch.SelectedValue.ToString() + " and semester = " + ddlSemYr.SelectedItem.ToString() + " and batch_year = " + ddlBatch.SelectedValue.ToString() + " and FromDate <='" + temp_date + "' " + strsec + "";
                                                        DataView dvsemsech = dsalldetails.Tables[6].DefaultView;

                                                        hatattendance.Clear();
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
                                                                    Boolean samehr_flag = false;
                                                                    roll_count = 0;
                                                                    present_count = 0;
                                                                    temp_hr_field = strDay + temp_hr;
                                                                    date_temp_field = "d" + final_date_string[1].ToString() + "d" + temp_hr;
                                                                    hatattendance.Clear();
                                                                    if (dvaltersech.Count > 0)
                                                                    {
                                                                        for (int hasrow = 0; hasrow < dvaltersech.Count; hasrow++)
                                                                        {
                                                                            full_hour = dvaltersech[hasrow][temp_hr_field].ToString();
                                                                            hatattendance.Clear();
                                                                            if (full_hour.Trim() != "")
                                                                            {
                                                                                temp_has_subj_code.Clear();
                                                                                string[] split_full_hour = full_hour.Split(';');
                                                                                for (int semi_colon = 0; semi_colon <= split_full_hour.GetUpperBound(0); semi_colon++)
                                                                                {
                                                                                    roll_count = 0;
                                                                                    single_hour = split_full_hour[semi_colon].ToString();
                                                                                    string[] split_single_hour = single_hour.Split('-');
                                                                                    if (split_single_hour.GetUpperBound(0) >= 1)
                                                                                    {
                                                                                        check_alter = true;
                                                                                        if (split_single_hour[0].ToString() == subject_no)
                                                                                        {

                                                                                            if (!temp_has_subj_code.ContainsKey(subject_no))
                                                                                            {
                                                                                                temp_has_subj_code.Add(subject_no, subject_no);
                                                                                                recflag = true;
                                                                                                roll_count = 0;

                                                                                                if (samehr_flag == false)
                                                                                                {
                                                                                                    span_count++;
                                                                                                    samehr_flag = true;
                                                                                                }
                                                                                                Hashtable has_stud_list = new Hashtable();

                                                                                                string subj_type = "";
                                                                                                if (!hatdclab.Contains(subject_no))
                                                                                                {
                                                                                                    subj_type = hatdclab[subject_no].ToString();
                                                                                                }
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
                                                                                                            if (!hatattendance.Contains(rollno.ToString().Trim().ToLower()))
                                                                                                            {
                                                                                                                hatattendance.Add(rollno.ToString().Trim().ToLower(), attval);
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
                                                                                                                    if (!hatattendance.Contains(rollno.ToString().Trim().ToLower()))
                                                                                                                    {
                                                                                                                        hatattendance.Add(rollno.ToString().Trim().ToLower(), attval);
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                if (hatattendance.Count > 0)
                                                                                                {
                                                                                                    for (int i = 0; i < consolidate.Rows.Count; i++)
                                                                                                    {
                                                                                                        string rollno = consolidate.Rows[i][1].ToString().Trim().ToLower();
                                                                                                        if (hatattendance.Contains(rollno.ToString()))
                                                                                                        {
                                                                                                            no_stud_flag = true;

                                                                                                            if (dicadmdate.ContainsKey(i))
                                                                                                            {
                                                                                                                string sdmdates = dicadmdate[i];
                                                                                                                string[] fromdatespit99 = sdmdates.ToString().Split('/');
                                                                                                                Admission_date = Convert.ToDateTime(fromdatespit99[1] + '/' + fromdatespit99[0] + '/' + fromdatespit99[2]);

                                                                                                                string attvalue = GetCorrespondingKey(rollno, hatattendance).ToString();
                                                                                                                value = Attmark(attvalue.ToString());
                                                                                                                if (temp_date >= Admission_date)
                                                                                                                {
                                                                                                                    if (consolidate.Rows[i][consolidate.Columns.Count - 1] == "HS")
                                                                                                                    {
                                                                                                                        if (!has_hs.ContainsKey((consolidate.Columns.Count - 1)))
                                                                                                                        {
                                                                                                                            has_hs.Add((consolidate.Columns.Count - 1), (consolidate.Rows[i][consolidate.Columns.Count - 1]));
                                                                                                                        }
                                                                                                                    }
                                                                                                                    if ((attvalue.ToString()) != "8")
                                                                                                                    {
                                                                                                                        if (value != "HS")
                                                                                                                        {
                                                                                                                            if (!has_attnd_masterset_notconsider.ContainsKey(attvalue.ToString()))
                                                                                                                            {
                                                                                                                                if (has_attnd_masterset.ContainsKey(attvalue.ToString()))
                                                                                                                                {
                                                                                                                                    string getval = Convert.ToString(GetCorrespondingKey(attvalue, has_attnd_masterset));

                                                                                                                                    if (getval.ToString() == "0" || getval.ToString() == "3")
                                                                                                                                    {
                                                                                                                                        present_count = Convert.ToInt16(GetCorrespondingKey(consolidate.Rows[i][1].ToString().Trim().ToLower(), has_load_rollno));
                                                                                                                                        present_count++;
                                                                                                                                        has_load_rollno[consolidate.Rows[i][1].ToString().ToLower().Trim()] = present_count;
                                                                                                                                    }

                                                                                                                                }
                                                                                                                                if (value != "NE")
                                                                                                                                {
                                                                                                                                    present_count = Convert.ToInt16(GetCorrespondingKey(consolidate.Rows[i][1].ToString().ToLower().Trim(), has_total_attnd_hour));
                                                                                                                                    present_count++;
                                                                                                                                    has_total_attnd_hour[consolidate.Rows[i][1].ToString().ToLower().Trim()] = present_count;
                                                                                                                                }
                                                                                                                                if (attvalue.ToString() == "3")
                                                                                                                                {
                                                                                                                                    if (!hatodtot.Contains(rollno.ToLower()))
                                                                                                                                    {
                                                                                                                                        hatodtot.Add(rollno.ToLower(), "1");
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        double getods = Convert.ToDouble(GetCorrespondingKey(rollno, hatodtot));
                                                                                                                                        getods = getods + 1;
                                                                                                                                        hatodtot[rollno.ToLower()] = getods;
                                                                                                                                    }
                                                                                                                                }
                                                                                                                                if (hatabsentvalues.ContainsKey(attvalue.ToString()))
                                                                                                                                {
                                                                                                                                    double getads = Convert.ToDouble(GetCorrespondingKey(rollno.ToLower(), has_total_absent_hour));
                                                                                                                                    getads = getads + 1;
                                                                                                                                    has_total_absent_hour[rollno.ToLower()] = getads;
                                                                                                                                }
                                                                                                                            }

                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                        else
                                                                                                        {
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
                                                                    samehr_flag = false;
                                                                    roll_count = 0;
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
                                                                                roll_count = 0;
                                                                                single_hour = split_full_hour_sem[semi_colon].ToString();
                                                                                string[] split_single_hour = single_hour.Split('-');

                                                                                if (split_single_hour.GetUpperBound(0) >= 1)
                                                                                {
                                                                                    if (split_single_hour[0].ToString() == subject_no)
                                                                                    {
                                                                                        if (!temp_has_subj_code.ContainsKey(subject_no))
                                                                                        {
                                                                                            temp_has_subj_code.Add(subject_no, subject_no);
                                                                                            recflag = true;

                                                                                            if (samehr_flag == false)
                                                                                            {
                                                                                                span_count++;
                                                                                                samehr_flag = true;
                                                                                            }
                                                                                            Hashtable has_stud_list = new Hashtable();

                                                                                            if (!hatdclab.Contains(subject_no))
                                                                                            {
                                                                                                subj_type = hatdclab[subject_no].ToString();
                                                                                            }
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
                                                                                                        if (!hatattendance.Contains(rollno.ToString().Trim().ToLower()))
                                                                                                        {
                                                                                                            hatattendance.Add(rollno.ToString().Trim().ToLower(), attval);
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
                                                                                                                if (!hatattendance.Contains(rollno.ToString().Trim().ToLower()))
                                                                                                                {
                                                                                                                    hatattendance.Add(rollno.ToString().Trim().ToLower(), attval);
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }

                                                                                            }

                                                                                            if (hatattendance.Count > 0)
                                                                                            {
                                                                                                for (int i = 0; i < consolidate.Rows.Count; i++)
                                                                                                {
                                                                                                    string rollno = consolidate.Rows[i][1].ToString().Trim().ToLower();
                                                                                                    if (hatattendance.Contains(rollno.ToString()))
                                                                                                    {
                                                                                                        no_stud_flag = true;
                                                                                                        // Admission_date = Convert.ToDateTime(FpEntry.Sheets[0].Cells[i, 1].Note.Trim());
                                                                                                        if (dicadmdate.ContainsKey(i))
                                                                                                        {
                                                                                                            string sdmdates = dicadmdate[i];
                                                                                                            string[] fromdatespit992 = sdmdates.ToString().Split('/');
                                                                                                            Admission_date = Convert.ToDateTime(fromdatespit992[1] + '/' + fromdatespit992[0] + '/' + fromdatespit992[2]);

                                                                                                            string attvalue = GetCorrespondingKey(rollno, hatattendance).ToString();
                                                                                                            value = Attmark(attvalue.ToString());
                                                                                                            if (temp_date >= Admission_date)
                                                                                                            {
                                                                                                                //consolidate.Rows[i][0] = true;
                                                                                                                if (consolidate.Rows[i][consolidate.Columns.Count - 1] == "HS")
                                                                                                                {
                                                                                                                    if (!has_hs.ContainsKey((consolidate.Columns.Count - 1)))
                                                                                                                    {
                                                                                                                        has_hs.Add((consolidate.Columns.Count - 1), (consolidate.Columns.Count - 1));
                                                                                                                    }
                                                                                                                }
                                                                                                                if ((attvalue.ToString()) != "8")
                                                                                                                {
                                                                                                                    if (value != "HS")
                                                                                                                    {
                                                                                                                        if (!has_attnd_masterset_notconsider.ContainsKey(attvalue.ToString()))
                                                                                                                        {
                                                                                                                            if (has_attnd_masterset.ContainsKey(attvalue.ToString()))
                                                                                                                            {

                                                                                                                                string getval = Convert.ToString(GetCorrespondingKey(attvalue, has_attnd_masterset));
                                                                                                                                //if (attvalue.ToString() != "3")
                                                                                                                                //{

                                                                                                                                if (getval.ToString() == "0" || getval.ToString() == "3")
                                                                                                                                {
                                                                                                                                    present_count = Convert.ToInt16(GetCorrespondingKey(consolidate.Rows[i][1].ToString().ToLower().Trim(), has_load_rollno));
                                                                                                                                    present_count++;
                                                                                                                                    has_load_rollno[consolidate.Rows[i][1].ToString().ToLower().Trim()] = present_count;
                                                                                                                                }
                                                                                                                                // }
                                                                                                                            }
                                                                                                                            if (value != "NE")
                                                                                                                            {
                                                                                                                                present_count = Convert.ToInt16(GetCorrespondingKey(consolidate.Rows[i][1].ToString().Trim().ToLower(), has_total_attnd_hour));
                                                                                                                                present_count++;
                                                                                                                                has_total_attnd_hour[consolidate.Rows[i][1].ToString().ToLower().Trim()] = present_count;
                                                                                                                            }
                                                                                                                            if (attvalue.ToString() == "3")
                                                                                                                            {
                                                                                                                                if (!hatodtot.Contains(rollno.ToLower()))
                                                                                                                                {
                                                                                                                                    hatodtot.Add(rollno.ToLower(), "1");
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    double getods = Convert.ToDouble(GetCorrespondingKey(rollno, hatodtot));
                                                                                                                                    getods = getods + 1;
                                                                                                                                    hatodtot[rollno.ToLower()] = getods;
                                                                                                                                }
                                                                                                                            }
                                                                                                                            if (hatabsentvalues.ContainsKey(attvalue.ToString()))
                                                                                                                            {
                                                                                                                                double getads = Convert.ToDouble(GetCorrespondingKey(rollno.ToLower(), has_total_absent_hour));
                                                                                                                                getads = getads + 1;
                                                                                                                                has_total_absent_hour[rollno.ToLower()] = getads;
                                                                                                                            }

                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
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
                                                }
                                                temp_date = temp_date.AddDays(1);
                                            }
                                        }
                                    }
                                }

                                string tolconghors = "";
                                Boolean prood = false;

                                if (recflag == true || spl_hr_flag == true)
                                {

                                
                                    double attnd_hr = 0, tot_hr = 0, ondutyvalue = 0;

                                    for (int row_cnt = 3; row_cnt < consolidate.Rows.Count; row_cnt++)
                                    {
                                        less70perc = 0;
                                        attnd_hr = 0;
                                        tot_hr = 0;
                                        sub_prc = 0;
                                        ondutyvalue = 0;
                                        string roll_number = consolidate.Rows[row_cnt][1].ToString().Trim().ToLower();

                                        if (has_load_rollno.Contains(roll_number))
                                        {
                                            attnd_hr = Convert.ToDouble(GetCorrespondingKey(roll_number, has_load_rollno));
                                            if (!over_per.Contains(roll_number))
                                            {
                                                over_per.Add(roll_number, attnd_hr);
                                            }
                                            else if (over_per.Contains(roll_number))
                                            {
                                                double d = Convert.ToDouble(GetCorrespondingKey(roll_number, over_per));
                                                double da2 = d + attnd_hr;
                                                over_per[roll_number] = da2;
                                            }
                                        }
                                        if (has_total_attnd_hour.Contains(roll_number))
                                        {
                                            tot_hr = Convert.ToDouble(GetCorrespondingKey(roll_number, has_total_attnd_hour));
                                        }
                                        if (has_total_onduty_hour.Contains(roll_number))
                                        {
                                            ondutyvalue = Convert.ToDouble(GetCorrespondingKey(roll_number, has_total_onduty_hour));
                                        }

                                        double tot = attnd_hr + ondutyvalue;
                                        sub_prc = Math.Round(((tot / tot_hr) * 100), 2);
                                        if (sub_prc.ToString().Trim().ToLower() == "nan" || sub_prc.ToString().Trim().ToLower() == "infinity")
                                        {
                                            sub_prc = 0;
                                        }
                                        if (prood == false)
                                        {
                                            if (sub_prc == 100)
                                            {
                                                tolconghors = Convert.ToString(tot_hr);
                                                prood = true;
                                            }
                                        }
                                        if (attnd_hr == 0 && tot_hr == 0)
                                        {

                                            consolidate.Rows[row_cnt][consolidate.Columns.Count - 1] = "-";

                                        }
                                        else
                                        {
                                            consolidate.Rows[row_cnt][consolidate.Columns.Count - 1] = attnd_hr.ToString();

                                        }
                                    }
                                    if (tolconghors != "")
                                    {
                                        subtolcont1 = subtolcont1 + Convert.ToInt16(tolconghors);
                                    }
                                    else
                                    {
                                        tolconghors = "0";
                                        subtolcont1 = subtolcont1 + Convert.ToInt16(tolconghors);
                                    }
                                }
                                subtolcont1233 = subtolcont1233 + subtolcont1;
                                consolidate.Columns.Add("PERIOD-" + PRODS + " NO OF HOURS OUT OF (" + subtolcont1 + ")", typeof(string));
                                consolidate.Rows[0][consolidate.Columns.Count - 1] = "PERIOD-" + PRODS + " NO OF HOURS OUT OF (" + subtolcont1 + ")";
                                consolidate.Rows[1][consolidate.Columns.Count - 1] ="PERIOD-" + PRODS + " NO OF HOURS OUT OF (" + subtolcont1 + ")";
                                consolidate.Rows[2][consolidate.Columns.Count - 1] = "PERIOD-" + PRODS + " NO OF HOURS OUT OF (" + subtolcont1 + ")";
                              

                            }
                            semstartdate = dssem25.Tables[0].Rows[proid]["exam_date"].ToString();


                        }


                        consolidate.Columns.Add("OVERALL PERCENTAGE %", typeof(string));
                        consolidate.Rows[0][consolidate.Columns.Count - 1] = "OVERALL PERCENTAGE %";
                        consolidate.Rows[1][consolidate.Columns.Count - 1] = "OVERALL PERCENTAGE %";
                        consolidate.Rows[2][consolidate.Columns.Count - 1] = "OVERALL PERCENTAGE %";
                        double attnd_hr12 = 0;
                        double attnd_hr1212 = 0;
                        double total_subper12 = 0;
                        for (int j = 3; j < consolidate.Rows.Count; j++)
                        {
                            double total_subper = 0;

                            string roll_number = consolidate.Rows[j][1].ToString().Trim().ToLower();

                            if (over_per1.Contains(roll_number))
                            {
                                attnd_hr1212 = Convert.ToDouble(GetCorrespondingKey(roll_number, over_per1));
                            }
                            else
                            {
                                attnd_hr1212 = 0;
                            }

                            consolidate.Rows[j][consolidate.Columns.Count - rowpedcont - 1] = Convert.ToString(attnd_hr1212);


                            total_subper = Math.Round((Convert.ToDouble(attnd_hr1212) / subtolcont) * 100, 2);
                            if (total_subper.ToString().Trim().ToLower() == "nan" || total_subper.ToString().Trim().ToLower() == "infinity")
                            {
                                total_subper = 0;
                            }
                            consolidate.Rows[j][consolidate.Columns.Count - rowpedcont] = Convert.ToString(total_subper);


                            if (over_per.Contains(roll_number))
                            {
                                attnd_hr12 = Convert.ToDouble(GetCorrespondingKey(roll_number, over_per));
                            }

                            total_subper12 = Math.Round((attnd_hr12 / subtolcont1233) * 100, 2);
                            if (total_subper12.ToString().Trim().ToLower() == "nan" || total_subper12.ToString().Trim().ToLower() == "infinity")
                            {
                                total_subper12 = 0;
                            }

                            consolidate.Rows[j][consolidate.Columns.Count - 1] = Convert.ToString(total_subper12);

                        }
                    }
                    else
                    {


                        criteriaassment = ddlTest.SelectedItem.Text;

                        Session["perioftest"] = "ASSESMENT PERIOD  : " + criteriaassment;


                        consolidate.Columns.Add("NO OF HOURS OUT OF (" + subtolcont + ")", typeof(string));
                        consolidate.Rows[0][consolidate.Columns.Count - 1] = "NO OF HOURS OUT OF (" + subtolcont + ")";
                        consolidate.Rows[1][consolidate.Columns.Count - 1] = "NO OF HOURS OUT OF (" + subtolcont + ")";
                        consolidate.Rows[2][consolidate.Columns.Count - 1] = "NO OF HOURS OUT OF (" + subtolcont + ")";

                        consolidate.Columns.Add("%", typeof(string));
                        consolidate.Rows[0][consolidate.Columns.Count - 1] = "%";
                        consolidate.Rows[1][consolidate.Columns.Count - 1] = "%";
                        consolidate.Rows[2][consolidate.Columns.Count - 1] = "%";

                        consolidate.Columns.Add("OVERALL PERCENTAGE %", typeof(string));
                        consolidate.Rows[0][consolidate.Columns.Count - 1] = "OVERALL PERCENTAGE %";
                        consolidate.Rows[1][consolidate.Columns.Count - 1] = "OVERALL PERCENTAGE %";
                        consolidate.Rows[2][consolidate.Columns.Count - 1] = "OVERALL PERCENTAGE %";

                        double attnd_hr3 = 0;
                        for (int j = 0; j < consolidate.Rows.Count; j++)
                        {
                            double total_subper = 0;

                            string roll_number = consolidate.Rows[j][1].ToString().Trim().ToLower();
                            if (over_per1.Contains(roll_number))
                            {
                                attnd_hr3 = Convert.ToDouble(GetCorrespondingKey(roll_number, over_per1));
                            }
                            else
                            {
                                attnd_hr3 = 0;
                            }
                            consolidate.Rows[j][consolidate.Columns.Count - 3] = Convert.ToString(attnd_hr3);


                            total_subper = Math.Round((Convert.ToDouble(attnd_hr3) / subtolcont) * 100, 2);
                            if (total_subper.ToString().Trim().ToLower() == "nan" || total_subper.ToString().Trim().ToLower() == "infinity")
                            {
                                total_subper = 0;
                            }

                            consolidate.Rows[j][consolidate.Columns.Count - 2] = Convert.ToString(total_subper);


                            consolidate.Rows[j][consolidate.Columns.Count - 1] = Convert.ToString(total_subper);

                        }
                    }


                    consolidate.Columns.Add("REMARKS", typeof(string));
                    consolidate.Rows[0][consolidate.Columns.Count - 1] = "REMARKS";
                    consolidate.Rows[1][consolidate.Columns.Count - 1] = "REMARKS";
                    consolidate.Rows[2][consolidate.Columns.Count - 1] = "REMARKS";


                    gridview1.DataSource = consolidate;
                    gridview1.DataBind();
                    gridview1.Visible = true;
                    lblnorec.Visible = false;

                    Button2.Visible = true;
                    btnExcel.Visible = true;
                    btnPrintMaster.Visible = false;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;


                    int rowcnt1 = gridview1.Rows.Count - 3;
                    //Rowspan
                    for (int rowIndex = gridview1.Rows.Count - rowcnt1 - 1; rowIndex >= 0; rowIndex--)
                    {
                        GridViewRow row = gridview1.Rows[rowIndex];
                        GridViewRow previousRow = gridview1.Rows[rowIndex + 1];
                        gridview1.Rows[rowIndex].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        gridview1.Rows[rowIndex].Font.Bold = true;
                        gridview1.Rows[rowIndex].HorizontalAlign = HorizontalAlign.Center;

                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            if (row.Cells[i].Text == previousRow.Cells[i].Text)
                            {
                                row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                       previousRow.Cells[i].RowSpan + 1;
                                previousRow.Cells[i].Visible = false;
                            }

                        }

                    }
                    //ColumnSpan
                    for (int rowIndex = gridview1.Rows.Count - rowcnt1 - 1; rowIndex >= 0; rowIndex--)
                    {


                        for (int cell = gridview1.Rows[rowIndex].Cells.Count - 1; cell > 0; cell--)
                        {
                            TableCell colum = gridview1.Rows[rowIndex].Cells[cell];
                            TableCell previouscol = gridview1.Rows[rowIndex].Cells[cell - 1];
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
                    foreach (KeyValuePair<int, string> dr in snoco)
                    {
                        int g = dr.Key;
                        string DicValue = dr.Value;
                        if (DicValue == "sno")
                        {
                            gridview1.Rows[g].BackColor = Color.LightGray;
                        }
                       
                    }

                }
                else
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "No Records Found";
                    Button2.Visible = false;
                    gridview1.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    btnPrintMaster.Visible = false;
                    return;
                }
            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "No Records Found";
                Button2.Visible = false;
                gridview1.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                btnPrintMaster.Visible = false;
                return;
            }
        }

        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }
    public string filerfunction()
    {
        string orderby_Setting = dacc.GetFunction("select value from master_Settings where settings='order_by'");
        string strorder = "";
        string strorder342 = "";
        string serialno = dacc.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
        if (serialno == "1")
        {
            strorder = "ORDER BY r.serialno";
            strorder342 = "ORDER BY registration.serialno";
        }
        else
        {
            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY r.roll_no";
                strorder342 = "ORDER BY registration.roll_no";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY r.Reg_No";
                strorder342 = "ORDER BY registration.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = "ORDER BY r.Stud_Name";
                strorder342 = "ORDER BY registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY r.roll_no,r.Reg_No,r.Stud_Name";
                strorder342 = "ORDER BY registration.roll_no,registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY r.roll_no,r.Reg_No";
                strorder342 = "ORDER BY registration.roll_no,registration.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY r.Reg_No,r.Stud_Name";
                strorder342 = "ORDER BY registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY r.roll_no,r.Stud_Name";
                strorder342 = "ORDER BY registration.roll_no,registration.Stud_Name";
            }
        }
        return strorder;
    }
    private string findday(int no, string sdate, string todate)//------------------find day order 
    {
        int order, holino;
        holino = 0;
        string day_order = "";
        string from_date = "", tmpdate = "";
        string fdate = "", smdate = "";
        int diff_work_day = 0;

        tmpdate = sdate.ToString();
        string[] semstart_date = tmpdate.Split(new Char[] { ' ' });
        string[] sm_date = semstart_date[0].Split(new Char[] { '/' });
        smdate = sm_date[0].ToString() + "/" + sm_date[1].ToString() + "/" + sm_date[2].ToString();


        from_date = todate.ToString();
        string[] fm_date = from_date.Split(new Char[] { '/' });
        fdate = fm_date[1].ToString() + "/" + fm_date[0].ToString() + "/" + fm_date[2].ToString();
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        SqlDataReader dr;
        con.Close();
        con.Open();
        cmd = new SqlCommand("select count(*) from holidaystudents where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and holiday_date between '" + sdate.ToString() + "' and  '" + fdate.ToString() + "' and halforfull='0' and isnull(Not_include_dayorder,0)<>'1'", con);//01.03.17 barath";"
        dr = cmd.ExecuteReader();
        dr.Read();
        if (dr.HasRows == true)
        {
            holino = Convert.ToInt16(dr[0].ToString());
        }

        string quer = "select nodays from PeriodAttndSchedule where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString();
        string nodays = d2.GetFunction(quer);
        int no_days = Convert.ToInt32(nodays);
        //DateTime dt1 = Convert.ToDateTime(fdate.ToString());
        //DateTime dt2 = Convert.ToDateTime(smdate.ToString());
        //TimeSpan t = dt1.Subtract(dt2);

        DateTime dt1 = Convert.ToDateTime(smdate);
        DateTime dt2 = Convert.ToDateTime(fdate);
        TimeSpan t = dt2 - dt1;

        int days = t.Days;

        diff_work_day = days - holino;
        order = Convert.ToInt16(diff_work_day.ToString()) % no_days;
        order = order + 1;
        string stastdayorder = "";

        stastdayorder = da.GetFunction("select starting_dayorder from seminfo where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "");
        if (stastdayorder.ToString().Trim() != "")
        {
            if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
            {
                order = order + (Convert.ToInt16(stastdayorder) - 1);
                if (order == (no_days + 1))
                    order = 1;
                else if (order > no_days)
                    order = order % no_days;
            }
        }

        if (order.ToString() == "0")
        {
            order = no_days;
        }
        if (order.ToString() == "1")
        {
            day_order = "mon";
        }
        else if (order.ToString() == "2")
        {
            day_order = "tue";
        }
        else if (order.ToString() == "3")
        {
            day_order = "wed";
        }
        else if (order.ToString() == "4")
        {
            day_order = "thu";
        }
        else if (order.ToString() == "5")
        {
            day_order = "fri";
        }
        else if (order.ToString() == "6")
        {
            day_order = "sat";
        }
        else if (order.ToString() == "7")
        {
            day_order = "sun";
        }
        return (day_order);
    }
    public void loadmarkss()
    {
        try
        {
            int EL = 0;
            int res = 0;

            string marks_per;
            int stu_count = 0;

            per_sub_count = ds2.Tables[1].Rows.Count;
            if (ddlSec.Enabled == true)
            {
                sections = ddlSec.SelectedItem.Text;
            }
            else
            {
                sections = "";
            }
            string strsecavl = "";
            if (sections.ToString().Trim() != "All" && sections.ToString().Trim() != string.Empty && sections.ToString().Trim() != "-1" && sections.ToString().Trim() != "")
            {
                strsecavl = " and sections='" + sections.ToString() + "'";
            }
            //Modified By Srinath 6/11/2015
            //   string strretestmark = "select t.Marks_Obtained,t.Roll_No,s.subject_no from tbl_result_retest t,Exam_type e,CriteriaForInternal c,syllabus_master sy,subject s where t.Exam_Code=e.exam_code and e.criteria_no=c.Criteria_no and sy.syll_code=c.syll_code and e.subject_no=s.subject_no and sy.syll_code=s.syll_code and sy.Batch_Year='"+ddlBatch.SelectedValue.ToString()+"' and degree_code='"+ddlBranch.SelectedValue.ToString()+"' and sy.semester='"+ddlSemYr.SelectedValue.ToString()+"' and c.Criteria_no='"+ddlTest.SelectedValue.ToString()+"' "+strsecavl+"";
            string strretestmark = "select t.Retest_Marks_obtained,t.Marks_obtained,t.Roll_No,s.subject_no from result t,Exam_type e,CriteriaForInternal c,syllabus_master sy,subject s where t.Exam_Code=e.exam_code and e.criteria_no=c.Criteria_no and sy.syll_code=c.syll_code and e.subject_no=s.subject_no and sy.syll_code=s.syll_code and sy.Batch_Year='" + ddlBatch.SelectedValue.ToString() + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and sy.semester='" + ddlSemYr.SelectedValue.ToString() + "' and c.Criteria_no='" + ddlTest.SelectedValue.ToString() + "' " + strsecavl + " and Retest_Marks_obtained is not null";
            DataSet dsresetmark = da.select_method_wo_parameter(strretestmark, "Text");

            if (ds2.Tables[0].Rows.Count != 0)
            {
                DataView dv_indstudmarks = new DataView();
                for (int i = 3; i < consolidate.Rows.Count; i++)
                {
                    for (int colcnt = 3; colcnt <= consolidate.Rows.Count - 1; colcnt ++)
                    {
                        for (int row_cnt = 3; row_cnt < consolidate.Rows.Count; row_cnt++)
                        {
                            if (colno1.ContainsKey(colcnt))//ko
                            {
                                string subno_val = colno1[colcnt];


                                int a = 0;
                                int sstat = 0;
                                if (subno_val.Trim().ToString() != "")
                                {


                                    string rollno = consolidate.Rows[row_cnt][1].ToString().Trim().ToLower();

                                    ds2.Tables[0].DefaultView.RowFilter = "RollNumber='" + rollno + "' and subject_no='" + subno_val + "'";
                                    dv_indstudmarks = ds2.Tables[0].DefaultView;
                                    if (dv_indstudmarks.Count > 0)
                                    {
                                        for (int cnt = 0; cnt < dv_indstudmarks.Count; cnt++)
                                        {
                                            sstat++;
                                            double marks = double.Parse(dv_indstudmarks[cnt]["mark"].ToString());
                                            marks_per = dv_indstudmarks[cnt]["mark"].ToString();

                                            dsresetmark.Tables[0].DefaultView.RowFilter = "Roll_No='" + rollno + "' and subject_no='" + subno_val + "'";
                                            DataView dvremark = dsresetmark.Tables[0].DefaultView;
                                            Boolean refail = false;
                                            if (dvremark.Count > 0)
                                            {
                                                string remark = dvremark[0]["Retest_Marks_obtained"].ToString();
                                                if (remark.Trim() != "" && remark.Trim() != null)
                                                {
                                                    marks = Convert.ToDouble(remark);
                                                    marks_per = remark;
                                                }
                                            }
                                            string min_marksstring = dv_indstudmarks[cnt]["min_mark"].ToString();
                                            if (min_marksstring != "")
                                            {
                                                min_mark = int.Parse(min_marksstring.ToString());
                                            }
                                            else
                                            {
                                                min_mark = 0;
                                            }
                                            // marks_per = marks.ToString();
                                            // marks_per = dv_indstudmarks[cnt]["mark"].ToString();

                                            switch (marks_per)
                                            {
                                                case "-1":

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

                                                case "-18":
                                                    marks_per = "RAA";
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
                                            }
                                            if (marks >= min_mark || marks_per == "EL" || marks_per == "EOD")
                                            {
                                                pass++;
                                                if ((RadioButtonList3.SelectedItem.ToString() == "Pass") || RadioButtonList3.SelectedItem.ToString() == "All")
                                                {

                                                    consolidate.Rows[row_cnt][colcnt + 1] = marks_per.ToString();

                                                }

                                            }
                                            else
                                            {
                                                fail++;
                                                if ((RadioButtonList3.SelectedItem.ToString() == "Fail") || RadioButtonList3.SelectedItem.ToString() == "All")
                                                {

                                                    if (marks >= 0)
                                                    {

                                                        consolidate.Rows[row_cnt][colcnt+1] = marks_per.ToString();
                                                        //FpEntry.Sheets[0].Cells[i, colcnt + 1].ForeColor = Color.Red;
                                                        //FpEntry.Sheets[0].Cells[i, colcnt + 1].Font.Underline = true;
                                                        //FpEntry.Sheets[0].Cells[i, colcnt + 1].Font.Name = "Book Antiqua";
                                                        //FpEntry.Sheets[0].Cells[i, colcnt + 1].HorizontalAlign = HorizontalAlign.Center;
                                                        //FpEntry.Sheets[0].Cells[i, colcnt + 1].Font.Size = FontUnit.Medium;
                                                    }
                                                    else
                                                    {
                                                        consolidate.Rows[row_cnt][colcnt + 1] = marks_per.ToString();
                                                        //FpEntry.Sheets[0].Cells[i, colcnt + 1].ForeColor = Color.Red;
                                                        //FpEntry.Sheets[0].Cells[i, colcnt + 1].Font.Underline = true;
                                                        //FpEntry.Sheets[0].Cells[i, colcnt + 1].Font.Name = "Book Antiqua";
                                                        //FpEntry.Sheets[0].Cells[i, colcnt + 1].HorizontalAlign = HorizontalAlign.Center;
                                                        //FpEntry.Sheets[0].Cells[i, colcnt + 1].Font.Size = FontUnit.Medium;
                                                        marks = 0;
                                                    }

                                                }


                                            }
                                            if ((RadioButtonList3.SelectedItem.ToString() == "Absent") || RadioButtonList3.SelectedItem.ToString() == "All")
                                            {
                                                if (marks < 0 && marks_per != "EL" && marks_per != "EOD")
                                                {
                                                    consolidate.Rows[row_cnt][colcnt + 1] = marks_per.ToString();
                                                    //FpEntry.Sheets[0].Cells[i, colcnt + 1].ForeColor = Color.Red;
                                                    //FpEntry.Sheets[0].Cells[i, colcnt + 1].Font.Underline = true;
                                                    //FpEntry.Sheets[0].Cells[i, colcnt + 1].Font.Name = "Book Antiqua";
                                                    //FpEntry.Sheets[0].Cells[i, colcnt + 1].HorizontalAlign = HorizontalAlign.Center;
                                                    //FpEntry.Sheets[0].Cells[i, colcnt + 1].Font.Size = FontUnit.Medium;
                                                }
                                            }
                                            tot_marks += marks;
                                            EL = 0;
                                            stu_count++;
                                        }
                                        if (chkretest.Checked == true)
                                        {
                                            string remark = "-";
                                            dsresetmark.Tables[0].DefaultView.RowFilter = "Roll_No='" + rollno + "' and subject_no='" + subno_val + "'";
                                            DataView dvremark = dsresetmark.Tables[0].DefaultView;
                                            Boolean refail = false;
                                            if (dvremark.Count > 0)
                                            {
                                                //remark = dvremark[0]["Retest_Marks_obtained"].ToString();
                                                remark = dvremark[0]["Marks_obtained"].ToString();
                                                if (remark.Trim() != "")
                                                {
                                                    if (Convert.ToInt32(remark) < min_mark)
                                                    {
                                                        refail = true;
                                                    }
                                                }
                                                switch (remark)
                                                {
                                                    case "-1":
                                                        remark = "AAA";
                                                        break;
                                                    case "-2":
                                                        remark = "EL";
                                                        break;
                                                    case "-3":
                                                        remark = "EOD";
                                                        break;
                                                    case "-4":
                                                        remark = "ML";
                                                        break;
                                                    case "-5":
                                                        remark = "SOD";
                                                        break;
                                                    case "-6":
                                                        remark = "NSS";
                                                        break;
                                                    case "-7":
                                                        remark = "NJ";
                                                        break;
                                                    case "-8":
                                                        remark = "S";
                                                        break;
                                                    case "-9":
                                                        remark = "L";
                                                        break;
                                                    case "-10":
                                                        remark = "NCC";
                                                        break;
                                                    case "-11":
                                                        remark = "HS";
                                                        break;
                                                    case "-12":
                                                        remark = "PP";
                                                        break;
                                                    case "-13":
                                                        remark = "SYOD";
                                                        break;
                                                    case "-14":
                                                        remark = "COD";
                                                        break;
                                                    case "-15":
                                                        remark = "OOD";
                                                        break;
                                                    case "-16":
                                                        remark = "OD";
                                                        break;
                                                    case "-17":
                                                        remark = "LA";
                                                        break;

                                                    case "-18":
                                                        remark = "RAA";
                                                        break;

                                                }
                                            }

                                            consolidate.Rows[row_cnt][colcnt + 2] = remark.ToString();
                                            if (refail == true)
                                            {
                                                //FpEntry.Sheets[0].Cells[i, colcnt + 2].Font.Underline = true;
                                                //FpEntry.Sheets[0].Cells[i, colcnt + 2].ForeColor = Color.Purple;
                                            }
                                            else if (remark.Trim() != "-")
                                            {
                                                // FpEntry.Sheets[0].Cells[i, colcnt + 2].ForeColor = Color.Green;
                                            }
                                            //FpEntry.Sheets[0].Cells[i, colcnt + 2].Font.Name = "Book Antiqua";
                                            //FpEntry.Sheets[0].Cells[i, colcnt + 2].HorizontalAlign = HorizontalAlign.Center;
                                            //FpEntry.Sheets[0].Cells[i, colcnt + 2].Font.Size = FontUnit.Medium;
                                        }
                                        a = a + 2;
                                    }
                                }

                            }
                          
                        }
                       
                    }

                }
            }
        }
        catch
        {
        }
    }

    public void load_attendance()
    {
        string sections = "";

        string strondutyquery = "";
        if (ddlSec.SelectedValue.ToString() == "" || ddlSec.SelectedValue.ToString() == "-1")
        {
            sections = "";

        }
        else
        {
            sections = ddlSec.SelectedItem.ToString();
        }
        Hashtable hatattendance = new Hashtable();
        string rstrsec = "";
        try
        {
            temp_date = dt1;
            string splhrsec = "";

            if (chkflag == false)
            {
                chkflag = true;
                has.Clear();
                has.Add("colege_code", Session["collegecode"].ToString());
                ds_attndmaster = dacc.select_method("ATT_MASTER_SETTING", has, "sp");
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

                string getdeteails1 = "select convert(nvarchar(15),s.start_date,101) as start_date,nodays,s.starting_dayorder from seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and p.semester=s.semester and s.semester='" + ddlSemYr.SelectedItem.ToString() + "' and s.batch_year='" + ddlBatch.Text.ToString() + "'  and s.degree_code='" + ddlBranch.SelectedValue.ToString() + "'";
                getdeteails1 = getdeteails1 + " ; select * from tbl_consider_day_order where semester='" + ddlSemYr.SelectedItem.ToString() + "' and batch_year='" + ddlBatch.Text.ToString() + "'  and degree_code='" + ddlBranch.SelectedValue.ToString() + "'";
                getdeteails1 = getdeteails1 + " ;select subject_type,LAB,subject_no From sub_sem ss,subject s where ss.subtype_no=s.subtype_no ";
                DataSet dssem1 = d2.select_method_wo_parameter(getdeteails1, "Text");
                if (dssem1.Tables[0].Rows.Count > 0)
                {
                    semstartdate = dssem1.Tables[0].Rows[0]["start_date"].ToString();
                    noofdays = dssem1.Tables[0].Rows[0]["nodays"].ToString();
                    startday = dssem1.Tables[0].Rows[0]["starting_dayorder"].ToString();
                }

                //  string strquery = "select distinct convert(nvarchar(15),MAX(e.exam_date),101) AS exam_date,c.criteria,c.criteria_no from    criteriaforinternal c,syllabus_master sm,exam_type e where  c.syll_code=sm.syll_code and e.criteria_no = c.Criteria_no and sm.degree_code='47' and sm.semester='7' and    sm.batch_year='2011' and sections='A'  GROUP BY C.criteria_no,C.criteria  order by exam_date ";


                for (int dc = 0; dc < dssem1.Tables[2].Rows.Count; dc++)
                {
                    string subject_nos = dssem1.Tables[2].Rows[dc]["subject_no"].ToString();
                    string LABdsa = dssem1.Tables[2].Rows[dc]["LAB"].ToString();

                    if (!hatdclab.Contains(subject_nos))
                    {
                        hatdclab.Add(subject_nos, LABdsa);
                    }

                }
                try
                {
                    for (int dc = 0; dc < dssem1.Tables[1].Rows.Count; dc++)
                    {
                        DateTime dtdcf = Convert.ToDateTime(dssem1.Tables[1].Rows[dc]["from_date"].ToString());
                        DateTime dtdct = Convert.ToDateTime(dssem1.Tables[1].Rows[dc]["to_date"].ToString());
                        for (DateTime dtc = dtdcf; dtc <= dtdct; dtc = dtc.AddDays(1))
                        {
                            if (!hatdc12.Contains(dtc))
                            {
                                hatdc12.Add(dtc, dtc);
                            }
                        }
                    }
                }
                catch
                {
                }
                //string getdeteails12 = " select distinct convert(nvarchar(15),MAX(e.exam_date),101) AS exam_date,c.criteria,c.criteria_no from   criteriaforinternal c,syllabus_master sm,exam_type e where    c.syll_code=sm.syll_code and    e.criteria_no = c.Criteria_no and c.criteria_no='" + ddlTest.SelectedValue.ToString() + "'    and sm.degree_code='" + ddlBranch.SelectedValue.ToString() + "'    and sm.semester='" + ddlSemYr.SelectedItem.ToString() + "' and   sm.batch_year='" + ddlBatch.Text.ToString() + "'  GROUP BY C.criteria_no,C.criteria";
                //DataSet dssem22 = d2.select_method_wo_parameter(getdeteails12, "Text");

                //if (dssem22.Tables[0].Rows.Count > 0)
                //{
                //    semstartdate123 = dssem22.Tables[0].Rows[0]["exam_date"].ToString();
                //    string noofdays123 = dssem22.Tables[0].Rows[0]["criteria"].ToString();
                //    string startday123 = dssem22.Tables[0].Rows[0]["criteria_no"].ToString();
                //}
                //string getdeteails14 = " select top 2 convert(nvarchar(15),MAX(e.exam_date),101) AS exam_date,c.criteria,c.criteria_no from   criteriaforinternal c,syllabus_master sm,exam_type e where    c.syll_code=sm.syll_code and    e.criteria_no = c.Criteria_no   and sm.degree_code='" + ddlBranch.SelectedValue.ToString() + "'    and sm.semester='" + ddlSemYr.SelectedItem.ToString() + "'  and  e.exam_date between '" + semstartdate + "' and '" + semstartdate123 + "' and  sm.batch_year='" + ddlBatch.Text.ToString() + "'  GROUP BY C.criteria_no,C.criteria order by e.exam_date desc";
                //DataSet dssem2 = d2.select_method_wo_parameter(getdeteails14, "Text");
                //if (dssem2.Tables[0].Rows.Count > 1)
                //{
                //    semstartdate123 = dssem2.Tables[0].Rows[0]["exam_date"].ToString();
                //    semstartdate = dssem2.Tables[0].Rows[1]["exam_date"].ToString();
                //    string noofdays12 = dssem2.Tables[0].Rows[0]["criteria"].ToString();
                //    string startday12 = dssem2.Tables[0].Rows[0]["criteria_no"].ToString();

                //    fdate = "";
                //    tdate = "";
                //    fdate = semstartdate;
                //    tdate = semstartdate123;
                //}
                //else
                //{
                //    fdate = "";
                //    tdate = "";
                //    fdate = semstartdate;
                //    tdate = semstartdate123;
                //}

            }
            string subno_val = "";
            int columncnt = 1;
            for (int colcnt = 3; colcnt <= consolidate.Columns.Count - 2; colcnt++)
            {
                roll_count = 0;
                present_count = 0;
                temp_hr_field = "";
                has_load_rollno.Clear();
                has_total_attnd_hour.Clear();
                has_total_onduty_hour.Clear();
                onduty = 0;

                if (colno1.ContainsKey(colcnt))//ko
                {
                    string subno_val1 = colno1[colcnt];

                    if (subno_val1.Trim().ToString() != "")
                    {
                        columncnt++;
                        subject_no = subno_val1;
                        fdate = "";
                        tdate = "";
                        if (hatexamdate.Contains(ddlTest.SelectedValue.ToString()))
                        {
                            string datesp = hatexamdate[ddlTest.SelectedValue.ToString()].ToString();
                            string[] spdate = datesp.Split(';');
                            if (spdate.GetUpperBound(0) >= 1)
                            {
                                fdate = spdate[0];
                                // tdate = spdate[1];
                            }
                        }
                        if (hatsubjectd.Contains(subject_no))
                        {
                            tdate = hatsubjectd[subject_no.ToString()].ToString();
                        }
                        string[] fromdatespit = fdate.ToString().Split('/');
                        string[] todatespit = tdate.ToString().Split('/');
                        spfromdate = Convert.ToDateTime(fromdatespit[0] + '/' + fromdatespit[1] + '/' + fromdatespit[2]);
                        sptodate = Convert.ToDateTime(todatespit[0] + '/' + todatespit[1] + '/' + todatespit[2]);
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

                        querystring = "select rights from  special_hr_rights where " + grouporusercode + "";
                        DataSet dsrights = da.select_method(querystring, hat, "Text");
                        if (dsrights.Tables[0].Rows.Count > 0)
                        {
                            if (dsrights.Tables[0].Rows[0]["rights"].ToString().ToLower().Trim() == "true")
                            {
                                splhr_flag = true;
                            }
                        }
                        temp_date = spfromdate;
                        dt2 = sptodate;

                        while (temp_date <= dt2)
                        {
                            if (!hatdc12.Contains(temp_date))
                            {
                                if (splhr_flag == true)
                                {
                                    if (ht_sphr.Contains(Convert.ToString(temp_date)))
                                    {
                                        getspecial_hr();
                                    }
                                }

                                span_count = 0;

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
                                    //temp_date = temp_date.AddDays(1); aruna 30oct2012
                                }
                                else
                                {
                                    holiflag = true;

                                    dsalldetails.Tables[7].DefaultView.RowFilter = "degree_code = " + ddlBranch.SelectedValue.ToString() + " and semester = " + ddlSemYr.SelectedItem.ToString() + " and batch_year = " + ddlBatch.SelectedValue.ToString() + " and FromDate ='" + temp_date + "' " + strsec + "";
                                    DataView dvaltersech = dsalldetails.Tables[7].DefaultView;

                                    dsalldetails.Tables[6].DefaultView.RowFilter = "degree_code = " + ddlBranch.SelectedValue.ToString() + " and semester = " + ddlSemYr.SelectedItem.ToString() + " and batch_year = " + ddlBatch.SelectedValue.ToString() + " and FromDate <='" + temp_date + "' " + strsec + "";
                                    DataView dvsemsech = dsalldetails.Tables[6].DefaultView;

                                    hatattendance.Clear();
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
                                                Boolean samehr_flag = false;
                                                roll_count = 0;
                                                present_count = 0;
                                                temp_hr_field = strDay + temp_hr;
                                                date_temp_field = "d" + final_date_string[1].ToString() + "d" + temp_hr;
                                                hatattendance.Clear();
                                                if (dvaltersech.Count > 0)
                                                {
                                                    for (int hasrow = 0; hasrow < dvaltersech.Count; hasrow++)
                                                    {
                                                        full_hour = dvaltersech[hasrow][temp_hr_field].ToString();
                                                        hatattendance.Clear();
                                                        if (full_hour.Trim() != "")
                                                        {
                                                            temp_has_subj_code.Clear();
                                                            string[] split_full_hour = full_hour.Split(';');
                                                            for (int semi_colon = 0; semi_colon <= split_full_hour.GetUpperBound(0); semi_colon++)
                                                            {
                                                                roll_count = 0;
                                                                single_hour = split_full_hour[semi_colon].ToString();
                                                                string[] split_single_hour = single_hour.Split('-');
                                                                if (split_single_hour.GetUpperBound(0) >= 1)
                                                                {
                                                                    check_alter = true;
                                                                    if (split_single_hour[0].ToString() == subject_no)
                                                                    {

                                                                        if (!temp_has_subj_code.ContainsKey(subject_no))
                                                                        {
                                                                            temp_has_subj_code.Add(subject_no, subject_no);
                                                                            recflag = true;
                                                                            roll_count = 0;

                                                                            if (samehr_flag == false)
                                                                            {
                                                                                span_count++;
                                                                                samehr_flag = true;
                                                                            }
                                                                            Hashtable has_stud_list = new Hashtable();

                                                                            string subj_type = "";
                                                                            if (!hatdclab.Contains(subject_no))
                                                                            {
                                                                                subj_type = hatdclab[subject_no].ToString();
                                                                            }
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
                                                                                        if (!hatattendance.Contains(rollno.ToString().Trim().ToLower()))
                                                                                        {
                                                                                            hatattendance.Add(rollno.ToString().Trim().ToLower(), attval);
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
                                                                                                if (!hatattendance.Contains(rollno.ToString().Trim().ToLower()))
                                                                                                {
                                                                                                    hatattendance.Add(rollno.ToString().Trim().ToLower(), attval);
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            if (hatattendance.Count > 0)
                                                                            {
                                                                                for (int i = 3; i < consolidate.Rows.Count; i++)
                                                                                {

                                                                                    string rollno = consolidate.Rows[i][1].ToString().Trim().ToString().ToLower();
                                                                                    if (hatattendance.Contains(rollno.ToString()))
                                                                                    {
                                                                                        no_stud_flag = true;

                                                                                        string sdmdates = consolidate.Rows[i][1].ToString().Trim().ToString().ToLower();
                                                                                        string[] fromdatespit99 = sdmdates.ToString().Split('/');
                                                                                        Admission_date = Convert.ToDateTime(fromdatespit99[1] + '/' + fromdatespit99[0] + '/' + fromdatespit99[2]);

                                                                                        string attvalue = GetCorrespondingKey(rollno, hatattendance).ToString();
                                                                                        string value = Attmark(attvalue.ToString());
                                                                                        if (temp_date >= Admission_date)
                                                                                        {
                                                                                            if (consolidate.Rows[i][consolidate.Columns.Count - 1] == "HS")
                                                                                            {
                                                                                                if (!has_hs.ContainsKey((consolidate.Columns.Count - 1)))
                                                                                                {
                                                                                                    has_hs.Add((consolidate.Columns.Count - 1), (consolidate.Columns.Count - 1));
                                                                                                }
                                                                                            }
                                                                                            if ((attvalue.ToString()) != "8")
                                                                                            {
                                                                                                if (value != "HS")
                                                                                                {
                                                                                                    if (!has_attnd_masterset_notconsider.ContainsKey(attvalue.ToString()))
                                                                                                    {
                                                                                                        if (has_attnd_masterset.ContainsKey(attvalue.ToString()))
                                                                                                        {
                                                                                                            string getval = Convert.ToString(GetCorrespondingKey(attvalue, has_attnd_masterset));

                                                                                                            if (getval.ToString() == "0" || getval.ToString() == "3")
                                                                                                            {
                                                                                                                present_count = Convert.ToInt16(GetCorrespondingKey(consolidate.Rows[i][1].ToString().Trim().ToLower(), has_load_rollno));
                                                                                                                present_count++;
                                                                                                                has_load_rollno[consolidate.Rows[i][1].ToString().ToLower().Trim()] = present_count;
                                                                                                            }

                                                                                                        }
                                                                                                        if (value != "NE")
                                                                                                        {
                                                                                                            present_count = Convert.ToInt16(GetCorrespondingKey(consolidate.Rows[i][1].ToString().ToLower().Trim(), has_total_attnd_hour));
                                                                                                            present_count++;
                                                                                                            has_total_attnd_hour[consolidate.Rows[i][1].ToString().ToLower().Trim()] = present_count;
                                                                                                        }
                                                                                                        if (attvalue.ToString() == "3")
                                                                                                        {
                                                                                                            if (!hatodtot.Contains(rollno.ToLower()))
                                                                                                            {
                                                                                                                hatodtot.Add(rollno.ToLower(), "1");
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                double getods = Convert.ToDouble(GetCorrespondingKey(rollno, hatodtot));
                                                                                                                getods = getods + 1;
                                                                                                                hatodtot[rollno.ToLower()] = getods;
                                                                                                            }
                                                                                                        }
                                                                                                        if (hatabsentvalues.ContainsKey(attvalue.ToString()))
                                                                                                        {
                                                                                                            double getads = Convert.ToDouble(GetCorrespondingKey(rollno.ToLower(), has_total_absent_hour));
                                                                                                            getads = getads + 1;
                                                                                                            has_total_absent_hour[rollno.ToLower()] = getads;
                                                                                                        }
                                                                                                    }

                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
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
                                                samehr_flag = false;
                                                roll_count = 0;
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
                                                            roll_count = 0;
                                                            single_hour = split_full_hour_sem[semi_colon].ToString();
                                                            string[] split_single_hour = single_hour.Split('-');

                                                            if (split_single_hour.GetUpperBound(0) >= 1)
                                                            {
                                                                if (split_single_hour[0].ToString() == subject_no)
                                                                {
                                                                    if (!temp_has_subj_code.ContainsKey(subject_no))
                                                                    {
                                                                        temp_has_subj_code.Add(subject_no, subject_no);
                                                                        recflag = true;

                                                                        if (samehr_flag == false)
                                                                        {
                                                                            span_count++;
                                                                            samehr_flag = true;
                                                                        }
                                                                        Hashtable has_stud_list = new Hashtable();

                                                                        if (!hatdclab.Contains(subject_no))
                                                                        {
                                                                            subj_type = hatdclab[subject_no].ToString();
                                                                        }
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
                                                                                    if (!hatattendance.Contains(rollno.ToString().Trim().ToLower()))
                                                                                    {
                                                                                        hatattendance.Add(rollno.ToString().Trim().ToLower(), attval);
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
                                                                                            if (!hatattendance.Contains(rollno.ToString().Trim().ToLower()))
                                                                                            {
                                                                                                hatattendance.Add(rollno.ToString().ToLower().Trim(), attval);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }

                                                                        }

                                                                        if (hatattendance.Count > 0)
                                                                        {
                                                                            for (int i = 3; i < consolidate.Rows.Count; i++)
                                                                            {
                                                                                string rollno = consolidate.Rows[i][1].ToString().Trim().ToLower();
                                                                                if (hatattendance.Contains(rollno.ToString()))
                                                                                {
                                                                                    no_stud_flag = true;

                                                                                    if (dicadmdate.ContainsKey(i))
                                                                                    {
                                                                                        string sdmdates = dicadmdate[i];
                                                                                        string[] fromdatespit99 = sdmdates.ToString().Split('/');
                                                                                        Admission_date = Convert.ToDateTime(fromdatespit99[1] + '/' + fromdatespit99[0] + '/' + fromdatespit99[2]);

                                                                                        string attvalue = GetCorrespondingKey(rollno, hatattendance).ToString();
                                                                                        string value = Attmark(attvalue.ToString());
                                                                                        if (temp_date >= Admission_date)
                                                                                        {
                                                                                            // FpEntry.Sheets[0].Rows[i].Visible = true;
                                                                                            if (consolidate.Rows[i][consolidate.Columns.Count - 1] == "HS")
                                                                                            {
                                                                                                if (!has_hs.ContainsKey((consolidate.Columns.Count - 1)))
                                                                                                {
                                                                                                    has_hs.Add((consolidate.Columns.Count - 1), (consolidate.Columns.Count - 1));
                                                                                                }
                                                                                            }
                                                                                            if ((attvalue.ToString()) != "8")
                                                                                            {
                                                                                                if (value != "HS")
                                                                                                {
                                                                                                    if (!has_attnd_masterset_notconsider.ContainsKey(attvalue.ToString()))
                                                                                                    {
                                                                                                        if (has_attnd_masterset.ContainsKey(attvalue.ToString()))
                                                                                                        {

                                                                                                            string getval = Convert.ToString(GetCorrespondingKey(attvalue, has_attnd_masterset));

                                                                                                            if (getval.ToString() == "0" || getval.ToString() == "3")
                                                                                                            {
                                                                                                                present_count = Convert.ToInt16(GetCorrespondingKey(consolidate.Rows[i][1].ToString().ToLower().Trim(), has_load_rollno));
                                                                                                                present_count++;
                                                                                                                has_load_rollno[consolidate.Rows[i][1].ToString().ToLower().Trim()] = present_count;
                                                                                                            }

                                                                                                        }
                                                                                                        if (value != "NE")
                                                                                                        {
                                                                                                            present_count = Convert.ToInt16(GetCorrespondingKey(consolidate.Rows[i][1].ToString().Trim().ToLower(), has_total_attnd_hour));
                                                                                                            present_count++;
                                                                                                            has_total_attnd_hour[consolidate.Rows[i][1].ToString().ToLower().Trim()] = present_count;
                                                                                                        }
                                                                                                        if (attvalue.ToString() == "3")
                                                                                                        {
                                                                                                            if (!hatodtot.Contains(rollno.ToLower()))
                                                                                                            {
                                                                                                                hatodtot.Add(rollno.ToLower(), "1");
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                double getods = Convert.ToDouble(GetCorrespondingKey(rollno, hatodtot));
                                                                                                                getods = getods + 1;
                                                                                                                hatodtot[rollno.ToLower()] = getods;
                                                                                                            }
                                                                                                        }
                                                                                                        if (hatabsentvalues.ContainsKey(attvalue.ToString()))
                                                                                                        {
                                                                                                            double getads = Convert.ToDouble(GetCorrespondingKey(rollno.ToLower(), has_total_absent_hour));
                                                                                                            getads = getads + 1;
                                                                                                            has_total_absent_hour[rollno.ToLower()] = getads;
                                                                                                        }

                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
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
                            }
                            temp_date = temp_date.AddDays(1);

                        }

                        if (recflag == true || spl_hr_flag == true)
                        {
                            tolal = 0;
                            double attnd_hr = 0, sub_prc = 0, tot_hr = 0, ondutyvalue = 0;
                            hval.Clear();
                            for (int row_cnt = 3; row_cnt < consolidate.Rows.Count; row_cnt++)
                            {
                                attnd_hr = 0;
                                tot_hr = 0;
                                sub_prc = 0;
                                ondutyvalue = 0;
                                string roll_number = consolidate.Rows[row_cnt][1].ToString().Trim().ToLower();

                                if (has_load_rollno.Contains(roll_number))
                                {
                                    attnd_hr = Convert.ToDouble(GetCorrespondingKey(roll_number, has_load_rollno));
                                    if (!over_per.Contains(roll_number))
                                    {
                                        over_per.Add(roll_number, attnd_hr);
                                    }
                                    else if (over_per.Contains(roll_number))
                                    {
                                        double d = Convert.ToDouble(GetCorrespondingKey(roll_number, over_per));
                                        double da2 = d + attnd_hr;
                                        over_per[roll_number] = da2;
                                    }
                                    if (!over_per1.Contains(roll_number))
                                    {
                                        over_per1.Add(roll_number, attnd_hr);
                                    }
                                    else if (over_per1.Contains(roll_number))
                                    {
                                        double d = Convert.ToDouble(GetCorrespondingKey(roll_number, over_per1));
                                        double da2 = d + attnd_hr;
                                        over_per1[roll_number] = da2;
                                    }

                                }
                                if (has_total_attnd_hour.Contains(roll_number))
                                {
                                    tot_hr = Convert.ToDouble(GetCorrespondingKey(roll_number, has_total_attnd_hour));
                                }
                                if (has_total_onduty_hour.Contains(roll_number))
                                {
                                    ondutyvalue = Convert.ToDouble(GetCorrespondingKey(roll_number, has_total_onduty_hour));
                                }

                                double tot = attnd_hr + ondutyvalue;
                                sub_prc = Math.Round(((tot / tot_hr) * 100), 2);
                                if (sub_prc.ToString().Trim().ToLower() == "nan" || sub_prc.ToString().Trim().ToLower() == "infinity")
                                {
                                    sub_prc = 0;
                                }

                                if (attnd_hr == 0 && tot_hr == 0)
                                {

                                    consolidate.Rows[row_cnt][colcnt - 1] = "-";

                                }
                                else
                                {
                                    no_stud_flag = true;
                                    if (roll_number.ToString() != "")
                                    {
                                        if (tolal < tot_hr)
                                        {
                                            tolal = tot_hr;
                                        }

                                        //consolidate.Rows[2][colcnt] = "H (" + tolal.ToString() + ")";
                                        hval.Add(row_cnt, "H (" + tolal.ToString() + ")");
                                        consolidate.Rows[row_cnt][colcnt] = attnd_hr.ToString();
                                        //FpEntry.Sheets[0].Cells[row_cnt, colcnt].HorizontalAlign = HorizontalAlign.Center;
                                        //FpEntry.Sheets[0].Cells[row_cnt, colcnt].Font.Name = "Book Antiqua";
                                        //FpEntry.Sheets[0].Cells[row_cnt, colcnt].Font.Size = FontUnit.Medium;


                                    }
                                    else
                                    {
                                        consolidate.Rows[row_cnt][colcnt] = "-";
                                        //FpEntry.Sheets[0].Cells[row_cnt, colcnt].HorizontalAlign = HorizontalAlign.Center;
                                        //FpEntry.Sheets[0].Cells[row_cnt, colcnt].Font.Name = "Book Antiqua";
                                        //FpEntry.Sheets[0].Cells[row_cnt, colcnt].Font.Size = FontUnit.Medium;

                                    }
                                }

                            }
                          
                            subtolcont = subtolcont + tolal;
                            subtolcont1233 = subtolcont1233 + Convert.ToInt16(tolal);
                           
                        }
                        else
                        {
                            loadtolss = true;
                            lblnorec.Visible = true;
                            lblnorec.Text = "No Records Found";
                            Button2.Visible = false;
                            gridview1.Visible = false;
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnExcel.Visible = false;
                            btnPrintMaster.Visible = false;
                            return;
                        }
                    }
                }

                if (holiflag == true)
                {
                    if (no_stud_flag == false)
                    {
                        loadtolss = true;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Student(s) Not Available Or Attendance Cant Be Marked";
                        Button2.Visible = false;
                        gridview1.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnExcel.Visible = false;
                        btnPrintMaster.Visible = false;
                        return;
                    }
                }
                else
                {
                    loadtolss = true;
                    lblnorec.Visible = true;
                    lblnorec.Text = "No Records Found";
                    Button2.Visible = false;
                    gridview1.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    btnPrintMaster.Visible = false;
                    return;
                }
               
            }

        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int r = 0;


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int j = 1; j < consolidate.Columns.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        catch
        {
        }
    }
    public string Attmark(string Attstr_mark)
    {
        Att_mark = "";

        if (Attstr_mark == "1")
        {
            Att_mark = "P";

        }

        else if (Attstr_mark == "2")
        {
            Att_mark = "A";

        }
        else if (Attstr_mark == "3")
        {
            Att_mark = "OD";

        }
        else if (Attstr_mark == "4")
        {
            Att_mark = "ML";

        }
        else if (Attstr_mark == "5")
        {
            Att_mark = "SOD";

        }
        else if (Attstr_mark == "6")
        {
            Att_mark = "NSS";

        }
        else if (Attstr_mark == "7")
        {
            Att_mark = "H";

        }
        else if (Attstr_mark == "8")
        {
            Att_mark = "NJ";

        }
        else if (Attstr_mark == "9")
        {
            Att_mark = "S";

        }
        else if (Attstr_mark == "10")
        {
            Att_mark = "L";

        }
        else if (Attstr_mark == "11")
        {
            Att_mark = "NCC";

        }
        else if (Attstr_mark == "12")
        {
            Att_mark = "HS";

        }
        else if (Attstr_mark == "13")
        {
            Att_mark = "PP";
        }
        else if (Attstr_mark == "14")
        {
            Att_mark = "SYOD";
        }
        else if (Attstr_mark == "15")
        {
            Att_mark = "COD";
        }
        else if (Attstr_mark == "16")
        {
            Att_mark = "OOD";
        }
        else
        {
            Att_mark = "NE";
        }
        //return Convert.ToInt32(Att_mark);
        return Att_mark;


    }
    public string Attvalues(string Att_str1)
    {
        string Attvalue;

        Attvalue = "";
        if (Att_str1 == "P")
        {
            Attvalue = "1";

        }
        else if (Att_str1 == "A")
        {
            Attvalue = "2";

        }
        else if (Att_str1 == "OD")
        {
            Attvalue = "3";

        }
        else if (Att_str1 == "ML")
        {
            Attvalue = "4";

        }
        else if (Att_str1 == "SOD")
        {
            Attvalue = "5";
        }

        else if (Att_str1 == "NSS")
        {
            Attvalue = "6";

        }
        else if (Att_str1 == "H")
        {
            Attvalue = "7";

        }

        else if (Att_str1 == "NJ")
        {
            Attvalue = "8";

        }
        else if (Att_str1 == "S")
        {
            Attvalue = "9";

        }
        else if (Att_str1 == "L")
        {
            Attvalue = "10";

        }
        else if (Att_str1 == "NCC")
        {
            Attvalue = "11";

        }
        else if (Att_str1 == "HS")
        {
            Attvalue = "12";
        }

        else if (Att_str1 == "PP")
        {
            Attvalue = "13";
        }
        else if (Att_str1 == "SYOD")
        {
            Attvalue = "14";
        }
        else if (Att_str1 == "COD")
        {
            Attvalue = "15";
        }
        else if (Att_str1 == "OOD")
        {
            Attvalue = "16";
        }
        else
        {
            Attvalue = "NE";
        }
        return Attvalue;
    }

    protected void ddlTest_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            if (ddlTest.SelectedItem.Text == "Select")
            {
                lblnorec.Visible = false;
                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                lblnorecc.Visible = false;
                RadioHeader.Visible = false;
                Radiowithoutheader.Visible = false;
                lblpages.Visible = false;
                ddlpage.Visible = false;
                gridview1.Visible = false;
                lblnorec.Visible = false;
                Button2.Visible = false;
            }
            Buttontotal.Visible = false;
            lblrecord.Visible = false;
            DropDownListpage.Visible = false;
            TextBoxother.Visible = false;
            lblpage.Visible = false;
            TextBoxpage.Visible = false;
            gridview1.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnExcel.Visible = false;
            lblnorecc.Visible = false;
            RadioHeader.Visible = false;
            Radiowithoutheader.Visible = false;
            lblpages.Visible = false;
            ddlpage.Visible = false;
            gridview1.Visible = false;
            lblnorec.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnExcel.Visible = false;
            Button2.Visible = false;
        }
        catch
        {
            lblnorec.Visible = true;
            gridview1.Visible = true;

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
        lblnorec.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        gridview1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        lblnorecc.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        ddlpage.Visible = false;
        lblpages.Visible = false;
        Button2.Visible = false;
        string collegecode = ddlcollege.SelectedValue.ToString();
        string usercode = Session["usercode"].ToString();

        binddegree();
        bindbranch();
        bindsem();
        bindsec();
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
            gridview1.Visible = true;

            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnExcel.Visible = true;
            //FpEntry.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            //CalculateTotalPages();

        }
        //FpEntry.SaveChanges();
        //FpEntry.CurrentPage = 0;
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
                    gridview1.Visible = true;

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
                    //FpEntry.CurrentPage = Convert.ToInt16(TextBoxpage.Text) - 1;
                    gridview1.Visible = true;

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

                //FpEntry.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
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
    //public string Getdate(string Att_strqueryst)
    //{
    //    string sqlstr;
    //    sqlstr = Att_strqueryst;
    //    mycon1.Close();
    //    mycon1.Open();
    //    SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
    //    SqlCommand cmd5a = new SqlCommand(sqlstr);
    //    cmd5a.Connection = mycon1;
    //    SqlDataReader drnew;
    //    drnew = cmd5a.ExecuteReader();
    //    drnew.Read();
    //    if (drnew.HasRows == true)
    //    {
    //        return drnew[0].ToString();
    //    }
    //    else
    //    {
    //        return "";
    //    }

    //}

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
            //Added By Subburaj 21.08.2014****//
            case 18:
                atten = "RAA";
                break;
            //***END*************//
        }
        return atten;


    }
    protected void RadioButtonList3_SelectedIndexChanged(object sender, EventArgs e)
    {

        ddlTest.SelectedIndex = -1;
        TextBoxother.Visible = false;
        TextBoxother.Text = "";
        TextBoxpage.Text = "";
        gridview1.Visible = false;
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
        gridview1.Visible = false;

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
        gridview1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        lblnorecc.Visible = false;
    }
    protected void RadioHeader_CheckedChanged(object sender, EventArgs e)
    {
        ddlpage.Items.Clear();
        int totrowcount = gridview1.Rows.Count;
        int pages = totrowcount / 14;
        int intialrow = 1;
        int remainrows = totrowcount % 14;
        if (gridview1.Rows.Count > 0)
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
            for (int i = 0; i < gridview1.Rows.Count; i++)
            {
                // FpEntry.Sheets[0].Rows[i].Visible = true;
            }
            Double totalRows = 0;
            totalRows = Convert.ToInt32(gridview1.Rows.Count);
            // Session["totalPages"] = (int)Math.Ceiling(totalRows / FpEntry.Sheets[0].PageSize);
            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
            DropDownListpage.Items.Clear();
            if (totalRows >= 10)
            {
                //FpEntry.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                {
                    DropDownListpage.Items.Add((k + 10).ToString());
                }
                DropDownListpage.Items.Add("Others");
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                // FpEntry.Height = 335;

            }
            else if (totalRows == 0)
            {
                DropDownListpage.Items.Add("0");
                // FpEntry.Height = 100;
            }
            else
            {
                // FpEntry.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                //  DropDownListpage.Items.Add(FpEntry.Sheets[0].PageSize.ToString());
                // FpEntry.Height = 100 + (10 * Convert.ToInt32(totalRows));
            }
            if (Convert.ToInt32(gridview1.Rows.Count) > 10)
            {
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                // FpEntry.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
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
        int totrowcount = gridview1.Rows.Count;
        int pages = totrowcount / 14;
        int intialrow = 1;
        int remainrows = totrowcount % 14;
        if (gridview1.Rows.Count > 0)
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
            for (int i = 0; i < gridview1.Rows.Count; i++)
            {
                //FpEntry.Sheets[0].Rows[i].Visible = true;
            }
            Double totalRows = 0;
            totalRows = Convert.ToInt32(gridview1.Rows.Count);
            //  Session["totalPages"] = (int)Math.Ceiling(totalRows / FpEntry.Sheets[0].PageSize);
            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
            DropDownListpage.Items.Clear();
            if (totalRows >= 10)
            {
                // FpEntry.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                {
                    DropDownListpage.Items.Add((k + 10).ToString());
                }
                DropDownListpage.Items.Add("Others");
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                // FpEntry.Height = 335;

            }
            else if (totalRows == 0)
            {
                DropDownListpage.Items.Add("0");
                //FpEntry.Height = 100;
            }
            else
            {
                // FpEntry.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                // DropDownListpage.Items.Add(FpEntry.Sheets[0].PageSize.ToString());
                // FpEntry.Height = 100 + (10 * Convert.ToInt32(totalRows));
            }
            if (Convert.ToInt32(gridview1.Rows.Count) > 10)
            {
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                //FpEntry.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);

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
                SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
                setcon.Close();
                setcon.Open();
                DataSet ds_splhr_query_master = new DataSet();

                string splhr_query_master = "select attendance from specialhr_attendance where roll_no='" + stud_roll + "'  and hrdet_no in(" + hrdetno + ")";
                SqlDataReader dr_splhr_query_master;
                cmd = new SqlCommand(splhr_query_master, setcon);
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
        lblnorec.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        gridview1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        lblnorecc.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;
        ddlpage.Visible = false;
        lblpages.Visible = false;
        bindbatch();
        binddegree();
        bindbranch();
        bindsem();
        bindsec();

        GetTest();
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Printcontrolhed2.loadspreaddetails(gridview1, "Consolidated_report.aspx", "Consolidated Attendance And Mark Details Report @ Date :" + DateTime.Now.ToString("dd/MM/yyyy") + "");
        Printcontrolhed2.Visible = true;
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string strexcelname = txtexcelname.Text;
            if (strexcelname != "")
            {
                d2.printexcelreportgrid(gridview1, strexcelname);
            }
            else
            {
                lblnorecc.Text = "Please enter your Report Name";
                lblnorecc.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }

    }

    public int onduty { get; set; }

    public string querystring { get; set; }

    public DateTime from_date { get; set; }

    public DateTime to_date { get; set; }

    public DateTime f_date { get; set; }

    public DateTime t_date { get; set; }

    public string fdate { get; set; }

    public string tdate { get; set; }

    public DataSet ds_student { get; set; }

    protected void btnPrint_Click1(object sender, EventArgs e)
    {
        string secton = "";
        if (ddlSec.Enabled == true)
        {

            if (ddlSec.SelectedItem.Text == "")
            {
                secton = "";
            }
            else
            {
                secton = "-" + ddlSec.SelectedItem.Text.ToString();
            }
        }
        classs = ddlBranch.SelectedItem.Text.ToString() + secton;
        string Academicyear = "select distinct value from master_settings where settings='Academic year'";
        DataSet Academic = d2.select_method_wo_parameter(Academicyear, "Text");
        if (Academic.Tables[0].Rows.Count > 0)
        {
            Academicyears = Academic.Tables[0].Rows[0]["value"].ToString();
            string[] dsplit123 = Academicyears.Split(new Char[] { ',' });
            Academicyears = dsplit123[0].ToString() + "-" + dsplit123[1].ToString();
        }
        else
        {
            Academicyears = ddlBatch.SelectedValue.ToString();
        }
        Session["column_header_row_count"] = 5;
        string dcommt = "Consolidated Attendance And Mark Details Report" + '@' + "Batch :" + ddlBatch.SelectedItem.ToString() + '@' + "Class :" + classs + '@' + Session["perioftest"].ToString() + '@' + "ACADAMIC YEAR " + Academicyears + " & ODD/EVEN SEMESTER " + '@' + "Date :" + DateTime.Now.ToString("dd/MM/yyyy") + "";
        Printcontrolhed2.loadspreaddetails(gridview1, "Consolidated_report.aspx", dcommt);
        Printcontrolhed2.Visible = true;
    }
    protected void Retest_CheckedChanged(object sender, EventArgs e)
    {
        gridview1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        lblnorecc.Visible = false;
        btnPrint.Visible = false;
        Button2.Visible = false;
    }
}

