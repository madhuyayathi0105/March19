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
using System.Data.SqlClient;
using System.Configuration;


public partial class ExamQuesSettings : System.Web.UI.Page
{
    SqlConnection cona = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    DAccess2 da = new DAccess2();
    DAccess2 dt = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    Hashtable ht = new Hashtable();
    DataTable dtCommon = new DataTable();
    ReuasableMethods rs = new ReuasableMethods();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string error = "";
    string qryCollege = string.Empty;
    string qry = string.Empty;
    string qryBatch = string.Empty;
    string norow = string.Empty;
    string nocol = string.Empty;
    string allotseat = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string grouporusercode = string.Empty;

   

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

                //bindbranch();
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
                rbInternal.Checked = true;
                GridReport.Visible = false;
                Button1.Visible = false;
                Bindcollege();
                BindRightsBaseBatch();
                binddegree();
                bindbranch();
                bindSem();
                bindSuType();
                BindSubject();
                bindTest();
                year1();
                month1();
                ddlYear.Enabled = false;
                ddlMonth.Enabled = false;
            }

        }
        catch (Exception ex)
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
            ddlbatch.Items.Clear();
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
                    ddlbatch.DataSource = ds;
                    ddlbatch.DataTextField = "Batch_Year";
                    ddlbatch.DataValueField = "Batch_Year";
                    ddlbatch.DataBind();
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
                columnfield = " and dp.group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string valBatch = string.Empty;

            if (ddlbatch.Items.Count > 0)
                valBatch = Convert.ToString(ddlbatch.SelectedValue);
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
                columnfield = " and dp.group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            //string valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            //string valDegree = rs.GetSelectedItemsValueAsString(cblDegree);

            string valBatch = string.Empty;// rs.GetSelectedItemsValueAsString(cblBatch);
            string valDegree = string.Empty;//rs.GetSelectedItemsValueAsString(cblBranch);
            if (ddlbatch.Items.Count > 0)
                valBatch = Convert.ToString(ddlbatch.SelectedValue);
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

    public void bindTest()
    {
        ddlTest.Items.Clear();
        string valBatch = string.Empty; //rs.GetSelectedItemsValueAsString(cblBatch);
        string valDegree = string.Empty; //rs.GetSelectedItemsValueAsString(cblBranch);
        string subjectCode = string.Empty;
        if (ddlbatch.Items.Count > 0)
            valBatch = Convert.ToString(ddlbatch.SelectedValue);
        if (cblBranch.Items.Count > 0)
            valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
        if (CblSubject.Items.Count > 0)
            subjectCode = rs.GetSelectedItemsValueAsString(CblSubject);
        string selTest = string.Empty;
        string sem = Convert.ToString(ddlsem.SelectedValue);

        if (!string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree) && !string.IsNullOrEmpty(subjectCode))
        {
            //selTest = "select distinct ci.criteria from CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e where ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no  and sm.semester=r.Current_Semester and sm.Batch_Year=r.Batch_Year and r.degree_code=sm.degree_code and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and sm.Batch_Year in ('" + valBatch + "') and sm.degree_code in ('" + valDegree + "') order by ci.criteria";
            string visi = string.Empty;
            if (!string.IsNullOrEmpty(Session["staff_code"].ToString().Trim()))
                visi = " and e.Visiblity='1'";

            //selTest = "select distinct ci.criteria from CriteriaForInternal ci,Registration r,syllabus_master sm,Exam_type e,subject s where s.subject_no=e.subject_no and s.syll_code=sm.syll_code and ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no  and sm.semester=r.Current_Semester and sm.Batch_Year=r.Batch_Year and r.degree_code=sm.degree_code and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and sm.Batch_Year in ('" + valBatch + "') and sm.degree_code in ('" + valDegree + "') and subject_code in('" + subjectCode + "') " + visi + " order by ci.criteria";
            selTest = " select distinct ci.criteria from CriteriaForInternal ci,syllabus_master sm,Exam_type e,subject s where s.subject_no=e.subject_no and s.syll_code=sm.syll_code and ci.syll_code=sm.syll_code and ci.Criteria_no=e.criteria_no   and sm.Batch_Year in ('" + valBatch + "') and sm.degree_code in ('" + valDegree + "') and subject_code in('" + subjectCode + "') and sm.semester='" + sem + "' " + visi + " order by ci.criteria";
            ds = da.select_method_wo_parameter(selTest, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlTest.DataSource = ds;
                ddlTest.DataTextField = "criteria";
                ddlTest.DataValueField = "criteria";
                ddlTest.DataBind();
                ddlTest.SelectedIndex = 0;
                ddlTest.Enabled = true;
            }

        }
        else
        {
            //lblErrmsg.Visible = true;
            //lblErrmsg.Text = "Invalid to select";
        }
    }

    public void bindSuType()
    {
        try
        {
            string pro = string.Empty;
            if (rbExternal.Checked)
                pro = "   and sub_sem.promote_count<>0";
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            txttype.Text = "---Select---";
            CheckBox1.Checked = false;
            CheckBoxList1.Items.Clear();
            if (ddlbatch.Items.Count > 0)
                valBatch = Convert.ToString(ddlbatch.SelectedValue);
            if (cblBranch.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
            if (!string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                string selBranch = "select distinct ss.subject_type from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=sy.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no  and  sy.Batch_Year in('" + valBatch + "') and sy.degree_code in('" + valDegree + "')  and sy.semester='" + Convert.ToString(ddlsem.SelectedValue) + "' order by ss.subject_type";//sub_sem.syll_Code = subject.syll_code and
                ds = da.select_method_wo_parameter(selBranch, "Text");
                ds = da.select_method_wo_parameter(selBranch, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    CheckBoxList1.DataSource = ds;
                    CheckBoxList1.DataTextField = "subject_type";
                    CheckBoxList1.DataValueField = "subject_type";
                    CheckBoxList1.DataBind();
                    checkBoxListselectOrDeselect(CheckBoxList1, true);
                    CallCheckboxListChange(CheckBox1, CheckBoxList1, txttype, lblSubType.Text, "--Select--");
                }
            }
        }
        catch
        {

        }
    }

    public void BindSubject()
    {
        try
        {
            string degreecode = string.Empty;
            txtSubject.Text = "---Select---";
            chkSubject.Checked = false;
            CblSubject.Items.Clear();
            ds.Clear();
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            string subtype = string.Empty;
            string sem = Convert.ToString(ddlsem.SelectedValue);
            if (CheckBoxList1.Items.Count > 0)
                subtype = getCblSelectedText(CheckBoxList1);
            if (ddlbatch.Items.Count > 0)
                valBatch = Convert.ToString(ddlbatch.SelectedValue);
            if (cblBranch.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
            string pro = string.Empty;
            if (rbExternal.Checked)
                pro = "   and sub_sem.promote_count<>0";
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree) && !string.IsNullOrEmpty(subtype))
            {

                if (!string.IsNullOrEmpty(Session["staff_code"].ToString().Trim()))
                {
                    //selBranch = "select distinct subject_name,subject_code,CONVERT(nvarchar(max),isnull(subject.subject_code,'')+'-'+isnull(subject.subject_name,'')) as text from subject,sub_sem,syllabus_master,staff_selector ,registration where  subject.subtype_no = sub_sem.subtype_no  and subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code=registration.degree_code and syllabus_master.semester =registration.current_semester and syllabus_master.batch_year=registration.batch_year  and subject.subject_no =staff_selector.subject_no  and  registration.degree_code in('" + valDegree + "') and registration.batch_year in('" + valBatch + "') and RollNo_Flag<>'0' and cc='0' and DelFlag<>1  and exam_flag <> 'DEBAR' and sub_sem.subject_type in(" + subtype + ") and staff_selector.staff_code='" + Session["staff_code"].ToString() + "'    order by subject.subject_name";
                    selBranch = " select distinct subject_name,subject_code,CONVERT(nvarchar(max),isnull(subject.subject_code,'')+'-'+isnull(subject.subject_name,'')) as text from subject,sub_sem,syllabus_master,staff_selector  where  subject.subtype_no = sub_sem.subtype_no  and subject.syll_code=syllabus_master.syll_code and subject.subject_no =staff_selector.subject_no  and  syllabus_master.degree_code in('" + valDegree + "') and syllabus_master.batch_year in('" + valBatch + "') and sub_sem.subject_type in(" + subtype + ") and staff_selector.staff_code='" + Session["staff_code"].ToString() + "' and syllabus_master.semester='" + sem + "' " + pro + "   order by subject.subject_name";
                }
                else
                {

                    selBranch = "select distinct subject_name,subject_code,CONVERT(nvarchar(max),isnull(subject.subject_code,'')+'-'+isnull(subject.subject_name,'')) as text from subject,sub_sem,syllabus_master where  subject.subtype_no = sub_sem.subtype_no  and subject.syll_code=syllabus_master.syll_code   and  syllabus_master.degree_code in('" + valDegree + "') and syllabus_master.batch_year in('" + valBatch + "') and syllabus_master.semester='" + sem + "' and sub_sem.subject_type in(" + subtype + ")  " + pro + "   order by subject.subject_name";

                    // selBranch = "select distinct subject_name,subject_code,CONVERT(nvarchar(max),isnull(subject.subject_code,'')+'-'+isnull(subject.subject_name,'')) as text from subject,sub_sem,syllabus_master,registration where  subject.subtype_no = sub_sem.subtype_no  and subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code=registration.degree_code and syllabus_master.semester =registration.current_semester and syllabus_master.batch_year=registration.batch_year   and  registration.degree_code in('" + valDegree + "') and registration.batch_year in('" + valBatch + "') and RollNo_Flag<>'0' and cc='0' and DelFlag<>1  and exam_flag <> 'DEBAR' and sub_sem.subject_type in(" + subtype + ")  order by subject.subject_name";//sub_sem.syll_Code = subject.syll_code and
                }
                ds = da.select_method_wo_parameter(selBranch, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                CblSubject.DataSource = ds;
                CblSubject.DataTextField = "text";
                CblSubject.DataValueField = "subject_code";
                CblSubject.DataBind();
                checkBoxListselectOrDeselect(CblSubject, true);
                CallCheckboxListChange(chkSubject, CblSubject, txtSubject, lblSubject.Text, "--Select--");
            }
        }
        catch
        {

        }
    }

    public void bindSem()
    {
        string selBranch = string.Empty;
        string valBatch = string.Empty;
        string valDegree = string.Empty;
        string subtype = string.Empty;
        ddlsem.ClearSelection();
        ddlsem.Items.Clear();
        if (ddlbatch.Items.Count > 0)
            valBatch = Convert.ToString(ddlbatch.SelectedValue);
        if (cblBranch.Items.Count > 0)
            valDegree = rs.GetSelectedItemsValueAsString(cblBranch);

        string selectQ = "select distinct current_semester from registration where batch_year=" + valBatch + " and degree_code in('" + valDegree + "')  and cc=0 and delflag<>1 and exam_flag<>'debar' order by current_semester";
        DataTable dtSem = dirAcc.selectDataTable(selectQ);
        if (dtSem.Rows.Count > 0)
        {
            ddlsem.DataSource = dtSem;
            ddlsem.DataTextField = "current_semester";
            ddlsem.DataValueField = "current_semester";
            ddlsem.DataBind();
        }
    }

    protected void ddlSubType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSubject();
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            BindRightsBaseBatch();
            binddegree();
            bindbranch();
            BindSubject();
            bindTest();


        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddegree();
            bindbranch();
            bindSem();
            bindSuType();
            BindSubject();
            bindTest();
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
            bindSem();
            bindSuType();
            BindSubject();
            bindTest();

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
            bindSem();
            bindSuType();
            BindSubject();
            bindTest();

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
            bindSem();
            bindSuType();
            BindSubject();
            bindTest();

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
            bindSem();
            bindSuType();
            BindSubject();
            bindTest();

        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindSuType();
        BindSubject();
        bindTest();
    }

    protected void ddlTest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //BindSubject();
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkSubject_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkSubject, CblSubject, txtSubject, lblSubject.Text, "--Select--");
            bindTest();
        }
        catch (Exception ex)
        {
            CallCheckboxChange(chkSubject, CblSubject, txtSubject, lblSubject.Text, "--Select--");
            bindTest();
        }
    }

    protected void CblSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chkSubject, CblSubject, txtSubject, lblSubject.Text, "--Select--");
            bindTest();
        }
        catch (Exception ex)
        {
        }
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(CheckBox1, CheckBoxList1, txttype, lblTest.Text, "--Select--");
            BindSubject();
            bindTest();
        }
        catch (Exception ex)
        {
            CallCheckboxChange(CheckBox1, CheckBoxList1, txttype, lblTest.Text, "--Select--");
            bindTest();

        }
    }

    protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(CheckBox1, CheckBoxList1, txttype, lblTest.Text, "--Select--");
        BindSubject();
        bindTest();
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            GridReport.Visible = false;
            GridView1.Visible = false;
            Button1.Visible = false;
            btnSave.Visible = false;
            Button2.Visible = false;
            GridView2.Visible = false;
            GridView3.Visible = false;
            Button3.Visible = false;
            int noPart = 0;
            DataTable dtPartAlloc = new DataTable();
            dtPartAlloc.Columns.Add("PartNo");
            dtPartAlloc.Columns.Add("PartName");
            dtPartAlloc.Columns.Add("Qno");
            string batchYear = Convert.ToString(ddlbatch.SelectedValue);
            string degr = rs.getCblSelectedValue(cblBranch);
            string subCode = rs.getCblSelectedValue(CblSubject);
            string criNo = Convert.ToString(ddlTest.SelectedValue);
            string sem=Convert.ToString(ddlsem.SelectedValue);
            string selectQ = string.Empty;
            if (rbInternal.Checked)
                selectQ = "select max(e.max_mark) as maxMark from subject s,syllabus_master sy,CriteriaForInternal c,Exam_type e where s.subject_no=e.subject_no and c.Criteria_no=e.criteria_no and s.syll_code=sy.syll_code and c.syll_code=sy.syll_code and s.subject_code in('" + subCode + "') and sy.Batch_Year in(" + batchYear + ") and sy.degree_code in('" + degr + "') and c.criteria='" + criNo + "'";
            else
                selectQ = "select max(s.maxtotal) as maxMark from subject s,syllabus_master sy where  s.syll_code=sy.syll_code  and sy.Batch_Year in(" + batchYear + ") and sy.degree_code in('" + degr + "') and s.subject_code in('" + subCode + "') and sy.semester='" + sem + "'";

            string MaxMark = da.GetFunction(selectQ);
            lblMaxMark.Text = MaxMark;
            DataRow dr = null;

            if (!string.IsNullOrEmpty(txtNoPart.Text))
            {
                int.TryParse(txtNoPart.Text, out noPart);
                if (noPart > 0)
                {
                    for (int i = 1; i <= noPart; i++)
                    {
                        dr = dtPartAlloc.NewRow();
                        dr["PartNo"] = i;
                        dr["PartName"] = getPartText(i.ToString());
                        dr["Qno"] = "";
                        dtPartAlloc.Rows.Add(dr);
                    }
                }
            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Enter No.of Part";
                divPopAlert.Visible = true;
            }
            if (dtPartAlloc.Rows.Count > 0)
            {
                GridPart.DataSource = dtPartAlloc;
                GridPart.DataBind();
                GridPart.Visible = true;
                Button1.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            GridReport.Visible = false;
            btnSave.Visible = false;
            GridView3.Visible = false;
            Button3.Visible = false;
            GridView1.Visible = false;
            int noPart = 0;
            DataTable dtSettings = new DataTable();
            dtSettings.Columns.Add("PartNo");
            dtSettings.Columns.Add("NO_Ques");
            //dtSettings.Columns.Add("Unit");
            dtSettings.Columns.Add("Partname");
            DataRow dr = null;
            bool isVal = false;

            if (!string.IsNullOrEmpty(txtNoPart.Text))
            {
                int.TryParse(txtNoPart.Text, out noPart);
                if (noPart > 0)
                {
                    foreach (GridViewRow gr in GridPart.Rows)
                    {
                        string partNo = (gr.FindControl("lblPart") as Label).Text;
                        string partname = (gr.FindControl("lblPartNo") as Label).Text;
                        int NoQues = 0;
                        string NoQ = (gr.FindControl("txtNoQ") as TextBox).Text;
                        if (string.IsNullOrEmpty(NoQ))
                        {
                            isVal = true;
                        }
                        int.TryParse(NoQ, out NoQues);
                        if (NoQues > 0)
                        {
                            for (int i = 1; i <= NoQues; i++)
                            {
                                dr = dtSettings.NewRow();
                                dr["PartNo"] = partNo;
                                dr["PartName"] = getPartText(partNo);
                                dr["NO_Ques"] = NoQues;
                                dtSettings.Rows.Add(dr);
                            }
                        }
                    }
                }
                if (dtSettings.Rows.Count > 0 && !isVal)
                {
                    GridView2.DataSource = dtSettings;
                    GridView2.DataBind();
                    GridView2.Visible = true;
                    Button2.Visible = true;
                }
                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Enter Valid Marks";
                    divPopAlert.Visible = true;
                    GridView2.Visible = false;
                    Button2.Visible = false;
                }
            }
        }
        catch
        {

        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            bool boxQ1 = false;
            GridReport.Visible = false;
            btnSave.Visible = false;
            GridView3.Visible = false;
            Button3.Visible = false;
            GridView1.Visible = false;
            int noPart = 0;
            DataTable dtSettings = new DataTable();
            dtSettings.Columns.Add("PartNo");
            dtSettings.Columns.Add("Partname");
            dtSettings.Columns.Add("NO_Ques");
            dtSettings.Columns.Add("Qno");
            dtSettings.Columns.Add("Sub1");
            //dtSettings.Columns.Add("Sub2");
            DataRow dr = null;
            bool isVal = false;

            if (!string.IsNullOrEmpty(txtNoPart.Text))
            {
                int.TryParse(txtNoPart.Text, out noPart);
                if (noPart > 0)
                {
                    foreach (GridViewRow gr in GridView2.Rows)
                    {
                        string partNo = (gr.FindControl("lblPart") as Label).Text;
                        string partname = (gr.FindControl("lblPartNo") as Label).Text;
                        int NoQues = 0;
                        int Nosub1 = 0;
                        string NoQ = (gr.FindControl("lblNoQ") as Label).Text;
                        //int.TryParse(NoQ, out NoQues);
                        //{
                        //    for (int a = 1; a <= NoQues; a++)
                        //    {
                        string QNo = (gr.FindControl("lblQNo") as Label).Text;
                        string sub1 = (gr.FindControl("txtNoQ1") as TextBox).Text;
                        
                        int.TryParse(sub1, out Nosub1);
                        if (Nosub1 > 1)
                        {
                            for (int i = 1; i <= Nosub1; i++)
                            {
                                boxQ1 = true;
                                dr = dtSettings.NewRow();
                                dr["PartNo"] = partNo;
                                dr["PartName"] = getPartText(partNo);
                                dr["NO_Ques"] = NoQ;
                                dr["Qno"] = QNo;
                                dr["Sub1"] = getSubText1(i.ToString());
                                dtSettings.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            dr = dtSettings.NewRow();
                            dr["PartNo"] = partNo;
                            dr["PartName"] = getPartText(partNo);
                            dr["NO_Ques"] = NoQ;
                            dr["Qno"] = QNo;
                            dr["Sub1"] = "";
                            dtSettings.Rows.Add(dr);
                        }
                        //    }
                        //}
                    }
                }

                if (dtSettings.Rows.Count > 0 && !isVal)
                {
                    GridView3.DataSource = dtSettings;
                    GridView3.DataBind();
                    GridView3.Visible = true;
                    Button3.Visible = true;
                    if (boxQ1 == false)
                    {
                        GridView3.Enabled = false;
                        Button3_Click(sender, e);
                    }
                    else
                    {
                        GridView3.Enabled = true;
                    }

                }
                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Enter Valid Marks";
                    divPopAlert.Visible = true;
                    GridView3.Visible = false;
                    Button3.Visible = false;
                }
            }
            
        }
        catch
        {
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        try
        {
            GridReport.Visible = false;
            bool sub2flag = false;
            btnSave.Visible = false;
            GridView1.Visible = false;
            int noPart = 0;
            DataTable dtSettings = new DataTable();
            dtSettings.Columns.Add("PartNo");
            dtSettings.Columns.Add("Partname");
            dtSettings.Columns.Add("NO_Ques");
            dtSettings.Columns.Add("Qno");
            dtSettings.Columns.Add("Sub1");
            dtSettings.Columns.Add("Sub2");

            foreach (GridViewRow gr1 in GridView2.Rows)
            {
                TextBox txtb = (gr1.FindControl("txtNoQ1")) as TextBox;
                string txtboxQ1 = Convert.ToString(txtb.Text);
                 //(gr1.FindControl("txtNoQ1") 
                if (string.IsNullOrEmpty(txtboxQ1))
                {
                    sub2flag = false;
                }
                else
                {
                    sub2flag = true;
                    goto sublbl;
                }
            }
        sublbl:
           
               
            
            DataRow dr = null;
            bool isVal = false;

            if (!string.IsNullOrEmpty(txtNoPart.Text))
            {
                int.TryParse(txtNoPart.Text, out noPart);
                if (noPart > 0)
                {
                    foreach (GridViewRow gr in GridView3.Rows)
                    {
                        string partNo = (gr.FindControl("lblPart") as Label).Text;
                        string partname = (gr.FindControl("lblPartNo") as Label).Text;
                        string NoQ = (gr.FindControl("lblNoQ") as Label).Text;
                        string QNo = (gr.FindControl("lblQNo") as Label).Text;
                        string sub1 = (gr.FindControl("lblSub1") as Label).Text;
                        string sub2 = (gr.FindControl("txtNoQ2") as TextBox).Text;
                        int Nosub2 = 0;
                        int.TryParse(sub2, out Nosub2);
                        if (Nosub2 > 1)
                        {
                            for (int i = 1; i <= Nosub2; i++)
                            {
                                dr = dtSettings.NewRow();
                                dr["PartNo"] = partNo;
                                dr["PartName"] = getPartText(partNo);
                                dr["NO_Ques"] = NoQ;
                                dr["Qno"] = QNo;
                                dr["Sub1"] = sub1;
                                 dr["Sub2"] = getSubText2(i.ToString());
                                dtSettings.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            dr = dtSettings.NewRow();
                            dr["PartNo"] = partNo;
                            dr["PartName"] = getPartText(partNo);
                            dr["NO_Ques"] = NoQ;
                            dr["Qno"] = QNo;
                            dr["Sub1"] = sub1;
                            dr["Sub2"] = "";
                            dtSettings.Rows.Add(dr);
                        }
                        //    }
                        //}
                    }
                }
                if (dtSettings.Rows.Count > 0 && !isVal)
                {
                    GridView1.DataSource = dtSettings;
                    GridView1.DataBind();
                    if (sub2flag == false)
                    {
                        GridView1.Columns[4].Visible = false;
                    }
                    else
                    {
                        GridView1.Columns[4].Visible = true;
                    }
                    GridView1.Visible = true;
                    btnSave.Visible = true;
                }
                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Enter Valid Marks";
                    divPopAlert.Visible = true;
                    GridView1.Visible = false;
                    btnSave.Visible = false;
                }
            }
        }
        catch
        {
        }
    }

    protected void OnDataBound(object sender, EventArgs e)
    {
        try
        {
            DataTable dtCoSettings = dirAcc.selectDataTable("select * from Master_Settings where settings='COSettings'");

            for (int rowIndex = GridView1.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = GridView1.Rows[rowIndex];
                GridViewRow previousRow = GridView1.Rows[rowIndex + 1];

                string l1 = (row.FindControl("lblPartgNO") as Label).Text;
                string l2 = (previousRow.FindControl("lblPartgNO") as Label).Text;
                if (l1 == l2)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                           previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;
                    row.Cells[1].RowSpan = row.Cells[0].RowSpan;
                    previousRow.Cells[1].Visible = false;
                    //previousRow.Cells[1].Visible = false;
                }
                string l111 = (row.FindControl("lblgQno") as Label).Text;
                string l211 = (previousRow.FindControl("lblgQno") as Label).Text;
                if (l111 == l211)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                           previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                    row.Cells[2].RowSpan = row.Cells[2].RowSpan;

                    //previousRow.Cells[1].Visible = false;
                }
            }
            foreach (GridViewRow grid in GridView1.Rows)
            {
                DropDownList dr1 = new DropDownList();
                dr1 = (grid.FindControl("ddlgUnit") as DropDownList);
                dr1.DataSource = dtCoSettings;
                dr1.DataTextField = "template";
                dr1.DataValueField = "masterno";
                dr1.DataBind();
                dr1.Items.Insert(0, " ");
            }
        }
        catch
        {
        }
    }
    protected void GridView2_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            for (int rowIndex = GridView2.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = GridView2.Rows[rowIndex];
                GridViewRow previousRow = GridView2.Rows[rowIndex + 1];

                string l1 = (row.FindControl("lblPartNo") as Label).Text;
                string l2 = (previousRow.FindControl("lblPartNo") as Label).Text;
                if (l1 == l2)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                           previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;
                    row.Cells[1].RowSpan = row.Cells[0].RowSpan;
                    previousRow.Cells[1].Visible = false;
                    //previousRow.Cells[1].Visible = false;
                }



            }
        }
        catch
        {
        }
    }
    protected void GridView3_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            for (int rowIndex = GridView3.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = GridView3.Rows[rowIndex];
                GridViewRow previousRow = GridView3.Rows[rowIndex + 1];

                string l1 = (row.FindControl("lblPartNo") as Label).Text;
                string l2 = (previousRow.FindControl("lblPartNo") as Label).Text;
                if (l1 == l2)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                           previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;
                    row.Cells[1].RowSpan = row.Cells[0].RowSpan;
                    previousRow.Cells[1].Visible = false;
                    //previousRow.Cells[1].Visible = false;
                }
                string l111 = (row.FindControl("lblQNo") as Label).Text;
                string l211 = (previousRow.FindControl("lblQNo") as Label).Text;
                if (l111 == l211)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                           previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                    row.Cells[2].RowSpan = row.Cells[2].RowSpan;

                    //previousRow.Cells[1].Visible = false;
                }

            }
        }
        catch
        {
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string batchYear = string.Empty;
            string degcode = string.Empty;
            string semes = string.Empty;
            string criteria = Convert.ToString(ddlTest.SelectedItem.Text);
            DataTable dtInsert = new DataTable();
            Hashtable hat = new Hashtable();
            string sem = Convert.ToString(ddlsem.SelectedValue);
            string examYear = Convert.ToString(ddlYear.SelectedValue);
            string examMonth = Convert.ToString(ddlMonth.SelectedValue);
            dtInsert.Columns.Add("subjectNo");
            dtInsert.Columns.Add("No_part");
            dtInsert.Columns.Add("PartNo");
            dtInsert.Columns.Add("NO_Ques");
            dtInsert.Columns.Add("QNo");
            dtInsert.Columns.Add("Mark");
            dtInsert.Columns.Add("sub1");
            dtInsert.Columns.Add("sub2");
            dtInsert.Columns.Add("CourseOutComeNo");
            dtInsert.Columns.Add("CriteriaNo");
            dtInsert.Columns.Add("examtype");
            DataRow drinsert = null;
            double maxmar = 0;
            double.TryParse(lblMaxMark.Text, out maxmar);
            bool ismax = false;
            bool isSave = false;
            bool isEmpty = false;
            bool ismark = false;
            string degName = string.Empty;
            foreach (GridViewRow GridRow in GridView1.Rows)
            {
                double mar = 0;
                string Co = Convert.ToString((GridRow.FindControl("ddlgUnit") as DropDownList).SelectedValue);
                string marva = Convert.ToString((GridRow.FindControl("txtgMarks") as TextBox).Text);//
                double.TryParse(marva, out mar);
                if (maxmar > 0 && mar > maxmar)
                    ismax = true;
                if (string.IsNullOrEmpty(marva))
                    ismark = true;
                if (string.IsNullOrEmpty(Co) || Co == "0" || Co == " ")
                    isEmpty = true;
            }
            if (isEmpty)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Select Cource outcome!!";
                divPopAlert.Visible = true;
                return;
            }
            if (ismark)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Mark is empty!!";
                divPopAlert.Visible = true;
                return;
            }
            if (rbExternal.Checked)
            {
                if (string.IsNullOrEmpty(examYear) && string.IsNullOrEmpty(examMonth))
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Select exam and Month & Year";
                    divPopAlert.Visible = true;
                    return;
                }
            }

            if (!ismax)
            {
                if (!string.IsNullOrEmpty(criteria))
                {
                    if (GridView1.Rows.Count > 0)
                    {
                        batchYear = Convert.ToString(ddlbatch.SelectedValue);
                        if (!string.IsNullOrEmpty(batchYear))
                        {

                            for (int deg = 0; deg < cblBranch.Items.Count; deg++)
                            {

                                if (cblBranch.Items[deg].Selected)
                                {
                                    degcode = Convert.ToString(cblBranch.Items[deg].Value);
                                    degName = Convert.ToString(cblBranch.Items[deg].Text);
                                    //semes = da.GetFunction("select distinct r.Current_Semester from Registration r,syllabus_master sy where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.semester=r.Current_Semester and r.degree_code=" + degcode + " and r.Batch_Year=" + batchYear + " and RollNo_Flag<>'0' and cc='0' and DelFlag<>1  and exam_flag <> 'DEBAR'  order by r.Current_Semester desc");
                                    DataTable dtSyll = new DataTable();
                                    if(rbInternal.Checked)
                                     dtSyll = dirAcc.selectDataTable("select distinct ee.max_mark,c.Criteria_no,c.syll_code,r.Current_Semester,s.subject_code,s.subject_no from Registration r,syllabus_master sy,CriteriaForInternal c,subject s,Exam_type ee where ee.criteria_no=c.Criteria_no and  s.subject_no=ee.subject_no and   s.syll_code=sy.syll_code and sy.syll_code=c.syll_code and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.semester=r.Current_Semester and r.degree_code=" + degcode + " and r.Batch_Year=" + batchYear + " and RollNo_Flag<>'0' and cc='0' and DelFlag<>1  and exam_flag <> 'DEBAR' and c.criteria='" + criteria.Trim() + "'   order by c.Criteria_no,c.syll_code desc");
                                    else
                                      dtSyll = dirAcc.selectDataTable("select distinct s.maxtotal,e.exam_code,sy.syll_code,e.Current_Semester,s.subject_code,s.subject_no from subject s,syllabus_master sy,Exam_Details e,exam_appl_details ead,exam_application ea where  e.exam_code=ea.exam_code and s.subject_no=ead.subject_no and s.syll_code=sy.syll_code and sy.Batch_Year=e.batch_year and sy.degree_code=e.degree_code and ead.appl_no=ea.appl_no and e.Exam_year='"+examYear+"' and e.Exam_Month='"+examMonth+"' and sy.Batch_Year=" + batchYear + " and sy.degree_code=" + degcode + " and e.current_semester='"+sem+"'");

                                    if (dtSyll.Rows.Count > 0)
                                    {
                                        dtInsert.Clear();
                                        for (int sub = 0; sub < CblSubject.Items.Count; sub++)
                                        {
                                            if (CblSubject.Items[sub].Selected)
                                            {
                                                bool isNotValid = false;
                                                string subjectCode = Convert.ToString(CblSubject.Items[sub].Value);
                                                dtSyll.DefaultView.RowFilter = "subject_code='" + subjectCode + "'";
                                                DataView dv = dtSyll.DefaultView;
                                                if (!rbExternal.Checked)
                                                {
                                                    if (dv.Count > 0)
                                                    {
                                                        string error1 = string.Empty;
                                                        double subMax = 0;
                                                        string maxMark = Convert.ToString(dv[0]["max_mark"]);
                                                        double.TryParse(maxMark, out subMax);
                                                        bool isval = false;
                                                        string SelectQ = "select * from NewInternalMarkEntry where ExamCode in(select exam_code from Exam_type where subject_no='" + Convert.ToString(dv[0]["subject_no"]) + "' and criteria_no='" + Convert.ToString(dv[0]["Criteria_no"]) + "') and ISNULL(examtype,0)='0'";
                                                        DataTable DataReport = dirAcc.selectDataTable(SelectQ);
                                                        if (DataReport.Rows.Count > 0)
                                                        {
                                                            isval = true;
                                                        }
                                                        if (!isval)
                                                        {
                                                            int dele = da.update_method_wo_parameter("delete  from  CAQuesSettingsParent where subjectNo='" + Convert.ToString(dv[0]["subject_no"]) + "' and CriteriaNo='" + Convert.ToString(dv[0]["Criteria_no"]) + "' and ISNULL(examtype,0)='0' ", "text");

                                                            foreach (GridViewRow Grid in GridView1.Rows)
                                                            {
                                                                isNotValid = false;
                                                                drinsert = dtInsert.NewRow();
                                                                drinsert["subjectNo"] = Convert.ToString(dv[0]["subject_no"]);
                                                                drinsert["No_part"] = Convert.ToString(GridPart.Rows.Count);
                                                                drinsert["PartNo"] = Convert.ToString((Grid.FindControl("lblPart") as Label).Text);
                                                                drinsert["NO_Ques"] = Convert.ToString((Grid.FindControl("txtgNoQ") as Label).Text);
                                                                drinsert["QNo"] = Convert.ToString((Grid.FindControl("lblgQno") as Label).Text);
                                                                drinsert["sub1"] = Convert.ToString((Grid.FindControl("lblSub1") as Label).Text);
                                                                drinsert["sub2"] = Convert.ToString((Grid.FindControl("lblSub2") as Label).Text);
                                                                drinsert["Mark"] = Convert.ToString((Grid.FindControl("txtgMarks") as TextBox).Text);
                                                                drinsert["examtype"] = "0";
                                                                double smark = 0;
                                                                double.TryParse(Convert.ToString((Grid.FindControl("txtgMarks") as TextBox).Text), out smark);
                                                                if (smark > subMax)
                                                                {
                                                                    isNotValid = true;
                                                                    if (!hat.ContainsKey(batchYear + " - " + degName + " - " + " : " + subjectCode))
                                                                    {
                                                                        hat.Add(batchYear + " - " + degName + " - " + " : " + subjectCode, batchYear + " - " + degName + " - " + " : " + subjectCode);

                                                                        error1 = error1 = batchYear + " - " + degName + " - " + " : " + subjectCode + '-' + " -Mark exit from Total Mark" + "\r\n";
                                                                        if (error == "")
                                                                        {
                                                                            error = error1;
                                                                        }
                                                                        else
                                                                        {
                                                                            error = error + "" + error1;
                                                                        }
                                                                    }
                                                                }
                                                                drinsert["CourseOutComeNo"] = Convert.ToString((Grid.FindControl("ddlgUnit") as DropDownList).SelectedItem.Value);
                                                                drinsert["CriteriaNo"] = Convert.ToString(dv[0]["Criteria_no"]);
                                                                if (!isNotValid)
                                                                {
                                                                    dtInsert.Rows.Add(drinsert);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {

                                                            if (isval)
                                                                error1 = error1 = batchYear + " - " + degName + " - " + " : " + subjectCode + '-' + " -Please Remove Mark" + "\r\n";
                                                            if (error == "")
                                                            {
                                                                error = error1;
                                                            }
                                                            else
                                                            {
                                                                error = error + "" + error1;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (dv.Count > 0)
                                                    {
                                                        string error1 = string.Empty;
                                                        double subMax = 0;
                                                        string maxMark = Convert.ToString(dv[0]["maxtotal"]);
                                                        string examCode = Convert.ToString(dv[0]["exam_code"]);
                                                        double.TryParse(maxMark, out subMax);
                                                        bool isval = false;
                                                        string SelectQ = "select * from NewInternalMarkEntry where ExamCode in('" + examCode + "') and ISNULL(examtype,0)='1'";
                                                        DataTable DataReport = dirAcc.selectDataTable(SelectQ);
                                                        if (DataReport.Rows.Count > 0)
                                                        {
                                                            isval = true;
                                                        }
                                                        if (!isval)
                                                        {
                                                            int dele = da.update_method_wo_parameter("delete  from  CAQuesSettingsParent where subjectNo='" + Convert.ToString(dv[0]["subject_no"]) + "' and CriteriaNo='" + examCode + "' and ISNULL(examtype,0)='1'", "text");

                                                            foreach (GridViewRow Grid in GridView1.Rows)
                                                            {
                                                                isNotValid = false;
                                                                drinsert = dtInsert.NewRow();
                                                                drinsert["subjectNo"] = Convert.ToString(dv[0]["subject_no"]);
                                                                drinsert["No_part"] = Convert.ToString(GridPart.Rows.Count);
                                                                drinsert["PartNo"] = Convert.ToString((Grid.FindControl("lblPart") as Label).Text);
                                                                drinsert["NO_Ques"] = Convert.ToString((Grid.FindControl("txtgNoQ") as Label).Text);
                                                                drinsert["QNo"] = Convert.ToString((Grid.FindControl("lblgQno") as Label).Text);
                                                                drinsert["sub1"] = Convert.ToString((Grid.FindControl("lblSub1") as Label).Text);
                                                                drinsert["sub2"] = Convert.ToString((Grid.FindControl("lblSub2") as Label).Text);
                                                                drinsert["Mark"] = Convert.ToString((Grid.FindControl("txtgMarks") as TextBox).Text);
                                                                drinsert["examtype"] = "1";
                                                                double smark = 0;
                                                                double.TryParse(Convert.ToString((Grid.FindControl("txtgMarks") as TextBox).Text), out smark);
                                                                if (smark > subMax)
                                                                {
                                                                    isNotValid = true;
                                                                    if (!hat.ContainsKey(batchYear + " - " + degName + " - " + " : " + subjectCode))
                                                                    {
                                                                        hat.Add(batchYear + " - " + degName + " - " + " : " + subjectCode, batchYear + " - " + degName + " - " + " : " + subjectCode);

                                                                        error1 = error1 = batchYear + " - " + degName + " - " + " : " + subjectCode + '-' + " -Mark exit from Total Mark" + "\r\n";
                                                                        if (error == "")
                                                                        {
                                                                            error = error1;
                                                                        }
                                                                        else
                                                                        {
                                                                            error = error + "" + error1;
                                                                        }
                                                                    }
                                                                }
                                                                drinsert["CourseOutComeNo"] = Convert.ToString((Grid.FindControl("ddlgUnit") as DropDownList).SelectedItem.Value);
                                                                drinsert["CriteriaNo"] = Convert.ToString(dv[0]["exam_code"]);
                                                                if (!isNotValid)
                                                                {
                                                                    dtInsert.Rows.Add(drinsert);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {

                                                            if (isval)
                                                                error1 = error1 = batchYear + " - " + degName + " - " + " : " + subjectCode + '-' + " -Please Remove Mark" + "\r\n";
                                                            if (error == "")
                                                            {
                                                                error = error1;
                                                            }
                                                            else
                                                            {
                                                                error = error + "" + error1;
                                                            }
                                                        }
                                                    }
                                                }


                                            }
                                        }
                                        if (dtInsert.Rows.Count > 0)
                                        {
                                            cona.Open();
                                            using (SqlBulkCopy sqlbc = new SqlBulkCopy(cona))
                                            {
                                                sqlbc.DestinationTableName = "CAQuesSettingsParent";
                                                sqlbc.ColumnMappings.Add("subjectNo", "subjectNo");
                                                sqlbc.ColumnMappings.Add("No_part", "No_part");
                                                sqlbc.ColumnMappings.Add("PartNo", "PartNo");
                                                sqlbc.ColumnMappings.Add("NO_Ques", "NO_Ques");
                                                sqlbc.ColumnMappings.Add("QNo", "QNo");
                                                sqlbc.ColumnMappings.Add("Mark", "Mark");
                                                sqlbc.ColumnMappings.Add("CourseOutComeNo", "CourseOutComeNo");
                                                sqlbc.ColumnMappings.Add("sub1", "sub1");
                                                sqlbc.ColumnMappings.Add("sub2", "sub2");
                                                sqlbc.ColumnMappings.Add("CriteriaNo", "CriteriaNo");
                                                sqlbc.ColumnMappings.Add("examtype", "examtype");
                                                sqlbc.WriteToServer(dtInsert);
                                                isSave = true;
                                                //Response.Write("Bulk data stored successfully");
                                            }
                                            cona.Close();
                                        }

                                    }
                                }
                            }
                        }

                        if (isSave)
                        {
                            lblAlertMsg.Visible = true;
                            lblAlertMsg.Text = "Saved";
                            divPopAlert.Visible = true;
                        }
                        if (!string.IsNullOrEmpty(error))
                        {
                            cannot_insert_div.Visible = true;
                            lbl_cannotsave.Visible = true;
                            lbl_cannotsave.Text = "Should Not Save Subject Code";
                            lbl_cannotinsert.Visible = true;
                            lbl_cannotinsert.Text = error;
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "NO Test were Found";
                    divPopAlert.Visible = true;

                }
            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Enter Mark Less Than Or Equal To Maximum Mark";
                divPopAlert.Visible = true;
            }

        }
        catch
        {
        }
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

    protected void btnReport_Click(object sender, EventArgs e)
    {
        try
        {
            GridReport.Visible = false;
            GridView1.Visible = false;
            Button1.Visible = false;
            btnSave.Visible = false;
            Button2.Visible = false;
            GridView2.Visible = false;
            GridView3.Visible = false;
            Button3.Visible = false;
            string batchYear = string.Empty;
            string degcode = string.Empty;
            string criteria = Convert.ToString(ddlTest.SelectedItem.Text);
            DataTable dtReport = new DataTable();
            dtReport.Columns.Add("DegInfo");
            dtReport.Columns.Add("SubjectNo");
            dtReport.Columns.Add("Subject Code");
            dtReport.Columns.Add("CriteriaNo");
            dtReport.Columns.Add("Subject Name");
            dtReport.Columns.Add("NoPart");
            dtReport.Columns.Add("PartNo");
            dtReport.Columns.Add("Qno");
            dtReport.Columns.Add("Course Outcome");
            dtReport.Columns.Add("Mark");
            dtReport.Columns.Add("Sub1");
            dtReport.Columns.Add("Sub2");
            dtReport.Columns.Add("Status");
            DataRow drRow = null;
            DataTable degInfo = dirAcc.selectDataTable("select * from Degree");
            batchYear = Convert.ToString(ddlbatch.SelectedValue);
            if (!string.IsNullOrEmpty(batchYear))
            {

                for (int deg = 0; deg < cblBranch.Items.Count; deg++)
                {

                    if (cblBranch.Items[deg].Selected)
                    {
                        string DegName = Convert.ToString(cblBranch.Items[deg].Text);
                        degcode = Convert.ToString(cblBranch.Items[deg].Value);
                        degInfo.DefaultView.RowFilter = "Degree_Code='" + degcode + "'";
                        DataView dvdeg = degInfo.DefaultView;
                        if (dvdeg.Count > 0)
                            DegName = Convert.ToString(dvdeg[0]["Acronym"]);
                        //semes = da.GetFunction("select distinct r.Current_Semester from Registration r,syllabus_master sy where sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.semester=r.Current_Semester and r.degree_code=" + degcode + " and r.Batch_Year=" + batchYear + " and RollNo_Flag<>'0' and cc='0' and DelFlag<>1  and exam_flag <> 'DEBAR'  order by r.Current_Semester desc");

                        DataTable dtSyll = dirAcc.selectDataTable("select distinct  c.Criteria_no,c.syll_code,r.Current_Semester,s.subject_code,s.subject_no,s.subject_name from Registration r,syllabus_master sy,CriteriaForInternal c,subject s where  s.syll_code=sy.syll_code and sy.syll_code=c.syll_code and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.semester=r.Current_Semester and r.degree_code=" + degcode + " and r.Batch_Year=" + batchYear + " and RollNo_Flag<>'0' and cc='0' and DelFlag<>1  and exam_flag <> 'DEBAR' and c.criteria='" + criteria.Trim() + "'   order by c.Criteria_no,c.syll_code desc");
                        if (dtSyll.Rows.Count > 0)
                        {

                            for (int sub = 0; sub < CblSubject.Items.Count; sub++)
                            {
                                if (CblSubject.Items[sub].Selected)
                                {
                                    string subjectCode = Convert.ToString(CblSubject.Items[sub].Value);
                                    dtSyll.DefaultView.RowFilter = "subject_code='" + subjectCode + "'";
                                    DataView dv = dtSyll.DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        string SelectQ = "Select MasterID,subjectNo,CriteriaNo,No_part,PartNo,QNo,sub1,sub2,(select isnull(template,'') from Master_Settings m where settings='COSettings' and cc.CourseOutComeNo=m.masterno) coValue,s.subject_code,s.subject_name,mark from CAQuesSettingsParent cc,subject s where s.subject_no=cc.subjectNo and subjectNo='" + Convert.ToString(dv[0]["subject_no"]) + "' and CriteriaNo='" + Convert.ToString(dv[0]["Criteria_no"]) + "' order by PartNo,QNo";
                                        DataTable dele = dirAcc.selectDataTable(SelectQ);
                                        if (dele.Rows.Count > 0)
                                        {
                                            foreach (DataRow drNew in dele.Rows)
                                            {
                                                drRow = dtReport.NewRow();///DegInfo
                                                drRow["DegInfo"] = batchYear + "-" + DegName;
                                                drRow["SubjectNo"] = Convert.ToString(drNew["subjectNo"]);
                                                drRow["CriteriaNo"] = Convert.ToString(drNew["CriteriaNo"]);
                                                drRow["Subject Name"] = Convert.ToString(drNew["subject_name"]);
                                                drRow["Subject Code"] = Convert.ToString(drNew["subject_code"]);
                                                drRow["NoPart"] = Convert.ToString(drNew["No_part"]);
                                                drRow["PartNo"] = Convert.ToString(drNew["PartNo"]);
                                                drRow["Qno"] = Convert.ToString(drNew["QNo"]);
                                                drRow["Course Outcome"] = Convert.ToString(drNew["coValue"]);
                                                drRow["mark"] = Convert.ToString(drNew["mark"]);
                                                drRow["sub1"] = Convert.ToString(drNew["sub1"]);
                                                drRow["sub2"] = Convert.ToString(drNew["sub2"]);
                                                drRow["Status"] = "1";
                                                dtReport.Rows.Add(drRow);
                                            }
                                        }
                                        else
                                        {

                                            drRow = dtReport.NewRow();
                                            drRow["DegInfo"] = batchYear + "-" + DegName;
                                            drRow["SubjectNo"] = Convert.ToString(dv[0]["subject_no"]);
                                            drRow["CriteriaNo"] = Convert.ToString(dv[0]["Criteria_no"]);
                                            drRow["Subject Name"] = Convert.ToString(dv[0]["subject_name"]);
                                            drRow["Subject Code"] = Convert.ToString(dv[0]["subject_code"]);
                                            drRow["NoPart"] = "";
                                            drRow["PartNo"] = "";
                                            drRow["Qno"] = "";
                                            drRow["Course Outcome"] = "";
                                            drRow["Status"] = "0";
                                            drRow["mark"] = "";
                                            drRow["sub1"] = "";
                                            drRow["sub2"] = "";
                                            dtReport.Rows.Add(drRow);
                                        }
                                    }

                                }
                            }
                        }

                    }
                }
            }

            if (dtReport.Rows.Count > 0)
            {
                GridReport.DataSource = dtReport;
                GridReport.DataBind();
                GridReport.Visible = true;
            }
            else
            {
                lblAlertMsg.Text = "No Subject Were found";
                divPopAlert.Visible = true;
            }
        }
        catch
        {

        }
    }

    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = (e.Row.FindControl("lblStatus") as Label).Text;
                if (status.Trim() == "1" || status.Trim().ToLower() == "true")
                {
                    e.Row.BackColor = Color.MediumSeaGreen;
                }
            }
        }
        catch
        {
        }
    }
    protected void GridReport_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            for (int rowIndex = GridReport.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = GridReport.Rows[rowIndex];
                GridViewRow previousRow = GridReport.Rows[rowIndex + 1];
                string l1 = (row.FindControl("lblSubCode") as LinkButton).Text;
                string l2 = (previousRow.FindControl("lblSubCode") as LinkButton).Text;
                if (l1 == l2)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                           previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;

                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                         previousRow.Cells[3].RowSpan + 1;
                    previousRow.Cells[3].Visible = false;
                }
                string l11 = (row.FindControl("lblDegInfo") as LinkButton).Text;
                string l21 = (previousRow.FindControl("lblDegInfo") as LinkButton).Text;
                if (l11 == l21)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;

                }
            }

            for (int rowIndex = GridReport.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = GridReport.Rows[rowIndex];
                GridViewRow previousRow = GridReport.Rows[rowIndex + 1];
                string l1 = (row.FindControl("lblNOPart") as LinkButton).Text;
                string l2 = (previousRow.FindControl("lblNOPart") as LinkButton).Text;
                if (l1 == l2)
                {
                    row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                           previousRow.Cells[4].RowSpan + 1;
                    previousRow.Cells[4].Visible = false;
                }
            }
        }
        catch
        {
        }
    }
    public void btn_Exit_Click1(object sender, EventArgs e)
    {
        cannot_insert_div.Visible = false;
    }

    private string getPartText(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "1":
                    mark = "Part A";
                    break;
                case "2":
                    mark = "Part B";
                    break;
                case "3":
                    mark = "Part C";
                    break;
                case "4":
                    mark = "Part D";
                    break;
                case "5":
                    mark = "Part E";
                    break;
                case "6":
                    mark = "Part F";
                    break;
                case "7":
                    mark = "Part G";
                    break;
                case "8":
                    mark = "Part H";
                    break;
                case "9":
                    mark = "Part I";
                    break;
                case "10":
                    mark = "Part J";
                    break;
            }
        }
        catch
        {
        }
        return mark;
    }

    private string getSubText1(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "1":
                    mark = "A";
                    break;
                case "2":
                    mark = "B";
                    break;
                case "3":
                    mark = "C";
                    break;
                case "4":
                    mark = "D";
                    break;
                case "5":
                    mark = "E";
                    break;
                case "6":
                    mark = "F";
                    break;
                case "7":
                    mark = "G";
                    break;
                case "8":
                    mark = "H";
                    break;
                case "9":
                    mark = "I";
                    break;
                case "10":
                    mark = "J";
                    break;
            }
        }
        catch
        {
        }
        return mark;
    }

    private string getSubText2(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "1":
                    mark = "i";
                    break;
                case "2":
                    mark = "ii";
                    break;
                case "3":
                    mark = "iii";
                    break;
                case "4":
                    mark = "iv";
                    break;
                case "5":
                    mark = "v";
                    break;
                case "6":
                    mark = "vi";
                    break;
                case "7":
                    mark = "vii";
                    break;
                case "8":
                    mark = "viii";
                    break;
                case "9":
                    mark = "ix";
                    break;
                case "10":
                    mark = "x";
                    break;
            }
        }
        catch
        {
        }
        return mark;
    }

    protected void GridView4_OnDataBound(object sender, EventArgs e)
    {
        for (int rowIndex = GridView4.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = GridView4.Rows[rowIndex];
            GridViewRow previousRow = GridView4.Rows[rowIndex + 1];

            string l1 = (row.FindControl("lblPartgNO") as Label).Text;
            string l2 = (previousRow.FindControl("lblPartgNO") as Label).Text;
            if (l1 == l2)
            {
                row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                       previousRow.Cells[0].RowSpan + 1;
                previousRow.Cells[0].Visible = false;
                row.Cells[1].RowSpan = row.Cells[0].RowSpan;
                previousRow.Cells[1].Visible = false;
                //previousRow.Cells[1].Visible = false;
            }
            string l111 = (row.FindControl("lblgQno") as Label).Text;
            string l211 = (previousRow.FindControl("lblgQno") as Label).Text;
            if (l111 == l211)
            {
                row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                       previousRow.Cells[2].RowSpan + 1;
                previousRow.Cells[2].Visible = false;
                row.Cells[2].RowSpan = row.Cells[2].RowSpan;

                //previousRow.Cells[1].Visible = false;
            }
        }
    }
    protected void btnclosespread_OnClick(object sender, EventArgs e)
    {
        divPopSpread.Visible = false;
    }
    public void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            int update = 0;
            foreach (GridViewRow grid in GridView4.Rows)
            {

                string MasterNo = Convert.ToString((grid.FindControl("lblMasterNo") as Label).Text);
                string coNo = Convert.ToString((grid.FindControl("ddlgUnit") as DropDownList).SelectedValue);
                string mark = Convert.ToString((grid.FindControl("txtgMarks") as TextBox).Text);
                if (!string.IsNullOrEmpty(MasterNo) && !string.IsNullOrEmpty(coNo) && !string.IsNullOrEmpty(coNo))
                {
                    update = da.update_method_wo_parameter("update CAQuesSettingsParent SET CourseOutComeNo='" + coNo + "',mark='" + mark + "' where masterid='" + MasterNo + "'", "text");
                }
            }
            if (update != 0)
            {
                btnReport_Click(sender, e);
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Updated!!";
                divPopAlert.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void lnkAttMark11(object sender, EventArgs e)
    {
        LinkButton lnkSelected = (LinkButton)sender;
        string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxS) - 2;
        //GridReport.Visible = false;
        GridReport.Visible = false;
        GridView1.Visible = false;
        Button1.Visible = false;
        btnSave.Visible = false;
        Button2.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        Button3.Visible = false;
        string suNo = (GridReport.Rows[rowIndx].FindControl("lblSubNo") as Label).Text;
        string SubjectName = (GridReport.Rows[rowIndx].FindControl("lblSubjectName") as LinkButton).Text;
        string SubCode = (GridReport.Rows[rowIndx].FindControl("lblSubCode") as LinkButton).Text;
        string CrNo = (GridReport.Rows[rowIndx].FindControl("lblCriteria") as Label).Text;
        string Status = (GridReport.Rows[rowIndx].FindControl("lblStatus") as Label).Text;

        Dictionary<string, string> ParametersDic = new Dictionary<string, string>();
        ParametersDic.Add("@subno", Convert.ToString(suNo));
        ParametersDic.Add("@CriteriaNO", CrNo);
        DataTable dtSettings = storeAcc.selectDataTable("getCAQuesSettings", ParametersDic);
        DataTable dtCoSett = dirAcc.selectDataTable("select * from Master_Settings where settings='COSettings'");

        DataTable dtMarks = dirAcc.selectDataTable("select * from result where exam_code in(select exam_code from exam_type where criteria_no='" + CrNo + "' and subject_no='" + suNo + "')");
        if (dtMarks.Rows.Count > 0)//dtMarks.Rows.Count > 0
        {
            if (Status.Trim().ToLower() == "true" || Status.Trim() == "1")
            {
                //CREATE TABLE CAQuesSettingsParent (MasterID int identity(1,1) NOT NULL,subjectNo nvarchar(50),CriteriaNo nvarchar(50), No_part int,PartNo numeric(18,0),NO_Ques numeric(18,0),QNo int,Mark float,CourseOutComeNo int,PRIMARY KEY (MasterID)); 

                DataTable dtRec = new DataTable();
                dtRec.Columns.Add("MasterID");
                dtRec.Columns.Add("PartNo");
                dtRec.Columns.Add("PartName");
                dtRec.Columns.Add("CourseOutComeNo");
                dtRec.Columns.Add("NO_Ques");
                dtRec.Columns.Add("QNo");
                dtRec.Columns.Add("sub1");
                dtRec.Columns.Add("sub2");
                dtRec.Columns.Add("Mark");
                DataRow drRow = null;

                if (dtSettings.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSettings.Rows)
                    {
                        drRow = dtRec.NewRow();
                        drRow["MasterID"] = Convert.ToString(dr["MasterID"]);
                        drRow["PartNo"] = Convert.ToString(dr["PartNo"]);
                        drRow["PartName"] = "Part " + Convert.ToString(dr["PartNo"]);
                        drRow["CourseOutComeNo"] = Convert.ToString(dr["CourseOutComeNo"]);
                        drRow["NO_Ques"] = Convert.ToString(dr["NO_Ques"]);
                        drRow["QNo"] = Convert.ToString(dr["QNo"]);
                        drRow["sub1"] = Convert.ToString(dr["sub1"]);
                        drRow["sub2"] = Convert.ToString(dr["sub2"]);
                        drRow["Mark"] = Convert.ToString(dr["Mark"]);
                        dtRec.Rows.Add(drRow);
                        //string masterid=
                    }
                }
                if (dtRec.Rows.Count > 0)
                {
                    GridView4.DataSource = dtRec;
                    GridView4.DataBind();
                    divPopSpread.Visible = true;
                    lblSuName.Text = SubCode + "  - " + SubjectName;
                    foreach (GridViewRow grid in GridView4.Rows)
                    {
                        string MaterNo = Convert.ToString((grid.FindControl("lblCo") as Label).Text);
                        DropDownList dr1 = new DropDownList();
                        TextBox txt = new TextBox();
                        txt = (grid.FindControl("txtgMarks") as TextBox);
                        dr1 = (grid.FindControl("ddlgUnit") as DropDownList);
                        dr1.DataSource = dtCoSett;
                        dr1.DataTextField = "template";
                        dr1.DataValueField = "masterno";
                        dr1.DataBind();
                        dr1.Items.Insert(0, " ");
                        dr1.Items.FindByValue(MaterNo).Selected = true;
                        txt.Enabled = false;
                    }
                }
            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Question Setting Not found!";
                divPopAlert.Visible = true;
            }
        }
        else
        {
            DataTable dtPart = dtSettings.DefaultView.ToTable(true, "No_part");
            DataTable dtPartNo = dtSettings.DefaultView.ToTable(true, "No_part", "partNo");
            DataTable dtPartQno = dtSettings.DefaultView.ToTable(true, "No_part", "partNo", "Qno");
            DataTable dtPartQnoSub1 = dtSettings.DefaultView.ToTable(true, "No_part", "partNo", "Qno", "Sub1");
            DataTable dtPartQnoSub2 = dtSettings.DefaultView.ToTable(true, "No_part", "partNo", "Qno", "Sub1", "sub2");
            if (dtPart.Rows.Count > 0)
            {
                string NoPart = Convert.ToString(dtPart.Rows[0]["No_part"]);
                txtNoPart.Text = NoPart;
                btnGo_Click(sender, e);
                foreach (GridViewRow gr1 in GridPart.Rows)
                {
                    string partNo = (gr1.FindControl("lblPart") as Label).Text;
                    TextBox txt = new TextBox();
                    txt = (gr1.FindControl("txtNoQ") as TextBox);
                    dtPartQno.DefaultView.RowFilter = "partNo='" + partNo + "'";
                    DataView dvQno = dtPartQno.DefaultView;
                    if (dvQno.Count > 0)
                    {
                        txt.Text = Convert.ToString(dvQno.Count);
                    }
                }
                Button1_Click(sender, e);
                foreach (GridViewRow gr1 in GridView2.Rows)
                {

                    string QNo = (gr1.FindControl("lblQNo") as Label).Text;
                    TextBox txt = new TextBox();
                    txt = (gr1.FindControl("txtNoQ1") as TextBox);
                    dtPartQnoSub1.DefaultView.RowFilter = "Qno='" + QNo + "'";
                    DataView dvQno = dtPartQnoSub1.DefaultView;
                    if (dvQno.Count > 0)
                    {
                        txt.Text = Convert.ToString(dvQno.Count);
                    }

                }
                Button2_Click(sender, e);
                foreach (GridViewRow gr1 in GridView3.Rows)
                {

                    string QNo = (gr1.FindControl("lblQNo") as Label).Text;
                    string sub1 = (gr1.FindControl("lblSub1") as Label).Text;
                    TextBox txt = new TextBox();
                    txt = (gr1.FindControl("txtNoQ2") as TextBox);
                    dtPartQnoSub2.DefaultView.RowFilter = "Qno='" + QNo + "' and sub1='" + sub1 + "'";
                    DataView dvQno = dtPartQnoSub2.DefaultView;
                    if (dvQno.Count > 0)
                    {
                        txt.Text = Convert.ToString(dvQno.Count);
                    }

                }
                Button3_Click(sender, e);

                foreach (GridViewRow gr1 in GridView1.Rows)
                {
                    string QNo = (gr1.FindControl("lblgQno") as Label).Text;
                    string sub1 = (gr1.FindControl("lblSub1") as Label).Text;
                    string sub2 = (gr1.FindControl("lblSub2") as Label).Text;
                    string s1 = string.Empty;
                    string s2 = string.Empty;
                    if (!string.IsNullOrEmpty(sub1))
                        s1 = "  and sub1='" + sub1 + "'";
                    if (!string.IsNullOrEmpty(sub2))
                        s2 = "  and sub1='" + sub2 + "'";

                    dtSettings.DefaultView.RowFilter = "Qno='" + QNo + "' " + s1 + " " + s2 + "";
                    TextBox txt = new TextBox();
                    txt = (gr1.FindControl("txtgMarks") as TextBox);
                    DropDownList dr1 = new DropDownList();
                    dr1 = (gr1.FindControl("ddlgUnit") as DropDownList);
                    DataView dvQno = dtSettings.DefaultView;
                    if (dvQno.Count > 0)
                    {
                        txt.Text = Convert.ToString(dvQno[0]["mark"]);//
                        string CoNo = Convert.ToString(dvQno[0]["courseoutcomeno"]);
                        dr1.Items.FindByValue(CoNo).Selected = true;
                    }

                }
                //CblSubject.ClearSelection();
                //CblSubject.Items.FindByValue(SubCode).Selected = true;
                //CblSubject_SelectedIndexChanged(sender, e);
            }

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

    protected void rbInternal_OnCheckedChanged(object sender, EventArgs e)
    {

        GridPart.Visible = false;
        GridReport.Visible = false;
        GridView1.Visible = false;
        Button1.Visible = false;
        btnSave.Visible = false;
        Button2.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        Button3.Visible = false;
        ddlTest.Enabled = true;
        ddlYear.Enabled = false;
        ddlMonth.Enabled = false;
        BindSubject();
    }

    protected void rbExternal_OnCheckedChanged(object sender, EventArgs e)
    {
        GridPart.Visible = false;
        GridReport.Visible = false;
        GridView1.Visible = false;
        Button1.Visible = false;
        btnSave.Visible = false;
        Button2.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        Button3.Visible = false;
        ddlTest.Enabled = false;
        ddlYear.Enabled = true;
        ddlMonth.Enabled = true;
        BindSubject();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        month1();
    }

    public void year1()
    {
        usercode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {

            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            grouporusercode = " and group_code='" + group_user + "'";
        }
        else
        {
            grouporusercode = " and usercode='" + Session["usercode"].ToString().Trim() + "'";
        }
        Boolean setflag = false;
        ddlYear.Items.Clear();
        string getexamvalue = da.GetFunction("select value from master_settings where settings='Exam year and month for Mark' " + grouporusercode + "");//Exam year and month Valuation
        if (getexamvalue.Trim() != null && getexamvalue.Trim() != "" && getexamvalue.Trim() != "0")
        {
            string[] spe = getexamvalue.Split(',');
            if (spe.GetUpperBound(0) == 1)
            {
                if (spe[0].Trim() != "0")
                {
                    ddlYear.Items.Add(new ListItem(Convert.ToString(spe[0]), Convert.ToString(spe[0])));
                    setflag = true;
                }
            }
        }
        //setflag = false;
        if (setflag == false)
        {
            //dsss.Clear();
            DataSet dsss = da.select_method_wo_parameter(" select distinct Exam_year from exam_details order by Exam_year desc", "Text");
            if (dsss.Tables.Count > 0 && dsss.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = dsss;
                ddlYear.DataTextField = "Exam_year";
                ddlYear.DataValueField = "Exam_year";
                ddlYear.DataBind();

            }
        }
        ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
    }

    protected void month1()
    {
        try
        {
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            ddlMonth.Items.Clear();
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                grouporusercode = " and group_code='" + group_user + "'";
            }
            else
            {
                grouporusercode = " and usercode='" + Session["usercode"].ToString().Trim() + "'";
            }
            Boolean setflag = false;
            string monthval = string.Empty;
            string getexamvalue = da.GetFunction("select value from master_settings where settings='Exam year and month for Mark' " + grouporusercode + "");//Exam year and month Valuation
            if (getexamvalue.Trim() != null && getexamvalue.Trim() != "" && getexamvalue.Trim() != "0")
            {
                string[] spe = getexamvalue.Split(',');
                if (spe.GetUpperBound(0) == 1)
                {
                    if (spe[1].Trim() != "0")
                    {
                        string val = spe[1].ToString();
                        monthval = " and Exam_month='" + val + "'";
                    }
                }
            }
            
            string year1 = ddlYear.SelectedValue;
            string strsql = "select distinct Exam_month,upper(convert(varchar(3),DateAdd(month,Exam_month,-1))) as monthName from exam_details where Exam_year='" + year1 + "'" + monthval + " order by Exam_month desc";
            DataSet dsss = da.select_method_wo_parameter(strsql, "Text");
            if (dsss.Tables.Count > 0 && dsss.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = dsss;
                ddlMonth.DataTextField = "monthName";
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataBind();
                ddlMonth.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
        }
    }
}