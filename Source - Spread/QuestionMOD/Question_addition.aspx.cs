using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
//using it= iTextSharp.text;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using InsproDataAccess;
using System.Web;
using System.Configuration;

public partial class Question_addition : System.Web.UI.Page
{
    #region Field Declaration

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    Hashtable hat = new Hashtable();
    static Hashtable importHash = new Hashtable();
    bool isSchool = false;
    string qry = string.Empty;

    Dictionary<string, string> dicParam = new Dictionary<string, string>();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();

    public enum ObjectiveQuestionType
    {
        MCQ = 1,
        blanks = 2,
        Matches = 3,
        TrueFalse = 4,
        Rearange = 5,
        ParagraphWithOption = 6
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

    public enum QuestionLevel
    {
        E = 0,
        M = 1,
        D = 2,
        H = 3
    };

    public enum QuestionType
    {
        O = 0,
        D = 1
    };

    #endregion

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

            string grouporusercode1 = string.Empty;
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode1 = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + string.Empty;
            }
            else
            {
                grouporusercode1 = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + string.Empty;
            }

            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode1 + string.Empty;
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
                fQuesBankImport.Visible = false;
                help.Visible = false;
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                questiongrid.Visible = false;
                divMainQuestionMaster.Visible = false;
                divAddQmarks.Visible = false;
                divPopAlert.Visible = false;
                divWarning.Visible = false;
                txtQuestionName.Text = string.Empty;
                txtQMarks.Text = string.Empty;
                Add_questiontype.Visible = false;
                SetDefaultValuesOfQuestionMaster();

                Session["ParaGraphQuestions"] = null;

                txtNoofQuestionCount.Attributes.Add("max", "10");
                txtNoofQuestionCount.Attributes.Add("min", "0");

                if (rblObjectiveDescriptive.Items.Count > 0)
                {
                    rblObjectiveDescriptive.Items[0].Selected = true;
                    rblObjectiveDescriptive.Items[1].Selected = false;
                }
                chkAddQuesImage.Checked = false;
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
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Page Load

    #region Logout

    protected void lb3_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
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

    #region Bind Header

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

