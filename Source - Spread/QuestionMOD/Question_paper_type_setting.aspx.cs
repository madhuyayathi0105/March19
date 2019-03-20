using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;
using System.Configuration;

public partial class Question_paper_type_setting : System.Web.UI.Page
{
    #region Fields Declaration

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    Hashtable hat = new Hashtable();
    Hashtable availablequestion = new Hashtable();

    bool cellclick = false;
    bool isSchool = false;

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

    #region Page Load

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
            bindcollege();
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
            ChangeHeaderName(isSchool);

        }
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

    protected void btn_errorclose1_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
    }

    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBatch();
        BindDegree();
        bindbranch();
        bindsem();
        BindSectionDetail();
        GetSubject();

        objective_check.Visible = false;
    }

    protected void ddlsubject_Selectchanged(object sender, EventArgs e)
    {
        // testname();
        objective_check.Visible = false;

    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDegree();
        bindbranch();
        bindsem();
        BindSectionDetail();
        GetSubject();

        objective_check.Visible = false;
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        BindSectionDetail();
        GetSubject();
        objective_check.Visible = false;
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
        BindSectionDetail();
        GetSubject();
        objective_check.Visible = false;
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSectionDetail();
        GetSubject();
        objective_check.Visible = false;
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSubject();
        objective_check.Visible = false;
    }

    public void btn_gendrate_Click(object sender, EventArgs e)
    {
        //bindreport();
        generatePDF();
    }

    public void btn_go_Click(object sender, EventArgs e)
    {
        objective_check.Visible = false;
        format2();
        // format1();
        // sec_grid();
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
            string rno = Convert.ToString(ctrlid.UniqueID).Split('$')[1].Replace("ctl", "");
            int.TryParse(rno, out rownumber);
            rownumber -= 2;
        }
        catch { rownumber = -1; }

        return rownumber;
    }

    public void format2()
    {
        try
        {
            FpSpread2.Visible = false;
            chk_answer.Visible = false;
            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 5;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.SaveChanges();
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Exam Date";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Internal/ External";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Test Name/ Month and Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Questions";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;


            string batch = Convert.ToString(ddlbatch.SelectedItem.Text);
            string degreecod = Convert.ToString(ddlbranch.SelectedItem.Value);
            string sem = Convert.ToString(ddlsem.SelectedItem.Text);
            string subject_cd="";
            //string testcod=Convert.ToString(ddl_testname.SelectedItem.Value);
            if(ddlsubject.Items.Count>0)
             subject_cd = Convert.ToString(ddlsubject.SelectedItem.Value);

            string section = "";
            string strsec = "";
            if (ddlsec.Items.Count > 0)
            {
                section = Convert.ToString(ddlsec.SelectedItem.Text);

                if (ddlsec.Text.ToString().ToLower() == "all" || ddlsec.Text.ToString() == "")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and qb.Sections='" + section.ToString() + "'";
                }
            }
            ds.Clear();

            //string getquery = " select count (eq.QuestionMasterFK)as question,CONVERT(varchar(10), qb.exam_date,103) as exam_date, c.criteria,c.Criteria_no from tbl_question_bank_master qb, tbl_question_master qm,CriteriaForInternal c,Exist_questions eq where  qb.exam_type='2' and  qm.Subject_no=qb.Subject_no and eq.subject_no=qb.Subject_no and eq.Test_code=c.Criteria_no and eq.Test_code=qb.exam and eq.subject_no=qm.subject_no and qm.QuestionMasterPK=eq.QuestionMasterFK and   qm.Subject_no='" + subject_cd + "'  and qb.Batch_year='" + batch + "'and qb.Degree_Code='" + degreecod + "' and   qb.Semester='" + sem + "' " + strsec + " and c.Criteria_no=qb.exam  group by  Exam,qb.exam_date,c.criteria,c.Criteria_no order by qb.exam_date";

            string getquery = "  select eq.is_internal, count (eq.QuestionMasterFK)as question,CONVERT(varchar(10), qb.exam_date,103) as exam_date, c.criteria,c.Criteria_no from tbl_question_bank_master qb, tbl_question_master qm,CriteriaForInternal c,Exist_questions eq where  qb.exam_type='2' and  qm.Subject_no=qb.Subject_no and eq.subject_no=qb.Subject_no and eq.Test_code=c.Criteria_no and eq.Test_code=qb.exam and eq.subject_no=qm.subject_no and qm.QuestionMasterPK=eq.QuestionMasterFK and   qm.Subject_no='" + subject_cd + "'  and qb.Batch_year='" + batch + "' and qb.Degree_Code='" + degreecod + "' and   qb.Semester='" + sem + "' " + strsec + " and c.Criteria_no=qb.exam  group by  eq.is_internal,Exam,qb.exam_date,c.criteria,c.Criteria_no  union select eq.is_internal,count (eq.QuestionMasterFK)as question,CONVERT(varchar(10), qb.exam_date,103) as exam_date,qb.exam_month criteria,qb.exam_year Criteria_no from tbl_question_bank_master qb, tbl_question_master qm,Exist_questions eq where qm.Subject_no=qb.Subject_no and eq.subject_no=qb.Subject_no and eq.subject_no=qm.subject_no and qm.QuestionMasterPK=eq.QuestionMasterFK and qb.exam_type='1'  and eq.is_internal=1 and qb.exam_month=eq.Exam_month and eq.Exam_year=qb.exam_year and  qm.Subject_no='" + subject_cd + "' and qb.Batch_year='" + batch + "'   and qb.Degree_Code='" + degreecod + "' and   qb.Semester='" + sem + "'    " + strsec + "  group by  eq.is_internal,Exam,qb.exam_date,qb.exam_month,qb.exam_year order by exam_date";

            ds = d2.select_method_wo_parameter(getquery, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int he = 100;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);

                        FpSpread1.Sheets[0].Cells[i, 1].Text = "";
                        FpSpread1.Sheets[0].Cells[i, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["exam_date"]);

                        string exmtyp = Convert.ToString(ds.Tables[0].Rows[i]["is_internal"]).Trim();
                        string isinternal = "";
                        if (exmtyp == "2")
                        {
                            isinternal = "Internal";
                            FpSpread1.Sheets[0].Cells[i, 3].Text = ds.Tables[0].Rows[i]["criteria"].ToString();
                            FpSpread1.Sheets[0].Cells[i, 3].Tag = ds.Tables[0].Rows[i]["Criteria_no"].ToString();
                        }
                        else if (exmtyp == "1")
                        {
                            isinternal = "External";
                            int month = 0;
                            string name = Convert.ToString(ds.Tables[0].Rows[i]["criteria"]).Trim();
                            int.TryParse(name.Trim(), out month);
                            if (month != 0)
                            {
                                name = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                            }
                            FpSpread1.Sheets[0].Cells[i, 3].Text = name + "-" + Convert.ToString(ds.Tables[0].Rows[i]["Criteria_no"]);
                            FpSpread1.Sheets[0].Cells[i, 3].Note = Convert.ToString(ds.Tables[0].Rows[i]["criteria"]);
                            FpSpread1.Sheets[0].Cells[i, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Criteria_no"]);
                        }

                        FpSpread1.Sheets[0].Cells[i, 2].Text = isinternal;
                        FpSpread1.Sheets[0].Cells[i, 2].Tag = exmtyp;

                        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;

                        FpSpread1.Sheets[0].Cells[i, 4].Text = ds.Tables[0].Rows[i]["question"].ToString();
                        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

                        he = he + 25;

                    }

                    FpSpread1.Columns[0].Locked = true;
                    FpSpread1.Columns[1].Locked = true;
                    FpSpread1.Columns[4].Locked = true;
                    FpSpread1.Columns[3].Locked = true;
                    FpSpread1.Columns[2].Width = 85;
                    FpSpread1.Columns[3].Width = 155;

                    FpSpread1.Height = he;
                    FpSpread1.SaveChanges();
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        FpSpread1.Visible = true;
                        FpSpread2.Visible = false;
                        chk_answer.Visible = false;
                        btn_gendrate.Visible = false;
                        objective_check.Visible = true;
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    FpSpread1.Visible = false;
                    FpSpread2.Visible = false;
                    chk_answer.Visible = false;
                    objective_check.Visible = false;
                }
            }
            else
            {
                objective_check.Visible = false;
                FpSpread2.Visible = false;
                chk_answer.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                FpSpread1.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

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
                FpSpread2.Visible = false;
                chk_answer.Visible = false;
                chk_answer.Checked = false;
                int activerow = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
                int activecol = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
                string valu = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag.ToString();
                string month_name = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text.ToString();
                string isinternal = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();
                if (isinternal == "1")
                {
                    format1(valu);
                }
                else if (isinternal == "2")
                {
                    format1(valu);
                }
            }
        }
        catch
        {
        }

    }

    public void format1(string criteriacode)
    {
        try
        {
            Hashtable addpkhas = new Hashtable();
            FpSpread2.Visible = false;
            chk_answer.Visible = false;
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
            FarPoint.Web.Spread.CheckBoxCellType chk1 = new FarPoint.Web.Spread.CheckBoxCellType();
            chk1.AutoPostBack = true;
            FpSpread2.Width = 850;
            FpSpread2.Height = 500;
            FpSpread2.Sheets[0].Columns[0].Width = 25;
            FpSpread2.Sheets[0].Columns[1].Width = 25;
            FpSpread2.Sheets[0].Columns[2].Width = 30;
            FpSpread2.Sheets[0].Columns[3].Width = 35;
            FpSpread2.Sheets[0].Columns[4].Width = 150;

            FpSpread2.SaveChanges();
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Section";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Mark";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Type";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Questions";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Option";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Answer";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;

            if (chk_answer.Checked == true)
            {
                FpSpread2.Columns[6].Visible = true;
            }
            else
            {
                FpSpread2.Columns[6].Visible = false;
            }

            int activerow = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
            int activecol = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
            string valu = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();
            string month_name = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Note.ToString();
            string year = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag.ToString();


            string batch = Convert.ToString(ddlbatch.SelectedItem.Text);
            string degreecod = Convert.ToString(ddlbranch.SelectedItem.Value);
            string sem = Convert.ToString(ddlsem.SelectedItem.Text);
            //string testcod=Convert.ToString(ddl_testname.SelectedItem.Value);
            string subject_cd = Convert.ToString(ddlsubject.SelectedItem.Value);

            string section = "";
            string strsec = "";
            if (ddlsec.Items.Count > 0)
            {
                section = Convert.ToString(ddlsec.SelectedItem.Text);

                if (ddlsec.Text.ToString().ToLower().Trim() == "all" || ddlsec.Text.ToString().Trim() == "")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and qb.Sections='" + section.ToString() + "' ";
                }
            }

            string add_query = "";
            if (valu == "1")
            {
                add_query = " and qb.exam_month=eq.Exam_month and eq.Exam_year=qb.exam_year and qb.Exam_month='" + month_name + "' and qb.Exam_year='" + year + "' and eq.is_internal='1'";
            }
            else if (valu == "2")
            {

                add_query = " and eq.Test_code=qb.Exam  and eq.Test_code='" + criteriacode + "' and eq.is_internal='2' ";
            }
            string subno = "";

            if (ddlsubject.Items.Count > 0)
            {
                subno = Convert.ToString(ddlsubject.SelectedItem.Value);

            }
            string Existqtns = "";

            ds.Clear();
            //string sqry = " select  eq.QNo,tq.QuestionMasterPK,question,mark,options,answer,tq.syllabus,is_descriptive,tq.subject_no, type ,eq.Section from tbl_question_master tq, Exist_questions eq  where tq.QuestionMasterPK=eq.QuestionMasterFK and  tq.syllabus=eq.syllabus and eq.subject_no=tq.subject_no and  tq. subject_no ='" + subno + "' " + add_query + " order by Section,eq.QNo";
            //string sqry = " select  tq.subject_no,tq.syllabus,eq.Section,eq.QNo,is_descriptive,type,tq.QuestionType,tq.QuestionSubType,tq.totalChoice,mark,tq.QuestionMasterPK,question,options,answer,tq.file_name,tq.file_type,tq.quetion_image,tq.is_matching,tq.qmatching from tbl_question_master tq, Exist_questions eq  where tq.QuestionMasterPK=eq.QuestionMasterFK and  tq.syllabus=eq.syllabus and eq.subject_no=tq.subject_no and  tq. subject_no ='" + subno + "' " + add_query + " order by Section,eq.QNo";

            string sqry = " select tq.subject_no,tq.syllabus,eq.Section,eq.QNo,is_descriptive,type,tq.QuestionType,tq.QuestionSubType,tq.totalChoice,mark,tq.QuestionMasterPK,question,options,answer,tq.file_name,tq.file_type,tq.quetion_image,tq.is_matching,tq.qmatching from tbl_question_bank_master qb, tbl_question_master tq,Exist_questions eq where tq.Subject_no=qb.Subject_no and eq.subject_no=qb.Subject_no and eq.subject_no=tq.subject_no and tq.QuestionMasterPK=eq.QuestionMasterFK and  tq. subject_no ='" + subno + "' and qb.Batch_year='" + batch + "' and qb.Degree_Code='" + degreecod + "' and qb.Semester='" + sem + "' " + strsec + add_query + " order by Section,eq.QNo";

            ds = d2.select_method_wo_parameter(sqry, "Text");

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread2.Sheets[0].RowCount);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["QuestionMasterPK"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;


                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Section"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag = criteriacode;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;

                        string desc = Convert.ToString(ds.Tables[0].Rows[i]["is_descriptive"]);

                        string subjectNo = Convert.ToString(ds.Tables[0].Rows[i]["subject_no"]).Trim();
                        string unitNo = Convert.ToString(ds.Tables[0].Rows[i]["syllabus"]).Trim();

                        string ObjectiveDescript = Convert.ToString(ds.Tables[0].Rows[i]["is_descriptive"]).Trim();
                        string questionGradeNo = Convert.ToString(ds.Tables[0].Rows[i]["type"]).Trim();

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
                        string questionType = Convert.ToString(ds.Tables[0].Rows[i]["QuestionType"]).Trim();
                        //string QuestionObjType = Convert.ToString(ds.Tables[0].Rows[i]["QuestionMainType"]).Trim();
                        string questionSubType = Convert.ToString(ds.Tables[0].Rows[i]["QuestionSubType"]).Trim();
                        //string questionSubType = Convert.ToString(ds.Tables[0].Rows[i]["Question_SubType"]).Trim();

                        if (desc == "0")
                        {
                            desc = "Objective";

                            switch (questionType)
                            {
                                case "1":
                                    break;
                                case "2":
                                    break;
                                case "3":
                                    break;
                                case "4":
                                    break;
                                case "5":
                                    break;
                                case "6":
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (desc == "1")
                        {
                            desc = "Descriptive";
                        }

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["mark"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Locked = true;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = desc;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Locked = true;


                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["question"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Locked = true;

                        string optnvalu = "";

                        string option = Convert.ToString(ds.Tables[0].Rows[i]["options"]);
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
                        string qOptions = questionOptions.Trim();
                        if (!string.IsNullOrEmpty(qOptions.Trim()))
                        {
                            switch (questionType.Trim())
                            {
                                case "1":
                                case "2":
                                case "3":
                                case "4":
                                case "5":
                                default:
                                    if (qOptions.Contains(';') || qOptions.Contains("#malang#"))
                                    {
                                        string[] split1;
                                        split1 = (!qOptions.Contains("#malang#")) ? (qOptions.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)) : (qOptions.Split(new string[] { "#malang#" }, StringSplitOptions.RemoveEmptyEntries));
                                        for (int row = 0; row < split1.Length; row++)
                                        {
                                            optnvalu = optnvalu + " " + Convert.ToString(row + 1) + "." + split1[row] + ", ";
                                        }
                                    }
                                    break;
                                case "6":
                                    string[] qParaOptions = qOptions.Split(new string[] { "#Qpara#" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (qParaOptions.Length > 0)
                                    {
                                        for (int para = 0; para < qParaOptions.Length; para++)
                                        {
                                            string[] qparaopt = qParaOptions[para].Split(new string[] { "#Qparaopt#" }, StringSplitOptions.RemoveEmptyEntries);
                                            //optnvalu = optnvalu + " " + Convert.ToString(para + 1) + ")";
                                            if (qparaopt.Length > 0)
                                            {
                                                string colvalue = " " + Convert.ToString(para + 1) + ")";
                                                for (int qqp = 0; qqp < qparaopt.Length; qqp++)
                                                {
                                                    colvalue += " " + Convert.ToString(qqp + 1) + "." + qparaopt[qqp] + ", ";
                                                }
                                                optnvalu += colvalue;
                                            }
                                        }
                                    }
                                    break;
                            }
                        }


                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = optnvalu;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Locked = true;



                        string newQuestionAnswer = questionAnswer;

                        if (questionType.Trim() == "1")
                        {
                            if (questionType.Trim() == "2")
                            {
                                string[] newAns = (questionAnswer.Split(new string[] { "#ans#" }, StringSplitOptions.RemoveEmptyEntries));
                                if (newAns.Length > 0)
                                {
                                    //newQuestionAnswer = string.Join(",", newAns);
                                    newQuestionAnswer = string.Empty;
                                    for (int newans1 = 0; newans1 < newAns.Length; newans1++)
                                    {
                                        if (!string.IsNullOrEmpty(newQuestionAnswer.Trim()))
                                        {
                                            newQuestionAnswer += "," + Convert.ToString(newans1 + 1) + "." + newAns[newans1].Trim();
                                        }
                                        else
                                        {
                                            newQuestionAnswer = Convert.ToString(newans1 + 1) + "." + newAns[newans1].Trim();
                                        }
                                    }
                                }
                                else
                                {
                                    newQuestionAnswer = questionAnswer;
                                }
                                //((questionAnswer.Contains("#ans#")) ? (questionAnswer.Split(new string[] { "#ans#" }, StringSplitOptions.RemoveEmptyEntries)) : questionAnswer.Trim());
                            }
                        }
                        else if (questionType.Trim() == "6")
                        {
                            string[] newAns = (questionAnswer.Split(new string[] { "#Qpara#" }, StringSplitOptions.RemoveEmptyEntries));
                            if (newAns.Length > 0)
                            {
                                //newQuestionAnswer = string.Join(",", newAns);

                                newQuestionAnswer = string.Empty;
                                for (int newans1 = 0; newans1 < newAns.Length; newans1++)
                                {
                                    if (!string.IsNullOrEmpty(newQuestionAnswer.Trim()))
                                    {
                                        newQuestionAnswer += "," + Convert.ToString(newans1 + 1) + "." + newAns[newans1].Trim();
                                    }
                                    else
                                    {
                                        newQuestionAnswer = Convert.ToString(newans1 + 1) + "." + newAns[newans1].Trim();
                                    }
                                }
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

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = newQuestionAnswer; //Convert.ToString(ds.Tables[0].Rows[i]["answer"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Locked = true;
                    }
                    FpSpread2.Visible = true;
                    chk_answer.Visible = true;
                    btn_gendrate.Visible = true;
                }
            }
            else
            {
                FpSpread2.Visible = false;
                chk_answer.Visible = false;
            }
            if (FpSpread2.Sheets[0].RowCount > 0)
            {
                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                FpSpread2.Visible = true;
                objective_check.Visible = true;
                btn_gendrate.Visible = true;
                chk_answer.Visible = true;
            }
            else
            {
                //objective_check.Visible = false;
                FpSpread2.Visible = false;
                chk_answer.Visible = false;
                btn_gendrate.Visible = false;
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
                FpSpread2.Columns[6].Visible = true;
            }
            else
            {
                FpSpread2.Columns[6].Visible = false;
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
            imgdiv2.Visible = true;
        }
    }

    protected void FpSpread2_ButtonCommand(object sender, EventArgs e)
    {

    }

    public void bindreport()
    {
        try
        {
            string qry1 = string.Empty;
            string batch = ddlbatch.SelectedItem.Value;
            string degreecod = ddlbranch.SelectedItem.Value;
            string sem = ddlsem.SelectedItem.Text;
            string section = string.Empty;

            string qrysec = string.Empty;

            if (ddlsec.Items.Count > 0)
            {
                if (ddlsec.SelectedItem.Text.Trim().ToLower() != "all" && string.IsNullOrEmpty(ddlsec.SelectedItem.Text.Trim().ToLower()))
                {
                    section = ddlsec.SelectedItem.Text;
                    qrysec = "  and  Sections='" + section + "'";
                }
            }

            string pdfname = string.Empty;
            // string testvalu = ddl_testname.SelectedItem.Value;
            string subno = ddlsubject.SelectedItem.Value;
            int totalmark = 0;
            int sno = 0;
            int activerow = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
            int activecol = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
            string valu = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();
            string month_name = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Note.ToString();
            string criteriacode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);

            string add_query = "";
            if (valu == "1")
            {
                add_query = " and Exam_month='" + month_name + "' and Exam_year='" + criteriacode + "' and is_internal='1'";
            }
            else if (valu == "2")
            {

                add_query = " and Test_code='" + criteriacode + "' and is_internal='2' ";
            }

            string qry = " select COUNT(Exist_questionPK) Total_Quesions,Section,mark,Must_attend,eq.section_name  from Exist_questions eq,tbl_question_master tqm where tqm.syllabus=eq.syllabus and eq.QuestionMasterFK=tqm.QuestionMasterPK and eq.subject_no='" + subno + "' " + add_query + "  group by Section,Mark,Must_attend,eq.section_name ";

            ds1 = d2.select_method_wo_parameter(qry, "Text");
            if (ds1.Tables.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int rows = 0; rows < ds1.Tables[0].Rows.Count; rows++)
                    {
                        string must_questn = Convert.ToString(ds1.Tables[0].Rows[rows]["Must_attend"]);
                        string mark = Convert.ToString(ds1.Tables[0].Rows[rows]["mark"]);

                        if (must_questn != "" && mark != "")
                        {
                            int mulmark = Convert.ToInt32(must_questn) * Convert.ToInt32(mark);
                            totalmark = totalmark + mulmark;
                        }

                    }
                }
            }
            string total_mark = "Total Mark: " + totalmark;
            DataSet printds = new DataSet();
            string sql3 = "";
            string sql4 = "";
            DataSet printds_new = new DataSet();
            DataSet printds_rows = new DataSet();
            DataSet gradeds = new DataSet();
            Font colleg = new Font("Book Antiqua", 18, FontStyle.Bold);
            Font Fontarial1 = new Font("Book Antiqua", 16, FontStyle.Bold);
            Font Fontarial2 = new Font("Book Antiqua", 14, FontStyle.Bold);
            Font Fontarial3 = new Font("Book Antiqua", 11, FontStyle.Regular);
            Font Fontarial4 = new Font("Book Antiqua", 12, FontStyle.Regular);


            Gios.Pdf.PdfDocument page_question = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;
            mypdfpage = page_question.NewPage();

            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
            {
                PdfImage LogoImage = page_question.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));

                mypdfpage.Add(LogoImage, 25, 15, 400);
            }
            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
            {
                PdfImage LogoImage = page_question.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));

                mypdfpage.Add(LogoImage, 500, 15, 400);
            }

            string get_time = "";
            int hight = 20;

            string collegename = ddl_collegename.SelectedItem.Text;
            PdfTextArea pdftext_clg = new PdfTextArea(colleg, System.Drawing.Color.Black, new PdfArea(page_question, 150, hight, 300, 50), System.Drawing.ContentAlignment.MiddleCenter, collegename);

            mypdfpage.Add(pdftext_clg);
            hight = hight + 40;
            if (valu == "1")
            {
                string strexam = month_name + " - " + criteriacode + "";
                PdfTextArea pdftextmark = new PdfTextArea(Fontarial1, System.Drawing.Color.Black, new PdfArea(page_question, 150, hight, 300, 50), System.Drawing.ContentAlignment.MiddleCenter, strexam);
                mypdfpage.Add(pdftextmark);

                get_time = d2.GetFunction("select  Duration  from tbl_question_bank_master where  Batch_year='" + batch + "'and Degree_Code='" + degreecod + "' and  Semester='" + sem + "' " + qrysec + " and exam_year='" + criteriacode + "' and exam_month='" + month_name + "' ");

            }
            else if (valu == "2")
            {

                string strexam = d2.GetFunction("select c.criteria,c.Criteria_no from CriteriaForInternal c, syllabus_master sy where c.Criteria_no ='" + criteriacode + "' ");
                get_time = d2.GetFunction("select  Duration  from tbl_question_bank_master where  Batch_year='" + batch + "'and Degree_Code='" + degreecod + "' and  Semester='" + sem + "' " + qrysec + " and Exam= '" + criteriacode.Trim() + "' ");

                PdfTextArea pdftextmark = new PdfTextArea(Fontarial1, System.Drawing.Color.Black, new PdfArea(page_question, 150, hight, 300, 50), System.Drawing.ContentAlignment.MiddleCenter, strexam);
                mypdfpage.Add(pdftextmark);
                pdfname = ddlsubject.SelectedItem.Text + "_" + strexam;

                // CONVERT(varchar(10), exam_date,103) as exam_date

            }


            hight += 40;

            PdfTextArea tot_mark = new PdfTextArea(Fontarial2, System.Drawing.Color.Black, new PdfArea(page_question, 38, hight, 500, 50), System.Drawing.ContentAlignment.MiddleLeft, total_mark);
            mypdfpage.Add(tot_mark);


            if (get_time != "0" && get_time.Trim() != "")
            {
                get_time = "Time :" + get_time + "   Minute ";
            }

            PdfTextArea pdftext = new PdfTextArea(Fontarial2, System.Drawing.Color.Black, new PdfArea(page_question, 38, hight, 500, 50), System.Drawing.ContentAlignment.MiddleRight, get_time);
            mypdfpage.Add(pdftext);
            DataView dv = new DataView();



            string sqry = " select tq.subject_no,tq.syllabus,eq.Section,eq.QNo,eq.Exist_questionPK,tq.QuestionMasterPK,question,mark,is_descriptive,type,options,answer,Must_attend,tq.QuestionType,tq.QuestionSubType,tq.totalChoice,isnull(tq.is_matching,0) as is_matching,tq.qmatching,tq.quetion_image from tbl_question_master tq, Exist_questions eq  where tq.QuestionMasterPK=eq.QuestionMasterFK and  tq.syllabus=eq.syllabus and eq.subject_no=tq.subject_no and  eq.subject_no ='" + subno + "' " + add_query + "  order by eq.QNo,eq.Exist_questionPK,eq.Section";
            ds = d2.select_method_wo_parameter(sqry, "Text");

            qry1 = "select qc.choiceID,qc.QuestionID,qc.choiceNo,qc.QChoice as LHS,qc.QChoiceImage as LHS_Image,CHAR(64 + choiceNo) as AnswerSno,qc.QMatchR as RHS,qc.QChoiceImageR as RHS_Image,isAnswer,isMatching from tbl_question_master tq,QuestionsChoice qc,Exist_questions eq where eq.QuestionMasterFK=qc.QuestionID and tq.QuestionMasterPK=qc.QuestionID and tq.QuestionMasterPK=eq.QuestionMasterFK and tq.syllabus=eq.syllabus and eq.subject_no=tq.subject_no and  eq.subject_no ='" + subno + "' " + add_query + "  order by qc.QuestionID";
            DataSet dsChoice = new DataSet();
            dsChoice = d2.select_method_wo_parameter(qry1, "Text");
            //, NEWID()
            for (int rs = 0; rs < ds1.Tables[0].Rows.Count; rs++)
            {
                int check_h = hight / 750;
                if (check_h == 1)
                {
                    mypdfpage.SaveToDocument();
                    mypdfpage = page_question.NewPage();
                    hight = 10;
                }

                hight = hight + 40;
                string questn = Convert.ToString(ds1.Tables[0].Rows[rs]["Must_attend"]);
                string outqstn = Convert.ToString(ds1.Tables[0].Rows[rs]["Total_Quesions"]);
                string mark = Convert.ToString(ds1.Tables[0].Rows[rs]["mark"]);
                string p_sections = Convert.ToString(ds1.Tables[0].Rows[rs]["Section"]);
                string section_name = Convert.ToString(ds1.Tables[0].Rows[rs]["section_name"]);

                string addpart = "Part - " + p_sections + " -  " + section_name + "  ( " + questn + " Out of " + outqstn + " ) ";
                PdfTextArea part = new PdfTextArea(Fontarial2, System.Drawing.Color.Black, new PdfArea(page_question, 15, hight, 500, 50), System.Drawing.ContentAlignment.MiddleLeft, addpart);
                mypdfpage.Add(part);
                string part2 = questn + " X " + mark + "  = " + Convert.ToString(Convert.ToInt32(questn) * Convert.ToInt32(mark));

                PdfTextArea part2last = new PdfTextArea(Fontarial2, System.Drawing.Color.Black, new PdfArea(page_question, 25, hight, 550, 50), System.Drawing.ContentAlignment.MiddleRight, part2);
                mypdfpage.Add(part2last);
                PdfImage Questionimg;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].DefaultView.RowFilter = "Section='" + p_sections + "' and mark='" + mark + "'";
                        dv = ds.Tables[0].DefaultView;

                        if (dv.Count > 0)
                        {
                            for (int i = 0; i < dv.Count; i++)
                            {
                                //int srilanohight = hight;
                                string questionNo = Convert.ToString(dv[i]["QNo"]);
                                string questionPk = Convert.ToString(dv[i]["QuestionMasterPK"]);
                                string questionName = Convert.ToString(dv[i]["question"]);
                                string queswtionOptions = Convert.ToString(dv[i]["options"]);
                                string questionAnswer = Convert.ToString(dv[i]["answer"]);
                                string questionMark = Convert.ToString(dv[i]["mark"]);

                                string questionMatchingName = Convert.ToString(dv[i]["qmatching"]);
                                string questionMatchingorNot = Convert.ToString(dv[i]["is_matching"]);

                                string questionObjDesc = Convert.ToString(dv[i]["is_descriptive"]);
                                string questionGrade = Convert.ToString(dv[i]["type"]);
                                string questionType = Convert.ToString(dv[i]["QuestionType"]);
                                string questionSubType = Convert.ToString(dv[i]["QuestionSubType"]);
                                string totalChoices = Convert.ToString(dv[i]["totalChoice"]);

                                string questionImage = Convert.ToString(dv[i]["quetion_image"]);

                                hight = hight + 20;
                                int check_hieght = hight / 800;
                                if (check_hieght == 1)
                                {
                                    mypdfpage.SaveToDocument();
                                    mypdfpage = page_question.NewPage();
                                    hight = 30;
                                }
                                string tx_sno = Convert.ToString(sno = sno + 1) + ". ";

                                string question_img = Convert.ToString(dv[i]["quetion_image"]);
                                string question_pk = Convert.ToString(dv[i]["QuestionMasterPK"]);

                                MemoryStream memoryStream = new MemoryStream();
                                // DataSet dsstdpho = new DataSet();
                                int imageHight = 0;
                                int imagewidth = 0;
                                if (question_img.ToString().Trim() != "")
                                {
                                    byte[] file = (byte[])dv[i]["quetion_image"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + question_pk + ".jpeg")))
                                        {
                                            //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                        }
                                        else
                                        {
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + question_pk + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                        }
                                    }
                                    imageHight = 0;
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + question_pk + ".jpeg")))
                                    {
                                        hight += 30;

                                        Questionimg = page_question.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/" + question_pk + ".jpeg"));
                                        imagewidth = Questionimg.Width;
                                        mypdfpage.Add(Questionimg, (page_question.PageWidth / 2) - imagewidth / 3, hight, 250);
                                        imageHight = Questionimg.Height;
                                    }
                                    else
                                    {
                                        //Questionimg = page_question.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/" + question_pk + ".jpeg"));
                                        //mypdfpage.Add(Questionimg, 25, hight, 300);
                                    }
                                }

                                //**
                                string question = Convert.ToString(dv[i]["question"]);
                                int qustnwidth = question.Length;
                                if (qustnwidth > 50)
                                {
                                    qustnwidth = qustnwidth / 50;
                                    int hightanswer = 10 * qustnwidth;
                                    hight = hight + hightanswer;
                                }

                                check_hieght = hight / 800;
                                if (check_hieght == 1)
                                {
                                    mypdfpage.SaveToDocument();
                                    mypdfpage = page_question.NewPage();
                                    hight = 30;
                                }

                                if (question != "")
                                {
                                    PdfTextArea t_sno = new PdfTextArea(Fontarial4, System.Drawing.Color.Black, new PdfArea(page_question, 15, hight, 30, 50), System.Drawing.ContentAlignment.MiddleLeft, tx_sno);
                                    mypdfpage.Add(t_sno);
                                    if (question_img != "")
                                    {
                                        hight += imageHight / 4;
                                        PdfTextArea questionbind = new PdfTextArea(Fontarial4, System.Drawing.Color.Black, new PdfArea(page_question, 150, hight, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, question);
                                        mypdfpage.Add(questionbind);
                                    }
                                    else
                                    {
                                        PdfTextArea questionbind = new PdfTextArea(Fontarial4, System.Drawing.Color.Black, new PdfArea(page_question, 35, hight, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, question);
                                        mypdfpage.Add(questionbind);
                                    }
                                }
                                string matchs = Convert.ToString(dv[i]["qmatching"]);
                                Gios.Pdf.PdfTable match_following;
                                Gios.Pdf.PdfTablePage addtabletopdf;
                                if (questionObjDesc.Trim() == "0")
                                {
                                    switch (questionType)
                                    {
                                        case "1":
                                        case "2":
                                        case "4":
                                            break;
                                        case "3":
                                            switch (questionSubType.Trim())
                                            {
                                                case "1":
                                                case "2":
                                                    break;
                                                case "3":
                                                case "4":
                                                case "5":
                                                case "6":
                                                    PdfImage questionMatchImage;
                                                    PdfImage questionMatchImageR;
                                                    if (dsChoice.Tables.Count > 0 && dsChoice.Tables[0].Rows.Count > 0)
                                                    {
                                                        dsChoice.Tables[0].DefaultView.RowFilter = "QuestionID='" + questionPk.Trim() + "' and isMatching=1";
                                                        DataView dvChoice = new DataView();
                                                        dvChoice = dsChoice.Tables[0].DefaultView;

                                                        hight = hight + 40;
                                                        check_hieght = hight / 750;
                                                        if (check_hieght == 1)
                                                        {
                                                            mypdfpage.SaveToDocument();
                                                            mypdfpage = page_question.NewPage();
                                                            hight = 30;
                                                        }
                                                        if (dvChoice.Count > 0)
                                                        {
                                                            match_following = page_question.NewTable(Fontarial3, dvChoice.Count, 5, 4);
                                                            match_following.SetBorders(Color.Black, 1, BorderType.None);

                                                            match_following.SetColumnsWidth(new int[] { 10, 100, 30, 10, 100 });

                                                            for (int newmatch = 0; newmatch < dvChoice.Count; newmatch++)
                                                            {
                                                                string choiceid = Convert.ToString(dvChoice[newmatch]["choiceID"]);
                                                                string lsno = Convert.ToString(dvChoice[newmatch]["choiceNo"]);
                                                                string lhs = Convert.ToString(dvChoice[newmatch]["LHS"]);

                                                                string rsno = Convert.ToString(dvChoice[newmatch]["AnswerSno"]);
                                                                string rhs = Convert.ToString(dvChoice[newmatch]["RHS"]);

                                                                string lImage = Convert.ToString(dvChoice[newmatch]["LHS_Image"]);
                                                                string RImage = Convert.ToString(dvChoice[newmatch]["RHS_Image"]);

                                                                match_following.Cell(newmatch, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                                                match_following.Cell(newmatch, 0).SetContent(Convert.ToString(lsno));
                                                                if (questionSubType.Trim() == "3" || questionSubType.Trim() == "4")
                                                                {
                                                                    match_following.Cell(newmatch, 1).SetContent(lhs);
                                                                }
                                                                else if (questionSubType.Trim() == "5" || questionSubType.Trim() == "6")
                                                                {
                                                                    if (!string.IsNullOrEmpty(lImage))
                                                                    {
                                                                        byte[] file = (byte[])dvChoice[newmatch]["LHS_Image"];
                                                                        memoryStream.Write(file, 0, file.Length);
                                                                        if (file.Length > 0)
                                                                        {
                                                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + question_pk + choiceid + ".jpeg")))
                                                                            {
                                                                                //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                                                            }
                                                                            else
                                                                            {
                                                                                thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + question_pk + choiceid + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                                                //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                                                            }
                                                                        }
                                                                    }
                                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + question_pk + choiceid + ".jpeg")))
                                                                    {

                                                                        questionMatchImage = page_question.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/" + question_pk + choiceid + ".jpeg"));

                                                                        //mypdfpage.Add(questionMatchImage, (page_question.PageWidth / 2) - imagewidth / 3, hight, 250);
                                                                        match_following.Cell(newmatch, 1).SetContent(questionMatchImage);

                                                                    }

                                                                }
                                                                match_following.Cell(newmatch, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                                match_following.Cell(newmatch, 2).SetContent(Convert.ToString("-"));
                                                                match_following.Cell(newmatch, 2).SetContentAlignment
            (ContentAlignment.MiddleLeft);

                                                                //LHS_Image  RHS_Image  match_following.Cell(newmatch, 3).SetContent(matchvalu1);
                                                                match_following.Cell(newmatch, 3).SetContent(Convert.ToString("[" + rsno + "].  "));

                                                                if (questionSubType.Trim() == "3" || questionSubType.Trim() == "5")
                                                                {
                                                                    match_following.Cell(newmatch, 4).SetContent(rhs);
                                                                }
                                                                else if (questionSubType.Trim() == "6" || questionSubType.Trim() == "4")
                                                                {
                                                                    if (!string.IsNullOrEmpty(RImage))
                                                                    {
                                                                        byte[] file = (byte[])dvChoice[newmatch]["RHS_Image"];
                                                                        memoryStream.Write(file, 0, file.Length);
                                                                        if (file.Length > 0)
                                                                        {
                                                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + question_pk + choiceid + ".jpeg")))
                                                                            {
                                                                                //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                                                            }
                                                                            else
                                                                            {
                                                                                thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + question_pk + choiceid + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                                                //image3.ImageUrl = "~/college/" + stdappno + ".jpeg";
                                                                            }
                                                                        }
                                                                    }
                                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + question_pk + choiceid + ".jpeg")))
                                                                    {
                                                                        questionMatchImageR = page_question.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/" + question_pk + choiceid + ".jpeg"));

                                                                        //mypdfpage.Add(questionMatchImage, (page_question.PageWidth / 2) - imagewidth / 3, hight, 250);
                                                                        //match_following.Cell(newmatch, 1)
                                                                        match_following.Cell(newmatch, 1).SetContent(questionMatchImageR);

                                                                        //PdfCircle pc = newPdfTablePage.CellArea(index, 2).InnerCircle(Color.Blue, 2);
                                                                        //pc.StrokeWidth = 3.5;
                                                                        //newPdfPage.Add(pc);

                                                                    }
                                                                }
                                                                match_following.Cell(newmatch, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                            }
                                                            addtabletopdf = match_following.CreateTablePage(new Gios.Pdf.PdfArea(page_question, (page_question.PageWidth / 2) - 200, hight, 400, (page_question.PageWidth / 2) + 200));
                                                            mypdfpage.Add(addtabletopdf);
                                                            double heights = addtabletopdf.Area.Height;
                                                            hight += Convert.ToInt32(heights);
                                                        }
                                                        else
                                                        {
                                                            if (matchs != "")
                                                            {
                                                                if (matchs.Contains("^"))
                                                                {
                                                                    if (matchs.Contains(';'))
                                                                    {
                                                                        string[] splitmatch = matchs.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);

                                                                        match_following = page_question.NewTable(Fontarial3, splitmatch.Length, 5, 4);
                                                                        match_following.SetBorders(Color.Black, 1, BorderType.None);

                                                                        match_following.SetColumnsWidth(new int[] { 10, 100, 30, 10, 100 });
                                                                        int q_no = 0;
                                                                        char alp = 'A';
                                                                        for (int mch = 0; mch < splitmatch.Length; mch++)
                                                                        {
                                                                            if (Convert.ToString(splitmatch[mch]).Contains(';'))
                                                                            {
                                                                                string[] split1 = splitmatch[mch].Split(';');
                                                                                if (Convert.ToString(split1[0]) != "" && Convert.ToString(split1[1]) != "")
                                                                                {
                                                                                    string matchvalu1 = split1[0];
                                                                                    string matchvalu2 = split1[1];
                                                                                    match_following.Cell(q_no, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                                                                    match_following.Cell(q_no, 0).SetContent(Convert.ToString("[" + (q_no + 1) + "].  "));
                                                                                    match_following.Cell(q_no, 1).SetContent(matchvalu1);
                                                                                    match_following.Cell(q_no, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                                                    match_following.Cell(q_no, 2).SetContent(Convert.ToString("-"));
                                                                                    match_following.Cell(q_no, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                                                    match_following.Cell(q_no, 3).SetContent(matchvalu1);
                                                                                    match_following.Cell(q_no, 3).SetContent(Convert.ToString("[" + alp + "].  "));

                                                                                    match_following.Cell(q_no, 4).SetContent(matchvalu2);
                                                                                    match_following.Cell(q_no, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                                    q_no++;
                                                                                    alp++;
                                                                                }
                                                                            }
                                                                        }
                                                                        addtabletopdf = match_following.CreateTablePage(new Gios.Pdf.PdfArea(page_question, (page_question.PageWidth / 2) - 200, hight, 400, (page_question.PageWidth / 2) + 200));

                                                                        mypdfpage.Add(addtabletopdf);

                                                                        double heights = addtabletopdf.Area.Height;
                                                                        hight += Convert.ToInt32(heights);
                                                                    }
                                                                    else
                                                                    {
                                                                        string[] splitmatch = matchs.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);

                                                                        match_following = page_question.NewTable(Fontarial3, splitmatch.Length - 1, 2, 4);
                                                                        match_following.SetBorders(Color.Black, 1, BorderType.None);

                                                                        match_following.SetColumnsWidth(new int[] { 10, 100 });
                                                                        int q_no = 0;

                                                                        for (int mch = 0; mch < splitmatch.Length; mch++)
                                                                        {
                                                                            if (Convert.ToString(splitmatch[mch]) != "")
                                                                            {
                                                                                string matchvalu1 = splitmatch[mch];
                                                                                match_following.Cell(q_no, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                                                                match_following.Cell(q_no, 0).SetContent(Convert.ToString("[" + (q_no + 1) + "].  "));
                                                                                match_following.Cell(q_no, 1).SetContent(matchvalu1);
                                                                                match_following.Cell(q_no, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                                                q_no++;

                                                                            }

                                                                        }
                                                                        addtabletopdf = match_following.CreateTablePage(new Gios.Pdf.PdfArea(page_question, (page_question.PageWidth / 2) - 200, hight, 400, (page_question.PageWidth / 2) + 200));

                                                                        mypdfpage.Add(addtabletopdf);
                                                                        double heights = addtabletopdf.Area.Height;
                                                                        hight += Convert.ToInt32(heights);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (matchs != "")
                                                        {
                                                            hight = hight + 40;
                                                            check_hieght = hight / 750;
                                                            if (check_hieght == 1)
                                                            {
                                                                mypdfpage.SaveToDocument();
                                                                mypdfpage = page_question.NewPage();
                                                                hight = 30;
                                                            }
                                                            if (matchs.Contains("^"))
                                                            {
                                                                if (matchs.Contains(';'))
                                                                {
                                                                    string[] splitmatch = matchs.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);

                                                                    match_following = page_question.NewTable(Fontarial3, splitmatch.Length, 5, 4);
                                                                    match_following.SetBorders(Color.Black, 1, BorderType.None);

                                                                    match_following.SetColumnsWidth(new int[] { 10, 100, 30, 10, 100 });
                                                                    int q_no = 0;
                                                                    char alp = 'A';
                                                                    for (int mch = 0; mch < splitmatch.Length; mch++)
                                                                    {
                                                                        if (Convert.ToString(splitmatch[mch]).Contains(';'))
                                                                        {
                                                                            string[] split1 = splitmatch[mch].Split(';');
                                                                            if (Convert.ToString(split1[0]) != "" && Convert.ToString(split1[1]) != "")
                                                                            {
                                                                                string matchvalu1 = split1[0];
                                                                                string matchvalu2 = split1[1];
                                                                                match_following.Cell(q_no, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                                                                match_following.Cell(q_no, 0).SetContent(Convert.ToString("[" + (q_no + 1) + "].  "));
                                                                                match_following.Cell(q_no, 1).SetContent(matchvalu1);
                                                                                match_following.Cell(q_no, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                                                match_following.Cell(q_no, 2).SetContent(Convert.ToString("-"));
                                                                                match_following.Cell(q_no, 2).SetContentAlignment
                            (ContentAlignment.MiddleLeft);

                                                                                match_following.Cell(q_no, 3).SetContent(matchvalu1);
                                                                                match_following.Cell(q_no, 3).SetContent(Convert.ToString("[" + alp + "].  "));

                                                                                match_following.Cell(q_no, 4).SetContent(matchvalu2);
                                                                                match_following.Cell(q_no, 4).SetContentAlignment
                            (ContentAlignment.MiddleLeft);
                                                                                q_no++;
                                                                                alp++;
                                                                            }
                                                                        }
                                                                    }
                                                                    addtabletopdf = match_following.CreateTablePage(new Gios.Pdf.PdfArea(page_question, (page_question.PageWidth / 2) - 200, hight, 400, (page_question.PageWidth / 2) + 200));

                                                                    mypdfpage.Add(addtabletopdf);

                                                                    double heights = addtabletopdf.Area.Height;
                                                                    hight += Convert.ToInt32(heights);

                                                                }
                                                                else
                                                                {
                                                                    string[] splitmatch = matchs.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);

                                                                    match_following = page_question.NewTable(Fontarial3, splitmatch.Length - 1, 2, 4);
                                                                    match_following.SetBorders(Color.Black, 1, BorderType.None);

                                                                    match_following.SetColumnsWidth(new int[] { 10, 100 });
                                                                    int q_no = 0;

                                                                    for (int mch = 0; mch < splitmatch.Length; mch++)
                                                                    {

                                                                        if (Convert.ToString(splitmatch[mch]) != "")
                                                                        {
                                                                            string matchvalu1 = splitmatch[mch];
                                                                            match_following.Cell(q_no, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                                                            match_following.Cell(q_no, 0).SetContent(Convert.ToString("[" + (q_no + 1) + "].  "));
                                                                            match_following.Cell(q_no, 1).SetContent(matchvalu1);
                                                                            match_following.Cell(q_no, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                                            q_no++;

                                                                        }

                                                                    }
                                                                    addtabletopdf = match_following.CreateTablePage(new Gios.Pdf.PdfArea(page_question, (page_question.PageWidth / 2) - 200, hight, 400, (page_question.PageWidth / 2) + 200));

                                                                    mypdfpage.Add(addtabletopdf);
                                                                    double heights = addtabletopdf.Area.Height;
                                                                    hight += Convert.ToInt32(heights);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    if (matchs != "")
                                                    {
                                                        hight = hight + 40;
                                                        check_hieght = hight / 750;
                                                        if (check_hieght == 1)
                                                        {
                                                            mypdfpage.SaveToDocument();
                                                            mypdfpage = page_question.NewPage();
                                                            hight = 30;
                                                        }
                                                        if (matchs.Contains("^"))
                                                        {
                                                            if (matchs.Contains(';'))
                                                            {
                                                                string[] splitmatch = matchs.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);

                                                                match_following = page_question.NewTable(Fontarial3, splitmatch.Length, 5, 4);
                                                                match_following.SetBorders(Color.Black, 1, BorderType.None);

                                                                match_following.SetColumnsWidth(new int[] { 10, 100, 30, 10, 100 });
                                                                int q_no = 0;
                                                                char alp = 'A';
                                                                for (int mch = 0; mch < splitmatch.Length; mch++)
                                                                {
                                                                    if (Convert.ToString(splitmatch[mch]).Contains(';'))
                                                                    {
                                                                        string[] split1 = splitmatch[mch].Split(';');
                                                                        if (Convert.ToString(split1[0]) != "" && Convert.ToString(split1[1]) != "")
                                                                        {
                                                                            string matchvalu1 = split1[0];
                                                                            string matchvalu2 = split1[1];
                                                                            match_following.Cell(q_no, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                                                            match_following.Cell(q_no, 0).SetContent(Convert.ToString("[" + (q_no + 1) + "].  "));
                                                                            match_following.Cell(q_no, 1).SetContent(matchvalu1);
                                                                            match_following.Cell(q_no, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                                            match_following.Cell(q_no, 2).SetContent(Convert.ToString("-"));
                                                                            match_following.Cell(q_no, 2).SetContentAlignment
                        (ContentAlignment.MiddleLeft);

                                                                            match_following.Cell(q_no, 3).SetContent(matchvalu1);
                                                                            match_following.Cell(q_no, 3).SetContent(Convert.ToString("[" + alp + "].  "));

                                                                            match_following.Cell(q_no, 4).SetContent(matchvalu2);
                                                                            match_following.Cell(q_no, 4).SetContentAlignment
                        (ContentAlignment.MiddleLeft);
                                                                            q_no++;
                                                                            alp++;
                                                                        }
                                                                    }
                                                                }
                                                                addtabletopdf = match_following.CreateTablePage(new Gios.Pdf.PdfArea(page_question, (page_question.PageWidth / 2) - 200, hight, 400, (page_question.PageWidth / 2) + 200));

                                                                mypdfpage.Add(addtabletopdf);

                                                                double heights = addtabletopdf.Area.Height;
                                                                hight += Convert.ToInt32(heights);

                                                            }
                                                            else
                                                            {
                                                                string[] splitmatch = matchs.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);

                                                                match_following = page_question.NewTable(Fontarial3, splitmatch.Length, 2, 4);
                                                                match_following.SetBorders(Color.Black, 1, BorderType.None);

                                                                match_following.SetColumnsWidth(new int[] { 10, 100 });
                                                                int q_no = 0;

                                                                for (int mch = 0; mch < splitmatch.Length; mch++)
                                                                {

                                                                    if (Convert.ToString(splitmatch[mch]) != "")
                                                                    {
                                                                        string matchvalu1 = splitmatch[mch];
                                                                        match_following.Cell(q_no, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                                                        match_following.Cell(q_no, 0).SetContent(Convert.ToString("[" + (q_no + 1) + "].  "));
                                                                        match_following.Cell(q_no, 1).SetContent(matchvalu1);
                                                                        match_following.Cell(q_no, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                                        q_no++;
                                                                    }
                                                                }
                                                                addtabletopdf = match_following.CreateTablePage(new Gios.Pdf.PdfArea(page_question, (page_question.PageWidth / 2) - 200, hight, 400, (page_question.PageWidth / 2) + 200));

                                                                mypdfpage.Add(addtabletopdf);
                                                                double heights = addtabletopdf.Area.Height;
                                                                hight += Convert.ToInt32(heights);
                                                            }
                                                        }
                                                    }
                                                    break;
                                            }
                                            break;
                                        case "5":
                                            string[] qrearrange = questionMatchingName.Split(new string[] { "#Qpara#" }, StringSplitOptions.RemoveEmptyEntries);
                                            if (qrearrange.Length > 0)
                                            {
                                                hight = hight + 40;
                                                check_hieght = hight / 750;
                                                if (check_hieght == 1)
                                                {
                                                    mypdfpage.SaveToDocument();
                                                    mypdfpage = page_question.NewPage();
                                                    hight = 30;
                                                }

                                                match_following = page_question.NewTable(Fontarial3, qrearrange.Length, 2, 4);
                                                match_following.SetBorders(Color.Black, 1, BorderType.None);
                                                match_following.SetColumnsWidth(new int[] { 10, 100 });

                                                int q_no = 0;
                                                for (int mch = 0; mch < qrearrange.Length; mch++)
                                                {
                                                    if (Convert.ToString(qrearrange[mch]) != "")
                                                    {
                                                        string matchvalu1 = qrearrange[mch];
                                                        match_following.Cell(q_no, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                                        match_following.Cell(q_no, 0).SetContent(Convert.ToString("[" + (q_no + 1) + "].  "));
                                                        match_following.Cell(q_no, 1).SetContent(matchvalu1);
                                                        match_following.Cell(q_no, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        q_no++;
                                                    }
                                                }
                                                addtabletopdf = match_following.CreateTablePage(new Gios.Pdf.PdfArea(page_question, (page_question.PageWidth / 2) - 200, hight, 400, (page_question.PageWidth / 2) + 200));

                                                mypdfpage.Add(addtabletopdf);
                                                double heights = addtabletopdf.Area.Height;
                                                hight += Convert.ToInt32(heights);

                                            }
                                            break;
                                        case "6":
                                            string[] qPara = questionMatchingName.Split(new string[] { "#Qpara#" }, StringSplitOptions.RemoveEmptyEntries);
                                            string[] qAnswer = questionAnswer.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                            string[] qOptions = queswtionOptions.Split(new string[] { "#Qpara#" }, StringSplitOptions.RemoveEmptyEntries);
                                            if (qPara.Length > 0 && Convert.ToInt16(totalChoices) > 0)
                                            {
                                                hight = hight + 90;
                                                check_hieght = hight / 750;
                                                if (check_hieght == 1)
                                                {
                                                    mypdfpage.SaveToDocument();
                                                    mypdfpage = page_question.NewPage();
                                                    hight = 30;
                                                }
                                                match_following = page_question.NewTable(Fontarial3, qPara.Length * Convert.ToInt16(totalChoices) + qPara.Length, 2, 4);
                                                match_following.SetBorders(Color.Black, 1, BorderType.None);
                                                match_following.SetColumnsWidth(new int[] { 10, 100 });

                                                int q_no = 0;
                                                int opno = 1;
                                                for (int para = 0; para < qPara.Length; para++)
                                                {
                                                    if (Convert.ToString(qPara[para]) != "")
                                                    {
                                                        string matchvalu1 = qPara[para];
                                                        int qrow = para * Convert.ToInt16(totalChoices);//((para + 1) * Convert.ToInt16(q_no + 1) * opno) - 1;  + ((para == 0) ? 0 : 1)
                                                        opno = 1;
                                                        match_following.Cell(qrow + para, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                                        match_following.Cell(qrow + para, 0).SetContent(Convert.ToString("[" + (q_no + 1) + "].  "));
                                                        match_following.Cell(qrow + para, 1).SetContent(matchvalu1);
                                                        match_following.Cell(qrow + para, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        string[] qparaopt = qOptions[para].Split(new string[] { "#Qparaopt#" }, StringSplitOptions.RemoveEmptyEntries);
                                                        for (int col = 0; col < qparaopt.Length; col++)
                                                        {
                                                            //TextBox txtopt = gvParagraph.Rows[para].FindControl("txtParaOptions" + para + (col + 3 + 1)) as TextBox;
                                                            //txtopt.Text = qparaopt[col];
                                                            match_following.Cell(qrow + para + opno, 1).SetContent(qparaopt[col]);
                                                            match_following.Cell(qrow + para + opno, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                            opno++;
                                                        }
                                                        q_no++;
                                                    }
                                                }
                                                addtabletopdf = match_following.CreateTablePage(new Gios.Pdf.PdfArea(page_question, (page_question.PageWidth / 2) - 200, hight, 400, (page_question.PageWidth / 2) + 200));

                                                mypdfpage.Add(addtabletopdf);
                                                double heights = addtabletopdf.Area.Height;
                                                hight += Convert.ToInt32(heights);

                                            }
                                            break;
                                        default:
                                            matchs = Convert.ToString(dv[i]["qmatching"]);
                                            if (questionMatchingorNot.Trim().ToLower() == "true")
                                            {
                                                if (matchs != "")
                                                {
                                                    hight = hight + 40;
                                                    check_hieght = hight / 750;
                                                    if (check_hieght == 1)
                                                    {
                                                        mypdfpage.SaveToDocument();
                                                        mypdfpage = page_question.NewPage();
                                                        hight = 30;
                                                    }
                                                    if (matchs.Contains("^"))
                                                    {
                                                        if (matchs.Contains(';'))
                                                        {
                                                            string[] splitmatch = matchs.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);

                                                            match_following = page_question.NewTable(Fontarial3, splitmatch.Length, 5, 4);
                                                            match_following.SetBorders(Color.Black, 1, BorderType.None);

                                                            match_following.SetColumnsWidth(new int[] { 10, 100, 30, 10, 100 });
                                                            int q_no = 0;
                                                            char alp = 'A';
                                                            for (int mch = 0; mch < splitmatch.Length; mch++)
                                                            {
                                                                if (Convert.ToString(splitmatch[mch]).Contains(';'))
                                                                {
                                                                    string[] split1 = splitmatch[mch].Split(';');
                                                                    if (Convert.ToString(split1[0]) != "" && Convert.ToString(split1[1]) != "")
                                                                    {
                                                                        string matchvalu1 = split1[0];
                                                                        string matchvalu2 = split1[1];
                                                                        match_following.Cell(q_no, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                                                        match_following.Cell(q_no, 0).SetContent(Convert.ToString("[" + (q_no + 1) + "].  "));
                                                                        match_following.Cell(q_no, 1).SetContent(matchvalu1);
                                                                        match_following.Cell(q_no, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                                        match_following.Cell(q_no, 2).SetContent(Convert.ToString("-"));
                                                                        match_following.Cell(q_no, 2).SetContentAlignment
                    (ContentAlignment.MiddleLeft);

                                                                        match_following.Cell(q_no, 3).SetContent(matchvalu1);
                                                                        match_following.Cell(q_no, 3).SetContent(Convert.ToString("[" + alp + "].  "));

                                                                        match_following.Cell(q_no, 4).SetContent(matchvalu2);
                                                                        match_following.Cell(q_no, 4).SetContentAlignment
                    (ContentAlignment.MiddleLeft);
                                                                        q_no++;
                                                                        alp++;
                                                                    }
                                                                }
                                                            }
                                                            addtabletopdf = match_following.CreateTablePage(new Gios.Pdf.PdfArea(page_question, (page_question.PageWidth / 2) - 200, hight, 400, (page_question.PageWidth / 2) + 200));

                                                            mypdfpage.Add(addtabletopdf);

                                                            double heights = addtabletopdf.Area.Height;
                                                            hight += Convert.ToInt32(heights);

                                                        }
                                                        else
                                                        {
                                                            string[] splitmatch = matchs.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);

                                                            match_following = page_question.NewTable(Fontarial3, splitmatch.Length, 2, 4);
                                                            match_following.SetBorders(Color.Black, 1, BorderType.None);

                                                            match_following.SetColumnsWidth(new int[] { 10, 100 });
                                                            int q_no = 0;

                                                            for (int mch = 0; mch < splitmatch.Length; mch++)
                                                            {

                                                                if (Convert.ToString(splitmatch[mch]) != "")
                                                                {
                                                                    string matchvalu1 = splitmatch[mch];
                                                                    match_following.Cell(q_no, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                                                    match_following.Cell(q_no, 0).SetContent(Convert.ToString("[" + (q_no + 1) + "].  "));
                                                                    match_following.Cell(q_no, 1).SetContent(matchvalu1);
                                                                    match_following.Cell(q_no, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                                    q_no++;
                                                                }
                                                            }
                                                            addtabletopdf = match_following.CreateTablePage(new Gios.Pdf.PdfArea(page_question, (page_question.PageWidth / 2) - 200, hight, 400, (page_question.PageWidth / 2) + 200));

                                                            mypdfpage.Add(addtabletopdf);
                                                            double heights = addtabletopdf.Area.Height;
                                                            hight += Convert.ToInt32(heights);
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                }

                                if (questionType.Trim() != "6")
                                {
                                    string option = Convert.ToString(dv[i]["options"]).Trim();
                                    int optn = option.Length;
                                    double htsp = 0;
                                    if (option != "")
                                    {
                                        hight = hight + 40;
                                        if (option.Contains(';') || option.Contains("#malang#"))
                                        {
                                            string[] split1 = (!option.Contains("#malang#")) ? (option.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)) : (option.Split(new string[] { "#malang#" }, StringSplitOptions.RemoveEmptyEntries));
                                            if (option != "")
                                            {
                                                Gios.Pdf.PdfTable optional = page_question.NewTable(Fontarial3, split1.Length, 4, 4);
                                                //optional.VisibleHeaders = false;
                                                //  optional.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                optional.SetBorders(Color.Black, 1, BorderType.None);
                                                optional.Columns[0].SetWidth(2);
                                                optional.Columns[2].SetWidth(2);

                                                for (int row = 0; row < split1.Length; row = row + 2)
                                                {
                                                    string opt = Convert.ToString(split1[row]).Trim();
                                                    if (opt != "")
                                                    {
                                                        // int widthopt = opt.Length * 5;
                                                        int widthopt = opt.Length;
                                                        optional.Cell(row, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                                        optional.Cell(row, 0).SetContent(Convert.ToString("[" + (row + 1) + "]"));

                                                        optional.Cell(row, 1).SetContent(opt);
                                                        optional.Cell(row, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                        if ((row + 1) < split1.Length)
                                                        {
                                                            string opt2 = Convert.ToString(split1[row + 1]).Trim();

                                                            if (opt2 != "")
                                                            {
                                                                optional.Cell(row, 2).SetContent(Convert.ToString("[" + (row + 2) + "]"));
                                                                optional.Cell(row, 2).SetContentAlignment(ContentAlignment.TopLeft);
                                                                optional.Cell(row, 3).SetContent(opt2);
                                                                optional.Cell(row, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                            }
                                                        }
                                                    }
                                                }
                                                double ht = 0;
                                                for (int op_count = 0; op_count < split1.Length; op_count++)
                                                {
                                                    string opt = Convert.ToString(split1[op_count]).Trim();
                                                    int widthopt = opt.Length;
                                                    if (widthopt > 35)
                                                    {
                                                        ht = widthopt / 3;
                                                        if (htsp == 0)
                                                        {
                                                            htsp = ht;
                                                        }
                                                        else
                                                        {
                                                            if (htsp < ht)
                                                            {
                                                                htsp = ht;
                                                            }
                                                        }

                                                    }

                                                }
                                                if (ht != 0)
                                                {
                                                    htsp = ht + 40;
                                                }
                                                else
                                                {
                                                    htsp = 30;
                                                }

                                                check_hieght = hight / 800;
                                                if (check_hieght == 1)
                                                {
                                                    mypdfpage.SaveToDocument();
                                                    mypdfpage = page_question.NewPage();
                                                    hight = 30;
                                                }

                                                addtabletopdf = optional.CreateTablePage(new Gios.Pdf.PdfArea(page_question, 40, hight, 500, 100));

                                                mypdfpage.Add(addtabletopdf);

                                            }
                                        }
                                        hight = hight + Convert.ToInt32(htsp);
                                    }
                                }

                            }
                        }
                    }
                }
            }
            mypdfpage.SaveToDocument();

            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = pdfname.Trim().Replace(" ", "_") + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                page_question.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch (Exception ex)
        {
            lbl_alert1.Visible = true;
            lbl_alert1.Text = ex.Message;
        }
    }

    public void generatePDF()
    {
        try
        {
            string qry1 = string.Empty;
            string qrysec = string.Empty;
            string collegeName = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            string semester = string.Empty;
            string section = string.Empty;
            string subjectNo = string.Empty;
            string degreeName = string.Empty;
            string courseName = string.Empty;
            string displayDept = string.Empty;
            string subjectName = string.Empty;
            string testName = string.Empty;
            string examDuration = string.Empty;


            bool leftLogo = false;
            bool rightLogo = false;

            StringBuilder sbQuestionHtml = new StringBuilder();

            DataSet dsQuestionSections = new DataSet();
            DataSet dsAllQuestions = new DataSet();
            DataSet dsChoice = new DataSet();


            if (ddl_collegename.Items.Count > 0)
            {
                collegeName = Convert.ToString(ddl_collegename.SelectedItem.Text).Trim();
            }
            if (ddlbatch.Items.Count > 0)
            {
                batchYear = Convert.ToString(ddlbatch.SelectedItem.Value).Trim();
            }
            if (ddldegree.Items.Count > 0)
            {
                degreeName = Convert.ToString(ddldegree.SelectedItem.Text).Trim();
            }
            if (ddlbranch.Items.Count > 0)
            {
                degreeCode = Convert.ToString(ddlbranch.SelectedItem.Value).Trim();
                courseName = Convert.ToString(ddlbranch.SelectedItem.Text).Trim();
            }
            if (ddlsem.Items.Count > 0)
            {
                semester = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            }
            if (ddlsec.Items.Count > 0)
            {
                if (Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower() != "all" && string.IsNullOrEmpty(Convert.ToString(ddlsec.SelectedItem.Text).Trim().ToLower()))
                {
                    section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                    qrysec = "  and  qb.Sections='" + section + "'";
                }
            }
            if (ddlsubject.Items.Count > 0)
            {
                subjectNo = Convert.ToString(ddlsubject.SelectedValue.Trim()).Trim();
                subjectName = Convert.ToString(ddlsubject.SelectedItem.Text.Trim()).Trim();
            }

            displayDept = ((!string.IsNullOrEmpty(batchYear.Trim()) ? batchYear.Trim() + " - " : "") + (!string.IsNullOrEmpty(degreeName.Trim()) ? degreeName.Trim() + " - " : "") + (!string.IsNullOrEmpty(courseName.Trim()) ? courseName.Trim() + " - " : "") + (!string.IsNullOrEmpty(semester.Trim()) ? " Sem : " + semester.Trim() + (!string.IsNullOrEmpty(section.Trim()) ? " - " + section.Trim() : "") : ""));
            int totalmark = 0;
            int sno = 0;
            int activerow = FpSpread1.ActiveSheetView.ActiveRow;
            int activecol = FpSpread1.ActiveSheetView.ActiveColumn;
            string isInternalOrExternal = Convert.ToString(FpSpread1.Sheets[0].Cells[activerow, 2].Tag);
            string monthName = Convert.ToString(FpSpread1.Sheets[0].Cells[activerow, 3].Note);
            string criteriaNo = Convert.ToString(FpSpread1.Sheets[0].Cells[activerow, 3].Tag);

            string qryInternalExternal = "";
            if (isInternalOrExternal == "1")
            {
                qryInternalExternal = " and qb.exam_month=eq.Exam_month and eq.Exam_year=qb.exam_year and qb.Exam_month='" + monthName + "' and qb.Exam_year='" + criteriaNo + "' and eq.is_internal='1' ";//and qb.Exam_month='" + monthName + "' and qb.Exam_year='" + criteriaNo + "' and eq.is_internal='1'
                testName = monthName + " - " + criteriaNo + "";
                examDuration = d2.GetFunction("select  Duration  from tbl_question_bank_master qb where  Batch_year='" + batchYear + "'and Degree_Code='" + degreeCode + "' and  Semester='" + semester + "' " + qrysec + " and exam_year='" + criteriaNo + "' and exam_month='" + monthName + "' ");
            }
            else if (isInternalOrExternal == "2")
            {
                testName = d2.GetFunction("select c.criteria from CriteriaForInternal c, syllabus_master sy where sy.syll_code=c.syll_code and sy.semester='" + semester + "' and sy.degree_code='" + degreeCode + "' and sy.Batch_Year='" + batchYear + "' and c.Criteria_no ='" + criteriaNo + "' ");
                examDuration = d2.GetFunction("select  Duration  from tbl_question_bank_master qb where  Batch_year='" + batchYear + "'and Degree_Code='" + degreeCode + "' and  Semester='" + semester + "' " + qrysec + " and Exam= '" + criteriaNo.Trim() + "' ");
                qryInternalExternal = " and eq.Test_code=qb.Exam  and eq.Test_code='" + criteriaNo + "' and eq.is_internal='2' ";//and eq.Test_code=qb.Exam  and eq.Test_code='" + criteriaNo + "' and eq.is_internal='2' 
            }

            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
            {
                leftLogo = true;
            }
            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
            {
                rightLogo = true;
            }

            string qry = " select COUNT(Exist_questionPK) Total_Quesions,eq.Section,tq.mark,Must_attend,eq.section_name from tbl_question_bank_master qb, tbl_question_master tq,Exist_questions eq where tq.Subject_no=qb.Subject_no and eq.subject_no=qb.Subject_no and eq.subject_no=tq.subject_no and tq.QuestionMasterPK=eq.QuestionMasterFK and eq.subject_no='" + subjectNo + "' and qb.Batch_year='" + batchYear + "' and qb.Degree_Code='" + degreeCode + "' and   qb.Semester='" + semester + "' " + qrysec + qryInternalExternal + "  group by Section,Mark,Must_attend,eq.section_name ";

            dsQuestionSections.Clear();
            dsQuestionSections = d2.select_method_wo_parameter(qry, "Text");
            if (dsQuestionSections.Tables.Count > 0 && dsQuestionSections.Tables[0].Rows.Count > 0)
            {
                for (int rows = 0; rows < dsQuestionSections.Tables[0].Rows.Count; rows++)
                {
                    string mustAttend = Convert.ToString(dsQuestionSections.Tables[0].Rows[rows]["Must_attend"]).Trim();
                    string mark = Convert.ToString(dsQuestionSections.Tables[0].Rows[rows]["mark"]).Trim();
                    if (!string.IsNullOrEmpty(mark.Trim()) && !string.IsNullOrEmpty(mustAttend.Trim()))
                    {
                        int mulmark = Convert.ToInt32(mustAttend) * Convert.ToInt32(mark);
                        totalmark = totalmark + mulmark;
                    }
                }

                sbQuestionHtml.Append("<div style='padding-left:5px;height: 1110px; width:774px;'><table class='collegeHeader' style='height:auto;width:100%; margin:2px; padding:0px;'><tr><td   align='left' rowspan='4' style='height:auto; width:15%;'>" + ((leftLogo) ? "<img src='" + "college/Left_Logo.jpeg" + "' alt='' width='80px' height='80px;'/>" : "") + "</td><td style='text-align:center; height:auto; width:65%; padding:3px;'>" + collegeName + "</td><td align='right' rowspan='4' style='height:auto; width:15%;'>" + ((rightLogo) ? "<img src='" + "college/Right_Logo.jpeg" + "' alt='' width='80px' height='80px;'/>" : "") + "</td></tr><tr><td style='text-align:center; height:auto; width:65%; padding:3px;'>" + displayDept + "</td></tr><tr><td style='text-align:center; height:auto; width:65%; padding:3px;'>" + testName + "</td></tr><tr><td style='text-align:center; height:auto; width:65%; padding:3px;'>" + subjectName + "</td></tr><tr><td style='text-align:left; font-size:10px;'>Total Marks : " + totalmark + "</td><td></td><td style='text-align:right; font-size:10px;'>Duration : " + examDuration + "</td></tr></table><hr/>");

                qry1 = " select tq.subject_no,tq.syllabus,eq.Section,eq.QNo,eq.Exist_questionPK,tq.QuestionMasterPK,question,mark,is_descriptive,type,options,answer,Must_attend,tq.QuestionType,tq.QuestionSubType,tq.totalChoice,isnull(tq.is_matching,0) as is_matching,tq.qmatching,tq.quetion_image,ISNULL(needChoice,0) as needChoice from tbl_question_bank_master qb, tbl_question_master tq, Exist_questions eq  where tq.Subject_no=qb.Subject_no and eq.subject_no=qb.Subject_no and eq.subject_no=tq.subject_no and tq.QuestionMasterPK=eq.QuestionMasterFK and  tq.syllabus=eq.syllabus and eq.subject_no=tq.subject_no and  eq.subject_no ='" + subjectNo + "' and qb.Batch_year='" + batchYear + "' and qb.Degree_Code='" + degreeCode + "' and   qb.Semester='" + semester + "' " + qrysec + qryInternalExternal + "  order by eq.QNo,eq.Exist_questionPK,eq.Section";
                dsAllQuestions = d2.select_method_wo_parameter(qry1, "Text");

                qry1 = "select qc.choiceID,qc.QuestionID,qc.choiceNo,qc.QChoice as LHS,qc.QChoiceImage as LHS_Image,CHAR(64 + choiceNo) as AnswerSno,qc.QMatchR as RHS,qc.QChoiceImageR as RHS_Image,isAnswer,isMatching from tbl_question_bank_master qb,tbl_question_master tq,QuestionsChoice qc,Exist_questions eq where tq.Subject_no=qb.Subject_no and eq.subject_no=qb.Subject_no and eq.subject_no=tq.subject_no and tq.QuestionMasterPK=eq.QuestionMasterFK and eq.QuestionMasterFK=qc.QuestionID and tq.QuestionMasterPK=qc.QuestionID and tq.QuestionMasterPK=eq.QuestionMasterFK and tq.syllabus=eq.syllabus and eq.subject_no=tq.subject_no and  eq.subject_no ='" + subjectNo + "' and qb.Batch_year='" + batchYear + "' and qb.Degree_Code='" + degreeCode + "' and   qb.Semester='" + semester + "' " + qrysec + qryInternalExternal + "  order by qc.QuestionID";
                dsChoice = d2.select_method_wo_parameter(qry1, "Text");

                for (int qsection = 0; qsection < dsQuestionSections.Tables[0].Rows.Count; qsection++)
                {
                    string attendQuestions = Convert.ToString(dsQuestionSections.Tables[0].Rows[qsection]["Must_attend"]).Trim();
                    string totalQuestions = Convert.ToString(dsQuestionSections.Tables[0].Rows[qsection]["Total_Quesions"]).Trim();
                    string marks = Convert.ToString(dsQuestionSections.Tables[0].Rows[qsection]["mark"]).Trim();
                    string qsections = Convert.ToString(dsQuestionSections.Tables[0].Rows[qsection]["Section"]).Trim();
                    string section_name = Convert.ToString(dsQuestionSections.Tables[0].Rows[qsection]["section_name"]).Trim();

                    string partHeading = "Part - " + qsections + " -  " + section_name + "  ( " + attendQuestions + " Out of " + totalQuestions + " ) ";
                    string partMarks = attendQuestions + " X " + marks + "  = " + Convert.ToString(Convert.ToInt32(attendQuestions) * Convert.ToInt32(marks));

                    sbQuestionHtml.Append("<table class='subHead' style='height:auto;width:100%; padding:3px;'><tr><td style='width:85%; height:auto; text-align:left;'>" + partHeading + "</td><td style='width:10%; height:auto; text-align:right;'>" + partMarks + "</td></tr></table>");
                    if (dsAllQuestions.Tables.Count > 0 && dsAllQuestions.Tables[0].Rows.Count > 0)
                    {
                        DataView dvQuestions = new DataView();
                        dsAllQuestions.Tables[0].DefaultView.RowFilter = "Section='" + qsections + "' and mark='" + marks + "'";
                        dvQuestions = dsAllQuestions.Tables[0].DefaultView;

                        if (dvQuestions.Count > 0)
                        {
                            sbQuestionHtml.Append("<table style='height:auto;width:100%; padding:3px;'>");
                            for (int q = 0; q < dvQuestions.Count; q++)
                            {
                                DataTable dtMatch = new DataTable();
                                DataTable dtOptions = new DataTable();

                                sbQuestionHtml.Append("<tr >");

                                byte[] questionImagebyte = new byte[0];

                                string questionNo = Convert.ToString(dvQuestions[q]["QNo"]).Trim();
                                string questionPk = Convert.ToString(dvQuestions[q]["QuestionMasterPK"]).Trim();
                                string questionName = Convert.ToString(dvQuestions[q]["question"]).Trim();
                                string questionOptions = Convert.ToString(dvQuestions[q]["options"]).Trim();
                                string questionAnswer = Convert.ToString(dvQuestions[q]["answer"]).Trim();
                                string questionMark = Convert.ToString(dvQuestions[q]["mark"]).Trim();

                                string questionMatchingName = Convert.ToString(dvQuestions[q]["qmatching"]).Trim();
                                string questionMatchingorNot = Convert.ToString(dvQuestions[q]["is_matching"]).Trim();

                                string questionObjDesc = Convert.ToString(dvQuestions[q]["is_descriptive"]).Trim();
                                string questionGrade = Convert.ToString(dvQuestions[q]["type"]).Trim();
                                string questionType = Convert.ToString(dvQuestions[q]["QuestionType"]).Trim();
                                string questionSubType = Convert.ToString(dvQuestions[q]["QuestionSubType"]).Trim();
                                string totalChoices = Convert.ToString(dvQuestions[q]["totalChoice"]).Trim();
                                string questionImage = Convert.ToString(dvQuestions[q]["quetion_image"]).Trim();

                                bool isMatching = false;
                                bool.TryParse(questionMatchingorNot.Trim(), out isMatching);
                                bool needChoice = false;
                                bool.TryParse(Convert.ToString(dvQuestions[q]["needChoice"]).Trim(), out needChoice);
                                sbQuestionHtml.Append("<td colspan='2' align='left'>");
                                sno++;
                                if (!string.IsNullOrEmpty(questionName.Trim()))
                                {
                                    sbQuestionHtml.Append(Convert.ToString(sno) + "." + questionName.Trim());
                                }
                                sbQuestionHtml.Append("</td>");
                                sbQuestionHtml.Append("</tr>");
                                //"data:image/png;base64," + Convert.ToBase64String(imagByte);
                                sbQuestionHtml.Append("<tr>");
                                sbQuestionHtml.Append("<td colspan='2' align='center'>");
                                if (!string.IsNullOrEmpty(questionImage.Trim()))
                                {
                                    questionImagebyte = (byte[])(dvQuestions[q]["quetion_image"]);
                                    if (questionImagebyte.Length > 0)
                                    {
                                        sbQuestionHtml.Append("<img src='data:image/png;base64," + Convert.ToBase64String(questionImagebyte) + "' width='65px' height='65px;' alt=''>");

                                    }
                                }
                                sbQuestionHtml.Append("</td>");
                                sbQuestionHtml.Append("</tr>");

                                if (questionObjDesc.Trim() == "0")
                                {
                                    switch (questionType)
                                    {
                                        case "1":
                                        case "2":
                                        case "4":
                                            break;
                                        case "3":
                                        default:
                                            GetMatches(((dsChoice.Tables.Count > 0) ? dsChoice.Tables[0] : dtOptions), questionPk, isMatching, questionOptions, ref dtMatch, ref dtOptions, questionMatchingName);
                                            if (dtMatch.Rows.Count > 0)
                                            {
                                                sbQuestionHtml.Append("<tr>");
                                                sbQuestionHtml.Append("<td colspan='2'><table style='width:auto; margin-left:5%; margin-top:10px; padding:1px;'>");
                                                for (int qmatch = 0; qmatch < dtMatch.Rows.Count; qmatch++)
                                                {
                                                    sbQuestionHtml.Append("<tr>");

                                                    //sbQuestionHtml.Append("<td>");
                                                    //sbQuestionHtml.Append("</td>");
                                                    //sbQuestionHtml.Append("<td>");
                                                    //sbQuestionHtml.Append("</td>");
                                                    byte[] leftQuestion = new byte[0];
                                                    byte[] rightQuestion = new byte[0];
                                                    string choiceid = Convert.ToString(dtMatch.Rows[qmatch]["choiceID"]);
                                                    string lsno = Convert.ToString(dtMatch.Rows[qmatch]["choiceNo"]);
                                                    string lhs = Convert.ToString(dtMatch.Rows[qmatch]["LHS"]);
                                                    string lImage = Convert.ToString(dtMatch.Rows[qmatch]["LHS_Image"]);
                                                    string rsno = Convert.ToString(dtMatch.Rows[qmatch]["AnswerSno"]);
                                                    string rhs = Convert.ToString(dtMatch.Rows[qmatch]["RHS"]);
                                                    string RImage = Convert.ToString(dtMatch.Rows[qmatch]["RHS_Image"]);

                                                    if (string.IsNullOrEmpty(lhs))
                                                    {
                                                        if (!string.IsNullOrEmpty(lImage.Trim()))
                                                        {
                                                            leftQuestion = (byte[])(dtMatch.Rows[qmatch]["LHS_Image"]);
                                                            if (leftQuestion.Length > 0)
                                                            {
                                                                sbQuestionHtml.Append("<td align='left' style='text-align:left;'>");
                                                                sbQuestionHtml.Append(lsno + ".");
                                                                sbQuestionHtml.Append("</td>");
                                                                sbQuestionHtml.Append("<td align='left' style='text-align:left;'>");
                                                                sbQuestionHtml.Append("<img src='data:image/png;base64," + Convert.ToBase64String(leftQuestion) + "' width='50px' height='50px;' alt=''>");
                                                                sbQuestionHtml.Append("</td>");
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        sbQuestionHtml.Append("<td align='left' style='text-align:left;'>");
                                                        sbQuestionHtml.Append(lsno + ".");
                                                        sbQuestionHtml.Append("</td>");
                                                        sbQuestionHtml.Append("<td align='left' style='text-align:left;'>");
                                                        sbQuestionHtml.Append(lhs);
                                                        sbQuestionHtml.Append("</td>");
                                                    }
                                                    sbQuestionHtml.Append("<td align='center' style='width:15px; text-align:center;'>");
                                                    sbQuestionHtml.Append("-");
                                                    sbQuestionHtml.Append("</td>");
                                                    if (string.IsNullOrEmpty(rhs))
                                                    {
                                                        if (!string.IsNullOrEmpty(RImage.Trim()))
                                                        {
                                                            rightQuestion = (byte[])(dtMatch.Rows[qmatch]["RHS_Image"]);
                                                            if (rightQuestion.Length > 0)
                                                            {
                                                                sbQuestionHtml.Append("<td align='left' style='text-align:left;'>");
                                                                sbQuestionHtml.Append(rsno + ".");
                                                                sbQuestionHtml.Append("</td>");
                                                                sbQuestionHtml.Append("<td align='left' style='text-align:left;'>");
                                                                sbQuestionHtml.Append("<img src='data:image/png;base64," + Convert.ToBase64String(rightQuestion) + "' width='50px' height='50px;' alt=''>");
                                                                sbQuestionHtml.Append("</td>");
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        sbQuestionHtml.Append("<td align='left' style='text-align:left;'>");
                                                        sbQuestionHtml.Append(rsno + ".");
                                                        sbQuestionHtml.Append("</td>");
                                                        sbQuestionHtml.Append("<td align='left' style='text-align:left;'>");
                                                        sbQuestionHtml.Append(rhs);
                                                        sbQuestionHtml.Append("</td>");
                                                    }
                                                    sbQuestionHtml.Append("</tr>");
                                                }
                                                sbQuestionHtml.Append("</table></td>");
                                                sbQuestionHtml.Append("</tr>");
                                            }
                                            break;
                                        case "5":
                                            string[] qrearrange = questionMatchingName.Split(new string[] { "#Qpara#" }, StringSplitOptions.RemoveEmptyEntries);
                                            if (qrearrange.Length > 0)
                                            {
                                                sbQuestionHtml.Append("<tr>");
                                                sbQuestionHtml.Append("<td colspan='2'><table style='width:auto; margin-left:5%; margin-top:10px; padding:1px;'>");
                                                int qno = 1;
                                                for (int mch = 0; mch < qrearrange.Length; mch++)
                                                {
                                                    if (!string.IsNullOrEmpty(Convert.ToString(qrearrange[mch]).Trim()))
                                                    {
                                                        sbQuestionHtml.Append("<tr>");
                                                        string matchvalu1 = qrearrange[mch];
                                                        sbQuestionHtml.Append("<td align='left' style='text-align:left;'>");
                                                        sbQuestionHtml.Append("[" + qno + "].  ");
                                                        sbQuestionHtml.Append("</td>");
                                                        sbQuestionHtml.Append("<td align='left' style='text-align:left;'>");
                                                        sbQuestionHtml.Append(matchvalu1);
                                                        sbQuestionHtml.Append("</td>");
                                                        qno++;
                                                        sbQuestionHtml.Append("</tr>");
                                                    }
                                                }
                                                sbQuestionHtml.Append("</table></td>");
                                                sbQuestionHtml.Append("</tr>");
                                            }
                                            break;
                                        case "6":
                                            string[] qPara = questionMatchingName.Split(new string[] { "#Qpara#" }, StringSplitOptions.RemoveEmptyEntries);
                                            string[] qAnswer = questionAnswer.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                            string[] qOptions = questionOptions.Split(new string[] { "#Qpara#" }, StringSplitOptions.RemoveEmptyEntries);
                                            if (qPara.Length > 0)
                                            {
                                                sbQuestionHtml.Append("<tr>");
                                                sbQuestionHtml.Append("<td colspan='2'><table style='width:auto; margin-left:5%; margin-top:10px; padding:1px;'>");
                                                int qno = 1;
                                                int opno = 1;
                                                for (int para = 0; para < qPara.Length; para++)
                                                {
                                                    if (!string.IsNullOrEmpty(Convert.ToString(qPara[para]).Trim()))
                                                    {
                                                        string matchvalu1 = qPara[para];
                                                        int qrow = para * Convert.ToInt16(totalChoices);//((para + 1) * Convert.ToInt16(q_no + 1) * opno) - 1;  + ((para == 0) ? 0 : 1)
                                                        opno = 1;
                                                        sbQuestionHtml.Append("<tr>");
                                                        sbQuestionHtml.Append("<td align='left' style='text-align:left;'>");
                                                        sbQuestionHtml.Append("[" + qno + "].  ");
                                                        sbQuestionHtml.Append("</td>");
                                                        sbQuestionHtml.Append("<td align='left' style='text-align:left;'>");
                                                        sbQuestionHtml.Append(matchvalu1);
                                                        sbQuestionHtml.Append("</td>");
                                                        sbQuestionHtml.Append("</tr>");
                                                        string[] qparaopt = qOptions[para].Split(new string[] { "#Qparaopt#" }, StringSplitOptions.RemoveEmptyEntries);
                                                        if (qparaopt.Length > 0 && needChoice)
                                                        {
                                                            sbQuestionHtml.Append("<tr>");
                                                            sbQuestionHtml.Append("<td colspan='2'><table style='width:auto; margin-left:3%; margin-top:5px; margin-bottom:5px; padding:1px;'>");

                                                            for (int col = 0; col < qparaopt.Length; col++)
                                                            {
                                                                if (!string.IsNullOrEmpty(Convert.ToString(qparaopt[col]).Trim()))
                                                                {
                                                                    sbQuestionHtml.Append("<tr>");
                                                                    sbQuestionHtml.Append("<td align='left' style='text-align:left;'>");
                                                                    sbQuestionHtml.Append("[" + (char)(opno + 96) + "].  ");
                                                                    sbQuestionHtml.Append("</td>");
                                                                    sbQuestionHtml.Append("<td align='left' style='text-align:left;'>");
                                                                    sbQuestionHtml.Append(qparaopt[col]);
                                                                    sbQuestionHtml.Append("</td>");
                                                                    sbQuestionHtml.Append("</tr>");
                                                                    opno++;
                                                                }
                                                            }
                                                            sbQuestionHtml.Append("</table></td>");
                                                            sbQuestionHtml.Append("</tr>");
                                                        }
                                                        qno++;
                                                    }
                                                }
                                                sbQuestionHtml.Append("</table></td>");
                                                sbQuestionHtml.Append("</tr>");
                                            }
                                            break;
                                    }
                                }

                                if (questionType.Trim() != "6")
                                {
                                    GetMatches(((dsChoice.Tables.Count > 0) ? dsChoice.Tables[0] : dtOptions), questionPk, false, questionOptions, ref dtMatch, ref dtOptions);
                                    if (dtOptions.Rows.Count > 0 && needChoice)
                                    {
                                        sbQuestionHtml.Append("<tr>");
                                        sbQuestionHtml.Append("<td colspan='2'><table style='width:auto; margin-left:5%; margin-top:8px; margin-bottom:5px; padding:1px;'>");
                                        for (int options = 0; options < dtOptions.Rows.Count; options++)
                                        {
                                            if (options % 2 == 0)
                                            {
                                                sbQuestionHtml.Append("<tr>");
                                                if (options == dtOptions.Rows.Count - 1)
                                                {
                                                    sbQuestionHtml.Append("<td colspan='2'>");
                                                }
                                                else
                                                {
                                                    sbQuestionHtml.Append("<td>");
                                                }

                                                sbQuestionHtml.Append(Convert.ToString(dtOptions.Rows[options]["OptionNo"]).Trim() + " . " + Convert.ToString(dtOptions.Rows[options]["Option"]).Trim());
                                                sbQuestionHtml.Append("</td>");
                                                if (options == dtOptions.Rows.Count - 1)
                                                {
                                                    sbQuestionHtml.Append("</tr>");
                                                }
                                            }
                                            else
                                            {
                                                sbQuestionHtml.Append("<td>");
                                                sbQuestionHtml.Append(Convert.ToString(dtOptions.Rows[options]["OptionNo"]).Trim() + " . " + Convert.ToString(dtOptions.Rows[options]["Option"]).Trim());
                                                sbQuestionHtml.Append("</td>");
                                                sbQuestionHtml.Append("</tr>");
                                            }
                                        }
                                        sbQuestionHtml.Append("</table></td>");
                                        sbQuestionHtml.Append("</tr>");
                                    }
                                }
                            }
                            sbQuestionHtml.Append("</table>");
                        }
                    }
                }
                sbQuestionHtml.Append("</div >");
                questionPaper.InnerHtml = sbQuestionHtml.ToString();
                questionPaper.Visible = true;
                // Response.ContentType = "application/pdf";
                // Response.AddHeader("content-disposition", "attachment;filename=Passpercentageanalysis.pdf");
                // Response.Cache.SetCacheability(HttpCacheability.NoCache);
                // StringWriter sw = new StringWriter();
                // HtmlTextWriter hw = new HtmlTextWriter(sw);
                //it.Document pdfDoc = new it.Document(it.PageSize.A4, 10f, 10f, 5f, 0f);
                //itp.PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                // pdfDoc.Open();
                // questionPaper.RenderControl(hw);
                // StringReader sr = new StringReader(sw.ToString());
                // HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                // htmlparser.Parse(sr);                
                // Response.Write(pdfDoc);
                // Response.End();
                ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "PrintDiv();", true);
            }
        }
        catch (Exception ex)
        {
        }
    }

    public bool addImage(string path, ref string imageTag, int width = -1, int height = -1, int align = 0)
    {
        bool isSuccess = false;
        imageTag = string.Empty;
        try
        {
            if (!string.IsNullOrEmpty(path))
            {
                isSuccess = true;
                switch (align)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }
                imageTag = "<img " + ((width != -1) ? "width='" + width.ToString() + "px' " : "") + ((height != -1) ? "width='" + height.ToString() + "px'" : "") + " alt='' src='" + path + "'/>";
            }
            return isSuccess;
        }
        catch (Exception ex)
        {
            return false;
        }

    }

    public void GetMatches(DataTable dtChoice, string questionID, bool isMatchesOrChoice, string questionsOptions, ref DataTable dtMatches, ref DataTable dtOptions, string questionMatch = null)
    {
        try
        {
            dtMatches.Rows.Clear();
            dtMatches.Columns.Clear();
            dtMatches.Columns.Add("choiceID", typeof(string));
            dtMatches.Columns.Add("choiceNo", typeof(string));
            dtMatches.Columns.Add("LHS", typeof(string));
            dtMatches.Columns.Add("LHS_Image", typeof(byte[]));
            dtMatches.Columns.Add("AnswerSno", typeof(string));
            dtMatches.Columns.Add("RHS", typeof(string));
            dtMatches.Columns.Add("RHS_Image", typeof(byte[]));

            dtOptions.Columns.Clear();
            dtOptions.Rows.Clear();
            dtOptions.Columns.Add("OptionNo", typeof(string));
            dtOptions.Columns.Add("Option", typeof(string));
            bool haschoice = false;
            bool hasMatch = false;
            int autochar = 96;
            DataRow drMatch;
            if (dtChoice.Rows.Count > 0)
            {
                if (isMatchesOrChoice)
                {
                    dtChoice.DefaultView.RowFilter = "QuestionID='" + questionID.Trim() + "' and isMatching=" + isMatchesOrChoice;
                    DataView dvChoice = new DataView();
                    dvChoice = dtChoice.DefaultView;
                    if (dvChoice.Count > 0)
                    {
                        hasMatch = true;
                        for (int newmatch = 0; newmatch < dvChoice.Count; newmatch++)
                        {
                            drMatch = dtMatches.NewRow();
                            string choiceid = Convert.ToString(dvChoice[newmatch]["choiceID"]);
                            string lsno = Convert.ToString(dvChoice[newmatch]["choiceNo"]);
                            string lhs = Convert.ToString(dvChoice[newmatch]["LHS"]);
                            string lImage = Convert.ToString(dvChoice[newmatch]["LHS_Image"]);
                            string rsno = Convert.ToString(dvChoice[newmatch]["AnswerSno"]);
                            string rhs = Convert.ToString(dvChoice[newmatch]["RHS"]);
                            string RImage = Convert.ToString(dvChoice[newmatch]["RHS_Image"]);

                            drMatch["choiceID"] = choiceid;
                            drMatch["choiceNo"] = lsno;
                            drMatch["LHS"] = lhs;
                            drMatch["LHS_Image"] = (byte[])(dvChoice[newmatch]["LHS_Image"]);
                            drMatch["AnswerSno"] = rsno;
                            drMatch["RHS"] = rhs;
                            drMatch["RHS_Image"] = (byte[])(dvChoice[newmatch]["RHS_Image"]);
                            dtMatches.Rows.Add(drMatch);
                        }
                    }
                }
                else
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
            }
            if (isMatchesOrChoice)
            {
                if (!hasMatch)
                {
                    if (!string.IsNullOrEmpty(questionMatch))
                    {
                        if (questionMatch.Contains("^"))
                        {
                            if (questionMatch.Contains(';'))
                            {
                                string[] splitmatch = questionMatch.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);

                                int q_no = 0;
                                char alp = 'A';
                                for (int mch = 0; mch < splitmatch.Length; mch++)
                                {
                                    if (Convert.ToString(splitmatch[mch]).Contains(';'))
                                    {
                                        string[] split1 = splitmatch[mch].Split(';');
                                        if (Convert.ToString(split1[0]) != "" && Convert.ToString(split1[1]) != "")
                                        {
                                            string matchvalu1 = split1[0];
                                            string matchvalu2 = split1[1];

                                            drMatch = dtMatches.NewRow();
                                            string choiceid = Convert.ToString((q_no + 1));
                                            string lsno = Convert.ToString((q_no + 1));
                                            string lhs = Convert.ToString(matchvalu1);
                                            string rsno = Convert.ToString(alp);
                                            string rhs = Convert.ToString(matchvalu2);

                                            drMatch["choiceID"] = choiceid;
                                            drMatch["choiceNo"] = lsno;
                                            drMatch["LHS"] = lhs;
                                            drMatch["AnswerSno"] = rsno;
                                            drMatch["RHS"] = rhs;
                                            dtMatches.Rows.Add(drMatch);
                                            q_no++;
                                            alp++;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string[] splitmatch = questionMatch.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);
                                int q_no = 0;
                                for (int mch = 0; mch < splitmatch.Length; mch++)
                                {
                                    if (Convert.ToString(splitmatch[mch]) != "")
                                    {
                                        string matchvalu1 = splitmatch[mch];

                                        drMatch = dtMatches.NewRow();
                                        string choiceid = Convert.ToString((q_no + 1));
                                        string lsno = Convert.ToString((q_no + 1));
                                        string lhs = Convert.ToString(matchvalu1);
                                        drMatch["choiceID"] = choiceid;
                                        drMatch["choiceNo"] = lsno;
                                        drMatch["LHS"] = lhs;
                                        dtMatches.Rows.Add(drMatch);
                                        q_no++;
                                    }
                                }
                            }
                        }

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

}