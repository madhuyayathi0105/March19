using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.Drawing;
using InsproDataAccess;

public partial class MarkMod_Student_Performance_Report : System.Web.UI.Page
{

    #region Field_Declaration
    SqlConnection con_Getfunc = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_subcrd = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con_splhr_query_master = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlCommand cmd = new SqlCommand();
    ReuasableMethods rs = new ReuasableMethods();
    InsproDirectAccess dir = new InsproDirectAccess();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string examcodeval = string.Empty;

    string strsec = string.Empty;
    string strsec1 = string.Empty;

    string sqlstr = string.Empty;
    string sqlpercmd, sqlsylcmd, sqlmarkcmd, sqlsubjcmd;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;

    int subjectctot = 0, criteriatot = 0, tottet;

    static int subjectcnt = 0;
    public string criteriano, subjno;
    string strsem = string.Empty;
    static int sectioncnt = 0;
    int count4 = 0;
    int countv = 0;
    string group_code = "", columnfield = string.Empty;
    int rocount = 0;
    string hcrollno = string.Empty;
    static Hashtable htb = new Hashtable();
    static Hashtable htcriteria = new Hashtable();
    string strorder = string.Empty;
    string strregorder = string.Empty;
    string studname = string.Empty;
    string latmode = string.Empty;
    string regn = string.Empty;
    static Hashtable htsubjcide = new Hashtable();
    Hashtable hat = new Hashtable();
    Hashtable htv = new Hashtable();
    Hashtable htv3 = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet dsbind = new DataSet();
    DataTable dt = new DataTable();
    DataSet dsmethodgoper = new DataSet();
    DataSet dsmethodgosubj = new DataSet();
    DataSet dsmethodgocriteria = new DataSet();
    DataSet dsmethodgomark = new DataSet();
    DataSet tempdssubj = new DataSet();
    DataSet dsprint = new DataSet();
    string grouporusercode = "";
    Dictionary<int, string> diccoursehead = new Dictionary<int, string>();
    Dictionary<int, string> diccoursevalue = new Dictionary<int, string>();
    Dictionary<int, string> dictotper = new Dictionary<int, string>();
    int colcnt = 0;
    DataTable data = new DataTable();
    DataRow drow;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!Request.FilePath.Contains("CAMHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/MarkMod/CAMHome.aspx");
                return;
            }
        }
        collegecode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
        usercode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
        singleuser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
        group_user = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";


        if (!IsPostBack)
        {
            rbtformate1.Checked = true;

            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
            }

            string set = "select * from Master_Settings where settings in('Admission No','RollNo','RegisterNo','Student_Type') " + grouporusercode + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(set, "text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int u = 0; u < ds.Tables[0].Rows.Count; u++)
                {
                    if (ds.Tables[0].Rows[u]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[u]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[u]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[u]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[u]["settings"].ToString() == "Admission No" && ds.Tables[0].Rows[u]["value"].ToString() == "1")
                    {
                        Session["AdmissionNo"] = "1";
                    }

                }
            }
            bindcollege();
            if (ddlcollege.Items.Count >= 1)
            {
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                if (ddldegree.Items.Count > 0)
                {
                    BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                    BindSem(strbranch, strbatchyear, collegecode);
                    BindSectionDetail(strbatch, strbranch);
                    BindCourseoutcome();
                    BindSubjecttest(strbatch, strbranch, strsem, strsec);
                    Bindtest();
                }
                else
                {
                    //errmsg.Visible = true;
                    //errmsg.Text = "Give degree rights to staff";
                }
            }
            else
            {
                // errmsg.Visible = true;
                // errmsg.Text = "Give college rights to staff";
            }
        }
    }


    #region College
    public void bindcollege()
    {
        try
        {

            Session["QueryString"] = string.Empty;
            group_code = Session["group_code"].ToString();
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            dsprint = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = dsprint;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();

            }


        }
        catch
        {
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcollege.Items.Count >= 1)
        {
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            if (ddldegree.Items.Count > 0)
            {
                BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                BindSem(strbranch, strbatchyear, collegecode);
                BindSectionDetail(strbatch, strbranch);
                BindCourseoutcome();
                BindSubjecttest(strbatch, strbranch, strsem, strsec);
                Bindtest();
            }
            else
            {
                //errmsg.Visible = true;
                //errmsg.Text = "Give degree rights to staff";
            }
        }
        if (Convert.ToString(Session["QueryString"]) != "")
        {
            PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            // make collection editable
            isreadonly.SetValue(this.Request.QueryString, false, null);
            // remove
            this.Request.QueryString.Remove(Convert.ToString(Session["QueryString"]));
            Request.QueryString.Clear();
        }
        Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();
        Showgrid.Visible = false;
        divMainContents.Visible = false;
        printtable.Visible = false;
    }
    #endregion

    #region Batch
    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds2;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            BindDegree(singleuser, group_user, collegecode, usercode);
            if (ddldegree.Items.Count > 0)
            {
                BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                BindSem(strbranch, strbatchyear, collegecode);
                BindSectionDetail(strbatch, strbranch);
                BindSubjecttest(strbatch, strbranch, strsem, strsec);
                Bindtest();
            }
            Showgrid.Visible = false;
            divMainContents.Visible = false;
            printtable.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region Degree
    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            ddldegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds2;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (ddldegree.Items.Count > 0)
            {
                BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                BindSem(strbranch, strbatchyear, collegecode);
                BindSectionDetail(strbatch, strbranch);
                BindSubjecttest(strbatch, strbranch, strsem, strsec);
                Bindtest();
            }
            Showgrid.Visible = false;
            divMainContents.Visible = false;
            printtable.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region Branch
    public void BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            course_id = ddldegree.SelectedValue.ToString();
            ddlbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds2;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {


        try
        {
            //if ((ddlbranch.SelectedIndex != 0) && (ddlbranch.SelectedIndex > 0))
            //{
            BindSem(strbranch, strbatchyear, collegecode);
            BindSectionDetail(strbatch, strbranch);
            BindSubjecttest(strbatch, strbranch, strsem, strsec);
            Bindtest();
            // }
            Showgrid.Visible = false;
            divMainContents.Visible = false;
            printtable.Visible = false;
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }
    #endregion

    #region Sem
    public void BindSem(string strbranch, string strbatchyear, string collegecode)
    {
        try
        {
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            ddlsemester.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSem(strbranch, strbatchyear, collegecode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds2.Tables[0].Rows[0][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds2.Tables[0].Rows[0][0]).ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsemester.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsemester.Items.Add(i.ToString());
                    }
                }
            }
            //BindSubjecttest(strbatch, strbranch, strsem, strsec);
            //  Bindtest(strbatch, strbranch, strsem, strsec1);
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


            DataSet testsubj = new DataSet();
            BindSectionDetail(strbatch, strbranch);
            BindSubjecttest(strbatch, strbranch, strsem, strsec);
            Bindtest();
            Showgrid.Visible = false;
            divMainContents.Visible = false;
            printtable.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region Sec
    public void BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {
            strbatch = ddlbatch.SelectedValue.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            ddlsection.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSectionDetail(strbatch, strbranch);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlsection.DataSource = ds2;
                ddlsection.DataTextField = "sections";
                ddlsection.DataBind();
                ddlsection.Items.Insert(0, "All");
                if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                {
                    ddlsection.Enabled = false;
                    //BindSubjecttest(strbatch, strbranch, strsem, strsec);
                    //Bindtest();
                }
                else
                {
                    ddlsection.Enabled = true;
                    // BindSubjecttest(strbatch, strbranch, strsem, strsec);
                    //    Bindtest(strbatch, strbranch, strsem, strsec1);
                }
            }
            else
            {
                ddlsection.Enabled = false;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        printtable.Visible = false;
        Showgrid.Visible = false;
        divMainContents.Visible = false;
        BindSubjecttest(strbatch, strbranch, strsem, strsec);
        Bindtest();
    }

    #endregion

    #region Courseoutcome
    public void BindCourseoutcome()
    {
        try
        {
            Textcourse.Text = "---Select---";
            Checkcourse.Checked = false;
            CkLcourse.Items.Clear();
            dsmethodgosubj.Clear();
            string course = "Select distinct template,masterno from  Master_Settings where settings='COSettings'";
            dsmethodgosubj = d2.select_method_wo_parameter(course, "Text");
            if (dsmethodgosubj.Tables[0].Rows.Count > 0)
            {
                CkLcourse.DataSource = dsmethodgosubj;
                CkLcourse.DataTextField = "template";
                CkLcourse.DataValueField = "masterno";
                CkLcourse.DataBind();

            }
            if (CkLcourse.Items.Count > 0)
            {
                for (int row = 0; row < CkLcourse.Items.Count; row++)
                {
                    CkLcourse.Items[row].Selected = true;
                    Checkcourse.Checked = true;
                }
                Textcourse.Text = "Course(" + CkLcourse.Items.Count + ")";
            }
            else
            {
                Textcourse.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void Checkcourse_CheckedChanged(object sender, EventArgs e)
    {
        if (Checkcourse.Checked == true)
        {
            for (int i = 0; i < CkLcourse.Items.Count; i++)
            {
                CkLcourse.Items[i].Selected = true;
            }
            Textcourse.Text = "Course(" + (CkLcourse.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < CkLcourse.Items.Count; i++)
            {
                CkLcourse.Items[i].Selected = false;
            }
            Textcourse.Text = "---Select---";
        }
        Showgrid.Visible = false;
        divMainContents.Visible = false;
        printtable.Visible = false;
    }

    protected void CkLcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        Pcourse.Focus();
        Checkcourse.Checked = false;
        Textcourse.Text = "---Select---";
        int corcount = 0;
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < CkLcourse.Items.Count; i++)
        {
            if (CkLcourse.Items[i].Selected == true)
            {
                value = CkLcourse.Items[i].Text;
                code = CkLcourse.Items[i].Value.ToString();
                corcount = corcount + 1;
            }
        }
        if (corcount > 0)
        {
            Textcourse.Text = "Course(" + corcount.ToString() + ")";
            if (corcount == CkLcourse.Items.Count)
            {
                Checkcourse.Checked = true;
            }
        }
        subjectcnt = corcount;
        Showgrid.Visible = false;
        divMainContents.Visible = false;
        printtable.Visible = false;
    }
    #endregion

    #region Subject
    public void BindSubjecttest(string strbatch, string strbranch, string strsem, string strsec)
    {
        try
        {

            if (ddlsection.Text.ToString() == "All" || ddlsection.Text.ToString() == string.Empty || ddlsection.Text.ToString() == "-1")
            {
                strsec = string.Empty;
                strsec1 = string.Empty;
            }
            else
            {
                strsec = " and registration.sections='" + ddlsection.Text.ToString() + "'";
                strsec1 = " and sections='" + ddlsection.Text.ToString() + "'";
            }
            strbatch = ddlbatch.SelectedValue.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            strsem = ddlsemester.SelectedValue.ToString();
            dsmethodgosubj.Dispose();
            dsmethodgosubj.Reset();
            if (Session["Staff_Code"].ToString() == "")
            {
                dsmethodgosubj = d2.BindSubjecttest(strbatch, strbranch, strsem, strsec);
            }
            else if (Session["Staff_Code"].ToString() != "")
            {
                dsmethodgosubj = d2.BindparticularstaffSubject(strbatch, strbranch, strsem, strsec, Session["Staff_Code"].ToString());
            }
            if (dsmethodgosubj.Tables[0].Rows.Count > 0)
            {
                ddl_subject.DataSource = dsmethodgosubj;
                ddl_subject.DataTextField = "subject_name";
                ddl_subject.DataValueField = "subject_no";
                ddl_subject.DataBind();
                htb.Clear();
                htsubjcide.Clear();
                //chklstsubject.SelectedIndex = chklstsubject.Items.Count - 1;
                for (int i = 0; i < ddl_subject.Items.Count; i++)
                {
                    string subjno = "", subjtype = "", subjcode = string.Empty;
                    subjno = dsmethodgosubj.Tables[0].Rows[i]["subject_no"].ToString();
                    subjtype = dsmethodgosubj.Tables[0].Rows[i]["subject_type"].ToString();
                    subjcode = dsmethodgosubj.Tables[0].Rows[i]["subject_code"].ToString();
                    if (htb.Contains(Convert.ToString(subjno)))
                    {
                        string subjtypeve = Convert.ToString(GetCorrespondingKey(Convert.ToString(subjno), htb));
                        htb[Convert.ToString(subjno)] = subjtypeve;
                    }
                    else
                    {
                        htb.Add(Convert.ToString(subjno), subjtype);
                    }
                    if (htsubjcide.Contains(Convert.ToString(subjno)))
                    {
                        string subjcodeve = Convert.ToString(GetCorrespondingKey(Convert.ToString(subjno), htsubjcide));
                        htsubjcide[Convert.ToString(subjno)] = subjcodeve;
                    }
                    else
                    {
                        htsubjcide.Add(Convert.ToString(subjno), subjcode);
                    }

                }


            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddl_subject_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        printtable.Visible = false;
        Showgrid.Visible = false;
        divMainContents.Visible = false;
        Bindtest();

    }

    #endregion

    #region Test
    public void Bindtest()
    {
        try
        {

            txttest.Text = "--Select--";
            chktest.Checked = false;
            cbltest.Items.Clear();
            DataSet titles = new DataSet();
            string sems = string.Empty;
            int selSem = 0;
            string semester = string.Empty;
            string subjectno = string.Empty;

            string sections = string.Empty;
            string strsec = string.Empty;

            string degreecode = string.Empty;
            string collegecode = string.Empty;
            string batchyear = string.Empty;

            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value).Trim();
            }
            if (ddlbatch.Items.Count > 0)
            {
                batchyear = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
            }
            if (ddlbranch.Items.Count > 0)
            {
                degreecode = Convert.ToString(ddlbranch.SelectedItem.Value).Trim();
            }

            if (ddl_subject.Items.Count > 0)
            {
                subjectno = Convert.ToString(ddl_subject.SelectedValue).Trim();
            }

            if (ddlsection.Items.Count > 0)
            {
                sections = Convert.ToString(ddlsection.SelectedValue).Trim();
                if (Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "" || Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "-1")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and isnull(ltrim(rtrim(r.Sections)),'')='" + Convert.ToString(sections).Trim() + "'";
                }
            }
            if (ddlsemester.Items.Count > 0)
            {
                semester = Convert.ToString(ddlsemester.SelectedValue).Trim();
            }

            if (!string.IsNullOrEmpty(semester.Trim()) && !string.IsNullOrEmpty(subjectno) && !string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batchyear) && !string.IsNullOrEmpty(degreecode))
            {
                sems = " and s.semester in(" + semester + ")";
                string Sqlstr = "select distinct c.criteria,c.criteria_no from criteriaforinternal c,registration r,syllabus_master s,Exam_type et,subject sub where et.batch_year=r.Batch_Year and et.criteria_no=c.Criteria_no and et.subject_no=sub.subject_no and sub.syll_code=s.syll_code and sub.syll_code=c.syll_code and  r.degree_code=s.degree_code and r.batch_year=s.batch_year and c.syll_code=s.syll_code and cc=0 and delflag=0 and r.exam_flag<>'debar' and r.batch_year='" + batchyear + "' and r.college_code='" + collegecode + "' and sub.subject_no='" + subjectno + "' and r.degree_code in(" + degreecode + ") " + sems + strsec + " order by c.criteria,c.criteria_no asc";
                titles.Clear();
                titles.Dispose();
                titles = d2.select_method_wo_parameter(Sqlstr, "Test");
            }
            if (titles.Tables.Count > 0 && titles.Tables[0].Rows.Count > 0)
            {
                cbltest.DataSource = titles;
                cbltest.DataValueField = "criteria_no";
                cbltest.DataTextField = "criteria";
                cbltest.DataBind();
            }

            if (cbltest.Items.Count > 0)
            {
                for (int row = 0; row < cbltest.Items.Count; row++)
                {
                    cbltest.Items[row].Selected = true;
                    chktest.Checked = true;
                }
                txttest.Text = "Test(" + cbltest.Items.Count + ")";
            }
            else
            {
                txttest.Text = "--Select--";
            }


        }
        catch (Exception ex)
        {

        }
    }

    protected void chktest_CheckedChanged(object sender, EventArgs e)
    {
        if (chktest.Checked == true)
        {
            for (int i = 0; i < cbltest.Items.Count; i++)
            {
                cbltest.Items[i].Selected = true;
            }
            txttest.Text = "Test(" + (cbltest.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbltest.Items.Count; i++)
            {
                cbltest.Items[i].Selected = false;
            }
            txttest.Text = "---Select---";
        }
        Showgrid.Visible = false;
        divMainContents.Visible = false;
        printtable.Visible = false;
    }

    protected void cbltest_SelectedIndexChanged(object sender, EventArgs e)
    {
        chktest.Checked = false;
        txttest.Text = "---Select---";

        int subjectcount = 0;
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < cbltest.Items.Count; i++)
        {
            if (cbltest.Items[i].Selected == true)
            {
                value = cbltest.Items[i].Text;
                code = cbltest.Items[i].Value.ToString();
                subjectcount = subjectcount + 1;
            }
        }
        if (subjectcount > 0)
        {
            txttest.Text = "Test(" + subjectcount.ToString() + ")";
            if (subjectcount == cbltest.Items.Count)
            {
                chktest.Checked = true;
            }
        }
        subjectcnt = subjectcount;
        Showgrid.Visible = false;
        divMainContents.Visible = false;
        printtable.Visible = false;
    }
    #endregion

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value.ToString();
            }
        }
        return null;
    }

    #region GO
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            
            if (rbtformate2.Checked)
            {
                LabMarkReport();
            }
            else if (rbtformate3.Checked)
            {
                formate3();
            }
            else
            {
                diccoursehead.Clear();
                diccoursevalue.Clear();
                dictotper.Clear();
                string couseid = "";
                string subcode = "";
                string tstcode = "";
                string course = "";
                string subjectno = "";
                string textcode = "";
                string coursename = "";
                string partname = "";
                string textname = "";
                DataSet getstudet = new DataSet();
                DataSet dspart = new DataSet();
                DataView dvpart = new DataView();
                int col = 0;
                data.Clear();
                string sections = string.Empty;
                string strsec = string.Empty;
                string degreecode = string.Empty;
                string collegecode = string.Empty;
                string batchyear = string.Empty;
                string semester = string.Empty;

                if (ddlcollege.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value).Trim();
                }
                if (ddlbatch.Items.Count > 0)
                {
                    batchyear = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
                }
                if (ddlbranch.Items.Count > 0)
                {
                    degreecode = Convert.ToString(ddlbranch.SelectedItem.Value).Trim();
                }
                if (ddlsection.Items.Count > 0)
                {
                    sections = Convert.ToString(ddlsection.SelectedValue).Trim();

                    if (Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "" || Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "-1")
                    {
                        strsec = "";
                    }
                    else
                    {
                        strsec = "  and isnull(ltrim(rtrim(r.Sections)),'')='" + Convert.ToString(sections).Trim() + "'";
                    }
                }
                if (ddlsemester.Items.Count > 0)
                {
                    semester = Convert.ToString(ddlsemester.SelectedValue).Trim();
                }

                DataSet dsst = new DataSet();
                Hashtable hat = new Hashtable();
                Hashtable hat1 = new Hashtable();
                string stddet = "select  r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Name,r.App_no  from Registration r where r.batch_year='" + batchyear + "' and r.degree_code='" + degreecode + "' and r.Current_Semester='" + semester + "' " + strsec + "  and  RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR'  order by r.reg_no,r.roll_no";//and isnull(ltrim(rtrim(r.Sections)),'')='A'

                dsst = d2.select_method_wo_parameter(stddet, "Text");

                if (dsst.Tables.Count > 0 && dsst.Tables[0].Rows.Count > 0)
                {
                    if (CkLcourse.Items.Count > 0)
                        couseid = Convert.ToString(rs.getCblSelectedValue(CkLcourse));
                    if (ddl_subject.Items.Count > 0)
                        subcode = Convert.ToString(ddl_subject.SelectedValue).Trim();
                    if (cbltest.Items.Count > 0)
                        tstcode = Convert.ToString(rs.getCblSelectedValue(cbltest));

                    if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(couseid) && !string.IsNullOrEmpty(subcode) && !string.IsNullOrEmpty(tstcode))
                    {
                        string sqlqry = "select distinct im.app_no,im.ExamCode from CAQuesSettingsParent ca,NewInternalMarkEntry im,Exam_type et where ca.MasterID=im.MasterID and ca.subjectNo=et.subject_no and im.ExamCode=et.exam_code and ca.CourseOutComeNo in('" + couseid + "') and ca.subjectno in('" + subcode + "') and ca.CriteriaNo in('" + tstcode + "')";

                        sqlqry += "select distinct PartNo,CourseOutComeNo,subjectno,CriteriaNo from CAQuesSettingsParent  where CourseOutComeNo in('" + couseid + "' )  and subjectno in('" + subcode + "') and CriteriaNo in('" + tstcode + "')";
                        sqlqry += "select * from CAQuesSettingsParent  where CourseOutComeNo in('" + couseid + "' )  and subjectno in('" + subcode + "') and CriteriaNo in('" + tstcode + "')";

                        getstudet.Clear();

                        getstudet = d2.select_method_wo_parameter(sqlqry, "text");

                        DataSet dstd = new DataSet();

                        //string stdmark = "select SUM(im.marks) mark,examcode,app_no,ca.CourseOutComeNo,ca.PartNo,(select isnull(template,'') from Master_Settings where settings='COSettings' and masterno=ca.CourseOutComeNo) as CourseoutCome from CAQuesSettingsParent ca,NewInternalMarkEntry im where ca.MasterID=im.MasterID and subjectno in('" + subcode + "') and CriteriaNo in('" + tstcode + "') and (marks<>-1 and marks<>-16 and marks<>-20) group by examcode,app_no,ca.CourseOutComeNo,ca.PartNo ";

                        string stdmark = "select SUM(im.marks) mark,examcode,app_no,ca.CourseOutComeNo,ca.PartNo,(select isnull(template,'') from Master_Settings where settings='COSettings' and masterno=ca.CourseOutComeNo) as CourseoutCome,c.criteria from CAQuesSettingsParent ca,NewInternalMarkEntry im,criteriaforInternal c where ca.MasterID=im.MasterID and subjectno in('" + subcode + "') and CriteriaNo in('" + tstcode + "') and (marks<>-1 and marks<>-16 and marks<>-20) and c.criteria_no=ca.criteriano  group by examcode,app_no,ca.CourseOutComeNo,ca.PartNo,c.criteria";

                        dstd = d2.select_method_wo_parameter(stdmark, "Text");

                        //DataTable dtQsettings = dir.selectDataTable("select * from CAQuesSettingsParent  where CourseOutComeNo in('" + couseid + "' )  and subjectno in('" + subcode + "') and CriteriaNo in('" + tstcode + "')");

                        DataTable dtQsettings = dir.selectDataTable("select (select isnull(template,'') from Master_Settings where settings='COSettings' and masterno=ca.CourseOutComeNo) as CourseoutCome,ca.partno,ca.qno,ca.mark,ca.sub1,c.criteria from CAQuesSettingsParent ca,criteriaforInternal c  where ca.CourseOutComeNo in('" + couseid + "')  and ca.subjectno in('" + subcode + "') and ca.CriteriaNo in('" + tstcode + "') and c.criteria_no=ca.criteriano ");

                        DataTable dicPart = dtQsettings.DefaultView.ToTable(true, "partNo", "qno");
                        DataTable dicPartCo = dtQsettings.DefaultView.ToTable(true, "partNo", "CourseoutCome", "criteria");
                        DataTable dicQSub = dtQsettings.DefaultView.ToTable(true, "partNo", "CourseoutCome", "criteria", "qno", "sub1");



                        if (getstudet.Tables.Count > 0 && getstudet.Tables[0].Rows.Count > 0)
                        {

                            ArrayList arrColHdrNames1 = new ArrayList();
                            ArrayList arrColHdrNames2 = new ArrayList();
                            ArrayList arrColHdrNames3 = new ArrayList();
                            ArrayList arrColHdrNames4 = new ArrayList();

                            arrColHdrNames1.Add("S.No");
                            arrColHdrNames2.Add("S.No");
                            arrColHdrNames3.Add("S.No");
                            arrColHdrNames4.Add("S.No");
                            data.Columns.Add("col 0");
                            //if (Convert.ToString(Session["Rollflag"]) == "1")
                            //{
                            arrColHdrNames1.Add("Roll No");
                            arrColHdrNames2.Add("Roll No");
                            arrColHdrNames3.Add("Roll No");
                            arrColHdrNames4.Add("Roll No");

                            col++;
                            data.Columns.Add("col " + col);                         
                            arrColHdrNames1.Add("Register No");
                            arrColHdrNames2.Add("Register No");
                            arrColHdrNames3.Add("Register No");
                            arrColHdrNames4.Add("Register No");

                            col++;
                            data.Columns.Add("col " + col);                           
                            col = col + 1;
                            colcnt = col + 1;
                            arrColHdrNames1.Add("Student Name");
                            arrColHdrNames2.Add("Student Name");
                            arrColHdrNames3.Add("Student Name");
                            arrColHdrNames4.Add("Student Name");
                            data.Columns.Add("col " + col);

                            int colHdrIndx = colcnt;

                            for (int cou = 0; cou < CkLcourse.Items.Count; cou++)
                            {
                                if (CkLcourse.Items[cou].Selected == true)
                                {
                                    course = CkLcourse.Items[cou].Value;
                                    coursename = CkLcourse.Items[cou].Text;
                                    for (int test = 0; test < cbltest.Items.Count; test++)
                                    {
                                        if (cbltest.Items[test].Selected == true)
                                        {
                                            textcode = cbltest.Items[test].Value;
                                            textname = cbltest.Items[test].Text;

                                            dicPartCo.DefaultView.RowFilter = "CourseoutCome='" + coursename + "'  and criteria='" + textname + "' ";//and CriteriaNo='" + textcode + "'

                                            DataView dvipart = dicPartCo.DefaultView;
                                            if (dvipart.Count > 0)
                                            {
                                                partname = "";
                                                double totalmark = 0;
                                                for (int p = 0; p < dvipart.Count; p++)
                                                {
                                                    {
                                                        partname = Convert.ToString(dvipart[p]["PartNo"]);
                                                        arrColHdrNames1.Add(coursename);
                                                        arrColHdrNames2.Add(textname);
                                                        arrColHdrNames3.Add("Part " + Convert.ToString(dvipart[p]["PartNo"]));
                                                        data.Columns.Add("col " + colHdrIndx);
                                                        colHdrIndx++;
                                                        col++;
                                                        diccoursevalue.Add(col, course + "$" + subjectno + "$" + textcode + "$" + Convert.ToString(dvipart[p]["PartNo"]));
                                                        //}
                                                    }
                                                }

                                                diccoursehead.Add(col, coursename + "$" + textname + "$" + partname);

                                                arrColHdrNames1.Add(coursename);
                                                arrColHdrNames2.Add("Total");
                                                arrColHdrNames3.Add("Total");                                   
                                                data.Columns.Add("col " + colHdrIndx);
                                                colHdrIndx++;                                           
                                                col = col + 2;
                                            }
                                        }
                                    }
                                    arrColHdrNames1.Add(coursename);
                                    arrColHdrNames1.Add(coursename);
                                    data.Columns.Add("col " + colHdrIndx);
                                    colHdrIndx++;
                                    arrColHdrNames2.Add("Grand Total");
                                    arrColHdrNames3.Add("Grand Total");
                                    data.Columns.Add("col " + colHdrIndx);
                                    colHdrIndx++;
                                    arrColHdrNames2.Add("%");
                                    arrColHdrNames3.Add("%");
                                  
                                }
                            }

                            DataRow drHdr1 = data.NewRow();
                            DataRow drHdr2 = data.NewRow();
                            DataRow drHdr3 = data.NewRow();
                            //DataRow drHdr4 = data.NewRow();

                            for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                            {

                                drHdr1["col " + grCol] = arrColHdrNames1[grCol];
                                drHdr2["col " + grCol] = arrColHdrNames2[grCol];
                                drHdr3["col " + grCol] = arrColHdrNames3[grCol];
                                //drHdr4["col " + grCol] = arrColHdrNames4[grCol];

                            }
                            data.Rows.Add(drHdr1);
                            data.Rows.Add(drHdr2);
                            data.Rows.Add(drHdr3);
                            // data.Rows.Add(drHdr4);

                            if (data.Columns.Count > 0)
                            {
                                drow = data.NewRow();
                                data.Rows.Add(drow);

                                double totalmark = 0;
                                double finaltotalmark1 = 0;
                                string partNo = string.Empty;
                                string testtxt = string.Empty;
                                for (int j = 0; j < data.Columns.Count; j++)
                                {

                                    double partSum = 0;
                                    string coName = Convert.ToString(data.Rows[0][j]);
                                    string TestName = Convert.ToString(data.Rows[1][j]);
                                    string Subtype = Convert.ToString(data.Rows[2][j]);

                                    if (Subtype.Trim().ToLower() == "s.no")
                                        data.Rows[data.Rows.Count - 1][j] = "S.No";
                                    else if (Subtype.Trim().ToLower() == "roll no")
                                        data.Rows[data.Rows.Count - 1][j] = "Roll No";
                                    else if (Subtype.Trim().ToLower() == "register no")
                                        data.Rows[data.Rows.Count - 1][j] = "Register No";
                                    else if (Subtype.Trim().ToLower() == "student name")
                                        data.Rows[data.Rows.Count - 1][j] = "Student Name";
                                    else if (Subtype.Trim().ToLower() == "total")
                                    {
                                        if (!hat.ContainsKey(coName + "-" + testtxt + "-" + partNo))
                                        {
                                            hat.Add(coName + "-" + testtxt + "-" + partNo, totalmark);
                                            data.Rows[data.Rows.Count - 1][j] = totalmark;
                                            finaltotalmark1 += totalmark;
                                            totalmark = 0;
                                            partNo = string.Empty;
                                            testtxt = string.Empty;
                                        }                                       
                                    }
                                    else if (Subtype.Trim().ToLower() == "grand total")
                                    {
                                        if (!hat1.ContainsKey(coName))
                                        {
                                            hat1.Add(coName, finaltotalmark1);
                                            data.Rows[data.Rows.Count - 1][j] = finaltotalmark1;
                                            finaltotalmark1 = 0;
                                        }
                                    }
                                    else if (Subtype.Trim().ToLower() == "%")
                                        data.Rows[data.Rows.Count - 1][j] = "%";
                                    else
                                    {
                                        string[] pNo = Subtype.Split(' ');
                                        partNo = Subtype;
                                        testtxt = TestName;
                                        dtQsettings.DefaultView.RowFilter = "CourseoutCome='" + coName + "'  and criteria='" + TestName + "' and PartNo='" + Convert.ToString(pNo[1]) + "'";//and CriteriaNo='" + textcode + "'

                                        DataTable dvipart = dtQsettings.DefaultView.ToTable();
                                        if (dvipart.Rows.Count > 0)
                                        {
                                            DataTable dtPar = dvipart.DefaultView.ToTable(true, "Qno");

                                            if (dtPar.Rows.Count > 0)
                                            {
                                                foreach (DataRow dr in dtPar.Rows)
                                                {
                                                    string Qno = Convert.ToString(dr["Qno"]);
                                                    dicQSub.DefaultView.RowFilter = "CourseoutCome='" + coName + "'  and criteria='" + TestName + "' and PartNo='" + Convert.ToString(pNo[1]) + "' and Qno='" + Qno + "'";
                                                    DataTable dicP = dicQSub.DefaultView.ToTable();
                                                    object sum = dtQsettings.Compute("Sum(Mark)", "Qno='" + Qno + "' and CourseoutCome='" + coName + "' and criteria='" + TestName + "' and PartNo='" + Convert.ToString(pNo[1]) + "' and Qno='" + Qno + "'");

                                                    totalmark = totalmark + (Convert.ToDouble(sum) / dicP.Rows.Count);
                                                    partSum = partSum + (Convert.ToDouble(sum) / dicP.Rows.Count);
                                                }
                                            }

                                            data.Rows[data.Rows.Count - 1][j] = partSum.ToString();
                                            partSum = 0;
                                        }
                                    }
                                }
                                totalmark = 0;
                                double finaltotmark = 0;
                                partNo = string.Empty;
                                testtxt = string.Empty;
                                if (dstd.Tables.Count > 0 & dstd.Tables[0].Rows.Count > 0)
                                {
                                    int sno = 0;
                                    for (int std = 0; std < dsst.Tables[0].Rows.Count; std++)
                                    {
                                        sno++;
                                        finaltotmark = 0;
                                        string appno = Convert.ToString(dsst.Tables[0].Rows[std]["app_no"]);
                                        string RollNo = Convert.ToString(dsst.Tables[0].Rows[std]["Roll_No"]);
                                        string RegNo = Convert.ToString(dsst.Tables[0].Rows[std]["Reg_No"]);
                                        string Name = Convert.ToString(dsst.Tables[0].Rows[std]["Stud_Name"]);
                                        drow = data.NewRow();
                                        data.Rows.Add(drow);
                                        for (int j = 0; j < data.Columns.Count; j++)
                                        {
                                            string coName = Convert.ToString(data.Rows[0][j]);
                                            string TestName = Convert.ToString(data.Rows[1][j]);
                                            string Subtype = Convert.ToString(data.Rows[2][j]);
                                            if (Subtype.Trim().ToLower() == "s.no")
                                                data.Rows[data.Rows.Count - 1][j] = sno.ToString();
                                            else if (Subtype.Trim().ToLower() == "roll no")
                                                data.Rows[data.Rows.Count - 1][j] = RollNo;
                                            else if (Subtype.Trim().ToLower() == "register no")
                                                data.Rows[data.Rows.Count - 1][j] = RegNo;
                                            else if (Subtype.Trim().ToLower() == "student name")
                                                data.Rows[data.Rows.Count - 1][j] = Name;
                                            else if (Subtype.Trim().ToLower() == "total")
                                            {
                                                if (totalmark > 0)
                                                    data.Rows[data.Rows.Count - 1][j] = Convert.ToString(totalmark);
                                                else
                                                    data.Rows[data.Rows.Count - 1][j] = Convert.ToString("-");
                                                //totalmark = 0;
                                                if (totalmark > 0)
                                                    finaltotmark += totalmark;

                                                totalmark = 0;
                                            }
                                            else if (Subtype.Trim().ToLower() == "grand total")
                                            {
                                                if(finaltotmark>0)
                                                    data.Rows[data.Rows.Count - 1][j] = Convert.ToString(finaltotmark);
                                                else
                                                    data.Rows[data.Rows.Count - 1][j] = Convert.ToString("-");

                                               // finaltotmark = 0;
                                            }
                                            else if (Subtype.Trim().ToLower() == "%")
                                            {
                                                string tot = Convert.ToString(hat1[coName]);
                                                double sumtot = 0;
                                                double.TryParse(tot, out sumtot);
                                                double per = 0;
                                                if (sumtot > 0 && finaltotmark > 0)
                                                {
                                                    per = (finaltotmark / sumtot);
                                                    per = per * 100;
                                                    per = Math.Round(per, 0, MidpointRounding.AwayFromZero);
                                                    data.Rows[data.Rows.Count - 1][j] = per.ToString();
                                                }
                                                else
                                                    data.Rows[data.Rows.Count - 1][j] = "-";

                                                finaltotmark = 0;
                                                partNo = string.Empty;
                                                testtxt = string.Empty;
                                            }
                                            else
                                            {
                                                string[] s = Subtype.Split(' ');
                                                partNo = Subtype;
                                                testtxt = TestName;
                                                if (dstd.Tables.Count > 0 && dstd.Tables[0].Rows.Count > 0 && s.Length > 1)
                                                {
                                                    double sumVal = 0;
                                                    //dstd.Tables[0].DefaultView.RowFilter = "CourseoutCome='" + coName + "' and criteria='" + TestName + "' and PartNo='" +Convert.ToString(s[1]) + "'";
                                                    // DataTable dtstuMark=dstd.Tables[0].DefaultView.ToTable();
                                                    object sum = dstd.Tables[0].Compute("Sum(Mark)", "CourseoutCome='" + coName + "' and criteria='" + TestName + "' and PartNo='" + Convert.ToString(s[1]) + "' and app_no='" + appno + "'");
                                                    double.TryParse(Convert.ToString(sum), out sumVal);
                                                    totalmark = totalmark + sumVal;
                                                    data.Rows[data.Rows.Count - 1][j] = Convert.ToString(sum);
                                                }
                                            }
                                        }
                                    }

                                    Showgrid.DataSource = data;
                                    Showgrid.DataBind();
                                    Showgrid.Visible = true;
                                    divMainContents.Visible = true;
                                    printtable.Visible = true;
                                }

                            }


                            #region old
                            //int sno = 0;
                            //if (dstd.Tables.Count > 0 & dstd.Tables[0].Rows.Count > 0)
                            //{
                            //    for (int std = 0; std < dsst.Tables[0].Rows.Count; std++)
                            //    {
                            //        sno++;

                            //        string appno = Convert.ToString(dsst.Tables[0].Rows[std]["app_no"]);


                            //        getstudet.Tables[0].DefaultView.RowFilter = "App_no='" + appno + "'";
                            //        DataView dvStudent = getstudet.Tables[0].DefaultView;
                            //        drow = data.NewRow();
                            //        data.Rows.Add(drow);
                            //        if (dvStudent.Count > 0)
                            //        {
                            //            string ExamCode = Convert.ToString(dvStudent[0]["ExamCode"]);
                            //            int c = 0;
                            //            data.Rows[data.Rows.Count - 1][c] = Convert.ToString(sno);
                            //            if (Convert.ToString(Session["Rollflag"]) == "1")
                            //            {
                            //                c++;
                            //                data.Rows[data.Rows.Count - 1][c] = Convert.ToString(dsst.Tables[0].Rows[std]["Roll_No"]);


                            //            }
                            //            if (Convert.ToString(Session["Regflag"]) == "1")
                            //            {
                            //                c++;
                            //                data.Rows[data.Rows.Count - 1][c] = Convert.ToString(dsst.Tables[0].Rows[std]["Reg_No"]);

                            //            }

                            //            if (Convert.ToString(Session["AdmissionNo"]) == "1")
                            //            {
                            //                c++;
                            //                data.Rows[data.Rows.Count - 1][c] = Convert.ToString(dsst.Tables[0].Rows[std]["Roll_Admit"]);

                            //            }
                            //            c++;
                            //            data.Rows[data.Rows.Count - 1][c] = Convert.ToString(dsst.Tables[0].Rows[std]["Stud_Name"]);

                            //            double total = 0;
                            //            double per = 0;
                            //            double totalmar = 0;
                            //            string vale = "";
                            //            for (int cou = colcnt; cou < col; cou++)
                            //            {
                            //                if (diccoursevalue.ContainsKey(cou))
                            //                {
                            //                    string subdet = diccoursevalue[cou];
                            //                    string[] spilt = subdet.Split('$');
                            //                    dstd.Tables[0].DefaultView.RowFilter = "examcode='" + ExamCode + "' and app_no='" + appno + "' and CourseOutComeNo='" + spilt[0].ToString() + "' and  PartNo ='" + spilt[3].ToString() + "'";
                            //                    DataView dvStud = dstd.Tables[0].DefaultView;

                            //                    if (dvStud.Count > 0)
                            //                    {
                            //                        string value = Convert.ToString(dvStud[0]["mark"]);
                            //                        if (value != "")
                            //                        {
                            //                            if (Convert.ToDouble(value) >= 0)
                            //                            {
                            //                                vale = value;

                            //                                string totmark = data.Rows[3][cou].ToString();
                            //                                if (totmark.Trim().All(char.IsNumber))
                            //                                    totalmar = totalmar + Convert.ToDouble(totmark);

                            //                                total = total + Convert.ToDouble(vale);
                            //                                data.Rows[data.Rows.Count - 1][cou] = value;
                            //                            }
                            //                            else
                            //                            {
                            //                                value = "-1";
                            //                                string mark = checkStatus(value).ToString();
                            //                                data.Rows[data.Rows.Count - 1][cou] = mark;
                            //                            }

                            //                        }
                            //                        else
                            //                        {
                            //                            data.Rows[data.Rows.Count - 1][cou] = "";
                            //                        }
                            //                    }
                            //                }
                            //                else
                            //                {

                            //                    if (totalmar != 0 && total != 0)
                            //                    {
                            //                        per = total / totalmar;
                            //                        double pertage = per * 100;
                            //                        string pert = Convert.ToString(Math.Round(Convert.ToDouble(pertage), 2));
                            //                        data.Rows[data.Rows.Count - 1][cou] = total;
                            //                        data.Rows[data.Rows.Count - 1][cou + 1] = pert;
                            //                    }
                            //                    else
                            //                    {
                            //                        data.Rows[data.Rows.Count - 1][cou] = "";
                            //                        data.Rows[data.Rows.Count - 1][cou + 1] = "";
                            //                    }
                            //                    cou++;
                            //                    total = 0;
                            //                    totalmar = 0;
                            //                }


                            //            }

                            //        }
                            //        else
                            //        {
                            //            int c = 0;
                            //            data.Rows[data.Rows.Count - 1][c] = Convert.ToString(sno);
                            //            if (Convert.ToString(Session["Rollflag"]) == "1")
                            //            {
                            //                c++;
                            //                data.Rows[data.Rows.Count - 1][c] = Convert.ToString(dsst.Tables[0].Rows[std]["Roll_No"]);


                            //            }
                            //            if (Convert.ToString(Session["Regflag"]) == "1")
                            //            {
                            //                c++;
                            //                data.Rows[data.Rows.Count - 1][c] = Convert.ToString(dsst.Tables[0].Rows[std]["Reg_No"]);

                            //            }

                            //            if (Convert.ToString(Session["AdmissionNo"]) == "1")
                            //            {
                            //                c++;
                            //                data.Rows[data.Rows.Count - 1][c] = Convert.ToString(dsst.Tables[0].Rows[std]["Roll_Admit"]);

                            //            }
                            //            c++;
                            //            data.Rows[data.Rows.Count - 1][c] = Convert.ToString(dsst.Tables[0].Rows[std]["Stud_Name"]);
                            //            for (int cou = colcnt; cou < col; cou++)
                            //            {

                            //            }



                            //        }
                            //    }
                            //}

                            #endregion

                            if (data.Columns.Count > 0)
                            {
                                Showgrid.DataSource = data;
                                Showgrid.DataBind();
                                Showgrid.Visible = true;
                                divMainContents.Visible = true;
                                printtable.Visible = true;

                                int rowcnt = Showgrid.Rows.Count - 4;
                                //Rowspan
                                for (int rowIndex = Showgrid.Rows.Count - rowcnt - 1; rowIndex >= 0; rowIndex--)
                                {
                                    GridViewRow row = Showgrid.Rows[rowIndex];
                                    GridViewRow previousRow = Showgrid.Rows[rowIndex + 1];
                                    Showgrid.Rows[rowIndex].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                    Showgrid.Rows[rowIndex].Font.Bold = true;
                                    Showgrid.Rows[rowIndex].HorizontalAlign = HorizontalAlign.Center;
                                    if (rowIndex == 3)
                                    {
                                        for (int i = 0; i < colcnt; i++)
                                        {
                                            if (row.Cells[i].Text == previousRow.Cells[i].Text)
                                            {

                                                row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                                       previousRow.Cells[i].RowSpan + 1;
                                                previousRow.Cells[i].Visible = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; i < row.Cells.Count; i++)
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

                                //ColumnSpan
                                for (int rowIndex = Showgrid.Rows.Count - rowcnt - 2; rowIndex >= 0; rowIndex--)
                                {


                                    for (int cell = Showgrid.Rows[rowIndex].Cells.Count - 1; cell > 0; cell--)
                                    {
                                        TableCell colum = Showgrid.Rows[rowIndex].Cells[cell];
                                        TableCell previouscol = Showgrid.Rows[rowIndex].Cells[cell - 1];
                                        if (colum.Text == previouscol.Text)
                                        {
                                            if (previouscol.ColumnSpan == 0)
                                            {
                                                if (colum.ColumnSpan == 0)
                                                {
                                                    previouscol.ColumnSpan += 2;

                                                }
                                                else
                                                {
                                                    previouscol.ColumnSpan += colum.ColumnSpan + 1;

                                                }
                                                colum.Visible = false;

                                            }
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            Showgrid.Visible = false;
                            divMainContents.Visible = false;
                            printtable.Visible = false;
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);

                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Select All The Feild')", true);
                        return;
                    }
                }
                else
                {
                    Showgrid.Visible = false;
                    divMainContents.Visible = false;
                    printtable.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Student(s) Found!')", true);
                }
            }
        }
        catch
        {

        }

    }

    #endregion

    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    e.Row.Cells[grCol].Visible = false;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                for (int j = colcnt; j < data.Columns.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }



        }
        catch
        {
        }
    }

    protected string checkStatus(string mark)
    {
        try
        {
            if (mark == "-1")
                return "AAA";
            else if (mark == "-20")  //added by Mullai
                return " ";
            else
                return mark;
        }
        catch
        {
            return null;
        }
    }

    public void Excel_OnClick(object sender, EventArgs e)
    {
        try
        {
            string report = txtreptname.Text;

            if (report.ToString().Trim() != "")
            {

                d2.printexcelreportgrid(Showgrid, report);
                Label1.Visible = false;
            }
            else
            {
                Label1.Text = "Please Enter Your Report Name";
                Label1.Visible = true;
            }

        }
        catch (Exception ex)
        {
            Label1.Text = ex.ToString();
        }
    }


    //protected void Excel_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string report = txtreptname.Text;
    //        if (report.ToString().Trim() != "")
    //        {
    //            d2.printexcelreportgrid(Showgrid, report);

    //        }
    //        else
    //        {
    //            Label1.Text = "Please Enter Your Report Name";
    //            Label1.Visible = true;
    //        }

    //    }

    //    catch (Exception ex)
    //    {
    //        Label1.Visible = true;
    //        Label1.Text = ex.ToString();
    //    }
    //}

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void Print_OnClick(object sender, EventArgs e)
    {
        try
        {

            string ss = Session["usercode"].ToString();
            string degreedetails = "Course Outcomes Based Report - 1";
            string pagename = "Student_Performance_Report.aspx";
            NEWPrintMater1.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
            ////Printcontrol.loadspreaddetails(attnd_report, pagename, degreedetails);
            NEWPrintMater1.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "checkmain", "checkmain();", true);
        }
        catch (Exception ex)
        {

        }
    }

    public void LabMarkReport()
    {
        try
        {
            string sections = string.Empty;
            string strsec = string.Empty;
            string degreecode = string.Empty;
            string collegecode = string.Empty;
            string batchyear = string.Empty;
            string semester = string.Empty;

            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value).Trim();
            }
            if (ddlbatch.Items.Count > 0)
            {
                batchyear = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
            }
            if (ddlbranch.Items.Count > 0)
            {
                degreecode = Convert.ToString(ddlbranch.SelectedItem.Value).Trim();
            }
            if (ddlsection.Items.Count > 0)
            {
                sections = Convert.ToString(ddlsection.SelectedValue).Trim();
                if (Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "" || Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "-1")
                {
                    strsec = "";
                }
                else
                {
                    strsec = "  and isnull(ltrim(rtrim(r.Sections)),'')='" + Convert.ToString(sections).Trim() + "'";
                }
            }
            if (ddlsemester.Items.Count > 0)
            {
                semester = Convert.ToString(ddlsemester.SelectedValue).Trim();
            }
            string Cou = string.Empty;
            string test = string.Empty;
            if (CkLcourse.Items.Count > 0)
                Cou = rs.getCblSelectedValue(CkLcourse);
            if (cbltest.Items.Count > 0)
                test = rs.getCblSelectedValue(cbltest);



            string SubNo = Convert.ToString(ddl_subject.SelectedValue).Trim();
            string stddet = "select distinct r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Name,r.App_no  from Registration r where r.batch_year='" + batchyear + "' and r.degree_code='" + degreecode + "' and r.Current_Semester='" + semester + "' " + strsec + "  and  RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' order by r.reg_no,r.roll_no ";//and app_no='11855'
            DataTable dsst = dir.selectDataTable(stddet);

            string SelectMark = "select r.reg_no,r.roll_no,r.App_no,r.Stud_Name,sm.testmark,sm.retestmark,(select isnull(template,'') from Master_Settings where settings='COSettings' and masterno=se.coNo) as CourseoutCome,se.coNo,e.exam_code,e.criteria_No,e.subject_no,c.criteria,se.subsubjectName from registration r,subSubjectWiseMarkEntry sm,subsubjectTestDetails se,exam_type e,criteriaforInternal c where c.criteria_no=e.criteria_no and e.exam_code=se.examCode and se.subjectid=sm.subjectid and r.app_no=sm.appNo and se.coNo in('" + Cou + "')  and isnull(ltrim(rtrim(r.sections)),'')=isnull(ltrim(rtrim(e.sections)),'') and e.criteria_no in('" + test + "') and e.subject_no='" + SubNo + "'  and r.batch_year='" + batchyear + "' and r.degree_code='" + degreecode + "' and r.Current_Semester='" + semester + "' " + strsec + "  and  RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR'";
            DataTable dtMarks = dir.selectDataTable(SelectMark);

            string strresult = "select r.reg_no,r.roll_no,r.App_no,r.Stud_Name,e.exam_code,e.criteria_No,e.subject_no,c.criteria,re.marks_obtained from registration r,result re,exam_type e,criteriaforInternal c where c.criteria_no=e.criteria_no and e.exam_code=re.exam_Code and r.roll_no=re.roll_no   and isnull(ltrim(rtrim(r.sections)),'')=isnull(ltrim(rtrim(e.sections)),'') and e.criteria_no in('" + test + "') and e.subject_no='" + SubNo + "'  and r.batch_year='" + batchyear + "' and r.degree_code='" + degreecode + "' and r.Current_Semester='" + semester + "'   " + strsec + "  and  RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR'";
            DataTable dtResult = dir.selectDataTable(strresult);

            DataTable dicSubName = dtMarks.DefaultView.ToTable(true, "criteria_No", "subsubjectName");

            string strpartResult = "select SUM(im.marks) mark,examcode,app_no,(select isnull(template,'') from Master_Settings where settings='COSettings' and masterno=ca.CourseOutComeNo) as CourseoutCome,c.criteria,e.subject_no from CAQuesSettingsParent ca,NewInternalMarkEntry im,exam_type e,criteriaforInternal c where c.criteria_no=e.criteria_no  and e.exam_code=im.examcode and  ca.MasterID=im.MasterID and  e.criteria_no in('" + test + "') and  e.subject_no='" + SubNo + "' and im.marks>0 group by examcode,app_no,ca.CourseOutComeNo,ca.PartNo,c.criteria,e.subject_no  ";
            DataTable dtPartresult = dir.selectDataTable(strpartResult);
            DataTable dtData = new DataTable();
            int col = 0;
            if (dsst.Rows.Count > 0 && (dtMarks.Rows.Count > 0 || dtResult.Rows.Count > 0))
            {
                ArrayList arrColHdrNames1 = new ArrayList();
                ArrayList arrColHdrNames2 = new ArrayList();
                ArrayList arrColHdrNames3 = new ArrayList();
                //ArrayList arrColHdrNames4 = new ArrayList();
                arrColHdrNames1.Add("S.No");
                arrColHdrNames2.Add("S.No");
                arrColHdrNames3.Add("S.No");
                //arrColHdrNames4.Add("S.No");
                data.Columns.Add("col 0");
                arrColHdrNames1.Add("Roll No");
                arrColHdrNames2.Add("Roll No");
                arrColHdrNames3.Add("Roll No");
                col++;
                data.Columns.Add("col " + col);

                arrColHdrNames1.Add("Register No");
                arrColHdrNames2.Add("Register No");
                arrColHdrNames3.Add("Register No");

                col++;
                data.Columns.Add("col " + col);
                col = col + 1;
                colcnt = col + 1;
                arrColHdrNames1.Add("Student Name");
                arrColHdrNames2.Add("Student Name");
                arrColHdrNames3.Add("Student Name");
                data.Columns.Add("col " + col);
                int colHdrIndx = col + 1;
                if (CkLcourse.Items.Count > 0)
                {
                    for (int co = 0; co < CkLcourse.Items.Count; co++)
                    {
                        if (CkLcourse.Items[co].Selected)
                        {
                            string CONo = Convert.ToString(CkLcourse.Items[co].Text);
                            string coursename = CkLcourse.Items[co].Text;
                            for (int test1 = 0; test1 < cbltest.Items.Count; test1++)
                            {
                                if (cbltest.Items[test1].Selected == true)
                                {
                                    string textcode = cbltest.Items[test1].Value;
                                    string textname = cbltest.Items[test1].Text;
                                    dicSubName.DefaultView.RowFilter = "criteria_No='" + textcode + "'";
                                    DataTable dvipart = dicSubName.DefaultView.ToTable();
                                    if (dvipart.Rows.Count > 0)
                                    {
                                        for (int p = 0; p < dvipart.Rows.Count; p++)
                                        {
                                            arrColHdrNames1.Add(coursename);
                                            arrColHdrNames2.Add(textname);
                                            arrColHdrNames3.Add(Convert.ToString(dvipart.Rows[p]["subsubjectName"]));
                                            data.Columns.Add("col " + colHdrIndx);
                                            colHdrIndx++;
                                            col++;

                                        }
                                    }
                                    else
                                    {
                                        arrColHdrNames1.Add(textname);
                                        arrColHdrNames2.Add(textname);
                                        arrColHdrNames3.Add(coursename);
                                        data.Columns.Add("col " + colHdrIndx);
                                        colHdrIndx++;
                                        col++;
                                    }
                                }
                            }
                        }
                    }
                }

                DataRow drHdr1 = data.NewRow();
                DataRow drHdr2 = data.NewRow();
                DataRow drHdr3 = data.NewRow();
                DataRow drHdr4 = data.NewRow();

                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                {

                    drHdr1["col " + grCol] = arrColHdrNames1[grCol];
                    drHdr2["col " + grCol] = arrColHdrNames2[grCol];
                    drHdr3["col " + grCol] = arrColHdrNames3[grCol];
                }
                data.Rows.Add(drHdr1);
                data.Rows.Add(drHdr2);
                data.Rows.Add(drHdr3);
                if (data.Columns.Count > 0)
                {
                    for (int i = 0; i < dsst.Rows.Count; i++)
                    {
                        string roll = Convert.ToString(dsst.Rows[i]["roll_no"]);
                        string RegNo = Convert.ToString(dsst.Rows[i]["Reg_no"]);
                        string StuName = Convert.ToString(dsst.Rows[i]["stud_name"]);
                        string appNO = Convert.ToString(dsst.Rows[i]["app_no"]);
                        drow = data.NewRow();
                        data.Rows.Add(drow);
                        for (int j = 0; j < data.Columns.Count; j++)
                        {
                            string coName = Convert.ToString(data.Rows[0][j]);
                            string TestName = Convert.ToString(data.Rows[1][j]);
                            string Subtype = Convert.ToString(data.Rows[2][j]);
                            int sNo = i + 1;

                            if (Subtype.Trim().ToLower() == "s.no")
                                data.Rows[data.Rows.Count - 1][j] = sNo.ToString();
                            else if (Subtype.Trim().ToLower() == "roll no")
                                data.Rows[data.Rows.Count - 1][j] = roll;
                            else if (Subtype.Trim().ToLower() == "register no")
                                data.Rows[data.Rows.Count - 1][j] = RegNo;
                            else if (Subtype.Trim().ToLower() == "student name")
                                data.Rows[data.Rows.Count - 1][j] = StuName;
                            else
                            {
                                dtMarks.DefaultView.RowFilter = "App_no='" + appNO + "' and CourseoutCome='" + coName + "' and criteria='" + TestName + "' and subsubjectName='" + Subtype + "'";
                                DataView dvma = dtMarks.DefaultView;
                                if (dvma.Count > 0)
                                {
                                    data.Rows[data.Rows.Count - 1][j] = getMarkText(Convert.ToString(dvma[0]["testmark"]));
                                }
                                else
                                {
                                    if (dtPartresult.Rows.Count > 0)
                                    {
                                        dtPartresult.DefaultView.RowFilter = "App_no='" + appNO + "' and criteria='" + TestName + "' and CourseoutCome='" + Subtype + "'";
                                        DataView dvmar = dtPartresult.DefaultView;
                                        if (dvmar.Count > 0)
                                        {
                                            data.Rows[data.Rows.Count - 1][j] = getMarkText(Convert.ToString(dvmar[0]["mark"]));
                                        }
                                        else
                                            data.Rows[data.Rows.Count - 1][j] = "-";
                                    }
                                    else
                                        data.Rows[data.Rows.Count - 1][j] = "-";
                                }
                            }
                        }

                    }

                    //string coName=Convert.ToString(data.Rows[0][])


                    Showgrid.DataSource = data;
                    Showgrid.DataBind();
                    Showgrid.Visible = true;
                    divMainContents.Visible = true;
                    printtable.Visible = true;

                    int rowcnt = Showgrid.Rows.Count - 2;

                    Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Showgrid.Rows[0].Font.Bold = true;
                    Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                    Showgrid.Rows[2].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Showgrid.Rows[2].Font.Bold = true;
                    Showgrid.Rows[2].HorizontalAlign = HorizontalAlign.Center;
                    Showgrid.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Showgrid.Rows[1].Font.Bold = true;
                    Showgrid.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                    //Rowspan
                    for (int rowIndex = Showgrid.Rows.Count - rowcnt - 1; rowIndex >= 0; rowIndex--)
                    {
                        GridViewRow row = Showgrid.Rows[rowIndex];
                        GridViewRow previousRow = Showgrid.Rows[rowIndex + 1];
                        Showgrid.Rows[rowIndex].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        Showgrid.Rows[rowIndex].Font.Bold = true;
                        Showgrid.Rows[rowIndex].HorizontalAlign = HorizontalAlign.Center;
                        if (rowIndex == 3)
                        {
                            for (int i = 0; i < colcnt; i++)
                            {
                                if (row.Cells[i].Text == previousRow.Cells[i].Text)
                                {

                                    row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                           previousRow.Cells[i].RowSpan + 1;
                                    previousRow.Cells[i].Visible = false;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < row.Cells.Count; i++)
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
                    //ColumnSpan
                    for (int rowIndex = Showgrid.Rows.Count - rowcnt - 2; rowIndex >= 0; rowIndex--)
                    {


                        for (int cell = Showgrid.Rows[rowIndex].Cells.Count - 1; cell > 0; cell--)
                        {
                            TableCell colum = Showgrid.Rows[rowIndex].Cells[cell];
                            TableCell previouscol = Showgrid.Rows[rowIndex].Cells[cell - 1];
                            if (colum.Text == previouscol.Text)
                            {
                                if (previouscol.ColumnSpan == 0)
                                {
                                    if (colum.ColumnSpan == 0)
                                    {
                                        previouscol.ColumnSpan += 2;

                                    }
                                    else
                                    {
                                        previouscol.ColumnSpan += colum.ColumnSpan + 1;

                                    }
                                    colum.Visible = false;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
            }
        }
        catch
        {
        }
    }
    public void formate3()
    {
        try
        {
           
            DataTable dtknow = new DataTable();
            DataRow dr;
            string sections = string.Empty;
            string strsec = string.Empty;
            string degreecode = string.Empty;
            string collegecode = string.Empty;
            string batchyear = string.Empty;
            string semester = string.Empty;

            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value).Trim();
            }
            if (ddlbatch.Items.Count > 0)
            {
                batchyear = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
            }
            if (ddlbranch.Items.Count > 0)
            {
                degreecode = Convert.ToString(ddlbranch.SelectedItem.Value).Trim();
            }
            if (ddlsection.Items.Count > 0)
            {
                sections = Convert.ToString(ddlsection.SelectedValue).Trim();
                if (Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "all" || Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "" || Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() == "-1")
                {
                    strsec = "";
                }
                else
                {
                    strsec = "  and isnull(ltrim(rtrim(r.Sections)),'')='" + Convert.ToString(sections).Trim() + "'";
                }
            }
            if (ddlsemester.Items.Count > 0)
            {
                semester = Convert.ToString(ddlsemester.SelectedValue).Trim();
            }
            string Cou = string.Empty;
            string test = string.Empty;
            if (CkLcourse.Items.Count > 0)
                Cou = rs.getCblSelectedValue(CkLcourse);
            if (cbltest.Items.Count > 0)
                test = rs.getCblSelectedValue(cbltest);



            string SubNo = Convert.ToString(ddl_subject.SelectedValue).Trim();
            string stddet = "select distinct r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Name,r.App_no  from Registration r where r.batch_year='" + batchyear + "' and r.degree_code='" + degreecode + "' and r.Current_Semester='" + semester + "' " + strsec + "  and  RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' order by r.reg_no,r.roll_no ";//and app_no='11855'
            DataTable dsst = dir.selectDataTable(stddet);

            string SelectMark = "select r.reg_no,r.roll_no,r.App_no,r.Stud_Name,sm.testmark,sm.retestmark,(select isnull(template,'') from Master_Settings where settings='COSettings' and masterno=se.coNo) as CourseoutCome,se.coNo,e.exam_code,e.criteria_No,e.subject_no,c.criteria,se.subsubjectName from registration r,subSubjectWiseMarkEntry sm,subsubjectTestDetails se,exam_type e,criteriaforInternal c where c.criteria_no=e.criteria_no and e.exam_code=se.examCode and se.subjectid=sm.subjectid and r.app_no=sm.appNo and se.coNo in('" + Cou + "')  and isnull(ltrim(rtrim(r.sections)),'')=isnull(ltrim(rtrim(e.sections)),'') and e.criteria_no in('" + test + "') and e.subject_no='" + SubNo + "'  and r.batch_year='" + batchyear + "' and r.degree_code='" + degreecode + "' and r.Current_Semester='" + semester + "' " + strsec + "  and  RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR'";
            DataTable dtMarks = dir.selectDataTable(SelectMark);

            string strresult = "select r.reg_no,r.roll_no,r.App_no,r.Stud_Name,e.exam_code,e.criteria_No,e.subject_no,c.criteria,re.marks_obtained from registration r,result re,exam_type e,criteriaforInternal c where c.criteria_no=e.criteria_no and e.exam_code=re.exam_Code and r.roll_no=re.roll_no   and isnull(ltrim(rtrim(r.sections)),'')=isnull(ltrim(rtrim(e.sections)),'') and e.criteria_no in('" + test + "') and e.subject_no='" + SubNo + "'  and r.batch_year='" + batchyear + "' and r.degree_code='" + degreecode + "' and r.Current_Semester='" + semester + "'   " + strsec + "  and  RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR'";
            DataTable dtResult = dir.selectDataTable(strresult);

            DataTable dicSubName = dtMarks.DefaultView.ToTable(true, "criteria_No", "subsubjectName");

            //string strpartResult = "select SUM(im.marks) mark,examcode,app_no,(select isnull(template,'') from Master_Settings where settings='COSettings' and masterno=ca.CourseOutComeNo) as CourseoutCome,c.criteria,e.subject_no from CAQuesSettingsParent ca,NewInternalMarkEntry im,exam_type e,criteriaforInternal c where c.criteria_no=e.criteria_no  and e.exam_code=im.examcode and  ca.MasterID=im.MasterID and  e.criteria_no in('" + test + "') and  e.subject_no='" + SubNo + "' and im.marks>0 group by examcode,app_no,ca.CourseOutComeNo,ca.PartNo,c.criteria,e.subject_no  ";
            //DataTable dtPartresult = dir.selectDataTable(strpartResult);
            DataTable dtData = new DataTable();

            DataTable dtQsettings = dir.selectDataTable("select (select isnull(template,'') from Master_Settings where settings='COSettings' and masterno=ca.CourseOutComeNo) as CourseoutCome,ca.partno,ca.qno,ca.mark,ca.sub1,c.criteria from CAQuesSettingsParent ca,criteriaforInternal c  where ca.CourseOutComeNo in('" + Cou + "')  and ca.subjectno in('" + SubNo + "') and ca.CriteriaNo in('" + test + "') and c.criteria_no=ca.criteriano ");

            DataTable dicPart = dtQsettings.DefaultView.ToTable(true, "partNo", "qno");
            DataTable dicPartCo = dtQsettings.DefaultView.ToTable(true, "partNo", "CourseoutCome", "criteria");
            DataTable dicQSub = dtQsettings.DefaultView.ToTable(true, "partNo", "CourseoutCome", "criteria", "qno", "sub1");
            string couseid = string.Empty;
            string subcode = string.Empty;
            string tstcode = string.Empty;
            if (CkLcourse.Items.Count > 0)
                couseid = Convert.ToString(rs.getCblSelectedValue(CkLcourse));
            if (ddl_subject.Items.Count > 0)
                subcode = Convert.ToString(ddl_subject.SelectedValue).Trim();
            if (cbltest.Items.Count > 0)
                tstcode = Convert.ToString(rs.getCblSelectedValue(cbltest));





            DataSet dstd = new DataSet();

            //string stdmark = "select SUM(im.marks) mark,examcode,app_no,ca.CourseOutComeNo,ca.PartNo,(select isnull(template,'') from Master_Settings where settings='COSettings' and masterno=ca.CourseOutComeNo) as CourseoutCome from CAQuesSettingsParent ca,NewInternalMarkEntry im where ca.MasterID=im.MasterID and subjectno in('" + subcode + "') and CriteriaNo in('" + tstcode + "') and (marks<>-1 and marks<>-16 and marks<>-20) group by examcode,app_no,ca.CourseOutComeNo,ca.PartNo ";

            string stdmark = "select SUM(im.marks) mark,examcode,app_no,ca.CourseOutComeNo,ca.PartNo,(select isnull(template,'') from Master_Settings where settings='COSettings' and masterno=ca.CourseOutComeNo) as CourseoutCome,c.criteria from CAQuesSettingsParent ca,NewInternalMarkEntry im,criteriaforInternal c where ca.MasterID=im.MasterID and subjectno in('" + SubNo + "') and CriteriaNo in('" + test + "') and (marks<>-1 and marks<>-16 and marks<>-20) and c.criteria_no=ca.criteriano  group by examcode,app_no,ca.CourseOutComeNo,ca.PartNo,c.criteria";

            dstd = d2.select_method_wo_parameter(stdmark, "Text");

            int col = 0;
            if (dsst.Rows.Count > 0 && (dtMarks.Rows.Count > 0 || dtResult.Rows.Count > 0))
            {

                dtknow.Columns.Add("S.No");
                dtknow.Columns.Add("Roll No");
                dtknow.Columns.Add("Register No");
                dtknow.Columns.Add("Student Name");
                dr = dtknow.NewRow();
                dr["S.No"] = "S.No";
                dr["Roll No"] = "Roll No";
                dr["Register No"] = "Register No";
                dr["Student Name"] = "Student Name";
                dtknow.Rows.Add(dr);
                dr = dtknow.NewRow();
                dr["S.No"] = "S.No";
                dr["Roll No"] = "Roll No";
                dr["Register No"] = "Register No";
                dr["Student Name"] = "Student Name";
                dtknow.Rows.Add(dr);
                //dr = dtknow.NewRow();
                //dr["S.No"] = "S.No";
                //dr["Roll No"] = "Roll No";
                //dr["Register No"] = "Register No";
                //dr["Student Name"] = "Student Name";
                //dtknow.Rows.Add(dr);
                int g = 0;
                if (cbl_know.Items.Count > 0)
                {
                    for (int ko = 0; ko < cbl_know.Items.Count; ko++)
                    {
                        int m = 0;
                        if (cbl_know.Items[ko].Selected)
                        {
                            string course = "select distinct template,masterno from COThresholdSettings ct,Master_Settings ms,CO_MasterValues co where ct.CourseID=ms.masterno and ct.Knowledgeid=co.MasterCode and CollegeCode='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and MasterCriteria='Knowledgesettings' and MasterCode ='" + Convert.ToString(cbl_know.Items[ko].Value) + "'";
                            DataSet know_led = d2.select_method_wo_parameter(course, "Test");

                            if (know_led.Tables.Count > 0 && know_led.Tables[0].Rows.Count > 0)
                            {
                                for (int kno = 0; kno < know_led.Tables[0].Rows.Count; kno++)
                                {
                                    if (CkLcourse.Items.Count > 0)
                                    {
                                        for (int co = 0; co < CkLcourse.Items.Count; co++)
                                        {
                                            if (CkLcourse.Items[co].Selected)
                                            {
                                                string CONo = Convert.ToString(CkLcourse.Items[co].Text);
                                                string coursename = CkLcourse.Items[co].Text;
                                                if (Convert.ToString(CkLcourse.Items[co].Value) == Convert.ToString(know_led.Tables[0].Rows[kno]["masterno"]))
                                                {
                                                    Boolean colcun = false;
                                                    for (int test1 = 0; test1 < cbltest.Items.Count; test1++)
                                                    {
                                                        if (cbltest.Items[test1].Selected == true)
                                                        {


                                                            string textcode = cbltest.Items[test1].Value;
                                                            string textname = cbltest.Items[test1].Text;
                                                            dicPartCo.DefaultView.RowFilter = "CourseoutCome='" + coursename + "'  and criteria='" + textname + "' ";//and CriteriaNo='" + textcode + "'


                                                            DataView dvipart = dicPartCo.DefaultView;

                                                            if (dvipart.Count > 0)
                                                            {
                                                                if (colcun == false)
                                                                {
                                                                    colcun = true;
                                                                    m++;
                                                                   
                                                                    dtknow.Columns.Add(coursename + '-' + g);



                                                                    //  dtknow.Rows[dtknow.Rows.Count - 1][coursename + '-' + g] = textname;
                                                                    dtknow.Rows[dtknow.Rows.Count - 1][coursename + '-' + g] = coursename;
                                                                    dtknow.Rows[dtknow.Rows.Count - 2][coursename + '-' + g] = Convert.ToString(cbl_know.Items[ko].Text);
                                                                }
                                                            }
                                                        }
                                                    }


                                                    
                                                }
                                            }
                                        }
                                        g++;
                                    }
                                   
                                }
                                if (m != 0)
                                {
                                   
                                    
                                        dtknow.Columns.Add("Total" + '-' + g);
                                       // dtknow.Rows[dtknow.Rows.Count - 3]["Total" + '-' + g] = Convert.ToString(cbl_know.Items[ko].Text);
                                        dtknow.Rows[dtknow.Rows.Count - 2]["Total" + '-' + g] = Convert.ToString(cbl_know.Items[ko].Text);
                                        dtknow.Rows[dtknow.Rows.Count - 1]["Total" + '-' + g] = "Total";

                                       
                                        dtknow.Columns.Add("Percentage" + '-' + g);
                                        //dtknow.Rows[dtknow.Rows.Count - 3]["Percentage" + '-' + g] = Convert.ToString(cbl_know.Items[ko].Text);
                                        dtknow.Rows[dtknow.Rows.Count - 2]["Percentage" + '-' + g] = Convert.ToString(cbl_know.Items[ko].Text);
                                        dtknow.Rows[dtknow.Rows.Count - 1]["Percentage" + '-' + g] = "Percentage";
                                    
                                }

                            }

                        }
                    }
                }
                if (dtknow.Columns.Count > 0)
                {
                    dr = dtknow.NewRow();
                    dtknow.Rows.Add(dr);
                    double totalmark = 0;
                    string partNo = string.Empty;
                    string testtxt = string.Empty;
                    int hsno = 0;
                    double total = 0;
                    for (int j = 0; j < dtknow.Columns.Count; j++)
                    {
                        double partSum = 0;
                        string coName = Convert.ToString(dtknow.Rows[0][j]);
                        string Subtype = Convert.ToString(dtknow.Rows[1][j]);
                        string TestName = Convert.ToString(dtknow.Rows[2][j]);

                        if (Subtype.Trim().ToLower() == "s.no")
                            dtknow.Rows[dtknow.Rows.Count - 1][j] = "S.No";
                        else if (Subtype.Trim().ToLower() == "roll no")
                            dtknow.Rows[dtknow.Rows.Count - 1][j] = "Roll No";
                        else if (Subtype.Trim().ToLower() == "register no")
                            dtknow.Rows[dtknow.Rows.Count - 1][j] = "Register No";
                        else if (Subtype.Trim().ToLower() == "student name")
                            dtknow.Rows[dtknow.Rows.Count - 1][j] = "Student Name";
                        else if (Subtype.Trim().ToLower() == "total")
                        {
                            
                                if (!hat.ContainsKey(hsno))
                                {
                                    hat.Add(hsno, totalmark);
                                    
                                        dtknow.Rows[dtknow.Rows.Count - 1][j] = totalmark;
                                    
                                    totalmark = 0;
                                    partNo = string.Empty;
                                    testtxt = string.Empty;
                                    hsno++;
                                }
                            
                        }
                        else if (Subtype.Trim().ToLower() == "percentage")
                        {
                           
                                dtknow.Rows[dtknow.Rows.Count - 1][j] = "Percentage";
                            
                        }
                        else
                        {
                            for (int test1 = 0; test1 < cbltest.Items.Count; test1++)
                            {
                                if (cbltest.Items[test1].Selected == true)
                                {


                                    string textcode = cbltest.Items[test1].Value;
                                    string textname = cbltest.Items[test1].Text;
                                    dicPartCo.DefaultView.RowFilter = "CourseoutCome='" + Subtype + "'  and criteria='" + textname + "' ";//and CriteriaNo='" + textcode + "'


                                    DataView dviparts = dicPartCo.DefaultView;

                                    if (dviparts.Count > 0)
                                    {
                                        dtQsettings.DefaultView.RowFilter = "CourseoutCome='" + Subtype + "'  and criteria='" + textname + "' ";//and CriteriaNo='" + textcode + "'

                                        DataTable dvipart = dtQsettings.DefaultView.ToTable();
                                        if (dvipart.Rows.Count > 0)
                                        {
                                            DataTable dtPar = dvipart.DefaultView.ToTable(true, "Qno");

                                            if (dtPar.Rows.Count > 0)
                                            {
                                                int sm = 0;
                                                foreach (DataRow drs in dtPar.Rows)
                                                {
                                                    string Qno = Convert.ToString(dvipart.Rows[sm]["qno"]);
                                                    dicQSub.DefaultView.RowFilter = "CourseoutCome='" + Subtype + "'  and criteria='" + textname + "' and PartNo='" + Convert.ToString(dvipart.Rows[sm]["partno"]) + "' and Qno='" + Qno + "'";
                                                    DataTable dicP = dicQSub.DefaultView.ToTable();
                                                    object sum = dtQsettings.Compute("Sum(Mark)", "Qno='" + Qno + "' and CourseoutCome='" + Subtype + "' and criteria='" + textname + "' and PartNo='" + Convert.ToString(dvipart.Rows[sm]["partno"]) + "' and Qno='" + Qno + "'");

                                                    totalmark = totalmark + (Convert.ToDouble(sum) / dicP.Rows.Count);
                                                    partSum = partSum + (Convert.ToDouble(sum) / dicP.Rows.Count);

                                                    sm++;
                                                }
                                            }
                                        }
                                    }
                                }
                                dtknow.Rows[dtknow.Rows.Count - 1][j] = partSum;
                            }
                                partSum = 0;
                            


                        }

                    }

                }

               
                if (dtknow.Columns.Count > 0)
                {
                    for (int i = 0; i < dsst.Rows.Count; i++)
                    {
                        string roll = Convert.ToString(dsst.Rows[i]["roll_no"]);
                        string RegNo = Convert.ToString(dsst.Rows[i]["Reg_no"]);
                        string StuName = Convert.ToString(dsst.Rows[i]["stud_name"]);
                        string appNO = Convert.ToString(dsst.Rows[i]["app_no"]);
                        int hsdno = 0;
                        dr = dtknow.NewRow();
                        dtknow.Rows.Add(dr);
                        double total = 0;
                        for (int j = 0; j < dtknow.Columns.Count; j++)
                        {

                            string coName = Convert.ToString(dtknow.Rows[0][j]);
                            string Subtype = Convert.ToString(dtknow.Rows[1][j]);
                            string TestName = Convert.ToString(dtknow.Rows[2][j]);
                            int sNo = i + 1;
                           
                           
                            if (Subtype.Trim().ToLower() == "s.no")
                                dtknow.Rows[dtknow.Rows.Count - 1][j] = sNo.ToString();
                            else if (Subtype.Trim().ToLower() == "roll no")
                                dtknow.Rows[dtknow.Rows.Count - 1][j] = roll;
                            else if (Subtype.Trim().ToLower() == "register no")
                                dtknow.Rows[dtknow.Rows.Count - 1][j] = RegNo;
                            else if (Subtype.Trim().ToLower() == "student name")
                                dtknow.Rows[dtknow.Rows.Count - 1][j] = StuName;
                           
                            else if (Subtype.Trim().ToLower() == "total")
                            {
                               
                            
                                if (total > 0)
                                    dtknow.Rows[dtknow.Rows.Count - 1][j] = total;
                                else
                                    dtknow.Rows[dtknow.Rows.Count - 1][j] = "-";
                                 

                            }
                            else if (Subtype.Trim().ToLower() == "percentage")
                            {

                                if (hat.ContainsKey(hsdno))
                                {
                                    double tos = 0;
                                    double.TryParse(Convert.ToString(hat[hsdno]), out tos);
                                    tos = (total / tos) * 100;
                                    tos = Math.Round(tos);
                                    if (tos > 0)
                                        dtknow.Rows[dtknow.Rows.Count - 1][j] = tos;
                                    else
                                        dtknow.Rows[dtknow.Rows.Count - 1][j] = "-";
                                    hsdno++;
                                    total = 0;
                                }
                                else
                                    dtknow.Rows[dtknow.Rows.Count - 1][j] = "-";

                            }
                            else
                            {
                                double totalmark = 0;
                                for (int test1 = 0; test1 < cbltest.Items.Count; test1++)
                                {
                                    if (cbltest.Items[test1].Selected == true)
                                    {


                                        string textcode = cbltest.Items[test1].Value;
                                        string textname = cbltest.Items[test1].Text;
                                        dicPartCo.DefaultView.RowFilter = "CourseoutCome='" + Subtype + "'  and criteria='" + textname + "' ";//and CriteriaNo='" + textcode + "'


                                        DataView dviparts = dicPartCo.DefaultView;

                                        if (dviparts.Count > 0)
                                        {

                                            dicPartCo.DefaultView.RowFilter = "CourseoutCome='" + Subtype + "'  and criteria='" + textname + "' ";//and CriteriaNo='" + textcode + "'


                                            DataView dvipart = dicPartCo.DefaultView;

                                            if (dvipart.Count > 0)
                                            {

                                               
                                                for (int p = 0; p < dvipart.Count; p++)
                                                {

                                                    if (dstd.Tables.Count > 0 && dstd.Tables[0].Rows.Count > 0)
                                                    {
                                                        double sumVal = 0;
                                                        //dstd.Tables[0].DefaultView.RowFilter = "CourseoutCome='" + coName + "' and criteria='" + TestName + "' and PartNo='" +Convert.ToString(s[1]) + "'";
                                                        // DataTable dtstuMark=dstd.Tables[0].DefaultView.ToTable();
                                                        object sum = dstd.Tables[0].Compute("Sum(Mark)", "CourseoutCome='" + Subtype + "' and criteria='" + textname + "' and PartNo='" + Convert.ToString(dvipart[p]["partno"]) + "' and app_no='" + appNO + "'");
                                                        double.TryParse(Convert.ToString(sum), out sumVal);
                                                        totalmark = totalmark + sumVal;

                                                    }
                                                }
                                               
                                            }
                                        }
                                    }
                                }
                                if (totalmark > 0)
                                {
                                    dtknow.Rows[dtknow.Rows.Count - 1][j] = Convert.ToString(totalmark);
                                    total += totalmark;
                                }
                                else
                                    dtknow.Rows[dtknow.Rows.Count - 1][j] = "-";
                            }
                        }

                    }

                    //string coName=Convert.ToString(data.Rows[0][])

                    if (dtknow.Rows.Count > 2)
                    {
                        

                            Showgrid.DataSource = dtknow;
                        Showgrid.DataBind();
                        Showgrid.Visible = true;
                        divMainContents.Visible = true;
                        printtable.Visible = true;

                        //for (int cell = Showgrid.Rows[1].Cells.Count - 1; cell > 4; cell--)
                        //{
                        //    string colname = Showgrid.Rows[1].Cells[cell].Text;
                        //    if (ItemList.Contains(colname))
                        //    {
                        //        for (int rowIndex = Showgrid.Rows.Count - 1; rowIndex >= 1; rowIndex--)
                        //        {
                        //            Showgrid.Rows[rowIndex].Cells[cell].Visible = true;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        for (int rowIndex = Showgrid.Rows.Count - 1; rowIndex >=1; rowIndex--)
                        //        {
                        //            Showgrid.Rows[rowIndex].Cells[cell].Visible = false;
                        //        }
                        //    }
                        //}
                        
                        



                        int rowcnt = Showgrid.Rows.Count - 2;

                        Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        Showgrid.Rows[0].Font.Bold = true;
                        Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                        Showgrid.Rows[2].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        Showgrid.Rows[2].Font.Bold = true;
                        Showgrid.Rows[2].HorizontalAlign = HorizontalAlign.Center;
                        Showgrid.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        Showgrid.Rows[1].Font.Bold = true;
                        Showgrid.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                        //Rowspan
                        for (int rowIndex = Showgrid.Rows.Count - rowcnt; rowIndex >= 0; rowIndex--)
                        {
                            GridViewRow row = Showgrid.Rows[rowIndex];
                            GridViewRow previousRow = Showgrid.Rows[rowIndex + 1];
                            Showgrid.Rows[rowIndex].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            Showgrid.Rows[rowIndex].Font.Bold = true;
                            Showgrid.Rows[rowIndex].HorizontalAlign = HorizontalAlign.Center;
                            if (rowIndex == 3)
                            {
                                for (int i = 0; i < colcnt; i++)
                                {
                                    if (row.Cells[i].Text == previousRow.Cells[i].Text)
                                    {

                                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                               previousRow.Cells[i].RowSpan + 1;
                                        previousRow.Cells[i].Visible = false;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < row.Cells.Count; i++)
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
                        //ColumnSpan
                        for (int rowIndex = Showgrid.Rows.Count - rowcnt; rowIndex >= 0; rowIndex--)
                        {


                            for (int cell = Showgrid.Rows[rowIndex].Cells.Count - 1; cell > 0; cell--)
                            {
                                TableCell colum = Showgrid.Rows[rowIndex].Cells[cell];
                                TableCell previouscol = Showgrid.Rows[rowIndex].Cells[cell - 1];
                                if (colum.Text == previouscol.Text)
                                {
                                    if (previouscol.ColumnSpan == 0)
                                    {
                                        if (colum.ColumnSpan == 0)
                                        {
                                            previouscol.ColumnSpan += 2;

                                        }
                                        else
                                        {
                                            previouscol.ColumnSpan += colum.ColumnSpan + 1;

                                        }
                                        colum.Visible = false;
                                    }
                                }
                            }
                        }

                        for (int rowIndex = Showgrid.Rows.Count - 1; rowIndex >= 0; rowIndex--)
                        {
                            //GridViewRow row = Showgrid.Rows[rowIndex];
                            //GridViewRow previousRow = Showgrid.Rows[rowIndex + 1];
                            //Showgrid.Rows[rowIndex].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            // Showgrid.Rows[rowIndex].Font.Bold = true;
                            for (int colm = 4; colm < Showgrid.Rows[rowIndex].Cells.Count; colm++)
                            {
                                Showgrid.Rows[rowIndex].Cells[colm].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }

                        
                    }
                }
            }

        }
        catch
        {
        }
    }
    private string getMarkText(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "-1":
                    mark = "AAA";
                    break;
                case "-2":
                    mark = "EL";
                    break;
                case "-3":
                    mark = "EOD";
                    break;
                case "-4":
                    mark = "ML";
                    break;
                case "-5":
                    mark = "SOD";
                    break;
                case "-6":
                    mark = "NSS";
                    break;
                case "-7":
                    mark = "NJ";
                    break;
                case "-8":
                    mark = "S";
                    break;
                case "-9":
                    mark = "L";
                    break;
                case "-10":
                    mark = "NCC";
                    break;
                case "-11":
                    mark = "HS";
                    break;
                case "-12":
                    mark = "PP";
                    break;
                case "-13":
                    mark = "SYOD";
                    break;
                case "-14":
                    mark = "COD";
                    break;
                case "-15":
                    mark = "OOD";
                    break;
                case "-16":
                    mark = "OD";
                    break;
                case "-17":
                    mark = "LA";
                    break;
                case "-18":
                    mark = "RAA";
                    break;
                case "-20":
                    mark = "RAA";
                    break;
            }
        }
        catch
        {
        }
        return mark;
    }
    #region Knowledge
    public void bindknowledge()
    {
        try
        {
            txtkonw.Text = "---Select---";
            chk_know.Checked = false;
            cbl_know.Items.Clear();
            dsmethodgosubj.Clear();

            string course = "select * from CO_MasterValues where  MasterCriteria ='Knowledgesettings' and CollegeCode ='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' ";
            dsmethodgosubj = d2.select_method_wo_parameter(course, "Text");
            if (dsmethodgosubj.Tables[0].Rows.Count > 0)
            {
                cbl_know.DataSource = dsmethodgosubj;
                cbl_know.DataTextField = "MasterValue";
                cbl_know.DataValueField = "MasterCode";
                cbl_know.DataBind();

            }
            if (cbl_know.Items.Count > 0)
            {
                for (int row = 0; row < cbl_know.Items.Count; row++)
                {
                    cbl_know.Items[row].Selected = true;
                    chk_know.Checked = true;
                }
                txtkonw.Text = "Knowledge(" + cbl_know.Items.Count + ")";
            }
            else
            {
                txtkonw.Text = "--Select--";
            }
            bindcoknowldge();
        }
        catch (Exception ex)
        {

        }
    }
    public void bindcoknowldge()
    {
        try
        {
            Textcourse.Text = "---Select---";
            Checkcourse.Checked = false;
            CkLcourse.Items.Clear();
            dsmethodgosubj.Clear();
            string typ = string.Empty;
            if (cbl_know.Items.Count > 0)
            {
                for (int i = 0; i < cbl_know.Items.Count; i++)
                {
                    if (cbl_know.Items[i].Selected == true)
                    {
                        if (typ == "")
                        {
                            typ = "" + cbl_know.Items[i].Value + "";
                        }
                        else
                        {
                            typ = typ + "" + "," + "" + cbl_know.Items[i].Value + "";
                        }
                    }

                }
            }
            if (typ != "")
            {
                string course = "    select  distinct masterno,template from COThresholdSettings ct,Master_Settings ms,CO_MasterValues co where ct.CourseID=ms.masterno and ct.Knowledgeid=co.MasterCode and CollegeCode='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' and MasterCriteria='Knowledgesettings' and MasterCode in(" + typ + ")";
                dsmethodgosubj = d2.select_method_wo_parameter(course, "Text");
                if (dsmethodgosubj.Tables[0].Rows.Count > 0)
                {
                    CkLcourse.DataSource = dsmethodgosubj;
                    CkLcourse.DataTextField = "template";
                    CkLcourse.DataValueField = "masterno";
                    CkLcourse.DataBind();

                }
                if (CkLcourse.Items.Count > 0)
                {
                    for (int row = 0; row < CkLcourse.Items.Count; row++)
                    {
                        CkLcourse.Items[row].Selected = true;
                        Checkcourse.Checked = true;
                    }
                    Textcourse.Text = "Course(" + CkLcourse.Items.Count + ")";
                }
                else
                {
                    Textcourse.Text = "--Select--";
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void chk_know_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_know.Checked == true)
        {
            for (int i = 0; i < cbl_know.Items.Count; i++)
            {
                cbl_know.Items[i].Selected = true;
            }
            txtkonw.Text = "Knowledge(" + (cbl_know.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_know.Items.Count; i++)
            {
                cbl_know.Items[i].Selected = false;
            }
            txtkonw.Text = "---Select---";
        }
        Showgrid.Visible = false;
        divMainContents.Visible = false;
        printtable.Visible = false;
    }

    protected void cbl_know_SelectedIndexChanged(object sender, EventArgs e)
    {
        Pcourse.Focus();
        chk_know.Checked = false;
        txtkonw.Text = "---Select---";
        int corcount = 0;
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < cbl_know.Items.Count; i++)
        {
            if (cbl_know.Items[i].Selected == true)
            {
                value = cbl_know.Items[i].Text;
                code = cbl_know.Items[i].Value.ToString();
                corcount = corcount + 1;
            }
        }
        if (corcount > 0)
        {
            txtkonw.Text = "Course(" + corcount.ToString() + ")";
            if (corcount == cbl_know.Items.Count)
            {
                chk_know.Checked = true;
            }
        }
        bindcoknowldge();
        subjectcnt = corcount;
        Showgrid.Visible = false;
        divMainContents.Visible = false;
        printtable.Visible = false;
    }
    protected void rbtformate3_CheckedChanged(object sender, EventArgs e)
    {
        try
        {


            if (rbtformate3.Checked == true)
            {
                bindknowledge();
                bindcoknowldge();
                tblknowlwdge.Visible = true;
                lblknow.Visible = true;
            }
            else
            {
                BindCourseoutcome();
                tblknowlwdge.Visible = false;
                lblknow.Visible = false;
            }
        }
        catch
        {
        }

    }

    #endregion


}