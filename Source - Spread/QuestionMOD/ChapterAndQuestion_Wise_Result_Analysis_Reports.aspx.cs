using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using FarPoint.Web.Spread;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Farpoint = FarPoint.Web.Spread;
using System.Configuration;

public partial class ChapterAndQuestion_Wise_Result_Analysis_Reports : System.Web.UI.Page
{

    Hashtable hat = new Hashtable();

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string batch_year = string.Empty;
    string degree_code = string.Empty;
    string semester = string.Empty;
    string section = string.Empty;
    string test_name = string.Empty;
    string test_no = string.Empty;
    string subject_no = string.Empty;
    string exam_type = string.Empty;
    string exam_code = string.Empty;
    string questionid = string.Empty;
    string qry = string.Empty;
    string qrysec = string.Empty;
    string qryInternal = string.Empty;

    bool isSchool = false;
    bool isInternal = true;

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

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
                Printcontrol1.Visible = false;
                rptprint1.Visible = false;
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                popupdiv.Visible = false;
                FpSpread1.Visible = false;
                chkShowSelQuestions.Checked = false;
                //btnSave.Visible = false;

                #region LoadHeader

                Bindcollege();
                BindBatch();
                BindDegree();
                bindbranch();
                bindsem();
                BindSectionDetail();
                GetSubject();
                BindTest();
                bindQuestions();

