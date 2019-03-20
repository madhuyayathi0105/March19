using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;
using InsproDataAccess;
using System.Configuration;
public partial class stud_hall_rpt : System.Web.UI.Page
{
    string batchyearselected = string.Empty;
    string semselected = string.Empty;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    int count = 0;
    InsproDirectAccess dirAccess = new InsproDirectAccess();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet ds2 = new DataSet();
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
    ArrayList avoidcol = new ArrayList();
    ReuasableMethods rs = new ReuasableMethods();

    #region Attendance variable

    double conductedDays = 0;
    double presentDays = 0;
    double absentDays = 0;
    double conductedHours = 0;
    double presentHours = 0;
    double absentHours = 0;
    double absentDaysPercentage = 0;
    double absentHoursPercentage = 0;
    string absentDaysPercentage1 = string.Empty;
    string absentHoursPercentage1 = string.Empty;
    string dum_tage_date = "", dum_tage_hrs;
    double leavfinaeamount = 0;
    double medicalLeaveDays = 0;
    double moringabsentfine = 0, eveingabsentfine = 0, studentabsentfine = 0;
    int medicalLeaveHours = 0;
    int medicalLeaveCountPerSession = 0;

    DateTime per_from_date;
    DateTime per_to_date;
    DateTime per_from_gendate;
    DateTime per_to_gendate;
    DateTime per_from_cumdate;
    DateTime per_to_cumdate;
    DateTime dumm_from_date;
    DateTime Admission_date;

    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    int col_count = 0;
    int next = 0;
    int minpresII = 0;
    string value, date;
    int i, rows_count;
    string tempvalue = "-1";
    int ObtValue = -1;
    int cal_from_date, cal_from_date_tmp, cal_from_cumdate_tmp;
    int cal_to_date, start_column = 0, cal_to_date_tmp, cal_to_cumdate_tmp;

    double per_perhrs, per_abshrs, cum_perhrs, cum_abshrs;
    double per_ondu, per_leave, per_hhday, cum_ondu, cum_leave, cum_hhday;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    double halfday = 0;
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double cum_present_date, cum_ondu_date, cum_leave_date, cum_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double cum_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double per_tage_date, cum_tage_date, per_tage_hrs, cum_tage_hrs;
    double cum_tot_point, per_holidate, cum_per_holidate;

    int per_dum_unmark, cum_dum_unmark, dum_unmark;
    int tot_per_hrs, per_tot_per_hrs, cum_per_tot_per, tot_wok_hrs;
    double per_con_hrs, cum_con_hrs;
    double njhr, njdate, per_njhr, per_njdate, cum_njhr, cum_njdate;
    double per_per_hrs, cum_per_perhrs;
    double tot_ondu, per_tot_ondu, cum_tot_ondu, cum_tot_ml, tot_ml, per_tot_ml;

