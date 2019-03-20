using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using wc = System.Web.UI.WebControls;
using BalAccess;
using System.Configuration;

public partial class MarkMod_ClassWiseTestMarkStatisticalAnalysis : System.Web.UI.Page
{

    #region Field Declaration

    DAccess2 da = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();

    DataTable dtCommon = new DataTable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();

    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;

    string collegeCode = string.Empty;
    string batchYear = string.Empty;
    string courseId = string.Empty;
    string degreeCode = string.Empty;
    string semester = string.Empty;
    string section = string.Empty;
    string testName = string.Empty;
    string testNo = string.Empty;
    string subjectName = string.Empty;
    string subjectNo = string.Empty;
    string subjectCode = string.Empty;
    string sections = string.Empty;

    string orderBy = string.Empty;
    string orderBySetting = string.Empty;

    string qry = string.Empty;
    string qryCollegeCode = string.Empty;
    string qryCollegeCode1 = string.Empty;
    string qryBatchYear = string.Empty;
    string qryDegreeCode = string.Empty;
    string qrySemester = string.Empty;
    string qrySection = string.Empty;
    string qryCourseId = string.Empty;
    string qrytestNo = string.Empty;
    string qrytestName = string.Empty;
    string qrySubjectNo = string.Empty;
    string qrySubjectName = string.Empty;
    string qrySubjectCode = string.Empty;

    //added by rajasekar 08/10/2018
    DataTable dtl = new DataTable();
    DataRow dtrow = null;
    string[] tagval = new string[30];
    int colcou = 0;
    int colstr = 0;
    int colfalse = 0;

    string valempty = "";
    //============================//

    int selectedCount = 0;

    Institution institute;

    #endregion