                #endregion LoadHeader

                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                string grouporusercode = "";

                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }

                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet ds = d2.select_method(Master, hat, "Text");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim() == "Roll No" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim() == "Register No" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim() == "Student_Type" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                }
                ChangeHeaderName(isSchool);
                Init_Spread();
            }
        }
        catch (Exception ex) { }
    }

    #endregion Page Load

    #region Bind Header

    public void bindcollege()
    {
        try
        {
            Printcontrol1.Visible = false;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            FpSpread1.Visible = false;
            ////btnSave.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    public void Bindcollege()
    {
        try
        {

            Printcontrol1.Visible = false;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            FpSpread1.Visible = false;

            string columnfield = "";
            group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", Convert.ToString(columnfield));
            ds.Dispose();
            ds.Clear();
            ds.Reset();
            ds = d2.select_method("bind_college", hat, "sp");
            ddlCollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
            else
            {
                lblErrSearch.Text = "Set college rights to the staff";
                lblErrSearch.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindBatch()
    {
        try
        {
            Printcontrol1.Visible = false;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
                ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    public void BindDegree()
    {
        try
        {
            Printcontrol1.Visible = false;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            ddldegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();

            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }

    }

    public void bindbranch()
    {
        try
        {
            Printcontrol1.Visible = false;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            string course_id = Convert.ToString(ddldegree.SelectedValue);
            ddlbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    public void BindSectionDetail()
    {
        try
        {
            Printcontrol1.Visible = false;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;

            string strbatch = Convert.ToString(ddlbatch.SelectedValue);
            string strbranch = Convert.ToString(ddlbranch.SelectedValue);

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
            GetSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    public void bindsem()
    {
        try
        {
            Printcontrol1.Visible = false;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;

            string strbatchyear = Convert.ToString(ddlbatch.Text);
            string strbranch = Convert.ToString(ddlbranch.SelectedValue);

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
                bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(Convert.ToString(i));
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsem.Items.Add(Convert.ToString(i));
                    }
                }
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    public void GetSubject()
    {
        try
        {
            Printcontrol1.Visible = false;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;

            string subjectquery = string.Empty;
            ddlsubject.Items.Clear();

            string sections = string.Empty;// Convert.ToString(ddlsec.SelectedValue);
            string strsec = "";
            if (ddlsec.Items.Count > 0)
            {
                sections = Convert.ToString(ddlsec.SelectedValue);
                if (Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() == "")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and st.Sections='" + Convert.ToString(sections) + "'";
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
                        sems = "and SM.semester='" + Convert.ToString(ddlsem.SelectedValue).Trim() + "'";
                    }

                    if (Convert.ToString(Session["Staff_Code"]) == "")
                    {
                        //subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' order by S.subject_no ";
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "' order by S.subject_no ";
                    }
                    else if (Convert.ToString(Session["Staff_Code"]) != "")
                    {
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and st.subject_no=s.subject_no and s.syll_code=SM.syll_code and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue) + "'  and SC.staff_code='" + Convert.ToString(Session["Staff_Code"]) + "' " + strsec + " order by S.subject_no ";
                    }
                    if (subjectquery != "")
                    {
                        ds.Dispose();
                        ds.Reset();
                        ds = d2.select_method(subjectquery, hat, "Text");
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
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    public void BindTest()
    {
        try
        {
            Printcontrol1.Visible = false;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;

            ddlTest.Items.Clear();
            ds.Clear();
            ds.Reset();
            ds.Dispose();
            batch_year = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
            degree_code = Convert.ToString(ddlbranch.SelectedValue).Trim();
            semester = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            qry = "";
            if (!string.IsNullOrEmpty(batch_year) && !string.IsNullOrEmpty(degree_code) && !string.IsNullOrEmpty(semester))
            {
                qry = "select c.criteria,c.Criteria_no from CriteriaForInternal c, syllabus_master sy where c.syll_code=sy.syll_code and sy.Batch_Year='" + batch_year + "' and sy.degree_code='" + degree_code + "' and sy.semester='" + semester + "'";
                ds = d2.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlTest.DataSource = ds;
                    ddlTest.DataTextField = "criteria";
                    ddlTest.DataValueField = "Criteria_no";
                    ddlTest.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void bindQuestions()
    {
        try
        {
            Printcontrol1.Visible = false;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;

            cbl_Questions.Items.Clear();
            txt_Questions.Text = "-- Select --";
            cb_Questions.Checked = false;
            if (ddlCollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }

            if (ddlbatch.Items.Count != 0)
            {
                batch_year = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
            }

            if (ddlbranch.Items.Count != 0)
            {
                degree_code = Convert.ToString(ddlbranch.SelectedValue).Trim();
            }

            if (ddlsem.Items.Count != 0)
            {
                semester = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            }

            if (ddlsec.Enabled == false || ddlsec.Items.Count == 0)
            {
                section = "";
                qrysec = "";
            }
            else if (ddlsec.Items.Count > 0)
            {
                section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                qrysec = "  and Sections='" + section + "'";
            }
            if (ddlsubject.Items.Count != 0)
            {
                subject_no = Convert.ToString(ddlsubject.SelectedValue).Trim();
            }
            if (ddlTest.Items.Count > 0)
            {
                test_no = Convert.ToString(ddlTest.SelectedValue).Trim();
            }

            if (!string.IsNullOrEmpty(collegecode.Trim()) && !string.IsNullOrEmpty(batch_year.Trim()) && !string.IsNullOrEmpty(degree_code.Trim()) && !string.IsNullOrEmpty(semester.Trim()) && !string.IsNullOrEmpty(subject_no.Trim()) && !string.IsNullOrEmpty(test_no))
            {
                //if (section != "")
                //{
                //qry = "select distinct Questionentryid,No_Sections,qbd.qsection_no,Total_Questions,Minimu_Attend,no_Option,Marks,Syllabus,Questions from tbl_question_bank_master qb,tbl_Question_Bank_details qbd,tbl_Question_Bank_Questions qbq where qb.Questionid=qbd.Questionid and qbq.Questionid=qb.Questionid and qbd.qsection_no=qbq.qsection_no and qb.Subject_no=qbd.Subject_no and qbq.Subject_no=qbd.Subject_no   and qbd.Subject_no='" + subject_no + "' and Degree_Code='" + degree_code + "' and Batch_year='" + batch_year + "' and Semester='" + semester + "' " + qrysec + " order by qbd.qsection_no";
                qry = "select exq.Exist_questionPK,qm.QuestionMasterPK,qm.question,exq.Section,qm.mark from tbl_question_bank_master qbm,tbl_question_master qm, Exist_questions exq,sub_unit_details sud where qbm.Subject_no=qm.subject_no and qm.subject_no=exq.subject_no and exq.subject_no=qbm.Subject_no and qm.syllabus=exq.syllabus and qbm.exam_type=exq.is_internal and exq.QuestionMasterFK=qm.QuestionMasterPK and sud.subject_no=qbm.Subject_no and sud.subject_no=qm.subject_no and exq.subject_no=sud.subject_no and sud.topic_no=qm.syllabus and sud.topic_no=exq.syllabus and qbm.Batch_year='" + batch_year + "' and qbm.Degree_Code='" + degree_code + "' and qbm.Semester='" + semester + "' and qbm.Subject_no='" + subject_no + "' " + qrysec + " and exq.Test_code=qbm.Exam and exq.Test_code='" + test_no + "' and exq.is_internal=2 order by exq.Exist_questionPK,exq.Section,qm.QuestionMasterPK ; ";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(qry, "Text");
                //}
                //else
                //{
                //    qry = "select distinct Questionentryid,No_Sections,qbd.qsection_no,Total_Questions,Minimu_Attend,no_Option,Marks,Syllabus,Questions from tbl_question_bank_master qb,tbl_Question_Bank_details qbd,tbl_Question_Bank_Questions qbq where qb.Questionid=qbd.Questionid and qbq.Questionid=qb.Questionid and qbd.qsection_no=qbq.qsection_no and qb.Subject_no=qbd.Subject_no and qbq.Subject_no=qbd.Subject_no and qbd.Subject_no='" + subject_no + "' and Degree_Code='" + degree_code + "' and Batch_year='" + batch_year + "' and Semester='" + semester + "'  order by qbd.qsection_no";
                //    ds.Clear();
                //    ds.Reset();
                //    ds.Dispose();
                //    ds = d2.select_method_wo_parameter(qry, "Text");
                //}
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_Questions.DataSource = ds;
                    cbl_Questions.DataTextField = "question";
                    cbl_Questions.DataValueField = "QuestionMasterPK";
                    cbl_Questions.DataBind();
                    for (int h = 0; h < cbl_Questions.Items.Count; h++)
                    {
                        cbl_Questions.Items[h].Selected = true;
                    }
                    txt_Questions.Text = "Question" + "(" + cbl_Questions.Items.Count + ")";
                    cb_Questions.Checked = true;
                }
            }
           

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    public void Init_Spread()
    {
        #region FpSpread Style

        FpSpread1.Visible = false;
        FpSpread1.Sheets[0].ColumnCount = 0;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
        FpSpread1.CommandBar.Visible = false;

        #endregion FpSpread Style

        FpSpread1.Visible = false;
        FpSpread1.CommandBar.Visible = false;
        FpSpread1.RowHeader.Visible = false;
        FpSpread1.Sheets[0].AutoPostBack = false;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 4;
        FpSpread1.Sheets[0].FrozenColumnCount = 4;

        #region SpreadStyles

        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
        //darkstyle.ForeColor = System.Drawing.Color.Black;
        darkstyle.Font.Name = "Book Antiqua";
        darkstyle.Font.Size = FontUnit.Medium;
        darkstyle.Font.Bold = true;
        darkstyle.HorizontalAlign = HorizontalAlign.Center;
        darkstyle.VerticalAlign = VerticalAlign.Middle;
        darkstyle.ForeColor = System.Drawing.Color.White;
        darkstyle.Border.BorderSize = 1;
        darkstyle.Border.BorderColor = System.Drawing.Color.Black;

        FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
        //sheetstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
        //darkstyle.ForeColor = System.Drawing.Color.Black;
        sheetstyle.Font.Name = "Book Antiqua";
        sheetstyle.Font.Size = FontUnit.Medium;
        sheetstyle.Font.Bold = true;
        sheetstyle.HorizontalAlign = HorizontalAlign.Center;
        sheetstyle.VerticalAlign = VerticalAlign.Middle;
        sheetstyle.ForeColor = System.Drawing.Color.Black;
        sheetstyle.Border.BorderSize = 1;
        sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

        #endregion SpreadStyles

        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        FpSpread1.Sheets[0].DefaultStyle = sheetstyle;
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread1.Sheets[0].Columns[0].Width = 40;
        FpSpread1.Sheets[0].Columns[1].Width = 150;
        FpSpread1.Sheets[0].Columns[2].Width = 40;
        FpSpread1.Sheets[0].Columns[3].Width = 80;
        FpSpread1.Sheets[0].Columns[0].Locked = true;
        FpSpread1.Sheets[0].Columns[1].Locked = true;
        FpSpread1.Sheets[0].Columns[2].Locked = true;
        FpSpread1.Sheets[0].Columns[3].Locked = true;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Chapter Name";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "QNo.";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Max.Mark";

        FpSpread1.Sheets[0].Columns[0].Resizable = false;
        FpSpread1.Sheets[0].Columns[1].Resizable = false;
        FpSpread1.Sheets[0].Columns[2].Resizable = false;
        FpSpread1.Sheets[0].Columns[3].Resizable = false;

        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
    }

    public void Init_Spread(FpSpread FpSpread1)
    {
        #region FpSpread Style

        FpSpread1.Visible = false;
        FpSpread1.Sheets[0].ColumnCount = 0;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
        FpSpread1.CommandBar.Visible = false;

        #endregion FpSpread Style

        //FpSpread1.Visible = false;
        FpSpread1.CommandBar.Visible = false;
        FpSpread1.RowHeader.Visible = false;
        FpSpread1.Sheets[0].AutoPostBack = true;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 4;
        //FpSpread1.Sheets[0].FrozenColumnCount = 4;

        #region SpreadStyles

        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
        //darkstyle.ForeColor = System.Drawing.Color.Black;
        darkstyle.Font.Name = "Book Antiqua";
        darkstyle.Font.Size = FontUnit.Medium;
        darkstyle.Font.Bold = true;
        darkstyle.HorizontalAlign = HorizontalAlign.Center;
        darkstyle.VerticalAlign = VerticalAlign.Middle;
        darkstyle.ForeColor = System.Drawing.Color.White;
        darkstyle.Border.BorderSize = 1;
        darkstyle.Border.BorderColor = System.Drawing.Color.Black;

        FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
        //sheetstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
        //darkstyle.ForeColor = System.Drawing.Color.Black;
        sheetstyle.Font.Name = "Book Antiqua";
        sheetstyle.Font.Size = FontUnit.Medium;
        sheetstyle.Font.Bold = true;
        sheetstyle.HorizontalAlign = HorizontalAlign.Center;
        sheetstyle.VerticalAlign = VerticalAlign.Middle;
        sheetstyle.ForeColor = System.Drawing.Color.Black;
        sheetstyle.Border.BorderSize = 1;
        sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

        #endregion SpreadStyles

        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        FpSpread1.Sheets[0].DefaultStyle = sheetstyle;
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread1.Sheets[0].Columns[0].Width = 40;
        FpSpread1.Sheets[0].Columns[1].Width = 150;
        FpSpread1.Sheets[0].Columns[2].Width = 40;
        FpSpread1.Sheets[0].Columns[3].Width = 80;
        FpSpread1.Sheets[0].Columns[0].Locked = true;
        FpSpread1.Sheets[0].Columns[1].Locked = true;
        FpSpread1.Sheets[0].Columns[2].Locked = true;
        FpSpread1.Sheets[0].Columns[3].Locked = true;

        FpSpread1.Sheets[0].Columns[0].Resizable = false;
        FpSpread1.Sheets[0].Columns[1].Resizable = false;
        FpSpread1.Sheets[0].Columns[2].Resizable = false;
        FpSpread1.Sheets[0].Columns[3].Resizable = false;

        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Chapter Name";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "QNo.";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Max.Mark";
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
    }

    public void ChangeHeaderName(bool isschool)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = "";
            lblCollege.Text = ((!isschool) ? "College" : "School");
            lbl_Batchyear.Text = ((!isschool) ? "Batch" : "Year");
            lbldegree.Text = ((!isschool) ? "Degree" : "School Type");
            lblbranch.Text = ((!isschool) ? "Department" : "Standard");
            lblsem.Text = ((!isschool) ? "Semester" : "Term");
            lblsec.Text = ((!isschool) ? "Section" : "Section");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Bind Header

    #region Logout

    protected void logout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    #endregion Logout

    #region DropDown Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Printcontrol1.Visible = false;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            FpSpread1.Visible = false;
            gvChapWiseDmg.Visible = false;
            gvQuesWiseDmg.Visible = false;
            ChartChapterWiseDmg.Visible = false;
            ChartQuesWiseDmg.Visible = false;
            chkShowSelQuestions.Checked = false;
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
            BindTest();
            bindQuestions();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        Printcontrol1.Visible = false;
        rptprint1.Visible = false;
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        FpSpread1.Visible = false;
        gvChapWiseDmg.Visible = false;
        gvQuesWiseDmg.Visible = false;
        ChartChapterWiseDmg.Visible = false;
        ChartQuesWiseDmg.Visible = false;
        chkShowSelQuestions.Checked = false;
        ////btnSave.Visible = false;
        BindDegree();
        bindbranch();
        bindsem();
        BindSectionDetail();
        GetSubject();
        BindTest();
        bindQuestions();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        Printcontrol1.Visible = false;
        rptprint1.Visible = false;
        popupdiv.Visible = false;
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        FpSpread1.Visible = false;
        gvChapWiseDmg.Visible = false;
        gvQuesWiseDmg.Visible = false;
        ChartChapterWiseDmg.Visible = false;
        ChartQuesWiseDmg.Visible = false;
        chkShowSelQuestions.Checked = false;

        // //btnSave.Visible = false;
        bindbranch();
        bindsem();
        BindSectionDetail();
        GetSubject();
        BindTest();
        bindQuestions();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        Printcontrol1.Visible = false;
        rptprint1.Visible = false;
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        FpSpread1.Visible = false;
        gvChapWiseDmg.Visible = false;
        gvQuesWiseDmg.Visible = false;
        ChartChapterWiseDmg.Visible = false;
        ChartQuesWiseDmg.Visible = false;
        chkShowSelQuestions.Checked = false;
        ////btnSave.Visible = false;
        bindsem();
        BindSectionDetail();
        GetSubject();
        BindTest();
        bindQuestions();
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        Printcontrol1.Visible = false;
        rptprint1.Visible = false;
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        FpSpread1.Visible = false;
        gvChapWiseDmg.Visible = false;
        gvQuesWiseDmg.Visible = false;
        ChartChapterWiseDmg.Visible = false;
        ChartQuesWiseDmg.Visible = false;
        chkShowSelQuestions.Checked = false;

        //btnSave.Visible = false;
        BindSectionDetail();
        GetSubject();
        BindTest();
        bindQuestions();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        Printcontrol1.Visible = false;
        rptprint1.Visible = false;
        FpSpread1.Visible = false;
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        gvChapWiseDmg.Visible = false;
        gvQuesWiseDmg.Visible = false;
        ChartChapterWiseDmg.Visible = false;
        ChartQuesWiseDmg.Visible = false;
        chkShowSelQuestions.Checked = false;

        //btnSave.Visible = false;
        GetSubject();
        BindTest();
        bindQuestions();
    }

    protected void ddlsubject_Selectchanged(object sender, EventArgs e)
    {
        Printcontrol1.Visible = false;
        rptprint1.Visible = false;
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        gvChapWiseDmg.Visible = false;
        gvQuesWiseDmg.Visible = false;
        ChartChapterWiseDmg.Visible = false;
        ChartQuesWiseDmg.Visible = false;
        chkShowSelQuestions.Checked = false;

        //btnSave.Visible = false;
        FpSpread1.Visible = false;
        BindTest();
        bindQuestions();


    }

    protected void ddlTest_Selectchanged(object sender, EventArgs e)
    {
        Printcontrol1.Visible = false;
        rptprint1.Visible = false;
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        gvChapWiseDmg.Visible = false;
        gvQuesWiseDmg.Visible = false;
        ChartChapterWiseDmg.Visible = false;
        ChartQuesWiseDmg.Visible = false;
        chkShowSelQuestions.Checked = false;

        FpSpread1.Visible = false;
        bindQuestions();

    }

    protected void cb_Questions_CheckedChanged(object sender, EventArgs e)
    {
        Printcontrol1.Visible = false;
        rptprint1.Visible = false;
        popupdiv.Visible = false;
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        //btnSave.Visible = false;
        FpSpread1.Visible = false;
        gvChapWiseDmg.Visible = false;
        gvQuesWiseDmg.Visible = false;
        ChartChapterWiseDmg.Visible = false;
        ChartQuesWiseDmg.Visible = false;
        int count = 0;
        if (cb_Questions.Checked == true)
        {
            count++;
            for (int i = 0; i < cbl_Questions.Items.Count; i++)
            {
                cbl_Questions.Items[i].Selected = true;
            }
            txt_Questions.Text = "Question (" + (cbl_Questions.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_Questions.Items.Count; i++)
            {
                cbl_Questions.Items[i].Selected = false;
            }
            txt_Questions.Text = "--Select--";
        }
    }

    protected void cbl_Questions_SelectedIndexChanged(object sender, EventArgs e)
    {
        Printcontrol1.Visible = false;
        rptprint1.Visible = false;
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        //btnSave.Visible = false;
        FpSpread1.Visible = false;
        gvChapWiseDmg.Visible = false;
        gvQuesWiseDmg.Visible = false;
        ChartChapterWiseDmg.Visible = false;
        ChartQuesWiseDmg.Visible = false;
        int commcount = 0;
        for (int i = 0; i < cbl_Questions.Items.Count; i++)
        {
            if (cbl_Questions.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_Questions.Items.Count)
            {
                cb_Questions.Checked = true;
            }
            txt_Questions.Text = "Question (" + Convert.ToString(commcount) + ")";
        }
    }

    protected void chkShowSelQuestions_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkShowSelQuestions.Checked)
                ShowQuestions();
        }
        catch (Exception ex)
        {
        }
    }

    #endregion DropDown Events

    #region Button Events

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            int spreadHeight = 0;
            int selQuestionsCount = 0;

            FpSpread1.Visible = false;
            popupdiv.Visible = false;
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            rptprint1.Visible = false;

            Hashtable htChapter = new Hashtable();

            DataRow drNew;

            DataTable dtChapWise = new DataTable();
            DataTable dtQwiseDmg = new DataTable();

            DataTable dtQuestionWiseClassDMG = new DataTable();
            DataTable dtQuestionWiseStudDMG = new DataTable();

            DataTable dtChapterWiseClassDMG = new DataTable();
            DataTable dtChapterWiseStudDMG = new DataTable();

            string secval = string.Empty;
            string qrysec = string.Empty;
            string qryInternal1 = string.Empty;
            string qryInternal2 = string.Empty;
            string qryQues = string.Empty;

            dtQwiseDmg.Columns.Clear();
            dtQwiseDmg.Rows.Clear();
            dtQwiseDmg.Columns.Add("Question_No");
            dtQwiseDmg.Columns.Add("Chapter_No");
            dtQwiseDmg.Columns.Add("Chapters");
            dtQwiseDmg.Columns.Add("QwiseDmg%");

            ChartQuesWiseDmg.Series.Clear();
            ChartQuesWiseDmg.Series.Add("Question_No");

            dtChapWise.Columns.Clear();
            dtChapWise.Rows.Clear();
            dtChapWise.Columns.Add("Chapter_No");
            dtChapWise.Columns.Add("Chapters");
            dtChapWise.Columns.Add("CLASS_DMG%");

            ChartChapterWiseDmg.Series.Clear();
            ChartChapterWiseDmg.Series.Add("Chapters");

            isInternal = true;

            double queswisedmgtotal = 0;
            double queswisedmgavg = 0;

            int startcol = 0;

            if (ddlCollege.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "School" : "College") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }

            if (ddlbatch.Items.Count != 0)
            {
                batch_year = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
            }
            else
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Year" : " Batch") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            if (ddldegree.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "School Type" : "Degree") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlbranch.Items.Count != 0)
            {
                degree_code = Convert.ToString(ddlbranch.SelectedValue).Trim();
            }
            else
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Standard" : "Department") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            if (ddlsem.Items.Count != 0)
            {
                semester = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            }
            else
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Term" : " Semester") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            if (ddlsec.Enabled == false || ddlsec.Items.Count == 0)
            {
                section = "";
                secval = "";
                qrysec = "";
            }
            else if (ddlsec.Items.Count > 0)
            {
                section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                secval = " and qbm.Sections='" + section + "'"; // and qbm.Sections='"+section+"'
                qrysec = " and Sections='" + section + "'";
            }
            if (ddlsubject.Items.Count != 0)
            {
                subject_no = Convert.ToString(ddlsubject.SelectedValue).Trim();
            }
            else
            {
                lblpopuperr.Text = "No Subject were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlTest.Items.Count == 0)
            {
                lblpopuperr.Text = "No Test Were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                test_name = Convert.ToString(ddlTest.SelectedItem.Text).Trim();
                test_no = Convert.ToString(ddlTest.SelectedItem.Value).Trim();
            }

            questionid = "";
            int count = 0;
            if (chkShowSelQuestions.Checked && !chkShowSelQuestions.Checked)
            {
                if (cbl_Questions.Items.Count != 0)
                {
                    for (int i = 0; i < cbl_Questions.Items.Count; i++)
                    {
                        if (cbl_Questions.Items[i].Selected == true)
                        {
                            count++;
                            if (questionid == "")
                            {
                                questionid = "'" + Convert.ToString(cbl_Questions.Items[i].Value).Trim() + "'";
                            }
                            else
                            {
                                questionid = questionid + ",'" + Convert.ToString(cbl_Questions.Items[i].Value).Trim() + "'";
                            }
                        }
                    }
                }
                else
                {
                    lblpopuperr.Text = "No Question were Found";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }

                if (count == 0)
                {
                    lblpopuperr.Text = "Please Select At Least One Question!!!";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }

            if (chkShowSelQuestions.Checked)
            {
                questionid = "";
                FpShowQuestions.SaveChanges();
                selQuestionsCount = 0;
                for (int ques = 1; ques < FpShowQuestions.Sheets[0].RowCount; ques++)
                {
                    int val = 0;
                    string quesId = Convert.ToString(FpShowQuestions.Sheets[0].Cells[ques, 1].Tag).Trim();
                    int.TryParse(Convert.ToString(FpShowQuestions.Sheets[0].Cells[ques, 0].Value).Trim(), out val);
                    if (val == 1)
                    {
                        selQuestionsCount++;
                        if (!string.IsNullOrEmpty(quesId))
                        {
                            if (questionid == "")
                            {
                                questionid = "'" + Convert.ToString(quesId) + "'";
                            }
                            else
                            {
                                questionid = questionid + ",'" + Convert.ToString(quesId) + "'";
                            }
                        }
                    }
                }
                if (FpShowQuestions.Sheets[0].RowCount > 1)
                {
                    if (selQuestionsCount == 0)
                    {
                        lblpopuperr.Text = "Please Select At Least One Question!!!";
                        lblpopuperr.Visible = true;
                        popupdiv.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblpopuperr.Text = "No Questions were Found";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
                if (!string.IsNullOrEmpty(questionid))
                    qryQues = " and exq.QuestionMasterFK in (" + questionid + ")";
                else
                    qryQues = string.Empty;
            }
            else if (!chkShowSelQuestions.Checked)
            {
                qryQues = string.Empty;
            }


            //if (section != "")
            //{
            //qry = "select qbq.Questionentryid,qbq.Questions,qbq.qsection_no,sud.unit_name,qbd.Minimu_Attend,qbd.Total_Questions,qbd.Marks,qbq.Syllabus from tbl_question_bank_master qb,tbl_Question_Bank_details qbd,tbl_Question_Bank_Questions qbq,sub_unit_details sud where sud.subject_no=qbq.Subject_no and Convert(nvarchar(150),sud.topic_no)= Convert(nvarchar(150),qbq.Syllabus) and qb.Questionid=qbd.Questionid and qbq.Questionid=qb.Questionid and qbd.qsection_no=qbq.qsection_no and qb.Subject_no=qbd.Subject_no and qbq.Subject_no=qbd.Subject_no   and qbd.Subject_no='" + subject_no + "' and Degree_Code='" + degree_code + "' and Batch_year='" + batch_year + "'  and Semester='" + semester + "' and qbq.Questionentryid in (" + questionid + ") and Sections='" + section + "'  order by qbq.qsection_no,qbq.Questionentryid ; select Roll_No,Reg_No,Stud_Name from Registration where CC=0 and DelFlag=0 and Exam_Flag<>'debar' and college_code='" + Convert.ToString(Session["collegecode"]) + "' and Batch_Year='" + batch_year + "' and degree_code='" + degree_code + "' and Current_Semester='" + semester + "' and Sections='" + section + "' order by Roll_No  ; select qm.Questionentryid,qbq.Questions,qbq.qsection_no,sud.unit_name,r.serialno,r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,qm.mark_obtained,qbd.Minimu_Attend,qbd.Total_Questions,qbd.Marks from  Registration r,questionwise_marksentry qm,tbl_Question_Bank_Questions qbq,tbl_question_bank_master qbm,tbl_Question_Bank_details qbd,sub_unit_details sud where r.Roll_No=qm.roll_no and r.degree_code=qbm.Degree_Code and qbm.Batch_year=r.Batch_Year and qbm.Semester=r.Current_Semester and qbm.Sections=r.Sections and qm.subject_no=sud.subject_no and sud.subject_no=qbq.Subject_no and qbm.Subject_no=qbq.Subject_no and Convert(nvarchar(150),sud.topic_no)= Convert(nvarchar(150),qbq.Syllabus) and qbd.Questionid=qbm.Questionid and qbd.qSection=qbq.qsection_no and qbd.Subject_no=qm.subject_no and qbq.Questionentryid=qm.Questionentryid and CC=0 and DelFlag=0 and Exam_Flag<>'debar'  and r.college_code='" + Convert.ToString(Session["collegecode"]) + "' and r.Batch_Year='" + batch_year + "' and qm.Questionentryid in (" + questionid + ") and r.degree_code='" + degree_code + "' and r.Current_Semester='" + semester + "' and qm.subject_no='" + subject_no + "' and r.Sections='" + section + "' order by r.Roll_No,qbq.qsection_no,qm.Questionentryid ; select distinct Syllabus,sud.unit_name from  tbl_Question_Bank_Questions qbq,tbl_question_bank_master qbm,tbl_Question_Bank_details qbd,sub_unit_details sud where  qbq.subject_no=sud.subject_no and sud.subject_no=qbq.Subject_no and qbm.Subject_no=qbq.Subject_no and Convert(nvarchar(150),sud.topic_no)= Convert(nvarchar(150),qbq.Syllabus) and qbd.Questionid=qbm.Questionid  and qbd.Questionid=qbq.questionid and qbm.Batch_Year='" + batch_year + "' and qbq.Questionentryid in (" + questionid + ") and qbm.degree_code='" + degree_code + "' and qbm.Semester='" + semester + "' and qbq.subject_no='" + subject_no + "' and qbm.Sections='" + section + "' order by Syllabus ;";
            //}
            //else
            //{
            //    qry = "select qbq.Questionentryid,qbq.Questions,qbq.qsection_no,sud.unit_name,qbd.Minimu_Attend,qbd.Total_Questions,qbd.Marks,qbq.Syllabus from tbl_question_bank_master qb,tbl_Question_Bank_details qbd,tbl_Question_Bank_Questions qbq,sub_unit_details sud where sud.subject_no=qbq.Subject_no and Convert(nvarchar(150),sud.topic_no)= Convert(nvarchar(150),qbq.Syllabus) and qb.Questionid=qbd.Questionid and qbq.Questionid=qb.Questionid and qbd.qsection_no=qbq.qsection_no and qb.Subject_no=qbd.Subject_no and qbq.Subject_no=qbd.Subject_no   and qbd.Subject_no='" + subject_no + "' and Degree_Code='" + degree_code + "' and Batch_year='" + batch_year + "'  and Semester='" + semester + "' and qbq.Questionentryid in (" + questionid + ") order by qbq.qsection_no,qbq.Questionentryid; select Roll_No,Reg_No,Stud_Name from Registration where CC=0 and DelFlag=0 and Exam_Flag<>'debar' and college_code='" + Convert.ToString(Session["collegecode"]) + "' and Batch_Year='" + batch_year + "' and degree_code='" + degree_code + "' and Current_Semester='" + semester + "' order by Roll_No  ; select qm.Questionentryid,qbq.Questions,qbq.qsection_no,sud.unit_name,r.serialno,r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,qm.mark_obtained,qbd.Minimu_Attend,qbd.Total_Questions,qbd.Marks from  Registration r,questionwise_marksentry qm,tbl_Question_Bank_Questions qbq,tbl_question_bank_master qbm,tbl_Question_Bank_details qbd,sub_unit_details sud where r.Roll_No=qm.roll_no and r.degree_code=qbm.Degree_Code and qbm.Batch_year=r.Batch_Year and qbm.Semester=r.Current_Semester and qbm.Sections=r.Sections and qm.subject_no=sud.subject_no and sud.subject_no=qbq.Subject_no and qbm.Subject_no=qbq.Subject_no and Convert(nvarchar(150),sud.topic_no)= Convert(nvarchar(150),qbq.Syllabus) and qbd.Questionid=qbm.Questionid and qbd.qSection=qbq.qsection_no and qbd.Subject_no=qm.subject_no and qbq.Questionentryid=qm.Questionentryid and CC=0 and DelFlag=0 and Exam_Flag<>'debar'  and r.college_code='" + Convert.ToString(Session["collegecode"]) + "' and r.Batch_Year='" + batch_year + "' and qm.Questionentryid in (" + questionid + ") and r.degree_code='" + degree_code + "' and r.Current_Semester='" + semester + "' and qm.subject_no='" + subject_no + "' order by r.Roll_No,qbq.qsection_no,qm.Questionentryid ; select distinct Syllabus,sud.unit_name from  tbl_Question_Bank_Questions qbq,tbl_question_bank_master qbm,tbl_Question_Bank_details qbd,sub_unit_details sud where  qbq.subject_no=sud.subject_no and sud.subject_no=qbq.Subject_no and qbm.Subject_no=qbq.Subject_no and Convert(nvarchar(150),sud.topic_no)= Convert(nvarchar(150),qbq.Syllabus) and qbd.Questionid=qbm.Questionid  and qbd.Questionid=qbq.questionid and qbm.Batch_Year='" + batch_year + "' and qbq.Questionentryid in (" + questionid + ") and qbm.degree_code='" + degree_code + "' and qbm.Semester='" + semester + "' and qbq.subject_no='" + subject_no + "' order by Syllabus ; ";
            //}

            //qry = "select qbq.Questionentryid,qbq.Questions,qbq.qsection_no,sud.unit_name,qbd.Minimu_Attend,qbd.Total_Questions,qbd.Marks,qbq.Syllabus from tbl_question_bank_master qb,tbl_Question_Bank_details qbd,tbl_Question_Bank_Questions qbq,sub_unit_details sud where sud.subject_no=qbq.Subject_no and Convert(nvarchar(150),sud.topic_no)= Convert(nvarchar(150),qbq.Syllabus) and qb.Questionid=qbd.Questionid and qbq.Questionid=qb.Questionid and qbd.qsection_no=qbq.qsection_no and qb.Subject_no=qbd.Subject_no and qbq.Subject_no=qbd.Subject_no and qbd.Subject_no='" + subject_no + "' and Degree_Code='" + degree_code + "' and Batch_year='" + batch_year + "'  and Semester='" + semester + "' and qbq.Questionentryid in (" + questionid + ") " + qrysec + "  order by qbq.qsection_no,qbq.Questionentryid ; select Roll_No,Reg_No,Stud_Name from Registration where CC=0 and DelFlag=0 and Exam_Flag<>'debar' and college_code='" + collegecode + "' and Batch_Year='" + batch_year + "' and degree_code='" + degree_code + "' and Current_Semester='" + semester + "' " + qrysec + "  order by Roll_No ; select qm.Questionentryid,qbq.Questions,qbq.qsection_no,sud.topic_no,sud.unit_name,r.serialno,r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,qm.mark_obtained,qbd.Minimu_Attend,qbd.Total_Questions,qbd.Marks from  Registration r,questionwise_marksentry qm,tbl_Question_Bank_Questions qbq,tbl_question_bank_master qbm,tbl_Question_Bank_details qbd,sub_unit_details sud where r.Roll_No=qm.roll_no and r.degree_code=qbm.Degree_Code and qbm.Batch_year=r.Batch_Year and qbm.Semester=r.Current_Semester and qbm.Sections=r.Sections and qm.subject_no=sud.subject_no and sud.subject_no=qbq.Subject_no and qbm.Subject_no=qbq.Subject_no and Convert(nvarchar(150),sud.topic_no)= Convert(nvarchar(150),qbq.Syllabus) and qbd.Questionid=qbm.Questionid and qbd.qsection_no=qbq.qsection_no and qbd.Subject_no=qm.subject_no and qbq.Questionentryid=qm.Questionentryid and CC=0 and DelFlag=0 and Exam_Flag<>'debar'  and r.college_code='" + collegecode + "' and r.Batch_Year='" + batch_year + "' and qm.Questionentryid in (" + questionid + ") and r.degree_code='" + degree_code + "' and r.Current_Semester='" + semester + "' and qm.subject_no='" + subject_no + "'  " + secval + " order by r.Roll_No,qbq.qsection_no,qm.Questionentryid ; select distinct Syllabus,sud.unit_name from  tbl_Question_Bank_Questions qbq,tbl_question_bank_master qbm,tbl_Question_Bank_details qbd,sub_unit_details sud where  qbq.subject_no=sud.subject_no and sud.subject_no=qbq.Subject_no and qbm.Subject_no=qbq.Subject_no and Convert(nvarchar(150),sud.topic_no)= Convert(nvarchar(150),qbq.Syllabus) and qbd.Questionid=qbm.Questionid  and qbd.Questionid=qbq.questionid and qbm.Batch_Year='" + batch_year + "' and qbq.Questionentryid in (" + questionid + ") and qbm.degree_code='" + degree_code + "' and qbm.Semester='" + semester + "' and qbq.subject_no='" + subject_no + "' " + secval + " order by Syllabus ;";

            if (isInternal)
            {
                qryInternal = " and exq.Test_code=qbm.Exam and exq.Test_code='" + test_no + "' and exq.is_internal='2' " + qryQues;// and exq.QuestionMasterFK in ("+questionid+")";
                qryInternal1 = " and qbm.exam_type=qwm.isinternal and qwm.isinternal=exq.is_internal and Convert(nvarchar(100),qwm.criteria_no)=qbm.Exam and qwm.criteria_no=exq.Test_code and exq.Test_code=qbm.Exam and exq.Test_code='" + test_no + "' and exq.is_internal='2' " + qryQues;// and exq.QuestionMasterFK in (" + questionid + ")";

            }

            qry = "select exq.Test_code,exq.Exist_questionPK,qm.QuestionMasterPK,qm.question,sud.topic_no,exq.Must_attend,sud.unit_name,exq.Section,exq.section_name,qm.mark,qm.subject_no from tbl_question_bank_master qbm,tbl_question_master qm, Exist_questions exq,sub_unit_details sud where qbm.Subject_no=qm.subject_no and qm.subject_no=exq.subject_no and exq.subject_no=qbm.Subject_no and qm.syllabus=exq.syllabus and qbm.exam_type=exq.is_internal and exq.QuestionMasterFK=qm.QuestionMasterPK and Batch_year='" + batch_year + "' and Degree_Code='" + degree_code + "' and Semester='" + semester + "' and qbm.Subject_no='" + subject_no + "' " + qrysec + " and sud.subject_no=qbm.Subject_no and sud.subject_no=qm.subject_no and sud.subject_no=exq.subject_no and sud.topic_no=exq.syllabus and qm.syllabus=sud.topic_no " + qryInternal + "  order by exq.Test_code,exq.subject_no,exq.Exist_questionPK,exq.Section,qm.QuestionMasterPK,sud.topic_no ; select Roll_No,Reg_No,Stud_Name from Registration where CC=0 and DelFlag=0 and Exam_Flag<>'debar' and college_code='" + collegecode + "' and Batch_Year='" + batch_year + "' and degree_code='" + degree_code + "' and Current_Semester='" + semester + "'  " + qrysec + "  order by Roll_No ; select r.serialno,r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,r.Batch_Year,r.degree_code,r.Current_Semester,qbm.Semester,r.Sections,exq.Test_code,exq.subject_no,sud.topic_no,sud.unit_name,exq.is_internal,exq.Exam_month,exq.Exam_year,exq.Exist_questionPK,qm.QuestionMasterPK,qm.question,qm.answer,qm.is_descriptive,qm.is_matching,qm.mark as Max_Mark,qm.qmatching,qm.options,qm.type,exq.Must_attend,exq.Section as Questions_Section,exq.section_name,qwm.mark_obtained,Convert(nvarchar(30),qbm.exam_date,103) as Exam_Date from tbl_question_bank_master qbm,tbl_question_master qm, Exist_questions exq,Registration r,sub_unit_details sud,questionwise_marksentry qwm where qbm.Subject_no=qm.subject_no and qm.subject_no=exq.subject_no and exq.subject_no=qbm.Subject_no and qm.syllabus=exq.syllabus and qbm.exam_type=exq.is_internal and exq.QuestionMasterFK=qm.QuestionMasterPK and r.degree_code=qbm.Degree_Code and r.Batch_Year=qbm.Batch_year and r.Sections=qbm.Sections and r.Roll_No=qwm.roll_no and qwm.subject_no=qm.subject_no and qwm.subject_no=qbm.Subject_no and qwm.Subject_no=exq.subject_no and sud.subject_no=qwm.subject_no and sud.subject_no=qbm.Subject_no and sud.subject_no=qm.subject_no and exq.subject_no=sud.subject_no and sud.topic_no=qm.syllabus and sud.topic_no=exq.syllabus and qwm.Questionentryid=qm.QuestionMasterPK and qwm.Questionentryid=exq.QuestionMasterFK and r.college_code='" + collegecode + "' and qbm.Batch_year='" + batch_year + "' and qbm.Degree_Code='" + degree_code + "' and qbm.Semester='" + semester + "' and qbm.Subject_no='" + subject_no + "' " + secval + " and r.CC=0 and r.Exam_Flag<>'debar' and DelFlag=0 " + qryInternal1 + " order by r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,r.Roll_No,r.Reg_No,exq.Test_code,exq.subject_no,exq.Exist_questionPK,exq.Section,qm.QuestionMasterPK,sud.topic_no ; select distinct sud.topic_no,sud.unit_name from tbl_question_bank_master qbm,tbl_question_master qm, Exist_questions exq,sub_unit_details sud where qbm.Subject_no=qm.subject_no and qm.subject_no=exq.subject_no and exq.subject_no=qbm.Subject_no and qm.syllabus=exq.syllabus and qbm.exam_type=exq.is_internal and exq.QuestionMasterFK=qm.QuestionMasterPK and Batch_year='" + batch_year + "' and Degree_Code='" + degree_code + "' and Semester='" + semester + "' and qbm.Subject_no='" + subject_no + "' " + qrysec + " and sud.subject_no=qbm.Subject_no and sud.subject_no=qm.subject_no and sud.subject_no=exq.subject_no and sud.topic_no=exq.syllabus and qm.syllabus=sud.topic_no " + qryInternal + " order by sud.topic_no ; select ROW_NUMBER() OVER (ORDER BY exq.Exist_questionPK,exq.Section,QuestionMasterPK) as QNo,exq.Exist_questionPK,qm.QuestionMasterPK,qm.question,exq.Section,qm.mark  from tbl_question_bank_master qbm,tbl_question_master qm, Exist_questions exq,sub_unit_details sud where qbm.Subject_no=qm.subject_no and qm.subject_no=exq.subject_no and exq.subject_no=qbm.Subject_no and qm.syllabus=exq.syllabus and qbm.exam_type=exq.is_internal and exq.QuestionMasterFK=qm.QuestionMasterPK and sud.subject_no=qbm.Subject_no and sud.subject_no=qm.subject_no and exq.subject_no=sud.subject_no and sud.topic_no=qm.syllabus and sud.topic_no=exq.syllabus and qbm.Batch_year='" + batch_year + "' and qbm.Degree_Code='" + degree_code + "' and qbm.Semester='" + semester + "' and qbm.Subject_no='" + subject_no + "' " + qrysec + " and exq.Test_code=qbm.Exam and exq.Test_code='" + test_no + "' and exq.is_internal=2 order by exq.Exist_questionPK,exq.Section,qm.QuestionMasterPK ;";
            //-- For Internal
            //and exq.Test_code=qbm.Exam and exq.Test_code='' and exq.is_internal='' and exq.QuestionMasterFK in ()
            //--For External
            //-- and exq.is_internal=1 and qbm.exam_month=exq.Exam_month and exq.Exam_year=qbm.exam_year and qbm.exam_month='February' and qbm.exam_year='2015'

            //-- For Internal Table 3
            //-- and qbm.exam_type=qwm.isinternal and qwm.isinternal=exq.is_internal and qwm.exam_month=qbm.exam_month and qwm.exam_month=exq.Exam_month and Exq.Exam_year=qwm.exam_year and exq.is_internal='' and qbm.exam_month=exq.Exam_month and exq.Exam_year=qbm.exam_year and qbm.exam_month='' and qbm.exam_year=''

            //-- For Internal Table 1
            //--For External
            //-- and exq.is_internal=1 and qbm.exam_month=exq.Exam_month and exq.Exam_year=qbm.exam_year and qbm.exam_month='February' and qbm.exam_year='2015'

            ds.Clear();
            ds.Reset();
            ds.Dispose();

            if (qry != "")
            {
                ds = d2.select_method_wo_parameter(qry, "Text");
            }

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Init_Spread();
                Init_Spread(FpSpread2);
                string newpath = Server.MapPath("~/Image/");
                degree_code = Convert.ToString(ddlbranch.SelectedValue).Trim();
                batch_year = Convert.ToString(ddlbatch.SelectedValue).Trim();
                string current_sem = Convert.ToString(ddlsem.SelectedValue).Trim();
                string branch = Convert.ToString(ddlbranch.SelectedItem).Trim();
                if (ddlsec.Items.Count > 0)
                {
                    if (Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() != "all")
                    {
                        section = "&nbsp;-&nbsp;" + Convert.ToString(ddlsec.SelectedValue).Trim().ToUpper();
                    }
                    else
                    {
                        section = "";
                    }
                }
                string degreedetails = "";
                degreedetails = Convert.ToString(branch).Trim().ToUpper() + "&nbsp;" + section + "&nbsp;(" + ((isSchool) ? "YEAR" : "BATCH") + "&nbsp;" + Convert.ToString(batch_year).Trim() + ")&nbsp;" + ((isSchool) ? "TERM" : "SEM") + "&nbsp;-&nbsp;" + Convert.ToString(current_sem).Trim();


                DataSet dscol = d2.select_method_wo_parameter("Select collname,address1,address2,address3,category,university from Collinfo where college_code='" + collegecode + "' ", "Text");
                if (dscol.Tables[0].Rows.Count > 0)
                {
                    spancollname.Text = Convert.ToString(dscol.Tables[0].Rows[0]["collname"]).Trim();
                    spancollname.Style.Add("text-decoration", "none");
                    spancollname.Style.Add("font-family", "Book Antiqua;");
                    spancollname.Style.Add("font-size", "22px");
                    spancollname.Style.Add("text-align", "center");

                    string address = "";
                    if (Convert.ToString(dscol.Tables[0].Rows[0]["address1"]).Trim() != "")
                    {
                        address = Convert.ToString(dscol.Tables[0].Rows[0]["address1"]).Trim();
                    }
                    if (Convert.ToString(dscol.Tables[0].Rows[0]["address2"]).Trim() != "")
                    {
                        if (address == "")
                        {
                            address = Convert.ToString(dscol.Tables[0].Rows[0]["address2"]).Trim();
                        }
                        else
                        {
                            address = address + ", " + Convert.ToString(dscol.Tables[0].Rows[0]["address2"]).Trim();
                        }
                    }
                    if (Convert.ToString(dscol.Tables[0].Rows[0]["address3"]).Trim() != "")
                    {
                        if (address == "")
                        {
                            address = Convert.ToString(dscol.Tables[0].Rows[0]["address3"]).Trim();
                        }
                        else
                        {
                            address = address + ", " + Convert.ToString(dscol.Tables[0].Rows[0]["address3"]).Trim();
                        }
                    }
                    if (address.Trim() != "")
                    {
                        spanaddr.Text = address.Trim();
                        spanaddr.Style.Add("text-decoration", "none");
                        spanaddr.Style.Add("font-family", "Book Antiqua;");
                        spanaddr.Style.Add("font-size", "20px");
                        spanaddr.Style.Add("text-align", "center");
                    }
                }

                spandegdetails.Text = degreedetails.Trim();
                spandegdetails.Style.Add("text-decoration", "none");
                spandegdetails.Style.Add("font-family", "Book Antiqua;");
                spandegdetails.Style.Add("font-size", "18px");
                spandegdetails.Style.Add("text-align", "center");

                spanTitle.Text = "Chapter And Question Wise Result Analysis Report";
                spanTitle.Style.Add("text-decoration", "none");
                spanTitle.Style.Add("font-family", "Book Antiqua;");
                spanTitle.Style.Add("font-size", "18px");
                spanTitle.Style.Add("text-align", "center");

                spanSub.Text = "Subject Name : " + Convert.ToString(ddlsubject.SelectedItem.Text);
                spanSub.Style.Add("text-decoration", "none");
                spanSub.Style.Add("font-family", "Book Antiqua;");
                spanSub.Style.Add("font-size", "18px");
                spanSub.Style.Add("text-align", "left");

                int totstudent = 0;
                if (ds.Tables.Count >= 2 && ds.Tables[1].Rows.Count > 0)
                {

                    if (ds.Tables.Count >= 3 && ds.Tables[2].Rows.Count > 0 && ds.Tables.Count >= 5 && ds.Tables[4].Rows.Count > 0)
                    {
                        ChapterWiseClassDMG(ds.Tables[3], ds.Tables[1], ds.Tables[2], ref dtChapterWiseClassDMG, ref dtChapterWiseStudDMG);
                        QuestionWiseClassDMG(ds.Tables[4], ds.Tables[0], ds.Tables[1], ds.Tables[2], ref dtQuestionWiseClassDMG, ref dtQuestionWiseStudDMG);
                        ChapterWiseDMG(ds.Tables[3], dtQuestionWiseClassDMG, ref dtChapterWiseClassDMG);
                    }
                    totstudent = ds.Tables[1].Rows.Count;
                    for (int col = 0; col < ds.Tables[1].Rows.Count; col++)
                    {
                        #region Spread1

                        FpSpread1.Sheets[0].ColumnCount += 2;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(ds.Tables[1].Rows[col]["Roll_No"]);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Locked = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;

                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].Locked = true;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;

                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].Resizable = false;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Resizable = false;

                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 2, 1, 2);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Text = "MS";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Locked = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;

                        FpSpread1.Sheets[0].ColumnHeader.Columns[FpSpread1.Sheets[0].ColumnCount - 2].Width = (FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Text.Length) * 25;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "DMG%";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = (FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text.Length) * 15;

                        #endregion Spread1

                        #region Spread2

                        FpSpread2.Sheets[0].ColumnCount += 2;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].Text = Convert.ToString(ds.Tables[1].Rows[col]["Roll_No"]);
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].Locked = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 2].Locked = true;
                        FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Locked = true;

                        FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 2].Resizable = false;
                        FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Resizable = false;

                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 2, 1, 2);
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 2].Text = "MS";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 2].Locked = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].ColumnHeader.Columns[FpSpread2.Sheets[0].ColumnCount - 2].Width = (FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 2].Text.Length) * 25;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = "DMG%";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Locked = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].ColumnHeader.Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = (FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text.Length) * 15;

                        #endregion

                        //FpSpread1.Sheets[0].SetColumnWidth(FpSpread1.Sheets[0].ColumnCount - 1, (FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text.Length) * 10);
                    }
                    //FpSpread1.Sheets[0].ColumnCount++;
                    #region Spread1

                    FpSpread1.Sheets[0].ColumnCount += 2;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(ds.Tables[1].Rows[col]["Roll_No"]);
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].Locked = true;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;

                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].Resizable = false;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Resizable = false;

                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 2, 2, 1);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Text = "Question Wise Total DMG";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;

                    FpSpread1.Sheets[0].ColumnHeader.Columns[FpSpread1.Sheets[0].ColumnCount - 2].Width = (FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Text.Length) * 10;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Question Wise Total DMG%";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].ColumnHeader.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = (FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text.Length) * 10;
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);

                    #endregion Spread1

                    #region Spread2

                    FpSpread2.Sheets[0].ColumnCount += 2;
                    //FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].Text = Convert.ToString(ds.Tables[1].Rows[col]["Roll_No"]);
                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 2].Locked = true;
                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Locked = true;

                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 2].Resizable = false;
                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Resizable = false;

                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 2, 2, 1);
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].Text = "Question Wise Total DMG";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].Locked = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;

                    FpSpread2.Sheets[0].ColumnHeader.Columns[FpSpread2.Sheets[0].ColumnCount - 2].Width = (FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 2].Text.Length) * 10;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Question Wise Total DMG%";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Locked = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    FpSpread2.Sheets[0].ColumnHeader.Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = (FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text.Length) * 10;
                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 1, 2, 1);

                    #endregion

                }
                else
                {
                    gvChapWiseDmg.Visible = false;
                    gvQuesWiseDmg.Visible = false;
                    ChartQuesWiseDmg.Visible = false;
                    ChartChapterWiseDmg.Visible = false;
                    divShowQuestions.Visible = false;
                    rptprint1.Visible = false;
                    FpSpread1.Visible = false;
                    lblpopuperr.Text = "No Students Record(s) Found";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }

                for (int q = 0; q < ds.Tables[0].Rows.Count; q++)
                {
                    startcol = 4;
                    double max_mark = 0;
                    double mark_obtained = 0;
                    double dmg = 0;
                    queswisedmgtotal = 0;
                    queswisedmgavg = 0;
                    drNew = dtQwiseDmg.NewRow();

                    string questionId = Convert.ToString(ds.Tables[0].Rows[q]["QuestionMasterPK"]);
                    string qno = "";
                    if (ds.Tables.Count >= 5 && ds.Tables[4].Rows.Count > 0)
                    {
                        ds.Tables[4].DefaultView.RowFilter = "QuestionMasterPK='" + questionId + "'";
                        DataView dvQues = ds.Tables[4].DefaultView;
                        if (dvQues.Count > 0)
                        {
                            qno = Convert.ToString(dvQues[0]["QNo"]);
                        }
                        else
                        {
                            qno = Convert.ToString((q + 1));
                        }
                    }
                    else
                    {
                        qno = Convert.ToString((q + 1));
                    }

                    #region Spread1

                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString((q + 1));
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[q]["unit_name"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[q]["topic_no"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Locked = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[q]["Questionentryid"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(qno);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = true;

                    spreadHeight += FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Height;

                    #endregion

                    #region Spread 2

                    FpSpread2.Sheets[0].RowCount++;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString((q + 1));
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[q]["unit_name"]);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[q]["topic_no"]);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                    //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[q]["Questionentryid"]);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(qno);

                    #endregion

                    //drNew["Question_No"] = Convert.ToString(ds.Tables[0].Rows[q]["Questionentryid"]);
                    drNew["Question_No"] = Convert.ToString(qno);
                    drNew["Chapter_No"] = Convert.ToString(ds.Tables[0].Rows[q]["topic_no"]);
                    drNew["Chapters"] = Convert.ToString(ds.Tables[0].Rows[q]["unit_name"]);

                    #region Spread1

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[q]["mark"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[q]["mark"]), out max_mark);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                    #endregion Spread1

                    #region Spread2

                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[q]["mark"]);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Locked = true;
                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[q]["mark"]), out max_mark);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                    #endregion Spread2

                    for (int col = 4; col < FpSpread1.Sheets[0].ColumnCount - 2; col += 2)
                    {
                        DataView dvStudMark = new DataView();
                        DataTable dtStuMark = new DataTable();
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            ds.Tables[2].DefaultView.RowFilter = "Roll_No='" + Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Text) + "' and QuestionMasterPK='" + Convert.ToString(ds.Tables[0].Rows[q]["QuestionMasterPK"]) + "'";
                            dvStudMark = ds.Tables[2].DefaultView;
                            dtStuMark = dvStudMark.ToTable();
                        }
                        if (dtStuMark.Rows.Count > 0)
                        {

                            #region Spread 1

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, startcol].Text = Convert.ToString(dtStuMark.Rows[0]["mark_obtained"]);
                            double.TryParse(Convert.ToString(dtStuMark.Rows[0]["mark_obtained"]), out mark_obtained);
                            dmg = ((max_mark - mark_obtained) / max_mark) * 100;
                            dmg = Math.Round(dmg, 0, MidpointRounding.AwayFromZero);//,MidpointRounding.AwayFromZero
                            queswisedmgtotal += dmg;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, startcol].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, startcol].Locked = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, startcol].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, startcol].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, startcol + 1].Text = Convert.ToString(dmg);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, startcol + 1].Locked = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, startcol + 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, startcol + 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, startcol + 1].VerticalAlign = VerticalAlign.Middle;

                            #endregion Spread 1

                            #region Spread 2

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, startcol].Text = Convert.ToString(dtStuMark.Rows[0]["mark_obtained"]);
                            //double.TryParse(Convert.ToString(dtStuMark.Rows[0]["mark_obtained"]), out mark_obtained);
                            //dmg = ((max_mark - mark_obtained) / max_mark) * 100;
                            //dmg = Math.Round(dmg, 0, MidpointRounding.AwayFromZero);//,MidpointRounding.AwayFromZero
                            //queswisedmgtotal += dmg;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, startcol].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, startcol].Locked = true;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, startcol].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, startcol].VerticalAlign = VerticalAlign.Middle;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, startcol + 1].Text = Convert.ToString(dmg);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, startcol + 1].Locked = true;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, startcol + 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, startcol + 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, startcol + 1].VerticalAlign = VerticalAlign.Middle;

                            #endregion Spread 2

                        }
                        else
                        {

                        }
                        startcol += 2;
                    }
                    queswisedmgavg = Math.Round((queswisedmgtotal / totstudent), 0, MidpointRounding.AwayFromZero);

                    #region Spread 1

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(queswisedmgtotal);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Locked = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(queswisedmgavg);
                    drNew["QwiseDmg%"] = Convert.ToString(queswisedmgavg);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    dtQwiseDmg.Rows.Add(drNew);

                    #endregion Spread 1

                    #region Spread 2

                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, FpSpread2.Sheets[0].ColumnCount - 2].Text = Convert.ToString(queswisedmgtotal);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, FpSpread2.Sheets[0].ColumnCount - 2].Locked = true;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, FpSpread2.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, FpSpread2.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, FpSpread2.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;

                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(queswisedmgavg);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, FpSpread2.Sheets[0].ColumnCount - 1].Locked = true;
                    //drNew["QwiseDmg%"] = Convert.ToString(queswisedmgavg);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, FpSpread2.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    //dtQwiseDmg.Rows.Add(drNew);

                    #endregion Spread 2
                }

                #region Spread 1

                FpSpread1.Sheets[0].RowCount++;
                int totrow = FpSpread1.Sheets[0].RowCount - 1;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Total";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Locked = true;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].SpanModel.Add(totrow, 1, 1, 2);

                spreadHeight += FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Height;

                #endregion Spread 1

                #region Spread 2

                FpSpread2.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Total";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Locked = true;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].SpanModel.Add(totrow, 1, 1, 2);

                #endregion Spread 2

                if (ds.Tables[3].Rows.Count > 0)
                {
                    //htChapter.Clear();
                    for (int chap = 0; chap < ds.Tables[3].Rows.Count; chap++)
                    {
                        #region Spread 1

                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[3].Rows[chap][1]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[3].Rows[chap][0]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, 2);

                        spreadHeight += FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Height;

                        #endregion Spread 1

                        #region Spread 2

                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[3].Rows[chap][1]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[3].Rows[chap][0]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 1, 1, 2);

                        #endregion Spread 2
                        //htChapter.Add(ds.Tables[3].Rows[chap]["Syllabus"], ds.Tables[3].Rows[chap]["unit_name"]);
                    }
                }
                for (int col = 3; col < FpSpread1.Sheets[0].ColumnCount - 2; )
                {
                    double total = 0;
                    double mrks = 0;
                    double chapwisetotal = 0;
                    Hashtable htChapCount = new Hashtable();
                    htChapter.Clear();
                    for (int row = 0; row < totrow; row++)
                    {
                        double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[row, col].Text), out mrks);
                        total += mrks;
                        //chapwisetotal = 0;
                        if (col != 3)
                        {
                            if (htChapter.ContainsKey(Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag)))
                            {
                                double already = 0;
                                int chapcount = 0;
                                int.TryParse(Convert.ToString(htChapCount[Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag)]), out chapcount);
                                double.TryParse(Convert.ToString(htChapter[Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag)]), out already);
                                chapcount += 1;
                                chapwisetotal = already + mrks;
                                htChapCount[Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag)] = chapcount;
                                htChapter[Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag)] = chapwisetotal;
                            }
                            else
                            {
                                //double already = 0;
                                //double.TryParse("0", out already);
                                htChapter.Add(Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag), mrks);
                                htChapCount.Add(Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag), 1);
                            }
                        }
                    }

                    #region Spread 1

                    FpSpread1.Sheets[0].Cells[totrow, col].Text = Convert.ToString(Math.Round(total, 1, MidpointRounding.AwayFromZero));
                    FpSpread1.Sheets[0].Cells[totrow, col].Locked = true;
                    FpSpread1.Sheets[0].Cells[totrow, col].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[totrow, col].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[totrow, col].VerticalAlign = VerticalAlign.Middle;

                    #endregion Spread 1


                    #region Spread 2

                    FpSpread2.Sheets[0].Cells[totrow, col].Text = Convert.ToString(Math.Round(total, 1, MidpointRounding.AwayFromZero));
                    FpSpread2.Sheets[0].Cells[totrow, col].Locked = true;
                    FpSpread2.Sheets[0].Cells[totrow, col].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[totrow, col].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[totrow, col].VerticalAlign = VerticalAlign.Middle;

                    #endregion Spread 2

                    for (int chap = 0; chap < ds.Tables[3].Rows.Count; chap++)
                    {
                        if (htChapter.ContainsKey(Convert.ToString(ds.Tables[3].Rows[chap][0])))
                        {
                            double chapmrk = 0;
                            int chap_cnt = 0;
                            double.TryParse(Convert.ToString(htChapter[Convert.ToString(ds.Tables[3].Rows[chap][0])]), out chapmrk);
                            int.TryParse(Convert.ToString(htChapCount[Convert.ToString(ds.Tables[3].Rows[chap][0])]), out chap_cnt);
                            #region Spread 1

                            FpSpread1.Sheets[0].Cells[totrow + (chap + 1), col].Text = Convert.ToString(Math.Round((chapmrk / chap_cnt), 0, MidpointRounding.AwayFromZero));
                            FpSpread1.Sheets[0].Cells[totrow + (chap + 1), col].Locked = true;
                            FpSpread1.Sheets[0].Cells[totrow + (chap + 1), col].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[totrow + (chap + 1), col].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[totrow + (chap + 1), col].VerticalAlign = VerticalAlign.Middle;

                            #endregion Spread 1

                            #region Spread 2

                            FpSpread2.Sheets[0].Cells[totrow + (chap + 1), col].Text = Convert.ToString(Math.Round((chapmrk / chap_cnt), 0, MidpointRounding.AwayFromZero));
                            FpSpread2.Sheets[0].Cells[totrow + (chap + 1), col].Locked = true;
                            FpSpread2.Sheets[0].Cells[totrow + (chap + 1), col].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[totrow + (chap + 1), col].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[totrow + (chap + 1), col].VerticalAlign = VerticalAlign.Middle;

                            #endregion Spread 2
                        }
                    }
                    if (col == 3)
                    {
                        col++;
                    }
                    else
                    {
                        col += 2;
                    }
                }
                if (dtQwiseDmg.Rows.Count > 0)
                {
                    gvQuesWiseDmg.DataSource = dtQwiseDmg;
                    gvQuesWiseDmg.DataBind();
                    gvQuesWiseDmg.Visible = true;
                    if (gvQuesWiseDmg.HeaderRow.Cells.Count > 0)
                    {
                        for (int headerRows = 0; headerRows < gvQuesWiseDmg.HeaderRow.Cells.Count; headerRows++)
                        {
                            string headerValues = gvQuesWiseDmg.HeaderRow.Cells[headerRows].Text;
                            gvQuesWiseDmg.HeaderRow.Cells[headerRows].BackColor = ColorTranslator.FromHtml("#00aff0");
                            gvQuesWiseDmg.HeaderRow.Cells[headerRows].ForeColor = System.Drawing.Color.Black;
                            gvQuesWiseDmg.HeaderRow.Cells[headerRows].BorderColor = System.Drawing.Color.Black;
                            gvQuesWiseDmg.HeaderRow.Cells[headerRows].Wrap = true;
                            gvQuesWiseDmg.HeaderRow.Cells[headerRows].Width = headerValues.Length * 10 + 20;
                            if (headerRows == 1)
                            {
                                gvQuesWiseDmg.HeaderRow.Cells[headerRows].Visible = false;
                            }
                        }
                    }
                    double total = 0;
                    double mrks = 0;
                    double chapwisetotal = 0;
                    Hashtable htChapCount = new Hashtable();
                    htChapter.Clear();

                    ChartQuesWiseDmg.RenderType = RenderType.ImageTag;
                    ChartQuesWiseDmg.ImageType = ChartImageType.Png;
                    ChartQuesWiseDmg.ImageStorageMode = ImageStorageMode.UseImageLocation;


                    //string imgPath = HttpContext.Current.Request.PhysicalApplicationPath + "image\\" + "chartChapterDMG" + index;
                    ChartQuesWiseDmg.ImageLocation = Path.Combine("~/Image/", "chartQuestionWiseDMG");

                    for (int r = 0; r < dtQwiseDmg.Rows.Count; r++)
                    {

                        //for (int c = 0; c < dtQwiseDmg.Columns.Count; c++)
                        //{
                        //if (c == 0)
                        //{
                        ChartQuesWiseDmg.Series[0].Points.AddXY(Convert.ToString(dtQwiseDmg.Rows[r]["Question_No"]), Convert.ToString(dtQwiseDmg.Rows[r]["QwiseDmg%"]));
                        ChartQuesWiseDmg.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                        ChartQuesWiseDmg.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                        ChartQuesWiseDmg.Series[0].IsValueShownAsLabel = true;
                        ChartQuesWiseDmg.Series[0].IsXValueIndexed = true;

                        ChartQuesWiseDmg.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                        ChartQuesWiseDmg.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                        ChartQuesWiseDmg.ChartAreas[0].AxisY.Maximum = 100;
                        ChartQuesWiseDmg.ChartAreas[0].AxisY.Minimum = 0;
                        //ChartQuesWiseDmg.ChartAreas[0].AxisY.IntervalOffset = 10;
                        //}
                        double.TryParse(Convert.ToString(dtQwiseDmg.Rows[r]["QwiseDmg%"]), out mrks);
                        if (htChapter.ContainsKey(Convert.ToString(dtQwiseDmg.Rows[r][1])))
                        {
                            double already = 0;
                            int chapcount = 0;
                            int.TryParse(Convert.ToString(htChapCount[Convert.ToString(dtQwiseDmg.Rows[r][1])]), out chapcount);
                            double.TryParse(Convert.ToString(htChapter[Convert.ToString(dtQwiseDmg.Rows[r][1])]), out already);
                            chapcount += 1;
                            chapwisetotal = already + mrks;
                            htChapCount[Convert.ToString(dtQwiseDmg.Rows[r][1])] = chapcount;
                            htChapter[Convert.ToString(dtQwiseDmg.Rows[r][1])] = chapwisetotal;
                        }
                        else
                        {
                            //double already = 0;
                            //double.TryParse("0", out already);
                            htChapter.Add(Convert.ToString(dtQwiseDmg.Rows[r][1]), mrks);
                            htChapCount.Add(Convert.ToString(dtQwiseDmg.Rows[r][1]), 1);
                        }
                        //if (r != 0)
                        //gvQuesWiseDmg.Rows[r].Cells[c].HorizontalAlign = HorizontalAlign.Center;
                        //}
                    }
                    for (int chap = 0; chap < ds.Tables[3].Rows.Count; chap++)
                    {
                        drNew = dtChapWise.NewRow();
                        if (htChapter.ContainsKey(Convert.ToString(ds.Tables[3].Rows[chap][0])))
                        {
                            double chapmrk = 0;
                            int chap_cnt = 0;
                            double.TryParse(Convert.ToString(htChapter[Convert.ToString(ds.Tables[3].Rows[chap][0])]), out chapmrk);
                            int.TryParse(Convert.ToString(htChapCount[Convert.ToString(ds.Tables[3].Rows[chap][0])]), out chap_cnt);
                            drNew["Chapters"] = Convert.ToString(ds.Tables[3].Rows[chap][1]);
                            drNew["Chapter_No"] = Convert.ToString(ds.Tables[3].Rows[chap][0]);
                            drNew["CLASS_DMG%"] = Convert.ToString(Math.Round((chapmrk / chap_cnt), 0, MidpointRounding.AwayFromZero));
                        }
                        dtChapWise.Rows.Add(drNew);
                    }
                    ChartQuesWiseDmg.Visible = true;
                    //ChartQuesWiseDmg.Ser
                    //string imgPath = HttpContext.Current.Request.PhysicalApplicationPath + "charts_0\\QueswiseDmg.png";
                    ////ChartQuesWiseDmg.Width = (dtQwiseDmg.Rows.Count * 40) + 100;
                    //ChartQuesWiseDmg.SaveImage(imgPath);
                }
                if (dtChapWise.Rows.Count > 0)
                {
                    gvChapWiseDmg.DataSource = dtChapWise;
                    gvChapWiseDmg.DataBind();
                    gvChapWiseDmg.Visible = true;

                    if (gvChapWiseDmg.HeaderRow.Cells.Count > 0)
                    {
                        for (int headerRows = 0; headerRows < gvChapWiseDmg.HeaderRow.Cells.Count; headerRows++)
                        {
                            string headerValues = gvChapWiseDmg.HeaderRow.Cells[headerRows].Text;
                            gvChapWiseDmg.HeaderRow.Cells[headerRows].BackColor = ColorTranslator.FromHtml("#00aff0");
                            gvChapWiseDmg.HeaderRow.Cells[headerRows].ForeColor = System.Drawing.Color.Black;
                            gvChapWiseDmg.HeaderRow.Cells[headerRows].BorderColor = System.Drawing.Color.Black;
                            gvChapWiseDmg.HeaderRow.Cells[headerRows].Wrap = true;
                            gvChapWiseDmg.HeaderRow.Cells[headerRows].Width = headerValues.Length * 10 + 20;
                            if (headerRows == 0)
                            {
                                gvChapWiseDmg.HeaderRow.Cells[headerRows].Visible = false;
                            }
                        }
                    }

                    ChartChapterWiseDmg.RenderType = RenderType.ImageTag;
                    ChartChapterWiseDmg.ImageType = ChartImageType.Png;
                    ChartChapterWiseDmg.ImageStorageMode = ImageStorageMode.UseImageLocation;


                    //string imgPath = HttpContext.Current.Request.PhysicalApplicationPath + "image\\" + "chartChapterDMG" + index;
                    ChartChapterWiseDmg.ImageLocation = Path.Combine("~/Image/", "chartChapterWiseDMG");
                    for (int r = 0; r < dtChapWise.Rows.Count; r++)
                    {
                        ChartChapterWiseDmg.Series[0].Points.AddXY(Convert.ToString(dtChapWise.Rows[r]["Chapters"]), Convert.ToString(dtChapWise.Rows[r]["CLASS_DMG%"]));
                        ChartChapterWiseDmg.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                        ChartChapterWiseDmg.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                        ChartChapterWiseDmg.Series[0].IsValueShownAsLabel = true;
                        ChartChapterWiseDmg.Series[0].IsXValueIndexed = true;

                        ChartChapterWiseDmg.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                        ChartChapterWiseDmg.ChartAreas[0].AxisX.LabelStyle.Interval = 1;

                        ChartChapterWiseDmg.ChartAreas[0].AxisY.Interval = 5;
                    }
                    ChartChapterWiseDmg.Visible = true;
                    //string imgPath = HttpContext.Current.Request.PhysicalApplicationPath + "charts_0\\ChapterWiseDmg.png";
                    ////ChartChapterWiseDmg.Width = (dtChapWise.Rows.Count * 60) + 100;
                    //ChartChapterWiseDmg.SaveImage(imgPath);
                }

                #region Spread 1

                for (int sh = 0; sh < FpSpread1.Sheets[0].ColumnHeader.RowCount; sh++)
                {
                    spreadHeight += FpSpread1.Sheets[0].ColumnHeader.Rows[sh].Height;
                }

                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.Width = 900;
                FpSpread1.Height = (spreadHeight) + 45;
                FpSpread1.SaveChanges();
                FpSpread1.Visible = true;

                #endregion Spread 1

                #region Spread 2

                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                //FpSpread2.Width = 900;
                FpSpread2.Height = (spreadHeight) + 45; //(FpSpread2.Sheets[0].RowCount * 27) + 45;//(spreadHeight) + 45;
                FpSpread2.SaveChanges();
                FpSpread2.Visible = true;
                divShowQuestions.Visible = false;

                #endregion Spread 2

                rptprint1.Visible = true;
                lblpopuperr.Visible = false;
                popupdiv.Visible = false;
            }
            else
            {
                gvChapWiseDmg.Visible = false;
                gvQuesWiseDmg.Visible = false;
                ChartQuesWiseDmg.Visible = false;
                ChartChapterWiseDmg.Visible = false;
                rptprint1.Visible = false;
                FpSpread1.Visible = false;
                lblpopuperr.Text = "No Record(s) Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            //lblpopuperr.Text =  Convert.ToString(ex);
            //lblpopuperr.Visible = true;
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname1.Text;
            if (Convert.ToString(reportname).Trim() != "")
            {
                lbl_norec1.Visible = false;
                string degree_code = Convert.ToString(ddlbranch.SelectedValue);
                string batch_year = Convert.ToString(ddlbatch.SelectedValue);
                string current_sem = Convert.ToString(ddlsem.SelectedValue);
                string branch = Convert.ToString(ddlbranch.SelectedItem);
                if (ddlsec.Items.Count > 0)
                {
                    if (ddlsec.SelectedItem.Text != "ALL")
                    {
                        section = "&nbsp;-&nbsp;" + Convert.ToString(ddlsec.SelectedValue).ToUpper();
                    }
                    else
                    {
                        section = "";
                    }
                }

                string degreedetails = "";
                reportname = reportname.Trim() + "_Chapter_And_Question_Wise_Result_Analysis_Report";
                degreedetails = branch.ToUpper() + "&nbsp;" + section + "&nbsp;(" + ((isSchool) ? "YEAR" : "BATCH") + "&nbsp;" + Convert.ToString(batch_year) + ")&nbsp;" + ((isSchool) ? "TERM" : "SEM") + "&nbsp;-&nbsp;" + Convert.ToString(current_sem);
                Response.ClearContent();
                Response.AddHeader("content-disposition",
                    "attachment;filename=" + reportname.Replace(" ", "_").Trim() + ".xls");
                Response.ContentType = "application/excel";
                StringWriter sw = new StringWriter(); ;
                HtmlTextWriter htm = new HtmlTextWriter(sw);

                DataSet dscol = d2.select_method_wo_parameter("Select collname,address1,address2,address3,category,university from Collinfo where college_code='" + Convert.ToString(ddlCollege.SelectedValue) + "' ", "Text");
                Label lb = new Label();
                htm.InnerWriter.WriteLine("<center>");
                if (dscol.Tables[0].Rows.Count > 0)
                {
                    lb.Text = Convert.ToString(dscol.Tables[0].Rows[0]["collname"]) + "<br> ";
                    lb.Style.Add("height", "100px");
                    lb.Style.Add("text-decoration", "none");
                    lb.Style.Add("font-family", "Book Antiqua;");
                    lb.Style.Add("font-size", "18px");
                    lb.Style.Add("font-weight", "bold");
                    lb.Style.Add("text-align", "center");
                    lb.RenderControl(htm);

                    string address = "";
                    if (Convert.ToString(dscol.Tables[0].Rows[0]["address1"]).Trim() != "")
                    {
                        address = Convert.ToString(dscol.Tables[0].Rows[0]["address1"]);
                    }
                    if (Convert.ToString(dscol.Tables[0].Rows[0]["address2"]).Trim() != "")
                    {
                        if (address == "")
                        {
                            address = Convert.ToString(dscol.Tables[0].Rows[0]["address2"]);
                        }
                        else
                        {
                            address = address + ", " + Convert.ToString(dscol.Tables[0].Rows[0]["address2"]);
                        }
                    }
                    if (Convert.ToString(dscol.Tables[0].Rows[0]["address3"]).Trim() != "")
                    {
                        if (address == "")
                        {
                            address = Convert.ToString(dscol.Tables[0].Rows[0]["address3"]);
                        }
                        else
                        {
                            address = address + ", " + Convert.ToString(dscol.Tables[0].Rows[0]["address3"]);
                        }
                    }
                    if (address.Trim() != "")
                    {
                        lb.Text = address + "<br> ";
                        lb.Style.Add("height", "100px");
                        lb.Style.Add("text-decoration", "none");
                        lb.Style.Add("font-family", "Book Antiqua;");
                        lb.Style.Add("font-size", "12px");
                        lb.Style.Add("text-align", "center");
                        lb.RenderControl(htm);
                    }
                }
                Label lb2 = new Label();
                lb2.Text = degreedetails;
                lb2.Style.Add("height", "100px");
                lb2.Style.Add("text-decoration", "none");
                lb2.Style.Add("font-family", "Book Antiqua;");
                lb2.Style.Add("font-size", "10px");
                lb2.Style.Add("font-weight", "bold");
                lb2.Style.Add("text-align", "center");
                lb2.RenderControl(htm);
                Label lb3 = new Label();
                lb3.Text = "<br>";
                lb3.Style.Add("height", "200px");
                lb3.Style.Add("text-decoration", "none");
                lb3.Style.Add("font-family", "Book Antiqua;");
                lb3.Style.Add("font-size", "10px");
                lb3.Style.Add("text-align", "left");
                lb3.RenderControl(htm);
                Label lb4 = new Label();
                lb4.Text = "Chapter And Question Wise Result Analysis Report<br><br>";
                lb4.Style.Add("height", "200px");
                lb4.Style.Add("font-weight", "bold");
                lb4.Style.Add("text-decoration", "none");
                lb4.Style.Add("font-family", "Book Antiqua;");
                lb4.Style.Add("font-size", "10px");
                lb4.Style.Add("text-align", "center");
                lb4.RenderControl(htm);

                htm.InnerWriter.WriteLine("</center>");

                lb4.Text = "Subject Name : " + Convert.ToString(ddlsubject.SelectedItem.Text) + " <br><br/>";
                lb4.Style.Add("height", "200px");
                lb4.Style.Add("text-decoration", "none");
                lb4.Style.Add("font-family", "Book Antiqua;");
                lb4.Style.Add("font-size", "10px");
                lb4.Style.Add("font-weight", "bold");
                lb4.Style.Add("text-align", "left");
                lb4.RenderControl(htm);

                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    FpSpread1.Visible = true;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
                    darkstyle.Font.Name = "Book Antiqua";
                    darkstyle.Font.Size = FontUnit.Medium;
                    darkstyle.Font.Bold = true;
                    darkstyle.HorizontalAlign = HorizontalAlign.Center;
                    darkstyle.VerticalAlign = VerticalAlign.Middle;
                    darkstyle.ForeColor = System.Drawing.Color.White;
                    darkstyle.Border.BorderSize = 1;
                    darkstyle.Border.BorderColor = System.Drawing.Color.Black;

                    FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
                    sheetstyle.Font.Name = "Book Antiqua";
                    sheetstyle.Font.Size = FontUnit.Medium;
                    sheetstyle.Font.Bold = true;
                    sheetstyle.HorizontalAlign = HorizontalAlign.Center;
                    sheetstyle.VerticalAlign = VerticalAlign.Middle;
                    sheetstyle.ForeColor = System.Drawing.Color.Black;
                    sheetstyle.Border.BorderSize = 1;
                    sheetstyle.Border.BorderColor = System.Drawing.Color.Black;
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    FpSpread1.Sheets[0].DefaultStyle = sheetstyle;
                    FpSpread1.RenderControl(htm);
                }

                lb2 = new Label();
                lb2.Text = "<br/><br/><br/>Students Question Wise Damage Analysis <br/>";
                lb2.Style.Add("height", "100px");
                lb2.Style.Add("text-decoration", "none");
                lb2.Style.Add("font-family", "Book Antiqua;");
                lb2.Style.Add("font-size", "10px");
                lb2.Style.Add("font-weight", "bold");
                lb2.Style.Add("text-align", "center");
                lb2.RenderControl(htm);

                btngo_Click(sender, e);

                htm.InnerWriter.WriteLine("<br/><center>");
                gvQuesWiseDmg.RenderControl(htm);
                htm.InnerWriter.WriteLine("</center><br/>");

                lb2 = new Label();
                lb2.Text = "<br/><br/><br/><br/><br/><br/>";
                lb2.Style.Add("height", "100px");
                lb2.Style.Add("text-decoration", "none");
                lb2.Style.Add("font-family", "Book Antiqua;");
                lb2.Style.Add("font-size", "10px");
                lb2.Style.Add("font-weight", "bold");
                lb2.Style.Add("text-align", "center");
                lb2.RenderControl(htm);

                lb2 = new Label();
                lb2.Text = "<br/><br/><br/><br/><br/><br/>";
                lb2.Style.Add("height", "100px");
                lb2.Style.Add("text-decoration", "none");
                lb2.Style.Add("font-family", "Book Antiqua;");
                lb2.Style.Add("font-size", "10px");
                lb2.Style.Add("font-weight", "bold");
                lb2.Style.Add("text-align", "center");
                lb2.RenderControl(htm);


                htm.InnerWriter.WriteLine("<br/><br/><b><span style='font-family:Book Antiqua; font-size:10px;font-weight:bold; text-align:center; '>Student Chapter Wise Damage Analysis</span><br/><br/></b><center>");
                gvChapWiseDmg.RenderControl(htm);
                htm.InnerWriter.WriteLine("</center><br/><br/>");

                lb2 = new Label();
                lb2.Text = "<br/><br/>";
                lb2.Style.Add("height", "100px");
                lb2.Style.Add("text-decoration", "none");
                lb2.Style.Add("font-family", "Book Antiqua;");
                lb2.Style.Add("font-size", "10px");
                lb2.Style.Add("text-align", "center");
                lb2.RenderControl(htm);

                lb2 = new Label();
                lb2.Text = "<br/><br/>";
                lb2.Style.Add("height", "100px");
                lb2.Style.Add("text-decoration", "none");
                lb2.Style.Add("font-family", "Book Antiqua;");
                lb2.Style.Add("font-size", "10px");
                lb2.Style.Add("text-align", "center");
                lb2.RenderControl(htm);
                htm.InnerWriter.WriteLine("</center>");
                Response.Write(Convert.ToString(sw));
                Response.End();
                Response.Clear();
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    string dptname = "Question Wise Mark Entry";
        //    string pagename = "Question_Wise_Mark_Entry.aspx";
        //    dptname = dptname + "@ " + "Subject : " +  Convert.ToString(ddlsubject.SelectedItem);
        //    if (FpSpread1.Visible == true)
        //    {
        //        Printcontrol1.loadspreaddetails(FpSpread1, pagename, dptname);
        //    }
        //    Printcontrol1.Visible = true;
        //    lbl_norec1.Visible = false;
        //}
        //catch
        //{
        //}
        try
        {
            string degree_code = Convert.ToString(ddlbranch.SelectedValue);
            string batch_year = Convert.ToString(ddlbatch.SelectedValue);
            string current_sem = Convert.ToString(ddlsem.SelectedValue);
            string branch = Convert.ToString(ddlbranch.SelectedItem);
            if (ddlsec.Items.Count > 0)
            {
                if (ddlsec.SelectedItem.Text != "ALL")
                {
                    section = "&nbsp;-&nbsp;" + Convert.ToString(ddlsec.SelectedValue).ToUpper();
                }
                else
                {
                    section = "";
                }
            }
            string degreedetails = "";
            degreedetails = branch.ToUpper() + "&nbsp;" + section + "&nbsp;(" + ((isSchool) ? "YEAR" : "BATCH") + "&nbsp;" + Convert.ToString(batch_year) + ")&nbsp;" + ((isSchool) ? "TERM" : "SEM") + "&nbsp;-&nbsp;" + Convert.ToString(current_sem);//Convert.ToString(ddlDegree.SelectedItem).ToUpper() + "&nbsp;-&nbsp;" + 
            btngo_Click(sender, e);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Chapter_And_Question_Wise_Result_Analysis_Report.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Document pdfDoc = new Document(PageSize.B4, 10f, 10f, 5f, 0f);
            pdfDoc.SetPageSize(iTextSharp.text.PageSize.B4.Rotate());
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            Label lb = new Label();

            string collegename = "";

            DataSet dscol = d2.select_method_wo_parameter("Select collname,address1,address2,address3,category,university from Collinfo where college_code='" + Convert.ToString(ddlCollege.SelectedValue) + "' ", "Text");
            if (dscol.Tables[0].Rows.Count > 0)
            {
                lb.Text = Convert.ToString(dscol.Tables[0].Rows[0]["collname"]) + "<br> ";
                lb.Style.Add("height", "100px");
                lb.Style.Add("text-decoration", "none");
                lb.Style.Add("font-family", "Book Antiqua;");
                lb.Style.Add("font-size", "18px");
                lb.Style.Add("text-align", "center");
                lb.RenderControl(hw);

                string address = "";
                if (Convert.ToString(dscol.Tables[0].Rows[0]["address1"]).Trim() != "")
                {
                    address = Convert.ToString(dscol.Tables[0].Rows[0]["address1"]);
                }
                if (Convert.ToString(dscol.Tables[0].Rows[0]["address2"]).Trim() != "")
                {
                    if (address == "")
                    {
                        address = Convert.ToString(dscol.Tables[0].Rows[0]["address2"]);
                    }
                    else
                    {
                        address = address + ", " + Convert.ToString(dscol.Tables[0].Rows[0]["address2"]);
                    }
                }
                if (Convert.ToString(dscol.Tables[0].Rows[0]["address3"]).Trim() != "")
                {
                    if (address == "")
                    {
                        address = Convert.ToString(dscol.Tables[0].Rows[0]["address3"]);
                    }
                    else
                    {
                        address = address + ", " + Convert.ToString(dscol.Tables[0].Rows[0]["address3"]);
                    }
                }
                if (address.Trim() != "")
                {
                    lb.Text = address + "<br> ";
                    lb.Style.Add("height", "100px");
                    lb.Style.Add("text-decoration", "none");
                    lb.Style.Add("font-family", "Book Antiqua;");
                    lb.Style.Add("font-size", "12px");
                    lb.Style.Add("text-align", "center");
                    lb.RenderControl(hw);
                }
            }

            Label lb2 = new Label();
            lb2.Text = degreedetails;
            lb2.Style.Add("height", "100px");
            lb2.Style.Add("text-decoration", "none");
            lb2.Style.Add("font-family", "Book Antiqua;");
            lb2.Style.Add("font-size", "10px");
            lb2.Style.Add("text-align", "center");
            lb2.RenderControl(hw);

            Label lb3 = new Label();
            lb3.Text = "<br>";
            lb3.Style.Add("height", "200px");
            lb3.Style.Add("text-decoration", "none");
            lb3.Style.Add("font-family", "Book Antiqua;");
            lb3.Style.Add("font-size", "10px");
            lb3.Style.Add("text-align", "left");
            lb3.RenderControl(hw);

            Label lb4 = new Label();
            lb4.Text = "Chapter And Question Wise Result Analysis Report<br><br>";
            lb4.Style.Add("height", "200px");
            lb4.Style.Add("text-decoration", "none");
            lb4.Style.Add("font-family", "Book Antiqua;");
            lb4.Style.Add("font-size", "10px");
            lb4.Style.Add("text-align", "center");
            lb4.RenderControl(hw);

            StringWriter sw00 = new StringWriter();
            HtmlTextWriter hw00 = new HtmlTextWriter(sw00);

            lb4.Text = "Subject Name : " + Convert.ToString(ddlsubject.SelectedItem.Text) + " <br><br/>";
            lb4.Style.Add("height", "200px");
            lb4.Style.Add("text-decoration", "none");
            lb4.Style.Add("font-family", "Book Antiqua;");
            lb4.Style.Add("font-size", "10px");
            lb4.Style.Add("text-align", "left");
            lb4.RenderControl(hw00);

            //if (FpSpread1.Sheets[0].RowCount > 0)
            //{
            //    FpSpread1.Visible = true;

            //    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            //    darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            //    //darkstyle.ForeColor = System.Drawing.Color.Black;
            //    darkstyle.Font.Name = "Book Antiqua";
            //    darkstyle.Font.Size = FontUnit.Medium;
            //    darkstyle.Font.Bold = true;
            //    darkstyle.HorizontalAlign = HorizontalAlign.Center;
            //    darkstyle.VerticalAlign = VerticalAlign.Middle;
            //    darkstyle.ForeColor = System.Drawing.Color.White;
            //    darkstyle.Border.BorderSize = 0;
            //    darkstyle.Border.BorderColor = System.Drawing.Color.Transparent;
            //    //FpSpread1.AllowPaging = false;
            //    //FpSpread1.ColumnHeader.DefaultStyle = darkstyle;
            //    //gvStudTest.HeaderRow.Style.Add("width", "15%");
            //    //gvStudTest.HeaderRow.Style.Add("font-size", "8px");
            //    //gvStudTest.HeaderRow.Style.Add("text-align", "center");
            //    //gvStudTest.Style.Add("font-family", "Book Antiqua;");
            //    //gvStudTest.Style.Add("font-size", "6px");
            //    FpSpread1.DataBind();
            //    FpSpread1.Enabled = true;
            //    FpSpread1.RenderControl(hw00);
            //    FpSpread1.DataBind();
            //    //FpSpread1.Visible = false;
            //}

            //PdfPTable pdftbl0 = new PdfPTable(FpSpread1.Sheets[0].ColumnCount);
            //pdftbl0.TotalWidth = 500f;
            //PdfPCell cell;
            //if (FpSpread1.Sheets[0].ColumnCount > 0)
            //{
            //    for (int col = 0; col < FpSpread1.Sheets[0].ColumnCount; col++)
            //    {
            //        for (int row = 0; row < FpSpread1.Sheets[0].ColumnHeader.RowCount; row++)
            //        {
            //            if (col < 4)
            //            {
            //                cell = new PdfPCell(new Phrase(FpSpread1.Sheets[0].ColumnHeader.Cells[row, col].Text));
            //                cell.Border = 1;
            //                cell.HorizontalAlignment = 1;
            //                cell.VerticalAlignment = 1;
            //                pdftbl0.AddCell(cell);
            //            }
            //        }
            //    }
            //    pdfDoc.Add(pdftbl0);
            //}
            //if (FpSpread1.Sheets[0].RowCount > 0)
            //{
            //}

            //float[] width = new float[] { 200f, 100f, 200f, 200f };
            //pdftbl0.SetWidths(width);

            //PdfPCell cell = new PdfPCell(new Phrase("CLASS INCHARGE"));
            //cell.Border = 0;
            //cell.HorizontalAlignment = 0;
            //pdftbl0.AddCell(cell);
            //cell = new PdfPCell(new Phrase("HOD"));
            //cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            //pdftbl0.AddCell(cell);
            //cell = new PdfPCell(new Phrase("VICE PRINCIPAL"));
            //cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            //pdftbl0.AddCell(cell);
            //cell = new PdfPCell(new Phrase("PRINCIPAL"));
            //cell.Border = 0;
            //cell.HorizontalAlignment = 2;
            //pdftbl0.AddCell(cell);
            //pdfDoc.Add(pdftbl0);

            //if (gvStudTest.Rows.Count > 0)
            //{
            //    gvStudTest.Visible = true;
            //    gvStudTest.AllowPaging = false;
            //    gvStudTest.HeaderRow.Style.Add("width", "15%");
            //    gvStudTest.HeaderRow.Style.Add("font-size", "8px");
            //    gvStudTest.HeaderRow.Style.Add("text-align", "center");
            //    gvStudTest.Style.Add("font-family", "Book Antiqua;");
            //    gvStudTest.Style.Add("font-size", "6px");
            //    gvStudTest.DataBind();
            //    gvStudTest.Enabled = true;
            //    gvStudTest.RenderControl(hw00);
            //    gvStudTest.DataBind();
            //    gvStudTest.Visible = false;
            //}

            StringReader sr = new StringReader(Convert.ToString(sw));
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);

            sr = new StringReader(Convert.ToString(sw00));
            htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);

            StringWriter sw0 = new StringWriter();
            HtmlTextWriter hw0 = new HtmlTextWriter(sw0);

            lb4 = new Label();
            if (ChartQuesWiseDmg.Visible == true)
            {
                lb4.Text = "<br>Question Wise Damage Result Analysis Chart<br><br><br>";
                lb4.Style.Add("height", "100px");
                lb4.Style.Add("text-decoration", "none");
                lb4.Style.Add("font-family", "Book Antiqua;");
                lb4.Style.Add("font-size", "10px");
                lb4.Style.Add("font-weight", "bold");
                lb4.Style.Add("text-align", "left");
                lb4.RenderControl(hw0);
            }

            if (gvQuesWiseDmg.Rows.Count > 0)
            {
                gvQuesWiseDmg.AllowPaging = false;
                gvQuesWiseDmg.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                gvQuesWiseDmg.HeaderRow.Style.Add("width", "15%");
                gvQuesWiseDmg.HeaderRow.Style.Add("font-size", "10px");
                gvQuesWiseDmg.HeaderRow.Style.Add("text-align", "center");
                gvQuesWiseDmg.HeaderRow.Style.Add("font-weight", "bold");
                gvQuesWiseDmg.Style.Add("font-family", "Book Antiqua;");
                gvQuesWiseDmg.Style.Add("font-size", "6px");
                gvQuesWiseDmg.DataBind();
                gvQuesWiseDmg.Enabled = true;
                gvQuesWiseDmg.RenderControl(hw0);
                gvQuesWiseDmg.DataBind();
            }

            sr = new StringReader(Convert.ToString(sw0));
            htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);

            if (ChartQuesWiseDmg.Visible == true)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    ChartQuesWiseDmg.SaveImage(stream, ChartImageFormat.Png);
                    iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
                    chartImage.ScalePercent(75f);
                    pdfDoc.Add(chartImage);
                }
            }

            StringWriter sw1 = new StringWriter();
            HtmlTextWriter hw1 = new HtmlTextWriter(sw1);

            lb4 = new Label();
            if (ChartChapterWiseDmg.Visible == true)
            {
                lb4.Text = "<br>Chapter Wise Damage Result Analysis Chart<br><br><br>";
                lb4.Style.Add("height", "100px");
                lb4.Style.Add("text-decoration", "none");
                lb4.Style.Add("font-family", "Book Antiqua;");
                lb4.Style.Add("font-size", "10px");
                lb4.Style.Add("font-weight", "bold");
                lb4.Style.Add("text-align", "left");
                lb4.RenderControl(hw1);
            }

            if (gvChapWiseDmg.Rows.Count > 0)
            {
                gvChapWiseDmg.AllowPaging = false;
                gvChapWiseDmg.HeaderRow.Style.Add("width", "15%");
                gvChapWiseDmg.HeaderRow.Style.Add("font-size", "8px");
                gvChapWiseDmg.HeaderRow.Style.Add("text-align", "center");
                gvChapWiseDmg.Style.Add("font-family", "Book Antiqua;");
                gvChapWiseDmg.Style.Add("font-size", "6px");
                gvChapWiseDmg.DataBind();
                gvChapWiseDmg.Enabled = true;
                gvChapWiseDmg.RenderControl(hw1);
                gvChapWiseDmg.DataBind();
            }

            lb3.Text = "<br><b><br><br><br><br><br><br>";
            lb3.Style.Add("height", "200px");
            lb3.Style.Add("text-decoration", "none");
            lb3.Style.Add("font-family", "Book Antiqua;");
            lb3.Style.Add("font-size", "10px");
            lb3.Style.Add("text-align", "left");
            lb3.RenderControl(hw1);
            sr = new StringReader(Convert.ToString(sw1));
            htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);

            if (ChartChapterWiseDmg.Visible == true)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    ChartChapterWiseDmg.SaveImage(stream, ChartImageFormat.Png);
                    iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
                    chartImage.ScalePercent(75f);
                    pdfDoc.Add(chartImage);
                }
            }

            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        popupdiv.Visible = false;
    }

    protected void imgbtnClose_OnClick(object sender, EventArgs e)
    {
        try
        {
            chkShowSelQuestions.Checked = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblpopuperr.Text = "";
            popupdiv.Visible = false;
            divShowQuestions.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        try
        {
            chkShowSelQuestions.Checked = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblpopuperr.Text = "";
            popupdiv.Visible = false;
            divShowQuestions.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void FpShowQuestions_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string actrow = e.SheetView.ActiveRow.ToString();
        if (actrow == "0")
        {
            for (int j = 1; j < Convert.ToInt16(FpShowQuestions.Sheets[0].RowCount); j++)
            {
                string actcol = e.SheetView.ActiveColumn.ToString();
                string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                if (seltext != "System.Object")
                    FpShowQuestions.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
            }
        }

    }

    #endregion Button Events

    protected void fp1_Databind(object sender, FpSpreadTemplateReplacement e)
    {

    }

    protected void gvQuesWiseDmg_rowbound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int count = e.Row.Cells.Count;
                for (int i = 0; i < count; i++)
                {
                    if (i == 1)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    if (i == 2)
                    {
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Left;
                    }
                    else
                    {
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    protected void gvChapWiseDmg_rowbound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int count = e.Row.Cells.Count;
                for (int i = 0; i < count; i++)
                {
                    if (i == 0)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                    if (i == 1)
                    {
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Left;
                    }
                    else
                    {
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = ex.StackTrace;
            lblErrSearch.Visible = true;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    private void ShowQuestions(FpSpread FpSpread1)
    {
        try
        {
            Printcontrol1.Visible = false;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;

            bool isectionAvail = false;

            string qrySection = string.Empty;


            if (ddlCollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Year" : " Batch") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddldegree.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "School Type" : "Degree") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlbatch.Items.Count != 0)
            {
                batch_year = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
            }

            if (ddlbranch.Items.Count != 0)
            {
                degree_code = Convert.ToString(ddlbranch.SelectedValue).Trim();
            }
            else
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Standard" : "Department") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlsem.Items.Count != 0)
            {
                semester = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            }
            else
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Term" : " Semester") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlsec.Enabled == false || ddlsec.Items.Count == 0)
            {
                section = "";
                qrysec = "";
            }
            else if (ddlsec.Items.Count > 0)
            {
                section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                qrySection = " and Sections='" + section + "'";
            }

            if (ddlsubject.Items.Count != 0)
            {
                subject_no = Convert.ToString(ddlsubject.SelectedValue).Trim();

            }
            else
            {
                lblpopuperr.Text = "No Subject were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlTest.Items.Count > 0)
            {
                test_no = Convert.ToString(ddlTest.SelectedValue).Trim();
            }
            else
            {
                lblpopuperr.Text = "No Test Were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            //if (batch_year != "" && degree_code != "" && semester != "" && subject_no != "")
            if (!string.IsNullOrEmpty(collegecode.Trim()) && !string.IsNullOrEmpty(batch_year.Trim()) && !string.IsNullOrEmpty(degree_code.Trim()) && !string.IsNullOrEmpty(semester.Trim()) && !string.IsNullOrEmpty(subject_no.Trim()) && !string.IsNullOrEmpty(test_no))
            {
                //qry = "select distinct Questionentryid,No_Sections,qbd.qsection_no,Total_Questions,Minimu_Attend,no_Option,Marks,Syllabus,Questions from tbl_question_bank_master qb,tbl_Question_Bank_details qbd,tbl_Question_Bank_Questions qbq where qb.Questionid=qbd.Questionid and qbq.Questionid=qb.Questionid and qbd.qsection_no=qbq.qsection_no and qb.Subject_no=qbd.Subject_no and qbq.Subject_no=qbd.Subject_no   and qbd.Subject_no='" + subject_no + "' and Degree_Code='" + degree_code + "' and Batch_year='" + batch_year + "' and Semester='" + semester + "' " + qrySection + " order by qbd.qsection_no";

                qry = "select ROW_NUMBER() OVER (ORDER BY exq.Exist_questionPK,exq.Section,QuestionMasterPK) as QNo,exq.Exist_questionPK,qm.QuestionMasterPK,qm.question,exq.Section,qm.mark  from tbl_question_bank_master qbm,tbl_question_master qm, Exist_questions exq,sub_unit_details sud where qbm.Subject_no=qm.subject_no and qm.subject_no=exq.subject_no and exq.subject_no=qbm.Subject_no and qm.syllabus=exq.syllabus and qbm.exam_type=exq.is_internal and exq.QuestionMasterFK=qm.QuestionMasterPK and sud.subject_no=qbm.Subject_no and sud.subject_no=qm.subject_no and exq.subject_no=sud.subject_no and sud.topic_no=qm.syllabus and sud.topic_no=exq.syllabus and qbm.Batch_year='" + batch_year + "' and qbm.Degree_Code='" + degree_code + "' and qbm.Semester='" + semester + "' and qbm.Subject_no='" + subject_no + "' " + qrysec + " and exq.Test_code=qbm.Exam and exq.Test_code='" + test_no + "' and exq.is_internal=2 order by exq.Exist_questionPK,exq.Section,qm.QuestionMasterPK ; ";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(qry, "Text");
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                //Init_Spread(FpShowQuestions);

                #region FpSpread Style

                //FpShowQuestions.Visible = false;
                FpShowQuestions.Sheets[0].ColumnCount = 0;
                FpShowQuestions.Sheets[0].RowCount = 0;
                FpShowQuestions.Sheets[0].SheetCorner.ColumnCount = 0;
                FpShowQuestions.CommandBar.Visible = false;

                #endregion FpSpread Style

                //FpSpreadChapterWiseDMG.Visible = false;
                FpShowQuestions.CommandBar.Visible = false;
                FpShowQuestions.RowHeader.Visible = false;
                FpShowQuestions.Sheets[0].AutoPostBack = false;
                FpShowQuestions.Sheets[0].RowCount = 0;
                FpShowQuestions.Sheets[0].ColumnCount = 0;

                FpShowQuestions.Sheets[0].AutoPostBack = false;

                #region SpreadStyles

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
                //darkstyle.ForeColor = System.Drawing.Color.Black;
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Medium;
                darkstyle.Font.Bold = true;
                darkstyle.HorizontalAlign = HorizontalAlign.Center;
                darkstyle.VerticalAlign = VerticalAlign.Middle;
                darkstyle.ForeColor = System.Drawing.Color.White;
                darkstyle.Border.BorderSize = 1;
                darkstyle.Border.BorderColor = System.Drawing.Color.Black;

                FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
                //sheetstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
                //darkstyle.ForeColor = System.Drawing.Color.Black;
                sheetstyle.Font.Name = "Book Antiqua";
                sheetstyle.Font.Size = FontUnit.Medium;
                sheetstyle.Font.Bold = true;
                sheetstyle.HorizontalAlign = HorizontalAlign.Center;
                sheetstyle.VerticalAlign = VerticalAlign.Middle;
                sheetstyle.ForeColor = System.Drawing.Color.Black;
                sheetstyle.Border.BorderSize = 1;
                sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

                #endregion SpreadStyles

                FpShowQuestions.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                FpShowQuestions.Sheets[0].DefaultStyle = sheetstyle;
                FpShowQuestions.Sheets[0].ColumnHeader.RowCount = 1;

                int spreadHeight = 0;
                FpShowQuestions.Sheets[0].RowCount = 1;
                FpShowQuestions.Sheets[0].ColumnCount = 5;
                FpShowQuestions.Sheets[0].ColumnHeader.RowCount = 1;

                FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
                chkcell1.AutoPostBack = true;

                FpShowQuestions.Sheets[0].Columns[0].Width = 40;
                FpShowQuestions.Sheets[0].Columns[1].Width = 50;
                FpShowQuestions.Sheets[0].Columns[2].Width = 100;
                FpShowQuestions.Sheets[0].Columns[3].Width = 280;
                FpShowQuestions.Sheets[0].Columns[4].Width = 100;

                FpShowQuestions.Sheets[0].Columns[0].Locked = false;
                FpShowQuestions.Sheets[0].Columns[1].Locked = true;
                FpShowQuestions.Sheets[0].Columns[2].Locked = true;
                FpShowQuestions.Sheets[0].Columns[3].Locked = true;
                FpShowQuestions.Sheets[0].Columns[4].Locked = true;

                FpShowQuestions.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
                FpShowQuestions.Sheets[0].ColumnHeader.Cells[0, 1].Text = "QNo";
                FpShowQuestions.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Question Section";
                FpShowQuestions.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Questions";
                FpShowQuestions.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Max.Mark";

                FpShowQuestions.Sheets[0].SetColumnMerge(2, Farpoint.Model.MergePolicy.Always);

                FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 0].CellType = chkcell1;
                FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpShowQuestions.Sheets[0].FrozenRowCount = 1;
                spreadHeight = FpShowQuestions.Sheets[0].ColumnHeader.Rows[0].Height;
                for (int ques = 0; ques < ds.Tables[0].Rows.Count; ques++)
                {
                    FpShowQuestions.Sheets[0].RowCount++;

                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 0].CellType = chkcell;
                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[ques]["QNo"]);
                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[ques]["QuestionMasterPK"]);

                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[ques]["Section"]);

                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[ques]["question"]);

                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[ques]["mark"]);

                    spreadHeight += FpShowQuestions.Sheets[0].Rows[ques].Height;
                }
                //FpShowQuestions.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 1);
                //FpShowQuestions.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, 1);
                //FpShowQuestions.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, 1);
                //FpShowQuestions.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, 1);
                FpShowQuestions.Sheets[0].PageSize = FpShowQuestions.Sheets[0].RowCount;
                //FpShowQuestions.Width = 900;
                FpShowQuestions.Height = (spreadHeight) + 45;
                FpShowQuestions.SaveChanges();
                FpShowQuestions.Visible = true;
                divShowQuestions.Visible = true;
            }
            else
            {
                lblpopuperr.Text = "No Recoed(s) were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void ShowQuestions()
    {
        try
        {
            Printcontrol1.Visible = false;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;

            bool isectionAvail = false;

            string qrySection = string.Empty;


            if (ddlCollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Year" : " Batch") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddldegree.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "School Type" : "Degree") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlbatch.Items.Count != 0)
            {
                batch_year = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
            }

            if (ddlbranch.Items.Count != 0)
            {
                degree_code = Convert.ToString(ddlbranch.SelectedValue).Trim();
            }
            else
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Standard" : "Department") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlsem.Items.Count != 0)
            {
                semester = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            }
            else
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Term" : " Semester") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlsec.Enabled == false || ddlsec.Items.Count == 0)
            {
                section = "";
                qrysec = "";
            }
            else if (ddlsec.Items.Count > 0)
            {
                section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                qrySection = " and Sections='" + section + "'";
            }

            if (ddlsubject.Items.Count != 0)
            {
                subject_no = Convert.ToString(ddlsubject.SelectedValue).Trim();
            }
            else
            {
                lblpopuperr.Text = "No Subject were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlTest.Items.Count > 0)
            {
                test_no = Convert.ToString(ddlTest.SelectedValue).Trim();
                test_name = Convert.ToString(ddlTest.SelectedItem.Text).Trim();
            }
            else
            {
                lblpopuperr.Text = "No Test Were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            //if (batch_year != "" && degree_code != "" && semester != "" && subject_no != "")
            if (!string.IsNullOrEmpty(collegecode.Trim()) && !string.IsNullOrEmpty(batch_year.Trim()) && !string.IsNullOrEmpty(degree_code.Trim()) && !string.IsNullOrEmpty(semester.Trim()) && !string.IsNullOrEmpty(subject_no.Trim()) && !string.IsNullOrEmpty(test_no))
            {
                //qry = "select distinct Questionentryid,No_Sections,qbd.qsection_no,Total_Questions,Minimu_Attend,no_Option,Marks,Syllabus,Questions from tbl_question_bank_master qb,tbl_Question_Bank_details qbd,tbl_Question_Bank_Questions qbq where qb.Questionid=qbd.Questionid and qbq.Questionid=qb.Questionid and qbd.qsection_no=qbq.qsection_no and qb.Subject_no=qbd.Subject_no and qbq.Subject_no=qbd.Subject_no   and qbd.Subject_no='" + subject_no + "' and Degree_Code='" + degree_code + "' and Batch_year='" + batch_year + "' and Semester='" + semester + "' " + qrySection + " order by qbd.qsection_no";

                qry = "select ROW_NUMBER() OVER (ORDER BY exq.Exist_questionPK,exq.Section,QuestionMasterPK) as QNo,exq.Exist_questionPK,qm.QuestionMasterPK,qm.question,exq.Section,qm.mark  from tbl_question_bank_master qbm,tbl_question_master qm, Exist_questions exq,sub_unit_details sud where qbm.Subject_no=qm.subject_no and qm.subject_no=exq.subject_no and exq.subject_no=qbm.Subject_no and qm.syllabus=exq.syllabus and qbm.exam_type=exq.is_internal and exq.QuestionMasterFK=qm.QuestionMasterPK and sud.subject_no=qbm.Subject_no and sud.subject_no=qm.subject_no and exq.subject_no=sud.subject_no and sud.topic_no=qm.syllabus and sud.topic_no=exq.syllabus and qbm.Batch_year='" + batch_year + "' and qbm.Degree_Code='" + degree_code + "' and qbm.Semester='" + semester + "' and qbm.Subject_no='" + subject_no + "' " + qrysec + " and exq.Test_code=qbm.Exam and exq.Test_code='" + test_no + "' and exq.is_internal=2 order by exq.Exist_questionPK,exq.Section,qm.QuestionMasterPK ; ";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(qry, "Text");
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                //Init_Spread(FpShowQuestions);
                int spreadHeight = 0;
                #region FpSpread Style

                //FpShowQuestions.Visible = false;
                FpShowQuestions.Sheets[0].ColumnCount = 0;
                FpShowQuestions.Sheets[0].RowCount = 0;
                FpShowQuestions.Sheets[0].SheetCorner.ColumnCount = 0;
                FpShowQuestions.CommandBar.Visible = false;

                #endregion FpSpread Style

                //FpSpreadChapterWiseDMG.Visible = false;
                FpShowQuestions.CommandBar.Visible = false;
                FpShowQuestions.RowHeader.Visible = false;
                FpShowQuestions.Sheets[0].AutoPostBack = false;
                FpShowQuestions.Sheets[0].RowCount = 0;
                FpShowQuestions.Sheets[0].ColumnCount = 0;

                FpShowQuestions.Sheets[0].AutoPostBack = false;

                #region SpreadStyles

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
                //darkstyle.ForeColor = System.Drawing.Color.Black;
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Medium;
                darkstyle.Font.Bold = true;
                darkstyle.HorizontalAlign = HorizontalAlign.Center;
                darkstyle.VerticalAlign = VerticalAlign.Middle;
                darkstyle.ForeColor = System.Drawing.Color.White;
                darkstyle.Border.BorderSize = 1;
                darkstyle.Border.BorderColor = System.Drawing.Color.Black;

                FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
                //sheetstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
                //darkstyle.ForeColor = System.Drawing.Color.Black;
                sheetstyle.Font.Name = "Book Antiqua";
                sheetstyle.Font.Size = FontUnit.Medium;
                sheetstyle.Font.Bold = true;
                sheetstyle.HorizontalAlign = HorizontalAlign.Center;
                sheetstyle.VerticalAlign = VerticalAlign.Middle;
                sheetstyle.ForeColor = System.Drawing.Color.Black;
                sheetstyle.Border.BorderSize = 1;
                sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

                #endregion SpreadStyles

                FpShowQuestions.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                FpShowQuestions.Sheets[0].DefaultStyle = sheetstyle;
                FpShowQuestions.Sheets[0].ColumnHeader.RowCount = 1;

                FpShowQuestions.Sheets[0].RowCount = 1;
                FpShowQuestions.Sheets[0].ColumnCount = 5;
                FpShowQuestions.Sheets[0].ColumnHeader.RowCount = 1;

                FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
                chkcell1.AutoPostBack = true;

                FpShowQuestions.Sheets[0].Columns[0].Width = 40;
                FpShowQuestions.Sheets[0].Columns[1].Width = 50;
                FpShowQuestions.Sheets[0].Columns[2].Width = 100;
                FpShowQuestions.Sheets[0].Columns[3].Width = 280;
                FpShowQuestions.Sheets[0].Columns[4].Width = 100;

                FpShowQuestions.Sheets[0].Columns[0].Locked = false;
                FpShowQuestions.Sheets[0].Columns[1].Locked = true;
                FpShowQuestions.Sheets[0].Columns[2].Locked = true;
                FpShowQuestions.Sheets[0].Columns[3].Locked = true;
                FpShowQuestions.Sheets[0].Columns[4].Locked = true;

                FpShowQuestions.Sheets[0].Columns[0].Resizable = false;
                FpShowQuestions.Sheets[0].Columns[1].Resizable = false;
                FpShowQuestions.Sheets[0].Columns[2].Resizable = false;
                FpShowQuestions.Sheets[0].Columns[3].Resizable = false;
                FpShowQuestions.Sheets[0].Columns[4].Resizable = false;

                FpShowQuestions.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
                FpShowQuestions.Sheets[0].ColumnHeader.Cells[0, 1].Text = "QNo";
                FpShowQuestions.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Question Section";
                FpShowQuestions.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Questions";
                FpShowQuestions.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Max.Mark";

                FpShowQuestions.Sheets[0].SetColumnMerge(2, Farpoint.Model.MergePolicy.Always);

                FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 0].CellType = chkcell1;
                FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpShowQuestions.Sheets[0].FrozenRowCount = 1;
                spreadHeight = FpShowQuestions.Sheets[0].ColumnHeader.Rows[0].Height;
                int colwidth = 280;
                int oldSpreadWidth = 290;
                int newSpreadWidth = 0;
                for (int ques = 0; ques < ds.Tables[0].Rows.Count; ques++)
                {
                    FpShowQuestions.Sheets[0].RowCount++;

                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 0].CellType = chkcell;
                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[ques]["QNo"]);
                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[ques]["QuestionMasterPK"]);
                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;


                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 2].Text = "Part - " + Convert.ToString(ds.Tables[0].Rows[ques]["Section"]).Trim();
                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;


                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[ques]["question"]);
                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                    int newColWidth = Convert.ToString(ds.Tables[0].Rows[ques]["question"]).Length * 10 + 20;
                    if (newColWidth >= colwidth)
                    {
                        FpShowQuestions.Sheets[0].Columns[3].Width = newColWidth;
                        colwidth = newColWidth;
                    }
                    else
                    {
                        FpShowQuestions.Sheets[0].Columns[3].Width = colwidth;
                    }
                    newSpreadWidth = oldSpreadWidth + colwidth;

                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[ques]["mark"]);
                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpShowQuestions.Sheets[0].Cells[FpShowQuestions.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                    spreadHeight += FpShowQuestions.Sheets[0].Rows[ques].Height;
                }
                //FpShowQuestions.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 1);
                //FpShowQuestions.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, 1);
                //FpShowQuestions.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, 1);
                //FpShowQuestions.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, 1);
                FpShowQuestions.Sheets[0].PageSize = FpShowQuestions.Sheets[0].RowCount;
                FpShowQuestions.Width = newSpreadWidth;
                FpShowQuestions.Height = (spreadHeight) + 45;
                FpShowQuestions.SaveChanges();
                FpShowQuestions.Visible = true;
                if (chkShowSelQuestions.Checked)
                    divShowQuestions.Visible = true;
            }
            else
            {
                //lblpopuperr.Text = "No Questions were Found";
                lblpopuperr.Text = "No Questions Were Found For Selected Test " + test_name + " !!!";// "No Recoed(s) were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void ChapterWiseDMG(DataTable dtChapters, DataTable dtQuestionWiseClassDMG, ref DataTable dtChapterWiseClassDMG)
    {
        try
        {
            dtChapterWiseClassDMG.Columns.Clear();
            dtChapterWiseClassDMG.Rows.Clear();
            dtChapterWiseClassDMG.Columns.Add("Chapter_No");
            dtChapterWiseClassDMG.Columns.Add("Chapters");
            dtChapterWiseClassDMG.Columns.Add("CLASS_DMG%");
            DataRow drClassDmg;
            if (dtChapters.Rows.Count > 0)
            {
                if (dtQuestionWiseClassDMG.Rows.Count > 0)
                {
                    for (int chapter = 0; chapter < dtChapters.Rows.Count; chapter++)
                    {
                        drClassDmg = dtChapterWiseClassDMG.NewRow();

                        DataView dvQuesWiseClassDMG = new DataView();
                        drClassDmg["Chapters"] = Convert.ToString(dtChapters.Rows[chapter]["unit_name"]);
                        drClassDmg["Chapter_No"] = Convert.ToString(dtChapters.Rows[chapter]["topic_no"]);
                        double chapterClassDMG = 0;
                        double chapterClassAvg = 0;

                        dtQuestionWiseClassDMG.DefaultView.RowFilter = "Chapter_No='" + Convert.ToString(dtChapters.Rows[chapter]["topic_no"]) + "'";
                        dvQuesWiseClassDMG = dtQuestionWiseClassDMG.DefaultView;
                        if (dvQuesWiseClassDMG.Count > 0)
                        {
                            for (int ques = 0; ques < dvQuesWiseClassDMG.Count; ques++)
                            {
                                double questionWiseDMG = 0;
                                double.TryParse(Convert.ToString(dvQuesWiseClassDMG[ques]["QwiseDmg%"]), out questionWiseDMG);
                                chapterClassDMG += questionWiseDMG;
                            }
                            chapterClassAvg = Math.Round(chapterClassDMG / dvQuesWiseClassDMG.Count, 0, MidpointRounding.AwayFromZero);
                        }
                        drClassDmg["CLASS_DMG%"] = Convert.ToString(chapterClassAvg);
                        dtChapterWiseClassDMG.Rows.Add(drClassDmg);
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void ChapterWiseClassDMG(DataTable dtChapters, DataTable dtStudDetails, DataTable dtStudAllMarks, ref DataTable dtChapterWiseClassDMG, ref DataTable dtStudChapterDMG)
    {
        try
        {
            dtChapterWiseClassDMG.Columns.Clear();
            dtChapterWiseClassDMG.Rows.Clear();
            dtChapterWiseClassDMG.Columns.Add("Chapter_No");
            dtChapterWiseClassDMG.Columns.Add("Chapters");
            dtChapterWiseClassDMG.Columns.Add("CLASS_DMG%");

            dtStudChapterDMG.Columns.Clear();
            dtStudChapterDMG.Rows.Clear();
            dtStudChapterDMG.Columns.Add("Roll_No");
            dtStudChapterDMG.Columns.Add("Stud_Name");
            dtStudChapterDMG.Columns.Add("Chapter_No");
            dtStudChapterDMG.Columns.Add("Chapter_Name");
            dtStudChapterDMG.Columns.Add("Chapter_DMG");

            DataRow drClassDmg;
            DataRow drStudClassDmg;

            int totstudents = 0;
            if (dtChapters.Rows.Count > 0)
            {
                if (dtStudDetails.Rows.Count > 0)
                {
                    totstudents = dtStudDetails.Rows.Count;
                    for (int chapter = 0; chapter < dtChapters.Rows.Count; chapter++)
                    {
                        drClassDmg = dtChapterWiseClassDMG.NewRow();
                        drClassDmg["Chapters"] = Convert.ToString(dtChapters.Rows[chapter]["unit_name"]);
                        drClassDmg["Chapter_No"] = Convert.ToString(dtChapters.Rows[chapter]["topic_no"]);
                        double chapterClassDMG = 0;
                        double chapterClassAvg = 0;
                        //drClassDmg["Chapter_DMG"] = Convert.ToString(dtChapters.Rows[chapter][""]);
                        for (int stud = 0; stud < dtStudDetails.Rows.Count; stud++)
                        {
                            double chapterStudDMG = 0;
                            double avg = 0;
                            drStudClassDmg = dtStudChapterDMG.NewRow();
                            drStudClassDmg["Roll_No"] = Convert.ToString(dtStudDetails.Rows[stud]["Roll_No"]);
                            drStudClassDmg["Stud_Name"] = Convert.ToString(dtStudDetails.Rows[stud]["Stud_Name"]);
                            drStudClassDmg["Chapter_Name"] = Convert.ToString(dtChapters.Rows[chapter]["unit_name"]);
                            drStudClassDmg["Chapter_No"] = Convert.ToString(dtChapters.Rows[chapter]["topic_no"]);
                            dtStudAllMarks.DefaultView.RowFilter = "topic_no='" + Convert.ToString(dtChapters.Rows[chapter]["topic_no"]) + "' and Roll_No='" + Convert.ToString(dtStudDetails.Rows[stud]["Roll_No"]) + "'";//
                            DataView dvStudMark = dtStudAllMarks.DefaultView;
                            if (dvStudMark.Count > 0)
                            {
                                for (int chp = 0; chp < dvStudMark.Count; chp++)
                                {
                                    string maxMark = Convert.ToString(dvStudMark[chp]["Max_Mark"]);
                                    string marksec = Convert.ToString(dvStudMark[chp]["mark_obtained"]).Trim();
                                    double studdmg = 0;
                                    double marks = 0;
                                    FindDMG(maxMark, marksec, out studdmg);
                                    chapterClassDMG += studdmg;
                                    double.TryParse(marksec, out marks);
                                    chapterStudDMG += marks;
                                }
                                avg = Math.Round(chapterStudDMG / dvStudMark.Count, 0, MidpointRounding.AwayFromZero);
                            }
                            drStudClassDmg["Chapter_DMG"] = Convert.ToString(avg);//Convert.ToString(((dvStudMark.Count!=0)?Convert.ToString(avg):"0"));
                            dtStudChapterDMG.Rows.Add(drStudClassDmg);
                        }
                        chapterClassAvg = Math.Round(chapterClassDMG / dtStudDetails.Rows.Count, 0, MidpointRounding.AwayFromZero);
                        drClassDmg["CLASS_DMG%"] = Convert.ToString(chapterClassAvg);
                        dtChapterWiseClassDMG.Rows.Add(drClassDmg);
                    }
                    //if (dtStudChapterDMG.Rows.Count > 0)
                    //{
                    //    dtStudChapterDMG.DefaultView.Sort = "Roll_No,Chapter_No";
                    //}
                }
                //if (dtStudAllMarks.Rows.Count > 0)
                //{
                //    for (int chapter = 0; chapter < dtChapters.Rows.Count; chapter++)
                //    {
                //        dtStudAllMarks.DefaultView.RowFilter = "topic_no='" + Convert.ToString(dtChapters.Rows[chapter]["Syllabus"]) + "'";//
                //        DataView dvStudMark = dtStudAllMarks.DefaultView;
                //    }
                //}
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void QuestionWiseClassDMG(DataTable dtAllQuestions, DataTable dtQuestions, DataTable dtStudDetails, DataTable dtStudAllMarks, ref DataTable dtQuestionWiseClassDMG, ref DataTable dtStudQuestionsDMG)
    {
        try
        {
            dtQuestionWiseClassDMG.Columns.Clear();
            dtQuestionWiseClassDMG.Rows.Clear();
            dtQuestionWiseClassDMG.Columns.Add("SNo");
            dtQuestionWiseClassDMG.Columns.Add("Question_No");
            dtQuestionWiseClassDMG.Columns.Add("Chapter_No");
            dtQuestionWiseClassDMG.Columns.Add("Chapters");
            dtQuestionWiseClassDMG.Columns.Add("QwiseDmg%");
            //dtQuestionWiseClassDMG.Columns.Add("SNo");
            //dtQuestionWiseClassDMG.Columns.Add("Question_Name");
            //dtQuestionWiseClassDMG.Columns.Add("Question_No");
            //dtQuestionWiseClassDMG.Columns.Add("Chapter_No");
            //dtQuestionWiseClassDMG.Columns.Add("Chapter_Name");
            //dtQuestionWiseClassDMG.Columns.Add("Question_DMG");

            dtStudQuestionsDMG.Columns.Clear();
            dtStudQuestionsDMG.Rows.Clear();
            dtStudQuestionsDMG.Columns.Add("Roll_No");
            dtStudQuestionsDMG.Columns.Add("Stud_Name");
            dtStudQuestionsDMG.Columns.Add("Question_No");
            dtStudQuestionsDMG.Columns.Add("Question_Name");
            dtStudQuestionsDMG.Columns.Add("Chapter_No");
            dtStudQuestionsDMG.Columns.Add("Chapter_Name");
            dtStudQuestionsDMG.Columns.Add("Question_DMG");

            DataRow drClassDmg;
            DataRow drStudClassDmg;

            int totstudents = 0;
            if (dtQuestions.Rows.Count > 0)
            {
                if (dtStudDetails.Rows.Count > 0)
                {
                    totstudents = dtStudDetails.Rows.Count;
                    for (int question = 0; question < dtQuestions.Rows.Count; question++)
                    {
                        drClassDmg = dtQuestionWiseClassDMG.NewRow();
                        DataTable dtNewQues = new DataTable();
                        if (dtAllQuestions.Rows.Count > 0)
                        {
                            DataView dv = new DataView();
                            dtAllQuestions.DefaultView.RowFilter = "QuestionMasterPK='" + Convert.ToString(dtQuestions.Rows[question]["QuestionMasterPK"]) + "'";
                            dv = dtAllQuestions.DefaultView;
                            dtNewQues = dv.ToTable(true, "QNo", "Section", "mark");
                        }
                        if (dtNewQues.Rows.Count > 0)
                        {
                            drClassDmg["SNo"] = Convert.ToString((dtNewQues.Rows[0]["QNo"]));
                        }
                        else
                        {
                            drClassDmg["SNo"] = Convert.ToString((question + 1));
                        }

                        //drClassDmg["SNo"] = (question + 1);
                        //drClassDmg["Question_Name"] = Convert.ToString(dtQuestions.Rows[question]["question"]);
                        drClassDmg["Question_No"] = Convert.ToString(dtQuestions.Rows[question]["QuestionMasterPK"]);
                        drClassDmg["Chapter_No"] = Convert.ToString(dtQuestions.Rows[question]["topic_no"]);
                        drClassDmg["Chapters"] = Convert.ToString(dtQuestions.Rows[question]["unit_name"]);
                        double QuestionClassDMG = 0;
                        double QuestionClassAvg = 0;
                        //drClassDmg["Chapter_DMG"] = Convert.ToString(dtQuestions.Rows[chapter][""]);Syllabus
                        for (int stud = 0; stud < dtStudDetails.Rows.Count; stud++)
                        {
                            double QuestionStudDMG = 0;
                            double avg = 0;
                            drStudClassDmg = dtStudQuestionsDMG.NewRow();
                            drStudClassDmg["Roll_No"] = Convert.ToString(dtStudDetails.Rows[stud]["Roll_No"]);
                            drStudClassDmg["Stud_Name"] = Convert.ToString(dtStudDetails.Rows[stud]["Stud_Name"]);
                            drStudClassDmg["Question_Name"] = Convert.ToString(dtQuestions.Rows[question]["question"]);
                            drStudClassDmg["Question_No"] = Convert.ToString(dtQuestions.Rows[question]["QuestionMasterPK"]);
                            drStudClassDmg["Chapter_No"] = Convert.ToString(dtQuestions.Rows[question]["topic_no"]);
                            drStudClassDmg["Chapter_Name"] = Convert.ToString(dtQuestions.Rows[question]["unit_name"]);

                            dtStudAllMarks.DefaultView.RowFilter = "QuestionMasterPK='" + Convert.ToString(dtQuestions.Rows[question]["QuestionMasterPK"]) + "' and Roll_No='" + Convert.ToString(dtStudDetails.Rows[stud]["Roll_No"]) + "'";//
                            DataView dvStudMark = dtStudAllMarks.DefaultView;
                            if (dvStudMark.Count > 0)
                            {
                                for (int chp = 0; chp < dvStudMark.Count; chp++)
                                {
                                    string maxMark = Convert.ToString(dvStudMark[chp]["Max_Mark"]);
                                    string marksec = Convert.ToString(dvStudMark[chp]["mark_obtained"]).Trim();
                                    double studdmg = 0;
                                    double marks = 0;
                                    FindDMG(maxMark, marksec, out studdmg);
                                    QuestionClassDMG += studdmg;
                                    double.TryParse(marksec, out marks);
                                    QuestionStudDMG += studdmg;
                                }
                                avg = Math.Round(QuestionStudDMG, 0, MidpointRounding.AwayFromZero);
                            }
                            drStudClassDmg["Question_DMG"] = Convert.ToString(avg);//Convert.ToString(((dvStudMark.Count!=0)?Convert.ToString(avg):"0"));
                            dtStudQuestionsDMG.Rows.Add(drStudClassDmg);
                        }
                        QuestionClassAvg = Math.Round(QuestionClassDMG / dtStudDetails.Rows.Count, 0, MidpointRounding.AwayFromZero);
                        drClassDmg["QwiseDmg%"] = Convert.ToString(QuestionClassAvg);
                        dtQuestionWiseClassDMG.Rows.Add(drClassDmg);
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    private bool FindDMG(string maxMark, string markSecured, out double Dmg)
    {
        Dmg = 0;
        bool isSuccess = false;
        try
        {
            double max = 0;
            double markSec = 0;
            bool isValidMax = double.TryParse(maxMark, out max);
            bool isValidSec = double.TryParse(markSecured, out markSec);
            if (isValidMax && isValidSec)
            {
                if (max > 0)
                {
                    Dmg = ((max - markSec) / max) * 100;
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            return isSuccess;
        }
        catch (Exception ex)
        {
            return isSuccess;
        }
    }

}