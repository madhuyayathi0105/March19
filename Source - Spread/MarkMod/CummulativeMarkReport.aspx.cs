using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

public partial class CummulativeMarkReport : System.Web.UI.Page
{
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";

    string strquery = "";
    int column = 2;
    int col = 3;
    int b = 0;

    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataTable dTab = new DataTable();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataView dv1 = new DataView();
    DataView dv2 = new DataView();
    Hashtable hastble4 = new Hashtable();
    Hashtable columnhash = new Hashtable();
    ArrayList columarray = new ArrayList();
    DataSet dset31 = new DataSet();

    // calculate present precentage
    DataSet ds3 = new DataSet();
    DataSet ds2 = new DataSet();
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
    string isonumber = "";
    int inirow_count = 0;
    // string usercode = "";

    int demfcal, demtcal;
    string monthcal;
    string startdate = "";
    string enddate = "";
    string tempvalue = "-1";
    Boolean yesflag = false;
    string currentsem = "";
    string batchyear = "";
    string degreecode = "";
    string latmode = "";
    string cgpa = "";
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
    string college_code = "";

    int count = 0;

    string stdappno = "";
    string stdphtsql = "";


    Hashtable hatcumonduty = new Hashtable();
    // calculate present precentage


    protected void Page_Load(object sender, EventArgs e)
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

