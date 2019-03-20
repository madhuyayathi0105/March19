using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;
using wc = System.Web.UI.WebControls;
using System.Text;
using InsproDataAccess;
using System.Globalization;
using System.IO;
using System.Drawing;

public partial class AttendanceMOD_CommonHomeWork : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection ssql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    ReuasableMethods rs = new ReuasableMethods();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    NotificationSend ns = new NotificationSend();
    Hashtable hat1 = new Hashtable();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string grouporusercode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string qryCollege = string.Empty;
    string qryBatch = string.Empty;
    static string subno = string.Empty;
    Institution institute;
    int selBranch = 0;
    string qryBranch = string.Empty;
    string newBranchCode = string.Empty;
    string selectedBatchYears = string.Empty;
    DataTable dtable = new DataTable();
    static Dictionary<int, string> dicRowColor = new Dictionary<int, string>();
    DataRow drow = null;
    static ArrayList arrayst;
    static ArrayList arr;
    static string staffcodesession = "";
    static string subnosession = "";
    static string ss = "";
    static string sunum = string.Empty;
    string subjectco = string.Empty;
    static string sbjeno = string.Empty;
    static string idnos = string.Empty;
    string heading = string.Empty;
    static string seme = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("AttendanceHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/AttendanceMOD/AttendanceHome.aspx");
                return;
            }
        }
        if (!IsPostBack)
        {
            staffcodesession = Session["Staff_Code"].ToString();
            txtdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            setLabelText();
            bindcollege();
            bindbatch();
            binddegree();
            bindbranch();
            bindsem();
            bindsec();
            bindsubtype();
            bindsubject();
        }
    }

    #region Bind Values
    public void bindcollege()
    {
        ddlcollege.Items.Clear();
        string grporusercode = string.Empty;
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grporusercode = " and user_code=" + Session["usercode"].ToString().Trim() + "";
        }


        string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where cp.college_code=cf.college_code " + grporusercode + "";
        ds = d2.select_method_wo_parameter(selectQuery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
    }

    public void bindbatch()
    {
        try
        {
            DataSet dsBatch = new DataSet();
            usercode = string.Empty;
            group_user = string.Empty;
            qryUserOrGroupCode = string.Empty;
            collegecode = string.Empty;
            ds.Clear();
            ddlbatch.Items.Clear();
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(";"))
                {
                    string[] group_semi = group.Split(';');
                    group_user = Convert.ToString(group_semi[0]);
                }
                if (!string.IsNullOrEmpty(group_user))
                {
                    qryUserOrGroupCode = " and user_id='" + group_user + "'";
                }
            }
            else if (Session["usercode"] != null)
            {
                usercode = Convert.ToString(Session["usercode"]).Trim();
                if (!string.IsNullOrEmpty(usercode))
                {
                    qryUserOrGroupCode = " and user_id='" + usercode + "'";
                }
            }
            if (ddlcollege.Items.Count > 0 && ddlcollege.Visible)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegecode))
                {
                    qryCollege = " and r.college_code in(" + collegecode + ")";
                }
            }

            dsBatch.Clear();
            if (!string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string qry = "select distinct batch_year from tbl_attendance_rights r where batch_year<>'' " + qryUserOrGroupCode + " order by batch_year desc";
                dsBatch = d2.select_method_wo_parameter(qry, "Text");
            }
            qryBatch = string.Empty;
            if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
            {
                List<int> lstBatch = dsBatch.Tables[0].AsEnumerable().Select(r => r.Field<int>("batch_year")).ToList();
                if (lstBatch.Count > 0)
                    qryBatch = " and r.Batch_Year in('" + string.Join("','", lstBatch.ToArray()) + "')";
            }
            string batchquery = string.Empty;
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(qryCollege))
            {
                batchquery = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 and r.cc='0' and delflag='0' and exam_flag<>'debar' " + qryCollege + qryBatch + " order by r.Batch_Year desc";
                //ds.Clear();
                ds = d2.select_method_wo_parameter(batchquery, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlbatch.DataSource = ds;
                    ddlbatch.DataTextField = "Batch_Year";
                    ddlbatch.DataValueField = "Batch_Year";
                    ddlbatch.DataBind();
                }
            }
        }
        catch { }

        //ds = d2.select_method_wo_parameter("bind_batch", "sp");
        //int count = ds.Tables[0].Rows.Count;
        //if (count > 0)
        //{
        //    ddlbatch.DataSource = ds;
        //    ddlbatch.DataTextField = "batch_year";
        //    ddlbatch.DataValueField = "batch_year";
        //    ddlbatch.DataBind();
        //}
        //int count1 = ds.Tables[1].Rows.Count;
        //if (count > 0)
        //{
        //    int max_bat = 0;
        //    max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
        //    ddlbatch.SelectedValue = max_bat.ToString();
        //    con.Close();
        //}
    }

    public void binddegree()
    {
        try
        {
            ds.Clear();
            txtdegree.Text = "---Select---";
            string batchCode = string.Empty;
            ddldegree.Items.Clear();
            collegecode = string.Empty;
            if (ddlcollege.Items.Count > 0)
                collegecode = ddlcollege.SelectedValue.ToString().Trim();

            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and dp.group_code='" + group_user + "'";
            }


            else if (Session["usercode"] != null)
            {
                columnfield = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string valBatch = string.Empty;

            if (ddlbatch.Items.Count > 0)
                valBatch = Convert.ToString(ddlbatch.SelectedValue);
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(valBatch))
            {
                string selDegree = "SELECT DISTINCT c.course_id,c.course_name,c.Priority,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegecode + "') AND r.Batch_Year in('" + valBatch + "') " + columnfield + " ORDER BY CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = d2.select_method_wo_parameter(selDegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
                checkBoxListselectOrDeselect(ddldegree, false);
                CallCheckboxListChange(chckdegree, ddldegree, txtdegree, lbldegree.Text, "--Select--");
            }
        }
        catch { }

        //string StaffCode = Session["staff_code"].ToString().Trim();
        //if (string.IsNullOrEmpty(StaffCode))
        //{
        //    usercode = Session["usercode"].ToString();
        //    collegecode = Session["collegecode"].ToString();
        //    singleuser = Session["single_user"].ToString();
        //    group_user = Session["group_code"].ToString();
        //    if (group_user.Contains(';'))
        //    {
        //        string[] group_semi = group_user.Split(';');
        //        group_user = group_semi[0].ToString();
        //    }
        //    has.Clear();
        //    has.Add("single_user", singleuser);
        //    has.Add("group_code", group_user);
        //    has.Add("college_code", collegecode);
        //    has.Add("user_code", usercode);
        //    ds = d2.select_method("bind_degree", has, "sp");
        //    int count1 = ds.Tables[0].Rows.Count;
        //    if (count1 > 0)
        //    {
        //        ddldegree.DataSource = ds;
        //        ddldegree.DataTextField = "course_name";
        //        ddldegree.DataValueField = "course_id";
        //        ddldegree.DataBind();
        //        checkBoxListselectOrDeselect(ddldegree, true);
        //        CallCheckboxListChange(chckdegree, ddldegree, txtdegree, lbldegree.Text, "--Select--");
        //    }
        //}
        //else
        //{
        //    string batch=string.Empty;
        //if(!string.IsNullOrEmpty(ddlbatch.SelectedValue))
        //{
        //    batch=ddlbatch.SelectedValue;
        //}
        //string qry = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages,staff_selector s where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + collegecode + " and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + " and s.batch_year='" + batch + "' and s.staff_code='" + StaffCode + "'";
        //}
    }

    public void bindbranch()
    {
        try
        {
            string degreecode = string.Empty;
            //collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            txtbranch.Text = "---Select---";
            chckbranch.Checked = false;
            ddlbranch.Items.Clear();
            ds.Clear();
            collegecode = string.Empty;
            if (ddlcollege.Items.Count > 0)
                collegecode = ddlcollege.SelectedValue.ToString().Trim();
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
                columnfield = " and dp.group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }

            string valBatch = string.Empty;
            string valDegree = string.Empty;
            if (ddlbatch.Items.Count > 0)
                valBatch = Convert.ToString(ddlbatch.SelectedValue);
            if (ddldegree.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(ddldegree);

            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegecode + "') AND r.Batch_Year in('" + valBatch + "') AND c.Course_Id in('" + valDegree + "') " + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = d2.select_method_wo_parameter(selBranch, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
                checkBoxListselectOrDeselect(ddlbranch, false);
                CallCheckboxListChange(chckbranch, ddlbranch, txtbranch, lblbranch.Text, "--Select--");
            }
        }
        catch { }



        //    ddlbranch.Items.Clear();
        //    string degVal = string.Empty;
        //    string qry = string.Empty;
        //    has.Clear();
        //    usercode = Session["usercode"].ToString();
        //    collegecode = Session["collegecode"].ToString();
        //    singleuser = Session["single_user"].ToString();
        //    group_user = Session["group_code"].ToString();
        //    if (group_user.Contains(';'))
        //    {
        //        string[] group_semi = group_user.Split(';');
        //        group_user = group_semi[0].ToString();
        //    }
        //    has.Add("single_user", singleuser);
        //    has.Add("group_code", group_user);
        //    has.Add("course_id", ddldegree.SelectedValue);
        //    has.Add("college_code", collegecode);
        //    has.Add("user_code", usercode);

        //    if (ddldegree.Items.Count > 0)
        //    {
        //        for (int deg = 0; deg < ddldegree.Items.Count; deg++)
        //        {
        //            if (ddldegree.Items[deg].Selected == true)
        //            {
        //                if (string.IsNullOrEmpty(degVal))
        //                {
        //                    degVal = ddldegree.Items[deg].Value;
        //                }
        //                else
        //                {
        //                    degVal = degVal + "," + ddldegree.Items[deg].Value;
        //                }
        //            }
        //        }
        //    }
        //    ds = d2.BindBranchMultiple(singleuser, group_user, degVal, collegecode, usercode);
        //    int count2 = ds.Tables[0].Rows.Count;
        //    if (count2 > 0)
        //    {
        //        ddlbranch.DataSource = ds;
        //        ddlbranch.DataTextField = "dept_name";
        //        ddlbranch.DataValueField = "degree_code";
        //        ddlbranch.DataBind();

        //        checkBoxListselectOrDeselect(ddlbranch, true);
        //        CallCheckboxListChange(chckbranch, ddlbranch, txtbranch, lblbranch.Text, "--Select--");
        //    }
        //}
        //catch
        //{
        //}
    }

    public void bindsem()
    {
        //if (string.IsNullOrEmpty(Convert.ToString(Session["staff_code"]).Trim()))
        //{
        string selBranch = string.Empty;
        string valBatch = string.Empty;
        string valDegree = string.Empty;
        string subtype = string.Empty;
        ddlsem.ClearSelection();
        ddlsem.Items.Clear();
        if (ddlbatch.Items.Count > 0)
            valBatch = Convert.ToString(ddlbatch.SelectedValue);
        if (ddlbranch.Items.Count > 0)
            valDegree = rs.GetSelectedItemsValueAsString(ddlbranch);

        string selectQ = "select distinct current_semester from registration where batch_year=" + valBatch + " and degree_code in('" + valDegree + "')  and cc=0 and delflag<>1 and exam_flag<>'debar' order by current_semester";
        DataTable dtSem = dirAcc.selectDataTable(selectQ);
        if (dtSem.Rows.Count > 0)
        {
            ddlsem.DataSource = dtSem;
            ddlsem.DataTextField = "current_semester";
            ddlsem.DataValueField = "current_semester";
            ddlsem.DataBind();
        }
        //}
        #region
        //ddlsem.Items.Clear();
        //Boolean first_year = false;
        //has.Clear();
        //collegecode = Session["collegecode"].ToString();
        //has.Add("degree_code", ddlbranch.SelectedValue.ToString());
        //has.Add("batch_year", ddlbatch.SelectedValue.ToString());
        //has.Add("college_code", collegecode);
        //ds = d2.select_method("bind_sem", has, "sp");
        //int duration = 0;
        //int i = 0;
        //int selBranch = 0;
        //string qryBranch = string.Empty;
        //string newBranchCode = string.Empty;
        //string selectedBatchYears = string.Empty;

        //foreach (ListItem li in ddlbranch.Items)
        //{
        //    if (li.Selected)
        //    {
        //        selBranch++;
        //        if (string.IsNullOrEmpty(newBranchCode.Trim()))
        //        {
        //            newBranchCode = "'" + li.Value + "'";
        //        }
        //        else
        //        {
        //            newBranchCode += ",'" + li.Value + "'";
        //        }
        //    }
        //}
        //if (!string.IsNullOrEmpty(ddlbatch.SelectedValue))
        //{
        //    selectedBatchYears = ddlbatch.SelectedValue;
        //}
        //if (selBranch > 0)
        //{
        //    qryBranch = " and degree_code in(" + newBranchCode + ")";
        //}
        //string strgetsem = "select distinct ndurations,first_year_nonsemester from ndegree where batch_year in ('" + selectedBatchYears.ToString() + "') and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "' " + qryBranch + " order by NDurations desc ";
        //ds.Dispose();
        //ds.Reset();
        //ds = d2.select_method_wo_parameter(strgetsem, "Text");
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
        //    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
        //    for (i = 1; i <= duration; i++)
        //    {
        //        if (first_year == false)
        //        {
        //            ddlsem.Items.Add(i.ToString());
        //        }
        //        else if (first_year == true && i != 2)
        //        {
        //            ddlsem.Items.Add(i.ToString());
        //        }
        //    }
        //}
        //else
        //{
        //    strgetsem = "select distinct duration,first_year_nonsemester  from degree where college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "' " + qryBranch + " order by  duration desc";
        //    ddlsem.Items.Clear();
        //    ds.Dispose();
        //    ds.Reset();
        //    ds = d2.select_method_wo_parameter(strgetsem, "Text");
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
        //        duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
        //        for (i = 1; i <= duration; i++)
        //        {
        //            if (first_year == false)
        //            {
        //                ddlsem.Items.Add(i.ToString());
        //            }
        //            else if (first_year == true && i != 2)
        //            {
        //                ddlsem.Items.Add(i.ToString());
        //            }
        //        }
        //    }
        //}
        #endregion
    }

    public void bindsec()
    {
        try
        {
            string semes = string.Empty;
            chcksec.Checked = false;
            ddlsec.Items.Clear();
            txtsec.Text = "-- Select --";
            txtsec.Enabled = false;
            selBranch = 0;
            qryBranch = string.Empty;
            newBranchCode = string.Empty;
            selectedBatchYears = string.Empty;
            foreach (ListItem li in ddlbranch.Items)
            {
                if (li.Selected)
                {
                    selBranch++;
                    if (string.IsNullOrEmpty(newBranchCode.Trim()))
                    {
                        newBranchCode = "'" + li.Value + "'";
                    }
                    else
                    {
                        newBranchCode += ",'" + li.Value + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(ddlbatch.SelectedValue))
            {
                selectedBatchYears = ddlbatch.SelectedValue;
            }
            if (selBranch > 0)
            {
                qryBranch = " and degree_code in(" + newBranchCode + ")";
            }
            if (ddlsem.Items.Count > 0)
                semes = Convert.ToString(ddlsem.SelectedValue);
            string strect = "select distinct case when (isnull(Rtrim(Ltrim(r.sections)),'') ='') then 'Empty' else isnull(Rtrim(Ltrim(r.sections)),'') end  as sections,isnull(Rtrim(Ltrim(r.sections)),'') as SecVal from registration r,staff_selector ss where r.Sections=ss.Sections and r.Current_Semester='" + semes + "' and r.batch_year in ('" + selectedBatchYears.ToString() + "') " + qryBranch + "  and ss.sections<>'-1' and delflag=0 and exam_flag<>'Debar'  order by SecVal";//and sections<>' ' //union select'Empty' as sections,'' as SecVal,,,union select'Empty' as sections,'' as SecVal
            DataSet ds = d2.select_method_wo_parameter(strect, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataValueField = "SecVal";
                ddlsec.DataBind();
                checkBoxListselectOrDeselect(ddlsec, true);
                CallCheckboxListChange(chcksec, ddlsec, txtsec, lblsec.Text, "--Select--");
                txtsec.Enabled = true;
            }
            else
            {
                txtsec.Enabled = false;
            }
        }
        catch
        {
        }


        //ddlsec.Items.Clear();
        //has.Clear();
        //has.Add("batch_year", ddlbatch.SelectedValue.ToString());
        //has.Add("degree_code", ddlbranch.SelectedValue);
        //ds = d2.select_method("bind_sec", has, "sp");
        //int count5 = ds.Tables[0].Rows.Count;
        //if (count5 > 0)
        //{
        //    ddlsec.DataSource = ds;
        //    ddlsec.DataTextField = "sections";
        //    ddlsec.DataValueField = "sections";
        //    ddlsec.DataBind();
        //    ddlsec.Enabled = true;
        //}
        //else
        //{
        //    ddlsec.Enabled = false;
        //}
    }

    public void bindsubtype()
    {
        try
        {

            string valBatch = string.Empty;
            string valDegree = string.Empty;
            txtSubtype.Text = "---Select---";
            chkSubtype.Checked = false;
            cblSubtype.Items.Clear();
            if (ddlbatch.Items.Count > 0)
                valBatch = Convert.ToString(ddlbatch.SelectedValue);
            if (ddlbranch.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(ddlbranch);
            if (!string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                string selBranch = "select distinct ss.subject_type from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=sy.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no  and  sy.Batch_Year in('" + valBatch + "') and sy.degree_code in('" + valDegree + "')  and sy.semester='" + Convert.ToString(ddlsem.SelectedValue) + "' order by ss.subject_type";//sub_sem.syll_Code = subject.syll_code and
                ds = d2.select_method_wo_parameter(selBranch, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblSubtype.DataSource = ds;
                    cblSubtype.DataTextField = "subject_type";
                    cblSubtype.DataValueField = "subject_type";
                    cblSubtype.DataBind();
                    checkBoxListselectOrDeselect(cblSubtype, false);
                    CallCheckboxListChange(chkSubtype, cblSubtype, txtSubtype, lblSuType.Text, "--Select--");
                }
            }
            #region
            //string batch = string.Empty;
            //string degCode = string.Empty;
            //string sem = string.Empty;
            //cblSubtype.Items.Clear();
            //txtSubtype.Text = "--Select--";
            //chkSubtype.Checked = false;
            //if (!string.IsNullOrEmpty(ddlbatch.SelectedValue))
            //{
            //    batch = ddlbatch.SelectedValue;
            //}
            //if (ddlbranch.Items.Count > 0)
            //{
            //    for (int i = 0; i < ddlbranch.Items.Count; i++)
            //    {
            //        if (ddlbranch.Items[i].Selected)
            //        {
            //            if (string.IsNullOrEmpty(degCode))
            //            {
            //                degCode = ddlbranch.Items[i].Value;
            //            }
            //            else
            //            {
            //                degCode = degCode + "','" + ddlbranch.Items[i].Value;
            //            }
            //        }
            //    }
            //}
            //if (!string.IsNullOrEmpty(ddlsem.SelectedValue))
            //{
            //    sem = ddlsem.SelectedValue;
            //}

            //string qrys = "select distinct ss.subject_type from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=s.syll_code and ss.subType_no=s.subType_no and  sy.Batch_Year='" + batch + "' and sy.degree_code in('" + degCode + "') and sy.semester='" + sem + "'  order by ss.subject_type";//promote_count=1
            //DataSet dsset = d2.select_method_wo_parameter(qrys, "Text");
            //if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
            //{
            //    cblSubtype.DataSource = dsset;
            //    cblSubtype.DataTextField = "subject_type";
            //    cblSubtype.DataValueField = "subject_type";
            //    cblSubtype.DataBind();
            //    checkBoxListselectOrDeselect(cblSubtype, true);
            //    CallCheckboxListChange(chkSubtype, cblSubtype, txtSubtype, lblSuType.Text, "--Select--");
            //}
            #endregion
        }
        catch
        {
        }
    }

    public void bindsubject()
    {
        try
        {
            string degreecode = string.Empty;
            txtSubject.Text = "---Select---";
            cbSubjet.Checked = false;
            cblSubject.Items.Clear();
            ds.Clear();
            collegecode = string.Empty;
            if (ddlcollege.Items.Count > 0)
                collegecode = ddlcollege.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            string subtype = string.Empty;
            string sem = Convert.ToString(ddlsem.SelectedValue);
            if (cblSubtype.Items.Count > 0)
                subtype = getCblSelectedText(cblSubtype);
            if (ddlbatch.Items.Count > 0)
                valBatch = Convert.ToString(ddlbatch.SelectedValue);
            if (ddlbranch.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(ddlbranch);

            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree) && !string.IsNullOrEmpty(subtype))
            {
                if (!string.IsNullOrEmpty(Session["staff_code"].ToString().Trim()))
                {
                    selBranch = " select distinct subject_name,subject_code,CONVERT(nvarchar(max),isnull(subject.subject_code,'')+'-'+isnull(subject.subject_name,'')) as text from subject,sub_sem,syllabus_master,staff_selector  where  subject.subtype_no = sub_sem.subtype_no  and subject.syll_code=syllabus_master.syll_code and subject.subject_no =staff_selector.subject_no  and  syllabus_master.degree_code in('" + valDegree + "') and syllabus_master.batch_year in('" + valBatch + "') and sub_sem.subject_type in(" + subtype + ") and staff_selector.staff_code='" + Session["staff_code"].ToString() + "' and syllabus_master.semester='" + sem + "' order by subject.subject_name";
                }
                else
                {
                    selBranch = "select distinct subject_name,subject_code,CONVERT(nvarchar(max),isnull(subject.subject_code,'')+'-'+isnull(subject.subject_name,'')) as text from subject,sub_sem,syllabus_master where  subject.subtype_no = sub_sem.subtype_no  and subject.syll_code=syllabus_master.syll_code   and  syllabus_master.degree_code in('" + valDegree + "') and syllabus_master.batch_year in('" + valBatch + "') and syllabus_master.semester='" + sem + "' and sub_sem.subject_type in(" + subtype + ")  order by subject.subject_name";
                }
                ds = d2.select_method_wo_parameter(selBranch, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblSubject.DataSource = ds;
                cblSubject.DataTextField = "text";
                cblSubject.DataValueField = "subject_code";
                cblSubject.DataBind();
                checkBoxListselectOrDeselect(cblSubject, false);
                CallCheckboxListChange(cbSubjet, cblSubject, txtSubject, lblSubject.Text, "--Select--");
            }
            #region
            //cblSubject.Items.Clear();
            //txtSubject.Text = "--Select--";
            //cbSubjet.Checked = false;
            //string batch = string.Empty;
            //string sem = string.Empty;
            //string degCode = string.Empty;
            //if (!string.IsNullOrEmpty(ddlsem.SelectedValue))
            //{
            //    sem = ddlsem.SelectedValue.ToString();
            //}
            //if (!string.IsNullOrEmpty(ddlbatch.SelectedValue))
            //{
            //    batch = ddlbatch.SelectedValue.ToString();
            //}
            //if (ddlbranch.Items.Count > 0)
            //{
            //    for (int deg = 0; deg < ddlbranch.Items.Count; deg++)
            //    {
            //        if (ddlbranch.Items[deg].Selected)
            //        {
            //            if (degCode == "")
            //            {
            //                degCode = ddlbranch.Items[deg].Value;
            //            }
            //            else
            //            {
            //                degCode = degCode + "','" + ddlbranch.Items[deg].Value;
            //            }
            //        }
            //    }
            //}

            //string subtype = string.Empty;
            //if (cblSubtype.Items.Count > 0)
            //    subtype = getCblSelectedText(cblSubtype);


            //if (!string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(degCode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(subtype))
            //{
            //    string SelectQ = "select distinct s.subject_code,s.subject_name,CONVERT(nvarchar(max),isnull(s.subject_code,'')+'-'+isnull(s.subject_name,'')) as text from syllabus_master sy,sub_sem ss,subject s where s.syll_code=sy.syll_code and  ss.subType_no=s.subType_no and  sy.Batch_Year='" + batch + "' and sy.degree_code in('" + degCode + "') and sy.semester='" + sem + "' and ss.subject_type in(" + subtype + ") order by s.subject_code,s.subject_name,CONVERT(nvarchar(max),isnull(s.subject_code,'')+'-'+isnull(s.subject_name,''))";//and ss.promote_count=1
            //    DataSet dtsubject = d2.select_method_wo_parameter(SelectQ, "Text");

            //    if (dtsubject.Tables.Count > 0 && dtsubject.Tables[0].Rows.Count > 0)
            //    {
            //        cblSubject.DataSource = dtsubject;
            //        cblSubject.DataTextField = "text";
            //        cblSubject.DataValueField = "subject_code";
            //        cblSubject.DataBind();
            //        checkBoxListselectOrDeselect(cblSubject, true);
            //        CallCheckboxListChange(cbSubjet, cblSubject, txtSubject, lblSubject.Text, "--Select--");
            //    }
            //}
            #endregion
        }
        catch
        {
        }
    }
    #endregion

    #region SelectedIndexChanged

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbatch();
        binddegree();
        bindbranch();
        bindsem();
        bindsec();
        bindsubtype();
        bindsubject();
        gviewhomewrk.Visible = false;
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddegree();
        bindbranch();
        bindsem();
        bindsec();
        bindsubtype();
        bindsubject();
        gviewhomewrk.Visible = false;
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddldegree.Items.Count > 0)
        {
            int count = 0;
            for (int con = 0; con < ddldegree.Items.Count; con++)
            {
                if (ddldegree.Items[con].Selected)
                {
                    count++;
                }
            }
            txtdegree.Text = lbldegree.Text + "(" + count + ")";
        }
        bindbranch();
        bindsem();
        bindsec();
        bindsubtype();
        bindsubject();
        gviewhomewrk.Visible = false;
    }

    protected void chckdegree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (chckdegree.Checked)
            {
                checkBoxListselectOrDeselect(ddldegree, true);
                CallCheckboxListChange(chckdegree, ddldegree, txtdegree, lbldegree.Text, "--Select--");
            }
            else
            {
                checkBoxListselectOrDeselect(ddldegree, false);
                txtdegree.Text = "--Select--";
            }
            bindbranch();
            bindsem();
            bindsec();
            bindsubtype();
            bindsubject();
            gviewhomewrk.Visible = false;
        }
        catch
        {
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlbranch.Items.Count > 0)
        {
            int count = 0;
            for (int con = 0; con < ddlbranch.Items.Count; con++)
            {
                if (ddlbranch.Items[con].Selected)
                {
                    count++;
                }
            }
            txtbranch.Text = lblbranch.Text + "(" + count + ")";
        }
        bindsem();
        bindsec();
        bindsubtype();
        bindsubject();
        gviewhomewrk.Visible = false;
    }

    protected void chckbranch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (chckbranch.Checked)
            {
                checkBoxListselectOrDeselect(ddlbranch, true);
                CallCheckboxListChange(chckbranch, ddlbranch, txtbranch, lblbranch.Text, "--Select--");
            }
            else
            {
                checkBoxListselectOrDeselect(ddlbranch, false);
                txtbranch.Text = "--Select--";
            }
            bindsem();
            bindsec();
            bindsubtype();
            bindsubject();
            gviewhomewrk.Visible = false;
        }
        catch
        {
        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsec();
        bindsubtype();
        bindsubject();
        gviewhomewrk.Visible = false;
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlsec.Items.Count > 0)
        {
            int count = 0;
            for (int con = 0; con < ddlsec.Items.Count; con++)
            {
                if (ddlsec.Items[con].Selected)
                {
                    count++;
                }
            }
            txtsec.Text = lblsec.Text + "(" + count + ")";
        }
        bindsubtype();
        bindsubject();
        gviewhomewrk.Visible = false;
    }

    protected void chcksec_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (chcksec.Checked)
            {
                checkBoxListselectOrDeselect(ddlsec, true);
                CallCheckboxListChange(chcksec, ddlsec, txtsec, lblsec.Text, "--Select--");
            }
            else
            {
                checkBoxListselectOrDeselect(ddlsec, false);
                txtsec.Text = "--Select--";
            }
            bindsubtype();
            bindsubject();
            gviewhomewrk.Visible = false;
        }
        catch
        {
        }
    }

    #region Subject_Type

    protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cblSubtype.Items.Count > 0)
        {
            int count = 0;
            for (int con = 0; con < cblSubtype.Items.Count; con++)
            {
                if (cblSubtype.Items[con].Selected)
                {
                    count++;
                }
            }
            txtSubtype.Text = lblSuType.Text + "(" + count + ")";
        }
        bindsubject();
    }

    protected void CheckBox1_checkedchange(object sender, EventArgs e)
    {
        if (chkSubtype.Checked)
        {
            checkBoxListselectOrDeselect(cblSubtype, true);
            CallCheckboxListChange(chkSubtype, cblSubtype, txtSubtype, lblSuType.Text, "--Select--");
        }
        else
        {
            checkBoxListselectOrDeselect(cblSubtype, false);
            txtSubtype.Text = "--Select--";
        }
        bindsubject();
    }
    #endregion

    #region Subject

    protected void cblSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cblSubject.Items.Count > 0)
        {
            int count = 0;
            for (int con = 0; con < cblSubject.Items.Count; con++)
            {
                if (cblSubject.Items[con].Selected)
                {
                    count++;
                }
            }
            txtSubject.Text = lblSubject.Text + "(" + count + ")";
        }
    }

    protected void cbSubjet_checkedchange(object sender, EventArgs e)
    {
        if (cbSubjet.Checked)
        {
            checkBoxListselectOrDeselect(cblSubject, true);
            CallCheckboxListChange(cbSubjet, cblSubject, txtSubject, lblSubject.Text, "--Select--");
        }
        else
        {
            checkBoxListselectOrDeselect(cblSubject, false);
            txtSubject.Text = "--Select--";
        }
    }
    #endregion

    #endregion

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
            lbl.Add(lblcollege);
            lbl.Add(lbldegree);
            lbl.Add(lblbranch);
            lbl.Add(lblsem);
            fields.Add(0);
            fields.Add(2);
            fields.Add(3);
            fields.Add(4);
            if (institute != null && institute.TypeInstitute == 1)
            {
                lblbatch.Text = "Year";
            }
            else
            {
                lblbatch.Text = "Batch";
            }
            new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        }
        catch (Exception ex)
        {
        }
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

    protected void txtdate1_TextChanged(object sender, EventArgs e)
    {

        string date = txtdate1.Text;
        string cudate = DateTime.Now.ToString("dd/MM/yyyy"); //DateTime.Now;
        string[] cuspl = cudate.Split('/');
        string[] datespl = date.Split('/');
        int month = Convert.ToInt32(datespl[0]) - Convert.ToInt32(cuspl[0]);
        int day = Convert.ToInt32(datespl[1]) - Convert.ToInt32(cuspl[1]);
        int yer = Convert.ToInt32(datespl[2]) - Convert.ToInt32(cuspl[2]);

        if (day >= 0 && month >= 0 && yer >= 0)
        {
            string degr = string.Empty;
            string bcth = string.Empty;
            string sem = string.Empty;
            string sect = string.Empty;
            string name = lblsubtext.Text;
            string dte = txtdate1.Text;
            string fromdate = txtdate1.Text;
            string[] fdate = fromdate.Split('/');
            // sbjeno = d2.GetFunction("select subject_no from subject where subject_name='" + name + "'");
            if (fdate.Length == 3)
                dte = fdate[2].ToString() + "-" + fdate[1].ToString() + "-" + fdate[0].ToString();
            string sql = "select Homework,PhotoAttachment,FileAttachment,Heading from Home_Work where subjectno='" + Label3.Text + "' and Date='" + dte + "'";
            DataSet dshm = d2.select_method_wo_parameter(sql, "Text");
            if (dshm.Tables.Count > 0 && dshm.Tables[0].Rows.Count > 0)
            {
                for (int s = 0; s < dshm.Tables[0].Rows.Count; s++)
                {
                    string headi = Convert.ToString(dshm.Tables[0].Rows[s]["Heading"]);
                    string hwork = Convert.ToString(dshm.Tables[0].Rows[s]["Homework"]);
                    string photatt = Convert.ToString(dshm.Tables[0].Rows[s]["PhotoAttachment"]);
                    string fileatt = Convert.ToString(dshm.Tables[0].Rows[s]["FileAttachment"]);

                    txtheading.Text = headi;
                    txthomework.Text = hwork;
                    if (!string.IsNullOrEmpty(photatt))
                    { lblshowpic.Text = photatt; lblshowpic.Visible = true; lnkdelpic.Visible = true; fudfile.Visible = true; }
                    if (!string.IsNullOrEmpty(fileatt))
                    { lblshowdoc.Text = fileatt; lblshowdoc.Visible = true; lnkdeldoc.Visible = true; fudattachemntss.Visible = true; }
                    btnsavewrk.Enabled = true;
                    txtheading.Enabled = true;
                    txthomework.Enabled = true;
                    lblshowpic.Enabled = true;
                    lnkdelpic.Enabled = true;
                    lblshowdoc.Enabled = true;
                    lnkdeldoc.Enabled = true;

                    fudfile.Enabled = true;
                    fudattachemntss.Enabled = true;
                }
            }
            else
            {
                btnsavewrk.Enabled = true;
                txtheading.Enabled = true;
                txthomework.Enabled = true;
                lblshowpic.Enabled = true;
                lnkdelpic.Enabled = true;
                lblshowdoc.Enabled = true;
                lnkdeldoc.Enabled = true;
                fudfile.Enabled = true;
                fudattachemntss.Enabled = true;
                txtheading.Text = "";
                txthomework.Text = "";
                lblshowpic.Text = "";
                lnkdelpic.Text = "";
                lblshowdoc.Text = "";
                lnkdeldoc.Text = "";
                lblshowpic.Visible = true; lnkdelpic.Visible = true; fudfile.Visible = true;
                lblshowdoc.Visible = true; lnkdeldoc.Visible = true; fudattachemntss.Visible = true;
            }
        }
        else
        {
            string degr = string.Empty;
            string bcth = string.Empty;
            string sem = string.Empty;
            string sect = string.Empty;
            string dte = txtdate1.Text;
            string fromdate = txtdate1.Text;
            string[] fdate = fromdate.Split('/');
            string name = lblsubtext.Text;
            string sbjeno = d2.GetFunction("select subject_no from subject where subject_name='" + name + "'");
            if (fdate.Length == 3)
                dte = fdate[2].ToString() + "-" + fdate[1].ToString() + "-" + fdate[0].ToString();
            string sql = "select Homework,PhotoAttachment,FileAttachment,Heading from Home_Work where subjectno='" + sbjeno + "' and Date='" + dte + "'";
            DataSet dshm = d2.select_method_wo_parameter(sql, "Text");
            if (dshm.Tables.Count > 0 && dshm.Tables[0].Rows.Count > 0)
            {
                for (int s = 0; s < dshm.Tables[0].Rows.Count; s++)
                {
                    string headi = Convert.ToString(dshm.Tables[0].Rows[s]["Heading"]);
                    string hwork = Convert.ToString(dshm.Tables[0].Rows[s]["Homework"]);
                    string photatt = Convert.ToString(dshm.Tables[0].Rows[s]["PhotoAttachment"]);
                    string fileatt = Convert.ToString(dshm.Tables[0].Rows[s]["FileAttachment"]);

                    txtheading.Text = headi;
                    txthomework.Text = hwork;
                    if (!string.IsNullOrEmpty(photatt))
                    { lblshowpic.Text = photatt; lblshowpic.Visible = true; lnkdelpic.Visible = false; fudfile.Visible = false; }
                    if (!string.IsNullOrEmpty(fileatt))
                    { lblshowdoc.Text = fileatt; lblshowdoc.Visible = true; lnkdeldoc.Visible = false; fudattachemntss.Visible = false; }

                    txtheading.Enabled = false;
                    txthomework.Enabled = false;
                    lblshowpic.Enabled = false;
                    lnkdelpic.Enabled = false;
                    lblshowdoc.Enabled = false;
                    lnkdeldoc.Enabled = false;

                    fudfile.Enabled = false;
                    fudattachemntss.Enabled = false;
                    btnsavewrk.Enabled = false;
                }
            }
            else
            {
                btnsavewrk.Enabled = false;
                txtheading.Enabled = false;
                txthomework.Enabled = false;
                lblshowpic.Enabled = false;
                lnkdelpic.Enabled = false;
                lblshowdoc.Enabled = false;
                lnkdeldoc.Enabled = false;
                fudfile.Enabled = false;
                fudattachemntss.Enabled = false;
                txtheading.Text = "";
                txthomework.Text = "";
                lblshowpic.Text = "";
                lnkdelpic.Text = "";
                lblshowdoc.Text = "";
                lnkdeldoc.Text = "";
                lblshowpic.Visible = true; lnkdelpic.Visible = true; fudfile.Visible = true;
                lblshowdoc.Visible = true; lnkdeldoc.Visible = true; fudattachemntss.Visible = true;
            }
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            dtable.Rows.Clear();
            dtable.Clear();
            string batch = string.Empty;
            string sem = string.Empty;
            string degCode = string.Empty;
            string subCode = string.Empty;
            string SelectQ = string.Empty;
            string colCode = string.Empty;
            string sectt = string.Empty;
            string sectio = string.Empty;

            dtable.Columns.Add("text");
            dtable.Columns.Add("subcode");
            dtable.Columns.Add("subject");
            dtable.Columns.Add("degreedetailtag");

            if (!string.IsNullOrEmpty(ddlsem.SelectedValue))
            {
                sem = ddlsem.SelectedValue.ToString();
            }
            if (!string.IsNullOrEmpty(ddlbatch.SelectedValue))
            {
                batch = ddlbatch.SelectedValue.ToString();
            }
            if (ddlsec.Items.Count > 0)
            {
                for (int i = 0; i < ddlsec.Items.Count; i++)
                {
                    if (ddlsec.Items[i].Selected)
                    {
                        if (sectt == "")
                        {
                            if (string.IsNullOrEmpty(ddlsec.Items[i].Value))
                            {
                                sectt = " ";
                            }
                            else
                            {
                                sectt = ddlsec.Items[i].Value;
                            }
                        }
                        else
                        {
                            sectt = sectt + "','" + ddlsec.Items[i].Value;
                        }
                        sectio = "and sl.sections in('" + sectt + "')";
                    }
                }
            }

            if (ddlbranch.Items.Count > 0)
            {
                for (int deg = 0; deg < ddlbranch.Items.Count; deg++)
                {
                    if (ddlbranch.Items[deg].Selected)
                    {
                        if (degCode == "")
                        {
                            degCode = ddlbranch.Items[deg].Value;
                        }
                        else
                        {
                            degCode = degCode + "','" + ddlbranch.Items[deg].Value;
                        }
                    }
                }
            }
            if (cblSubject.Items.Count > 0)
            {
                for (int sub = 0; sub < cblSubject.Items.Count; sub++)
                {
                    if (cblSubject.Items[sub].Selected)
                    {
                        if (subCode == "")
                        {
                            subCode = cblSubject.Items[sub].Value;
                        }
                        else
                        {
                            subCode = subCode + "','" + cblSubject.Items[sub].Value;
                        }
                    }
                }

            }
            string subje = cblSubject.SelectedItem.Value;
            string subnum = d2.GetFunction("select subject_no from subject where subject_code='" + subje + "'");
            string subtype = string.Empty;
            if (cblSubtype.Items.Count > 0)
                subtype = getCblSelectedText(cblSubtype);

            sectt = d2.GetFunction("select distinct r.Sections from registration r,staff_selector ss,subjectChooser sc where sc.roll_no=r.roll_acr and ss.subject_no=sc.subject_no and sc.subject_no='" + subnum + "'");

            if (!string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(degCode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(subtype))
            {
                string StaffCode = Session["staff_code"].ToString().Trim();
                if (string.IsNullOrEmpty(StaffCode))
                {
                    SelectQ = "select distinct s.subject_code,s.subject_name,CONVERT(nvarchar(max),isnull(sy.Batch_Year,''))+'-'+CONVERT(nvarchar(max),isnull(c.Course_Name,''))+'-'+CONVERT(nvarchar(max),isnull(de.dept_acronym,''))+'-'+CONVERT(nvarchar(max),isnull(sy.semester,''))+'-'+CONVERT(nvarchar(max),isnull(sl.Sections,'')) as text,CONVERT(nvarchar(max),isnull(sy.Batch_Year,''))+'-'+CONVERT(nvarchar(max),isnull(c.Course_Id,''))+'-'+CONVERT(nvarchar(max),isnull(d.Degree_Code,''))+'-'+CONVERT(nvarchar(max),isnull(sy.semester,''))+'-'+CONVERT(nvarchar(max),isnull(sl.Sections,'')) as Valtext from syllabus_master sy,sub_sem ss,subject s,Degree d,course c,Department de,staff_selector sl  where sl.subject_no=s.subject_no and c.Course_Id=d.Course_Id and de.Dept_Code=d.Dept_Code and d.Degree_Code=sy.degree_code and s.syll_code=sy.syll_code and  ss.subType_no=s.subType_no and  sy.Batch_Year='" + batch + "' and sy.degree_code in('" + degCode + "') and sy.semester='" + sem + "' " + sectio + " and ss.subject_type in(" + subtype + ") and s.subject_code in('" + subCode + "') order by CONVERT(nvarchar(max),isnull(sy.Batch_Year,''))+'-'+CONVERT(nvarchar(max),isnull(c.Course_Name,''))+'-'+CONVERT(nvarchar(max),isnull(de.dept_acronym,''))+'-'+CONVERT(nvarchar(max),isnull(sy.semester,''))+'-'+CONVERT(nvarchar(max),isnull(sl.Sections,'')),s.subject_code,s.subject_name";
                }
                else
                {
                    bool staffSelector = false;
                    string qryStudeStaffSelector = string.Empty;
                    string qryStudeStaffSelector1 = string.Empty;
                    if (!string.IsNullOrEmpty(ddlcollege.SelectedValue))
                    {
                        colCode = ddlcollege.SelectedValue;
                    }
                    string minimumabsentsms = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + colCode + "'");
                    string[] splitminimumabsentsms = minimumabsentsms.Split('-');
                    if (splitminimumabsentsms.Length == 2)
                    {
                        int batchyearsetting = 0;
                        int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                        if (splitminimumabsentsms[0].ToString() == "1")
                        {
                            if (Convert.ToInt32(batch.ToString()) >= batchyearsetting)
                            {
                                staffSelector = true;
                            }
                        }
                    }
                    else if (splitminimumabsentsms.Length > 0)
                    {
                        if (splitminimumabsentsms[0].ToString() == "1")
                        {
                            staffSelector = true;
                        }
                    }
                    if (staffSelector)
                    {
                        qryStudeStaffSelector = " and sc.staffcode like '%" + StaffCode + "%'";
                        qryStudeStaffSelector1 = " and sc.staffcode=''" + StaffCode + "''";

                        SelectQ = "select distinct s.subject_code,s.subject_name,CONVERT(nvarchar(max),isnull(sy.Batch_Year,''))+'-'+CONVERT(nvarchar(max),isnull(c.Course_Name,''))+'-'+CONVERT(nvarchar(max),isnull(de.dept_acronym,''))+'-'+CONVERT(nvarchar(max),isnull(sy.semester,''))+'-'+CONVERT(nvarchar(max),isnull(sl.Sections,'')) as text,CONVERT(nvarchar(max),isnull(sy.Batch_Year,''))+'-'+CONVERT(nvarchar(max),isnull(c.Course_Id,''))+'-'+CONVERT(nvarchar(max),isnull(d.Degree_Code,''))+'-'+CONVERT(nvarchar(max),isnull(sy.semester,''))+'-'+CONVERT(nvarchar(max),isnull(sl.Sections,'')) as Valtext from syllabus_master sy,sub_sem ss,subject s,staff_selector sl,Degree d,course c,Department de,subjectChooser sc where sl.subject_no=s.subject_no and c.Course_Id=d.Course_Id and de.Dept_Code=d.Dept_Code and d.Degree_Code=sy.degree_code and s.syll_code=sy.syll_code and  ss.subType_no=s.subType_no and  sy.Batch_Year='" + batch + "' and sy.degree_code in('" + degCode + "') and sy.semester='" + sem + "' " + sectio + " and ss.subject_type in(" + subtype + ") and s.subject_code in('" + subCode + "') " + qryStudeStaffSelector + " order by CONVERT(nvarchar(max),isnull(sy.Batch_Year,''))+'-'+CONVERT(nvarchar(max),isnull(c.Course_Name,''))+'-'+CONVERT(nvarchar(max),isnull(de.dept_acronym,''))+'-'+CONVERT(nvarchar(max),isnull(sy.semester,''))+'-'+CONVERT(nvarchar(max),isnull(sl.Sections,'')),s.subject_code,s.subject_name";
                    }
                    else
                    {
                        SelectQ = "select distinct s.subject_code,s.subject_name,CONVERT(nvarchar(max),isnull(sy.Batch_Year,''))+'-'+CONVERT(nvarchar(max),isnull(c.Course_Name,''))+'-'+CONVERT(nvarchar(max),isnull(de.dept_acronym,''))+'-'+CONVERT(nvarchar(max),isnull(sy.semester,''))+'-'+CONVERT(nvarchar(max),isnull(sl.Sections,'')) as text,CONVERT(nvarchar(max),isnull(sy.Batch_Year,''))+'-'+CONVERT(nvarchar(max),isnull(c.Course_Id,''))+'-'+CONVERT(nvarchar(max),isnull(d.Degree_Code,''))+'-'+CONVERT(nvarchar(max),isnull(sy.semester,''))+'-'+CONVERT(nvarchar(max),isnull(sl.Sections,'')) as Valtext from syllabus_master sy,sub_sem ss,subject s,Degree d,course c,Department de,staff_selector sl where sl.subject_no=s.subject_no and c.Course_Id=d.Course_Id and de.Dept_Code=d.Dept_Code and d.Degree_Code=sy.degree_code and s.syll_code=sy.syll_code and  ss.subType_no=s.subType_no and  sy.Batch_Year='" + batch + "' and sy.degree_code in('" + degCode + "') and sy.semester='" + sem + "' " + sectio + " and ss.subject_type in(" + subtype + ") and s.subject_code in('" + subCode + "') and  sl.staff_code like '%" + StaffCode + "%' order by CONVERT(nvarchar(max),isnull(sy.Batch_Year,''))+'-'+CONVERT(nvarchar(max),isnull(c.Course_Name,''))+'-'+CONVERT(nvarchar(max),isnull(de.dept_acronym,''))+'-'+CONVERT(nvarchar(max),isnull(sy.semester,''))+'-'+CONVERT(nvarchar(max),isnull(sl.Sections,'')),s.subject_code,s.subject_name";
                    }
                }
                ds = d2.select_method_wo_parameter(SelectQ, "Text");


                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int rec = 0; rec < ds.Tables[0].Rows.Count; rec++)
                    {
                        drow = dtable.NewRow();
                        drow["text"] = Convert.ToString(ds.Tables[0].Rows[rec]["text"]);
                        drow["subcode"] = Convert.ToString(ds.Tables[0].Rows[rec]["subject_code"]);
                        drow["subject"] = Convert.ToString(ds.Tables[0].Rows[rec]["subject_name"]);
                        drow["degreedetailtag"] = Convert.ToString(ds.Tables[0].Rows[rec]["Valtext"]);
                        dtable.Rows.Add(drow);
                    }

                    gviewhomewrk.DataSource = dtable;
                    gviewhomewrk.DataBind();
                    gviewhomewrk.Visible = true;
                    //Rowspan
                    int rowcnt1 = gviewhomewrk.Rows.Count - 2;
                    for (int rowIndex = gviewhomewrk.Rows.Count - 2; rowIndex >= 0; rowIndex--)
                    {
                        GridViewRow row = gviewhomewrk.Rows[rowIndex];
                        GridViewRow previousRow = gviewhomewrk.Rows[rowIndex + 1];

                        //for (int i = 0; i < row.Cells.Count; i++)
                        //{
                        Label AccessNo = (Label)gviewhomewrk.Rows[row.RowIndex].FindControl("lbldegr");
                        string degreese = AccessNo.Text.Trim();
                        Label details = (Label)gviewhomewrk.Rows[previousRow.RowIndex].FindControl("lbldegr");
                        string dete = details.Text.Trim();
                        if (degreese == dete)
                        {
                            row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                                   previousRow.Cells[1].RowSpan + 1;
                            previousRow.Cells[1].Visible = false;
                        }

                        //}

                    }

                }
            }
        }
        catch
        {
        }
    }


    protected void btnadd_Onclick(object sender, EventArgs e)
    {
        try
        {
            //  txtdate1.Text = "";
            divEnterHomework.Visible = true;
            Button addbtn = (Button)sender;
            string rowIndxS = addbtn.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            CheckBox chk = (gviewhomewrk.Rows[rowIndx].FindControl("chck") as CheckBox);

            if (chk.Checked)
            {
                string f = "";
                string finalvalue1 = string.Empty;
                for (int i = 0; i < gviewhomewrk.Rows.Count; i++)
                {
                    CheckBox chek = (gviewhomewrk.Rows[i].FindControl("chck") as CheckBox);
                    if (chek.Checked)
                    {
                        Label AccessNo = (Label)gviewhomewrk.Rows[i].FindControl("lbldegr");

                        if (AccessNo.Text.Trim() != "")
                        {
                            f = AccessNo.Text.Trim();

                        }
                        if (finalvalue1 == "")
                        {
                            finalvalue1 = f;
                        }
                        else
                        {
                            finalvalue1 = finalvalue1 + "," + f;

                        }

                    }
                }

                string subtxt = (gviewhomewrk.Rows[rowIndx].FindControl("lblsub") as Label).Text;
                lblsubtext.Text = subtxt;
                btnsavewrk.Visible = true;
                btnupdate.Visible = false;
                btndelete.Visible = false;
                btnsend1.Visible = false;
                txtdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                string degr = string.Empty;
                lblshowpic.Visible = true; lnkdelpic.Visible = false; fudfile.Visible = true;
                lblshowdoc.Visible = true; lnkdeldoc.Visible = false; fudattachemntss.Visible = true;
            }
            else
            {
                divEnterHomework.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Choose the Subject";
            }


        }
        catch
        {
        }
    }

    protected void btnview_Onclick(object sender, EventArgs e)
    {

        div1.Visible = true;
        divhme.Visible = true;
        string degreedeta1 = string.Empty;

        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        Button addbtn = (Button)sender;
        string rowIndxS = addbtn.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxS) - 2;
        Label degdet = (Label)gviewhomewrk.Rows[rowIndx].FindControl("lbldegdetail");
        if (degdet.Text.Trim() != "")
        {
            degreedeta1 = degdet.Text.Trim();

        }
        ss = degreedeta1;

        sunum = (gviewhomewrk.Rows[rowIndx].FindControl("lblsubcde") as Label).Text;
        Label4.Text = sunum;
        gviewhme.Visible = true;
        divCounsellingReport.Visible = true;
        btngo1_Click(sender, e);
    }

    protected void lnkDownloadpic_click(object sender, EventArgs e)
    {
        try
        {
            string activerow = string.Empty;
            string activecol = string.Empty;


            LinkButton lnkSelected = (LinkButton)sender;
            string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            string iduni = (gviewhme.Rows[rowIndx].FindControl("lbluniq") as Label).Text;
            int colIndx = 5;
            DataSet dspicture = new DataSet();

            activerow = rowIndx.ToString();
            activecol = colIndx.ToString();

            if (Convert.ToInt32(activecol) == 5)
            {
                string qrys = "select PhotoAttachment,PhotoContentType,PhotoData from Home_Work where idno='" + iduni + "'";
                dspicture.Clear();
                dspicture = d2.select_method_wo_parameter(qrys, "Text");
                if (dspicture.Tables.Count > 0 && dspicture.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dspicture.Tables[0].Rows.Count; i++)
                    {
                        Response.ContentType = dspicture.Tables[0].Rows[i]["PhotoContentType"].ToString();
                        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dspicture.Tables[0].Rows[i]["PhotoAttachment"] + "\"");
                        Response.BinaryWrite((byte[])dspicture.Tables[0].Rows[i]["PhotoData"]);
                        Response.End();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //  d2.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }

    protected void lnkDownloadfile_click(object sender, EventArgs e)
    {
        try
        {
            string activerow = string.Empty;
            string activecol = string.Empty;


            LinkButton lnkSelected = (LinkButton)sender;
            string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            string iduni = (gviewhme.Rows[rowIndx].FindControl("lbluniq") as Label).Text;
            int colIndx = 6;
            DataSet dspicture1 = new DataSet();

            activerow = rowIndx.ToString();
            activecol = colIndx.ToString();

            if (Convert.ToInt32(activecol) == 6)
            {
                string qrys = "select FileAttachment,FileContentType,FileData from Home_Work where idno='" + iduni + "'";
                dspicture1.Clear();
                dspicture1 = d2.select_method_wo_parameter(qrys, "Text");
                if (dspicture1.Tables.Count > 0 && dspicture1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dspicture1.Tables[0].Rows.Count; i++)
                    {
                        Response.ContentType = dspicture1.Tables[0].Rows[i]["FileContentType"].ToString();
                        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dspicture1.Tables[0].Rows[i]["FileAttachment"] + "\"");
                        Response.BinaryWrite((byte[])dspicture1.Tables[0].Rows[i]["FileData"]);
                        Response.End();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //  d2.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;

        }
        catch (Exception ex)
        { }
    }

    protected void gviewhme_onRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count - 2; i++)
            {
                //if (i == 1)
                //{
                //    e.Row.Cells[5].ForeColor = Color.Black;
                //    e.Row.Cells[6].ForeColor = Color.Black;
                //}

                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void gviewhme_selectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            div1.Visible = false;
            btnsavewrk.Visible = false;


            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            lblshowpic.Visible = false;
            lnkdelpic.Visible = false;
            lblshowdoc.Visible = false;
            lnkdeldoc.Visible = false;
            fudfile.Visible = true;
            fudattachemntss.Visible = true;
            lblattachements.Visible = true;
            string subject = (gviewhme.Rows[rowIndex].FindControl("lblsubject") as Label).Text;
            string date = (gviewhme.Rows[rowIndex].FindControl("lbldate") as Label).Text;
            string unid = (gviewhme.Rows[rowIndex].FindControl("lbluniq") as Label).Text;
            string topic = (gviewhme.Rows[rowIndex].FindControl("lbltopic") as Label).Text;
            string picattach = (gviewhme.Rows[rowIndex].FindControl("lnkdownloadpic") as LinkButton).Text;
            string fileattach = (gviewhme.Rows[rowIndex].FindControl("lnkdownloadfile") as LinkButton).Text;
            string head = (gviewhme.Rows[rowIndex].FindControl("lblhead") as Label).Text;

            divEnterHomework.Visible = true;
            lblsubtext.Text = subject;
            lbldel.Text = unid;
            txtheading.Text = head;
            txthomework.Text = topic;
            txtdate1.Text = date;
            btnupdate.Visible = true;
            btndelete.Visible = true;
            btnsend1.Visible = true;

            if (!string.IsNullOrEmpty(picattach))
            { lblshowpic.Text = picattach; lblshowpic.Visible = true; lnkdelpic.Visible = true; fudfile.Visible = false; }
            if (!string.IsNullOrEmpty(fileattach))
            { lblshowdoc.Text = fileattach; lblshowdoc.Visible = true; lnkdeldoc.Visible = true; fudattachemntss.Visible = false; }

            
            if ((gviewhme.Rows[rowIndex].FindControl("lbltype") as Label).Text == "Send")
            {
                divEnterHomework.Visible = false;
                divhme.Visible = true;
                div1.Visible = true;
                divCounsellingReport.Visible = true;
            }
          

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }

    protected void btnsavewrk_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean pic = false;
            Boolean file = false;

            if (string.IsNullOrEmpty(txtheading.Text))
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Give some heading for Home Work";

                return;
            }
            if (string.IsNullOrEmpty(txthomework.Text))
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Write some work for students";

                return;
            }
            if (fudfile.HasFile)
            {
                if (fudfile.FileName.EndsWith(".jpg") || fudfile.FileName.EndsWith(".gif") || fudfile.FileName.EndsWith(".png") || fudfile.FileName.EndsWith(".jpeg"))
                { pic = true; }
                else
                {
                    pic = false;
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "The file you selected is not a valid image file. Please select another file";
                    return;
                }
            }
            if (fudattachemntss.HasFile)
            {
                if (fudattachemntss.FileName.EndsWith(".txt") || fudattachemntss.FileName.EndsWith(".doc") || fudattachemntss.FileName.EndsWith(".xls") || fudattachemntss.FileName.EndsWith(".docx") || fudattachemntss.FileName.EndsWith(".txt") || fudattachemntss.FileName.EndsWith(".document") || fudattachemntss.FileName.EndsWith(".xls") || fudattachemntss.FileName.EndsWith(".xlsx") || fudattachemntss.FileName.EndsWith(".pdf") || fudattachemntss.FileName.EndsWith(".ppt") || fudattachemntss.FileName.EndsWith(".pptx"))
                { file = true; }
                else
                {
                    file = false;
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "The file you selected is not a valid file. Please select another file";

                    return;
                }
            }
            homeworksave();

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }

    protected void homeworksave()
    {
        try
        {
            byte[] picbinary = new byte[0];
            string fileName = string.Empty;
            string fileExtension = string.Empty;
            string documentType = string.Empty;
            int picsize = 0;
            string degreedeta = string.Empty;
            byte[] filebinary = new byte[0];
            string fileName1 = string.Empty;
            string fileExtension1 = string.Empty;
            string documentType1 = string.Empty;
            int filesize = 0;
            string dte = string.Empty;
            string ddlsub = string.Empty;
            string f = string.Empty;
            string ddlcodee = string.Empty;
            string degre = string.Empty;
            string bth = string.Empty;

            string section = string.Empty;
            string heading = string.Empty;

            string hmewrk = txthomework.Text;
            heading = txtheading.Text;
            string update = lblsubtext.Text;
            string idno = lbldel.Text;
            //section = ddlsec.SelectedItem.Text;
            string fromDate = txtdate1.Text;
            string[] fromdate = fromDate.Split('/');
            if (fromdate.Length == 3)
                dte = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();

            string finalvalue1 = string.Empty;
            string sectionVal = string.Empty;
            string sect = string.Empty;

            for (int i = 0; i < gviewhomewrk.Rows.Count; i++)
            {
                CheckBox chek = (gviewhomewrk.Rows[i].FindControl("chck") as CheckBox);
                if (chek.Checked)
                {
                    Label AccessNo = (Label)gviewhomewrk.Rows[i].FindControl("lbldegr");
                    string degVal = (gviewhomewrk.Rows[i].FindControl("lbldegdetail") as Label).Text;
                    string[] degspl = degVal.Split('-');
                    if (AccessNo.Text.Trim() != "")
                    {
                        f = AccessNo.Text.Trim();

                    }
                    Label subcode = (Label)gviewhomewrk.Rows[i].FindControl("lblsubcde");
                    if (subcode.Text.Trim() != "")
                    {
                        subjectco = subcode.Text.Trim();

                    }
                    Label degdet = (Label)gviewhomewrk.Rows[i].FindControl("lbldegdetail");
                    if (degdet.Text.Trim() != "")
                    {
                        degreedeta = degdet.Text.Trim();

                    }
                    if (finalvalue1 == "")
                    {
                        finalvalue1 = f;
                    }
                    else
                    {
                        finalvalue1 = finalvalue1 + "," + f;

                    }
                    if (degspl.Length == 5)
                    {
                        if (string.IsNullOrEmpty(sectionVal))
                        {
                            sectionVal = degspl[4];
                        }
                        else
                        {
                            sectionVal = sectionVal + "','" + degspl[4];
                        }
                        sect = "and section in('" + sectionVal + "')";
                    }


                }

            }
            int result = 0;

            DateTime dtAttendanceDate1 = new DateTime();
            DateTime.TryParseExact(Convert.ToString(dte), "dd/MM/yyyy", null, DateTimeStyles.None, out dtAttendanceDate1);
            if (fudfile.HasFile)
            {
                bool FileFromat = false;
                FileFromat = FileTypeCheck(fudfile, ref fileName, ref fileExtension, ref documentType);
                picsize = fudfile.PostedFile.ContentLength;
                picbinary = new byte[picsize];
                fudfile.PostedFile.InputStream.Read(picbinary, 0, picsize);//string datetime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            }
            if (fudattachemntss.HasFile)
            {
                bool FileFromat1 = false;
                FileFromat1 = FileTypeCheck1(fudattachemntss, ref fileName1, ref fileExtension1, ref documentType1);
                filesize = fudattachemntss.PostedFile.ContentLength;
                filebinary = new byte[filesize];
                fudattachemntss.PostedFile.InputStream.Read(filebinary, 0, filesize);
            }



            string[] sp = degreedeta.Split('-');
            if (sp.Length == 5)
            {
                degre = sp[2].ToString();
                bth = sp[0].ToString();
                seme = sp[3].ToString();

            }



            ddlsub = lblsubtext.Text;
            string subssno = d2.GetFunction("select s.subject_no from  subject s,syllabus_master sy where s.syll_code=sy.syll_code and s.subject_code in('" + subjectco + "') and degree_code in('" + degre + "') and Batch_Year in('" + bth + "') and semester in('" + seme + "')");
            subno = subssno;



            string hmewrok = d2.GetFunction("select subjectno from Home_Work where Date='" + dte + "'  " + sect + " ");
            if (hmewrok == subno)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "For this Subject Homework is already saved";
                return;
            }


            if (finalvalue1 != "")
            {
                string[] subsplit = finalvalue1.Split(',');
                if (subsplit.Length > 0)
                {
                    for (int m = 0; m < subsplit.Length; m++)
                    {
                        string details = subsplit[m].ToString();
                        string[] spit1 = details.Split('-');
                        section = spit1[4].ToString();


                        SqlCommand cmdnotes = new SqlCommand();

                        if (picsize == 0 && filesize == 0)
                        {
                            cmdnotes.CommandText = " insert into Home_Work(subjectno,Date,Homework,Section,PhotoAttachment,PhotoContentType,PhotoData,FileAttachment,FileContentType,FileData,Heading)" + " VALUES (@subjno,@date,@hmewrk,@section,@picname,@pictype,@picdata,@filename,@filetype,@filedata,@head)";
                        }
                        else if (picsize != 0 && filesize != 0)
                        {
                            cmdnotes.CommandText = " insert into Home_Work(subjectno,Date,Homework,Section,PhotoAttachment,PhotoContentType,PhotoData,FileAttachment,FileContentType,FileData,Heading)" + " VALUES (@subjno,@date,@hmewrk,@section,@picname,@pictype,@picdata,@filename,@filetype,@filedata,@head)";
                        }
                        else if (picsize != 0 && filesize == 0)
                        {
                            cmdnotes.CommandText = " insert into Home_Work(subjectno,Date,Homework,Section,PhotoAttachment,PhotoContentType,PhotoData,FileAttachment,FileContentType,FileData,Heading)" + " VALUES (@subjno,@date,@hmewrk,@section,@picname,@pictype,@picdata,@filename,@filetype,@filedata,@head)";
                        }
                        else if (picsize == 0 && filesize != 0)
                        {
                            cmdnotes.CommandText = " insert into Home_Work(subjectno,Date,Homework,Section,PhotoAttachment,PhotoContentType,PhotoData,FileAttachment,FileContentType,FileData,Heading)" + " VALUES (@subjno,@date,@hmewrk,@section,@picname,@pictype,@picdata,@filename,@filetype,@filedata,@head)";
                        }

                        cmdnotes.CommandType = CommandType.Text;
                        cmdnotes.Connection = ssql;
                        SqlParameter subjno = new SqlParameter("@subjno", SqlDbType.Int, 100);
                        subjno.Value = Convert.ToString(subno);
                        cmdnotes.Parameters.Add(subjno);

                        SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 50);
                        uploadedDate.Value = dte;
                        cmdnotes.Parameters.Add(uploadedDate);

                        SqlParameter homewrk = new SqlParameter("@hmewrk", SqlDbType.NVarChar, 1500);
                        homewrk.Value = hmewrk.ToString();
                        cmdnotes.Parameters.Add(homewrk);

                        SqlParameter sec = new SqlParameter("@section", SqlDbType.NVarChar, 100);
                        sec.Value = section.ToString();
                        cmdnotes.Parameters.Add(sec);

                        SqlParameter picnme = new SqlParameter("@picname", SqlDbType.NVarChar, 100);
                        picnme.Value = fileName.ToString();
                        cmdnotes.Parameters.Add(picnme);

                        SqlParameter pictype = new SqlParameter("@pictype", SqlDbType.NVarChar, 100);
                        pictype.Value = documentType.ToString();
                        cmdnotes.Parameters.Add(pictype);

                        SqlParameter picdata = new SqlParameter("@picdata", SqlDbType.Binary, picsize);
                        picdata.Value = picbinary;
                        cmdnotes.Parameters.Add(picdata);

                        SqlParameter filenme = new SqlParameter("@filename", SqlDbType.NVarChar, 100);
                        filenme.Value = fileName1.ToString();
                        cmdnotes.Parameters.Add(filenme);

                        SqlParameter filetype = new SqlParameter("@filetype", SqlDbType.NVarChar, 100);
                        filetype.Value = documentType1.ToString();
                        cmdnotes.Parameters.Add(filetype);

                        SqlParameter filedata = new SqlParameter("@filedata", SqlDbType.Binary, filesize);
                        filedata.Value = filebinary;
                        cmdnotes.Parameters.Add(filedata);

                        SqlParameter head = new SqlParameter("@head", SqlDbType.NVarChar, 100);
                        head.Value = heading.ToString();
                        cmdnotes.Parameters.Add(head);


                        ssql.Close();
                        ssql.Open();
                        result = cmdnotes.ExecuteNonQuery();
                    }
                }

            }
            if (result > 0)
            {

                lbldel.Text = "";
                lbldel.Text = "";
                txtheading.Text = "";
                txthomework.Text = "";
                lblshowpic.Visible = false;
                lnkdelpic.Visible = false;
                lblshowdoc.Visible = false;
                lnkdeldoc.Visible = false;

                fudfile.Visible = true;
                fudattachemntss.Visible = true;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Saved Successfully";


            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Not Saved Successfully";
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            btnupdate.Visible = true;
            btndelete.Visible = true;
            string ddlcodee = string.Empty;
            string dte = string.Empty;
            string degre = ddlbranch.SelectedItem.Value;
            string seme = ddlsem.SelectedItem.Value;
            string bth = ddlbatch.SelectedItem.Value;
            byte[] picbinary = new byte[0];
            string fileName = string.Empty;
            string fileExtension = string.Empty;
            string documentType = string.Empty;
            int picsize = 0;

            byte[] filebinary = new byte[0];
            string fileName1 = string.Empty;
            string fileExtension1 = string.Empty;
            string documentType1 = string.Empty;
            int filesize = 0;

            string ddlsub = string.Empty;
            string subno = string.Empty;
            string section = string.Empty;
            string heading = string.Empty;

            string hmewrk = txthomework.Text;
            heading = txtheading.Text;
            string update = lblsubtext.Text;
            string idno = lbldel.Text;

            string fromDate = txtdate1.Text;
            string[] fromdate = fromDate.Split('/');
            if (fromdate.Length == 3)
                dte = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();

            DateTime dtAttendanceDate1 = new DateTime();
            DateTime.TryParseExact(Convert.ToString(dte), "d-MM-yyyy", null, DateTimeStyles.None, out dtAttendanceDate1);
            if (fudfile.HasFile)
            {
                bool FileFromat = false;
                FileFromat = FileTypeCheck(fudfile, ref fileName, ref fileExtension, ref documentType);
                picsize = fudfile.PostedFile.ContentLength;
                picbinary = new byte[picsize];
                fudfile.PostedFile.InputStream.Read(picbinary, 0, picsize);//string datetime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            }
            if (fudattachemntss.HasFile)
            {
                bool FileFromat1 = false;
                FileFromat1 = FileTypeCheck1(fudattachemntss, ref fileName1, ref fileExtension1, ref documentType1);
                filesize = fudattachemntss.PostedFile.ContentLength;
                filebinary = new byte[filesize];
                fudattachemntss.PostedFile.InputStream.Read(filebinary, 0, filesize);
            }

            ddlsub = lblsubtext.Text;
            //ddlcodee = Label3.Text;
            //string subssno = d2.GetFunction("select s.subject_no from  subject s,syllabus_master sy where s.syll_code=sy.syll_code and s.subject_code in('" + ddlcodee + "') and degree_code in('" + degre + "') and Batch_Year in('" + bth + "') and semester in('" + seme + "')");
            subno = Label3.Text;
            // subno = sbjeno;
            SqlCommand cmdnotes = new SqlCommand();

            if (picsize == 0 && filesize == 0)
            {
                cmdnotes.CommandText = "if exists(select * from Home_Work where idno='" + idno + "') update Home_Work set Homework=@hmewrk,Heading=@head where idno='" + idno + "'  else insert into Home_Work(subjectno,Date,Homework,Section,PhotoAttachment,PhotoContentType,PhotoData,FileAttachment,FileContentType,FileData,Heading)" + " VALUES (@subjno,@date,@hmewrk,@section,@picname,@pictype,@picdata,@filename,@filetype,@filedata,@head)";
            }
            else if (picsize != 0 && filesize != 0)
            {
                cmdnotes.CommandText = "if exists(select * from Home_Work where idno='" + idno + "') update Home_Work set Homework=@hmewrk,Heading=@head,PhotoAttachment=@picname,PhotoContentType=@pictype,PhotoData=@picdata,FileAttachment=@filename,FileContentType=@filetype,FileData=@filedata where idno='" + idno + "'  else insert into Home_Work(subjectno,Date,Homework,Section,PhotoAttachment,PhotoContentType,PhotoData,FileAttachment,FileContentType,FileData,Heading)" + " VALUES (@subjno,@date,@hmewrk,@section,@picname,@pictype,@picdata,@filename,@filetype,@filedata,@head)";
            }
            else if (picsize != 0 && filesize == 0)
            {
                cmdnotes.CommandText = "if exists(select * from Home_Work where idno='" + idno + "') update Home_Work set Homework=@hmewrk,Heading=@head,PhotoAttachment=@picname,PhotoContentType=@pictype,PhotoData=@picdata where idno='" + idno + "'  else insert into Home_Work(subjectno,Date,Homework,Section,PhotoAttachment,PhotoContentType,PhotoData,FileAttachment,FileContentType,FileData,Heading)" + " VALUES (@subjno,@date,@hmewrk,@section,@picname,@pictype,@picdata,@filename,@filetype,@filedata,@head)";
            }
            else if (picsize == 0 && filesize != 0)
            {
                cmdnotes.CommandText = "if exists(select * from Home_Work where idno='" + idno + "') update Home_Work set Homework=@hmewrk,Heading=@head,FileAttachment=@filename,FileContentType=@filetype,FileData=@filedata where idno='" + idno + "'  else insert into Home_Work(subjectno,Date,Homework,Section,PhotoAttachment,PhotoContentType,PhotoData,FileAttachment,FileContentType,FileData,Heading)" + " VALUES (@subjno,@date,@hmewrk,@section,@picname,@pictype,@picdata,@filename,@filetype,@filedata,@head)";
            }

            cmdnotes.CommandType = CommandType.Text;
            cmdnotes.Connection = ssql;

            SqlParameter subjno = new SqlParameter("@subjno", SqlDbType.Int, 100);
            subjno.Value = Convert.ToInt32(subno);
            cmdnotes.Parameters.Add(subjno);

            SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 50);
            uploadedDate.Value = dte;
            cmdnotes.Parameters.Add(uploadedDate);

            SqlParameter homewrk = new SqlParameter("@hmewrk", SqlDbType.NVarChar, 1500);
            homewrk.Value = hmewrk.ToString();
            cmdnotes.Parameters.Add(homewrk);

            SqlParameter sec = new SqlParameter("@section", SqlDbType.NVarChar, 100);
            sec.Value = section.ToString();
            cmdnotes.Parameters.Add(sec);

            SqlParameter picnme = new SqlParameter("@picname", SqlDbType.NVarChar, 100);
            picnme.Value = fileName.ToString();
            cmdnotes.Parameters.Add(picnme);

            SqlParameter pictype = new SqlParameter("@pictype", SqlDbType.NVarChar, 100);
            pictype.Value = documentType.ToString();
            cmdnotes.Parameters.Add(pictype);

            SqlParameter picdata = new SqlParameter("@picdata", SqlDbType.Binary, picsize);
            picdata.Value = picbinary;
            cmdnotes.Parameters.Add(picdata);

            SqlParameter filenme = new SqlParameter("@filename", SqlDbType.NVarChar, 100);
            filenme.Value = fileName1.ToString();
            cmdnotes.Parameters.Add(filenme);

            SqlParameter filetype = new SqlParameter("@filetype", SqlDbType.NVarChar, 100);
            filetype.Value = documentType1.ToString();
            cmdnotes.Parameters.Add(filetype);

            SqlParameter filedata = new SqlParameter("@filedata", SqlDbType.Binary, filesize);
            filedata.Value = filebinary;
            cmdnotes.Parameters.Add(filedata);

            SqlParameter head = new SqlParameter("@head", SqlDbType.NVarChar, 100);
            head.Value = heading.ToString();
            cmdnotes.Parameters.Add(head);

            ssql.Close();
            ssql.Open();
            int result = cmdnotes.ExecuteNonQuery();
            if (result > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Updated Successfully";
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Not Updated Successfully";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void lnlremovepic(object sender, EventArgs e)
    {
        hat1.Clear();
        string idnum = lbldel.Text;
        string delpic = "update Home_Work set PhotoAttachment='',PhotoContentType='',PhotoData=0 where idno='" + idnum + "'";
        int k = d2.insert_method(delpic, hat1, "Text");
        if (k > 0)
        {
            lblshowpic.Visible = false;
            lnkdelpic.Visible = false;
            lblfile.Visible = true;
            fudfile.Visible = true;

        }
    }

    protected void lnlremovedoc(object sender, EventArgs e)
    {
        hat1.Clear();
        string idnum = lbldel.Text;
        string delpic = "update Home_Work set FileAttachment='',FileContentType='',FileData=0 where idno='" + idnum + "'";
        int k = d2.insert_method(delpic, hat1, "Text");
        if (k > 0)
        {
            lblshowdoc.Visible = false;
            lnkdeldoc.Visible = false;
            lblattachements.Visible = true;
            fudattachemntss.Visible = true;

        }
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {

        hat1.Clear();
        string unique = lbldel.Text;
        string qurys = "delete from Home_Work where idno='" + unique + "'";
        int delet = d2.insert_method(qurys, hat1, "Text");
        if (delet > 0)
        {
            lbldel.Text = "";
            txtheading.Text = "";
            txthomework.Text = "";
            fudfile.Visible = true;
            fudattachemntss.Visible = true;
            lblshowpic.Visible = false;
            lnkdelpic.Visible = false;

            alertpopwindow.Visible = true;
            lblalerterr.Text = "Deleted Successfully";
        }
        else
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Not Deleted Successfully";
        }
    }

    protected void btnsend_Click(object sender, EventArgs e)
    {
        try
        {
            btnupdate.Enabled = false;
            btndelete.Enabled = false;
            string dte = string.Empty;
            string degreedeta1 = string.Empty;
            string f1 = string.Empty;
            string degr = string.Empty;
            string sect = string.Empty;
            string bcth = string.Empty;
            string sem = string.Empty;
            string Send = string.Empty;
            string fromDate = txtdate1.Text;
            string[] fromdate = fromDate.Split('/');
            string[] sp = ss.Split('-');
            for (int i = 0; i < gviewhomewrk.Rows.Count; i++)
            {
                Label AccessNo = (Label)gviewhomewrk.Rows[i].FindControl("lbldegr");
                string degVal = (gviewhomewrk.Rows[i].FindControl("lbldegdetail") as Label).Text;
                string[] degspl = degVal.Split('-');
                if (AccessNo.Text.Trim() != "")
                {
                    f1 = AccessNo.Text.Trim();

                }
                Label subcode = (Label)gviewhomewrk.Rows[i].FindControl("lblsubcde");
                if (subcode.Text.Trim() != "")
                {
                    subjectco = subcode.Text.Trim();

                }
                Label degdet = (Label)gviewhomewrk.Rows[i].FindControl("lbldegdetail");
                if (degdet.Text.Trim() != "")
                {
                    degreedeta1 = degdet.Text.Trim();


                }

            }
            string[] sp1 = degreedeta1.Split('-');
            if (sp1.Length == 5)
            {
                degr = sp1[2].ToString();
                bcth = sp1[0].ToString();
                seme = sp1[3].ToString();
               
            }
            if (fromdate.Length == 3)
                dte = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();
            DataSet dsset = d2.select_method_wo_parameter("select r.App_No [app_no] from subjectChooser sc,Registration r where sc.roll_no=r.Roll_No and semester='" + seme + "' and subject_no='" + Label3.Text + "'", "Text");
            idnos = d2.GetFunction("select idno from home_work where subjectno=" + Label3.Text + " and Date='" + dte + "' order by idno desc");

            string hmewrok1 = d2.GetFunction("select homework_id from stud_homework_status where date='" + dte + "' ");
            if (hmewrok1 == idnos)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "For this Subject Homework is already Send";
                return;
            }
            if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsset.Tables[0].Rows.Count; i++)
                {
                    string appnum = Convert.ToString(dsset.Tables[0].Rows[i]["app_no"]);


                    String qry = "if exists(select * from  stud_homework_status where app_no='" + appnum + "' and homework_id=" + idnos + " and date='" + dte + "')update stud_homework_status set app_no='" + appnum + "' , homework_id=" + idnos + " , date='" + dte + "' where app_no='" + appnum + "' and homework_id='" + idnos + "' and date='" + dte + "' else insert into stud_homework_status  values('" + appnum + "'," + idnos + ",'" + dte + "',1,0)";
                    int x = d2.update_method_wo_parameter(qry, "Text");
                    string token = d2.GetFunction("select fcm_token from Registration where app_no='" + appnum + "'");
                    if (token != "")
                    {
                        if (x == 1)
                        {

                            heading = "Student Login" + "~" + appnum + "~" + idnos + "~" + dte;
                            ns.SendMessage(token, heading, txthomework.Text);
                        }
                    }

                }
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Send Successfully";
                btnsend1.Enabled = false;
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Not Send Successfully";
            }


        }
        catch (Exception ex)
        {
        }
    }

    protected void btngo1_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dssett = new DataSet();
            DataTable dtview = new DataTable();
            DataRow drhme;
            dicRowColor.Clear();
            string fromdte = string.Empty;
            string todte = string.Empty;
            string sect = string.Empty;
            string degreedeta1 = string.Empty;
            string degr = string.Empty;
            string bcth = string.Empty;
            string sem = string.Empty;
            string Send = string.Empty;
            dtview.Columns.Add("SNo");
            dtview.Columns.Add("uniqid");
            dtview.Columns.Add("Date");
            dtview.Columns.Add("Type");
            dtview.Columns.Add("Subject");
            dtview.Columns.Add("Subjectno");
            dtview.Columns.Add("Heading");
            dtview.Columns.Add("Topic");
            dtview.Columns.Add("Photo");
            dtview.Columns.Add("Attachment");

            //drhme = dtview.NewRow();
            //drhme["SNo"] = "SNo";
            //drhme["uniqid"] = "uniqid";
            //drhme["Date"] = "Date";
            //drhme["Subject"] = "Subject";
            //drhme["Subjectno"] = "Subjectno";
            //drhme["Heading"] = "Heading";
            //drhme["Topic"] = "Topic";
            //drhme["Photo"] = "Photo";
            //drhme["Attachment"] = "Attachment";
            //dtview.Rows.Add(drhme);

            string[] sp = ss.Split('-');
            if (sp.Length == 5)
            {
                degr = sp[2].ToString();
                bcth = sp[0].ToString();
                sem = sp[3].ToString();
                sect = sp[4].ToString();
            }

            int rowCnt = 0;
            string frmdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] fromdate = frmdate.Split('/');
            string[] tdate = todate.Split('/');
            if (fromdate.Length == 3)
                fromdte = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();
            if (tdate.Length == 3)
                todte = tdate[2].ToString() + "-" + tdate[1].ToString() + "-" + tdate[0].ToString();

            string subjectnoo = d2.GetFunction(" select s.subject_no from  subject s,syllabus_master sy where s.syll_code=sy.syll_code and s.subject_code in('" + sunum + "') and degree_code in('" + degr + "') and Batch_Year in('" + bcth + "') and semester in('" + sem + "')");

            Label3.Text = subjectnoo;
            if (string.IsNullOrEmpty(sunum))
            {
                dssett.Clear();

                dssett = d2.select_method_wo_parameter("select distinct CONVERT(varchar(20),HW.Date,103) Date,Heading,Homework,PhotoAttachment,FileAttachment,subjectno,HW.idno ,isnull((select distinct delivered from stud_homework_status SS where SS.homework_id=HW.idno ),0) as sendtype  from Home_Work HW  where  HW.Date between '" + fromdte + "'and '" + todte + "'", "Text");
            }
            else
            {
                if (string.IsNullOrEmpty(sect))
                {
                    dssett.Clear();
                    dssett = d2.select_method_wo_parameter("select distinct CONVERT(varchar(20),HW.Date,103) Date,Heading,Homework,PhotoAttachment,FileAttachment,subjectno,HW.idno ,isnull((select distinct delivered from stud_homework_status SS where SS.homework_id=HW.idno ),0) as sendtype  from Home_Work HW  where  subjectno='" + subjectnoo + "' and HW.Date between '" + fromdte + "'and '" + todte + "'", "Text");
                }
                else
                {
                    dssett.Clear();

                    dssett = d2.select_method_wo_parameter("select distinct CONVERT(varchar(20),HW.Date,103) Date,Heading,Homework,PhotoAttachment,FileAttachment,subjectno,HW.idno ,isnull((select distinct delivered from stud_homework_status SS where SS.homework_id=HW.idno ),0) as sendtype  from Home_Work HW where subjectno='" + subjectnoo + "' and HW.Date between '" + fromdte + "'and '" + todte + "'  ", "Text");
                }
            }

            if (dssett.Tables.Count > 0 && dssett.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                for (int i = 0; i < dssett.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    drhme = dtview.NewRow();

                    string uniq = Convert.ToString(dssett.Tables[0].Rows[i]["idno"]);
                    string date1 = Convert.ToString(dssett.Tables[0].Rows[i]["Date"]);
                    string typ = Convert.ToString(dssett.Tables[0].Rows[i]["sendtype"]);
                    if (typ == "0")
                    {
                        typ = "Not Send";
                    }
                    else if (typ == "1")
                    {
                        typ = "Send";
                    }
                    string subjectno = Convert.ToString(dssett.Tables[0].Rows[i]["subjectno"]);
                    string subjectname = d2.GetFunction("Select Subject_Name from subject where subject_no='" + subjectno + "'");
                    string headng = Convert.ToString(dssett.Tables[0].Rows[i]["Heading"]);
                    string topic = Convert.ToString(dssett.Tables[0].Rows[i]["Homework"]);
                    string photo = Convert.ToString(dssett.Tables[0].Rows[i]["PhotoAttachment"]);
                    string file = Convert.ToString(dssett.Tables[0].Rows[i]["FileAttachment"]);

                    drhme["SNo"] = sno.ToString();
                    drhme["uniqid"] = uniq;
                    drhme["Type"] = typ;
                    if (typ == "Not Send")
                    {
                        dicRowColor.Add(rowCnt, "Not Send");
                    }
                    else if (typ == "Send")
                    {
                        dicRowColor.Add(rowCnt, "Send");
                    }

                    drhme["Date"] = date1;
                    drhme["Subject"] = subjectname;
                    drhme["Subjectno"] = subjectno;
                    drhme["Heading"] = headng;
                    drhme["Topic"] = topic;
                    drhme["Photo"] = photo;
                    drhme["Attachment"] = file;

                    dtview.Rows.Add(drhme);
                    rowCnt++;
                }
                divCounsellingReport.Visible = true;
                gviewhme.DataSource = dtview;
                gviewhme.DataBind();
                //RowHead(gviewhme);
                gviewhme.Visible = true;
                divhme.Visible = true;
                btnPrint1.Visible = true;
                foreach (KeyValuePair<int, string> dr in dicRowColor)
                {
                   
                    int RowVal = dr.Key;
                    string DicValue = dr.Value;
                    if (DicValue == "Send")
                    {
                        gviewhme.Rows[RowVal].BackColor = Color.LightGreen;
                     
                    }
                }
            }
            else
            {
                div1.Visible = true;
                gviewhme.Visible = true;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "There is no Homework for this subject";
            }
        }
        catch (Exception)
        {
        }

    }

    //protected void RowHead(GridView gviewhme)
    //{
    //    for (int head = 0; head < 1; head++)
    //    {
    //        gviewhme.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
    //        gviewhme.Rows[head].Font.Bold = true;
    //        gviewhme.Rows[head].HorizontalAlign = HorizontalAlign.Center;

    //    }
    //}

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {

            string degreedetails;
            string pagename;
            degreedetails = "CommonHomeWork";
            pagename = "CommonHomeWork";
            string ss = Session["usercode"].ToString();
            Printcontrol.loadspreaddetails(gviewhme, pagename, degreedetails, 0, ss);
            Printcontrol.Visible = true;
        }
        catch (Exception ex) { }
    }



    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btnclosespread1_OnClick(object sender, EventArgs e)
    {
        divEnterHomework.Visible = false;
        lblsubtext.Text = "";
        txtheading.Text = "";
        txthomework.Text = "";
        lblshowpic.Text = "";
        lblshowdoc.Text = "";
        txtheading.Enabled = true;
        txthomework.Enabled = true;
        lblshowpic.Enabled = true;
        lnkdelpic.Enabled = true;
        lblshowdoc.Enabled = true;
        lnkdeldoc.Enabled = true;
        fudfile.Enabled = true;
        fudattachemntss.Enabled = true;
        btnupdate.Enabled = true;
        btndelete.Enabled = true;
        btnsend1.Enabled = true;
    }

    protected void chck_OnCheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkbx = (CheckBox)sender;
        //Button addbtn = (Button)sender;
        string rowIndxS = chkbx.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxS) - 2;
        string code = (gviewhomewrk.Rows[rowIndx].FindControl("lblsubcde") as Label).Text;

        CheckBox checkc = (gviewhomewrk.Rows[rowIndx].FindControl("chck") as CheckBox);

        for (int i = 0; i < gviewhomewrk.Rows.Count; i++)
        {

            if (i != rowIndx)
            {
                if (checkc.Checked)
                {
                    CheckBox check1 = (gviewhomewrk.Rows[i].FindControl("chck") as CheckBox);
                    if (check1.Checked)
                    {
                        string sub_code = (gviewhomewrk.Rows[i].FindControl("lblsubcde") as Label).Text;
                        if (sub_code == code)
                        {
                            CheckBox check = (gviewhomewrk.Rows[rowIndx].FindControl("chck") as CheckBox);
                            check.Checked = true;
                        }
                        else
                        {
                            CheckBox check = (gviewhomewrk.Rows[rowIndx].FindControl("chck") as CheckBox);
                            check.Checked = false;
                        }
                    }
                }
            }
        }

    }

    protected bool FileTypeCheck(FileUpload UploadFile, ref string fileName, ref string fileExtension, ref string documentType)
    {
        bool fileBool = false;
        try
        {
            if (UploadFile.FileName.EndsWith(".jpg") || UploadFile.FileName.EndsWith(".gif") || UploadFile.FileName.EndsWith(".png") || UploadFile.FileName.EndsWith(".txt") || UploadFile.FileName.EndsWith(".doc") || UploadFile.FileName.EndsWith(".xls") || UploadFile.FileName.EndsWith(".docx") || UploadFile.FileName.EndsWith(".txt") || UploadFile.FileName.EndsWith(".document") || UploadFile.FileName.EndsWith(".xls") || UploadFile.FileName.EndsWith(".xlsx") || UploadFile.FileName.EndsWith(".pdf") || UploadFile.FileName.EndsWith(".ppt") || UploadFile.FileName.EndsWith(".pptx"))
            {
                fileName = Path.GetFileName(UploadFile.PostedFile.FileName);
                fileExtension = Path.GetExtension(UploadFile.PostedFile.FileName);
                documentType = string.Empty;
                switch (fileExtension)
                {
                    case ".pdf":
                        documentType = "application/pdf";
                        break;
                    case ".xls":
                        documentType = "application/vnd.ms-excel";
                        break;
                    case ".xlsx":
                        documentType = "application/vnd.ms-excel";
                        break;
                    case ".doc":
                        documentType = "application/vnd.ms-word";
                        break;
                    case ".docx":
                        documentType = "application/vnd.ms-word";
                        break;
                    case ".gif":
                        documentType = "image/gif";
                        break;
                    case ".png":
                        documentType = "image/png";
                        break;
                    case ".jpg":
                        documentType = "image/jpg";
                        break;
                    case ".ppt":
                        documentType = "application/vnd.ms-ppt";
                        break;
                    case ".pptx":
                        documentType = "application/vnd.ms-pptx";
                        break;
                    case ".txt":
                        documentType = "application/txt";
                        break;
                }
                if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(fileExtension) && !string.IsNullOrEmpty(documentType))
                    fileBool = true;
            }
        }
        catch { return fileBool; }
        return fileBool;
    }

    protected bool FileTypeCheck1(FileUpload UploadFile1, ref string fileName1, ref string fileExtension1, ref string documentType1)
    {
        bool fileBool = false;
        try
        {
            if (UploadFile1.FileName.EndsWith(".jpg") || UploadFile1.FileName.EndsWith(".gif") || UploadFile1.FileName.EndsWith(".png") || UploadFile1.FileName.EndsWith(".txt") || UploadFile1.FileName.EndsWith(".doc") || UploadFile1.FileName.EndsWith(".xls") || UploadFile1.FileName.EndsWith(".docx") || UploadFile1.FileName.EndsWith(".txt") || UploadFile1.FileName.EndsWith(".document") || UploadFile1.FileName.EndsWith(".xls") || UploadFile1.FileName.EndsWith(".xlsx") || UploadFile1.FileName.EndsWith(".pdf") || UploadFile1.FileName.EndsWith(".ppt") || UploadFile1.FileName.EndsWith(".pptx"))
            {
                fileName1 = Path.GetFileName(UploadFile1.PostedFile.FileName);
                fileExtension1 = Path.GetExtension(UploadFile1.PostedFile.FileName);
                documentType1 = string.Empty;
                switch (fileExtension1)
                {
                    case ".pdf":
                        documentType1 = "application/pdf";
                        break;
                    case ".xls":
                        documentType1 = "application/vnd.ms-excel";
                        break;
                    case ".xlsx":
                        documentType1 = "application/vnd.ms-excel";
                        break;
                    case ".doc":
                        documentType1 = "application/vnd.ms-word";
                        break;
                    case ".docx":
                        documentType1 = "application/vnd.ms-word";
                        break;
                    case ".gif":
                        documentType1 = "image/gif";
                        break;
                    case ".png":
                        documentType1 = "image/png";
                        break;
                    case ".jpg":
                        documentType1 = "image/jpg";
                        break;
                    case ".ppt":
                        documentType1 = "application/vnd.ms-ppt";
                        break;
                    case ".pptx":
                        documentType1 = "application/vnd.ms-pptx";
                        break;
                    case ".txt":
                        documentType1 = "application/txt";
                        break;
                }
                if (!string.IsNullOrEmpty(fileName1) && !string.IsNullOrEmpty(fileExtension1) && !string.IsNullOrEmpty(documentType1))
                    fileBool = true;
            }
        }
        catch { return fileBool; }
        return fileBool;
    }

    protected void btnclosespread_OnClick(object sender, EventArgs e)
    {
        divhme.Visible = false;
        div1.Visible = false;
        gviewhomewrk.Visible = true;
    }

}