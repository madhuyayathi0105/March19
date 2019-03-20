using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Individual_mark_entry : System.Web.UI.Page
{
    #region Fields Declaration

    bool cellclick = false;

    DAccess2 d2 = new DAccess2();

    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();

    bool isSchool = false;

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    Hashtable hat = new Hashtable();

    #endregion

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
            divMain.Attributes.Add("Style", "display:block");
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }

            string grouporusercode1 = "";
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
                FpSpread1.Visible = false;
                btn_save.Visible = false;
                bindcollege();
                BindBatch();
                BindDegree();
                bindbranch();
                bindsem();
                BindSectionDetail();
                rollno_load();
                GetSubject();
                ChangeHeaderName(isSchool);
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

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

    public void rollno_load()
    {
        try
        {
            string batch = "";
            if (ddlbatch.Items.Count > 0)
            {
                batch = Convert.ToString(ddlbatch.SelectedItem.Text);
            }
            string degreecod = "";
            if (ddlbranch.Items.Count > 0)
            {
                degreecod = Convert.ToString(ddlbranch.SelectedItem.Value);
            }
            string sem = "";
            if (ddlsem.Items.Count > 0)
            {
                sem = Convert.ToString(ddlsem.SelectedItem.Text);
            }
            string section = "";
            string strsec = "";
            if (ddlsec.Items.Count > 0)
            {
                section = Convert.ToString(ddlsec.SelectedItem.Text);

                if (ddlsec.Text.ToString() == "All" || ddlsec.Text.ToString() == "")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and Sections='" + section.ToString() + "'";
                }
            }


            ds.Clear();


            string qury = " select Roll_No,Reg_No,Stud_Name from Registration where Batch_Year='" + batch + "' and degree_code='" + degreecod + "'  and Current_Semester ='" + sem + "' " + strsec + "  and college_code='" + ddl_collegename.SelectedItem.Value + "' and  CC='0' and DelFlag='0' and Exam_Flag<>'debar'";
            ds = d2.select_method_wo_parameter(qury, "Text");

            Cbl_rollno.Items.Clear();
            Txt_rollno.Text = "---Select---";
            Cb_rollno.Checked = false;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    Cbl_rollno.DataSource = ds;
                    Cbl_rollno.DataTextField = "Roll_No";
                    Cbl_rollno.DataValueField = "Reg_No";
                    Cbl_rollno.DataBind();
                    if (Cbl_rollno.Items.Count > 0)
                    {
                        for (int row = 0; row < Cbl_rollno.Items.Count; row++)
                        {
                            Cbl_rollno.Items[row].Selected = true;
                            Cb_rollno.Checked = true;
                        }
                        Txt_rollno.Text = "Roll No(" + Cbl_rollno.Items.Count + ")";
                    }

                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void GetSubject()
    {
        try
        {
            string subjectquery = string.Empty;
            ddlsubject.Items.Clear();
            string sections = string.Empty; //ddlsec.SelectedValue.ToString();
            string strsec = "";
            if (ddlsec.Items.Count > 0)
            {
                sections = Convert.ToString(ddlsec.SelectedValue).Trim();
                if (Convert.ToString(ddlsec.SelectedValue).Trim().ToLower() == "all" || Convert.ToString(ddlsec.SelectedValue).Trim().ToLower() == "")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and st.Sections='" + sections.ToString() + "'";
                }
            }

            string sems = "";
            if (ddlsem.Items.Count > 0)
            {
                if (Convert.ToString(ddlsem.SelectedValue).Trim() != "")
                {
                    if (Convert.ToString(ddlsem.SelectedValue).Trim() == "")
                    {
                        sems = "";
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
                                testname();
                            }
                            else
                            {
                                ddlsubject.Enabled = false;
                            }
                        }
                    }
                }
            }
            testname();
            load_questions();
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
            string sem = "";
            if (ddlsem.Items.Count > 0)
            {
                sem = Convert.ToString(ddlsem.SelectedItem.Text);
            }

            string section = "";
            if (ddlsec.Items.Count > 0)
            {
                section = Convert.ToString(ddlsec.SelectedItem.Text);
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

    public void load_questions()
    {
        try
        {
            string batch = Convert.ToString(ddlbatch.SelectedItem.Text);
            string degreecod = Convert.ToString(ddlbranch.SelectedItem.Value);
            string sem = "";
            if (ddlsem.Items.Count > 0)
            {
                sem = Convert.ToString(ddlsem.SelectedItem.Text);
            }
            string section = "";
            string qrysec = "";
            if (ddlsec.Items.Count > 0)
            {
                section = Convert.ToString(ddlsec.SelectedItem.Text);
                qrysec = " and Sections='" + section + "'";
            }
            string testcode = "";
            if (ddl_testname.Items.Count > 0)
            {
                testcode = Convert.ToString(ddl_testname.SelectedItem.Value).Trim();
            }
            string sub = "";
            if (ddlsubject.Items.Count > 0)
            {
                sub = ddlsubject.SelectedItem.Value;
            }
            string is_internal = "";
            if (rblisIntExt.SelectedValue == "2")
            {
                is_internal = " and exq.Test_code=qbm.Exam and exq.Test_code='" + testcode + "' and exq.is_internal='" + rblisIntExt.SelectedValue + "'";
            }
            else
            {
                is_internal = " and exq.is_internal='" + rblisIntExt.SelectedValue + "' and qbm.exam_month=exq.Exam_month and exq.Exam_year=qbm.exam_year and qbm.exam_month='" + ddl_month.SelectedItem.Value + "' and qbm.exam_year='" + ddl_year.SelectedItem.Text.Trim() + "' ";
            }

            ds.Clear();

            string selqry = "";
            if (ddlsubject.Items.Count > 0)
            {
                if (batch != "" && degreecod != "" && sem != "" && sub.Trim() != "" && testcode != "")
                {
                    selqry = "select exq.Exist_questionPK,Question, QuestionMasterPK,exq.Section , qm.mark  from tbl_question_bank_master qbm,tbl_question_master qm, Exist_questions exq where qbm.Subject_no=qm.subject_no and qm.subject_no=exq.subject_no and exq.subject_no=qbm.Subject_no and qm.syllabus=exq.syllabus and qbm.exam_type=exq.is_internal and exq.QuestionMasterFK=qm.QuestionMasterPK and Batch_year='" + batch + "' and Degree_Code='" + degreecod + "' and Semester='" + sem + "' and qbm.Subject_no='" + sub + "'  " + qrysec + " " + is_internal + " order by exq.Exist_questionPK,exq.Section,QuestionMasterPK";

                    ds = d2.select_method_wo_parameter(selqry, "Text");
                }
            }

            Cbl_qstn.Items.Clear();
            Txt_question.Text = "---Select---";
            Cb_qstn.Checked = false;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Cbl_qstn.DataSource = ds;
                    Cbl_qstn.DataTextField = "Question";
                    Cbl_qstn.DataValueField = "QuestionMasterPK";
                    Cbl_qstn.DataBind();
                    if (Cbl_qstn.Items.Count > 0)
                    {
                        for (int row = 0; row < Cbl_qstn.Items.Count; row++)
                        {
                            Cbl_qstn.Items[row].Selected = true;
                            Cb_qstn.Checked = true;
                        }
                        Txt_question.Text = "Questions(" + Cbl_qstn.Items.Count + ")";
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void month_year()
    {
        try
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
                ddl_year.Items.Insert(s, new ListItem(Convert.ToString(r), Convert.ToString(r)));
                s++;
            }
            if (max_yr == min_yr)
            {
                ddl_year.Items.Insert(s, new ListItem(Convert.ToString(max_yr), Convert.ToString(max_yr)));
            }
        }
        catch (Exception ex)
        {
        }
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

    #endregion

    protected void btn_errorclose1_Click(object sender, EventArgs e)
    {
        try
        {
            imgdiv3.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            BindBatch();
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
            rollno_load();
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
            rollno_load();
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            bindsem();
            BindSectionDetail();
            GetSubject();
            rollno_load();
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
            rollno_load();
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            BindSectionDetail();
            GetSubject();
            rollno_load();
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            GetSubject();
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            rollno_load();
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlsubject_Selectchanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            testname();
            load_questions();
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddl_testname_Selectchanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            load_questions();
        }
        catch (Exception ex)
        {
        }
    }

    public void Cb_qstn_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            int cout = 0;
            Txt_question.Text = "--Select--";
            if (Cb_qstn.Checked == true)
            {
                cout++;
                for (int i = 0; i < Cbl_qstn.Items.Count; i++)
                {
                    Cbl_qstn.Items[i].Selected = true;
                }
                Txt_question.Text = "Questions(" + (Cbl_qstn.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cbl_qstn.Items.Count; i++)
                {
                    Cbl_qstn.Items[i].Selected = false;
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

    public void Cbl_qstn_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            Cb_qstn.Checked = false;
            int commcount = 0;
            Txt_question.Text = "--Select--";

            for (int i = 0; i < Cbl_qstn.Items.Count; i++)
            {
                if (Cbl_qstn.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    Cb_qstn.Checked = false;

                }
            }
            if (commcount > 0)
            {
                if (commcount == Cbl_qstn.Items.Count)
                {

                    Cb_qstn.Checked = true;
                }
                Txt_question.Text = "Questions(" + commcount.ToString() + ")";

            }
        }
        catch (Exception ex)
        {

        }
    }

    public void chkCalculateTotalMarks_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            //lblErrSearch.Text = string.Empty;
            //lblErrSearch.Visible = false;
            if (!chkCalculateTotalMarks.Checked)
            {
                Cb_qstn.Checked = false;
                Cb_qstn.Enabled = true;
                Txt_question.Text = "--Select--";
                foreach (ListItem li in Cbl_qstn.Items)
                {
                    li.Selected = false;
                    li.Enabled = true;
                }
            }
            else
            {
                if (Cbl_qstn.Items.Count == 0)
                {
                    chkCalculateTotalMarks.Checked = false;
                    FpSpread1.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Quetions Were Found";
                    return;
                }
                Cb_qstn.Checked = true;
                Cb_qstn.Enabled = false;
                foreach (ListItem li in Cbl_qstn.Items)
                {
                    li.Selected = true;
                    li.Enabled = false;
                }
                Txt_question.Text = "Questions(" + (Cbl_qstn.Items.Count) + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void Cb_rollno_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            int cout = 0;
            Txt_rollno.Text = "--Select--";
            if (Cb_rollno.Checked == true)
            {
                cout++;
                for (int i = 0; i < Cbl_rollno.Items.Count; i++)
                {
                    Cbl_rollno.Items[i].Selected = true;
                }
                Txt_rollno.Text = "Questions(" + (Cbl_rollno.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cbl_rollno.Items.Count; i++)
                {
                    Cbl_rollno.Items[i].Selected = false;
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

    public void Cbl_rollno_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            Cb_rollno.Checked = false;
            int commcount = 0;
            Txt_rollno.Text = "--Select--";

            for (int i = 0; i < Cbl_rollno.Items.Count; i++)
            {
                if (Cbl_rollno.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    Cb_rollno.Checked = false;

                }
            }
            if (commcount > 0)
            {
                if (commcount == Cbl_rollno.Items.Count)
                {

                    Cb_rollno.Checked = true;
                }
                Txt_rollno.Text = "Questions(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    public void rblisIntExt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            if (rblisIntExt.SelectedValue == "2")
            {
                tbl_testname.Visible = true;
                manth_and_Year.Visible = false;
            }
            else
            {
                tbl_testname.Visible = false;
                manth_and_Year.Visible = true;
                month_year();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddl_month_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            load_questions();
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddl_year_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            load_questions();
        }
        catch (Exception ex)
        {
        }
    }

    #region Go Button Click

    public void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            FpSpread1.Visible = true;
            format1();
        }
        catch (Exception ex)
        {
        }
    }

    public void format1()
    {
        try
        {
            string rollno = "";
            DataTable dtQSec = new DataTable();
            FpSpread1.Visible = false;
            btn_save.Visible = false;
            string batch = string.Empty;
            string degreecod = string.Empty;
            string sem = string.Empty;
            string subject_cd = string.Empty;
            string testcod = string.Empty;
            string sub = string.Empty;

            if (ddl_collegename.Items.Count == 0)
            {
                lbl_alert1.Text = "No " + ((isSchool) ? "School" : "College") + " were Found";
                lbl_alert1.Visible = true;
                imgdiv2.Visible = true;
                return;
            }
            else
            {
                collegecode = Convert.ToString(ddl_collegename.SelectedValue);
            }

            if (ddlbatch.Items.Count == 0)
            {
                lbl_alert1.Text = "No " + ((isSchool) ? "Year" : " Batch") + " were Found";
                lbl_alert1.Visible = true;
                imgdiv2.Visible = true;
                return;
            }
            else
            {
                batch = Convert.ToString(ddlbatch.SelectedItem.Text);
            }

            if (ddldegree.Items.Count == 0)
            {
                lbl_alert1.Text = "No " + ((isSchool) ? "School Type" : "Degree") + " were Found";
                lbl_alert1.Visible = true;
                imgdiv2.Visible = true;
                return;
            }

            if (ddlbranch.Items.Count != 0)
            {
                degreecod = Convert.ToString(ddlbranch.SelectedValue);
            }
            else
            {
                lbl_alert1.Text = "No " + ((isSchool) ? "Standard" : "Department") + " were Found";
                lbl_alert1.Visible = true;
                imgdiv2.Visible = true;
                return;
            }

            if (ddlsem.Items.Count != 0)
            {
                sem = Convert.ToString(ddlsem.SelectedItem.Text);
            }
            else
            {
                lbl_alert1.Text = "No " + ((isSchool) ? "Term" : " Semester") + " were Found";
                lbl_alert1.Visible = true;
                imgdiv2.Visible = true;
                return;
            }
            if (ddlsubject.Items.Count != 0)
            {
                subject_cd = Convert.ToString(ddlsubject.SelectedValue);
            }
            else
            {
                lbl_alert1.Text = "No Subject were Found";
                lbl_alert1.Visible = true;
                imgdiv2.Visible = true;
                return;
            }

            if (ddlsubject.Items.Count > 0)
            {
                sub = ddlsubject.SelectedItem.Value.Trim();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Subject Were Found";
                return;
            }
            if (ddl_testname.Items.Count > 0)
            {
                testcod = ddl_testname.SelectedItem.Value.Trim();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Test Were Found";
                return;
            }

            string existquestionPK = "";

            if (Cbl_qstn.Items.Count == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Questions Were Found";
                return;
            }
            else
            {
                for (int i = 0; i < Cbl_qstn.Items.Count; i++)
                {
                    if (Cbl_qstn.Items[i].Selected == true)
                    {
                        if (existquestionPK == "")
                        {
                            existquestionPK = "'" + Cbl_qstn.Items[i].Value.Trim() + "'";
                        }
                        else
                        {
                            string questionname = Convert.ToString(Cbl_qstn.Items[i].Value.Trim().Replace("'", "''"));

                            existquestionPK = existquestionPK + ", '" + questionname + "'";
                        }
                    }
                }
                if (existquestionPK == "")
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "Please Select At Least one Question";
                    return;
                }
            }

            if (Cbl_rollno.Items.Count == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Students Were Found";
                return;
            }
            for (int i = 0; i < Cbl_rollno.Items.Count; i++)
            {
                if (Cbl_rollno.Items[i].Selected == true)
                {
                    if (rollno == "")
                    {
                        rollno = "'" + Cbl_rollno.Items[i].Text.Trim() + "'";
                    }
                    else
                    {
                        rollno = rollno + "," + "'" + Cbl_rollno.Items[i].Text.Trim() + "'";
                    }
                }
            }
            if (rollno == "")
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "Please Select At Least One Student.";
                return;
            }

            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].ColumnCount = 4;
            FpSpread1.Sheets[0].FrozenColumnCount = 4;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].SelectionBackColor = Color.LightGreen;
            FarPoint.Web.Spread.TextCellType txtType = new FarPoint.Web.Spread.TextCellType();
            FpSpread1.SaveChanges();
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Columns[1].CellType = txtType;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Columns[2].CellType = txtType;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].Width = 180;

            FpSpread1.Sheets[0].Columns[0].Resizable = false;
            FpSpread1.Sheets[0].Columns[1].Resizable = false;
            FpSpread1.Sheets[0].Columns[2].Resizable = false;
            FpSpread1.Sheets[0].Columns[3].Resizable = false;

            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

            batch = "";
            if (ddlbatch.Items.Count > 0)
            {
                batch = Convert.ToString(ddlbatch.SelectedItem.Text);
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Batch Were Found";
                return;
            }

            degreecod = "";
            if (ddlbranch.Items.Count > 0)
            {
                degreecod = Convert.ToString(ddlbranch.SelectedItem.Value).Trim();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Branch Were Found";
                return;
            }

            sem = "";
            if (ddlsem.Items.Count > 0)
            {
                sem = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Semester Were Found";
                return;
            }

            string section = "";
            string strsec = "";
            if (ddlsec.Items.Count > 0)
            {
                section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                if (Convert.ToString(ddlsec.SelectedValue).Trim().ToLower() == "all" || Convert.ToString(ddlsec.SelectedValue).Trim().ToLower() == "")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and Sections='" + Convert.ToString(section).Trim() + "'";
                }
            }

            if (ddlsubject.Items.Count > 0)
            {
                sub = ddlsubject.SelectedItem.Value.Trim();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Subject Were Found";
                return;
            }

            if (ddl_testname.Items.Count > 0)
            {
                testcod = ddl_testname.SelectedItem.Value.Trim();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Test Were Found";
                return;
            }

            string is_internal = "";
            if (rblisIntExt.SelectedValue == "2")
            {
                is_internal = " and exq.Test_code=qbm.Exam and exq.Test_code='" + testcod + "' and exq.is_internal='" + rblisIntExt.SelectedValue + "'";
            }
            else
            {
                is_internal = " and exq.is_internal='" + rblisIntExt.SelectedValue + "' and qbm.exam_month=exq.Exam_month and exq.Exam_year=qbm.exam_year and qbm.exam_month='" + ddl_month.SelectedItem.Value + "' and qbm.exam_year='" + ddl_year.SelectedItem.Text.Trim() + "' ";
            }

            ds.Clear();

            string qury = " select Roll_No,Reg_No,Stud_Name from Registration where Batch_Year='" + batch + "' and degree_code='" + degreecod + "'  and Current_Semester ='" + sem + "' " + strsec + " and Roll_No in(" + rollno + ") and  CC='0' and DelFlag='0' and Exam_Flag<>'debar' order by Roll_No";

            qury = qury + "  select ROW_NUMBER() OVER (ORDER BY exq.Exist_questionPK,exq.Section,QuestionMasterPK) AS QNo1,exq.Exist_questionPK,Question, QuestionMasterPK, qm.mark,exq.Section,QNo  from tbl_question_bank_master qbm,tbl_question_master qm, Exist_questions exq where qbm.Subject_no=qm.subject_no and qm.subject_no=exq.subject_no and exq.subject_no=qbm.Subject_no and qm.syllabus=exq.syllabus and qbm.exam_type=exq.is_internal and exq.QuestionMasterFK=qm.QuestionMasterPK and Batch_year='" + batch + "' and Degree_Code='" + degreecod + "' and Semester='" + sem + "' and qbm.Subject_no='" + sub + "'  " + strsec + " " + is_internal + " order by  exq.Exist_questionPK,exq.Section,QuestionMasterPK ; ";

            qury = qury + " select * from questionwise_marksentry where subject_no='" + sub + "' and isinternal='2' and Questionentryid in(" + existquestionPK + ") and roll_no in(" + rollno + ") and criteria_no='" + testcod + "' ";
            ds = d2.select_method_wo_parameter(qury, "Text");

            int qstnno = 0;
            DataView dvq = new DataView();

            if (ds.Tables.Count >= 2 && ds.Tables[1].Rows.Count > 0)
            {
                dtQSec = ds.Tables[1].DefaultView.ToTable(true, "Section", "mark");
                if (dtQSec.Rows.Count > 0)
                {
                    int qno = 0;
                    for (int dr = 0; dr < dtQSec.Rows.Count; dr++)
                    {
                        ds.Tables[1].DefaultView.RowFilter = "Section='" + Convert.ToString(dtQSec.Rows[dr]["Section"]) + "' and QuestionMasterPK in(" + existquestionPK + ")";
                        dvq = ds.Tables[1].DefaultView;
                        //dvq = dtQSec.DefaultView;
                        if (dvq.Count > 0)
                        {
                            int spancol = FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Resizable = false;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "PART - " + Convert.ToString(dtQSec.Rows[dr]["Section"]) + " Max Mark (" + Convert.ToString(dtQSec.Rows[dr]["mark"] + ") ");
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            for (int i = 0; i < dvq.Count; i++)
                            {
                                if (i != 0)
                                {
                                    FpSpread1.Sheets[0].ColumnCount++;
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Resizable = false;

                                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "PART - " + Convert.ToString(dvq[i]["Section"]) + " Max Mark (" + Convert.ToString(dvq[i]["mark"] + ") ");
                                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 1, dvq.Count);
                                }
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dvq[i]["QNo"]);// Convert.ToString(qstnno + 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dvq[i]["QuestionMasterPK"]);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = Convert.ToString(dvq[i]["mark"]);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.Gold;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                qstnno++;
                                // FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 100;
                            }
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, spancol, 1, dvq.Count);
                        }
                    }
                }
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int he = 30;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    he = he + 30;
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                    FpSpread1.Sheets[0].Cells[i, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                    FpSpread1.Sheets[0].Cells[i, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                    FpSpread1.Sheets[0].Cells[i, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                    for (int cl = 4; cl < FpSpread1.Sheets[0].Columns.Count; cl++)
                    {
                        FarPoint.Web.Spread.DoubleCellType intgrcel = new FarPoint.Web.Spread.DoubleCellType();
                        intgrcel.MinimumValue = 0;
                        FpSpread1.Sheets[0].Cells[i, cl].CellType = intgrcel;

                        string QuestionId = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, cl].Tag);

                        intgrcel.MaximumValue = Convert.ToInt32(FpSpread1.Sheets[0].ColumnHeader.Cells[1, cl].Note);
                        intgrcel.ErrorMessage = "Enter Valid  Mark Between 0 and " + Convert.ToInt32(FpSpread1.Sheets[0].ColumnHeader.Cells[1, cl].Note);
                        FpSpread1.Sheets[0].Cells[i, cl].HorizontalAlign = HorizontalAlign.Center;
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            DataView dv = new DataView();
                            DataTable marks = new DataTable();
                            ds.Tables[2].DefaultView.RowFilter = "roll_no='" + Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]) + "' and Questionentryid='" + QuestionId + "'";
                            dv = ds.Tables[2].DefaultView;
                            if (dv.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[i, cl].Text = Convert.ToString(dv[0]["mark_obtained"]);
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[i, cl].Text = "";
                            }
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[i, cl].Text = "";
                        }

                    }
                }
                FpSpread1.Columns[0].Locked = true;
                FpSpread1.Columns[1].Locked = true;
                FpSpread1.Columns[2].Locked = true;
                FpSpread1.Columns[3].Locked = true;
                FpSpread1.Height = he + 60;
                FpSpread1.SaveChanges();
                FpSpread1.Visible = true;
                btn_save.Visible = true;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            }
            else
            {
                FpSpread1.Visible = false;
                btn_save.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    #endregion  Go Button Click

    #region Save Click

    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string subj = "";
            FpSpread1.SaveChanges();
            bool check = false;
            string batch_year = string.Empty;
            string degree_code = string.Empty;
            string testNo = string.Empty;
            string qrysec = string.Empty;
            string qrysec1 = string.Empty;
            string semester = string.Empty;

            string duration = string.Empty;
            string exam_code = string.Empty;
            string exam_date = string.Empty;
            string minMark = string.Empty;
            string maxMark = string.Empty;
            string section = string.Empty;
            DataSet dsExamDetails = new DataSet();
            if (ddlbatch.Items.Count == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No " + ((isSchool) ? "Year" : " Batch") + " were Found";
                return;
            }
            else
            {
                batch_year = Convert.ToString(ddlbatch.SelectedValue).Trim();
            }
            if (ddlbranch.Items.Count == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No " + ((isSchool) ? "Standard" : "Department") + " were Found";
                return;
            }
            else
            {
                degree_code = Convert.ToString(ddlbranch.SelectedValue).Trim();
            }
            if (ddlsem.Items.Count == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No " + ((isSchool) ? "Term" : " Semester") + " were Found";
                return;
            }
            else
            {
                semester = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            }

            if (ddlsubject.Items.Count > 0)
            {
                subj = Convert.ToString(ddlsubject.SelectedItem.Value).Trim();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Subject Were Found";
                return;
            }
            if (ddl_testname.Items.Count == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Test Were Found";
                return;
            }
            else
            {
                testNo = Convert.ToString(ddl_testname.SelectedValue).Trim();
            }
            section = string.Empty;
            string qmsec = string.Empty;
            if (ddlsec.Items.Count > 0)
            {
                if (ddlsec.Enabled)
                {
                    if (Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddlsec.SelectedItem.Text).Trim() != "")
                    {
                        section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                        qrysec = " and sections='" + Convert.ToString(ddlsec.SelectedItem.Text).Trim() + "'";
                        qrysec1 = " and Sections='" + Convert.ToString(ddlsec.SelectedItem.Text).Trim() + "' ";
                        qmsec = "  and qm.Sections='" + Convert.ToString(ddlsec.SelectedItem.Text).Trim() + "'";
                    }
                    else
                    {
                        section = string.Empty;
                        qrysec = string.Empty;
                    }
                }
                else
                {
                    section = string.Empty;
                    qrysec = string.Empty;
                }
            }
            else
            {
                section = string.Empty;
                qrysec = string.Empty;
            }

            string is_internal = "";
            string creatqry = "";
            string creatvalu = "";
            string qrynew = "";
            if (rblisIntExt.SelectedValue == "2")
            {
                string testname = "";
                qrynew = "select Batch_year,Degree_Code,Semester,Sections,Subject_no,exam_type,Exam,Duration,Exam,Convert(nvarchar(50),exam_date,103) as exam_date,min_mark,max_mark from tbl_question_bank_master where Batch_year='" + batch_year + "' and Degree_Code='" + degree_code + "' and Semester='" + semester + "' " + qrysec1 + "  and Subject_no='" + subj + "' and exam_type='" + Convert.ToString(rblisIntExt.SelectedValue).Trim() + "' and Exam='" + testNo + "'";
                dsExamDetails = d2.select_method_wo_parameter(qrynew, "text");
                if (dsExamDetails.Tables.Count > 0 && dsExamDetails.Tables[0].Rows.Count > 0)
                {
                    duration = Convert.ToString(dsExamDetails.Tables[0].Rows[0]["Duration"]).Trim();
                    exam_date = Convert.ToString(dsExamDetails.Tables[0].Rows[0]["exam_date"]).Trim();
                    minMark = Convert.ToString(dsExamDetails.Tables[0].Rows[0]["min_mark"]).Trim();
                    maxMark = Convert.ToString(dsExamDetails.Tables[0].Rows[0]["max_mark"]).Trim();
                    if (maxMark.Trim() == "" || maxMark.Trim() == "0")
                    {
                        string max = d2.GetFunctionv("select sum(distinct qmq.mark * Must_attend) from Exist_questions eq,tbl_question_bank_master qm,tbl_question_master qmq where qmq.subject_no=eq.subject_no and qmq.syllabus=eq.syllabus and qmq.QuestionMasterPK=eq.QuestionMasterFK and qmq.subject_no=qm.Subject_no  and Test_code=Exam and qm.Subject_no=eq.subject_no and qm.exam_type=eq.is_internal and qm.Batch_year='" + batch_year.Trim() + "' and qm.Degree_Code='" + degree_code.Trim() + "' and qm.Semester='" + semester.Trim() + "' " + qmsec + " and qm.exam_type='" + Convert.ToString(rblisIntExt.SelectedValue).Trim() + "' and eq.subject_no='" + subj.Trim() + "' and Test_code='" + testNo.Trim() + "' group by eq.Test_code");

                        string newMin = d2.GetFunctionv("select min_mark from CriteriaForInternal where Criteria_no='" + testNo.Trim() + "'");
                        string newMax = d2.GetFunctionv("select max_mark from CriteriaForInternal where Criteria_no='" + testNo.Trim() + "'");
                        //where Criteria_no='332'"
                        max = ((max.Trim() == "" || max.Trim() == "0") ? newMax : max);
                        string min = ((minMark.Trim() == "" || minMark.Trim() == "0") ? newMin : minMark);
                        ToConvertedMark(max, ref max, ref min);
                        maxMark = max;
                        minMark = min;
                        int insres = d2.update_method_wo_parameter("if exists (select * from tbl_question_bank_master where Batch_year='" + batch_year + "' and Degree_Code='" + degree_code + "' and Semester='" + semester + "' " + qrysec1 + "  and Subject_no='" + subj + "' and exam_type='" + Convert.ToString(rblisIntExt.SelectedValue).Trim() + "' and Exam='" + testNo + "') update tbl_question_bank_master set min_mark='" + min.Trim() + "',max_mark='" + max.Trim() + "' where Batch_year='" + batch_year + "' and Degree_Code='" + degree_code + "' and Semester='" + semester + "' " + qrysec1 + "  and Subject_no='" + subj + "' and exam_type='" + Convert.ToString(rblisIntExt.SelectedValue).Trim() + "' and Exam='" + testNo + "' ", "Text");
                    }

                    //maxMark = ((maxMark.Trim() == "" || maxMark.Trim() == "0") ? "100" : maxMark);
                    //minMark = ((minMark.Trim() == "" || minMark.Trim() == "0") ? "0" : minMark);

                }
                if (ddl_testname.Items.Count > 0)
                {
                    testname = ddl_testname.SelectedItem.Value;
                }
                is_internal = " and criteria_no='" + testname + "' and  isinternal='" + Convert.ToString(rblisIntExt.SelectedValue).Trim() + "'";
                creatqry = " , isinternal,criteria_no";
                creatvalu = " ,'" + Convert.ToString(rblisIntExt.SelectedValue).Trim() + "','" + testname + "' ";
            }
            else
            {
                is_internal = " and  isinternal='" + Convert.ToString(rblisIntExt.SelectedValue).Trim() + "' and exam_month='" + ddl_month.SelectedItem.Value + "' and  exam_year='" + ddl_year.SelectedItem.Text.Trim() + "' ";
                creatqry = " , isinternal,exam_month,exam_year ";
                creatvalu = " ,'" + Convert.ToString(rblisIntExt.SelectedValue).Trim() + "', '" + ddl_month.SelectedItem.Value + "','" + ddl_year.SelectedItem.Text.Trim() + "'";
            }


            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                string rollno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Text.Trim());
                string regno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text).Trim();
                string studentname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text).Trim();
                double totalMarks = 0;
                for (int col = 4; col < FpSpread1.Sheets[0].Columns.Count; col++)
                {
                    string marks = Convert.ToString(FpSpread1.Sheets[0].Cells[i, col].Text).Trim();
                    string questionpk = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag);
                    // string insqry = "insert into questionwise_marksentry (Questionentryid,mark_obtained,roll_no,subject_no) values('" + subj + "','" + marks + "','" + rollno + "','" + subj + "')";
                    double qmark = 0;
                    bool isValmark = double.TryParse(marks.Trim(), out qmark);
                    if (isValmark)
                    {
                        totalMarks += qmark;
                        DateTime currentdt = DateTime.Now;
                        string insqry = "  if exists(select * from questionwise_marksentry where roll_no='" + rollno + "' and Questionentryid='" + questionpk + "' and subject_no='" + subj.Trim() + "' " + is_internal + " ) update questionwise_marksentry set mark_obtained='" + qmark + "',Mark_Entry_date='" + currentdt.ToString("MM/dd/yyyy") + "'  where   roll_no='" + rollno + "' and Questionentryid='" + questionpk + "' and subject_no='" + subj.Trim() + "'  else insert into questionwise_marksentry (Questionentryid,mark_obtained,roll_no,subject_no,Mark_Entry_date  " + creatqry + ") values('" + questionpk + "','" + qmark + "','" + rollno + "','" + subj.Trim() + "','" + currentdt.ToString("MM/dd/yyyy") + "'  " + creatvalu + ") ";
                        int inqury = d2.update_method_wo_parameter(insqry, "Text");
                        if (inqury != 0)
                        {
                            check = true;
                        }
                    }
                }
                if (rblisIntExt.SelectedValue == "2")
                {
                    DateTime dtExamDate = new DateTime();
                    DateTime.TryParseExact(exam_date, "dd/MM/yyyy", null, DateTimeStyles.None, out dtExamDate);
                    string newqry = "if exists (select * from Exam_type where criteria_no='" + testNo + "' and subject_no='" + subj.Trim() + "' and batch_year='" + batch_year + "' " + qrysec + " ) update Exam_type set duration='" + duration.Trim() + "',entry_date='" + DateTime.Now.ToString("MM/dd/yyyy") + "',exam_date='" + dtExamDate.ToString("MM/dd/yyyy") + "',max_mark='" + maxMark.Trim() + "',min_mark='" + minMark.Trim() + "',new_maxmark='" + maxMark.Trim() + "',new_minmark='" + minMark.Trim() + "' where criteria_no='" + testNo.Trim() + "' and subject_no='" + subj.Trim() + "' and batch_year='" + batch_year.Trim() + "' " + qrysec + "   else insert into Exam_type (criteria_no,subject_no,duration,entry_date,exam_date,batch_year,max_mark,min_mark,sections,new_maxmark,new_minmark,islock) values ('" + testNo + "','" + subj.Trim() + "','" + duration.Trim() + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + dtExamDate.ToString("MM/dd/yyyy") + "','" + batch_year.Trim() + "','" + maxMark.Trim() + "','" + minMark.Trim() + "','" + section.Trim() + "','" + maxMark.Trim() + "','" + minMark.Trim() + "','0')";
                    int newres = d2.update_method_wo_parameter(newqry, "text");
                    exam_code = d2.GetFunctionv("select exam_code from Exam_type where criteria_no='" + testNo.Trim() + "' and subject_no='" + subj.Trim() + "' and batch_year='" + batch_year.Trim() + "' " + qrysec + "").Trim();
                    if (chkCalculateTotalMarks.Checked)
                    {
                        if (exam_code.Trim() != "")
                        {
                            double newmax = 0;
                            double.TryParse(maxMark.Trim(), out newmax);
                            if (totalMarks > newmax)
                            {
                                totalMarks = newmax;
                            }
                            string nj = "if exists(select * from Result  where exam_code='" + exam_code.Trim() + "' and roll_no='" + rollno.Trim() + "') update Result set marks_obtained='" + totalMarks + "' where exam_code='" + exam_code.Trim() + "' and roll_no='" + rollno.Trim() + "' else insert into Result (exam_code,roll_no,marks_obtained) values('" + exam_code.Trim() + "','" + rollno.Trim() + "','" + totalMarks + "')";
                            int insresult = d2.update_method_wo_parameter(nj, "Text");
                        }
                    }
                }

            }
            if (check == true)
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "Saved Successfully";
                return;
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "Not Saved";
                return;
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }

    }

    #endregion Save Click

    protected void FpSpread1_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        FpSpread1.SaveChanges();
        try
        {
            int row = FpSpread1.ActiveSheetView.ActiveRow;
            int col = FpSpread1.ActiveSheetView.ActiveColumn;
            if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 6].Value) == 1)
            {
                FpSpread1.Sheets[0].Cells[row, 6].Value = 1;
            }
            else if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, 6].Value) == 0)
            {
                FpSpread1.Sheets[0].Cells[row, 6].Value = 0;

            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    private bool IsValidDauration(string duartion)
    {
        try
        {
            Regex regex = new Regex(@"^[0-2]{1}[0-3]{1}:[0-5]{1}[0-9]{1}$");
            Match match = regex.Match(duartion);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public void ToConvertedMark(string txtConvertTo, ref string maxMark, ref string minMark)
    {
        try
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
                            if (minmultyply > 0)
                                min = int.Parse(txtConvertTo) / minmultyply;
                        }
                        break;
                }
                minMark = min.ToString();
                maxMark = txtConvertTo;
            }
        }
        catch (Exception ex)
        {

        }
    }

    /// <summary>
    /// author Malang Raja T
    /// </summary>
    /// <param name="txtConvertTo">A string type txtConvertTo is used for to be converted</param>
    /// <param name="maxMark">ref type maxMark parameter was used to gives the minimum mark for converted obtained marks</param>
    /// <param name="obtainedMark">ref type obtainedMark parameter was used to gives the calculated or converted obtained marks</param>
    /// <param name="minMark">ref type minMark parameter was used to gives the minimum mark for converted obtained marks</param>
    public void ConvertedMark(string txtConvertTo, ref string maxMark, ref string obtainedMark, ref string minMark)
    {
        try
        {
            int Mark, max;
            bool r = int.TryParse(obtainedMark, out Mark);
            bool maxflag = int.TryParse(txtConvertTo, out max);
            double multiply;
            double minmultyply;
            double min = 0;
            double max_minCal = 0;
            bool maxbool = double.TryParse(maxMark, out max_minCal);
            bool minbool = double.TryParse(minMark, out min);
            if (maxflag)
            {
                if (r)
                {
                    switch (txtConvertTo)
                    {
                        default:
                            if (max_minCal > 0)
                            {
                                multiply = double.Parse(txtConvertTo) / int.Parse(maxMark);
                                if (maxbool == true && minbool == true && min > 0)
                                {
                                    minmultyply = max_minCal / min;
                                    if (minmultyply > 0)
                                        min = int.Parse(txtConvertTo) / minmultyply;
                                }
                                obtainedMark = Convert.ToString(Mark * multiply);
                            }
                            break;
                    }
                }
                minMark = min.ToString();
                maxMark = txtConvertTo;
            }
        }
        catch (Exception ex)
        {
        }
    }

}