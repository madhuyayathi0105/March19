#region Namespace Declaration

using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Collections;
using Farpnt = FarPoint.Web.Spread;
using System.IO;
using Gios.Pdf;
using System.Configuration;


#endregion Namespace Declaration

public partial class ReportCard_Nursery_LKG_UKG_ : System.Web.UI.Page
{

    #region Variable Declaration

    Hashtable hat = new Hashtable();
    string usercode = "", collegecode = "", singleuser = "", group_user = "";
    string batch_year = "", degree_code = "", semester = "", section = "", test_name = "", test_no = "", rollnos = "";

    string grouporusercode = "";

    bool serialflag;
    string qry = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet studgradeds = new DataSet();
    Boolean b_school = false;

    FarPoint.Web.Spread.ComboBoxCellType combocol = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType combocolgrade = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxcol = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxsel_all = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.TextCellType txtceltype = new FarPoint.Web.Spread.TextCellType();

    #region For Attendance Calculation

    string currentsem = "";
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double per_holidate;
    int tot_per_hrs;
    double njhr, njdate, per_njdate;
    double per_per_hrs;

    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds_attnd_pts = new DataSet();

    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = "";

    string startdate = "";
    string enddate = "";
    string tempvalue = "-1";
    Boolean yesflag = false;

    static Hashtable ht_sphr = new Hashtable();
    Hashtable hatonduty = new Hashtable();
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();

    double per_perhrs, per_abshrs;
    double per_ondu, per_leave, per_hhday;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    string working = "";
    string present = "";
    string working1 = "";
    string present1 = "";
    string fvalue = "";
    string lvalue = "";

    int ObtValue = -1;
    TimeSpan ts;
    int rows_count;
    string value, date;
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
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
    string criteria_no = "";

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

    string strorderby = "";
    string lbltot_att1 = "";
    string lbltot_work1 = "";
    string lbltot_att2 = "";
    string lbltot_work2 = "";

    #endregion

    #region For Report

    DataTable dtallcol = new DataTable();
    ArrayList faillist = new ArrayList();
    ArrayList subfaillist = new ArrayList();

    #endregion

