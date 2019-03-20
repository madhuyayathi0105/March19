#region Namespace Declaration

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;
using Farpnt = FarPoint.Web.Spread;
using System.Configuration;

#endregion Namespace Declaration

public partial class ReportCard_For_KG : System.Web.UI.Page
{
    #region Variable Declaration

    Hashtable hat = new Hashtable();
    string usercode = "", collegecode = "", singleuser = "", group_user = string.Empty;
    string batch_year = "", degree_code = "", semester = "", section = "", test_name = "", test_no = "", rollnos = string.Empty;

    string grouporusercode = string.Empty;

    bool serialflag;
    string qry = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    Boolean b_school = false;

    FarPoint.Web.Spread.ComboBoxCellType combocol = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType combocolgrade = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxcol = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxsel_all = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.TextCellType txtceltype = new FarPoint.Web.Spread.TextCellType();

    Dictionary<string, byte> dicHeaderAll = new Dictionary<string, byte>();
    byte reportHeaderBased = 0;

    #region For Attendance Calculation

    string currentsem = string.Empty;
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
    string value_holi_status = string.Empty;
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = string.Empty;

    string startdate = string.Empty;
    string enddate = string.Empty;
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
            string grouporusercode = string.Empty;
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
                lblErrSearch.Text = string.Empty;
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
                BindHeaderSettings();
                setPrevVal();
                GetHeaderSettings(ref dicHeaderAll, ref reportHeaderBased);
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
            string columnfield = string.Empty;
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
            lblErrSearch.Text = string.Empty;
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
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddlDegree.Items.Clear();
            usercode = Convert.ToString(Session["usercode"]);
            collegecode = Convert.ToString(ddlCollege.SelectedValue); ;
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
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
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddlDept.Items.Clear();
            hat.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Convert.ToString(ddlCollege.SelectedValue); ;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
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

    public void bindsem()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txtSem.Text = "---Select---";
            int i = 0;
            batch_year = Convert.ToString(ddlbatch.SelectedValue);
            degree_code = Convert.ToString(ddlDept.SelectedValue);

            if (batch_year != "" && degree_code != "")
            {
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = d2.BindSem(degree_code, batch_year, Convert.ToString(ddlCollege.SelectedValue));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].DefaultView.RowFilter = "ndurations=max(ndurations)";
                    DataView dv = ds.Tables[0].DefaultView;
                    if (dv.Count > 0)
                    {
                        int semcount = 0;
                        string semcountstring = Convert.ToString(dv[0][0]);
                        if (semcountstring != "")
                        {
                            semcount = Convert.ToInt32(semcountstring);
                        }
                        for (i = 1; i <= semcount; i++)
                        {
                            cbl_sem.Items.Add(i.ToString());
                        }
                    }

                    if (cbl_sem.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_sem.Items.Count; i++)
                        {
                            cbl_sem.Items[i].Selected = true;
                        }
                        txtSem.Text = ((!b_school) ? "Semester(" : "Term(") + cbl_sem.Items.Count + ")";

