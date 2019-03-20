using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Draw = System.Drawing;
using Farpoint = FarPoint.Web.Spread;
using System.Configuration;

public partial class Individual_Students_Performance_Results_Analysis_Report : System.Web.UI.Page
{
   
    #region Field Declaration

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

    string qry = string.Empty;
    string qrysec = string.Empty;

    bool isSchool = false;

    GridView[] gvStudentsSubjects = new GridView[0];
    Chart[] chartPerformance = new Chart[0];
    DataTable[] dtAllStudMarks = new DataTable[0];
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    private bool isInternal;

    #endregion Field Declaration

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
        if (string.IsNullOrEmpty(contextKey))
        {
        }
        else if (contextKey != "0" && contextKey.Trim() != "" && contextKey != null)
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
                //rptprint1.Visible = false;
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                popupdiv.Visible = false;
                //divMainContent.Visible = false;
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
                //Init_Spread();
            }
            else
            {
                //if (FpSpreadChapterWiseDMG.Sheets[0].RowCount > 0)
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

    #region Bind Header

    public void bindcollege()
    {
        try
        {
            //rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            //divMainContent.Visible = false;
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
            //rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            //divMainContent.Visible = false;

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
            //rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            //divMainContent.Visible = false;

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
            //rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            //divMainContent.Visible = false;

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
            //rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            //divMainContent.Visible = false;

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
            //rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            //divMainContent.Visible = false;

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
            //rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            //divMainContent.Visible = false;

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
            //rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            //divMainContent.Visible = false;

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
                if (Convert.ToString(ddlSem.SelectedValue).Trim() == "")
                {
                    sems = "";
                }
                else
                {
                    sems = " and SM.semester='" + Convert.ToString(ddlSem.SelectedValue).Trim() + "' ";
                }
                if (Convert.ToString(Session["Staff_Code"]).Trim() == "")
                {
                    //subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' " + Convert.ToString(sems).Trim() + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "' order by S.subject_no ";
                    subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code='" + Convert.ToString(ddlBranch.SelectedValue) + "' " + Convert.ToString(sems) + " and  S.subtype_no = Sem.subtype_no and promote_count='1' and SM.batch_year='" + Convert.ToString(ddlBatch.SelectedValue) + "' order by S.subject_no ";
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

                        cblSubjects.DataSource = ds;
                        cblSubjects.DataTextField = "subject_name";
                        cblSubjects.DataValueField = "subject_no";
                        cblSubjects.DataBind();
                        for (int h = 0; h < cblSubjects.Items.Count; h++)
                        {
                            cblSubjects.Items[h].Selected = true;
                        }
                        txtSubjects.Text = "Subjects" + "(" + cblSubjects.Items.Count + ")";
                        chkSubjects.Checked = true;
                    }
                    else
                    {
                        ddlSubject.Enabled = false;
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
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;

            subject_no = string.Empty;
            batch_year = string.Empty;
            degree_code = string.Empty;
            semester = string.Empty;
            section = string.Empty;
            string cblSubjectNo = string.Empty;

            int selSubjectCount = 0;
            ddlTestCompare.Items.Clear();
            ds.Clear();
            ds.Reset();
            ds.Dispose();
            if (ddlBatch.Items.Count > 0)
                batch_year = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
            if (ddlBranch.Items.Count > 0)
                degree_code = Convert.ToString(ddlBranch.SelectedValue).Trim();
            if (ddlSem.Items.Count > 0)
                semester = Convert.ToString(ddlSem.SelectedItem.Text).Trim();
            if (ddlSubject.Items.Count > 0)
            {
                subject_no = Convert.ToString(ddlSubject.SelectedItem.Value).Trim();
            }

            selSubjectCount = 0;

            if (cblSubjects.Items.Count == 0)
            {
                //lblpopuperr.Text = "No Subject were Found";
                //lblpopuperr.Visible = true;
                //popupdiv.Visible = true;
                //return;
            }
            else
            {
                selSubjectCount = 0;
                cblSubjectNo = string.Empty;
                foreach (System.Web.UI.WebControls.ListItem li in cblSubjects.Items)
                {
                    if (li.Selected)
                    {
                        selSubjectCount++;
                        //Array.Resize(ref subjectNumber, selSubjectCount);
                        //Array.Resize(ref subjectName, selSubjectCount);
                        //subjectNumber[selSubjectCount - 1] = li.Value;
                        //subjectName[selSubjectCount - 1] = li.Text;
                        if (string.IsNullOrEmpty(cblSubjectNo))
                        {
                            cblSubjectNo = "'" + li.Value + "'";
                        }
                        else
                        {
                            cblSubjectNo += ",'" + li.Value + "'";
                        }
                    }
                }
                if (selSubjectCount == 0)
                {
                    //lblpopuperr.Text = "Please Select Atleast One Subject";
                    //lblpopuperr.Visible = true;
                    //popupdiv.Visible = true;
                    //return;
                }
            }

            if (ddlSec.Enabled == false || ddlSec.Items.Count == 0)
            {
                section = "";
            }
            else if (ddlSec.Items.Count > 0)
            {
                section = Convert.ToString(ddlSec.SelectedItem.Text).Trim();
                qrysec = " and ts.section='" + section + "' ";
            }

            qry = "";
            if (!string.IsNullOrEmpty(batch_year) && !string.IsNullOrEmpty(degree_code) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(cblSubjectNo))
            {
                //qry = "select c.criteria,c.Criteria_no from CriteriaForInternal c, syllabus_master sy where c.syll_code=sy.syll_code and sy.Batch_Year='" + batch_year + "' and sy.degree_code='" + degree_code + "' and sy.semester='" + semester + "'";
                qry = "select distinct ts.comparisionName from Test_Comparision_Settings ts,FromTest_Comparision ft,ToTest_Comparision tt,subject s where ts.comaparisionID=ft.comaparisionID and tt.comaparisionID=ft.comaparisionID and ts.comaparisionID=tt.comaparisionID and s.subject_no=ts.subject_no and ts.subject_no in(" + cblSubjectNo + ") " + qrysec + " order by ts.comparisionName";
                ds = d2.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlTestCompare.DataSource = ds;
                    ddlTestCompare.DataTextField = "comparisionName";
                    ddlTestCompare.DataValueField = "comparisionName";
                    ddlTestCompare.DataBind();
                }
            }
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
            darkstyle.BackColor =Draw.ColorTranslator.FromHtml("#00aff0");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = Draw.Color.White;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = Draw.Color.Black;

            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            //sheetstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Medium;
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Center;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = Draw.Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = Draw.Color.Black;

            #endregion SpreadStyles

            FpSpreadChapterWiseDMG.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpreadChapterWiseDMG.Sheets[0].DefaultStyle = sheetstyle;
            FpSpreadChapterWiseDMG.Sheets[0].ColumnHeader.RowCount = 1;
                       
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
            //rptprint1.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            //divMainContent.Visible = false;

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
                qry = "select Roll_No,Reg_No,Stud_Name from Registration where Batch_Year='" + batch_year + "' and degree_code='" + degree_code + "'   " + qrysec + " and college_code='" + collegecode + "' and  CC='0' and DelFlag='0' and Exam_Flag<>'debar' order by Roll_No";//and Current_Semester ='" + semester + "'
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

    public void ChangeHeaderName(bool isschool)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
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

    #endregion Bind Header

    #region DropDown Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            txtRollNo.Text = "";
            divMainContent.Visible = false;
            divMainChart.Visible = false;
            rptprint1.Visible = false;
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            BindRollNo();
            GetSubject();
            BindTest();
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
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            txtRollNo.Text = "";
            divMainContent.Visible = false;
            divMainChart.Visible = false;
            rptprint1.Visible = false;
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            BindRollNo();
            GetSubject();
            BindTest();
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
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            txtRollNo.Text = "";
            divMainContent.Visible = false;
            divMainChart.Visible = false;
            rptprint1.Visible = false;
            bindbranch();
            bindsem();
            BindSectionDetail();
            BindRollNo();
            GetSubject();
            BindTest();
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
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            txtRollNo.Text = "";
            divMainContent.Visible = false;
            divMainChart.Visible = false;
            rptprint1.Visible = false;
            bindsem();
            BindSectionDetail();
            BindRollNo();
            GetSubject();
            BindTest();
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
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            txtRollNo.Text = "";
            divMainContent.Visible = false;
            divMainChart.Visible = false;
            rptprint1.Visible = false;
            BindSectionDetail();
            BindRollNo();
            GetSubject();
            BindTest();
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
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;
            divMainChart.Visible = false;
            rptprint1.Visible = false;

            txtRollNo.Text = "";
            BindRollNo();
            GetSubject();
            BindTest();
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
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            txtRollNo.Text = "";
            divMainContent.Visible = false;
            divMainChart.Visible = false;
            rptprint1.Visible = false;

            BindTest();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlTestCompare_Selectchanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;
            divMainChart.Visible = false;
            rptprint1.Visible = false;

            txtRollNo.Text = "";
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }

    }

    protected void cblSelRollNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            txtRollNo.Text = "";
            divMainContent.Visible = false;
            divMainChart.Visible = false;
            rptprint1.Visible = false;

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
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            txtRollNo.Text = "";
            divMainContent.Visible = false;
            divMainChart.Visible = false;
            rptprint1.Visible = false;

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
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;
            divMainChart.Visible = false;
            rptprint1.Visible = false;

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

    protected void chkSubjects_CheckedChanged(object sender, EventArgs e)
    {

        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        popupdiv.Visible = false;
        lblpopuperr.Text = string.Empty;
        divMainContent.Visible = false;
        divMainChart.Visible = false;
        rptprint1.Visible = false;


        txtSubjects.Text = "--Select--";
        int count = 0;
        if (chkSubjects.Checked == true)
        {
            count++;
            for (int i = 0; i < cblSubjects.Items.Count; i++)
            {
                cblSubjects.Items[i].Selected = true;
            }
            txtSubjects.Text = "Subjects (" + (cblSubjects.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblSubjects.Items.Count; i++)
            {
                cblSubjects.Items[i].Selected = false;
            }
            txtSubjects.Text = "--Select--";
        }
    }

    protected void cblSubjects_SelectedIndexChanged(object sender, EventArgs e)
    {

        lblErrSearch.Text = string.Empty;
        lblErrSearch.Visible = false;
        chkSubjects.Checked = false;
        popupdiv.Visible = false;
        lblpopuperr.Text = string.Empty;
        divMainContent.Visible = false;
        divMainChart.Visible = false;
        rptprint1.Visible = false;


        txtSubjects.Text = "--Select--";
        int commcount = 0;
        for (int i = 0; i < cblSubjects.Items.Count; i++)
        {
            if (cblSubjects.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cblSubjects.Items.Count)
            {
                chkSubjects.Checked = true;
            }
            txtSubjects.Text = "Subjects (" + Convert.ToString(commcount) + ")";
        }
    }

    #endregion DropDown Events

    #region Button Events

    #region GO BUTTON

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            int spreadHeight = 0;
            int selQuestionsCount = 0;

            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            divMainContent.Visible = false;
            divMainChart.Visible = false;
            rptprint1.Visible = false;

            lblpopuperr.Text = string.Empty;          

            string newroll_No = string.Empty;
            string secval = string.Empty;
            string qrysec = string.Empty;
            string qryInternal1 = string.Empty;
            string qryInternal2 = string.Empty;
            string qryQues = string.Empty;
            string qryRoll_no = string.Empty;
            string cblSubjectNo = string.Empty;

            string[] subjectNumber = new string[0];
            string[] subjectName = new string[0];
            string[] studRollNumber = new string[0];
           
            bool isQuesWiseSucc = false;
            bool isChpterWiseSucc = false;
            bool isIndividualOrMultiStudent = false;

            /// 0 means all students ; 
            /// 1 means only one students ;
            /// 2 means selective Students ;

            int IndividualOrMultiStudent = 0;
            int selSubjectCount = 0;
            int totalStudents = 0;
            int totalColumns = 0;

            isInternal = true;

            DataTable dtFromTest = new DataTable();
            DataTable dtToTest = new DataTable();

            gvStudentsSubjects = new GridView[0];
            chartPerformance = new Chart[0];

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
                secval = " and ts.section='" + section + "'";
                qrysec = " and Sections='" + section + "'";//and Sections=''

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

            selSubjectCount = 0;
            if (cblSubjects.Items.Count == 0)
            {
                lblpopuperr.Text = "No Subject were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                selSubjectCount = 0;
                cblSubjectNo = string.Empty;
                foreach (System.Web.UI.WebControls.ListItem li in cblSubjects.Items)
                {
                    if (li.Selected)
                    {
                        selSubjectCount++;
                        Array.Resize(ref subjectNumber, selSubjectCount);
                        Array.Resize(ref subjectName, selSubjectCount);
                        subjectNumber[selSubjectCount - 1] = li.Value;
                        subjectName[selSubjectCount - 1] = li.Text;
                        if (string.IsNullOrEmpty(cblSubjectNo))
                        {
                            cblSubjectNo = "'" + li.Value + "'";
                        }
                        else
                        {
                            cblSubjectNo += ",'" + li.Value + "'";
                        }
                    }
                }
                if (selSubjectCount == 0)
                {
                    lblpopuperr.Text = "Please Select Atleast One Subject";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }

            if (ddlTestCompare.Items.Count == 0)
            {
                lblpopuperr.Text = "No Test Comparision Were Found.Please Mapping The Test Comparision And Then Proceed.";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                test_name = Convert.ToString(ddlTestCompare.SelectedItem.Text).Trim();
                test_no = Convert.ToString(ddlTestCompare.SelectedItem.Value).Trim();
            }

            if (rblMultiSingleSelective.SelectedValue.Trim() == "0")
            {
                IndividualOrMultiStudent = 0;
                DataSet dsAllStud = new DataSet();
                if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batch_year) && !string.IsNullOrEmpty(degree_code))
                {
                    qry = "select Roll_No,Reg_No,Stud_Type,Stud_Name from Registration where CC=0 and DelFlag=0 and Exam_Flag<>'debar' and college_code='" + collegecode + "' and Batch_Year='" + batch_year + "' and degree_code='" + degree_code + "' " + qrysec + " order by Roll_No";
                   
                    dsAllStud = d2.select_method_wo_parameter(qry, "Text");
                }
                if (dsAllStud.Tables.Count > 0 && dsAllStud.Tables[0].Rows.Count > 0)
                {
                    studRollNumber = new string[dsAllStud.Tables[0].Rows.Count];
                    gvStudentsSubjects = new GridView[dsAllStud.Tables[0].Rows.Count];
                    chartPerformance = new Chart[dsAllStud.Tables[0].Rows.Count];
                    for (int stud = 0; stud <  dsAllStud.Tables[0].Rows.Count; stud++)
                    {
                        studRollNumber[stud] = Convert.ToString(dsAllStud.Tables[0].Rows[stud]["Roll_No"]);
                    }
                }
                else
                {
                    lblpopuperr.Text = "No Students Were Found";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
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
                        studRollNumber = new string[allRollNo.Length];
                        gvStudentsSubjects = new GridView[allRollNo.Length];
                        chartPerformance = new Chart[allRollNo.Length];
                        newroll_No = "";
                        for (int roll = 0; roll < allRollNo.Length; roll++)
                        {
                            studRollNumber[roll] = allRollNo[roll].Trim();
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

                foreach (System.Web.UI.WebControls.ListItem li in cblSelRollNo.Items)
                {
                    if (li.Selected)
                    {
                        selRollNoCount++;
                        Array.Resize(ref studRollNumber, selRollNoCount);
                        Array.Resize(ref gvStudentsSubjects, selRollNoCount);
                        Array.Resize(ref chartPerformance, selRollNoCount);
                        studRollNumber[selRollNoCount - 1] = li.Text;

                        //gvStudentsSubjects = new GridView[allRollNo.Length];
                        //chartPerformance = new Chart[allRollNo.Length];

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
            if (!string.IsNullOrEmpty(test_no) && !string.IsNullOrEmpty(subject_no) && !string.IsNullOrEmpty(cblSubjectNo))
            {
                qry = "select ts.comaparisionID,ts.comparisionName,ts.convertedTo,ts.section,ts.subject_no,case when ft.type=1 then ft.criteria_no else ft.criteria_no_New end as From_Criteria_No,ft.type as FromType,case when tt.type=1 then tt.criteria_no else tt.criteria_no_New end as To_Criteria_No,tt.type as ToType from Test_Comparision_Settings ts,FromTest_Comparision ft,ToTest_Comparision tt where ts.comaparisionID=ft.comaparisionID and ft.comaparisionID=tt.comaparisionID and tt.comaparisionID=ts.comaparisionID and ts.subject_no in (" + cblSubjectNo + ") " + secval + "  and ts.comparisionName='" + test_no + "'";

                ds.Clear();
                ds.Reset();
                ds = d2.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    degree_code = Convert.ToString(ddlBranch.SelectedValue).Trim();
                    batch_year = Convert.ToString(ddlBatch.SelectedValue).Trim();
                    string current_sem = Convert.ToString(ddlSem.SelectedValue).Trim();
                    string branch = Convert.ToString(ddlBranch.SelectedItem).Trim();
                    section = string.Empty;
                    pnlStudentsSubjects.Controls.Clear();
                    pnlStudentsChart.Controls.Clear();
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

                    spanTitle.Text = "Individual Students Test Performance Analysis Report";
                    spanTitle.Style.Add("text-decoration", "none");
                    spanTitle.Style.Add("font-family", "Book Antiqua;");
                    spanTitle.Style.Add("font-size", "18px");
                    spanTitle.Style.Add("text-align", "center");

                    //spanSub.Text = "Subject Name : " + Convert.ToString(ddlSubject.SelectedItem.Text);
                    //spanSub.Style.Add("text-decoration", "none");
                    //spanSub.Style.Add("font-family", "Book Antiqua;");
                    //spanSub.Style.Add("font-size", "18px");
                    //spanSub.Style.Add("text-align", "left");

                    dtFromTest = new DataTable();
                    dtToTest = new DataTable();
                    dtFromTest = ds.Tables[0].DefaultView.ToTable(true, "From_Criteria_No", "FromType", "subject_no", "section");
                    dtToTest = ds.Tables[0].DefaultView.ToTable(true, "To_Criteria_No", "ToType", "subject_no", "section");
                    string fromTestName = "";
                    string toTestName = "";
                    DataTable dtFromTest1 = ds.Tables[0].DefaultView.ToTable(true, "From_Criteria_No", "FromType");
                    DataTable dtToTest1 = ds.Tables[0].DefaultView.ToTable(true, "To_Criteria_No", "ToType");
                    Test_Name(dtFromTest1, ref fromTestName);
                    Test_Name(dtToTest1, ref toTestName);
                    dtAllStudMarks = new DataTable[studRollNumber.Length];
                    if (studRollNumber.Length > 0)
                    {
                        dtAllStudMarks = new DataTable[studRollNumber.Length];
                        //gvStudentsSubjects = new GridView[studRollNumber.Length];
                        //chartPerformance = new Chart[studRollNumber.Length];
                        for (int allstud = 0; allstud < studRollNumber.Length; allstud++)
                        {
                            dtAllStudMarks[allstud] = new DataTable();
                            gvStudentsSubjects[allstud] = new GridView();
                            chartPerformance[allstud] = new Chart();
                            int index = pnlStudentsSubjects.Controls.OfType<Chart>().ToList().Count + 1;
                            chartPerformance[allstud] = new Chart();
                            chartPerformance[allstud].ID = "chartStudentPerformance" + index;
                            dtAllStudMarks[allstud].Rows.Clear();
                            dtAllStudMarks[allstud].Columns.Clear();
                            dtAllStudMarks[allstud].Columns.Add("RollNo");
                            dtAllStudMarks[allstud].Columns.Add("Student_Name");
                            dtAllStudMarks[allstud].Columns.Add("Subject");
                            dtAllStudMarks[allstud].Columns.Add(fromTestName);
                            dtAllStudMarks[allstud].Columns.Add(toTestName);
                            string rollNo = studRollNumber[allstud];
                            string studName = d2.GetFunctionv("select Stud_Name from Registration where Roll_No='" + rollNo + "'");
                            string secname = d2.GetFunctionv("select sections from Registration where Roll_No='" + rollNo + "'");
                            DataRow dr;
                            chartPerformance[allstud].Series.Clear();
                            chartPerformance[allstud].Titles.Clear();
                            chartPerformance[allstud].Legends.Clear();
                            chartPerformance[allstud].Series.Add(fromTestName);
                            chartPerformance[allstud].Series.Add(toTestName);
                            Title title = new Title(studName + " Test Performance Analysis", Docking.Top, new System.Drawing.Font("Book Antiqua", 10, Draw.FontStyle.Bold), System.Drawing.Color.Black);
                            chartPerformance[allstud].Titles.Add(title);
                            chartPerformance[allstud].Legends.Add(fromTestName);
                            chartPerformance[allstud].Legends.Add(toTestName);
                            chartPerformance[allstud].Legends[0].Alignment = Draw.StringAlignment.Center;
                            chartPerformance[allstud].Legends[1].Alignment = Draw.StringAlignment.Center;
                            chartPerformance[allstud].Legends[0].Docking = Docking.Bottom;
                            chartPerformance[allstud].Legends[1].Docking = Docking.Bottom;
                            chartPerformance[allstud].ChartAreas.Clear();
                            chartPerformance[allstud].ChartAreas.Add("Test Performance");
                            chartPerformance[allstud].Width = 600;
                            chartPerformance[allstud].RenderType = RenderType.ImageTag;
                            chartPerformance[allstud].ImageType = ChartImageType.Png;
                            chartPerformance[allstud].ImageStorageMode = ImageStorageMode.UseImageLocation;


                            //string imgPath = HttpContext.Current.Request.PhysicalApplicationPath + "image\\" + "chartChapterDMG" + index;
                            chartPerformance[allstud].ImageLocation = Path.Combine("~/Image/", "chartStudentPerformance" + index); 

                            for (int sub = 0; sub < selSubjectCount; sub++)
                            {
                                dr = dtAllStudMarks[allstud].NewRow();
                                dr["RollNo"] = Convert.ToString(rollNo);
                                dr["Student_Name"] = Convert.ToString(studName);
                                dr["Subject"] = Convert.ToString(subjectName[sub]);
                                string fromMark="";
                                string toMarks="";
                                Calculate_Mark(dtFromTest, ref fromMark, subjectNumber[sub], rollNo, secname);
                                Calculate_Mark(dtToTest, ref toMarks, subjectNumber[sub], rollNo, secname);
                                dr[fromTestName] = Convert.ToString(fromMark);
                                dr[toTestName] = Convert.ToString(toMarks);
                                dtAllStudMarks[allstud].Rows.Add(dr);
                            }
                            if (dtAllStudMarks[allstud].Rows.Count > 0)
                            {
                                gvStudentsSubjects[allstud].DataSource =dtAllStudMarks[allstud];
                                gvStudentsSubjects[allstud].DataBind();
                                pnlStudentsSubjects.Controls.Add(gvStudentsSubjects[allstud]);

                                if (gvStudentsSubjects[allstud].HeaderRow.Cells.Count > 0)
                                {
                                    for (int headerRows = 0; headerRows < gvStudentsSubjects[allstud].HeaderRow.Cells.Count; headerRows++)
                                    {
                                        string headerValues = gvStudentsSubjects[allstud].HeaderRow.Cells[headerRows].Text;
                                        //var output = Regex.Replace(headerValues, @"[\d-]", string.Empty);
                                        //gvNew.HeaderRow.Cells[headerRows].Text = output;
                                        gvStudentsSubjects[allstud].HeaderRow.Cells[headerRows].BackColor = Draw.ColorTranslator.FromHtml("#00aff0");
                                        gvStudentsSubjects[allstud].HeaderRow.Cells[headerRows].ForeColor = Draw.Color.Black;
                                        gvStudentsSubjects[allstud].HeaderRow.Cells[headerRows].BorderColor = Draw.Color.Black;
                                        gvStudentsSubjects[allstud].HeaderRow.Cells[headerRows].Wrap = true;
                                        gvStudentsSubjects[allstud].HeaderRow.Cells[headerRows].Width = headerValues.Length * 10 + 20;
                                    }
                                }

                                for (int rr = 0; rr <  gvStudentsSubjects[allstud].Rows.Count; rr++)
                                {
                                    for (int cc = 0; cc < gvStudentsSubjects[allstud].Rows[rr].Cells.Count; cc++)
                                    {
                                        if (cc == 2)
                                        {
                                            gvStudentsSubjects[allstud].Rows[rr].Cells[cc].HorizontalAlign = HorizontalAlign.Left;
                                            gvStudentsSubjects[allstud].Rows[rr].Cells[cc].VerticalAlign = VerticalAlign.Middle;
                                        }
                                        else
                                        {

                                            gvStudentsSubjects[allstud].Rows[rr].Cells[cc].HorizontalAlign = HorizontalAlign.Center;
                                            gvStudentsSubjects[allstud].Rows[rr].Cells[cc].VerticalAlign = VerticalAlign.Middle;
                                        }
                                    }
                                }

                                for (int i = gvStudentsSubjects[allstud].Rows.Count - 1; i > 0; i--)
                                {
                                    GridViewRow row = gvStudentsSubjects[allstud].Rows[i];
                                    GridViewRow previousRow = gvStudentsSubjects[allstud].Rows[i - 1];
                                    for (int j = 1; j <= 1; j++)
                                    {
                                        //Label lnlname = (Label)row.FindControl("lbl_edulevel");
                                        //Label lnlname1 = (Label)previousRow.FindControl("lbl_edulevel");

                                        string firstRow = row.Cells[j].Text;
                                        string lastRow = row.Cells[j].Text;
                                        if (firstRow == lastRow)
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
                                for (int i = gvStudentsSubjects[allstud].Rows.Count - 1; i > 0; i--)
                                {
                                    GridViewRow row = gvStudentsSubjects[allstud].Rows[i];
                                    GridViewRow previousRow = gvStudentsSubjects[allstud].Rows[i - 1];
                                    for (int j = 0; j <= 0; j++)
                                    {
                                        //Label lnlname = (Label)row.FindControl("lbl_edulevel");
                                        //Label lnlname1 = (Label)previousRow.FindControl("lbl_edulevel");

                                        string firstRow = row.Cells[j].Text;
                                        string lastRow = row.Cells[j].Text;
                                        if (firstRow == lastRow)
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
                            if (dtAllStudMarks[allstud].Rows.Count > 0)
                            {
                                for (int chp = 0; chp < dtAllStudMarks[allstud].Rows.Count; chp++)
                                {
                                    chartPerformance[allstud].Series[0].Points.AddXY(Convert.ToString(dtAllStudMarks[allstud].Rows[chp]["Subject"]), Convert.ToString(dtAllStudMarks[allstud].Rows[chp][fromTestName]));
                                    chartPerformance[allstud].Series[1].Points.AddXY(Convert.ToString(dtAllStudMarks[allstud].Rows[chp]["Subject"]), Convert.ToString(dtAllStudMarks[allstud].Rows[chp][toTestName]));
                                    chartPerformance[allstud].ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                    chartPerformance[allstud].ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                                    chartPerformance[allstud].Series[0].IsValueShownAsLabel = true;
                                    chartPerformance[allstud].Series[0].IsXValueIndexed = true;

                                    chartPerformance[allstud].Series[1].IsValueShownAsLabel = true;
                                    chartPerformance[allstud].Series[1].IsXValueIndexed = true;

                                    chartPerformance[allstud].ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                    chartPerformance[allstud].ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                    chartPerformance[allstud].ChartAreas[0].AxisY.Maximum = 100;
                                    chartPerformance[allstud].ChartAreas[0].AxisY.Minimum = 0;
                                }
                                pnlStudentsSubjects.Controls.Add(chartPerformance[allstud]);
                            }
                        }
                    }
                    divMainContent.Visible = true;
                    divMainChart.Visible = true;
                    rptprint1.Visible = true;

                    popupdiv.Visible =false;
                    lblpopuperr.Text = "";
                }
                else
                {
                    divMainContent.Visible = false;
                    divMainChart.Visible = false;
                    rptprint1.Visible = false;

                    lblpopuperr.Text = "No Record(s) Were Found";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
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
               //htm.InnerWriter.WriteLine("<center>");
               //if (dscol.Tables[0].Rows.Count > 0)
               //{
               //    lb.Text = Convert.ToString(dscol.Tables[0].Rows[0]["collname"]) + "<br> ";
               //    lb.Style.Add("height", "100px");
               //    lb.Style.Add("text-decoration", "none");
               //    lb.Style.Add("font-family", "Book Antiqua;");
               //    lb.Style.Add("font-size", "18px");
               //    lb.Style.Add("font-weight", "bold");
               //    lb.Style.Add("text-align", "center");
               //    lb.RenderControl(htm);

               //    string address = "";
               //    if (Convert.ToString(dscol.Tables[0].Rows[0]["address1"]).Trim() != "")
               //    {
               //        address = Convert.ToString(dscol.Tables[0].Rows[0]["address1"]);
               //    }
               //    if (Convert.ToString(dscol.Tables[0].Rows[0]["address2"]).Trim() != "")
               //    {
               //        if (address == "")
               //        {
               //            address = Convert.ToString(dscol.Tables[0].Rows[0]["address2"]);
               //        }
               //        else
               //        {
               //            address = address + ", " + Convert.ToString(dscol.Tables[0].Rows[0]["address2"]);
               //        }
               //    }
               //    if (Convert.ToString(dscol.Tables[0].Rows[0]["address3"]).Trim() != "")
               //    {
               //        if (address == "")
               //        {
               //            address = Convert.ToString(dscol.Tables[0].Rows[0]["address3"]);
               //        }
               //        else
               //        {
               //            address = address + ", " + Convert.ToString(dscol.Tables[0].Rows[0]["address3"]);
               //        }
               //    }
               //    if (address.Trim() != "")
               //    {
               //        lb.Text = address + "<br> ";
               //        lb.Style.Add("height", "100px");
               //        lb.Style.Add("text-decoration", "none");
               //        lb.Style.Add("font-family", "Book Antiqua;");
               //        lb.Style.Add("font-size", "12px");
               //        lb.Style.Add("text-align", "center");
               //        lb.RenderControl(htm);
               //    }
               //}
               //Label lb2 = new Label();
               //lb2.Text = degreedetails;
               //lb2.Style.Add("height", "100px");
               //lb2.Style.Add("text-decoration", "none");
               //lb2.Style.Add("font-family", "Book Antiqua;");
               //lb2.Style.Add("font-size", "10px");
               //lb2.Style.Add("font-weight", "bold");
               //lb2.Style.Add("text-align", "center");
               //lb2.RenderControl(htm);
               //Label lb3 = new Label();
               //lb3.Text = "<br>";
               //lb3.Style.Add("height", "200px");
               //lb3.Style.Add("text-decoration", "none");
               //lb3.Style.Add("font-family", "Book Antiqua;");
               //lb3.Style.Add("font-size", "10px");
               //lb3.Style.Add("text-align", "left");
               //lb3.RenderControl(htm);
               //Label lb4 = new Label();
               //lb4.Text = "Individual Student's Test Performance Analysis_Report<br><br>";
               //lb4.Style.Add("height", "200px");
               //lb4.Style.Add("font-weight", "bold");
               //lb4.Style.Add("text-decoration", "none");
               //lb4.Style.Add("font-family", "Book Antiqua;");
               //lb4.Style.Add("font-size", "10px");
               //lb4.Style.Add("text-align", "center");
               //lb4.RenderControl(htm);

               htm.InnerWriter.WriteLine("</center>");

               btnGo_Click(sender, e);

               Label lb2 = new Label();
               lb2.Text = "<br/><br/><br/><br/><br/><br/>";
               lb2.Style.Add("height", "100px");
               lb2.Style.Add("text-decoration", "none");
               lb2.Style.Add("font-family", "Book Antiqua;");
               lb2.Style.Add("font-size", "10px");
               lb2.Style.Add("font-weight", "bold");
               lb2.Style.Add("text-align", "center");
               lb2.RenderControl(htm);

               divMainContent.RenderControl(htm);

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
            Response.AddHeader("content-disposition", "attachment;filename=Individual_Student's_Test_Performance_Analysis_Report.pdf");
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
            lb4.Text = "Individual Student's Test Performance Analysis Report<br><br>";
            lb4.Style.Add("height", "200px");
            lb4.Style.Add("text-decoration", "none");
            lb4.Style.Add("font-family", "Book Antiqua;");
            lb4.Style.Add("font-size", "10px");
            lb4.Style.Add("text-align", "center");
            lb4.RenderControl(hw);

            StringReader sr = new StringReader(Convert.ToString(sw));
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);           

            StringWriter sw1 = new StringWriter();
            HtmlTextWriter hw1 = new HtmlTextWriter(sw1);

            btnGo_Click(sender, e);

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

            if (divMainContent.Visible == true)
            {
                if (dtAllStudMarks.Length > 0 && chartPerformance.Length > 0)
                {
                    if (chartPerformance.Length == dtAllStudMarks.Length)
                    {
                        for (int rows = 0; rows < dtAllStudMarks.Length; rows++)
                        {
                            //for (int rr = 0; rr < gvStudentsSubjects[rows].Rows.Count; rr++)
                            //{
                            //    for (int cc = 0; cc < gvStudentsSubjects[rows].Rows[rr].Cells.Count; cc++)
                            //    {
                            //        if (cc == 2)
                            //        {
                            //            gvStudentsSubjects[rows].Rows[rr].Cells[cc].HorizontalAlign = HorizontalAlign.Left;
                            //            gvStudentsSubjects[rows].Rows[rr].Cells[cc].VerticalAlign = VerticalAlign.Middle;
                            //        }
                            //        else
                            //        {

                            //            gvStudentsSubjects[rows].Rows[rr].Cells[cc].HorizontalAlign = HorizontalAlign.Center;
                            //            gvStudentsSubjects[rows].Rows[rr].Cells[cc].VerticalAlign = VerticalAlign.Middle;
                            //        }
                            //    }
                            //}

                            //for (int i = gvStudentsSubjects[rows].Rows.Count - 1; i > 0; i--)
                            //{
                            //    GridViewRow row = gvStudentsSubjects[rows].Rows[i];
                            //    GridViewRow previousRow = gvStudentsSubjects[rows].Rows[i - 1];
                            //    for (int j = 1; j <= 1; j++)
                            //    {
                            //        //Label lnlname = (Label)row.FindControl("lbl_edulevel");
                            //        //Label lnlname1 = (Label)previousRow.FindControl("lbl_edulevel");

                            //        string firstRow = row.Cells[j].Text;
                            //        string lastRow = row.Cells[j].Text;
                            //        if (firstRow == lastRow)
                            //        {
                            //            if (previousRow.Cells[j].RowSpan == 0)
                            //            {
                            //                if (row.Cells[j].RowSpan == 0)
                            //                {
                            //                    previousRow.Cells[j].RowSpan += 2;
                            //                }
                            //                else
                            //                {
                            //                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            //                }
                            //                row.Cells[j].Visible = false;
                            //            }
                            //        }
                            //    }
                            //}
                            //for (int i = gvStudentsSubjects[rows].Rows.Count - 1; i > 0; i--)
                            //{
                            //    GridViewRow row = gvStudentsSubjects[rows].Rows[i];
                            //    GridViewRow previousRow = gvStudentsSubjects[rows].Rows[i - 1];
                            //    for (int j = 0; j <= 0; j++)
                            //    {
                            //        //Label lnlname = (Label)row.FindControl("lbl_edulevel");
                            //        //Label lnlname1 = (Label)previousRow.FindControl("lbl_edulevel");

                            //        string firstRow = row.Cells[j].Text;
                            //        string lastRow = row.Cells[j].Text;
                            //        if (firstRow == lastRow)
                            //        {
                            //            if (previousRow.Cells[j].RowSpan == 0)
                            //            {
                            //                if (row.Cells[j].RowSpan == 0)
                            //                {
                            //                    previousRow.Cells[j].RowSpan += 2;
                            //                }
                            //                else
                            //                {
                            //                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            //                }
                            //                row.Cells[j].Visible = false;
                            //            }
                            //        }
                            //    }
                            //}
                            GridView gvNew = new GridView();
                            sw1 = new StringWriter();
                            hw1 = new HtmlTextWriter(sw1);
                            if (dtAllStudMarks[rows].Rows.Count > 0)
                            {
                                gvNew.DataSource = dtAllStudMarks[rows];
                                gvNew.DataBind();

                                if (gvNew.HeaderRow.Cells.Count > 0)
                                {
                                    for (int headerRows = 0; headerRows < gvNew.HeaderRow.Cells.Count; headerRows++)
                                    {
                                        string headerValues = gvNew.HeaderRow.Cells[headerRows].Text;
                                        //var output = Regex.Replace(headerValues, @"[\d-]", string.Empty);
                                        //gvNew.HeaderRow.Cells[headerRows].Text = output;
                                        gvNew.HeaderRow.Cells[headerRows].BackColor =Draw.ColorTranslator.FromHtml("#00aff0");
                                        gvNew.HeaderRow.Cells[headerRows].ForeColor = Draw.Color.Black;
                                        gvNew.HeaderRow.Cells[headerRows].BorderColor = Draw.Color.Black;
                                        gvNew.HeaderRow.Cells[headerRows].HorizontalAlign = HorizontalAlign.Center;
                                        gvNew.HeaderRow.Cells[headerRows].VerticalAlign = VerticalAlign.Middle;
                                        gvNew.HeaderRow.Cells[headerRows].Wrap = true;
                                        gvNew.HeaderRow.Cells[headerRows].Width = headerValues.Length * 10 + 20;
                                    }
                                }
                                for (int rr = 0; rr < gvNew.Rows.Count; rr++)
                                {
                                    for (int cc = 0; cc < gvNew.Rows[rr].Cells.Count; cc++)
                                    {
                                        if (cc == 2)
                                        {
                                            gvNew.Rows[rr].Cells[cc].HorizontalAlign = HorizontalAlign.Left;
                                            gvNew.Rows[rr].Cells[cc].VerticalAlign = VerticalAlign.Middle;
                                        }
                                        else
                                        {
                                            gvNew.Rows[rr].Cells[cc].HorizontalAlign = HorizontalAlign.Center;
                                            gvNew.Rows[rr].Cells[cc].VerticalAlign = VerticalAlign.Middle;
                                        }
                                    }
                                }
                                gvNew.RenderControl(hw1);
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
                            using (MemoryStream stream = new MemoryStream())
                            {
                                chartPerformance[rows].SaveImage(stream, ChartImageFormat.Png);
                                iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
                                chartImage.ScalePercent(250f);
                                pdfDoc.Add(chartImage);
                            }
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

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    
    #endregion Button Events

    public void Calculate_Mark(DataTable dtFromOrTo, ref string Marks, string subjectNo, string rollNo, string section = null)
    {
        try
        {
           // criteriaNo = criteriaNo.Trim();
            int testCount = dtFromOrTo.Rows.Count;

            if (testCount > 0)
            {
                dtFromOrTo.DefaultView.RowFilter = "subject_no='" + subjectNo + "' " + ((string.IsNullOrEmpty(section.Trim())) ? "" : " and section='" + section + "'");//dtFromOrTo
                DataView dv = dtFromOrTo.DefaultView;
                dtFromOrTo = dv.ToTable();
            }
            testCount = dtFromOrTo.Rows.Count;
            //int typeCount = Type.Length;
            Marks = Marks.Trim();
            subjectNo = subjectNo.Trim();
            section = section.Trim();
            rollNo = rollNo.Trim();
            string qrysec = string.Empty;
            string qrySubNo = string.Empty;

            double totalMarks = 0;
            double average = 0;
            //int rollNumberCount = rollNo.Length;
            if (!string.IsNullOrEmpty(subjectNo))
            {

            }
            if (!string.IsNullOrEmpty(section))
            {
                qrysec=" and e.sections='"+section+"'";
            }
            if (testCount > 0  && !string.IsNullOrEmpty(subjectNo) && !string.IsNullOrEmpty(rollNo))
            {
                for (int test = 0; test < testCount; test++)
                {
                    string testNo = Convert.ToString(dtFromOrTo.Rows[test][0]).Trim();
                    string testType = Convert.ToString(dtFromOrTo.Rows[test][1]).Trim();
                    string testMark = string.Empty;
                    double testMarks = 0;
                    switch (testType)
                    {
                        case "1":
                            qry = "select case when re.marks_obtained>0 then (re.marks_obtained/e.max_mark)*100 else re.marks_obtained end as OUTOF100 from CriteriaForInternal c,Exam_type e, Result re  where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and e.subject_no in('" + subjectNo + "') and re.roll_no in('" + rollNo + "') and c.Criteria_no='" + testNo + "' " + qrysec + " order by re.roll_no,e.subject_no,c.Criteria_no;";
                        //select re.roll_no,e.subject_no,c.criteria,c.Criteria_no,c.max_mark,e.max_mark,case when re.marks_obtained>0 then (re.marks_obtained/e.max_mark)*c.max_mark else re.marks_obtained end as Actual_Mark,case when re.marks_obtained>0 then (re.marks_obtained/e.max_mark)*100 else re.marks_obtained end as OUTOF100 from CriteriaForInternal c,Exam_type e, Result re  where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and e.subject_no in('1745') and re.roll_no in('14JEAE101') order by re.roll_no,e.subject_no,c.Criteria_no
                            testMark = d2.GetFunctionv(qry);
                            break;
                        case "2":
                            string testName = "";
                            FindTestName(testNo, testType, ref testName);
                            qry = "select sum(case when re.marks_obtained>0 then (re.marks_obtained/e.max_mark)*100 end)/COUNT(ca.caluationID) as OUTOF100 from CriteriaForInternal c,Exam_type e,CAM_Calculation_Make_New_Test ca, CAM_Calculation_Test_Settings se,Result re  where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and ca.caluationID=se.caluationID and ca.subject_no=se.subject_no and se.subject_no=e.subject_no and ca.subject_no=e.subject_no and e.sections=ca.section and e.sections=se.section and se.section=ca.section and se.criteria_no=c.Criteria_no and re.roll_no in('" + rollNo + "') and ca.New_Test_Name in('" + testName + "') and e.subject_no in ('" + subjectNo + "') " + qrysec + " group by re.roll_no,e.subject_no,ca.New_Test_Name,ca.caluationID,ca.convertedTo order by re.roll_no,e.subject_no,ca.caluationID ;";
                        
                        //select re.roll_no,e.subject_no,ca.New_Test_Name as Test_Name,ca.caluationID,ca.convertedTo,sum(case when re.marks_obtained>0 then (re.marks_obtained/e.max_mark)*ca.convertedTo end)/COUNT(ca.caluationID) as Calculated_Mark,sum(case when re.marks_obtained>0 then (re.marks_obtained/e.max_mark)*100 end)/COUNT(ca.caluationID) as OUTOF100 from CriteriaForInternal c,Exam_type e,CAM_Calculation_Make_New_Test ca, CAM_Calculation_Test_Settings se,Result re  where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and ca.caluationID=se.caluationID and ca.subject_no=se.subject_no and se.subject_no=e.subject_no and ca.subject_no=e.subject_no and e.sections=ca.section and e.sections=se.section and se.section=ca.section and se.criteria_no=c.Criteria_no and re.roll_no in('14JEAE101') and ca.caluationID in(1) and e.subject_no in (2116) group by re.roll_no,e.subject_no,ca.New_Test_Name,ca.caluationID,ca.convertedTo order by re.roll_no,e.subject_no,ca.caluationID;

                            testMark = d2.GetFunctionv(qry);
                            break;
                    }
                    double.TryParse(testMark, out testMarks);
                    if (testMarks >= 0)
                    {
                        totalMarks += testMarks;
                    }
                }
                average = totalMarks / testCount;
                average = Math.Round(average, 0, MidpointRounding.AwayFromZero);
            }
            Marks = Convert.ToString(average);
        }
        catch (Exception ex)
        {
        }
    }

    public void Test_Name(DataTable dtFromOrTo,ref string TestName)
    {
        try
        {
            int testCount = dtFromOrTo.Rows.Count;
            if (testCount > 0)
            {
                string testno2 = "";
                string test_name2 = "";
                for (int test = 0; test < testCount; test++)
                {
                    string testNo = Convert.ToString(dtFromOrTo.Rows[test][0]).Trim();
                    string testType = Convert.ToString(dtFromOrTo.Rows[test][1]).Trim();
                    string test_name = "";
                    switch (testType)
                    {
                        case "1":
                            qry = "select distinct criteria from CriteriaForInternal where Criteria_no='"+testNo+"'";
                            test_name = d2.GetFunctionv(qry);
                            break;
                        case "2":
                            //qry = "select distinct New_Test_Name from CAM_Calculation_Make_New_Test where caluationID='" + testNo + "'";
                            //test_name = d2.GetFunctionv(qry);
                            if (testno2 == "")
                            {
                                testno2 = "'" + testNo + "'";
                            }
                            else
                            {
                                testno2 += ",'" + testNo + "'";
                            }
                            break;
                    }
                    if (test_name.Trim() != "")
                    {
                        if (TestName.Trim() == "")
                        {
                            TestName = test_name.Trim();
                        }
                        else
                        {
                            TestName += " & " + test_name.Trim();
                        }
                    }
                }
                if (testno2.Trim() != "")
                {
                    qry = "select distinct New_Test_Name from CAM_Calculation_Make_New_Test where caluationID in(" + testno2 + ")";
                    DataSet dst = d2.select_method_wo_parameter(qry,"Text");
                    if (dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
                    {
                        for (int r = 0; r <  dst.Tables[0].Rows.Count; r++)
                        {
                            if (TestName.Trim() == "")
                            {
                                TestName = Convert.ToString(dst.Tables[0].Rows[r][0]).Trim();
                            }
                            else
                            {
                                TestName += " & " + Convert.ToString(dst.Tables[0].Rows[r][0]).Trim();
                            }
                        }
                    }
                }
                
                if (TestName != "")
                {
                    TestName +=test_name2+ " (AVG)";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void FindTestName(string testNo, string Type, ref string testName)
    {
        try
        {
            testNo = testNo.Trim();
            Type = Type.Trim();

            if (testNo != "")
            {
                switch (Type)
                {
                    case "1":
                        testName = d2.GetFunctionv("select criteria from CriteriaForInternal where Criteria_no='" + testNo + "'");
                        break;
                    case "2":
                        testName = d2.GetFunctionv("select New_Test_Name from CAM_Calculation_Make_New_Test where caluationID='" + testNo + "'");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

}