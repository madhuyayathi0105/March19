#region Namespace Declaration

using System;
using System.Collections.Generic;
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

public partial class ReportCard_CBSE : System.Web.UI.Page
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

    DataSet dsdel = new DataSet();


    DataTable dtallcol = new DataTable();
    DataTable dtFASAcol = new DataTable();
    DataTable dtallotherscol = new DataTable();

    DataSet otherds_subject = new DataSet();
    DataSet ds_subject = new DataSet();

    ArrayList faillist = new ArrayList();
    ArrayList subfaillist = new ArrayList();
    ArrayList termselected = new ArrayList();
    ArrayList avoidrows = new ArrayList();
    ArrayList avg_grade_col = new ArrayList();

    Farpnt.FpSpread fpspread11 = new Farpnt.FpSpread();
    Boolean booleanheaderformat1 = true;

    TreeNode node;
    TreeNode subchildnode;

    double cgpacalc = 0;
    int twosubcount = 0;

    string otherssubjectcode = "";
    string otherssubjectcode01 = "";
    string sql = "";
    string sqlcondition = "";
    string collcode = "";
    string batchyear = "";
    string degreecode = "";
    string term = "";
    string sec = "";

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
                loadheader();
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

    public void loadheader()
    {
        try
        {
            string batch_year = Convert.ToString(ddlbatch.SelectedItem.Text);
            string degree_code = Convert.ToString(ddlDept.SelectedItem.Value);
            string buildvalue1 = "";
            ds.Reset();
            ds.Dispose();
            treeview_spreadfields.Nodes.Clear();
            for (int i = 0; i < ddlSem.Items.Count; i++)
            {
                if (Convert.ToInt32(Convert.ToString(ddlSem.SelectedItem.Text)) > i)
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
            ds = d2.select_method_wo_parameter(straccheadquery, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    node = new TreeNode(Convert.ToString(ds.Tables[0].Rows[i]["semester"]), Convert.ToString(ds.Tables[0].Rows[i]["semester"]));
                    string strled = "SELECT distinct  Istype,CRITERIA_NO,y.semester,M.Conversion_value FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batch_year + "' and degree_code = '" + degree_code + "' and semester in ('" + Convert.ToString(ds.Tables[0].Rows[i]["semester"]) + "')  and CRITERIA_NO <>''  order by semester";
                    ds1 = d2.select_method_wo_parameter(strled, "Text");
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
            loadheader();
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
            loadheader();
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
            loadheader();
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
            loadheader();
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
            loadheader();
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
            loadheader();
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
                    bindbutn(rollnos.Trim().Trim(','));
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

    #region Grade Sheet

    //protected void btnGrade_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblErrSearch.Text = "";
    //        lblErrSearch.Visible = false;

    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;            
    //    }
    //}

    protected void btnGrade_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = "";

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
            rollnos = "";
            FpViewSpread.SaveChanges();
            int checkedcount = 0;
            for (int i = 0; i < FpViewSpread.Sheets[0].RowCount; i++)
            {
                if (Convert.ToInt32(FpViewSpread.Sheets[0].Cells[i, 1].Value) == 1)
                {
                    checkedcount++;
                }
            }

            string parttitle1a = "";
            Boolean flag = true;
            ArrayList arrcourrid = new ArrayList();
            ArrayList partcolumnnames = new ArrayList();

            System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);

            for (int i = 0; i < FpViewSpread.Sheets[0].RowCount; i++)
            {
                if (Convert.ToInt32(FpViewSpread.Sheets[0].Cells[i, 1].Value) == 1)
                {
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

            if (rollnos != "")
            {
                sql = "select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No in ('" + rollnos + "') ;";
                studgradeds.Clear();
                studgradeds = d2.select_method_wo_parameter(sql, "text");
                if (studgradeds.Tables[0].Rows.Count > 0)
                {
                    bool isCamCal = false;
                    string errormsg = "";
                    for (int roll = 0; roll < studgradeds.Tables[0].Rows.Count; roll++)
                    {
                        string rcrollno = "";
                        rcrollno = Convert.ToString(studgradeds.Tables[0].Rows[roll][1]);
                        bindstudentmark(rcrollno);
                        // bindbutn(rcrollno);

                        isCamCal = false;

                        DataSet ds = new DataSet();
                        DataSet partsds = new DataSet();
                        DAccess2 da = new DAccess2();
                        string stdappno = "";

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
                            if (ddlSem.SelectedItem.Text == "1")
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
                            if (ddlSem.SelectedItem.Text == "1")
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

                            if (ddlSem.SelectedItem.Text == "1" && columncountspread == 5)
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
                            if (Convert.ToString(ddlSem.SelectedItem.Text).Trim() == "1" && columncountspread == 5)
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

                            if (Convert.ToString(ddlSem.SelectedItem.Text).Trim() == "2" && columncountspread == 13)
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


                            sql = " select  ca.CoCurr_ID,ca.Title_Name,tv.TextCode, tv.TextVal,ca.SubTitle from activity_entry ae,CoCurr_Activitie ca,textvaltable tv where ae.CoCurr_ID=ca.CoCurr_ID and ae.Batch_Year=ca.Batch_Year and ae.Degree_Code=ca.Degree_Code and ae.Batch_Year='" + batchyear + "' and ae.Degree_Code='" + degreecode + "' and ae.term='" + Convert.ToString(ddlSem.SelectedItem) + "' and tv.TextCode=ae.ActivityTextVal  and ae.ActivityTextVal in (select ActivityTextVal from CoCurrActivitie_Det where Roll_No='" + Roll_No + "' and Degree_Code='" + degreecode + "' and Batch_Year='" + batchyear + "' and ae.term='" + Convert.ToString(ddlSem.SelectedItem) + "' and mark<>0)  order by SubTitle";

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
                                        string colheadername = "";
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
                                                    string sqlff = "";
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
                        lblErrSearch.Text = errormsg;
                        lblErrSearch.Visible = true;
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
                            lblErrSearch.Text = errormsg;
                            lblErrSearch.Visible = true;
                        }
                    }
                }
                else
                {
                    lblErrSearch.Text = "No Records Found";
                    lblErrSearch.Visible = true;
                }
            }
            else
            {
                lblErrSearch.Text = "Please Select Any One Record";
                lblErrSearch.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion

    #endregion Button Click

    #region ReportCard

    public void bindbutn(string rollno)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = "";

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
                qry = "select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile,parentF_Mobile,bldgrp from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No in (" + rollnos + ") ;";
                studgradeds.Clear();
                studgradeds = d2.select_method_wo_parameter(qry, "text");
                if (studgradeds.Tables[0].Rows.Count > 0)
                {

                    for (int roll = 0; roll < studgradeds.Tables[0].Rows.Count; roll++)
                    {
                        string rcrollno = "";
                        rcrollno = Convert.ToString(studgradeds.Tables[0].Rows[roll][1]);
                        bindstudentmark(rcrollno);
                        //bindbutn(rcrollno);
                        // bindrptcard(rcrollno);

                        DataSet ds = new DataSet();
                        DataSet dschool = new DataSet();
                        DAccess2 da = new DAccess2();
                        DataSet dset = new DataSet();
                        string college_code = collegecode;
                        string stdappno = "";
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
                        qry = "SELECT collname,affliatedby,address1,address2,address3,pincode,email,logo1,logo2,email  from collinfo ;select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name,r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile,parentF_Mobile,StuPer_Id,parent_addressc,StudHeight,StudWeight,VisionLeft,VisionRight,DentalHygiene,Goals,Strenghts,ExcepAchieve,bldgrp,studhouse from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Roll_No='" + Roll_No + "'";
                        ds.Clear();
                        ds.Dispose();
                        ds = da.select_method_wo_parameter(qry, "Text");
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

                            string stdcc = "";
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
                            PdfTextArea pdf119zzzzz = new PdfTextArea(f3, System.Drawing.Color.Black, new PdfArea(mydoc, 390, 170, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Class" + "                               " + Convert.ToString(ddlDept.SelectedItem.Text) + " ");
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
                            if (ddlSem.SelectedItem.Text == "1")
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
                            if (ddlSem.SelectedItem.Text == "1")
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
                            if (ddlSem.SelectedItem.Text == "1")
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


                            if (ddlSem.SelectedItem.Text == "1")
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



                            if (Convert.ToString(ddlSem.SelectedItem.Text).Trim() == "1")
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

                            if (Convert.ToString(ddlSem.SelectedItem.Text).Trim() == "2")
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

                                if (twosubcount > 0 && ddlSem.SelectedItem.Text == "2")
                                {
                                    //table1forpage2.Cell(rowcountspread - 2, 9).SetContent(Convert.ToString(finalatt + "/" + finalwholeatt));
                                    //table1forpage2.Cell(rowcountspread - 2, 10).SetContent(Convert.ToString(Convert.ToString(strformate) + "%"));
                                }

                                table1forpage2.Cell(rowcountspread - 2, 11).SetContent(Convert.ToString("CGPA"));

                                if (twosubcount > 0 && ddlSem.SelectedItem.Text == "2")
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
                            if (ddlSem.SelectedItem.Text == "2")
                            {
                                //PdfTextArea pdf28 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black, new PdfArea(mydoc, 80, page2col, 595, 50), System.Drawing.ContentAlignment.TopLeft, "Cumulative Grade Point Average (CGPA)");
                                //mypdfpage1.Add(pdf28);
                            }



                            page2col = page2col + 30;
                            PdfArea overallgradepa1 = new PdfArea(mydoc, 70, page2col, 220, 28);
                            PdfRectangle overallgradepa1pr3 = new PdfRectangle(mydoc, overallgradepa1, Color.Black);

                            page2col = page2col + 5;
                            if (ddlSem.SelectedItem.Text == "2")
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

                            if (ddlSem.SelectedItem.Text == "2")
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


                            qry = " select  ca.CoCurr_ID,ca.Title_Name,tv.TextCode, tv.TextVal,ca.SubTitle from activity_entry ae,CoCurr_Activitie ca,textvaltable tv where ae.CoCurr_ID=ca.CoCurr_ID and ae.Batch_Year=ca.Batch_Year and ae.Degree_Code=ca.Degree_Code and ae.Batch_Year='" + batchyear + "' and ae.Degree_Code='" + degreecode + "' and term='2' and tv.TextCode=ae.ActivityTextVal  and ae.ActivityTextVal in (select ActivityTextVal from CoCurrActivitie_Det where Roll_No='" + Roll_No + "' and Degree_Code='" + degreecode + "' and Batch_Year='" + batchyear + "' and mark<>0 )  order by SubTitle";

                            DataSet partsds = new DataSet();
                            ArrayList arrcourrid = new ArrayList();
                            string parttitle1a = "";
                            Boolean flag = true;
                            ArrayList partcolumnnames = new ArrayList();
                            partsds.Clear();
                            partsds = d2.select_method_wo_parameter(qry, "Text");

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
                                        qry = "select IsActivity,IsActDesc,IsGrade  from CoCurr_Activitie where CoCurr_ID='" + courrid + "' ";
                                        ds.Clear();
                                        ds = d2.select_method_wo_parameter(qry, "Text");
                                        int colcountpart = 0;
                                        string colheadername = "";
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
                                                    string sqlff = "";
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

                            if (ddlSem.SelectedItem.Text == "2")
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
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
        lblErrSearch.Visible = false;
        lblErrSearch.Text = "";
        string termselectf1 = Convert.ToString(ddlSem.SelectedItem.Text);
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

        string otherssubject_sql = "";
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

        otherssubjectcode = "";

        string fasaCRITERIA_NO = "";
        double fatotal = 0;
        //double satotal = 0;
        //double fulltotal = 0;
        double maxfatotal = 0;
        double maxsatotal = 0;
        double maxfulltotal = 0;
        // collcode = " and r.college_code='" + Convert.ToString(ddschool.SelectedItem.Value) + "'";
        batchyear = "  and y.Batch_Year='" + Convert.ToString(ddlbatch.SelectedItem.Text) + "'";
        degreecode = "  and degree_code='" + Convert.ToString(ddlDept.SelectedItem.Value) + "'";
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

        for (int i = 0; i < cblterm.Items.Count; i++)
        {
            if (cblterm.Items[i].Selected == true)
            {
                term = " and semester in ('" + Convert.ToString(cblterm.Items[i].Text) + "')";
                otherssubjectcode = "";
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
                    otherssubjectcode = "";
                    otherssubjectcode01 = "";
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
                            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Tag = "";
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
                            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Tag = "";
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
            lblErrSearch.Visible = false;
            lblErrSearch.Text = "";
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
            batchyear = Convert.ToString(ddlbatch.SelectedItem.Text);
            degreecode = Convert.ToString(ddlDept.SelectedItem.Value);
            //term = ddlSem.SelectedItem.Text;
            string selterm = "";
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
            double overallfa = 0;
            double overallsa = 0;

            double overallconfa = 0;
            double overallconsa = 0;

            int count = dtallcol.Rows.Count;
            //if (count > 0)
            //{
            //}
            //return;
            string admdate = "";
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

                    FpViewSpread.SaveChanges();

                    overalltotalall = 0;


                }

            }
            FpViewSpread.SaveChanges();


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

    public string loadmarkat(string mr)
    {
        string strgetval = "";
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = "";
            strgetval = "";
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

    #endregion Reusable Methods

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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

}