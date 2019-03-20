using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using Farpoint = FarPoint.Web.Spread;
using wc = System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;

public partial class cummulativemark_and_grade : System.Web.UI.Page
{
    string group_user = string.Empty;
    string singleuser = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string group_code = string.Empty;
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
    string qry = string.Empty;

    DataTable dtallcol = new DataTable();
    DataTable dtallotherscol = new DataTable();

    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();

    DataSet studgradeds = new DataSet();
    ArrayList gradesystemfa = new ArrayList();
    ArrayList gradesystemsa = new ArrayList();
    FarPoint.Web.Spread.ComboBoxCellType combocol = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.ComboBoxCellType combocolgrade = new FarPoint.Web.Spread.ComboBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxcol = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxsel_all = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.TextCellType txtceltype = new FarPoint.Web.Spread.TextCellType();

    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds_attnd_pts = new DataSet();
    DataSet ds5 = new DataSet();
    DataSet ds = new DataSet();

    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double per_holidate;
    int tot_per_hrs;
    double njhr, njdate, per_njdate;
    double per_per_hrs;

    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    int cal_to_date_tmp;
    string value_holi_status = string.Empty;
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = string.Empty;
    string split_holiday_status_2 = string.Empty;

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
    string halforfull = string.Empty;
    string mng = string.Empty;
    string evng = string.Empty;
    string holiday_sched_details = string.Empty;
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

    string lbltot_att1 = string.Empty;
    string lbltot_work1 = string.Empty;
    string lbltot_att2 = string.Empty;
    string lbltot_work2 = string.Empty;


    Institution institute;

    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();

    #region Attendance

    double per_leavehrs;
    DateTime per_from_gendate;
    DateTime per_to_gendate;
    string fromDate = string.Empty;
    string toDate = string.Empty;

    DateTime dtFromDate = new DateTime();
    DateTime dtToDate = new DateTime();
    bool isValidDate = false;

    TimeSpan tsFromToDiff = new TimeSpan();
    int cal_from_date_tmp;

    //TimeSpan ts;
    Boolean deptflag = false;

    string frdate, todate;

    double leavfinaeamount = 0;
    double minpresday = 0, nohrsprsentperday = 0, noofdaypresen = 0;
    double moringabsentfine = 0, eveingabsentfine = 0, studentabsentfine = 0;
    double medicalLeaveDays = 0;

    string dd = string.Empty;
    int unmark;
    int per_dum_unmark, dum_unmark;
    int medicalLeaveHours = 0;
    int medicalLeaveCountPerSession = 0;

    #endregion Attendance

    protected void Page_Load(object sender, EventArgs e)
    {
        lblerrormsg.Visible = false;

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
            divTest.Visible = false;
            bindschool();
            bindyear();
            bindschooltype();
            bindstandard();
            bindterm();
            bindsec();
            BindPreviousTestName();

            final.Visible = false;
            FpSpread1.Visible = false;
            divMainContents.Visible = false;

            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].RowCount = 1;
            FpSpread1.Sheets[0].ColumnCount = 3;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 3;
            FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 40;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";

            FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 50;
            FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 120;

            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[2].Locked = true;

            for (int i = 0; i < 3; i++)
            {
                FpSpread1.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
            }

            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = " Admn. No.";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name";

