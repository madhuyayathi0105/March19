using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Text;
using InsproDataAccess;
using System.Configuration;

public partial class Staff_Time_Table : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    InsproDirectAccess dir = new InsproDirectAccess();
    string collegecode = string.Empty;
    static string clgcode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string strstaffcode = string.Empty;
    Hashtable hat = new Hashtable();
    Dictionary<string, string> dicDbCol = new Dictionary<string, string>();
    Dictionary<string, string> dicDays = new Dictionary<string, string>();
    Dictionary<string, string> class_tt_dic = new Dictionary<string, string>();
    Dictionary<string, string> class_tt_det_dic = new Dictionary<string, string>();
    Dictionary<string, string> multiple_dic = new Dictionary<string, string>();
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
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            strstaffcode = Session["Staff_Code"].ToString();

            if (!IsPostBack)
            {
                bindcollege();
                collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
                clgcode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
                binddept();
                designation();
                stafftype();
                bindStaff();
                tdStfCode.Visible = true;
                tdStfName.Visible = false;
                tdStfCodeAuto.Visible = true;
                tdStfNameAuto.Visible = false;
                txtFrmDt.Attributes.Add("readonly", "readonly");
                txtToDt.Attributes.Add("readonly", "readonly");
                if (!String.IsNullOrEmpty(strstaffcode) && strstaffcode.Trim() != "0")
                {
                    ddlcollege.Enabled = false;
                    txt_dept.Enabled = false;
                    txtDesig.Enabled = false;
                    txtStfType.Enabled = false;
                    ddlStfName.Enabled = false;
                    ddlSearchOption.Enabled = false;
                    txt_scode.Enabled = false;
                    txt_sname.Enabled = false;
                }

            }
            collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
            clgcode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
            lblMainErr.Visible = false;
        }
        catch (Exception ex) { }
    }
    Hashtable htData = new Hashtable();
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_code like '" + prefixText + "%' and college_code='" + clgcode + "'";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> stfName = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like '" + prefixText + "%' and college_code='" + clgcode + "'";
        stfName = ws.Getname(query);
        return stfName;
    }

    private void bindcollege()
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

    private void binddept()
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
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = "Department (" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
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

    private void designation()
    {
        try
        {
            ds.Clear();
            cblDesig.Items.Clear();
            txtDesig.Text = "--Select--";
            cbDesig.Checked = false;
            string statequery = "select desig_code,desig_name from desig_master where collegeCode='" + collegecode + "' order by desig_name";
            ds = d2.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblDesig.DataSource = ds;
                cblDesig.DataTextField = "desig_name";
                cblDesig.DataValueField = "desig_code";
                cblDesig.DataBind();
                if (cblDesig.Items.Count > 0)
                {
                    for (int i = 0; i < cblDesig.Items.Count; i++)
                    {
                        cblDesig.Items[i].Selected = true;
                    }
                    txtDesig.Text = "Designation (" + cblDesig.Items.Count + ")";
                    cbDesig.Checked = true;
                }
            }
        }
        catch { }
    }

    private void stafftype()
    {
        try
        {
            ds.Clear();
            cblStfType.Items.Clear();
            txtStfType.Text = "--Select--";
            cbStfType.Checked = false;
            string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + collegecode + "' order by stftype";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblStfType.DataSource = ds;
                cblStfType.DataTextField = "stftype";
                cblStfType.DataBind();
                if (cblStfType.Items.Count > 0)
                {
                    for (int i = 0; i < cblStfType.Items.Count; i++)
                    {
                        cblStfType.Items[i].Selected = true;
                    }
                    txtStfType.Text = "StaffType (" + cblStfType.Items.Count + ")";
                    cbStfType.Checked = true;
                }
            }
        }
        catch { }
    }

    private void bindStaff()
    {
        try
        {
            ds.Clear();
            ddlStfName.Items.Clear();
            string SelQ = "select sm.staff_code,(sm.staff_code+' - '+sm.staff_name) as Staff_Name from staffmaster sm,stafftrans st,staff_appl_master sa where sm.staff_code=st.staff_code and sm.appl_no=sa.appl_no and sm.resign='0' and sm.settled='0' and ISNULL(sm.Discontinue,'0')='0' and st.latestrec='1' and sm.college_code='" + collegecode + "' order by len(sm.staff_code),sm.staff_Code";
            ds = d2.select_method_wo_parameter(SelQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlStfName.DataSource = ds;
                ddlStfName.DataTextField = "Staff_Name";
                ddlStfName.DataValueField = "staff_code";
                ddlStfName.DataBind();
                ddlStfName.Items.Insert(0, "Select");
            }
            else
            {
                ddlStfName.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    protected void ddlcollege_change(object sender, EventArgs e)
    {
        collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
        clgcode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
        binddept();
        designation();
        stafftype();
        bindStaff();
        grdStf_TT.Visible = false;
        tdStfCode.Visible = true;
        tdStfName.Visible = false;
        tdStfCodeAuto.Visible = true;
        tdStfNameAuto.Visible = false;
        btnComPrint.Visible = false;
        txt_scode.Text = "";
        txt_sname.Text = "";
    }

    protected void cb_dept_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_dept, cbl_dept, txt_dept, "Department");
        binddesig();
        stafftype1();
        staffcategory();
        grdStf_TT.Visible = false;
        btnComPrint.Visible = false;
    }

    protected void cbl_dept_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_dept, cbl_dept, txt_dept, "Department");
        binddesig();
        stafftype1();
        staffcategory();
        grdStf_TT.Visible = false;
        btnComPrint.Visible = false;
    }

    protected void cbDesig_CheckedChange(object sender, EventArgs e)
    {
        grdStf_TT.Visible = false;
        btnComPrint.Visible = false;
        chkchange(cbDesig, cblDesig, txtDesig, "Designation");
        stafftype1();
        staffcategory();
    }

    protected void cblDesig_SelectedIndexChange(object sender, EventArgs e)
    {
        grdStf_TT.Visible = false;
        btnComPrint.Visible = false;
        chklstchange(cbDesig, cblDesig, txtDesig, "Designation");
        stafftype1();
        staffcategory();
    }

    protected void cbStfType_CheckedChange(object sender, EventArgs e)
    {
        grdStf_TT.Visible = false;
        btnComPrint.Visible = false;
        chkchange(cbStfType, cblStfType, txtStfType, "StaffType");
        staffcategory();
    }

    protected void cblStfType_SelectedIndexChange(object sender, EventArgs e)
    {
        grdStf_TT.Visible = false;
        btnComPrint.Visible = false;
        chklstchange(cbStfType, cblStfType, txtStfType, "StaffType");
        staffcategory();
    }

    protected void txt_scode_Change(object sender, EventArgs e)
    {
        grdStf_TT.Visible = false;
        btnComPrint.Visible = false;
        txt_sname.Text = string.Empty;
        ddlStfName.ClearSelection();
    }

    protected void txt_sname_Change(object sender, EventArgs e)
    {
        grdStf_TT.Visible = false;
        btnComPrint.Visible = false;
        txt_scode.Text = string.Empty;
        ddlStfName.ClearSelection();
    }

    protected void radSemWise_Change(object sender, EventArgs e)
    {
        grdStf_TT.Visible = false;
        btnComPrint.Visible = false;
        tdlbFrm.Visible = false;
        txtFrmDt.Visible = false;
        lblToDt.Visible = false;
        txtToDt.Visible = false;
    }

    protected void radDayWise_Change(object sender, EventArgs e)
    {
        grdStf_TT.Visible = false;
        btnComPrint.Visible = false;
        tdlbFrm.Visible = true;
        txtFrmDt.Visible = true;
        lblToDt.Visible = true;
        txtToDt.Visible = true;
        txtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }

    protected void ddlSearchOption_Change(object sender, EventArgs e)
    {
        grdStf_TT.Visible = false;
        btnComPrint.Visible = false;
        if (ddlSearchOption.SelectedIndex == 0)
        {
            tdStfCode.Visible = true;
            tdStfName.Visible = false;
            tdStfCodeAuto.Visible = true;
            tdStfNameAuto.Visible = false;
            string stfcode = Convert.ToString(Session["Staff_Code"]);
            if (Convert.ToString(Session["Staff_Code"]) == "")
            {
                if (ddlStfName.SelectedItem.Text.ToLower().Trim() != "select")
                {
                    if (!string.IsNullOrEmpty(ddlStfName.SelectedValue))
                    {
                        stfcode = ddlStfName.SelectedValue;
                        Session["StaffCode"] = stfcode;
                        if (ddlSearchOption.SelectedValue == "0")
                        {
                            txt_scode.Text = stfcode;
                        }
                        else
                        {
                            string staffName = ddlStfName.SelectedItem.Text;
                            string[] splstffName = staffName.Split('-');
                            txt_sname.Text = splstffName[1].Trim();
                        }
                    }
                }
            }
        }
        else
        {
            tdStfCode.Visible = false;
            tdStfName.Visible = true;
            tdStfCodeAuto.Visible = false;
            tdStfNameAuto.Visible = true;
            string stfcode = Convert.ToString(Session["Staff_Code"]);
            if (Convert.ToString(Session["Staff_Code"]) == "")
            {
                if (ddlStfName.SelectedItem.Text.ToLower().Trim() != "select")
                {
                    if (!string.IsNullOrEmpty(ddlStfName.SelectedValue))
                    {
                        stfcode = ddlStfName.SelectedValue;
                        Session["StaffCode"] = stfcode;
                        if (ddlSearchOption.SelectedValue == "0")
                        {
                            txt_scode.Text = stfcode;
                        }
                        else
                        {
                            string staffName = ddlStfName.SelectedItem.Text;
                            string[] splstffName = staffName.Split('-');
                            txt_sname.Text = splstffName[1].Trim();
                        }
                    }
                }
            }
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            string staffCode = string.Empty;
            DataTable dtTTDisp = new DataTable();
            dtTTDisp.Columns.Add("DateDisp");
            dtTTDisp.Columns.Add("DateVal");
            dtTTDisp.Columns.Add("P1Val");
            dtTTDisp.Columns.Add("TT_1");
            dtTTDisp.Columns.Add("P2Val");
            dtTTDisp.Columns.Add("TT_2");
            dtTTDisp.Columns.Add("P3Val");
            dtTTDisp.Columns.Add("TT_3");
            dtTTDisp.Columns.Add("P4Val");
            dtTTDisp.Columns.Add("TT_4");
            dtTTDisp.Columns.Add("P5Val");
            dtTTDisp.Columns.Add("TT_5");
            dtTTDisp.Columns.Add("P6Val");
            dtTTDisp.Columns.Add("TT_6");
            dtTTDisp.Columns.Add("P7Val");
            dtTTDisp.Columns.Add("TT_7");
            dtTTDisp.Columns.Add("P8Val");
            dtTTDisp.Columns.Add("TT_8");
            dtTTDisp.Columns.Add("P9Val");
            dtTTDisp.Columns.Add("TT_9");
            dtTTDisp.Columns.Add("P10Val");
            dtTTDisp.Columns.Add("TT_10");
            grdStf_TT.Visible = false;
            DataRow drNew = null;
            string stfcode = Convert.ToString(Session["Staff_Code"]);
            if (Convert.ToString(Session["Staff_Code"]) == "")
            {
                if (ddlStfName.SelectedItem.Text.ToLower().Trim() != "select")
                {
                    if (!string.IsNullOrEmpty(ddlStfName.SelectedValue))
                    {
                        staffCode = ddlStfName.SelectedValue;
                        Session["StaffCode"] = staffCode;
                        if (ddlSearchOption.SelectedValue == "0")
                        {
                            txt_scode.Text = staffCode;
                        }
                        else
                        {
                            string staffName = ddlStfName.SelectedItem.Text;
                            string[] splstffName = staffName.Split('-');
                            txt_sname.Text = splstffName[1].Trim();
                        }
                    }
                }
                else if (Convert.ToString(ddlSearchOption.SelectedValue).Trim() != "")
                {
                    Session["StaffCode"] = Convert.ToString(txt_scode.Text).Trim();
                }
                else
                {
                    string staff_Name = Convert.ToString(Convert.ToString(ddlSearchOption.SelectedValue)).Trim();
                    if (staff_Name != "")
                    {
                        string staff_Code = d2.GetFunction("select staff_code from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like '%" + staff_Name + "%' and college_code='" + collegecode + "'");
                        Session["StaffCode"] = staff_Code.Trim();
                        ddlSearchOption.SelectedValue = staff_Code.Trim();
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
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Branch!";
                return;
            }

            string SchOrder = d2.GetFunction(" select schOrder from PeriodAttndSchedule order by semester,schOrder desc");



            DateTime cur_date = DateTime.Now;
            string strCurrDate = Convert.ToString(cur_date).Split(new Char[] { ' ' })[0];
            DataSet dsAllDetails = new DataSet();
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
            dsAllDetails = d2.select_method_wo_parameter(qryAllDetails, "Text");
            DataView dvSemTT = new DataView();
            DataView dvAlternateSemTT = new DataView();
            Hashtable hat = new Hashtable();
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
                            if (!hat.ContainsKey((dsDegreeDetails.Tables[0].Rows[i]["batch_year"].ToString() + "-" + dsDegreeDetails.Tables[0].Rows[i]["degree_code"].ToString() + "-" + dsDegreeDetails.Tables[0].Rows[i]["semester"].ToString() + "-" + strSec)))
                            {
                                hat.Add(dsDegreeDetails.Tables[0].Rows[i]["batch_year"].ToString() + "-" + dsDegreeDetails.Tables[0].Rows[i]["degree_code"].ToString() + "-" + dsDegreeDetails.Tables[0].Rows[i]["semester"].ToString() + "-" + strSec, dsDegreeDetails.Tables[0].Rows[i]["degree_code"].ToString());

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
                                                            spreadCellValue = spreadCellValue + ";" + oldValue;
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
            }

            for (int row = 0; row < noOfDays; row++)
            {
                drNew = dtTTDisp.NewRow();
                string r = row.ToString();
                string dayName = DaysName[row];
                string dayAcronym = DaysAcronym[row];

                if (SchOrder == "1")
                {
                    drNew["DateDisp"] = dayName;
                    drNew["DateVal"] = dayAcronym;
                }
                else
                {
                    int dayNo = row + 1;
                    drNew["DateDisp"] = "Day " + dayNo;
                    drNew["DateVal"] = dayNo;
                }

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
                                    if (!string.IsNullOrEmpty(cellValue))
                                        cellValue = cellValue + ";" + "<br/><br/>" + val[0];
                                    else
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


                        string lbl1 = "P" + col + "Val";
                        string lbl2 = "TT_" + col;

                        drNew[lbl1] = cellValue;
                        drNew[lbl2] = cellNoteValue;

                    }

                }
                dtTTDisp.Rows.Add(drNew);
            }
            if (dtTTDisp.Rows.Count > 0)
            {
                grdStf_TT.DataSource = dtTTDisp;
                grdStf_TT.DataBind();
                grdStf_TT.Visible = true;
                btnComPrint.Visible = true;
            }
            if (noOfHrs != 0)
            {
                for (int i = 1; i <= noOfHrs; i++)
                {

                    grdStf_TT.Columns[i].Visible = true;
                }

            }
        }
        catch
        {
        }
    }
    protected void lnkAttMark(object sender, EventArgs e)
    {
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + " (" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }

    protected void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                    count = count + 1;
            }
            if (count > 0)
            {
                txtchange.Text = label + " (" + count + ")";
                if (count == chklstchange.Items.Count)
                    chkchange.Checked = true;
            }
        }
        catch { }
    }

    private string GetSelectedItemsText(CheckBoxList cblColumn)
    {
        StringBuilder sbAppend = new StringBuilder();
        try
        {
            for (int j = 0; j < cblColumn.Items.Count; j++)
            {
                if (cblColumn.Items[j].Selected == true)
                {
                    if (sbAppend.Length == 0)
                        sbAppend.Append(Convert.ToString(cblColumn.Items[j].Text));
                    else
                        sbAppend.Append("','" + Convert.ToString(cblColumn.Items[j].Text));
                }
            }
        }
        catch { sbAppend.Clear(); }
        return sbAppend.ToString();
    }

    private string GetSelectedItemsValue(CheckBoxList cblColumn)
    {
        StringBuilder sbAppend = new StringBuilder();
        try
        {
            for (int j = 0; j < cblColumn.Items.Count; j++)
            {
                if (cblColumn.Items[j].Selected == true)
                {
                    if (sbAppend.Length == 0)
                        sbAppend.Append(Convert.ToString(cblColumn.Items[j].Value));
                    else
                        sbAppend.Append("','" + Convert.ToString(cblColumn.Items[j].Value));
                }
            }
        }
        catch { sbAppend.Clear(); }
        return sbAppend.ToString();
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

            string qry = "select distinct (c.Course_Name+' ('+de.dept_acronym+')-'+'Sem'+CONVERT(varchar, r.Current_Semester)+' '+ISNULL(r.Sections,''))Degree,r.Current_Semester  from Registration r,Degree d,Department de,course c where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0  and r.degree_code='" + Convert.ToString(arr[0]).Trim() + "' and r.Batch_Year='" + Convert.ToString(arr[2]).Trim() + "' and r.Current_Semester='" + Convert.ToString(arr[1]).Trim() + "'" + strsec + " ";//and r.college_code='" + Convert.ToString(collegecode).Trim() + "'
            DataTable dDeg = dir.selectDataTable(qry);
            string str1 = string.Empty;
            if (dDeg.Rows.Count > 0)
            {
                string deg = Convert.ToString(dDeg.Rows[0]["Degree"]);
                string semD = Convert.ToString(dDeg.Rows[0]["Current_Semester"]);
                string sems = string.Empty;
                if (semD.Trim() == "1" || semD.Trim() == "2")
                    sems = "I Year ";
                if (semD.Trim() == "3" || semD.Trim() == "4")
                    sems = "II Year ";
                if (semD.Trim() == "5" || semD.Trim() == "6")
                    sems = "III Year ";
                if (semD.Trim() == "7" || semD.Trim() == "8")
                    sems = "IV Year ";
                if (semD.Trim() == "9" || semD.Trim() == "10")
                    sems = "V Year ";

                str1 = sems + "-" + " " + deg;
            }

            //textValue = d2.GetFunction(qry);

            strSubName = (d2.GetFunction("select subject_name from subject where subject_no=" + Convert.ToString(subjectNo) + " "));
            noteValue = Convert.ToString(strScheduledHour) + "," + strSemSchedule;
            string room = string.Empty;
            //room = d2.GetFunction("select rd.room_name from subject s,Room_detail rd where s.roompk=rd.roompk and s.subject_no='" + Convert.ToString(subjectNo) + "'");
            //if (!string.IsNullOrEmpty(room) && room != "0")
            // room = " R:" + room;
            //else
            // room = string.Empty;
            return str1 + "-" + " " + room + "-" + strSubName + "#" + noteValue;
            //return strSubName + "-" + textValue + room + "#" + noteValue;
        }
        catch
        {
            return null;
        }
    }

    private List<string> NewStringColors()
    {
        List<string> clrList = new List<string>();
        clrList.Add("#FEB739");
        clrList.Add("#FF6863");
        clrList.Add("#55D2FF");
        clrList.Add("#C6C6C6");
        clrList.Add("#C5C47B");
        clrList.Add("#CDDC39");
        clrList.Add("#B5E496");
        clrList.Add("#AFDEF8");
        clrList.Add("#F9C4CE");
        clrList.Add("#8EA39A");
        clrList.Add("#7283D1");
        clrList.Add("#06D995");
        clrList.Add("#4CAF50");
        clrList.Add("#57BC30");
        clrList.Add("#8BC34A");
        clrList.Add("#FFCCCC");
        clrList.Add("#FF9800");
        clrList.Add("#00BCD4");
        clrList.Add("#009688");
        clrList.Add("#FF033B");
        clrList.Add("#FF5722");
        clrList.Add("#795548");
        clrList.Add("#9E9E9E");
        clrList.Add("#607D8B");
        clrList.Add("#03A9F4");
        clrList.Add("#E91E63");
        clrList.Add("#CDDC39");
        clrList.Add("#F06292");
        clrList.Add("#3F51B5");
        clrList.Add("#FFC107");
        clrList.Add("#CC0066");
        clrList.Add("#CCCC99");
        clrList.Add("#00CCCC");
        clrList.Add("#FF33CC");
        clrList.Add("#CCFF00");
        clrList.Add("#CCCCCC");
        clrList.Add("#FFCC99");
        clrList.Add("#0099FF");
        clrList.Add("#FF6699");
        clrList.Add("#CCFF99");
        clrList.Add("#CCCCFF");
        clrList.Add("#99CC66");
        clrList.Add("#99FFCC");
        clrList.Add("#FFCC00");
        clrList.Add("#FFCC33");
        clrList.Add("#99CCCC");
        clrList.Add("#673AB7");
        clrList.Add("#CCFFCC");
        return clrList;
    }

    protected void grdStf_TT_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                DataTable Per = dir.selectDataTable(" select distinct  Period1 , convert(varchar(5), start_time, 108) start_time,convert(varchar(5), end_time, 108) end_time from BellSchedule b,Registration r where r.degree_code=b.Degree_Code and r.Batch_Year=b.batch_year and r.Current_Semester=b.semester and r.cc=0 and r.DelFlag<>1 and Exam_Flag<>'Debar' and r.Degree_Code=45 and b.semester=3 and r.batch_year=2017  order by Period1 asc");
                if (Per.Rows.Count > 0)
                {
                    Label lbl1 = (e.Row.FindControl("Label1") as Label);
                    Label lbl2 = (e.Row.FindControl("Label2") as Label);
                    Label lbl3 = (e.Row.FindControl("Label3") as Label);
                    Label lbl4 = (e.Row.FindControl("Label4") as Label);
                    Label lbl5 = (e.Row.FindControl("Label5") as Label);
                    Label lbl6 = (e.Row.FindControl("Label6") as Label);
                    Label lbl7 = (e.Row.FindControl("Label7") as Label);
                    Label lbl8 = (e.Row.FindControl("Label8") as Label);
                    Label lbl9 = (e.Row.FindControl("Label9") as Label);
                    Label lbl10 = (e.Row.FindControl("Label10") as Label);
                    Per.DefaultView.RowFilter = "Period1='1'";
                    DataView dv1 = Per.DefaultView;
                    lbl1.Text = Convert.ToString(dv1[0]["start_time"]) + "-" + Convert.ToString(dv1[0]["end_time"]);

                    Per.DefaultView.RowFilter = "Period1='2'";
                    DataView dv2 = Per.DefaultView;
                    lbl2.Text = Convert.ToString(dv2[0]["start_time"]) + "-" + Convert.ToString(dv2[0]["end_time"]);

                    Per.DefaultView.RowFilter = "Period1='3'";
                    DataView dv3 = Per.DefaultView;
                    lbl3.Text = Convert.ToString(dv3[0]["start_time"]) + "-" + Convert.ToString(dv3[0]["end_time"]);

                    Per.DefaultView.RowFilter = "Period1='4'";
                    DataView dv4 = Per.DefaultView;
                    lbl4.Text = Convert.ToString(dv4[0]["start_time"]) + "-" + Convert.ToString(dv4[0]["end_time"]);

                    Per.DefaultView.RowFilter = "Period1='5'";
                    DataView dv5 = Per.DefaultView;
                    lbl5.Text = Convert.ToString(dv5[0]["start_time"]) + "-" + Convert.ToString(dv5[0]["end_time"]);

                    Per.DefaultView.RowFilter = "Period1='6'";
                    DataView dv6 = Per.DefaultView;
                    lbl6.Text = Convert.ToString(dv6[0]["start_time"]) + "-" + Convert.ToString(dv6[0]["end_time"]);

                    Per.DefaultView.RowFilter = "Period1='7'";
                    DataView dv7 = Per.DefaultView;
                    lbl7.Text = Convert.ToString(dv7[0]["start_time"]) + "-" + Convert.ToString(dv7[0]["end_time"]);

                    Per.DefaultView.RowFilter = "Period1='8'";
                    DataView dv8 = Per.DefaultView;
                    lbl8.Text = Convert.ToString(dv8[0]["start_time"]) + "-" + Convert.ToString(dv8[0]["end_time"]);

                    Per.DefaultView.RowFilter = "Period1='9'";
                    DataView dv9 = Per.DefaultView;
                    lbl9.Text = Convert.ToString(dv9[0]["start_time"]) + "-" + Convert.ToString(dv9[0]["end_time"]);

                    Per.DefaultView.RowFilter = "Period1='10'";
                    DataView dv10 = Per.DefaultView;
                    lbl10.Text = Convert.ToString(dv10[0]["start_time"]) + "-" + Convert.ToString(dv10[0]["end_time"]);

                }
            }

        }
        catch
        {

        }
    }

    protected void grdStfDet_TT_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string subjectcode = e.Row.Cells[2].Text;
            if (class_tt_dic.ContainsKey(subjectcode.Trim()))
            {
                string cellcolor = Convert.ToString(class_tt_dic[subjectcode]);
                e.Row.BackColor = ColorTranslator.FromHtml(cellcolor);
            }
            else
            {
                if (multiple_dic.ContainsKey(subjectcode))
                {
                    string cellcolor = Convert.ToString(multiple_dic[subjectcode]);
                    e.Row.BackColor = ColorTranslator.FromHtml(cellcolor);
                }
            }
        }
    }

    public void btnPrint11()
    {
        string college_code = Convert.ToString(ddlcollege.SelectedValue);
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = d2.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string acr = string.Empty;
        string City = string.Empty;
        if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
        {
            collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "STAFF TIME TABLE FOR THE ACADEMIC YEAR  " + year + "-" + (year + 1);
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);
        spStaffName.InnerHtml = "Satff: " + Convert.ToString(Session["StaffCode"]) + "";

    }

    public void ddlStfName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        grdStf_TT.Visible = false;
        btnComPrint.Visible = false;
        txt_sname.Text = string.Empty;
        txt_scode.Text = string.Empty;
        string staffCode = string.Empty;
        string stfcode = Convert.ToString(Session["Staff_Code"]);
        if (Convert.ToString(Session["Staff_Code"]) == "")
        {
            if (ddlStfName.SelectedItem.Text.ToLower().Trim() != "select")
            {
                if (!string.IsNullOrEmpty(ddlStfName.SelectedValue))
                {
                    staffCode = ddlStfName.SelectedValue;
                    Session["StaffCode"] = staffCode;
                    if (ddlSearchOption.SelectedValue == "0")
                    {
                        txt_scode.Text = staffCode;
                    }
                    else
                    {
                        string staffName = ddlStfName.SelectedItem.Text;
                        string[] splstffName = staffName.Split('-');
                        txt_sname.Text = splstffName[1].Trim();
                    }
                }
            }
            else if (Convert.ToString(ddlSearchOption.SelectedValue).Trim() != "")
            {
                Session["StaffCode"] = Convert.ToString(txt_scode.Text).Trim();
            }
            else
            {
                string staff_Name = Convert.ToString(Convert.ToString(ddlSearchOption.SelectedValue)).Trim();
                if (staff_Name != "")
                {
                    string staff_Code = d2.GetFunction("select staff_code from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like '%" + staff_Name + "%' and college_code='" + collegecode + "'");
                    Session["StaffCode"] = staff_Code.Trim();
                    ddlSearchOption.SelectedValue = staff_Code.Trim();
                }
            }
        }
    }

    protected void binddesig()
    {
        try
        {
            txt_scode.Text = "";
            txt_sname.Text = "";
            cblDesig.Items.Clear();
            txtDesig.Text = "--Select--";
            cbDesig.Checked = false;
            Dictionary<string, string> dicgetcode = new Dictionary<string, string>();
            dicgetcode.Clear();
            Dictionary<string, string> dicdescode = new Dictionary<string, string>();
            dicdescode.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            if (cbl_dept.Items.Count > 0)
            {
                for (int ik = 0; ik < cbl_dept.Items.Count; ik++)
                {
                    if (cbl_dept.Items[ik].Selected == true)
                    {
                        if (!dicgetcode.ContainsKey(Convert.ToString(cbl_dept.Items[ik].Value)))
                        {
                            string selq = "select desig_code,desig_name from desig_master where ((dept_code like '" + Convert.ToString(cbl_dept.Items[ik].Value) + ";%') or (dept_code like '%;" + Convert.ToString(cbl_dept.Items[ik].Value) + "%') or (dept_code like '%" + Convert.ToString(cbl_dept.Items[ik].Value) + "') or (dept_code='" + Convert.ToString(cbl_dept.Items[ik].Value) + "'))";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selq, "Text");
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                for (int jk = 0; jk < ds.Tables[0].Rows.Count; jk++)
                                {
                                    if (!dicdescode.ContainsKey(Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"])))
                                    {
                                        cblDesig.Items.Add(new ListItem(Convert.ToString(ds.Tables[0].Rows[jk]["desig_name"]), Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"])));
                                        dicdescode.Add(Convert.ToString(ds.Tables[0].Rows[jk]["desig_code"]), Convert.ToString(ds.Tables[0].Rows[jk]["desig_name"]));
                                    }
                                }
                            }
                            dicgetcode.Add(Convert.ToString(cbl_dept.Items[ik].Value), Convert.ToString(cbl_dept.Items[ik].Text));
                        }
                    }
                }
            }
            if (cblDesig.Items.Count > 0)
            {
                for (int i = 0; i < cblDesig.Items.Count; i++)
                {
                    cblDesig.Items[i].Selected = true;
                }
                txtDesig.Text = "Designation (" + cblDesig.Items.Count + ")";
                cbDesig.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void stafftype1()
    {
        try
        {
            txt_scode.Text = "";
            txt_sname.Text = "";
            cblStfType.Items.Clear();
            txtStfType.Text = "--Select--";
            cbStfType.Checked = false;
            Dictionary<string, string> dicgetcode = new Dictionary<string, string>();
            dicgetcode.Clear();
            Dictionary<string, string> dicdescode = new Dictionary<string, string>();
            dicdescode.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            if (cblDesig.Items.Count > 0)
            {
                for (int ik = 0; ik < cblDesig.Items.Count; ik++)
                {
                    if (cblDesig.Items[ik].Selected == true)
                    {
                        if (!dicgetcode.ContainsKey(Convert.ToString(cblDesig.Items[ik].Value)))
                        {
                            //string selq = "select desig_code,desig_name from desig_master where ((dept_code like '" + Convert.ToString(cbl_dept.Items[ik].Value) + ";%') or (dept_code like '%;" + Convert.ToString(cbl_dept.Items[ik].Value) + "%') or (dept_code like '%" + Convert.ToString(cbl_dept.Items[ik].Value) + "') or (dept_code='" + Convert.ToString(cbl_dept.Items[ik].Value) + "'))";
                            //select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '13' and desig_code in ('','',';' )

                            string selq = " select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + collcode + "' and desig_code='" + cblDesig.Items[ik].Value + "' ";

                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selq, "Text");
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                for (int jk = 0; jk < ds.Tables[0].Rows.Count; jk++)
                                {
                                    if (!dicdescode.ContainsKey(Convert.ToString(ds.Tables[0].Rows[jk]["stftype"])))
                                    {
                                        cblStfType.Items.Add(new ListItem(Convert.ToString(ds.Tables[0].Rows[jk]["stftype"]), Convert.ToString(ds.Tables[0].Rows[jk]["stftype"])));
                                        dicdescode.Add(Convert.ToString(ds.Tables[0].Rows[jk]["stftype"]), Convert.ToString(ds.Tables[0].Rows[jk]["stftype"]));
                                    }
                                }
                            }
                            dicgetcode.Add(Convert.ToString(cblDesig.Items[ik].Value), Convert.ToString(cblDesig.Items[ik].Text));
                        }
                    }
                }
            }
            if (cblStfType.Items.Count > 0)
            {
                for (int i = 0; i < cblStfType.Items.Count; i++)
                {
                    cblStfType.Items[i].Selected = true;
                }
                txtStfType.Text = "StaffType (" + cblStfType.Items.Count + ")";
                cbStfType.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void staffcategory()
    {
        try
        {
            txt_scode.Text = "";
            txt_sname.Text = "";
            ddlStfName.Items.Clear();
            Dictionary<string, string> dicgetcode = new Dictionary<string, string>();
            dicgetcode.Clear();
            Dictionary<string, string> dicdescode = new Dictionary<string, string>();
            dicdescode.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string deptcode = string.Empty;
            if (cbl_dept.Items.Count > 0)
            {
                for (int ik = 0; ik < cbl_dept.Items.Count; ik++)
                {
                    if (cbl_dept.Items[ik].Selected == true)
                    {
                        if (deptcode == "")
                            deptcode = Convert.ToString(cbl_dept.Items[ik].Value);
                        else
                            deptcode = deptcode + "','" + Convert.ToString(cbl_dept.Items[ik].Value);
                    }
                }
            }
            string design = string.Empty;
            if (cblDesig.Items.Count > 0)
            {
                for (int ik = 0; ik < cblDesig.Items.Count; ik++)
                {
                    if (cblDesig.Items[ik].Selected == true)
                    {
                        if (design == "")
                            design = Convert.ToString(cblDesig.Items[ik].Value);
                        else
                            design = design + "','" + Convert.ToString(cblDesig.Items[ik].Value);
                    }
                }
            }
            string type = string.Empty;

            int jd = ddlStfName.Items.Count;
            if (cblStfType.Items.Count > 0)
            {
                for (int ik = 0; ik < cblStfType.Items.Count; ik++)
                {
                    if (cblStfType.Items[ik].Selected == true)
                    {
                        if (type == "")
                            type = Convert.ToString(cblStfType.Items[ik].Value);
                        else
                            type = type + "','" + Convert.ToString(cblStfType.Items[ik].Value);
                    }
                }
            }
            if (type != "" && design != "" && deptcode != "")
            {
                //for (int ik = 0; ik < cblStfType.Items.Count; ik++)
                //{
                //    if (cblStfType.Items[ik].Selected == true)
                //    {
                //        if (!dicgetcode.ContainsKey(Convert.ToString(cblStfType.Items[ik].Value)))
                //        {
                //string selq = "select desig_code,desig_name from desig_master where ((dept_code like '" + Convert.ToString(cbl_dept.Items[ik].Value) + ";%') or (dept_code like '%;" + Convert.ToString(cbl_dept.Items[ik].Value) + "%') or (dept_code like '%" + Convert.ToString(cbl_dept.Items[ik].Value) + "') or (dept_code='" + Convert.ToString(cbl_dept.Items[ik].Value) + "'))";
                //select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '13' and desig_code in ('','',';' )

                //string selq = "select distinct sc.category_code,sc.category_name   from stafftrans t,staffcategorizer sc where t.category_code=sc.category_code and college_code ='" + collcode + "' and stftype ='" + cblStfType.Items[ik].Value + "'";


                string selq = "select sm.staff_code,(sm.staff_code+' - '+sm.staff_name) as Staff_Name from staffmaster sm,stafftrans st,staff_appl_master sa,Desig_Master ds,hrdept_master hm where sm.staff_code=st.staff_code and sm.appl_no=sa.appl_no and sm.resign='0' and sm.settled='0' and ISNULL(sm.Discontinue,'0')='0' and st.latestrec='1' and sm.college_code='" + collcode + "' and sa.desig_code=ds.desig_code and ds.collegeCode=sa.college_code and hm.dept_code=sa.dept_code and hm.college_code=sa.college_code and  sa.dept_code in('" + deptcode + "') and sa.desig_code in('" + design + "') and staff_type in('" + type + "') order by len(sm.staff_code),sm.staff_Code";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlStfName.DataSource = ds;
                    ddlStfName.DataTextField = "Staff_Name";
                    ddlStfName.DataValueField = "staff_code";
                    ddlStfName.DataBind();
                    ddlStfName.Items.Insert(0, "Select");
                }
            }
            else
            {
                ddlStfName.Items.Insert(0, "Select");
            }
            //                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //                {
            //                    for (int jk = 0; jk < ds.Tables[0].Rows.Count; jk++)
            //                    {
            //                        if (!dicdescode.ContainsKey(Convert.ToString(ds.Tables[0].Rows[jk]["category_code"])))
            //                        {
            //                            ddlStfName.Items.Add(new ListItem(Convert.ToString(ds.Tables[0].Rows[jk]["category_name"]), Convert.ToString(ds.Tables[0].Rows[jk]["category_code"])));
            //                            dicdescode.Add(Convert.ToString(ds.Tables[0].Rows[jk]["category_code"]), Convert.ToString(ds.Tables[0].Rows[jk]["category_name"]));
            //                        }
            //                    }
            //                }
            //                dicgetcode.Add(Convert.ToString(cblStfType.Items[ik].Value), Convert.ToString(cblStfType.Items[ik].Text));
            //            }
            //        }
            //    }
            //}
            //if (cbl_stype.Items.Count > 0)
            //{
            //    for (int i = 0; i < ddlStfName.Items.Count; i++)
            //    {
            //        cbl_scat.Items[i].Selected = true;
            //    }
            //    txt_scat.Text = "StaffCategory (" + cbl_scat.Items.Count + ")";
            //    cb_scat.Checked = true;
            //}
        }
        catch (Exception ex)
        {
        }
    }
}