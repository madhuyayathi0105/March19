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
using System.Globalization;
using System.Text;
using System.Configuration;

#endregion Namespace Declaration

public partial class ReportCardICSE_IX_To_X : System.Web.UI.Page
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
                    ICSEReportcardVI_VIII(rollnos.Trim().Trim(','));
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

    public void ICSEReportcardVI_VIII(string rollno)
    {

        int secwise_stud_count = 0;
        int divwise_stud_count = 0;
        string batch = "";
        string degreecode1 = "";
        string semester = "";
        string sections = "";
        string exam_held = "";
        string criteria_no = "";
        string test = "";
        int checkpoint = 0;
        int coltop = 5;
        string multiple = "";
        DataTable dtsub = new DataTable();
        DataSet dschool = new DataSet();
        DataSet ds = new DataSet();
        DAccess2 da = new DAccess2();
        DataSet dscriteria = new DataSet();
        bool status = false;
        DataSet dsexammark = new DataSet();
        DataSet dsRmark = new DataSet();
        DataSet dset = new DataSet();
        DataSet dsparts = new DataSet();
        DataSet dsSubParts = new DataSet();
        DataView view = new DataView();
        DataTable dtsub1 = new DataTable();
        DataTable dtsub2 = new DataTable();
        DataTable dtAtt = new DataTable();
        DataTable dtRmrk = new DataTable();
        DataTable dtSubRank = new DataTable();
        DataTable dtSecRank = new DataTable();
        DataTable strqueryytablefil = new DataTable();
        DataTable dtTest = new DataTable();
        DataTable dtTest1 = new DataTable();
        DataTable dtTest2 = new DataTable();
        DataTable dtparts = new DataTable();
        DataTable dtparts1 = new DataTable();
        DataTable dtparts2 = new DataTable();
        DataTable dtparts3 = new DataTable();
        batch = Convert.ToString(ddlbatch.SelectedValue);
        degreecode1 = Convert.ToString(ddlDept.SelectedValue);
        sections = Convert.ToString(ddlsec.SelectedValue);
        semester = Convert.ToString(ddlSem.SelectedValue);
        int totsecrank = 0, totdivrank = 0;
        double totdivhigh = 0, totsechigh = 0;
        double totdivavg = 0.0, totsecavg = 0.0;
        StringBuilder strErr = new StringBuilder();
        string studname = "";
        string newtest = "";
        string newtest1 = "";
        string testname1 = "";
        string testname2 = "";
        string parttitle1a = "";
        Boolean flag = true;
        ArrayList arrcourrid = new ArrayList();
        ArrayList arrsubparts = new ArrayList();
        ArrayList partcolumnnames = new ArrayList();
        Font fntisceheader = new Font("OldEnglish", 16, FontStyle.Bold);
        Font Fontbold = new Font("Times New Roman", 12, FontStyle.Bold);
        Font f11 = new Font("Times New Roman", 11, FontStyle.Bold);
        Font f8 = new Font("Times New Roman", 10, FontStyle.Bold);
        Font fr8 = new Font("Times New Roman", 10, FontStyle.Regular);
        Font f4 = new Font("Times New Roman", 10, FontStyle.Regular);
        Font f5 = new Font("Times New Roman", 11, FontStyle.Regular);
        Font Fontsmall = new Font("Times New Roman", 12, FontStyle.Regular);
        arrsubparts.Add("2a");
        arrsubparts.Add("2b");
        arrsubparts.Add("2c");
        arrsubparts.Add("2d");
        lblErrSearch.Text = "";
        lblErrSearch.Visible = false;
        const int rpt_type_index = 8;// Convert.ToInt32(ddlreporttype.SelectedItem.Value);
        try
        {
            strorderby = d2.GetFunction("select LinkValue from inssettings where college_code=" + Convert.ToString(collegecode) + " and linkname='Student Attendance'");
            qry = "SELECT collname,affliatedby,address1,address2,district,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile,parentF_Mobile,StuPer_Id,parent_addressc,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,a.bldgrp,studhouse,Districtp,parent_statep,parent_pincodep,parentM_Mobile,countryp,serialno,emailM,ParentidP from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code";
            ds.Clear();
            ds.Dispose();
            ds = da.select_method_wo_parameter(qry, "Text");
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

            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;
            int checkattalign = 650;
            string college_code = Convert.ToString(collegecode);
            string stdappno = "";
            string maxmark_c1 = "";
            string maxmark_c2 = "";
            string qrycri = "select Criteria_no,Criteria,ci.min_mark,ci.max_mark from syllabus_master sm,CriteriaForInternal ci where sm.syll_code=ci.syll_code and Batch_Year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and degree_code='" + degreecode1 + "' and semester='" + Convert.ToString(ddlSem.SelectedValue) + "' order by Criteria_no asc";
            dscriteria = da.select_method_wo_parameter(qrycri, "Text");
            if (dscriteria.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dscriteria.Tables[0].Rows.Count; i++)
                {
                    if ((i == 0 || i == 1))
                    {
                        if (criteria_no == "")
                        {
                            criteria_no = Convert.ToString(dscriteria.Tables[0].Rows[i]["Criteria_no"]);
                            test = Convert.ToString(dscriteria.Tables[0].Rows[i]["Criteria"]);
                            newtest = Convert.ToString(dscriteria.Tables[0].Rows[0]["Criteria_no"]);
                            testname1 = Convert.ToString(dscriteria.Tables[0].Rows[0]["Criteria"]);
                            maxmark_c1 = Convert.ToString(dscriteria.Tables[0].Rows[0]["max_mark"]);
                        }
                        else
                        {
                            criteria_no += "','" + Convert.ToString(dscriteria.Tables[0].Rows[i]["Criteria_no"]);
                            test = "','" + Convert.ToString(dscriteria.Tables[0].Rows[i]["Criteria"]);
                            newtest1 = Convert.ToString(dscriteria.Tables[0].Rows[1]["Criteria_no"]);
                            testname2 = Convert.ToString(dscriteria.Tables[0].Rows[1]["Criteria"]);
                            maxmark_c2 = Convert.ToString(dscriteria.Tables[0].Rows[1]["max_mark"]);
                        }
                    }
                }
                if (newtest != "" && newtest != null && newtest1 != "" && newtest1 != null)
                {
                    string query = "select distinct TextCode,TextVal from textvaltable where TextCriteria = 'Rmrk' and college_code = '" + Convert.ToString(collegecode) + "'";
                    dsRmark.Clear();
                    dsRmark = da.select_method_wo_parameter(query, "Text");
                    //string mrk_changed1 = "select R.Current_Semester,r.Roll_No,r.sections,r.Reg_No,r.Stud_Name,r.Roll_Admit,c.criteria,c.Criteria_no,e.exam_code,convert(varchar(10),exam_date,103)as exam_date,e.max_mark,e.min_mark,sub.subject_name,sub.subject_code,s.subject_no,re.marks_obtained,isnull(re.remarks,'') as remarks,r.degree_code,sub.syll_code,sub.subType_no from Registration r,CriteriaForInternal c,Exam_type e,Result re,subject sub,subjectchooser s where c.Criteria_no=e.criteria_no and sub.subject_no=s.subject_no and s.subject_no=e.subject_no and s.roll_no=r.roll_no and e.exam_code=re.exam_code and r.roll_no=re.roll_no and r.Batch_Year='" +  Convert.ToString(ddlbatch.SelectedItem) + "' and r.degree_code='" +  Convert.ToString(ddlDept.SelectedValue) + "' and e.criteria_no in('" + criteria_no + "');SELECT * from CoCurrActivitie_Det where Degree_Code='" +  Convert.ToString(ddlDept.SelectedValue) + "' and batch_year='" +  Convert.ToString(ddlbatch.SelectedValue) + "' and istype='Att' and term='" + ddlSem.SelectedItem.Text + "';SELECT * from CoCurrActivitie_Det where Degree_Code='" +  Convert.ToString(ddlDept.SelectedValue) + "' and batch_year='" +  Convert.ToString(ddlbatch.SelectedValue) + "' and istype='remks' and term='" + ddlSem.SelectedItem.Text + "';select r.Roll_No,sum(re.marks_obtained) as total,sub.subject_no,r.Sections from Registration r,CriteriaForInternal c,Exam_type e,Result re,subject sub,subjectchooser s where c.Criteria_no=e.criteria_no and sub.subject_no=s.subject_no and s.subject_no=e.subject_no and s.roll_no=r.roll_no and e.exam_code=re.exam_code and r.roll_no=re.roll_no and r.Batch_Year='" +  Convert.ToString(ddlbatch.SelectedValue) + "' and r.degree_code='" +  Convert.ToString(ddlDept.SelectedValue) + "' and e.criteria_no in('" + criteria_no + "') and re.marks_obtained>=0 group by r.Roll_No,sub.subject_no,r.Sections order by sub.subject_no;select r.Roll_No,sum(re.marks_obtained) as total,r.sections from Registration r,CriteriaForInternal c,Exam_type e,Result re,subject sub,subjectchooser s where c.Criteria_no=e.criteria_no and sub.subject_no=s.subject_no and s.subject_no=e.subject_no and s.roll_no=r.roll_no and e.exam_code=re.exam_code and r.roll_no=re.roll_no and r.Batch_Year='" +  Convert.ToString(ddlbatch.SelectedValue) + "' and r.degree_code='" +  Convert.ToString(ddlDept.SelectedValue) + "' and e.criteria_no in('" + criteria_no + "') and re.marks_obtained>=0 group by r.Roll_No,r.sections order by total desc;";
                    string mrk_changed1 = "select R.Current_Semester,r.Roll_No,r.sections,r.Reg_No,r.Stud_Name,r.Roll_Admit,c.criteria,c.Criteria_no,e.exam_code,convert(varchar(10),e.exam_date,103)as exam_date,e.max_mark,e.min_mark,sub.subject_name,sub.subject_code,s.subject_no,re.marks_obtained,isnull(re.remarks,'') as remarks,r.degree_code,sub.subType_no,sub.syll_code from Registration r,CriteriaForInternal c,Exam_type e,Result re,subject sub,subjectchooser s where c.Criteria_no=e.criteria_no and sub.subject_no=s.subject_no and s.subject_no=e.subject_no and s.roll_no=r.roll_no and e.exam_code=re.exam_code and r.roll_no=re.roll_no and r.Batch_Year='" + Convert.ToString(ddlbatch.SelectedItem) + "' and r.degree_code='" + Convert.ToString(ddlDept.SelectedValue) + "' and e.criteria_no in('" + criteria_no + "') and college_code='" + Convert.ToString(collegecode) + "' and cc=0 and delflag=0 and exam_flag<>'Debar';SELECT * from CoCurrActivitie_Det where Degree_Code='" + Convert.ToString(ddlDept.SelectedValue) + "' and batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and istype='Att' and term='" + ddlSem.SelectedItem.Text + "';SELECT * from CoCurrActivitie_Det where Degree_Code='" + degreecode1 + "' and batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and istype='remks' and term='" + Convert.ToString(ddlSem.SelectedItem.Text) + "';select r.Roll_No,sum(re.marks_obtained) as total,sub.subject_no,r.Sections from Registration r,CriteriaForInternal c,Exam_type e,Result re,subject sub,subjectchooser s where c.Criteria_no=e.criteria_no and sub.subject_no=s.subject_no and s.subject_no=e.subject_no and s.roll_no=r.roll_no and e.exam_code=re.exam_code and r.roll_no=re.roll_no and r.Batch_Year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and r.degree_code='" + degreecode1 + "' and e.criteria_no in('" + criteria_no + "') and re.marks_obtained>=0 and college_code='" + Convert.ToString(collegecode) + "' and cc=0 and delflag=0 and exam_flag<>'Debar' group by r.Roll_No,sub.subject_no,r.Sections order by sub.subject_no;select r.Roll_No,sum(re.marks_obtained) as total,r.sections from Registration r,CriteriaForInternal c,Exam_type e,Result re,subject sub,subjectchooser s where c.Criteria_no=e.criteria_no and sub.subject_no=s.subject_no and s.subject_no=e.subject_no and s.roll_no=r.roll_no and e.exam_code=re.exam_code and r.roll_no=re.roll_no and r.Batch_Year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and r.degree_code='" + degreecode1 + "' and e.criteria_no in('" + criteria_no + "') and re.marks_obtained>=0 and college_code='" + Convert.ToString(collegecode) + "' and cc=0 and delflag=0 and exam_flag<>'Debar' group by r.Roll_No,r.sections order by total desc;";
                    dsexammark = d2.select_method_wo_parameter(mrk_changed1, "Text");
                    query = "select ca.CoCurr_ID,ca.Title_Name,tv.TextCode,tv.TextVal,ca.SubTitle from activity_entry ae,CoCurr_Activitie ca,textvaltable tv where ae.CoCurr_ID=ca.CoCurr_ID and ae.Batch_Year=ca.Batch_Year and ae.Degree_Code=ca.Degree_Code and ae.Batch_Year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and ae.Degree_Code='" + Convert.ToString(ddlDept.SelectedValue) + "' and ae.term='" + Convert.ToString(ddlSem.SelectedValue) + "' and tv.TextCode=ae.ActivityTextVal  and ae.ActivityTextVal in (select ActivityTextVal from CoCurrActivitie_Det where Degree_Code='" + Convert.ToString(ddlDept.SelectedValue) + "' and Batch_Year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and term='" + Convert.ToString(ddlSem.SelectedValue) + "' and mark<>0) order by SubTitle; select ca.SubTitle,TextCode,textval from textvaltable tv,CoCurr_Activitie ca where TextCode=Title_Name and Batch_Year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and Degree_Code='" + Convert.ToString(ddlDept.SelectedValue) + "'; select tv.TextVal as Activity, ag.Grade,cd.Roll_No,cd.ActivityTextVal from activity_gd ag ,CoCurrActivitie_Det cd,textvaltable tv where ag.ActivityTextVal=cd.ActivityTextVal and tv.TextCode=ag.ActivityTextVal and tv.TextCode=cd.ActivityTextVal and mark between frompoint and topoint and cd.Batch_Year=ag.batch_year and cd.term=ag.term and cd.Degree_Code=ag.Degree_Code and cd.term='" + Convert.ToString(ddlSem.SelectedValue) + "' and cd.Batch_Year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and cd.Degree_Code='" + Convert.ToString(ddlDept.SelectedValue) + "'";
                    dsparts = da.select_method_wo_parameter(query, "Text");
                    query = " select distinct subType_no from subject where syll_code in (select syll_code from syllabus_master where degree_code='" + degreecode1 + "' and Batch_Year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and semester='" + Convert.ToString(ddlSem.SelectedValue) + "'); select * from subject where syll_code in (select syll_code from syllabus_master where degree_code='" + degreecode1 + "' and Batch_Year='" + Convert.ToString(ddlbatch.SelectedValue) + "' and semester='" + Convert.ToString(ddlSem.SelectedValue) + "');";
                    dsSubParts = da.select_method_wo_parameter(query, "Text");
                    rollnos = rollno;
                    bool noentry = false;
                    if (rollnos != "")
                    {
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
                        int passcount = 0;
                        int failcount = 0;
                        PdfTablePage tblpage;
                        PdfTable tblpayprocess;
                        PdfTable tablestanes11;
                        PdfTable tblpart1;
                        PdfTable tblpart2;
                        DataView dvsection = new DataView();
                        if (studgradeds.Tables[0].Rows.Count > 0)
                        {
                            for (int roll = 0; roll < studgradeds.Tables[0].Rows.Count; roll++)
                            {
                                faillist.Clear();
                                secwise_stud_count = 0;
                                divwise_stud_count = 0;
                                checkattalign = 620;
                                passcount = 0;
                                failcount = 0;
                                int subpartsubcnt = 0;
                                noentry = false;
                                string rcrollno = "";
                                mypdfpage = mydoc.NewPage();
                                rcrollno = Convert.ToString(studgradeds.Tables[0].Rows[roll][1]);
                                string Roll_No = rcrollno;
                                string sec = Convert.ToString(studgradeds.Tables[0].Rows[roll]["Sections"]);
                                rollnos = rcrollno;

                                dsexammark.Tables[0].DefaultView.RowFilter = "Criteria_no in('" + criteria_no + "') and Roll_No='" + Roll_No + "'";
                                view = dsexammark.Tables[0].DefaultView;
                                view.Sort = "subject_no asc";
                                dtsub = view.ToTable(true, "Roll_No", "marks_obtained", "exam_date", "Subject_name", "max_mark", "min_mark", "subject_no", "Criteria_no", "remarks", "exam_code", "Criteria", "subType_no");

                                //dsexammark.Tables[0].DefaultView.RowFilter = "Criteria_no in('" + criteria_no + "')";
                                //view = dsexammark.Tables[0].DefaultView;
                                //view.Sort = "subject_no asc";
                                //dtTest = view.ToTable(true, "Roll_No", "marks_obtained", "exam_date", "Subject_name", "max_mark", "min_mark", "subject_no", "Criteria_no", "remarks", "exam_code", "Criteria", "subType_no");

                                int subcnt = dtsub.Rows.Count;

                                dsexammark.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                                view = dsexammark.Tables[1].DefaultView;
                                dtAtt = view.ToTable();

                                dsexammark.Tables[2].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                                view = dsexammark.Tables[2].DefaultView;
                                dtRmrk = view.ToTable();

                                DateTime dt = new DateTime();
                                exam_held = (dtsub.Rows.Count > 0) ? dtsub.Rows[0]["exam_date"].ToString() : "";
                                if (exam_held != "")
                                {
                                    DateTime.TryParseExact(exam_held, "dd/MM/yyyy", null, DateTimeStyles.None, out dt);
                                }
                                DataView dv = new DataView();
                                ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                                dv = ds.Tables[1].DefaultView;
                                int count4 = 0;
                                count4 = dv.Count;

                                if (ds.Tables[0].Rows.Count > 0 && count4 > 0)
                                {
                                    string serialno = Convert.ToString(dv[0]["serialno"]);
                                    string currentsem = Convert.ToString(dv[0]["Current_Semester"]);
                                    string batchyear = Convert.ToString(dv[0]["Batch_Year"]);
                                    string degreecode = Convert.ToString(dv[0]["degree_code"]);
                                    stdappno = Convert.ToString(dv[0]["App_No"]);
                                    string admdate = Convert.ToString(dv[0]["adm_date"]);
                                    string stdcc = "";
                                    stdcc = Convert.ToString(ds.Tables[1].Rows[0]["Dept_Name"]);

                                    sec = da.GetFunctionv("select sections from registration where roll_no='" + Roll_No + "'");
                                    if (sec != null && sec != "")
                                    {
                                        dsexammark.Tables[4].DefaultView.RowFilter = "sections='" + sec + "'";
                                        view = dsexammark.Tables[4].DefaultView;
                                        view.Sort = "total desc";
                                        dtSecRank = view.ToTable(true, "Roll_No", "total", "sections");
                                    }
                                    else
                                    {
                                        view = dsexammark.Tables[4].DefaultView;
                                        view.Sort = "total desc";
                                        dtSecRank = view.ToTable(true, "Roll_No", "total", "sections");
                                    }
                                    string dob = Convert.ToString(dv[0]["dob"]);
                                    string[] dobspit = dob.Split('/');
                                    string addressline1 = Convert.ToString(dv[0]["parent_addressP"]);
                                    string addressline2 = "";

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
                                    parentstatt = d2.GetFunction("select distinct textval from applyn  a,textvaltable t where a.parent_statep = t.TextCode and t.TextCode=" + parentstatt + "");

                                    if (parentstatt.Trim() == "" || parentstatt.Trim() == "0")
                                    {
                                        parentstatt = "";
                                    }
                                    string addressline3 = Convert.ToString(dv[0]["Districtp"]) + ", " + parentstatt + ", " + Convert.ToString(dv[0]["parent_pincodep"]);

                                    if (Convert.ToString(dv[0]["Districtp"]).Trim() != "" && parentstatt != "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() != "")
                                    {
                                        addressline3 = Convert.ToString(dv[0]["Districtp"]) + ", " + parentstatt + ", " + Convert.ToString(dv[0]["parent_pincodep"]);
                                    }
                                    else if (Convert.ToString(dv[0]["Districtp"]).Trim() != "" && parentstatt != "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() == "")
                                    {
                                        addressline3 = Convert.ToString(dv[0]["Districtp"]) + ", " + parentstatt;
                                    }
                                    else if (Convert.ToString(dv[0]["Districtp"]).Trim() != "" && parentstatt == "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() != "")
                                    {
                                        addressline3 = Convert.ToString(dv[0]["Districtp"]) + ", " + Convert.ToString(dv[0]["parent_pincodep"]);
                                    }
                                    else if (Convert.ToString(dv[0]["Districtp"]).Trim() == "" && parentstatt != "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() != "")
                                    {
                                        addressline3 = parentstatt + ", " + Convert.ToString(dv[0]["parent_pincodep"]);
                                    }
                                    else if (Convert.ToString(dv[0]["Districtp"]).Trim() == "" && parentstatt == "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() != "")
                                    {
                                        addressline3 = Convert.ToString(dv[0]["parent_pincodep"]);
                                    }
                                    else if (Convert.ToString(dv[0]["Districtp"]).Trim() != "" && parentstatt == "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() == "")
                                    {
                                        addressline3 = Convert.ToString(dv[0]["Districtp"]);
                                    }
                                    else if (Convert.ToString(dv[0]["Districtp"]).Trim() == "" && parentstatt != "" && Convert.ToString(dv[0]["parent_pincodep"]).Trim() == "")
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
                                    int moveleftvalue = 20;
                                    PdfTextArea pdf1 = new PdfTextArea(fntisceheader, System.Drawing.Color.Black, new PdfArea(mydoc, 30, 10, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Convert.ToString(ds.Tables[0].Rows[0][0]) + "");

                                    PdfTextArea pdfAff = new PdfTextArea(f8, System.Drawing.Color.Black, new PdfArea(mydoc, 30, 30, 595, 50), System.Drawing.ContentAlignment.TopCenter, "( " + ds.Tables[0].Rows[0]["affliatedby"].ToString() + " )");
                                    PdfTextArea pdf11 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 30, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["address2"].ToString().ToUpper() + " " + ds.Tables[0].Rows[0]["district"].ToString().ToUpper() + " - " + ds.Tables[0].Rows[0]["pincode"].ToString().ToUpper() + "");
                                    PdfArea pa1 = new PdfArea(mydoc, 30, 5, 560, 834);
                                    PdfArea pahealth = new PdfArea(mydoc, 2, 765, 591, 70);
                                    PdfRectangle pr1 = new PdfRectangle(mydoc, pa1, Color.Black);
                                    PdfArea pa2 = new PdfArea(mydoc, 189, 175, 224, 40);

                                    mypdfpage.Add(pdf1);
                                    mypdfpage.Add(pdf11);
                                    mypdfpage.Add(pdfAff);
                                    mypdfpage.Add(pr1);

                                    string sqlschool = "select value from Master_Settings where settings='Academic year'";
                                    dschool = da.select_method_wo_parameter(sqlschool, "Text");
                                    string splitvalue = Convert.ToString(dschool.Tables[0].Rows[0]["value"]);
                                    string[] dsplit = splitvalue.Split(',');

                                    string fvalue = Convert.ToString(dsplit[0]);
                                    string lvalue = Convert.ToString(dsplit[1]);
                                    string acdmic_date = fvalue + "-" + lvalue;
                                    PdfTextArea pdf_acadamicyear = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 55, 595, 50), System.Drawing.ContentAlignment.TopCenter, "STUDENT CUMULATIVE RECORD");
                                    mypdfpage.Add(pdf_acadamicyear);

                                    pdf_acadamicyear = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 70, 595, 50), System.Drawing.ContentAlignment.TopCenter, "Academic Year: " + acdmic_date + "");
                                    mypdfpage.Add(pdf_acadamicyear);

                                    //pdf_acadamicyear = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 80, 595, 50), System.Drawing.ContentAlignment.TopCenter,  " - " + ((dtsub.Rows.Count > 0) ? string.Format("{0:MMM-yyyy}", dt) : exam_held).ToString());
                                    //mypdfpage.Add(pdf_acadamicyear);

                                    pdf_acadamicyear = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 85, 595, 50), System.Drawing.ContentAlignment.TopCenter, "-----------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfpage.Add(pdf_acadamicyear);

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

                                    tblpage = tablestanes11.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, 105, 530, 250));
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

                                    tblpage = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, 140, 580, 250));
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

                                    tblpage = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, 190, 480, 250));
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

                                    tblpage = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, 225, 580, 250));
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
                                    tablestanes11.Cell(0, 6).SetContentAlignment(ContentAlignment.TopLeft);
                                    tablestanes11.Cell(0, 6).SetContent("Rank");
                                    tablestanes11.Cell(0, 7).SetContentAlignment(ContentAlignment.TopLeft);
                                    tablestanes11.Cell(0, 7).SetContent(":");
                                    tablestanes11.Cell(0, 8).SetContentAlignment(ContentAlignment.BottomLeft);
                                    tablestanes11.Cell(0, 8).SetFont(f4);

                                    pdf_acadamicyear = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 20, 260, 595, 50), System.Drawing.ContentAlignment.TopCenter, "-----------------------------------------------------------------------------------------------------------------------------------");
                                    mypdfpage.Add(pdf_acadamicyear);

                                    string romannew = ToRoman(Convert.ToString(ddlSem.SelectedItem.Text.Trim())).Trim();

                                    //if (ddlSem.SelectedItem.Text.Trim() == "1")
                                    //{
                                    //    romannew = "I";
                                    //}
                                    //else if (ddlSem.SelectedItem.Text.Trim() == "2")
                                    //{
                                    //    romannew = "II";
                                    //}
                                    //else if (ddlSem.SelectedItem.Text.Trim() == "3")
                                    //{
                                    //    romannew = "III";
                                    //}
                                    pdf_acadamicyear = new PdfTextArea(f11, System.Drawing.Color.Black, new PdfArea(mydoc, 0 + 15, 275, 595, 50), System.Drawing.ContentAlignment.TopCenter, "TERM - " + romannew + "");
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
                                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                        mypdfpage.Add(LogoImage, 35, 17, 490);
                                    }
                                    string marks = "";
                                    string marks1 = "";
                                    string Totmarks = "";
                                    double total = 0;
                                    double avg = 0;
                                    double subTot = 0;
                                    double overallTot = 0;
                                    int tblrow_cnt = 0;
                                    string standard = ddlDept.SelectedItem.Text.ToString();
                                    DataView dvsk = new DataView();

                                    if (dtsub.Rows.Count > 0)
                                    {
                                        if (newtest != "" && newtest != null)
                                        {
                                            dtsub.DefaultView.RowFilter = "Criteria_no=" + newtest;
                                            view = dtsub.DefaultView;
                                            view.Sort = "subject_no asc";
                                            dtsub1 = view.ToTable();
                                        }

                                        if (newtest1 != "" && newtest1 != null)
                                        {
                                            dtsub.DefaultView.RowFilter = "Criteria_no=" + newtest1;
                                            view = dtsub.DefaultView;
                                            view.Sort = "subject_no asc";
                                            dtsub2 = view.ToTable();
                                        }

                                        int subpart_cnt = 0;
                                        if (rpt_type_index == 8)
                                        {
                                            subpart_cnt = dsSubParts.Tables[0].Rows.Count;
                                            tblrow_cnt = (dtsub.Rows.Count / 2) + subpart_cnt + 2;
                                        }
                                        else if (rpt_type_index == 7)
                                        {
                                            subpart_cnt = 1;
                                            tblrow_cnt = (dtsub.Rows.Count / 2) + subpart_cnt;
                                        }

                                        else if (rpt_type_index == 9)
                                        {
                                            subpart_cnt = dsSubParts.Tables[0].Rows.Count;
                                            tblrow_cnt = (dtsub.Rows.Count / 2) + subpart_cnt + 3;
                                        }

                                        string[] colName = new string[] { "Subjects", "", "", "Total", "Remarks" };
                                        int sub = 0;
                                        tblpayprocess = mydoc.NewTable(fr8, tblrow_cnt, 5, 3);
                                        tblpayprocess.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        tblpayprocess.VisibleHeaders = false;
                                        tblpayprocess.SetColumnsWidth(new int[] { 255, 130, 130, 140, 260 });
                                        for (int r = 0; r < colName.Length; r++)
                                        {
                                            if (r != 1 && r != 2 && r != 3)
                                            {
                                                tblpayprocess.Cell(0, r).SetContent(colName[r]);
                                            }
                                            else
                                            {
                                                if (r == 1)
                                                {
                                                    tblpayprocess.Cell(0, r).SetContent(testname1 + "\n(" + maxmark_c1 + ")");
                                                }
                                                else if (r == 2)
                                                {
                                                    tblpayprocess.Cell(0, r).SetContent(testname2 + "\n(" + maxmark_c2 + ")");
                                                }
                                            }
                                            if (r == 3)
                                                tblpayprocess.Cell(0, r).SetContent(colName[r] + "\n(" + (Convert.ToInt16(maxmark_c1) + Convert.ToInt16(maxmark_c2)) + ")");
                                            tblpayprocess.Cell(0, r).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tblpayprocess.Cell(0, r).SetFont(f8);
                                        }

                                        if (dtsub1.Rows.Count > 0 && dtsub1.Rows.Count == dtsub2.Rows.Count)
                                        {
                                            status = true;
                                            noentry = true;
                                            int startrow = 0;
                                            int rcount = dtsub1.Rows.Count;
                                            if (rpt_type_index == 7)
                                            {
                                                rcount = dtsub1.Rows.Count;
                                            }
                                            else if (rpt_type_index == 8)
                                            {
                                                rcount = dtsub1.Rows.Count + subpart_cnt;
                                            }
                                            else if (rpt_type_index == 9)
                                            {
                                                rcount = dtsub1.Rows.Count;
                                            }
                                            if (rpt_type_index != 7)
                                            {
                                                subTot = 0;
                                                if (dsSubParts.Tables[0].Rows.Count > 0)
                                                {
                                                    DataTable dtsubpart1 = new DataTable();
                                                    DataTable dtsubpart2 = new DataTable();
                                                    double subpartTotal = 0;
                                                    double partavg = 0;
                                                    subpartsubcnt = 0;
                                                    for (int sp = 0; sp < dsSubParts.Tables[0].Rows.Count; sp++)
                                                    {
                                                        partavg = 0;
                                                        string Sub_Part_name = "";
                                                        if (dsSubParts.Tables[0].Rows[sp]["subType_no"].ToString() != "" && dsSubParts.Tables[0].Rows[sp]["subType_no"].ToString() != null)
                                                        {
                                                            dtsub1.DefaultView.RowFilter = "subType_no='" + dsSubParts.Tables[0].Rows[sp]["subType_no"] + "'";
                                                            view = dtsub1.DefaultView;
                                                            dtsubpart1 = view.ToTable();

                                                            dtsub2.DefaultView.RowFilter = "subType_no='" + dsSubParts.Tables[0].Rows[sp]["subType_no"] + "'";
                                                            view = dtsub2.DefaultView;
                                                            dtsubpart2 = view.ToTable();

                                                            Sub_Part_name = da.GetFunctionv("select subject_type from sub_sem where subType_no=" + dsSubParts.Tables[0].Rows[sp]["subType_no"]);
                                                        }
                                                        subpartTotal = 0;
                                                        if (dtsubpart1.Rows.Count > 0)
                                                        {
                                                            for (sub = 0; sub < dtsubpart1.Rows.Count; sub++)
                                                            {
                                                                subTot = 0;
                                                                tblpayprocess.Cell(startrow + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                tblpayprocess.Cell(startrow + 1, 0).SetContent(dtsubpart1.Rows[sub]["Subject_name"].ToString());
                                                                multiple = "";
                                                                marks = dtsubpart1.Rows[sub]["marks_obtained"].ToString();
                                                                marks1 = dtsubpart2.Rows[sub]["marks_obtained"].ToString();
                                                                if (marks == "" || marks == null)
                                                                {
                                                                    marks = "0";
                                                                }
                                                                if (marks1 == "" || marks1 == null)
                                                                {
                                                                    marks1 = "0";
                                                                }
                                                                if (Sub_Part_name == "Extra")
                                                                {
                                                                    subpartsubcnt++;
                                                                }
                                                                double minmark = Convert.ToInt16(dtsubpart1.Rows[sub]["min_mark"].ToString());
                                                                double minmark1 = Convert.ToInt16(dtsubpart2.Rows[sub]["min_mark"].ToString());
                                                                string obtmrk = dtsubpart1.Rows[sub]["marks_obtained"].ToString();
                                                                string obtmrk1 = dtsubpart2.Rows[sub]["marks_obtained"].ToString();
                                                                if (obtmrk == "" || obtmrk == null)
                                                                    obtmrk = "0";
                                                                if (obtmrk1 == "" || obtmrk1 == null)
                                                                    obtmrk1 = "0";
                                                                if (Convert.ToDouble(obtmrk) >= minmark && Convert.ToDouble(obtmrk1) >= minmark1)
                                                                {
                                                                    passcount++;
                                                                }
                                                                else
                                                                {
                                                                    failcount++;
                                                                }
                                                                if (Convert.ToDouble(marks) >= 0 && Convert.ToDouble(marks1) >= 0)
                                                                {
                                                                    subTot += Convert.ToDouble(marks) + Convert.ToDouble(marks1);
                                                                }
                                                                else if (Convert.ToDouble(marks) >= 0 && Convert.ToDouble(marks1) < 0)
                                                                {
                                                                    subTot += Convert.ToDouble(marks);
                                                                }
                                                                else if (Convert.ToDouble(marks) < 0 && Convert.ToDouble(marks1) >= 0)
                                                                {
                                                                    subTot += Convert.ToDouble(marks1);
                                                                }
                                                                //overallTot += subTot;
                                                                subpartTotal += Math.Round(subTot, 0);
                                                                if (Convert.ToDouble(marks) < 0)
                                                                {
                                                                    marks = findresult(marks);
                                                                }
                                                                if (Convert.ToDouble(marks1) < 0)
                                                                {
                                                                    marks1 = findresult(marks1);
                                                                }
                                                                tblpayprocess.Cell(startrow + 1, 1).SetContent(marks);
                                                                tblpayprocess.Cell(startrow + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                tblpayprocess.Cell(startrow + 1, 3).SetContent(Math.Round(subTot, 0));
                                                                tblpayprocess.Cell(startrow + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                if (dtsubpart2.Rows.Count > 0)
                                                                {
                                                                    string remark = "";
                                                                    if (dtsubpart2.Rows[sub]["remarks"].ToString() != "" && dtsubpart2.Rows[sub]["remarks"].ToString() != null)
                                                                    {
                                                                        remark = dtsubpart2.Rows[sub]["remarks"].ToString();
                                                                    }
                                                                    else if (dtsubpart1.Rows[sub]["remarks"].ToString() != "" && dtsubpart1.Rows[sub]["remarks"].ToString() != null)
                                                                    {
                                                                        remark = dtsubpart1.Rows[sub]["remarks"].ToString();
                                                                    }
                                                                    tblpayprocess.Cell(startrow + 1, 2).SetContent(marks1);
                                                                    tblpayprocess.Cell(startrow + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                    if (dsRmark.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        for (int rmrk = 0; rmrk < dsRmark.Tables[0].Rows.Count; rmrk++)
                                                                        {
                                                                            if (dsRmark.Tables[0].Rows[rmrk]["TextCode"].ToString() == remark)
                                                                                tblpayprocess.Cell(startrow + 1, 4).SetContent(dsRmark.Tables[0].Rows[rmrk]["TextVal"].ToString());
                                                                        }
                                                                        tblpayprocess.Cell(startrow + 1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                    }
                                                                }
                                                                startrow++;
                                                            }
                                                            if (Sub_Part_name != "Extra")
                                                            {
                                                                if (subpartTotal != null)
                                                                {
                                                                    partavg = Math.Round((subpartTotal / dtsubpart1.Rows.Count), 0);
                                                                    overallTot += partavg;
                                                                    subpartsubcnt++;
                                                                }
                                                                tblpayprocess.Cell(startrow + 1, 3).SetContent(partavg);
                                                                tblpayprocess.Cell(startrow + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                tblpayprocess.Cell(startrow + 1, 0).SetContent("* * * Average * * *");
                                                                tblpayprocess.Cell(startrow + 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                foreach (PdfCell pc in tblpayprocess.CellRange(startrow + 1, 0, startrow + 1, 0).Cells)
                                                                {
                                                                    pc.ColSpan = 3;
                                                                }
                                                                tblpayprocess.Cell(startrow + 1, 4).SetContent(Sub_Part_name);
                                                            }
                                                            else
                                                            {
                                                                overallTot += subpartTotal;
                                                            }
                                                            tblpayprocess.Cell(startrow + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            startrow++;
                                                            //startrow += dtsubpart1.Rows.Count;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                for (sub = 0; sub < rcount; sub++)
                                                {
                                                    subTot = 0;
                                                    //subfaillist.Clear();
                                                    if (sec != "" && sec != null)
                                                    {
                                                        dsexammark.Tables[3].DefaultView.RowFilter = "sections='" + sec + "' and subject_no='" + dtsub1.Rows[sub]["subject_no"].ToString() + "'";
                                                        view = dsexammark.Tables[3].DefaultView;
                                                        view.Sort = "total desc";
                                                        dtSubRank = view.ToTable(true, "Roll_No", "total", "subject_no");

                                                        //dtTest.DefaultView.RowFilter = "sections='" + sec + "' and subject_no='" + dtsub1.Rows[sub]["subject_no"].ToString() + "' and Criteria_no=" + newtest;
                                                        //view = dtTest.DefaultView;
                                                        //view.Sort = "subject_no asc";
                                                        //dtTest1 = view.ToTable();

                                                        //dtTest.DefaultView.RowFilter = "sections='" + sec + "' and subject_no='" + dtsub2.Rows[sub]["subject_no"].ToString() + "' and Criteria_no=" + newtest1;
                                                        //view = dtTest.DefaultView;
                                                        //view.Sort = "subject_no asc";
                                                        //dtTest2 = view.ToTable();
                                                    }
                                                    else
                                                    {
                                                        dsexammark.Tables[3].DefaultView.RowFilter = "subject_no='" + dtsub1.Rows[sub]["subject_no"].ToString() + "'";
                                                        view = dsexammark.Tables[3].DefaultView;
                                                        view.Sort = "total desc";
                                                        dtSubRank = view.ToTable(true, "Roll_No", "total", "subject_no");

                                                        //dtTest.DefaultView.RowFilter = "subject_no='" + dtsub1.Rows[sub]["subject_no"].ToString() + "' and Criteria_no=" + newtest;
                                                        //view = dtTest.DefaultView;
                                                        //view.Sort = "subject_no asc";
                                                        //dtTest1 = view.ToTable();

                                                        //dtTest.DefaultView.RowFilter = "subject_no='" + dtsub2.Rows[sub]["subject_no"].ToString() + "' and Criteria_no=" + newtest1;
                                                        //view = dtTest.DefaultView;
                                                        //view.Sort = "subject_no asc";
                                                        //dtTest2 = view.ToTable();
                                                    }
                                                    tblpayprocess.Cell(sub + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    tblpayprocess.Cell(sub + 1, 0).SetContent(dtsub1.Rows[sub]["Subject_name"].ToString());
                                                    multiple = "";
                                                    marks = dtsub1.Rows[sub]["marks_obtained"].ToString();
                                                    marks1 = dtsub2.Rows[sub]["marks_obtained"].ToString();
                                                    if (marks == "" || marks == null)
                                                    {
                                                        marks = "0";
                                                    }
                                                    if (marks1 == "" || marks1 == null)
                                                    {
                                                        marks1 = "0";
                                                    }
                                                    double minmark = Convert.ToInt16(dtsub1.Rows[sub]["min_mark"].ToString());
                                                    double minmark1 = Convert.ToInt16(dtsub2.Rows[sub]["min_mark"].ToString());
                                                    string obtmrk = dtsub1.Rows[sub]["marks_obtained"].ToString();
                                                    string obtmrk1 = dtsub2.Rows[sub]["marks_obtained"].ToString();
                                                    if (obtmrk == "" || obtmrk == null)
                                                        obtmrk = "0";
                                                    if (obtmrk1 == "" || obtmrk1 == null)
                                                        obtmrk1 = "0";
                                                    if (Convert.ToDouble(obtmrk) < 0 || Convert.ToDouble(obtmrk1) < 0)
                                                    {
                                                        failcount++;
                                                    }
                                                    //if(Convert.ToDouble(obtmrk)
                                                    if (Convert.ToDouble(obtmrk) >= minmark && Convert.ToDouble(obtmrk1) >= minmark1)
                                                    {
                                                        passcount++;
                                                    }
                                                    else
                                                    {
                                                        failcount++;
                                                    }
                                                    if (Convert.ToDouble(marks) >= 0 && Convert.ToDouble(marks1) >= 0)
                                                    {
                                                        subTot += Convert.ToDouble(marks) + Convert.ToDouble(marks1);
                                                    }
                                                    else if (Convert.ToDouble(marks) >= 0 && Convert.ToDouble(marks1) < 0)
                                                    {
                                                        subTot += Convert.ToDouble(marks);
                                                    }
                                                    else if (Convert.ToDouble(marks) < 0 && Convert.ToDouble(marks1) >= 0)
                                                    {
                                                        subTot += Convert.ToDouble(marks1);
                                                    }
                                                    overallTot += Math.Round(subTot, 0);
                                                    if (Convert.ToDouble(marks) < 0)
                                                    {
                                                        marks = findresult(marks);
                                                    }
                                                    if (Convert.ToDouble(marks1) < 0)
                                                    {
                                                        marks1 = findresult(marks1);
                                                    }
                                                    tblpayprocess.Cell(sub + 1, 1).SetContent(marks);
                                                    tblpayprocess.Cell(sub + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    tblpayprocess.Cell(sub + 1, 3).SetContent(Math.Round(subTot, 0));
                                                    tblpayprocess.Cell(sub + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    if (dtsub2.Rows.Count > 0)
                                                    {
                                                        tblpayprocess.Cell(sub + 1, 2).SetContent(marks1);
                                                        tblpayprocess.Cell(sub + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        string remark = "";
                                                        if (dtsub1.Rows[sub]["remarks"].ToString() != "" && dtsub1.Rows[sub]["remarks"].ToString() != null)
                                                        {
                                                            remark = dtsub1.Rows[sub]["remarks"].ToString();
                                                        }
                                                        else if (dtsub2.Rows[sub]["remarks"].ToString() != "" && dtsub2.Rows[sub]["remarks"].ToString() != null)
                                                        {
                                                            remark = dtsub2.Rows[sub]["remarks"].ToString();
                                                        }
                                                        if (dsRmark.Tables[0].Rows.Count > 0)
                                                        {
                                                            for (int rmrk = 0; rmrk < dsRmark.Tables[0].Rows.Count; rmrk++)
                                                            {
                                                                if (dsRmark.Tables[0].Rows[rmrk]["TextCode"].ToString() == remark)
                                                                    tblpayprocess.Cell(sub + 1, 4).SetContent(dsRmark.Tables[0].Rows[rmrk]["TextVal"].ToString());
                                                            }
                                                            tblpayprocess.Cell(sub + 1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (rpt_type_index != 7)
                                        {
                                            if (rpt_type_index == 8)
                                            {
                                                tblpayprocess.Cell(tblrow_cnt - 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tblpayprocess.Cell(tblrow_cnt - 1, 0).SetContent("Total");
                                                tblpayprocess.Cell(tblrow_cnt - 1, 0).SetFont(f8);
                                                foreach (PdfCell pc in tblpayprocess.CellRange(tblrow_cnt - 1, 0, tblrow_cnt - 1, 0).Cells)
                                                {
                                                    pc.ColSpan = 3;
                                                }
                                                tblpayprocess.Cell(tblrow_cnt - 1, 3).SetContent(Math.Round(overallTot, 0));
                                                tblpayprocess.Cell(tblrow_cnt - 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            }
                                            else
                                            {

                                                tblpayprocess.Cell(tblrow_cnt - 2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tblpayprocess.Cell(tblrow_cnt - 2, 0).SetContent("Total");
                                                tblpayprocess.Cell(tblrow_cnt - 2, 0).SetFont(f8);

                                                tblpayprocess.Cell(tblrow_cnt - 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tblpayprocess.Cell(tblrow_cnt - 1, 0).SetContent("Average");
                                                tblpayprocess.Cell(tblrow_cnt - 1, 0).SetFont(f8);
                                                foreach (PdfCell pc in tblpayprocess.CellRange(tblrow_cnt - 2, 0, tblrow_cnt - 2, 0).Cells)
                                                {
                                                    pc.ColSpan = 3;
                                                }

                                                foreach (PdfCell pc in tblpayprocess.CellRange(tblrow_cnt - 1, 0, tblrow_cnt - 1, 0).Cells)
                                                {
                                                    pc.ColSpan = 3;
                                                }
                                                avg = Math.Round((Math.Round(overallTot, 0) / subpartsubcnt), 2);
                                                tblpayprocess.Cell(tblrow_cnt - 2, 3).SetContent(Math.Round(overallTot, 0));
                                                tblpayprocess.Cell(tblrow_cnt - 1, 3).SetContent(avg);
                                                tblpayprocess.Cell(tblrow_cnt - 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tblpayprocess.Cell(tblrow_cnt - 2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            }
                                        }
                                        multiple = "";
                                        checkpoint = 0;
                                        if (dtSecRank.Rows.Count > 0 && checkpoint == 0)
                                        {
                                            int rank = 1;
                                            multiple = Math.Round(Convert.ToDouble(dtSecRank.Rows[0]["total"].ToString()), 0).ToString();
                                            totsechigh = Convert.ToDouble(dtSecRank.Rows[0]["total"].ToString());
                                            for (int r = 0; r < dtSecRank.Rows.Count; r++)
                                            {
                                                if (Roll_No != dtSecRank.Rows[r]["Roll_No"].ToString())
                                                {
                                                    if (multiple != Math.Round(Convert.ToDouble(dtSecRank.Rows[r]["total"].ToString()), 0).ToString())
                                                    {
                                                        rank++;
                                                        multiple = Math.Round(Convert.ToDouble(dtSecRank.Rows[r]["total"].ToString()), 0).ToString();
                                                    }
                                                }
                                                else
                                                {
                                                    if (multiple != Math.Round(Convert.ToDouble(dtSecRank.Rows[r]["total"].ToString()), 0).ToString())
                                                    {
                                                        rank++;
                                                        totsecrank = rank;
                                                        checkpoint = 1;
                                                        multiple = Math.Round(Convert.ToDouble(dtSecRank.Rows[r]["total"].ToString()), 0).ToString();
                                                    }
                                                    else
                                                    {
                                                        totsecrank = rank;
                                                        checkpoint = 1;
                                                    }
                                                }
                                                if (checkpoint == 1)
                                                {
                                                    multiple = "";
                                                    rank = 0;
                                                }
                                            }
                                        }
                                        string totRnk = "";
                                        //if (failcount == 0)
                                        //{
                                        totRnk = totsecrank.ToString();
                                        //}
                                        tablestanes11.Cell(0, 8).SetContent(totRnk);
                                        tblpage = tablestanes11.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, 245, 580, 250));
                                        mypdfpage.Add(tblpage);

                                        coltop = 300;
                                        tblpage = tblpayprocess.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19 + moveleftvalue, coltop, 296, 620));
                                        mypdfpage.Add(tblpage);
                                        double width = 0;
                                        double getheigh = 0;

                                        coltop = 300;
                                        width = tblpage.Area.Width;
                                        int colcount = 0;

                                        string parttitle1a1 = "";
                                        if (dsparts.Tables[0].Rows.Count > 0)
                                        {
                                            string courrid1 = "";
                                            string courrid2 = "";
                                            DataView partdv = new DataView();
                                            arrcourrid.Clear();
                                            dsparts.Tables[0].DefaultView.RowFilter = "SubTitle='2a'";
                                            view = dsparts.Tables[0].DefaultView;
                                            dtparts = view.ToTable();

                                            dsparts.Tables[0].DefaultView.RowFilter = "SubTitle='2b'";
                                            view = dsparts.Tables[0].DefaultView;
                                            dtparts1 = view.ToTable();

                                            dsparts.Tables[0].DefaultView.RowFilter = "SubTitle='2c'";
                                            view = dsparts.Tables[0].DefaultView;
                                            dtparts2 = view.ToTable();

                                            dsparts.Tables[0].DefaultView.RowFilter = "SubTitle='2d'";
                                            view = dsparts.Tables[0].DefaultView;
                                            dtparts3 = view.ToTable();
                                            if (dtparts.Rows.Count > 0 && dtparts1.Rows.Count > 0 && dtparts1.Rows.Count == dtparts.Rows.Count)
                                            {
                                                courrid1 = dtparts.Rows[0]["CoCurr_ID"].ToString();
                                                courrid2 = dtparts1.Rows[0]["CoCurr_ID"].ToString();
                                                int partrowcount = 0;
                                                partrowcount = dtparts.Rows.Count;
                                                tblpart1 = mydoc.NewTable(fr8, partrowcount + 2, 4, 3);
                                                tblpart1.VisibleHeaders = false;
                                                tblpart1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                tblpart1.SetColumnsWidth(new int[] { 50, 20, 50, 20 });
                                                parttitle1a = da.GetFunction(" select textval from textvaltable where TextCode= '" + dtparts.Rows[0]["Title_Name"].ToString() + "'");
                                                parttitle1a1 = da.GetFunction(" select textval from textvaltable where TextCode= '" + dtparts1.Rows[0]["Title_Name"].ToString() + "'");
                                                tblpart1.Cell(0, 0).SetContent(parttitle1a);
                                                tblpart1.Cell(0, 0).SetFont(f8);
                                                tblpart1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                foreach (PdfCell pc in tblpart1.CellRange(0, 0, 0, 0).Cells)
                                                {
                                                    pc.ColSpan = 2;
                                                }
                                                tblpart1.Cell(0, 2).SetContent(parttitle1a1);
                                                tblpart1.Cell(0, 2).SetFont(f8);
                                                tblpart1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                foreach (PdfCell pc in tblpart1.CellRange(0, 2, 0, 2).Cells)
                                                {
                                                    pc.ColSpan = 2;
                                                }
                                                tblpart1.Cell(1, 0).SetContent("Skill");
                                                tblpart1.Cell(1, 0).SetFont(f8);
                                                tblpart1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tblpart1.Cell(1, 1).SetContent("Grade");
                                                tblpart1.Cell(1, 1).SetFont(f8);
                                                tblpart1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tblpart1.Cell(1, 2).SetContent("Attitude Towards");
                                                tblpart1.Cell(1, 2).SetFont(f8);
                                                tblpart1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tblpart1.Cell(1, 3).SetContent("Grade");
                                                tblpart1.Cell(1, 3).SetFont(f8);
                                                tblpart1.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                for (int j = 0; j < dtparts.Rows.Count; j++)
                                                {
                                                    for (int partcolumn = 0; partcolumn < 4; partcolumn++)
                                                    {
                                                        string content = "";
                                                        if (partcolumn == 0 || partcolumn == 1)
                                                        {
                                                            dsparts.Tables[2].DefaultView.RowFilter = "ActivityTextVal='" + dtparts.Rows[j]["Textcode"] + "' and Roll_No='" + Roll_No + "'";
                                                            view = dsparts.Tables[2].DefaultView;
                                                            if (view.Count > 0)
                                                            {
                                                                if (partcolumn == 0)
                                                                {
                                                                    content = view[0]["Activity"].ToString();
                                                                    tblpart1.Cell(j + 2, partcolumn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                }
                                                                else
                                                                {
                                                                    content = view[0]["Grade"].ToString();
                                                                    tblpart1.Cell(j + 2, partcolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                }
                                                            }
                                                            tblpart1.Cell(j + 2, partcolumn).SetContent(content);
                                                        }
                                                        else if (partcolumn == 2 || partcolumn == 3)
                                                        {
                                                            dsparts.Tables[2].DefaultView.RowFilter = "ActivityTextVal='" + dtparts1.Rows[j]["Textcode"] + "' and Roll_No='" + Roll_No + "'";
                                                            view = dsparts.Tables[2].DefaultView;
                                                            if (view.Count > 0)
                                                            {
                                                                if (partcolumn == 2)
                                                                {
                                                                    content = view[0]["Activity"].ToString();
                                                                    tblpart1.Cell(j + 2, partcolumn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                }
                                                                else
                                                                {
                                                                    content = view[0]["Grade"].ToString();
                                                                    tblpart1.Cell(j + 2, partcolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                }
                                                            }
                                                            tblpart1.Cell(j + 2, partcolumn).SetContent(content);
                                                        }

                                                    }
                                                }
                                                tblpage = tblpart1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, Convert.ToInt16(width) + 52, coltop, 235, 320));
                                                mypdfpage.Add(tblpage);
                                                coltop += Convert.ToInt16(Math.Round(tblpage.Area.Height, 0)) + 10;
                                            }
                                            if (dtparts2.Rows.Count > 0 && dtparts3.Rows.Count > 0 && dtparts3.Rows.Count == dtparts2.Rows.Count)
                                            {
                                                courrid1 = dtparts2.Rows[0]["CoCurr_ID"].ToString();
                                                courrid2 = dtparts3.Rows[0]["CoCurr_ID"].ToString();
                                                int partrowcount = 0;
                                                partrowcount = dtparts2.Rows.Count;
                                                tblpart2 = mydoc.NewTable(fr8, partrowcount + 1, 4, 3);
                                                tblpart2.VisibleHeaders = false;
                                                tblpart2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                tblpart2.SetColumnsWidth(new int[] { 50, 20, 50, 20 });
                                                parttitle1a = da.GetFunction(" select textval from textvaltable where TextCode= '" + dtparts2.Rows[0]["Title_Name"].ToString() + "'");
                                                parttitle1a1 = da.GetFunction(" select textval from textvaltable where TextCode= '" + dtparts3.Rows[0]["Title_Name"].ToString() + "'");
                                                tblpart2.Cell(0, 0).SetContent(parttitle1a);
                                                tblpart2.Cell(0, 0).SetFont(f8);
                                                tblpart2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tblpart2.Cell(0, 1).SetContent("Grade");
                                                tblpart2.Cell(0, 1).SetFont(f8);
                                                tblpart2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tblpart2.Cell(0, 2).SetContent(parttitle1a1);
                                                tblpart2.Cell(0, 2).SetFont(f8);
                                                tblpart2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tblpart2.Cell(0, 3).SetContent("Grade");
                                                tblpart2.Cell(0, 3).SetFont(f8);
                                                tblpart2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                for (int j = 0; j < dtparts2.Rows.Count; j++)
                                                {
                                                    for (int partcolumn = 0; partcolumn < 4; partcolumn++)
                                                    {
                                                        string content = "";
                                                        if (partcolumn == 0 || partcolumn == 1)
                                                        {
                                                            dsparts.Tables[2].DefaultView.RowFilter = "ActivityTextVal='" + dtparts2.Rows[j]["Textcode"] + "' and Roll_No='" + Roll_No + "'";
                                                            view = dsparts.Tables[2].DefaultView;
                                                            if (view.Count > 0)
                                                            {
                                                                if (partcolumn == 0)
                                                                {
                                                                    content = view[0]["Activity"].ToString();
                                                                    tblpart2.Cell(j + 1, partcolumn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                }
                                                                else
                                                                {
                                                                    content = view[0]["Grade"].ToString();
                                                                    tblpart2.Cell(j + 1, partcolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                }
                                                            }
                                                            tblpart2.Cell(j + 1, partcolumn).SetContent(content);
                                                        }
                                                        else if (partcolumn == 2 || partcolumn == 3)
                                                        {
                                                            dsparts.Tables[2].DefaultView.RowFilter = "ActivityTextVal='" + dtparts3.Rows[j]["Textcode"] + "' and Roll_No='" + Roll_No + "'";
                                                            view = dsparts.Tables[2].DefaultView;
                                                            if (view.Count > 0)
                                                            {
                                                                if (partcolumn == 2)
                                                                {
                                                                    content = view[0]["Activity"].ToString();
                                                                    tblpart2.Cell(j + 1, partcolumn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                }
                                                                else
                                                                {
                                                                    content = view[0]["Grade"].ToString();
                                                                    tblpart2.Cell(j + 1, partcolumn).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                }
                                                            }
                                                            tblpart2.Cell(j + 1, partcolumn).SetContent(content);
                                                        }
                                                    }
                                                }
                                                tblpage = tblpart2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, Convert.ToInt16(width) + 52, coltop, 235, 280));
                                                mypdfpage.Add(tblpage);
                                                coltop += Convert.ToInt16(Math.Round(tblpage.Area.Height, 0)) + 10;
                                            }
                                        }
                                        tblpayprocess = mydoc.NewTable(fr8, 2, 4, 3);
                                        tblpayprocess.VisibleHeaders = false;
                                        tblpayprocess.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        tblpayprocess.SetColumnsWidth(new int[] { 50, 20, 50, 20 });
                                        tblpayprocess.Cell(0, 0).SetContent("Grading System");
                                        tblpayprocess.Cell(0, 0).SetFont(f8);
                                        tblpayprocess.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        foreach (PdfCell pc in tblpayprocess.CellRange(0, 0, 0, 0).Cells)
                                        {
                                            pc.ColSpan = 4;
                                        }
                                        tblpayprocess.Cell(1, 0).SetContent("Grade A : Excellent \n Grade B : Good");
                                        tblpayprocess.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        foreach (PdfCell pc in tblpayprocess.CellRange(1, 0, 1, 0).Cells)
                                        {
                                            pc.ColSpan = 2;
                                        }
                                        tblpayprocess.Cell(1, 2).SetContent("Grade C : Fair \n Grade D : Can Do Better");
                                        tblpayprocess.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        foreach (PdfCell pc in tblpayprocess.CellRange(1, 2, 1, 2).Cells)
                                        {
                                            pc.ColSpan = 2;
                                        }
                                        tblpage = tblpayprocess.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, Convert.ToInt16(width) + 52, coltop, 235, 250));
                                        mypdfpage.Add(tblpage);
                                    }
                                    checkattalign = checkattalign + 59;
                                    if (dtAtt.Rows.Count > 0)
                                    {
                                        pdf_acadamicyear = new PdfTextArea(f11, System.Drawing.Color.Black, new PdfArea(mydoc, 20 + moveleftvalue, checkattalign, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Attendance :\t" + Convert.ToString(dtAtt.Rows[0]["Mark"]) + " / " + Convert.ToString(dtAtt.Rows[0]["totatt_remarks"]) + "  Days");
                                    }
                                    else
                                    {
                                        pdf_acadamicyear = new PdfTextArea(f11, System.Drawing.Color.Black, new PdfArea(mydoc, 20 + moveleftvalue, checkattalign, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Attendance :\t__________ / __________  Days");
                                    }
                                    mypdfpage.Add(pdf_acadamicyear);

                                    checkattalign = checkattalign + 20;
                                    if (dtRmrk.Rows.Count > 0)
                                    {
                                        pdf_acadamicyear = new PdfTextArea(f11, System.Drawing.Color.Black, new PdfArea(mydoc, 20 + moveleftvalue, checkattalign, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Remarks     :\t");
                                        mypdfpage.Add(pdf_acadamicyear);
                                        pdf_acadamicyear = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80 + moveleftvalue, checkattalign + 17, 450, 300), System.Drawing.ContentAlignment.TopLeft, "  " + dtRmrk.Rows[0]["totatt_remarks"].ToString() + "");
                                        mypdfpage.Add(pdf_acadamicyear);
                                    }
                                    else
                                    {
                                        pdf_acadamicyear = new PdfTextArea(f11, System.Drawing.Color.Black, new PdfArea(mydoc, 20 + moveleftvalue, checkattalign, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "Remarks     :\t");
                                        mypdfpage.Add(pdf_acadamicyear);
                                        checkattalign = checkattalign + 5;
                                        pdf_acadamicyear = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80 + moveleftvalue, checkattalign, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "_____________________________________________________________________________");
                                        mypdfpage.Add(pdf_acadamicyear);
                                        pdf_acadamicyear = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydoc, 80 + moveleftvalue, checkattalign + 20, 595, 50), System.Drawing.ContentAlignment.MiddleLeft, "_____________________________________________________________________________");
                                        mypdfpage.Add(pdf_acadamicyear);
                                    }
                                    checkattalign = 0;
                                    //============================================== Attedance And Remarks ==================================================
                                    tablestanes1 = mydoc.NewTable(f11, 1, 3, 3);
                                    tablestanes1.VisibleHeaders = false;
                                    tablestanes1.SetBorders(Color.Black, 1, BorderType.None);

                                    checkattalign = checkattalign + 70;
                                    tablestanes1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablestanes1.Cell(0, 0).SetContent("Signature of Teacher");
                                    tablestanes1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablestanes1.Cell(0, 1).SetContent("Signature of Principal");
                                    tablestanes1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablestanes1.Cell(0, 2).SetContent("Signature of Parent");
                                    tblpage = tablestanes1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 19, 800, 580, 250));
                                    mypdfpage.Add(tblpage);
                                }
                                if (noentry == true)
                                    mypdfpage.SaveToDocument();
                            }

                        }
                        //=======================
                    }
                }
                else
                {
                    strErr.Append("Must Create Both the Tests!!!");
                }
            }
            if (dscriteria.Tables[0].Rows.Count == 0)
                strErr.Append("Test is not Found!!!");
            else
            {
                if (dtsub.Rows.Count == 0)
                    strErr.Append("Please Check The Marks Entry For Selected Students!!!");
                else
                {
                    strErr.Append("Please Verify!!!! Both The Tests Have Same No.Of Subjects And Must Be Enter The Marks For Both The Tests for all the Selected Students!!! ");
                }
            }

            if (strErr.Length > 0)
            {
                lblErrSearch.Text = Convert.ToString(strErr);
                lblErrSearch.Visible = true;
            }
            else
            {
                lblErrSearch.Text = "";
                lblErrSearch.Visible = false;
            }
            if (status == true)
            {
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = Convert.ToString("ICSE_IX_To_X_Reportcard" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmss")).Trim().Replace(" ", "").Trim() + ".pdf";
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

    #endregion Reusable Methods

}