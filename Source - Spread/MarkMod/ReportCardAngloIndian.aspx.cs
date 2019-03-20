using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.IO;
using Gios.Pdf;
using System.Configuration;


public partial class ReportCardAngloIndian : System.Web.UI.Page
{
    Boolean serialflag = true;
    DataTable headerrights = new DataTable();
    double cgpacalc = 0;
    DataTable dtallcol = new DataTable();
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = string.Empty;
    string strquery = string.Empty;
    string regularflag = string.Empty;
    string genderflag = string.Empty;
    string strdayflag = string.Empty;
    string sql = string.Empty;
    string sqlcondition = string.Empty;
    string collcode = string.Empty;
    string batchyear = string.Empty;
    string degreecode = string.Empty;
    string term = string.Empty;
    string sec = string.Empty;
    string rollnos = string.Empty;
    string currentsem = string.Empty;
    string strorder = string.Empty;
    string strregorder = string.Empty;
    DataSet dsdel = new DataSet();
    ArrayList termselected = new ArrayList();
    ArrayList avoidrows = new ArrayList();
    ArrayList avg_grade_col = new ArrayList();
    ArrayList faillist = new ArrayList();
    ArrayList subfaillist = new ArrayList();
    Boolean booleanheaderformat1 = true;
    DataTable dtFASAcol = new DataTable();
    DataTable dtallotherscol = new DataTable();

    Boolean teamlast = false;
    int twosubcount = 0;
    DataSet ds_subject = new DataSet();
    DataSet otherds_subject = new DataSet();
    string otherssubjectcode = string.Empty;
    string otherssubjectcode01 = string.Empty;
    string total500 = string.Empty;
    int subjectscount = 0;

    TreeNode node;
    TreeNode subchildnode;

    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataSet studgradeds = new DataSet();

    FarPoint.Web.Spread.ComboBoxCellType combocol = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType combocolgrade = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxcol = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxsel_all = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.TextCellType txtceltype = new FarPoint.Web.Spread.TextCellType();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds5 = new DataSet();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double per_holidate;
    int tot_per_hrs;
    double njhr, njdate, per_njdate;
    double per_per_hrs;

    DataSet ds_attnd_pts = new DataSet();
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    string value_holi_status = string.Empty;
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = string.Empty;

    string startdate = string.Empty;
    string enddate = string.Empty;
    string tempvalue = "-1";
    Boolean yesflag = false;

    Hashtable hatonduty = new Hashtable();
    static Hashtable ht_sphr = new Hashtable();
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    double per_perhrs, per_abshrs;
    double per_ondu, per_leave, per_hhday;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    string working = string.Empty;
    string present = string.Empty;
    string working1 = string.Empty;
    string present1 = string.Empty;
    string fvalue = string.Empty;
    string lvalue = string.Empty;

    int ObtValue = -1;
    TimeSpan ts;
    int rows_count;
    string value, date;
    string halforfull = "", mng = "", evng = "", holiday_sched_details = string.Empty;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0, tot_ml_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;
    int notconsider_value = 0;
    int next = 0;
    int moncount;
    double dif_date = 0;
    double dif_date1 = 0;
    int cal_from_date;
    int cal_to_date;
    string criteria_no = string.Empty;
    DateTime per_from_date;
    DateTime per_to_date;
    DateTime dumm_from_date;
    DateTime Admission_date;
    static Boolean splhr_flag = false;
    string diff_date;
    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    int minpresII = 0;
    int mmyycount;
    int count = 0;
    string strorderby = string.Empty;
    string lbltot_att1 = string.Empty;
    string lbltot_work1 = string.Empty;
    string lbltot_att2 = string.Empty;
    string lbltot_work2 = string.Empty;
    ArrayList gradesystemfa = new ArrayList();
    ArrayList gradesystemsa = new ArrayList();
    //ArrayList rights = new ArrayList();
    string grouporusercode = string.Empty;

    ArrayList rights = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            lblErr.Visible = false;
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
                lblerrormsg.Visible = false;
                lblerrormsg.Text = string.Empty;
                //lblerrmsg2.Tex
                btnmatric_page1.Visible = false;
                btnmatric_page2.Visible = false;
                bindschool();
                bindyear();
                bindschooltype();
                bindstandard();
                bindterm();
                bindsec();
                treeview_spreadfields.Visible = true;
                treeview_spreadfields.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " and group_code=" + Convert.ToString(Session["group_code"]).Trim() + " and rights_code in (13011,13012,13013,13014,13015,13016,13017,13018,13019,13020)";
                }
                else
                {
                    grouporusercode = " and user_code=" + Convert.ToString(Session["usercode"]).Trim() + " and rights_code in (13011,13012,13013,13014,13015,13016,13017,13018,13019,13020,13021)";
                }

                //ddlreporttype.Items.Add( "CBSE");
                //ddlreporttype.Items.Insert(1, "Report Card  VI- VIII");
                //ddlreporttype.Items.Insert(2, "Report Card XI - XII");
                //ddlreporttype.Items.Insert(3, "Anglo Indian");
                //ddlreporttype.Items.Insert(4, "Matric Report Card 9th & 10th");
                //ddlreporttype.Items.Insert(5, "Anglo Indian Report Card Xth,XIth & XIIth");
                //ddlreporttype.Items.Insert(6, "ICSE Reportcard I - V");
                //ddlreporttype.Items.Insert(7, "ICSE Reportcard VI - VIII");
                //ddlreporttype.Items.Insert(8, "ICSE Reportcard IX - X");
                //ddlreporttype.Items.Insert(9, "ICSE Reportcard XI - XII");

                //if (rights.Contains("13011"))
                //{
                //    ddlreporttype.Items.Add(new ListItem("CBSE", "0"));
                //}
                //if (rights.Contains("13012"))
                //{
                //    ddlreporttype.Items.Add(new ListItem("Report Card  VI- VIII", "1"));
                //}
                //if (rights.Contains("13013"))
                //{
                //    ddlreporttype.Items.Add(new ListItem("Report Card XI - XII", "2"));

                //}
                //if (rights.Contains("13014"))
                //{
                //    ddlreporttype.Items.Add(new ListItem("Anglo Indian", "3"));
                //}
                //if (rights.Contains("13015"))
                //{
                //    ddlreporttype.Items.Add(new ListItem("Matric Report Card 9th & 10th", "4"));
                //}
                //if (rights.Contains("13016"))
                //{
                //    ddlreporttype.Items.Add(new ListItem("Anglo Indian Report Card Xth,XIth & XIIth", "5"));
                //}
                //if (rights.Contains("13017"))
                //{
                //    ddlreporttype.Items.Add(new ListItem("ICSE Reportcard I - V", "6"));
                //}
                //if (rights.Contains("13018"))
                //{
                //    ddlreporttype.Items.Add(new ListItem("ICSE Reportcard VI - VIII", "7"));
                //}
                //if (rights.Contains("13019"))
                //{
                //    ddlreporttype.Items.Add(new ListItem("ICSE Reportcard IX - X", "8"));
                //}
                //if (rights.Contains("13020"))
                //{
                //    ddlreporttype.Items.Add(new ListItem("ICSE Reportcard XI - XII", "9"));
                //}
                //if (rights.Contains("13021"))
                //{
                //    ddlreporttype.Items.Add(new ListItem("Nursery-LKG & UKG Reportcard", "10"));
                //}
                //ddlreporttype.Items.Add(new ListItem("LKG & UKG Reportcard", "11"));
                ////if (rights.Contains("13020"))
                ////{
                ////    ddlreporttype.Items.Add(new ListItem("Nursery-LKG & UKG Reportcard","10"));
                ////}
                ddlreporttype.Items.Clear();
                ddlreporttype.Items.Insert(0, new ListItem("Anglo Indian", "3"));
                ddlreporttype.SelectedIndex = 0;

                btngrade.Visible = false;
                btnrpt.Visible = false;
                lblTest.Visible = false;
                ptest.Visible = false;
                txt_Test.Visible = false;

                ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Dec", "12"));

                int year;
                year = Convert.ToInt16(DateTime.Today.Year);
                ddlYear.Items.Clear();
                for (int l = 0; l <= 20; l++)
                {
                    ddlYear.Items.Add(Convert.ToString(year - l));
                }

                fpspread.Visible = false;
                FpSpread1.Visible = false;
                FpSpread1.Height = 350;
                FpSpread1.Width = 538;

                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.CommandBar.Visible = false;

                FpSpread1.Sheets[0].RowCount = 1;
                FpSpread1.Sheets[0].ColumnCount = 5;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 40;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";

                FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 50;
                FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 120;
                FpSpread1.Sheets[0].ColumnHeader.Columns[3].Width = 253;
                FpSpread1.Sheets[0].ColumnHeader.Columns[4].Width = 77;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;

                for (int i = 0; i < 5; i++)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
                }

                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = " ";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Sex";
                chkboxsel_all.AutoPostBack = true;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
                //darkstyle.ForeColor = System.Drawing.Color.Black;
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Medium;
                darkstyle.Border.BorderSize = 0;
                darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].CellType = chkboxsel_all;
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

                for (int g = 0; g < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; g++)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].ForeColor = Color.White;
                }

                for (int g = 0; g < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; g++)
                {
                    FpSpread1.Sheets[0].Columns[g].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[g].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Columns[g].ForeColor = Color.Black;
                }

                FpSpread1.Sheets[0].Cells[0, 1].CellType = chkboxsel_all;
                FpSpread1.SaveChanges();

                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                loadheader();
                ddlreporttype_OnSelectedIndexChanged(sender, e);
                //---------------------------
            }
            else
            {
                //    fpspread.Visible = true;
            }

            if (ddschool.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddschool.SelectedValue);
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    protected void Fpspread1_Command(object sender, EventArgs e)
    {
        lblerrormsg.Visible = false;
        lblerrormsg.Text = string.Empty;
        if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 1].Value) == 1)
        {
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Cells[i, 1].Value = 1;
            }
        }
        else if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 1].Value) == 0)
        {
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
            }

        }

        FpSpread1.Visible = true;
    }

    public void bindschool()
    {
        try
        {
            lblerrormsg.Visible = false;
            lblerrormsg.Text = string.Empty;
            string columnfield = string.Empty;
            usercode = Convert.ToString(Session["UserCode"]);
            group_code = Convert.ToString(Session["group_code"]);
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_code).Trim() != "") && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]) + "'";
            }
            hat.Clear();
            hat.Add("column_field", Convert.ToString(columnfield));
            studgradeds.Clear();
            studgradeds = d2.select_method("bind_college", hat, "sp");
            ddschool.Items.Clear();
            if (studgradeds.Tables[0].Rows.Count > 0)
            {
                ddschool.DataSource = studgradeds;
                ddschool.DataTextField = "collname";
                ddschool.DataValueField = "college_code";
                ddschool.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void bindyear()
    {
        try
        {
            lblerrormsg.Visible = false;
            lblerrormsg.Text = string.Empty;
            dropyear.Items.Clear();
            studgradeds.Clear();
            studgradeds = d2.select_method_wo_parameter("bind_batch", "sp");

            int count = studgradeds.Tables[0].Rows.Count;
            if (count > 0)
            {
                dropyear.DataSource = studgradeds;
                dropyear.DataTextField = "batch_year";
                dropyear.DataValueField = "batch_year";
                dropyear.DataBind();
            }
            if (studgradeds.Tables[1].Rows.Count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(Convert.ToString(studgradeds.Tables[1].Rows[0][0]));
                dropyear.SelectedValue = Convert.ToString(max_bat);
            }
            dropyear.Text = "batch (" + 1 + ")";
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void bindschooltype()
    {
        try
        {
            lblerrormsg.Visible = false;
            lblerrormsg.Text = string.Empty;
            ddschooltype.Items.Clear();
            usercode = Convert.ToString(Session["usercode"]);
            collegecode = Convert.ToString(ddschool.SelectedItem.Value);
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            studgradeds.Clear();
            studgradeds = d2.select_method("bind_degree", hat, "sp");
            if (studgradeds.Tables[0].Rows.Count > 0)
            {
                ddschooltype.DataSource = studgradeds;
                ddschooltype.DataTextField = "course_name";
                ddschooltype.DataValueField = "course_id";
                ddschooltype.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void bindstandard()
    {
        try
        {
            lblerrormsg.Visible = false;
            lblerrormsg.Text = string.Empty;
            hat.Clear();
            usercode = Convert.ToString(Session["usercode"]);
            //collegecode = Convert.ToString(collegecode);
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", Convert.ToString(ddschooltype.SelectedValue));
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            studgradeds.Clear();
            studgradeds = d2.select_method("bind_branch", hat, "sp");
            if (studgradeds.Tables[0].Rows.Count > 0)
            {
                ddstandard.DataSource = studgradeds;
                ddstandard.DataTextField = "dept_name";
                ddstandard.DataValueField = "degree_code";
                ddstandard.DataBind();
                bindterm();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void bindterm()
    {
        try
        {
            //dropterm.Items.Clear();
            //Boolean first_year;
            //first_year = false;
            //int duration = 0;
            //int i = 0;
            //string strstandard =string.Empty;

            //if (ddstandard.SelectedValue != "")
            //{
            //    strstandard = ddstandard.SelectedValue;
            //}

            //if (strstandard.Trim() != "")
            //{
            //    strstandard = " and degree_code in(" + strstandard + ")";
            //}

            //strquery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + Convert.ToString(ddschool.SelectedValue) + " and batch_year=" + Convert.ToString(dropyear.Text) + " and degree_code=" + Convert.ToString(ddstandard.Text) + " order by NDurations desc";
            //studgradeds.Reset();
            //studgradeds.Dispose();
            ////  studgradeds = d2.select_method_wo_parameter(strquery, "Text");
            //studgradeds = d2.BindSem(Convert.ToString(ddstandard.Text), Convert.ToString(dropyear.Text), Convert.ToString(ddschool.SelectedValue));
            //if (studgradeds.Tables[0].Rows.Count > 0)
            //{
            //    first_year = Convert.ToBoolean(Convert.ToString(studgradeds.Tables[0].Rows[0][1]));
            //    duration = Convert.ToInt16(Convert.ToString(studgradeds.Tables[0].Rows[0][0]));
            //    for (i = 1; i <= duration; i++)
            //    {
            //        if (first_year == false)
            //        {
            //            dropterm.Items.Add(Convert.ToString(i));
            //        }
            //        else if (first_year == true && i != 2)
            //        {
            //            dropterm.Items.Add(Convert.ToString(i));
            //        }
            //    }
            //}
            //else
            //{
            //    strquery = "select distinct duration,first_year_nonsemester  from degree where college_code=" + Convert.ToString(ddschool.SelectedValue) + " " + Convert.ToString(ddstandard.SelectedValue) + " order by duration desc";
            //    studgradeds.Reset();
            //    studgradeds.Dispose();
            //    studgradeds = d2.select_method_wo_parameter(strquery, "Text");
            //    if (studgradeds.Tables[0].Rows.Count > 0)
            //    {
            //        first_year = Convert.ToBoolean(Convert.ToString(studgradeds.Tables[0].Rows[0][1]));
            //        duration = Convert.ToInt16(Convert.ToString(studgradeds.Tables[0].Rows[0][0]));

            //        for (i = 1; i <= duration; i++)
            //        {
            //            if (first_year == false)
            //            {
            //                dropterm.Items.Add(Convert.ToString(i));
            //            }
            //            else if (first_year == true && i != 2)
            //            {
            //                dropterm.Items.Add(Convert.ToString(i));
            //            }
            //        }
            //    }
            //}
            lblerrormsg.Visible = false;
            lblerrormsg.Text = string.Empty;

            dropterm.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;

            string sqlnew = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + Convert.ToString(ddstandard.SelectedValue) + " and batch_year=" + Convert.ToString(dropyear.Text) + " and college_code=" + Convert.ToString(ddschool.SelectedValue) + "";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = d2.select_method_wo_parameter(sqlnew, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]));
                duration = Convert.ToInt16(Convert.ToString(ds.Tables[0].Rows[0][0]));
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        dropterm.Items.Add(Convert.ToString(i));
                        //ddlSemYr.Enabled = false;
                    }
                    else if (first_year == true && i == 2)
                    {
                        dropterm.Items.Add(Convert.ToString(i));
                    }

                }
            }
            else
            {
                sqlnew = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + Convert.ToString(ddstandard.SelectedValue) + " and college_code=" + Convert.ToString(ddschool.SelectedValue) + "";

                ds.Clear();
                ds = d2.select_method_wo_parameter(sqlnew, "Text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]));
                    //duration = Convert.ToInt16(Convert.ToString(ds.Tables[0].Rows[0][0]));

                    bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]), out first_year);
                    int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]), out duration);
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            dropterm.Items.Add(Convert.ToString(i));
                        }
                        else if (first_year == true && i != 2)
                        {
                            dropterm.Items.Add(Convert.ToString(i));
                        }
                    }
                }
            }
            if (dropterm.Items.Count > 0)
            {
                dropterm.SelectedIndex = 0;
                bindsec();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    protected void go_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    #region Logout

    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
        }
    }

    #endregion Logout

    protected void ddschool_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
            lblerrormsg.Text = string.Empty;
            bindyear();
            bindschooltype();
            bindstandard();
            bindterm();
            GetTest();
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            btngrade.Visible = false;
            btnrpt.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    protected void dropyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
            lblerrormsg.Text = string.Empty;
            bindschooltype();
            bindstandard();
            bindterm();
            bindsec();
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            btngrade.Visible = false;
            btnrpt.Visible = false;
            GetTest();
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    protected void dropschooltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
            lblerrormsg.Text = string.Empty;
            bindstandard();
            bindterm();
            bindsec();
            GetTest();
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            btngrade.Visible = false;
            btnrpt.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    protected void ddstandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
            lblerrormsg.Text = string.Empty;
            bindterm();
            bindsec();
            loadheader();
            GetTest();
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            btngrade.Visible = false;
            btnrpt.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    protected void ddlreporttype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
            lblerrormsg.Text = string.Empty;

            lblErr.Text = string.Empty;
            lblErr.Visible = false;

            FpSpread1.Visible = false;
            btngrade.Visible = false;
            btnrpt.Visible = false;

            btnmatric_page1.Visible = false;
            btnmatric_page2.Visible = false;

            if (ddlreporttype.Items.Count > 0)
            {
                if (Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 3 || Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 4)
                {
                    //GetTest();
                    btngrade.Visible = false;
                    Label1.Visible = false;
                    txtaccheader.Visible = false;
                    paccheader.Visible = false;
                    lblTest.Visible = false;
                    ptest.Visible = false;
                    txt_Test.Visible = false;
                    lblExamMonth.Visible = false;
                    lblExamYear.Visible = false;
                    ddlMonth.Visible = false;
                    ddlYear.Visible = false;
                    // btngo.Attributes.Add("Style", "margin-left: -171px;");452px
                    btngo.Attributes.Add("Style", "background-color: silver;    border: 2px solid white;    color: Black;    font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    height: 27px;    margin-left: 452px;    margin-top: 40px;    position: absolute;    width: 42px;");
                }
                else
                {
                    //btngrade.Visible = true;
                    lblTest.Visible = false;
                    ptest.Visible = false;
                    txt_Test.Visible = false;
                    lblExamMonth.Visible = false;
                    lblExamYear.Visible = false;
                    ddlMonth.Visible = false;
                    ddlYear.Visible = false;
                    Label1.Visible = true;
                    txtaccheader.Visible = true;
                    paccheader.Visible = true;
                    btngo.Attributes.Add("Style", "background-color: silver;    border: 2px solid white;    color: Black;    font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    height: 27px;    margin-left: -261px;    margin-top: 40px;    position: absolute;    width: 42px;");
                }
                if (Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 3)
                {
                    btngrade.Visible = false;
                    Label1.Visible = false;
                    txtaccheader.Visible = false;
                    paccheader.Visible = false;
                    lblTest.Visible = false;
                    ptest.Visible = false;
                    txt_Test.Visible = false;
                    lblExamMonth.Visible = false;
                    lblExamYear.Visible = false;
                    ddlMonth.Visible = false;
                    ddlYear.Visible = false;
                    // btngo.Attributes.Add("Style", "margin-left: -171px;");452px
                    btngo.Attributes.Add("Style", "background-color: silver;    border: 2px solid white;    color: Black;    font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    height: 27px;    margin-left: 452px;    margin-top: 4px;    position: absolute;    width: 42px;");
                }

            }
            else
            {
                lblerrormsg.Text = "Please Check The Rights And Change The Rights";
                lblerrormsg.Visible = true;
            }
            //else if (Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 6 || Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 7 || Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 8 || Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 9)
            //{
            //    //GetTest();
            //    btngrade.Visible = false;
            //    Label1.Visible = false;
            //    txtaccheader.Visible = false;
            //    paccheader.Visible = false;
            //    //lblTest.Visible = false;
            //    //ptest.Visible = false;
            //    //txt_Test.Visible = false;
            //    //lblExamMonth.Visible = false;
            //    //lblExamYear.Visible = false;
            //    //ddlMonth.Visible = false;
            //    //ddlYear.Visible = false;
            //    btngrade.Visible = false;
            //}
            //if (Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 4)
            //{
            //    btngrade.Visible = false;
            //    btnmatric_page1.Visible = true;
            //    btnmatric_page2.Visible = true;
            //    btnrpt.Visible = false;

            //}
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    protected void dropterm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
            lblerrormsg.Text = string.Empty;

            bindsec();
            loadheader();
            GetTest();
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            btngrade.Visible = false;
            btnrpt.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    protected void dropsec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
            lblerrormsg.Text = string.Empty;

            FpSpread1.Visible = false;
            btngrade.Visible = false;
            btnrpt.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public string loadmarkat(string mr)
    {
        lblerrormsg.Visible = false;
        lblerrormsg.Text = string.Empty;
        string strgetval = string.Empty;
        if (mr == "-1")
        {
            strgetval = "AB";
        }
        else if (mr == "-2")
        {
            strgetval = "EL";
        }
        else if (mr == "-3")
        {
            strgetval = "EOD";
        }
        else if (mr == "-4")
        {
            strgetval = "ML";
        }
        else if (mr == "-5")
        {
            strgetval = "SOD";
        }
        else if (mr == "-6")
        {
            strgetval = "NSS";
        }
        else if (mr == "-7")
        {
            strgetval = "NJ";
        }
        else if (mr == "-8")
        {
            strgetval = "S";
        }
        else if (mr == "-9")
        {
            strgetval = "L";
        }
        else if (mr == "-10")
        {
            strgetval = "NCC";
        }
        else if (mr == "-11")
        {
            strgetval = "HS";
        }
        else if (mr == "-12")
        {
            strgetval = "PP";
        }
        else if (mr == "-13")
        {
            strgetval = "SYOD";
        }
        else if (mr == "-14")
        {
            strgetval = "COD";
        }
        else if (mr == "-15")
        {
            strgetval = "OOD";
        }
        else if (mr == "-16")
        {
            strgetval = "OD";
        }
        else if (mr == "-17")
        {
            strgetval = "LA";
        }
        else if (mr == "-18")
        {
            strgetval = "RAA";
        }
        return strgetval;
    }

    protected void btngrade_Click(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
            lblerrormsg.Text = string.Empty;

            System.Drawing.Font Fontboldhead = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
            System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
            System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontbold1 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
            System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
            System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
            System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
            System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
            System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            System.Drawing.Font Fontmediumv = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
            System.Drawing.Font Fontmedium1V = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);

            System.Drawing.Font f1 = new System.Drawing.Font("Book Antiqua", 7, FontStyle.Regular);
            System.Drawing.Font f2 = new System.Drawing.Font("Book Antiqua", 8, FontStyle.Regular);
            System.Drawing.Font f3 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
            System.Drawing.Font f4 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
            System.Drawing.Font f5 = new System.Drawing.Font("Book Antiqua", 11, FontStyle.Regular);
            System.Drawing.Font f6 = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);

            System.Drawing.Font f7 = new System.Drawing.Font("Book Antiqua", 7, FontStyle.Bold);
            System.Drawing.Font f8 = new System.Drawing.Font("Book Antiqua", 8, FontStyle.Bold);
            System.Drawing.Font f9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold);
            System.Drawing.Font f10 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
            System.Drawing.Font f11 = new System.Drawing.Font("Book Antiqua", 11, FontStyle.Bold);
            System.Drawing.Font f12 = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            rollnos = string.Empty;
            FpSpread1.SaveChanges();
            int checkedcount = 0;
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 1].Value) == 1)
                {
                    checkedcount++;
                }
            }

            string parttitle1a = string.Empty;
            Boolean flag = true;
            ArrayList arrcourrid = new ArrayList();
            ArrayList partcolumnnames = new ArrayList();

            System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);

            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 1].Value) == 1)
                {
                    if (rollnos == "")
                    {
                        rollnos = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                    }
                    else
                    {
                        rollnos = rollnos + "','" + Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                    }
                }
            }

            if (rollnos != "")
            {
                sql = "select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No in ('" + rollnos + "') ;";
                studgradeds.Clear();
                studgradeds = d2.select_method_wo_parameter(sql, "text");
                if (studgradeds.Tables[0].Rows.Count > 0)
                {
                    bool isCamCal = false;
                    string errormsg = string.Empty;
                    for (int roll = 0; roll < studgradeds.Tables[0].Rows.Count; roll++)
                    {
                        string rcrollno = string.Empty;
                        rcrollno = Convert.ToString(studgradeds.Tables[0].Rows[roll][1]);
                        bindstudentmark(rcrollno);
                        // bindbutn(rcrollno);

                        isCamCal = false;

                        DataSet ds = new DataSet();
                        DataSet partsds = new DataSet();
                        DAccess2 da = new DAccess2();
                        string stdappno = string.Empty;

                        string Roll_No = rcrollno;
                        sql = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No='" + Roll_No + "'";
                        ds.Clear();
                        ds.Dispose();
                        ds = da.select_method_wo_parameter(sql, "Text");
                        DataView dv = new DataView();
                        ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                        dv = ds.Tables[1].DefaultView;
                        int count4 = 0;
                        count4 = dv.Count;

                        if (ds.Tables[0].Rows.Count > 0 && count4 > 0)
                        {
                            string studname = Convert.ToString(dv[0]["stud_name"]);
                            string course = Convert.ToString(dv[0]["Dept_Name"]);
                            string admitno = Convert.ToString(dv[0]["roll_admit"]);
                            string admdate = Convert.ToString(dv[0]["adm_date"]);
                            string currentsem = Convert.ToString(dv[0]["Current_Semester"]);
                            string batchyear = Convert.ToString(dv[0]["Batch_Year"]);
                            string degreecode = Convert.ToString(dv[0]["degree_code"]);
                            stdappno = Convert.ToString(dv[0]["App_No"]);
                            string allsem = "1";
                            if (Convert.ToInt32(currentsem) > 1)
                            {
                                int term = Convert.ToInt32(currentsem);
                                for (int i = 2; i <= term; i++)
                                {
                                    allsem = allsem + "'" + "," + "'" + i;
                                }
                            }
                            if (Convert.ToInt32(currentsem) >= 1)
                            {
                                int term = Convert.ToInt32(currentsem);
                                for (int i = 1; i <= term; i++)
                                {
                                    string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + i + "' and degree_code='" + degreecode + "' and batch_year='" + batchyear + "'";
                                    DataSet dset = da.select_method_wo_parameter(sem, "Text");

                                    if (dset.Tables[0].Rows.Count > 0)
                                    {
                                        string startdate = Convert.ToString(dset.Tables[0].Rows[0]["start_date"]);
                                        string enddate = Convert.ToString(dset.Tables[0].Rows[0]["end_date"]);
                                        persentmonthcal(Roll_No, admdate, startdate, enddate);
                                    }
                                    if (i == 1)
                                    {
                                        lbltot_att1 = Convert.ToString(pre_present_date);
                                        lbltot_work1 = Convert.ToString(per_workingdays);
                                        working1 = Convert.ToString(pre_present_date);
                                        present1 = Convert.ToString(per_workingdays);
                                    }
                                }
                            }
                            string table1sql = "select * from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'  and semester = '" + currentsem + "' ; SELECT COUNT(*) as facount FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "'); SELECT Istype,CRITERIA_NO,y.semester FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "') ; SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "')";
                            DataSet ds1fortable1 = new DataSet();
                            ds1fortable1.Clear();
                            ds1fortable1.Dispose();
                            ds1fortable1 = da.select_method_wo_parameter(table1sql, "Text");
                            DataView dvforpage2 = new DataView();

                            Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                            //Gios.Pdf.PdfPage mypdfpage1 = mydoc.NewPage();
                            //Gios.Pdf.PdfPage mypdfpage2 = mydoc.NewPage();
                            //Gios.Pdf.PdfPage mypdfpage6 = mydoc.NewPage();

                            PdfTextArea collinfo = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(ds.Tables[0].Rows[0][0]) + "");
                            mypdfpage.Add(collinfo);
                            collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(ds.Tables[0].Rows[0][1]) + "");
                            mypdfpage.Add(collinfo);
                            string address = Convert.ToString(ds.Tables[0].Rows[0][2]) + "" + Convert.ToString(ds.Tables[0].Rows[0][3]) + "" + Convert.ToString(ds.Tables[0].Rows[0][4]);
                            collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 55, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
                            mypdfpage.Add(collinfo);
                            collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 70, 595, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  :" + Convert.ToString(ds.Tables[0].Rows[0][6]));
                            mypdfpage.Add(collinfo);

                            PdfArea pa1 = new PdfArea(mydoc, 14, 12, 564, 821);
                            PdfRectangle border = new PdfRectangle(mydoc, pa1, Color.Black);
                            mypdfpage.Add(border);

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 50, 96, 450);
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                mypdfpage.Add(LogoImage1, 280, 96, 450);
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"));
                                mypdfpage.Add(LogoImage2, 450, 96, 450);
                            }
                            else
                            {
                                Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                mypdfpage.Add(LogoImage2, 450, 96, 270);
                            }

                            //Hashtable hatsubject = new Hashtable();
                            //Hashtable hatcriter = new Hashtable();
                            //PdfTextArea partinfo = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 180, 595, 50), System.Drawing.ContentAlignment.TopCenter, "co schemadsfmoi sfds");
                            //mypdfpage.Add(partinfo);
                            Gios.Pdf.PdfTable studinfo = mydoc.NewTable(Fontsmall1, 2, 7, 1);
                            studinfo.VisibleHeaders = false;
                            studinfo.SetBorders(Color.Black, 1, BorderType.None);
                            studinfo.Columns[0].SetWidth(20);
                            studinfo.Columns[1].SetWidth(4);
                            studinfo.Columns[2].SetWidth(70);
                            studinfo.Columns[3].SetWidth(110);
                            studinfo.Columns[4].SetWidth(22);
                            studinfo.Columns[5].SetWidth(4);
                            studinfo.Columns[6].SetWidth(30);

                            for (int i = 0; i < 7; i++)
                            {
                                studinfo.Columns[i].SetContentAlignment(ContentAlignment.MiddleLeft);
                            }

                            studinfo.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
                            studinfo.Columns[5].SetContentAlignment(ContentAlignment.MiddleCenter);
                            for (int i = 0; i < 2; i++)
                            {
                                studinfo.Cell(i, 1).SetContent(":");
                                studinfo.Cell(i, 5).SetContent(":");
                            }
                            studinfo.Cell(0, 0).SetContent("Name");
                            studinfo.Cell(0, 0).SetFont(Fontsmall1bold);
                            studinfo.Cell(1, 0).SetContent("Course");
                            studinfo.Cell(1, 0).SetFont(Fontsmall1bold);
                            studinfo.Cell(0, 2).SetContent(studname);
                            studinfo.Cell(1, 2).SetContent(course);

                            studinfo.Cell(0, 4).SetContent("Adm No.");
                            studinfo.Cell(0, 4).SetFont(Fontsmall1bold);
                            studinfo.Cell(1, 4).SetContent("Batch");
                            studinfo.Cell(1, 4).SetFont(Fontsmall1bold);
                            studinfo.Cell(0, 6).SetContent(admitno);
                            studinfo.Cell(1, 6).SetContent(batchyear);
                            Gios.Pdf.PdfTablePage addtabletopage = studinfo.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 180, 553, 600));
                            mypdfpage.Add(addtabletopage);

                            string part1nametitle = d2.GetFunction("select TextVal as part1 from CoCurr_Activitie ca,textvaltable tv where ca.Title_Name=tv.TextCode and TextCriteria = 'RTnam' and ca.Degree_Code='" + degreecode + "' and Batch_Year='" + batchyear + "' and SubTitle='1a' ");

                            PdfTextArea parttitiles = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 22, 210, 595, 50), System.Drawing.ContentAlignment.TopLeft, part1nametitle);
                            mypdfpage.Add(parttitiles);

                            DataTable term1dt = new DataTable();
                            DataTable term2dt = new DataTable();
                            if (dropterm.SelectedItem.Text == "1")
                            {
                                term1dt.Clear();
                                term1dt.Columns.Add("Subject");
                                term1dt.Columns.Add("FA1");
                                term1dt.Columns.Add("FA2");
                                term1dt.Columns.Add("SA1");
                                term1dt.Columns.Add("Total");

                                for (int i = 0; i < 2; i++)
                                {
                                    term1dt.Rows.Add("", "", "", "");
                                }
                            }
                            int rowcountspread = fpspread.Sheets[0].RowCount + 4;
                            int columncountspread = fpspread.Sheets[0].ColumnCount;

                            Gios.Pdf.PdfTable table1forpage2;
                            if (dropterm.SelectedItem.Text == "1")
                            {
                                table1forpage2 = mydoc.NewTable(Fontsmall1, rowcountspread, columncountspread, 6);
                            }
                            else
                            {
                                table1forpage2 = mydoc.NewTable(Fontsmall1, rowcountspread, columncountspread, 6);
                            }
                            //Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(Fontsmall1, rowcountspread, columncountspread, 1);
                            table1forpage2.VisibleHeaders = false;
                            table1forpage2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            //table1forpage2.Columns[1].SetWidth(25);
                            // table1forpage2.Columns[0].SetWidth(25);

                            int ss = fpspread.Sheets[0].ColumnHeader.RowCount;


                            int sk = 1, sk1 = 1;

                            if (dropterm.SelectedItem.Text == "1" && columncountspread == 5)
                            {
                                //table1forpage2.Cell(0, 0).SetContent("S.No");
                                isCamCal = true;
                                table1forpage2.Cell(0, 0).SetContent("SCHOLASTIC AREA");
                                table1forpage2.Cell(0, 1).SetContent("TERM-I");
                                table1forpage2.Cell(1, 0).SetContent("Subject");
                                table1forpage2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage2.Cell(1, 1).SetContent("Formative Assessment-1");
                                table1forpage2.Cell(1, 2).SetContent("Formative Assessment-2");
                                table1forpage2.Cell(1, 3).SetContent("Summative Assessment-1");
                                table1forpage2.Cell(1, 4).SetContent("TOTAL  (FA1+FA2+SA1)");

                                //table1forpage2.Columns[1].SetWidth(20);
                                //table1forpage2.Columns[2].SetWidth(20);
                                //table1forpage2.Columns[3].SetWidth(20);
                                //table1forpage2.Columns[4].SetWidth(20);
                                //shree
                                foreach (PdfCell pr in table1forpage2.CellRange(0, 1, 0, 1).Cells)
                                {
                                    pr.ColSpan = 4;
                                }

                                for (int ii = 0; ii < 5; ii++)
                                {
                                    table1forpage2.Cell(0, ii).SetFont(Fontboldhead);
                                    table1forpage2.Cell(1, ii).SetFont(Fontboldhead);
                                    table1forpage2.Cell(0, ii).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2.Cell(1, ii).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2.Cell(0, ii).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));
                                    table1forpage2.Cell(1, ii).SetColors(Color.Black, ColorTranslator.FromHtml("#fccd99"));
                                }

                                for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                                {
                                    for (int j = 0; j < columncountspread; j++)
                                    {
                                        string coldata = fpspread.Sheets[0].Cells[i, j].Text;
                                        table1forpage2.Cell(i + 2, j).SetContentAlignment(ContentAlignment.MiddleCenter);

                                        table1forpage2.Cell(i + 2, j).SetContent(coldata);
                                    }
                                }
                            }
                            else if (columncountspread == 13)
                            {
                                isCamCal = true;
                                //table1forpage2.Cell(0, 0).SetContent("S.No");
                                table1forpage2.Cell(0, 0).SetContent("         SCHOLASTIC AREA      (9 Point Scale)");
                                table1forpage2.Cell(0, 1).SetContent("TERM-I");
                                table1forpage2.Cell(0, 5).SetContent("TERM-II");
                                table1forpage2.Cell(0, 9).SetContent("FINAL ASSESSMENT");
                                table1forpage2.Cell(1, 0).SetContent("Subject");
                                table1forpage2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage2.Cell(1, 1).SetContent("FA1 10%");
                                table1forpage2.Cell(1, 2).SetContent("FA2 10%");
                                table1forpage2.Cell(1, 3).SetContent("SA1 30%");
                                table1forpage2.Cell(1, 4).SetContent("TOTAL 50%");

                                table1forpage2.Cell(1, 5).SetContent("FA3 10%");
                                table1forpage2.Cell(1, 6).SetContent("FA4 10%");
                                table1forpage2.Cell(1, 7).SetContent("SA2 30%");
                                table1forpage2.Cell(1, 8).SetContent("TOTAL 50%");

                                table1forpage2.Cell(1, 9).SetContent("FA 40%");
                                table1forpage2.Cell(1, 10).SetContent("SA 60%");
                                table1forpage2.Cell(1, 11).SetContent("Overall 100%");
                                table1forpage2.Cell(1, 12).SetContent("Grade Point");


                                foreach (PdfCell pr in table1forpage2.CellRange(0, 1, 0, 1).Cells)
                                {
                                    pr.ColSpan = 4;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(0, 5, 0, 5).Cells)
                                {
                                    pr.ColSpan = 4;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(0, 9, 0, 9).Cells)
                                {
                                    pr.ColSpan = 4;
                                }
                                //foreach (PdfCell pr in table1forpage2.CellRange(0, 0, 0, 0).Cells)
                                //{
                                //    pr.RowSpan = 2;
                                //}


                                for (int ii = 0; ii < columncountspread; ii++)
                                {
                                    table1forpage2.Cell(0, ii).SetFont(Fontboldhead);
                                    table1forpage2.Cell(1, ii).SetFont(Fontboldhead);
                                    table1forpage2.Cell(0, ii).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2.Cell(1, ii).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2.Cell(0, ii).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));
                                    table1forpage2.Cell(1, ii).SetColors(Color.Black, ColorTranslator.FromHtml("#fccd99"));
                                }

                                for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                                {
                                    for (int j = 0; j < columncountspread; j++)
                                    {
                                        string coldata = fpspread.Sheets[0].Cells[i, j].Text;

                                        table1forpage2.Cell(i + 2, j).SetContentAlignment(ContentAlignment.MiddleCenter);

                                        table1forpage2.Cell(i + 2, j).SetContent(coldata);


                                        //if (coldata == "Attendance")
                                        //{
                                        //    foreach (PdfCell pr in table1forpage2.CellRange(i + 3, 0, i + 3, 0).Cells)
                                        //    {
                                        //        pr.ColSpan = 2;
                                        //    }
                                        //    sk++; sk1++;
                                        //    table1forpage2.Cell(i + 3, j).SetContentAlignment(ContentAlignment.MiddleRight);

                                        //    table1forpage2.Cell(i + 3, j).SetContent(coldata);
                                        //}

                                        //else if (coldata.Contains("Nine Point"))
                                        //{
                                        //    foreach (PdfCell pr in table1forpage2.CellRange(i + 3, 0, i + 3, 0).Cells)
                                        //    {
                                        //        pr.ColSpan = columncountspread;

                                        //        sk++;
                                        //    }

                                        //    table1forpage2.Cell(i + 3, j).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        //    table1forpage2.Cell(i + 3, j).SetContent(coldata);
                                        //}
                                    }
                                }
                            }

                            double grandtotcreditfull = 0;
                            if (Convert.ToString(dropterm.SelectedItem.Text).Trim() == "1" && columncountspread == 5)
                            {
                                isCamCal = true;
                                // rowcountspread = rowcountspread - 1;
                                table1forpage2.Cell(rowcountspread - 2, 0).SetContentAlignment(ContentAlignment.MiddleRight);

                                table1forpage2.Cell(rowcountspread - 2, 0).SetContent("Attendance");
                                double perctot_work1 = 0;
                                if (lbltot_work1.Trim() != "0")
                                {
                                    perctot_work1 = Convert.ToDouble(lbltot_att1) / Convert.ToDouble(lbltot_work1);
                                }

                                perctot_work1 = perctot_work1 * 100;
                                string strformate = String.Format("{0:0.00}", perctot_work1);

                                if (dtallcol.Rows.Count > 0)
                                {
                                    for (int y = 0; y < dtallcol.Rows.Count; y++)
                                    {
                                        if (Convert.ToString(dtallcol.Rows[y][0]).ToLower() == "fa1" || Convert.ToString(dtallcol.Rows[y][0]).ToLower() == "f1")
                                        {
                                            table1forpage2.Cell(rowcountspread - 2, 1).SetContent(lbltot_att1 + "/" + lbltot_work1);
                                            table1forpage2.Cell(rowcountspread - 2, 2).SetContent(Convert.ToString(strformate) + "%");
                                        }
                                    }
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(rowcountspread - 2, 3, rowcountspread - 2, 3).Cells)
                                {
                                    pr.ColSpan = 2;
                                }

                                table1forpage2.Cell(rowcountspread - 1, 0).SetContent("Nine Point Grading Scale : A1 = 91% - 100%; A2 = 81% - 90%; B1 = 71% - 80%; B2 = 61% - 70%; C1 = 51% - 60%;                 C2 = 41%- 50%; D = 33% - 41%; E1 = 21% - 32%; E2 = 20% AND BELOW.");
                                table1forpage2.Cell(rowcountspread - 1, 0).SetFont(Fontboldhead);

                                table1forpage2.Cell(rowcountspread - 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                foreach (PdfCell pr in table1forpage2.CellRange(rowcountspread - 1, 0, rowcountspread - 1, 0).Cells)
                                {
                                    pr.ColSpan = columncountspread;
                                }
                            }

                            if (Convert.ToString(dropterm.SelectedItem.Text).Trim() == "2" && columncountspread == 13)
                            {

                                isCamCal = true;
                                table1forpage2.Cell(rowcountspread - 2, 0).SetContentAlignment(ContentAlignment.MiddleRight);

                                table1forpage2.Cell(rowcountspread - 2, 0).SetContent("Attendance");

                                double perctot_work1 = 0;
                                if (lbltot_work1.Trim() != "0")
                                {
                                    perctot_work1 = Convert.ToDouble(lbltot_att1) / Convert.ToDouble(lbltot_work1);
                                }

                                perctot_work1 = perctot_work1 * 100;
                                string strformate = String.Format("{0:0.00}", perctot_work1);

                                foreach (PdfCell pr in table1forpage2.CellRange(rowcountspread - 2, 1, rowcountspread - 2, 1).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                if (dtallcol.Rows.Count > 0)
                                {
                                    for (int y = 0; y < dtallcol.Rows.Count; y++)
                                    {
                                        if (Convert.ToString(dtallcol.Rows[y][0]).ToLower() == "fa1")
                                        {
                                            table1forpage2.Cell(rowcountspread - 2, 1).SetContent(lbltot_att1 + "/" + lbltot_work1);
                                            table1forpage2.Cell(rowcountspread - 2, 3).SetContent(Convert.ToString(strformate) + "%");
                                        }
                                    }
                                }

                                foreach (PdfCell pr in table1forpage2.CellRange(rowcountspread - 2, 3, rowcountspread - 2, 3).Cells)
                                {
                                    pr.ColSpan = 2;
                                }


                                //   table1forpage2.Cell(rowcountspread - 2, 5).SetContent(pre_present_date + "/" + per_workingdays);

                                if (per_workingdays != 0)
                                {
                                    perctot_work1 = Convert.ToDouble(pre_present_date) / Convert.ToDouble(per_workingdays);
                                }
                                else
                                {
                                    perctot_work1 = 0;
                                }
                                perctot_work1 = perctot_work1 * 100;
                                strformate = String.Format("{0:0.00}", perctot_work1);
                                if (dtallcol.Rows.Count > 0)
                                {
                                    for (int y = 0; y < dtallcol.Rows.Count; y++)
                                    {
                                        if (Convert.ToString(dtallcol.Rows[y][0]).ToLower() == "fa2")
                                        {
                                            table1forpage2.Cell(rowcountspread - 2, 5).SetContent(pre_present_date + "/" + per_workingdays);
                                            table1forpage2.Cell(rowcountspread - 2, 7).SetContent(Convert.ToString(strformate) + "%");
                                        }
                                    }
                                }

                                double finalatt = Convert.ToDouble(lbltot_att1) + Convert.ToDouble(pre_present_date);
                                double finalwholeatt = Convert.ToDouble(lbltot_work1) + Convert.ToDouble(per_workingdays);

                                if (finalwholeatt != 0)
                                {
                                    perctot_work1 = Convert.ToDouble(finalatt) / Convert.ToDouble(finalwholeatt);
                                }
                                else
                                {
                                    perctot_work1 = 0;
                                }

                                perctot_work1 = perctot_work1 * 100;
                                strformate = String.Format("{0:0.00}", perctot_work1);


                                foreach (PdfCell pr in table1forpage2.CellRange(rowcountspread - 2, 5, rowcountspread - 2, 5).Cells)
                                {
                                    pr.ColSpan = 2;
                                }

                                if (dtallcol.Rows.Count == 9)
                                {
                                    table1forpage2.Cell(rowcountspread - 2, 9).SetContent(Convert.ToString(finalatt + "/" + finalwholeatt));
                                    table1forpage2.Cell(rowcountspread - 2, 10).SetContent(Convert.ToString(Convert.ToString(strformate) + "%"));
                                }

                                table1forpage2.Cell(rowcountspread - 2, 11).SetContent(Convert.ToString("CGPA"));

                                if (dtallcol.Rows.Count == 9)
                                {
                                    cgpacalc = cgpacalc / twosubcount;
                                    strformate = String.Format("{0:0.00}", cgpacalc);

                                    table1forpage2.Cell(rowcountspread - 2, 12).SetContent(Convert.ToString(strformate));
                                }
                                else
                                {
                                    table1forpage2.Cell(rowcountspread - 2, 12).SetContent(Convert.ToString(""));
                                }

                                //shree
                                foreach (PdfCell pr in table1forpage2.CellRange(rowcountspread - 2, 7, rowcountspread - 2, 7).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                table1forpage2.Cell(rowcountspread - 1, 0).SetFont(Fontboldhead);
                                table1forpage2.Cell(rowcountspread - 1, 0).SetContent("Nine Point Grading Scale : A1 = 91% - 100%; A2 = 81% - 90%; B1 = 71% - 80%; B2 = 61% - 70%; C1 = 51% - 60%;                 C2 = 41%- 50%; D = 33% - 41%; E1 = 21% - 32%; E2 = 20% AND BELOW.");

                                //foreach (PdfCell pr in table1forpage2.CellRange(rowcountspread - 2, 0, rowcountspread - 2, 0).Cells)
                                //{
                                //    pr.ColSpan = 2;
                                //}
                                table1forpage2.Cell(rowcountspread - 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                foreach (PdfCell pr in table1forpage2.CellRange(rowcountspread - 1, 0, rowcountspread - 1, 0).Cells)
                                {
                                    pr.ColSpan = columncountspread;
                                }
                            }

                            if (columncountspread == 13 || columncountspread == 5)
                            {
                                table1forpage2.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage2.Columns[0].SetWidth(30);
                                //table1forpage2.Columns[0].SetWidth(25);
                                //table1forpage2.Columns[1].SetWidth(13);

                                //table1forpage2.Columns[fpspread.Sheets[0].ColumnCount - 1].SetWidth(10);
                                //table1forpage2.Columns[fpspread.Sheets[0].ColumnCount - 2].SetWidth(10);
                                //table1forpage2.Columns[2].SetWidth(70);

                                //foreach (PdfCell rr in table1forpage2.Cells)
                                //{
                                //    rr.SetCellPadding(8);
                                //}
                                addtabletopage = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 230, 553, 600));
                                mypdfpage.Add(addtabletopage);
                            }

                            Double getheigh = addtabletopage.Area.Height;
                            getheigh = Math.Round(getheigh, 2);

                            double page2col = getheigh + 240;



                            PdfTextArea pdf28 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, page2col, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Cumulative Grade Point Average (CGPA)");
                            mypdfpage.Add(pdf28);


                            string cgpapdf1 = Convert.ToString(0);

                            PdfTextArea pdf28a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 290, page2col, 595, 50), System.Drawing.ContentAlignment.TopLeft, "ccgp");
                            mypdfpage.Add(pdf28a);
                            page2col = page2col + 30;
                            PdfArea overallgradepa1 = new PdfArea(mydoc, 70, page2col, 220, 28);
                            PdfRectangle overallgradepa1pr3 = new PdfRectangle(mydoc, overallgradepa1, Color.Black);


                            sql = " select  ca.CoCurr_ID,ca.Title_Name,tv.TextCode, tv.TextVal,ca.SubTitle from activity_entry ae,CoCurr_Activitie ca,textvaltable tv where ae.CoCurr_ID=ca.CoCurr_ID and ae.Batch_Year=ca.Batch_Year and ae.Degree_Code=ca.Degree_Code and ae.Batch_Year='" + batchyear + "' and ae.Degree_Code='" + degreecode + "' and ae.term='" + Convert.ToString(dropterm.SelectedItem) + "' and tv.TextCode=ae.ActivityTextVal  and ae.ActivityTextVal in (select ActivityTextVal from CoCurrActivitie_Det where Roll_No='" + Roll_No + "' and Degree_Code='" + degreecode + "' and Batch_Year='" + batchyear + "' and ae.term='" + Convert.ToString(dropterm.SelectedItem) + "' and mark<>0)  order by SubTitle";

                            partsds.Clear();
                            partsds = d2.select_method_wo_parameter(sql, "Text");

                            if (partsds.Tables[0].Rows.Count > 0)
                            {
                                DataView partdv = new DataView();
                                arrcourrid.Clear();
                                for (int i = 0; i < partsds.Tables[0].Rows.Count; i++)
                                {
                                    string courrid = Convert.ToString(partsds.Tables[0].Rows[i]["CoCurr_ID"]);
                                    if (!arrcourrid.Contains(courrid))
                                    {
                                        partsds.Tables[0].DefaultView.RowFilter = "CoCurr_ID='" + courrid + "'";
                                        partdv = partsds.Tables[0].DefaultView;
                                        int partrowcount = 0;
                                        partrowcount = partdv.Count;
                                        sql = "select IsActivity,IsActDesc,IsGrade  from CoCurr_Activitie where CoCurr_ID='" + courrid + "' ";
                                        ds.Clear();
                                        ds = d2.select_method_wo_parameter(sql, "Text");
                                        int colcountpart = 0;
                                        string colheadername = string.Empty;
                                        for (int dd = 0; dd < ds.Tables[0].Rows.Count; dd++)
                                        {
                                            if (Convert.ToString(ds.Tables[0].Rows[0][0]) == "True")
                                            {
                                                colcountpart++;
                                                partcolumnnames.Add("Activity");
                                                if (colheadername == "")
                                                {
                                                    colheadername = "Activity";
                                                }
                                            }
                                            if (Convert.ToString(ds.Tables[0].Rows[0][1]) == "True")
                                            {
                                                colcountpart++;
                                                partcolumnnames.Add("Description");
                                                if (colheadername == "")
                                                {
                                                    colheadername = "Description";
                                                }
                                                else
                                                {
                                                    colheadername = colheadername + ";" + "Description";
                                                }
                                            }
                                            if (Convert.ToString(ds.Tables[0].Rows[0][2]) == "True")
                                            {
                                                colcountpart++;
                                                partcolumnnames.Add("Grade");
                                                if (colheadername == "")
                                                {
                                                    colheadername = "Grade";
                                                }
                                                else
                                                {
                                                    colheadername = colheadername + ";" + "Grade";
                                                }
                                            }
                                        }

                                        Gios.Pdf.PdfTable tableparts = mydoc.NewTable(Fontsmall1, partrowcount + 1, colcountpart, 10);
                                        Gios.Pdf.PdfTable tablepartsduplicate = mydoc.NewTable(Fontsmall1, partrowcount + 1, colcountpart, 10);
                                        tableparts.VisibleHeaders = false;
                                        tablepartsduplicate.VisibleHeaders = false;
                                        tablepartsduplicate.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        tableparts.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        string[] splitcolheadername = colheadername.Split(';');
                                        if (splitcolheadername.GetUpperBound(0) > 0)
                                        {
                                            for (int jf = 0; jf <= splitcolheadername.GetUpperBound(0); jf++)
                                            {
                                                tableparts.Cell(0, jf).SetContent(splitcolheadername[jf]);
                                                tableparts.Cell(0, jf).SetFont(Fontsmall1bold);
                                                tableparts.Cell(0, jf).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                tablepartsduplicate.Cell(0, jf).SetContent(splitcolheadername[jf]);
                                                tablepartsduplicate.Cell(0, jf).SetFont(Fontsmall1bold);
                                                tablepartsduplicate.Cell(0, jf).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            }

                                            for (int j = 0; j < partdv.Count; j++)
                                            {
                                                parttitle1a = da.GetFunction(" select textval from textvaltable where TextCode= '" + Convert.ToString(partdv[0]["Title_Name"]) + "'");
                                                for (int partcolumn = 0; partcolumn < partcolumnnames.Count; partcolumn++)
                                                {
                                                    string sqlff = string.Empty;
                                                    if (Convert.ToString(splitcolheadername[partcolumn]).ToLower() == "activity")
                                                    {
                                                        sqlff = " tv.TextVal as Activity";
                                                    }
                                                    else if (Convert.ToString(splitcolheadername[partcolumn]).ToLower() == "grade")
                                                    {
                                                        sqlff = " ag.Grade";
                                                    }
                                                    else
                                                    {
                                                        sqlff = "ag.description";
                                                    }
                                                    sqlff = da.GetFunction("select " + sqlff + " from activity_gd ag ,CoCurrActivitie_Det cd,textvaltable tv where ag.ActivityTextVal=cd.ActivityTextVal and tv.TextCode=ag.ActivityTextVal and tv.TextCode=cd.ActivityTextVal and  cd.ActivityTextVal='" + Convert.ToString(partdv[j]["Textcode"]) + "'  and cd.Roll_No='" + Roll_No + "' and mark between frompoint and topoint ");
                                                    tableparts.Cell(j + 1, partcolumn).SetContent(sqlff);
                                                    tableparts.Cell(j + 1, partcolumn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    if (Convert.ToString(splitcolheadername[partcolumn]) == "Grade")
                                                    {
                                                        tableparts.Columns[partcolumn].SetWidth(7);
                                                        tableparts.Cell(j + 1, partcolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    }

                                                    if (Convert.ToString(splitcolheadername[partcolumn]) == "Activity")
                                                    {
                                                        tableparts.Columns[partcolumn].SetWidth(15);
                                                    }
                                                    tablepartsduplicate.Cell(j + 1, partcolumn).SetContent(sqlff);
                                                }
                                            }
                                            page2col = page2col + 20;
                                            addtabletopage = tablepartsduplicate.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, page2col, 553, 600));
                                            getheigh = addtabletopage.Area.Height;
                                            getheigh = Math.Round(getheigh, 2);

                                            double dummycolval = page2col + getheigh + 20;
                                            if (842 > dummycolval)
                                            {

                                            }
                                            else
                                            {
                                                page2col = page2col + 2;
                                            }

                                            if (842 > dummycolval && flag == true)
                                            {
                                                parttitiles = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 22, page2col, 595, 50), System.Drawing.ContentAlignment.TopLeft, parttitle1a);
                                                mypdfpage.Add(parttitiles);
                                                page2col = page2col + 15;
                                                addtabletopage = tableparts.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, page2col, 553, 600));
                                                mypdfpage.Add(addtabletopage);
                                                page2col = page2col + getheigh;
                                            }
                                            else if (842 > dummycolval)
                                            {
                                                parttitiles = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 22, page2col, 595, 50), System.Drawing.ContentAlignment.TopLeft, parttitle1a);
                                                mypdfpage.Add(parttitiles);
                                                page2col = page2col + 15;
                                                Gios.Pdf.PdfTablePage addtabletopagenew = tableparts.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, page2col, 553, 600));
                                                mypdfpage.Add(addtabletopagenew);
                                                page2col = page2col + getheigh;

                                            }
                                            else
                                            {
                                                flag = false;
                                                mypdfpage.SaveToDocument();
                                                mypdfpage = mydoc.NewPage();
                                                mypdfpage.Add(border);
                                                page2col = 40;
                                                parttitiles = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 22, page2col, 595, 50), System.Drawing.ContentAlignment.TopLeft, parttitle1a);
                                                mypdfpage.Add(parttitiles);
                                                page2col = page2col + 15;
                                                Gios.Pdf.PdfTablePage addtabletopagenew = tableparts.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, page2col, 553, 600));
                                                mypdfpage.Add(addtabletopagenew);
                                                page2col = page2col + getheigh;

                                            }
                                        }
                                        partcolumnnames.Clear();
                                        arrcourrid.Add(courrid);
                                    }
                                }
                            }
                            if (isCamCal)
                                mypdfpage.SaveToDocument();
                            else
                            {
                                if (errormsg == "")
                                {
                                    errormsg = "Please Check Test Mark Entry or CAM Calculation Process For " + Roll_No + " !!!";
                                }
                                else
                                {
                                    errormsg += ",\nPlease Check Test Mark Entry or CAM Calculation Process For " + Roll_No + " !!!";
                                }
                            }
                        }
                    }

                    if (errormsg != "")
                    {
                        lblerrormsg.Text = errormsg;
                        lblerrormsg.Visible = true;
                    }

                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "" && isCamCal)
                    {
                        string szPath = appPath + "/Report/";
                        string szFile = "grade" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                        Response.Buffer = true;
                        Response.Clear();
                        mydoc.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                    }
                    else
                    {
                        if (errormsg != "")
                        {
                            lblerrormsg.Text = errormsg;
                            lblerrormsg.Visible = true;
                        }
                    }
                }
                else
                {
                    lblerrormsg.Text = "No Records Found";
                    lblerrormsg.Visible = true;
                }
            }
            else
            {
                lblerrormsg.Text = "Please Select Any One Record";
                lblerrormsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void bindbutn(string rollno)
    {
        try
        {
            lblerrormsg.Visible = false;
            lblerrormsg.Text = string.Empty;

            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;
            Gios.Pdf.PdfPage mypdfpage1;
            Gios.Pdf.PdfPage mypdfpage2;
            Gios.Pdf.PdfPage mypdfpage6;
            Gios.Pdf.PdfPage mypdfpagefinal;
            Gios.Pdf.PdfPage mypdfpage5;
            rollnos = rollno;
            if (rollnos != "")
            {
                sql = "select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile,parentF_Mobile,bldgrp from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No in ('" + rollnos + "') ;";
                studgradeds.Clear();
                studgradeds = d2.select_method_wo_parameter(sql, "text");
                if (studgradeds.Tables[0].Rows.Count > 0)
                {

                    for (int roll = 0; roll < studgradeds.Tables[0].Rows.Count; roll++)
                    {
                        string rcrollno = string.Empty;
                        rcrollno = Convert.ToString(studgradeds.Tables[0].Rows[roll][1]);
                        bindstudentmark(rcrollno);
                        //bindbutn(rcrollno);
                        // bindrptcard(rcrollno);

                        DataSet ds = new DataSet();
                        DataSet dschool = new DataSet();
                        DAccess2 da = new DAccess2();
                        DataSet dset = new DataSet();
                        string college_code = collegecode;
                        string stdappno = string.Empty;
                        System.Drawing.Font Fontboldhead = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                        System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
                        System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                        System.Drawing.Font Fontbold1 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
                        System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
                        System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                        System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
                        System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
                        System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
                        System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
                        System.Drawing.Font Fontmediumv = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
                        System.Drawing.Font Fontmedium1V = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);

                        System.Drawing.Font f1 = new System.Drawing.Font("Book Antiqua", 7, FontStyle.Regular);
                        System.Drawing.Font f2 = new System.Drawing.Font("Book Antiqua", 8, FontStyle.Regular);
                        System.Drawing.Font f3 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
                        System.Drawing.Font f4 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
                        System.Drawing.Font f5 = new System.Drawing.Font("Book Antiqua", 11, FontStyle.Regular);
                        System.Drawing.Font f6 = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);

                        System.Drawing.Font f7 = new System.Drawing.Font("Book Antiqua", 7, FontStyle.Bold);
                        System.Drawing.Font f8 = new System.Drawing.Font("Book Antiqua", 8, FontStyle.Bold);
                        System.Drawing.Font f9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold);
                        System.Drawing.Font f10 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                        System.Drawing.Font f11 = new System.Drawing.Font("Book Antiqua", 11, FontStyle.Bold);
                        System.Drawing.Font f12 = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);

                        string Roll_No = rcrollno;
                        sql = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile,parentF_Mobile,StuPer_Id,parent_addressc,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,bldgrp,studhouse from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No='" + Roll_No + "'";
                        ds.Clear();
                        ds.Dispose();
                        ds = da.select_method_wo_parameter(sql, "Text");
                        DataView dv = new DataView();
                        ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                        dv = ds.Tables[1].DefaultView;
                        int count4 = 0;
                        count4 = dv.Count;

                        if (ds.Tables[0].Rows.Count > 0 && count4 > 0)
                        {
                            string currentsem = Convert.ToString(dv[0]["Current_Semester"]);
                            string batchyear = Convert.ToString(dv[0]["Batch_Year"]);
                            string degreecode = Convert.ToString(dv[0]["degree_code"]);
                            stdappno = Convert.ToString(dv[0]["App_No"]);
                            string allsem = "1";
                            string admdate = Convert.ToString(dv[0]["adm_date"]);

                            string stdcc = string.Empty;
                            stdcc = Convert.ToString(ds.Tables[1].Rows[0]["Dept_Name"]);
                            string lblclassq = "CLASS - IX & X Academic Year :";

                            if (Convert.ToInt32(currentsem) > 1)
                            {
                                int term = Convert.ToInt32(currentsem);
                                for (int i = 2; i <= term; i++)
                                {
                                    allsem = allsem + "'" + "," + "'" + i;
                                }
                            }

                            if (Convert.ToInt32(currentsem) >= 1)
                            {
                                int term = Convert.ToInt32(currentsem);
                                for (int i = 1; i <= term; i++)
                                {
                                    string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + i + "' and degree_code='" + degreecode + "' and batch_year='" + batchyear + "'";
                                    dset = da.select_method_wo_parameter(sem, "Text");

                                    if (dset.Tables[0].Rows.Count > 0)
                                    {
                                        string startdate = Convert.ToString(dset.Tables[0].Rows[0]["start_date"]);
                                        string enddate = Convert.ToString(dset.Tables[0].Rows[0]["end_date"]);
                                        persentmonthcal(Roll_No, admdate, startdate, enddate);
                                    }
                                    if (i == 1)
                                    {

                                        lbltot_att1 = Convert.ToString(pre_present_date);
                                        lbltot_work1 = Convert.ToString(per_workingdays);
                                        working1 = Convert.ToString(pre_present_date);
                                        present1 = Convert.ToString(per_workingdays);
                                    }
                                }
                            }

                            string table1sql = "select * from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'  and semester = '" + currentsem + "' ; SELECT COUNT(*) as facount FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "'); SELECT Istype,CRITERIA_NO,y.semester FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "') ; SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "')";
                            DataSet ds1fortable1 = new DataSet();
                            ds1fortable1.Clear();
                            ds1fortable1.Dispose();
                            ds1fortable1 = da.select_method_wo_parameter(table1sql, "Text");
                            DataView dvforpage2 = new DataView();

                            string dob = Convert.ToString(dv[0]["dob"]);
                            string[] dobspit = dob.Split('/');
                            string addressline1 = Convert.ToString(dv[0]["parent_addressP"]);
                            addressline1 = addressline1 + " " + Convert.ToString(dv[0]["Streetp"]);
                            string addressline2 = Convert.ToString(dv[0]["Cityp"]);
                            string mobileno = Convert.ToString(dv[0]["parentF_Mobile"]);
                            addressline2 = addressline1 + ", " + addressline2 + " - " + Convert.ToString(dv[0]["parent_pincodep"]);


                            mypdfpage = mydoc.NewPage();
                            mypdfpage1 = mydoc.NewPage();
                            mypdfpage2 = mydoc.NewPage();
                            mypdfpage6 = mydoc.NewPage();

                            PdfTextArea pdf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(ds.Tables[0].Rows[0][0]) + "");
                            PdfTextArea pdf11 = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(ds.Tables[0].Rows[0][1]) + "");
                            string address = Convert.ToString(ds.Tables[0].Rows[0][2]) + "" + Convert.ToString(ds.Tables[0].Rows[0][3]) + "" + Convert.ToString(ds.Tables[0].Rows[0][4]);
                            PdfTextArea pdf12 = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 55, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
                            PdfTextArea pdf172 = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 70, 595, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  :" + Convert.ToString(ds.Tables[0].Rows[0][6]));

                            PdfArea pa1 = new PdfArea(mydoc, 2, 2, 591, 838);

                            PdfArea pahealth = new PdfArea(mydoc, 2, 765, 591, 75);
                            PdfRectangle pr1 = new PdfRectangle(mydoc, pa1, Color.Black);
                            PdfArea pa2 = new PdfArea(mydoc, 189, 175, 224, 40);
                            //PdfRectangle pr2 = new PdfRectangle(mydoc, pa2, Color.Black);


                            string sqlschool = "select value from Master_Settings where settings='Academic year'";
                            dschool = da.select_method_wo_parameter(sqlschool, "Text");
                            string splitvalue = Convert.ToString(dschool.Tables[0].Rows[0]["value"]);
                            string[] dsplit = splitvalue.Split(',');

                            string fvalue = Convert.ToString(dsplit[0]);
                            string lvalue = Convert.ToString(dsplit[1]);
                            string acdmic_date = fvalue + "-" + lvalue;


                            PdfTextArea pdf13 = new PdfTextArea(f12, System.Drawing.Color.Black, new PdfArea(mydoc, 190, 90, 304, 30), System.Drawing.ContentAlignment.TopLeft, "Record of Academic Performance");
                            PdfTextArea pdf14 = new PdfTextArea(f12, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 90, 595, 50), System.Drawing.ContentAlignment.MiddleCenter, acdmic_date);


                            PdfTextArea pdf116 = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 130, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Registration No." + "        " + Convert.ToString(dv[0]["Reg_No"]));
                            PdfTextArea pdf118b1 = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 390, 150, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Roll No." + "                           " + Convert.ToString(dv[0]["Roll_No"]));
                            mypdfpage.Add(pdf116);
                            mypdfpage.Add(pdf118b1);

                            PdfTextArea pdf18 = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 150, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Name of Student");

                            PdfTextArea pdf110a = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 120, 150, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["stud_name"]) + "");

                            PdfTextArea pdf111 = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 390, 130, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Admission No.");
                            PdfTextArea pdf113a = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 490, 130, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["roll_admit"]) + "");
                            mypdfpage.Add(pdf110a);
                            mypdfpage.Add(pdf111);
                            mypdfpage.Add(pdf113a);
                            mypdfpage.Add(pdf172);


                            PdfTextArea pdf125 = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 170, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Father's Name");
                            PdfTextArea pdf127a = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 120, 170, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["parent_name"]) + "");
                            mypdfpage.Add(pdf125);
                            mypdfpage.Add(pdf127a);
                            PdfTextArea pdf119zzzzz = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 390, 170, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Class" + "                               " + Convert.ToString(ddstandard.SelectedItem.Text) + " ");
                            mypdfpage.Add(pdf119zzzzz);

                            PdfTextArea pdf119 = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 390, 190, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Birth" + "                    " + Convert.ToString(dv[0]["dob"]));
                            mypdfpage.Add(pdf119);

                            PdfTextArea pdf122 = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 190, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Mother's Name");
                            PdfTextArea pdf124a = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 120, 190, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["mother"]) + "");
                            mypdfpage.Add(pdf122);
                            mypdfpage.Add(pdf124a);


                            PdfTextArea pdf128 = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 210, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Address");
                            PdfTextArea pdf130a = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 120, 210, 400, 50), System.Drawing.ContentAlignment.TopLeft, "" + addressline2 + "");
                            mypdfpage.Add(pdf128);
                            mypdfpage.Add(pdf130a);

                            PdfTextArea pdf147z;
                            PdfTextArea pdf147zq;
                            if (dropterm.SelectedItem.Text == "1")
                            {
                                pdf147z = new PdfTextArea(f5, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 580, 595, 50), System.Drawing.ContentAlignment.TopLeft, " ");
                                mypdfpage.Add(pdf147z);

                                pdf147zq = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 600, 595, 50), System.Drawing.ContentAlignment.TopLeft, " ");//Absent in one subject of SA1, Should take exams seriously
                                mypdfpage.Add(pdf147zq);
                            }
                            else
                            {
                                pdf147z = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 10, 700, 595, 50), System.Drawing.ContentAlignment.TopLeft, "RESULT : ");
                                mypdfpage.Add(pdf147z);

                                //pdf147zq = new PdfTextArea(Fontsmall9, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 710, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Absent in one subject of SA1, Should take exams seriously");
                                //mypdfpage.Add(pdf147zq);
                            }

                            PdfTextArea pdf146 = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 30, 780, 595, 50), System.Drawing.ContentAlignment.TopLeft, "CLASS TEACHER'S SIGNATURE");
                            mypdfpage.Add(pdf146);
                            pdf146 = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 250, 780, 595, 50), System.Drawing.ContentAlignment.TopLeft, "PARENT'S SIGNATURE");
                            mypdfpage.Add(pdf146);
                            PdfTextArea pdf147 = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 780, 595, 50), System.Drawing.ContentAlignment.TopLeft, "PRINCIPAL'S SIGNATURE & SEAL");
                            mypdfpage.Add(pdf147);

                            //PdfTextArea pdf147UI = new PdfTextArea(Fontmedium1V, System.Drawing.Color.Black, new PdfArea(mydoc, 15, 780, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_____________________________________________________________________________________________________");
                            //mypdfpage.Add(pdf147UI);

                            Gios.Pdf.PdfTable table1forpage3v1a;
                            table1forpage3v1a = mydoc.NewTable(Fontsmall1, 1, 2, 1);

                            table1forpage3v1a.VisibleHeaders = false;
                            table1forpage3v1a.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                            table1forpage3v1a.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage3v1a.Rows[0].SetCellPadding(10);
                            table1forpage3v1a.Cell(0, 0).SetContent("Note: (1) Promotion is based on the day-to-day continuous assessment throughout the year.");

                            Gios.Pdf.PdfTablePage newpdftabpage3av2a = table1forpage3v1a.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 2, 800, 591, 50));
                            mypdfpage.Add(newpdftabpage3av2a);

                            //PdfTextArea pdf14712 = new PdfTextArea(Fontmedium1V, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 805, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Note: (1)");
                            //mypdfpage.Add(pdf14712);

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 20, 45, 450);
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                //Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                //mypdfpage.Add(LogoImage1, 280, 96, 450);
                            }

                            string stdphtsql = "select * from StdPhoto where app_no='" + stdappno + "'";
                            MemoryStream memoryStream = new MemoryStream();
                            DataSet dsstdpho = new DataSet();
                            dsstdpho.Clear();
                            dsstdpho.Dispose();
                            dsstdpho = d2.select_method_wo_parameter(stdphtsql, "Text");
                            if (dsstdpho.Tables[0].Rows.Count > 0)
                            {
                                byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                                    {
                                        //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                    }
                                    else
                                    {
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                    }



                                }

                            }

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"));
                                mypdfpage.Add(LogoImage2, 460, 45, 450);
                            }
                            else
                            {
                                Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                mypdfpage.Add(LogoImage2, 450, 35, 270);
                            }

                            Hashtable hatsubject = new Hashtable();
                            Hashtable hatcriter = new Hashtable();


                            //////////////////////////////////////////////////////////////////page 2/////////////////////
                            DataTable term1dt = new DataTable();
                            DataTable term2dt = new DataTable();
                            if (dropterm.SelectedItem.Text == "1")
                            {
                                term1dt.Clear();
                                term1dt.Columns.Add("Subject");
                                term1dt.Columns.Add("FA1");
                                term1dt.Columns.Add("FA2");
                                term1dt.Columns.Add("SA1");
                                term1dt.Columns.Add("Total");

                                for (int i = 0; i < 2; i++)
                                {
                                    term1dt.Rows.Add("", "", "", "");
                                }
                            }

                            int rowcountspread = fpspread.Sheets[0].RowCount + 4;
                            int columncountspread = fpspread.Sheets[0].ColumnCount;

                            Gios.Pdf.PdfTable table1forpage2;
                            if (dropterm.SelectedItem.Text == "1")
                            {
                                table1forpage2 = mydoc.NewTable(Fontsmall1, rowcountspread, columncountspread, 6);
                            }
                            else
                            {
                                table1forpage2 = mydoc.NewTable(Fontsmall1, rowcountspread, columncountspread, 6);
                            }
                            //Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(Fontsmall1, rowcountspread, columncountspread, 1);
                            table1forpage2.VisibleHeaders = false;
                            table1forpage2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            //table1forpage2.Columns[1].SetWidth(25);
                            // table1forpage2.Columns[0].SetWidth(25);

                            int ss = fpspread.Sheets[0].ColumnHeader.RowCount;


                            int sk = 1, sk1 = 1;


                            if (dropterm.SelectedItem.Text == "1")
                            {

                                //table1forpage2.Cell(0, 0).SetContent("S.No");
                                table1forpage2.Cell(0, 0).SetContent("SCHOLASTIC AREA");
                                table1forpage2.Cell(0, 1).SetContent("TERM-I");
                                table1forpage2.Cell(1, 0).SetContent("Subject");
                                table1forpage2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage2.Cell(1, 1).SetContent("Formative Assessment-1");
                                table1forpage2.Cell(1, 2).SetContent("Formative Assessment-2");
                                table1forpage2.Cell(1, 3).SetContent("Summative Assessment-1");
                                table1forpage2.Cell(1, 4).SetContent("TOTAL  (FA1+FA2+SA1)");

                                //table1forpage2.Columns[1].SetWidth(20);
                                //table1forpage2.Columns[2].SetWidth(20);
                                //table1forpage2.Columns[3].SetWidth(20);
                                //table1forpage2.Columns[4].SetWidth(20);
                                //shree
                                foreach (PdfCell pr in table1forpage2.CellRange(0, 1, 0, 1).Cells)
                                {
                                    pr.ColSpan = 4;
                                }


                                for (int ii = 0; ii < 5; ii++)
                                {
                                    table1forpage2.Cell(0, ii).SetFont(Fontboldhead);
                                    table1forpage2.Cell(1, ii).SetFont(Fontboldhead);
                                    table1forpage2.Cell(0, ii).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2.Cell(1, ii).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2.Cell(0, ii).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));
                                    table1forpage2.Cell(1, ii).SetColors(Color.Black, ColorTranslator.FromHtml("#fccd99"));
                                }

                                for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                                {

                                    string coldata = fpspread.Sheets[0].Cells[i, 0].Text;
                                    table1forpage2.Cell(i + 2, 0).SetContent(coldata);

                                    table1forpage2.Cell(i + 2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);

                                }

                                for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                                {
                                    for (int j = 0; j < columncountspread; j++)
                                    {
                                        string coldata = fpspread.Sheets[0].Cells[i, j].Text;
                                        table1forpage2.Cell(i + 2, j).SetContent(coldata);

                                        //for (int h = 0; h < headerrights.Rows.Count; h++)
                                        //{
                                        //    if (Convert.ToString(headerrights.Rows[h][0]).ToLower() == "fa1")
                                        //    {
                                        //        coldata = fpspread.Sheets[0].Cells[i, 1].Text;
                                        //        table1forpage2.Cell(i + 2, 1).SetContent(coldata);
                                        //    }
                                        //    if (Convert.ToString(headerrights.Rows[h][0]).ToLower() == "fa2")
                                        //    {
                                        //        coldata = fpspread.Sheets[0].Cells[i, 2].Text;
                                        //        table1forpage2.Cell(i + 2, 2).SetContent(coldata);
                                        //    }
                                        //    if (Convert.ToString(headerrights.Rows[h][0]).ToLower() == "sa1")
                                        //    {
                                        //        coldata = fpspread.Sheets[0].Cells[i, 3].Text;
                                        //        table1forpage2.Cell(i + 2, 3).SetContent(coldata);
                                        //    }
                                        //}

                                        table1forpage2.Cell(i + 2, j).SetContentAlignment(ContentAlignment.MiddleCenter);

                                    }
                                }

                            }
                            else
                            {
                                //table1forpage2.Cell(0, 0).SetContent("S.No");
                                table1forpage2.Cell(0, 0).SetContent("         SCHOLASTIC AREA      (9 Point Scale)");
                                table1forpage2.Cell(0, 1).SetContent("TERM-I");
                                table1forpage2.Cell(0, 5).SetContent("TERM-II");
                                table1forpage2.Cell(0, 9).SetContent("FINAL ASSESSMENT");
                                table1forpage2.Cell(1, 0).SetContent("Subject");
                                table1forpage2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage2.Cell(1, 1).SetContent("FA1 10%");
                                table1forpage2.Cell(1, 2).SetContent("FA2 10%");
                                table1forpage2.Cell(1, 3).SetContent("SA1 30%");
                                table1forpage2.Cell(1, 4).SetContent("TOTAL 50%");

                                table1forpage2.Cell(1, 5).SetContent("FA3 10%");
                                table1forpage2.Cell(1, 6).SetContent("FA4 10%");
                                table1forpage2.Cell(1, 7).SetContent("SA2 30%");
                                table1forpage2.Cell(1, 8).SetContent("TOTAL 50%");

                                table1forpage2.Cell(1, 9).SetContent("FA 40%");
                                table1forpage2.Cell(1, 10).SetContent("SA 60%");
                                table1forpage2.Cell(1, 11).SetContent("Overall 100%");
                                table1forpage2.Cell(1, 12).SetContent("Grade Point");


                                foreach (PdfCell pr in table1forpage2.CellRange(0, 1, 0, 1).Cells)
                                {
                                    pr.ColSpan = 4;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(0, 5, 0, 5).Cells)
                                {
                                    pr.ColSpan = 4;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(0, 9, 0, 9).Cells)
                                {
                                    pr.ColSpan = 4;
                                }
                                //foreach (PdfCell pr in table1forpage2.CellRange(0, 0, 0, 0).Cells)
                                //{
                                //    pr.RowSpan = 2;
                                //}


                                for (int ii = 0; ii < columncountspread; ii++)
                                {
                                    table1forpage2.Cell(0, ii).SetFont(Fontboldhead);
                                    table1forpage2.Cell(1, ii).SetFont(Fontboldhead);
                                    table1forpage2.Cell(0, ii).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2.Cell(1, ii).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2.Cell(0, ii).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));
                                    table1forpage2.Cell(1, ii).SetColors(Color.Black, ColorTranslator.FromHtml("#fccd99"));
                                }



                                for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                                {
                                    string coldata = fpspread.Sheets[0].Cells[i, 0].Text;
                                    table1forpage2.Cell(i + 2, 0).SetContent(coldata);
                                    table1forpage2.Cell(i + 2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                }

                                for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                                {
                                    for (int j = 0; j < columncountspread; j++)
                                    {
                                        string coldata = fpspread.Sheets[0].Cells[i, j].Text;
                                        table1forpage2.Cell(i + 2, j).SetContent(coldata);
                                        table1forpage2.Cell(i + 2, j).SetContentAlignment(ContentAlignment.MiddleCenter);

                                    }
                                }
                            }

                            double grandtotcreditfull = 0;



                            if (Convert.ToString(dropterm.SelectedItem.Text).Trim() == "1")
                            {
                                // rowcountspread = rowcountspread - 1;
                                table1forpage2.Cell(rowcountspread - 2, 0).SetContentAlignment(ContentAlignment.MiddleRight);

                                table1forpage2.Cell(rowcountspread - 2, 0).SetContent("Attendance");
                                double perctot_work1 = 0;
                                if (lbltot_work1.Trim() != "0")
                                {
                                    perctot_work1 = Convert.ToDouble(lbltot_att1) / Convert.ToDouble(lbltot_work1);
                                }

                                perctot_work1 = perctot_work1 * 100;
                                string strformate = String.Format("{0:0.00}", perctot_work1);

                                if (dtallcol.Rows.Count > 0)
                                {
                                    for (int y = 0; y < dtallcol.Rows.Count; y++)
                                    {
                                        if (Convert.ToString(dtallcol.Rows[y][0]).ToLower() == "fa1")
                                        {
                                            //table1forpage2.Cell(rowcountspread - 2, 1).SetContent(lbltot_att1 + "/" + lbltot_work1);
                                            //table1forpage2.Cell(rowcountspread - 2, 2).SetContent(Convert.ToString(strformate) + "%");

                                        }
                                    }
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(rowcountspread - 2, 3, rowcountspread - 2, 3).Cells)
                                {
                                    pr.ColSpan = 2;
                                }

                                table1forpage2.Cell(rowcountspread - 1, 0).SetContent("Nine Point Grading Scale : A1 = 91% - 100%; A2 = 81% - 90%; B1 = 71% - 80%; B2 = 61% - 70%; C1 = 51% - 60%;                 C2 = 41%- 50%; D = 33% - 41%; E1 = 21% - 32%; E2 = 20% AND BELOW.");
                                table1forpage2.Cell(rowcountspread - 1, 0).SetFont(Fontboldhead);



                                table1forpage2.Cell(rowcountspread - 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);


                                foreach (PdfCell pr in table1forpage2.CellRange(rowcountspread - 1, 0, rowcountspread - 1, 0).Cells)
                                {
                                    pr.ColSpan = columncountspread;
                                }
                            }

                            if (Convert.ToString(dropterm.SelectedItem.Text).Trim() == "2")
                            {

                                table1forpage2.Cell(rowcountspread - 2, 0).SetContentAlignment(ContentAlignment.MiddleRight);

                                table1forpage2.Cell(rowcountspread - 2, 0).SetContent("Attendance");

                                double perctot_work1 = 0;
                                if (lbltot_work1.Trim() != "0")
                                {
                                    perctot_work1 = Convert.ToDouble(lbltot_att1) / Convert.ToDouble(lbltot_work1);
                                }

                                perctot_work1 = perctot_work1 * 100;
                                string strformate = String.Format("{0:0.00}", perctot_work1);

                                foreach (PdfCell pr in table1forpage2.CellRange(rowcountspread - 2, 1, rowcountspread - 2, 1).Cells)
                                {
                                    pr.ColSpan = 2;
                                }



                                if (dtallcol.Rows.Count > 0)
                                {
                                    for (int y = 0; y < dtallcol.Rows.Count; y++)
                                    {
                                        if (Convert.ToString(dtallcol.Rows[y][0]).ToLower() == "fa1")
                                        {
                                            //table1forpage2.Cell(rowcountspread - 2, 1).SetContent(lbltot_att1 + "/" + lbltot_work1);
                                            //table1forpage2.Cell(rowcountspread - 2, 3).SetContent(Convert.ToString(strformate) + "%");

                                        }
                                    }
                                }

                                foreach (PdfCell pr in table1forpage2.CellRange(rowcountspread - 2, 3, rowcountspread - 2, 3).Cells)
                                {
                                    pr.ColSpan = 2;
                                }


                                //   table1forpage2.Cell(rowcountspread - 2, 5).SetContent(pre_present_date + "/" + per_workingdays);

                                if (per_workingdays != 0)
                                {
                                    perctot_work1 = Convert.ToDouble(pre_present_date) / Convert.ToDouble(per_workingdays);
                                }
                                else
                                {
                                    perctot_work1 = 0;
                                }
                                perctot_work1 = perctot_work1 * 100;
                                strformate = String.Format("{0:0.00}", perctot_work1);
                                if (dtallcol.Rows.Count > 0)
                                {
                                    for (int y = 0; y < dtallcol.Rows.Count; y++)
                                    {
                                        if (Convert.ToString(dtallcol.Rows[y][0]).ToLower() == "fa2")
                                        {
                                            //table1forpage2.Cell(rowcountspread - 2, 5).SetContent(pre_present_date + "/" + per_workingdays);
                                            //table1forpage2.Cell(rowcountspread - 2, 7).SetContent(Convert.ToString(strformate) + "%");

                                        }
                                    }
                                }

                                double finalatt = Convert.ToDouble(lbltot_att1) + Convert.ToDouble(pre_present_date);
                                double finalwholeatt = Convert.ToDouble(lbltot_work1) + Convert.ToDouble(per_workingdays);

                                if (finalwholeatt != 0)
                                {
                                    perctot_work1 = Convert.ToDouble(finalatt) / Convert.ToDouble(finalwholeatt);
                                }
                                else
                                {
                                    perctot_work1 = 0;
                                }

                                perctot_work1 = perctot_work1 * 100;
                                strformate = String.Format("{0:0.00}", perctot_work1);


                                foreach (PdfCell pr in table1forpage2.CellRange(rowcountspread - 2, 5, rowcountspread - 2, 5).Cells)
                                {
                                    pr.ColSpan = 2;
                                }

                                if (twosubcount > 0 && dropterm.SelectedItem.Text == "2")
                                {
                                    //table1forpage2.Cell(rowcountspread - 2, 9).SetContent(Convert.ToString(finalatt + "/" + finalwholeatt));
                                    //table1forpage2.Cell(rowcountspread - 2, 10).SetContent(Convert.ToString(Convert.ToString(strformate) + "%"));
                                }

                                table1forpage2.Cell(rowcountspread - 2, 11).SetContent(Convert.ToString("CGPA"));

                                if (twosubcount > 0 && dropterm.SelectedItem.Text == "2")
                                {
                                    cgpacalc = cgpacalc / twosubcount;
                                    strformate = String.Format("{0:0.00}", cgpacalc);

                                    table1forpage2.Cell(rowcountspread - 2, 12).SetContent(Convert.ToString(strformate));

                                }
                                else
                                {
                                    table1forpage2.Cell(rowcountspread - 2, 12).SetContent(Convert.ToString(""));

                                }

                                //shree
                                foreach (PdfCell pr in table1forpage2.CellRange(rowcountspread - 2, 7, rowcountspread - 2, 7).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                table1forpage2.Cell(rowcountspread - 1, 0).SetFont(Fontboldhead);
                                table1forpage2.Cell(rowcountspread - 1, 0).SetContent("Nine Point Grading Scale : A1 = 91% - 100%; A2 = 81% - 90%; B1 = 71% - 80%; B2 = 61% - 70%; C1 = 51% - 60%;                 C2 = 41%- 50%; D = 33% - 41%; E1 = 21% - 32%; E2 = 20% AND BELOW.");

                                //foreach (PdfCell pr in table1forpage2.CellRange(rowcountspread - 2, 0, rowcountspread - 2, 0).Cells)
                                //{
                                //    pr.ColSpan = 2;
                                //}
                                table1forpage2.Cell(rowcountspread - 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                foreach (PdfCell pr in table1forpage2.CellRange(rowcountspread - 1, 0, rowcountspread - 1, 0).Cells)
                                {
                                    pr.ColSpan = columncountspread;
                                }
                            }


                            table1forpage2.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Columns[0].SetWidth(30);

                            Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 2, 230, 591, 600));
                            mypdfpage.Add(newpdftabpage2);

                            PdfRectangle pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
                            mypdfpage1.Add(pr3);

                            string partone = d2.GetFunction("select  TextVal as part1 from CoCurr_Activitie ca,textvaltable tv where ca.Title_Name=tv.TextCode and TextCriteria = 'RTnam' and ca.Degree_Code='" + degreecode + "' and Batch_Year='" + batchyear + "' and SubTitle='1a'");

                            Double getheigh = newpdftabpage2.Area.Height;
                            getheigh = Math.Round(getheigh, 2);
                            double page2col = getheigh + 110;
                            if (dropterm.SelectedItem.Text == "2")
                            {
                                //PdfTextArea pdf28 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 80, page2col, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Cumulative Grade Point Average (CGPA)");
                                //mypdfpage1.Add(pdf28);
                            }



                            page2col = page2col + 30;
                            PdfArea overallgradepa1 = new PdfArea(mydoc, 70, page2col, 220, 28);
                            PdfRectangle overallgradepa1pr3 = new PdfRectangle(mydoc, overallgradepa1, Color.Black);

                            page2col = page2col + 5;
                            if (dropterm.SelectedItem.Text == "2")
                            {

                                //PdfTextArea pdf29 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 40, 600, 595, 50), System.Drawing.ContentAlignment.TopLeft, "*Upgraded Grade Part 2 (2A)");
                                //mypdfpage1.Add(pdf29);
                            }
                            page2col = page2col + 40;



                            mypdfpage.Add(pdf1);
                            mypdfpage.Add(pdf11);
                            mypdfpage.Add(pdf12);
                            mypdfpage.Add(pdf13);
                            mypdfpage.Add(pdf14);
                            mypdfpage.Add(pdf18);
                            mypdfpage.Add(pr1);



                            // -------- add1 end
                            DataTable dpdfhealth = new DataTable();
                            DataSet dhealth = new DataSet();

                            page2col = 10;

                            if (dropterm.SelectedItem.Text == "2")
                            {
                                Gios.Pdf.PdfTable tablepage4b = mydoc.NewTable(f3, 5, 2, 5);
                                //table = mydoc.NewTable(Fontbold1, 7, 2, 1);
                                tablepage4b.VisibleHeaders = false;
                                tablepage4b.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                tablepage4b.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage4b.Cell(0, 0).SetContent("Self Awareness");
                                tablepage4b.Cell(0, 0).SetFont(f11);
                                foreach (PdfCell pr in tablepage4b.CellRange(0, 0, 0, 0).Cells)
                                {
                                    pr.ColSpan = 2;
                                }


                                tablepage4b.Cell(0, 0).SetColors(Color.Black, ColorTranslator.FromHtml("#fccd99"));

                                tablepage4b.Cell(1, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                tablepage4b.Cell(1, 0).SetContent("My Goals");
                                tablepage4b.Cell(1, 0).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));

                                tablepage4b.Cell(2, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                tablepage4b.Cell(2, 0).SetContent("My Strengths");
                                tablepage4b.Cell(2, 0).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));

                                tablepage4b.Cell(3, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                tablepage4b.Cell(3, 0).SetContent("My Interests and Hobbies");
                                tablepage4b.Cell(3, 0).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));

                                tablepage4b.Cell(4, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                tablepage4b.Cell(4, 0).SetContent("Responsibilities Discharged / Exceptional Achievements");
                                tablepage4b.Cell(4, 0).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));

                                tablepage4b.Columns[0].SetWidth(150);
                                //tablepage4b.Columns[1].SetWidth(150);

                                //tablepage4b.Cell(0, 0).SetCellPadding(6);
                                //tablepage4b.Cell(1, 0).SetCellPadding(6);
                                //tablepage4b.Cell(2, 0).SetCellPadding(6);
                                //tablepage4b.Cell(3, 0).SetCellPadding(1);

                                //foreach (PdfCell rr in tablepage4b.Cells)
                                //    rr.SetCellPadding(18);
                                Gios.Pdf.PdfTablePage newpdftabpage4b = tablepage4b.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 2, getheigh + 230, 591, 250));
                                mypdfpage.Add(newpdftabpage4b);

                                tablepage4b = mydoc.NewTable(f3, 3, 7, 5);
                                //table = mydoc.NewTable(Fontbold1, 7, 2, 1);
                                tablepage4b.VisibleHeaders = false;
                                tablepage4b.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                tablepage4b.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage4b.Cell(0, 0).SetContent("Health Status");
                                tablepage4b.Cell(0, 0).SetFont(f11);

                                string sdddd = Convert.ToString(dv[0]["Strenghts"]);
                                tablepage4b.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage4b.Cell(2, 0).SetContent(Convert.ToString(dv[0]["Strenghts"]));
                                tablepage4b.Cell(2, 0).SetFont(f4);

                                foreach (PdfCell pr in tablepage4b.CellRange(0, 0, 0, 0).Cells)
                                {
                                    pr.RowSpan = 2;
                                }


                                tablepage4b.Cell(0, 0).SetColors(Color.Black, ColorTranslator.FromHtml("#fccd99"));

                                tablepage4b.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage4b.Cell(0, 1).SetContent("Height");

                                tablepage4b.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage4b.Cell(2, 1).SetContent(Convert.ToString(dv[0]["StudHeight"]));
                                tablepage4b.Cell(2, 1).SetFont(f4);


                                tablepage4b.Cell(0, 1).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));
                                foreach (PdfCell pr in tablepage4b.CellRange(0, 1, 0, 1).Cells)
                                {
                                    pr.RowSpan = 2;
                                }

                                tablepage4b.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage4b.Cell(0, 2).SetContent("Weight");
                                tablepage4b.Cell(0, 2).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));

                                tablepage4b.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage4b.Cell(2, 2).SetContent(Convert.ToString(dv[0]["StudWeight"]));
                                tablepage4b.Cell(2, 2).SetFont(f4);


                                foreach (PdfCell pr in tablepage4b.CellRange(0, 2, 0, 2).Cells)
                                {
                                    pr.RowSpan = 2;
                                }

                                tablepage4b.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage4b.Cell(0, 3).SetContent("Blood Group");
                                tablepage4b.Cell(0, 3).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));

                                tablepage4b.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string bloodd = da.GetFunctionv("select textval FROM textvaltable where TextCode='" + Convert.ToString(dv[0]["bldgrp"]) + "'");
                                tablepage4b.Cell(2, 3).SetContent(bloodd);
                                tablepage4b.Cell(2, 3).SetFont(f4);

                                foreach (PdfCell pr in tablepage4b.CellRange(0, 3, 0, 3).Cells)
                                {
                                    pr.RowSpan = 2;
                                }


                                tablepage4b.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage4b.Cell(0, 4).SetContent("Vision");
                                tablepage4b.Cell(0, 4).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));

                                foreach (PdfCell pr in tablepage4b.CellRange(0, 4, 0, 4).Cells)
                                {
                                    pr.ColSpan = 2;
                                }

                                tablepage4b.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage4b.Cell(1, 4).SetContent("L");
                                tablepage4b.Cell(1, 4).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));

                                tablepage4b.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage4b.Cell(2, 4).SetContent(Convert.ToString(dv[0]["VisionLeft"]));
                                tablepage4b.Cell(2, 4).SetFont(f4);

                                tablepage4b.Cell(1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage4b.Cell(1, 5).SetContent("R");
                                tablepage4b.Cell(1, 5).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));

                                tablepage4b.Cell(2, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage4b.Cell(2, 5).SetContent(Convert.ToString(dv[0]["VisionRight"]));
                                tablepage4b.Cell(2, 5).SetFont(f4);

                                tablepage4b.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage4b.Cell(0, 6).SetContent("Dental Hygiene");
                                tablepage4b.Cell(0, 6).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));

                                tablepage4b.Cell(2, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage4b.Cell(2, 6).SetContent(Convert.ToString(dv[0]["DentalHygiene"]));
                                tablepage4b.Cell(2, 6).SetFont(f4);

                                foreach (PdfCell pr in tablepage4b.CellRange(0, 6, 0, 6).Cells)
                                {
                                    pr.RowSpan = 2;
                                }



                                // tablepage4b.Columns[0].SetWidth(90);
                                //tablepage4b.Columns[6].SetWidth(150);

                                //tablepage4b.Cell(0, 0).SetCellPadding(6);
                                //tablepage4b.Cell(1, 0).SetCellPadding(6);
                                //tablepage4b.Cell(2, 0).SetCellPadding(6);
                                //tablepage4b.Cell(3, 0).SetCellPadding(1);

                                //foreach (PdfCell rr in tablepage4b.Cells)
                                //    rr.SetCellPadding(18);
                                newpdftabpage4b = tablepage4b.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 2, 770, 591, 250));
                                mypdfpage1.Add(newpdftabpage4b);


                            }

                            PdfTextArea pdf460 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 260, 580, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Sign:");





                            Gios.Pdf.PdfTable tablepage4c = mydoc.NewTable(Fontmedium, 4, 3, 1);
                            //table = mydoc.NewTable(Fontbold1, 7, 2, 1);
                            tablepage4c.VisibleHeaders = false;
                            tablepage4c.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                            tablepage4c.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            tablepage4c.Cell(0, 0).SetContent("");
                            tablepage4c.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage4c.Cell(0, 1).SetContent("Term - I ");
                            tablepage4c.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage4c.Cell(0, 2).SetContent("Term - II   ");

                            tablepage4c.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            tablepage4c.Cell(1, 0).SetContent("Class Teacher");
                            tablepage4c.Cell(1, 1).SetContentAlignment(ContentAlignment.TopLeft);
                            tablepage4c.Cell(1, 1).SetContent("");
                            tablepage4c.Cell(1, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            tablepage4c.Cell(1, 2).SetContent("");

                            tablepage4c.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            tablepage4c.Cell(2, 0).SetContent("Principal");
                            tablepage4c.Cell(2, 1).SetContentAlignment(ContentAlignment.TopLeft);
                            tablepage4c.Cell(2, 1).SetContent("");
                            tablepage4c.Cell(2, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            tablepage4c.Cell(2, 2).SetContent("");

                            tablepage4c.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            tablepage4c.Cell(3, 0).SetContent("Parent");
                            tablepage4c.Cell(3, 1).SetContentAlignment(ContentAlignment.TopLeft);
                            tablepage4c.Cell(3, 1).SetContent("");
                            tablepage4c.Cell(3, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            tablepage4c.Cell(3, 2).SetContent("");

                            foreach (PdfCell rr in tablepage4c.Cells)
                                rr.SetCellPadding(15);
                            Gios.Pdf.PdfTablePage newpdftabpage4c = tablepage4c.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 610, 550, 600));

                            Gios.Pdf.PdfTablePage addtabletopage;
                            PdfTextArea parttitiles;
                            PdfRectangle border = new PdfRectangle(mydoc, pa1, Color.Black);


                            sql = " select  ca.CoCurr_ID,ca.Title_Name,tv.TextCode, tv.TextVal,ca.SubTitle from activity_entry ae,CoCurr_Activitie ca,textvaltable tv where ae.CoCurr_ID=ca.CoCurr_ID and ae.Batch_Year=ca.Batch_Year and ae.Degree_Code=ca.Degree_Code and ae.Batch_Year='" + batchyear + "' and ae.Degree_Code='" + degreecode + "' and term='2' and tv.TextCode=ae.ActivityTextVal  and ae.ActivityTextVal in (select ActivityTextVal from CoCurrActivitie_Det where Roll_No='" + Roll_No + "' and Degree_Code='" + degreecode + "' and Batch_Year='" + batchyear + "' and mark<>0 )  order by SubTitle";

                            DataSet partsds = new DataSet();
                            ArrayList arrcourrid = new ArrayList();
                            string parttitle1a = string.Empty;
                            Boolean flag = true;
                            ArrayList partcolumnnames = new ArrayList();
                            partsds.Clear();
                            partsds = d2.select_method_wo_parameter(sql, "Text");

                            if (partsds.Tables[0].Rows.Count > 0)
                            {
                                PdfTextArea pdf210as = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 10, 620, 50), System.Drawing.ContentAlignment.TopCenter, "CO-SCHOLASTIC Part-2 & Part-3");
                                mypdfpage1.Add(pdf210as);
                                DataView partdv = new DataView();
                                arrcourrid.Clear();
                                for (int i = 0; i < partsds.Tables[0].Rows.Count; i++)
                                {
                                    string courrid = Convert.ToString(partsds.Tables[0].Rows[i]["CoCurr_ID"]);
                                    string partnamess = Convert.ToString(partsds.Tables[0].Rows[i]["SubTitle"]);
                                    if (partnamess.Contains('2'))
                                    {
                                        partnamess = "Part 2 : (" + Convert.ToString(partsds.Tables[0].Rows[i]["SubTitle"]) + ")";
                                    }
                                    if (partnamess.Contains('3'))
                                    {
                                        partnamess = "Part 3 :(" + Convert.ToString(partsds.Tables[0].Rows[i]["SubTitle"]) + ")";
                                    }
                                    if (!arrcourrid.Contains(courrid))
                                    {
                                        partsds.Tables[0].DefaultView.RowFilter = "CoCurr_ID='" + courrid + "'";
                                        partdv = partsds.Tables[0].DefaultView;
                                        int partrowcount = 0;
                                        partrowcount = partdv.Count;
                                        sql = "select IsActivity,IsActDesc,IsGrade  from CoCurr_Activitie where CoCurr_ID='" + courrid + "' ";
                                        ds.Clear();
                                        ds = d2.select_method_wo_parameter(sql, "Text");
                                        int colcountpart = 0;
                                        string colheadername = string.Empty;
                                        for (int dd = 0; dd < ds.Tables[0].Rows.Count; dd++)
                                        {
                                            if (Convert.ToString(ds.Tables[0].Rows[0][0]).Trim().ToLower() == "true")
                                            {
                                                colcountpart++;
                                                partcolumnnames.Add("Activity");
                                                if (colheadername == "")
                                                {
                                                    colheadername = "Area of Assessment";
                                                }
                                            }
                                            if (Convert.ToString(ds.Tables[0].Rows[0][1]).Trim().ToLower() == "true")
                                            {
                                                colcountpart++;
                                                partcolumnnames.Add("Description");
                                                if (colheadername == "")
                                                {
                                                    colheadername = "Descriptive Indicators";
                                                }
                                                else
                                                {
                                                    colheadername = colheadername + ";" + "Descriptive Indicators";
                                                }
                                            }
                                            if (Convert.ToString(ds.Tables[0].Rows[0][2]).Trim().ToLower() == "true")
                                            {
                                                colcountpart++;
                                                partcolumnnames.Add("Grade");
                                                if (colheadername == "")
                                                {
                                                    colheadername = "Grade";
                                                }
                                                else
                                                {
                                                    colheadername = colheadername + ";" + "Grade";
                                                }
                                            }
                                        }

                                        Gios.Pdf.PdfTable tableparts = mydoc.NewTable(Fontsmall1, partrowcount + 2, colcountpart + 1, 6);
                                        Gios.Pdf.PdfTable tablepartsduplicate = mydoc.NewTable(Fontsmall1, partrowcount + 2, colcountpart + 1, 6);
                                        tableparts.VisibleHeaders = false;
                                        tablepartsduplicate.VisibleHeaders = false;
                                        tablepartsduplicate.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        tableparts.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        string[] splitcolheadername = colheadername.Split(';');
                                        if (splitcolheadername.GetUpperBound(0) > 0)
                                        {
                                            foreach (PdfCell pr in tableparts.CellRange(0, 0, 0, 0).Cells)
                                            {
                                                int colss = Convert.ToInt32(splitcolheadername.GetUpperBound(0) + 2);
                                                pr.ColSpan = colss;
                                            }
                                            foreach (PdfCell pr in tablepartsduplicate.CellRange(0, 0, 0, 0).Cells)
                                            {
                                                int colss = Convert.ToInt32(splitcolheadername.GetUpperBound(0) + 2);
                                                pr.ColSpan = colss;
                                            }
                                            for (int jf = 0; jf <= splitcolheadername.GetUpperBound(0); jf++)
                                            {

                                                //table1forpage2.Cell(0, ii).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));
                                                //table1forpage2.Cell(1, ii).SetColors(Color.Black, ColorTranslator.FromHtml("#fccd99"));
                                                tableparts.Cell(1, 0).SetContent("Sr.No.");
                                                tableparts.Cell(1, 0).SetFont(f9);
                                                tableparts.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tableparts.Cell(1, 0).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));

                                                tableparts.Cell(1, jf + 1).SetContent(splitcolheadername[jf]);
                                                tableparts.Cell(1, jf + 1).SetFont(f9);
                                                tableparts.Cell(1, jf + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tableparts.Cell(1, jf + 1).SetColors(Color.Black, ColorTranslator.FromHtml("#cefecc"));
                                                tablepartsduplicate.Cell(1, jf + 1).SetContent(splitcolheadername[jf]);
                                                tablepartsduplicate.Cell(1, jf + 1).SetFont(f9);
                                                tablepartsduplicate.Cell(1, jf + 1).SetContentAlignment(ContentAlignment.MiddleCenter);


                                            }

                                            for (int j = 0; j < partdv.Count; j++)
                                            {
                                                parttitle1a = da.GetFunction(" select textval from textvaltable where TextCode= '" + Convert.ToString(partdv[0]["Title_Name"]) + "'");
                                                tableparts.Cell(0, 0).SetContent(partnamess + " " + parttitle1a);
                                                tableparts.Cell(0, 0).SetColors(Color.Black, ColorTranslator.FromHtml("#fccd99"));
                                                tableparts.Cell(0, 0).SetFont(f9);
                                                tableparts.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Cell(j + 2, 0).SetContent(Convert.ToString(j + 1));
                                                tableparts.Cell(j + 2, 0).SetFont(f6);
                                                tableparts.Cell(j + 2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tableparts.Columns[0].SetWidth(3);
                                                for (int partcolumn = 0; partcolumn < partcolumnnames.Count; partcolumn++)
                                                {
                                                    string sqlff = string.Empty;
                                                    if (Convert.ToString(splitcolheadername[partcolumn]).ToLower() == "area of assessment")
                                                    {
                                                        sqlff = " tv.TextVal as Activity";

                                                    }
                                                    else if (Convert.ToString(splitcolheadername[partcolumn]).ToLower() == "grade")
                                                    {
                                                        sqlff = " ag.Grade";

                                                    }
                                                    else
                                                    {
                                                        sqlff = "ag.description";
                                                    }
                                                    string ssss = "select " + sqlff + " from activity_gd ag ,CoCurrActivitie_Det cd,textvaltable tv where ag.ActivityTextVal=cd.ActivityTextVal and tv.TextCode=ag.ActivityTextVal and tv.TextCode=cd.ActivityTextVal and  cd.ActivityTextVal='" + Convert.ToString(partdv[j]["Textcode"]) + "'  and cd.Roll_No='" + Roll_No + "' and ag.term=cd.term  and ag.term='2' and mark between frompoint and topoint ";
                                                    sqlff = da.GetFunction("select " + sqlff + " from activity_gd ag ,CoCurrActivitie_Det cd,textvaltable tv where ag.ActivityTextVal=cd.ActivityTextVal and tv.TextCode=ag.ActivityTextVal and tv.TextCode=cd.ActivityTextVal and  cd.ActivityTextVal='" + Convert.ToString(partdv[j]["Textcode"]) + "'  and cd.Roll_No='" + Roll_No + "' and mark between frompoint and topoint ");
                                                    tableparts.Cell(j + 2, partcolumn + 1).SetContent(sqlff);
                                                    tableparts.Cell(j + 2, partcolumn + 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    if (Convert.ToString(splitcolheadername[partcolumn]) == "Grade")
                                                    {
                                                        tableparts.Columns[partcolumn + 1].SetWidth(3);
                                                        tableparts.Cell(j + 2, partcolumn + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    }

                                                    if (Convert.ToString(splitcolheadername[partcolumn]).ToLower() == "area of assessment")
                                                    {
                                                        tableparts.Columns[partcolumn + 1].SetWidth(10);
                                                    }
                                                    tablepartsduplicate.Cell(j + 2, partcolumn + 1).SetContent(sqlff);
                                                }

                                            }


                                            page2col = page2col + 10;
                                            addtabletopage = tablepartsduplicate.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 2, page2col, 591, 600));

                                            getheigh = addtabletopage.Area.Height;
                                            getheigh = Math.Round(getheigh, 2);

                                            double dummycolval = page2col + getheigh + 20;
                                            if (842 > dummycolval)
                                            {

                                            }
                                            else
                                            {
                                                page2col = page2col + 2;
                                            }

                                            // page2col = page2col + caltableheight;
                                            if (842 > dummycolval && flag == true)
                                            {
                                                //parttitiles = new PdfTextArea(f6, System.Drawing.Color.Black, new PdfArea(mydoc, 4, page2col, 595, 50), System.Drawing.ContentAlignment.TopLeft, parttitle1a);
                                                //mypdfpage1.Add(parttitiles);
                                                page2col = page2col + 5;
                                                addtabletopage = tableparts.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 2, page2col, 591, 600));
                                                mypdfpage1.Add(addtabletopage);
                                                page2col = page2col + getheigh;
                                            }
                                            else if (842 > dummycolval)
                                            {
                                                //parttitiles = new PdfTextArea(f6, System.Drawing.Color.Black, new PdfArea(mydoc, 4, page2col, 595, 50), System.Drawing.ContentAlignment.TopLeft, parttitle1a);
                                                //mypdfpage1.Add(parttitiles);
                                                page2col = page2col + 5;
                                                Gios.Pdf.PdfTablePage addtabletopagenew = tableparts.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 2, page2col, 591, 600));
                                                mypdfpage1.Add(addtabletopagenew);
                                                page2col = page2col + getheigh;

                                            }
                                            else
                                            {
                                                flag = false;
                                                mypdfpage1.SaveToDocument();
                                                mypdfpage1 = mydoc.NewPage();
                                                mypdfpage1.Add(border);
                                                page2col = 40;
                                                //parttitiles = new PdfTextArea(f6, System.Drawing.Color.Black, new PdfArea(mydoc, 4, page2col, 595, 50), System.Drawing.ContentAlignment.TopLeft, parttitle1a);
                                                //mypdfpage1.Add(parttitiles);
                                                page2col = page2col + 5;
                                                Gios.Pdf.PdfTablePage addtabletopagenew = tableparts.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 2, page2col, 591, 600));
                                                mypdfpage1.Add(addtabletopagenew);
                                                page2col = page2col + getheigh;
                                                //parttitiles = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 22, 210, 595, 50), System.Drawing.ContentAlignment.TopLeft, "part1");
                                                //mypdfpage.Add(parttitiles);
                                            }
                                        }
                                        // double caltableheight = (((partdv.Count+1) * 10) * 5) / 2;
                                        // table addd pdf

                                        partcolumnnames.Clear();
                                        // mypdfpage.SaveToDocument();
                                        arrcourrid.Add(courrid);

                                    }


                                }

                            }

                            if (dropterm.SelectedItem.Text == "2")
                            {
                                mypdfpage.SaveToDocument();
                                mypdfpage1.SaveToDocument();
                                //mypdfpage2.SaveToDocument();
                                //mypdfpage6.SaveToDocument();
                                //mypdfpagefinal.SaveToDocument();
                                //mypdfpage5.SaveToDocument();
                                //mypdfpage = mydoc.NewPage();
                                //mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                mypdfpage1 = mydoc.NewPage();
                                //mypdfpage2 = mydoc.NewPage();
                                //mypdfpage6 = mydoc.NewPage();
                                //mypdfpagefinal = mydoc.NewPage();
                                //mypdfpage5 = mydoc.NewPage();
                            }
                            else
                            {

                                mypdfpage.SaveToDocument();
                                // mypdfpage1.SaveToDocument();

                                mypdfpage = mydoc.NewPage();
                                //mypdfpage1 = mydoc.NewPage();
                            }
                        }
                    }
                }
            }

            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "rankcard" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";

                mydoc.SaveToFile(szPath + szFile);
                mydoc.SaveToFile(szPath + szFile);

                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }

        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void bindheaderformat1()
    {
        //fpspread.Sheets[0].ColumnCount = 3;
        fpspread.Sheets[0].ColumnCount = 1;
        fpspread.Sheets[0].RowCount = 0;
        fpspread.Sheets[0].ColumnHeader.Rows.Count = 2;
        DropDownList cblterm = new DropDownList();
        cblterm.Items.Clear();
        lblerrormsg.Visible = false;
        lblerrormsg.Text = string.Empty;
        string termselectf1 = Convert.ToString(dropterm.SelectedItem.Text);
        if (termselectf1 == "1")
        {
            cblterm.Items.Add("1");
        }
        else if (termselectf1 == "2")
        {
            cblterm.Items.Add("1");
            cblterm.Items.Add("2");
        }

        for (int i = 0; i < cblterm.Items.Count; i++)
        {
            cblterm.Items[i].Selected = true;
        }
        DataTable spancolval = new DataTable();
        spancolval.Clear();
        spancolval.Columns.Clear();
        spancolval.Columns.Add("Colno");

        spancolval.Columns.Add("colc");
        spancolval.Columns.Add("rowc");
        spancolval.Columns.Add("Colrow");

        string otherssubject_sql = string.Empty;
        int termcount = 0;
        ArrayList colfaspan = new ArrayList();
        avg_grade_col.Clear();
        dtallcol.Columns.Clear();
        dtallotherscol.Columns.Clear();
        // fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
        dtallcol.Columns.Add("Colname");
        dtallcol.Columns.Add("colno");
        dtallcol.Columns.Add("Criteria nos");
        dtallcol.Columns.Add("Term");

        dtFASAcol.Columns.Add("Colname");
        dtFASAcol.Columns.Add("colno");
        dtFASAcol.Columns.Add("Term");


        dtallotherscol.Columns.Add("Colname");
        dtallotherscol.Columns.Add("colno");
        dtallotherscol.Columns.Add("subjetno");

        otherssubjectcode = string.Empty;

        string fasaCRITERIA_NO = string.Empty;
        double fatotal = 0;
        //double satotal = 0;
        //double fulltotal = 0;
        double maxfatotal = 0;
        double maxsatotal = 0;
        double maxfulltotal = 0;
        // collcode = " and r.college_code='" + Convert.ToString(ddschool.SelectedItem.Value) + "'";
        batchyear = "  and y.Batch_Year='" + Convert.ToString(dropyear.SelectedItem.Text) + "'";
        degreecode = "  and degree_code='" + Convert.ToString(ddstandard.SelectedItem.Value) + "'";
        string selterm = string.Empty;
        for (int i = 0; i < cblterm.Items.Count; i++)
        {
            if (cblterm.Items[i].Selected == true)
            {
                termcount++;
                if (selterm == "")
                {
                    selterm = cblterm.Items[i].Text;
                }
                else
                {
                    selterm = selterm + "','" + cblterm.Items[i].Text;
                }
            }
        }
        if (selterm != "")
        {
            // term = " and semester in ('" + selterm + "')";
            term = " and semester in ('3')";
            selterm = " and semester in ('" + selterm + "')";
        }

        for (int i = 0; i < cblterm.Items.Count; i++)
        {
            if (cblterm.Items[i].Selected == true)
            {
                term = " and semester in ('" + Convert.ToString(cblterm.Items[i].Text) + "')";
                otherssubjectcode = string.Empty;
                otherssubject_sql = "select distinct  subject_no,subject_name from subject s,syllabus_master y,sub_sem ss  where s.syll_code = y.syll_code and ss.syll_code=s.syll_code and s.subType_no=ss.subType_no and y.syll_code=ss.syll_code   and subject_type='others' and promote_count=1 ";
                otherssubject_sql = otherssubject_sql + batchyear + degreecode + term + " order by subject_no,subject_name;";

                otherds_subject.Clear();
                otherds_subject = d2.select_method_wo_parameter(otherssubject_sql, "Text");


                for (int ii = 0; ii < otherds_subject.Tables[0].Rows.Count; ii++)
                {
                    if (otherssubjectcode == "")
                    {
                        otherssubjectcode = Convert.ToString(otherds_subject.Tables[0].Rows[ii][0]);
                        otherssubjectcode01 = Convert.ToString(otherds_subject.Tables[0].Rows[ii][0]);
                    }
                    else
                    {
                        otherssubjectcode = otherssubjectcode + "','" + Convert.ToString(otherds_subject.Tables[0].Rows[ii][0]);
                        otherssubjectcode01 = otherssubjectcode01 + "','" + Convert.ToString(otherds_subject.Tables[0].Rows[ii][0]);
                    }
                }

                if (otherssubjectcode != "")
                {
                    otherssubjectcode = " and c.subject_no not in('" + otherssubjectcode + "')";
                    otherssubjectcode01 = " and c.subject_no  in('" + otherssubjectcode01 + "')";
                }
                else
                {
                    otherssubjectcode = string.Empty;
                    otherssubjectcode01 = string.Empty;
                }

                string subject_sql = "select distinct  subject_code,subject_no,subject_name from subject s,syllabus_master y,sub_sem ss  where s.syll_code = y.syll_code and ss.syll_code=s.syll_code and s.subType_no=ss.subType_no and y.syll_code=ss.syll_code   and subject_type<>'others' ";
                subject_sql = subject_sql + batchyear + degreecode + term + "  order by subject_no,subject_name;";

                subject_sql = subject_sql + ";" + "SELECT distinct c.Istype,conversion as Conversion_value,CRITERIA_NO FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no   " + batchyear + "   " + degreecode + "  " + term + " " + otherssubjectcode + "  and CRITERIA_NO <>''  and c.Istype<>'settings' and  c.Istype not like 'SA%' and c.Istype not like 'prac%'";
                subject_sql = subject_sql + ";" + "SELECT distinct c.Istype,conversion as Conversion_value,CRITERIA_NO  FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no   " + batchyear + "   " + degreecode + "  " + term + " " + otherssubjectcode + "  and CRITERIA_NO <>''  and c.Istype<>'settings' and  c.Istype  like 'SA%'  and c.Istype not like 'prac%'";

                subject_sql = subject_sql + ";" + "SELECT distinct c.Istype,conversion as Conversion_value FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no   " + batchyear + "   " + degreecode + "  " + term + " " + otherssubjectcode01 + "  and CRITERIA_NO <>''  and c.Istype<>'settings'";

                ds_subject.Clear();
                ds_subject = d2.select_method_wo_parameter(subject_sql, "Text");

                twosubcount = ds_subject.Tables[0].Rows.Count;
                int checkallvaluescount = 0;
                // fppagesize = twosubcount;
                if (ds_subject.Tables[0].Rows.Count > 0)
                {
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SCHOLASTIC AREA";
                    fpspread.Sheets[0].ColumnHeader.Cells[1, 0].Text = "Subject";

                    int cc = 0;
                    int startcol = 0;

                    double totalfa = 0;
                    double satotal = 0;

                    fpspread.Sheets[0].ColumnCount++;
                    cc++;
                    startcol = fpspread.Sheets[0].ColumnCount - 1;

                    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Text = "TERM " + Convert.ToString(cblterm.Items[i].Text) + "";
                    //  fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = "FA";

                    for (int ii = 0; ii < 2; ii++)
                    {
                        if (ds_subject.Tables[1].Rows.Count > ii)
                        {
                            checkallvaluescount++;
                            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds_subject.Tables[1].Rows[ii]["Istype"]) + "  " + Convert.ToString(ds_subject.Tables[1].Rows[ii]["Conversion_value"]);
                            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds_subject.Tables[1].Rows[ii]["CRITERIA_NO"]);
                            totalfa = totalfa + Convert.ToDouble(Convert.ToString(ds_subject.Tables[1].Rows[ii]["Conversion_value"]));

                            dtallcol.Rows.Add(Convert.ToString(ds_subject.Tables[1].Rows[ii]["Istype"]), fpspread.Sheets[0].ColumnCount - 1, Convert.ToString(ds_subject.Tables[1].Rows[ii]["CRITERIA_NO"]), Convert.ToString(cblterm.Items[i].Text));
                        }
                        else
                        {
                            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = "-";
                            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Tag = string.Empty;
                        }
                        cc++;
                        fpspread.Sheets[0].ColumnCount++;
                    }

                    for (int ii = 0; ii < 1; ii++)
                    {
                        if (ds_subject.Tables[2].Rows.Count > ii)
                        {
                            checkallvaluescount++;
                            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = "SA(" + Convert.ToString(ds_subject.Tables[2].Rows[ii]["Conversion_value"]) + ")";
                            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds_subject.Tables[2].Rows[ii]["CRITERIA_NO"]);
                            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds_subject.Tables[2].Rows[ii]["CRITERIA_NO"]);
                            satotal = satotal + Convert.ToDouble(Convert.ToString(ds_subject.Tables[2].Rows[ii]["Conversion_value"]));

                            // dtallcol.Rows.Add("SA", fpspread.Sheets[0].ColumnCount - 1, Convert.ToString(ds_subject.Tables[1].Rows[ii]["CRITERIA_NO"]), cblterm.Items[i].Text.ToString());
                            fpspread.Sheets[0].ColumnHeaderSpanModel.Add(1, fpspread.Sheets[0].ColumnCount - 1, 2, 1);

                            cc++;
                            dtallcol.Rows.Add("SA", fpspread.Sheets[0].ColumnCount - 1, "", Convert.ToString(cblterm.Items[i].Text));
                        }
                        else
                        {
                            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = "-";
                            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Tag = string.Empty;
                        }
                        fpspread.Sheets[0].ColumnCount++;
                    }

                    if (checkallvaluescount == 3)
                    {
                        fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = "Total";
                        dtallcol.Rows.Add("Total", fpspread.Sheets[0].ColumnCount - 1, "", Convert.ToString(cblterm.Items[i].Text));
                    }
                    else
                    {
                        fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = "-";
                    }



                    // fpspread.Sheets[0].ColumnCount++;
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(1, fpspread.Sheets[0].ColumnCount - 1, 2, 1);

                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(1, fpspread.Sheets[0].ColumnCount - 1, 2, 1);
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, startcol, 1, cc + 1);

                }

            }
        }

        if (termselectf1 == "2")
        {
            fpspread.Sheets[0].ColumnCount++;
            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = "FA 40%";
            dtallcol.Rows.Add("Overallfa", fpspread.Sheets[0].ColumnCount - 1, "", "");

            fpspread.Sheets[0].ColumnCount++;
            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = "SA 60%";
            dtallcol.Rows.Add("Overallsa", fpspread.Sheets[0].ColumnCount - 1, "", "");

            fpspread.Sheets[0].ColumnCount++;
            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = "Overall 100%";
            dtallcol.Rows.Add("OverallTotal", fpspread.Sheets[0].ColumnCount - 1, "", "");

            fpspread.Sheets[0].ColumnCount++;
            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = "Grade Point";
        }

        fpspread.Sheets[0].RowCount = 0;
        if (ds_subject.Tables[0].Rows.Count > 0)
        {
            for (int ii = 0; ii < ds_subject.Tables[0].Rows.Count; ii++)
            {

                fpspread.Sheets[0].RowCount++;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds_subject.Tables[0].Rows[ii]["subject_name"]);
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds_subject.Tables[0].Rows[ii]["subject_code"]);

                // dtallcol.Rows.Add("SA", fpspread.Sheets[0].ColumnCount - 1, ds_subject.Tables[1].Rows[ii]["CRITERIA_NO"].ToString(), cblterm.Items[i].Text.ToString());
                // fpspread.Sheets[0].ColumnHeaderSpanModel.Add(1, fpspread.Sheets[0].ColumnCount - 1, 2, 1);
            }
        }



        //fpspread.Sheets[0].ColumnCount++;
        //fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Text = "Overall Total ";
        //dtallcol.Rows.Add("OverallTotal", fpspread.Sheets[0].ColumnCount - 1, "", "");
        //fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnCount - 1, 3, 1);
        //fpspread.Sheets[0].ColumnCount++;
        //fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Text = "Total ";
        //avg_grade_col.Add(fpspread.Sheets[0].ColumnCount - 1);
        //dtallcol.Rows.Add("AVRTotal", fpspread.Sheets[0].ColumnCount - 1, "", "");
        //fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnCount - 1, 3, 1);
        //fpspread.Sheets[0].ColumnCount++;
        //fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Text = "Overall Grade";
        //dtallcol.Rows.Add("AVRGrade", fpspread.Sheets[0].ColumnCount - 1, "", "");
        //fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnCount - 1, 3, 1);

        //if (otherds_subject.Tables[0].Rows.Count > 0 && termcount == 3)
        //{
        //    string otherconvetedvalue = "0";

        //    for (int i = 0; i < otherds_subject.Tables[0].Rows.Count; i++)
        //    {

        //        string str_subject_name = otherds_subject.Tables[0].Rows[i]["subject_name"].ToString();
        //        string str_subject_no = otherds_subject.Tables[0].Rows[i]["subject_no"].ToString();

        //        if (ds_subject.Tables[2].Rows.Count > 0)
        //        {
        //            otherconvetedvalue = ds_subject.Tables[3].Rows[0]["Conversion_value"].ToString();
        //        }
        //        fpspread.Sheets[0].ColumnCount++;


        //        fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Text = str_subject_name;
        //        fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
        //        fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        //        fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].ForeColor = Color.White;


        //        //fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = "Mark " + otherconvetedvalue + "";
        //        //fpspread.Sheets[0].ColumnHeaderSpanModel.Add(1, fpspread.Sheets[0].ColumnCount - 1, 2, 1);
        //        //fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
        //        //fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        //        //fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
        //        //dtallotherscol.Rows.Add("Mark", fpspread.Sheets[0].ColumnCount - 1, str_subject_no);
        //        //fpspread.Sheets[0].ColumnCount++;

        //        fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = "Grade";
        //        fpspread.Sheets[0].ColumnHeaderSpanModel.Add(1, fpspread.Sheets[0].ColumnCount - 1, 2, 1);
        //        fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
        //        fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        //        fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].ForeColor = Color.White;

        //        dtallotherscol.Rows.Add("OthersGrade", fpspread.Sheets[0].ColumnCount - 1, str_subject_no);

        //        //----fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnCount - 2, 1, 2);
        //        // fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 48, 1, 2);
        //        //fpspread.Sheets[0].ColumnHeaderSpanModel.Add(1, 48, 2, 1);
        //    }


        //}


        //fpspread.Sheets[0].ColumnCount++;
        //fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Text = "Attendance ";
        //spancolval.Rows.Add(fpspread.Sheets[0].ColumnCount - 1, (termcount * 2), 1, 0);
        //ArrayList attspan = new ArrayList();
        //attspan.Clear();
        //attspan.Add(fpspread.Sheets[0].ColumnCount - 1);
        //for (int i = 0; i < cblterm.Items.Count; i++)
        //{

        //    if (cblterm.Items[i].Selected == true)
        //    {



        //        //fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnCount - 6, 1, 2);

        //        fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = "Term " + cblterm.Items[i].Text.ToString() + "";
        //        spancolval.Rows.Add(fpspread.Sheets[0].ColumnCount - 1, 2, 1, 1);


        //        fpspread.Sheets[0].ColumnHeader.Cells[2, fpspread.Sheets[0].ColumnCount - 1].Text = "No of Days Present";
        //        //dtallcol.Rows.Add("Termatt", fpspread.Sheets[0].ColumnCount - 1, "", "");
        //        dtallcol.Rows.Add("Termatt", fpspread.Sheets[0].ColumnCount - 1, "", cblterm.Items[i].Text.ToString());
        //        // fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnCount - 1, 1, 2);
        //        fpspread.Sheets[0].ColumnCount++;

        //        fpspread.Sheets[0].ColumnHeader.Cells[2, fpspread.Sheets[0].ColumnCount - 1].Text = "%";
        //        dtallcol.Rows.Add("Termattper", fpspread.Sheets[0].ColumnCount - 1, "", cblterm.Items[i].Text.ToString());
        //        //--fpspread.Sheets[0].ColumnHeaderSpanModel.Add(1, fpspread.Sheets[0].ColumnCount - 1, 2, 1);
        //        fpspread.Sheets[0].ColumnCount++;

        //    }
        //}

        ////termcount

        ////  fpspread.Sheets[0].ColumnCount++;
        //fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Text = "Remarks";
        //fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnCount - 1, 3, 1);



        if (spancolval.Rows.Count > 0)
        {
            for (int g = 0; g < spancolval.Rows.Count; g++)
            {
                fpspread.Sheets[0].ColumnHeaderSpanModel.Add(Convert.ToInt32(Convert.ToString(spancolval.Rows[g][2])), Convert.ToInt32(Convert.ToString(spancolval.Rows[g][0])), Convert.ToInt32(Convert.ToString(spancolval.Rows[g][2])), Convert.ToInt32(Convert.ToString(spancolval.Rows[g][1])));

            }
        }
        fpspread.SaveChanges();

        fpspread.Sheets[0].PageSize = twosubcount;
        fpspread.Height = 500;
        //fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnCount - 3, 1, 2);
        //bindvaules();
        // fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, Convert.ToInt32(attspan[0].ToString()), 1, (termcount * 2));

    }

    public void bindvaulesformat1(string rollno)
    {
        try
        {
            lblerrormsg.Visible = false;
            lblerrormsg.Text = string.Empty;
            double subjecttotalfinal = 0;
            double classoveralltotal = 0;

            double overcontottalfasa = 0;
            //DataTable attendance = new DataTable();

            ArrayList gradef1 = new ArrayList();
            gradef1.Clear();
            gradef1.Add("FA1");
            gradef1.Add("FA2");

            ArrayList gradef2 = new ArrayList();
            gradef2.Clear();
            gradef2.Add("FA3");
            gradef2.Add("FA4");

            ArrayList gradefs = new ArrayList();
            gradefs.Clear();
            gradefs.Add("FS1");
            gradefs.Add("FS2");
            gradefs.Add("FS3");

            ArrayList gradesa = new ArrayList();
            gradesa.Clear();
            gradesa.Add("SA1");
            gradesa.Add("SA2");
            gradesa.Add("SA3");

            //ArrayList gradeterm = new ArrayList();
            //gradeterm.Clear();
            //gradeterm.Add("T1");
            //gradeterm.Add("T2");
            //gradeterm.Add("T3");

            int termscount = 0;
            double overalltotalall = 0;
            batchyear = Convert.ToString(dropyear.SelectedItem.Text);
            degreecode = Convert.ToString(ddstandard.SelectedItem.Value);
            //term = dropterm.SelectedItem.Text;
            string selterm = string.Empty;
            termselected.Clear();
            //for (int i = 0; i < cblterm.Items.Count; i++)
            //{
            //    if (cblterm.Items[i].Selected == true)
            //    {
            //        termscount++;
            //        termselected.Add(cblterm.Items[i].Text);
            //        if (selterm == "")
            //        {
            //            selterm = cblterm.Items[i].Text;
            //        }
            //        else
            //        {
            //            selterm = selterm + "','" + cblterm.Items[i].Text;
            //        }
            //    }
            //}
            if (selterm != "")
            {
                // term = " and semester in ('" + selterm + "')";
            }
            int checkoutfinalcal = dtallcol.Rows.Count;

            string str_colno = string.Empty;
            string str_rolladmit = string.Empty;
            string str_criteriano = string.Empty;
            string str_subject_no = string.Empty;
            string[] split_criteriano;
            double fatotal = 0;
            double satotal = 0;
            double fulltotal = 0;
            double convertedvalue = 0;
            string grademain = string.Empty;
            DataSet dsgradechk = new DataSet();
            DataSet ds = new DataSet();
            DataView dv = new DataView();
            double overallfa = 0;
            double overallsa = 0;

            double overallconfa = 0;
            double overallconsa = 0;

            int count = dtallcol.Rows.Count;
            //if (count > 0)
            //{
            //}
            //return;
            string admdate = string.Empty;
            if (count > 0)
            {
                for (int admitno = 0; admitno < fpspread.Sheets[0].RowCount; admitno++)
                {
                    int skiprow = 0;
                    string stud_roll = rollno;
                    string subjectclasscode = Convert.ToString(fpspread.Sheets[0].Cells[admitno, 0].Tag);
                    str_rolladmit = d2.GetFunction("select Roll_Admit from Registration where Roll_No='" + stud_roll + "'");
                    // term = FpSpread1.Sheets[0].Cells[admitno, 3].Text.Trim();
                    string clm = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_Admit='" + str_rolladmit + "' ;";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(clm, "text");
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    {
                        ds.Tables[1].DefaultView.RowFilter = "Roll_Admit='" + str_rolladmit + "'";
                        dv = ds.Tables[1].DefaultView;
                        int count4 = 0;
                        count4 = dv.Count;
                        if (count4 > 0)
                        {
                            admdate = Convert.ToString(dv[0]["adm_date"]);
                            string Roll_No = Convert.ToString(dv[0]["Roll_No"]);
                            currentsem = Convert.ToString(dv[0]["Current_Semester"]);
                            //string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + term + "' and degree_code='" + degreecode + "' and batch_year='" + batchyear + "'";
                            //ds.Clear();
                            //ds = d2.select_method_wo_parameter(sem, "Text");

                            //if (ds.Tables[0].Rows.Count > 0)
                            //{
                            //    string startdate = ds.Tables[0].Rows[0]["start_date"].ToString();
                            //    string enddate = ds.Tables[0].Rows[0]["end_date"].ToString();
                            //    persentmonthcal(Roll_No, admdate, startdate, enddate);
                            //    lbltot_att1 =  Convert.ToString(pre_present_date);
                            //    lbltot_work1 =  Convert.ToString(per_workingdays);
                            //}

                        }
                    }

                    for (int i = 0; i < dtallcol.Rows.Count; i++)
                    {
                        term = Convert.ToString(dtallcol.Rows[i]["term"]).Trim();
                        if (Convert.ToString(dtallcol.Rows[i]["Colname"]).Trim().ToLower() == "fa1" || Convert.ToString(dtallcol.Rows[i]["Colname"]).Trim().ToLower() == "f1")
                        {
                            str_colno = Convert.ToString(dtallcol.Rows[i]["colno"]).Trim();
                            str_criteriano = Convert.ToString(dtallcol.Rows[i]["Criteria nos"]).Trim();
                            str_subject_no = subjectclasscode;
                            term = Convert.ToString(dtallcol.Rows[i]["Term"]);
                            str_subject_no = d2.GetFunction("select distinct subject_no from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'and semester in ('" + term + "') and  subject_code='" + str_subject_no + "'");


                            fatotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no='" + str_criteriano + "'  and s.subject_no='" + str_subject_no + "'"));
                            // FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fatotal);

                            grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='fa1' and  " + fatotal + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);

                            }
                            else
                            {
                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='fa1' and  " + fatotal + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);

                                }
                            }
                        }

                        if (Convert.ToString(dtallcol.Rows[i]["Colname"]).Trim().ToLower() == "fa2")
                        {
                            str_colno = Convert.ToString(dtallcol.Rows[i]["colno"]).Trim();
                            str_criteriano = Convert.ToString(dtallcol.Rows[i]["Criteria nos"]).Trim();
                            str_subject_no = subjectclasscode;
                            term = Convert.ToString(dtallcol.Rows[i]["Term"]);
                            str_subject_no = d2.GetFunction("select distinct subject_no from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'and semester in ('" + term + "') and  subject_code='" + str_subject_no + "'");


                            fatotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no='" + str_criteriano + "'  and s.subject_no='" + str_subject_no + "'"));

                            // FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fatotal);

                            grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='fa2' and  " + fatotal + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);

                            }
                            else
                            {
                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='fa2' and  " + fatotal + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);

                                }
                            }

                        }

                        if (Convert.ToString(dtallcol.Rows[i]["Colname"]).Trim().ToLower() == "fa3")
                        {
                            str_colno = Convert.ToString(dtallcol.Rows[i]["colno"]).Trim();
                            str_criteriano = Convert.ToString(dtallcol.Rows[i]["Criteria nos"]).Trim();
                            str_subject_no = subjectclasscode;
                            term = Convert.ToString(dtallcol.Rows[i]["Term"]);
                            str_subject_no = d2.GetFunction("select distinct subject_no from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'and semester in ('" + term + "') and  subject_code='" + str_subject_no + "'");


                            fatotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no='" + str_criteriano + "'  and s.subject_no='" + str_subject_no + "'"));

                            // FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fatotal);

                            grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='fa3' and  " + fatotal + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);

                            }
                            else
                            {
                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='fa3' and  " + fatotal + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);

                                }
                            }

                        }

                        if (Convert.ToString(dtallcol.Rows[i]["Colname"]).Trim().ToLower() == "fa4")
                        {
                            str_colno = Convert.ToString(dtallcol.Rows[i]["colno"]).Trim();
                            str_criteriano = Convert.ToString(dtallcol.Rows[i]["Criteria nos"]).Trim();
                            str_subject_no = subjectclasscode;
                            term = Convert.ToString(dtallcol.Rows[i]["Term"]);
                            str_subject_no = d2.GetFunction("select distinct subject_no from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'and semester in ('" + term + "') and  subject_code='" + str_subject_no + "'");


                            fatotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no='" + str_criteriano + "'  and s.subject_no='" + str_subject_no + "'"));



                            // FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fatotal);

                            grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='fa4' and  " + fatotal + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);

                            }
                            else
                            {
                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='fa4' and  " + fatotal + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);

                                }
                            }

                        }



                        //if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "FAGrade")
                        //{
                        //    str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                        //    grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradefs[Convert.ToInt32(term) - 1].ToString() + "' and  " + fatotal + " between Frange and Trange";
                        //    dsgradechk.Clear();
                        //    dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                        //    if (dsgradechk.Tables[0].Rows.Count > 0)
                        //    {
                        //        FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                        //    }
                        //    else
                        //    {
                        //        grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradefs[Convert.ToInt32(term) - 1].ToString() + "' and  " + fatotal + " between Frange and Trange";
                        //        dsgradechk.Clear();
                        //        dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                        //        if (dsgradechk.Tables[0].Rows.Count > 0)
                        //        {
                        //            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                        //        }
                        //    }

                        //}
                        if (Convert.ToString(dtallcol.Rows[i]["Colname"]).Trim() == "SA")
                        {
                            //if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "FA")
                            //{
                            //str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();

                            str_subject_no = subjectclasscode;

                            str_subject_no = d2.GetFunction("select distinct subject_no from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'and semester in ('" + term + "') and  subject_code='" + str_subject_no + "'");


                            fatotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no is null and c.Istype='Calculate 1' and s.subject_no='" + str_subject_no + "'"));
                            convertedvalue = Convert.ToDouble(d2.GetFunction("SELECT c.conversion FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no is null and c.Istype='Calculate 1' and s.subject_no='" + str_subject_no + "'"));
                            overallconfa = overallconfa + convertedvalue;
                            overcontottalfasa = overcontottalfasa + convertedvalue;
                            // FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fatotal);
                            fulltotal = fatotal;
                            overallfa = overallfa + fatotal;
                            classoveralltotal = classoveralltotal + fatotal;
                            //}
                            str_colno = Convert.ToString(dtallcol.Rows[i]["colno"]).Trim();
                            str_criteriano = Convert.ToString(dtallcol.Rows[i]["Criteria nos"]).Trim();
                            str_subject_no = subjectclasscode;
                            str_subject_no = d2.GetFunction("select distinct subject_no from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'and semester in ('" + term + "') and  subject_code='" + str_subject_no + "'");


                            satotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no is null and c.Istype='Calculate 2' and s.subject_no='" + str_subject_no + "'"));
                            overallsa = overallsa + satotal;
                            convertedvalue = Convert.ToDouble(d2.GetFunction("SELECT c.conversion FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no is null and c.Istype='Calculate 2' and s.subject_no='" + str_subject_no + "'"));
                            classoveralltotal = classoveralltotal + satotal;
                            overallconsa = overallconsa + convertedvalue;
                            overcontottalfasa = overcontottalfasa + convertedvalue;
                            fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(satotal);
                            fulltotal = fulltotal + satotal;
                            str_colno = Convert.ToString(dtallcol.Rows[i]["colno"]).Trim();
                            grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + Convert.ToString(gradesa[Convert.ToInt32(term) - 1]) + "' and  " + satotal + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);

                            }
                            else
                            {
                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + Convert.ToString(gradesa[Convert.ToInt32(term) - 1]) + "' and  " + satotal + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);

                                }
                            }
                        }

                        if (Convert.ToString(dtallcol.Rows[i]["Colname"]).Trim() == "Total")
                        {

                            overalltotalall = overalltotalall + fulltotal;
                            str_colno = Convert.ToString(dtallcol.Rows[i]["colno"]).Trim();

                            str_colno = Convert.ToString(dtallcol.Rows[i]["colno"]).Trim();

                            if (overcontottalfasa != 0 && overcontottalfasa > 0)
                            {
                                fulltotal = (fulltotal / overcontottalfasa);
                                fulltotal = fulltotal * 100;
                            }
                            else
                            {
                                fulltotal = 0;
                            }

                            grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fulltotal + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);
                                fpspread.Sheets[0].ColumnHeader.Cells[1, (Convert.ToInt32(str_colno) - 1)].Tag = Convert.ToString(convertedvalue);
                            }
                            else
                            {
                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fulltotal + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);
                                    fpspread.Sheets[0].ColumnHeader.Cells[1, (Convert.ToInt32(str_colno) - 1)].Tag = Convert.ToString(convertedvalue);
                                }
                            }
                            fatotal = 0;
                            satotal = 0;
                            fulltotal = 0;
                            convertedvalue = 0;
                            overcontottalfasa = 0;
                        }

                        //if (checkoutfinalcal == 9)
                        //{
                        if (Convert.ToString(dtallcol.Rows[i]["Colname"]).Trim().ToLower() == "overallfa")
                        {
                            str_colno = Convert.ToString(dtallcol.Rows[i]["colno"]).Trim();
                            // FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(overallfa);
                            str_colno = Convert.ToString(dtallcol.Rows[i]["colno"]).Trim();

                            if (overallconfa != 0 && overallconfa > 0)
                            {
                                overallfa = (overallfa / overallconfa);
                                overallfa = overallfa * 100;
                            }
                            else
                            {
                                overallfa = 0;
                            }

                            grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + overallfa + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);
                                fpspread.Sheets[0].ColumnHeader.Cells[1, (Convert.ToInt32(str_colno) - 1)].Tag = Convert.ToString(convertedvalue);
                            }
                            else
                            {
                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + overallfa + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);
                                    fpspread.Sheets[0].ColumnHeader.Cells[1, (Convert.ToInt32(str_colno) - 1)].Tag = Convert.ToString(convertedvalue);
                                }
                            }
                            overallfa = 0;
                            overallconfa = 0;
                        }

                        if (Convert.ToString(dtallcol.Rows[i]["Colname"]).Trim().ToLower() == "overallsa")
                        {
                            str_colno = Convert.ToString(dtallcol.Rows[i]["colno"]).Trim();
                            // FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(overallfa);
                            str_colno = Convert.ToString(dtallcol.Rows[i]["colno"]).Trim();
                            if (overallconsa != 0 && overallconsa > 0)
                            {
                                overallsa = (overallsa / overallconsa);
                                overallsa = overallsa * 100;
                            }
                            else
                            {
                                overallsa = 0;
                            }


                            grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + overallsa + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);
                                fpspread.Sheets[0].ColumnHeader.Cells[1, (Convert.ToInt32(str_colno) - 1)].Tag = Convert.ToString(convertedvalue);
                            }
                            else
                            {
                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + overallsa + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);
                                    fpspread.Sheets[0].ColumnHeader.Cells[1, (Convert.ToInt32(str_colno) - 1)].Tag = Convert.ToString(convertedvalue);
                                }
                            }
                            overallsa = 0;
                            overallconsa = 0;
                        }

                        if (Convert.ToString(dtallcol.Rows[i]["Colname"]).Trim() == "OverallTotal")
                        {
                            str_colno = Convert.ToString(dtallcol.Rows[i]["colno"]).Trim();
                            fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(classoveralltotal);
                            str_colno = Convert.ToString(dtallcol.Rows[i]["colno"]).Trim();
                            grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + classoveralltotal + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);
                                fpspread.Sheets[0].ColumnHeader.Cells[1, (Convert.ToInt32(str_colno) - 1)].Tag = Convert.ToString(convertedvalue);
                                fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno) + 1].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Credit_Points"]);
                                cgpacalc = cgpacalc + Convert.ToDouble(Convert.ToString(dsgradechk.Tables[0].Rows[0]["Credit_Points"]));
                            }
                            else
                            {
                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + classoveralltotal + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);
                                    fpspread.Sheets[0].ColumnHeader.Cells[1, (Convert.ToInt32(str_colno) - 1)].Tag = Convert.ToString(convertedvalue);
                                    fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno + 1)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Credit_Points"]);
                                    cgpacalc = cgpacalc + Convert.ToDouble(Convert.ToString(dsgradechk.Tables[0].Rows[0]["Credit_Points"]));
                                }
                            }
                            classoveralltotal = 0;
                        }
                        // }
                    }
                    if (dtallotherscol.Rows.Count > 0)
                    {
                        term = "3";
                        for (int i = 0; i < dtallotherscol.Rows.Count; i++)
                        {
                            if (Convert.ToString(dtallotherscol.Rows[i]["Colname"]).Trim() == "OthersGrade")
                            {
                                str_colno = Convert.ToString(dtallotherscol.Rows[i]["colno"]).Trim();
                                //str_criteriano = dtallcol.Rows[i]["Criteria nos"].ToString().Trim();
                                str_subject_no = Convert.ToString(dtallotherscol.Rows[i]["subjetno"]).Trim();

                                //fatotal = Convert.ToDouble(d2.GetFunction("select top 1  r.marks_obtained from result r,registration reg,exam_type et,subjectchooser sc  where r.exam_code=et.exam_code  and reg.roll_no=r.roll_no and sc.roll_no=reg.roll_no and reg.cc=0 and reg.delflag=0 and reg.exam_flag <>'Debar'  and et.subject_no='" + str_subject_no + "' and et.subject_no=sc.subject_no  and r.roll_no='" + stud_roll + "'  ORDER BY reg.roll_no"));
                                fatotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no is null and c.Istype='Calculate 1' and s.subject_no='" + str_subject_no + "'"));
                                //double maximtotal = Convert.ToDouble(d2.GetFunction("select maxtotal from subject where subject_no='" + str_subject_no + "'"));
                                //fatotal = (fatotal / maximtotal);
                                //fatotal = fatotal * 100;
                                fatotal = Math.Round(fatotal, 2);
                                fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fatotal);
                            }

                            if (Convert.ToString(dtallotherscol.Rows[i]["Colname"]).Trim() == "OthersGrade")
                            {
                                str_colno = Convert.ToString(dtallotherscol.Rows[i]["colno"]).Trim();
                                grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fatotal + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);

                                }
                                else
                                {
                                    grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fatotal + " between Frange and Trange";
                                    dsgradechk.Clear();
                                    dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                    if (dsgradechk.Tables[0].Rows.Count > 0)
                                    {
                                        fpspread.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"]);

                                    }
                                }
                            }
                        }
                    }
                    fpspread.SaveChanges();

                    FpSpread1.SaveChanges();

                    overalltotalall = 0;


                }

            }
            FpSpread1.SaveChanges();


        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }

    }

    public void bindstudentmark(string rollno)
    {
        try
        {
            if (booleanheaderformat1 == true)
            {
                bindheaderformat1();
                booleanheaderformat1 = false;
            }

            bindvaulesformat1(rollno);
        }

        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void bindbutn_two(string rollno)
    {
        try
        {
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;
            Gios.Pdf.PdfPage mypdfpage1;
            Gios.Pdf.PdfPage mypdfpage2;
            Gios.Pdf.PdfPage mypdfpage6;
            Gios.Pdf.PdfPage mypdfpagefinal;
            Gios.Pdf.PdfPage mypdfpage5;
            rollnos = rollno;
            if (rollnos != "")
            {
                sql = "select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No in ('" + rollnos + "') ;";
                studgradeds.Clear();
                studgradeds = d2.select_method_wo_parameter(sql, "text");
                if (studgradeds.Tables[0].Rows.Count > 0)
                {

                    for (int roll = 0; roll < studgradeds.Tables[0].Rows.Count; roll++)
                    {
                        string rcrollno = string.Empty;
                        rcrollno = Convert.ToString(studgradeds.Tables[0].Rows[roll][1]);
                        bindstudentdetails(rcrollno);
                        //bindbutn(rcrollno);
                        DataSet ds = new DataSet();
                        DataSet dschool = new DataSet();
                        DAccess2 da = new DAccess2();
                        DataSet dset = new DataSet();
                        string college_code = Convert.ToString(collegecode);
                        string stdappno = string.Empty;
                        System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
                        System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                        System.Drawing.Font Fontbold1 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
                        System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
                        System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                        System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
                        System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
                        System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
                        System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);



                        string Roll_No = rcrollno;
                        sql = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile,parentF_Mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No='" + Roll_No + "'";
                        ds.Clear();
                        ds.Dispose();
                        ds = da.select_method_wo_parameter(sql, "Text");
                        DataView dv = new DataView();
                        ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                        dv = ds.Tables[1].DefaultView;
                        int count4 = 0;
                        count4 = dv.Count;

                        if (ds.Tables[0].Rows.Count > 0 && count4 > 0)
                        {
                            string currentsem = Convert.ToString(dv[0]["Current_Semester"]);
                            string batchyear = Convert.ToString(dv[0]["Batch_Year"]);
                            string degreecode = Convert.ToString(dv[0]["degree_code"]);
                            stdappno = Convert.ToString(dv[0]["App_No"]);
                            string allsem = "1";
                            string admdate = Convert.ToString(dv[0]["adm_date"]);
                            string section = Convert.ToString(dv[0]["Sections"]);
                            string stdcc = string.Empty;
                            stdcc = Convert.ToString(ds.Tables[1].Rows[0]["Dept_Name"]);
                            string lblclassq = "CLASS - " + stdcc;

                            if (Convert.ToInt32(currentsem) > 1)
                            {
                                int term = Convert.ToInt32(currentsem);
                                for (int i = 2; i <= term; i++)
                                {
                                    allsem = allsem + "'" + "," + "'" + i;
                                }
                            }

                            if (Convert.ToInt32(currentsem) >= 1)
                            {
                                int term = Convert.ToInt32(currentsem);
                                for (int i = 1; i <= term; i++)
                                {
                                    string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + i + "' and degree_code='" + degreecode + "' and batch_year='" + batchyear + "'";
                                    dset = da.select_method_wo_parameter(sem, "Text");

                                    if (dset.Tables[0].Rows.Count > 0)
                                    {
                                        string startdate = Convert.ToString(dset.Tables[0].Rows[0]["start_date"]);
                                        string enddate = Convert.ToString(dset.Tables[0].Rows[0]["end_date"]);
                                        persentmonthcal(Roll_No, admdate, startdate, enddate);
                                    }

                                    if (i == 1)
                                    {
                                        lbltot_att1 = Convert.ToString(pre_present_date);
                                        lbltot_work1 = Convert.ToString(per_workingdays);
                                        working1 = Convert.ToString(pre_present_date);
                                        present1 = Convert.ToString(per_workingdays);
                                    }
                                }
                            }

                            string table1sql = "select * from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'  and semester = '" + currentsem + "' ; SELECT COUNT(*) as facount FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "'); SELECT Istype,CRITERIA_NO,y.semester FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "') ; SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "')";
                            DataSet ds1fortable1 = new DataSet();
                            ds1fortable1.Clear();
                            ds1fortable1.Dispose();
                            ds1fortable1 = da.select_method_wo_parameter(table1sql, "Text");
                            DataView dvforpage2 = new DataView();

                            string dob = Convert.ToString(dv[0]["dob"]);
                            string[] dobspit = dob.Split('/');
                            string addressline1 = Convert.ToString(dv[0]["parent_addressP"]);
                            addressline1 = addressline1 + " " + Convert.ToString(dv[0]["Streetp"]);
                            string addressline2 = Convert.ToString(dv[0]["Cityp"]);
                            addressline2 = addressline2 + " - " + Convert.ToString(dv[0]["parent_pincodep"]);
                            string mobileno = Convert.ToString(dv[0]["parentF_Mobile"]);

                            mypdfpage = mydoc.NewPage();
                            mypdfpage1 = mydoc.NewPage();
                            mypdfpage2 = mydoc.NewPage();
                            mypdfpage6 = mydoc.NewPage();

                            PdfTextArea pdf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(ds.Tables[0].Rows[0][0]) + "");
                            PdfTextArea pdf11 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(ds.Tables[0].Rows[0][1]) + "");
                            string address = Convert.ToString(ds.Tables[0].Rows[0][2]) + "" + Convert.ToString(ds.Tables[0].Rows[0][3]) + "" + Convert.ToString(ds.Tables[0].Rows[0][4]);
                            PdfTextArea pdf12 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 55, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
                            PdfTextArea pdf172 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 70, 595, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  :" + Convert.ToString(ds.Tables[0].Rows[0][6]));

                            PdfArea pa1 = new PdfArea(mydoc, 14, 12, 564, 821);

                            PdfArea pahealth = new PdfArea(mydoc, 20, 215, 550, 100);
                            PdfRectangle pr1 = new PdfRectangle(mydoc, pa1, Color.Black);
                            PdfArea pa2 = new PdfArea(mydoc, 230, 175, 150, 40);
                            PdfRectangle pr2 = new PdfRectangle(mydoc, pa2, Color.Black);
                            mypdfpage.Add(pr1);
                            mypdfpage.Add(pr2);


                            string sqlschool = "select value from Master_Settings where settings='Academic year'";
                            dschool = da.select_method_wo_parameter(sqlschool, "Text");
                            string splitvalue = Convert.ToString(dschool.Tables[0].Rows[0]["value"]);
                            string[] dsplit = splitvalue.Split(',');

                            string fvalue = Convert.ToString(dsplit[0]);
                            string lvalue = Convert.ToString(dsplit[1]);
                            string acdmic_date = fvalue + "-" + lvalue;


                            PdfTextArea pdf13 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 240, 190, 204, 30), System.Drawing.ContentAlignment.TopLeft, "REPORT BOOK");
                            PdfTextArea pdf14 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 210, 595, 50), System.Drawing.ContentAlignment.MiddleCenter, "CLASS VI to VIII");
                            PdfTextArea pdf16 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 230, 595, 50), System.Drawing.ContentAlignment.TopCenter, "__________________");
                            PdfTextArea pdf15 = new PdfTextArea(Fontmedium1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 255, 595, 50), System.Drawing.ContentAlignment.TopCenter, "(" + "Academic Year :" + acdmic_date + ")");
                            PdfTextArea pdf17 = new PdfTextArea(Fontmedium1, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 290, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Student Profile");
                            mypdfpage.Add(pdf15);
                            mypdfpage.Add(pdf16);

                            PdfTextArea pdf18 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 330, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Name of the Student");
                            PdfTextArea pdf19 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 150, 330, 595, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                            PdfTextArea pdf110a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 330, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["stud_name"]) + "");
                            PdfTextArea pdf110 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 330, 595, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________________________________");


                            PdfTextArea pdf128 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 360, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Class & Sec");
                            PdfTextArea pdf129 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 150, 360, 595, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                            PdfTextArea pdf1102a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 360, 595, 50), System.Drawing.ContentAlignment.TopLeft, stdcc + "  " + section);
                            PdfTextArea pdf1120 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 360, 595, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________________________________");
                            mypdfpage.Add(pdf128);
                            mypdfpage.Add(pdf129);
                            mypdfpage.Add(pdf1102a);
                            mypdfpage.Add(pdf1120);

                            PdfTextArea pdf111 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 390, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Admission No.");
                            PdfTextArea pdf112 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 150, 390, 595, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                            PdfTextArea pdf113 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 390, 595, 50), System.Drawing.ContentAlignment.TopLeft, "___________________________");
                            PdfTextArea pdf113a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 390, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["roll_admit"]) + "");
                            PdfTextArea pdf114 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 380, 390, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Exam No.");
                            PdfTextArea pdf115 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 440, 390, 595, 50), System.Drawing.ContentAlignment.TopLeft, "    " + Convert.ToString(dv[0]["Reg_No"]) + "");
                            PdfTextArea pdf000 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 460, 390, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_____________");
                            mypdfpage.Add(pdf000);
                            mypdfpage.Add(pdf110a);
                            mypdfpage.Add(pdf111);
                            mypdfpage.Add(pdf112);
                            mypdfpage.Add(pdf113);
                            mypdfpage.Add(pdf113a);
                            mypdfpage.Add(pdf114);
                            mypdfpage.Add(pdf115);
                            mypdfpage.Add(pdf172);

                            PdfTextArea pdf116 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 420, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Registration No.");
                            PdfTextArea pdf117 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 150, 420, 595, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                            PdfTextArea pdf118 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 420, 595, 50), System.Drawing.ContentAlignment.TopLeft, "___________________________");
                            PdfTextArea pdf118a1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 420, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["Reg_No"]) + "");
                            PdfTextArea pdf118b1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 380, 420, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Roll No." + "       " + Convert.ToString(dv[0]["Roll_No"]));
                            PdfTextArea pdf118c1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 440, 420, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_______________");
                            mypdfpage.Add(pdf116);
                            mypdfpage.Add(pdf117);
                            mypdfpage.Add(pdf118);
                            mypdfpage.Add(pdf118a1);
                            mypdfpage.Add(pdf118b1);
                            mypdfpage.Add(pdf118c1);

                            PdfTextArea pdf119 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 450, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Birth");
                            PdfTextArea pdf120 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 150, 450, 595, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                            PdfTextArea pdf121 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 450, 595, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dv[0]["dob"]));
                            PdfTextArea pdf12123 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 450, 595, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________________________________");
                            mypdfpage.Add(pdf12123);
                            mypdfpage.Add(pdf119);
                            mypdfpage.Add(pdf120);
                            mypdfpage.Add(pdf121);

                            PdfTextArea pdf122 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 480, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Mothers Name");
                            PdfTextArea pdf123 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 150, 480, 595, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                            PdfTextArea pdf124 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 480, 595, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________________________________");
                            PdfTextArea pdf124a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 480, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["mother"]) + "");
                            mypdfpage.Add(pdf122);
                            mypdfpage.Add(pdf123);
                            mypdfpage.Add(pdf124);
                            mypdfpage.Add(pdf124a);

                            PdfTextArea pdf125 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 510, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Fathers Name");
                            PdfTextArea pdf126 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 150, 510, 595, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                            PdfTextArea pdf127 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 510, 595, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________________________________");
                            PdfTextArea pdf127a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 510, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["parent_name"]) + "");
                            mypdfpage.Add(pdf125);
                            mypdfpage.Add(pdf126);
                            mypdfpage.Add(pdf127);
                            mypdfpage.Add(pdf127a);

                            PdfTextArea pdf1281 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 540, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Residential Address");
                            PdfTextArea pdf1291 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 150, 540, 595, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                            PdfTextArea pdf130 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 540, 595, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________________________________");
                            PdfTextArea pdf130a = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 540, 400, 50), System.Drawing.ContentAlignment.TopLeft, "" + addressline1 + " , " + addressline2 + "");
                            mypdfpage.Add(pdf1281);
                            mypdfpage.Add(pdf1291);
                            mypdfpage.Add(pdf130);
                            mypdfpage.Add(pdf130a);

                            PdfTextArea pdf131 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 570, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Telephone No.");
                            PdfTextArea pdf132 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 150, 570, 595, 50), System.Drawing.ContentAlignment.TopLeft, "");
                            PdfTextArea pdf133 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 570, 595, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________________________________");
                            PdfTextArea pdf133a = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 570, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + mobileno + "");
                            //PdfTextArea pdf134 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 570, 595, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________________________________");
                            //PdfTextArea pdf134a = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 570, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" +  + "");
                            mypdfpage.Add(pdf131);
                            mypdfpage.Add(pdf132);
                            mypdfpage.Add(pdf133);
                            mypdfpage.Add(pdf133a);
                            //mypdfpage.Add(pdf134);
                            //mypdfpage.Add(pdf134a);
                            PdfTextArea pdf135 = new PdfTextArea(Fontmedium1, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 620, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Attendance:");
                            mypdfpage.Add(pdf135);
                            PdfTextArea pdf136 = new PdfTextArea(Fontmedium1, System.Drawing.Color.Black, new PdfArea(mydoc, 340, 620, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Term I ");
                            mypdfpage.Add(pdf136);
                            PdfTextArea pdf137 = new PdfTextArea(Fontmedium1, System.Drawing.Color.Black, new PdfArea(mydoc, 450, 620, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Term II ");
                            mypdfpage.Add(pdf137);
                            PdfTextArea pdf138 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 670, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Total attendance of the Student");
                            PdfTextArea pdf139 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 300, 670, 595, 50), System.Drawing.ContentAlignment.TopLeft, "             " + lbltot_att1 + "");
                            PdfTextArea pdf140 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 440, 670, 595, 50), System.Drawing.ContentAlignment.TopLeft, "       " + lbltot_att2 + "");
                            mypdfpage.Add(pdf138);
                            mypdfpage.Add(pdf140);
                            mypdfpage.Add(pdf139);

                            PdfTextArea pdf141 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 700, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Total working days");
                            PdfTextArea pdf142 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 300, 700, 595, 50), System.Drawing.ContentAlignment.TopLeft, "             " + lbltot_work1 + "");
                            PdfTextArea pdf143 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 440, 700, 595, 50), System.Drawing.ContentAlignment.TopLeft, "       " + lbltot_work2 + "");
                            mypdfpage.Add(pdf141);
                            mypdfpage.Add(pdf142);
                            mypdfpage.Add(pdf143);
                            PdfTextArea pdf144 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 80, 790, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Signature");
                            mypdfpage.Add(pdf144);
                            PdfTextArea pdf145 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 170, 790, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Student");
                            mypdfpage.Add(pdf145);
                            PdfTextArea pdf146 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 260, 790, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Class Teacher");
                            mypdfpage.Add(pdf146);
                            PdfTextArea pdf147 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 380, 790, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Principal");
                            mypdfpage.Add(pdf147);
                            PdfTextArea pdf148 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 480, 790, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Parent");
                            mypdfpage.Add(pdf148);


                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 50, 96, 450);
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                mypdfpage.Add(LogoImage1, 280, 96, 450);
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"));
                                mypdfpage.Add(LogoImage2, 450, 190, 450);
                            }
                            else
                            {
                                Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                mypdfpage.Add(LogoImage2, 450, 96, 270);
                            }

                            Hashtable hatsubject = new Hashtable();
                            Hashtable hatcriter = new Hashtable();


                            //////////////////////////////////////////////////////////////////page 2/////////////////////


                            int rowcountspread = fpspread.Sheets[0].RowCount + 3;
                            int columncountspread = fpspread.Sheets[0].ColumnCount;

                            Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(Fontsmall1, rowcountspread, columncountspread, 1);
                            table1forpage2.VisibleHeaders = false;
                            table1forpage2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table1forpage2.Columns[1].SetWidth(25);
                            table1forpage2.Columns[0].SetWidth(8);

                            int ss = fpspread.Sheets[0].ColumnHeader.RowCount;

                            for (int i = 0; i < 3; i++)
                            {
                                for (int j = 0; j < columncountspread; j++)
                                {
                                    string coldata = fpspread.Sheets[0].ColumnHeader.Cells[i, j].Text;
                                    table1forpage2.Cell(i, j).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2.Cell(i, j).SetContent(coldata);
                                    if (i == 0)
                                    {
                                        string spannote = fpspread.Sheets[0].ColumnHeader.Cells[i, j].Note;
                                        if (spannote.Trim() != "")
                                        {
                                            string[] splitspannote = spannote.Split('-');
                                            if (splitspannote.GetUpperBound(0) == 1)
                                            {
                                                string colno = Convert.ToString(splitspannote[0]);
                                                string colspancount = Convert.ToString(splitspannote[1]);

                                                int startcol = Convert.ToInt32(colno);
                                                int endcol = Convert.ToInt32(colspancount);
                                                int noofrow = endcol - startcol;

                                                foreach (PdfCell pr in table1forpage2.CellRange(0, startcol, 0, startcol).Cells)
                                                {
                                                    pr.ColSpan = endcol;
                                                }
                                            }
                                        }
                                    }
                                }
                            }



                            foreach (PdfCell pr in table1forpage2.CellRange(0, 0, 0, 0).Cells)
                            {
                                pr.RowSpan = 3;
                            }
                            foreach (PdfCell pr in table1forpage2.CellRange(0, 1, 0, 1).Cells)
                            {
                                pr.RowSpan = 3;
                            }

                            for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                            {
                                for (int j = 0; j < columncountspread; j++)
                                {
                                    string coldata = fpspread.Sheets[0].Cells[i, j].Text;

                                    table1forpage2.Cell(i + 3, j).SetContentAlignment(ContentAlignment.MiddleCenter);

                                    table1forpage2.Cell(i + 3, j).SetContent(coldata);
                                }
                            }

                            table1forpage2.Columns[1].SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                            table1forpage2.Columns[0].SetWidth(7);
                            //table1forpage2.Columns[1].SetWidth(13);

                            table1forpage2.Columns[fpspread.Sheets[0].ColumnCount - 1].SetWidth(15);
                            table1forpage2.Columns[fpspread.Sheets[0].ColumnCount - 2].SetWidth(15);
                            //table1forpage2.Columns[2].SetWidth(70);

                            foreach (PdfCell rr in table1forpage2.Cells)
                            {


                                rr.SetCellPadding(8);

                            }
                            Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 85, 553, 600));
                            mypdfpage1.Add(newpdftabpage2);

                            PdfRectangle pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
                            mypdfpage1.Add(pr3);
                            PdfTextArea pdf21 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 40, 40, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Name ");
                            mypdfpage1.Add(pdf21);
                            PdfTextArea pdf22 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 80, 40, 595, 50), System.Drawing.ContentAlignment.TopLeft, "___________________________");
                            mypdfpage1.Add(pdf22);

                            PdfTextArea pdf22a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 37, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["stud_name"]) + "");
                            mypdfpage1.Add(pdf22a);

                            PdfTextArea pdf23 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 320, 40, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Class & Sec ");
                            mypdfpage1.Add(pdf23);
                            PdfTextArea pdf24 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 425, 40, 595, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dv[0]["Dept_Name"]) + " " + Convert.ToString(dv[0]["Sections"]));
                            mypdfpage1.Add(pdf24);
                            pdf24 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 400, 40, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_____________________");
                            mypdfpage1.Add(pdf24);

                            string partone = d2.GetFunction("select  TextVal as part1 from CoCurr_Activitie ca,textvaltable tv where ca.Title_Name=tv.TextCode and TextCriteria = 'RTnam' and ca.Degree_Code='" + degreecode + "' and Batch_Year='" + batchyear + "' and SubTitle='1a'");
                            PdfTextArea pdf27 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 35, 62, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Part - I - Academic Performance " + " : " + partone + "   (9 Point Scale)");
                            mypdfpage1.Add(pdf27);

                            Double getheigh = newpdftabpage2.Area.Height;
                            getheigh = Math.Round(getheigh, 2);
                            double page2col = getheigh + 110;
                            if (dropterm.SelectedItem.Text == "2")
                            {
                                PdfTextArea pdf28 = new PdfTextArea(Fontsmall9, System.Drawing.Color.Black, new PdfArea(mydoc, 440, page2col - 10, 595, 50), System.Drawing.ContentAlignment.TopLeft, "CGPA :");
                                mypdfpage1.Add(pdf28);

                                PdfTextArea pdf28r = new PdfTextArea(Fontsmall9, System.Drawing.Color.Black, new PdfArea(mydoc, 440, page2col, 595, 50), System.Drawing.ContentAlignment.TopLeft, "(Avg Grade Point");
                                mypdfpage1.Add(pdf28r);
                            }

                            double credittotal = 0;
                            double grandtotcredit = 0;
                            double grandtotcreditfull = 0;

                            string maxmain = "SELECT MAX(Credit_Points) from Grade_Master where Semester='" + currentsem + "' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria in ('',null)";
                            ds.Clear();
                            string maxgd = string.Empty;
                            ds = da.select_method_wo_parameter(maxmain, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string check_maxgd = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                maxgd = Convert.ToString(ds.Tables[0].Rows[0][0]);
                            }

                            if (maxgd.Trim() == "")
                            {
                                maxmain = "SELECT MAX(Credit_Points) from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria in ('',null)";
                                ds.Clear();
                                ds = da.select_method_wo_parameter(maxmain, "text");
                            }
                            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows.Count != null)
                            {
                                grandtotcredit = Convert.ToDouble(Convert.ToString(ds.Tables[0].Rows[0][0]));
                                grandtotcredit = grandtotcredit * fpspread.Sheets[0].RowCount;
                            }
                            if (dropterm.SelectedItem.Text.Trim() != "1")
                            {
                                for (int i = fpspread.Sheets[0].ColumnCount - 1; i < fpspread.Sheets[0].ColumnCount; i++)
                                {
                                    for (int j = 0; j < fpspread.Sheets[0].RowCount; j++)
                                    {
                                        if (Convert.ToString(fpspread.Sheets[0].Cells[j, i].Text).Trim() != "")
                                        {
                                            credittotal = credittotal + Convert.ToDouble(Convert.ToString(fpspread.Sheets[0].Cells[j, i].Text));
                                        }
                                    }
                                }
                            }

                            grandtotcreditfull = (credittotal / grandtotcredit);
                            grandtotcredit = Math.Round(grandtotcreditfull, 2);

                            //cgpa = da.Calculete_CGPA(Roll_No, currentsem, degreecode, batchyear, latmode, college_code);
                            string cgpapdf = Convert.ToString(grandtotcredit);
                            //}
                            if (dropterm.SelectedItem.Text == "2")
                            {
                                PdfTextArea pdf28a1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 500, page2col - 35, 595, 50), System.Drawing.ContentAlignment.TopLeft, "________");
                                mypdfpage1.Add(pdf28a1);

                                PdfTextArea pdf28a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 520, page2col - 5, 595, 50), System.Drawing.ContentAlignment.TopLeft, cgpapdf);
                                mypdfpage1.Add(pdf28a);

                                PdfTextArea pdf28a2 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 500, page2col, 595, 50), System.Drawing.ContentAlignment.TopLeft, "________");
                                mypdfpage1.Add(pdf28a2);

                                PdfArea pa5 = new PdfArea(mydoc, 100, 450, 250, 40);
                                PdfRectangle pr5 = new PdfRectangle(mydoc, pa5, Color.Black);
                                mypdfpage1.Add(pr5);

                                page2col = page2col + 30;
                                PdfArea overallgradepa1 = new PdfArea(mydoc, 70, page2col, 220, 28);
                                PdfRectangle overallgradepa1pr3 = new PdfRectangle(mydoc, overallgradepa1, Color.Black);

                                grandtotcredit = grandtotcreditfull * 100;


                                PdfTextArea pdd1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 250, 466, 595, 50), System.Drawing.ContentAlignment.TopLeft, lblgradeval.Text);
                                mypdfpage1.Add(pdd1);

                                PdfTextArea pdd = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 105, 466, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Overall Grade :" + " _______________");
                                mypdfpage1.Add(pdd);

                            }
                            string totmain = "SELECT * from Grade_Master where Semester='" + currentsem + "' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria = '' ";
                            ds.Clear();
                            ds = da.select_method_wo_parameter(totmain, "text");
                            double totmaincheckmarkmm = grandtotcredit;
                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                totmain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria =''";
                                ds.Clear();
                                ds = da.select_method_wo_parameter(totmain, "text");
                            }
                            for (int grd = 0; grd < ds.Tables[0].Rows.Count; grd++)
                            {
                                if (Convert.ToInt32(ds.Tables[0].Rows[grd][1]) <= totmaincheckmarkmm && Convert.ToInt32(ds.Tables[0].Rows[grd][2]) >= totmaincheckmarkmm)
                                {
                                    //lblgradeval.Text = ds.Tables[0].Rows[grd][0].ToString(); sri

                                }
                            }


                            page2col = page2col + 5;
                            //if (chkoverallgradeall.Checked == true)  sri
                            //{
                            //    mypdfpage1.Add(overallgradepa1pr3);
                            //    string gradeoveraall = lblgradeval.Text;
                            //    pdf28 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 80, page2col, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Overall Grade :           " + gradeoveraall + "");
                            //    mypdfpage1.Add(pdf28);
                            //    pdf28 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 180, page2col, 595, 50), System.Drawing.ContentAlignment.TopLeft, "____________");
                            //    mypdfpage1.Add(pdf28);
                            //}

                            page2col = page2col + 40;
                            string strqueryytable = "select  ca.Title_Name,tv.TextCode, tv.TextVal,ca.SubTitle from activity_entry ae,CoCurr_Activitie ca,textvaltable tv where ae.CoCurr_ID=ca.CoCurr_ID and ae.Batch_Year=ca.Batch_Year and ae.Degree_Code=ca.Degree_Code and ae.Batch_Year='" + batchyear + "' and ae.Degree_Code='" + degreecode + "' and tv.TextCode=ae.ActivityTextVal  order by SubTitle";
                            DataTable dttablevalue1 = da.select_method_wop_table(strqueryytable, "Text");
                            dttablevalue1.DefaultView.RowFilter = "SubTitle='2A'";
                            DataView dvskactv1 = dttablevalue1.DefaultView;
                            string allactivity = string.Empty;
                            string partnametitlle = string.Empty;
                            for (int i = 0; i < dvskactv1.Count; i++)
                            {

                                if (allactivity == "")
                                {
                                    allactivity = Convert.ToString(dvskactv1[i]["TextCode"]);
                                }
                                else
                                {
                                    allactivity = allactivity + "','" + Convert.ToString(dvskactv1[i]["TextCode"]);
                                }
                                partnametitlle = Convert.ToString(dvskactv1[i]["Title_Name"]);
                            }


                            strqueryytable = "select tv.TextVal,ag.Grade,ag.description,cd.Mark from activity_gd ag ,CoCurrActivitie_Det cd,textvaltable tv where ag.ActivityTextVal=cd.ActivityTextVal and tv.TextCode=ag.ActivityTextVal and tv.TextCode=cd.ActivityTextVal and  cd.ActivityTextVal in ('" + allactivity + "') and cd.Roll_No='" + Roll_No + "' and cd.Mark between frompoint and topoint  ";

                            DataTable strqueryytablefil = da.select_method_wop_table(strqueryytable, "Text");
                            DataView dvsk = strqueryytablefil.DefaultView;

                            if (dropterm.SelectedItem.Text == "2")
                            {

                                if (dvsk.Count > 0)
                                {
                                    partnametitlle = da.GetFunction("select upper(SubTitle)+' : '+textval from textvaltable tv,CoCurr_Activitie ca  where ca.Title_Name=tv.TextCode and ca.Title_Name='" + partnametitlle + "'");

                                    PdfTextArea pdefff = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 580, 585, 50), System.Drawing.ContentAlignment.TopCenter, "Part - II" + "-" + " " + "Co-Scholastic Areas");
                                    mypdfpage1.Add(pdefff);
                                    PdfTextArea pdf210 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 660, 620, 50), System.Drawing.ContentAlignment.TopLeft, partnametitlle);
                                    mypdfpage1.Add(pdf210);
                                    PdfTextArea pdf29 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 600, 595, 50), System.Drawing.ContentAlignment.TopCenter, "(To be assessed on 5 Point Scale once in a Session)");
                                    mypdfpage1.Add(pdf29);



                                    DataTable pdfbind = new DataTable();
                                    Gios.Pdf.PdfTable table = mydoc.NewTable(Fontmedium, 2, 3, 1);
                                    table.VisibleHeaders = false;
                                    table.SetRowHeight(10);
                                    table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(0, 0).SetContent("S.No.");


                                    table.Columns[0].SetWidth(30);
                                    table.Columns[1].SetWidth(200);
                                    table.Columns[2].SetWidth(50);

                                    table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(0, 1).SetContent("Descriptive Indicators *");
                                    table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(0, 2).SetContent("Grade");
                                    for (int snda = 0; snda < 1; snda++)
                                    {
                                        if (snda == 0)
                                        {
                                            table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(1, 0).SetContent(snda + 1);

                                            table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table.Cell(1, 1).SetContent(Convert.ToString(dvsk[snda]["description"]));

                                            table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(1, 2).SetContent(Convert.ToString(dvsk[snda]["Grade"]));
                                        }
                                    }

                                    foreach (PdfCell rr in table.Cells)
                                        rr.SetCellPadding(8);
                                    //foreach (PdfRow pr in table.Rows) pr.SetRowHeight(50);
                                    Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 680, 550, 300));
                                    mypdfpage1.Add(newpdftabpage);
                                }
                                else
                                {

                                    PdfTextArea pdf210 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 40, 660, 620, 50), System.Drawing.ContentAlignment.TopLeft, "");
                                    mypdfpage1.Add(pdf210);
                                    PdfTextArea pdf211 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 250, 660, 620, 50), System.Drawing.ContentAlignment.TopLeft, "");
                                    mypdfpage1.Add(pdf211);

                                    Gios.Pdf.PdfTable table = mydoc.NewTable(Fontmedium, 2, 3, 1);
                                    table.Columns[0].SetWidth(30);
                                    table.Columns[1].SetWidth(200);
                                    table.Columns[2].SetWidth(50);
                                    table.VisibleHeaders = false;
                                    table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(0, 0).SetContent("S.No.");
                                    table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(0, 1).SetContent(" Descriptive Indicators *      ");
                                    table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(0, 2).SetContent(" Grade     ");

                                    table.Cell(1, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                    table.Cell(1, 0).SetContent("");
                                    table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(1, 1).SetContent("");
                                    table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(1, 2).SetContent("");

                                    table.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table.Cell(2, 0).SetContent("");
                                    table.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table.Cell(2, 1).SetContent("");
                                    table.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table.Cell(2, 2).SetContent("");

                                    // foreach (PdfRow pr in table.Rows) pr.SetRowHeight(50);
                                    foreach (PdfCell rr in table.Cells)
                                        rr.SetCellPadding(8);
                                    Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 680, 550, 600));

                                    mypdfpage1.Add(newpdftabpage);
                                }
                            }
                            /////////////////////////newadded 2nd page/////////////////////////
                            mypdfpagefinal = mydoc.NewPage();
                            mypdfpagefinal.Add(pr3);
                            if (dvsk.Count > 1)
                            {
                                //PdfTextArea pdf210 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 40, 20, 620, 50), System.Drawing.ContentAlignment.TopLeft, dvsk[0]["partname"].ToString() + " " + dvsk[0]["tTextVal"].ToString());
                                //mypdfpagefinal.Add(pdf210);

                                DataTable pdfbind = new DataTable();
                                Gios.Pdf.PdfTable table = mydoc.NewTable(Fontmedium, dvsk.Count, 3, 1);
                                table.SetRowHeight(10);
                                table.Columns[0].SetWidth(30);
                                table.Columns[1].SetWidth(200);
                                table.Columns[2].SetWidth(50);
                                table.VisibleHeaders = false;
                                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 0).SetContent("S.No.");
                                table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 1).SetContent(" Descriptive Indicators *      ");
                                table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 2).SetContent(" Grade     ");
                                int d = 0;
                                for (int snda = 1; snda < dvsk.Count; snda++)
                                {
                                    if (snda >= 1)
                                    {
                                        table.Cell(d, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table.Cell(d, 0).SetContent(snda + 1);
                                        table.Cell(d, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table.Cell(d, 1).SetContent(Convert.ToString(dvsk[snda]["description"]));
                                        table.Cell(d, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table.Cell(d, 2).SetContent(Convert.ToString(dvsk[snda]["Grade"]));
                                        d++;
                                    }

                                }
                                foreach (PdfCell rr in table.Cells)
                                    rr.SetCellPadding(8);
                                Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 20, 550, 300));
                                mypdfpagefinal.Add(newpdftabpage);
                            }
                            else
                            {

                                PdfTextArea pdf210 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 40, 660, 620, 50), System.Drawing.ContentAlignment.TopLeft, "");
                                mypdfpagefinal.Add(pdf210);
                                PdfTextArea pdf211 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 250, 660, 620, 50), System.Drawing.ContentAlignment.TopLeft, "");
                                mypdfpagefinal.Add(pdf211);

                                Gios.Pdf.PdfTable table = mydoc.NewTable(Fontmedium, 3, 3, 1);
                                table.Columns[0].SetWidth(30);
                                table.Columns[1].SetWidth(200);
                                table.Columns[2].SetWidth(50);
                                table.VisibleHeaders = false;
                                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 0).SetContent("S.No.");
                                table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 1).SetContent(" Descriptive Indicators *      ");
                                table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 2).SetContent(" Grade     ");

                                table.Cell(1, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                table.Cell(1, 0).SetContent("");
                                table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 1).SetContent("");
                                table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 2).SetContent("");

                                table.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(2, 0).SetContent("");
                                table.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(2, 1).SetContent("");
                                table.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(2, 2).SetContent("");

                                // foreach (PdfRow pr in table.Rows) pr.SetRowHeight(50);
                                foreach (PdfCell rr in table.Cells)
                                    rr.SetCellPadding(10);
                                table.Cell(0, 0).SetCellPadding(2);
                                table.Cell(0, 1).SetCellPadding(2);
                                table.Cell(0, 2).SetCellPadding(2);

                                Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 20, 550, 300));
                                mypdfpagefinal.Add(newpdftabpage);
                                mypdfpagefinal.Add(pr3);
                            }
                            mypdfpage.Add(pdf1);
                            mypdfpage.Add(pdf11);
                            mypdfpage.Add(pdf12);
                            mypdfpage.Add(pdf13);
                            mypdfpage.Add(pdf14);

                            mypdfpage.Add(pdf17);
                            mypdfpage.Add(pdf18);
                            mypdfpage.Add(pdf19);
                            mypdfpage.Add(pdf110);


                            PdfTextArea pddf29 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 800, 595, 50), System.Drawing.ContentAlignment.TopLeft, "*Descriptive Indicators are statements used to describe each learner.");
                            mypdfpagefinal.Add(pddf29);
                            ////////////////////////page 3/////////////////////

                            mypdfpage2.Add(pr3);

                            DataTable dtgrid1 = new DataTable();
                            dttablevalue1.DefaultView.RowFilter = "SubTitle='2B'";

                            dvskactv1 = dttablevalue1.DefaultView;
                            allactivity = string.Empty;
                            partnametitlle = string.Empty;
                            for (int i = 0; i < dvskactv1.Count; i++)
                            {

                                if (allactivity == "")
                                {
                                    allactivity = Convert.ToString(dvskactv1[i]["TextCode"]);
                                }
                                else
                                {
                                    allactivity = allactivity + "','" + Convert.ToString(dvskactv1[i]["TextCode"]);
                                }
                                partnametitlle = Convert.ToString(dvskactv1[i]["Title_Name"]);
                            }


                            strqueryytable = "select tv.TextVal,ag.Grade,ag.description,cd.Mark from activity_gd ag ,CoCurrActivitie_Det cd,textvaltable tv where ag.ActivityTextVal=cd.ActivityTextVal and tv.TextCode=ag.ActivityTextVal and tv.TextCode=cd.ActivityTextVal and  cd.ActivityTextVal in ('" + allactivity + "') and cd.Roll_No='" + Roll_No + "' and cd.Mark between frompoint and topoint  ";

                            strqueryytablefil = da.select_method_wop_table(strqueryytable, "Text");


                            DataView dvsk1 = strqueryytablefil.DefaultView;
                            Gios.Pdf.PdfTable tablepage3a = mydoc.NewTable(Fontmedium, 2, 2, 2);
                            tablepage3a.VisibleHeaders = false;

                            if (dvsk1.Count > 0)
                            {
                                partnametitlle = da.GetFunction("select upper(SubTitle)+' : '+textval from textvaltable tv,CoCurr_Activitie ca  where ca.Title_Name=tv.TextCode and ca.Title_Name='" + partnametitlle + "'");

                                PdfTextArea pdf31 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 30, 20, 595, 200), System.Drawing.ContentAlignment.TopLeft, partnametitlle);
                                mypdfpage2.Add(pdf31);

                                tablepage3a.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                tablepage3a.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage3a.Cell(0, 0).SetContent("   Descriptive Indicators*  ");
                                tablepage3a.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage3a.Cell(0, 1).SetContent("  Grade   ");


                                tablepage3a.Columns[0].SetWidth(150);
                                tablepage3a.Columns[1].SetWidth(30);

                                for (int snda = 0; snda < 1; snda++)
                                {
                                    if (dvsk1.Count > snda)
                                    {
                                        tablepage3a.Cell(snda + 1, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                        tablepage3a.Cell(snda + 1, 0).SetContent(Convert.ToString(dvsk1[snda]["description"]));
                                        tablepage3a.Cell(snda + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tablepage3a.Cell(snda + 1, 1).SetContent(Convert.ToString(dvsk1[snda]["Grade"]));
                                    }
                                }
                            }

                            else
                            {

                                tablepage3a.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                tablepage3a.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                tablepage3a.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage3a.Cell(0, 0).SetContent("   Descriptive Indicators*  ");
                                tablepage3a.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage3a.Cell(0, 1).SetContent("  Grade   ");


                                tablepage3a.Columns[0].SetWidth(150);
                                tablepage3a.Columns[1].SetWidth(30);


                                tablepage3a.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tablepage3a.Cell(1, 0).SetContent("");
                                tablepage3a.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage3a.Cell(1, 1).SetContent("");
                            }

                            tablepage3a.VisibleHeaders = false;
                            foreach (PdfCell rr in tablepage3a.Cells)
                                rr.SetCellPadding(8);
                            Gios.Pdf.PdfTablePage newpdftabpage3 = tablepage3a.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 40, 550, 600));
                            mypdfpage2.Add(newpdftabpage3);


                            Gios.Pdf.PdfTable tablepage3b = mydoc.NewTable(Fontmedium, 2, 2, 2);
                            dttablevalue1.DefaultView.RowFilter = "SubTitle='2C'";
                            dvskactv1 = dttablevalue1.DefaultView;
                            allactivity = string.Empty;
                            partnametitlle = string.Empty;
                            for (int i = 0; i < dvskactv1.Count; i++)
                            {

                                if (allactivity == "")
                                {
                                    allactivity = Convert.ToString(dvskactv1[i]["TextCode"]);
                                }
                                else
                                {
                                    allactivity = allactivity + "','" + Convert.ToString(dvskactv1[i]["TextCode"]);
                                }
                                partnametitlle = Convert.ToString(dvskactv1[i]["Title_Name"]);
                            }


                            strqueryytable = "select tv.TextVal,ag.Grade,ag.description,cd.Mark from activity_gd ag ,CoCurrActivitie_Det cd,textvaltable tv where ag.ActivityTextVal=cd.ActivityTextVal and tv.TextCode=ag.ActivityTextVal and tv.TextCode=cd.ActivityTextVal and  cd.ActivityTextVal in ('" + allactivity + "') and cd.Roll_No='" + Roll_No + "' and cd.Mark between frompoint and topoint  ";

                            strqueryytablefil = da.select_method_wop_table(strqueryytable, "Text");
                            DataView dvsk2 = strqueryytablefil.DefaultView;
                            if (dvsk2.Count > 0)
                            {
                                int sno = 1;
                                partnametitlle = da.GetFunction("select upper(SubTitle)+' : '+textval from textvaltable tv,CoCurr_Activitie ca  where ca.Title_Name=tv.TextCode and ca.Title_Name='" + partnametitlle + "'");
                                PdfTextArea pdf32 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 30, 150, 595, 150), System.Drawing.ContentAlignment.TopLeft, partnametitlle);
                                mypdfpage2.Add(pdf32);

                                DataTable dtgrida = new DataTable();
                                tablepage3b.VisibleHeaders = false;
                                tablepage3b.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                tablepage3b.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage3b.Cell(0, 0).SetContent("   Descriptive Indicators*   ");
                                tablepage3b.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage3b.Cell(0, 1).SetContent("  Grade   ");

                                for (int snda = 0; snda < 1; snda++)
                                {
                                    tablepage3b.VisibleHeaders = false;

                                    if (dvsk2.Count > snda)
                                    {
                                        tablepage3b.Cell(snda + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tablepage3b.Cell(snda + 1, 0).SetContent(Convert.ToString(dvsk2[snda]["description"]));
                                        tablepage3b.Cell(snda + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tablepage3b.Cell(snda + 1, 1).SetContent(Convert.ToString(dvsk2[snda]["Grade"]));
                                    }
                                    tablepage3b.Columns[0].SetWidth(150);
                                    tablepage3b.Columns[1].SetWidth(30);

                                }
                            }
                            else
                            {
                                tablepage3b.VisibleHeaders = false;
                                tablepage3b.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                tablepage3b.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tablepage3b.Cell(0, 0).SetContent("   Descriptive Indicators*  ");
                                tablepage3b.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage3b.Cell(0, 1).SetContent("  Grade   ");

                                tablepage3b.VisibleHeaders = false;
                                tablepage3b.Cell(1, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                tablepage3b.Cell(1, 0).SetContent("");
                                tablepage3b.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage3b.Cell(1, 1).SetContent("");

                                tablepage3b.Columns[0].SetWidth(150);
                                tablepage3b.Columns[1].SetWidth(30);
                            }

                            foreach (PdfCell rr in tablepage3b.Cells)
                                rr.SetCellPadding(8);
                            Gios.Pdf.PdfTablePage newpdftabpage3b = tablepage3b.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 170, 550, 600));
                            mypdfpage2.Add(newpdftabpage3b);



                            Gios.Pdf.PdfTable tablepage3c = mydoc.NewTable(Fontmedium, 5, 2, 1);
                            dttablevalue1.DefaultView.RowFilter = "SubTitle='2D'";
                            dvskactv1 = dttablevalue1.DefaultView;
                            allactivity = string.Empty;
                            partnametitlle = string.Empty;
                            for (int i = 0; i < dvskactv1.Count; i++)
                            {

                                if (allactivity == "")
                                {
                                    allactivity = Convert.ToString(dvskactv1[i]["TextCode"]);
                                }
                                else
                                {
                                    allactivity = allactivity + "','" + Convert.ToString(dvskactv1[i]["TextCode"]);
                                }
                                partnametitlle = Convert.ToString(dvskactv1[i]["Title_Name"]);
                            }


                            strqueryytable = "select tv.TextVal,ag.Grade,ag.description,cd.Mark from activity_gd ag ,CoCurrActivitie_Det cd,textvaltable tv where ag.ActivityTextVal=cd.ActivityTextVal and tv.TextCode=ag.ActivityTextVal and tv.TextCode=cd.ActivityTextVal and  cd.ActivityTextVal in ('" + allactivity + "') and cd.Roll_No='" + Roll_No + "' and cd.Mark between frompoint and topoint  ";

                            strqueryytablefil = da.select_method_wop_table(strqueryytable, "Text");
                            DataView dvsk3 = strqueryytablefil.DefaultView;

                            if (dvsk3.Count > 0)
                            {
                                partnametitlle = da.GetFunction("select upper(SubTitle)+' : '+textval from textvaltable tv,CoCurr_Activitie ca  where ca.Title_Name=tv.TextCode and ca.Title_Name='" + partnametitlle + "'");
                                PdfTextArea pdf3882 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 30, 280, 595, 150), System.Drawing.ContentAlignment.TopLeft, partnametitlle);
                                mypdfpage2.Add(pdf3882);

                                tablepage3c.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                tablepage3c.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage3c.Cell(0, 0).SetContent("   Descriptive Indicators*  ");
                                tablepage3c.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablepage3c.Cell(0, 1).SetContent("  Grade   ");

                                for (int snda = 0; snda < 4; snda++)
                                {
                                    if (dvsk3.Count > snda)
                                    {
                                        DataTable dtgrid5 = new DataTable();
                                        tablepage3c.VisibleHeaders = false;


                                        tablepage3c.Cell(snda + 1, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                        tablepage3c.Cell(snda + 1, 0).SetContent(Convert.ToString(dvsk3[snda]["description"]));
                                        tablepage3c.Cell(snda + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tablepage3c.Cell(snda + 1, 1).SetContent(Convert.ToString(dvsk3[snda]["Grade"]));
                                    }
                                    else
                                    {
                                        tablepage3c.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                        tablepage3c.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tablepage3c.Cell(0, 0).SetContent("   Descriptive Indicators*  ");
                                        tablepage3c.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tablepage3c.Cell(0, 1).SetContent("  Grade   ");


                                        tablepage3c.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tablepage3c.Cell(1, 0).SetContent("");
                                        tablepage3c.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tablepage3c.Cell(1, 1).SetContent("");

                                    }
                                }
                            }
                            tablepage3c.Columns[0].SetWidth(150);
                            tablepage3c.Columns[1].SetWidth(30);

                            foreach (PdfCell rr in tablepage3c.Cells)
                                rr.SetCellPadding(10);
                            Gios.Pdf.PdfTablePage newpdftabpage3c = tablepage3c.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 300, 550, 600));
                            mypdfpage2.Add(newpdftabpage3c);

                            dttablevalue1.DefaultView.RowFilter = "SubTitle='3A'";
                            dvskactv1 = dttablevalue1.DefaultView;
                            allactivity = string.Empty;
                            partnametitlle = string.Empty;
                            for (int i = 0; i < dvskactv1.Count; i++)
                            {

                                if (allactivity == "")
                                {
                                    allactivity = Convert.ToString(dvskactv1[i]["TextCode"]);
                                }
                                else
                                {
                                    allactivity = allactivity + "','" + Convert.ToString(dvskactv1[i]["TextCode"]);
                                }
                                partnametitlle = Convert.ToString(dvskactv1[i]["Title_Name"]);
                            }


                            strqueryytable = "select tv.TextVal,ag.Grade,ag.description,cd.Mark from activity_gd ag ,CoCurrActivitie_Det cd,textvaltable tv where ag.ActivityTextVal=cd.ActivityTextVal and tv.TextCode=ag.ActivityTextVal and tv.TextCode=cd.ActivityTextVal and  cd.ActivityTextVal in ('" + allactivity + "') and cd.Roll_No='" + Roll_No + "' and cd.Mark between frompoint and topoint  ";

                            strqueryytablefil = da.select_method_wop_table(strqueryytable, "Text");
                            DataView dvsk4 = strqueryytablefil.DefaultView;
                            if (dvsk4.Count > 0)
                            {
                                partnametitlle = da.GetFunction("select upper(SubTitle)+' : '+textval from textvaltable tv,CoCurr_Activitie ca  where ca.Title_Name=tv.TextCode and ca.Title_Name='" + partnametitlle + "'");
                                PdfTextArea pdf34 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 610, 595, 150), System.Drawing.ContentAlignment.TopLeft, partnametitlle);
                                //PdfTextArea pdf34 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 610, 595, 150), System.Drawing.ContentAlignment.TopLeft, "3 (B)" + "Health and Physical Education");
                                mypdfpage2.Add(pdf34);
                            }
                            else
                            {
                                PdfTextArea pdf34 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 610, 595, 150), System.Drawing.ContentAlignment.TopLeft, "");
                                mypdfpage2.Add(pdf34);
                            }

                            PdfTextArea pdf35 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 625, 595, 150), System.Drawing.ContentAlignment.TopLeft, "(Any two to be assessed) ");
                            mypdfpage2.Add(pdf35);

                            PdfTextArea pdf36 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 640, 595, 150), System.Drawing.ContentAlignment.TopLeft, "1. Literary & Creative Skills");
                            mypdfpage2.Add(pdf36);

                            PdfTextArea pdf37 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 180, 640, 595, 150), System.Drawing.ContentAlignment.TopLeft, "2. Scientific Skills");
                            mypdfpage2.Add(pdf37);

                            PdfTextArea pdf38 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 280, 640, 595, 150), System.Drawing.ContentAlignment.TopLeft, "3. Information and Communication");
                            mypdfpage2.Add(pdf38);

                            PdfTextArea pdf39 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 655, 595, 150), System.Drawing.ContentAlignment.TopLeft, " Technology (ICT)");
                            mypdfpage2.Add(pdf39);

                            PdfTextArea pdf40 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 130, 655, 595, 150), System.Drawing.ContentAlignment.TopLeft, "4. Organisational & Leadership Skills (Clubs).");
                            mypdfpage2.Add(pdf40);

                            DataTable dtgrid6 = new DataTable();

                            Gios.Pdf.PdfTable tablepage3d = mydoc.NewTable(Fontmedium, 3, 3, 1);
                            //table = mydoc.NewTable(Fontbold1, 7, 2, 1);
                            tablepage3d.VisibleHeaders = false;
                            tablepage3d.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            tablepage3d.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage3d.Cell(0, 0).SetContent("   S.No.  ");
                            tablepage3d.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage3d.Cell(0, 1).SetContent("   Descriptive Indicators*  ");
                            tablepage3d.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage3d.Cell(0, 2).SetContent("  Grade   ");

                            if (dvsk4.Count > 0)
                            {
                                for (int snda = 0; snda < 2; snda++)
                                {

                                    if (dvsk4.Count > snda)
                                    {
                                        tablepage3d.VisibleHeaders = false;
                                        tablepage3d.Cell(snda + 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        //tablepage3d.Cell(snda + 1, 0).SetContent("" + Convert.ToString(snda + 1) + ". " + dvsk4[snda]["aTextVal"].ToString());
                                        tablepage3d.Cell(snda + 1, 0).SetContent("" + Convert.ToString(snda + 1));
                                        tablepage3d.Cell(snda + 1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tablepage3d.Cell(snda + 1, 1).SetContent(Convert.ToString(dvsk4[snda]["description"]));
                                        tablepage3d.Cell(snda + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tablepage3d.Cell(snda + 1, 2).SetContent(Convert.ToString(dvsk4[snda]["Grade"]));
                                    }
                                    else
                                    {


                                    }
                                }
                            }

                            tablepage3d.Columns[0].SetWidth(30);
                            tablepage3d.Columns[1].SetWidth(200);
                            tablepage3d.Columns[2].SetWidth(50);

                            foreach (PdfCell rr in tablepage3d.Cells)
                                rr.SetCellPadding(8);

                            Gios.Pdf.PdfTablePage newpdftabpage3d = tablepage3d.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 680, 550, 600));
                            mypdfpage2.Add(newpdftabpage3d);



                            ////////////////////page 4///////////////////////
                            mypdfpage6.Add(pr3);
                            if (dropterm.SelectedItem.Text == "2")
                            {


                                //PdfTextArea pdf42 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 35, 595, 500), System.Drawing.ContentAlignment.TopLeft, "(Any two to be assessed)");
                                //mypdfpage6.Add(pdf42);

                                PdfTextArea pdf43 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 50, 60, 595, 500), System.Drawing.ContentAlignment.TopLeft, "1. Sports / Indigenous sports (Kho - Kho etc.), ");
                                //mypdfpage6.Add(pdf43);

                                PdfTextArea pdf44 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 320, 60, 595, 500), System.Drawing.ContentAlignment.TopLeft, "2. NCC/ NSS   3. Scouting and Guiding ");
                                //mypdfpage6.Add(pdf44);
                                PdfTextArea pdf45 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 50, 75, 595, 500), System.Drawing.ContentAlignment.TopLeft, "4. Swimming  5. Gymnastics  6. Yoga  7. First Aid  8. Gardening / Shramdaan. ");
                                //mypdfpage6.Add(pdf45);
                                Gios.Pdf.PdfTable tablepage4a = mydoc.NewTable(Fontmedium, 3, 3, 1);
                                dttablevalue1.DefaultView.RowFilter = "SubTitle='3B'";
                                dvskactv1 = dttablevalue1.DefaultView;
                                allactivity = string.Empty;
                                partnametitlle = string.Empty;
                                for (int i = 0; i < dvskactv1.Count; i++)
                                {

                                    if (allactivity == "")
                                    {
                                        allactivity = Convert.ToString(dvskactv1[i]["TextCode"]);
                                    }
                                    else
                                    {
                                        allactivity = allactivity + "','" + Convert.ToString(dvskactv1[i]["TextCode"]);
                                    }
                                    partnametitlle = Convert.ToString(dvskactv1[i]["Title_Name"]);
                                }


                                strqueryytable = "select tv.TextVal,ag.Grade,ag.description,cd.Mark from activity_gd ag ,CoCurrActivitie_Det cd,textvaltable tv where ag.ActivityTextVal=cd.ActivityTextVal and tv.TextCode=ag.ActivityTextVal and tv.TextCode=cd.ActivityTextVal and  cd.ActivityTextVal in ('" + allactivity + "') and cd.Roll_No='" + Roll_No + "' and cd.Mark between frompoint and topoint  ";

                                strqueryytablefil = da.select_method_wop_table(strqueryytable, "Text");
                                DataView dvsk5 = strqueryytablefil.DefaultView;

                                if (dvsk5.Count > 0)
                                {
                                    partnametitlle = da.GetFunction("select upper(SubTitle)+' : '+textval from textvaltable tv,CoCurr_Activitie ca  where ca.Title_Name=tv.TextCode and ca.Title_Name='" + partnametitlle + "'");
                                    PdfTextArea pdf41 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 20, 595, 500), System.Drawing.ContentAlignment.TopLeft, partnametitlle);
                                    mypdfpage6.Add(pdf41);
                                    pdf35 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 35, 595, 150), System.Drawing.ContentAlignment.TopLeft, "(Any two to be assessed) ");
                                    mypdfpage6.Add(pdf35);
                                    tablepage4a.VisibleHeaders = false;
                                    tablepage4a.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    tablepage4a.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablepage4a.Cell(0, 0).SetContent(" S.No.  ");
                                    tablepage4a.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablepage4a.Cell(0, 1).SetContent("   Descriptive Indicators*  ");
                                    tablepage4a.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablepage4a.Cell(0, 2).SetContent("  Grade   ");

                                    for (int snda = 0; snda < 2; snda++)
                                    {
                                        if (dvsk5.Count > snda)
                                        {
                                            tablepage4a.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            tablepage4a.Cell(snda + 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            //tablepage4a.Cell(snda + 1, 0).SetContent("" + Convert.ToString(snda + 1) + ". " + dvsk5[snda]["aTextVal"].ToString());
                                            tablepage4a.Cell(snda + 1, 0).SetContent("" + Convert.ToString(snda + 1));
                                            tablepage4a.Cell(snda + 1, 1).SetContentAlignment(ContentAlignment.TopLeft);
                                            tablepage4a.Cell(snda + 1, 1).SetContent(Convert.ToString(dvsk5[snda]["description"]));
                                            tablepage4a.Cell(snda + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tablepage4a.Cell(snda + 1, 2).SetContent(Convert.ToString(dvsk5[snda]["Grade"]));
                                        }
                                        else
                                        {

                                            PdfTextArea pdf4441 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 20, 595, 500), System.Drawing.ContentAlignment.TopLeft, "");
                                            mypdfpage6.Add(pdf4441);

                                        }
                                    }
                                }
                                else
                                {
                                    tablepage4a.VisibleHeaders = false;
                                    tablepage4a.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    tablepage4a.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablepage4a.Cell(0, 0).SetContent(" Activity  ");
                                    tablepage4a.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablepage4a.Cell(0, 1).SetContent("   Descriptive Indicators*  ");
                                    tablepage4a.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablepage4a.Cell(0, 2).SetContent("  Grade   ");

                                    tablepage4a.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    tablepage4a.Cell(1, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                    tablepage4a.Cell(1, 0).SetContent("");
                                    tablepage4a.Cell(1, 1).SetContentAlignment(ContentAlignment.TopLeft);
                                    tablepage4a.Cell(1, 1).SetContent("");
                                    tablepage4a.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablepage4a.Cell(1, 2).SetContent("");

                                    tablepage4a.Cell(2, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                    tablepage4a.Cell(2, 0).SetContent("");
                                    tablepage4a.Cell(2, 1).SetContentAlignment(ContentAlignment.TopLeft);
                                    tablepage4a.Cell(2, 1).SetContent("");
                                    tablepage4a.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablepage4a.Cell(2, 2).SetContent("");
                                }

                                //tablepage4a.Columns[0].SetWidth(80);
                                //tablepage4a.Columns[1].SetWidth(150);
                                //tablepage4a.Columns[2].SetWidth(50);

                                tablepage4a.Columns[0].SetWidth(30);
                                tablepage4a.Columns[1].SetWidth(200);
                                tablepage4a.Columns[2].SetWidth(50);

                                foreach (PdfCell rr in tablepage4a.Cells)
                                    rr.SetCellPadding(5);
                                Gios.Pdf.PdfTablePage newpdftabpage4 = tablepage4a.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 95, 550, 600));
                                mypdfpage6.Add(newpdftabpage4);
                            }
                            DataTable dpdfhealth = new DataTable();
                            DataSet dhealth = new DataSet();

                            string extraactivity = "  SELECT isnull(StudHeight,'') StudHeight ,isnull(StudWeight,'')StudWeight,isnull(VisionLeft,'')VisionLeft,isnull(VisionRight,'')VisionRight,isnull(DentalHygiene,'')DentalHygiene,isnull(Goals,'')Goals,isnull(Strenghts,'')Strenghts,isnull(ExcepAchieve,'')ExcepAchieve,isnull(hobbies,'')hobbies,isnull(TextVal,'') bldgrp From applyn A  inner join Registration R on A.app_no = R.App_No left join textvaltable t on t.TextCode = a.bldgrp and R.Degree_Code='" + degreecode + "' and R.Batch_Year='" + batchyear + "'";
                            dhealth = da.select_method_wo_parameter(extraactivity, "text");
                            if (dropterm.SelectedItem.Text == "2")
                            {

                                if (dhealth.Tables[0].Rows.Count > 0)
                                {
                                    PdfRectangle prhealth = new PdfRectangle(mydoc, pahealth, Color.Black);
                                    mypdfpage6.Add(prhealth);

                                    PdfTextArea pdf46 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 240, 225, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Health Status ");
                                    mypdfpage6.Add(pdf46);
                                    //pdf46 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 250, 200, 595, 500), System.Drawing.ContentAlignment.TopLeft, "________________");
                                    //mypdfpage6.Add(pdf46);

                                    PdfTextArea pdf47 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 50, 250, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Height");
                                    mypdfpage6.Add(pdf47);

                                    pdf47 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 100, 250, 595, 500), System.Drawing.ContentAlignment.TopLeft, "_____________");
                                    mypdfpage6.Add(pdf47);

                                    PdfTextArea pdf48 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 120, 250, 595, 500), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dhealth.Tables[0].Rows[0]["StudHeight"]));
                                    mypdfpage6.Add(pdf48);

                                    PdfTextArea pdf49 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 220, 250, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Weight");
                                    mypdfpage6.Add(pdf49);

                                    pdf49 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 270, 250, 595, 500), System.Drawing.ContentAlignment.TopLeft, "___________");
                                    mypdfpage6.Add(pdf49);

                                    PdfTextArea pdf50 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 270, 250, 595, 500), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dhealth.Tables[0].Rows[0]["StudWeight"]));
                                    mypdfpage6.Add(pdf50);
                                    PdfTextArea pdf451 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 360, 250, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Blood Group");
                                    mypdfpage6.Add(pdf451);

                                    pdf451 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 450, 250, 595, 500), System.Drawing.ContentAlignment.TopLeft, "______________");
                                    mypdfpage6.Add(pdf451);

                                    PdfTextArea pdf452 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 460, 250, 595, 500), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dhealth.Tables[0].Rows[0]["bldgrp"]));
                                    mypdfpage6.Add(pdf452);


                                    PdfTextArea pdf453 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 50, 270, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Vision (L) ");
                                    mypdfpage6.Add(pdf453);

                                    pdf453 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 115, 270, 595, 500), System.Drawing.ContentAlignment.TopLeft, "____________");
                                    mypdfpage6.Add(pdf453);

                                    PdfTextArea pdf454 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 120, 270, 595, 500), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dhealth.Tables[0].Rows[0]["VisionRight"]));
                                    mypdfpage6.Add(pdf454);

                                    PdfTextArea pdf455 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 220, 270, 595, 500), System.Drawing.ContentAlignment.TopLeft, "(R)");
                                    mypdfpage6.Add(pdf455);

                                    pdf455 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 250, 270, 595, 500), System.Drawing.ContentAlignment.TopLeft, "______________");
                                    mypdfpage6.Add(pdf455);

                                    PdfTextArea pdf456 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 270, 270, 595, 500), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dhealth.Tables[0].Rows[0]["VisionLeft"]));
                                    mypdfpage6.Add(pdf456);


                                    PdfTextArea pdf457 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 360, 270, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Dental Hygiene");
                                    mypdfpage6.Add(pdf457);

                                    pdf457 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 460, 270, 595, 500), System.Drawing.ContentAlignment.TopLeft, "____________");
                                    mypdfpage6.Add(pdf457);

                                    PdfTextArea pdf458 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 460, 270, 595, 500), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dhealth.Tables[0].Rows[0]["DentalHygiene"]));
                                    mypdfpage6.Add(pdf458);

                                    PdfTextArea pdf459 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 240, 340, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Self Awareness");
                                    mypdfpage6.Add(pdf459);


                                }
                                else
                                {
                                    PdfRectangle prhealth = new PdfRectangle(mydoc, pahealth, Color.Black);
                                    mypdfpage6.Add(prhealth);

                                    PdfTextArea pdf46 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 240, 200, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Health Status ");
                                    mypdfpage6.Add(pdf46);


                                    PdfTextArea pdf47 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 50, 230, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Height");
                                    mypdfpage6.Add(pdf47);

                                    PdfTextArea pdf48 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 120, 230, 595, 500), System.Drawing.ContentAlignment.TopLeft, "");
                                    mypdfpage6.Add(pdf48);

                                    PdfTextArea pdf49 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 220, 230, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Weight");
                                    mypdfpage6.Add(pdf49);

                                    PdfTextArea pdf50 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 270, 230, 595, 500), System.Drawing.ContentAlignment.TopLeft, "");
                                    mypdfpage6.Add(pdf50);
                                    PdfTextArea pdf451 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 360, 230, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Blood Group");
                                    mypdfpage6.Add(pdf451);

                                    PdfTextArea pdf452 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 460, 230, 595, 500), System.Drawing.ContentAlignment.TopLeft, "");
                                    mypdfpage6.Add(pdf452);


                                    PdfTextArea pdf453 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 50, 250, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Vision (L) ");
                                    mypdfpage6.Add(pdf453);

                                    PdfTextArea pdf454 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 120, 250, 595, 500), System.Drawing.ContentAlignment.TopLeft, "");
                                    mypdfpage6.Add(pdf454);

                                    PdfTextArea pdf455 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 220, 250, 595, 500), System.Drawing.ContentAlignment.TopLeft, "(R)");
                                    mypdfpage6.Add(pdf455);

                                    PdfTextArea pdf456 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 270, 250, 595, 500), System.Drawing.ContentAlignment.TopLeft, "");
                                    mypdfpage6.Add(pdf456);
                                    PdfTextArea pdf457 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 360, 250, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Dental Hygiene");
                                    mypdfpage6.Add(pdf457);

                                    PdfTextArea pdf458 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 460, 250, 595, 500), System.Drawing.ContentAlignment.TopLeft, "");
                                    mypdfpage6.Add(pdf458);

                                    PdfTextArea pdf459 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 240, 320, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Self Awareness");
                                    mypdfpage6.Add(pdf459);
                                }

                                Gios.Pdf.PdfTable tablepage4b = mydoc.NewTable(Fontmedium, 4, 1, 1);
                                //table = mydoc.NewTable(Fontbold1, 7, 2, 1);
                                tablepage4b.VisibleHeaders = false;
                                tablepage4b.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                tablepage4b.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                tablepage4b.Cell(0, 0).SetContent("My Goals :");

                                tablepage4b.Cell(1, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                tablepage4b.Cell(1, 0).SetContent("My Strengths :");
                                tablepage4b.Cell(2, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                tablepage4b.Cell(2, 0).SetContent("My Interests and Hobbies :");
                                tablepage4b.Cell(3, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                tablepage4b.Cell(3, 0).SetContent("Responsibilities Discharged / Exceptional Achievements :");

                                foreach (PdfCell rr in tablepage4b.Cells)
                                    rr.SetCellPadding(18);
                                Gios.Pdf.PdfTablePage newpdftabpage4b = tablepage4b.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 370, 550, 600));
                                mypdfpage6.Add(newpdftabpage4b);
                            }

                            PdfTextArea pdf460 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 50, 690, 595, 500), System.Drawing.ContentAlignment.TopLeft, "SIGN:");

                            if (dropterm.SelectedItem.Text.Trim() == "2")
                            {
                                mypdfpage6.Add(pdf460);
                            }
                            else
                            {
                                mypdfpage1.Add(pdf460);
                            }

                            Gios.Pdf.PdfTable tablepage4c = mydoc.NewTable(Fontmedium, 4, 3, 1);
                            //table = mydoc.NewTable(Fontbold1, 7, 2, 1);
                            tablepage4c.VisibleHeaders = false;
                            tablepage4c.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                            tablepage4c.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            tablepage4c.Cell(0, 0).SetContent("");
                            tablepage4c.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage4c.Cell(0, 1).SetContent("Term - I ");
                            tablepage4c.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage4c.Cell(0, 2).SetContent("Term - I   ");

                            tablepage4c.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            tablepage4c.Cell(1, 0).SetContent("Class Teacher");
                            tablepage4c.Cell(1, 1).SetContentAlignment(ContentAlignment.TopLeft);
                            tablepage4c.Cell(1, 1).SetContent("");
                            tablepage4c.Cell(1, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            tablepage4c.Cell(1, 2).SetContent("");

                            tablepage4c.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            tablepage4c.Cell(2, 0).SetContent("Principal");
                            tablepage4c.Cell(2, 1).SetContentAlignment(ContentAlignment.TopLeft);
                            tablepage4c.Cell(2, 1).SetContent("");
                            tablepage4c.Cell(2, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            tablepage4c.Cell(2, 2).SetContent("");

                            tablepage4c.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            tablepage4c.Cell(3, 0).SetContent("Parent");
                            tablepage4c.Cell(3, 1).SetContentAlignment(ContentAlignment.TopLeft);
                            tablepage4c.Cell(3, 1).SetContent("");
                            tablepage4c.Cell(3, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            tablepage4c.Cell(3, 2).SetContent("");

                            foreach (PdfCell rr in tablepage4c.Cells)
                                rr.SetCellPadding(15);
                            Gios.Pdf.PdfTablePage newpdftabpage4c = tablepage4c.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 100, 610, 450, 600));


                            if (dropterm.SelectedItem.Text.Trim() == "2")
                            {
                                mypdfpage6.Add(newpdftabpage4c);
                            }
                            else
                            {
                                mypdfpage1.Add(newpdftabpage4c);
                            }





                            ////////////////////////page 5/////////////////////

                            mypdfpage5 = mydoc.NewPage();
                            mypdfpage5.Add(pr3);

                            PdfTextArea pdf6q1 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 10, 50, 595, 50), System.Drawing.ContentAlignment.TopCenter, "Scholastic Areas (Graading on 9 Point Scale) ");
                            mypdfpage5.Add(pdf6q1);



                            Gios.Pdf.PdfTable tablepage5 = mydoc.NewTable(Fontmedium, 10, 3, 1);
                            tablepage5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            tablepage5.VisibleHeaders = false;
                            tablepage5.SetColumnsWidth(new int[] { 100, 200, 130 });
                            tablepage5.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(0, 0).SetContent(" Grade");
                            tablepage5.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(0, 1).SetContent("Mark Range");
                            tablepage5.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(0, 2).SetContent(" Grade Point");


                            tablepage5.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(1, 0).SetContent("A1");
                            tablepage5.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(1, 1).SetContent("91 - 100");
                            tablepage5.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(1, 2).SetContent("10.0");


                            tablepage5.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(2, 0).SetContent("A2");
                            tablepage5.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(2, 1).SetContent("81 - 90");
                            tablepage5.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(2, 2).SetContent("9.0");

                            tablepage5.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(3, 0).SetContent("B1");
                            tablepage5.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(3, 1).SetContent("71 - 80");
                            tablepage5.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(3, 2).SetContent("8.0");

                            tablepage5.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(4, 0).SetContent("B2");
                            tablepage5.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(4, 1).SetContent("61 - 70");
                            tablepage5.Cell(4, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(4, 2).SetContent("7.0");

                            tablepage5.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(5, 0).SetContent("C1");
                            tablepage5.Cell(5, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(5, 1).SetContent("51 - 60");
                            tablepage5.Cell(5, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(5, 2).SetContent("6.0");

                            tablepage5.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(6, 0).SetContent("C2");
                            tablepage5.Cell(6, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(6, 1).SetContent("41 - 50");
                            tablepage5.Cell(6, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(6, 2).SetContent("5.0");


                            tablepage5.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(7, 0).SetContent("D");
                            tablepage5.Cell(7, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(7, 1).SetContent("33 - 40");
                            tablepage5.Cell(7, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(7, 2).SetContent("4.0");


                            tablepage5.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(8, 0).SetContent("E1");
                            tablepage5.Cell(8, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(8, 1).SetContent("21 - 32");
                            tablepage5.Cell(8, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(8, 2).SetContent("3.0");


                            tablepage5.Cell(9, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(9, 0).SetContent("E2");
                            tablepage5.Cell(9, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(9, 1).SetContent("00 - 20");
                            tablepage5.Cell(9, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepage5.Cell(9, 2).SetContent("2.0");

                            int coltop = 100;


                            Gios.Pdf.PdfTablePage newpdftabpage5 = tablepage5.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 80, coltop, 430, 600));
                            mypdfpage5.Add(newpdftabpage5);


                            PdfTextArea pdf61 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 10, 300, 595, 50), System.Drawing.ContentAlignment.TopCenter, "Co-Scholastic Areas (Graading on 5 Point Scale) ");
                            mypdfpage5.Add(pdf61);



                            Gios.Pdf.PdfTable tablepagefive = mydoc.NewTable(Fontmedium, 6, 3, 1);
                            tablepagefive.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            tablepagefive.SetColumnsWidth(new int[] { 100, 200, 130 });
                            tablepagefive.VisibleHeaders = false;
                            tablepagefive.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(0, 0).SetContent("Grade");
                            tablepagefive.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(0, 1).SetContent("Marks Range");
                            tablepagefive.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(0, 2).SetContent("Grade Point");

                            tablepagefive.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(1, 0).SetContent("A");
                            tablepagefive.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(1, 1).SetContent("4.1 - 5.0");
                            tablepagefive.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(1, 2).SetContent("5");


                            tablepagefive.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(2, 0).SetContent("B");
                            tablepagefive.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(2, 1).SetContent("3.1 - 4.0");
                            tablepagefive.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(2, 2).SetContent("4");


                            tablepagefive.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(3, 0).SetContent("C");
                            tablepagefive.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(3, 1).SetContent("2.1 - 3.0");
                            tablepagefive.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(3, 2).SetContent("3");


                            tablepagefive.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(4, 0).SetContent("D");
                            tablepagefive.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(4, 1).SetContent("1.1 - 2.0");
                            tablepagefive.Cell(4, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(4, 2).SetContent("2");

                            tablepagefive.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(5, 0).SetContent("E");
                            tablepagefive.Cell(5, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(5, 1).SetContent("0.1 - 1.0");
                            tablepagefive.Cell(5, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablepagefive.Cell(5, 2).SetContent("1");

                            Gios.Pdf.PdfTablePage newtablepagepoint = tablepagefive.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 80, 350, 430, 600));
                            mypdfpage5.Add(newtablepagepoint);

                            PdfTextArea pdf6w1 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 500, 595, 50), System.Drawing.ContentAlignment.TopCenter, "Student must obtain the qualifying grade (minimum grade D) in all");
                            mypdfpage5.Add(pdf6w1);

                            PdfTextArea pdf6w12 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 520, 595, 50), System.Drawing.ContentAlignment.TopCenter, "the subjects under Scholastic and Co-Scholastic Domain.");
                            mypdfpage5.Add(pdf6w12);


                            PdfArea pa21 = new PdfArea(mydoc, 90, 500, 450, 40);
                            PdfRectangle pr21 = new PdfRectangle(mydoc, pa21, Color.Black);
                            mypdfpage5.Add(pr21);

                            PdfTextArea pdf6wq12 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 30, 560, 595, 50), System.Drawing.ContentAlignment.TopLeft, "*    First Term       ");
                            mypdfpage5.Add(pdf6wq12);

                            PdfTextArea pdf6wee12 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 30, 580, 595, 50), System.Drawing.ContentAlignment.TopLeft, "*   Second Term   ");
                            mypdfpage5.Add(pdf6wee12);

                            PdfTextArea pdf6wr12 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 30, 600, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Formative Assessment");
                            mypdfpage5.Add(pdf6wr12);

                            PdfTextArea pdf6wt12 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 30, 620, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Summative Assessment");
                            mypdfpage5.Add(pdf6wt12);

                            PdfTextArea pdf6wq121 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 200, 560, 595, 50), System.Drawing.ContentAlignment.TopLeft, " : ");
                            mypdfpage5.Add(pdf6wq121);

                            PdfTextArea pdf6wee122 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 200, 580, 595, 50), System.Drawing.ContentAlignment.TopLeft, " : ");
                            mypdfpage5.Add(pdf6wee122);

                            PdfTextArea pdf6wr123 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 200, 600, 595, 50), System.Drawing.ContentAlignment.TopLeft, " : ");
                            mypdfpage5.Add(pdf6wr123);

                            PdfTextArea pdf6wt124 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 200, 620, 595, 50), System.Drawing.ContentAlignment.TopLeft, " : ");
                            mypdfpage5.Add(pdf6wt124);

                            PdfTextArea pdf6wq1221 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 220, 560, 595, 50), System.Drawing.ContentAlignment.TopLeft, " FA1 (10%) + FA2(10%) + SA1 (30%) = 50% ");
                            mypdfpage5.Add(pdf6wq1221);

                            PdfTextArea pdf6wee1232 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 220, 580, 595, 50), System.Drawing.ContentAlignment.TopLeft, " FA3 (10%) + FA4(10%) + SA2 (30%) = 50%  ");
                            mypdfpage5.Add(pdf6wee1232);

                            PdfTextArea pdf6wr1243 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 220, 600, 595, 50), System.Drawing.ContentAlignment.TopLeft, " FA1 (10%) + FA2(10%) + FA3 (10%) + FA4(10%) = 40% ");
                            mypdfpage5.Add(pdf6wr1243);

                            PdfTextArea pdf6wt1254 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 220, 620, 595, 50), System.Drawing.ContentAlignment.TopLeft, " SA1 (30%) +  SA2 (30%) = 60%  ");
                            mypdfpage5.Add(pdf6wt1254);

                            if (dropterm.SelectedItem.Text == "2")
                            {
                                mypdfpage.SaveToDocument();
                                mypdfpage1.SaveToDocument();
                                mypdfpagefinal.SaveToDocument();
                                mypdfpage2.SaveToDocument();
                                mypdfpage6.SaveToDocument();
                                mypdfpage5.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                mypdfpage1 = mydoc.NewPage();

                                mypdfpage2 = mydoc.NewPage();
                                mypdfpage6 = mydoc.NewPage();
                                mypdfpage5 = mydoc.NewPage();
                            }
                            else
                            {
                                mypdfpage.SaveToDocument();
                                mypdfpage1.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                mypdfpage1 = mydoc.NewPage();





                            }


                        }
                    }
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "rankcard" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";

                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void bindbutn_three(string rollno)
    {
        try
        {
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            Gios.Pdf.PdfPage mypdfpage;
            Gios.Pdf.PdfPage mypdfpage1;
            ArrayList testcriterianos = new ArrayList();
            rollnos = rollno;
            if (rollnos != "")
            {
                sql = "select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No in ('" + rollnos + "') ;";
                studgradeds.Clear();
                studgradeds = d2.select_method_wo_parameter(sql, "text");
                if (studgradeds.Tables[0].Rows.Count > 0)
                {

                    for (int roll = 0; roll < studgradeds.Tables[0].Rows.Count; roll++)
                    {
                        string rcrollno = string.Empty;
                        rcrollno = Convert.ToString(studgradeds.Tables[0].Rows[roll][1]);

                        bindstudentdetails(rcrollno);
                        bindstudentdetails_new(rcrollno);
                        //bindbutn(rcrollno);


                        DataSet ds = new DataSet();
                        DataSet dschool = new DataSet();
                        DAccess2 da = new DAccess2();
                        DataSet dset = new DataSet();
                        string college_code = Convert.ToString(collegecode);
                        string stdappno = string.Empty;
                        System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
                        System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                        System.Drawing.Font Fontbold1 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
                        System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
                        System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                        System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 7, FontStyle.Regular);
                        System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
                        System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
                        System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);



                        string Roll_No = rcrollno;
                        sql = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No='" + Roll_No + "'";
                        ds.Clear();
                        ds.Dispose();
                        ds = da.select_method_wo_parameter(sql, "Text");
                        DataView dv = new DataView();
                        ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                        dv = ds.Tables[1].DefaultView;
                        int count4 = 0;
                        count4 = dv.Count;

                        if (ds.Tables[0].Rows.Count > 0 && count4 > 0)
                        {
                            string currentsem = Convert.ToString(dv[0]["Current_Semester"]);
                            string batchyear = Convert.ToString(dv[0]["Batch_Year"]);
                            string degreecode = Convert.ToString(dv[0]["degree_code"]);
                            stdappno = Convert.ToString(dv[0]["App_No"]);
                            string allsem = "1";
                            string admdate = Convert.ToString(dv[0]["adm_date"]);
                            string section = string.Empty;
                            string stdcc = string.Empty;
                            stdcc = Convert.ToString(ds.Tables[1].Rows[0]["Dept_Name"]);
                            section = Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
                            string lblclassq = "CLASS - " + "XI - XII" + "  Academic Year :";
                            //persentmonthcal(Roll_No,admdate,);
                            if (Convert.ToInt32(currentsem) > 1)
                            {
                                int term = Convert.ToInt32(currentsem);
                                for (int i = 2; i <= term; i++)
                                {
                                    allsem = allsem + "'" + "," + "'" + i;
                                }
                            }




                            if (Convert.ToInt32(currentsem) >= 1)
                            {
                                int term = Convert.ToInt32(currentsem);
                                for (int i = 1; i <= term; i++)
                                {
                                    string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + i + "' and degree_code='" + degreecode + "' and batch_year='" + batchyear + "'";
                                    dset = da.select_method_wo_parameter(sem, "Text");

                                    if (dset.Tables[0].Rows.Count > 0)
                                    {
                                        string startdate = Convert.ToString(dset.Tables[0].Rows[0]["start_date"]);
                                        string enddate = Convert.ToString(dset.Tables[0].Rows[0]["end_date"]);
                                        persentmonthcal(Roll_No, admdate, startdate, enddate);
                                    }

                                    if (i == 1)
                                    {

                                        lbltot_att1 = Convert.ToString(pre_present_date);
                                        lbltot_work1 = Convert.ToString(per_workingdays);
                                        working1 = Convert.ToString(pre_present_date);
                                        present1 = Convert.ToString(per_workingdays);
                                    }

                                }
                            }

                            string table1sql = "select * from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'  and semester = '" + currentsem + "' ; SELECT COUNT(*) as facount FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "'); SELECT Istype,CRITERIA_NO,y.semester FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "') ; SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "')";
                            DataSet ds1fortable1 = new DataSet();
                            ds1fortable1.Clear();
                            ds1fortable1.Dispose();
                            ds1fortable1 = da.select_method_wo_parameter(table1sql, "Text");
                            DataView dvforpage2 = new DataView();

                            string dob = Convert.ToString(dv[0]["dob"]);
                            string[] dobspit = dob.Split('/');
                            string addressline1 = Convert.ToString(dv[0]["parent_addressP"]);
                            addressline1 = addressline1 + " " + Convert.ToString(dv[0]["Streetp"]);
                            string addressline2 = Convert.ToString(dv[0]["Cityp"]);
                            addressline2 = addressline2 + " - " + Convert.ToString(dv[0]["parent_pincodep"]);
                            string mobileno = Convert.ToString(dv[0]["student_mobile"]);

                            mypdfpage = mydoc.NewPage();


                            PdfTextArea pdf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 420, 20, 420, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(ds.Tables[0].Rows[0][0]) + "");
                            PdfTextArea pdf11 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 420, 40, 420, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(ds.Tables[0].Rows[0][1]) + "");
                            string address = Convert.ToString(ds.Tables[0].Rows[0][2]) + "" + Convert.ToString(ds.Tables[0].Rows[0][3]) + "" + Convert.ToString(ds.Tables[0].Rows[0][4]);
                            PdfTextArea pdf12 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 420, 55, 420, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
                            PdfTextArea pdf172 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 420, 70, 420, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  :" + Convert.ToString(ds.Tables[0].Rows[0][6]));


                            PdfArea pa2 = new PdfArea(mydoc, 510, 165, 224, 40);
                            PdfRectangle pr2 = new PdfRectangle(mydoc, pa2, Color.Black);
                            mypdfpage.Add(pr2);

                            string sqlschool = "select value from Master_Settings where settings='Academic year'";
                            dschool = da.select_method_wo_parameter(sqlschool, "Text");
                            string splitvalue = Convert.ToString(dschool.Tables[0].Rows[0]["value"]);
                            string[] dsplit = splitvalue.Split(',');

                            string fvalue = Convert.ToString(dsplit[0]);
                            string lvalue = Convert.ToString(dsplit[1]);
                            string acdmic_date = fvalue + "-" + lvalue;

                            PdfTextArea pdf13 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 520, 175, 204, 30), System.Drawing.ContentAlignment.TopLeft, "Report of Academic Performance ");
                            PdfTextArea pdf14 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 220, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + lblclassq + "" + acdmic_date);
                            PdfTextArea pdf17 = new PdfTextArea(Fontmedium1, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 260, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Student Profile:");


                            PdfTextArea pdf111 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 295, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Admission No.");
                            PdfTextArea pdf112 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 550, 295, 595, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                            PdfTextArea pdf113 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 565, 295, 595, 50), System.Drawing.ContentAlignment.TopLeft, "________");
                            PdfTextArea pdf113a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 570, 295, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["roll_admit"]) + "");
                            PdfTextArea pdf114 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 635, 295, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Exam No :");
                            PdfTextArea pdf115 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 690, 295, 595, 50), System.Drawing.ContentAlignment.TopLeft, "    " + Convert.ToString(dv[0]["Reg_No"]) + "");
                            PdfTextArea pdf000 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 690, 295, 595, 50), System.Drawing.ContentAlignment.TopLeft, "__________________");

                            PdfTextArea pdf118b1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 315, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Roll No ");
                            PdfTextArea pdf118qb1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 570, 315, 595, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dv[0]["Roll_No"]));
                            PdfTextArea pdf118bq1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 550, 315, 595, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                            PdfTextArea pdf118c1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 565, 315, 595, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________");

                            PdfTextArea pdf116 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 335, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Registration No.");
                            PdfTextArea pdf117 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 550, 335, 595, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                            PdfTextArea pdf118 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 565, 335, 595, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________");
                            PdfTextArea pdf118a1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 570, 335, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["Reg_No"]) + "");
                            mypdfpage.Add(pdf116);
                            mypdfpage.Add(pdf117);
                            mypdfpage.Add(pdf118);
                            mypdfpage.Add(pdf118a1);
                            mypdfpage.Add(pdf118b1);
                            mypdfpage.Add(pdf118c1);
                            mypdfpage.Add(pdf118qb1);
                            mypdfpage.Add(pdf118bq1);
                            PdfTextArea pdf18ab = new PdfTextArea(Fontsmall9, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 350, 595, 50), System.Drawing.ContentAlignment.TopLeft, "(alloted by the Board");
                            PdfTextArea pdf18a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 370, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Name");
                            PdfTextArea pdf1811 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 550, 370, 595, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                            PdfTextArea pdf1822 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 570, 370, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["stud_name"]) + "");
                            PdfTextArea pdfee = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 565, 370, 595, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________");
                            mypdfpage.Add(pdf18ab);
                            mypdfpage.Add(pdf1822);
                            mypdfpage.Add(pdf1811);
                            mypdfpage.Add(pdfee);
                            mypdfpage.Add(pdf000);
                            mypdfpage.Add(pdf18a);
                            mypdfpage.Add(pdf111);
                            mypdfpage.Add(pdf112);
                            mypdfpage.Add(pdf113);
                            mypdfpage.Add(pdf113a);
                            mypdfpage.Add(pdf114);
                            mypdfpage.Add(pdf115);
                            mypdfpage.Add(pdf172);



                            PdfTextArea pdf119 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 390, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Birth");
                            PdfTextArea pdf120 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 550, 390, 595, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                            PdfTextArea pdf121 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 570, 390, 595, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dv[0]["dob"]));
                            PdfTextArea pdf1221 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 565, 390, 595, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________");
                            mypdfpage.Add(pdf1221);
                            mypdfpage.Add(pdf119);
                            mypdfpage.Add(pdf120);
                            mypdfpage.Add(pdf121);

                            PdfTextArea pdf122 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 410, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Mother's Name");
                            PdfTextArea pdf123 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 550, 410, 595, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                            PdfTextArea pdf124 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 565, 410, 595, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________");
                            PdfTextArea pdf124a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 570, 410, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["mother"]) + "");
                            mypdfpage.Add(pdf122);
                            mypdfpage.Add(pdf123);
                            mypdfpage.Add(pdf124);
                            mypdfpage.Add(pdf124a);

                            PdfTextArea pdf125 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 430, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Father's Name");
                            PdfTextArea pdf126 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 550, 430, 595, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                            PdfTextArea pdf127 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 565, 430, 595, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________");
                            PdfTextArea pdf127a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 570, 430, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["parent_name"]) + "");
                            mypdfpage.Add(pdf125);
                            mypdfpage.Add(pdf126);
                            mypdfpage.Add(pdf127);
                            mypdfpage.Add(pdf127a);

                            PdfTextArea pdf128 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 450, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Residential Address");
                            PdfTextArea pdf129 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 550, 450, 595, 50), System.Drawing.ContentAlignment.TopLeft, ":");
                            PdfTextArea pdf130 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 565, 450, 595, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________");
                            PdfTextArea pdf130a = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 570, 450, 400, 50), System.Drawing.ContentAlignment.TopLeft, "" + addressline1 + "");
                            mypdfpage.Add(pdf128);
                            mypdfpage.Add(pdf129);
                            mypdfpage.Add(pdf130);
                            mypdfpage.Add(pdf130a);

                            PdfTextArea pdf131 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 490, 595, 50), System.Drawing.ContentAlignment.TopLeft, " Telephone No.");
                            PdfTextArea pdf132 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 565, 470, 595, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________");
                            PdfTextArea pdf133a = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 570, 470, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + addressline2 + "");
                            PdfTextArea pdf134 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 565, 490, 595, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________");
                            PdfTextArea pdf134a = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 570, 490, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + mobileno + "");
                            mypdfpage.Add(pdf131);
                            mypdfpage.Add(pdf132);
                            mypdfpage.Add(pdf133a);
                            mypdfpage.Add(pdf134);
                            mypdfpage.Add(pdf134a);
                            PdfTextArea pdf135 = new PdfTextArea(Fontmedium1, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 510, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Attendance:");
                            mypdfpage.Add(pdf135);
                            PdfTextArea pdf136 = new PdfTextArea(Fontmedium1, System.Drawing.Color.Black, new PdfArea(mydoc, 630, 510, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Term I ");
                            mypdfpage.Add(pdf136);
                            PdfTextArea pdf137 = new PdfTextArea(Fontmedium1, System.Drawing.Color.Black, new PdfArea(mydoc, 750, 510, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Term II ");
                            mypdfpage.Add(pdf137);
                            PdfTextArea pdf138 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 530, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Total attendance of the Student");
                            PdfTextArea pdf139 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 615, 530, 595, 50), System.Drawing.ContentAlignment.TopLeft, "             " + lbltot_att1 + "");
                            PdfTextArea pdf140 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 735, 530, 595, 50), System.Drawing.ContentAlignment.TopLeft, "       " + lbltot_att2 + "");
                            mypdfpage.Add(pdf138);
                            mypdfpage.Add(pdf140);
                            mypdfpage.Add(pdf139);

                            PdfTextArea pdf14ws2 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 615, 530, 595, 50), System.Drawing.ContentAlignment.TopLeft, "______________");
                            PdfTextArea pdf14ws3 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 730, 530, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_____________");
                            mypdfpage.Add(pdf14ws3);
                            mypdfpage.Add(pdf14ws2);

                            PdfTextArea pdf141 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 550, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Total working days");
                            PdfTextArea pdf142 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 615, 550, 595, 50), System.Drawing.ContentAlignment.TopLeft, "             " + lbltot_work1 + "");
                            PdfTextArea pdf143 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 735, 550, 595, 50), System.Drawing.ContentAlignment.TopLeft, "       " + lbltot_work2 + "");
                            mypdfpage.Add(pdf141);
                            mypdfpage.Add(pdf142);
                            mypdfpage.Add(pdf143);
                            PdfTextArea pdf14w2 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 615, 550, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_____________");
                            PdfTextArea pdf14w3 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 730, 550, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_____________");
                            mypdfpage.Add(pdf14w3);
                            mypdfpage.Add(pdf14w2);

                            //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            //{
                            //    Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            //    mypdfpage.Add(LogoImage, 450, 96, 450);
                            //}
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                mypdfpage.Add(LogoImage1, 580, 96, 450);
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"));
                                mypdfpage.Add(LogoImage2, 750, 96, 450);
                            }
                            else
                            {
                                Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                mypdfpage.Add(LogoImage2, 750, 96, 450);
                            }

                            Hashtable hatsubject = new Hashtable();
                            Hashtable hatcriter = new Hashtable();



                            PdfArea pahealth = new PdfArea(mydoc, 30, 50, 350, 100);

                            DataTable dpdfhealth = new DataTable();
                            DataSet dhealth = new DataSet();

                            string extraactivity = "  SELECT isnull(StudHeight,'') StudHeight ,isnull(StudWeight,'')StudWeight,isnull(VisionLeft,'')VisionLeft,isnull(VisionRight,'')VisionRight,isnull(DentalHygiene,'')DentalHygiene,isnull(Goals,'')Goals,isnull(Strenghts,'')Strenghts,isnull(ExcepAchieve,'')ExcepAchieve,isnull(hobbies,'')hobbies,isnull(TextVal,'') bldgrp From applyn A  inner join Registration R on A.app_no = R.App_No left join textvaltable t on t.TextCode = a.bldgrp and R.Degree_Code='" + degreecode + "' and R.Batch_Year='" + batchyear + "'";
                            dhealth = da.select_method_wo_parameter(extraactivity, "text");

                            if (dhealth.Tables[0].Rows.Count > 0)
                            {

                                PdfRectangle prhealth = new PdfRectangle(mydoc, pahealth, Color.Black);
                                mypdfpage.Add(prhealth);

                                PdfTextArea pdf46 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 35, 25, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Health Status ");
                                mypdfpage.Add(pdf46);
                                //pdf46 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 250, 200, 595, 500), System.Drawing.ContentAlignment.TopLeft, "________________");
                                //mypdfpage6.Add(pdf46);

                                PdfTextArea pdf47 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 33, 55, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Height");
                                mypdfpage.Add(pdf47);

                                pdf47 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 80, 55, 595, 500), System.Drawing.ContentAlignment.TopLeft, "_________________");
                                mypdfpage.Add(pdf47);

                                PdfTextArea pdf48 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 55, 595, 500), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dhealth.Tables[0].Rows[0]["StudHeight"]));
                                mypdfpage.Add(pdf48);

                                PdfTextArea pdf49 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 200, 55, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Weight");
                                mypdfpage.Add(pdf49);

                                pdf49 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 240, 55, 595, 500), System.Drawing.ContentAlignment.TopLeft, "____________________");
                                mypdfpage.Add(pdf49);

                                PdfTextArea pdf50 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 250, 55, 595, 500), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dhealth.Tables[0].Rows[0]["StudWeight"]));
                                mypdfpage.Add(pdf50);
                                PdfTextArea pdf451 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 33, 85, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Blood Group");
                                mypdfpage.Add(pdf451);

                                PdfTextArea pdf451s = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 100, 85, 595, 500), System.Drawing.ContentAlignment.TopLeft, "_____________");
                                mypdfpage.Add(pdf451s);

                                PdfTextArea pdf452 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 120, 85, 595, 500), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dhealth.Tables[0].Rows[0]["bldgrp"]));
                                mypdfpage.Add(pdf452);


                                PdfTextArea pdf453 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 200, 85, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Vision (L) ");
                                mypdfpage.Add(pdf453);

                                PdfTextArea pdf45s3 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 253, 85, 595, 500), System.Drawing.ContentAlignment.TopLeft, "______");
                                mypdfpage.Add(pdf45s3);

                                PdfTextArea pdf454 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 260, 85, 595, 500), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dhealth.Tables[0].Rows[0]["VisionRight"]));
                                mypdfpage.Add(pdf454);

                                PdfTextArea pdf455 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 310, 85, 595, 500), System.Drawing.ContentAlignment.TopLeft, "(R)");
                                mypdfpage.Add(pdf455);

                                pdf455 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 330, 85, 595, 500), System.Drawing.ContentAlignment.TopLeft, "______");
                                mypdfpage.Add(pdf455);

                                PdfTextArea pdf456 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 350, 85, 595, 500), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dhealth.Tables[0].Rows[0]["VisionLeft"]));
                                mypdfpage.Add(pdf456);


                                PdfTextArea pdf457 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 33, 115, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Dental Hygiene");
                                mypdfpage.Add(pdf457);

                                pdf457 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 120, 115, 595, 500), System.Drawing.ContentAlignment.TopLeft, "______________________________________");
                                mypdfpage.Add(pdf457);

                                PdfTextArea pdf458 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 140, 115, 595, 500), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dhealth.Tables[0].Rows[0]["DentalHygiene"]));
                                mypdfpage.Add(pdf458);

                            }
                            else
                            {
                                PdfRectangle prhealth = new PdfRectangle(mydoc, pahealth, Color.Black);
                                mypdfpage.Add(prhealth);

                                PdfTextArea pdf46 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 35, 25, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Health Status ");
                                mypdfpage.Add(pdf46);

                                PdfTextArea pdf47 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 33, 55, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Height");
                                mypdfpage.Add(pdf47);

                                pdf47 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 80, 55, 595, 500), System.Drawing.ContentAlignment.TopLeft, "_________________");
                                mypdfpage.Add(pdf47);

                                PdfTextArea pdf49 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 200, 55, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Weight");
                                mypdfpage.Add(pdf49);

                                pdf49 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 240, 55, 595, 500), System.Drawing.ContentAlignment.TopLeft, "____________________");
                                mypdfpage.Add(pdf49);

                                PdfTextArea pdf451 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 33, 85, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Blood Group");
                                mypdfpage.Add(pdf451);

                                PdfTextArea pdf451s = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 100, 85, 595, 500), System.Drawing.ContentAlignment.TopLeft, "_____________");
                                mypdfpage.Add(pdf451s);

                                PdfTextArea pdf453 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 200, 85, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Vision (L) ");
                                mypdfpage.Add(pdf453);

                                PdfTextArea pdf45s3 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 253, 85, 595, 500), System.Drawing.ContentAlignment.TopLeft, "______");
                                mypdfpage.Add(pdf45s3);

                                PdfTextArea pdf455 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 310, 85, 595, 500), System.Drawing.ContentAlignment.TopLeft, "(R)");
                                mypdfpage.Add(pdf455);

                                pdf455 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 330, 85, 595, 500), System.Drawing.ContentAlignment.TopLeft, "______");
                                mypdfpage.Add(pdf455);

                                PdfTextArea pdf457 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 33, 115, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Dental Hygiene");
                                mypdfpage.Add(pdf457);

                                pdf457 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 120, 115, 595, 500), System.Drawing.ContentAlignment.TopLeft, "______________________________________");
                                mypdfpage.Add(pdf457);

                            }

                            PdfTextArea pdf45s7 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 35, 180, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Grading");
                            mypdfpage.Add(pdf45s7);


                            PdfArea pahealtsh = new PdfArea(mydoc, 30, 200, 350, 200);
                            PdfRectangle prhealtsh = new PdfRectangle(mydoc, pahealtsh, Color.Black);
                            mypdfpage.Add(prhealtsh);

                            PdfTextArea pdf45s7q = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 65, 210, 595, 500), System.Drawing.ContentAlignment.TopLeft, "A-1  -  Top 1/8th of the passed Candidates");
                            mypdfpage.Add(pdf45s7q);


                            PdfTextArea pdf45s7w = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 65, 230, 595, 500), System.Drawing.ContentAlignment.TopLeft, "A-2  -  Next 1/8th of the passed Candidates");
                            mypdfpage.Add(pdf45s7w);


                            PdfTextArea pdf45s7e = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 65, 250, 595, 500), System.Drawing.ContentAlignment.TopLeft, "B-1  -  Next 1/8th of the passed Candidates");
                            mypdfpage.Add(pdf45s7e);


                            PdfTextArea pdf415s7e = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 65, 270, 595, 500), System.Drawing.ContentAlignment.TopLeft, "B-2  -  Next 1/8th of the passed Candidates");
                            mypdfpage.Add(pdf415s7e);


                            PdfTextArea pdf45s37e = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 65, 290, 595, 500), System.Drawing.ContentAlignment.TopLeft, "C-1  -  Next 1/8th of the passed Candidates");
                            mypdfpage.Add(pdf45s37e);


                            PdfTextArea pdf45s74e = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 65, 310, 595, 500), System.Drawing.ContentAlignment.TopLeft, "C-2  -  Next 1/8th of the passed Candidates");
                            mypdfpage.Add(pdf45s74e);

                            PdfTextArea pdf45s7e5 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 65, 330, 595, 500), System.Drawing.ContentAlignment.TopLeft, "D-1  -  Next 1/8th of the passed Candidates");
                            mypdfpage.Add(pdf45s7e5);

                            PdfTextArea pdf45s7e3 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 65, 350, 595, 500), System.Drawing.ContentAlignment.TopLeft, "D-2  -  Next 1/8th of the passed Candidates");
                            mypdfpage.Add(pdf45s7e3);

                            PdfTextArea pdf45s72e = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 65, 370, 595, 500), System.Drawing.ContentAlignment.TopLeft, "E     -  Failed Candidates");
                            mypdfpage.Add(pdf45s72e);

                            PdfTextArea PDFQ = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 425, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Note:");
                            mypdfpage.Add(PDFQ);


                            PdfTextArea PDsFQ = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 445, 400, 500), System.Drawing.ContentAlignment.TopLeft, "The qualifying marks in each subject of external examination shall be  ");
                            mypdfpage.Add(PDsFQ);

                            PdfTextArea PDssFQ = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 460, 400, 500), System.Drawing.ContentAlignment.TopLeft, "33% However in a subject involvinf practical work , a candidate must  ");
                            mypdfpage.Add(PDssFQ);

                            PdfTextArea PDssFaQ = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 475, 400, 500), System.Drawing.ContentAlignment.TopLeft, "obtain 33% marks in the theory and 33% marks in the practical");
                            mypdfpage.Add(PDssFaQ);

                            PdfTextArea PDssFQr = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 490, 400, 500), System.Drawing.ContentAlignment.TopLeft, "separately in addition to 33% marks in aggregate , in order to qualify ");
                            mypdfpage.Add(PDssFQr);

                            PdfTextArea PDssFwQr = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 505, 400, 500), System.Drawing.ContentAlignment.TopLeft, "in the subject");
                            mypdfpage.Add(PDssFwQr);

                            mypdfpage.Add(pdf1);
                            mypdfpage.Add(pdf11);
                            mypdfpage.Add(pdf12);
                            mypdfpage.Add(pdf13);
                            mypdfpage.Add(pdf14);
                            mypdfpage.Add(pdf17);
                            ////////////////////total rec/////////////////
                            //PdfArea pa1 = new PdfArea(mydoc, 14, 12, 810, 575);
                            //PdfRectangle pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
                            //mypdfpage.Add(pr3);
                            ///////////////////left rec/////////////////////////

                            PdfArea pa12 = new PdfArea(mydoc, 14, 12, 390, 575);
                            PdfRectangle pr4 = new PdfRectangle(mydoc, pa12, Color.Black);
                            mypdfpage.Add(pr4);

                            /////////////////right////////////////////////

                            PdfArea pa5 = new PdfArea(mydoc, 415, 12, 410, 575);
                            PdfRectangle pr5 = new PdfRectangle(mydoc, pa5, Color.Black);
                            mypdfpage.Add(pr5);
                            mypdfpage.SaveToDocument();
                            mypdfpage1 = mydoc.NewPage();
                            //int fp2rowcount=FpSpread2.Sheets[0].Rows.Count;
                            //int fp2colcount=FpSpread2.Sheets[0].Columns.Count;


                            //=========================================
                            PdfTextArea Psw = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 40, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Name");
                            mypdfpage1.Add(Psw);

                            PdfTextArea Pqsw = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, " : ");
                            mypdfpage1.Add(Pqsw);


                            PdfTextArea Pssw = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 105, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dv[0]["stud_name"]));
                            mypdfpage1.Add(Pssw);



                            PdfTextArea Psww = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 480, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Class & Sec");
                            mypdfpage1.Add(Psww);

                            PdfTextArea Pqswe = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 560, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, " : ");
                            mypdfpage1.Add(Pqswe);


                            PdfTextArea Psswu = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 580, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, stdcc + "     " + section);
                            mypdfpage1.Add(Psswu);


                            int rowcountspread = FpSpread2.Sheets[0].RowCount + 5;
                            int columncountspread = FpSpread2.Sheets[0].ColumnCount;

                            Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(Fontsmall1, rowcountspread, columncountspread, 4);
                            table1forpage2.VisibleHeaders = false;
                            table1forpage2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table1forpage2.Columns[1].SetWidth(25);
                            table1forpage2.Columns[0].SetWidth(25);

                            int ss = fpspread.Sheets[0].ColumnHeader.RowCount;

                            for (int i = 0; i < 1; i++)
                            {
                                for (int j = 0; j < columncountspread; j++)
                                {
                                    if (j > 1)
                                    {
                                        testcriterianos.Add(FpSpread2.Sheets[0].ColumnHeader.Cells[i, j].Note);
                                    }
                                    string coldata = FpSpread2.Sheets[0].ColumnHeader.Cells[i, j].Text;
                                    table1forpage2.Cell(i, j).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1forpage2.Cell(i, j).SetContent(coldata);
                                    table1forpage2.Columns[j].SetWidth(30);
                                }
                            }

                            table1forpage2.Columns[0].SetWidth(60);
                            table1forpage2.Columns[1].SetWidth(40);


                            for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
                            {
                                for (int j = 0; j < columncountspread; j++)
                                {

                                    string coldata = FpSpread2.Sheets[0].Cells[i, j].Text;

                                    table1forpage2.Cell(i + 1, j).SetContentAlignment(ContentAlignment.MiddleCenter);

                                    table1forpage2.Cell(i + 1, j).SetContent(coldata);
                                }
                            }
                            table1forpage2.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, 0).SetContent("Attendance   No of Working Days / No of Present Days");
                            table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 2, 0).SetContent("Signature");
                            table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 2, 1).SetContent("Class Teacher");
                            table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 3, 1).SetContent("Principal");
                            table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 4, 1).SetContent("Parent");
                            table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, 2).SetContent("                                                        ");
                            table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 2, 2).SetContent("                                                        ");
                            table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 3, 2).SetContent("                                                        ");
                            table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 4, 2).SetContent("                                                        ");
                            string testdates = string.Empty;
                            int totalworkingdayscol = 1;
                            int lastcoldays = 0;
                            string totaldays_presentdays = string.Empty;
                            if (Convert.ToInt32(currentsem) >= 1)
                            {
                                int term = Convert.ToInt32(dropterm.SelectedItem.Text);
                                for (int i = 1; i <= term; i++)
                                {
                                    string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + i + "' and degree_code='" + degreecode + "' and batch_year='" + batchyear + "'";
                                    dset = da.select_method_wo_parameter(sem, "Text");

                                    if (dset.Tables[0].Rows.Count > 0)
                                    {
                                        string startdate = Convert.ToString(dset.Tables[0].Rows[0]["start_date"]);
                                        string enddate = Convert.ToString(dset.Tables[0].Rows[0]["end_date"]);
                                        for (int ij = lastcoldays; ij < testcriterianos.Count; ij++)
                                        {

                                            string testcriteadate = Convert.ToString(testcriterianos[ij]);
                                            lastcoldays++;
                                            totalworkingdayscol++;
                                            per_workingdays = 0;
                                            pre_present_date = 0;
                                            if (testcriteadate.Trim() != "")
                                            {
                                                testcriteadate = "select CONVERT(VARCHAR(30),exam_date,111) as exam_date  from Exam_type where criteria_no='" + testcriteadate + "'";
                                                testdates = d2.GetFunction(testcriteadate);
                                                persentmonthcal(Roll_No, admdate, startdate, testdates);
                                                totaldays_presentdays = Convert.ToString(per_workingdays) + " / " + Convert.ToString(pre_present_date);
                                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, totalworkingdayscol).SetContent(totaldays_presentdays);
                                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, totalworkingdayscol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            }
                                            else
                                            {
                                                persentmonthcal(Roll_No, admdate, startdate, enddate);
                                                totaldays_presentdays = Convert.ToString(per_workingdays) + " / " + Convert.ToString(pre_present_date);
                                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, totalworkingdayscol).SetContent(totaldays_presentdays);
                                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, totalworkingdayscol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                goto nextterm;
                                            }

                                        }
                                    nextterm: ;
                                        //string enddate =  Convert.ToString(dset.Tables[0].Rows[0]["end_date"]);

                                    }



                                }
                            }

                            //for(int i=0;i<testcriterianos.Count;i++)
                            //{

                            //    string testcriteadate =  Convert.ToString(testcriterianos[i]);
                            //    if (testcriteadate.Trim() != "")
                            //    {
                            //        testcriteadate = "select exam_date from Exam_type where criteria_no='" + testcriteadate + "'";
                            //        testdates = d2.GetFunction(testcriteadate);
                            //        persentmonthcal(Roll_No,admdate;
                            //            // lbltot_att1 =  Convert.ToString(pre_present_date);
                            //            //lbltot_work1 =  Convert.ToString(per_workingdays);
                            //            //working1 =  Convert.ToString(pre_present_date);
                            //            //present1 =  Convert.ToString(per_workingdays);
                            //    }

                            //}
                            //table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, 0).SetContent

                            foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 2, 0, FpSpread2.Sheets[0].RowCount + 2, 0).Cells)
                            {
                                pr.RowSpan = 2;
                            }
                            foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 2, 2, FpSpread2.Sheets[0].RowCount + 2, 2).Cells)
                            {
                                pr.ColSpan = 2;
                            }
                            foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 3, 2, FpSpread2.Sheets[0].RowCount + 3, 2).Cells)
                            {
                                pr.ColSpan = 2;
                            }
                            foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 4, 2, FpSpread2.Sheets[0].RowCount + 4, 2).Cells)
                            {
                                pr.ColSpan = 2;
                            }
                            table1forpage2.Columns[fpspread.Sheets[0].ColumnCount - 2].SetWidth(30);


                            Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 85, 800, 600));
                            mypdfpage1.Add(newpdftabpage2);
                            mypdfpage1.SaveToDocument();
                        }

                    }
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "rankcard" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";

                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void persentmonthcal(string rollno, string admdate, string fdate, string tdate)
    {
        per_njdate = 0;
        njdate = 0;
        pre_present_date = 0; Present = 0; njdate = 0;
        per_per_hrs = 0;
        tot_per_hrs = 0;
        per_absent_date = 0;
        Absent = 0;
        pre_ondu_date = 0; Onduty = 0;
        pre_leave_date = 0;
        Leave = 0;
        per_workingdays = 0; workingdays = 0;
        per_njdate = 0;

        per_workingdays1 = 0;
        mng_conducted_half_days = 0;
        fnhrs = 0; evng_conducted_half_days = 0;
        NoHrs = 0;
        fnhrs = 0;
        notconsider_value = 0;


        DAccess2 da = new DAccess2();
        DataSet ds = new DataSet();
        DataSet dsondutyval = new DataSet();
        Boolean isadm = false;
        hatonduty.Clear();
        try
        {
            per_abshrs_spl = 0;
            tot_per_hrs_spl = 0;
            per_leave = 0;
            tot_conduct_hr_spl = 0;
            tot_ondu_spl = 0;
            tot_ml_spl = 0;
            int my_un_mark = 0;
            int njdate_mng = 0, njdate_evng = 0;
            int per_holidate_mng = 0, per_holidate_evng = 0;

            mng_conducted_half_days = 0;
            evng_conducted_half_days = 0;

            notconsider_value = 0;

            string frdate = fdate;
            string todate = tdate;
            string[] spf = frdate.Split('/');
            string[] spt = todate.Split('/');
            cal_from_date = Convert.ToInt32(spf[0]) * 12 + Convert.ToInt32(spf[1]);
            cal_to_date = Convert.ToInt32(spt[0]) * 12 + Convert.ToInt32(spt[1]);

            per_from_date = Convert.ToDateTime(frdate);
            per_to_date = Convert.ToDateTime(todate);
            dumm_from_date = Convert.ToDateTime(frdate);    //"2014-12-01"

            // admdate =  Convert.ToString(ds4.Tables[0].Rows[rows_count]["adm_date"]);
            string[] admdatesp = admdate.Split(new Char[] { '/' });
            admdate = Convert.ToString(admdatesp[2]) + "/" + Convert.ToString(admdatesp[1]) + "/" + Convert.ToString(admdatesp[0]);
            Admission_date = Convert.ToDateTime(admdate);

            hat.Clear();
            hat.Add("std_rollno", rollno);
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds2 = da.select_method("STUD_ATTENDANCE", hat, "sp");
            mmyycount = ds2.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
            if (rows_count == 0)
            {
                hat.Clear();
                hat.Add("degree_code", int.Parse(degreecode));
                hat.Add("sem", int.Parse(currentsem));
                hat.Add("from_date", Convert.ToString(frdate));
                hat.Add("to_date", Convert.ToString(todate));
                hat.Add("coll_code", int.Parse(Convert.ToString(collegecode)));

                int iscount = 0;
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + Convert.ToString(frdate) + "' and '" + Convert.ToString(todate) + "' and degree_code=" + degreecode + " and semester=" + currentsem + "";
                DataSet dsholiday = da.select_method_wo_parameter(sqlstr_holiday, "Text");
                if (dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(Convert.ToString(dsholiday.Tables[0].Rows[0]["cnt"]));
                }
                hat.Add("iscount", iscount);

                ds3 = da.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");

                hat.Clear();
                hat.Add("degree_code", degreecode);
                hat.Add("sem_ester", int.Parse(currentsem));
                ds = da.select_method("period_attnd_schedule", hat, "sp");
                if (ds.Tables[0].Rows.Count != 0)
                {
                    NoHrs = int.Parse(Convert.ToString(ds.Tables[0].Rows[0]["PER DAY"]));
                    fnhrs = int.Parse(Convert.ToString(ds.Tables[0].Rows[0]["I_HALF_DAY"]));
                    anhrs = int.Parse(Convert.ToString(ds.Tables[0].Rows[0]["II_HALF_DAY"]));
                    minpresI = int.Parse(Convert.ToString(ds.Tables[0].Rows[0]["MIN PREE I DAY"]));
                    minpresII = int.Parse(Convert.ToString(ds.Tables[0].Rows[0]["MIN PREE II DAY"]));
                }
                hat.Clear();
                hat.Add("colege_code", Convert.ToString(collegecode));
                ds1 = da.select_method("ATT_MASTER_SETTING", hat, "sp");
                count = ds1.Tables[0].Rows.Count;

                DataSet dsondutyva = new DataSet();

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
                        if (Convert.ToString(ds3.Tables[0].Rows[0]["halforfull"]) == "False")
                        {
                            halforfull = "0";
                        }
                        else
                        {
                            halforfull = "1";
                        }
                        if (Convert.ToString(ds3.Tables[0].Rows[0]["morning"]) == "False")
                        {
                            mng = "0";
                        }
                        else
                        {
                            mng = "1";
                        }
                        if (Convert.ToString(ds3.Tables[0].Rows[0]["evening"]) == "False")
                        {
                            evng = "0";
                        }
                        else
                        {
                            evng = "1";
                        }

                        holiday_sched_details = halforfull + "*" + mng + "*" + evng;

                        string[] split_date_time1 = Convert.ToString(ds3.Tables[0].Rows[k]["HOLI_DATE"]).Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        if (!holiday_table11.Contains((Convert.ToString(Convert.ToInt16(dummy_split[2]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[1]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[0])))))
                        {
                            holiday_table11.Add((Convert.ToString(Convert.ToInt16(dummy_split[2]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[1]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[0]))), holiday_sched_details);
                        }

                    }
                }

                if (ds3.Tables[1].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
                    {
                        string[] split_date_time1 = Convert.ToString(ds3.Tables[1].Rows[k]["HOLI_DATE"]).Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        holiday_table21.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);

                        if (Convert.ToString(ds3.Tables[1].Rows[k]["halforfull"]) == "False")
                        {
                            halforfull = "0";
                        }
                        else
                        {
                            halforfull = "1";
                        }
                        if (Convert.ToString(ds3.Tables[1].Rows[k]["morning"]) == "False")
                        {
                            mng = "0";
                        }
                        else
                        {
                            mng = "1";
                        }
                        if (Convert.ToString(ds3.Tables[1].Rows[k]["evening"]) == "False")
                        {
                            evng = "0";
                        }
                        else
                        {
                            evng = "1";
                        }

                        holiday_sched_details = halforfull + "*" + mng + "*" + evng;

                        if (!holiday_table11.ContainsKey((Convert.ToString(Convert.ToInt16(dummy_split[2]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[1]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[0])))))
                        {
                            holiday_table11.Add((Convert.ToString(Convert.ToInt16(dummy_split[2]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[1]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[0]))), holiday_sched_details);
                        }
                        if (!holiday_table2.ContainsKey((Convert.ToString(Convert.ToInt16(dummy_split[2]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[1]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[0])))))
                        {
                            holiday_table2.Add((Convert.ToString(Convert.ToInt16(dummy_split[2]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[1]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[0]))), k);
                        }
                    }
                }

                if (ds3.Tables[2].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[2].Rows.Count; k++)
                    {
                        string[] split_date_time1 = Convert.ToString(ds3.Tables[2].Rows[k]["HOLI_DATE"]).Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        if (!holiday_table31.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                        {
                            holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                        }

                        if (Convert.ToString(ds3.Tables[2].Rows[k]["halforfull"]) == "False")
                        {
                            halforfull = "0";
                        }
                        else
                        {
                            halforfull = "1";
                        }
                        if (Convert.ToString(ds3.Tables[2].Rows[k]["morning"]) == "False")
                        {
                            mng = "0";
                        }
                        else
                        {
                            mng = "1";
                        }
                        if (Convert.ToString(ds3.Tables[2].Rows[k]["evening"]) == "False")
                        {
                            evng = "0";
                        }
                        else
                        {
                            evng = "1";
                        }

                        holiday_sched_details = halforfull + "*" + mng + "*" + evng;

                        if (!holiday_table11.ContainsKey((Convert.ToString(Convert.ToInt16(dummy_split[2]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[1]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[0])))))
                        {
                            holiday_table11.Add((Convert.ToString(Convert.ToInt16(dummy_split[2]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[1]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[0]))), holiday_sched_details);
                        }
                        if (holiday_table3.ContainsKey((Convert.ToString(Convert.ToInt16(dummy_split[2]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[1]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[0])))))
                        {
                            holiday_table3.Add((Convert.ToString(Convert.ToInt16(dummy_split[2]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[1]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[0]))), k);
                        }
                    }
                }
            }

            //------------------------------------------------------------------
            if (ds3.Tables[0].Rows.Count != 0)
            {
                ts = DateTime.Parse(Convert.ToString(ds3.Tables[0].Rows[0]["HOLI_DATE"])).Subtract(DateTime.Parse(Convert.ToString(dumm_from_date)));
                diff_date = Convert.ToString(ts.Days);
                dif_date1 = double.Parse(Convert.ToString(diff_date));
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

                        }

                        for (int i = 1; i <= mmyycount; i++)
                        {
                            ds2.Tables[0].DefaultView.RowFilter = "month_year='" + cal_from_date + "' and roll_no='" + rollno + "'";
                            DataView dvattvalue = ds2.Tables[0].DefaultView;
                            if (dvattvalue.Count > 0)//Added by srinath 13/10/2014
                            {

                                if (cal_from_date == int.Parse(Convert.ToString(dvattvalue[0]["month_year"])))
                                {
                                    string[] split_date_time1 = Convert.ToString(dumm_from_date).Split(' ');
                                    string[] dummy_split = split_date_time1[0].Split('/');


                                    if (!holiday_table11.ContainsKey(Convert.ToString((Convert.ToInt16(dummy_split[1]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[0]))) + "/" + (Convert.ToString(Convert.ToInt16(dummy_split[2])))))
                                    {
                                        holiday_table11.Add((Convert.ToString((Convert.ToInt16(dummy_split[1]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[0]))) + "/" + (Convert.ToString(Convert.ToInt16(dummy_split[2])))), "3*0*0");
                                    }

                                    if (holiday_table11.Contains(Convert.ToString((Convert.ToInt16(dummy_split[1]))) + "/" + Convert.ToString((Convert.ToInt16(dummy_split[0]))) + "/" + (Convert.ToString(Convert.ToInt16(dummy_split[2])))))
                                    {
                                        value_holi_status = Convert.ToString(GetCorrespondingKey(Convert.ToString(dummy_split[1]) + "/" + Convert.ToString(dummy_split[0]) + "/" + Convert.ToString(dummy_split[2]), holiday_table11));
                                        split_holiday_status = value_holi_status.Split('*');

                                        if (Convert.ToString(split_holiday_status[0]) == "3")//=========ful day working day
                                        {
                                            split_holiday_status_1 = "1";
                                            split_holiday_status_2 = "1";
                                        }
                                        else if (Convert.ToString(split_holiday_status[0]) == "1")//=============half day working day
                                        {
                                            if (Convert.ToString(split_holiday_status[1]) == "1")//==============mng holiday//evng working day
                                            {
                                                split_holiday_status_1 = "0";
                                                split_holiday_status_2 = "1";
                                            }

                                            if (Convert.ToString(split_holiday_status[2]) == "1")//==============evng holiday//mng working day
                                            {
                                                split_holiday_status_1 = "1";
                                                split_holiday_status_2 = "0";
                                            }
                                        }
                                        else if (Convert.ToString(split_holiday_status[0]) == "0")
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
                                            ts = DateTime.Parse(Convert.ToString(ds3.Tables[1].Rows[rowcount]["HOLI_DATE"])).Subtract(DateTime.Parse(Convert.ToString(dumm_from_date)));
                                            diff_date = Convert.ToString(ts.Days);
                                            dif_date = double.Parse(Convert.ToString(diff_date));
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
                                            ts = DateTime.Parse(Convert.ToString(ds3.Tables[2].Rows[0]["HOLI_DATE"])).Subtract(DateTime.Parse(Convert.ToString(dumm_from_date)));
                                            diff_date = Convert.ToString(ts.Days);
                                            dif_date = double.Parse(Convert.ToString(diff_date));
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
                                                date = "d" + Convert.ToString(dumm_from_date.Day) + "d" + Convert.ToString(i);

                                                value = Convert.ToString(dvattvalue[0][date]);
                                                //Added by srinath 31/1/2014=========Start
                                                if (value != null && value != "0" && value != "7" && value != "")
                                                {
                                                    if (tempvalue != value)
                                                    {
                                                        tempvalue = value;
                                                        for (int j = 0; j < count; j++)
                                                        {

                                                            if (Convert.ToString(ds1.Tables[0].Rows[j]["LeaveCode"]) == Convert.ToString(value))
                                                            {
                                                                ObtValue = int.Parse(Convert.ToString(ds1.Tables[0].Rows[j]["CalcFlag"]));
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
                                                        // tot_ondu += 1;

                                                    }
                                                    else if (value == "10")
                                                    {
                                                        per_leave += 1;
                                                    }
                                                    else if (value == "4")
                                                    {
                                                        //tot_ml += 1;
                                                    }

                                                }
                                                else if (value == "7")
                                                {
                                                    per_hhday += 1;

                                                }
                                                else
                                                {

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

                                            }
                                            else
                                            {
                                                // dum_unmark = temp_unmark;
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
                                                date = "d" + Convert.ToString(dumm_from_date.Day) + "d" + Convert.ToString(i);
                                                value = Convert.ToString(dvattvalue[0][date]);
                                                if (value != null && value != "0" && value != "7" && value != "")
                                                {
                                                    if (tempvalue != value)
                                                    {
                                                        tempvalue = value;
                                                        for (int j = 0; j < count; j++)
                                                        {

                                                            if (Convert.ToString(ds1.Tables[0].Rows[j]["LeaveCode"]) == Convert.ToString(value))
                                                            {
                                                                ObtValue = int.Parse(Convert.ToString(ds1.Tables[0].Rows[j]["CalcFlag"]));
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
                                                        // tot_ondu += 1;
                                                    }
                                                    else if (value == "10")
                                                    {
                                                        per_leave += 1;
                                                    }
                                                    if (value == "4")
                                                    {
                                                        //  tot_ml += 1;
                                                    }
                                                }
                                                else if (value == "7")
                                                {
                                                    per_hhday += 1;
                                                }
                                                else
                                                {

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


                                            }
                                            else
                                            {

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


            per_njdate = njdate;
            pre_present_date = Present - njdate;
            per_per_hrs = tot_per_hrs;
            per_absent_date = Absent;
            pre_ondu_date = Onduty;
            pre_leave_date = Leave;
            per_workingdays = workingdays - per_njdate;

            per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value;

            lbltot_att2 = Convert.ToString(pre_present_date);
            lbltot_work2 = Convert.ToString(per_workingdays);
            working = Convert.ToString(per_workingdays);
            present = Convert.ToString(pre_present_date);

        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (Convert.ToString(e.Key) == Convert.ToString(key))
            {
                return e.Value;
            }
        }
        return null;
    }

    protected void btnrpt_Click(object sender, EventArgs e)
    {
        try
        {
            rollnos = string.Empty;
            int checkedcount = 0;
            FpSpread1.SaveChanges();
            lblErr.Text = string.Empty;
            lblErr.Visible = false;
            lblerrormsg.Visible = false;

            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 1].Value) == 1)
                {
                    checkedcount++;
                    if (rollnos == "")
                    {
                        rollnos = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                    }
                    else
                    {
                        rollnos = rollnos + "','" + Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                    }

                }

            }
            if (rollnos != "")
            {
                if (Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 0)
                {
                    bindbutn(rollnos);
                }
                else if (Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 1)
                {
                    bindbutn_two(rollnos);
                }
                else if (Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 2)
                {
                    bindbutn_three(rollnos);
                }
                else if (Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 3)
                {
                    bindbutn_four(rollnos);
                }
                else if (Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 4)
                {
                    btnmatric_page1.Visible = true;
                    btnrpt.Visible = false;
                    btngrade.Visible = false;
                }
            }
            else
            {
                lblErr.Text = "Please Select Any One Record";
                lblErr.Visible = true;
            }
            fpspread.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void bindbutn_matricp2(string rollno)
    {

        try
        {
            if (dropterm.SelectedItem.Text == "1")
            {
                Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                Gios.Pdf.PdfPage mypdfpage;
                Gios.Pdf.PdfPage mypdfpage1;
                ArrayList testcriterianos = new ArrayList();
                rollnos = rollno;
                if (rollnos != "")
                {
                    sql = "select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No in ('" + rollnos + "') ;";
                    studgradeds.Clear();
                    studgradeds = d2.select_method_wo_parameter(sql, "text");
                    if (studgradeds.Tables[0].Rows.Count > 0)
                    {

                        for (int roll = 0; roll < studgradeds.Tables[0].Rows.Count; roll++)
                        {
                            string rcrollno = string.Empty;
                            rcrollno = Convert.ToString(studgradeds.Tables[0].Rows[roll][1]);

                            bindstudentdetails_matric(rcrollno);

                            DataSet ds = new DataSet();
                            DataSet dschool = new DataSet();
                            DAccess2 da = new DAccess2();
                            DataSet dset = new DataSet();
                            string college_code = Convert.ToString(collegecode);
                            string stdappno = string.Empty;
                            System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
                            System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                            System.Drawing.Font Fontbold1 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
                            System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
                            System.Drawing.Font Fontlarge = new System.Drawing.Font("Book Antiqua", 26, FontStyle.Regular);
                            System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                            System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 7, FontStyle.Regular);
                            System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
                            System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
                            System.Drawing.Font Fontsmallb = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                            System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);

                            Gios.Pdf.PdfTable table1;
                            System.Drawing.Font Fontbold9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold);

                            string Roll_No = rcrollno;
                            sql = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No='" + Roll_No + "'";
                            ds.Clear();
                            ds.Dispose();
                            ds = da.select_method_wo_parameter(sql, "Text");
                            DataView dv = new DataView();
                            ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                            dv = ds.Tables[1].DefaultView;
                            int count4 = 0;
                            count4 = dv.Count;

                            if (ds.Tables[0].Rows.Count > 0 && count4 > 0)
                            {
                                string currentsem = Convert.ToString(dv[0]["Current_Semester"]);
                                string batchyear = Convert.ToString(dv[0]["Batch_Year"]);
                                string degreecode = Convert.ToString(dv[0]["degree_code"]);
                                stdappno = Convert.ToString(dv[0]["App_No"]);
                                string allsem = "1";
                                string admdate = Convert.ToString(dv[0]["adm_date"]);
                                string section = string.Empty;
                                string stdcc = string.Empty;
                                stdcc = Convert.ToString(ds.Tables[1].Rows[0]["Dept_Name"]);
                                section = Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
                                string lblclassq1 = "CLASS X";
                                if (Convert.ToInt32(currentsem) > 1)
                                {
                                    int term = Convert.ToInt32(currentsem);
                                    for (int i = 2; i <= term; i++)
                                    {
                                        allsem = allsem + "'" + "," + "'" + i;
                                    }
                                }

                                if (Convert.ToInt32(currentsem) >= 1)
                                {
                                    int term = Convert.ToInt32(currentsem);
                                    for (int i = 1; i <= term; i++)
                                    {
                                        string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + i + "' and degree_code='" + degreecode + "' and batch_year='" + batchyear + "'";
                                        dset = da.select_method_wo_parameter(sem, "Text");

                                        if (dset.Tables[0].Rows.Count > 0)
                                        {
                                            string startdate = Convert.ToString(dset.Tables[0].Rows[0]["start_date"]);
                                            string enddate = Convert.ToString(dset.Tables[0].Rows[0]["end_date"]);
                                            persentmonthcal(Roll_No, admdate, startdate, enddate);
                                        }

                                        if (i == 1)
                                        {
                                            lbltot_att1 = Convert.ToString(pre_present_date);
                                            lbltot_work1 = Convert.ToString(per_workingdays);
                                            working1 = Convert.ToString(pre_present_date);
                                            present1 = Convert.ToString(per_workingdays);
                                        }
                                    }
                                }

                                string table1sql = "select * from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'  and semester = '" + currentsem + "' ; SELECT COUNT(*) as facount FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "'); SELECT Istype,CRITERIA_NO,y.semester FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "') ; SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "')";
                                DataSet ds1fortable1 = new DataSet();
                                ds1fortable1.Clear();
                                ds1fortable1.Dispose();
                                ds1fortable1 = da.select_method_wo_parameter(table1sql, "Text");
                                DataView dvforpage2 = new DataView();

                                string dob = Convert.ToString(dv[0]["dob"]);
                                string[] dobspit = dob.Split('/');
                                string addressline1 = Convert.ToString(dv[0]["parent_addressP"]);
                                addressline1 = addressline1 + " " + Convert.ToString(dv[0]["Streetp"]);
                                string addressline2 = Convert.ToString(dv[0]["Cityp"]);
                                addressline2 = addressline2 + " - " + Convert.ToString(dv[0]["parent_pincodep"]);
                                string mobileno = Convert.ToString(dv[0]["student_mobile"]);


                                PdfTextArea pdf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 420, 20, 420, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(ds.Tables[0].Rows[0][0]) + "");
                                PdfTextArea pdf11 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 420, 40, 420, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(ds.Tables[0].Rows[0][1]) + "");
                                string address = Convert.ToString(ds.Tables[0].Rows[0][2]) + "" + Convert.ToString(ds.Tables[0].Rows[0][3]) + "" + Convert.ToString(ds.Tables[0].Rows[0][4]);
                                PdfTextArea pdf12 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 420, 55, 420, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
                                PdfTextArea pdf172 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 420, 70, 420, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  :" + Convert.ToString(ds.Tables[0].Rows[0][6]));

                                PdfArea pa2 = new PdfArea(mydoc, 510, 165, 224, 40);
                                PdfRectangle pr2 = new PdfRectangle(mydoc, pa2, Color.Black);

                                string sqlschool = "select value from Master_Settings where settings='Academic year'";
                                dschool = da.select_method_wo_parameter(sqlschool, "Text");
                                string splitvalue = Convert.ToString(dschool.Tables[0].Rows[0]["value"]);
                                string[] dsplit = splitvalue.Split(',');

                                string fvalue = Convert.ToString(dsplit[0]);
                                string lvalue = Convert.ToString(dsplit[1]);
                                string acdmic_date = fvalue + " - " + lvalue;

                                PdfTextArea pdf14;
                                PdfTextArea pdf13 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 520, 177, 204, 30), System.Drawing.ContentAlignment.TopLeft, "     PROGRESS REPORT");
                                if (Convert.ToString(dv[0]["Sections"]) != "")
                                {
                                    pdf14 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 480, 220, 595, 50), System.Drawing.ContentAlignment.TopLeft, "                   " + lblclassq1 + " - " + Convert.ToString(dv[0]["Sections"]));
                                }
                                else
                                {
                                    pdf14 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 480, 220, 595, 50), System.Drawing.ContentAlignment.TopLeft, "                       " + lblclassq1);
                                }
                                PdfTextArea pdf15 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 480, 240, 595, 50), System.Drawing.ContentAlignment.TopLeft, "                      " + acdmic_date);
                                PdfTextArea pdf116 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 345, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Exam No. ");
                                PdfTextArea pdf118 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 490, 345, 595, 50), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                                PdfTextArea pdf118a1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 495, 345, 595, 50), System.Drawing.ContentAlignment.TopLeft, "  ");

                                PdfTextArea pdf18a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 280, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Name of the Pupil");
                                PdfTextArea pdf1822 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 542, 280, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["stud_name"]) + "");
                                PdfTextArea pdfee = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 528, 280, 595, 50), System.Drawing.ContentAlignment.TopLeft, " __________________________________________");

                                PdfTextArea pdf111 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Class");
                                PdfTextArea pdf113 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 470, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, "____________________");
                                PdfTextArea pdf113a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 480, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, " X ");
                                PdfTextArea pdf114 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 615, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Section");
                                PdfTextArea pdf115 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 680, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, " ");
                                PdfTextArea pdf000 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 666, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, " _____________________");

                                PdfTextArea pdf119 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 380, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Birth");
                                PdfTextArea pdf121 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 513, 380, 595, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dv[0]["dob"]));
                                PdfTextArea pdf1221 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 506, 380, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_____________" + " " + " Computer No." + " " + "____________________");
                                PdfTextArea pdf121x = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 690, 380, 595, 50), System.Drawing.ContentAlignment.TopLeft, " ");

                                PdfTextArea pdf125 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 410, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Name of the");
                                PdfTextArea pdf126 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 425, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Parent or Guardian");
                                PdfTextArea pdf126m = new PdfTextArea(Fontlarge, System.Drawing.Color.Black, new PdfArea(mydoc, 536, 410, 595, 300), System.Drawing.ContentAlignment.TopLeft, "} ");
                                PdfTextArea pdf127 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 550, 413, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________");
                                PdfTextArea pdf127a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 560, 413, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["parent_name"]) + "");

                                PdfTextArea pdf128 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 446, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Address");
                                PdfTextArea pdf130 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 483, 446, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________________________________");
                                PdfTextArea pdf130a = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 487, 446, 400, 50), System.Drawing.ContentAlignment.TopLeft, "" + addressline1 + "");

                                PdfTextArea pdf131 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 507, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Father's Phone " + " ____________________________________________");
                                PdfTextArea pdf131a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 530, 507, 595, 50), System.Drawing.ContentAlignment.TopLeft, mobileno);
                                PdfTextArea pdf132 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 477, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________________________________________");
                                PdfTextArea pdf133a = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 434, 477, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + addressline2 + "");

                                PdfTextArea pdf138 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 536, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Specimen Signature of");
                                PdfTextArea pdf138z = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 551, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Parent or Guardian");
                                PdfTextArea pdf138zz = new PdfTextArea(Fontlarge, System.Drawing.Color.Black, new PdfArea(mydoc, 547, 535, 595, 50), System.Drawing.ContentAlignment.TopLeft, " }");
                                PdfTextArea pdf139 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 578, 540, 595, 50), System.Drawing.ContentAlignment.TopLeft, " ");
                                PdfTextArea pdf14ws2 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 566, 540, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_____________________________________");



                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                {

                                }


                                Hashtable hatsubject = new Hashtable();
                                Hashtable hatcriter = new Hashtable();
                                DataTable dpdfhealth = new DataTable();
                                DataSet dhealth = new DataSet();

                                PdfArea pahealth = new PdfArea(mydoc, 30, 50, 350, 100);

                                PdfTextArea pdf46 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 160, 220, 595, 500), System.Drawing.ContentAlignment.TopLeft, "ASSESSMENT");

                                PdfTextArea pdf46z = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 60, 265, 595, 500), System.Drawing.ContentAlignment.TopLeft, "_____________________________________________");

                                table1 = mydoc.NewTable(Fontsmall1, 7, 3, 3);
                                table1.SetBorders(Color.Black, 1, BorderType.ColumnsAndBounds);

                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 0).SetContent("Grade");
                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 1).SetContent("Academic");
                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 2).SetContent("Character");
                                table1.Rows[0].SetCellPadding(9);
                                table1.Rows[1].SetCellPadding(5);
                                table1.Rows[2].SetCellPadding(5);
                                table1.Rows[3].SetCellPadding(5);
                                table1.Rows[4].SetCellPadding(5);
                                table1.Rows[5].SetCellPadding(5);
                                table1.Rows[6].SetCellPadding(5);
                                table1.Cell(0, 0).SetFont(Fontbold);
                                table1.Cell(0, 1).SetFont(Fontbold);
                                table1.Cell(0, 2).SetFont(Fontbold);

                                table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(1, 0).SetContent("E");
                                table1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(1, 1).SetContent("       Excellent");
                                table1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(1, 2).SetContent("       80 to 100 %");

                                table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(2, 0).SetContent("O");
                                table1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(2, 1).SetContent("       Outstanding");
                                table1.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(2, 2).SetContent("       70 to 79 %");

                                table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(3, 0).SetContent("A");
                                table1.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(3, 1).SetContent("       Good");
                                table1.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(3, 2).SetContent("       60 to 60 %");

                                table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(4, 0).SetContent("B");
                                table1.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(4, 1).SetContent("       Improving");
                                table1.Cell(4, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(4, 2).SetContent("       50 to 59 %");

                                table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(5, 0).SetContent("C");
                                table1.Cell(5, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(5, 1).SetContent("       Must Improve");
                                table1.Cell(5, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(5, 2).SetContent("       40 to 49 %");

                                table1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(6, 0).SetContent("D");
                                table1.Cell(6, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(6, 1).SetContent("       Undesirable");
                                table1.Cell(6, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(6, 2).SetContent("       below 40 %");
                                table1.VisibleHeaders = false;

                                Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 60, 250, 300, 500));
                                //mypdfpage.Add(newpdftabpage1);

                                //mypdfpage.Add(pdf1);
                                //mypdfpage.Add(pdf11);
                                //mypdfpage.Add(pdf12);
                                //mypdfpage.Add(pdf13);
                                //mypdfpage.Add(pdf14);
                                //mypdfpage.Add(pdf15);

                                ////////////////////total rec/////////////////
                                //PdfArea pa1 = new PdfArea(mydoc, 14, 12, 810, 575);
                                //PdfRectangle pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
                                //mypdfpage.Add(pr3);
                                ///////////////////left rec/////////////////////////

                                PdfArea pa12 = new PdfArea(mydoc, 14, 12, 390, 575);
                                PdfRectangle pr4 = new PdfRectangle(mydoc, pa12, Color.Black);
                                //mypdfpage.Add(pr4);

                                /////////////////right////////////////////////

                                PdfArea pa5 = new PdfArea(mydoc, 415, 12, 410, 575);
                                PdfRectangle pr5 = new PdfRectangle(mydoc, pa5, Color.Black);
                                //mypdfpage.Add(pr5);
                                //mypdfpage.SaveToDocument();
                                mypdfpage1 = mydoc.NewPage();
                                //=========================================
                                PdfTextArea Psw = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 15, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Name");
                                mypdfpage1.Add(Psw);

                                PdfTextArea Pqsw = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 48, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, " :");
                                mypdfpage1.Add(Pqsw);

                                PdfTextArea Pssw = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 62, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dv[0]["stud_name"]));
                                mypdfpage1.Add(Pssw);

                                //PdfTextArea Psw1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 340, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, "X Std. " +  Convert.ToString(dv[0]["Sections"]));
                                //mypdfpage1.Add(Psw1);

                                PdfTextArea Psww = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 260, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Class & Sec");
                                mypdfpage1.Add(Psww);

                                PdfTextArea Pqswe = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 325, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, " :");
                                mypdfpage1.Add(Pqswe);

                                PdfTextArea Psswu = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 337, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, stdcc + " " + Convert.ToString(dv[0]["Sections"]));
                                mypdfpage1.Add(Psswu);

                                PdfTextArea Psww1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 440, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Exam. No." + " ");
                                mypdfpage1.Add(Psww1);

                                int rowcountspread = FpSpread2.Sheets[0].RowCount + 10;
                                int columncountspread = FpSpread2.Sheets[0].ColumnCount;

                                Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(Fontsmall1, rowcountspread, columncountspread, 2);
                                table1forpage2.VisibleHeaders = false;
                                table1forpage2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table1forpage2.Columns[1].SetWidth(25);
                                table1forpage2.Columns[0].SetWidth(25);

                                int ss = fpspread.Sheets[0].ColumnHeader.RowCount;

                                for (int i = 0; i < 1; i++)
                                {
                                    for (int j = 0; j < columncountspread; j++)
                                    {
                                        if (j > 1)
                                        {
                                            testcriterianos.Add(FpSpread2.Sheets[0].ColumnHeader.Cells[i, j].Note);
                                        }
                                        string coldata = FpSpread2.Sheets[0].ColumnHeader.Cells[i, j].Text;
                                        table1forpage2.Cell(i, j).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1forpage2.Cell(i, j).SetContent(coldata);
                                        table1forpage2.Columns[j].SetWidth(20);
                                        table1forpage2.Cell(i, j).SetCellPadding(20);
                                        table1forpage2.Cell(i, j).SetFont(Fontsmall);
                                    }
                                }

                                table1forpage2.Columns[0].SetWidth(100);
                                table1forpage2.Columns[1].SetWidth(35);

                                for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
                                {
                                    for (int j = 0; j < columncountspread; j++)
                                    {
                                        string coldata = FpSpread2.Sheets[0].Cells[i, j].Text;
                                        table1forpage2.Cell(i + 1, j).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1forpage2.Cell(i + 1, j).SetContent(coldata);
                                        table1forpage2.Cell(i + 1, j).SetCellPadding(10);
                                    }
                                }

                                table1forpage2.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage2.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, 0).SetContent("Attendance No of Working Days");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, 0).SetCellPadding(10);
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 2, 0).SetContent("No of Present Days");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 2, 0).SetCellPadding(10);
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 3, 0).SetCellPadding(10);
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 4, 0).SetCellPadding(10);
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 5, 0).SetCellPadding(10);
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 6, 0).SetCellPadding(10);
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 7, 0).SetCellPadding(10);
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 8, 0).SetCellPadding(10);
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 9, 0).SetCellPadding(10);
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 3, 0).SetContent("Class Teacher");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 4, 0).SetContent("Principal");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 5, 0).SetContent("Parent / Guardian");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 6, 1).SetContent("Punctuality");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 6, 2).SetContent("Cleanliness");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 6, 4).SetContent("Conduct");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 6, 6).SetContent("Hand Writing");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 6, 8).SetContent("Remarks");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 6, 10).SetContentAlignment(ContentAlignment.BottomCenter);
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 6, 10).SetContent("Principal");

                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 7, 0).SetContent("I - Term");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 8, 0).SetContent("II - Term");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 9, 0).SetContent("III - Term");

                                string testdates = string.Empty;
                                int totalworkingdayscol = 1;
                                int lastcoldays = 0;
                                string totaldays_presentdays = string.Empty;
                                string totaldays_presentdays1 = string.Empty;

                                if (Convert.ToInt32(currentsem) >= 1)
                                {
                                    int term = Convert.ToInt32(dropterm.SelectedItem.Text);
                                    for (int i = 1; i <= term; i++)
                                    {
                                        string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + i + "' and degree_code='" + degreecode + "' and batch_year='" + batchyear + "'";
                                        dset = da.select_method_wo_parameter(sem, "Text");

                                        if (dset.Tables[0].Rows.Count > 0)
                                        {
                                            string startdate = Convert.ToString(dset.Tables[0].Rows[0]["start_date"]);
                                            string enddate = Convert.ToString(dset.Tables[0].Rows[0]["end_date"]);

                                            for (int ij = 2; ij < FpSpread2.Sheets[0].ColumnCount; ij++)
                                            {
                                                string testcriteadate = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, ij].Tag);
                                                lastcoldays++;
                                                totalworkingdayscol++;
                                                per_workingdays = 0;
                                                pre_present_date = 0;
                                                if (testcriteadate.Trim() != "")
                                                {
                                                    testcriteadate = "select CONVERT(VARCHAR(30),exam_date,111) as exam_date  from Exam_type where criteria_no='" + testcriteadate + "'";
                                                    testdates = d2.GetFunction(testcriteadate);
                                                    persentmonthcal(Roll_No, admdate, startdate, testdates);
                                                    //totaldays_presentdays =  Convert.ToString(per_workingdays) + " / " +  Convert.ToString(pre_present_date);

                                                    totaldays_presentdays = Convert.ToString(per_workingdays);
                                                    table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, totalworkingdayscol).SetContent(totaldays_presentdays);
                                                    table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, totalworkingdayscol).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                    totaldays_presentdays1 = Convert.ToString(pre_present_date);
                                                    table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 2, totalworkingdayscol).SetContent(totaldays_presentdays1);
                                                    table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 2, totalworkingdayscol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                }
                                                else
                                                {
                                                    persentmonthcal(Roll_No, admdate, startdate, enddate);
                                                    //totaldays_presentdays =  Convert.ToString(per_workingdays) + " / " +  Convert.ToString(pre_present_date);

                                                    totaldays_presentdays = Convert.ToString(per_workingdays);
                                                    table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, totalworkingdayscol).SetContent(totaldays_presentdays);
                                                    table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, totalworkingdayscol).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                    totaldays_presentdays1 = Convert.ToString(pre_present_date);
                                                    table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 2, totalworkingdayscol).SetContent(totaldays_presentdays1);
                                                    table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 2, totalworkingdayscol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    goto nextterm;
                                                }
                                            }
                                        nextterm: ;
                                        }
                                    }
                                }

                                // ---------- 1st span
                                //foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 6, 1, FpSpread2.Sheets[0].RowCount + 6, 1).Cells)
                                //{
                                //    pr.ColSpan = 2;
                                //}
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 6, 2, FpSpread2.Sheets[0].RowCount + 6, 2).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 6, 4, FpSpread2.Sheets[0].RowCount + 6, 4).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 6, 6, FpSpread2.Sheets[0].RowCount + 6, 6).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 6, 8, FpSpread2.Sheets[0].RowCount + 6, 8).Cells)
                                {
                                    pr.ColSpan = 2;
                                }

                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 6, 10, FpSpread2.Sheets[0].RowCount + 6, 10).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 6, 10, FpSpread2.Sheets[0].RowCount + 6, 10).Cells)
                                {
                                    pr.RowSpan = 4;
                                }

                                // --------- 2nd span
                                //foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 7, 1, FpSpread2.Sheets[0].RowCount + 7, 1).Cells)
                                //{
                                //    pr.ColSpan = 2;
                                //}
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 7, 2, FpSpread2.Sheets[0].RowCount + 7, 2).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 7, 4, FpSpread2.Sheets[0].RowCount + 7, 4).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 7, 6, FpSpread2.Sheets[0].RowCount + 7, 6).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 7, 8, FpSpread2.Sheets[0].RowCount + 7, 8).Cells)
                                {
                                    pr.ColSpan = 2;
                                }

                                // --------- 3rd span
                                //foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 8, 1, FpSpread2.Sheets[0].RowCount + 8, 1).Cells)
                                //{
                                //    pr.ColSpan = 2;
                                //}
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 8, 2, FpSpread2.Sheets[0].RowCount + 8, 2).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 8, 4, FpSpread2.Sheets[0].RowCount + 8, 4).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 8, 6, FpSpread2.Sheets[0].RowCount + 8, 6).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 8, 8, FpSpread2.Sheets[0].RowCount + 8, 8).Cells)
                                {
                                    pr.ColSpan = 2;
                                }

                                //foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 9, 1, FpSpread2.Sheets[0].RowCount + 9, 1).Cells)
                                //{
                                //    pr.ColSpan = 2;
                                //}
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 9, 2, FpSpread2.Sheets[0].RowCount + 9, 2).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 9, 4, FpSpread2.Sheets[0].RowCount + 9, 4).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 9, 6, FpSpread2.Sheets[0].RowCount + 9, 6).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 9, 8, FpSpread2.Sheets[0].RowCount + 9, 8).Cells)
                                {
                                    pr.ColSpan = 2;
                                }

                                table1forpage2.Columns[fpspread.Sheets[0].ColumnCount - 2].SetWidth(30);

                                Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 7, 85, 580, 800));
                                mypdfpage1.Add(newpdftabpage2);

                                mypdfpage1.SaveToDocument();
                            }
                        }
                    }
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "rankcard " + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    Response.Buffer = true;
                    Response.Clear();
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
            else
            {
                Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                //Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
                //Gios.Pdf.PdfDocument mydocback = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
                Gios.Pdf.PdfPage mypdfpage;
                Gios.Pdf.PdfPage mypdfpage1;
                //Gios.Pdf.PdfPage mypdfpage1back;
                ArrayList testcriterianos = new ArrayList();
                rollnos = rollno;
                if (rollnos != "")
                {
                    sql = "select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No in ('" + rollnos + "') ;";
                    studgradeds.Clear();
                    studgradeds = d2.select_method_wo_parameter(sql, "text");
                    if (studgradeds.Tables[0].Rows.Count > 0)
                    {

                        for (int roll = 0; roll < studgradeds.Tables[0].Rows.Count; roll++)
                        {
                            string rcrollno = string.Empty;
                            rcrollno = Convert.ToString(studgradeds.Tables[0].Rows[roll][1]);

                            bindstudentdetails(rcrollno);
                            bindstudentdetails_new(rcrollno);

                            DataSet ds = new DataSet();
                            DataSet dschool = new DataSet();
                            DAccess2 da = new DAccess2();
                            DataSet dset = new DataSet();
                            string college_code = Convert.ToString(collegecode);
                            string stdappno = string.Empty;
                            System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
                            System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                            System.Drawing.Font Fontbold1 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
                            System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
                            System.Drawing.Font Fontlarge = new System.Drawing.Font("Book Antiqua", 26, FontStyle.Regular);
                            System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                            System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 7, FontStyle.Regular);
                            System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
                            System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
                            System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);

                            Gios.Pdf.PdfTable table1;
                            System.Drawing.Font Fontbold9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold);

                            string Roll_No = rcrollno;
                            sql = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No='" + Roll_No + "'";
                            ds.Clear();
                            ds.Dispose();
                            ds = da.select_method_wo_parameter(sql, "Text");
                            DataView dv = new DataView();
                            ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                            dv = ds.Tables[1].DefaultView;
                            int count4 = 0;
                            count4 = dv.Count;

                            if (ds.Tables[0].Rows.Count > 0 && count4 > 0)
                            {
                                string currentsem = Convert.ToString(dv[0]["Current_Semester"]);
                                string batchyear = Convert.ToString(dv[0]["Batch_Year"]);
                                string degreecode = Convert.ToString(dv[0]["degree_code"]);
                                stdappno = Convert.ToString(dv[0]["App_No"]);
                                string allsem = "1";
                                string admdate = Convert.ToString(dv[0]["adm_date"]);
                                string section = string.Empty;
                                string stdcc = string.Empty;
                                stdcc = Convert.ToString(ds.Tables[1].Rows[0]["Dept_Name"]);
                                section = Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
                                string lblclassq1 = "CLASS X";
                                if (Convert.ToInt32(currentsem) > 1)
                                {
                                    int term = Convert.ToInt32(currentsem);
                                    for (int i = 2; i <= term; i++)
                                    {
                                        allsem = allsem + "'" + "," + "'" + i;
                                    }
                                }

                                if (Convert.ToInt32(currentsem) >= 1)
                                {
                                    int term = Convert.ToInt32(currentsem);
                                    for (int i = 1; i <= term; i++)
                                    {
                                        string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + i + "' and degree_code='" + degreecode + "' and batch_year='" + batchyear + "'";
                                        dset = da.select_method_wo_parameter(sem, "Text");

                                        if (dset.Tables[0].Rows.Count > 0)
                                        {
                                            string startdate = Convert.ToString(dset.Tables[0].Rows[0]["start_date"]);
                                            string enddate = Convert.ToString(dset.Tables[0].Rows[0]["end_date"]);
                                            persentmonthcal(Roll_No, admdate, startdate, enddate);
                                        }

                                        if (i == 1)
                                        {
                                            lbltot_att1 = Convert.ToString(pre_present_date);
                                            lbltot_work1 = Convert.ToString(per_workingdays);
                                            working1 = Convert.ToString(pre_present_date);
                                            present1 = Convert.ToString(per_workingdays);
                                        }
                                    }
                                }

                                string table1sql = "select * from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'  and semester = '" + currentsem + "' ; SELECT COUNT(*) as facount FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "'); SELECT Istype,CRITERIA_NO,y.semester FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "') ; SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "')";
                                DataSet ds1fortable1 = new DataSet();
                                ds1fortable1.Clear();
                                ds1fortable1.Dispose();
                                ds1fortable1 = da.select_method_wo_parameter(table1sql, "Text");
                                DataView dvforpage2 = new DataView();

                                string dob = Convert.ToString(dv[0]["dob"]);
                                string[] dobspit = dob.Split('/');
                                string addressline1 = Convert.ToString(dv[0]["parent_addressP"]);
                                addressline1 = addressline1 + " " + Convert.ToString(dv[0]["Streetp"]);
                                string addressline2 = Convert.ToString(dv[0]["Cityp"]);
                                addressline2 = addressline2 + " - " + Convert.ToString(dv[0]["parent_pincodep"]);
                                string mobileno = Convert.ToString(dv[0]["student_mobile"]);

                                //mypdfpage = mydoc.NewPage();
                                //mypdfpage1back = mydocback.NewPage();

                                PdfTextArea pdf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 420, 20, 420, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(ds.Tables[0].Rows[0][0]) + "");
                                PdfTextArea pdf11 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 420, 40, 420, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(ds.Tables[0].Rows[0][1]) + "");
                                string address = Convert.ToString(ds.Tables[0].Rows[0][2]) + "" + Convert.ToString(ds.Tables[0].Rows[0][3]) + "" + Convert.ToString(ds.Tables[0].Rows[0][4]);
                                PdfTextArea pdf12 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 420, 55, 420, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
                                PdfTextArea pdf172 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 420, 70, 420, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  :" + Convert.ToString(ds.Tables[0].Rows[0][6]));

                                PdfArea pa2 = new PdfArea(mydoc, 510, 165, 224, 40);
                                PdfRectangle pr2 = new PdfRectangle(mydoc, pa2, Color.Black);
                                //mypdfpage.Add(pr2);

                                string sqlschool = "select value from Master_Settings where settings='Academic year'";
                                dschool = da.select_method_wo_parameter(sqlschool, "Text");
                                string splitvalue = Convert.ToString(dschool.Tables[0].Rows[0]["value"]);
                                string[] dsplit = splitvalue.Split(',');

                                string fvalue = Convert.ToString(dsplit[0]);
                                string lvalue = Convert.ToString(dsplit[1]);
                                string acdmic_date = fvalue + " - " + lvalue;

                                PdfTextArea pdf14;
                                PdfTextArea pdf13 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 520, 177, 204, 30), System.Drawing.ContentAlignment.TopLeft, "     PROGRESS REPORT");
                                if (Convert.ToString(dv[0]["Sections"]) != "")
                                {
                                    pdf14 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 480, 220, 595, 50), System.Drawing.ContentAlignment.TopLeft, "                   " + lblclassq1 + " - " + Convert.ToString(dv[0]["Sections"]));
                                }
                                else
                                {
                                    pdf14 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 480, 220, 595, 50), System.Drawing.ContentAlignment.TopLeft, "                       " + lblclassq1);
                                }
                                PdfTextArea pdf15 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 480, 240, 595, 50), System.Drawing.ContentAlignment.TopLeft, "                      " + acdmic_date);
                                PdfTextArea pdf116 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 345, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Exam No. ");
                                PdfTextArea pdf118 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 490, 345, 595, 50), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                                PdfTextArea pdf118a1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 495, 345, 595, 50), System.Drawing.ContentAlignment.TopLeft, "  ");

                                PdfTextArea pdf18a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 280, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Name of the Pupil");
                                PdfTextArea pdf1822 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 542, 280, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["stud_name"]) + "");
                                PdfTextArea pdfee = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 528, 280, 595, 50), System.Drawing.ContentAlignment.TopLeft, " __________________________________________");

                                PdfTextArea pdf111 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Class");
                                PdfTextArea pdf113 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 470, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, "____________________");
                                PdfTextArea pdf113a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 480, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, " X ");
                                PdfTextArea pdf114 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 615, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Section");
                                PdfTextArea pdf115 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 680, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, " ");
                                PdfTextArea pdf000 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 666, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, " _____________________");

                                PdfTextArea pdf119 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 380, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Birth");
                                PdfTextArea pdf121 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 513, 380, 595, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dv[0]["dob"]));
                                PdfTextArea pdf1221 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 506, 380, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_____________" + " " + " Computer No." + " " + "____________________");
                                PdfTextArea pdf121x = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 690, 380, 595, 50), System.Drawing.ContentAlignment.TopLeft, " ");

                                PdfTextArea pdf125 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 410, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Name of the");
                                PdfTextArea pdf126 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 425, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Parent or Guardian");
                                PdfTextArea pdf126m = new PdfTextArea(Fontlarge, System.Drawing.Color.Black, new PdfArea(mydoc, 536, 410, 595, 300), System.Drawing.ContentAlignment.TopLeft, "} ");
                                PdfTextArea pdf127 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 550, 413, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________");
                                PdfTextArea pdf127a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 560, 413, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["parent_name"]) + "");

                                PdfTextArea pdf128 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 446, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Address");
                                PdfTextArea pdf130 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 483, 446, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________________________________");
                                PdfTextArea pdf130a = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 487, 446, 400, 50), System.Drawing.ContentAlignment.TopLeft, "" + addressline1 + "");

                                //PdfTextArea pdf131 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 507, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Phone " + " ___________________________________________________" + mobileno);
                                PdfTextArea pdf131 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 507, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Father's Phone " + " ____________________________________________");
                                PdfTextArea pdf131a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 530, 507, 595, 50), System.Drawing.ContentAlignment.TopLeft, mobileno);
                                PdfTextArea pdf132 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 477, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________________________________________");
                                PdfTextArea pdf133a = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 434, 477, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + addressline2 + "");

                                PdfTextArea pdf138 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 536, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Specimen Signature of");
                                PdfTextArea pdf138z = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 551, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Parent or Guardian");
                                PdfTextArea pdf138zz = new PdfTextArea(Fontlarge, System.Drawing.Color.Black, new PdfArea(mydoc, 547, 535, 595, 50), System.Drawing.ContentAlignment.TopLeft, " }");
                                PdfTextArea pdf139 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 578, 540, 595, 50), System.Drawing.ContentAlignment.TopLeft, " ");
                                PdfTextArea pdf14ws2 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 566, 540, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_____________________________________");

                                //mypdfpage.Add(pdf116);
                                //mypdfpage.Add(pdf118);
                                //mypdfpage.Add(pdf118a1);
                                //mypdfpage.Add(pdf1822);
                                //mypdfpage.Add(pdfee);
                                //mypdfpage.Add(pdf000);
                                //mypdfpage.Add(pdf18a);
                                //mypdfpage.Add(pdf111);
                                //mypdfpage.Add(pdf113);
                                //mypdfpage.Add(pdf113a);
                                //mypdfpage.Add(pdf114);
                                //mypdfpage.Add(pdf115);
                                //mypdfpage.Add(pdf172);
                                //mypdfpage.Add(pdf1221);
                                //mypdfpage.Add(pdf119);
                                //mypdfpage.Add(pdf121);
                                //mypdfpage.Add(pdf121x);
                                //mypdfpage.Add(pdf125);
                                //mypdfpage.Add(pdf126);
                                //mypdfpage.Add(pdf126m);
                                //mypdfpage.Add(pdf127);
                                //mypdfpage.Add(pdf127a);
                                //mypdfpage.Add(pdf128);
                                //mypdfpage.Add(pdf130);
                                //mypdfpage.Add(pdf130a);
                                //mypdfpage.Add(pdf131);
                                //mypdfpage.Add(pdf131a);
                                //mypdfpage.Add(pdf132);
                                //mypdfpage.Add(pdf133a);
                                //mypdfpage.Add(pdf138);
                                //mypdfpage.Add(pdf138z);
                                //mypdfpage.Add(pdf138zz);
                                //mypdfpage.Add(pdf139);
                                //mypdfpage.Add(pdf14ws2);

                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                {
                                    //Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                    //mypdfpage.Add(LogoImage1, 590, 96, 450);
                                }
                                //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                                //{
                                //    Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"));
                                //    mypdfpage.Add(LogoImage2, 750, 96, 450);
                                //}
                                //else
                                //{
                                //    Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                //    mypdfpage.Add(LogoImage2, 750, 96, 450);
                                //}

                                Hashtable hatsubject = new Hashtable();
                                Hashtable hatcriter = new Hashtable();
                                DataTable dpdfhealth = new DataTable();
                                DataSet dhealth = new DataSet();

                                PdfArea pahealth = new PdfArea(mydoc, 30, 50, 350, 100);

                                PdfTextArea pdf46 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 160, 220, 595, 500), System.Drawing.ContentAlignment.TopLeft, "ASSESSMENT");
                                //mypdfpage.Add(pdf46);

                                PdfTextArea pdf46z = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 60, 265, 595, 500), System.Drawing.ContentAlignment.TopLeft, "_____________________________________________");
                                //mypdfpage.Add(pdf46z);

                                table1 = mydoc.NewTable(Fontsmall1, 7, 3, 3);
                                table1.SetBorders(Color.Black, 1, BorderType.ColumnsAndBounds);

                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 0).SetContent("Grade");
                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 1).SetContent("Academic");
                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 2).SetContent("Character");
                                table1.Rows[0].SetCellPadding(9);
                                table1.Rows[1].SetCellPadding(5);
                                table1.Rows[2].SetCellPadding(5);
                                table1.Rows[3].SetCellPadding(5);
                                table1.Rows[4].SetCellPadding(5);
                                table1.Rows[5].SetCellPadding(5);
                                table1.Rows[6].SetCellPadding(5);
                                table1.Cell(0, 0).SetFont(Fontbold);
                                table1.Cell(0, 1).SetFont(Fontbold);
                                table1.Cell(0, 2).SetFont(Fontbold);

                                table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(1, 0).SetContent("E");
                                table1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(1, 1).SetContent("       Excellent");
                                table1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(1, 2).SetContent("       80 to 100 %");

                                table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(2, 0).SetContent("O");
                                table1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(2, 1).SetContent("       Outstanding");
                                table1.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(2, 2).SetContent("       70 to 79 %");

                                table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(3, 0).SetContent("A");
                                table1.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(3, 1).SetContent("       Good");
                                table1.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(3, 2).SetContent("       60 to 60 %");

                                table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(4, 0).SetContent("B");
                                table1.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(4, 1).SetContent("       Improving");
                                table1.Cell(4, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(4, 2).SetContent("       50 to 59 %");

                                table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(5, 0).SetContent("C");
                                table1.Cell(5, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(5, 1).SetContent("       Must Improve");
                                table1.Cell(5, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(5, 2).SetContent("       40 to 49 %");

                                table1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(6, 0).SetContent("D");
                                table1.Cell(6, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(6, 1).SetContent("       Undesirable");
                                table1.Cell(6, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(6, 2).SetContent("       below 40 %");
                                table1.VisibleHeaders = false;

                                Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 60, 250, 300, 500));
                                //mypdfpage.Add(newpdftabpage1);

                                //mypdfpage.Add(pdf1);
                                //mypdfpage.Add(pdf11);
                                //mypdfpage.Add(pdf12);
                                //mypdfpage.Add(pdf13);
                                //mypdfpage.Add(pdf14);
                                //mypdfpage.Add(pdf15);

                                ////////////////////total rec/////////////////
                                //PdfArea pa1 = new PdfArea(mydoc, 14, 12, 810, 575);
                                //PdfRectangle pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
                                //mypdfpage.Add(pr3);
                                ///////////////////left rec/////////////////////////

                                PdfArea pa12 = new PdfArea(mydoc, 14, 12, 390, 575);
                                PdfRectangle pr4 = new PdfRectangle(mydoc, pa12, Color.Black);
                                //mypdfpage.Add(pr4);

                                /////////////////right////////////////////////

                                PdfArea pa5 = new PdfArea(mydoc, 415, 12, 410, 575);
                                PdfRectangle pr5 = new PdfRectangle(mydoc, pa5, Color.Black);
                                //mypdfpage.Add(pr5);
                                //mypdfpage.SaveToDocument();
                                mypdfpage1 = mydoc.NewPage();
                                //=========================================
                                PdfTextArea Psw = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 15, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Name");
                                mypdfpage1.Add(Psw);

                                PdfTextArea Pqsw = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 48, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, " :");
                                mypdfpage1.Add(Pqsw);

                                PdfTextArea Pssw = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 62, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dv[0]["stud_name"]));
                                mypdfpage1.Add(Pssw);

                                //PdfTextArea Psw1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 340, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, "X Std. " +  Convert.ToString(dv[0]["Sections"]));
                                //mypdfpage1.Add(Psw1);

                                PdfTextArea Psww = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 260, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Class & Sec");
                                mypdfpage1.Add(Psww);

                                PdfTextArea Pqswe = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 325, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, " :");
                                mypdfpage1.Add(Pqswe);

                                PdfTextArea Psswu = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 337, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, stdcc + " " + Convert.ToString(dv[0]["Sections"]));
                                mypdfpage1.Add(Psswu);

                                PdfTextArea Psww1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 440, 50, 595, 500), System.Drawing.ContentAlignment.TopLeft, "Exam. No." + " ");
                                mypdfpage1.Add(Psww1);

                                int rowcountspread = FpSpread2.Sheets[0].RowCount + 10;
                                int columncountspread = FpSpread2.Sheets[0].ColumnCount;

                                Gios.Pdf.PdfTable table1forpage2 = mydoc.NewTable(Fontsmall1, rowcountspread, columncountspread, 4);
                                table1forpage2.VisibleHeaders = false;
                                table1forpage2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table1forpage2.Columns[1].SetWidth(25);
                                table1forpage2.Columns[0].SetWidth(25);

                                int ss = fpspread.Sheets[0].ColumnHeader.RowCount;

                                for (int i = 0; i < 1; i++)
                                {
                                    for (int j = 0; j < columncountspread; j++)
                                    {
                                        if (j > 1)
                                        {
                                            testcriterianos.Add(FpSpread2.Sheets[0].ColumnHeader.Cells[i, j].Note);
                                        }
                                        string coldata = FpSpread2.Sheets[0].ColumnHeader.Cells[i, j].Text;
                                        table1forpage2.Cell(i, j).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1forpage2.Cell(i, j).SetContent(coldata);
                                        table1forpage2.Columns[j].SetWidth(30);
                                    }
                                }

                                table1forpage2.Columns[0].SetWidth(70);
                                table1forpage2.Columns[1].SetWidth(40);

                                for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
                                {
                                    for (int j = 0; j < columncountspread; j++)
                                    {
                                        string coldata = FpSpread2.Sheets[0].Cells[i, j].Text;
                                        table1forpage2.Cell(i + 1, j).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1forpage2.Cell(i + 1, j).SetContent(coldata);
                                    }
                                }

                                table1forpage2.Columns[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1forpage2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage2.Columns[1].SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1forpage2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, 0).SetContent("Attendance No of Working Days");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 2, 0).SetContent("No of Present Days");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 3, 0).SetContent("Class Teacher");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 4, 0).SetContent("Principal");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 5, 0).SetContent("Parent / Guardian");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 6, 1).SetContent("Punctuality");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 6, 4).SetContent("Cleanliness");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 6, 7).SetContent("Conduct");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 6, 10).SetContent("Hand Writing");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 6, 13).SetContent("Remarks");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 6, 16).SetContentAlignment(ContentAlignment.BottomCenter);
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 6, 16).SetContent("Principal");

                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 7, 0).SetContent("I - Term");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 8, 0).SetContent("II - Term");
                                table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 9, 0).SetContent("III - Term");

                                string testdates = string.Empty;
                                int totalworkingdayscol = 1;
                                int lastcoldays = 0;
                                string totaldays_presentdays = string.Empty;
                                string totaldays_presentdays1 = string.Empty;

                                if (Convert.ToInt32(currentsem) >= 1)
                                {
                                    int term = Convert.ToInt32(dropterm.SelectedItem.Text);
                                    for (int i = 1; i <= term; i++)
                                    {
                                        string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + i + "' and degree_code='" + degreecode + "' and batch_year='" + batchyear + "'";
                                        dset = da.select_method_wo_parameter(sem, "Text");

                                        if (dset.Tables[0].Rows.Count > 0)
                                        {
                                            string startdate = Convert.ToString(dset.Tables[0].Rows[0]["start_date"]);
                                            string enddate = Convert.ToString(dset.Tables[0].Rows[0]["end_date"]);

                                            for (int ij = lastcoldays; ij < testcriterianos.Count; ij++)
                                            {
                                                string testcriteadate = Convert.ToString(testcriterianos[ij]);
                                                lastcoldays++;
                                                totalworkingdayscol++;
                                                per_workingdays = 0;
                                                pre_present_date = 0;
                                                if (testcriteadate.Trim() != "")
                                                {
                                                    testcriteadate = "select CONVERT(VARCHAR(30),exam_date,111) as exam_date  from Exam_type where criteria_no='" + testcriteadate + "'";
                                                    testdates = d2.GetFunction(testcriteadate);
                                                    persentmonthcal(Roll_No, admdate, startdate, testdates);
                                                    Convert.ToString(pre_present_date);
                                                    totaldays_presentdays = Convert.ToString(per_workingdays);
                                                    table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, totalworkingdayscol).SetContent(totaldays_presentdays);
                                                    table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, totalworkingdayscol).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                    totaldays_presentdays1 = Convert.ToString(pre_present_date);
                                                    table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 2, totalworkingdayscol).SetContent(totaldays_presentdays1);
                                                    table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 2, totalworkingdayscol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                }
                                                else
                                                {
                                                    persentmonthcal(Roll_No, admdate, startdate, enddate);
                                                    Convert.ToString(pre_present_date);
                                                    totaldays_presentdays = Convert.ToString(per_workingdays);
                                                    table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, totalworkingdayscol).SetContent(totaldays_presentdays);
                                                    table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 1, totalworkingdayscol).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                    totaldays_presentdays1 = Convert.ToString(pre_present_date);
                                                    table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 2, totalworkingdayscol).SetContent(totaldays_presentdays1);
                                                    table1forpage2.Cell(FpSpread2.Sheets[0].RowCount + 2, totalworkingdayscol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    goto nextterm;
                                                }
                                            }
                                        nextterm: ;
                                        }
                                    }
                                }

                                // ---------- 1st span
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 6, 1, FpSpread2.Sheets[0].RowCount + 6, 1).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 6, 4, FpSpread2.Sheets[0].RowCount + 6, 4).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 6, 7, FpSpread2.Sheets[0].RowCount + 6, 7).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 6, 10, FpSpread2.Sheets[0].RowCount + 6, 10).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 6, 13, FpSpread2.Sheets[0].RowCount + 6, 13).Cells)
                                {
                                    pr.ColSpan = 3;
                                }

                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 6, 16, FpSpread2.Sheets[0].RowCount + 6, 16).Cells)
                                {
                                    pr.ColSpan = 6;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 6, 16, FpSpread2.Sheets[0].RowCount + 6, 16).Cells)
                                {
                                    pr.RowSpan = 4;
                                }

                                // --------- 2nd span
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 7, 1, FpSpread2.Sheets[0].RowCount + 7, 1).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 7, 4, FpSpread2.Sheets[0].RowCount + 7, 4).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 7, 7, FpSpread2.Sheets[0].RowCount + 7, 7).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 7, 10, FpSpread2.Sheets[0].RowCount + 7, 10).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 7, 13, FpSpread2.Sheets[0].RowCount + 7, 13).Cells)
                                {
                                    pr.ColSpan = 3;
                                }

                                // --------- 3rd span
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 8, 1, FpSpread2.Sheets[0].RowCount + 8, 1).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 8, 4, FpSpread2.Sheets[0].RowCount + 8, 4).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 8, 7, FpSpread2.Sheets[0].RowCount + 8, 7).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 8, 10, FpSpread2.Sheets[0].RowCount + 8, 10).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 8, 13, FpSpread2.Sheets[0].RowCount + 8, 13).Cells)
                                {
                                    pr.ColSpan = 3;
                                }

                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 9, 1, FpSpread2.Sheets[0].RowCount + 9, 1).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 9, 4, FpSpread2.Sheets[0].RowCount + 9, 4).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 9, 7, FpSpread2.Sheets[0].RowCount + 9, 7).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 9, 10, FpSpread2.Sheets[0].RowCount + 9, 10).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1forpage2.CellRange(FpSpread2.Sheets[0].RowCount + 9, 13, FpSpread2.Sheets[0].RowCount + 9, 13).Cells)
                                {
                                    pr.ColSpan = 3;
                                }

                                table1forpage2.Columns[fpspread.Sheets[0].ColumnCount - 2].SetWidth(30);

                                Gios.Pdf.PdfTablePage newpdftabpage2 = table1forpage2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 7, 85, 580, 600));
                                mypdfpage1.Add(newpdftabpage2);

                                mypdfpage1.SaveToDocument();
                            }
                        }
                    }
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "rankcard " + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    Response.Buffer = true;
                    Response.Clear();
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }

            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void calrank(string batchyear, string sec, string criteriano, string roll_no, int colno)
    {

        string batch = batchyear;
        string strsec = sec;
        string criteria_no = criteriano;
        double fail = 0;
        double tot_marks = 0;

        double per_marks = 0;
        double no_of_all_clear = 0;

        string pass_fail = string.Empty;
        string per_tage = string.Empty;
        Hashtable studabs = new Hashtable();
        int pass = 0;
        double per_mark = 0;
        double sub_max_marks = 0;
        int stusubabsent = 0;
        int per_sub_count = 0;
        int EL = 0;
        int res = 0;
        int min_mark = 0;
        ds3.Clear();
        ds2.Clear();
        string filterwithsection = "a.app_no=r.app_no and r.degree_code='" + Convert.ToString(degreecode) + "' and et.subject_no=s.subject_no and r.batch_year='" + Convert.ToString(batch) + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + Convert.ToString(criteria_no) + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and  r.sections='" + Convert.ToString(strsec) + "'";
        string filterwithoutsection = "a.app_no=r.app_no and r.degree_code='" + Convert.ToString(degreecode) + "' and et.subject_no=s.subject_no and r.batch_year='" + Convert.ToString(batch) + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + Convert.ToString(criteria_no) + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and  delflag=0  ";
        hat.Clear();
        hat.Add("batchyear", Convert.ToString(batch));
        hat.Add("degreecode", Convert.ToString(degreecode));
        hat.Add("criteria_no", Convert.ToString(criteria_no));
        hat.Add("sections", Convert.ToString(strsec));
        hat.Add("filterwithsection", Convert.ToString(filterwithsection));
        hat.Add("filterwithoutsection", Convert.ToString(filterwithoutsection));

        ds2 = d2.select_method("PROC_STUD_ALL_SUBMARK", hat, "sp");

        ds3 = d2.select_method_wo_parameter("Delete_Rank_Table", "sp");

        int d_count, h_count, t_count, e_count, b_count, g_count;
        string marks_per;
        int stu_count = 0;
        int sub_strength = 0;

        // tot_stu = ds5.Tables[0].Rows.Count;
        per_sub_count = ds2.Tables[1].Rows.Count;

        if (ds2.Tables[0].Rows.Count != 0)   //15.02.12
        {
            string filterwithsectionsub = "a.app_no=r.app_no and r.degree_code='" + Convert.ToString(degreecode) + "' and r.batch_year='" + Convert.ToString(batch) + "'   and RollNo_Flag<>0 and cc=0 and  d.degree_code=r.degree_code and c.course_id=d.course_id and exam_flag <> 'DEBAR' and delflag=0   and sections='" + Convert.ToString(strsec) + "' and r.degree_code=a.degree_code  and ((r.mode=1) or (r.mode=3) or (r.mode=2))and ((r.mode=1) or (r.mode=3) or (r.mode=2))  ";
            string filterwithoutsectionsub = "a.app_no=r.app_no and r.degree_code='" + Convert.ToString(degreecode) + "' and r.batch_year='" + Convert.ToString(batch) + "'   and RollNo_Flag<>0 and cc=0 and  d.degree_code=r.degree_code and c.course_id=d.course_id and exam_flag <> 'DEBAR' and delflag=0 and r.degree_code=a.degree_code  and ((r.mode=1) or (r.mode=3) or (r.mode=2))and ((r.mode=1) or (r.mode=3) or (r.mode=2))   ";
            hat.Clear();
            hat.Add("bath_year", Convert.ToString(batch));
            hat.Add("degree_code", Convert.ToString(degreecode));
            hat.Add("sec", Convert.ToString(strsec));
            hat.Add("filterwithsectionsub", Convert.ToString(filterwithsectionsub));
            hat.Add("filterwithoutsectionsub", Convert.ToString(filterwithoutsectionsub));
            ds5.Clear();
            ds5 = d2.select_method("SELECT _ALL_STUDENT_CAM_REPORTS_DETAILS", hat, "sp");
            DataView dv_indstudmarks = new DataView();

            for (int i = 0; i < ds5.Tables[0].Rows.Count; i++)
            {
                int sstat = 0;
                for (int j = 0; j < per_sub_count; j++)
                {

                    if (stu_count < ds2.Tables[0].Rows.Count)
                    {
                        ds2.Tables[0].DefaultView.RowFilter = "roll='" + Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"]) + "' and subject_no='" + Convert.ToString(ds2.Tables[1].Rows[j]["subject_no"]) + "'";
                        dv_indstudmarks = ds2.Tables[0].DefaultView;
                        if (dv_indstudmarks.Count > 0)
                        {
                            for (int cnt = 0; cnt < dv_indstudmarks.Count; cnt++)
                            {
                                sstat++;
                                double marks = double.Parse(Convert.ToString(dv_indstudmarks[cnt]["mark"]));
                                marks_per = Convert.ToString(dv_indstudmarks[cnt]["mark"]);
                                string min_marksstring = Convert.ToString(dv_indstudmarks[cnt]["min_mark"]);
                                if (min_marksstring != "")
                                {
                                    min_mark = int.Parse(Convert.ToString(min_marksstring));
                                }
                                else
                                {
                                    min_mark = 0;
                                }
                                marks_per = marks.ToString();
                                marks_per = Convert.ToString(dv_indstudmarks[cnt]["mark"]);

                                switch (marks_per)
                                {
                                    case "-1":
                                        if (studabs.Contains(Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"])))
                                        {
                                            if (Convert.ToString(studabs[ds5.Tables[0].Rows[i]["RollNumber"]]) == "1")
                                                studabs[Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"])] = "1";
                                        }
                                        else
                                        {
                                            studabs.Add(Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"]), "1");
                                        }
                                        stusubabsent++;
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
                                    //Added by Subburaj 21.08.2014*************//
                                    case "-18":
                                        marks_per = "RAA";
                                        break;
                                    //****************End*************************//
                                }
                                if (marks_per == "EL" || marks_per == "EOD")
                                {
                                    pass++;
                                }
                                if (marks >= 0 && (Convert.ToString(marks) != string.Empty))
                                {
                                    per_mark += marks;
                                    sub_max_marks += double.Parse(dv_indstudmarks[cnt]["max_mark"].ToString());

                                    if (studabs.Contains(Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"])))
                                    {
                                        studabs[Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"])] = "0";
                                    }
                                    else
                                    {
                                        studabs.Add(Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"]), "0");
                                    }
                                }
                                if (marks >= min_mark || marks_per == "EL" || marks_per == "EOD")
                                {
                                    pass++;


                                }
                                else
                                {
                                    fail++;



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
                    hat.Add("RollNumber", Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"]));
                    hat.Add("criteria_no", Convert.ToString(criteria_no));
                    hat.Add("Total", Convert.ToString(tot_marks));
                    hat.Add("avg", Convert.ToString(per_tage));
                    hat.Add("rank", "");
                    int o = d2.insert_method("INSERT_RANK", hat, "sp");
                }
                pass = 0;
                fail = 0;
                per_mark = 0;
                sub_max_marks = 0;
            }
        }
        int ra_nk = 0;
        DataView dvrank = new DataView();
        ////ra_nk = 1;
        double temp_rank = 0;
        int zx = 1;
        ds3 = d2.select_method_wo_parameter("SELECT_RANK", "sp");

        if (ds3.Tables[0].Rows.Count != 0)
        {
            ////double top_no = double.Parse( Convert.ToString(ds3.Tables[0].Rows[0]["Total"]));
            ////for (int rank = 0; rank < ds3.Tables[0].Rows.Count; rank++)
            ////{

            ////    if (top_no > double.Parse( Convert.ToString(ds3.Tables[0].Rows[rank]["Total"])))
            ////    {
            ////        ra_nk += 1;
            ////    }
            ////    else
            ////    {
            ////        ra_nk = ra_nk;
            ////    }
            ////    top_no = double.Parse( Convert.ToString(ds3.Tables[0].Rows[rank]["Total"]));
            ////    hat.Clear();
            ////    hat.Add("RollNumber",  Convert.ToString(ds3.Tables[0].Rows[rank]["Rollno"]));
            ////    hat.Add("criteria_no",  Convert.ToString(criteria_no));
            ////    hat.Add("Total",  Convert.ToString(tot_marks));
            ////    hat.Add("avg",  Convert.ToString(per_tage));
            ////    hat.Add("rank",  Convert.ToString(ra_nk));
            ////    int o = d2.insert_method("INSERT_RANK", hat, "sp");
            ////}
            for (int rank = 0; rank < ds3.Tables[0].Rows.Count; rank++)
            {
                if (temp_rank == 0)
                {
                    ra_nk = 1;
                    hat.Clear();
                    hat.Add("RollNumber", Convert.ToString(ds3.Tables[0].Rows[rank]["Rollno"]));
                    hat.Add("criteria_no", Convert.ToString(criteria_no));
                    hat.Add("Total", Convert.ToString(tot_marks));
                    hat.Add("avg", Convert.ToString(per_tage));
                    hat.Add("rank", Convert.ToString(ra_nk));
                    int o = d2.insert_method("INSERT_RANK", hat, "sp");

                    temp_rank = double.Parse(Convert.ToString(ds3.Tables[0].Rows[rank]["Total"]));
                    zx++;
                }
                else if (temp_rank != 0)
                {
                    if (temp_rank > double.Parse(Convert.ToString(ds3.Tables[0].Rows[rank]["Total"])))
                    {
                        //   ra_nk += 1;
                        ra_nk = zx;
                        hat.Clear();
                        hat.Add("RollNumber", Convert.ToString(ds3.Tables[0].Rows[rank]["Rollno"]));
                        hat.Add("criteria_no", Convert.ToString(criteria_no));
                        hat.Add("Total", Convert.ToString(tot_marks));
                        hat.Add("avg", Convert.ToString(per_tage));
                        hat.Add("rank", Convert.ToString(ra_nk));
                        int o = d2.insert_method("INSERT_RANK", hat, "sp");

                        temp_rank = double.Parse(Convert.ToString(ds3.Tables[0].Rows[rank]["Total"]));
                        zx++;
                    }
                    else if (temp_rank == double.Parse(Convert.ToString(ds3.Tables[0].Rows[rank]["Total"])))
                    {

                        hat.Clear();
                        hat.Add("RollNumber", Convert.ToString(ds3.Tables[0].Rows[rank]["Rollno"]));
                        hat.Add("criteria_no", Convert.ToString(criteria_no));
                        hat.Add("Total", Convert.ToString(tot_marks));
                        hat.Add("avg", Convert.ToString(per_tage));
                        hat.Add("rank", Convert.ToString(ra_nk));
                        int o = d2.insert_method("INSERT_RANK", hat, "sp");
                        temp_rank = double.Parse(Convert.ToString(ds3.Tables[0].Rows[rank]["Total"]));
                    }
                }

            }


            ds3 = d2.select_method_wo_parameter("SELECT_RANK", "sp");
            int rank_row_count = 0;
            //for (int i = 0; i < ds5.Tables[0].Rows.Count; i++)
            //{
            string roll = roll_no;
            ds3.Tables[1].DefaultView.RowFilter = "rollno='" + roll + "'";
            dvrank = ds3.Tables[1].DefaultView;
            if (dvrank.Count > 0)
            {
                string finalrank = Convert.ToString(dvrank[0]["Rank"]);
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colno].Text = finalrank;
                //FpEntry.Sheets[0].Cells[i, rankcount].Text = 
                //FpEntry.Sheets[0].Cells[i, rankcount].HorizontalAlign = HorizontalAlign.Center;
            }
            else
            {
                string finalrank = "-";
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colno].Text = "-";
                //FpEntry.Sheets[0].Cells[i, rankcount].Text = "-";
                //FpEntry.Sheets[0].Cells[i, rankcount].HorizontalAlign = HorizontalAlign.Center;
            }

            //if (rank_row_count < ds3.Tables[1].Rows.Count)
            //{
            //    if ( Convert.ToString(ds3.Tables[1].Rows[rank_row_count]["Rollno"]) ==  Convert.ToString(ds5.Tables[0].Rows[i]["RollNumber"]))
            //    {
            //        FpEntry.Sheets[0].Cells[i, rankcount].Text =  Convert.ToString(ds3.Tables[1].Rows[rank_row_count]["Rank"]);
            //        rank_row_count++;
            //    }
            //}

        }

    }

    //public void bindstudentdetails(string rollno)
    //{
    //    try
    //    {

    //    }
    //    catch
    //    {

    //    }
    //}

    public void bindstudentdetails(string rollno)
    {
        try
        {
            ArrayList arravoidcol = new ArrayList();
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            DataSet dsphoto = new DataSet();
            DataView dv = new DataView();
            DataTable dtmark = new DataTable();
            DataSet dset = new DataSet();
            int fppagesize = 0;
            string Roll_No = rollno;
            DataSet ds = new DataSet();
            string stdphtsql = string.Empty;

            string stdappno = string.Empty;

            string clm = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No='" + Roll_No + "' ;";
            ds.Clear();
            ds = d2.select_method_wo_parameter(clm, "text");
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string stdcc = string.Empty;
                    stdcc = Convert.ToString(ds.Tables[1].Rows[0]["Dept_Name"]);
                    //    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                    //    {
                    //        image1.ImageUrl = "~/college/Left_Logo.jpeg";
                    //    }

                    //    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                    //    {
                    //        image2.ImageUrl = "~/college/Right_Logo.jpeg";
                    //    }


                    stdappno = Convert.ToString(ds.Tables[1].Rows[0]["App_No"]);
                    stdphtsql = "select * from StdPhoto where app_no='" + stdappno + "'";
                    MemoryStream memoryStream = new MemoryStream();
                    DataSet dsstdpho = new DataSet();
                    dsstdpho.Clear();
                    dsstdpho.Dispose();
                    dsstdpho = d2.select_method_wo_parameter(stdphtsql, "Text");
                    if (dsstdpho.Tables[0].Rows.Count > 0)
                    {
                        byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                        memoryStream.Write(file, 0, file.Length);
                        if (file.Length > 0)
                        {
                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                            {
                                //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                            }
                            else
                            {
                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                            }
                        }
                    }
                    ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                    dv = ds.Tables[1].DefaultView;
                    int count4 = 0;
                    count4 = dv.Count;

                    if (ds.Tables[0].Rows.Count > 0 && count4 > 0)
                    {
                        currentsem = Convert.ToString(dv[0]["Current_Semester"]);
                        batchyear = Convert.ToString(dv[0]["Batch_Year"]);
                        degreecode = Convert.ToString(dv[0]["degree_code"]);
                        string allsem = "1";
                        string admdate = Convert.ToString(dv[0]["adm_date"]);

                        dtmark.Columns.Add("S.No.", typeof(string));
                        dtmark.Columns.Add("", typeof(string));
                        int n = 1;
                        if (dropterm.SelectedItem.Text == "1")
                        {
                            allsem = "1";
                            dtmark.Columns.Add("Term" + "1", typeof(string));
                        }
                        else if (dropterm.SelectedItem.Text == "2")
                        {

                            if (Convert.ToInt32(currentsem) > 1)
                            {
                                int term = Convert.ToInt32(currentsem);
                                for (int i = 2; i <= term; i++)
                                {
                                    allsem = allsem + "'" + "," + "'" + i;
                                    dtmark.Columns.Add("Term" + i, typeof(string));

                                }
                            }
                        }
                        string table1sql = "select  * from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'  and semester in ('" + allsem + "') ; SELECT COUNT(*) as facount FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "')  and CRITERIA_NO <>'' ; SELECT distinct  Istype,CRITERIA_NO,y.semester,M.Conversion_value FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "')  and CRITERIA_NO <>''  order by semester  ; SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "')";
                        DataSet ds1fortable1 = new DataSet();
                        ds1fortable1.Clear();
                        ds1fortable1.Dispose();

                        ds1fortable1 = d2.select_method_wo_parameter(table1sql, "Text");
                        DataView dvforpage2 = new DataView();
                        ds1fortable1.Tables[3].DefaultView.RowFilter = "roll_no='" + Roll_No + "'";
                        dvforpage2 = ds1fortable1.Tables[3].DefaultView;

                        int table1forpage2_rows = ds1fortable1.Tables[0].Rows.Count;
                        if (dropterm.SelectedItem.Text.Trim() == "2")
                        {
                            table1forpage2_rows = table1forpage2_rows / 2;
                        }

                        fppagesize = table1forpage2_rows;
                        int table1forpage2_columns = Convert.ToInt32(ds1fortable1.Tables[2].Rows.Count);

                        if (dropterm.SelectedItem.Text.Trim() == "1")
                        {
                            table1forpage2_columns = 5;
                        }
                        else
                        {
                            table1forpage2_columns = 13;
                        }
                        Hashtable markset = new Hashtable();
                        int percenttotal = 0;
                        int fa_all_col = 0;
                        int sa_all_col = 0;
                        string fa_sa_col = string.Empty;
                        fpspread.Sheets[0].RowHeader.Visible = false;
                        fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                        fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                        fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                        fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                        fpspread.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.Black;
                        fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Small;
                        fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                        fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                        fpspread.Sheets[0].DefaultStyle.Font.Bold = false;
                        fpspread.Sheets[0].AutoPostBack = true;
                        fpspread.CommandBar.Visible = false;
                        fpspread.Sheets[0].RowCount = table1forpage2_rows;
                        fpspread.Sheets[0].ColumnCount = table1forpage2_columns;
                        fpspread.Sheets[0].ColumnHeader.RowCount = 3;
                        fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 30;
                        fpspread.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";
                        fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                        fpspread.Sheets[0].ColumnHeader.Columns[1].Width = 250;
                        fpspread.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject";
                        fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);

                        fpspread.Sheets[0].ColumnHeader.Cells[1, 1].Font.Bold = true;
                        fpspread.Sheets[0].ColumnHeader.Columns[1].Font.Size = FontUnit.Small;
                        fpspread.Sheets[0].ColumnHeader.Cells[1, 0].Text = " ";
                        fpspread.Sheets[0].ColumnCount++;


                        for (int i = 2; i < fpspread.Sheets[0].ColumnCount; i++)
                        {
                            fpspread.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
                        }
                        fpspread.Width = 720;
                        fpspread.Height = 500;

                        for (int i = 0; i < fpspread.Sheets[0].ColumnHeader.RowCount; i++)
                        {
                            for (int j = 0; j < fpspread.Sheets[0].ColumnHeader.Columns.Count; j++)
                            {
                                fpspread.Sheets[0].ColumnHeader.Cells[i, j].ForeColor = Color.Black;
                            }
                        }

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                        darkstyle.ForeColor = System.Drawing.Color.White;

                        fpspread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        Hashtable hatsubject = new Hashtable();
                        Hashtable hatcriter = new Hashtable();


                        if (ds1fortable1.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < table1forpage2_rows; i++)
                            {
                                fpspread.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                                fpspread.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                                fpspread.Sheets[0].Cells[i, 1].Text = ds1fortable1.Tables[0].Rows[i][1].ToString();

                                if (currentsem == "2")
                                {
                                    for (int jj = 1; jj < 3; jj++)
                                    {
                                        ds1fortable1.Tables[0].DefaultView.RowFilter = "semester='" + jj + "'   ";
                                        DataView dvforpage2a = new DataView();
                                        dvforpage2a = ds1fortable1.Tables[0].DefaultView;
                                        int countsubjmarkrow = 0;
                                        countsubjmarkrow = dvforpage2a.Count;
                                        if (countsubjmarkrow > 0)
                                        {
                                            if (jj == 1)
                                            {
                                                fpspread.Sheets[0].Cells[i, 1].Tag = dvforpage2a[i]["subject_no"].ToString();
                                            }
                                            else
                                            {
                                                fpspread.Sheets[0].Cells[i, 1].Note = dvforpage2a[i]["subject_no"].ToString();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    fpspread.Sheets[0].Cells[i, 1].Tag = ds1fortable1.Tables[0].Rows[i]["subject_no"].ToString();
                                }



                            }
                            int fbcount = fpspread.Sheets[0].RowCount;
                            //fpspread.Sheets[0].RowCount++;
                            //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Attendance";
                        }

                        int first = 2;
                        string cretianame = string.Empty;
                        Boolean createspan = false;
                        string roman = string.Empty;

                        int finalperc_col = 0;
                        int lastcol = 2;
                        ArrayList finalcol = new ArrayList();
                        ArrayList firstcol = new ArrayList();

                        Hashtable terms = new Hashtable();
                        string fa_colname = string.Empty;
                        string sa_colname = string.Empty;
                        string fa_colnamest = string.Empty;
                        string sa_colnamest = string.Empty;

                        for (int i = 0; i < Convert.ToInt32(currentsem); i++)
                        {

                            int ss = i + 1;
                            DataView dvforpage2a = new DataView();
                            ds1fortable1.Tables[2].DefaultView.RowFilter = "semester='" + ss + "'";
                            dvforpage2a = ds1fortable1.Tables[2].DefaultView;
                            int count = 0;
                            count = dvforpage2a.Count;
                            int scolcount = count - 1;
                            for (int j = 0; j < count; j++)
                            {

                                string hasterms = lastcol + "-" + count;
                                if (!terms.ContainsKey("Term - " + ss + " "))
                                {

                                    terms.Add("Term - " + ss + " ", hasterms);
                                }

                                if (j != scolcount)
                                {
                                    if (fa_colname == "")
                                    {
                                        fa_colname = Convert.ToString(lastcol);
                                        fa_colnamest = dvforpage2a[j][0].ToString();
                                    }
                                    else
                                    {
                                        fa_colname = fa_colname + " + " + lastcol;
                                        fa_colnamest = fa_colnamest + " + " + dvforpage2a[j][0].ToString();
                                    }
                                }
                                else
                                {
                                    if (sa_colname == "")
                                    {
                                        sa_colname = Convert.ToString(lastcol);
                                        sa_colnamest = dvforpage2a[j][0].ToString();
                                    }
                                    else
                                    {
                                        sa_colname = sa_colname + " + " + lastcol;
                                        sa_colnamest = sa_colnamest + " + " + dvforpage2a[j][0].ToString();
                                    }
                                }
                                fpspread.Sheets[0].ColumnHeader.Cells[1, lastcol].Text = dvforpage2a[j][0].ToString();
                                fpspread.Sheets[0].ColumnHeader.Cells[1, lastcol].Note = dvforpage2a[j]["CRITERIA_NO"].ToString();
                                fpspread.Sheets[0].ColumnHeader.Cells[2, lastcol].Text = dvforpage2a[j]["Conversion_value"].ToString() + "%";
                                fpspread.Sheets[0].ColumnHeader.Cells[2, lastcol].HorizontalAlign = HorizontalAlign.Center;
                                finalcol.Add(lastcol);
                                percenttotal = percenttotal + Convert.ToInt32(dvforpage2a[j]["Conversion_value"].ToString());
                                fpspread.Sheets[0].ColumnHeader.Cells[1, lastcol].HorizontalAlign = HorizontalAlign.Center;
                                fpspread.Sheets[0].ColumnHeader.Columns[lastcol].Width = 50;

                                if (cretianame == "")
                                {
                                    cretianame = dvforpage2a[j][0].ToString();
                                }
                                else
                                {
                                    cretianame = cretianame + " + " + dvforpage2a[j][0].ToString();
                                }


                                lastcol++;
                            }

                            if (count > 0)
                            {


                                string romannew = string.Empty;

                                if (ss == 1)
                                {
                                    romannew = "I";
                                    if (roman == "")
                                    {
                                        roman = "I";

                                    }
                                    else
                                    {
                                        roman = roman + " + " + "I";
                                    }
                                }
                                else if (ss == 2)
                                {
                                    romannew = "II";
                                    if (roman == "")
                                    {
                                        roman = "II";

                                    }
                                    else
                                    {
                                        roman = roman + " + " + "II";
                                        //roman = "FINAL ASSESSMENT";
                                    }

                                }
                                else if (ss == 3)
                                {
                                    romannew = "III";
                                    if (roman == "")
                                    {
                                        roman = "III";

                                    }
                                    else
                                    {
                                        roman = roman + " + " + "II" + " + " + "III";

                                    }

                                }
                                else if (ss == 4)
                                {
                                    romannew = "IV";
                                    if (roman == "")
                                    {
                                        roman = "IV";
                                    }
                                    else
                                    {
                                        roman = roman + " + " + "III" + " + " + "IV";
                                    }

                                }
                                fpspread.Sheets[0].ColumnHeader.Cells[0, first].Text = "Term - " + romannew + "";

                                fpspread.Sheets[0].ColumnHeader.Cells[0, first].HorizontalAlign = HorizontalAlign.Center;
                                fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, first, 1, count + 1);
                                int spancount = count + 1;
                                fpspread.Sheets[0].ColumnHeader.Cells[0, first].Note = Convert.ToString(first) + "-" + Convert.ToString(spancount);

                                fpspread.Sheets[0].ColumnHeader.Cells[1, lastcol].Text = cretianame;
                                fpspread.Sheets[0].ColumnHeader.Cells[1, lastcol].Note = cretianame;

                                //if ("FA1 + FA2 + SA1".ToLower() == cretianame.ToLower())
                                //{
                                //    fpspread.Sheets[0].ColumnHeader.Cells[1, lastcol].Text = "Total";
                                //    fpspread.Sheets[0].ColumnHeader.Cells[1, lastcol].Note = cretianame;
                                //}
                                //else if ("FA3 + FA4 + SA2".ToLower() == cretianame.ToLower())
                                //{
                                //    fpspread.Sheets[0].ColumnHeader.Cells[1, lastcol].Text = "Total";
                                //    fpspread.Sheets[0].ColumnHeader.Cells[1, lastcol].Note = cretianame;
                                //}

                                if (fa_sa_col == "")
                                {
                                    fa_sa_col = Convert.ToString(lastcol);
                                }
                                else
                                {
                                    fa_sa_col = fa_sa_col + " - " + Convert.ToString(lastcol);
                                }
                                fpspread.Sheets[0].ColumnHeader.Cells[2, lastcol].Text = "" + percenttotal + "%";
                                fpspread.Sheets[0].ColumnHeader.Cells[2, lastcol].HorizontalAlign = HorizontalAlign.Center;

                                cretianame = string.Empty;
                                percenttotal = 0;
                                lastcol++;
                                createspan = true;
                                first = lastcol;
                            }
                        }
                        if (createspan == true)
                        {
                            if (dropterm.SelectedItem.Text.Trim() != "1")
                            {

                                fpspread.Sheets[0].ColumnHeader.Cells[0, first].Text = "FINAL ASSESSMENT";
                                if (Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 1)
                                {
                                    fpspread.Sheets[0].ColumnHeader.Cells[0, first].Text = "Total";
                                }
                                fpspread.Sheets[0].ColumnHeader.Cells[0, first].HorizontalAlign = HorizontalAlign.Center;
                                fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, first, 1, 4);

                                fpspread.Sheets[0].ColumnHeader.Cells[0, first].Note = Convert.ToString(first) + "-" + Convert.ToString(4);
                                //if ("FA1 + FA2 + FA3 + FA4".ToLower() == fa_colnamest.ToLower())
                                //{
                                //    fpspread.Sheets[0].ColumnHeader.Cells[1, first].Text = "FA";
                                //}

                                fpspread.Sheets[0].ColumnHeader.Cells[1, first].Text = fa_colnamest;
                                fa_all_col = first;
                                fpspread.Sheets[0].ColumnHeader.Cells[1, first].HorizontalAlign = HorizontalAlign.Center;

                                //if ("SA1 + SA2".ToLower() == sa_colnamest.ToLower())
                                //{
                                //    fpspread.Sheets[0].ColumnHeader.Cells[1, first + 1].Text = "SA";
                                //} 
                                fpspread.Sheets[0].ColumnHeader.Cells[1, first + 1].Text = sa_colnamest;
                                sa_all_col = first + 1;
                                fpspread.Sheets[0].ColumnHeader.Cells[1, first + 1].HorizontalAlign = HorizontalAlign.Center;
                                fpspread.Sheets[0].ColumnHeader.Cells[1, first + 2].Text = "Total Overall Grade";
                                fpspread.Sheets[0].ColumnHeader.Cells[1, first + 2].HorizontalAlign = HorizontalAlign.Center;
                                finalperc_col = first + 2;
                                fpspread.Sheets[0].ColumnHeader.Cells[1, first + 3].Text = "Upgrade + Grade Point";
                                fpspread.Sheets[0].ColumnHeader.Cells[1, first + 3].HorizontalAlign = HorizontalAlign.Center;
                            }

                        }

                        ds1fortable1.Tables[3].DefaultView.RowFilter = "roll_no='" + Roll_No + "'";
                        dvforpage2 = ds1fortable1.Tables[3].DefaultView;
                        int countsubjmark = 0;
                        countsubjmark = dvforpage2.Count;
                        double calculatedvalue = 0;

                        int SSSD = 0;
                        string[] spitsa_colname = sa_colname.Split('+');
                        string[] spitfa_colname = fa_colname.Split('+');
                        string[] spitfa_sa_colname = fa_sa_col.Split('-');
                        if (countsubjmark > 0)
                        {
                            for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                            {
                                string subject_code = fpspread.Sheets[0].Cells[i, 1].Note.ToString();
                                calculatedvalue = 0;

                                for (int j = 0; j < finalcol.Count; j++)
                                {
                                    SSSD = Convert.ToInt32(finalcol[j].ToString());
                                    string cretiacode = fpspread.Sheets[0].ColumnHeader.Cells[1, SSSD].Note.ToString();
                                    string cretianamecheck = fpspread.Sheets[0].ColumnHeader.Cells[1, SSSD].Text.ToString();
                                    if (cretianamecheck.Trim().ToLower() == "fa1" || cretianamecheck.Trim().ToLower() == "fa2" || cretianamecheck.Trim().ToLower() == "sa1")
                                    {
                                        subject_code = fpspread.Sheets[0].Cells[i, 1].Tag.ToString();
                                    }
                                    else
                                    {
                                        subject_code = fpspread.Sheets[0].Cells[i, 1].Note.ToString();
                                    }

                                    ds1fortable1.Tables[3].DefaultView.RowFilter = "roll_no='" + Roll_No + "' and subject_no='" + subject_code + "' and CRITERIA_NO='" + cretiacode + "' ";
                                    DataView dvforpage2a = new DataView();
                                    dvforpage2a = ds1fortable1.Tables[3].DefaultView;
                                    int countsubjmarkrow = 0;
                                    countsubjmarkrow = dvforpage2a.Count;


                                    if (countsubjmarkrow > 0)
                                    {
                                        Double getheigh = Convert.ToDouble(dvforpage2a[0]["Exammark"].ToString());
                                        getheigh = Math.Round(getheigh, 2);



                                        fpspread.Sheets[0].Cells[i, SSSD].Text = Convert.ToString(getheigh);

                                    }
                                    else
                                    {
                                        fpspread.Sheets[0].Cells[i, SSSD].Text = "0";
                                    }
                                }
                            }
                            calculatedvalue = 0;
                            for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                            {
                                for (int j = 0; j <= spitfa_colname.GetUpperBound(0); j++)
                                {
                                    int col = Convert.ToInt32(spitfa_colname[j].ToString());
                                    // int markfinal = Convert.ToInt32(fpspread.Sheets[0].Cells[i, col].Text);
                                    double markfinal = Convert.ToDouble(fpspread.Sheets[0].Cells[i, col].Text);
                                    calculatedvalue = calculatedvalue + markfinal;


                                }
                                if (dropterm.SelectedItem.Text.Trim() != "1")
                                {
                                    fpspread.Sheets[0].Cells[i, fa_all_col].Text = Convert.ToString(calculatedvalue);
                                }
                                calculatedvalue = 0;
                            }
                            calculatedvalue = 0;
                            for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                            {
                                for (int j = 0; j <= spitsa_colname.GetUpperBound(0); j++)
                                {
                                    int col = Convert.ToInt32(spitsa_colname[j].ToString());
                                    double markvalue = Convert.ToDouble(fpspread.Sheets[0].Cells[i, col].Text);
                                    calculatedvalue = calculatedvalue + markvalue;
                                }
                                if (dropterm.SelectedItem.Text.Trim() != "1")
                                {
                                    fpspread.Sheets[0].Cells[i, sa_all_col].Text = Convert.ToString(calculatedvalue);
                                }
                                calculatedvalue = 0;
                            }
                            calculatedvalue = 0;
                            int last_fa_sa = 0;
                            for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                            {
                                for (int k = 0; k <= spitfa_sa_colname.GetUpperBound(0); k++)
                                {
                                    int fa_sa_cc = Convert.ToInt32(spitfa_sa_colname[k].ToString());

                                    for (int j = 0; j < finalcol.Count; j++)
                                    {
                                        int col = Convert.ToInt32(finalcol[j].ToString());
                                        if (fa_sa_cc > col && last_fa_sa < col)
                                        {
                                            double markvalue = Convert.ToDouble(fpspread.Sheets[0].Cells[i, col].Text);
                                            calculatedvalue = calculatedvalue + markvalue;
                                        }
                                    }
                                    if (dropterm.SelectedItem.Text.Trim() == "1")
                                    {
                                        fpspread.Sheets[0].Cells[i, fa_sa_cc].Text = Convert.ToString(calculatedvalue);
                                        fpspread.Sheets[0].Cells[i, fa_sa_cc].Note = Convert.ToString(calculatedvalue);
                                    }
                                    if (dropterm.SelectedItem.Text.Trim() == "2")
                                    {
                                        fpspread.Sheets[0].Cells[i, fa_sa_cc].Text = Convert.ToString(calculatedvalue);
                                        fpspread.Sheets[0].Cells[i, fa_sa_cc].Note = Convert.ToString(calculatedvalue);
                                    }
                                    last_fa_sa = fa_sa_cc;
                                    calculatedvalue = 0;
                                }
                                last_fa_sa = 0;
                            }
                            double finalpercentage = 0;
                            calculatedvalue = 0;
                            for (int j = 0; j <= spitfa_colname.GetUpperBound(0); j++)
                            {
                                int col = Convert.ToInt32(spitfa_colname[j].ToString());
                                string markvalue = fpspread.Sheets[0].ColumnHeader.Cells[2, col].Text;
                                string[] marpersplit = markvalue.Split('%');
                                int marpersplitvalue = Convert.ToInt32(marpersplit[0]);
                                calculatedvalue = calculatedvalue + marpersplitvalue;
                            }
                            finalpercentage = finalpercentage + calculatedvalue;
                            fpspread.Sheets[0].ColumnHeader.Cells[2, fa_all_col].Text = Convert.ToString(calculatedvalue + "%");
                            fpspread.Sheets[0].ColumnHeader.Cells[2, fa_all_col].HorizontalAlign = HorizontalAlign.Center;

                            calculatedvalue = 0;
                            for (int j = 0; j <= spitsa_colname.GetUpperBound(0); j++)
                            {
                                int col = Convert.ToInt32(spitsa_colname[j].ToString());
                                string markvalue = fpspread.Sheets[0].ColumnHeader.Cells[2, col].Text;
                                string[] marpersplit = markvalue.Split('%');
                                int marpersplitvalue = Convert.ToInt32(marpersplit[0]);
                                calculatedvalue = calculatedvalue + marpersplitvalue;
                            }
                            finalpercentage = finalpercentage + calculatedvalue;
                            fpspread.Sheets[0].ColumnHeader.Cells[2, sa_all_col].Text = Convert.ToString(calculatedvalue + "%");
                            fpspread.Sheets[0].ColumnHeader.Cells[2, sa_all_col].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].ColumnHeader.Cells[2, finalperc_col].Text = Convert.ToString(finalpercentage + "%");
                            fpspread.Sheets[0].ColumnHeader.Cells[2, finalperc_col].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].ColumnHeader.Cells[2, finalperc_col + 1].Text = Convert.ToString(" ");


                            calculatedvalue = 0;
                            //for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                            //{

                            //    double markvaluefa = Convert.ToDouble(fpspread.Sheets[0].Cells[i, fa_all_col].Text);
                            //    double markvaluesa = Convert.ToDouble(fpspread.Sheets[0].Cells[i, sa_all_col].Text);
                            //    calculatedvalue = markvaluefa + markvaluesa;




                            //    fpspread.Sheets[0].Cells[i, finalperc_col].Text = Convert.ToString(calculatedvalue);
                            //    calculatedvalue = 0;
                            //}

                            string main = "SELECT * from Grade_Master where Semester='" + currentsem + "' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria =''";
                            ds = d2.select_method_wo_parameter(main, "text");
                            double checkmarkmm = 0;
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                                {
                                    if (dropterm.SelectedItem.Text.Trim() != "1")
                                    {
                                        double markvaluefa = Convert.ToDouble(fpspread.Sheets[0].Cells[i, fa_all_col].Text);
                                        double markvaluesa = Convert.ToDouble(fpspread.Sheets[0].Cells[i, sa_all_col].Text);
                                        string percentage1 = fpspread.Sheets[0].ColumnHeader.Cells[2, fpspread.Sheets[0].Columns.Count - 2].Text.ToString();
                                        string[] splitpercentage1 = percentage1.Split('%');
                                        double perccents = Convert.ToDouble(splitpercentage1[0].ToString());
                                        calculatedvalue = (markvaluefa + markvaluesa) * 100;

                                        calculatedvalue = calculatedvalue / perccents;

                                        checkmarkmm = calculatedvalue;
                                        for (int grd = 0; grd < ds.Tables[0].Rows.Count; grd++)
                                        {
                                            if (Convert.ToInt32(ds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(ds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                            {

                                                fpspread.Sheets[0].Cells[i, finalperc_col].Text = ds.Tables[0].Rows[grd][0].ToString();
                                                fpspread.Sheets[0].Cells[i, finalperc_col + 1].Text = ds.Tables[0].Rows[grd]["Credit_Points"].ToString();

                                            }
                                        }
                                    }
                                    calculatedvalue = 0;
                                }
                            }
                            else
                            {
                                main = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria =''";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(main, "text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < fpspread.Sheets[0].RowCount; i++)
                                    {
                                        if (dropterm.SelectedItem.Text.Trim() != "1")
                                        {
                                            double markvaluefa = Convert.ToDouble(fpspread.Sheets[0].Cells[i, fa_all_col].Text);
                                            double markvaluesa = Convert.ToDouble(fpspread.Sheets[0].Cells[i, sa_all_col].Text);
                                            string percentage1 = fpspread.Sheets[0].ColumnHeader.Cells[2, fpspread.Sheets[0].Columns.Count - 2].Text.ToString();
                                            string[] splitpercentage1 = percentage1.Split('%');
                                            double perccents = Convert.ToDouble(splitpercentage1[0].ToString());
                                            calculatedvalue = (markvaluefa + markvaluesa) * 100;

                                            calculatedvalue = calculatedvalue / perccents;
                                            checkmarkmm = calculatedvalue;
                                            for (int grd = 0; grd < ds.Tables[0].Rows.Count; grd++)
                                            {
                                                if (Convert.ToInt32(ds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(ds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                                {

                                                    fpspread.Sheets[0].Cells[i, finalperc_col].Text = ds.Tables[0].Rows[grd][0].ToString();
                                                    fpspread.Sheets[0].Cells[i, finalperc_col + 1].Text = ds.Tables[0].Rows[grd]["Credit_Points"].ToString();

                                                }
                                            }
                                        }
                                        calculatedvalue = 0;
                                    }
                                }
                            }
                        }

                        fpspread.Visible = false;


                        for (int i = 0; i < Convert.ToInt32(currentsem); i++)
                        {
                            int ss = i + 1;
                            DataView dvforpage2a = new DataView();
                            ds1fortable1.Tables[2].DefaultView.RowFilter = "semester='" + ss + "'";
                            dvforpage2a = ds1fortable1.Tables[2].DefaultView;
                            int count = 0;
                            count = dvforpage2a.Count;
                        }

                        if (ds1fortable1.Tables[0].Rows.Count > 0)
                        {
                        }
                    }

                    int newcolval = 0;
                    if (dropterm.SelectedItem.Text.Trim() == "1")
                    {
                        newcolval = fpspread.Sheets[0].ColumnCount;
                    }
                    else
                    {
                        newcolval = fpspread.Sheets[0].ColumnCount - 1;
                    }

                    for (int i = 2; i < newcolval; i++)
                    {
                        string finpercent = fpspread.Sheets[0].ColumnHeader.Cells[2, i].Text.ToString();
                        string fincretia = fpspread.Sheets[0].ColumnHeader.Cells[1, i].Text.ToString();
                        string[] splitfinpercent = finpercent.Split('%');
                        string[] splitfincretia = fincretia.Split('+');
                        //double percc = Convert.ToDouble();
                        //string finmarkval = fpspread.Sheets[0].ColumnHeader.Cells[2, i].Text.ToString();
                        if (splitfincretia.GetUpperBound(0) == 0)
                        {
                            if (splitfincretia[0].Trim().ToLower() == "fa1" || splitfincretia[0].Trim().ToLower() == "fa2" || splitfincretia[0].Trim().ToLower() == "fa3" || splitfincretia[0].Trim().ToLower() == "fa4" || splitfincretia[0].Trim().ToLower() == "sa1" || splitfincretia[0].Trim().ToLower() == "sa2")
                            {
                                for (int j = 0; j < fpspread.Sheets[0].RowCount; j++)
                                {
                                    string finmarkval = fpspread.Sheets[0].Cells[j, i].Text.ToString();

                                    double convertedval = (Convert.ToDouble(finmarkval) * 100);
                                    convertedval = convertedval / Convert.ToDouble(splitfinpercent[0]);
                                    string main = "SELECT * from Grade_Master where College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "' and Criteria='" + fincretia + "'";
                                    ds = d2.select_method_wo_parameter(main, "text");
                                    double checkmarkmm = 0;

                                    if (ds.Tables[0].Rows.Count > 0)
                                    {


                                        checkmarkmm = convertedval;
                                        for (int grd = 0; grd < ds.Tables[0].Rows.Count; grd++)
                                        {
                                            if (Convert.ToInt32(ds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(ds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                            {
                                                fpspread.Sheets[0].Cells[j, i].Text = ds.Tables[0].Rows[grd][0].ToString();

                                            }
                                        }
                                        convertedval = 0;

                                    }

                                }
                            }
                        }
                        else
                        {
                            string main = "SELECT * from Grade_Master where Semester='" + currentsem + "' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='' ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(main, "text");
                            double checkmarkmm = 0;
                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                main = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria =''";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(main, "text");
                            }
                            for (int j = 0; j < fpspread.Sheets[0].RowCount; j++)
                            {

                                //string sddd = fpspread.Sheets[0].Cells[j, i+1].Text.ToString();
                                string finmarkval = fpspread.Sheets[0].Cells[j, i].Text.ToString();
                                if (splitfinpercent[0] != "")
                                {
                                    double convertedval = (Convert.ToDouble(finmarkval) * 100);
                                    convertedval = convertedval / Convert.ToDouble(splitfinpercent[0]);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        checkmarkmm = convertedval;
                                        for (int grd = 0; grd < ds.Tables[0].Rows.Count; grd++)
                                        {
                                            if (Convert.ToInt32(ds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(ds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                            {
                                                fpspread.Sheets[0].Cells[j, i].Text = ds.Tables[0].Rows[grd][0].ToString();

                                            }
                                        }
                                        convertedval = 0;
                                    }
                                }
                            }
                        }
                    }

                    double credittotal = 0;
                    double grandtotcredit = 0;
                    double grandtotcreditfull = 0;

                    string maxmain = "SELECT MAX(Credit_Points) from Grade_Master where Semester='" + currentsem + "' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='' ";
                    ds.Clear();
                    string maxgd = string.Empty;
                    ds = d2.select_method_wo_parameter(maxmain, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        maxgd = Convert.ToString(ds.Tables[0].Rows[0][0]);
                    }

                    if (maxgd.Trim() == "")
                    {
                        maxmain = "SELECT MAX(Credit_Points) from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria =''";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(maxmain, "text");
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        grandtotcredit = Convert.ToDouble(Convert.ToString(ds.Tables[0].Rows[0][0]));
                        grandtotcredit = grandtotcredit * fpspread.Sheets[0].RowCount;
                    }
                    if (dropterm.SelectedItem.Text.Trim() != "1")
                    {
                        for (int i = fpspread.Sheets[0].ColumnCount - 1; i < fpspread.Sheets[0].ColumnCount; i++)
                        {
                            for (int j = 0; j < fpspread.Sheets[0].RowCount; j++)
                            {
                                if (fpspread.Sheets[0].Cells[j, i].Text.ToString().Trim() != "")
                                {
                                    credittotal = credittotal + Convert.ToDouble(fpspread.Sheets[0].Cells[j, i].Text.ToString());
                                }
                            }
                        }
                    }

                    grandtotcreditfull = (credittotal / grandtotcredit);
                    grandtotcredit = Math.Round(grandtotcreditfull, 2);
                    fpspread.SaveChanges();
                    fpspread.Sheets[0].PageSize = fppagesize;
                    //cgpa = da.Calculete_CGPA(Roll_No, currentsem, degreecode, batchyear, latmode, college_code);
                    //lblcgpa.Text = Convert.ToString(grandtotcredit);
                    grandtotcredit = grandtotcreditfull * 100;


                    string totmain = "SELECT * from Grade_Master where Semester='" + currentsem + "' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria = '' ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(totmain, "text");
                    double totmaincheckmarkmm = grandtotcredit;
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        totmain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegecode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria =''";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(totmain, "text");
                    }
                    for (int grd = 0; grd < ds.Tables[0].Rows.Count; grd++)
                    {
                        if (Convert.ToInt32(ds.Tables[0].Rows[grd][1]) <= totmaincheckmarkmm && Convert.ToInt32(ds.Tables[0].Rows[grd][2]) >= totmaincheckmarkmm)
                        {
                            lblgradeval.Text = ds.Tables[0].Rows[grd][0].ToString();
                        }
                    }

                    //if (dropterm.SelectedItem.Text.Trim() == "1")
                    //{
                    //    fpspread.Sheets[0].RowCount++;
                    //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Attendance";
                    //    fpspread.Sheets[0].RowCount++;
                    //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Nine Point Grading Scale : A1 = 91% - 100%; A2 = 81% - 90%; B1 = 71% - 80%; B2 = 61% - 70%; C1 = 51% - 60%;     C2 = 41% - 50%; D = 33% - 41%; E2 = 21% - 32%; E1 = 20% AND BELOW; ";
                    //    int rcolcount = fpspread.Sheets[0].RowCount;
                    //}
                    //else
                    //{
                    //    fpspread.Sheets[0].RowCount++;
                    //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Attendance";
                    //    fpspread.Sheets[0].RowCount++;
                    //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Nine Point Grading Scale : A1 = 91% - 100%; A2 = 81% - 90%; B1 = 71% - 80%; B2 = 61% - 70%; C1 = 51% - 60%;     C2 = 41% - 50%; D = 33% - 41%; E2 = 21% - 32%; E1 = 20% AND BELOW; ";
                    //    int rcolcount = fpspread.Sheets[0].RowCount;
                    //}

                    //fpspread.Sheets[0].Columns[0].Visible = false;

                }
                else
                {

                }
            }
        }

        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void bindstudentdetails_new(string rollno)
    {
        try
        {
            FpSpread2.Height = 500;

            FpSpread2.Width = 1500;
            ArrayList arravoidcol = new ArrayList();
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            DataSet dsphoto = new DataSet();
            DataView dv = new DataView();
            DataTable dtmark = new DataTable();
            DataSet dset = new DataSet();
            int fppagesize = 0;
            string Roll_No = rollno;
            DataSet ds = new DataSet();
            string stdphtsql = string.Empty;
            ArrayList testcolsterm1 = new ArrayList();
            ArrayList testcolsterm2 = new ArrayList();
            string stdappno = string.Empty;

            string clm = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No='" + Roll_No + "' ;";
            ds.Clear();
            ds = d2.select_method_wo_parameter(clm, "text");
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    string stdcc = string.Empty;
                    stdcc = Convert.ToString(ds.Tables[1].Rows[0]["Dept_Name"]);

                    stdappno = Convert.ToString(ds.Tables[1].Rows[0]["App_No"]);
                    stdphtsql = "select * from StdPhoto where app_no='" + stdappno + "'";
                    MemoryStream memoryStream = new MemoryStream();
                    DataSet dsstdpho = new DataSet();
                    dsstdpho.Clear();
                    dsstdpho.Dispose();
                    dsstdpho = d2.select_method_wo_parameter(stdphtsql, "Text");
                    if (dsstdpho.Tables[0].Rows.Count > 0)
                    {
                        byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                        memoryStream.Write(file, 0, file.Length);
                        if (file.Length > 0)
                        {
                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                            {

                            }
                            else
                            {
                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                            }



                        }

                    }





                    ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                    dv = ds.Tables[1].DefaultView;
                    int count4 = 0;
                    count4 = dv.Count;

                    if (ds.Tables[0].Rows.Count > 0 && count4 > 0)
                    {
                        currentsem = Convert.ToString(dv[0]["Current_Semester"]);
                        batchyear = Convert.ToString(dv[0]["Batch_Year"]);
                        degreecode = Convert.ToString(dv[0]["degree_code"]);
                        string allsem = "1";
                        string admdate = Convert.ToString(dv[0]["adm_date"]);
                        FpSpread2.Sheets[0].RowCount = 0;

                        FpSpread2.Sheets[0].SheetCorner.ColumnCount = 0;
                        FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread2.Sheets[0].ColumnCount = 2;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Subjects";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Max Marks";

                        if (dropterm.SelectedItem.Text == "1")
                        {
                            allsem = "1";

                        }
                        else if (dropterm.SelectedItem.Text == "2")
                        {
                            if (Convert.ToInt32(currentsem) > 1)
                            {
                                int term = Convert.ToInt32(currentsem);
                                for (int i = 2; i <= term; i++)
                                {
                                    allsem = allsem + "'" + "," + "'" + i;


                                }
                            }
                        }

                        string table1sql = "select  * from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'  and semester in ('" + allsem + "') ; SELECT COUNT(*) as facount FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "')  and CRITERIA_NO <>'' ; SELECT distinct  Istype,CRITERIA_NO,y.semester,M.Conversion_value FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "')  and CRITERIA_NO <>''  order by semester  ; SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "')";
                        DataSet ds1fortable1 = new DataSet();
                        ds1fortable1.Clear();
                        ds1fortable1.Dispose();
                        int testmarks = 0;
                        ds1fortable1 = d2.select_method_wo_parameter(table1sql, "Text");
                        DataView dvforpage2 = new DataView();
                        ds1fortable1.Tables[3].DefaultView.RowFilter = "roll_no='" + Roll_No + "'";
                        dvforpage2 = ds1fortable1.Tables[3].DefaultView;
                        int table1forpage2_rows = ds1fortable1.Tables[0].Rows.Count;
                        if (dropterm.SelectedItem.Text.Trim() == "2")
                        {
                            table1forpage2_rows = table1forpage2_rows / 2;
                        }
                        FpSpread2.Sheets[0].RowCount = table1forpage2_rows;
                        if (ds1fortable1.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < table1forpage2_rows; i++)
                            {
                                // FpSpread2.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                                FpSpread2.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                                //FpSpread2.Sheets[0].Cells[i, 1].Text = ds1fortable1.Tables[0].Rows[i][1].ToString();

                                if (currentsem == "2")
                                {
                                    for (int jj = 1; jj < 3; jj++)
                                    {
                                        ds1fortable1.Tables[0].DefaultView.RowFilter = "semester='" + jj + "'   ";
                                        DataView dvforpage2a = new DataView();
                                        dvforpage2a = ds1fortable1.Tables[0].DefaultView;
                                        int countsubjmarkrow = 0;
                                        countsubjmarkrow = dvforpage2a.Count;
                                        if (countsubjmarkrow > 0)
                                        {
                                            if (jj == 1)
                                            {
                                                FpSpread2.Sheets[0].Cells[i, 0].Tag = dvforpage2a[i]["subject_no"].ToString();
                                                FpSpread2.Sheets[0].Cells[i, 0].Text = dvforpage2a[i]["subject_name"].ToString();

                                                FpSpread2.Sheets[0].Cells[i, 1].Text = dvforpage2a[i]["maxtotal"].ToString();



                                            }
                                            else
                                            {
                                                FpSpread2.Sheets[0].Cells[i, 0].Note = dvforpage2a[i]["subject_no"].ToString();
                                                FpSpread2.Sheets[0].Cells[i, 0].Text = dvforpage2a[i]["subject_name"].ToString();
                                                FpSpread2.Sheets[0].Cells[i, 1].Text = dvforpage2a[i]["maxtotal"].ToString();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    FpSpread2.Sheets[0].Cells[i, 0].Tag = ds1fortable1.Tables[0].Rows[i]["subject_no"].ToString();
                                    FpSpread2.Sheets[0].Cells[i, 0].Text = ds1fortable1.Tables[0].Rows[i]["subject_name"].ToString();
                                    FpSpread2.Sheets[0].Cells[i, 1].Text = ds1fortable1.Tables[0].Rows[i]["maxtotal"].ToString();
                                }



                            }

                            if (ds1fortable1.Tables[2].Rows.Count > 0)
                            {

                                for (int jj = 1; jj < 3; jj++)
                                {
                                    ds1fortable1.Tables[2].DefaultView.RowFilter = "semester='" + jj + "'   ";
                                    DataView dvforpage2a = new DataView();
                                    dvforpage2a = ds1fortable1.Tables[2].DefaultView;
                                    int countrow = 0;
                                    countrow = dvforpage2a.Count;

                                    for (int i = 0; i < countrow; i++)
                                    {

                                        if (countrow > 0)
                                        {
                                            string cretianos = dvforpage2a[i]["CRITERIA_NO"].ToString();
                                            string[] splitcretianos = cretianos.Split(',');
                                            string criterianame = string.Empty;
                                            for (int ii = 0; ii <= splitcretianos.GetUpperBound(0); ii++)
                                            {
                                                string cretianumber = splitcretianos[ii].ToString();
                                                if (jj == 1)
                                                {
                                                    criterianame = d2.GetFunction("select distinct c.criteria,c.Criteria_no,e.max_mark from CriteriaForInternal c,Exam_type e,syllabus_master sy where sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sy.Batch_Year = '" + batchyear + "' and sy.degree_code =  '" + degreecode + "' and sy.semester = '" + jj + "' and c.Criteria_no='" + cretianumber + "'");
                                                    FpSpread2.Sheets[0].ColumnCount++;
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = criterianame;
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Note = cretianumber;
                                                    testcolsterm1.Add(FpSpread2.Sheets[0].ColumnCount - 1);
                                                }
                                                else
                                                {
                                                    criterianame = d2.GetFunction("select distinct c.criteria,c.Criteria_no,e.max_mark from CriteriaForInternal c,Exam_type e,syllabus_master sy where sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sy.Batch_Year = '" + batchyear + "' and sy.degree_code =  '" + degreecode + "' and sy.semester = '" + jj + "' and c.Criteria_no='" + cretianumber + "'");
                                                    FpSpread2.Sheets[0].ColumnCount++;
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = criterianame;
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Note = cretianumber;
                                                    testcolsterm2.Add(FpSpread2.Sheets[0].ColumnCount - 1);
                                                }
                                            }
                                        }

                                    }
                                    if (jj == 1)
                                    {
                                        FpSpread2.Sheets[0].ColumnCount++;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Term I";

                                    }
                                    if (dropterm.SelectedItem.Text == "2")
                                    {
                                        if (jj == 2)
                                        {
                                            FpSpread2.Sheets[0].ColumnCount++;
                                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Term II";
                                        }
                                    }

                                }



                            }
                        }
                    }
                    string strsubjno = string.Empty;
                    string cretianumb = string.Empty;
                    int cols = 0;
                    int oldcols = 0;
                    int uptosubjrows = FpSpread2.Sheets[0].RowCount;
                    int tearm1col = 0;
                    int tearm2col = 0;
                    for (int j = 0; j < FpSpread2.Sheets[0].RowCount; j++)
                    {
                        for (int i = 0; i < testcolsterm1.Count; i++)
                        {
                            cols = Convert.ToInt32(testcolsterm1[i].ToString());
                            strsubjno = FpSpread2.Sheets[0].Cells[j, 0].Tag.ToString();
                            cretianumb = FpSpread2.Sheets[0].ColumnHeader.Cells[0, cols].Note.ToString();
                            string getmarks = d2.GetFunction("SELECT marks_obtained FROM Result u,Registration r,Exam_type e,CriteriaForInternal c   WHERE U.roll_no = R.Roll_No AND U.exam_code = E.exam_code AND E.criteria_no = C.Criteria_no    AND subject_no = '" + strsubjno + "' and R.Roll_No= '" + Roll_No + "' and c.criteria_no='" + cretianumb + "' order by u.roll_no,STUD_NAME,e.exam_date,c.criteria_no");
                            int getmm = Convert.ToInt32(getmarks);
                            if (getmm > 0)
                            {
                                FpSpread2.Sheets[0].Cells[j, cols].Text = getmarks.ToString();
                                FpSpread2.Sheets[0].Cells[j, cols].Note = getmarks.ToString();
                            }
                            else
                            {
                                FpSpread2.Sheets[0].Cells[j, cols].Note = getmarks.ToString();
                                getmarks = loadmarkat(getmarks);
                                FpSpread2.Sheets[0].Cells[j, cols].Text = getmarks.ToString();
                            }

                        }
                    }

                    for (int k = 0; k < fpspread.Sheets[0].Columns.Count; k++)
                    {
                        if (fpspread.Sheets[0].ColumnHeader.Cells[1, k].Text.Trim().ToLower().ToString() == "fa1 + fa2 + sa1")
                        {
                            oldcols = k;

                        }

                    }
                    for (int j = 0; j < FpSpread2.Sheets[0].RowCount; j++)
                    {


                        FpSpread2.Sheets[0].Cells[j, cols + 1].Text = fpspread.Sheets[0].Cells[j, oldcols].Note.ToString();
                        tearm1col = cols + 1;
                    }

                    for (int j = 0; j < FpSpread2.Sheets[0].RowCount; j++)
                    {
                        for (int i = 0; i < testcolsterm2.Count; i++)
                        {
                            cols = Convert.ToInt32(testcolsterm2[i].ToString());
                            strsubjno = FpSpread2.Sheets[0].Cells[j, 0].Note.ToString();
                            cretianumb = FpSpread2.Sheets[0].ColumnHeader.Cells[0, cols].Note.ToString();
                            string getmarks = d2.GetFunction("SELECT marks_obtained FROM Result u,Registration r,Exam_type e,CriteriaForInternal c   WHERE U.roll_no = R.Roll_No AND U.exam_code = E.exam_code AND E.criteria_no = C.Criteria_no    AND subject_no = '" + strsubjno + "' and R.Roll_No= '" + Roll_No + "' and c.criteria_no='" + cretianumb + "' order by u.roll_no,STUD_NAME,e.exam_date,c.criteria_no");
                            int getmm = Convert.ToInt32(getmarks);
                            if (getmm > 0)
                            {
                                FpSpread2.Sheets[0].Cells[j, cols].Text = getmarks.ToString();
                                FpSpread2.Sheets[0].Cells[j, cols].Note = getmarks.ToString();
                            }
                            else
                            {
                                FpSpread2.Sheets[0].Cells[j, cols].Note = getmarks.ToString();
                                getmarks = loadmarkat(getmarks);
                                FpSpread2.Sheets[0].Cells[j, cols].Text = getmarks.ToString();
                            }
                        }
                    }

                    for (int k = 0; k < fpspread.Sheets[0].Columns.Count; k++)
                    {
                        if (fpspread.Sheets[0].ColumnHeader.Cells[1, k].Text.Trim().ToLower().ToString() == "fa3 + fa4 + sa2")
                        {
                            oldcols = k;
                        }
                    }
                    for (int j = 0; j < FpSpread2.Sheets[0].RowCount; j++)
                    {
                        FpSpread2.Sheets[0].Cells[j, cols + 1].Text = Convert.ToString(fpspread.Sheets[0].Cells[j, oldcols].Note);
                        tearm2col = cols + 1;
                    }
                    if (FpSpread2.Sheets[0].RowCount > 0)
                    {
                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "Total";
                        double caltotal = 0;
                        for (int i = 0; i < testcolsterm1.Count; i++)
                        {
                            for (int ij = 0; ij < uptosubjrows; ij++)
                            {
                                int isnum = Convert.ToInt32(Convert.ToString(FpSpread2.Sheets[0].Cells[ij, Convert.ToInt32(testcolsterm1[i].ToString())].Note));
                                if (isnum > 0)
                                {
                                    caltotal = caltotal + Convert.ToDouble(Convert.ToString(FpSpread2.Sheets[0].Cells[ij, Convert.ToInt32(Convert.ToString(testcolsterm1[i]))].Text));
                                }


                            }
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, Convert.ToInt32(Convert.ToString(testcolsterm1[i]))].Text = Convert.ToString(caltotal);
                            caltotal = 0;
                        }
                        caltotal = 0;
                        for (int i = 0; i < testcolsterm2.Count; i++)
                        {
                            for (int ij = 0; ij < uptosubjrows; ij++)
                            {
                                int isnum = Convert.ToInt32(FpSpread2.Sheets[0].Cells[ij, Convert.ToInt32(testcolsterm2[i].ToString())].Note.ToString());
                                if (isnum > 0)
                                {
                                    caltotal = caltotal + Convert.ToDouble(FpSpread2.Sheets[0].Cells[ij, Convert.ToInt32(testcolsterm2[i].ToString())].Text);
                                }


                            }
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, Convert.ToInt32(testcolsterm2[i].ToString())].Text = Convert.ToString(caltotal);
                            caltotal = 0;
                        }
                        caltotal = 0;
                        for (int ij = 0; ij < uptosubjrows; ij++)
                        {
                            int isnum = Convert.ToInt32(FpSpread2.Sheets[0].Cells[ij, tearm1col].Text.ToString());
                            if (isnum > 0)
                            {
                                caltotal = caltotal + Convert.ToDouble(FpSpread2.Sheets[0].Cells[ij, tearm1col].Text);
                            }
                        }
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, tearm1col].Text = Convert.ToString(caltotal);
                        caltotal = 0;
                        for (int ij = 0; ij < uptosubjrows; ij++)
                        {
                            int isnum = Convert.ToInt32(FpSpread2.Sheets[0].Cells[ij, tearm2col].Text.ToString());
                            if (isnum > 0)
                            {
                                caltotal = caltotal + Convert.ToDouble(FpSpread2.Sheets[0].Cells[ij, tearm2col].Text);
                            }
                        }
                        if (dropterm.SelectedItem.Text == "2" && currentsem == "2")
                        {
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, tearm2col].Text = Convert.ToString(caltotal);
                        }

                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "Grade";
                        //FpSpread2.Sheets[0].RowCount++;
                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "Attendance     No of Working Days /    No. of Present Days";

                    }

                    FpSpread2.SaveChanges();
                    //FpSpread2.Visible = true;
                    //FpSpread2.Visible = true;

                }
                else
                {

                }
            }
        }

        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void bindstudentdetails_matric(string rollno)
    {
        try
        {
            string ranksect = string.Empty;
            FpSpread2.Height = 500;

            FpSpread2.Width = 1500;
            ArrayList arravoidcol = new ArrayList();
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            DataSet dsphoto = new DataSet();
            DataView dv = new DataView();
            DataTable dtmark = new DataTable();
            DataSet dset = new DataSet();
            int fppagesize = 0;
            string Roll_No = rollno;
            DataSet ds = new DataSet();
            string stdphtsql = string.Empty;
            ArrayList testcolsterm1 = new ArrayList();
            ArrayList testcolsterm2 = new ArrayList();
            string stdappno = string.Empty;
            double caltotala = 0, t1 = 0, t2 = 0, t3 = 0;

            string clm = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No='" + Roll_No + "' ;";
            ds.Clear();
            ds = d2.select_method_wo_parameter(clm, "text");
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    string stdcc = string.Empty;
                    stdcc = Convert.ToString(ds.Tables[1].Rows[0]["Dept_Name"]);

                    stdappno = Convert.ToString(ds.Tables[1].Rows[0]["App_No"]);
                    stdphtsql = "select * from StdPhoto where app_no='" + stdappno + "'";
                    MemoryStream memoryStream = new MemoryStream();
                    DataSet dsstdpho = new DataSet();
                    dsstdpho.Clear();
                    dsstdpho.Dispose();
                    dsstdpho = d2.select_method_wo_parameter(stdphtsql, "Text");
                    if (dsstdpho.Tables[0].Rows.Count > 0)
                    {
                        byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                        memoryStream.Write(file, 0, file.Length);
                        if (file.Length > 0)
                        {
                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                            {

                            }
                            else
                            {
                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                            }
                        }
                    }

                    ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                    dv = ds.Tables[1].DefaultView;
                    int count4 = 0;
                    count4 = dv.Count;
                    DataSet ds1fortable1 = new DataSet();

                    if (ds.Tables[0].Rows.Count > 0 && count4 > 0)
                    {
                        currentsem = Convert.ToString(dv[0]["Current_Semester"]);
                        batchyear = Convert.ToString(dv[0]["Batch_Year"]);
                        degreecode = Convert.ToString(dv[0]["degree_code"]);
                        ranksect = Convert.ToString(dv[0]["Sections"]);
                        string allsem = "1";
                        string admdate = Convert.ToString(dv[0]["adm_date"]);
                        FpSpread2.Sheets[0].RowCount = 0;

                        FpSpread2.Sheets[0].SheetCorner.ColumnCount = 0;
                        FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread2.Sheets[0].ColumnCount = 2;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Subjects";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Max Marks";

                        if (dropterm.SelectedItem.Text == "1")
                        {
                            allsem = "1";

                        }
                        else if (dropterm.SelectedItem.Text == "2")
                        {
                            if (Convert.ToInt32(currentsem) > 1)
                            {
                                int term = Convert.ToInt32(currentsem);
                                for (int i = 2; i <= term; i++)
                                {
                                    allsem = allsem + "'" + "," + "'" + i;
                                }
                            }
                        }

                        string table1sql = "select  * from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'  and semester in ('" + allsem + "') ; select distinct c.criteria,c.Criteria_no,e.max_mark from CriteriaForInternal c,Exam_type e, syllabus_master sy where   sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sy.Batch_Year = '" + batchyear + "'    and sy.degree_code =  '" + degreecode + "' and sy.semester in ('" + allsem + "') ";

                        ds1fortable1.Clear();
                        ds1fortable1.Dispose();
                        int testmarks = 0;
                        ds1fortable1 = d2.select_method_wo_parameter(table1sql, "Text");
                        DataView dvforpage2 = new DataView();
                        // ds1fortable1.Tables[3].DefaultView.RowFilter = "roll_no='" + Roll_No + "'";
                        // dvforpage2 = ds1fortable1.Tables[3].DefaultView;
                        int table1forpage2_rows = ds1fortable1.Tables[0].Rows.Count;
                        //if (dropterm.SelectedItem.Text.Trim() == "2")
                        //{
                        //    table1forpage2_rows = table1forpage2_rows / 2;
                        //}
                        FpSpread2.Sheets[0].RowCount = table1forpage2_rows;
                        if (ds1fortable1.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < table1forpage2_rows; i++)
                            {
                                // FpSpread2.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                                FpSpread2.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                                //FpSpread2.Sheets[0].Cells[i, 1].Text = ds1fortable1.Tables[0].Rows[i][1].ToString();

                                FpSpread2.Sheets[0].Cells[i, 0].Tag = Convert.ToString(ds1fortable1.Tables[0].Rows[i]["subject_no"]);
                                FpSpread2.Sheets[0].Cells[i, 0].Text = Convert.ToString(ds1fortable1.Tables[0].Rows[i]["subject_name"]);
                                FpSpread2.Sheets[0].Cells[i, 1].Text = Convert.ToString(ds1fortable1.Tables[0].Rows[i]["maxtotal"]);

                            }

                            if (ds1fortable1.Tables[1].Rows.Count > 0)
                            {

                                for (int i = 0; i < ds1fortable1.Tables[1].Rows.Count; i++)
                                {
                                    FpSpread2.Sheets[0].ColumnCount++;
                                    // FpSpread2.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    //FpSpread2.Sheets[0].Cells[i, 1].Text = ds1fortable1.Tables[0].Rows[i][1].ToString();

                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds1fortable1.Tables[1].Rows[i]["Criteria_no"]);
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds1fortable1.Tables[1].Rows[i]["criteria"]);
                                    // FpSpread2.Sheets[0].Cells[i, 1].Text = ds1fortable1.Tables[0].Rows[i]["maxtotal"].ToString();

                                }
                            }
                        }
                    }
                    string strsubjno = string.Empty;
                    string cretianumb = string.Empty;
                    int cols = 0;
                    int oldcols = 0;
                    int uptosubjrows = FpSpread2.Sheets[0].RowCount;
                    int tearm1col = 0;
                    int tearm2col = 0;
                    for (int j = 0; j < FpSpread2.Sheets[0].RowCount; j++)
                    {
                        for (int i = 2; i < FpSpread2.Sheets[0].ColumnCount; i++)
                        {
                            cols = Convert.ToInt32(i);
                            strsubjno = Convert.ToString(FpSpread2.Sheets[0].Cells[j, 0].Tag);
                            cretianumb = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, i].Tag);
                            string getmarks = d2.GetFunction("SELECT marks_obtained FROM Result u,Registration r,Exam_type e,CriteriaForInternal c   WHERE U.roll_no = R.Roll_No AND U.exam_code = E.exam_code AND E.criteria_no = C.Criteria_no    AND subject_no = '" + strsubjno + "' and R.Roll_No= '" + Roll_No + "' and c.criteria_no='" + cretianumb + "' order by u.roll_no,STUD_NAME,e.exam_date,c.criteria_no");
                            int getmm = Convert.ToInt32(getmarks);
                            if (getmm > 0)
                            {
                                FpSpread2.Sheets[0].Cells[j, cols].Text = Convert.ToString(getmarks);
                                FpSpread2.Sheets[0].Cells[j, cols].Note = Convert.ToString(getmarks);
                            }
                            else
                            {
                                FpSpread2.Sheets[0].Cells[j, cols].Note = Convert.ToString(getmarks);
                                getmarks = loadmarkat(getmarks);
                                FpSpread2.Sheets[0].Cells[j, cols].Text = Convert.ToString(getmarks);
                            }
                        }
                    }






                    if (FpSpread2.Sheets[0].RowCount > 0)
                    {
                        // ----------- total
                        //string totl = ds1fortable1.Tables[0].Rows[0]["maxtotal"].ToString();
                        //ds1fortable1.Tables[0].DefaultView.RowFilter = "maxtotal='" + totl + "'";

                        int tot = 0, tot2 = testcolsterm1.Count;
                        double tot1 = 0;

                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "Total";

                        double caltotal = 0;
                        for (int i = 2; i < FpSpread2.Sheets[0].ColumnCount; i++)
                        {
                            //tot2--;
                            for (int ij = 0; ij < uptosubjrows; ij++)
                            {
                                string totl = Convert.ToString(ds1fortable1.Tables[0].Rows[ij]["maxtotal"]);
                                tot = tot + Convert.ToInt32(totl);
                                int dummy;
                                if (Int32.TryParse(FpSpread2.Sheets[0].Cells[ij, Convert.ToInt32(i)].Text, out dummy))
                                {
                                    caltotal = caltotal + Convert.ToDouble(FpSpread2.Sheets[0].Cells[ij, Convert.ToInt32(i)].Text);
                                }
                                else
                                {
                                    //caltotal = 0;
                                }
                            }
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, i].Text = Convert.ToString(caltotal);
                            caltotal = 0;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(tot);
                            tot = 0;
                        }

                        caltotal = 0;
                        for (int i = 0; i < testcolsterm2.Count; i++)
                        {
                            for (int ij = 0; ij < uptosubjrows; ij++)
                            {
                                int isnum = Convert.ToInt32(FpSpread2.Sheets[0].Cells[ij, Convert.ToInt32(testcolsterm2[i].ToString())].Note.ToString());
                                if (isnum > 0)
                                {
                                    caltotal = caltotal + Convert.ToDouble(FpSpread2.Sheets[0].Cells[ij, Convert.ToInt32(testcolsterm2[i].ToString())].Text);
                                }
                            }
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, Convert.ToInt32(testcolsterm2[i].ToString())].Text = Convert.ToString(caltotal);
                            caltotal = 0;
                        }


                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "Rank";

                        for (int rank = 2; rank < FpSpread2.Sheets[0].ColumnCount; rank++)
                        {
                            //  string criternum = FpSpread2.Sheets[0].ColumnHeader.Cells[0, rank].Text.ToString();
                            string criternum = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, rank].Tag);

                            calrank(batchyear, ranksect, criternum, rollno, rank);
                            // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, rank].Text = "I";
                        }
                        FpSpread2.Sheets[0].RowCount++;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "Class Average";
                        caltotal = 0;
                        tot = 0;
                        for (int i = 2; i < FpSpread2.Sheets[0].ColumnCount; i++)
                        {
                            string criternum = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, i].Tag);
                            for (int ij = 0; ij < uptosubjrows; ij++)
                            {
                                string totl = Convert.ToString(ds1fortable1.Tables[0].Rows[ij]["maxtotal"]);
                                tot = tot + Convert.ToInt32(totl);

                            }
                            string totstudent = d2.GetFunction("(select count(Registration.roll_no) as count   from registration, applyn a where a.app_no=registration.app_no and registration.degree_code='" + degreecode + "'    and registration.current_semester='" + currentsem + "' and registration.batch_year='" + batchyear + "'  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR'    and delflag=0 )");
                            tot = tot * Convert.ToInt32(totstudent);
                            double overallmark_ob_tot = Convert.ToDouble(d2.GetFunction("SELECT sum(marks_obtained) as total FROM Result u,Registration r,Exam_type e,CriteriaForInternal c   WHERE U.roll_no = R.Roll_No     AND U.exam_code = E.exam_code AND E.criteria_no = C.Criteria_no        and c.criteria_no='" + criternum + "' and R.roll_no in (select Registration.roll_no   from registration, applyn a where a.app_no=registration.app_no and registration.degree_code='" + degreecode + "'    and registration.current_semester='" + currentsem + "' and registration.batch_year='" + batchyear + "'  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR'    and delflag=0 )"));
                            overallmark_ob_tot = overallmark_ob_tot / tot;
                            overallmark_ob_tot = Math.Round(overallmark_ob_tot, 2);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, i].Text = Convert.ToString(overallmark_ob_tot);
                            overallmark_ob_tot = 0;
                            tot = 0;
                        }

                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "Student Average";
                        caltotal = 0;
                        for (int i = 2; i < FpSpread2.Sheets[0].ColumnCount; i++)
                        {
                            for (int ij = 0; ij < uptosubjrows; ij++)
                            {
                                string totl = Convert.ToString(ds1fortable1.Tables[0].Rows[ij]["maxtotal"]);
                                tot = tot + Convert.ToInt32(totl);
                                int dummy;
                                if (Int32.TryParse(FpSpread2.Sheets[0].Cells[ij, Convert.ToInt32(i)].Text, out dummy))
                                {
                                    caltotal = caltotal + Convert.ToDouble(FpSpread2.Sheets[0].Cells[ij, Convert.ToInt32(i)].Text);
                                }
                                else
                                {
                                    // caltotal = 0;
                                }
                            }
                            double coltotal1 = Convert.ToDouble(caltotal) / uptosubjrows;
                            double passper = Math.Round(coltotal1, 2);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, i].Text = Convert.ToString(passper);
                            caltotal = 0; coltotal1 = 0; tot = 0;
                        }
                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text =string.Empty;
                    }
                    FpSpread2.SaveChanges();
                }
                else
                {

                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            collcode = " and r.college_code='" + Convert.ToString(ddschool.SelectedItem.Value) + "'";
            batchyear = " and r.Batch_Year='" + Convert.ToString(dropyear.SelectedItem.Text) + "'";
            degreecode = " and r.degree_code='" + Convert.ToString(ddstandard.SelectedItem.Value) + "'";
            // term = "and sc.semester='" + dropterm.SelectedItem.Text.ToString() + "'";     
            lblerrormsg.Text = string.Empty;
            lblerrormsg.Visible = false;

            if (ddlreporttype.Items.Count == 0)
            {
                FpSpread1.Visible = false;
                btngrade.Visible = false;
                btnrpt.Visible = false;

                lblerrormsg.Text = "Please Check The Rights For Report Type And Change The Rights";
                lblerrormsg.Visible = true;
                return;
            }

            if (dropsec.Enabled == true)
            {
                //sec = dropsec.SelectedItem.Text.Trim();
                // --------------- add start
                if (dropsec.SelectedItem.Text != "All")
                {
                    for (int sc = 0; sc < dropsec.Items.Count; sc++)
                    {
                        sec = "and r.Sections in ('" + Convert.ToString(dropsec.SelectedItem.Text) + "')";
                    }
                }
                else
                {
                    sec = string.Empty;
                }
            }
            else
            {
                sec = string.Empty;
            }
            for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
            {
                FpSpread1.Sheets[0].Cells[i, 1].Value = 0;

            }
            sqlcondition = collcode + batchyear + degreecode + sec;

            strorderby = d2.GetFunction("select LinkValue from inssettings where college_code=" + Convert.ToString(collegecode) + " and linkname='Student Attendance'");

            if (strorderby == "1")
            {
                serialflag = true;
            }
            else
            {
                serialflag = false;
            }
            strorderby = d2.GetFunction("select value from Master_Settings where settings='order_by'");
            if (strorderby == "")
            {
                strorderby = string.Empty;
            }
            else
            {
                if (strorderby == "0")
                {
                    strorderby = "ORDER BY r.Roll_No";
                }
                else if (strorderby == "1")
                {
                    strorderby = "ORDER BY r.Reg_No";
                }
                else if (strorderby == "2")
                {
                    strorderby = "ORDER BY r.Stud_Name";
                }
                else if (strorderby == "0,1,2")
                {
                    strorderby = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                }
                else if (strorderby == "0,1")
                {
                    strorderby = "ORDER BY r.Roll_No,r.Reg_No";
                }
                else if (strorderby == "1,2")
                {
                    strorderby = "ORDER BY r.Reg_No,r.Stud_Name";
                }
                else if (strorderby == "0,2")
                {
                    strorderby = "ORDER BY r.Roll_No,r.Stud_Name";
                }
            }

            if (serialflag == false)
            {

                sql = "SELECT distinct r.Roll_No,R.Stud_Name,a.sex,serialno,r.Reg_No FROM Registration R,Applyn A WHERE R.App_No = A.App_No     " + sqlcondition + " and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' " + strorderby + ""; //and r.Current_Semester<='" + Convert.ToString(dropterm.SelectedValue) + "'
            }
            else
            {
                sql = "SELECT distinct r.Roll_No,R.Stud_Name,a.sex,serialno,r.Reg_No FROM Registration R,Applyn A WHERE R.App_No = A.App_No     " + sqlcondition + " and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' ORDER BY serialno";// and r.Current_Semester<='" + Convert.ToString(dropterm.SelectedValue) + "'
            }

            studgradeds.Clear();
            studgradeds = d2.select_method_wo_parameter(sql, "Text");
            if (studgradeds.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Sheets[0].Rows.Count = studgradeds.Tables[0].Rows.Count + 1;
                for (int i = 0; i < studgradeds.Tables[0].Rows.Count; i++)
                {
                    FpSpread1.Sheets[0].Cells[i + 1, 1].CellType = chkboxcol;
                    FpSpread1.Sheets[0].Cells[i + 1, 0].Text = Convert.ToString(i + 1);
                    FpSpread1.Sheets[0].Cells[i + 1, 2].CellType = txtceltype;
                    FpSpread1.Sheets[0].Cells[i + 1, 2].Text = Convert.ToString(studgradeds.Tables[0].Rows[i][0]);
                    FpSpread1.Sheets[0].Cells[i + 1, 3].Text = Convert.ToString(studgradeds.Tables[0].Rows[i][1]);

                    FpSpread1.Sheets[0].Cells[i + 1, 2].Tag = Convert.ToString(studgradeds.Tables[0].Rows[i][3]);

                    string ssex = Convert.ToString(studgradeds.Tables[0].Rows[i][2]);
                    if (ssex.Trim() == "0")
                    {
                        ssex = "Male";
                    }
                    else
                    {
                        ssex = "Female";
                    }
                    FpSpread1.Sheets[0].Cells[i + 1, 4].Text = ssex;

                }
                for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    FpSpread1.Sheets[0].Rows[i].BackColor = ColorTranslator.FromHtml("#E6e6e6");
                    i++;
                }
                FpSpread1.SaveChanges();
                FpSpread1.Visible = true;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                //btngrade.Visible = true;
                //btnrpt.Visible = true;
                if (Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 3)
                {
                    btngrade.Visible = false;
                    lblErr.Text = string.Empty;
                    lblErr.Visible = false;
                    FpSpread1.Visible = true;
                    btnrpt.Visible = true;
                }
                else
                {
                    btngrade.Visible = true;
                }

                if (Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 4)
                {
                    btngrade.Visible = false;
                    btnmatric_page1.Visible = true;
                    btnmatric_page2.Visible = true;
                    btnrpt.Visible = false;
                }
                if (Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 5 || Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 10)
                {
                    lblErr.Text = string.Empty;
                    lblErr.Visible = false;
                    if (txt_Test.Text != "---Select---")
                    {
                        lblErr.Text = string.Empty;
                        lblErr.Visible = false;
                        FpSpread1.Visible = true;
                        btnrpt.Visible = true;
                    }
                    FpSpread1.Visible = true;
                    btnrpt.Visible = true;
                    btngrade.Visible = false;
                }
                if (Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 6 || Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 7 || Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 8 || Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 9 || Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 11)
                {
                    lblErr.Text = string.Empty;
                    lblErr.Visible = false;
                    FpSpread1.Visible = true;
                    btnrpt.Visible = true;
                    btngrade.Visible = false;
                }
                if (Convert.ToInt32(ddlreporttype.SelectedItem.Value) == 0)
                {
                    btngrade.Visible = true;
                    btnrpt.Visible = true;
                }
            }
            else
            {
                FpSpread1.Visible = false;
                lblerrormsg.Text = string.Empty;
                lblerrormsg.Visible = false;
                lblErr.Text = "There are no students available.";
                lblErr.Visible = true;
                btngrade.Visible = false;
                btnrpt.Visible = false;
                btnmatric_page1.Visible = false;
                btnmatric_page2.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void bindsec()
    {
        try
        {
            dropsec.Enabled = false;
            dropsec.Items.Clear();
            hat.Clear();
            studgradeds.Clear();
            studgradeds = d2.BindSectionDetail(dropyear.SelectedValue, ddstandard.SelectedValue);
            int count5 = studgradeds.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                dropsec.DataSource = studgradeds;
                dropsec.DataTextField = "sections";
                dropsec.DataValueField = "sections";
                dropsec.DataBind();
                dropsec.Enabled = true;
                dropsec.Items.Insert(0, "All");
            }
            else
            {
                dropsec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    protected void chkaccheader_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkaccheader.Checked == true)
            {
                for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                {
                    treeview_spreadfields.Nodes[remv].Checked = true;
                    txtaccheader.Text = "Header(" + (treeview_spreadfields.Nodes.Count) + ")";
                    if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                    {
                        for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                        {
                            treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked = true;
                        }
                    }
                }
            }
            else
            {
                for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                {
                    treeview_spreadfields.Nodes[remv].Checked = false;
                    txtaccheader.Text = "---Select---";
                    if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                    {
                        for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                        {
                            treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void loadheader()
    {
        try
        {
            string batch_year = Convert.ToString(dropyear.SelectedItem.Text);
            string degree_code = Convert.ToString(ddstandard.SelectedItem.Value);
            string buildvalue1 = string.Empty;
            ds.Reset();
            ds.Dispose();
            treeview_spreadfields.Nodes.Clear();
            for (int i = 0; i < dropterm.Items.Count; i++)
            {
                if (Convert.ToInt32(Convert.ToString(dropterm.SelectedItem.Text)) > i)
                {
                    if (buildvalue1 == "")
                    {
                        buildvalue1 = Convert.ToString(i + 1);
                    }
                    else
                    {
                        buildvalue1 = buildvalue1 + "','" + Convert.ToString(i + 1);
                    }
                }
            }
            string straccheadquery = "SELECT distinct  y.semester FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batch_year + "' and degree_code = '" + degree_code + "'  and semester in ('" + buildvalue1 + "')  and CRITERIA_NO <>''  order by semester";
            //string straccheadquery = "SELECT distinct  Istype,CRITERIA_NO,y.semester,M.Conversion_value FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batch_year + "' and degree_code = '" + degree_code + "' and semester in ('" + buildvalue1 + "')  and CRITERIA_NO <>''  order by semester";
            //string straccheadquery = "select distinct a.header_id,a.header_name from chlheadersettings c,Acctheader a where c.Header_ID=a.header_id and a.header_name not in ('arrear') " + type + "";
            ds = da.select_method_wo_parameter(straccheadquery, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    node = new TreeNode(Convert.ToString(ds.Tables[0].Rows[i]["semester"]), Convert.ToString(ds.Tables[0].Rows[i]["semester"]));
                    string strled = "SELECT distinct  Istype,CRITERIA_NO,y.semester,M.Conversion_value FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batch_year + "' and degree_code = '" + degree_code + "' and semester in ('" + Convert.ToString(ds.Tables[0].Rows[i]["semester"]) + "')  and CRITERIA_NO <>''  order by semester";
                    ds1 = da.select_method_wo_parameter(strled, "Text");
                    for (int ledge = 0; ledge < ds1.Tables[0].Rows.Count; ledge++)
                    {
                        subchildnode = new TreeNode(Convert.ToString(ds1.Tables[0].Rows[ledge]["Istype"]), Convert.ToString(ds1.Tables[0].Rows[ledge]["CRITERIA_NO"]));
                        subchildnode.ShowCheckBox = true;
                        node.ChildNodes.Add(subchildnode);
                    }
                    node.ShowCheckBox = true;
                    treeview_spreadfields.Nodes.Add(node);
                }
                if (chkaccheader.Checked == true)
                {

                    for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                    {
                        treeview_spreadfields.Nodes[remv].Checked = true;
                        txtaccheader.Text = "Header(" + (treeview_spreadfields.Nodes.Count) + ")";
                        if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                        {
                            for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                            {
                                treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked = true;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void bindbutn_four(string rollno)
    {
        try
        {
            string sagradeonly = string.Empty;
            string fagradeonly = string.Empty;

            strorderby = d2.GetFunction("select LinkValue from inssettings where college_code=" + Convert.ToString(collegecode) + " and linkname='Student Attendance'");

            if (strorderby == "1")
            {
                serialflag = true;
            }
            else
            {
                serialflag = false;
            }
            strorderby = d2.GetFunction("select value from Master_Settings where settings='order_by'");
            if (strorderby == "")
            {
                strorderby = string.Empty;
            }
            else
            {
                if (strorderby == "0")
                {
                    strorderby = "ORDER BY r.Roll_No";
                }
                else if (strorderby == "1")
                {
                    strorderby = "ORDER BY r.Reg_No";
                }
                else if (strorderby == "2")
                {
                    strorderby = "ORDER BY r.Stud_Name";
                }
                else if (strorderby == "0,1,2")
                {
                    strorderby = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                }
                else if (strorderby == "0,1")
                {
                    strorderby = "ORDER BY r.Roll_No,r.Reg_No";
                }
                else if (strorderby == "1,2")
                {
                    strorderby = "ORDER BY r.Reg_No,r.Stud_Name";
                }
                else if (strorderby == "0,2")
                {
                    strorderby = "ORDER BY r.Roll_No,r.Stud_Name";
                }
            }
            gradesystemfa.Clear();
            gradesystemsa.Clear();
            string partone = string.Empty;
            gradesystemfa.Add("FS1");
            gradesystemfa.Add("FS2");
            gradesystemfa.Add("FS3");

            gradesystemsa.Add("SA1");
            gradesystemsa.Add("SA2");
            gradesystemsa.Add("SA3");

            dtallcol.Clear();
            dtallcol.Columns.Clear();

            Boolean firsttime = true;
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;
            Gios.Pdf.PdfPage mypdfpage1;
            Gios.Pdf.PdfPage mypdfpage2;
            Gios.Pdf.PdfPage mypdfpage6;
            Gios.Pdf.PdfPage mypdfpagefinal;
            Gios.Pdf.PdfPage mypdfpage5;
            DataSet ds = new DataSet();
            DataSet dschool = new DataSet();
            DAccess2 da = new DAccess2();
            DataSet dset = new DataSet();
            int checkattalign = 650;
            string college_code = Convert.ToString(collegecode);
            string stdappno = string.Empty;
            System.Drawing.Font Fontboldhead = new System.Drawing.Font("Times New Roman", 10, FontStyle.Bold);
            System.Drawing.Font Fontbold = new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold);
            System.Drawing.Font Fontbolda = new System.Drawing.Font("Times New Roman", 14, FontStyle.Bold);
            System.Drawing.Font Fontbold1 = new System.Drawing.Font("Times New Roman", 16, FontStyle.Bold);
            System.Drawing.Font Fontmedium = new System.Drawing.Font("Times New Roman", 14, FontStyle.Regular);
            System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Times New Roman", 14, FontStyle.Bold);
            System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Times New Roman", 9, FontStyle.Regular);
            System.Drawing.Font Fontsmall = new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular);
            System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Times New Roman", 10, FontStyle.Regular);
            System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            System.Drawing.Font Fontmediumv = new System.Drawing.Font("Times New Roman", 10, FontStyle.Regular);
            System.Drawing.Font Fontmedium1V = new System.Drawing.Font("Times New Roman", 10, FontStyle.Regular);

            System.Drawing.Font f1 = new System.Drawing.Font("Times New Roman", 7, FontStyle.Regular);
            System.Drawing.Font f2 = new System.Drawing.Font("Times New Roman", 8, FontStyle.Regular);
            System.Drawing.Font f3 = new System.Drawing.Font("Times New Roman", 9, FontStyle.Regular);
            System.Drawing.Font f4 = new System.Drawing.Font("Times New Roman", 10, FontStyle.Regular);
            System.Drawing.Font f5 = new System.Drawing.Font("Times New Roman", 11, FontStyle.Regular);
            System.Drawing.Font f6 = new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular);
            System.Drawing.Font f16 = new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular);

            System.Drawing.Font f7 = new System.Drawing.Font("Times New Roman", 7, FontStyle.Bold);
            System.Drawing.Font f8 = new System.Drawing.Font("Times New Roman", 8, FontStyle.Bold);
            System.Drawing.Font f9 = new System.Drawing.Font("Times New Roman", 9, FontStyle.Bold);
            System.Drawing.Font f10 = new System.Drawing.Font("Times New Roman", 10, FontStyle.Bold);
            System.Drawing.Font f11 = new System.Drawing.Font("Times New Roman", 11, FontStyle.Bold);
            System.Drawing.Font f12 = new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold);
            rollnos = rollno;
            if (rollnos != "")
            {
                if (serialflag == false)
                {
                    sql = "select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile,parentF_Mobile,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,bldgrp,studhouse from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No in ('" + rollnos + "') " + strorderby + "";
                }
                else
                {
                    sql = "select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile,parentF_Mobile,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,bldgrp,studhouse,serialno from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No in ('" + rollnos + "') order by serialno";
                }

                studgradeds.Clear();
                studgradeds = d2.select_method_wo_parameter(sql, "text");
                if (studgradeds.Tables[0].Rows.Count > 0)
                {
                    for (int roll = 0; roll < studgradeds.Tables[0].Rows.Count; roll++)
                    {
                        checkattalign = 620;
                        string rcrollno = string.Empty;
                        rcrollno = Convert.ToString(studgradeds.Tables[0].Rows[roll][1]);
                        string Roll_No = rcrollno;
                        rollnos = rcrollno;
                        sql = "SELECT collname,affliatedby,address1,address2,district,address3,pincode,email,logo1,logo2,email,'Phone: '+phoneno+' Email ID: '+email+' Website: '+website+' 'as Line3,college_code from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.college_code,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile,parentF_Mobile,StuPer_Id,parent_addressc,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,a.bldgrp,studhouse,Districtp,parent_statep,parent_pincodep,parentM_Mobile,countryp,serialno,a.EMIS_Number,a.Aadhaar_Enroll_No from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No='" + Roll_No + "'";
                        ds.Clear();
                        ds.Dispose();
                        ds = da.select_method_wo_parameter(sql, "Text");
                        DataView dv = new DataView();
                        int count4 = 0;
                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                        {
                            ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                            dv = ds.Tables[1].DefaultView;
                            count4 = dv.Count;
                        }
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && count4 > 0)
                        {
                            string serialno = Convert.ToString(dv[0]["serialno"]);//Roll_No
                            string currentsem = Convert.ToString(dv[0]["Current_Semester"]);
                            string batchyear = Convert.ToString(dv[0]["Batch_Year"]);
                            string degreecode = Convert.ToString(dv[0]["degree_code"]);
                            stdappno = Convert.ToString(dv[0]["App_No"]);
                            string emisNo = Convert.ToString(dv[0]["EMIS_Number"]).Trim();
                            string aadhaarNo = Convert.ToString(dv[0]["Aadhaar_Enroll_No"]).Trim();
                            string allsem = "1";
                            string admdate = Convert.ToString(dv[0]["adm_date"]);
                            string line3 = Convert.ToString(ds.Tables[0].Rows[0]["Line3"]).Trim();
                            string collegeCode = Convert.ToString(dv[0]["college_code"]).Trim();

                            DataTable dtCollegeInfo = new DataTable();
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                ds.Tables[0].DefaultView.RowFilter = "college_code='" + collegeCode + "'";
                                dtCollegeInfo = ds.Tables[0].DefaultView.ToTable();
                            }
                            string stdcc = string.Empty;
                            stdcc = Convert.ToString(ds.Tables[1].Rows[0]["Dept_Name"]);
                            string table1sql = "select * from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'  and semester = '" + currentsem + "' ; SELECT COUNT(*) as facount FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "'); SELECT Istype,CRITERIA_NO,y.semester FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "') ; SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "')";
                            DataSet ds1fortable1 = new DataSet();
                            ds1fortable1.Clear();
                            ds1fortable1.Dispose();
                            ds1fortable1 = da.select_method_wo_parameter(table1sql, "Text");
                            DataView dvforpage2 = new DataView();

                            string dob = Convert.ToString(dv[0]["dob"]);
                            string[] dobspit = dob.Split('/');
                            string addressline1 = Convert.ToString(dv[0]["parent_addressP"]);
                            //addressline1 = addressline1 + " " + Convert.ToString(dv[0]["Streetp"]);
                            string addressline2 = string.Empty;

                            if (Convert.ToString(dv[0]["Cityp"]).Trim() != "" && Convert.ToString(dv[0]["Streetp"]).Trim() != "")
                            {
                                addressline2 = Convert.ToString(dv[0]["Streetp"]) + ", " + Convert.ToString(dv[0]["Cityp"]);
                            }
                            else if (Convert.ToString(dv[0]["Streetp"]).Trim() != "" && Convert.ToString(dv[0]["Cityp"]).Trim() == "")
                            {
                                addressline2 = Convert.ToString(dv[0]["Streetp"]);
                            }
                            else if (Convert.ToString(dv[0]["Streetp"]).Trim() == "" && Convert.ToString(dv[0]["Cityp"]).Trim() != "")
                            {
                                addressline2 = Convert.ToString(dv[0]["Cityp"]);
                            }

                            string parentstatt = Convert.ToString(dv[0]["parent_statep"]);
                            parentstatt = d2.GetFunction("select distinct textval from applyn a,textvaltable t where a.parent_statep = t.TextCode and t.TextCode='" + parentstatt + "'");

                            if (parentstatt.Trim() == "" || parentstatt.Trim() == "0")
                            {
                                parentstatt = string.Empty;
                            }

                            string addressline3 = Convert.ToString(dv[0]["Districtp"]) + ", " + parentstatt + ", " + Convert.ToString(dv[0]["parent_pincodep"]);

                            if (Convert.ToString(dv[0]["Districtp"]).Trim() != "" && parentstatt != "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() != "")
                            {
                                addressline3 = Convert.ToString(dv[0]["Districtp"]) + ", " + parentstatt + ", " + Convert.ToString(dv[0]["parent_pincodep"]);
                            }
                            else if (dv[0]["Districtp"].ToString().Trim() != "" && parentstatt != "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() == "")
                            {
                                addressline3 = dv[0]["Districtp"].ToString() + ", " + parentstatt;
                            }
                            else if (dv[0]["Districtp"].ToString().Trim() != "" && parentstatt == "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() != "")
                            {
                                addressline3 = Convert.ToString(dv[0]["Districtp"]) + ", " + Convert.ToString(dv[0]["parent_pincodep"]);
                            }
                            else if (Convert.ToString(dv[0]["Districtp"]).Trim() == "" && parentstatt != "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() != "")
                            {
                                addressline3 = parentstatt + ", " + Convert.ToString(dv[0]["parent_pincodep"]);
                            }
                            else if (dv[0]["Districtp"].ToString().Trim() == "" && parentstatt == "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() != "")
                            {
                                addressline3 = Convert.ToString(dv[0]["parent_pincodep"]);
                            }
                            else if (dv[0]["Districtp"].ToString().Trim() != "" && parentstatt == "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() == "")
                            {
                                addressline3 = dv[0]["Districtp"].ToString();
                            }
                            else if (dv[0]["Districtp"].ToString().Trim() == "" && parentstatt != "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() == "")
                            {
                                addressline3 = parentstatt;
                            }
                            else if (dv[0]["Districtp"].ToString().Trim() == "" && parentstatt == "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() == "")
                            {
                                addressline3 = string.Empty;
                            }

                            string parentcountry = dv[0]["countryp"].ToString();

                            int num = 0;
                            if (int.TryParse(parentcountry, out num))
                            {
                                parentcountry = d2.GetFunction("select distinct textval from textvaltable t where  t.TextCode='" + parentcountry + "'");
                            }
                            if (parentcountry.Trim() == "" || parentcountry.Trim() == "0" || parentcountry == null)
                            {
                                parentcountry = string.Empty;
                            }

                            if (addressline3.Trim() != "" && parentcountry.Trim() != "")
                            {
                                addressline3 = addressline3 + ", " + parentcountry;
                            }
                            string mobileno = dv[0]["parentF_Mobile"].ToString() + "/" + dv[0]["parentM_Mobile"].ToString();

                            if (dv[0]["parentF_Mobile"].ToString().Trim() != "" && dv[0]["parentM_Mobile"].ToString().Trim() != "")
                            {
                                mobileno = dv[0]["parentF_Mobile"].ToString() + " / " + dv[0]["parentM_Mobile"].ToString();
                            }
                            else if (dv[0]["parentF_Mobile"].ToString().Trim() != "" && dv[0]["parentM_Mobile"].ToString().Trim() == "")
                            {
                                mobileno = dv[0]["parentF_Mobile"].ToString();
                            }
                            else if (dv[0]["parentF_Mobile"].ToString().Trim() == "" && dv[0]["parentM_Mobile"].ToString().Trim() != "")
                            {
                                mobileno = dv[0]["parentM_Mobile"].ToString();
                            }
                            else if (dv[0]["parentF_Mobile"].ToString().Trim() == "" && dv[0]["parentM_Mobile"].ToString().Trim() == "")
                            {
                                mobileno = string.Empty;
                            }
                            // addressline2 = addressline1 + ", " + addressline2 + " - " + Convert.ToString(dv[0]["parent_pincodep"]);

                            int moveleftvalue = 30;
                            mypdfpage = mydoc.NewPage();
                            mypdfpage1 = mydoc.NewPage();
                            mypdfpage2 = mydoc.NewPage();
                            mypdfpage6 = mydoc.NewPage();

                            PdfTextArea pdf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 40, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(dtCollegeInfo.Rows[0][0]) + "");
                            PdfTextArea pdf11 = new PdfTextArea(f16, System.Drawing.Color.Black, new PdfArea(mydoc, 40, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(dtCollegeInfo.Rows[0]["address1"]).Trim().Trim(',') + ", " + dtCollegeInfo.Rows[0]["address2"].ToString().ToUpper() + " " + dtCollegeInfo.Rows[0]["district"].ToString().ToUpper() + " - " + dtCollegeInfo.Rows[0]["pincode"].ToString().ToUpper() + "");
                            string address = Convert.ToString(dtCollegeInfo.Rows[0]["address1"]) + "" + Convert.ToString(dtCollegeInfo.Rows[0]["address2"]) + "" + Convert.ToString(dtCollegeInfo.Rows[0]["district"]);
                            PdfTextArea pdf12 = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 40, 55, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + line3);

                            //PdfTextArea pdf172 = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 70, 595, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  :" +  Convert.ToString(dtCollegeInfo.Rows[0][6]));
                            // PdfTextArea pdf172 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 50, 595, 50), System.Drawing.ContentAlignment.TopCenter, "STUDENT CUMULATIVE RECORD");


                            PdfArea pa1 = new PdfArea(mydoc, 42, 5, 545, 834);

                            PdfArea pahealth = new PdfArea(mydoc, 2, 765, 591, 70);
                            PdfRectangle pr1 = new PdfRectangle(mydoc, pa1, Color.Black);
                            PdfArea pa2 = new PdfArea(mydoc, 189, 175, 224, 40);
                            //PdfRectangle pr2 = new PdfRectangle(mydoc, pa2, Color.Black);

                            mypdfpage.Add(pdf1);
                            mypdfpage.Add(pdf11);
                            mypdfpage.Add(pdf12);
                            mypdfpage.Add(pr1);

                            string sqlschool = "select value from Master_Settings where settings='Academic year'";
                            dschool = da.select_method_wo_parameter(sqlschool, "Text");
                            string splitvalue = Convert.ToString(dschool.Tables[0].Rows[0]["value"]);
                            string[] dsplit = splitvalue.Split(',');

                            string fvalue = Convert.ToString(dsplit[0]);
                            string lvalue = Convert.ToString(dsplit[1]);
                            string acdmic_date = fvalue + "-" + lvalue;
                            PdfTextArea pdf_acadamicyear = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 85, 595, 50), System.Drawing.ContentAlignment.TopCenter, "Academic Year: " + acdmic_date + "");
                            mypdfpage.Add(pdf_acadamicyear);

                            pdf_acadamicyear = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 70, 595, 50), System.Drawing.ContentAlignment.TopCenter, "STUDENT CUMULATIVE RECORD");
                            mypdfpage.Add(pdf_acadamicyear);

                            pdf_acadamicyear = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 105, 595, 50), System.Drawing.ContentAlignment.TopCenter, "-----------------------------------------------------------------------------------------------------------------------------------");
                            mypdfpage.Add(pdf_acadamicyear);


                            Gios.Pdf.PdfTable tablestanes1 = mydoc.NewTable(f11, 1, 12, 1);
                            tablestanes1.VisibleHeaders = false;
                            tablestanes1.SetBorders(Color.Black, 1, BorderType.None);
                            tablestanes1.SetColumnsWidth(new int[] { 30, 5, 170, 48, 5, 50, 38, 5, 70, 31, 5, 44 });

                            tablestanes1.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 0).SetContent("Name");
                            tablestanes1.Cell(0, 1).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 1).SetContent(":");
                            tablestanes1.Cell(0, 1).SetCellPadding(1);
                            tablestanes1.Cell(0, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 2).SetContent(Convert.ToString(dv[0]["stud_name"]));
                            tablestanes1.Cell(0, 2).SetFont(f4);

                            tablestanes1.Cell(0, 3).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 3).SetContent("Std & Sec");
                            tablestanes1.Cell(0, 4).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 4).SetContent(":");
                            tablestanes1.Cell(0, 5).SetContentAlignment(ContentAlignment.TopLeft);
                            string stdsec = string.Empty;

                            if (Convert.ToString(dv[0]["Sections"]).Trim() == "")
                            {
                                stdsec = " " + ddstandard.SelectedItem.Text.ToString();
                            }
                            else
                            {
                                stdsec = " " + ddstandard.SelectedItem.Text.ToString() + " - " + Convert.ToString(dv[0]["Sections"]) + "";
                            }

                            tablestanes1.Cell(0, 5).SetContent(stdsec);
                            tablestanes1.Cell(0, 5).SetFont(f4);

                            tablestanes1.Cell(0, 6).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 6).SetContent("Roll No");
                            tablestanes1.Cell(0, 7).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 7).SetContent(":");
                            tablestanes1.Cell(0, 8).SetContentAlignment(ContentAlignment.TopLeft);
                            //tablestanes1.Cell(0, 11).SetContent( Convert.ToString(dv[0]["Roll_No"]));
                            tablestanes1.Cell(0, 8).SetContent(serialno);
                            tablestanes1.Cell(0, 8).SetFont(f4);

                            tablestanes1.Cell(0, 9).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 9).SetContent("D.O.B");
                            tablestanes1.Cell(0, 10).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 10).SetContent(":");
                            tablestanes1.Cell(0, 11).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 11).SetContent(Convert.ToString(dv[0]["dob"]));
                            tablestanes1.Cell(0, 11).SetFont(f4);

                            Gios.Pdf.PdfTablePage newpdftabpage2 = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, 130, 530, 250));
                            mypdfpage.Add(newpdftabpage2);

                            tablestanes1 = mydoc.NewTable(f11, 1, 12, 1);
                            tablestanes1.VisibleHeaders = false;
                            tablestanes1.SetBorders(Color.Black, 1, BorderType.None);
                            tablestanes1.SetColumnsWidth(new int[] { 83, 5, 165, 40, 5, 82, 76, 5, 85, 23, 5, 33 });

                            tablestanes1.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 0).SetContent("Admission No");
                            tablestanes1.Cell(0, 1).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 1).SetContent(":");
                            tablestanes1.Cell(0, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 2).SetContent(Convert.ToString(dv[0]["roll_admit"]));
                            tablestanes1.Cell(0, 2).SetFont(f4);

                            tablestanes1.Cell(0, 3).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 3).SetContent("House");
                            tablestanes1.Cell(0, 4).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 4).SetContent(":");
                            tablestanes1.Cell(0, 5).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 5).SetContent(dv[0]["studhouse"].ToString());
                            tablestanes1.Cell(0, 5).SetFont(f4);

                            tablestanes1.Cell(0, 6).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 6).SetContent("Blood Group");
                            tablestanes1.Cell(0, 7).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 7).SetContent(":");
                            tablestanes1.Cell(0, 8).SetContentAlignment(ContentAlignment.TopLeft);
                            string bldgrp = dv[0]["bldgrp"].ToString();
                            bldgrp = d2.GetFunction("select distinct textval from applyn  a,textvaltable t where a.bldgrp = t.TextCode and t.TextCode=" + dv[0]["bldgrp"].ToString() + "");
                            if (bldgrp.Trim() == "0" || bldgrp.Trim() == "")
                            {
                                bldgrp = string.Empty;
                            }
                            tablestanes1.Cell(0, 8).SetContent(bldgrp);
                            tablestanes1.Cell(0, 8).SetFont(f4);

                            newpdftabpage2 = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, 148, 520, 250));
                            mypdfpage.Add(newpdftabpage2);

                            tablestanes1 = mydoc.NewTable(f11, 1, 12, 1);
                            tablestanes1.VisibleHeaders = false;
                            tablestanes1.SetBorders(Color.Black, 1, BorderType.None);
                            tablestanes1.SetColumnsWidth(new int[] { 55, 5, 186, 90, 5, 50, 40, 5, 70, 50, 5, 44 });

                            tablestanes1.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 0).SetContent("EMIS No");
                            tablestanes1.Cell(0, 1).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 1).SetContent(":");
                            tablestanes1.Cell(0, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 2).SetContent(emisNo);
                            tablestanes1.Cell(0, 2).SetFont(f4);

                            tablestanes1.Cell(0, 3).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 3).SetContent("AADHAAR No");
                            tablestanes1.Cell(0, 4).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 4).SetContent(":");
                            tablestanes1.Cell(0, 5).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 5).SetContent(aadhaarNo);
                            tablestanes1.Cell(0, 5).SetFont(f4);
                            foreach (PdfCell pc in tablestanes1.CellRange(0, 5, 0, 5).Cells)
                            {
                                pc.ColSpan = 5;
                            }
                            newpdftabpage2 = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, 169, 530, 100));
                            mypdfpage.Add(newpdftabpage2);

                            tablestanes1 = mydoc.NewTable(f11, 2, 6, 1);
                            tablestanes1.VisibleHeaders = false;
                            tablestanes1.SetBorders(Color.Black, 1, BorderType.None);
                            tablestanes1.SetColumnsWidth(new int[] { 240, 13, 300, 150, 13, 300 });
                            tablestanes1.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 0).SetContent("Father's / Guardian Name");
                            tablestanes1.Cell(0, 1).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 1).SetContent(":");
                            tablestanes1.Cell(0, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 2).SetContent(Convert.ToString(dv[0]["parent_name"]));
                            tablestanes1.Cell(0, 2).SetFont(f4);

                            tablestanes1.Cell(0, 3).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 3).SetContent("Mother's Name");
                            tablestanes1.Cell(0, 4).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 4).SetContent(":");
                            tablestanes1.Cell(0, 5).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 5).SetContent(Convert.ToString(dv[0]["mother"]));
                            tablestanes1.Cell(0, 5).SetFont(f4);
                            Gios.Pdf.PdfTablePage newpdftabpage3 = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, 190, mydoc.PageWidth - 50, 250));

                            mypdfpage.Add(newpdftabpage3);

                            tablestanes1 = mydoc.NewTable(f11, 1, 3, 1);

                            tablestanes1.VisibleHeaders = false;
                            tablestanes1.SetBorders(Color.Black, 1, BorderType.None);
                            tablestanes1.SetColumnsWidth(new int[] { 180, 5, 600 });//
                            tablestanes1.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 0).SetContent("Communication Address");
                            tablestanes1.Cell(0, 1).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 1).SetContent(":");
                            tablestanes1.Cell(0, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 2).SetContent(addressline1);
                            tablestanes1.Cell(0, 2).SetFont(f4);

                            Gios.Pdf.PdfTablePage newpdftabpage4 = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, 220, mydoc.PageWidth - 50, 500));

                            mypdfpage.Add(newpdftabpage4);

                            tablestanes1 = mydoc.NewTable(f11, 1, 6, 1);

                            tablestanes1.VisibleHeaders = false;
                            tablestanes1.SetBorders(Color.Black, 1, BorderType.None);
                            tablestanes1.SetColumnsWidth(new int[] { 78, 5, 193, 50, 5, 180 });
                            tablestanes1.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 0).SetContent("Contact Number");
                            tablestanes1.Cell(0, 1).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 1).SetContent(":");
                            tablestanes1.Cell(0, 2).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 2).SetContent(mobileno);
                            tablestanes1.Cell(0, 2).SetFont(f4);

                            tablestanes1.Cell(0, 3).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 3).SetContent("Email - ID");
                            tablestanes1.Cell(0, 4).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 4).SetContent(":");
                            tablestanes1.Cell(0, 5).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 5).SetContent("____________________________");
                            tablestanes1.Cell(0, 5).SetFont(f4);

                            Gios.Pdf.PdfTablePage newpdftabpage5 = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, 250, mydoc.PageWidth - 50, 200));

                            mypdfpage.Add(newpdftabpage5);

                            tablestanes1 = mydoc.NewTable(f11, 2, 9, 1);

                            tablestanes1.VisibleHeaders = false;
                            tablestanes1.SetBorders(Color.Black, 1, BorderType.None);

                            tablestanes1.SetColumnsWidth(new int[] { 28, 5, 72, 28, 5, 72, 50, 5, 150 });

                            tablestanes1.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 0).SetContent("Height");
                            tablestanes1.Cell(0, 1).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 1).SetContent(":");
                            tablestanes1.Cell(0, 2).SetContentAlignment(ContentAlignment.BottomLeft);
                            //tablestanes1.Cell(0, 2).SetContent(dv[0]["StudHeight"].ToString() + " Cms");
                            if (dv[0]["StudHeight"].ToString().Trim() != "" || dv[0]["StudHeight"].ToString().Trim() != null)
                            {
                                tablestanes1.Cell(0, 2).SetContent(dv[0]["StudHeight"].ToString() + " cms");
                            }
                            //tablestanes1.Cell(0, 2).SetContent(" ________");
                            tablestanes1.Cell(0, 2).SetFont(f4);

                            tablestanes1.Cell(0, 3).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 3).SetContent("Weight");
                            tablestanes1.Cell(0, 4).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 4).SetContent(":");
                            tablestanes1.Cell(0, 5).SetContentAlignment(ContentAlignment.BottomLeft);
                            if (dv[0]["StudWeight"].ToString().Trim() != "" || dv[0]["StudWeight"].ToString().Trim() != null)
                            {
                                tablestanes1.Cell(0, 5).SetContent(dv[0]["StudWeight"].ToString() + " kgs");
                            }

                            //tablestanes1.Cell(0, 5).SetContent(" ________");
                            tablestanes1.Cell(0, 5).SetFont(f4);

                            tablestanes1.Cell(0, 6).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 6).SetContent("Health Status");
                            tablestanes1.Cell(0, 7).SetContentAlignment(ContentAlignment.TopLeft);
                            tablestanes1.Cell(0, 7).SetContent(":");
                            tablestanes1.Cell(0, 8).SetContentAlignment(ContentAlignment.TopLeft);
                            //tablestanes1.Cell(0, 2).SetContent(dv[0]["Strenghts"].ToString() + "  ");
                            tablestanes1.Cell(0, 8).SetContent("____________________________");
                            tablestanes1.Cell(0, 8).SetFont(f4);
                            Gios.Pdf.PdfTablePage newpdftabpage6 = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, 270, 580, 180));

                            mypdfpage.Add(newpdftabpage6);


                            pdf_acadamicyear = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 295, 595, 30), System.Drawing.ContentAlignment.TopCenter, "-----------------------------------------------------------------------------------------------------------------------------------");
                            mypdfpage.Add(pdf_acadamicyear);

                            string romannew = string.Empty;

                            if (dropterm.SelectedItem.Text.Trim() == "1")
                            {
                                romannew = "I";

                            }
                            else if (dropterm.SelectedItem.Text.Trim() == "2")
                            {
                                romannew = "II";
                            }
                            else if (dropterm.SelectedItem.Text.Trim() == "3")
                            {
                                romannew = "III";
                            }

                            pdf_acadamicyear = new PdfTextArea(f11, System.Drawing.Color.Black, new PdfArea(mydoc, 0 + 15, 315, 595, 30), System.Drawing.ContentAlignment.TopCenter, "TERM - " + romannew + "");
                            mypdfpage.Add(pdf_acadamicyear);

                            string stdphtsql = "select * from StdPhoto where app_no='" + stdappno + "'";
                            MemoryStream memoryStream = new MemoryStream();
                            DataSet dsstdpho = new DataSet();
                            dsstdpho.Clear();
                            dsstdpho.Dispose();
                            dsstdpho = da.select_method_wo_parameter(stdphtsql, "Text");
                            if (dsstdpho.Tables[0].Rows.Count > 0)
                            {
                                byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                                    {
                                    }
                                    else
                                    {
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    }
                                }
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 10 + 35, 17, 490);
                            }
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collegeCode.ToString() + ".jpeg")))
                            {
                                byte[] file = (byte[])dtCollegeInfo.Rows[0]["logo1"];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collegeCode.ToString() + ".jpeg")))
                                    {
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collegeCode.ToString() + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    }
                                }
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collegeCode.ToString() + ".jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collegeCode.ToString() + ".jpeg"));
                                mypdfpage.Add(LogoImage, 10 + 35, 17, 490);
                            }

                            string str_rolladmit = Convert.ToString(dv[0]["roll_admit"]);
                            term = dropterm.SelectedItem.Text.ToString();
                            string clm = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_Admit='" + str_rolladmit + "' and r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar' and r.Batch_Year='" + Convert.ToString(dropyear.SelectedValue).Trim() + "' and r.degree_code='" + Convert.ToString(ddstandard.SelectedValue).Trim() + "';";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(clm, "text");
                            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                            {
                                degreecode = Convert.ToString(ddstandard.SelectedItem.Value);
                                ds.Tables[1].DefaultView.RowFilter = "Roll_Admit='" + str_rolladmit + "'";
                                dv = ds.Tables[1].DefaultView;
                                count4 = 0;
                                count4 = dv.Count;
                                if (count4 > 0)
                                {
                                    admdate = Convert.ToString(dv[0]["adm_date"]);
                                    Roll_No = Convert.ToString(dv[0]["Roll_No"]);
                                    currentsem = Convert.ToString(dv[0]["Current_Semester"]);
                                    string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + term + "' and degree_code='" + degreecode + "' and batch_year='" + batchyear + "'";
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(sem, "Text");

                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string startdate = ds.Tables[0].Rows[0]["start_date"].ToString();
                                        string enddate = ds.Tables[0].Rows[0]["end_date"].ToString();
                                        persentmonthcal(Roll_No, admdate, startdate, enddate);
                                        lbltot_att1 = Convert.ToString(pre_present_date);
                                        lbltot_work1 = Convert.ToString(per_workingdays);
                                    }
                                }
                            }

                            if (firsttime == true)
                            {
                                bindheader();
                                firsttime = false;
                            }
                            DataSet dssubject = new DataSet();
                            DataSet otherds_subject = new DataSet();
                            string otherssubject_sql = string.Empty;
                            string[] split_criteriano;
                            batchyear = "  and y.Batch_Year='" + Convert.ToString(dropyear.SelectedItem.Text) + "'";
                            degreecode = "  and degree_code='" + Convert.ToString(ddstandard.SelectedItem.Value) + "'";
                            term = " and semester in ('" + dropterm.SelectedItem.Text + "')";
                            otherssubject_sql = "select distinct  subject_no,subject_name from subject s,syllabus_master y,sub_sem ss  where s.syll_code = y.syll_code and ss.syll_code=s.syll_code and s.subType_no=ss.subType_no and y.syll_code=ss.syll_code   and subject_type='others' and promote_count=1 ";
                            otherssubject_sql = otherssubject_sql + batchyear + degreecode + term + " order by subject_no,subject_name;";
                            otherds_subject.Clear();
                            otherds_subject = d2.select_method_wo_parameter(otherssubject_sql, "Text");
                            string otherssubjectcode = string.Empty;
                            string otherssubjectcode01 = string.Empty;
                            int othersubjectcount = otherds_subject.Tables[0].Rows.Count;
                            for (int ii = 0; ii < otherds_subject.Tables[0].Rows.Count; ii++)
                            {
                                if (otherssubjectcode == "")
                                {
                                    otherssubjectcode = otherds_subject.Tables[0].Rows[ii][0].ToString();
                                    otherssubjectcode01 = otherds_subject.Tables[0].Rows[ii][0].ToString();
                                }
                                else
                                {
                                    otherssubjectcode = otherssubjectcode + "','" + otherds_subject.Tables[0].Rows[ii][0].ToString();
                                    otherssubjectcode01 = otherssubjectcode01 + "','" + otherds_subject.Tables[0].Rows[ii][0].ToString();
                                }
                            }

                            if (otherssubjectcode != "")
                            {
                                otherssubjectcode = " and c.subject_no not in('" + otherssubjectcode + "')";
                                otherssubjectcode01 = " and c.subject_no  in('" + otherssubjectcode01 + "')";
                            }
                            else
                            {
                                otherssubjectcode = string.Empty;
                                otherssubjectcode01 = string.Empty;
                            }

                            string subject_sql = "select distinct  subject_no,subject_name from subject s,syllabus_master y,sub_sem ss  where s.syll_code = y.syll_code and ss.syll_code=s.syll_code and s.subType_no=ss.subType_no and y.syll_code=ss.syll_code   and subject_type<>'others' ";
                            subject_sql = subject_sql + batchyear + degreecode + term + "  order by subject_no,subject_name;";

                            subject_sql = subject_sql + ";" + "SELECT distinct c.Istype,conversion as Conversion_value FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no   " + batchyear + "   " + degreecode + "  " + term + " " + otherssubjectcode + "  and CRITERIA_NO is null  and c.Istype<>'settings'";

                            subject_sql = subject_sql + ";" + "SELECT distinct c.Istype,conversion as Conversion_value FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no   " + batchyear + "   " + degreecode + "  " + term + " " + otherssubjectcode01 + "  and CRITERIA_NO is null  and c.Istype<>'settings'";

                            double fatotal = 0;
                            double satotal = 0;
                            double fulltotal = 0;
                            string grademain = string.Empty;

                            DataSet dsgradechk = new DataSet();
                            dsgradechk.Clear();
                            dssubject.Clear();
                            batchyear = Convert.ToString(dropyear.SelectedItem.Text);
                            degreecode = Convert.ToString(ddstandard.SelectedItem.Value);
                            term = Convert.ToString(dropterm.SelectedItem.Text);
                            dssubject = d2.select_method_wo_parameter(subject_sql, "Text");
                            if (dssubject.Tables.Count > 0 && dssubject.Tables[0].Rows.Count > 0)
                            {
                                partone = d2.GetFunction("select  TextVal as part1 from CoCurr_Activitie ca,textvaltable tv where ca.Title_Name=tv.TextCode and TextCriteria = 'RTnam' and ca.Degree_Code='" + degreecode + "' and Batch_Year='" + batchyear + "' and SubTitle='1a'");
                                pdf_acadamicyear = new PdfTextArea(f10, System.Drawing.Color.Black, new PdfArea(mydoc, 20 + moveleftvalue, 310, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Part-1 : " + partone + "");
                                mypdfpage.Add(pdf_acadamicyear);

                                partone = d2.GetFunction("select  TextVal as part1 from CoCurr_Activitie ca,textvaltable tv where ca.Title_Name=tv.TextCode and TextCriteria = 'RTnam' and ca.Degree_Code='" + degreecode + "' and Batch_Year='" + batchyear + "' and SubTitle='2a'");

                                tablestanes1 = mydoc.NewTable(f4, dssubject.Tables[0].Rows.Count + othersubjectcount + 1, 4, 4);
                                tablestanes1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                tablestanes1.VisibleHeaders = false;

                                tablestanes1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablestanes1.Cell(0, 0).SetContent("Subject");
                                tablestanes1.Cell(0, 0).SetFont(f11);
                                tablestanes1.Columns[0].SetWidth(100);

                                tablestanes1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablestanes1.Cell(0, 1).SetContent("FA Grade");
                                tablestanes1.Cell(0, 1).SetFont(f11);
                                tablestanes1.Columns[1].SetWidth(50);
                                tablestanes1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablestanes1.Cell(0, 2).SetContent("SA Grade");
                                tablestanes1.Cell(0, 2).SetFont(f11);
                                tablestanes1.Columns[2].SetWidth(50);
                                tablestanes1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablestanes1.Cell(0, 3).SetContent("Total Grade");
                                tablestanes1.Cell(0, 3).SetFont(f11);
                                tablestanes1.Columns[3].SetWidth(50);
                                int bothmark = 0;
                                double maxconvermark = 0;
                                string faminus = string.Empty;
                                string saminus = string.Empty;
                                for (int g = 0; g < dssubject.Tables[0].Rows.Count; g++)
                                {
                                    tablestanes1.Cell(g + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    tablestanes1.Cell(g + 1, 0).SetContent(dssubject.Tables[0].Rows[g]["subject_name"].ToString());
                                    string subjectnog = Convert.ToString(dssubject.Tables[0].Rows[g]["subject_no"].ToString());
                                    for (int s = 0; s < 3; s++)
                                    {
                                        string crit_fasa = dtallcol.Rows[s]["Criteria nos"].ToString();
                                        if (dtallcol.Rows[s]["Colname"].ToString().Trim() == "FAGrade")
                                        {
                                            string checkcamcal = d2.GetFunction("select value from Master_Settings where settings='Report Throw Cam Calculation'");
                                            if (checkcamcal.Trim() == "0")
                                            {
                                                fatotal = Convert.ToDouble(d2.GetFunction(" select marks_obtained from criteriaforinternal c,syllabus_master y,Exam_type e,Result r,subject u,sub_sem ss where c.syll_code = y.syll_code and c.Criteria_no = e.criteria_no and r.exam_code = e.exam_code and e.subject_no = u.subject_no  and y.Batch_Year = '" + dropyear.SelectedItem.Text.ToString() + "' and  degree_code = '" + Convert.ToString(ddstandard.SelectedItem.Value) + "' and semester = '" + dropterm.SelectedItem.Text + "'    and ss.syll_code=y.syll_code and roll_no='" + Roll_No + "' and u.subject_no='" + subjectnog + "' and criteria='FA' and ss.subType_no=u.subType_no"));

                                                maxconvermark = Convert.ToDouble(d2.GetFunction(" select e.max_mark from criteriaforinternal c,syllabus_master y,Exam_type e,Result r,subject u,sub_sem ss where c.syll_code = y.syll_code and c.Criteria_no = e.criteria_no and r.exam_code = e.exam_code and e.subject_no = u.subject_no  and y.Batch_Year = '" + dropyear.SelectedItem.Text.ToString() + "' and  degree_code = '" + Convert.ToString(ddstandard.SelectedItem.Value) + "' and semester = '" + Convert.ToString(dropterm.SelectedItem.Text) + "' and ss.syll_code=y.syll_code and roll_no='" + Roll_No + "' and u.subject_no='" + subjectnog + "' and criteria='FA' and ss.subType_no=u.subType_no"));
                                            }
                                            else
                                            {

                                                fatotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + Roll_No + "'  and Criteria_no is null and c.Istype='Calculate 1' and s.subject_no='" + subjectnog + "'"));
                                                maxconvermark = maxconvermark + Convert.ToDouble(d2.GetFunction("SELECT c.conversion FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + Roll_No + "'  and Criteria_no is null and c.Istype='Calculate 1' and s.subject_no='" + subjectnog + "'"));
                                            }

                                            if (fatotal < 0)
                                            {
                                                string abs = loadmarkat(Convert.ToString(fatotal));
                                                tablestanes1.Cell(g + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tablestanes1.Cell(g + 1, 1).SetContent(Convert.ToString(abs));
                                                faminus = abs;
                                            }
                                            else
                                            {
                                                faminus = string.Empty;
                                                fulltotal = fatotal;
                                                grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradesystemfa[Convert.ToInt32(term) - 1] + "' and  " + fatotal + " between Frange and Trange";
                                                dsgradechk.Clear();
                                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                                {
                                                    tablestanes1.Cell(g + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    tablestanes1.Cell(g + 1, 1).SetContent(Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString()));
                                                    fagradeonly = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                                                }
                                                else
                                                {
                                                    grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegeCode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradesystemfa[Convert.ToInt32(term) - 1] + "' and  " + fatotal + " between Frange and Trange";
                                                    dsgradechk.Clear();
                                                    dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                                    if (dsgradechk.Tables[0].Rows.Count > 0)
                                                    {
                                                        tablestanes1.Cell(g + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        tablestanes1.Cell(g + 1, 1).SetContent(Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString()));
                                                        fagradeonly = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                                    }
                                                }
                                            }
                                        }


                                        if (dtallcol.Rows[s]["Colname"].ToString().Trim() == "SAGrade")
                                        {
                                            string checkcamcal = d2.GetFunction("select value from Master_Settings where settings='Report Throw Cam Calculation'");
                                            if (checkcamcal.Trim() == "0")
                                            {
                                                satotal = Convert.ToDouble(d2.GetFunction(" select marks_obtained   from criteriaforinternal c,syllabus_master y,Exam_type e,Result r,subject u,sub_sem ss where c.syll_code = y.syll_code and c.Criteria_no = e.criteria_no and r.exam_code = e.exam_code and e.subject_no = u.subject_no  and y.Batch_Year = '" + dropyear.SelectedItem.Text.ToString() + "' and  degree_code = '" + Convert.ToString(ddstandard.SelectedItem.Value) + "' and semester = '" + dropterm.SelectedItem.Text + "'    and ss.syll_code=y.syll_code and roll_no='" + Roll_No + "' and u.subject_no='" + subjectnog + "' and criteria='SA' and ss.subType_no=u.subType_no"));
                                                maxconvermark = maxconvermark + Convert.ToDouble(d2.GetFunction(" select e.max_mark     from criteriaforinternal c,syllabus_master y,Exam_type e,Result r,subject u,sub_sem ss where c.syll_code = y.syll_code and c.Criteria_no = e.criteria_no and r.exam_code = e.exam_code and e.subject_no = u.subject_no  and y.Batch_Year = '" + dropyear.SelectedItem.Text.ToString() + "' and  degree_code = '" + Convert.ToString(ddstandard.SelectedItem.Value) + "' and semester = '" + dropterm.SelectedItem.Text + "'    and ss.syll_code=y.syll_code and roll_no='" + Roll_No + "' and u.subject_no='" + subjectnog + "' and criteria='SA' and ss.subType_no=u.subType_no"));
                                            }
                                            else
                                            {
                                                satotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + Roll_No + "'  and Criteria_no is null and c.Istype='Calculate 2' and s.subject_no='" + subjectnog + "'"));
                                                maxconvermark = maxconvermark + Convert.ToDouble(d2.GetFunction("SELECT c.conversion FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + Roll_No + "'  and Criteria_no is null and c.Istype='Calculate 2' and s.subject_no='" + subjectnog + "'"));
                                            }

                                            if (satotal < 0)
                                            {
                                                string abs = loadmarkat(Convert.ToString(satotal));
                                                tablestanes1.Cell(g + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tablestanes1.Cell(g + 1, 2).SetContent(abs);
                                                saminus = abs;
                                            }
                                            else
                                            {
                                                saminus = string.Empty;
                                                fulltotal = fulltotal + satotal;
                                                grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradesystemsa[Convert.ToInt32(term) - 1] + "' and  " + satotal + " between Frange and Trange";
                                                dsgradechk.Clear();
                                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                                {
                                                    tablestanes1.Cell(g + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    tablestanes1.Cell(g + 1, 2).SetContent(Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString()));
                                                    sagradeonly = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                                                }
                                                else
                                                {
                                                    grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegeCode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradesystemsa[Convert.ToInt32(term) - 1] + "' and  " + satotal + " between Frange and Trange";
                                                    dsgradechk.Clear();
                                                    dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                                    if (dsgradechk.Tables[0].Rows.Count > 0)
                                                    {
                                                        tablestanes1.Cell(g + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        tablestanes1.Cell(g + 1, 2).SetContent(Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString()));
                                                        sagradeonly = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                                    }
                                                }
                                            }

                                        }

                                        if (dtallcol.Rows[s]["Colname"].ToString().Trim() == "TotalGrade")
                                        {
                                            string fasaminusgrade = string.Empty;
                                            if (maxconvermark > 0)
                                            {
                                                fulltotal = (fulltotal / maxconvermark);
                                                fulltotal = fulltotal * 100;
                                            }
                                            else
                                            {
                                                fulltotal = 0;
                                            }
                                            grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fulltotal + " between Frange and Trange";
                                            dsgradechk.Clear();
                                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                            if (dsgradechk.Tables[0].Rows.Count > 0)
                                            {
                                                //FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                                                tablestanes1.Cell(g + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                // tablestanes1.Cell(g + 1, 3).SetContent(Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString()));
                                                fasaminusgrade = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                            }
                                            else
                                            {
                                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegeCode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fulltotal + " between Frange and Trange";
                                                dsgradechk.Clear();
                                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                                {
                                                    //FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                                                    tablestanes1.Cell(g + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    //tablestanes1.Cell(g + 1, 3).SetContent(Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString()));
                                                    fasaminusgrade = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                                }
                                            }

                                            if (faminus != "" && saminus == "")
                                            {
                                                fasaminusgrade = faminus + "/" + sagradeonly;
                                                tablestanes1.Cell(g + 1, 3).SetContent(fasaminusgrade);
                                            }
                                            if (faminus == "" && saminus != "")
                                            {
                                                fasaminusgrade = fagradeonly + "/" + saminus;
                                                tablestanes1.Cell(g + 1, 3).SetContent(fasaminusgrade);
                                            }
                                            if (faminus == "" && saminus == "")
                                            {

                                                tablestanes1.Cell(g + 1, 3).SetContent(fasaminusgrade);
                                            }
                                            if (faminus != "" && saminus != "")
                                            {
                                                fasaminusgrade = faminus + "/" + saminus;
                                                tablestanes1.Cell(g + 1, 3).SetContent(fasaminusgrade);
                                            }
                                            fatotal = 0;
                                            satotal = 0;
                                            fulltotal = 0;
                                            faminus = string.Empty;
                                            saminus = string.Empty;
                                            bothmark = 0;
                                            maxconvermark = 0;
                                            sagradeonly = string.Empty;
                                            fagradeonly = string.Empty;
                                        }
                                    }
                                }
                                int othersfasastartrow = dssubject.Tables[0].Rows.Count + 1;

                                if (otherds_subject.Tables[0].Rows.Count > 0)
                                {
                                    string checkcamcal = d2.GetFunction("select value from Master_Settings where settings='Report Throw Cam Calculation'");
                                    if (checkcamcal.Trim() == "0")
                                    {

                                        for (int i = 0; i < otherds_subject.Tables[0].Rows.Count; i++)
                                        {
                                            tablestanes1.Cell(othersfasastartrow, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            tablestanes1.Cell(othersfasastartrow, 0).SetContent(Convert.ToString(otherds_subject.Tables[0].Rows[i]["subject_name"].ToString()));

                                            string str_subject_no = otherds_subject.Tables[0].Rows[i]["subject_no"].ToString().Trim();

                                            fatotal = Convert.ToDouble(d2.GetFunction(" select ISNULL(marks_obtained,0) from criteriaforinternal c,syllabus_master y,Exam_type e,Result r,subject u,sub_sem ss where c.syll_code = y.syll_code and c.Criteria_no = e.criteria_no and r.exam_code = e.exam_code and e.subject_no = u.subject_no  and y.Batch_Year = '" + dropyear.SelectedItem.Text.ToString() + "' and  degree_code = '" + Convert.ToString(ddstandard.SelectedItem.Value) + "' and semester = '" + dropterm.SelectedItem.Text + "'    and ss.syll_code=y.syll_code and roll_no='" + Roll_No + "' and u.subject_no='" + str_subject_no + "' and criteria like 'other%' and ss.subType_no=u.subType_no"));


                                            satotal = 0;
                                            //fulltotal = fatotal + satotal;

                                            double maxim = Convert.ToDouble(d2.GetFunction(" select ISNULL(e.max_mark,0) from criteriaforinternal c,syllabus_master y,Exam_type e,Result r,subject u,sub_sem ss where c.syll_code = y.syll_code and c.Criteria_no = e.criteria_no and r.exam_code = e.exam_code and e.subject_no = u.subject_no  and y.Batch_Year = '" + dropyear.SelectedItem.Text.ToString() + "' and  degree_code = '" + Convert.ToString(ddstandard.SelectedItem.Value) + "' and semester = '" + dropterm.SelectedItem.Text + "'    and ss.syll_code=y.syll_code and roll_no='" + Roll_No + "' and u.subject_no='" + str_subject_no + "' and criteria like 'other%' and ss.subType_no=u.subType_no"));
                                            //maxim = maxim + Convert.ToDouble(d2.GetFunction(" select e.max_mark   from criteriaforinternal c,syllabus_master y,Exam_type e,Result r,subject u,sub_sem ss where c.syll_code = y.syll_code and c.Criteria_no = e.criteria_no and r.exam_code = e.exam_code and e.subject_no = u.subject_no  and y.Batch_Year = '" + dropyear.SelectedItem.Text.ToString() + "' and  degree_code = '" + ddstandard.SelectedItem. Convert.ToString(value) + "' and semester = '" + dropterm.SelectedItem.Text + "'    and ss.syll_code=y.syll_code and roll_no='" + Roll_No + "' and u.subject_no='" + str_subject_no + "' and criteria='SA' and ss.subType_no=u.subType_no"));

                                            if (maxim > 0)
                                            {
                                                fatotal = fatotal / maxim;
                                                fatotal = fatotal * 100;
                                            }
                                            else
                                            {
                                                fatotal = 0;
                                            }
                                            if (fatotal < 0)
                                            {
                                                string abs = loadmarkat(Convert.ToString(fatotal));
                                                tablestanes1.Cell(othersfasastartrow, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tablestanes1.Cell(othersfasastartrow, 1).SetContent(Convert.ToString(abs));

                                            }
                                            else
                                            {
                                                grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fatotal + " between Frange and Trange";
                                                dsgradechk.Clear();
                                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                                {
                                                    //FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                                    tablestanes1.Cell(othersfasastartrow, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    tablestanes1.Cell(othersfasastartrow, 1).SetContent(Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString()));
                                                }
                                                else
                                                {
                                                    grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegeCode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fatotal + " between Frange and Trange";
                                                    dsgradechk.Clear();
                                                    dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                                    if (dsgradechk.Tables[0].Rows.Count > 0)
                                                    {
                                                        //FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                                        tablestanes1.Cell(othersfasastartrow, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        tablestanes1.Cell(othersfasastartrow, 1).SetContent(Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString()));
                                                    }
                                                }
                                            }
                                            foreach (PdfCell pr in tablestanes1.CellRange(othersfasastartrow, 1, othersfasastartrow, 1).Cells)
                                            {
                                                pr.ColSpan = 3;
                                            }
                                            othersfasastartrow++;
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; i < otherds_subject.Tables[0].Rows.Count; i++)
                                        {
                                            tablestanes1.Cell(othersfasastartrow, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            tablestanes1.Cell(othersfasastartrow, 0).SetContent(Convert.ToString(otherds_subject.Tables[0].Rows[i]["subject_name"].ToString()));
                                            //str_colno = otherds_subject.Tables[0].Rows[i]["colno"].ToString().Trim();
                                            //str_criteriano = dtallcol.Rows[i]["Criteria nos"].ToString().Trim();
                                            string str_subject_no = otherds_subject.Tables[0].Rows[i]["subject_no"].ToString().Trim();

                                            fatotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + Roll_No + "'  and Criteria_no is null and c.Istype='Calculate 1' and s.subject_no='" + str_subject_no + "'"));
                                            grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fatotal + " between Frange and Trange";
                                            dsgradechk.Clear();
                                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                            if (dsgradechk.Tables[0].Rows.Count > 0)
                                            {
                                                //FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno) + 1].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                                tablestanes1.Cell(othersfasastartrow, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tablestanes1.Cell(othersfasastartrow, 1).SetContent(Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString()));
                                            }
                                            else
                                            {
                                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegeCode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fatotal + " between Frange and Trange";
                                                dsgradechk.Clear();
                                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                                {
                                                    // FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno) + 1].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                                    tablestanes1.Cell(othersfasastartrow, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    tablestanes1.Cell(othersfasastartrow, 1).SetContent(Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString()));
                                                }
                                            }
                                            foreach (PdfCell pr in tablestanes1.CellRange(othersfasastartrow, 1, othersfasastartrow, 1).Cells)
                                            {
                                                pr.ColSpan = 3;
                                            }
                                            othersfasastartrow++;
                                        }

                                    }
                                }

                                Gios.Pdf.PdfTablePage newpdftabpagefasa = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, 355, 250, 400));

                                mypdfpage.Add(newpdftabpagefasa);
                                double heightoftab = 0;
                                heightoftab = 0;
                                heightoftab = newpdftabpagefasa.Area.Height + 320;
                                //pdf_acadamicyear = new PdfTextArea(f11, System.Drawing.Color.Black, new PdfArea(mydoc, 20, checkattalign, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Attendance :   " + lbltot_att1 + " / " + lbltot_work1 + "  Days");
                                grademain = "SELECT * from CoCurrActivitie_Det where   Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "' and istype='Att' and term='" + dropterm.SelectedItem.Text + "' and Roll_No='" + rollnos + "'";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    double presentDays = 0;
                                    double totalWorkingDays = 0;
                                    double attendancePercentage = 0;
                                    double.TryParse(Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark"]).Trim(), out presentDays);
                                    double.TryParse(Convert.ToString(dsgradechk.Tables[0].Rows[0]["totatt_remarks"]).Trim(), out totalWorkingDays);
                                    if (totalWorkingDays > 0 && presentDays > 0)
                                        attendancePercentage = Math.Round((presentDays / totalWorkingDays) * 100, 1, MidpointRounding.AwayFromZero);
                                    pdf_acadamicyear = new PdfTextArea(f11, System.Drawing.Color.Black, new PdfArea(mydoc, 20 + moveleftvalue, checkattalign, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Attendance :   " + dsgradechk.Tables[0].Rows[0]["Mark"].ToString() + " / " + dsgradechk.Tables[0].Rows[0]["totatt_remarks"].ToString() + "  Days");
                                    mypdfpage.Add(pdf_acadamicyear);

                                    pdf_acadamicyear = new PdfTextArea(f11, System.Drawing.Color.Black, new PdfArea(mydoc, 450 + moveleftvalue, checkattalign, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Percentage : " + ((attendancePercentage == 0) ? "" : attendancePercentage.ToString()) + "%");
                                    mypdfpage.Add(pdf_acadamicyear);
                                }
                                else
                                {
                                    pdf_acadamicyear = new PdfTextArea(f11, System.Drawing.Color.Black, new PdfArea(mydoc, 20 + moveleftvalue, checkattalign, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Attendance :   __________ / __________  Days");
                                    mypdfpage.Add(pdf_acadamicyear);

                                    pdf_acadamicyear = new PdfTextArea(f11, System.Drawing.Color.Black, new PdfArea(mydoc, 450 + moveleftvalue, checkattalign, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Percentage :     %");
                                    mypdfpage.Add(pdf_acadamicyear);
                                }

                                grademain = "SELECT * from CoCurrActivitie_Det where   Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "' and istype='remks' and term='" + dropterm.SelectedItem.Text + "' and Roll_No='" + rollnos + "'";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    checkattalign = checkattalign + 50;
                                    pdf_acadamicyear = new PdfTextArea(f11, System.Drawing.Color.Black, new PdfArea(mydoc, 20 + moveleftvalue, checkattalign, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Remarks :   ");
                                    mypdfpage.Add(pdf_acadamicyear);
                                    // checkattalign = checkattalign ;
                                    pdf_acadamicyear = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80 + moveleftvalue, checkattalign + 17, 450, 300), System.Drawing.ContentAlignment.TopLeft, "" + dsgradechk.Tables[0].Rows[0]["totatt_remarks"].ToString() + "");
                                    mypdfpage.Add(pdf_acadamicyear);

                                    //checkattalign = checkattalign + 5;
                                    //pdf_acadamicyear = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80 + moveleftvalue, checkattalign, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "_____________________________________________________________________________");
                                    //mypdfpage.Add(pdf_acadamicyear);
                                }
                                else
                                {
                                    checkattalign = checkattalign + 50;
                                    pdf_acadamicyear = new PdfTextArea(f11, System.Drawing.Color.Black, new PdfArea(mydoc, 20 + moveleftvalue, checkattalign, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Remarks :   ");
                                    mypdfpage.Add(pdf_acadamicyear);
                                    checkattalign = checkattalign + 5;
                                    pdf_acadamicyear = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80 + moveleftvalue, checkattalign, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "_____________________________________________________________________________");
                                    mypdfpage.Add(pdf_acadamicyear);
                                    pdf_acadamicyear = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80 + moveleftvalue, checkattalign + 30, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "_____________________________________________________________________________");
                                    mypdfpage.Add(pdf_acadamicyear);
                                }
                                checkattalign = 0;
                            }


                            //if (otherds_subject.Tables[0].Rows.Count > 0)
                            //{

                            //    for (int i = 0; i < otherds_subject.Tables[0].Rows.Count; i++)
                            //    {


                            //            //str_colno = otherds_subject.Tables[0].Rows[i]["colno"].ToString().Trim();
                            //            //str_criteriano = dtallcol.Rows[i]["Criteria nos"].ToString().Trim();
                            //           string str_subject_no = otherds_subject.Tables[0].Rows[i]["subjetno"].ToString().Trim();

                            //            fatotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no is null and c.Istype='Calculate 1' and s.subject_no='" + str_subject_no + "'"));




                            //            grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fatotal + " between Frange and Trange";
                            //            dsgradechk.Clear();
                            //            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            //            if (dsgradechk.Tables[0].Rows.Count > 0)
                            //            {
                            //                FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno) + 1].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                            //            }
                            //            else
                            //            {
                            //                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + collegeCode + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fatotal + " between Frange and Trange";
                            //                dsgradechk.Clear();
                            //                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            //                if (dsgradechk.Tables[0].Rows.Count > 0)
                            //                {
                            //                    FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno) + 1].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                            //                }
                            //            }


                            //    }
                            //}

                            tablestanes1 = mydoc.NewTable(f11, 1, 3, 3);

                            tablestanes1.VisibleHeaders = false;
                            tablestanes1.SetBorders(Color.Black, 1, BorderType.None);
                            checkattalign = checkattalign + 70;
                            tablestanes1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablestanes1.Cell(0, 0).SetContent("Signature of Teacher");
                            tablestanes1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablestanes1.Cell(0, 1).SetContent("Signature of Headmaster");
                            tablestanes1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablestanes1.Cell(0, 2).SetContent("Signature of Parent");



                            Gios.Pdf.PdfTablePage newpdftabpagef = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19, 790, 580, 250));

                            mypdfpage.Add(newpdftabpagef);

                            if (dtallcol.Rows.Count > 0)
                            {
                            }
                            ArrayList arrcourrid = new ArrayList();
                            ArrayList partcolumnnames = new ArrayList();

                            sql = " select distinct ca.CoCurr_ID,ca.Title_Name,tv.TextCode, tv.TextVal,ca.SubTitle from activity_entry ae,CoCurr_Activitie ca,textvaltable tv where ae.CoCurr_ID=ca.CoCurr_ID and ae.Batch_Year=ca.Batch_Year and ae.Degree_Code=ca.Degree_Code and ae.Batch_Year='" + batchyear + "' and ae.Degree_Code='" + degreecode + "' and tv.TextCode=ae.ActivityTextVal  and ae.ActivityTextVal in (select ActivityTextVal from CoCurrActivitie_Det where Roll_No='" + Roll_No + "' and Degree_Code='" + degreecode + "' and Batch_Year='" + batchyear + "' and CoCurrActivitie_Det.term ='" + term + "')  and SubTitle='2a' and ae.term='" + term + "' order by TextCode";
                            DataSet partsds = new DataSet();
                            partsds.Clear();
                            partsds = d2.select_method_wo_parameter(sql, "Text");
                            if (partsds.Tables[0].Rows.Count > 0)
                            {
                                pdf_acadamicyear = new PdfTextArea(f10, System.Drawing.Color.Black, new PdfArea(mydoc, 300 + moveleftvalue, 310, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Part-2 : " + partone + "");
                                mypdfpage.Add(pdf_acadamicyear);

                                tablestanes1 = mydoc.NewTable(f4, partsds.Tables[0].Rows.Count + 1, 2, 4);

                                tablestanes1.VisibleHeaders = false;
                                tablestanes1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                tablestanes1.SetColumnsWidth(new int[] { 230, 60 });
                                tablestanes1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablestanes1.Cell(0, 0).SetContent("Subject");
                                tablestanes1.Cell(0, 0).SetFont(f11);
                                tablestanes1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablestanes1.Cell(0, 1).SetContent("Grade");
                                tablestanes1.Cell(0, 1).SetFont(f11);

                                string allactivity = string.Empty;
                                string partnametitlle = string.Empty;
                                for (int i = 0; i < partsds.Tables[0].Rows.Count; i++)
                                {

                                    if (allactivity == "")
                                    {
                                        allactivity = partsds.Tables[0].Rows[i]["TextCode"].ToString();
                                    }
                                    else
                                    {
                                        allactivity = allactivity + "','" + partsds.Tables[0].Rows[i]["TextCode"].ToString();
                                    }
                                    partnametitlle = partsds.Tables[0].Rows[i]["Title_Name"].ToString();
                                }

                                string strqueryytable = "select distinct tv.TextCode,tv.TextVal,ag.Grade,ag.description,cd.Mark from activity_gd ag ,CoCurrActivitie_Det cd,textvaltable tv where ag.ActivityTextVal=cd.ActivityTextVal and tv.TextCode=ag.ActivityTextVal and tv.TextCode=cd.ActivityTextVal and  cd.ActivityTextVal in ('" + allactivity + "') and cd.Roll_No='" + Roll_No + "' and cd.Mark between frompoint and topoint  and cd.Degree_Code=ag.Degree_Code and cd.Batch_Year=ag.batch_year  and cd.term=ag.term  and cd.term='" + term + "' order by tv.TextCode ";

                                DataTable strqueryytablefil = da.select_method_wop_table(strqueryytable, "Text");
                                DataView dvsk = strqueryytablefil.DefaultView;
                                Gios.Pdf.PdfTable table;
                                if (dvsk.Count > 0)
                                {
                                    DataTable pdfbind = new DataTable();
                                    int sno = 1;
                                    for (int snda = 0; snda < dvsk.Count; snda++)
                                    {
                                        tablestanes1.Cell(snda + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tablestanes1.Cell(snda + 1, 0).SetContent(dvsk[snda]["TextVal"].ToString() + " :");
                                        tablestanes1.Cell(snda + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tablestanes1.Cell(snda + 1, 1).SetContent(dvsk[snda]["Grade"].ToString());
                                    }
                                }
                                Gios.Pdf.PdfTablePage newpdftabpage6s = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 300 + moveleftvalue, 355, 250, 400));
                                mypdfpage.Add(newpdftabpage6s);
                            }
                            mypdfpage.SaveToDocument();
                            mypdfpage = mydoc.NewPage();
                        }
                    }
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "rankcard" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";

                mydoc.SaveToFile(szPath + szFile);
                mydoc.SaveToFile(szPath + szFile);

                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
            FpSpread1.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void bindheader()
    {
        dtallcol.Clear();
        dtallcol.Columns.Clear();
        dtallcol.Columns.Add("Colname");
        dtallcol.Columns.Add("colno");
        dtallcol.Columns.Add("Criteria nos");
        dtallcol.Rows.Add("FAGrade", "", "");
        dtallcol.Rows.Add("SAGrade", "", "");
        dtallcol.Rows.Add("TotalGrade", "", " ");

    }

    protected void btnmatric_page1_Click(object sender, EventArgs e)
    {
        try
        {
            rollnos = string.Empty;
            int checkedcount = 0;
            FpSpread1.SaveChanges();

            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 1].Value) == 1)
                {
                    checkedcount++;
                }
            }

            //if (checkedcount > 1)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please Select Only One Student')", true);
            //    //lblerrormsg.Text = "Please Select Only One Student";
            //    //lblerrormsg.Visible = false;
            //    // lblerrormsg.Visible = true;
            //    return;

            //}
            //else
            //{

            //    //lblerrormsg.Visible = false;
            //}

            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 1].Value) == 1)
                {
                    if (rollnos == "")
                    {
                        rollnos = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                    }
                    else
                    {
                        rollnos = rollnos + "','" + Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                    }
                }
            }

            if (rollnos != "")
            {
                bindbutn_matricp1(rollnos);
                lblerrormsg.Visible = false;
            }
            else
            {
                lblerrormsg.Text = "Please Select Any One Record";
                lblerrormsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    protected void btnmatric_page2_Click(object sender, EventArgs e)
    {
        try
        {
            rollnos = string.Empty;
            int checkedcount = 0;
            FpSpread1.SaveChanges();

            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 1].Value) == 1)
                {
                    checkedcount++;
                }
            }

            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 1].Value) == 1)
                {
                    if (rollnos == "")
                    {
                        rollnos = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                    }
                    else
                    {
                        rollnos = rollnos + "','" + Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                    }
                }
            }

            if (rollnos != "")
            {
                bindbutn_matricp2(rollnos);
                lblerrormsg.Visible = false;
            }
            else
            {
                lblerrormsg.Text = "Please Select Any One Record";
                lblerrormsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void bindbutn_matricp1(string rollno)
    {
        try
        {
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            //Gios.Pdf.PdfDocument mydocback = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            Gios.Pdf.PdfPage mypdfpage;
            //Gios.Pdf.PdfPage mypdfpage1;
            //Gios.Pdf.PdfPage mypdfpage1back;
            ArrayList testcriterianos = new ArrayList();
            rollnos = rollno;
            if (rollnos != "")
            {
                sql = "select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No in ('" + rollnos + "') ;";
                studgradeds.Clear();
                studgradeds = d2.select_method_wo_parameter(sql, "text");
                if (studgradeds.Tables[0].Rows.Count > 0)
                {
                    for (int roll = 0; roll < studgradeds.Tables[0].Rows.Count; roll++)
                    {
                        string rcrollno = string.Empty;
                        rcrollno = Convert.ToString(studgradeds.Tables[0].Rows[roll][1]);

                        // bindstudentdetails(rcrollno);
                        // bindstudentdetails_new(rcrollno);

                        DataSet ds = new DataSet();
                        DataSet dschool = new DataSet();
                        DAccess2 da = new DAccess2();
                        DataSet dset = new DataSet();
                        string college_code = Convert.ToString(collegecode);
                        string stdappno = string.Empty;
                        System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
                        System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                        System.Drawing.Font Fontbold1 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
                        System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
                        System.Drawing.Font Fontlarge = new System.Drawing.Font("Book Antiqua", 26, FontStyle.Regular);
                        System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
                        System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 7, FontStyle.Regular);
                        System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
                        System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
                        System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);

                        Gios.Pdf.PdfTable table1;
                        System.Drawing.Font Fontbold9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold);

                        string Roll_No = rcrollno;
                        sql = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No='" + Roll_No + "'";
                        ds.Clear();
                        ds.Dispose();
                        ds = da.select_method_wo_parameter(sql, "Text");
                        DataView dv = new DataView();
                        ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                        dv = ds.Tables[1].DefaultView;
                        int count4 = 0;
                        count4 = dv.Count;

                        if (ds.Tables[0].Rows.Count > 0 && count4 > 0)
                        {
                            string currentsem = Convert.ToString(dv[0]["Current_Semester"]);
                            string batchyear = Convert.ToString(dv[0]["Batch_Year"]);
                            string degreecode = Convert.ToString(dv[0]["degree_code"]);
                            stdappno = Convert.ToString(dv[0]["App_No"]);
                            string allsem = "1";
                            string admdate = Convert.ToString(dv[0]["adm_date"]);
                            string section = string.Empty;
                            string stdcc = string.Empty;
                            stdcc = Convert.ToString(ds.Tables[1].Rows[0]["Dept_Name"]);
                            section = Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
                            string lblclassq1 = "CLASS X";
                            if (Convert.ToInt32(currentsem) > 1)
                            {
                                int term = Convert.ToInt32(currentsem);
                                for (int i = 2; i <= term; i++)
                                {
                                    allsem = allsem + "'" + "," + "'" + i;
                                }
                            }

                            if (Convert.ToInt32(currentsem) >= 1)
                            {
                                int term = Convert.ToInt32(currentsem);
                                for (int i = 1; i <= term; i++)
                                {
                                    string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + i + "' and degree_code='" + degreecode + "' and batch_year='" + batchyear + "'";
                                    dset = da.select_method_wo_parameter(sem, "Text");

                                    if (dset.Tables[0].Rows.Count > 0)
                                    {
                                        string startdate = Convert.ToString(dset.Tables[0].Rows[0]["start_date"]);
                                        string enddate = Convert.ToString(dset.Tables[0].Rows[0]["end_date"]);
                                        persentmonthcal(Roll_No, admdate, startdate, enddate);
                                    }

                                    if (i == 1)
                                    {
                                        lbltot_att1 = Convert.ToString(pre_present_date);
                                        lbltot_work1 = Convert.ToString(per_workingdays);
                                        working1 = Convert.ToString(pre_present_date);
                                        present1 = Convert.ToString(per_workingdays);
                                    }
                                }
                            }

                            string table1sql = "select * from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'  and semester = '" + currentsem + "' ; SELECT COUNT(*) as facount FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "'); SELECT Istype,CRITERIA_NO,y.semester FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "') ; SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + allsem + "')";
                            DataSet ds1fortable1 = new DataSet();
                            ds1fortable1.Clear();
                            ds1fortable1.Dispose();
                            ds1fortable1 = da.select_method_wo_parameter(table1sql, "Text");
                            DataView dvforpage2 = new DataView();

                            string dob = Convert.ToString(dv[0]["dob"]);
                            string[] dobspit = dob.Split('/');
                            string addressline1 = Convert.ToString(dv[0]["parent_addressP"]);
                            addressline1 = addressline1 + " " + Convert.ToString(dv[0]["Streetp"]);
                            string addressline2 = Convert.ToString(dv[0]["Cityp"]);
                            addressline2 = addressline2 + " - " + Convert.ToString(dv[0]["parent_pincodep"]);
                            string mobileno = Convert.ToString(dv[0]["student_mobile"]);
                            mypdfpage = mydoc.NewPage();
                            //mypdfpage1back = mydocback.NewPage();

                            PdfTextArea pdf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 420, 20, 420, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(ds.Tables[0].Rows[0][0]) + "");
                            PdfTextArea pdf11 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 420, 40, 420, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(ds.Tables[0].Rows[0][1]) + "");
                            string address = Convert.ToString(ds.Tables[0].Rows[0][2]) + "" + Convert.ToString(ds.Tables[0].Rows[0][3]) + "" + Convert.ToString(ds.Tables[0].Rows[0][4]);
                            PdfTextArea pdf12 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 420, 55, 420, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
                            PdfTextArea pdf172 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 420, 70, 420, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  :" + Convert.ToString(ds.Tables[0].Rows[0][6]));

                            PdfArea pa2 = new PdfArea(mydoc, 510, 165, 224, 40);
                            PdfRectangle pr2 = new PdfRectangle(mydoc, pa2, Color.Black);
                            mypdfpage.Add(pr2);

                            string sqlschool = "select value from Master_Settings where settings='Academic year'";
                            dschool = da.select_method_wo_parameter(sqlschool, "Text");
                            string splitvalue = Convert.ToString(dschool.Tables[0].Rows[0]["value"]);
                            string[] dsplit = splitvalue.Split(',');

                            string fvalue = Convert.ToString(dsplit[0]);
                            string lvalue = Convert.ToString(dsplit[1]);
                            string acdmic_date = fvalue + " - " + lvalue;

                            PdfTextArea pdf14;
                            PdfTextArea pdf13 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 520, 177, 204, 30), System.Drawing.ContentAlignment.TopLeft, "     PROGRESS REPORT");
                            if (Convert.ToString(dv[0]["Sections"]) != "")
                            {
                                pdf14 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 480, 220, 595, 50), System.Drawing.ContentAlignment.TopLeft, "                   " + lblclassq1 + " - " + Convert.ToString(dv[0]["Sections"]));
                            }
                            else
                            {
                                pdf14 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 480, 220, 595, 50), System.Drawing.ContentAlignment.TopLeft, "                       " + lblclassq1);
                            }
                            PdfTextArea pdf15 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 480, 240, 595, 50), System.Drawing.ContentAlignment.TopLeft, "                      " + acdmic_date);
                            PdfTextArea pdf116 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 345, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Exam No. ");
                            PdfTextArea pdf118 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 490, 345, 595, 50), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                            PdfTextArea pdf118a1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 495, 345, 595, 50), System.Drawing.ContentAlignment.TopLeft, "  ");

                            PdfTextArea pdf18a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 280, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Name of the Pupil");
                            PdfTextArea pdf1822 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 542, 280, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["stud_name"]) + "");
                            PdfTextArea pdfee = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 528, 280, 595, 50), System.Drawing.ContentAlignment.TopLeft, " __________________________________________");

                            PdfTextArea pdf111 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Class");
                            PdfTextArea pdf113 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 470, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, "____________________");
                            PdfTextArea pdf113a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 480, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, " X ");
                            PdfTextArea pdf114 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 615, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Section");
                            PdfTextArea pdf115 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 680, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, " ");
                            PdfTextArea pdf000 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 666, 314, 595, 50), System.Drawing.ContentAlignment.TopLeft, " _____________________");

                            PdfTextArea pdf119 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 380, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Birth");
                            PdfTextArea pdf121 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 513, 380, 595, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(dv[0]["dob"]));
                            PdfTextArea pdf1221 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 506, 380, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_____________" + " " + " Computer No." + " " + "____________________");
                            PdfTextArea pdf121x = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 690, 380, 595, 50), System.Drawing.ContentAlignment.TopLeft, " ");

                            PdfTextArea pdf125 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 410, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Name of the");
                            PdfTextArea pdf126 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 425, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Parent or Guardian");
                            PdfTextArea pdf126m = new PdfTextArea(Fontlarge, System.Drawing.Color.Black, new PdfArea(mydoc, 536, 410, 595, 300), System.Drawing.ContentAlignment.TopLeft, "} ");
                            PdfTextArea pdf127 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 550, 413, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________");
                            PdfTextArea pdf127a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 560, 413, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dv[0]["parent_name"]) + "");

                            PdfTextArea pdf128 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 446, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Address");
                            PdfTextArea pdf130 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 483, 446, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________________________________");
                            PdfTextArea pdf130a = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 487, 446, 400, 50), System.Drawing.ContentAlignment.TopLeft, "" + addressline1 + "");

                            //PdfTextArea pdf131 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 507, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Phone " + " ___________________________________________________" + mobileno);
                            PdfTextArea pdf131 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 507, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Father's Phone " + " ____________________________________________");
                            PdfTextArea pdf131a = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 530, 507, 595, 50), System.Drawing.ContentAlignment.TopLeft, mobileno);
                            PdfTextArea pdf132 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 477, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________________________________________");
                            PdfTextArea pdf133a = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 434, 477, 595, 50), System.Drawing.ContentAlignment.TopLeft, "" + addressline2 + "");

                            PdfTextArea pdf138 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 536, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Specimen Signature of");
                            PdfTextArea pdf138z = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 430, 551, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Parent or Guardian");
                            PdfTextArea pdf138zz = new PdfTextArea(Fontlarge, System.Drawing.Color.Black, new PdfArea(mydoc, 547, 535, 595, 50), System.Drawing.ContentAlignment.TopLeft, " }");
                            PdfTextArea pdf139 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 578, 540, 595, 50), System.Drawing.ContentAlignment.TopLeft, " ");

                            mypdfpage.Add(pdf116);
                            mypdfpage.Add(pdf118);
                            mypdfpage.Add(pdf118a1);
                            mypdfpage.Add(pdf1822);
                            mypdfpage.Add(pdfee);
                            mypdfpage.Add(pdf000);
                            mypdfpage.Add(pdf18a);
                            mypdfpage.Add(pdf111);
                            mypdfpage.Add(pdf113);
                            mypdfpage.Add(pdf113a);
                            mypdfpage.Add(pdf114);
                            mypdfpage.Add(pdf115);
                            mypdfpage.Add(pdf172);
                            mypdfpage.Add(pdf1221);
                            mypdfpage.Add(pdf119);
                            mypdfpage.Add(pdf121);
                            mypdfpage.Add(pdf121x);
                            mypdfpage.Add(pdf125);
                            mypdfpage.Add(pdf126);
                            mypdfpage.Add(pdf126m);
                            mypdfpage.Add(pdf127);
                            mypdfpage.Add(pdf127a);
                            mypdfpage.Add(pdf128);
                            mypdfpage.Add(pdf130);
                            mypdfpage.Add(pdf130a);
                            mypdfpage.Add(pdf131);
                            mypdfpage.Add(pdf131a);
                            mypdfpage.Add(pdf132);
                            mypdfpage.Add(pdf133a);
                            mypdfpage.Add(pdf138);
                            mypdfpage.Add(pdf138z);
                            mypdfpage.Add(pdf138zz);
                            mypdfpage.Add(pdf139);

                            PdfTextArea pdf14ws2 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 566, 540, 595, 50), System.Drawing.ContentAlignment.TopLeft, "_____________________________________");
                            mypdfpage.Add(pdf14ws2);

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            {
                                Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                mypdfpage.Add(LogoImage1, 590, 96, 450);
                            }
                            //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg")))
                            //{
                            //    Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"));
                            //    mypdfpage.Add(LogoImage2, 750, 96, 450);
                            //}
                            //else
                            //{
                            //    Gios.Pdf.PdfImage LogoImage2 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                            //    mypdfpage.Add(LogoImage2, 750, 96, 450);
                            //}

                            Hashtable hatsubject = new Hashtable();
                            Hashtable hatcriter = new Hashtable();
                            DataTable dpdfhealth = new DataTable();
                            DataSet dhealth = new DataSet();

                            PdfArea pahealth = new PdfArea(mydoc, 30, 50, 350, 100);

                            PdfTextArea pdf46 = new PdfTextArea(Fontbolda, System.Drawing.Color.Black, new PdfArea(mydoc, 160, 220, 595, 500), System.Drawing.ContentAlignment.TopLeft, "ASSESSMENT");
                            mypdfpage.Add(pdf46);

                            PdfTextArea pdf46z = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 60, 265, 595, 500), System.Drawing.ContentAlignment.TopLeft, "_____________________________________________");
                            mypdfpage.Add(pdf46z);

                            table1 = mydoc.NewTable(Fontsmall1, 7, 3, 3);
                            table1.SetBorders(Color.Black, 1, BorderType.ColumnsAndBounds);

                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 0).SetContent("Grade");
                            table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 1).SetContent("Academic");
                            table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(0, 2).SetContent("Character");
                            table1.Rows[0].SetCellPadding(9);
                            table1.Rows[1].SetCellPadding(5);
                            table1.Rows[2].SetCellPadding(5);
                            table1.Rows[3].SetCellPadding(5);
                            table1.Rows[4].SetCellPadding(5);
                            table1.Rows[5].SetCellPadding(5);
                            table1.Rows[6].SetCellPadding(5);
                            table1.Cell(0, 0).SetFont(Fontbold);
                            table1.Cell(0, 1).SetFont(Fontbold);
                            table1.Cell(0, 2).SetFont(Fontbold);

                            table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(1, 0).SetContent("E");
                            table1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(1, 1).SetContent("       Excellent");
                            table1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(1, 2).SetContent("       80 to 100 %");

                            table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(2, 0).SetContent("O");
                            table1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(2, 1).SetContent("       Outstanding");
                            table1.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(2, 2).SetContent("       70 to 79 %");

                            table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(3, 0).SetContent("A");
                            table1.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(3, 1).SetContent("       Good");
                            table1.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(3, 2).SetContent("       60 to 60 %");

                            table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(4, 0).SetContent("B");
                            table1.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(4, 1).SetContent("       Improving");
                            table1.Cell(4, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(4, 2).SetContent("       50 to 59 %");

                            table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(5, 0).SetContent("C");
                            table1.Cell(5, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(5, 1).SetContent("       Must Improve");
                            table1.Cell(5, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(5, 2).SetContent("       40 to 49 %");

                            table1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table1.Cell(6, 0).SetContent("D");
                            table1.Cell(6, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(6, 1).SetContent("       Undesirable");
                            table1.Cell(6, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(6, 2).SetContent("       below 40 %");
                            table1.VisibleHeaders = false;

                            Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 60, 250, 300, 500));
                            mypdfpage.Add(newpdftabpage1);

                            mypdfpage.Add(pdf1);
                            mypdfpage.Add(pdf11);
                            mypdfpage.Add(pdf12);
                            mypdfpage.Add(pdf13);
                            mypdfpage.Add(pdf14);
                            mypdfpage.Add(pdf15);

                            ////////////////////total rec/////////////////
                            //PdfArea pa1 = new PdfArea(mydoc, 14, 12, 810, 575);
                            //PdfRectangle pr3 = new PdfRectangle(mydoc, pa1, Color.Black);
                            //mypdfpage.Add(pr3);
                            ///////////////////left rec/////////////////////////

                            PdfArea pa12 = new PdfArea(mydoc, 14, 12, 390, 575);
                            PdfRectangle pr4 = new PdfRectangle(mydoc, pa12, Color.Black);
                            mypdfpage.Add(pr4);

                            /////////////////right////////////////////////

                            PdfArea pa5 = new PdfArea(mydoc, 415, 12, 410, 575);
                            PdfRectangle pr5 = new PdfRectangle(mydoc, pa5, Color.Black);
                            mypdfpage.Add(pr5);
                            mypdfpage.SaveToDocument();

                        }
                    }
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "rankcard" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";

                mydoc.SaveToFile(szPath + szFile);
                //mydocback.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = Convert.ToString(ex);
            lblerrormsg.Visible = true;
        }
    }

    public void GetTest()
    {
        try
        {
            string SyllabusYr;
            string SyllabusQry;
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + Convert.ToString(ddstandard.SelectedValue) + " and semester =" + Convert.ToString(dropterm.SelectedValue) + " and batch_year=" + Convert.ToString(dropyear.SelectedValue) + "";
            SyllabusYr = d2.GetFunction(SyllabusQry.ToString());
            string Sqlstr;
            Sqlstr = string.Empty;
            if (SyllabusYr == "0")
                SyllabusYr = "null";
            Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + Convert.ToString(ddstandard.SelectedValue) + " and semester=" + Convert.ToString(dropterm.SelectedValue) + " and batch_year=" + Convert.ToString(dropyear.SelectedValue) + " order by criteria";

            DataSet titles = new DataSet();
            titles = d2.select_method_wo_parameter(Sqlstr, "text");
            count = 0;
            if (titles.Tables[0].Rows.Count > 0)
            {
                chklstest.DataSource = titles;
                chklstest.DataValueField = "Criteria_No";
                chklstest.DataTextField = "Criteria";
                chklstest.DataBind();
                chklstest.SelectedIndex = chklstest.Items.Count - 1;
                for (int i = 0; i < chklstest.Items.Count; i++)
                {
                    chklstest.Items[i].Selected = true;
                    if (chklstest.Items[i].Selected == true)
                    {
                        count += 1;
                        txt_Test.Text = "Test(" + count.ToString() + ")";
                    }
                    if (chklstest.Items.Count == count)
                    {
                        chktest.Checked = true;
                    }
                    else
                    {
                        chktest.Checked = false;
                    }
                }
            }
            else
            {
                chklstest.Items.Clear();
                txt_Test.Text = "---Select---";
            }
        }
        catch
        {

        }
    }

    protected void chktest_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chktest.Checked == true)
            {
                for (int i = 0; i < chklstest.Items.Count; i++)
                {
                    chklstest.Items[i].Selected = true;
                    txt_Test.Text = "Test(" + (chklstest.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstest.Items.Count; i++)
                {
                    chklstest.Items[i].Selected = false;
                    txt_Test.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            //lblset.Text =  Convert.ToString(ex);
        }
    }

    protected void chklsttest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clg = string.Empty;
            int commcount = 0;
            for (int i = 0; i < chklstest.Items.Count; i++)
            {
                if (chklstest.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txt_Test.Text = "Test(" + Convert.ToString(commcount) + ")";
                    if (clg == "")
                    {
                        clg = Convert.ToString(chklstest.Items[i].Value);
                    }
                    else
                    {
                        clg = clg + "','" + Convert.ToString(chklstest.Items[i].Value);
                    }
                }
            }
            if (chklstest.Items.Count == commcount)
            {
                chktest.Checked = true;
            }
            else
            {
                chktest.Checked = false;
            }
            if (commcount == 0)
            {
                txt_Test.Text = "--Select--";
                chktest.Checked = false;
            }
        }
        catch (Exception ex)
        {
            //lblset.Text =  Convert.ToString(ex);
        }
    }

    public void filteration()
    {
        string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");

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

    public string findresult(string att)
    {
        string atten = att;
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
            //Added By subburaj 21.08.2014****//
            case "-18":
                atten = "RAA";
                break;
            //********End**********************//
        }
        return atten;
    }

    /// <summary>
    /// author Malang Raja T
    /// </summary>
    /// <param name="txtConvertTo">A string type txtConvertTo is used for to be converted</param>
    /// <param name="maxMark">ref type maxMark parameter was used to gives the minimum mark for converted obtained marks</param>
    /// <param name="obtainedMark">ref type obtainedMark parameter was used to gives the calculated or converted obtained marks</param>
    /// <param name="minMark">ref type minMark parameter was used to gives the minimum mark for converted obtained marks</param>
    public void ConvertedMark(string txtConvertTo, ref string maxMark, ref string obtainedMark, ref string minMark)
    {
        int Mark, max;
        bool r = int.TryParse(obtainedMark, out Mark);
        bool maxflag = int.TryParse(txtConvertTo, out max);
        double multiply;
        double minmultyply;
        double min = 0;
        double max_minCal = 0;
        bool maxbool = double.TryParse(maxMark, out max_minCal);
        bool minbool = double.TryParse(minMark, out min);
        if (maxflag)
        {
            if (r)
            {
                switch (txtConvertTo)
                {
                    default:
                        multiply = double.Parse(txtConvertTo) / int.Parse(maxMark);
                        if (maxbool == true && minbool == true && min > 0)
                        {
                            minmultyply = max_minCal / min;
                            min = int.Parse(txtConvertTo) / minmultyply;
                        }
                        obtainedMark = Convert.ToString(Mark * multiply);
                        break;
                }
            }
            minMark = min.ToString();
            maxMark = txtConvertTo;
        }
    }

    public bool findgrade(DataTable dt, string obtainedmarks, ref string Grade)
    {
        bool result = false;
        if (dt.Rows.Count > 0)
        {
            double marks = 0;
            double.TryParse(obtainedmarks, out marks);
            marks = Math.Round(marks, 0);
            //"Between Frange and Trange";
            dt.DefaultView.RowFilter = "Frange<='" + marks + "' and Trange>='" + marks + "'";
            DataView dv = new DataView();
            dv = dt.DefaultView;
            if (dv.Count > 0)
            {
                if (dv[0]["Mark_Grade"].ToString() != "" && dv[0]["Mark_Grade"].ToString() != null)
                {
                    Grade = dv[0]["Mark_Grade"].ToString();
                    result = true;
                }
                else
                {
                    Grade = obtainedmarks;
                    return false;
                }
            }
            else
            {
                Grade = obtainedmarks;
                result = false;
                return false;
            }
        }
        else
        {
            Grade = obtainedmarks;
            result = false;
        }
        return result;
    }

}