    #region Page Load

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
                setLabelText();
                Bindcollege();
                BindBatch();
                BindDegree();
                bindbranch();
                bindsem();
                BindSectionDetail();
                //divStudentDetail.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region bindMethod

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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindBatch()
    {
        try
        {
            ddlBatch.Items.Clear();
            string Master1 = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                Master1 = group.Split(';')[0];
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = Convert.ToString(group_semi[0]).Trim();
                }
            }
            else
            {
                Master1 = Convert.ToString(Session["usercode"]).Trim();
            }
            string collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(Master1.Trim()) && !string.IsNullOrEmpty(collegecode))
            {
                string strbinddegree = "select distinct batch_year from tbl_attendance_rights where college_code='" + collegecode + "'";
                //user_id='" + Master1 + "' and 
                ds = da.select_method_wo_parameter(strbinddegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "Batch_year";
                ddlBatch.DataValueField = "Batch_year";
                ddlBatch.DataBind();
                ddlBatch.SelectedIndex = ddlBatch.Items.Count - 1;
            }
        }
        catch
        {
        }
    }

    public void BindDegree()
    {
        string college_code = Convert.ToString(ddlCollege.SelectedValue).Trim();
        string query = string.Empty;
        ddlDegree.Items.Clear();
        string usercode = Convert.ToString(Session["usercode"]).Trim();
        string singleuser = Convert.ToString(Session["single_user"]).Trim();
        string group_user = Convert.ToString(Session["group_code"]).Trim();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(group_user).Trim() != "0") && (Convert.ToString(group_user).Trim() != "-1"))
        {
            query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "' ";
        }
        else
        {
            query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' ";
        }
        DataSet ds = new DataSet();
        ds.Clear();
        ds = da.select_method_wo_parameter(query, "Text");
        // DataSet ds = ClsAttendanceAccess.GetDegreeDetail(collegecode.ToString());
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlDegree.DataSource = ds;
            ddlDegree.DataValueField = "Course_Id";
            ddlDegree.DataTextField = "Course_Name";
            ddlDegree.DataBind();
            // ddlDegree.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
    }

    public void bindbranch()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.Clear();
            ddlBranch.Items.Clear();
            ht.Clear();
            string usercode = Convert.ToString(Session["usercode"]).Trim();
            string collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            string singleuser = Convert.ToString(Session["single_user"]).Trim();
            string group_user = Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            string course_id = string.Empty;// ddlDegree.SelectedValue.ToString();
            if (ddlDegree.Items.Count > 0)
            {
                course_id = Convert.ToString(ddlDegree.SelectedValue).Trim();
                string query = string.Empty;
                if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(group_user).Trim() != "0") && (Convert.ToString(group_user).Trim() != "-1"))
                {
                    query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id='" + course_id + "' and degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "'";
                }
                else
                {
                    query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id='" + course_id + "' and degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' ";
                }
                ds = da.select_method_wo_parameter(query, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count2 = ds.Tables[0].Rows.Count;
                if (count2 > 0)
                {
                    ddlBranch.DataSource = ds;
                    ddlBranch.DataTextField = "dept_name";
                    ddlBranch.DataValueField = "degree_code";
                    ddlBranch.DataBind();
                }
            }
        }
        catch
        {
        }
    }

    public void bindsem()
    {
        //--------------------semester load
        ddlSem.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        string query = string.Empty;
        DataSet ds = new DataSet();
        if (ddlBatch.Items.Count > 0 && ddlBranch.Items.Count > 0 && Session["collegecode"] != null)
        {
            query = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and batch_year='" + Convert.ToString(ddlBatch.SelectedItem.Text).Trim() + "' and college_code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
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
                    ddlSem.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {

                    ddlSem.Items.Add(i.ToString());
                }
            }
        }
        else
        {
            if (ddlBranch.Items.Count > 0 && Session["collegecode"] != null)
            {
                query = "select distinct duration,first_year_nonsemester  from degree where degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and college_code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'";
                ddlSem.Items.Clear();
                ds = new DataSet();
                ds.Clear();
                ds = da.select_method_wo_parameter(query, "Text");
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
                        ddlSem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSem.Items.Add(i.ToString());
                    }
                }
            }
        }
        if (ddlSem.Items.Count > 0)
        {
            ddlSem.SelectedIndex = 0;
            BindSectionDetail();
        }
        //     ddlSemYr.Items.Insert(0, new ListItem("--Select--", "-1"));
    }

    public void BindSectionDetail()
    {
        DataSet ds = new DataSet();
        ddlSec.Items.Clear();
        if (ddlBranch.Items.Count > 0 && ddlBatch.Items.Count > 0)
        {
            string branch = Convert.ToString(ddlBranch.SelectedValue).Trim();
            string batch = Convert.ToString(ddlBatch.SelectedValue).Trim();
            string query = "select distinct sections from registration where batch_year='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "' and degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlSec.DataSource = ds;
            ddlSec.DataTextField = "sections";
            ddlSec.DataValueField = "sections";
            ddlSec.DataBind();
            ddlSec.Items.Insert(0, new ListItem("All", "0"));
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim() == "")
            {
                ddlSec.Enabled = false;

            }
            else
            {
                ddlSec.Enabled = true;
                //RequiredFieldValidator5.Visible = true;
            }
        }
        else
        {
            ddlSec.Enabled = false;

        }
    }

    public void Get_Semester()
    {
        bool first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;
        ddlSem.Items.Clear();
        //int typeval = 4;
        if (ddlBatch.Items.Count > 0 && ddlBranch.Items.Count > 0 && Session["collegecode"] != null)
        {
            string batch = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
            string collegecode = Convert.ToString(Session["collegecode"]).Trim();
            string degree = Convert.ToString(ddlBranch.SelectedValue).Trim();
            batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();
            //Session["collegecode"].ToString();
            DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                //duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                for (int i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSem.Items.Add(i.ToString());
                    }
                }
                //ddlSemYr.Items.Insert(0, new ListItem("- -Select- -", "-1"));
            }
        }
    }

    public void bindTest()
    {
        try
        {
            string Query = " select distinct Criteria,criteria_no from CriteriaForInternal C,syllabus_Master sy where sy.syll_code=c.syll_code and sy.degree_code ='" + ddlBranch.SelectedValue + "' and sy.batch_year='" + ddlBatch.SelectedItem.Text + "' and sy.semester ='" + ddlSem.SelectedItem.Text + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(Query, "Text");
            //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    ddlTest.DataSource = ds;
            //    ddlTest.DataTextField = "Criteria";
            //    ddlTest.DataValueField = "criteria_no";
            //    ddlTest.DataBind();
            //}

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblTest.DataSource = ds;
                cblTest.DataTextField = "Criteria";
                cblTest.DataValueField = "criteria_no";
                cblTest.DataBind();
            }
        }
        catch
        {

        }
    }

    public void bindSubjects()
    {
        try
        {
            string TestVal = string.Empty;
            string TestValCode = string.Empty;
            string Secval = string.Empty;
            ds.Clear();
            cblSubjectNEW.Items.Clear();
            if (cblTest.Items.Count > 0)
            {
                foreach (ListItem li in cblTest.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(TestVal.Trim()))
                        {
                            TestVal = "'" + li.Text + "'";
                            TestValCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            TestVal += ",'" + li.Text + "'";
                            TestValCode += ",'" + li.Value + "'";
                        }
                    }
                }
            }
            string sectionval = string.Empty;
            if (ddlSec.Enabled == true)
            {
                if (ddlSec.SelectedItem.ToString().ToUpper() == "ALL")
                {
                    if (ddlSec.Items.Count > 0)
                    {
                        for (int i = 0; i < ddlSec.Items.Count; i++)
                        {
                            if (string.IsNullOrEmpty(Secval.Trim()))
                            {
                                Secval = "'" + Convert.ToString((ddlSec.Items[i].ToString().ToUpper() == "ALL") ? string.Empty : ddlSec.Items[i].ToString()) + "'";
                            }
                            else
                            {
                                Secval += ",'" + Convert.ToString((ddlSec.Items[i].ToString().ToUpper() == "ALL") ? string.Empty : ddlSec.Items[i].ToString()) + "'";
                            }
                        }

                    }
                }
                else
                {
                    Secval = "'" + ddlSec.SelectedItem.ToString() + "'";
                }
                sectionval = "  and e.sections In(" + Secval + ")";
            }

            if (!string.IsNullOrEmpty(TestValCode))
            {
                string Query = " SELECT distinct e.subject_no,s.subject_name,ss.subject_type,ss.subType_no,subjectpriority FROM CriteriaForInternal c,Exam_type e,Result re,subject s ,sub_sem ss where ss.subType_no=s.subtype_no and s.subject_no=e.subject_no and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code  and c.criteria_no In (" + TestValCode + ") " + sectionval + " and marks_obtained >=0 order by subjectpriority asc ";

                ds = da.select_method_wo_parameter(Query, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblSubjectNEW.DataSource = ds;
                cblSubjectNEW.DataTextField = "subject_name";
                cblSubjectNEW.DataValueField = "subject_no";
                cblSubjectNEW.DataBind();
            }

        }
        catch
        {

        }
    }

    public void HeaderBind(DataSet dhead)
    {
        try
        {
            int col = 0;
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);

            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);

            dtl.Columns.Add("S.No", typeof(string));

            dtl.Rows[0][col] = "S.No";
            col++;
            dtl.Columns.Add("Subject", typeof(string));

            dtl.Rows[0][col] = "Subject";
            col++;
            dtl.Columns.Add("Test", typeof(string));

            dtl.Rows[0][col] = "Test";
            col++;
            dtl.Columns.Add("Standard", typeof(string));

            dtl.Rows[0][col] = "Standard";
            col++;
            if (ddlDisplayFormat.SelectedValue == "0")
            {
                

                dtl.Columns.Add("No of Students", typeof(string));

                dtl.Rows[0][col] = "No of Students";
                col++;
                

                dtl.Columns.Add("Highest Mark", typeof(string));

                dtl.Rows[0][col] = "Highest Mark";
                col++;

                dtl.Columns.Add("Least Mark", typeof(string));

                dtl.Rows[0][col] = "Least Mark";
                col++;

                dtl.Columns.Add("Subject Average", typeof(string));

                dtl.Rows[0][col] = "Subject Average";
                col++;
            }
            else
            {
                

                dtl.Columns.Add("No of Students", typeof(string));

                dtl.Rows[0][col] = "No of Students";
                col++;


                dtl.Columns.Add("Subject Average", typeof(string));

                dtl.Rows[0][col] = "Subject Average";
                col++;


                dtl.Columns.Add("Highest Mark", typeof(string));

                dtl.Rows[0][col] = "Highest Mark";
                col++;

                dtl.Columns.Add("Least Average", typeof(string));

                dtl.Rows[0][col] = "Least Average";
                col++;
            }

            int ColCount = 0;
            if (ddlDisplayFormat.SelectedValue == "0")
            {
                if (dhead.Tables.Count > 0 && dhead.Tables[0].Rows.Count > 0)
                {
                    for (int intdhead = 0; intdhead < dhead.Tables[0].Rows.Count; intdhead++)
                    {
                        string RangeValue = string.Empty;
                        if (cbGrade.Checked == true)
                        {
                            RangeValue = Convert.ToString(dhead.Tables[0].Rows[intdhead]["Ranges"]) + " (" + Convert.ToString(dhead.Tables[0].Rows[intdhead]["Mark_Grade"]) + ")";
                        }
                        else
                        {
                            RangeValue = Convert.ToString(dhead.Tables[0].Rows[intdhead]["Ranges"]);
                        }
                        dtl.Columns.Add("", typeof(string));
                        ColCount++;
                        if (intdhead == 0)
                        {
                            
                            dtl.Rows[0][col] = "Number of Students";
                           
                        }
                        

                        valempty = valempty + " ";
                        dtl.Columns[dtl.Columns.Count - 1].ColumnName =  Convert.ToString(RangeValue).Trim() + valempty;
                        dtl.Rows[1][col] = Convert.ToString(RangeValue).Trim();
                        col++;
                        tagval[dtl.Columns.Count - 1] = Convert.ToString(dhead.Tables[0].Rows[intdhead]["Ranges"]);

                        if (colstr == 0)
                            colstr = dtl.Columns.Count - 1;

                        colcou++;
                    }
                }
            }
            else
            {
                
                ColCount++;

                

                dtl.Columns.Add("95-100", typeof(string));
                dtl.Rows[0][col] = "Number of Students";
                dtl.Rows[1][col] = "95-100";
                col++;
                tagval[dtl.Columns.Count - 1] = "95-100";
                colstr = dtl.Columns.Count - 1;
                colcou++;

                
                ColCount++;
                

                dtl.Columns.Add("90-94", typeof(string));
                dtl.Rows[1][col] = "90-94";
                col++;
                tagval[dtl.Columns.Count - 1] = "90-94";
                colcou++;
                
                
                ColCount++;
                


                dtl.Columns.Add("75-89", typeof(string));
                dtl.Rows[1][col] = "75-89";
                col++;
                tagval[dtl.Columns.Count - 1] = "75-89";
                colcou++;

                
                ColCount++;
                

                dtl.Columns.Add("60-74", typeof(string));
                dtl.Rows[1][col] = "60-74";
                col++;
                tagval[dtl.Columns.Count - 1] = "60-74";
                colcou++;

                
                ColCount++;
                

                dtl.Columns.Add("40-59", typeof(string));
                dtl.Rows[1][col] = "40-59";
                col++;
                tagval[dtl.Columns.Count - 1] = "40-59";
                colcou++;

                
                ColCount++;
                


                dtl.Columns.Add("Below 40", typeof(string));
                dtl.Rows[1][col] = "Below 40";
                col++;
                tagval[dtl.Columns.Count - 1] = "40";
                colcou++;

                
                ColCount++;
                

                dtl.Columns.Add("Below 35", typeof(string));
                dtl.Rows[1][col] = "Below 35";
                col++;
                tagval[dtl.Columns.Count - 1] = "35";
                colfalse = dtl.Columns.Count - 1;
                
            }

            

            dtl.Columns.Add("No of Students Failed(Below 35) ", typeof(string));
            dtl.Rows[0][col] = "No of Students Failed(Below 35) ";
            col++;
            tagval[dtl.Columns.Count - 1] = "0-0";


            

            dtl.Columns.Add("No of Absentees", typeof(string));
            dtl.Rows[0][col] = "No of Absentees";
            col++;
            tagval[dtl.Columns.Count - 1] = "0-0";

            

            dtl.Columns.Add("Name of the Subject Teacher", typeof(string));
            dtl.Rows[0][col] = "Name of the Subject Teacher";
            col++;
            
        }
        catch
        {

        }
    }

    #endregion

    private DataSet GetSettings()
    {
        DataSet dsSettings = new DataSet();
        try
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
                string Master1 = "select distinct settings,value,ROW_NUMBER() over (ORDER BY settings DESC) as SetValue1,Case when settings='Admission No' then '1' when settings='Register No' then '2' when settings='Roll No' then '3' end as SetValue from Master_Settings where settings in('Roll No','Register No','Admission No') and value='1' " + grouporusercode + "";
                dsSettings = dirAcc.selectDataSet(Master1);
            }
            else
            {
                dsSettings.Clear();
                DataTable dt = new DataTable();
                dt.Columns.Clear();
                dt.Rows.Clear();
                dt.Columns.Add("settings");
                dt.Columns.Add("SetValue");
                dt.Rows.Add("Admission No", "1");
                dt.Rows.Add("Register No", "2");
                dt.Rows.Add("Roll No", "3");
                dsSettings.Tables.Add(dt);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
        return dsSettings;
    }

    #region Index Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string studentApplicationNo = string.Empty;
            ShowReport.Visible = false;

            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            bindTest();

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            ShowReport.Visible = false;

            string studentApplicationNo = string.Empty;

            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            bindTest();

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlDegree_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;

            string studentApplicationNo = string.Empty;

            bindbranch();
            bindsem();
            BindSectionDetail();
            bindTest();

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlBranch_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;

            string studentApplicationNo = string.Empty;

            bindsem();
            BindSectionDetail();
            bindTest();

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlSem_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;

            txtTest.Text = "-- Select --";
            txtSubjectNEW.Text = "-- Select --";
            chkTest.Checked = false;
            chkSubjectNEW.Checked = false;
            chkSubjectNEW_CheckedChangedNEW(sender, e);

            string studentApplicationNo = string.Empty;
            BindSectionDetail();
            bindTest();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlSec_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;

            txtTest.Text = "-- Select --";
            txtSubjectNEW.Text = "-- Select --";
            chkTest.Checked = false;
            chkSubjectNEW.Checked = false;
            chkSubjectNEW_CheckedChangedNEW(sender, e);

            string studentApplicationNo = string.Empty;
            bindTest();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlTest_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;

            string studentApplicationNo = string.Empty;

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkTest_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;


            string studentApplicationNo = string.Empty;

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void cblTest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;


            string studentApplicationNo = string.Empty;

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    

    #endregion Index Changed Events

    #region Button Events

    #region Get Students Details

    #region btngo_OldClick


    #endregion

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            string CollegeCode = ddlCollege.SelectedValue;
            string batchYear = ddlBatch.SelectedItem.Text;
            string degreeCode = ddlBranch.SelectedValue;
            string semester = ddlSem.SelectedItem.Text;
            string sections = string.Empty;
            string TextCode = string.Empty;
            DataView dvstudent = new DataView();
            DataView dvsubject = new DataView();
            DataTable dtsubject = new DataTable();
            DataTable dtSingle = new DataTable();
            DataTable dtSubjectPriority = new DataTable();

            string TestName = string.Empty;

            string TestVal = string.Empty;
            string TestValCode = string.Empty;
            int columnMaxCount = 16;

            if (cblTest.Items.Count > 0)
            {
                foreach (ListItem li in cblTest.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(TestVal.Trim()))
                        {
                            TestVal = "'" + li.Text + "'";
                            TestValCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            TestVal += ",'" + li.Text + "'";
                            TestValCode += ",'" + li.Value + "'";
                        }
                    }
                }
            }
            int sno_valNew = 0;

            string subjectName = string.Empty;
            string subjectNo = string.Empty;

            if (cblSubjectNEW.Items.Count > 0)
            {
                foreach (ListItem li in cblSubjectNEW.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(subjectName.Trim()))
                        {
                            subjectName = "'" + li.Text + "'";
                            subjectNo = "'" + li.Value + "'";
                        }
                        else
                        {
                            subjectName += ",'" + li.Text + "'";
                            subjectNo += ",'" + li.Value + "'";
                        }
                    }
                }
            }

            string GradeRangeQuery = " select gm.College_Code,gm.batch_year,gm.Degree_Code,ISNULL(gm.Semester,'0') as Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,LTRIM(RTRIM(ISNULL(gm.Criteria,''))) as Criteria,gm.classify,CONVERT(Varchar(50),gm.Frange)+' - '+CONVERT(Varchar(50),gm.Trange) as Ranges from Grade_Master gm where gm.batch_year='" + batchYear + "' and gm.College_Code='" + CollegeCode + "' and gm.Degree_Code='" + degreeCode + "' and ISNULL(gm.Semester,'0')='" + semester + "' union select gm.College_Code,gm.batch_year,gm.Degree_Code,ISNULL(gm.Semester,'0') as Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,LTRIM(RTRIM(ISNULL(gm.Criteria,''))) as Criteria,gm.classify,CONVERT(Varchar(50),gm.Frange)+' - '+CONVERT(Varchar(50),gm.Trange) as Ranges from Grade_Master gm where gm.batch_year='" + batchYear + "' and gm.College_Code='" + CollegeCode + "' and gm.Degree_Code='" + degreeCode + "' and ISNULL(gm.Semester,'0')='0'";// order by gm.College_Code,gm.batch_year,gm.Degree_Code,gm.Semester,gm.Criteria,gm.Frange desc ,gm.Trange desc 
            ds.Clear();
            ds = da.select_method_wo_parameter(GradeRangeQuery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //   HeaderBind(ds);
                ds.Tables[0].DefaultView.Sort = "College_Code,batch_year,Degree_Code,Semester,Criteria,Frange desc,Trange desc";
                DataSet dsGradeList = new DataSet();
                dsGradeList.Tables.Add(ds.Tables[0].DefaultView.ToTable());
                HeaderBind(dsGradeList);

                if (ddlSec.Items.Count > 0 && ddlSec.SelectedItem.Text != "All")
                {
                    sections = ddlSec.SelectedItem.Text;
                }
                //if (ddlTest.Items.Count > 0)
                //{
                //    TextCode = ddlTest.SelectedItem.Value;
                //}

                string GetValueQuery = string.Empty;
                // Subject details
                GetValueQuery = " SELECT distinct e.subject_no,s.subject_name,e.Max_Mark,e.Min_Mark,e.sections,ISNULL(ss.isSingleSubject,'0') as isSingleSubject,ss.subject_type,ss.subType_no,isnull(subjectpriority,'0') as subjectpriority,c.criteria_no,c.criteria FROM CriteriaForInternal c,Exam_type e,Result re,subject s ,sub_sem ss where ss.subType_no=s.subtype_no and s.subject_no=e.subject_no and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code  and c.criteria_no IN (" + TestValCode + ") and marks_obtained >=0 and e.subject_no in (" + subjectNo + ")";
                if (sections.Trim() != "")
                {
                    GetValueQuery += "  and e.sections ='" + sections + "' ";
                }
                GetValueQuery += " order by e.sections,subjectpriority asc,c.Criteria_no";
                //No of Student
                GetValueQuery += " SELECT Count(re.roll_no) as Count,e.subject_no,e.sections,c.Criteria_no FROM CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code  and c.criteria_no IN (" + TestValCode + ") and marks_obtained >=0 and e.subject_no in (" + subjectNo + ")";
                if (sections.Trim() != "")
                {
                    GetValueQuery += "  and e.sections ='" + sections + "' ";
                }
                GetValueQuery += " group by e.subject_no,e.sections,c.Criteria_no";

                //Max Mark
                GetValueQuery += " SELECT Max(re.marks_obtained) as Count,e.subject_no,e.sections,c.Criteria_no FROM CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and c.criteria_no IN (" + TestValCode + ") and marks_obtained >=0 and e.subject_no in (" + subjectNo + ")";
                if (sections.Trim() != "")
                {
                    GetValueQuery += "  and e.sections ='" + sections + "' ";
                }
                GetValueQuery += " group by e.subject_no,e.sections,c.Criteria_no";

                //Min Mark
                GetValueQuery += " SELECT Min(re.marks_obtained) as Count,e.subject_no,e.sections,c.Criteria_no FROM CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and c.criteria_no IN (" + TestValCode + ") and marks_obtained >=0 and e.subject_no in (" + subjectNo + ")";
                if (sections.Trim() != "")
                {
                    GetValueQuery += "  and e.sections ='" + sections + "' ";
                }
                GetValueQuery += " group by e.subject_no,e.sections,c.Criteria_no";
                string ConvertionMark = string.Empty;
                if (txt_Convertion.Text.Trim() != "" && txt_Convertion.Text.Trim() != "0")
                {
                    ConvertionMark = Convert.ToString(txt_Convertion.Text.Trim());
                }
                else
                {
                    ConvertionMark = "e.Max_Mark";
                }
                //Average Mark
                GetValueQuery += " SELECT Sum(Round(((Marks_obtained/e.Max_Mark)*(" + ConvertionMark + ")),0)) as Count,Sum(Round(((Marks_obtained/e.Max_Mark)*(e.Max_Mark)),0)) as CountNew,e.subject_no,e.sections,c.Criteria_no FROM CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and c.criteria_no IN (" + TestValCode + ") and marks_obtained >=0 and e.subject_no in (" + subjectNo + ")";
                if (sections.Trim() != "")
                {
                    GetValueQuery += "  and e.sections ='" + sections + "' ";
                }
                GetValueQuery += " group by e.subject_no,e.sections,c.Criteria_no";

                // Range Mark Values 
                GetValueQuery += " SELECT re.roll_no,re.Marks_obtained ,e.subject_no,Round(((Marks_obtained/e.Max_Mark)*(" + ConvertionMark + ")),0) as ConvertMark,Round(((Marks_obtained/e.Max_Mark)*(e.Max_Mark)),0) as ConvertMarkNew,e.sections,c.Criteria_no FROM CriteriaForInternal c,Exam_type e,Result re,subject s where s.subject_no=e.subject_no and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code  and c.criteria_no IN (" + TestValCode + ") and e.subject_no in (" + subjectNo + ") and marks_obtained >=0 ";
                if (sections.Trim() != "")
                {
                    GetValueQuery += "  and e.sections ='" + sections + "' ";
                }
                GetValueQuery += " order by e.sections,c.Criteria_no";
                // Absent Count
                GetValueQuery += " SELECT Count(re.roll_no) as Count,e.subject_no,e.sections,c.Criteria_no FROM CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code  and c.criteria_no IN (" + TestValCode + ") and marks_obtained ='-1' and e.subject_no in (" + subjectNo + ") ";
                if (sections.Trim() != "")
                {
                    GetValueQuery += "  and e.sections ='" + sections + "' ";
                }
                GetValueQuery += " group by e.subject_no,e.sections,c.Criteria_no";
                //Staff Details
                GetValueQuery += " SELECT distinct s.subject_no,s.staff_code,ss.staff_name,LTRIM(RTRIM(isnull(e.sections,''))) sections,su.acronym FROM CriteriaForInternal c,Exam_type e,Result re,staff_selector s,staffMaster ss,Subject su where su.subject_no=s.subject_no and su.subject_no=e.subject_no and s.staff_code=ss.staff_code and s.subject_no=e.subject_no and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code  and c.criteria_no IN (" + TestValCode + ") and s.subject_no in (" + subjectNo + ")  and marks_obtained >=0 and LTRIM(RTRIM(isnull(e.sections,'')))= LTRIM(RTRIM(isnull(s.sections,'')))";
                string qrySections = string.Empty;
                if (sections.Trim() != "")
                {
                    qrySections = "  and LTRIM(RTRIM(isnull(e.sections,''))) ='" + sections + "'";
                    GetValueQuery += "  and LTRIM(RTRIM(isnull(e.sections,''))) ='" + sections + "'";
                }
                ds.Clear();
                ds = da.select_method_wo_parameter(GetValueQuery, "Text");

                //Common therory and lab failed & absentees
                qry = "select COUNT(distinct re.roll_no) as Failed,e.subject_no,LTRIM(RTRIM(isnull(e.sections,''))) sections,ss.subType_no,cast(ss.isSingleSubject as int)as isSingleSubject,cast(ss.Lab as int)as Lab  from CriteriaForInternal c,Exam_type e,Result re,sub_sem ss,subject s where s.syll_code=ss.syll_code and ss.syll_code=c.syll_code and s.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subType_no=ss.subType_no and s.subject_no=e.subject_no  and c.Criteria_no IN (" + TestValCode + ") " + qrySections + " and ISNULL(re.marks_obtained,'0')<e.min_mark and ss.isSingleSubject='0' group by e.subject_no,ss.subType_no,ss.isSingleSubject,ss.Lab,sections;  select COUNT(distinct re.roll_no) as Failed,e.subject_no,ss.subType_no,LTRIM(RTRIM(isnull(e.sections,''))) sections,cast(ss.isSingleSubject as int)as isSingleSubject,cast(ss.Lab as int)as Lab  from CriteriaForInternal c,Exam_type e,Result re,sub_sem ss,subject s where s.syll_code=ss.syll_code and ss.syll_code=c.syll_code and s.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subType_no=ss.subType_no and s.subject_no=e.subject_no and c.Criteria_no  IN (" + TestValCode + ") " + qrySections + " and ISNULL(re.marks_obtained,'0')<'0' and ss.isSingleSubject='0' group by e.subject_no,ss.subType_no,ss.isSingleSubject,ss.Lab,sections";
                DataSet dsFailedCount = da.select_method_wo_parameter(qry, "Text");

                //particular subject of therory and lab failed & absentees
                //string squry = "select COUNT(distinct Tab1.roll_no) as Failed,LTRIM(RTRIM(isnull(Tab1.sections,''))) sections,Tab1.subType_no,cast(Tab1.isSingleSubject as int)as isSingleSubject,cast(Tab1.Lab as int)as Lab from (select distinct re.roll_no,LTRIM(RTRIM(isnull(e.sections,''))) sections,ss.subType_no,cast(ss.isSingleSubject as int)as isSingleSubject,cast(ss.Lab as int)as Lab,SUM(ISNULL(re.marks_obtained,'0')) SecureMarks,SUM(ISNULL(e.min_mark,'0')) as MinPass from CriteriaForInternal c,Exam_type e,Result re,sub_sem ss,subject s where s.syll_code=ss.syll_code and ss.syll_code=c.syll_code and s.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subType_no=ss.subType_no and s.subject_no=e.subject_no and ISNULL(re.marks_obtained,'0')>'0' and ss.isSingleSubject='1' and c.Criteria_no IN (" + TestValCode + ")" + qrySections + "  group by re.roll_no,ss.subType_no,ss.isSingleSubject,ss.Lab,sections) as Tab1 where Tab1.SecureMarks<Tab1.MinPass group by Tab1.subType_no,Tab1.isSingleSubject,Tab1.Lab,sections ";
                //modified
                string squry = "select COUNT(distinct Tab1.roll_no) as Failed,LTRIM(RTRIM(isnull(Tab1.sections,''))) sections,Tab1.subType_no,cast(Tab1.isSingleSubject as int)as isSingleSubject,cast(Tab1.Lab as int)as Lab,Tab1.Criteria_no from (select distinct re.roll_no,LTRIM(RTRIM(isnull(e.sections,''))) sections,ss.subType_no,cast(ss.isSingleSubject as int)as isSingleSubject,c.Criteria_no ,cast(ss.Lab as int)as Lab,SUM(ISNULL(re.marks_obtained,'0')) SecureMarks,SUM(ISNULL(e.min_mark,'0')) as MinPass from CriteriaForInternal c,Exam_type e,Result re,sub_sem ss,subject s where s.syll_code=ss.syll_code and ss.syll_code=c.syll_code and s.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subType_no=ss.subType_no and s.subject_no=e.subject_no and ISNULL(re.marks_obtained,'0')>'0' and ss.isSingleSubject='1' and c.Criteria_no IN (" + TestValCode + ")" + qrySections + "  group by re.roll_no,ss.subType_no,ss.isSingleSubject,ss.Lab,sections,c.Criteria_no) as Tab1 where Tab1.SecureMarks<Tab1.MinPass group by Tab1.subType_no,Tab1.isSingleSubject,Tab1.Lab,sections,Tab1.Criteria_no ";

                qry = "select COUNT(distinct re.roll_no) as Failed,e.subject_no,LTRIM(RTRIM(isnull(e.sections,''))) sections,ss.subType_no,cast(ss.isSingleSubject as int)as isSingleSubject,cast(ss.Lab as int)as Lab  from CriteriaForInternal c,Exam_type e,Result re,sub_sem ss,subject s where s.syll_code=ss.syll_code and ss.syll_code=c.syll_code and s.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subType_no=ss.subType_no and s.subject_no=e.subject_no  and c.Criteria_no IN (" + TestValCode + ") " + qrySections + " and ISNULL(re.marks_obtained,'0')<e.min_mark and ss.isSingleSubject='0' group by e.subject_no,ss.subType_no,ss.isSingleSubject,ss.Lab,sections;  select COUNT(distinct re.roll_no) as absentees,e.subject_no,ss.subType_no,LTRIM(RTRIM(isnull(e.sections,''))) sections,cast(ss.isSingleSubject as int)as isSingleSubject,cast(ss.Lab as int)as Lab  from CriteriaForInternal c,Exam_type e,Result re,sub_sem ss,subject s where s.syll_code=ss.syll_code and ss.syll_code=c.syll_code and s.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subType_no=ss.subType_no and s.subject_no=e.subject_no and c.Criteria_no  IN (" + TestValCode + ") " + qrySections + " and ISNULL(re.marks_obtained,'0')<'0' and ss.isSingleSubject='0' group by e.subject_no,ss.subType_no,ss.isSingleSubject,ss.Lab,sections";
                DataSet dsNewFailed = da.select_method_wo_parameter(qry, "Text");


                DataSet dsFailedCount1 = da.select_method_wo_parameter(squry, "Text");
                if (ds.Tables.Count > 6 && ds.Tables[0].Rows.Count > 0)
                {


                    ds.Tables[0].DefaultView.RowFilter = "isSingleSubject='False' and criteria_no in (" + TestValCode + ")";
                    dtsubject = ds.Tables[0].DefaultView.ToTable();
                    dtSubjectPriority = ds.Tables[0].DefaultView.ToTable(true, "subjectpriority", "subject_no");

                    ds.Tables[0].DefaultView.RowFilter = "isSingleSubject='True' and criteria_no in (" + TestValCode + ")";
                    dtSingle = ds.Tables[0].DefaultView.ToTable();
                    DataTable dtSubType = dtSingle.DefaultView.ToTable(true, "subType_no", "sections", "subject_type");
                    DataTable dtSubPriority = dtSingle.DefaultView.ToTable(true, "subType_no", "subject_type");
                    DataView DvSubTypeSubject = new DataView();
                    DataRow dr;
                    if (dtSubPriority.Rows.Count > 0)
                    {
                        for (int intST = 0; intST < dtSubPriority.Rows.Count; intST++)
                        {
                            dtSingle.DefaultView.RowFilter = "SubType_no='" + Convert.ToString(dtSubPriority.Rows[intST]["SubType_no"]) + "'";
                            DvSubTypeSubject = dtSingle.DefaultView;
                            DvSubTypeSubject.Sort = "subjectpriority asc";
                            if (DvSubTypeSubject.Count > 0)
                            {
                                dr = dtSubjectPriority.NewRow();
                                dr[0] = Convert.ToString(DvSubTypeSubject[0]["subjectpriority"]);
                                dr[1] = Convert.ToString(DvSubTypeSubject[0]["SubType_no"]);
                                dtSubjectPriority.Rows.Add(dr);
                            }
                        }
                    }
                    DvSubTypeSubject = dtSubjectPriority.DefaultView;
                    DvSubTypeSubject.Sort = "subjectpriority asc";
                    dtSubjectPriority = DvSubTypeSubject.ToTable();

                    DataTable dtSingleSubject = dtsubject.DefaultView.ToTable(true, "subject_no");
                    if (dtSubjectPriority.Rows.Count > 0)
                    {
                        int Sno = 0;
                        int ColCount = 0;

                        #region SingleSubject

                        for (int intds = 0; intds < dtSubjectPriority.Rows.Count; intds++)
                        {
                            if (TestValCode.Split(',').Length > 0)
                            {
                                string[] Testarr = TestValCode.Split(',');
                                foreach (string TestCodeNew in Testarr)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = " criteria_no in (" + TestCodeNew + ")";
                                    DataTable dtexamname = ds.Tables[0].DefaultView.ToTable();

                                    if (dtexamname.Rows.Count > 0)
                                        TestName = Convert.ToString(dtexamname.Rows[0]["criteria"]);

                                    ds.Tables[0].DefaultView.RowFilter = "subject_no=" + Convert.ToString(dtSubjectPriority.Rows[intds]["subject_no"]) + "  and criteria_no in (" + TestCodeNew + ")";
                                    dvsubject = ds.Tables[0].DefaultView;


                                     sno_valNew ++;
                                    if (dvsubject.Count > 0)
                                    {
                                        DataTable dtsubjectTable = dvsubject.ToTable();
                                        if (dtsubjectTable.Rows.Count > 0)
                                        {
                                            for (int intSec = 0; intSec < dtsubjectTable.Rows.Count; intSec++)
                                            {
                                                string SectionValue = Convert.ToString(dtsubjectTable.Rows[intSec]["sections"]);
                                                string SecQuery = string.Empty;
                                                if (SectionValue.Trim() != "" && SectionValue.Trim() != "0")
                                                {
                                                    SecQuery = " and sections ='" + SectionValue + "'";
                                                }
                                                //Sno++;
                                                

                                                dtrow = dtl.NewRow();
                                                dtl.Rows.Add(dtrow);

                                                for (int snoval = 0; snoval <  dtsubjectTable.Rows.Count; snoval++)
                                                {
                                                    

                                                    dtl.Rows[dtl.Rows.Count - 1][0] = sno_valNew.ToString();
                                                }

                                                
                                                dtl.Rows[dtl.Rows.Count - 1][1] = Convert.ToString(dtsubjectTable.Rows[intSec]["subject_name"]);



                                                
                                                dtl.Rows[dtl.Rows.Count - 1][2] = TestName;

                                                

                                                dtl.Rows[dtl.Rows.Count - 1][3] = Convert.ToString(ddlBranch.SelectedItem.Text + " - " + SectionValue);


                                                string SubjectNo = Convert.ToString(dtsubjectTable.Rows[intSec]["subject_no"]);
                                                string MaxMark = Convert.ToString(dtsubjectTable.Rows[intSec]["Max_Mark"]);
                                                int NoofStudent = 0;
                                                double TotalSumMark = 0;
                                                double Max = 0;
                                                double.TryParse(MaxMark, out  Max);
                                                double convert = 0;
                                                if (txt_Convertion.Text.Trim() != "" && txt_Convertion.Text.Trim() != "0")
                                                {
                                                    double.TryParse(Convert.ToString(txt_Convertion.Text), out convert);
                                                }
                                                ds.Tables[1].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' " + SecQuery + " and criteria_no in (" + TestCodeNew + ")";
                                                dvstudent = ds.Tables[1].DefaultView;
                                                if (dvstudent.Count > 0)
                                                {
                                                    int.TryParse(Convert.ToString(dvstudent[0]["Count"]), out NoofStudent);
                                                    


                                                    


                                                    dtl.Rows[dtl.Rows.Count - 1][4] = Convert.ToString(NoofStudent);
                                                }
                                                ds.Tables[2].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' " + SecQuery + "";
                                                dvstudent = ds.Tables[2].DefaultView;
                                                if (dvstudent.Count > 0)
                                                {
                                                    if (convert != 0 && Max != 0)
                                                    {
                                                        double HighScore = 0;
                                                        double.TryParse(Convert.ToString(dvstudent[0]["Count"]), out HighScore);
                                                        HighScore = (HighScore / Max) * convert;
                                                        if (ddlDisplayFormat.SelectedValue == "0")
                                                        {
                                                            

                                                            dtl.Rows[dtl.Rows.Count - 1][5] = Convert.ToString(Math.Round(HighScore,0,MidpointRounding.AwayFromZero));
                                                        }
                                                        else
                                                        {
                                                            

                                                            dtl.Rows[dtl.Rows.Count - 1][6] = Convert.ToString(Math.Round(HighScore,0,MidpointRounding.AwayFromZero));
                                                        }
                                                        
                                                    }
                                                    else
                                                    {
                                                        if (ddlDisplayFormat.SelectedValue == "0")
                                                        {
                                                            

                                                            dtl.Rows[dtl.Rows.Count - 1][5] = Convert.ToString(dvstudent[0]["Count"]);
                                                        }
                                                        else
                                                        {
                                                            

                                                            dtl.Rows[dtl.Rows.Count - 1][6] = Convert.ToString(dvstudent[0]["Count"]);
                                                        }
                                                        
                                                    }

                                                }

                                                ds.Tables[3].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' " + SecQuery + "  and criteria_no in (" + TestCodeNew + ")";
                                                dvstudent = ds.Tables[3].DefaultView;
                                                if (dvstudent.Count > 0)
                                                {
                                                    if (convert != 0 && Max != 0)
                                                    {
                                                        double LowScore = 0;
                                                        double.TryParse(Convert.ToString(dvstudent[0]["Count"]), out LowScore);
                                                        LowScore = (LowScore / Max) * convert;
                                                        if (ddlDisplayFormat.SelectedValue == "0")
                                                        {
                                                            

                                                            dtl.Rows[dtl.Rows.Count - 1][6] = Convert.ToString(Math.Round(LowScore,0,MidpointRounding.AwayFromZero));
                                                        }
                                                        else
                                                        {
                                                            

                                                            dtl.Rows[dtl.Rows.Count - 1][7] = Convert.ToString(Math.Round(LowScore,0,MidpointRounding.AwayFromZero));
                                                        }
                                                        
                                                    }
                                                    else
                                                    {
                                                        if (ddlDisplayFormat.SelectedValue == "0")
                                                        {
                                                            

                                                            dtl.Rows[dtl.Rows.Count - 1][6] = Convert.ToString(dvstudent[0]["Count"]);
                                                        }
                                                        else
                                                        {
                                                            

                                                            dtl.Rows[dtl.Rows.Count - 1][7] = Convert.ToString(dvstudent[0]["Count"]);
                                                        }
                                                        
                                                    }
                                                }

                                                // average
                                                ds.Tables[4].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' " + SecQuery + " and criteria_no in (" + TestCodeNew + ")";
                                                dvstudent = ds.Tables[4].DefaultView;

                                                if (dvstudent.Count > 0)
                                                {
                                                    double.TryParse(Convert.ToString(dvstudent[0]["Count"]), out TotalSumMark);
                                                    double AvgMark = 0;
                                                    if (NoofStudent != 0)
                                                    {
                                                        AvgMark = (TotalSumMark / Convert.ToDouble(NoofStudent));
                                                        //AvgMark = (AvgMark / Max) * convert;
                                                    }
                                                    if (ddlDisplayFormat.SelectedValue == "0")
                                                    {
                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][7] = Convert.ToString(Math.Round(AvgMark,0,MidpointRounding.AwayFromZero));
                                                    }
                                                    else
                                                    {
                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][5] = Convert.ToString(Math.Round(AvgMark,0,MidpointRounding.AwayFromZero));
                                                    }

                                                    
                                                }


                                                ColCount = 0; ;
                                                int columnValue = 7;
                                                if (ddlDisplayFormat.SelectedValue == "0")
                                                {
                                                    for (int intCol = 7; intCol < dtl.Columns.Count - 2; intCol++)
                                                    {
                                                        string[] GetTagvalue = tagval[intCol + 1].Split('-');
                                                       
                                                        
                                                        ColCount++;
                                                        columnValue++;
                                                        //Range
                                                        ds.Tables[5].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' and ConvertMark>='" + GetTagvalue[0].ToString().Trim() + "' and ConvertMark <='" + GetTagvalue[1].ToString().Trim() + "' " + SecQuery + " and criteria_no in (" + TestValCode + ")";
                                                        dvstudent = ds.Tables[5].DefaultView;
                                                        if (dvstudent.Count > 0)
                                                        {
                                                           

                                                            dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = Convert.ToString(dvstudent.Count);
                                                        }
                                                        else
                                                        {
                                                            

                                                            dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = "-";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    ds.Tables[5].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' and ConvertMark>='95' and ConvertMark<='100' " + SecQuery + " and criteria_no in (" + TestValCode + ")";
                                                    dvstudent = ds.Tables[5].DefaultView;
                                                    ColCount++;
                                                    columnValue++;
                                                    int intcol = 7;
                                                    if (dvstudent.Count > 0)
                                                    {
                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][8] = Convert.ToString(dvstudent.Count);
                                                    }
                                                    else
                                                    {
                                                       

                                                        dtl.Rows[dtl.Rows.Count - 1][8] = "-";
                                                    }

                                                    ds.Tables[5].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' and ConvertMark>='90' and ConvertMark<='94' " + SecQuery + " and criteria_no in (" + TestValCode + ")";
                                                    dvstudent = ds.Tables[5].DefaultView;
                                                    ColCount++;
                                                    columnValue++;
                                                    intcol++;
                                                    if (dvstudent.Count > 0)
                                                    {
                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][9] = Convert.ToString(dvstudent.Count);
                                                    }
                                                    else
                                                    {
                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][9] = "-";
                                                    }

                                                    ds.Tables[5].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' and ConvertMark>='75' and ConvertMark<='89' " + SecQuery + " and criteria_no in (" + TestValCode + ")";
                                                    dvstudent = ds.Tables[5].DefaultView;
                                                    ColCount++;
                                                    columnValue++;
                                                    intcol++;
                                                    if (dvstudent.Count > 0)
                                                    {
                                                       

                                                        dtl.Rows[dtl.Rows.Count - 1][10] = Convert.ToString(dvstudent.Count);
                                                    }
                                                    else
                                                    {
                                                        
                                                        dtl.Rows[dtl.Rows.Count - 1][10] = "-";
                                                    }

                                                    ds.Tables[5].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' and ConvertMark>='60' and ConvertMark <='74' " + SecQuery + " and criteria_no in (" + TestValCode + ")";
                                                    dvstudent = ds.Tables[5].DefaultView;
                                                    ColCount++;
                                                    columnValue++;
                                                    intcol++;
                                                    if (dvstudent.Count > 0)
                                                    {
                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][11] = Convert.ToString(dvstudent.Count);
                                                    }
                                                    else
                                                    {
                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][11] = "-";
                                                    }

                                                    ds.Tables[5].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' and ConvertMark>='40' and ConvertMark <='59' " + SecQuery + " and criteria_no in (" + TestValCode + ")";
                                                    dvstudent = ds.Tables[5].DefaultView;
                                                    ColCount++;
                                                    columnValue++;
                                                    intcol++;
                                                    if (dvstudent.Count > 0)
                                                    {
                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][12] = Convert.ToString(dvstudent.Count);
                                                    }
                                                    else
                                                    {
                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][12] = "-";
                                                    }

                                                    ds.Tables[5].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' and ConvertMark <'40' " + SecQuery + " and criteria_no in (" + TestValCode + ")";
                                                    dvstudent = ds.Tables[5].DefaultView;
                                                    ColCount++;
                                                    columnValue++;
                                                    intcol++;
                                                    if (dvstudent.Count > 0)
                                                    {
                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][13] = Convert.ToString(dvstudent.Count);
                                                    }
                                                    else
                                                    {
                                                       

                                                        dtl.Rows[dtl.Rows.Count - 1][13] = "-";
                                                    }

                                                    ds.Tables[5].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' and ConvertMark <'35' " + SecQuery + " and criteria_no in (" + TestValCode + ")";
                                                    dvstudent = ds.Tables[5].DefaultView;
                                                    ColCount++;
                                                    columnValue++;
                                                    intcol++;
                                                    if (dvstudent.Count > 0)
                                                    {
                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][14] = Convert.ToString(dvstudent.Count);
                                                    }
                                                    else
                                                    {
                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][14] = "-";
                                                    }
                                                }

                                                DataTable dtTheoryFailedStudent = new DataTable();
                                                DataTable dtTheoryAbsentStudent = new DataTable();

                                                DataTable dtLabFailedStudent = new DataTable();
                                                DataTable dtLabAbsentStudent = new DataTable();

                                                DataTable dt_failed = new DataTable();
                                                DataTable dt_Absent = new DataTable();

                                                string failed_Count = string.Empty;
                                                string Absent_Count = string.Empty;
                                                dsNewFailed.Tables[0].DefaultView.RowFilter = "subject_no='" + SubjectNo + "'" + SecQuery + "";
                                                dt_failed = dsNewFailed.Tables[0].DefaultView.ToTable();

                                                dsNewFailed.Tables[1].DefaultView.RowFilter = "subject_no='" + SubjectNo + "'" + SecQuery + "";
                                                dt_Absent = dsNewFailed.Tables[1].DefaultView.ToTable();


                                                if (dsFailedCount.Tables.Count > 0 && dsFailedCount.Tables[0].Rows.Count > 0)
                                                {
                                                    string failedCnt = string.Empty;  //theory failed
                                                    dsFailedCount.Tables[0].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' and Lab='0'" + SecQuery + "";
                                                    dtTheoryFailedStudent = dsFailedCount.Tables[0].DefaultView.ToTable();

                                                    if (dtTheoryFailedStudent.Rows.Count > 0)
                                                        failedCnt = Convert.ToString(dtTheoryFailedStudent.Rows[0]["Failed"]);
                                                    else
                                                        failedCnt = "-";

                                                    if (ddlDisplayFormat.SelectedValue == "0")
                                                    {
                                                        

                                                      //  dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 6] = failedCnt;
                                                        dtl.Rows[dtl.Rows.Count - 1][16] = failedCnt;
                                                    }
                                                    else
                                                    {
                                                        if (dt_failed.Rows.Count > 0)
                                                        {
                                                            
                                                            dtl.Rows[dtl.Rows.Count - 1][15] = Convert.ToString(dt_failed.Rows[0]["Failed"]);
                                                        }
                                                        else
                                                        {
                                                            

                                                            dtl.Rows[dtl.Rows.Count - 1][15] = "-";
                                                        }
                                                        
                                                    }

                                                    //lab failed
                                                    dsFailedCount.Tables[0].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' and lab='1' " + SecQuery + "";
                                                    dtLabFailedStudent = dsFailedCount.Tables[0].DefaultView.ToTable();
                                                    if (dtLabFailedStudent.Rows.Count > 0)
                                                        failedCnt = Convert.ToString(dtLabFailedStudent.Rows[0]["Failed"]);
                                                    else
                                                        failedCnt = "-";

                                                    if (ddlDisplayFormat.SelectedValue == "0")
                                                    {
                                                        

                                                        //dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 7] = failedCnt;
                                                    }
                                                }
                                                else
                                                {
                                                    if (ddlDisplayFormat.SelectedValue == "0")
                                                    {
                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 6] = "--";

                                                        dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 7] = "--";
                                                    }
                                                }

                                                if (dsFailedCount.Tables.Count > 1 && dsFailedCount.Tables[1].Rows.Count > 0)
                                                {
                                                    string failedCnt = string.Empty;
                                                    dsFailedCount.Tables[1].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' and lab='0' " + SecQuery + " ";
                                                    //theory absent
                                                    dtTheoryAbsentStudent = dsFailedCount.Tables[1].DefaultView.ToTable();

                                                    if (dtTheoryAbsentStudent.Rows.Count > 0)
                                                        failedCnt = Convert.ToString(dtTheoryAbsentStudent.Rows[0]["Failed"]);
                                                    else
                                                        failedCnt = "-";

                                                    if (dt_Absent.Rows.Count > 0)
                                                    {
                                                        Absent_Count = Convert.ToString(dt_Absent.Rows[0]["absentees"]);
                                                    }
                                                    else
                                                    {
                                                        Absent_Count = "-";
                                                    }

                                                    if (ddlDisplayFormat.SelectedValue == "0")
                                                    {
                                                        

                                                        //dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 5] = failedCnt;
                                                        dtl.Rows[dtl.Rows.Count - 1][17] = Absent_Count;
                                                    }
                                                    else
                                                    {
                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][16] = Absent_Count;
                                                    }


                                                    //lab absent
                                                    dsFailedCount.Tables[1].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' and lab='1' " + SecQuery + " ";
                                                    dtLabAbsentStudent = dsFailedCount.Tables[1].DefaultView.ToTable();

                                                    if (dtLabAbsentStudent.Rows.Count > 0)
                                                        failedCnt = Convert.ToString(dtLabAbsentStudent.Rows[0]["Failed"]);
                                                    else
                                                        failedCnt = "-";

                                                    if (ddlDisplayFormat.SelectedValue == "0")
                                                    {
                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 4] = failedCnt;
                                                    }
                                                }
                                                else
                                                {
                                                    if (ddlDisplayFormat.SelectedValue == "0")
                                                    {
                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 4] = "--";

                                                        dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 5] = "--";
                                                    }
                                                }

                                                //Absent
                                                ds.Tables[6].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' " + SecQuery + " and criteria_no in (" + TestCodeNew + ")";
                                                dvstudent = ds.Tables[6].DefaultView;
                                                if (dvstudent.Count > 0)
                                                {
                                                    if (ddlDisplayFormat.SelectedValue == "0")
                                                    {
                                                        


                                                        dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 3] = Convert.ToString(dvstudent[0]["Count"]);
                                                    }

                                                }
                                                else
                                                {
                                                    if (ddlDisplayFormat.SelectedValue == "0")
                                                    {
                                                        
    


                                                        dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 3] = "-";
                                                    }
                                                }
                                                ds.Tables[7].DefaultView.RowFilter = "subject_no='" + SubjectNo + "' " + SecQuery + " ";
                                                dvstudent = ds.Tables[7].DefaultView;
                                                if (dvstudent.Count > 0)
                                                {
                                                    

                                                    dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 1] = Convert.ToString(dvstudent[0]["staff_name"]) + "  (" + Convert.ToString(dvstudent[0]["acronym"]) + ")";
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Combined Subject Details
                                        #region Multiple Subject

                                        dtSubType.DefaultView.RowFilter = "subType_no='" + Convert.ToString(dtSubjectPriority.Rows[intds]["subject_no"]) + "' ";
                                        DataView DvSubTypePriority = dtSubType.DefaultView;
                                        if (DvSubTypePriority.Count > 0)
                                        {
                                            //sno_valNew++;
                                            for (int intd = 0; intd < DvSubTypePriority.Count; intd++)
                                            {
                                                string SectionValue = Convert.ToString(DvSubTypePriority[intd]["sections"]);
                                                string SecQuery = string.Empty;
                                                string Sec = string.Empty;
                                                if (SectionValue.Trim() != "" && SectionValue.Trim() != "0")
                                                {
                                                    SecQuery = " and sections ='" + SectionValue + "'";
                                                    Sec = SectionValue;
                                                }
                                                dtSingle.DefaultView.RowFilter = "subType_no=" + Convert.ToString(DvSubTypePriority[intd]["subType_no"]) + " " + SecQuery + " and criteria_no in (" + TestCodeNew + ")";
                                                dvsubject = dtSingle.DefaultView;

                                                if (dvsubject.Count > 0)
                                                {
                                                    DataTable dtsubjectTable = dvsubject.ToTable();
                                                    StringBuilder SubjectNoStringValue = new StringBuilder();
                                                    if (dtsubjectTable.Rows.Count > 0)
                                                    {
                                                        for (int intS = 0; intS < dtsubjectTable.Rows.Count; intS++)
                                                        {
                                                            SubjectNoStringValue.Append(Convert.ToString(dtsubjectTable.Rows[intS]["subject_no"]) + ",");
                                                        }
                                                        if (SubjectNoStringValue.Length > 0)
                                                        {
                                                            SubjectNoStringValue.Remove(SubjectNoStringValue.Length - 1, 1);
                                                        }
                                                        //Sno++;

                                                        dtrow = dtl.NewRow();
                                                        dtl.Rows.Add(dtrow);

                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][0] = sno_valNew.ToString();


                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][1] = Convert.ToString(dtSubType.Rows[intd]["subject_type"]);

                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][2] = TestName;

                                                        

                                                        dtl.Rows[dtl.Rows.Count - 1][3] = Convert.ToString(ddlBranch.SelectedItem.Text + " - " + Sec);

                                                        string SubjectNo = string.Empty;

                                                        string GetQuery = " SELECT sum(e.Max_Mark) as MaxMark FROM CriteriaForInternal c,Exam_type e where c.Criteria_no=e.criteria_no and c.criteria_no in (" + TestCodeNew + ")  and e.subject_no in (" + SubjectNoStringValue + ")";
                                                        if (Sec.Trim() != "")
                                                        {
                                                            GetQuery += "  and e.sections ='" + Sec + "' ";
                                                        }
                                                        string MaxMark = da.GetFunction(GetQuery);
                                                        int NoofStudent = 0;
                                                        double TotalSumMark = 0;
                                                        double Max = 0;
                                                        double.TryParse(MaxMark, out  Max);
                                                        double convert = 0;
                                                        double.TryParse(Convert.ToString(MaxMark), out Max);
                                                        if (txt_Convertion.Text.Trim() != "" && txt_Convertion.Text.Trim() != "0")
                                                        {
                                                            double.TryParse(Convert.ToString(txt_Convertion.Text), out convert);
                                                        }
                                                        if (convert == 0)
                                                        {
                                                            convert = Max;
                                                        }
                                                        GetValueQuery = " SELECT round((sum(re.marks_obtained)/" + Max + " )*" + convert + ",0) as Count,re.roll_no FROM CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and c.criteria_no in (" + TestCodeNew + ") and marks_obtained >=0 and e.subject_no in (" + SubjectNoStringValue + ")";
                                                        if (Sec.Trim() != "")
                                                        {
                                                            GetValueQuery += "  and LTRIM(RTRIM(isnull(e.sections,''))) ='" + Sec + "' ";
                                                            qrySections = "  and LTRIM(RTRIM(isnull(e.sections,''))) ='" + Sec + "'";
                                                        }
                                                        GetValueQuery += " group by re.roll_no order by sum(re.marks_obtained) desc";
                                                        GetValueQuery += "  SELECT  distinct re.roll_no,Count(distinct e.subject_no)  FROM CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and c.criteria_no in (" + TestCodeNew + ")  and marks_obtained ='-1' and e.subject_no in (" + SubjectNoStringValue + ")";
                                                        //string qrySection = string.Empty;
                                                        if (Sec.Trim() != "")
                                                        {
                                                            //qrySection = "  and e.sections ='" + Sec + "' ";
                                                            GetValueQuery += "  and LTRIM(RTRIM(isnull(e.sections,''))) ='" + Sec + "' ";
                                                        }
                                                        GetValueQuery += " group by re.roll_no having Count(distinct e.subject_no) =" + dtsubjectTable.Rows.Count + "";
                                                        DataSet dsmulti = da.select_method_wo_parameter(GetValueQuery, "Text");
                                                        NoofStudent = 0;
                                                        double MaxSubjectMark = 0;
                                                        double MinSubjectMark = 0;
                                                        double SumSubjectMark = 0;
                                                        double AvgMark = 0;
                                                        if (dsmulti.Tables.Count > 0 && dsmulti.Tables[0].Rows.Count > 0)
                                                        {

                                                            int.TryParse(Convert.ToString(dsmulti.Tables[0].Rows.Count), out NoofStudent);
                                                            

                                                            dtl.Rows[dtl.Rows.Count - 1][4] = Convert.ToString(NoofStudent);

                                                            
                                                            double.TryParse(Convert.ToString(dsmulti.Tables[0].Rows[0][0]), out MaxSubjectMark);
                                                            double.TryParse(Convert.ToString(dsmulti.Tables[0].Rows[dsmulti.Tables[0].Rows.Count - 1][0]), out MinSubjectMark);
                                                            double.TryParse(Convert.ToString(dsmulti.Tables[0].Compute("sum(Count)", "")), out SumSubjectMark);
                                                            AvgMark = (SumSubjectMark / NoofStudent);

                                                            //attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(MaxSubjectMark));

                                                            
                                                            if (ddlDisplayFormat.SelectedValue == "0")
                                                            {
                                                                
                                                                dtl.Rows[dtl.Rows.Count - 1][6] = Convert.ToString(Math.Round(MinSubjectMark,0,MidpointRounding.AwayFromZero));
                                                            }
                                                            else
                                                            {
                                                                

                                                                dtl.Rows[dtl.Rows.Count - 1][7] = Convert.ToString(Math.Round(MinSubjectMark,0,MidpointRounding.AwayFromZero));
                                                            }

                                                            
                                                            if (ddlDisplayFormat.SelectedValue == "0")
                                                            {
                                                                

                                                                dtl.Rows[dtl.Rows.Count - 1][7] = Convert.ToString(Math.Round(AvgMark,0,MidpointRounding.AwayFromZero));
                                                            }
                                                            else
                                                            {


                                                                dtl.Rows[dtl.Rows.Count - 1][5] = Convert.ToString(Math.Round(AvgMark, 0, MidpointRounding.AwayFromZero
));
                                                            }

                                                            
                                                            ColCount = 0;
                                                            if (ddlDisplayFormat.SelectedValue == "0")
                                                            {
                                                                for (int intCol = 7; intCol < dtl.Columns.Count - 2; intCol++)
                                                                {
                                                                    string[] GetTagvalue = tagval[intCol + 1].Split('-');

                                                                    

                                                                    ColCount++;
                                                                    //Range
                                                                    dsmulti.Tables[0].DefaultView.RowFilter = "Count>='" + GetTagvalue[0].ToString().Trim() + "' and Count <='" + GetTagvalue[1].ToString().Trim() + "'";
                                                                    dvstudent = dsmulti.Tables[0].DefaultView;
                                                                    if (dvstudent.Count > 0)
                                                                    {
                                                                        

                                                                        dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = Convert.ToString(dvstudent.Count);
                                                                    }
                                                                    else
                                                                    {
                                                                        

                                                                        dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = "-";
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                dsmulti.Tables[0].DefaultView.RowFilter = "Count>='95' and Count <='100'";
                                                                dvstudent = dsmulti.Tables[0].DefaultView;
                                                                ColCount++;
                                                                int intCol = 7;
                                                                if (dvstudent.Count > 0)
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = Convert.ToString(dvstudent.Count);
                                                                }
                                                                else
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = "-";
                                                                }

                                                                dsmulti.Tables[0].DefaultView.RowFilter = "Count>='90' and Count <='94'";
                                                                dvstudent = dsmulti.Tables[0].DefaultView;
                                                                ColCount++;
                                                                intCol++;
                                                                if (dvstudent.Count > 0)
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = Convert.ToString(dvstudent.Count);
                                                                }
                                                                else
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = "-";
                                                                }

                                                                dsmulti.Tables[0].DefaultView.RowFilter = "Count>='75' and Count <='89'";
                                                                dvstudent = dsmulti.Tables[0].DefaultView;
                                                                ColCount++;
                                                                intCol++;
                                                                if (dvstudent.Count > 0)
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = Convert.ToString(dvstudent.Count);
                                                                }
                                                                else
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = "-";
                                                                }

                                                                dsmulti.Tables[0].DefaultView.RowFilter = "Count>='60' and Count <='74'";
                                                                dvstudent = dsmulti.Tables[0].DefaultView;
                                                                ColCount++;
                                                                intCol++;
                                                                if (dvstudent.Count > 0)
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = Convert.ToString(dvstudent.Count);
                                                                }
                                                                else
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = "-";
                                                                }

                                                                dsmulti.Tables[0].DefaultView.RowFilter = "Count>='40' and Count <='59'";
                                                                dvstudent = dsmulti.Tables[0].DefaultView;
                                                                ColCount++;
                                                                intCol++;
                                                                if (dvstudent.Count > 0)
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = Convert.ToString(dvstudent.Count);
                                                                }
                                                                else
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = "-";
                                                                }

                                                                dsmulti.Tables[0].DefaultView.RowFilter = "Count < '40'";
                                                                dvstudent = dsmulti.Tables[0].DefaultView;
                                                                ColCount++;
                                                                intCol++;
                                                                if (dvstudent.Count > 0)
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = Convert.ToString(dvstudent.Count);
                                                                }
                                                                else
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = "-";
                                                                }

                                                                dsmulti.Tables[0].DefaultView.RowFilter = "Count < '35'";
                                                                dvstudent = dsmulti.Tables[0].DefaultView;
                                                                ColCount++;
                                                                intCol++;
                                                                if (dvstudent.Count > 0)
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = Convert.ToString(dvstudent.Count);
                                                                }
                                                                else
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][intCol + 1] = "-";
                                                                }
                                                            }

                                                            DataTable dtTheoryFailedStudent = new DataTable();
                                                            DataTable dtTheoryAbsentStudent = new DataTable();

                                                            DataTable dtLabFailedStudent = new DataTable();
                                                            DataTable dtLabAbsentStudent = new DataTable();

                                                            //DataTable dt_Failed = new DataTable();
                                                            //DataTable dt_Absent = new DataTable();

                                                            //string Failed_Count = string.Empty;
                                                            //string Absent_Count=string.Empty

                                                            if (dsFailedCount1.Tables.Count > 0 && dsFailedCount1.Tables[0].Rows.Count > 0)
                                                            {
                                                                string failedCnt1 = string.Empty;
                                                                dsFailedCount1.Tables[0].DefaultView.RowFilter = "subType_no='" + Convert.ToString(DvSubTypePriority[intd]["subType_no"]) + "' and Lab='0'" + SecQuery + " and criteria_no in (" + TestCodeNew + ")";
                                                                dtTheoryFailedStudent = dsFailedCount1.Tables[0].DefaultView.ToTable();

                                                                if (dtTheoryFailedStudent.Rows.Count > 0)
                                                                    failedCnt1 = Convert.ToString(dtTheoryFailedStudent.Rows[0]["Failed"]);
                                                                else
                                                                    failedCnt1 = "-";
                                                                if (ddlDisplayFormat.SelectedValue == "0")
                                                                {
                                                                    
                                                                    dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 7] = failedCnt1;
                                                                }
                                                                else
                                                                {
                                                                   

                                                                    dtl.Rows[dtl.Rows.Count - 1][15] = failedCnt1;
                                                                }

                                                                dsFailedCount1.Tables[0].DefaultView.RowFilter = "subType_no='" + Convert.ToString(DvSubTypePriority[intd]["subType_no"]) + "' and lab='1' " + SecQuery + " and criteria_no in (" + TestCodeNew + ")";
                                                                dtLabFailedStudent = dsFailedCount1.Tables[0].DefaultView.ToTable();
                                                                if (dtLabFailedStudent.Rows.Count > 0)
                                                                    failedCnt1 = Convert.ToString(dtLabFailedStudent.Rows[0]["Failed"]);
                                                                else
                                                                    failedCnt1 = "-";
                                                                if (ddlDisplayFormat.SelectedValue == "0")
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 6] = failedCnt1;
                                                                }
                                                                else
                                                                {

                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (ddlDisplayFormat.SelectedValue == "0")
                                                                {
                                                                   

                                                                    dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 6] = "--";

                                                                    dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 7] = "--";
                                                                }
                                                                else
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][15] = "--";
                                                                }
                                                            }
                                                            //qry = "select COUNT(distinct Tab1.roll_no) as Absent,LTRIM(RTRIM(isnull(Tab1.sections,''))) sections,Tab1.subType_no,cast(Tab1.isSingleSubject as int)as isSingleSubject,cast(Tab1.Lab as int)as Lab from (select distinct re.roll_no,LTRIM(RTRIM(isnull(e.sections,''))) sections,ss.subType_no,cast(ss.isSingleSubject as int)as isSingleSubject,cast(ss.Lab as int)as Lab, COUNT(distinct e.subject_no) as FailedSubject from CriteriaForInternal c,Exam_type e,Result re,sub_sem ss,subject s where s.syll_code=ss.syll_code and ss.syll_code=c.syll_code and s.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subType_no=ss.subType_no and s.subject_no=e.subject_no and ISNULL(re.marks_obtained,'0')<'0' and ss.isSingleSubject='1' and c.Criteria_no in (" + TestCodeNew + ")" + qrySections + " group by re.roll_no,ss.subType_no,ss.isSingleSubject,ss.Lab,sections having COUNT(distinct e.subject_no)='" + dtsubjectTable.Rows.Count + "') as Tab1  group by Tab1.subType_no,Tab1.isSingleSubject,Tab1.Lab,sections";
                                                            //modified
                                                            qry = "select COUNT(distinct Tab1.roll_no) as Absent,LTRIM(RTRIM(isnull(Tab1.sections,''))) sections,Tab1.subType_no,cast(Tab1.isSingleSubject as int)as isSingleSubject,cast(Tab1.Lab as int)as Lab,Tab1.Criteria_no from (select distinct re.roll_no,LTRIM(RTRIM(isnull(e.sections,''))) sections,ss.subType_no,cast(ss.isSingleSubject as int)as isSingleSubject,cast(ss.Lab as int)as Lab,c.Criteria_no, COUNT(distinct e.subject_no) as FailedSubject from CriteriaForInternal c,Exam_type e,Result re,sub_sem ss,subject s where s.syll_code=ss.syll_code and ss.syll_code=c.syll_code and s.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subType_no=ss.subType_no and s.subject_no=e.subject_no and ISNULL(re.marks_obtained,'0')<'0' and ss.isSingleSubject='1' and c.Criteria_no in (" + TestCodeNew + ") " + qrySections + " group by re.roll_no,ss.subType_no,ss.isSingleSubject,ss.Lab,sections,c.Criteria_no having COUNT(distinct e.subject_no)='" + dtsubjectTable.Rows.Count + "') as Tab1  group by Tab1.subType_no,Tab1.isSingleSubject,Tab1.Lab,sections,Tab1.Criteria_no";

                                                            DataTable dtAbsentees = new DataTable();
                                                            dtAbsentees = dirAcc.selectDataTable(qry);
                                                            if (dtAbsentees.Rows.Count > 0)
                                                            {
                                                                string failedCnt1 = string.Empty;
                                                                dtAbsentees.DefaultView.RowFilter = "subType_no='" + Convert.ToString(DvSubTypePriority[intd]["subType_no"]) + "' and Lab='0'" + SecQuery + " and criteria_no in (" + TestCodeNew + ")";
                                                                dtTheoryAbsentStudent = dtAbsentees.DefaultView.ToTable();
                                                                if (dtTheoryAbsentStudent.Rows.Count > 0)
                                                                    failedCnt1 = Convert.ToString(dtTheoryAbsentStudent.Rows[0]["Absent"]);
                                                                else
                                                                    failedCnt1 = "-";
                                                                if (ddlDisplayFormat.SelectedValue == "0")
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 5] = failedCnt1;
                                                                }
                                                                else
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][16] = failedCnt1;
                                                                }


                                                                dtAbsentees.DefaultView.RowFilter = "subType_no='" + Convert.ToString(DvSubTypePriority[intd]["subType_no"]) + "' and lab='1' " + SecQuery + " and criteria_no in (" + TestCodeNew + ")";
                                                                dtLabAbsentStudent = dtAbsentees.DefaultView.ToTable();
                                                                if (dtLabAbsentStudent.Rows.Count > 0)
                                                                    failedCnt1 = Convert.ToString(dtLabAbsentStudent.Rows[0]["Absent"]);
                                                                else
                                                                    failedCnt1 = "-";
                                                                if (ddlDisplayFormat.SelectedValue == "0")
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 4] = failedCnt1;
                                                                }
                                                                else
                                                                {

                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (ddlDisplayFormat.SelectedValue == "0")
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 4] = "--";

                                                                    dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 5] = "--";
                                                                }
                                                                else
                                                                {
                                                                   

                                                                    dtl.Rows[dtl.Rows.Count - 1][16] = "--";
                                                                }
                                                            }

                                                            if (dsmulti.Tables[1].Rows.Count > 0)
                                                            {
                                                                if (ddlDisplayFormat.SelectedValue == "0")
                                                                {
                                                                   

                                                                    dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 3] = Convert.ToString(dsmulti.Tables[1].Rows.Count);
                                                                }
                                                                else
                                                                {
                                                                    //attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1,15].Text = Convert.ToString(dsmulti.Tables[1].Rows.Count);

                                                                    
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (ddlDisplayFormat.SelectedValue == "0")
                                                                {
                                                                    

                                                                    dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 3] = "-";
                                                                }
                                                                else
                                                                {
                                                                    //attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 15].Text = "-";

                                                                    
                                                                }
                                                            }

                                                            ds.Tables[7].DefaultView.RowFilter = "subject_no in (" + SubjectNoStringValue + ") " + SecQuery + "";
                                                            DataView dvs = ds.Tables[7].DefaultView;
                                                            if (dvs.Count > 0)
                                                            {
                                                                SubjectNoStringValue = new StringBuilder();
                                                                for (int intS = 0; intS < dvs.Count; intS++)
                                                                {
                                                                    SubjectNoStringValue.Append(Convert.ToString(dvs[intS]["staff_name"]) + "  (" + Convert.ToString(dvs[intS]["acronym"]) + ")" + ",");
                                                                }
                                                                if (SubjectNoStringValue.Length > 0)
                                                                {
                                                                    SubjectNoStringValue.Remove(SubjectNoStringValue.Length - 1, 1);
                                                                }

                                                                


                                                                dtl.Rows[dtl.Rows.Count - 1][dtl.Columns.Count - 1] = Convert.ToString(SubjectNoStringValue);
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                        #endregion

                                    }
                                }
                        #endregion

                                ShowReport.Visible = true;
                                


                            
                            }
                            //else
                            //{
                            //    divPopAlert.Visible = true;
                            //    ShowReport.Visible = false;
                            //    lblAlertMsg.Text = "No Records Found";
                            //}
                        }

                        if (dtl.Rows.Count > 0)
                        {

                            if (colfalse != 0)
                            {
                                dtl.Columns.RemoveAt(colfalse);
                                colfalse = 0;
                            }
                            Showgrid.DataSource = dtl;
                            Showgrid.DataBind();
                            Showgrid.Visible = true;
                            Showgrid.HeaderRow.Visible = false;
                            Showgrid.Width = 1500;

                            int dtrowcount = dtl.Rows.Count;
                            int rowspanstart0 = 0;
                            int rowspanstart1 = 0;
                            int rowspanstart2 = 0;





                            for (int i = 0; i < Showgrid.Rows.Count; i++)
                            {


                                int rowspancount0 = 0;
                                int rowspancount1 = 0;
                                int rowspancount2 = 0;


                                if (i != dtrowcount - 1)
                                {
                                    if (rowspanstart0 == i)
                                    {
                                        for (int k = rowspanstart0 + 1; Showgrid.Rows[i].Cells[0].Text == Showgrid.Rows[k].Cells[0].Text; k++)
                                        {
                                            rowspancount0++;
                                            if (k == dtrowcount - 1)
                                                break;
                                        }
                                        rowspanstart0++;
                                    }
                                    if (rowspanstart1 == i)
                                    {
                                        for (int k = rowspanstart1 + 1; Showgrid.Rows[i].Cells[1].Text == Showgrid.Rows[k].Cells[1].Text; k++)
                                        {
                                            rowspancount1++;
                                            if (k == dtrowcount - 1)
                                                break;
                                        }
                                        rowspanstart1++;
                                    }
                                    if (rowspanstart2 == i)
                                    {
                                        for (int k = rowspanstart2 + 1; Showgrid.Rows[i].Cells[2].Text == Showgrid.Rows[k].Cells[2].Text; k++)
                                        {
                                            rowspancount2++;
                                            if (k == dtrowcount - 1)
                                                break;
                                        }
                                        rowspanstart2++;
                                    }


                                    if (rowspancount0 != 0)
                                    {
                                        rowspanstart0 = rowspanstart0 + rowspancount0;

                                        Showgrid.Rows[i].Cells[0].RowSpan = rowspancount0 + 1;
                                        for (int a = i; a < rowspanstart0 - 1; a++)
                                            Showgrid.Rows[a + 1].Cells[0].Visible = false;

                                    }
                                    if (rowspancount1 != 0)
                                    {
                                        rowspanstart1 = rowspanstart1 + rowspancount1;

                                        Showgrid.Rows[i].Cells[1].RowSpan = rowspancount1 + 1;
                                        for (int a = i; a < rowspanstart1 - 1; a++)
                                            Showgrid.Rows[a + 1].Cells[1].Visible = false;

                                    }

                                    if (rowspancount2 != 0)
                                    {
                                        rowspanstart2 = rowspanstart2 + rowspancount2;

                                        Showgrid.Rows[i].Cells[2].RowSpan = rowspancount2 + 1;
                                        for (int a = i; a < rowspanstart2 - 1; a++)
                                            Showgrid.Rows[a + 1].Cells[2].Visible = false;

                                    }





                                }

                                for (int j = 0; j < dtl.Columns.Count; j++)
                                {

                                    if (i == 0 || i == 1)
                                    {
                                        Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                        Showgrid.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                        Showgrid.Rows[i].Cells[j].Font.Bold = true;

                                        if (i == 0)
                                        {
                                            if (j == colstr)
                                            {
                                                Showgrid.Rows[i].Cells[j].ColumnSpan = colcou;
                                                for (int a = j + 1; a < colstr + colcou; a++)
                                                    Showgrid.Rows[i].Cells[a].Visible = false;

                                            }
                                            else if (j < colstr || j >= colstr + colcou)
                                            {
                                                Showgrid.Rows[i].Cells[j].RowSpan = 2;
                                                for (int a = i; a < 1; a++)
                                                    Showgrid.Rows[a + 1].Cells[j].Visible = false;
                                            }

                                        }
                                        else if (j >= colstr && j < colstr + colcou)
                                        {
                                            Showgrid.Rows[i].Cells[j].Width = 50;

                                        }

                                    }
                                    else
                                    {
                                        if (j != dtl.Columns.Count - 1)
                                        {
                                            Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                                            if (j >= colstr && j < colstr + colcou)
                                            {
                                                Showgrid.Rows[i].Cells[j].Width = 50;

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
                else
                {
                    divPopAlert.Visible = true;
                    ShowReport.Visible = false;
                    lblAlertMsg.Text = "No Records Found";
                }

            }
            else
            {
                divPopAlert.Visible = true;
                ShowReport.Visible = false;
                lblAlertMsg.Text = "Please Set Mark or Grade Range";
            }
        }
        catch (Exception ex)
        {
           
        }
    }

    public void Footer()
    {
        try
        {
            
        }
        catch
        {
        }
    }

    public void OldCode()
    {
        try
        {
         
        }
        catch
        {

        }
    }

    #endregion

    #region Alert Popup Close

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

    #endregion

    #region Confirmation Yes/No Click

    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirmBox.Visible = false;
        }
        catch
        {
        }
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirmBox.Visible = false;
        }
        catch
        {
        }
    }

    #endregion

    #endregion

    #region Reusable Method

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
                //spPageHeading.InnerHtml = "Student's Previous Test Report";
                //Page.Title = "Student's Previous Test Report";
            }
            else
            {
                lblBatch.Text = "Batch";
                //spPageHeading.InnerHtml = "Student's Previous CAM Report";
                //Page.Title = "Student's Previous CAM Report";
            }
            new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    private bool CheckSchoolOrCollege(string collegeCode)
    {
        bool isSchoolOrCollege = false;
        try
        {
            if (!string.IsNullOrEmpty(collegeCode))
            {
                //qry = "select ISNULL(InstType,'0') as InstType,case when ISNULL(InstType,'0')='0' then 'College' when ISNULL(InstType,'0')='1' then 'School' end as CollegeOrSchool from collinfo where college_code='" + collegeCode + "'";
                string qry = "select ISNULL(InstType,'0') as InstType from collinfo where college_code='" + collegeCode + "'";
                string insType = dirAcc.selectScalarString(qry).Trim();
                if (string.IsNullOrEmpty(insType) || insType.Trim() == "0")
                {
                    isSchoolOrCollege = false;
                }
                else if (!string.IsNullOrEmpty(insType) && insType.Trim() == "1")
                {
                    isSchoolOrCollege = true;
                }
                else
                {
                    isSchoolOrCollege = false;
                }
            }
            return isSchoolOrCollege;
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
            return false;
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
            lblErrSearch.Text = Convert.ToString(ex).Trim();
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
        return null;
    }

    private string orderByStudents(string collegeCode, string aliasName = null, string tableName = null)
    {
        string orderBy = string.Empty;
        try
        {
            string orderBySetting = dirAcc.selectScalarString("select value from master_Settings where settings='order_by' ");//and value<>''
            orderBySetting = orderBySetting.Trim();

            string serialNo = dirAcc.selectScalarString("select LinkValue from inssettings where college_code='" + collegeCode + "' and linkname='Student Attendance'");

            string aliasOrTableName = ((string.IsNullOrEmpty(aliasName) && string.IsNullOrEmpty(tableName)) ? "" : ((!string.IsNullOrEmpty(tableName)) ? tableName.Trim() + "." : ((!string.IsNullOrEmpty(aliasName)) ? aliasName.Trim() + "." : "")));

            orderBy = "ORDER BY " + aliasOrTableName + "roll_no";
            if (serialNo.Trim().ToLower() == "1" || serialNo.ToLower().Trim() == "true")
                orderBy = "ORDER BY " + aliasOrTableName + "serialno";
            else
                switch (orderBySetting)
                {
                    case "0":
                        orderBy = "ORDER BY " + aliasOrTableName + "roll_no";
                        break;
                    case "1":
                        orderBy = "ORDER BY " + aliasOrTableName + "Reg_No";
                        break;
                    case "2":
                        orderBy = "ORDER BY " + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,1,2":
                        orderBy = "ORDER BY " + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No," + aliasOrTableName + "stud_name";
                        break;
                    case "0,1":
                        orderBy = "ORDER BY " + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No";
                        break;
                    case "1,2":
                        orderBy = "ORDER BY " + aliasOrTableName + "Reg_No," + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,2":
                        orderBy = "ORDER BY " + aliasOrTableName + "roll_no," + aliasOrTableName + "Stud_Name";
                        break;
                    default:
                        orderBy = "ORDER BY " + aliasOrTableName + "roll_no";
                        break;
                }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex).Trim();
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
        return orderBy;
    }

    /// <summary>
    /// Developed By Malang Raja on Dec 7 2016
    /// </summary>
    /// <param name="type">0 For Roll No,1 For Register No,2 For Admission No, 3 For Student Type , 4 For Application No</param>
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
                    string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type','Application No') and value='1' " + grouporusercode + "";
                    dsSettings = dirAcc.selectDataSet(Master1);
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
                        case 4:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "application no")
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
            return false;
        }
    }

    private string getMarkText(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "-1":
                    mark = "Ab";
                    break;
                case "-2":
                    mark = "EL";
                    break;
                case "-3":
                    mark = "EOD";
                    break;
                case "-4":
                    mark = "ML";
                    break;
                case "-5":
                    mark = "SOD";
                    break;
                case "-6":
                    mark = "NSS";
                    break;
                case "-7":
                    mark = "NJ";
                    break;
                case "-8":
                    mark = "S";
                    break;
                case "-9":
                    mark = "L";
                    break;
                case "-10":
                    mark = "NCC";
                    break;
                case "-11":
                    mark = "HS";
                    break;
                case "-12":
                    mark = "PP";
                    break;
                case "-13":
                    mark = "SYOD";
                    break;
                case "-14":
                    mark = "COD";
                    break;
                case "-15":
                    mark = "OOD";
                    break;
                case "-16":
                    mark = "OD";
                    break;
                case "-17":
                    mark = "LA";
                    break;
                case "-18":
                    mark = "RAA";
                    break;
            }
        }
        catch
        {
        }
        return mark;
    }

    private void GetSubjectGrade()
    {
        try
        {

        }
        catch
        {
        }
    }

    #endregion

    #region Print

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                
                da.printexcelreportgrid(Showgrid, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }
    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string Acrdemicyear = da.GetFunction("select value from master_settings where settings='Academic year'");

            string[] split = Acrdemicyear.Split(',');
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string Calss = "Branch";
            string Section = string.Empty;
            string AddSection = string.Empty;
            if (ddlSec.Items.Count > 0 && ddlSec.SelectedItem.Text != "All")
            {
                Section = ddlSec.SelectedItem.Text;
                AddSection += " and sections='" + Section + "'";
            }
            if (lblCollege.Text.Trim().ToLower() == "school")
            {
                Calss = "Class";
            }
            string classAdv = da.GetFunction("select class_advisor from Semester_Schedule  where batch_year='" + ddlBatch.SelectedItem.Text + "' and degree_code='" + ddlBranch.SelectedValue + "' and semester='" + ddlSem.SelectedItem.Text + "'" + AddSection);
            string Total = da.GetFunction(" select count(r.roll_no)as Count from registration r where cc=0 and delflag=0 and exam_flag<>'DEBAR' and  degree_code ='" + ddlBranch.SelectedValue + "' and college_code ='" + ddlCollege.SelectedValue + "' and batch_year='" + ddlBatch.SelectedItem.Text + "' and current_semester='" + ddlSem.SelectedItem.Text + "' " + AddSection + "");
            string degreedetails;
            string pagename;
            degreedetails = getCblSelectedText(cblTest) + "$" + "" + split[0].ToString().Trim() + " - " + split[1].ToString().Trim() + " " + "$" + "STATISTICAL ANALYSIS" + "$" + "" + Calss + " - " + ddlBranch.SelectedItem.Text + "  " + Section + " @ NAME OF THE CLASS TEACHER:" + classAdv + " @ No on Roll: " + Total + " ";
            //+ '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "ClassWiseTestMarkStatisticalAnalysis.aspx";
            degreedetails = "Class Wise Test Mark Statictical Analysis";
            string ss = Session["usercode"].ToString();
            Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
            Printcontrol.Visible = true;

            //string Course_Name = Convert.ToString(ddlDegree.SelectedItem).Trim();
            //rptheadname += "$" + ((ddlTest.Items.Count > 0) ? ddlTest.SelectedItem.Text : "") + "$ SUBJECT : " + ((ddlSubject.Items.Count > 0) ? Convert.ToString(ddlSubject.SelectedItem.Text).Trim() : "") + "@ " + Course_Name + " - " + Convert.ToString(ddlBranch.SelectedItem).Trim() + "@ " + " Year of Admission : " + Convert.ToString(ddlBatch.SelectedItem).Trim() + "@ " + " " + lblSem.Text.Trim() + " : " + Convert.ToString(ddlSem.SelectedItem).Trim();
        }
        catch { }
    }

    #endregion

    #region  MultiSelect in Test and Subject added By Prabha on Nov 13 2017

    protected void ddlTest_SelectedIndexChangedNEW(object sender, EventArgs e)
    {
        //ddlSubjectNEW.Items.Clear();
    }

    protected void chkTest_CheckedChangedNEW(object sender, EventArgs e)
    {
        int count = 0;
        if (chkTest.Checked == true)
        {
            count++;
            for (int i = 0; i < cblTest.Items.Count; i++)
            {
                cblTest.Items[i].Selected = true;
            }
            txtTest.Text = "Test (" + (cblTest.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblTest.Items.Count; i++)
            {
                cblTest.Items[i].Selected = false;
            }
            txtTest.Text = "-- Select --";
        }
        bindSubjects();

    }

    protected void cbltest_SelectedIndexChangedNEW(object sender, EventArgs e)
    {
        int commcount = 0;
        txtTest.Text = "-- Select --";
        chkTest.Checked = false;
        for (int i = 0; i < cblTest.Items.Count; i++)
        {
            if (cblTest.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cblTest.Items.Count)
            {
                chkTest.Checked = true;
            }
            txtTest.Text = "Test (" + Convert.ToString(commcount) + ")";
        }
        bindSubjects();
    }

    protected void ddlSubjectNEW_SelectedIndexChangedNEW(object sender, EventArgs e)
    {

    }

    protected void chkSubjectNEW_CheckedChangedNEW(object sender, EventArgs e)
    {
        int count = 0;
        if (chkSubjectNEW.Checked == true)
        {
            count++;
            for (int i = 0; i < cblSubjectNEW.Items.Count; i++)
            {
                cblSubjectNEW.Items[i].Selected = true;
            }
            txtSubjectNEW.Text = "Subject (" + (cblSubjectNEW.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblSubjectNEW.Items.Count; i++)
            {
                cblSubjectNEW.Items[i].Selected = false;
            }
            txtSubjectNEW.Text = "-- Select --";
        }
    }

    protected void cblSubjectNEW_SelectedIndexChangedNEW(object sender, EventArgs e)
    {
        int commcount = 0;
        txtSubjectNEW.Text = "-- Select --";
        chkSubjectNEW.Checked = false;
        for (int i = 0; i < cblSubjectNEW.Items.Count; i++)
        {
            if (cblSubjectNEW.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cblSubjectNEW.Items.Count)
            {
                chkSubjectNEW.Checked = true;
            }
            txtSubjectNEW.Text = "Subject (" + Convert.ToString(commcount) + ")";
        }
    }

    public void btnPrint11()
    {
        DAccess2 d2 = new DAccess2();
        string college_code = Convert.ToString(ddlCollege.SelectedValue);
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
        spReportName.InnerHtml = "Class Wise Test Mark Statistical Analysis";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }

    
    public override void VerifyRenderingInServerForm(Control control)
    { }
    #endregion

}