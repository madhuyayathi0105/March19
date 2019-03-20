using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Drawing;

public partial class CoeMod_StaffPerformanceComparisonReport : System.Web.UI.Page
{
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;
    string selectQuery = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet dsprint = new DataSet();
    ArrayList colord = new ArrayList();
    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
    DataView dvhead = new DataView();
    DataSet dscol = new DataSet();
    Hashtable ht = new Hashtable();
    DataTable dtCommon = new DataTable();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    static byte roll = 0;

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
    string orderBy = string.Empty;
    string orderBySetting = string.Empty;
    string qry = string.Empty;
    string qryCollegeCode = string.Empty;
    string qryCollegeCode1 = string.Empty;
    string qryBatchYear = string.Empty;
    string qryDegreeCode = string.Empty;
    string qrySemester = string.Empty;
    string examYear = string.Empty;
    string qryExamYear = string.Empty;
    string examMonth = string.Empty;
    string qryExamMonth = string.Empty;

    InsproDirectAccess dirAcc = new InsproDirectAccess();
    Dictionary<int, string> dicval = new Dictionary<int, string>();

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
            else
            {
                userCollegeCode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            }
            if (!IsPostBack)
            {
                //ShowReport.Visible = false;
                Bindcollege();
                bindBtch();
                BindDegree();
                bindbranch();
                //BindRightsBasedSectionDetail();
                BindExamYear();
                BindExamMonth();
            }
        }
        catch (Exception ex)
        {
        }

    }

    #region college

    public void Bindcollege()
    {
        try
        {
            divmain.Visible = false;
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

    #endregion

    #region batch
    public void bindBtch()
    {
        try
        {
            divmain.Visible = false;
            cbl_batch.Items.Clear();
            cb_batch.Checked = false;
            txt_batch.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                if (cbl_batch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[i].Selected = true;
                    }
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    cb_batch.Checked = true;
                }
            }
        }
        catch { }
    }

    #endregion


    #region degree

    public void BindDegree()
    {
        divmain.Visible = false;
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

    #endregion

    #region Branch

    public void bindbranch()
    {
        try
        {
            divmain.Visible = false;
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

    #endregion

    #region ExamYear

    public void BindExamYear()
    {
        try
        {
            divmain.Visible = false;
            ddlExamYear.Items.Clear();
            ds.Clear();
            collegeCode = string.Empty;
            batchYear = string.Empty;
            degreeCode = string.Empty;
            qryCollegeCode = string.Empty;
            qryDegreeCode = string.Empty;
            qryBatchYear = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and dg.college_code in (" + collegeCode + ")";
                }
            }
            batchYear = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batchYear == "")
                    {
                        batchYear = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batchYear += "," + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }

            }

            if (!string.IsNullOrEmpty(batchYear))
            {
                qryBatchYear = " and ed.batch_year in(" + batchYear + ")";
            }

            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and ed.degree_code in(" + degreeCode + ")";
                }
            }
            if (!string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qryDegreeCode))
            {
                string qry = "select distinct ed.Exam_year from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_year<>'0' " + qryCollegeCode + qryDegreeCode + qryBatchYear + " order by ed.Exam_year desc";
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
        }
    }

    #endregion

    #region ExamMonth

    private void BindExamMonth()
    {
        try
        {
            divmain.Visible = false;
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            ddlExamMonth.Items.Clear();
            ds.Clear();
            collegeCode = string.Empty;
            batchYear = string.Empty;
            degreeCode = string.Empty;
            qryCollegeCode = string.Empty;
            qryDegreeCode = string.Empty;
            qryBatchYear = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and dg.college_code in (" + collegeCode + ")";
                }
            }
            batchYear = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batchYear == "")
                    {
                        batchYear = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batchYear += "," + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }

            }
            if (!string.IsNullOrEmpty(batchYear))
            {
                qryBatchYear = " and ed.batch_year in(" + batchYear + ")";
            }

            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and ed.degree_code in(" + degreeCode + ")";
                }
            }
            examYear = string.Empty;
            qryExamYear = string.Empty;
            if (ddlExamYear.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem li in ddlExamYear.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(examYear))
                        {
                            examYear = "'" + li.Value + "'";
                        }
                        else
                        {
                            examYear += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(examYear))
                {
                    qryExamYear = " and Exam_year in (" + examYear + ")";
                }
            }
            if (!string.IsNullOrEmpty(qryExamYear) && !string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qryDegreeCode))
            {
                string qry = "select distinct ed.Exam_Month,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1))) as Month_Name from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_Month<>'0' " + qryCollegeCode + qryDegreeCode + qryBatchYear + qryExamYear + " order by Exam_Month";
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
        }
    }

    #endregion


    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindBtch();
            BindDegree();
            bindbranch();
            BindExamYear();
            BindExamMonth();
            divmain.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }


    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_batch.Text = "--Select--";
            if (cb_batch.Checked == true)
            {

                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = true;
                }
                txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                }
            }
            BindDegree();
            bindbranch();
            BindExamYear();
            BindExamMonth();
            divmain.Visible = false;
        }
        catch { }
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_batch.Checked = false;
            int commcount = 0;
            txt_batch.Text = "--Select--";
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
                txt_batch.Text = "Batch(" + commcount.ToString() + ")";
            }
            BindDegree();
            bindbranch();
            BindExamYear();
            BindExamMonth();
            divmain.Visible = false;
        }
        catch { }
    }

    protected void ddlDegree_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            bindbranch();
            BindExamYear();
            BindExamMonth();
            divmain.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlBranch_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            BindExamYear();
            BindExamMonth();
            divmain.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlExamYear_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            BindExamMonth();
            divmain.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlExamMonth_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            divmain.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            string sems = string.Empty;
            string examcode = string.Empty;
            string examcode1 = string.Empty;
            string allbatch = string.Empty;
            dicval.Clear();
            DataTable dt = new DataTable();
            DataRow dr = null;
            Dictionary<int, string> dicval1 = new Dictionary<int, string>();
            Dictionary<string, string> dcv = new Dictionary<string, string>();
            bool norecflag = false;
            dt.Columns.Add("Sno");
            dt.Columns.Add("SubjectName");
            dt.Columns.Add("staffName1");
            dt.Columns.Add("Pass1%");
            dt.Columns.Add("staffName2");
            dt.Columns.Add("Pass2%");
            dt.Columns.Add("staffName3");
            dt.Columns.Add("Pass3%");

            int s = 0;
            #region query

            string qry1 = "select exam_code,current_semester,batch_year,Exam_year,Exam_Month,degree_code from Exam_Details where degree_code='" + Convert.ToString(ddlBranch.SelectedValue) + "' order by batch_year desc";
            qry1 = qry1 + "  select  s.subject_name,semester,sm.Batch_Year,s.subject_no,COUNT (distinct st.staff_code) as staffcount,s.subject_code from syllabus_master sm,subject s,sub_sem ss,staff_selector st where  degree_code='" + Convert.ToString(ddlBranch.SelectedValue) + "'  and sm.syll_code=s.syll_code and ss.subType_no=s.subType_no and sm.syll_code=ss.syll_code and ss.syll_code=s.syll_code and ss.promote_count='1' and st.batch_year=sm.Batch_Year and st.subject_no=s.subject_no group by s.subject_name,semester,sm.Batch_Year,s.subject_no,s.subject_code order by s.subject_name";
            qry1 = qry1 + " select distinct ss.staff_code,sm.staff_name,sm.appl_no,ss.Subject_No from staff_selector ss,staffmaster sm,staff_appl_master sam where sm.staff_code=ss.staff_code and sam.appl_no=sm.appl_no";
            DataSet ds1 = da.select_method_wo_parameter(qry1, "text");

            string qry2 = "select count( distinct m.roll_no) as appear,s.subject_name,m.subject_no,r.Batch_Year,m.exam_code,ss.staff_code,r.degree_code from mark_entry m,subject s,Registration r,staff_selector ss where s.subject_no=m.subject_no and m.roll_no=r.Roll_No and r.degree_code='" + Convert.ToString(ddlBranch.SelectedValue) + "' and ss.subject_no=m.subject_no and s.subject_no=ss.subject_no   group by s.subject_name,m.subject_no,r.Batch_Year,m.exam_code,ss.staff_code,r.degree_code";
            qry2 = qry2 + " select count( distinct m.roll_no) as pass,s.subject_name,m.subject_no,r.Batch_Year,m.exam_code,ss.staff_code,r.degree_code from mark_entry m,subject s,Registration r,staff_selector ss where s.subject_no=m.subject_no and m.roll_no=r.Roll_No and r.degree_code='" + Convert.ToString(ddlBranch.SelectedValue) + "'  and ss.subject_no=m.subject_no and s.subject_no=ss.subject_no and m.result='pass'   group by s.subject_name,m.subject_no,r.Batch_Year,m.exam_code,ss.staff_code,r.degree_code";
            DataSet ds12 = d2.select_method_wo_parameter(qry2, "text");

            #endregion

            string batch = string.Empty;
            int prevbatch1 = 0;
            int prevbatch2 = 0;
            string prebatch = string.Empty;
            for (int j = 0; j < cbl_batch.Items.Count; j++)
            {
                if (cbl_batch.Items[j].Selected == true)
                {
                    batch = Convert.ToString(cbl_batch.Items[j].Text);
                    prevbatch1 = Convert.ToInt32(batch) - 1;
                    prevbatch2 = Convert.ToInt32(batch) - 2;
                    allbatch = "'" + batch + "','" + Convert.ToString(prevbatch1) + "','" + Convert.ToString(prevbatch2) + "'";
                    prebatch = "'" + Convert.ToString(prevbatch1) + "','" + Convert.ToString(prevbatch2) + "'";
                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        ds1.Tables[0].DefaultView.RowFilter = " Exam_year='" + Convert.ToString(ddlExamYear.SelectedItem.Text) + "' and Exam_Month='" + Convert.ToString(ddlExamMonth.SelectedValue) + "' and batch_year ='" + batch + "'";
                        DataView dv = ds1.Tables[0].DefaultView;
                        if (dv.Count > 0)
                        {
                            sems = Convert.ToString(dv[0]["current_semester"]);
                            examcode = Convert.ToString(dv[0]["exam_code"]);

                            string semroman = string.Empty;
                            string yearroman = string.Empty;
                            if (sems.Trim() == "1" || sems.Trim() == "2")
                                yearroman = "1st Year";
                            else if (sems.Trim() == "3" || sems.Trim() == "4")
                                yearroman = "2nd Year";
                            else if (sems.Trim() == "5" || sems.Trim() == "6")
                                yearroman = "3rd Year";
                            else if (sems.Trim() == "7" || sems.Trim() == "8")
                                yearroman = "4th Year";
                            else if (sems.Trim() == "9" || sems.Trim() == "10")
                                yearroman = "5th Year";

                            if (sems.Trim() == "1")
                                semroman = "I";
                            else if (sems.Trim() == "2")
                                semroman = "II";
                            else if (sems.Trim() == "3")
                                semroman = "III";
                            else if (sems.Trim() == "4")
                                semroman = "IV";
                            else if (sems.Trim() == "5")
                                semroman = "v";
                            else if (sems.Trim() == "6")
                                semroman = "VI";
                            else if (sems.Trim() == "7")
                                semroman = "VII";
                            else if (sems.Trim() == "8")
                                semroman = "VIII";
                            else if (sems.Trim() == "9")
                                semroman = "IX";
                            else if (sems.Trim() == "10")
                                semroman = "X";

                            dr = dt.NewRow();
                            dr["Sno"] = yearroman + "-" + semroman + " Semester";
                            dt.Rows.Add(dr);
                            s = dt.Rows.Count;
                            dicval.Add(s, yearroman);

                            dr = dt.NewRow();
                            dr["Sno"] = "S.No";
                            dr["SubjectName"] = "Subject Name";
                            dr["staffName1"] = batch + "-" + (Convert.ToInt32(batch) + 1);
                            dr["Pass1%"] = "";
                            dr["staffName2"] = prevbatch1 + "-" + (Convert.ToInt32(prevbatch1) + 1);
                            dr["Pass2%"] = "";
                            dr["staffName3"] = prevbatch2 + "-" + (Convert.ToInt32(prevbatch2) + 1);
                            dr["Pass3%"] = "";
                            dt.Rows.Add(dr);

                            dr = dt.NewRow();
                            dr["Sno"] = "S.No";
                            dr["SubjectName"] = "Subject Name";
                            dr["staffName1"] = "Staff Name";
                            dr["Pass1%"] = "%";
                            dr["staffName2"] = "Staff Name";
                            dr["Pass2%"] = "%";
                            dr["staffName3"] = "Staff Name";
                            dr["Pass3%"] = "%";
                            dt.Rows.Add(dr);
                        }
                        dcv.Clear();
                        ds1.Tables[1].DefaultView.RowFilter = "  semester='" + sems + "' and Batch_Year  in (" + allbatch + ")";
                        DataView dv2 = ds1.Tables[1].DefaultView;
                        DataTable dt2 = dv2.ToTable();
                        int sno = 0;
                        int currstafct = 0;
                        int stafct = 0;
                        if (dt2.Rows.Count > 0)
                        {
                            int colct = dt.Rows.Count;
                            for (int i = 0; i < dt2.Rows.Count; i++)
                            {
                                string subnam = Convert.ToString(dt2.Rows[i]["subject_name"]);
                                string batchy = Convert.ToString(dt2.Rows[i]["Batch_Year"]);
                                string staffct = Convert.ToString(dt2.Rows[i]["staffcount"]);
                                if (!dcv.ContainsKey(subnam))
                                {
                                    dr = dt.NewRow();
                                    dcv.Add(subnam, staffct);
                                    stafct = Convert.ToInt32(staffct);
                                    sno++;
                                    dr["sno"] = Convert.ToString(sno);
                                    dr["SubjectName"] = Convert.ToString(dt2.Rows[i]["subject_name"]);
                                    dt.Rows.Add(dr);
                                    for (int q = 1; q < Convert.ToInt32(stafct); q++)
                                    {
                                        dr = dt.NewRow();
                                        dr["sno"] = Convert.ToString(sno);
                                        dr["SubjectName"] = Convert.ToString(dt2.Rows[i]["subject_name"]);
                                        dt.Rows.Add(dr);
                                    }

                                }
                                else
                                {
                                    int stc = 0;
                                    if (stafct < Convert.ToInt32(staffct))
                                    {
                                        stc = Convert.ToInt32(staffct) - stafct;
                                    }
                                    if (stc != 0)
                                    {
                                        for (int s1 = 0; s1 < stc; s1++)
                                        {
                                            dr = dt.NewRow();
                                            dr["sno"] = Convert.ToString(sno);
                                            dr["SubjectName"] = Convert.ToString(dt2.Rows[i]["subject_name"]);
                                            dt.Rows.Add(dr);
                                        }
                                    }
                                    stafct = Convert.ToInt32(staffct);
                                }

                            }
                            int m = 0;
                            for (int f = colct; f < dt.Rows.Count; f = f + m)
                            {
                                string[] batspt = allbatch.Split(',');
                                int js = 0;
                                for (int d = 0; d < batspt.Length; d++)
                                {

                                    int subview = 0;
                                    string colhead = "staffName" + (d + 1);
                                    string colhead1 = "pass" + (d + 1) + "%";
                                    string bat = Convert.ToString(batspt[d]);
                                    string subnam = Convert.ToString(dt.Rows[f]["SubjectName"]);
                                    ds1.Tables[1].DefaultView.RowFilter = " subject_name ='" + subnam + "' and batch_year=" + bat + " and semester='" + sems + "'";
                                    DataView dv3 = ds1.Tables[1].DefaultView;
                                    if (dv3.Count > 0)
                                    {
                                        ds1.Tables[0].DefaultView.RowFilter = " current_semester='" + sems + "' and batch_year =" + bat + " and degree_code='" + Convert.ToString(ddlBranch.SelectedValue) + "'";
                                        DataView dv1 = ds1.Tables[0].DefaultView;
                                        if (dv1.Count > 0)
                                        {
                                            examcode1 = Convert.ToString(dv1[0]["exam_code"]);
                                        }
                                        string subno = Convert.ToString(dv3[0]["subject_no"]);
                                        ds1.Tables[2].DefaultView.RowFilter = " Subject_No ='" + subno + "'";
                                        DataView dv4 = ds1.Tables[2].DefaultView;
                                        if (dv4.Count > 0)
                                        {
                                            m = 0;
                                            for (int a = 0; a < dv4.Count; a++)
                                            {
                                                 string totstudapp=string.Empty;
                                                 string passcount = string.Empty;
                                                dt.Rows[f + a]["" + colhead + ""] = Convert.ToString(dv4[a]["staff_name"]);
                                                string staffcode = Convert.ToString(dv4[a]["staff_code"]);
                                                ds12.Tables[0].DefaultView.RowFilter = " degree_code='" + Convert.ToString(ddlBranch.SelectedValue) + "' and Batch_Year=" + bat + "  and exam_code='" + examcode1 + "' and staff_code ='" + staffcode + "' and subject_no='" + subno + "'";
                                                DataView dv6 = ds12.Tables[0].DefaultView;
                                                if (dv6.Count > 0)
                                                    totstudapp = Convert.ToString(dv6[0]["appear"]);
                                                ds12.Tables[1].DefaultView.RowFilter = " degree_code='" + Convert.ToString(ddlBranch.SelectedValue) + "' and Batch_Year=" + bat + "  and exam_code='" + examcode1 + "' and staff_code ='" + staffcode + "' and subject_no='" + subno + "'";
                                                DataView dv7 = ds12.Tables[1].DefaultView;
                                                if (dv6.Count > 0)
                                                    passcount = Convert.ToString(dv7[0]["pass"]);
                                                double passct = 0;
                                                double totstudap = 0;
                                                double.TryParse(totstudapp, out totstudap);
                                                double.TryParse(passcount, out passct);
                                                if (totstudap != 0 && passct != 0)
                                                {
                                                    double passperc = passct / totstudap;
                                                    passperc = passperc * 100;
                                                    passperc = Math.Round(passperc, 0, MidpointRounding.AwayFromZero);
                                                    dt.Rows[f + a]["" + colhead1 + ""] = Convert.ToString(passperc);
                                                }
                                                m++;
                                                subview++;
                                                if (subview > js)
                                                    js = subview;
                                            }
                                        }
                                    }
                                }
                                if (js > m)
                                    m = js;
                            }

                        }

                    }
                    else
                    {
                        divmain.Visible = false;
                        divPopAlert.Visible = true;
                        divPopAlertContent.Visible = true;
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "No Records Found";
                        return;
                    }
                    //if (norecflag == false)
                    //{
                    //    dr = dt.NewRow();
                    //    dr["SubjectName"] = "No Records";
                    //    dt.Rows.Add(dr);
                    //}
                }

            }
            gridview1.Visible = true;
            gridview1.DataSource = dt;
            gridview1.DataBind();
            divmain.Visible = true;
            #region spanning

            Dictionary<int, int> dicval2 = new Dictionary<int, int>();
            if (dicval.Count > 0)
            {
                foreach (KeyValuePair<int, string> dc in dicval)
                {
                    int rw = dc.Key;
                    dicval2.Add(rw, rw);
                    dicval2.Add(rw + 1, rw + 1);
                    dicval2.Add(rw - 1, rw - 1);
                    gridview1.Rows[rw - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    gridview1.Rows[rw - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    gridview1.Rows[rw - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    gridview1.Rows[rw - 1].Font.Bold = true;
                    gridview1.Rows[rw - 1].Font.Bold = true;
                    gridview1.Rows[rw - 1].Font.Bold = true;
                    gridview1.Rows[rw - 1].Cells[0].ColumnSpan = 8;
                    gridview1.Rows[rw - 1].Cells[1].Visible = false;
                    gridview1.Rows[rw - 1].Cells[2].Visible = false;
                    gridview1.Rows[rw - 1].Cells[3].Visible = false;
                    gridview1.Rows[rw - 1].Cells[4].Visible = false;
                    gridview1.Rows[rw - 1].Cells[5].Visible = false;
                    gridview1.Rows[rw - 1].Cells[6].Visible = false;
                    gridview1.Rows[rw - 1].Cells[7].Visible = false;

                    gridview1.Rows[rw].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    gridview1.Rows[rw].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    gridview1.Rows[rw].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    gridview1.Rows[rw].Font.Bold = true;
                    gridview1.Rows[rw].Font.Bold = true;
                    gridview1.Rows[rw].Font.Bold = true;
                    gridview1.Rows[rw].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    gridview1.Rows[rw].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    gridview1.Rows[rw].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                    gridview1.Rows[rw].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                    gridview1.Rows[rw].Cells[2].ColumnSpan = 2;
                    gridview1.Rows[rw].Cells[3].Visible = false;
                    gridview1.Rows[rw].Cells[4].ColumnSpan = 2;
                    gridview1.Rows[rw].Cells[5].Visible = false;
                    gridview1.Rows[rw].Cells[6].ColumnSpan = 2;
                    gridview1.Rows[rw].Cells[7].Visible = false;

                    gridview1.Rows[rw + 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    gridview1.Rows[rw + 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    gridview1.Rows[rw + 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    gridview1.Rows[rw + 1].Font.Bold = true;
                    gridview1.Rows[rw + 1].Font.Bold = true;
                    gridview1.Rows[rw + 1].Font.Bold = true;
                    gridview1.Rows[rw + 1].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    gridview1.Rows[rw + 1].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    gridview1.Rows[rw + 1].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                    gridview1.Rows[rw + 1].Cells[6].HorizontalAlign = HorizontalAlign.Center;

                    gridview1.Rows[rw].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    gridview1.Rows[rw].Font.Bold = true;
                    gridview1.Rows[rw].HorizontalAlign = HorizontalAlign.Center;
                    GridViewRow row = gridview1.Rows[rw];
                    GridViewRow previousRow = gridview1.Rows[rw + 1];
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (i < 2)
                        {
                            if (row.Cells[i].Text == previousRow.Cells[i].Text)
                            {
                                row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                       previousRow.Cells[i].RowSpan + 1;
                                previousRow.Cells[i].Visible = false;
                            }
                        }
                    }
                }
            }
            for (int i = gridview1.Rows.Count - 1; i > 0; i--)
            {
                if (!dicval2.ContainsKey(i))
                {
                    if (i != 0 && i != 1 && i != 2)
                    {
                        GridViewRow row = gridview1.Rows[i];
                        GridViewRow previousRow = gridview1.Rows[i - 1];
                        int rwv = row.RowIndex;
                        int prw = previousRow.RowIndex;
                        for (int j = 0; j < row.Cells.Count; j++)
                        {
                            if (j == 0 || j == 1)
                            {
                                string strsel = (row.FindControl("lblsno") as Label).Text;
                                string presel = (previousRow.FindControl("lblsno") as Label).Text;
                                string strsub = (row.FindControl("lblsubjectname") as Label).Text;
                                string presub = (previousRow.FindControl("lblsubjectname") as Label).Text;
                                //if (row.Cells[j].Text == previousRow.Cells[j].Text)
                                if (strsel == presel || strsub == presub)
                                {
                                    if (previousRow.Cells[j].RowSpan == 0)
                                    {
                                        if (row.Cells[j].RowSpan == 0)
                                        {
                                            previousRow.Cells[j].RowSpan += 2;
                                        }
                                        else
                                        {
                                            previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                        }
                                        row.Cells[j].Visible = false;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            #endregion

        }
        catch
        {
        }
    }

    protected void gridview1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }

    #region PopupAlert

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {

            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divPopAlertContent.Visible = false;
        }
        catch
        {


        }
    }

    #endregion

    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
        string ss = Convert.ToString(Session["usercode"]);
        gridview1.Visible = true;
        Printcontrol.loadspreaddetails(gridview1, "StaffPerformanceComparisonReport.aspx", "Staff Performance Comparison Report", 0, ss);
        Printcontrol.Visible = true;
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.ToString().Trim();
            if (reportname != "")
            {
                d2.printexcelreportgrid(gridview1, reportname);
                lbl_err.Visible = false;
            }
            else
            {
                lbl_err.Text = "Please Enter Your Report Name";
                lbl_err.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }
}