            chkboxsel_all.AutoPostBack = true;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = System.Drawing.Color.Teal;
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.White;
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
            FpSpread1.SaveChanges();
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            //---------------------------
        }
    }

    protected void Fpspread1_Command(object sender, EventArgs e)
    {
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
    }

    public void bindschool()
    {
        try
        {
            string columnfield = string.Empty;
            usercode = Session["UserCode"].ToString();
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
            studgradeds.Clear();
            studgradeds = d2.select_method("bind_college", hat, "sp");
            ddschool.Items.Clear();
            if (studgradeds.Tables.Count > 0 && studgradeds.Tables[0].Rows.Count > 0)
            {
                ddschool.DataSource = studgradeds;
                ddschool.DataTextField = "collname";
                ddschool.DataValueField = "college_code";
                ddschool.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void bindyear()
    {
        try
        {
            dropyear.Items.Clear();
            studgradeds.Clear();
            studgradeds = d2.select_method_wo_parameter("bind_batch", "sp");
            if (studgradeds.Tables.Count > 0 && studgradeds.Tables[0].Rows.Count > 0)
            {
                int count = studgradeds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    dropyear.DataSource = studgradeds;
                    dropyear.DataTextField = "batch_year";
                    dropyear.DataValueField = "batch_year";
                    dropyear.DataBind();
                }
            }
            if (studgradeds.Tables.Count > 1 && studgradeds.Tables[1].Rows.Count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(studgradeds.Tables[1].Rows[0][0].ToString());
                dropyear.SelectedValue = max_bat.ToString();
            }
            dropyear.Text = "batch (" + 1 + ")";
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void bindschooltype()
    {
        try
        {
            ddschooltype.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddschool.SelectedItem.Value;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
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
            studgradeds.Clear();
            studgradeds = d2.select_method("bind_degree", hat, "sp");
            //if (studgradeds.Tables.Count > 0 && studgradeds.Tables[0].Rows.Count > 0)
            if (studgradeds.Tables.Count > 0 && studgradeds.Tables[0].Rows.Count > 0)
            {
                ddschooltype.DataSource = studgradeds;
                ddschooltype.DataTextField = "course_name";
                ddschooltype.DataValueField = "course_id";
                ddschooltype.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void bindstandard()
    {
        try
        {
            hat.Clear();
            ddstandard.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddschooltype.SelectedValue);
            hat.Add("college_code", Convert.ToString(ddschool.SelectedValue).Trim());
            hat.Add("user_code", usercode);
            studgradeds.Clear();
            studgradeds = d2.select_method("bind_branch", hat, "sp");
            if (studgradeds.Tables.Count > 0 && studgradeds.Tables[0].Rows.Count > 0)
            {
                ddstandard.DataSource = studgradeds;
                ddstandard.DataTextField = "dept_name";
                ddstandard.DataValueField = "degree_code";
                ddstandard.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
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
            //string strstandard  =string.Empty;

            //if (ddstandard.SelectedValue != "")
            //{
            //    strstandard = ddstandard.SelectedValue;
            //}

            //if (strstandard.Trim() != "")
            //{
            //    strstandard = " and degree_code in(" + strstandard + ")";
            //}

            //strquery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + ddschool.SelectedValue.ToString() + " and batch_year=" + dropyear.Text.ToString() + " and degree_code=" + ddstandard.Text.ToString() + " order by NDurations desc";
            //studgradeds.Reset();
            //studgradeds.Dispose();
            ////  studgradeds = d2.select_method_wo_parameter(strquery, "Text");
            //studgradeds = d2.BindSem(ddstandard.Text.ToString(), dropyear.Text.ToString(), ddschool.SelectedValue.ToString());
            //if (studgradeds.Tables.Count > 0 && studgradeds.Tables[0].Rows.Count > 0)
            //{
            //    first_year = Convert.ToBoolean(studgradeds.Tables[0].Rows[0][1].ToString());
            //    duration = Convert.ToInt16(studgradeds.Tables[0].Rows[0][0].ToString());
            //    for (i = 1; i <= duration; i++)
            //    {
            //        if (first_year == false)
            //        {
            //            dropterm.Items.Add(i.ToString());
            //        }
            //        else if (first_year == true && i != 2)
            //        {
            //            dropterm.Items.Add(i.ToString());
            //        }
            //    }
            //}
            //else
            //{
            //    strquery = "select distinct duration,first_year_nonsemester  from degree where college_code=" + ddschool.SelectedValue.ToString() + " " + ddstandard.SelectedValue.ToString() + " order by duration desc";
            //    studgradeds.Reset();
            //    studgradeds.Dispose();
            //    studgradeds = d2.select_method_wo_parameter(strquery, "Text");
            //    if (studgradeds.Tables.Count > 0 && studgradeds.Tables[0].Rows.Count > 0)
            //    {
            //        first_year = Convert.ToBoolean(studgradeds.Tables[0].Rows[0][1].ToString());
            //        duration = Convert.ToInt16(studgradeds.Tables[0].Rows[0][0].ToString());

            //        for (i = 1; i <= duration; i++)
            //        {
            //            if (first_year == false)
            //            {
            //                dropterm.Items.Add(i.ToString());
            //            }
            //            else if (first_year == true && i != 2)
            //            {
            //                dropterm.Items.Add(i.ToString());
            //            }
            //        }
            //    }
            //}
            dropterm.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;

            string sqlnew = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code='" + ddstandard.SelectedValue.ToString() + "' and batch_year='" + dropyear.Text.ToString() + "' and college_code='" + ddschool.SelectedValue.ToString() + "'";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = d2.select_method_wo_parameter(sqlnew, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        dropterm.Items.Add(i.ToString());
                        //ddlSemYr.Enabled = false;
                    }
                    else if (first_year == true && i == 2)
                    {
                        dropterm.Items.Add(i.ToString());
                    }
                }
            }
            else
            {
                sqlnew = "select distinct duration,first_year_nonsemester  from degree where degree_code='" + ddstandard.SelectedValue.ToString() + "' and college_code='" + ddschool.SelectedValue.ToString() + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sqlnew, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            dropterm.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            dropterm.Items.Add(i.ToString());
                        }
                    }
                }
            }
            if (dropterm.Items.Count > 0)
            {
                dropterm.SelectedIndex = 0;
                bindsec();

            }
            BindPreviousTestName();
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
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
        }
    }

    private void BindPreviousTestName()
    {
        try
        {
            string batchYear = string.Empty;
            string collegeCode = string.Empty;
            string degreeCode = string.Empty;
            string semester = string.Empty;
            string sections = string.Empty;
            string section = string.Empty;

            string qrySection = string.Empty;
            string qryCollegeCode = string.Empty;
            string qryBatchYear = string.Empty;
            string qryDegreeCode = string.Empty;
            string qrySemester = string.Empty;
            DataTable dtCommon = new DataTable();
            dtCommon.Clear();
            ddlTest.Items.Clear();
            ddlTest.Enabled = false;
            cblTest.Items.Clear();
            chkTest.Checked = false;
            txtTest.Text = "--Select--";
            txtTest.Enabled = false;

            if (ddschool.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddschool.SelectedValue).Trim();
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and college_code in(" + collegeCode + ")";
            }
            if (dropyear.Items.Count > 0)
            {
                batchYear = Convert.ToString(dropyear.SelectedItem.Text).Trim();
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and batch_year in(" + batchYear + ")";
                }
            }
            if (ddstandard.Items.Count > 0)
            {
                degreeCode = Convert.ToString(ddstandard.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and degree_code in(" + degreeCode + ")";
                }
            }
            if (dropterm.Items.Count > 0)
            {
                semester = Convert.ToString(dropterm.SelectedValue).Trim();
                qrySemester = " and sm.semester in(" + semester + ")";
            }
            if (dropsec.Items.Count > 0 && dropsec.Enabled == true)
            {
                section = Convert.ToString(dropsec.SelectedValue).Trim();
                qrySection = string.Empty;
                if (string.IsNullOrEmpty(section) || section.ToLower().Trim() == "all" || section.Trim().ToLower() == "-1")
                    section = string.Empty;
                else
                {

                    qrySection = " and LTRIM(RTRIM(ISNULL(e.sections,''))) in('" + section + "')";
                }
            }
            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("batchYear", batchYear);
                dicQueryParameter.Add("degreeCode", degreeCode);
                dicQueryParameter.Add("semester", semester);
                dicQueryParameter.Add("section", section);
                dtCommon = storeAcc.selectDataTable("uspGetPreviousTestDetails", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                ddlTest.DataSource = dtCommon;
                ddlTest.DataTextField = "criteria";
                ddlTest.DataValueField = "Criteria_no";
                ddlTest.DataBind();
                ddlTest.SelectedIndex = 0;
                ddlTest.Enabled = true;

                cblTest.DataSource = dtCommon;
                cblTest.DataTextField = "criteria";
                cblTest.DataValueField = "Criteria_no";
                cblTest.DataBind();
                txtTest.Enabled = true;
                checkBoxListselectOrDeselect(cblTest, true);
                CallCheckboxListChange(chkTest, cblTest, txtTest, lblTest.Text, "--Select--");

            }
        }
        catch
        {
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

    protected void ddschool_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindyear();
            bindschooltype();
            bindstandard();
            bindterm();
            bindsec();
            BindPreviousTestName();
            final.Visible = false;
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            divMainContents.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void dropyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindschooltype();
            bindstandard();
            bindterm();
            bindsec();
            BindPreviousTestName();
            final.Visible = false;
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            divMainContents.Visible = false;

        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void dropschooltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindstandard();
            bindterm();
            bindsec();
            BindPreviousTestName();
            final.Visible = false;
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            divMainContents.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddstandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindterm();
            bindsec();
            BindPreviousTestName();
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            divMainContents.Visible = false;
            final.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void dropterm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsec();
            BindPreviousTestName();
            final.Visible = false;
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            divMainContents.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void dropsec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindPreviousTestName();
            final.Visible = false;
            lblerrormsg.Visible = false;
            FpSpread1.Visible = false;
            divMainContents.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlReportType_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            divTest.Visible = false;
            if (ddlReportType.SelectedIndex == 1)
            {
                divTest.Visible = true;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlTest_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            string studentApplicationNo = string.Empty;
        }
        catch (Exception ex)
        {

        }
    }

    protected void chkTest_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkTest, cblTest, txtTest, lblTest.Text, "--Select--");
            string studentApplicationNo = string.Empty;
        }
        catch (Exception ex)
        {

        }
    }

    protected void cblTest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(chkTest, cblTest, txtTest, lblTest.Text, "--Select--");
        }
        catch (Exception ex)
        {
        }
    }

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        StringBuilder selectedvalue = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append("'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                    else
                    {
                        selectedvalue.Append(",'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedvalue.ToString();
    }

    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        StringBuilder selectedText = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append("'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                    else
                    {
                        selectedText.Append(",'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedText.ToString();
    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }

    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true)
    {
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
            }
        }
        catch
        {
        }
    }

    private bool getSelectedCheckBoxListCount(CheckBoxList cbl, out int selectedCount)
    {
        selectedCount = 0;
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                if (li.Selected)
                {
                    selectedCount++;
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="c">Only Data Bound Controls eg.DropDownList,RadioButtonList,CheckBoxList </param>
    /// <param name="selectedValue"></param>
    /// <param name="selectedText"></param>
    /// <param name="type">0 - Index; 1 - Text; 2 - Value;</param>
    private void SelectDataBound(Control c, string selectedValue, string selectedText)
    {
        try
        {
            bool isDataBoundControl = false;
            if (c is DataBoundControl)
            {
                if (c is CheckBoxList || c is DropDownList || c is RadioButtonList)
                {
                    isDataBoundControl = true;
                }
                if (isDataBoundControl)
                {
                    ListControl lstControls = (ListControl)c;
                    if (lstControls.Items.Count > 0)
                    {
                        ListItem[] listItem = new ListItem[lstControls.Items.Count];
                        lstControls.Items.CopyTo(listItem, 0);
                        if (listItem.Contains(new ListItem(selectedText, selectedValue)))
                        {
                            lstControls.SelectedValue = selectedValue;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    public string loadmarkat(string mr)
    {
        string strgetval = string.Empty;
        if (mr == "-1")
        {
            strgetval = "AAA";
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

            // admdate = ds4.Tables[0].Rows[rows_count]["adm_date"].ToString();
            string[] admdatesp = admdate.Split(new Char[] { '/' });
            admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
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
                hat.Add("from_date", frdate.ToString());
                hat.Add("to_date", todate.ToString());
                hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));

                int iscount = 0;
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + degreecode + " and semester=" + currentsem + "";
                DataSet dsholiday = da.select_method_wo_parameter(sqlstr_holiday, "Text");
                if (dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                }
                hat.Add("iscount", iscount);

                ds3 = da.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");

                hat.Clear();
                hat.Add("degree_code", degreecode);
                hat.Add("sem_ester", int.Parse(currentsem));
                ds = da.select_method("period_attnd_schedule", hat, "sp");
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
                ds1 = da.select_method("ATT_MASTER_SETTING", hat, "sp");
                count = ds1.Tables[0].Rows.Count;

                DataSet dsondutyva = new DataSet();
                Dictionary<string, int> holiday_table1 = new Dictionary<string, int>();
                Dictionary<string, int> holiday_table2 = new Dictionary<string, int>();
                Dictionary<string, int> holiday_table3 = new Dictionary<string, int>();

                holiday_table11.Clear();
                holiday_table21.Clear();
                holiday_table31.Clear();
                if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count != 0)
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
                        if (!holiday_table11.Contains((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                        }
                    }
                }

                if (ds3.Tables.Count > 1 && ds3.Tables[1].Rows.Count != 0)
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
                        if (!holiday_table2.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table2.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                        }
                    }
                }

                if (ds3.Tables.Count > 2 && ds3.Tables[2].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[2].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        if (!holiday_table31.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                        {
                            holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
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
                        if (holiday_table3.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table3.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                        }
                    }
                }
            }

            //------------------------------------------------------------------
            if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count != 0)
            {
                ts = DateTime.Parse(ds3.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                diff_date = Convert.ToString(ts.Days);
                dif_date1 = double.Parse(diff_date.ToString());
            }
            next = 0;

            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count != 0)
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
                                if (cal_from_date == int.Parse(dvattvalue[0]["month_year"].ToString()))
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

                                        if (ds3.Tables.Count > 2 && ds3.Tables[2].Rows.Count != 0)
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
                                                value = dvattvalue[0][date].ToString();
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
                                                date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                                value = dvattvalue[0][date].ToString();
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
            lbltot_att2 = pre_present_date.ToString();
            lbltot_work2 = per_workingdays.ToString();
            working = per_workingdays.ToString();
            present = pre_present_date.ToString();
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
                return e.Value;
            }
        }
        return null;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        string collegeCode = string.Empty;
        string batchYear = string.Empty;
        string courseId = string.Empty;
        string degreeCode = string.Empty;
        string semester = string.Empty;
        string section = string.Empty;
        string testName = string.Empty;
        string testNo = string.Empty;
        string subjectName = string.Empty;
        string subjectNo = string.Empty;
        string subjectCode = string.Empty;
        string sections = string.Empty;

        string orderBy = string.Empty;
        string orderBySetting = string.Empty;

        string qry = string.Empty;
        string qryCollegeCode = string.Empty;
        string qryCollegeCode1 = string.Empty;
        string qryBatchYear = string.Empty;
        string qryDegreeCode = string.Empty;
        string qrySemester = string.Empty;
        string qrySection = string.Empty;
        string qryCourseId = string.Empty;
        string qrytestNo = string.Empty;
        string qrytestName = string.Empty;
        string qrySubjectNo = string.Empty;
        string qrySubjectName = string.Empty;
        string qrySubjectCode = string.Empty;
        string qryBatchYear1 = string.Empty;
        string qryDegreeCode1 = string.Empty;
        string qrySemester1 = string.Empty;
        string qrySection1 = string.Empty;
        try
        {
            divMainContents.Visible = false;
            Printcontrol.Visible = false;
            final.Visible = false;


            int selectedCount = 0;
            DataTable dtStudentMarks = new DataTable();
            DataTable dtGradeDetails = new DataTable();

            if (ddschool.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblschool.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                collegeCode = Convert.ToString(ddschool.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode1 = " and srh.collegeCode in(" + collegeCode + ")";
                    qryCollegeCode = " and r.college_code in(" + collegeCode + ")";
                }
            }
            if (dropyear.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblyear.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                batchYear = Convert.ToString(dropyear.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and r.Batch_Year in(" + batchYear + ")";
                    qryBatchYear1 = " and srh.BatchYear in(" + batchYear + ")";
                }
            }
            if (ddschooltype.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblschooltype.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                courseId = Convert.ToString(ddschooltype.SelectedValue).Trim();
            }
            if (ddstandard.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblstandard.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                degreeCode = Convert.ToString(ddstandard.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and r.degree_code in(" + degreeCode + ")";
                    qryDegreeCode1 = " and srh.degreeCode in(" + degreeCode + ")";
                }
            }
            if (dropterm.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblterm.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                semester = Convert.ToString(dropterm.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(semester))
                {
                    qrySemester = " and r.current_semester in(" + semester + ")";
                    qrySemester1 = " and srh.semester in(" + semester + ")";
                }
            }
            if (dropsec.Items.Count > 0 && dropsec.Enabled)
            {
                string secValue = Convert.ToString(dropsec.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(secValue) && secValue.Trim().ToLower() != "all" && secValue.Trim().ToLower() != "0" && secValue.Trim().ToLower() != "-1")
                {
                    section = secValue;
                    qrySection = " and LTRIM(RTRIM(ISNULL(e.sections,''))) in('" + secValue + "')";
                    qrySection1 = " and LTRIM(RTRIM(ISNULL(ss.Sections,''))) in('" + secValue + "')";
                }
            }

            #region Format 1

            if (ddlReportType.SelectedIndex == 0)
            {
                gradesystemfa.Add("FS1");
                gradesystemfa.Add("FS2");
                gradesystemfa.Add("FS3");

                gradesystemsa.Add("SA1");
                gradesystemsa.Add("SA2");
                gradesystemsa.Add("SA3");

                collcode = " and r.college_code='" + ddschool.SelectedItem.Value.ToString() + "'";
                batchyear = " and r.Batch_Year='" + dropyear.SelectedItem.Text.ToString() + "'";
                degreecode = " and r.degree_code='" + ddstandard.SelectedItem.Value.ToString() + "'";
                // term = "and sc.semester='" + dropterm.SelectedItem.Text.ToString() + "'";     
                FpSpread1.Sheets[0].ColumnCount = 3;

                if (dropsec.Enabled == true)
                {
                    if (dropsec.SelectedItem.Text.Trim().ToLower() != "all" && dropsec.SelectedItem.Text.Trim().ToLower() != "" && dropsec.SelectedItem.Text.Trim().ToLower() != "-1")
                    {
                        for (int sc = 0; sc < dropsec.Items.Count; sc++)
                        {
                            sec = "and r.Sections in ('" + dropsec.SelectedItem.Text.ToString() + "')";
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
                for (int i = 0; i < 3; i++)
                {
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, first].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, i, 3, 1);
                }
                sqlcondition = collcode + batchyear + degreecode + sec;
                string strorderby = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
                Boolean serialflag;
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
                    sql = "SELECT distinct r.Roll_No,r.Reg_No,R.Stud_Name,a.sex,r.Roll_Admit,r.Sections,serialno FROM Registration R,Applyn A WHERE R.App_No = A.App_No     " + sqlcondition + " and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' " + strorderby + "";
                }
                else
                {
                    sql = "SELECT distinct r.Roll_No,r.Reg_No,R.Stud_Name,a.sex,r.Roll_Admit,r.Sections,serialno FROM Registration R,Applyn A WHERE R.App_No = A.App_No     " + sqlcondition + " and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' ORDER BY serialno";
                }
                studgradeds.Clear();
                studgradeds = d2.select_method_wo_parameter(sql, "Text");
                if (studgradeds.Tables.Count > 0 && studgradeds.Tables[0].Rows.Count > 0)
                {
                    FpSpread1.Sheets[0].Rows.Count = studgradeds.Tables[0].Rows.Count;
                    for (int i = 0; i < studgradeds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                        FpSpread1.Sheets[0].Cells[i, 1].CellType = txtceltype;
                        FpSpread1.Sheets[0].Cells[i, 1].Text = studgradeds.Tables[0].Rows[i]["Roll_Admit"].ToString();
                        FpSpread1.Sheets[0].Cells[i, 1].Tag = studgradeds.Tables[0].Rows[i]["Roll_No"].ToString();
                        FpSpread1.Sheets[0].Cells[i, 2].Text = studgradeds.Tables[0].Rows[i]["Stud_Name"].ToString();
                        FpSpread1.Sheets[0].Cells[i, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[i, 2].HorizontalAlign = HorizontalAlign.Left;
                    }

                    string checkcamcal = d2.GetFunction("select value from Master_Settings where settings='Report Throw Cam Calculation'");
                    if (checkcamcal.Trim() == "0")
                    {
                        //-----------------------------

                        string otherssubject_sql = string.Empty;
                        DataSet ds_subject = new DataSet();
                        DataSet otherds_subject = new DataSet();
                        dtallcol.Columns.Clear();
                        dtallcol.Columns.Add("Colname");
                        dtallcol.Columns.Add("colno");
                        dtallcol.Columns.Add("Criteria nos");
                        dtallcol.Columns.Add("subjetno");

                        dtallotherscol.Columns.Add("Colname");
                        dtallotherscol.Columns.Add("colno");
                        dtallotherscol.Columns.Add("subjetno");

                        string fasaCRITERIA_NO = string.Empty;
                        double fatotal = 0;
                        //double satotal = 0;
                        //double fulltotal = 0;
                        double maxfatotal = 0;
                        double maxsatotal = 0;
                        double maxfulltotal = 0;
                        string otherconvetedvalue = string.Empty;
                        // collcode = " and r.college_code='" + ddschool.SelectedItem.Value.ToString() + "'";
                        batchyear = "  and y.Batch_Year='" + dropyear.SelectedItem.Text.ToString() + "'";
                        degreecode = "  and degree_code='" + ddstandard.SelectedItem.Value.ToString() + "'";
                        term = " and semester in ('" + dropterm.SelectedItem.Text + "')";
                        //string subject_sql = "select distinct  subject_no,subject_name from subject s,syllabus_master y where s.syll_code = y.syll_code  ";
                        //subject_sql = subject_sql + batchyear + degreecode + term + " order by subject_no,subject_name;";

                        otherssubject_sql = "select distinct  subject_no,subject_name from subject s,syllabus_master y,sub_sem ss  where s.syll_code = y.syll_code and ss.syll_code=s.syll_code and s.subType_no=ss.subType_no and y.syll_code=ss.syll_code   and subject_type='others' and promote_count=1 ";
                        otherssubject_sql = otherssubject_sql + batchyear + degreecode + term + " order by subject_no,subject_name;";

                        otherds_subject.Clear();
                        otherds_subject = d2.select_method_wo_parameter(otherssubject_sql, "Text");
                        string otherssubjectcode = string.Empty;
                        string otherssubjectcode01 = string.Empty;

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
                        ds_subject.Clear();
                        ds_subject = d2.select_method_wo_parameter(subject_sql, "Text");
                        if (ds_subject.Tables.Count > 0 && ds_subject.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds_subject.Tables[0].Rows.Count; i++)
                            {
                                DataView dvmark = new DataView();
                                string str_subject_name = ds_subject.Tables[0].Rows[i]["subject_name"].ToString();
                                string str_subject_no = ds_subject.Tables[0].Rows[i]["subject_no"].ToString();

                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = str_subject_name;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;

                                maxfatotal = 0;
                                fasaCRITERIA_NO = string.Empty;
                                maxfatotal = Convert.ToInt32(d2.GetFunction("select max_mark from criteriaforinternal where syll_code in (select syll_code from syllabus_master where degree_code='" + ddstandard.SelectedItem.Value.ToString() + "' and semester='" + dropterm.SelectedItem.Text + "'  and Batch_Year='" + dropyear.SelectedItem.Text.ToString() + "') and criteria in ('FA')"));
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "FA";

                                dtallcol.Rows.Add("FA", FpSpread1.Sheets[0].ColumnCount - 1, " ", str_subject_no);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(maxfatotal);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;

                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Grade";
                                dtallcol.Rows.Add("Grade", FpSpread1.Sheets[0].ColumnCount - 1, fasaCRITERIA_NO, str_subject_no);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);

                                FpSpread1.Sheets[0].ColumnCount++;
                                maxsatotal = Convert.ToInt32(d2.GetFunction("select max_mark from criteriaforinternal where syll_code in (select syll_code from syllabus_master where degree_code='" + ddstandard.SelectedItem.Value.ToString() + "' and semester='" + dropterm.SelectedItem.Text + "'  and Batch_Year='" + dropyear.SelectedItem.Text.ToString() + "') and criteria in ('SA')"));
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "SA";
                                dtallcol.Rows.Add("SA", FpSpread1.Sheets[0].ColumnCount - 1, fasaCRITERIA_NO, str_subject_no);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(maxsatotal);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Grade";
                                dtallcol.Rows.Add("SAGrade", FpSpread1.Sheets[0].ColumnCount - 1, fasaCRITERIA_NO, str_subject_no);
                                fasaCRITERIA_NO = string.Empty;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);

                                maxfulltotal = maxfatotal + maxsatotal;
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                                dtallcol.Rows.Add("Total", FpSpread1.Sheets[0].ColumnCount - 1, " ", str_subject_no);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(maxfulltotal);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Grade";
                                dtallcol.Rows.Add("TotalGrade", FpSpread1.Sheets[0].ColumnCount - 1, " ", str_subject_no);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 6, 1, 6);

                            }
                        }
                        if (otherds_subject.Tables.Count > 0 && otherds_subject.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < otherds_subject.Tables[0].Rows.Count; i++)
                            {
                                string str_subject_name = otherds_subject.Tables[0].Rows[i]["subject_name"].ToString();
                                string str_subject_no = otherds_subject.Tables[0].Rows[i]["subject_no"].ToString();

                                otherconvetedvalue = "100";
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = str_subject_name;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Mark " + otherconvetedvalue + "";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                dtallotherscol.Rows.Add("Mark", FpSpread1.Sheets[0].ColumnCount - 1, str_subject_no);
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Grade";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                dtallotherscol.Rows.Add("Grade", FpSpread1.Sheets[0].ColumnCount - 2, str_subject_no);
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 2, 1, 2);
                                // FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 48, 1, 2);
                                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 48, 2, 1);
                            }
                        }
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Attendance";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                        //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 6, 1, 2);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "No. of Days Present ";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "%";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Remarks";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 3, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 3, 1, 2);

                        if (dtallcol.Rows.Count > 0)
                        {
                            onlyfasa();
                        }
                    }
                    else
                    {
                        bindheader();
                    }
                    for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                    {
                        final.Visible = true;
                        FpSpread1.Sheets[0].Rows[i].BackColor = ColorTranslator.FromHtml("#E6e6e6");
                        i++;
                    }
                    FpSpread1.SaveChanges();
                    FpSpread1.Visible = true;
                    divMainContents.Visible = true;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                }
                else
                {
                    lblerrormsg.Text = "No Records Found";
                    lblerrormsg.Visible = true;
                }
                Printcontrol.Visible = false;
            }

            #endregion

            else
            {
                if (ddlTest.Items.Count > 0 && ddlTest.Visible)
                {
                    testNo = Convert.ToString(ddlTest.SelectedValue).Trim();
                    testName = Convert.ToString(ddlTest.SelectedItem.Text).Trim();
                    if (!string.IsNullOrEmpty(testNo))
                    {
                        qrytestNo = " and c.Criteria_no in(" + testNo + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select " + lblTest.Text.Trim() + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else if (cblTest.Items.Count > 0 && txtTest.Visible)
                {
                    testNo = getCblSelectedValue(cblTest);
                    testName = getCblSelectedText(cblTest);
                    if (!string.IsNullOrEmpty(testNo))
                    {
                        qrytestNo = " and c.Criteria_no in(" + testNo + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select " + lblTest.Text.Trim() + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblAlertMsg.Text = "No " + lblTest.Text.Trim() + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                DataTable dtSubjectDetails = new DataTable();
                DataTable dtAllSubjectDetails = new DataTable();
                DataTable dtGeneralGrade = new DataTable();
                string fromDate = string.Empty;
                string toDate = string.Empty;
                if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(testNo))
                {
                    qry = "SELECT Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.App_no),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.App_No),''))) end App_no,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.Roll_no),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.Roll_no),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Roll_No),''))) end Roll_No,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.collegeCode),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.collegeCode),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.college_code),''))) end college_code,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.RegNo),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.RegNo),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Reg_No),''))) end Reg_No,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.BatchYear),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.BatchYear),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Batch_Year),''))) end Batch_Year,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.degreeCode),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.degreeCode),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.degree_code),''))) end degree_code,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.semester),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.semester),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Current_Semester),''))) end semester,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.sections),'')))<>'' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.sections),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Sections),''))) end ClassSection,LTRIM(RTRIM(ISNULL(Convert(varchar(500),e.sections),''))) as ExamSection,a.app_formno as ApplicationNo,Case when LTRIM(RTRIM(ISNULL(Convert(varchar(20),srh.admissionDate,103),'')))<>'' and LTRIM(RTRIM(ISNULL(Convert(varchar(20),srh.admissionDate,103),'')))<>'01/01/1900' then LTRIM(RTRIM(ISNULL(Convert(varchar(500),srh.admissionDate,103),''))) else LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.Adm_Date,103),''))) end AdmissionDate,r.Stud_Name,r.Stud_Type,r.Roll_Admit,ISNULL(r.serialno,'0') as serialno,c.Criteria_no as TestNo,c.criteria as TestName,c.min_mark as TestMinMark,c.max_mark as TestMaxMark,s.subject_code,s.subject_name,case when a.sex='0' then 'Male' when a.sex='1' then 'Female' else 'Transgender' end as Gender,ss.subject_type,ss.subType_no,ISNULL(s.subjectpriority,'0') as subjectpriority,s.subject_no,s.min_int_marks as SubjectMinINT,s.max_int_marks as SubjectMaxINT,s.min_ext_marks as SubjectMinEXT,s.max_ext_marks as SubjectMaxEXT,s.mintotal as SubjectMinTotal,s.maxtotal as SubjectMaxTotal,e.exam_code,CAST(ISNULL(e.min_mark,'0') as float) as ConductedMinMark,CAST(ISNULL(e.max_mark,'0') as float) as ConductedMaxMark,CAST(ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'0') as float) as TestMark,ISNULL(CONVERT(VARCHAR(100),re.Retest_Marks_obtained),'0') as RetestMark,case when (ISNULL(re.marks_obtained,'0')>='0' and ISNULL(re.marks_obtained,'0')>=ISNULL(e.min_mark,'0')) then 'Pass' when ISNULL(re.marks_obtained,'0')='-1' then 'AAA' else 'Fail' end as Result,CAST(case when ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'')<>'' and ISNULL(re.marks_obtained,'0')>=0 and ISNULL(CONVERT(VARCHAR(100),e.max_mark),'')<>'' and ISNULL(e.max_mark,'0')>=0 then ROUND(ISNULL(re.marks_obtained,'0')/ ISNULL(e.max_mark,'0') * 100, 0)  else ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'0') end as float) OutOffMarks,ISNULL(ss.isSingleSubject,'0') as Single FROM CriteriaForInternal c,Exam_type e,Result re,sub_sem ss,subject s,applyn a,Registration r left join StudentRegisterHistory srh on r.App_No=srh.App_no and srh.RedoType='2' " + qryCollegeCode1 + qryBatchYear1 + qryDegreeCode1 + qrySemester1 + " where ss.syll_code=s.syll_code and ss.syll_code=c.syll_code and s.subType_no=ss.subType_no and r.App_No=a.app_no and s.subject_no=e.subject_no and s.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and r.Roll_no=re.roll_no and LTRIM(RTRIM(ISNULL(e.sections,'')))=LTRIM(RTRIM(ISNULL(r.sections,''))) " + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySection + qrytestNo + qrySubjectCode + "";
                    dtStudentMarks = dirAcc.selectDataTable(qry);

                    qry = "select distinct s.subject_code,s.subject_name,ISNULL(s.subjectpriority,'0') as subjectpriority,ss.subject_type,ss.subType_no,ISNULL(ss.isSingleSubject,'0') as isSingleSubject from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and ISNULL(ss.isSingleSubject,'0')='0' and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + " union select subject_code=STUFF((select '$mr$'+s.subject_code from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and ISNULL(ss.isSingleSubject,'0')=1 and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + " for XML PATH('')),1,4,''),ss.subject_type as subject_name,min(ISNULL(s.subjectpriority,'0')) as subjectpriority,ss.subject_type,ss.subType_no,ISNULL(ss.isSingleSubject,'0') as isSingleSubject from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and ISNULL(ss.isSingleSubject,'0')='1' and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + " group by ss.subject_type,ss.subType_no,isSingleSubject order by subjectpriority,subject_code";
                    dtSubjectDetails = dirAcc.selectDataTable(qry);

                    qry = "select distinct ss.subject_type,ss.subType_no,ISNULL(ss.isSingleSubject,'0') as isSingleSubject,s.subject_no,s.subject_code,s.subject_name,ISNULL(s.subjectpriority,'0') as subjectpriority from CriteriaForInternal c,Exam_type e,syllabus_master sm,subject s,sub_sem ss where sm.syll_code=s.syll_code and s.syll_code=c.syll_code and ss.syll_code=s.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=e.subject_no and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and c.Criteria_no in(" + testNo + ") " + qrySection + " order by subjectpriority,subject_code,ss.subType_no,isSingleSubject";
                    dtAllSubjectDetails = dirAcc.selectDataTable(qry);

                    fromDate = dirAcc.selectScalarString("select CONVERT(varchar(20),start_date,103) as start_date from seminfo where semester='" + semester + "' and degree_code='" + degreeCode + "' and batch_year='" + batchYear + "'");
                    toDate = dirAcc.selectScalarString("select CONVERT(varchar(20),end_date,103) as end_date from seminfo where semester='" + semester + "' and degree_code='" + degreeCode + "' and batch_year='" + batchYear + "'"); ;

                    qry = "select gm.College_Code,gm.batch_year,gm.Degree_Code,ISNULL(gm.Semester,'0') as Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,LTRIM(RTRIM(ISNULL(gm.Criteria,''))) as Criteria,gm.classify,CONVERT(Varchar(50),gm.Frange)+' - '+CONVERT(Varchar(50),gm.Trange) as Ranges from Grade_Master gm where gm.batch_year='" + batchYear + "' and gm.College_Code='" + collegeCode + "' and gm.Degree_Code='" + degreeCode + "' and ISNULL(gm.Semester,'0')='" + semester + "' union select gm.College_Code,gm.batch_year,gm.Degree_Code,ISNULL(gm.Semester,'0') as Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,LTRIM(RTRIM(ISNULL(gm.Criteria,''))) as Criteria,gm.classify,CONVERT(Varchar(50),gm.Frange)+' - '+CONVERT(Varchar(50),gm.Trange) as Ranges from Grade_Master gm where gm.batch_year='" + batchYear + "' and gm.College_Code='" + collegeCode + "' and gm.Degree_Code='" + degreeCode + "' and ISNULL(gm.Semester,'0')='0' order by gm.College_Code,gm.batch_year,gm.Degree_Code,gm.Semester,gm.Criteria,gm.Trange desc,gm.Frange desc";
                    dtGradeDetails = dirAcc.selectDataTable(qry);
                    if (dtGradeDetails.Rows.Count > 0)
                    {
                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' and Criteria='General'";
                        dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
                    }
                    if (dtGeneralGrade.Rows.Count == 0)
                    {
                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Criteria='General'";
                        dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
                    }
                    if (dtGeneralGrade.Rows.Count == 0)
                    {
                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' and Criteria=''";
                        dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
                    }
                    if (dtGeneralGrade.Rows.Count == 0)
                    {
                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Criteria=''";
                        dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
                    }

                }

                if (dtStudentMarks.Rows.Count > 0)
                {
                    DataTable dtStudMarks = new DataTable();
                    dtStudMarks.Columns.Add("app_no", typeof(long));
                    dtStudMarks.Columns.Add("subject_type", typeof(string));
                    dtStudMarks.Columns.Add("subject_name", typeof(string));
                    dtStudMarks.Columns.Add("subject_code", typeof(string));
                    dtStudMarks.Columns.Add("subject_no", typeof(string));
                    dtStudMarks.Columns.Add("ApplicationNo", typeof(string));
                    dtStudMarks.Columns.Add("AdmissionDate", typeof(string));
                    dtStudMarks.Columns.Add("Roll_No", typeof(string));
                    dtStudMarks.Columns.Add("Reg_No", typeof(string));
                    dtStudMarks.Columns.Add("Roll_Admit", typeof(string));
                    dtStudMarks.Columns.Add("serialno", typeof(string));
                    dtStudMarks.Columns.Add("Stud_Name", typeof(string));
                    dtStudMarks.Columns.Add("Stud_Type", typeof(string));
                    dtStudMarks.Columns.Add("ClassSection", typeof(string));
                    dtStudMarks.Columns.Add("ExamSection", typeof(string));
                    dtStudMarks.Columns.Add("Gender", typeof(string));
                    dtStudMarks.Columns.Add("Batch_Year", typeof(string));
                    dtStudMarks.Columns.Add("college_code", typeof(string));
                    dtStudMarks.Columns.Add("degree_code", typeof(string));
                    dtStudMarks.Columns.Add("semester", typeof(string));
                    dtStudMarks.Columns.Add("TestName", typeof(string));
                    dtStudMarks.Columns.Add("TestNo", typeof(string));
                    dtStudMarks.Columns.Add("TestMark", typeof(decimal));
                    dtStudMarks.Columns.Add("ConductedMaxMark", typeof(decimal));
                    dtStudMarks.Columns.Add("ConductedMinMark", typeof(decimal));
                    dtStudMarks.Columns.Add("OutOffMarks", typeof(decimal));
                    DataTable dtDistinctStudents = new DataTable();

                    dtStudentMarks.DefaultView.Sort = orderByStudents(collegeCode, includeOrderBy: 1);
                    dtDistinctStudents = dtStudentMarks.DefaultView.ToTable(true, "App_no", "Roll_No", "Reg_No", "ApplicationNo", "Stud_Type", "Roll_Admit", "serialno");

                    DataTable dtDistinctTestDetails = new DataTable();
                    dtDistinctTestDetails = dtStudentMarks.DefaultView.ToTable(true, "TestNo", "TestName");

                    int totalTestColumns = dtDistinctTestDetails.Rows.Count + 1;
                    int serialNo = 0;
                    object count = 0;
                    double subjectHeighestMarks = 0;
                    double subjectLeastMarks = 0;
                    double subjectAverage = 0;
                    double absenteesCount = 0;
                    double appearedCount = 0;
                    double subjectTotal = 0;

                    Dictionary<string, double> dicSubjectWiseLeastMark = new Dictionary<string, double>();
                    Dictionary<string, double> dicSubjectWiseHieghestMark = new Dictionary<string, double>();
                    Dictionary<string, double> dicSubjectWiseTotalMark = new Dictionary<string, double>();
                    Dictionary<string, double> dicSubjectWiseAverageMark = new Dictionary<string, double>();
                    Dictionary<string, double> dicSubjectWiseAppearedCount = new Dictionary<string, double>();
                    Dictionary<string, double> dicSubjectWiseAbsentCount = new Dictionary<string, double>();

                    int totalColumnValue = 0;
                    int rankColumnValue = 0;
                    int attendanceColumnValue = 0;
                    int remarkColumnValue = 0;
                    int spanStartColumn = 0;
                    Init_Spread(FpSpread1, 0);
                    foreach (DataRow drSubjectDetails in dtSubjectDetails.Rows)
                    {
                        string subjectCodeN = Convert.ToString(drSubjectDetails["subject_code"]).Trim();
                        string subjectNameN = Convert.ToString(drSubjectDetails["subject_name"]).Trim();
                        string subjectPriorityN = Convert.ToString(drSubjectDetails["subjectpriority"]).Trim();
                        string subjectTypeNameN = Convert.ToString(drSubjectDetails["subject_type"]).Trim();
                        string subjectTypeNoN = Convert.ToString(drSubjectDetails["subType_no"]).Trim();
                        string isSingleSubject = Convert.ToString(drSubjectDetails["isSingleSubject"]).Trim();

                        DataTable dtSingleSubjects = new DataTable();
                        if (isSingleSubject.Trim().ToLower() == "1" || isSingleSubject.Trim().ToLower() == "true")
                        {
                            dtAllSubjectDetails.DefaultView.RowFilter = "isSingleSubject=1 and subType_no='" + subjectTypeNoN + "'";
                            dtSingleSubjects = dtAllSubjectDetails.DefaultView.ToTable();
                        }
                        else
                        {
                            dtAllSubjectDetails.DefaultView.RowFilter = "isSingleSubject=0 and subject_code='" + subjectCodeN + "'";
                            dtSingleSubjects = dtAllSubjectDetails.DefaultView.ToTable();
                        }
                        bool testFlag = false;
                        foreach (DataRow drSingleSubject in dtSingleSubjects.Rows)
                        {
                            subjectCodeN = Convert.ToString(drSingleSubject["subject_code"]).Trim();
                            subjectNameN = Convert.ToString(drSingleSubject["subject_name"]).Trim();
                            subjectPriorityN = Convert.ToString(drSingleSubject["subjectpriority"]).Trim();
                            subjectTypeNameN = Convert.ToString(drSingleSubject["subject_type"]).Trim();
                            subjectTypeNoN = Convert.ToString(drSingleSubject["subType_no"]).Trim();
                            isSingleSubject = Convert.ToString(drSingleSubject["isSingleSubject"]).Trim();
                            string subjectNoN = Convert.ToString(drSingleSubject["subject_no"]).Trim();
                            totalTestColumns = dtDistinctTestDetails.Rows.Count + 1;
                            FpSpread1.Sheets[0].ColumnCount += totalTestColumns;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Text = subjectNameN;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Tag = subjectNoN;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Note = isSingleSubject;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - totalTestColumns, 1, totalTestColumns);


                            bool visibleflag = true;
                            foreach (DataRow drTest in dtDistinctTestDetails.Rows)
                            {
                                string testNameN = Convert.ToString(drTest["TestName"]).Trim();
                                string testNoN = Convert.ToString(drTest["TestNo"]).Trim();

                                dtStudentMarks.DefaultView.RowFilter = "subject_no= '" + subjectNoN + "'  and TestNo= '" + testNoN + "'"; //and TestName='" + testNameN + "'
                                DataTable dtSubjectAvailable = dtStudentMarks.DefaultView.ToTable();

                                if (dtSubjectAvailable.Rows.Count > 0)
                                {
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Width = 80;
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Locked = true;
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Resizable = false;
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Visible = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Text = subjectNameN;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Tag = subjectNoN;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Note = isSingleSubject;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Text = testNameN;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Tag = testNoN;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Note = subjectTypeNoN;
                                }
                                else
                                {
                                    visibleflag = false;
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Visible = false;
                                }

                                totalTestColumns--;
                            }

                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Text = "Total";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Tag = "0";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Note = isSingleSubject;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - totalTestColumns, 1, 1);

                            if (visibleflag == false)
                            {
                                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Visible = false;
                            }
                        }

                        if (isSingleSubject.Trim().ToLower() == "1" || isSingleSubject.Trim().ToLower() == "true")
                        {
                            totalTestColumns = dtDistinctTestDetails.Rows.Count + 1;
                            subjectNameN = Convert.ToString(drSubjectDetails["subject_name"]).Trim();
                            subjectCodeN = Convert.ToString(drSubjectDetails["subject_code"]).Trim();
                            subjectNameN = Convert.ToString(drSubjectDetails["subject_name"]).Trim();
                            subjectPriorityN = Convert.ToString(drSubjectDetails["subjectpriority"]).Trim();
                            subjectTypeNameN = Convert.ToString(drSubjectDetails["subject_type"]).Trim();
                            subjectTypeNoN = Convert.ToString(drSubjectDetails["subType_no"]).Trim();
                            isSingleSubject = Convert.ToString(drSubjectDetails["isSingleSubject"]).Trim();

                            FpSpread1.Sheets[0].ColumnCount += totalTestColumns;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Text = subjectNameN;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Tag = "0";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Note = isSingleSubject;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - totalTestColumns, 1, totalTestColumns);

                            foreach (DataRow drTest in dtDistinctTestDetails.Rows)
                            {
                                string testNameN = Convert.ToString(drTest["TestName"]).Trim();
                                string testNoN = Convert.ToString(drTest["TestNo"]).Trim();
                                dtStudentMarks.DefaultView.RowFilter = "subType_no= '" + subjectTypeNoN + "'  and TestNo= '" + testNoN + "'"; //and TestName='" + testNameN + "'
                                DataTable dtSubjectAvailable = dtStudentMarks.DefaultView.ToTable();
                                if (dtSubjectAvailable.Rows.Count > 0)
                                {
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Width = 80;
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Locked = true;
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Resizable = false;
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Visible = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Text = subjectNameN;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Tag = "0";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Note = isSingleSubject;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Text = testNameN;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Tag = testNoN;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Note = subjectTypeNoN;


                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Visible = false;
                                }
                                totalTestColumns--;
                            }
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Text = "Total";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Tag = "0";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - totalTestColumns].Note = isSingleSubject;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - totalTestColumns, 2, 1);
                        }
                    }
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "ATT %";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                    Dictionary<string, int> dicGradeWiseCount = new Dictionary<string, int>();

                    Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
                    int testCount = 0;
                    int endColumn = FpSpread1.Sheets[0].ColumnCount - 1;
                    int startingRows = 0;
                    Dictionary<string, double> dicStudentTotal = new Dictionary<string, double>();
                    Dictionary<string, double> dicStudentPassedTotal = new Dictionary<string, double>();
                    Dictionary<string, double> dicStudentTotalOutof100 = new Dictionary<string, double>();
                    Dictionary<string, double> dicStudentPassedTotalOutof100 = new Dictionary<string, double>();

                    Dictionary<string, int> dicStudentPassedSubjectCount = new Dictionary<string, int>();

                    Dictionary<string, double> dicStudentPassedAverage = new Dictionary<string, double>();
                    Dictionary<string, double> dicStudentPassedAverageOutof100 = new Dictionary<string, double>();
                    if (dtDistinctStudents.Rows.Count > 0)
                    {
                        totalTestColumns = dtDistinctTestDetails.Rows.Count + 1;
                        foreach (DataRow drStudent in dtDistinctStudents.Rows)
                        {
                            string subjectCodeVal = string.Empty;
                            string subjectNameVal = string.Empty;
                            string subjectNoVal = string.Empty;
                            string testMark = string.Empty;
                            string testMaxMark = string.Empty;
                            string testMinMark = string.Empty;
                            double testSubMarks = 0;
                            double testMaxMarks = 0;
                            double testMinMarks = 0;
                            int subjectCount = 0;

                            int columnVal = 0;

                            string studentAppNos = Convert.ToString(drStudent["App_no"]).Trim();

                            string dt = string.Empty;
                            string[] dsplit = new string[20];
                            int demfcal = 0;
                            string monthcal = string.Empty;
                            int demtcal = 0;
                            string dum_tage_date = string.Empty;
                            string dum_tage_hrs = string.Empty;

                            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                            {
                                frdate = fromDate;
                                todate = toDate;
                                dt = frdate;
                                dsplit = dt.Split(new Char[] { '/' });
                                frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                                demfcal = int.Parse(dsplit[2].ToString());
                                demfcal = demfcal * 12;
                                cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                                cal_from_date_tmp = demfcal + int.Parse(dsplit[1].ToString());

                                monthcal = cal_from_date.ToString();
                                dt = todate;
                                dsplit = dt.Split(new Char[] { '/' });
                                todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                                demtcal = int.Parse(dsplit[2].ToString());
                                demtcal = demtcal * 12;
                                cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
                                cal_to_date_tmp = demtcal + int.Parse(dsplit[1].ToString());

                                per_from_gendate = Convert.ToDateTime(frdate);
                                per_to_gendate = Convert.ToDateTime(todate);

                                per_abshrs_spl = 0;
                                tot_per_hrs_spl = 0;
                                tot_ondu_spl = 0;
                                tot_ml_spl = 0;
                                tot_conduct_hr_spl = 0;
                                per_workingdays1 = 0;
                                leavfinaeamount = 0;
                                medicalLeaveDays = 0;
                                medicalLeaveHours = 0;

                                DataTable dtStudent = new DataTable();
                                dtStudentMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "'";
                                dtStudent = dtStudentMarks.DefaultView.ToTable(true, "app_no", "ApplicationNo", "AdmissionDate", "Roll_No", "Reg_No", "Batch_Year", "college_code", "degree_code", "semester");
                                if (dtStudent.Rows.Count > 0)
                                {
                                    string appNo = Convert.ToString(dtStudent.Rows[0]["app_no"]).Trim();
                                    string applicationNo = Convert.ToString(dtStudent.Rows[0]["ApplicationNo"]).Trim();
                                    string admissionDate = Convert.ToString(dtStudent.Rows[0]["AdmissionDate"]).Trim();
                                    string rollNo = Convert.ToString(dtStudent.Rows[0]["Roll_No"]).Trim();
                                    string regNo = Convert.ToString(dtStudent.Rows[0]["Reg_No"]).Trim();
                                    string batch = Convert.ToString(dtStudent.Rows[0]["Batch_Year"]).Trim();
                                    string college = Convert.ToString(dtStudent.Rows[0]["college_code"]).Trim();
                                    string degree = Convert.ToString(dtStudent.Rows[0]["degree_code"]).Trim();
                                    string sems = Convert.ToString(dtStudent.Rows[0]["semester"]).Trim();

                                    persentmonthcal(college, degree, sems, rollNo, admissionDate);

                                    double absenthours = per_workingdays1 - per_per_hrs;
                                    double per_tage_date = 0;// ((pre_present_date / per_workingdays) * 100);

                                    if (per_workingdays > 0)
                                    {
                                        per_tage_date = ((pre_present_date / per_workingdays) * 100);
                                    }
                                    if (per_tage_date > 100)
                                    {
                                        per_tage_date = 100;
                                    }

                                    double per_tage_hrs = 0;// (((per_per_hrs) / (per_workingdays1)) * 100);

                                    if (per_workingdays1 > 0)
                                    {
                                        per_tage_hrs = (((per_per_hrs) / (per_workingdays1)) * 100);
                                    }

                                    if (per_tage_hrs > 100)
                                    {
                                        per_tage_hrs = 100;
                                    }

                                    dum_tage_date = string.Format("{0:0,0.00}", float.Parse(per_tage_date.ToString()));

                                    per_tage_hrs = Math.Round(per_tage_hrs, 2);
                                    dum_tage_hrs = per_tage_hrs.ToString();
                                    dum_tage_hrs = string.Format("{0:0,0.00}", float.Parse(per_tage_hrs.ToString()));
                                    if (dum_tage_hrs == "NaN")
                                    {
                                        dum_tage_hrs = "0.00";
                                    }
                                    else if (dum_tage_hrs == "Infinity")
                                    {
                                        dum_tage_hrs = "0.00";
                                    }

                                    if (dum_tage_date == "NaN")
                                    {
                                        dum_tage_date = "0.00";
                                    }
                                    else if (dum_tage_date == "Infinity")
                                    {
                                        dum_tage_date = "0.00";
                                    }

                                }
                            }
                            int columnCount = 0;
                            double SubjectTotal = 0;
                            int subjectRow = 0;
                            bool result = false;
                            bool absent = false;
                            int col = 7;
                            int colNEW = 0;
                            bool cntnflag = false;

                            for (col = 7; col < FpSpread1.Sheets[0].ColumnCount - 1; col++)
                            {
                                bool ShouldnotProceed = false;
                                int totalTestColumnsNEW = 0;
                                string testCodeValNEW = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag).Trim();
                                string subjectNoValNEW = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Tag).Trim();
                                string subjectNameValNEW = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Text).Trim();
                                string isSingleNEW = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Note).Trim();
                                string subjTypeNoNEW = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Note).Trim();

                                colNEW = col;

                                if (String.IsNullOrEmpty(testCodeValNEW) && String.IsNullOrEmpty(subjectNameValNEW) && String.IsNullOrEmpty(isSingleNEW) && String.IsNullOrEmpty(subjectNoValNEW) && String.IsNullOrEmpty(subjTypeNoNEW))
                                {
                                    colNEW = col - 1;
                                    ShouldnotProceed = true;
                                }
                                else if (testCodeValNEW == "0" || String.IsNullOrEmpty(subjectNoValNEW))
                                {
                                    totalTestColumnsNEW++;
                                }

                                if (!ShouldnotProceed)
                                {
                                    columnCount++;
                                    string displayMark = string.Empty;
                                    string displayGrade = string.Empty;

                                    if (columnCount == totalTestColumns || totalTestColumnsNEW > 0)
                                    {
                                        FpSpread1.Sheets[0].Cells[subjectRow, colNEW].CellType = txtCell;

                                        if (SubjectTotal > 0 && !absent)
                                            FpSpread1.Sheets[0].Cells[subjectRow, colNEW].Text = Convert.ToString(SubjectTotal).Trim();
                                        else
                                            FpSpread1.Sheets[0].Cells[subjectRow, colNEW].Text = (absent && SubjectTotal > 0) ? Convert.ToString(SubjectTotal).Trim() : (absent) ? "AAA" : "--";
                                        FpSpread1.Sheets[0].Cells[subjectRow, colNEW].ForeColor = (result) ? Color.Black : Color.Red;
                                        FpSpread1.Sheets[0].Cells[subjectRow, colNEW].Font.Name = "Book Antiqua";
                                        //endColumn = FpSpread1.Sheets[0].Columns[col].Visible ? col : endColumn;
                                        //if (FpSpread1.Sheets[0].Columns[col].Visible)
                                        //    subjectVisibleCount = 1;
                                        FpSpread1.Sheets[0].Cells[subjectRow, colNEW].Locked = true;
                                        FpSpread1.Sheets[0].Cells[subjectRow, colNEW].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[subjectRow, colNEW].VerticalAlign = VerticalAlign.Middle;
                                        columnCount = 0;
                                        SubjectTotal = 0;
                                        absent = false;
                                        subjectCount++;
                                        ShouldnotProceed = true;
                                        cntnflag = true;
                                        continue;
                                    }
                                    result = false;
                                    string testCodeVal = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag).Trim();
                                    subjectNoVal = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Tag).Trim();
                                    subjectNameVal = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Text).Trim();
                                    string isSingle = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Note).Trim();
                                    string subjTypeNo = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Note).Trim();

                                    DataView dvTestMark = new DataView();
                                    if (isSingle.Trim().ToLower() == "0" || isSingle.Trim().ToLower() == "false")
                                    {
                                        if (!String.IsNullOrEmpty(subjectNoVal) && !String.IsNullOrEmpty(testCodeVal))
                                        {
                                            dtStudentMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "' and subject_no='" + subjectNoVal + "' and testNo='" + testCodeVal + "'";
                                            dvTestMark = dtStudentMarks.DefaultView;
                                        }
                                    }
                                    else
                                    {
                                        //modified by prabha 6/12/2017 by Prabha
                                        if (!String.IsNullOrEmpty(subjectNoVal) && !String.IsNullOrEmpty(testCodeVal))
                                        {
                                            dtStudentMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "' and subject_no='" + subjectNoVal + "' and testNo='" + testCodeVal + "'";
                                            dvTestMark = dtStudentMarks.DefaultView;

                                            if (subjectNoVal == "0")
                                            {
                                                dtStudentMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "' and subType_no='" + subjTypeNo + "' and testNo='" + testCodeVal + "'";
                                                dvTestMark = dtStudentMarks.DefaultView;
                                                double absentCount = 0;
                                                object subjectTotalVal = dvTestMark.ToTable().Compute("SUM(TestMark)", "TestMark>=0 ");
                                                //count = dtStudentMarks.Compute("MAX(TestMark)", "TestMark>=0 ");
                                                double subTypeTotal = 0;
                                                double.TryParse(Convert.ToString(subjectTotalVal).Trim(), out subTypeTotal);

                                                subjectTotalVal = dvTestMark.ToTable().Compute("Count(TestMark)", "TestMark<0 ");
                                                double.TryParse(Convert.ToString(subjectTotalVal).Trim(), out absentCount);

                                                string displaySubTypeTotal = string.Empty;
                                                if (dvTestMark.Count > 0)
                                                {
                                                    if (subTypeTotal >= 0)
                                                    {
                                                        if (absentCount > 0 && subTypeTotal == 0)
                                                        {
                                                            displaySubTypeTotal = getMarkText("-1");
                                                        }
                                                        else
                                                        {
                                                            if (subTypeTotal > 0 && dvTestMark.Count > 0)
                                                                subTypeTotal = subTypeTotal / dvTestMark.Count;
                                                            subTypeTotal = Math.Round(subTypeTotal, 0, MidpointRounding.AwayFromZero);
                                                            displaySubTypeTotal = subTypeTotal.ToString();
                                                            SubjectTotal += subTypeTotal;
                                                            result = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        displaySubTypeTotal = getMarkText(testMark);
                                                    }
                                                    if (absentCount > 0)
                                                    {
                                                        absent = true;
                                                    }
                                                }
                                                else
                                                {
                                                    displaySubTypeTotal = "--";
                                                    result = true;
                                                }
                                                FpSpread1.Sheets[0].Cells[subjectRow, col].CellType = txtCell;
                                                FpSpread1.Sheets[0].Cells[subjectRow, col].Text = Convert.ToString(displaySubTypeTotal).Trim();
                                                FpSpread1.Sheets[0].Cells[subjectRow, col].Tag = Convert.ToString(subjectCodeVal).Trim();
                                                FpSpread1.Sheets[0].Cells[subjectRow, col].Note = Convert.ToString(subjectNoVal).Trim();
                                                FpSpread1.Sheets[0].Cells[subjectRow, col].ForeColor = (result) ? Color.Black : Color.Red;
                                                FpSpread1.Sheets[0].Cells[subjectRow, col].Font.Name = "Book Antiqua";
                                                //endColumn = FpSpread1.Sheets[0].Columns[col].Visible ? col : endColumn;
                                                //if (FpSpread1.Sheets[0].Columns[col].Visible)
                                                //    subjectVisibleCount = 1;
                                                FpSpread1.Sheets[0].Cells[subjectRow, col].Locked = true;
                                                FpSpread1.Sheets[0].Cells[subjectRow, col].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[subjectRow, col].VerticalAlign = VerticalAlign.Middle;
                                                //columnCount = 0;
                                                //SubjectTotal = 0;
                                                //absent = false;
                                                //subjectCount++;
                                                continue;
                                            }
                                        }
                                    }
                                    //else
                                    //    if (dtStudMarks.Rows.Count > 0)
                                    //    {
                                    //        dtStudMarks.DefaultView.RowFilter = "App_no='" + studentAppNos + "' and subject_no='" + subjectNoVal + "'  and testNo='" + testCodeVal + "'";
                                    //        dvTestMark = dtStudMarks.DefaultView;
                                    //    }


                                    int subjectVisibleCount = 0;
                                    int spanCount = 0;

                                    subjectRow = 0;
                                    if (FpSpread1.Sheets[0].Columns[0].Visible)
                                        spanCount = 1;
                                    if (FpSpread1.Sheets[0].Columns[1].Visible)
                                        spanCount = 2;
                                    if (FpSpread1.Sheets[0].Columns[2].Visible)
                                        spanCount = 3;
                                    if (FpSpread1.Sheets[0].Columns[3].Visible)
                                        spanCount = 4;
                                    if (FpSpread1.Sheets[0].Columns[4].Visible)
                                        spanCount = 5;
                                    if (FpSpread1.Sheets[0].Columns[5].Visible)
                                        spanCount = 6;
                                    if (FpSpread1.Sheets[0].Columns[6].Visible)
                                        spanCount = 7;
                                    if (dvTestMark.Count > 0)
                                    {
                                        if (subjectCount == 0 && columnCount == 1)
                                        {
                                            FpSpread1.Sheets[0].RowCount++;
                                            serialNo++;
                                            subjectRow = FpSpread1.Sheets[0].RowCount - 1;
                                            startingRows = subjectRow;
                                        }
                                        else
                                        {
                                            subjectRow = startingRows;
                                        }

                                        testMark = Convert.ToString(dvTestMark[0]["TestMark"]).Trim();
                                        testMaxMark = Convert.ToString(dvTestMark[0]["ConductedMaxMark"]).Trim();
                                        testMinMark = Convert.ToString(dvTestMark[0]["ConductedMinMark"]).Trim();

                                        //double.TryParse(testMaxMark, out maximumTestMarks);
                                        //maximumTestMarks = (chkRoundOffMarks.Checked) ? Math.Round(maximumTestMarks, 0, MidpointRounding.AwayFromZero) : maximumTestMarks;

                                        subjectNameVal = Convert.ToString(dvTestMark[0]["subject_name"]).Trim();
                                        subjectCodeVal = Convert.ToString(dvTestMark[0]["subject_code"]).Trim();
                                        subjectNoVal = Convert.ToString(dvTestMark[0]["subject_no"]).Trim();

                                        string appNo = Convert.ToString(dvTestMark[0]["app_no"]).Trim();
                                        string applicationNo = Convert.ToString(dvTestMark[0]["ApplicationNo"]).Trim();
                                        string admissionDate = Convert.ToString(dvTestMark[0]["AdmissionDate"]).Trim();
                                        string rollNo = Convert.ToString(dvTestMark[0]["Roll_No"]).Trim();
                                        string regNo = Convert.ToString(dvTestMark[0]["Reg_No"]).Trim();
                                        string admissionNo = Convert.ToString(dvTestMark[0]["Roll_Admit"]).Trim();
                                        string serialNos = Convert.ToString(dvTestMark[0]["serialno"]).Trim();
                                        string studentName = Convert.ToString(dvTestMark[0]["Stud_Name"]).Trim();
                                        string studentType = Convert.ToString(dvTestMark[0]["Stud_Type"]).Trim();
                                        string classSection = Convert.ToString(dvTestMark[0]["ClassSection"]).Trim();
                                        string examSection = Convert.ToString(dvTestMark[0]["ExamSection"]).Trim();
                                        string gender = Convert.ToString(dvTestMark[0]["Gender"]).Trim();

                                        string batch = Convert.ToString(dvTestMark[0]["Batch_Year"]).Trim();
                                        string college = Convert.ToString(dvTestMark[0]["college_code"]).Trim();
                                        string degree = Convert.ToString(dvTestMark[0]["degree_code"]).Trim();
                                        string sems = Convert.ToString(dvTestMark[0]["semester"]).Trim();
                                        string testNames = Convert.ToString(dvTestMark[0]["TestName"]).Trim();
                                        string testNos = Convert.ToString(dvTestMark[0]["TestNo"]).Trim();

                                        bool isSuccess = false;
                                        string convertMarkNew = Convert.ToString(dvTestMark[0]["OutOffMarks"]).Trim();
                                        isSuccess = double.TryParse(testMark, out testSubMarks);
                                        //testSubMarks = Math.Round(testSubMarks, (chkRoundOffMarks.Checked) ? 0 : 1, MidpointRounding.AwayFromZero);
                                        testMark = testSubMarks.ToString();
                                        //testMark = (isSuccess && chkRoundOffMarks.Checked) ? testSubMarks.ToString() : testMark;
                                        double.TryParse(testMaxMark, out testMaxMarks);
                                        double.TryParse(testMinMark, out testMinMarks);

                                        double outof100 = 0;

                                        double convertedMinMark = 0;
                                        double convertedMaxMark = 0;
                                        string convertedObtainedMark = testMark;
                                        string convertedMinimumMark = testMinMark;
                                        string convertedMaximumMark = testMaxMark;
                                        ConvertedMark("100", ref convertedMaximumMark, ref convertedObtainedMark, ref convertedMinimumMark);
                                        double.TryParse(convertedMinimumMark, out convertedMinMark);
                                        double.TryParse(convertedMaximumMark, out convertedMaxMark);
                                        double outOff = 0;
                                        isSuccess = double.TryParse(convertedObtainedMark, out outOff);
                                        //outOff = Math.Round(outOff, 1, MidpointRounding.AwayFromZero);
                                        outOff = Math.Round(outOff, 0, MidpointRounding.AwayFromZero);
                                        convertedObtainedMark = outOff.ToString();
                                        convertedObtainedMark = convertedObtainedMark;

                                        if (testSubMarks != 0 && testMaxMarks > 0)
                                            outof100 = Math.Round((testSubMarks / testMaxMarks) * 100, 0, MidpointRounding.AwayFromZero);
                                        DataView dvGrade = new DataView();
                                        //if (dtGradeDetails.Rows.Count > 0)
                                        //{
                                        //    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='" + sems + "' and Criteria='" + testNames.Trim() + "' and Frange<='" + testSubMarks + "' and Trange>='" + testSubMarks + "'";
                                        //    dvGrade = dtGradeDetails.DefaultView;
                                        //    if (dvGrade.Count == 0)
                                        //    {
                                        //        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='0' and Criteria='" + testNames.Trim() + "' and Frange<='" + testSubMarks + "' and Trange>='" + testSubMarks + "'";
                                        //        dvGrade = dtGradeDetails.DefaultView;
                                        //    }
                                        //    if (dvGrade.Count == 0)
                                        //    {
                                        //        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='" + sems + "' and Frange<='" + outof100 + "' and Trange>='" + outof100 + "'";
                                        //        dvGrade = dtGradeDetails.DefaultView;
                                        //    }
                                        //    if (dvGrade.Count == 0)
                                        //    {
                                        //        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='0' and Frange<='" + outof100 + "' and Trange>='" + outof100 + "'";
                                        //        dvGrade = dtGradeDetails.DefaultView;
                                        //    }
                                        //}
                                        if (testSubMarks < 0)
                                        {
                                            displayMark = getMarkText(testMark);
                                            convertedObtainedMark = displayMark;
                                            absent = true;
                                        }
                                        else if (string.IsNullOrEmpty(testMark))
                                        {
                                            displayMark = "--";
                                            convertedObtainedMark = "--";
                                            result = true;
                                        }
                                        else
                                        {
                                            if (testSubMarks >= testMinMarks)
                                                result = true;
                                            SubjectTotal += testSubMarks;
                                            displayMark = testSubMarks.ToString();
                                        }
                                        if (dvGrade.Count > 0)
                                        {
                                            displayGrade = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                                            //result = true;
                                            if (!string.IsNullOrEmpty(displayGrade))
                                            {
                                                if (!dicGradeWiseCount.ContainsKey(subjectNoVal.Trim() + "@" + displayGrade.Trim().ToLower()))
                                                {
                                                    dicGradeWiseCount.Add(subjectNoVal.Trim() + "@" + displayGrade.Trim().ToLower(), 1);
                                                }
                                                else
                                                {
                                                    dicGradeWiseCount[subjectNoVal.Trim() + "@" + displayGrade.Trim().ToLower()] += 1;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            displayGrade = "--";
                                        }

                                        if (!dicStudentTotal.ContainsKey(studentAppNos.Trim()))
                                            dicStudentTotal.Add(studentAppNos.Trim(), (testSubMarks < 0) ? 0 : testSubMarks);
                                        else
                                            dicStudentTotal[studentAppNos.Trim()] += ((testSubMarks < 0) ? 0 : testSubMarks);

                                        if (!dicStudentTotalOutof100.ContainsKey(studentAppNos.Trim()))
                                            dicStudentTotalOutof100.Add(studentAppNos.Trim(), (outOff < 0) ? 0 : outOff);
                                        else
                                            dicStudentTotalOutof100[studentAppNos.Trim()] += ((outOff < 0) ? 0 : outOff);

                                        if (!dicStudentPassedTotal.ContainsKey(studentAppNos.Trim()))
                                            dicStudentPassedTotal.Add(studentAppNos.Trim(), (testSubMarks < 0) ? 0 : testSubMarks);
                                        else
                                            dicStudentPassedTotal[studentAppNos.Trim()] += ((testSubMarks < 0) ? 0 : testSubMarks);

                                        if (!dicStudentPassedTotalOutof100.ContainsKey(studentAppNos.Trim()))
                                            dicStudentPassedTotalOutof100.Add(studentAppNos.Trim(), ((outOff < 0) ? 0 : outOff));
                                        else
                                            dicStudentPassedTotalOutof100[studentAppNos.Trim()] += ((outOff < 0) ? 0 : outOff);


                                        if (result && !string.IsNullOrEmpty(testMark))
                                        {
                                            if (!dicStudentPassedSubjectCount.ContainsKey(studentAppNos.Trim()))
                                                dicStudentPassedSubjectCount.Add(studentAppNos.Trim(), 1);
                                            else
                                                dicStudentPassedSubjectCount[studentAppNos.Trim()] += 1;
                                        }


                                        int markCol = 0;

                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(serialNo).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                        if (FpSpread1.Sheets[0].Columns[markCol].Visible)
                                            spanCount = markCol + 1;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                                        markCol++;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(rollNo).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                        if (FpSpread1.Sheets[0].Columns[markCol].Visible)
                                            spanCount = markCol + 1;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                                        markCol++;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(regNo).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                        if (FpSpread1.Sheets[0].Columns[markCol].Visible)
                                            spanCount = markCol + 1;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                                        markCol++;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(admissionNo).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                        if (FpSpread1.Sheets[0].Columns[markCol].Visible)
                                            spanCount = markCol + 1;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                                        markCol++;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(studentType).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                        if (FpSpread1.Sheets[0].Columns[markCol].Visible)
                                            spanCount = markCol + 1;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                                        markCol++;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(gender).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                        if (FpSpread1.Sheets[0].Columns[markCol].Visible)
                                            spanCount = markCol + 1;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                                        markCol++;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].CellType = txtCell;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Text = Convert.ToString(studentName).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Tag = Convert.ToString(subjectCodeVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Note = Convert.ToString(subjectNoVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].Locked = true;
                                        //if (FpSpread1.Sheets[0].Columns[markCol].Visible)
                                        //    spanCount = markCol + 1;
                                        //endColumn = FpSpread1.Sheets[0].Columns[markCol].Visible ? markCol : endColumn;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[subjectRow, markCol].VerticalAlign = VerticalAlign.Middle;

                                        markCol++;
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].CellType = txtCell;

                                        FpSpread1.Sheets[0].Cells[subjectRow, col].Text = Convert.ToString(displayMark).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].Tag = Convert.ToString(subjectCodeVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].Note = Convert.ToString(subjectNoVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].ForeColor = (result) ? Color.Black : Color.Red;
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].Font.Name = "Book Antiqua";
                                        //endColumn = FpSpread1.Sheets[0].Columns[col].Visible ? col : endColumn;
                                        if (FpSpread1.Sheets[0].Columns[col].Visible)
                                            subjectVisibleCount = 1;
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].Locked = true;
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].VerticalAlign = VerticalAlign.Middle;
                                        //markCol++;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].CellType = txtCell;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].Text = Convert.ToString(displayGrade).Trim();
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].Tag = Convert.ToString(subjectCodeVal).Trim();
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].Note = Convert.ToString(subjectNoVal).Trim();
                                        //if (FpSpread1.Sheets[0].Columns[col + 1].Visible)
                                        //    subjectVisibleCount = 2;
                                        ////endColumn = FpSpread1.Sheets[0].Columns[col + 1].Visible ? col + 2 : endColumn;
                                        ////FpSpread1.Sheets[0].Cells[subjectRow, col + 1].ForeColor = (result) ? Color.Black : Color.Red;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].Font.Name = "Book Antiqua";
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].Locked = true;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].HorizontalAlign = HorizontalAlign.Center;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].VerticalAlign = VerticalAlign.Middle;

                                        //markCol++;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].CellType = txtCell;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].Text = Convert.ToString(convertedObtainedMark).Trim();
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].Tag = Convert.ToString(subjectCodeVal).Trim();
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].Note = Convert.ToString(subjectNoVal).Trim();
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].ForeColor = (result) ? Color.Black : Color.Red;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].Font.Name = "Book Antiqua";
                                        ////endColumn = FpSpread1.Sheets[0].Columns[col + 2].Visible ? col + 1 : endColumn;
                                        //if (FpSpread1.Sheets[0].Columns[col + 2].Visible)
                                        //    subjectVisibleCount = 3;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].Locked = true;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].HorizontalAlign = HorizontalAlign.Center;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].VerticalAlign = VerticalAlign.Middle;


                                        //subjectRow++;
                                    }
                                    else
                                    {
                                        if (subjectCount == 0 && columnCount == 1)
                                        {
                                            FpSpread1.Sheets[0].RowCount++;
                                            serialNo++;
                                            subjectRow = FpSpread1.Sheets[0].RowCount - 1;
                                            startingRows = subjectRow;
                                        }
                                        else
                                        {
                                            subjectRow = startingRows;
                                        }
                                        displayMark = "--";
                                        displayGrade = "--";
                                        result = true;
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].CellType = txtCell;
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].Text = Convert.ToString(displayMark).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].Tag = Convert.ToString(subjectCodeVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].Note = Convert.ToString(subjectNoVal).Trim();
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].ForeColor = (result) ? Color.Black : Color.Red;
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].Font.Name = "Book Antiqua";
                                        if (FpSpread1.Sheets[0].Columns[col].Visible)
                                            subjectVisibleCount = 1;
                                        //endColumn = FpSpread1.Sheets[0].Columns[col].Visible ? col : endColumn;
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].Locked = true;
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[subjectRow, col].VerticalAlign = VerticalAlign.Middle;

                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].CellType = txtCell;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].Text = Convert.ToString(displayMark).Trim();
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].Tag = Convert.ToString(subjectCodeVal).Trim();
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].Note = Convert.ToString(subjectNoVal).Trim();
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].ForeColor = (result) ? Color.Black : Color.Red;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].Font.Name = "Book Antiqua";
                                        ////endColumn = FpSpread1.Sheets[0].Columns[col + 1].Visible ? col + 1 : endColumn;
                                        //if (FpSpread1.Sheets[0].Columns[col + 1].Visible)
                                        //    subjectVisibleCount = 2;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].Locked = true;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].HorizontalAlign = HorizontalAlign.Center;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 1].VerticalAlign = VerticalAlign.Middle;

                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].CellType = txtCell;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].Text = Convert.ToString(displayGrade).Trim();
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].Tag = Convert.ToString(subjectCodeVal).Trim();
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].Note = Convert.ToString(subjectNoVal).Trim();
                                        ////endColumn = FpSpread1.Sheets[0].Columns[col + 2].Visible ? col + 2 : endColumn;
                                        //if (FpSpread1.Sheets[0].Columns[col + 2].Visible)
                                        //    subjectVisibleCount = 3;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].Font.Name = "Book Antiqua";
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].Locked = true;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].HorizontalAlign = HorizontalAlign.Center;
                                        //FpSpread1.Sheets[0].Cells[subjectRow, col + 2].VerticalAlign = VerticalAlign.Middle;
                                    }
                                }
                            }
                            FpSpread1.Sheets[0].Cells[subjectRow, col].CellType = txtCell;
                            FpSpread1.Sheets[0].Cells[subjectRow, col].Text = Convert.ToString(dum_tage_date).Trim();
                            FpSpread1.Sheets[0].Cells[subjectRow, col].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[subjectRow, col].Locked = true;
                            FpSpread1.Sheets[0].Cells[subjectRow, col].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[subjectRow, col].VerticalAlign = VerticalAlign.Middle;
                            cntnflag = true;

                        }
                        divMainContents.Visible = true;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        //FpStudentMarkList.Width = 1000;
                        FpSpread1.Height = 500;
                        FpSpread1.SaveChanges();
                        FpSpread1.Visible = true;
                        final.Visible = true;
                    }

                }
                else
                {
                    lblAlertMsg.Text = "No Record(s) Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegeCode, "Cummilative Mark and Grade"); }
    }

    public void Init_Spread(Farpoint.FpSpread FpSpread1, int type = 0)
    {
        try
        {
            #region FpSpread Style

            FpSpread1.Visible = false;
            divMainContents.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;

            #endregion FpSpread Style

            #region SpreadStyles

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.Black;
            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Medium;
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Center;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

            #endregion SpreadStyles

            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].DefaultStyle = sheetstyle;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.HorizontalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpSpread1.VerticalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.RowHeader.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            Dictionary<string, byte> dicColumnVisiblity = new Dictionary<string, byte>();
            //columnVisibility(ref dicColumnVisiblity);
            bool isRollNoVisible = ColumnHeaderVisiblity(0);
            bool isRegNoVisible = ColumnHeaderVisiblity(1);
            bool isAdmissionNoVisible = ColumnHeaderVisiblity(2);
            bool isStudentTypeVisible = ColumnHeaderVisiblity(3);
            bool isVisibleColumn = false;
            if (type == 0)
            {
                FpSpread1.Sheets[0].ColumnCount = 7;

                byte value = 0;
                FpSpread1.Sheets[0].Columns[0].Width = 35;
                FpSpread1.Sheets[0].Columns[0].Locked = false;
                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                FpSpread1.Sheets[0].Columns[1].Width = 100;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Visible = isRollNoVisible;
                //startColumm = (isRollNoVisible) ? startColumm + 2 : startColumm;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                FpSpread1.Sheets[0].Columns[2].Width = 100;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Visible = isRegNoVisible;
                //startColumm = (isRegNoVisible) ? startColumm + 2 : startColumm;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].SetColumnMerge(2, Farpoint.Model.MergePolicy.Always);

                FpSpread1.Sheets[0].Columns[3].Width = 100;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Visible = isAdmissionNoVisible;
                //startColumm = (isAdmissionNoVisible) ? startColumm + 2 : startColumm;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

                FpSpread1.Sheets[0].Columns[4].Width = 100;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Visible = isStudentTypeVisible;
                //startColumm = (isStudentTypeVisible) ? startColumm + 2 : startColumm;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                FpSpread1.Sheets[0].Columns[5].Width = 85;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Gender";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

                FpSpread1.Sheets[0].Columns[6].Width = 220;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);

                //FpSpread1.Sheets[0].Columns[7].Width = 80;
                //FpSpread1.Sheets[0].Columns[7].Locked = true;
                //FpSpread1.Sheets[0].Columns[7].Resizable = false;
                //FpSpread1.Sheets[0].Columns[7].Visible = true;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Marks\n";
                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);

                //FpSpread1.Sheets[0].Columns[8].Width = 80;
                //FpSpread1.Sheets[0].Columns[8].Locked = true;
                //FpSpread1.Sheets[0].Columns[8].Resizable = false;
                //string convertMark = txtConvertedMaxMark.Text;
                //double convertedMax = 0;
                //double.TryParse(convertMark.Trim(), out convertedMax);
                //string display = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? ((!chkIncludeGrade.Checked) ? "(Out of " + Convert.ToString(convertedMax).Trim() + ")" : Convert.ToString(convertedMax).Trim()) : "";
                //FpSpread1.Sheets[0].Columns[8].Visible = (chkConvertedTo.Checked && txtConvertedMaxMark.Enabled && convertedMax > 0) ? true : false;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Mark\n" + display;
                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);

                //FpSpread1.Sheets[0].Columns[9].Width = 80;
                //FpSpread1.Sheets[0].Columns[9].Locked = true;
                //FpSpread1.Sheets[0].Columns[9].Resizable = false;
                //FpSpread1.Sheets[0].Columns[9].Visible = (chkIncludeGrade.Checked);
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Grade";
                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);

            }
            else
            {
                FpSpread1.Sheets[0].ColumnCount = 7;
                FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.Sheets[0].Columns[0].Width = 45;
                FpSpread1.Sheets[0].Columns[1].Width = 80;
                FpSpread1.Sheets[0].Columns[2].Width = 80;
                FpSpread1.Sheets[0].Columns[3].Width = 100;
                FpSpread1.Sheets[0].Columns[4].Width = 200;
                FpSpread1.Sheets[0].Columns[5].Width = 100;
                FpSpread1.Sheets[0].Columns[6].Width = 100;

                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Locked = false;
                FpSpread1.Sheets[0].Columns[6].Locked = false;

                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;

                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].Columns[1].Visible = true;
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].Columns[6].Visible = true;

                FpSpread1.Sheets[0].Columns[5].CellType = new Farpoint.CheckBoxCellType();
                FpSpread1.Sheets[0].Columns[6].CellType = new Farpoint.CheckBoxCellType();

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = lblterm.Text;
                FpSpread1.Sheets[0].SetColumnMerge(1, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Type";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Code";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Mark";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Grade";

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);

            }
        }
        catch (Exception ex)
        {

        }
    }

    private bool CheckSchoolOrCollege(string collegeCode)
    {
        bool isSchoolOrCollege = false;
        try
        {
            if (!string.IsNullOrEmpty(collegeCode))
            {
                //qry = "select ISNULL(InstType,'0') as InstType,case when ISNULL(InstType,'0')='0' then 'College' when ISNULL(InstType,'0')='1' then 'School' end as CollegeOrSchool from collinfo where college_code='" + collegeCode + "'";
                string qry = "select ISNULL(InstType,'0') as InstType from collinfo where college_code='" + collegeCode + "'";
                string insType = dirAcc.selectScalarString(qry).Trim();
                if (string.IsNullOrEmpty(insType) || insType.Trim() == "0")
                {
                    isSchoolOrCollege = false;
                }
                else if (!string.IsNullOrEmpty(insType) && insType.Trim() == "1")
                {
                    isSchoolOrCollege = true;
                }
                else
                {
                    isSchoolOrCollege = false;
                }
            }
            return isSchoolOrCollege;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public string GetAttendanceStatusName(string attStatusCode)
    {
        string attendanceStatus = string.Empty;
        attStatusCode = attStatusCode.Trim();
        switch (attStatusCode)
        {
            case "1":
                attendanceStatus = "P";
                break;
            case "2":
                attendanceStatus = "A";
                break;
            case "3":
                attendanceStatus = "OD";
                break;
            case "4":
                attendanceStatus = "ML";
                break;
            case "5":
                attendanceStatus = "SOD";
                break;
            case "6":
                attendanceStatus = "NSS";
                break;
            case "7":
                attendanceStatus = "H";
                break;
            case "8":
                attendanceStatus = "NJ";
                break;
            case "9":
                attendanceStatus = "S";
                break;
            case "10":
                attendanceStatus = "L";
                break;
            case "11":
                attendanceStatus = "NCC";
                break;
            case "12":
                attendanceStatus = "HS";
                break;
            case "13":
                attendanceStatus = "PP";
                break;
            case "14":
                attendanceStatus = "SYOD";
                break;
            case "15":
                attendanceStatus = "COD";
                break;
            case "16":
                attendanceStatus = "OOD";
                break;
            case "17":
                attendanceStatus = "LA";
                break;
            default:
                attendanceStatus = string.Empty;
                break;
        }
        return attendanceStatus.ToUpper().Trim();
    }

    public string GetAttendanceStatusCode(string attStatusCode)
    {
        string attendanceStatus = string.Empty;
        attStatusCode = attStatusCode.Trim().ToUpper();
        switch (attStatusCode)
        {
            case "P":
                attendanceStatus = "1";
                break;
            case "A":
                attendanceStatus = "2";
                break;
            case "OD":
                attendanceStatus = "3";
                break;
            case "ML":
                attendanceStatus = "4";
                break;
            case "SOD":
                attendanceStatus = "5";
                break;
            case "NSS":
                attendanceStatus = "6";
                break;
            case "H":
                attendanceStatus = "7";
                break;
            case "NJ":
                attendanceStatus = "8";
                break;
            case "S":
                attendanceStatus = "9";
                break;
            case "L":
                attendanceStatus = "10";
                break;
            case "NCC":
                attendanceStatus = "11";
                break;
            case "HS":
                attendanceStatus = "12";
                break;
            case "PP":
                attendanceStatus = "13";
                break;
            case "SYOD":
                attendanceStatus = "14";
                break;
            case "COD":
                attendanceStatus = "15";
                break;
            case "OOD":
                attendanceStatus = "16";
                break;
            case "LA":
                attendanceStatus = "17";
                break;
            default:
                attendanceStatus = string.Empty;
                break;
        }
        return attendanceStatus;
    }

    private string orderByStudents(string collegeCode, string aliasName = null, string tableName = null, byte includeOrderBy = 0)
    {
        string orderBy = string.Empty;
        try
        {
            string orderBySetting = dirAcc.selectScalarString("select value from master_Settings where settings='order_by' ");//and value<>''
            orderBySetting = orderBySetting.Trim();

            string serialNo = dirAcc.selectScalarString("select LinkValue from inssettings where college_code='" + collegeCode + "' and linkname='Student Attendance'");

            string aliasOrTableName = ((string.IsNullOrEmpty(aliasName) && string.IsNullOrEmpty(tableName)) ? "" : ((!string.IsNullOrEmpty(tableName)) ? tableName.Trim() + "." : ((!string.IsNullOrEmpty(aliasName)) ? aliasName.Trim() + "." : "")));

            orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "roll_no";
            if (serialNo.Trim().ToLower() == "1" || serialNo.ToLower().Trim() == "true")
                orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "serialno";
            else
                switch (orderBySetting)
                {
                    case "0":
                        orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "roll_no";
                        break;
                    case "1":
                        orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "Reg_No";
                        break;
                    case "2":
                        orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,1,2":
                        orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No," + aliasOrTableName + "stud_name";
                        break;
                    case "0,1":
                        orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No";
                        break;
                    case "1,2":
                        orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "Reg_No," + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,2":
                        orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "roll_no," + aliasOrTableName + "Stud_Name";
                        break;
                    default:
                        orderBy = (includeOrderBy == 0) ? "ORDER BY " : "" + aliasOrTableName + "roll_no";
                        break;
                }
        }
        catch (Exception ex)
        {
        }
        return orderBy;
    }

    /// <summary>
    /// Developed By Malang Raja on Dec 7 2016
    /// </summary>
    /// <param name="type">0 For Roll No,1 For Register No,2 For Admission No, 3 For Student Type , 4 For Application No</param>
    /// <param name="dsSettingsOptional">it is Optional Parameter</param>
    /// <returns>true or false</returns>
    private bool ColumnHeaderVisiblity(int type, DataSet dsSettingsOptional = null)
    {
        bool hasValues = false;
        try
        {
            DataSet dsSettings = new DataSet();
            if (dsSettingsOptional == null)
            {
                string grouporusercode = string.Empty;
                if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    string groupCode = Convert.ToString(Session["group_code"]).Trim();
                    string[] groupUser = Convert.ToString(groupCode).Trim().Split(';');
                    if (groupUser.Length > 0)
                    {
                        groupCode = groupUser[0].Trim();
                    }
                    if (!string.IsNullOrEmpty(groupCode.Trim()))
                    {
                        grouporusercode = " and  group_code=" + Convert.ToString(groupCode).Trim() + "";
                    }
                }
                else if (Session["usercode"] != null)
                {
                    grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }
                if (!string.IsNullOrEmpty(grouporusercode))
                {
                    string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type','Application No') and value='1' " + grouporusercode + "";
                    dsSettings = dirAcc.selectDataSet(Master1);
                }
            }
            else
            {
                dsSettings = dsSettingsOptional;
            }
            if (dsSettings.Tables.Count > 0 && dsSettings.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drSettings in dsSettings.Tables[0].Rows)
                {
                    switch (type)
                    {
                        case 0:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "roll no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 1:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "register no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 2:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "admission no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 3:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "student_type")
                            {
                                hasValues = true;
                            }
                            break;
                        case 4:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "application no")
                            {
                                hasValues = true;
                            }
                            break;
                    }
                    if (hasValues)
                        break;
                }
            }
            return hasValues;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    private string getMarkText(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "-1":
                    mark = "AAA";
                    break;
                case "-2":
                    mark = "EL";
                    break;
                case "-3":
                    mark = "EOD";
                    break;
                case "-4":
                    mark = "ML";
                    break;
                case "-5":
                    mark = "SOD";
                    break;
                case "-6":
                    mark = "NSS";
                    break;
                case "-7":
                    mark = "NJ";
                    break;
                case "-8":
                    mark = "S";
                    break;
                case "-9":
                    mark = "L";
                    break;
                case "-10":
                    mark = "NCC";
                    break;
                case "-11":
                    mark = "HS";
                    break;
                case "-12":
                    mark = "PP";
                    break;
                case "-13":
                    mark = "SYOD";
                    break;
                case "-14":
                    mark = "COD";
                    break;
                case "-15":
                    mark = "OOD";
                    break;
                case "-16":
                    mark = "OD";
                    break;
                case "-17":
                    mark = "LA";
                    break;
                case "-18":
                    mark = "RAA";
                    break;
            }
        }
        catch
        {
        }
        return mark;
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
        double convertMax = max_minCal;
        if (maxflag)
        {
            if (r && max_minCal > 0)
            {
                //multiply = max / max_minCal;
                if (maxbool == true && minbool == true && min > 0 && max_minCal > 0)
                {
                    //minmultyply = max_minCal / min;
                    //min = max / minmultyply;
                    double convertMin = (min / max_minCal) * max;
                    min = convertMin;
                }
                if (Mark >= 0)
                    obtainedMark = Convert.ToString(max * (Mark / max_minCal));
                convertMax = max;
            }
            minMark = min.ToString();
            maxMark = txtConvertTo;
        }
    }

    public void persentmonthcal(string collegeCode, string degree, string sem, string rollno, string admitDate)
    {
        try
        {
            medicalLeaveCountPerSession = 0;
            Boolean isadm = false;
            per_abshrs_spl = 0;
            tot_per_hrs_spl = 0;
            tot_conduct_hr_spl = 0;
            tot_ondu_spl = 0;
            tot_ml_spl = 0;
            int my_un_mark = 0;
            int njdate_mng = 0, njdate_evng = 0;
            int per_holidate_mng = 0, per_holidate_evng = 0;
            mng_conducted_half_days = 0;
            evng_conducted_half_days = 0;
            notconsider_value = 0;
            cal_from_date = cal_from_date_tmp;
            cal_to_date = cal_to_date_tmp;
            per_from_date = per_from_gendate;
            per_to_date = per_to_gendate;
            dumm_from_date = per_from_date;

            Hashtable hat = new Hashtable();
            string admdate = admitDate;// ds4.Tables[0].Rows[rows_count]["adm_date"].ToString();
            //Admission_date = Convert.ToDateTime(admdate);
            DateTime.TryParseExact(admdate, "dd/MM/yyyy", null, DateTimeStyles.None, out Admission_date);

            hat.Clear();
            hat.Add("degree_code", degree);
            hat.Add("sem_ester", int.Parse(sem));
            ds = d2.select_method("period_attnd_schedule", hat, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                minpresday = int.Parse(ds.Tables[0].Rows[0]["MIN PREE PER DAY"].ToString());
            }

            hat.Clear();
            hat.Add("colege_code", collegeCode);
            ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
            count = (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0) ? ds1.Tables[0].Rows.Count : 0;

            string daywisecal = da.GetFunction("select value from Master_Settings where settings='Attendance Day Wise Calculation'");
            bool attendanceDayWiseCalculation = false;
            if (daywisecal.Trim() == "1")
            {
                attendanceDayWiseCalculation = true;
            }



            dd = rollno.Trim();
            hat.Clear();
            ds2.Clear();
            hat.Add("std_rollno", rollno.Trim());
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds2 = da.select_method("STUD_ATTENDANCE", hat, "sp");

            mmyycount = (ds2.Tables.Count > 0) ? ds2.Tables[0].Rows.Count : 0;
            moncount = mmyycount - 1;
            deptflag = false;
            if (deptflag == false)
            {
                deptflag = true;
                hat.Clear();
                hat.Add("degree_code", int.Parse(Convert.ToString(degree).Trim()));
                hat.Add("sem", int.Parse(Convert.ToString(sem).Trim()));
                hat.Add("from_date", Convert.ToString(frdate));
                hat.Add("to_date", Convert.ToString(todate));
                hat.Add("coll_code", int.Parse(Convert.ToString(collegeCode)));
                int iscount = 0;
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + degree + " and semester=" + sem;
                DataSet dsholiday = da.select_method_wo_parameter(sqlstr_holiday, "Text");
                if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                }
                hat.Add("iscount", iscount);
                ds3 = da.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");

                DataSet dsondutyva = new DataSet();
                Dictionary<string, int> holiday_table1 = new Dictionary<string, int>();
                Dictionary<string, int> holiday_table2 = new Dictionary<string, int>();
                Dictionary<string, int> holiday_table3 = new Dictionary<string, int>();

                holiday_table11.Clear();
                holiday_table21.Clear();
                holiday_table31.Clear();
                if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count != 0)
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
                        if (!holiday_table11.Contains((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                        }
                    }
                }

                if (ds3.Tables.Count > 1 && ds3.Tables[1].Rows.Count != 0)
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
                        if (!holiday_table2.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table2.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                        }
                    }
                }

                if (ds3.Tables.Count > 2 && ds3.Tables[2].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[2].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        if (!holiday_table31.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                        {
                            holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
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
                        if (holiday_table3.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table3.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                        }
                    }
                }
            }
            if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
            {
                ts = DateTime.Parse(ds3.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                diff_date = Convert.ToString(ts.Days);
                dif_date1 = double.Parse(diff_date.ToString());
            }
            next = 0;

            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                int rowcount = 0;
                int ccount;
                ccount = (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0) ? ds3.Tables[1].Rows.Count : 0;
                ccount = ccount - 1;
                while (dumm_from_date <= (per_to_date))
                {
                    medicalLeaveCountPerSession = 0;
                    nohrsprsentperday = 0;
                    noofdaypresen = 0;
                    isadm = false;
                    if (dumm_from_date >= Admission_date)
                    {
                        isadm = true;
                        int temp_unmark = 0;

                        for (int i = 1; i <= mmyycount; i++)
                        {
                            ds2.Tables[0].DefaultView.RowFilter = "month_year='" + cal_from_date + "' and roll_no='" + rollno + "'";
                            DataView dvattvalue = ds2.Tables[0].DefaultView;
                            if (dvattvalue.Count > 0)
                            {
                                if (cal_from_date == int.Parse(dvattvalue[0]["month_year"].ToString()))
                                {
                                    string[] split_date_time1 = dumm_from_date.ToString().Split(' ');
                                    string[] dummy_split = split_date_time1[0].Split('/');


                                    if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()))
                                    {
                                        holiday_table11.Add(((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()), "3*0*0");
                                    }

                                    if (holiday_table11.Contains((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()))
                                    {
                                        value_holi_status = holiday_table11[dummy_split[1].ToString() + "/" + dummy_split[0].ToString() + "/" + dummy_split[2].ToString()].ToString();
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

                                        if (ds3.Tables.Count > 1 && ds3.Tables[1].Rows.Count != 0)
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
                                        if (ds3.Tables.Count > 2 && ds3.Tables[2].Rows.Count != 0)
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
                                        per_leavehrs = 0;
                                        if (split_holiday_status_1 == "1")
                                        {
                                            for (i = 1; i <= fnhrs; i++)
                                            {
                                                date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                                value = dvattvalue[0][date].ToString();
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
                                                    if (value == "4")
                                                    {
                                                        medicalLeaveCountPerSession++;
                                                        medicalLeaveHours++;
                                                    }

                                                    if (value == "10")
                                                    {
                                                        per_leavehrs++;
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
                                                    my_un_mark++;
                                                }

                                            }
                                            nohrsprsentperday = per_perhrs + njhr;
                                            if (per_perhrs + njhr >= minpresI)
                                            {
                                                Present += 0.5;
                                                noofdaypresen = 0.5;
                                            }
                                            else if (per_abshrs >= 1)
                                            {
                                                Absent += 0.5;
                                                absent_point += absent_pointer / 2;
                                                studentabsentfine = studentabsentfine + moringabsentfine;
                                                if (per_leavehrs > 0)
                                                {
                                                    Leave += 0.5;
                                                    leavfinaeamount = leavfinaeamount + moringabsentfine;
                                                }
                                            }
                                            if (njhr >= minpresI)
                                            {
                                                njdate += 0.5;
                                                njdate_mng += 1;
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
                                            if (medicalLeaveCountPerSession + njhr >= minpresI)
                                            {
                                                medicalLeaveDays = medicalLeaveDays + 0.5;
                                            }
                                        }

                                        medicalLeaveCountPerSession = 0;
                                        per_perhrs = 0;
                                        per_abshrs = 0;
                                        temp_unmark = 0;
                                        per_leavehrs = 0;
                                        njhr = 0;
                                        int k = fnhrs + 1;
                                        if (split_holiday_status_2 == "1")
                                        {
                                            for (i = k; i <= NoHrs; i++)
                                            {
                                                date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                                value = dvattvalue[0][date].ToString();
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
                                                    if (value == "10")
                                                    {
                                                        per_leavehrs++;
                                                    }
                                                    if (value == "4")
                                                    {
                                                        medicalLeaveCountPerSession++;
                                                        medicalLeaveHours++;
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
                                                    my_un_mark++;
                                                }
                                            }
                                            nohrsprsentperday = nohrsprsentperday + per_perhrs + njhr;
                                            if (per_perhrs + njhr >= minpresII)
                                            {
                                                Present += 0.5;
                                                noofdaypresen = noofdaypresen + 0.5;
                                            }
                                            else if (per_abshrs >= 1)
                                            {
                                                Absent += 0.5;
                                                absent_point += absent_pointer / 2;
                                                studentabsentfine = studentabsentfine + eveingabsentfine;
                                                if (per_leavehrs > 0)
                                                {
                                                    Leave += 0.5;
                                                    leavfinaeamount = leavfinaeamount + eveingabsentfine;
                                                }
                                            }
                                            if (njhr >= minpresII)
                                            {
                                                njdate_evng += 1;
                                                njdate += 0.5;
                                            }

                                            if (medicalLeaveCountPerSession + njhr >= minpresII)
                                            {
                                                medicalLeaveDays = medicalLeaveDays + 0.5;
                                            }

                                            if (attendanceDayWiseCalculation)
                                            {
                                                if (nohrsprsentperday < minpresday)
                                                {
                                                    Present = Present - noofdaypresen;
                                                    Absent = Absent + noofdaypresen;
                                                }
                                            }
                                            nohrsprsentperday = 0;
                                            noofdaypresen = 0;
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
                                        per_abshrs = 0;
                                        unmark = 0;
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
                                i = mmyycount + 1;
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
                    nohrsprsentperday = 0;
                    noofdaypresen = 0;
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
            per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value - dum_unmark; //dum_unmark hided on 08.08.12 // ((Convert.ToInt16(njdate_mng) * fnhrs) + (Convert.ToInt16(njdate_evng) * (NoHrs - fnhrs)));
            per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value; //added on 08.08.12,notconsider value added for hr suspension,09.08.12 mythili
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
        }
        catch
        {
        }
    }

    public void CalculateRankByPercentage(Dictionary<string, double> dicTotalMarks, Dictionary<string, double> dicTotalPercentage, ref DataTable dtRankList, bool rankOnePlus = false)
    {
        try
        {
            dicTotalPercentage = dicTotalPercentage.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            dicTotalMarks = dicTotalMarks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            dtRankList = new DataTable();
            dtRankList.Clear();
            dtRankList.Columns.Add("AppNo");
            dtRankList.Columns.Add("Total");
            dtRankList.Columns.Add("Percentage");
            dtRankList.Columns.Add("Rank");
            dtRankList.Columns.Add("RankOnePlus");
            DataRow drRankList;
            int rank = 1;
            int rankOnePlusBy = 1;
            int actualRank = 0;
            double previousPercentage = 0;
            foreach (KeyValuePair<string, double> keyPercentage in dicTotalPercentage)
            {
                string keyAppNo = keyPercentage.Key.Trim();
                double currentPercentage = keyPercentage.Value;
                double totalMark = 0;
                if (dicTotalMarks.ContainsKey(keyAppNo))
                {
                    totalMark = dicTotalMarks[keyAppNo];
                }
                bool equalToPrevious = true;
                if (previousPercentage != 0 && previousPercentage != currentPercentage)
                {
                    if (rankOnePlus && actualRank != 0)
                    {
                        rankOnePlusBy = actualRank;
                    }
                    rank++;
                    rankOnePlusBy++;
                    equalToPrevious = false;
                }
                actualRank++;
                previousPercentage = currentPercentage;
                drRankList = dtRankList.NewRow();
                drRankList["AppNo"] = keyAppNo;
                drRankList["Total"] = totalMark;
                drRankList["Percentage"] = currentPercentage;
                drRankList["Rank"] = rank;
                drRankList["RankOnePlus"] = rankOnePlusBy;
                dtRankList.Rows.Add(drRankList);
            }
        }
        catch (Exception ex)
        {
        }
    }

    #region Alert Popup Close

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    #endregion

    public void bindvaules()
    {
        double maximtot = 0;
        batchyear = dropyear.SelectedItem.Text.ToString();
        degreecode = ddstandard.SelectedItem.Value.ToString();
        term = dropterm.SelectedItem.Text;
        string str_colno = string.Empty;
        string str_rolladmit = string.Empty;
        string str_criteriano = string.Empty;
        string str_subject_no = string.Empty;
        string[] split_criteriano;
        double fatotal = 0;
        double satotal = 0;
        double fulltotal = 0;
        string grademain = string.Empty;
        DataSet dsgradechk = new DataSet();
        DataSet ds = new DataSet();
        DataView dv = new DataView();

        int count = dtallcol.Rows.Count;
        if (count > 0)
        {
            for (int admitno = 0; admitno < FpSpread1.Sheets[0].RowCount; admitno++)
            {
                str_rolladmit = FpSpread1.Sheets[0].Cells[admitno, 1].Text.Trim();
                string stud_roll = FpSpread1.Sheets[0].Cells[admitno, 1].Tag.ToString();
                string clm = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code    and r.Roll_Admit='" + str_rolladmit + "' ;";
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
                        string admdate = dv[0]["adm_date"].ToString();
                        string Roll_No = dv[0]["Roll_No"].ToString();
                        currentsem = dv[0]["Current_Semester"].ToString(); ;
                        string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + term + "' and degree_code='" + degreecode + "' and batch_year='" + batchyear + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sem, "Text");

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            string startdate = ds.Tables[0].Rows[0]["start_date"].ToString();
                            string enddate = ds.Tables[0].Rows[0]["end_date"].ToString();
                            persentmonthcal(Roll_No, admdate, startdate, enddate);
                            lbltot_att1 = pre_present_date.ToString();
                            lbltot_work1 = per_workingdays.ToString();
                        }

                    }
                }

                for (int i = 0; i < dtallcol.Rows.Count; i++)
                {

                    if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "FA")
                    {
                        maximtot = 0;
                        fatotal = 0;
                        str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                        str_criteriano = dtallcol.Rows[i]["Criteria nos"].ToString().Trim();
                        str_subject_no = dtallcol.Rows[i]["subjetno"].ToString().Trim();
                        split_criteriano = str_criteriano.Split('-');

                        fatotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no is null and c.Istype='Calculate 1' and s.subject_no='" + str_subject_no + "'"));
                        maximtot = maximtot + Convert.ToDouble(d2.GetFunction("SELECT  c.conversion FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no is null and c.Istype='Calculate 1' and s.subject_no='" + str_subject_no + "'"));
                        FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fatotal);
                        fulltotal = fatotal;
                    }

                    if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "Grade")
                    {
                        str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                        grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradesystemfa[Convert.ToInt32(term) - 1] + "' and  " + fatotal + " between Frange and Trange";
                        dsgradechk.Clear();
                        dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                        if (dsgradechk.Tables[0].Rows.Count > 0)
                        {
                            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                        }
                        else
                        {
                            grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradesystemfa[Convert.ToInt32(term) - 1] + "' and  " + fatotal + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                            }
                        }

                    }
                    if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "SA")
                    {
                        str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                        str_criteriano = dtallcol.Rows[i]["Criteria nos"].ToString().Trim();
                        str_subject_no = dtallcol.Rows[i]["subjetno"].ToString().Trim();
                        split_criteriano = str_criteriano.Split('-');
                        //if (split_criteriano.GetUpperBound(0) >= 0)
                        //{
                        //    for (int j = 0; j <= split_criteriano.GetUpperBound(0); j++)
                        //    {

                        satotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no is null and c.Istype='Calculate 2' and s.subject_no='" + str_subject_no + "'"));
                        maximtot = maximtot + Convert.ToDouble(d2.GetFunction("SELECT c.conversion FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no is null and c.Istype='Calculate 2' and s.subject_no='" + str_subject_no + "'"));
                        //    }
                        //}
                        FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(satotal);
                        fulltotal = fulltotal + satotal;

                    }
                    if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "SAGrade")
                    {

                        str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                        grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradesystemsa[Convert.ToInt32(term) - 1] + "' and  " + satotal + " between Frange and Trange";
                        dsgradechk.Clear();
                        dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                        if (dsgradechk.Tables[0].Rows.Count > 0)
                        {
                            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                        }
                        else
                        {
                            grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradesystemsa[Convert.ToInt32(term) - 1] + "' and  " + satotal + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                            }
                        }
                    }


                    if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "Total")
                    {
                        str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();

                        FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fulltotal);


                    }

                    if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "TotalGrade")
                    {
                        fulltotal = fulltotal / maximtot;
                        fulltotal = fulltotal * 100;
                        fulltotal = Math.Round(fulltotal, 2);
                        str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                        str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                        grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fulltotal + " between Frange and Trange";
                        dsgradechk.Clear();
                        dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                        if (dsgradechk.Tables[0].Rows.Count > 0)
                        {
                            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                        }
                        else
                        {
                            grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fulltotal + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                            }
                        }
                        fatotal = 0;
                        satotal = 0;
                        fulltotal = 0;
                        FpSpread1.Sheets[0].Cells[admitno, FpSpread1.Sheets[0].ColumnCount - 3].Text = lbltot_att1;

                        double percent = 0;

                        if (lbltot_work1.Trim() != "" && lbltot_att1.Trim() != "" && lbltot_work1.Trim() != "0" && lbltot_att1.Trim() != "0")
                        {
                            percent = (Convert.ToDouble(lbltot_att1) / Convert.ToDouble(lbltot_work1));
                            percent = percent * 100;
                            percent = Math.Round(percent, 2);
                        }

                        FpSpread1.Sheets[0].Cells[admitno, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(percent);
                    }

                }

                if (dtallotherscol.Rows.Count > 0)
                {

                    for (int i = 0; i < dtallotherscol.Rows.Count; i++)
                    {

                        if (dtallotherscol.Rows[i]["Colname"].ToString().Trim() == "Mark")
                        {
                            str_colno = dtallotherscol.Rows[i]["colno"].ToString().Trim();
                            //str_criteriano = dtallcol.Rows[i]["Criteria nos"].ToString().Trim();
                            str_subject_no = dtallotherscol.Rows[i]["subjetno"].ToString().Trim();

                            //fatotal = Convert.ToDouble(d2.GetFunction("select top 1  r.marks_obtained from result r,registration reg,exam_type et,subjectchooser sc  where r.exam_code=et.exam_code  and reg.roll_no=r.roll_no and sc.roll_no=reg.roll_no and reg.cc=0 and reg.delflag=0 and reg.exam_flag <>'Debar'  and et.subject_no='" + str_subject_no + "' and et.subject_no=sc.subject_no  and r.roll_no='" + stud_roll + "'  ORDER BY reg.roll_no"));
                            //double maximtotal = Convert.ToDouble(d2.GetFunction("select maxtotal from subject where subject_no='" + str_subject_no + "'"));
                            //fatotal = (fatotal / maximtotal);
                            //fatotal = fatotal * 100;
                            //fatotal = Math.Round(fatotal, 2);
                            fatotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no is null and c.Istype='Calculate 1' and s.subject_no='" + str_subject_no + "'"));
                            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fatotal);

                        }

                        if (dtallotherscol.Rows[i]["Colname"].ToString().Trim() == "Grade")
                        {
                            str_colno = dtallotherscol.Rows[i]["colno"].ToString().Trim();
                            grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fatotal + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno) + 1].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                            }
                            else
                            {
                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fatotal + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno) + 1].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                                }
                            }

                        }
                    }
                }
            }


        }

    }

    public void bindheader()
    {
        try
        {
            string otherssubject_sql = string.Empty;
            dtallcol.Columns.Add("Colname");
            dtallcol.Columns.Add("colno");
            dtallcol.Columns.Add("Criteria nos");
            dtallcol.Columns.Add("subjetno");

            dtallotherscol.Columns.Add("Colname");
            dtallotherscol.Columns.Add("colno");
            dtallotherscol.Columns.Add("subjetno");

            DataSet ds_subject = new DataSet();
            DataSet otherds_subject = new DataSet();

            string fasaCRITERIA_NO = string.Empty;
            double fatotal = 0;
            //double satotal = 0;
            //double fulltotal = 0;
            double maxfatotal = 0;
            double maxsatotal = 0;
            double maxfulltotal = 0;
            string otherconvetedvalue = string.Empty;
            // collcode = " and r.college_code='" + ddschool.SelectedItem.Value.ToString() + "'";
            batchyear = "  and y.Batch_Year='" + dropyear.SelectedItem.Text.ToString() + "'";
            degreecode = "  and degree_code='" + ddstandard.SelectedItem.Value.ToString() + "'";
            term = " and semester in ('" + dropterm.SelectedItem.Text + "')";
            //string subject_sql = "select distinct  subject_no,subject_name from subject s,syllabus_master y where s.syll_code = y.syll_code  ";
            //subject_sql = subject_sql + batchyear + degreecode + term + " order by subject_no,subject_name;";

            otherssubject_sql = "select distinct  subject_no,subject_name from subject s,syllabus_master y,sub_sem ss  where s.syll_code = y.syll_code and ss.syll_code=s.syll_code and s.subType_no=ss.subType_no and y.syll_code=ss.syll_code   and subject_type='others' and promote_count=1 ";
            otherssubject_sql = otherssubject_sql + batchyear + degreecode + term + " order by subject_no,subject_name;";

            otherds_subject.Clear();
            otherds_subject = d2.select_method_wo_parameter(otherssubject_sql, "Text");
            string otherssubjectcode = string.Empty;
            string otherssubjectcode01 = string.Empty;

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
            ds_subject.Clear();
            ds_subject = d2.select_method_wo_parameter(subject_sql, "Text");
            if (ds_subject.Tables[0].Rows.Count > 0)
            {
                if (ds_subject.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds_subject.Tables[0].Rows.Count; i++)
                    {
                        DataView dvmark = new DataView();
                        string str_subject_name = ds_subject.Tables[0].Rows[i]["subject_name"].ToString();
                        string str_subject_no = ds_subject.Tables[0].Rows[i]["subject_no"].ToString();
                        //  ds_subject.Tables[1].DefaultView.RowFilter = "subject_no='" + str_subject_no + "'";
                        // dvmark = ds_subject.Tables[1].DefaultView;
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = str_subject_name;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;

                        // FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 1, 6);
                        maxfatotal = 0;
                        fasaCRITERIA_NO = string.Empty;
                        for (int j = 0; j < ds_subject.Tables[1].Rows.Count; j++)
                        {
                            //if (fasaCRITERIA_NO.Trim() == "")
                            //{
                            //    fasaCRITERIA_NO = Convert.ToString(ds_subject.Tables[1].Rows[j]["CRITERIA_NO"].ToString());
                            //}
                            if (j < 1)
                            {
                                maxfatotal = maxfatotal + Convert.ToDouble(ds_subject.Tables[1].Rows[j]["Conversion_value"].ToString());
                                //fatotal = Convert.ToDouble(ds_subject.Tables[1].Rows[j]["Conversion_value"].ToString());
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "FA";
                                //if (j == 1)
                                //{
                                //    fasaCRITERIA_NO = fasaCRITERIA_NO + "-" + Convert.ToString(ds_subject.Tables[1].Rows[j]["CRITERIA_NO"].ToString());
                                //    dtallcol.Rows.Add("FA", FpSpread1.Sheets[0].ColumnCount - 1, fasaCRITERIA_NO, str_subject_no);
                                //}
                                dtallcol.Rows.Add("FA", FpSpread1.Sheets[0].ColumnCount - 1, " ", str_subject_no);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(maxfatotal);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                            }
                            else
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Grade";
                                dtallcol.Rows.Add("Grade", FpSpread1.Sheets[0].ColumnCount - 1, fasaCRITERIA_NO, str_subject_no);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                maxsatotal = Convert.ToDouble(ds_subject.Tables[1].Rows[j]["Conversion_value"].ToString());
                                FpSpread1.Sheets[0].ColumnCount++;
                                //fasaCRITERIA_NO  =string.Empty;
                                //if (fasaCRITERIA_NO.Trim() == "")
                                //{
                                //    fasaCRITERIA_NO = Convert.ToString(ds_subject.Tables[1].Rows[j]["CRITERIA_NO"].ToString());
                                //}
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "SA";
                                dtallcol.Rows.Add("SA", FpSpread1.Sheets[0].ColumnCount - 1, fasaCRITERIA_NO, str_subject_no);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(maxsatotal);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Grade";
                                dtallcol.Rows.Add("SAGrade", FpSpread1.Sheets[0].ColumnCount - 1, fasaCRITERIA_NO, str_subject_no);
                                fasaCRITERIA_NO = string.Empty;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                            }
                            maxfulltotal = maxfatotal + maxsatotal;
                        }
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                        dtallcol.Rows.Add("Total", FpSpread1.Sheets[0].ColumnCount - 1, " ", str_subject_no);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(maxfulltotal);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Grade";
                        dtallcol.Rows.Add("TotalGrade", FpSpread1.Sheets[0].ColumnCount - 1, " ", str_subject_no);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                        //int columnno = FpSpread1.Sheets[0].ColumnCount - 1;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 6, 1, 6);
                    }
                }
            }
            if (otherds_subject.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < otherds_subject.Tables[0].Rows.Count; i++)
                {
                    string str_subject_name = otherds_subject.Tables[0].Rows[i]["subject_name"].ToString();
                    string str_subject_no = otherds_subject.Tables[0].Rows[i]["subject_no"].ToString();

                    if (ds_subject.Tables[2].Rows.Count > 0)
                    {
                        otherconvetedvalue = ds_subject.Tables[2].Rows[0]["Conversion_value"].ToString();
                    }
                    FpSpread1.Sheets[0].ColumnCount++;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = str_subject_name;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Mark " + otherconvetedvalue + "";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                    dtallotherscol.Rows.Add("Mark", FpSpread1.Sheets[0].ColumnCount - 1, str_subject_no);
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Grade";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;

                    dtallotherscol.Rows.Add("Grade", FpSpread1.Sheets[0].ColumnCount - 2, str_subject_no);

                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 2, 1, 2);
                    // FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 48, 1, 2);
                    //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 48, 2, 1);
                }
            }
            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Attendance";
            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 6, 1, 2);
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "No. of Days Present ";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "%";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Remarks";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 3, 1, 2);
            bindvaules();
            for (int g = 0; g < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; g++)
            {
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].ForeColor = Color.White;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, g].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, g].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, g].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, g].ForeColor = Color.White;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, g].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, g].VerticalAlign = VerticalAlign.Middle;

                FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].ForeColor = Color.White;
                FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].VerticalAlign = VerticalAlign.Middle;
            }

            for (int g = 3; g < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; g++)
            {
                for (int gr = 0; gr < FpSpread1.Sheets[0].Rows.Count; gr++)
                {
                    FpSpread1.Sheets[0].Columns[2].Width = 350;
                    FpSpread1.Sheets[0].Cells[gr, g].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[gr, g].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[gr, g].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[gr, g].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[gr, g].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].Cells[gr, g].ForeColor = Color.Black;
                }
            }
        }
        catch
        {
        }

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
                    string szFile = print + ".xls";
                    FpSpread1.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Flush();
                    Response.WriteFile(szPath + szFile);
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

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = true;
            lblnorec.Text = string.Empty;
            string sec = string.Empty;
            if (dropsec.Enabled == true)
            {
                if (dropsec.Items.Count > 0)
                {
                    sec = Convert.ToString(dropsec.SelectedItem.Text).Trim();
                    if (sec.ToLower().Trim() != "all" && sec.ToLower().Trim() != "" && sec.ToLower().Trim() != "-1")
                    {
                        sec = Convert.ToString(dropsec.SelectedItem.Text).Trim();
                    }
                    else
                    {
                        sec = string.Empty;
                    }
                }
            }
            string date_filt = "Class :" + ddstandard.SelectedItem.Text.ToString() + "-" + ((string.IsNullOrEmpty(sec)) ? "Section :" + sec : "");
            string test = "Term :" + dropterm.SelectedItem.ToString();
            string degreedetails = string.Empty;

            degreedetails = "Scholastic Areas" + "@" + date_filt + "@" + test;
            string pagename = "scholatic_grade.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }

    }

    public void onlyfasa()
    {
        batchyear = dropyear.SelectedItem.Text.ToString();
        degreecode = ddstandard.SelectedItem.Value.ToString();
        term = dropterm.SelectedItem.Text;
        string str_colno = string.Empty;
        string str_rolladmit = string.Empty;
        string str_criteriano = string.Empty;
        string str_subject_no = string.Empty;
        string[] split_criteriano;
        double fatotal = 0;
        double satotal = 0;
        double fulltotal = 0;
        string grademain = string.Empty;
        DataSet dsgradechk = new DataSet();
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        string faminus = string.Empty;
        string saminus = string.Empty;
        int count = dtallcol.Rows.Count;
        if (count > 0)
        {
            for (int admitno = 0; admitno < FpSpread1.Sheets[0].RowCount; admitno++)
            {
                str_rolladmit = FpSpread1.Sheets[0].Cells[admitno, 1].Text.Trim();
                string stud_roll = FpSpread1.Sheets[0].Cells[admitno, 1].Tag.ToString();
                string clm = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code    and r.Roll_Admit='" + str_rolladmit + "' ;";
                ds.Clear();
                ds = d2.select_method_wo_parameter(clm, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    ds.Tables[1].DefaultView.RowFilter = "Roll_Admit='" + str_rolladmit + "'";
                    dv = ds.Tables[1].DefaultView;
                    int count4 = 0;
                    count4 = dv.Count;
                    if (count4 > 0)
                    {
                        string admdate = dv[0]["adm_date"].ToString();
                        string Roll_No = dv[0]["Roll_No"].ToString();
                        currentsem = dv[0]["Current_Semester"].ToString(); ;
                        string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + term + "' and degree_code='" + degreecode + "' and batch_year='" + batchyear + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sem, "Text");

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            string startdate = ds.Tables[0].Rows[0]["start_date"].ToString();
                            string enddate = ds.Tables[0].Rows[0]["end_date"].ToString();
                            persentmonthcal(Roll_No, admdate, startdate, enddate);
                            lbltot_att1 = pre_present_date.ToString();
                            lbltot_work1 = per_workingdays.ToString();
                        }
                    }
                }

                for (int i = 0; i < dtallcol.Rows.Count; i++)
                {
                    if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "FA")
                    {
                        fatotal = 0;
                        str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                        //str_criteriano = dtallcol.Rows[i]["Criteria nos"].ToString().Trim();
                        str_subject_no = dtallcol.Rows[i]["subjetno"].ToString().Trim();
                        //split_criteriano = str_criteriano.Split('-');
                        fatotal = Convert.ToDouble(d2.GetFunction(" select marks_obtained from criteriaforinternal c,syllabus_master y,Exam_type e,Result r,subject u,sub_sem ss where c.syll_code = y.syll_code and c.Criteria_no = e.criteria_no and r.exam_code = e.exam_code and e.subject_no = u.subject_no  and y.Batch_Year = '" + dropyear.SelectedItem.Text.ToString() + "' and  degree_code = '" + ddstandard.SelectedItem.Value.ToString() + "' and semester = '" + dropterm.SelectedItem.Text + "'    and ss.syll_code=y.syll_code and roll_no='" + stud_roll + "' and u.subject_no='" + str_subject_no + "' and criteria='FA' and ss.subType_no=u.subType_no"));
                        if (fatotal < 0)
                        {
                            // loadmarkat(string mr)
                            faminus = loadmarkat(Convert.ToString(fatotal));
                            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(faminus);
                        }
                        else
                        {
                            faminus = string.Empty;
                            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fatotal);
                            fulltotal = fatotal;
                        }
                    }

                    if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "Grade")
                    {
                        str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                        if (faminus == "")
                        {
                            grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradesystemfa[Convert.ToInt32(term) - 1] + "' and  " + fatotal + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                            }
                            else
                            {
                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradesystemfa[Convert.ToInt32(term) - 1] + "' and  " + fatotal + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                }
                            }
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = faminus;
                        }
                    }
                    if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "SA")
                    {
                        str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                        //str_criteriano = dtallcol.Rows[i]["Criteria nos"].ToString().Trim();
                        str_subject_no = dtallcol.Rows[i]["subjetno"].ToString().Trim();
                        //split_criteriano = str_criteriano.Split('-');
                        //if (split_criteriano.GetUpperBound(0) >= 0)
                        //{
                        //    for (int j = 0; j <= split_criteriano.GetUpperBound(0); j++)
                        //    {
                        satotal = Convert.ToDouble(d2.GetFunction(" select marks_obtained   from criteriaforinternal c,syllabus_master y,Exam_type e,Result r,subject u,sub_sem ss where c.syll_code = y.syll_code and c.Criteria_no = e.criteria_no and r.exam_code = e.exam_code and e.subject_no = u.subject_no  and y.Batch_Year = '" + dropyear.SelectedItem.Text.ToString() + "' and  degree_code = '" + ddstandard.SelectedItem.Value.ToString() + "' and semester = '" + dropterm.SelectedItem.Text + "'    and ss.syll_code=y.syll_code and roll_no='" + stud_roll + "' and u.subject_no='" + str_subject_no + "' and criteria='SA' and ss.subType_no=u.subType_no"));
                        //    }
                        //}
                        if (satotal < 0)
                        {
                            // loadmarkat(string mr)
                            saminus = loadmarkat(Convert.ToString(satotal));
                            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(saminus);
                        }
                        else
                        {
                            saminus = string.Empty;
                            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(satotal);
                            fulltotal = fulltotal + satotal;
                        }
                    }
                    if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "SAGrade")
                    {
                        str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                        if (saminus == "")
                        {
                            grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradesystemsa[Convert.ToInt32(term) - 1] + "' and  " + satotal + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                            }
                            else
                            {
                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradesystemsa[Convert.ToInt32(term) - 1] + "' and  " + satotal + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                }
                            }
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = saminus;
                        }
                    }
                    if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "Total")
                    {
                        str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                        FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fulltotal);
                        //if(faminus!="" && saminus=="")
                        //{
                        //    FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = faminus+"/"+ Convert.ToString(fulltotal);
                        //}
                        //if (faminus == "" && saminus != "")
                        //{
                        //    FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fulltotal) + "/" + saminus;
                        //}
                        //if (faminus == "" && saminus == "")
                        //{
                        //    FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fulltotal);
                        //}
                        //if (faminus != "" && saminus != "")
                        //{
                        //    FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = faminus + "/" + saminus;
                        //}
                    }
                    if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "TotalGrade")
                    {
                        string fasaminusgrade = string.Empty;
                        str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                        str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                        grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fulltotal + " between Frange and Trange";
                        dsgradechk.Clear();
                        dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                        if (dsgradechk.Tables[0].Rows.Count > 0)
                        {
                            // FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                            fasaminusgrade = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                        }
                        else
                        {
                            grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fulltotal + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                // FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                fasaminusgrade = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                            }
                        }

                        if (faminus != "" && saminus == "")
                        {
                            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = faminus + "/" + fasaminusgrade;
                        }
                        if (faminus == "" && saminus != "")
                        {
                            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = fasaminusgrade + "/" + saminus;
                        }
                        if (faminus == "" && saminus == "")
                        {
                            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = fasaminusgrade;
                        }
                        if (faminus != "" && saminus != "")
                        {
                            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = faminus + "/" + saminus;
                        }
                        fatotal = 0;
                        satotal = 0;
                        fulltotal = 0;
                        faminus = string.Empty;
                        saminus = string.Empty;
                        FpSpread1.Sheets[0].Cells[admitno, FpSpread1.Sheets[0].ColumnCount - 3].Text = lbltot_att1;
                        double percent = 0;
                        if (lbltot_work1.Trim() != "" && lbltot_att1.Trim() != "" && lbltot_work1.Trim() != "0" && lbltot_att1.Trim() != "0")
                        {
                            percent = (Convert.ToDouble(lbltot_att1) / Convert.ToDouble(lbltot_work1));
                            percent = percent * 100;
                            percent = Math.Round(percent, 2);
                        }
                        FpSpread1.Sheets[0].Cells[admitno, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(percent);
                    }
                }

                if (dtallotherscol.Rows.Count > 0)
                {
                    for (int i = 0; i < dtallotherscol.Rows.Count; i++)
                    {
                        if (dtallotherscol.Rows[i]["Colname"].ToString().Trim() == "Mark")
                        {
                            str_colno = dtallotherscol.Rows[i]["colno"].ToString().Trim();
                            //str_criteriano = dtallcol.Rows[i]["Criteria nos"].ToString().Trim();
                            str_subject_no = dtallotherscol.Rows[i]["subjetno"].ToString().Trim();

                            //fatotal = Convert.ToDouble(d2.GetFunction("select top 1  r.marks_obtained from result r,registration reg,exam_type et,subjectchooser sc  where r.exam_code=et.exam_code  and reg.roll_no=r.roll_no and sc.roll_no=reg.roll_no and reg.cc=0 and reg.delflag=0 and reg.exam_flag <>'Debar'  and et.subject_no='" + str_subject_no + "' and et.subject_no=sc.subject_no  and r.roll_no='" + stud_roll + "'  ORDER BY reg.roll_no"));
                            //double maximtotal = Convert.ToDouble(d2.GetFunction("select maxtotal from subject where subject_no='" + str_subject_no + "'"));
                            //fatotal = (fatotal / maximtotal);
                            //fatotal = fatotal * 100;
                            //fatotal = Math.Round(fatotal, 2);
                            fatotal = Convert.ToDouble(d2.GetFunction(" select marks_obtained   from criteriaforinternal c,syllabus_master y,Exam_type e,Result r,subject u,sub_sem ss where c.syll_code = y.syll_code and c.Criteria_no = e.criteria_no and r.exam_code = e.exam_code and e.subject_no = u.subject_no  and y.Batch_Year = '" + dropyear.SelectedItem.Text.ToString() + "' and  degree_code = '" + ddstandard.SelectedItem.Value.ToString() + "' and semester = '" + dropterm.SelectedItem.Text + "'    and ss.syll_code=y.syll_code and roll_no='" + stud_roll + "' and u.subject_no='" + str_subject_no + "' and criteria like 'other%' and ss.subType_no=u.subType_no"));
                            // fatotal=fatotal+ Convert.ToDouble(d2.GetFunction(" select marks_obtained   from criteriaforinternal c,syllabus_master y,Exam_type e,Result r,subject u,sub_sem ss where c.syll_code = y.syll_code and c.Criteria_no = e.criteria_no and r.exam_code = e.exam_code and e.subject_no = u.subject_no  and y.Batch_Year = '" + dropyear.SelectedItem.Text.ToString() + "' and  degree_code = '" + ddstandard.SelectedItem.Value.ToString() + "' and semester = '" + dropterm.SelectedItem.Text + "'    and ss.syll_code=y.syll_code and roll_no='" + stud_roll + "' and u.subject_no='" + str_subject_no + "' and criteria='SA' and ss.subType_no=u.subType_no"));

                            double maxim = Convert.ToDouble(d2.GetFunction(" select e.max_mark   from criteriaforinternal c,syllabus_master y,Exam_type e,Result r,subject u,sub_sem ss where c.syll_code = y.syll_code and c.Criteria_no = e.criteria_no and r.exam_code = e.exam_code and e.subject_no = u.subject_no  and y.Batch_Year = '" + dropyear.SelectedItem.Text.ToString() + "' and  degree_code = '" + ddstandard.SelectedItem.Value.ToString() + "' and semester = '" + dropterm.SelectedItem.Text + "'    and ss.syll_code=y.syll_code and roll_no='" + stud_roll + "' and u.subject_no='" + str_subject_no + "'  and criteria like 'other%' and ss.subType_no=u.subType_no"));
                            //  maxim = maxim + Convert.ToDouble(d2.GetFunction(" select e.max_mark   from criteriaforinternal c,syllabus_master y,Exam_type e,Result r,subject u,sub_sem ss where c.syll_code = y.syll_code and c.Criteria_no = e.criteria_no and r.exam_code = e.exam_code and e.subject_no = u.subject_no  and y.Batch_Year = '" + dropyear.SelectedItem.Text.ToString() + "' and  degree_code = '" + ddstandard.SelectedItem.Value.ToString() + "' and semester = '" + dropterm.SelectedItem.Text + "'    and ss.syll_code=y.syll_code and roll_no='" + stud_roll + "' and u.subject_no='" + str_subject_no + "' and criteria='SA' and ss.subType_no=u.subType_no"));
                            if (maxim > 0)
                            {
                                fatotal = fatotal / maxim;
                                fatotal = fatotal * 100;
                            }
                            else
                            {
                                fatotal = 0;
                            }
                            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fatotal);
                        }

                        if (dtallotherscol.Rows[i]["Colname"].ToString().Trim() == "Grade")
                        {
                            str_colno = dtallotherscol.Rows[i]["colno"].ToString().Trim();
                            grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fatotal + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno) + 1].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                            }
                            else
                            {
                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fatotal + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno) + 1].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                }
                            }
                        }
                    }
                }
            }
        }

        for (int g = 0; g < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; g++)
        {
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].ForeColor = Color.White;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, g].VerticalAlign = VerticalAlign.Middle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, g].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, g].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, g].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, g].ForeColor = Color.White;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, g].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, g].VerticalAlign = VerticalAlign.Middle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].ForeColor = Color.White;
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].VerticalAlign = VerticalAlign.Middle;
        }

        for (int g = 3; g < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; g++)
        {
            for (int gr = 0; gr < FpSpread1.Sheets[0].Rows.Count; gr++)
            {
                FpSpread1.Sheets[0].Columns[2].Width = 350;
                FpSpread1.Sheets[0].Cells[gr, g].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[gr, g].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[gr, g].Font.Bold = true;
                FpSpread1.Sheets[0].Cells[gr, g].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[gr, g].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Cells[gr, g].ForeColor = Color.Black;
            }
        }

        for (int g = 0; g < 3; g++)
        {
            for (int gr = 0; gr < FpSpread1.Sheets[0].Rows.Count; gr++)
            {
                FpSpread1.Sheets[0].Cells[gr, g].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[gr, g].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[gr, g].Font.Bold = true;
                FpSpread1.Sheets[0].Cells[gr, g].ForeColor = Color.Black;
            }
        }
    }

    protected void rblSubjectOrSubjectType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;

            //divMainContents.Visible = false;
            //CallCheckboxListChange(chkTest, cblTest, txtTest, lblTest.Text, "--Select--");
            //string studentApplicationNo = string.Empty;
            ////BindPreviousSubject();
            btnMarkTypeSettings_Click(sender, e);
        }
        catch (Exception ex)
        {

        }
    }

    #region Settings

    protected void btnMarkTypeSettings_Click(object sender, EventArgs e)
    {
        try
        {
            divSubjectSetting.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;

            divMainContents.Visible = false;

            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string courseId = string.Empty;
            string degreeCode = string.Empty;
            string semester = string.Empty;
            string section = string.Empty;
            string testName = string.Empty;
            string testNo = string.Empty;
            string subjectName = string.Empty;
            string subjectNo = string.Empty;
            string subjectCode = string.Empty;
            string sections = string.Empty;

            string orderBy = string.Empty;
            string orderBySetting = string.Empty;

            string qry = string.Empty;
            string qryCollegeCode = string.Empty;
            string qryCollegeCode1 = string.Empty;
            string qryBatchYear = string.Empty;
            string qryDegreeCode = string.Empty;
            string qrySemester = string.Empty;
            string qrySection = string.Empty;
            string qryCourseId = string.Empty;
            string qrytestNo = string.Empty;
            string qrytestName = string.Empty;
            string qrySubjectNo = string.Empty;
            string qrySubjectName = string.Empty;
            string qrySubjectCode = string.Empty;
            string qryBatchYear1 = string.Empty;
            string qryDegreeCode1 = string.Empty;
            string qrySemester1 = string.Empty;
            string qrySection1 = string.Empty;

            int selectedCount = 0;
            DataTable dtStudentMarks = new DataTable();
            DataTable dtGradeDetails = new DataTable();



            //DataTable dtStudentMarks = new DataTable();

            DataTable dtStudentDetails = new DataTable();

            bool isSubjectType = false;
            if (rblSubjectOrSubjectType.Items.Count > 0 && rblSubjectOrSubjectType.SelectedIndex != 0)
            {
                isSubjectType = true;
            }

            if (ddschool.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblschool.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                collegeCode = Convert.ToString(ddschool.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode1 = " and srh.collegeCode in(" + collegeCode + ")";
                    qryCollegeCode = " and r.college_code in(" + collegeCode + ")";
                }
            }
            if (dropyear.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblyear.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                batchYear = Convert.ToString(dropyear.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and r.Batch_Year in(" + batchYear + ")";
                    qryBatchYear1 = " and srh.BatchYear in(" + batchYear + ")";
                }
            }
            if (ddschooltype.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblschooltype.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                courseId = Convert.ToString(ddschooltype.SelectedValue).Trim();
            }
            if (ddstandard.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblstandard.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                degreeCode = Convert.ToString(ddstandard.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and r.degree_code in(" + degreeCode + ")";
                    qryDegreeCode1 = " and srh.degreeCode in(" + degreeCode + ")";
                }
            }
            if (dropterm.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblterm.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                semester = Convert.ToString(dropterm.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(semester))
                {
                    qrySemester = " and r.current_semester in(" + semester + ")";
                    qrySemester1 = " and srh.semester in(" + semester + ")";
                }
            }
            if (dropsec.Items.Count > 0 && dropsec.Enabled)
            {
                string secValue = Convert.ToString(dropsec.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(secValue) && secValue.Trim().ToLower() != "all" && secValue.Trim().ToLower() != "0" && secValue.Trim().ToLower() != "-1")
                {
                    section = secValue;
                    qrySection = " and LTRIM(RTRIM(ISNULL(e.sections,''))) in('" + secValue + "')";
                    qrySection1 = " and LTRIM(RTRIM(ISNULL(ss.Sections,''))) in('" + secValue + "')";
                }
            }
            //if (ddlTest.Items.Count > 0 && ddlTest.Visible)
            //{
            //    testNo = Convert.ToString(ddlTest.SelectedValue).Trim();
            //    testName = Convert.ToString(ddlTest.SelectedItem.Text).Trim();
            //    if (!string.IsNullOrEmpty(testNo))
            //    {
            //        qrytestNo = " and c.Criteria_no in(" + testNo + ")";
            //    }
            //    else
            //    {
            //        lblAlertMsg.Text = "Please Select " + lblTest.Text.Trim() + " And Then Proceed";
            //        divPopAlert.Visible = true;
            //        return;
            //    }
            //}
            //else if (cblTest.Items.Count > 0 && txtTest.Visible && txtTest.Enabled)
            //{
            //    testNo = getCblSelectedValue(cblTest);
            //    testName = getCblSelectedText(cblTest);
            //    if (!string.IsNullOrEmpty(testNo))
            //    {
            //        qrytestNo = " and c.Criteria_no in(" + testNo + ")";
            //    }
            //    else
            //    {
            //        lblAlertMsg.Text = "Please Select " + lblTest.Text.Trim() + " And Then Proceed";
            //        divPopAlert.Visible = true;
            //        return;
            //    }                
            //}
            //else
            //{
            //    lblAlertMsg.Text = "No " + lblTest.Text.Trim() + " Were Found";
            //    divPopAlert.Visible = true;
            //    return;
            //}
            DataTable dtSubjects = new DataTable();
            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester))
            {
                qry = "select distinct sm.Batch_Year,sm.degree_code,sm.semester,ss.subject_type,ss.subType_no,s.subject_name,s.subject_code,s.acronym,ISNULL(s.subjectpriority,'0') as subjectpriority,ISNULL( ss.isSingleSubject,'0') as isSingleSubject,ISNULL(s.subjectMarkType,'1') as subjectMarkOrGrade,s.subject_no from subject s,syllabus_master sm,sub_sem ss where sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and s.syll_code=ss.syll_code and s.subType_no=ss.subType_no and sm.degree_code='" + degreeCode + "' and sm.Batch_Year='" + batchYear + "' and sm.semester<='" + semester + "' order by sm.Batch_Year,sm.degree_code,sm.semester,subjectpriority,ss.subType_no,s.subject_code;";

                if (isSubjectType)
                    qry = "select distinct sm.Batch_Year,sm.degree_code,sm.semester,ss.subject_type,ss.subType_no,'' subject_name,'' subject_code,'' acronym,'0' as subjectpriority,ISNULL(ss.isSingleSubject,'0') as isSingleSubject,'1' as subjectMarkOrGrade,'' subject_no from subject s,syllabus_master sm,sub_sem ss where sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and s.syll_code=ss.syll_code and s.subType_no=ss.subType_no and sm.degree_code='" + degreeCode + "' and sm.Batch_Year='" + batchYear + "' and sm.semester<='" + semester + "' order by sm.Batch_Year,sm.degree_code,sm.semester,subjectpriority,ss.subType_no,s.subject_code";
                dtSubjects = dirAcc.selectDataTable(qry);
            }
            if (dtSubjects.Rows.Count > 0)
            {
                Init_Spread(FpSubjectList, 1);
                Farpoint.CheckBoxCellType chkall = new Farpoint.CheckBoxCellType();
                chkall.AutoPostBack = true;
                FpSubjectList.Sheets[0].RowCount = 1;
                for (int col = 5; col < FpSubjectList.Sheets[0].ColumnCount; col++)
                {
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, col].CellType = chkall;
                }
                int serialNo = 0;
                foreach (DataRow drSubject in dtSubjects.Rows)
                {
                    serialNo++;
                    FpSubjectList.Sheets[0].RowCount++;
                    string subjectCodeNew = Convert.ToString(drSubject["subject_code"]).Trim();
                    string subjectNameNew = Convert.ToString(drSubject["subject_name"]).Trim();
                    string subjectTypeNew = Convert.ToString(drSubject["subject_type"]).Trim();
                    string subjectTypeNoNew = Convert.ToString(drSubject["subType_no"]).Trim();
                    string subjectNoNew = Convert.ToString(drSubject["subject_no"]).Trim();
                    string subjectSemester = Convert.ToString(drSubject["semester"]).Trim();
                    string subjectMarkOrGrade = Convert.ToString(drSubject["subjectMarkOrGrade"]).Trim();
                    string isSingleSubject = Convert.ToString(drSubject["isSingleSubject"]).Trim();

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNo);
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(subjectSemester);
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(subjectTypeNew);
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(subjectTypeNoNew);
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(subjectCodeNew);
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(subjectNoNew);
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(subjectNameNew);
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(subjectNoNew);
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 5].CellType = chkall;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 6].CellType = chkall;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 5].Value = 0;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 6].Value = 0;

                    FpSubjectList.Sheets[0].Columns[0].Visible = true;
                    FpSubjectList.Sheets[0].Columns[1].Visible = true;
                    FpSubjectList.Sheets[0].Columns[2].Visible = true;
                    FpSubjectList.Sheets[0].Columns[3].Visible = true;
                    FpSubjectList.Sheets[0].Columns[4].Visible = true;
                    FpSubjectList.Sheets[0].Columns[5].Visible = true;
                    FpSubjectList.Sheets[0].Columns[6].Visible = true;
                    if (isSubjectType)
                    {
                        FpSubjectList.Sheets[0].Columns[0].Visible = true;
                        FpSubjectList.Sheets[0].Columns[1].Visible = true;
                        FpSubjectList.Sheets[0].Columns[2].Visible = true;
                        FpSubjectList.Sheets[0].Columns[3].Visible = false;
                        FpSubjectList.Sheets[0].Columns[4].Visible = false;
                        FpSubjectList.Sheets[0].Columns[5].Visible = true;
                        FpSubjectList.Sheets[0].Columns[6].Visible = false;
                        if (isSingleSubject.Trim() == "1" || isSingleSubject.Trim().ToLower() == "true")
                        {
                            FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 5].Value = 1;
                        }
                        else
                        {
                            FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 5].Value = 0;
                        }
                    }
                    else
                    {
                        if (subjectMarkOrGrade.Trim() == "1")
                        {
                            FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 5].Value = 1;
                        }
                        else if (subjectMarkOrGrade.Trim() == "2")
                        {
                            FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 6].Value = 1;
                        }
                        else
                        {
                            FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 5].Value = 1;
                        }
                    }

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSubjectList.Sheets[0].Cells[FpSubjectList.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;

                }
                divSubjectSetting.Visible = true;
                FpSubjectList.Visible = true;
                FpSubjectList.Sheets[0].PageSize = FpSubjectList.Sheets[0].RowCount;
                FpSubjectList.Height = 350;
                FpSubjectList.Width = 800;
                FpSubjectList.SaveChanges();
            }
            else
            {
                lblAlertMsg.Text = "No Subject(s) Found";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool isSaved = false;
            bool isSubjectType = false;
            if (rblSubjectOrSubjectType.Items.Count > 0 && rblSubjectOrSubjectType.SelectedIndex != 0)
            {
                isSubjectType = true;
            }
            if (FpSubjectList.Sheets[0].RowCount > 1)
            {
                for (int row = 1; row < FpSubjectList.Sheets[0].RowCount; row++)
                {
                    string subjectNo = Convert.ToString(FpSubjectList.Sheets[0].Cells[row, 4].Tag).Trim();
                    string subjectTypeNo = Convert.ToString(FpSubjectList.Sheets[0].Cells[row, 2].Tag).Trim();
                    string markType = string.Empty;
                    for (int col = 5; col < FpSubjectList.Sheets[0].ColumnCount; col++)
                    {
                        string typeVal = Convert.ToString(FpSubjectList.Sheets[0].Cells[row, col].Value).Trim();
                        if (isSubjectType)
                        {
                            markType = typeVal;
                            break;
                        }
                        else
                        {
                            if (typeVal == "1")
                            {
                                if (col == 5)
                                    markType = "1";
                                else if (col == 6)
                                    markType = "2";
                                break;
                            }
                        }
                    }
                    if (isSubjectType)
                    {
                        if (!string.IsNullOrEmpty(subjectTypeNo))
                        {
                            qry = "update sub_sem set isSingleSubject='" + markType + "' where subType_no='" + subjectTypeNo + "'";
                            int upd = dirAcc.updateData(qry);
                            if (upd != 0)
                                isSaved = true;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(subjectNo))
                        {
                            qry = "update subject set subjectMarkType='" + markType + "' where subject_no='" + subjectNo + "'";
                            int upd = dirAcc.updateData(qry);
                            if (upd != 0)
                                isSaved = true;
                        }
                    }
                }
            }
            else
            {
                lblAlertMsg.Text = "No Record(s) Were Found";
                divPopAlert.Visible = true;
                return;
            }
            lblAlertMsg.Text = (isSaved) ? "Saved Successfully" : "Not Saved";
            divPopAlert.Visible = true;
            return;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        try
        {
            divSubjectSetting.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void FpSubjectList_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            int r = FpSubjectList.Sheets[0].ActiveRow;
            int j = FpSubjectList.Sheets[0].ActiveColumn;
            int k = Convert.ToInt32(j);

            int a = Convert.ToInt32(r);
            int b = Convert.ToInt32(j);
            bool isSubjectType = false;
            if (rblSubjectOrSubjectType.Items.Count > 0 && rblSubjectOrSubjectType.SelectedIndex != 0)
            {
                isSubjectType = true;
            }
            if (r >= 0 && FpSubjectList.Sheets[0].ColumnHeader.Cells[0, j].Text.Trim().ToLower() != "select" && !isSubjectType)
            {
                if (Convert.ToInt32(r) == 0)
                {
                    if (r.ToString().Trim() != "" && j.ToString().Trim() != "")
                    {
                        if (FpSubjectList.Sheets[0].RowCount > 0)
                        {
                            int checkval = Convert.ToInt32(FpSubjectList.Sheets[0].Cells[a, b].Value);
                            if (checkval == 0)
                            {
                                string headervalue = Convert.ToString(FpSubjectList.Sheets[0].ColumnHeader.Cells[0, b].Tag);
                                for (int i = 1; i < FpSubjectList.Sheets[0].RowCount; i++)
                                {
                                    for (int col = 5; col < FpSubjectList.Sheets[0].ColumnCount; col++)
                                    {
                                        if (col != b)
                                        {
                                            FpSubjectList.Sheets[0].Cells[i, col].Value = 0;
                                            FpSubjectList.Sheets[0].Cells[0, col].Value = 0;
                                        }
                                        else
                                        {
                                            FpSubjectList.Sheets[0].Cells[i, col].Value = 1;
                                            FpSubjectList.Sheets[0].Cells[0, col].Value = 1;
                                        }
                                    }

                                }
                            }
                            else if (checkval == 1)
                            {
                                for (int i = 1; i < FpSubjectList.Sheets[0].RowCount; i++)
                                {
                                    FpSubjectList.Sheets[0].Cells[i, b].Value = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    string headervalue = Convert.ToString(FpSubjectList.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt32(j)].Tag);

                    for (int col = 5; col < FpSubjectList.Sheets[0].ColumnCount; col++)
                    {
                        if (col != j)
                        {
                            FpSubjectList.Sheets[0].Cells[a, col].Value = 0;
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }

    #endregion

}