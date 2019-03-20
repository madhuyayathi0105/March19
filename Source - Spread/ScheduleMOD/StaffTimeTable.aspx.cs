using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using wc = System.Web.UI.WebControls;


public partial class ScheduleMOD_StaffTimeTable : System.Web.UI.Page
{
    SqlConnection con2a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(Convert.ToString(ConfigurationManager.AppSettings["con"]));
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    static string collegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string strstaffcode = string.Empty;
    bool cellClick = false;
    static string code = "";
    Boolean allowCombineClass = false;
    static string selectedSubjectNo = "";
    static string selectedDept = string.Empty;
    static string selectedDesig = string.Empty;
    static string selectedCategory = string.Empty;
    Hashtable htData = new Hashtable();
    bool replace = false;
    bool isChanged = false;
    int status = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("ScheduleHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/ScheduleMOD/ScheduleHome.aspx");
                    return;
                }
            }
            collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            strstaffcode = Session["Staff_Code"].ToString();
            Session["StaffCode"] = Session["Staff_Code"].ToString();
            if (!IsPostBack)
            {
                bindCollege();
                bindDept();
                bindDesignation();
                bindStaffCategory();
                checkUser();
                getValues();
                spreadTimeTable.Visible = false;
                btnSave.Visible = false;
                divTreeView.Visible = false;
                gridSelTT.Visible = false;
                btnAdd.Visible = false;
            }
        }
        catch (Exception ex) { }
    }

    private void bindCollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = string.Empty;
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
                columnfield = " and group_code='" + group_code + "'";
            else
                columnfield = " and user_code='" + Session["usercode"] + "'";

            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception e) { }
    }
    protected void ddlcollege_change(object sender, EventArgs e)
    {
        collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
        bindDept();
        bindDesignation();
        bindStaffCategory();
        tdStfCodeAuto.Visible = true;
        tdStfNameAuto.Visible = false;

    }
    private void bindDept()
    {
        try
        {
            cbl_dept.Items.Clear();
            ds.Clear();

            string group_user = string.Empty;
            string cmd = string.Empty;
            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + usercode + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode + "') order by dept_name";
            }
            else
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode + "') order by dept_name";

            }
            ds = d2.select_method_wo_parameter(cmd, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds.Tables[0];
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "dept_code";
                    cbl_dept.DataBind();
                    checkBoxListselectOrDeselect(cbl_dept, true);
                    CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, Label2.Text, "--Select--");
                }
                else
                {
                    txt_dept.Text = "--Select--";
                    cb_dept.Checked = false;
                }
            }
        }
        catch { }
    }
    protected void cb_dept_CheckedChange(object sender, EventArgs e)
    {
        txt_scode.Text = "";
        txt_sname.Text = "";
        CallCheckboxChange(cb_dept, cbl_dept, txt_dept, "Department", "--Select--");
        bindDesignation();
        getValues();

    }
    protected void cbl_dept_SelectedIndexChange(object sender, EventArgs e)
    {
        txt_scode.Text = "";
        txt_sname.Text = "";
        CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, "Department", "--Select--");
        bindDesignation();
        getValues();

    }
    private void bindDesignation()
    {
        try
        {
            ds.Clear();
            cblDesig.Items.Clear();
            txtDesig.Text = "--Select--";
            cbDesig.Checked = false;
            string deptCodes = getCblSelectedValue(cbl_dept);
            //string statequery = "select desig_code,desig_name from desig_master where collegeCode='" + collegecode + "'and dept_code in ('" + deptCodes + "') order by desig_name";
            //ds = d2.select_method_wo_parameter(statequery, "Text");
            //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    cblDesig.DataSource = ds;
            //    cblDesig.DataTextField = "desig_name";
            //    cblDesig.DataValueField = "desig_code";
            //    cblDesig.DataBind();
            //    if (cblDesig.Items.Count > 0)
            //    {
            //        for (int i = 0; i < cblDesig.Items.Count; i++)
            //        {
            //            cblDesig.Items[i].Selected = true;
            //        }
            //        txtDesig.Text = "Designation (" + cblDesig.Items.Count + ")";
            //        cbDesig.Checked = true;
            //    }
            //}

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            {
                string[] arr = deptCodes.Split(new string[] { "','" }, StringSplitOptions.None);
                for (int i = 0; i < arr.Length; i++)
                {
                    dt1.Clear();
                    string deptcode = Convert.ToString(arr[i]);
                    string qq = "select desig_code,desig_name from desig_master where collegeCode='" + collegecode + "'and dept_code like '%" + deptcode + "%' order by desig_name";
                    dt1 = d2.select_method_wop_table(qq, "Text");
                    dt2.Merge(dt1);
                    dt3 = dt2.DefaultView.ToTable(true, "desig_code", "desig_name");
                }
                if (dt3.Rows.Count > 0)
                {
                    cblDesig.DataSource = dt3;
                    cblDesig.DataTextField = "desig_name";
                    cblDesig.DataValueField = "desig_code";
                    cblDesig.DataBind();
                    checkBoxListselectOrDeselect(cblDesig, true);
                    CallCheckboxListChange(cbDesig, cblDesig, txtDesig, Label3.Text, "--Select--");
                }
            }



        }
        catch { }
    }
    protected void cbDesig_CheckedChange(object sender, EventArgs e)
    {
        txt_scode.Text = "";
        txt_sname.Text = "";
        CallCheckboxChange(cbDesig, cblDesig, txtDesig, "Designation", "--Select--");
        getValues();

    }
    protected void cblDesig_SelectedIndexChange(object sender, EventArgs e)
    {
        txt_scode.Text = "";
        txt_sname.Text = "";
        CallCheckboxListChange(cbDesig, cblDesig, txtDesig, "Designation", "--Select--");
        getValues();

    }
    protected void bindStaffCategory()
    {
        try
        {
            ds.Clear();
            int count = 0;
            cblStfCategry.Items.Clear();
            string item = "select distinct category_name,category_code from staffcategorizer where  college_code='" + collegecode + "' ";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblStfCategry.DataSource = ds;
                cblStfCategry.DataTextField = "category_name";
                cblStfCategry.DataValueField = "category_code";
                cblStfCategry.DataBind();
                checkBoxListselectOrDeselect(cbl_dept, true);
                CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, Label2.Text, "--Select--");
            }
            else
            {
                txtStfCategry.Text = "--Select--";
                cbStfCategry.Checked = false;
            }
        }
        catch { }
    }
    protected void cbStfCategry_CheckedChange(object sender, EventArgs e)
    {
        txt_scode.Text = "";
        txt_sname.Text = "";
        CallCheckboxChange(cbStfCategry, cblStfCategry, txtStfCategry, "Staff Category", "--Select--");
        getValues();

    }
    protected void cblStfCategry_SelectedIndexChange(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbStfCategry, cblStfCategry, txtStfCategry, "Staff Category", "--Select--");
        getValues();
        txt_scode.Text = "";
        txt_sname.Text = "";
    }
    protected void ddlSearchOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSearchOption.SelectedIndex == 0)
        {
            tdStfCodeAuto.Visible = true;
            tdStfNameAuto.Visible = false;
            if (Convert.ToString(Session["Staff_Code"]) == "")
                txt_sname.Text = "";
        }
        else
        {
            tdStfCodeAuto.Visible = false;
            tdStfNameAuto.Visible = true;
            if (Convert.ToString(Session["Staff_Code"]) == "")
                txt_scode.Text = "";
        }
    }
    protected void ddltimetable_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string[] arr = Convert.ToString(Session["code"]).Split('-');
            string selectedBatch = Convert.ToString(arr[0]).Trim();
            string selectedDegCode = Convert.ToString(arr[1]).Trim();
            string selectedSem = Convert.ToString(arr[2]).Trim();
            string selectedSec = Convert.ToString(arr[3]).Trim();
            loaddetail(selectedBatch, selectedDegCode, selectedSem, selectedSec);
        }
        catch { }
    }

    public void getValues()
    {
        try
        {
            selectedDept = getCblSelectedValue(cbl_dept);
            selectedDesig = getCblSelectedValue(cblDesig);
            selectedCategory = getCblSelectedValue(cblStfCategry);
        }
        catch { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        //string query = "select staff_code from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_code like '" + prefixText + "%' and college_code='" + collegecode + "'";
        string query = "select distinct st.staff_code from stafftrans st, staffmaster sm where st.staff_code=sm.staff_code and resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and   dept_code in ('" + selectedDept + "') and desig_code in ('" + selectedDesig + "') and category_code in ('" + selectedCategory + "') and st.staff_code like '" + prefixText + "%' and college_code='" + collegecode + "'";

        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> stfName = new List<string>();
        //string query = "select staff_name  from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like '" + prefixText + "%' and college_code='" + collegecode + "'";
        string query = "select distinct sm.staff_name from stafftrans st, staffmaster sm where st.staff_code=sm.staff_code and resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and   dept_code in ('" + selectedDept + "') and desig_code in ('" + selectedDesig + "') and category_code in ('" + selectedCategory + "') and sm.staff_name like '" + prefixText + "%' and college_code='" + collegecode + "'";

        stfName = ws.Getname(query);
        return stfName;
    }

    protected void btnGo_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txt_scode.Text) && string.IsNullOrEmpty(txt_sname.Text))
            {
                lblAlertMsg.Text = "Enter staff code/Name";
                divPopAlert.Visible = true;
                btnSave.Visible = false;
                spreadTimeTable.Visible = false;
                divTreeView.Visible = false;
                gridSelTT.Visible = false;
                btnAdd.Visible = false;
                return;
            }
            loadTimeTableSpread();
            spreadTimeTable.Visible = true;
            btnSave.Visible = false;
            divTreeView.Visible = false;
            gridSelTT.Visible = false;
            btnAdd.Visible = false;
            if (status > 0)
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                lblAlertMsg.Text = "Saved Successfully";
                divPopAlert.Visible = true;
                btnSave.Visible = false;
                btndelete.Visible = false;
                //btnGo_OnClick(sender, e);
            }
        }
        catch { }
    }

    private void loadTimeTableSpread()
    {
        try
        {
            #region spread design
            spreadTimeTable.Sheets[0].AutoPostBack = true;
            spreadTimeTable.Sheets[0].ColumnHeader.RowCount = 1;
            spreadTimeTable.Sheets[0].ColumnCount = 1;
            spreadTimeTable.Sheets[0].RowCount = 0;
            spreadTimeTable.CommandBar.Visible = false;
            spreadTimeTable.Sheets[0].RowHeader.Visible = false;
            spreadTimeTable.Columns.Default.Font.Name = "Book Antiqua";
            spreadTimeTable.Columns.Default.Font.Size = FontUnit.Medium;
            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = FontUnit.Medium;
            style.Font.Bold = true;
            style.Font.Name = "Book Antiqua";
            style.HorizontalAlign = HorizontalAlign.Center;
            style.ForeColor = Color.Black;
            style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            spreadTimeTable.Sheets[0].ColumnHeader.DefaultStyle = style;
            spreadTimeTable.Sheets[0].Columns.Default.Font.Name = "Book Antiqua";
            spreadTimeTable.Sheets[0].Columns.Default.Font.Size = FontUnit.Medium;
            spreadTimeTable.Sheets[0].Rows.Default.Font.Name = "Book Antiqua";
            spreadTimeTable.Sheets[0].Rows.Default.Font.Size = FontUnit.Medium;
            spreadTimeTable.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Day/Week";
            spreadTimeTable.Sheets[0].Columns[0].ForeColor = Color.Black;
            spreadTimeTable.Sheets[0].Columns[0].Locked = true;
            #endregion

            if (Convert.ToString(Session["Staff_Code"]) == "")
            {
                if (txt_scode.Text.Trim() != "")
                    Session["StaffCode"] = txt_scode.Text.Trim();
                else
                {
                    string staff_Name = Convert.ToString(txt_sname.Text).Trim();
                    if (staff_Name != "")
                    {
                        string staff_Code = d2.GetFunction("select staff_code from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like '%" + staff_Name + "%' and college_code='" + collegecode + "'");
                        Session["StaffCode"] = staff_Code.Trim();
                        txt_scode.Text = staff_Code.Trim();
                    }
                }
            }

            htData.Clear();
            string[] DaysAcronym = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string[] DaysName = new string[7] { "Monday", "Tuesday", "wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            string sql = "select max(No_of_hrs_per_day)HoursPerDay,MAX(nodays)NoOfDays from PeriodAttndSchedule";
            DataSet ds = d2.select_method_wo_parameter(sql, "Text");
            int noOfHrs = 0;
            int noOfDays = 0;
            string dayvalue = string.Empty;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["HoursPerDay"].ToString().Trim() != "" && ds.Tables[0].Rows[0]["HoursPerDay"].ToString().Trim() != null && ds.Tables[0].Rows[0]["HoursPerDay"].ToString().Trim() != "0")
                {
                    noOfHrs = Convert.ToInt32(ds.Tables[0].Rows[0]["HoursPerDay"].ToString());
                    noOfDays = Convert.ToInt32(ds.Tables[0].Rows[0]["NoOfDays"].ToString());
                }
            }
            else
            {

            }
            if (noOfHrs != 0)
            {
                for (int i = 1; i <= noOfHrs; i++)
                {
                    spreadTimeTable.Sheets[0].ColumnCount = spreadTimeTable.Sheets[0].ColumnCount + 1;
                    spreadTimeTable.Sheets[0].ColumnHeader.Cells[0, spreadTimeTable.Sheets[0].ColumnCount - 1].Text = "Period " + Convert.ToString(i);
                }

            }

            string SchOrder = d2.GetFunction("select distinct top 1 schOrder from PeriodAttndSchedule");
            for (int day = 0; day < noOfDays; day++)
            {
                string dayName = DaysName[day];
                string dayAcronym = DaysAcronym[day];
                spreadTimeTable.Sheets[0].RowCount++;
                if (SchOrder == "1")
                {
                    spreadTimeTable.Sheets[0].Cells[spreadTimeTable.Sheets[0].RowCount - 1, 0].Text = dayName;
                    spreadTimeTable.Sheets[0].Cells[spreadTimeTable.Sheets[0].RowCount - 1, 0].Note = dayAcronym;
                }
                else
                {
                    int dayNo = day + 1;
                    spreadTimeTable.Sheets[0].Cells[spreadTimeTable.Sheets[0].RowCount - 1, 0].Text = "Day " + dayNo;
                }
            }
            DateTime cur_date = DateTime.Now;
            string strCurrDate = Convert.ToString(cur_date).Split(new Char[] { ' ' })[0];

            string qryGetDegDetails = "";

            qryGetDegDetails = " select distinct r.Batch_Year,r.degree_code,sy.semester,r.Sections,si.end_date from staff_selector ss,Registration r,";
            qryGetDegDetails = qryGetDegDetails + " subject s,sub_sem sm,syllabus_master sy,seminfo si where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code";
            qryGetDegDetails = qryGetDegDetails + " and sy.semester=r.Current_Semester and sy.syll_code=sm.syll_code and sm.subType_no=s.subType_no ";
            qryGetDegDetails = qryGetDegDetails + " and s.subject_no=ss.subject_no and isnull(r.sections,'')=isnull(ss.sections,'') and ss.batch_year=r.Batch_Year";
            qryGetDegDetails = qryGetDegDetails + " and si.Batch_Year=r.Batch_Year and si.degree_code=r.degree_code and si.semester=r.Current_Semester and ";
            qryGetDegDetails = qryGetDegDetails + " si.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and si.semester=sy.Semester and r.CC=0 and r.Exam_Flag<>'debar'";
            qryGetDegDetails = qryGetDegDetails + " and r.DelFlag=0 and ss.staff_code='" + Convert.ToString(Session["StaffCode"]) + "' union select distinct r.Batch_Year,r.degree_code,sy.semester,r.Sections,si.end_date from alternateStaffDetails asd,Registration r,sub_sem sm,syllabus_master sy,seminfo si, subject s  where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.semester=r.Current_Semester and sy.syll_code=sm.syll_code and sm.subType_no=s.subType_no  and s.subject_no=asd.subjectNo and si.Batch_Year=r.Batch_Year and si.degree_code=r.degree_code and si.semester=r.Current_Semester and  si.Batch_Year=sy.Batch_Year and sy.degree_code=r.degree_code and si.semester=sy.Semester and r.CC=0 and r.Exam_Flag<>'debar' and r.DelFlag=0 and asd.alterStaffCode='" + Convert.ToString(Session["StaffCode"]) + "'";
            DataSet dsDegreeDetails = d2.select_method_wo_parameter(qryGetDegDetails, "Text");

            // string qryAllDetails = "select * from Semester_Schedule order by FromDate desc;";
            string qryAllDetails = " select * from Semester_Schedule where (mon1 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or mon2 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or mon3 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or mon4 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or mon5 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or mon6 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or mon7 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or mon8 like '%" + Convert.ToString(Session["StaffCode"]) + "%') or (tue1 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or tue2 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or tue3 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or tue4 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or tue5 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or tue6 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or tue7 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or tue8 like '%" + Convert.ToString(Session["StaffCode"]) + "%') or (wed1 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or wed2 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or wed3 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or wed4 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or wed5 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or wed6 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or wed7 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or wed8 like '%" + Convert.ToString(Session["StaffCode"]) + "%') or (thu1 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or thu2 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or thu3 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or thu4 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or thu5 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or thu6 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or thu7 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or thu8 like '%" + Convert.ToString(Session["StaffCode"]) + "%') or (fri1 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or fri2 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or fri3 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or fri4 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or fri5 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or fri6 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or fri7 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or fri8 like '%" + Convert.ToString(Session["StaffCode"]) + "%') or (sat1 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sat2 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sat3 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sat4 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sat5 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sat6 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sat7 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sat8 like '%" + Convert.ToString(Session["StaffCode"]) + "%') or (sun1 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sun2 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sun3 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sun4 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sun5 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sun6 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sun7 like '%" + Convert.ToString(Session["StaffCode"]) + "%' or sun8 like '%" + Convert.ToString(Session["StaffCode"]) + "%') order by FromDate desc";
            // qryAllDetails = qryAllDetails + "select * from Alternate_Schedule order by FromDate desc;";

            DataSet dsAllDetails = d2.select_method_wo_parameter(qryAllDetails, "Text");
            DataView dvSemTT = new DataView();
            DataView dvAlternateSemTT = new DataView();

            if (dsDegreeDetails.Tables.Count > 0 && dsDegreeDetails.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsDegreeDetails.Tables[0].Rows.Count; i++)
                {
                    string strSec = string.Empty;
                    if (dsDegreeDetails.Tables[0].Rows[i]["sections"].ToString() != "-1" && dsDegreeDetails.Tables[0].Rows[i]["sections"].ToString() != null && dsDegreeDetails.Tables[0].Rows[i]["sections"].ToString().Trim() != "")
                    {
                        strSec = "and Sections='" + dsDegreeDetails.Tables[0].Rows[i]["sections"].ToString() + "'";
                    }

                    if (dsAllDetails.Tables.Count > 0)
                    {
                        bool checkRow = false;
                        if (dsAllDetails.Tables[0].Rows.Count > 0)
                        {
                            string strDegDetails = "";
                            dsAllDetails.Tables[0].DefaultView.RowFilter = "batch_year='" + dsDegreeDetails.Tables[0].Rows[i]["batch_year"].ToString() + "' and degree_code='" + dsDegreeDetails.Tables[0].Rows[i]["degree_code"].ToString() + "' and semester='" + dsDegreeDetails.Tables[0].Rows[i]["semester"].ToString() + "' " + strSec + " and FromDate<='" + strCurrDate.ToString() + "'";
                            dvSemTT = dsAllDetails.Tables[0].DefaultView;
                            checkRow = false;
                            if (dvSemTT.Count > 0)
                            {
                                strDegDetails = Convert.ToString(dvSemTT[0]["degree_code"]) + "," + Convert.ToString(dvSemTT[0]["semester"]) + "," + Convert.ToString(dvSemTT[0]["batch_year"]) + "," + Convert.ToString(dvSemTT[0]["ttname"]) + "," + Convert.ToString(dvSemTT[0]["fromdate"]).Split(' ')[0] + "," + Convert.ToString(dvSemTT[0]["sections"]);

                                if (checkRow == false)
                                {
                                    for (int day = 0; day < noOfDays; day++)
                                    {
                                        for (int hr = 1; hr <= noOfHrs; hr++)
                                        {
                                            string str = DaysAcronym[day].ToString() + hr;

                                            string val = Convert.ToString(dvSemTT[0][str]);
                                            if (!string.IsNullOrEmpty(val))
                                            {
                                                if (val.Contains(Convert.ToString((Session["StaffCode"]))))
                                                {
                                                    string row = "";
                                                    switch (DaysAcronym[day].ToString())
                                                    {
                                                        case "mon":
                                                            row = "0";
                                                            break;
                                                        case "tue":
                                                            row = "1";
                                                            break;
                                                        case "wed":
                                                            row = "2";
                                                            break;
                                                        case "thu":
                                                            row = "3"; break;
                                                        case "fri":
                                                            row = "4"; break;
                                                        case "sat":
                                                            row = "5"; break;
                                                        case "sun":
                                                            row = "6";
                                                            break;

                                                    }
                                                    string spreadCellValue = "";
                                                    if (val.Contains(';'))
                                                    {
                                                        string[] arr = val.Split(';');
                                                        for (int k = 0; k < arr.Length; k++)
                                                        {
                                                            if (arr[k].Contains(Convert.ToString((Session["StaffCode"]))))
                                                            {
                                                                if (spreadCellValue == "")
                                                                    //spreadCellValue = Convert.ToString(arr[k]);
                                                                    spreadCellValue = getSpreadCellValue(Convert.ToString(arr[k]), strDegDetails);
                                                                else
                                                                    spreadCellValue = spreadCellValue + ";" + getSpreadCellValue(Convert.ToString(arr[k]), strDegDetails);
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        //spreadCellValue = val;
                                                        spreadCellValue = getSpreadCellValue(val, strDegDetails);
                                                    }

                                                    if (!htData.ContainsKey(row + hr))
                                                    {
                                                        htData.Add(row + hr, spreadCellValue);
                                                    }
                                                    else
                                                    {
                                                        string oldValue = Convert.ToString(htData[row + hr]);
                                                        //if (oldValue.Split('#')[1] != spreadCellValue.Split('#')[1])
                                                        //{
                                                        spreadCellValue = spreadCellValue + ";" + oldValue;
                                                        //  }
                                                        htData.Remove(row + hr);
                                                        htData.Add(row + hr, spreadCellValue);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    checkRow = true;
                                }
                            }
                        }

                    }
                }
            }

            for (int row = 0; row < noOfDays; row++)
            {
                string r = row.ToString();
                for (int col = 1; col <= noOfHrs; col++)
                {
                    string cellValue = "";
                    string cellNoteValue = "";
                    string c = col.ToString();
                    if (htData.ContainsKey(r + c))
                    {
                        if (Convert.ToString(htData[r + c]).Contains(';'))
                        {
                            string[] arr = Convert.ToString(htData[r + c]).Split(';');
                            for (int k = 0; k < arr.Length; k++)
                            {
                                string[] val = Convert.ToString(arr[k]).Split('#');

                                if (cellValue == "")
                                {
                                    cellValue = val[0];
                                    cellNoteValue = val[1];
                                }
                                else
                                {
                                    cellValue = cellValue + ";" + val[0];
                                    cellNoteValue = cellNoteValue + ";" + val[1];
                                }
                            }
                        }
                        else
                        {
                            string[] val = Convert.ToString(htData[r + c]).Split('#');
                            cellValue = val[0];
                            cellNoteValue = val[1];
                        }

                        spreadTimeTable.Sheets[0].Cells[row, col].Text = cellValue;
                        spreadTimeTable.Sheets[0].Cells[row, col].Note = cellNoteValue;
                        spreadTimeTable.Sheets[0].Cells[row, col].HorizontalAlign = HorizontalAlign.Left;
                    }
                }
            }

        }
        catch { }
    }
    protected void spreadTimeTable_OnCellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        cellClick = true;
        lblErrorMsg.Visible = false;
    }
    protected void spreadTimeTable_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (cellClick == true)
            {
                string activerow = spreadTimeTable.ActiveSheetView.ActiveRow.ToString();
                string activecol = spreadTimeTable.ActiveSheetView.ActiveColumn.ToString();
                if (activecol != "0")
                {
                    spreadTimeTable.SaveChanges();
                    divTreeView.Visible = true;
                    tr_date.Visible = false;
                    gridSelTT.Visible = false;
                    btnAdd.Visible = false;
                    loadTree();
                    btndelete.Visible = true;
                }
                else
                {
                    spreadTimeTable.SaveChanges();
                    divTreeView.Visible = false;
                }
            }
        }
        catch { }
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            spreadTimeTable.SaveChanges();
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };

           
            for (int row = 0; row < spreadTimeTable.Sheets[0].RowCount; row++)
            {
                for (int col = 1; col < spreadTimeTable.Sheets[0].ColumnCount; col++)
                {
                    string Daycoulmn = string.Empty;
                    string Daycoulmnvalue = string.Empty;
                    string dayofweek = Days[row];
                    Daycoulmn = dayofweek + Convert.ToString(col);
                    string cellNoteVal = Convert.ToString(spreadTimeTable.Sheets[0].Cells[row, col].Note);
                    if (cellNoteVal != "")
                    {
                        if (cellNoteVal.Contains(';'))
                        {
                            string[] arrMultipleVal = cellNoteVal.Split(';');
                            for (int i = 0; i < arrMultipleVal.Length; i++)
                            {
                                string[] arrCode = arrMultipleVal[i].Split(',');
                                string batch = Convert.ToString(arrCode[3]);
                                string degcode = Convert.ToString(arrCode[1]);
                                string sem = Convert.ToString(arrCode[2]);
                                string sec = Convert.ToString(arrCode[6]);
                                string ttDate = Convert.ToString(arrCode[5]);
                                string ttName = Convert.ToString(arrCode[4]);
                                Daycoulmnvalue = Convert.ToString(arrCode[0]);

                                status = insertRecord(degcode, sem, batch, Daycoulmn, Daycoulmnvalue, ttName, ttDate, sec);
                            }
                        }
                        else
                        {
                            string[] arrCode = cellNoteVal.Split(',');
                            string batch = Convert.ToString(arrCode[3]);
                            string degcode = Convert.ToString(arrCode[1]);
                            string sem = Convert.ToString(arrCode[2]);
                            string sec = Convert.ToString(arrCode[6]);
                            string ttDate = Convert.ToString(arrCode[5]);
                            string ttName = Convert.ToString(arrCode[4]);
                            Daycoulmnvalue = Convert.ToString(arrCode[0]);
                            status = insertRecord(degcode, sem, batch, Daycoulmn, Daycoulmnvalue, ttName, ttDate, sec);
                        }
                    }
                }
            }
            if (status > 0)
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
                //lblAlertMsg.Text = "Saved Successfully";
                //divPopAlert.Visible = true;
                //btnSave.Visible = false;
                //btndelete.Visible = false;
                //return;
                btnGo_OnClick(sender, e);
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
                lblAlertMsg.Text = "Saved Successfully";
                divPopAlert.Visible = true;
                btnSave.Visible = true;
                btndelete.Visible = true;
                return;
            }
        }
        catch { }
    }
    protected void btndelete_OnClick(object sender, EventArgs e)
    {
        try
        {
            int status = 0;
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string activerow = spreadTimeTable.ActiveSheetView.ActiveRow.ToString();
            string activecol = spreadTimeTable.ActiveSheetView.ActiveColumn.ToString();
            if (activecol != "0" && activerow != "-1")
            {
                string Daycoulmn = string.Empty;

                string cellNoteVal = Convert.ToString(spreadTimeTable.Sheets[0].Cells[int.Parse(activerow), int.Parse(activecol)].Note);
                if (cellNoteVal != "")
                {
                    string dayofweek = Days[int.Parse(activerow)];
                    Daycoulmn = dayofweek + Convert.ToString(int.Parse(activecol));
                    if (cellNoteVal.Contains(';'))
                    {
                        string[] arrMultipleVal = cellNoteVal.Split(';');
                        for (int i = 0; i < arrMultipleVal.Length; i++)
                        {
                            string[] arrCode = arrMultipleVal[i].Split(',');
                            string batch = Convert.ToString(arrCode[3]);
                            string degcode = Convert.ToString(arrCode[1]);
                            string sem = Convert.ToString(arrCode[2]);
                            string sec = Convert.ToString(arrCode[6]);
                            string ttDate = Convert.ToString(arrCode[5]);
                            string ttName = Convert.ToString(arrCode[4]);


                            status = deleteRecord(degcode, sem, batch, Daycoulmn, ttName, ttDate, sec);
                        }
                    }
                    else
                    {
                        string[] arrCode = cellNoteVal.Split(',');
                        string batch = Convert.ToString(arrCode[3]);
                        string degcode = Convert.ToString(arrCode[1]);
                        string sem = Convert.ToString(arrCode[2]);
                        string sec = Convert.ToString(arrCode[6]);
                        string ttDate = Convert.ToString(arrCode[5]);
                        string ttName = Convert.ToString(arrCode[4]);

                        status = deleteRecord(degcode, sem, batch, Daycoulmn, ttName, ttDate, sec);
                    }
                }
                if (status > 0)
                {
                    spreadTimeTable.Sheets[0].Cells[int.Parse(activerow), int.Parse(activecol)].Text = string.Empty;
                    spreadTimeTable.Sheets[0].Cells[int.Parse(activerow), int.Parse(activecol)].Tag = string.Empty;
                    spreadTimeTable.Sheets[0].Cells[int.Parse(activerow), int.Parse(activecol)].Note = string.Empty;
                    spreadTimeTable.SaveChanges();
                    btnSave.Visible = true;
                    lblErrorMsg.Visible = false;
                }
            }

        }
        catch { }
    }

    private void loadTree()
    {
        try
        {
            subjtree.Nodes.Clear();

            string qrySubDetails = "select distinct (CONVERT(varchar,r.Batch_Year)+'-'+c.Course_Name+' ('+de.dept_acronym+')-'+'Sem'+CONVERT(varchar, r.Current_Semester)+' '+ISNULL(r.Sections,''))Degree ,r.Batch_Year,r.degree_code,r.Current_Semester,ISNULL(r.Sections,'')Section,(CONVERT(varchar,r.Batch_Year)+' - '+CONVERT(varchar,r.degree_code)+' - '+CONVERT(varchar, r.Current_Semester)+' - '+ISNULL(r.Sections,''))Code from Registration r,Degree d,Department de,course c where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0 and r.Batch_Year in (select distinct Batch_Year from Registration where CC=0 and DelFlag=0 and Exam_Flag<>'debar') and r.college_code='" + collegecode + "' order by r.Batch_Year desc";
            DataSet dsSubDetails = d2.select_method_wo_parameter(qrySubDetails, "Text");

            if (dsSubDetails.Tables.Count > 0 && dsSubDetails.Tables[0].Rows.Count > 0)
            {
                TreeNode node;
                int rec_count = 0;
                for (int i = 0; i < dsSubDetails.Tables[0].Rows.Count; i++)
                {
                    string code = Convert.ToString(dsSubDetails.Tables[0].Rows[i]["code"]);
                    string[] arrCode = code.Split('-');
                    string batch = Convert.ToString(arrCode[0]).Trim();
                    string degcode = Convert.ToString(arrCode[1]).Trim();
                    string sem = Convert.ToString(arrCode[2]).Trim();
                    string sec = Convert.ToString(arrCode[3]).Trim();
                    string strSec = string.Empty;
                    if (sec != "" && sec != "-1" && sec != null)
                    {
                        strSec = "and sections='" + sec + "'";
                    }
                    string Syllabus_year = string.Empty;
                    Syllabus_year = GetSyllabusYear(degcode, batch, sem);

                    if (Syllabus_year != "-1")
                    {
                        string qry = "select distinct subject.subtype_no,subject_type from subject, sub_sem where sub_sem.subtype_no=subject.subtype_no  and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + degcode + " and semester=" + sem + " and syllabus_year = " + Syllabus_year + " and batch_year = " + batch + ") order by subject.subtype_no";
                        DataSet ds = d2.select_method_wo_parameter(qry, "Text");

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            node = new TreeNode(Convert.ToString(dsSubDetails.Tables[0].Rows[i]["Degree"]), Convert.ToString(code));
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                if ((ds.Tables[0].Rows[j]["subject_type"].ToString()) != "0")
                                {
                                    TreeNode subNode;
                                    string qry1 = "";
                                    if (Convert.ToString(Session["StaffCode"]) != "")
                                    {
                                        qry1 = "select subject.subtype_no,subject_type,subject.subject_no,subject_name,subject_code from subject,sub_sem,staff_selector ss  where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + degcode + " and semester=" + sem + " and syllabus_year = " + Syllabus_year + " and batch_year = " + batch + ") and subject.subtype_no=" + Convert.ToString(ds.Tables[0].Rows[j]["subtype_no"]) + " and subject.subject_no=ss.subject_no and staff_code='" + Convert.ToString(Session["StaffCode"]) + "' " + strSec + " order by subject.subtype_no,subject.subject_no";

                                    }
                                    else
                                    {
                                        //qry1 = "select subject.subtype_no,subject_type,subject_no,subject_name,subject_code from subject,sub_sem where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + degcode + " and semester=" + sem + " and syllabus_year = " + Syllabus_year + " and batch_year = " + batch + ") and subject.subtype_no=" + Convert.ToString(ds.Tables[0].Rows[j]["subtype_no"]) + " order by subject.subtype_no,subject.subject_no";
                                        qry1 = "select subject.subtype_no,subject_type,subject.subject_no,subject_name,subject_code from subject,sub_sem,staff_selector ss  where sub_sem.subtype_no=subject.subtype_no and subject.syll_code=(select syll_code from syllabus_master where degree_code=" + degcode + " and semester=" + sem + " and syllabus_year = " + Syllabus_year + " and batch_year = " + batch + ") and subject.subtype_no=" + Convert.ToString(ds.Tables[0].Rows[j]["subtype_no"]) + " and subject.subject_no=ss.subject_no and staff_code='" + txt_scode.Text.Trim() + "' " + strSec + " order by subject.subtype_no,subject.subject_no";

                                    }

                                    DataSet ds1 = d2.select_method_wo_parameter(qry1, "Text");
                                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                                    {
                                        TreeNode subChildNode;
                                        subNode = new TreeNode(Convert.ToString(ds.Tables[0].Rows[j]["subject_type"]), Convert.ToString(rec_count));
                                        node.ChildNodes.Add(subNode);
                                        for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                                        {
                                            subChildNode = new TreeNode(Convert.ToString(ds1.Tables[0].Rows[k]["subject_name"]), Convert.ToString(ds1.Tables[0].Rows[k]["subject_no"]));
                                            subNode.ChildNodes.Add(subChildNode);
                                            rec_count = rec_count + 1;
                                        }
                                    }
                                }
                            }
                            if (node.ChildNodes.Count > 0)
                                subjtree.Nodes.Add(node);
                        }

                    }

                }
            }

        }
        catch { }
    }
    protected void subjtree_OnSelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            Session["code"] = 0;
            Session["codeName"] = "";
            int parent_count = subjtree.Nodes.Count;
            for (int i = 0; i < parent_count; i++)
            {
                for (int child1 = 0; child1 < subjtree.Nodes[i].ChildNodes.Count; child1++)
                {
                    for (int child2 = 0; child2 < subjtree.Nodes[i].ChildNodes[child1].ChildNodes.Count; child2++)
                    {
                        if (subjtree.Nodes[i].ChildNodes[child1].ChildNodes[child2].Selected == true)
                        {
                            selectedSubjectNo = subjtree.Nodes[i].ChildNodes[child1].ChildNodes[child2].Value;
                            Session["code"] = subjtree.Nodes[i].Value;
                            Session["codeName"] = subjtree.Nodes[i].Text;
                            string[] arr = Convert.ToString(Session["code"]).Split('-');
                            string selectedBatch = Convert.ToString(arr[0]).Trim();
                            string selectedDegCode = Convert.ToString(arr[1]).Trim();
                            string selectedSem = Convert.ToString(arr[2]).Trim();
                            string selectedSec = Convert.ToString(arr[3]).Trim();
                            loadExistingTT(selectedBatch, selectedDegCode, selectedSem, selectedSec);
                            bindTimeTableGrid(selectedDegCode, selectedSem);
                        }
                    }
                }
            }
            tr_date.Visible = true;
            gridSelTT.Visible = true;
            btnAdd.Visible = true;
            lblErrorMsg.Visible = false;
        }
        catch { }
    }
    protected void loadExistingTT(string batch, string degCode, string sem, string sec)
    {
        try
        {
            ddltimetable.Items.Clear();
            string section = string.Empty;
            if (sec != "" && sec != "-1" && sec.Trim().ToLower() != "all")
            {
                section = "and sections='" + sec + "'";
            }
            ds.Dispose();
            ds.Reset();
            string strtimetable = "Select DISTINCT top 1 TTname,FromDate from semester_schedule where batch_year=" + batch + " and degree_code=" + degCode + " and semester=" + sem + " " + section + " order by  FromDate desc";
            ds = d2.select_method(strtimetable, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddltimetable.DataSource = ds;
                ddltimetable.DataTextField = "TTname";
                ddltimetable.DataBind();
                bindDate(batch, degCode, sem, sec);
            }
            ddltimetable.Items.Insert(0, "");
            ddltimetable.Items.Insert(1, "New");
            if (ddltimetable.Items.Count >= 3)
            {
                ddltimetable.SelectedIndex = ddltimetable.Items.Count - 1;
                loaddetail(batch, degCode, sem, sec);
            }
            else
            {
                ddltimetable.SelectedIndex = 0;
            }
            txttimetable.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }
    protected void loaddetail(string batch, string degCode, string sem, string sec)
    {
        try
        {
            if (ddltimetable.SelectedItem.ToString() == "New")
            {
                tdTime.Visible = true;
                txttimetable.Visible = true;
                txttimetable.Text = string.Empty;
                tdTime.Attributes.Add("style", "display:block;");
            }
            else
            {
                tdTime.Attributes.Add("style", "display:none;");
                txttimetable.Visible = false;
                tdTime.Visible = false;
                if (ddltimetable.SelectedItem.ToString() != "")
                {
                    bindDate(batch, degCode, sem, sec);
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void bindDate(string batch, string degCode, string sem, string sec)
    {
        try
        {
            txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            string section = string.Empty;
            if (sec != "" && sec != "-1" && sec != "All")
            {
                section = "and sections='" + sec + "'";
            }
            string date = d2.GetFunction("Select convert(nvarchar(15),Fromdate,103) as date from semester_schedule where batch_year=" + batch + " and degree_code=" + degCode + " and semester=" + sem + " " + section + " and ttname='" + Convert.ToString(ddltimetable.SelectedItem) + "' ");
            if (date != "" && date != null && date != "0" && ddltimetable.Enabled == true)
            {
                txtdate.Text = date;//StartDate 
                DateTime dt1 = new DateTime();// Convert.ToDateTime(datefrom.ToString());
                bool isValidDate = DateTime.TryParseExact(date, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dt1);
                if(isValidDate)
                    CalToDate.StartDate = dt1;
            }
        }
        catch (Exception ex)
        {

        }
    }
    private void bindTimeTableGrid(string degCode, string sem)
    {
        try
        {
            string dayvalue = string.Empty;
            int dayorder = 0;
            int hourPerDay = 0;
            int noofdays = 0;
            string holiday = string.Empty;

            DataTable dtTTSel = new DataTable();

            string day = string.Empty;
            DataSet dsDay = new DataSet();
            int date = 0;

            string strpriodquery = "Select No_of_hrs_per_day,schorder,nodays,holiday from PeriodAttndSchedule where degree_code = '" + degCode + "' and semester = " + sem + "";
            dsDay = d2.select_method(strpriodquery, hat, "Text");
            if (dsDay.Tables.Count > 0 && dsDay.Tables[0].Rows.Count > 0)
            {
                // dayorder = Convert.ToInt32(dsDay.Tables[0].Rows[0]["schorder"]);
                hourPerDay = Convert.ToInt32(dsDay.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                noofdays = Convert.ToInt32(dsDay.Tables[0].Rows[0]["nodays"]);
                holiday = Convert.ToString(dsDay.Tables[0].Rows[0]["holiday"]);
                // Session["dayorder"] = Convert.ToString(dayorder);
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Check Sem Info";
                return;
            }

            string SchOrder = d2.GetFunction("select distinct top 1 schOrder from PeriodAttndSchedule");
            dayorder = Convert.ToInt32(SchOrder);

            if (dayorder == 1)
            {
                dtTTSel.Columns.Add("Day");
                dtTTSel.Columns.Add("DayVal");
                dtTTSel.Columns.Add("H1");
                dtTTSel.Columns.Add("H2");
                dtTTSel.Columns.Add("H3");
                dtTTSel.Columns.Add("H4");
                dtTTSel.Columns.Add("H5");
                dtTTSel.Columns.Add("H6");
                dtTTSel.Columns.Add("H7");
                dtTTSel.Columns.Add("H8");
                dtTTSel.Columns.Add("H9");
                dtTTSel.Columns.Add("H10");

                for (int i = 1; i <= noofdays; i++)
                {
                    switch (i)
                    {
                        case 1:
                            day = "Monday";
                            break;
                        case 2:
                            day = "Tuesday";
                            break;
                        case 3:
                            day = "Wednesday";
                            break;
                        case 4:
                            day = "Thursday";
                            break;
                        case 5:
                            day = "Friday";
                            break;
                        case 6:
                            day = "Saturday";
                            break;
                        case 7:
                            day = "Sunday";
                            break;
                    }
                    DataRow dr = dtTTSel.NewRow();
                    dr["Day"] = day;
                    dr["DayVal"] = i;
                    dtTTSel.Rows.Add(dr);
                }

                gridSelTT.DataSource = dtTTSel;
                gridSelTT.DataBind();
            }
            else
            {
                dtTTSel.Columns.Add("Day");
                dtTTSel.Columns.Add("DayVal");

                for (int day1 = 0; day1 < noofdays; day1++)
                {
                    DataRow dr = dtTTSel.NewRow();

                    int daysetweek = day1 + 2;

                    if (day1 == noofdays)
                    {
                        daysetweek = 1;
                    }
                    if (!holiday.Contains(daysetweek.ToString()))
                    {
                        if (dayorder == 1)
                        {
                        }
                        else
                        {
                            date = day1 + 1;
                            dr["Day"] = "Day" + " " + date;
                            dr["DayVal"] = date;
                            dtTTSel.Rows.Add(dr);
                        }
                    }
                }
                gridSelTT.DataSource = dtTTSel;
                gridSelTT.DataBind();
            }
            for (int cell = 1; cell <= 10; cell++)
            {
                if (cell > hourPerDay)
                {
                    gridSelTT.Columns[cell].Visible = false;
                }
                else
                {
                    gridSelTT.Columns[cell].Visible = true;
                }
            }

        }
        catch
        { }
    }
    protected void btnAdd_OnClick(object sender, EventArgs e)
    {
        try
        {
            spreadTimeTable.SaveChanges();
            string splval = string.Empty;
            string subno_staff = string.Empty;
            string subno_staffnote = string.Empty;
            string activerow = spreadTimeTable.ActiveSheetView.ActiveRow.ToString();
            string activecol = spreadTimeTable.ActiveSheetView.ActiveColumn.ToString();
            string[] date = txtdate.Text.Split('/');
            string fromdate = date[1] + '/' + date[0] + '/' + date[2];
            string staffName = "";
            string staffCode = "";
            string qry = "";
            string tablevalue = string.Empty;
            string ttName = "";
            Hashtable hatdegree = new Hashtable();
            string history_data = string.Empty;
            string[] arr = Convert.ToString(Session["code"]).Split('-');
            string selectedBatch = Convert.ToString(arr[0]).Trim();
            string selectedDegCode = Convert.ToString(arr[1]).Trim();
            string selectedSem = Convert.ToString(arr[2]).Trim();
            string selectedSec = Convert.ToString(arr[3]).Trim();

            if (ddlSearchOption.SelectedIndex == 0)
            {
                staffCode = Convert.ToString(txt_scode.Text).Trim();
                staffName = d2.GetFunction("select staff_name from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_code like '%" + staffCode + "%' and college_code='" + collegecode + "'");
            }
            else if (ddlSearchOption.SelectedIndex == 1)
            {
                staffName = Convert.ToString(txt_sname.Text).Trim();
                staffCode = d2.GetFunction("select staff_code from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like '%" + staffName + "%' and college_code='" + collegecode + "'");
            }

            if (ddltimetable.SelectedItem.ToString() == "New")
            {
                ttName = txttimetable.Text.Trim();
            }
            else
            {
                ttName = ddltimetable.SelectedItem.ToString();
            }

            string strDegreeDetails = selectedDegCode + "," + selectedSem + "," + selectedBatch + "," + ttName + "," + fromdate + "," + selectedSec;

            if (ttName != "")
            {
                string textValue = Convert.ToString(Session["codeName"]);
                if (splval == "")
                {
                    string val = "S";
                    string subj_type = d2.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + Convert.ToString(selectedSubjectNo) + "'");
                    if (subj_type == "1" || subj_type.ToLower().Trim() == "true")
                    {
                        val = "L";
                    }

                    splval = (d2.GetFunction("select subject_name from subject where subject_no=" + Convert.ToString(selectedSubjectNo) + " ") + "-" + staffCode + "-" + "" + val + "");
                    subno_staffnote = Convert.ToString(selectedSubjectNo) + "-" + staffCode + "-" + val + "," + strDegreeDetails;
                    subno_staff = (d2.GetFunction("select subject_name from subject where subject_no=" + Convert.ToString(selectedSubjectNo) + " ") + "-" + val + "-" + textValue);
                }
                else
                {
                    string val = "S";
                    string subj_type = d2.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + Convert.ToString(selectedSubjectNo) + "'");
                    if (subj_type == "1" || subj_type.Trim().ToLower() == "true")
                    {
                        val = "L";
                    }

                    splval = splval + ";" + (d2.GetFunction("select subject_name from subject where subject_no=" + Convert.ToString(selectedSubjectNo) + " ") + "-" + staffCode + "-" + "" + val + "");
                    subno_staffnote = subno_staffnote + ";" + Convert.ToString(selectedSubjectNo) + "-" + staffCode + "-" + val + "," + strDegreeDetails;
                    subno_staff = subno_staff + ";" + (d2.GetFunction("select subject_name from subject where subject_no=" + Convert.ToString(selectedSubjectNo) + " ") + "-" + val + "-" + "" + textValue + "");
                }
                int x = spreadTimeTable.ActiveSheetView.ActiveRow;
                int y = spreadTimeTable.ActiveSheetView.ActiveColumn;
                if (y > 0)
                {
                    int noOfHrs = 0;
                    int.TryParse(d2.GetFunction("select max(No_of_hrs_per_day) from PeriodAttndSchedule"), out noOfHrs);
                    if (gridSelTT.Visible)
                    {
                        for (int rowI = 0; rowI < gridSelTT.Rows.Count; rowI++)
                        {
                            for (int colI = 1; colI <= noOfHrs; colI++)
                            {
                                DropDownList ddlVal = (DropDownList)gridSelTT.Rows[rowI].FindControl("ddlH" + colI);
                                if (ddlVal.SelectedIndex == 1)
                                {
                                    if (spreadTimeTable.Sheets[0].RowCount > rowI)
                                    {
                                        string cellValue = Convert.ToString(spreadTimeTable.Sheets[0].Cells[rowI, colI].Text);
                                        string cellNote = Convert.ToString(spreadTimeTable.Sheets[0].Cells[rowI, colI].Note);

                                        if (cellValue == "")
                                        {
                                            spreadTimeTable.Sheets[0].Cells[rowI, colI].Text = subno_staff.ToString();
                                            spreadTimeTable.Sheets[0].Cells[rowI, colI].Note = subno_staffnote.ToString();
                                        }
                                        else
                                        {
                                            if (!isChanged)
                                            {
                                                alert2PopUp.Show();
                                                goto golabel;
                                            }
                                        golabel1: ;
                                            if (replace)
                                            {
                                                spreadTimeTable.Sheets[0].Cells[rowI, colI].Text = subno_staff.ToString();
                                                spreadTimeTable.Sheets[0].Cells[rowI, colI].Note = subno_staffnote.ToString();
                                            }
                                            else
                                            {
                                                spreadTimeTable.Sheets[0].Cells[rowI, colI].Text = cellValue + ";" + subno_staff.ToString();
                                                spreadTimeTable.Sheets[0].Cells[rowI, colI].Note = cellNote + ";" + subno_staffnote.ToString();
                                            }

                                        }

                                        FarPoint.Web.Spread.SheetView sv = spreadTimeTable.ActiveSheetView;
                                        sv.ActiveColumn = y;
                                        sv.ActiveRow = x;
                                    }
                                }


                            }
                        }
                    }
                    else
                    {
                        string cellValue = Convert.ToString(spreadTimeTable.Sheets[0].Cells[x, y].Text);
                        string cellNote = Convert.ToString(spreadTimeTable.Sheets[0].Cells[x, y].Note);
                        if (cellValue == "")
                        {
                            spreadTimeTable.Sheets[0].Cells[x, y].Text = subno_staff.ToString();
                            spreadTimeTable.Sheets[0].Cells[x, y].Note = subno_staffnote.ToString();
                        }
                        else
                        {
                            if (!isChanged)
                            {
                                alert2PopUp.Show();
                                goto golabel;
                            }

                            if (replace)
                            {
                                spreadTimeTable.Sheets[0].Cells[x, y].Text = subno_staff.ToString();
                                spreadTimeTable.Sheets[0].Cells[x, y].Note = subno_staffnote.ToString();
                            }
                            else
                            {
                                spreadTimeTable.Sheets[0].Cells[x, y].Text = cellValue + ";" + subno_staff.ToString();
                                spreadTimeTable.Sheets[0].Cells[x, y].Note = cellNote + ";" + subno_staffnote.ToString();
                            }

                        }
                        FarPoint.Web.Spread.SheetView sv = spreadTimeTable.ActiveSheetView;
                        sv.ActiveColumn = y;
                        sv.ActiveRow = x;
                    }
                }


            golabel: ;

                divTreeView.Visible = false;
                btnSave.Visible = true;
                btndelete.Visible = true;
                lblErrorMsg.Visible = false;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Time Table Name')", true);
                return;
            }
        }
        catch { }
    }

    protected void btnUpdate_OnClick(object sender, EventArgs e)
    {
        alertPopUp.Hide();
        try
        {
            replace = false;
            allowCombineClass = true;
            btnAdd_OnClick(sender, e);
        }
        catch
        {
        }
    }
    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        try
        {
            alertPopUp.Hide();
            return;
        }
        catch
        {
        }
    }

    protected void btnReplace_OnClick(object sender, EventArgs e)
    {
        alert2PopUp.Hide();
        //  alertPopUp.Hide();
        try
        {
            //if (allowCombineClass == false)
            //    allowCombineClass = true;
            isChanged = true;
            replace = true;
            // allowCombineClass = false;
            btnAdd_OnClick(sender, e);
            //goto golabel1;
        }
        catch { }
    }
    protected void btnCombine_OnClick(object sender, EventArgs e)
    {
        alert2PopUp.Hide();
        try
        {

            //  allowCombineClass = true;
            isChanged = true;
            replace = false;
            btnAdd_OnClick(sender, e);
        }
        catch { }
    }
    protected void btnCancel2_OnClick(object sender, EventArgs e)
    {
        try
        {
            alert2PopUp.Hide();
            return;
        }
        catch { }
    }

    protected void checkUser()
    {
        try
        {
            ds.Clear();
            string staffCode = Session["StaffCode"].ToString();
            string staffName = d2.GetFunction("select staff_name from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_code like '%" + staffCode + "%' and college_code='" + collegecode + "'");
            if (staffCode != "")
            {
                string qry = "select * from stafftrans where staff_code='" + staffCode + "' and latestrec=1";
                ds = d2.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    txt_dept.Text = cbl_dept.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["dept_code"])).ToString();
                    txtDesig.Text = cblDesig.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["desig_code"])).ToString();
                    txtStfCategry.Text = cblStfCategry.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["category_code"])).ToString();
                    txt_scode.Text = staffCode;
                    txt_sname.Text = staffName;
                    //  ddlcollege.Items.FindByValue("13");
                    ddlcollege.Enabled = false;
                    txt_dept.Enabled = false;
                    txtDesig.Enabled = false;
                    txtStfCategry.Enabled = false;
                    txt_scode.Enabled = false;
                    txt_sname.Enabled = false;
                }
            }

        }
        catch { }
    }
    private string GetSyllabusYear(string degree_code, string batch_year, string sem)
    {
        try
        {
            string syl_year = string.Empty;
            con2a.Close();
            con2a.Open();
            SqlCommand cmd2a;
            SqlDataReader get_syl_year;
            cmd2a = new SqlCommand("select syllabus_year from syllabus_master where degree_code=" + degree_code + " and semester =" + sem + " and batch_year=" + batch_year + " ", con2a);
            get_syl_year = cmd2a.ExecuteReader();
            get_syl_year.Read();
            if (get_syl_year.HasRows == true)
            {
                if (get_syl_year[0].ToString() == "\0")
                {
                    syl_year = "-1";
                }
                else
                {
                    syl_year = get_syl_year[0].ToString();
                }
            }
            else
            {
                syl_year = "-1";
            }
            return syl_year;

        }
        catch
        {
            return string.Empty;
        }
    }

    protected string getSpreadCellValue(string strScheduledHour, string strSemSchedule)
    {
        try
        {
            string strSubName = "";
            string textValue = "";
            string noteValue = "";
            string subjectNo = strScheduledHour.Split('-')[0];
            string[] arr = strSemSchedule.Split(',');

            string sec = Convert.ToString(arr[5]).Trim();
            string strsec = "";

            if (sec != "" && sec != "-1" && sec != "all" && sec != null)
            {

                strsec = "and r.sections='" + sec + "'";
            }

            string subType = "S";
            string subj_type = d2.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + Convert.ToString(subjectNo) + "'");
            if (subj_type == "1" || subj_type.ToLower().Trim() == "true")
            {
                subType = "L";
            }

            string qry = "select distinct (CONVERT(varchar,r.Batch_Year)+'-'+c.Course_Name+' ('+de.dept_acronym+')-'+'Sem'+CONVERT(varchar, r.Current_Semester)+' '+ISNULL(r.Sections,''))Degree  from Registration r,Degree d,Department de,course c where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0  and r.degree_code='" + Convert.ToString(arr[0]).Trim() + "' and r.Batch_Year='" + Convert.ToString(arr[2]).Trim() + "' and r.Current_Semester='" + Convert.ToString(arr[1]).Trim() + "'" + strsec + " and r.college_code='" + Convert.ToString(collegecode).Trim() + "'";

            textValue = d2.GetFunction(qry);

            strSubName = (d2.GetFunction("select subject_name from subject where subject_no=" + Convert.ToString(subjectNo) + " "));
            noteValue = Convert.ToString(strScheduledHour) + "," + strSemSchedule;

            return strSubName + "-" + subType + "-" + textValue + "#" + noteValue;
        }
        catch
        {
            return null;
        }
    }

    protected int insertRecord(string degCode, string sem, string batch, string colName, string colVal, string ttName, string ttDate, string sec)
    {
        try
        {
            int status = 0;
            string columnValue = "";

            string existingColValue = d2.GetFunction("select " + colName + " from Semester_Schedule where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and Sections='" + Convert.ToString(sec) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "'");

            if (existingColValue != "" && existingColValue != "0" && existingColValue != null)
            {
                if (existingColValue.Contains(Session["StaffCode"].ToString()))
                {
                    if (existingColValue.Contains(';'))
                    {
                        string temp = "";
                        string[] arrVal = existingColValue.Split(';');
                        for (int i = 0; i < arrVal.Length; i++)
                        {
                            string val = arrVal[i];
                            if (val.Contains(Session["StaffCode"].ToString()))
                            {
                                if (temp == "")
                                    temp = colVal;
                                else
                                    temp = temp + ";" + colVal;
                            }
                            else
                            {
                                if (temp == "")
                                    temp = val;
                                else
                                    temp = temp + ";" + val;
                            }
                        }
                        columnValue = temp;
                    }
                    else
                    {
                        columnValue = colVal;
                    }
                }
                else
                {
                    columnValue = existingColValue + ";" + colVal;
                }
            }
            else
            {
                columnValue = colVal;
            }
            string insertQuery = "";

            if (sec != "")
            {
                insertQuery = " if exists(select * from Semester_Schedule where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and Sections='" + Convert.ToString(sec) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "'  ) update Semester_Schedule set " + colName + "='" + columnValue + "' where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and Sections='" + Convert.ToString(sec) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "' else  insert into Semester_Schedule (degree_code,batch_year,semester,sections,TTName,FromDate," + colName + ",lastrec) values(" + Convert.ToString(degCode) + "," + Convert.ToString(batch) + "," + Convert.ToString(sem) + ",'" + Convert.ToString(sec) + "','" + ttName + "','" + ttDate + "','" + columnValue + "',1)";
            }
            else
            {
                insertQuery = " if exists(select * from Semester_Schedule where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "'  ) update Semester_Schedule set " + colName + "='" + columnValue + "' where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "' else  insert into Semester_Schedule (degree_code,batch_year,semester,TTName,FromDate," + colName + ",lastrec) values(" + Convert.ToString(degCode) + "," + Convert.ToString(batch) + "," + Convert.ToString(sem) + ",'" + ttName + "','" + ttDate + "','" + columnValue + "',1)";
            }
            status = d2.update_method_wo_parameter(insertQuery, "Text");

            return status;
        }
        catch
        {
            return 0;
        }
    }

    protected int deleteRecord(string degCode, string sem, string batch, string colName, string ttName, string ttDate, string sec)
    {
        try
        {
            int status = 0;
            string columnValue = "";

            string existingColValue = d2.GetFunction("select " + colName + " from Semester_Schedule where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and Sections='" + Convert.ToString(sec) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "'");

            if (existingColValue != "" && existingColValue != "0" && existingColValue != null)
            {
                if (existingColValue.Contains(Session["StaffCode"].ToString()))
                {
                    if (existingColValue.Contains(';'))
                    {
                        string temp = "";
                        string[] arrVal = existingColValue.Split(';');
                        for (int i = 0; i < arrVal.Length; i++)
                        {
                            string val = arrVal[i];
                            if (!val.Contains(Session["StaffCode"].ToString()))
                            {
                                if (temp == "")
                                    temp = val;
                                else
                                    temp = temp + ";" + val;
                            }
                        }
                        columnValue = temp;
                    }
                    else
                    {
                        columnValue = null;
                    }
                }
                else
                {
                    columnValue = existingColValue;
                }
            }
            string query = "";
            if (sec != "")
            {
                query = " if exists(select * from Semester_Schedule where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and Sections='" + Convert.ToString(sec) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "'  ) update Semester_Schedule set " + colName + "='" + columnValue + "' where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and Sections='" + Convert.ToString(sec) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "'";
            }
            else
            {
                query = " if exists(select * from Semester_Schedule where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "'  ) update Semester_Schedule set " + colName + "='" + columnValue + "' where degree_code='" + Convert.ToString(degCode) + "' and batch_year='" + Convert.ToString(batch) + "' and semester='" + Convert.ToString(sem) + "' and TTName='" + ttName + "' and FromDate='" + ttDate + "'";
            }
            status = d2.update_method_wo_parameter(query, "Text");

            return status;
        }
        catch { return 0; }
    }

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
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
            string name = "";
            cb.Checked = false;
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
            else
            {
                txt.Text = "--Select--";
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

    #endregion

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value.ToString();
            }
        }
        return null;
    }

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

    protected void txtdate_TextChanged(object sender, EventArgs e)
    {
        
    }

}