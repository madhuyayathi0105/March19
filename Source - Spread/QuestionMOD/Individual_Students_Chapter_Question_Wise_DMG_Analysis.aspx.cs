using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Farpoint = FarPoint.Web.Spread;
using System.Configuration;

public partial class Individual_Students_Chapter_Question_Wise_DMG_Analysis : System.Web.UI.Page
{

    #region Fields Delaration

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

    GridView gvNew = new GridView();
    DataTable dtNew = new DataTable();

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet dsCollegeInfo = new DataSet();

    Chart[] chartChapterDMG = new Chart[1];
    Chart[] chartQuestionDMG = new Chart[1];

    DataTable dtStudChapterDMG = new DataTable();
    DataTable dtStudQuestionDMG = new DataTable();

    #endregion Fields Delaration

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetRollNo(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select Roll_No from Registration where DelFlag=0 and Exam_Flag <>'Debar' and Roll_No Like '" + prefixText + "%' order by Roll_No";
        name = ws.Getname(query);
        return name;
    }
    
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> NewGetRollNo(string prefixText, int count, string contextKey)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string qrybatch = string.Empty;

        if (contextKey != "0" && contextKey.Trim() != "" && contextKey != null)
        {
            string query = "select Roll_No from Registration where DelFlag=0 and cc=0 and Exam_Flag <>'Debar' and Roll_No Like '" + prefixText + "%' " + contextKey + " order by Roll_No";
            name = ws.Getname(query);
        }
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetRollNo1(string prefixText, string batch, string degreecode, string sem)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select Roll_No from Registration where DelFlag=0 and Exam_Flag <>'Debar' and Roll_No Like '" + prefixText + "%' order by Roll_No";
        name = ws.Getname(query);
        return name;
    }

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.DataBind();
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
            usercode = Convert.ToString(Session["usercode"]).Trim();
            collegecode = Convert.ToString(Session["collegecode"]).Trim();
            singleuser = Convert.ToString(Session["single_user"]).Trim();
            group_user = Convert.ToString(Session["group_code"]).Trim();
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
                Session["ChapterCharts"] = null;
                Session["QuestionCharts"] = null;
                rptprint1.Visible = false;
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                popupdiv.Visible = false;
                divMainContent.Visible = false;
                chkShowSelQuestions.Checked = false;
                rblMultiSingleSelective.SelectedValue = "0";
                txtRollNo.Text = "";

                #region LoadHeader

                Bindcollege();
                BindBatch();
                BindDegree();
                bindbranch();
                bindsem();
                BindSectionDetail();
                BindRollNo();
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
            else
            {
                if (FpSpreadChapterWiseDMG.Sheets[0].RowCount > 0)
                    btnGo_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        //SetCharts();
        //InitComplete();
        //List<string> keys = Request.Form.AllKeys.Where(key => key.Contains("chartChapterDMG")).ToList();
        //List<string> keys1 = Request.Form.AllKeys.Where(key => key.Contains("chartQuestionDMG")).ToList();
        //int i = 1;
        //int j = 1;
        //foreach (string key in keys)
        //{
        //    this.CreateChapterControls("chartChapterDMG" + i);
        //    i++;
        //}
        //foreach (string key in keys1)
        //{
        //    this.CreateQuestionControls("chartQuestionDMG" + j);
        //    j++;
        //}
    }

    #endregion Page Load

    #region Bind Header

    public void bindcollege()
    {
        try
        {
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void Bindcollege()
    {
        try
        {

            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;

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
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;

            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "Batch_year";
                ddlBatch.DataValueField = "Batch_year";
                ddlBatch.DataBind();
                ddlBatch.SelectedIndex = ddlBatch.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindDegree()
    {
        try
        {
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;

            ddlDegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDegree.DataSource = ds;
                ddlDegree.DataTextField = "course_name";
                ddlDegree.DataValueField = "course_id";
                ddlDegree.DataBind();

            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }

    }

    public void bindbranch()
    {
        try
        {
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;

            string course_id = Convert.ToString(ddlDegree.SelectedValue).Trim();
            ddlBranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlBranch.DataSource = ds;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindSectionDetail()
    {
        try
        {
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;

            string strbatch = Convert.ToString(ddlBatch.SelectedValue).Trim();
            string strbranch = Convert.ToString(ddlBranch.SelectedValue).Trim();

            ddlSec.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSectionDetail(strbatch, strbranch);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlSec.DataSource = ds;
                ddlSec.DataTextField = "sections";
                ddlSec.DataBind();
                if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
                {
                    ddlSec.Enabled = false;
                }
                else
                {
                    ddlSec.Enabled = true;
                }
            }
            else
            {
                ddlSec.Enabled = false;
            }
            GetSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void bindsem()
    {
        try
        {
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;

            string strbatchyear = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
            string strbranch = Convert.ToString(ddlBranch.SelectedValue).Trim();

            ddlSem.Items.Clear();
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
                        ddlSem.Items.Add(Convert.ToString(i));
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSem.Items.Add(Convert.ToString(i));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void GetSubject()
    {
        try
        {
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;

            string subjectquery = string.Empty;
            ddlSubject.Items.Clear();
            string sections = "";
            string strsec = string.Empty;
            if (ddlSec.Items.Count > 0)
            {
                sections = Convert.ToString(ddlSec.SelectedValue).Trim();
                if (Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() == "")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and st.Sections='" + Convert.ToString(sections).Trim() + "'";
                }
            }

            string sems = "";
            if (ddlSem.Items.Count > 0)
            {
                if (Convert.ToString(ddlSem.SelectedValue).Trim() != "")
                {
                    if (Convert.ToString(ddlSem.SelectedValue).Trim() == "")
                    {
                        sems = "";
                    }
                    else
                    {
                        sems = "and SM.semester=" + Convert.ToString(ddlSem.SelectedValue).Trim() + "";
                    }

                    if (Convert.ToString(Session["Staff_Code"]).Trim() == "")
                    {
                        //subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' " + Convert.ToString(sems).Trim() + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "' order by S.subject_no ";
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code=" + Convert.ToString(ddlBranch.SelectedValue) + " " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlBatch.SelectedValue) + "' order by S.subject_no ";
                    }
                    else if (Convert.ToString(Session["Staff_Code"]).Trim() != "")
                    {
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and st.subject_no=s.subject_no and s.syll_code=SM.syll_code and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' " + Convert.ToString(sems).Trim() + " and  SM.batch_year='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "'  and SC.staff_code='" + Convert.ToString(Session["Staff_Code"]).Trim() + "' " + strsec + "  order by S.subject_no ";
                    }
                    if (subjectquery != "")
                    {
                        ds.Dispose();
                        ds.Reset();
                        ds = d2.select_method(subjectquery, hat, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlSubject.Enabled = true;
                            ddlSubject.DataSource = ds;
                            ddlSubject.DataValueField = "Subject_No";
                            ddlSubject.DataTextField = "Subject_Name";
                            ddlSubject.DataBind();
                        }
                        else
                        {
                            ddlSubject.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
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
            batch_year = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
            degree_code = Convert.ToString(ddlBranch.SelectedValue).Trim();
            semester = Convert.ToString(ddlSem.SelectedItem.Text).Trim();
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
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;

            bool isectionAvail = false;
            string qrySection = string.Empty;
            cblQuestions.Items.Clear();
            txtQuestions.Text = "-- Select --";
            chkQuestions.Checked = false;

            if (ddlCollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }

            if (ddlBatch.Items.Count != 0)
            {
                batch_year = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
            }

            if (ddlBranch.Items.Count != 0)
            {
                degree_code = Convert.ToString(ddlBranch.SelectedValue).Trim();
            }

            if (ddlSem.Items.Count != 0)
            {
                semester = Convert.ToString(ddlSem.SelectedItem.Text).Trim();
            }
            if (ddlSec.Enabled == false || ddlSec.Items.Count == 0)
            {
                section = "";
                qrysec = "";
            }
            else if (ddlSec.Items.Count > 0)
            {
                section = Convert.ToString(ddlSec.SelectedItem.Text).Trim();
                qrySection = " and Sections='" + section + "'";
            }
            if (ddlSubject.Items.Count != 0)
            {
                subject_no = Convert.ToString(ddlSubject.SelectedValue).Trim();
            }
            if (ddlTest.Items.Count > 0)
            {
                test_no = Convert.ToString(ddlTest.SelectedValue).Trim();
            }
            //if (batch_year != "" && degree_code != "" && semester != "" && subject_no != "")
            if (!string.IsNullOrEmpty(collegecode.Trim()) && !string.IsNullOrEmpty(batch_year.Trim()) && !string.IsNullOrEmpty(degree_code.Trim()) && !string.IsNullOrEmpty(semester.Trim()) && !string.IsNullOrEmpty(subject_no.Trim()) && !string.IsNullOrEmpty(test_no))
            {
                //qry = "select distinct Questionentryid,No_Sections,qbd.qsection_no,Total_Questions,Minimu_Attend,no_Option,Marks,Syllabus,Questions from tbl_question_bank_master qb,tbl_Question_Bank_details qbd,tbl_Question_Bank_Questions qbq where qb.Questionid=qbd.Questionid and qbq.Questionid=qb.Questionid and qbd.qsection_no=qbq.qsection_no and qb.Subject_no=qbd.Subject_no and qbq.Subject_no=qbd.Subject_no   and qbd.Subject_no='" + subject_no + "' and Degree_Code='" + degree_code + "' and Batch_year='" + batch_year + "' and Semester='" + semester + "' " + qrySection + " order by qbd.qsection_no";

                qry = "select exq.Exist_questionPK,qm.QuestionMasterPK,qm.question,exq.Section,qm.mark  from tbl_question_bank_master qbm,tbl_question_master qm, Exist_questions exq,sub_unit_details sud where qbm.Subject_no=qm.subject_no and qm.subject_no=exq.subject_no and exq.subject_no=qbm.Subject_no and qm.syllabus=exq.syllabus and qbm.exam_type=exq.is_internal and exq.QuestionMasterFK=qm.QuestionMasterPK and sud.subject_no=qbm.Subject_no and sud.subject_no=qm.subject_no and exq.subject_no=sud.subject_no and sud.topic_no=qm.syllabus and sud.topic_no=exq.syllabus and qbm.Batch_year='" + batch_year + "' and qbm.Degree_Code='" + degree_code + "' and qbm.Semester='" + semester + "' and qbm.Subject_no='" + subject_no + "' " + qrysec + " and exq.Test_code=qbm.Exam and exq.Test_code='" + test_no + "' and exq.is_internal=2 order by exq.Exist_questionPK,exq.Section,qm.QuestionMasterPK ; ";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(qry, "Text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblQuestions.DataSource = ds;
                    cblQuestions.DataTextField = "question";
                    cblQuestions.DataValueField = "QuestionMasterPK";
                    cblQuestions.DataBind();
                    for (int h = 0; h < cblQuestions.Items.Count; h++)
                    {
                        cblQuestions.Items[h].Selected = true;
                    }
                    txtQuestions.Text = "Question" + "(" + cblQuestions.Items.Count + ")";
                    chkQuestions.Checked = true;
                }
            }
           
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void Init_Spread()
    {
        try
        {
            #region FpSpread Style

            FpSpreadChapterWiseDMG.Visible = false;
            FpSpreadChapterWiseDMG.Sheets[0].ColumnCount = 0;
            FpSpreadChapterWiseDMG.Sheets[0].RowCount = 0;
            FpSpreadChapterWiseDMG.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpreadChapterWiseDMG.CommandBar.Visible = false;

            #endregion FpSpread Style

            FpSpreadChapterWiseDMG.Visible = false;
            FpSpreadChapterWiseDMG.CommandBar.Visible = false;
            FpSpreadChapterWiseDMG.RowHeader.Visible = false;
            FpSpreadChapterWiseDMG.Sheets[0].AutoPostBack = true;
            FpSpreadChapterWiseDMG.Sheets[0].RowCount = 0;
            //FpSpreadChapterWiseDMG.Sheets[0].ColumnCount = 4;
            //FpSpreadChapterWiseDMG.Sheets[0].FrozenColumnCount = 4;

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

            FpSpreadChapterWiseDMG.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpreadChapterWiseDMG.Sheets[0].DefaultStyle = sheetstyle;
            FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.RowCount = 1;

            //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            //FpSpreadChapterWiseDMG.Sheets[0].Columns[0].Width = 40;
            //FpSpreadChapterWiseDMG.Sheets[0].Columns[1].Width = 150;
            //FpSpreadChapterWiseDMG.Sheets[0].Columns[2].Width = 40;
            //FpSpreadChapterWiseDMG.Sheets[0].Columns[3].Width = 80;
            //FpSpreadChapterWiseDMG.Sheets[0].Columns[0].Locked = true;
            //FpSpreadChapterWiseDMG.Sheets[0].Columns[1].Locked = true;
            //FpSpreadChapterWiseDMG.Sheets[0].Columns[2].Locked = true;
            //FpSpreadChapterWiseDMG.Sheets[0].Columns[3].Locked = true;
            //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Chapter Name";
            //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.Cells[0, 2].Text = "QNo.";
            //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Max.Mark";
            //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void Init_Spread(Farpoint.FpSpread FpSpreadChapterWiseDMG)
    {
        try
        {
            #region FpSpread Style

            FpSpreadChapterWiseDMG.Visible = false;
            FpSpreadChapterWiseDMG.Sheets[0].ColumnCount = 0;
            FpSpreadChapterWiseDMG.Sheets[0].RowCount = 0;
            FpSpreadChapterWiseDMG.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpreadChapterWiseDMG.CommandBar.Visible = false;

            #endregion FpSpread Style

            //FpSpreadChapterWiseDMG.Visible = false;
            FpSpreadChapterWiseDMG.CommandBar.Visible = false;
            FpSpreadChapterWiseDMG.RowHeader.Visible = false;
            FpSpreadChapterWiseDMG.Sheets[0].AutoPostBack = false;
            FpSpreadChapterWiseDMG.Sheets[0].RowCount = 0;
            FpSpreadChapterWiseDMG.Sheets[0].ColumnCount = 0;

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

            FpSpreadChapterWiseDMG.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpreadChapterWiseDMG.Sheets[0].DefaultStyle = sheetstyle;
            FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.RowCount = 1;

            //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            //FpSpreadChapterWiseDMG.Sheets[0].Columns[0].Width = 40;
            //FpSpreadChapterWiseDMG.Sheets[0].Columns[1].Width = 150;
            //FpSpreadChapterWiseDMG.Sheets[0].Columns[2].Width = 40;
            //FpSpreadChapterWiseDMG.Sheets[0].Columns[3].Width = 80;
            //FpSpreadChapterWiseDMG.Sheets[0].Columns[0].Locked = true;
            //FpSpreadChapterWiseDMG.Sheets[0].Columns[1].Locked = true;
            //FpSpreadChapterWiseDMG.Sheets[0].Columns[2].Locked = true;
            //FpSpreadChapterWiseDMG.Sheets[0].Columns[3].Locked = true;
            //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Chapter Name";
            //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.Cells[0, 2].Text = "QNo.";
            //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Max.Mark";
            //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindRollNo()
    {
        try
        {
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;

            batch_year = string.Empty;
            degree_code = string.Empty;
            semester = string.Empty;
            section = string.Empty;
            qrysec = string.Empty;
            cblSelRollNo.Items.Clear();
            chkSelRollNo.Checked = false;
            txtSelRollNo.Text = "--- Select ---";
            if (ddlCollege.Items.Count != 0)
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            if (ddlBatch.Items.Count != 0)
            {
                batch_year = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
            }

            if (ddlBranch.Items.Count != 0)
            {
                degree_code = Convert.ToString(ddlBranch.SelectedValue).Trim();
            }
            if (ddlSem.Items.Count != 0)
            {
                semester = Convert.ToString(ddlSem.SelectedItem.Text).Trim();
            }
            if (ddlSec.Enabled == false || ddlSec.Items.Count == 0)
            {
                section = "";
                qrysec = string.Empty;
            }
            else
            {
                section = Convert.ToString(ddlSec.SelectedItem.Text).Trim();
                qrysec = " and Sections='" + section + "'";
            }

            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batch_year) && !string.IsNullOrEmpty(degree_code) && !string.IsNullOrEmpty(semester))
            {
                qry = "select Roll_No,Reg_No,Stud_Name from Registration where Batch_Year='" + batch_year + "' and degree_code='" + degree_code + "'  and Current_Semester ='" + semester + "' " + qrysec + " and college_code='" + collegecode + "' and  CC='0' and DelFlag='0' and Exam_Flag<>'debar' order by Roll_No";
                ds.Clear();
                ds.Reset();
                ds = d2.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblSelRollNo.DataSource = ds;
                    cblSelRollNo.DataTextField = "Roll_No";
                    cblSelRollNo.DataValueField = "Reg_No";
                    cblSelRollNo.DataBind();
                    if (cblSelRollNo.Items.Count > 0)
                    {
                        for (int row = 0; row < cblSelRollNo.Items.Count; row++)
                        {
                            cblSelRollNo.Items[row].Selected = true;
                            chkSelRollNo.Checked = true;
                        }
                        txtSelRollNo.Text = "Roll No(" + cblSelRollNo.Items.Count + ")";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Bind Header

    #region Logout

    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Logout

    #region DropDown Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["ChapterCharts"] = null;
            Session["QuestionCharts"] = null;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;
            chkShowSelQuestions.Checked = false;
            txtRollNo.Text = "";

            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            BindRollNo();
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

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["ChapterCharts"] = null;
            Session["QuestionCharts"] = null;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;
            chkShowSelQuestions.Checked = false;
            txtRollNo.Text = "";

            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            BindRollNo();
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

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["ChapterCharts"] = null;
            Session["QuestionCharts"] = null;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;
            chkShowSelQuestions.Checked = false;
            txtRollNo.Text = "";

            bindbranch();
            bindsem();
            BindSectionDetail();
            BindRollNo();
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

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["ChapterCharts"] = null;
            Session["QuestionCharts"] = null;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;
            chkShowSelQuestions.Checked = false;
            txtRollNo.Text = "";

            bindsem();
            BindSectionDetail();
            BindRollNo();
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

    protected void ddlSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["ChapterCharts"] = null;
            Session["QuestionCharts"] = null;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;
            chkShowSelQuestions.Checked = false;
            txtRollNo.Text = "";

            BindSectionDetail();
            BindRollNo();
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

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["ChapterCharts"] = null;
            Session["QuestionCharts"] = null;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;
            chkShowSelQuestions.Checked = false;
            txtRollNo.Text = "";

            BindRollNo();
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

    protected void ddlSubject_Selectchanged(object sender, EventArgs e)
    {
        try
        {
            Session["ChapterCharts"] = null;
            Session["QuestionCharts"] = null;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;
            chkShowSelQuestions.Checked = false;
            txtRollNo.Text = "";

            BindTest();
            bindQuestions();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlTest_Selectchanged(object sender, EventArgs e)
    {
        Session["ChapterCharts"] = null;
        Session["QuestionCharts"] = null;
        rptprint1.Visible = false;
        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        divMainContent.Visible = false;
        chkShowSelQuestions.Checked = false;
        bindQuestions();
        txtRollNo.Text = "";

    }

    protected void chkQuestions_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            Session["ChapterCharts"] = null;
            Session["QuestionCharts"] = null;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;
            txtRollNo.Text = "";

            int count = 0;
            if (chkQuestions.Checked == true)
            {
                count++;
                for (int i = 0; i < cblQuestions.Items.Count; i++)
                {
                    cblQuestions.Items[i].Selected = true;
                }
                txtQuestions.Text = "Question (" + (cblQuestions.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblQuestions.Items.Count; i++)
                {
                    cblQuestions.Items[i].Selected = false;
                }
                txtQuestions.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cblQuestions_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["ChapterCharts"] = null;
            Session["QuestionCharts"] = null;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;
            txtRollNo.Text = "";

            int commcount = 0;
            for (int i = 0; i < cblQuestions.Items.Count; i++)
            {
                if (cblQuestions.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblQuestions.Items.Count)
                {
                    chkQuestions.Checked = true;
                }
                txtQuestions.Text = "Question (" + Convert.ToString(commcount).Trim() + ")";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
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

    protected void cblSelRollNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["ChapterCharts"] = null;
            Session["QuestionCharts"] = null;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;
            txtRollNo.Text = "";

            chkSelRollNo.Checked = false;
            txtSelRollNo.Text = "--- Select ---";

            int count = 0;
            foreach (System.Web.UI.WebControls.ListItem li in cblSelRollNo.Items)
            {
                if (li.Selected)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                if (count == cblSelRollNo.Items.Count)
                {
                    chkSelRollNo.Checked = true;
                }
                txtSelRollNo.Text = "Roll No(" + count + ")";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void chkSelRollNo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            Session["ChapterCharts"] = null;
            Session["QuestionCharts"] = null;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;
            txtRollNo.Text = "";

            int count = 0;

            if (chkSelRollNo.Checked)
            {
                foreach (System.Web.UI.WebControls.ListItem li in cblSelRollNo.Items)
                {
                    li.Selected = true;
                }
                txtSelRollNo.Text = "Roll No(" + cblSelRollNo.Items.Count + ")";
            }
            else
            {
                foreach (System.Web.UI.WebControls.ListItem li in cblSelRollNo.Items)
                {
                    li.Selected = false;
                }
                txtSelRollNo.Text = "--- Select ---";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void rblMultiSingleSelective_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtRollNo.Text = "";
            Session["ChapterCharts"] = null;
            Session["QuestionCharts"] = null;
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;

            RollNo.Attributes.Add("style", "display:none;");
            typeRollNo.Attributes.Add("style", "display:none;");
            tdSelRollNo.Attributes.Add("style", "display:none;");
            tdSelRollNo1.Attributes.Add("style", "display:none;");
            if (rblMultiSingleSelective.SelectedValue.Trim() == "0")
            {
                RollNo.Attributes.Add("style", "display:none;");
                typeRollNo.Attributes.Add("style", "display:none;");
                tdSelRollNo.Attributes.Add("style", "display:none;");
                tdSelRollNo1.Attributes.Add("style", "display:none;");
            }
            else if (rblMultiSingleSelective.SelectedValue.Trim() == "1")
            {
                //autoCmpExtRollNo.ServiceMethod ="GetRollNo(txtRollNo, ddlBatch.SelectedValue, ddlDegree.SelectedValue, ddlSem.SelectedValue)";
                RollNo.Attributes.Add("style", "display:table-cell;");
                typeRollNo.Attributes.Add("style", "display:table-cell;");
                tdSelRollNo.Attributes.Add("style", "display:none;");
                tdSelRollNo1.Attributes.Add("style", "display:none;");
            }
            else if (rblMultiSingleSelective.SelectedValue.Trim() == "2")
            {
                BindRollNo();
                RollNo.Attributes.Add("style", "display:none;");
                typeRollNo.Attributes.Add("style", "display:none;");
                tdSelRollNo.Attributes.Add("style", "display:table-cell;");
                tdSelRollNo1.Attributes.Add("style", "display:table-cell;");
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void txtRollNo_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtRollNo.Text.Trim() != "")
                btnGo_Click(sender, e);
            else
            {
                lblpopuperr.Text = "Please Enter The Roll_No!!!";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion DropDown Events

    #region Button Events

    #region GO BUTTON

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            Session["ChapterCharts"] = null;
            Session["QuestionCharts"] = null;

            int spreadHeight = 0;
            int selQuestionsCount = 0;

            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;

            lblpopuperr.Text = string.Empty;
            pnlQuestions_DMG.Visible = false;
            divMainContent.Visible = false;

            string newroll_No = string.Empty;
            string secval = string.Empty;
            string qrysec = string.Empty;
            string qryInternal1 = string.Empty;
            string qryInternal2 = string.Empty;
            string qryQues = string.Empty;
            string qryRoll_no = string.Empty;

            dtNew.Columns.Clear();
            dtNew.Rows.Clear();
            bool isQuesWiseSucc = false;
            bool isChpterWiseSucc = false;
            bool isIndividualOrMultiStudent = false;

            /// 0 means all students ; 
            /// 1 means only one students ;
            /// 2 means selective Students ;

            int IndividualOrMultiStudent = 0;

            int totalStudents = 0;
            int totalColumns = 0;

            isInternal = true;

            if (ddlCollege.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "School" : "College") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue);
            }

            if (ddlBatch.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Year" : " Batch") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                batch_year = Convert.ToString(ddlBatch.SelectedItem.Text);
            }
            if (ddlDegree.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "School Type" : "Degree") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlBranch.Items.Count != 0)
            {
                degree_code = Convert.ToString(ddlBranch.SelectedValue);
            }
            else
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Standard" : "Department") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            if (ddlSem.Items.Count != 0)
            {
                semester = Convert.ToString(ddlSem.SelectedItem.Text);
            }
            else
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Term" : " Semester") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlSec.Enabled == false || ddlSec.Items.Count == 0)
            {
                section = "";
            }
            else
            {
                section = Convert.ToString(ddlSec.SelectedItem.Text);
                secval = " and qbm.Sections='" + section + "'";
                qrysec = " and Sections='" + section + "'";

            }
            if (ddlSubject.Items.Count != 0)
            {
                subject_no = Convert.ToString(ddlSubject.SelectedValue);
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
                test_name = Convert.ToString(ddlTest.SelectedItem.Text);
                test_no = Convert.ToString(ddlTest.SelectedItem.Value);
            }

            questionid = "";
            int count = 0;
            if (chkShowSelQuestions.Checked && !chkShowSelQuestions.Checked)
            {
                if (cblQuestions.Items.Count != 0)
                {
                    for (int i = 0; i < cblQuestions.Items.Count; i++)
                    {
                        if (cblQuestions.Items[i].Selected == true)
                        {
                            count++;
                            if (questionid == "")
                            {
                                questionid = "'" + Convert.ToString(cblQuestions.Items[i].Value) + "'";
                            }
                            else
                            {
                                questionid = questionid + ",'" + Convert.ToString(cblQuestions.Items[i].Value) + "'";
                            }
                        }
                    }
                }
                else
                {
                    lblpopuperr.Text = "No Questions were Found";
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

            if (rblMultiSingleSelective.SelectedValue.Trim() == "0")
            {
                IndividualOrMultiStudent = 0;
            }
            else if (rblMultiSingleSelective.SelectedValue.Trim() == "1")
            {
                IndividualOrMultiStudent = 1;
                newroll_No = txtRollNo.Text.Trim();
                if (!string.IsNullOrEmpty(newroll_No))
                {
                    string[] allRollNo = newroll_No.Split(',');
                    if (allRollNo.Length > 0)
                    {
                        newroll_No = "";
                        for (int roll = 0; roll < allRollNo.Length; roll++)
                        {
                            if (newroll_No == "")
                            {
                                newroll_No = "'" + allRollNo[roll] + "'";
                            }
                            else
                            {
                                newroll_No += ",'" + allRollNo[roll] + "'";
                            }
                        }
                    }
                    qryRoll_no = " and Roll_No in(" + newroll_No + ")";
                }
                else
                {
                    lblpopuperr.Text = "Please Enter The Roll_No!!!";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            else if (rblMultiSingleSelective.SelectedValue.Trim() == "2")
            {
                IndividualOrMultiStudent = 2;
                int selRollNoCount = 0;
                newroll_No = string.Empty;
                //if (!string.IsNullOrEmpty(newroll_No))
                //{
                //    string[] allRollNo = newroll_No.Split(',');
                //    if (allRollNo.Length > 0)
                //    {
                //        newroll_No = "";
                //        for (int roll = 0; roll < allRollNo.Length; roll++)
                //        {
                //            if (newroll_No == "")
                //            {
                //                newroll_No = "'" + allRollNo[roll] + "'";
                //            }
                //            else
                //            {
                //                newroll_No += ",'" + allRollNo[roll] + "'";
                //            }
                //        }
                //    }

                foreach (System.Web.UI.WebControls.ListItem li in cblSelRollNo.Items)
                {
                    if (li.Selected)
                    {
                        selRollNoCount++;
                        if (newroll_No == "")
                        {
                            newroll_No = "'" + li.Text + "'";
                        }
                        else
                        {
                            newroll_No += ",'" + li.Text + "'";
                        }
                    }
                }
                if (selRollNoCount > 0)
                {
                    qryRoll_no = " and Roll_No in(" + newroll_No + ")";
                }
                else if (cblSelRollNo.Items.Count == 0)
                {
                    lblpopuperr.Text = "No Roll_No Were Found!!!";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
                else if (cblSelRollNo.Items.Count > 0 && selRollNoCount == 0)
                {
                    lblpopuperr.Text = "Please Select Atleast One Roll_No";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }

            //if (section != "")
            //{
            //qry = "select qbq.Questionentryid,qbq.Questions,qbq.qsection_no,sud.unit_name,qbd.Minimu_Attend,qbd.Total_Questions,qbd.Marks,qbq.Syllabus from tbl_question_bank_master qb,tbl_Question_Bank_details qbd,tbl_Question_Bank_Questions qbq,sub_unit_details sud where sud.subject_no=qbq.Subject_no and Convert(nvarchar(150),sud.topic_no)= Convert(nvarchar(150),qbq.Syllabus) and qb.Questionid=qbd.Questionid and qbq.Questionid=qb.Questionid and qbd.qsection_no=qbq.qsection_no and qb.Subject_no=qbd.Subject_no and qbq.Subject_no=qbd.Subject_no and qbd.Subject_no='" + subject_no + "' and Degree_Code='" + degree_code + "' and Batch_year='" + batch_year + "'  and Semester='" + semester + "' and qbq.Questionentryid in (" + questionid + ") " + qrysec + "  order by qbq.qsection_no,qbq.Questionentryid ; select Roll_No,Reg_No,Stud_Name from Registration where CC=0 and DelFlag=0 and Exam_Flag<>'debar' and college_code='" + Convert.ToString(Session["collegecode"]) + "' and Batch_Year='" + batch_year + "' and degree_code='" + degree_code + "' and Current_Semester='" + semester + "' " + qrysec + "  order by Roll_No ; select qm.Questionentryid,qbq.Questions,qbq.qsection_no,sud.topic_no,sud.unit_name,r.serialno,r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,qm.mark_obtained,qbd.Minimu_Attend,qbd.Total_Questions,qbd.Marks from  Registration r,questionwise_marksentry qm,tbl_Question_Bank_Questions qbq,tbl_question_bank_master qbm,tbl_Question_Bank_details qbd,sub_unit_details sud where r.Roll_No=qm.roll_no and r.degree_code=qbm.Degree_Code and qbm.Batch_year=r.Batch_Year and qbm.Semester=r.Current_Semester and qbm.Sections=r.Sections and qm.subject_no=sud.subject_no and sud.subject_no=qbq.Subject_no and qbm.Subject_no=qbq.Subject_no and Convert(nvarchar(150),sud.topic_no)= Convert(nvarchar(150),qbq.Syllabus) and qbd.Questionid=qbm.Questionid and qbd.qsection_no=qbq.qsection_no and qbd.Subject_no=qm.subject_no and qbq.Questionentryid=qm.Questionentryid and CC=0 and DelFlag=0 and Exam_Flag<>'debar'  and r.college_code='" + Convert.ToString(Session["collegecode"]) + "' and r.Batch_Year='" + batch_year + "' and qm.Questionentryid in (" + questionid + ") and r.degree_code='" + degree_code + "' and r.Current_Semester='" + semester + "' and qm.subject_no='" + subject_no + "'  " + secval + " order by r.Roll_No,qbq.qsection_no,qm.Questionentryid ; select distinct Syllabus,sud.unit_name from  tbl_Question_Bank_Questions qbq,tbl_question_bank_master qbm,tbl_Question_Bank_details qbd,sub_unit_details sud where  qbq.subject_no=sud.subject_no and sud.subject_no=qbq.Subject_no and qbm.Subject_no=qbq.Subject_no and Convert(nvarchar(150),sud.topic_no)= Convert(nvarchar(150),qbq.Syllabus) and qbd.Questionid=qbm.Questionid  and qbd.Questionid=qbq.questionid and qbm.Batch_Year='" + batch_year + "' and qbq.Questionentryid in (" + questionid + ") and qbm.degree_code='" + degree_code + "' and qbm.Semester='" + semester + "' and qbq.subject_no='" + subject_no + "' " + secval + " order by Syllabus ;";
            //}
            //else and Convert(nvarchar(150),qbd.qSection)=Convert(nvarchar(150),qbq.qsection_no) and Convert(nvarchar(150),qbd.Subject_no)=Convert(nvarchar(150),qm.subject_no)
            //{
            //qry = "select qbq.Questionentryid,qbq.Questions,qbq.qsection_no,sud.unit_name,qbd.Minimu_Attend,qbd.Total_Questions,qbd.Marks,qbq.Syllabus from tbl_question_bank_master qb,tbl_Question_Bank_details qbd,tbl_Question_Bank_Questions qbq,sub_unit_details sud where sud.subject_no=qbq.Subject_no and Convert(nvarchar(150),sud.topic_no)= Convert(nvarchar(150),qbq.Syllabus) and qb.Questionid=qbd.Questionid and qbq.Questionid=qb.Questionid and qbd.qsection_no=qbq.qsection_no and qb.Subject_no=qbd.Subject_no and qbq.Subject_no=qbd.Subject_no   and qbd.Subject_no='" + subject_no + "' and Degree_Code='" + degree_code + "' and Batch_year='" + batch_year + "'  and Semester='" + semester + "' and qbq.Questionentryid in (" + questionid + ") order by qbq.qsection_no,qbq.Questionentryid; select Roll_No,Reg_No,Stud_Name from Registration where CC=0 and DelFlag=0 and Exam_Flag<>'debar' and college_code='" + Convert.ToString(Session["collegecode"]) + "' and Batch_Year='" + batch_year + "' and degree_code='" + degree_code + "' and Current_Semester='" + semester + "' order by Roll_No ; select qm.Questionentryid,qbq.Questions,qbq.qsection_no,sud.topic_no,sud.unit_name,r.serialno,r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,qm.mark_obtained,qbd.Minimu_Attend,qbd.Total_Questions,qbd.Marks from  Registration r,questionwise_marksentry qm,tbl_Question_Bank_Questions qbq,tbl_question_bank_master qbm,tbl_Question_Bank_details qbd,sub_unit_details sud where r.Roll_No=qm.roll_no and r.degree_code=qbm.Degree_Code and qbm.Batch_year=r.Batch_Year and qbm.Semester=r.Current_Semester and qbm.Sections=r.Sections and qm.subject_no=sud.subject_no and sud.subject_no=qbq.Subject_no and qbm.Subject_no=qbq.Subject_no and Convert(nvarchar(150),sud.topic_no)= Convert(nvarchar(150),qbq.Syllabus) and qbd.Questionid=qbm.Questionid and qbd.qSection=qbq.qsection_no and qbd.Subject_no=qm.subject_no and qbq.Questionentryid=qm.Questionentryid and CC=0 and DelFlag=0 and Exam_Flag<>'debar'  and r.college_code='" + Convert.ToString(Session["collegecode"]) + "' and r.Batch_Year='" + batch_year + "' and qm.Questionentryid in (" + questionid + ") and r.degree_code='" + degree_code + "' and r.Current_Semester='" + semester + "' and qm.subject_no='" + subject_no + "' order by r.Roll_No,qbq.qsection_no,qm.Questionentryid ; select distinct Syllabus,sud.unit_name from  tbl_Question_Bank_Questions qbq,tbl_question_bank_master qbm,tbl_Question_Bank_details qbd,sub_unit_details sud where  qbq.subject_no=sud.subject_no and sud.subject_no=qbq.Subject_no and qbm.Subject_no=qbq.Subject_no and Convert(nvarchar(150),sud.topic_no)= Convert(nvarchar(150),qbq.Syllabus) and qbd.Questionid=qbm.Questionid  and qbd.Questionid=qbq.questionid and qbm.Batch_Year='" + batch_year + "' and qbq.Questionentryid in (" + questionid + ") and qbm.degree_code='" + degree_code + "' and qbm.Semester='" + semester + "' and qbq.subject_no='" + subject_no + "' order by Syllabus ; ";
            //}

            if (isInternal)
            {
                qryInternal = " and exq.Test_code=qbm.Exam and exq.Test_code='" + test_no + "' and exq.is_internal=2 " + qryQues + " ";//and exq.QuestionMasterFK in (" + questionid + ")";
                qryInternal1 = " and qbm.exam_type=qwm.isinternal and qwm.isinternal=exq.is_internal and Convert(nvarchar(100),qwm.criteria_no)=qbm.Exam and qwm.criteria_no=exq.Test_code and exq.Test_code=qbm.Exam and exq.Test_code='" + test_no + "' and exq.is_internal='2'  " + qryQues + " ";//and exq.QuestionMasterFK in (" + questionid + ")";

            }

            qry = "select exq.Test_code,exq.Exist_questionPK,qm.QuestionMasterPK,qm.question,sud.topic_no,exq.Must_attend,sud.unit_name,exq.Section,exq.section_name,qm.mark,qm.subject_no from tbl_question_bank_master qbm,tbl_question_master qm, Exist_questions exq,sub_unit_details sud where qbm.Subject_no=qm.subject_no and qm.subject_no=exq.subject_no and exq.subject_no=qbm.Subject_no and qm.syllabus=exq.syllabus and qbm.exam_type=exq.is_internal and exq.QuestionMasterFK=qm.QuestionMasterPK and Batch_year='" + batch_year + "' and Degree_Code='" + degree_code + "' and Semester='" + semester + "' and qbm.Subject_no='" + subject_no + "' " + qrysec + " and sud.subject_no=qbm.Subject_no and sud.subject_no=qm.subject_no and sud.subject_no=exq.subject_no and sud.topic_no=exq.syllabus and qm.syllabus=sud.topic_no " + qryInternal + "  order by exq.Test_code,exq.subject_no,exq.Exist_questionPK,exq.Section,qm.QuestionMasterPK,sud.topic_no ; select Roll_No,Reg_No,Stud_Name from Registration where CC=0 and DelFlag=0 and Exam_Flag<>'debar' and college_code='" + collegecode + "' and Batch_Year='" + batch_year + "' and degree_code='" + degree_code + "' and Current_Semester='" + semester + "'  " + qrysec + qryRoll_no + "  order by Roll_No ; select r.serialno,r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,r.Batch_Year,r.degree_code,r.Current_Semester,qbm.Semester,r.Sections,exq.Test_code,exq.subject_no,sud.topic_no,sud.unit_name,exq.is_internal,exq.Exam_month,exq.Exam_year,exq.Exist_questionPK,qm.QuestionMasterPK,qm.question,qm.answer,qm.is_descriptive,qm.is_matching,qm.mark as Max_Mark,qm.qmatching,qm.options,qm.type,exq.Must_attend,exq.Section as Questions_Section,exq.section_name,qwm.mark_obtained,Convert(nvarchar(30),qbm.exam_date,103) as Exam_Date from tbl_question_bank_master qbm,tbl_question_master qm, Exist_questions exq,Registration r,sub_unit_details sud,questionwise_marksentry qwm where qbm.Subject_no=qm.subject_no and qm.subject_no=exq.subject_no and exq.subject_no=qbm.Subject_no and qm.syllabus=exq.syllabus and qbm.exam_type=exq.is_internal and exq.QuestionMasterFK=qm.QuestionMasterPK and r.degree_code=qbm.Degree_Code and r.Batch_Year=qbm.Batch_year and r.Sections=qbm.Sections and r.Roll_No=qwm.roll_no and qwm.subject_no=qm.subject_no and qwm.subject_no=qbm.Subject_no and qwm.Subject_no=exq.subject_no and sud.subject_no=qwm.subject_no and sud.subject_no=qbm.Subject_no and sud.subject_no=qm.subject_no and exq.subject_no=sud.subject_no and sud.topic_no=qm.syllabus and sud.topic_no=exq.syllabus and qwm.Questionentryid=qm.QuestionMasterPK and qwm.Questionentryid=exq.QuestionMasterFK and r.college_code='" + collegecode + "' and qbm.Batch_year='" + batch_year + "' and qbm.Degree_Code='" + degree_code + "' and qbm.Semester='" + semester + "' and qbm.Subject_no='" + subject_no + "' " + secval + " and r.CC=0 and r.Exam_Flag<>'debar' and DelFlag=0 " + qryInternal1 + " order by r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,r.Roll_No,r.Reg_No,exq.Test_code,exq.subject_no,exq.Exist_questionPK,exq.Section,qm.QuestionMasterPK,sud.topic_no ; select distinct sud.topic_no,sud.unit_name from tbl_question_bank_master qbm,tbl_question_master qm, Exist_questions exq,sub_unit_details sud where qbm.Subject_no=qm.subject_no and qm.subject_no=exq.subject_no and exq.subject_no=qbm.Subject_no and qm.syllabus=exq.syllabus and qbm.exam_type=exq.is_internal and exq.QuestionMasterFK=qm.QuestionMasterPK and Batch_year='" + batch_year + "' and Degree_Code='" + degree_code + "' and Semester='" + semester + "' and qbm.Subject_no='" + subject_no + "' " + qrysec + " and sud.subject_no=qbm.Subject_no and sud.subject_no=qm.subject_no and sud.subject_no=exq.subject_no and sud.topic_no=exq.syllabus and qm.syllabus=sud.topic_no " + qryInternal + " order by sud.topic_no ; select ROW_NUMBER() OVER (ORDER BY exq.Exist_questionPK,exq.Section,QuestionMasterPK) as QNo,exq.Exist_questionPK,qm.QuestionMasterPK,qm.question,exq.Section,qm.mark  from tbl_question_bank_master qbm,tbl_question_master qm, Exist_questions exq,sub_unit_details sud where qbm.Subject_no=qm.subject_no and qm.subject_no=exq.subject_no and exq.subject_no=qbm.Subject_no and qm.syllabus=exq.syllabus and qbm.exam_type=exq.is_internal and exq.QuestionMasterFK=qm.QuestionMasterPK and sud.subject_no=qbm.Subject_no and sud.subject_no=qm.subject_no and exq.subject_no=sud.subject_no and sud.topic_no=qm.syllabus and sud.topic_no=exq.syllabus and qbm.Batch_year='" + batch_year + "' and qbm.Degree_Code='" + degree_code + "' and qbm.Semester='" + semester + "' and qbm.Subject_no='" + subject_no + "' " + qrysec + " and exq.Test_code=qbm.Exam and exq.Test_code='" + test_no + "' and exq.is_internal=2 order by exq.Exist_questionPK,exq.Section,qm.QuestionMasterPK ;";

            DataSet dsSelStud = d2.select_method_wo_parameter("select Roll_No,Reg_No,Stud_Name from Registration where CC=0 and DelFlag=0 and Exam_Flag<>'debar' and college_code='" + collegecode + "' and Batch_Year='" + batch_year + "' and degree_code='" + degree_code + "' and Current_Semester='" + semester + "'  " + qrysec + "  order by Roll_No", "text");
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

            if (ds.Tables.Count > 0)
            {
                string newpath = Server.MapPath("~/Image/");
                DataTable dtChapterWiseClassDMG = new DataTable();
                DataTable dtChapterWiseStudDMG = new DataTable();
                DataTable dtQuestionWiseClassDMG = new DataTable();
                DataTable dtQuestionWiseStudDMG = new DataTable();
                isQuesWiseSucc = false;
                isChpterWiseSucc = false;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    degree_code = Convert.ToString(ddlBranch.SelectedValue).Trim();
                    batch_year = Convert.ToString(ddlBatch.SelectedValue).Trim();
                    string current_sem = Convert.ToString(ddlSem.SelectedValue).Trim();
                    string branch = Convert.ToString(ddlBranch.SelectedItem).Trim();
                    section = string.Empty;

                    if (ddlSec.Items.Count > 0)
                    {
                        if (Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "all")
                        {
                            section = "&nbsp;-&nbsp;" + Convert.ToString(ddlSec.SelectedValue).Trim().ToUpper();
                        }
                        else
                        {
                            section = "";
                        }
                    }

                    string degreedetails = "";
                    degreedetails = Convert.ToString(branch).Trim().ToUpper() + "&nbsp;" + section + "&nbsp;(" + ((isSchool) ? "YEAR" : "BATCH") + "&nbsp;" + Convert.ToString(batch_year).Trim() + ")&nbsp;" + ((isSchool) ? "TERM" : "SEM") + "&nbsp;-&nbsp;" + Convert.ToString(current_sem).Trim();

                    DataSet dscol = d2.select_method_wo_parameter("Select collname,address1,address2,address3,category,university from Collinfo where college_code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "' ", "Text");
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

                    spanSub.Text = "Subject Name : " + Convert.ToString(ddlSubject.SelectedItem.Text);
                    spanSub.Style.Add("text-decoration", "none");
                    spanSub.Style.Add("font-family", "Book Antiqua;");
                    spanSub.Style.Add("font-size", "18px");
                    spanSub.Style.Add("text-align", "left");

                    if (ds.Tables.Count >= 2 && ds.Tables[1].Rows.Count > 0)
                    {
                        totalStudents = ds.Tables[1].Rows.Count;
                        plhChapterWise.Controls.Clear();
                        plhQuestionWise.Controls.Clear();
                        Array.Resize(ref chartChapterDMG, totalStudents);
                        Array.Resize(ref chartQuestionDMG, totalStudents);

                        totalColumns = (totalStudents * 3) + 1;

                        if (ds.Tables.Count >= 3 && ds.Tables[2].Rows.Count > 0 && dsSelStud.Tables.Count > 0 && dsSelStud.Tables[0].Rows.Count > 0)
                        {
                            QuestionWiseClassDMG(ds.Tables[4], ds.Tables[0], dsSelStud.Tables[0], ds.Tables[2], ref dtQuestionWiseClassDMG, ref dtQuestionWiseStudDMG);
                            isQuesWiseSucc = true;
                            isChpterWiseSucc = false;
                        }
                        else
                        {
                            isQuesWiseSucc = false;
                            isChpterWiseSucc = false;
                            lblpopuperr.Text = "No Student's Marks Were Found";
                            lblpopuperr.Visible = true;
                            popupdiv.Visible = true;
                            return;
                        }
                        if (ds.Tables.Count >= 4 && ds.Tables[3].Rows.Count > 0)
                        {
                            if (ds.Tables.Count >= 3 && ds.Tables[2].Rows.Count > 0 && dsSelStud.Tables.Count > 0 && dsSelStud.Tables[0].Rows.Count > 0)
                            {
                                ChapterWiseClassDMG(ds.Tables[3], dsSelStud.Tables[0], ds.Tables[2], ref dtChapterWiseClassDMG, ref dtChapterWiseStudDMG);
                                QuestionWiseClassDMG(ds.Tables[4], ds.Tables[0], dsSelStud.Tables[0], ds.Tables[2], ref dtQuestionWiseClassDMG, ref dtQuestionWiseStudDMG);
                                ChapterWiseDMG(ds.Tables[3], dtQuestionWiseClassDMG, ref dtChapterWiseClassDMG);
                                isQuesWiseSucc = true;
                                isChpterWiseSucc = true;
                            }
                            else
                            {
                                isChpterWiseSucc = false;
                                lblpopuperr.Text = "No Student's Marks Were Found";
                                lblpopuperr.Visible = true;
                                popupdiv.Visible = true;
                                return;
                            }
                        }
                        else
                        {
                            lblpopuperr.Text = "No Subject's Chapters or Syllabus Were Found";
                            lblpopuperr.Visible = true;
                            popupdiv.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        lblpopuperr.Text = "No Students Were Found";
                        lblpopuperr.Visible = true;
                        popupdiv.Visible = true;
                        return;
                    }
                    if (isChpterWiseSucc)
                    {
                        Init_Spread();
                        FpSpreadChapterWiseDMG.Sheets[0].ColumnCount = 0;
                        FpSpreadChapterWiseDMG.Sheets[0].ColumnCount++;
                        FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpreadChapterWiseDMG.Sheets[0].Columns[0].Width = 40;
                        FpSpreadChapterWiseDMG.Sheets[0].Columns[0].Locked = true;
                        //FpSpreadChapterWiseDMG.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1); Roll_No,Reg_No,Stud_Name

                        dtNew.Columns.Add("Sno");
                        if (ds.Tables[3].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0 && dtChapterWiseClassDMG.Rows.Count > 0 && dtChapterWiseStudDMG.Rows.Count > 0)
                        {
                            for (int stud = 0; stud < ds.Tables[1].Rows.Count; stud++)
                            {
                                DataRow drNew;
                                FpSpreadChapterWiseDMG.Sheets[0].ColumnCount += 3;
                                int rows = 0;
                                string studendName = Convert.ToString(ds.Tables[1].Rows[stud]["Stud_Name"]);
                                string studendRollNo = Convert.ToString(ds.Tables[1].Rows[stud]["Roll_No"]);
                                string studendRegNo = Convert.ToString(ds.Tables[1].Rows[stud]["Reg_No"]);

                                int index = plhChapterWise.Controls.OfType<Chart>().ToList().Count + 1;
                                chartChapterDMG[stud] = new Chart();
                                chartChapterDMG[stud].ID = "chartChapterDMG" + index;
                                //chartChapterDMG[stud].EnableViewState = true;

                                //int index = plhChapterWise.Controls.OfType<Chart>().ToList().Count + 1;
                                //this.CreateChapterControls("chartChapterDMG" + index);

                                int index1 = plhQuestionWise.Controls.OfType<Chart>().ToList().Count + 1;
                                //pnlTextBoxes.Controls.OfType<TextBox>().ToList().Count + 1;
                                chartQuestionDMG[stud] = new Chart();
                                chartQuestionDMG[stud].ID = "chartQuestionDMG" + index1;

                                //chartQuestionDMG[stud].EnableViewState = true;
                                //chartChapterDMG[stud].EnableViewState = true;

                                dtStudChapterDMG.Rows.Clear();
                                dtStudChapterDMG.Columns.Clear();
                                dtStudChapterDMG.Columns.Add("ChapterName");
                                dtStudChapterDMG.Columns.Add("ClassDMG");
                                dtStudChapterDMG.Columns.Add("StudentDMG");

                                DataRow drChapter;
                                DataRow drQuestion;

                                dtStudQuestionDMG.Rows.Clear();
                                dtStudQuestionDMG.Columns.Clear();
                                dtStudQuestionDMG.Columns.Add("QuestionNo");
                                dtStudQuestionDMG.Columns.Add("QuestionName");
                                dtStudQuestionDMG.Columns.Add("ClassDMG");
                                dtStudQuestionDMG.Columns.Add("StudentDMG");


                                //DataTable dtClassChapterDMG = new DataTable();
                                //DataTable dtClassQuestionDMG = new DataTable();

                                chartChapterDMG[stud].Series.Clear();
                                chartChapterDMG[stud].Titles.Clear();
                                chartChapterDMG[stud].Legends.Clear();
                                chartChapterDMG[stud].Series.Add(studendName.Trim() + " DMG");
                                chartChapterDMG[stud].Series.Add("CHDMG");
                                Title title = new Title(studendName + " Chapter DMG Analyisis", Docking.Top, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), System.Drawing.Color.Black);
                                chartChapterDMG[stud].Titles.Add(title);
                                chartChapterDMG[stud].Legends.Add(studendName.Trim() + " DMG");
                                chartChapterDMG[stud].Legends.Add("CHDMG");
                                chartChapterDMG[stud].Legends[0].Alignment = StringAlignment.Center;
                                chartChapterDMG[stud].Legends[1].Alignment = StringAlignment.Center;
                                chartChapterDMG[stud].Legends[0].Docking = Docking.Bottom;
                                chartChapterDMG[stud].Legends[1].Docking = Docking.Bottom;
                                chartChapterDMG[stud].ChartAreas.Clear();
                                chartChapterDMG[stud].ChartAreas.Add("ChapterWise");
                                chartChapterDMG[stud].Width = 600;
                                chartChapterDMG[stud].RenderType = RenderType.ImageTag;
                                chartChapterDMG[stud].ImageType = ChartImageType.Png;
                                chartChapterDMG[stud].ImageStorageMode = ImageStorageMode.UseImageLocation;


                                //string imgPath = HttpContext.Current.Request.PhysicalApplicationPath + "image\\" + "chartChapterDMG" + index;
                                chartChapterDMG[stud].ImageLocation = Path.Combine("~/Image/", "chartChapterDMG" + index); //imgPath;// Server.MapPath("~/image/" + "chartChapterDMG" + index);
                                //chartChapterDMG[stud].SaveImage(imgPath);//Server.MapPath("~/image/" + "chartChapterDMG" + index));

                                chartQuestionDMG[stud].Series.Clear();
                                chartQuestionDMG[stud].Titles.Clear();
                                chartQuestionDMG[stud].Legends.Clear();
                                chartQuestionDMG[stud].Series.Add(studendName.Trim() + " DMG");
                                chartQuestionDMG[stud].Series.Add("CLADMG");
                                title = new Title(studendName + " Question DMG Analyisis", Docking.Top, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), System.Drawing.Color.Black);
                                chartQuestionDMG[stud].Titles.Add(title);
                                chartQuestionDMG[stud].Legends.Add(studendName.Trim() + " DMG");
                                chartQuestionDMG[stud].Legends.Add("CLADMG");
                                //chartQuestionDMG[stud].Legends[0].BackColor = System.Drawing.Color.Green;
                                chartQuestionDMG[stud].Legends[0].Alignment = StringAlignment.Center;
                                chartQuestionDMG[stud].Legends[1].Alignment = StringAlignment.Center;
                                chartQuestionDMG[stud].Legends[0].Docking = Docking.Bottom;
                                chartQuestionDMG[stud].Legends[1].Docking = Docking.Bottom;

                                chartQuestionDMG[stud].ChartAreas.Clear();
                                chartQuestionDMG[stud].ChartAreas.Add("QuestionWise");
                                chartQuestionDMG[stud].Width = 600;
                                chartQuestionDMG[stud].RenderType = RenderType.ImageTag;
                                chartQuestionDMG[stud].ImageType = ChartImageType.Png;
                                //imgPath = HttpContext.Current.Request.PhysicalApplicationPath + "image\\" + "chartQuestionDMG" + index1;
                                ////ChartChapterWiseDmg.Width = (dtChapWise.Rows.Count * 60) + 100;
                                //ChartChapterWiseDmg.SaveImage(imgPath);
                                chartQuestionDMG[stud].ImageStorageMode = ImageStorageMode.UseImageLocation;
                                chartQuestionDMG[stud].ImageLocation = Path.Combine("~/Image/" + "chartQuestionDMG" + index); //imgPath;
                                //chartQuestionDMG[stud].SaveImage(imgPath);

                                FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.Cells[0, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 3].Text = "Chapter Name";
                                dtNew.Columns.Add("Chapter Name" + (stud + 1));
                                FpSpreadChapterWiseDMG.Sheets[0].Columns[FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 3].Width = 40;
                                FpSpreadChapterWiseDMG.Sheets[0].Columns[FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 3].Locked = true;

                                FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.Cells[0, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].Text = "CHDMG";
                                dtNew.Columns.Add("CHDMG" + (stud + 1));
                                FpSpreadChapterWiseDMG.Sheets[0].Columns[FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].Width = 40;
                                FpSpreadChapterWiseDMG.Sheets[0].Columns[FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].Locked = true;

                                FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.Cells[0, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Text = studendName.Trim() + " DMG";
                                dtNew.Columns.Add(studendName.Trim() + " DMG");
                                FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.Cells[0, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Tag = studendRollNo;
                                FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.Cells[0, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Note = studendRegNo;
                                FpSpreadChapterWiseDMG.Sheets[0].Columns[FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Width = 40;
                                FpSpreadChapterWiseDMG.Sheets[0].Columns[FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Locked = true;

                                for (int chapter = 0; chapter < ds.Tables[3].Rows.Count; chapter++)
                                {
                                    string chaptername = Convert.ToString(ds.Tables[3].Rows[chapter]["unit_name"]);
                                    string chapterNo = Convert.ToString(ds.Tables[3].Rows[chapter]["topic_no"]);

                                    DataView dvClassDMG = new DataView();
                                    DataView dvClassStudDMG = new DataView();

                                    drChapter = dtStudChapterDMG.NewRow();
                                    drChapter["ChapterName"] = chaptername;
                                    if (stud == 0)
                                    {
                                        FpSpreadChapterWiseDMG.Sheets[0].RowCount++;
                                        drNew = dtNew.NewRow();
                                        FpSpreadChapterWiseDMG.Sheets[0].Cells[FpSpreadChapterWiseDMG.Sheets[0].RowCount - 1, 0].Text = Convert.ToString((chapter + 1));
                                        drNew["SNo"] = Convert.ToString((chapter + 1));
                                        FpSpreadChapterWiseDMG.Sheets[0].Cells[FpSpreadChapterWiseDMG.Sheets[0].RowCount - 1, 0].Locked = true;
                                        FpSpreadChapterWiseDMG.Sheets[0].Cells[FpSpreadChapterWiseDMG.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        FpSpreadChapterWiseDMG.Sheets[0].Cells[FpSpreadChapterWiseDMG.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpreadChapterWiseDMG.Sheets[0].Cells[FpSpreadChapterWiseDMG.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                        dtNew.Rows.Add(drNew);
                                    }
                                    dtNew.Rows[chapter]["Chapter Name" + (stud + 1)] = chaptername.Trim();
                                    FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 3].Text = chaptername.Trim();
                                    FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 3].Font.Name = "Book Antiqua";
                                    FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 3].Locked = true;
                                    FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 3].Font.Bold = false;
                                    FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;

                                    if (dtChapterWiseClassDMG.Rows.Count > 0)
                                    {
                                        dtChapterWiseClassDMG.DefaultView.RowFilter = "Chapter_No='" + chapterNo + "'";
                                        dvClassDMG = dtChapterWiseClassDMG.DefaultView;
                                        if (dvClassDMG.Count > 0)//
                                        {
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].Text = Convert.ToString(dvClassDMG[0]["Chapter_DMG"]);
                                            dtNew.Rows[chapter]["CHDMG" + (stud + 1)] = Convert.ToString(dvClassDMG[0]["Chapter_DMG"]);
                                            drChapter["ClassDMG"] = Convert.ToString(dvClassDMG[0]["Chapter_DMG"]);
                                        }
                                    }
                                    FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].Locked = true;
                                    FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                    FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].Font.Bold = false;
                                    FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;

                                    if (dtChapterWiseStudDMG.Rows.Count > 0)
                                    {
                                        dtChapterWiseStudDMG.DefaultView.RowFilter = "Chapter_No='" + chapterNo + "' and Roll_No='" + studendRollNo + "'";
                                        dvClassStudDMG = dtChapterWiseStudDMG.DefaultView;
                                        if (dvClassStudDMG.Count > 0)
                                        {
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dvClassStudDMG[0]["Chapter_DMG"]);
                                            dtNew.Rows[chapter][studendName.Trim() + " DMG"] = Convert.ToString(dvClassDMG[0]["Chapter_DMG"]);
                                            drChapter["StudentDMG"] = Convert.ToString(dvClassStudDMG[0]["Chapter_DMG"]);
                                        }
                                    }
                                    FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Locked = true;
                                    FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Font.Bold = false;
                                    FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadChapterWiseDMG.Sheets[0].Cells[chapter, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                    dtStudChapterDMG.Rows.Add(drChapter);
                                }
                                rows = ds.Tables[3].Rows.Count;
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int question = 0; question < ds.Tables[0].Rows.Count; question++)
                                    {
                                        DataView dvClassQuesDMG = new DataView();
                                        DataView dvClassQuesStudDMG = new DataView();
                                        string questionNo = Convert.ToString(ds.Tables[0].Rows[question]["QuestionMasterPK"]);
                                        string questionName = Convert.ToString(ds.Tables[0].Rows[question]["question"]);
                                        drQuestion = dtStudQuestionDMG.NewRow();

                                        DataTable dtNewQues = new DataTable();
                                        if (ds.Tables.Count >= 5 && ds.Tables[4].Rows.Count > 0)
                                        {
                                            DataView dv = new DataView();
                                            ds.Tables[4].DefaultView.RowFilter = "QuestionMasterPK='" + questionNo + "'";
                                            dv = ds.Tables[4].DefaultView;
                                            dtNewQues = dv.ToTable(true, "QNo", "Section", "mark");
                                        }
                                        if (dtNewQues.Rows.Count > 0)
                                        {
                                            drQuestion["QuestionNo"] = Convert.ToString((dtNewQues.Rows[0]["QNo"]));
                                        }
                                        else
                                        {
                                            drQuestion["QuestionNo"] = Convert.ToString(question + 1);
                                        }
                                        //drQuestion["QuestionNo"] = Convert.ToString(question + 1);
                                        drQuestion["QuestionName"] = Convert.ToString(ds.Tables[0].Rows[question]["question"]);

                                        if (question == 0)
                                        {
                                            if (stud == 0)
                                            {
                                                FpSpreadChapterWiseDMG.Sheets[0].RowCount++;
                                                drNew = dtNew.NewRow();
                                                FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, 0].Text = Convert.ToString("QNo");
                                                drNew[0] = Convert.ToString("QNo");
                                                dtNew.Rows.Add(drNew);
                                                FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, 0].Locked = true;
                                                FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, 0].Font.Bold = true;
                                                FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, 0].Font.Name = "Book Antiqua";
                                                FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, 0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, 0].VerticalAlign = VerticalAlign.Middle;
                                            }

                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].Text = "CLADMG";
                                            dtNew.Rows[rows]["CHDMG" + (stud + 1)] = "CLADMG";
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].Locked = true;
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;

                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Text = studendName.Trim() + " DMG";
                                            dtNew.Rows[rows][studendName.Trim() + " DMG"] = studendName.Trim() + " DMG";
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Locked = true;
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                        }

                                        if (stud == 0)
                                        {
                                            FpSpreadChapterWiseDMG.Sheets[0].RowCount++;
                                            drNew = dtNew.NewRow();
                                            dtNewQues = new DataTable();
                                            if (ds.Tables.Count >= 5 && ds.Tables[4].Rows.Count > 0)
                                            {
                                                DataView dv = new DataView();
                                                ds.Tables[4].DefaultView.RowFilter = "QuestionMasterPK='" + questionNo + "'";
                                                dv = ds.Tables[4].DefaultView;
                                                dtNewQues = dv.ToTable(true, "QNo", "Section", "mark");
                                            }
                                            if (dtNewQues.Rows.Count > 0)
                                            {
                                                drNew[0] = Convert.ToString((dtNewQues.Rows[0]["QNo"]));
                                                FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, 0].Text = Convert.ToString((dtNewQues.Rows[0]["QNo"]));
                                            }
                                            else
                                            {
                                                drNew[0] = Convert.ToString((question + 1));
                                                FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, 0].Text = Convert.ToString((question + 1));
                                            }

                                            dtNew.Rows.Add(drNew);
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, 0].Locked = true;
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, 0].Font.Name = "Book Antiqua";
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, 0].Font.Bold = false;
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, 0].VerticalAlign = VerticalAlign.Middle;
                                        }

                                        if (dtQuestionWiseClassDMG.Rows.Count > 0)
                                        {
                                            dtQuestionWiseClassDMG.DefaultView.RowFilter = "Question_No='" + questionNo + "'";
                                            dvClassQuesDMG = dtQuestionWiseClassDMG.DefaultView;
                                            if (dvClassQuesDMG.Count > 0)//
                                            {
                                                FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].Text = Convert.ToString(dvClassQuesDMG[0]["Question_DMG"]);
                                                dtNew.Rows[rows + question + 1]["CHDMG" + (stud + 1)] = Convert.ToString(dvClassQuesDMG[0]["Question_DMG"]);
                                                drQuestion["ClassDMG"] = Convert.ToString(dvClassQuesDMG[0]["Question_DMG"]);
                                            }
                                        }

                                        FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                        FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].Locked = true;
                                        FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].Font.Bold = false;
                                        FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
                                        if (dtQuestionWiseStudDMG.Rows.Count > 0)
                                        {
                                            dtQuestionWiseStudDMG.DefaultView.RowFilter = "Question_No='" + questionNo + "' and Roll_No='" + studendRollNo + "'";
                                            dvClassQuesStudDMG = dtQuestionWiseStudDMG.DefaultView;
                                            if (dvClassQuesStudDMG.Count > 0)
                                            {
                                                FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dvClassQuesStudDMG[0]["Question_DMG"]);
                                                dtNew.Rows[rows + question + 1][studendName.Trim() + " DMG"] = Convert.ToString(dvClassQuesDMG[0]["Question_DMG"]);
                                                drQuestion["StudentDMG"] = Convert.ToString(dvClassQuesStudDMG[0]["Question_DMG"]);
                                            }
                                        }

                                        FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Locked = true;
                                        FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].Font.Bold = false;
                                        FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpreadChapterWiseDMG.Sheets[0].Cells[rows + question + 1, FpSpreadChapterWiseDMG.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                        dtStudQuestionDMG.Rows.Add(drQuestion);
                                    }
                                }

                                if (dtStudChapterDMG.Rows.Count > 0)
                                {
                                    for (int chp = 0; chp < dtStudChapterDMG.Rows.Count; chp++)
                                    {
                                        chartChapterDMG[stud].Series[0].Points.AddXY(Convert.ToString(dtStudChapterDMG.Rows[chp]["ChapterName"]), Convert.ToString(dtStudChapterDMG.Rows[chp]["StudentDMG"]));
                                        chartChapterDMG[stud].Series[1].Points.AddXY(Convert.ToString(dtStudChapterDMG.Rows[chp]["ChapterName"]), Convert.ToString(dtStudChapterDMG.Rows[chp]["ClassDMG"]));
                                        chartChapterDMG[stud].ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                        chartChapterDMG[stud].ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                                        chartChapterDMG[stud].Series[0].IsValueShownAsLabel = true;
                                        chartChapterDMG[stud].Series[0].IsXValueIndexed = true;

                                        //chartChapterDMG[stud].Series[0].Color = System.Drawing.Color.Red;
                                        //chartChapterDMG[stud].Series[1].Color = System.Drawing.Color.DarkBlue;

                                        chartChapterDMG[stud].Series[1].IsValueShownAsLabel = true;
                                        chartChapterDMG[stud].Series[1].IsXValueIndexed = true;

                                        chartChapterDMG[stud].ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                        chartChapterDMG[stud].ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                        chartChapterDMG[stud].ChartAreas[0].AxisY.Maximum = 100;
                                        chartChapterDMG[stud].ChartAreas[0].AxisY.Minimum = 0;
                                    }
                                    plhChapterWise.Controls.Add(chartChapterDMG[stud]);
                                }

                                if (dtStudQuestionDMG.Rows.Count > 0)
                                {
                                    for (int chp = 0; chp < dtStudQuestionDMG.Rows.Count; chp++)
                                    {
                                        chartQuestionDMG[stud].Series[0].Points.AddXY(Convert.ToString(dtStudQuestionDMG.Rows[chp]["QuestionNo"]), Convert.ToString(dtStudQuestionDMG.Rows[chp]["StudentDMG"]));
                                        chartQuestionDMG[stud].Series[1].Points.AddXY(Convert.ToString(dtStudQuestionDMG.Rows[chp]["QuestionNo"]), Convert.ToString(dtStudQuestionDMG.Rows[chp]["ClassDMG"]));
                                        chartQuestionDMG[stud].ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                        chartQuestionDMG[stud].ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                                        chartQuestionDMG[stud].Series[0].IsValueShownAsLabel = true;
                                        chartQuestionDMG[stud].Series[0].IsXValueIndexed = true;

                                        chartQuestionDMG[stud].Series[1].IsValueShownAsLabel = true;
                                        chartQuestionDMG[stud].Series[1].IsXValueIndexed = true;

                                        chartQuestionDMG[stud].ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                        chartQuestionDMG[stud].ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                        chartQuestionDMG[stud].ChartAreas[0].AxisY.Maximum = 100;
                                        chartQuestionDMG[stud].ChartAreas[0].AxisY.Minimum = 0;
                                    }
                                    plhQuestionWise.Controls.Add(chartQuestionDMG[stud]);
                                }
                            }

                            if (dtNew.Rows.Count > 0)
                            {
                                gvNew.Visible = true;
                                gvNew.DataSource = dtNew;
                                gvNew.DataBind();
                                pnlQuestions_DMG.Controls.Add(gvNew);
                                if (gvNew.HeaderRow.Cells.Count > 0)
                                {
                                    for (int headerRows = 0; headerRows < gvNew.HeaderRow.Cells.Count; headerRows++)
                                    {
                                        string headerValues = gvNew.HeaderRow.Cells[headerRows].Text;
                                        var output = Regex.Replace(headerValues, @"[\d-]", string.Empty);
                                        gvNew.HeaderRow.Cells[headerRows].BackColor = ColorTranslator.FromHtml("#00aff0");
                                        gvNew.HeaderRow.Cells[headerRows].ForeColor = System.Drawing.Color.White;
                                        gvNew.HeaderRow.Cells[headerRows].BorderColor = System.Drawing.Color.Black;
                                        gvNew.HeaderRow.Cells[headerRows].Text = output;
                                        gvNew.HeaderRow.Cells[headerRows].Wrap = true;
                                        gvNew.HeaderRow.Cells[headerRows].Width = output.Length * 10 + 20;
                                    }
                                }
                                for (int gv = 0; gv < gvNew.Rows.Count; gv++)
                                {
                                    gvNew.Rows[gv].HorizontalAlign = HorizontalAlign.Center;
                                    int necol = 1;
                                    int iteration = 1;
                                    for (int gvcol = 0; gvcol < gvNew.Rows[gv].Cells.Count; gvcol++)
                                    {
                                        gvNew.Rows[gv].Cells[gvcol].HorizontalAlign = HorizontalAlign.Center;
                                        if (gvcol == necol)
                                        {
                                            gvNew.Rows[gv].Cells[gvcol].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                        if (gvcol != 0)
                                            iteration++;
                                        if (iteration == ((necol == 1) ? 4 : 3))
                                        {
                                            iteration = 0;
                                            necol = gvcol + 1;
                                        }
                                    }
                                }
                            }
                        }
                        FpSpreadChapterWiseDMG.Sheets[0].PageSize = FpSpreadChapterWiseDMG.Sheets[0].RowCount;
                        FpSpreadChapterWiseDMG.Width = 900;
                        FpSpreadChapterWiseDMG.Height = (FpSpreadChapterWiseDMG.Sheets[0].RowCount * 27) + 45;
                        FpSpreadChapterWiseDMG.SaveChanges();
                        FpSpreadChapterWiseDMG.Visible = true;
                        divChapterWiseDMG.Visible = true;
                        divMainChart.Visible = true;
                        divMainContent.Visible = true;
                        rptprint1.Visible = true;
                        divShowQuestions.Visible = false;
                        popupdiv.Visible = false;
                        lblpopuperr.Text = string.Empty;
                    }
                    if (isQuesWiseSucc)
                    {
                        if (!isQuesWiseSucc)
                        {
                            Init_Spread();
                        }
                    }
                }
                else
                {
                    lblpopuperr.Text = "No Subject's Questions or Syllabus Were Found";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            else
            {
                lblpopuperr.Text = "No Record(s) Were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion GO BUTTON

    #region Print Excel

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname1.Text.Trim();
            if (Convert.ToString(reportname).Trim() != "")
            {
                lbl_norec1.Visible = false;
                string degree_code = Convert.ToString(ddlBranch.SelectedValue);
                string batch_year = Convert.ToString(ddlBatch.SelectedValue);
                string current_sem = Convert.ToString(ddlSem.SelectedValue);
                string branch = Convert.ToString(ddlBranch.SelectedItem);
                if (ddlSec.Items.Count > 0)
                {
                    if (ddlSec.SelectedItem.Text != "ALL")
                    {
                        section = "&nbsp;-&nbsp;" + Convert.ToString(ddlSec.SelectedValue).ToUpper();
                    }
                    else
                    {
                        section = "";
                    }
                }
                string degreedetails = "";
                reportname = reportname.Trim() + "_Individual_Student's_Chapter_And_Question_Wise_DMG_Analysis_Report";
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
                lb4.Text = "Individual Student's Chapter And Question Wise DMG Analysis_Report<br><br>";
                lb4.Style.Add("height", "200px");
                lb4.Style.Add("font-weight", "bold");
                lb4.Style.Add("text-decoration", "none");
                lb4.Style.Add("font-family", "Book Antiqua;");
                lb4.Style.Add("font-size", "10px");
                lb4.Style.Add("text-align", "center");
                lb4.RenderControl(htm);

                htm.InnerWriter.WriteLine("</center>");

                lb4.Text = "Subject Name : " + Convert.ToString(ddlSubject.SelectedItem.Text) + " <br><br/>";
                lb4.Style.Add("height", "200px");
                lb4.Style.Add("text-decoration", "none");
                lb4.Style.Add("font-family", "Book Antiqua;");
                lb4.Style.Add("font-size", "10px");
                lb4.Style.Add("font-weight", "bold");
                lb4.Style.Add("text-align", "left");
                lb4.RenderControl(htm);

                if (dtNew.Rows.Count > 0)
                {
                    gvNew.Visible = true;
                    gvNew.DataSource = dtNew;
                    gvNew.DataBind();
                    if (gvNew.HeaderRow.Cells.Count > 0)
                    {
                        for (int headerRows = 0; headerRows < gvNew.HeaderRow.Cells.Count; headerRows++)
                        {
                            string headerValues = gvNew.HeaderRow.Cells[headerRows].Text;
                            var output = Regex.Replace(headerValues, @"[\d-]", string.Empty);
                            gvNew.HeaderRow.Cells[headerRows].BackColor = ColorTranslator.FromHtml("#00aff0");
                            gvNew.HeaderRow.Cells[headerRows].ForeColor = System.Drawing.Color.White;
                            gvNew.HeaderRow.Cells[headerRows].BorderColor = System.Drawing.Color.Black;
                            gvNew.HeaderRow.Cells[headerRows].Text = output;
                            gvNew.HeaderRow.Cells[headerRows].Wrap = true;
                            gvNew.HeaderRow.Cells[headerRows].Width = output.Length * 10 + 20;
                        }
                    }
                    for (int gv = 0; gv < gvNew.Rows.Count; gv++)
                    {
                        gvNew.Rows[gv].HorizontalAlign = HorizontalAlign.Center;
                        int necol = 1;
                        int iteration = 1;
                        for (int gvcol = 0; gvcol < gvNew.Rows[gv].Cells.Count; gvcol++)
                        {
                            gvNew.Rows[gv].Cells[gvcol].HorizontalAlign = HorizontalAlign.Center;
                            if (gvcol == necol)
                            {
                                gvNew.Rows[gv].Cells[gvcol].HorizontalAlign = HorizontalAlign.Left;
                            }
                            if (gvcol != 0)
                                iteration++;
                            if (iteration == ((necol == 1) ? 4 : 3))
                            {
                                iteration = 0;
                                necol = gvcol + 1;
                            }
                        }
                    }
                    gvNew.RenderControl(htm);
                }

                //if (FpSpreadChapterWiseDMG.Sheets[0].RowCount > 0)
                //{
                //    FpSpreadChapterWiseDMG.Visible = true;

                //    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                //    darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
                //    darkstyle.Font.Name = "Book Antiqua";
                //    darkstyle.Font.Size = FontUnit.Medium;
                //    darkstyle.Font.Bold = true;
                //    darkstyle.HorizontalAlign = HorizontalAlign.Center;
                //    darkstyle.VerticalAlign = VerticalAlign.Middle;
                //    darkstyle.ForeColor = System.Drawing.Color.White;
                //    darkstyle.Border.BorderSize = 1;
                //    darkstyle.Border.BorderColor = System.Drawing.Color.Black;

                //    FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
                //    sheetstyle.Font.Name = "Book Antiqua";
                //    sheetstyle.Font.Size = FontUnit.Medium;
                //    sheetstyle.Font.Bold = true;
                //    sheetstyle.HorizontalAlign = HorizontalAlign.Center;
                //    sheetstyle.VerticalAlign = VerticalAlign.Middle;
                //    sheetstyle.ForeColor = System.Drawing.Color.Black;
                //    sheetstyle.Border.BorderSize = 1;
                //    sheetstyle.Border.BorderColor = System.Drawing.Color.Black;
                //    FpSpreadChapterWiseDMG.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                //    FpSpreadChapterWiseDMG.Sheets[0].DefaultStyle = sheetstyle;
                //    FpSpreadChapterWiseDMG.RenderControl(htm);
                //}


                //lb2 = new Label();
                //lb2.Text = "<br/><br/><br/>Students Question Wise Damage Analysis <br/>";
                //lb2.Style.Add("height", "100px");
                //lb2.Style.Add("text-decoration", "none");
                //lb2.Style.Add("font-family", "Book Antiqua;");
                //lb2.Style.Add("font-size", "10px");
                //lb2.Style.Add("font-weight", "bold");
                //lb2.Style.Add("text-align", "center");
                //lb2.RenderControl(htm);

                btnGo_Click(sender, e);
                //divMainContent.RenderControl(htm);
                //htm.InnerWriter.WriteLine("<br/><center>");
                //gvQuesWiseDmg.RenderControl(htm);
                //htm.InnerWriter.WriteLine("</center><br/>");

                //string imgPath2 = Request.Url.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~/" + "QueswiseDmg.png");
                //if (ChartQuesWiseDmg.Visible == true)
                //{
                //    htm.InnerWriter.WriteLine("<br/><br/><center><Table><tr><td><img src='" + imgPath2 + @"' \></td></tr></Table><br/><br/><br/></center><br/><br/>");
                //}

                lb2 = new Label();
                lb2.Text = "<br/><br/><br/><br/><br/><br/>";
                lb2.Style.Add("height", "100px");
                lb2.Style.Add("text-decoration", "none");
                lb2.Style.Add("font-family", "Book Antiqua;");
                lb2.Style.Add("font-size", "10px");
                lb2.Style.Add("font-weight", "bold");
                lb2.Style.Add("text-align", "center");
                lb2.RenderControl(htm);

                //lb2 = new Label();
                //lb2.Text = "<br/><br/><br/><br/><br/><br/>";
                //lb2.Style.Add("height", "100px");
                //lb2.Style.Add("text-decoration", "none");
                //lb2.Style.Add("font-family", "Book Antiqua;");
                //lb2.Style.Add("font-size", "10px");
                //lb2.Style.Add("font-weight", "bold");
                //lb2.Style.Add("text-align", "center");
                //lb2.RenderControl(htm);


                //htm.InnerWriter.WriteLine("<br/><br/><b><span style='font-family:Book Antiqua; font-size:10px;font-weight:bold; text-align:center; '>Student Chapter Wise Damage Analysis</span><br/><br/></b><center>");
                //gvChapWiseDmg.RenderControl(htm);
                //htm.InnerWriter.WriteLine("</center><br/><br/>");

                //lb2 = new Label();
                //lb2.Text = "<br/><br/>";
                //lb2.Style.Add("height", "100px");
                //lb2.Style.Add("text-decoration", "none");
                //lb2.Style.Add("font-family", "Book Antiqua;");
                //lb2.Style.Add("font-size", "10px");
                //lb2.Style.Add("text-align", "center");
                //lb2.RenderControl(htm);

                //lb2 = new Label();
                //lb2.Text = "<br/><br/>";
                //lb2.Style.Add("height", "100px");
                //lb2.Style.Add("text-decoration", "none");
                //lb2.Style.Add("font-family", "Book Antiqua;");
                //lb2.Style.Add("font-size", "10px");
                //lb2.Style.Add("text-align", "center");
                //lb2.RenderControl(htm);
                //htm.InnerWriter.WriteLine("</center>");
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

    #endregion Print Excel

    #region Print PDF

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string degree_code = Convert.ToString(ddlBranch.SelectedValue);
            string batch_year = Convert.ToString(ddlBatch.SelectedValue);
            string current_sem = Convert.ToString(ddlSem.SelectedValue);
            string branch = Convert.ToString(ddlBranch.SelectedItem);
            if (ddlSec.Items.Count > 0)
            {
                if (ddlSec.SelectedItem.Text != "ALL")
                {
                    section = "&nbsp;-&nbsp;" + Convert.ToString(ddlSec.SelectedValue).ToUpper();
                }
                else
                {
                    section = "";
                }
            }

            string degreedetails = "";
            degreedetails = branch.ToUpper() + "&nbsp;" + section + "&nbsp;(" + ((isSchool) ? "YEAR" : "BATCH") + "&nbsp;" + Convert.ToString(batch_year) + ")&nbsp;" + ((isSchool) ? "TERM" : "SEM") + "&nbsp;-&nbsp;" + Convert.ToString(current_sem);//Convert.ToString(ddlDegree.SelectedItem).ToUpper() + "&nbsp;-&nbsp;" + 
            btnGo_Click(sender, e);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Individual_Student's_Chapter_And_Question_Wise_DMG_Analysis_Report.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Document pdfDoc = new Document(PageSize.A0, 10f, 10f, 10f, 10f);
            pdfDoc.SetPageSize(iTextSharp.text.PageSize.A0.Rotate());
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();


            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            Label lb = new Label();

            string collegename = "";

            DataSet dscol = d2.select_method_wo_parameter("Select collname,address1,address2,address3,category,university from Collinfo where college_code='" + Convert.ToString(ddlCollege.SelectedValue) + "' ", "Text");
            if (dscol.Tables[0].Rows.Count > 0)
            {
                pdfDoc.AddHeader(Convert.ToString(dscol.Tables[0].Rows[0]["collname"]), Convert.ToString(dscol.Tables[0].Rows[0]["collname"]));
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
            lb4.Text = "Individual Student's Chapter And Question Wise DMG Analysis Report<br><br>";
            lb4.Style.Add("height", "200px");
            lb4.Style.Add("text-decoration", "none");
            lb4.Style.Add("font-family", "Book Antiqua;");
            lb4.Style.Add("font-size", "10px");
            lb4.Style.Add("text-align", "center");
            lb4.RenderControl(hw);




            StringWriter sw00 = new StringWriter();
            HtmlTextWriter hw00 = new HtmlTextWriter(sw00);

            lb4.Text = "Subject Name : " + Convert.ToString(ddlSubject.SelectedItem.Text) + " <br><br/>";
            lb4.Style.Add("height", "200px");
            lb4.Style.Add("text-decoration", "none");
            lb4.Style.Add("font-family", "Book Antiqua;");
            lb4.Style.Add("font-size", "10px");
            lb4.Style.Add("text-align", "left");
            lb4.RenderControl(hw00);

            divChapterWiseDMG.RenderControl(hw00);
            FpSpreadChapterWiseDMG.Visible = true;
            //FpSpreadChapterWiseDMG.RenderControl(hw00);

            //GridView gvS = new GridView();
            //gvS.DataSource = FpSpreadChapterWiseDMG.DataSource;
            //gvS.DataBind();
            //gvS.RenderControl(hw00);

            if (dtNew.Rows.Count > 0)
            {
                gvNew.Visible = true;
                gvNew.DataSource = dtNew;
                gvNew.DataBind();
                pnlQuestions_DMG.Controls.Add(gvNew);
                //pnlQuestions_DMG.Visible = true;
                if (gvNew.HeaderRow.Cells.Count > 0)
                {
                    for (int headerRows = 0; headerRows < gvNew.HeaderRow.Cells.Count; headerRows++)
                    {
                        string headerValues = gvNew.HeaderRow.Cells[headerRows].Text;
                        var output = Regex.Replace(headerValues, @"[\d-]", string.Empty);
                        gvNew.HeaderRow.Cells[headerRows].Text = output;
                        gvNew.HeaderRow.Cells[headerRows].BackColor = ColorTranslator.FromHtml("#00aff0");
                        gvNew.HeaderRow.Cells[headerRows].ForeColor = System.Drawing.Color.Black;
                        gvNew.HeaderRow.Cells[headerRows].BorderColor = System.Drawing.Color.Black;
                        gvNew.HeaderRow.Cells[headerRows].Wrap = true;
                        gvNew.HeaderRow.Cells[headerRows].Width = output.Length * 10 + 20;
                    }
                }
                for (int gv = 0; gv < gvNew.Rows.Count; gv++)
                {
                    gvNew.Rows[gv].HorizontalAlign = HorizontalAlign.Center;
                    int necol = 1;
                    int iteration = 1;
                    for (int gvcol = 0; gvcol < gvNew.Rows[gv].Cells.Count; gvcol++)
                    {
                        gvNew.Rows[gv].Cells[gvcol].HorizontalAlign = HorizontalAlign.Center;
                        if (gvcol == necol)
                        {
                            gvNew.Rows[gv].Cells[gvcol].HorizontalAlign = HorizontalAlign.Left;
                        }
                        if (gvcol != 0)
                            iteration++;
                        if (iteration == ((necol == 1) ? 4 : 3))
                        {
                            iteration = 0;
                            necol = gvcol + 1;
                        }
                    }
                }
                gvNew.RenderControl(hw00);
            }

            StringReader sr = new StringReader(Convert.ToString(sw));
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);

            sr = new StringReader(Convert.ToString(sw00));
            htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);

            StringWriter sw1 = new StringWriter();
            HtmlTextWriter hw1 = new HtmlTextWriter(sw1);

            lb4 = new Label();
            if (divMainChart.Visible == true)
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

            lb3.Text = "<br><b><br><br>";
            lb3.Style.Add("height", "200px");
            lb3.Style.Add("text-decoration", "none");
            lb3.Style.Add("font-family", "Book Antiqua;");
            lb3.Style.Add("font-size", "10px");
            lb3.Style.Add("text-align", "left");
            lb3.RenderControl(hw1);
            sr = new StringReader(Convert.ToString(sw1));
            htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);
            if (divMainChart.Visible == true)
            {
                if (chartChapterDMG.Length > 0)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        for (int chapter = 0; chapter < chartChapterDMG.Length; chapter++)
                        {
                            chartChapterDMG[chapter].SaveImage(stream, ChartImageFormat.Png);
                            iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
                            chartImage.ScalePercent(100f);
                            pdfDoc.Add(chartImage);
                        }
                    }
                }
            }

            StringWriter sw0 = new StringWriter();
            HtmlTextWriter hw0 = new HtmlTextWriter(sw0);
            lb4 = new Label();
            if (divMainChart.Visible == true)
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

            sr = new StringReader(Convert.ToString(sw0));
            htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);

            if (divMainChart.Visible == true)
            {
                if (chartQuestionDMG.Length > 0)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        for (int question = 0; question < chartQuestionDMG.Length; question++)
                        {
                            chartQuestionDMG[question].SaveImage(stream, ChartImageFormat.Png);
                            iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
                            chartImage.ScalePercent(100f);
                            chartImage.GetImageRotation();
                            pdfDoc.Add(chartImage);
                        }
                    }
                }
            }
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Print PDF

    #region Close PopUpDiv

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblpopuperr.Text = "";
            popupdiv.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Close PopUpDiv

    protected void imgbtnClose_OnClick(object sender, EventArgs e)
    {
        try
        {
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

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
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

    #region Reusable Methods

    private void ChapterWiseDMG(DataTable dtChapters, DataTable dtQuestionWiseClassDMG, ref DataTable dtChapterWiseClassDMG)
    {
        try
        {
            dtChapterWiseClassDMG.Columns.Clear();
            dtChapterWiseClassDMG.Rows.Clear();
            dtChapterWiseClassDMG.Columns.Add("Chapter_Name");
            dtChapterWiseClassDMG.Columns.Add("Chapter_No");
            dtChapterWiseClassDMG.Columns.Add("Chapter_DMG");
            DataRow drClassDmg;
            if (dtChapters.Rows.Count > 0)
            {
                if (dtQuestionWiseClassDMG.Rows.Count > 0)
                {
                    for (int chapter = 0; chapter < dtChapters.Rows.Count; chapter++)
                    {
                        drClassDmg = dtChapterWiseClassDMG.NewRow();

                        DataView dvQuesWiseClassDMG = new DataView();
                        drClassDmg["Chapter_Name"] = Convert.ToString(dtChapters.Rows[chapter]["unit_name"]);
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
                                double.TryParse(Convert.ToString(dvQuesWiseClassDMG[ques]["Question_DMG"]), out questionWiseDMG);
                                chapterClassDMG += questionWiseDMG;
                            }
                            chapterClassAvg = Math.Round(chapterClassDMG / dvQuesWiseClassDMG.Count, 0, MidpointRounding.AwayFromZero);
                        }
                        drClassDmg["Chapter_DMG"] = Convert.ToString(chapterClassAvg);
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
            dtChapterWiseClassDMG.Columns.Add("Chapter_Name");
            dtChapterWiseClassDMG.Columns.Add("Chapter_No");
            dtChapterWiseClassDMG.Columns.Add("Chapter_DMG");

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
                        drClassDmg["Chapter_Name"] = Convert.ToString(dtChapters.Rows[chapter]["unit_name"]);
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
                        drClassDmg["Chapter_DMG"] = Convert.ToString(chapterClassAvg);
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
            dtQuestionWiseClassDMG.Columns.Add("Question_Name");
            dtQuestionWiseClassDMG.Columns.Add("Question_No");
            dtQuestionWiseClassDMG.Columns.Add("Chapter_No");
            dtQuestionWiseClassDMG.Columns.Add("Chapter_Name");
            dtQuestionWiseClassDMG.Columns.Add("Question_DMG");

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
                        drClassDmg["Question_Name"] = Convert.ToString(dtQuestions.Rows[question]["question"]);
                        drClassDmg["Question_No"] = Convert.ToString(dtQuestions.Rows[question]["QuestionMasterPK"]);
                        drClassDmg["Chapter_No"] = Convert.ToString(dtQuestions.Rows[question]["topic_no"]);
                        drClassDmg["Chapter_Name"] = Convert.ToString(dtQuestions.Rows[question]["unit_name"]);
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
                        drClassDmg["Question_DMG"] = Convert.ToString(QuestionClassAvg);
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

    private void SetCharts()
    {
        try
        {
            Chart[] chartChapters;
            Chart[] chartQuestions;
            //Session["ChapterCharts"] = chartChapterDMG;
            //Session["QuestionCharts"] = chartQuestionDMG;
            if (Session["ChapterCharts"] != null)
            {
                chartChapters = (Chart[])Session["ChapterCharts"];
                if (chartChapters.Length > 0)
                {
                    for (int chap = 0; chap < chartChapters.Length; chap++)
                    {
                        plhChapterWise.Controls.Add(chartChapters[chap]);
                    }
                }
            }
            else
            {
                plhChapterWise.Controls.Clear();
            }
            if (Session["QuestionCharts"] != null)
            {
                chartQuestions = (Chart[])Session["QuestionCharts"];
                if (chartQuestions.Length > 0)
                {
                    for (int chap = 0; chap < chartQuestions.Length; chap++)
                    {
                        plhQuestionWise.Controls.Add(chartQuestions[chap]);
                    }
                }
            }
            else
            {
                plhQuestionWise.Controls.Clear();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void CreateChapterControls(string id)
    {
        Chart chart = new Chart();
        chart.ID = id;
        plhChapterWise.Controls.Add(chart);
    }

    public void CreateQuestionControls(string id)
    {
        Chart chart = new Chart();
        chart.ID = id;
        plhQuestionWise.Controls.Add(chart);
    }

    public void CreateTxtBox(string id)
    {
        TextBox txt = new TextBox();
        txt.ID = id;
        plhChapterWise.Controls.Add(txt);
    }

    public void ChangeHeaderName(bool isschool)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = "";
            lblCollege.Text = ((!isschool) ? "College" : "School");
            lblBatch.Text = ((!isschool) ? "Batch" : "Year");
            lblDegree.Text = ((!isschool) ? "Degree" : "School Type");
            lblBranch.Text = ((!isschool) ? "Department" : "Standard");
            lblSem.Text = ((!isschool) ? "Semester" : "Term");
            lblSec.Text = ((!isschool) ? "Section" : "Section");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    private void ShowQuestions()
    {
        try
        {
            rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;

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

            if (ddlDegree.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "School Type" : "Degree") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlBatch.Items.Count != 0)
            {
                batch_year = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
            }

            if (ddlBranch.Items.Count != 0)
            {
                degree_code = Convert.ToString(ddlBranch.SelectedValue).Trim();
            }
            else
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Standard" : "Department") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlSem.Items.Count != 0)
            {
                semester = Convert.ToString(ddlSem.SelectedItem.Text).Trim();
            }
            else
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Term" : " Semester") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlSec.Enabled == false || ddlSec.Items.Count == 0)
            {
                section = "";
                qrysec = "";
            }
            else if (ddlSec.Items.Count > 0)
            {
                section = Convert.ToString(ddlSec.SelectedItem.Text).Trim();
                qrySection = " and Sections='" + section + "'";
            }

            if (ddlSubject.Items.Count != 0)
            {
                subject_no = Convert.ToString(ddlSubject.SelectedValue).Trim();

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
                Init_Spread(FpShowQuestions);
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
                if (chkQuestions.Checked)
                    divShowQuestions.Visible = true;
            }
            else
            {
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

    #endregion Reusable Methods

}