                        cb_sem.Checked = true;
                    }
                }
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
            lblErrSearch.Text = string.Empty;
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
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            txt_test.Text = "--Select--";
            Cb_test.Checked = false;
            Cbl_test.Items.Clear();

            batch_year = Convert.ToString(ddlbatch.SelectedValue).Trim();
            degree_code = Convert.ToString(ddlDept.SelectedValue).Trim();
            semester = string.Empty;
            foreach (ListItem li in cbl_sem.Items)
            {
                if (li.Selected)
                {
                    if (semester == "")
                    {
                        semester = li.Value;
                    }
                    else
                    {
                        semester += "," + li.Value;
                    }
                }
            }

            string SyllabusYr;
            string SyllabusQry;


            if (batch_year.Trim() != "" && degree_code.Trim() != "" && semester.Trim() != "")
            {
                //string Sqlstr = "select distinct c.criteria from criteriaforinternal c,registration r,syllabus_master s where r.degree_code=s.degree_code and r.batch_year=s.batch_year and c.syll_code=s.syll_code and cc=0 and delflag=0 and r.exam_flag<>'debar'   and r.batch_year='" + batch_year + "' and r.degree_code in(" + degree_code + ") and  s.semester in (" + semester + ") order by criteria asc";
                SyllabusQry = "select syllabus_year from syllabus_master where degree_code in (" + Convert.ToString(degree_code) + ") and semester in (" + Convert.ToString(semester) + ") and batch_year in (" + Convert.ToString(batch_year) + ")";
                SyllabusYr = d2.GetFunction(SyllabusQry.ToString());
                string Sqlstr;
                Sqlstr = string.Empty;
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
            lblErrSearch.Text = string.Empty;
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
            lblErrSearch.Text = string.Empty;
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
            lblErrSearch.Text = string.Empty;
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
            lblErrSearch.Text = string.Empty;
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

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
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

    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divViewSpread.Visible = false;
            int i = 0;
            txtSem.Text = "--Select--";
            if (cb_sem.Checked == true)
            {
                for (i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                }
                txtSem.Text = ((!b_school) ? "Semester(" : "Term(") + (cbl_sem.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
            }
            bindSection();
            bindtestname();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void Cb_test_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
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

    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divViewSpread.Visible = false;

            int i = 0;
            cb_sem.Checked = false;
            int commcount = 0;
            txtSem.Text = "--Select--";
            for (i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sem.Items.Count)
                {
                    cb_sem.Checked = true;
                }
                txtSem.Text = ((b_school) ? "Term(" : "Semester(") + commcount.ToString() + ")";
            }
            bindSection();
            bindtestname();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void Cbl_test_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
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
                txt_test.Text = "Test(" + commcount.ToString() + ")";
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
            lblErrSearch.Text = string.Empty;
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
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpoperr.Text = string.Empty;
            batch_year = string.Empty;
            degree_code = string.Empty;
            collegecode = string.Empty;
            semester = string.Empty;
            section = string.Empty;
            test_name = string.Empty;
            test_no = string.Empty;
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
            if (cbl_sem.Items.Count == 0)
            {
                lblpoperr.Text = ((!b_school) ? "Semester" : "Term") + " is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                semester = string.Empty;
                foreach (ListItem li in cbl_sem.Items)
                {
                    if (li.Selected)
                    {
                        if (selsem != 0)
                            Array.Resize(ref arr_semester, selsem + 1);
                        int.TryParse(li.Value, out arr_semester[selsem]);
                        selsem++;
                        if (semester == "")
                        {
                            semester = "'" + li.Value + "'";
                        }
                        else
                        {
                            semester += ",'" + li.Value + "'";
                        }
                    }
                }
                if (selsem == 0)
                {
                    lblpoperr.Text = "Please Select Atleast One " + ((!b_school) ? "Semester" : "Term");
                    popupdiv.Visible = true;
                    return;
                }
            }

            if (ddlsec.Enabled == true)
            {
                if (ddlsec.Items.Count > 0)
                {
                    section = string.Empty;
                    if (Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() != "")
                    {
                        section = Convert.ToString(ddlsec.SelectedItem.Text);
                        section = "and r.sections in ('" + section + "') ";
                        //newsecqry = " and sections in ('" + Convert.ToString(ddlsec.SelectedItem.Text) + "') ";
                    }
                    else
                    {
                        section = string.Empty;
                        //newsecqry  =string.Empty;
                    }
                }
            }
            else
            {
                section = string.Empty;
                //newsecqry  =string.Empty;
            }

            if (Cbl_test.Items.Count == 0)
            {
                lblpoperr.Text = "Test is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                test_no = string.Empty;
                test_name = string.Empty;
                foreach (ListItem li in Cbl_test.Items)
                {
                    if (li.Selected)
                    {
                        if (seltest != 0)
                            Array.Resize(ref arr_test, seltest + 1);
                        int.TryParse(li.Value, out arr_test[seltest]);
                        seltest++;
                        if (test_no == "")
                        {
                            test_no = "'" + li.Value + "'";
                            test_name = "'" + li.Text + "'";
                        }
                        else
                        {
                            test_no += ",'" + li.Value + "'";
                            test_name += ",'" + li.Text + "'";
                        }
                    }
                }
                if (seltest == 0)
                {
                    lblpoperr.Text = "Please Select Atleast One Test";
                    popupdiv.Visible = true;
                    return;
                }
            }

            string collcode = " and r.college_code='" + Convert.ToString(collegecode) + "'";
            string batchyear = " and r.Batch_Year='" + Convert.ToString(batch_year) + "'";
            string degreecode = " and r.degree_code='" + Convert.ToString(degree_code) + "'";
            string sec = string.Empty;
            // term = "and sc.semester='" + dropterm.SelectedItem.Text.ToString() + "'";     

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
                    sec = string.Empty;
                }
            }
            else
            {
                sec = string.Empty;
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

                qry = "SELECT distinct r.Roll_No,R.Stud_Name,a.sex,serialno,r.Reg_No FROM Registration R,Applyn A WHERE R.App_No = A.App_No " + sqlcondition + " and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' " + strorderby + "";//and r.Current_Semester<='" + Convert.ToString(arr_semester.Max()) + "'
            }
            else
            {
                qry = "SELECT distinct r.Roll_No,R.Stud_Name,a.sex,serialno,r.Reg_No FROM Registration R,Applyn A WHERE R.App_No = A.App_No " + sqlcondition + "  and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' ORDER BY serialno";//and r.Current_Semester<='" + Convert.ToString(arr_semester.Max()) + "'
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
            lblpoperr.Text = string.Empty;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            int checkedcount = 0;
            rollnos = string.Empty;
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
                            rollnos = Convert.ToString(FpViewSpread.Sheets[0].Cells[i, 2].Text);
                        }
                        else
                        {
                            rollnos = rollnos + "','" + Convert.ToString(FpViewSpread.Sheets[0].Cells[i, 2].Text);
                        }
                    }
                }
                if (checkedcount == 0)
                {
                    lblpoperr.Text = "Please Select Atleast One Student";
                    popupdiv.Visible = true;
                    return;
                }
                if (rollnos.Trim().Trim(',') != "")
                {
                    LKG_UKG_Format(rollnos.Trim().Trim(','));
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
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Popup Error

    #endregion Button Click

    #region Reusable Methods

    //Developed By Selvam
    public string ToRoman(string part)
    {
        string roman = string.Empty;
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

    public void ChangeHeaderName(bool isschool)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
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

    public string loadmarkat(string mr)
    {
        string strgetval = string.Empty;
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            strgetval = string.Empty;
            mr = mr.Trim();
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
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            return "";
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
            ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");
            mmyycount = ds2.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
            if (rows_count == 0)
            {
                hat.Clear();
                hat.Add("degree_code", int.Parse(degree_code));
                hat.Add("sem", int.Parse(currentsem));
                hat.Add("from_date", Convert.ToString(frdate));
                hat.Add("to_date", Convert.ToString(todate));
                hat.Add("coll_code", int.Parse(Convert.ToString(collegecode)));

                int iscount = 0;
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + Convert.ToString(frdate) + "' and '" + Convert.ToString(todate) + "' and degree_code=" + degree_code + " and semester=" + currentsem + "";
                DataSet dsholiday = da.select_method_wo_parameter(sqlstr_holiday, "Text");
                if (dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(Convert.ToString(dsholiday.Tables[0].Rows[0]["cnt"]));
                }
                hat.Add("iscount", iscount);

                ds3 = da.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");

                hat.Clear();
                hat.Add("degree_code", degree_code);
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
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

    public void ToConvertedMark(string txtConvertTo, ref string maxMark, ref string obtainedMark, ref string minMark)
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
                    Grade = (Convert.ToString(Math.Round(Convert.ToDouble(Grade), 2, MidpointRounding.AwayFromZero)));


                    return false;
                }
            }
            else
            {
                Grade = obtainedmarks;
                Grade = (Convert.ToString(Math.Round(Convert.ToDouble(Grade), 2, MidpointRounding.AwayFromZero)));

                result = false;
                return false;
            }
            //result = true;
        }
        else
        {
            Grade = obtainedmarks;
            Grade = (Convert.ToString(Math.Round(Convert.ToDouble(Grade), 2, MidpointRounding.AwayFromZero)));

            result = false;
        }
        return result;
    }

    public void LKG_UKG_Format(string roll_no)
    {
        try
        {
            DataSet ds_studmrk = new DataSet();

            Font fontCol_Name = new Font("Book Antiqua", 16, FontStyle.Bold);
            Font fontclgAddrHeader = new Font("Book Antiqua", 11, FontStyle.Regular);
            Font fontmin_bold = new Font("Book Antiqua", 11, FontStyle.Bold);

            Font fontclgReportHeader = new Font("Book Antiqua", 13, FontStyle.Bold);
            Font fontstudClass = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font fontReportContent = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font fontStudentDetailsContent = new Font("Book Antiqua", 10, FontStyle.Regular);
            Font fontReportStudProfileHeader = new Font("Book Antiqua", 12, FontStyle.Bold);

            int selsem = 0;
            int[] arr_semester = new int[1];
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;

            string batch = string.Empty;
            string degreecode1 = string.Empty;
            string semester1 = string.Empty;
            string sections = string.Empty;
            int[] arr_test = new int[1];
            string[] arr_testname = new string[1];
            string test = string.Empty;
            int coltop = 5;

            bool status = false;
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            batch = Convert.ToString(ddlbatch.SelectedValue);
            degreecode1 = Convert.ToString(ddlDept.SelectedValue);
            sections = Convert.ToString(ddlsec.SelectedValue);
            batch_year = batch;
            degree_code = degreecode1;
            foreach (ListItem li in cbl_sem.Items)
            {
                if (li.Selected)
                {
                    if (selsem != 0)
                        Array.Resize(ref arr_semester, selsem + 1);
                    int.TryParse(li.Value, out arr_semester[selsem]);
                    selsem++;
                    if (semester1 == "")
                    {
                        semester1 = li.Value;
                    }
                    else
                    {
                        semester1 += "','" + li.Value;
                    }
                }
            }
            int selct_test = 0;
            if (Cbl_test.Items.Count == 0)
            {
                lblpoperr.Text = "Test is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                foreach (ListItem lis in Cbl_test.Items)
                {
                    if (lis.Selected)
                    {
                        if (selct_test != 0)
                        {
                            Array.Resize(ref arr_testname, selct_test + 1);
                            Array.Resize(ref arr_test, selct_test + 1);
                        }
                        int.TryParse(lis.Value, out arr_test[selct_test]);
                        arr_testname[selct_test] = lis.Text;
                        selct_test++;
                        if (test == "")
                        {
                            test = lis.Value;
                        }
                        else
                        {
                            test += "," + lis.Value;
                        }
                    }
                }
                if (selct_test == 0)
                {
                    lblpoperr.Text = "Please Select Atleast One Test";
                    popupdiv.Visible = true;
                    return;
                }
            }

            StringBuilder sbErr = new StringBuilder();

            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;

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

            string sec = string.Empty;
            string subsec = string.Empty;
            if (ddlsec.Enabled == true)
            {
                if (ddlsec.SelectedItem.Text.ToLower().Trim() != "all" && ddlsec.SelectedItem.Text.ToLower().Trim() != "" && ddlsec.SelectedItem.Text.ToLower().Trim() != "-1")
                {
                    sec = "and rg.Sections in ('" + Convert.ToString(ddlsec.SelectedItem.Text) + "')";
                    subsec = "and e.Sections in ('" + Convert.ToString(ddlsec.SelectedItem.Text) + "')";
                }
                else
                {
                    sec = string.Empty;
                    subsec = string.Empty;
                }
            }
            else
            {
                sec = string.Empty;
                subsec = string.Empty;
            }

            if (rollnos != "")
            {
                qry = "SELECT collname,affliatedby,address1,address2,district,address3,pincode,email,logo1,logo2,website from collinfo where college_code='" + collegecode + "';";
                if (serialflag == false)
                {
                    qry += "select r.serialno,r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,case when (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp)<>'' then (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp) when convert(varchar(20),a.Cityp)='-1' then '' else convert(varchar(20),a.Cityp)  end as Cityp,parent_pincodep,student_mobile,parentF_Mobile,StuPer_Id,parent_addressc,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,case when (select TextVal from TextValTable tt where TextCriteria='bgrou' and convert(varchar(20),tt.TextCode)=a.bldgrp)<>'' then (select TextVal from TextValTable tt where TextCriteria='bgrou' and convert(varchar(20),tt.TextCode)=a.bldgrp) when convert(varchar(20),a.bldgrp)='-1' then '' else convert(varchar(20),a.bldgrp)  end as Blood_Grp,studhouse,case when (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp)<>'' then (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp) when convert(varchar(20),a.Districtp)='-1' then '' else convert(varchar(20),a.Districtp) end as Districtp,case when (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep)<>'' then (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep) when convert(varchar(20),a.parent_statep)='-1' then '' else convert(varchar(20),a.parent_statep)  end as parent_statep,parent_pincodep,parentM_Mobile,case when (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp)<>'' then (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp) when convert(varchar(20),a.countryp)='-1' then '' else convert(varchar(20),a.countryp)  end as countryp,serialno,emailM,ParentidP,guardian_name,guardian_mobile,gurdian_email,emailp from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No in ('" + roll_no + "') " + strorderby + " ;";
                }
                else
                {
                    qry += "select r.serialno,r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,case when (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp)<>'' then (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp) when convert(varchar(20),a.Cityp)='-1' then '' else convert(varchar(20),a.Cityp)  end as Cityp,parent_pincodep,student_mobile,parentF_Mobile,StuPer_Id,parent_addressc,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,case when (select TextVal from TextValTable tt where TextCriteria='bgrou' and convert(varchar(20),tt.TextCode)=a.bldgrp)<>'' then (select TextVal from TextValTable tt where TextCriteria='bgrou' and convert(varchar(20),tt.TextCode)=a.bldgrp) when convert(varchar(20),a.bldgrp)='-1' then '' else convert(varchar(20),a.bldgrp)  end as Blood_Grp,studhouse,case when (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp)<>'' then (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp) when convert(varchar(20),a.Districtp)='-1' then '' else convert(varchar(20),a.Districtp) end as Districtp,case when (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep)<>'' then (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep) when convert(varchar(20),a.parent_statep)='-1' then '' else convert(varchar(20),a.parent_statep)  end as parent_statep,parent_pincodep,parentM_Mobile,case when (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp)<>'' then (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp) when convert(varchar(20),a.countryp)='-1' then '' else convert(varchar(20),a.countryp)  end as countryp,serialno,emailM,ParentidP,guardian_name,guardian_mobile,gurdian_email,emailp from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code  and r.Roll_No in ('" + roll_no + "') order by serialno";
                }
                ds.Clear();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(qry, "Text");

                ds_studmrk.Clear();
                string selcqury = " select distinct s.subject_name,s.subject_code,e.sections from Exam_type e,subject s,CriteriaForInternal c,syllabus_master sm where sm.syll_code=s.syll_code and c.syll_code=sm.syll_code and c.syll_code=s.syll_code and c.Criteria_no=e.criteria_no and s.subject_no=e.subject_no and sm.degree_code='" + degreecode1 + "' and sm.Batch_Year='" + batch + "' and semester in ('" + semester1 + "') and sm.Batch_Year=e.batch_year and c.Criteria_no in (" + test + ") " + subsec + " order by s.subject_code;";
                selcqury = selcqury + " select r.roll_no,rg.Reg_No,rg.Stud_Name,rg.Roll_Admit,rg.degree_code,rg.Current_Semester,sm.semester,c.Criteria_no,c.criteria,e.exam_code,c.max_mark as Convert_Maxmark,c.min_mark Convert_Minmark,e.max_mark as Conducted_max,e.min_mark as Conduct_Minmark,s.subject_code,s.subject_no,s.subType_no,s.syll_code,s.subject_name,r.marks_obtained,isnull(r.remarks,'') as remarks,convert(varchar(10),e.exam_date,101)as exam_date from Registration rg,CriteriaForInternal c,Exam_type e,Result r,syllabus_master sm,subject s where rg.Roll_No =r.roll_no and c.Criteria_no=e.criteria_no and s.subject_no=e.subject_no  and e.exam_code=r.exam_code and e.batch_year=rg.Batch_Year and e.sections=rg.Sections and sm.Batch_Year=rg.Batch_Year and rg.degree_code=sm.degree_code and sm.syll_code=s.syll_code and sm.syll_code=c.syll_code and e.batch_year=sm.Batch_Year and rg.Batch_Year='" + batch + "' and rg.degree_code='" + degreecode1 + "' and rg.college_code='" + collegecode + "' " + sec + " and cc=0 and delflag=0 and exam_flag<>'Debar' and c.criteria_no in(" + test + ") and sm.semester in ('" + semester1 + "') and rg.Roll_No in('" + roll_no + "') order by rg.Roll_No,c.Criteria_no,s.subject_code;  select Frange,Trange,Mark_Grade,Grade_Master.Credit_Points from Grade_Master where College_Code='" + collegecode + "' and batch_year='" + batch + "' and Degree_Code='" + degreecode1 + "' ;";
                ds_studmrk = d2.select_method_wo_parameter(selcqury, "Text");

                string rmark_qry = "SELECT * from CoCurrActivitie_Det where istype='remks' and Degree_Code='" + degreecode1 + "' and batch_year='" + batch + "' and term in('" + semester1 + "') and Roll_No in('" + roll_no + "');";
                DataSet rmk = new DataSet();
                rmk = d2.select_method_wo_parameter(rmark_qry, "Text");

                if (ds_studmrk.Tables.Count > 0 && ds_studmrk.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables.Count == 2 && ds.Tables[1].Rows.Count > 0)
                        {
                            for (int studrow = 0; studrow < ds.Tables[1].Rows.Count; studrow++)
                            {
                                coltop = 5;
                                bool staus = false;

                                PdfRectangle pdfMainRect;
                                PdfTextArea pdftext;
                                PdfLine pdfline;
                                Gios.Pdf.PdfTablePage tblPage;

                                string strexam = string.Empty;
                                string Roll_No = Convert.ToString(ds.Tables[1].Rows[studrow]["Roll_No"]).Trim();
                                string stud_app_no = Convert.ToString(ds.Tables[1].Rows[studrow]["App_No"]).Trim();
                                string section1 = Convert.ToString(ds.Tables[1].Rows[studrow]["Sections"]).Trim();
                                string stu_hight = Convert.ToString(ds.Tables[1].Rows[studrow]["StudHeight"]).Trim();
                                string stu_wight = Convert.ToString(ds.Tables[1].Rows[studrow]["StudWeight"]).Trim();

                                DataView dvsec = new DataView();
                                DataTable studmarks = new DataTable();
                                DataTable dtStudMarks = new DataTable();
                                DataView dvStudMark = new DataView();

                                if (ds_studmrk.Tables.Count > 0 && ds_studmrk.Tables[0].Rows.Count > 0)
                                {
                                    ds_studmrk.Tables[0].DefaultView.RowFilter = "Sections='" + section1 + "'";
                                    dvsec = ds_studmrk.Tables[0].DefaultView;
                                    dvsec.Sort = "subject_code,subject_name";
                                    studmarks = dvsec.ToTable(true, "subject_name");
                                    if (ds_studmrk.Tables.Count >= 2 && ds_studmrk.Tables[1].Rows.Count > 0)
                                    {
                                        ds_studmrk.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                                        dvStudMark = ds_studmrk.Tables[1].DefaultView;
                                        dtStudMarks = dvStudMark.ToTable();
                                        if (dtStudMarks.Rows.Count > 0)
                                        {
                                            staus = true;
                                        }
                                    }
                                }
                                DataView dv = new DataView();
                                DataTable dtStudInfo = new DataTable();
                                ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                                dv = ds.Tables[1].DefaultView;
                                dtStudInfo = dv.ToTable();

                                PdfImage CollegeLeftLogo = null;
                                PdfImage collegeRightLogo = null;
                                PdfImage studPhoto = null;
                                if (dtStudInfo.Rows.Count > 0)
                                {
                                    if (dtStudMarks.Rows.Count > 0)
                                    {
                                        DataTable dtcolinfo = new DataTable();
                                        dtcolinfo = ds.Tables[0];
                                        string stdphtsql = "select * from StdPhoto where app_no='" + stud_app_no + "'";
                                        MemoryStream memoryStream = new MemoryStream();
                                        DataSet dsstdpho = new DataSet();
                                        dsstdpho.Clear();
                                        dsstdpho.Dispose();
                                        dsstdpho = d2.select_method_wo_parameter(stdphtsql, "Text");
                                        if (dsstdpho.Tables.Count > 0 && dsstdpho.Tables[0].Rows.Count > 0)
                                        {
                                            byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stud_app_no + ".jpeg")))
                                                {
                                                }
                                                else
                                                {
                                                    thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + stud_app_no + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                }
                                            }
                                        }

                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stud_app_no + ".jpeg")))
                                        {
                                            studPhoto = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/" + stud_app_no + ".jpeg"));
                                        }
                                        else
                                        {
                                            studPhoto = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/NoImage.jpg"));
                                        }
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collegecode.ToString() + ".jpeg")))
                                        {
                                            byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collegecode.ToString() + ".jpeg")))
                                                {
                                                }
                                                else
                                                {
                                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collegecode.ToString() + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                }
                                            }
                                        }
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collegecode.ToString() + ".jpeg")))
                                        {

                                        }
                                        else
                                        {
                                            CollegeLeftLogo = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collegecode.ToString() + ".jpeg"));
                                        }

                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + collegecode.ToString() + ".jpeg")))
                                        {
                                            byte[] file = (byte[])ds.Tables[0].Rows[0]["logo2"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + collegecode.ToString() + ".jpeg")))
                                                {
                                                }
                                                else
                                                {
                                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + collegecode.ToString() + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                }
                                            }
                                        }
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + collegecode.ToString() + ".jpeg")))
                                        {
                                        }
                                        else
                                        {
                                            collegeRightLogo = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + collegecode.ToString() + ".jpeg"));
                                        }

                                        #region PAGE 1

                                        if (staus)
                                        {
                                            status = true;
                                        }

                                        #endregion PAGE 1

                                        #region PAGE 2

                                        coltop = 0;
                                        if (staus)
                                            status = true;
                                        mypdfpage = mydoc.NewPage();
                                        object minExmDate = string.Empty;

                                        double[] tot_mark = new double[arr_testname.Length];
                                        double[] div_sub = new double[arr_testname.Length];

                                        strexam = "OVERALL PERFORMANCE";

                                        int rows_count = Convert.ToInt32(studmarks.Rows.Count);

                                        Gios.Pdf.PdfTable table1marks1 = mydoc.NewTable(fontReportContent, rows_count + 8, selct_test + 1, 5);
                                        table1marks1.VisibleHeaders = false;
                                        table1marks1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                        table1marks1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1marks1.Cell(0, 0).SetContent("OVERALL PERFORMANCE");
                                        table1marks1.Cell(0, 0).SetForegroundColor(Color.Black);
                                        foreach (PdfCell pr in table1marks1.CellRange(0, 0, 0, 0).Cells)
                                        {
                                            pr.ColSpan = selct_test + 1;
                                        }
                                        table1marks1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1marks1.Cell(1, 0).SetContent("Subjects");
                                        table1marks1.Cell(1, 0).SetForegroundColor(Color.Black);
                                        table1marks1.Cell(1, 0).SetFont(fontstudClass);
                                        int[] columnwidth = new int[arr_test.Length];
                                        int tnm = 0;
                                        for (int i = 0; i < arr_test.Length; i++)
                                        {
                                            tnm++;
                                            table1marks1.Cell(1, tnm).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            int width = (Convert.ToString(arr_testname[i]).Trim().Length + 10) * 10 + 10;
                                            columnwidth[i] = width;
                                            table1marks1.Columns[tnm].SetWidth(width);
                                            table1marks1.Cell(1, tnm).SetContent(Convert.ToString(arr_testname[i]).Trim());
                                            table1marks1.Cell(1, tnm).SetForegroundColor(Color.Black);
                                            table1marks1.Cell(1, tnm).SetFont(fontstudClass);
                                        }
                                        table1marks1.Columns[0].SetWidth(columnwidth.Max() * 2 + 10);
                                        string Roll = Convert.ToString(ds.Tables[1].Rows[studrow]["Roll_No"]);
                                        DataView dvs = new DataView();
                                        DataTable dtstud_mark = new DataTable();
                                        ds_studmrk.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll + "'";
                                        dvs = ds_studmrk.Tables[1].DefaultView;
                                        dtstud_mark = dvs.ToTable();

                                        if (dvs.Count > 0)
                                        {
                                            DataView dv_subj = new DataView();
                                            for (int i = 0; i < studmarks.Rows.Count; i++)
                                            {
                                                dtstud_mark.DefaultView.RowFilter = "subject_name='" + Convert.ToString(studmarks.Rows[i]["subject_name"]) + "' ";
                                                dv_subj = dtstud_mark.DefaultView;
                                                DataTable studmark = new DataTable();
                                                studmark = dv_subj.ToTable();
                                                DataView dv_grad = new DataView();
                                                table1marks1.Cell(i + 2, 0).SetContent(Convert.ToString(studmarks.Rows[i]["subject_name"]));
                                                table1marks1.Cell(i + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1marks1.Cell(i + 2, 0).SetForegroundColor(Color.Black);
                                                table1marks1.Cell(i + 2, 0).SetFont(fontstudClass);
                                                if (dv_subj.Count > 0)
                                                {
                                                    for (int j = 0; j < arr_testname.Length; j++)
                                                    {
                                                        studmark.DefaultView.RowFilter = "Criteria_no='" + Convert.ToString(arr_test[j]) + "'";
                                                        dv_grad = studmark.DefaultView;
                                                        table1marks1.Cell(i + 2, j + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        String minmark = "0";
                                                        String obtmark = string.Empty;
                                                        string conductedmax = string.Empty;
                                                        String Toconvert = string.Empty;
                                                        string ConductMaxmark = string.Empty;
                                                        double obtmark1 = 0;
                                                        if (dv_grad.Count > 0)
                                                        {
                                                            Toconvert = Convert.ToString(dv_grad[0]["Convert_Maxmark"]);
                                                            conductedmax = Convert.ToString(dv_grad[0]["Conducted_max"]);
                                                            obtmark = Convert.ToString(dv_grad[0]["marks_obtained"]);
                                                            obtmark1 = Convert.ToDouble(obtmark);
                                                            string grade = string.Empty;
                                                            bool res = true;
                                                            if (obtmark1 < 0)
                                                            {
                                                                res = false;
                                                                grade = loadmarkat(obtmark);
                                                                table1marks1.Cell(i + 2, j + 1).SetForegroundColor(Color.Red);
                                                            }
                                                            else
                                                            {
                                                                double mark = obtmark1;
                                                                tot_mark[j] += mark;
                                                                ToConvertedMark(Convert.ToString(Toconvert), ref conductedmax, ref obtmark, ref minmark);
                                                                if (ds_studmrk.Tables.Count == 3 && ds_studmrk.Tables[2].Rows.Count > 0)
                                                                {
                                                                    findgrade(ds_studmrk.Tables[2], obtmark, ref grade);
                                                                }
                                                                else
                                                                {
                                                                    grade = obtmark;
                                                                }
                                                                table1marks1.Cell(i + 2, j + 1).SetForegroundColor(Color.Black);
                                                            }
                                                            table1marks1.Cell(i + 2, j + 1).SetContent(grade);
                                                            table1marks1.Cell(i + 2, j + 1).SetFont(fontReportContent);
                                                        }
                                                        else
                                                        {
                                                            table1marks1.Cell(i + 2, j + 1).SetContent("--");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        rows_count++;
                                        table1marks1.Cell(rows_count + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1marks1.Cell(rows_count + 1, 0).SetContent("OVERALL GRADE");
                                        table1marks1.Cell(rows_count + 1, 0).SetForegroundColor(Color.Black);
                                        table1marks1.Cell(rows_count + 1, 0).SetFont(fontstudClass);

                                        table1marks1.Cell(rows_count + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1marks1.Cell(rows_count + 2, 0).SetContent("Attendance");
                                        table1marks1.Cell(rows_count + 2, 0).SetForegroundColor(Color.Black);
                                        table1marks1.Cell(rows_count + 2, 0).SetFont(fontstudClass);

                                        string admitdate = Convert.ToString(ds.Tables[1].Rows[0]["adm_date"]);
                                        double[] tst_attendece = new double[arr_testname.Length];
                                        for (int grd = 0; grd < arr_test.Length; grd++)
                                        {
                                            ds_studmrk.Tables[1].DefaultView.RowFilter = " Criteria_no='" + arr_test[grd] + "'";

                                            DataView dvUniqueSubjects = new DataView();
                                            dvUniqueSubjects = ds_studmrk.Tables[1].DefaultView;
                                            DataTable dtee = ds_studmrk.Tables[1].DefaultView.ToTable(true, "subject_name", "Criteria_no", "semester");
                                            minExmDate = dvUniqueSubjects.ToTable(true, "subject_name", "exam_date", "Criteria_no").Compute("Min(exam_date)", "Criteria_no='" + arr_test[grd] + "'");
                                            if (dtee.Rows.Count > 0)
                                            {
                                                double overalgrad = tot_mark[grd] / dtee.Rows.Count;
                                                table1marks1.Cell(rows_count - 5, grd + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                string grade = string.Empty;
                                                findgrade(ds_studmrk.Tables[2], Convert.ToString(overalgrad), ref grade);

                                                table1marks1.Cell(rows_count + 1, grd + 1).SetContent(grade);
                                                table1marks1.Cell(rows_count + 1, grd + 1).SetForegroundColor(Color.Black);
                                                table1marks1.Cell(rows_count + 1, grd + 1).SetFont(fontReportContent);
                                                string term_sem = Convert.ToString(dtee.Rows[0]["semester"]);
                                                string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + term_sem + "' and degree_code='" + degree_code + "' and batch_year='" + batch_year + "'";
                                                DataSet dsSem = d2.select_method_wo_parameter(sem, "Text");
                                                if (dsSem.Tables[0].Rows.Count > 0)
                                                {
                                                    string startdate = Convert.ToString(dsSem.Tables[0].Rows[0]["start_date"]);
                                                    string enddate = string.Empty;
                                                    if (Convert.ToString(minExmDate).Trim() != "")
                                                    {
                                                        enddate = Convert.ToString(minExmDate);
                                                        DateTime dtend = new DateTime();
                                                        DateTime.TryParseExact(enddate, "MM/dd/yyyy", null, DateTimeStyles.None, out dtend);
                                                        enddate = dtend.ToString("yyyy/MM/dd");
                                                    }
                                                    else
                                                        enddate = Convert.ToString(dsSem.Tables[0].Rows[0]["end_date"]);
                                                    currentsem = term_sem;
                                                    persentmonthcal(Roll_No, admitdate, startdate, enddate);

                                                    string dum_tage_date;
                                                    double per_tage_date = ((pre_present_date / per_workingdays) * 100);
                                                    if (per_tage_date > 100)
                                                    {
                                                        per_tage_date = 100;
                                                    }

                                                    dum_tage_date = String.Format("{0:0,0.00}", float.Parse(per_tage_date.ToString()));
                                                    if (dum_tage_date == "NaN")
                                                    {
                                                        dum_tage_date = "0";
                                                    }
                                                    else if (dum_tage_date == "Infinity")
                                                    {
                                                        dum_tage_date = "0";
                                                    }

                                                    table1marks1.Cell(rows_count + 2, grd + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table1marks1.Cell(rows_count + 2, grd + 1).SetContent(Convert.ToString(dum_tage_date));
                                                    table1marks1.Cell(rows_count + 2, grd + 1).SetForegroundColor(Color.Black);
                                                    double atendancpersent = Convert.ToDouble(dum_tage_date);
                                                    table1marks1.Cell(rows_count + 2, grd + 1).SetContent(Convert.ToString(Math.Round(atendancpersent, 1, MidpointRounding.AwayFromZero)) + "%");
                                                }
                                            }
                                        }

                                        table1marks1.Cell(rows_count + 3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1marks1.Cell(rows_count + 3, 0).SetContent("Signature of Class Teacher");
                                        table1marks1.Cell(rows_count + 3, 0).SetForegroundColor(Color.Black);
                                        table1marks1.Cell(rows_count + 3, 0).SetFont(fontstudClass);

                                        table1marks1.Cell(rows_count + 4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1marks1.Cell(rows_count + 4, 0).SetContent("Signature Principal");
                                        table1marks1.Cell(rows_count + 4, 0).SetForegroundColor(Color.Black);
                                        table1marks1.Cell(rows_count + 4, 0).SetFont(fontstudClass);

                                        table1marks1.Cell(rows_count + 5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1marks1.Cell(rows_count + 5, 0).SetContent("Signature Parent");
                                        table1marks1.Cell(rows_count + 5, 0).SetForegroundColor(Color.Black);
                                        table1marks1.Cell(rows_count + 5, 0).SetFont(fontstudClass);

                                        table1marks1.Cell(rows_count + 6, 0).SetContent("Five Point Grading Scale\t:\t A*\t-\tOutstanding\t: 90%-100%;\t A\t-\t Excellent\t:\t75%-89%;\nB\t-\tVery Good:\t56%-74%;\tC\t-\tGood\t:\t35%-55%;\t D\t-\tScope for improvement\t:\t(Below 35%);");
                                        table1marks1.Cell(rows_count + 6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                        table1marks1.Cell(rows_count + 6, 0).SetForegroundColor(Color.Black);
                                        table1marks1.Cell(rows_count + 6, 0).SetFont(fontstudClass);

                                        foreach (PdfCell pr in table1marks1.CellRange(rows_count + 6, 0, rows_count + 6, 0).Cells)
                                        {
                                            pr.ColSpan = selct_test + 1;
                                        }

                                        CommonFrontPage(mypdfpage, mydoc: mydoc, dtStudInfo: dtStudInfo, dtcolinfo: ds.Tables[0], status: ref staus, coltop: out coltop, pdfMarkTable: table1marks1, colLeftLogo: CollegeLeftLogo, studPhoto: studPhoto, colRightLogo: collegeRightLogo);

                                        #endregion PAGE 2

                                        #region PAGE 3

                                        mypdfpage = mydoc.NewPage();

                                        pdfMainRect = new PdfRectangle(mydoc, new PdfArea(mydoc, 15, 15, mydoc.PageWidth - 30, mydoc.PageHeight - 30), Color.Black, 1);
                                        mypdfpage.Add(pdfMainRect);
                                        mypdfpage.Add(pdfMainRect);
                                        coltop = 30;

                                        strexam = "TEACHER'S REMARKS";
                                        pdftext = new PdfTextArea(fontCol_Name, System.Drawing.Color.Black, new PdfArea(mydoc, 2, coltop, mydoc.PageWidth, 20), System.Drawing.ContentAlignment.MiddleCenter, strexam);
                                        mypdfpage.Add(pdftext);

                                        pdfline = new PdfLine(mydoc, new PointF((float)(mydoc.PageWidth / 2 - 85), coltop + 20), new PointF((float)((mydoc.PageWidth / 2 + 90)), coltop + 20), Color.Black, 1);

                                        mypdfpage.Add(pdfline);


                                        int cnt = 0;
                                        for (int i = 0; i < arr_semester.Length; i++)
                                        {
                                            if (cnt < 2)
                                            {
                                                cnt++;
                                                strexam = ((b_school) ? "TERM" : "Semester") + " -" + ToRoman(Convert.ToString(arr_semester[i]));
                                                pdftext = new PdfTextArea(fontCol_Name, System.Drawing.Color.Black, new PdfArea(mydoc, 40, ((i == 0) ? coltop + 30 : coltop + 10), mydoc.PageWidth - 80, 20), System.Drawing.ContentAlignment.TopLeft, strexam);
                                                mypdfpage.Add(pdftext);
                                                DataView remark = new DataView();
                                                string[] Remarkline = new string[4] { "", "", "", "" };
                                                int remark_row = 1;
                                                if (rmk.Tables[0].Rows.Count > 0)
                                                {
                                                    rmk.Tables[0].DefaultView.RowFilter = " Roll_No='" + Roll_No + "' and  term='" + arr_semester[i] + "'";
                                                    remark = rmk.Tables[0].DefaultView;
                                                    if (remark.Count > 0)
                                                    {
                                                        string student_remark = Convert.ToString(remark[0]["totatt_remarks"]).Trim();
                                                        int remark_length = student_remark.Length;
                                                        if (remark_length <= 85)
                                                        {
                                                            remark_row = 1;
                                                            Remarkline[0] = student_remark.Substring(0, remark_length);
                                                        }
                                                        else if (remark_length > 85 && remark_length <= 170)
                                                        {
                                                            remark_row = 2;
                                                            Remarkline[0] = student_remark.Substring(0, 85);
                                                            Remarkline[1] = student_remark.Substring(85, remark_length - 85);
                                                        }
                                                        else if (remark_length > 170 && remark_length <= 255)
                                                        {
                                                            remark_row = 3;
                                                            Remarkline[0] = student_remark.Substring(0, 85);
                                                            Remarkline[1] = student_remark.Substring(85, 85);
                                                            Remarkline[2] = student_remark.Substring(170, remark_length - 170);
                                                        }
                                                        else if (remark_length > 255 && remark_length <= 340)
                                                        {
                                                            remark_row = 4;
                                                            Remarkline[0] = student_remark.Substring(0, 85);
                                                            Remarkline[1] = student_remark.Substring(85, 85);
                                                            Remarkline[2] = student_remark.Substring(170, 85);
                                                            Remarkline[3] = student_remark.Substring(255, remark_length - 255);
                                                        }
                                                        else if (remark_length > 340)
                                                        {
                                                            remark_row = 4;
                                                            Remarkline[0] = student_remark.Substring(0, 85);
                                                            Remarkline[1] = student_remark.Substring(89, 85);
                                                            Remarkline[2] = student_remark.Substring(170, 85);
                                                            Remarkline[3] = student_remark.Substring(255, (((remark_length - 255) > 85) ? 85 : remark_length - 255));
                                                        }
                                                    }
                                                }

                                                Gios.Pdf.PdfTable tbl_remark = mydoc.NewTable(fontReportContent, remark_row, 1, 7);
                                                tbl_remark.VisibleHeaders = false;
                                                tbl_remark.SetBorders(Color.Black, 1, BorderType.None);
                                                if (i == 0)
                                                    coltop += 50;
                                                else
                                                    coltop += 30;

                                                for (int rr = 0; rr < remark_row; rr++)
                                                {
                                                    tbl_remark.Cell(rr, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    tbl_remark.Cell(rr, 0).SetContent(Remarkline[rr].Trim());
                                                    tbl_remark.Cell(rr, 0).SetForegroundColor(Color.Black);
                                                    tbl_remark.Cell(rr, 0).SetFont(fontstudClass);
                                                }

                                                tblPage = tbl_remark.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, coltop, mydoc.PageWidth - 80, 150));
                                                mypdfpage.Add(tblPage);
                                                for (int index = tblPage.FirstRow; index <= tblPage.LastRow; index++)
                                                {
                                                    PdfLine pc = tblPage.CellArea(index, 0).LowerBound(Color.Black, 1);
                                                    mypdfpage.Add(pc);
                                                }

                                                double heights = tblPage.Area.Height;
                                                coltop += Convert.ToInt32(heights) + 20;

                                            }
                                        }

                                        strexam = "HEALTH";

                                        pdftext = new PdfTextArea(fontCol_Name, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop, mydoc.PageWidth - 80, 20), System.Drawing.ContentAlignment.TopLeft, strexam);
                                        mypdfpage.Add(pdftext);


                                        Gios.Pdf.PdfTable tbl_health = mydoc.NewTable(fontReportContent, 3, selsem + 1, 10);
                                        tbl_health.VisibleHeaders = false;
                                        tbl_health.SetColumnsWidth(new int[] { 50 });
                                        tbl_health.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        coltop += 20;

                                        tbl_health.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tbl_health.Cell(0, 0).SetContent("Aspects");
                                        tbl_health.Cell(0, 0).SetForegroundColor(Color.Black);
                                        tbl_health.Cell(0, 0).SetFont(fontstudClass);

                                        int trm = 0;
                                        for (int i = 0; i < arr_semester.Length; i++)
                                        {
                                            trm++;
                                            strexam = "TERM -" + ToRoman(Convert.ToString(arr_semester[i]));

                                            tbl_health.Cell(0, trm).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tbl_health.Cell(0, trm).SetContent("TERM -" + ToRoman(Convert.ToString(arr_semester[i])));
                                            tbl_health.Cell(0, trm).SetFont(fontstudClass);

                                            tbl_health.Cell(0, trm).SetForegroundColor(Color.Black);
                                            tbl_health.Cell(1, trm).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tbl_health.Cell(1, trm).SetContent(stu_hight);
                                            tbl_health.Cell(1, trm).SetForegroundColor(Color.Black);

                                            tbl_health.Cell(2, trm).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tbl_health.Cell(2, trm).SetContent(stu_wight);
                                            tbl_health.Cell(2, trm).SetForegroundColor(Color.Black);

                                        }

                                        tbl_health.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tbl_health.Cell(1, 0).SetContent("Height(Cms)");
                                        tbl_health.Cell(1, 0).SetForegroundColor(Color.Black);

                                        tbl_health.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tbl_health.Cell(2, 0).SetContent("Weight(Kgs)");
                                        tbl_health.Cell(2, 0).SetForegroundColor(Color.Black);

                                        tblPage = tbl_health.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, coltop, mydoc.PageWidth - 80, 150));
                                        mypdfpage.Add(tblPage);

                                        Gios.Pdf.PdfTable tbl_signatur = mydoc.NewTable(fontReportContent, 3, 4, 10);
                                        tbl_signatur.VisibleHeaders = false;
                                        tbl_signatur.SetColumnsWidth(new int[] { 100 });
                                        tbl_signatur.SetBorders(Color.Black, 1, BorderType.None);
                                        double height = tblPage.Area.Height;
                                        coltop += Convert.ToInt32(height) + 10;

                                        tbl_signatur.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tbl_signatur.Cell(0, 0).SetContent("Signature of Class Teacher");
                                        tbl_signatur.Cell(0, 0).SetForegroundColor(Color.Black);

                                        tbl_signatur.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tbl_signatur.Cell(1, 0).SetContent("Signature of Principal");
                                        tbl_signatur.Cell(1, 0).SetForegroundColor(Color.Black);

                                        tbl_signatur.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tbl_signatur.Cell(2, 0).SetContent("Signature of Parent");
                                        tbl_signatur.Cell(2, 0).SetForegroundColor(Color.Black);
                                        coltop += 5;

                                        tblPage = tbl_signatur.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, coltop, mydoc.PageWidth - 100, 400));
                                        mypdfpage.Add(tblPage);

                                        coltop -= 30;
                                        for (int col = 0; col < 3; col++)
                                        {
                                            coltop += 34;
                                            int rect_width = 110;
                                            for (int rectwidth = 0; rectwidth < arr_semester.Length; rectwidth++)
                                            {
                                                rect_width += 110;
                                                pdfMainRect = new PdfRectangle(mydoc, new PdfArea(mydoc, rect_width, coltop, 90, 25), Color.Black);
                                                mypdfpage.Add(pdfMainRect);
                                            }
                                        }
                                        if (staus)
                                        {
                                            status = true;
                                            mypdfpage.SaveToDocument();
                                        }

                                        #endregion PAGE 3

                                    }
                                    else
                                    {
                                        sbErr.Append("\nRoll Number " + Roll_No + " does not have Marks. Please Check Mark Entry!!!");
                                    }
                                }
                                else
                                {
                                    if (dtStudInfo.Rows.Count == 0)
                                        sbErr.Append("\nRoll Number " + Roll_No + " does not Exists.");
                                }
                            }
                        }
                        else
                        {

                            sbErr.Append("There Is No Student Were Found");
                        }
                    }
                    else
                    {
                        sbErr.Append("No College Were Found");
                    }
                }
                else
                {
                    sbErr.Append("No Test Were Conducted");
                }
            }
            else
            {
                sbErr.Append("Please Select Atleast One Student");
            }
            if (sbErr.Length > 0)
            {
                lblErrSearch.Text = sbErr.ToString();
                lblErrSearch.Visible = true;
            }
            else
            {
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
            }
            if (status == true)
            {
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "LKG_UKG_ReportCard" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
            else
            {
                lblpoperr.Text = "There is No Report Card Generated";
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

    public void CommonFrontPage(PdfPage mypdfpage, PdfDocument mydoc, DataTable dtStudInfo, DataTable dtcolinfo, ref bool status, out int coltop, PdfTable pdfMarkTable, int type = 0, PdfImage colLeftLogo = null, PdfImage studPhoto = null, PdfImage colRightLogo = null, int[] semester = null)
    {
        coltop = 0;
        try
        {
            #region Font Creation

            Font fontCol_Name = new Font("Book Antiqua", 16, FontStyle.Bold);
            Font fontclgAddrHeader = new Font("Book Antiqua", 11, FontStyle.Regular);
            Font fontclgReportHeader = new Font("Book Antiqua", 13, FontStyle.Bold);
            Font fontstudClass = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font fontReportContent = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font fontStudentDetailsContent = new Font("Book Antiqua", 10, FontStyle.Regular);
            Font fontReportStudProfileHeader = new Font("Book Antiqua", 12, FontStyle.Bold);

            #endregion Font Creation

            if (dicHeaderAll.Count == 0)
            {
                reportHeaderBased = 0;
                GetHeaderSettings(ref dicHeaderAll, ref reportHeaderBased);
            }

            if (dicHeaderAll.Count > 0)
            {

            }

            coltop = 15;
            if (dtcolinfo.Rows.Count > 0)
            {
                string clgname = Convert.ToString(dtcolinfo.Rows[0]["collname"]).Trim();
                string clgaff = "(" + Convert.ToString(dtcolinfo.Rows[0]["affliatedby"]) + ")";
                string clgaddress1 = Convert.ToString(dtcolinfo.Rows[0]["address1"]).Trim();
                string clgaddress2 = Convert.ToString(dtcolinfo.Rows[0]["address2"]).Trim();
                string clgaddress3 = Convert.ToString(dtcolinfo.Rows[0]["address3"]).Trim();
                string clgdistrict = Convert.ToString(dtcolinfo.Rows[0]["district"]).Trim();
                string clgpincode = Convert.ToString(dtcolinfo.Rows[0]["pincode"]).Trim();
                string clgemail = "Email : " + Convert.ToString(dtcolinfo.Rows[0]["email"]).Trim();
                string clgwebsite = "Website : " + Convert.ToString(dtcolinfo.Rows[0]["website"]).Trim();
                string clgfulladdress = string.Empty;
                PdfTextArea pdftxt;
                PdfRectangle pdfMainRect;
                PdfRectangle pdfrect;
                PdfLine pdfnewline;
                PdfTable pdfNewTable;
                PdfTablePage pdfNewTablePage;

                #region College Address

                if (clgaddress1.Trim().Trim(',') != "")
                {
                    clgfulladdress = clgaddress1.Trim().Trim(',');
                }
                if (clgaddress2.Trim().Trim(',') != "")
                {
                    if (clgfulladdress != "")
                    {
                        clgfulladdress += ", " + clgaddress2.Trim().Trim(',');
                    }
                    else
                    {
                        clgfulladdress = clgaddress2.Trim().Trim(',');
                    }
                }
                if (clgaddress3.Trim().Trim(',') != "")
                {
                    if (clgfulladdress != "")
                    {
                        clgfulladdress += ", " + clgaddress3.Trim().Trim(',');
                    }
                    else
                    {
                        clgfulladdress = clgaddress3.Trim().Trim(',');
                    }
                }
                if (clgdistrict.Trim().Trim(',') != "")
                {
                    if (clgfulladdress != "")
                    {
                        clgfulladdress += ", " + clgdistrict.Trim().Trim(',');
                    }
                    else
                    {
                        clgfulladdress = clgdistrict.Trim().Trim(',');
                    }
                }
                if (clgpincode.Trim().Trim(',') != "")
                {
                    if (clgfulladdress != "")
                    {
                        clgfulladdress += "-" + clgpincode.Trim().Trim(',').Trim('.') + ".";
                    }
                    else
                    {
                        clgfulladdress = clgpincode.Trim().Trim(',').Trim('.') + ".";
                    }
                }

                #endregion College Address

                if (dtStudInfo.Rows.Count > 0)
                {
                    string studname = Convert.ToString(dtStudInfo.Rows[0]["stud_name"]).Trim();
                    string Admit_no = Convert.ToString(dtStudInfo.Rows[0]["roll_admit"]).Trim();
                    string roll_no = Convert.ToString(dtStudInfo.Rows[0]["Roll_No"]).Trim();
                    string reg_no = Convert.ToString(dtStudInfo.Rows[0]["Reg_No"]).Trim();

                    string admitdate = Convert.ToString(dtStudInfo.Rows[0]["adm_date"]).Trim();
                    string cur_sem = Convert.ToString(dtStudInfo.Rows[0]["Current_Semester"]).Trim();
                    string standard = Convert.ToString(dtStudInfo.Rows[0]["Dept_Name"]).Trim();
                    string section = Convert.ToString(dtStudInfo.Rows[0]["Sections"]).Trim();
                    string dob = Convert.ToString(dtStudInfo.Rows[0]["dob"]).Trim();
                    string blood_grp = Convert.ToString(dtStudInfo.Rows[0]["Blood_Grp"]).Trim();

                    string mother_name = Convert.ToString(dtStudInfo.Rows[0]["mother"]).Trim();
                    string father_name = Convert.ToString(dtStudInfo.Rows[0]["parent_name"]).Trim();
                    string guardian_name = Convert.ToString(dtStudInfo.Rows[0]["guardian_name"]).Trim();

                    string deg_code = Convert.ToString(dtStudInfo.Rows[0]["degree_code"]).Trim();
                    string batch_year = Convert.ToString(dtStudInfo.Rows[0]["Batch_Year"]).Trim();

                    string studaddr = Convert.ToString(dtStudInfo.Rows[0]["parent_addressP"]).Trim().Trim();
                    string studstreetname = Convert.ToString(dtStudInfo.Rows[0]["Streetp"]).Trim().Trim();
                    string studcity = Convert.ToString(dtStudInfo.Rows[0]["Cityp"]).Trim().Trim();
                    string studdist = Convert.ToString(dtStudInfo.Rows[0]["Districtp"]).Trim().Trim();
                    string studsate = Convert.ToString(dtStudInfo.Rows[0]["parent_statep"]).Trim().Trim();
                    string studcountry = Convert.ToString(dtStudInfo.Rows[0]["countryp"]).Trim().Trim();
                    string studpincode = Convert.ToString(dtStudInfo.Rows[0]["parent_pincodep"]).Trim().Trim();

                    string studmob_no = Convert.ToString(dtStudInfo.Rows[0]["student_mobile"]).Trim().Trim();
                    string studFathermob_no = Convert.ToString(dtStudInfo.Rows[0]["parentF_Mobile"]).Trim().Trim();
                    string studMothermob_no = Convert.ToString(dtStudInfo.Rows[0]["parentM_Mobile"]).Trim().Trim();
                    string guardianmob_no = Convert.ToString(dtStudInfo.Rows[0]["guardian_mobile"]).Trim().Trim();

                    //string studEmail = Convert.ToString(dtStudInfo.Rows[0]["parentM_Mobile"]).Trim();
                    string motheremail = Convert.ToString(dtStudInfo.Rows[0]["emailM"]).Trim();
                    string fatheremail = Convert.ToString(dtStudInfo.Rows[0]["emailp"]).Trim();
                    string guardianemail = Convert.ToString(dtStudInfo.Rows[0]["gurdian_email"]).Trim();

                    string studclassandsec = ((standard != "") ? standard + ((section != "") ? " & " + section : "") : ((section != "") ? section : ""));
                    string mobile_no = ((studFathermob_no != "") ? studFathermob_no : "") + ((studFathermob_no != "" && guardianmob_no != "") ? "," + guardianmob_no : ((guardianmob_no != "") ? guardianmob_no : ""));
                    //string medium = Convert.ToString(dtStudInfo.Rows[0][""]);,
                    string studresidentialaddress = "", studresidentialaddress1 = string.Empty;

                    #region Student Address For Line 1

                    if (studaddr.Trim().Trim(',') != "")
                    {
                        studresidentialaddress = studaddr.Trim().Trim(',');
                    }
                    if (studstreetname.Trim().Trim(',') != "")
                    {
                        if (studresidentialaddress != "")
                        {
                            studresidentialaddress += ", " + studstreetname.Trim().Trim(',');
                        }
                        else
                        {
                            studresidentialaddress = studstreetname.Trim().Trim(',');
                        }
                    }
                    if (studcity.Trim().Trim(',') != "")
                    {
                        if (studresidentialaddress != "")
                        {
                            studresidentialaddress += ", " + studcity.Trim().Trim(',');
                        }
                        else
                        {
                            studresidentialaddress = studcity.Trim().Trim(',');
                        }
                    }
                    //if (studresidentialaddress.Length > 40)
                    //{
                    //    studresidentialaddress1 = studresidentialaddress.Substring(40, studresidentialaddress.Length - 40);
                    //    studresidentialaddress = studresidentialaddress.Substring(0, 40);
                    //}

                    #endregion Student Address For Line 1

                    #region Student Address For Line 2

                    if (studdist.Trim().Trim(',') != "")
                    {
                        if (studresidentialaddress != "")
                        {
                            studresidentialaddress += ", " + studdist.Trim().Trim(',');
                        }
                        else
                        {
                            studresidentialaddress = studdist.Trim().Trim(',');
                        }
                    }
                    if (studsate.Trim().Trim(',') != "")
                    {
                        if (studresidentialaddress != "")
                        {
                            studresidentialaddress += ", " + studsate.Trim().Trim(',');
                        }
                        else
                        {
                            studresidentialaddress = studsate.Trim().Trim(',');
                        }
                    }
                    if (studcountry.Trim().Trim(',') != "")
                    {
                        if (studresidentialaddress != "")
                        {
                            studresidentialaddress += ", " + studcountry.Trim().Trim(',');
                        }
                        else
                        {
                            studresidentialaddress = studcountry.Trim().Trim(',');
                        }
                    }
                    if (studpincode.Trim().Trim(',') != "")
                    {
                        if (studresidentialaddress != "")
                        {
                            studresidentialaddress += ", Pincode : " + studpincode.Trim().Trim(',');
                        }
                        else
                        {
                            studresidentialaddress = "Pincode : " + studpincode.Trim().Trim(',');
                        }
                    }

                    #endregion Student Address For Line 2

                    mypdfpage = mydoc.NewPage();

                    #region OUTLINE RECTANGLE

                    pdfMainRect = new PdfRectangle(mydoc, new PdfArea(mydoc, 15, 15, mydoc.PageWidth - 30, mydoc.PageHeight - 30), Color.Black, 1);
                    mypdfpage.Add(pdfMainRect);

                    #endregion OUTLINE RECTANGLE

                    #region College Details

                    if (reportHeaderBased == 1 && dicHeaderAll.Count > 0)
                    {
                        if (dicHeaderAll.ContainsKey("Institution Name") && dicHeaderAll["Institution Name"] == 1)
                        {
                            coltop += 10;
                            pdftxt = new PdfTextArea(fontCol_Name, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, clgname);
                            mypdfpage.Add(pdftxt);
                        }

                        if (dicHeaderAll.ContainsKey("Affiliated By") && dicHeaderAll["Affiliated By"] == 1)
                        {
                            coltop += 20;
                            pdftxt = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, clgaff);
                            mypdfpage.Add(pdftxt);
                        }

                        if (dicHeaderAll.ContainsKey("Address") && dicHeaderAll["Address"] == 1)
                        {
                            coltop += 20;
                            pdftxt = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, clgfulladdress);
                            mypdfpage.Add(pdftxt);
                        }
                        if (dicHeaderAll.ContainsKey("Website") && dicHeaderAll["Website"] == 1)
                        {
                            coltop += 20;
                            pdftxt = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, clgwebsite);
                            mypdfpage.Add(pdftxt);
                        }
                        if (dicHeaderAll.ContainsKey("Email") && dicHeaderAll["Email"] == 1)
                        {
                            coltop += 20;
                            pdftxt = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, clgemail);
                            mypdfpage.Add(pdftxt);
                        }
                        //coltop = 70;
                    }
                    else
                    {
                        coltop += 10;
                        pdftxt = new PdfTextArea(fontCol_Name, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, clgname);
                        mypdfpage.Add(pdftxt);

                        coltop += 20;
                        pdftxt = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, clgaff);
                        mypdfpage.Add(pdftxt);

                        coltop += 20;
                        pdftxt = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, clgfulladdress);
                        mypdfpage.Add(pdftxt);

                        coltop += 20;
                        pdftxt = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, clgemail);
                        mypdfpage.Add(pdftxt);
                    }

                    #endregion College Details

                    //coltop += 35;

                    #region Student Photo Recangle

                    pdfrect = new PdfRectangle(mydoc, new PdfArea(mydoc, (mydoc.PageWidth - 130), 65, 90, 90), Color.Black);
                    mypdfpage.Add(pdfrect);

                    #endregion Student Photo Recangle

                    #region College Left Logo

                    if (reportHeaderBased == 0)
                    {
                        if (colLeftLogo != null)
                        {
                            mypdfpage.Add(colLeftLogo, 35, coltop, 330);
                        }
                    }
                    else if (dicHeaderAll.Count > 0)
                    {
                        if (dicHeaderAll.ContainsKey("Left Logo") && dicHeaderAll["Left Logo"] == 1)
                        {
                            if (colLeftLogo != null)
                            {
                                mypdfpage.Add(colLeftLogo, 35, 70, 330);
                            }
                        }
                    }

                    #endregion College Left Logo

                    #region College Right Logo

                    if (reportHeaderBased == 1 && dicHeaderAll.Count > 0)
                    {
                        if (dicHeaderAll.ContainsKey("Right Logo") && dicHeaderAll["Right Logo"] == 1)
                        {
                            //if (colRightLogo != null)
                            //{
                            //    mypdfpage.Add(colRightLogo, (mydoc.PageWidth / 2) - 40, 70+25, 285);
                            //}
                        }
                    }
                    else
                    {
                        //if (colRightLogo != null)
                        //{
                        //    mypdfpage.Add(colRightLogo, (mydoc.PageWidth / 2) - 40, 70+25, 285);
                        //}                        
                    }

                    #endregion College Right Logo

                    #region Student Photos

                    if (studPhoto != null)
                    {
                        mypdfpage.Add(studPhoto, (mydoc.PageWidth - 125), 70, 320);
                    }

                    #endregion Student Photos

                    #region Report Heading

                    if (reportHeaderBased == 1 && dicHeaderAll.Count > 0)
                    {
                        if (dicHeaderAll.ContainsKey("Report Type") && dicHeaderAll["Report Type"] == 1)
                        {
                            coltop += 20;
                            pdftxt = new PdfTextArea(fontclgReportHeader, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, "Record of Academic Performance");
                            mypdfpage.Add(pdftxt);
                        }
                    }
                    else
                    {
                        coltop += 20;
                        pdftxt = new PdfTextArea(fontclgReportHeader, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, "Record of Academic Performance");
                        mypdfpage.Add(pdftxt);
                    }

                    #endregion

                    #region Academic Year

                    if (reportHeaderBased == 1 && dicHeaderAll.Count > 0)
                    {
                        if (dicHeaderAll.ContainsKey("Academic Year") && dicHeaderAll["Academic Year"] == 1)
                        {
                            coltop += 20;
                            string academicyear = d2.GetFunctionv("select value from master_settings where settings='Academic year'");
                            academicyear = "(Academic Year " + academicyear.Trim().Trim(',').Replace(",", "-") + ")";

                            pdftxt = new PdfTextArea(fontclgReportHeader, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, academicyear);
                            mypdfpage.Add(pdftxt);
                        }
                    }
                    else
                    {
                        coltop += 20;
                        string academicyear = d2.GetFunctionv("select value from master_settings where settings='Academic year'");
                        academicyear = "(Academic Year " + academicyear.Trim().Trim(',').Replace(",", "-") + ")";

                        pdftxt = new PdfTextArea(fontclgReportHeader, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, academicyear);
                        mypdfpage.Add(pdftxt);
                    }

                    #endregion Academic Year

                    #region Student Personal Details

                    if (coltop < 70)
                    {
                        coltop = 110;
                    }
                    coltop += 50;
                    pdfNewTable = mydoc.NewTable(fontStudentDetailsContent, 5, 4, 5);
                    pdfNewTable.VisibleHeaders = false;
                    pdfNewTable.SetBorders(Color.Black, 1, BorderType.None);
                    pdfNewTable.SetColumnsWidth(new int[] { 200, 400, 200, 200 });

                    //Row 0
                    pdfNewTable.Cell(0, 0).SetContent("Registration No.");
                    pdfNewTable.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                    pdfNewTable.Cell(0, 1).SetContent(reg_no);
                    pdfNewTable.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                    pdfNewTable.Cell(0, 2).SetContent("Admission No.");
                    pdfNewTable.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                    pdfNewTable.Cell(0, 3).SetContent(Admit_no);
                    pdfNewTable.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                    //Row 1

                    pdfNewTable.Cell(1, 0).SetContent("Name of Student");
                    pdfNewTable.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                    pdfNewTable.Cell(1, 1).SetContent(studname);
                    pdfNewTable.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                    pdfNewTable.Cell(1, 2).SetContent("Roll No.");
                    pdfNewTable.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                    pdfNewTable.Cell(1, 3).SetContent(roll_no);
                    pdfNewTable.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                    //Row 2

                    pdfNewTable.Cell(2, 0).SetContent("Father's Name");
                    pdfNewTable.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                    pdfNewTable.Cell(2, 1).SetContent(father_name);
                    pdfNewTable.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                    pdfNewTable.Cell(2, 2).SetContent("Class & Section");
                    pdfNewTable.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                    pdfNewTable.Cell(2, 3).SetContent(studclassandsec);
                    pdfNewTable.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                    //Row 3

                    pdfNewTable.Cell(3, 0).SetContent("Mother's Name");
                    pdfNewTable.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                    pdfNewTable.Cell(3, 1).SetContent(mother_name);
                    pdfNewTable.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                    pdfNewTable.Cell(3, 2).SetContent("Date of Birth");
                    pdfNewTable.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                    pdfNewTable.Cell(3, 3).SetContent(dob);
                    pdfNewTable.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                    //Row 4

                    pdfNewTable.Cell(4, 0).SetContent("Address");
                    pdfNewTable.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                    pdfNewTable.Cell(4, 1).SetContent(studresidentialaddress.Trim(',') + studresidentialaddress1.Trim(','));
                    pdfNewTable.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                    foreach (PdfCell pc in pdfNewTable.CellRange(4, 1, 4, 1).Cells)
                    {
                        pc.ColSpan = 3;
                    }

                    pdfNewTablePage = pdfNewTable.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, coltop, mydoc.PageWidth - 50, 400));
                    mypdfpage.Add(pdfNewTablePage);
                    double height = pdfNewTablePage.Area.Height;
                    coltop += int.Parse(Convert.ToString(height)) + 15;

                    #endregion Student Personal Details

                    pdfNewTablePage = pdfMarkTable.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, coltop, mydoc.PageWidth - 50, mydoc.PageHeight - coltop - 20));
                    mypdfpage.Add(pdfNewTablePage);

                    coltop += Convert.ToInt16(pdfNewTablePage.Area.Height) + 15;

                    if (status)
                        mypdfpage.SaveToDocument();
                }

            }

        }
        catch (Exception ex)
        {

        }
    }

    #endregion

    #region Added By Malang Raja on Jan 5 2017

    protected void lnlbtnHeaderSettings_Click(object sender, EventArgs e)
    {
        divHeaderSettings.Visible = true;
    }

    private void BindHeaderSettings()
    {
        DataTable dtMandFee = new DataTable();
        dtMandFee.Columns.Add("Header_Name");
        dtMandFee.Columns.Add("SetOrReset");

        dtMandFee.Rows.Add("Institution Name", "0");
        dtMandFee.Rows.Add("Affiliated By", "0");
        dtMandFee.Rows.Add("Address", "0");
        dtMandFee.Rows.Add("Website", "0");
        dtMandFee.Rows.Add("Email", "0");
        dtMandFee.Rows.Add("Report Type", "0");
        dtMandFee.Rows.Add("Academic Year", "0");
        dtMandFee.Rows.Add("Left Logo", "0");
        dtMandFee.Rows.Add("Right Logo", "0");
        gvHeaderSettings.DataSource = dtMandFee;
        gvHeaderSettings.DataBind();
    }

    protected void gvHeaderSettings_OnDataBound(object sender, EventArgs e)
    {
        setPrevVal();
    }

    private void setPrevVal()
    {
        try
        {
            chkHeaderBased.Checked = false;
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = Convert.ToString(Session["group_code"]).Trim().Split(';')[0];
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = Convert.ToString(Session["usercode"]).Trim();
            }
            foreach (GridViewRow gRow in gvHeaderSettings.Rows)
            {
                Label lblHeaderName = (Label)gRow.FindControl("lblHeaderName");
                string linkVal = lblHeaderName.Text.Trim() + "@#CBSEReportHeaderSettings";
                CheckBox chkSelectHeader = (CheckBox)gRow.FindControl("chkSelectHeader");
                byte prevVal = Convert.ToByte(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='" + linkVal + "' and user_code ='" + grouporusercode + "' and college_code ='" + ddlCollege.SelectedValue + "'").Trim());
                if (prevVal == 1)
                {
                    chkSelectHeader.Checked = true;
                }
                else
                {
                    chkSelectHeader.Checked = false;
                }
            }
            byte reportHeaderBased = 0;
            string insqry = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='CBSEReportHeaderSettings' and user_code ='" + grouporusercode + "' and college_code ='" + ddlCollege.SelectedValue + "'").Trim();
            byte.TryParse(insqry.Trim(), out reportHeaderBased);
            if (reportHeaderBased != 0)
            {
                chkHeaderBased.Checked = true;
            }
        }
        catch { }
    }

    protected void btnResetHeader_Click(object sender, EventArgs e)
    {
        setPrevVal();
    }

    protected void chkSelectAllHeader_OnCheckChange(object sender, EventArgs e)
    {
        foreach (GridViewRow gRow in gvHeaderSettings.Rows)
        {
            CheckBox chkSelectHeader = (CheckBox)gRow.FindControl("chkSelectHeader");
            if (chkSelectAllHeader.Checked)
            {
                chkSelectHeader.Checked = true;
            }
            else
            {
                chkSelectHeader.Checked = false;
            }
        }
    }

    protected void btnSaveHeader_Click(object sender, EventArgs e)
    {
        try
        {
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = Convert.ToString(Session["group_code"]).Trim().Split(';')[0];
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = Convert.ToString(Session["usercode"]).Trim();
            }
            string insqry = string.Empty;
            foreach (GridViewRow gRow in gvHeaderSettings.Rows)
            {
                Label lblFee = (Label)gRow.FindControl("lblHeaderName");
                string linkVal = lblFee.Text.Trim() + "@#CBSEReportHeaderSettings";
                CheckBox chkSel = (CheckBox)gRow.FindControl("chkSelectHeader");
                byte saveVal = 0;
                if (chkSel.Checked)
                {
                    saveVal = 1;
                }
                insqry = "if exists (select LinkValue from New_InsSettings where LinkName='" + linkVal + "' and user_code ='" + grouporusercode + "' and college_code ='" + ddlCollege.SelectedValue + "' ) update New_InsSettings set LinkValue ='" + saveVal + "' where LinkName='" + linkVal + "' and user_code ='" + grouporusercode + "' and college_code ='" + ddlCollege.SelectedValue + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('" + linkVal + "','" + saveVal + "','" + grouporusercode + "','" + ddlCollege.SelectedValue + "')";
                d2.update_method_wo_parameter(insqry, "Text");
            }
            byte reportHeaderBased = 0;
            if (chkHeaderBased.Checked)
            {
                reportHeaderBased = 1;
            }
            insqry = "if exists (select LinkValue from New_InsSettings where LinkName='CBSEReportHeaderSettings' and user_code ='" + grouporusercode + "' and college_code ='" + ddlCollege.SelectedValue + "' ) update New_InsSettings set LinkValue ='" + reportHeaderBased + "' where LinkName='CBSEReportHeaderSettings' and user_code ='" + grouporusercode + "' and college_code ='" + ddlCollege.SelectedValue + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('CBSEReportHeaderSettings','" + reportHeaderBased + "','" + grouporusercode + "','" + ddlCollege.SelectedValue + "')";
            d2.update_method_wo_parameter(insqry, "Text");
            popupdiv.Visible = true;
            lblpoperr.Text = "Saved Successfully";
        }
        catch
        {
            popupdiv.Visible = true;
            lblpoperr.Text = "Not Saved";
        }
    }

    protected void btnCloseHeader_Click(object sender, EventArgs e)
    {
        divHeaderSettings.Visible = false;
    }

    private void GetHeaderSettings(ref Dictionary<string, byte> dicHeaderSettings, ref byte reportHeaderBased)
    {
        dicHeaderSettings.Clear();
        dicHeaderSettings.Add("Institution Name", 0);
        dicHeaderSettings.Add("Affiliated By", 0);
        dicHeaderSettings.Add("Address", 0);
        dicHeaderSettings.Add("Website", 0);
        dicHeaderSettings.Add("Email", 0);
        dicHeaderSettings.Add("Report Type", 0);
        dicHeaderSettings.Add("Academic Year", 0);
        dicHeaderSettings.Add("Left Logo", 0);
        dicHeaderSettings.Add("Right Logo", 0);

        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = Convert.ToString(Session["group_code"]).Trim().Split(';')[0];
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = Convert.ToString(Session["usercode"]).Trim();
        }
        string allHeaderSettings = string.Join(",", dicHeaderSettings.Select(kvp => Convert.ToString("'") + kvp.Key.ToString() + "@#CBSEReportHeaderSettings'").ToArray()).Trim().Trim(',');
        string HeaderBased = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='CBSEReportHeaderSettings' and user_code ='" + grouporusercode + "' and college_code ='" + ddlCollege.SelectedValue + "'");
        byte.TryParse(HeaderBased.Trim(), out reportHeaderBased);
        DataSet dsHeaderSettings = new DataSet();
        if (!string.IsNullOrEmpty(grouporusercode) && !string.IsNullOrEmpty(allHeaderSettings))
        {
            string qry = "select * from New_InsSettings where LinkName in (" + allHeaderSettings + ") and user_code='" + grouporusercode + "' and college_code='" + ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13") + "' and LinkValue='1'";
            dsHeaderSettings = d2.select_method_wo_parameter(qry, "Text");
        }
        if (dsHeaderSettings.Tables.Count > 0 && dsHeaderSettings.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow drSettings in dsHeaderSettings.Tables[0].Rows)
            {
                string settingsLinkName = Convert.ToString(drSettings["LinkName"]).Trim().Replace("@#CBSEReportHeaderSettings", "").Trim();
                if (dicHeaderAll.ContainsKey(settingsLinkName))
                {
                    dicHeaderAll[settingsLinkName] = 1;
                }
            }
        }

    }

    #endregion Added By Malang Raja on Jan 5 2017

}