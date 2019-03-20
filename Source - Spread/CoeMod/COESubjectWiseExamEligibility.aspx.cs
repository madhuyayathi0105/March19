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
using System.Configuration;

#endregion Namespace Declaration

public partial class CoeMod_COESubjectWiseExamEligiblity : System.Web.UI.Page
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

    string[] string_session_values = new string[100];
    string[] new_header_string_split;
    string[] split_holiday_status = new string[1000];

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
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtToDate.Attributes.Add("readonly", "readonly");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                lblAlertMsg.Text = string.Empty;
                divPopAlert.Visible = false;
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                divMainContents.Visible = false;

                setLabelText();
                Bindcollege();
                BindStream();
                BindEduLevel();
                BindRightsBaseBatch();
                BindDegree();
                BindBranch();
                BindSem();
                setMinimumExamEligibility();
                BindRightsBasedSectionDetail();
                BindSubject();
                BindExamYear();
                BindExamMonth();
            }
        }
        catch (ThreadAbortException tex)
        {
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            cblCollege.Items.Clear();
            ddlCollege.Items.Clear();
            chkCollege.Checked = false;
            txtCollege.Enabled = false;
            ddlCollege.Enabled = false;
            txtCollege.Text = "--Select--";
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = dsprint;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.Enabled = true;
                ddlCollege.SelectedIndex = 0;

                cblCollege.DataSource = dsprint;
                cblCollege.DataTextField = "collname";
                cblCollege.DataValueField = "college_code";
                cblCollege.DataBind();
                txtCollege.Enabled = false;
                checkBoxListselectOrDeselect(cblCollege, true);
                CallCheckboxListChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindStream()
    {
        try
        {
            collegeCodes = string.Empty;
            ds.Clear();
            cblStream.Items.Clear();
            chkStream.Checked = false;
            txtStream.Enabled = false;
            txtStream.Text = "--Select--";
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
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
            else if (cblCollege.Items.Count > 0 && txtCollege.Enabled)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and dg.college_code in(" + collegeCodes + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes.Trim()))
            {
                string mode = "select distinct ltrim(rtrim(isnull(c.type,''))) as type from course c where c.college_code in (" + collegeCodes + ") and c.type is not null and c.type<>''";
                ds = da.select_method_wo_parameter(mode, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblStream.DataSource = ds;
                cblStream.DataTextField = "type";
                cblStream.DataValueField = "type";
                cblStream.DataBind();
                checkBoxListselectOrDeselect(cblStream, true);
                CallCheckboxListChange(chkStream, cblStream, txtStream, lblStream.Text, "--Select--");
                txtStream.Enabled = true;
            }
            else
            {
                txtStream.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindEduLevel()
    {
        try
        {
            ds.Clear();
            ddlEduLevel.Items.Clear();
            cblEduLevel.Items.Clear();
            chkEduLevel.Checked = false;
            txtEduLevel.Text = "--Select--";
            txtEduLevel.Enabled = false;
            ddlEduLevel.Enabled = false;
            collegeCodes = string.Empty;
            streamNames = string.Empty;
            qryStream = string.Empty;
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
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
            else if (cblCollege.Items.Count > 0 && txtCollege.Enabled)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
            }
            if (cblStream.Items.Count > 0)
            {
                streamNames = getCblSelectedText(cblStream);
                if (!string.IsNullOrEmpty(streamNames))
                {
                    qryStream = " and ltrim(rtrim(isnull(c.type,''))) in(" + streamNames + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                string qry = "select distinct c.Edu_Level from course c where c.college_code in(" + collegeCodes + ") " + qryStream + " order by c.Edu_Level desc";
                ds = da.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblEduLevel.DataSource = ds;
                    cblEduLevel.DataTextField = "Edu_Level";
                    cblEduLevel.DataValueField = "Edu_Level";
                    cblEduLevel.DataBind();
                    checkBoxListselectOrDeselect(cblEduLevel, true);
                    CallCheckboxListChange(chkEduLevel, cblEduLevel, txtEduLevel, lblEduLevel.Text, "--Select--");

                    ddlEduLevel.DataSource = ds;
                    ddlEduLevel.DataTextField = "Edu_Level";
                    ddlEduLevel.DataValueField = "Edu_Level";
                    ddlEduLevel.DataBind();
                    ddlEduLevel.SelectedIndex = 0;

                    txtEduLevel.Enabled = true;
                    ddlEduLevel.Enabled = true;
                }
                else
                {
                    txtEduLevel.Enabled = true;
                    ddlEduLevel.Enabled = true;
                }
            }
            else
            {
                txtEduLevel.Enabled = false;
                ddlEduLevel.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
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
            else if (cblCollege.Items.Count > 0 && txtCollege.Enabled)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in cblCollege.Items)
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
            }
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                qryCollege = " and r.college_code in(" + collegeCodes + ")";
            }
            if (cblStream.Items.Count > 0)
            {
                streamNames = getCblSelectedText(cblStream);
                if (!string.IsNullOrEmpty(streamNames))
                {
                    qryStream = " and ltrim(rtrim(isnull(c.type,''))) in(" + streamNames + ")";
                }
            }
            if (cblEduLevel.Items.Count > 0)
            {
                eduLevels = getCblSelectedText(cblEduLevel);
                if (!string.IsNullOrEmpty(eduLevels))
                {
                    qryEduLevel = " and ltrim(rtrim(isnull(c.edu_level,''))) in(" + eduLevels + ")";
                }
            }
            DataSet dsBatch = new DataSet();
            if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string qry = "select distinct batch_year from tbl_attendance_rights r where batch_year<>'' " + qryCollege + qryUserOrGroupCode + " order by batch_year desc";
                dsBatch = da.select_method_wo_parameter(qry, "Text");
            }
            if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = dsBatch;
                ddlBatch.DataTextField = "Batch_year";
                ddlBatch.DataValueField = "Batch_year";
                ddlBatch.DataBind();
                ddlBatch.SelectedIndex = 0;
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
                    ddlBatch.DataSource = ds;
                    ddlBatch.DataTextField = "Batch_Year";
                    ddlBatch.DataValueField = "Batch_Year";
                    ddlBatch.DataBind();
                    ddlBatch.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
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
            else if (cblCollege.Items.Count > 0 && txtCollege.Enabled)
            {
                collegeCode = string.Empty;
                foreach (ListItem li in cblCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollege = " and dg.college_code in(" + collegeCode + ")";
                }
            }
            if (cblStream.Items.Count > 0)
            {
                streamNames = getCblSelectedText(cblStream);
                if (!string.IsNullOrEmpty(streamNames))
                {
                    qryStream = " and LTRIM(RTRIM(ISNULL(c.type,''))) in(" + streamNames + ")";
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
            else if (cblEduLevel.Items.Count > 0 && txtEduLevel.Visible == true && cblEduLevel.Visible == true)
            {
                eduLevels = getCblSelectedText(cblEduLevel);
                if (!string.IsNullOrEmpty(eduLevels))
                {
                    qryEduLevel = " and c.Edu_Level in(" + eduLevels + ")";
                }
            }
            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                foreach (ListItem li in ddlBatch.Items)
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
                    CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
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
            else if (cblCollege.Items.Count > 0 && txtCollege.Enabled)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and dg.college_code in(" + collegeCodes + ")";
                }
            }
            if (cblStream.Items.Count > 0)
            {
                streamNames = getCblSelectedText(cblStream);
                if (!string.IsNullOrEmpty(streamNames))
                {
                    qryStream = " and LTRIM(RTRIM(ISNULL(c.type,''))) in(" + streamNames + ")";
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
            else if (cblEduLevel.Items.Count > 0 && txtEduLevel.Visible && cblEduLevel.Visible)
            {
                eduLevels = getCblSelectedText(cblEduLevel);
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
                ds = da.select_method_wo_parameter("select distinct dg.degree_code,c.course_name + ' - '+ dt.dept_name as degree,dg.Acronym,dg.course_id,dt.dept_name,c.priority from Degree dg,Course c,Department dt,DeptPrivilages dp where dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and c.college_code=dt.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code " + qryCollege + columnfield + qryStream + qryEduLevel + qryCourseId + " order by c.priority,dt.dept_name ", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblBranch.DataSource = ds;
                    cblBranch.DataTextField = "degree";
                    cblBranch.DataValueField = "degree_code";
                    cblBranch.DataBind();
                    checkBoxListselectOrDeselect(cblBranch, true);
                    txtBranch.Enabled = true;
                    CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");

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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindSem()
    {
        try
        {
            ds.Clear();
            ddlSem.Items.Clear();
            ddlSem.Enabled = false;
            bool first_year = false;
            int duration = 0;
            int i = 0;
            selected = 0;
            qryCollege = string.Empty;
            collegeCode = string.Empty;
            qryBatch = string.Empty;
            batchYears = string.Empty;
            courseIds = string.Empty;
            qryCourseId = string.Empty;
            qryDegreeCode = string.Empty;
            degreeCodes = string.Empty;
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
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
            else if (cblCollege.Items.Count > 0 && txtCollege.Enabled)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and dg.college_code in(" + collegeCodes + ")";
                }
            }
            if (cblStream.Items.Count > 0)
            {
                streamNames = getCblSelectedText(cblStream);
                if (!string.IsNullOrEmpty(streamNames))
                {
                    qryStream = " and LTRIM(RTRIM(ISNULL(c.type,''))) in(" + streamNames + ")";
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
            else if (cblEduLevel.Items.Count > 0 && txtEduLevel.Visible == true && cblEduLevel.Visible == true)
            {
                eduLevels = getCblSelectedText(cblEduLevel);
                if (!string.IsNullOrEmpty(eduLevels))
                {
                    qryEduLevel = " and c.Edu_Level in(" + eduLevels + ")";
                }
            }
            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
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
                    qryBatch = " and Batch_year in(" + batchYears + ")";
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
                    qryDegreeCode = " and degree_code in(" + degreeCodes + ")";
                }
            }
            else if (cblBranch.Items.Count > 0 && txtBranch.Enabled)
            {
                degreeCodes = string.Empty;
                foreach (ListItem li in cblBranch.Items)
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
                    qryDegreeCode = " and degree_code in(" + degreeCodes + ")";
                }
            }
            string sqlnew = string.Empty;
            if (!string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryBatch))
            {
                sqlnew = "select distinct max(ndurations) ndurations,first_year_nonsemester from ndegree dg where ndurations<>'0'" + qryDegreeCode + qryCollege + qryBatch + " group by first_year_nonsemester";
                ds.Clear();
                ds = da.select_method_wo_parameter(sqlnew, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                //duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                }
                ddlSem.SelectedIndex = 0;
                ddlSem.Enabled = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollege))
                {
                    sqlnew = "select distinct max(duration) duration,first_year_nonsemester from degree dg where duration<>'0' " + qryDegreeCode + qryCollege + " group by first_year_nonsemester";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(sqlnew, "Text");
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    //duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                    bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                    int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                    }
                    ddlSem.SelectedIndex = 0;
                    ddlSem.Enabled = true;
                }
                else
                {
                    ddlSem.Enabled = false;
                }
            }
            setMinimumExamEligibility();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindRightsBasedSectionDetail()
    {
        try
        {
            batchYears = string.Empty;
            collegeCodes = string.Empty;
            degreeCodes = string.Empty;
            string sections = string.Empty;

            qrySection = string.Empty;
            qryCollege = string.Empty;
            qryBatch = string.Empty;
            qryDegreeCode = string.Empty;
            ds.Clear();
            ddlSection.Items.Clear();
            cblSec.Items.Clear();
            chkSec.Checked = false;
            ddlSection.Enabled = false;
            txtSec.Enabled = false;
            txtSec.Text = "-- Select --";
            userCode = string.Empty;
            groupUserCode = string.Empty;
            qryUserOrGroupCode = string.Empty;
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
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
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
            else if (cblCollege.Items.Count > 0 && cblCollege.Visible == true)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in cblCollege.Items)
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
            }
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                qryCollege = " and r.college_code in(" + collegeCodes + ")";
            }
            if (cblStream.Items.Count > 0)
            {
                streamNames = getCblSelectedText(cblStream);
                if (!string.IsNullOrEmpty(streamNames))
                {
                    qryStream = " and ltrim(rtrim(isnull(c.type,''))) in(" + streamNames + ")";
                }
            }
            if (cblEduLevel.Items.Count > 0)
            {
                eduLevels = getCblSelectedText(cblEduLevel);
                if (!string.IsNullOrEmpty(eduLevels))
                {
                    qryEduLevel = " and ltrim(rtrim(isnull(c.edu_level,''))) in(" + eduLevels + ")";
                }
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
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
                    qryBatch = " and r.batch_year in(" + batchYears + ")";
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
                    qryDegreeCode = " and r.degree_code in(" + degreeCodes + ")";
                }
            }
            else if (cblBranch.Items.Count > 0 && txtBranch.Enabled)
            {
                degreeCodes = string.Empty;
                foreach (ListItem li in cblBranch.Items)
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
            }
            if (ddlSem.Items.Count > 0 && ddlSem.Enabled)
            {
                semesters = string.Empty;
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(semesters))
                        {
                            semesters = "'" + li.Value + "'";
                        }
                        else
                        {
                            semesters += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and r.current_semester in(" + semesters + ")";
                }
            }
            if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(batchYears) && !string.IsNullOrEmpty(qryUserOrGroupCode) && !string.IsNullOrEmpty(qryBatch))
            {
                qrySection = da.GetFunctionv("select distinct sections from tbl_attendance_rights r where r.batch_year<>'' " + qryUserOrGroupCode + qryCollege + qryBatch).Trim();
            }
            if (!string.IsNullOrEmpty(qrySection.Trim()))
            {
                string[] sectionsAll = qrySection.Trim().Split(new char[] { ',' }, StringSplitOptions.None);
                sections = string.Empty;
                qrySection = string.Empty;
                bool hasEmpty = false;
                if (sectionsAll.Length > 0)
                {
                    for (int sec = 0; sec < sectionsAll.Length; sec++)
                    {
                        if (!string.IsNullOrEmpty(sectionsAll[sec].Trim()))
                        {
                            if (sections.Trim() == "")
                            {
                                sections = "'" + sectionsAll[sec] + "'";
                            }
                            else
                            {
                                sections += ",'" + sectionsAll[sec] + "'";
                            }
                        }
                        else
                        {
                            if (!hasEmpty)
                            {
                                if (sections.Trim() == "")
                                {
                                    sections = "'" + sectionsAll[sec] + "'";
                                }
                                else
                                {
                                    sections += ",'" + sectionsAll[sec] + "'";
                                }
                                hasEmpty = true;
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(sections.Trim()))
            {
                qrySection = " and isnull(ltrim(rtrim(r.sections)),'') in (" + sections + ") ";
            }
            else
            {
                qrySection = string.Empty;
            }
            if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(batchYears) && !string.IsNullOrEmpty(qryBatch))// && !string.IsNullOrEmpty(qrySection)
            {
                string sqlnew = "select distinct case when isnull(ltrim(rtrim(r.sections)),'')='' then 'Empty' else isnull(ltrim(rtrim(r.sections)),'') end sections, isnull(ltrim(rtrim(r.sections)),'') SecValues from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code and isnull(ltrim(rtrim(r.sections)),'')<>'-1' and isnull(ltrim(rtrim(r.sections)),'')<>' ' and delflag=0 and exam_flag<>'Debar' " + qryCollege + qrySemester + qryDegreeCode + qryBatch + qrySection + qryStream + qryEduLevel + " order by SecValues";
                ds.Clear();
                ds = da.select_method_wo_parameter(sqlnew, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSection.DataSource = ds;
                ddlSection.DataTextField = "sections";
                ddlSection.DataValueField = "SecValues";
                ddlSection.DataBind();
                ddlSection.Enabled = true;

                cblSec.DataSource = ds;
                cblSec.DataTextField = "sections";
                cblSec.DataValueField = "sections";
                cblSec.DataBind();
                checkBoxListselectOrDeselect(cblSec, true);
                CallCheckboxListChange(chkSec, cblSec, txtSec, lblSec.Text, "--Select--");
                chkSec.Checked = true;
                txtSec.Enabled = true;
                ddlSection.Enabled = true;

            }
            else
            {
                ddlSection.Enabled = false;
                txtSec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    private void setMinimumExamEligibility()
    {
        try
        {
            if (cblBranch.Items.Count > 0 && txtBranch.Visible == true && cblBranch.Visible == true)
            {
                degreeCodes = getCblSelectedText(cblBranch);
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and degree_code in(" + degreeCodes + ")";
                }
            }
            if (ddlSem.Items.Count > 0 && ddlSem.Enabled)
            {
                semesters = string.Empty;
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(semesters))
                        {
                            semesters = "'" + li.Value + "'";
                        }
                        else
                        {
                            semesters += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " semester in(" + semesters + ")";
                }
            }
            double minEligiblePercentage = 0;
            string minattexam = string.Empty;
            if (!string.IsNullOrEmpty(semesters) && !string.IsNullOrEmpty(qrySemester) && !string.IsNullOrEmpty(degreeCodes) && !string.IsNullOrEmpty(qryDegreeCode))
            {
                minattexam = da.GetFunction("select distinct ISNULL(percent_eligible_for_Subject,'0') as percent_eligible_for_Subject from PeriodAttndSchedule where  " + qrySemester + qryDegreeCode + " order by percent_eligible_for_Subject desc");
                double.TryParse(minattexam, out minEligiblePercentage);
            }
            txtMinAttForEligiblePercentage.Text = string.Empty;
            if (minattexam.Trim() != "")
            {
                txtMinAttForEligiblePercentage.Text = minEligiblePercentage.ToString();
            }
        }
        catch (Exception)
        {

        }
    }

    /// <summary>
    /// Developed By Malang Raja
    /// </summary>
    public void BindExamYear()
    {
        try
        {
            ddlExamYear.Items.Clear();
            ds.Clear();
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
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
            else if (cblCollege.Items.Count > 0 && txtCollege.Enabled)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and dg.college_code in (" + collegeCodes + ")";
                }
            }
            if (cblStream.Items.Count > 0)
            {
                streamNames = getCblSelectedText(cblStream);
                if (!string.IsNullOrEmpty(streamNames))
                {
                    qryStream = " and LTRIM(RTRIM(ISNULL(c.type,''))) in(" + streamNames + ")";
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
            if (ddlBatch.Items.Count > 0)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    /// <summary>
    /// Developed By Malang Raja
    /// </summary>
    private void BindExamMonth()
    {
        try
        {
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            ddlExamMonth.Items.Clear();
            ds.Clear();
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCodes = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
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
            else if (cblCollege.Items.Count > 0 && txtCollege.Enabled)
            {
                collegeCodes = getCblSelectedValue(cblCollege);
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollege = " and dg.college_code in (" + collegeCodes + ")";
                }
            }
            if (cblStream.Items.Count > 0)
            {
                streamNames = getCblSelectedText(cblStream);
                if (!string.IsNullOrEmpty(streamNames))
                {
                    qryStream = " and LTRIM(RTRIM(ISNULL(c.type,''))) in(" + streamNames + ")";
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
            if (ddlBatch.Items.Count > 0)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    private void BindSubject()
    {
        try
        {
            ds.Clear();
            ddlSubejct.Items.Clear();
            cblSubject.Items.Clear();
            chkSubject.Checked = false;
            txtSubject.Text = "--Select--";
            txtSubject.Enabled = false;
            ddlSubejct.Enabled = false;

            examMonth = string.Empty;
            examYear = string.Empty;
            collegeCodes = string.Empty;
            degreeCodes = string.Empty;
            semesters = string.Empty;
            sections = string.Empty;
            batchYears = string.Empty;

            qryCollege = string.Empty;
            qryDegreeCode = string.Empty;
            qrySection = string.Empty;
            qrySemester = string.Empty;
            qryBatch = string.Empty;
            qryExamMonth = string.Empty;

            if (ddlBatch.Items.Count > 0)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
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
                    qryBatch = " and sm.Batch_year in(" + batchYears + ")";
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
                    qryDegreeCode = " and sm.degree_code in(" + degreeCodes + ")";
                }
            }
            else if (cblBranch.Items.Count > 0 && txtBranch.Enabled)
            {
                degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and sm.degree_code in(" + degreeCodes + ")";
                }
            }
            if (ddlSem.Items.Count > 0)
            {
                semesters = string.Empty;
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(semesters))
                        {
                            semesters = "'" + li.Text + "'";
                        }
                        else
                        {
                            semesters += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and sm.semester in(" + semesters + ")";
                }
            }
            //if (ddlSection.Items.Count > 0)
            //{
            //    sections = string.Empty;
            //    foreach (ListItem li in ddlSection.Items)
            //    {
            //        if (li.Selected)
            //        {
            //            if (string.IsNullOrEmpty(sections))
            //            {
            //                sections = "'" + li.Value + "'";
            //            }
            //            else
            //            {
            //                sections += ",'" + li.Value + "'";
            //            }
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(sections))
            //    {
            //        qrySection = " and LTRIM(RTRIM(ISNULL(sf.Sections,''))) in(" + sections + ")";
            //    }
            //}
            if (ddlSection.Items.Count > 0 && ddlSection.Visible)
            {
                sections = string.Empty;
                foreach (ListItem li in ddlSection.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(sections))
                        {
                            sections = "'" + li.Value + "'";
                        }
                        else
                        {
                            sections += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sections))
                {
                    qrySection = " and LTRIM(RTRIM(ISNULL(sf.Sections,''))) in(" + sections + ")";
                }
            }
            else if (cblSec.Items.Count > 0 && txtSec.Enabled && txtSec.Visible)
            {
                sections = getCblSelectedValue(cblSec);
                if (!string.IsNullOrEmpty(sections))
                {
                    qrySection = " and LTRIM(RTRIM(ISNULL(sf.Sections,''))) in(" + sections + ")";
                }
            }
            if (!string.IsNullOrEmpty(qrySemester) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryBatch))
            {
                if (Session["staff_code"] == null || string.IsNullOrEmpty(Convert.ToString(Session["staff_code"]).Trim()))
                {
                    qry = "select distinct s.subject_no,s.subject_code,s.subject_name from subject s, syllabus_master sm,sub_sem ss,subjectChooser sc,staff_selector sf where sc.subject_no=s.subject_no and sf.subject_no=s.subject_no and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.promote_count=1 " + qryBatch + qrySemester + qryDegreeCode + " order by s.subject_code";
                }
                else if (Session["staff_code"] != null)
                {
                    qry = "select distinct s.subject_no,s.subject_code,s.subject_name from subject s, syllabus_master sm,sub_sem ss,subjectChooser sc,staff_selector sf where sc.subject_no=s.subject_no and sf.subject_no=s.subject_no and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.promote_count=1 " + qryBatch + qrySemester + qryDegreeCode + qrySection + " and sf.staff_code='" + Convert.ToString(Session["staff_code"]).Trim() + "' order by s.subject_code";
                }
                else
                {
                    qry = "select distinct s.subject_no,s.subject_code,s.subject_name from subject s, syllabus_master sm,sub_sem ss,subjectChooser sc,staff_selector sf where sc.subject_no=s.subject_no and sf.subject_no=s.subject_no and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.promote_count=1 " + qryBatch + qrySemester + qryDegreeCode + " order by s.subject_code";
                }
                ds.Clear();
                ds = da.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblSubject.DataSource = ds;
                    cblSubject.DataTextField = "subject_name";
                    cblSubject.DataValueField = "subject_no";
                    cblSubject.DataBind();
                    checkBoxListselectOrDeselect(cblSubject, true);
                    CallCheckboxListChange(chkSubject, cblSubject, txtSubject, lblSubjects.Text, "--Select--");
                    txtSubject.Enabled = true;

                    ddlSubejct.DataSource = ds;
                    ddlSubejct.DataTextField = "subject_name";
                    ddlSubejct.DataValueField = "subject_no";
                    ddlSubejct.DataBind();
                    ddlSubejct.Enabled = true;
                    ddlSubejct.SelectedIndex = 0;
                }
                else
                {
                    ddlSubejct.Enabled = false;
                    txtSubject.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void Init_Spread(Farpoint.FpSpread FpSpread1, int type = 0)
    {
        try
        {
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

            DataSet dsSettings = new DataSet();
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " and  group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            if (!string.IsNullOrEmpty(grouporusercode))
            {
                string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type') and value='1' " + grouporusercode + "";
                dsSettings = da.select_method_wo_parameter(Master1, "Text");
            }
            bool isRollVisible = ColumnHeaderVisiblity(0, dsSettings);
            bool isRegVisible = ColumnHeaderVisiblity(1, dsSettings);
            bool isAdmitNoVisible = ColumnHeaderVisiblity(2, dsSettings);
            bool isStudentTypeVisible = ColumnHeaderVisiblity(3, dsSettings);

            if (type == 0)
            {
                FpSpread1.Sheets[0].FrozenRowCount = 1;
                FpSpread1.Sheets[0].ColumnCount = 6;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].Columns[0].Width = 40;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Admission No";
                FpSpread1.Sheets[0].Columns[1].Width = 80;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Visible = isAdmitNoVisible;
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                FpSpread1.Sheets[0].Columns[2].Width = 100;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Visible = isRollVisible;
                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                FpSpread1.Sheets[0].Columns[3].Width = 100;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Visible = isRegVisible;
                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
                FpSpread1.Sheets[0].Columns[4].Width = 90;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Visible = isStudentTypeVisible;
                FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student Name";
                FpSpread1.Sheets[0].Columns[5].Width = 250;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

            }
            else
            {
                FpSpread1.Sheets[0].FrozenRowCount = 1;
                FpSpread1.Sheets[0].ColumnCount = 6;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].Columns[0].Width = 40;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Admission No";
                FpSpread1.Sheets[0].Columns[1].Width = 80;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Visible = isAdmitNoVisible;
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                FpSpread1.Sheets[0].Columns[2].Width = 100;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Visible = isRollVisible;
                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                FpSpread1.Sheets[0].Columns[3].Width = 100;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Visible = isRegVisible;
                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
                FpSpread1.Sheets[0].Columns[4].Width = 90;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Visible = isStudentTypeVisible;
                FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student Name";
                FpSpread1.Sheets[0].Columns[5].Width = 250;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    /// <summary>
    /// Developed By Malang Raja on Dec 7 2016
    /// </summary>
    /// <param name="type">0 For Roll No,1 For Register No,2 For Admission No, 3 For Student Type</param>
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
                    string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type') and value='1' " + grouporusercode + "";
                    dsSettings = da.select_method_wo_parameter(Master1, "Text");
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
                    }
                    if (hasValues)
                        break;
                }
            }
            return hasValues;
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
            return false;
        }
    }

    private void setDefault()
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            //if (ddlPercentageType.Items.Count > 0)
            //{
            //    ddlPercentageType.SelectedIndex = 0;
            //}
            setMinimumExamEligibility();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Bind Header

    #region Index Changed Events

    protected void chkCollege_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
            BindStream();
            BindEduLevel();
            BindRightsBaseBatch();
            BindDegree();
            BindBranch();
            BindSem();
            setMinimumExamEligibility();
            BindRightsBasedSectionDetail();
            BindSubject();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void cblCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
            BindStream();
            BindEduLevel();
            BindRightsBaseBatch();
            BindDegree();
            BindBranch();
            BindSem();
            setMinimumExamEligibility();
            BindRightsBasedSectionDetail();
            BindSubject();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindStream();
            BindEduLevel();
            BindRightsBaseBatch();
            BindDegree();
            BindBranch();
            BindSem();
            setMinimumExamEligibility();
            BindRightsBasedSectionDetail();
            BindSubject();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkStream_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkStream, cblStream, txtStream, lblStream.Text, "--Select--");
            BindEduLevel();
            BindRightsBaseBatch();
            BindDegree();
            BindBranch();
            BindSem();
            BindRightsBasedSectionDetail();
            BindSubject();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void cblStream_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkStream, cblStream, txtStream, lblStream.Text, "--Select--");
            BindEduLevel();
            BindRightsBaseBatch();
            BindDegree();
            BindBranch();
            BindSem();
            BindRightsBasedSectionDetail();
            BindSubject();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkEduLevel_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkEduLevel, cblEduLevel, txtEduLevel, lblEduLevel.Text, "--Select--");
            BindRightsBaseBatch();
            BindDegree();
            BindBranch();
            BindSem();
            BindRightsBasedSectionDetail();
            BindSubject();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void cblEduLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkEduLevel, cblEduLevel, txtEduLevel, lblEduLevel.Text, "--Select--");
            BindRightsBaseBatch();
            BindDegree();
            BindBranch();
            BindSem();
            BindRightsBasedSectionDetail();
            BindSubject();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            BindSem();
            BindRightsBasedSectionDetail();
            BindSubject();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            BindSem();
            BindRightsBasedSectionDetail();
            BindSubject();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            CallCheckboxChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            BindBranch();
            BindSem();
            BindRightsBasedSectionDetail();
            BindSubject();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            BindBranch();
            BindSem();
            BindRightsBasedSectionDetail();
            BindSubject();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            CallCheckboxChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            BindBranch();
            BindSem();
            BindRightsBasedSectionDetail();
            BindSubject();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            BindSem();
            BindRightsBasedSectionDetail();
            BindSubject();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindSem();
            BindRightsBasedSectionDetail();
            BindSubject();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindRightsBasedSectionDetail();
            BindSubject();
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkSec_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkSec, cblSec, txtSec, lblSec.Text, "--Select--");
            BindSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void cblSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkSec, cblSec, txtSec, lblSec.Text, "--Select--");
            BindSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            BindSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkSubject_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkSubject, cblSubject, txtSubject, lblSubjects.Text, "--Select--");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void cblSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkSubject, cblSubject, txtSubject, lblSubjects.Text, "--Select--");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlSubejct_SelectedIndexChanged(object sender, EventArgs e)
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlPercentageType_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblAlertMsg.Text = string.Empty;
        divPopAlert.Visible = false;
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        divMainContents.Visible = false;
        //if (ddlPercentageType.SelectedIndex == 0)
        //{
        //    //rblPercDays.Visible = true;
        //    //lblPercDays.Visible = true;
        //}
        //else
        //{
        //    //rblPercDays.Visible = false;
        //    //lblPercDays.Visible = false;
        //}
    }

    protected void ddlShowReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblAlertMsg.Text = string.Empty;
        divPopAlert.Visible = false;
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        divMainContents.Visible = false;
    }

    protected void FpExamEligiblity_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpExamEligiblity.SaveChanges();
            int r = FpExamEligiblity.Sheets[0].ActiveRow;
            int j = FpExamEligiblity.Sheets[0].ActiveColumn;
            if (r == 0 && j == 1)
            {
                int val = 0;
                int.TryParse(Convert.ToString(FpExamEligiblity.Sheets[0].Cells[r, j].Value).Trim(), out val);
                for (int row = 1; row < FpExamEligiblity.Sheets[0].RowCount; row++)
                {
                    if (FpExamEligiblity.Sheets[0].Cells[row, 0].Text != string.Empty)
                    {
                        if (val == 1)
                            FpExamEligiblity.Sheets[0].Cells[row, j].Value = 1;
                        else
                            FpExamEligiblity.Sheets[0].Cells[row, j].Value = 0;
                    }
                }
            }
        }
        catch
        {
        }
    }

    #endregion Index Changed Events

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

            dtFromDate = new DateTime();
            dtToDate = new DateTime();
            dtDummyDate = new DateTime();

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

            if (cblCollege.Items.Count == 0 && ddlCollege.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblCollege.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
                {
                    collegeCodes = string.Empty;
                    foreach (ListItem li in ddlCollege.Items)
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
                        lblAlertMsg.Text = "Please Select " + lblCollege.Text.Trim() + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else if (cblCollege.Items.Count > 0 && txtCollege.Enabled)
                {
                    collegeCodes = getCblSelectedValue(cblCollege);
                    if (!string.IsNullOrEmpty(collegeCodes))
                    {
                        qryCollege = " and r.college_code in(" + collegeCodes + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select " + lblCollege.Text.Trim() + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
            }
            if (cblStream.Items.Count > 0)
            {
                streamNames = getCblSelectedText(cblStream);
                if (!string.IsNullOrEmpty(streamNames))
                {
                    qryStream = " and LTRIM(RTRIM(ISNULL(c.type,''))) in(" + streamNames + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblStream.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
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
            else if (cblEduLevel.Items.Count > 0 && txtEduLevel.Visible && cblEduLevel.Visible)
            {
                eduLevels = getCblSelectedText(cblEduLevel);
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

            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
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
                    lblAlertMsg.Text = "Please Select " + lblBatch.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblBatch.Text.Trim() + " Were Found";
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
                    lblAlertMsg.Text = "Please Select " + lblDegree.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblDegree.Text.Trim() + " Were Found";
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
                    lblAlertMsg.Text = "Please Select " + lblBranch.Text.Trim() + " And Then Proceed";
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
                    lblAlertMsg.Text = "Please Select " + lblBranch.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblBranch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            if (ddlSem.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblSem.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                semesters = string.Empty;
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(semesters))
                        {
                            semesters = "'" + li.Value + "'";
                        }
                        else
                        {
                            semesters += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and r.current_semester in(" + semesters + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblSem.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            string splhrsec = string.Empty;
            if (ddlSection.Items.Count > 0 && ddlSection.Visible)
            {
                sections = string.Empty;
                foreach (ListItem li in ddlSection.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(sections))
                        {
                            sections = "'" + li.Value + "'";
                        }
                        else
                        {
                            sections += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sections))
                {
                    qrySection = " and r.sections in(" + sections + ")";
                    strsec = " and sections in(" + sections + ")";
                    rstrsec = " and r.sections in(" + sections + ")";
                    section_lab = " and l.sections in(" + sections + ")";
                    splhrsec = " and sections in(" + sections + ")";
                }
                //else
                //{
                //    lblAlertMsg.Text = "Please Select " + lblSec.Text.Trim() + " And Then Proceed";
                //    divPopAlert.Visible = true;
                //    return;
                //}
            }
            else if (cblSec.Items.Count > 0 && txtSec.Enabled && txtSec.Visible)
            {
                sections = getCblSelectedValue(cblSec);
                if (!string.IsNullOrEmpty(sections))
                {
                    qrySection = " and r.sections in(" + sections + ")";
                    strsec = " and sections in(" + sections + ")";
                    rstrsec = " and r.sections in(" + sections + ")";
                    section_lab = " and l.sections in(" + sections + ")";
                    splhrsec = " and sections in(" + sections + ")";
                }
                //else
                //{
                //    lblAlertMsg.Text = "Please Select " + lblSec.Text.Trim() + " And Then Proceed";
                //    divPopAlert.Visible = true;
                //    return;
                //}
            }

            if (cblSubject.Items.Count > 0 && txtSubject.Enabled && txtSubject.Visible)
            {
                subjectNos = getCblSelectedValue(cblSubject);
                subjectNames = getCblSelectedText(cblSubject);
                if (!string.IsNullOrEmpty(subjectNos))
                {
                    qrySubjectNos = " and s.subject_no in(" + subjectNos + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblSubjects.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else if (ddlSubejct.Items.Count > 0 && ddlSubejct.Enabled && ddlSubejct.Visible)
            {
                subjectNos = string.Empty;
                foreach (ListItem li in ddlSubejct.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(subjectNos))
                        {
                            subjectNos = "'" + li.Value + "'";
                        }
                        else
                        {
                            subjectNos += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(subjectNos))
                {
                    qrySubjectNos = " and s.subject_no in(" + subjectNos + ")";
                }
                else
                {
                    lblAlertMsg.Text = "Please Select " + lblSubjects.Text.Trim() + " And Then Proceed";
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblSubjects.Text.Trim() + " Were Found";
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

            dtFromDate = new DateTime();
            fromDate = txtFromDate.Text.Trim();
            toDate = txtToDate.Text.Trim();
            if (!string.IsNullOrEmpty(fromDate.Trim()))
            {
                if (!DateTime.TryParseExact(fromDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate))
                {
                    lblAlertMsg.Text = "From Date Must Be in the Format dd/MM/yyyy";
                    lblAlertMsg.Visible = true;
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "Please Select From Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(toDate.Trim()))
            {
                if (!DateTime.TryParseExact(toDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate))
                {
                    lblAlertMsg.Text = "To Date Must Be in the Format dd/MM/yyyy";
                    lblAlertMsg.Visible = true;
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "Please Select To Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            if (dtToDate < dtFromDate)
            {
                lblAlertMsg.Text = "From Date Must Be Lesser Than To Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            double minAttendanceForEligibleExam = 0;
            double.TryParse(txtMinAttForEligiblePercentage.Text.Trim(), out minAttendanceForEligibleExam);

            if (string.IsNullOrEmpty(txtMinAttForEligiblePercentage.Text.Trim()))
            {
                lblAlertMsg.Text = "Please Set Minimum Attendance % For Eligibility To Write Exam";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }

            if (!string.IsNullOrEmpty(qryCollege) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryBatch) && !string.IsNullOrEmpty(qrySemester) && !string.IsNullOrEmpty(qrySubjectNos))
            {
                string[] subjectNoSelected = subjectNos.Replace("'", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                // subjectName = subjectNames.Replace("'", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, string> dicSubjectName = new Dictionary<string, string>();
                foreach (ListItem liSubject in cblSubject.Items)
                {
                    if (liSubject.Selected)
                    {
                        string subNo = Convert.ToString(liSubject.Value).Trim();
                        string subName = Convert.ToString(liSubject.Text).Trim();
                        if (!dicSubjectName.ContainsKey(subNo.Trim().ToLower()))
                        {
                            dicSubjectName.Add(subNo.Trim().ToLower(), subName.Trim());
                        }
                    }
                }
                string sex = "0";
                string flag = "-1";
                DataSet dsStudentsList = new DataSet();
                DataSet dsHolidays = new DataSet();
                DataSet dsHolidayList = new DataSet();

                DataSet dsAlterSchedule1 = new DataSet();
                DataSet dsSemesterSchedule1 = new DataSet();
                DataSet dsCurrentLab = new DataSet();
                DataSet dsSplHrRights = new DataSet();
                DataSet dsTheoryAlter = new DataSet();
                DataSet dsSem = new DataSet();
                DataSet dsPracticalAlter = new DataSet();
                DataSet dsTheorySchedule = new DataSet();
                DataSet dsPracticalSchedule = new DataSet();
                DataSet dsAttndanceMaster = new DataSet();

                Hashtable htTotalConductedHrs = new Hashtable();
                Hashtable httotalStudPresentHrs = new Hashtable();
                Hashtable httotalStudPercentage = new Hashtable();

                string halforFull = string.Empty;
                string mng = string.Empty;
                string evng = string.Empty;
                string holiday_sched_details = string.Empty;

                Hashtable htHoliday = new Hashtable();

                string strdayflag = string.Empty;
                string genderflag = string.Empty;
                string regularflag = string.Empty;
                string grouporusercode = string.Empty;
                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";
                }
                else
                {
                    grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                }
                string qryfilter = string.Empty;
                qry = "select * from Master_Settings where " + grouporusercode + "";
                ds.Clear();
                ds.Dispose();
                ds.Reset();
                ds = da.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow mtrdr in ds.Tables[0].Rows)
                    {
                        if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "sex" && Convert.ToString(mtrdr["value"]) == "1")
                        {
                            sex = "1";
                        }
                        if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "general" && Convert.ToString(mtrdr["value"]) == "1")
                        {
                            flag = "0";
                        }
                        if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "as per lesson" && Convert.ToString(mtrdr["value"]) == "1")
                        {
                            flag = "1";
                        }
                        if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "male" && Convert.ToString(mtrdr["value"]).Trim() == "1")
                        {
                            genderflag = " and (app.sex='0'";
                        }
                        if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "female" && Convert.ToString(mtrdr["value"]).Trim() == "1")
                        {
                            if (genderflag != "" && genderflag != "\0")
                            {
                                genderflag = genderflag + " or app.sex='1'";
                            }
                            else
                            {
                                genderflag = " and (app.sex='1'";
                            }
                        }
                        if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "days scholor" && Convert.ToString(mtrdr["value"]).Trim() == "1")
                        {
                            strdayflag = " and (r.Stud_Type='Day Scholar'";
                        }
                        if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "hostel" && Convert.ToString(mtrdr["value"]).Trim() == "1")
                        {
                            if (strdayflag != null && strdayflag != "\0")
                            {
                                strdayflag = strdayflag + " or r.Stud_Type='Hostler'";
                            }
                            else
                            {
                                strdayflag = " and (r.Stud_Type='Hostler'";
                            }
                        }
                        if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "regular")
                        {
                            regularflag = "and ((r.mode=1)";
                        }
                        if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "lateral")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (r.mode=3)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((r.mode=3)";
                            }
                        }
                        if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "transfer")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (r.mode=2)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((r.mode=2)";
                            }
                        }
                    }
                }
                if (strdayflag != null && strdayflag != "")
                {
                    strdayflag = strdayflag + ")";
                }
                qryfilter = strdayflag;
                if (regularflag != "")
                {
                    regularflag = regularflag + ")";
                }
                if (genderflag != "")
                {
                    genderflag = genderflag + ")";
                }
                qryfilter += regularflag + genderflag;
                string orderBySetting = da.GetFunction("select value from master_Settings where settings='order_by'");
                string strOrderBy = " ORDER BY r.roll_no";
                string serialno = da.GetFunction("select LinkValue from inssettings where college_code='" + collegeCode + "' and linkname='Student Attendance'");
                if (serialno == "1")
                {
                    strOrderBy = "ORDER BY r.serialno";
                }
                else
                {
                    if (orderBySetting == "0")
                    {
                        strOrderBy = "ORDER BY r.roll_no";
                    }
                    else if (orderBySetting == "1")
                    {
                        strOrderBy = "ORDER BY r.Reg_No";
                    }
                    else if (orderBySetting == "2")
                    {
                        strOrderBy = "ORDER BY r.Stud_Name";
                    }
                    else if (orderBySetting == "0,1,2")
                    {
                        strOrderBy = "ORDER BY r.roll_no,r.Reg_No,r.Stud_Name";
                    }
                    else if (orderBySetting == "0,1")
                    {
                        strOrderBy = "ORDER BY r.roll_no,r.Reg_No";
                    }
                    else if (orderBySetting == "1,2")
                    {
                        strOrderBy = "ORDER BY r.Reg_No,r.Stud_Name";
                    }
                    else if (orderBySetting == "0,2")
                    {
                        strOrderBy = "ORDER BY r.roll_no,r.Stud_Name";
                    }
                }

                ht.Clear();
                ht.Add("from_date", dtFromDate);
                ht.Add("to_date", dtToDate);
                ht.Add("degree_code", degreeCodes.Replace("'", ""));
                ht.Add("sem", semesters.Replace("'", ""));
                ht.Add("coll_code", collegeCodes.Replace("'", ""));
                int iscount = 0;
                string qryHoliday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + dtFromDate.ToString() + "' and '" + dtToDate.ToString() + "' and degree_code in (" + degreeCodes + ") and semester in(" + semesters + ")";
                dsHolidays = da.select_method_wo_parameter(qryHoliday, "Text");
                if (dsHolidays.Tables.Count > 0 && dsHolidays.Tables[0].Rows.Count > 0)
                {
                    int.TryParse(Convert.ToString(dsHolidays.Tables[0].Rows[0]["cnt"]).Trim(), out iscount);
                }
                ht.Add("iscount", iscount);
                dsHolidayList = da.select_method("HOLIDATE_DETAILS_FINE", ht, "sp");
                if (dsHolidayList.Tables.Count > 0 && dsHolidayList.Tables[0].Rows.Count > 0)
                {
                    for (int holi = 0; holi < dsHolidayList.Tables[0].Rows.Count; holi++)
                    {
                        if (Convert.ToString(dsHolidayList.Tables[0].Rows[holi]["halforfull"]).Trim().ToLower() == "false" || Convert.ToString(dsHolidayList.Tables[0].Rows[holi]["halforfull"]).Trim().ToLower() == "0")
                        {
                            halforFull = "0";
                        }
                        else
                        {
                            halforFull = "1";
                        }
                        if (Convert.ToString(dsHolidayList.Tables[0].Rows[holi]["morning"]).Trim().ToLower() == "false" || Convert.ToString(dsHolidayList.Tables[0].Rows[holi]["morning"]).Trim().ToLower() == "0")
                        {
                            mng = "0";
                        }
                        else
                        {
                            mng = "1";
                        }
                        if (Convert.ToString(dsHolidayList.Tables[0].Rows[holi]["evening"]).Trim().ToLower() == "false" || Convert.ToString(dsHolidayList.Tables[0].Rows[holi]["evening"]).Trim().ToLower() == "0")
                        {
                            evng = "0";
                        }
                        else
                        {
                            evng = "1";
                        }
                        holiday_sched_details = halforFull + "*" + mng + "*" + evng;
                        if (!htHoliday.ContainsKey(Convert.ToDateTime(Convert.ToString(dsHolidayList.Tables[0].Rows[holi]["HOLI_DATE"]).Trim())))
                        {
                            htHoliday.Add(Convert.ToDateTime(Convert.ToString(dsHolidayList.Tables[0].Rows[holi]["HOLI_DATE"]).Trim()), holiday_sched_details);
                        }
                    }
                }
                qry = " select distinct r.app_no,r.Roll_Admit,r.Stud_Type,r.roll_no as 'ROLL NO', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO',p.No_of_hrs_per_day as 'PER DAY',schorder as 'order',start_date,r.degree_code,r.Batch_Year,no_of_hrs_I_half_day,no_of_hrs_II_half_day,len(a.roll_no), convert(varchar(15),adm_date,103) as adm_date,r.serialno FROM attendance a , registration r , Department d ,  PeriodAttndSchedule p  ,seminfo s,applyn app,subjectchooser sc WHERE a.roll_no=r.roll_no and sc.roll_no=r.roll_no and sc.subject_no in(" + subjectNos + ") and r.degree_code=p.degree_code and  r.Batch_Year in(" + batchYears + ")  and  s.batch_Year in(" + batchYears + ") and r.degree_code in(" + degreeCodes + ") and s.degree_code in (" + degreeCodes + ") and  s.semester in(" + semesters + ") and p.semester in(" + semesters + ")  and (r.CC = '0')  AND (r.DelFlag = '0')  AND (r.Exam_Flag <> 'debar') AND (r.Current_Semester IS NOT NULL) and  r.app_no=app.app_no " + qrySection + qryfilter + " " + strOrderBy + "  ";
                dsStudentsList = da.select_method_wo_parameter(qry, "text");

                dsAlterSchedule1.Clear();
                qry = "select  * from alternate_schedule where degree_code in (" + degreeCodes.Trim() + ") and semester in(" + semesters + ") and batch_year in(" + batchYears + ") and FromDate between '" + dtFromDate + "' and '" + dtToDate + "' " + strsec + " order by FromDate Desc";
                dsAlterSchedule1 = da.select_method_wo_parameter(qry, "Text");

                dsSemesterSchedule1.Clear();
                qry = "select  * from semester_schedule where degree_code in(" + degreeCodes.Trim() + ") and semester in(" + semesters + ") and batch_year in(" + batchYears + ")" + strsec + " order by FromDate Desc";
                dsSemesterSchedule1 = da.select_method_wo_parameter(qry, "Text");

                string currlabsub = "select distinct s.subject_no,s.subject_name,s.subject_code,sy.Batch_Year,sy.degree_code,sy.semester from syllabus_master sy,sub_sem sm,subject s where sy.syll_code=sm.syll_code and sy.syll_code=s.syll_code and sm.syll_code=s.syll_code and sm.subType_no=s.subType_no and sm.Lab=1 and sy.Batch_Year in(" + batchYears + ") and sy.degree_code in (" + degreeCodes.Trim() + ") and sy.semester in(" + semesters + ") order by sy.Batch_Year,sy.degree_code,sy.semester";
                dsCurrentLab = da.select_method_wo_parameter(currlabsub, "Text");

                string getdeteails = "select convert(nvarchar(15),s.start_date,101) as start_date,nodays,s.starting_dayorder from seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and p.semester=s.semester and s.semester in(" + semesters + ") and s.batch_year in(" + batchYears + ")  and s.degree_code in (" + degreeCodes.Trim() + ")";
                getdeteails = getdeteails + " ; select * from tbl_consider_day_order where semester in(" + semesters + ") and batch_year in(" + batchYears + ")  and degree_code in (" + degreeCodes.Trim() + ")";
                dsSem = da.select_method_wo_parameter(getdeteails, "Text");

                ht.Clear();
                ht.Add("colege_code", collegeCodes.Replace("'", ""));
                dsAttndanceMaster = da.select_method("ATT_MASTER_SETTING", ht, "sp");

                grouporusercode = string.Empty;
                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim() + "'";
                }
                else
                {
                    grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                }
                string qryNew = "select rights from special_hr_rights where " + grouporusercode + "";
                dsSplHrRights = da.select_method_wo_parameter(qryNew, "Text");

                //and a.month_year='" + month_year + "'
                string qryTheryAlter = "select distinct s.subject_no,r.roll_no,a.*, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from registration r ,subjectchooser s,attendance a  where r.roll_no=a.roll_no and s.roll_no=a.roll_no  and r.degree_code in(" + degreeCodes.Trim() + ") and batch_year in(" + batchYears + ") and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=s.roll_no and s.semester in(" + semesters + ") " + strsec + " and  subject_no in(" + subjectNos + ") " + strOrderBy + "";
                dsTheoryAlter = da.select_method_wo_parameter(qryTheryAlter, "Text");

                //and hour_value='" + temp_hr + "' and a.month_year='" + month_year + "'  and day_value='" + strDay + "'
                string qryLabAlter = "select distinct s.subject_no,FromDate,hour_value,day_value,r.roll_no,a.*, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from  registration r,subjectchooser_New s,laballoc_new l,attendance a  where a.roll_no=s.roll_no and r.roll_no=a.roll_no  and r.degree_code in(" + degreeCodes + ") and r.batch_year in(" + batchYears + ") and cc=0 and delflag=0 and exam_flag<>'debar'  " + rstrsec + " and r.roll_no=s.roll_no and s.subject_no in(" + subjectNos + ") and r.degree_code=l.degree_code and r.batch_year=l.batch_year and l.Semester=r.Current_Semester and s.subject_no =l.subject_no and s.Batch=l.Stu_Batch and l.subject_no in(" + subjectNos + ")  " + section_lab + " and FromDate between '" + dtFromDate + "' and '" + dtToDate + "' and l.fdate=s.fromdate " + strOrderBy + "";
                dsPracticalAlter = da.select_method_wo_parameter(qryLabAlter, "Text");

                //and a.month_year='" + month_year + "'
                string qryTherySchedule = "select distinct s.subject_no,r.roll_no,a.*, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from registration r ,subjectchooser s,attendance a where r.roll_no=a.roll_no and a.roll_no=s.roll_no  and r.degree_code in(" + degreeCodes + ") and batch_year in(" + batchYears + ") and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=s.roll_no and s.semester in(" + semesters + ") " + strsec + " and  subject_no in(" + subjectNos + ") " + strOrderBy + "";
                dsTheorySchedule = da.select_method_wo_parameter(qryTherySchedule, "Text");

                //and hour_value='" + temp_hr + "'  and day_value='" + strDay + "' and a.month_year='" + month_year + "'
                string qryLabSchedule = "select distinct s.subject_no,hour_value,day_value,r.roll_no,a.*, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from  registration r,subjectchooser s,laballoc l,attendance a  where r.roll_no=a.roll_no and a.roll_no=s.roll_no  and r.degree_code in(" + degreeCodes + ") and r.batch_year in(" + batchYears + ") and cc=0 and delflag=0 and exam_flag<>'debar' and s.Batch=l.Stu_Batch  " + rstrsec + " and r.roll_no=s.roll_no and s.subject_no in(" + subjectNos + ")  and r.degree_code=l.degree_code and r.batch_year=l.batch_year and l.Semester=r.Current_Semester and s.subject_no =l.subject_no   and l.subject_no in(" + subjectNos + ")  " + section_lab + " " + strOrderBy + "";
                dsPracticalSchedule = da.select_method_wo_parameter(qryLabSchedule, "Text");

                int subCount = 0;
                int[] totalConductedHrs = new int[subjectNoSelected.Length];

                FarPoint.Web.Spread.CheckBoxCellType chkOneByOne = new FarPoint.Web.Spread.CheckBoxCellType();
                FarPoint.Web.Spread.CheckBoxCellType chkSelectAll = new FarPoint.Web.Spread.CheckBoxCellType();
                chkSelectAll.AutoPostBack = true;

                if (dsStudentsList.Tables.Count > 0 && dsStudentsList.Tables[0].Rows.Count > 0)
                {
                    //Farpoint.CheckBoxCellType chkCellAll = new Farpoint.CheckBoxCellType();
                    //chkCellAll.AutoPostBack = true;
                    //Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
                    //Farpoint.CheckBoxCellType chkCell = new Farpoint.CheckBoxCellType();
                    //chkCell.AutoPostBack = false;

                    htSplHr.Clear();
                    string hrdetno = string.Empty;
                    string getsphr = "select distinct date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code in(" + degreeCodes + ") and batch_year in(" + batchYears + ") and semester in(" + semesters + ") and date between '" + dtFromDate.ToString() + "' and '" + dtToDate.ToString() + "' " + splhrsec + "";
                    dsSplHr = da.select_method(getsphr, hat, "Text");
                    if (dsSplHr.Tables.Count > 0 && dsSplHr.Tables[0].Rows.Count > 0)
                    {
                        for (int sphr = 0; sphr < dsSplHr.Tables[0].Rows.Count; sphr++)
                        {
                            if (htSplHr.Contains(Convert.ToString(dsSplHr.Tables[0].Rows[sphr]["date"]).Trim()))
                            {
                                hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(dsSplHr.Tables[0].Rows[sphr]["date"]).Trim(), htSplHr));
                                hrdetno = hrdetno + "," + Convert.ToString(dsSplHr.Tables[0].Rows[sphr]["hrdet_no"]).Trim();
                                htSplHr[Convert.ToString(dsSplHr.Tables[0].Rows[sphr]["date"])] = hrdetno.Trim();
                            }
                            else
                            {
                                htSplHr.Add(Convert.ToString(dsSplHr.Tables[0].Rows[sphr]["date"]).Trim(), Convert.ToString(dsSplHr.Tables[0].Rows[sphr]["hrdet_no"]).Trim());
                            }
                        }
                    }

                    collegeCode = string.Empty;
                    string collegeName = string.Empty;
                    string batchYear = string.Empty;
                    string courseId = string.Empty;
                    string deptCode = string.Empty;
                    string degreeCode = string.Empty;
                    string courseType = string.Empty;
                    string eduLevel = string.Empty;
                    string courseName = string.Empty;
                    string departmentName = string.Empty;
                    string departmentAcr = string.Empty;
                    string degreeName = string.Empty;
                    string examCode = string.Empty;
                    //string examYear = string.Empty;
                    //string examMonth = string.Empty;
                    string monthName = string.Empty;
                    string examMonthYear = string.Empty;
                    string currentSemester = string.Empty;
                    string redoStatus = string.Empty;
                    string maxDuration = string.Empty;
                    int serialNos = 1;

                    Init_Spread(FpExamEligiblity, 0);
                    ht.Clear();
                    FpExamEligiblity.Width = 950;
                    FpExamEligiblity.Visible = true;
                    FpExamEligiblity.Sheets[0].RowCount = 0;

                    semStartDate = Convert.ToString(dsStudentsList.Tables[0].Rows[0]["start_date"]).Trim();
                    order = Convert.ToString(dsStudentsList.Tables[0].Rows[0]["order"]).Trim();
                    int.TryParse(Convert.ToString(dsStudentsList.Tables[0].Rows[0]["PER DAY"]).Trim(), out noOfHours);
                    int.TryParse(Convert.ToString(dsStudentsList.Tables[0].Rows[0]["no_of_hrs_I_half_day"]).Trim(), out firstHalf);
                    int.TryParse(Convert.ToString(dsStudentsList.Tables[0].Rows[0]["no_of_hrs_II_half_day"]).Trim(), out secondHalf);
                    if (noOfHours > 0)
                    {
                        foreach (DataRow drStudent in dsStudentsList.Tables[0].Rows)
                        {
                            int rowCount = 0;
                            string serialNo = Convert.ToString(drStudent["serialno"]).Trim();
                            string rollNo = Convert.ToString(drStudent["ROLL NO"]).Trim();
                            string regNo = Convert.ToString(drStudent["REG NO"]).Trim();
                            string appNo = Convert.ToString(drStudent["app_no"]).Trim();
                            string admissionNo = Convert.ToString(drStudent["Roll_Admit"]).Trim();
                            string admissionDate = Convert.ToString(drStudent["adm_date"]).Trim();
                            string studentName = Convert.ToString(drStudent["STUD NAME"]).Trim();
                            string studentType = Convert.ToString(drStudent["Stud_Type"]).Trim();
                            string degreeCodeNew = Convert.ToString(drStudent["degree_code"]).Trim();
                            string batchYearNew = Convert.ToString(drStudent["Batch_Year"]).Trim();

                            dtAdmissionDate = new DateTime();

                            semStartDate = Convert.ToString(drStudent["start_date"]).Trim();
                            order = Convert.ToString(drStudent["order"]).Trim();
                            int.TryParse(Convert.ToString(drStudent["PER DAY"]).Trim(), out noOfHours);
                            int.TryParse(Convert.ToString(drStudent["no_of_hrs_I_half_day"]).Trim(), out firstHalf);
                            int.TryParse(Convert.ToString(drStudent["no_of_hrs_II_half_day"]).Trim(), out secondHalf);

                            if (DateTime.TryParseExact(admissionDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtAdmissionDate))
                            {
                                if (!dicStudentAdmDate.ContainsKey(rollNo.Trim().ToLower()))
                                {
                                    dicStudentAdmDate.Add(rollNo.Trim().ToLower(), dtAdmissionDate);
                                }
                            }
                            else
                            {
                                if (!dicStudentAdmDate.ContainsKey(rollNo.Trim().ToLower()))
                                {
                                    dicStudentAdmDate.Add(rollNo.Trim().ToLower(), dtAdmissionDate);
                                }
                            }
                            if (!has_load_rollno.ContainsKey(rollNo.Trim().ToLower()))
                            {
                                has_load_rollno.Add(rollNo.Trim().ToLower(), 0);
                            }
                            if (!has_total_attnd_hour.ContainsKey(rollNo.Trim().Trim().ToLower()))
                            {
                                has_total_attnd_hour.Add(rollNo.Trim().ToLower(), 0);
                            }
                            FpExamEligiblity.Sheets[0].RowCount++;
                            rowCount = FpExamEligiblity.Sheets[0].RowCount - 1;
                            if (!hatsplhrattendance.Contains(rollNo.Trim().ToLower()))
                            {
                                hatsplhrattendance.Add(rollNo.Trim().ToLower(), rowCount);
                            }
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNos).Trim();
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(degreeCodeNew).Trim();
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 0].Locked = true;
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(admissionNo).Trim();
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(batchYearNew).Trim();
                            //string admdate = admissionDate;// ds_student.Tables[0].Rows[row_count]["adm_date"].ToString();
                            //string[] admdatesp = admdate.Split(new Char[] { '/' });
                            //admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(dtAdmissionDate.ToString("yyyy/MM/dd")).Trim();
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 1].Locked = true;
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(rollNo).Trim();
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(appNo).Trim();
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 2].Locked = true;
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(regNo).Trim();
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 3].Locked = true;
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(studentType).Trim();
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 4].Locked = true;
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(studentName).Trim();
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 5].Locked = true;
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                            FpExamEligiblity.Sheets[0].Cells[FpExamEligiblity.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                            serialNos++;
                        }
                        if (subjectNoSelected.Length > 0)
                        {
                            int subjectCount = 0;
                            bool check_row_visible = false;
                            int rowValue = 0;
                            int subjectStartColumn = FpExamEligiblity.Sheets[0].ColumnCount;
                            int overAllDetailsColumns = 0;
                            foreach (string subjects in subjectNoSelected)
                            {
                                //subjects = subjects.Trim();
                                subjectCount++;
                                int[] totalStudConductedHrs = new int[subjectNoSelected.Length];
                                int[] totalStudPresentHrs = new int[subjectNoSelected.Length];
                                double[] totalStudPercentage = new double[subjectNoSelected.Length];
                                CalculateAttendance(dsTheoryAlter, dsPracticalAlter, dsTheorySchedule, dsPracticalSchedule, strOrderBy, dsAttndanceMaster, dsSplHrRights, dsSem, dsSemesterSchedule1, dsAlterSchedule1, dsCurrentLab, dtFromDate, dtToDate, subjects, ref has_load_rollno, ref has_total_attnd_hour, ref has_od);
                                if (dicSubjectName.ContainsKey(subjects.Trim().ToLower()))
                                {
                                    subjectNames = dicSubjectName[subjects.Trim().ToLower()];
                                }
                                string subjectAcronymn = da.GetFunctionv("select acronym from subject where subject_no='" + subjects.Trim() + "'");
                                subCount++;
                                max_tot = 0;
                                attnd_hr = 0;
                                tot_hr = 0;

                                FpExamEligiblity.Sheets[0].ColumnCount = FpExamEligiblity.Sheets[0].ColumnCount + 7;
                                subjectStartColumn = FpExamEligiblity.Sheets[0].ColumnCount;

                                FpExamEligiblity.Sheets[0].ColumnHeader.Cells[0, subjectStartColumn - 7].Text = (string.IsNullOrEmpty(subjectAcronymn) ? subjectNames : subjectAcronymn);
                                FpExamEligiblity.Sheets[0].ColumnHeader.Cells[0, subjectStartColumn - 7].Tag = Convert.ToString(subjects).Trim();
                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 7].Width = 100;
                                FpExamEligiblity.Sheets[0].ColumnHeaderSpanModel.Add(0, subjectStartColumn - 7, 1, 6);
                                FpExamEligiblity.Sheets[0].ColumnHeader.Cells[1, (subjectStartColumn - 7)].Text = "Con. Hrs";
                                FpExamEligiblity.Sheets[0].ColumnHeader.Cells[1, (subjectStartColumn - 6)].Text = "Present";
                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 6].Width = 100;
                                FpExamEligiblity.Sheets[0].ColumnHeader.Cells[1, (subjectStartColumn - 5)].Text = "OD";
                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 5].Width = 100;
                                FpExamEligiblity.Sheets[0].ColumnHeader.Cells[1, (subjectStartColumn - 4)].Text = "Tot.Hrs";
                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 4].Width = 100;
                                FpExamEligiblity.Sheets[0].ColumnHeader.Cells[1, (subjectStartColumn - 3)].Text = "Absent";
                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 3].Width = 100;
                                FpExamEligiblity.Sheets[0].ColumnHeader.Cells[1, (subjectStartColumn - 2)].Text = "  \t\t%\t\t  ";
                                FpExamEligiblity.Sheets[0].ColumnHeader.Cells[1, (subjectStartColumn - 2)].Font.Bold = true;
                                FpExamEligiblity.Sheets[0].ColumnHeader.Cells[0, (subjectStartColumn - 2)].Note = "0";
                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 2].Width = 100;

                                FpExamEligiblity.Sheets[0].ColumnHeader.Cells[0, subjectStartColumn - 1].Text = "Status";
                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 1].Width = 100;
                                FpExamEligiblity.Sheets[0].ColumnHeaderSpanModel.Add(0, subjectStartColumn - 1, 2, 1);


                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 7].Resizable = false;
                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 7].Locked = true;

                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 6].Resizable = false;
                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 6].Locked = true;

                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 5].Resizable = false;
                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 5].Locked = true;

                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 4].Resizable = false;
                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 4].Locked = true;

                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 3].Resizable = false;
                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 3].Locked = true;

                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 2].Resizable = false;
                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 2].Locked = true;

                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 1].Resizable = false;
                                FpExamEligiblity.Sheets[0].Columns[subjectStartColumn - 1].Locked = true;

                                int sno = 0;
                                int countInvisible = 0;
                                for (int row_cnt = 0; row_cnt < FpExamEligiblity.Sheets[0].RowCount; row_cnt++)
                                {
                                    bool check_flag = false;
                                    attnd_hr = 0;
                                    roll_no = Convert.ToString(FpExamEligiblity.Sheets[0].Cells[row_cnt, 2].Text).Trim();
                                    if (has_load_rollno.Contains(roll_no.Trim().ToLower()))
                                    {
                                        attnd_hr = Convert.ToDouble(GetCorrespondingKey(roll_no.Trim().ToLower(), has_load_rollno));
                                        if (FpExamEligiblity.Sheets[0].Rows[row_cnt].Visible == true)
                                        {
                                            sno++;
                                            FpExamEligiblity.Sheets[0].Cells[row_cnt, 0].Text = sno.ToString();
                                            FpExamEligiblity.Sheets[0].Cells[row_cnt, 0].Locked = true;
                                            FpExamEligiblity.Sheets[0].Cells[row_cnt, 0].Font.Name = "Book Antiqua";
                                            FpExamEligiblity.Sheets[0].Cells[row_cnt, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpExamEligiblity.Sheets[0].Cells[row_cnt, 0].VerticalAlign = VerticalAlign.Middle;
                                        }
                                        double hrs = 0;
                                        if (!httotalStudPresentHrs.Contains(roll_no.Trim().ToLower()))
                                        {
                                            httotalStudPresentHrs.Add(roll_no.Trim().ToLower(), attnd_hr);
                                        }
                                        else
                                        {
                                            hrs = Convert.ToDouble(GetCorrespondingKey(roll_no.Trim().ToLower(), httotalStudPresentHrs));
                                            hrs += attnd_hr;
                                            httotalStudPresentHrs[roll_no.Trim().ToLower()] = hrs;
                                        }
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, (subjectStartColumn - 6)].Text = attnd_hr.ToString();
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 6].Locked = true;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 6].Font.Name = "Book Antiqua";
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 6].VerticalAlign = VerticalAlign.Middle;
                                    }
                                    tot_hr = 0;
                                    if (has_total_attnd_hour.Contains(roll_no.Trim().ToLower()))
                                    {
                                        tot_hr = Convert.ToDouble(GetCorrespondingKey(roll_no.Trim().ToLower(), has_total_attnd_hour));
                                        if (row_cnt == 0)
                                        {
                                            if (max_tot < tot_hr)
                                            {
                                                max_tot = tot_hr;
                                            }
                                            Session["max_tot_hour"] = max_tot.ToString();
                                        }
                                    }
                                    od_count = 0;
                                    if (has_od.Contains(roll_no.Trim().ToLower()))
                                    {
                                        od_count = Convert.ToInt16(GetCorrespondingKey(roll_no.Trim().ToLower(), has_od));
                                        check_flag = true;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, (subjectStartColumn - 5)].Text = od_count.ToString();
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 5].Locked = true;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 5].Font.Name = "Book Antiqua";
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 5].HorizontalAlign = HorizontalAlign.Center;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 5].VerticalAlign = VerticalAlign.Middle;
                                    }
                                    if (check_flag == false)
                                    {
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 5].Text = "0";
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 5].Locked = true;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 5].Font.Name = "Book Antiqua";
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 5].HorizontalAlign = HorizontalAlign.Center;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 5].VerticalAlign = VerticalAlign.Middle;
                                    }
                                    if (attnd_hr == 0 && od_count == 0)
                                    {
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 4].Text = "-";
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 4].Locked = true;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 4].Font.Name = "Book Antiqua";
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 4].HorizontalAlign = HorizontalAlign.Center;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 4].VerticalAlign = VerticalAlign.Middle;
                                    }
                                    else
                                    {
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 4].Text = (attnd_hr + od_count).ToString();
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 4].Locked = true;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 4].Font.Name = "Book Antiqua";
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 4].HorizontalAlign = HorizontalAlign.Center;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 4].VerticalAlign = VerticalAlign.Middle;
                                    }
                                    if (attnd_hr == 0 && tot_hr == 0)
                                    {
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 3].Text = "-";
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 3].Locked = true;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 3].Font.Name = "Book Antiqua";
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 3].HorizontalAlign = HorizontalAlign.Center;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 3].VerticalAlign = VerticalAlign.Middle;
                                    }
                                    else
                                    {
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 3].Text = (tot_hr - (attnd_hr + od_count)).ToString();  //(tot_hr - attnd_hr).ToString();
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 3].Locked = true;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 3].Font.Name = "Book Antiqua";
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 3].HorizontalAlign = HorizontalAlign.Center;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 3].VerticalAlign = VerticalAlign.Middle;
                                    }
                                    FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 7].Text = tot_hr.ToString();
                                    double hrsCon = 0;
                                    if (!htTotalConductedHrs.Contains(roll_no.Trim().ToLower()))
                                    {
                                        htTotalConductedHrs.Add(roll_no.Trim().ToLower(), tot_hr);
                                    }
                                    else
                                    {
                                        hrsCon = Convert.ToDouble(GetCorrespondingKey(roll_no.ToLower().Trim(), htTotalConductedHrs));
                                        hrsCon += tot_hr;
                                        //htTotalConductedHrs.Add(roll_no.Trim(), hrsCon);
                                        htTotalConductedHrs[roll_no.Trim().ToLower()] = hrsCon;
                                    }
                                    FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 7].Locked = true;
                                    FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 7].Font.Name = "Book Antiqua";
                                    FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 7].HorizontalAlign = HorizontalAlign.Center;
                                    FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 7].VerticalAlign = VerticalAlign.Middle;
                                    if (FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 4].Text == "0" && tot_hr == 0)
                                    {
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 2].Text = "-";
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 2].Locked = true;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 2].Font.Name = "Book Antiqua";
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 2].VerticalAlign = VerticalAlign.Middle;
                                    }
                                    else
                                    {
                                        double avg_val = 0, avgstudent3 = 0;
                                        decimal avgstudent1 = 0, avgstudent2 = 0;
                                        double attnd_perc = 0;
                                        avg_val = (((attnd_hr + od_count) / tot_hr) * 100);
                                        if (double.IsInfinity(avg_val) || double.IsNaN(avg_val) || double.IsNegativeInfinity(avg_val) || double.IsPositiveInfinity(avg_val))
                                        {
                                            //avgstudent1 = Convert.ToDecimal(avg_val);
                                            //avgstudent2 = Math.Round(avgstudent1);
                                            //avgstudent3 = Convert.ToDouble(avgstudent2);
                                            //attnd_perc = Convert.ToString(avgstudent3);
                                            attnd_perc = 0;
                                        }
                                        else
                                        {
                                            attnd_perc = Math.Round(avg_val, 2);
                                        }
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 2].Text = attnd_perc.ToString();
                                        double hrsPer = 0;
                                        if (!httotalStudPercentage.Contains(roll_no.Trim().ToLower()))
                                        {
                                            httotalStudPercentage.Add(roll_no.Trim().ToLower(), attnd_perc);
                                        }
                                        else
                                        {
                                            hrsPer = Convert.ToDouble(GetCorrespondingKey(roll_no.Trim().ToLower(), httotalStudPercentage));
                                            hrsPer += attnd_perc;
                                            httotalStudPercentage[roll_no.Trim().ToLower()] = hrsPer;
                                        }
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 2].Locked = true;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 2].Font.Name = "Book Antiqua";
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 2].VerticalAlign = VerticalAlign.Middle;
                                        //if (attMinShorage < attnd_perc)
                                        //{
                                        //    countInvisible++;
                                        //    FpExamEligiblity.Sheets[0].Rows[row_cnt].Visible = false;
                                        //    //.Cells[row_cnt, (subjectStartColumn - 1)]
                                        //}
                                        string status = string.Empty;
                                        bool isEligible = true;
                                        if (minAttendanceForEligibleExam > attnd_perc)
                                        {
                                            countInvisible++;
                                            isEligible = false;
                                        }
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 1].Text = (isEligible) ? "Eligible" : "Not Eligible";
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 1].Tag = isEligible;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 1].Locked = true;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 1].Font.Name = "Book Antiqua";
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 1].VerticalAlign = VerticalAlign.Middle;
                                        FpExamEligiblity.Sheets[0].Cells[row_cnt, subjectStartColumn - 1].ForeColor = (isEligible) ? Color.Green : Color.Red;
                                        FpExamEligiblity.Sheets[0].Rows[row_cnt].Visible = true;
                                        if (ddlShowReport.Items.Count > 0)
                                        {
                                            int value = ddlShowReport.SelectedIndex;
                                            switch (value)
                                            {
                                                case 0:
                                                    break;
                                                case 1:
                                                    if (!isEligible)
                                                    {
                                                        FpExamEligiblity.Sheets[0].Rows[row_cnt].Visible = false;
                                                    }
                                                    break;
                                                case 2:
                                                    if (isEligible)
                                                    {
                                                        FpExamEligiblity.Sheets[0].Rows[row_cnt].Visible = false;
                                                    }
                                                    break;
                                            }
                                        }
                                    }

                                }

                                //if (!check_row_visible)
                                //{
                                //    divMainContents.Visible = false;
                                //    lblAlertMsg.Text = "No Record(s) Were Found";
                                //    lblAlertMsg.Visible = true;
                                //    divPopAlert.Visible = true;
                                //    return;
                                //}
                                //for (int row_visible = 0; row_visible < FpExamEligiblity.Sheets[0].RowCount; row_visible++)
                                //{
                                //    if (FpExamEligiblity.Sheets[0].Rows[row_visible].Visible == true)
                                //    {
                                //        rowValue++;
                                //        check_row_visible = true;
                                //        FpExamEligiblity.Sheets[0].Cells[row_visible, 0].Text = rowValue.ToString();
                                //    }
                                //}
                                //if (!check_row_visible || countInvisible == FpExamEligiblity.Sheets[0].RowCount)
                                //{  
                                //    divMainContents.Visible = false;
                                //    lblAlertMsg.Text = "No Record(s) Were Found";
                                //    lblAlertMsg.Visible = true;
                                //    divPopAlert.Visible = true;
                                //    return;
                                //    return;
                                //}
                            }

                            for (int roll = 0; roll < FpExamEligiblity.Sheets[0].RowCount; roll++)
                            {
                                if (roll == 0)
                                {
                                    FpExamEligiblity.Sheets[0].ColumnCount = FpExamEligiblity.Sheets[0].ColumnCount + 4;
                                    FpExamEligiblity.Sheets[0].ColumnHeader.Cells[0, FpExamEligiblity.Sheets[0].ColumnCount - 4].Text = "Tot Con";
                                    FpExamEligiblity.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExamEligiblity.Sheets[0].ColumnCount - 4, 2, 1);
                                    FpExamEligiblity.Sheets[0].Columns[FpExamEligiblity.Sheets[0].ColumnCount - 4].Resizable = false;
                                    FpExamEligiblity.Sheets[0].Columns[FpExamEligiblity.Sheets[0].ColumnCount - 4].Locked = true;
                                    FpExamEligiblity.Sheets[0].Columns[FpExamEligiblity.Sheets[0].ColumnCount - 4].Width = 100;

                                    FpExamEligiblity.Sheets[0].ColumnHeader.Cells[0, FpExamEligiblity.Sheets[0].ColumnCount - 3].Text = "Tot. Present";
                                    FpExamEligiblity.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExamEligiblity.Sheets[0].ColumnCount - 3, 2, 1);
                                    FpExamEligiblity.Sheets[0].Columns[FpExamEligiblity.Sheets[0].ColumnCount - 3].Resizable = false;
                                    FpExamEligiblity.Sheets[0].Columns[FpExamEligiblity.Sheets[0].ColumnCount - 3].Locked = true;
                                    FpExamEligiblity.Sheets[0].Columns[FpExamEligiblity.Sheets[0].ColumnCount - 3].Width = 100;

                                    FpExamEligiblity.Sheets[0].ColumnHeader.Cells[0, FpExamEligiblity.Sheets[0].ColumnCount - 2].Text = "  \t\t%\t\t  ";
                                    FpExamEligiblity.Sheets[0].ColumnHeader.Cells[0, FpExamEligiblity.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                    FpExamEligiblity.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExamEligiblity.Sheets[0].ColumnCount - 2, 2, 1);
                                    FpExamEligiblity.Sheets[0].Columns[FpExamEligiblity.Sheets[0].ColumnCount - 2].Resizable = false;
                                    FpExamEligiblity.Sheets[0].Columns[FpExamEligiblity.Sheets[0].ColumnCount - 2].Locked = true;
                                    FpExamEligiblity.Sheets[0].Columns[FpExamEligiblity.Sheets[0].ColumnCount - 2].Width = 100;

                                    FpExamEligiblity.Sheets[0].ColumnHeader.Cells[0, FpExamEligiblity.Sheets[0].ColumnCount - 1].Text = "Remarks";
                                    FpExamEligiblity.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExamEligiblity.Sheets[0].ColumnCount - 1, 2, 1);
                                    FpExamEligiblity.Sheets[0].Columns[FpExamEligiblity.Sheets[0].ColumnCount - 1].Resizable = false;
                                    FpExamEligiblity.Sheets[0].Columns[FpExamEligiblity.Sheets[0].ColumnCount - 1].Locked = true;
                                    FpExamEligiblity.Sheets[0].Columns[FpExamEligiblity.Sheets[0].ColumnCount - 1].Width = 100;
                                }
                                roll_no = FpExamEligiblity.Sheets[0].Cells[roll, 2].Text.ToString().Trim();
                                double totConducted = 0;
                                double overallPresent = 0;
                                double overallPercentage = 0;
                                if (htTotalConductedHrs.Contains(roll_no.Trim().ToLower()))
                                {
                                    totConducted = Convert.ToDouble(GetCorrespondingKey(roll_no.Trim().ToLower(), htTotalConductedHrs));
                                    FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 4].Text = Convert.ToString(totConducted);
                                }
                                else
                                {
                                    FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 4].Text = Convert.ToString("--");
                                }
                                FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 4].Locked = true;
                                FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 4].Font.Name = "Book Antiqua";
                                FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                                FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 4].VerticalAlign = VerticalAlign.Middle;
                                if (httotalStudPresentHrs.Contains(roll_no.Trim().ToLower()))
                                {
                                    overallPresent = Convert.ToDouble(GetCorrespondingKey(roll_no.Trim().ToLower(), httotalStudPresentHrs));
                                    FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 3].Text = Convert.ToString(overallPresent);
                                }
                                else
                                {
                                    FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 3].Text = Convert.ToString("--");
                                }
                                FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 3].Locked = true;
                                FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 3].Font.Name = "Book Antiqua";
                                FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                                FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                                double avg = 0;
                                if (overallPresent > 0 && totConducted > 0)
                                {
                                    avg = (overallPresent / totConducted) * 100;
                                    avg = Math.Round(avg, 2, MidpointRounding.AwayFromZero);
                                }
                                FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 2].Text = Convert.ToString(avg);
                                FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 2].Locked = true;
                                FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                FpExamEligiblity.Sheets[0].Cells[roll, FpExamEligiblity.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
                                if (FpExamEligiblity.Sheets[0].Rows[roll].Visible == true)
                                {
                                    rowValue++;
                                    check_row_visible = true;
                                    FpExamEligiblity.Sheets[0].Cells[roll, 0].Text = rowValue.ToString();
                                }
                            }
                            if (!check_row_visible)
                            {
                                divMainContents.Visible = false;
                                lblAlertMsg.Text = "No Record(s) Were Found";
                                lblAlertMsg.Visible = true;
                                divPopAlert.Visible = true;
                                return;
                            }
                        }
                        FpExamEligiblity.SaveChanges();
                        FpExamEligiblity.Sheets[0].PageSize = FpExamEligiblity.Sheets[0].RowCount;
                        FpExamEligiblity.Height = 500;
                        FpExamEligiblity.SaveChanges();
                        FpExamEligiblity.Visible = true;
                        divMainContents.Visible = true;
                    }
                    else
                    {
                        lblErrSearch.Text = string.Empty;
                        lblErrSearch.Visible = false;
                        divMainContents.Visible = false;
                        lblAlertMsg.Text = "Update Master Setting";
                        lblAlertMsg.Visible = true;
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    divMainContents.Visible = false;
                    lblAlertMsg.Text = "No Record(s) Were Found";
                    lblAlertMsg.Visible = true;
                    divPopAlert.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion

    #region Save

    protected void btnSaveEligiblity_Click(object sender, EventArgs e)
    {
        try
        {
            bool isSaved = false;
            int result = 0;
            examYear = string.Empty;
            if (ddlExamYear.Items.Count > 0)
            {
                examYear = Convert.ToString(ddlExamYear.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblExamYear.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            examMonth = string.Empty;
            if (ddlExamMonth.Items.Count > 0)
            {
                examMonth = Convert.ToString(ddlExamMonth.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblExamMonth.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            for (int rows = 0; rows < FpExamEligiblity.Sheets[0].RowCount; rows++)
            {
                string degreeCode = Convert.ToString(FpExamEligiblity.Sheets[0].Cells[rows, 0].Tag).Trim();
                string batchYear = Convert.ToString(FpExamEligiblity.Sheets[0].Cells[rows, 1].Tag).Trim();
                string rollNo = Convert.ToString(FpExamEligiblity.Sheets[0].Cells[rows, 2].Text).Trim();
                string appNo = Convert.ToString(FpExamEligiblity.Sheets[0].Cells[rows, 2].Tag).Trim();
                string Eligible = string.Empty;
                string examCode = da.GetFunction("select ed.exam_code from Exam_Details ed where ed.degree_code='" + degreeCode + "' and ed.batch_year='" + batchYear + "' and ed.Exam_Month='" + examMonth + "' and ed.Exam_year='" + examYear + "'").Trim();
                for (int col = 6; col < FpExamEligiblity.Sheets[0].ColumnCount - 4; col += 7)
                {
                    string subjectNo = Convert.ToString(FpExamEligiblity.Sheets[0].ColumnHeader.Cells[0, col].Tag).Trim();

                    Eligible = Convert.ToString(FpExamEligiblity.Sheets[0].Cells[rows, col + 6].Tag).Trim();
                    string workingHr = Convert.ToString(FpExamEligiblity.Sheets[0].Cells[rows, col].Text).Trim();
                    bool isEligible = false;
                    bool.TryParse(Eligible, out isEligible);
                    string presentHr = Convert.ToString(FpExamEligiblity.Sheets[0].Cells[rows, col + 3].Text).Trim();
                    string absentHr = Convert.ToString(FpExamEligiblity.Sheets[0].Cells[rows, col + 4].Text).Trim();
                    string percentageHr = Convert.ToString(FpExamEligiblity.Sheets[0].Cells[rows, col + 5].Text).Trim();
                    if (!string.IsNullOrEmpty(examCode) && examCode != "0")
                    {
                        qry = "if exists(select * from coeSubjectWiseExamEligibility where appNo='" + appNo + "' and subjectNo='" + subjectNo + "' and examCode='" + examCode + "') update coeSubjectWiseExamEligibility set isEligible='" + isEligible + "',workingHours='" + workingHr + "',presentHours='" + presentHr + "',absentHours='" + absentHr + "',percentageHour='" + percentageHr + "' where appNo='" + appNo + "' and subjectNo='" + subjectNo + "' and examCode='" + examCode + "' else insert into coeSubjectWiseExamEligibility (appNo,subjectNo,examCode,isEligible,workingHours,presentHours,absentHours,percentageHour) values('" + appNo + "','" + subjectNo + "','" + examCode + "','" + isEligible + "','" + workingHr + "','" + presentHr + "','" + absentHr + "','" + percentageHr + "')";
                        result = 0;
                        result = da.update_method_wo_parameter(qry, "text");
                        if (result != 0)
                        {
                            isSaved = true;
                        }
                    }
                }
            }
            if (isSaved)
            {
                lblAlertMsg.Text = "Saved Successfully";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                lblAlertMsg.Text = "Not Saved";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Save

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
                if (FpExamEligiblity.Visible == true)
                {
                    da.printexcelreport(FpExamEligiblity, reportname);
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            rptheadname = "Subject Wise Exam Eligibility Report";
            string pagename = "COESubjectWiseExamEligibility.aspx";
            //string Course_Name = Convert.ToString(cblDegree.SelectedItem).Trim();
            //rptheadname += "@ " + Course_Name + " - " + Convert.ToString(ddlbranch.SelectedItem).Trim() + "@ " + " Year of Admission : " + Convert.ToString(ddlbatch.SelectedItem).Trim() + "@ " + " Semester : " + Convert.ToString(ddlsem.SelectedItem).Trim();
            if (FpExamEligiblity.Visible == true)
            {
                printMaster1.loadspreaddetails(FpExamEligiblity, pagename, rptheadname);
            }
            printMaster1.Visible = true;
            lblExcelError.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            lbl.Add(lblCollege);
            lbl.Add(lblDegree);
            lbl.Add(lblBranch);
            lbl.Add(lblSem);
            fields.Add(0);
            fields.Add(2);
            fields.Add(3);
            fields.Add(4);
            if (institute != null && institute.TypeInstitute == 1)
            {
                lblBatch.Text = "Year";
            }
            else
            {
                lblBatch.Text = "Batch";
            }
            new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        try
        {
            IDictionaryEnumerator e = hashTable.GetEnumerator();
            while (e.MoveNext())
            {
                if (Convert.ToString(e.Key).Trim() == Convert.ToString(key).Trim())
                {
                    return e.Value;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
        return null;
    }

    public string Attmark(string Attstr_mark)
    {
        Att_mark = string.Empty;
        if (Attstr_mark == "1")
        {
            Att_mark = "P";
        }
        else if (Attstr_mark == "2")
        {
            Att_mark = "A";
        }
        else if (Attstr_mark == "3")
        {
            Att_mark = "OD";
        }
        else if (Attstr_mark == "4")
        {
            Att_mark = "ML";
        }
        else if (Attstr_mark == "5")
        {
            Att_mark = "SOD";
        }
        else if (Attstr_mark == "6")
        {
            Att_mark = "NSS";
        }
        else if (Attstr_mark == "7")
        {
            Att_mark = "H";
        }
        else if (Attstr_mark == "8")
        {
            Att_mark = "NJ";
        }
        else if (Attstr_mark == "9")
        {
            Att_mark = "S";
        }
        else if (Attstr_mark == "10")
        {
            Att_mark = "L";
        }
        else if (Attstr_mark == "11")
        {
            Att_mark = "NCC";
        }
        else if (Attstr_mark == "12")
        {
            Att_mark = "HS";
        }
        else if (Attstr_mark == "13")
        {
            Att_mark = "PP";
        }
        else if (Attstr_mark == "14")
        {
            Att_mark = "SYOD";
        }
        else if (Attstr_mark == "15")
        {
            Att_mark = "COD";
        }
        else if (Attstr_mark == "16")
        {
            Att_mark = "OOD";
        }
        else
        {
            Att_mark = "NE";
        }
        //return Convert.ToInt32(Att_mark);
        return Att_mark;
    }

    public string Attvalues(string Att_str1)
    {
        string Attvalue;
        Attvalue = string.Empty;
        if (Att_str1 == "P")
        {
            Attvalue = "1";
        }
        else if (Att_str1 == "A")
        {
            Attvalue = "2";
        }
        else if (Att_str1 == "OD")
        {
            Attvalue = "3";
        }
        else if (Att_str1 == "ML")
        {
            Attvalue = "4";
        }
        else if (Att_str1 == "SOD")
        {
            Attvalue = "5";
        }
        else if (Att_str1 == "NSS")
        {
            Attvalue = "6";
        }
        else if (Att_str1 == "H")
        {
            Attvalue = "7";
        }
        else if (Att_str1 == "NJ")
        {
            Attvalue = "8";
        }
        else if (Att_str1 == "S")
        {
            Attvalue = "9";
        }
        else if (Att_str1 == "L")
        {
            Attvalue = "10";
        }
        else if (Att_str1 == "NCC")
        {
            Attvalue = "11";
        }
        else if (Att_str1 == "HS")
        {
            Attvalue = "12";
        }
        else if (Att_str1 == "PP")
        {
            Attvalue = "13";
        }
        else if (Att_str1 == "SYOD")
        {
            Attvalue = "14";
        }
        else if (Att_str1 == "COD")
        {
            Attvalue = "15";
        }
        else if (Att_str1 == "OOD")
        {
            Attvalue = "16";
        }
        else
        {
            Attvalue = "NE";
        }
        return Attvalue;
    }

    public void CalculateAttendance(DataSet dsTheoryAlter, DataSet dsPracticalAlter, DataSet dsTheorySchedule, DataSet dsPracticalSchedule, string strOrderBy, DataSet dsAttndanceMaster, DataSet dsSplHrRights1, DataSet dsSem, DataSet dsSemesterSchedule1, DataSet dsAlterSchedule1, DataSet dsCurrentLab, DateTime dtFromDate, DateTime dtToDate, string subjectNo, ref Hashtable has_load_rollno, ref Hashtable has_total_attnd_hour, ref Hashtable has_od)
    {
        try
        {
            HashValueToZero(has_load_rollno);
            HashValueToZero(has_total_attnd_hour);
            HashValueToZero(has_od);
            //string orderBySetting = d2.GetFunction("select value from master_Settings where settings='order_by'");
            //string strOrderBy = " ORDER BY r.roll_no";
            //string serialno = d2.GetFunction("select LinkValue from inssettings where college_code='" + collegeCode + "' and linkname='Student Attendance'");
            //if (serialno == "1")
            //{
            //    strOrderBy = "ORDER BY r.serialno";
            //}
            //else
            //{
            //    if (orderBySetting == "0")
            //    {
            //        strOrderBy = "ORDER BY r.roll_no";
            //    }
            //    else if (orderBySetting == "1")
            //    {
            //        strOrderBy = "ORDER BY r.Reg_No";
            //    }
            //    else if (orderBySetting == "2")
            //    {
            //        strOrderBy = "ORDER BY r.Stud_Name";
            //    }
            //    else if (orderBySetting == "0,1,2")
            //    {
            //        strOrderBy = "ORDER BY r.roll_no,r.Reg_No,r.Stud_Name";
            //    }
            //    else if (orderBySetting == "0,1")
            //    {
            //        strOrderBy = "ORDER BY r.roll_no,r.Reg_No";
            //    }
            //    else if (orderBySetting == "1,2")
            //    {
            //        strOrderBy = "ORDER BY r.Reg_No,r.Stud_Name";
            //    }
            //    else if (orderBySetting == "0,2")
            //    {
            //        strOrderBy = "ORDER BY r.roll_no,r.Stud_Name";
            //    }
            //}
            string temp_tag = "", rstrsec = string.Empty;
            dtTempDate = dtFromDate;
            string subject_no = subjectNo;
            //if (ddlSection.Items.Count > 0)
            //{
            //    if (Convert.ToString(ddlSection.SelectedItem.Text).Trim() == "-1" || Convert.ToString(ddlSection.SelectedItem.Text).Trim() == "")
            //    {
            //        strsec = string.Empty;
            //        rstrsec = string.Empty;
            //    }
            //    else
            //    {
            //        strsec = " and sections='" + Convert.ToString(ddlSection.SelectedItem.Text).Trim() + "'";
            //        rstrsec = " and r.sections='" + Convert.ToString(ddlSection.SelectedItem.Text).Trim() + "'";
            //        section_lab = " and l.sections='" + Convert.ToString(ddlSection.SelectedItem.Text).Trim() + "'";
            //    }
            //}
            if (ddlSection.Items.Count > 0 && ddlSection.Visible)
            {
                sections = string.Empty;
                foreach (ListItem li in ddlSection.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(sections))
                        {
                            sections = "'" + li.Value + "'";
                        }
                        else
                        {
                            sections += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sections))
                {
                    qrySection = " and r.sections in(" + sections + ")";
                    strsec = " and sections in(" + sections + ")";
                    rstrsec = " and r.sections in(" + sections + ")";
                    section_lab = " and l.sections in(" + sections + ")";
                }
            }
            else if (cblSec.Items.Count > 0 && txtSec.Enabled && txtSec.Visible)
            {
                sections = getCblSelectedValue(cblSec);
                if (!string.IsNullOrEmpty(sections))
                {
                    qrySection = " and r.sections in(" + sections + ")";
                    strsec = " and sections in(" + sections + ")";
                    rstrsec = " and r.sections in(" + sections + ")";
                    section_lab = " and l.sections in(" + sections + ")";
                }
            }
            Hashtable hatlab = new Hashtable();
            if (dsCurrentLab.Tables.Count > 0 && dsCurrentLab.Tables[0].Rows.Count > 0)
            {
                for (int l = 0; l < dsCurrentLab.Tables[0].Rows.Count; l++)
                {
                    string strSubNo = Convert.ToString(dsCurrentLab.Tables[0].Rows[l]["subject_no"]).Trim();
                    if (!hatlab.Contains(strSubNo))
                    {
                        hatlab.Add(strSubNo, strSubNo);
                    }
                }
            }
            string semstartdate = string.Empty;
            string noofdays = string.Empty;
            string startday = string.Empty;
            if (dsSem.Tables.Count > 0)
            {
                if (dsSem.Tables.Count > 0 && dsSem.Tables[0].Rows.Count > 0)
                {
                    semstartdate = Convert.ToString(dsSem.Tables[0].Rows[0]["start_date"]).Trim();
                    noofdays = Convert.ToString(dsSem.Tables[0].Rows[0]["nodays"]).Trim();
                    startday = Convert.ToString(dsSem.Tables[0].Rows[0]["starting_dayorder"]).Trim();
                }
                try
                {
                    if (dsSem.Tables.Count > 1 && dsSem.Tables[1].Rows.Count > 0)
                    {
                        for (int dc = 0; dc < dsSem.Tables[1].Rows.Count; dc++)
                        {
                            DateTime dtdcf = Convert.ToDateTime(Convert.ToString(dsSem.Tables[1].Rows[dc]["from_date"]).Trim());
                            DateTime dtdct = Convert.ToDateTime(Convert.ToString(dsSem.Tables[1].Rows[dc]["to_date"]).Trim());
                            for (DateTime dtc = dtdcf; dtc <= dtdct; dtc = dtc.AddDays(1))
                            {
                                if (!hatdc.Contains(dtc))
                                {
                                    hatdc.Add(dtc, dtc);
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            if (chkflag == false)
            {
                chkflag = true;
                int count_master = 0;
                if (dsAttndanceMaster.Tables.Count > 0)
                {
                    count_master = (dsAttndanceMaster.Tables[0].Rows.Count);
                    if (count_master > 0)
                    {
                        for (count_master = 0; count_master < dsAttndanceMaster.Tables[0].Rows.Count; count_master++)
                        {
                            if (Convert.ToString(dsAttndanceMaster.Tables[0].Rows[count_master]["calcflag"]).Trim() == "0")
                            {
                                has_attnd_masterset.Add(Convert.ToString(dsAttndanceMaster.Tables[0].Rows[count_master]["leavecode"]).Trim(), Convert.ToString(dsAttndanceMaster.Tables[0].Rows[count_master]["leavecode"]).Trim());
                            }
                            if (Convert.ToString(dsAttndanceMaster.Tables[0].Rows[count_master]["calcflag"]).Trim() == "2")
                            {
                                if (!has_attnd_masterset_notconsider.ContainsKey(Convert.ToString(dsAttndanceMaster.Tables[0].Rows[count_master]["leavecode"]).Trim()))
                                {
                                    has_attnd_masterset_notconsider.Add(Convert.ToString(dsAttndanceMaster.Tables[0].Rows[count_master]["leavecode"]).Trim(), Convert.ToString(dsAttndanceMaster.Tables[0].Rows[count_master]["leavecode"]).Trim());
                                }
                            }
                        }
                    }
                }
                //string grouporusercode =string.Empty;
                //if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                //{
                //    grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
                //}
                //else
                //{
                //    grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                //}
                //string qryNew = "select rights from  special_hr_rights where " + grouporusercode + "";
                DataSet dsSplHrRights = new DataSet();
                dsSplHrRights = dsSplHrRights1;
                //dsSplHrRights = d2.select_method_wo_parameter(qryNew, "Text");
                if (dsSplHrRights.Tables.Count > 0 && dsSplHrRights.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr_rights_spl_hr in dsSplHrRights.Tables[0].Rows)
                    {
                        string spl_hr_rights = string.Empty;
                        Hashtable od_has = new Hashtable();
                        spl_hr_rights = Convert.ToString(dr_rights_spl_hr["rights"]).Trim();
                        if (spl_hr_rights.Trim().ToLower() == "true" || spl_hr_rights.Trim().ToLower() == "1")
                        {
                            splhr_flag = true;
                        }
                    }
                }
            }
            while (dtTempDate <= dtToDate)
            {
                if (!hatdc.Contains(dtTempDate))
                {
                    if (splhr_flag == true)
                    {
                        if (htSplHr.Contains(Convert.ToString(dtTempDate).Trim()))
                        {
                            getspecial_hr(htSplHr);
                        }
                    }
                    span_count = 0;
                    if (!hat_holy.ContainsKey(dtTempDate))
                    {
                        if (!hat_holy.ContainsKey(dtTempDate))
                        {
                            hat_holy.Add(dtTempDate, "3*0*0");
                        }
                    }
                    value_holi_status = GetCorrespondingKey(dtTempDate, hat_holy).ToString();
                    split_holiday_status = value_holi_status.Split('*');
                    if (split_holiday_status[0].ToString() == "3")//=========ful day working day
                    {
                        split_holiday_status_1 = 1;
                        split_holiday_status_2 = noOfHours;
                    }
                    else if (split_holiday_status[0].ToString() == "1")//=============half day working day
                    {
                        if (split_holiday_status[1].ToString() == "1")//==============mng holiday//evng working day
                        {
                            split_holiday_status_1 = firstHalf + 1;
                            split_holiday_status_2 = noOfHours;
                        }
                        if (split_holiday_status[2].ToString() == "1")//==============evng holiday//mng working day
                        {
                            split_holiday_status_1 = 1;
                            split_holiday_status_2 = firstHalf;
                        }
                    }
                    else if (split_holiday_status[0].ToString() == "0")//=================fulday holiday
                    {
                        split_holiday_status_1 = 0;
                        split_holiday_status_2 = 0;
                    }
                    //------------------------------
                    if (split_holiday_status_1 == 0 && split_holiday_status_2 == 0)
                    {
                        //  temp_date = temp_date.AddDays(1);//Hidden by srinath 11/9/2014
                    }
                    else
                    {
                        //---------------alternate schedule
                        if (dsSemesterSchedule1.Tables.Count > 0 && dsSemesterSchedule1.Tables[0].Rows.Count > 0)
                        {
                            dsSemesterSchedule1.Tables[0].DefaultView.RowFilter = "FromDate <='" + dtTempDate + "'";
                            DataView dv = dsSemesterSchedule1.Tables[0].DefaultView;
                            dsSemesterSchedule.Clear();
                            dsSemesterSchedule = new DataSet();
                            dsSemesterSchedule.Tables.Add(dv.ToTable());
                        }
                        if (dsAlterSchedule1.Tables.Count > 0 && dsAlterSchedule1.Tables[0].Rows.Count > 0)
                        {
                            dsAlterSchedule1.Tables[0].DefaultView.RowFilter = "FromDate ='" + dtTempDate + "'";
                            DataView dv1 = dsAlterSchedule1.Tables[0].DefaultView;
                            dsAlterSchedule.Clear();
                            dsAlterSchedule = new DataSet();
                            dsAlterSchedule.Tables.Add(dv1.ToTable());
                        }
                        if (dsSemesterSchedule.Tables.Count > 0 && dsSemesterSchedule.Tables[0].Rows.Count > 0)
                        {
                            if (noOfHours > 0)
                            {
                                dummy_date = dtTempDate.ToString();
                                string[] dummy_date_split = dummy_date.Split(' ');
                                string[] final_date_string = dummy_date_split[0].Split('/');
                                dummy_date = final_date_string[1].ToString() + "/" + final_date_string[0].ToString() + "/" + final_date_string[2].ToString();
                                month_year = ((Convert.ToInt16(final_date_string[2].ToString()) * 12) + (Convert.ToInt16(final_date_string[0].ToString()))).ToString();
                                if (order != "0")
                                {
                                    strDay = dtTempDate.ToString("ddd");
                                }
                                else
                                {
                                    string[] sp = dummy_date.Split('/');
                                    string curdate = sp[1] + '/' + sp[0] + '/' + sp[2];
                                    strDay = da.findday(curdate, degreeCodes.Replace("'", ""), semesters.Replace("'", ""), batchYears.Replace("'", ""), semstartdate, noofdays, startday);
                                }
                                for (int temp_hr = split_holiday_status_1; temp_hr <= split_holiday_status_2; temp_hr++)
                                {
                                    bool samehr_flag = false;
                                    roll_count = 0;
                                    present_count = 0;
                                    temp_hr_field = strDay + temp_hr;
                                    date_temp_field = "d" + final_date_string[1].ToString() + "d" + temp_hr;
                                    hatattendance.Clear();
                                    if (dsAlterSchedule.Tables.Count > 0 && dsAlterSchedule.Tables[0].Rows.Count > 0)
                                    {
                                        for (int hasrow = 0; hasrow < dsAlterSchedule.Tables[0].Rows.Count; hasrow++)
                                        {
                                            full_hour = dsAlterSchedule.Tables[0].Rows[hasrow][temp_hr_field].ToString();
                                            if (full_hour.Trim() != "")
                                            {
                                                temp_has_subj_code.Clear();
                                                string[] split_full_hour = full_hour.Split(';');
                                                bool batchflag = false;
                                                for (int g = 0; g <= split_full_hour.GetUpperBound(0); g++)
                                                {
                                                    string[] valhr = split_full_hour[g].ToString().Split('-');
                                                    if (valhr.GetUpperBound(0) > 1)
                                                    {
                                                        string lsub = valhr[0].ToString();
                                                        if (hatlab.Contains(lsub))
                                                        {
                                                            batchflag = true;
                                                        }
                                                    }
                                                }
                                                for (int semi_colon = 0; semi_colon <= split_full_hour.GetUpperBound(0); semi_colon++)
                                                {
                                                    roll_count = 0;
                                                    single_hour = split_full_hour[semi_colon].ToString();
                                                    string[] split_single_hour = single_hour.Split('-');
                                                    if (split_single_hour.GetUpperBound(0) >= 1)
                                                    {
                                                        check_alter = true;
                                                        if (split_single_hour[0].ToString().Trim() == subject_no.Trim())
                                                        {
                                                            if (!temp_has_subj_code.ContainsKey(subject_no.Trim()))
                                                            {
                                                                temp_has_subj_code.Add(subject_no.Trim(), subject_no.Trim());
                                                                //----------------------check lab allocation
                                                                recflag = true;
                                                                roll_count = 0;
                                                                if (samehr_flag == false)
                                                                {
                                                                    span_count++;
                                                                    samehr_flag = true;
                                                                }
                                                                //------------------------attendance
                                                                Hashtable has_stud_list = new Hashtable();
                                                                //------------------find subject type
                                                                if (batchflag == false)
                                                                {
                                                                    subj_type = "0";
                                                                }
                                                                else
                                                                {
                                                                    subj_type = "1";
                                                                }
                                                                //====================
                                                                if (subj_type.Trim().ToLower() != "1" && subj_type.Trim() != "True" && subj_type.Trim().ToUpper() != "TRUE" && subj_type.Trim().ToLower() != "true")
                                                                {
                                                                    //===Raja 1
                                                                    string strquery = "select distinct r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from registration r ,subjectchooser s,attendance a  where r.roll_no=a.roll_no and s.roll_no=a.roll_no and a.month_year='" + month_year + "' and r.degree_code='" + degreeCodes.Replace("'", "").Trim() + "' and batch_year='" + batchYears.Replace("'", "").Trim() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=s.roll_no and s.semester= '" + semesters.Replace("'", "").Trim() + "' " + strsec + " and  subject_no='" + subject_no + "' " + strOrderBy + "";
                                                                    DataSet dsquery = new DataSet();
                                                                    if (dsTheoryAlter.Tables.Count > 0 && dsTheoryAlter.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        dsTheoryAlter.Tables[0].DefaultView.RowFilter = "month_year='" + month_year + "' and  subject_no='" + subject_no + "'";
                                                                        DataTable dtTemp = new DataTable();
                                                                        dtTemp = dsTheoryAlter.Tables[0].DefaultView.ToTable();
                                                                        dsquery.Tables.Add(dtTemp);
                                                                    }
                                                                    //dsquery = d2.select_method(strquery, hat, "Text");
                                                                    if (dsquery.Tables.Count > 0 && dsquery.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                        {
                                                                            string rollno = Convert.ToString(dsquery.Tables[0].Rows[i]["roll_no"]).Trim();
                                                                            if (!hatattendance.Contains(rollno.Trim().ToLower()))
                                                                            {
                                                                                //hatattendance.Add(rollno.Trim(), Convert.ToString(dsquery.Tables[0].Rows[i]["attvalue"]).Trim());
                                                                                hatattendance.Add(rollno.Trim().ToLower(), Convert.ToString(dsquery.Tables[0].Rows[i][date_temp_field]).Trim());
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    string strquery = "select distinct  r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from  registration r,subjectchooser_New s,laballoc_new l,attendance a  where a.roll_no=s.roll_no and r.roll_no=a.roll_no and a.month_year='" + month_year + "' and r.degree_code='" + degreeCodes.Replace("'", "").Trim() + "' and r.batch_year='" + batchYears.Replace("'", "").Trim() + "' and cc=0 and delflag=0 and exam_flag<>'debar'  " + rstrsec + " and r.roll_no=s.roll_no and s.subject_no='" + subject_no + "' and r.degree_code=l.degree_code and r.batch_year=l.batch_year and l.Semester=r.Current_Semester and s.subject_no =l.subject_no and hour_value='" + temp_hr + "' and day_value='" + strDay + "' and l.subject_no=" + subject_no + "  " + section_lab + " and FromDate ='" + dtTempDate + "' and l.fdate=s.fromdate " + strOrderBy + "";
                                                                    //strquery = "select distinct registration.roll_no,registration.app_no,registration.reg_no,registration.roll_admit ,registration.stud_name,registration.stud_type,registration.serialno,registration.degree_code,Registration.college_code from subjectchooser_New,sub_sem,subject,registration where fromdate='" + getdate + "' and  todate='" + getdate + "' and batch in(select   distinct stu_batch from laballoc_new where subject_no='" + subject_no + "'  and batch_year=" + byear + "  and hour_value=" + hr + "   " + strsec + "  and degree_code=" + degree_code + " and fdate='" + getdate + "' and  tdate='" + getdate + "' and day_value='" + Day_Var + "' ) and subjectchooser_New.subtype_no=sub_sem.subtype_no and  semester =  " + semester.ToString() + " and subjectchooser_New.subject_no=subject.subject_no and  registration.roll_no=subjectchooser_New.roll_no and  registration.current_semester=subjectchooser_New.semester and subjectchooser_New.subject_no='" + subject_no + "'   and adm_date<='" + sel_date + "'  and SubjectChooser_new.Semester=registration.current_semester " + strsec + " and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR'";
                                                                    //and s.batch=l.stu_batch
                                                                    //DataSet dsquery = d2.select_method(strquery, hat, "Text");
                                                                    DataSet dsquery = new DataSet();
                                                                    if (dsPracticalAlter.Tables.Count > 0 && dsPracticalAlter.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        dsPracticalAlter.Tables[0].DefaultView.RowFilter = "month_year='" + month_year + "' and  subject_no='" + subject_no + "' and FromDate ='" + dtTempDate + "' and hour_value='" + temp_hr + "'  and day_value='" + strDay + "'";
                                                                        DataTable dtTemp = new DataTable();
                                                                        dtTemp = dsPracticalAlter.Tables[0].DefaultView.ToTable();
                                                                        dsquery.Tables.Add(dtTemp);
                                                                    }
                                                                    //dsquery = d2.select_method(strquery, hat, "Text");
                                                                    if (dsquery.Tables.Count > 0 && dsquery.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                        {
                                                                            string rollno = Convert.ToString(dsquery.Tables[0].Rows[i]["roll_no"]).Trim();
                                                                            if (!hatattendance.Contains(rollno.Trim().ToLower()))
                                                                            {
                                                                                //hatattendance.Add(rollno.Trim(), Convert.ToString(dsquery.Tables[0].Rows[i]["attvalue"]).Trim());
                                                                                hatattendance.Add(rollno.Trim().ToLower(), Convert.ToString(dsquery.Tables[0].Rows[i][date_temp_field]).Trim());
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (hatattendance.Count > 0)
                                                                {
                                                                    for (int i = 0; i < FpExamEligiblity.Sheets[0].RowCount; i++)
                                                                    {
                                                                        string rollno = Convert.ToString(FpExamEligiblity.Sheets[0].Cells[i, 2].Text).Trim();
                                                                        if (hatattendance.Contains(rollno.Trim().ToLower()))
                                                                        {
                                                                            Admission_date = Convert.ToDateTime(FpExamEligiblity.Sheets[0].Cells[i, 1].Note.Trim());
                                                                            string attvalue = Convert.ToString(GetCorrespondingKey(rollno.Trim().ToLower(), hatattendance)).Trim();
                                                                            string value = Attmark(attvalue.Trim());
                                                                            if (dtTempDate >= Admission_date)
                                                                            {
                                                                                FpExamEligiblity.Sheets[0].Rows[i].Visible = true;
                                                                                if (attvalue == "3")
                                                                                {
                                                                                    temp_tag = "3";
                                                                                }
                                                                                else
                                                                                {
                                                                                    temp_tag = "0";
                                                                                }
                                                                                if ((attvalue.ToString()) != "8")
                                                                                {
                                                                                    if (value != "HS")
                                                                                    {
                                                                                        if (!has_attnd_masterset_notconsider.ContainsKey(attvalue.ToString().Trim()))
                                                                                        {
                                                                                            if (temp_tag == "0")
                                                                                            {
                                                                                                if (has_attnd_masterset.ContainsKey(attvalue.Trim()))
                                                                                                {
                                                                                                    present_count = Convert.ToInt16(GetCorrespondingKey(rollno.Trim().ToLower(), has_load_rollno));
                                                                                                    present_count++;
                                                                                                    has_load_rollno[rollno.ToLower().Trim()] = present_count;
                                                                                                }
                                                                                                if (value != "NE")
                                                                                                {
                                                                                                    present_count = Convert.ToInt16(GetCorrespondingKey(rollno.Trim().ToLower(), has_total_attnd_hour));
                                                                                                    present_count++;
                                                                                                    has_total_attnd_hour[rollno.Trim().ToLower()] = present_count;
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                if (!has_od.ContainsKey(FpExamEligiblity.Sheets[0].Cells[i, 2].Text.ToLower().Trim()))
                                                                                                {
                                                                                                    has_od.Add(rollno.Trim().ToLower(), 1);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    od_count = Convert.ToInt16(GetCorrespondingKey(rollno.Trim().ToLower(), has_od));
                                                                                                    od_count++;
                                                                                                    has_od[rollno.Trim().ToLower()] = od_count;
                                                                                                }
                                                                                                if (value != "NE")
                                                                                                {
                                                                                                    {
                                                                                                        present_count = Convert.ToInt16(GetCorrespondingKey(rollno.Trim().ToLower(), has_total_attnd_hour));
                                                                                                        present_count++;
                                                                                                        has_total_attnd_hour[rollno.Trim().ToLower()] = present_count;
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    samehr_flag = false;
                                    roll_count = 0;
                                    present_count = 0;
                                    if (check_alter == false)
                                    {
                                        full_hour = Convert.ToString(dsSemesterSchedule.Tables[0].Rows[0][temp_hr_field]).Trim();
                                        if (full_hour.Trim() != "")
                                        {
                                            temp_has_subj_code.Clear();
                                            string[] split_full_hour_sem = full_hour.Split(';');
                                            bool batchflag = false;
                                            for (int g = 0; g <= split_full_hour_sem.GetUpperBound(0); g++)
                                            {
                                                string[] valhr = split_full_hour_sem[g].ToString().Split('-');
                                                if (valhr.GetUpperBound(0) > 1)
                                                {
                                                    string lsub = Convert.ToString(valhr[0]).Trim();
                                                    if (hatlab.Contains(lsub))
                                                    {
                                                        batchflag = true;
                                                    }
                                                }
                                            }
                                            for (int semi_colon = 0; semi_colon <= split_full_hour_sem.GetUpperBound(0); semi_colon++)
                                            {
                                                roll_count = 0;
                                                single_hour = Convert.ToString(split_full_hour_sem[semi_colon]).Trim();
                                                string[] split_single_hour = single_hour.Split('-');
                                                if (split_single_hour.GetUpperBound(0) >= 1)
                                                {
                                                    if (Convert.ToString(split_single_hour[0]).Trim() == subject_no.Trim())
                                                    {
                                                        if (!temp_has_subj_code.ContainsKey(subject_no.Trim()))
                                                        {
                                                            temp_has_subj_code.Add(subject_no.Trim(), subject_no.Trim());
                                                            recflag = true;
                                                            if (samehr_flag == false)
                                                            {
                                                                span_count++;
                                                                samehr_flag = true;
                                                            }
                                                            Hashtable has_stud_list = new Hashtable();
                                                            if (batchflag == true)
                                                            {
                                                                subj_type = "1";
                                                            }
                                                            else
                                                            {
                                                                subj_type = "0";
                                                            }
                                                            if (subj_type != "1" && subj_type != "True" && subj_type != "TRUE" && subj_type != "true")
                                                            {
                                                                string strquery = "select distinct r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from registration r ,subjectchooser s,attendance a where r.roll_no=a.roll_no and a.roll_no=s.roll_no and a.month_year='" + month_year + "' and r.degree_code='" + degreeCodes.Replace("'", "").Trim() + "' and batch_year='" + batchYears.Replace("'", "").Trim() + "' and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=s.roll_no and s.semester= '" + semesters.Replace("'", "").Trim() + "' " + strsec + " and  subject_no='" + subject_no + "' " + strOrderBy + "";
                                                                //DataSet dsquery = d2.select_method(strquery, hat, "Text");
                                                                DataSet dsquery = new DataSet();
                                                                if (dsTheorySchedule.Tables.Count > 0 && dsTheorySchedule.Tables[0].Rows.Count > 0)
                                                                {
                                                                    dsTheorySchedule.Tables[0].DefaultView.RowFilter = "month_year='" + month_year + "' and  subject_no='" + subject_no + "'";
                                                                    DataTable dtTemp = new DataTable();
                                                                    dtTemp = dsTheorySchedule.Tables[0].DefaultView.ToTable();
                                                                    dsquery.Tables.Add(dtTemp);
                                                                }
                                                                //dsquery = d2.select_method(strquery, hat, "Text");
                                                                if (dsquery.Tables.Count > 0 && dsquery.Tables[0].Rows.Count > 0)
                                                                {
                                                                    for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                    {
                                                                        string rollno = Convert.ToString(dsquery.Tables[0].Rows[i]["Roll_no"]).Trim();
                                                                        if (!hatattendance.Contains(rollno.Trim().ToLower()))
                                                                        {
                                                                            //hatattendance.Add(rollno.Trim(), Convert.ToString(dsquery.Tables[0].Rows[i]["attvalue"]).Trim());
                                                                            hatattendance.Add(rollno.Trim().ToLower(), Convert.ToString(dsquery.Tables[0].Rows[i][date_temp_field]).Trim());
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                string strquery = "select distinct r.roll_no,a." + date_temp_field + " AS attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from  registration r,subjectchooser s,laballoc l,attendance a  where r.roll_no=a.roll_no and a.roll_no=s.roll_no and a.month_year='" + month_year + "' and r.degree_code='" + degreeCodes.Replace("'", "").Trim() + "' and r.batch_year='" + batchYears.Replace("'", "").Trim() + "' and cc=0 and delflag=0 and exam_flag<>'debar'  " + rstrsec + " and r.roll_no=s.roll_no and s.subject_no='" + subject_no + "' and r.degree_code=l.degree_code and r.batch_year=l.batch_year and l.Semester=r.Current_Semester and s.subject_no =l.subject_no and s.batch=l.stu_batch  and hour_value='" + temp_hr + "'  and day_value='" + strDay + "' and l.subject_no='" + subject_no + "' " + section_lab + " " + strOrderBy + "";
                                                                //and s.batch=l.stu_batch 
                                                                //DataSet dsquery = d2.select_method(strquery, hat, "Text");
                                                                //DataSet dsquery = d2.select_method(strquery, hat, "Text");
                                                                DataSet dsquery = new DataSet();
                                                                if (dsPracticalSchedule.Tables.Count > 0 && dsPracticalSchedule.Tables[0].Rows.Count > 0)
                                                                {
                                                                    dsPracticalSchedule.Tables[0].DefaultView.RowFilter = "month_year='" + month_year + "' and  subject_no='" + subject_no + "'  and hour_value='" + temp_hr + "'  and day_value='" + strDay + "' ";//and section='A'
                                                                    DataTable dtTemp = new DataTable();
                                                                    dtTemp = dsPracticalSchedule.Tables[0].DefaultView.ToTable();
                                                                    dsquery.Tables.Add(dtTemp);
                                                                }
                                                                //dsquery = d2.select_method(strquery, hat, "Text");
                                                                if (dsquery.Tables.Count > 0 && dsquery.Tables[0].Rows.Count > 0)
                                                                {
                                                                    for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                    {
                                                                        string rollno = Convert.ToString(dsquery.Tables[0].Rows[i]["Roll_no"]).Trim();
                                                                        if (!hatattendance.Contains(rollno.Trim().ToLower()))
                                                                        {
                                                                            //hatattendance.Add(rollno.Trim(), Convert.ToString(dsquery.Tables[0].Rows[i]["attvalue"]).Trim());
                                                                            hatattendance.Add(rollno.Trim().ToLower(), Convert.ToString(dsquery.Tables[0].Rows[i][date_temp_field]).Trim());
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (hatattendance.Count > 0)
                                                            {
                                                                for (int i = 0; i < FpExamEligiblity.Sheets[0].RowCount; i++)
                                                                {
                                                                    string rollno = Convert.ToString(FpExamEligiblity.Sheets[0].Cells[i, 2].Text).Trim();
                                                                    if (hatattendance.Contains(rollno.Trim().ToLower()))
                                                                    {
                                                                        Admission_date = Convert.ToDateTime(FpExamEligiblity.Sheets[0].Cells[i, 1].Note.Trim());
                                                                        string attvalue = Convert.ToString(GetCorrespondingKey(rollno.Trim().ToLower(), hatattendance)).Trim();
                                                                        string value = Attmark(Convert.ToString(attvalue).Trim());
                                                                        if (dtTempDate >= Admission_date)
                                                                        {
                                                                            FpExamEligiblity.Sheets[0].Rows[i].Visible = true;
                                                                            if (attvalue == "3")
                                                                            {
                                                                                temp_tag = "3";
                                                                            }
                                                                            else
                                                                            {
                                                                                temp_tag = "0";
                                                                            }
                                                                            if ((attvalue.ToString()) != "8")
                                                                            {
                                                                                if (value != "HS")
                                                                                {
                                                                                    if (temp_tag == "0")
                                                                                    {
                                                                                        if (has_attnd_masterset.ContainsKey(attvalue.Trim()))
                                                                                        {
                                                                                            if (temp_tag == "0")
                                                                                            {
                                                                                                present_count = Convert.ToInt16(GetCorrespondingKey(rollno.Trim().ToLower(), has_load_rollno));
                                                                                                present_count++;
                                                                                                has_load_rollno[rollno.ToLower().Trim()] = present_count;
                                                                                            }
                                                                                        }
                                                                                        if (value != "NE")
                                                                                        {
                                                                                            {
                                                                                                present_count = Convert.ToInt16(GetCorrespondingKey(rollno.Trim().ToLower(), has_total_attnd_hour));
                                                                                                present_count++;
                                                                                                has_total_attnd_hour[rollno.Trim().ToLower()] = present_count;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (!has_od.ContainsKey(rollno.Trim().ToLower()))
                                                                                        {
                                                                                            has_od.Add(rollno.ToLower().Trim(), 1);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            od_count = Convert.ToInt16(GetCorrespondingKey(rollno.Trim().ToLower(), has_od));
                                                                                            od_count++;
                                                                                            has_od[rollno.Trim().ToLower()] = od_count;
                                                                                        }
                                                                                        if (value != "NE")
                                                                                        {
                                                                                            present_count = Convert.ToInt16(GetCorrespondingKey(rollno.Trim().ToLower(), has_total_attnd_hour));
                                                                                            present_count++;
                                                                                            has_total_attnd_hour[rollno.Trim().ToLower()] = present_count;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    check_alter = false;
                                }
                            }
                        }
                    }
                }
                dtTempDate = dtTempDate.AddDays(1);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    private Hashtable HashValueToZero(Hashtable theHash)
    {
        object[] keys = new object[theHash.Keys.Count];
        theHash.Keys.CopyTo(keys, 0);
        foreach (object key in keys)
        {
            theHash[key] = 0;
        }
        return theHash;
    }

    public void getspecial_hr(Hashtable htSplHr)
    {
        try
        {
            string hrdetno = string.Empty;
            if (htSplHr.Contains(Convert.ToString(dtTempDate).Trim()))
            {
                hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(dtTempDate).Trim(), htSplHr));
            }
            if (hrdetno != "")
            {
                DataSet ds_splhr_query_master = new DataSet();
                string splhr_query_master = "select spa.roll_no,spa.attendance from  specialhr_attendance spa,specialhr_details spd where spa.hrdet_no=spd.hrdet_no and spd.subject_no='" + subject_no + "'  and spd.hrdet_no in(" + hrdetno + ") order by spa.roll_no asc";
                ds_splhr_query_master = da.select_method_wo_parameter(splhr_query_master, "text");
                //SqlDataReader dr_splhr_query_master;
                //cmd = new SqlCommand(splhr_query_master, con_splhr_query_master);
                //dr_splhr_query_master = cmd.ExecuteReader();
                if (ds_splhr_query_master.Tables.Count > 0 && ds_splhr_query_master.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr_splhr_query_master in ds_splhr_query_master.Tables[0].Rows)
                    {
                        if (hatsplhrattendance.Contains(Convert.ToString(dr_splhr_query_master[0]).Trim().ToLower()))
                        {
                            roll_count = Convert.ToInt32(GetCorrespondingKey(Convert.ToString(dr_splhr_query_master[0]).Trim().ToLower(), hatsplhrattendance));
                            recflag = true;
                            if (FpExamEligiblity.Sheets[0].Cells[roll_count, 2].Text.Trim().ToLower() == Convert.ToString(dr_splhr_query_master[0]).Trim().ToLower())
                            {
                                FpExamEligiblity.Sheets[0].Rows[roll_count].Visible = true;
                                if ((Convert.ToString(dr_splhr_query_master[1]).Trim()) != "8")
                                {
                                    if (GetAttendanceStatusName(Convert.ToString(dr_splhr_query_master[1]).Trim()) != "HS")
                                    {
                                        if (has_attnd_masterset.ContainsKey((Convert.ToString(dr_splhr_query_master[1]).Trim())))
                                        {
                                            present_count = Convert.ToInt16(has_load_rollno[Convert.ToString(dr_splhr_query_master[0]).Trim().ToLower()].ToString());
                                            present_count++;
                                            has_load_rollno[FpExamEligiblity.Sheets[0].Cells[roll_count, 2].Text.Trim().ToLower()] = present_count;
                                        }
                                        if (GetAttendanceStatusName(dr_splhr_query_master[1].ToString()) != "NE")
                                        {
                                            present_count = Convert.ToInt16(has_total_attnd_hour[Convert.ToString(dr_splhr_query_master[0]).Trim().ToLower()].ToString());
                                            present_count++;
                                            has_total_attnd_hour[FpExamEligiblity.Sheets[0].Cells[roll_count, 2].Text.Trim().ToLower()] = present_count;
                                        }
                                    }
                                }
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

}