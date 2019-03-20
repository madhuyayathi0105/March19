#region Namespace Declaration

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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

public partial class ReportCard_I_To_II : System.Web.UI.Page
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
                    ReportCard_Class_I_And_II(rollnos.Trim().Trim(','));
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

    public void Before_ChangeReportCard_Class_I_And_II(string roll_no)
    {
        try
        {
            Font fontCol_Name = new Font("Times New Roman", 16, FontStyle.Bold);
            Font fontclgAddrHeader = new Font("Times New Roman", 13, FontStyle.Regular);
            Font fontclgReportHeader = new Font("Times New Roman", 14, FontStyle.Bold);
            Font fontstudClass = new Font("Times New Roman", 13, FontStyle.Bold);
            Font fontReportContent = new Font("Times New Roman", 13, FontStyle.Regular);

            int selsem = 0;
            int seltest = 0;

            int[] arr_testno = new int[1];
            int[] arr_semester = new int[1];
            string[] arr_testname = new string[1];

            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;

            string batch = string.Empty;
            string degreecode1 = string.Empty;
            string semester1 = string.Empty;
            string sections = string.Empty;

            int coltop = 5;

            bool status = false;
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            batch = Convert.ToString(ddlbatch.SelectedValue);
            degreecode1 = Convert.ToString(ddlDept.SelectedValue);
            sections = Convert.ToString(ddlsec.SelectedValue);
            batch_year = batch;
            degree_code = degreecode1;

            //semester = Convert.ToString(.SelectedValue);
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
                        semester1 += "," + li.Value;
                    }
                }
            }
            string sec = string.Empty;
            string subsec = string.Empty;
            // term = "and sc.semester='" + dropterm.SelectedItem.Text.ToString() + "'";     

            if (ddlsec.Enabled == true)
            {
                if (ddlsec.SelectedItem.Text != "All")
                {
                    for (int sc = 0; sc < ddlsec.Items.Count; sc++)
                    {
                        sec = "and rg.Sections in ('" + Convert.ToString(ddlsec.SelectedItem.Text) + "')";
                        subsec = "and e.Sections in ('" + Convert.ToString(ddlsec.SelectedItem.Text) + "')";
                    }
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
                        {
                            Array.Resize(ref arr_testno, seltest + 1);
                            Array.Resize(ref arr_testname, seltest + 1);
                        }
                        int.TryParse(li.Value, out arr_testno[seltest]);
                        arr_testname[seltest] = li.Text;
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

            if (rollnos != "")
            {
                qry = "SELECT collname,affliatedby,address1,address2,district,address3,pincode,email,logo1,logo2,email  from collinfo where college_code='" + collegecode + "';";
                if (serialflag == false)
                {
                    qry += "select r.serialno,r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,case when (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp)<>'' then (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp) when convert(varchar(20),a.Cityp)='-1' then '' else convert(varchar(20),a.Cityp)  end as Cityp,parent_pincodep,student_mobile,parentF_Mobile,StuPer_Id,parent_addressc,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,case when (select TextVal from TextValTable tt where TextCriteria='bgrou' and convert(varchar(20),tt.TextCode)=a.bldgrp)<>'' then (select TextVal from TextValTable tt where TextCriteria='bgrou' and convert(varchar(20),tt.TextCode)=a.bldgrp) when convert(varchar(20),a.bldgrp)='-1' then '' else convert(varchar(20),a.bldgrp)  end as Blood_Grp,studhouse,case when (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp)<>'' then (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp) when convert(varchar(20),a.Districtp)='-1' then '' else convert(varchar(20),a.Districtp) end as Districtp,case when (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep)<>'' then (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep) when convert(varchar(20),a.parent_statep)='-1' then '' else convert(varchar(20),a.parent_statep)  end as parent_statep,parent_pincodep,parentM_Mobile,case when (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp)<>'' then (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp) when convert(varchar(20),a.countryp)='-1' then '' else convert(varchar(20),a.countryp)  end as countryp,serialno,emailM,ParentidP,guardian_name,guardian_mobile,gurdian_email,emailp from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code  and r.Roll_No in (" + roll_no + ") " + strorderby + " ;";
                }
                else
                {
                    qry += "select r.serialno,r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,case when (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp)<>'' then (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp) when convert(varchar(20),a.Cityp)='-1' then '' else convert(varchar(20),a.Cityp)  end as Cityp,parent_pincodep,student_mobile,parentF_Mobile,StuPer_Id,parent_addressc,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,case when (select TextVal from TextValTable tt where TextCriteria='bgrou' and convert(varchar(20),tt.TextCode)=a.bldgrp)<>'' then (select TextVal from TextValTable tt where TextCriteria='bgrou' and convert(varchar(20),tt.TextCode)=a.bldgrp) when convert(varchar(20),a.bldgrp)='-1' then '' else convert(varchar(20),a.bldgrp)  end as Blood_Grp,studhouse,case when (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp)<>'' then (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp) when convert(varchar(20),a.Districtp)='-1' then '' else convert(varchar(20),a.Districtp) end as Districtp,case when (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep)<>'' then (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep) when convert(varchar(20),a.parent_statep)='-1' then '' else convert(varchar(20),a.parent_statep)  end as parent_statep,parent_pincodep,parentM_Mobile,case when (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp)<>'' then (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp) when convert(varchar(20),a.countryp)='-1' then '' else convert(varchar(20),a.countryp) end as countryp,serialno,emailM,ParentidP,guardian_name,guardian_mobile,gurdian_email,emailp from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No in (" + roll_no + ") order by serialno";
                }
                ds.Clear();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(qry, "Text");

                DataSet dsStudMarks = new DataSet();
                DataTable dtStudMark = new DataTable();
                DataView dvStudMark = new DataView();

                DataTable dtdistinctTest = new DataTable();
                DataTable dtStudSub = new DataTable();
                qry = "select distinct s.subject_name,s.subject_code,e.sections from Exam_type e,subject s,CriteriaForInternal c,syllabus_master sm where sm.syll_code=s.syll_code and c.syll_code=sm.syll_code and c.syll_code=s.syll_code and c.Criteria_no=e.criteria_no and s.subject_no=e.subject_no and sm.degree_code='" + degree_code + "' and sm.Batch_Year='" + batch_year + "' and semester in (" + semester1 + ") and sm.Batch_Year=e.batch_year and c.Criteria_no in (" + test_no + ") " + subsec + " order by s.subject_code;select r.roll_no,rg.Reg_No,rg.Stud_Name,rg.Roll_Admit,rg.degree_code,rg.Current_Semester,sm.semester,rg.sections,c.Criteria_no,c.criteria,e.exam_code,c.max_mark as Convert_Maxmark,c.min_mark Convert_Minmark,e.max_mark as Conducted_max,e.min_mark as Conduct_Minmark,s.subject_code,s.subject_no,s.subType_no,s.syll_code,s.subject_name,r.marks_obtained,isnull(r.remarks,'') as remarks,convert(varchar(10),e.exam_date,103)as exam_date from Registration rg,CriteriaForInternal c,Exam_type e,Result r,syllabus_master sm,subject s where rg.Roll_No =r.roll_no and c.Criteria_no=e.criteria_no and s.subject_no=e.subject_no  and e.exam_code=r.exam_code and e.batch_year=rg.Batch_Year and e.sections=rg.Sections and sm.Batch_Year=rg.Batch_Year and rg.degree_code=sm.degree_code and sm.syll_code=s.syll_code and sm.syll_code=c.syll_code and e.batch_year=sm.Batch_Year and rg.Batch_Year='" + batch_year + "' and rg.degree_code='" + degree_code + "' and rg.college_code='" + collegecode + "' and cc=0 and delflag=0 and exam_flag<>'Debar' and e.criteria_no in(" + test_no + ") and sm.semester in (" + semester1 + ") " + sec + " and rg.Roll_No in(" + roll_no + ") order by rg.Roll_No,c.Criteria_no,s.subject_code; select Frange,Trange,Mark_Grade,Grade_Master.Credit_Points from Grade_Master where College_Code='" + collegecode + "' and batch_year='" + batch_year + "' and Degree_Code='" + degree_code + "';";
                dsStudMarks = d2.select_method_wo_parameter(qry, "text");
                //dsStudMarks.Tables[1].DefaultView.Sort
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables.Count == 2 && ds.Tables[1].Rows.Count > 0)
                    {
                        for (int studrow = 0; studrow < ds.Tables[1].Rows.Count; studrow++)
                        {
                            coltop = 5;
                            bool staus = false;

                            PdfRectangle pdfRect;
                            PdfRectangle pdfMainRect;

                            PdfLine pdfnewline;

                            PdfTextArea pdftxt;

                            PdfTablePage tblNewPage;
                            PdfTable tblNewTable;

                            PdfImage CollegeLeftLogo;
                            PdfImage collegeRightLogo;
                            PdfImage studPhoto;

                            DataTable dtStudMarks = new DataTable();
                            dvStudMark = new DataView();

                            string Roll_No = Convert.ToString(ds.Tables[1].Rows[studrow]["Roll_No"]);
                            string stud_app_no = Convert.ToString(ds.Tables[1].Rows[studrow]["App_No"]);
                            string admitdate = Convert.ToString(ds.Tables[1].Rows[studrow]["adm_date"]);
                            string studname = Convert.ToString(ds.Tables[1].Rows[studrow]["stud_name"]);
                            string classname = Convert.ToString(ds.Tables[1].Rows[studrow]["Dept_Name"]);
                            string section = Convert.ToString(ds.Tables[1].Rows[studrow]["Sections"]);
                            currentsem = Convert.ToString(ds.Tables[1].Rows[studrow]["Current_Semester"]);
                            string studclassandsec = ((classname != "") ? classname + ((section != "") ? " & " + section : "") : ((section != "") ? section : ""));

                            DataView dv = new DataView();
                            DataTable dtStudInfo = new DataTable();
                            ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                            dv = ds.Tables[1].DefaultView;
                            dtStudInfo = dv.ToTable();
                            DataView dvSub = new DataView();
                            DataTable dtSubjects = new DataTable();

                            if (dtStudInfo.Rows.Count > 0)
                            {
                                if (dsStudMarks.Tables.Count > 0 && dsStudMarks.Tables[0].Rows.Count > 0)
                                {
                                    dsStudMarks.Tables[0].DefaultView.RowFilter = "sections='" + section + "'";
                                    dsStudMarks.Tables[0].DefaultView.Sort = "subject_code,subject_name";
                                    dtSubjects = dsStudMarks.Tables[0].DefaultView.ToTable(true, "subject_name");
                                    if (dsStudMarks.Tables.Count >= 2 && dsStudMarks.Tables[1].Rows.Count > 0)
                                    {
                                        //staus = true;
                                        dsStudMarks.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                                        dvStudMark = dsStudMarks.Tables[1].DefaultView;
                                        dtStudMarks = dvStudMark.ToTable();
                                        if (dtStudMarks.Rows.Count > 0)
                                        {
                                            staus = true;
                                        }
                                    }
                                }

                                DataTable dtcolinfo = new DataTable();
                                dtcolinfo = ds.Tables[0];
                                string stdphtsql = "select * from StdPhoto where app_no='" + stud_app_no + "'";
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
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                        {
                                        }
                                        else
                                        {
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                    }
                                    CollegeLeftLogo = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                }
                                else
                                {
                                    CollegeLeftLogo = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                }

                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                {
                                    byte[] file = (byte[])ds.Tables[0].Rows[0]["logo2"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                                        {
                                        }
                                        else
                                        {
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                    }
                                    collegeRightLogo = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                }
                                else
                                {
                                    collegeRightLogo = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                }

                                #region PAGE 1

                                CommonFrontPage(mydoc: mydoc, dtStudInfo: dtStudInfo, dtcolinfo: ds.Tables[0], rpthead: "ACHIEVEMENT REPORT", studclass: "CLASS I & II", status: ref staus, type: 1, colLeftLogo: CollegeLeftLogo, studPhoto: studPhoto, colRightLogo: collegeRightLogo);

                                //staus = true;
                                if (staus)
                                {
                                    status = true;
                                    //mypdfpage.SaveToDocument();
                                }

                                #endregion PAGE 1

                                #region PAGE 2
                                if (dsStudMarks.Tables.Count > 0 && dsStudMarks.Tables[0].Rows.Count > 0 && dtSubjects.Rows.Count > 0)
                                {
                                    if (dsStudMarks.Tables.Count >= 2 && dsStudMarks.Tables[1].Rows.Count > 0 && dtStudMarks.Rows.Count > 0)
                                    {
                                        staus = true;
                                        if (staus)
                                            status = true;
                                        //mypdfpage.SaveToDocument();
                                        mypdfpage = mydoc.NewPage();

                                        dtdistinctTest = dsStudMarks.Tables[1].DefaultView.ToTable("DitstinctTest", true, "Criteria_no", "criteria", "semester");
                                        dtStudSub = dsStudMarks.Tables[1].DefaultView.ToTable("StudentSubjects", true, "subject_code", "subject_name", "sections");
                                        //        var distinctRows = (from DataRow dRow in dtdistinctTest.Rows
                                        //select dRow["Criteria_no"]).Distinct();
                                        //c.Criteria_no,c.criteria,sm.semester  s.subject_code,s.subject_name,rg.sections
                                        #region OUTLINE RECTANGLE

                                        pdfMainRect = new PdfRectangle(mydoc, new PdfArea(mydoc, 25, 25, mydoc.PageWidth - 50, mydoc.PageHeight - 50), Color.Black, 3);
                                        mypdfpage.Add(pdfMainRect);
                                        coltop = 45;

                                        #endregion OUTLINE RECTANGLE

                                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 40, coltop, 80, 20), ContentAlignment.MiddleLeft, "Name : ");
                                        mypdfpage.Add(pdftxt);

                                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)(80), coltop, (mydoc.PageWidth / 2), 20), ContentAlignment.MiddleLeft, studname);
                                        mypdfpage.Add(pdftxt);

                                        pdfnewline = new PdfLine(mydoc, new PointF((float)(80), coltop + 15), new PointF((float)((mydoc.PageWidth / 2) + 38), coltop + 15), Color.Black, 1);
                                        mypdfpage.Add(pdfnewline);

                                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (mydoc.PageWidth / 2) + 38, coltop, 100, 20), ContentAlignment.MiddleLeft, "Class & Sec : ");
                                        mypdfpage.Add(pdftxt);

                                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)((mydoc.PageWidth / 2) + 110), coltop, (mydoc.PageWidth / 2), 20), ContentAlignment.MiddleLeft, studclassandsec);
                                        mypdfpage.Add(pdftxt);

                                        pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) + 110), coltop + 15), new PointF((float)(mydoc.PageWidth - 40), coltop + 15), Color.Black, 1);
                                        mypdfpage.Add(pdfnewline);


                                        //dtdistinctTest=
                                        coltop += 40;
                                        int r = dtSubjects.Rows.Count;
                                        int col = arr_testname.Length + 1;
                                        int startcol = 1;
                                        int startrow = 2;
                                        double[] subjectWiseTotal = new double[dtSubjects.Rows.Count];
                                        double[] subjectWiseMaxTotal = new double[dtSubjects.Rows.Count];
                                        double[] subjectWiseTest = new double[dtSubjects.Rows.Count];
                                        double[] Termwise_TotWorking = new double[arr_semester.Length];
                                        double[] Termwise_Present = new double[arr_semester.Length];

                                        tblNewTable = mydoc.NewTable(fontReportContent, r + 4, col + 2, 5);
                                        tblNewTable.VisibleHeaders = false;
                                        tblNewTable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        //tblNewTable.SetColumnsWidth(new int[] { });
                                        tblNewTable.Cell(0, 0).SetContent("SUBJECTS");

                                        tblNewTable.Columns[0].SetWidth(350);
                                        tblNewTable.Cell(0, 0).SetForegroundColor(Color.Black);
                                        tblNewTable.Cell(0, 0).SetFont(fontstudClass);
                                        int[] columnwidth = new int[col + 1];
                                        tblNewTable.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        foreach (PdfCell pc in tblNewTable.CellRange(0, 0, 0, 0).Cells)
                                        {
                                            pc.RowSpan = 2;
                                        }
                                        double[] termwiseMax = new double[arr_semester.Length];
                                        for (int term = 0; term < arr_semester.Length; term++)
                                        {
                                            DataView dvTest = new DataView();
                                            dtdistinctTest.DefaultView.RowFilter = "semester='" + (arr_semester[term]) + "'";
                                            dvTest = dtdistinctTest.DefaultView;
                                            DateTime dtSemStart = new DateTime();
                                            DateTime dtSemEnd = new DateTime();

                                            string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + (arr_semester[term]) + "' and degree_code='" + degree_code + "' and batch_year='" + batch_year + "'";
                                            DataSet dsSem = d2.select_method_wo_parameter(sem, "Text");

                                            if (dsSem.Tables.Count > 0 && dsSem.Tables[0].Rows.Count > 0)
                                            {
                                                string startdate = Convert.ToString(dsSem.Tables[0].Rows[0]["start_date"]);
                                                string enddate = Convert.ToString(dsSem.Tables[0].Rows[0]["end_date"]);
                                                currentsem = Convert.ToString((arr_semester[term]));
                                                persentmonthcal(Roll_No, admitdate, startdate, enddate);
                                                Termwise_TotWorking[term] = per_workingdays;
                                                Termwise_Present[term] = pre_present_date;
                                            }

                                            int stcol = startcol;
                                            if (dvTest.Count > 0)
                                            {
                                                tblNewTable.Cell(0, startcol).SetContent(((b_school) ? "Term " + ToRoman(Convert.ToString((arr_semester[term]))) : "Semester " + ToRoman(Convert.ToString((arr_semester[term])))));
                                                tblNewTable.Cell(0, startcol).SetForegroundColor(Color.Black);
                                                tblNewTable.Cell(0, startcol).SetFont(fontstudClass);
                                                tblNewTable.Cell(0, startcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                foreach (PdfCell pc in tblNewTable.CellRange(0, startcol, 0, startcol).Cells)
                                                {
                                                    pc.ColSpan = dvTest.Count;
                                                }
                                                stcol = startcol;
                                                for (int tt = 0; tt < dvTest.Count; tt++)
                                                {
                                                    string newtest_no = Convert.ToString(dvTest[tt]["Criteria_no"]);

                                                    dsStudMarks.Tables[1].DefaultView.RowFilter = "Criteria_no='" + newtest_no + "'";
                                                    DataTable dtTestSubjects = dsStudMarks.Tables[1].DefaultView.ToTable(true, "subject_name", "Criteria_no");
                                                    DataTable dtTestMax = dsStudMarks.Tables[1].DefaultView.ToTable(true, "Convert_Maxmark");
                                                    double maxMark = 0;
                                                    if (dtTestMax.Rows.Count > 0)
                                                    {
                                                        double.TryParse(Convert.ToString(dtTestMax.Rows[0]["Convert_Maxmark"]), out maxMark);
                                                    }
                                                    termwiseMax[term] += maxMark;
                                                    tblNewTable.Cell(1, startcol).SetContent(Convert.ToString(dvTest[tt]["criteria"] + "\n" + Convert.ToString(maxMark) + " %"));
                                                    int width = Convert.ToString(dvTest[tt]["criteria"] + "\n" + Convert.ToString(maxMark) + " %").Length + 10 * 10 + 10;
                                                    tblNewTable.Columns[startcol].SetWidth(width);
                                                    tblNewTable.Cell(1, startcol).SetForegroundColor(Color.Black);
                                                    tblNewTable.Cell(1, startcol).SetFont(fontstudClass);
                                                    double beforeTot_obtained = 0;
                                                    double afterTot_obtained = 0;
                                                    double convertTotMax = 0;
                                                    double conductTotMax = 0;

                                                    int sub_row = startrow;

                                                    string conductedMaxMarks = string.Empty;
                                                    string convertedMaxMarks = string.Empty;
                                                    string conductMinmarks = string.Empty;
                                                    string convertMinMarks = string.Empty;
                                                    string obtainedMarks = string.Empty;
                                                    string minmarks = string.Empty;
                                                    string grade = string.Empty;

                                                    if (term == 0 && tt == 0)
                                                    {
                                                        sub_row = startrow;

                                                        tblNewTable.Columns[startcol].SetWidth(width);
                                                        for (int subrow = 0; subrow < dtSubjects.Rows.Count; subrow++)
                                                        {
                                                            tblNewTable.Cell(sub_row, 0).SetContent(Convert.ToString(dtSubjects.Rows[subrow]["subject_name"]));
                                                            tblNewTable.Cell(sub_row, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                            tblNewTable.Cell(sub_row, 0).SetForegroundColor(Color.Black);
                                                            sub_row++;
                                                        }

                                                        tblNewTable.Cell(sub_row, 0).SetContent("ATTENDANCE\n No of Working days");
                                                        tblNewTable.Cell(sub_row, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tblNewTable.Cell(sub_row, 0).SetFont(fontstudClass);
                                                        tblNewTable.Cell(sub_row, 0).SetForegroundColor(Color.Black);
                                                        //foreach (PdfCell pc in tblNewTable.CellRange(sub_row, 0, sub_row, 0).Cells)
                                                        //{
                                                        //    pc.RowSpan = 2;
                                                        //}
                                                        tblNewTable.Cell(sub_row + 1, 0).SetContent("No of days present");
                                                        tblNewTable.Cell(sub_row + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tblNewTable.Cell(sub_row + 1, 0).SetForegroundColor(Color.Black);
                                                    }
                                                    else
                                                    {
                                                        tblNewTable.Columns[startcol].SetWidth(100);
                                                    }
                                                    sub_row = startrow;
                                                    for (int subrow = 0; subrow < dtSubjects.Rows.Count; subrow++)
                                                    {
                                                        obtainedMarks = string.Empty;
                                                        conductedMaxMarks = string.Empty;
                                                        convertedMaxMarks = string.Empty;
                                                        conductMinmarks = string.Empty;
                                                        convertMinMarks = string.Empty;
                                                        minmarks = string.Empty;
                                                        grade = string.Empty;

                                                        double obtmark = 0;
                                                        double conductMax = 0;
                                                        double convertMax = 0;
                                                        double conductMin = 0;
                                                        double convertMin = 0;

                                                        DataView dvMark = new DataView();
                                                        dtStudMarks.DefaultView.RowFilter = "Criteria_no='" + newtest_no + "' and subject_name='" + Convert.ToString(dtSubjects.Rows[subrow]["subject_name"]) + "'";
                                                        dvMark = dtStudMarks.DefaultView;
                                                        if (dvMark.Count > 0)
                                                        {
                                                            obtmark = 0;
                                                            conductMax = 0;
                                                            convertMax = 0;
                                                            conductMin = 0;
                                                            convertMin = 0;
                                                            obtainedMarks = Convert.ToString(dvMark[0]["marks_obtained"]).Trim();
                                                            conductedMaxMarks = Convert.ToString(dvMark[0]["Conducted_max"]).Trim();
                                                            convertedMaxMarks = Convert.ToString(dvMark[0]["Convert_Maxmark"]).Trim();
                                                            conductMinmarks = Convert.ToString(dvMark[0]["Conduct_Minmark"]).Trim();
                                                            convertMinMarks = Convert.ToString(dvMark[0]["Convert_Minmark"]).Trim();
                                                            minmarks = string.Empty;
                                                            grade = string.Empty;
                                                            double.TryParse(obtainedMarks, out obtmark);
                                                            double.TryParse(conductedMaxMarks, out conductMax);
                                                            double.TryParse(convertedMaxMarks, out convertMax);
                                                            double.TryParse(conductMinmarks, out conductMin);
                                                            double.TryParse(convertMinMarks, out convertMin);
                                                            if (obtmark < 0)
                                                            {
                                                                obtainedMarks = loadmarkat(Convert.ToString(obtmark));
                                                                tblNewTable.Cell(sub_row, startcol).SetContent(Convert.ToString(obtainedMarks));
                                                                tblNewTable.Cell(sub_row, startcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                tblNewTable.Cell(sub_row, startcol).SetForegroundColor(Color.Red);
                                                            }
                                                            else
                                                            {
                                                                beforeTot_obtained += obtmark;
                                                                ConvertedMark(convertedMaxMarks, ref conductedMaxMarks, ref obtainedMarks, ref minmarks);
                                                                double.TryParse(obtainedMarks, out obtmark);
                                                                afterTot_obtained += obtmark;
                                                                subjectWiseTotal[subrow] += obtmark;
                                                                if (dsStudMarks.Tables.Count == 3 && dsStudMarks.Tables[2].Rows.Count > 0)
                                                                {
                                                                    findgrade(dsStudMarks.Tables[2], obtainedMarks, ref grade);
                                                                    tblNewTable.Cell(sub_row, startcol).SetContent(grade);
                                                                }
                                                                else
                                                                {
                                                                    tblNewTable.Cell(sub_row, startcol).SetContent(obtainedMarks);
                                                                }
                                                                tblNewTable.Cell(sub_row, startcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                tblNewTable.Cell(sub_row, startcol).SetForegroundColor(Color.Black);
                                                            }
                                                            subjectWiseMaxTotal[subrow] += convertMax;
                                                            conductTotMax += conductMax;
                                                            convertTotMax += convertMax;
                                                        }
                                                        else
                                                        {
                                                            tblNewTable.Cell(sub_row, startcol).SetContent("--");
                                                            tblNewTable.Cell(sub_row, startcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            tblNewTable.Cell(sub_row, startcol).SetForegroundColor(Color.Black);
                                                        }
                                                        //if (tt == dvTest.Count - 1)
                                                        //{
                                                        //    tblNewTable.Cell(sub_row, startcol+1).SetContent(subjectWiseTotal[sub_row]);
                                                        //    tblNewTable.Cell(sub_row, startcol+1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        //    tblNewTable.Cell(sub_row, startcol+1).SetForegroundColor(Color.Black);
                                                        //}
                                                        sub_row++;
                                                    }
                                                    if (tt == dvTest.Count - 1)
                                                    {
                                                        tblNewTable.Cell(sub_row, stcol).SetContent(Convert.ToString(Termwise_TotWorking[term] + " Days"));
                                                        tblNewTable.Cell(sub_row, stcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        tblNewTable.Cell(sub_row, stcol).SetFont(fontstudClass);
                                                        tblNewTable.Cell(sub_row, stcol).SetForegroundColor(Color.Black);
                                                        //foreach (PdfCell pc in tblNewTable.CellRange(sub_row, stcol, sub_row, stcol).Cells)
                                                        //{
                                                        //    pc.RowSpan = 2;
                                                        //}
                                                        foreach (PdfCell pc in tblNewTable.CellRange(sub_row, stcol, sub_row, stcol).Cells)
                                                        {
                                                            pc.ColSpan = dvTest.Count;
                                                        }


                                                        tblNewTable.Cell(sub_row + 1, stcol).SetContent(Convert.ToString(Termwise_Present[term] + " Days"));
                                                        tblNewTable.Cell(sub_row + 1, stcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        tblNewTable.Cell(sub_row + 1, stcol).SetForegroundColor(Color.Black);
                                                        foreach (PdfCell pc in tblNewTable.CellRange(sub_row + 1, stcol, sub_row + 1, stcol).Cells)
                                                        {
                                                            pc.ColSpan = dvTest.Count;
                                                        }

                                                    }
                                                    //}                                                   
                                                    startcol++;
                                                }
                                            }
                                            if (term == arr_semester.Length - 1)
                                            {
                                                string overmax = string.Empty;
                                                double overalltotal = 0;
                                                for (int over = 0; over < termwiseMax.Length; over++)
                                                {
                                                    overalltotal += termwiseMax[over];
                                                    overmax += termwiseMax[over] + " % + ";
                                                }
                                                //overmax = overmax.Trim().Trim('+') + " = " + overalltotal + " %";
                                                overmax = ("Over all Grade \n" + overmax.Trim().Trim('+') + " = (" + overalltotal + " % /" + overalltotal + " ) * 100 = 100%").Trim();
                                                int overallgradewidth = (overmax.Trim().Length) * 5;
                                                tblNewTable.Columns[startcol].SetWidth(overallgradewidth);
                                                tblNewTable.Columns[startcol + 1].SetWidth(overallgradewidth / 3);
                                                columnwidth[startcol - 1] = overallgradewidth;
                                                columnwidth[startcol] = overallgradewidth / 3;//int.Parse(Convert.ToString

                                                //tblNewTable.Columns[startcol].SetWidth(200);
                                                //tblNewTable.Columns[startcol + 1].SetWidth(100);
                                                tblNewTable.Cell(0, startcol).SetContent(Convert.ToString("Over all Grade \n" + overmax));
                                                foreach (PdfCell pc in tblNewTable.CellRange(0, startcol, 0, startcol).Cells)
                                                {
                                                    pc.RowSpan = 2;
                                                }
                                                foreach (PdfCell pc in tblNewTable.CellRange(0, startcol, 0, startcol).Cells)
                                                {
                                                    pc.ColSpan = 2;
                                                }
                                                tblNewTable.Cell(0, startcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tblNewTable.Cell(0, startcol).SetFont(fontstudClass);
                                                tblNewTable.Cell(0, startcol).SetForegroundColor(Color.Black);

                                                int sub_row1 = startrow;
                                                for (int subrow = 0; subrow < dtSubjects.Rows.Count; subrow++)
                                                {
                                                    string overallgrade = string.Empty;
                                                    double total = 0;
                                                    double maxtot = 0;
                                                    double avg = 0;
                                                    double.TryParse(Convert.ToString(subjectWiseTotal[subrow]), out total);
                                                    double.TryParse(Convert.ToString(subjectWiseMaxTotal[subrow]), out maxtot);

                                                    if (total != 0 && maxtot != 0)
                                                    {
                                                        avg = (total / maxtot) * 100;
                                                        avg = Math.Round(avg, 1, MidpointRounding.AwayFromZero);
                                                    }
                                                    //tblNewTable.Cell(sub_row1, startcol).SetContent(subjectWiseTotal[subrow] + "/" + subjectWiseMaxTotal[subrow]);
                                                    if (dsStudMarks.Tables.Count == 3 && dsStudMarks.Tables[2].Rows.Count > 0)
                                                    {
                                                        findgrade(dsStudMarks.Tables[2], Convert.ToString(avg), ref overallgrade);
                                                        tblNewTable.Cell(sub_row1, startcol).SetContent(overallgrade);
                                                    }
                                                    else
                                                    {
                                                        tblNewTable.Cell(sub_row1, startcol).SetContent(avg);
                                                    }
                                                    tblNewTable.Cell(sub_row1, startcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    //tblNewTable.Cell(sub_row1, startcol).SetFont(fontstudClass);
                                                    tblNewTable.Cell(sub_row1, startcol).SetForegroundColor(Color.Black);
                                                    foreach (PdfCell pc in tblNewTable.CellRange(sub_row1, startcol, sub_row1, startcol).Cells)
                                                    {
                                                        pc.ColSpan = 2;
                                                    }
                                                    sub_row1++;
                                                }
                                                //string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + (term + 1) + "' and degree_code='" + degree_code + "' and batch_year='" + batch_year + "'";

                                                sem = "select CONVERT(VARCHAR(30),Min(start_date),111) as start_date ,CONVERT(VARCHAR(30),max(end_date),111) as end_date from seminfo where degree_code='" + degree_code + "' and batch_year='" + batch_year + "'";
                                                dsSem.Clear();
                                                dsSem = d2.select_method_wo_parameter(sem, "Text");
                                                double overall_TotWorking = 0;
                                                double overall_Present = 0;
                                                if (dsSem.Tables.Count > 0 && dsSem.Tables[0].Rows.Count > 0)
                                                {
                                                    string startdate = Convert.ToString(dsSem.Tables[0].Rows[0]["start_date"]);
                                                    string enddate = Convert.ToString(dsSem.Tables[0].Rows[0]["end_date"]);
                                                    currentsem = Convert.ToString((arr_semester[term]));
                                                    persentmonthcal(Roll_No, admitdate, startdate, enddate);
                                                    overall_TotWorking = per_workingdays;
                                                    overall_Present = pre_present_date;
                                                }


                                                foreach (PdfCell pc in tblNewTable.CellRange(sub_row1, startcol, sub_row1, startcol).Cells)
                                                {
                                                    pc.ColSpan = 2;
                                                }
                                                tblNewTable.Cell(sub_row1, startcol).SetContent(overall_TotWorking + " Days");
                                                tblNewTable.Cell(sub_row1, startcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tblNewTable.Cell(sub_row1, startcol).SetFont(fontstudClass);
                                                tblNewTable.Cell(sub_row1, startcol).SetForegroundColor(Color.Black);

                                                tblNewTable.Cell(sub_row1 + 1, startcol).SetContent(Convert.ToString(overall_Present + " Days"));
                                                foreach (PdfCell pc in tblNewTable.CellRange(sub_row1 + 1, startcol, sub_row1 + 1, startcol).Cells)
                                                {
                                                    pc.ColSpan = 2;
                                                }
                                                tblNewTable.Cell(sub_row1 + 1, startcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tblNewTable.Cell(sub_row1 + 1, startcol).SetForegroundColor(Color.Black);
                                            }
                                        }
                                        tblNewPage = tblNewTable.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 35, coltop, mydoc.PageWidth - 75, mydoc.PageHeight - coltop - 50));
                                        mypdfpage.Add(tblNewPage);
                                        coltop += Convert.ToInt16(tblNewPage.Area.Height) + 25;

                                        pdftxt = new PdfTextArea(fontclgReportHeader, Color.Black, new PdfArea(mydoc, 35, coltop, 200, 20), ContentAlignment.MiddleLeft, "NOTE TO PARENTS : ");
                                        mypdfpage.Add(pdftxt);
                                        coltop += 25;
                                        tblNewTable = mydoc.NewTable(fontReportContent, 5, 2, 6);
                                        tblNewTable.VisibleHeaders = false;
                                        tblNewTable.SetBorders(Color.Black, 1, BorderType.None);
                                        tblNewTable.SetColumnsWidth(new int[] { 45, 350 });

                                        tblNewTable.Cell(0, 0).SetContent("i");
                                        tblNewTable.Cell(0, 0).SetContentAlignment(ContentAlignment.TopCenter);
                                        tblNewTable.Cell(0, 0).SetForegroundColor(Color.Black);
                                        tblNewTable.Cell(0, 1).SetContent("The progress report is an important yardstick for evaluating the all round development of the ward, inspiring him/her to achieve the designated goals with the strength, courage, determination and conviction.");
                                        tblNewTable.Cell(0, 1).SetContentAlignment(ContentAlignment.TopLeft);
                                        tblNewTable.Cell(0, 1).SetForegroundColor(Color.Black);


                                        tblNewTable.Cell(1, 0).SetContent("ii");
                                        tblNewTable.Cell(1, 0).SetContentAlignment(ContentAlignment.TopCenter);
                                        tblNewTable.Cell(1, 0).SetForegroundColor(Color.Black);
                                        tblNewTable.Cell(1, 1).SetContent("The teachers have been asked to give a fair and unbiased record of your child's work.");
                                        tblNewTable.Cell(1, 1).SetContentAlignment(ContentAlignment.TopLeft);
                                        tblNewTable.Cell(1, 1).SetForegroundColor(Color.Black);


                                        tblNewTable.Cell(2, 0).SetContent("iii");
                                        tblNewTable.Cell(2, 0).SetContentAlignment(ContentAlignment.TopCenter);
                                        tblNewTable.Cell(2, 0).SetForegroundColor(Color.Black);
                                        tblNewTable.Cell(2, 1).SetContent("The minimum attendance required will be 75% of the total no. of working days.");
                                        tblNewTable.Cell(2, 1).SetContentAlignment(ContentAlignment.TopLeft);
                                        tblNewTable.Cell(2, 1).SetForegroundColor(Color.Black);

                                        tblNewTable.Cell(3, 0).SetContent("iv");
                                        tblNewTable.Cell(3, 0).SetContentAlignment(ContentAlignment.TopCenter);
                                        tblNewTable.Cell(3, 0).SetForegroundColor(Color.Black);
                                        tblNewTable.Cell(3, 1).SetContent("Marks of tests, assignments and all terminal examinations will be taken into account for promotion.");
                                        tblNewTable.Cell(3, 1).SetContentAlignment(ContentAlignment.TopLeft);
                                        tblNewTable.Cell(3, 1).SetForegroundColor(Color.Black);


                                        tblNewTable.Cell(4, 0).SetContent("v");
                                        tblNewTable.Cell(4, 0).SetContentAlignment(ContentAlignment.TopCenter);
                                        tblNewTable.Cell(4, 0).SetForegroundColor(Color.Black);
                                        tblNewTable.Cell(4, 1).SetContent("The progress report must be returned duly signed within 3 days, on receipt of the same.");
                                        tblNewTable.Cell(4, 1).SetContentAlignment(ContentAlignment.TopLeft);
                                        tblNewTable.Cell(4, 1).SetForegroundColor(Color.Black);

                                        tblNewPage = tblNewTable.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 35, coltop, mydoc.PageWidth - 75, mydoc.PageHeight - coltop - 50));

                                        mypdfpage.Add(tblNewPage);
                                        if (staus)
                                        {
                                            status = true;
                                            mypdfpage.SaveToDocument();
                                        }
                                    }
                                }

                                #endregion PAGE 2

                                #region PAGE 3

                                #endregion PAGE 3

                            }
                            else
                            {
                                sbErr.Append("\nRoll Number " + Roll_No + " does not Exists.");
                            }
                        }
                    }
                    else
                    {
                        sbErr.Append("\nThere Is No Student Were Found");
                    }
                }
                else
                {
                    sbErr.Append("\nNo College Were Found");
                }
            }
            else
            {
                sbErr.Append("\nPlease Select Atleast One Student");
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
                    string szFile = "ReportCard_Class_I_And_II" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
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

    public void ReportCard_Class_I_And_II(string roll_no)
    {
        try
        {
            Font fontCol_Name = new Font("Book Antiqua", 16, FontStyle.Bold);
            Font fontclgAddrHeader = new Font("Book Antiqua", 11, FontStyle.Regular);
            Font fontclgReportHeader = new Font("Book Antiqua", 13, FontStyle.Bold);
            Font fontstudClass = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font fontReportContent = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font fontStudentDetailsContent = new Font("Book Antiqua", 10, FontStyle.Regular);
            Font fontReportStudProfileHeader = new Font("Book Antiqua", 12, FontStyle.Bold);
            Font fontReportStudActivityHeader = new Font("Book Antiqua", 10, FontStyle.Bold);

            int selsem = 0;
            int seltest = 0;

            int[] arr_testno = new int[1];
            int[] arr_semester = new int[1];
            string[] arr_testname = new string[1];

            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;

            string batch = string.Empty;
            string degreecode1 = string.Empty;
            string semester1 = string.Empty;
            string sections = string.Empty;

            int coltop = 5;
            bool isManualAttendance = false;
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
                        semester1 += "," + li.Value;
                    }
                }
            }
            string sec = string.Empty;
            string subsec = string.Empty;

            if (ddlsec.Enabled == true)
            {
                if (Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() != "-1")
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
                        {
                            Array.Resize(ref arr_testno, seltest + 1);
                            Array.Resize(ref arr_testname, seltest + 1);
                        }
                        int.TryParse(li.Value, out arr_testno[seltest]);
                        arr_testname[seltest] = li.Text;
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

            if (chkManualAttendance.Checked)
            {
                isManualAttendance = true;
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

            if (rollnos != "")
            {
                qry = "SELECT collname,affliatedby,address1,address2,district,address3,pincode,email,logo1,logo2,website from collinfo where college_code='" + collegecode + "';";
                if (serialflag == false)
                {
                    qry += "select r.serialno,r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,case when (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp)<>'' then (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp) when convert(varchar(20),a.Cityp)='-1' then '' else convert(varchar(20),a.Cityp)  end as Cityp,parent_pincodep,student_mobile,parentF_Mobile,StuPer_Id,parent_addressc,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,case when (select TextVal from TextValTable tt where TextCriteria='bgrou' and convert(varchar(20),tt.TextCode)=a.bldgrp)<>'' then (select TextVal from TextValTable tt where TextCriteria='bgrou' and convert(varchar(20),tt.TextCode)=a.bldgrp) when convert(varchar(20),a.bldgrp)='-1' then '' else convert(varchar(20),a.bldgrp)  end as Blood_Grp,studhouse,case when (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp)<>'' then (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp) when convert(varchar(20),a.Districtp)='-1' then '' else convert(varchar(20),a.Districtp) end as Districtp,case when (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep)<>'' then (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep) when convert(varchar(20),a.parent_statep)='-1' then '' else convert(varchar(20),a.parent_statep)  end as parent_statep,parent_pincodep,parentM_Mobile,case when (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp)<>'' then (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp) when convert(varchar(20),a.countryp)='-1' then '' else convert(varchar(20),a.countryp)  end as countryp,serialno,emailM,ParentidP,guardian_name,guardian_mobile,gurdian_email,emailp from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code  and r.Roll_No in (" + roll_no + ") " + strorderby + " ;";
                }
                else
                {
                    qry += "select r.serialno,r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,case when (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp)<>'' then (select TextVal from TextValTable tt where TextCriteria='city' and convert(varchar(20),tt.TextCode)=a.Cityp) when convert(varchar(20),a.Cityp)='-1' then '' else convert(varchar(20),a.Cityp)  end as Cityp,parent_pincodep,student_mobile,parentF_Mobile,StuPer_Id,parent_addressc,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,case when (select TextVal from TextValTable tt where TextCriteria='bgrou' and convert(varchar(20),tt.TextCode)=a.bldgrp)<>'' then (select TextVal from TextValTable tt where TextCriteria='bgrou' and convert(varchar(20),tt.TextCode)=a.bldgrp) when convert(varchar(20),a.bldgrp)='-1' then '' else convert(varchar(20),a.bldgrp)  end as Blood_Grp,studhouse,case when (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp)<>'' then (select TextVal from TextValTable tt where TextCriteria='dis' and convert(varchar(20),tt.TextCode)=a.Districtp) when convert(varchar(20),a.Districtp)='-1' then '' else convert(varchar(20),a.Districtp) end as Districtp,case when (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep)<>'' then (select TextVal from TextValTable tt where TextCriteria='state' and convert(varchar(20),tt.TextCode)=a.parent_statep) when convert(varchar(20),a.parent_statep)='-1' then '' else convert(varchar(20),a.parent_statep)  end as parent_statep,parent_pincodep,parentM_Mobile,case when (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp)<>'' then (select TextVal from TextValTable tt where TextCriteria='Coun' and convert(varchar(20),tt.TextCode)=a.countryp) when convert(varchar(20),a.countryp)='-1' then '' else convert(varchar(20),a.countryp) end as countryp,serialno,emailM,ParentidP,guardian_name,guardian_mobile,gurdian_email,emailp from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No in (" + roll_no + ") order by serialno";
                }
                ds.Clear();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(qry, "Text");

                DataSet dsStudMarks = new DataSet();
                DataSet dsAttRemark = new DataSet();
                DataSet dsPart = new DataSet();

                DataView dvStudMark = new DataView();

                DataTable dtAtt = new DataTable();
                DataTable dtRemark = new DataTable();
                DataTable dtPart = new DataTable();
                DataTable dtdistinctTest = new DataTable();
                DataTable dtStudSub = new DataTable();
                DataTable dtStudMark = new DataTable();

                qry = "select distinct s.subject_name,s.subject_code,e.sections from Exam_type e,subject s,CriteriaForInternal c,syllabus_master sm where sm.syll_code=s.syll_code and c.syll_code=sm.syll_code and c.syll_code=s.syll_code and c.Criteria_no=e.criteria_no and s.subject_no=e.subject_no and sm.degree_code='" + degree_code + "' and sm.Batch_Year='" + batch_year + "' and semester in (" + semester1 + ") and sm.Batch_Year=e.batch_year and c.Criteria_no in (" + test_no + ") " + subsec + " order by s.subject_code;select r.roll_no,rg.Reg_No,rg.Stud_Name,rg.Roll_Admit,rg.degree_code,rg.Current_Semester,sm.semester,rg.sections,c.Criteria_no,c.criteria,e.exam_code,c.max_mark as Convert_Maxmark,c.min_mark Convert_Minmark,e.max_mark as Conducted_max,e.min_mark as Conduct_Minmark,s.subject_code,s.subject_no,s.subType_no,s.syll_code,s.subject_name,r.marks_obtained,isnull(r.remarks,'') as remarks,convert(varchar(10),e.exam_date,103)as exam_date from Registration rg,CriteriaForInternal c,Exam_type e,Result r,syllabus_master sm,subject s where rg.Roll_No =r.roll_no and c.Criteria_no=e.criteria_no and s.subject_no=e.subject_no  and e.exam_code=r.exam_code and e.batch_year=rg.Batch_Year and e.sections=rg.Sections and sm.Batch_Year=rg.Batch_Year and rg.degree_code=sm.degree_code and sm.syll_code=s.syll_code and sm.syll_code=c.syll_code and e.batch_year=sm.Batch_Year and rg.Batch_Year='" + batch_year + "' and rg.degree_code='" + degree_code + "' and rg.college_code='" + collegecode + "' and cc=0 and delflag=0 and exam_flag<>'Debar' and e.criteria_no in(" + test_no + ") and sm.semester in (" + semester1 + ") " + sec + " and rg.Roll_No in(" + roll_no + ") order by rg.Roll_No,c.Criteria_no,s.subject_code; select Frange,Trange,Mark_Grade,Grade_Master.Credit_Points from Grade_Master where College_Code='" + collegecode + "' and batch_year='" + batch_year + "' and Degree_Code='" + degree_code + "';";
                dsStudMarks = d2.select_method_wo_parameter(qry, "text");

                qry = "SELECT * from CoCurrActivitie_Det where  istype='Att'and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "' and term in(" + semester1 + ") and Roll_No in(" + roll_no + ");SELECT * from CoCurrActivitie_Det where istype='remks' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "' and term in(" + semester1 + ") and Roll_No in(" + roll_no + "); ";
                dsAttRemark = new DataSet();
                dsAttRemark = d2.select_method_wo_parameter(qry, "Text");

                //string rmark_qry = "SELECT * from CoCurrActivitie_Det where istype='remks' and Degree_Code='" + degreecode1 + "' and batch_year='" + batch + "' and term in('" + semester1 + "') and Roll_No in('" + roll_no + "');";
                //DataSet rmk = new DataSet();
                //rmk = d2.select_method_wo_parameter(rmark_qry, "Text");


                qry = "select distinct PartName from CoCurr_Activitie where Batch_Year='" + batch_year + "' and Degree_Code='" + degree_code + "' and PartName<>'Part-1'; select distinct ca.CoCurr_ID,Part_No,UserPartName,PartName,Title_Name,tv.TextCode,tv.TextVal,ca.SubTitle,ae.term from activity_entry ae,CoCurr_Activitie ca,TextValTable tv where ca.Batch_Year=ae.Batch_Year and ae.Degree_Code=ca.Degree_Code and ae.CoCurr_ID=ca.CoCurr_ID and tv.TextCode=ae.ActivityTextVal and tv.college_code='" + collegecode + "' and ae.Batch_Year='" + batch_year + "' and ae.Degree_Code='" + degree_code + "' and ae.term in(" + semester1 + ") order by ae.term,Part_No,PartName,ca.SubTitle,ca.CoCurr_ID ; select distinct ca.CoCurr_ID,Part_No,UserPartName,PartName,Title_Name,tv.TextCode,tv.TextVal,ca.SubTitle,ae.term,det.Roll_No,det.Mark from activity_entry ae,CoCurr_Activitie ca,TextValTable tv,CoCurrActivitie_Det det where ca.Batch_Year=ae.Batch_Year and ae.Degree_Code=ca.Degree_Code and ae.CoCurr_ID=ca.CoCurr_ID and tv.TextCode=ae.ActivityTextVal and ca.Batch_Year=det.Batch_Year and det.Batch_Year=ae.Batch_Year and ae.Degree_Code=det.Degree_Code and det.Degree_Code=ca.Degree_Code and ae.term=det.term and det.ActivityTextVal=ae.ActivityTextVal and det.ActivityTextVal=tv.TextCode and tv.college_code='" + collegecode + "' and ae.Batch_Year='" + batch_year + "' and ae.Degree_Code='" + degree_code + "'  and ae.term in(" + semester1 + ") and det.Roll_No in(" + roll_no + ") order by ae.term,Part_No,PartName,ca.SubTitle,ca.CoCurr_ID ; select ca.CoCurr_ID,Part_No,UserPartName,PartName,Title_Name,tv.TextCode,tv.TextVal,ca.SubTitle,ae.term,det.Roll_No,det.Mark,ag.Grade,ag.description,ag.frompoint,ag.topoint from activity_entry ae,CoCurr_Activitie ca,TextValTable tv,CoCurrActivitie_Det det,activity_gd ag where ca.Batch_Year=ae.Batch_Year and ae.Degree_Code=ca.Degree_Code and ae.CoCurr_ID=ca.CoCurr_ID and tv.TextCode=ae.ActivityTextVal and ca.Batch_Year=det.Batch_Year and det.Batch_Year=ae.Batch_Year and ae.Degree_Code=det.Degree_Code and det.Degree_Code=ca.Degree_Code and ae.term=det.term and ag.batch_year=det.Batch_Year and ag.batch_year=ae.Batch_Year and ag.batch_year=ca.Batch_Year and ae.Degree_Code=ag.Degree_Code and ag.Degree_Code=ca.Degree_Code and det.Degree_Code=ag.Degree_Code and ag.term=det.term and ag.term=ae.term and tv.TextCode=ag.ActivityTextVal and ae.ActivityTextVal=ag.ActivityTextVal and det.ActivityTextVal=ag.ActivityTextVal and det.Mark between ag.frompoint and ag.topoint and det.ActivityTextVal=ae.ActivityTextVal and det.ActivityTextVal=tv.TextCode and tv.college_code='" + collegecode + "' and ae.Batch_Year='" + batch_year + "' and ae.Degree_Code='" + degree_code + "' and ae.term in(" + semester1 + ") and det.Roll_No in(" + roll_no + ") order by ae.term,Part_No,PartName,ca.SubTitle,ca.CoCurr_ID ";
                dsPart.Clear();
                dsPart.Reset();
                dsPart = d2.select_method_wo_parameter(qry, "Text");

                if (dsStudMarks.Tables.Count > 0 && dsStudMarks.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables.Count == 2 && ds.Tables[1].Rows.Count > 0)
                        {
                            for (int studrow = 0; studrow < ds.Tables[1].Rows.Count; studrow++)
                            {
                                coltop = 5;
                                bool staus = false;
                                bool hasActivity = false;

                                PdfRectangle pdfRect;
                                PdfRectangle pdfMainRect;

                                PdfLine pdfnewline;

                                PdfTextArea pdftxt;

                                PdfTablePage tblNewPage;
                                PdfTable tblNewTable;

                                PdfImage CollegeLeftLogo = null;
                                PdfImage collegeRightLogo = null;
                                PdfImage studPhoto = null;

                                DataTable dtStudMarks = new DataTable();
                                dvStudMark = new DataView();

                                DataTable dtStudPart = new DataTable();
                                DataTable dtStudActivityMarks = new DataTable();

                                string Roll_No = Convert.ToString(ds.Tables[1].Rows[studrow]["Roll_No"]).Trim();
                                string stud_app_no = Convert.ToString(ds.Tables[1].Rows[studrow]["App_No"]).Trim();
                                string admitdate = Convert.ToString(ds.Tables[1].Rows[studrow]["adm_date"]).Trim();
                                string studname = Convert.ToString(ds.Tables[1].Rows[studrow]["stud_name"]).Trim();
                                string classname = Convert.ToString(ds.Tables[1].Rows[studrow]["Dept_Name"]).Trim();
                                string section = Convert.ToString(ds.Tables[1].Rows[studrow]["Sections"]).Trim();
                                string studheight = Convert.ToString(ds.Tables[1].Rows[studrow]["StudHeight"]).Trim();
                                string studweight = Convert.ToString(ds.Tables[1].Rows[studrow]["StudWeight"]).Trim();

                                currentsem = Convert.ToString(ds.Tables[1].Rows[studrow]["Current_Semester"]);
                                string studclassandsec = ((classname != "") ? classname + ((section != "") ? " & " + section : "") : ((section != "") ? section : ""));

                                DataView dv = new DataView();
                                DataTable dtStudInfo = new DataTable();
                                ds.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                                dv = ds.Tables[1].DefaultView;
                                dtStudInfo = dv.ToTable();
                                DataView dvSub = new DataView();
                                DataTable dtSubjects = new DataTable();

                                int totparts = 0;

                                if (dtStudInfo.Rows.Count > 0)
                                {
                                    string[] studentRemarks = new string[arr_semester.Length];
                                    string[] studentAttendanceTotWorking = new string[arr_semester.Length];
                                    string[] studentAttendanceTotPresent = new string[arr_semester.Length];
                                    studentRemarks = Enumerable.Range(0, arr_semester.Length).Select(i => string.Empty).ToArray();
                                    studentAttendanceTotWorking = Enumerable.Range(0, arr_semester.Length).Select(i => string.Empty).ToArray();
                                    studentAttendanceTotPresent = Enumerable.Range(0, arr_semester.Length).Select(i => string.Empty).ToArray();
                                    if (dsStudMarks.Tables.Count > 0 && dsStudMarks.Tables[0].Rows.Count > 0)
                                    {
                                        dsStudMarks.Tables[0].DefaultView.RowFilter = "sections='" + section + "'";
                                        dsStudMarks.Tables[0].DefaultView.Sort = "subject_code,subject_name";
                                        dtSubjects = dsStudMarks.Tables[0].DefaultView.ToTable(true, "subject_name");
                                        if (dsStudMarks.Tables.Count >= 2 && dsStudMarks.Tables[1].Rows.Count > 0)
                                        {
                                            dsStudMarks.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                                            dvStudMark = dsStudMarks.Tables[1].DefaultView;
                                            dtStudMarks = dvStudMark.ToTable();
                                            if (dtStudMarks.Rows.Count > 0)
                                            {
                                                staus = true;
                                            }
                                        }
                                    }

                                    if (dsPart.Tables.Count > 0)
                                    {
                                        if (dsPart.Tables[0].Rows.Count > 0)
                                        {
                                            totparts = dsPart.Tables[0].Rows.Count;
                                        }
                                        if (dsPart.Tables.Count >= 2 && dsPart.Tables[1].Rows.Count > 0)
                                        {
                                            dsPart.Tables[1].DefaultView.RowFilter = string.Empty;
                                            dtStudPart = dsPart.Tables[1].DefaultView.ToTable(true);
                                        }
                                        if (dsPart.Tables.Count >= 3 && dsPart.Tables[2].Rows.Count > 0)
                                        {
                                            dsPart.Tables[2].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                                            dtStudPart = dsPart.Tables[2].DefaultView.ToTable(true, "CoCurr_ID", "Part_No", "UserPartName", "PartName", "Title_Name", "TextCode", "TextVal", "SubTitle");
                                            //ca.CoCurr_ID,Part_No,UserPartName,PartName,Title_Name,tv.TextCode,tv.TextVal,ca.SubTitle,ae.term,det.Roll_No,det.Mark 
                                        }
                                        if (dsPart.Tables.Count >= 4 && dsPart.Tables[3].Rows.Count > 0)
                                        {
                                            dsPart.Tables[3].DefaultView.RowFilter = "Roll_No='" + Roll_No + "'";
                                            dtStudActivityMarks = dsPart.Tables[3].DefaultView.ToTable(true, "Roll_No", "CoCurr_ID", "Part_No", "UserPartName", "PartName", "Title_Name", "TextCode", "TextVal", "SubTitle", "term", "Mark", "Grade", "description");
                                            totparts = dsPart.Tables[3].DefaultView.ToTable(true, "Part_No").Rows.Count;
                                            if (dtStudActivityMarks.Rows.Count > 0)
                                            {
                                                hasActivity = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        hasActivity = false;
                                    }

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
                                            //CollegeLeftLogo = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
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
                                        //collegeRightLogo = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                    }
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + collegecode.ToString() + ".jpeg")))
                                    {
                                    }
                                    else
                                    {
                                        collegeRightLogo = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo" + collegecode.ToString() + ".jpeg"));
                                    }

                                    #region PAGE 1

                                    mypdfpage = mydoc.NewPage();
                                    if (staus)
                                    {
                                        status = true;
                                    }

                                    #endregion PAGE 1

                                    #region PAGE 2

                                    if (dsStudMarks.Tables.Count > 0 && dsStudMarks.Tables[0].Rows.Count > 0 && dtSubjects.Rows.Count > 0)
                                    {
                                        if (dsStudMarks.Tables.Count >= 2 && dsStudMarks.Tables[1].Rows.Count > 0 && dtStudMarks.Rows.Count > 0)
                                        {
                                            if (status)
                                            {
                                                staus = true;
                                                if (staus)
                                                    status = true;
                                                //mypdfpage.SaveToDocument();
                                                //mypdfpage = mydoc.NewPage();

                                                dtdistinctTest = dsStudMarks.Tables[1].DefaultView.ToTable("DitstinctTest", true, "Criteria_no", "criteria", "semester");
                                                dtStudSub = dsStudMarks.Tables[1].DefaultView.ToTable("StudentSubjects", true, "subject_code", "subject_name", "sections");

                                                studentRemarks = Enumerable.Range(0, arr_semester.Length).Select(i => string.Empty).ToArray();
                                                studentAttendanceTotWorking = Enumerable.Range(0, arr_semester.Length).Select(i => string.Empty).ToArray();
                                                studentAttendanceTotPresent = Enumerable.Range(0, arr_semester.Length).Select(i => string.Empty).ToArray();

                                                #region OUTLINE RECTANGLE

                                                //pdfMainRect = new PdfRectangle(mydoc, new PdfArea(mydoc, 25, 25, mydoc.PageWidth - 50, mydoc.PageHeight - 50), Color.Black, 3);
                                                //mypdfpage.Add(pdfMainRect);
                                                //coltop = 45;

                                                #endregion OUTLINE RECTANGLE

                                                int r = dtSubjects.Rows.Count;
                                                int col = dtdistinctTest.Rows.Count + 1;
                                                int startcol = 1;
                                                int startrow = 2;

                                                double[] subjectWiseTotal = new double[dtSubjects.Rows.Count];
                                                double[] subjectWiseMaxTotal = new double[dtSubjects.Rows.Count];
                                                double[] subjectWiseTest = new double[dtSubjects.Rows.Count];
                                                double[] Termwise_TotWorking = new double[arr_semester.Length];
                                                double[] Termwise_Present = new double[arr_semester.Length];
                                                double overall_TotWorking = 0;
                                                double overall_Present = 0;
                                                bool isAuto = false;


                                                tblNewTable = mydoc.NewTable(fontReportContent, r + 5, col + 2, 5);
                                                tblNewTable.VisibleHeaders = false;
                                                tblNewTable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                tblNewTable.Cell(0, 0).SetContent("SUBJECTS");
                                                tblNewTable.Columns[0].SetWidth(350);
                                                tblNewTable.Cell(0, 0).SetForegroundColor(Color.Black);
                                                tblNewTable.Cell(0, 0).SetFont(fontstudClass);
                                                tblNewTable.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                int[] columnwidth = new int[col + 1];
                                                foreach (PdfCell pc in tblNewTable.CellRange(0, 0, 0, 0).Cells)
                                                {
                                                    pc.RowSpan = 2;
                                                }
                                                double[] termwiseMax = new double[arr_semester.Length];
                                                bool isSubjectPrint = false;
                                                for (int term = 0; term < arr_semester.Length; term++)
                                                {
                                                    DataView dvTest = new DataView();
                                                    dtdistinctTest.DefaultView.RowFilter = "semester='" + (arr_semester[term]) + "'";
                                                    dvTest = dtdistinctTest.DefaultView;
                                                    DateTime dtSemStart = new DateTime();
                                                    DateTime dtSemEnd = new DateTime();

                                                    string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + (arr_semester[term]) + "' and degree_code='" + degree_code + "' and batch_year='" + batch_year + "'";
                                                    DataSet dsSem = d2.select_method_wo_parameter(sem, "Text");

                                                    DataView dvAttRmk = new DataView();
                                                    if (!isManualAttendance)
                                                    {
                                                        if (dsSem.Tables.Count > 0 && dsSem.Tables[0].Rows.Count > 0)
                                                        {
                                                            string startdate = Convert.ToString(dsSem.Tables[0].Rows[0]["start_date"]);
                                                            string enddate = Convert.ToString(dsSem.Tables[0].Rows[0]["end_date"]);
                                                            currentsem = Convert.ToString((arr_semester[term]));
                                                            persentmonthcal(Roll_No, admitdate, startdate, enddate);
                                                            Termwise_TotWorking[term] = per_workingdays;
                                                            Termwise_Present[term] = pre_present_date;
                                                            studentAttendanceTotWorking[term] = Convert.ToString(Termwise_TotWorking[term]);
                                                            studentAttendanceTotPresent[term] = Convert.ToString(Termwise_Present[term]);
                                                            overall_TotWorking += Termwise_TotWorking[term];
                                                            overall_Present += Termwise_Present[term];
                                                            isAuto = true;
                                                        }
                                                        else
                                                        {
                                                            isAuto = false;
                                                            studentAttendanceTotWorking[term] = string.Empty;
                                                            studentAttendanceTotPresent[term] = string.Empty;
                                                            Termwise_TotWorking[term] = 0;
                                                            Termwise_Present[term] = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dsAttRemark.Tables.Count > 0)
                                                        {
                                                            if (dsAttRemark.Tables[0].Rows.Count > 0)
                                                            {
                                                                dsAttRemark.Tables[0].DefaultView.RowFilter = "Roll_No='" + Roll_No + "' and term='" + arr_semester[term] + "' and Mark<>0 and totatt_remarks<>'-'";
                                                                dvAttRmk = dsAttRemark.Tables[0].DefaultView;
                                                                if (dvAttRmk.Count > 0)
                                                                {
                                                                    isAuto = true;
                                                                    studentAttendanceTotWorking[term] = Convert.ToString(dvAttRmk[0]["totatt_remarks"]).Trim();
                                                                    studentAttendanceTotPresent[term] = Convert.ToString(dvAttRmk[0]["Mark"]).Trim();
                                                                    double.TryParse(studentAttendanceTotWorking[term], out Termwise_TotWorking[term]);

                                                                    double.TryParse(studentAttendanceTotPresent[term], out Termwise_Present[term]);
                                                                    overall_TotWorking += Termwise_TotWorking[term];
                                                                    overall_Present += Termwise_Present[term];
                                                                }
                                                                else
                                                                {
                                                                    isAuto = false;
                                                                    studentAttendanceTotWorking[term] = string.Empty;
                                                                    studentAttendanceTotPresent[term] = string.Empty;
                                                                    Termwise_TotWorking[term] = 0;
                                                                    Termwise_Present[term] = 0;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                isAuto = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            isAuto = false;
                                                        }
                                                    }
                                                    if (dsAttRemark.Tables.Count >= 2 && dsAttRemark.Tables[1].Rows.Count > 0)
                                                    {
                                                        dsAttRemark.Tables[1].DefaultView.RowFilter = "Roll_No='" + Roll_No + "' and term='" + arr_semester[term] + "'";
                                                        dvAttRmk = dsAttRemark.Tables[1].DefaultView;
                                                        if (dvAttRmk.Count > 0)
                                                        {
                                                            studentRemarks[term] = Convert.ToString(dvAttRmk[0]["totatt_remarks"]).Trim();
                                                        }
                                                        else
                                                        {
                                                            studentRemarks[term] = string.Empty;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        studentRemarks[term] = string.Empty;
                                                    }

                                                    int stcol = startcol;
                                                    if (dvTest.Count > 0)
                                                    {
                                                        tblNewTable.Cell(0, startcol).SetContent(((b_school) ? "Term " + ToRoman(Convert.ToString((arr_semester[term]))) : "Semester " + ToRoman(Convert.ToString((arr_semester[term])))));
                                                        tblNewTable.Cell(0, startcol).SetForegroundColor(Color.Black);
                                                        tblNewTable.Cell(0, startcol).SetFont(fontstudClass);
                                                        tblNewTable.Cell(0, startcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        foreach (PdfCell pc in tblNewTable.CellRange(0, startcol, 0, startcol).Cells)
                                                        {
                                                            pc.ColSpan = dvTest.Count;
                                                        }
                                                        stcol = startcol;
                                                        for (int tt = 0; tt < dvTest.Count; tt++)
                                                        {
                                                            string newtest_no = Convert.ToString(dvTest[tt]["Criteria_no"]);

                                                            dsStudMarks.Tables[1].DefaultView.RowFilter = "Criteria_no='" + newtest_no + "'";
                                                            DataTable dtTestSubjects = dsStudMarks.Tables[1].DefaultView.ToTable(true, "subject_name", "Criteria_no");
                                                            DataTable dtTestMax = dsStudMarks.Tables[1].DefaultView.ToTable(true, "Convert_Maxmark");
                                                            double maxMark = 0;
                                                            if (dtTestMax.Rows.Count > 0)
                                                            {
                                                                double.TryParse(Convert.ToString(dtTestMax.Rows[0]["Convert_Maxmark"]), out maxMark);
                                                            }
                                                            termwiseMax[term] += maxMark;
                                                            tblNewTable.Cell(1, startcol).SetContent(Convert.ToString(Convert.ToString(dvTest[tt]["criteria"]).Trim() + "\n" + Convert.ToString(maxMark).Trim() + " %").Trim());
                                                            int width = (Convert.ToString(Convert.ToString(dvTest[tt]["criteria"]).Trim() + "\n" + Convert.ToString(maxMark).Trim() + " %").Trim().Length + 10) * 10 + 10;
                                                            columnwidth[startcol - 1] = width;
                                                            tblNewTable.Cell(1, startcol).SetForegroundColor(Color.Black);
                                                            tblNewTable.Cell(1, startcol).SetFont(fontstudClass);
                                                            double beforeTot_obtained = 0;
                                                            double afterTot_obtained = 0;
                                                            double convertTotMax = 0;
                                                            double conductTotMax = 0;

                                                            int sub_row = startrow;

                                                            string conductedMaxMarks = string.Empty;
                                                            string convertedMaxMarks = string.Empty;
                                                            string conductMinmarks = string.Empty;
                                                            string convertMinMarks = string.Empty;
                                                            string obtainedMarks = string.Empty;
                                                            string minmarks = string.Empty;
                                                            string grade = string.Empty;

                                                            if (term == 0 && tt == 0)
                                                            {
                                                                sub_row = startrow;
                                                                tblNewTable.Columns[startcol].SetWidth(width);
                                                                isSubjectPrint = true;
                                                                for (int subrow = 0; subrow < dtSubjects.Rows.Count; subrow++)
                                                                {
                                                                    tblNewTable.Cell(sub_row, 0).SetContent(Convert.ToString(dtSubjects.Rows[subrow]["subject_name"]));
                                                                    tblNewTable.Cell(sub_row, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                    tblNewTable.Cell(sub_row, 0).SetForegroundColor(Color.Black);
                                                                    sub_row++;
                                                                }

                                                                tblNewTable.Cell(sub_row, 0).SetContent("ATTENDANCE\n No of Working days");
                                                                tblNewTable.Cell(sub_row, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                tblNewTable.Cell(sub_row, 0).SetFont(fontstudClass);
                                                                tblNewTable.Cell(sub_row, 0).SetForegroundColor(Color.Black);
                                                                tblNewTable.Cell(sub_row + 1, 0).SetContent("No of days present");
                                                                tblNewTable.Cell(sub_row + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                tblNewTable.Cell(sub_row + 1, 0).SetForegroundColor(Color.Black);
                                                            }
                                                            else
                                                            {
                                                                if (!isSubjectPrint)
                                                                {
                                                                    isSubjectPrint = true;
                                                                    for (int subrow = 0; subrow < dtSubjects.Rows.Count; subrow++)
                                                                    {
                                                                        tblNewTable.Cell(sub_row, 0).SetContent(Convert.ToString(dtSubjects.Rows[subrow]["subject_name"]));
                                                                        tblNewTable.Cell(sub_row, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                        tblNewTable.Cell(sub_row, 0).SetForegroundColor(Color.Black);
                                                                        sub_row++;
                                                                    }

                                                                    tblNewTable.Cell(sub_row, 0).SetContent("ATTENDANCE\n No of Working days");
                                                                    tblNewTable.Cell(sub_row, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                    tblNewTable.Cell(sub_row, 0).SetFont(fontstudClass);
                                                                    tblNewTable.Cell(sub_row, 0).SetForegroundColor(Color.Black);
                                                                    tblNewTable.Cell(sub_row + 1, 0).SetContent("No of days present");
                                                                    tblNewTable.Cell(sub_row + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                    tblNewTable.Cell(sub_row + 1, 0).SetForegroundColor(Color.Black);
                                                                }
                                                                tblNewTable.Columns[startcol].SetWidth(width);
                                                            }
                                                            sub_row = startrow;
                                                            for (int subrow = 0; subrow < dtSubjects.Rows.Count; subrow++)
                                                            {
                                                                obtainedMarks = string.Empty;
                                                                conductedMaxMarks = string.Empty;
                                                                convertedMaxMarks = string.Empty;
                                                                conductMinmarks = string.Empty;
                                                                convertMinMarks = string.Empty;
                                                                minmarks = string.Empty;
                                                                grade = string.Empty;

                                                                double obtmark = 0;
                                                                double conductMax = 0;
                                                                double convertMax = 0;
                                                                double conductMin = 0;
                                                                double convertMin = 0;

                                                                DataView dvMark = new DataView();
                                                                dtStudMarks.DefaultView.RowFilter = "Criteria_no='" + newtest_no + "' and subject_name='" + Convert.ToString(dtSubjects.Rows[subrow]["subject_name"]) + "'";
                                                                dvMark = dtStudMarks.DefaultView;
                                                                if (dvMark.Count > 0)
                                                                {
                                                                    obtmark = 0;
                                                                    conductMax = 0;
                                                                    convertMax = 0;
                                                                    conductMin = 0;
                                                                    convertMin = 0;
                                                                    obtainedMarks = Convert.ToString(dvMark[0]["marks_obtained"]).Trim();
                                                                    conductedMaxMarks = Convert.ToString(dvMark[0]["Conducted_max"]).Trim();
                                                                    convertedMaxMarks = Convert.ToString(dvMark[0]["Convert_Maxmark"]).Trim();
                                                                    conductMinmarks = Convert.ToString(dvMark[0]["Conduct_Minmark"]).Trim();
                                                                    convertMinMarks = Convert.ToString(dvMark[0]["Convert_Minmark"]).Trim();
                                                                    minmarks = string.Empty;
                                                                    grade = string.Empty;
                                                                    double.TryParse(obtainedMarks, out obtmark);
                                                                    double.TryParse(conductedMaxMarks, out conductMax);
                                                                    double.TryParse(convertedMaxMarks, out convertMax);
                                                                    double.TryParse(conductMinmarks, out conductMin);
                                                                    double.TryParse(convertMinMarks, out convertMin);
                                                                    if (obtmark < 0)
                                                                    {
                                                                        obtainedMarks = loadmarkat(Convert.ToString(obtmark));
                                                                        tblNewTable.Cell(sub_row, startcol).SetContent(Convert.ToString(obtainedMarks));
                                                                        tblNewTable.Cell(sub_row, startcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                        tblNewTable.Cell(sub_row, startcol).SetForegroundColor(Color.Red);
                                                                    }
                                                                    else
                                                                    {
                                                                        beforeTot_obtained += obtmark;
                                                                        ConvertedMark(convertedMaxMarks, ref conductedMaxMarks, ref obtainedMarks, ref minmarks);
                                                                        double.TryParse(obtainedMarks, out obtmark);
                                                                        afterTot_obtained += obtmark;
                                                                        subjectWiseTotal[subrow] += obtmark;
                                                                        if (dsStudMarks.Tables.Count == 3 && dsStudMarks.Tables[2].Rows.Count > 0)
                                                                        {
                                                                            string grademark = obtainedMarks;
                                                                            ConvertedMark("100", ref convertedMaxMarks, ref grademark, ref minmarks);
                                                                            findgrade(dsStudMarks.Tables[2], grademark, ref grade);
                                                                            tblNewTable.Cell(sub_row, startcol).SetContent(grade);
                                                                        }
                                                                        else
                                                                        {
                                                                            tblNewTable.Cell(sub_row, startcol).SetContent(obtainedMarks);
                                                                        }
                                                                        tblNewTable.Cell(sub_row, startcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                        tblNewTable.Cell(sub_row, startcol).SetForegroundColor(Color.Black);
                                                                    }
                                                                    subjectWiseMaxTotal[subrow] += convertMax;
                                                                    conductTotMax += conductMax;
                                                                    convertTotMax += convertMax;
                                                                }
                                                                else
                                                                {
                                                                    tblNewTable.Cell(sub_row, startcol).SetContent("--");
                                                                    tblNewTable.Cell(sub_row, startcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                    tblNewTable.Cell(sub_row, startcol).SetForegroundColor(Color.Black);
                                                                }
                                                                sub_row++;
                                                            }
                                                            if (tt == dvTest.Count - 1)
                                                            {
                                                                tblNewTable.Cell(sub_row, stcol).SetContent(((isAuto) ? Convert.ToString(studentAttendanceTotWorking[term] + " Days") : ""));
                                                                tblNewTable.Cell(sub_row, stcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                tblNewTable.Cell(sub_row, stcol).SetFont(fontstudClass);
                                                                tblNewTable.Cell(sub_row, stcol).SetForegroundColor(Color.Black);
                                                                foreach (PdfCell pc in tblNewTable.CellRange(sub_row, stcol, sub_row, stcol).Cells)
                                                                {
                                                                    pc.ColSpan = dvTest.Count;
                                                                }

                                                                tblNewTable.Cell(sub_row + 1, stcol).SetContent(((isAuto) ? Convert.ToString(studentAttendanceTotPresent[term] + " Days") : ""));
                                                                tblNewTable.Cell(sub_row + 1, stcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                tblNewTable.Cell(sub_row + 1, stcol).SetForegroundColor(Color.Black);
                                                                foreach (PdfCell pc in tblNewTable.CellRange(sub_row + 1, stcol, sub_row + 1, stcol).Cells)
                                                                {
                                                                    pc.ColSpan = dvTest.Count;
                                                                }
                                                            }
                                                            startcol++;
                                                        }
                                                    }
                                                    if (term == arr_semester.Length - 1)
                                                    {
                                                        string overmax = string.Empty;
                                                        double overalltotal = 0;
                                                        for (int over = 0; over < termwiseMax.Length; over++)
                                                        {
                                                            overalltotal += termwiseMax[over];
                                                            overmax += termwiseMax[over] + " % + ";
                                                        }
                                                        overmax = ("Over all Grade \n" + overmax.Trim().Trim('+') + " = (" + overalltotal + " % /" + overalltotal + " ) * 100 = 100%").Trim();
                                                        int overallgradewidth = (overmax.Trim().Length) * 5;
                                                        tblNewTable.Columns[startcol].SetWidth(overallgradewidth);
                                                        tblNewTable.Columns[startcol + 1].SetWidth(overallgradewidth / 3);
                                                        columnwidth[startcol - 1] = overallgradewidth;
                                                        columnwidth[startcol] = overallgradewidth / 3;
                                                        tblNewTable.Cell(0, startcol).SetContent(Convert.ToString(overmax));
                                                        foreach (PdfCell pc in tblNewTable.CellRange(0, startcol, 0, startcol).Cells)
                                                        {
                                                            pc.RowSpan = 2;
                                                        }
                                                        foreach (PdfCell pc in tblNewTable.CellRange(0, startcol, 0, startcol).Cells)
                                                        {
                                                            pc.ColSpan = 2;
                                                        }
                                                        tblNewTable.Cell(0, startcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        tblNewTable.Cell(0, startcol).SetFont(fontstudClass);
                                                        tblNewTable.Cell(0, startcol).SetForegroundColor(Color.Black);

                                                        int sub_row1 = startrow;
                                                        for (int subrow = 0; subrow < dtSubjects.Rows.Count; subrow++)
                                                        {
                                                            string overallgrade = string.Empty;
                                                            double total = 0;
                                                            double maxtot = 0;
                                                            double avg = 0;
                                                            double.TryParse(Convert.ToString(subjectWiseTotal[subrow]), out total);
                                                            double.TryParse(Convert.ToString(subjectWiseMaxTotal[subrow]), out maxtot);
                                                            if (total != 0 && maxtot != 0)
                                                            {
                                                                avg = (total / maxtot) * 100;
                                                                avg = Math.Round(avg, 1, MidpointRounding.AwayFromZero);
                                                            }
                                                            if (dsStudMarks.Tables.Count >= 3 && dsStudMarks.Tables[2].Rows.Count > 0)
                                                            {
                                                                findgrade(dsStudMarks.Tables[2], Convert.ToString(avg), ref overallgrade);
                                                                tblNewTable.Cell(sub_row1, startcol).SetContent(overallgrade);
                                                            }
                                                            else
                                                            {
                                                                tblNewTable.Cell(sub_row1, startcol).SetContent(avg);
                                                            }
                                                            tblNewTable.Cell(sub_row1, startcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            tblNewTable.Cell(sub_row1, startcol).SetForegroundColor(Color.Black);
                                                            foreach (PdfCell pc in tblNewTable.CellRange(sub_row1, startcol, sub_row1, startcol).Cells)
                                                            {
                                                                pc.ColSpan = 2;
                                                            }
                                                            sub_row1++;
                                                        }

                                                        sem = "select CONVERT(VARCHAR(30),Min(start_date),111) as start_date ,CONVERT(VARCHAR(30),max(end_date),111) as end_date from seminfo where degree_code='" + degree_code + "' and batch_year='" + batch_year + "'";
                                                        dsSem.Clear();
                                                        dsSem = d2.select_method_wo_parameter(sem, "Text");
                                                        if (!isManualAttendance)
                                                        {
                                                            if (dsSem.Tables.Count > 0 && dsSem.Tables[0].Rows.Count > 0)
                                                            {
                                                                string startdate = Convert.ToString(dsSem.Tables[0].Rows[0]["start_date"]);
                                                                string enddate = Convert.ToString(dsSem.Tables[0].Rows[0]["end_date"]);
                                                                currentsem = Convert.ToString((arr_semester[term]));
                                                                persentmonthcal(Roll_No, admitdate, startdate, enddate);
                                                                overall_TotWorking = per_workingdays;
                                                                overall_Present = pre_present_date;
                                                            }
                                                        }
                                                        foreach (PdfCell pc in tblNewTable.CellRange(sub_row1, startcol, sub_row1, startcol).Cells)
                                                        {
                                                            pc.ColSpan = 2;
                                                        }
                                                        tblNewTable.Cell(sub_row1, startcol).SetContent(((isAuto) ? overall_TotWorking + " Days" : ""));
                                                        tblNewTable.Cell(sub_row1, startcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        tblNewTable.Cell(sub_row1, startcol).SetFont(fontstudClass);
                                                        tblNewTable.Cell(sub_row1, startcol).SetForegroundColor(Color.Black);

                                                        tblNewTable.Cell(sub_row1 + 1, startcol).SetContent(((isAuto) ? Convert.ToString(overall_Present + " Days") : ""));
                                                        foreach (PdfCell pc in tblNewTable.CellRange(sub_row1 + 1, startcol, sub_row1 + 1, startcol).Cells)
                                                        {
                                                            pc.ColSpan = 2;
                                                        }
                                                        tblNewTable.Cell(sub_row1 + 1, startcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        tblNewTable.Cell(sub_row1 + 1, startcol).SetForegroundColor(Color.Black);
                                                        tblNewTable.Cell(sub_row1 + 2, 0).SetContent(Convert.ToString("Five Point Grading Scale\t:\t A*\t-\tOutstanding\t: 90%-100%;\t A\t-\t Excellent\t:\t75%-89%;\nB\t-\tVery Good:\t56%-74%;\tC\t-\tGood\t:\t35%-55%;\t D\t-\tScope for improvement\t:\t(Below 35%);"));
                                                        foreach (PdfCell pc in tblNewTable.CellRange(sub_row1 + 2, 0, sub_row1 + 2, 0).Cells)
                                                        {
                                                            pc.ColSpan = col + 2;
                                                        }
                                                        tblNewTable.Cell(sub_row1 + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tblNewTable.Cell(sub_row1 + 2, 0).SetForegroundColor(Color.Black);
                                                    }
                                                }
                                                tblNewTable.Columns[0].SetWidth(columnwidth.Max() + 150);
                                                CommonFrontPage(mypdfpage, mydoc: mydoc, dtStudInfo: dtStudInfo, dtcolinfo: ds.Tables[0], status: ref staus, coltop: out coltop, pdfMarkTable: tblNewTable, type: 3, colLeftLogo: CollegeLeftLogo, studPhoto: studPhoto, colRightLogo: collegeRightLogo);
                                                if (staus)
                                                {
                                                    status = true;
                                                }
                                            }
                                            else
                                            {
                                                sbErr.Append("\nRoll Number " + Roll_No + " does not have Marks. Please Check Mark Entry!!!");
                                            }
                                        }
                                        else
                                        {
                                            sbErr.Append("\nRoll Number " + Roll_No + " does not have Marks. Please Check Mark Entry!!!");
                                        }
                                    }
                                    else
                                    {
                                        sbErr.Append("\nRoll Number " + Roll_No + " does not have Marks. Please Check Mark Entry!!!");
                                    }

                                    #endregion PAGE 2

                                    #region PAGE 3

                                    double Activitytableheight = 0;
                                    int newcoltop = 25;
                                    bool newpage = false;
                                    int pageheight = 0;
                                    if (hasActivity)
                                    {
                                        mypdfpage = mydoc.NewPage();

                                        #region OUTLINE RECTANGLE

                                        pdfMainRect = new PdfRectangle(mydoc, new PdfArea(mydoc, 15, 15, mydoc.PageWidth - 30, mydoc.PageHeight - 30), Color.Black, 1);
                                        mypdfpage.Add(pdfMainRect);

                                        #endregion OUTLINE RECTANGLE

                                        coltop = 25;
                                        if (dsPart.Tables.Count > 0)
                                        {
                                            if (dtStudActivityMarks.Rows.Count > 0)
                                            {
                                                DataTable dtSubPart = new DataTable();
                                                dtStudActivityMarks.DefaultView.RowFilter = string.Empty;
                                                int totrows = 0;
                                                int temprows = 0;
                                                for (int part = 0; part < totparts; part++)
                                                {
                                                    dtStudActivityMarks.DefaultView.RowFilter = "Part_No in('" + (part + 2) + "')";
                                                    int rc = dtStudActivityMarks.DefaultView.ToTable(true, "Part_No", "TextVal", "SubTitle").Rows.Count;
                                                    int totsub = dtStudActivityMarks.DefaultView.ToTable(true, "Title_Name", "SubTitle").Rows.Count - 1;
                                                    if ((part + 2) % 2 == 0)
                                                    {
                                                        temprows = rc;
                                                        totrows += totsub;
                                                    }
                                                    else
                                                    {
                                                        if (rc >= temprows)
                                                            temprows = rc;
                                                        totrows += temprows + totsub;
                                                    }
                                                    if (part == totparts - 1)
                                                    {
                                                        totrows += temprows + totsub;
                                                    }
                                                }

                                                tblNewTable = mydoc.NewTable(fontclgAddrHeader, totrows + 2, (arr_semester.Length * 2) + 6, 2);
                                                tblNewTable.VisibleHeaders = false;
                                                tblNewTable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                int autochar = 97;
                                                int partstartrow = 0;
                                                int tempstartrow = 0;
                                                int partActStartrow = 0;
                                                int tempActivityrow = 0;
                                                int activityCounts = 0;

                                                for (int part = 0; part < totparts; part++)
                                                {
                                                    int partstartcol = 0;
                                                    string subtitle = string.Empty;
                                                    string partName = string.Empty;
                                                    string subpartTittlename = string.Empty;
                                                    dtSubPart = new DataTable();
                                                    dtStudActivityMarks.DefaultView.RowFilter = "Part_No='" + (part + 2) + "'";
                                                    dtSubPart = dtStudActivityMarks.DefaultView.ToTable(true, "Title_Name", "SubTitle");
                                                    if ((part + 2) % 2 == 0)
                                                    {
                                                        partstartcol = 0;
                                                        if (part != 0)
                                                        {
                                                            tempstartrow++;
                                                            if (tempActivityrow >= tempstartrow)
                                                                tempstartrow = tempActivityrow + 1;
                                                            if (part != totparts - 1)
                                                            {
                                                                foreach (PdfCell pc in tblNewTable.CellRange(partstartrow, 0, partstartrow, 0).Cells)
                                                                {
                                                                    pc.RowSpan = tempActivityrow - partstartrow + 1;
                                                                }
                                                                foreach (PdfCell pc in tblNewTable.CellRange(partstartrow, arr_semester.Length + 3, partstartrow, arr_semester.Length + 3).Cells)
                                                                {
                                                                    pc.RowSpan = tempActivityrow - partstartrow + 1;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                foreach (PdfCell pc in tblNewTable.CellRange(partstartrow, 0, partstartrow, 0).Cells)
                                                                {
                                                                    pc.RowSpan = tempActivityrow - partstartrow + 1;
                                                                }
                                                                foreach (PdfCell pc in tblNewTable.CellRange(partstartrow, arr_semester.Length + 3, partstartrow, arr_semester.Length + 3).Cells)
                                                                {
                                                                    pc.RowSpan = tempActivityrow - partstartrow + 1;
                                                                }
                                                            }
                                                            partstartrow = tempstartrow;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        partstartcol = arr_semester.Length + 3;
                                                        tempActivityrow = tempstartrow;
                                                        if (activityCounts < tempActivityrow)
                                                            activityCounts = tempActivityrow;
                                                        tempstartrow = partstartrow;
                                                        if (part == totparts - 1)
                                                        {
                                                            foreach (PdfCell pc in tblNewTable.CellRange(partstartrow, 0, partstartrow, 0).Cells)
                                                            {
                                                                pc.RowSpan = tempActivityrow - partstartrow + 1;
                                                            }
                                                            foreach (PdfCell pc in tblNewTable.CellRange(partstartrow, arr_semester.Length + 3, partstartrow, arr_semester.Length + 3).Cells)
                                                            {
                                                                pc.RowSpan = tempActivityrow - partstartrow + 1;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            foreach (PdfCell pc in tblNewTable.CellRange(partstartrow, 0, partstartrow, 0).Cells)
                                                            {
                                                                pc.RowSpan = tempActivityrow - partstartrow + 1;
                                                            }
                                                            foreach (PdfCell pc in tblNewTable.CellRange(partstartrow, arr_semester.Length + 3, partstartrow, arr_semester.Length + 3).Cells)
                                                            {
                                                                pc.RowSpan = tempActivityrow - partstartrow + 1;
                                                            }
                                                        }
                                                    }
                                                    if (totparts % 2 != 0 && part == totparts - 1)
                                                    {
                                                        partstartcol = arr_semester.Length + 3;
                                                        tempActivityrow = tempstartrow;
                                                        tempstartrow = partstartrow;
                                                        if (part == totparts - 1)
                                                        {
                                                            foreach (PdfCell pc in tblNewTable.CellRange(partstartrow, 0, partstartrow, 0).Cells)
                                                            {
                                                                pc.RowSpan = tempActivityrow - partstartrow + 1;
                                                            }
                                                            foreach (PdfCell pc in tblNewTable.CellRange(partstartrow, arr_semester.Length + 3, partstartrow, arr_semester.Length + 3).Cells)
                                                            {
                                                                pc.RowSpan = tempActivityrow - partstartrow + 1;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            foreach (PdfCell pc in tblNewTable.CellRange(partstartrow, 0, partstartrow, 0).Cells)
                                                            {
                                                                pc.RowSpan = tempActivityrow - partstartrow + 1;
                                                            }
                                                            foreach (PdfCell pc in tblNewTable.CellRange(partstartrow, arr_semester.Length + 3, partstartrow, arr_semester.Length + 3).Cells)
                                                            {
                                                                pc.RowSpan = tempActivityrow - partstartrow + 1;
                                                            }
                                                        }
                                                    }
                                                    if (part == 0)
                                                    {
                                                        partstartrow = 0;
                                                        tempstartrow = 0;
                                                        if (partstartcol == 0 || partstartcol == arr_semester.Length + 3)
                                                        {
                                                            int w = Convert.ToString("SUBJECTS").Trim().Length * 10 + 10;
                                                            tblNewTable.Cell(0, partstartcol).SetContent("SUBJECTS");
                                                            tblNewTable.Cell(0, partstartcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            tblNewTable.Cell(0, partstartcol).SetForegroundColor(Color.Black);
                                                            tblNewTable.Cell(0, partstartcol).SetFont(fontReportStudActivityHeader);
                                                            tblNewTable.Columns[0].SetWidth(300);
                                                            tblNewTable.Columns[1].SetWidth(50);
                                                            tblNewTable.Columns[2].SetWidth(300);
                                                            tblNewTable.Cell(0, partstartcol).SetBackgroundColor(ColorTranslator.FromHtml("#fccd99"));
                                                            foreach (PdfCell pc in tblNewTable.CellRange(0, partstartcol, 0, partstartcol).Cells)
                                                            {
                                                                pc.ColSpan = 3;
                                                            }
                                                            tblNewTable.Cell(0, arr_semester.Length + 3).SetContent("SUBJECTS");
                                                            tblNewTable.Cell(0, arr_semester.Length + 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            tblNewTable.Cell(0, arr_semester.Length + 3).SetForegroundColor(Color.Black);
                                                            tblNewTable.Cell(0, arr_semester.Length + 3).SetFont(fontReportStudActivityHeader);
                                                            tblNewTable.Columns[arr_semester.Length + 3].SetWidth(300);
                                                            tblNewTable.Columns[arr_semester.Length + 4].SetWidth(50);
                                                            tblNewTable.Columns[arr_semester.Length + 5].SetWidth(300);
                                                            tblNewTable.Cell(0, arr_semester.Length + 3).SetBackgroundColor(ColorTranslator.FromHtml("#fccd99"));
                                                            foreach (PdfCell pc in tblNewTable.CellRange(0, arr_semester.Length + 3, 0, arr_semester.Length + 3).Cells)
                                                            {
                                                                pc.ColSpan = 3;
                                                            }
                                                            for (int partsem = 0; partsem < arr_semester.Length; partsem++)
                                                            {
                                                                tblNewTable.Cell(0, partstartcol + partsem + 3).SetContent("Eval-" + arr_semester[partsem]);
                                                                tblNewTable.Cell(0, partstartcol + partsem + 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                tblNewTable.Cell(0, partstartcol + partsem + 3).SetForegroundColor(Color.Black);
                                                                w = Convert.ToString("Evaluation-" + arr_semester[partsem]).Trim().Length * 10 + 10;
                                                                tblNewTable.Columns[partstartcol + partsem + 3].SetWidth(w);
                                                                tblNewTable.Cell(0, partstartcol + partsem + 3).SetFont(fontReportStudActivityHeader);
                                                                tblNewTable.Cell(0, partstartcol + partsem + 3).SetBackgroundColor(ColorTranslator.FromHtml("#fccd99"));
                                                                tblNewTable.Cell(0, arr_semester.Length + 3 + partsem + 3).SetContent("Eval-" + arr_semester[partsem]);
                                                                tblNewTable.Cell(0, arr_semester.Length + 3 + partsem + 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                tblNewTable.Cell(0, arr_semester.Length + 3).SetForegroundColor(Color.Black);
                                                                w = Convert.ToString("Evaluation-" + arr_semester[partsem]).Trim().Length * 10 + 10;
                                                                tblNewTable.Columns[arr_semester.Length + 3 + partsem + 3].SetWidth(w + 55);
                                                                tblNewTable.Cell(0, arr_semester.Length + 3 + partsem + 3).SetFont(fontReportStudActivityHeader);
                                                                tblNewTable.Cell(0, arr_semester.Length + 3 + partsem + 3).SetBackgroundColor(ColorTranslator.FromHtml("#fccd99"));
                                                            }
                                                        }
                                                        tempstartrow++;
                                                        partstartrow = tempstartrow;
                                                    }

                                                    if (dtSubPart.Rows.Count > 0)
                                                    {
                                                        partActStartrow = partstartrow;
                                                        tempstartrow = partActStartrow;
                                                        for (int subpart = 0; subpart < dtSubPart.Rows.Count; subpart++)
                                                        {
                                                            if (subpart != 0)
                                                            {
                                                                tempstartrow++;
                                                            }
                                                            subtitle = Convert.ToString(dtSubPart.Rows[subpart][1]);
                                                            GetSubTitleName(degree_code, batch_year, subtitle, ref subpartTittlename);
                                                            tblNewTable.Cell(tempstartrow, partstartcol + 1).SetContent(subpartTittlename);
                                                            tblNewTable.Cell(tempstartrow, partstartcol + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            tblNewTable.Cell(tempstartrow, partstartcol + 1).SetForegroundColor(Color.Black);
                                                            tblNewTable.Cell(tempstartrow, partstartcol + 1).SetBackgroundColor(ColorTranslator.FromHtml("#cefecc"));
                                                            foreach (PdfCell pc in tblNewTable.CellRange(tempstartrow, partstartcol + 1, tempstartrow, partstartcol + 1).Cells)
                                                            {
                                                                pc.ColSpan = arr_semester.Length + 2;
                                                            }
                                                            if (dtStudActivityMarks.Rows.Count > 0)
                                                            {
                                                                DataView dvActivity = new DataView();
                                                                DataTable dtActivityList = new DataTable();
                                                                dtStudActivityMarks.DefaultView.RowFilter = "SubTitle='" + subtitle + "'";
                                                                dtActivityList = dtStudActivityMarks.DefaultView.ToTable(true, "UserPartName", "TextVal");
                                                                string activityname = string.Empty;
                                                                if (subpart == 0)
                                                                    activityCounts += dtActivityList.Rows.Count;
                                                                for (int allAct = 0; allAct < dtActivityList.Rows.Count; allAct++)
                                                                {
                                                                    DataTable dtActivity = new DataTable();
                                                                    dtStudActivityMarks.DefaultView.RowFilter = "SubTitle='" + subtitle + "' ";
                                                                    dvActivity = dtStudActivityMarks.DefaultView;
                                                                    dtActivity = dvActivity.ToTable();
                                                                    tblNewTable.Cell(tempstartrow, partstartcol).SetContent(Convert.ToString(dtActivityList.Rows[allAct]["UserPartName"]));
                                                                    tblNewTable.Cell(tempstartrow, partstartcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                    tblNewTable.Cell(tempstartrow, partstartcol).SetForegroundColor(Color.Black);
                                                                    tempstartrow++;
                                                                    activityname = Convert.ToString(dtActivityList.Rows[allAct]["TextVal"]);
                                                                    partName = Convert.ToString(dtActivityList.Rows[allAct]["UserPartName"]);
                                                                    tblNewTable.Cell(tempstartrow, partstartcol).SetContent(Convert.ToString(dtActivityList.Rows[allAct]["UserPartName"]));
                                                                    tblNewTable.Cell(tempstartrow, partstartcol).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                    tblNewTable.Cell(tempstartrow, partstartcol).SetForegroundColor(Color.Black);

                                                                    tblNewTable.Cell(tempstartrow, partstartcol + 1).SetContent(allAct + 1);
                                                                    tblNewTable.Cell(tempstartrow, partstartcol + 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                    tblNewTable.Cell(tempstartrow, partstartcol + 1).SetForegroundColor(Color.Black);

                                                                    tblNewTable.Cell(tempstartrow, partstartcol + 2).SetContent(activityname);
                                                                    tblNewTable.Cell(tempstartrow, partstartcol + 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                    tblNewTable.Cell(tempstartrow, partstartcol + 2).SetForegroundColor(Color.Black);
                                                                    if (dtActivity.Rows.Count > 0)
                                                                    {
                                                                        for (int act = 0; act < dtActivity.Rows.Count; act++)
                                                                        {
                                                                            string grade = string.Empty;
                                                                            for (int term = 0; term < arr_semester.Length; term++)
                                                                            {
                                                                                dtActivity.DefaultView.RowFilter = "term='" + arr_semester[term] + "'";
                                                                                dvActivity = dtActivity.DefaultView;
                                                                                grade = string.Empty;
                                                                                if (dvActivity.Count > 0)
                                                                                {
                                                                                    grade = Convert.ToString(dvActivity[0]["Grade"]);

                                                                                    tblNewTable.Cell(tempstartrow, partstartcol + term + 3).SetContent(grade);
                                                                                    tblNewTable.Cell(tempstartrow, partstartcol + term + 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                                    tblNewTable.Cell(tempstartrow, partstartcol + term + 3).SetForegroundColor(Color.Black);
                                                                                }
                                                                                else
                                                                                {
                                                                                    tblNewTable.Cell(tempstartrow, partstartcol + term + 3).SetContent("--");
                                                                                    tblNewTable.Cell(tempstartrow, partstartcol + term + 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                                    tblNewTable.Cell(tempstartrow, partstartcol + term + 3).SetForegroundColor(Color.Black);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                tblNewPage = tblNewTable.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, coltop, mydoc.PageWidth - 50, mydoc.PageHeight - coltop - 20));
                                                mypdfpage.Add(tblNewPage);
                                                Activitytableheight = tblNewPage.Area.Height;
                                                newcoltop = 0;
                                                newpage = true;
                                                pageheight = (int)Activitytableheight + coltop + 20;
                                                if (pageheight <= (mydoc.PageHeight / 2))
                                                {
                                                    newcoltop = pageheight + 25;
                                                    newpage = false;
                                                }
                                                else
                                                {
                                                    newpage = true;
                                                    newcoltop = 25;
                                                }
                                                if (staus)
                                                {
                                                    status = true;
                                                }
                                            }
                                        }
                                    }

                                    #region PAGE 4

                                    if (staus)
                                    {
                                        if (newpage)
                                        {
                                            if (staus)
                                            {
                                                status = true;
                                                mypdfpage.SaveToDocument();
                                            }
                                            coltop = 25;
                                            mypdfpage = mydoc.NewPage();
                                        }

                                        #region OUTLINE RECTANGLE

                                        pdfMainRect = new PdfRectangle(mydoc, new PdfArea(mydoc, 15, 15, mydoc.PageWidth - 30, mydoc.PageHeight - 30), Color.Black, 1);
                                        mypdfpage.Add(pdfMainRect);

                                        #endregion OUTLINE RECTANGLE

                                        string strexam = string.Empty;
                                        coltop = newcoltop;

                                        tblNewTable = mydoc.NewTable(fontReportContent, 4, selsem + 1, 5);
                                        tblNewTable.VisibleHeaders = false;
                                        tblNewTable.SetColumnsWidth(new int[] { 50 });
                                        tblNewTable.SetBorders(Color.Black, 1, BorderType.None);

                                        tblNewTable.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        tblNewTable.Cell(0, 0).SetContent("HEALTH");
                                        tblNewTable.Cell(0, 0).SetForegroundColor(Color.Black);
                                        tblNewTable.Cell(0, 0).SetFont(fontReportStudProfileHeader);
                                        foreach (PdfCell pr in tblNewTable.CellRange(0, 0, 0, 0).Cells)
                                        {
                                            pr.ColSpan = selsem + 1;
                                        }
                                        tblNewTable.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tblNewTable.Cell(1, 0).SetContent("Aspects");
                                        tblNewTable.Cell(1, 0).SetForegroundColor(Color.Black);
                                        tblNewTable.Cell(1, 0).SetFont(fontstudClass);

                                        int trm = 0;
                                        for (int i = 0; i < arr_semester.Length; i++)
                                        {
                                            trm++;
                                            strexam = "TERM -" + ToRoman(Convert.ToString(arr_semester[i]));

                                            tblNewTable.Cell(1, trm).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tblNewTable.Cell(1, trm).SetContent("TERM -" + ToRoman(Convert.ToString(arr_semester[i])));
                                            tblNewTable.Cell(1, trm).SetFont(fontstudClass);

                                            tblNewTable.Cell(1, trm).SetForegroundColor(Color.Black);
                                            tblNewTable.Cell(2, trm).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tblNewTable.Cell(2, trm).SetContent(studheight);
                                            tblNewTable.Cell(2, trm).SetForegroundColor(Color.Black);

                                            tblNewTable.Cell(3, trm).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tblNewTable.Cell(3, trm).SetContent(studweight);
                                            tblNewTable.Cell(3, trm).SetForegroundColor(Color.Black);
                                        }

                                        tblNewTable.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tblNewTable.Cell(2, 0).SetContent("Height(Cms)");
                                        tblNewTable.Cell(2, 0).SetForegroundColor(Color.Black);

                                        tblNewTable.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tblNewTable.Cell(3, 0).SetContent("Weight(Kgs)");
                                        tblNewTable.Cell(3, 0).SetForegroundColor(Color.Black);

                                        tblNewPage = tblNewTable.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 30, coltop, mydoc.PageWidth - 60, 150));
                                        mypdfpage.Add(tblNewPage);

                                        for (int index = tblNewPage.FirstRow + 1; index <= tblNewPage.LastRow; index++)
                                        {
                                            for (int cl = 0; cl < selsem + 1; cl++)
                                            {
                                                PdfRectangle pc = tblNewPage.CellArea(index, cl).ToRectangle(Color.Black);
                                                mypdfpage.Add(pc);
                                            }
                                        }

                                        double tableheight = tblNewPage.Area.Height;
                                        coltop += (int)tableheight + 20;

                                        for (int eval = 0; eval < arr_semester.Length; eval++)
                                        {
                                            strexam = "EVALUATION-" + ToRoman(Convert.ToString(arr_semester[eval]));

                                            pdftxt = new PdfTextArea(fontReportStudProfileHeader, System.Drawing.Color.Black, new PdfArea(mydoc, 2, coltop, mydoc.PageWidth, 20), System.Drawing.ContentAlignment.MiddleCenter, strexam);
                                            mypdfpage.Add(pdftxt);

                                            //studentRemarks[];

                                            strexam = "Specific participation";

                                            pdftxt = new PdfTextArea(fontReportStudProfileHeader, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop + 30, mydoc.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, strexam);
                                            mypdfpage.Add(pdftxt);

                                            strexam = "General Remarks";

                                            pdftxt = new PdfTextArea(fontReportStudProfileHeader, System.Drawing.Color.Black, new PdfArea(mydoc, 40, coltop + 90, mydoc.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, strexam);
                                            mypdfpage.Add(pdftxt);

                                            coltop += 20;
                                            pdftxt = new PdfTextArea(fontstudClass, System.Drawing.Color.Black, new PdfArea(mydoc, 45, coltop + 90, mydoc.PageWidth - 45, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(studentRemarks[eval]).Trim());
                                            mypdfpage.Add(pdftxt);

                                            pdfMainRect = new PdfRectangle(mydoc, new PdfArea(mydoc, 30, coltop + 10, mydoc.PageWidth - 60, 163), Color.Black, 1);
                                            mypdfpage.Add(pdfMainRect);
                                            coltop += 155;

                                            tblNewTable = mydoc.NewTable(fontReportContent, 1, 3, 10);
                                            tblNewTable.VisibleHeaders = false;
                                            tblNewTable.SetBorders(Color.Black, 1, BorderType.None);

                                            tblNewTable.SetContentAlignment(ContentAlignment.MiddleCenter);
                                            tblNewTable.SetForegroundColor(Color.Black);
                                            tblNewTable.SetFont(fontstudClass);
                                            tblNewTable.Cell(0, 0).SetContent("Class Teacher");
                                            tblNewTable.Cell(0, 1).SetContent("Principal");
                                            tblNewTable.Cell(0, 2).SetContent("Parent");

                                            tblNewPage = tblNewTable.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, coltop - 10, mydoc.PageWidth - 100, 50));
                                            mypdfpage.Add(tblNewPage);
                                            coltop += 40;
                                            if (coltop <= (mydoc.PageHeight - 160 - 25))
                                            {

                                            }
                                            else
                                            {
                                                coltop -= 40;
                                                mypdfpage.SaveToDocument();

                                                coltop = 25;

                                                mypdfpage = mydoc.NewPage();

                                                #region OUTLINE RECTANGLE

                                                pdfMainRect = new PdfRectangle(mydoc, new PdfArea(mydoc, 15, 15, mydoc.PageWidth - 30, mydoc.PageHeight - 30), Color.Black, 1);
                                                mypdfpage.Add(pdfMainRect);

                                                #endregion OUTLINE RECTANGLE

                                            }
                                        }

                                        strexam = "Congratulation!! Promoted to Class..............................................";
                                        pdftxt = new PdfTextArea(fontReportStudProfileHeader, System.Drawing.Color.Black, new PdfArea(mydoc, 80, coltop, mydoc.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, strexam);
                                        mypdfpage.Add(pdftxt);
                                        coltop += 25;
                                        if (coltop <= (mydoc.PageHeight - 160 - 25))
                                        {

                                        }
                                        else
                                        {
                                            coltop -= 25;
                                            mypdfpage.SaveToDocument();
                                            coltop = 25;

                                            mypdfpage = mydoc.NewPage();

                                            #region OUTLINE RECTANGLE

                                            pdfMainRect = new PdfRectangle(mydoc, new PdfArea(mydoc, 15, 15, mydoc.PageWidth - 30, mydoc.PageHeight - 30), Color.Black, 1);
                                            mypdfpage.Add(pdfMainRect);

                                            #endregion OUTLINE RECTANGLE

                                        }
                                        strexam = "New Session Begins on.......................................................................";
                                        pdftxt = new PdfTextArea(fontReportStudProfileHeader, System.Drawing.Color.Black, new PdfArea(mydoc, 80, coltop, mydoc.PageWidth, 20), System.Drawing.ContentAlignment.MiddleLeft, strexam);
                                        mypdfpage.Add(pdftxt);
                                    }
                                    if (staus)
                                    {
                                        status = true;
                                        mypdfpage.SaveToDocument();
                                    }
                                    #endregion PAGE4

                                    #endregion PAGE 3

                                }
                                else
                                {
                                    sbErr.Append("\nRoll Number " + Roll_No + " does not Exists.");
                                }
                            }
                        }
                        else
                        {
                            sbErr.Append("\nThere Is No Student Were Found");
                        }
                    }
                    else
                    {
                        sbErr.Append("\nNo College Were Found");
                    }
                }
                else
                {
                    sbErr.Append("No Test Were Conducted");
                }
            }
            else
            {
                sbErr.Append("\nPlease Select Atleast One Student");
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
                    string szFile = "ReportCard_Class_I_And_II" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
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

    public void CommonFrontPage(PdfDocument mydoc, DataTable dtStudInfo, DataTable dtcolinfo, string rpthead, string studclass, ref bool status, int type = 0, PdfImage colLeftLogo = null, PdfImage studPhoto = null, PdfImage colRightLogo = null)
    {
        try
        {
            Gios.Pdf.PdfPage mypdfpage;

            #region Font Creation

            Font fontCol_Name = new Font("Times New Roman", 16, FontStyle.Bold);
            Font fontclgAddrHeader = new Font("Times New Roman", 13, FontStyle.Regular);
            Font fontclgReportHeader = new Font("Times New Roman", 14, FontStyle.Bold);
            Font fontstudClass = new Font("Times New Roman", 13, FontStyle.Bold);
            Font fontReportContent = new Font("Times New Roman", 13, FontStyle.Regular);
            Font fontReportStudProfileHeader = new Font("Times New Roman", 12, FontStyle.Bold);

            #endregion Font Creation

            int coltop = 30;
            if (dtcolinfo.Rows.Count > 0)
            {
                string clgname = Convert.ToString(dtcolinfo.Rows[0]["collname"]);
                string clgaff = "(" + Convert.ToString(dtcolinfo.Rows[0]["affliatedby"]) + ")";
                string clgaddress1 = Convert.ToString(dtcolinfo.Rows[0]["address1"]).Trim();
                string clgaddress2 = Convert.ToString(dtcolinfo.Rows[0]["address2"]).Trim();
                string clgaddress3 = Convert.ToString(dtcolinfo.Rows[0]["address3"]).Trim();
                string clgdistrict = Convert.ToString(dtcolinfo.Rows[0]["district"]).Trim();
                string clgpincode = Convert.ToString(dtcolinfo.Rows[0]["pincode"]).Trim();
                string clgemail = "Email : " + Convert.ToString(dtcolinfo.Rows[0]["email"]).Trim();
                string clgfulladdress = string.Empty;
                PdfTextArea pdftxt;
                PdfRectangle pdfMainRect;
                PdfRectangle pdfrect;

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

                    //string moblie_no = Convert.ToString(dtStudInfo.Rows[0][""]).Trim();
                    //string phone_no = Convert.ToString(dtStudInfo.Rows[0][""]).Trim();
                    //string studemail = Convert.ToString(dtStudInfo.Rows[0][""]).Trim();

                    string studaddr = Convert.ToString(dtStudInfo.Rows[0]["parent_addressP"]).Trim();
                    string studstreetname = Convert.ToString(dtStudInfo.Rows[0]["Streetp"]).Trim();
                    string studcity = Convert.ToString(dtStudInfo.Rows[0]["Cityp"]).Trim();
                    string studdist = Convert.ToString(dtStudInfo.Rows[0]["Districtp"]).Trim();
                    string studsate = Convert.ToString(dtStudInfo.Rows[0]["parent_statep"]).Trim();
                    string studcountry = Convert.ToString(dtStudInfo.Rows[0]["countryp"]).Trim();
                    string studpincode = Convert.ToString(dtStudInfo.Rows[0]["parent_pincodep"]).Trim();

                    string studmob_no = Convert.ToString(dtStudInfo.Rows[0]["student_mobile"]).Trim();
                    string studFathermob_no = Convert.ToString(dtStudInfo.Rows[0]["parentF_Mobile"]).Trim();
                    string studMothermob_no = Convert.ToString(dtStudInfo.Rows[0]["parentM_Mobile"]).Trim();
                    string guardianmob_no = Convert.ToString(dtStudInfo.Rows[0]["guardian_mobile"]).Trim();

                    //string studEmail = Convert.ToString(dtStudInfo.Rows[0]["parentM_Mobile"]).Trim();
                    string motheremail = Convert.ToString(dtStudInfo.Rows[0]["emailM"]).Trim();
                    string fatheremail = Convert.ToString(dtStudInfo.Rows[0]["emailp"]).Trim();
                    string guardianemail = Convert.ToString(dtStudInfo.Rows[0]["gurdian_email"]).Trim();

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
                    if (studresidentialaddress.Length > 40)
                    {
                        studresidentialaddress1 = studresidentialaddress.Substring(40, studresidentialaddress.Length - 40);
                        studresidentialaddress = studresidentialaddress.Substring(0, 40);
                    }

                    #endregion Student Address For Line 1

                    #region Student Address For Line 2

                    if (studdist.Trim().Trim(',') != "")
                    {
                        if (studresidentialaddress1 != "")
                        {
                            studresidentialaddress1 += ", " + studdist.Trim().Trim(',');
                        }
                        else
                        {
                            studresidentialaddress1 = studdist.Trim().Trim(',');
                        }
                    }
                    if (studsate.Trim().Trim(',') != "")
                    {
                        if (studresidentialaddress1 != "")
                        {
                            studresidentialaddress1 += ", " + studsate.Trim().Trim(',');
                        }
                        else
                        {
                            studresidentialaddress1 = studsate.Trim().Trim(',');
                        }
                    }
                    if (studcountry.Trim().Trim(',') != "")
                    {
                        if (studresidentialaddress1 != "")
                        {
                            studresidentialaddress1 += ", " + studcountry.Trim().Trim(',');
                        }
                        else
                        {
                            studresidentialaddress1 = studcountry.Trim().Trim(',');
                        }
                    }
                    if (studpincode.Trim().Trim(',') != "")
                    {
                        if (studresidentialaddress1 != "")
                        {
                            studresidentialaddress1 += ", Pincode : " + studpincode.Trim().Trim(',');
                        }
                        else
                        {
                            studresidentialaddress1 = "Pincode : " + studpincode.Trim().Trim(',');
                        }
                    }

                    #endregion Student Address For Line 2

                    PdfLine pdfnewline;

                    mypdfpage = mydoc.NewPage();

                    #region OUTLINE RECTANGLE

                    pdfMainRect = new PdfRectangle(mydoc, new PdfArea(mydoc, 25, 25, mydoc.PageWidth - 50, mydoc.PageHeight - 50), Color.Black, 3);
                    mypdfpage.Add(pdfMainRect);

                    #endregion OUTLINE RECTANGLE

                    #region College Details

                    coltop += 20;
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

                    #endregion College Details

                    coltop += 35;

                    #region Student Photo Recangle

                    if (type == 0 || type == 1)
                    {
                        pdfrect = new PdfRectangle(mydoc, new PdfArea(mydoc, (mydoc.PageWidth / 2) + 145, coltop - 5, 98, 98), Color.Black);
                        mypdfpage.Add(pdfrect);
                    }
                    else
                    {
                        //coltop += 35;
                        pdfrect = new PdfRectangle(mydoc, new PdfArea(mydoc, (mydoc.PageWidth / 2) + 145, coltop + 80, 98, 98), Color.Black);
                        mypdfpage.Add(pdfrect);
                    }

                    #endregion Student Photo Recangle

                    #region College Right Logo

                    if (colRightLogo != null)
                    {
                        //mypdfpage.Add(colRightLogo, 70, coltop, 285);
                        if (type == 1)
                            mypdfpage.Add(colRightLogo, 70, coltop, 285);
                        else if (type == 2 || type == 3)
                            mypdfpage.Add(colRightLogo, 50, coltop + 85, 300);
                    }

                    #endregion College Right Logo

                    #region College Left Logo

                    if (colLeftLogo != null)
                    {
                        //if (type == 0 || type == 1)
                        mypdfpage.Add(colLeftLogo, (mydoc.PageWidth / 2) - 40, coltop, 285);
                    }

                    #endregion College Left Logo

                    #region Student Photos

                    if (studPhoto != null)
                    {
                        if (type == 0 || type == 1)
                            mypdfpage.Add(studPhoto, (mydoc.PageWidth / 2) + 150, coltop, 285);
                        else
                            mypdfpage.Add(studPhoto, (mydoc.PageWidth / 2) + 150, coltop + 85, 285);
                    }

                    #endregion Student Photos

                    #region Report Card Heading


                    //if (type == 0 || type == 1 || type == 2)
                    //{

                    coltop += 108;
                    int cap = 50;
                    double width = 0;
                    double height = 0;
                    double xpos = 0;
                    double xpos1 = 0;
                    double ypos = coltop;
                    if (type == 0)
                    {
                        width = 290;
                    }
                    else if (type == 1)
                    {
                        width = 235;
                    }
                    else if (type == 2)
                    {
                        width = 235;
                    }
                    else if (type == 3)
                    {
                        width = 200;
                    }
                    if (type != 3)
                    {
                        cap = 40;
                        xpos = (mydoc.PageWidth / 2) - ((width / 2) + 3);
                        xpos1 = (mydoc.PageWidth / 2) - ((width / 2));
                        ypos = coltop - 3;
                    }
                    else
                    {
                        cap = 30;
                        xpos = (mydoc.PageWidth / 2) - ((width / 2) - 3);
                        xpos1 = (mydoc.PageWidth / 2) - ((width / 2));
                        ypos = coltop + 3;
                    }
                    pdfrect = new PdfRectangle(mydoc, new PdfArea(mydoc, xpos, ypos, width, 30), Color.Black, Color.Black);
                    pdfrect.StrokeWidth = 3;
                    mypdfpage.Add(pdfrect);
                    pdfrect = new PdfRectangle(mydoc, new PdfArea(mydoc, xpos1, coltop, width, 30), Color.Black, Color.White);
                    pdfrect.StrokeWidth = 2;
                    mypdfpage.Add(pdfrect);
                    pdftxt = new PdfTextArea(fontclgReportHeader, Color.Black, new PdfArea(mydoc, 5, coltop + 7, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, rpthead);
                    mypdfpage.Add(pdftxt);
                    //}
                    //else
                    //{
                    //    coltop += 110;
                    //    pdfrect = new PdfRectangle(mydoc, new PdfArea(mydoc, (mydoc.PageWidth / 2) - 153, coltop - 3, 300, 40), Color.Black, Color.Black);
                    //    pdfrect.StrokeWidth = 3;
                    //    mypdfpage.Add(pdfrect);
                    //    pdfrect = new PdfRectangle(mydoc, new PdfArea(mydoc, (mydoc.PageWidth / 2) - 150, coltop, 300, 40), Color.Black, Color.White);
                    //    pdfrect.StrokeWidth = 2;
                    //    mypdfpage.Add(pdfrect);
                    //    pdftxt = new PdfTextArea(fontclgReportHeader, Color.Black, new PdfArea(mydoc, 5, coltop + 10, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, rpthead);
                    //    mypdfpage.Add(pdftxt);
                    //}

                    #endregion Report Card Heading

                    #region Academic Year
                    string academicyear = d2.GetFunctionv("select value from master_settings where settings='Academic year'");
                    academicyear = "(Academic Year " + academicyear.Trim().Trim(',').Replace(",", "-") + ")";
                    if (type == 1 || type == 2)
                    {
                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 5, coltop + 40, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, academicyear);
                        mypdfpage.Add(pdftxt);
                    }

                    #endregion Academic Year

                    #region Student Class

                    coltop += 50;
                    pdftxt = new PdfTextArea(fontstudClass, Color.Black, new PdfArea(mydoc, 5, coltop + 10, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, studclass);
                    mypdfpage.Add(pdftxt);

                    #endregion Student Class

                    #region Academic Year For Format 0

                    if (type == 3)
                    {
                        coltop += 20;
                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 5, coltop + 10, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, academicyear);
                        mypdfpage.Add(pdftxt);

                        #region Student Prpfile

                        coltop += cap;
                        pdftxt = new PdfTextArea(fontReportStudProfileHeader, Color.Black, new PdfArea(mydoc, 60, coltop + 10, (mydoc.PageWidth / 2) - 30, 20), ContentAlignment.MiddleLeft, "Student Profile");
                        mypdfpage.Add(pdftxt);

                        #endregion Student Prpfile

                    }

                    #endregion Academic Year  For Format 0

                    #region Admission Number For Format 0

                    if (type == 0)
                    {
                        coltop += 35;
                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 5, coltop + 7, mydoc.PageWidth - 60, 20), ContentAlignment.MiddleRight, Admit_no);
                        mypdfpage.Add(pdftxt);
                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 5, coltop + 10, mydoc.PageWidth - 45, 20), ContentAlignment.MiddleRight, "Admn.No..................");
                        mypdfpage.Add(pdftxt);
                    }

                    #endregion Admission Number For Format 0

                    #region Student Name

                    string studnameheader = string.Empty;
                    if (type == 0 || type == 3)
                    {
                        studnameheader = "Name of the Student ";
                    }
                    else if (type == 1 || type == 2)
                    {
                        studnameheader = "Name \t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t";
                    }

                    coltop += cap;
                    pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 60, coltop + 10, (mydoc.PageWidth / 2) - 30, 20), ContentAlignment.MiddleLeft, studnameheader + "\t\t\t\t\t\t\t\t\t\t\t\t\\t\t\t\t : \t\t\t");
                    mypdfpage.Add(pdftxt);

                    pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)((mydoc.PageWidth / 2) - 50), coltop + 10, (mydoc.PageWidth - 40), 20), ContentAlignment.MiddleLeft, studname);
                    mypdfpage.Add(pdftxt);

                    pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) - 50), coltop + 25), new PointF((float)((mydoc.PageWidth - 40)), coltop + 25), Color.Black, 1);
                    mypdfpage.Add(pdfnewline);

                    #endregion Student Name

                    #region Class And Section

                    coltop += cap;
                    if (type != 3)
                    {
                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 60, coltop + 10, (mydoc.PageWidth / 2) - 30, 20), ContentAlignment.MiddleLeft, "Class\t\t\t\t\t\t\t\t\t\t\t\t\\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t: \t\t\t");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)((mydoc.PageWidth / 2) - 50), coltop + 10, ((mydoc.PageWidth / 2) + 80), 20), ContentAlignment.MiddleLeft, standard);
                        mypdfpage.Add(pdftxt);

                        pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) - 50), coltop + 25), new PointF((float)(((mydoc.PageWidth / 2) + 80)), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);


                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, ((mydoc.PageWidth / 2) + 83), coltop + 10, ((mydoc.PageWidth / 2) + 100), 20), ContentAlignment.MiddleLeft, "Section \t\t\t : \t\t\t");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, ((mydoc.PageWidth / 2) + 145), coltop + 10, (mydoc.PageWidth - 40), 20), ContentAlignment.MiddleLeft, section);
                        mypdfpage.Add(pdftxt);

                        pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) + 145), coltop + 25), new PointF((float)((mydoc.PageWidth - 40)), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);
                    }
                    else
                    {
                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 60, coltop + 10, (mydoc.PageWidth / 2) - 30, 20), ContentAlignment.MiddleLeft, "Class & Sec \t\\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t: \t\t\t");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)((mydoc.PageWidth / 2) - 50), coltop + 10, ((mydoc.PageWidth / 2) - 40), 20), ContentAlignment.MiddleLeft, standard + " & " + section);
                        mypdfpage.Add(pdftxt);

                        pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) - 50), coltop + 25), new PointF((float)((mydoc.PageWidth - 40)), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);


                        //pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)((mydoc.PageWidth / 2) - 50), coltop + 10, (mydoc.PageWidth - 40), 20), ContentAlignment.MiddleLeft, studname);
                        //mypdfpage.Add(pdftxt);

                        //pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) - 50), coltop + 25), new PointF((float)((mydoc.PageWidth - 40)), coltop + 25), Color.Black, 1);
                        //mypdfpage.Add(pdfnewline);

                    }

                    #endregion Class And Section

                    #region Admision Number And Exam Number

                    if (type != 0 && type == 3)
                    {
                        coltop += cap;
                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 60, coltop + 10, (mydoc.PageWidth / 2) - 30, 20), ContentAlignment.MiddleLeft, "Admission No. \t\t\t\t\t\t\t\t\t\t\t\\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t: \t\t\t");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)((mydoc.PageWidth / 2) - 50), coltop + 10, ((mydoc.PageWidth / 2) + 80), 20), ContentAlignment.MiddleLeft, Admit_no);
                        mypdfpage.Add(pdftxt);

                        pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) - 50), coltop + 25), new PointF((float)(((mydoc.PageWidth / 2) + 80)), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);


                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, ((mydoc.PageWidth / 2) + 83), coltop + 10, ((mydoc.PageWidth / 2) + 100), 20), ContentAlignment.MiddleLeft, "Exam. No \t\t : \t\t\t");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, ((mydoc.PageWidth / 2) + 148), coltop + 10, (mydoc.PageWidth - 40), 20), ContentAlignment.MiddleLeft, reg_no);
                        mypdfpage.Add(pdftxt);

                        pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) + 148), coltop + 25), new PointF((float)((mydoc.PageWidth - 40)), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);
                    }

                    #endregion

                    #region DOB

                    coltop += cap;
                    pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 60, coltop + 10, (mydoc.PageWidth / 2) - 30, 20), ContentAlignment.MiddleLeft, "Date of Birth \t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\\t\t\t\t\t\t\t\t\t\t : \t\t\t");
                    mypdfpage.Add(pdftxt);

                    pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)((mydoc.PageWidth / 2) - 50), coltop + 10, (mydoc.PageWidth - 40), 20), ContentAlignment.MiddleLeft, dob);
                    mypdfpage.Add(pdftxt);

                    pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) - 50), coltop + 25), new PointF((float)((mydoc.PageWidth - 40)), coltop + 25), Color.Black, 1);
                    mypdfpage.Add(pdfnewline);

                    #endregion DOB

                    #region Admision Number And Exam Number

                    if (type != 0 && type != 3)
                    {
                        coltop += cap;
                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 60, coltop + 10, (mydoc.PageWidth / 2) - 30, 20), ContentAlignment.MiddleLeft, "Admission No. \t\t\t\t\t\t\t\t\t\t\t\\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t: \t\t\t");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)((mydoc.PageWidth / 2) - 50), coltop + 10, ((mydoc.PageWidth / 2) + 80), 20), ContentAlignment.MiddleLeft, Admit_no);
                        mypdfpage.Add(pdftxt);

                        pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) - 50), coltop + 25), new PointF((float)(((mydoc.PageWidth / 2) + 80)), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);


                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, ((mydoc.PageWidth / 2) + 83), coltop + 10, ((mydoc.PageWidth / 2) + 100), 20), ContentAlignment.MiddleLeft, "Exam. No \t: \t\t\t");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, ((mydoc.PageWidth / 2) + 148), coltop + 10, (mydoc.PageWidth - 40), 20), ContentAlignment.MiddleLeft, reg_no);
                        mypdfpage.Add(pdftxt);

                        pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) + 148), coltop + 25), new PointF((float)((mydoc.PageWidth - 40)), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);
                    }

                    #endregion

                    #region Father/Guardian Name

                    coltop += cap;
                    pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 60, coltop + 10, (mydoc.PageWidth / 2) - 30, 20), ContentAlignment.MiddleLeft, "Name of the Father/Guardian \t\t\t: \t\t\t");
                    mypdfpage.Add(pdftxt);

                    pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)((mydoc.PageWidth / 2) - 50), coltop + 10, (mydoc.PageWidth - 40), 20), ContentAlignment.MiddleLeft, (father_name != "") ? father_name : guardian_name);
                    mypdfpage.Add(pdftxt);

                    pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) - 50), coltop + 25), new PointF((float)((mydoc.PageWidth - 40)), coltop + 25), Color.Black, 1);
                    mypdfpage.Add(pdfnewline);

                    #endregion Father/Guardian Name

                    #region Mother Name

                    coltop += cap;
                    pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 60, coltop + 10, (mydoc.PageWidth / 2) - 30, 20), ContentAlignment.MiddleLeft, "Name of the Mother \t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t  : \t\t\t");
                    mypdfpage.Add(pdftxt);

                    pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)((mydoc.PageWidth / 2) - 50), coltop + 10, (mydoc.PageWidth - 40), 20), ContentAlignment.MiddleLeft, mother_name);
                    mypdfpage.Add(pdftxt);

                    pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) - 50), coltop + 25), new PointF((float)((mydoc.PageWidth - 40)), coltop + 25), Color.Black, 1);
                    mypdfpage.Add(pdfnewline);

                    #endregion Mother Name

                    #region Residential Address

                    coltop += cap;
                    pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 60, coltop + 10, (mydoc.PageWidth / 2) - 30, 20), ContentAlignment.MiddleLeft, "Residential Address \t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t  : \t\t\t");
                    mypdfpage.Add(pdftxt);

                    pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)((mydoc.PageWidth / 2) - 50), coltop + 10, 350, 20), ContentAlignment.MiddleLeft, studresidentialaddress);
                    mypdfpage.Add(pdftxt);

                    pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) - 50), coltop + 25), new PointF((float)((mydoc.PageWidth - 40)), coltop + 25), Color.Black, 1);
                    mypdfpage.Add(pdfnewline);
                    coltop += 25;
                    pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)((mydoc.PageWidth / 2) - 50), coltop + 10, 350, 20), ContentAlignment.MiddleLeft, studresidentialaddress1);
                    mypdfpage.Add(pdftxt);
                    pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) - 50), coltop + 25), new PointF((float)((mydoc.PageWidth - 40)), coltop + 25), Color.Black, 1);
                    mypdfpage.Add(pdfnewline);

                    #endregion Residential Address

                    #region Blood Group and Phone No/ Mobile No

                    coltop += cap;
                    if (type == 0)
                    {
                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 60, coltop + 10, 100, 20), ContentAlignment.MiddleLeft, "Blood Group : ");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 160, coltop + 10, 100, 20), ContentAlignment.MiddleLeft, blood_grp);
                        mypdfpage.Add(pdftxt);

                        pdfnewline = new PdfLine(mydoc, new PointF(160, coltop + 25), new PointF((float)(270), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 280, coltop + 10, 100, 20), ContentAlignment.MiddleLeft, "Phone/Mobile No. : ");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 380, coltop + 10, 100, 20), ContentAlignment.MiddleLeft, mobile_no);
                        mypdfpage.Add(pdftxt);

                        pdfnewline = new PdfLine(mydoc, new PointF(380, coltop + 25), new PointF((float)(mydoc.PageWidth - 40), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);
                    }
                    else
                    {
                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 60, coltop + 10, 100, 20), ContentAlignment.MiddleLeft, "Telephone No. \t\t\t\t\t\t\t\t\t\t\t\\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t: \t\t\t");
                        mypdfpage.Add(pdftxt);


                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)((mydoc.PageWidth / 2) - 50), coltop + 10, (mydoc.PageWidth - 40), 20), ContentAlignment.MiddleLeft, mobile_no);
                        mypdfpage.Add(pdftxt);

                        //pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 160, coltop + 10, 100, 20), ContentAlignment.MiddleLeft, blood_grp);
                        //mypdfpage.Add(pdftxt);

                        //pdfnewline = new PdfLine(mydoc, new PointF(160, coltop + 25), new PointF((float)(270), coltop + 25), Color.Black, 1);
                        //mypdfpage.Add(pdfnewline);
                        pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) - 50), coltop + 25), new PointF((float)((mydoc.PageWidth - 40)), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);
                    }

                    #endregion Blood Group and Phone No/ Mobile No

                    #region Attendance Details

                    if (type == 3)
                    {
                        string Tot_Sec_Att_term1 = "", TotWorking_term1 = "", Tot_Sec_Att_term2 = "", TotWorking_term2 = string.Empty;
                        int term = 0;
                        int.TryParse(cur_sem.Trim(), out term);
                        DataSet dsSem = new DataSet();
                        for (int i = 1; i <= term; i++)
                        {
                            string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + i + "' and degree_code='" + deg_code + "' and batch_year='" + batch_year + "'";
                            dsSem = d2.select_method_wo_parameter(sem, "Text");

                            if (dsSem.Tables.Count > 0 && dsSem.Tables[0].Rows.Count > 0)
                            {
                                string startdate = Convert.ToString(dsSem.Tables[0].Rows[0]["start_date"]);
                                string enddate = Convert.ToString(dsSem.Tables[0].Rows[0]["end_date"]);
                                currentsem = Convert.ToString(i);
                                persentmonthcal(roll_no, admitdate, startdate, enddate);
                            }

                            if (i == 1)
                            {
                                Tot_Sec_Att_term1 = Convert.ToString(pre_present_date);
                                TotWorking_term1 = Convert.ToString(per_workingdays);
                                //working1 =  Convert.ToString(pre_present_date);
                                //present1 =  Convert.ToString(per_workingdays);
                            }
                            else if (i == 2)
                            {
                                Tot_Sec_Att_term2 = Convert.ToString(pre_present_date);
                                TotWorking_term2 = Convert.ToString(per_workingdays);
                            }
                        }

                        coltop += cap + 10;
                        pdftxt = new PdfTextArea(fontReportStudProfileHeader, Color.Black, new PdfArea(mydoc, 60, coltop + 10, (mydoc.PageWidth / 2) - 30, 20), ContentAlignment.MiddleLeft, "Attendance");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportStudProfileHeader, Color.Black, new PdfArea(mydoc, (mydoc.PageWidth / 2) + 20, coltop + 10, (mydoc.PageWidth / 4) - 30, 20), ContentAlignment.MiddleCenter, "Term-I");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportStudProfileHeader, Color.Black, new PdfArea(mydoc, (mydoc.PageWidth / 2) + (mydoc.PageWidth / 4), coltop + 10, (mydoc.PageWidth / 4) - 30, 20), ContentAlignment.MiddleCenter, "Term-II");
                        mypdfpage.Add(pdftxt);

                        coltop += cap;
                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 60, coltop + 10, (mydoc.PageWidth / 2) - 30, 20), ContentAlignment.MiddleLeft, "Total Working Days ");
                        mypdfpage.Add(pdftxt);


                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (mydoc.PageWidth / 2) + 20, coltop + 10, (mydoc.PageWidth / 4) - 30, 20), ContentAlignment.MiddleCenter, (TotWorking_term1.Trim() != "") ? TotWorking_term1 + " Days" : "");
                        mypdfpage.Add(pdftxt);

                        pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) + 20), coltop + 25), new PointF((float)((mydoc.PageWidth / 2) + (mydoc.PageWidth / 4) - 10), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (mydoc.PageWidth / 2) + (mydoc.PageWidth / 4), coltop + 10, (mydoc.PageWidth / 4) - 30, 20), ContentAlignment.MiddleCenter, (TotWorking_term2.Trim() != "") ? TotWorking_term2 + " Days" : "");
                        mypdfpage.Add(pdftxt);

                        pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) + (mydoc.PageWidth / 4)), coltop + 25), new PointF((float)((mydoc.PageWidth / 2) + (mydoc.PageWidth / 4) + (mydoc.PageWidth / 4) - 30), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);



                        coltop += cap;
                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 60, coltop + 10, (mydoc.PageWidth / 2) - 30, 20), ContentAlignment.MiddleLeft, "Total Attendance of the Student ");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (mydoc.PageWidth / 2) + 20, coltop + 10, (mydoc.PageWidth / 4) - 30, 20), ContentAlignment.MiddleCenter, (Tot_Sec_Att_term1.Trim() != "") ? Tot_Sec_Att_term1 + " Days" : "--");
                        mypdfpage.Add(pdftxt);

                        pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) + 20), coltop + 25), new PointF((float)((mydoc.PageWidth / 2) + (mydoc.PageWidth / 4) - 10), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (mydoc.PageWidth / 2) + (mydoc.PageWidth / 4), coltop + 10, (mydoc.PageWidth / 4) - 30, 20), ContentAlignment.MiddleCenter, (Tot_Sec_Att_term2.Trim() != "") ? Tot_Sec_Att_term2 + " Days" : "--");
                        mypdfpage.Add(pdftxt);

                        pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) + (mydoc.PageWidth / 4)), coltop + 25), new PointF((float)((mydoc.PageWidth / 2) + (mydoc.PageWidth / 4) + (mydoc.PageWidth / 4) - 30), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);

                    }

                    #endregion Attendance Details

                    #region Footer

                    if (type != 3)
                        coltop += 90;
                    else
                        coltop += 10;
                    if (type == 0)
                    {
                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 60, coltop + 10, (mydoc.PageWidth / 2) - 30, 20), ContentAlignment.MiddleCenter, "Class Teacher");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (mydoc.PageWidth / 2), coltop + 10, (mydoc.PageWidth / 2) - 45, 20), ContentAlignment.MiddleCenter, "Principal");
                        mypdfpage.Add(pdftxt);
                    }
                    else if (type != 3)
                    {
                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 60, coltop + 10, (mydoc.PageWidth / 2) - 170, 90), ContentAlignment.TopLeft, "Speciment Signature of Parents / Guardian ");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)((mydoc.PageWidth / 2) - 50), coltop + 10, 100, 20), ContentAlignment.MiddleLeft, "Father ");
                        mypdfpage.Add(pdftxt);

                        //pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)((mydoc.PageWidth / 2)-10), coltop + 10, ((mydoc.PageWidth / 2) + 78), 20), ContentAlignment.MiddleLeft, "");
                        //mypdfpage.Add(pdftxt);

                        pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) - 10), coltop + 25), new PointF((float)(((mydoc.PageWidth / 2) + 78)), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, ((mydoc.PageWidth / 2) + 83), coltop + 10, ((mydoc.PageWidth / 2) + 100), 20), ContentAlignment.MiddleLeft, "Mother ");
                        mypdfpage.Add(pdftxt);

                        //pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, ((mydoc.PageWidth / 2) + 128), coltop + 10, (mydoc.PageWidth - 40), 20), ContentAlignment.MiddleLeft, "");
                        //mypdfpage.Add(pdftxt);

                        pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2) + 128), coltop + 25), new PointF((float)((mydoc.PageWidth - 40)), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);

                        coltop += cap;
                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, ((mydoc.PageWidth / 2) - 50), coltop + 10, (mydoc.PageWidth / 2) - 30, 20), ContentAlignment.MiddleLeft, "Guardian ");
                        mypdfpage.Add(pdftxt);

                        //pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (float)((mydoc.PageWidth / 2)), coltop + 10, (mydoc.PageWidth - 40), 20), ContentAlignment.MiddleLeft, "");
                        //mypdfpage.Add(pdftxt);

                        pdfnewline = new PdfLine(mydoc, new PointF((float)((mydoc.PageWidth / 2)), coltop + 25), new PointF((float)((mydoc.PageWidth - 40)), coltop + 25), Color.Black, 1);
                        mypdfpage.Add(pdfnewline);

                    }
                    else
                    {
                        coltop += 30;
                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 45, coltop, 100, 90), ContentAlignment.MiddleLeft, "Signature");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 145, coltop, 100, 90), ContentAlignment.MiddleLeft, "Student");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (mydoc.PageWidth / 2) - 50, coltop, 100, 90), ContentAlignment.MiddleLeft, "Class Teacher");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, (mydoc.PageWidth / 2) + 70, coltop, 100, 90), ContentAlignment.MiddleLeft, "Principal");
                        mypdfpage.Add(pdftxt);

                        pdftxt = new PdfTextArea(fontReportContent, Color.Black, new PdfArea(mydoc, 0, coltop, mydoc.PageWidth - 75, 90), ContentAlignment.MiddleRight, "Parent");
                        mypdfpage.Add(pdftxt);

                    }

                    #endregion Footer

                    if (status)
                        mypdfpage.SaveToDocument();

                }
            }
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

    //public void CommonFrontPage(PdfPage mypdfpage, PdfDocument mydoc, DataTable dtStudInfo, DataTable dtcolinfo, ref bool status, out int coltop, PdfTable pdfMarkTable, int type = 0, PdfImage colLeftLogo = null, PdfImage studPhoto = null, PdfImage colRightLogo = null, int[] semester = null)
    //{
    //    coltop = 0;
    //    try
    //    {
    //        #region Font Creation

    //        Font fontCol_Name = new Font("Book Antiqua", 16, FontStyle.Bold);
    //        Font fontclgAddrHeader = new Font("Book Antiqua", 11, FontStyle.Regular);
    //        Font fontclgReportHeader = new Font("Book Antiqua", 13, FontStyle.Bold);
    //        Font fontstudClass = new Font("Book Antiqua", 13, FontStyle.Regular);
    //        Font fontReportContent = new Font("Book Antiqua", 13, FontStyle.Regular);
    //        Font fontStudentDetailsContent = new Font("Book Antiqua", 10, FontStyle.Regular);
    //        Font fontReportStudProfileHeader = new Font("Book Antiqua", 12, FontStyle.Bold);

    //        #endregion Font Creation

    //        if (dicHeaderAll.Count == 0)
    //        {
    //            reportHeaderBased = 0;
    //            GetHeaderSettings(ref dicHeaderAll, ref reportHeaderBased);
    //        }

    //        if (dicHeaderAll.Count > 0)
    //        {

    //        }

    //        coltop = 15;
    //        if (dtcolinfo.Rows.Count > 0)
    //        {
    //            string clgname = Convert.ToString(dtcolinfo.Rows[0]["collname"]).Trim();
    //            string clgaff = "(" + Convert.ToString(dtcolinfo.Rows[0]["affliatedby"]).Trim() + ")";
    //            string clgaddress1 = Convert.ToString(dtcolinfo.Rows[0]["address1"]).Trim();
    //            string clgaddress2 = Convert.ToString(dtcolinfo.Rows[0]["address2"]).Trim();
    //            string clgaddress3 = Convert.ToString(dtcolinfo.Rows[0]["address3"]).Trim();
    //            string clgdistrict = Convert.ToString(dtcolinfo.Rows[0]["district"]).Trim();
    //            string clgpincode = Convert.ToString(dtcolinfo.Rows[0]["pincode"]).Trim();
    //            string clgemail = "Email : " + Convert.ToString(dtcolinfo.Rows[0]["email"]).Trim();
    //            string clgwebsite = "Website : " + Convert.ToString(dtcolinfo.Rows[0]["website"]).Trim();
    //            string clgfulladdress = string.Empty;
    //            PdfTextArea pdftxt;
    //            PdfRectangle pdfMainRect;
    //            PdfRectangle pdfrect;
    //            PdfLine pdfnewline;
    //            PdfTable pdfNewTable;
    //            PdfTablePage pdfNewTablePage;

    //            #region College Address

    //            if (clgaddress1.Trim().Trim(',') != "")
    //            {
    //                clgfulladdress = clgaddress1.Trim().Trim(',');
    //            }
    //            if (clgaddress2.Trim().Trim(',') != "")
    //            {
    //                if (clgfulladdress != "")
    //                {
    //                    clgfulladdress += ", " + clgaddress2.Trim().Trim(',');
    //                }
    //                else
    //                {
    //                    clgfulladdress = clgaddress2.Trim().Trim(',');
    //                }
    //            }
    //            if (clgaddress3.Trim().Trim(',') != "")
    //            {
    //                if (clgfulladdress != "")
    //                {
    //                    clgfulladdress += ", " + clgaddress3.Trim().Trim(',');
    //                }
    //                else
    //                {
    //                    clgfulladdress = clgaddress3.Trim().Trim(',');
    //                }
    //            }
    //            if (clgdistrict.Trim().Trim(',') != "")
    //            {
    //                if (clgfulladdress != "")
    //                {
    //                    clgfulladdress += ", " + clgdistrict.Trim().Trim(',');
    //                }
    //                else
    //                {
    //                    clgfulladdress = clgdistrict.Trim().Trim(',');
    //                }
    //            }
    //            if (clgpincode.Trim().Trim(',') != "")
    //            {
    //                if (clgfulladdress != "")
    //                {
    //                    clgfulladdress += "-" + clgpincode.Trim().Trim(',').Trim('.') + ".";
    //                }
    //                else
    //                {
    //                    clgfulladdress = clgpincode.Trim().Trim(',').Trim('.') + ".";
    //                }
    //            }

    //            #endregion College Address

    //            if (dtStudInfo.Rows.Count > 0)
    //            {
    //                string studname = Convert.ToString(dtStudInfo.Rows[0]["stud_name"]).Trim();
    //                string Admit_no = Convert.ToString(dtStudInfo.Rows[0]["roll_admit"]).Trim();
    //                string roll_no = Convert.ToString(dtStudInfo.Rows[0]["Roll_No"]).Trim();
    //                string reg_no = Convert.ToString(dtStudInfo.Rows[0]["Reg_No"]).Trim();

    //                string admitdate = Convert.ToString(dtStudInfo.Rows[0]["adm_date"]).Trim();
    //                string cur_sem = Convert.ToString(dtStudInfo.Rows[0]["Current_Semester"]).Trim();
    //                string standard = Convert.ToString(dtStudInfo.Rows[0]["Dept_Name"]).Trim();
    //                string section = Convert.ToString(dtStudInfo.Rows[0]["Sections"]).Trim();
    //                string dob = Convert.ToString(dtStudInfo.Rows[0]["dob"]).Trim();
    //                string blood_grp = Convert.ToString(dtStudInfo.Rows[0]["Blood_Grp"]).Trim();

    //                string mother_name = Convert.ToString(dtStudInfo.Rows[0]["mother"]).Trim();
    //                string father_name = Convert.ToString(dtStudInfo.Rows[0]["parent_name"]).Trim();
    //                string guardian_name = Convert.ToString(dtStudInfo.Rows[0]["guardian_name"]).Trim();

    //                string deg_code = Convert.ToString(dtStudInfo.Rows[0]["degree_code"]).Trim();
    //                string batch_year = Convert.ToString(dtStudInfo.Rows[0]["Batch_Year"]).Trim();

    //                string studaddr = Convert.ToString(dtStudInfo.Rows[0]["parent_addressP"]).Trim();
    //                string studstreetname = Convert.ToString(dtStudInfo.Rows[0]["Streetp"]).Trim();
    //                string studcity = Convert.ToString(dtStudInfo.Rows[0]["Cityp"]).Trim();
    //                string studdist = Convert.ToString(dtStudInfo.Rows[0]["Districtp"]).Trim();
    //                string studsate = Convert.ToString(dtStudInfo.Rows[0]["parent_statep"]).Trim();
    //                string studcountry = Convert.ToString(dtStudInfo.Rows[0]["countryp"]).Trim();
    //                string studpincode = Convert.ToString(dtStudInfo.Rows[0]["parent_pincodep"]).Trim();

    //                string studmob_no = Convert.ToString(dtStudInfo.Rows[0]["student_mobile"]).Trim();
    //                string studFathermob_no = Convert.ToString(dtStudInfo.Rows[0]["parentF_Mobile"]).Trim();
    //                string studMothermob_no = Convert.ToString(dtStudInfo.Rows[0]["parentM_Mobile"]).Trim();
    //                string guardianmob_no = Convert.ToString(dtStudInfo.Rows[0]["guardian_mobile"]).Trim();

    //                //string studEmail = Convert.ToString(dtStudInfo.Rows[0]["parentM_Mobile"]).Trim();
    //                string motheremail = Convert.ToString(dtStudInfo.Rows[0]["emailM"]).Trim();
    //                string fatheremail = Convert.ToString(dtStudInfo.Rows[0]["emailp"]).Trim();
    //                string guardianemail = Convert.ToString(dtStudInfo.Rows[0]["gurdian_email"]).Trim();

    //                string studclassandsec = ((standard != "") ? standard + ((section != "") ? " & " + section : "") : ((section != "") ? section : ""));
    //                string mobile_no = ((studFathermob_no != "") ? studFathermob_no : "") + ((studFathermob_no != "" && guardianmob_no != "") ? "," + guardianmob_no : ((guardianmob_no != "") ? guardianmob_no : ""));
    //                //string medium = Convert.ToString(dtStudInfo.Rows[0][""]);,
    //                string studresidentialaddress = "", studresidentialaddress1 = string.Empty;

    //                #region Student Address For Line 1

    //                if (studaddr.Trim().Trim(',') != "")
    //                {
    //                    studresidentialaddress = studaddr.Trim().Trim(',');
    //                }
    //                if (studstreetname.Trim().Trim(',') != "")
    //                {
    //                    if (studresidentialaddress != "")
    //                    {
    //                        studresidentialaddress += ", " + studstreetname.Trim().Trim(',');
    //                    }
    //                    else
    //                    {
    //                        studresidentialaddress = studstreetname.Trim().Trim(',');
    //                    }
    //                }
    //                if (studcity.Trim().Trim(',') != "")
    //                {
    //                    if (studresidentialaddress != "")
    //                    {
    //                        studresidentialaddress += ", " + studcity.Trim().Trim(',');
    //                    }
    //                    else
    //                    {
    //                        studresidentialaddress = studcity.Trim().Trim(',');
    //                    }
    //                }
    //                //if (studresidentialaddress.Length > 40)
    //                //{
    //                //    studresidentialaddress1 = studresidentialaddress.Substring(40, studresidentialaddress.Length - 40);
    //                //    studresidentialaddress = studresidentialaddress.Substring(0, 40);
    //                //}

    //                #endregion Student Address For Line 1

    //                #region Student Address For Line 2

    //                if (studdist.Trim().Trim(',') != "")
    //                {
    //                    if (studresidentialaddress != "")
    //                    {
    //                        studresidentialaddress += ", " + studdist.Trim().Trim(',');
    //                    }
    //                    else
    //                    {
    //                        studresidentialaddress = studdist.Trim().Trim(',');
    //                    }
    //                }
    //                if (studsate.Trim().Trim(',') != "")
    //                {
    //                    if (studresidentialaddress != "")
    //                    {
    //                        studresidentialaddress += ", " + studsate.Trim().Trim(',');
    //                    }
    //                    else
    //                    {
    //                        studresidentialaddress = studsate.Trim().Trim(',');
    //                    }
    //                }
    //                if (studcountry.Trim().Trim(',') != "")
    //                {
    //                    if (studresidentialaddress != "")
    //                    {
    //                        studresidentialaddress += ", " + studcountry.Trim().Trim(',');
    //                    }
    //                    else
    //                    {
    //                        studresidentialaddress = studcountry.Trim().Trim(',');
    //                    }
    //                }
    //                if (studpincode.Trim().Trim(',') != "")
    //                {
    //                    if (studresidentialaddress != "")
    //                    {
    //                        studresidentialaddress += ", Pincode : " + studpincode.Trim().Trim(',');
    //                    }
    //                    else
    //                    {
    //                        studresidentialaddress = "Pincode : " + studpincode.Trim().Trim(',');
    //                    }
    //                }

    //                #endregion Student Address For Line 2

    //                mypdfpage = mydoc.NewPage();

    //                #region OUTLINE RECTANGLE

    //                pdfMainRect = new PdfRectangle(mydoc, new PdfArea(mydoc, 15, 15, mydoc.PageWidth - 30, mydoc.PageHeight - 30), Color.Black, 1);
    //                mypdfpage.Add(pdfMainRect);

    //                #endregion OUTLINE RECTANGLE

    //                #region College Details

    //                coltop += 10;
    //                pdftxt = new PdfTextArea(fontCol_Name, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, clgname);
    //                mypdfpage.Add(pdftxt);

    //                coltop += 20;
    //                pdftxt = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, clgaff);
    //                mypdfpage.Add(pdftxt);

    //                coltop += 20;
    //                pdftxt = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, clgfulladdress);
    //                mypdfpage.Add(pdftxt);

    //                coltop += 20;
    //                pdftxt = new PdfTextArea(fontclgAddrHeader, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, clgemail);
    //                mypdfpage.Add(pdftxt);

    //                #endregion College Details

    //                #region Student Photo Recangle

    //                pdfrect = new PdfRectangle(mydoc, new PdfArea(mydoc, (mydoc.PageWidth - 130), coltop - 5, 90, 90), Color.Black);
    //                mypdfpage.Add(pdfrect);

    //                #endregion Student Photo Recangle

    //                #region College Right Logo

    //                if (colRightLogo != null)
    //                {
    //                    mypdfpage.Add(colRightLogo, 35, coltop, 330);
    //                }

    //                #endregion College Right Logo

    //                #region College Left Logo

    //                //if (colLeftLogo != null)
    //                //{
    //                //    mypdfpage.Add(colLeftLogo, (mydoc.PageWidth / 2) - 40, coltop+25, 285);
    //                //}

    //                #endregion College Left Logo

    //                #region Student Photos

    //                if (studPhoto != null)
    //                {
    //                    mypdfpage.Add(studPhoto, (mydoc.PageWidth - 125), coltop, 320);//(mydoc.PageWidth / 2) + 150
    //                }

    //                #endregion Student Photos

    //                #region Report Heading

    //                coltop += 20;
    //                pdftxt = new PdfTextArea(fontclgReportHeader, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, "Record of Academic Performance");
    //                mypdfpage.Add(pdftxt);

    //                #endregion

    //                #region Academic Year

    //                coltop += 20;
    //                string academicyear = d2.GetFunctionv("select value from master_settings where settings='Academic year'");
    //                academicyear = "(Academic Year " + academicyear.Trim().Trim(',').Replace(",", "-") + ")";

    //                pdftxt = new PdfTextArea(fontclgReportHeader, Color.Black, new PdfArea(mydoc, 5, coltop, mydoc.PageWidth, 20), ContentAlignment.MiddleCenter, academicyear);
    //                mypdfpage.Add(pdftxt);

    //                #endregion Academic Year

    //                #region Student Personal Details

    //                coltop += 50;
    //                pdfNewTable = mydoc.NewTable(fontStudentDetailsContent, 5, 4, 5);
    //                pdfNewTable.VisibleHeaders = false;
    //                pdfNewTable.SetBorders(Color.Black, 1, BorderType.None);
    //                pdfNewTable.SetColumnsWidth(new int[] { 200, 400, 200, 200 });

    //                //Row 0
    //                pdfNewTable.Cell(0, 0).SetContent("Registration No.");
    //                pdfNewTable.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                pdfNewTable.Cell(0, 1).SetContent(reg_no);
    //                pdfNewTable.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                pdfNewTable.Cell(0, 2).SetContent("Admission No.");
    //                pdfNewTable.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                pdfNewTable.Cell(0, 3).SetContent(Admit_no);
    //                pdfNewTable.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                //Row 1

    //                pdfNewTable.Cell(1, 0).SetContent("Name of Student");
    //                pdfNewTable.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                pdfNewTable.Cell(1, 1).SetContent(studname);
    //                pdfNewTable.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                pdfNewTable.Cell(1, 2).SetContent("Roll No.");
    //                pdfNewTable.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                pdfNewTable.Cell(1, 3).SetContent(roll_no);
    //                pdfNewTable.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                //Row 2

    //                pdfNewTable.Cell(2, 0).SetContent("Father's Name");
    //                pdfNewTable.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                pdfNewTable.Cell(2, 1).SetContent(father_name);
    //                pdfNewTable.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                pdfNewTable.Cell(2, 2).SetContent("Class & Section");
    //                pdfNewTable.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                pdfNewTable.Cell(2, 3).SetContent(studclassandsec);
    //                pdfNewTable.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                //Row 3

    //                pdfNewTable.Cell(3, 0).SetContent("Mother's Name");
    //                pdfNewTable.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                pdfNewTable.Cell(3, 1).SetContent(mother_name);
    //                pdfNewTable.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                pdfNewTable.Cell(3, 2).SetContent("Date of Birth");
    //                pdfNewTable.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                pdfNewTable.Cell(3, 3).SetContent(dob);
    //                pdfNewTable.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                //Row 4

    //                pdfNewTable.Cell(4, 0).SetContent("Address");
    //                pdfNewTable.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                pdfNewTable.Cell(4, 1).SetContent(studresidentialaddress.Trim(',') + studresidentialaddress1.Trim(','));
    //                pdfNewTable.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

    //                foreach (PdfCell pc in pdfNewTable.CellRange(4, 1, 4, 1).Cells)
    //                {
    //                    pc.ColSpan = 3;
    //                }

    //                pdfNewTablePage = pdfNewTable.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, coltop, mydoc.PageWidth - 50, 400));
    //                mypdfpage.Add(pdfNewTablePage);
    //                double height = pdfNewTablePage.Area.Height;
    //                coltop += int.Parse(Convert.ToString(height)) + 15;

    //                #endregion Student Personal Details

    //                pdfNewTablePage = pdfMarkTable.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 25, coltop, mydoc.PageWidth - 50, mydoc.PageHeight - coltop - 20));
    //                mypdfpage.Add(pdfNewTablePage);

    //                coltop += Convert.ToInt16(pdfNewTablePage.Area.Height) + 15;

    //                if (status)
    //                    mypdfpage.SaveToDocument();
    //            }

    //        }

    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

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

    private void GetSubTitleName(string degcode, string batchyr, string subtitle, ref string subtitlename)
    {
        try
        {
            subtitlename = string.Empty;
            if (degcode.Trim() != "" && batchyr.Trim() != "" && subtitle.Trim() != "")
            {
                qry = "select  TextVal as part1 from CoCurr_Activitie ca,textvaltable tv where ca.Title_Name=tv.TextCode and TextCriteria = 'RTnam' and ca.Degree_Code='" + degcode.Trim() + "' and Batch_Year='" + batchyr.Trim() + "' and SubTitle='" + subtitle.Trim() + "'";
                subtitlename = d2.GetFunctionv(qry);
            }
            else
            {
                subtitlename = string.Empty;
            }
        }
        catch (Exception ex)
        {
            subtitlename = string.Empty;
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