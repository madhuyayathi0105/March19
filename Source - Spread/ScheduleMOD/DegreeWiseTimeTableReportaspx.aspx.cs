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
using InsproDataAccess;
using wc = System.Web.UI.WebControls;
using Farpoint = FarPoint.Web.Spread;
using System.Configuration;

public partial class DegreeWiseTimeTableReportaspx : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DAccess2 dt = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DataSet ds1 = new DataSet();
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();
    DataTable dtCommon = new DataTable();
    ReuasableMethods rs = new ReuasableMethods();
    Hashtable htData = new Hashtable();
    Hashtable hat = new Hashtable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string build = "", buildvalue = string.Empty;
    string qryCollege = string.Empty;
    string qry = string.Empty;
    string qryBatch = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                userCollegeCode = Convert.ToString(Session["collegecode"]).Trim();
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
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
            
            if (!IsPostBack)
            {
                Bindcollege();
                BindRightsBaseBatch();
                binddegree();
                bindbranch();
                loadSem();
                bindSubject();
                bindStaff();
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
            ddlCollege.Items.Clear();
            dtCommon.Clear();
            ddlCollege.Enabled = false;
            DataSet dsprint = new DataSet();
            dsprint.Clear();
            string qryUserCodeOrGroupCode = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
            {
                qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtCommon;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
                ddlCollege.Enabled = true;
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void BindRightsBaseBatch()
    {
        try
        {
            DataSet dsBatch = new DataSet();
            userCode = string.Empty;
            groupUserCode = string.Empty;
            qryUserOrGroupCode = string.Empty;
            collegeCode = string.Empty;
            ds.Clear();
            chkBatch.Checked = false;
            cblBatch.Items.Clear();
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(";"))
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
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollege = " and r.college_code in(" + collegeCode + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollege = " and r.college_code in(" + collegeCode + ")";
            }

            dsBatch.Clear();
            if (!string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string qry = "select distinct batch_year from tbl_attendance_rights r where batch_year<>'' " + qryUserOrGroupCode + " order by batch_year desc";
                dsBatch = da.select_method_wo_parameter(qry, "Text");
            }
            qryBatch = string.Empty;
            if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
            {
                List<int> lstBatch = dsBatch.Tables[0].AsEnumerable().Select(r => r.Field<int>("batch_year")).ToList();
                if (lstBatch.Count > 0)
                    qryBatch = " and r.Batch_Year in('" + string.Join("','", lstBatch.ToArray()) + "')";
            }
            string batchquery = string.Empty;
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryCollege))
            {
                batchquery = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 and r.cc='0' and delflag='0' and exam_flag<>'debar' " + qryCollege + qryBatch + " order by r.Batch_Year desc";
                //ds.Clear();
                ds = da.select_method_wo_parameter(batchquery, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblBatch.DataSource = ds;
                    cblBatch.DataTextField = "Batch_Year";
                    cblBatch.DataValueField = "Batch_Year";
                    cblBatch.DataBind();

                    checkBoxListselectOrDeselect(cblBatch, true);
                    CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void binddegree()
    {
        try
        {
            ds.Clear();
            txtDegree.Text = "---Select---";
            string batchCode = string.Empty;
            chkDegree.Checked = false;
            cblDegree.Items.Clear();
            //userCode = Session["usercode"].ToString();
            //singleUser = Session["single_user"].ToString();
            //groupUserCode = Session["group_code"].ToString();
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();

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
            string valBatch = string.Empty;
            if (cblBatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch))
            {
                string selDegree = "SELECT DISTINCT c.course_id,c.course_name,c.Priority,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') " + columnfield + " ORDER BY CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = da.select_method_wo_parameter(selDegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblDegree.DataSource = ds;
                cblDegree.DataTextField = "course_name";
                cblDegree.DataValueField = "course_id";
                cblDegree.DataBind();
                checkBoxListselectOrDeselect(cblDegree, true);
                CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void bindbranch()
    {
        try
        {
            string degreecode = string.Empty;
            //collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            txtBranch.Text = "---Select---";
            chkBranch.Checked = false;
            cblBranch.Items.Clear();
            ds.Clear();
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;
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

            string valBatch = string.Empty;// rs.GetSelectedItemsValueAsString(cblBatch);
            string valDegree = string.Empty;//rs.GetSelectedItemsValueAsString(cblBranch);
            if (cblBatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (cblDegree.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(cblDegree);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') AND c.Course_Id in('" + valDegree + "') " + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = da.select_method_wo_parameter(selBranch, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblBranch.DataSource = ds;
                cblBranch.DataTextField = "dept_name";
                cblBranch.DataValueField = "degree_code";
                cblBranch.DataBind();
                checkBoxListselectOrDeselect(cblBranch, true);
                CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void loadSem()
    {
        ds.Clear();
        collegeCode = string.Empty;
        string valBatch = string.Empty;
        string valDegree = string.Empty;
        if (ddlCollege.Items.Count > 0)
            collegeCode = ddlCollege.SelectedValue.ToString().Trim();
        if (cblBatch.Items.Count > 0)
            valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
        if (cblDegree.Items.Count > 0)
            valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
        string SelSem = string.Empty;
        if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
        {
            SelSem = "select distinct current_semester from Registration where Batch_Year in('" + valBatch + "') and degree_code in('" + valDegree + "') and cc=0 and delflag<>1 and Exam_Flag<>'debar'  and isredo<>1  order by Current_Semester ";//and degree_code in('" + valDegree + "')
            ds = da.select_method_wo_parameter(SelSem, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_sem.DataSource = ds;
                cbl_sem.DataTextField = "current_semester";
                cbl_sem.DataValueField = "current_semester";
                cbl_sem.DataBind();
                checkBoxListselectOrDeselect(cbl_sem, true);
                CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, lbl_org_sem.Text, "--Select--");
            }
        }

    }
    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            BindRightsBaseBatch();
            binddegree();
            bindbranch();
            loadSem();

        }
        catch (Exception ex)
        {
        }
    }
    protected void chkBatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
            binddegree();
            bindbranch();
            loadSem();

        }
        catch (Exception ex)
        {
        }
    }
    protected void cblBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
            binddegree();
            bindbranch();
            loadSem();


        }
        catch (Exception ex)
        {
        }
    }
    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            bindbranch();
            loadSem();

        }
        catch (Exception ex)
        {
        }
    }
    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            bindbranch();
            loadSem();

        }
        catch (Exception ex)
        {
        }
    }
    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            loadSem();

        }
        catch (Exception ex)
        {
        }
    }
    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            loadSem();

        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_sem_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sem, cbl_sem, txt_sem, lbl_org_sem.Text, "--Select--");

    }
    protected void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, lbl_org_sem.Text, "--Select--");
    }
    protected void lnkAttMark(object sender, EventArgs e)
    {
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            Print();
            DataTable dtTTDisp = new DataTable();
            dtTTDisp.Columns.Add("DegDet");
            dtTTDisp.Columns.Add("RoomNo");
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
            GridView1.Visible = false;
            DataRow drNew = null;
            htData.Clear();
            string[] DaysAcronym = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            string[] DaysName = new string[7] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            string sql = "select max(No_of_hrs_per_day)HoursPerDay,MAX(nodays)NoOfDays from PeriodAttndSchedule";
            DataSet ds = da.select_method_wo_parameter(sql, "Text");
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

            string staffcode = Convert.ToString(Session["staff_code"]).Trim();
            string SchOrder = da.GetFunction("select distinct top 1 schOrder from PeriodAttndSchedule");
            DateTime dt1 = new DateTime();
            string fDate = string.Empty;
            Hashtable htSubject = new Hashtable();
            DataSet dsAllDetails = new DataSet();
            string batchy = string.Empty;
            string degCodeV = string.Empty;
            string semes = string.Empty;
            string sect = string.Empty;

            if (cblBatch.Items.Count > 0)
                batchy = rs.getCblSelectedValue(cblBatch);
            else
            {

            }

            if (cblBranch.Items.Count > 0)
                degCodeV = rs.getCblSelectedValue(cblBranch);
            else
            {

            }
            if (cbl_sem.Items.Count > 0)
                semes = rs.getCblSelectedValue(cbl_sem);
            else
            {

            }

            string qryGetDegDetails = "select distinct s.subject_no,s.subject_code,s.subject_name,r.Batch_Year,de.Dept_Name,r.Current_Semester,r.Sections,r.degree_code  from collinfo cc, Registration r,subject s,syllabus_master sm,Department de,course c,Degree d where  c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and de.Dept_Code=d.Dept_Code and r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and r.Current_Semester=sm.semester and s.syll_code=sm.syll_code and cc.college_code=r.college_code and r.batch_year in('" + batchy + "') and r.degree_code in('" + degCodeV + "') and r.current_semester in('" + semes + "')  and ISNULL(r.DelFlag,0)=0 and r.Exam_Flag<>'Debar' and r.CC=0";

            DataSet dsDegreeDetails = da.select_method_wo_parameter(qryGetDegDetails, "Text");
            DataTable dicDeg = new DataTable();
            if (dsDegreeDetails.Tables.Count > 0 && dsDegreeDetails.Tables[0].Rows.Count > 0)
            {
                dicDeg = dsDegreeDetails.Tables[0].DefaultView.ToTable(true, "Batch_Year", "degree_code", "Current_Semester", "Sections");
                foreach (DataRow dts in dsDegreeDetails.Tables[0].Rows)
                {
                    string sub = Convert.ToString(dts["subject_no"]);
                    string subName = Convert.ToString(dts["subject_name"]);
                    if (!htSubject.ContainsKey(sub))
                    {
                        htSubject.Add(sub, subName);
                    }
                }
            }
            string qryAllDetails = string.Empty;
            foreach (DataRow dr in dicDeg.Rows)
            {
                string batch = Convert.ToString(dr["Batch_Year"]);
                string degCode = Convert.ToString(dr["degree_code"]);
                string seme = Convert.ToString(dr["Current_Semester"]);
                string sec = Convert.ToString(dr["Sections"]);
                string sections = string.Empty;
                if (!string.IsNullOrEmpty(sec))
                    sections = "  and  Sections='" + sec + "'";
                if (string.IsNullOrEmpty(qryAllDetails))
                {
                    qryAllDetails = "select * from Semester_Schedule where  batch_year='" + batch + "' and degree_code='" + degCode + "' and semester='" + seme + "'" + sections;
                }
                else
                {
                    qryAllDetails = qryAllDetails + "  union all select * from Semester_Schedule where  batch_year='" + batch + "' and degree_code='" + degCode + "' and semester='" + seme + "'" + sections;
                }
            }

            dsAllDetails = da.select_method_wo_parameter(qryAllDetails, "Text");
            DataView dvSemTT = new DataView();
            DataView dvAlternateSemTT = new DataView();
            Hashtable hat = new Hashtable();

            string textValue = string.Empty;
            string rooom = string.Empty;
            if (dsDegreeDetails.Tables.Count > 0 && dsDegreeDetails.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsDegreeDetails.Tables[0].Rows.Count; i++)
                {
                    string strSec = string.Empty;
                    textValue = string.Empty;
                    rooom = string.Empty;
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
                            dsAllDetails.Tables[0].DefaultView.RowFilter = "batch_year='" + dsDegreeDetails.Tables[0].Rows[i]["batch_year"].ToString() + "' and degree_code='" + dsDegreeDetails.Tables[0].Rows[i]["degree_code"].ToString() + "' and semester='" + dsDegreeDetails.Tables[0].Rows[i]["Current_Semester"].ToString() + "' " + strSec + "";
                            dvSemTT = dsAllDetails.Tables[0].DefaultView;

                            //-------------------------------
                            string qry = "select distinct (CONVERT(varchar,r.Batch_Year)+'-'+c.Course_Name+' ('+de.dept_acronym+')-'+'Sem'+CONVERT(varchar, r.Current_Semester)+' '+ISNULL(r.Sections,''))Degree  from Registration r,Degree d,Department de,course c where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0  and r.degree_code='" + Convert.ToString(dsDegreeDetails.Tables[0].Rows[i]["degree_code"]).Trim() + "' and r.Batch_Year='" + Convert.ToString(dsDegreeDetails.Tables[0].Rows[i]["batch_year"]).Trim() + "' and r.Current_Semester='" + Convert.ToString(dsDegreeDetails.Tables[0].Rows[i]["Current_Semester"]).Trim() + "'" + strSec + " ";//and r.college_code='" + Convert.ToString(collegecode).Trim() + "'

                            textValue = da.GetFunction(qry);
                            //---------------------------------

                            checkRow = false;
                            if (!hat.ContainsKey((dsDegreeDetails.Tables[0].Rows[i]["batch_year"].ToString() + "-" + dsDegreeDetails.Tables[0].Rows[i]["degree_code"].ToString() + "-" + dsDegreeDetails.Tables[0].Rows[i]["Current_Semester"].ToString() + "-" + strSec)))
                            {
                                hat.Add(dsDegreeDetails.Tables[0].Rows[i]["batch_year"].ToString() + "-" + dsDegreeDetails.Tables[0].Rows[i]["degree_code"].ToString() + "-" + dsDegreeDetails.Tables[0].Rows[i]["Current_Semester"].ToString() + "-" + strSec, dsDegreeDetails.Tables[0].Rows[i]["degree_code"].ToString());

                                if (dvSemTT.Count > 0)
                                {
                                    strDegDetails = Convert.ToString(dvSemTT[0]["degree_code"]) + "," + Convert.ToString(dvSemTT[0]["semester"]) + "," + Convert.ToString(dvSemTT[0]["batch_year"]) + "," + Convert.ToString(dvSemTT[0]["ttname"]) + "," + Convert.ToString(dvSemTT[0]["fromdate"]).Split(' ')[0] + "," + Convert.ToString(dvSemTT[0]["sections"]);



                                    if (string.IsNullOrEmpty(staffcode))
                                    {
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
                                                                string[] subD = Convert.ToString(arr[k]).Split('-');
                                                                if (htSubject.ContainsKey(Convert.ToString(subD[0])))
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
                                                            string[] subD = Convert.ToString(val).Split('-');
                                                            if (htSubject.ContainsKey(Convert.ToString(subD[0])))
                                                            {
                                                                spreadCellValue = getSpreadCellValue(val, strDegDetails);
                                                            }
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
                                            checkRow = true;
                                        }
                                    }
                                    else
                                    {

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
                                                        if (val.Contains(Convert.ToString((staffcode))))
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
                                                                    if (arr[k].Contains(Convert.ToString((staffcode))))
                                                                    {
                                                                        if (spreadCellValue == "")
                                                                            spreadCellValue = getSpreadCellValue(Convert.ToString(arr[k]), strDegDetails);
                                                                        else
                                                                            spreadCellValue = spreadCellValue + ";" + getSpreadCellValue(Convert.ToString(arr[k]), strDegDetails);
                                                                    }
                                                                }

                                                            }
                                                            else
                                                            {

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

                            if (htData.Count > 0)
                            {
                                string rM = string.Empty;
                                for (int row = 0; row < noOfDays; row++)
                                {
                                    rM = string.Empty;
                                    drNew = dtTTDisp.NewRow();
                                    string r = row.ToString();
                                    string dayName = DaysName[row];
                                    string dayAcronym = DaysAcronym[row];
                                    drNew["DegDet"] = textValue;

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
                                                        //singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                                                        cellValue = val[0];
                                                        cellNoteValue = val[1];
                                                        string roomStr = cellNoteValue.Split('-')[0];
                                                        if (string.IsNullOrEmpty(rM))
                                                            rM = (RoomInfo(roomStr) != "0" || !string.IsNullOrEmpty(Convert.ToString(RoomInfo(roomStr))) ? RoomInfo(roomStr) : "");
                                                        else
                                                            rM = rM + "-" + (RoomInfo(roomStr) != "0" || !string.IsNullOrEmpty(Convert.ToString(RoomInfo(roomStr))) ? RoomInfo(roomStr) : "");
                                                    }
                                                    else
                                                    {
                                                        cellValue = cellValue + ";" + val[0];
                                                        cellNoteValue = cellNoteValue + ";" + val[1];
                                                        string roomStr = cellNoteValue.Split('-')[0];
                                                        if (string.IsNullOrEmpty(rM))
                                                            rM = (RoomInfo(roomStr) != "0" || !string.IsNullOrEmpty(Convert.ToString(RoomInfo(roomStr))) ? RoomInfo(roomStr) : "");
                                                        else
                                                            rM = rM + "-" + (RoomInfo(roomStr) != "0" || !string.IsNullOrEmpty(Convert.ToString(RoomInfo(roomStr))) ? RoomInfo(roomStr) : "");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                string[] val = Convert.ToString(htData[r + c]).Split('#');
                                                if (val.Length > 1)
                                                {
                                                    cellValue = val[0];
                                                    cellNoteValue = val[1];
                                                    string roomStr = cellNoteValue.Split('-')[0];
                                                    if (string.IsNullOrEmpty(rM))
                                                        rM = (RoomInfo(roomStr) != "0" || !string.IsNullOrEmpty(Convert.ToString(RoomInfo(roomStr))) ? RoomInfo(roomStr) : "");
                                                    else
                                                        rM = rM + "-" + (RoomInfo(roomStr) != "0" || !string.IsNullOrEmpty(Convert.ToString(RoomInfo(roomStr))) ? RoomInfo(roomStr) : "");
                                                }
                                            }
                                            string lbl1 = "P" + col + "Val";
                                            string lbl2 = "TT_" + col;
                                            drNew[lbl1] = cellValue;
                                            drNew[lbl2] = cellNoteValue;
                                        }
                                    }
                                    drNew["RoomNo"] = rM;
                                    dtTTDisp.Rows.Add(drNew);
                                }
                                htData.Clear();
                            }
                        }
                    }

                }
            }

            if (dtTTDisp.Rows.Count > 0)
            {
                GridView1.DataSource = dtTTDisp;
                GridView1.DataBind();
                GridView1.Visible = true;

            }
            int cell = GridView1.Columns.Count;
            if (noOfHrs != 0)
            {
                for (int i = 0; i < cell; i++)
                {
                    if (i < noOfHrs + 3)
                        GridView1.Columns[i].Visible = true;
                    else
                        GridView1.Columns[i].Visible = false;
                }

            }
        }
        catch
        {

        }
    }
    protected string getSpreadCellValue(string strScheduledHour, string strSemSchedule)
    {
        try
        {
            string dispalyText = string.Empty;
            string strSubName = string.Empty;
            string strSubCode = string.Empty;
            string strSubAcr = string.Empty;
            string textValue = "";
            string noteValue = "";
            //string room = string.Empty;
            string subjectNo = strScheduledHour.Split('-')[0];
            string[] arr = strSemSchedule.Split(',');

            string sec = Convert.ToString(arr[5]).Trim();
            string strsec = "";

            if (sec != "" && sec != "-1" && sec != "all" && sec != null)
            {

                strsec = "and r.sections='" + sec + "'";
            }
            noteValue = Convert.ToString(strScheduledHour) + "," + strSemSchedule;

            DataTable dtSubject = dirAcc.selectDataTable("select subject_name,subject_code,acronym from subject where subject_no=" + Convert.ToString(subjectNo) + " ");
            if (dtSubject.Rows.Count > 0)
            {
                if (CheckBoxList1.Items[1].Selected)
                {
                    strSubName = Convert.ToString(dtSubject.Rows[0]["subject_name"]);
                    if (string.IsNullOrEmpty(dispalyText))
                        dispalyText = strSubName;
                    else
                        dispalyText = dispalyText + "-" + strSubName;
                }
                if (CheckBoxList1.Items[0].Selected)
                {
                    strSubCode = Convert.ToString(dtSubject.Rows[0]["subject_code"]);
                    if (string.IsNullOrEmpty(dispalyText))
                        dispalyText = strSubCode;
                    else
                        dispalyText = dispalyText + "-" + strSubCode;
                }
                if (CheckBoxList1.Items[2].Selected)
                {
                    strSubAcr = Convert.ToString(dtSubject.Rows[0]["acronym"]);
                    if (string.IsNullOrEmpty(dispalyText))
                        dispalyText = strSubAcr;
                    else
                        dispalyText = dispalyText + "-" + strSubAcr;
                }
            }


            string qryStaff = "select sm.staff_code,sm.staff_name from staff_selector ss,staffmaster sm where sm.staff_code=ss.staff_code and subject_no='" + subjectNo + "' and ss.staff_code='" + Convert.ToString(strScheduledHour.Split('-')[1]) + "'";

            string sc = string.Empty;
            string sn = string.Empty;
            DataTable dtStff = dirAcc.selectDataTable(qryStaff);
            string staffNamedet = string.Empty;
            foreach (DataRow dr1 in dtStff.Rows)
            {
                if (cblStaff.Items[1].Selected)
                {
                    sc = Convert.ToString(dr1["staff_code"]);
                    if (string.IsNullOrEmpty(dispalyText))
                        dispalyText = sc;
                    else
                        dispalyText = dispalyText + "-" + sc;
                }
                if (cblStaff.Items[0].Selected)
                {
                    sn = Convert.ToString(dr1["staff_name"]);
                    if (string.IsNullOrEmpty(dispalyText))
                        dispalyText = sn;
                    else
                        dispalyText = dispalyText + "-" + sn;
                }



            }

            //if (!string.IsNullOrEmpty(room) && room != "0")
            //    room = "  $" + room;
            //else
            //    room = string.Empty;

            return dispalyText + "#" + noteValue;//+ room
        }
        catch
        {
            return null;
        }
    }
    protected void OnDataBound(object sender, EventArgs e)
    {
        try
        {
            for (int rowIndex = GridView1.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = GridView1.Rows[rowIndex];
                GridViewRow previousRow = GridView1.Rows[rowIndex + 1];

                string l1 = (row.FindControl("lblDegreeDet") as Label).Text;
                string l2 = (previousRow.FindControl("lblDegreeDet") as Label).Text;
                if (l1 == l2)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                           previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;
                }
            }
        }
        catch
        {
        }
    }
    protected string RoomInfo(string subjectNo)
    {
        try
        {
            string room = string.Empty;
            room = da.GetFunction("select rd.room_name from subject s,Room_detail rd where s.roompk=rd.roompk and s.subject_no='" + Convert.ToString(subjectNo) + "'");
            return room;
        }
        catch
        {
            return null;
        }
    }
    public void bindStaff()
    {
        try
        {
            cblStaff.Items.Clear();
            cblStaff.Items.Add("StaffName");
            cblStaff.Items.Add("Staff Code");
            checkBoxListselectOrDeselect(cblStaff, true);
            CallCheckboxListChange(chkStaff, cblStaff, txtStaff, "Staff", "--Select--");
        }
        catch
        {
        }
    }
    protected void chkStaff_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(chkStaff, cblStaff, txtStaff, "Staff", "--Select--");

    }
    protected void cblStaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chkStaff, cblStaff, txtStaff, "Staff", "--Select--");
    }
    public void bindSubject()
    {
        try
        {
            CheckBoxList1.Items.Clear();
            CheckBoxList1.Items.Add("Subject Code");
            CheckBoxList1.Items.Add("Subject Name");
            CheckBoxList1.Items.Add("Subject Acronym");
            checkBoxListselectOrDeselect(CheckBoxList1, true);
            CallCheckboxListChange(CheckBox1, CheckBoxList1, txtSubject, "Subject", "--Select--");
        }
        catch
        {
        }
    }
    protected void CheckBox1_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(CheckBox1, CheckBoxList1, txtSubject, "Subject", "--Select--");

    }
    protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(CheckBox1, CheckBoxList1, txtSubject, "Subject", "--Select--");
    }
    public void Print()
    {
        string college_code = Convert.ToString(ddlCollege.SelectedValue);
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = da.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string City = string.Empty;
        string acr = string.Empty;
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
            acr = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["acr"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "GENRAl TIME TABLE FOR THE ACADEMIC YEAR  " + year + "-" + (year + 1);
        int sem = 0;
        //int.TryParse(Convert.ToString(ddlSem.SelectedValue).Trim(), out sem);
        //spReportName.InnerHtml = "Timetable for " + ((sem % 2 == 0) ? "Even" : "ODD") + " " + Convert.ToString(lblSem.Text).Trim() + " " + Convert.ToString(ddlBatch.SelectedValue).Trim();
        //spDegreeName.InnerHtml = "<b>Degree: </b>" + Convert.ToString(ddlDegree.SelectedItem.Text).Trim();
        //spSem.InnerHtml = "<b>Semester: </b>" + Convert.ToString(ddlSem.SelectedItem.Text).Trim();
        //spProgremme.InnerHtml = "<b>Programme: </b>" + Convert.ToString(ddlBranch.SelectedItem.Text).Trim();
        //spSection.InnerHtml = ((ddlSec.Items.Count > 0 && ddlSec.SelectedItem.Text.Trim().ToLower() != "" && ddlSec.SelectedItem.Text.Trim().ToLower() != "all") ? "<b>Section: </b>" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() : "");

        //StringBuilder SbHtml = new StringBuilder();
        //SbHtml.Append("<div style='padding-left:5px;height: 900px; width:650px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 650px; padding-left:15px; font-family:Times New Roman; font-size:16px;'><tr><td rowspan='5'><img src='" + "college/Left_Logo.jpeg" + "' style='height:80px; width:80px;'/></td><td colspan='7' style='align:center'>" + collegeName + " " + collegeCateg + "</td></tr><tr><td colspan='7' style='align:center'>" + collegeAff + "</td></tr><tr><td colspan='7' style='align:center'>" + collegeAdd + "</td></tr><tr><td colspan='7' style='align:center'>" + collegePhone + " " + collegeFax + "</td></tr><tr><td colspan='7' style='align:center'>" + collegeWeb + " " + collegeEmai + "</td></tr><tr><td colspan='8'><hr style='height:2px; width:650px;'></td></tr></table>");
        //SbHtml.Append("</div>");

        //contentDiv.InnerHtml += SbHtml.ToString();
        //contentDiv.Visible = true;
        //ScriptManager.RegisterStartupScript(this, GetType(), "btnPrint", "PrintDiv();", true);
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
}