    string[] string_session_values;
    DataSet ds_attnd_pts = new DataSet();
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    string value_holi_status = string.Empty;
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = string.Empty;
    string isonumber = string.Empty;
    int inirow_count = 0;
    int demfcal, demtcal;
    string monthcal;
    Hashtable hatonduty = new Hashtable();
    Hashtable hatcumonduty = new Hashtable();
    DataSet ds1 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    bool yesflag = false;
    int dum_diff_date, unmark;
    double per_leavehrs;
    double minpresday = 0, nohrsprsentperday = 0, noofdaypresen = 0;
    string leftlogo = "", rightlogo = "", leftlength = "", rightlength = "", multi_iso = string.Empty;
    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0, tot_ml_spl_fals = 0;
    double per_leave_fals = 0;
    int per_abshrs_spl_true = 0, tot_per_hrs_spl_true = 0, tot_conduct_hr_spl_true = 0, tot_ondu_spl_true = 0, tot_ml_spl_true = 0;
    double per_leave_true = 0;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0, tot_ml_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;
    int notconsider_value = 0;
    double conduct_hour_new = 0;
    int mmyycount;
    string dd = string.Empty;
    int moncount;
    double dif_date = 0;
    double dif_date1 = 0;
    bool deptflag = false;
    static bool splhr_flag = false;
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    string halforfull = "", mng = "", evng = "", holiday_sched_details = string.Empty;
    TimeSpan ts;
    string diff_date;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
                    return;
                }
            }
            //****************************************************//
            //magesh 29/1/18
            Session["attdaywisecla"] = "0";
            string daywisecal = d2.GetFunction("select value from Master_Settings where settings='Attendance Day Wise Calculation'");
            if (daywisecal.Trim() == "1")
            {
                Session["attdaywisecla"] = "1";
            }//magesh 29/1/18
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                loadsubjecttype();
                bindedulevel();
                cklgender.Items.Add("Male");
                cklgender.Items.Add("Female");
                cklgender.Items.Add("Transgender");
                final.Visible = false;
                loadtype();
                Bindcollege();
                collegecode = ddlcollege.SelectedValue.ToString();
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);

                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                BindSectionDetail();
                FpSpread1.Visible = false;
                //  bindsemester();
            }
        }
        catch (Exception ex) { }
    }

    protected void go_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/default.aspx", false);
    }

    public void loadtype()
    {
        try
        {
            collegecode = Session["collegecode"].ToString();
            ddltype.Items.Clear();
            string strquery = "select distinct type from course where college_code='" + collegecode + "' and type is not null and type<>''";
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataBind();
                //ddltype.Items.Insert(0, "Select");
                ddltype.Enabled = true;
            }
            else
            {
                ddltype.Enabled = false;
            }
        }
        catch
        {
        }
    }

    public void Bindcollege()
    {
        try
        {
            string columnfield = "";
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            DataSet dsprint = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = dsprint;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
                loadtype();
            }
            else
            {
                lblerrormsg.Text = "Set college rights to the staff";
                lblerrormsg.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds2;
                ddlBatch.DataTextField = "Batch_year";
                ddlBatch.DataValueField = "Batch_year";
                ddlBatch.DataBind();
                // ddlBatch.SelectedIndex = ddlBatch.Items.Count - 1;
                //for (int i = 0; i < chklsbatch.Items.Count; i++)
                //{
                //    chklsbatch.Items[i].Selected = true;
                //    if (chklsbatch.Items[i].Selected == true)
                //    {
                //        count += 1;
                //    }
                //    if (chklsbatch.Items.Count == count)
                //    {
                //        chkbatch.Checked = true;
                //    }
                //}
                //if (chkbatch.Checked == true)
                //{
                //    for (int i = 0; i < chklsbatch.Items.Count; i++)
                //    {
                //        chklsbatch.Items[i].Selected = true;
                //        txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < chklsbatch.Items.Count; i++)
                //    {
                //        chklsbatch.Items[i].Selected = false;
                //        txtbatch.Text = "---Select---";
                //    }
                //}
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            lblerrormsg.Visible = false;
            count = 0;
            chklstdegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            // ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (singleuser == "True")
            {
                ds2.Dispose();
                ds2.Reset();
                string strquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id  and course.college_code = degree.college_code   and degree.college_code=" + Session["collegecode"].ToString() + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + "  and course.type='" + ddltype.SelectedItem.Text.ToString() + "' and course.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "'";
                ds2 = da.select_method_wo_parameter(strquery, "Text");
            }
            else
            {
                ds2.Dispose();
                ds2.Reset();
                string strquery1 = "select distinct degree.course_id,course.course_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code  and degree.college_code=" + Session["collegecode"].ToString() + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + " and course.type='" + ddltype.SelectedItem.Text.ToString() + "' and course.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "'";
                ds2 = da.select_method_wo_parameter(strquery1, "Text");
            }
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstdegree.DataSource = ds2;
                chklstdegree.DataTextField = "course_name";
                chklstdegree.DataValueField = "course_id";
                chklstdegree.DataBind();
                chklstdegree.Items[0].Selected = true;
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    if (chklstdegree.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstdegree.Items.Count == count)
                    {
                        chkdegree.Checked = true;
                    }
                }
                if (chkdegree.Checked == true)
                {
                    for (int i = 0; i < chklstdegree.Items.Count; i++)
                    {
                        chklstdegree.Items[i].Selected = true;
                        txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
                    }
                }
                else
                {
                    for (int i = 0; i < chklstdegree.Items.Count; i++)
                    {
                        chklstdegree.Items[i].Selected = false;
                        txtdegree.Text = "---Select---";
                    }
                }
                txtdegree.Enabled = true;
                // BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            }
            else
            {
                txtdegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;
            collegecode = ddlcollege.SelectedValue.ToString();
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            if (course_id.Trim() != "")
            {
                //ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                if (singleuser == "True")
                {
                    ds2.Dispose();
                    ds2.Reset();
                    string strquery = "select distinct degree.degree_code, department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code=" + Session["collegecode"].ToString() + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + "  and course.type='" + ddltype.SelectedItem.Text.ToString() + "' and course.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "'";
                    ds2 = da.select_method_wo_parameter(strquery, "Text");
                }
                else
                {
                    ds2.Dispose();
                    ds2.Reset();
                    string strquery1 = "select distinct degree.degree_code, department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code=" + Session["collegecode"].ToString() + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + " and course.type='" + ddltype.SelectedItem.Text.ToString() + "' and course.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "'";
                    ds2 = da.select_method_wo_parameter(strquery1, "Text");
                }
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    chklstbranch.DataSource = ds2;
                    chklstbranch.DataTextField = "dept_name";
                    chklstbranch.DataValueField = "degree_code";
                    chklstbranch.DataBind();
                    chklstbranch.Items[0].Selected = true;
                    for (int i = 0; i < chklstbranch.Items.Count; i++)
                    {
                        chklstbranch.Items[i].Selected = true;
                        if (chklstbranch.Items[i].Selected == true)
                        {
                            count += 1;
                        }
                        if (chklstbranch.Items.Count == count)
                        {
                            chkbranch.Checked = true;
                        }
                    }
                    if (chkbranch.Checked == true)
                    {
                        for (int i = 0; i < chklstbranch.Items.Count; i++)
                        {
                            chklstbranch.Items[i].Selected = true;
                            txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
                        }
                    }
                    else
                    {
                        for (int i = 0; i < chklstbranch.Items.Count; i++)
                        {
                            chkbranch.Checked = false;
                            chklstbranch.Items[i].Selected = false;
                            txtbranch.Text = "---Select---";
                        }
                    }
                    bindsemester();
                }
            }
            else
            {
                txtbranch.Text = "---Select---";
                chklstbranch.Items.Clear();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void bindedulevel()
    {
        string sql = "select distinct Edu_Level from course where college_code='" + Session["collegecode"].ToString() + "' order by Edu_Level desc";
        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        ddledulevel.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddledulevel.DataSource = ds;
            ddledulevel.DataTextField = "Edu_Level";
            ddledulevel.DataBind();
        }
    }

    public void bindsemester()
    {
        ddlSemYr.Items.Clear();
        cblterm.Items.Clear();
        DataSet studgradeds = new DataSet();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        // int i = 0;
        string strstandard = "";
        //if (ddstandard.SelectedValue != "")
        //{
        //    strstandard = ddstandard.SelectedValue;
        //}
        if (ddlBatch.Items.Count > 0)
        {
            string batch = ddlBatch.SelectedItem.Text.ToString();

            //for (int i = 0; i < chklsbatch.Items.Count; i++)
            //{
            //    if (chklsbatch.Items[i].Selected == true)
            //    {
            //        if (batch == "")
            //        {
            //            batch = chklsbatch.Items[i].Text.ToString();
            //        }
            //        else
            //        {
            //            batch = batch + "," + chklsbatch.Items[i].Text.ToString();
            //        }
            //    }
            //}
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    if (strstandard == "")
                    {
                        strstandard = chklstbranch.Items[i].Value.ToString();
                    }
                    else
                    {
                        strstandard = strstandard + "," + chklstbranch.Items[i].Value.ToString();
                    }
                }
            }
            //if (strstandard.Trim() != "")
            //{
            //    strstandard = " and degree_code in(" + strstandard + ")";
            //}
            //string strquery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + Session["collegecode"].ToString() + " and batch_year in (" + batch + ") and degree_code in (" + strstandard + ") order by NDurations desc";
            //studgradeds.Reset();
            //studgradeds.Dispose();
            //studgradeds = d2.select_method_wo_parameter(strquery, "Text");
            studgradeds = d2.BindSem(strstandard, batch, Session["collegecode"].ToString());
            if (studgradeds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(studgradeds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(studgradeds.Tables[0].Rows[0][0].ToString());
                for (int i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        // cblterm.Items.Add(i.ToString());
                        ddlSemYr.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        // cblterm.Items.Add(i.ToString());
                        ddlSemYr.Items.Add(i.ToString());
                    }
                }
                //if (cblterm.Items.Count > 0)
                //{
                //    int cout = 0;
                //    for (int iq = 0; iq < cblterm.Items.Count; iq++)
                //    {
                //        cout++;
                //        cblterm.Items[iq].Selected = true;
                //    }
                //    cbterm.Checked = true;
                //    txtterm.Text = "Sem (" + cout + ")";
                //}
                //else
                //{
                //    cbterm.Checked = false;
                //    txtterm.Text = "-Select-";
                //}
            }
        }
    }

    public void loadsubjecttype()
    {
        try
        {
            string strquery = "  select distinct subject_type from sub_sem order by subject_type";
            ds.Clear();
            ds = da.select_method_wo_parameter(strquery, "Text");
            dropsubjecttype.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                dropsubjecttype.DataSource = ds;
                dropsubjecttype.DataTextField = "subject_type";
                dropsubjecttype.DataBind();
                //dropsubjecttype.Items.Insert(0, "All");
            }
        }
        catch
        {
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
            collegecode = ddlcollege.SelectedValue.ToString();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Text = "";
        hide();
        BindDegree(singleuser, group_user, collegecode, usercode);
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        BindSectionDetail();
        lblerrormsg.Visible = true;
        return;
    }

    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkbatch.Checked == true)
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                }
                txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = false;
                }
                txtbatch.Text = "---Select---";
            }

            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
        }
    }

    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txtbatch.Text = "--Select--";
            chkbatch.Checked = false;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbatch.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == chklsbatch.Items.Count)
                {
                    chkbatch.Checked = true;
                }
            }

            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddledulevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Text = "";
        hide();
        BindDegree(singleuser, group_user, collegecode, usercode);
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        lblerrormsg.Visible = true;
        return;
    }

    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            collegecode = ddlcollege.SelectedValue.ToString();
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                }
                txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                }
                txtdegree.Text = "---Select---";
                txtbranch.Text = "---Select---";
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            BindSectionDetail();
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
        }
    }

    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            collegecode = ddlcollege.SelectedValue.ToString();
            int commcount = 0;
            chkdegree.Checked = false;
            txtdegree.Text = "---Select---";
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdegree.Text = "Degree(" + commcount.ToString() + ")";
                if (commcount == chklstdegree.Items.Count)
                {
                    chkdegree.Checked = true;
                }
            }

            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            BindSectionDetail();
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                }
                txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                }
                chkbranch.Checked = false;
                txtbranch.Text = "---Select---";
            }
            BindSectionDetail();
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
            string clg = "";
            int commcount = 0;
            txtbranch.Text = "--Select--";
            chkbranch.Checked = false;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbranch.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == chklstbranch.Items.Count)
                {
                    chkbranch.Checked = true;
                }
            }

            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void chksection_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chksection.Checked == true)
            {
                for (int i = 0; i < chk1section.Items.Count; i++)
                {
                    chk1section.Items[i].Selected = true;
                }
                txtsection.Text = "Section(" + (chk1section.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chk1section.Items.Count; i++)
                {
                    chk1section.Items[i].Selected = false;
                }
                chksection.Checked = false;
                txtsection.Text = "---Select---";
            }
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void chk1section_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;

            int commcount = 0;
            txtsection.Text = "--Select--";
            chksection.Checked = false;
            for (int i = 0; i < chk1section.Items.Count; i++)
            {
                if (chk1section.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtsection.Text = "Section(" + commcount.ToString() + ")";
                if (commcount == chk1section.Items.Count)
                {
                    chksection.Checked = true;
                }
            }
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void BindSectionDetail()
    {
        try
        {
            chk1section.Items.Clear();
            string batch = "";
            string collegeSel = ddlcollege.SelectedItem.Value;
            string branch = rs.GetSelectedItemsText(chklstbranch);

            if (ddlBatch.Items.Count > 0)
            {
                batch = ddlBatch.SelectedItem.Value;
            }
            string sqlquery = "select distinct sections from registration r,degree d, department dt where r.degree_code=d.degree_code and dt.dept_code=d.dept_code and  batch_year in('" + batch + "') and dept_name in('" + branch + "') and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
            DataSet ds = new DataSet();
            ds = d2.select_method_wo_parameter(sqlquery, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chk1section.DataSource = ds;
                chk1section.DataTextField = "sections";
                chk1section.DataValueField = "sections";
                chk1section.DataBind();
                //chk1section.Items.Insert( chk1section.Items.Count, "Empty");
                chk1section.Items.Add(new ListItem("Empty", ""));

            }
            else
            {
                chk1section.Items.Add(new ListItem("Empty", ""));
                txtsection.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

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
                txtterm.Text = "Sem (" + cout + ")";
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
                txtterm.Text = "Sem (" + cout + ")";
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

    protected void dropsubjecttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblnorec.Text = "";
        hide();
        lblerrormsg.Visible = true;
        //if (dropsubjecttype.Items.Count > 0)
        //{
        //    //bindsubject();
        //    //  hiddenfiels();
        //}
    }

    protected void ddlrpttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Text = "";
            hide();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            BindSectionDetail();

            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ckgender_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txtgender.Text = "---Select---";
            if (ckgender.Checked == true)
            {
                for (int i = 0; i < cklgender.Items.Count; i++)
                {
                    cklgender.Items[i].Selected = true;
                }
                txtgender.Text = "Gender(" + (cklgender.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cklgender.Items.Count; i++)
                {
                    cklgender.Items[i].Selected = false;
                }
                txtgender.Text = "---Select---";
            }
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
        }
    }

    protected void cklgender_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            ckgender.Checked = false;
            txtgender.Text = "---Select---";
            for (int i = 0; i < cklgender.Items.Count; i++)
            {
                if (cklgender.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtgender.Text = "Gender(" + commcount.ToString() + ")";
                if (commcount == cklgender.Items.Count)
                {
                    ckgender.Checked = true;
                }
            }
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void hide()
    {
        Printcontrol.Visible = false;
        FpSpread1.Visible = false;
        final.Visible = false;
    }

    public void bindheader()
    {
        FpSpread1.Sheets[0].RowHeader.Visible = false;
        FpSpread1.Sheets[0].AutoPostBack = true;
        FpSpread1.CommandBar.Visible = false;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 0;
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
        FpSpread1.Sheets[0].Columns.Count = 8;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.no";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Language";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Hall";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Religion";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Community";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Caste";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Remarks";
        FpSpread1.SaveChanges();
        FpSpread1.Visible = true;
    }

    public void bindheader1()
    {
        FpSpread1.Sheets[0].RowHeader.Visible = false;
        FpSpread1.Sheets[0].AutoPostBack = false;
        FpSpread1.CommandBar.Visible = false;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 0;
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
        FpSpread1.Sheets[0].Columns.Count = 6;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.no";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Attendance Percentage";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Remarks";

        FpSpread1.Sheets[0].Columns[0].Width = 50;
        FpSpread1.Sheets[0].Columns[1].Width = 120;
        FpSpread1.Sheets[0].Columns[2].Width = 180;
        FpSpread1.Sheets[0].Columns[3].Width = 100;
        FpSpread1.Sheets[0].Columns[4].Width = 100;
        //  FpSpread1.Sheets[0].Columns[5].Width = 100;

        FpSpread1.SaveChanges();
        FpSpread1.Visible = true;

    }

    public void bindvalue()
    {
        //string type = "";
        //if (ddltype.Items.Count > 0)
        //{
        //    type = ddltype.SelectedItem.Text.ToString();
        //}
        string deptid = "";
        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {
                if (deptid == "")
                {
                    deptid = chklstbranch.Items[i].Value.ToString();
                }
                else
                {
                    deptid = deptid + "','" + chklstbranch.Items[i].Value.ToString();
                }
            }
        }
        string section = rs.GetSelectedItemsValueAsString(chk1section);//modified in hall subquery delsi1602

        string sql = " select r.Current_Semester,r.degree_code,r.stud_name,R.Batch_year,course_name+'-'+dept_name degree,isnull(r.Sections,'') as Sections,(select isnull(Building_acronym,'') from HT_HostelRegistration s,Building_Master b where s.BuildingFK = b.Code and s.APP_No = r.App_No and ISNULL(IsVacated,0)=0 and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsSuspend,0)=0) as hall,(select textval from textvaltable t where t.TextCode = a.religion) religion,(select textval from textvaltable t where t.TextCode = a.community ) community,(select textval from textvaltable t where t.TextCode = a.caste) caste, r.roll_no,a.app_no,a.religion as religioncode,a.community as communitycode,a.sex,1 TotalStrength from Registration r,applyn a,Degree g,course c,department d where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code  and g.college_code = d.college_code  and r.batch_year = '" + batchyearselected + "' and r.degree_code in ('" + deptid + "') and r.college_code='" + Session["collegecode"].ToString() + "' and r.Current_Semester='" + ddlSemYr.SelectedItem.Text.ToString() + "' and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' ";
        if (section != "")
            sql += " and isNull(r.Sections,'') in ('" + section + "','')";
        else
            sql += " and isNull(r.Sections,'') in ('') ";
        sql += " order by r.reg_no, Course_Name,Dept_Name,hall";

        ds.Clear();
        ds = d2.select_method_wo_parameter(sql, "text");
        DataTable dtCondonation = dirAccess.selectDataTable("select app_no,Roll_no,case when is_eligible=1 then 'EC' when is_eligible=2 then 'NE' else 'PA' end as Condonation from Eligibility_list el where el.Semester='" + ddlSemYr.SelectedItem.Text.ToString() + "' and el.batch_year in ('" + batchyearselected + "') and degree_code in ('" + deptid + "')");
        DataSet subjchoos = new DataSet();
        sql = "select roll_no,acronym from subjectchooser c,subject s ,sub_sem u where c.subject_no = s.subject_no and s.subType_no = u.subType_no and subject_type = 'Foundation Course - I' and roll_no in (select roll_no from Registration r where  r.batch_year = '" + batchyearselected + "' and r.degree_code in ('" + deptid + "') and r.Current_Semester='" + ddlSemYr.SelectedItem.Text.ToString() + "' and r.college_code='" + Session["collegecode"].ToString() + "' and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' ) and semester = '" + ddlSemYr.SelectedItem.Text.ToString() + "' and s.subject_name not like 'Tamil%'";
        subjchoos.Clear();
        subjchoos = d2.select_method_wo_parameter(sql, "text");
        DataView dvsubjchoos = new DataView();
        int sno = 1;
        string year = "";
        string degreetitle = "";
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            string sectionname = Convert.ToString(ds.Tables[0].Rows[i]["Sections"]).Trim();
            if (ds.Tables[0].Rows[i][0].ToString() == "1" || ds.Tables[0].Rows[i][0].ToString() == "2")
            {
                year = "I  " + ds.Tables[0].Rows[i]["degree"].ToString() + ((!string.IsNullOrEmpty(sectionname.Trim())) ? " - " + sectionname.Trim().ToUpper() : "") + "  " + ds.Tables[0].Rows[i]["Batch_year"].ToString() + " - " + (Convert.ToInt32(ds.Tables[0].Rows[i]["Batch_year"].ToString()) + 1);
            }
            if (ds.Tables[0].Rows[i][0].ToString() == "3" || ds.Tables[0].Rows[i][0].ToString() == "4")
            {
                year = "II " + ds.Tables[0].Rows[i]["degree"].ToString() + ((!string.IsNullOrEmpty(sectionname.Trim())) ? " - " + sectionname.Trim().ToUpper() : "") + "  " + ds.Tables[0].Rows[i]["Batch_year"].ToString() + " - " + (Convert.ToInt32(ds.Tables[0].Rows[i]["Batch_year"].ToString()) + 1);
            }
            if (ds.Tables[0].Rows[i][0].ToString() == "5" || ds.Tables[0].Rows[i][0].ToString() == "6")
            {
                year = "III " + ds.Tables[0].Rows[i]["degree"].ToString() + ((!string.IsNullOrEmpty(sectionname.Trim())) ? " - " + sectionname.Trim().ToUpper() : "") + "  " + ds.Tables[0].Rows[i]["Batch_year"].ToString() + " - " + (Convert.ToInt32(ds.Tables[0].Rows[i]["Batch_year"].ToString()) + 1);
            }
            if (ds.Tables[0].Rows[i][0].ToString() == "7" || ds.Tables[0].Rows[i][0].ToString() == "8")
            {
                year = "IV " + ds.Tables[0].Rows[i]["degree"].ToString() + ((!string.IsNullOrEmpty(sectionname.Trim())) ? " - " + sectionname.Trim().ToUpper() : "") + "  " + ds.Tables[0].Rows[i]["Batch_year"].ToString() + " - " + (Convert.ToInt32(ds.Tables[0].Rows[i]["Batch_year"].ToString()) + 1);
            }
            if (degreetitle.Trim().ToLower() != ds.Tables[0].Rows[i]["degree"].ToString().Trim().ToLower() + ((!string.IsNullOrEmpty(" - " + sectionname.Trim())) ? sectionname.Trim().ToLower() : ""))
            {
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = year;
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                degreetitle = ds.Tables[0].Rows[i]["degree"].ToString().Trim().ToLower() + ((!string.IsNullOrEmpty(" - " + sectionname.Trim())) ? sectionname.Trim().ToLower() : "");
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#219DA5");
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = "";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = "";
            }
            //else
            //{
            //}
            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
            sno++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["stud_name"].ToString();
            subjchoos.Tables[0].DefaultView.RowFilter = "roll_no='" + ds.Tables[0].Rows[i]["roll_no"].ToString() + "'";
            dvsubjchoos = subjchoos.Tables[0].DefaultView;
            if (dvsubjchoos.Count > 0)
            {
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dvsubjchoos[0]["acronym"].ToString();
            }
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["hall"].ToString();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["religion"].ToString();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["community"].ToString();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["caste"].ToString();
            string condo = "EC";
            if (dtCondonation.Rows.Count > 0)
            {
                dtCondonation.DefaultView.RowFilter = "app_no='" + ds.Tables[0].Rows[i]["app_no"].ToString() + "'";
                DataView dvNew = dtCondonation.DefaultView;
                if (dvNew.Count > 0)
                {
                    condo = dvNew[0]["Condonation"].ToString();
                }
            }
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = condo;
        }

        #region catagory total 02.03.17 barath
        if (cb_includetotal.Checked == true)
        {
            FpSpread1.Sheets[0].RowCount++; FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "S.No";
            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#99CCFF");

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Section";
            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#99CCFF");





            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Category";
            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].ForeColor = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#99CCFF");
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = "Male";
            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].BackColor = ColorTranslator.FromHtml("#99CCFF");
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "Female";
            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].ForeColor = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].BackColor = ColorTranslator.FromHtml("#99CCFF");
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "Total";
            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = true;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].BackColor = ColorTranslator.FromHtml("#99CCFF");
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].BackColor = ColorTranslator.FromHtml("#99CCFF");
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].BackColor = ColorTranslator.FromHtml("#99CCFF");
            double malecout = 0; double femalecout = 0; double total = 0;
            DataTable dt_ds = ds.Tables[0];
            DataTable dt_category = new DataTable();
            dt_category = dt_ds.DefaultView.ToTable(true, "communitycode", "community");
            Hashtable totalvalue_dic = new Hashtable();
            if (dt_category.Rows.Count > 0)
            {
                sno = 0;
                foreach (DataRow dr in dt_category.Rows)
                {
                    sno++;
                    for (int i = 0; i < chk1section.Items.Count; i++)
                    {
                        if (chk1section.Items[i].Selected == true)
                        {
                            malecout = 0; femalecout = 0; total = 0;
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(Convert.ToString(chk1section.Items[i].Value));
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr["community"]);
                            double.TryParse(Convert.ToString(Convert.ToString(ds.Tables[0].Compute("Sum(TotalStrength)", " communitycode='" + dr["communitycode"] + "' and sex='0' and Sections='" + Convert.ToString(chk1section.Items[i].Value) + "'"))), out malecout);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(malecout);
                            double.TryParse(Convert.ToString(Convert.ToString(ds.Tables[0].Compute("Sum(TotalStrength)", " communitycode='" + dr["communitycode"] + "' and sex='1' and Sections='" + Convert.ToString(chk1section.Items[i].Value) + "'"))), out femalecout);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(femalecout);
                            total = malecout + femalecout;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(total);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;


                        }
                    }
                    //FpSpread1.Sheets[0].RowCount++;
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr["community"]);
                    //double.TryParse(Convert.ToString(Convert.ToString(ds.Tables[0].Compute("Sum(TotalStrength)", " communitycode='" + dr["communitycode"] + "' and sex='0' "))), out malecout);
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(malecout);
                    //double.TryParse(Convert.ToString(Convert.ToString(ds.Tables[0].Compute("Sum(TotalStrength)", " communitycode='" + dr["communitycode"] + "' and sex='1' "))), out femalecout);
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(femalecout);

                    //double.TryParse(Convert.ToString(Convert.ToString(ds.Tables[0].Compute("Sum(TotalStrength)", " communitycode='" + dr["communitycode"] + "'"))), out total);
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(total);
                    //maleGT += malecout;
                    //femaleGT += femalecout;
                    //totalGT += total;

                }
                for (int i = 0; i < chk1section.Items.Count; i++)
                {
                    if (chk1section.Items[i].Selected == true)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Grand Total";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(Convert.ToString(chk1section.Items[i].Value));
                        double.TryParse(Convert.ToString(Convert.ToString(ds.Tables[0].Compute("Sum(TotalStrength)", " sex='0' and Sections='" + Convert.ToString(chk1section.Items[i].Value) + "'"))), out malecout);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(malecout);
                        double.TryParse(Convert.ToString(Convert.ToString(ds.Tables[0].Compute("Sum(TotalStrength)", " sex='1' and Sections='" + Convert.ToString(chk1section.Items[i].Value) + "'"))), out femalecout);
                        total = malecout + femalecout;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(femalecout);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(total);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = ColorTranslator.FromHtml("#219DA5");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].ForeColor = ColorTranslator.FromHtml("#219DA5");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = ColorTranslator.FromHtml("#219DA5");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = ColorTranslator.FromHtml("#219DA5");
                    }
                }
            }
            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "S.No";
            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.Blue;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#99CCFF");

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Section";
            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#99CCFF");




            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Religion";
            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].ForeColor = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#99CCFF");
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Male";
            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].BackColor = ColorTranslator.FromHtml("#99CCFF");
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "Female";
            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].ForeColor = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].BackColor = ColorTranslator.FromHtml("#99CCFF");
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "Total";
            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = true;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].BackColor = ColorTranslator.FromHtml("#99CCFF");
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].BackColor = ColorTranslator.FromHtml("#99CCFF");
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].BackColor = ColorTranslator.FromHtml("#99CCFF");
            DataTable dt_ds1 = ds.Tables[0];
            DataTable dt_category1 = new DataTable();
            dt_category1 = dt_ds1.DefaultView.ToTable(true, "religioncode", "religion");

            if (dt_category1.Rows.Count > 0)
            {
                sno = 0;
                foreach (DataRow dr in dt_category1.Rows)
                {

                    sno++;
                    for (int i = 0; i < chk1section.Items.Count; i++)
                    {
                        if (chk1section.Items[i].Selected == true)
                        {
                            malecout = 0; femalecout = 0; total = 0;

                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(Convert.ToString(chk1section.Items[i].Value));
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr["religion"]);
                            double.TryParse(Convert.ToString(Convert.ToString(ds.Tables[0].Compute("Sum(TotalStrength)", " religioncode='" + dr["religioncode"] + "' and sex='0' and Sections='" + Convert.ToString(chk1section.Items[i].Value) + "'"))), out malecout);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(malecout);
                            double.TryParse(Convert.ToString(Convert.ToString(ds.Tables[0].Compute("Sum(TotalStrength)", " religioncode='" + dr["religioncode"] + "' and sex='1' and Sections='" + Convert.ToString(chk1section.Items[i].Value) + "'"))), out femalecout);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(femalecout);
                            total = malecout + femalecout;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(total);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                        }
                    }
                }
                for (int i = 0; i < chk1section.Items.Count; i++)
                {
                    if (chk1section.Items[i].Selected == true)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Grand Total";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(Convert.ToString(chk1section.Items[i].Value));
                        double.TryParse(Convert.ToString(Convert.ToString(ds.Tables[0].Compute("Sum(TotalStrength)", " sex='0' and Sections='" + Convert.ToString(chk1section.Items[i].Value) + "'"))), out malecout);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(malecout);
                        double.TryParse(Convert.ToString(Convert.ToString(ds.Tables[0].Compute("Sum(TotalStrength)", " sex='1' and Sections='" + Convert.ToString(chk1section.Items[i].Value) + "'"))), out femalecout);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(femalecout);
                        total = malecout + femalecout;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(total);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = ColorTranslator.FromHtml("#219DA5");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].ForeColor = ColorTranslator.FromHtml("#219DA5");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = ColorTranslator.FromHtml("#219DA5");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = ColorTranslator.FromHtml("#219DA5");
                    }
                }
            }
            FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        }
        #endregion

        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Columns[7].Visible = false;//02.03.17 barath
        for (int i = 0; i < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; i++)
        {
            FpSpread1.Sheets[0].ColumnHeader.Columns[i].ForeColor = Color.White;
            FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Columns[i].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[i].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[i].Font.Size = FontUnit.Medium;
            // FpSpread1.Sheets[0].Columns[i].Font.Bold = true;
            FpSpread1.Sheets[0].Columns[i].ForeColor = Color.Black;
            FpSpread1.Sheets[0].Columns[i].Locked = true;
        }
        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        FpSpread1.SaveChanges();
    }

    public void bindvalue1()
    {
        //string type = "";
        //if (ddltype.Items.Count > 0)
        //{
        //    type = ddltype.SelectedItem.Text.ToString();
        //}
        string deptid = "";
        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {
                if (deptid == "")
                {
                    deptid = chklstbranch.Items[i].Value.ToString();
                }
                else
                {
                    deptid = deptid + "','" + chklstbranch.Items[i].Value.ToString();
                }
            }
        }


        string sql = " select r.Current_Semester,r.degree_code,r.stud_name,R.Batch_year,course_name+'-'+dept_name degree,(select isnull(Building_acronym,'') from Hostel_StudentDetails s,Building_Master b where s.Building_Name = b.Building_Name and s.Roll_Admit = r.roll_admit) hall,(select textval from textvaltable t where t.TextCode = a.religion) religion,(select textval from textvaltable t where t.TextCode = a.community ) community,(select textval from textvaltable t where t.TextCode = a.caste) caste, r.roll_no, r.reg_no,r.college_code,r.Roll_No,CONVERT(varchar(10), r.Adm_Date,103)Adm_Date,r.app_no  from Registration r,applyn a,Degree g,course c,department d where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code  and g.college_code = d.college_code  and r.batch_year = '" + batchyearselected + "' and r.degree_code in ('" + deptid + "') and r.college_code='" + Session["collegecode"].ToString() + "' and r.DelFlag=0 and r.Exam_Flag<>'debar' order by r.reg_no";//and r.Current_Semester='" + ddlSemYr.SelectedItem.Text.ToString() + "'
        sql += " select app_no,Remarks,Semester,batch_year,degree_code,is_eligible from Eligibility_list where batch_year='" + batchyearselected + "'  and degree_code in('" + deptid + "') and Semester='" + ddlSemYr.SelectedItem.Text.ToString() + "'";//
        DataSet studentDet = new DataSet();
        studentDet = d2.select_method_wo_parameter(sql, "text");
        int sno = 1;
        string year = "";
        string degreetitle = "";
        string dept = "";
        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        for (int i = 0; i < studentDet.Tables[0].Rows.Count; i++)
        {
            if (studentDet.Tables[0].Rows[i][0].ToString() == "1" || studentDet.Tables[0].Rows[i][0].ToString() == "2")
            {
                year = "I  " + studentDet.Tables[0].Rows[i]["degree"].ToString() + "  " + studentDet.Tables[0].Rows[i]["Batch_year"].ToString() + " - " + (Convert.ToInt32(studentDet.Tables[0].Rows[i]["Batch_year"].ToString()) + 1);
                dept = studentDet.Tables[0].Rows[i]["degree"].ToString();
            }
            if (studentDet.Tables[0].Rows[i][0].ToString() == "3" || studentDet.Tables[0].Rows[i][0].ToString() == "4")
            {
                year = "II " + studentDet.Tables[0].Rows[i]["degree"].ToString() + "  " + studentDet.Tables[0].Rows[i]["Batch_year"].ToString() + " - " + (Convert.ToInt32(studentDet.Tables[0].Rows[i]["Batch_year"].ToString()) + 1);
                dept = studentDet.Tables[0].Rows[i]["degree"].ToString();
            }
            if (studentDet.Tables[0].Rows[i][0].ToString() == "5" || studentDet.Tables[0].Rows[i][0].ToString() == "6")
            {
                year = "III " + studentDet.Tables[0].Rows[i]["degree"].ToString() + "  " + studentDet.Tables[0].Rows[i]["Batch_year"].ToString() + " - " + (Convert.ToInt32(studentDet.Tables[0].Rows[i]["Batch_year"].ToString()) + 1);
                dept = studentDet.Tables[0].Rows[i]["degree"].ToString();
            }
            if (studentDet.Tables[0].Rows[i][0].ToString() == "7" || studentDet.Tables[0].Rows[i][0].ToString() == "8")
            {
                year = "IV " + studentDet.Tables[0].Rows[i]["degree"].ToString() + "  " + studentDet.Tables[0].Rows[i]["Batch_year"].ToString() + " - " + (Convert.ToInt32(studentDet.Tables[0].Rows[i]["Batch_year"].ToString()) + 1);//change by abarna
                dept = studentDet.Tables[0].Rows[i]["degree"].ToString();
            }
            if (degreetitle.Trim().ToLower() != studentDet.Tables[0].Rows[i]["degree"].ToString().Trim().ToLower())
            {
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = year;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = dept;


                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                string sem = Convert.ToString(ddlSemYr.SelectedItem.Text);//studentDet.Tables[0].Rows[i]["Current_Semester"]).Trim();//delsi 09/10
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = sem;

                degreetitle = studentDet.Tables[0].Rows[i]["degree"].ToString().Trim().ToLower();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#219DA5");
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "";
            }
            //else
            //{
            //}

            FpSpread1.Sheets[0].RowCount++;


            string rollno = Convert.ToString(studentDet.Tables[0].Rows[i]["Roll_No"]).Trim();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = rollno;
            sno++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = studentDet.Tables[0].Rows[i]["stud_name"].ToString();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = studentDet.Tables[0].Rows[i]["reg_no"].ToString();
            string collegecode = Convert.ToString(studentDet.Tables[0].Rows[i]["college_code"]).Trim();
            string degreeCode = Convert.ToString(studentDet.Tables[0].Rows[i]["degree_code"]).Trim();
            string Sem = ddlSemYr.SelectedItem.Text.ToString();// Convert.ToString(studentDet.Tables[0].Rows[i]["Current_Semester"]).Trim();

            string admittedDate = Convert.ToString(studentDet.Tables[0].Rows[i]["Adm_Date"]).Trim();
            string Batchyear = Convert.ToString(studentDet.Tables[0].Rows[i]["Batch_year"]).Trim();
            string AppNo = Convert.ToString(studentDet.Tables[0].Rows[i]["app_no"]).Trim();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Batchyear;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = AppNo;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Sem;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Note = degreeCode;
            AttendancePercentage(collegecode, degreeCode, Sem, rollno, admittedDate);
            double presentPercentage = 0;
            double absentPercentage = 0;

            if (rblHrDaywise.SelectedIndex == 0)
                double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_date), 2)), out presentPercentage);
            else
                double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_hrs), 2)), out presentPercentage);

            if (rblHrDaywise.SelectedIndex == 0)
                double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(absentDays), 2)), out absentPercentage);
            else
                double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(absentHours), 2)), out absentPercentage);

            //if (rblHrDaywise.SelectedIndex == 0)
            //    double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(absentDaysPercentage1), 2)), out absentPercentage);
            //else
            //    double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(absentHoursPercentage1), 2)), out absentPercentage);

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(absentPercentage).Trim();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(presentPercentage).Trim();

            studentDet.Tables[1].DefaultView.RowFilter = " app_no='" + AppNo + "' and batch_year='" + Batchyear + "' and Semester='" + Sem + "' and degree_code='" + degreeCode + "'";
            DataView RemarkDv = studentDet.Tables[1].DefaultView;
            string Remarks = string.Empty;
            if (RemarkDv.Count > 0)
                Remarks = Convert.ToString(RemarkDv[0]["Remarks"]);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Remarks;
        }
        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
        for (int i = 0; i < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; i++)
        {
            FpSpread1.Sheets[0].ColumnHeader.Columns[i].ForeColor = Color.White;
            FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Columns[i].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[i].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[i].Font.Size = FontUnit.Medium;
            // FpSpread1.Sheets[0].Columns[i].Font.Bold = true;
            FpSpread1.Sheets[0].Columns[i].ForeColor = Color.Black;
            FpSpread1.Sheets[0].Columns[i].Locked = true;
        }
        FpSpread1.Sheets[0].Columns[4].Locked = false;
        FpSpread1.Height = FpSpread1.Sheets[0].RowCount * 26;
        FpSpread1.Width = 570;
        FpSpread1.WaitMessage = "Loading";
        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        FpSpread1.SaveChanges();
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            lblnorec.Text = "";
            hide();
            lblerrormsg.Visible = true;
            int count = 0;
            count = 0;
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                lblerrormsg.Text = "Please Select Atleast One Degree";
                hide();
                lblerrormsg.Visible = true;
                return;
            }
            else
            {
                lblerrormsg.Text = "";
            }
            count = 0;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                lblerrormsg.Text = "Please Select Atleast One Branch";
                hide();
                lblerrormsg.Visible = true;
                return;
            }
            else
            {
                lblerrormsg.Text = "";
            }
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = System.Drawing.Color.Teal;
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            batchyearselected = ddlBatch.SelectedItem.Text.ToString();
            semselected = ddlSemYr.SelectedItem.Text.ToString();
            if (ddlrpttype.SelectedIndex == 0)
            {
                bindheader();
                bindvalue();
            }
            else
            {
                bindheader1();
                bindvalue1();
            }
            // bindvalue();
            if (FpSpread1.Sheets[0].Rows.Count > 0)
            {
                FpSpread1.Visible = true;
                final.Visible = true;
                if (ddlrpttype.SelectedIndex == 1)
                    btnremarkssave.Visible = true;
                else
                    btnremarkssave.Visible = false;
            }
            else
            {
                FpSpread1.Visible = false;
                final.Visible = false;
                lblerrormsg.Text = "No Records Found";
                hide();
                lblerrormsg.Visible = true;
                //return;
            }
            ds.Clear();
            ds.Dispose();
            avoidcol.Clear();
            // avoirows.Clear();
        }
        catch
        {
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string print = "";
            string appPath = HttpContext.Current.Server.MapPath("~");
            string strexcelname = "";
            if (appPath != "")
            {
                strexcelname = txtexcelname.Text;
                appPath = appPath.Replace("\\", "/");
                if (strexcelname != "")
                {
                    print = strexcelname;
                    //FpEntry.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);
                    //Aruna on 26feb2013============================
                    string szPath = appPath + "/Report/";
                    string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")
                    FpSpread1.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
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
                    txtexcelname.Focus();
                    lblnorec.Text = "Please Enter Your Report Name";
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
            lblnorec.Text = "";
            //// string date_filt = "From : " + tbstart_date.Text.ToString() + "   " + "To : " + tbend_date.Text.ToString();
            //string degreeset = da.GetFunction("select (Course_Name+' - '+Acronym) as degreeset from course c, degree d where c.Course_Id=d.Course_Id and Degree_Code='" + ddstandard.SelectedItem.Value.ToString() + "'");
            //degreeset=degreeset+" - "+ ddlSemYr.SelectedItem.Text.ToString();
            //string strsec = "";
            //if (ddlSec.Enabled == true)
            //{
            //    string sections = ddlSec.SelectedItem.Text.ToString();
            //    if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
            //    {
            //        strsec = "";
            //    }
            //    else
            //    {
            //        strsec = " - " + sections.ToString();
            //    }
            //}
            //degreeset = degreeset +  strsec;
            //int batchyear = Convert.ToInt32(dropyear.SelectedItem.Text.ToString());
            //string date_filt = "Batch : "+ddlBatch.SelectedItem.Text.ToString()+" ";
            int batchh = Convert.ToInt32(ddlBatch.SelectedItem.Text.ToString());
            //date_filt = date_filt + "@" + "Degree : ";
            string degreedetails = string.Empty;
            // string sqlschool = "select value from Master_Settings where settings='Academic year'";
            //ds.Clear();
            //                ds = da.select_method_wo_parameter(sqlschool, "Text");
            //                string splitvalue = ds.Tables[0].Rows[0]["value"].ToString();
            //                string[] dsplit = splitvalue.Split(',');
            //                string fvalue = dsplit[0].ToString();
            //                string lvalue = dsplit[1].ToString();
            //                string acdmic_date = fvalue + "-" + lvalue;
            degreedetails = "Languages strength Report " + (batchh) + " - " + (batchh + 1);
            degreedetails = "";
            string pagename = "languagewise_stngth.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }

    protected void btnprintdirect_Click(object sender, EventArgs e)
    {
        bindpdf();
    }

    protected void btnprint_Click(object sender, EventArgs e)
    {
        bindformate3pdf();
    }

    public void bindformate3pdf()
    {

        try
        {
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
            System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontbold1 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
            System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
            System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
            System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
            System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
            System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
            System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            Gios.Pdf.PdfPage mypdfpage;


            #region attendancesheet
            mypdfpage = mydoc.NewPage();

            string sql = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            DataSet ds = new DataSet();
            ds.Clear();
            ds.Dispose();
            ds = da.select_method_wo_parameter(sql, "Text");
            PdfTextArea collinfo = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][0].ToString() + "");
            mypdfpage.Add(collinfo);

            string address = ds.Tables[0].Rows[0][2].ToString() + "" + ds.Tables[0].Rows[0][3].ToString() + "" + ds.Tables[0].Rows[0][4].ToString();
            collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
            mypdfpage.Add(collinfo);

            PdfArea pa1 = new PdfArea(mydoc, 14, 12, 564, 821);
            PdfRectangle border = new PdfRectangle(mydoc, pa1, Color.Black);
            mypdfpage.Add(border);
            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
            {
                Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                mypdfpage.Add(LogoImage, 20, 20, 450);
            }
            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
            {
                Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                mypdfpage.Add(LogoImage1, 500, 20, 450);
            }
            Gios.Pdf.PdfTable studinfoss;
            int rowscc = 0;
            studinfoss = mydoc.NewTable(Fontsmall1, 37, 5, 3);
            int rowtable1 = 1;
            studinfoss.VisibleHeaders = true;
            studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
            studinfoss.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
            studinfoss.Cell(0, 0).SetFont(Fontbold);
            studinfoss.Cell(0, 0).SetContent("S.No");
            studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            studinfoss.Cell(0, 1).SetFont(Fontbold);
            studinfoss.Cell(0, 1).SetContent("Reg No");
            studinfoss.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
            studinfoss.Cell(0, 2).SetFont(Fontbold);
            studinfoss.Cell(0, 2).SetContent("Name");
            studinfoss.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
            studinfoss.Cell(0, 3).SetFont(Fontbold);
            studinfoss.Cell(0, 3).SetContent("No of days Absent");
            studinfoss.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
            studinfoss.Cell(0, 4).SetFont(Fontbold);
            studinfoss.Cell(0, 4).SetContent("Remarks");
            studinfoss.Columns[0].SetWidth(3);
            studinfoss.Columns[1].SetWidth(5);
            studinfoss.Columns[2].SetWidth(15);
            studinfoss.Columns[3].SetWidth(10);
            studinfoss.Columns[4].SetWidth(5);

            studinfoss.Cell(35, 0).SetContent("");
            Gios.Pdf.PdfTablePage addtabletopage;
            // studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
            for (int studcount = 0; studcount < FpSpread1.Sheets[0].RowCount; studcount++)
            {
                if (rowtable1 >= 37)
                {
                    rowscc = FpSpread1.Sheets[0].RowCount - studcount;
                    int tablerow = 0;
                    if (rowscc >= 37)
                        tablerow = 37;
                    else
                        tablerow = rowscc + 1;
                    addtabletopage = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 100, 553, 800));
                    mypdfpage.Add(addtabletopage);
                    mypdfpage.SaveToDocument();
                    mypdfpage = mydoc.NewPage();
                    collinfo = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][0].ToString() + "");
                    mypdfpage.Add(collinfo);
                    collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
                    mypdfpage.Add(collinfo);
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                    {
                        Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 20, 20, 450);
                    }
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                    {
                        Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                        mypdfpage.Add(LogoImage1, 500, 20, 450);
                    }
                    mypdfpage.Add(border);
                    studinfoss = mydoc.NewTable(Fontsmall1, tablerow, 5, 3);//need to modify in ths
                    studinfoss.VisibleHeaders = false;
                    //studinfoss.SetBorders(Color.Black, 1, BorderType.None);
                    studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    studinfoss.Columns[0].SetWidth(3);
                    studinfoss.Columns[1].SetWidth(5);
                    studinfoss.Columns[2].SetWidth(15);
                    studinfoss.Columns[3].SetWidth(10);
                    studinfoss.Columns[4].SetWidth(5);
                    studinfoss.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    studinfoss.Cell(0, 0).SetFont(Fontbold);
                    studinfoss.Cell(0, 0).SetContent("S.No");
                    studinfoss.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    studinfoss.Cell(0, 1).SetFont(Fontbold);
                    studinfoss.Cell(0, 1).SetContent("Reg No");
                    studinfoss.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                    studinfoss.Cell(0, 2).SetFont(Fontbold);
                    studinfoss.Cell(0, 2).SetContent("Name");
                    studinfoss.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                    studinfoss.Cell(0, 3).SetFont(Fontbold);
                    studinfoss.Cell(0, 3).SetContent("No of days Absent");
                    studinfoss.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                    studinfoss.Cell(0, 4).SetFont(Fontbold);
                    studinfoss.Cell(0, 4).SetContent("Remarks");


                    rowtable1 = 1;
                }

                if (rowtable1 <= 37)
                {
                    string sem1 = Convert.ToString(FpSpread1.Sheets[0].Cells[studcount, 0].Note);
                    //magesh 3/2/2018
                    string deptba = Convert.ToString(FpSpread1.Sheets[0].Cells[studcount, 3].Note);
                    string batchdept = Convert.ToString(ddlBatch.SelectedValue).Trim();//magesh 3/2/18

                    studinfoss.Cell(rowtable1, 0).SetContent(FpSpread1.Sheets[0].Cells[studcount, 0].Text.ToString());//year and degree binding here
                    int totalstudents = 0;
                    if (Int32.TryParse(FpSpread1.Sheets[0].Cells[studcount, 0].Text.ToString(), out totalstudents))
                    {
                        studinfoss.Cell(rowtable1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    }
                    else
                    {
                        //magesh 29/1/18
                        studinfoss.Cell(rowtable1, 0).SetContent(deptba + "-" + batchdept + "-" + "Sem" + "-" + sem1);
                        //studinfoss.Cell(rowtable1, 0).SetContent(FpSpread1.Sheets[0].Cells[studcount, 3].Note.ToString() + "-" + "Sem" + "-" + sem1);
                        foreach (PdfCell pr in studinfoss.CellRange(rowtable1, 0, rowtable1, 0).Cells)
                        {
                            pr.ColSpan = 4;
                        }
                        studinfoss.Cell(rowtable1, 0).SetFont(Fontsmall1bold);
                        studinfoss.Cell(rowtable1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                    }
                    string format = Convert.ToString(FpSpread1.Sheets[0].Cells[studcount, 3].Tag);
                    string format1 = string.Format("{0:0.00}", format);

                    studinfoss.Cell(rowtable1, 2).SetContent(FpSpread1.Sheets[0].Cells[studcount, 2].Text.ToString());
                    studinfoss.Cell(rowtable1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                    studinfoss.Cell(rowtable1, 1).SetContent(FpSpread1.Sheets[0].Cells[studcount, 1].Text.ToString());
                    studinfoss.Cell(rowtable1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    studinfoss.Cell(rowtable1, 4).SetContent(FpSpread1.Sheets[0].Cells[studcount, 4].Text.ToString().ToUpper());
                    studinfoss.Cell(rowtable1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                    //studinfoss.Cell(rowtable1, 3).SetContent(Convert.ToString(FpSpread1.Sheets[0].Cells[studcount, 3].Tag));
                    studinfoss.Cell(rowtable1, 3).SetContent(format1);
                    studinfoss.Cell(rowtable1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);






                    rowtable1++;
                }

            }
            addtabletopage = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 100, 553, 800));
            mypdfpage.Add(addtabletopage);
            mypdfpage.SaveToDocument();
            #endregion

            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "Marksheets" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                Response.Buffer = true;
                Response.Clear();
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch
        {
        }

    }

    public void bindpdf()
    {
        try
        {
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
            System.Drawing.Font Fontbolda = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontbold1 = new System.Drawing.Font("Book Antiqua", 16, FontStyle.Bold);
            System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
            System.Drawing.Font Fontmedium1 = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Bold);
            System.Drawing.Font Fontsmall9 = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Regular);
            System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
            System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
            System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
            System.Drawing.Font tamil = new System.Drawing.Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            Gios.Pdf.PdfPage mypdfpage;
            if (ddlrpttype.SelectedIndex == 0)
            {
                #region student details

                mypdfpage = mydoc.NewPage();
                //Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                string sql = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                DataSet ds = new DataSet();
                ds.Clear();
                ds.Dispose();
                ds = da.select_method_wo_parameter(sql, "Text");
                PdfTextArea collinfo = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][0].ToString() + "");
                mypdfpage.Add(collinfo);
                //collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][1].ToString() + "");
                //mypdfpage.Add(collinfo);
                string address = ds.Tables[0].Rows[0][2].ToString() + "" + ds.Tables[0].Rows[0][3].ToString() + "" + ds.Tables[0].Rows[0][4].ToString();
                collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
                mypdfpage.Add(collinfo);
                //collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 70, 595, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  :" + ds.Tables[0].Rows[0][6].ToString());
                //mypdfpage.Add(collinfo);
                PdfArea pa1 = new PdfArea(mydoc, 14, 12, 564, 821);
                PdfRectangle border = new PdfRectangle(mydoc, pa1, Color.Black);
                mypdfpage.Add(border);
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                {
                    Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                    mypdfpage.Add(LogoImage, 20, 20, 450);
                }
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                {
                    Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                    mypdfpage.Add(LogoImage1, 500, 20, 450);
                }
                Gios.Pdf.PdfTable studinfoss;
                int fpdiv2rowcount = FpSpread1.Sheets[0].RowCount / 2;
                int rowscc = 0;
                if (fpdiv2rowcount <= 36)
                {
                    studinfoss = mydoc.NewTable(Fontsmall1, 36, 8, 3);
                }
                else
                {
                    rowscc = fpdiv2rowcount;
                    rowscc = rowscc - 36;
                    studinfoss = mydoc.NewTable(Fontsmall1, 36, 8, 3);
                }
                int rowtable1 = 0;
                int rowtable2 = 1;

                studinfoss.Columns[0].SetWidth(7);
                studinfoss.Columns[1].SetWidth(35);
                studinfoss.Columns[2].SetWidth(8);
                studinfoss.Columns[3].SetWidth(8);
                studinfoss.Columns[4].SetWidth(7);
                studinfoss.Columns[5].SetWidth(35);
                studinfoss.Columns[6].SetWidth(10);
                studinfoss.Columns[7].SetWidth(10);
                studinfoss.Cell(35, 0).SetContent("");
                studinfoss.Cell(35, 4).SetContent("");
                int sno = 0;
                // studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                for (int studcount = 0; studcount < FpSpread1.Sheets[0].RowCount; studcount++)
                {
                    string tabl1stend = studinfoss.Cell(35, 0).Content.ToString();
                    string tabl2stend = studinfoss.Cell(35, 4).Content.ToString();
                    if (tabl1stend.Trim() == "" && tabl2stend.Trim() == "")
                    {
                        //studinfoss.Cell(rowtable1, 0).SetContent(FpSpread1.Sheets[0].Cells[studcount, 0].Text.ToString());
                        studinfoss.Cell(rowtable1, 1).SetContent(FpSpread1.Sheets[0].Cells[studcount, 1].Text.ToString());
                        studinfoss.Cell(rowtable1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(rowtable1, 2).SetContent(FpSpread1.Sheets[0].Cells[studcount, 2].Text.ToString());
                        studinfoss.Cell(rowtable1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(rowtable1, 3).SetContent(FpSpread1.Sheets[0].Cells[studcount, 3].Text.ToString());
                        studinfoss.Cell(rowtable1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        int totalstudents = 0;
                        if (Int32.TryParse(FpSpread1.Sheets[0].Cells[studcount, 0].Text.ToString(), out totalstudents))
                        {
                            studinfoss.Cell(rowtable1, 0).SetContent(Convert.ToString(sno));
                            studinfoss.Cell(rowtable1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        }
                        else
                        {
                            studinfoss.Cell(rowtable1, 0).SetContent(FpSpread1.Sheets[0].Cells[studcount, 0].Text.ToString());
                            foreach (PdfCell pr in studinfoss.CellRange(rowtable1, 0, rowtable1, 0).Cells)
                            {
                                pr.ColSpan = 2;
                            }
                            studinfoss.Cell(rowtable1, 0).SetFont(Fontsmall1bold);
                            studinfoss.Cell(rowtable1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            studinfoss.Cell(rowtable1, 2).SetContent("L");
                            studinfoss.Cell(rowtable1, 3).SetContent("H");
                            sno = 0;
                        }
                        rowtable1++;
                    }
                    else if (tabl1stend.Trim() != "" && tabl2stend.Trim() == "")
                    {
                        studinfoss.Cell(0, 6).SetContent("L");
                        studinfoss.Cell(0, 7).SetContent("H");
                        //studinfoss.Cell(rowtable2, 4).SetContent(FpSpread1.Sheets[0].Cells[studcount, 0].Text.ToString());
                        studinfoss.Cell(rowtable2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(rowtable2, 5).SetContent(FpSpread1.Sheets[0].Cells[studcount, 1].Text.ToString());
                        studinfoss.Cell(rowtable2, 5).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(rowtable2, 6).SetContent(FpSpread1.Sheets[0].Cells[studcount, 2].Text.ToString());
                        studinfoss.Cell(rowtable2, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(rowtable2, 7).SetContent(FpSpread1.Sheets[0].Cells[studcount, 3].Text.ToString());
                        studinfoss.Cell(rowtable2, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                        int totalstudents = 0;
                        if (Int32.TryParse(FpSpread1.Sheets[0].Cells[studcount, 0].Text.ToString(), out totalstudents))
                        {
                            studinfoss.Cell(rowtable2, 4).SetContent(Convert.ToString(sno));
                        }
                        else
                        {
                            studinfoss.Cell(rowtable2, 4).SetContent(FpSpread1.Sheets[0].Cells[studcount, 0].Text.ToString());
                            foreach (PdfCell pr in studinfoss.CellRange(rowtable2, 4, rowtable2, 4).Cells)
                            {
                                pr.ColSpan = 2;
                            }
                            studinfoss.Cell(rowtable2, 4).SetFont(Fontsmall1bold);
                            studinfoss.Cell(rowtable2, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                            studinfoss.Cell(rowtable2, 6).SetContent("L");
                            studinfoss.Cell(rowtable2, 7).SetContent("H");
                            sno = 0;
                        }
                        rowtable2++;
                    }
                    else if (tabl1stend.Trim() != "" && tabl2stend.Trim() != "")
                    {
                        Gios.Pdf.PdfTablePage addtabletopage = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 100, 553, 800));
                        mypdfpage.Add(addtabletopage);
                        mypdfpage.SaveToDocument();
                        mypdfpage = mydoc.NewPage();
                        collinfo = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][0].ToString() + "");
                        mypdfpage.Add(collinfo);
                        //collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][1].ToString() + "");
                        //mypdfpage.Add(collinfo);
                        collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
                        mypdfpage.Add(collinfo);
                        //collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 70, 595, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  : " + ds.Tables[0].Rows[0][6].ToString());
                        //mypdfpage.Add(collinfo);
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                        {
                            Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 20, 20, 450);
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                        {
                            Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                            mypdfpage.Add(LogoImage1, 500, 20, 450);
                        }
                        mypdfpage.Add(border);
                        studinfoss = mydoc.NewTable(Fontsmall1, 36, 8, 3);
                        studinfoss.VisibleHeaders = false;
                        studinfoss.SetBorders(Color.Black, 1, BorderType.None);
                        studinfoss.Columns[0].SetWidth(7);
                        studinfoss.Columns[1].SetWidth(35);
                        studinfoss.Columns[2].SetWidth(8);
                        studinfoss.Columns[3].SetWidth(8);
                        studinfoss.Columns[4].SetWidth(7);
                        studinfoss.Columns[5].SetWidth(35);
                        studinfoss.Columns[6].SetWidth(10);
                        studinfoss.Columns[7].SetWidth(10);
                        rowtable1 = 0;
                        studinfoss.Cell(35, 0).SetContent("");
                        studinfoss.Cell(35, 4).SetContent("");
                        studinfoss.Cell(rowtable1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(rowtable1, 1).SetContent(FpSpread1.Sheets[0].Cells[studcount, 1].Text.ToString());
                        studinfoss.Cell(rowtable1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(rowtable1, 2).SetContent(FpSpread1.Sheets[0].Cells[studcount, 2].Text.ToString());
                        studinfoss.Cell(rowtable1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(rowtable1, 3).SetContent(FpSpread1.Sheets[0].Cells[studcount, 3].Text.ToString());
                        studinfoss.Cell(rowtable1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        int totalstudents = 0;
                        if (Int32.TryParse(FpSpread1.Sheets[0].Cells[studcount, 0].Text.ToString(), out totalstudents))
                        {
                            studinfoss.Cell(rowtable1, 0).SetContent(Convert.ToString(sno));
                            studinfoss.Cell(rowtable1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        }
                        else
                        {
                            studinfoss.Cell(rowtable1, 0).SetContent(FpSpread1.Sheets[0].Cells[studcount, 0].Text.ToString());
                            foreach (PdfCell pr in studinfoss.CellRange(rowtable1, 0, rowtable1, 0).Cells)
                            {
                                pr.ColSpan = 2;
                            }
                            studinfoss.Cell(rowtable1, 0).SetFont(Fontsmall1bold);
                            studinfoss.Cell(rowtable1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            studinfoss.Cell(rowtable1, 2).SetContent("L");
                            studinfoss.Cell(rowtable1, 3).SetContent("H");
                            sno = 0;
                        }
                        rowtable1++;
                        rowtable2 = 1;
                    }
                    sno++;
                }
                Gios.Pdf.PdfTablePage addtabletopage11 = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 100, 553, 800));
                mypdfpage.Add(addtabletopage11);
                mypdfpage.SaveToDocument();
                #endregion
            }
            else
            {
                #region attendancesheet
                mypdfpage = mydoc.NewPage();
                //Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                string sql = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                DataSet ds = new DataSet();
                ds.Clear();
                ds.Dispose();
                ds = da.select_method_wo_parameter(sql, "Text");
                PdfTextArea collinfo = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][0].ToString() + "");
                mypdfpage.Add(collinfo);
                //collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][1].ToString() + "");
                //mypdfpage.Add(collinfo);
                string address = ds.Tables[0].Rows[0][2].ToString() + "" + ds.Tables[0].Rows[0][3].ToString() + "" + ds.Tables[0].Rows[0][4].ToString();
                collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
                mypdfpage.Add(collinfo);
                //collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 70, 595, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  :" + ds.Tables[0].Rows[0][6].ToString());
                //mypdfpage.Add(collinfo);
                PdfArea pa1 = new PdfArea(mydoc, 14, 12, 564, 821);
                PdfRectangle border = new PdfRectangle(mydoc, pa1, Color.Black);
                mypdfpage.Add(border);
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                {
                    Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                    mypdfpage.Add(LogoImage, 20, 20, 450);
                }
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                {
                    Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                    mypdfpage.Add(LogoImage1, 500, 20, 450);
                }
                Gios.Pdf.PdfTable studinfoss;
                int fpdiv2rowcount = FpSpread1.Sheets[0].RowCount / 2;
                int rowscc = 0;
                if (fpdiv2rowcount <= 36)
                {
                    studinfoss = mydoc.NewTable(Fontsmall1, 36, 8, 3);
                }
                else
                {
                    rowscc = fpdiv2rowcount;
                    rowscc = rowscc - 36;
                    studinfoss = mydoc.NewTable(Fontsmall1, 36, 8, 3);
                }
                int rowtable1 = 0;
                int rowtable2 = 1;//control coming here
                studinfoss.Columns[0].SetWidth(3);
                studinfoss.Columns[1].SetWidth(16);
                studinfoss.Columns[2].SetWidth(35);
                studinfoss.Columns[3].SetWidth(5);

                studinfoss.Columns[4].SetWidth(3);
                studinfoss.Columns[5].SetWidth(16);
                studinfoss.Columns[6].SetWidth(35);
                studinfoss.Columns[7].SetWidth(5);

                studinfoss.Cell(35, 0).SetContent("");
                studinfoss.Cell(35, 4).SetContent("");
                // studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                for (int studcount = 0; studcount < FpSpread1.Sheets[0].RowCount; studcount++)
                {
                    string tabl1stend = studinfoss.Cell(35, 0).Content.ToString();
                    string tabl2stend = studinfoss.Cell(35, 4).Content.ToString();
                    if (tabl1stend.Trim() == "" && tabl2stend.Trim() == "")
                    {
                        studinfoss.Cell(rowtable1, 0).SetContent(FpSpread1.Sheets[0].Cells[studcount, 0].Text.ToString());
                        int totalstudents = 0;
                        if (Int32.TryParse(FpSpread1.Sheets[0].Cells[studcount, 0].Text.ToString(), out totalstudents))
                        {
                            studinfoss.Cell(rowtable1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        }
                        else
                        {
                            foreach (PdfCell pr in studinfoss.CellRange(rowtable1, 0, rowtable1, 0).Cells)
                            {
                                pr.ColSpan = 4;
                            }
                            studinfoss.Cell(rowtable1, 0).SetFont(Fontsmall1bold);
                            studinfoss.Cell(rowtable1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        }
                        // studinfoss.Cell(rowtable1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(rowtable1, 2).SetContent(FpSpread1.Sheets[0].Cells[studcount, 2].Text.ToString());
                        studinfoss.Cell(rowtable1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(rowtable1, 1).SetContent(FpSpread1.Sheets[0].Cells[studcount, 1].Text.ToString());
                        studinfoss.Cell(rowtable1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(rowtable1, 3).SetContent(FpSpread1.Sheets[0].Cells[studcount, 4].Text.ToString().ToUpper());
                        studinfoss.Cell(rowtable1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        //studinfoss.Cell(rowtable1, 3).SetContent(FpSpread1.Sheets[0].Cells[studcount, 3].Text.ToString());
                        //studinfoss.Cell(rowtable1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        rowtable1++;
                    }
                    else if (tabl1stend.Trim() != "" && tabl2stend.Trim() == "")
                    {
                        studinfoss.Cell(rowtable2, 4).SetContent(FpSpread1.Sheets[0].Cells[studcount, 0].Text.ToString());
                        studinfoss.Cell(rowtable2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        int totalstudents = 0;
                        if (Int32.TryParse(FpSpread1.Sheets[0].Cells[studcount, 0].Text.ToString(), out totalstudents))
                        {
                        }
                        else
                        {
                            foreach (PdfCell pr in studinfoss.CellRange(rowtable2, 4, rowtable2, 4).Cells)
                            {
                                pr.ColSpan = 4;
                            }
                            studinfoss.Cell(rowtable2, 4).SetFont(Fontsmall1bold);
                            studinfoss.Cell(rowtable2, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                        }
                        studinfoss.Cell(rowtable2, 5).SetContent(FpSpread1.Sheets[0].Cells[studcount, 1].Text.ToString());
                        studinfoss.Cell(rowtable2, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(rowtable2, 6).SetContent(FpSpread1.Sheets[0].Cells[studcount, 2].Text.ToString());
                        studinfoss.Cell(rowtable2, 6).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(rowtable2, 7).SetContent(FpSpread1.Sheets[0].Cells[studcount, 4].Text.ToString().ToUpper());
                        studinfoss.Cell(rowtable2, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                        rowtable2++;
                    }
                    else if (tabl1stend.Trim() != "" && tabl2stend.Trim() != "")
                    {
                        Gios.Pdf.PdfTablePage addtabletopage = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 100, 553, 800));
                        mypdfpage.Add(addtabletopage);
                        mypdfpage.SaveToDocument();
                        mypdfpage = mydoc.NewPage();
                        collinfo = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][0].ToString() + "");
                        mypdfpage.Add(collinfo);
                        //collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0][1].ToString() + "");
                        //mypdfpage.Add(collinfo);
                        collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + address);
                        mypdfpage.Add(collinfo);
                        //collinfo = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 70, 595, 50), System.Drawing.ContentAlignment.TopCenter, "E-mail  : " + ds.Tables[0].Rows[0][6].ToString());
                        //mypdfpage.Add(collinfo);
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                        {
                            Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 20, 20, 450);
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                        {
                            Gios.Pdf.PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                            mypdfpage.Add(LogoImage1, 500, 20, 450);
                        }
                        mypdfpage.Add(border);
                        studinfoss = mydoc.NewTable(Fontsmall1, 36, 8, 3);
                        studinfoss.VisibleHeaders = false;
                        //studinfoss.SetBorders(Color.Black, 1, BorderType.None);
                        //  studinfoss.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        studinfoss.Columns[0].SetWidth(3);
                        studinfoss.Columns[1].SetWidth(16);
                        studinfoss.Columns[2].SetWidth(35);
                        studinfoss.Columns[3].SetWidth(5);

                        studinfoss.Columns[4].SetWidth(3);
                        studinfoss.Columns[5].SetWidth(16);
                        studinfoss.Columns[6].SetWidth(35);
                        studinfoss.Columns[7].SetWidth(5);
                        rowtable1 = 0;
                        studinfoss.Cell(35, 0).SetContent("");
                        studinfoss.Cell(35, 4).SetContent("");
                        studinfoss.Cell(rowtable1, 0).SetContent(FpSpread1.Sheets[0].Cells[studcount, 0].Text.ToString());
                        studinfoss.Cell(rowtable1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        int totalstudents = 0;
                        if (Int32.TryParse(FpSpread1.Sheets[0].Cells[studcount, 0].Text.ToString(), out totalstudents))
                        {
                            studinfoss.Cell(rowtable1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        }
                        else
                        {
                            foreach (PdfCell pr in studinfoss.CellRange(rowtable1, 0, rowtable1, 0).Cells)
                            {
                                pr.ColSpan = 2;
                            }
                            studinfoss.Cell(rowtable1, 0).SetFont(Fontsmall1bold);
                            studinfoss.Cell(rowtable1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        }
                        studinfoss.Cell(rowtable1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(rowtable1, 2).SetContent(FpSpread1.Sheets[0].Cells[studcount, 2].Text.ToString());
                        studinfoss.Cell(rowtable1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                        studinfoss.Cell(rowtable1, 1).SetContent(FpSpread1.Sheets[0].Cells[studcount, 1].Text.ToString());
                        studinfoss.Cell(rowtable1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        studinfoss.Cell(rowtable1, 3).SetContent(FpSpread1.Sheets[0].Cells[studcount, 4].Text.ToString().ToUpper());
                        studinfoss.Cell(rowtable1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        rowtable1++;
                        rowtable2 = 1;
                    }
                }
                Gios.Pdf.PdfTablePage addtabletopage11 = studinfoss.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 100, 553, 800));
                mypdfpage.Add(addtabletopage11);
                mypdfpage.SaveToDocument();
                #endregion
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "Marksheets" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                Response.Buffer = true;
                Response.Clear();
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch
        {
        }
    }

    protected void AttendancePercentage(string collegeCodeP, string degreeP, string semP, string rollnoP, string admDateP)
    {
        string SemInfoQry = "select semester,CONVERT(varchar(10), start_date,103)start_date,CONVERT(varchar(10), end_date,103)end_date,no_of_working_days from seminfo where degree_code=" + degreeP + " and semester =" + ddlSemYr.SelectedItem.Text + " and batch_year= " + ddlBatch.SelectedItem.Text + "  order by semester ";
        DataSet semdetailsDs = dirAccess.selectDataSet(SemInfoQry);
        string SemStartDate = string.Empty;
        string SemEndDate = string.Empty;
        if (semdetailsDs.Tables[0].Rows.Count > 0)
        {
            SemStartDate = Convert.ToString(semdetailsDs.Tables[0].Rows[0]["start_date"]);
            SemEndDate = Convert.ToString(semdetailsDs.Tables[0].Rows[0]["end_date"]);
        }


        string fdate = txtfdate.Text.ToString();
        string[] spf = fdate.Split('/');
        string tdate = txttodate.Text.ToString();
        string[] spt = tdate.Split('/');

        //string dt = SemStartDate;
        //string[] dsplit = dt.Split(new Char[] { '/' });
        //SemStartDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();

        string dt = fdate;
        string[] dsplit = dt.Split(new Char[] { '/' });
        SemStartDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();

        demfcal = int.Parse(dsplit[2].ToString());
        demfcal = demfcal * 12;
        cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
        cal_from_date_tmp = demfcal + int.Parse(dsplit[1].ToString());
        monthcal = cal_from_date.ToString();

        //dt = SemEndDate;
        //dsplit = dt.Split(new Char[] { '/' });
        //SemEndDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
        //demtcal = int.Parse(dsplit[2].ToString());
        //demtcal = demtcal * 12;
        //cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
        //cal_to_date_tmp = demtcal + int.Parse(dsplit[1].ToString());

        dt = tdate;
        dsplit = dt.Split(new Char[] { '/' });
        SemEndDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
        demtcal = int.Parse(dsplit[2].ToString());
        demtcal = demtcal * 12;
        cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
        cal_to_date_tmp = demtcal + int.Parse(dsplit[1].ToString());

        string startdate = spf[2] + '/' + spf[1] + '/' + spf[0];
        string enddate = spt[2] + '/' + spt[1] + '/' + spt[0];

        per_from_gendate = Convert.ToDateTime(SemStartDate);
        per_to_gendate = Convert.ToDateTime(SemEndDate);

        ArrayList arrDegree = new ArrayList();

        if (!arrDegree.Contains(degreeP))
        {
            hat.Clear();
            hat.Add("degree_code", degreeP);
            hat.Add("sem_ester", int.Parse(semP));
            ds = d2.select_method("period_attnd_schedule", hat, "sp");
            if (ds.Tables[0].Rows.Count != 0)
            {
                NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                minpresday = int.Parse(ds.Tables[0].Rows[0]["MIN PREE PER DAY"].ToString());
            }
            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
            count = ds1.Tables[0].Rows.Count;
            arrDegree.Add(degreeP);
        }
        //persentmonthcal(collegeCodeP, degreeP, semP, rollnoP, admDateP, startdate, enddate );

        persentmonthcal(collegeCodeP, degreeP, semP, rollnoP, admDateP, SemStartDate, SemEndDate);

        conductedDays = per_workingdays;
        presentDays = pre_present_date;

        if (per_workingdays > 0)
        {
            absentDays = per_workingdays - pre_present_date;
        }
        if (per_workingdays > 0)
        {
            per_tage_date = ((pre_present_date / per_workingdays) * 100);
            absentDaysPercentage = ((absentDays / per_workingdays) * 100);
        }
        else
        {
            per_tage_date = 0;
            absentDaysPercentage = 0;
        }
        conductedHours = per_workingdays1;
        presentHours = per_per_hrs;
        if (per_workingdays1 > 0)
        {
            absentHours = per_workingdays1 - per_per_hrs;
        }
        if (per_workingdays1 > 0)
        {
            per_tage_hrs = (((per_per_hrs) / (per_workingdays1)) * 100);
            absentHoursPercentage = ((absentHours / per_workingdays1) * 100);
        }
        else
        {
            per_tage_hrs = 0;
            absentHoursPercentage = 0;
        }
        if (per_tage_date.ToString() == "NaN")
        {
            per_tage_date = 0;
        }
        else if (per_tage_date.ToString() == "Infinity")
        {
            per_tage_date = 0;
        }
        if (per_tage_date > 100)
        {
            per_tage_date = 100;
        }
        per_tage_date = Math.Round(per_tage_date, 2);
        absentDaysPercentage = Math.Round(absentDaysPercentage, 2);
        per_tage_hrs = Math.Round(per_tage_hrs, 2);
        absentHoursPercentage = Math.Round(absentHoursPercentage, 2);
        //dum_tage_hrs = per_tage_hrs.ToString();
        dum_tage_date = String.Format("{0:0,0.00}", float.Parse(Convert.ToString(per_tage_date).Trim()));
        dum_tage_hrs = String.Format("{0:0,0.00}", float.Parse(Convert.ToString(per_tage_hrs).Trim()));
        absentDaysPercentage1 = String.Format("{0:0,0.00}", float.Parse(Convert.ToString(absentDaysPercentage).Trim()));
        absentHoursPercentage1 = String.Format("{0:0,0.00}", float.Parse(Convert.ToString(absentHoursPercentage).Trim()));
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
        double compareValue = 0;
        if (rblHrDaywise.SelectedIndex == 0)
        {
            //if (rblPercDays.SelectedIndex == 0)
            //{
            //    compareValue = per_tage_date;
            //}
            //else
            //{
            compareValue = presentDays;
            //}
        }
        else
        {
            compareValue = per_tage_hrs;
        }
    }

    public void persentmonthcal(string collegeCode, string degree, string sem, string rollno, string admitDate, string SemStartDate, string SemEndDate)
    {
        medicalLeaveCountPerSession = 0;
        bool isadm = false;
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
        string admdate = admitDate;
        DateTime.TryParseExact(admdate, "dd/MM/yyyy", null, DateTimeStyles.None, out Admission_date);
        dd = rollno.Trim();
        hat.Clear();
        hat.Add("std_rollno", rollno.Trim());
        hat.Add("from_month", cal_from_date);
        hat.Add("to_month", cal_to_date);
        ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");   //no rows found in the rsult of this SP
        mmyycount = ds2.Tables[0].Rows.Count;
        moncount = mmyycount - 1;
        if (deptflag == false)
        {
            deptflag = true;
            hat.Clear();
            hat.Add("degree_code", int.Parse(Convert.ToString(degree).Trim()));
            hat.Add("sem", int.Parse(Convert.ToString(sem).Trim()));
            hat.Add("from_date", Convert.ToString(SemStartDate));
            hat.Add("to_date", Convert.ToString(SemEndDate));
            hat.Add("coll_code", int.Parse(Convert.ToString(collegeCode)));
            int iscount = 0;
            string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + SemStartDate.ToString() + "' and '" + SemEndDate.ToString() + "' and degree_code=" + degree + " and semester=" + sem;
            DataSet dsholiday = d2.select_method_wo_parameter(sqlstr_holiday, "Text");
            if (dsholiday.Tables[0].Rows.Count > 0)
            {
                iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
            }
            hat.Add("iscount", iscount);
            ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
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
                                    value_holi_status = holiday_table11[(Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()].ToString();//dummy_split[1].ToString() + "/" + dummy_split[0].ToString() + "/" + dummy_split[2].ToString()
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
                                        //magesh 29/1/18
                                        if (per_ondu >= 1)
                                        {
                                            Onduty += 0.5;
                                        }//magesh 29/1/18
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
                                        //magesh 29/1/18
                                        if (per_ondu >= 1)
                                        {
                                            Onduty += 0.5;
                                        }//magesh 29/1/18
                                        if (medicalLeaveCountPerSession + njhr >= minpresII)
                                        {
                                            medicalLeaveDays = medicalLeaveDays + 0.5;
                                        }
                                        if (Session["attdaywisecla"] != null && Session["attdaywisecla"].ToString() == "1")
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

        per_con_hrs = per_workingdays1;//added 080812//my

        per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / (per_con_hrs + tot_conduct_hr_spl_fals)) * 100);



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

    protected void btnRemarksave_Click(object sender, EventArgs e)
    {
        int ins = 0;
        string strinsupdaval = string.Empty;

        Hashtable eligibiltyCriteriaHash = new Hashtable();
        eligibiltyCriteriaHash.Add("NE", 3);
        eligibiltyCriteriaHash.Add("CAG", 2);
        eligibiltyCriteriaHash.Add("EC", 1);

        FpSpread1.SaveChanges();

        for (int r = 0; r < FpSpread1.Sheets[0].RowCount; r++)
        {

            string eligibiltyCriteria = "1";
            string rollNo = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 0].Tag).Trim();
            string app_no = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 1].Note).Trim();
            if (!string.IsNullOrEmpty(rollNo.Trim()))
            {
                string Remarks = Convert.ToString(FpSpread1.Sheets[0].Cells[r, 4].Text).Trim().ToUpper();
                if (eligibiltyCriteriaHash.Contains(Remarks))
                    eligibiltyCriteria = eligibiltyCriteriaHash[Remarks].ToString();

                string name = Convert.ToString(FpSpread1.Sheets[0].GetText(r, 2)).Trim();


                string batch = Convert.ToString(FpSpread1.Sheets[0].GetTag(r, 1)).Trim();
                string sem1 = Convert.ToString(ddlSemYr.SelectedItem.Value);//Convert.ToString(FpSpread1.Sheets[0].GetTag(r, 2)).Trim();
                string degcode = Convert.ToString(FpSpread1.Sheets[0].GetNote(r, 2)).Trim();

                strinsupdaval = " if exists(select * from Eligibility_list where Semester='" + sem1 + "' and app_no='" + app_no + "' and degree_code='" + degcode + "' and batch_year='" + batch + "') update Eligibility_list set Remarks='" + Remarks + "' where batch_year='" + batch + "' and Semester='" + sem1 + "' and degree_code='" + degcode + "' and app_no='" + app_no + "'  else  insert into Eligibility_list(Roll_no,stud_name,is_eligible,batch_year,Semester,degree_code,app_no,Remarks) values('" + rollNo + "','" + name + "','" + eligibiltyCriteria + "','" + batch + "','" + sem1 + "','" + degcode + "','" + app_no + "','" + Remarks + "')";
                ins = dirAccess.insertData(strinsupdaval);
            }
        }
        if (ins != 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
        }
    }

    protected void rblHrDaywiseClick(object sender, EventArgs e)
    {
        btngo_Click(sender, e);
    }

    #region for date filter added on 1/12/2017

    protected void txtfdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string fdate = txtfdate.Text.ToString();
            string[] spf = fdate.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string tdate = txttodate.Text.ToString();
            if (!string.IsNullOrEmpty(tdate))
            {
                string[] spt = tdate.Split('/');
                DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
                if (dtt < dtf)
                {
                    txtfdate.Text = DateTime.Today.ToString("d/MM/yyyy");
                    txttodate.Text = DateTime.Today.ToString("d/MM/yyyy");
                    divPopUpAlert.Visible = true;
                    lblAlertMsg.Text = "Please Enter The From Date Must Be Lesser Than Or Equal To Date";
                }
            }
        }
        catch (Exception ex)
        {
            divPopUpAlert.Visible = true;
            lblAlertMsg.Text = ex.ToString();
        }
    }

    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string fdate = txtfdate.Text.ToString();
            string[] spf = fdate.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string tdate = txttodate.Text.ToString();
            string[] spt = tdate.Split('/');
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            if (dtt < dtf)
            {
                txtfdate.Text = DateTime.Today.ToString("d/MM/yyyy");
                txttodate.Text = DateTime.Today.ToString("d/MM/yyyy");
                divPopUpAlert.Visible = true;
                lblAlertMsg.Text = "Please Enter The From Date Must Be Lesser Than Or Equal To Date";
            }
        }
        catch (Exception ex)
        {
            divPopUpAlert.Visible = true;
            lblAlertMsg.Text = ex.ToString();
        }
    }

    protected void btnPopUpAlertClose_Click(object sender, EventArgs e)
    {
        divPopUpAlert.Visible = false;
        lblAlertMsg.Text = string.Empty;
    }

    #endregion

}