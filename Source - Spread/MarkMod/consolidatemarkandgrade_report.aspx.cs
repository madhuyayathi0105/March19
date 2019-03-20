using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Configuration;
public partial class consolidatemarkandgrade_report : System.Web.UI.Page
{

    Hashtable hat = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    ArrayList termselected = new ArrayList();
    ArrayList avoidrows = new ArrayList();
    ArrayList avg_grade_col = new ArrayList();
    Boolean teamlast = false;

    DataSet ds_subject = new DataSet();
    DataSet otherds_subject = new DataSet();
    string otherssubjectcode = "";
    string otherssubjectcode01 = "";

    int subjectscount = 0;
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    string strquery = "";
    string regularflag = "";
    string genderflag = "";
    string strdayflag = "";
    string sql = "";
    string sqlcondition = "";
    string collcode = "";
    string batchyear = "";
    string degreecode = "";
    string term = "";
    string sec = "";
    string rollnos = "";
    string currentsem = "";
    DataTable dtallcol = new DataTable();
    DataTable dtallotherscol = new DataTable();


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
    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = "";

    string startdate = "";
    string enddate = "";
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

    string lbltot_att1 = "";
    string lbltot_work1 = "";
    string lbltot_att2 = "";
    string lbltot_work2 = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
                bindschool();
                bindyear();
                bindschooltype();
                bindstandard();
                bindterm();
                bindsec();
                //bindsubject();
                FpSpread1.Visible = false;


                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].RowCount = 1;
                FpSpread1.Sheets[0].ColumnCount = 4;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 40;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";

                FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 50;
                FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 120;

                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;


                for (int i = 0; i < 4; i++)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
                }

                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = " Admn.  No.";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Term";

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
            }
        }
        catch (Exception ex) { }
    }

    public void bindschool()
    {
        try
        {
            string columnfield = "";
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
            ds.Clear();
            ds = d2.select_method("bind_college", hat, "sp");
            ddschool.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddschool.DataSource = ds;
                ddschool.DataTextField = "collname";
                ddschool.DataValueField = "college_code";
                ddschool.DataBind();
            }
            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }


    public void bindyear()
    {
        try
        {
            dropyear.Items.Clear();
            ds.Clear();
            ds = d2.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                dropyear.DataSource = ds;
                dropyear.DataTextField = "batch_year";
                dropyear.DataValueField = "batch_year";
                dropyear.DataBind();
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                dropyear.SelectedValue = max_bat.ToString();
            }
            dropyear.Text = "batch (" + 1 + ")";
            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
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
            ds.Clear();
            ds = d2.select_method("bind_degree", hat, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddschooltype.DataSource = ds;
                ddschooltype.DataTextField = "course_name";
                ddschooltype.DataValueField = "course_id";
                ddschooltype.DataBind();
            }
            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }

    public void bindstandard()
    {
        try
        {
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
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddschooltype.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds.Clear();
            ds = d2.select_method("bind_branch", hat, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddstandard.DataSource = ds;
                ddstandard.DataTextField = "dept_name";
                ddstandard.DataValueField = "degree_code";
                ddstandard.DataBind();
            }

            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }

    public void bindterm()
    {
        cblterm.Items.Clear();
        DataSet studgradeds = new DataSet();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        string strstandard = "";

        if (ddstandard.SelectedValue != "")
        {
            strstandard = ddstandard.SelectedValue;
        }

        if (strstandard.Trim() != "")
        {
            strstandard = " and degree_code in(" + strstandard + ")";
        }

        string strquery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + ddschool.SelectedValue.ToString() + " and batch_year=" + dropyear.Text.ToString() + " and degree_code=" + ddstandard.Text.ToString() + " order by NDurations desc";
        studgradeds.Reset();
        studgradeds.Dispose();
        //  studgradeds = d2.select_method_wo_parameter(strquery, "Text");
        studgradeds = d2.BindSem(ddstandard.Text.ToString(), dropyear.Text.ToString(), ddschool.SelectedValue.ToString());
        if (studgradeds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(studgradeds.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(studgradeds.Tables[0].Rows[0][0].ToString());
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    cblterm.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    cblterm.Items.Add(i.ToString());
                }
            }

            if (cblterm.Items.Count > 0)
            {
                bindsec();
                int cout = 0;
                for (int iq = 0; iq < cblterm.Items.Count; iq++)
                {
                    cout++;
                    cblterm.Items[iq].Selected = true;
                }
                cbterm.Checked = true;
                txtterm.Text = "Term (" + cout + ")";
            }
            else
            {
                cbterm.Checked = false;
                txtterm.Text = "-Select-";
            }
        }
    }


    public void bindsec()
    {
        try
        {

            //dropsec.Enabled = false;
            //dropsec.Items.Clear();
            hat.Clear();
            ds.Clear();
            ds = d2.BindSectionDetail(dropyear.SelectedValue, ddstandard.SelectedValue);

            if (ds.Tables[0].Rows.Count > 0)
            {
                cblsec.Items.Clear();
                cblsec.DataSource = ds;
                cblsec.DataTextField = "sections";
                cblsec.DataValueField = "sections";
                cblsec.DataBind();
            }
            else
            {
                txtsec.Text = "-Select-";
                cbsec.Checked = false;
                //cblsec.Items.Clear();
            }

            if (cblsec.Items.Count > 0)
            {
                int cout = 0;
                for (int iq = 0; iq < cblsec.Items.Count; iq++)
                {
                    cout++;
                    cblsec.Items[iq].Selected = true;
                }
                cbsec.Checked = true;
                txtsec.Text = "Sec (" + cout + ")";
            }
            else
            {
                cbsec.Checked = false;
                txtsec.Text = "-Select-";
            }

            //int count5 = ds.Tables[0].Rows.Count;
            //if (count5 > 0)
            //{
            //    dropsec.DataSource = ds;
            //    dropsec.DataTextField = "sections";
            //    dropsec.DataValueField = "sections";
            //    dropsec.DataBind();
            //    dropsec.Enabled = true;
            //    dropsec.Items.Insert(0, "ALL");
            //}

            //else
            //{
            //    dropsec.Enabled = false;
            //}
            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }

    protected void cbsec_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbsec.Checked == true)
            {
                int cout = 0;
                for (int i = 0; i < cblsec.Items.Count; i++)
                {
                    cout++;
                    cblsec.Items[i].Selected = true;
                    cbsec.Checked = true;
                    txtsec.Text = "Sec (" + cout + ")";
                }
            }
            else
            {
                int cout = 0;
                for (int i = 0; i < cblsec.Items.Count; i++)
                {
                    cout++;
                    cblsec.Items[i].Selected = false;
                    txtsec.Text = "-Select-";
                    cbsec.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }

    protected void cblsec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            cbsec.Checked = false;
            txtsec.Text = "-Select-";
            for (int i = 0; i < cblsec.Items.Count; i++)
            {
                if (cblsec.Items[i].Selected == true)
                {
                    cout++;
                }
            }
            if (cout > 0)
            {
                txtsec.Text = "Sec (" + cout + ")";
                if (cout == cblsec.Items.Count)
                {
                    cbsec.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
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


            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
            lblerrormsg.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
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


            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
            lblerrormsg.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }
    protected void dropschooltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindstandard();
            bindterm();
            bindsec();


            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
            lblerrormsg.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }
    protected void ddstandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindterm();
            bindsec();


            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
            lblerrormsg.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }
    //protected void dropterm_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        bindsec();

    //        FpSpread1.Visible = false;
    //        lblerrormsg.Visible = false;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblerrormsg.Text = ex.ToString();
    //        lblerrormsg.Visible = true;
    //    }
    //}

    protected void cbterm_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbterm.Checked == true)
            {
                int cout = 0;
                for (int i = 0; i < cblterm.Items.Count; i++)
                {
                    cout++;
                    cblterm.Items[i].Selected = true;

                }
                cbterm.Checked = true;
                txtterm.Text = "Term (" + cout + ")";
            }
            else
            {
                int cout = 0;
                for (int i = 0; i < cblterm.Items.Count; i++)
                {
                    cout++;
                    cblterm.Items[i].Selected = false;

                }
                cbterm.Checked = false;
                txtterm.Text = "-Select-";
            }

            if (cblterm.Items.Count > 0)
            {
                bindsec();
            }

        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }

    protected void cblterm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            cbterm.Checked = false;
            txtterm.Text = "-Select-";
            for (int i = 0; i < cblterm.Items.Count; i++)
            {
                if (cblterm.Items[i].Selected == true)
                {
                    cout++;
                }
            }
            if (cout > 0)
            {
                bindsec();
                txtterm.Text = "Term (" + cout + ")";
                if (cout == cblterm.Items.Count)
                {
                    cbterm.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }

    protected void dropsec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
            lblerrormsg.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }




    public string loadmarkat(string mr)
    {
        string strgetval = "";
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


    protected void btnexcel_OnClick(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcell.Text;

            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lblerror.Visible = false;
            }
            else
            {
                lblerror.Text = "Please Enter Your Report Name";
                lblerror.Visible = true;
            }
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }

    protected void btnprint_OnClick(object sender, EventArgs e)
    {
        try
        {
            //string degreedetails = "PERFORMANCE COMPARISON REPORT" + '@' + "                                                                                                  " + "BATCHWISE PERFORMANCE COMPARISON" + '@';

            ///string degreedetails = "  " + '@' + "                       Subject Name: " + ddlsubject.SelectedItem.Text + "                                                        " + "Standard: " + ddstandard.SelectedItem.Text + "                                              " + "Year: " + dropyear.SelectedItem.Text + '@';
            // string degreedetails = "";
            string date_filt = "Class : " + ddstandard.SelectedItem.Text.ToString() + "      " + "Section : " + sec;
            string selterm = "";
            for (int i = 0; i < cblterm.Items.Count; i++)
            {
                if (cblterm.Items[i].Selected == true)
                {
                    termselected.Add(cblterm.Items[i].Text);
                    if (selterm == "")
                    {
                        selterm = cblterm.Items[i].Text;
                    }
                    else
                    {
                        selterm = selterm + ", " + cblterm.Items[i].Text;
                    }
                }
            }
            string test = "Term :" + selterm;

            string degreedetails = string.Empty;

            degreedetails = "Scholastic Areas" + "@" + date_filt + "@" + test;
            string pagename = "pcreport.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {

        try
        {

            collcode = " and r.college_code='" + ddschool.SelectedItem.Value.ToString() + "'";
            batchyear = " and r.Batch_Year='" + dropyear.SelectedItem.Text.ToString() + "'";
            degreecode = " and r.degree_code='" + ddstandard.SelectedItem.Value.ToString() + "'";
            // term = "and sc.semester='" + dropterm.SelectedItem.Text.ToString() + "'";     
            FpSpread1.Sheets[0].ColumnCount = 4;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            bool serialflag = false;
            string strorderby = d2.GetFunction("select LinkValue from inssettings where college_code=" + Convert.ToString(ddschool.SelectedItem.Value) + " and linkname='Student Attendance'");

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

            string selterm = "";
            for (int i = 0; i < cblterm.Items.Count; i++)
            {
                if (cblterm.Items[i].Selected == true)
                {
                    termselected.Add(cblterm.Items[i].Text);
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
                term = " and semester in ('" + selterm + "')";
            }
            if (cblsec.Items.Count > 0)
            {
                //sec = dropsec.SelectedItem.Text.Trim();

                // --------------- add start

                for (int sc = 0; sc < cblsec.Items.Count; sc++)
                {
                    if (cblsec.Items[sc].Selected == true)
                    {
                        if (sec == "")
                        {
                            sec = cblsec.Items[sc].Text.ToString();
                        }
                        else
                        {
                            sec = sec + "','" + cblsec.Items[sc].Text.ToString();
                        }


                    }
                }



            }
            if (sec != "")
            {
                sec = "and r.Sections in ('" + sec + "')";
            }
            else
            {
                sec = "";
            }
            for (int i = 0; i < 3; i++)
            {
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, first].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, i, 3, 1);

            }
            sqlcondition = collcode + batchyear + degreecode + sec;
            if (serialflag == false)
            {
                sql = "SELECT distinct r.Roll_No,R.Stud_Name,a.sex,r.Roll_Admit FROM Registration R,Applyn A WHERE R.App_No = A.App_No     " + sqlcondition + " and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' " + strorderby;
            }
            else
            {
                sql = "SELECT distinct r.serialno,r.Roll_No,R.Stud_Name,a.sex,r.Roll_Admit FROM Registration R,Applyn A WHERE R.App_No = A.App_No     " + sqlcondition + " and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' order by r.serialno";
            }
            avoidrows.Clear();
            studgradeds.Clear();
            studgradeds = d2.select_method_wo_parameter(sql, "Text");
            if (studgradeds.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Sheets[0].RowCount = 0;
                // FpSpread1.Sheets[0].Rows.Count = studgradeds.Tables[0].Rows.Count;
                for (int i = 0; i < studgradeds.Tables[0].Rows.Count; i++)
                {
                    for (int j = 0; j < termselected.Count; j++)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].CellType = txtceltype;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(studgradeds.Tables[0].Rows[i]["Roll_Admit"].ToString());
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Tag = Convert.ToString(studgradeds.Tables[0].Rows[i]["Roll_No"].ToString()); ;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Text = studgradeds.Tables[0].Rows[i]["Stud_Name"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Text = termselected[j].ToString();

                    }

                    FpSpread1.Sheets[0].Rows.Count++;
                    avoidrows.Add(FpSpread1.Sheets[0].Rows.Count - 1);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Text = "Avg. & Grade";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - (termselected.Count + 1), 0, termselected.Count + 1, 1);
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - (termselected.Count + 1), 1, termselected.Count + 1, 1);
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - (termselected.Count + 1), 2, termselected.Count + 1, 1);



                }

                bindheader();



                for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    //final.Visible = true;
                    FpSpread1.Sheets[0].Rows[i].BackColor = ColorTranslator.FromHtml("#E6e6e6");
                    i++;
                }
                FpSpread1.SaveChanges();
                FpSpread1.Visible = true;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;

            }
            else
            {
                lblerrormsg.Text = "No Records Found";
                lblerrormsg.Visible = true;
            }
            Printcontrol.Visible = false;

        }
        catch
        {
        }
    }

    public void bindvaules()
    {

        try
        {

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
            batchyear = dropyear.SelectedItem.Text.ToString();
            degreecode = ddstandard.SelectedItem.Value.ToString();
            //term = dropterm.SelectedItem.Text;
            string selterm = "";
            termselected.Clear();
            for (int i = 0; i < cblterm.Items.Count; i++)
            {
                if (cblterm.Items[i].Selected == true)
                {
                    termscount++;
                    termselected.Add(cblterm.Items[i].Text);
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
            }
            string str_colno = "";
            string str_rolladmit = "";
            string str_criteriano = "";
            string str_subject_no = "";
            string[] split_criteriano;
            double fatotal = 0;
            double satotal = 0;
            double fulltotal = 0;
            double convertedvalue = 0;
            string grademain = "";
            DataSet dsgradechk = new DataSet();
            DataSet ds = new DataSet();
            DataView dv = new DataView();

            int count = dtallcol.Rows.Count;
            if (count > 0)
            {
                show();
                //avg_grade_col.Clear();
                for (int admitno = 0; admitno < FpSpread1.Sheets[0].RowCount; admitno++)
                {

                    if (!avoidrows.Contains(admitno))
                    {
                        string stud_roll = FpSpread1.Sheets[0].Cells[admitno, 1].Tag.ToString();

                        str_rolladmit = FpSpread1.Sheets[0].Cells[admitno, 1].Text.Trim();
                        term = FpSpread1.Sheets[0].Cells[admitno, 3].Text.Trim();
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
                                string admdate = dv[0]["adm_date"].ToString();
                                string Roll_No = dv[0]["Roll_No"].ToString();
                                currentsem = dv[0]["Current_Semester"].ToString(); ;
                                string sem = "select CONVERT(VARCHAR(30),start_date,111) as start_date ,CONVERT(VARCHAR(30),end_date,111) as end_date from seminfo where semester='" + term + "' and degree_code='" + degreecode + "' and batch_year='" + batchyear + "'";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(sem, "Text");

                                if (ds.Tables[0].Rows.Count > 0)
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
                                str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                                //str_criteriano = dtallcol.Rows[i]["Criteria nos"].ToString().Trim();
                                str_subject_no = dtallcol.Rows[i]["subjetno"].ToString().Trim();

                                str_subject_no = d2.GetFunction("select distinct subject_no from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'and semester in ('" + term + "') and  subject_code='" + str_subject_no + "'");
                                //str_criteriano = "SELECT distinct  Istype,CRITERIA_NO,y.semester,M.Conversion_value FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "')  and CRITERIA_NO <>''  order by semester ";

                                fatotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no is null and c.Istype='Calculate 1' and s.subject_no='" + str_subject_no + "'"));
                                convertedvalue = convertedvalue + Convert.ToDouble(d2.GetFunction("SELECT c.conversion FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no is null and c.Istype='Calculate 1' and s.subject_no='" + str_subject_no + "'"));


                                FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fatotal);
                                fulltotal = fatotal;
                            }

                            if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "Grade")
                            {
                                str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                                grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradefs[Convert.ToInt32(term) - 1].ToString() + "' and  " + fatotal + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                                }
                                else
                                {
                                    grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradefs[Convert.ToInt32(term) - 1].ToString() + "' and  " + fatotal + " between Frange and Trange";
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
                                str_subject_no = d2.GetFunction("select distinct subject_no from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batchyear + "'  and degree_code = '" + degreecode + "'and semester in ('" + term + "') and  subject_code='" + str_subject_no + "'");
                                // str_criteriano = "SELECT distinct  Istype,CRITERIA_NO,y.semester,M.Conversion_value FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "')  and CRITERIA_NO <>''  order by semester ";

                                satotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no is null and c.Istype='Calculate 2' and s.subject_no='" + str_subject_no + "'"));
                                convertedvalue = convertedvalue + Convert.ToDouble(d2.GetFunction("SELECT c.conversion FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no is null and c.Istype='Calculate 2' and s.subject_no='" + str_subject_no + "'"));

                                FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(satotal);
                                fulltotal = fulltotal + satotal;

                            }
                            if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "SAGrade")
                            {

                                str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                                grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradesa[Convert.ToInt32(term) - 1].ToString() + "' and  " + satotal + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                                }
                                else
                                {
                                    grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='" + gradesa[Convert.ToInt32(term) - 1].ToString() + "' and  " + satotal + " between Frange and Trange";
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

                                overalltotalall = overalltotalall + fulltotal;
                                str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();

                                FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fulltotal);


                            }

                            if (dtallcol.Rows[i]["Colname"].ToString().Trim() == "TotalGrade")
                            {
                                // str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();
                                str_colno = dtallcol.Rows[i]["colno"].ToString().Trim();

                                if (convertedvalue != 0 && convertedvalue > 0)
                                {

                                    fulltotal = (fulltotal / convertedvalue);
                                    fulltotal = fulltotal * 100;
                                }
                                else
                                {
                                    fulltotal = 0;
                                }

                                grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fulltotal + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, (Convert.ToInt32(str_colno) - 1)].Tag = Convert.ToString(convertedvalue);

                                }
                                else
                                {
                                    grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fulltotal + " between Frange and Trange";
                                    dsgradechk.Clear();
                                    dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                    if (dsgradechk.Tables[0].Rows.Count > 0)
                                    {
                                        FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, (Convert.ToInt32(str_colno) - 1)].Tag = Convert.ToString(convertedvalue);
                                        //  convertedvalues.Add((Convert.ToInt32(str_colno) - 1), convertedvalue);

                                    }
                                }
                                fatotal = 0;
                                satotal = 0;
                                fulltotal = 0;
                                convertedvalue = 0;
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

                                if (dtallotherscol.Rows[i]["Colname"].ToString().Trim() == "others")
                                {
                                    str_colno = dtallotherscol.Rows[i]["colno"].ToString().Trim();
                                    //str_criteriano = dtallcol.Rows[i]["Criteria nos"].ToString().Trim();
                                    str_subject_no = dtallotherscol.Rows[i]["subjetno"].ToString().Trim();

                                    //fatotal = Convert.ToDouble(d2.GetFunction("select top 1  r.marks_obtained from result r,registration reg,exam_type et,subjectchooser sc  where r.exam_code=et.exam_code  and reg.roll_no=r.roll_no and sc.roll_no=reg.roll_no and reg.cc=0 and reg.delflag=0 and reg.exam_flag <>'Debar'  and et.subject_no='" + str_subject_no + "' and et.subject_no=sc.subject_no  and r.roll_no='" + stud_roll + "'  ORDER BY reg.roll_no"));
                                    fatotal = Convert.ToDouble(d2.GetFunction("SELECT c.Exammark FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batchyear + "' and degree_code = '" + degreecode + "' and semester in ('" + term + "') and roll_no='" + stud_roll + "'  and Criteria_no is null and c.Istype='Calculate 1' and s.subject_no='" + str_subject_no + "'"));
                                    //double maximtotal = Convert.ToDouble(d2.GetFunction("select maxtotal from subject where subject_no='" + str_subject_no + "'"));
                                    //fatotal = (fatotal / maximtotal);
                                    //fatotal = fatotal * 100;
                                    fatotal = Math.Round(fatotal, 2);
                                    FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(fatotal);

                                }

                                if (dtallotherscol.Rows[i]["Colname"].ToString().Trim() == "others")
                                {
                                    str_colno = dtallotherscol.Rows[i]["colno"].ToString().Trim();
                                    grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fatotal + " between Frange and Trange";
                                    dsgradechk.Clear();
                                    dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                    if (dsgradechk.Tables[0].Rows.Count > 0)
                                    {
                                        FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                                    }
                                    else
                                    {
                                        grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + fatotal + " between Frange and Trange";
                                        dsgradechk.Clear();
                                        dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                        if (dsgradechk.Tables[0].Rows.Count > 0)
                                        {
                                            FpSpread1.Sheets[0].Cells[admitno, Convert.ToInt32(str_colno)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                                        }
                                    }

                                }
                            }
                        }

                        if ((teamlast == true && dtallotherscol.Rows.Count == 0) || (teamlast == false))
                        {
                            FpSpread1.Sheets[0].Cells[admitno, FpSpread1.Sheets[0].ColumnCount - 6].Text = Convert.ToString(overalltotalall);
                            double totalavg = (overalltotalall / subjectscount);
                            totalavg = Math.Round(totalavg, 2);
                            FpSpread1.Sheets[0].Cells[admitno, FpSpread1.Sheets[0].ColumnCount - 5].Text = Convert.ToString(totalavg);

                            grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + totalavg + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[admitno, FpSpread1.Sheets[0].ColumnCount - 4].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                            }
                            else
                            {
                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + totalavg + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    FpSpread1.Sheets[0].Cells[admitno, FpSpread1.Sheets[0].ColumnCount - 4].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                                }
                            }
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[admitno, FpSpread1.Sheets[0].ColumnCount - 7].Text = Convert.ToString(overalltotalall);
                            double totalavg = (overalltotalall / subjectscount);
                            totalavg = Math.Round(totalavg, 2);
                            FpSpread1.Sheets[0].Cells[admitno, FpSpread1.Sheets[0].ColumnCount - 6].Text = Convert.ToString(totalavg);

                            grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + totalavg + " between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[admitno, FpSpread1.Sheets[0].ColumnCount - 5].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                            }
                            else
                            {
                                grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + totalavg + " between Frange and Trange";
                                dsgradechk.Clear();
                                dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                {
                                    FpSpread1.Sheets[0].Cells[admitno, FpSpread1.Sheets[0].ColumnCount - 5].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                                }
                            }
                        }
                        overalltotalall = 0;
                    }

                }
                //string otherssubject_sql = "select distinct  subject_no,subject_name from subject s,syllabus_master y,sub_sem ss  where s.syll_code = y.syll_code and ss.syll_code=s.syll_code and s.subType_no=ss.subType_no and y.syll_code=ss.syll_code   and subject_type='others' and promote_count=1 ";
                //otherssubject_sql = otherssubject_sql + batchyear + degreecode + term + " order by subject_no,subject_name;";
                //DataSet otherds_subject = new DataSet();
                //otherds_subject.Clear();
                //otherds_subject = d2.select_method_wo_parameter(otherssubject_sql, "Text");
                //if (otherds_subject.Tables[0].Rows.Count > 0)
                //{
                //    for (int admitno = 0; admitno < FpSpread1.Sheets[0].RowCount; admitno++)
                //    {
                //        str_rolladmit = FpSpread1.Sheets[0].Cells[admitno, 1].Tag.ToString();


                //        FpSpread1.Sheets[0].Cells[admitno, FpSpread1.Sheets[0].ColumnCount - 4].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                //    }

                //}
            }
            FpSpread1.SaveChanges();
            // int totalfprowcount = FpSpread1.Sheets[0].Rows.Count;
            double avgtotal = 0;
            int incrementrow = 0;
            double totavgconversn = 0;
            for (int i = 0; i < avoidrows.Count; i++)
            {
                string fprowno = avoidrows[i].ToString();
                for (int j = 0; j < avg_grade_col.Count; j++)
                {
                    string fpcolno = avg_grade_col[j].ToString();
                    incrementrow = Convert.ToInt32(fprowno);
                    incrementrow = incrementrow - termselected.Count;
                    for (int s = 0; s < termselected.Count; s++)
                    {
                        string sdr = FpSpread1.Sheets[0].Cells[incrementrow, Convert.ToInt32(fpcolno)].Text;

                        if (s == 0)
                        {
                            totavgconversn = Convert.ToDouble(FpSpread1.Sheets[0].ColumnHeader.Cells[1, (Convert.ToInt32(fpcolno))].Tag);



                        }

                        // totavgconversn = totavgconversn + Convert.ToDouble(FpSpread1.Sheets[0].ColumnHeader.Cells[2, Convert.ToInt32(fpcolno)].Tag);
                        avgtotal = avgtotal + Convert.ToDouble(FpSpread1.Sheets[0].Cells[incrementrow, Convert.ToInt32(fpcolno)].Text);
                        incrementrow++;
                    }

                    avgtotal = avgtotal / termscount;
                    avgtotal = Math.Round(avgtotal, 2);

                    //string stravgtotal = string.Format("{0.00}", avgtotal);

                    FpSpread1.Sheets[0].Cells[Convert.ToInt32(fprowno), Convert.ToInt32(fpcolno)].Text = Convert.ToString(avgtotal);

                    if (totavgconversn > 0)
                    {
                        avgtotal = (avgtotal / totavgconversn);
                        avgtotal = avgtotal * 100;
                    }
                    else
                    {
                        avgtotal = 0;
                    }
                    grademain = "SELECT * from Grade_Master where Semester='" + term + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + avgtotal + " between Frange and Trange";
                    dsgradechk.Clear();
                    dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                    if (dsgradechk.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.Sheets[0].Cells[Convert.ToInt32(fprowno), Convert.ToInt32(fpcolno) + 1].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                    }
                    else
                    {
                        grademain = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and Criteria ='General' and  " + avgtotal + " between Frange and Trange";
                        dsgradechk.Clear();
                        dsgradechk = d2.select_method_wo_parameter(grademain, "Text");
                        if (dsgradechk.Tables[0].Rows.Count > 0)
                        {
                            FpSpread1.Sheets[0].Cells[Convert.ToInt32(fprowno), Convert.ToInt32(fpcolno) + 1].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());

                        }
                    }
                    FpSpread1.Sheets[0].SpanModel.Add(Convert.ToInt32(fprowno), (Convert.ToInt32(fpcolno) - 4), 1, 4);
                    //FpSpread1.Sheets[0].Cells[Convert.ToInt32(fprowno), Convert.ToInt32(fpcolno)].Text = Convert.ToString(avgtotal);
                    avgtotal = 0;
                }
            }
        }
        catch
        {
        }

    }

    public void bindheader()
    {
        string otherssubject_sql = "";
        int termcount = 0;
        avg_grade_col.Clear();
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
        dtallcol.Columns.Add("Colname");
        dtallcol.Columns.Add("colno");
        dtallcol.Columns.Add("Criteria nos");
        dtallcol.Columns.Add("subjetno");

        dtallotherscol.Columns.Add("Colname");
        dtallotherscol.Columns.Add("colno");
        dtallotherscol.Columns.Add("subjetno");



        string fasaCRITERIA_NO = "";
        double fatotal = 0;
        //double satotal = 0;
        //double fulltotal = 0;
        double maxfatotal = 0;
        double maxsatotal = 0;
        double maxfulltotal = 0;
        // collcode = " and r.college_code='" + ddschool.SelectedItem.Value.ToString() + "'";
        batchyear = "  and y.Batch_Year='" + dropyear.SelectedItem.Text.ToString() + "'";
        degreecode = "  and degree_code='" + ddstandard.SelectedItem.Value.ToString() + "'";
        string selterm = "";
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

        otherssubject_sql = "select distinct  subject_no,subject_name from subject s,syllabus_master y,sub_sem ss  where s.syll_code = y.syll_code and ss.syll_code=s.syll_code and s.subType_no=ss.subType_no and y.syll_code=ss.syll_code   and subject_type='others' and promote_count=1 ";
        otherssubject_sql = otherssubject_sql + batchyear + degreecode + term + " order by subject_no,subject_name;";

        otherds_subject.Clear();
        otherds_subject = d2.select_method_wo_parameter(otherssubject_sql, "Text");


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
            otherssubjectcode = "";
            otherssubjectcode01 = "";
        }

        string subject_sql = "select distinct  subject_code,subject_name from subject s,syllabus_master y,sub_sem ss  where s.syll_code = y.syll_code and ss.syll_code=s.syll_code and s.subType_no=ss.subType_no and y.syll_code=ss.syll_code   and subject_type<>'others' ";
        subject_sql = subject_sql + batchyear + degreecode + selterm + "  order by subject_name;";

        subject_sql = subject_sql + ";" + "SELECT distinct c.Istype,conversion as Conversion_value FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no   " + batchyear + "   " + degreecode + "  " + selterm + " " + otherssubjectcode + "  and CRITERIA_NO is null  and c.Istype<>'settings'";

        subject_sql = subject_sql + ";" + "SELECT distinct c.Istype,conversion as Conversion_value FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no   " + batchyear + "   " + degreecode + "  " + selterm + " " + otherssubjectcode01 + "  and CRITERIA_NO is null  and c.Istype<>'settings'";


        ds_subject.Clear();


        ds_subject = d2.select_method_wo_parameter(subject_sql, "Text");

        if (ds_subject.Tables[0].Rows.Count > 0)
        {
            subjectscount = ds_subject.Tables[0].Rows.Count;
            if (ds_subject.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < ds_subject.Tables[0].Rows.Count; i++)
                {
                    string str_subject_name = ds_subject.Tables[0].Rows[i]["subject_name"].ToString();
                    string str_subject_no = ds_subject.Tables[0].Rows[i]["subject_code"].ToString();

                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = str_subject_name;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;

                    // FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 1, 6);
                    maxfatotal = 0;
                    for (int j = 0; j < 2; j++)
                    {
                        //if (fasaCRITERIA_NO.Trim() == "")
                        //{
                        //    fasaCRITERIA_NO = str_subject_no;
                        //}
                        if (j < 1)
                        {
                            maxfatotal = maxfatotal + Convert.ToDouble(ds_subject.Tables[1].Rows[j]["Conversion_value"].ToString());

                            //fatotal = Convert.ToDouble(ds_subject.Tables[1].Rows[j]["Conversion_value"].ToString());
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "FA";

                            dtallcol.Rows.Add("FA", FpSpread1.Sheets[0].ColumnCount - 1, "", str_subject_no);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                            // FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(maxfatotal);
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
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
                            //fasaCRITERIA_NO = "";
                            //if (fasaCRITERIA_NO.Trim() == "")
                            //{
                            //    fasaCRITERIA_NO = str_subject_no;
                            //}
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "SA";
                            dtallcol.Rows.Add("SA", FpSpread1.Sheets[0].ColumnCount - 1, "", str_subject_no);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(maxsatotal);
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Grade";
                            dtallcol.Rows.Add("SAGrade", FpSpread1.Sheets[0].ColumnCount - 1, fasaCRITERIA_NO, str_subject_no);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                        }
                        maxfulltotal = maxfatotal + maxsatotal;

                    }
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                    avg_grade_col.Add(FpSpread1.Sheets[0].ColumnCount - 1);
                    dtallcol.Rows.Add("Total", FpSpread1.Sheets[0].ColumnCount - 1, " ", str_subject_no);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(maxfulltotal);
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;
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

        FpSpread1.Sheets[0].ColumnCount++;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Overall Total ";
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 3, 1);
        FpSpread1.Sheets[0].ColumnCount++;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total ";
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 3, 1);
        FpSpread1.Sheets[0].ColumnCount++;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Overall Grade";
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 3, 1);

        if (otherds_subject.Tables[0].Rows.Count > 0 && termcount == 3)
        {
            teamlast = true;
            for (int i = 0; i < otherds_subject.Tables[0].Rows.Count; i++)
            {

                string str_subject_name = otherds_subject.Tables[0].Rows[i]["subject_name"].ToString();
                string str_subject_no = otherds_subject.Tables[0].Rows[i]["subject_no"].ToString();

                if (str_subject_name.Trim().ToLower() == "pet")
                {
                    FpSpread1.Sheets[0].ColumnCount++;


                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = str_subject_name;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = str_subject_no;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;

                    dtallotherscol.Rows.Add("others", FpSpread1.Sheets[0].ColumnCount - 1, str_subject_no);


                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 3, 1);
                }
                // FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 48, 1, 2);
                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 48, 2, 1);
            }


        }
        else
        {
            teamlast = false;
        }



        FpSpread1.Sheets[0].ColumnCount++;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Attendance ";

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

            //FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].Font.Size = FontUnit.Medium;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].Font.Name = "Book Antiqua";
            //FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].Font.Bold = true;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].ForeColor = Color.White;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[2, g].VerticalAlign = VerticalAlign.Middle;
        }

        for (int g = 0; g < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; g++)
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
                        if (!holiday_table11.Contains((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                        }

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
                        if (!holiday_table2.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table2.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                        }
                    }
                }

                if (ds3.Tables[2].Rows.Count != 0)
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

                                                value = dvattvalue[0][date].ToString();
                                                //Added by srinath 31/1/2014=========Start
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

    public void hide()
    {
        //lblerror
        lblexportxl.Visible = false;
        txtexcell.Visible = false;
        btnexcel.Visible = false;
        btnprint.Visible = false;
        Printcontrol.Visible = false;
    }
    public void show()
    {
        //lblerror
        lblexportxl.Visible = true;
        txtexcell.Visible = true;
        btnexcel.Visible = true;
        btnprint.Visible = true;
        Printcontrol.Visible = true;
    }

}