using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Consolidate_question_paper_master : System.Web.UI.Page
{
    #region Fields Declaration

    DAccess2 d2 = new DAccess2();

    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();

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

    bool isSchool = false;
    bool cellclick = false;
    bool cellclick1 = false;

    int spreadHeight = 0;
    string qry = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    Hashtable hat = new Hashtable();

    //public SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());

    #endregion Fields Declaration

    #region Page Load

    protected void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session["ParaGraphQuestions"] != null)
                {
                    Session.Remove("ParaGraphQuestions");
                }
            }
            callGridBind();
        }
        catch (Exception ex)
        {
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session["ParaGraphQuestions"] != null)
                {
                    Session.Remove("ParaGraphQuestions");
                }
            }
            callGridBind();
        }
        catch (Exception ex)
        {
        }
    }

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
                Session["ParaGraphQuestions"] = null;
                Session["imgQuestionsToPopUp"] = null;
                bindcollege();
                BindBatch();
                BindDegree();
                bindbranch();
                bindsem();
                BindSectionDetail();
                GetSubject();
                ChangeHeaderName(isSchool);
            }
            else
            {
                if (ScriptManager.GetCurrent(this).IsInAsyncPostBack)
                {
                    // Do something only when the page is partially posted back         
                }
                if (Session["fuQuestionImage"] == null && fuQuestionImage.HasFile)
                {
                    Session["fuQuestionImage"] = fuQuestionImage;
                    Label1.Text = fuQuestionImage.FileName;
                }
                else if (Session["fuQuestionImage"] != null && (!fuQuestionImage.HasFile))
                {
                    fuQuestionImage = (FileUpload)Session["fuQuestionImage"];
                    Label1.Text = fuQuestionImage.FileName;
                }
                else if (fuQuestionImage.HasFile)
                {
                    Session["fuQuestionImage"] = fuQuestionImage;
                    Label1.Text = fuQuestionImage.FileName;
                }
            }
        }
        catch (Exception ex) { }
    }

    #endregion Page Load

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
                    strsec = " and st.Sections='" + Convert.ToString(sections).Trim() + "'";
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
                        //subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and   SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' " + Convert.ToString(sems) + "  and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' order by S.subject_no ";
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' order by S.subject_no ";
                    }
                    else if (Convert.ToString(Session["Staff_Code"]).Trim() != "")
                    {
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and st.subject_no=s.subject_no and s.syll_code=SM.syll_code and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' " + Convert.ToString(sems) + " and  SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "'  and SC.staff_code='" + Convert.ToString(Session["Staff_Code"]).Trim() + "' " + strsec + " order by S.subject_no ";
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

    #region  DropDownList Changed Events

    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        BindDegree();
        bindbranch();
        bindsem();
        BindSectionDetail();
        GetSubject();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        bindsem();
        BindSectionDetail();
        GetSubject();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        bindbranch();
        bindsem();
        BindSectionDetail();
        GetSubject();
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        BindSectionDetail();
        GetSubject();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        GetSubject();
    }

    protected void ddlsubjectc_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
    }

    #endregion DropDownList Changed Events

    protected void FpSpread1_OnCellClick(object sender, EventArgs e)
    {
        try
        {
            string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
            cellclick = true;
            FpSpread1.SaveChanges();
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }

    }

    protected void FpSpread1_Selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            if (cellclick == true)
            {
                int activerow = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
                int activecol = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
                string valu = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text.ToString();
                if (valu.Trim() != "")
                {
                    if (valu.Trim() == "-")
                    {
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "No Question Found";
                        FpSpread1.Visible = true;
                        FpSpread2.Visible = false;
                        divPopQuesAns.Visible = false;
                        return;
                    }
                    ViewState["QuestionMasterPK"] = "";
                    string isdesc = "";
                    if (activecol <= 2)
                    {
                    }
                    else if (activecol > 2 && activecol <= 7)
                    {
                        isdesc = " and is_descriptive='0' ";
                    }
                    else if (activecol > 7)
                    {
                        isdesc = " and is_descriptive='1' ";
                    }
                    //else if (activecol == 2)
                    //{
                    //}

                    string syllabus = "";
                    string sylb = "";
                    string sbno = "";
                    if (!chklst_general.Checked)
                    {
                        if (activerow != Convert.ToInt32(FpSpread1.Sheets[0].RowCount - 1))
                        {
                            syllabus = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString();
                            sylb = " and syllabus='" + syllabus + "'";
                            sbno = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Note.ToString();
                        }
                        else
                        {
                            sbno = Convert.ToString(ddlsubject.SelectedItem.Value);
                        }
                    }
                    string type = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, activecol].Text.Trim().ToLower());
                    string typno = "";
                    if (type == "easy")
                    {
                        typno = " and type='0'";
                    }
                    else if (type == "medium")
                    {
                        typno = " and type='1'";
                    }
                    else if (type == "difficult")
                    {
                        typno = " and type='2'";
                    }
                    else if (type == "hard")
                    {
                        typno = " and type='3'";
                    }
                    else if (type == "total")
                    {
                    }
                    string questqry="";
                    //string questqry = "select QuestionMasterPK, question,mark,options,answer,syllabus,is_descriptive,subject_no,is_matching,qmatching, type, (select unit_name from sub_unit_details where topic_no = syllabus) as [Unit Name],file_name,file_type,quetion_image,QuestionType,QuestionSubType,totalChoice from tbl_question_master where subject_no='" + sbno + "' " + sylb + " " + isdesc + " " + typno + " order by subject_no,syllabus,is_descriptive,type,QuestionType,QuestionSubType,mark,question,QuestionMasterPK";
                    if (!chklst_general.Checked)
                        questqry = "select subject_no,(SELECT unit_name FROM sub_unit_details WHERE topic_no = syllabus) AS [Unit Name],syllabus,is_descriptive,CASE WHEN is_descriptive=0 THEN 'Objective' WHEN is_descriptive=1 THEN 'Descriptive' else Convert(varchar(10),isnull(is_descriptive,'')) END AS Question_Type,TYPE,CASE WHEN TYPE=0 THEN 'Easy' WHEN TYPE=1 THEN 'Medium' WHEN TYPE=2 THEN 'Difficult' WHEN TYPE=3 THEN 'Hard' else Convert(varchar(10),isnull(TYPE,'')) END AS Question_Grade,QuestionMasterPK,question,mark,OPTIONS,answer,is_matching,qmatching,file_name,file_type,quetion_image,QuestionType,QuestionSubType,totalChoice,CASE WHEN QuestionType=1 THEN 'MCQ' WHEN QuestionType=2 THEN 'Fill in the Blanks' WHEN QuestionType=3 THEN 'Match The Following' WHEN QuestionType=4 THEN 'True or False' WHEN QuestionType=5 THEN 'Rearranging' WHEN QuestionType=6 THEN 'Paragraph With Questions' ELSE Convert(varchar(10),isnull(QuestionType,'')) END AS QuestionMainType,CASE WHEN ((QuestionType=1) AND QuestionSubType=1) THEN 'Single answer' WHEN ((QuestionType=1) AND QuestionSubType=2) THEN 'Multiple Answer' WHEN ((QuestionType=3) AND QuestionSubType=3) THEN 'Statement Vs Statement' WHEN ((QuestionType=3) AND QuestionSubType=4) THEN 'Statement Vs Image' WHEN ((QuestionType=3) AND QuestionSubType=5) THEN 'Image Vs Statement' WHEN ((QuestionType=3) AND QuestionSubType=6) THEN 'Image Vs Image' ELSE Convert(varchar(10),isnull(QuestionSubType,'')) END Question_SubType,isnull(needChoice,0) as needChoice from tbl_question_master where subject_no='" + sbno + "' and General='1'  " + sylb + " " + isdesc + " " + typno + " ORDER BY subject_no,syllabus,is_descriptive,TYPE,QuestionType,QuestionSubType,mark,question,QuestionMasterPK";
                    else
                        questqry = "select is_descriptive,CASE WHEN is_descriptive=0 THEN 'Objective' WHEN is_descriptive=1 THEN 'Descriptive' else Convert(varchar(10),isnull(is_descriptive,'')) END AS Question_Type,TYPE,CASE WHEN TYPE=0 THEN 'Easy' WHEN TYPE=1 THEN 'Medium' WHEN TYPE=2 THEN 'Difficult' WHEN TYPE=3 THEN 'Hard' else Convert(varchar(10),isnull(TYPE,'')) END AS Question_Grade,QuestionMasterPK,question,mark,OPTIONS,answer,is_matching,qmatching,file_name,file_type,quetion_image,QuestionType,QuestionSubType,totalChoice,CASE WHEN QuestionType=1 THEN 'MCQ' WHEN QuestionType=2 THEN 'Fill in the Blanks' WHEN QuestionType=3 THEN 'Match The Following' WHEN QuestionType=4 THEN 'True or False' WHEN QuestionType=5 THEN 'Rearranging' WHEN QuestionType=6 THEN 'Paragraph With Questions' ELSE Convert(varchar(10),isnull(QuestionType,'')) END AS QuestionMainType,CASE WHEN ((QuestionType=1) AND QuestionSubType=1) THEN 'Single answer' WHEN ((QuestionType=1) AND QuestionSubType=2) THEN 'Multiple Answer' WHEN ((QuestionType=3) AND QuestionSubType=3) THEN 'Statement Vs Statement' WHEN ((QuestionType=3) AND QuestionSubType=4) THEN 'Statement Vs Image' WHEN ((QuestionType=3) AND QuestionSubType=5) THEN 'Image Vs Statement' WHEN ((QuestionType=3) AND QuestionSubType=6) THEN 'Image Vs Image' ELSE Convert(varchar(10),isnull(QuestionSubType,'')) END Question_SubType,isnull(needChoice,0) as needChoice from tbl_question_master where   General='0' " + isdesc + " " + typno + " ORDER BY is_descriptive,TYPE,QuestionType,QuestionSubType,mark,question,QuestionMasterPK";

                    ds1 = d2.select_method_wo_parameter(questqry, "Text");
                    //format2(ds1);
                    formatNew2(ds1);
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

    #region Go Click

    public void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            format1();
            pre_parten.Visible = true;
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

            string batch = string.Empty;//
            string degreecod = string.Empty;// Convert.ToString(ddlbranch.SelectedItem.Value);
            string sem = string.Empty;//Convert.ToString(ddlsem.SelectedItem.Text);
            //string testcod=Convert.ToString(ddl_testname.SelectedItem.Value);
            string subject_cd = string.Empty;// Convert.ToString(ddlsubject.SelectedItem.Value);

            if (ddl_collegename.Items.Count == 0)
            {
                //imgdiv2.Visible = true;
                //lbl_alert1.Text = "No Records Found";
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

            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 13;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Visible = true;
            FarPoint.Web.Spread.CheckBoxCellType chk1 = new FarPoint.Web.Spread.CheckBoxCellType();
            chk1.AutoPostBack = true;
            FpSpread1.Width = 950;
            FpSpread1.Height = 500;
            FpSpread1.SaveChanges();

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Unit Name";
            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Total Number of Question ";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Objective";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, 5);

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].Text = "Easy";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Medium";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].Text = "Difficult";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Hard";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Total";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Descriptive";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 1, 5);

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Easy";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].Text = "Medium";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].Text = "Difficult";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 11].Text = "Hard";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 11].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 11].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 11].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 11].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 12].Text = "Total";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 12].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 12].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 12].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 12].HorizontalAlign = HorizontalAlign.Center;

            ds.Clear();
            ds.Reset();
            string subno = "";
            if (ddlsubject.Items.Count > 0)
            {
                subno = Convert.ToString(ddlsubject.SelectedItem.Value);
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Subjects Were Found";
                FpSpread1.Visible = false;
                return;
            }

            if (!chklst_general.Checked)
            {
                string sqry = " select unit_name,topic_no ,subject_no from sub_unit_details where subject_no='" + subno + "' order by subject_no,topic_no,parent_code";
                sqry = sqry + " select syllabus,is_descriptive,type,count(question) as total_question,subject_no from tbl_question_master where subject_no='" + subno + "' group by subject_no, syllabus,is_descriptive,type";
                ds = d2.select_method_wo_parameter(sqry, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count >= 2 && ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataView dv = new DataView();
                            string sylabus = Convert.ToString(ds.Tables[0].Rows[i]["topic_no"]);
                            ds.Tables[1].DefaultView.RowFilter = "syllabus='" + Convert.ToString(ds.Tables[0].Rows[i]["topic_no"]) + "' and subject_no='" + Convert.ToString(ds.Tables[0].Rows[i]["subject_no"]) + "'";

                            dv = ds.Tables[1].DefaultView;
                            int total = 0;
                            if (dv.Count > 0)
                            {
                                FpSpread1.Sheets[0].RowCount++;

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["unit_name"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["topic_no"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(ds.Tables[0].Rows[i]["subject_no"]);

                                for (int dvro = 0; dvro < dv.Count; dvro++)
                                {
                                    total = total + Convert.ToInt32(dv[dvro]["total_question"]);
                                    string qustiontype = Convert.ToString(dv[dvro]["is_descriptive"]);
                                    string type = Convert.ToString(dv[dvro]["type"]);
                                    if (qustiontype == "0")
                                    {
                                        if (type == "0")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[dvro]["total_question"]);
                                        }
                                        else if (type == "1")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[dvro]["total_question"]);
                                        }
                                        else if (type == "2")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[dvro]["total_question"]);
                                        }
                                        else if (type == "3")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dv[dvro]["total_question"]);
                                        }
                                    }
                                    else if (qustiontype == "1")
                                    {
                                        if (type == "0")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dv[dvro]["total_question"]);
                                        }
                                        else if (type == "1")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dv[dvro]["total_question"]);
                                        }
                                        else if (type == "2")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(dv[dvro]["total_question"]);
                                        }
                                        else if (type == "3")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(dv[dvro]["total_question"]);
                                        }
                                    }
                                }
                                int objtot = 0, discript = 0;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(total);

                                for (int objtotal = 3; objtotal < 7; objtotal++)
                                {
                                    Int32 count = 0;
                                    Int32.TryParse(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, objtotal].Text, out count);
                                    objtot += count;
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(objtot);
                                for (int dis = 8; dis < 12; dis++)
                                {
                                    Int32 count = 0;
                                    Int32.TryParse(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, dis].Text, out count);
                                    discript += count;
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(discript);
                            }
                        }

                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.Blue;

                        for (int col = 2; col < FpSpread1.Sheets[0].Columns.Count; col++)
                        {
                            int columnwise_total = 0;
                            for (int r = 0; r < FpSpread1.Sheets[0].Rows.Count - 1; r++)
                            {
                                if (Convert.ToString(FpSpread1.Sheets[0].Cells[r, col].Text) != "")
                                {
                                    columnwise_total += Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, col].Text);
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[r, col].Text = "-";
                                }
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(columnwise_total);
                        }
                        for (int cl = 0; cl < FpSpread1.Sheets[0].Columns.Count; cl++)
                        {
                            FpSpread1.Sheets[0].Columns[cl].Locked = true;
                            FpSpread1.Sheets[0].Columns[cl].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[cl].VerticalAlign = VerticalAlign.Middle;
                        }
                        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    }
                    else
                    {
                        //imgdiv2.Visible = true;
                        //lbl_alert1.Text = "No Syllubus or Questions Were Found";
                        //FpSpread1.Visible = false;
                        FpSpread1.Visible = false;
                        if (ds.Tables.Count >= 2 && ds.Tables[1].Rows.Count == 0)
                        {
                            lbl_alert1.Text = "No Questions Were Found";
                        }
                        else if (ds.Tables[0].Rows.Count == 0)
                        {
                            lbl_alert1.Text = "No Syllubus Were Found";
                        }
                        imgdiv2.Visible = true;
                        return;
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Syllubus or Questions Were Found";
                    FpSpread1.Visible = false;
                    //imgdiv2.Visible = true;
                    //lbl_alert1.Text = "No Records Found";
                    //FpSpread1.Visible = false;
                }
            }
            else
            {
                string strsec = "";
                if (ddlsec.Items.Count > 0)
                {

                    string sections = Convert.ToString(ddlsec.SelectedValue).Trim();
                    if (!string.IsNullOrEmpty(sections))
                        strsec = "  and isnull(ltrim(rtrim(tqm.Sections)),'')='" + Convert.ToString(sections).Trim() + "'";
                    else

                        strsec = "";

                }
                string sqry = "select tm.is_descriptive,type,count(question) as total_question from tbl_question_master tm,tbl_question_bank_master tqm where tqm.exam_type=tm.General and  tqm.Batch_year='" + Convert.ToString(ddlbatch.SelectedValue)
+ "' and tqm.Degree_Code='" + Convert.ToString(ddlbranch.SelectedValue) + "' and tqm.Semester='" + Convert.ToString(ddlsem.SelectedValue) + "' " + strsec + " group by is_descriptive,type ";

                ds = d2.select_method_wo_parameter(sqry, "Text");

                if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    int total = 0;
                    FpSpread1.Sheets[0].Columns[1].Visible = false;
                    FpSpread1.Sheets[0].RowCount++;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["unit_name"]);
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["topic_no"]);
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(ds.Tables[0].Rows[i]["subject_no"]);


                        total = total + Convert.ToInt32(ds.Tables[0].Rows[i]["total_question"]);
                        string qustiontype = Convert.ToString(ds.Tables[0].Rows[i]["is_descriptive"]);
                        string type = Convert.ToString(ds.Tables[0].Rows[i]["type"]);
                        if (qustiontype == "0")
                        {
                            if (type == "0")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["total_question"]);
                            }
                            else if (type == "1")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["total_question"]);
                            }
                            else if (type == "2")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["total_question"]);
                            }
                            else if (type == "3")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["total_question"]);
                            }
                        }
                        else if (qustiontype == "1")
                        {
                            if (type == "0")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["total_question"]);
                            }
                            else if (type == "1")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[i]["total_question"]);
                            }
                            else if (type == "2")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[i]["total_question"]);
                            }
                            else if (type == "3")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(ds.Tables[0].Rows[i]["total_question"]);
                            }
                        }



                    }
                    int objtot = 0, discript = 0;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(total);

                    for (int objtotal = 3; objtotal < 7; objtotal++)
                    {
                        Int32 count = 0;
                        Int32.TryParse(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, objtotal].Text, out count);
                        objtot += count;
                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(objtot);
                    for (int dis = 8; dis < 12; dis++)
                    {
                        Int32 count = 0;
                        Int32.TryParse(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, dis].Text, out count);
                        discript += count;
                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(discript);

                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.Blue;

                    for (int col = 2; col < FpSpread1.Sheets[0].Columns.Count; col++)
                    {
                        int columnwise_total = 0;
                        for (int r = 0; r < FpSpread1.Sheets[0].Rows.Count - 1; r++)
                        {
                            if (Convert.ToString(FpSpread1.Sheets[0].Cells[r, col].Text) != "")
                            {
                                columnwise_total += Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, col].Text);
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[r, col].Text = "-";
                            }
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(columnwise_total);
                    }
                    for (int cl = 0; cl < FpSpread1.Sheets[0].Columns.Count; cl++)
                    {
                        FpSpread1.Sheets[0].Columns[cl].Locked = true;
                        FpSpread1.Sheets[0].Columns[cl].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[cl].VerticalAlign = VerticalAlign.Middle;
                    }
                    FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                }
                else
                {

                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Questions Were Found";
                    FpSpread1.Visible = false;
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

    #endregion Go Click

    public void format2(DataSet dsQuesAns)
    {
        try
        {
            divPopQuesAns.Visible = false;
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 0;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].AutoPostBack = true;
            FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].ColumnCount = 7;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread2.Visible = true;
            FpSpread2.Width = 900;
            FpSpread2.Height = 500;
            FpSpread2.SaveChanges();

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[0].Width = 40;
            FpSpread2.Sheets[0].Columns[0].Locked = true;
            FpSpread2.Sheets[0].Columns[0].Resizable = false;
            FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Unit Name";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[1].Width = 300;
            FpSpread2.Sheets[0].Columns[1].Locked = true;
            FpSpread2.Sheets[0].Columns[1].Resizable = false;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
            FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Type";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[2].Width = 100;
            FpSpread2.Sheets[0].Columns[2].Locked = true;
            FpSpread2.Sheets[0].Columns[2].Resizable = false;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Middle;
            FpSpread2.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Question Grade";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[3].Width = 100;
            FpSpread2.Sheets[0].Columns[3].Locked = true;
            FpSpread2.Sheets[0].Columns[3].Resizable = false;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].VerticalAlign = VerticalAlign.Middle;
            FpSpread2.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Question";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[4].Width = 300;
            FpSpread2.Sheets[0].Columns[4].Locked = true;
            FpSpread2.Sheets[0].Columns[4].Resizable = false;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].VerticalAlign = VerticalAlign.Middle;

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Answer";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Columns[5].Width = 300;
            FpSpread2.Sheets[0].Columns[5].Locked = true;
            FpSpread2.Sheets[0].Columns[5].Resizable = false;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].VerticalAlign = VerticalAlign.Middle;

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Mark";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Columns[6].Width = 50;
            FpSpread2.Sheets[0].Columns[6].Locked = true;
            FpSpread2.Sheets[0].Columns[6].Resizable = false;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].VerticalAlign = VerticalAlign.Middle;

            ds.Clear();
            ds = dsQuesAns;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread2.Sheets[0].RowCount++;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Unit Name"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["QuestionMasterPK"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(ds.Tables[0].Rows[i]["subject_no"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                        string descript = Convert.ToString(ds.Tables[0].Rows[i]["is_descriptive"]);
                        string isdiscrpt = "";
                        if (descript == "0")
                        {
                            isdiscrpt = "Objective";
                        }
                        else if (descript == "1")
                        {
                            isdiscrpt = "Descriptive";
                        }
                        else
                        {
                            isdiscrpt = "-";
                        }
                        string strenght = "";
                        string typ = Convert.ToString(ds.Tables[0].Rows[i]["type"]);
                        if (typ == "0")
                        {
                            strenght = "Easy";
                        }
                        else if (typ == "1")
                        {
                            strenght = "Medium";
                        }
                        else if (typ == "2")
                        {
                            strenght = "Difficult";
                        }
                        else if (typ == "3")
                        {
                            strenght = "Hard";
                        }

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = isdiscrpt;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["syllabus"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = strenght;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["type"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["question"]);
                        string option = Convert.ToString(ds.Tables[0].Rows[i]["options"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["options"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["answer"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["is_matching"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Note = Convert.ToString(ds.Tables[0].Rows[i]["qmatching"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["mark"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Tag = (Convert.ToString(ds.Tables[0].Rows[i]["quetion_image"]) != null && Convert.ToString(ds.Tables[0].Rows[i]["quetion_image"]) != "") ? (byte[])(ds.Tables[0].Rows[i]["quetion_image"]) : new byte[1];

                        //file_name,file_type,quetion_image
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Note = (Convert.ToString(ds.Tables[0].Rows[i]["file_name"]).Trim() != null && Convert.ToString(ds.Tables[0].Rows[i]["file_name"]).Trim() != "") ? Convert.ToString(ds.Tables[0].Rows[i]["file_name"]).Trim() + ((Convert.ToString(ds.Tables[0].Rows[i]["file_type"]).Trim() != null && Convert.ToString(ds.Tables[0].Rows[i]["file_type"]).Trim() != "") ? ";" + (Convert.ToString(ds.Tables[0].Rows[i]["file_type"]).Trim()) : "") : "";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;

                        spreadHeight += FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Height;

                    }

                    FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                    FpSpread2.Height = (spreadHeight) + 45; //(FpSpread2.Sheets[0].RowCount * 27) + 45;//(spreadHeight) + 45;
                    FpSpread2.SaveChanges();
                    //FpSpread1.Visible = false;
                    divPopQuesAns.Visible = true;
                }
                else
                {
                    divPopQuesAns.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                }
            }
            else
            {
                divPopQuesAns.Visible = false;
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

    public void formatNew2(DataSet dsQuesAns)
    {
        try
        {
            divPopQuesAns.Visible = false;
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 0;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].AutoPostBack = true;
            FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].ColumnCount = 7;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread2.Visible = true;
            FpSpread2.Width = 900;
            FpSpread2.Height = 500;
            FpSpread2.SaveChanges();

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[0].Width = 40;
            FpSpread2.Sheets[0].Columns[0].Locked = true;
            FpSpread2.Sheets[0].Columns[0].Resizable = false;
            FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Unit Name";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[1].Width = 300;
            FpSpread2.Sheets[0].Columns[1].Locked = true;
            FpSpread2.Sheets[0].Columns[1].Resizable = false;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
            FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Type";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[2].Width = 100;
            FpSpread2.Sheets[0].Columns[2].Locked = true;
            FpSpread2.Sheets[0].Columns[2].Resizable = false;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Middle;
            FpSpread2.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Question Grade";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[3].Width = 100;
            FpSpread2.Sheets[0].Columns[3].Locked = true;
            FpSpread2.Sheets[0].Columns[3].Resizable = false;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].VerticalAlign = VerticalAlign.Middle;
            FpSpread2.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Question";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[4].Width = 300;
            FpSpread2.Sheets[0].Columns[4].Locked = true;
            FpSpread2.Sheets[0].Columns[4].Resizable = false;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].VerticalAlign = VerticalAlign.Middle;

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Answer";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Columns[5].Width = 300;
            FpSpread2.Sheets[0].Columns[5].Locked = true;
            FpSpread2.Sheets[0].Columns[5].Resizable = false;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].VerticalAlign = VerticalAlign.Middle;

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Mark";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Columns[6].Width = 50;
            FpSpread2.Sheets[0].Columns[6].Locked = true;
            FpSpread2.Sheets[0].Columns[6].Resizable = false;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].VerticalAlign = VerticalAlign.Middle;

            ds.Clear();
            ds = dsQuesAns;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!chklst_general.Checked)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string subjectNo = Convert.ToString(ds.Tables[0].Rows[i]["subject_no"]).Trim();
                        string unitName = Convert.ToString(ds.Tables[0].Rows[i]["Unit Name"]).Trim();
                        string unitNo = Convert.ToString(ds.Tables[0].Rows[i]["syllabus"]).Trim();

                        string questionTypeNo = Convert.ToString(ds.Tables[0].Rows[i]["is_descriptive"]).Trim();
                        string questionType = Convert.ToString(ds.Tables[0].Rows[i]["Question_Type"]).Trim();
                        string questionGradeNo = Convert.ToString(ds.Tables[0].Rows[i]["Type"]).Trim();
                        string questionGrade = Convert.ToString(ds.Tables[0].Rows[i]["Question_Grade"]).Trim();//Question_Grade

                        string questionPK = Convert.ToString(ds.Tables[0].Rows[i]["QuestionMasterPK"]).Trim();
                        string questionName = Convert.ToString(ds.Tables[0].Rows[i]["question"]).Trim();
                        string questionMark = Convert.ToString(ds.Tables[0].Rows[i]["mark"]).Trim();
                        string questionOptions = Convert.ToString(ds.Tables[0].Rows[i]["options"]).Trim();
                        string questionAnswer = Convert.ToString(ds.Tables[0].Rows[i]["answer"]).Trim();

                        string questionMatchingorNot = Convert.ToString(ds.Tables[0].Rows[i]["is_matching"]).Trim();
                        string questionMatchingName = Convert.ToString(ds.Tables[0].Rows[i]["qmatching"]).Trim();

                        string qfileName = Convert.ToString(ds.Tables[0].Rows[i]["file_name"]).Trim();
                        string qfileType = Convert.ToString(ds.Tables[0].Rows[i]["file_type"]).Trim();
                        string questionImages = Convert.ToString(ds.Tables[0].Rows[i]["quetion_image"]);

                        string totalChoice = Convert.ToString(ds.Tables[0].Rows[i]["totalChoice"]).Trim();
                        string QuestionObjTypeNo = Convert.ToString(ds.Tables[0].Rows[i]["QuestionType"]).Trim();
                        string QuestionObjType = Convert.ToString(ds.Tables[0].Rows[i]["QuestionMainType"]).Trim();
                        string questionSubTypeNo = Convert.ToString(ds.Tables[0].Rows[i]["QuestionSubType"]).Trim();
                        string questionSubType = Convert.ToString(ds.Tables[0].Rows[i]["Question_SubType"]).Trim();
                        bool needChoice = false;
                        bool.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["needChoice"]).Trim(), out needChoice);
                        FpSpread2.Sheets[0].RowCount++;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Tag = QuestionObjTypeNo.Trim();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Note = QuestionObjType.Trim();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = unitName.Trim();
                        // Convert.ToString(ds.Tables[0].Rows[i]["Unit Name"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag = questionPK.Trim();
                        // Convert.ToString(ds.Tables[0].Rows[i]["QuestionMasterPK"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = subjectNo.Trim();
                        // Convert.ToString(ds.Tables[0].Rows[i]["subject_no"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                        //string descript = Convert.ToString(ds.Tables[0].Rows[i]["is_descriptive"]).Trim();
                        //string isdiscrpt = "";
                        //if (descript == "0")
                        //{
                        //    isdiscrpt = "Objective";
                        //}
                        //else if (descript == "1")
                        //{
                        //    isdiscrpt = "Descriptive";
                        //}
                        //else
                        //{
                        //    isdiscrpt = "-";
                        //}

                        //string strenght = "";
                        //string typ = Convert.ToString(ds.Tables[0].Rows[i]["type"]);
                        //if (typ == "0")
                        //{
                        //    strenght = "Easy";
                        //}
                        //else if (typ == "1")
                        //{
                        //    strenght = "Medium";
                        //}
                        //else if (typ == "2")
                        //{
                        //    strenght = "Difficult";
                        //}
                        //else if (typ == "3")
                        //{
                        //    strenght = "Hard";
                        //}

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = questionType;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Note = questionTypeNo.Trim();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = unitNo.Trim();
                        // Convert.ToString(ds.Tables[0].Rows[i]["syllabus"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = questionGrade.Trim();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Tag = questionGradeNo.Trim();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Note = totalChoice + ";" + questionSubType.Trim() + ";" + needChoice;
                        // Convert.ToString(ds.Tables[0].Rows[i]["type"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = questionName.Trim();
                        // Convert.ToString(ds.Tables[0].Rows[i]["question"]);
                        //string option = questionOptions.Trim();
                        // Convert.ToString(ds.Tables[0].Rows[i]["options"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag = questionOptions.Trim();
                        // Convert.ToString(ds.Tables[0].Rows[i]["options"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Note = questionSubTypeNo.Trim();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                        string newQuestionAnswer = string.Empty;

                        if (QuestionObjTypeNo.Trim() == "1")
                        {
                            if (questionSubTypeNo.Trim() == "2")
                            {
                                string[] newAns = (questionAnswer.Split(new string[] { "#ans#" }, StringSplitOptions.RemoveEmptyEntries));
                                if (newAns.Length > 0)
                                {
                                    newQuestionAnswer = string.Join(",", newAns);
                                }
                                else
                                {
                                    newQuestionAnswer = questionAnswer;
                                }
                                //((questionAnswer.Contains("#ans#")) ? (questionAnswer.Split(new string[] { "#ans#" }, StringSplitOptions.RemoveEmptyEntries)) : questionAnswer.Trim());
                            }
                        }
                        else if (QuestionObjTypeNo.Trim() == "6")
                        {
                            string[] newAns = (questionAnswer.Split(new string[] { "#Qpara#" }, StringSplitOptions.RemoveEmptyEntries));
                            if (newAns.Length > 0)
                            {
                                newQuestionAnswer = string.Join(",", newAns);
                            }
                            else
                            {
                                newQuestionAnswer = questionAnswer;
                            }
                        }
                        else
                        {
                            newQuestionAnswer = questionAnswer;
                        }

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = newQuestionAnswer.Trim();
                        // Convert.ToString(ds.Tables[0].Rows[i]["answer"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Tag = questionMatchingorNot;
                        // Convert.ToString(ds.Tables[0].Rows[i]["is_matching"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Note = questionMatchingName.Trim();
                        // Convert.ToString(ds.Tables[0].Rows[i]["qmatching"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = questionMark.Trim();
                        // Convert.ToString(ds.Tables[0].Rows[i]["mark"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Tag = (!string.IsNullOrEmpty(questionImages)) ? (byte[])(ds.Tables[0].Rows[i]["quetion_image"]) : new byte[1];
                        //file_name,file_type,quetion_image
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Note = (!string.IsNullOrEmpty(qfileName.Trim())) ? qfileName.Trim() + ((!string.IsNullOrEmpty(qfileType.Trim())) ? ";" + qfileType.Trim() : "") : "";
                        //(Convert.ToString(ds.Tables[0].Rows[i]["file_name"]).Trim() != null && Convert.ToString(ds.Tables[0].Rows[i]["file_name"]).Trim() != "") ? Convert.ToString(ds.Tables[0].Rows[i]["file_name"]).Trim() + ((Convert.ToString(ds.Tables[0].Rows[i]["file_type"]).Trim() != null && Convert.ToString(ds.Tables[0].Rows[i]["file_type"]).Trim() != "") ? ";" + (Convert.ToString(ds.Tables[0].Rows[i]["file_type"]).Trim()) : "") : "";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                        spreadHeight += FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Height;

                    }
                }
                else
                {
                    FpSpread2.Sheets[0].Columns[1].Visible = false;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        

                        string questionTypeNo = Convert.ToString(ds.Tables[0].Rows[i]["is_descriptive"]).Trim();
                        string questionType = Convert.ToString(ds.Tables[0].Rows[i]["Question_Type"]).Trim();
                        string questionGradeNo = Convert.ToString(ds.Tables[0].Rows[i]["Type"]).Trim();
                        string questionGrade = Convert.ToString(ds.Tables[0].Rows[i]["Question_Grade"]).Trim();//Question_Grade

                        string questionPK = Convert.ToString(ds.Tables[0].Rows[i]["QuestionMasterPK"]).Trim();
                        string questionName = Convert.ToString(ds.Tables[0].Rows[i]["question"]).Trim();
                        string questionMark = Convert.ToString(ds.Tables[0].Rows[i]["mark"]).Trim();
                        string questionOptions = Convert.ToString(ds.Tables[0].Rows[i]["options"]).Trim();
                        string questionAnswer = Convert.ToString(ds.Tables[0].Rows[i]["answer"]).Trim();

                        string questionMatchingorNot = Convert.ToString(ds.Tables[0].Rows[i]["is_matching"]).Trim();
                        string questionMatchingName = Convert.ToString(ds.Tables[0].Rows[i]["qmatching"]).Trim();

                        string qfileName = Convert.ToString(ds.Tables[0].Rows[i]["file_name"]).Trim();
                        string qfileType = Convert.ToString(ds.Tables[0].Rows[i]["file_type"]).Trim();
                        string questionImages = Convert.ToString(ds.Tables[0].Rows[i]["quetion_image"]);

                        string totalChoice = Convert.ToString(ds.Tables[0].Rows[i]["totalChoice"]).Trim();
                        string QuestionObjTypeNo = Convert.ToString(ds.Tables[0].Rows[i]["QuestionType"]).Trim();
                        string QuestionObjType = Convert.ToString(ds.Tables[0].Rows[i]["QuestionMainType"]).Trim();
                        string questionSubTypeNo = Convert.ToString(ds.Tables[0].Rows[i]["QuestionSubType"]).Trim();
                        string questionSubType = Convert.ToString(ds.Tables[0].Rows[i]["Question_SubType"]).Trim();
                        bool needChoice = false;
                        bool.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["needChoice"]).Trim(), out needChoice);
                        FpSpread2.Sheets[0].RowCount++;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Tag = QuestionObjTypeNo.Trim();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Note = QuestionObjType.Trim();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;

                        
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag = questionPK.Trim();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = questionType;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Note = questionTypeNo.Trim();
                      
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = questionGrade.Trim();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Tag = questionGradeNo.Trim();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Note = totalChoice + ";" + questionSubType.Trim() + ";" + needChoice;
               
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = questionName.Trim();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag = questionOptions.Trim();
                     
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Note = questionSubTypeNo.Trim();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                        string newQuestionAnswer = string.Empty;

                        if (QuestionObjTypeNo.Trim() == "1")
                        {
                            if (questionSubTypeNo.Trim() == "2")
                            {
                                string[] newAns = (questionAnswer.Split(new string[] { "#ans#" }, StringSplitOptions.RemoveEmptyEntries));
                                if (newAns.Length > 0)
                                {
                                    newQuestionAnswer = string.Join(",", newAns);
                                }
                                else
                                {
                                    newQuestionAnswer = questionAnswer;
                                }
                                //((questionAnswer.Contains("#ans#")) ? (questionAnswer.Split(new string[] { "#ans#" }, StringSplitOptions.RemoveEmptyEntries)) : questionAnswer.Trim());
                            }
                            else 
                            {
                                newQuestionAnswer = questionAnswer;
                            }
                        }
                        else if (QuestionObjTypeNo.Trim() == "6")
                        {
                            string[] newAns = (questionAnswer.Split(new string[] { "#Qpara#" }, StringSplitOptions.RemoveEmptyEntries));
                            if (newAns.Length > 0)
                            {
                                newQuestionAnswer = string.Join(",", newAns);
                            }
                            else
                            {
                                newQuestionAnswer = questionAnswer;
                            }
                        }
                        else
                        {
                            newQuestionAnswer = questionAnswer;
                        }

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = newQuestionAnswer.Trim();
                        // Convert.ToString(ds.Tables[0].Rows[i]["answer"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Tag = questionMatchingorNot;
                        // Convert.ToString(ds.Tables[0].Rows[i]["is_matching"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Note = questionMatchingName.Trim();
                        // Convert.ToString(ds.Tables[0].Rows[i]["qmatching"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = questionMark.Trim();
                        // Convert.ToString(ds.Tables[0].Rows[i]["mark"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Tag = (!string.IsNullOrEmpty(questionImages)) ? (byte[])(ds.Tables[0].Rows[i]["quetion_image"]) : new byte[1];
                        //file_name,file_type,quetion_image
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Note = (!string.IsNullOrEmpty(qfileName.Trim())) ? qfileName.Trim() + ((!string.IsNullOrEmpty(qfileType.Trim())) ? ";" + qfileType.Trim() : "") : "";
                        //(Convert.ToString(ds.Tables[0].Rows[i]["file_name"]).Trim() != null && Convert.ToString(ds.Tables[0].Rows[i]["file_name"]).Trim() != "") ? Convert.ToString(ds.Tables[0].Rows[i]["file_name"]).Trim() + ((Convert.ToString(ds.Tables[0].Rows[i]["file_type"]).Trim() != null && Convert.ToString(ds.Tables[0].Rows[i]["file_type"]).Trim() != "") ? ";" + (Convert.ToString(ds.Tables[0].Rows[i]["file_type"]).Trim()) : "") : "";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Locked = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                        spreadHeight += FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Height;

                    }
                
                
                }
                for (int sh = 0; sh < FpSpread2.Sheets[0].ColumnHeader.RowCount; sh++)
                {
                    spreadHeight += FpSpread2.Sheets[0].ColumnHeader.Rows[sh].Height;
                }
                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                FpSpread2.Height = (spreadHeight) + 45; //(FpSpread2.Sheets[0].RowCount * 27) + 45;//(spreadHeight) + 45;
                FpSpread2.SaveChanges();
                //FpSpread1.Visible = false;
                divPopQuesAns.Visible = true;
            }
            else
            {
                divPopQuesAns.Visible = false;
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

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        // Add_questiontype.Visible = false;
        divPopQuesAns.Visible = false;
    }

    #region Print Excel

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        lbl_norec1.Visible = false;
        try
        {
            string reportname = txtexcelname1.Text;
            if (reportname.ToString().Trim() != "")
            {

                if (FpSpread2.Visible == true)
                {
                    d2.printexcelreport(FpSpread2, reportname);

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

    #endregion Print Excel

    #region Print PDF

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string dptname = "Question Preparation";
            string pagename = "Consolidate_question_paper_master.aspx";

            if (FpSpread2.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpSpread2, pagename, dptname);
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

    #endregion Print PDF

    public string Get_file_format(string file_extension)
    {
        try
        {
            string file_type = "";
            switch (file_extension)
            {

                //case ".pdf":
                //    file_type = "application/pdf";
                //    break;

                //case ".txt":
                //    file_type = "application/notepad";
                //    break;

                //case ".xls":
                //    file_type = "application/vnd.ms-excel";
                //    break;

                //case ".xlsx":
                //    file_type = "application/vnd.ms-excel";
                //    break;

                //case ".doc":
                //    file_type = "application/vnd.ms-word";
                //    break;

                //case ".docx":
                //    file_type = "application/vnd.ms-word";
                //    break;

                case ".gif":
                    file_type = "image/gif";
                    break;

                case ".png":
                    file_type = "image/png";
                    break;

                case ".jpg":
                    file_type = "image/jpg";
                    break;

                case ".jpeg":
                    file_type = "image/jpeg";
                    break;

            }
            return file_type;
        }
        catch
        {
            return null;
        }
    }

    protected void FpSpread2_OnCellClick(object sender, EventArgs e)
    {
        try
        {
            int activerow = Convert.ToInt32(FpSpread2.ActiveSheetView.ActiveRow.ToString());
            int activecol = Convert.ToInt32(FpSpread2.ActiveSheetView.ActiveColumn.ToString());
            cellclick1 = true;
            FpSpread2.SaveChanges();
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }

    }

    protected void FpSpread2_Selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            bool isNew = true;
            if (cellclick1 == true)
            {
                if (isNew)
                {
                    ViewState["QuestionMasterPK"] = "";
                    Session["imgQuestionsToPopUp"] = null;
                    Session["imgName"] = null;
                    Session["imgType"] = null;
                    int activeRow = FpSpread2.ActiveSheetView.ActiveRow;
                    int activeCol = FpSpread2.ActiveSheetView.ActiveColumn;
                    string questionPK = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 1].Tag).Trim();
                    Array GradeValue = Enum.GetValues(typeof(QuestionGrade));
                    if (!string.IsNullOrEmpty(questionPK.Trim()))
                    {
                        ViewState["QuestionMasterPK"] = questionPK.Trim();
                        loadQuestionMarks();
                        divMainQuestionMaster.Visible = true;
                        string unitNo = "";
                        if (!chklst_general.Checked)
                        {
                            string unitName = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 1].Text).Trim();
                             unitNo = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 2].Tag).Trim();
                        }
                        string questionType = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 2].Text).Trim();
                        string questionTypeNo = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 2].Note).Trim();

                        string questionGradeNo = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 3].Tag).Trim();
                        string questionGrade = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 3].Text).Trim();

                        string questionName = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 4].Text).Trim();
                        string questionAnswer = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 5].Text).Trim();
                        string questionOptions = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 4].Tag).Trim();

                        string questionMatchorNot = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 5].Tag).Trim();
                        string questionMatchingName = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 5].Note).Trim();

                        string[] totchoiceAndSubtypeNo = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 3].Note).Trim().Split(';');
                        string totalChoice = ((totchoiceAndSubtypeNo.Length > 0) ? totchoiceAndSubtypeNo[0] : "");

                        string questionObjType = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 0].Note).Trim();
                        string questionObjTypeNo = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 0].Tag).Trim();

                        string questionSubType = ((totchoiceAndSubtypeNo.Length > 0) ? ((totchoiceAndSubtypeNo.Length > 1) ? totchoiceAndSubtypeNo[1] : "") : "");

                        string needChoice = ((totchoiceAndSubtypeNo.Length > 0) ? ((totchoiceAndSubtypeNo.Length > 2) ? totchoiceAndSubtypeNo[2] : "False") : "False");
                        bool hasOptions = false;
                        bool.TryParse(needChoice.Trim(), out hasOptions);

                        string questionSubTypeNo = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 4].Note).Trim();

                        string[] questypeFile = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 6].Note).Trim().Split(';');
                        string questionFileName = ((questypeFile.Length > 0) ? questypeFile[0] : "");
                        string questionFileType = ((questypeFile.Length > 0) ? ((questypeFile.Length > 1) ? questypeFile[1] : "") : "");

                        string questionImage = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 6].Tag).Trim();
                        string questionMark = Convert.ToString(FpSpread2.Sheets[0].Cells[activeRow, 6].Text).Trim();

                        ViewState["syllbuscode"] = unitNo.Trim();

                        int quesTypeNo = 0;
                        int.TryParse(questionTypeNo.Trim(), out quesTypeNo);

                        int quesobjType = 0;
                        int.TryParse(questionObjTypeNo.Trim(), out quesobjType);

                        int matchType = 0;
                        int.TryParse(questionSubTypeNo.Trim(), out matchType);
                        chkNeedOptions.Checked = hasOptions;

                        //int d = (int)QuestionType.Descriptive;
                        //int o = (int)QuestionType.Objective;
                        switch (quesTypeNo)
                        {
                            case (int)QuestionType.Objective:
                                SetDefaultValuesOfQuestionMaster();
                                rblObjectiveDescriptive.SelectedValue = questionTypeNo;
                                rblObjectiveDescriptive.Items[1].Enabled = false;
                                rblObjectiveDescriptive.Items[0].Enabled = true;
                                rblMatchSubType.Visible = false;
                                rblSingleorMutiChoice.Visible = false;
                                divMatches.Visible = false;

                                //rblSingleorMutiChoice.Attributes.Add("style", "display:none;");
                                //rblMatchSubType.Attributes.Add("style", "display:none;");
                                txtNoofQuestionCount.Text = string.Empty;
                                txtNoofQuestionCount.Visible = false;
                                lblMQuestionCount.Visible = false;
                                divMatchSubType.Visible = false;
                                txtNoofOptionsCount.Text = string.Empty;
                                lblNoofOptions.Visible = false;
                                txtNoofOptionsCount.Visible = false;
                                txtNoofOptionsCount.Enabled = true;
                                chkAddQuesImage.Checked = false;
                                fuQuestionImage.Visible = false;
                                Session["ParaGraphQuestions"] = null;
                                divParagraph.Visible = false;
                                divSubType.Visible = false;

                                divOptions.Visible = false;
                                if (chkNeedOptions.Checked)
                                {
                                    txtNoofOptionsCount.Enabled = true;
                                }
                                else
                                {
                                    txtNoofOptionsCount.Enabled = false;
                                }
                                //divOptions.Attributes.Add("style", "display:none;");
                                bool issingle = true;
                                if (rblObjectiveDescriptive.Items.Count > 0)
                                {
                                    if (rblObjectiveDescriptive.SelectedIndex == 0)
                                    {
                                        if (rblQuestionType.Items.Count > 0)
                                        {
                                            //rblQuestionType.SelectedIndex = 0;
                                            foreach (ListItem li in rblQuestionType.Items)
                                            {
                                                li.Enabled = false;
                                                if (li.Value.Trim() == questionObjTypeNo.Trim())
                                                {
                                                    li.Selected = true;
                                                    li.Enabled = true;
                                                }
                                                else
                                                {
                                                    li.Selected = false;
                                                }
                                            }
                                            switch (quesobjType)
                                            {
                                                case (int)ObjectiveQuestionType.MCQ:
                                                    rblSingleorMutiChoice.Visible = true;
                                                    divSubType.Visible = true;
                                                    if (rblSingleorMutiChoice.Items.Count > 0)
                                                    {
                                                        foreach (ListItem li in rblSingleorMutiChoice.Items)
                                                        {
                                                            li.Enabled = false;
                                                            if (li.Value.Trim() == questionSubTypeNo.Trim())
                                                            {
                                                                li.Selected = true;
                                                                li.Enabled = true;
                                                            }
                                                            else
                                                            {
                                                                li.Selected = false;
                                                            }
                                                        }
                                                        switch (matchType)
                                                        {
                                                            case (int)ObjectiveQuestionSubType.single:
                                                                issingle = true;
                                                                rblSingleorMutiChoice.Items[0].Selected = true;
                                                                break;
                                                            case (int)ObjectiveQuestionSubType.multiple:
                                                                issingle = false;
                                                                questionAnswer.Replace(",", "#ans#");
                                                                rblSingleorMutiChoice.Items[1].Selected = true;
                                                                break;
                                                            case (int)ObjectiveQuestionSubType.State_Vs_State:
                                                                break;
                                                            case (int)ObjectiveQuestionSubType.State_Vs_Image:
                                                                break;
                                                            case (int)ObjectiveQuestionSubType.Image_Vs_State:
                                                                break;
                                                            case (int)ObjectiveQuestionSubType.Image_Vs_Image:
                                                                break;
                                                            default:
                                                                issingle = true;
                                                                rblSingleorMutiChoice.Items[0].Selected = true;
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case (int)ObjectiveQuestionType.blanks:
                                                    txtNoofOptionsCount.Visible = true;
                                                    lblNoofOptions.Visible = true;
                                                    break;
                                                case (int)ObjectiveQuestionType.Matches:
                                                    divSubType.Visible = true;
                                                    //rblMatchSubType.Attributes.Add("style", "display:table-cell;");
                                                    rblMatchSubType.Visible = true;
                                                    txtNoofQuestionCount.Visible = true;
                                                    lblMQuestionCount.Visible = true;
                                                    txtNoofOptionsCount.Visible = true;
                                                    lblNoofOptions.Visible = true;
                                                    if (rblMatchSubType.Items.Count > 0)
                                                    {
                                                        foreach (ListItem li in rblMatchSubType.Items)
                                                        {
                                                            li.Enabled = false;
                                                            if (li.Value.Trim() == questionSubTypeNo.Trim())
                                                            {
                                                                li.Selected = true;
                                                                li.Enabled = true;
                                                            }
                                                            else
                                                            {
                                                                li.Selected = false;
                                                            }
                                                        }
                                                        switch (matchType)
                                                        {
                                                            case (int)ObjectiveQuestionSubType.single:
                                                                break;
                                                            case (int)ObjectiveQuestionSubType.multiple:
                                                                break;
                                                            case (int)ObjectiveQuestionSubType.State_Vs_State:
                                                            default:
                                                                rblMatchSubType.Items[0].Selected = true;
                                                                //questionMatchingName=
                                                                if (!string.IsNullOrEmpty(questionMatchingName.Trim()))
                                                                {
                                                                    if (questionMatchingName.Contains('^'))
                                                                    {
                                                                        string[] split1 = questionMatchingName.Split(new string[] { "^" }, StringSplitOptions.RemoveEmptyEntries);
                                                                        DataTable dt = new DataTable();
                                                                        dt.Columns.Add("Sno");
                                                                        dt.Columns.Add("Options");
                                                                        dt.Columns.Add("Answer");
                                                                        dt.Columns.Add("AnswerSno");
                                                                        dt.Columns.Add("Left_Image");
                                                                        dt.Columns.Add("Right_Image");
                                                                        DataRow dr;
                                                                        txtNoofQuestionCount.Text = Convert.ToString(split1.Length);
                                                                        int autochar = 65;
                                                                        for (int row = 0; row < split1.Length; row++)
                                                                        {
                                                                            dr = dt.NewRow();
                                                                            dr["Sno"] = Convert.ToString(row + 1);
                                                                            //(split1[row].Contains(';'))?
                                                                            string[] splitAns = split1[row].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                                                            if (splitAns.Length > 0)
                                                                            {
                                                                                if (splitAns.Length >= 2)
                                                                                {
                                                                                    dr["Options"] = splitAns[0];
                                                                                    dr["Answer"] = splitAns[1];
                                                                                }
                                                                                else if (splitAns.Length == 1)
                                                                                {
                                                                                    dr["Options"] = splitAns[0];
                                                                                }
                                                                            }
                                                                            dr["AnswerSno"] = (char)(autochar);
                                                                            autochar++;
                                                                            dt.Rows.Add(dr);
                                                                        }
                                                                        if (dt.Rows.Count > 0)
                                                                        {
                                                                            //if (cb_matchthefollowing.Checked == true)
                                                                            //{
                                                                            gvMatchQuestion.DataSource = dt;
                                                                            gvMatchQuestion.DataBind();
                                                                            gvMatchQuestion.Visible = true;
                                                                            divMatches.Visible = true;
                                                                            //}
                                                                            //else
                                                                            //{
                                                                            //    gvMatchQuestion.Visible = false;
                                                                            //    divMatches.Visible = false;
                                                                            //}
                                                                        }
                                                                        else
                                                                        {
                                                                            gvMatchQuestion.Visible = false;
                                                                            divMatches.Visible = false;
                                                                        }
                                                                    }
                                                                }
                                                                break;
                                                            case (int)ObjectiveQuestionSubType.State_Vs_Image:
                                                                rblMatchSubType.Items[1].Selected = true;
                                                                if (GetAllMatches(questionPK, ref ds))
                                                                {
                                                                    txtNoofQuestionCount.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                                                                    gvMatchQuestion.DataSource = ds;
                                                                    gvMatchQuestion.DataBind();
                                                                    gvMatchQuestion.Visible = true;
                                                                    divMatches.Visible = true;
                                                                }
                                                                else
                                                                {
                                                                    gvMatchQuestion.Visible = false;
                                                                    divMatches.Visible = false;
                                                                }
                                                                break;
                                                            case (int)ObjectiveQuestionSubType.Image_Vs_State:
                                                                rblMatchSubType.Items[2].Selected = true;
                                                                if (GetAllMatches(questionPK, ref ds))
                                                                {
                                                                    txtNoofQuestionCount.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                                                                    gvMatchQuestion.DataSource = ds;
                                                                    gvMatchQuestion.DataBind();
                                                                    gvMatchQuestion.Visible = true;
                                                                    divMatches.Visible = true;
                                                                }
                                                                else
                                                                {
                                                                    gvMatchQuestion.Visible = false;
                                                                    divMatches.Visible = false;
                                                                }
                                                                break;
                                                            case (int)ObjectiveQuestionSubType.Image_Vs_Image:
                                                                rblMatchSubType.Items[3].Selected = true;
                                                                if (GetAllMatches(questionPK, ref ds))
                                                                {
                                                                    txtNoofQuestionCount.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                                                                    gvMatchQuestion.DataSource = ds;
                                                                    gvMatchQuestion.DataBind();
                                                                    gvMatchQuestion.Visible = true;
                                                                    divMatches.Visible = true;
                                                                }
                                                                else
                                                                {
                                                                    gvMatchQuestion.Visible = false;
                                                                    divMatches.Visible = false;
                                                                }
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case (int)ObjectiveQuestionType.TrueFalse:
                                                    txtNoofOptionsCount.Visible = true;
                                                    txtNoofOptionsCount.Enabled = false;
                                                    lblNoofOptions.Visible = true;
                                                    txtNoofOptionsCount.Text = "2";
                                                    if (AddNewRowsToGrid1(gvQOptions, "2"))
                                                    {
                                                        divOptions.Visible = true;
                                                        // divOptions.Attributes.Add("style", "display:table-row;");
                                                    }
                                                    break;
                                                case (int)ObjectiveQuestionType.Rearange:
                                                    txtNoofQuestionCount.Visible = true;
                                                    lblMQuestionCount.Visible = true;

                                                    txtNoofOptionsCount.Visible = true;
                                                    lblNoofOptions.Visible = true;
                                                    string[] qrearrange = questionMatchingName.Split(new string[] { "#Qpara#" }, StringSplitOptions.RemoveEmptyEntries);
                                                    txtNoofQuestionCount.Text = Convert.ToString(qrearrange.Length);
                                                    txtNoofOptionsCount.Text = totalChoice;
                                                    if (qrearrange.Length > 0)
                                                    {
                                                        DataTable dtNew = new DataTable();
                                                        if (loadParagraph(qrearrange.Length, 1, dtNew, 1))
                                                        {
                                                            divParagraph.Visible = true;
                                                        }
                                                        for (int rear = 0; rear < qrearrange.Length; rear++)
                                                        {
                                                            if (gvParagraph.Rows.Count > 0)
                                                            {

                                                            }
                                                        }
                                                        if (divParagraph.Visible == true)
                                                        {
                                                            if (gvParagraph.Rows.Count > 0 && qrearrange.Length == gvParagraph.Rows.Count)
                                                            {
                                                                for (int rear = 0; rear < qrearrange.Length; rear++)
                                                                {
                                                                    TextBox txtQuestionsPara = gvParagraph.Rows[rear].FindControl("txtParaQuestions" + rear) as TextBox;
                                                                    txtQuestionsPara.Text = qrearrange[rear].Trim();
                                                                    //TextBox txtQuestionsParaAns = gvParagraph.Rows[rear].FindControl("txtParaAnswers" + rear) as TextBox;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //questionMatchingName.Split("#QPara#", StringSplitOptions.RemoveEmptyEntries);

                                                    break;
                                                case (int)ObjectiveQuestionType.ParagraphQuestionsWithOption:
                                                    txtNoofQuestionCount.Visible = true;
                                                    lblMQuestionCount.Visible = true;
                                                    txtNoofOptionsCount.Visible = true;
                                                    lblNoofOptions.Visible = true;

                                                    string[] qPara = questionMatchingName.Split(new string[] { "#Qpara#" }, StringSplitOptions.RemoveEmptyEntries);
                                                    string[] qAnswer = questionAnswer.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                                    string[] qOptions = questionOptions.Split(new string[] { "#Qpara#" }, StringSplitOptions.RemoveEmptyEntries);
                                                    txtNoofQuestionCount.Text = Convert.ToString(qPara.Length);
                                                    txtNoofOptionsCount.Text = totalChoice;
                                                    if (qPara.Length > 0 && Convert.ToInt16(totalChoice) > 0)
                                                    {
                                                        DataTable dtNew = new DataTable();
                                                        if (loadParagraph(qPara.Length, Convert.ToInt16(totalChoice), dtNew, 0))
                                                        {
                                                            divParagraph.Visible = true;
                                                        }
                                                        for (int para = 0; para < qPara.Length; para++)
                                                        {
                                                            if (gvParagraph.Rows.Count > 0)
                                                            {

                                                            }
                                                        }
                                                        if (divParagraph.Visible == true)
                                                        {
                                                            if (gvParagraph.Rows.Count > 0 && qPara.Length == gvParagraph.Rows.Count)
                                                            {
                                                                for (int para = 0; para < qPara.Length; para++)
                                                                {
                                                                    TextBox txtQuestionsPara = gvParagraph.Rows[para].FindControl("txtParaQuestions" + para) as TextBox;
                                                                    txtQuestionsPara.Text = qPara[para].Trim();
                                                                    TextBox txtQuestionsParaAns = gvParagraph.Rows[para].FindControl("txtParaAnswers" + para) as TextBox;
                                                                    txtQuestionsParaAns.Text = qAnswer[para].Trim();

                                                                    string[] qparaopt = qOptions[para].Split(new string[] { "#Qparaopt#" }, StringSplitOptions.RemoveEmptyEntries);
                                                                    for (int col = 0; col < qparaopt.Length; col++)
                                                                    {
                                                                        TextBox txtopt = gvParagraph.Rows[para].FindControl("txtParaOptions" + para + (col + 3 + 1)) as TextBox;
                                                                        txtopt.Text = qparaopt[col];
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    break;
                                                default:
                                                    foreach (ListItem li in rblMatchSubType.Items)
                                                    {
                                                        li.Enabled = false;
                                                        if (li.Value.Trim() == questionSubTypeNo.Trim())
                                                        {
                                                            li.Selected = true;
                                                            li.Enabled = true;
                                                        }
                                                        else
                                                        {
                                                            li.Selected = false;
                                                        }
                                                    }
                                                    txtNoofOptionsCount.Visible = true;
                                                    lblNoofOptions.Visible = true;
                                                    if (questionMatchorNot.ToLower().Trim() != "true")
                                                    {
                                                        rblQuestionType.Items[0].Selected = true;
                                                        rblMatchSubType.Visible = false;
                                                        rblSingleorMutiChoice.Items[0].Selected = true;
                                                        rblSingleorMutiChoice.Visible = true;
                                                        divSubType.Visible = true;
                                                        txtNoofOptionsCount.Visible = true;
                                                        lblNoofOptions.Visible = true;
                                                    }
                                                    else
                                                    {
                                                        rblQuestionType.Items[2].Selected = true;
                                                        rblMatchSubType.Visible = true;
                                                        divSubType.Visible = true;
                                                        txtNoofQuestionCount.Visible = true;
                                                        lblMQuestionCount.Visible = true;
                                                        txtNoofOptionsCount.Visible = true;
                                                        lblNoofOptions.Visible = true;
                                                        rblMatchSubType.Items[0].Selected = true;
                                                        rblSingleorMutiChoice.Visible = false;
                                                        rblMatchSubType.Items[0].Selected = true;
                                                        //questionMatchingName=
                                                        if (!string.IsNullOrEmpty(questionMatchingName.Trim()))
                                                        {
                                                            if (questionMatchingName.Contains('^'))
                                                            {
                                                                string[] split1 = questionMatchingName.Split(new string[] { "^" }, StringSplitOptions.RemoveEmptyEntries);
                                                                DataTable dt = new DataTable();
                                                                dt.Columns.Add("Sno");
                                                                dt.Columns.Add("Options");
                                                                dt.Columns.Add("Answer");
                                                                dt.Columns.Add("AnswerSno");
                                                                dt.Columns.Add("Left_Image");
                                                                dt.Columns.Add("Right_Image");
                                                                DataRow dr;
                                                                txtNoofQuestionCount.Text = Convert.ToString(split1.Length);
                                                                int autochar = 65;
                                                                for (int row = 0; row < split1.Length; row++)
                                                                {
                                                                    dr = dt.NewRow();
                                                                    dr["Sno"] = Convert.ToString(row + 1);
                                                                    //(split1[row].Contains(';'))?
                                                                    string[] splitAns = split1[row].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                                                    if (splitAns.Length > 0)
                                                                    {
                                                                        if (splitAns.Length >= 2)
                                                                        {
                                                                            dr["Options"] = splitAns[0];
                                                                            dr["Answer"] = splitAns[1];
                                                                        }
                                                                        else if (splitAns.Length == 1)
                                                                        {
                                                                            dr["Options"] = splitAns[0];
                                                                        }
                                                                    }
                                                                    dr["AnswerSno"] = (char)(autochar);
                                                                    autochar++;
                                                                    dt.Rows.Add(dr);
                                                                }
                                                                if (dt.Rows.Count > 0)
                                                                {
                                                                    //if (cb_matchthefollowing.Checked == true)
                                                                    //{
                                                                    gvMatchQuestion.DataSource = dt;
                                                                    gvMatchQuestion.DataBind();
                                                                    gvMatchQuestion.Visible = true;
                                                                    divMatches.Visible = true;
                                                                }
                                                                else
                                                                {
                                                                    gvMatchQuestion.Visible = false;
                                                                    divMatches.Visible = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    break;
                                            }
                                            switch (quesobjType)
                                            {
                                                case (int)ObjectiveQuestionType.MCQ:
                                                case (int)ObjectiveQuestionType.blanks:
                                                case (int)ObjectiveQuestionType.Matches:
                                                case (int)ObjectiveQuestionType.TrueFalse:
                                                case (int)ObjectiveQuestionType.Rearange:
                                                default:
                                                    txtNoofOptionsCount.Visible = true;
                                                    lblNoofOptions.Visible = true;
                                                    txtNoofOptionsCount.Text = string.Empty;
                                                    if (!string.IsNullOrEmpty(questionOptions.Trim()))
                                                    {
                                                        if (questionOptions.Contains(';') || questionOptions.Contains("#malang#"))
                                                        {
                                                            string[] split1;
                                                            split1 = (!questionOptions.Contains("#malang#")) ? (questionOptions.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)) : (questionOptions.Split(new string[] { "#malang#" }, StringSplitOptions.RemoveEmptyEntries));
                                                            DataTable dt = new DataTable();
                                                            dt.Columns.Add("Sno");
                                                            dt.Columns.Add("Options");
                                                            dt.Columns.Add("Answer");
                                                            dt.Columns.Add("AnswerSno");
                                                            dt.Columns.Add("Left_Image");
                                                            dt.Columns.Add("Right_Image");
                                                            DataRow dr;
                                                            string AnswerQuestion = d2.GetFunction("select answer from tbl_question_master where QuestionMasterPK='" + questionPK + "'");
                                                            txtNoofOptionsCount.Text = Convert.ToString(split1.Length);
                                                            for (int row = 0; row < split1.Length; row++)
                                                            {
                                                                if (!string.IsNullOrEmpty(split1[row].Trim()))
                                                                {
                                                                    dr = dt.NewRow();
                                                                    dr["Sno"] = Convert.ToString(row + 1);
                                                                    dr["Options"] = split1[row];
                                                                    if (issingle && AnswerQuestion == split1[row].Trim())
                                                                    {
                                                                        dr["Answer"] = "1";
                                                                    }
                                                                    else if (!issingle && AnswerQuestion == split1[row].Trim())
                                                                    {
                                                                        dr["Answer"] = "1";
                                                                    }
                                                                    else if (!issingle)
                                                                    {
                                                                        string[] splitMultiAns = AnswerQuestion.Split(new string[] { "#ans#" }, StringSplitOptions.RemoveEmptyEntries);
                                                                        for (int mul = 0; mul < splitMultiAns.Length; mul++)
                                                                        {
                                                                            if (split1[row].Trim() == splitMultiAns[mul].Trim())
                                                                            {
                                                                                dr["Answer"] = "1";
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        dr["Answer"] = "0";
                                                                    }
                                                                    dt.Rows.Add(dr);
                                                                }
                                                            }
                                                            if (dt.Rows.Count > 0)
                                                            {
                                                                gvQOptions.DataSource = dt;
                                                                gvQOptions.DataBind();
                                                                divOptions.Visible = true;
                                                            }
                                                            for (int grd = 0; grd < gvQOptions.Rows.Count; grd++)
                                                            {
                                                                //TextBox ans = (gridView1.Rows[grd].FindControl("txtOption") as TextBox);
                                                                //string answers = ans.Text;
                                                                string getvalu = d2.GetFunction("select answer from tbl_question_master where QuestionMasterPK='" + questionPK + "'");

                                                                (gvQOptions.Rows[grd].FindControl("txtQOption") as TextBox).Text = split1[grd];

                                                                if (getvalu != "0")
                                                                {
                                                                    if (getvalu.ToLower() == split1[grd].ToLower())
                                                                    {
                                                                        (gvQOptions.Rows[grd].FindControl("chkQOptionAnswer") as CheckBox).Checked = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        string[] splitMultiAns = AnswerQuestion.Split(new string[] { "#ans#" }, StringSplitOptions.RemoveEmptyEntries);
                                                                        for (int mul = 0; mul < splitMultiAns.Length; mul++)
                                                                        {
                                                                            if (split1[grd].Trim() == splitMultiAns[mul].Trim())
                                                                            {
                                                                                (gvQOptions.Rows[grd].FindControl("chkQOptionAnswer") as CheckBox).Checked = true;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (!string.IsNullOrEmpty(totalChoice.Trim()))
                                                    {
                                                        txtNoofOptionsCount.Text = totalChoice.Trim();
                                                    }
                                                    break;
                                                case (int)ObjectiveQuestionType.ParagraphQuestionsWithOption:
                                                    break;
                                            }
                                        }
                                    }
                                }
                                break;
                            case (int)QuestionType.Descriptive:
                                SetDefaultValuesOfQuestionMaster(1);
                                rblObjectiveDescriptive.SelectedValue = questionTypeNo;
                                rblObjectiveDescriptive.Items[1].Enabled = true;
                                rblObjectiveDescriptive.Items[0].Enabled = false;
                                txtQuestionAnswer.Text = questionAnswer.Trim();
                                break;
                        }

                        byte[] img = (byte[])FpSpread2.Sheets[0].Cells[activeRow, 6].Tag;
                        Session["imgName"] = Convert.ToString(questionFileName.Trim());
                        Session["imgType"] = Convert.ToString(questionFileType.Trim());

                        if (img.Length > 1)
                        {
                            //img_uplod.FileBytes= img;
                            //FileUpload
                            imgQuestionImage.Attributes.Add("style", "display:table-cell;");
                            string newpath = Server.MapPath("~/image");
                            Dictionary<string, byte[]> Document = new Dictionary<string, byte[]>();
                            Session["imgQuestionsToPopUp"] = img;
                            imgQuestionImage.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String(img);
                            imgQuestionImage.Height = 90;
                            imgQuestionImage.Width = 90;
                            imgQuestionImage.Visible = true;
                            //FileToByteArray(newpath + "new.jpg", out Document);
                            //ShowDocument(newpath + "new.jpg", img);
                        }
                        int grading = 0;
                        int.TryParse(questionGradeNo.Trim(), out grading);
                        foreach (ListItem li in rblQuestionGrading.Items)
                        {
                            li.Enabled = true;
                            if (li.Value == grading.ToString())
                            {
                                switch (grading)
                                {
                                    case (int)QuestionGrade.easy:
                                    case (int)QuestionGrade.medium:
                                    case (int)QuestionGrade.difficult:
                                    case (int)QuestionGrade.hard:
                                        li.Selected = true;
                                        li.Enabled = true;
                                        break;
                                }
                            }
                            else
                            {
                                li.Selected = false;
                            }
                        }

                        txtQMarks.Text = questionMark.Trim();
                        if (ddlQMarks.Items.Count > 0)
                        {
                            bool isSelect = false;
                            foreach (ListItem li in ddlQMarks.Items)
                            {
                                if (li.Text.Trim() == questionMark.Trim())
                                {
                                    li.Selected = true;
                                    isSelect = true;
                                }
                                else
                                {
                                    li.Selected = false;
                                }
                            }
                            if (!isSelect)
                            {
                                ddlQMarks.SelectedIndex = 0;
                                if (questionMark.Trim() != "0" && !string.IsNullOrEmpty(questionMark.Trim()))
                                {
                                    qry = "if exists ( select * from TextValTable where TextVal ='" + questionMark.Trim() + "' and TextCriteria ='QMark' and college_code ='" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "') update TextValTable set TextVal ='" + questionMark.Trim() + "' where TextVal ='" + questionMark.Trim() + "' and TextCriteria ='QMark' and college_code ='" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + questionMark.Trim() + "','QMark','" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "')";
                                    int insert = d2.update_method_wo_parameter(qry, "Text");
                                    loadQuestionMarks();
                                    foreach (ListItem li in ddlQMarks.Items)
                                    {
                                        if (li.Text.Trim() == questionMark.Trim())
                                        {
                                            li.Selected = true;
                                            isSelect = true;
                                        }
                                        else
                                        {
                                            li.Selected = false;
                                        }
                                    }
                                }

                            }
                        }
                        txtQuestionName.Text = questionName.Trim();
                        //if(rbl


                    }
                }
                else
                {
                    #region OLD

                    ViewState["QuestionMasterPK"] = "";
                    int activerow = Convert.ToInt32(FpSpread2.ActiveSheetView.ActiveRow.ToString());
                    int activecol = Convert.ToInt32(FpSpread2.ActiveSheetView.ActiveColumn.ToString());
                    string valu = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text.ToString();
                    Add_questiontype.Visible = true;
                    if (valu.Trim() != "")
                    {
                        if (valu.Trim() == "-")
                        {
                            Add_questiontype.Visible = true;
                            return;
                        }
                        //FpSpread1.Visible = false;
                        //divPopQuesAns.Visible = false;
                        string isdescript = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text.ToString().Trim().ToLower();
                        string answer = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text.Trim().ToString();

                        string qustionanswerpk = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString();

                        cb_matchthefollowing.Checked = false;
                        ViewState["QuestionMasterPK"] = qustionanswerpk;
                        string sbnumber = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Note.ToString();
                        string sylabs = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();
                        ViewState["syllbuscode"] = sylabs;


                        if (isdescript == "objective")
                        {
                            objectiv.Visible = true;
                            rb_object.Checked = true;
                            rb_discript.Checked = false;
                            rb_discript.Enabled = false;
                            descript.Visible = false;
                        }
                        else if (isdescript == "descriptive")
                        {
                            txt_answer.Text = answer;
                            rb_discript.Checked = true;
                            rb_object.Checked = false;
                            objectiv.Visible = false;
                            descript.Visible = true;
                            rb_object.Enabled = false;
                        }
                        string type = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text.ToString().Trim().ToLower();
                        if (type == "easy")
                        {
                            rb_Easy.Checked = true;
                            rb_medium.Checked = false;
                            rb_difficult.Checked = false;
                            rb_hard.Checked = false;
                        }
                        else if (type == "medium")
                        {
                            rb_medium.Checked = true;
                            rb_Easy.Checked = false;
                            rb_difficult.Checked = false;
                            rb_hard.Checked = false;
                        }
                        else if (type == "difficult")
                        {
                            rb_difficult.Checked = true;
                            rb_Easy.Checked = false;
                            rb_medium.Checked = false;
                            rb_hard.Checked = false;
                        }
                        else if (type == "hard")
                        {
                            rb_hard.Checked = true;
                            rb_Easy.Checked = false;
                            rb_medium.Checked = false;
                            rb_difficult.Checked = false;
                        }
                        string question = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text.Trim().ToString();
                        txt_questionname.Text = question;
                        string mark = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text.ToString();
                        txt_marks.Text = mark;
                        string ismatch = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag.ToString();
                        if (ismatch.ToLower() == "true")
                        {
                            cb_matchthefollowing.Checked = true;
                        }
                        else
                        {
                            gridView2.Visible = false;
                        }
                        string qstnmatchs = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Note.ToString();
                        if (qstnmatchs != "")
                        {
                            if (qstnmatchs.Contains('^'))
                            {
                                string[] split1 = qstnmatchs.Split(new string[] { "^" }, StringSplitOptions.RemoveEmptyEntries);// qstnmatchs.Split('\\');
                                DataTable dt = new DataTable();
                                dt.Columns.Add("Sno");
                                dt.Columns.Add("Option");
                                dt.Columns.Add("Answer");
                                dt.Columns.Add("orderkey");

                                DataRow dr;
                                txt_qstcount.Text = Convert.ToString(split1.Length);
                                char alpa = 'A';
                                for (int row = 0; row < split1.Length; row++)
                                {
                                    dr = dt.NewRow();
                                    dr[0] = Convert.ToString(row + 1);
                                    dr[2] = alpa + ". ";
                                    alpa++;
                                    dt.Rows.Add(dr);
                                }
                                if (dt.Rows.Count > 0)
                                {
                                    if (cb_matchthefollowing.Checked == true)
                                    {
                                        gridView2.DataSource = dt;
                                        gridView2.DataBind();
                                        gridView2.Visible = true;
                                    }
                                    else
                                    {
                                        gridView2.Visible = false;
                                    }
                                }
                                for (int grd = 0; grd < gridView2.Rows.Count; grd++)
                                {

                                    if (Convert.ToString(split1[grd]).Contains(';'))
                                    {
                                        string[] split2 = Convert.ToString(split1[grd]).Split(';');

                                        if (Convert.ToString(split2[0]) != "" && Convert.ToString(split2[1]) != "")
                                        {
                                            (gridView2.Rows[grd].FindControl("txtqstn") as TextBox).Text = split2[0];
                                            (gridView2.Rows[grd].FindControl("txt_answer") as TextBox).Text = split2[1];
                                        }
                                    }
                                    else if (Convert.ToString(split1[grd]) != "")
                                    {
                                        (gridView2.Rows[grd].FindControl("txtqstn") as TextBox).Text = split1[grd];
                                    }
                                }
                            }
                        }
                        imgQuestions.Visible = false;
                        Session["imgQuestionsToPopUp"] = null;
                        Session["imgName"] = null;
                        Session["imgType"] = null;
                        string option = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag.ToString();
                        byte[] img = (byte[])FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Tag;
                        string imgName_Type = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Note).Trim();
                        if (!string.IsNullOrEmpty(imgName_Type.Trim()))
                        {
                            string[] split = imgName_Type.Split(';');
                            if (split.Length > 0)
                            {
                                if (split.Length == 2)
                                {
                                    Session["imgName"] = Convert.ToString(split[0].Trim());
                                    Session["imgType"] = Convert.ToString(split[1].Trim());
                                }
                            }

                        }
                        if (img.Length > 1)
                        {
                            //img_uplod.FileBytes= img;
                            //FileUpload
                            imgQuestions.Attributes.Add("style", "display:table-cell;");
                            string newpath = Server.MapPath("~/image");
                            Dictionary<string, byte[]> Document = new Dictionary<string, byte[]>();
                            Session["imgQuestionsToPopUp"] = img;
                            imgQuestions.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String(img);
                            imgQuestions.Height = 70;
                            imgQuestions.Width = 70;
                            imgQuestions.Visible = true;
                            //FileToByteArray(newpath + "new.jpg", out Document);
                            //ShowDocument(newpath + "new.jpg", img);
                        }
                        if (option != "")
                        {
                            if (option.Contains(';') || option.Contains("#malang#"))
                            {
                                //string[] split1 = option.Split(new string[] { "#malang#" }, 1, StringSplitOptions.RemoveEmptyEntries);
                                string[] split1;
                                string[] ddsk = option.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                                split1 = (!option.Contains("#malang#")) ? (option.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)) : (option.Split(new string[] { "#malang#" }, StringSplitOptions.RemoveEmptyEntries));
                                DataTable dt = new DataTable();
                                dt.Columns.Add("Sno");
                                dt.Columns.Add("Option");
                                dt.Columns.Add("Amount");
                                DataRow dr;
                                txt_nooption.Text = Convert.ToString(split1.Length);
                                for (int row = 0; row < split1.Length; row++)
                                {
                                    if (!string.IsNullOrEmpty(split1[row].Trim()))
                                    {
                                        dr = dt.NewRow();
                                        dr[0] = Convert.ToString(row + 1);
                                        dr[1] = split1[row];
                                        dt.Rows.Add(dr);
                                    }
                                }
                                if (dt.Rows.Count > 0)
                                {
                                    gridView1.DataSource = dt;
                                    gridView1.DataBind();
                                    optionqstn.Visible = true;
                                }

                                for (int grd = 0; grd < gridView1.Rows.Count; grd++)
                                {
                                    //TextBox ans = (gridView1.Rows[grd].FindControl("txtOption") as TextBox);
                                    //string answers = ans.Text;
                                    string getvalu = d2.GetFunction("select answer from tbl_question_master where QuestionMasterPK='" + qustionanswerpk + "'");

                                    (gridView1.Rows[grd].FindControl("txtOption") as TextBox).Text = split1[grd];

                                    if (getvalu != "0")
                                    {
                                        if (getvalu.ToLower() == split1[grd].ToLower())
                                        {
                                            (gridView1.Rows[grd].FindControl("cb_answer") as CheckBox).Checked = true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion OLD
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

    public void cb_answer_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int rowindex = rowIndxClicked();
            if (gridView1.Rows.Count > 0)
            {
                for (int grd = 0; grd < gridView1.Rows.Count; grd++)
                {
                    if (rowindex == grd)
                    {
                        (gridView1.Rows[grd].FindControl("cb_answer") as CheckBox).Checked = true;
                    }
                    else
                    {
                        (gridView1.Rows[grd].FindControl("cb_answer") as CheckBox).Checked = false;
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

    public void cb_imagqstn_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_imagqstn.Checked == true)
        {
            img_uplod.Visible = true;
        }
        else
        {
            img_uplod.Visible = false;
        }
    }

    public void cb_matchthefollowing_CheckedChanged(object sender, EventArgs e)
    {
        //addmatchs();
        if (cb_matchthefollowing.Checked == true)
        {
            lbl_noof_question.Visible = true;
            txt_qstcount.Visible = true;
            gridView2.Visible = true;


        }
        else
        {
            lbl_noof_question.Visible = false;
            txt_qstcount.Visible = false;
            gridView2.Visible = false;
        }

    }

    public void addmatchs()
    {
        try
        {
            if (txt_qstcount.Text != "")
            {
                int rowIndex = Convert.ToInt32(txt_qstcount.Text.ToString());
                int.TryParse(txt_qstcount.Text.ToString(), out rowIndex);

                if (rowIndex > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Sno");
                    dt.Columns.Add("Option");
                    dt.Columns.Add("Answer");
                    dt.Columns.Add("orderkey");

                    DataRow dr;
                    char alpa = 'A';
                    for (int row = 0; row < rowIndex; row++)
                    {
                        dr = dt.NewRow();
                        dr[0] = Convert.ToString(row + 1);
                        dr[1] = "Option" + Convert.ToString(row + 1);
                        dr[2] = "Answer" + Convert.ToString(row + 1);
                        dr[3] = alpa + ". ";
                        alpa++;
                        dt.Rows.Add(dr);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (cb_matchthefollowing.Checked == true)
                        {
                            gridView2.DataSource = dt;
                            gridView2.DataBind();
                            gridView2.Visible = true;
                            //  match_option.Visible = true;
                        }
                        else
                        {
                            // match_option.Visible = false;
                            gridView2.Visible = false;

                        }
                    }
                }
                else
                {
                    optionqstn.Visible = false;
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

    public void FileToByteArray(string fileName, out Dictionary<string, byte[]> Document)
    {
        byte[] fileContent = null;
        string DocName = "";
        byte[] DocContent = new byte[1];
        Document = new Dictionary<string, byte[]>();
        Document.Clear();
        System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fs);
        long byteLength = new System.IO.FileInfo(fileName).Length;
        fileContent = binaryReader.ReadBytes((Int32)byteLength);
        fs.Close();
        fs.Dispose();
        binaryReader.Close();
        //Document = new Document();
        DocName = fileName;
        DocContent = fileContent;
        //return Document;
        Document.Add(DocName, DocContent);
    }

    private void ShowDocument(string fileName, byte[] fileContent)
    {
        //Split the string by character . to get file extension type
        string[] stringParts = fileName.Split(new char[] { '.' });
        string strType = stringParts[1];
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
        //Set the content type as file extension type
        Response.ContentType = strType;

        //Write the file content
        this.Response.BinaryWrite(fileContent);
        this.Response.End();

    }

    #region Add Image To Quetions

    public void chkAddQuesImage_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            fuQuestionImage.Visible = false;
            Session["fuQuestionImage"] = null;
            if (chkAddQuesImage.Checked == true)
            {
                fuQuestionImage.Visible = true;
            }
            else
            {
                fuQuestionImage.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Add Image To Quetions

    #region Add Marks

    public void btnAddQMarks_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divAddQmarks.Visible = true;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void btn_add_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divAddQmarks.Visible = true;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Add Marks

    #region Delete Marks

    public void btnDeleteQMarks_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divWarning.Visible = true;
            lblWarningMsgs.Visible = true;
            string QuestionMarks = string.Empty;

            if (ddlQMarks.Items.Count > 0)
            {
                QuestionMarks = Convert.ToString(ddlQMarks.SelectedItem.Text.Trim());
                lblWarningMsgs.Text = "Are You Sure You Want Delete Question Mark " + QuestionMarks + "?";
                if (Convert.ToString(ddlQMarks.SelectedValue).Trim() == "0")
                {
                    divWarning.Visible = false;
                    divPopAlert.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Please Select Any One Marks";
                    return;
                }
            }
            else
            {
                divWarning.Visible = false;
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Mark is Not Found";
                return;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Delete Marks

    #region Save Mark

    public void btnSaveAddQMarks_Click(object sender, EventArgs e)
    {
        try
        {
            //string header = txt_header.Text.ToString();
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            txtAddQmark.Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txtAddQmark.Text).Trim();
            string Qmark = txtAddQmark.Text.Trim();
            double qmarks = 0;
            bool isValidMark = double.TryParse(Qmark, out qmarks);
            if (txtAddQmark.Text.Trim() != "" && isValidMark)
            {
                if (Qmark.Trim() == "0")
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Please Enter Mark Greater Than Zero";
                    return;
                }
                else
                {
                    string sql = "if exists ( select * from TextValTable where TextVal ='" + txtAddQmark.Text.Trim() + "' and TextCriteria ='QMark' and college_code ='" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "') update TextValTable set TextVal ='" + Convert.ToString(txtAddQmark.Text).Trim() + "' where TextVal ='" + txtAddQmark.Text.Trim() + "' and TextCriteria ='QMark' and college_code ='" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + txtAddQmark.Text.Trim() + "','QMark','" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "')";
                    int insert = d2.update_method_wo_parameter(sql, "Text");
                    if (insert != 0)
                    {
                        divPopAlert.Visible = true;
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "Saved Successfully";
                        txtAddQmark.Text = "";
                        divAddQmarks.Visible = false;
                    }
                    else
                    {
                        divPopAlert.Visible = true;
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "Not Saved";
                        txtAddQmark.Text = "";
                        divAddQmarks.Visible = false;
                    }
                }
                loadQuestionMarks();
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Enter Mark";
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Save Mark

    #region  Load Marks

    public void loadQuestionMarks()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddlQMarks.Items.Clear();
            ddlQMarks.Items.Insert(0, new ListItem("Select", "0"));
            ds.Reset();
            ds.Dispose();
            ds.Clear();
            string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='QMark' and college_code ='" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "' order by convert(int ,TextVal) asc";
            ds = d2.select_method_wo_parameter(sql, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlQMarks.DataSource = ds;
                ddlQMarks.DataTextField = "TextVal";
                ddlQMarks.DataValueField = "TextCode";
                ddlQMarks.DataBind();
                ddlQMarks.Items.Insert(0, new ListItem("Select", "0"));
                ddlQMarks.SelectedIndex = 0;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Load Marks

    #region Exit Mark Popup

    public void btnExitAddQMark_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            txtAddQmark.Text = string.Empty;
            divAddQmarks.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }



    #endregion Exit Mark Popup

    #region Delete Marks

    public void btnExitWarningNo_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divWarning.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void btnWarningMsgYes_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            if (ddlQMarks.Items.Count > 0)
            {
                if (ddlQMarks.SelectedValue.Trim() == "0")
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Please Select Any One Question Mark";
                }
                divWarning.Visible = false;
                string college = "";

                qry = "select Count(*) from tbl_question_master where mark='" + Convert.ToString(ddlQMarks.SelectedItem.Text).Trim() + "'";
                string totalquestions = d2.GetFunctionv(qry);
                int total = 0;
                int.TryParse(totalquestions.Trim(), out total);

                if (total == 0)
                {
                    string sql = "delete from textvaltable where TextCode='" + Convert.ToString(ddlQMarks.SelectedItem.Value).Trim() + "' and TextCriteria='QMark' and college_code in ('" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "') ";
                    int delete = d2.update_method_wo_parameter(sql, "Text");
                    if (delete != 0)
                    {
                        divPopAlert.Visible = true;
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = "Deleted Successfully";
                    }
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "You Couldn't Delete The Mark " + Convert.ToString(ddlQMarks.SelectedItem.Text).Trim() + ".Becauze It is in Use.";
                }
                //loadQuestionMarks();
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Marks Were Found";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }



    #endregion

    #region Popup Error Close

    //protected void btnPopErrClose_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblErrSearch.Text = string.Empty;
    //        lblErrSearch.Visible = false;
    //        divPopErr.Visible = false;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //    }
    //}


    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void btn_errorclose1_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lbl_alert.Text = string.Empty;
            imgdiv3.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Popup Error Close

    #region Close Question Popup

    protected void imgbtnQuestionMaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            //Add_questiontype.Visible = false;
            divMainQuestionMaster.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            Add_questiontype.Visible = false;
            //divMainQuestionMaster.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Close Question Popup

    #region No. of Option Changed

    //public void txtNoofOptionsCount_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblErrSearch.Text = string.Empty;
    //        lblErrSearch.Visible = false;
    //        divOptions.Visible = false;
    //        int rows = 0;
    //        int cols = 0;
    //        DataTable dtPara = new DataTable();
    //        string totalOptions = txtNoofOptionsCount.Text.Trim();
    //        string totalQuestions = txtNoofQuestionCount.Text.Trim();
    //        divParagraph.Visible = false;
    //        Session["ParaGraphQuestions"] = null;
    //        if (totalOptions.Trim() != "0" && totalOptions.Trim() != "")
    //        {

    //        }
    //        else
    //        {
    //            divPopAlert.Visible = true;
    //            lblAlertMsg.Text = "Please Enter Total Options Other Than Zero";
    //            return;
    //        }
    //        if (rblObjectiveDescriptive.SelectedValue.Trim() == "0")
    //        {
    //            string QuestionType = rblQuestionType.SelectedValue.Trim();
    //            switch (QuestionType)
    //            {
    //                case "1":
    //                case "2":
    //                case "3":
    //                    if (AddNewRowsToGrid1(gvQOptions, totalOptions.Trim()))
    //                    {
    //                        divOptions.Visible = true;
    //                        divOptions.Attributes.Add("style", "display:table-row");
    //                        //divOptions.Attributes.Add(
    //                    }
    //                    break;
    //                case "5":
    //                    if (AddNewRowsToGrid1(gvQOptions, totalOptions.Trim()))
    //                    {
    //                        divOptions.Visible = true;
    //                        divOptions.Attributes.Add("style", "display:table-row");
    //                    }
    //                    int.TryParse(totalQuestions.Trim(), out rows);
    //                    int.TryParse(totalOptions.Trim(), out cols);
    //                    if (loadParagraph(rows, cols, dtPara, 1))
    //                    {
    //                        divParagraph.Visible = true;
    //                    }
    //                    break;
    //                case "6":
    //                    int.TryParse(totalQuestions.Trim(), out rows);
    //                    int.TryParse(totalOptions.Trim(), out cols);
    //                    //DataTable dtPara = new DataTable();
    //                    if (loadParagraph(rows, cols, dtPara))
    //                    {
    //                        divParagraph.Visible = true;
    //                    }
    //                    break;
    //            }
    //            //if (rblQuestionType.SelectedValue.Trim() == "1")
    //            //{
    //            //    if (AddNewRowsToGrid1(gvQOptions, totalOptions.Trim()))
    //            //    {
    //            //        divOptions.Visible = true;
    //            //        divOptions.Attributes.Add("style", "display:table-row");
    //            //        //divOptions.Attributes.Add(
    //            //    }
    //            //    else
    //            //    {
    //            //    }
    //            //}
    //            //else if (rblQuestionType.SelectedValue.Trim() == "2")
    //            //{
    //            //    if (AddNewRowsToGrid1(gvQOptions, totalOptions.Trim()))
    //            //    {
    //            //        divOptions.Visible = true;
    //            //        divOptions.Attributes.Add("style", "display:table-row");
    //            //    }
    //            //    else
    //            //    {
    //            //    }
    //            //}
    //            //else if (rblQuestionType.SelectedValue.Trim() == "3")
    //            //{
    //            //    if (AddNewRowsToGrid1(gvQOptions, totalOptions.Trim()))
    //            //    {
    //            //        divOptions.Visible = true;
    //            //        divOptions.Attributes.Add("style", "display:table-row");
    //            //    }
    //            //    else
    //            //    {
    //            //    }
    //            //}
    //            //else if (rblQuestionType.SelectedValue.Trim() == "4")
    //            //{

    //            //}
    //            //else if (rblQuestionType.SelectedValue.Trim() == "5")
    //            //{
    //            //    if (AddNewRowsToGrid1(gvQOptions, totalOptions.Trim()))
    //            //    {
    //            //        divOptions.Visible = true;
    //            //        divOptions.Attributes.Add("style", "display:table-row");
    //            //    }
    //            //    else
    //            //    {
    //            //    }
    //            //}
    //            //else if (rblQuestionType.SelectedValue.Trim() == "6")
    //            //{
    //            //    int rows = 0;
    //            //    int cols = 0;
    //            //    int.TryParse(totalQuestions.Trim(), out rows);
    //            //    int.TryParse(totalOptions.Trim(), out cols);
    //            //    DataTable dtPara = new DataTable();
    //            //    if (loadParagraph(rows, cols, dtPara))
    //            //    {
    //            //        divParagraph.Visible = true;
    //            //    }
    //            //    //if (AddNewRowsToGrid1(gvQOptions, totalOptions.Trim()))
    //            //    //{
    //            //    //    divOptions.Visible = true;
    //            //    //    divOptions.Attributes.Add("style", "display:table-row");
    //            //    //}
    //            //    //else
    //            //    //{
    //            //    //}
    //            //}
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //    }
    //}

    //Old
    //public void Txt_nooption_OnTextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblErrSearch.Text = string.Empty;
    //        lblErrSearch.Visible = false;
    //        AddNewRowToGrid1();
    //        //gridView1.Visible = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //    }
    //}

    public void txtNoofOptionsCount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divOptions.Visible = false;
            int rows = 0;
            int cols = 0;
            DataTable dtPara = new DataTable();
            string totalOptions = txtNoofOptionsCount.Text.Trim();
            string totalQuestions = txtNoofQuestionCount.Text.Trim();
            divParagraph.Visible = false;
            Session["ParaGraphQuestions"] = null;
            if (totalOptions.Trim() != "0" && totalOptions.Trim() != "")
            {

            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Enter Total Options Other Than Zero";
                return;
            }
            if (rblObjectiveDescriptive.SelectedValue.Trim() == "0")
            {
                string QuestionType = rblQuestionType.SelectedValue.Trim();
                switch (QuestionType)
                {
                    case "1":
                    case "2":
                    case "3":
                        if (AddNewRowsToGrid1(gvQOptions, totalOptions.Trim()))
                        {
                            divOptions.Visible = true;
                            divOptions.Attributes.Add("style", "display:table-row");
                            //divOptions.Attributes.Add(
                        }
                        break;
                    case "4":
                        txtNoofOptionsCount.Visible = true;
                        txtNoofOptionsCount.Enabled = false;
                        lblNoofOptions.Visible = true;
                        txtNoofOptionsCount.Text = "2";

                        if (AddNewRowsToGrid1(gvQOptions, "2"))
                        {
                            divOptions.Visible = true;
                            divOptions.Attributes.Add("style", "display:table-row;");
                        }
                        break;
                    case "5":
                        if (AddNewRowsToGrid1(gvQOptions, totalOptions.Trim()))
                        {
                            divOptions.Visible = true;
                            divOptions.Attributes.Add("style", "display:table-row");
                        }
                        int.TryParse(totalQuestions.Trim(), out rows);
                        int.TryParse(totalOptions.Trim(), out cols);
                        if (loadParagraph(rows, cols, dtPara, 1))
                        {
                            divParagraph.Visible = true;
                        }
                        break;
                    case "6":
                        int.TryParse(totalQuestions.Trim(), out rows);
                        int.TryParse(totalOptions.Trim(), out cols);
                        //DataTable dtPara = new DataTable();
                        if (loadParagraph(rows, cols, dtPara))
                        {
                            divParagraph.Visible = true;
                        }
                        break;
                }
                //if (rblQuestionType.SelectedValue.Trim() == "1")
                //{
                //    if (AddNewRowsToGrid1(gvQOptions, totalOptions.Trim()))
                //    {
                //        divOptions.Visible = true;
                //        divOptions.Attributes.Add("style", "display:table-row");
                //        //divOptions.Attributes.Add(
                //    }
                //    else
                //    {
                //    }
                //}
                //else if (rblQuestionType.SelectedValue.Trim() == "2")
                //{
                //    if (AddNewRowsToGrid1(gvQOptions, totalOptions.Trim()))
                //    {
                //        divOptions.Visible = true;
                //        divOptions.Attributes.Add("style", "display:table-row");
                //    }
                //    else
                //    {
                //    }
                //}
                //else if (rblQuestionType.SelectedValue.Trim() == "3")
                //{
                //    if (AddNewRowsToGrid1(gvQOptions, totalOptions.Trim()))
                //    {
                //        divOptions.Visible = true;
                //        divOptions.Attributes.Add("style", "display:table-row");
                //    }
                //    else
                //    {
                //    }
                //}
                //else if (rblQuestionType.SelectedValue.Trim() == "4")
                //{

                //}
                //else if (rblQuestionType.SelectedValue.Trim() == "5")
                //{
                //    if (AddNewRowsToGrid1(gvQOptions, totalOptions.Trim()))
                //    {
                //        divOptions.Visible = true;
                //        divOptions.Attributes.Add("style", "display:table-row");
                //    }
                //    else
                //    {
                //    }
                //}
                //else if (rblQuestionType.SelectedValue.Trim() == "6")
                //{
                //    int rows = 0;
                //    int cols = 0;
                //    int.TryParse(totalQuestions.Trim(), out rows);
                //    int.TryParse(totalOptions.Trim(), out cols);
                //    DataTable dtPara = new DataTable();
                //    if (loadParagraph(rows, cols, dtPara))
                //    {
                //        divParagraph.Visible = true;
                //    }
                //    //if (AddNewRowsToGrid1(gvQOptions, totalOptions.Trim()))
                //    //{
                //    //    divOptions.Visible = true;
                //    //    divOptions.Attributes.Add("style", "display:table-row");
                //    //}
                //    //else
                //    //{
                //    //}
                //}
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void chkNeedOptions_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkNeedOptions.Checked)
            {
                txtNoofOptionsCount.Enabled = true;
                txtNoofOptionsCount_TextChanged(sender, e);
            }
            else
            {
                txtNoofOptionsCount.Text = "1";
                txtNoofOptionsCount.Enabled = false;
                txtNoofOptionsCount_TextChanged(sender, e);
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion No. of Option Changed

    #region No of Options Changed(OLD)

    public void Txt_nooption_OnTextChanged(object sender, EventArgs e)
    {
        AddNewRowToGrid1();
    }

    #endregion

    #region No of Questions Changed

    public void txt_qstcount_OnTextChanged(object sender, EventArgs e)
    {
        addmatchs();
    }

    public void txtNoofQuestionCount_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string totalQuestions = txtNoofQuestionCount.Text.Trim();
            string totalOptions = txtNoofOptionsCount.Text.Trim();
            divParagraph.Visible = false;
            string QuestionType = rblQuestionType.SelectedValue.Trim();
            if (rblQuestionType.SelectedValue.Trim() != "6" && rblQuestionType.SelectedValue.Trim() != "5")
            {
                if (AddNewRowsToGrid1(gvMatchQuestion, totalQuestions))
                {
                    divMatches.Visible = true;
                    //divMatches.Attributes.Add("style", "display:table-row;");
                }
            }
            else
            {
                int rows = 0;
                int cols = 0;
                int.TryParse(totalOptions.Trim(), out cols);
                int.TryParse(totalQuestions.Trim(), out rows);
                DataTable dtPara = new DataTable();
                if (loadParagraph(rows, cols, dtPara, ((QuestionType == "6") ? 0 : 1)))
                {
                    divParagraph.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion No of Questions Changed

    #region Question Type Changed Event

    public void rblQuestionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            //Session["fuQuestionImage"] = null;
            txtQuestionName.Text = string.Empty;
            txtQuestionAnswer.Text = string.Empty;
            //rblSingleorMutiChoice.Attributes.Add("style", "display:none;");
            //rblMatchSubType.Attributes.Add("style", "display:none;");
            rblSingleorMutiChoice.Visible = false;
            rblMatchSubType.Visible = false;
            txtNoofQuestionCount.Text = string.Empty;
            txtNoofQuestionCount.Visible = false;
            lblMQuestionCount.Visible = false;
            divMatchSubType.Visible = false;
            txtNoofOptionsCount.Text = string.Empty;
            lblNoofOptions.Visible = false;
            txtNoofOptionsCount.Visible = false;
            txtNoofOptionsCount.Enabled = true;
            chkAddQuesImage.Checked = false;
            fuQuestionImage.Visible = false;
            Session["ParaGraphQuestions"] = null;
            divParagraph.Visible = false;
            divSubType.Visible = false;
            if (ddlQMarks.Items.Count > 0)
            {
                ddlQMarks.SelectedIndex = 0;
            }

            divMatches.Visible = false;
            //divOptions.Attributes.Add("style", "display:none;");
            divOptions.Visible = false;
            if (rblQuestionType.Items.Count > 0)
            {
                if (rblQuestionType.SelectedValue.Trim() == "1")
                {
                    divSubType.Visible = true;
                    //rblSingleorMutiChoice.Attributes.Add("style", "display:table-cell;");
                    rblSingleorMutiChoice.Visible = true;
                    if (rblSingleorMutiChoice.Items.Count > 0)
                    {
                        rblSingleorMutiChoice.SelectedIndex = 0;
                    }
                    txtNoofOptionsCount.Visible = true;
                    lblNoofOptions.Visible = true;
                }
                else if (rblQuestionType.SelectedValue.Trim() == "2")
                {
                    txtNoofOptionsCount.Visible = true;
                    lblNoofOptions.Visible = true;

                }
                else if (rblQuestionType.SelectedValue.Trim() == "3")
                {
                    //divMatchSubType.Visible = true;
                    divSubType.Visible = true;
                    //rblMatchSubType.Attributes.Add("style", "display:table-cell;");
                    rblMatchSubType.Visible = true;
                    if (rblMatchSubType.Items.Count > 0)
                    {
                        rblMatchSubType.SelectedIndex = 0;
                    }
                    //divMatches.Visible = true;
                    //if (ddlMatchSubType.Items.Count > 0)
                    //{
                    //    ddlMatchSubType.SelectedIndex = 0;
                    //}
                    //string totalQuestions = txtNoofQuestionCount.Text.Trim();

                    //if (AddNewRowsToGrid1(gvMatchQuestion, totalQuestions))
                    //{
                    //    divMatches.Visible = true;
                    //    //divMatches.Attributes.Add("style", "display:table-row;");
                    //}
                    txtNoofQuestionCount.Visible = true;
                    lblMQuestionCount.Visible = true;

                    txtNoofOptionsCount.Visible = true;
                    lblNoofOptions.Visible = true;
                }
                else if (rblQuestionType.SelectedValue.Trim() == "4")
                {
                    txtNoofOptionsCount.Visible = true;
                    txtNoofOptionsCount.Enabled = false;
                    lblNoofOptions.Visible = true;
                    txtNoofOptionsCount.Text = "2";

                    if (AddNewRowsToGrid1(gvQOptions, "2"))
                    {
                        divOptions.Visible = true;
                        //divOptions.Attributes.Add("style", "display:table-row;");
                    }
                }
                else if (rblQuestionType.SelectedValue.Trim() == "5")
                {
                    txtNoofQuestionCount.Visible = true;
                    lblMQuestionCount.Visible = true;

                    txtNoofOptionsCount.Visible = true;
                    lblNoofOptions.Visible = true;
                }
                else if (rblQuestionType.SelectedValue.Trim() == "6")
                {
                    txtNoofQuestionCount.Visible = true;
                    lblMQuestionCount.Visible = true;
                    txtNoofOptionsCount.Visible = true;
                    lblNoofOptions.Visible = true;
                }

            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void rblMatchSubType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string totalQuestions = txtNoofQuestionCount.Text.Trim();
            divMatches.Visible = false;
            if (AddNewRowsToGrid1(gvMatchQuestion, totalQuestions))
            {
                divMatches.Visible = true;
                //divMatches.Attributes.Add("style", "display:table-row;");
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void ddlMatchSubType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string totalQuestions = txtNoofQuestionCount.Text.Trim();
            divMatches.Visible = false;
            if (AddNewRowsToGrid1(gvMatchQuestion, totalQuestions))
            {
                divMatches.Visible = true;
                //divMatches.Attributes.Add("style", "display:table-row;");
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Question Type Changed Event

    #region  Answer CheckBoxChanged in Option GridView

    //public void chkQOptionAnswer_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblErrSearch.Text = string.Empty;
    //        lblErrSearch.Visible = false;
    //        int rowindex = rowIndxClicked();

    //        bool isSingle = true;
    //        if (rblQuestionType.Items.Count > 0)
    //        {
    //            if (rblQuestionType.SelectedValue.Trim() == "1")
    //            {
    //                if (rblSingleorMutiChoice.Items.Count > 0)
    //                {
    //                    if (rblSingleorMutiChoice.SelectedValue.Trim() == "1")
    //                    {
    //                        isSingle = true;
    //                    }
    //                    else
    //                    {
    //                        isSingle = false;
    //                    }
    //                }
    //            }
    //            else if (rblQuestionType.SelectedValue.Trim() == "2")
    //            {
    //                if (rblSingleorMutiChoice.Items.Count > 0)
    //                {
    //                    if (rblSingleorMutiChoice.SelectedValue.Trim() == "1")
    //                    {
    //                        isSingle = true;
    //                    }
    //                    else
    //                    {
    //                        isSingle = false;
    //                    }
    //                }
    //            }
    //        }
    //        if (gvQOptions.Rows.Count > 0)
    //        {
    //            for (int grd = 0; grd < gvQOptions.Rows.Count; grd++)
    //            {
    //                if (isSingle)
    //                {
    //                    if (rowindex == grd)
    //                    {
    //                        (gvQOptions.Rows[grd].FindControl("chkQOptionAnswer") as CheckBox).Checked = true;
    //                    }
    //                    else
    //                    {
    //                        (gvQOptions.Rows[grd].FindControl("chkQOptionAnswer") as CheckBox).Checked = false;
    //                    }
    //                }
    //                else
    //                {
    //                    if (rowindex == grd)
    //                    {
    //                        (gvQOptions.Rows[grd].FindControl("chkQOptionAnswer") as CheckBox).Checked = (gvQOptions.Rows[grd].FindControl("chkQOptionAnswer") as CheckBox).Checked;
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrSearch.Text = Convert.ToString(ex);
    //        lblErrSearch.Visible = true;
    //    }
    //}

    public void chkQOptionAnswer_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            int rowindex = rowIndxClicked();

            bool isSingle = true;
            if (rblQuestionType.Items.Count > 0)
            {
                if (rblQuestionType.SelectedValue.Trim() == "1")
                {
                    if (rblSingleorMutiChoice.Items.Count > 0)
                    {
                        if (rblSingleorMutiChoice.SelectedValue.Trim() == "1")
                        {
                            isSingle = true;
                        }
                        else
                        {
                            isSingle = false;
                        }
                    }
                }
                else if (rblQuestionType.SelectedValue.Trim() == "2")
                {
                    if (rblSingleorMutiChoice.Items.Count > 0)
                    {
                        if (rblSingleorMutiChoice.SelectedValue.Trim() == "1")
                        {
                            isSingle = true;
                        }
                        else
                        {
                            isSingle = false;
                        }
                    }
                }
            }
            if (gvQOptions.Rows.Count > 0)
            {
                for (int grd = 0; grd < gvQOptions.Rows.Count; grd++)
                {
                    if (isSingle)
                    {
                        if (rowindex == grd)
                        {
                            (gvQOptions.Rows[grd].FindControl("chkQOptionAnswer") as CheckBox).Checked = true;
                        }
                        else
                        {
                            (gvQOptions.Rows[grd].FindControl("chkQOptionAnswer") as CheckBox).Checked = false;
                        }
                    }
                    else
                    {
                        if (rowindex == grd)
                        {
                            (gvQOptions.Rows[grd].FindControl("chkQOptionAnswer") as CheckBox).Checked = (gvQOptions.Rows[grd].FindControl("chkQOptionAnswer") as CheckBox).Checked;
                            break;
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

    #endregion Answer CheckBoxChanged in Option GridView

    #region Save Questions

    public void btn_Savequestion_Click(object sender, EventArgs e)
    {
        try
        {
            string qustiontype = "";

            string questionname = "";
            questionname = Convert.ToString(txt_questionname.Text);
            questionname = Convert.ToString(questionname.Replace("'", "''"));
            questionname = Convert.ToString(questionname.Replace("‘", "''"));
            questionname = Convert.ToString(questionname.Replace("’", "''"));

            bool isSuccess = false;
            string matchthe_following = "";
            string type = "";
            if (rb_Easy.Checked == true)
            {
                type = "0";
            }
            else if (rb_medium.Checked == true)
            {
                type = "1";
            }
            else if (rb_difficult.Checked == true)
            {
                type = "2";
            }
            else if (rb_hard.Checked == true)
            {
                type = "3";
            }
            string options = "";
            string answer = "";
            bool fromImage = false;
            if (rb_object.Checked == true)
            {
                qustiontype = "0";
                for (int i = 0; i < gridView1.Rows.Count; i++)
                {
                    TextBox strhdname = (TextBox)gridView1.Rows[i].FindControl("txtOption");
                    if (strhdname.Text.Trim() != "")
                    {
                        options = options + Convert.ToString(strhdname.Text) + ";";
                    }
                    CheckBox anser = (CheckBox)gridView1.Rows[i].FindControl("cb_answer");
                    if (anser.Checked == true)
                    {
                        answer = Convert.ToString(strhdname.Text);
                    }
                    if (options == "")
                    {
                        imgdiv3.Visible = true;
                        lbl_alert.Text = "Please Enter Option";
                    }
                }
                if (cb_matchthefollowing.Checked == true)
                {
                    if (txt_qstcount.Text.Trim() == "")
                    {
                        imgdiv3.Visible = true;
                        lbl_alert.Text = "Please Enter Question Count";
                        return;
                    }

                    string m_value = "";
                    for (int i = 0; i < gridView2.Rows.Count; i++)
                    {
                        TextBox question = (TextBox)gridView2.Rows[i].FindControl("txtqstn");
                        TextBox answere = (TextBox)gridView2.Rows[i].FindControl("txt_answer");
                        if (question.Text.Trim() != "")
                        {
                            string m_question = Convert.ToString(question.Text);
                            m_question = Convert.ToString(m_question.Replace("'", "''"));
                            string m_answer = Convert.ToString(answere.Text);
                            m_answer = Convert.ToString(m_answer.Replace("'", "''"));
                            if (answere.Text.Trim() != "")
                            {
                                m_answer = ";" + m_answer;
                            }
                            m_value = m_value + m_question + m_answer + "^";
                        }
                    }

                    matchthe_following = " , is_matching='1' , qmatching='" + m_value + "'";

                }
            }
            else if (rb_discript.Checked == true)
            {
                qustiontype = "1";
                answer = Convert.ToString(txt_answer.Text);
            }
            string sylubuscod = Convert.ToString(ViewState["syllbuscode"]);
            string subj_no = Convert.ToString(ddlsubject.SelectedItem.Value);
            string marks = Convert.ToString(txt_marks.Text);
            byte[] img = new byte[1];
            if (imgQuestions.Visible = true && Session["imgQuestionsToPopUp"] != null)
            {
                fromImage = true;
                img = (byte[])Session["imgQuestionsToPopUp"];
            }
            if (marks.Trim() == "" && marks == "0")
            {
                imgdiv3.Visible = true;
                lbl_alert.Text = "Please Enter Mark";
                return;
            }

            options = Convert.ToString(options.Replace("'", "''"));
            options = Convert.ToString(options.Replace("‘", "''"));
            options = Convert.ToString(options.Replace("’", "''"));

            string questiopk = Convert.ToString(ViewState["QuestionMasterPK"]);
            if (questionname != "")
            {
                string insertqry = "if exists (select*from tbl_question_master where QuestionMasterPK='" + questiopk + "'  ) update tbl_question_master set subject_no='" + subj_no + "' ,  syllabus='" + sylubuscod + "' ,  is_descriptive='" + qustiontype + "', mark='" + marks + "', question =N'" + questionname + "',  type='" + type + "' , options='" + options + "',answer='" + answer + "'" + matchthe_following + " where  QuestionMasterPK='" + questiopk + "'";

                int insert = d2.update_method_wo_parameter(insertqry, "Text");
                if (insert != 0)
                {
                    int llogo = 1;
                    isSuccess = true;
                    if (img_uplod.HasFile == true)
                    {
                        string fileName = Path.GetFileName(img_uplod.PostedFile.FileName);
                        string file_type = "";
                        if (img_uplod.FileName.ToLower().EndsWith(".jpg") || img_uplod.FileName.ToLower().EndsWith(".gif") || img_uplod.FileName.ToLower().EndsWith(".png") || img_uplod.FileName.ToLower().EndsWith(".jpeg"))
                        {
                            int fileSize = img_uplod.PostedFile.ContentLength;
                            llogo = fileSize;
                            byte[] byteimage = new byte[fileSize];
                            img_uplod.PostedFile.InputStream.Read(byteimage, 0, fileSize);
                            file_type = Path.GetExtension(img_uplod.PostedFile.FileName);
                            file_type = file_type.ToLower();
                            file_type = Get_file_format(file_type);
                            if (fileName != "" && fileSize != 0 && file_type != "")
                            {
                                string qry = "  update tbl_question_master set file_name=@file_name,file_type=@file_type,quetion_image=@quetion_image where   QuestionMasterPK='" + questiopk + "'";

                                SqlParameter[] sqlpara = new SqlParameter[3];
                                sqlpara[0] = new SqlParameter("@file_name", SqlDbType.NVarChar, 300);
                                sqlpara[0].Value = fileName;

                                sqlpara[1] = new SqlParameter("@quetion_image", SqlDbType.Image, fileSize);
                                sqlpara[1].Value = byteimage;

                                sqlpara[2] = new SqlParameter("@file_type", SqlDbType.NVarChar, 300);
                                sqlpara[2].Value = file_type.ToString();

                                isSuccess = InsertImageQuery(qry, sqlpara);

                            }
                            else
                            {
                                lbl_alert.Text = "Please Select Image Files Only";
                                imgdiv3.Visible = true;
                                return;
                            }


                        }
                        else
                        {
                            lbl_alert.Text = "Please Select .jpg,.gif,.png and .jpeg Image Files Only !!!";
                            imgdiv3.Visible = true;
                            return;
                        }
                    }
                    else if (img.Length > 1 && imgQuestions.Visible == true && Session["imgQuestionsToPopUp"] != null)
                    {
                        string qry = "  update tbl_question_master set file_name=@file_name,file_type=@file_type,quetion_image=@quetion_image where   QuestionMasterPK='" + questiopk + "'";

                        SqlParameter[] sqlpara = new SqlParameter[3];
                        sqlpara[0] = new SqlParameter("@file_name", SqlDbType.NVarChar, 300);
                        sqlpara[0].Value = ((Session["imgName"] == null) ? "newImage.jpg" : Convert.ToString(Session["imgName"]).Trim());

                        sqlpara[1] = new SqlParameter("@quetion_image", SqlDbType.Image, img.Length);
                        sqlpara[1].Value = img;

                        sqlpara[2] = new SqlParameter("@file_type", SqlDbType.NVarChar, 300);
                        sqlpara[2].Value = ((Session["imgType"] == null) ? "image/jpg" : Convert.ToString(Session["imgType"]).Trim());

                        isSuccess = InsertImageQuery(qry, sqlpara);
                    }
                    imgdiv3.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                    txt_questionname.Text = "";
                    txt_nooption.Text = "";
                    txt_marks.Text = "";
                    rb_Easy.Checked = true;
                    rb_medium.Checked = false;
                    rb_difficult.Checked = false;
                    rb_hard.Checked = false;
                    for (int i = 0; i < gridView1.Rows.Count; i++)
                    {
                        (gridView1.Rows[i].FindControl("txtOption") as TextBox).Text = "";
                        (gridView1.Rows[i].FindControl("cb_answer") as CheckBox).Checked = false;
                    }
                    txt_answer.Text = "";
                    Add_questiontype.Visible = false;
                }
                else
                {
                    imgdiv3.Visible = true;
                    lbl_alert.Text = "Not Saved";
                    txt_questionname.Text = "";
                    txt_nooption.Text = "";
                    txt_marks.Text = "";
                    rb_Easy.Checked = true;
                    rb_medium.Checked = false;
                    rb_difficult.Checked = false;
                    rb_hard.Checked = false;
                    for (int i = 0; i < gridView1.Rows.Count; i++)
                    {
                        (gridView1.Rows[i].FindControl("txtOption") as TextBox).Text = "";
                        (gridView1.Rows[i].FindControl("cb_answer") as CheckBox).Checked = false;
                    }
                    txt_answer.Text = "";
                    Add_questiontype.Visible = false;
                }
            }
            else
            {
                imgdiv3.Visible = true;
                lbl_alert.Text = "Enter Test Name";
            }
        }
        catch (Exception ex)
        {

            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    public void btnSaveQuestions_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            string subjectNo = string.Empty;
            string topicNo = string.Empty;
            string questionName = string.Empty;
            string quesObjectiveDescript = string.Empty;
            string questionType = string.Empty;
            string questionSubType = string.Empty;
            string questionGrade = string.Empty;
            string questionMarks = string.Empty;

            string quesImageName = string.Empty;
            string quesImageType = string.Empty;

            string[] quesAnswers = new string[0];
            string[] quesOptions = new string[0];
            bool[] quesOptionAnwer = new bool[0];

            string qOptions = string.Empty;
            string qAnswer = string.Empty;

            string[] questionPara = new string[0];
            string[] questionParaAns = new string[0];
            string[,] questionParaopt = new string[0, 0];

            bool fromImage = false;

            string totalOptions = txtNoofOptionsCount.Text.Trim();
            string totalQuestions = txtNoofQuestionCount.Text.Trim();

            int totalOptionCount = 0;
            int totalQuestionCount = 0;

            int.TryParse(totalOptions.Trim(), out totalOptionCount);
            int.TryParse(totalQuestions.Trim(), out totalQuestionCount);

            int questionImageLenth = 0;
            string questionImageType = string.Empty;
            string questionImageFileName = string.Empty;
            int row = (totalQuestionCount == 0) ? totalOptionCount : totalQuestionCount;
            int col = (totalQuestionCount == 0) ? 1 : 2;
            string[,] quesMatchLR = new string[row, col];
            byte[,] qmatchImage = new byte[row, col];
            ArrayList[] qmatcLImage = new ArrayList[row];
            ArrayList[] qmatcRImage = new ArrayList[row];
            string[] quesMatchRight = new string[0];
            byte[] quesImage = new byte[0];
            string qmatchingName = string.Empty;

            int imageLength = 0;

            bool isObjectiveorDescriptive = true;
            bool isSuccess = false;
            bool quesHasImage = false;
            bool isMatching = false;
            if (ddlsubject.Items.Count == 0)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Subjects Were Found";
                return;
            }
            else
            {
                subjectNo = Convert.ToString(ddlsubject.SelectedValue).Trim();
                subjectNo = Convert.ToString(ddlsubject.SelectedItem.Value).Trim();
            }

            if (ViewState["syllbuscode"] != null)
            {
                topicNo = Convert.ToString(ViewState["syllbuscode"]).Trim();
            }
            else
            {
                divMainQuestionMaster.Visible = false;
                return;
            }

            //string marks = Convert.ToString(txt_marks.Text);
            byte[] img = new byte[1];
            if (imgQuestions.Visible = true && Session["imgQuestionsToPopUp"] != null)
            {
                fromImage = true;
                img = (byte[])Session["imgQuestionsToPopUp"];
            }
            string questionpk = Convert.ToString(ViewState["QuestionMasterPK"]);
            questionName = Convert.ToString(txtQuestionName.Text).Trim();
            questionName = Convert.ToString(questionName.Replace("'", "''"));
            questionName = Convert.ToString(questionName.Replace("‘", "''"));
            questionName = Convert.ToString(questionName.Replace("’", "''"));

            if (rblObjectiveDescriptive.Items.Count > 0)
            {
                quesObjectiveDescript = rblObjectiveDescriptive.SelectedValue.Trim();
                if (rblObjectiveDescriptive.SelectedValue.Trim() == "0")
                {
                    isObjectiveorDescriptive = true;
                }
                else if (rblObjectiveDescriptive.SelectedValue.Trim() == "1")
                {
                    isObjectiveorDescriptive = false;
                }
            }
            if (string.IsNullOrEmpty(questionName.Trim()))
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Type Question and Then Proceed";
                return;
            }

            if (rblQuestionGrading.Items.Count > 0)
            {
                questionGrade = rblQuestionGrading.SelectedValue.Trim();
            }

            if (ddlQMarks.Items.Count == 0)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Please Add Marks and Then Proceed";
                return;
            }
            else
            {
                questionMarks = Convert.ToString(ddlQMarks.SelectedItem.Text).Trim();
                if (questionMarks == "" || questionMarks == "0" || Convert.ToString(ddlQMarks.SelectedItem.Value).Trim() == "0")
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Please Choose Marks Other Than Zero and Empty";
                    return;
                }
            }
            if (chkAddQuesImage.Checked)
            {
                quesHasImage = false;
                if (fuQuestionImage.HasFile)
                {
                    string errorms = string.Empty;
                    if (!CheckValidFiles(fuQuestionImage, out errorms))
                    {
                        divPopAlert.Visible = true;
                        lblAlertMsg.Visible = true;
                        lblAlertMsg.Text = errorms; //"Please Choose Marks Any Images And Then Proceed";
                        return;
                    }
                    else
                    {
                        quesHasImage = true;
                        questionImageFileName = Path.GetFileName(fuQuestionImage.PostedFile.FileName);
                        questionImageLenth = 0;
                        //int fileSize = fuQuestionImage.PostedFile.ContentLength;
                        questionImageLenth = fuQuestionImage.PostedFile.ContentLength;
                        quesImage = new byte[questionImageLenth];
                        fuQuestionImage.PostedFile.InputStream.Read(quesImage, 0, questionImageLenth);
                        //if (img_uplod.FileName.EndsWith(".xls") || FileUpload1.FileName.EndsWith(".xlsx"))
                        questionImageType = Path.GetExtension(fuQuestionImage.PostedFile.FileName);
                        questionImageType = questionImageType.ToLower();
                        questionImageType = GetImageFormat(questionImageType);
                        if (questionImageFileName != "" && questionImageLenth != 0 && questionImageType != "" && !string.IsNullOrEmpty(questionImageType))
                        {
                        }
                    }
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Please Choose Any Images And Then Proceed";
                    return;
                }
            }
            //Objective
            if (isObjectiveorDescriptive)
            {
                if (rblQuestionType.Items.Count > 0)
                {
                    questionType = rblQuestionType.SelectedValue.Trim();
                    switch (questionType)
                    {
                        case "1":
                        case "2":
                        case "4":
                            if (questionType == "1")
                            {
                                if (rblSingleorMutiChoice.Items.Count > 0)
                                {
                                    questionSubType = rblSingleorMutiChoice.SelectedValue;
                                }
                            }
                            else
                            {
                                questionSubType = "1";
                            }
                            if (string.IsNullOrEmpty(totalOptions.Trim()))
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Visible = true;
                                lblAlertMsg.Text = "Please Enter The No. of Options And Then Proceed";
                                return;
                            }
                            else
                            {
                                if (gvQOptions.Rows.Count > 0)
                                {
                                    qOptions = string.Empty;
                                    qAnswer = string.Empty;
                                    string newqAnswer = string.Empty;
                                    int optCount = 0;
                                    int optAnsCount = 0;
                                    for (int option = 0; option < gvQOptions.Rows.Count; option++)
                                    {
                                        Array.Resize(ref quesOptions, option + 1);
                                        Array.Resize(ref quesMatchRight, option + 1);
                                        Array.Resize(ref quesOptionAnwer, option + 1);
                                        TextBox txtOptionValue = (TextBox)gvQOptions.Rows[option].FindControl("txtQOption");
                                        CheckBox anser = (CheckBox)gvQOptions.Rows[option].FindControl("chkQOptionAnswer");
                                        string optn = Convert.ToString(txtOptionValue.Text).Trim();
                                        quesOptions[option] = string.Empty;
                                        quesMatchRight[option] = string.Empty;
                                        quesMatchLR[option, 0] = string.Empty;

                                        if (optn.Trim() != "")
                                        {
                                            optCount++;
                                            optn = Convert.ToString(optn.Replace("'", "''"));
                                            optn = Convert.ToString(optn.Replace("'", "''"));
                                            optn = Convert.ToString(optn.Replace("‘", "''"));
                                            optn = Convert.ToString(optn.Replace("’", "''"));
                                            quesOptions[option] = optn;
                                            quesMatchRight[option] = string.Empty;
                                            quesMatchLR[option, 0] = optn;
                                            qOptions = qOptions + optn + "#malang#";
                                        }
                                        if (anser.Checked == true)
                                        {
                                            optAnsCount++;
                                            Array.Resize(ref quesAnswers, optAnsCount);
                                            if (string.IsNullOrEmpty(qAnswer.Trim()))
                                            {
                                                qAnswer = Convert.ToString(optn);
                                            }
                                            else
                                            {
                                                qAnswer += "#ans#" + Convert.ToString(optn);
                                            }
                                            quesOptionAnwer[option] = true;
                                            quesAnswers[optAnsCount - 1] = Convert.ToString(optn);
                                        }
                                        else
                                        {
                                            quesOptionAnwer[option] = false;
                                        }
                                    }
                                    if (optCount == 0)
                                    {
                                        divPopAlert.Visible = true;
                                        lblAlertMsg.Visible = true;
                                        lblAlertMsg.Text = "Please Enter The Options Name And Then Proceed";
                                        return;
                                    }
                                    else if (optCount != gvQOptions.Rows.Count)
                                    {
                                        divPopAlert.Visible = true;
                                        lblAlertMsg.Visible = true;
                                        lblAlertMsg.Text = "Please Fill All The Options Name And Then Proceed";
                                        return;
                                    }
                                    if (optAnsCount == 0)
                                    {
                                        divPopAlert.Visible = true;
                                        lblAlertMsg.Visible = true;
                                        lblAlertMsg.Text = "Please Select Any One Option As Answer";
                                        return;
                                    }
                                    newqAnswer = string.Join("#ans#", quesAnswers);
                                }
                            }
                            break;
                        case "3":
                            isMatching = true;
                            qmatchingName = string.Empty;
                            string matchType = string.Empty;
                            if (ddlMatchSubType.Items.Count > 0)
                            {
                                matchType = Convert.ToString(ddlMatchSubType.SelectedValue).Trim();
                            }
                            if (rblMatchSubType.Items.Count > 0)
                            {
                                matchType = Convert.ToString(rblMatchSubType.SelectedValue).Trim();
                            }
                            questionSubType = matchType;
                            if (string.IsNullOrEmpty(totalQuestions.Trim()))
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Visible = true;
                                lblAlertMsg.Text = "Please Enter The No. of Questions And Then Proceed";
                                return;
                            }
                            else
                            {
                                if (gvMatchQuestion.Rows.Count > 0)
                                {
                                    qOptions = string.Empty;
                                    qAnswer = string.Empty;
                                    int matchCount = 0;
                                    int matchAnsCount = 0;
                                    for (int match = 0; match < gvMatchQuestion.Rows.Count; match++)
                                    {
                                        Array.Resize(ref quesOptions, match + 1);
                                        Array.Resize(ref quesOptionAnwer, match + 1);
                                        Array.Resize(ref quesMatchRight, match + 1);

                                        Label lblQMatchSno = (Label)gvMatchQuestion.Rows[match].FindControl("lblQMatchSno");
                                        Label lblQuesName = (Label)gvMatchQuestion.Rows[match].FindControl("lblMatchQuestions");
                                        Label lblMatchAnsSno = (Label)gvMatchQuestion.Rows[match].FindControl("lblMatchAnsSno");

                                        TextBox txtQuestionsName = gvMatchQuestion.Rows[match].FindControl("txtMatchQuestions") as TextBox;
                                        TextBox txtQAnswer = gvMatchQuestion.Rows[match].FindControl("txtMatchAnswer") as TextBox;

                                        FileUpload fuQMatch = gvMatchQuestion.Rows[match].FindControl("fuLhsQMatch") as FileUpload;
                                        FileUpload fuAnsMatch = gvMatchQuestion.Rows[match].FindControl("fuRhsAMatch") as FileUpload;

                                        System.Web.UI.WebControls.Image imgQMatch = gvMatchQuestion.Rows[match].FindControl("imgLhsQMatch") as System.Web.UI.WebControls.Image;
                                        System.Web.UI.WebControls.Image imgAnsMatch = gvMatchQuestion.Rows[match].FindControl("imgRhsAMatch") as System.Web.UI.WebControls.Image;
                                        quesOptions[match] = string.Empty;
                                        quesMatchRight[match] = string.Empty;
                                        quesMatchLR[match, 0] = string.Empty;
                                        quesMatchLR[match, 1] = string.Empty;

                                        string Lhs = txtQuestionsName.Text.Trim();
                                        string rhs = txtQAnswer.Text.Trim();

                                        if (!string.IsNullOrEmpty(Lhs.Trim()))
                                        {
                                            matchCount++;
                                            Lhs = Convert.ToString(Lhs.Replace("'", "''"));
                                            Lhs = Convert.ToString(Lhs.Replace("'", "''"));
                                            Lhs = Convert.ToString(Lhs.Replace("‘", "''"));
                                            Lhs = Convert.ToString(Lhs.Replace("’", "''"));
                                            quesOptions[match] = Lhs.Trim();
                                            quesMatchLR[match, 0] = Lhs.Trim();
                                            //qOptions = qOptions + Lhs + "#malang#";
                                        }
                                        if (!string.IsNullOrEmpty(rhs.Trim()))
                                        {
                                            quesMatchRight[match] = rhs.Trim();
                                            quesMatchLR[match, 1] = rhs.Trim();
                                        }
                                        if (!string.IsNullOrEmpty(Lhs.Trim()))
                                        {
                                            string m_question = Convert.ToString(Lhs.Trim());
                                            m_question = Convert.ToString(m_question.Replace("'", "''"));
                                            m_question = Convert.ToString(m_question.Replace("‘", "''"));
                                            m_question = Convert.ToString(m_question.Replace("’", "''"));
                                            string m_answer = Convert.ToString(rhs);
                                            m_answer = Convert.ToString(m_answer.Replace("'", "''"));
                                            m_answer = Convert.ToString(m_answer.Replace("‘", "''"));
                                            m_answer = Convert.ToString(m_answer.Replace("’", "''"));
                                            if (rhs.Trim() != "")
                                            {
                                                m_answer = ";" + m_answer;
                                            }
                                            qmatchingName = qmatchingName + m_question + m_answer + "^";
                                        }
                                    }
                                }
                            }
                            switch (matchType)
                            {
                                case "3":
                                default:
                                    break;
                                case "4":
                                    break;
                                case "5":
                                    break;
                                case "6":
                                    break;
                            }
                            break;
                        case "5":
                            if (string.IsNullOrEmpty(totalQuestions.Trim()))
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Visible = true;
                                lblAlertMsg.Text = "Please Enter The No. of Questions And Then Proceed";
                                return;
                            }
                            if (string.IsNullOrEmpty(totalOptions.Trim()))
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Visible = true;
                                lblAlertMsg.Text = "Please Enter The No. of Questions And Then Proceed";
                                return;
                            }
                            else
                            {
                                questionPara = new string[totalQuestionCount];
                                //quesOptionAnwer = new bool[totalQuestionCount];
                                questionParaAns = new string[totalQuestionCount];
                                questionParaopt = new string[totalQuestionCount, totalOptionCount];
                                qOptions = string.Empty;
                                qAnswer = string.Empty;
                                string newans = string.Empty;
                                if (gvParagraph.Rows.Count > 0)
                                {
                                    for (int qpara = 0; qpara < gvParagraph.Rows.Count; qpara++)
                                    {
                                        TextBox txtQuestionsPara = gvParagraph.Rows[qpara].FindControl("txtParaQuestions" + qpara) as TextBox;
                                        TextBox txtQuestionsParaAns = gvParagraph.Rows[qpara].FindControl("txtParaAnswers" + qpara) as TextBox;
                                        questionPara[qpara] = string.Empty;
                                        questionParaAns[qpara] = string.Empty;
                                        if (!string.IsNullOrEmpty(txtQuestionsPara.Text.Trim()))
                                        {
                                            questionPara[qpara] = txtQuestionsPara.Text.Trim();
                                        }
                                        //if (!string.IsNullOrEmpty(txtQuestionsParaAns.Text.Trim()))
                                        //{
                                        //    questionParaAns[qpara] = txtQuestionsParaAns.Text.Trim();
                                        //}
                                        //qAnswer += "#Qpara#" + txtQuestionsParaAns.Text.Trim();
                                        //string opt = string.Empty;
                                        //int op = 0;
                                        //for (int qparaopt = 3; qparaopt < gvParagraph.Rows[qpara].Cells.Count; qparaopt++)
                                        //{
                                        //    TextBox txtopt = gvParagraph.Rows[qpara].FindControl("txtParaOptions" + qpara + (qparaopt + 1)) as TextBox;
                                        //    questionParaopt[qpara, op] = string.Empty;
                                        //    if (!string.IsNullOrEmpty(txtopt.Text.Trim()))
                                        //    {
                                        //        questionParaopt[qpara, op] = txtopt.Text.Trim();
                                        //    }
                                        //    opt += "#Qparaopt#" + questionParaopt[qpara, op];
                                        //    op++;
                                        //}
                                        //qOptions += opt + "#Qpara#";
                                        //qOptions += string.Join("Q" + qpara, questionParaopt[qpara]);
                                    }
                                }
                                qmatchingName = string.Join("#Qpara#", questionPara);
                                //newans = string.Join("#Qpara#", questionParaAns);
                            }
                            break;
                        case "6":
                            if (string.IsNullOrEmpty(totalQuestions.Trim()))
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Visible = true;
                                lblAlertMsg.Text = "Please Enter The No. of Questions And Then Proceed";
                                return;
                            }
                            if (string.IsNullOrEmpty(totalOptions.Trim()))
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Visible = true;
                                lblAlertMsg.Text = "Please Enter The No. of Questions And Then Proceed";
                                return;
                            }
                            else
                            {
                                questionPara = new string[totalQuestionCount];
                                //quesOptionAnwer = new bool[totalQuestionCount];
                                questionParaAns = new string[totalQuestionCount];
                                questionParaopt = new string[totalQuestionCount, totalOptionCount];
                                qOptions = string.Empty;
                                qAnswer = string.Empty;
                                string newans = string.Empty;
                                if (gvParagraph.Rows.Count > 0)
                                {
                                    for (int qpara = 0; qpara < gvParagraph.Rows.Count; qpara++)
                                    {
                                        TextBox txtQuestionsPara = gvParagraph.Rows[qpara].FindControl("txtParaQuestions" + qpara) as TextBox;
                                        TextBox txtQuestionsParaAns = gvParagraph.Rows[qpara].FindControl("txtParaAnswers" + qpara) as TextBox;
                                        questionPara[qpara] = string.Empty;
                                        questionParaAns[qpara] = string.Empty;
                                        if (!string.IsNullOrEmpty(txtQuestionsPara.Text.Trim()))
                                        {
                                            questionPara[qpara] = txtQuestionsPara.Text.Trim();
                                        }
                                        if (!string.IsNullOrEmpty(txtQuestionsParaAns.Text.Trim()))
                                        {
                                            questionParaAns[qpara] = txtQuestionsParaAns.Text.Trim();
                                        }
                                        qAnswer += "#Qpara#" + txtQuestionsParaAns.Text.Trim();
                                        string opt = string.Empty;
                                        int op = 0;
                                        for (int qparaopt = 3; qparaopt < gvParagraph.Rows[qpara].Cells.Count; qparaopt++)
                                        {
                                            TextBox txtopt = gvParagraph.Rows[qpara].FindControl("txtParaOptions" + qpara + (qparaopt + 1)) as TextBox;
                                            questionParaopt[qpara, op] = string.Empty;
                                            if (!string.IsNullOrEmpty(txtopt.Text.Trim()))
                                            {
                                                questionParaopt[qpara, op] = txtopt.Text.Trim();
                                            }
                                            opt += "#Qparaopt#" + questionParaopt[qpara, op];
                                            op++;
                                        }
                                        qOptions += opt + "#Qpara#";
                                        //qOptions += string.Join("Q" + qpara, questionParaopt[qpara]);
                                    }
                                }
                                qmatchingName = string.Join("#Qpara#", questionPara);
                                newans = string.Join("#Qpara#", questionParaAns);
                            }
                            break;
                    }
                    if (!string.IsNullOrEmpty(questionType.Trim()) && !questionType.Trim().Contains('1') && !questionType.Trim().Contains('2') && !questionType.Trim().Contains('4') && !questionType.Trim().Contains('6'))
                    {
                        if (divOptions.Visible)
                        {
                            if (string.IsNullOrEmpty(totalOptions.Trim()))
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Visible = true;
                                lblAlertMsg.Text = "Please Enter The No. of Options And Then Proceed";
                                return;
                            }
                            else
                            {
                                if (gvQOptions.Rows.Count > 0)
                                {
                                    qOptions = string.Empty;
                                    qAnswer = string.Empty;
                                    int optCount = 0;
                                    int optAnsCount = 0;
                                    for (int option = 0; option < gvQOptions.Rows.Count; option++)
                                    {
                                        TextBox txtOptionValue = (TextBox)gvQOptions.Rows[option].FindControl("txtQOption");
                                        CheckBox anser = (CheckBox)gvQOptions.Rows[option].FindControl("chkQOptionAnswer");
                                        string optn = Convert.ToString(txtOptionValue.Text).Trim();
                                        if (optn.Trim() != "")
                                        {
                                            optCount++;
                                            optn = Convert.ToString(optn.Replace("'", "''"));
                                            optn = Convert.ToString(optn.Replace("'", "''"));
                                            optn = Convert.ToString(optn.Replace("‘", "''"));
                                            optn = Convert.ToString(optn.Replace("’", "''"));
                                            qOptions = qOptions + optn + "#malang#";
                                        }
                                        if (anser.Checked == true)
                                        {
                                            optAnsCount++;
                                            qAnswer = Convert.ToString(optn);
                                        }
                                    }
                                    if (optCount == 0)
                                    {
                                        divPopAlert.Visible = true;
                                        lblAlertMsg.Visible = true;
                                        lblAlertMsg.Text = "Please Enter The Options Name And Then Proceed";
                                        return;
                                    }
                                    else if (optCount != gvQOptions.Rows.Count)
                                    {
                                        divPopAlert.Visible = true;
                                        lblAlertMsg.Visible = true;
                                        lblAlertMsg.Text = "Please Fill All The Options Name And Then Proceed";
                                        return;
                                    }
                                    if (optAnsCount == 0)
                                    {
                                        divPopAlert.Visible = true;
                                        lblAlertMsg.Visible = true;
                                        lblAlertMsg.Text = "Please Select Any One Option As Answer";
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                qAnswer = string.Empty;
                qAnswer = txtQuestionAnswer.Text.Trim();
            }

            if (!string.IsNullOrEmpty(questionName.Trim()) && !string.IsNullOrEmpty(questionpk.Trim()))
            {
                //,file_name='" + questionImageFileName.Trim() + "',file_type='" + questionImageType.Trim() + "',quetion_image='" + quesImage + "'
                //qry = "if exists (select * from tbl_question_master where QuestionMasterPK='" + questionpk + "' ) update tbl_question_master set subject_no='" + subjectNo + "' ,  syllabus='" + topicNo + "' ,  is_descriptive='" + quesObjectiveDescript + "', mark='" + questionMarks + "', question =N'" + questionName.Trim() + "',  type='" + questionGrade.Trim() + "' , options=N'" + qOptions.Trim() + "',answer=N'" + qAnswer.Trim() + "',Already_exist='0',QuestionType='" + questionType.Trim() + "',QuestionSubType='" + questionSubType.Trim() + "',totalChoice='" + totalOptionCount + "',is_matching='" + isMatching + "',qmatching='" + qmatchingName + "',needChoice='" + chkNeedOptions.Checked + "'  where QuestionMasterPK='" + questionpk + "'  else  insert into tbl_question_master (subject_no, syllabus, is_descriptive, question , type,options,mark,answer,QuestionType,QuestionSubType,totalChoice,is_matching,qmatching,Already_exist,needChoice) values('" + subjectNo + "','" + topicNo + "' ,'" + quesObjectiveDescript + "',N'" + questionName.Trim() + "','" + questionGrade.Trim() + "',N'" + qOptions.Trim() + "','" + questionMarks.Trim() + "',N'" + qAnswer.Trim() + "','" + questionType.Trim() + "','" + questionSubType.Trim() + "','" + totalOptionCount + "','" + isMatching + "','" + qmatchingName + "','0','" + chkNeedOptions.Checked + "')";
                if (!chklst_general.Checked && !string.IsNullOrEmpty(subjectNo) && !string.IsNullOrEmpty(topicNo))
                    qry = "if exists (select * from tbl_question_master where subject_no='" + subjectNo + "' and question =N'" + questionName.Trim() + "' and syllabus='" + topicNo + "' and is_descriptive='" + quesObjectiveDescript + "' and General='1' ) update tbl_question_master set subject_no='" + subjectNo + "' ,  syllabus='" + topicNo + "' ,  is_descriptive='" + quesObjectiveDescript + "', mark='" + questionMarks + "', question =N'" + questionName.Trim() + "',  type='" + questionGrade.Trim() + "' , options=N'" + qOptions.Trim() + "',answer=N'" + qAnswer.Trim() + "',Already_exist='0',QuestionType='" + questionType.Trim() + "',QuestionSubType='" + questionSubType.Trim() + "',totalChoice='" + totalOptionCount + "',is_matching='" + isMatching + "',qmatching='" + qmatchingName + "',needChoice='" + chkNeedOptions.Checked + "',General='1'  where subject_no='" + subjectNo + "' and question =N'" + questionName.Trim() + "' and syllabus='" + topicNo + "' and is_descriptive='" + quesObjectiveDescript + "'      else  insert into tbl_question_master ( subject_no, syllabus, is_descriptive, question , type,options,mark,answer,QuestionType,QuestionSubType,totalChoice,is_matching,qmatching,Already_exist,needChoice,General) values('" + subjectNo + "','" + topicNo + "' ,'" + quesObjectiveDescript + "',N'" + questionName.Trim() + "','" + questionGrade.Trim() + "',N'" + qOptions.Trim() + "','" + questionMarks.Trim() + "',N'" + qAnswer.Trim() + "','" + questionType.Trim() + "','" + questionSubType.Trim() + "','" + totalOptionCount + "','" + isMatching + "','" + qmatchingName + "','0','" + chkNeedOptions.Checked + "','1')";
                else
                    qry = "if exists (select * from tbl_question_master where question =N'" + questionName.Trim() + "'  and is_descriptive='" + quesObjectiveDescript + "' and General='0' ) update tbl_question_master set  is_descriptive='" + quesObjectiveDescript + "', mark='" + questionMarks + "', question =N'" + questionName.Trim() + "',  type='" + questionGrade.Trim() + "' , options=N'" + qOptions.Trim() + "',answer=N'" + qAnswer.Trim() + "',Already_exist='0',QuestionType='" + questionType.Trim() + "',QuestionSubType='" + questionSubType.Trim() + "',totalChoice='" + totalOptionCount + "',is_matching='" + isMatching + "',qmatching='" + qmatchingName + "',needChoice='" + chkNeedOptions.Checked + "',General='0'  where  question =N'" + questionName.Trim() + "' and is_descriptive='" + quesObjectiveDescript + "'     else  insert into tbl_question_master (is_descriptive, question , type,options,mark,answer,QuestionType,QuestionSubType,totalChoice,is_matching,qmatching,Already_exist,needChoice,General) values('" + quesObjectiveDescript + "',N'" + questionName.Trim() + "','" + questionGrade.Trim() + "',N'" + qOptions.Trim() + "','" + questionMarks.Trim() + "',N'" + qAnswer.Trim() + "','" + questionType.Trim() + "','" + questionSubType.Trim() + "','" + totalOptionCount + "','" + isMatching + "','" + qmatchingName + "','0','" + chkNeedOptions.Checked + "','0')";
                int insert = d2.update_method_wo_parameter(qry, "Text");
                if (insert != 0)
                {
                    isSuccess = true;
                    string questionID = "";

                    if (!chklst_general.Checked && !string.IsNullOrEmpty(subjectNo) && !string.IsNullOrEmpty(topicNo))
                        questionID = d2.GetFunctionv("select QuestionMasterPK from tbl_question_master where subject_no='" + subjectNo + "' and question =N'" + questionName.Trim() + "' and syllabus='" + topicNo + "' and is_descriptive='" + quesObjectiveDescript + "'");
                    else
                        questionID = d2.GetFunctionv("select QuestionMasterPK from tbl_question_master where  question =N'" + questionName.Trim() + "'  and is_descriptive='" + quesObjectiveDescript + "' and General='0'");

                    if (quesHasImage)
                    {
                        if (questionImageFileName != "" && questionImageLenth != 0 && !string.IsNullOrEmpty(questionImageType) && quesImage != null)
                        {
                            if (!chklst_general.Checked && !string.IsNullOrEmpty(subjectNo) && !string.IsNullOrEmpty(topicNo))
                                qry = "if not exists ( select * from tbl_question_master where subject_no='" + subjectNo + "' and question =N'" + questionName.Trim() + "' and syllabus='" + topicNo + "' and is_descriptive='" + quesObjectiveDescript + "' and General='1'  )  insert into tbl_question_master (file_name,quetion_image,file_type) values(@file_name,@quetion_image,@file_type) else update tbl_question_master set file_name=@file_name,file_type=@file_type,quetion_image=@quetion_image where subject_no='" + subjectNo + "' and question =N'" + questionName.Trim() + "' and syllabus='" + topicNo + "' and is_descriptive='" + quesObjectiveDescript + "'";
                            else
                                qry = "if not exists ( select * from tbl_question_master where  question =N'" + questionName.Trim() + "' and General='0' and is_descriptive='" + quesObjectiveDescript + "'  )  insert into tbl_question_master (file_name,quetion_image,file_type) values(@file_name,@quetion_image,@file_type) else update tbl_question_master set file_name=@file_name,file_type=@file_type,quetion_image=@quetion_image where  question =N'" + questionName.Trim() + "' and syllabus='" + topicNo + "' and is_descriptive='" + quesObjectiveDescript + "' and General='0'";
                            SqlParameter[] sqlpara = new SqlParameter[3];
                            sqlpara[0] = new SqlParameter("@file_name", SqlDbType.NVarChar, 300);
                            sqlpara[0].Value = questionImageFileName;

                            sqlpara[1] = new SqlParameter("@quetion_image", SqlDbType.Image, questionImageLenth);
                            sqlpara[1].Value = quesImage;

                            sqlpara[2] = new SqlParameter("@file_type", SqlDbType.NVarChar, 300);
                            sqlpara[2].Value = questionImageType.ToString();

                            isSuccess = InsertImageQuery(qry, sqlpara);

                        }
                    }
                    else if (img.Length > 1 && imgQuestions.Visible == true && Session["imgQuestionsToPopUp"] != null)
                    {
                        qry = " update tbl_question_master set file_name=@file_name,file_type=@file_type,quetion_image=@quetion_image where   QuestionMasterPK='" + questionpk + "'";
                        qry = "if not exists ( select * from tbl_question_master where QuestionMasterPK='" + questionpk + "'  )  insert into tbl_question_master (file_name,quetion_image,file_type) values(@file_name,@quetion_image,@file_type) else update tbl_question_master set file_name=@file_name,file_type=@file_type,quetion_image=@quetion_image where QuestionMasterPK='" + questionpk + "'";

                        SqlParameter[] sqlpara = new SqlParameter[3];
                        sqlpara[0] = new SqlParameter("@file_name", SqlDbType.NVarChar, 300);
                        sqlpara[0].Value = ((Session["imgName"] == null) ? "newImage.jpg" : Convert.ToString(Session["imgName"]).Trim());

                        sqlpara[1] = new SqlParameter("@quetion_image", SqlDbType.Image, questionImageLenth);
                        sqlpara[1].Value = img;

                        sqlpara[2] = new SqlParameter("@file_type", SqlDbType.NVarChar, 300);
                        sqlpara[2].Value = ((Session["imgType"] == null) ? "image/jpg" : Convert.ToString(Session["imgType"]).Trim());

                        isSuccess = InsertImageQuery(qry, sqlpara);

                    }

                    //qry = "";
                    if (!string.IsNullOrEmpty(questionpk.Trim()))
                    {
                        qry = "delete from QuestionsChoice where QuestionID='" + questionpk.Trim() + "'";
                        int del = d2.update_method_wo_parameter(qry, "Text");
                        for (int opt = 0; opt < quesOptions.Length; opt++)
                        {
                            qry = "if exists (select * from QuestionsChoice where QuestionID=@QuestionID and choiceNo=@choiceNo) update QuestionsChoice set QChoice=@QChoice,isAnswer=@isAnswer,QChoiceImage=@QChoiceImage,QMatchR=@QMatchR,QChoiceImageR=@QChoiceImageR,isMatching=@isMatching where QuestionID=@QuestionID and choiceNo=@choiceNo else insert into QuestionsChoice (QuestionID,choiceNo,QChoice,QChoiceImage,isAnswer,QMatchR,QChoiceImageR,isMatching)  values(@QuestionID,@choiceNo,@QChoice,@QChoiceImage,@isAnswer,@QMatchR,@QChoiceImageR,@isMatching)";

                            SqlParameter[] sqlpara = new SqlParameter[8];

                            sqlpara[0] = new SqlParameter("@QuestionID", SqlDbType.NVarChar, 300);
                            sqlpara[0].Value = questionID.Trim();

                            sqlpara[1] = new SqlParameter("@choiceNo", SqlDbType.NVarChar, 300);
                            sqlpara[1].Value = Convert.ToString((opt + 1)).Trim();

                            sqlpara[2] = new SqlParameter("@QChoice", SqlDbType.NVarChar, 300);
                            sqlpara[2].Value = quesOptions[opt].Trim();

                            sqlpara[3] = new SqlParameter("@QMatchR", SqlDbType.NVarChar, 300);
                            sqlpara[3].Value = quesMatchRight[opt].Trim();

                            byte[] leftImage = new byte[0];
                            int leftLength = 0;
                            string leftImageName = string.Empty;
                            string LeftImageType = string.Empty;

                            byte[] rightImage = new byte[0];
                            int rightLength = 0;
                            string rightImageType = string.Empty;
                            string rightImageName = string.Empty;
                            if (isMatching)
                            {
                                if (gvMatchQuestion.Rows.Count > 0)
                                {
                                    if (opt < gvMatchQuestion.Rows.Count)
                                    {
                                        FileUpload fuQMatch = gvMatchQuestion.Rows[opt].FindControl("fuLhsQMatch") as FileUpload;
                                        FileUpload fuAnsMatch = gvMatchQuestion.Rows[opt].FindControl("fuRhsAMatch") as FileUpload;
                                        Label lblQuesName = gvMatchQuestion.Rows[opt].FindControl("lblMatchQuestions") as Label;
                                        Label lblRhsFile = gvMatchQuestion.Rows[opt].FindControl("lblRhsFile") as Label;

                                        System.Web.UI.WebControls.Image imgQMatch = gvMatchQuestion.Rows[opt].FindControl("imgLhsQMatch") as System.Web.UI.WebControls.Image;
                                        System.Web.UI.WebControls.Image imgAnsMatch = gvMatchQuestion.Rows[opt].FindControl("imgRhsAMatch") as System.Web.UI.WebControls.Image;

                                        string errorms = string.Empty;
                                        if (fuQMatch.HasFile)
                                        {
                                            if (CheckValidFiles(fuQMatch, out errorms))
                                            {
                                                leftImageName = Path.GetFileName(fuQMatch.PostedFile.FileName);
                                                leftLength = 0;
                                                leftLength = fuQMatch.PostedFile.ContentLength;
                                                leftImage = new byte[leftLength];
                                                fuQMatch.PostedFile.InputStream.Read(leftImage, 0, leftLength);
                                                LeftImageType = Path.GetExtension(fuQMatch.PostedFile.FileName);
                                                LeftImageType = LeftImageType.ToLower();
                                                LeftImageType = GetImageFormat(LeftImageType);
                                                if (leftImageName != "" && leftLength != 0 && LeftImageType != "" && !string.IsNullOrEmpty(LeftImageType))
                                                {
                                                }
                                            }
                                        }
                                        else if (!string.IsNullOrEmpty(lblQuesName.Text))
                                        {
                                            leftImage = Convert.FromBase64String(lblQuesName.Text);
                                        }
                                        if (fuAnsMatch.HasFile)
                                        {
                                            if (CheckValidFiles(fuAnsMatch, out errorms))
                                            {
                                                rightImageName = Path.GetFileName(fuAnsMatch.PostedFile.FileName);
                                                rightLength = 0;
                                                rightLength = fuAnsMatch.PostedFile.ContentLength;
                                                rightImage = new byte[rightLength];
                                                fuAnsMatch.PostedFile.InputStream.Read(rightImage, 0, rightLength);
                                                rightImageType = Path.GetExtension(fuAnsMatch.PostedFile.FileName);
                                                rightImageType = rightImageType.ToLower();
                                                rightImageType = GetImageFormat(rightImageType);
                                                if (rightImageName != "" && rightLength != 0 && rightImageType != "" && !string.IsNullOrEmpty(rightImageType))
                                                {
                                                }
                                            }
                                        }
                                        else if (!string.IsNullOrEmpty(lblRhsFile.Text))
                                        {
                                            rightImage = Convert.FromBase64String(lblRhsFile.Text);
                                        }
                                    }
                                }
                            }

                            sqlpara[4] = new SqlParameter("@QChoiceImage", SqlDbType.Image, leftImage.Length);
                            sqlpara[4].Value = leftImage;

                            sqlpara[5] = new SqlParameter("@QChoiceImageR", SqlDbType.Image, rightImage.Length);
                            sqlpara[5].Value = rightImage;

                            sqlpara[6] = new SqlParameter("@isAnswer", SqlDbType.NVarChar, 300);
                            sqlpara[6].Value = quesOptionAnwer[opt];

                            sqlpara[7] = new SqlParameter("@isMatching", SqlDbType.NVarChar, 300);
                            sqlpara[7].Value = isMatching;

                            isSuccess = InsertImageQuery(qry, sqlpara);
                        }
                    }
                }
            }
            btn_go_Click(sender, e);
            //FpSpread1_OnCellClick(sender, e);
            //FpSpread1_Selectedindexchange(sender, e);
            //FpSpread2_OnCellClick(sender, e);
            //FpSpread2_Selectedindexchange(sender, e);
            if (isSuccess)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Saved Successfully";
                return;
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Not Saved";
                return;
            }

        }
        catch (Exception ex)
        {
        }
    }

    #endregion Save Questions

    #region Delete Question

    protected void btn_Delete_Click(object sender, EventArgs e)
    {
        try
        {

            string questionanswerpk = Convert.ToString(ViewState["QuestionMasterPK"]);

            string insertqry = "delete tbl_question_master where QuestionMasterPK='" + questionanswerpk + "'  ";

            int insert = d2.update_method_wo_parameter(insertqry, "Text");
            if (insert != 0)
            {
                Add_questiontype.Visible = false;
                imgdiv3.Visible = true;
                lbl_alert.Text = "Deleted Successfully";
                txt_questionname.Text = "";
                txt_nooption.Text = "";
                txt_marks.Text = "";
                txt_answer.Text = "";
                rb_Easy.Checked = true;
                rb_medium.Checked = false;
                rb_difficult.Checked = false;
                rb_hard.Checked = false;
                for (int i = 0; i < gridView1.Rows.Count; i++)
                {
                    (gridView1.Rows[i].FindControl("txtOption") as TextBox).Text = "";
                    (gridView1.Rows[i].FindControl("cb_answer") as CheckBox).Checked = false;
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

    #endregion Delete Question

    #region Reusable Methods

    #region  Set Default Values To Question Master

    public void SetDefaultValuesOfQuestionMaster(int type = 0)
    {
        try
        {
            if (rblObjectiveDescriptive.Items.Count > 0)
            {
                divObjective.Visible = false;
                divDescriptive.Visible = false;
                txtQuestionAnswer.Text = string.Empty;
                //tdDescript.Attributes.Add("style", "display:none;");
                tdDescript.Visible = false;
                divOptions.Visible = false;
                Session["ParaGraphQuestions"] = null;
                txtQuestionName.Text = string.Empty;
                chkAddQuesImage.Checked = false;
                fuQuestionImage.Visible = false;
                divParagraph.Visible = false;
                divSubType.Visible = false;
                //fuQuestionImage.HasFile = false;

                divMatches.Visible = false;

                if (ddlQMarks.Items.Count > 0)
                {
                    ddlQMarks.SelectedIndex = 0;
                }
                if (rblQuestionGrading.Items.Count > 0)
                {
                    rblQuestionGrading.SelectedValue = "0";
                }
                if (type == 0)
                {
                    rblObjectiveDescriptive.SelectedValue = "0";
                    if (rblQuestionType.Items.Count > 0)
                    {
                        divObjective.Visible = true;
                        divDescriptive.Visible = false;
                        rblQuestionType.SelectedValue = "1";
                        divMatchSubType.Visible = false;
                        //rblSingleorMutiChoice.Attributes.Add("style", "display:table-cell;");
                        rblSingleorMutiChoice.Visible = true;
                        divMatches.Visible = false;
                        divOptions.Visible = false;
                        rblMatchSubType.Visible = false;
                        //divOptions.Attributes.Add("style", "display:none;");
                        //rblMatchSubType.Attributes.Add("style", "display:none;");

                        divSubType.Visible = true;
                        txtNoofQuestionCount.Text = string.Empty;
                        lblMQuestionCount.Visible = false;
                        txtNoofQuestionCount.Visible = false;

                        txtNoofOptionsCount.Text = string.Empty;
                        lblNoofOptions.Visible = true;
                        txtNoofOptionsCount.Visible = true;
                        txtNoofOptionsCount.Enabled = true;

                        if (rblSingleorMutiChoice.Items.Count > 0)
                        {
                            rblSingleorMutiChoice.SelectedValue = "1";
                        }
                        if (ddlMatchSubType.Items.Count > 0)
                        {
                            ddlMatchSubType.SelectedIndex = 0;
                        }
                        if (rblMatchSubType.Items.Count > 0)
                        {
                            rblMatchSubType.SelectedIndex = 0;
                        }
                    }
                }
                else
                {
                    rblObjectiveDescriptive.SelectedValue = "1";
                    //tdDescript.Attributes.Add("style", "display:table-row;");
                    tdDescript.Visible = true;
                    divDescriptive.Visible = true;
                }
            }
            chkAddQuesImage.Checked = false;
            fuQuestionImage.Visible = false;
            imgQuestionImage.Visible = false;
            txtQuestionName.Text = string.Empty;
            txtQMarks.Text = string.Empty;
            txtAddQmark.Text = string.Empty;
            lblAlertMsg.Text = string.Empty;
            lblWarningMsgs.Text = string.Empty;
        }
        catch (Exception ex)
        {

        }
    }

    #endregion Set Default Values To Question Master

    public string GetImageFormat(string file_extension)
    {
        string file_type = string.Empty;
        try
        {
            file_extension = file_extension.Trim().ToLower();
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            switch (file_extension)
            {
                case ".gif":
                    file_type = "image/gif";
                    break;
                case ".png":
                    file_type = "image/png";
                    break;
                case ".jpg":
                    file_type = "image/jpg";
                    break;
                case ".jpeg":
                    file_type = "image/jpeg";
                    break;
            }
            return file_type;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fuQImage">FileUpload Control With Files</param>
    /// <param name="type">1 - Means Image Type File(.jpg,.jpeg,.png,.gif); 2 - Means Document Type Files (.doc,.docx,.pdf,.txt)</param>
    /// <returns></returns>
    public bool CheckValidFiles(FileUpload fuQImage, out string errormsg, int type = 1)
    {
        bool isValidFile = false;
        bool isExceedSize = false;
        errormsg = string.Empty;
        try
        {
            if (fuQImage.HasFile)
            {
                string filename = fuQImage.FileName;
                string extension = string.Empty;
                string fileExtension = Path.GetExtension(fuQImage.PostedFile.FileName);
                extension = fileExtension.Trim().ToLower();
                switch (type)
                {
                    case 1:
                    default:
                        switch (extension)
                        {
                            case ".jpg":
                                isValidFile = true;
                                break;
                            case ".gif":
                                isValidFile = true;
                                break;
                            case ".jpeg":
                                isValidFile = true;
                                break;
                            case ".png":
                                isValidFile = true;
                                break;
                        }
                        if (!isValidFile)
                            errormsg = "It Allows .jpg,.jpeg,.png and .gif Image Format Only";
                        if (fuQImage.PostedFile.ContentLength <= 0)
                        {
                            if (!isValidFile)
                            {
                                errormsg += " and File Size Must Be Greater Than Zero.";
                            }
                            else
                            {
                                errormsg += "File Size Must Be Greater Than Zero.";
                            }
                            isValidFile = false;
                            isExceedSize = false;
                        }
                        else if (fuQImage.PostedFile.ContentLength > 2097152)
                        {
                            if (!isValidFile)
                            {
                                errormsg += " and File Size Exceed The Maximum Size 2MB.";
                            }
                            else
                            {
                                errormsg += "File Size Exceed The Maximum Size 2MB.";
                            }
                            isValidFile = false;
                            isExceedSize = false;
                        }
                        else
                        {
                            isExceedSize = true;
                        }
                        break;
                    case 2:
                        switch (extension)
                        {
                            case ".doc":
                                isValidFile = true;
                                break;
                            case ".docx":
                                isValidFile = true;
                                break;
                            case ".txt":
                                isValidFile = true;
                                break;
                            case ".pdf":
                                isValidFile = true;
                                break;
                        }
                        if (!isValidFile)
                            errormsg = "It Allows .doc,.docx,.pdf and .txt Document Format Only";
                        if (fuQImage.PostedFile.ContentLength > 0)
                        {
                            if (!isValidFile)
                            {
                                errormsg += " and File Size Must Be Greater Than Zero.";
                            }
                            else
                            {
                                errormsg += "File Size Must Be Greater Than Zero.";
                            }
                            isValidFile = false;
                            isExceedSize = false;
                        }
                        else if (fuQImage.PostedFile.ContentLength > 2097152)
                        {
                            if (!isValidFile)
                            {
                                errormsg += " and File Size Exceed The Maximum Size 2MB.";
                            }
                            else
                            {
                                errormsg += "File Size Exceed The Maximum Size 2MB.";
                            }
                            isValidFile = false;
                            isExceedSize = false;
                        }
                        else
                        {
                            isExceedSize = true;
                        }
                        break;
                }
            }
            else
            {
                isValidFile = false;
                isExceedSize = false;
                errormsg = "Please Choose The File And Then Proceed";
            }
            if (!isValidFile || !isExceedSize)
            {
                errormsg = errormsg;
            }
            else
            {
                errormsg = string.Empty;
            }
            return isValidFile;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool AddNewRowsToGrid1(GridView gvGrid, string rows)
    {
        bool isSuccess = false;
        try
        {
            rows = rows.Trim();
            int newRows = 0;
            int.TryParse(rows, out newRows);
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            if (newRows != 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Sno");
                dt.Columns.Add("Options");
                dt.Columns.Add("Answer");
                dt.Columns.Add("AnswerSno");
                dt.Columns.Add("Left_Image");
                dt.Columns.Add("Right_Image");
                DataRow dr;
                int autochar = 65;
                for (int row = 0; row < newRows; row++)
                {
                    dr = dt.NewRow();
                    dr["Sno"] = Convert.ToString(row + 1);
                    //dr["Option"] = "Option" + Convert.ToString(row + 1);
                    //dr["Answer"] = "Answer" + Convert.ToString(row + 1);
                    dr["AnswerSno"] = (char)(autochar);
                    autochar++;
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    isSuccess = true;
                    gvGrid.DataSource = dt;
                    gvGrid.DataBind();
                }
            }
            return isSuccess;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            return false;
        }
    }

    public void AddNewRowToGrid1()
    {
        try
        {
            int rowIndex = Convert.ToInt32(txt_nooption.Text.ToString());
            int.TryParse(txt_nooption.Text.ToString(), out rowIndex);

            if (rowIndex > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Sno");
                dt.Columns.Add("Option");
                dt.Columns.Add("Amount");
                DataRow dr;
                for (int row = 0; row < rowIndex; row++)
                {
                    dr = dt.NewRow();
                    dr[0] = Convert.ToString(row + 1);
                    dr[1] = "";
                    dt.Rows.Add(dr);

                }
                if (dt.Rows.Count > 0)
                {
                    gridView1.DataSource = dt;
                    gridView1.DataBind();
                    optionqstn.Visible = true;
                }

            }
            else
            {
                optionqstn.Visible = false;
            }
        }
        catch (Exception ex)
        {

            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
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

    //public int rowIndxClicked()
    //{
    //    int rownumber = -1;
    //    try
    //    {
    //        Control ctrlid = GetPostBackControl(this.Page);
    //        string rno = Convert.ToString(ctrlid.UniqueID).Split('$')[1].Replace("ctl", "");
    //        int.TryParse(rno, out rownumber);
    //        rownumber -= 2;
    //    }
    //    catch { rownumber = -1; }

    //    return rownumber;

    //}

    public int rowIndxClicked()
    {
        int rownumber = -1;
        try
        {
            Control ctrlid = GetPostBackControl(this.Page);
            string rno = Convert.ToString(ctrlid.UniqueID).Split('$')[3].Replace("ctl", "");
            int.TryParse(rno, out rownumber);
            rownumber -= 2;
        }
        catch { rownumber = -1; }
        return rownumber;
    }

    /// <summary>
    /// Developed By Malang Raja T on Sep 19 2016 
    /// </summary>
    /// <param name="qry"></param>
    /// <param name="sqlpara">SqlParameter Array</param>
    /// <param name="type">Integer Value: 0 Means Text Type; 1 or Other Means Stored Procedure</param>
    /// <returns>True or False True Means Success; False Means Failed</returns>
    public bool InsertImageQuery(string qry, SqlParameter[] sqlpara, int type = 0)
    {
        connection con = new connection();
        bool isInserted = false;
        try
        {
            if (!string.IsNullOrEmpty(qry))
            {
                SqlCommand cmd = new SqlCommand();
                int result = 0;
                cmd = new SqlCommand(qry);
                if (type == 0)
                {
                    cmd.CommandType = CommandType.Text;
                }
                else
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                cmd.Connection = con.CreateConnection();
                if (sqlpara.Length > 0)
                {
                    foreach (SqlParameter item in sqlpara)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    isInserted = true;
                }
            }
            return isInserted;
        }
        catch (Exception ex)
        {
            return false;
            throw ex;
        }
        finally
        {
            con.Close();
        }
    }

    #endregion

    #region Options or Choice Grid

    protected void gvQOptions_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSno = e.Row.FindControl("lblOptionSno") as Label;
                TextBox txtOptions = e.Row.FindControl("txtQOption") as TextBox;
                if (rblQuestionType.Items.Count > 0)
                {
                    if (rblQuestionType.SelectedValue.Trim() == "4")
                    {
                        if (lblSno.Text.Trim() == "1")
                            txtOptions.Text = "True";
                        if (lblSno.Text.Trim() == "2")
                            txtOptions.Text = "False";
                        txtOptions.Enabled = false;
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

    #endregion

    #region Match The Following Grid

    protected void gvMatchQuestion_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblQMatchSno = e.Row.FindControl("lblQMatchSno") as Label;
                Label lblQuesName = e.Row.FindControl("lblMatchQuestions") as Label;
                Label lblMatchAnsSno = e.Row.FindControl("lblMatchAnsSno") as Label;


                TextBox txtQuestionsName = e.Row.FindControl("txtMatchQuestions") as TextBox;
                TextBox txtQAnswer = e.Row.FindControl("txtMatchAnswer") as TextBox;

                FileUpload fuQMatch = e.Row.FindControl("fuLhsQMatch") as FileUpload;
                FileUpload fuAnsMatch = e.Row.FindControl("fuRhsAMatch") as FileUpload;

                System.Web.UI.WebControls.Image imgQMatch = e.Row.FindControl("imgLhsQMatch") as System.Web.UI.WebControls.Image;
                System.Web.UI.WebControls.Image imgAnsMatch = e.Row.FindControl("imgRhsAMatch") as System.Web.UI.WebControls.Image;

                txtQuestionsName.Enabled = true;
                txtQAnswer.Enabled = true;

                imgQMatch.Width = 50;
                imgQMatch.Height = 50;

                imgAnsMatch.Width = 50;
                imgAnsMatch.Height = 50;

                Button btnLhsImage = e.Row.FindControl("btnLhsImage") as Button;
                Button btnRhsImage = e.Row.FindControl("btnRhsImage") as Button;

                lblQuesName.Visible = false;
                txtQuestionsName.Visible = false;
                txtQAnswer.Visible = false;

                fuQMatch.Visible = false;
                fuAnsMatch.Visible = false;

                imgQMatch.Visible = false;
                imgAnsMatch.Visible = false;

                btnLhsImage.Visible = false;
                btnRhsImage.Visible = false;

                string MatchType = string.Empty;

                if (ddlMatchSubType.Items.Count > 0)
                {
                    MatchType = Convert.ToString(ddlMatchSubType.SelectedValue).Trim();
                }
                if (rblMatchSubType.Items.Count > 0)
                {
                    MatchType = Convert.ToString(rblMatchSubType.SelectedValue).Trim();
                }
                switch (MatchType)
                {
                    case "3":
                    default:
                        txtQuestionsName.Visible = true;
                        txtQAnswer.Visible = true;
                        break;
                    case "4":
                        txtQuestionsName.Visible = true;
                        fuAnsMatch.Visible = true;
                        imgAnsMatch.Visible = true;
                        btnRhsImage.Visible = true;
                        break;
                    case "5":
                        fuQMatch.Visible = true;
                        txtQAnswer.Visible = true;
                        imgQMatch.Visible = true;
                        btnLhsImage.Visible = true;
                        break;
                    case "6":
                        fuQMatch.Visible = true;
                        fuAnsMatch.Visible = true;
                        imgQMatch.Visible = true;
                        imgAnsMatch.Visible = true;
                        btnLhsImage.Visible = true;
                        btnRhsImage.Visible = true;
                        break;
                }
                //if (!string.IsNullOrEmpty(txtQuestionsName.Text.Trim()))
                //{
                //    txtQuestionsName.Enabled = false;
                //}
                //else
                //{
                //    txtQuestionsName.Enabled = true;                    
                //}
                //if (!string.IsNullOrEmpty(txtQAnswer.Text.Trim()))
                //{
                //    txtQAnswer.Enabled = false;
                //}
                //else
                //{
                //    txtQAnswer.Enabled = true;
                //}
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void gvMatchQuestion_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            int rowindex = rowIndxClicked();
            if (gvMatchQuestion.Rows.Count > 0)
            {
                if (rowindex < gvMatchQuestion.Rows.Count)
                {
                    Label lblQMatchSno = gvMatchQuestion.Rows[rowindex].FindControl("lblQMatchSno") as Label;

                    Label lblMatchAnsSno = gvMatchQuestion.Rows[rowindex].FindControl("lblMatchAnsSno") as Label;
                    Label lblQuesName = gvMatchQuestion.Rows[rowindex].FindControl("lblMatchQuestions") as Label;
                    Label lblRhsFile = gvMatchQuestion.Rows[rowindex].FindControl("lblRhsFile") as Label;

                    TextBox txtQuestionsName = gvMatchQuestion.Rows[rowindex].FindControl("txtMatchQuestions") as TextBox;
                    TextBox txtQAnswer = gvMatchQuestion.Rows[rowindex].FindControl("txtMatchAnswer") as TextBox;

                    FileUpload fuQMatch = gvMatchQuestion.Rows[rowindex].FindControl("fuLhsQMatch") as FileUpload;
                    FileUpload fuAnsMatch = gvMatchQuestion.Rows[rowindex].FindControl("fuRhsAMatch") as FileUpload;

                    System.Web.UI.WebControls.Image imgQMatch = gvMatchQuestion.Rows[rowindex].FindControl("imgLhsQMatch") as System.Web.UI.WebControls.Image;
                    System.Web.UI.WebControls.Image imgAnsMatch = gvMatchQuestion.Rows[rowindex].FindControl("imgRhsAMatch") as System.Web.UI.WebControls.Image;

                    Button btnLhsImage = gvMatchQuestion.Rows[rowindex].FindControl("btnLhsImage") as Button;
                    Button btnRhsImage = gvMatchQuestion.Rows[rowindex].FindControl("btnRhsImage") as Button;

                    byte[] imagByte = new byte[0];
                    string imgName = string.Empty;
                    int imgLength = 0;

                    if (e.CommandName == "Lupload")
                    {
                        if (fuQMatch.HasFile)
                        {
                            string errorms = string.Empty;
                            if (CheckValidFiles(fuQMatch, out errorms))
                            {
                                imgName = Path.GetFileName(fuQMatch.PostedFile.FileName);
                                imgLength = 0;
                                //int fileSize = fuQuestionImage.PostedFile.ContentLength;
                                imgLength = fuQMatch.PostedFile.ContentLength;
                                imagByte = new byte[imgLength];
                                fuQMatch.PostedFile.InputStream.Read(imagByte, 0, imgLength);
                                //if (img_uplod.FileName.EndsWith(".xls") || FileUpload1.FileName.EndsWith(".xlsx"))
                                //rightImageType = Path.GetExtension(fuAnsMatch.PostedFile.FileName);
                                //rightImageType = rightImageType.ToLower();
                                //rightImageType = GetImageFormat(rightImageType);
                                lblQuesName.Text = (imagByte != null) ? Convert.ToBase64String(imagByte) : "";
                                if ((imagByte != null))
                                {
                                    imgQMatch.Visible = true;
                                    imgQMatch.Width = 50;
                                    imgQMatch.Height = 50;
                                    imgQMatch.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(imagByte);
                                }
                                else
                                {
                                    imgQMatch.Visible = false;
                                }
                                //Convert.FromBase64String("");
                            }
                            else
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Visible = true;
                                lblAlertMsg.Text = errorms; //"Please Choose Marks Any Images And Then Proceed";
                                return;
                            }
                        }
                    }
                    if (e.CommandName == "Rupload")
                    {
                        if (fuAnsMatch.HasFile)
                        {
                            //fuAnsMatch.PostedFile.FileName
                            string errorms = string.Empty;
                            if (CheckValidFiles(fuAnsMatch, out errorms))
                            {
                                imgName = Path.GetFileName(fuAnsMatch.PostedFile.FileName);
                                imgLength = 0;
                                //int fileSize = fuQuestionImage.PostedFile.ContentLength;
                                imgLength = fuAnsMatch.PostedFile.ContentLength;
                                imagByte = new byte[imgLength];
                                fuAnsMatch.PostedFile.InputStream.Read(imagByte, 0, imgLength);
                                //if (img_uplod.FileName.EndsWith(".xls") || FileUpload1.FileName.EndsWith(".xlsx"))
                                //rightImageType = Path.GetExtension(fuAnsMatch.PostedFile.FileName);
                                //rightImageType = rightImageType.ToLower();
                                //rightImageType = GetImageFormat(rightImageType);
                                lblRhsFile.Text = (imagByte != null) ? Convert.ToBase64String(imagByte) : "";
                                if ((imagByte != null))
                                {
                                    imgAnsMatch.Width = 50;
                                    imgAnsMatch.Height = 50;
                                    imgAnsMatch.Visible = true;
                                    imgAnsMatch.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(imagByte);
                                }
                                else
                                {
                                    imgAnsMatch.Visible = false;
                                }
                                //Convert.FromBase64String("");
                            }
                            else
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Visible = true;
                                lblAlertMsg.Text = errorms; //"Please Choose Marks Any Images And Then Proceed";
                                return;
                            }

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

    #endregion  Match The Following Grid

    #region Paragraph Grid

    protected void gvParagraph_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[i].VerticalAlign = VerticalAlign.Middle;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells.Count > 0)
                {
                    for (int cell = 0; cell < e.Row.Cells.Count; cell++)
                    {
                        e.Row.Cells[cell].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[cell].VerticalAlign = VerticalAlign.Middle;
                        TextBox txt = new TextBox();
                        txt.Width = 100;
                        txt.Attributes.Add("style", "height:auto; font-family:Book Antiqua; font-size:medium; font-weight:bold; text-align: left;");
                        txt.Attributes.Add("autocomplete", "off");
                        DropDownList ddl = new DropDownList();
                        ddl.Attributes.Add("style", "height:auto; font-family:Book Antiqua; font-size:medium; font-weight:bold; text-align: left;");
                        if (cell == 0)
                        {
                            e.Row.Cells[0].Text = Convert.ToString(e.Row.RowIndex + 1);
                        }
                        else if (cell == 1)
                        {
                            txt.ID = "txtParaQuestions" + e.Row.RowIndex;
                            //txt.Width = 250;
                            txt.Width = 150;
                            e.Row.Cells[cell].Controls.Add(txt);
                        }
                        else if (cell == 2)
                        {
                            txt.ID = "txtParaAnswers" + e.Row.RowIndex;
                            //txt.Width = 150;
                            ddl.ID = "ddlParaAnswers" + e.Row.RowIndex;
                            ddl.Visible = false;
                            e.Row.Cells[cell].Controls.Add(txt);
                            e.Row.Cells[cell].Controls.Add(ddl);
                        }
                        else
                        {
                            txt.ID = "txtParaOptions" + e.Row.RowIndex + (cell + 1);
                            //txt.Width = 150;
                            e.Row.Cells[cell].Controls.Add(txt);
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

    public bool loadParagraph(int questions, int options, DataTable dtGridPara, int type = 0)
    {
        bool isSuccess = false;
        try
        {
            Session["ParaGraphQuestions"] = null;
            dtGridPara.Columns.Clear();
            dtGridPara.Rows.Clear();
            if (questions > 0 && options > 0)
            {
                dtGridPara.Columns.Add("Sno");
                dtGridPara.Columns.Add("Questions");
                DataRow drGridPara;
                if (type == 0)
                {
                    dtGridPara.Columns.Add("Answer");
                    for (int col = 0; col < options; col++)
                    {
                        dtGridPara.Columns.Add("Option-" + (col + 1));
                    }
                }
                for (int rows = 0; rows < questions; rows++)
                {
                    drGridPara = dtGridPara.NewRow();
                    dtGridPara.Rows.Add(drGridPara);
                }
            }
            if (dtGridPara.Rows.Count > 0)
            {
                Session["ParaGraphQuestions"] = dtGridPara;
                gvParagraph.DataSource = dtGridPara;
                gvParagraph.DataBind();
                //gvParagraph.HeaderRow.Visible = false;
                isSuccess = true;
            }
            return isSuccess;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public void callGridBind()
    {
        //string uid = this.Page.Request.Params.Get("__EVENTTARGET");
        //if (uid != null && uid.Contains("gridLedgeDetails"))
        //{
        if (Session["ParaGraphQuestions"] != null)
        {
            DataTable dtGrid = (DataTable)Session["ParaGraphQuestions"];
            gvParagraph.DataSource = dtGrid;
            gvParagraph.DataBind();
            gvParagraph.HeaderRow.Visible = false;
        }
        else
        {
            gvParagraph.DataSource = null;
            gvParagraph.DataBind();
        }

        //}
    }

    #endregion Paragraph Grid

    public bool GetAllMatches(string questionPk, ref DataSet dsMatch)
    {
        bool isSuccess = false;
        try
        {
            questionPk = questionPk.Trim();
            if (!string.IsNullOrEmpty(questionPk.Trim()))
            {
                qry = "select choiceNo as Sno,CHAR(64 + choiceNo) as AnswerSno,QChoice as Options,QChoiceImage Left_Image,QMatchR as Answer,QChoiceImageR as Right_Image from QuestionsChoice where QuestionID='" + questionPk.Trim() + "' and isMatching=1 order by choiceNo";
                dsMatch = new DataSet();
                dsMatch = d2.select_method_wo_parameter(qry, "Text");
                if (dsMatch.Tables.Count > 0 && dsMatch.Tables[0].Rows.Count > 0)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public void GetMatches(DataTable dtChoice, string questionID, ref DataTable dtOptions, string questionsOptions)
    {
        try
        {
            dtOptions.Columns.Clear();
            dtOptions.Rows.Clear();
            dtOptions.Columns.Add("OptionNo", typeof(string));
            dtOptions.Columns.Add("Option", typeof(string));
            bool haschoice = false;
            int autochar = 96;
            DataRow drMatch;
            if (dtChoice.Rows.Count > 0)
            {
                dtChoice.DefaultView.RowFilter = "QuestionID='" + questionID.Trim() + "' ";
                DataView dvChoice = new DataView();
                dvChoice = dtChoice.DefaultView;
                if (dvChoice.Count > 0)
                {
                    haschoice = true;
                    for (int newmatch = 0; newmatch < dvChoice.Count; newmatch++)
                    {
                        drMatch = dtOptions.NewRow();
                        string choiceid = Convert.ToString(dvChoice[newmatch]["choiceID"]);
                        string lsno = Convert.ToString(dvChoice[newmatch]["choiceNo"]);
                        string lhs = Convert.ToString(dvChoice[newmatch]["LHS"]);
                        string lImage = Convert.ToString(dvChoice[newmatch]["LHS_Image"]);
                        string rsno = Convert.ToString(dvChoice[newmatch]["AnswerSno"]);
                        string rhs = Convert.ToString(dvChoice[newmatch]["RHS"]);
                        string RImage = Convert.ToString(dvChoice[newmatch]["RHS_Image"]);

                        int choice = 0;
                        int.TryParse(lsno, out choice);
                        drMatch["OptionNo"] = Convert.ToString((char)(choice + autochar));
                        drMatch["Option"] = lhs;
                        dtOptions.Rows.Add(drMatch);
                    }
                }
            }
            if (!haschoice)
            {
                if (!string.IsNullOrEmpty(questionsOptions.Trim()))
                {
                    if (questionsOptions.Contains(';') || questionsOptions.Contains("#malang#"))
                    {
                        string[] split1 = (!questionsOptions.Contains("#malang#")) ? (questionsOptions.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)) : (questionsOptions.Split(new string[] { "#malang#" }, StringSplitOptions.RemoveEmptyEntries));
                        for (int row = 0; row < split1.Length; row++)
                        {
                            haschoice = true;
                            drMatch = dtOptions.NewRow();
                            string opt = Convert.ToString(split1[row]).Trim();
                            if (opt != "")
                            {
                                drMatch["OptionNo"] = Convert.ToString((char)((row + 1) + autochar));
                                drMatch["Option"] = opt;
                            }
                            dtOptions.Rows.Add(drMatch);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }


    //Added by saranyadevi 12.11.2018
    protected void chklst_general_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (chklst_general.Checked)
            {
                lblsubject.Visible = false;
                ddlsubject.Visible = false;
                FpSpread1.Visible = false;
            }
            else
            {
                lblsubject.Visible = true;
                ddlsubject.Visible = true;

            }

        }
        catch
        {


        }


    }
}