        if (!IsPostBack)
        {
            lblvalidation.Visible = false;
            FpSpread2.Visible = false;
            bindschool(); bindyear();
            bindschooltype();
            bindstandard();
            bindterm();
            binddate();
            bindsec();
            txtfromdate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");

        }
    }

    public void bindsec()
    {
        try
        {

            dropsec.Enabled = false;
            dropsec.Items.Clear();
            hat.Clear();

            ds = d2.BindSectionDetail(dropyear.SelectedValue, ddstandard.SelectedValue);
            int count5 = ds.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                dropsec.DataSource = ds;
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
    public void binddate()
    {
        try
        {
            string query = "select CONVERT(varchar(20), start_date,103) as start_date ,CONVERT(varchar(20), end_date,103) as end_date  from seminfo where degree_code =" + ddstandard.SelectedItem.Value + " and semester =" + dropterm.SelectedItem.Text + " and batch_year= " + dropyear.SelectedItem.Text + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtfromdate.Text = Convert.ToString(ds.Tables[0].Rows[0]["start_date"]);
                txttodate.Text = Convert.ToString(ds.Tables[0].Rows[0]["end_date"]);
                txtfromdate.Enabled = true;
                txttodate.Enabled = true;
            }
            else
            {
                txtfromdate.Text = "";
                txttodate.Text = "";
                txtfromdate.Enabled = false;
                txttodate.Enabled = false;
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
            ds = d2.select_method("bind_college", hat, "sp");
            ddschool.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddschool.DataSource = ds;
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
            ds = d2.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                dropyear.DataSource = ds;
                dropyear.DataTextField = "batch_year";
                dropyear.DataValueField = "batch_year";
                dropyear.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
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
            collegecode = Session["collegecode"].ToString();
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
            ds = d2.select_method("bind_degree", hat, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddschooltype.DataSource = ds;
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
            ds = d2.select_method("bind_branch", hat, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddstandard.DataSource = ds;
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
            dropterm.Items.Clear();
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

            strquery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + ddschool.SelectedValue.ToString() + " and batch_year=" + dropyear.Text.ToString() + " and degree_code=" + ddstandard.Text.ToString() + " order by NDurations desc";
            ds.Reset();
            ds.Dispose();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
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
            else
            {
                strquery = "select distinct duration,first_year_nonsemester  from degree where college_code=" + ddschool.SelectedValue.ToString() + " and degree_code=" + ddstandard.SelectedValue.ToString() + " order by duration desc";
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
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
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddschool_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindyear();
            bindschooltype();
            bindstandard();
            bindterm();
            binddate();
            FpSpread2.Visible = false;
            reportgrid1.Visible = false;

            lblerrormsg.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            lblvalidation.Visible = false;
            lblrptname.Visible = false;
            txt_excel.Visible = false;
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
            binddate();
            FpSpread2.Visible = false;
            reportgrid1.Visible = false;

            lblerrormsg.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            lblvalidation.Visible = false;
            lblrptname.Visible = false;
            txt_excel.Visible = false;
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
            binddate();
            bindsec();
            FpSpread2.Visible = false;
            reportgrid1.Visible = false;

            lblerrormsg.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            lblvalidation.Visible = false;
            lblrptname.Visible = false;
            txt_excel.Visible = false;
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
            binddate();
            bindsec();
            FpSpread2.Visible = false;
            reportgrid1.Visible = false;

            lblerrormsg.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            lblvalidation.Visible = false;
            lblrptname.Visible = false;
            txt_excel.Visible = false;
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
            binddate();
            bindsec();
            reportgrid1.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            lblerrormsg.Visible = false;
            FpSpread2.Visible = false;
            reportgrid1.Visible = false;

            lblerrormsg.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            lblvalidation.Visible = false;
            lblrptname.Visible = false;
            txt_excel.Visible = false;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void reportgrid1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Cells[1].Width = 80;
                //e.Row.Cells[2].Width = 150;
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                for (int j = 4; j >= col; j++)
                {
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void reportgrid1_DataBound(object sender, EventArgs e)
    {
        try
        {
            DataView dv11 = new DataView();
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell = null;
            if (columarray.Count > 0)
            {
                int col = 0;
                HeaderCell = new TableCell();
                HeaderCell.Text = "";
                HeaderCell.ColumnSpan = 4;
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderGridRow.Cells.Add(HeaderCell);
                reportgrid1.Controls[0].Controls.AddAt(0, HeaderGridRow);
                for (int j = 0; j < columarray.Count; j++)
                {
                    string value = Convert.ToString(columnhash[columarray[j]]);
                    if (value.Trim() != "")
                    {
                        col = col + Convert.ToInt32(value);
                    }
                    string querybound = "select distinct subject_name,s.Istype from internal_cam_calculation_master_setting s,subject u,syllabus_master Y ,tbl_Cam_Calculation C,Registration N where s.subject_no = u.subject_no and u.syll_code = y.syll_code and s.subject_no = c.subject_no and s.Istype = c.Istype and c.roll_no = n.roll_no and y.degree_code = " + ddstandard.SelectedValue + " and y.Batch_Year = " + dropyear.SelectedItem.Text + " and semester = " + dropterm.SelectedItem.Text + " Order by subject_name";
                    dset31 = d2.select_method_wo_parameter(querybound, "Text");
                    if (dset31.Tables[0].Rows.Count > 0)
                    {
                        string var = dset31.Tables[0].Rows[0]["Istype"].ToString();

                        string[] array = var.Split(',');
                        if (array.Length > 0)
                        {
                            HeaderCell = new TableCell();
                            HeaderCell.Text = columarray[j].ToString();
                            HeaderCell.ColumnSpan = array.Length + 5;
                            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                            HeaderGridRow.Cells.Add(HeaderCell);
                            reportgrid1.Controls[0].Controls.AddAt(0, HeaderGridRow);
                        }
                    }
                }
                HeaderCell = new TableCell();
                HeaderCell.Text = "";
                HeaderCell.ColumnSpan = 4;
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderGridRow.Cells.Add(HeaderCell);
                reportgrid1.Controls[0].Controls.AddAt(0, HeaderGridRow);
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void counttestvalue(DataTable d)
    {
        try
        {
            if (d.Rows.Count > 0)
            {
                DataView dvcheck = new DataView(d);

                for (int ik = 0; ik < ds1.Tables[0].Rows.Count; ik++)
                {
                    string value = ds1.Tables[0].Rows[ik]["subject_name"].ToString();
                    string gettoolname = ds1.Tables[0].Rows[ik]["subject_no"].ToString();
                    dvcheck.RowFilter = "subject_name='" + value + "'";
                    if (dvcheck.Count > 0)
                    {
                        if (!columarray.Contains(value))
                        {
                            columarray.Add(value);
                            columnhash.Add(value, dvcheck.Count);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds11 = new DataSet();
            DataView dv = new DataView();
            ArrayList addsubno = new ArrayList();
            if (dropterm.SelectedValue != "")
            {
                bool flage = false;
                if (flage == false)
                {
                    string query_value = "";
                    FpSpread2.Visible = true;
                    FpSpread2.Sheets[0].RowCount = 0;
                    FpSpread2.CommandBar.Visible = false;
                    FpSpread2.Sheets[0].RowHeader.Visible = false;
                    FpSpread2.Sheets[0].ColumnCount = 4;
                    FpSpread2.Sheets[0].AutoPostBack = true;
                    FpSpread2.Sheets[0].ColumnHeader.RowCount = 2;

                    FpSpread2.Sheets[0].Columns[0].Visible = true;
                    FpSpread2.Sheets[0].Columns[1].Visible = true;
                    FpSpread2.Sheets[0].Columns[2].Visible = true;
                    FpSpread2.Sheets[0].Columns[3].Visible = true;

                    FpSpread2.Sheets[0].Columns[0].Width = 50;
                    FpSpread2.Sheets[0].Columns[1].Width = 150;
                    FpSpread2.Sheets[0].Columns[2].Width = 150;
                    FpSpread2.Sheets[0].Columns[3].Width = 300;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";


                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register Number";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";


                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";

                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                    int row = 0;
                    string andsection = "";

                    if (dropsec.Enabled == true)
                    {
                        andsection = dropsec.SelectedItem.Text.Trim();
                        if (andsection != "")
                        {
                            andsection = "and n.Sections= '" + dropsec.SelectedItem.Text.Trim() + "'";
                        }
                    }
                    ds1.Clear();
                    query_value = "select distinct subject_name,u.subject_no from internal_cam_calculation_master_setting s,subject u,syllabus_master Y ,tbl_Cam_Calculation C,Registration N where s.subject_no = u.subject_no and u.syll_code = y.syll_code and s.subject_no = c.subject_no and s.Istype = c.Istype and c.roll_no = n.roll_no and y.degree_code = " + ddstandard.SelectedValue + " and y.Batch_Year = " + dropyear.SelectedItem.Text + " and semester = " + dropterm.SelectedItem.Text + " order by subject_name";
                    ds1 = d2.select_method_wo_parameter(query_value, "Text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        query_value = "select distinct  subject_name,u.subject_no,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,idno from internal_cam_calculation_master_setting s,subject u,syllabus_master Y  ,tbl_Cam_Calculation C where s.subject_no = u.subject_no and u.syll_code = y.syll_code  and s.subject_no = c.subject_no and s.Istype = c.Istype  and y.degree_code = " + ddstandard.SelectedValue + " and y.Batch_Year = " + dropyear.SelectedItem.Text + " and semester = " + dropterm.SelectedItem.Text + " order by subject_name, idno";
                        ds11.Clear();
                        ds11 = d2.select_method_wo_parameter(query_value, "Text");
                        if (ds11.Tables[0].Rows.Count > 0)
                        {
                            for (row = 0; row < ds1.Tables[0].Rows.Count; row++)
                            {
                                ds11.Tables[0].DefaultView.RowFilter = "subject_no=" + Convert.ToString(ds1.Tables[0].Rows[row]["subject_no"]) + "";
                                dv = ds11.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                {
                                    int count = 0;
                                    for (int j = 0; j < dv.Count; j++)
                                    {
                                        count += 2;
                                        FpSpread2.Sheets[0].ColumnCount++;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dv[j]["Istype"]);
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 100;
                                        FpSpread2.Sheets[0].ColumnCount++;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString("Grade");
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 100;
                                    }
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - count].Text = Convert.ToString(ds1.Tables[0].Rows[row]["subject_name"]);
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - count].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - count].Font.Bold = true;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - count].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - count].Font.Name = "Book Antiqua";
                                    addsubno.Add(ds1.Tables[0].Rows[row]["subject_no"]);
                                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - count, 1, count);
                                }
                            }
                            FpSpread2.Sheets[0].ColumnCount++;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString("No.of Days Present");
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnCount++;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString("%");
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";

                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].Text = Convert.ToString("Attendance");
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 2, 1, 2);

                            FpSpread2.Sheets[0].ColumnCount++;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString("Remarks");
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 1, 2, 1);

                            int sno = 0;
                            int totalsutdcount = 0;
                            int noofpresent = 0;
                            int noofabsent = 0;
                            int nooffail = 0;
                            ArrayList subcolumncount = new ArrayList();
                            Hashtable arrercount = new Hashtable();
                            query_value = "select c.roll_no,n.Reg_No,n.Stud_Name,Convert(nvarchar(15), n.Adm_Date,103) as Adm_Date,s.subject_no,subject_name,idno ,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype  ,Exammark,conversion from internal_cam_calculation_master_setting s ,subject u,syllabus_master Y ,tbl_Cam_Calculation C,Registration N  where s.subject_no = u.subject_no and u.syll_code = y.syll_code and s.subject_no = c.subject_no and s.Istype = c.Istype and c.roll_no = n.roll_no and y.degree_code = " + ddstandard.SelectedValue + "  and y.Batch_Year = " + dropyear.SelectedItem.Text + " and semester = " + dropterm.SelectedItem.Text + " " + andsection + "";
                            string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                            if (orderby_Setting == "0")
                            {
                                query_value = query_value + " ORDER BY n.Roll_No,subject_name,idno  ";
                            }
                            else if (orderby_Setting == "1")
                            {
                                query_value = query_value + " ORDER BY n.Reg_No,subject_name,idno  ";
                            }
                            else if (orderby_Setting == "2")
                            {
                                query_value = query_value + " ORDER BY n.Stud_Name,subject_name,idno  ";
                            }
                            else if (orderby_Setting == "0,1,2")
                            {
                                query_value = query_value + " ORDER BY n.Roll_No,r.Reg_No,r.Stud_Name,subject_name,idno  ";
                            }
                            else if (orderby_Setting == "0,1")
                            {
                                query_value = query_value + " ORDER BY n.Roll_No,r.Reg_No,subject_name,idno  ";
                            }
                            else if (orderby_Setting == "1,2")
                            {
                                query_value = query_value + " ORDER BY n.Reg_No,r.Stud_Name,subject_name,idno  ";
                            }
                            else if (orderby_Setting == "0,2")
                            {
                                query_value = query_value + " ORDER BY n.Roll_No,r.Stud_Name,subject_name,idno  ";
                            }
                            query_value = query_value + "   select distinct c.roll_no,n.Reg_No from internal_cam_calculation_master_setting s ,subject u,syllabus_master Y ,tbl_Cam_Calculation C,Registration N where s.subject_no = u.subject_no and u.syll_code = y.syll_code and s.subject_no = c.subject_no and s.Istype = c.Istype and c.roll_no = n.roll_no  and y.degree_code = " + ddstandard.SelectedValue + " and y.Batch_Year = " + dropyear.SelectedItem.Text + " and semester = " + dropterm.SelectedItem.Text + " " + andsection + " order by c.roll_no ";
                            DataSet duse = new DataSet();
                            duse.Clear();
                            duse = d2.select_method_wo_parameter(query_value, "Text");
                            if (duse.Tables[1].Rows.Count > 0)
                            {
                                for (row = 0; row < duse.Tables[1].Rows.Count; row++)
                                {
                                    duse.Tables[0].DefaultView.RowFilter = "roll_no='" + Convert.ToString(duse.Tables[1].Rows[row]["roll_no"]) + "'";
                                    dv = duse.Tables[0].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        sno++;
                                        totalsutdcount++;
                                        FpSpread2.Sheets[0].RowCount++;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        //  FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[0]["roll_no"]);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[0]["Reg_No"]);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[0]["Stud_Name"]);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                        string admission_date = Convert.ToString(dv[0]["Adm_Date"]);

                                        int col = 3;
                                        if (addsubno.Count > 0)
                                        {
                                            for (int jk = 0; jk < addsubno.Count; jk++)
                                            {
                                                nooffail = 0;
                                                subcolumncount.Add(col);
                                                dv.RowFilter = "subject_no=" + Convert.ToString(addsubno[jk]) + " and roll_no='" + Convert.ToString(duse.Tables[1].Rows[row]["roll_no"]) + "'";
                                                if (dv.Count > 0)
                                                {
                                                    for (int k = 0; k < dv.Count; k++)
                                                    {
                                                        col++;

                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv[k]["Exammark"]);
                                                        string criteria = FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Text.ToString();
                                                        string[] splitcriteria = criteria.Split('(');
                                                        criteria = splitcriteria[0].Trim();
                                                        string perc = splitcriteria[1].Trim();
                                                        string[] splitperc = perc.Split(')');
                                                        perc = splitperc[0].Trim().ToString();
                                                        string batchgrade = "";
                                                        string degreecodegrade = "";
                                                        batchgrade = dropyear.SelectedItem.Text.ToString();
                                                        degreecodegrade = ddstandard.SelectedItem.Value.ToString();

                                                        double exammmark = Convert.ToDouble(dv[k]["Exammark"]);
                                                        double calculatedmark = (exammmark / Convert.ToDouble(perc)) * 100;

                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;

                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                        col++;
                                                        DataSet dsgrade = new DataSet();
                                                        if (criteria.Trim().ToLower() == "fa1" || criteria.Trim().ToLower() == "fa2" || criteria.Trim().ToLower() == "fa3" || criteria.Trim().ToLower() == "fa4" || criteria.Trim().ToLower() == "sa1" || criteria.Trim().ToLower() == "sa2")
                                                        {
                                                            string grade = "select  Mark_Grade,Credit_Points  from Grade_Master where '" + calculatedmark + "' between frange and trange and  Criteria='" + criteria + "' and batch_year='" + batchgrade + "' and Degree_Code='" + degreecodegrade + "' and Semester='" + dropterm.SelectedItem.Text + "'";

                                                            dsgrade.Clear();
                                                            dsgrade = d2.select_method_wo_parameter(grade, "Text");
                                                            if (dsgrade.Tables[0].Rows.Count == 0)
                                                            {
                                                                grade = "select  Mark_Grade,Credit_Points  from Grade_Master where '" + calculatedmark + "' between frange and trange and  Criteria='" + criteria + "' and batch_year='" + batchgrade + "' and Degree_Code='" + degreecodegrade + "' and Semester='0'";

                                                                dsgrade.Clear();
                                                                dsgrade = d2.select_method_wo_parameter(grade, "Text");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            string grade = "select  Mark_Grade,Credit_Points  from Grade_Master where '" + calculatedmark + "' between frange and trange  and batch_year='" + batchgrade + "' and Degree_Code='" + degreecodegrade + "' and Semester='" + dropterm.SelectedItem.Text + "'  and Criteria = ''";

                                                            dsgrade.Clear();
                                                            dsgrade = d2.select_method_wo_parameter(grade, "Text");
                                                            if (dsgrade.Tables[0].Rows.Count == 0)
                                                            {
                                                                grade = "select  Mark_Grade,Credit_Points  from Grade_Master where '" + calculatedmark + "' between frange and trange  and batch_year='" + batchgrade + "' and Degree_Code='" + degreecodegrade + "' and Semester='0' and Criteria =''";

                                                                dsgrade.Clear();
                                                                dsgrade = d2.select_method_wo_parameter(grade, "Text");
                                                            }


                                                        }
                                                        if (dsgrade.Tables[0].Rows.Count > 0)
                                                        {
                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dsgrade.Tables[0].Rows[0]["Mark_Grade"]);
                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;

                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                        }
                                                        if (k == Convert.ToInt32(dv.Count) - 1)
                                                        {
                                                            if (dsgrade.Tables[0].Rows.Count > 0)
                                                            {
                                                                string credit = Convert.ToString(dsgrade.Tables[0].Rows[0]["Credit_Points"]);
                                                                if (credit.Trim() != "")
                                                                {
                                                                    if (credit == "0")
                                                                    {
                                                                        nooffail++;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                nooffail++;
                                                            }
                                                        }

                                                    }
                                                }
                                                if (!arrercount.Contains(Convert.ToString(addsubno[jk])))
                                                {
                                                    arrercount.Add(Convert.ToString(addsubno[jk]), nooffail);
                                                }
                                                else
                                                {
                                                    int getvalue = Convert.ToInt32(arrercount[Convert.ToString(addsubno[jk])]);
                                                    arrercount.Remove(Convert.ToString(addsubno[jk]));
                                                    getvalue = getvalue + nooffail;
                                                    arrercount.Add(Convert.ToString(addsubno[jk]), getvalue);
                                                }
                                            }
                                        }
                                        if (txtfromdate.Enabled == true && txttodate.Enabled == true)
                                        {
                                            string from_date = Convert.ToString(txtfromdate.Text);
                                            string to_date = Convert.ToString(txttodate.Text);
                                            persentmonthcal(Convert.ToString(duse.Tables[1].Rows[row]["roll_no"]), admission_date, from_date, to_date);
                                            col++;

                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(present);
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;

                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";

                                            col++;
                                            double percentage = 0;
                                            if (working.Trim() != "" && present.Trim() != "" && working != "0" && present != "0")
                                            {

                                                percentage = Convert.ToDouble(present) / Convert.ToDouble(working) * 100;
                                            }
                                            percentage = Math.Round(percentage, 2);
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(percentage);
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;

                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                        }

                                    }
                                }
                                // FpSpread2.Sheets[0].RowCount++;
                                if (subcolumncount.Count > 0)
                                {
                                    FpSpread2.Sheets[0].RowCount++;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = "No. of Roll";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    for (int total = 0; total < subcolumncount.Count; total++)
                                    {
                                        string addvlaue = Convert.ToString(subcolumncount[total]);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, Convert.ToInt32(addvlaue) + 1].Text = Convert.ToString(totalsutdcount);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, Convert.ToInt32(addvlaue) + 1].HorizontalAlign = HorizontalAlign.Center;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, Convert.ToInt32(addvlaue) + 1].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, Convert.ToInt32(addvlaue) + 1].Font.Name = "Book Antiqua";
                                    }
                                    FpSpread2.Sheets[0].RowCount++;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = "No. Present";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                    FpSpread2.Sheets[0].RowCount++;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = "No. Absent";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";


                                    FpSpread2.Sheets[0].RowCount++;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = "No. Failed";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    for (int total = 0; total < addsubno.Count; total++)
                                    {
                                        string addvlaue = Convert.ToString(subcolumncount[total]);
                                        string subjectno = Convert.ToString(addsubno[total]);
                                        int failcount = Convert.ToInt32(arrercount[Convert.ToString(subjectno)]);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, Convert.ToInt32(addvlaue) + 1].Text = Convert.ToString(failcount);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, Convert.ToInt32(addvlaue) + 1].HorizontalAlign = HorizontalAlign.Center;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, Convert.ToInt32(addvlaue) + 1].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, Convert.ToInt32(addvlaue) + 1].Font.Name = "Book Antiqua";
                                    }

                                    FpSpread2.Sheets[0].RowCount++;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = "Pass Percentage";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";


                                    for (int total = 0; total < addsubno.Count; total++)
                                    {
                                        string addvlaue = Convert.ToString(subcolumncount[total]);
                                        string subjectno = Convert.ToString(addsubno[total]);
                                        int failcount = Convert.ToInt32(arrercount[Convert.ToString(subjectno)]);
                                        double passcount = totalsutdcount - failcount;
                                        double percentage = passcount / Convert.ToDouble(totalsutdcount) * 100;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, Convert.ToInt32(addvlaue) + 1].Text = Convert.ToString(percentage + "%");
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, Convert.ToInt32(addvlaue) + 1].HorizontalAlign = HorizontalAlign.Center;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, Convert.ToInt32(addvlaue) + 1].Font.Size = FontUnit.Medium;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, Convert.ToInt32(addvlaue) + 1].Font.Name = "Book Antiqua";
                                    }


                                }

                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = "Signature of the Class teacher";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";


                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = "Signature of the A H M";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = "Signature of the Principal";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                string group_code = Session["group_code"].ToString();
                                string columnfield = "";
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
                                    columnfield = " and usercode='" + Session["usercode"] + "'";
                                }

                                string roll_no = d2.GetFunction("select value from Master_Settings where settings ='Roll No' " + columnfield + "");
                                if (roll_no == "0")
                                {
                                    if (FpSpread2.Sheets[0].Rows.Count > 0)
                                    {
                                        FpSpread2.Sheets[0].Columns[1].Visible = false;
                                    }
                                }
                                string registerno = d2.GetFunction("select value from Master_Settings where settings ='Register No' " + columnfield + "");
                                if (registerno == "0")
                                {
                                    if (FpSpread2.Sheets[0].Rows.Count > 0)
                                    {
                                        FpSpread2.Sheets[0].Columns[2].Visible = false;
                                    }
                                }


                                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                                int width = 0;
                                if (addsubno.Count >= 5)
                                {
                                    width = 2500;
                                }
                                else if (addsubno.Count < 5 && addsubno.Count > 3)
                                {
                                    width = 1500;
                                }
                                else
                                {
                                    width = 1000;
                                }

                                FpSpread2.Width = width;
                                FpSpread2.Visible = true;
                                FpSpread2.Sheets[0].Visible = true;
                                reportgrid1.Visible = true;
                                lblerrormsg.Visible = false;
                                g1btnexcel.Visible = true;
                                g1btnprint.Visible = true;
                                lblvalidation.Visible = false;
                                lblrptname.Visible = true;
                                txt_excel.Visible = true;
                            }
                            else
                            {
                                FpSpread2.Visible = false;
                                reportgrid1.Visible = false;
                                lblerrormsg.Text = "No Records Found";
                                lblerrormsg.Visible = true;
                                g1btnexcel.Visible = false;
                                g1btnprint.Visible = false;
                                lblvalidation.Visible = false;
                                lblrptname.Visible = false;
                                txt_excel.Visible = false;
                            }
                        }

                    }
                    else
                    {
                        FpSpread2.Visible = false;
                        reportgrid1.Visible = false;
                        lblerrormsg.Text = "No Records Found";
                        lblerrormsg.Visible = true;
                        g1btnexcel.Visible = false;
                        g1btnprint.Visible = false;
                        lblvalidation.Visible = false;
                        lblrptname.Visible = false;
                        txt_excel.Visible = false;
                    }
                }
                else
                {
                    int count = 1;
                    string query35A = "";
                    int column1 = 2;

                    DataRow dr = null;
                    ArrayList checkarray = new ArrayList();
                    DataTable dtble = new DataTable();
                    Hashtable hastble1 = new Hashtable();
                    Hashtable hastble3 = new Hashtable();
                    Hashtable hastblm = new Hashtable();


                    dTab.Columns.Add("S.No", typeof(Int32));
                    dTab.Columns.Add("Roll No", typeof(string));
                    dTab.Columns.Add("Reg No", typeof(string));
                    dTab.Columns.Add("Student Name", typeof(string));

                    string query1 = "";
                    query1 = "select c.roll_no,n.Reg_No,n.Stud_Name,s.subject_no,subject_name,Convert(nvarchar(15),s.Istype)+' ('+Convert(nvarchar(15),conversion)+')' as Istype,Exammark,conversion from internal_cam_calculation_master_setting s,subject u,syllabus_master Y ,tbl_Cam_Calculation C,Registration N where s.subject_no = u.subject_no and u.syll_code = y.syll_code and s.subject_no = c.subject_no and s.Istype = c.Istype and c.roll_no = n.roll_no and y.degree_code = " + ddstandard.SelectedValue + " and y.Batch_Year = " + dropyear.SelectedItem.Text + " and semester = " + dropterm.SelectedItem.Text + " order by c.roll_no ,subject_name,s.Istype";
                    ds1 = d2.select_method_wo_parameter(query1, "Text");
                    DataSet dsbest = new DataSet();
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        for (int ik = 0; ik < ds1.Tables[0].Rows.Count; ik++)
                        {
                            if (!hastble1.ContainsKey(ds1.Tables[0].Rows[ik]["roll_no"].ToString()))
                            {
                                string sub = ds1.Tables[0].Rows[ik]["roll_no"].ToString();
                                ds1.Tables[0].DefaultView.RowFilter = "roll_no='" + sub + "'";
                                dv1 = ds1.Tables[0].DefaultView;
                                if (dv1.Count > 0)
                                {
                                    string query31 = "";
                                    query31 = "select distinct subject_name from internal_cam_calculation_master_setting s,subject u,syllabus_master Y ,tbl_Cam_Calculation C,Registration N where s.subject_no = u.subject_no and u.syll_code = y.syll_code and s.subject_no = c.subject_no and s.Istype = c.Istype and c.roll_no = n.roll_no and y.degree_code = " + ddstandard.SelectedValue + " and y.Batch_Year = " + dropyear.SelectedItem.Text + " and semester = " + dropterm.SelectedItem.Text + " ";

                                    dset31 = d2.select_method_wo_parameter(query31, "Text");
                                    string subj = ds1.Tables[0].Rows[ik]["subject_name"].ToString();
                                    dset31.Tables[0].DefaultView.RowFilter = "subject_name='" + subj + "'";
                                    dv2 = dset31.Tables[0].DefaultView;
                                    if (dset31.Tables[0].Rows.Count > 0)
                                    {
                                        // *********** add Heading ***********
                                        if (checkarray.Count >= 0)
                                        {
                                            dtble = dv2.ToTable();
                                            counttestvalue(dtble);
                                            checkarray.Add(sub);
                                        }
                                        // *********** add Heading ***********
                                    }

                                    if (!hastble3.ContainsKey(dv1[0]["roll_no"]))
                                    {
                                        dr = dTab.NewRow();
                                        dr[0] = count;
                                        dr[1] = dv1[0]["roll_no"].ToString();
                                        dr[2] = dv1[0]["REG_NO"].ToString();
                                        dr[3] = dv1[0]["Stud_Name"].ToString();

                                        count++;
                                        Hashtable hastble2 = new Hashtable();
                                        {
                                            {
                                                int kd = 0;
                                                for (int p = 0; p < dv1.Count; p++)
                                                {
                                                    if (!hastble2.ContainsKey(dv1[p]["roll_no"] + "-" + dv1[p]["Istype"] + "-" + dv1[p]["subject_no"]))
                                                    {
                                                        if (dv1[p]["roll_no"] + "-" + dv1[p]["Istype"] == dv1[p]["roll_no"] + "-" + dv1[p]["Istype"])
                                                        {
                                                            if (!hastble2.ContainsKey(dv1[p]["roll_no"] + "-" + dv1[p]["Istype"] + "-" + dv1[p]["subject_no"]))
                                                            {
                                                                hastble2.Add(dv1[p]["roll_no"] + "-" + dv1[p]["Istype"] + "-" + dv1[p]["subject_no"], dv1[p]["Istype"]);
                                                                column++;

                                                                if (ds1.Tables[0].Rows.Count > 0)
                                                                {
                                                                    string mark = dv1[p]["Exammark"].ToString();
                                                                    string bestquery = "select Mark_Grade  from Grade_Master where '" + mark + "' between frange and trange";
                                                                    dsbest = d2.select_method_wo_parameter(bestquery, "Text");

                                                                    if (!hastble4.ContainsKey(dv1[p]["subject_no"] + "-" + dv1[p]["Istype"] + kd.ToString()))
                                                                    {
                                                                        dTab.Columns.Add(dv1[p]["Istype"] + kd.ToString());
                                                                        hastble4.Add(dv1[p]["subject_no"] + "-" + dv1[p]["Istype"] + kd, column);

                                                                        int cnt = column;
                                                                        dr[cnt + 1] = Convert.ToString(dv1[p]["Exammark"]);
                                                                    }
                                                                    else
                                                                    {
                                                                        int nm = Convert.ToInt32(hastble4[dv1[p]["subject_no"] + "-" + dv1[p]["Istype"] + kd.ToString()]);
                                                                        dr[nm + 1] = Convert.ToString(dv1[p]["Exammark"]);
                                                                    }
                                                                    b++;

                                                                    if (!hastble4.ContainsKey("Grade " + p))
                                                                    {
                                                                        column = column + 1;

                                                                        dTab.Columns.Add("Grade " + b, typeof(string));
                                                                        hastble4.Add("Grade " + kd, column);
                                                                        int cnt22 = column;
                                                                        dr[cnt22 + 1] = Convert.ToString(dsbest.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                                                    }
                                                                    else
                                                                    {
                                                                        int nm = Convert.ToInt32(hastble4["Grade " + p].ToString());
                                                                        if (dsbest.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            dr[nm + 1] = Convert.ToString(dsbest.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                                                        }
                                                                        else
                                                                        {
                                                                            dr[nm + 1] = "";
                                                                        }
                                                                        //hastble4.Clear();
                                                                    }
                                                                    kd++;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                int nm = Convert.ToInt32(hastble4[dv1[p]["Istype"]]);
                                                                dr[nm + 1] = Convert.ToString(dv1[p]["Exammark"]);
                                                            }

                                                            // *************** filter
                                                            ////if (dv1.Count - 1 == p)
                                                            //{
                                                            //    if (!hastblm.ContainsKey(dv1[p]["subject_no"] + "-" + dv1[p]["Istype"] + kd.ToString()))
                                                            //    {
                                                            //        dTab.Columns.Add(dv1[p]["Istype"] + kd.ToString());
                                                            //        hastblm.Add(dv1[p]["subject_no"] + "-" + dv1[p]["Istype"] + kd, column);

                                                            //        column1++;
                                                            //        int cnt3 = column1;
                                                            //        dr[cnt3 + 1] = Convert.ToString(dv1[p]["Exammark"]);
                                                            //    }
                                                            //    string nxt = ds1.Tables[0].Rows[ik]["subject_no"].ToString();
                                                            //    query35A = "select distinct s.subject_no,subject_name,s.Istype from internal_cam_calculation_master_setting s,subject u,syllabus_master Y ,tbl_Cam_Calculation C,Registration N where s.subject_no = u.subject_no and u.syll_code = y.syll_code and s.subject_no = c.subject_no and s.Istype = c.Istype and c.roll_no = n.roll_no and y.degree_code = " + ddstandard.SelectedValue + " and y.Batch_Year = " + dropyear.SelectedItem.Text + " and semester = " + dropterm.SelectedItem.Text + " order by s.subject_no,subject_name,s.Istype";
                                                            //    ds11 = d2.select_method_wo_parameter(query35A, "Text");
                                                            //    if (ds11.Tables[0].Rows.Count > 0)
                                                            //    {
                                                            //        int cnt22 = column;
                                                            //        //dr[cnt22 + 1] = Convert.ToString(dv1[p]["Exammark"]);


                                                            //        //int cnt22 = Convert.ToInt32(hastblm["Istype"].ToString());
                                                            //        //dr[cnt22 + 1] = Convert.ToString(ds11.Tables[0].Rows[0]["Exammark"].ToString()); 
                                                            //    }
                                                            //}
                                                            //*************** filter
                                                        }
                                                    }
                                                }
                                                kd = 0;
                                                b = 0;
                                            }
                                        }
                                        dTab.Rows.Add(dr);
                                        if (!hastble3.ContainsKey(dv1[0]["roll_no"]))
                                        {
                                            hastble3.Add(dv1[0]["roll_no"], dv1[0]["roll_no"]);
                                        }
                                    }
                                    if (count > 0)
                                    {
                                        reportgrid1.DataSource = dTab;
                                        reportgrid1.DataBind();
                                        reportgrid1.Visible = true;
                                        lblerrormsg.Visible = false;
                                        g1btnexcel.Visible = true;
                                        g1btnprint.Visible = true;
                                        lblrptname.Visible = true;
                                        txt_excel.Visible = true;
                                    }
                                    else
                                    {
                                        lblerrormsg.Visible = true;
                                        lblerrormsg.Text = "No Records Found";
                                        reportgrid1.Visible = false;
                                        g1btnprint.Visible = false;
                                        g1btnexcel.Visible = false;
                                        lblrptname.Visible = false;
                                        txt_excel.Visible = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        lblerrormsg.Visible = true;
                        lblerrormsg.Text = "No Records Found";
                        reportgrid1.Visible = false;
                        g1btnprint.Visible = false;
                        g1btnexcel.Visible = false;
                        lblrptname.Visible = false;
                        txt_excel.Visible = false;
                    }
                }
            }
            else
            {
                lblerrormsg.Visible = true;
                lblerrormsg.Text = "No Records Found";
                reportgrid1.Visible = false;
                g1btnprint.Visible = false;
                g1btnexcel.Visible = false;
                lblrptname.Visible = false;
                txt_excel.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void g1btnprint_OnClick(object sender, EventArgs e)
    {
        try
        {

            string degreedetails = "Cummulative Mark & Grade Report" + '@' + "Standard: " + dropyear.SelectedItem.Text + " - " + ddschooltype.SelectedItem.Text + " - " + ddstandard.SelectedItem.Text + " - " + dropterm.SelectedItem.Text + "";
            string pagename = "CummulativeMarkReport.aspx";
            Printcontrol.loadspreaddetails(FpSpread2, pagename, degreedetails);
            Printcontrol.Visible = true;

        }
        catch (Exception ex)
        {

        }
    }

    protected void g1btnexcel_OnClick(object sender, EventArgs e)
    {
        try
        {
            string txt_name = Convert.ToString(txt_excel.Text);
            if (txt_name.Trim() != "")
            {
                d2.printexcelreport(FpSpread2, txt_name);
                lblvalidation.Visible = false;
            }
            else
            {
                lblvalidation.Visible = true;
                lblvalidation.Text = "Please Enter Your Report Name";
                txt_excel.Focus();
            }

        }
        catch (Exception ex)
        {

        }
    }


    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {
        if (txtfromdate.Text == "")
        {
            lblerrormsg.Text = "You can not mark attendance for the date greater than today";
            lblerrormsg.Visible = true;
        }

    }
    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        if (txtfromdate.Text == "")
        {
            txttodate.Text = "";
            FpSpread2.Sheets[0].Visible = false;
            FpSpread2.Visible = false;
            lblerrormsg.Text = "Enter from date first";
            lblerrormsg.Visible = true;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            lblvalidation.Visible = false;
            lblrptname.Visible = false;
            txt_excel.Visible = false;

        }
        else
        {

            string datefad, dtfromad;
            string datefromad;
            string yr4, m4, d4;
            datefad = txtfromdate.Text.ToString();
            string[] split4 = datefad.Split(new Char[] { '/' });
            if (split4.Length == 3)
            {
                datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
                yr4 = split4[2].ToString();
                m4 = split4[1].ToString();
                d4 = split4[0].ToString();
                dtfromad = m4 + "/" + d4 + "/" + yr4;

                string date2ad;
                string datetoad;
                string yr5, m5, d5;
                date2ad = txttodate.Text.ToString();
                string[] split5 = date2ad.Split(new Char[] { '/' });
                if (split5.Length == 3)
                {
                    datetoad = split5[0].ToString() + "/" + split5[1].ToString() + "/" + split5[2].ToString();
                    yr5 = split5[2].ToString();
                    m5 = split5[1].ToString();
                    d5 = split5[0].ToString();
                    datetoad = m5 + "/" + d5 + "/" + yr5;
                    DateTime dt1 = Convert.ToDateTime(dtfromad);
                    DateTime dt2 = Convert.ToDateTime(datetoad);

                    TimeSpan ts = dt2 - dt1;

                    int days = ts.Days;
                    if (days < 0)
                    {

                        FpSpread2.Sheets[0].Visible = false;
                        FpSpread2.Visible = false;
                        lblerrormsg.Text = "From Date Should Be Less Than To Date";
                        lblerrormsg.Visible = true;
                        g1btnexcel.Visible = false;
                        g1btnprint.Visible = false;
                        lblvalidation.Visible = false;
                        lblrptname.Visible = false;
                        txt_excel.Visible = false;

                    }
                    if (dt1 > DateTime.Today)
                    {

                        FpSpread2.Sheets[0].Visible = false;
                        FpSpread2.Visible = false;
                        lblerrormsg.Text = "You can not mark attendance for the date greater than today";
                        lblerrormsg.Visible = true;
                        g1btnexcel.Visible = false;
                        g1btnprint.Visible = false;
                        lblvalidation.Visible = false;
                        lblrptname.Visible = false;
                        txt_excel.Visible = false;

                    }
                    if (dt2 > DateTime.Today)
                    {

                        FpSpread2.Sheets[0].Visible = false;
                        FpSpread2.Visible = false;
                        lblerrormsg.Text = "You can not mark attendance for the date greater than today";
                        lblerrormsg.Visible = true;
                        g1btnexcel.Visible = false;
                        g1btnprint.Visible = false;
                        lblvalidation.Visible = false;
                        lblrptname.Visible = false;
                        txt_excel.Visible = false;

                    }

                }
            }
        }

    }

    public void persentmonthcal(string rollno, string admdate, string fdate, string tdate)
    {

        DataSet dsondutyval = new DataSet();
        Boolean isadm = false;
        hatonduty.Clear();
        try
        {
            workingdays = 0;

            njdate = 0;
            Present = 0;
            tot_per_hrs = 0;
            Absent = 0;
            Onduty = 0;
            Leave = 0;
            workingdays = 0;


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
            working = "";
            present = "";
            notconsider_value = 0;

            string frdate = fdate;
            string todate = tdate;
            string[] spf = frdate.Split('/');
            string[] spt = todate.Split('/');
            cal_from_date = Convert.ToInt32(spf[2]) * 12 + Convert.ToInt32(spf[1]);
            cal_to_date = Convert.ToInt32(spt[2]) * 12 + Convert.ToInt32(spt[1]);

            per_from_date = Convert.ToDateTime(spf[1] + "/" + spf[0] + "/" + spf[2]);
            per_to_date = Convert.ToDateTime(spt[1] + "/" + spt[0] + "/" + spt[2]);
            dumm_from_date = Convert.ToDateTime(spf[1] + "/" + spf[0] + "/" + spf[2]);    //"2014-12-01"

            // admdate = ds4.Tables[0].Rows[rows_count]["adm_date"].ToString();
            string[] admdatesp = admdate.Split(new Char[] { '/' });
            admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
            Admission_date = Convert.ToDateTime(admdate);

            degreecode = Convert.ToString(ddstandard.SelectedItem.Value);
            currentsem = Convert.ToString(dropterm.SelectedItem.Text);

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
                hat.Add("degree_code", int.Parse(ddstandard.SelectedItem.Value));
                hat.Add("sem", int.Parse(dropterm.SelectedItem.Text));
                hat.Add("from_date", per_from_date.ToString("MM/dd/yyyy"));
                hat.Add("to_date", per_to_date.ToString("MM/dd/yyyy"));
                hat.Add("coll_code", int.Parse(ddschool.SelectedItem.Value));

                int iscount = 0;
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + per_from_date.ToString() + "' and '" + per_to_date.ToString() + "' and degree_code=" + degreecode + " and semester=" + currentsem + "";
                DataSet dsholiday = d2.select_method_wo_parameter(sqlstr_holiday, "Text");
                if (dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                }
                hat.Add("iscount", iscount);

                ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");

                hat.Clear();
                hat.Add("degree_code", degreecode);
                hat.Add("sem_ester", int.Parse(currentsem));
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

            //lbltot_att1.Text = pre_present_date.ToString();
            //lbltot_work1.Text = per_workingdays.ToString();
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


}