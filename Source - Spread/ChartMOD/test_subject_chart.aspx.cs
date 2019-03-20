using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.Configuration;

public partial class test_subject_chart : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet syllbus = new DataSet();
    DataSet syllbus1 = new DataSet();
    Hashtable ht = new Hashtable();
    DAccess2 da = new DAccess2();
    string usercode = "";
    string collegecode = "";
    string singleuser = "";
    string group_user = "";
    int ddlcount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("ChartMaster"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/ChartMOD/ChartMaster.aspx");
                return;
            }
        }
        //****************************************************//
        if (!IsPostBack)
        {
            rdTestWise.Checked = true;
            lblErrTest.Visible = false;
            college();
            bindyear();
            bindcourse();
            bindbranch(collegecode);
            bindsem();
            BindSectionDetail();
            test();
        }
    }
    public void college()
    {
        try
        {
            ddlcollege.Items.Insert(0, "All");
            ds = da.select_method_wo_parameter("select collname,college_code,acr from collinfo", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch
        {
        }
    }
    public void test()
    {
        try
        {
            string SyllabusYr = "";
            string SyllabusQry = "select distinct syllabus_year from syllabus_master where degree_code ='" + ddlBranch.SelectedItem.Value + "' and batch_year ='" + ddlBatch.SelectedItem.Value + "' ";
            syllbus = da.select_method_wo_parameter(SyllabusQry, "text");
            ddlTest.Items.Clear();

            if (syllbus.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < syllbus.Tables[0].Rows.Count; i++)
                {
                    if (SyllabusYr == "")
                    {
                        SyllabusYr = syllbus.Tables[0].Rows[i]["syllabus_year"].ToString();
                    }
                    else
                    {
                        SyllabusYr = SyllabusYr + "," + syllbus.Tables[0].Rows[i]["syllabus_year"].ToString();
                    }

                }
            }
            if (SyllabusYr != "")
            {

                string Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_year in(" + SyllabusYr.ToString() + ") and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by criteria";
                syllbus1 = da.select_method_wo_parameter(Sqlstr, "Text");
                // DropDownList2.Items.Clear();
                if (syllbus1.Tables[0].Rows.Count > 0)
                {
                    ddlTest.DataSource = syllbus1;
                    ddlTest.DataTextField = "criteria";
                    ddlTest.DataValueField = "criteria_no";
                    ddlTest.DataBind();
                }
            }
        }
        catch
        {
        }
    }
    protected void logout_btn_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.RemoveAll();
        Session.Abandon();
        Response.Redirect("~/Default.aspx");
    }
    public void bindcourse()
    {
        try
        {
            //CheckBoxListdegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ht.Clear();
            ht.Add("single_user", singleuser);
            ht.Add("group_code", group_user);
            ht.Add("college_code", ddlcollege.SelectedItem.Value);
            ht.Add("user_code", usercode);
            ds = da.select_method("bind_degree", ht, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            ddlDegree.Items.Clear();
            if (count1 > 0)
            {
                ddlDegree.DataSource = ds;
                ddlDegree.DataTextField = "course_name";
                ddlDegree.DataValueField = "course_id";
                ddlDegree.DataBind();
            }
        }
        catch
        {
        }

    }
    public void GetSubject()
    {


        string SyllabusYr;
        string SyllabusQry;
        if ((ddlBranch.SelectedValue.ToString() != "") && (ddlSemYr.SelectedValue.ToString() != "") && (ddlBatch.SelectedValue.ToString() != ""))
        {
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
            ds = da.select_method_wo_parameter(SyllabusQry, "text");
            string Sqlstr;
            Sqlstr = "";
            string sections = "";
            string strsec = "";
            sections = ddlSec.SelectedValue.ToString();
            if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
            {
                strsec = "";
            }
            else
            {
                strsec = " and registration.sections='" + sections.ToString() + "'";
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Session["Staff_Code"].ToString() == "")
                {
                    Sqlstr = "select distinct subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser,registration where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code=" + ddlBranch.SelectedValue.ToString() + " and syllabus_master.semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_master.batch_year=" + ddlBatch.SelectedValue.ToString() + " and syllabus_year=" + ds.Tables[0].Rows[0]["syllabus_year"].ToString() + " and subject.subject_no =subjectchooser.subject_no and subjectchooser.roll_no=registration.roll_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + " and registration.current_semester>=" + ddlSemYr.SelectedValue.ToString() + " and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + " and RollNo_Flag<>0 and cc=0 " + strsec.ToString() + " and exam_flag <> 'DEBAR'";
                }
                else if (Session["Staff_Code"].ToString() != "")
                {
                    Sqlstr = "select distinct subject_name,subject.subject_no,subject_code from subject,sub_sem,staff_selector,usermaster,registration,subjectchooser,syllabus_master where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and  subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code=" + ddlBranch.SelectedValue.ToString() + " and  syllabus_master.semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_master.batch_year= " + ddlBatch.SelectedValue.ToString() + " and syllabus_master.syllabus_year= " + ds.Tables[0].Rows[0]["syllabus_year"].ToString() + " and subject.subject_no =subjectchooser.subject_no and subjectchooser.roll_no=registration.roll_no and  registration.degree_code=" + ddlBranch.SelectedValue.ToString() + " and registration.current_semester>=" + ddlSemYr.SelectedValue.ToString() + " and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + " and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and staff_selector.staff_code=usermaster.staff_code and subject.subject_no=staff_selector.subject_no and usermaster.staff_code='" + Session["Staff_Code"].ToString() + "'" + strsec.ToString() + "";
                }
                if (Sqlstr != "")
                {
                    ds1 = da.select_method_wo_parameter(Sqlstr, "text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        ddlTest.DataSource = ds1;
                        ddlTest.DataValueField = "Subject_No";
                        ddlTest.DataTextField = "Subject_Name";
                        ddlTest.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- -Select- -", "-1"));
                        ddlTest.DataBind();
                    }
                }
            }

        }
    }


    public void bindbranch(string branch)
    {
        try
        {
            string commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + ddlDegree.SelectedItem.Value + "') and deptprivilages.Degree_code=degree.Degree_code ";


            //commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";

            {
                ds = da.select_method_wo_parameter(commname, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlBranch.DataSource = ds;
                    ddlBranch.DataTextField = "dept_name";
                    ddlBranch.DataValueField = "degree_code";
                    ddlBranch.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindyear()
    {
        try
        {
            ddlBatch.Items.Clear();
            ds = da.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "batch_year";
                ddlBatch.DataValueField = "batch_year";
                ddlBatch.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                ddlBatch.SelectedValue = max_bat.ToString();

            }
        }
        catch
        {
        }


    }
    public void bindsem()
    {
        try
        {
            //--------------------semester load
            DataSet ds3 = new DataSet();
            ddlSemYr.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;

            string sqluery = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + ddlcollege.SelectedItem.Value + "";

            ds3 = da.select_method_wo_parameter(sqluery, "text");
            if (ds3.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds3.Tables[0].Rows[0]["first_year_nonsemester"]);
                duration = Convert.ToInt16(ds3.Tables[0].Rows[0]["ndurations"]);
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }

                }
            }
            else
            {


                sqluery = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and college_code=" + ddlcollege.SelectedItem.Value + "";
                ddlSemYr.Items.Clear();
                ds3 = da.select_method_wo_parameter(sqluery, "text");
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds3.Tables[0].Rows[0]["first_year_nonsemester"]);
                    duration = Convert.ToInt16(ds3.Tables[0].Rows[0]["duration"]);
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSemYr.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSemYr.Items.Add(i.ToString());
                        }

                    }
                }


            }


        }
        catch
        {
        }
    }
    public void BindSectionDetail()
    {
        try
        {
            ddlSec.Items.Clear();
            if (ddlSemYr.Text != "")
            {

                string branch = ddlBranch.SelectedValue.ToString();
                string batch = ddlBatch.SelectedValue.ToString();

                string sqlquery = "select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";

                DataSet ds = new DataSet();
                ds = da.select_method_wo_parameter(sqlquery, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlSec.DataSource = ds;
                    ddlSec.DataTextField = "sections";
                    ddlSec.DataValueField = "sections";
                    ddlSec.DataBind();
                }
                //ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["sections"].ToString() == "")
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

        }
        catch
        {
        }
    }


    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_test.Text = "--Select--";
        bindyear();
        bindcourse();
        bindbranch(collegecode);
        bindsem();
        BindSectionDetail();
        if (radiobutton1.Text == "University Wise")
        {
            if (rdSubWise.Checked == true)
            {
                subject();
            }
        }
        else if (radiobutton1.Text == "CAM Wise")
        {
            if (rdTestWise.Checked == true)
            {
                test();
            }
            else if (rdSubWise.Checked == true)
            {
                subject();
            }
        }
    }
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_test.Text = "--Select--";
        bindcourse();
        bindbranch(collegecode);
        bindsem();
        lblErrTest.Visible = false;
        BindSectionDetail();
        if (radiobutton1.Text == "University Wise")
        {
            if (rdSubWise.Checked == true)
            {
                subject();
            }
        }
        else if (radiobutton1.Text == "CAM Wise")
        {
            if (rdTestWise.Checked == true)
            {
                test();
            }
            else if (rdSubWise.Checked == true)
            {
                subject();
            }
        }
    }
    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_test.Text = "--Select--";
        lblErrTest.Visible = false;
        bindbranch(collegecode);
        bindsem();
        BindSectionDetail();
        if (radiobutton1.Text == "University Wise")
        {
            if (rdSubWise.Checked == true)
            {
                subject();
            }
        }
        else if (radiobutton1.Text == "CAM Wise")
        {
            if (rdTestWise.Checked == true)
            {
                test();
            }
            else if (rdSubWise.Checked == true)
            {
                subject();
            }
        }
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_test.Text = "--Select--";
        lblErrTest.Visible = false;
        bindsem();
        BindSectionDetail();
        if (radiobutton1.Text == "University Wise")
        {
            if (rdSubWise.Checked == true)
            {
                subject();
            }
        }
        else if (radiobutton1.Text == "CAM Wise")
        {
            if (rdTestWise.Checked == true)
            {
                test();
            }
            else if (rdSubWise.Checked == true)
            {
                subject();
            }
        }
    }
    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_test.Text = "--Select--";
        lblErrTest.Visible = false;
        BindSectionDetail();
        if (radiobutton1.Text == "University Wise")
        {
            if (rdSubWise.Checked == true)
            {
                subject();
            }
        }
        else if (radiobutton1.Text == "CAM Wise")
        {
            if (rdTestWise.Checked == true)
            {
                test();
            }
            else if (rdSubWise.Checked == true)
            {
                subject();
            }
        }
    }
    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_test.Text = "--Select--";
        lblErrTest.Visible = false;
        if (radiobutton1.Text == "University Wise")
        {
            if (rdSubWise.Checked == true)
            {
                subject();
            }
        }
        else if (radiobutton1.Text == "Cam Wise")
        {
            if (rdTestWise.Checked == true)
            {
                test();
            }
            else if (rdSubWise.Checked == true)
            {
                subject();
            }
        }
    }
    protected void rdTestWise_CheckedChanged(object sender, EventArgs e)
    {
        txt_test.Text = "--Select--";
        lblErrTest.Visible = false;
        if (rdTestWise.Checked == true)
        {
            lblTest.Text = "Test";
            test();
        }
    }
    protected void rdSubWise_CheckedChanged(object sender, EventArgs e)
    {
        txt_test.Text = "--Select--";
        lblErrTest.Visible = false;
        if (rdSubWise.Checked == true)
        {
            lblTest.Text = "Subject";
            subject();

        }

    }
    protected void ddlTest_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_test.Text = "--Select--";
        lblErrTest.Visible = false;

    }
    protected void subject()
    {
        txt_test.Text = "--Select--";
        ddlTest.Items.Clear();
        if (ddlSemYr.Text != "")
        {
            string sql = "select distinct sem.subject_type,s.subject_code,S.subject_no,subject_name,s.acronym from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code  and SM.degree_code='" + ddlBranch.SelectedItem.Value + "' and SM.semester='" + ddlSemYr.SelectedItem.Value + "' and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + ddlBatch.SelectedItem.Value + "' order by S.subject_no";
            ddlTest.Items.Clear();
            ds1 = da.select_method_wo_parameter(sql, "text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddlTest.DataSource = ds1;
                ddlTest.DataTextField = "subject_name";
                ddlTest.DataValueField = "subject_no";
                ddlTest.DataBind();
            }
        }
        else
        {

        }
    }

    protected void chktesr_checkedchanged(object sender, EventArgs e)
    {
        txt_test.Text = "--Select--";
        if (chktesr.Checked == true)
        {
            if (rdTestWise.Checked == true)
            {
                for (int i = 0; i < ddlTest.Items.Count; i++)
                {
                    ddlTest.Items[i].Selected = true;
                    txt_test.Text = "Test(" + (ddlTest.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < ddlTest.Items.Count; i++)
                {
                    ddlTest.Items[i].Selected = true;
                    txt_test.Text = "Subject(" + (ddlTest.Items.Count) + ")";
                }

            }
        }
        else
        {
            for (int i = 0; i < ddlTest.Items.Count; i++)
            {
                ddlTest.Items[i].Selected = false;
                txt_test.Text = "--Select--";
            }
        }

    }

    protected void ddlTest_selectedchanged(object sender, EventArgs e)
    {
        try
        {
            txt_test.Text = "--Select--";

            string value = "";
            string code = "";
            for (int i = 0; i < ddlTest.Items.Count; i++)
            {
                if (rdTestWise.Checked == true)
                {
                    if (ddlTest.Items[i].Selected == true)
                    {

                        value = ddlTest.Items[i].Text;
                        code = ddlTest.Items[i].Value.ToString();
                        ddlcount = ddlcount + 1;
                        txt_test.Text = "Test(" + ddlcount.ToString() + ")";
                    }
                }
                else
                {
                    if (ddlTest.Items[i].Selected == true)
                    {

                        value = ddlTest.Items[i].Text;
                        code = ddlTest.Items[i].Value.ToString();
                        ddlcount = ddlcount + 1;
                        txt_test.Text = "Subject(" + ddlcount.ToString() + ")";
                    }
                }

            }

            if (ddlcount == 0)
                txt_test.Text = "---Select---";
        }
        catch
        {

        }

    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            string testcaM = "";
            int COUNT = 0;
            lblErrTest.Visible = false;
            if (radiobutton1.Text == "CAM Wise")
            {

                if (rdSubWise.Checked == true)
                {
                    if (ddlSemYr.Text != "")
                    {
                        if (ddlTest.Text != "")
                        {

                            if (ddlTest.Items.Count > 0)
                            {
                                for (int i = 0; i < ddlTest.Items.Count; i++)
                                {
                                    if (testcaM == "")
                                    {
                                        testcaM = ddlTest.Items[i].Value.ToString();
                                    }

                                    else
                                    {
                                        testcaM = testcaM + "," + ddlTest.Items[i].Value.ToString();
                                    }

                                }


                            }

                            lblErrTest.Visible = false;
                            string SyllabusYr = "";
                            string SyllabusQry = "select distinct syllabus_year from syllabus_master where degree_code ='" + ddlBranch.SelectedItem.Value + "' and batch_year ='" + ddlBatch.SelectedItem.Value + "' ";
                            syllbus = da.select_method_wo_parameter(SyllabusQry, "text");


                            if (syllbus.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < syllbus.Tables[0].Rows.Count; i++)
                                {
                                    if (SyllabusYr == "")
                                    {
                                        SyllabusYr = syllbus.Tables[0].Rows[i]["syllabus_year"].ToString();
                                    }
                                    else
                                    {
                                        SyllabusYr = SyllabusYr + "," + syllbus.Tables[0].Rows[i]["syllabus_year"].ToString();
                                    }

                                }
                            }
                            string creite = "";
                            string test = "";
                            double cammark = 0.0;
                            Boolean g = false;
                            if (SyllabusYr != "")
                            {

                                string Sqlstr = "select criteria,criteria_no,subject_no from criteriaforinternal,syllabus_master,subject s where s.syll_code=syllabus_master.syll_code and criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_year in(" + SyllabusYr.ToString() + ") and batch_year=" + ddlBatch.SelectedValue.ToString() + "  order by subject_no";
                                syllbus1 = da.select_method_wo_parameter(Sqlstr, "Text");
                                syllbus1.Tables[0].Columns.Add("Cammark", typeof(string));
                                if (ddlSec.Text == "")
                                {
                                    test = "";
                                }
                                else
                                {
                                    test = "and r.Sections='" + ddlSec.SelectedItem.Value + "'";
                                }

                                string sqlsubject = "select c.Criteria_no,s.subject_no,subject_name,e.min_mark,e.max_mark,re.marks_obtained,r.roll_no,r.Stud_Name from Exam_type e,CriteriaForInternal c,subject s,Result re,Registration r,syllabus_master SM where re.exam_code=e.exam_code and SM.syll_code=S.syll_code AND  r.Roll_No=re.roll_no and s.subject_no=e.subject_no and c.Criteria_no=e.criteria_no  and cc=0 and delflag=0 and exam_flag<>'debar' and r.Batch_Year='" + ddlBatch.SelectedItem.Value + "'  " + test + " and s.subject_no in(" + testcaM + ")";
                                ds = da.select_method_wo_parameter(sqlsubject, "text");
                                if (syllbus1.Tables[0].Rows.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < ddlTest.Items.Count; i++)
                                        {
                                            if (ddlTest.Items[i].Selected == true)
                                            {
                                                COUNT++;
                                                for (int j = 0; j < syllbus1.Tables[0].Rows.Count; j++)
                                                {
                                                    creite = syllbus1.Tables[0].Rows[j]["criteria_no"].ToString();
                                                    DataView dv = new DataView();
                                                    ds.Tables[0].DefaultView.RowFilter = "criteria_no='" + creite + "' and subject_no =" + ddlTest.Items[i].Value + " ";
                                                    dv = ds.Tables[0].DefaultView;
                                                    double data = dv.Count;

                                                    DataView dv1 = new DataView();
                                                    dv.RowFilter = "marks_obtained>=min_mark and criteria_no='" + creite + "' and subject_no =" + ddlTest.Items[i].Value + "";
                                                    dv1 = dv;
                                                    cammark = (dv1.Count / data) * 100;
                                                    if (dv.Count > 0)
                                                    {
                                                        if (ddlTest.Items[i].Value.ToString() == syllbus1.Tables[0].Rows[j]["subject_no"].ToString())
                                                        {
                                                            syllbus1.Tables[0].Rows[j]["Cammark"] = Math.Round(cammark);
                                                            g = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ddlTest.Items[i].Value.ToString() == syllbus1.Tables[0].Rows[j]["subject_no"].ToString())
                                                        {
                                                            syllbus1.Tables[0].Rows[j]["Cammark"] = " ";
                                                            g = true;
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }

                                }
                                for (int j = 0; j < ddlTest.Items.Count; j++)
                                {
                                    DataView dv = new DataView();
                                    if (ddlTest.Items[j].Selected == true)
                                    {
                                        g = false;
                                        syllbus1.Tables[0].DefaultView.RowFilter = " subject_no=" + ddlTest.Items[j].Value + "";
                                        dv = syllbus1.Tables[0].DefaultView;

                                        if (dv.Count > 0)
                                        {
                                            for (int i = 0; i < dv.Count; i++)
                                            {
                                                if (dv[i]["cammark"].ToString() == " ")
                                                {

                                                }
                                                else
                                                {
                                                    g = true;
                                                }
                                            }
                                            if (g == true)
                                            {
                                                Chart Chart1 = new Chart();
                                                Chart1.ChartAreas.Add("ChartArea1");

                                                Chart1.Series.Add("Series1").ChartType = SeriesChartType.Radar;
                                                if (COUNT == 1)
                                                {
                                                    //Chart1.ChartAreas["CHARTAREA1"].AlignmentStyle = 
                                                    Chart1.Width = 900;
                                                    Chart1.Height = 450;
                                                }
                                                else
                                                {
                                                    Chart1.Width = 450;
                                                    Chart1.Height = 400;
                                                }


                                                Chart1.Series["Series1"].BorderColor = Color.Blue;
                                                Chart1.Series["Series1"]["RadarDrawingStyle"] = "marker";
                                                Chart1.Series["Series1"]["CircularLabelsStyle"] = "Horizontal";
                                                Chart1.Series["Series1"].MarkerSize = 7;
                                                Chart1.Series["Series1"].MarkerStyle = MarkerStyle.Star4;
                                                Chart1.Series["Series1"].MarkerColor = Color.DarkBlue;
                                                Chart1.Series["Series1"].MarkerBorderWidth = 7;
                                                Chart1.Series["Series1"].BorderWidth = 3;
                                                Chart1.ChartAreas["ChartArea1"].AxisY.Interval = 25;
                                                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 100;
                                                Chart1.ChartAreas["ChartArea1"].AxisY.TitleForeColor = Chart1.ForeColor;
                                                Chart1.Series["Series1"].IsValueShownAsLabel = true;
                                                Chart1.Series["Series1"]["RadarDrawingStyle"] = "Line";
                                                Chart1.Series["Series1"].Color = Color.Red;
                                                Chart1.Series["Series1"].XValueMember = "criteria";
                                                Chart1.Series["Series1"].YValueMembers = "Cammark";
                                                Chart1.Series["Series1"].Font = new Font("Book Antiqua", 10, FontStyle.Bold);
                                                Chart1.Series["Series1"].Name = "Subject wise %";
                                                Chart1.ChartAreas["ChartArea1"].AxisY.LineColor = Color.Chocolate;
                                                Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.BlueViolet;

                                                Chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Book Antiqua", 8);
                                                Chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Green;
                                                Chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Book Antiqua", 8);
                                                Chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
                                                Title radarchart = Chart1.Titles.Add("" + ddlTest.Items[j] + "(CAM Subject Wise)");
                                                radarchart.Font = new Font("Book Antiqua", 10, FontStyle.Bold);
                                                Chart1.DataSource = dv;
                                                Chart1.DataBind();
                                                Chart1.SaveImage(Server.MapPath("App_Data/Sample.jpg"));
                                                panelchart.Controls.Add(Chart1);
                                            }
                                            else
                                            {

                                                Table b = new Table();
                                                TableCell tc4 = new TableCell();
                                                TableRow tr4 = new TableRow();
                                                Label lblerr = new Label();
                                                lblerr.Text = "No Records Found for " + ddlTest.Items[j] + "";
                                                lblerr.Font.Size = FontUnit.Medium;
                                                lblerr.Font.Name = "Book Antiqua";
                                                lblerr.ForeColor = Color.Red;
                                                lblerr.Font.Bold = true;
                                                //lblerr.Font.Name 
                                                lblerr.Visible = true;
                                                panel5.Controls.Add(b);
                                                tr4.Cells.Add(tc4);
                                                b.Rows.Add(tr4);
                                                tc4.Controls.Add(lblerr);
                                                // Chart1.Visible = false;
                                            }
                                        }
                                        else
                                        {
                                            Table b = new Table();
                                            TableCell tc4 = new TableCell();
                                            TableRow tr4 = new TableRow();
                                            Label lblerr = new Label();
                                            lblerr.Text = "No Records Found for " + ddlTest.Items[j] + "";
                                            lblerr.Font.Size = FontUnit.Medium;
                                            lblerr.Font.Name = "Book Antiqua";
                                            lblerr.ForeColor = Color.Red;
                                            lblerr.Font.Bold = true;
                                            //lblerr.Font.Name 
                                            lblerr.Visible = true;
                                            panel5.Controls.Add(b);
                                            tr4.Cells.Add(tc4);
                                            b.Rows.Add(tr4);
                                            tc4.Controls.Add(lblerr);
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            lblErrTest.Text = "Please Select Any One Subject";
                            lblErrTest.Visible = true;
                            // Chart1.Visible = false;
                        }
                    }
                    else
                    {
                        lblErrTest.Text = "Please Select Any One Sem";
                        lblErrTest.Visible = true;
                        // Chart1.Visible = false;
                    }
                }
                else if (rdTestWise.Checked == true)
                {
                    if (ddlSemYr.Text != " ")
                    {
                        if (ddlTest.Text.Trim() != "")
                        {

                            lblErrTest.Visible = false;
                            Boolean g = false;
                            string subject = "";
                            double cammark = 0.0;
                            string test = "";
                            if (ddlSec.Text == "")
                            {
                                test = "";
                            }
                            else
                            {
                                test = "and r.sections='" + ddlSec.SelectedItem.Value + "'";
                            }

                            if (ddlTest.Items.Count > 0)
                            {
                                for (int i = 0; i < ddlTest.Items.Count; i++)
                                {
                                    if (ddlTest.Items[i].Selected == true)
                                    {
                                        if (testcaM == "")
                                        {
                                            testcaM = ddlTest.Items[i].Value.ToString();
                                        }

                                        else
                                        {
                                            testcaM = testcaM + "," + ddlTest.Items[i].Value.ToString();
                                        }
                                    }

                                }


                            }
                            string subjectload = "select * from subject s,syllabus_master sm,Exam_type r,CriteriaForInternal c where s.syll_code=sm.syll_code and r.subject_no=s.subject_no and r.criteria_no=c.Criteria_no and c.Criteria_no in (" + testcaM + ") " + test + " ";
                            ds1 = da.select_method_wo_parameter(subjectload, "text");
                            ds1.Tables[0].Columns.Add("Cammark", typeof(string));
                            string sqlsubject = "select c.Criteria_no,s.subject_no,subject_name,e.min_mark,e.max_mark,re.marks_obtained,r.roll_no,r.Stud_Name from Exam_type e,CriteriaForInternal c,subject s,Result re,Registration r,syllabus_master SM where re.exam_code=e.exam_code and SM.syll_code=S.syll_code AND  r.Roll_No=re.roll_no and s.subject_no=e.subject_no and c.Criteria_no=e.criteria_no  and cc=0 and delflag=0 and exam_flag<>'debar' and r.Batch_Year='" + ddlBatch.SelectedItem.Value + "' " + test + "    and e.criteria_no in (" + testcaM + ")  ";
                            ds = da.select_method_wo_parameter(sqlsubject, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (ds1.Tables[0].Rows.Count > 0)
                                {
                                    for (int j = 0; j < ddlTest.Items.Count; j++)
                                    {
                                        if (ddlTest.Items[j].Selected == true)
                                        {
                                            COUNT++;
                                            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                                            {
                                                subject = ds1.Tables[0].Rows[i]["subject_no"].ToString();
                                                DataView dv = new DataView();
                                                ds.Tables[0].DefaultView.RowFilter = "subject_no='" + subject + "' and Criteria_no=" + ddlTest.Items[j].Value + "";
                                                dv = ds.Tables[0].DefaultView;
                                                double data = dv.Count;

                                                DataView dv1 = new DataView();
                                                dv.RowFilter = "marks_obtained>=min_mark and subject_no='" + subject + "' and Criteria_no=" + ddlTest.Items[j].Value + "";
                                                dv1 = dv;
                                                cammark = (dv1.Count / data) * 100;
                                                if (ddlTest.Items[j].Value.ToString() == ds1.Tables[0].Rows[i]["Criteria_no"].ToString())
                                                {
                                                    ds1.Tables[0].Rows[i]["Cammark"] = Math.Round(cammark);
                                                    g = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            for (int j = 0; j < ddlTest.Items.Count; j++)
                            {
                                DataView dv = new DataView();
                                if (ddlTest.Items[j].Selected == true)
                                {

                                    ds1.Tables[0].DefaultView.RowFilter = " Criteria_no=" + ddlTest.Items[j].Value + "";
                                    dv = ds1.Tables[0].DefaultView;

                                    if (dv.Count > 0)
                                    {
                                        if (g == true)
                                        {
                                            Chart Chart1 = new Chart();
                                            Chart1.ChartAreas.Add("ChartArea1");
                                            Chart1.Series.Add("Series1").ChartType = SeriesChartType.Radar;
                                            if (COUNT == 1)
                                            {
                                                //Chart1.ChartAreas["CHARTAREA1"].AlignmentStyle = 
                                                Chart1.Width = 900;
                                                Chart1.Height = 450;
                                            }
                                            else if (COUNT == 2)
                                            {
                                                Chart1.Width = 450;
                                                Chart1.Height = 400;
                                            }
                                            else
                                            {
                                                Chart1.Width = 300;
                                                Chart1.Height = 300;
                                            }

                                            Chart1.Series["Series1"].BorderColor = Color.Blue;
                                            Chart1.Series["Series1"]["RadarDrawingStyle"] = "marker";
                                            Chart1.Series["Series1"]["CircularLabelsStyle"] = "Horizontal";
                                            Chart1.Series["Series1"].MarkerSize = 7;
                                            Chart1.Series["Series1"].MarkerStyle = MarkerStyle.Star4;
                                            Chart1.Series["Series1"].MarkerColor = Color.DarkBlue;
                                            Chart1.Series["Series1"].MarkerBorderWidth = 7;
                                            Chart1.Series["Series1"].BorderWidth = 3;
                                            Chart1.ChartAreas["ChartArea1"].AxisY.Interval = 25;
                                            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 100;
                                            Chart1.ChartAreas["ChartArea1"].AxisY.TitleForeColor = Chart1.ForeColor;
                                            Chart1.Series["Series1"].IsValueShownAsLabel = true;
                                            Chart1.Series["Series1"].Font = new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold);
                                            Chart1.Series["Series1"]["RadarDrawingStyle"] = "Line";
                                            Chart1.Series["Series1"].Color = Color.Red;
                                            Chart1.Series["Series1"].XValueMember = "acronym";
                                            Chart1.Series["Series1"].YValueMembers = "Cammark";
                                            Chart1.Series["Series1"].Name = "Test wise %";
                                            Chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Book Antiqua", 8, FontStyle.Bold);
                                            Chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Green;
                                            Chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Book Antiqua", 8);
                                            Chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
                                            Chart1.ChartAreas["ChartArea1"].AxisY.LineColor = Color.Chocolate;
                                            Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.BlueViolet;

                                            Chart1.DataSource = dv;
                                            Chart1.DataBind();
                                            Chart1.SaveImage(Server.MapPath("App_Data/Sample.jpg"));
                                            Title radarchart = Chart1.Titles.Add("" + ddlTest.Items[j] + "(CAM Test Wise)");
                                            radarchart.Font = new Font("Book Antiqua", 10, FontStyle.Bold);

                                            panelchart.Controls.Add(Chart1);
                                        }
                                        else
                                        {
                                            lblErrTest.Text = "No Records Found";
                                            lblErrTest.Visible = true;
                                            //  Chart1.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        Table b = new Table();
                                        TableCell tc4 = new TableCell();
                                        TableRow tr4 = new TableRow();
                                        Label lblerr = new Label();
                                        lblerr.Text = "No Records Found for " + ddlTest.Items[j] + "";
                                        lblerr.Font.Size = FontUnit.Medium;
                                        lblerr.Font.Name = "Book Antiqua";
                                        lblerr.ForeColor = Color.Red;
                                        lblerr.Font.Bold = true;
                                        //lblerr.Font.Name 
                                        lblerr.Visible = true;
                                        panel5.Controls.Add(b);
                                        tr4.Cells.Add(tc4);
                                        b.Rows.Add(tr4);
                                        tc4.Controls.Add(lblerr);
                                    }
                                }

                            }
                        }
                        else
                        {
                            lblErrTest.Text = "Please Select Any One Test";
                            lblErrTest.Visible = true;
                            //  Chart1.Visible = false;
                        }

                    }
                    else
                    {
                        lblErrTest.Text = "Please Select Any One Sem";
                        lblErrTest.Visible = true;
                        // Chart1.Visible = false;
                    }
                }
            }
            else if (radiobutton1.Text == "University Wise")
            {
                string sec1;
                double grademark;
                double grademark1;
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();
                DataSet datagrade = new DataSet();
                Boolean flag_university = new Boolean();
                flag_university = false;
                if (ddlSec.Text == "")
                {
                    sec1 = "";
                }
                else
                {
                    sec1 = "and Sections='" + ddlSec.SelectedItem.Value + "'";
                }
                string sqldatabind = " select distinct Mark_Grade,Credit_Points from Grade_Master where Degree_Code='" + ddlBranch.SelectedItem.Value + "'  and college_code='" + ddlcollege.SelectedItem.Value + "' and batch_year='" + ddlBatch.SelectedItem.Value + "'order by Credit_Points desc";
                datagrade = da.select_method_wo_parameter(sqldatabind, "text");
                ht.Clear();
                string value = "";
                if (datagrade.Tables[0].Rows.Count > 0)
                {
                    ht.Clear();
                    for (int l = 0; l < datagrade.Tables[0].Rows.Count; l++)
                    {
                        value = datagrade.Tables[0].Rows[0]["Mark_Grade"].ToString();
                        ht.Add(datagrade.Tables[0].Rows[l]["Mark_Grade"], "" + datagrade.Tables[0].Rows[l]["Credit_Points"] + "");
                    }
                }
                string sqlqurey1 = " Select distinct Current_Semester,Exam_Code,exam_month,exam_year from Exam_Details where Degree_Code = '" + ddlBranch.SelectedItem.Value + "'  and Current_Semester='" + ddlSemYr.SelectedItem.Value + "' and Batch_Year = '" + ddlBatch.SelectedItem.Value + "'";
                ds2 = da.select_method_wo_parameter(sqlqurey1, "text");
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    string subject_no = "";
                    string sqlquery = "Select distinct s.mintotal as mintot,s.min_int_marks as mimark, s.min_ext_marks as mxmark,s.maxtotal as maxtot,s.acronym,subject_name,subject_code as Subject_Code,mark_entry.subject_no as Subject_No,semester,subject_type as Subtype,credit_points from Mark_Entry,Subject s,sub_sem,syllabus_master,staff_selector st,staffmaster sm,staff_appl_master stf,desig_master dm  where stf.appl_no=sm.appl_no and stf.desig_code=dm.desig_code and sm.college_code=dm.collegeCode and syllabus_master.syll_code=s.syll_code and Mark_Entry.Subject_No = s..Subject_No and s.subtype_no= sub_sem.subtype_no and sm.staff_code=st.staff_code and Exam_Code = '" + ds2.Tables[0].Rows[0]["Exam_Code"] + "' and attempts=1 and st.subject_no=s.subject_no order by semester desc,subject_type desc, mark_entry.subject_no asc";
                    ds = da.select_method_wo_parameter(sqlquery, "text");
                    ds.Tables[0].Columns.Add("cammark", typeof(string));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int g = 0; g < ds.Tables[0].Rows.Count; g++)
                        {
                            if (subject_no.ToString() == "")
                            {
                                subject_no = ds.Tables[0].Rows[g]["subject_no"].ToString();
                            }
                            else
                            {
                                subject_no = subject_no + "," + ds.Tables[0].Rows[g]["subject_no"].ToString();
                            }
                        }
                    }
                    if (subject_no.ToString() != "")
                    {
                        DataSet university = new DataSet();
                        string sqlquery1 = "Select grade_flag from grademaster where exam_month='" + ds2.Tables[0].Rows[0]["exam_month"] + "' and exam_year='" + ds2.Tables[0].Rows[0]["exam_year"] + "' and Batch_year='" + ddlBatch.SelectedItem.Value + "' and degree_code='" + ddlBranch.SelectedItem.Value + "'";
                        university = da.select_method_wo_parameter(sqlquery1, "text");
                        string sqlroll = "Select acronym as subject_name,subject_code,subject.subject_no,result,total,grade,Registration.roll_no,cp,mark_entry.subject_no,subject.min_ext_marks,subject.min_int_marks,isnull(Mark_Entry.internal_mark,0) as internal_mark ,isnull(Mark_Entry.external_mark,0) as external_mark,semester,Sections from Mark_Entry,Subject,sub_sem,syllabus_master,Registration where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No =Subject.Subject_No and  subject.subtype_no= sub_sem.subtype_no and Registration.Roll_No=mark_entry.roll_no and  Exam_Code in(" + ds2.Tables[0].Rows[0]["Exam_Code"] + ") and Subject.subject_no in (" + subject_no + ") " + sec1 + " and CC=0 and DelFlag=0 and Exam_Flag<>'debar' order by semester desc,subject_type desc,subject.subject_no asc";
                        ds3 = da.select_method_wo_parameter(sqlroll, "Text");
                        if (university.Tables[0].Rows.Count > 0)
                        {

                            if (university.Tables[0].Rows.Count > 0)
                            {
                                if (university.Tables[0].Rows[0]["grade_flag"].ToString() == "2")
                                {

                                    if (ds.Tables[0].Rows.Count > 0)
                                    {

                                        double totalmark = 0;
                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                        {
                                            grademark = 0.0;
                                            DataView dv = new DataView();
                                            ds3.Tables[0].DefaultView.RowFilter = "subject_code='" + ds.Tables[0].Rows[i]["subject_code"] + "'";
                                            dv = ds3.Tables[0].DefaultView;
                                            for (int j = 0; j < dv.Count; j++)
                                            {

                                                //int count = hat.Count;
                                                if (ds3.Tables[0].Rows[j]["cp"] == DBNull.Value)
                                                {

                                                }
                                                else
                                                {
                                                    string mark = dv[j]["grade"].ToString();
                                                    double mark2 = Convert.ToInt32(ht[value]) * Convert.ToInt32(dv[j]["cp"]);
                                                    double mark1 = 0;

                                                    foreach (DictionaryEntry child in ht)
                                                    {


                                                        if (mark.ToString() == child.Key.ToString())
                                                        {
                                                            if (child.Value.ToString() == "0")
                                                            {



                                                            }
                                                            else
                                                            {
                                                                mark1 = Convert.ToInt32(child.Value) * Convert.ToInt32(dv[j]["cp"]);
                                                                grademark++;
                                                                flag_university = true;
                                                            }
                                                        }

                                                    }
                                                }


                                            }
                                            for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
                                            {
                                                if (ds.Tables[0].Rows[l]["subject_no"].ToString() == dv[0]["subject_no"].ToString())
                                                {
                                                    grademark1 = (grademark / dv.Count) * 100;
                                                    ds.Tables[0].Rows[l]["cammark"] = Math.Round(grademark1);

                                                }
                                            }
                                        }
                                    }
                                }
                                else if (university.Tables[0].Rows[0]["grade_flag"].ToString() == "3")
                                {
                                    string dd = "select linkvalue from inssettings where linkname='corresponding grade' and college_code='" + Session["collegecode"].ToString() + "'";

                                    DataSet df1 = new DataSet();
                                    df1 = da.select_method_wo_parameter(dd, "text");
                                    if (df1.Tables[0].Rows[0]["linkvalue"].ToString() == "0")
                                    {
                                        string sqlroll1 = "Select acronym as subject_name,subject_code,subject.subject_no,result,total,grade,Registration.roll_no,cp,mark_entry.subject_no,subject.min_ext_marks,subject.min_int_marks,isnull(Mark_Entry.internal_mark,0) as internal_mark ,isnull(Mark_Entry.external_mark,0) as external_mark,semester,Sections from Mark_Entry,Subject,sub_sem,syllabus_master,Registration where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No =Subject.Subject_No and  subject.subtype_no= sub_sem.subtype_no and Registration.Roll_No=mark_entry.roll_no and  Exam_Code in(" + ds2.Tables[0].Rows[0]["Exam_Code"] + ") and Subject.subject_no in (" + subject_no + ") " + sec1 + " and CC=0 and DelFlag=0 and Exam_Flag<>'debar' order by semester desc,subject_type desc,subject.subject_no asc";
                                        ds3 = da.select_method_wo_parameter(sqlroll1, "Text");

                                        for (int j = 0; j < ds3.Tables[0].Rows.Count; j++)
                                        {
                                            grademark = 0.0;
                                            DataView dv = new DataView();
                                            ds3.Tables[0].DefaultView.RowFilter = "subject_code='" + ds3.Tables[0].Rows[j]["subject_code"] + "'";
                                            dv = ds3.Tables[0].DefaultView;
                                            for (int k = 0; k < dv.Count; k++)
                                            {
                                                double internal1 = Convert.ToDouble(dv[k]["internal_mark"].ToString());
                                                double external = Convert.ToDouble(dv[k]["external_mark"].ToString());
                                                double mark1 = Convert.ToDouble(dv[k]["min_ext_marks"].ToString());
                                                double mark2 = Convert.ToDouble(dv[k]["min_int_marks"].ToString());
                                                if (external == 0.0 && internal1 == 0.0)
                                                {
                                                    flag_university = false;

                                                }
                                                else
                                                {
                                                    if (internal1 >= mark2 && external >= mark1)
                                                    {
                                                        grademark++;
                                                        flag_university = true;

                                                    }

                                                }
                                            }
                                            for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
                                            {
                                                if (ds.Tables[0].Rows[l]["subject_no"].ToString() == dv[0]["subject_no"].ToString())
                                                {
                                                    grademark1 = (grademark / dv.Count) * 100;
                                                    ds.Tables[0].Rows[l]["cammark"] = Math.Round(grademark1);

                                                }
                                            }
                                        }



                                    }
                                    else if (df1.Tables[0].Rows[0]["linkvalue"].ToString() == "1")
                                    {


                                        double totalmark = 0;
                                        for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                                        {
                                            grademark = 0.0;
                                            DataView dv = new DataView();
                                            ds3.Tables[0].DefaultView.RowFilter = "subject_code='" + ds3.Tables[0].Rows[i]["subject_code"] + "'";
                                            dv = ds3.Tables[0].DefaultView;
                                            for (int j = 0; j < dv.Count; j++)
                                            {

                                                //int count = hat.Count;
                                                if (ds3.Tables[0].Rows[j]["cp"] == DBNull.Value)
                                                {

                                                }
                                                else
                                                {
                                                    string mark = dv[j]["grade"].ToString();
                                                    double mark2 = Convert.ToInt32(ht[value]) * Convert.ToInt32(dv[j]["cp"]);
                                                    double mark1 = 0;

                                                    foreach (DictionaryEntry child in ht)
                                                    {


                                                        if (mark.ToString() == child.Key.ToString())
                                                        {
                                                            if (child.Value.ToString() == "0")
                                                            {



                                                            }
                                                            else
                                                            {
                                                                mark1 = Convert.ToInt32(child.Value) * Convert.ToInt32(dv[j]["cp"]);
                                                                grademark++;
                                                                flag_university = true;
                                                            }
                                                        }

                                                    }
                                                }


                                            }
                                            for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
                                            {
                                                if (ds.Tables[0].Rows[l]["subject_no"].ToString() == dv[0]["subject_no"].ToString())
                                                {
                                                    grademark1 = (grademark / dv.Count) * 100;
                                                    ds.Tables[0].Rows[l]["cammark"] = Math.Round(grademark1);

                                                }
                                            }

                                        }
                                    }
                                }
                                else if (university.Tables[0].Rows[0]["grade_flag"].ToString() == "1")
                                {
                                    string sqlroll1 = "Select acronym as subject_name,subject_code,subject.subject_no,result,total,grade,Registration.roll_no,cp,mark_entry.subject_no,subject.min_ext_marks,subject.min_int_marks,isnull(Mark_Entry.internal_mark,0) as internal_mark ,isnull(Mark_Entry.external_mark,0) as external_mark,semester,Sections from Mark_Entry,Subject,sub_sem,syllabus_master,Registration where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No =Subject.Subject_No and  subject.subtype_no= sub_sem.subtype_no and Registration.Roll_No=mark_entry.roll_no and  Exam_Code in(" + ds2.Tables[0].Rows[0]["Exam_Code"] + ") and Subject.subject_no in (" + subject_no + ") " + sec1 + " and CC=0 and DelFlag=0 and Exam_Flag<>'debar' order by semester desc,subject_type desc,subject.subject_no asc";
                                    ds3 = da.select_method_wo_parameter(sqlroll1, "Text");

                                    for (int j = 0; j < ds3.Tables[0].Rows.Count; j++)
                                    {
                                        grademark = 0.0;
                                        DataView dv = new DataView();
                                        ds3.Tables[0].DefaultView.RowFilter = "subject_code='" + ds3.Tables[0].Rows[j]["subject_code"] + "'";
                                        dv = ds3.Tables[0].DefaultView;
                                        for (int k = 0; k < dv.Count; k++)
                                        {
                                            double internal1 = Convert.ToDouble(dv[k]["internal_mark"].ToString());
                                            double external = Convert.ToDouble(dv[k]["external_mark"].ToString());
                                            double mark1 = Convert.ToDouble(dv[k]["min_ext_marks"].ToString());
                                            double mark2 = Convert.ToDouble(dv[k]["min_int_marks"].ToString());
                                            if (external == 0.0 && internal1 == 0.0)
                                            {
                                                flag_university = false;

                                            }
                                            else
                                            {
                                                if (internal1 >= mark2 && external >= mark1)
                                                {
                                                    grademark++;
                                                    flag_university = true;

                                                }

                                            }
                                        }
                                        for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
                                        {
                                            if (ds.Tables[0].Rows[l]["subject_no"].ToString() == dv[0]["subject_no"].ToString())
                                            {
                                                grademark1 = (grademark / dv.Count) * 100;
                                                ds.Tables[0].Rows[l]["cammark"] = Math.Round(grademark1);

                                            }
                                        }
                                    }


                                }
                                if (flag_university == true)
                                {

                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        Chart Chart1 = new Chart();
                                        Chart1.ChartAreas.Add("ChartArea1");

                                        Chart1.Series.Add("Series1").ChartType = SeriesChartType.Radar;
                                        Chart1.Series["Series1"].BorderColor = Color.Blue;
                                        Chart1.Series["Series1"]["RadarDrawingStyle"] = "marker";
                                        Chart1.Series["Series1"]["CircularLabelsStyle"] = "Horizontal";
                                        Chart1.Series["Series1"].MarkerSize = 7;
                                        Chart1.Series["Series1"].MarkerStyle = MarkerStyle.Star4;
                                        Chart1.Series["Series1"].MarkerColor = Color.DarkBlue;
                                        Chart1.Series["Series1"].MarkerBorderWidth = 7;
                                        Chart1.Series["Series1"].BorderWidth = 3;
                                        Chart1.ChartAreas["ChartArea1"].AxisY.Interval = 25;
                                        Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 100;
                                        Chart1.ChartAreas["ChartArea1"].AxisY.TitleForeColor = Chart1.ForeColor;
                                        Chart1.Series["Series1"].IsValueShownAsLabel = true;
                                        Chart1.Series["Series1"]["RadarDrawingStyle"] = "Line";
                                        Chart1.Series["Series1"].Color = Color.Red;
                                        Chart1.Series["Series1"].XValueMember = "acronym";
                                        Chart1.Series["Series1"].YValueMembers = "Cammark";
                                        Chart1.Series["Series1"].Font = new Font("Book Antiqua", 10, FontStyle.Bold);
                                        Chart1.Series["Series1"].Name = "University Subject wise over all class Percentage";
                                        Chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Book Antiqua", 14);
                                        Chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Green;
                                        Chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Book Antiqua", 9);
                                        Chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
                                        Chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Blue;
                                        Chart1.ChartAreas[0].AxisY.LineColor = Color.Chocolate;
                                        Title radarchart = Chart1.Titles.Add("University Subject Wise");
                                        radarchart.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                                        Chart1.Width = 900;
                                        Chart1.Height = 500;
                                        Chart1.DataSource = ds;
                                        Chart1.DataBind();
                                        panelchart.Controls.Add(Chart1);
                                        Chart1.SaveImage(Server.MapPath("App_Data/Sample.jpg"));
                                    }


                                }
                                else
                                {
                                    lblErrTest.Text = "No Records Found";
                                    lblErrTest.Visible = true;
                                    // Chart1.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            lblErrTest.Text = "No Records Found";
                            lblErrTest.Visible = true;
                            // Chart1.Visible = false;
                        }
                    }
                    else
                    {
                        lblErrTest.Text = "No Records Found";
                        lblErrTest.Visible = true;
                        // Chart1.Visible = false;
                    }
                }
                else
                {
                    lblErrTest.Text = "No Records Found";
                    lblErrTest.Visible = true;
                    // Chart1.Visible = false;
                }
            }

        }
        catch
        {
        }

    }
    protected void radiobutton1_selectedindexchanged(object sender, EventArgs e)
    {
        if (radiobutton1.Text == "University Wise")
        {
            txt_test.Text = "--Select--";
            Panel4.Visible = false;
            UpdatePanel_Department.Visible = false;
            lblTest.Visible = false;
            ddlTest.Visible = false;


        }
        else if (radiobutton1.Text == "CAM Wise")
        {
            txt_test.Text = "--Select--";
            rdTestWise.Checked = true;
            UpdatePanel_Department.Visible = true;
            Panel4.Visible = true;
            lblTest.Visible = true;
            ddlTest.Visible = true;
            lblTest.Text = "Test";
            test();



        }
    }


}