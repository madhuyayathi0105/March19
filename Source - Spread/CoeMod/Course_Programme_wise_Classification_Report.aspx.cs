#region Namespace Declaration

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls;
using Farpoint = FarPoint.Web.Spread;
using wc = System.Web.UI.WebControls;
using Gios.Pdf;
using System.Web;
using System.IO;
using System.Globalization;
using System.Data.SqlClient;
using System.Configuration;

#endregion Namespace Declaration


public partial class CoeMod_Course_Programme_wise_Classification_Report : System.Web.UI.Page
{
    #region Field Declaration
    bool check_col_count_flag = false;
    bool btnclick_or_print = false;
    bool recflag = false;
    bool check_alter = false;
    bool chkflag = false;
    bool splhr_flag = false;
    bool isSchool = false;

    double max_tot = 0;
    double attnd_hr = 0, tot_hr = 0;

    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DAccess2 da2 = new DAccess2();
    DAccess2 da22 = new DAccess2();
    DAccess2 d2 = new DAccess2();
    Hashtable ht = new Hashtable();

    Institution institute;

    string userCode = string.Empty;
    string collegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;

    int selected = 0;

    string collegeCodes = string.Empty;
    string collegeNames = string.Empty;
    string streamNames = string.Empty;
    string courseTypes = string.Empty;
    string eduLevels = string.Empty;
    string courseIds = string.Empty;
    string courseNames = string.Empty;
    string batchYears = string.Empty;
    string degreeCodes = string.Empty;
    string departmentNames = string.Empty;
    string semesters = string.Empty;
    string sections = string.Empty;
    string subjectTypes = string.Empty;
    string subjectNames = string.Empty;
    string subjectNos = string.Empty;
    string subjectCodes = string.Empty;
    string examMonth = string.Empty;
    string examYear = string.Empty;
    string fromDate = string.Empty;
    string toDate = string.Empty;
    string strsec = string.Empty;
    string rstrsec = string.Empty;
    string section_lab = string.Empty;

    DateTime dtSemStartDate = new DateTime();
    DateTime dtSemEndDate = new DateTime();
    DateTime dtTempDate = new DateTime();
    DateTime dtAdmissionDate = new DateTime();
    DateTime temp_date = new DateTime();
    DateTime Admission_date = new DateTime();
    DateTime dtFromDate = new DateTime();
    DateTime dtToDate = new DateTime();
    DateTime dtDummyDate = new DateTime();

    string qry = string.Empty;
    string qryCollege = string.Empty;
    string qryStream = string.Empty;
    string qryEduLevel = string.Empty;
    string qryCourseId = string.Empty;
    string qryBatch = string.Empty;
    string qryDegree = string.Empty;
    string qryDepartment = string.Empty;
    string qryDegreeCode = string.Empty;
    string qrySemester = string.Empty;
    string qrySection = string.Empty;
    string qrySubjectNos = string.Empty;
    string qrySubjectNames = string.Empty;
    string qrySubjectCodes = string.Empty;
    string qryExamMonth = string.Empty;
    string qryExamYear = string.Empty;
    string qryFromDate = string.Empty;
    string qryToDate = string.Empty;

    string qrySubjectFilter = string.Empty;

    Dictionary<string, DateTime> dicStudentAdmDate = new Dictionary<string, DateTime>();

    Hashtable hashmark = new Hashtable();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    Hashtable htSubjectType = new Hashtable();
    Hashtable hatattendance = new Hashtable();
    Hashtable hatdc = new Hashtable();
    Hashtable has_od = new Hashtable();
    Hashtable hat_holy = new Hashtable();
    Hashtable htHoliday = new Hashtable();
    Hashtable hatsplhrattendance = new Hashtable();
    Hashtable has_total_attnd_hour = new Hashtable();
    Hashtable has_load_rollno = new Hashtable();
    Hashtable htSplHr = new Hashtable();
    Hashtable has_attnd_masterset_notconsider = new Hashtable();
    Hashtable has_attnd_masterset = new Hashtable();
    Hashtable holiday = new Hashtable();
    Hashtable temp_has_subj_code = new Hashtable();
    static Hashtable has_subtype = new Hashtable();

    DataSet dsHolidayList = new DataSet();
    DataSet dsHolidays = new DataSet();
    DataSet dsSplHr = new DataSet();
    DataSet dsAttndanceMaster = new DataSet();
    DataSet dsAlterSchedule = new DataSet();
    DataSet dsSemesterSchedule = new DataSet();

    int noOfHours = 0;
    int firstHalf = 0;
    int secondHalf = 0;
    int mng_hrs = 0, evng_hrs = 0;
    int od_count = 0;
    int between_visible_col_cnt = 0, between_visible_col_cnt_bal = 0;
    int x = 0;
    int visi_col = 0, visi_col1 = 0;
    int count_master = 0;
    int start_column = 0, end_column = 0;
    int new_header_count = 0;
    int temp_count = 0, final_print_col_cnt = 0, split_col_for_footer = 0, col_count = 0, footer_balanc_col = 0, footer_count = 0;
    int col_count_all = 0, span_cnt = 0, child_span_count = 0;
    int split_holiday_status_1 = 0, split_holiday_status_2 = 0;
    int temp_stud_count = 0;
    int row_count = 0;
    int stud_count = 0;
    int span_count = 0;
    int present_count = 0;
    int roll_count = 0;