    protected void bindcollege()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindBatch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindDegree()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }

    }

    public void bindbranch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string course_id = Convert.ToString(ddldegree.SelectedValue.Trim());
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }

    }

    public void BindSectionDetail()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string strbatch = Convert.ToString(ddlbatch.SelectedValue).Trim();
            string strbranch = Convert.ToString(ddlbranch.SelectedValue).Trim();

            ddlsec.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSectionDetail(strbatch, strbranch);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataBind();
                if (Convert.ToString(ds.Tables[0].Columns["sections"]).Trim() == string.Empty)
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void bindsem()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string strbatchyear = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
            string strbranch = Convert.ToString(ddlbranch.SelectedValue).Trim();
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void GetSubject()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string subjectquery = string.Empty;
            ddlsubject.Items.Clear();
            string sections = string.Empty;
            string strsec = string.Empty;

            if (ddlsec.Items.Count > 0)
            {
                sections = Convert.ToString(ddlsec.SelectedValue).Trim();
                if (Convert.ToString(ddlsec.SelectedValue).Trim().ToLower() == "all" || Convert.ToString(ddlsec.SelectedValue).Trim() == "")
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
                        sems = " and SM.semester='" + Convert.ToString(ddlsem.SelectedValue).Trim() + "' ";
                    }

                    if (Convert.ToString(Session["Staff_Code"]).Trim() == "")
                    {
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "' order by S.subject_no ";
                    }
                    else if (Convert.ToString(Session["Staff_Code"]).Trim() != "")
                    {
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and st.subject_no=s.subject_no and s.syll_code=SM.syll_code and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.degree_code='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' " + Convert.ToString(sems) + " and  SM.batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "'  and sc.staff_code='" + Convert.ToString(Session["Staff_Code"]).Trim() + "' " + strsec + "  order by S.subject_no ";
                    }
                    if (subjectquery != "")
                    {
                        ds.Clear();
                        ds.Dispose();
                        ds.Reset();
                        ds = d2.select_method(subjectquery, hat, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
                else
                {
                    ddlsubject.SelectedIndex = 0;
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

    #region Header Change Events

    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            questiongrid.Visible = false;
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            questiongrid.Visible = false;
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            questiongrid.Visible = false;
            bindsem();
            BindSectionDetail();
            GetSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            questiongrid.Visible = false;
            bindbranch();
            bindsem();
            BindSectionDetail();
            GetSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            questiongrid.Visible = false;
            GetSubject();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            questiongrid.Visible = false;
            GetSubject();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            questiongrid.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }


    #endregion Header Change Events

    #region Go Click

    public void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            if (!chklst_general.Checked)
            {
                help.Visible = true;
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                questiongrid.Visible = false;
                Session["ParaGraphQuestions"] = null;
                ViewState["syllbuscode"] = string.Empty;
                ArrayList addnew = new ArrayList();
                DataTable dtq = new DataTable();
                dtq.Columns.Add("Sno");
                dtq.Columns.Add("Unit");
                dtq.Columns.Add("Topic_No");
                string unitname = string.Empty;
                string topicn = string.Empty;
                string subno = string.Empty;
                if (ddlsubject.Items.Count > 0)
                {
                    subno = Convert.ToString(ddlsubject.SelectedItem.Value).Trim();
                }
                else
                {
                    divPopErr.Visible = true;
                    lblErrMsg.Text = "No Were Subject Found";
                    return;
                }

                if (subno != "")
                {
                    string selqry = "select unit_name,topic_no  from sub_unit_details where subject_no='" + subno + "' order by topic_no,parent_code";
                    ds = d2.select_method_wo_parameter(selqry, "Text");
                }
                else
                {
                    divPopErr.Visible = true;
                    lblErrMsg.Text = "Please Select Subject";
                    return;
                }

                DataRow dr;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            unitname = Convert.ToString(ds.Tables[0].Rows[i]["unit_name"]).Trim();
                            topicn = Convert.ToString(ds.Tables[0].Rows[i]["topic_no"]).Trim();
                            dr = dtq.NewRow();
                            dr[0] = Convert.ToString(i + 1).Trim();
                            dr[1] = unitname;
                            dr[2] = topicn;
                            dtq.Rows.Add(dr);
                        }
                        if (dtq.Rows.Count > 0)
                        {
                            questiongrid.DataSource = dtq;
                            questiongrid.DataBind();
                            questiongrid.Visible = true;
                        }
                        else
                        {
                            divPopErr.Visible = true;
                            lblErrMsg.Text = "No Syllubus Were Found";
                            return;
                        }
                    }
                    else
                    {
                        divPopErr.Visible = true;
                        lblErrMsg.Text = "No Syllubus Were Found";
                        return;
                    }
                }
                else
                {
                    divPopErr.Visible = true;
                    lblErrMsg.Text = "No Subject Syllubus Were Found";
                    return;
                }
            }
            else
            {
                btn_addquestion_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Go Click

    #region Add Questions To Chapters

    protected void btn_addquestion_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ViewState["syllbuscode"] = string.Empty;
            Session["fuQuestionImage"] = null;

            divMainQuestionMaster.Visible = true;

            SetDefaultValuesOfQuestionMaster();

            objectiv.Visible = true;
            descript.Visible = false;
            rb_discript.Checked = false;
            rb_object.Checked = true;

            int rowindex = rowIndxClicked();
            if (questiongrid.Rows.Count > 0)
            {
                for (int i = 0; i < questiongrid.Rows.Count; i++)
                {
                    if (rowindex == i)
                    {
                        Label uniteval = (Label)questiongrid.Rows[i].Cells[1].FindControl("lbl_no");
                        string syllno = Convert.ToString(uniteval.Text);
                        ViewState["syllbuscode"] = syllno;
                        break;
                    }
                }
            }
            loaddesc();
            loadQuestionMarks();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }

    }

    protected void questiongrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes["Onchange"] = Page.ClientScript.GetPostBackEventReference(questiongrid, "Select$" + e.Row.RowIndex);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Add Questions To Chapters

    #region RBL Changed Events For Objective or Descriptive

    public void rb_object_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            objectiv.Visible = true;
            descript.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void rb_discript_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            objectiv.Visible = false;
            descript.Visible = true;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void rblObjectiveDescriptive_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            objectiv.Visible = false;
            descript.Visible = false;
            divObjective.Visible = false;
            divDescriptive.Visible = false;
            Session["ParaGraphQuestions"] = null;
            divParagraph.Visible = false;
            chkNeedOptions.Visible = true;
            chkNeedOptions.Checked = false;

            if (rblObjectiveDescriptive.Items.Count > 0)
            {
                if (rblObjectiveDescriptive.SelectedValue.Trim() == "0")
                {
                    SetDefaultValuesOfQuestionMaster();
                    divObjective.Visible = true;
                    divDescriptive.Visible = false;
                    rblObjectiveDescriptive.SelectedValue = "0";
                    chkNeedOptions.Visible = true;
                    chkNeedOptions.Checked = false;
                    if (rblQuestionType.Items.Count > 0)
                    {
                        rblQuestionType.SelectedValue = "1";
                        rblSingleorMutiChoice.Attributes.Add("style", "display:table-cell");
                        tdDescript.Attributes.Add("style", "display:none;");
                        if (rblSingleorMutiChoice.Items.Count > 0)
                        {
                            rblSingleorMutiChoice.SelectedValue = "1";
                        }
                    }
                }
                else if (rblObjectiveDescriptive.SelectedValue.Trim() == "1")
                {
                    SetDefaultValuesOfQuestionMaster();
                    divObjective.Visible = false;
                    divDescriptive.Visible = true;
                    tdDescript.Attributes.Add("style", "display:table-row;");
                    txtQuestionAnswer.Text = string.Empty;
                    rblObjectiveDescriptive.SelectedValue = "1";
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion RBL Changed Events For Objective or Descriptive

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

    public void cb_imagqstn_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            if (cb_imagqstn.Checked == true)
            {
                img_uplod.Visible = true;
            }
            else
            {
                img_uplod.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Add Image To Quetions

    #region Question Type Changed Event

    public void rblQuestionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            txtQuestionName.Text = string.Empty;
            txtQuestionAnswer.Text = string.Empty;
            rblSingleorMutiChoice.Attributes.Add("style", "display:none;");
            rblMatchSubType.Attributes.Add("style", "display:none;");
            txtNoofQuestionCount.Text = string.Empty;
            txtNoofQuestionCount.Visible = false;
            lblMQuestionCount.Visible = false;
            divMatchSubType.Visible = false;
            txtNoofOptionsCount.Text = string.Empty;
            lblNoofOptions.Visible = false;
            txtNoofOptionsCount.Visible = false;
            if (chkNeedOptions.Checked)
                txtNoofOptionsCount.Enabled = true;
            else
            {
                txtNoofOptionsCount.Enabled = false;
                txtNoofOptionsCount.Text = "1";
            }
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
            divOptions.Attributes.Add("style", "display:none;");
            if (rblQuestionType.Items.Count > 0)
            {
                if (rblQuestionType.SelectedValue.Trim() == "1")
                {
                    divSubType.Visible = true;
                    rblSingleorMutiChoice.Attributes.Add("style", "display:table-cell;");
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
                    divSubType.Visible = true;
                    rblMatchSubType.Attributes.Add("style", "display:table-cell;");
                    if (rblMatchSubType.Items.Count > 0)
                    {
                        rblMatchSubType.SelectedIndex = 0;
                    }

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
                        divOptions.Attributes.Add("style", "display:table-row;");
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
                if (!chkNeedOptions.Checked)
                    txtNoofOptionsCount_TextChanged(sender, e);

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

    public void cb_answer_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
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
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            string qustiontype = string.Empty;
            string questionname = string.Empty;
            bool isSuccess = false;

            questionname = Convert.ToString(txt_questionname.Text);
            questionname = Convert.ToString(questionname.Replace("'", "''"));
            questionname = Convert.ToString(questionname.Replace("‘", "''"));
            questionname = Convert.ToString(questionname.Replace("’", "''"));

            string type = string.Empty;
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

            string options = string.Empty;
            string answer = string.Empty;
            string matchthe_following = string.Empty;
            string m_creat_creat_qry = string.Empty;
            string mcreat_valu = string.Empty;

            if (ddlmark.Items.Count == 0)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Please Add Marks and Then Proceed";
                return;
            }

            if (rb_object.Checked == true)
            {
                if (txt_questionname.Text.Trim() == "")
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Please Enter Question Name";
                    return;
                }
                qustiontype = "0";
                for (int i = 0; i < gridView1.Rows.Count; i++)
                {
                    TextBox strhdname = (TextBox)gridView1.Rows[i].FindControl("txtOption");
                    if (strhdname.Text.Trim() != "")
                    {
                        string optn = Convert.ToString(strhdname.Text);
                        optn = Convert.ToString(optn.Replace("'", "''"));
                        options = options + optn + ";";
                    }
                    CheckBox anser = (CheckBox)gridView1.Rows[i].FindControl("cb_answer");
                    if (anser.Checked == true)
                    {
                        answer = Convert.ToString(strhdname.Text);
                    }
                }
                if (options == "")
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Please Enter No. of Options";
                    return;
                }

                if (cb_matchthefollowing.Checked == true)
                {
                    if (txt_qstcount.Text.Trim() == "")
                    {
                        divPopAlert.Visible = true;
                        lblAlertMsg.Text = "Please Enter Question Count";
                        return;
                    }
                    string m_value = string.Empty;
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
                    m_creat_creat_qry = " , is_matching, qmatching";
                    mcreat_valu = ",'1','" + m_value + "'";
                }
            }
            else if (rb_discript.Checked == true)
            {
                qustiontype = "1";
                answer = Convert.ToString(txt_answer.Text);
                answer = Convert.ToString(answer.Replace("'", "''"));
            }

            string sylubuscod = Convert.ToString(ViewState["syllbuscode"]).Trim();
            string subj_no = Convert.ToString(ddlsubject.SelectedItem.Value).Trim();
            string marks = Convert.ToString(ddlmark.SelectedItem.Text).Trim();

            if (marks.Trim() == "--Select--")
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Please Type Mark";
                return;
            }
            options = Convert.ToString(options.Replace("'", "''"));
            options = Convert.ToString(options.Replace("‘", "''"));
            options = Convert.ToString(options.Replace("’", "''"));

            if (questionname.Trim() != "")
            {
                string insertqry = "if exists (select * from tbl_question_master where subject_no='" + subj_no + "' and question =N'" + questionname + "' and syllabus='" + sylubuscod + "' ) update tbl_question_master set subject_no='" + subj_no + "' ,  syllabus='" + sylubuscod + "' ,  is_descriptive='" + qustiontype + "', mark='" + marks + "', question =N'" + questionname + "',  type='" + type + "' , options='" + options + "',answer='" + answer + "' " + matchthe_following + " where subject_no='" + subj_no + "' and question =N'" + questionname + "' and syllabus='" + sylubuscod + "'     else  insert into tbl_question_master ( subject_no, syllabus, is_descriptive, question , type,options,mark,answer " + m_creat_creat_qry + ") values('" + subj_no + "','" + sylubuscod + "' ,'" + qustiontype + "',N'" + questionname + "','" + type + "','" + options + "','" + marks + "','" + answer + "' " + mcreat_valu + ")";

                int insert = d2.update_method_wo_parameter(insertqry, "Text");
                if (insert != 0)
                {
                    int llogo = 1;
                    isSuccess = true;
                    if (img_uplod.HasFile == true)
                    {
                        string fileName = Path.GetFileName(img_uplod.PostedFile.FileName);
                        string file_type = string.Empty;
                        if (img_uplod.FileName.ToLower().EndsWith(".jpg") || img_uplod.FileName.ToLower().EndsWith(".gif") || img_uplod.FileName.ToLower().EndsWith(".png") || img_uplod.FileName.ToLower().EndsWith(".jpeg"))
                        {
                            int fileSize = img_uplod.PostedFile.ContentLength;
                            llogo = fileSize;
                            byte[] byteimage = new byte[fileSize];
                            img_uplod.PostedFile.InputStream.Read(byteimage, 0, fileSize);
                            file_type = Path.GetExtension(img_uplod.PostedFile.FileName);
                            file_type = file_type.ToLower();
                            file_type = GetImageFormat(file_type);
                            if (fileName != "" && fileSize != 0 && file_type != "" && !string.IsNullOrEmpty(file_type))
                            {
                                string qry = "if not exists ( select * from tbl_question_master where subject_no='" + subj_no + "' and question ='" + questionname + "' and syllabus='" + sylubuscod + "'  )  insert into tbl_question_master (file_name,quetion_image,file_type) values(@file_name,@quetion_image,@file_type) else update tbl_question_master set file_name=@file_name,file_type=@file_type,quetion_image=@quetion_image where subject_no='" + subj_no + "' and question ='" + questionname + "' and syllabus='" + sylubuscod + "'";

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
                                lblAlertMsg.Text = "Please Select Image Files Only";
                                divPopAlert.Visible = true;
                                return;
                            }
                        }
                        else
                        {
                            lblAlertMsg.Text = "Please Select .jpg,.gif,.png and .jpeg Image Files Only !!!";
                            divPopAlert.Visible = true;
                            return;
                        }
                    }

                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Saved Successfully";
                    txt_questionname.Text = string.Empty;
                    txt_nooption.Text = string.Empty;
                    txt_marks.Text = string.Empty;
                    txt_qstcount.Text = string.Empty;
                    rb_Easy.Checked = true;
                    rb_medium.Checked = false;
                    rb_difficult.Checked = false;
                    rb_hard.Checked = false;

                    for (int i = 0; i < gridView1.Rows.Count; i++)
                    {
                        (gridView1.Rows[i].FindControl("txtOption") as TextBox).Text = string.Empty;
                        (gridView1.Rows[i].FindControl("cb_answer") as CheckBox).Checked = false;
                    }
                    for (int i = 0; i < gridView2.Rows.Count; i++)
                    {
                        (gridView2.Rows[i].FindControl("txtqstn") as TextBox).Text = string.Empty;
                        (gridView2.Rows[i].FindControl("txt_answer") as TextBox).Text = string.Empty;
                    }
                    txt_answer.Text = string.Empty;
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Not Saved";
                    return;
                }
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Enter Question Name";
                return;
            }
            if (isSuccess)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Saved Successfully";
                return;
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Not Saved";
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
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
                lblAlertMsg.Text = "No Subjects Were Found";
                return;
            }
            else
            {
                subjectNo = Convert.ToString(ddlsubject.SelectedValue).Trim();
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
                lblAlertMsg.Text = "Please Add Marks and Then Proceed";
                return;
            }
            else
            {
                questionMarks = Convert.ToString(ddlQMarks.SelectedItem.Text).Trim();
                if (questionMarks == "" || questionMarks == "0" || Convert.ToString(ddlQMarks.SelectedItem.Value).Trim() == "0")
                {
                    divPopAlert.Visible = true;
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
                        lblAlertMsg.Text = errorms;
                        return;
                    }
                    else
                    {
                        quesHasImage = true;
                        questionImageFileName = Path.GetFileName(fuQuestionImage.PostedFile.FileName);
                        questionImageLenth = 0;
                        questionImageLenth = fuQuestionImage.PostedFile.ContentLength;
                        quesImage = new byte[questionImageLenth];
                        fuQuestionImage.PostedFile.InputStream.Read(quesImage, 0, questionImageLenth);
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
                                        lblAlertMsg.Text = "Please Enter The Options Name And Then Proceed";
                                        return;
                                    }
                                    else if (optCount != gvQOptions.Rows.Count)
                                    {
                                        divPopAlert.Visible = true;
                                        lblAlertMsg.Text = "Please Fill All The Options Name And Then Proceed";
                                        return;
                                    }
                                    if (optAnsCount == 0)
                                    {
                                        divPopAlert.Visible = true;
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
                                lblAlertMsg.Text = "Please Enter The No. of Questions And Then Proceed";
                                return;
                            }
                            if (string.IsNullOrEmpty(totalOptions.Trim()))
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Text = "Please Enter The No. of Questions And Then Proceed";
                                return;
                            }
                            else
                            {
                                questionPara = new string[totalQuestionCount];
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
                                    }
                                }
                                qmatchingName = string.Join("#Qpara#", questionPara);
                                newans = string.Join("#Qpara#", questionParaAns);
                            }
                            break;
                        case "6":
                            if (string.IsNullOrEmpty(totalQuestions.Trim()))
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Text = "Please Enter The No. of Questions And Then Proceed";
                                return;
                            }
                            if (string.IsNullOrEmpty(totalOptions.Trim()))
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Text = "Please Enter The No. of Questions And Then Proceed";
                                return;
                            }
                            else
                            {
                                questionPara = new string[totalQuestionCount];
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
                                        lblAlertMsg.Text = "Please Enter The Options Name And Then Proceed";
                                        return;
                                    }
                                    else if (optCount != gvQOptions.Rows.Count)
                                    {
                                        divPopAlert.Visible = true;
                                        lblAlertMsg.Text = "Please Fill All The Options Name And Then Proceed";
                                        return;
                                    }
                                    if (optAnsCount == 0)
                                    {
                                        divPopAlert.Visible = true;
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

            if (!string.IsNullOrEmpty(questionName.Trim()))
            {
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
                    if (!string.IsNullOrEmpty(questionID.Trim()))
                    {
                        qry = "delete from QuestionsChoice where QuestionID='" + questionID.Trim() + "'";
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
            SetDefaultValuesOfQuestionMaster();
            Session["fuQuestionImage"] = null;
            Session.Remove("fuQuestionImage");
            if (isSuccess)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Saved Successfully";
                return;
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Not Saved";
                return;
            }

        }
        catch (Exception ex)
        {
        }
    }

    #endregion Save Questions

    #region Popup Error Close

    protected void btnPopErrClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopErr.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

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
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Close Question Popup

    #region  Match The Following

    public void cb_matchthefollowing_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
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
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void txt_qstcount_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            addmatchs();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
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

    #endregion Match The Following

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

    public void btn_min_Click(object sender, EventArgs e)
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
            }
            else
            {

            }
            lblWarningMsgs.Text = "Are You Sure You Want Delete Question Mark " + QuestionMarks + "?";
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

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
                    lblAlertMsg.Text = "Please Select Any One Marks";
                    return;
                }
            }
            else
            {
                divWarning.Visible = false;
                divPopAlert.Visible = true;
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
                        lblAlertMsg.Text = "Saved Successfully";
                        txtAddQmark.Text = string.Empty;
                        divAddQmarks.Visible = false;
                    }
                    else
                    {
                        divPopAlert.Visible = true;
                        lblAlertMsg.Text = "Not Saved";
                        txtAddQmark.Text = string.Empty;
                        divAddQmarks.Visible = false;
                    }
                }
                loaddesc();
                loadQuestionMarks();
            }
            else
            {
                divPopAlert.Visible = true;
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

    public void btn_addmark_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            txt_mark.Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_mark.Text).Trim();
            string Qmark = txt_mark.Text.Trim();
            double qmarks = 0;
            bool isValidMark = double.TryParse(Qmark, out qmarks);
            if (txt_mark.Text.Trim() != "" && isValidMark)
            {
                if (Qmark.Trim() == "0")
                {
                    imgdiv3.Visible = true;
                    lbl_alert.Text = "Please Enter Mark Greater Than Zero";
                    return;
                }
                else
                {
                    string sql = "if exists ( select * from TextValTable where TextVal ='" + txt_mark.Text.Trim() + "' and TextCriteria ='QMark' and college_code ='" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "') update TextValTable set TextVal ='" + Convert.ToString(txt_mark.Text).Trim() + "' where TextVal ='" + txt_mark.Text.Trim() + "' and TextCriteria ='QMark' and college_code ='" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + txt_mark.Text.Trim() + "','QMark','" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "')";
                    int insert = d2.update_method_wo_parameter(sql, "Text");
                    if (insert != 0)
                    {
                        imgdiv3.Visible = true;
                        lbl_alert.Text = "Saved Successfully";
                        txt_mark.Text = string.Empty;
                        imgdiv5.Visible = false;
                    }
                    else
                    {
                        imgdiv3.Visible = true;
                        lbl_alert.Text = "Not Saved";
                        txt_mark.Text = string.Empty;
                        imgdiv5.Visible = false;
                    }
                }
                loaddesc();
                loadQuestionMarks();
            }
            else
            {
                imgdiv3.Visible = true;
                lbl_alert.Text = "Please Enter Mark";
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

    public void loaddesc()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddlmark.Items.Clear();
            ds.Tables.Clear();
            ds.Reset();
            ds.Dispose();
            ddlmark.Items.Insert(0, new ListItem("Select", "0"));

            string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='QMark' and college_code ='" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "' order by TextVal asc";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlmark.DataSource = ds;
                ddlmark.DataTextField = "TextVal";
                ddlmark.DataValueField = "TextCode";
                ddlmark.DataBind();
                ddlmark.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

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

    public void btn_exitmark_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            txt_mark.Text = string.Empty;
            imgdiv5.Visible = false;
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
                    lblAlertMsg.Text = "Please Select Any One Question Mark";
                }
                divWarning.Visible = false;
                string college = string.Empty;

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
                        lblAlertMsg.Text = "Deleted Successfully";
                    }
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "You Couldn't Delete The Mark " + Convert.ToString(ddlQMarks.SelectedItem.Text).Trim() + ".Becauze It is in Use.";
                }
                loaddesc();
                loadQuestionMarks();
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "No Marks Were Found";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void btn_warningmsgmark_exit_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lbl_warningmsghed.Text = string.Empty;
            imgdiv4.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void btn_warningmsmark_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            if (ddlmark.Items.Count > 0)
            {
                if (ddlmark.SelectedValue.Trim() == "0")
                {
                    imgdiv3.Visible = true;
                    lbl_alert.Text = "Please Select Any One Question Mark";
                }
                imgdiv4.Visible = false;
                string college = string.Empty;

                string sql = "delete from textvaltable where TextCode='" + Convert.ToString(ddlmark.SelectedItem.Value).Trim() + "' and TextCriteria='QMark' and college_code in ('" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "') ";
                int delete = d2.update_method_wo_parameter(sql, "Text");
                if (delete != 0)
                {
                    imgdiv3.Visible = true;
                    lbl_alert.Text = "Deleted Successfully";
                }
                loaddesc();
                loadQuestionMarks();
            }
            else
            {
                imgdiv3.Visible = true;
                lbl_alert.Text = "No Marks Were Found";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion

    #region No. of Option Changed

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
                        if (loadParagraph(rows, cols, dtPara))
                        {
                            divParagraph.Visible = true;
                        }
                        break;
                }
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
                txtNoofOptionsCount.Text = string.Empty;
                txtNoofOptionsCount.Enabled = true;
                divOptions.Attributes.Add("style", "display:none;");
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

    //Old
    public void Txt_nooption_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            AddNewRowToGrid1();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion No. of Option Changed

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
                tdDescript.Attributes.Add("style", "display:none;");
                divOptions.Attributes.Add("style", "display:none;");
                Session["ParaGraphQuestions"] = null;
                txtQuestionName.Text = string.Empty;
                chkAddQuesImage.Checked = false;
                fuQuestionImage.Visible = false;
                divParagraph.Visible = false;
                divSubType.Visible = false;
                txtNoofOptionsCount.Text = string.Empty;
                txtNoofOptionsCount.Enabled = true;
                chkNeedOptions.Checked = false;
                chkNeedOptions.Visible = true;

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
                        rblSingleorMutiChoice.Attributes.Add("style", "display:table-cell;");
                        divMatches.Visible = false;
                        divOptions.Attributes.Add("style", "display:none;");
                        rblMatchSubType.Attributes.Add("style", "display:none;");
                        divSubType.Visible = true;
                        txtNoofQuestionCount.Text = string.Empty;
                        lblMQuestionCount.Visible = false;
                        txtNoofQuestionCount.Visible = false;

                        chkNeedOptions.Checked = false;
                        chkNeedOptions.Visible = true;
                        txtNoofOptionsCount.Text = "1";
                        lblNoofOptions.Visible = true;
                        txtNoofOptionsCount.Visible = true;
                        txtNoofOptionsCount.Enabled = false;

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
                        if (!chkNeedOptions.Checked)
                        {
                            if (AddNewRowsToGrid1(gvQOptions, "1"))
                            {
                                divOptions.Visible = true;
                                divOptions.Attributes.Add("style", "display:table-row;");
                            }
                        }
                    }
                }
                else
                {
                    rblObjectiveDescriptive.SelectedValue = "1";
                    tdDescript.Attributes.Add("style", "display:table-row;");
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
                dt.Columns.Add("Option");
                dt.Columns.Add("Answer");
                dt.Columns.Add("AnswerSno");
                DataRow dr;
                int autochar = 65;
                for (int row = 0; row < newRows; row++)
                {
                    dr = dt.NewRow();
                    dr["Sno"] = Convert.ToString(row + 1);
                    dr["Option"] = "Option" + Convert.ToString(row + 1);
                    dr["Answer"] = "Answer" + Convert.ToString(row + 1);
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
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            if (txt_nooption.Text.Trim() != "")
            {
                int rowIndex = 0;
                int.TryParse(Convert.ToString(txt_nooption.Text).Trim(), out rowIndex);

                if (rowIndex > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Sno");
                    dt.Columns.Add("Option");
                    dt.Columns.Add("Answer");
                    DataRow dr;
                    for (int row = 0; row < rowIndex; row++)
                    {
                        dr = dt.NewRow();
                        dr[0] = Convert.ToString(row + 1);
                        dr[1] = "Option" + Convert.ToString(row + 1);
                        dr[2] = "Answer" + Convert.ToString(row + 1);
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
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void addmatchs()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            if (txt_qstcount.Text.Trim() != "")
            {
                int rowIndex = 0;
                int.TryParse(Convert.ToString(txt_qstcount.Text).Trim(), out rowIndex);

                if (rowIndex > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Sno");
                    dt.Columns.Add("Option");
                    dt.Columns.Add("Answer");
                    dt.Columns.Add("orderkey");

                    DataRow dr;
                    char alpa = 'A';
                    int autochar = 65;
                    for (int row = 0; row < rowIndex; row++)
                    {
                        dr = dt.NewRow();
                        dr[0] = Convert.ToString(row + 1);
                        dr[1] = "Option" + Convert.ToString(row + 1);
                        dr[2] = "Answer" + Convert.ToString(row + 1);
                        dr[3] = (char)(autochar);
                        alpa++;
                        autochar++;
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
                }
                else
                {
                    optionqstn.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Reusable Methods

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
                        btnRhsImage.Visible = true;
                        break;
                    case "5":
                        fuQMatch.Visible = true;
                        txtQAnswer.Visible = true;
                        btnLhsImage.Visible = true;
                        break;
                    case "6":
                        fuQMatch.Visible = true;
                        fuAnsMatch.Visible = true;
                        btnLhsImage.Visible = true;
                        btnRhsImage.Visible = true;
                        break;
                }
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

                    FileUpload fuQMatch = gvMatchQuestion.Rows[rowindex].
                        FindControl("fuLhsQMatch") as FileUpload;
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
                                imgLength = fuQMatch.PostedFile.ContentLength;
                                imagByte = new byte[imgLength];
                                fuQMatch.PostedFile.InputStream.Read(imagByte, 0, imgLength);

                                lblQuesName.Text = (imagByte != null) ? Convert.ToBase64String(imagByte) : string.Empty;
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
                            }
                            else
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Text = errorms;
                                return;
                            }
                        }
                    }
                    if (e.CommandName == "Rupload")
                    {
                        if (fuAnsMatch.HasFile)
                        {
                            string errorms = string.Empty;
                            if (CheckValidFiles(fuAnsMatch, out errorms))
                            {
                                imgName = Path.GetFileName(fuAnsMatch.PostedFile.FileName);
                                imgLength = 0;
                                imgLength = fuAnsMatch.PostedFile.ContentLength;
                                imagByte = new byte[imgLength];
                                fuAnsMatch.PostedFile.InputStream.Read(imagByte, 0, imgLength);
                                lblRhsFile.Text = (imagByte != null) ? Convert.ToBase64String(imagByte) : string.Empty;
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
                            }
                            else
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Text = errorms;
                                return;
                            }
                        }
                        else
                        {
                            divPopAlert.Visible = true;
                            lblAlertMsg.Text = "Please Choose Any Images And Then Proceed";
                            return;
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
                            txt.Width = 150;
                            e.Row.Cells[cell].Controls.Add(txt);
                        }
                        else if (cell == 2)
                        {
                            txt.ID = "txtParaAnswers" + e.Row.RowIndex;
                            ddl.ID = "ddlParaAnswers" + e.Row.RowIndex;
                            ddl.Visible = false;
                            e.Row.Cells[cell].Controls.Add(txt);
                            e.Row.Cells[cell].Controls.Add(ddl);
                        }
                        else
                        {
                            txt.ID = "txtParaOptions" + e.Row.RowIndex + (cell + 1);
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
    }

    #endregion Paragraph Grid

    /// <summary>
    /// Developed By Malang Raja T on Sep 19 2016 
    /// </summary>
    /// <param name="qry">sql Query to insert,update or delete operations</param>
    /// <param name="sqlpara">SqlParameter Array to add parameter</param>
    /// <param name="type">Integer Value: 0 Means Text Type; 1 or Other Means Stored Procedure</param>
    /// <returns>True or False True Means Success; False Means Failed to execute</returns>
    /// 
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

    public void btn_import_Click(object sender, EventArgs e)
    {
        //int row = questiongrid.SelectedRow.RowIndex;
        //lbl_header.Text = ((questiongrid.Rows[row].FindControl("lbl_unit") as Label).Text);
        int rowindex = rowIndxClicked();
        if (questiongrid.Rows.Count > 0)
        {
            for (int i = 0; i < questiongrid.Rows.Count; i++)
            {
                if (rowindex == i)
                {
                    lbl_header.Text = ((questiongrid.Rows[i].FindControl("lbl_unit") as Label).Text);
                    Label uniteval = (Label)questiongrid.Rows[i].Cells[1].FindControl("lbl_no");
                    string syllno = Convert.ToString(uniteval.Text);
                    ViewState["syllbuscode"] = syllno;
                    break;
                }
            }
        }
        Browsefile_div.Visible = true;
    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        Browsefile_div.Visible = false;
    }

    protected void btn_upload_click(object sender, EventArgs e)
    {
        try
        {
            lbl_alert1.Visible = false;
            using (Stream stream = this.FileUpload1.FileContent as Stream)
            {
                string extension = Path.GetFileName(FileUpload1.PostedFile.FileName);
                if (extension.Trim() != "")
                {
                    string moduletype = Convert.ToString(ViewState["moduletype"]);
                    importQuesBank();
                }
                else
                {
                    lbl_alert1.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            Browsefile_div.Visible = false;
            //lbl_alerterror.Visible = true;
            //lbl_alerterror.Text = ex.Message;
            //alertmessage.Visible = true;
        }
    }

    private bool checkImageType(string extension, byte[] imgByte, ref string errormsg, ref string fileType)
    {
        bool isValidFile = false;
        bool isExceedSize = false;
        errormsg = string.Empty;
        fileType = string.Empty;
        try
        {
            switch (extension)
            {
                case "jpg":
                    fileType = "jpg";
                    isValidFile = true;
                    break;
                case "gif":
                    fileType = "gif";
                    isValidFile = true;
                    break;
                case "jpeg":
                    fileType = "jpeg";
                    isValidFile = true;
                    break;
                case "png":
                    fileType = "png";
                    isValidFile = true;
                    break;
            }
            if (!isValidFile)
                errormsg = "It Allows .jpg,.jpeg,.png and .gif Image Format Only";
            if (imgByte.Length <= 0)
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
            else if (imgByte.Length > 2097152)
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
        }
        catch
        {
        }
        return isValidFile;
    }

    protected void importQuesBank()
    {
        try
        {
            string subjectNo = string.Empty;
            ViewState["topicNo"] = string.Empty;
            bool isSuccess = false;
            subjectNo = ddlsubject.SelectedValue.ToString();
            ViewState["topicNo"] = ViewState["syllbuscode"];
            Dictionary<string, byte> dicMandColumns = new Dictionary<string, byte>();
            fQuesBankImport.Visible = false;

            string[] quesOptions = new string[0];
            bool[] quesOptionAnwer = new bool[0];

            using (Stream stream = this.FileUpload1.FileContent as Stream)
            {
                if (System.IO.Path.GetExtension(FileUpload1.FileName) == ".xls" || System.IO.Path.GetExtension(FileUpload1.FileName) == ".xlsx")
                {
                    stream.Position = 0;
                    this.fQuesBankImport.OpenExcel(stream);
                    fQuesBankImport.OpenExcel(stream);
                    fQuesBankImport.SaveChanges();


                    if (fQuesBankImport.Sheets[0].Rows.Count > 0)
                    {
                        importHash.Clear();
                        importHash.Add("topic name", "");
                        importHash.Add("question name", "question");
                        importHash.Add("need image question", "");
                        importHash.Add("question type", "is_descriptive");
                        importHash.Add("question level", "type");
                        importHash.Add("qmark", "mark");
                        importHash.Add("answer", "answer");
                        importHash.Add("qimage path", "");
                        importHash.Add("qcategory", "QuestionType");
                        importHash.Add("qsubcategory", "QuestionSubType");
                        importHash.Add("need choice", "needChoice");
                        importHash.Add("no of choice", "totalChoice");
                        importHash.Add("choices", "options");
                        importHash.Add("no of questions", "choiceNo");
                        importHash.Add("choice answers", "isAnswer");
                        importHash.Add("matching left image", "QChoiceImage");
                        importHash.Add("matching right image", "QChoiceImageR");

                        //0 - Not Mandatory; 1 - Mandatory; 2 - By Default 
                        dicMandColumns.Add("topic name", 1);
                        dicMandColumns.Add("question name", 1);
                        dicMandColumns.Add("question type", 1);
                        dicMandColumns.Add("question level", 1);
                        dicMandColumns.Add("qmark", 1);
                        // dicMandColumns.Add("answer", 1);
                        //dicMandColumns.Add("need image question", 2);
                        //dicMandColumns.Add("", 0);
                        DataTable dtImportRecords = new DataTable();
                        //dtImportRecords = (DataTable)fQuesBankImport.Sheets[0].DataSource;
                        bool hasInvalidColumns = false;
                        bool hasMandatoryColumns = true;
                        string columnName = string.Empty;
                        string invalidColumnName = string.Empty;
                        int mandatoryColCount = 0;
                        for (int col = 0; col < fQuesBankImport.Sheets[0].Columns.Count; col++)
                        {
                            columnName = Convert.ToString(fQuesBankImport.Sheets[0].Cells[0, col].Text).Trim().ToLower();

                            if (!string.IsNullOrEmpty(columnName.Trim()))
                            {
                            }
                            if (dicMandColumns.ContainsKey(columnName.Trim()))
                            {
                                mandatoryColCount++;
                            }
                            if (!importHash.Contains(columnName))
                            {
                                hasInvalidColumns = true;
                                if (invalidColumnName == "")
                                {
                                    invalidColumnName = Convert.ToString(fQuesBankImport.Sheets[0].Cells[0, col].Text).Trim();
                                }
                                else
                                {
                                    invalidColumnName += "," + Convert.ToString(fQuesBankImport.Sheets[0].Cells[0, col].Text).Trim();
                                }
                            }
                        }
                        if (mandatoryColCount != dicMandColumns.Count)
                        {
                            hasMandatoryColumns = false;
                        }
                        if (!hasInvalidColumns && hasMandatoryColumns)
                        {
                            string topicName = string.Empty;
                            string topicNo = string.Empty;
                            string questionName = string.Empty;
                            string needImgQues = string.Empty;
                            string quesImgPath = string.Empty;
                            string quesType = string.Empty;
                            bool isMatching = false;
                            string quesLevel = string.Empty;
                            string quesMark = string.Empty;
                            string answer = string.Empty;
                            string questionImageName = string.Empty;
                            string qImageType = string.Empty;
                            byte[] qImageByte = new byte[0];
                            bool needChoice = false;
                            string qMatching = string.Empty;
                            string qChoices = string.Empty;
                            string QuestionType = string.Empty;
                            int questionType = 0;

                            string QuestionSubType = string.Empty;
                            int questionSubType = 0;

                            string matchLImage = string.Empty;
                            string matchRImage = string.Empty;

                            string notInsertedRow = string.Empty;

                            topicNo = ViewState["topicNo"].ToString();

                            for (int row = 1; row < fQuesBankImport.Sheets[0].Rows.Count; row++)
                            {
                                string topicNoExcel = string.Empty;
                                topicName = string.Empty;
                                topicNo = string.Empty;
                                questionName = string.Empty;
                                needImgQues = string.Empty;
                                quesImgPath = string.Empty;
                                quesType = string.Empty;
                                isMatching = false;
                                quesLevel = string.Empty;
                                int qLevel = 0;
                                quesMark = string.Empty;
                                answer = string.Empty;
                                questionImageName = string.Empty;
                                qImageType = string.Empty;
                                qImageByte = new byte[0];
                                needChoice = false;
                                int totalChoice = 1;
                                byte alreadyExists = 0;
                                qMatching = string.Empty;
                                qChoices = string.Empty;
                                bool qNeedImage = false;
                                bool hasImageQ = false;
                                topicNo = ViewState["topicNo"].ToString();
                                bool isObjective = false;
                                int noOfQues = 0;
                                string choiceAnswers = string.Empty;
                                matchLImage = string.Empty;
                                matchRImage = string.Empty;
                                quesOptions = new string[0];
                                quesOptionAnwer = new bool[0];
                                string[] quesAnswers = new string[0];

                                for (int col = 0; col < fQuesBankImport.Sheets[0].Columns.Count; col++)
                                {
                                    string excelcolname = fQuesBankImport.Sheets[0].Cells[0, col].Text.Trim().ToLower();
                                    string dbtablecolname = Convert.ToString(importHash[excelcolname]);
                                    string Values = Convert.ToString(fQuesBankImport.Sheets[0].Cells[row, col].Text).Trim();
                                    bool isAvail = false;
                                    if (Values == "")
                                    {
                                        isAvail = false;
                                    }
                                    else
                                    {
                                        isAvail = true;
                                    }
                                    switch (excelcolname)
                                    {
                                        case "topic name":
                                            topicNoExcel = getTopicNo(subjectNo, Values);
                                            topicName = Values;
                                            break;
                                        case "question name":
                                            questionName = Values;
                                            break;
                                        case "need image question":
                                            if (Values.ToLower() == "yes")
                                                needImgQues = "true";
                                            else
                                                needImgQues = "false";
                                            bool.TryParse(needImgQues, out qNeedImage);
                                            break;
                                        case "question type":
                                            if (Values.ToLower() == "o")
                                            {
                                                quesType = "0";
                                                isObjective = true;
                                            }
                                            else
                                            {
                                                quesType = "1";
                                                isObjective = false;
                                            }
                                            break;
                                        case "question level":
                                            if (Values.ToLower() == "e")
                                                quesLevel = "0";
                                            else if (Values.ToLower() == "m")
                                                quesLevel = "1";
                                            else if (Values.ToLower() == "d")
                                                quesLevel = "2";
                                            else if (Values.ToLower() == "h")
                                                quesLevel = "3";
                                            int.TryParse(quesLevel, out qLevel);
                                            break;
                                        case "qmark":
                                            quesMark = Values;
                                            double qMark = 0;
                                            if (!string.IsNullOrEmpty(quesMark) && double.TryParse(quesMark, out qMark))
                                            {
                                                string sql = "if exists ( select * from TextValTable where TextVal ='" + quesMark + "' and TextCriteria ='QMark' and college_code ='" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "') update TextValTable set TextVal ='" + Convert.ToString(quesMark).Trim() + "' where TextVal ='" + quesMark + "' and TextCriteria ='QMark' and college_code ='" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + quesMark + "','QMark','" + Convert.ToString(ddl_collegename.SelectedItem.Value).Trim() + "')";
                                            }
                                            else
                                            {
                                                quesMark = "0";
                                            }
                                            break;
                                        case "answer":
                                            answer = Values;
                                            string[] answerList = new string[0];
                                            if (answer.Contains("#ans#"))
                                            {
                                                answerList = answer.Split(new string[] { "#ans#" }, StringSplitOptions.RemoveEmptyEntries);
                                            }
                                            else if (answer.Contains("##"))
                                            {
                                                answerList = answer.Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
                                            }
                                            else
                                            {
                                                Array.Resize(ref answerList, answerList.Length + 1);
                                                answerList[0] = answer;
                                            }
                                            quesAnswers = answerList;
                                            break;
                                        case "qimage path":
                                            quesImgPath = Values;
                                            questionImageName = string.Empty;
                                            qImageType = string.Empty;
                                            qImageByte = new byte[0];
                                            break;
                                        case "qcategory":
                                            //questionType = Convert.ToInt32(Values);
                                            int.TryParse(Values, out questionType);
                                            break;
                                        case "qsubcategory":
                                            //questionSubType = Convert.ToInt32(Values);
                                            if (questionType == 2 || questionType == 4)
                                                questionSubType = 1;
                                            else
                                                int.TryParse(Values, out questionSubType);
                                            break;
                                        case "need choice":
                                            if (questionType == 2 || questionType == 4)
                                                needChoice = true;
                                            else
                                                needChoice = ((Values.ToLower() == "yes") ? true : false);
                                            break;
                                        case "no of choice":
                                            //totalChoice = Convert.ToInt32(Values);
                                            int.TryParse(Values, out totalChoice);
                                            totalChoice = (totalChoice <= 0) ? 1 : totalChoice;
                                            break;
                                        case "choices":
                                            qChoices = Values;
                                            string[] str = new string[0];
                                            if (qChoices.Contains("#malang#"))
                                            {
                                                str = qChoices.Split(new string[] { "#malang#" }, StringSplitOptions.RemoveEmptyEntries);
                                            }
                                            else if (qChoices.Contains("#opt#"))
                                            {
                                                str = qChoices.Split(new string[] { "#opt#" }, StringSplitOptions.RemoveEmptyEntries);
                                            }
                                            else if (qChoices.Contains("##"))
                                            {
                                                str = qChoices.Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
                                            }
                                            else
                                            {
                                                Array.Resize(ref str, str.Length + 1);
                                                str[0] = qChoices;
                                            }
                                            quesOptions = str;
                                            Array.Resize(ref quesOptionAnwer, quesOptions.Length);
                                            break;
                                        case "no of questions":
                                            //noOfQues = Convert.ToInt32(Values);
                                            int.TryParse(Values, out noOfQues);
                                            break;
                                        case "choice answers":
                                            choiceAnswers = Values;
                                            string[] str1 = new string[0];
                                            if (choiceAnswers.Contains("#malang#"))
                                            {
                                                str1 = choiceAnswers.Split(new string[] { "#malang#" }, StringSplitOptions.RemoveEmptyEntries);
                                            }
                                            else if (choiceAnswers.Contains("#ans#"))
                                            {
                                                str1 = choiceAnswers.Split(new string[] { "#ans#" }, StringSplitOptions.RemoveEmptyEntries);
                                            }
                                            else if (choiceAnswers.Contains("##"))
                                            {
                                                str1 = choiceAnswers.Split(new string[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
                                            }
                                            else
                                            {
                                                //str1 = choiceAnswers;
                                                Array.Resize(ref str1, str1.Length + 1);
                                                str1[0] = choiceAnswers;
                                            }
                                            //foreach (string ans in str1)
                                            //{
                                            //    bool val = false;
                                            //    //bool.TryParse(ans, out val);
                                            //    Array.Resize(ref quesOptionAnwer, quesOptionAnwer.Length + 1);
                                            //    quesOptionAnwer[quesOptionAnwer.Length - 1] = val;
                                            //}
                                            //quesOptionAnwer = str1;
                                            break;
                                        case "matching left image":
                                            matchLImage = Values;
                                            break;
                                        case "matching right image":
                                            matchRImage = Values;
                                            break;

                                    }
                                }
                                if (quesOptions.Length > 0)
                                {
                                    int start = 0;
                                    foreach (string opt in quesOptions)
                                    {
                                        foreach (string ans in quesAnswers)
                                        {
                                            if (quesOptionAnwer.Length > start)
                                            {
                                                if (!quesOptionAnwer[start])
                                                {
                                                    quesOptionAnwer[start] = false;
                                                    if (ans.Trim().ToLower() == opt.Trim().ToLower())
                                                        quesOptionAnwer[start] = true;
                                                }
                                            }
                                        }
                                        start++;
                                    }
                                }
                                if (qNeedImage && !string.IsNullOrEmpty(quesImgPath))
                                {
                                    if (File.Exists(quesImgPath))
                                    {
                                        //FileStream fs = new FileStream(quesImgPath, FileMode.Open, FileAccess.Read);
                                        //BinaryReader br = new BinaryReader(fs);
                                        //Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                        //br.Close();
                                        //fs.Close();
                                        byte[] bytes = new byte[0];
                                        FileInfo fInfo = new FileInfo(quesImgPath);
                                        string[] fileExtend = fInfo.Name.Split('.');
                                        string errormsg = string.Empty;
                                        string fileType = string.Empty;
                                        using (var stream1 = new FileStream(quesImgPath, FileMode.Open, FileAccess.Read))
                                        {
                                            using (var reader = new BinaryReader(stream1))
                                            {
                                                bytes = reader.ReadBytes((int)stream.Length);
                                            }
                                        }
                                        if (checkImageType((fileExtend.Length > 0 ? fileExtend[fileExtend.Length - 1] : ""), bytes, ref errormsg, ref fileType))
                                        {
                                            questionImageName = fInfo.Name;
                                            qImageType = "images/" + fileType;
                                            qImageByte = bytes;
                                        }
                                        //string errorms = string.Empty;
                                        //if (!CheckValidFiles(fuQuestionImage, out errorms))
                                        //{
                                        //    divPopAlert.Visible = true;
                                        //    lblAlertMsg.Text = errorms;
                                        //    return;
                                        //}
                                        //else
                                        //{
                                        //    quesHasImage = true;
                                        //    questionImageFileName = Path.GetFileName(fuQuestionImage.PostedFile.FileName);
                                        //    questionImageLenth = 0;
                                        //    questionImageLenth = fuQuestionImage.PostedFile.ContentLength;
                                        //    quesImage = new byte[questionImageLenth];
                                        //    fuQuestionImage.PostedFile.InputStream.Read(quesImage, 0, questionImageLenth);
                                        //    questionImageType = Path.GetExtension(fuQuestionImage.PostedFile.FileName);
                                        //    questionImageType = questionImageType.ToLower();
                                        //    questionImageType = GetImageFormat(questionImageType);
                                        //    if (questionImageFileName != "" && questionImageLenth != 0 && questionImageType != "" && !string.IsNullOrEmpty(questionImageType))
                                        //    {
                                        //    }
                                        //}
                                    }
                                    //else
                                    //{
                                    //    divPopAlert.Visible = true;
                                    //    lblAlertMsg.Text = "Please Choose Any Images And Then Proceed";
                                    //    return;
                                    //}
                                }

                                string ifQuesExists = d2.GetFunction("select * from tbl_question_master where subject_no='" + subjectNo + "' and question ='" + questionName.Trim() + "' and syllabus='" + topicNo + "'  and is_descriptive='" + quesType + "'");
                                if (string.IsNullOrEmpty(ifQuesExists) || ifQuesExists == "0" && topicNoExcel != "")
                                {
                                    if (!string.IsNullOrEmpty(questionName.Trim()) && !string.IsNullOrEmpty(subjectNo) && !string.IsNullOrEmpty(topicNo) && topicNoExcel == topicNo)
                                    {
                                        qry = "if exists (select * from tbl_question_master where subject_no=@subject_no and question =@question and syllabus=@syllabus  and is_descriptive=@is_descriptive ) update tbl_question_master set subject_no=@subject_no,  syllabus=@syllabus ,  is_descriptive=@is_descriptive, mark=@mark, question =@question,  type=@type, options=@options, answer=@answer, QuestionType=@QuestionType,QuestionSubType=@QuestionSubType,file_name=@file_name,file_type=@file_type,quetion_image=@quetion_image , totalChoice=@totalChoice,is_matching=@is_matching, qmatching=@qmatching, needChoice=@needChoice  where subject_no=@subject_no and question =@question and syllabus=@syllabus and is_descriptive=@is_descriptive else  insert into tbl_question_master ( subject_no, syllabus, is_descriptive, question , type,options,mark,answer, QuestionType ,QuestionSubType ,totalChoice ,is_matching ,qmatching, Already_exist, needChoice,file_name,file_type,quetion_image) values(@subject_no, @syllabus, @is_descriptive, @question , @type,@options,@mark, @answer,@QuestionType,@QuestionSubType,@totalChoice, @is_matching, @qmatching ,@Already_exist ,@needChoice,@file_name,@file_type,@quetion_image)";

                                        SqlParameter[] sqlpara = new SqlParameter[18];

                                        sqlpara[0] = new SqlParameter("@subject_no", SqlDbType.Int, 300);
                                        sqlpara[0].Value = subjectNo;

                                        sqlpara[1] = new SqlParameter("@syllabus", SqlDbType.NVarChar, 300);
                                        sqlpara[1].Value = topicNo;

                                        sqlpara[2] = new SqlParameter("@mark", SqlDbType.Real, 300);
                                        sqlpara[2].Value = quesMark;

                                        sqlpara[3] = new SqlParameter("@is_descriptive", SqlDbType.Int, 300);
                                        sqlpara[3].Value = quesType;

                                        sqlpara[4] = new SqlParameter("@question", SqlDbType.NVarChar, -1);
                                        sqlpara[4].Value = questionName.Trim();

                                        sqlpara[5] = new SqlParameter("@answer", SqlDbType.NVarChar, -1);
                                        sqlpara[5].Value = answer.Trim();

                                        sqlpara[6] = new SqlParameter("@type", SqlDbType.Int, 300);
                                        sqlpara[6].Value = qLevel;

                                        sqlpara[7] = new SqlParameter("@Already_exist", SqlDbType.TinyInt, 50);
                                        sqlpara[7].Value = alreadyExists;

                                        sqlpara[8] = new SqlParameter("@file_name", SqlDbType.NVarChar, 300);
                                        sqlpara[8].Value = questionImageName;

                                        sqlpara[9] = new SqlParameter("@file_type", SqlDbType.NVarChar, 300);
                                        sqlpara[9].Value = qImageType;

                                        sqlpara[10] = new SqlParameter("@quetion_image", SqlDbType.Image, 300);
                                        sqlpara[10].Value = qImageByte;

                                        sqlpara[11] = new SqlParameter("@is_matching", SqlDbType.Bit);
                                        sqlpara[11].Value = isMatching;

                                        sqlpara[12] = new SqlParameter("@qmatching", SqlDbType.NVarChar, -1);
                                        sqlpara[12].Value = qMatching;

                                        sqlpara[13] = new SqlParameter("@QuestionType", SqlDbType.TinyInt, 50);
                                        sqlpara[13].Value = questionType;

                                        sqlpara[14] = new SqlParameter("@QuestionSubType", SqlDbType.TinyInt, 50);
                                        sqlpara[14].Value = questionSubType;

                                        sqlpara[15] = new SqlParameter("@needChoice", SqlDbType.Bit);
                                        if (isObjective == false)
                                            needChoice = false;
                                        //else
                                        //    sqlpara[15].Value = needChoice;
                                        sqlpara[15].Value = needChoice;

                                        sqlpara[16] = new SqlParameter("@totalChoice", SqlDbType.Int, 100);
                                        sqlpara[16].Value = ((totalChoice <= 0) ? 1 : totalChoice);

                                        sqlpara[17] = new SqlParameter("@options", SqlDbType.NVarChar, -1);
                                        sqlpara[17].Value = (questionSubType == 1 && quesOptions.Length == 0) ? answer.Trim() + "#malang#" : (quesOptions.Length == 1) ? string.Join("#malang#", quesOptions) + "#malang#" : string.Join("#malang#", quesOptions);

                                        isSuccess = InsertImageQuery(qry, sqlpara);

                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(questionName.Trim()))
                                        {
                                            if ((lbl_header.Text.Trim()) == topicName)
                                            {
                                                fQuesBankImport.Visible = false;
                                                lbl_alerterror.Visible = true;
                                                lbl_alerterror.Text = "Question Name is mandatory";
                                                alertmessage.Visible = true;
                                                Browsefile_div.Visible = false;
                                                return;
                                            }

                                        }
                                        else if (topicNoExcel != topicNo)
                                        {

                                            fQuesBankImport.Visible = false;
                                            lbl_alerterror.Visible = true;
                                            lbl_alerterror.Text = "Topic name mismatched";
                                            alertmessage.Visible = true;
                                            Browsefile_div.Visible = false;


                                        }
                                    }

                                    if (isObjective == true && (!isSuccess == false))
                                    {
                                        string questionID = d2.GetFunctionv("select QuestionMasterPK from tbl_question_master where subject_no='" + subjectNo + "' and question =N'" + questionName.Trim() + "' and syllabus='" + topicNo + "' and is_descriptive='" + quesType + "'");

                                        if (!string.IsNullOrEmpty(questionID.Trim()))
                                        {
                                            qry = "delete from QuestionsChoice where QuestionID='" + questionID.Trim() + "'";
                                            int del = d2.update_method_wo_parameter(qry, "Text");
                                            for (int opt = 0; opt < quesOptions.Length; opt++)
                                            {
                                                qry = "if exists (select * from QuestionsChoice where QuestionID=@QuestionID and choiceNo=@choiceNo) update QuestionsChoice set QChoice=@QChoice,isAnswer=@isAnswer,QChoiceImage='',QMatchR='',QChoiceImageR='',isMatching=@isMatching where QuestionID=@QuestionID and choiceNo=@choiceNo else insert into QuestionsChoice (QuestionID,choiceNo,QChoice,QChoiceImage,isAnswer,QMatchR,QChoiceImageR,isMatching)  values(@QuestionID,@choiceNo,@QChoice,'',@isAnswer,'','',@isMatching)";

                                                SqlParameter[] sqlpara = new SqlParameter[5];

                                                sqlpara[0] = new SqlParameter("@QuestionID", SqlDbType.NVarChar, 300);
                                                sqlpara[0].Value = questionID.Trim();

                                                sqlpara[1] = new SqlParameter("@choiceNo", SqlDbType.NVarChar, 300);
                                                sqlpara[1].Value = Convert.ToString((opt + 1)).Trim();

                                                sqlpara[2] = new SqlParameter("@QChoice", SqlDbType.NVarChar, 300);
                                                sqlpara[2].Value = quesOptions[opt].Trim();

                                                //sqlpara[3] = new SqlParameter("@QMatchR", SqlDbType.NVarChar, 300);
                                                //sqlpara[3].Value = quesMatchRight[opt].Trim();

                                                //byte[] leftImage = new byte[0];
                                                //int leftLength = 0;
                                                //string leftImageName = string.Empty;
                                                //string LeftImageType = string.Empty;

                                                //byte[] rightImage = new byte[0];
                                                //int rightLength = 0;
                                                //string rightImageType = string.Empty;
                                                //string rightImageName = string.Empty;
                                                //if (isMatching)
                                                //{
                                                //    if (gvMatchQuestion.Rows.Count > 0)
                                                //    {
                                                //        if (opt < gvMatchQuestion.Rows.Count)
                                                //        {
                                                //            FileUpload fuQMatch = gvMatchQuestion.Rows[opt].FindControl("fuLhsQMatch") as FileUpload;
                                                //            FileUpload fuAnsMatch = gvMatchQuestion.Rows[opt].FindControl("fuRhsAMatch") as FileUpload;
                                                //            Label lblQuesName = gvMatchQuestion.Rows[opt].FindControl("lblMatchQuestions") as Label;
                                                //            Label lblRhsFile = gvMatchQuestion.Rows[opt].FindControl("lblRhsFile") as Label;

                                                //            System.Web.UI.WebControls.Image imgQMatch = gvMatchQuestion.Rows[opt].FindControl("imgLhsQMatch") as System.Web.UI.WebControls.Image;
                                                //            System.Web.UI.WebControls.Image imgAnsMatch = gvMatchQuestion.Rows[opt].FindControl("imgRhsAMatch") as System.Web.UI.WebControls.Image;

                                                //            string errorms = string.Empty;
                                                //            if (fuQMatch.HasFile)
                                                //            {
                                                //                if (CheckValidFiles(fuQMatch, out errorms))
                                                //                {
                                                //                    leftImageName = Path.GetFileName(fuQMatch.PostedFile.FileName);
                                                //                    leftLength = 0;
                                                //                    leftLength = fuQMatch.PostedFile.ContentLength;
                                                //                    leftImage = new byte[leftLength];
                                                //                    fuQMatch.PostedFile.InputStream.Read(leftImage, 0, leftLength);

                                                //                    LeftImageType = Path.GetExtension(fuQMatch.PostedFile.FileName);
                                                //                    LeftImageType = LeftImageType.ToLower();
                                                //                    LeftImageType = GetImageFormat(LeftImageType);
                                                //                    if (leftImageName != "" && leftLength != 0 && LeftImageType != "" && !string.IsNullOrEmpty(LeftImageType))
                                                //                    {
                                                //                    }
                                                //                }
                                                //            }
                                                //            else if (!string.IsNullOrEmpty(lblQuesName.Text))
                                                //            {
                                                //                leftImage = Convert.FromBase64String(lblQuesName.Text);
                                                //            }
                                                //            if (fuAnsMatch.HasFile)
                                                //            {
                                                //                if (CheckValidFiles(fuAnsMatch, out errorms))
                                                //                {
                                                //                    rightImageName = Path.GetFileName(fuAnsMatch.PostedFile.FileName);
                                                //                    rightLength = 0;
                                                //                    rightLength = fuAnsMatch.PostedFile.ContentLength;
                                                //                    rightImage = new byte[rightLength];
                                                //                    fuAnsMatch.PostedFile.InputStream.Read(rightImage, 0, rightLength);

                                                //                    rightImageType = Path.GetExtension(fuAnsMatch.PostedFile.FileName);
                                                //                    rightImageType = rightImageType.ToLower();
                                                //                    rightImageType = GetImageFormat(rightImageType);
                                                //                    if (rightImageName != "" && rightLength != 0 && rightImageType != "" && !string.IsNullOrEmpty(rightImageType))
                                                //                    {
                                                //                    }
                                                //                }
                                                //            }
                                                //            else if (!string.IsNullOrEmpty(lblRhsFile.Text))
                                                //            {
                                                //                rightImage = Convert.FromBase64String(lblRhsFile.Text);
                                                //            }
                                                //        }
                                                //    }
                                                //}

                                                //sqlpara[4] = new SqlParameter("@QChoiceImage", SqlDbType.Image, leftImage.Length);
                                                //sqlpara[4].Value = leftImage;

                                                //sqlpara[5] = new SqlParameter("@QChoiceImageR", SqlDbType.Image, rightImage.Length);
                                                //sqlpara[5].Value = rightImage;

                                                sqlpara[3] = new SqlParameter("@isAnswer", SqlDbType.NVarChar, 300);
                                                sqlpara[3].Value = (quesOptionAnwer.Length > opt) ? quesOptionAnwer[opt] : false;

                                                sqlpara[4] = new SqlParameter("@isMatching", SqlDbType.NVarChar, 300);
                                                sqlpara[4].Value = isMatching;

                                                isSuccess = InsertImageQuery(qry, sqlpara);

                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    if ((lbl_header.Text.Trim()) == topicName)
                                    {
                                        if (notInsertedRow == "")
                                        {
                                            notInsertedRow = "Excel row no." + (row + 1) + " : Question already exists!!";
                                        }
                                        else
                                        {
                                            notInsertedRow += " \n" + "Excel row no." + (row + 1) + " : Question already exists!!";
                                        }

                                    }
                                    if (topicNoExcel == "")
                                    {
                                        notInsertedRow += " \n" + "Excel row no." + (row + 1) + " : Topic name is mandatory!!";
                                    }
                                }
                            }
                            fQuesBankImport.Visible = false;
                            lbl_alerterror.Visible = true;
                            lbl_alerterror.Text = ((isSuccess) ? ((lbl_header.Text.Trim()) + " imported successfully ") : "Failed to import : Either " + ((lbl_header.Text.Trim()) + " does not exist in excel or topic name mismatched"));

                            Browsefile_div.Visible = false;
                            if (!string.IsNullOrEmpty(notInsertedRow))
                            {
                                lbl_cannotsave.Text = "Not inserted rows ";
                                txt_cannotinsert.Text = notInsertedRow;
                                cannot_insert_div.Visible = true;
                            }
                            alertmessage.Visible = true;
                        }
                        else
                        {
                            fQuesBankImport.Visible = false;
                            lbl_alerterror.Visible = true;
                            lbl_alerterror.Text = "File has invalid columns or empty mandatory columns";
                            alertmessage.Visible = true;
                        }
                    }
                    else
                    {
                        fQuesBankImport.Visible = false;
                        Browsefile_div.Visible = false;
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Please Browse Import File";
                        alertmessage.Visible = true;
                    }

                }
                else
                {
                    fQuesBankImport.Visible = false;
                    Browsefile_div.Visible = false;
                    lbl_alerterror.Visible = true;
                    lbl_alerterror.Text = "Please Import Only .xls Format";
                    alertmessage.Visible = true;
                }
                //fQuesBankImport.Visible = false;
                //lbl_alerterror.Visible = true;
                //lbl_alerterror.Text = ((isSuccess) ? "Imported Successfully" : "Not Imported");
                //alertmessage.Visible = true;
                //Browsefile_div.Visible = false;
            }
        }
        catch (Exception e)
        {
            fQuesBankImport.Visible = false;
        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertmessage.Visible = false;
    }

    public string getTopicNo(string subjectNo, string unitName)
    {
        string topicNo = string.Empty;
        try
        {
            string qry = "select topic_no  from sub_unit_details where subject_no='" + subjectNo + "' and unit_name='" + unitName + "' order by topic_no";
            topicNo = dirAcc.selectScalarString(qry);
        }
        catch
        {
        }
        return topicNo;
    }

    protected void help_click(object sender, EventArgs e)
    {
        string filename = "HelpQuestion_excel";
        downloadhelp_excel(filename);

    }

    protected void download_click(object sender, EventArgs e)
    {
        string filename = "DownloadQuestionSample";
        downloadhelp_excel(filename);
    }

    protected void downloadhelp_excel(string filename)
    {
        try
        {
            string print = "";
            string appPath = HttpContext.Current.Server.MapPath("~");
            string strexcelname = "";
            if (appPath != "")
            {
                strexcelname = filename;
                appPath = appPath.Replace("\\", "/");
                if (strexcelname != "")
                {
                    print = strexcelname;
                    string szPath = appPath + "/Importhelp/";
                    string szFile = print + ".xls";
                    System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    System.Web.HttpContext.Current.Response.Flush();
                    System.Web.HttpContext.Current.Response.WriteFile(szPath + szFile);
                }
            }
        }
        catch (Exception ex)
        {
            Browsefile_div.Visible = false;
            lbl_alerterror.Visible = true;
            lbl_alerterror.Text = ex.Message;
            alertmessage.Visible = true;
        }
    }

    public void btn_Exit_Click1(object sender, EventArgs e)
    {
        cannot_insert_div.Visible = false;
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
                download.Visible = false;
                help.Visible = false;
                questiongrid.Visible = false;
            }
            else
            {
                lblsubject.Visible = true;
                ddlsubject.Visible = true;
                download.Visible = true;
            }

        }
        catch
        {


        }


    }

}