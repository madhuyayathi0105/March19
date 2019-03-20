using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Question_paper_generator : System.Web.UI.Page
{

    #region Field Declaration

    DAccess2 d2 = new DAccess2();

    DataSet ds = new DataSet();

    DataSet ds1 = new DataSet();

    string usercode = "", collegecode = "", singleuser = "", group_user = string.Empty;

    Hashtable hat = new Hashtable();
    bool isSchool = false;
    Hashtable availablequestion = new Hashtable();

    string questionType = string.Empty;
    string questionSubType = string.Empty;
    string qryQuestionType = string.Empty;

    public enum QuestionType
    {
        Objective = 0,
        Descriptive = 1
    };

    public enum ObjectiveQuestionType
    {
        MCQ = 1,
        blanks = 2,
        Matches = 3,
        TrueFalse = 4,
        Rearange = 5,
        ParagraphQuestionsWithOption = 6
    };

    public enum ObjectiveQuestionSubType
    {
        single = 1,
        multiple = 2,
        State_Vs_State = 3,
        State_Vs_Image = 4,
        Image_Vs_State = 5,
        Image_Vs_Image = 6
    };

    public enum QuestionGrade
    {
        easy = 0, medium = 1, difficult = 2, hard = 3
    };

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ////****************************************************//
        //string PageLogOut = "";
        //string sess = Convert.ToString(Session["IsLogin"]);
        //PageLogOut = Convert.ToString(Session["PageLogout"]);
        //if (sess == "")
        //{
        //}
        //else
        //{
        //    if (!Request.FilePath.Contains("QuesM"))
        //    {
        //        string strPreviousPage = "";
        //        if (Request.UrlReferrer != null)
        //        {
        //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //        }
        //        if (strPreviousPage == "")
        //        {
        //            string redrURI = ConfigurationManager.AppSettings["Question"].Trim();
        //            Response.Redirect(redrURI, false);
        //            return;
        //        }
        //    }
        //}
        //if (Session["collegecode"] == null)
        //{
        //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
        //    Response.Redirect(redrURI, false);
        //    return;
        //}
        ////****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("QuestionsMasterHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/QuestionMOD/QuestionsMasterHome.aspx");
                return;
            }
        }
        usercode = Convert.ToString(Session["usercode"]);
        collegecode = Convert.ToString(Session["collegecode"]);
        singleuser = Convert.ToString(Session["single_user"]);
        group_user = Convert.ToString(Session["group_code"]);

        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = Convert.ToString(group_semi[0]);
        }

        string grouporusercode1 = string.Empty;
        if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode1 = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else
        {
            grouporusercode1 = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }

        DataSet schoolds = new DataSet();
        string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode1 + "";
        schoolds.Clear();
        schoolds.Dispose();
        schoolds = d2.select_method_wo_parameter(sqlschool, "Text");
        if (schoolds.Tables[0].Rows.Count > 0)
        {
            string schoolvalue = Convert.ToString(schoolds.Tables[0].Rows[0]["value"]);
            if (schoolvalue.Trim() == "0")
            {
                isSchool = true;
            }
        }

        if (!IsPostBack)
        {
            bindcollege();
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
            PopulateTreeview();
            testname();
            lbl_testname.Visible = true;
            ddl_testname.Visible = true;
            lbl_month.Visible = false;
            ddl_month.Visible = false;
            lbl_year.Visible = false;
            ddl_year.Visible = false;
            month_year();
            ChangeHeaderName(isSchool);
        }
    }

    protected void btn_errorclose1_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
    }

    #region Bind Header

    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            ddl_collegename.Items.Clear();
            string clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    public void BindBatch()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlbatch.DataSource = ds;
                    ddlbatch.DataTextField = "Batch_year";
                    ddlbatch.DataValueField = "Batch_year";
                    ddlbatch.DataBind();
                    ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
                }
            }
        }
        catch (Exception ex)
        {

            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    public void BindDegree()
    {
        try
        {
            ddldegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddldegree.DataSource = ds;
                    ddldegree.DataTextField = "course_name";
                    ddldegree.DataValueField = "course_id";
                    ddldegree.DataBind();

                }
            }
        }
        catch (Exception ex)
        {

            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }

    }

    public void bindbranch()
    {
        try
        {
            string course_id = ddldegree.SelectedValue.ToString();
            ddlbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlbranch.DataSource = ds;
                    ddlbranch.DataTextField = "dept_name";
                    ddlbranch.DataValueField = "degree_code";
                    ddlbranch.DataBind();
                }
            }
        }
        catch (Exception ex)
        {

            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }

    }

    public void BindSectionDetail()
    {
        try
        {
            string strbatch = ddlbatch.SelectedValue.ToString();
            string strbranch = ddlbranch.SelectedValue.ToString();

            ddlsec.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSectionDetail(strbatch, strbranch);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataBind();
                if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
                {
                    ddlsec.Enabled = false;
                }
                else
                {
                    //ddlsec.Items.Insert(0, "All");
                    ddlsec.Enabled = true;
                }
            }
            else
            {
                ddlsec.Enabled = false;
            }
        }
        catch (Exception ex)
        {

            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    public void bindsem()
    {
        try
        {
            string strbatchyear = ddlbatch.Text.ToString();
            string strbranch = ddlbranch.SelectedValue.ToString();
            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSem(strbranch, strbatchyear, collegecode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]).ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    public void GetSubject()
    {
        try
        {
            string subjectquery = string.Empty;
            ddlsubject.Items.Clear();
            string sections = string.Empty;
            string strsec = string.Empty;
            if (ddlsec.Items.Count > 0)
            {
                sections = Convert.ToString(ddlsec.SelectedValue).Trim();
                if (Convert.ToString(ddlsec.SelectedValue).Trim().ToLower() == "all" || Convert.ToString(ddlsec.SelectedValue).Trim().ToLower() == "")
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and st.Sections='" + Convert.ToString(sections).Trim() + "'";
                }
            }

            string sems = string.Empty;
            if (ddlsem.Items.Count > 0)
            {
                if (Convert.ToString(ddlsem.SelectedValue).Trim() != "")
                {
                    if (Convert.ToString(ddlsem.SelectedValue).Trim() == "")
                    {
                        sems = string.Empty;
                    }
                    else
                    {
                        sems = "and SM.semester=" + Convert.ToString(ddlsem.SelectedValue).Trim() + "";
                    }


                    if (Convert.ToString(Session["Staff_Code"]).Trim() == "")
                    {
                        //subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and   SM.degree_code=" + ddlbranch.SelectedValue.ToString() + " " + sems.ToString() + " and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + ddlbatch.SelectedValue.ToString() + "' order by S.subject_no ";
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' order by S.subject_no ";
                    }
                    else if (Convert.ToString(Session["Staff_Code"]).Trim() != "")
                    {
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and st.subject_no=s.subject_no and s.syll_code=SM.syll_code and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' " + Convert.ToString(sems) + " and  SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "'  and sc.staff_code='" + Convert.ToString(Session["Staff_Code"]).Trim() + "' " + strsec + " order by S.subject_no ";
                    }
                    if (subjectquery != "")
                    {
                        ds.Dispose();
                        ds.Reset();
                        ds = d2.select_method(subjectquery, hat, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ddlsubject.Enabled = true;
                                ddlsubject.DataSource = ds;
                                ddlsubject.DataValueField = "Subject_No";
                                ddlsubject.DataTextField = "Subject_Name";
                                ddlsubject.DataBind();
                            }
                            else
                            {
                                ddlsubject.Enabled = false;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    public void testname()
    {
        try
        {
            string testname = string.Empty;
            ddl_testname.Items.Clear();


            string batch = Convert.ToString(ddlbatch.SelectedItem.Text);
            string degreecod = Convert.ToString(ddlbranch.SelectedItem.Value);
            string sem = string.Empty;
            if (ddlsem.Items.Count > 0)
            {
                sem = Convert.ToString(ddlsem.SelectedItem.Text);
            }

            string section = string.Empty;
            if (ddlsec.Items.Count > 0)
            {

                if (Convert.ToString(ddlsec.SelectedValue).Trim().ToLower() == "all" || Convert.ToString(ddlsec.SelectedValue).Trim().ToLower() == "")
                {
                    section = string.Empty;
                }
                else
                {
                    section = Convert.ToString(ddlsec.SelectedItem.Text);
                }

            }
            if (ddlsubject.Items.Count > 0)
            {
                if (batch != "" && degreecod != "" && sem != "")
                {
                    string getquery = " select c.criteria,c.Criteria_no from CriteriaForInternal c, syllabus_master sy where c.syll_code=sy.syll_code and sy.Batch_Year='" + batch + "' and sy.degree_code='" + degreecod + "' and sy.semester='" + sem + "'";
                    ds1 = d2.select_method_wo_parameter(getquery, "Text");
                    if (ds1.Tables.Count > 0)
                    {

                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            ddl_testname.Enabled = true;
                            ddl_testname.DataSource = ds1;
                            ddl_testname.DataValueField = "Criteria_no";
                            ddl_testname.DataTextField = "criteria";
                            ddl_testname.DataBind();
                        }
                        else
                        {
                            ddl_testname.Enabled = false;
                        }
                    }

                }
            }

        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    public void month_year()
    {
        var cbstrmonth = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

        ddl_month.Items.Clear();
        ddl_year.Items.Clear();

        for (int m = 0; m < cbstrmonth.Length; m++)
        {
            if (cbstrmonth[m] != "")
            {
                ddl_month.Items.Insert(m, new ListItem(cbstrmonth[m], Convert.ToString(m + 1)));
            }
        }
        //cbstrmonth = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
        string[] cbyear = new string[1];
        ArrayList cbsyear = new ArrayList();

        string max_yr = d2.GetFunction("select  max(Batch_Year) from Registration");
        string min_yr = d2.GetFunction("select min(Batch_Year) from Registration");
        int year = Convert.ToInt32(max_yr);
        int s = 0;
        for (int r = year; r > Convert.ToInt32(min_yr); r--)
        {
            if (s != 0)
            {
                Array.Resize(ref cbyear, cbyear.Length + 1);
            }
            cbyear[s] = Convert.ToString(r);
            //year--;
            ddl_year.Items.Insert(s, new ListItem(Convert.ToString(r), Convert.ToString(r)));
            s++;
            // r = r - 1;

        }
        if (max_yr == min_yr)
        {
            ddl_year.Items.Insert(s, new ListItem(Convert.ToString(max_yr), Convert.ToString(max_yr)));
        }
    }

    private DataTable GetMonth()
    {
        DataTable dtMon = new DataTable();
        DataRow drMon;
        try
        {
            dtMon.Rows.Clear();
            dtMon.Columns.Clear();
            dtMon.Columns.Add("Month_Name");
            dtMon.Columns.Add("Month_Value");
            var mon = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            for (int m = 0; m < mon.Length; m++)
            {
                if (mon[m] != "")
                {
                    drMon = dtMon.NewRow();
                    drMon["Month_Name"] = mon[m];
                    drMon["Month_Value"] = m + 1;
                    dtMon.Rows.Add(drMon);
                }
            }
        }
        catch (Exception ex)
        {

        }
        return dtMon;
    }

    public void ChangeHeaderName(bool isschool)
    {
        try
        {
            lbl_clg.Text = ((!isschool) ? "College" : "School");
            lblbatch.Text = ((!isschool) ? "Batch" : "Year");
            lbldegree.Text = ((!isschool) ? "Degree" : "School Type");
            lblbranch.Text = ((!isschool) ? "Department" : "Standard");
            lblsem.Text = ((!isschool) ? "Semester" : "Term");
            lblsec.Text = ((!isschool) ? "Section" : "Section");
        }
        catch (Exception ex)
        {

        }
    }

    #endregion Bind Header

    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBatch();
        BindDegree();
        bindbranch();
        bindsem();
        BindSectionDetail();
        GetSubject();
        PopulateTreeview();
        testname();
        objective_check.Visible = false;
    }

    protected void ddlsubject_Selectchanged(object sender, EventArgs e)
    {
        testname();
        PopulateTreeview();
        objective_check.Visible = false;
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDegree();
        bindbranch();
        bindsem();
        BindSectionDetail();
        GetSubject();
        PopulateTreeview();
        testname();
        objective_check.Visible = false;
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        BindSectionDetail();
        GetSubject();
        PopulateTreeview();
        testname();
        objective_check.Visible = false;
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
        BindSectionDetail();
        GetSubject();
        PopulateTreeview();
        testname();
        objective_check.Visible = false;
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSectionDetail();
        GetSubject();
        PopulateTreeview();
        testname();
        objective_check.Visible = false;
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSubject();
        PopulateTreeview();
        testname();
        objective_check.Visible = false;
    }

    public void btn_go_Click(object sender, EventArgs e)
    {
        grd_dynamic.Visible = false;
        cb_existqstn.Visible = false;
        cb_notexist.Visible = false;
        btn_gendrate.Visible = false;
        lblerrors.Visible = false;

        objectiveseet();
        objective_check.Visible = true;
        objectiveseet1();

    }

    public void objectiveseet()
    {
        try
        {
            ds1.Clear();
            string testcode = string.Empty;

            string qry = string.Empty;
            string subject = string.Empty;
            if (ddlsubject.Items.Count > 0)
            {
                subject = ddlsubject.SelectedItem.Value;
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Subject Were Found";
                grd_dynamic.Visible = false;
                return;
            }

            if (treeTopic.Nodes.Count == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "Please Add Syllubus To The Subject";
                grd_dynamic.Visible = false;
                return;
            }

            string SubjectTopicNo = string.Empty;
            string qryTopicNo = string.Empty;
            int selTopic = 0;
            ArrayList arrSelTopicParent = new ArrayList();

            Dictionary<string, int> dicTopicParent = new Dictionary<string, int>();
            selTopic = 0;
            arrSelTopicParent.Clear();
            dicTopicParent.Clear();
            for (int a = 0; a < treeTopic.CheckedNodes.Count; a++)
            {
                selTopic++;

                if (SubjectTopicNo == "")
                {
                    SubjectTopicNo = treeTopic.CheckedNodes[a].Value;
                    //SubjectTopicNo = treeTopic.CheckedNodes[a].Text;
                }
                else
                {
                    SubjectTopicNo = SubjectTopicNo + "," + treeTopic.CheckedNodes[a].Value;
                    //topicname = topicname + "," + treeTopic.CheckedNodes[a].Text;
                }
                if (treeTopic.CheckedNodes[a].Parent != null)
                {
                    if (!arrSelTopicParent.Contains(treeTopic.CheckedNodes[a].Parent.Value))
                    {
                        arrSelTopicParent.Add(treeTopic.CheckedNodes[a].Parent.Value);
                        if (SubjectTopicNo == "")
                        {
                            SubjectTopicNo = treeTopic.CheckedNodes[a].Parent.Value;
                            //SubjectTopicNo = treeTopic.CheckedNodes[a].Text;
                        }
                        else
                        {
                            SubjectTopicNo = SubjectTopicNo + "," + treeTopic.CheckedNodes[a].Parent.Value;
                            //topicname = topicname + "," + treeTopic.CheckedNodes[a].Text;
                        }
                    }
                    else
                    {

                    }
                }
            }
            if (selTopic > 0)
            {
                qryTopicNo = " and syllabus in(" + SubjectTopicNo + ")";
            }
            if (rb_internel.Checked == true)
            {
                if (ddl_testname.Items.Count > 0)
                {
                    testcode = ddl_testname.SelectedItem.Value;
                }
                else
                {
                    grd_dynamic.Visible = false;
                }
                qry = "select No_Sections,Section_Type,(select criteria from CriteriaForInternal where Convert(nvarchar(25),Criteria_no) = Exam) as [Exam] ,Exam as syllubuscod from tbl_question_bank_master where exam_type='2' and Exam='" + testcode + "' and Subject_no='" + subject + "'";
            }
            else
            {
                if (ddl_year.Items.Count > 0)
                {
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "Please Select Year";
                    grd_dynamic.Visible = false;
                    return;
                }

                string mont = ddl_month.SelectedItem.Value;
                string year = ddl_year.SelectedItem.Text;
                qry = "select No_Sections,Section_Type,Exam as syllubuscod from tbl_question_bank_master where Exam='Regular' and exam_type='1' and exam_month='" + mont + "' and exam_year='" + year + "' and Subject_no='" + subject + "'";
            }
            ds1 = d2.select_method_wo_parameter(qry, "Text");
            if (ds1.Tables.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    int rowIndex = Convert.ToInt32(ds1.Tables[0].Rows[0]["No_Sections"]);
                    string sectiontyp = Convert.ToString(ds1.Tables[0].Rows[0]["Section_Type"]);

                    if (rowIndex > 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Sno");
                        dt.Columns.Add("Section");
                        dt.Columns.Add("Amount");
                        DataRow dr;

                        string i;
                        char c2 = 'A';
                        char rom = 'I';
                        int autochar = 65;
                        int high = 50;
                        for (int row = 0; row < rowIndex; row++)
                        {
                            dr = dt.NewRow();
                            dr[0] = Convert.ToString(row + 1);
                            if (sectiontyp.Trim().ToLower() == "alpha")
                            {
                                dr[1] = (char)autochar;
                                autochar++;
                            }
                            else if (sectiontyp.Trim().ToLower() == "numeric")
                            {
                                dr[1] = Convert.ToString(row + 1);
                            }
                            else if (sectiontyp.Trim().ToLower() == "roman")
                            {
                                dr[1] = NumberToRoman(row + 1);
                            }
                            dt.Rows.Add(dr);
                            high = high + 30;
                        }
                        grd_dynamic.Height = high;
                        grd_dynamic.Width = 976;
                        if (dt.Rows.Count > 0)
                        {
                            grd_dynamic.DataSource = dt;
                            grd_dynamic.DataBind();
                            grd_dynamic.Visible = true;
                            objective_check.Visible = true;
                        }
                        grd_dynamic.Visible = true;
                        cb_existqstn.Visible = true;
                        cb_notexist.Visible = true;
                        btn_gendrate.Visible = true;
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No records Found";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No records Found";
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
            return;
        }
    }

    public void objectiveseet1()
    {
        try
        {
            if (ddlsubject.Items.Count <= 0)
            {
                grd_dynamic.Visible = false;
                lbl_alert1.Visible = true;
                lbl_alert1.Text = "No Subject Were Found";
                imgdiv2.Visible = true;
                return;
            }
            string subjects = ddlsubject.SelectedItem.Value;

            if (treeTopic.Nodes.Count == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "Please Add Syllubus To The Subject";
                grd_dynamic.Visible = false;
                return;
            }

            //cb_existqstn.Checked = true;
            //cb_notexist.Checked = false;
            ds1.Clear();
            string conition = string.Empty;
            if (rb_internel.Checked == true)
            {
                if (ddl_testname.Items.Count <= 0)
                {
                    grd_dynamic.Visible = false;
                    lbl_alert1.Visible = true;
                    lbl_alert1.Text = "Please Select Test Name";
                    imgdiv2.Visible = true;
                    return;
                }

                string testcode = ddl_testname.SelectedItem.Value;

                conition = " and eq.Test_code=qb.Exam and eq.is_internal='2' and eq.Test_code='" + testcode + "'  and eq.subject_no='" + subjects + "' ";
            }
            else if (rb_external.Checked == true)
            {
                string month = ddl_month.SelectedItem.Value;
                string year = ddl_year.SelectedItem.Value;

                conition = " and qb.exam_month=eq.Exam_month and eq.Exam_year=qb.exam_year and eq.is_internal='1' and eq.Exam_month='" + month + "'  and eq.Exam_year='" + year + "' and   eq.subject_no='" + subjects + "' ";
            }

            if (treeTopic.Nodes.Count > 0)
            {
                string qry1 = " select distinct eq.syllabus from tbl_question_bank_master qb, tbl_question_master tq,Exist_questions eq where tq.Subject_no=qb.Subject_no and eq.subject_no=qb.Subject_no and eq.subject_no=tq.subject_no and tq.QuestionMasterPK=eq.QuestionMasterFK  " + conition + " order by eq.syllabus";
                DataSet dsTopic = d2.select_method_wo_parameter(qry1, "Text");
                if (dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
                {
                    for (int topic = 0; topic < dsTopic.Tables[0].Rows.Count; topic++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dsTopic.Tables[0].Rows[topic]["syllabus"])))
                        {
                            for (int node = 0; node < treeTopic.Nodes.Count; node++)
                            {
                                AddNodeAndChildNodesToList(treeTopic.Nodes[node], Convert.ToString(dsTopic.Tables[0].Rows[topic]["syllabus"]));
                            }
                        }
                    }
                }
            }

            string SubjectTopicNo = string.Empty;
            string qryTopicNo = string.Empty;
            int selTopic = 0;
            ArrayList arrSelTopicParent = new ArrayList();

            Dictionary<string, int> dicTopicParent = new Dictionary<string, int>();
            selTopic = 0;
            arrSelTopicParent.Clear();
            dicTopicParent.Clear();
            for (int a = 0; a < treeTopic.CheckedNodes.Count; a++)
            {
                selTopic++;
                if (SubjectTopicNo == "")
                {
                    SubjectTopicNo = "'" + treeTopic.CheckedNodes[a].Value + "'";
                    //SubjectTopicNo = treeTopic.CheckedNodes[a].Text;
                }
                else
                {
                    SubjectTopicNo = SubjectTopicNo + ",'" + treeTopic.CheckedNodes[a].Value + "'";
                    //topicname = topicname + "," + treeTopic.CheckedNodes[a].Text;
                }
                if (treeTopic.CheckedNodes[a].Parent != null)
                {
                    if (!arrSelTopicParent.Contains(treeTopic.CheckedNodes[a].Parent.Value))
                    {
                        arrSelTopicParent.Add(treeTopic.CheckedNodes[a].Parent.Value);
                        if (SubjectTopicNo == "")
                        {
                            SubjectTopicNo = "'" + treeTopic.CheckedNodes[a].Parent.Value + "'";
                            //SubjectTopicNo = treeTopic.CheckedNodes[a].Text;
                        }
                        else
                        {
                            SubjectTopicNo = SubjectTopicNo + ",'" + treeTopic.CheckedNodes[a].Parent.Value + "'";
                            //topicname = topicname + "," + treeTopic.CheckedNodes[a].Text;
                        }
                    }
                    else
                    {

                    }
                }
            }


            if (selTopic > 0)
            {
                qryTopicNo = " and syllabus in(" + SubjectTopicNo + ")";
            }
            //   string qry = "select No_Sections,Section_Type,(select criteria from CriteriaForInternal where Convert(nvarchar(25),Criteria_no) = Exam) as [Exam] ,Exam as syllubuscod from tbl_question_bank_master where  Exam='" + testcode + "'";

            string qry = " select eq.subject_no,eq.Test_code,Must_attend,eq.Section,QuestionMasterPK,mark,is_descriptive,ISNULL(tq.QuestionType,0) as QuestionType,ISNULL(tq.QuestionSubType ,0) as QuestionSubType,question,type,eq.section_name,isnull(tq.Already_exist,0) as Already_exist from tbl_question_bank_master qb,tbl_question_master tq,Exist_questions eq where tq.Subject_no=qb.Subject_no and eq.subject_no=qb.Subject_no and eq.subject_no=tq.subject_no and tq.QuestionMasterPK=eq.QuestionMasterFK " + conition + " order by  eq.subject_no,eq.Test_code,eq.Section,eq.QNo";
            qry = qry + " select distinct eq.Section,mark,is_descriptive from tbl_question_bank_master qb, tbl_question_master tq,Exist_questions eq where tq.Subject_no=qb.Subject_no and eq.subject_no=qb.Subject_no and eq.subject_no=tq.subject_no and tq.QuestionMasterPK=eq.QuestionMasterFK " + conition + "";

            ds1 = d2.select_method_wo_parameter(qry, "Text");
            if (ds1.Tables.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0 && ds1.Tables[1].Rows.Count > 0)
                {
                    if (ds1.Tables[1].Rows.Count <= grd_dynamic.Rows.Count)
                    {
                        //object sumObject;
                        //sumObject = ds1.Tables[0].Compute("min(Already_exist)", "Already_exist is not null");
                        //int value = 0;
                        //int.TryParse(Convert.ToString(sumObject).Trim(), out value);

                        //if (value == 1)
                        //{
                        //    cb_notexist.Checked = true;
                        //}
                        //else
                        //{
                        //    cb_existqstn.Checked = false;
                        //}

                        for (int i = 0; i < ds1.Tables[1].Rows.Count; i++)
                        {
                            TextBox mrk = (TextBox)grd_dynamic.Rows[i].FindControl("txt_mark");
                            TextBox totqtn = (TextBox)grd_dynamic.Rows[i].FindControl("txt_noqustion");
                            TextBox esy = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Easy");
                            TextBox medm = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Medium");
                            TextBox diffict = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Difficult");
                            TextBox hard = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Hard");
                            CheckBox objec = (CheckBox)grd_dynamic.Rows[i].FindControl("rb_objct");
                            CheckBox descrp = (CheckBox)grd_dynamic.Rows[i].FindControl("rb_descrip");
                            Label section = (Label)grd_dynamic.Rows[i].FindControl("lbl_sec");
                            TextBox must = (TextBox)grd_dynamic.Rows[i].FindControl("txt_mustattnd");
                            Label availabl = (Label)grd_dynamic.Rows[i].FindControl("lbl_availabl");
                            Label easys = (Label)grd_dynamic.Rows[i].FindControl("lbl_easy");
                            Label med = (Label)grd_dynamic.Rows[i].FindControl("lbl_mediam");
                            Label diff = (Label)grd_dynamic.Rows[i].FindControl("lbl_diffc");
                            Label hards = (Label)grd_dynamic.Rows[i].FindControl("lbl_hard");
                            TextBox questionname = (TextBox)grd_dynamic.Rows[i].FindControl("txt_sec_name");

                            DropDownList ddlQuestionType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionType");
                            DropDownList ddlQuestionSubType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionSubType");
                            DropDownList ddlQuestionMatchType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionMatchType");
                            ddlQuestionType.Enabled = false;
                            ddlQuestionSubType.Visible = false;
                            ddlQuestionSubType.Enabled = false;
                            ddlQuestionMatchType.Enabled = false;
                            ddlQuestionMatchType.Visible = false;

                            questionType = string.Empty;
                            questionSubType = string.Empty;
                            qryQuestionType = string.Empty;

                            availabl.Text = string.Empty;
                            totqtn.Text = string.Empty;
                            easys.Text = string.Empty;
                            med.Text = string.Empty;
                            diff.Text = string.Empty;
                            hards.Text = string.Empty;
                            esy.Text = string.Empty;
                            medm.Text = string.Empty;
                            diffict.Text = string.Empty;
                            hard.Text = string.Empty;

                            DataView dv = new DataView();
                            string sectiontyp = Convert.ToString(ds1.Tables[1].Rows[i]["Section"]);
                            string mark1 = Convert.ToString(ds1.Tables[1].Rows[i]["mark"]);
                            string isdescriptive = Convert.ToString(ds1.Tables[1].Rows[i]["is_descriptive"]);

                            mrk.Text = mark1;
                            if (isdescriptive == "0")
                            {
                                objec.Checked = true;
                                DataTable dtQuesType = new DataTable();
                                ds1.Tables[0].DefaultView.RowFilter = "Section='" + sectiontyp + "'";
                                dtQuesType = ds1.Tables[0].DefaultView.ToTable(true, "QuestionType", "QuestionSubType");
                                //DataSet dsquesType = d2.select_method_wo_parameter("","text");
                                ddlQuestionType.Enabled = true;
                                if (dtQuesType.Rows.Count > 0)
                                {
                                    ddlQuestionType.SelectedValue = Convert.ToString(dtQuesType.Rows[0]["QuestionType"]).Trim();
                                    questionSubType = Convert.ToString(dtQuesType.Rows[0]["QuestionSubType"]).Trim();
                                    questionType = ddlQuestionType.SelectedValue.Trim();
                                    switch (questionType)
                                    {
                                        case "1":
                                        default:
                                            ddlQuestionSubType.Visible = true;
                                            ddlQuestionSubType.Enabled = true;
                                            switch (questionSubType)
                                            {
                                                case "1":
                                                default:
                                                    ddlQuestionSubType.Visible = true;
                                                    ddlQuestionSubType.Enabled = true;
                                                    if (questionSubType.Trim() != "0")
                                                    {
                                                        ddlQuestionSubType.SelectedValue = questionSubType;
                                                    }
                                                    else
                                                    {
                                                        ddlQuestionSubType.SelectedIndex = 0;
                                                    }
                                                    break;
                                                case "2":
                                                    ddlQuestionSubType.Visible = true;
                                                    ddlQuestionSubType.Enabled = true;
                                                    break;
                                                case "3":
                                                case "4":
                                                case "5":
                                                case "6":
                                                    ddlQuestionMatchType.Enabled = true;
                                                    ddlQuestionMatchType.Visible = true;
                                                    ddlQuestionMatchType.SelectedValue = questionSubType;
                                                    break;
                                            }
                                            break;
                                        case "2":
                                            break;
                                        case "3":
                                            ddlQuestionMatchType.Enabled = true;
                                            ddlQuestionMatchType.Visible = true;
                                            switch (questionSubType)
                                            {
                                                case "1":
                                                    break;
                                                case "2":
                                                    ddlQuestionSubType.Visible = true;
                                                    ddlQuestionSubType.Enabled = true;
                                                    break;
                                                case "3":
                                                case "4":
                                                case "5":
                                                case "6":
                                                default:
                                                    ddlQuestionMatchType.Enabled = true;
                                                    ddlQuestionMatchType.Visible = true;
                                                    if (questionSubType.Trim() != "0")
                                                    {
                                                        ddlQuestionMatchType.SelectedValue = questionSubType;
                                                    }
                                                    else
                                                    {
                                                        ddlQuestionMatchType.SelectedIndex = 0;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case "4":
                                            break;
                                        case "5":
                                            break;
                                        case "6":
                                            break;
                                    }
                                    if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                                    {
                                        qryQuestionType = " and QuestionType='" + questionType + "' and QuestionSubType='" + questionSubType + "'";
                                    }
                                    else if (!string.IsNullOrEmpty(questionType.Trim()) && questionType.Trim() != "0")
                                    {
                                        qryQuestionType = " and QuestionType='" + questionType + "'";
                                    }
                                }
                            }
                            else if (isdescriptive == "1")
                            {
                                descrp.Checked = true;
                            }

                            string Existqtns = string.Empty;
                            if (cb_notexist.Checked == true)
                            {
                                Existqtns = " and isnull(Already_exist,'0') <>'1'";
                            }
                            string avoil = d2.GetFunction("select  count(QuestionMasterPK) as total_question from tbl_question_master where subject_no='" + subjects + "' and is_descriptive='" + isdescriptive + "' and mark in('" + mark1 + "') " + Existqtns + qryTopicNo + qryQuestionType + "");

                            ds1.Tables[0].DefaultView.RowFilter = " Section='" + sectiontyp + "' and is_descriptive='" + isdescriptive + "' and mark='" + mark1 + "' " + qryQuestionType;

                            dv = ds1.Tables[0].DefaultView;

                            if (dv.Count > 0)
                            {
                                totqtn.Text = Convert.ToString(dv.Count);
                                string mustattend = Convert.ToString(dv[0]["Must_attend"]);
                                must.Text = mustattend;
                                questionname.Text = Convert.ToString(dv[0]["section_name"]);
                                availabl.Text = avoil;
                                DataTable dttabl = new DataTable();
                                dttabl = dv.ToTable();

                                for (int dvro = 0; dvro < 4; dvro++)
                                {
                                    DataView dvnext = new DataView();
                                    dttabl.DefaultView.RowFilter = "type='" + dvro + "'";
                                    dvnext = dttabl.DefaultView;
                                    if (dvnext.Count > 0)
                                    {
                                        if (dvro == 0)
                                        {
                                            esy.Text = Convert.ToString(dvnext.Count);
                                        }
                                        else if (dvro == 1)
                                        {
                                            medm.Text = Convert.ToString(dvnext.Count);
                                        }
                                        else if (dvro == 2)
                                        {
                                            diffict.Text = Convert.ToString(dvnext.Count);
                                        }
                                        else if (dvro == 3)
                                        {
                                            hard.Text = Convert.ToString(dvnext.Count);
                                        }
                                    }
                                }
                            }


                            string type = "  select count(*) no_of_question,type from tbl_question_master  where subject_no='" + subjects + "' and is_descriptive in ('" + isdescriptive + "')  " + Existqtns + qryQuestionType + qryTopicNo + " and  mark ='" + mark1 + "' group by type";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(type, "text");
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int qstion = 0; qstion < ds.Tables[0].Rows.Count; qstion++)
                                    {
                                        string typ = Convert.ToString(ds.Tables[0].Rows[qstion]["type"]);
                                        string noofqstion = Convert.ToString(ds.Tables[0].Rows[qstion]["no_of_question"]);
                                        if (typ != "")
                                        {
                                            if (typ == "0")
                                            {

                                                easys.Text = noofqstion;
                                            }
                                            else if (typ == "1")
                                            {

                                                med.Text = noofqstion;
                                            }
                                            else if (typ == "2")
                                            {

                                                diff.Text = noofqstion;
                                            }
                                            else if (typ == "3")
                                            {
                                                hards.Text = noofqstion;
                                            }
                                        }
                                    }

                                    if (easys.Text == "")
                                    {

                                        easys.Text = "0";
                                    }
                                    if (med.Text == "")
                                    {

                                        med.Text = "0";
                                    }
                                    if (diff.Text == "")
                                    {

                                        diff.Text = "0";
                                    }
                                    if (hards.Text == "")
                                    {
                                        hards.Text = "0";
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
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    public string NumberToRoman(int number)
    {
        if (number < 0 || number > 3999)
        {
            lbl_alert1.Text = "Value must be in the range 0 To 3,999";
            imgdiv2.Visible = true;
        }
        System.Text.StringBuilder result = new System.Text.StringBuilder();
        if (number == 0) return "I";
        int[] values = new int[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
        string[] numerals = new string[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
        for (int i = 0; i < 13; i++)
        {
            while (number >= values[i])
            {
                number -= values[i];
                result.Append(numerals[i]);
            }
        }
        return result.ToString();
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    public static Control GetPostBackControl(Page page)
    {
        Control control = null;
        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }

    public int rowIndxClicked()
    {
        int rownumber = -1;
        try
        {
            Control ctrlid = GetPostBackControl(this.Page);
            string rno = Convert.ToString(ctrlid.UniqueID).Split('$')[Convert.ToString(ctrlid.UniqueID).Split('$').Length - 2].Replace("ctl", "");
            int.TryParse(rno, out rownumber);
            rownumber -= 2;
        }
        catch { rownumber = -1; }

        return rownumber;
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        divPopQuesprepar.Visible = false;
    }

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        lbl_norec1.Visible = false;
        try
        {
            string reportname = txtexcelname1.Text;
            if (reportname.ToString().Trim() != "")
            {

                if (FpSpread1.Visible == true)
                {
                    d2.printexcelreport(FpSpread1, reportname);

                }

                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string dptname = "Question Preparation";
            string pagename = "Question_preparation.aspx";

            if (FpSpread1.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpSpread1, pagename, dptname);

            }

            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    public void format1()
    {
        try
        {
            Hashtable addpkhas = new Hashtable();
            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 6;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Visible = true;
            FarPoint.Web.Spread.CheckBoxCellType chk1 = new FarPoint.Web.Spread.CheckBoxCellType();
            chk1.AutoPostBack = true;
            FpSpread1.Width = 850;
            FpSpread1.Height = 500;
            FpSpread1.SaveChanges();
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Type";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Question Strenth";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Questions";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Option";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Answer";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;

            if (chk_answer.Checked == true)
            {
                FpSpread1.Columns[5].Visible = true;
            }
            else
            {
                FpSpread1.Columns[5].Visible = false;
            }

            if (chk_option.Checked == true)
            {
                FpSpread1.Columns[4].Visible = true;
            }
            else
            {
                FpSpread1.Columns[4].Visible = false;
            }

            string is_desc = string.Empty;
            string mark = string.Empty;
            string subno = Convert.ToString(ddlsubject.SelectedItem.Value);
            string Existqtns = string.Empty;

            string SubjectTopicNo = string.Empty;
            string qryTopicNo = string.Empty;
            int selTopic = 0;
            ArrayList arrSelTopicParent = new ArrayList();

            Dictionary<string, int> dicTopicParent = new Dictionary<string, int>();
            selTopic = 0;
            arrSelTopicParent.Clear();
            dicTopicParent.Clear();
            for (int a = 0; a < treeTopic.CheckedNodes.Count; a++)
            {
                selTopic++;

                if (SubjectTopicNo == "")
                {
                    SubjectTopicNo = "'" + treeTopic.CheckedNodes[a].Value + "'";
                    //SubjectTopicNo = treeTopic.CheckedNodes[a].Text;
                }
                else
                {
                    SubjectTopicNo = SubjectTopicNo + ",'" + treeTopic.CheckedNodes[a].Value + "'";
                    //topicname = topicname + "," + treeTopic.CheckedNodes[a].Text;
                }
                if (treeTopic.CheckedNodes[a].Parent != null)
                {
                    if (!arrSelTopicParent.Contains(treeTopic.CheckedNodes[a].Parent.Value))
                    {
                        arrSelTopicParent.Add(treeTopic.CheckedNodes[a].Parent.Value);
                        if (SubjectTopicNo == "")
                        {
                            SubjectTopicNo = "'" + treeTopic.CheckedNodes[a].Parent.Value + "'";
                            //SubjectTopicNo = treeTopic.CheckedNodes[a].Text;
                        }
                        else
                        {
                            SubjectTopicNo = SubjectTopicNo + ",'" + treeTopic.CheckedNodes[a].Parent.Value + "'";
                            //topicname = topicname + "," + treeTopic.CheckedNodes[a].Text;
                        }
                    }
                    else
                    {

                    }
                }
            }


            if (selTopic > 0)
            {
                qryTopicNo = " and syllabus in(" + SubjectTopicNo + ")";
            }

            if (cb_notexist.Checked == true)
            {
                Existqtns = " and isnull(Already_exist,'0') <>'1'";
            }
            ds.Clear();
            string sqry = " select QuestionMasterPK,question,mark,options,answer,syllabus,is_descriptive,subject_no,type,ISNULL(QuestionType,0) as QuestionType,ISNULL(QuestionSubType ,0) as QuestionSubType from tbl_question_master  where subject_no ='" + subno + "' " + Existqtns + qryTopicNo + "  order by NEWID()";
            ds = d2.select_method_wo_parameter(sqry, "Text");
            for (int i = 0; i < grd_dynamic.Rows.Count; i++)
            {
                TextBox mrk = (TextBox)grd_dynamic.Rows[i].FindControl("txt_mark");
                TextBox totqtn = (TextBox)grd_dynamic.Rows[i].FindControl("txt_noqustion");
                TextBox esy = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Easy");
                TextBox medm = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Medium");
                TextBox diffict = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Difficult");
                TextBox hard = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Hard");
                CheckBox objec = (CheckBox)grd_dynamic.Rows[i].FindControl("rb_objct");
                CheckBox descrp = (CheckBox)grd_dynamic.Rows[i].FindControl("rb_descrip");
                Label section = (Label)grd_dynamic.Rows[i].FindControl("lbl_sec");
                TextBox must = (TextBox)grd_dynamic.Rows[i].FindControl("txt_mustattnd");
                TextBox sectionnam = (TextBox)grd_dynamic.Rows[i].FindControl("txt_sec_name");

                DropDownList ddlQuestionType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionType");
                DropDownList ddlQuestionSubType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionSubType");
                DropDownList ddlQuestionMatchType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionMatchType");
                ddlQuestionType.Enabled = false;
                ddlQuestionSubType.Visible = false;
                ddlQuestionSubType.Enabled = false;
                ddlQuestionMatchType.Enabled = false;
                ddlQuestionMatchType.Visible = false;

                string questionType = string.Empty;
                string questionSubType = string.Empty;
                string qryQuestionType = string.Empty;

                is_desc = string.Empty;
                mark = mrk.Text;
                string sections = section.Text;
                if (mark != "")
                {
                    if (objec.Checked == true)
                    {
                        is_desc = "0";

                        ddlQuestionType.Enabled = true;
                        questionType = ddlQuestionType.SelectedValue.Trim();
                        switch (questionType)
                        {
                            case "1":
                            default:
                                ddlQuestionSubType.Visible = true;
                                ddlQuestionSubType.Enabled = true;
                                questionSubType = Convert.ToString(ddlQuestionSubType.SelectedValue).Trim();
                                switch (questionSubType)
                                {
                                    case "1":
                                    default:
                                        break;
                                    case "2":
                                        break;
                                }
                                break;
                            case "2":
                                break;
                            case "3":
                                ddlQuestionMatchType.Enabled = true;
                                ddlQuestionMatchType.Visible = true;
                                questionSubType = Convert.ToString(ddlQuestionMatchType.SelectedValue).Trim();
                                break;
                            case "4":
                                break;
                            case "5":
                                break;
                            case "6":
                                break;
                        }
                        if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                        {
                            qryQuestionType = " and QuestionType='" + questionType + "' and QuestionSubType='" + questionSubType + "'";
                        }
                        else if (!string.IsNullOrEmpty(questionType.Trim()) && questionType.Trim() != "0")
                        {
                            qryQuestionType = " and QuestionType='" + questionType + "'";
                        }
                    }
                    if (descrp.Checked == true)
                    {
                        is_desc = "1";
                    }
                    int easy = 0;
                    int mediam = 0;
                    int diff = 0;
                    int hards = 0;
                    if (esy.Text != "")
                    {
                        easy = Convert.ToInt32(esy.Text);
                    }
                    if (diffict.Text != "")
                    {
                        diff = Convert.ToInt32(diffict.Text);
                    } if (medm.Text != "")
                    {
                        mediam = Convert.ToInt32(medm.Text);
                    }
                    if (hard.Text != "")
                    {
                        hards = Convert.ToInt32(hard.Text);
                    }

                    string totalno = totqtn.Text;
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int sno = 1;
                            //for (int row = 0; i < ds.Tables[0].Rows.Count; i++)
                            //{
                            DataView dv = new DataView();

                            ds.Tables[0].DefaultView.RowFilter = "is_descriptive='" + is_desc + "' and mark='" + mark + "' " + qryQuestionType;

                            dv = ds.Tables[0].DefaultView;

                            if (dv.Count > 0)
                            {
                                DataTable dttabl = new DataTable();
                                dttabl = dv.ToTable();

                                for (int dvro = 0; dvro < 4; dvro++)
                                {
                                    DataView dvnext = new DataView();
                                    dttabl.DefaultView.RowFilter = "type='" + dvro + "'";
                                    dvnext = dttabl.DefaultView;
                                    int countvalu = 0;
                                    if (dvro == 0)
                                    {
                                        countvalu = easy;
                                    }
                                    else if (dvro == 1)
                                    {
                                        countvalu = mediam;
                                    }
                                    else if (dvro == 2)
                                    {
                                        countvalu = diff;
                                    }
                                    else if (dvro == 3)
                                    {
                                        countvalu = hards;
                                    }
                                    int rowcount = 0;
                                    if (dvnext.Count > 0)
                                    {
                                        for (int lrow = 0; lrow < dvnext.Count; lrow++)
                                        {
                                            string qustionpk = Convert.ToString(dvnext[lrow]["QuestionMasterPK"]);
                                            string type = Convert.ToString(dvnext[lrow]["type"]);

                                            if (!addpkhas.ContainsKey(qustionpk))
                                            {
                                                if (rowcount < countvalu)
                                                {
                                                    addpkhas.Add(qustionpk, type);

                                                    FpSpread1.Sheets[0].RowCount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dvnext[lrow]["QuestionMasterPK"]);

                                                    string desc = Convert.ToString(dvnext[lrow]["is_descriptive"]);

                                                    if (desc == "0")
                                                    {
                                                        desc = "Objective";
                                                    }
                                                    else if (desc == "1")
                                                    {
                                                        desc = "Descriptive";
                                                    }

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = desc;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = sections;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = sectionnam.Text;
                                                    if (type == "0")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Easy";
                                                    }
                                                    else if (type == "1")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Medium";
                                                    }
                                                    else if (type == "2")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Difficult";
                                                    }
                                                    else if (type == "3")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Hard";
                                                    }

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dvnext[lrow]["syllabus"]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dvnext[lrow]["question"]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = must.Text;
                                                    string optnvalu = string.Empty;
                                                    //if (optn.Contains(";"))
                                                    //{
                                                    //    optn = optn.Replace(";", ", ");
                                                    //}
                                                    string option = Convert.ToString(dvnext[lrow]["options"]).Trim();
                                                    if (option != "")
                                                    {
                                                        if (option.Contains(';'))
                                                        {
                                                            string[] split1 = option.Split(';');

                                                            for (int row = 0; row < split1.Length - 1; row++)
                                                            {
                                                                optnvalu = optnvalu + " " + Convert.ToString(row + 1) + "." + split1[row] + ", ";
                                                            }
                                                        }
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = optnvalu;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dvnext[lrow]["answer"]);
                                                    rowcount++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread1.Visible = true;
                        divPopQuesprepar.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    public void textvaldation()
    {
        try
        {
            lblerrors.Visible = false;
            lblerrors.Text = string.Empty;

            int rowindex = rowIndxClicked();
            for (int i = 0; i < grd_dynamic.Rows.Count; i++)
            {
                if (rowindex == i)
                {
                    TextBox totqtn = (TextBox)grd_dynamic.Rows[i].FindControl("txt_noqustion");
                    TextBox esy = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Easy");
                    TextBox medm = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Medium");
                    TextBox diffict = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Difficult");
                    TextBox hard = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Hard");
                    Label easys = (Label)grd_dynamic.Rows[i].FindControl("lbl_easy");
                    Label med = (Label)grd_dynamic.Rows[i].FindControl("lbl_mediam");
                    Label diff = (Label)grd_dynamic.Rows[i].FindControl("lbl_diffc");
                    Label hards = (Label)grd_dynamic.Rows[i].FindControl("lbl_hard");
                    TextBox must = (TextBox)grd_dynamic.Rows[i].FindControl("txt_mustattnd");

                    int total = 0;
                    if (esy.Text != "")
                    {
                        int esys = Convert.ToInt32(esy.Text);
                        total += esys;
                        if (easys.Text != "")
                        {
                            int lb_esy = Convert.ToInt32(easys.Text);

                            if (lb_esy < esys)
                            {
                                esy.Text = string.Empty;
                            }
                        }
                        else
                        {
                            esy.Text = string.Empty;
                        }

                    }
                    if (diffict.Text != "")
                    {
                        int diffic = Convert.ToInt32(diffict.Text);
                        total += diffic;
                        if (diff.Text != "")
                        {
                            int lbdiffcal = Convert.ToInt32(diff.Text);

                            if (diffic > lbdiffcal)
                            {
                                diffict.Text = string.Empty;
                            }
                        }
                        else
                        {
                            diffict.Text = string.Empty;
                        }


                    } if (medm.Text != "")
                    {
                        int txmed = Convert.ToInt32(medm.Text);
                        total += txmed;
                        if (med.Text != "")
                        {
                            int lbmed = Convert.ToInt32(med.Text);

                            if (lbmed < txmed)
                            {
                                medm.Text = string.Empty;
                            }
                        }
                        else
                        {
                            medm.Text = string.Empty;
                        }
                    }
                    if (hard.Text != "")
                    {
                        int txhard = Convert.ToInt32(hard.Text);
                        total += txhard;
                        if (hards.Text != "")
                        {
                            int lbhard = Convert.ToInt32(hards.Text);

                            if (lbhard < txhard)
                            {
                                hard.Text = string.Empty;
                            }
                        }
                        else
                        {
                            hard.Text = string.Empty;
                        }

                    }

                    string totalno = totqtn.Text;
                    if (totalno != "")
                    {

                        if (Convert.ToInt32(totalno) < Convert.ToInt32(total))
                        {
                            lblerrors.Visible = true;
                            lblerrors.Text = "Entered Question  Greater than No of Question count";
                            esy.Text = string.Empty;
                            medm.Text = string.Empty;
                            diffict.Text = string.Empty;
                            hard.Text = string.Empty;
                            return;
                        }
                    }
                    else
                    {
                        if (total != 0)
                        {
                            lblerrors.Visible = true;
                            lblerrors.Text = " Please Enter No of Question";
                            esy.Text = string.Empty;
                            medm.Text = string.Empty;
                            diffict.Text = string.Empty;
                            hard.Text = string.Empty;
                            return;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    public void txt_Easy_OnTextChanged(object sender, EventArgs e)
    {
        textvaldation();
    }

    public void txt_Medium_OnTextChanged(object sender, EventArgs e)
    {
        textvaldation();
    }

    public void txt_Difficult_OnTextChanged(object sender, EventArgs e)
    {
        textvaldation();
    }

    public void txt_Hard_OnTextChanged(object sender, EventArgs e)
    {
        textvaldation();
    }

    public void btn_gendrate_Click(object sender, EventArgs e)
    {
        try
        {
            Hashtable chk_desctype = new Hashtable();
            Hashtable existobject = new Hashtable();
            Hashtable queryhash = new Hashtable();
            string subno = ddlsubject.SelectedItem.Value;
            string checknoquetion = string.Empty;
            string chk_availabl = string.Empty;
            chk_answer.Checked = false;
            chk_option.Checked = false;

            string totalmark = string.Empty;

            for (int grd = 0; grd < grd_dynamic.Rows.Count; grd++)
            {

                TextBox mark = (TextBox)grd_dynamic.Rows[grd].FindControl("txt_mark");
                TextBox noqustion = (TextBox)grd_dynamic.Rows[grd].FindControl("txt_noqustion");
                TextBox easy = (TextBox)grd_dynamic.Rows[grd].FindControl("txt_Easy");
                TextBox medium = (TextBox)grd_dynamic.Rows[grd].FindControl("txt_Medium");
                TextBox diffical = (TextBox)grd_dynamic.Rows[grd].FindControl("txt_Difficult");
                TextBox hard = (TextBox)grd_dynamic.Rows[grd].FindControl("txt_Hard");
                RadioButton obj = (RadioButton)grd_dynamic.Rows[grd].FindControl("rb_objct");
                RadioButton descp = (RadioButton)grd_dynamic.Rows[grd].FindControl("rb_descrip");
                Label lbl_availablvalu = (Label)grd_dynamic.Rows[grd].FindControl("lbl_availabl");
                TextBox must = (TextBox)grd_dynamic.Rows[grd].FindControl("txt_mustattnd");
                TextBox section_nam = (TextBox)grd_dynamic.Rows[grd].FindControl("txt_sec_name");

                if (must.Text != "" && mark.Text != "")
                {
                    int mulmark = Convert.ToInt32(must.Text) * Convert.ToInt32(mark.Text);
                    totalmark = totalmark + mulmark;
                }
                if (noqustion.Text != "")
                {
                    checknoquetion = "1";
                }
                if (lbl_availablvalu.Text != "")
                {
                    chk_availabl = "1";
                }
                string marks = mark.Text;
                if (marks != "")
                {
                    if (lbl_availablvalu.Text == "")
                    {
                        lblerrors.Visible = true;
                        lblerrors.Text = " Questions Not available Find Sno " + Convert.ToString(grd + 1) + "";
                        return;
                    }
                }

                string isdes = string.Empty;
                if (obj.Checked == true)
                {
                    isdes = "0";
                }
                if (descp.Checked == true)
                {
                    isdes = "1";
                }
                string keyval = marks + "-" + isdes;
            }
            if (checknoquetion == "")
            {
                if (chk_availabl == "")
                {
                    lblerrors.Visible = true;
                    lblerrors.Text = "No Record Found ";
                    return;
                }
                else
                {
                    lblerrors.Visible = true;
                    lblerrors.Text = "Please Enter No of Quetions";
                    return;
                }
            }

            chk_desctype.Clear();
            lblerrors.Text = string.Empty;
            lblerrors.Visible = false;
            int rowindex = rowIndxClicked();
            if (grd_dynamic.Rows.Count > 0)
            {
                int rowcnt = 0;
                foreach (GridViewRow gvpopro in grd_dynamic.Rows)
                {
                    TextBox mark = (TextBox)gvpopro.Cells[2].FindControl("txt_mark");
                    TextBox noqustion = (TextBox)gvpopro.Cells[2].FindControl("txt_noqustion");
                    TextBox easy = (TextBox)gvpopro.Cells[3].FindControl("txt_Easy");
                    TextBox medium = (TextBox)gvpopro.Cells[4].FindControl("txt_Medium");
                    TextBox diffical = (TextBox)gvpopro.Cells[5].FindControl("txt_Difficult");
                    TextBox hard = (TextBox)gvpopro.Cells[6].FindControl("txt_Hard");
                    CheckBox obj = (CheckBox)gvpopro.Cells[6].FindControl("rb_objct");
                    CheckBox descp = (CheckBox)gvpopro.Cells[6].FindControl("rb_descrip");
                    string marks = mark.Text;
                    string marktyp = marks;
                    int total = 0;
                    if (marks != "")
                    {
                        int mar = 0;
                        int.TryParse(easy.Text, out mar);
                        int esy = 0;
                        int.TryParse(easy.Text, out esy);
                        int medm = 0;
                        int.TryParse(medium.Text, out medm);
                        int diff = 0;
                        int.TryParse(diffical.Text, out diff);
                        int hrd = 0;
                        int.TryParse(hard.Text, out hrd);
                        total = esy + medm + diff + hrd;
                    }

                    rowcnt++;
                    int totqustion = 0;
                    int.TryParse(noqustion.Text, out totqustion);
                    if (totqustion != 0)
                    {
                        if (totqustion != total)
                        {
                            lblerrors.Visible = true;
                            lblerrors.Text = "Count  not Equal for Type Total count Find S.No: " + rowcnt + "";
                            return;
                        }
                    }
                }
            }
            format1();
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    public void checkval()
    {
        lblerrors.Text = string.Empty;
        Hashtable existobject = new Hashtable();
        int rowindex = rowIndxClicked();

        //Hashtable queryhash = new Hashtable();
        //string subno = ddlsubject.SelectedItem.Value;
        //string hashquery = "SELECT COUNT(*) no_qu,mark,type,is_descriptive FROM tbl_question_master where subject_no='" + subno + "' group by mark,type,is_descriptive ";
        //ds = d2.select_method_wo_parameter(hashquery, "Text");
        //if (ds.Tables.Count > 0)
        //{
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        for (int has = 0; has < ds.Tables[0].Rows.Count; has++)
        //        {

        //            string hashkey = Convert.ToString(ds.Tables[0].Rows[has]["mark"]) + "-" + Convert.ToString(ds.Tables[0].Rows[has]["is_descriptive"]) + "-" + Convert.ToString(ds.Tables[0].Rows[has]["type"]);
        //            string hashvalu = Convert.ToString(ds.Tables[0].Rows[has]["no_qu"]);
        //            if (!queryhash.ContainsKey(hashkey))
        //            {
        //                queryhash.Add(hashkey, hashvalu);
        //            }
        //        }
        //    }
        //}
        //Hashtable gridtypehash = new Hashtable();
        if (treeTopic.Nodes.Count == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert1.Text = "Please Add Syllubus To The Subject";
            grd_dynamic.Visible = false;
            return;
        }

        string SubjectTopicNo = string.Empty;
        string qryTopicNo = string.Empty;
        int selTopic = 0;
        ArrayList arrSelTopicParent = new ArrayList();

        Dictionary<string, int> dicTopicParent = new Dictionary<string, int>();
        selTopic = 0;
        arrSelTopicParent.Clear();
        dicTopicParent.Clear();
        for (int a = 0; a < treeTopic.CheckedNodes.Count; a++)
        {
            selTopic++;
            if (SubjectTopicNo == "")
            {
                SubjectTopicNo = "'" + treeTopic.CheckedNodes[a].Value + "'";
                //SubjectTopicNo = treeTopic.CheckedNodes[a].Text;
            }
            else
            {
                SubjectTopicNo = SubjectTopicNo + ",'" + treeTopic.CheckedNodes[a].Value + "'";
                //topicname = topicname + "," + treeTopic.CheckedNodes[a].Text;
            }
            if (treeTopic.CheckedNodes[a].Parent != null)
            {
                if (!arrSelTopicParent.Contains(treeTopic.CheckedNodes[a].Parent.Value))
                {
                    arrSelTopicParent.Add(treeTopic.CheckedNodes[a].Parent.Value);
                    if (SubjectTopicNo == "")
                    {
                        SubjectTopicNo = "'" + treeTopic.CheckedNodes[a].Parent.Value + "'";
                        //SubjectTopicNo = treeTopic.CheckedNodes[a].Text;
                    }
                    else
                    {
                        SubjectTopicNo = SubjectTopicNo + ",'" + treeTopic.CheckedNodes[a].Parent.Value + "'";
                        //topicname = topicname + "," + treeTopic.CheckedNodes[a].Text;
                    }
                }
                else
                {

                }
            }
        }

        if (selTopic > 0)
        {
            qryTopicNo = " and syllabus in(" + SubjectTopicNo + ")";
        }
        for (int i = 0; i < grd_dynamic.Rows.Count; i++)
        {
            string subno = Convert.ToString(ddlsubject.SelectedItem.Value);
            TextBox totno = (TextBox)grd_dynamic.Rows[i].FindControl("txt_noqustion");
            Label easys = (Label)grd_dynamic.Rows[i].FindControl("lbl_easy");
            Label med = (Label)grd_dynamic.Rows[i].FindControl("lbl_mediam");
            Label diff = (Label)grd_dynamic.Rows[i].FindControl("lbl_diffc");
            Label hards = (Label)grd_dynamic.Rows[i].FindControl("lbl_hard");
            TextBox mrk = (TextBox)grd_dynamic.Rows[i].FindControl("txt_mark");
            TextBox esy = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Easy");
            TextBox medm = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Medium");
            TextBox diffict = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Difficult");
            TextBox hard = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Hard");
            RadioButton chkobj = (RadioButton)grd_dynamic.Rows[i].FindControl("rb_objct");
            RadioButton chjkdes = (RadioButton)grd_dynamic.Rows[i].FindControl("rb_descrip");
            Label availabl = (Label)grd_dynamic.Rows[i].FindControl("lbl_availabl");
            TextBox must = (TextBox)grd_dynamic.Rows[i].FindControl("txt_mustattnd");

            DropDownList ddlQuestionType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionType");
            DropDownList ddlQuestionSubType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionSubType");
            DropDownList ddlQuestionMatchType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionMatchType");
            ddlQuestionType.Enabled = false;
            ddlQuestionSubType.Visible = false;
            ddlQuestionSubType.Enabled = false;
            ddlQuestionMatchType.Enabled = false;
            ddlQuestionMatchType.Visible = false;

            string questionType = string.Empty;
            string questionSubType = string.Empty;
            string qryQuestionType = string.Empty;

            string marks = mrk.Text;
            string totnoqustion = totno.Text;
            string isdesc = string.Empty;
            if (chkobj.Checked == true)
            {
                isdesc = "0";
                ddlQuestionType.Enabled = true;
                questionType = ddlQuestionType.SelectedValue.Trim();
                switch (questionType)
                {
                    case "1":
                    default:
                        ddlQuestionSubType.Visible = true;
                        ddlQuestionSubType.Enabled = true;
                        questionSubType = Convert.ToString(ddlQuestionSubType.SelectedValue).Trim();
                        switch (questionSubType)
                        {
                            case "1":
                            default:
                                break;
                            case "2":
                                break;
                        }
                        break;
                    case "2":
                        break;
                    case "3":
                        ddlQuestionMatchType.Enabled = true;
                        ddlQuestionMatchType.Visible = true;
                        questionSubType = Convert.ToString(ddlQuestionMatchType.SelectedValue).Trim();
                        break;
                    case "4":
                        break;
                    case "5":
                        break;
                    case "6":
                        break;
                }
                if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                {
                    qryQuestionType = " and QuestionType='" + questionType + "' and QuestionSubType='" + questionSubType + "'";
                }
                else if (!string.IsNullOrEmpty(questionType.Trim()) && questionType.Trim() != "0")
                {
                    qryQuestionType = " and QuestionType='" + questionType + "'";
                }
            }
            else if (chjkdes.Checked == true)
            {
                isdesc = "1";
            }
            if (marks != "" && totnoqustion != "")
            {
                int avl = Convert.ToInt32(availabl.Text) - Convert.ToInt32(totnoqustion);

                string keyval = marks + "-" + isdesc + ((!string.IsNullOrEmpty(questionType.Trim())) ? "-" + questionType.Trim() : "");
                if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                {
                    keyval += "-" + questionSubType.Trim();
                }

                if (!existobject.ContainsKey(keyval))
                {
                    existobject.Add(keyval, avl);
                }
                else
                {
                    existobject[keyval] = avl;
                }
            }
            //if (marks != "")
            //{
            //    string valus = string.Empty;
            //    string keyval = marks + "-" + isdesc;

            //    if(esy.Text!="")
            //    {
            //        valus = esy.Text;
            //        keyval = keyval + "-" + "0";
            //    }
            //    else if (medm.Text!="")
            //    {
            //        valus =  medm.Text ;
            //        keyval = keyval + "-" + "1";

            //    }
            //    else if (diffict.Text != "")
            //    {
            //        valus =  diffict.Text ;
            //        keyval = keyval + "-" + "2";

            //    }
            //    else if (hard.Text != "")
            //    {
            //        valus = hard.Text;
            //        keyval = keyval + "-" + "3";

            //    }
            //    if (valus!="")
            //    {
            //        if (!gridtypehash.ContainsKey(keyval))
            //    {
            //        gridtypehash.Add(keyval, valus);
            //    }
            //    }
            //}

            if (rowindex == i)
            {
                totno.Text = string.Empty;
                easys.Text = string.Empty;
                med.Text = string.Empty;
                diff.Text = string.Empty;
                hards.Text = string.Empty;
                esy.Text = string.Empty;
                medm.Text = string.Empty;
                diffict.Text = string.Empty;
                hard.Text = string.Empty;
                must.Text = string.Empty;
                string Existqtns = string.Empty;
                if (cb_notexist.Checked == true)
                {
                    Existqtns = " and isnull(Already_exist,'0') <>'1'";
                }
                string compare = d2.GetFunction("select  count(QuestionMasterPK) as total_question from tbl_question_master where subject_no='" + subno + "' and mark ='" + mrk.Text + "' and is_descriptive in('" + isdesc + "') " + Existqtns + qryQuestionType + qryTopicNo + " ");
                if (compare != "0")
                {
                    int available = Convert.ToInt32(compare);
                    string keyval = marks + "-" + isdesc + ((!string.IsNullOrEmpty(questionType.Trim())) ? "-" + questionType.Trim() : ""); ;
                    if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                    {
                        keyval += "-" + questionSubType.Trim();
                    }

                    if (existobject.ContainsKey(keyval))
                    {
                        int getmark = Convert.ToInt32(existobject[keyval]);
                        if (available == getmark)
                        {
                            available = getmark;
                        }
                        else
                        {
                            existobject[keyval] = available;
                        }
                    }
                    availabl.Text = Convert.ToString(available);
                }
                else
                {
                    availabl.Text = string.Empty;
                }
                {
                    string type = "  select count(*) no_of_question,type from tbl_question_master  where subject_no='" + subno + "' and is_descriptive in ('" + isdesc + "')  " + Existqtns + "and  mark ='" + marks + "' " + qryQuestionType + qryTopicNo + " group by type";
                    ds1.Clear();
                    ds1 = d2.select_method_wo_parameter(type, "text");
                    if (ds1.Tables.Count > 0)
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            for (int qstion = 0; qstion < ds1.Tables[0].Rows.Count; qstion++)
                            {
                                string typ = Convert.ToString(ds1.Tables[0].Rows[qstion]["type"]);
                                string noofqstion = Convert.ToString(ds1.Tables[0].Rows[qstion]["no_of_question"]);
                                if (typ != "")
                                {
                                    if (typ == "0")
                                    {
                                        easys.Text = noofqstion;
                                    }
                                    else if (typ == "1")
                                    {
                                        med.Text = noofqstion;
                                    }
                                    else if (typ == "2")
                                    {
                                        diff.Text = noofqstion;
                                    }
                                    else if (typ == "3")
                                    {
                                        hards.Text = noofqstion;
                                    }
                                }
                            }
                            if (easys.Text == "")
                            {
                                easys.Text = "0";
                            }
                            if (med.Text == "")
                            {
                                med.Text = "0";
                            }
                            if (diff.Text == "")
                            {
                                diff.Text = "0";
                            }
                            if (hards.Text == "")
                            {
                                hards.Text = "0";
                            }
                        }
                    }
                }
            }
        }
    }

    protected void rb_objct_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            checkval();
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    protected void rb_descrip_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            checkval();
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    protected void cb_existqstn_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrors.Text = string.Empty;
            Hashtable existobject = new Hashtable();
            int rowindex = rowIndxClicked();
            for (int i = 0; i < grd_dynamic.Rows.Count; i++)
            {
                TextBox mrk = (TextBox)grd_dynamic.Rows[i].FindControl("txt_mark");
                string subno = Convert.ToString(ddlsubject.SelectedItem.Value);
                RadioButton chkobj = (RadioButton)grd_dynamic.Rows[i].FindControl("rb_objct");
                RadioButton chjkdes = (RadioButton)grd_dynamic.Rows[i].FindControl("rb_descrip");
                TextBox esy = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Easy");
                TextBox medm = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Medium");
                TextBox diffict = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Difficult");
                TextBox hard = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Hard");
                TextBox totalno = (TextBox)grd_dynamic.Rows[i].FindControl("txt_noqustion");
                Label easys = (Label)grd_dynamic.Rows[i].FindControl("lbl_easy");
                Label med = (Label)grd_dynamic.Rows[i].FindControl("lbl_mediam");
                Label diff = (Label)grd_dynamic.Rows[i].FindControl("lbl_diffc");
                Label hards = (Label)grd_dynamic.Rows[i].FindControl("lbl_hard");
                TextBox must = (TextBox)grd_dynamic.Rows[i].FindControl("txt_mustattnd");
                Label availabl = (Label)grd_dynamic.Rows[i].FindControl("lbl_availabl");

                DropDownList ddlQuestionType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionType");
                DropDownList ddlQuestionSubType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionSubType");
                DropDownList ddlQuestionMatchType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionMatchType");
                ddlQuestionType.Enabled = false;
                ddlQuestionSubType.Visible = false;
                ddlQuestionSubType.Enabled = false;
                ddlQuestionMatchType.Enabled = false;
                ddlQuestionMatchType.Visible = false;

                string questionType = string.Empty;
                string questionSubType = string.Empty;
                string qryQuestionType = string.Empty;

                esy.Text = string.Empty;
                medm.Text = string.Empty;
                diffict.Text = string.Empty;
                hard.Text = string.Empty;
                totalno.Text = string.Empty;
                easys.Text = string.Empty;
                med.Text = string.Empty;
                diff.Text = string.Empty;
                hards.Text = string.Empty;
                must.Text = string.Empty;
                string isdesc = string.Empty;
                if (chkobj.Checked == true)
                {
                    if (chjkdes.Checked == true)
                    {
                        isdesc = "0','1";
                    }
                    else
                    {
                        isdesc = "0";
                    }
                    ddlQuestionType.Enabled = true;
                    questionType = ddlQuestionType.SelectedValue.Trim();
                    switch (questionType)
                    {
                        case "1":
                        default:
                            ddlQuestionSubType.Visible = true;
                            ddlQuestionSubType.Enabled = true;
                            questionSubType = Convert.ToString(ddlQuestionSubType.SelectedValue).Trim();
                            switch (questionSubType)
                            {
                                case "1":
                                default:
                                    break;
                                case "2":
                                    break;
                            }
                            break;
                        case "2":
                            break;
                        case "3":
                            ddlQuestionMatchType.Enabled = true;
                            ddlQuestionMatchType.Visible = true;
                            questionSubType = Convert.ToString(ddlQuestionMatchType.SelectedValue).Trim();
                            break;
                        case "4":
                            break;
                        case "5":
                            break;
                        case "6":
                            break;
                    }
                    if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                    {
                        qryQuestionType = " and QuestionType='" + questionType + "' and QuestionSubType='" + questionSubType + "'";
                    }
                    else if (!string.IsNullOrEmpty(questionType.Trim()) && questionType.Trim() != "0")
                    {
                        qryQuestionType = " and QuestionType='" + questionType + "'";
                    }

                }
                else if (chjkdes.Checked == true)
                {

                    isdesc = "1";
                }
                else
                {
                    isdesc = "0','1";
                    ddlQuestionType.Enabled = true;
                    questionType = ddlQuestionType.SelectedValue.Trim();
                    switch (questionType)
                    {
                        case "1":
                        default:
                            ddlQuestionSubType.Visible = true;
                            ddlQuestionSubType.Enabled = true;
                            questionSubType = Convert.ToString(ddlQuestionSubType.SelectedValue).Trim();
                            switch (questionSubType)
                            {
                                case "1":
                                default:
                                    break;
                                case "2":
                                    break;
                            }
                            break;
                        case "2":
                            break;
                        case "3":
                            ddlQuestionMatchType.Enabled = true;
                            ddlQuestionMatchType.Visible = true;
                            questionSubType = Convert.ToString(ddlQuestionMatchType.SelectedValue).Trim();
                            break;
                        case "4":
                            break;
                        case "5":
                            break;
                        case "6":
                            break;
                    }
                    if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                    {
                        qryQuestionType = " and QuestionType='" + questionType + "' and QuestionSubType='" + questionSubType + "'";
                    }
                    else if (!string.IsNullOrEmpty(questionType.Trim()) && questionType.Trim() != "0")
                    {
                        qryQuestionType = " and QuestionType='" + questionType + "'";
                    }
                }
                if (treeTopic.Nodes.Count == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "Please Add Syllubus To The Subject";
                    grd_dynamic.Visible = false;
                    return;
                }

                string SubjectTopicNo = string.Empty;
                string qryTopicNo = string.Empty;
                int selTopic = 0;
                ArrayList arrSelTopicParent = new ArrayList();

                Dictionary<string, int> dicTopicParent = new Dictionary<string, int>();
                selTopic = 0;
                arrSelTopicParent.Clear();
                dicTopicParent.Clear();
                for (int a = 0; a < treeTopic.CheckedNodes.Count; a++)
                {
                    selTopic++;
                    if (SubjectTopicNo == "")
                    {
                        SubjectTopicNo = "'" + treeTopic.CheckedNodes[a].Value + "'";
                        //SubjectTopicNo = treeTopic.CheckedNodes[a].Text;
                    }
                    else
                    {
                        SubjectTopicNo = SubjectTopicNo + ",'" + treeTopic.CheckedNodes[a].Value + "'";
                        //topicname = topicname + "," + treeTopic.CheckedNodes[a].Text;
                    }
                    if (treeTopic.CheckedNodes[a].Parent != null)
                    {
                        if (!arrSelTopicParent.Contains(treeTopic.CheckedNodes[a].Parent.Value))
                        {
                            arrSelTopicParent.Add(treeTopic.CheckedNodes[a].Parent.Value);
                            if (SubjectTopicNo == "")
                            {
                                SubjectTopicNo = "'" + treeTopic.CheckedNodes[a].Parent.Value + "'";
                                //SubjectTopicNo = treeTopic.CheckedNodes[a].Text;
                            }
                            else
                            {
                                SubjectTopicNo = SubjectTopicNo + ",'" + treeTopic.CheckedNodes[a].Parent.Value + "'";
                                //topicname = topicname + "," + treeTopic.CheckedNodes[a].Text;
                            }
                        }
                        else
                        {

                        }
                    }
                }

                if (selTopic > 0)
                {
                    qryTopicNo = " and syllabus in(" + SubjectTopicNo + ")";
                }

                string Existqtns = string.Empty;
                if (cb_notexist.Checked == true)
                {
                    Existqtns = " and isnull(Already_exist,'0') <>'1'";
                }
                string marks = mrk.Text;
                string totnoqustion = totalno.Text;
                if (marks != "" && totnoqustion != "")
                {
                    int avl = Convert.ToInt32(availabl.Text) - Convert.ToInt32(totnoqustion);

                    string keyval = marks + "-" + isdesc + ((!string.IsNullOrEmpty(questionType.Trim())) ? "-" + questionType.Trim() : "");
                    if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                    {
                        keyval += "-" + questionSubType.Trim();
                    }

                    if (!existobject.ContainsKey(keyval))
                    {
                        existobject.Add(keyval, avl);
                    }
                    else
                    {
                        existobject[keyval] = avl;
                    }
                }

                string compare = d2.GetFunction("select  count(QuestionMasterPK) as total_question from tbl_question_master where subject_no='" + subno + "' and mark ='" + mrk.Text + "' and is_descriptive in('" + isdesc + "') " + Existqtns + qryTopicNo + qryQuestionType + " ");

                if (compare != "0")
                {
                    availabl.Text = compare;
                    int available = Convert.ToInt32(compare);
                    string keyval = marks + "-" + isdesc + ((!string.IsNullOrEmpty(questionType.Trim())) ? "-" + questionType.Trim() : ""); ;
                    if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                    {
                        keyval += "-" + questionSubType.Trim();
                    }

                    if (existobject.ContainsKey(keyval))
                    {
                        int getmark = Convert.ToInt32(existobject[keyval]);
                        if (available == getmark)
                        {
                            available = getmark;
                        }
                        else
                        {
                            existobject[keyval] = available;
                        }
                    }
                    availabl.Text = Convert.ToString(available);
                }
                else
                {
                    //Label availabl = (Label)grd_dynamic.Rows[i].FindControl("lbl_availabl");
                    availabl.Text = string.Empty;
                }
                string type = "  select count(*) no_of_question,type from tbl_question_master  where subject_no='" + subno + "' and is_descriptive in ('" + isdesc + "')  " + Existqtns + qryTopicNo + qryQuestionType + "and  mark ='" + mrk.Text + "' group by type";
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(type, "text");
                if (ds1.Tables.Count > 0)
                {
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        for (int qstion = 0; qstion < ds1.Tables[0].Rows.Count; qstion++)
                        {
                            string typ = Convert.ToString(ds1.Tables[0].Rows[qstion]["type"]);
                            string noofqstion = Convert.ToString(ds1.Tables[0].Rows[qstion]["no_of_question"]);
                            if (typ != "")
                            {
                                if (typ == "0")
                                {
                                    easys.Text = noofqstion;
                                }
                                else if (typ == "1")
                                {
                                    med.Text = noofqstion;
                                }
                                else if (typ == "2")
                                {
                                    diff.Text = noofqstion;
                                }
                                else if (typ == "3")
                                {
                                    hards.Text = noofqstion;
                                }
                            }
                        }
                        if (easys.Text == "")
                        {
                            easys.Text = "0";
                        }
                        if (med.Text == "")
                        {
                            med.Text = "0";
                        }
                        if (diff.Text == "")
                        {
                            diff.Text = "0";
                        }
                        if (hards.Text == "")
                        {
                            hards.Text = "0";
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    protected void cb_notexist_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrors.Text = string.Empty;
            Hashtable existobject = new Hashtable();
            int rowindex = rowIndxClicked();
            for (int i = 0; i < grd_dynamic.Rows.Count; i++)
            {
                TextBox mrk = (TextBox)grd_dynamic.Rows[i].FindControl("txt_mark");
                string subno = Convert.ToString(ddlsubject.SelectedItem.Value);
                RadioButton chkobj = (RadioButton)grd_dynamic.Rows[i].FindControl("rb_objct");
                RadioButton chjkdes = (RadioButton)grd_dynamic.Rows[i].FindControl("rb_descrip");
                TextBox esy = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Easy");
                TextBox medm = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Medium");
                TextBox diffict = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Difficult");
                TextBox hard = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Hard");
                TextBox totalno = (TextBox)grd_dynamic.Rows[i].FindControl("txt_noqustion");
                Label easys = (Label)grd_dynamic.Rows[i].FindControl("lbl_easy");
                Label med = (Label)grd_dynamic.Rows[i].FindControl("lbl_mediam");
                Label diff = (Label)grd_dynamic.Rows[i].FindControl("lbl_diffc");
                Label hards = (Label)grd_dynamic.Rows[i].FindControl("lbl_hard");
                TextBox must = (TextBox)grd_dynamic.Rows[i].FindControl("txt_mustattnd");
                Label availabl = (Label)grd_dynamic.Rows[i].FindControl("lbl_availabl");

                DropDownList ddlQuestionType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionType");
                DropDownList ddlQuestionSubType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionSubType");
                DropDownList ddlQuestionMatchType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionMatchType");
                ddlQuestionType.Enabled = false;
                ddlQuestionSubType.Visible = false;
                ddlQuestionSubType.Enabled = false;
                ddlQuestionMatchType.Enabled = false;
                ddlQuestionMatchType.Visible = false;

                string questionType = string.Empty;
                string questionSubType = string.Empty;
                string qryQuestionType = string.Empty;

                esy.Text = string.Empty;
                medm.Text = string.Empty;
                diffict.Text = string.Empty;
                hard.Text = string.Empty;
                totalno.Text = string.Empty;
                easys.Text = string.Empty;
                med.Text = string.Empty;
                diff.Text = string.Empty;
                hards.Text = string.Empty;
                must.Text = string.Empty;
                string isdesc = string.Empty;
                if (chkobj.Checked == true)
                {
                    if (chjkdes.Checked == true)
                    {
                        isdesc = "0','1";
                    }
                    else
                    {
                        isdesc = "0";
                    }
                    ddlQuestionType.Enabled = true;
                    questionType = ddlQuestionType.SelectedValue.Trim();
                    switch (questionType)
                    {
                        case "1":
                        default:
                            ddlQuestionSubType.Visible = true;
                            ddlQuestionSubType.Enabled = true;
                            questionSubType = Convert.ToString(ddlQuestionSubType.SelectedValue).Trim();
                            switch (questionSubType)
                            {
                                case "1":
                                default:
                                    break;
                                case "2":
                                    break;
                            }
                            break;
                        case "2":
                            break;
                        case "3":
                            ddlQuestionMatchType.Enabled = true;
                            ddlQuestionMatchType.Visible = true;
                            questionSubType = Convert.ToString(ddlQuestionMatchType.SelectedValue).Trim();
                            break;
                        case "4":
                            break;
                        case "5":
                            break;
                        case "6":
                            break;
                    }
                    if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                    {
                        qryQuestionType = " and QuestionType='" + questionType + "' and QuestionSubType='" + questionSubType + "'";
                    }
                    else if (!string.IsNullOrEmpty(questionType.Trim()) && questionType.Trim() != "0")
                    {
                        qryQuestionType = " and QuestionType='" + questionType + "'";
                    }
                }
                else if (chjkdes.Checked == true)
                {

                    isdesc = "1";
                }
                else
                {
                    isdesc = "0','1";
                    ddlQuestionType.Enabled = true;
                    questionType = ddlQuestionType.SelectedValue.Trim();
                    switch (questionType)
                    {
                        case "1":
                        default:
                            ddlQuestionSubType.Visible = true;
                            ddlQuestionSubType.Enabled = true;
                            questionSubType = Convert.ToString(ddlQuestionSubType.SelectedValue).Trim();
                            switch (questionSubType)
                            {
                                case "1":
                                default:
                                    break;
                                case "2":
                                    break;
                            }
                            break;
                        case "2":
                            break;
                        case "3":
                            ddlQuestionMatchType.Enabled = true;
                            ddlQuestionMatchType.Visible = true;
                            questionSubType = Convert.ToString(ddlQuestionMatchType.SelectedValue).Trim();
                            break;
                        case "4":
                            break;
                        case "5":
                            break;
                        case "6":
                            break;
                    }
                    if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                    {
                        qryQuestionType = " and QuestionType='" + questionType + "' and QuestionSubType='" + questionSubType + "'";
                    }
                    else if (!string.IsNullOrEmpty(questionType.Trim()) && questionType.Trim() != "0")
                    {
                        qryQuestionType = " and QuestionType='" + questionType + "'";
                    }
                }
                string Existqtns = string.Empty;
                if (cb_notexist.Checked == true)
                {
                    Existqtns = " and isnull(Already_exist,'0') <>'1'";
                }

                if (treeTopic.Nodes.Count == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "Please Add Syllubus To The Subject";
                    grd_dynamic.Visible = false;
                    return;
                }

                string SubjectTopicNo = string.Empty;
                string qryTopicNo = string.Empty;
                int selTopic = 0;
                ArrayList arrSelTopicParent = new ArrayList();

                Dictionary<string, int> dicTopicParent = new Dictionary<string, int>();
                selTopic = 0;
                arrSelTopicParent.Clear();
                dicTopicParent.Clear();
                for (int a = 0; a < treeTopic.CheckedNodes.Count; a++)
                {
                    selTopic++;
                    if (SubjectTopicNo == "")
                    {
                        SubjectTopicNo = "'" + treeTopic.CheckedNodes[a].Value + "'";
                        //SubjectTopicNo = treeTopic.CheckedNodes[a].Text;
                    }
                    else
                    {
                        SubjectTopicNo = SubjectTopicNo + ",'" + treeTopic.CheckedNodes[a].Value + "'";
                        //topicname = topicname + "," + treeTopic.CheckedNodes[a].Text;
                    }
                    if (treeTopic.CheckedNodes[a].Parent != null)
                    {
                        if (!arrSelTopicParent.Contains(treeTopic.CheckedNodes[a].Parent.Value))
                        {
                            arrSelTopicParent.Add(treeTopic.CheckedNodes[a].Parent.Value);
                            if (SubjectTopicNo == "")
                            {
                                SubjectTopicNo = "'" + treeTopic.CheckedNodes[a].Parent.Value + "'";
                                //SubjectTopicNo = treeTopic.CheckedNodes[a].Text;
                            }
                            else
                            {
                                SubjectTopicNo = SubjectTopicNo + ",'" + treeTopic.CheckedNodes[a].Parent.Value + "'";
                                //topicname = topicname + "," + treeTopic.CheckedNodes[a].Text;
                            }
                        }
                        else
                        {

                        }
                    }
                }

                if (selTopic > 0)
                {
                    qryTopicNo = " and syllabus in(" + SubjectTopicNo + ")";
                }

                string marks = mrk.Text;
                string totnoqustion = totalno.Text;
                if (marks != "" && totnoqustion != "")
                {
                    int avl = Convert.ToInt32(availabl.Text) - Convert.ToInt32(totnoqustion);

                    string keyval = marks + "-" + isdesc + ((!string.IsNullOrEmpty(questionType.Trim())) ? "-" + questionType.Trim() : "");
                    if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                    {
                        keyval += "-" + questionSubType.Trim();
                    }

                    if (!existobject.ContainsKey(keyval))
                    {
                        existobject.Add(keyval, avl);
                    }
                    else
                    {
                        existobject[keyval] = avl;
                    }
                }

                string compare = d2.GetFunction("select  count(QuestionMasterPK) as total_question from tbl_question_master where subject_no='" + subno + "' and mark ='" + mrk.Text + "' and is_descriptive in('" + isdesc + "') " + Existqtns + qryTopicNo + qryQuestionType + " ");

                if (compare != "0")
                {

                    availabl.Text = compare;

                    int available = Convert.ToInt32(compare);
                    string keyval = marks + "-" + isdesc + ((!string.IsNullOrEmpty(questionType.Trim())) ? "-" + questionType.Trim() : ""); ;
                    if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                    {
                        keyval += "-" + questionSubType.Trim();
                    }

                    if (existobject.ContainsKey(keyval))
                    {
                        int getmark = Convert.ToInt32(existobject[keyval]);
                        if (available == getmark)
                        {
                            available = getmark;
                        }
                        else
                        {
                            existobject[keyval] = available;
                        }
                    }
                    availabl.Text = Convert.ToString(available);
                }
                else
                {
                    //Label availabl = (Label)grd_dynamic.Rows[i].FindControl("lbl_availabl");
                    availabl.Text = string.Empty;
                }
                string type = "  select count(*) no_of_question,type from tbl_question_master where subject_no='" + subno + "' and is_descriptive in ('" + isdesc + "')  " + Existqtns + qryTopicNo + qryQuestionType + "and  mark ='" + mrk.Text + "' group by type";
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(type, "text");
                if (ds1.Tables.Count > 0)
                {
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        for (int qstion = 0; qstion < ds1.Tables[0].Rows.Count; qstion++)
                        {
                            string typ = Convert.ToString(ds1.Tables[0].Rows[qstion]["type"]);
                            string noofqstion = Convert.ToString(ds1.Tables[0].Rows[qstion]["no_of_question"]);
                            if (typ != "")
                            {
                                if (typ == "0")
                                {
                                    easys.Text = noofqstion;
                                }
                                else if (typ == "1")
                                {
                                    med.Text = noofqstion;
                                }
                                else if (typ == "2")
                                {
                                    diff.Text = noofqstion;
                                }
                                else if (typ == "3")
                                {
                                    hards.Text = noofqstion;
                                }
                            }
                        }
                        if (easys.Text == "")
                        {
                            easys.Text = "0";
                        }
                        if (med.Text == "")
                        {
                            med.Text = "0";
                        }
                        if (diff.Text == "")
                        {
                            diff.Text = "0";
                        }
                        if (hards.Text == "")
                        {
                            hards.Text = "0";
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    protected void chk_answer_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chk_answer.Checked == true)
            {
                FpSpread1.Columns[5].Visible = true;
            }
            else
            {
                FpSpread1.Columns[5].Visible = false;
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    protected void chk_option_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chk_option.Checked == true)
            {
                FpSpread1.Columns[4].Visible = true;
            }
            else
            {
                FpSpread1.Columns[4].Visible = false;
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    protected void ddl_testname_Selectchanged(object sender, EventArgs e)
    {
        objective_check.Visible = false;
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string subj_code = string.Empty;
            bool issucess = false;
            if (ddlsubject.Items.Count > 0)
            {
                subj_code = Convert.ToString(ddlsubject.SelectedItem.Value);
            }
            else
            {
                lbl_alert1.Text = "No Subject Were Found";
                imgdiv2.Visible = true;
                return;
            }

            //  string testname_code = ddl_testname.SelectedItem.Value;
            string sylcod = string.Empty;
            if (subj_code.Trim() == "")
            {
                lbl_alert1.Text = "Please Select Subject";
                imgdiv2.Visible = true;
                return;
            }

            if (rb_internel.Checked == true)
            {
                string testname_code = string.Empty;
                if (ddl_testname.Items.Count == 0)
                {
                }
                else
                {
                    testname_code = Convert.ToString(ddl_testname.SelectedItem.Value);
                }
                sylcod = d2.GetFunctionv("select * from Exist_questions where is_internal='2' and Test_code='" + testname_code + "'  and subject_no='" + subj_code + "' ").Trim();
                if (sylcod.Trim() != "0" && sylcod.Trim() != "")
                {
                    string deletequry = "delete from Exist_questions where is_internal='2' and  Test_code='" + testname_code + "' and subject_no='" + subj_code + "' ";
                    int inserts = d2.update_method_wo_parameter(deletequry, "Text");
                }
            }
            else if (rb_external.Checked == true)
            {
                string month = ddl_month.SelectedItem.Value;
                string year = ddl_year.SelectedItem.Value;

                sylcod = d2.GetFunction("select*from Exist_questions where is_internal='1' and Exam_month='" + month + "' and Exam_year='" + year + "'  and subject_no='" + subj_code + "' ");
                if (sylcod != "0" && sylcod.Trim() != "")
                {
                    string deletequry = "delete from Exist_questions where is_internal='1' and Exam_month='" + month + "' and Exam_year='" + year + "'  and subject_no='" + subj_code + "'";
                    int inserts = d2.update_method_wo_parameter(deletequry, "Text");

                }
            }

            if (FpSpread1.Sheets[0].RowCount > 0)
            {
                for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    string questionpk = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Tag);
                    string section = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                    string section_name = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Note);
                    string syllcod = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);
                    string must_attend = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Tag);
                    if (must_attend == "" && must_attend == "0")
                    {
                        lbl_alert1.Text = "Must Attend not available";
                        imgdiv2.Visible = true;
                        return;
                    }

                    string isinternls_creat = string.Empty;
                    string isinternls_value = string.Empty;

                    if (rb_internel.Checked == true)
                    {
                        string testname_code = ddl_testname.SelectedItem.Value;
                        isinternls_creat = " is_internal, Test_code, ";
                        isinternls_value = " '2', '" + testname_code + "', ";
                    }
                    else if (rb_external.Checked == true)
                    {
                        string month = ddl_month.SelectedItem.Value;
                        string year = ddl_year.SelectedItem.Value;

                        isinternls_creat = " is_internal, Exam_month, Exam_year, ";
                        isinternls_value = " '1', '" + month + "', '" + year + "', ";

                    }

                    string insertqry = " insert into Exist_questions ( syllabus, " + isinternls_creat + " Section,QuestionMasterFK,subject_no,Must_attend,section_name,QNo) values('" + syllcod + "', " + isinternls_value + " '" + section + "' ,'" + questionpk + "','" + subj_code + "','" + must_attend + "','" + section_name + "','" + (i + 1) + "')";

                    int insert = d2.update_method_wo_parameter(insertqry, "Text");
                    if (insert != 0)
                    {
                        issucess = true;
                        string upd = "update tbl_question_master set Already_exist='1' where QuestionMasterPK='" + questionpk + "' ";
                        int pkupdate = d2.update_method_wo_parameter(upd, "Text");
                        divPopQuesprepar.Visible = false;
                    }
                }
                if (issucess)
                {
                    addMin_MaxMark();
                    lbl_alert1.Visible = true;
                    lbl_alert1.Text = "Saved Successfully";
                    imgdiv2.Visible = true;
                }
                else
                {
                    lbl_alert1.Visible = true;
                    lbl_alert1.Text = "Not Saved";
                    imgdiv2.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }


    }

    protected void ddl_month_SelectedIndexChanged(object sender, EventArgs e)
    {
        objective_check.Visible = false;
    }

    protected void ddl_year_SelectedIndexChanged(object sender, EventArgs e)
    {
        objective_check.Visible = false;
    }

    public void rb_internel_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lbl_testname.Visible = true;
            ddl_testname.Visible = true;
            lbl_month.Visible = false;
            ddl_month.Visible = false;
            lbl_year.Visible = false;
            ddl_year.Visible = false;
            objective_check.Visible = false;
        }
        catch
        {
        }
    }

    public void rb_external_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lbl_testname.Visible = false;
            ddl_testname.Visible = false;
            lbl_month.Visible = true;
            ddl_month.Visible = true;
            lbl_year.Visible = true;
            ddl_year.Visible = true;
            objective_check.Visible = false;
        }
        catch
        {
        }
    }

    public void addMin_MaxMark()
    {
        int totalmark = 0;

        for (int grd = 0; grd < grd_dynamic.Rows.Count; grd++)
        {
            TextBox mark = (TextBox)grd_dynamic.Rows[grd].FindControl("txt_mark");
            TextBox noqustion = (TextBox)grd_dynamic.Rows[grd].FindControl("txt_noqustion");
            TextBox must = (TextBox)grd_dynamic.Rows[grd].FindControl("txt_mustattnd");

            if (must.Text != "" && mark.Text != "")
            {
                int mulmark = Convert.ToInt32(must.Text) * Convert.ToInt32(mark.Text);
                totalmark = totalmark + mulmark;
            }
        }
        string subject = ddlsubject.SelectedItem.Value;

        if (rb_internel.Checked == true)
        {
            string testcod = ddl_testname.SelectedItem.Value;
            string selectqry = "select max_mark,min_mark from CriteriaForInternal where Criteria_no='" + testcod + "'";
            ds = d2.select_method_wo_parameter(selectqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string maxmark = Convert.ToString(ds.Tables[0].Rows[0]["max_mark"]);
                string min = Convert.ToString(ds.Tables[0].Rows[0]["min_mark"]);
                string convertMark = Convert.ToString(totalmark);
                ToConvertedMark(convertMark, ref maxmark, ref min);
                if (maxmark != "0")
                {
                    string insertqry = " update  tbl_question_bank_master set max_mark='" + maxmark + "',min_mark='" + min + "' where Exam='" + testcod + "' and Subject_no='" + subject + "' and  exam_type='2' ";
                    int insert = d2.update_method_wo_parameter(insertqry, "Text");
                }
            }
        }
    }

    public void ToConvertedMark(string txtConvertTo, ref string maxMark, ref string minMark)
    {
        int max;
        bool maxflag = int.TryParse(txtConvertTo, out max);
        double minmultyply;
        double min = 0;
        double max_minCal = 0;
        bool maxbool = double.TryParse(maxMark, out max_minCal);
        bool minbool = double.TryParse(minMark, out min);
        if (maxflag)
        {
            switch (txtConvertTo)
            {
                default:
                    if (maxbool == true && minbool == true && min > 0)
                    {
                        minmultyply = max_minCal / min;
                        min = int.Parse(txtConvertTo) / minmultyply;
                    }
                    break;
            }
            minMark = min.ToString();
            maxMark = txtConvertTo;
        }
    }

    public void ddlQuestionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            checkval();
        }
        catch
        {
        }
    }

    public void ddlQuestionSubType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            checkval();
        }
        catch
        {
        }
    }

    public void ddlQuestionMatchType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            checkval();
        }
        catch
        {
        }
    }

    public void txt_mark_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrors.Text = string.Empty;
            string mark = string.Empty;
            int rowindex = rowIndxClicked();
            for (int i = 0; i < grd_dynamic.Rows.Count; i++)
            {
                if (rowindex == i)
                {
                    TextBox mrk = (TextBox)grd_dynamic.Rows[i].FindControl("txt_mark");
                    string subno = Convert.ToString(ddlsubject.SelectedItem.Value);
                    TextBox notqstn = (TextBox)grd_dynamic.Rows[i].FindControl("txt_noqustion");
                    RadioButton objec = (RadioButton)grd_dynamic.Rows[i].FindControl("rb_objct");
                    RadioButton descrp = (RadioButton)grd_dynamic.Rows[i].FindControl("rb_descrip");
                    Label avail = (Label)grd_dynamic.Rows[i].FindControl("lbl_availabl");
                    Label easys = (Label)grd_dynamic.Rows[i].FindControl("lbl_easy");
                    Label med = (Label)grd_dynamic.Rows[i].FindControl("lbl_mediam");
                    Label diff = (Label)grd_dynamic.Rows[i].FindControl("lbl_diffc");
                    Label hards = (Label)grd_dynamic.Rows[i].FindControl("lbl_hard");
                    Label availabl = (Label)grd_dynamic.Rows[i].FindControl("lbl_availabl");
                    TextBox esy = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Easy");
                    TextBox medm = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Medium");
                    TextBox diffict = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Difficult");
                    TextBox hard = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Hard");

                    DropDownList ddlQuestionType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionType");
                    DropDownList ddlQuestionSubType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionSubType");
                    DropDownList ddlQuestionMatchType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionMatchType");
                    ddlQuestionType.Enabled = false;
                    ddlQuestionSubType.Visible = false;
                    ddlQuestionSubType.Enabled = false;
                    ddlQuestionMatchType.Enabled = false;
                    ddlQuestionMatchType.Visible = false;

                    string questionType = string.Empty;
                    string questionSubType = string.Empty;
                    string qryQuestionType = string.Empty;


                    if (treeTopic.Nodes.Count == 0)
                    {
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "Please Add Syllubus To The Subject";
                        grd_dynamic.Visible = false;
                        return;
                    }

                    string SubjectTopicNo = string.Empty;
                    string qryTopicNo = string.Empty;
                    int selTopic = 0;
                    ArrayList arrSelTopicParent = new ArrayList();

                    Dictionary<string, int> dicTopicParent = new Dictionary<string, int>();
                    selTopic = 0;
                    arrSelTopicParent.Clear();
                    dicTopicParent.Clear();
                    for (int a = 0; a < treeTopic.CheckedNodes.Count; a++)
                    {
                        selTopic++;

                        if (SubjectTopicNo == "")
                        {
                            SubjectTopicNo = "'" + treeTopic.CheckedNodes[a].Value + "'";
                            //SubjectTopicNo = treeTopic.CheckedNodes[a].Text;
                        }
                        else
                        {
                            SubjectTopicNo = SubjectTopicNo + ",'" + treeTopic.CheckedNodes[a].Value + "'";
                            //topicname = topicname + "," + treeTopic.CheckedNodes[a].Text;
                        }
                        if (treeTopic.CheckedNodes[a].Parent != null)
                        {
                            if (!arrSelTopicParent.Contains(treeTopic.CheckedNodes[a].Parent.Value))
                            {
                                arrSelTopicParent.Add(treeTopic.CheckedNodes[a].Parent.Value);
                                if (SubjectTopicNo == "")
                                {
                                    SubjectTopicNo = "'" + treeTopic.CheckedNodes[a].Parent.Value + "'";
                                    //SubjectTopicNo = treeTopic.CheckedNodes[a].Text;
                                }
                                else
                                {
                                    SubjectTopicNo = SubjectTopicNo + ",'" + treeTopic.CheckedNodes[a].Parent.Value + "'";
                                    //topicname = topicname + "," + treeTopic.CheckedNodes[a].Text;
                                }
                            }
                            else
                            {

                            }
                        }
                    }


                    if (selTopic > 0)
                    {
                        qryTopicNo = " and syllabus in(" + SubjectTopicNo + ")";
                    }

                    string is_desc = string.Empty;
                    if (objec.Checked == true)
                    {
                        is_desc = "0";
                        ddlQuestionType.Enabled = true;
                        questionType = ddlQuestionType.SelectedValue.Trim();
                        switch (questionType)
                        {
                            case "1":
                            default:
                                ddlQuestionSubType.Visible = true;
                                ddlQuestionSubType.Enabled = true;
                                questionSubType = Convert.ToString(ddlQuestionSubType.SelectedValue).Trim();
                                switch (questionSubType)
                                {
                                    case "1":
                                    default:
                                        break;
                                    case "2":
                                        break;
                                }
                                break;
                            case "2":
                                break;
                            case "3":
                                ddlQuestionMatchType.Enabled = true;
                                ddlQuestionMatchType.Visible = true;
                                questionSubType = Convert.ToString(ddlQuestionMatchType.SelectedValue).Trim();
                                break;
                            case "4":
                                break;
                            case "5":
                                break;
                            case "6":
                                break;
                        }
                        if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                        {
                            qryQuestionType = " and QuestionType='" + questionType + "' and QuestionSubType='" + questionSubType + "'";
                        }
                        else if (!string.IsNullOrEmpty(questionType.Trim()) && questionType.Trim() != "0")
                        {
                            qryQuestionType = " and QuestionType='" + questionType + "'";
                        }

                    }
                    else if (descrp.Checked == true)
                    {
                        is_desc = "1";
                    }
                    string Existqtns = string.Empty;
                    if (cb_notexist.Checked == true)
                    {
                        Existqtns = " and isnull(Already_exist,'0') <>'1'";
                    }

                    string compare = d2.GetFunction("select count(QuestionMasterPK) as total_question from tbl_question_master where subject_no='" + subno + "' and is_descriptive='" + is_desc + "' and mark in('" + mrk.Text + "') " + Existqtns + qryQuestionType + qryTopicNo + " ");

                    if (mrk.Text != "")
                    {
                        if (compare != "0")
                        {
                            availabl.Text = compare;
                            checkval();
                        }
                        else
                        {
                            availabl.Text = string.Empty;
                            notqstn.Text = string.Empty;
                            easys.Text = string.Empty;
                            med.Text = string.Empty;
                            diff.Text = string.Empty;
                            hards.Text = string.Empty;
                        }
                    }
                    else
                    {
                        availabl.Text = string.Empty;
                        notqstn.Text = string.Empty;
                        easys.Text = string.Empty;
                        med.Text = string.Empty;
                        diff.Text = string.Empty;
                        hards.Text = string.Empty;
                        esy.Text = string.Empty;
                        medm.Text = string.Empty;
                        diffict.Text = string.Empty;
                        hard.Text = string.Empty;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    public void Txt_noqution_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string is_desc = string.Empty;
            string mark = string.Empty;
            int rowindex = rowIndxClicked();
            lblerrors.Text = string.Empty;
            string after_samevlue_del = string.Empty;

            if (treeTopic.Nodes.Count == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "Please Add Syllubus To The Subject";
                grd_dynamic.Visible = false;
                return;
            }

            string SubjectTopicNo = string.Empty;
            string qryTopicNo = string.Empty;
            int selTopic = 0;
            ArrayList arrSelTopicParent = new ArrayList();

            Dictionary<string, int> dicTopicParent = new Dictionary<string, int>();
            selTopic = 0;
            arrSelTopicParent.Clear();
            dicTopicParent.Clear();
            for (int a = 0; a < treeTopic.CheckedNodes.Count; a++)
            {
                selTopic++;
                if (SubjectTopicNo == "")
                {
                    SubjectTopicNo = "'" + treeTopic.CheckedNodes[a].Value + "'";
                    //SubjectTopicNo = treeTopic.CheckedNodes[a].Text;
                }
                else
                {
                    SubjectTopicNo = SubjectTopicNo + ",'" + treeTopic.CheckedNodes[a].Value + "'";
                    //topicname = topicname + "," + treeTopic.CheckedNodes[a].Text;
                }
                if (treeTopic.CheckedNodes[a].Parent != null)
                {
                    if (!arrSelTopicParent.Contains(treeTopic.CheckedNodes[a].Parent.Value))
                    {
                        arrSelTopicParent.Add(treeTopic.CheckedNodes[a].Parent.Value);
                        if (SubjectTopicNo == "")
                        {
                            SubjectTopicNo = "'" + treeTopic.CheckedNodes[a].Parent.Value + "'";
                            //SubjectTopicNo = treeTopic.CheckedNodes[a].Text;
                        }
                        else
                        {
                            SubjectTopicNo = SubjectTopicNo + ",'" + treeTopic.CheckedNodes[a].Parent.Value + "'";
                            //topicname = topicname + "," + treeTopic.CheckedNodes[a].Text;
                        }
                    }
                    else
                    {

                    }
                }
            }


            if (selTopic > 0)
            {
                qryTopicNo = " and syllabus in(" + SubjectTopicNo + ")";
            }

            for (int i = 0; i < grd_dynamic.Rows.Count; i++)
            {
                TextBox mrk = (TextBox)grd_dynamic.Rows[i].FindControl("txt_mark");
                TextBox totqtn = (TextBox)grd_dynamic.Rows[i].FindControl("txt_noqustion");
                TextBox esy = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Easy");
                TextBox medm = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Medium");
                TextBox diffict = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Difficult");
                TextBox hard = (TextBox)grd_dynamic.Rows[i].FindControl("txt_Hard");
                Label avail = (Label)grd_dynamic.Rows[i].FindControl("lbl_availabl");
                Label easys = (Label)grd_dynamic.Rows[i].FindControl("lbl_easy");
                Label med = (Label)grd_dynamic.Rows[i].FindControl("lbl_mediam");
                Label diff = (Label)grd_dynamic.Rows[i].FindControl("lbl_diffc");
                Label hards = (Label)grd_dynamic.Rows[i].FindControl("lbl_hard");
                string subno = Convert.ToString(ddlsubject.SelectedItem.Value);
                RadioButton objec = (RadioButton)grd_dynamic.Rows[i].FindControl("rb_objct");
                RadioButton descrp = (RadioButton)grd_dynamic.Rows[i].FindControl("rb_descrip");
                Label availabl = (Label)grd_dynamic.Rows[i].FindControl("lbl_availabl");
                TextBox must = (TextBox)grd_dynamic.Rows[i].FindControl("txt_mustattnd");

                DropDownList ddlQuestionType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionType");
                DropDownList ddlQuestionSubType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionSubType");
                DropDownList ddlQuestionMatchType = (DropDownList)grd_dynamic.Rows[i].FindControl("ddlQuestionMatchType");
                ddlQuestionType.Enabled = false;
                ddlQuestionSubType.Visible = false;
                ddlQuestionSubType.Enabled = false;
                ddlQuestionMatchType.Enabled = false;
                ddlQuestionMatchType.Visible = false;

                string questionType = string.Empty;
                string questionSubType = string.Empty;
                string qryQuestionType = string.Empty;

                is_desc = string.Empty;
                if (descrp.Checked == true)
                {
                    is_desc = "1";
                }
                else if (objec.Checked == true)
                {
                    is_desc = "0";

                    ddlQuestionType.Enabled = true;
                    questionType = ddlQuestionType.SelectedValue.Trim();
                    switch (questionType)
                    {
                        case "1":
                        default:
                            ddlQuestionSubType.Visible = true;
                            ddlQuestionSubType.Enabled = true;
                            questionSubType = Convert.ToString(ddlQuestionSubType.SelectedValue).Trim();
                            switch (questionSubType)
                            {
                                case "1":
                                default:
                                    break;
                                case "2":
                                    break;
                            }
                            break;
                        case "2":
                            break;
                        case "3":
                            ddlQuestionMatchType.Enabled = true;
                            ddlQuestionMatchType.Visible = true;
                            questionSubType = Convert.ToString(ddlQuestionMatchType.SelectedValue).Trim();
                            break;
                        case "4":
                            break;
                        case "5":
                            break;
                        case "6":
                            break;
                    }
                    if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                    {
                        qryQuestionType = " and QuestionType='" + questionType + "' and QuestionSubType='" + questionSubType + "'";
                    }
                    else if (!string.IsNullOrEmpty(questionType.Trim()) && questionType.Trim() != "0")
                    {
                        qryQuestionType = " and QuestionType='" + questionType + "'";
                    }
                }

                if (after_samevlue_del != "")
                {
                    string kkeyvalue = mrk.Text + is_desc + ((!string.IsNullOrEmpty(questionType.Trim())) ? "-" + questionType.Trim() : "");
                    if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                    {
                        kkeyvalue += "-" + questionSubType.Trim();
                    }
                    if (after_samevlue_del == kkeyvalue)
                    {
                        totqtn.Text = string.Empty;
                        esy.Text = string.Empty;
                        medm.Text = string.Empty;
                        medm.Text = string.Empty;
                        easys.Text = string.Empty;
                        med.Text = string.Empty;
                        diff.Text = string.Empty;
                        hards.Text = string.Empty;
                        availabl.Text = string.Empty;
                        mrk.Text = string.Empty;
                        objec.Checked = true;
                        descrp.Checked = false;
                    }
                }

                if (rowindex == i)
                {
                    mark = mrk.Text;
                    must.Text = string.Empty;
                    string avlablqution = avail.Text;
                    string totalno = totqtn.Text;
                    if (avail.Text != "" && totqtn.Text != "")
                    {
                        after_samevlue_del = mrk.Text + is_desc + ((!string.IsNullOrEmpty(questionType.Trim())) ? "-" + questionType.Trim() : "");
                        if (!string.IsNullOrEmpty(questionSubType.Trim()) && questionSubType.Trim() != "0")
                        {
                            after_samevlue_del += "-" + questionSubType.Trim();
                        }
                    }
                    string Existqtns = string.Empty;
                    if (cb_notexist.Checked == true)
                    {
                        Existqtns = " and isnull(Already_exist,'0') <>'1'";
                    }

                    if (totalno != "" && avlablqution != "")
                    {
                        string type = "  select count(*) no_of_question,type from tbl_question_master where subject_no='" + subno + "' and is_descriptive in ('" + is_desc + "')  " + Existqtns + " and  mark ='" + mark + "' " + qryQuestionType + qryTopicNo + " group by type";
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(type, "text");
                        if (ds1.Tables.Count > 0)
                        {
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                for (int qstion = 0; qstion < ds1.Tables[0].Rows.Count; qstion++)
                                {
                                    string typ = Convert.ToString(ds1.Tables[0].Rows[qstion]["type"]);
                                    string noofqstion = Convert.ToString(ds1.Tables[0].Rows[qstion]["no_of_question"]);
                                    if (typ != "")
                                    {
                                        if (typ == "0")
                                        {
                                            easys.Text = noofqstion;
                                        }
                                        else if (typ == "1")
                                        {
                                            med.Text = noofqstion;
                                        }
                                        else if (typ == "2")
                                        {
                                            diff.Text = noofqstion;
                                        }
                                        else if (typ == "3")
                                        {
                                            hards.Text = noofqstion;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        easys.Text = string.Empty;
                        med.Text = string.Empty;
                        diff.Text = string.Empty;
                        hards.Text = string.Empty;
                    }

                    if (totalno != "" && avlablqution != "")
                    {
                        if (Convert.ToInt32(totalno) > Convert.ToInt32(avlablqution))
                        {
                            lblerrors.Visible = true;
                            lblerrors.Text = "Please Enter No of Question count less than Available Question";
                            totqtn.Text = string.Empty;
                            return;
                        }
                    }
                    else if (avlablqution == "")
                    {
                        lblerrors.Visible = true;
                        lblerrors.Text = " No Questions";
                    }

                    if (mrk.Text == "")
                    {
                        lblerrors.Visible = true;
                        lblerrors.Text = "Please Enter Mark";
                        totqtn.Text = string.Empty;
                        return;
                    }
                    hard.Text = string.Empty;
                    medm.Text = string.Empty;
                    diffict.Text = string.Empty;
                    esy.Text = string.Empty;

                    string alredyexist = string.Empty;
                    if (cb_notexist.Checked == true)
                    {
                        alredyexist = " and Already_exist='1' ";
                    }

                    if (totalno != "")
                    {
                        string compare = d2.GetFunction("select  count(QuestionMasterPK) as total_question    from tbl_question_master where subject_no='" + subno + "' and mark in('" + mrk.Text + "') " + alredyexist + " " + Existqtns + qryQuestionType + qryTopicNo + "  ");

                        if (mark == "" || avail.Text == "")
                        {
                            totqtn.Text = string.Empty;
                            hard.Text = string.Empty;
                            medm.Text = string.Empty;
                            diffict.Text = string.Empty;
                            esy.Text = string.Empty;
                        }
                    }
                    else
                    {
                        hard.Text = string.Empty;
                        medm.Text = string.Empty;
                        diffict.Text = string.Empty;
                        esy.Text = string.Empty;
                    }

                    if (easys.Text == "")
                    {
                        easys.Text = "0";
                    }
                    if (med.Text == "")
                    {
                        med.Text = "0";
                    }
                    if (diff.Text == "")
                    {
                        diff.Text = "0";
                    }
                    if (hards.Text == "")
                    {
                        hards.Text = "0";
                    }
                }
            }
        }
        catch
        {
        }


    }

    protected void txt_mustattnd_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            int rowindex = rowIndxClicked();
            lblerrors.Text = string.Empty;
            string after_samevlue_del = string.Empty;
            for (int i = 0; i < grd_dynamic.Rows.Count; i++)
            {
                if (rowindex == i)
                {
                    TextBox must = (TextBox)grd_dynamic.Rows[i].FindControl("txt_mustattnd");
                    TextBox totqtn = (TextBox)grd_dynamic.Rows[i].FindControl("txt_noqustion");

                    int total_question = Convert.ToInt32(totqtn.Text);
                    int must_auttand = Convert.ToInt32(must.Text);
                    if (total_question < must_auttand)
                    {
                        lblerrors.Text = "Please Enter Less than Total Question count";
                        must.Text = string.Empty;
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void treeTopic_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkTopic_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkTopic.Checked == true)
            {
                for (int remv = 0; remv < treeTopic.Nodes.Count; remv++)
                {
                    treeTopic.Nodes[remv].Checked = true;
                    txtTopic.Text = "Header(" + (treeTopic.Nodes.Count) + ")";
                    if (treeTopic.Nodes[remv].ChildNodes.Count > 0)
                    {
                        for (int child = 0; child < treeTopic.Nodes[remv].ChildNodes.Count; child++)
                        {
                            treeTopic.Nodes[remv].ChildNodes[child].Checked = true;
                        }
                    }
                }
            }
            else
            {
                for (int remv = 0; remv < treeTopic.Nodes.Count; remv++)
                {
                    treeTopic.Nodes[remv].Checked = false;
                    txtTopic.Text = "---Select---";
                    if (treeTopic.Nodes[remv].ChildNodes.Count > 0)
                    {
                        for (int child = 0; child < treeTopic.Nodes[remv].ChildNodes.Count; child++)
                        {
                            treeTopic.Nodes[remv].ChildNodes[child].Checked = false;
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
        }
    }

    protected void treeTopic_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        try
        {
            objective_check.Visible = false;
            for (int iCnt = 0; iCnt < treeTopic.Nodes.Count; iCnt++)
            {
                if (treeTopic.Nodes[iCnt].Checked == true)
                {
                    if (treeTopic.Nodes[iCnt].ChildNodes.Count > 0)
                    {
                        for (int jCnt = 0; jCnt < treeTopic.Nodes[iCnt].ChildNodes.Count; jCnt++)
                        {
                            treeTopic.Nodes[iCnt].ChildNodes[jCnt].Checked = true;
                            for (int kcnt = 0; kcnt < treeTopic.Nodes[iCnt].ChildNodes[jCnt].ChildNodes.Count; kcnt++)
                            {
                                treeTopic.Nodes[iCnt].ChildNodes[jCnt].ChildNodes[kcnt].Checked = true;
                            }
                        }
                    }
                    else
                    {
                        for (int jCnt = 0; jCnt < treeTopic.Nodes[iCnt].ChildNodes.Count; jCnt++)
                        {
                            treeTopic.Nodes[iCnt].ChildNodes[jCnt].Checked = false;
                            for (int kcnt = 0; kcnt < treeTopic.Nodes[iCnt].ChildNodes[jCnt].ChildNodes.Count; kcnt++)
                            {
                                treeTopic.Nodes[iCnt].ChildNodes[jCnt].ChildNodes[kcnt].Checked = false;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void CheckTreeNodeRecursive(TreeNode parent, bool fCheck)
    {
        foreach (TreeNode child in parent.ChildNodes)
        {
            if (child.Checked != fCheck)
            {
                child.Checked = fCheck;
            }
            if (child.ChildNodes.Count > 0)
            {
                CheckTreeNodeRecursive(child, fCheck);
            }
        }
    }

    private void PopulateTreeview()
    {
        string dt_topics = string.Empty;
        string dt_topics1 = string.Empty;

        DataSet dstopic = new DataSet();

        treeTopic.Nodes.Clear();
        HierarchyTrees hierarchyTrees = new HierarchyTrees();
        HierarchyTrees.HTree objHTree = null;

        if (ddlsubject.Items.Count > 0)
        {
            //start=======common tree load
            string query = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + Convert.ToString(ddlsubject.SelectedValue).Trim() + "'  order by parent_code";
            dstopic = d2.select_method_wo_parameter(query, "Text");
            for (int i = 0; i < dstopic.Tables[0].Rows.Count; i++)
            {
                objHTree = new HierarchyTrees.HTree();
                objHTree.topic_no = int.Parse(dstopic.Tables[0].Rows[i]["Topic_no"].ToString());
                objHTree.parent_code = int.Parse(dstopic.Tables[0].Rows[i]["parent_code"].ToString());
                objHTree.unit_name = dstopic.Tables[0].Rows[i]["unit_name"].ToString();
                hierarchyTrees.Add(objHTree);
            }
            //end==========
        }
        foreach (HierarchyTrees.HTree hTree in hierarchyTrees)
        {
            HierarchyTrees.HTree parentNode = hierarchyTrees.Find(delegate(HierarchyTrees.HTree emp) { return emp.topic_no == hTree.parent_code; });
            if (parentNode != null)
            {
                foreach (TreeNode tn in treeTopic.Nodes)
                {
                    if (tn.Value == parentNode.topic_no.ToString())
                    {
                        tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
                        //Session["session_tnValue"] = tn;
                    }
                    if (tn.ChildNodes.Count > 0)
                    {
                        foreach (TreeNode ctn in tn.ChildNodes)
                        {
                            RecursiveChild(ctn, parentNode.topic_no.ToString(), hTree);
                        }
                    }
                }
            }
            else
            {
                treeTopic.Nodes.Add(new TreeNode(hTree.unit_name, hTree.topic_no.ToString()));
            }
            treeTopic.ExpandAll();
        }
        //if (treeTopic.Nodes.Count < 1)
        //{

        //    BtnSaveTree.Enabled = false;
        //}
        //else
        //{
        //    BtnSaveTree.Enabled = true;
        //}
    }

    public void RecursiveChild(TreeNode tn, string searchValue, HierarchyTrees.HTree hTree)
    {
        if (tn.Value == searchValue)
        {
            tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
        }
        if (tn.ChildNodes.Count > 0)
        {
            foreach (TreeNode ctn in tn.ChildNodes)
            {
                RecursiveChild(ctn, searchValue, hTree);
            }
        }
    }

    public class HierarchyTrees : List<HierarchyTrees.HTree>
    {
        public class HTree
        {
            private int m_topic_no;
            private int m_parent_code;
            private string m_unit_name;

            public int topic_no
            {
                get { return m_topic_no; }
                set { m_topic_no = value; }
            }

            public int parent_code
            {
                get { return m_parent_code; }
                set { m_parent_code = value; }
            }

            public string unit_name
            {
                get { return m_unit_name; }
                set { m_unit_name = value; }
            }

        }
    }

    public void AddNodeAndChildNodesToList(TreeNode node, string values)
    {
        if (node.Value == values)
        {
            node.Selected = true;
        }
        foreach (TreeNode actualNode in node.ChildNodes)
        {
            AddNodeAndChildNodesToList(actualNode, values); // recursive call
        }
    }

}