    string subject_no = string.Empty;
    string roll_no = string.Empty;
    string strDay = string.Empty;
    string dummy_date = string.Empty;
    string temp_hr_field = string.Empty;
    string full_hour = string.Empty;
    string single_hour = string.Empty;
    string order = string.Empty;
    string halforFull = string.Empty;
    string mng = string.Empty;
    string evng = string.Empty;
    string semStartDate = string.Empty;
    string holiday_sched_details = string.Empty;
    string new_header_string_index = string.Empty;
    string isonumber = string.Empty;
    string new_header_string = string.Empty;
    string column_field = string.Empty;
    string printvar = string.Empty;
    string view_footer = string.Empty;
    string view_header = string.Empty;
    string view_footer_text = string.Empty;
    string coll_name = string.Empty;
    string address1 = string.Empty;
    string address2 = string.Empty;
    string address3 = string.Empty;
    string form_name = string.Empty;
    string phoneno = string.Empty;
    string faxno = string.Empty;
    string email = string.Empty;
    string website = string.Empty;
    string degree_val = string.Empty;
    string footer_text = string.Empty;
    string header_alignment = string.Empty;
    string degree_deatil = string.Empty;
    string phone = string.Empty;
    string fax = string.Empty;
    string email_id = string.Empty;
    string web_add = string.Empty;
    string get_date_holiday = string.Empty;
    string subj_type = string.Empty;
    string date1 = string.Empty;
    string datefrom = string.Empty;
    string date2 = string.Empty;
    string dateto = string.Empty;
    string halforfull = string.Empty;
    string value_holi_status = string.Empty;
    string date_temp_field = string.Empty;
    string month_year = string.Empty;
    string Att_mark;
    string present_calcflag = string.Empty;
    static string grouporusercode = string.Empty;

    
    #endregion Field Declaration

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("CoeHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/CoeMod/CoeHome.aspx");
                    return;
                }
            }
            //****************************************************//
            if (!IsPostBack)
            {
                lblAlertMsg.Text = string.Empty;
                divPopAlert.Visible = false;
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                divMainContents.Visible = false;


                setLabelText();
                Bindcollege();
                BindEduLevel();
                BindRightsBaseBatch();
                BindDegree();
                BindBranch();
                BindExamYear();
                BindExamMonth();

            }
        }
         
         catch (Exception ex)
         {
             lblErrSearch.Text = Convert.ToString(ex);
             lblErrSearch.Visible = true;
             da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
         }

    }


    #endregion Page Load



    #region Bind Header

    public void Bindcollege()
    {
        try
        {
            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            ht.Clear();
            ht.Add("column_field", Convert.ToString(columnfield));
            DataSet dsprint = da.select_method("bind_college", ht, "sp");
            
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = dsprint;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
                ddlcollege.Enabled = true;
                ddlcollege.SelectedIndex = 0;

                
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    

    public void BindEduLevel()
    {
        try
        {
            ds.Clear();
            ddlEduLevel.Items.Clear();
            
            ddlEduLevel.Enabled = false;
            collegeCodes = string.Empty;
            streamNames = string.Empty;
            qryStream = string.Empty;
            if (ddlcollege.Items.Count > 0 && ddlcollege.Visible)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlcollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCodes))
                        {
                            collegeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and dg.college_code in(" + collegeCodes + ")";
                }
            }
            
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                string qry = "select distinct c.Edu_Level from course c where c.college_code in(" + collegeCodes + ") " + qryStream + " order by c.Edu_Level desc";
                ds = da.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    

                    ddlEduLevel.DataSource = ds;
                    ddlEduLevel.DataTextField = "Edu_Level";
                    ddlEduLevel.DataValueField = "Edu_Level";
                    ddlEduLevel.DataBind();
                    ddlEduLevel.SelectedIndex = 0;

                    
                    ddlEduLevel.Enabled = true;
                }
                else
                {
                    
                    ddlEduLevel.Enabled = true;
                }
            }
            else
            {
                
                ddlEduLevel.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindRightsBaseBatch()
    {
        try
        {
            userCode = string.Empty;
            groupUserCode = string.Empty;
            qryUserOrGroupCode = string.Empty;
            collegeCodes = string.Empty;
            ds.Clear();
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
                }
                if (!string.IsNullOrEmpty(groupUserCode))
                {
                    qryUserOrGroupCode = " and user_id='" + groupUserCode + "'";
                }
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                if (!string.IsNullOrEmpty(userCode))
                {
                    qryUserOrGroupCode = " and user_id='" + userCode + "'";
                }
            }
            if (ddlcollege.Items.Count > 0 && ddlcollege.Visible)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlcollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCodes))
                        {
                            collegeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and r.college_code in(" + collegeCodes + ")";
                }
            }
            
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                qryCollege = " and r.college_code in(" + collegeCodes + ")";
            }
            
            
            DataSet dsBatch = new DataSet();
            if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string qry = "select distinct batch_year from tbl_attendance_rights r where batch_year<>'' " + qryCollege + qryUserOrGroupCode + " order by batch_year desc";
                dsBatch = da.select_method_wo_parameter(qry, "Text");
            }
            if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = dsBatch;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
                ddlbatch.SelectedIndex = 0;
            }
            else
            {
                if (!string.IsNullOrEmpty(collegeCodes) && !string.IsNullOrEmpty(qryCollege))
                {
                    qry = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 " + qryCollege + qryEduLevel + qryStream + " order by r.Batch_Year desc";//and r.college_code in(" + collegeCodes + ") 
                    ds.Clear();
                    ds = da.select_method_wo_parameter(qry, "Text");
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlbatch.DataSource = ds;
                    ddlbatch.DataTextField = "Batch_Year";
                    ddlbatch.DataValueField = "Batch_Year";
                    ddlbatch.DataBind();
                    ddlbatch.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindDegree()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            cblDegree.Items.Clear();
            chkDegree.Checked = false;
            txtDegree.Enabled = false;
            txtDegree.Text = "--Select--";
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            collegeCode = string.Empty;
            qryCollege = string.Empty;
            batchYears = string.Empty;
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            userCode = string.Empty;
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if (!string.IsNullOrEmpty(groupUserCode) && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                qryUserOrGroupCode = " and dp.group_code='" + groupUserCode.Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                qryUserOrGroupCode = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (ddlcollege.Items.Count > 0 && ddlcollege.Visible)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlcollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCodes))
                        {
                            collegeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and dg.college_code in(" + collegeCodes + ")";
                }
            }
            
            if (ddlEduLevel.Items.Count > 0 && ddlEduLevel.Visible == true)
            {
                eduLevels = string.Empty;
                foreach (ListItem li in ddlEduLevel.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(eduLevels))
                        {
                            eduLevels = "'" + li.Text + "'";
                        }
                        else
                        {
                            eduLevels += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(eduLevels))
                {
                    qryEduLevel = " and c.Edu_Level in(" + eduLevels + ")";
                }
            }

            if (ddlbatch.Items.Count > 0 && ddlbatch.Visible == true)
            {
                foreach (ListItem li in ddlbatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYears))
                        {
                            batchYears = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYears += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and r.Batch_year in(" + batchYears + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes) && !string.IsNullOrEmpty(qryUserOrGroupCode) && !string.IsNullOrEmpty(qryEduLevel) && !string.IsNullOrEmpty(qryCollege))
            {
                ds = da.select_method_wo_parameter("select distinct dg.course_id,c.course_name,c.Priority from Degree dg,Course c,Department dt,DeptPrivilages dp where  dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code  " + qryCollege + qryUserOrGroupCode + qryStream + qryEduLevel + "  order by c.Priority,c.course_name", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblDegree.DataSource = ds;
                    cblDegree.DataTextField = "course_name";
                    cblDegree.DataValueField = "course_id";
                    cblDegree.DataBind();
                    cblDegree.SelectedIndex = 0;
                    checkBoxListselectOrDeselect(cblDegree, true);
                    CallCheckboxListChange(chkDegree, cblDegree, txtDegree, Lbldegree.Text, "--Select--");
                    txtDegree.Enabled = true;
                }
                else
                {
                    txtDegree.Enabled = false;
                }
            }
            else
            {
                txtDegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindBranch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            cblBranch.Items.Clear();
            ddlBranch.Items.Clear();
            chkBranch.Checked = false;
            txtBranch.Text = "--Select--";
            txtBranch.Enabled = false;
            ds.Clear();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            ds.Dispose();
            ds.Reset();
            string columnfield = string.Empty;
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(groupUserCode).Trim() != "") && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                columnfield = " and dp.group_code='" + groupUserCode.Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            selected = 0;
            collegeCodes = string.Empty;
            streamNames = string.Empty;
            eduLevels = string.Empty;
            qryCollege = string.Empty;
            qryStream = string.Empty;
            qryEduLevel = string.Empty;
            if (ddlcollege.Items.Count > 0 && ddlcollege.Visible)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlcollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCodes))
                        {
                            collegeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and dg.college_code in(" + collegeCodes + ")";
                }
            }
            
            if (ddlEduLevel.Items.Count > 0 && ddlEduLevel.Visible == true)
            {
                eduLevels = string.Empty;
                foreach (ListItem li in ddlEduLevel.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(eduLevels))
                        {
                            eduLevels = "'" + li.Text + "'";
                        }
                        else
                        {
                            eduLevels += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(eduLevels))
                {
                    qryEduLevel = " and c.Edu_Level in(" + eduLevels + ")";
                }
            }
            
            if (cblDegree.Items.Count > 0)
            {
                courseIds = getCblSelectedValue(cblDegree);
                if (!string.IsNullOrEmpty(courseIds))
                {
                    qryCourseId = " and c.Course_Id in(" + courseIds + ")";
                }
            }

            if (!string.IsNullOrEmpty(collegeCodes) && !string.IsNullOrEmpty(qryCourseId) && !string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(columnfield))
            {
                //ds = da.select_method_wo_parameter("select distinct dg.Degree_Code,dt.Dept_Name from Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r where  r.degree_code=dg.Degree_Code and dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and r.college_code=c.college_code and r.college_code=dg.college_code and dt.college_code=r.college_code " + qryCourseId + qryCollege + columnfield + qryStream + qryEduLevel + qryBatch + "order by dg.Degree_Code", "text");//and r.CC='1' and ISNULL(r.isRedo,'0')='0' 
                ds = da.select_method_wo_parameter("select distinct dg.degree_code,dt.dept_name as degree,dg.Acronym,dg.course_id,dt.dept_name,c.priority from Degree dg,Course c,Department dt,DeptPrivilages dp where dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and c.college_code=dt.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code " + qryCollege + columnfield + qryStream + qryEduLevel + qryCourseId + " order by c.priority,dt.dept_name ", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblBranch.DataSource = ds;
                    cblBranch.DataTextField = "degree";
                    cblBranch.DataValueField = "degree_code";
                    cblBranch.DataBind();
                    checkBoxListselectOrDeselect(cblBranch, true);
                    txtBranch.Enabled = true;
                    CallCheckboxListChange(chkBranch, cblBranch, txtBranch, LblBranch.Text, "--Select--");

                    ddlBranch.DataSource = ds;
                    ddlBranch.DataTextField = "degree";
                    ddlBranch.DataValueField = "degree_code";
                    ddlBranch.DataBind();
                    ddlBranch.SelectedIndex = 0;
                }
                else
                {
                    txtBranch.Enabled = false;
                }
            }
            else
            {
                txtBranch.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindExamYear()
    {
        try
        {
            ddlExamYear.Items.Clear();
            ds.Clear();
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            if (ddlcollege.Items.Count > 0 && ddlcollege.Visible)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlcollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCodes))
                        {
                            collegeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and dg.college_code in(" + collegeCodes + ")";
                }
            }
            
            if (ddlEduLevel.Items.Count > 0)
            {
                eduLevels = string.Empty;
                foreach (ListItem li in ddlEduLevel.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(eduLevels))
                        {
                            eduLevels = "'" + li.Text + "'";
                        }
                        else
                        {
                            eduLevels += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(eduLevels))
                {
                    qryEduLevel = " and c.Edu_Level in(" + eduLevels + ")";
                }
            }
            if (ddlbatch.Items.Count > 0)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlbatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYears))
                        {
                            batchYears = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYears += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and ed.Batch_year in(" + batchYears + ")";
                }
            }
            //if (cblDegree.Items.Count > 0)
            //{
            //    courseIds = getCblSelectedValue(cblDegree);
            //    if (!string.IsNullOrEmpty(courseIds))
            //    {
            //        qryCourseId = " and c.Course_Id in(" + courseIds + ")";
            //    }
            //}
            if (ddlBranch.Items.Count > 0 && ddlBranch.Visible)
            {
                degreeCodes = string.Empty;
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCodes))
                        {
                            degreeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and ed.degree_code in(" + degreeCodes + ")";
                }
            }
            else if (cblBranch.Items.Count > 0 && txtBranch.Enabled)
            {
                degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and ed.degree_code in(" + degreeCodes + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryBatch))
            {
                string qry = "select distinct ed.Exam_year from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_year<>'0' " + qryCollege + qryDegreeCode + qryBatch + " order by ed.Exam_year desc";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = da.select_method_wo_parameter(qry, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlExamYear.DataSource = ds;
                    ddlExamYear.DataTextField = "Exam_year";
                    ddlExamYear.DataValueField = "Exam_year";
                    ddlExamYear.DataBind();
                    ddlExamYear.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    
    private void BindExamMonth()
    {
        try
        {
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            ddlExamMonth.Items.Clear();
            ds.Clear();
            if (ddlcollege.Items.Count > 0 && ddlcollege.Visible)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlcollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCodes))
                        {
                            collegeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and dg.college_code in(" + collegeCodes + ")";
                }
            }
            
            if (ddlEduLevel.Items.Count > 0)
            {
                eduLevels = string.Empty;
                foreach (ListItem li in ddlEduLevel.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(eduLevels))
                        {
                            eduLevels = "'" + li.Text + "'";
                        }
                        else
                        {
                            eduLevels += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(eduLevels))
                {
                    qryEduLevel = " and c.Edu_Level in(" + eduLevels + ")";
                }
            }
            //if (cblDegree.Items.Count > 0)
            //{
            //    courseIds = getCblSelectedValue(cblDegree);
            //    if (!string.IsNullOrEmpty(courseIds))
            //    {
            //        qryCourseId = " and c.Course_Id in(" + courseIds + ")";
            //    }
            //}
            if (ddlbatch.Items.Count > 0)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlbatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYears))
                        {
                            batchYears = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYears += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and ed.Batch_year in(" + batchYears + ")";
                }
            }
            if (ddlBranch.Items.Count > 0 && ddlBranch.Visible)
            {
                degreeCodes = string.Empty;
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCodes))
                        {
                            degreeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and ed.degree_code in(" + degreeCodes + ")";
                }
            }
            else if (cblBranch.Items.Count > 0 && txtBranch.Enabled)
            {
                degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and ed.degree_code in(" + degreeCodes + ")";
                }
            }
            string ExamYear = string.Empty;
            if (ddlExamYear.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem li in ddlExamYear.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(ExamYear))
                        {
                            ExamYear = "'" + li.Value + "'";
                        }
                        else
                        {
                            ExamYear += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ExamYear))
                {
                    ExamYear = " and Exam_year in (" + ExamYear + ")";
                }
            }
            if (!string.IsNullOrEmpty(ExamYear) && !string.IsNullOrEmpty(collegeCodes) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryBatch))
            {
                string qry = "select distinct ed.Exam_Month,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1))) as Month_Name from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_Month>'0' and ed.Exam_Month<>'0' " + qryBatch + qryCollege + qryDegreeCode + ExamYear + " order by Exam_Month";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = da.select_method_wo_parameter(qry, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlExamMonth.DataSource = ds;
                    ddlExamMonth.DataTextField = "Month_Name";
                    ddlExamMonth.DataValueField = "Exam_Month";
                    ddlExamMonth.DataBind();
                    ddlExamMonth.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

   

    

    

    #endregion Bind Header

    #region Index Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindEduLevel();
            BindRightsBaseBatch();
            BindDegree();
            BindBranch();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlEduLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindRightsBaseBatch();
            BindDegree();
            BindBranch();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindDegree();
            BindBranch();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkDegree, cblDegree, txtDegree, Lbldegree.Text, "--Select--");
            BindBranch();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkDegree, cblDegree, txtDegree, Lbldegree.Text, "--Select--");
            BindBranch();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkBranch, cblBranch, txtBranch, LblBranch.Text, "--Select--");
            //BindBranch();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkBranch, cblBranch, txtBranch, LblBranch.Text, "--Select--");
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlExamMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
           
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Index Changed Events


    #region Button Events

    #region Popup Close

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion  Popup Close


     #region Go Click

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;


            collegeCodes = string.Empty;
            streamNames = string.Empty;
            eduLevels = string.Empty;
            batchYears = string.Empty;
            courseIds = string.Empty;
            degreeCodes = string.Empty;
            semesters = string.Empty;
            sections = string.Empty;
            subjectCodes = string.Empty;
            subjectNames = string.Empty;
            subjectNos = string.Empty;
            examMonth = string.Empty;
            examYear = string.Empty;
            fromDate = string.Empty;
            toDate = string.Empty;


            qryCollege = string.Empty;
            qryStream = string.Empty;
            qryEduLevel = string.Empty;
            qryBatch = string.Empty;
            qryCourseId = string.Empty;
            qryDegree = string.Empty;
            qryDegreeCode = string.Empty;
            qryDepartment = string.Empty;
            qrySemester = string.Empty;
            qrySection = string.Empty;
            qrySubjectNos = string.Empty;
            qrySubjectNames = string.Empty;
            qrySubjectCodes = string.Empty;
            qryExamYear = string.Empty;
            qryExamMonth = string.Empty;
            qryFromDate = string.Empty;
            qryToDate = string.Empty;

            string[] subjectName = new string[1];
            string qryRedoBatch = string.Empty;
            string qryRedoDegreeCode = string.Empty;
            bool isRedoStud = true;

            int classificationStartColumn = 0;
            string batchyearquery = string.Empty;


            if (ddlcollege.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + LblCollege.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                if (ddlcollege.Items.Count > 0 && ddlcollege.Visible)
                {
                    collegeCodes = string.Empty;
                    foreach (ListItem li in ddlcollege.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(collegeCodes))
                            {
                                collegeCodes = "'" + li.Value + "'";
                            }
                            else
                            {
                                collegeCodes += ",'" + li.Value + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(collegeCodes))
                    {
                        qryCollege = " and r.college_code in(" + collegeCodes + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select " + LblCollege.Text.Trim() + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                
            }

            if (ddlEduLevel.Items.Count > 0 && ddlEduLevel.Visible == true)
            {
                eduLevels = string.Empty;
                foreach (ListItem li in ddlEduLevel.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(eduLevels))
                        {
                            eduLevels = "'" + li.Text + "'";
                        }
                        else
                        {
                            eduLevels += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(eduLevels))
                {
                    qryEduLevel = " and c.Edu_Level in(" + eduLevels + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblEduLevel.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblEduLevel.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlbatch.Items.Count > 0 && ddlbatch.Visible)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlbatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYears))
                        {
                            batchYears = "'" + li.Value + "'";
                        }
                        else
                        {
                            batchYears += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and r.batch_year in(" + batchYears + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + Lblbatch.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + Lblbatch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            if (cblDegree.Items.Count > 0 && txtDegree.Enabled && txtDegree.Visible)
            {
                courseIds = getCblSelectedValue(cblDegree);
                if (!string.IsNullOrEmpty(courseIds))
                {
                    qryCourseId = " and course_id in(" + courseIds + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + Lbldegree.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + Lbldegree.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }

            if (ddlBranch.Items.Count > 0 && ddlBranch.Visible)
            {
                degreeCodes = string.Empty;
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCodes))
                        {
                            degreeCodes = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCodes += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and r.degree_code in(" + degreeCodes + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + LblBranch.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else if (cblBranch.Items.Count > 0 && txtBranch.Enabled && txtBranch.Visible)
            {
                degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and r.degree_code in(" + degreeCodes + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + LblBranch.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + LblBranch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlExamYear.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblExamYear.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                foreach (ListItem li in ddlExamYear.Items)
                {
                    if (li.Selected)
                    {
                        if (!string.IsNullOrEmpty(examYear))
                        {
                            examYear += ",'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            examYear = "'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(examYear))
                {
                    qryExamYear = " and et.Exam_Year in(" + examYear + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblExamYear.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            if (ddlExamMonth.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblExamMonth.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                foreach (ListItem li in ddlExamMonth.Items)
                {
                    if (li.Selected)
                    {
                        if (!string.IsNullOrEmpty(examMonth))
                        {
                            examMonth += ",'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            examMonth = "'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(examMonth))
                {
                    qryExamMonth = " and ed.Exam_Month in(" + examMonth + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblExamMonth.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }

           
                    #region FpSpread Style

                    FpSpread1.Visible = false;
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

                    
                    
                        FpSpread1.Sheets[0].FrozenRowCount = 1;
                        FpSpread1.Sheets[0].ColumnCount = 3;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].Columns[0].Width = 40;
                        FpSpread1.Sheets[0].Columns[0].Locked = true;
                        FpSpread1.Sheets[0].Columns[0].Resizable = false;
                        FpSpread1.Sheets[0].Columns[0].Visible = true;
                        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Programme";
                        FpSpread1.Sheets[0].Columns[1].Width = 350;
                        FpSpread1.Sheets[0].Columns[1].Locked = true;
                        FpSpread1.Sheets[0].Columns[1].Resizable = false;
                        FpSpread1.Sheets[0].Columns[1].Visible = true;
                        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);


                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "No. of Students Registered";
                        FpSpread1.Sheets[0].Columns[2].Width = 100;
                        FpSpread1.Sheets[0].Columns[2].Locked = true;
                        FpSpread1.Sheets[0].Columns[2].Resizable = false;
                        FpSpread1.Sheets[0].Columns[2].Visible = true;
                        FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);


                        
                        string qryfilter = string.Empty;
                        qry = "select * from coe_classification where collegecode='" + ddlcollege.SelectedValue + "' AND edu_level='" + ddlEduLevel.SelectedItem + "' AND  batch_year='" + ddlbatch.SelectedItem + "' order By frompoint DESC";
                        ds.Clear();
                        ds.Dispose();
                        ds.Reset();
                        ds = da.select_method_wo_parameter(qry, "Text");
                        int classificationcount= ds.Tables[0].Rows.Count;
                        double[] frompoint = new double[classificationcount];
                        double[] topoint = new double[classificationcount];
                        int[] classcate = new int[classificationcount];
                        int[] totnoofclasscate = new int[classificationcount];
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + ds.Tables[0].Rows.Count;
                            classificationStartColumn = FpSpread1.Sheets[0].ColumnCount;

                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, classificationStartColumn - ds.Tables[0].Rows.Count].Text = "Grade";
                            FpSpread1.Sheets[0].Columns[classificationStartColumn - ds.Tables[0].Rows.Count].Width = 100;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, classificationStartColumn - ds.Tables[0].Rows.Count, 1, ds.Tables[0].Rows.Count);


                            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                            {

                                classcate[row]=0;
                                totnoofclasscate[row]=0;
                                if (row == 0)
                                {

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, (classificationStartColumn - (ds.Tables[0].Rows.Count - row))].Text = Convert.ToString(ds.Tables[0].Rows[row]["classification"]).Trim();
                                    frompoint[row] = Convert.ToDouble(ds.Tables[0].Rows[row]["frompoint"]);
                                    topoint[row] = Convert.ToDouble(ds.Tables[0].Rows[row]["topoint"]);
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, (classificationStartColumn - (ds.Tables[0].Rows.Count - row))].Text = Convert.ToString(ds.Tables[0].Rows[row]["classification"]).Trim();
                                    FpSpread1.Sheets[0].Columns[classificationStartColumn - (ds.Tables[0].Rows.Count - row)].Width = 100;
                                    frompoint[row] = Convert.ToDouble(ds.Tables[0].Rows[row]["frompoint"]);
                                    topoint[row] = Convert.ToDouble(ds.Tables[0].Rows[row]["topoint"]);
                                }

                                FpSpread1.Sheets[0].Columns[classificationStartColumn - (ds.Tables[0].Rows.Count - row)].Resizable = false;
                                FpSpread1.Sheets[0].Columns[classificationStartColumn - (ds.Tables[0].Rows.Count - row)].Locked = true;

                               

                            }



                        }

                        FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 2;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Text = "No. of Students Passed";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 2, 2, 1);
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].Resizable = false;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].Locked = true;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].Width = 100;


                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Pass %";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 3].Resizable = false;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 3].Locked = true;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 3].Width = 100;



                        batchyearquery = "select distinct r.batch_year,e.current_semester,r.degree_code,dept_name,c.course_name + ' - '+ dept.dept_name as  branch,e.exam_code from registration r,exam_details e,department dept,degree d,course c where ";
                        if (ddlExamMonth.SelectedValue.ToString() != "" && ddlExamYear.SelectedValue.ToString() != "")
                            batchyearquery = batchyearquery + " e.Exam_Month='" + ddlExamMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlExamYear.SelectedValue.ToString() + "'";
                        if (degreeCodes != "")
                            batchyearquery = batchyearquery + " And d.Degree_Code in(" + degreeCodes + ")";
                        if (ddlbatch.SelectedValue.ToString() != "")
                            batchyearquery = batchyearquery + " And r.batch_year=" + ddlbatch.SelectedValue.ToString() + "";
                        if (courseIds != "")
                            batchyearquery = batchyearquery + " And c.course_id in(" + courseIds + ")";
                        batchyearquery = batchyearquery + " And d.course_id=c.course_id and e.batch_year=r.batch_year and dept.dept_code=d.dept_code and e.degree_code=r.degree_code and r.degree_code=d.degree_code  and r.college_code=" + ddlcollege.SelectedValue + " and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";


                        
                        
                        DataSet dsbatchyearquery = new DataSet();
                        dsbatchyearquery = da2.select_method_wo_parameter(batchyearquery, "Text");
                       
                        
                        if (dsbatchyearquery.Tables[0].Rows.Count > 0)
                        {    
                            int sno = 0;
                        
                            string batchyear = "";
                            string current_sem = "";
                            string degreecode = "";
                            string dept_name = "";
                            string examCode = "";
                            int totNoofStudentsReg = 0;
                            int totNoofStudPassed = 0;
                            double totpassper = 0;
                            for (int r = 0; r < dsbatchyearquery.Tables[0].Rows.Count; r++)
                            {
                                batchyear = dsbatchyearquery.Tables[0].Rows[r]["batch_year"].ToString();
                                current_sem = dsbatchyearquery.Tables[0].Rows[r]["current_semester"].ToString();
                                examCode = dsbatchyearquery.Tables[0].Rows[r]["exam_code"].ToString();
                                dept_name = dsbatchyearquery.Tables[0].Rows[r]["branch"].ToString();
                                degreecode = dsbatchyearquery.Tables[0].Rows[r]["degree_code"].ToString();


                                string SelectQ = "select count(*) FROM exam_application where exam_code='" + examCode + "' ";
                                SelectQ = SelectQ + " select count(distinct m.roll_no) from mark_entry m,registration r Where m.roll_no = r.roll_no And r.delflag = 0 And m.attempts = 1  and m.exam_code in ('" + examCode + "')";
                                SelectQ = SelectQ + "  select  count(distinct roll_no)  from mark_entry where  result = 'Pass' and passorfail=1 and exam_code in ('" + examCode + "') ";
                                SelectQ = SelectQ + "  select  count(distinct roll_no)  from mark_entry where  result = 'Fail' and passorfail=0 and exam_code in ('" + examCode + "') ";
                                SelectQ = SelectQ + "   select  count(distinct roll_no)  from mark_entry where  result = 'A%' and passorfail=0 and exam_code in ('" + examCode + "' )";
                                
                                DataSet studinfoads = new DataSet();

                                studinfoads = da22.select_method_wo_parameter(SelectQ, "Text");
                                int col = 0; 
                                if (studinfoads.Tables[0].Rows.Count > 0)
                                {
                                    string totalstudents = "";
                                    string studentappeared = "";
                                    string studentpassed = "";
                                    string studentfail = "";
                                    string studentabsent = "";
                                    string passpercent = "0";
                                  
                                    for (int studproci = 0; studproci < studinfoads.Tables[0].Rows.Count; studproci++)
                                    {
                                        totalstudents = studinfoads.Tables[0].Rows[0][0].ToString();
                                        studentappeared = studinfoads.Tables[1].Rows[0][0].ToString();
                                        studentpassed = studinfoads.Tables[2].Rows[0][0].ToString();
                                        studentfail = studinfoads.Tables[3].Rows[0][0].ToString();
                                        studentabsent = studinfoads.Tables[4].Rows[0][0].ToString();
                                        int passcount = Convert.ToInt32(studentappeared) - Convert.ToInt32(studentfail);
                                        studentpassed = passcount.ToString();

                                        sno++;
                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].Text = sno.ToString();
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].Locked = true;
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].VerticalAlign = VerticalAlign.Middle;
                                        col++;

                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].Text = dept_name.ToString();
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].Locked = true;
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].VerticalAlign = VerticalAlign.Middle;
                                        col++;


                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].Text = studentappeared.ToString();
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].Locked = true;
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].VerticalAlign = VerticalAlign.Middle;
                                        totNoofStudentsReg += Convert.ToInt32(studentappeared);
                                        col++;
                                      


                                        //string studdetail = "select distinct ea.roll_no from Exam_Details ed,exam_application ea where ed.exam_code=ea.exam_code and ed.Exam_Month='" + ddlExamMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlExamYear.SelectedValue.ToString() + "' and ed.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and ed.degree_code='" + degreecode + "'";

                                        string studdetail = " select distinct m.roll_no from mark_entry m,registration r Where m.roll_no = r.roll_no And r.delflag = 0 And m.attempts = 1  and m.exam_code in ('" + examCode + "')";

                                        DataSet studroll = new DataSet();

                                        studroll = da22.select_method_wo_parameter(studdetail, "Text");


                                        for (int rr = 0; rr < classificationcount; rr++)
                                            classcate[rr] = 0;

                                        if (studroll.Tables[0].Rows.Count > 0)
                                        {


                                            for (int studrollno = 0; studrollno < studroll.Tables[0].Rows.Count; studrollno++)
                                            {
                                                string rollnumber = studroll.Tables[0].Rows[studrollno]["roll_no"].ToString();

                                                string latemode = "1";
                                                string cgpa = d2.Calculete_CGPA(rollnumber, current_sem, degreecode, batchyear, latemode, Convert.ToString(ddlcollege.SelectedValue).Trim());

                                                if (cgpa != "-")
                                                {
                                                    for (int rr = 0; rr < classificationcount; rr++)
                                                    {
                                                        if (Convert.ToDouble(cgpa) >= frompoint[rr] && Convert.ToDouble(cgpa) <= topoint[rr])
                                                        {
                                                            classcate[rr]++;
                                                            break;
                                                        }
                                                    }
                                                }

                                            }

                                        }


                                        for (int cell = 0; cell < classificationcount; cell++)
                                        {

                                            FpSpread1.Sheets[0].Cells[(sno - 1), col].Text = classcate[cell].ToString();
                                            FpSpread1.Sheets[0].Cells[(sno - 1), col].Locked = true;
                                            FpSpread1.Sheets[0].Cells[(sno - 1), col].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[(sno - 1), col].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[(sno - 1), col].VerticalAlign = VerticalAlign.Middle;
                                            

                                            totnoofclasscate[cell] += classcate[cell];
                                            col++;
                                            

                                        }



                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].Text = studentpassed.ToString();
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].Locked = true;
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].VerticalAlign = VerticalAlign.Middle;
                                        totNoofStudPassed += Convert.ToInt32(studentpassed);
                                        col++;

                                        int total = Convert.ToInt32(studentpassed) + Convert.ToInt32(studentfail);
                                        if (studentpassed != "0")
                                        {
                                            double passpercent1 = 0;
                                            passpercent1 = Convert.ToDouble((Convert.ToDouble(studentpassed) / total) * 100);
                                            double passpercent2 = Math.Round(passpercent1, 2);
                                            passpercent = Convert.ToString(passpercent2);
                                        }

                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].Text = passpercent.ToString();
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].Locked = true;
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[(sno - 1), col].VerticalAlign = VerticalAlign.Middle;
                                        totpassper += Convert.ToDouble(passpercent);
                                        col++;

                                    }


                                }


                            }
                            int col1 = 1;
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].Text = "Grand Total";
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].Locked = true;
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].VerticalAlign = VerticalAlign.Middle;
                            col1++;


                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].Text = totNoofStudentsReg.ToString();
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].Locked = true;
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].VerticalAlign = VerticalAlign.Middle;
                            col1++;

                           
                            for (int cell = 0; cell < classificationcount; cell++)
                            {
                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].Text = totnoofclasscate[cell].ToString();
                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].Locked = true;
                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].VerticalAlign = VerticalAlign.Middle;
                                col1++;
                            }

                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].Text = totNoofStudPassed.ToString();
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].Locked = true;
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].VerticalAlign = VerticalAlign.Middle;
                            col1++;

                            double totalper = (totpassper / dsbatchyearquery.Tables[0].Rows.Count);
                            double totalper2 = Math.Round(totalper, 2);
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].Text = totalper2.ToString();
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].Locked = true;
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col1].VerticalAlign = VerticalAlign.Middle;


                            FpSpread1.SaveChanges();
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                            FpSpread1.Height = 500;
                            FpSpread1.SaveChanges();
                            FpSpread1.Visible = true;
                            divMainContents.Visible = true;
                        }
                        else
                        {
                            lblAlertMsg.Text = "No Records Found";
                            divPopAlert.Visible = true;
                            FpSpread1.Visible = false;
                            divMainContents.Visible = false;
                            
                        }


                       
                        

                    
                
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

     #endregion

    #region Generate Excel

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            printMaster1.Visible = false;
            string reportname = txtExcelFileName.Text.Trim().Replace(" ", "_").Trim();
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (FpSpread1.Visible == true)
                {
                    da.printexcelreport(FpSpread1, reportname);
                }
                lblExcelError.Visible = false;
            }
            else
            {
                lblExcelError.Text = "Please Enter Your Report Name";
                lblExcelError.Visible = true;
                txtExcelFileName.Focus();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Generate Excel

    #region Print PDF

    protected void btnPrintPDF_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string rptheadname = string.Empty;
            rptheadname = "Course/Programme wise Classification Report";
            string pagename = "Course_Programme_wise_Classification_Report.aspx";
            //string Course_Name = Convert.ToString(cblDegree.SelectedItem).Trim();
            //rptheadname += "@ " + Course_Name + " - " + Convert.ToString(ddlbranch.SelectedItem).Trim() + "@ " + " Year of Admission : " + Convert.ToString(ddlbatch.SelectedItem).Trim() + "@ " + " Semester : " + Convert.ToString(ddlsem.SelectedItem).Trim();
            if (FpSpread1.Visible == true)
            {
                printMaster1.loadspreaddetails(FpSpread1, pagename, rptheadname);
            }
            printMaster1.Visible = true;
            lblExcelError.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Print PDF

    #endregion Button Events



    private void setLabelText()
    {
        try
        {
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim().Split(',')[0] + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            institute = new Institution(grouporusercode);
            List<Label> lbl = new List<Label>();
            List<byte> fields = new List<byte>();
            lbl.Add(LblCollege);
            lbl.Add(Lbldegree);
            lbl.Add(LblBranch);
           
            fields.Add(0);
            fields.Add(2);
            fields.Add(3);
            
            if (institute != null && institute.TypeInstitute == 1)
            {
                Lblbatch.Text = "Year";
            }
            else
            {
                Lblbatch.Text = "Batch";
            }
            new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlcollege.Items.Count > 0 && ddlcollege.Visible) ? Convert.ToString(ddlcollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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

    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true, int selCount = 0)
    {
        try
        {
            int count = 0;
            foreach (wc.ListItem li in cbl.Items)
            {
                if (selCount != 0 && count == selCount)
                {
                    break;
                }
                count++;
                li.Selected = selected;
            }
        }
        catch
        {
        }
    }

    #endregion
}