    #endregion Variable Declaration

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.Header.DataBind();
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
            usercode = Convert.ToString(Session["usercode"]);
            collegecode = Convert.ToString(Session["collegecode"]);
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            string grouporusercode = "";
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = d2.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = Convert.ToString(schoolds.Tables[0].Rows[0]["value"]);
                if (schoolvalue.Trim() == "0")
                {
                    b_school = true;
                }
            }
            if (!IsPostBack)
            {
                Session["attdaywisecla"] = "0";
                string daywisecal = d2.GetFunction("select value from Master_Settings where settings='Attendance Day Wise Calculation'");
                if (daywisecal.Trim() == "1")
                {
                    Session["attdaywisecla"] = "1";
                }
                lblErrSearch.Text = "";
                lblErrSearch.Visible = false;
                popupdiv.Visible = false;
                divViewSpread.Visible = false;
                collegecode = Convert.ToString(Session["collegecode"]);
                Bindcollege();
                BindBatch();
                bindDegree();
                bindBranch();
                bindsem();
                bindSection();
                bindtestname();
            }
            if (ddlCollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue);
            }
            ChangeHeaderName(b_school);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }

    }

    #endregion Page Load

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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Logout

    #region Bind Header

    public void Bindcollege()
    {
        try
        {
            string columnfield = "";
            group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", Convert.ToString(columnfield));
            ds.Dispose();
            ds.Clear();
            ds.Reset();
            ds = d2.select_method("bind_college", hat, "sp");
            ddlCollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
            else
            {
                lblErrSearch.Text = "Set college rights to the staff";
                lblErrSearch.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindBatch()
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            ds.Dispose();
            ds.Reset();
            ds.Clear();
            ds = d2.select_method_wo_parameter("bind_batch", "sp");
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void bindDegree()
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            ddlDegree.Items.Clear();
            usercode = Convert.ToString(Session["usercode"]);
            collegecode = Convert.ToString(ddlCollege.SelectedValue); ;
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
            ds.Clear();
            ds.Reset();
            ds.Dispose();
            ds = d2.select_method("bind_degree", hat, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDegree.DataSource = ds;
                ddlDegree.DataTextField = "course_name";
                ddlDegree.DataValueField = "course_id";
                ddlDegree.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void bindBranch()
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            ddlDept.Items.Clear();
            hat.Clear();
            usercode = Convert.ToString(Session["usercode"]);
            collegecode = Convert.ToString(ddlCollege.SelectedValue); ;
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", Convert.ToString(ddlDegree.SelectedValue));
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds.Clear();
            ds.Reset();
            ds.Dispose();
            ds = d2.select_method("bind_branch", hat, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDept.DataSource = ds;
                ddlDept.DataTextField = "dept_name";
                ddlDept.DataValueField = "degree_code";
                ddlDept.DataBind();
                ddlDept.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    //public void bindsem()
    //{
    //    try
    //    {
    //        lblErrSearch.Text = "";
    //        lblErrSearch.Visible = false;
    //        cbl_sem.Items.Clear();
    //        cb_sem.Checked = false;
    //        txtSem.Text = "---Select---";
    //        int i = 0;
    //        batch_year = Convert.ToString(ddlbatch.SelectedValue);
    //        degree_code = Convert.ToString(ddlDept.SelectedValue);

    //        if (batch_year != "" && degree_code != "")
    //        {
    //            ds.Clear();
    //            ds.Reset();
    //            ds.Dispose();
    //            ds = d2.BindSem(degree_code, batch_year, Convert.ToString(ddlCollege.SelectedValue));

    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                ds.Tables[0].DefaultView.RowFilter = "ndurations=max(ndurations)";
    //                DataView dv = ds.Tables[0].DefaultView;
    //                if (dv.Count > 0)
    //                {
    //                    int semcount = 0;
    //                    string semcountstring = Convert.ToString(dv[0][0]);
    //                    if (semcountstring != "")
    //                    {
    //                        semcount = Convert.ToInt32(semcountstring);
    //                    }
    //                    for (i = 1; i <= semcount; i++)
    //                    {
    //                        cbl_sem.Items.Add(Convert.ToString(i));
    //                    }
    //                }

    //                if (cbl_sem.Items.Count > 0)
    //                {
    //                    for (i = 0; i < cbl_sem.Items.Count; i++)
    //                    {
    //                        cbl_sem.Items[i].Selected = true;
    //                    }
    //                    txtSem.Text = ((!b_school) ? "Semester(" : "Term(") + cbl_sem.Items.Count + ")";

    //                    cb_sem.Checked = true;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //    }
    //}

    public void bindsem()
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            ddlSem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            batch_year = Convert.ToString(ddlbatch.SelectedValue);
            degree_code = Convert.ToString(ddlDept.SelectedValue);
            string sqlnew = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + Convert.ToString(degree_code) + " and batch_year=" + Convert.ToString(batch_year) + " and college_code=" + Convert.ToString(ddlCollege.SelectedValue) + "";
            DataSet ds = new DataSet();
            ds.Clear();
            ds.Reset();
            ds.Dispose();
            ds = d2.select_method_wo_parameter(sqlnew, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]));
                duration = Convert.ToInt16(Convert.ToString(ds.Tables[0].Rows[0][0]));
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSem.Items.Add(Convert.ToString(i));
                    }
                    else if (first_year == true && i == 2)
                    {
                        ddlSem.Items.Add(Convert.ToString(i));
                    }
                }
            }
            else
            {
                sqlnew = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + Convert.ToString(degree_code) + " and college_code=" + Convert.ToString(ddlCollege.SelectedValue) + "";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(sqlnew, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]), out first_year);
                    int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]), out duration);
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSem.Items.Add(Convert.ToString(i));
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSem.Items.Add(Convert.ToString(i));
                        }
                    }
                }
            }
            if (ddlSem.Items.Count > 0)
            {
                ddlSem.SelectedIndex = 0;
                bindSection();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void bindSection()
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            ddlsec.Enabled = false;
            ddlsec.Items.Clear();
            hat.Clear();
            ds.Clear();
            ds.Reset();
            ds.Dispose();
            ds = d2.BindSectionDetail(ddlbatch.SelectedValue, ddlDept.SelectedValue);
            int count5 = ds.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataValueField = "sections";
                ddlsec.DataBind();
                ddlsec.Enabled = true;
                ddlsec.Items.Insert(0, "All");
            }
            else
            {
                ddlsec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void bindtestname()
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            txt_test.Text = "--Select--";
            Cb_test.Checked = false;
            Cbl_test.Items.Clear();

            batch_year = Convert.ToString(ddlbatch.SelectedValue).Trim();
            degree_code = Convert.ToString(ddlDept.SelectedValue).Trim();
            semester = Convert.ToString(ddlSem.SelectedValue).Trim();

            string SyllabusYr;
            string SyllabusQry;

            if (batch_year.Trim() != "" && degree_code.Trim() != "" && semester.Trim() != "")
            {
                //string Sqlstr = "select distinct c.criteria from criteriaforinternal c,registration r,syllabus_master s where r.degree_code=s.degree_code and r.batch_year=s.batch_year and c.syll_code=s.syll_code and cc=0 and delflag=0 and r.exam_flag<>'debar'   and r.batch_year='" + batch_year + "' and r.degree_code in(" + degree_code + ") and  s.semester in (" + semester + ") order by criteria asc";
                SyllabusQry = "select syllabus_year from syllabus_master where degree_code in (" + Convert.ToString(degree_code) + ") and semester in (" + Convert.ToString(semester) + ") and batch_year in (" + Convert.ToString(batch_year) + ")";
                SyllabusYr = d2.GetFunction(Convert.ToString(SyllabusQry));
                string Sqlstr;
                Sqlstr = "";
                if (SyllabusYr == "0")
                    SyllabusYr = "null";
                Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code in (" + Convert.ToString(degree_code) + ") and semester in (" + Convert.ToString(semester) + ") and batch_year in (" + Convert.ToString(batch_year) + ") order by criteria_no";

                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(Sqlstr, "Text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Cbl_test.DataSource = ds;
                    Cbl_test.DataValueField = "criteria_no";
                    Cbl_test.DataTextField = "criteria";
                    Cbl_test.DataBind();
                }
                if (Cbl_test.Items.Count > 0)
                {
                    for (int row = 0; row < Cbl_test.Items.Count; row++)
                    {
                        Cbl_test.Items[row].Selected = true;
                        Cb_test.Checked = true;
                    }
                    txt_test.Text = "Test(" + Cbl_test.Items.Count + ")";
                }
                else
                {
                    txt_test.Text = "--Select--";
                }
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Bind Header

    #region Initialize Spread

    public void Init_Spread()
    {
        try
        {
            bool isSpl = false;
            bool isfree = false;
            int reasonrow = 0;

            #region FpSpread Style

            FpViewSpread.Visible = false;
            FpViewSpread.Sheets[0].ColumnCount = 0;
            FpViewSpread.Sheets[0].RowCount = 0;
            FpViewSpread.Sheets[0].SheetCorner.ColumnCount = 0;
            FpViewSpread.CommandBar.Visible = false;

            #endregion FpSpread Style

            FpViewSpread.Height = 350;
            FpViewSpread.Width = 580;

            FpViewSpread.Visible = false;
            FpViewSpread.CommandBar.Visible = false;
            FpViewSpread.RowHeader.Visible = false;
            FpViewSpread.Sheets[0].AutoPostBack = false;
            FpViewSpread.Sheets[0].RowCount = 1;
            FpViewSpread.Sheets[0].ColumnCount = 5;
            FpViewSpread.Sheets[0].FrozenRowCount = 1;


            #region SpreadStyles

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = System.Drawing.Color.White;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.Black;

            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            //sheetstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Medium;
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Left;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

            #endregion SpreadStyles

            FpViewSpread.HorizontalScrollBarPolicy = Farpnt.ScrollBarPolicy.AsNeeded;
            FpViewSpread.VerticalScrollBarPolicy = Farpnt.ScrollBarPolicy.AsNeeded;

            FpViewSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpViewSpread.Sheets[0].DefaultStyle = sheetstyle;
            FpViewSpread.Sheets[0].ColumnHeader.RowCount = 2;
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg.No";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Name";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Sex";

            FpViewSpread.Sheets[0].Columns[0].Width = 40;
            FpViewSpread.Sheets[0].Columns[1].Width = 50;
            FpViewSpread.Sheets[0].Columns[2].Width = 120;
            FpViewSpread.Sheets[0].Columns[3].Width = 250;
            FpViewSpread.Sheets[0].Columns[4].Width = 100;

            FpViewSpread.Sheets[0].Columns[0].Locked = true;
            FpViewSpread.Sheets[0].Columns[2].Locked = true;
            FpViewSpread.Sheets[0].Columns[3].Locked = true;
            FpViewSpread.Sheets[0].Columns[4].Locked = true;

            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            chkboxsel_all.AutoPostBack = true;
            FpViewSpread.Sheets[0].Cells[0, 1].CellType = chkboxsel_all;
            FpViewSpread.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpViewSpread.Sheets[0].Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
            FpViewSpread.Sheets[0].SpanModel.Add(0, 2, 1, 3);
            FpViewSpread.SaveChanges();

            FpViewSpread.Sheets[0].PageSize = FpViewSpread.Sheets[0].RowCount;

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }

    }

    #endregion Initialize Spread

    #region DropDownList Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divViewSpread.Visible = false;
            BindBatch();
            bindDegree();
            bindBranch();
            bindsem();
            bindSection();
            bindtestname();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divViewSpread.Visible = false;
            bindDegree();
            bindBranch();
            bindsem();
            bindSection();
            bindtestname();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divViewSpread.Visible = false;
            bindBranch();
            bindsem();
            bindSection();
            bindtestname();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divViewSpread.Visible = false;
            bindsem();
            bindSection();
            bindtestname();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divViewSpread.Visible = false;
            bindSection();
            bindSection();
            bindtestname();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divViewSpread.Visible = false;
            bindtestname();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion DropDownList Events

    #region CheckBox Events

    //protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblErrSearch.Text = "";
    //        lblErrSearch.Visible = false;
    //        popupdiv.Visible = false;
    //        divViewSpread.Visible = false;
    //        int i = 0;
    //        txtSem.Text = "--Select--";
    //        if (cb_sem.Checked == true)
    //        {
    //            for (i = 0; i < cbl_sem.Items.Count; i++)
    //            {
    //                cbl_sem.Items[i].Selected = true;
    //            }
    //            txtSem.Text = ((!b_school) ? "Semester(" : "Term(") + (cbl_sem.Items.Count) + ")";
    //        }
    //        else
    //        {
    //            for (i = 0; i < cbl_sem.Items.Count; i++)
    //            {
    //                cbl_sem.Items[i].Selected = false;
    //            }
    //        }
    //        bindSection();
    //        bindtestname();
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //    }
    //}

    protected void Cb_test_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divViewSpread.Visible = false;
            int cout = 0;
            txt_test.Text = "--Select--";
            if (Cb_test.Checked == true)
            {
                cout++;
                for (int i = 0; i < Cbl_test.Items.Count; i++)
                {
                    Cbl_test.Items[i].Selected = true;
                }
                txt_test.Text = "Test(" + (Cbl_test.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cbl_test.Items.Count; i++)
                {
                    Cbl_test.Items[i].Selected = false;
                }
                txt_test.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion CheckBox Events

    #region CheckBoxList Events

    //protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblErrSearch.Text = "";
    //        lblErrSearch.Visible = false;
    //        popupdiv.Visible = false;
    //        divViewSpread.Visible = false;

    //        int i = 0;
    //        cb_sem.Checked = false;
    //        int commcount = 0;
    //        txtSem.Text = "--Select--";
    //        for (i = 0; i < cbl_sem.Items.Count; i++)
    //        {
    //            if (cbl_sem.Items[i].Selected == true)
    //            {
    //                commcount = commcount + 1;
    //            }
    //        }
    //        if (commcount > 0)
    //        {
    //            if (commcount == cbl_sem.Items.Count)
    //            {
    //                cb_sem.Checked = true;
    //            }
    //            txtSem.Text = ((b_school) ? "Term(" : "Semester(") +Convert.ToString( commcount) + ")";
    //        }
    //        bindSection();
    //        bindtestname();
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //    }
    //}

    protected void Cbl_test_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divViewSpread.Visible = false;

            int commcount = 0;
            txt_test.Text = "--Select--";
            Cb_test.Checked = false;

            for (int i = 0; i < Cbl_test.Items.Count; i++)
            {
                if (Cbl_test.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    Cb_test.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == Cbl_test.Items.Count)
                {

                    Cb_test.Checked = true;
                }
                txt_test.Text = "Test(" + Convert.ToString(commcount) + ")";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion CheckBoxList Events

    protected void FpViewSpread_Command(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = "";
            if (Convert.ToInt32(FpViewSpread.Sheets[0].Cells[0, 1].Value) == 1)
            {
                for (int i = 0; i < FpViewSpread.Sheets[0].RowCount; i++)
                {
                    FpViewSpread.Sheets[0].Cells[i, 1].Value = 1;
                }
            }
            else if (Convert.ToInt32(FpViewSpread.Sheets[0].Cells[0, 1].Value) == 0)
            {
                for (int i = 0; i < FpViewSpread.Sheets[0].RowCount; i++)
                {
                    FpViewSpread.Sheets[0].Cells[i, 1].Value = 0;
                }
            }
            FpViewSpread.Visible = true;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #region Button Click

    #region Go Button

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpoperr.Text = "";
            batch_year = "";
            degree_code = "";
            collegecode = "";
            semester = "";

            section = "";
            test_name = "";
            test_no = "";
            int selsem = 0;
            int seltest = 0;
            int[] arr_semester = new int[1];
            int[] arr_test = new int[1];

            if (ddlCollege.Items.Count == 0)
            {
                lblpoperr.Text = ((!b_school) ? "College" : "School") + " is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue);
            }

            if (ddlbatch.Items.Count == 0)
            {
                lblpoperr.Text = ((!b_school) ? "Batch Year" : "Year") + " is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                batch_year = Convert.ToString(ddlbatch.SelectedValue);
            }

            if (ddlDegree.Items.Count == 0)
            {
                lblpoperr.Text = ((!b_school) ? "Degree" : "School Type") + " is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
            }

            if (ddlDept.Items.Count == 0)
            {
                lblpoperr.Text = ((!b_school) ? "Branch" : "Standard") + " is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                degree_code = Convert.ToString(ddlDept.SelectedValue);
            }

            if (ddlSem.Items.Count == 0)
            {
                lblpoperr.Text = ((!b_school) ? "Semester" : "Term") + " is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                semester = Convert.ToString(ddlSem.SelectedValue);
            }

            if (ddlsec.Enabled == true)
            {
                if (ddlsec.Items.Count > 0)
                {
                    section = "";
                    if (Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() != "")
                    {
                        section = Convert.ToString(ddlsec.SelectedItem.Text);
                        section = "and r.sections in ('" + section + "') ";
                        //newsecqry = " and sections in ('" + Convert.ToString(ddlsec.SelectedItem.Text) + "') ";
                    }
                    else
                    {
                        section = "";
                        //newsecqry = "";
                    }
                }
            }
            else
            {
                section = "";
                //newsecqry = "";
            }
            //if (Cbl_test.Items.Count == 0)
            //{
            //    lblpoperr.Text = "Test is Not Found";
            //    popupdiv.Visible = true;
            //    return;
            //}
            //else
            //{
            //    test_no = "";
            //    test_name = "";
            //    foreach (ListItem li in Cbl_test.Items)
            //    {
            //        if (li.Selected)
            //        {
            //            if (seltest != 0)
            //                Array.Resize(ref arr_test, seltest + 1);
            //            int.TryParse(li.Value, out arr_test[seltest]);
            //            seltest++;
            //            if (test_no == "")
            //            {
            //                test_no = "'" + li.Value + "'";
            //                test_name = "'" + li.Text + "'";
            //            }
            //            else
            //            {
            //                test_no += ",'" + li.Value + "'";
            //                test_name += ",'" + li.Text + "'";
            //            }
            //        }
            //    }
            //    if (seltest == 0)
            //    {
            //        lblpoperr.Text = "Please Select Atleast One Test";
            //        popupdiv.Visible = true;
            //        return;
            //    }
            //}

            string collcode = " and r.college_code='" + Convert.ToString(collegecode) + "'";
            string batchyear = " and r.Batch_Year='" + Convert.ToString(batch_year) + "'";
            string degreecode = " and r.degree_code='" + Convert.ToString(degree_code) + "'";
            string sec = "";
            // term = "and sc.semester='" + Convert.ToString(ddlSem.SelectedItem.Text) + "'";     

            if (ddlsec.Enabled == true)
            {
                if (ddlsec.SelectedItem.Text != "All")
                {
                    for (int sc = 0; sc < ddlsec.Items.Count; sc++)
                    {
                        sec = "and r.Sections in ('" + Convert.ToString(ddlsec.SelectedItem.Text) + "')";
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
            for (int i = 0; i < FpViewSpread.Sheets[0].Rows.Count; i++)
            {
                FpViewSpread.Sheets[0].Cells[i, 1].Value = 0;
            }
            string sqlcondition = collcode + batchyear + degreecode + sec;
            bool serialflag = false;
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
                strorderby = "";
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

                qry = "SELECT distinct r.Roll_No,R.Stud_Name,a.sex,serialno,r.Reg_No FROM Registration R,Applyn A WHERE R.App_No = A.App_No " + sqlcondition + "  and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' " + strorderby + ""; //and r.Current_Semester<='" + Convert.ToString(arr_semester.Max()) + "'
            }
            else
            {
                qry = "SELECT distinct r.Roll_No,R.Stud_Name,a.sex,serialno,r.Reg_No FROM Registration R,Applyn A WHERE R.App_No = A.App_No " + sqlcondition + "  and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' ORDER BY serialno"; //and r.Current_Semester<='" + Convert.ToString(arr_semester.Max()) + "'
            }

            ds.Clear();
            ds = d2.select_method_wo_parameter(qry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Init_Spread();
                FpViewSpread.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count + 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FpViewSpread.Sheets[0].Cells[i + 1, 1].CellType = chkboxcol;
                    FpViewSpread.Sheets[0].Cells[i + 1, 1].Font.Name = "Book Antiqua";
                    FpViewSpread.Sheets[0].Cells[i + 1, 1].Font.Size = FontUnit.Medium;
                    FpViewSpread.Sheets[0].Cells[i + 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpViewSpread.Sheets[0].Cells[i + 1, 1].VerticalAlign = VerticalAlign.Middle;


                    FpViewSpread.Sheets[0].Cells[i + 1, 0].Text = Convert.ToString(i + 1);
                    FpViewSpread.Sheets[0].Cells[i + 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpViewSpread.Sheets[0].Cells[i + 1, 0].VerticalAlign = VerticalAlign.Middle;
                    FpViewSpread.Sheets[0].Cells[i + 1, 0].Locked = true;
                    FpViewSpread.Sheets[0].Cells[i + 1, 0].Font.Name = "Book Antiqua";
                    FpViewSpread.Sheets[0].Cells[i + 1, 0].Font.Size = FontUnit.Medium;


                    FpViewSpread.Sheets[0].Cells[i + 1, 2].CellType = txtceltype;
                    FpViewSpread.Sheets[0].Cells[i + 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i][0]);
                    FpViewSpread.Sheets[0].Cells[i + 1, 2].Locked = true;
                    FpViewSpread.Sheets[0].Cells[i + 1, 2].Font.Name = "Book Antiqua";
                    FpViewSpread.Sheets[0].Cells[i + 1, 2].Font.Size = FontUnit.Medium;
                    FpViewSpread.Sheets[0].Cells[i + 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpViewSpread.Sheets[0].Cells[i + 1, 2].VerticalAlign = VerticalAlign.Middle;

                    FpViewSpread.Sheets[0].Cells[i + 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i][1]);
                    FpViewSpread.Sheets[0].Cells[i + 1, 3].Locked = true;
                    FpViewSpread.Sheets[0].Cells[i + 1, 3].Font.Name = "Book Antiqua";
                    FpViewSpread.Sheets[0].Cells[i + 1, 3].Font.Size = FontUnit.Medium;
                    FpViewSpread.Sheets[0].Cells[i + 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpViewSpread.Sheets[0].Cells[i + 1, 3].VerticalAlign = VerticalAlign.Middle;

                    FpViewSpread.Sheets[0].Cells[i + 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i][3]);

                    string ssex = Convert.ToString(ds.Tables[0].Rows[i][2]);

                    if (ssex.Trim() == "0")
                    {
                        ssex = "Male";
                    }
                    else
                    {
                        ssex = "Female";
                    }

                    FpViewSpread.Sheets[0].Cells[i + 1, 4].Text = ssex;
                    FpViewSpread.Sheets[0].Cells[i + 1, 4].Locked = true;
                    FpViewSpread.Sheets[0].Cells[i + 1, 4].Font.Name = "Book Antiqua";
                    FpViewSpread.Sheets[0].Cells[i + 1, 4].Font.Size = FontUnit.Medium;
                    FpViewSpread.Sheets[0].Cells[i + 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    FpViewSpread.Sheets[0].Cells[i + 1, 4].VerticalAlign = VerticalAlign.Middle;

                }
                for (int i = 1; i < FpViewSpread.Sheets[0].Rows.Count; i++)
                {
                    FpViewSpread.Sheets[0].Rows[i].BackColor = ColorTranslator.FromHtml("#E6e6e6");
                    i++;
                }
                FpViewSpread.SaveChanges();
                //FpViewSpread.Sheets[0].PageSize = FpViewSpread.Sheets[0].RowCount;
                //FpViewSpread.Height = (FpViewSpread.Sheets[0].RowCount * 25) + 50;
                //if ((FpViewSpread.Sheets[0].RowCount * 25) + 50 < 200)
                //    FpViewSpread.Height = 450;
                divViewSpread.Visible = true;
                FpViewSpread.Visible = true;
                FpViewSpread.Sheets[0].PageSize = FpViewSpread.Sheets[0].RowCount;
            }
            else
            {
                divViewSpread.Visible = false;
                popupdiv.Visible = true;
                lblpoperr.Text = "There are no students available";
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion Go Button

    #region Print Report

    protected void btnrpt_Click(object sender, EventArgs e)
    {
        try
        {
            popupdiv.Visible = false;
            lblpoperr.Text = "";
            lblErrSearch.Text = "";
            lblErrSearch.Visible = false;
            int checkedcount = 0;
            rollnos = "";
            FpViewSpread.SaveChanges();
            if (FpViewSpread.Sheets[0].RowCount > 1)
            {
                for (int i = 0; i < FpViewSpread.Sheets[0].RowCount; i++)
                {
                    if (Convert.ToInt32(FpViewSpread.Sheets[0].Cells[i, 1].Value) == 1)
                    {
                        checkedcount++;
                        if (rollnos == "")
                        {
                            rollnos = "'" + Convert.ToString(FpViewSpread.Sheets[0].Cells[i, 2].Text) + "'";
                        }
                        else
                        {
                            rollnos = rollnos + ",'" + Convert.ToString(FpViewSpread.Sheets[0].Cells[i, 2].Text) + "'";
                        }
                    }
                }
                if (checkedcount == 0)
                {
                    lblpoperr.Text = "Please Select Atleast Any one Student";
                    popupdiv.Visible = true;
                    return;
                }
                if (rollnos.Trim().Trim(',') != "")
                {
                    if (Cbl_test.Items.Count == 0)
                    {
                        lblpoperr.Text = "No Test Were Found.";
                        lblpoperr.Visible = true;
                        popupdiv.Visible = true;
                        return;
                    }
                    else if (Cbl_test.Items.Count > 0)
                    {
                        if (txt_test.Text == "---Select---")
                        {
                            lblpoperr.Text = "Please Select Atleast one Test";
                            lblpoperr.Visible = true;
                            popupdiv.Visible = true;
                            return;
                        }
                        else
                        {
                            Nursery_LKG_UKG_Reportcard(rollnos.Trim().Trim(','));
                        }
                    }
                    //Nursery_LKG_UKG_Reportcard(rollnos.Trim().Trim(','));
                }
            }
            else
            {
                lblpoperr.Text = "No Student Were Found";
                popupdiv.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion

    #region Popup Error

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        try
        {
            popupdiv.Visible = false;
            lblpoperr.Text = "";
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Popup Error

    #endregion Button Click

    #region ReportCard

    public void Nursery_LKG_UKG_Reportcard(string rollno)
    {
        string batch = "";
        string degreecode1 = "";
        string semester = "";
        string exam_held = "";
        string criteria_no = "";
        string tstnum = "";
        string test = "";
        string test1 = "";
        string studname = "";
        Double getheigh = 0;
        int coltop = 0;
        int testcount = 0;
        string q = "";
        DataSet dsMarksDetails = new DataSet();
        DataSet dschool = new DataSet();
        lblErrSearch.Text = "";
        lblErrSearch.Visible = false;
        try
        {
            for (int i = 0; i < Cbl_test.Items.Count; i++)
            {
                if (Cbl_test.Items[i].Selected == true)
                {
                    testcount++;
                    if (criteria_no == "")
                    {
                        criteria_no = Convert.ToString(Cbl_test.Items[i].Value);
                        test = Convert.ToString(Cbl_test.Items[i].Text);
                        test1 = Convert.ToString(Cbl_test.Items[i].Text);
                        tstnum = Convert.ToString(Cbl_test.Items[i].Value);
                    }
                    else
                    {
                        criteria_no += "','" + Convert.ToString(Cbl_test.Items[i].Value);
                        test += "','" + Convert.ToString(Cbl_test.Items[i].Text);
                        test1 += "," + Convert.ToString(Cbl_test.Items[i].Text);
                        tstnum += "," + Convert.ToString(Cbl_test.Items[i].Value);
                    }
                }
            }
            bool status = false;
            DataSet dsexammark = new DataSet();
            strorderby = d2.GetFunction("select LinkValue from inssettings where college_code=" + Convert.ToString(collegecode) + " and linkname='Student Attendance'");

            qry = "SELECT collname,affliatedby,address1,address2,district,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile,parentF_Mobile,StuPer_Id,parent_addressc,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,a.bldgrp,studhouse,Districtp,parent_statep,parent_pincodep,parentM_Mobile,countryp,serialno,emailM,ParentidP from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code";
            ds.Clear();
            ds.Dispose();
            ds = d2.select_method_wo_parameter(qry, "Text");
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
                strorderby = "";
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

            dtallcol.Clear();
            dtallcol.Columns.Clear();

            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;

            DataSet dset = new DataSet();
            int checkattalign = 650;
            int moveleftvalue = 20;
            string college_code = Convert.ToString(collegecode);
            string stdappno = "";
            Font Fontmedium1 = new Font("Times New Roman", 14, FontStyle.Bold);
            Font Fontbold1 = new Font("Times New Roman", 16, FontStyle.Bold);
            Font Fontboldhead = new Font("Times New Roman", 10, FontStyle.Bold);
            Font Fontbold = new Font("Times New Roman", 12, FontStyle.Bold);
            //Font f12 = new Font("Times New Roman", 12, FontStyle.Bold);
            Font f7 = new Font("Times New Roman", 7, FontStyle.Bold);
            Font f8 = new Font("Times New Roman", 8, FontStyle.Bold);
            Font f9 = new Font("Times New Roman", 9, FontStyle.Bold);
            Font f10 = new Font("Times New Roman", 10, FontStyle.Bold);
            Font f11 = new Font("Times New Roman", 11, FontStyle.Bold);
            Font Fontmedium2 = new Font("Times New Roman", 16, FontStyle.Regular);
            Font Fontmedium = new Font("Times New Roman", 14, FontStyle.Regular);
            Font Fontsmall9 = new Font("Times New Roman", 9, FontStyle.Regular);
            Font Fontsmall = new Font("Times New Roman", 12, FontStyle.Regular);
            Font Fontsmall1 = new Font("Times New Roman", 10, FontStyle.Regular);
            Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            //Font f1 = new Font("Times New Roman", 7, FontStyle.Regular);
            Font f2 = new Font("Times New Roman", 8, FontStyle.Regular);
            Font f3 = new Font("Times New Roman", 9, FontStyle.Regular);
            Font f4 = new Font("Times New Roman", 10, FontStyle.Regular);
            Font f5 = new Font("Times New Roman", 11, FontStyle.Regular);
            Font f6 = new Font("Times New Roman", 12, FontStyle.Regular);
            Font f16 = new Font("Times New Roman", 12, FontStyle.Regular);
            Font fr8 = new Font("Times New Roman", 10, FontStyle.Regular);
            rollnos = rollno;
            if (rollnos != "")
            {
                DataTable dtStud = new DataTable();
                DataTable dtActivity = new DataTable();
                DataTable dtSubcount = new DataTable();
                DataTable dtGrade = new DataTable();
                DataTable dtAtt = new DataTable();
                DataTable dtRmrk = new DataTable();
                DataView dvStud = new DataView();
                if (serialflag == false)
                {
                    qry = "select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile,parentF_Mobile,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,bldgrp,studhouse from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No in (" + rollnos + ") " + strorderby + "";
                }
                else
                {
                    qry = "select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile,parentF_Mobile,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,bldgrp,studhouse,serialno from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No in (" + rollnos + ") order by serialno";
                }
                studgradeds.Clear();
                studgradeds = d2.select_method_wo_parameter(qry, "text");
                if (testcount > 0)
                {
                    q = "select R.Current_Semester,r.Roll_No,r.sections,r.Reg_No,r.Stud_Name,r.Roll_Admit,c.criteria,c.Criteria_no,e.exam_code,convert(varchar(10),e.exam_date,103)as exam_date,e.max_mark,e.min_mark,sub.subject_name,sub.subject_code,s.subject_no,re.marks_obtained,isnull(re.remarks,'') as remarks,r.degree_code,sub.subType_no,sub.syll_code from Registration r,CriteriaForInternal c,Exam_type e,Result re,subject sub,subjectchooser s where c.Criteria_no=e.criteria_no and sub.subject_no=s.subject_no and s.subject_no=e.subject_no and s.roll_no=r.roll_no and e.exam_code=re.exam_code and r.roll_no=re.roll_no and r.Batch_Year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and r.degree_code='" + Convert.ToString(ddlDept.SelectedValue) + "' and e.criteria_no in('" + criteria_no + "') and college_code='" + collegecode + "' and cc=0 and delflag=0 and exam_flag<>'Debar';select distinct e.subject_no,s.subject_name,s.subject_code from Exam_type e,subject s,CriteriaForInternal c where c.Criteria_no=e.criteria_no and s.subject_no=e.subject_no and c.syll_code in (select syll_code from syllabus_master where degree_code='" + Convert.ToString(ddlDept.SelectedValue) + "' and batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and semester='" + Convert.ToString(ddlSem.SelectedValue) + "') and c.criteria in ('" + test + "') order by e.subject_no;select distinct e.subject_no,s.subject_name,s.subject_code,c.Criteria_no,e.max_mark,e.min_mark from Exam_type e,subject s,CriteriaForInternal c where c.Criteria_no=e.criteria_no and s.subject_no=e.subject_no and c.syll_code in (select syll_code from syllabus_master where degree_code='" + Convert.ToString(ddlDept.SelectedValue) + "' and batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and semester='" + Convert.ToString(ddlSem.SelectedValue) + "') and criteria in ('" + test + "') order by e.subject_no; select Frange,Trange,Mark_Grade,Grade_Master.Credit_Points from Grade_Master where College_Code='" + college_code + "' and batch_year='" + Convert.ToString(ddlbatch.SelectedItem) + "' and Degree_Code='" + Convert.ToString(ddlDept.SelectedValue) + "' ; SELECT * from CoCurrActivitie_Det where Degree_Code='" + Convert.ToString(ddlDept.SelectedValue) + "' and batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and istype='Att' and term='" + Convert.ToString(ddlSem.SelectedValue) + "'; SELECT * from CoCurrActivitie_Det where Degree_Code='" + Convert.ToString(ddlDept.SelectedValue) + "' and batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and istype='remks' and term='" + Convert.ToString(ddlSem.SelectedValue) + "'; select distinct ca.CoCurr_ID,ca.Title_Name,tv.TextCode, tv.TextVal,ca.SubTitle from activity_entry ae,CoCurr_Activitie ca,textvaltable tv where ae.CoCurr_ID=ca.CoCurr_ID and ae.Batch_Year=ca.Batch_Year and ae.Degree_Code=ca.Degree_Code and college_code='" + college_code + "' and ae.Batch_Year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and ae.Degree_Code='" + Convert.ToString(ddlDept.SelectedValue) + "' and tv.TextCode=ae.ActivityTextVal  and ae.ActivityTextVal in (select ActivityTextVal from CoCurrActivitie_Det where Degree_Code='" + Convert.ToString(ddlDept.SelectedValue) + "' and Batch_Year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and CoCurrActivitie_Det.term ='" + Convert.ToString(ddlSem.SelectedValue) + "') and SubTitle='2a' and ae.term='" + Convert.ToString(ddlSem.SelectedValue) + "' order by TextCode;select distinct tv.TextCode,tv.TextVal,ag.Grade,ag.description,cd.Mark,Roll_No from activity_gd ag ,CoCurrActivitie_Det cd,textvaltable tv where ag.ActivityTextVal=cd.ActivityTextVal and tv.TextCode=ag.ActivityTextVal and tv.TextCode=cd.ActivityTextVal and cd.Mark between frompoint and topoint and ag.collegecode=tv.college_code  and cd.Degree_Code=ag.Degree_Code and cd.term=ag.term and cd.Batch_Year=ag.batch_year and cd.term=ag.term and ag.collegecode='" + college_code + "' and cd.Batch_Year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and cd.Degree_Code='" + Convert.ToString(ddlDept.SelectedValue) + "'  and cd.term='" + Convert.ToString(ddlSem.SelectedValue) + "' order by tv.TextCode;";
                    dsMarksDetails = d2.select_method_wo_parameter(q, "Text");
                    PdfTablePage tblpage;
                    PdfTable tblpayprocess;
                    PdfTable tablestanes11;
                    if (studgradeds.Tables[0].Rows.Count > 0)
                    {
                        for (int roll = 0; roll < studgradeds.Tables[0].Rows.Count; roll++)
                        {
                            string rcrollno = "";
                            rcrollno = Convert.ToString(studgradeds.Tables[0].Rows[roll][1]);

                            string Roll_No = rcrollno;

                            string sec = studgradeds.Tables[0].Rows[roll]["Sections"].ToString();
                            rollnos = rcrollno;

                            DataView dv = new DataView();
                            if (dsMarksDetails.Tables[0].Rows.Count > 0)
                            {
                                dsMarksDetails.Tables[0].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                                dvStud = dsMarksDetails.Tables[0].DefaultView;
                                dtStud = dvStud.ToTable();
                            }
                            if (dsMarksDetails.Tables[4].Rows.Count > 0)
                            {
                                dsMarksDetails.Tables[4].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                                dtAtt = dsMarksDetails.Tables[4].DefaultView.ToTable();
                            }
                            if (dsMarksDetails.Tables[5].Rows.Count > 0)
                            {
                                dsMarksDetails.Tables[5].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                                dtRmrk = dsMarksDetails.Tables[5].DefaultView.ToTable();
                            }
                            if (dsMarksDetails.Tables[7].Rows.Count > 0)
                            {
                                dsMarksDetails.Tables[7].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                                dtActivity = dsMarksDetails.Tables[7].DefaultView.ToTable();
                            }
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                                dv = ds.Tables[1].DefaultView;
                            }
                            int count4 = 0;
                            count4 = dv.Count;
                            if (ds.Tables[0].Rows.Count > 0 && count4 > 0 && dtStud.Rows.Count > 0)
                            {
                                string serialno = dv[0]["serialno"].ToString();
                                string currentsem = Convert.ToString(dv[0]["Current_Semester"]);
                                string batchyear = Convert.ToString(dv[0]["Batch_Year"]);
                                string degreecode = Convert.ToString(dv[0]["degree_code"]);
                                stdappno = Convert.ToString(dv[0]["App_No"]);
                                string admdate = Convert.ToString(dv[0]["adm_date"]);
                                status = true;
                                string dob = Convert.ToString(dv[0]["dob"]);
                                string[] dobspit = dob.Split('/');
                                string addressline1 = Convert.ToString(dv[0]["parent_addressP"]);
                                string addressline2 = "";

                                if (Convert.ToString(dv[0]["Cityp"]).Trim() != "" && Convert.ToString(dv[0]["Streetp"]).Trim() != "")
                                {
                                    addressline2 = Convert.ToString(dv[0]["Streetp"]) + " , " + Convert.ToString(dv[0]["Cityp"]);
                                }
                                else if (Convert.ToString(dv[0]["Streetp"]).Trim() != "" && Convert.ToString(dv[0]["Cityp"]).Trim() == "")
                                {
                                    addressline2 = Convert.ToString(dv[0]["Streetp"]);
                                }
                                else if (Convert.ToString(dv[0]["Streetp"]).Trim() == "" && Convert.ToString(dv[0]["Cityp"]).Trim() != "")
                                {
                                    addressline2 = Convert.ToString(dv[0]["Cityp"]);
                                }

                                string parentstatt = dv[0]["parent_statep"].ToString();
                                parentstatt = d2.GetFunction("select distinct textval from applyn  a,textvaltable t where a.parent_statep = t.TextCode and t.TextCode=" + parentstatt + "");

                                if (parentstatt.Trim() == "" || parentstatt.Trim() == "0")
                                {
                                    parentstatt = "";
                                }

                                string addressline3 = dv[0]["Districtp"].ToString() + " , " + parentstatt + " , " + Convert.ToString(dv[0]["parent_pincodep"]);

                                if (dv[0]["Districtp"].ToString().Trim() != "" && parentstatt != "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() != "")
                                {
                                    addressline3 = dv[0]["Districtp"].ToString() + " , " + parentstatt + " , " + Convert.ToString(dv[0]["parent_pincodep"]);
                                }
                                else if (dv[0]["Districtp"].ToString().Trim() != "" && parentstatt != "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() == "")
                                {
                                    addressline3 = dv[0]["Districtp"].ToString() + " , " + parentstatt;
                                }
                                else if (dv[0]["Districtp"].ToString().Trim() != "" && parentstatt == "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() != "")
                                {
                                    addressline3 = dv[0]["Districtp"].ToString() + " , " + Convert.ToString(dv[0]["parent_pincodep"]);
                                }
                                else if (dv[0]["Districtp"].ToString().Trim() == "" && parentstatt != "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() != "")
                                {
                                    addressline3 = parentstatt + " , " + Convert.ToString(dv[0]["parent_pincodep"]);
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
                                    addressline3 = "";
                                }
                                string parentcountry = dv[0]["countryp"].ToString();
                                int num = 0;
                                if (int.TryParse(parentcountry, out num))
                                {
                                    parentcountry = d2.GetFunction("select distinct textval from textvaltable t where  t.TextCode='" + parentcountry + "'");
                                }

                                if (parentcountry.Trim() == "" || parentcountry.Trim() == "0" || parentcountry == null)
                                {
                                    parentcountry = "";
                                }
                                if (addressline3.Trim() != "" && parentcountry.Trim() != "")
                                {
                                    addressline3 = addressline3 + " , " + parentcountry;
                                }
                                string mobileno = dv[0]["parentF_Mobile"].ToString() + " / " + dv[0]["parentM_Mobile"].ToString();

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
                                    mobileno = "";
                                }
                                //  addressline2 = addressline1 + ", " + addressline2 + " - " +  Convert.ToString(dv[0]["parent_pincodep"]);
                                string email = "";
                                if (dv[0]["ParentIdP"].ToString().Trim() != "" && dv[0]["emailM"].ToString().Trim() != "")
                                {
                                    email = dv[0]["ParentIdP"].ToString() + " / " + dv[0]["emailM"].ToString();
                                }
                                else if (dv[0]["parentF_Mobile"].ToString().Trim() != "" && dv[0]["emailM"].ToString().Trim() == "")
                                {
                                    email = dv[0]["ParentIdP"].ToString();
                                }
                                else if (dv[0]["ParentIdP"].ToString().Trim() == "" && dv[0]["emailM"].ToString().Trim() != "")
                                {
                                    email = dv[0]["emailM"].ToString();
                                }
                                else if (dv[0]["ParentIdP"].ToString().Trim() == "" && dv[0]["emailM"].ToString().Trim() == "")
                                {
                                    email = "";
                                }
                                mypdfpage = mydoc.NewPage();
                                PdfTextArea pdf1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 30, 10, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(ds.Tables[0].Rows[0][0]) + "");
                                PdfTextArea pdf11 = new PdfTextArea(f16, System.Drawing.Color.Black, new PdfArea(mydoc, 30, 30, 595, 50), System.Drawing.ContentAlignment.TopCenter, " " + ds.Tables[0].Rows[0]["address2"].ToString().ToUpper() + " " + ds.Tables[0].Rows[0]["district"].ToString().ToUpper() + " - " + ds.Tables[0].Rows[0]["pincode"].ToString().ToUpper() + "");
                                PdfArea pa1 = new PdfArea(mydoc, 30, 5, 560, 834);
                                PdfArea pahealth = new PdfArea(mydoc, 2, 765, 591, 70);
                                PdfRectangle pr1 = new PdfRectangle(mydoc, pa1, Color.Black);
                                PdfArea pa2 = new PdfArea(mydoc, 189, 175, 224, 40);

                                mypdfpage.Add(pdf1);
                                mypdfpage.Add(pdf11);
                                mypdfpage.Add(pr1);
                                string sqlschool = "select value from Master_Settings where settings='Academic year'";
                                dschool = d2.select_method_wo_parameter(sqlschool, "Text");
                                string splitvalue = Convert.ToString(dschool.Tables[0].Rows[0]["value"]);
                                string[] dsplit = splitvalue.Split(',');

                                string fvalue = Convert.ToString(dsplit[0]);
                                string lvalue = Convert.ToString(dsplit[1]);
                                string acdmic_date = fvalue + "-" + lvalue;
                                PdfTextArea pdf_acadamicyear = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 50, 595, 50), System.Drawing.ContentAlignment.TopCenter, "STUDENT CUMULATIVE RECORD");
                                mypdfpage.Add(pdf_acadamicyear);

                                pdf_acadamicyear = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 65, 595, 50), System.Drawing.ContentAlignment.TopCenter, "Academic Year: " + acdmic_date + "");
                                mypdfpage.Add(pdf_acadamicyear);

                                //pdf_acadamicyear = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 80, 595, 50), System.Drawing.ContentAlignment.TopCenter, test + " - " + ((dtsub.Rows.Count > 0) ? string.Format("{0:MMM-yyyy}", dt) : exam_held).ToString());
                                //mypdfpage.Add(pdf_acadamicyear);

                                pdf_acadamicyear = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 80, 595, 50), System.Drawing.ContentAlignment.TopCenter, "-----------------------------------------------------------------------------------------------------------------------------------");
                                mypdfpage.Add(pdf_acadamicyear);
                                coltop = 90;
                                tablestanes11 = mydoc.NewTable(f11, 2, 12, 3);
                                tablestanes11.VisibleHeaders = false;
                                tablestanes11.SetBorders(Color.Black, 1, BorderType.None);
                                tablestanes11.SetColumnsWidth(new int[] { 35, 7, 170, 80, 7, 50, 60, 7, 70, 50, 7, 44 });
                                tablestanes11.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(0, 0).SetContent("Name");
                                tablestanes11.Cell(0, 1).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(0, 1).SetContent(":");
                                tablestanes11.Cell(0, 2).SetContentAlignment(ContentAlignment.TopLeft);
                                studname = Convert.ToString(dv[0]["stud_name"]);
                                tablestanes11.Cell(0, 2).SetContent(Convert.ToString(dv[0]["stud_name"]));
                                tablestanes11.Cell(0, 2).SetFont(f4);

                                tablestanes11.Cell(0, 3).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(0, 3).SetContent("Admission No");
                                tablestanes11.Cell(0, 4).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(0, 4).SetContent(":");
                                tablestanes11.Cell(0, 5).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(0, 5).SetContent(Convert.ToString(dv[0]["roll_admit"]));
                                tablestanes11.Cell(0, 5).SetFont(f4);

                                tablestanes11.Cell(0, 6).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(0, 6).SetContent("Std & Sec");
                                tablestanes11.Cell(0, 7).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(0, 7).SetContent(":");
                                tablestanes11.Cell(0, 8).SetContentAlignment(ContentAlignment.TopLeft);
                                string stdsec = "";

                                if (Convert.ToString(dv[0]["Sections"]).Trim() == "")
                                {
                                    stdsec = " " + ddlDept.SelectedItem.Text.ToString();
                                }
                                else
                                {
                                    stdsec = " " + ddlDept.SelectedItem.Text.ToString() + " - " + Convert.ToString(dv[0]["Sections"]) + "";
                                }

                                tablestanes11.Cell(0, 8).SetContent(stdsec);
                                tablestanes11.Cell(0, 8).SetFont(f4);

                                tablestanes11.Cell(0, 9).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(0, 9).SetContent("Roll No");
                                tablestanes11.Cell(0, 10).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(0, 10).SetContent(":");
                                tablestanes11.Cell(0, 11).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(0, 11).SetContent(serialno);
                                tablestanes11.Cell(0, 11).SetFont(f4);

                                tablestanes11.Cell(1, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(1, 0).SetContent("D.O.B");
                                tablestanes11.Cell(1, 1).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(1, 1).SetContent(":");
                                tablestanes11.Cell(1, 2).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(1, 2).SetContent(Convert.ToString(dv[0]["dob"]));
                                tablestanes11.Cell(1, 2).SetFont(f4);

                                tablestanes11.Cell(1, 3).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(1, 3).SetContent("Blood Group");
                                tablestanes11.Cell(1, 4).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(1, 4).SetContent(":");
                                tablestanes11.Cell(1, 5).SetContentAlignment(ContentAlignment.TopLeft);
                                string bldgrp = dv[0]["bldgrp"].ToString();
                                bldgrp = d2.GetFunction("select distinct textval from applyn  a,textvaltable t where a.bldgrp = t.TextCode and t.TextCode=" + dv[0]["bldgrp"].ToString() + "");
                                if (bldgrp.Trim() == "0" || bldgrp.Trim() == "")
                                {
                                    bldgrp = "";
                                }
                                tablestanes11.Cell(1, 5).SetContent(bldgrp);
                                tablestanes11.Cell(1, 5).SetFont(f4);

                                tablestanes11.Cell(1, 6).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(1, 6).SetContent("House");
                                tablestanes11.Cell(1, 7).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(1, 7).SetContent(":");
                                tablestanes11.Cell(1, 8).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(1, 8).SetContent(dv[0]["studhouse"].ToString());
                                tablestanes11.Cell(1, 8).SetFont(f4);

                                tblpage = tablestanes11.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, coltop, 530, 250));
                                mypdfpage.Add(tblpage);

                                PdfTable tablestanes1 = mydoc.NewTable(f11, 2, 6, 3);
                                tablestanes1.VisibleHeaders = false;
                                tablestanes1.SetBorders(Color.Black, 1, BorderType.None);
                                tablestanes1.SetColumnsWidth(new int[] { 139, 7, 171, 85, 7, 210 });
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
                                tablestanes1.Cell(1, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes1.Cell(1, 0).SetContent("Contact Number");
                                tablestanes1.Cell(1, 1).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes1.Cell(1, 1).SetContent(":");
                                tablestanes1.Cell(1, 2).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes1.Cell(1, 2).SetContent(mobileno);
                                tablestanes1.Cell(1, 2).SetFont(f4);

                                tablestanes1.Cell(1, 3).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes1.Cell(1, 3).SetContent("Email - ID");
                                tablestanes1.Cell(1, 4).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes1.Cell(1, 4).SetContent(":");
                                tablestanes1.Cell(1, 5).SetContentAlignment(ContentAlignment.TopLeft);
                                //if (email != "")
                                //{
                                //    tablestanes1.Cell(1, 5).SetContent(email);
                                //}
                                //else
                                tablestanes1.Cell(1, 5).SetContent("____________________________");
                                tablestanes1.Cell(1, 5).SetFont(f4);

                                getheigh = tblpage.Area.Height;
                                getheigh = Math.Round(getheigh, 0);
                                coltop = coltop + Convert.ToInt32(getheigh) + 10;
                                tblpage = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, coltop, 580, 250));
                                mypdfpage.Add(tblpage);

                                tablestanes1 = mydoc.NewTable(f11, 1, 3, 3);
                                tablestanes1.VisibleHeaders = false;
                                tablestanes1.SetBorders(Color.Black, 1, BorderType.None);
                                tablestanes1.SetColumnsWidth(new int[] { 123, 7, 325 });
                                tablestanes1.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes1.Cell(0, 0).SetContent("Residential Address");
                                tablestanes1.Cell(0, 1).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes1.Cell(0, 1).SetContent(":");
                                tablestanes1.Cell(0, 2).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes1.Cell(0, 2).SetContent(addressline1);
                                tablestanes1.Cell(0, 2).SetFont(f4);
                                getheigh = tblpage.Area.Height;
                                getheigh = Math.Round(getheigh, 0);
                                coltop = coltop + Convert.ToInt32(getheigh) + 8;
                                tblpage = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, coltop, 480, 250));
                                mypdfpage.Add(tblpage);

                                tablestanes1 = mydoc.NewTable(f11, 2, 3, 3);

                                tablestanes1.VisibleHeaders = false;
                                tablestanes1.SetBorders(Color.Black, 1, BorderType.None);
                                tablestanes1.SetColumnsWidth(new int[] { 70, 7, 420 });
                                tablestanes1.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes1.Cell(0, 0).SetContent("Health Status");
                                tablestanes1.Cell(0, 1).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes1.Cell(0, 1).SetContent(":");
                                tablestanes1.Cell(0, 2).SetContentAlignment(ContentAlignment.TopLeft);
                                //tablestanes1.Cell(0, 2).SetContent(dv[0]["Strenghts"].ToString() + "  ");
                                tablestanes1.Cell(0, 2).SetContent(" _________________________________________________________________________________________");
                                tablestanes1.Cell(0, 2).SetFont(f4);
                                getheigh = tblpage.Area.Height;
                                getheigh = Math.Round(getheigh, 0);
                                coltop = coltop + Convert.ToInt32(getheigh) + 8;
                                tblpage = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, coltop, 580, 250));
                                mypdfpage.Add(tblpage);

                                tablestanes11 = mydoc.NewTable(f11, 2, 9, 3);
                                tablestanes11.VisibleHeaders = false;
                                tablestanes11.SetBorders(Color.Black, 1, BorderType.None);

                                tablestanes11.SetColumnsWidth(new int[] { 67, 7, 90, 60, 7, 90, 60, 7, 90 });
                                tablestanes11.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(0, 0).SetContent("Height");
                                tablestanes11.Cell(0, 1).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(0, 1).SetContent(":");
                                tablestanes11.Cell(0, 2).SetContentAlignment(ContentAlignment.BottomLeft);

                                if (dv[0]["StudHeight"].ToString().Trim() != "" || dv[0]["StudHeight"].ToString().Trim() == null)
                                {
                                    tablestanes11.Cell(0, 2).SetContent(dv[0]["StudHeight"].ToString() + " cms");
                                }
                                //tablestanes1.Cell(0, 2).SetContent(" ________");
                                tablestanes11.Cell(0, 2).SetFont(f4);
                                tablestanes11.Cell(0, 3).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(0, 3).SetContent("Weight");
                                tablestanes11.Cell(0, 4).SetContentAlignment(ContentAlignment.TopLeft);
                                tablestanes11.Cell(0, 4).SetContent(":");
                                tablestanes11.Cell(0, 5).SetContentAlignment(ContentAlignment.BottomLeft);
                                if (dv[0]["StudWeight"].ToString().Trim() != "" || dv[0]["StudWeight"].ToString().Trim() == null)
                                {
                                    tablestanes11.Cell(0, 5).SetContent(dv[0]["StudWeight"].ToString() + " kgs");
                                }
                                tablestanes11.Cell(0, 5).SetFont(f4);
                                //tablestanes11.Cell(0, 6).SetContentAlignment(ContentAlignment.TopLeft);
                                //tablestanes11.Cell(0, 6).SetContent("Rank");
                                //tablestanes11.Cell(0, 7).SetContentAlignment(ContentAlignment.TopLeft);
                                //tablestanes11.Cell(0, 7).SetContent(":");
                                //tablestanes11.Cell(0, 8).SetContentAlignment(ContentAlignment.BottomLeft);
                                //tablestanes11.Cell(0, 8).SetFont(f4);

                                getheigh = tblpage.Area.Height;
                                getheigh = Math.Round(getheigh, 0);
                                coltop = coltop + Convert.ToInt32(getheigh) + 8;
                                tblpage = tablestanes11.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, coltop, 580, 250));
                                mypdfpage.Add(tblpage);
                                pdf_acadamicyear = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, coltop + 10, 595, 50), System.Drawing.ContentAlignment.TopCenter, "-----------------------------------------------------------------------------------------------------------------------------------");
                                mypdfpage.Add(pdf_acadamicyear);

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

                                        }
                                        else
                                        {
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                    }
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 35, 17, 490);
                                }
                                int x = 0;
                                int rowcount = 0;
                                if (dsMarksDetails.Tables[1].Rows.Count > 0)
                                {
                                    rowcount = dsMarksDetails.Tables[1].Rows.Count;
                                }
                                tablestanes11 = mydoc.NewTable(f11, rowcount + 12, testcount + 1, 3);
                                tablestanes11.VisibleHeaders = false;
                                tablestanes11.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                tablestanes11.Cell(0, 0).SetContent("SUBJECTS");
                                foreach (PdfCell pc in tablestanes11.CellRange(0, 0, 0, 0).Cells)
                                {
                                    pc.RowSpan = 2;
                                }
                                tablestanes11.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablestanes11.Cell(0, 0).SetFont(f6);
                                tablestanes11.Cell(0, 1).SetContent("Examination");
                                tablestanes11.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablestanes11.Cell(0, 1).SetFont(f6);
                                foreach (PdfCell pc in tablestanes11.CellRange(0, 1, 0, 1).Cells)
                                {
                                    pc.ColSpan = testcount;
                                }
                                string[] testname = test1.Split(',');
                                string[] testno = tstnum.Split(',');
                                DataTable dtTestMarKs = new DataTable();
                                DataView dvTestMarks = new DataView();
                                for (int c = 0; c < testcount + 1; c++)
                                {
                                    if (c == 0)
                                    {
                                        int rs = 0;
                                        tablestanes11.Columns[c].SetWidth(200);
                                        if (rowcount > 0)
                                        {
                                            for (int r = 0; r < rowcount; r++)
                                            {
                                                tablestanes11.Cell(r + 2, c).SetContent(Convert.ToString(dsMarksDetails.Tables[1].Rows[r][1].ToString().Replace("-", " ")));
                                                tablestanes11.Cell(r + 2, c).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tablestanes11.Cell(r + 2, c).SetFont(f6);
                                            }
                                            rs = rowcount + 2;
                                        }
                                        else
                                        {
                                            rs = 2;
                                        }
                                        tablestanes11.Cell(rs, c).SetContent("Overall Grade");
                                        tablestanes11.Cell(rs, c).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tablestanes11.Cell(rs, c).SetFont(f6);
                                        tablestanes11.Cell(rs + 1, c).SetContent("Attendance");
                                        tablestanes11.Cell(rs + 1, c).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tablestanes11.Cell(rs + 1, c).SetFont(f6);
                                        tablestanes11.Cell(rs + 2, c).SetContent("Teacher's Signature");
                                        tablestanes11.Cell(rs + 2, c).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tablestanes11.Cell(rs + 2, c).SetFont(f6);
                                        tablestanes11.Cell(rs + 3, c).SetContent("AHM's Signature");
                                        tablestanes11.Cell(rs + 3, c).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tablestanes11.Cell(rs + 3, c).SetFont(f6);
                                        tablestanes11.Cell(rs + 4, c).SetContent("Principal");
                                        tablestanes11.Cell(rs + 4, c).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tablestanes11.Cell(rs + 4, c).SetFont(f6);
                                        tablestanes11.Cell(rs + 5, c).SetContent("Father's Signature");
                                        tablestanes11.Cell(rs + 5, c).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tablestanes11.Cell(rs + 5, c).SetFont(f6);
                                        tablestanes11.Cell(rs + 6, c).SetContent("Mother's Signature");
                                        tablestanes11.Cell(rs + 6, c).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tablestanes11.Cell(rs + 6, c).SetFont(f6);

                                        tablestanes11.Cell(rs + 7, c).SetContent("INDICATES GOOD  \t\t\t\t NEEDS TO IMPROVE");
                                        foreach (PdfCell pc in tablestanes11.CellRange(rs + 7, c, rs + 7, c).Cells)
                                        {
                                            pc.ColSpan = testcount + 1;
                                        }
                                        tablestanes11.Cell(rs + 7, c).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tablestanes11.Cell(rs + 7, c).SetFont(f6);
                                        tablestanes11.Cell(rs + 8, c).SetContent("The Grades Are As Follows");
                                        foreach (PdfCell pc in tablestanes11.CellRange(rs + 8, c, rs + 8, c).Cells)
                                        {
                                            pc.ColSpan = testcount + 1;
                                        }
                                        tablestanes11.Cell(rs + 8, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tablestanes11.Cell(rs + 8, c).SetFont(f6);
                                        tablestanes11.Cell(rs + 8, c).SetFont(new Font("Book Antiqua", 8, FontStyle.Bold));
                                        if (dsMarksDetails.Tables[3].Rows.Count > 0)
                                        {
                                            //foreach (PdfCell pc in tablestanes11.CellRange(rs + 9, c, rs + 9, c).Cells)
                                            //{
                                            //    pc.ColSpan = 2;
                                            //}
                                            string grades = "";
                                            DataRow dr = dtGrade.NewRow();
                                            //DataRow dr1=dtGrade.NewRow();
                                            dtGrade.Rows.Clear();
                                            dtGrade.Columns.Clear();
                                            dtGrade.Columns.Add("Grade1");
                                            dtGrade.Columns.Add("Marks1");
                                            dtGrade.Columns.Add("Credit1");
                                            dtGrade.Columns.Add("Grade2");
                                            dtGrade.Columns.Add("Marks2");
                                            dtGrade.Columns.Add("Credit2");
                                            for (int gc = 0; gc < dsMarksDetails.Tables[3].Rows.Count; gc++)
                                            {
                                                if (gc % 2 == 0)
                                                {
                                                    dtGrade.Rows.Add(dr);
                                                    dr["Grade1"] = dsMarksDetails.Tables[3].Rows[gc]["Mark_Grade"].ToString();
                                                    dr["Marks1"] = dsMarksDetails.Tables[3].Rows[gc]["Frange"].ToString() + " TO " + dsMarksDetails.Tables[3].Rows[gc]["Trange"].ToString();

                                                    dr["Credit1"] = dsMarksDetails.Tables[3].Rows[gc]["Credit_Points"].ToString();
                                                }
                                                else
                                                {
                                                    dr["Grade2"] = dsMarksDetails.Tables[3].Rows[gc]["Mark_Grade"].ToString();
                                                    dr["Marks2"] = dsMarksDetails.Tables[3].Rows[gc]["Frange"].ToString() + " TO " + dsMarksDetails.Tables[3].Rows[gc]["Trange"].ToString();
                                                    dr["Credit2"] = dsMarksDetails.Tables[3].Rows[gc]["Credit_Points"].ToString();
                                                    dr = dtGrade.NewRow();
                                                }
                                            }
                                            if (dtGrade.Rows.Count > 0)
                                            {
                                                grades = "\n";
                                                for (int r = 0; r < dtGrade.Rows.Count; r++)
                                                {
                                                    grades += dtGrade.Rows[r]["Grade1"].ToString() + "\t\t\t\t\t\t\t\t\t\t\t" + dtGrade.Rows[r]["Marks1"].ToString() + "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t" + dtGrade.Rows[r]["Grade2"].ToString() + "\t\t\t\t\t\t\t\t\t\t\t" + dtGrade.Rows[r]["Marks2"].ToString() + "\n";
                                                }
                                                grades += "\n";
                                            }
                                            tablestanes11.Cell(rs + 9, c).SetContent(grades);
                                            foreach (PdfCell pc in tablestanes11.CellRange(rs + 9, c, rs + 9, c).Cells)
                                            {
                                                pc.RowSpan = 1;
                                            }
                                            foreach (PdfCell pc in tablestanes11.CellRange(rs + 9, c, rs + 9, c).Cells)
                                            {
                                                pc.ColSpan = testcount + 1;
                                            }
                                            tablestanes11.Cell(rs + 9, c).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            tablestanes11.Cell(rs + 9, c).SetFont(f6);
                                            tablestanes11.Cell(rs + 9, c).SetCellPadding(1);
                                            tablestanes11.Cell(rs + 9, c).SetFont(new Font("Book Antiqua", 6, FontStyle.Bold));
                                        }
                                    }
                                    else
                                    {
                                        int rs = 0;
                                        double TestTotal = 0;
                                        double TestAvg = 0;
                                        int subcount = 0;
                                        tablestanes11.Columns[c].SetWidth(100);
                                        tablestanes11.Cell(1, c).SetContent(testname[c - 1]);
                                        tablestanes11.Cell(1, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tablestanes11.Cell(1, c).SetFont(f6);
                                        if (rowcount > 0)
                                        {
                                            if (c < testcount + 1)
                                            {
                                                dtStud.DefaultView.RowFilter = "Criteria_no='" + testno[c - 1] + "'";
                                                dvTestMarks = dtStud.DefaultView;
                                                dtTestMarKs = dvTestMarks.ToTable();
                                                dsMarksDetails.Tables[2].DefaultView.RowFilter = "Criteria_no='" + testno[c - 1] + "'";
                                                dtSubcount = dsMarksDetails.Tables[2].DefaultView.ToTable(true, "subject_no", "subject_name", "Criteria_no", "max_mark", "min_mark");
                                                subcount = dtSubcount.Rows.Count;
                                                if (dtTestMarKs.Rows.Count > 0)
                                                {
                                                    DataView view = new DataView();
                                                    TestTotal = 0;
                                                    for (int r = 0; r < rowcount; r++)
                                                    {
                                                        //dsMarksDetails.Tables[1].Rows[r]["subject_no"].ToString().Trim();
                                                        dtTestMarKs.DefaultView.RowFilter = "subject_no='" + dsMarksDetails.Tables[1].Rows[r]["subject_no"].ToString() + "'";
                                                        view = dtTestMarKs.DefaultView;
                                                        if (view.Count > 0)
                                                        {
                                                            int m = 0;
                                                            string maxmark = view[0]["max_mark"].ToString();
                                                            string minmark = view[0]["min_mark"].ToString();
                                                            string marks = view[0]["marks_obtained"].ToString();
                                                            int.TryParse(marks, out m);
                                                            if (m < 0)
                                                            {
                                                                marks = findresult(marks);
                                                                tablestanes11.Cell(r + 2, c).SetContent(marks);
                                                            }
                                                            else
                                                            {
                                                                string grade = "";
                                                                ConvertedMark("100", ref maxmark, ref marks, ref minmark);
                                                                int.TryParse(marks, out m);
                                                                //findgrade(dsMarksDetails.Tables[3], marks,ref grade);
                                                                TestTotal += m;
                                                                if (findgrade(dsMarksDetails.Tables[3], marks, ref grade))
                                                                {
                                                                    tablestanes11.Cell(r + 2, c).SetContent(grade);
                                                                }
                                                                else
                                                                {
                                                                    tablestanes11.Cell(r + 2, c).SetContent(marks);
                                                                }
                                                            }
                                                            tablestanes11.Cell(r + 2, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            tablestanes11.Cell(r + 2, c).SetFont(f6);
                                                        }
                                                    }
                                                }
                                            }
                                            rs = rowcount + 2;
                                        }
                                        else
                                        {
                                            rs = 2;
                                        }
                                        string overallgrade = "";
                                        if (subcount != 0)
                                        {
                                            double avg = Math.Round((TestTotal / subcount), 2);
                                            if (findgrade(dsMarksDetails.Tables[3], Convert.ToString(avg), ref overallgrade))
                                            {
                                                tablestanes11.Cell(rs, c).SetContent(overallgrade);
                                            }
                                            else
                                            {

                                                tablestanes11.Cell(rs, c).SetContent(avg);
                                            }
                                        }
                                        tablestanes11.Cell(rs, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tablestanes11.Cell(rs, c).SetFont(f6);

                                    }
                                }
                                getheigh = tblpage.Area.Height;
                                getheigh = Math.Round(getheigh, 0);
                                coltop = coltop + Convert.ToInt32(getheigh) + 8;
                                x = 19 + moveleftvalue;
                                tblpage = tablestanes11.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, x, coltop, 250, 700));
                                mypdfpage.Add(tblpage);

                                rowcount = 0;
                                if (dsMarksDetails.Tables[6].Rows.Count > 0)
                                {
                                    rowcount = dsMarksDetails.Tables[6].Rows.Count;
                                }
                                //tablestanes11.ImportDataTable(dt);
                                tablestanes1 = mydoc.NewTable(f11, rowcount + (testcount * 3) + 5, testcount + 1, 3);
                                tablestanes1.VisibleHeaders = false;
                                tablestanes1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                tablestanes1.Cell(0, 0).SetContent("PERSONALITY DEVELOPMENT");

                                tablestanes1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tablestanes1.Cell(0, 0).SetFont(f6);

                                testname = test1.Split(',');
                                testno = tstnum.Split(',');
                                int row = 0;
                                for (int c = 0; c < testcount + 1; c++)
                                {
                                    if (c == 0)
                                    {
                                        int rs = 0;
                                        tablestanes1.Columns[c].SetWidth(280);
                                        if (rowcount > 0)
                                        {
                                            for (int r = 0; r < rowcount; r++)
                                            {
                                                tablestanes1.Cell(r + 1, c).SetContent(Convert.ToString(dsMarksDetails.Tables[6].Rows[r]["TextVal"].ToString().Replace("-", " ")));
                                                tablestanes1.Cell(r + 1, c).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tablestanes1.Cell(r + 1, c).SetFont(f6);
                                            }
                                            rs = rowcount + 1;
                                        }
                                        else
                                        {
                                            rs = 1;
                                        }
                                        tablestanes1.Cell(rs, c).SetContent("TEACHER'S REMARKS");
                                        foreach (PdfCell pc in tablestanes1.CellRange(rs, c, rs, c).Cells)
                                        {
                                            pc.ColSpan = testcount + 1;
                                        }
                                        tablestanes1.Cell(rs, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tablestanes1.Cell(rs, c).SetFont(f6);

                                        for (int rmk = 0; rmk < testcount; rmk++)
                                        {
                                            rs++;
                                            tablestanes1.Cell(rs, c).SetContent(testname[rmk] + " : ");
                                            foreach (PdfCell pc in tablestanes1.CellRange(rs, c, rs, c).Cells)
                                            {
                                                pc.ColSpan = testcount + 1;
                                            }
                                            tablestanes1.Cell(rs, c).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            tablestanes1.Cell(rs, c).SetFont(f6);

                                            tablestanes1.Cell(rs + 1, c).SetContent("\n");
                                            tablestanes1.Cell(rs + 1, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tablestanes1.Cell(rs + 1, c).SetFont(f6);
                                            //tablestanes1.Cell(rs + 1, c).SetCellPadding(5);
                                            foreach (PdfCell pc in tablestanes1.CellRange(rs + 1, c, rs + 1, c).Cells)
                                            {
                                                pc.ColSpan = testcount + 1;
                                            }

                                            tablestanes1.Cell(rs + 2, c).SetContent("\n");
                                            tablestanes1.Cell(rs + 2, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tablestanes1.Cell(rs + 2, c).SetFont(f6);
                                            //tablestanes1.Cell(rs + 2, c).SetCellPadding(5);
                                            foreach (PdfCell pc in tablestanes1.CellRange(rs + 2, c, rs + 2, c).Cells)
                                            {
                                                pc.ColSpan = testcount + 1;
                                            }
                                            rs += 2;
                                        }
                                        rs++;
                                        tablestanes1.Cell(rs, c).SetContent("PROMOTION DETAILS");
                                        tablestanes1.Cell(rs, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tablestanes1.Cell(rs, c).SetFont(f6);
                                        foreach (PdfCell pc in tablestanes1.CellRange(rs, c, rs, c).Cells)
                                        {
                                            pc.ColSpan = testcount + 1;
                                        }
                                        tablestanes1.Cell(rs + 1, c).SetContent("Promoted To:\n\n-------------------");
                                        tablestanes1.Cell(rs + 1, c).SetContentAlignment(ContentAlignment.TopLeft);
                                        tablestanes1.Cell(rs + 1, c).SetFont(f6);
                                        tablestanes1.Cell(rs + 2, c).SetContent("Signature of the AHM:\n\n");
                                        tablestanes1.Cell(rs + 2, c).SetContentAlignment(ContentAlignment.TopLeft);
                                        tablestanes1.Cell(rs + 2, c).SetFont(f6);
                                        //rs += 2;
                                        //rowcount = rs;
                                        //for (int end = 0; end < testcount; end++)
                                        //{

                                        //}
                                        tablestanes1.Cell(rs + 1, c + 1).SetContent("Signature of the Teacher:\n\n");
                                        tablestanes1.Cell(rs + 1, c + 1).SetContentAlignment(ContentAlignment.TopLeft);
                                        tablestanes1.Cell(rs + 1, c + 1).SetFont(f6);
                                        foreach (PdfCell pc in tablestanes1.CellRange(rs + 1, c + 1, rs + 1, c + 1).Cells)
                                        {
                                            pc.ColSpan = testcount;
                                        }

                                        tablestanes1.Cell(rs + 2, c + 1).SetContent("Signature of the Principal:\n\n\n");
                                        tablestanes1.Cell(rs + 2, c + 1).SetContentAlignment(ContentAlignment.TopLeft);
                                        tablestanes1.Cell(rs + 2, c + 1).SetFont(f6);
                                        foreach (PdfCell pc in tablestanes1.CellRange(rs + 2, c + 1, rs + 2, c + 1).Cells)
                                        {
                                            pc.ColSpan = testcount;
                                        }
                                    }
                                    else
                                    {
                                        int rs = 0;
                                        double TestTotal = 0;
                                        double TestAvg = 0;
                                        int subcount = 0;
                                        tablestanes1.Columns[c].SetWidth(100);
                                        tablestanes1.Cell(0, c).SetContent(testname[c - 1]);
                                        tablestanes1.Cell(0, c).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tablestanes1.Cell(0, c).SetFont(f6);
                                        if (rowcount > 0)
                                        {

                                        }
                                    }
                                }

                                //tablestanes1.ImportDataTable(dt);


                                double width = tblpage.Area.Width;
                                width = Math.Round(width, 0);
                                x += Convert.ToInt32(width) + 20;
                                tblpage = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, x, coltop, 250, 700));
                                mypdfpage.Add(tblpage);

                                mypdfpage.SaveToDocument();
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    lblErrSearch.Text = "No Test Were Found";
                    lblErrSearch.Visible = true;
                    return;
                }
            }
            if (status == true)
            {
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = Convert.ToString("Reportcard_Nursery_LKG_UKG" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmss")).Trim().Replace(" ", "").Trim() + ".pdf";
                    mydoc.SaveToFile(szPath + szFile);
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion

    #region Reusable Methods

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
            //result = true;
        }
        else
        {
            Grade = obtainedmarks;
            result = false;
        }
        return result;
    }

    public void ChangeHeaderName(bool isschool)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = "";
            lblCollege.Text = ((!isschool) ? "College" : "School");
            lblBatch.Text = ((!isschool) ? "Batch" : "Year");
            lblDegree.Text = ((!isschool) ? "Degree" : "School Type");
            lblDept.Text = ((!isschool) ? "Department" : "Standard");
            lblSem.Text = ((!isschool) ? "Semester" : "Term");
            lblsec.Text = ((!isschool) ? "Section" : "Section");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public string ToRoman(string part)
    {
        string roman = "";
        try
        {
            switch (part)
            {
                case "1":
                    roman = "I";
                    break;

                case "2":
                    roman = "II";
                    break;
                case "3":
                    roman = "III";
                    break;
                case "4":
                    roman = "IV";
                    break;
                case "5":
                    roman = "V";
                    break;
                case "6":
                    roman = "VI";
                    break;
                case "7":
                    roman = "VII";
                    break;
                case "8":
                    roman = "VIII";
                    break;
                case "9":
                    roman = "IX";
                    break;
                case "10":
                    roman = "X";
                    break;
                case "11":
                    roman = "XI";
                    break;
                case "12":
                    roman = "XII";
                    break;
            }
        }
        catch (Exception ex)
        {

        }
        return roman;
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
        double Mark, max;
        bool r = double.TryParse(obtainedMark, out Mark);
        bool maxflag = double.TryParse(txtConvertTo, out max);
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
                //if (txtConvertTo.Text == "50")
                //{
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
                    //obtainedMark = obtainedMark;
                    //minMark = minMark;
                }
            }
            minMark = min.ToString();
            maxMark = txtConvertTo;
        }
    }

    #endregion Reusable Methods

}