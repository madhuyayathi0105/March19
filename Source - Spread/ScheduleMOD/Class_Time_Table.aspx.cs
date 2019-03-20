using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using InsproDataAccess;
using System.Configuration;

public partial class Class_Time_Table : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string collegecode = string.Empty;
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
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    Hashtable htData = new Hashtable();

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
                string DegCode = "0";
                string Batch = "0";
                string Branch = "0";
                string Sem = "0";
                bindcollege();
                collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);

                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                if (ddlDegree.Items.Count > 0)
                    DegCode = Convert.ToString(ddlDegree.SelectedItem.Value);
                BindBranch(singleuser, group_user, DegCode, collegecode, usercode);
                if (ddlBatch.Items.Count > 0)
                    Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
                if (ddlBranch.Items.Count > 0)
                    Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
                BindSem(Branch, Batch, collegecode);
                if (ddlSem.Items.Count > 0)
                    Sem = Convert.ToString(ddlSem.SelectedItem.Value);
                BindSectionDetail(Batch, Branch, Sem, collegecode);
                txtFrmDt.Attributes.Add("readonly", "readonly");
                txtToDt.Attributes.Add("readonly", "readonly");

            }
            collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
        }
        catch (Exception ex) { }
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
            if (ds.Tables[0].Rows.Count > 0)
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

    private void BindBatch()
    {
        try
        {
            string DegCode = "0";
            string Batch = "0";
            string Branch = "0";
            string Sem = "0";
            ds.Dispose();
            ds.Reset();
            string Master1 = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
            }
            else
            {
                Master1 = Session["usercode"].ToString();
            }
            string collegecode = Session["collegecode"].ToString();
            string strbinddegree = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "' order by batch_year desc";
            ds = d2.select_method_wo_parameter(strbinddegree, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "Batch_year";
                ddlBatch.DataValueField = "Batch_year";
                ddlBatch.DataBind();
            }
            BindDegree(singleuser, group_user, collegecode, usercode);
            if (ddlDegree.Items.Count > 0)
                DegCode = Convert.ToString(ddlDegree.SelectedItem.Value);
            if (ddlBatch.Items.Count > 0)
                Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
            if (ddlBranch.Items.Count > 0)
                Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
            BindBranch(singleuser, group_user, DegCode, collegecode, usercode);
            BindSem(Branch, Batch, collegecode);
            if (ddlSem.Items.Count > 0)
                Sem = Convert.ToString(ddlSem.SelectedItem.Value);
            BindSectionDetail(Batch, Branch, Sem, collegecode);
        }
        catch (Exception ex)
        {
            lblMainErr.Text = ex.ToString();
        }
    }

    private void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            string DegCode = "0";
            string Batch = "0";
            string Branch = "0";
            string Sem = "0";
            ddlDegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlDegree.DataSource = ds;
                ddlDegree.DataTextField = "course_name";
                ddlDegree.DataValueField = "course_id";
                ddlDegree.DataBind();
            }
            if (ddlDegree.Items.Count > 0)
                DegCode = Convert.ToString(ddlDegree.SelectedItem.Value);
            if (ddlBatch.Items.Count > 0)
                Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
            if (ddlBranch.Items.Count > 0)
                Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
            BindBranch(singleuser, group_user, DegCode, collegecode, usercode);
            BindSem(Branch, Batch, collegecode);
            if (ddlSem.Items.Count > 0)
                Sem = Convert.ToString(ddlSem.SelectedItem.Value);
            BindSectionDetail(Batch, Branch, Sem, collegecode);
        }
        catch (Exception ex)
        {
            lblMainErr.Text = ex.ToString();
        }
    }

    private void BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            course_id = "0";
            string Batch = "0";
            string Branch = "0";
            string Sem = "0";
            if (ddlDegree.Items.Count > 0)
                course_id = Convert.ToString(ddlDegree.SelectedItem.Value);
            ddlBranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBranch.DataSource = ds;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
            }
            if (ddlBatch.Items.Count > 0)
                Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
            if (ddlBranch.Items.Count > 0)
                Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
            BindSem(Branch, Batch, collegecode);
            if (ddlSem.Items.Count > 0)
                Sem = Convert.ToString(ddlSem.SelectedItem.Value);
            BindSectionDetail(Batch, Branch, Sem, collegecode);
        }
        catch (Exception ex)
        {
            lblMainErr.Text = "Please Select the Degree";
        }
    }

    private void BindSectionDetail(string strbatch, string strbranch, string strSem, string Collcode)
    {
        try
        {
            strbatch = "0";
            if (ddlBatch.Items.Count > 0)
                strbatch = ddlBatch.SelectedValue.ToString();
            strbranch = "0";
            if (ddlBranch.Items.Count > 0)
                strbranch = ddlBranch.SelectedValue.ToString();
            strSem = "0";
            if (ddlSem.Items.Count > 0)
                strSem = ddlSem.SelectedItem.Value.ToString();
            ddlSec.Items.Clear();
            ds.Dispose();
            ds.Reset();

            // ds = d2.select_method_wo_parameter("select distinct TT_sec from TT_ClassTimeTable T,TT_ClassTimeTabledet TT Where T.TT_ClassPK=TT.TT_ClassFK and TT_degCode='" + strbranch + "' and TT_batchyear='" + strbatch + "' and TT_sem='" + strSem + "' and TT_ColCode='" + Collcode + "'", "Text");

            ds = d2.select_method_wo_parameter("select distinct sections from Semester_Schedule where batch_year='" + strbatch + "' and degree_code='" + strbranch + "' and semester='" + strSem + "'", "Text");
            //select distinct sections from Semester_Schedule where batch_year='' and degree_code='' and semester=''

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSec.DataSource = ds;
                ddlSec.DataTextField = "sections";
                ddlSec.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblMainErr.Text = ex.ToString();
        }
    }

    private void BindSem(string strbranch, string strbatchyear, string collegecode)
    {
        try
        {
            strbatchyear = "0";
            if (ddlBatch.Items.Count > 0)
                strbatchyear = ddlBatch.SelectedValue.ToString();
            strbranch = "0";
            if (ddlBranch.Items.Count > 0)
                strbranch = ddlBranch.SelectedValue.ToString();
            ddlSem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSem(strbranch, strbatchyear, collegecode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]).ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                        ddlSem.Items.Add(i.ToString());
                    else if (first_year == true && i != 2)
                        ddlSem.Items.Add(i.ToString());
                }
            }
        }
        catch (Exception ex) { lblMainErr.Text = ex.ToString(); }
    }

    protected void ddlcollege_change(object sender, EventArgs e)
    {
        string DegCode = "0";
        string Batch = "0";
        string Branch = "0";
        string Sem = "0";
        collegecode = ddlcollege.Items.Count > 0 ? Convert.ToString(ddlcollege.SelectedItem.Value) : Convert.ToString(Session["collegecode"]);
        BindBatch();
        BindDegree(singleuser, group_user, collegecode, usercode);
        if (ddlDegree.Items.Count > 0)
            DegCode = Convert.ToString(ddlDegree.SelectedItem.Value);
        BindBranch(singleuser, group_user, DegCode, collegecode, usercode);
        if (ddlBatch.Items.Count > 0)
            Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
        if (ddlBranch.Items.Count > 0)
            Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
        BindSem(Branch, Batch, collegecode);
        if (ddlSem.Items.Count > 0)
            Sem = Convert.ToString(ddlSem.SelectedItem.Value);
        BindSectionDetail(Batch, Branch, Sem, collegecode);

    }

    protected void ddlBatch_Change(object sender, EventArgs e)
    {
        string Batch = "0";
        string Branch = "0";
        string Sem = "0";
        if (ddlBatch.Items.Count > 0)
            Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
        if (ddlBranch.Items.Count > 0)
            Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
        BindSem(Branch, Batch, collegecode);
        if (ddlSem.Items.Count > 0)
            Sem = Convert.ToString(ddlSem.SelectedItem.Value);
        BindSectionDetail(Batch, Branch, Sem, collegecode);
    }

    protected void ddlDegree_Change(object sender, EventArgs e)
    {
        string DegCode = "0";
        string Batch = "0";
        string Branch = "0";
        string Sem = "0";
        if (ddlDegree.Items.Count > 0)
            DegCode = Convert.ToString(ddlDegree.SelectedItem.Value);
        if (ddlBatch.Items.Count > 0)
            Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
        if (ddlBranch.Items.Count > 0)
            Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
        BindBranch(singleuser, group_user, DegCode, collegecode, usercode);
        BindSem(Branch, Batch, collegecode);
        if (ddlSem.Items.Count > 0)
            Sem = Convert.ToString(ddlSem.SelectedItem.Value);
        BindSectionDetail(Batch, Branch, Sem, collegecode);
    }

    protected void ddlBranch_Change(object sender, EventArgs e)
    {
        string Batch = "0";
        string Branch = "0";
        string Sem = "0";
        if (ddlBatch.Items.Count > 0)
            Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
        if (ddlBranch.Items.Count > 0)
            Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
        BindSem(Branch, Batch, collegecode);
        if (ddlSem.Items.Count > 0)
            Sem = Convert.ToString(ddlSem.SelectedItem.Value);
        BindSectionDetail(Batch, Branch, Sem, collegecode);
    }

    protected void ddlSem_Change(object sender, EventArgs e)
    {
        string Batch = "0";
        string Branch = "0";
        string Sem = "0";
        if (ddlBatch.Items.Count > 0)
            Batch = Convert.ToString(ddlBatch.SelectedItem.Value);
        if (ddlBranch.Items.Count > 0)
            Branch = Convert.ToString(ddlBranch.SelectedItem.Value);
        if (ddlSem.Items.Count > 0)
            Sem = Convert.ToString(ddlSem.SelectedItem.Value);
        BindSectionDetail(Batch, Branch, Sem, collegecode);
    }

    protected void radSemWise_Change(object sender, EventArgs e)
    {
        tdlbFrm.Visible = false;
        txtFrmDt.Visible = false;
        lblToDt.Visible = false;
        txtToDt.Visible = false;
    }

    protected void radDayWise_Change(object sender, EventArgs e)
    {
        tdlbFrm.Visible = true;
        txtFrmDt.Visible = true;
        lblToDt.Visible = true;
        txtToDt.Visible = true;
        txtFrmDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
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
            GridView1.Visible = false;
            DataRow drNew = null;
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


            string SchOrder = d2.GetFunction("select distinct top 1 schOrder from PeriodAttndSchedule");
            DateTime dt1 = new DateTime();
            string fDate = string.Empty;

            Hashtable htSubject = new Hashtable();
            DataSet dsAllDetails = new DataSet();

            string batchy = Convert.ToString(ddlBatch.SelectedValue);
            string degCodeV = Convert.ToString(ddlBranch.SelectedValue);
            string semes = Convert.ToString(ddlSem.SelectedValue);
            string sect = Convert.ToString(ddlSec.SelectedValue);
            string qryGetDegDetails = "select distinct s.subject_no,s.subject_code,s.subject_name,r.Batch_Year,de.Dept_Name,r.Current_Semester,r.Sections,r.degree_code  from collinfo cc, Registration r,subject s,syllabus_master sm,Department de,course c,Degree d where  c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and de.Dept_Code=d.Dept_Code and r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and r.Current_Semester=sm.semester and s.syll_code=sm.syll_code and cc.college_code=r.college_code and r.batch_year='" + batchy + "' and r.degree_code='" + degCodeV + "' and r.current_semester='" + semes + "' and ltrim(rtrim(isnull(r.Sections,'')))='" + sect + "' and ISNULL(r.DelFlag,0)=0 and r.Exam_Flag<>'Debar' and r.CC=0";


            DataSet dsDegreeDetails = d2.select_method_wo_parameter(qryGetDegDetails, "Text");
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
                            dsAllDetails.Tables[0].DefaultView.RowFilter = "batch_year='" + dsDegreeDetails.Tables[0].Rows[i]["batch_year"].ToString() + "' and degree_code='" + dsDegreeDetails.Tables[0].Rows[i]["degree_code"].ToString() + "' and semester='" + dsDegreeDetails.Tables[0].Rows[i]["Current_Semester"].ToString() + "' " + strSec + "";
                            dvSemTT = dsAllDetails.Tables[0].DefaultView;

                            checkRow = false;
                            if (!hat.ContainsKey((dsDegreeDetails.Tables[0].Rows[i]["batch_year"].ToString() + "-" + dsDegreeDetails.Tables[0].Rows[i]["degree_code"].ToString() + "-" + dsDegreeDetails.Tables[0].Rows[i]["Current_Semester"].ToString() + "-" + strSec)))
                            {
                                hat.Add(dsDegreeDetails.Tables[0].Rows[i]["batch_year"].ToString() + "-" + dsDegreeDetails.Tables[0].Rows[i]["degree_code"].ToString() + "-" + dsDegreeDetails.Tables[0].Rows[i]["Current_Semester"].ToString() + "-" + strSec, dsDegreeDetails.Tables[0].Rows[i]["degree_code"].ToString());

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
                                    cellValue = cellValue + ";" + val[0];
                                    cellNoteValue = cellNoteValue + ";" + val[1];
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
                            }
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
                GridView1.DataSource = dtTTDisp;
                GridView1.DataBind();
                GridView1.Visible = true;

            }
            if (noOfHrs != 0)
            {
                for (int i = 1; i <= noOfHrs; i++)
                {
                    GridView1.Columns[i].Visible = true;
                }
            }
        }
        catch
        {

        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected string getSpreadCellValue(string strScheduledHour, string strSemSchedule)
    {
        try
        {
            string strSubName = "";
            string textValue = "";
            string noteValue = "";
            string room = string.Empty;
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

            string qry = "select distinct (CONVERT(varchar,r.Batch_Year)+'-'+c.Course_Name+' ('+de.dept_acronym+')-'+'Sem'+CONVERT(varchar, r.Current_Semester)+' '+ISNULL(r.Sections,''))Degree  from Registration r,Degree d,Department de,course c where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0  and r.degree_code='" + Convert.ToString(arr[0]).Trim() + "' and r.Batch_Year='" + Convert.ToString(arr[2]).Trim() + "' and r.Current_Semester='" + Convert.ToString(arr[1]).Trim() + "'" + strsec + " ";//and r.college_code='" + Convert.ToString(collegecode).Trim() + "'

            textValue = d2.GetFunction(qry);

            strSubName = (d2.GetFunction("select subject_name from subject where subject_no=" + Convert.ToString(subjectNo) + " "));

            noteValue = Convert.ToString(strScheduledHour) + "," + strSemSchedule;
            room = d2.GetFunction("select rd.room_name from subject s,Room_detail rd where s.roompk=rd.roompk and s.subject_no='" + Convert.ToString(subjectNo) + "'");

            bool staffSelector = false;
            string staffName = string.Empty;
            string minimumabsentsms = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector'");
            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = 0;
                int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    staffSelector = true;
                }
            }
            else if (splitminimumabsentsms.Length > 0)
            {
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    staffSelector = true;
                }
            }
            string qryStaff = string.Empty;
            DataTable dtStff = new DataTable();
            string sect = string.Empty;
            string staffNamedet = string.Empty;
            if (ddlSec.Enabled == true)
            {
                sect = " and ss.sections='" + ddlSec.SelectedItem.Text.ToString() + "'";
            }
            string[] sss = strScheduledHour.Split('-');
            if (sss.Length > 0)
            {
                for (int z = 0; z < sss.Length; z++)
                {
                    dtStff.Dispose();
                    string stafCode = Convert.ToString(sss[z]);
                    if (staffSelector)
                    {
                        qryStaff = "select distinct sm.staff_code,sm.staff_name from  staff_selector ss,staffmaster sm,subjectchooser sc where sc.subject_no=ss.subject_no and sm.staff_code=ss.staff_code and sc.staffcode=ss.staff_code and ss.subject_no='" + subjectNo + "' " + sect + " and sm.staff_code='" + stafCode + "'";

                    }
                    else
                    {
                        qryStaff = "select sm.staff_code,sm.staff_name from staff_selector ss,staffmaster sm where sm.staff_code=ss.staff_code and subject_no='" + subjectNo + "' " + sect + " and sm.staff_code='" + stafCode + "'";

                    }
                    dtStff = dirAcc.selectDataTable(qryStaff);
                    if (dtStff.Rows.Count > 0)
                    {
                        string sc = Convert.ToString(dtStff.Rows[0]["staff_code"]);
                        string sn = Convert.ToString(dtStff.Rows[0]["staff_name"]);
                        if (string.IsNullOrEmpty(staffNamedet))
                            staffNamedet = sc + "-" + sn;
                        else
                            staffNamedet = staffNamedet + " -" + sc + "-" + sn;
                    }

                }
            }



            //foreach (DataRow dr1 in dtStff.Rows)
            //{
            //    string sc = Convert.ToString(dr1["staff_code"]);
            //    string sn = Convert.ToString(dr1["staff_name"]);
            //    if (string.IsNullOrEmpty(staffNamedet))
            //        staffNamedet = sc + "-" + sn;
            //    else
            //        staffNamedet = staffNamedet + " -" + sc + "-" + sn;
            //}

            if (!string.IsNullOrEmpty(room) && room != "0")
                room = "  $" + room;
            else
                room = string.Empty;

            return strSubName + "-" + subType + "-" + staffNamedet + "-" + room + "#" + noteValue;
        }
        catch
        {
            return null;
        }
    }

    protected void lnkAttMark(object sender, EventArgs e)
    {
    }

}