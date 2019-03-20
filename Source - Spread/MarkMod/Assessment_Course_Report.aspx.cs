using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InsproDataAccess;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

public partial class MarkMod_Assessment_Course_Report : System.Web.UI.Page
{

    #region Field_Declaration

    SqlConnection con_Getfunc = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_subcrd = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con_splhr_query_master = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlCommand cmd = new SqlCommand();

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
    ReuasableMethods rs = new ReuasableMethods();
    DataTable dtab = new DataTable();
    DataRow drow;



    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                collegecode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
                usercode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleuser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                group_user = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";

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
           
            if (!IsPostBack)
            {

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
                bindcollege(sender, e);
                bindconame();
                if (ddlcollege.Items.Count >= 1)
                {

                    BindBatch();
                    BindDegree(singleuser, group_user, collegecode, usercode);
                    if (ddldegree.Items.Count > 0)
                    {
                        BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                        BindSem(strbranch, strbatchyear, collegecode);
                        BindSectionDetail(strbatch, strbranch);
                        bindconame();
                        BindSubjecttest(strbatch, strbranch, strsem, strsec);
                        Bindtest();

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Give degree rights to staff')", true);

                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Give college rights to staff')", true);

                }
            }
        }
        catch (Exception ex) { }

    }


    #region College
    public void bindcollege(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
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
                    ddlcollege_SelectedIndexChanged(sender, e);
                }

            }
        }
        catch
        {
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
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

        if (!Page.IsPostBack == false)
        {
            ddlsemester.Items.Clear();
        }
        try
        {
            if ((ddlbranch.SelectedIndex != 0) && (ddlbranch.SelectedIndex > 0))
            {
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
            BindSubjecttest(strbatch, strbranch, strsem, strsec);
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

            if (!Page.IsPostBack == false)
            {
                ddlsection.Items.Clear();
            }
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
                    BindSubjecttest(strbatch, strbranch, strsem, strsec);
                    Bindtest();
                }
                else
                {
                    ddlsection.Enabled = true;
                    BindSubjecttest(strbatch, strbranch, strsem, strsec);
                    Bindtest();
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
        Showgrid.Visible = false;
        divMainContents.Visible = false;
        printtable.Visible = false;
        BindSubjecttest(strbatch, strbranch, strsem, strsec);
        Bindtest();
    }

    #endregion

    #region CourseOutCome
    public void bindconame()
    {
        try
        {
            txtco.Text = "---Select---";
            cbco.Checked = false;
            cblco.Items.Clear();
            dsmethodgosubj.Clear();
            string course = "Select distinct template,masterno from  Master_Settings where settings='COSettings'";
            dsmethodgosubj = d2.select_method_wo_parameter(course, "Text");
            if (dsmethodgosubj.Tables[0].Rows.Count > 0)
            {
                cblco.DataSource = dsmethodgosubj;
                cblco.DataTextField = "template";
                cblco.DataValueField = "masterno";
                cblco.DataBind();

            }
            if (cblco.Items.Count > 0)
            {
                for (int row = 0; row < cblco.Items.Count; row++)
                {
                    cblco.Items[row].Selected = true;
                    cbco.Checked = true;
                }
                txtco.Text = "Course(" + cblco.Items.Count + ")";
            }
            else
            {
                txtco.Text = "--Select--";
            }


        }
        catch
        {

        }
    }

    protected void cblco_SelectedIndexChanged(object sender, EventArgs e)
    {
        cbco.Checked = false;
        txtco.Text = "---Select---";
        int corcount = 0;
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < cblco.Items.Count; i++)
        {
            if (cblco.Items[i].Selected == true)
            {
                value = cblco.Items[i].Text;
                code = cblco.Items[i].Value.ToString();
                corcount = corcount + 1;
            }
        }
        if (corcount > 0)
        {
            txtco.Text = "Course(" + corcount.ToString() + ")";
            if (corcount == cblco.Items.Count)
            {
                cbco.Checked = true;
            }
        }
        subjectcnt = corcount;
        Showgrid.Visible = false;
        divMainContents.Visible = false;
        printtable.Visible = false;
    }
    protected void cbco_CheckedChanged(object sender, EventArgs e)
    {
        if (cbco.Checked == true)
        {
            for (int i = 0; i < cblco.Items.Count; i++)
            {
                cblco.Items[i].Selected = true;
            }
            txtco.Text = "Course(" + (cblco.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblco.Items.Count; i++)
            {
                cblco.Items[i].Selected = false;
            }
            txtco.Text = "---Select---";
        }
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
        Showgrid.Visible = false;
        divMainContents.Visible = false;
        printtable.Visible = false;
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
                string Sqlstr = "select distinct c.criteria,c.criteria_no from criteriaforinternal c,registration r,syllabus_master s,Exam_type et,subject sub where et.batch_year=r.Batch_Year and et.criteria_no=c.Criteria_no and et.subject_no=sub.subject_no and sub.syll_code=s.syll_code and sub.syll_code=c.syll_code and  r.degree_code=s.degree_code and r.batch_year=s.batch_year and c.syll_code=s.syll_code and cc=0 and delflag=0 and r.exam_flag<>'debar' and r.batch_year='" + batchyear + "' and r.college_code='" + collegecode + "' and sub.subject_no='" + subjectno + "' and r.degree_code in(" + degreecode + ") " + sems + strsec + " order by c.criteria_no asc";
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

    #region Symbol
    protected void ddlsymbol_OnSelectedIndexedChanged(object sender, EventArgs e)
    {
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
            DataSet getstudet = new DataSet();
            string couseid = "";
            string subcode = "";
            string tstcode = "";
            string per = "";
            double pert = 0;
            string subno = string.Empty;
            string pervalue = string.Empty;
            dtab.Columns.Add("Details");
            for (int i = 0; i < cblco.Items.Count; i++)
            {
                if (cblco.Items[i].Selected == true)
                {
                    dtab.Columns.Add(cblco.Items[i].Text);
                }
            }
            if (ddlsymbol.Items.Count > 0)
                per = Convert.ToString(ddlsymbol.SelectedValue).Trim();
            if (txtpercent.Text != "")
                pervalue = txtpercent.Text;
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Enter the %')", true);
                return;
            }



            if (cblco.Items.Count > 0)
                couseid = Convert.ToString(rs.getCblSelectedValue(cblco));
            if (ddl_subject.Items.Count > 0)
                subcode = Convert.ToString(ddl_subject.SelectedValue);
            if (cbltest.Items.Count > 0)
                tstcode = Convert.ToString(rs.getCblSelectedValue(cbltest));

            drow = dtab.NewRow();
            drow["Details"] = "Class Averag Marks %";
            dtab.Rows.Add(drow);
            drow = dtab.NewRow();
            drow["Details"] = "Median";
            dtab.Rows.Add(drow);
            drow = dtab.NewRow();
            drow["Details"] = per + pervalue + "%";
            dtab.Rows.Add(drow);
            drow = dtab.NewRow();
            drow["Details"] = "Co Achieved : Yes/No";
            dtab.Rows.Add(drow);

            string batchyear = "";
            string degreecode = "";
            string semester = "";
            string section = "";
            string sect = "";


            int col = 0;
            if (ddlcollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollege.SelectedValue).Trim();


            if (ddlbatch.Items.Count > 0)
                batchyear = Convert.ToString(ddlbatch.SelectedValue).Trim();


            if (ddlbranch.Items.Count > 0)
                degreecode = Convert.ToString(ddlbranch.SelectedValue).Trim();


            if (ddlsemester.Items.Count > 0)
                semester = Convert.ToString(ddlsemester.SelectedValue).Trim();

            if (ddlsection.Enabled == true)
            {
                if (ddlsection.Items.Count > 0)
                {
                    if (Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddlsection.SelectedItem.Text).Trim().ToLower() != "-1")
                    {
                        section = Convert.ToString(ddlsection.SelectedItem.Text).Trim();
                        sect = " and r.Sections='" + section + "'";
                    }
                    else
                    {
                        section = "";
                    }
                }
            }
            else
            {
                section = "";
            }



            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batchyear) && !string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(couseid))
            {
                string sqlqry = "select distinct r.App_No,q.CourseOutComeNo from NewInternalMarkEntry m,CAQuesSettingsParent q,registration r,SubjectChooser s,Exam_type e   where r.App_No=m.app_no and LTRIM(rtrim(ISNULL(r.sections,'')))=LTRIM(rtrim(ISNULL(e.sections,''))) and s.subject_no=q.subjectNo and e.exam_code=m.ExamCode and e.subject_no=q.subjectNo and e.criteria_no=q.CriteriaNo and r.Roll_No=s.roll_no and q.MasterID=m.MasterID and  RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR' and r.degree_code='" + degreecode + "' and r.college_code='" + collegecode + "' and r.Batch_Year='" + batchyear + "' and r.Current_Semester='" + semester + "' " + sect + "  and q.subjectNo in('" + subcode + "') and q.CriteriaNo in('" + tstcode + "')";
                //sqlqry += " select Sum(Mark),CourseOutComeNo,subjectno,CriteriaNo from CAQuesSettingsParent";

                //sqlqry += " select SUM(im.marks) mark,ca.CourseOutComeNo,ca.subjectno,ca.CriteriaNo from CAQuesSettingsParent ca,InternalMarkEntry im where ca.MasterID=im.MasterID ";

                getstudet.Clear();
                getstudet = d2.select_method_wo_parameter(sqlqry, "text");
                if (getstudet.Tables.Count > 0 && getstudet.Tables[0].Rows.Count > 0)
                {
                    for (int cou = 0; cou < cblco.Items.Count; cou++)
                    {
                        if (cblco.Items[cou].Selected == true)
                        {
                            col++;
                            couseid = cblco.Items[cou].Value;
                            getstudet.Tables[0].DefaultView.RowFilter = "CourseOutComeNo ='" + couseid + "'";
                            DataView dvstd = getstudet.Tables[0].DefaultView;

                            int totstd = dvstd.Count;

                            string toalcousermark = d2.GetFunction(" select Sum(Mark) from CAQuesSettingsParent  where CourseOutComeNo='" + couseid + "'  and subjectno='" + subcode + "' and CriteriaNo in('" + tstcode + "')");
                            // Calculate Class Averag Marks %

                            string value = d2.GetFunction("select SUM(im.marks) mark from CAQuesSettingsParent ca,NewInternalMarkEntry im where ca.MasterID=im.MasterID  and ca.CourseOutComeNo='" + couseid + "' and ca.subjectno in('" + subcode + "') and ca.CriteriaNo in('" + tstcode + "') ");

                            if (value != "" && Convert.ToDouble(value) > 0)
                            {
                                double toalstdmark = Convert.ToDouble(totstd) * Convert.ToDouble(toalcousermark);
                                pert = Convert.ToDouble(value) / Convert.ToDouble(toalstdmark);
                                double pertge = pert * 100;
                                // double pets = (pertge * txtpert) / 100;
                                string perts = Convert.ToString(Math.Round(Convert.ToDouble(pertge), 2));
                                dtab.Rows[dtab.Rows.Count - 4][col] = perts;
                            }
                            else
                                dtab.Rows[dtab.Rows.Count - 4][col] = "0";
                            //End



                            double mediamstadmark = 0;
                            double userentrystdmark = 0;
                            int mediamstdcnt = 0;
                            int userentryperstdcnt = 0;
                            string classavgper = dtab.Rows[dtab.Rows.Count - 4][col].ToString();

                            double txtperta = Convert.ToDouble(pervalue);
                            if (dvstd.Count > 0)
                            {
                                for (int std = 0; std < dvstd.Count; std++)
                                {
                                    double stdcomark = 0;

                                    string appno = Convert.ToString(dvstd[std]["app_no"]);
                                    string stdmark = d2.GetFunction("select SUM(im.marks) mark from CAQuesSettingsParent ca,NewInternalMarkEntry im where ca.MasterID=im.MasterID   and app_no='" + appno + "' and  ca.CourseOutComeNo ='" + couseid + "' and subjectno='" + subcode + "' and CriteriaNo in('" + tstcode + "') ");

                                    if (stdmark == "")
                                        stdcomark = 0;
                                    else
                                        stdcomark = Convert.ToDouble(stdmark);


                                    if (Convert.ToDouble(stdcomark) > 0)
                                    {
                                        double stdpert = Convert.ToDouble(stdcomark) / Convert.ToDouble(toalcousermark);
                                        double pertge = stdpert * 100;
                                        string stdpersentage = Convert.ToString(Math.Round(Convert.ToDouble(pertge), 2));

                                        // Calculate Mediam
                                        if (classavgper != "")
                                        {
                                            if (Convert.ToDouble(classavgper) < Convert.ToDouble(stdpersentage))
                                            {
                                                mediamstadmark = mediamstadmark + Convert.ToDouble(stdcomark);
                                                mediamstdcnt++;
                                            }
                                        }
                                        //End


                                        //Calculate user entry Per
                                        if (per == ">")
                                        {
                                            if (Convert.ToDouble(stdpersentage) > txtperta)
                                            {
                                                string stdpertge = stdpersentage;
                                                userentrystdmark = userentrystdmark + Convert.ToDouble(stdcomark);
                                                userentryperstdcnt++;
                                            }
                                        }
                                        else if (per == "<")
                                        {
                                            if (Convert.ToDouble(stdpersentage) < txtperta)
                                            {
                                                string stdpertge = stdpersentage;
                                                userentrystdmark = userentrystdmark + Convert.ToDouble(stdcomark);
                                                userentryperstdcnt++;
                                            }
                                        }
                                        else if (per == ">=")
                                        {
                                            if (Convert.ToDouble(stdpersentage) >= txtperta)
                                            {
                                                string stdpertge = stdpersentage;
                                                userentrystdmark = userentrystdmark + Convert.ToDouble(stdcomark);
                                                userentryperstdcnt++;
                                            }
                                        }
                                        else if (per == "<=")
                                        {
                                            if (Convert.ToDouble(stdpersentage) <= txtperta)
                                            {
                                                string stdpertge = stdpersentage;
                                                userentrystdmark = userentrystdmark + Convert.ToDouble(stdcomark);
                                                userentryperstdcnt++;
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToDouble(stdpersentage) == txtperta)
                                            {
                                                string stdpertge = stdpersentage;
                                                userentrystdmark = userentrystdmark + Convert.ToDouble(stdcomark);
                                                userentryperstdcnt++;
                                            }

                                        }

                                        //End
                                    }


                                }
                            }
                            //medium
                            if (Convert.ToInt32(mediamstadmark) != 0)
                            {
                                double totalmedian = Convert.ToDouble(mediamstdcnt) * Convert.ToDouble(toalcousermark);
                                pert = Convert.ToDouble(mediamstadmark) / Convert.ToDouble(totalmedian);
                                double pertge = pert * 100;

                                // double pertge = pert;
                                string perts = Convert.ToString(Math.Round(Convert.ToDouble(pertge), 2));
                                dtab.Rows[dtab.Rows.Count - 3][col] = perts;
                            }
                            else
                                dtab.Rows[dtab.Rows.Count - 3][col] = "0";

                            //User entry
                            if (Convert.ToInt32(userentrystdmark) != 0)
                            {
                                double totalusermark = Convert.ToDouble(userentryperstdcnt) * Convert.ToDouble(toalcousermark);
                                pert = Convert.ToDouble(userentrystdmark) / Convert.ToDouble(totalusermark);
                                double pertge = pert * 100;
                                string perts = Convert.ToString(Math.Round(Convert.ToDouble(pertge), 2));
                                dtab.Rows[dtab.Rows.Count - 2][col] = perts;
                            }
                            else
                                dtab.Rows[dtab.Rows.Count - 2][col] = "0";

                        }

                    }
                    if (dtab.Columns.Count > 0)
                    {
                        Showgrid.DataSource = dtab;
                        Showgrid.DataBind();
                        Showgrid.Visible = true;
                        divMainContents.Visible = true;
                        printtable.Visible = true;

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
                e.Row.HorizontalAlign = HorizontalAlign.Center;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Font.Bold = true;
                for (int j = 1; j < Showgrid.HeaderRow.Cells.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }



        }
        catch
        {
        }
    }

    protected void Excel_OnClick(object sender, EventArgs e)
    {
        try
        {
            string report = txtreptname.Text;
            if (report.ToString().Trim() != "")
            {
                // d2.printexcelreportgrid(Showgrid, report);

            }
            else
            {
                Label1.Text = "Please Enter Your Report Name";
                Label1.Visible = true;
            }

        }

        catch (Exception ex)
        {
            Label1.Visible = true;
            Label1.Text = ex.ToString();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void Print_OnClick(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {

        }
    }
}