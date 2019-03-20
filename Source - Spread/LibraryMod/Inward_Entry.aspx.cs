using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

public partial class LibraryMod_Inward_Entry : System.Web.UI.Page
{

    #region FieldDeclaration
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    Dictionary<string, string> dicStaffList = new Dictionary<string, string>();
    DataSet dsprint = new DataSet();
    ArrayList colord = new ArrayList();
    DAccess2 da = new DAccess2();
    DataView dvhead = new DataView();
    Hashtable ht = new Hashtable();
    DataTable dtCommon = new DataTable();
    Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    Dictionary<string, string> dictitle = new Dictionary<string, string>();
    public SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    public SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings[0].ToString());
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlConnection ssql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string collcode = string.Empty;
    string libcode = string.Empty;
    string qrylibcode = string.Empty;
    string catcode = string.Empty;
    string shelfno = string.Empty;
    string posno = string.Empty;
    string sqlqry = string.Empty;
    static string libcodes = string.Empty;
    static string dEntryType = "";
    static string dtitlelang = "";
    static string dtitlelang1 = "";
    string activerow = "";
    string activecol = "";
    string accessdate = "";
    string rackno = "";
    string AlreadyLoad = "N";
    int fl = 0;
    int rflag = 0;
    int varCopies = 0;
    int maxca = 0;
    int save = 0;
    int Delete = 0;
    string language = "";
    string insertqry = "";
    string Attache = "";
    string cate = "";
    string price = "";
    string currntype = "";
    string callno = "";
    string titlelang = "";
    string authorlang = "";
    string pubplace = "";
    string updateqry = "";
    string reference = "";
    string Book_Type = "";
    string booktype = "";
    string billdate = "";
    string Calldes = "";
    string inwardtype = "";
    string pos = "";
    string posplc = "";
    string etitle = "";
    string eauthor = "";
    Boolean BookSave = false;
    string Acctime = "";
    string Field_Name1 = "";
    string que = "";
    Boolean Cellclick = false;
    DataSet dsbooksave = new DataSet();
    string g1 = "";
    int demin = 0;
    string delete = "";
    string get = "";
    int getcnt = 0;
    Boolean searchaccno = false;
    Boolean searchaccno1 = false;

    //Pageno
    Boolean pageno = false;
    int ivalue = 0;
    int curpage = 0;
    double pageSize1 = 0.0;
    int pagecnt = 0;
    int pgsize = 0;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("LibraryHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/LibraryMod/LibraryHome.aspx");
                    return;
                }
            }
          
                userCollegeCode = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
         
            if (!IsPostBack)
            {
                supp();
                Bindcollege();
                getLibPrivil();
                Type();
                search();
                txtfromdate.Attributes.Add("readonly", "readonly");
                txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Txtbilldate.Attributes.Add("readonly", "readonly");
                Txtbilldate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttodate.Attributes.Add("readonly", "readonly");
                txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                ddl_curren.Attributes.Add("onfocus", "frelig1()");
                ddl_status.Attributes.Add("onfocus", "frelig2()");
                ddl_atta.Attributes.Add("onfocus", "frelig3()");
                ddl_language.Attributes.Add("onfocus", "frelig4()");
                //ddl_Category.Attributes.Add("onfocus", "frelig7()");
                //ddl_Budget.Attributes.Add("onfocus", "frelig8()");
                //ddl_publishplace.Attributes.Add("onfocus", "frelig9()");
                //ddl_CallNo.Attributes.Add("onfocus", "frelig10()");
                ddl_booktype.Attributes.Add("onfocus", "frelig11()");
                ddlentrytype_SelectedIndexChanged(sender, e);
                ddllibrary_SelectedIndexChanged(sender, e);
                loadTitle();
                loaddepartment();
                LoadLanguage();
                loadcategory();
                SearchTitle();
                grdInward.Visible = false;
                //select_range.Visible = false;
                btndel.Visible = false;
                rptprint1.Visible = false;
                ddlsearcbook.Visible = false;
            }
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    public void supp()
    {
        try
        {
            ddlsupp.Items.Clear();
            ds.Clear();
            string sup = "SELECT DISTINCT ISNULL(VendorCompName,'') as Supplier_Name FROM CO_VendorMaster S WHERE LibraryFlag='1' and ISNULL(VendorCompName,'') <> ''  ORDER BY Supplier_Name ";
            ds = da.select_method_wo_parameter(sup, "text");
            // libr_name = ds.Tables[0].Rows[0]["subject"].ToString();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsupp.DataSource = ds;
                ddlsupp.DataTextField = "Supplier_Name";
                ddlsupp.DataValueField = "Supplier_Name";
                ddlsupp.DataBind();
                ddlsupp.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { da.sendErrorMail(ex, collcode, "Inward_entry.aspx"); }
    }

    #region College

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            dtCommon.Clear();
            ddlCollege.Enabled = false;
            dsprint.Clear();
            string qryUserCodeOrGroupCode = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
            {
                qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtCommon;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
                ddlCollege.Enabled = true;

                ddlstat_college.DataSource = dtCommon;
                ddlstat_college.DataTextField = "collname";
                ddlstat_college.DataValueField = "college_code";
                ddlstat_college.DataBind();
                ddlstat_college.SelectedIndex = 0;
                ddlstat_college.Enabled = true;

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        getLibPrivil();
        grdInward.Visible = false;
        btndel.Visible = false;
        rptprint1.Visible = false;
        //select_range.Visible = false;
    }
    #endregion

    #region Library

    public void Library(string LibCollection)
    {
        try
        {
            ddllibrary.Items.Clear();
            ds.Clear();
            string College = ddlCollege.SelectedValue.ToString();
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                dicQueryParameter.Clear();
                SelectQ = "select *,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCollection + " AND  college_code in('" + College + "') ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = d2.select_method_wo_parameter(SelectQ, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary.DataSource = ds;
                    ddllibrary.DataTextField = "lib_name";
                    ddllibrary.DataValueField = "lib_code";
                    ddllibrary.DataBind();

                    ddllibrary_sts.DataSource = ds;
                    ddllibrary_sts.DataTextField = "lib_name";
                    ddllibrary_sts.DataValueField = "lib_code";
                    ddllibrary_sts.DataBind();

                    ddl_txt_lib.DataSource = ds;
                    ddl_txt_lib.DataTextField = "lib_name";
                    ddl_txt_lib.DataValueField = "lib_code";
                    ddl_txt_lib.DataBind();

                    // ddllibrary.Items.Insert(0, "All");
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            Session["libcode"] = libcode;
            libcodes = Convert.ToString(Session["libcode"]);
            grdInward.Visible = false;
            btndel.Visible = false;
            rptprint1.Visible = false;
            //select_range.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    #endregion

    #region Type
    public void Type()
    {
        try
        {
            ddltype.Items.Clear();
            ddl_entrytype.Items.Clear();
            ddltype.Items.Add("Books");
            //ddltype.Items.Add("Periodicals");
            ddltype.Items.Add("News Paper");
            ddltype.Items.Add("Question Bank");
            //ddltype.Items.Add("Project Books");
            ddl_entrytype.Items.Add("Books");
            ddl_entrytype.Items.Add("News Paper");
            ddl_entrytype.Items.Add("Question Bank");


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            search();
            grdInward.Visible = false;
            btndel.Visible = false;
            rptprint1.Visible = false;
            txtsearch.Visible = false;
            //select_range.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }
    #endregion

    #region LoadTitle
    protected void txtsearch_OnTextChanged(object sender, EventArgs e)
    {
        //if (ddlsearch.Text == "Title")
        //    loadTitle();
    }

    public void loadTitle()
    {
        try
        {
            ddlsearcbook.Items.Clear();
            string titlesearch = "";
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (txtsearch.Text != "")
                titlesearch = "Title like '" + txtsearch.Text + "%'";
            string qrycurrentype = "SELECT DISTINCT ISNULL(Author,'') Author FROM bookdetails where  Lib_Code='" + libcode + "' " + titlesearch + " order by Author";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrycurrentype, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsearcbook.DataSource = ds;
                ddlsearcbook.DataTextField = "Author";
                ddlsearcbook.DataValueField = "Author";
                ddlsearcbook.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }
    #endregion

    #region Search

    public void search()
    {
        try
        {
            if (ddltype.SelectedIndex == 0)
            {
                ddlsearch.Items.Clear();
                ddlsearch.Items.Add("All");
                ddlsearch.Items.Add("Access Number");
                ddlsearch.Items.Add("Call No");
                ddlsearch.Items.Add("Title");
                ddlsearch.Items.Add("Author");
                ddlsearch.Items.Add("Status");
                ddlsearch.Items.Add("Department");
                ddlsearch.Items.Add("Subject Head");
                ddlsearch.Items.Add("Billno");
                ddlsearch.Items.Add("Purchased");
                ddlsearch.Items.Add("Donated");
                ddlsearch.Items.Add("Type");
                ddlsearch.Items.Add("Topics");
                ddlsearch.Items.Add("Category");
                ddlsearch.Items.Add("Language");
                ddlsearch.Items.Add("Access Number With Acronym");
            }
            //else if (ddltype.SelectedIndex == 1)
            //{
            //    ddlsearch.Items.Clear();
            //    ddlsearch.Items.Add("All");
            //    ddlsearch.Items.Add("Paper Name");
            //    ddlsearch.Items.Add("Title");
            //    ddlsearch.Items.Add("Type");
            //    ddlsearch.Items.Add("Department");
            //    ddlsearch.Items.Add("Language");
            //    ddlsearch.Items.Add("Periodicity");
            //    ddlsearch.Items.Add("Subject");

            //}
            else if (ddltype.SelectedIndex == 1)
            {
                ddlsearch.Items.Clear();
                ddlsearch.Items.Add("All");
                ddlsearch.Items.Add("Paper Name");
            }
            else
            {
                ddlsearch.Items.Clear();
                ddlsearch.Items.Add("All");
                ddlsearch.Items.Add("Access Number");
                ddlsearch.Items.Add("Title");
                ddlsearch.Items.Add("Paper Name");
                ddlsearch.Items.Add("Semester");
                ddlsearch.Items.Add("Sem.Month");
                ddlsearch.Items.Add("Sem.Year");
            }
            //else
            //{
            //    ddlsearch.Items.Clear();
            //    ddlsearch.Items.Add("All");
            //    ddlsearch.Items.Add("Roll No");
            //    ddlsearch.Items.Add("Access No");
            //    ddlsearch.Items.Add("Guide Name");
            //    ddlsearch.Items.Add("Type Of Project");
            //    ddlsearch.Items.Add("Company Name");
            //    ddlsearch.Items.Add("Area Of Project");
            //    ddlsearch.Items.Add("Project Title");
            //    ddlsearch.Items.Add("Month");
            //    ddlsearch.Items.Add("Year");
            //    ddlsearch.Items.Add("Departmentwise");
            //}
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }




    }

    protected void ddlSearchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtsearch.Font.Name = "Arial";
            ddlsearcbook.Items.Clear();
            chkbetween.Visible = false;
            txtsearch.Visible = false;
            grdInward.Visible = false;
            //select_range.Visible = false;
            btndel.Visible = false;
            rptprint1.Visible = false;
            txtsearch.Text = "";
            if (ddltype.Text == "Books")
            {
                if (ddlsearch.SelectedIndex == 1)
                {
                    que = "Acc_No";
                    txtsearch.Visible = true;
                    chkbetween.Visible = true;
                    ddlsearcbook.Visible = false;
                    ddlsearch_title.Visible = false;
                }
                else if (ddlsearch.SelectedIndex == 2)
                {
                    que = "call_no";
                    txtsearch.Visible = true;
                    chkbetween.Visible = false;
                    ddlsearcbook.Visible = false;
                    ddlsearch_title.Visible = false;
                }
                else if (ddlsearch.SelectedIndex == 3)
                {
                    que = "Title";
                    txtsearch.Visible = true;
                    //txtsearch.Font.Name = "Amudham";
                    chkbetween.Visible = false;
                    ddlsearcbook.Visible = true;
                    ddlsearch_title.Visible = true;
                    loadTitle();
                }
                else if (ddlsearch.SelectedIndex == 4)
                {

                    que = "author";
                    txtsearch.Visible = true;
                    ddlsearcbook.Visible = false;
                    chkbetween.Visible = false;
                    ddlsearch_title.Visible = false;
                }
                else if (ddlsearch.SelectedIndex == 5)
                {
                    que = "book_status";
                    txtsearch.Visible = false;
                    ddlsearch_title.Visible = false;
                    ddlsearcbook.Visible = true;
                    ddlsearcbook.Items.Clear();
                    string sqlqrytitle = "select distinct rtrim(ltrim(book_Status)) book_Status from bookdetails where rtrim(ltrim(book_Status)) <> ''";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sqlqrytitle, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlsearcbook.DataSource = ds;
                        ddlsearcbook.DataTextField = "book_Status";
                        ddlsearcbook.DataValueField = "book_Status";
                        ddlsearcbook.DataBind();
                    }
                }
                else if (ddlsearch.SelectedIndex == 6)
                {

                    que = "dept_code";
                    ddlsearcbook.Visible = true;
                    chkbetween.Visible = false;
                    txtsearch.Visible = false;
                    ddlsearch_title.Visible = false;
                    loaddepartment();
                }
                else if (ddlsearch.SelectedIndex == 7)
                {
                    que = "subject";
                    txtsearch.Visible = true;
                    ddlsearcbook.Visible = false;
                    chkbetween.Visible = false;
                    ddlsearch_title.Visible = false;
                }
                else if (ddlsearch.SelectedIndex == 8)
                {
                    que = "bill_no";
                    txtsearch.Visible = true;
                    ddlsearcbook.Visible = false;
                    chkbetween.Visible = false;
                    ddlsearch_title.Visible = false;
                }
                else if (ddlsearch.SelectedIndex == 9)
                {
                    que = "pur_don";
                    txtsearch.Visible = false;
                    ddlsearcbook.Visible = false;
                    chkbetween.Visible = false;
                    ddlsearch_title.Visible = false;
                }
                else if (ddlsearch.SelectedIndex == 10)
                {
                    que = "pur_don";
                    txtsearch.Visible = false;
                    ddlsearcbook.Visible = false;
                    chkbetween.Visible = false;
                    ddlsearch_title.Visible = false;
                }
                else if (ddlsearch.SelectedIndex == 11)
                {
                    que = "typeofbook";
                    ddlsearch_title.Visible = false;
                    txtsearch.Visible = false;
                    ddlsearcbook.Visible = true;
                    ddlsearcbook.Items.Clear();
                    ddlsearcbook.Items.Add("Ordinary");
                    ddlsearcbook.Items.Add("Gift");
                    ddlsearcbook.Items.Add("Specimen Copy");
                }
                else if (ddlsearch.SelectedIndex == 12)
                {
                    que = "topics";
                    txtsearch.Visible = true;
                    ddlsearcbook.Visible = false;
                    chkbetween.Visible = false;
                    ddlsearch_title.Visible = false;
                }
                else if (ddlsearch.SelectedIndex == 13)
                {
                    que = "Category";
                    txtsearch.Visible = false;
                    ddlsearcbook.Visible = true;
                    ddlsearch_title.Visible = false;
                    loadcategory();
                }
                else if (ddlsearch.SelectedIndex == 14)
                {
                    que = "Language";
                    txtsearch.Visible = false;
                    ddlsearcbook.Visible = true;
                    ddlsearch_title.Visible = false;
                    LoadLanguage();
                }
                else if (ddlsearch.SelectedIndex == 15)
                {
                    que = "Acc_No";
                    txtsearch.Visible = true;
                    chkbetween.Visible = true;
                    ddlsearcbook.Visible = false;
                    ddlsearch_title.Visible = false;
                }
            }
            else if (ddltype.Text == "News Paper")
            {
                if (ddlsearch.Text == "All")
                {
                    chkbetween.Visible = false;
                    txtsearch.Visible = false;
                    ddlsearcbook.Visible = false;
                    ddlsearch_title.Visible = false;
                    txtsearch.Text = "";
                }
                if (ddlsearch.Text == "Paper Name")
                {
                    txtsearch.Visible = true;
                    ddlsearch_title.Visible = false;
                    ddlsearcbook.Visible = false;
                    chkbetween.Visible = false;
                }
            }
            else if (ddltype.Text == "Question Bank")
            {
                if (ddlsearcbook.Text == "All")
                {
                    txtsearch.Text = "";
                    ddlsearch_title.Visible = false;
                    ddlsearcbook.Visible = false;
                    chkbetween.Visible = false;
                    Field_Name1 = "";
                }
                else if (ddlsearcbook.Text == "Title")
                {
                    txtsearch.Text = "";
                    txtsearch.Visible = true;
                    ddlsearch_title.Visible = false;
                    ddlsearcbook.Visible = false;
                    chkbetween.Visible = false;
                    Field_Name1 = "university_question.title";
                }
                else if (ddlsearcbook.Text == "Access Number")
                {
                    txtsearch.Text = "";
                    txtsearch.Visible = true;
                    ddlsearch_title.Visible = false;
                    ddlsearcbook.Visible = false;
                    chkbetween.Visible = false;
                    Field_Name1 = "university_question.access_code";
                }
                else if (ddlsearcbook.Text == "Course Name")
                {
                    txtsearch.Text = "";
                    txtsearch.Visible = true;
                    ddlsearch_title.Visible = false;
                    ddlsearcbook.Visible = false;
                    chkbetween.Visible = false;
                    Field_Name1 = "course.course_name";
                }
                else if (ddlsearcbook.Text == "Dept.Name")
                {
                    txtsearch.Text = "";
                    txtsearch.Visible = true;
                    ddlsearch_title.Visible = false;
                    ddlsearcbook.Visible = false;
                    chkbetween.Visible = false;
                    Field_Name1 = "department.dept_name";
                }
                else if (ddlsearcbook.Text == "Sem.Month")
                {
                    txtsearch.Text = "";
                    txtsearch.Visible = true;
                    ddlsearch_title.Visible = false;
                    ddlsearcbook.Visible = false;
                    chkbetween.Visible = false;
                    Field_Name1 = "university_question.sem_month";
                }

                else if (ddlsearcbook.Text == "Paper Name")
                {
                    txtsearch.Text = "";
                    txtsearch.Visible = true;
                    ddlsearch_title.Visible = false;
                    ddlsearcbook.Visible = false;
                    chkbetween.Visible = false;
                    Field_Name1 = "university_question.paper_name";
                }
                else if (ddlsearcbook.Text == "Semester")
                {
                    txtsearch.Text = "";
                    txtsearch.Visible = true;
                    ddlsearch_title.Visible = false;
                    ddlsearcbook.Visible = false;
                    chkbetween.Visible = false;
                    Field_Name1 = "university_question.semester";
                }
                else
                {
                    txtsearch.Text = "";
                    txtsearch.Visible = true;
                    ddlsearch_title.Visible = false;
                    ddlsearcbook.Visible = false;
                    chkbetween.Visible = false;
                    Field_Name1 = "university_question.sem_year";
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void ddlsearcbook_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdInward.Visible = false;
        btndel.Visible = false;
        rptprint1.Visible = false;
        //select_range.Visible = false;
    }

    protected void chkbetween_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            grdInward.Visible = false;
            btndel.Visible = false;
            rptprint1.Visible = false;
            // select_range.Visible = false;
            if (chkbetween.Checked == true)
            {
                Txtbet1.Visible = true;
                Txtbet2.Visible = true;
            }
            else
            {
                Txtbet1.Visible = false;
                Txtbet2.Visible = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void ddlSearchchange_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdInward.Visible = false;
        btndel.Visible = false;
        rptprint1.Visible = false;
        //select_range.Visible = false;
    }

    #endregion

    #region Category
    public void loadcategory()
    {
        try
        {
            ddl_Category.Items.Clear();
            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddl_txt_lib.Items.Count > 0)
                libcode = Convert.ToString(ddl_txt_lib.SelectedValue);
            if (libcode != "All" && libcode != "")
            {
                qrylibcode = "and lib_code='" + libcode + "'";
            }
            string qrycate = "select distinct cat from libcat where  cat <> '' and college_code='" + collcode + "' " + qrylibcode + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrycate, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Category.DataSource = ds;
                ddl_Category.DataTextField = "cat";
                ddl_Category.DataValueField = "cat";
                ddl_Category.DataBind();

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }
    #endregion

    #region LoadBudHead
    public void LoadBudHead()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            string qrycallno = "SELECT DISTINCT  ISNULL(Budget_Head,'') Budget_Head FROM BookDetails WHERE  Lib_Code ='" + libcode + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrycallno, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Budget.DataSource = ds;
                ddl_Budget.DataTextField = "Budget_Head";
                ddl_Budget.DataValueField = "Budget_Head";
                ddl_Budget.DataBind();

                ddl_non_budget.DataSource = ds;
                ddl_non_budget.DataTextField = "Budget_Head";
                ddl_non_budget.DataValueField = "Budget_Head";
                ddl_non_budget.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }
    #endregion

    #region Load CallDes

    public void Loadcalldes()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            string qrycalldes = "select distinct CallNoDescription from CallNoEntry where CallNoDescription<>''";
            //WHERE  Lib_Code ='" + libcode + "' 
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrycalldes, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddl_Description.DataSource = ds;
                ddl_Description.DataTextField = "CallNoDescription";
                ddl_Description.DataValueField = "CallNoDescription";
                ddl_Description.DataBind();
                ddl_Description.Items.Insert(0, "");

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    #endregion

    #region LoadCall_No

    public void LoadCallNo(string calldescr)
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            string qrycallno = "SELECT DISTINCT CallNo FROM CallNoEntry WHERE  CallNoDescription like '" + calldescr + "%' and CallNo<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrycallno, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_CallNo.DataSource = ds;
                ddl_CallNo.DataTextField = "CallNo";
                ddl_CallNo.DataValueField = "CallNo";
                ddl_CallNo.DataBind();
                ddl_CallNo.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    #endregion

    #region Loadlang
    public void LoadLanguage()
    {
        try
        {
            ddlsearcbook.Items.Clear();
            ddlsearcbook.Visible = true;
            string sqlqrylang = "SELECT DISTINCT ISNULL(Language,'') Language FROM Bookdetails";
            ds.Clear();
            ds = d2.select_method_wo_parameter(sqlqrylang, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsearcbook.DataSource = ds;
                ddlsearcbook.DataTextField = "Language";
                ddlsearcbook.DataValueField = "Language";
                ddlsearcbook.DataBind();
                //ddlsendlang.DataSource = ds;
                //ddlsendlang.DataTextField = "Language";
                //ddlsendlang.DataValueField = "Language";
                //ddlsendlang.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    public void loadlan()
    {
        ddl_title_lan.Items.Clear();
        ddl_title_lan.Items.Add("English");
        ddl_title_lan.Items.Add("Tamil");

        ddl_Author_lan.Items.Clear();
        ddl_Author_lan.Items.Add("English");
        ddl_Author_lan.Items.Add("Tamil");
    }
    #endregion

    #region Categoryclick
    public void Categoryclick()
    {
        try
        {
            string rno = "";
            string sno = "";
            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (ddl_Category.Items.Count > 0)
                catcode = Convert.ToString(ddl_Category.SelectedItem.Text);
            string qrycat = "select rno,sno from libcat where lib_code='" + libcode + "' and college_code='" + collcode + "' and cat='" + catcode + "' order by rno";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrycat, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                rno = Convert.ToString(ds.Tables[0].Rows[0]["rno"]);
                sno = Convert.ToString(ds.Tables[0].Rows[0]["sno"]);
                string rcopies = d2.GetFunction("select no_of_copies from rack_master where rack_no='" + rno + "' and lib_code='" + libcode + "'");
                string rmax = d2.GetFunction("select max_capacity from rack_master where rack_no='" + rno + "' and lib_code='" + libcode + "'");
                string scopies = d2.GetFunction("select no_of_copies from rackrow_master where rack_no='" + rno + "'and row_no='" + sno + "' and lib_code='" + libcode + "'");
                string smax = d2.GetFunction("select max_capacity from rackrow_master where rack_no='" + rno + "' and row_no='" + sno + "' and lib_code='" + libcode + "'");
                if (!string.IsNullOrEmpty(rcopies) && !string.IsNullOrEmpty(rmax))
                {
                    if (rcopies != rmax)
                        ddl_Rack.Text = rno;
                }
                if (rno == "")
                    ddl_Rack.SelectedIndex = -1;
                if (!string.IsNullOrEmpty(scopies) && !string.IsNullOrEmpty(smax))
                {
                    if (scopies != smax)
                        ddl_shelf.Text = sno;
                }
                if (sno == "")
                    ddl_shelf.SelectedIndex = -1;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void ddl_Category_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdInward.Visible = false;
            btndel.Visible = false;
            rptprint1.Visible = false;
            //select_range.Visible = false;
            Categoryclick();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }
    #endregion

    #region loagRackno

    public void loagRackno()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            string qryrack = "select distinct rack_no from Rack_master  where  lib_code='" + libcode + "'  order by rack_no";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qryrack, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Rack.DataSource = ds;
                ddl_Rack.DataTextField = "rack_no";
                ddl_Rack.DataValueField = "rack_no";

                ddl_Rack.DataBind();
                ddl_Rack.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    #endregion

    #region Shelf

    public void loadshelf()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (ddl_Rack.Items.Count > 0)
                rackno = Convert.ToString(ddl_Rack.SelectedItem.Text);
            string qryshelf = "SELECT distinct row_no,len(row_no) from rackrow_master where rack_no='" + rackno + "' and lib_code='" + libcode + "' order by len(row_no),row_no";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qryshelf, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_shelf.DataSource = ds;
                ddl_shelf.DataTextField = "row_no";
                ddl_shelf.DataValueField = "row_no";
                ddl_shelf.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    #endregion

    #region  loadposition

    public void loadposition()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (ddl_Rack.Items.Count > 0)
                rackno = Convert.ToString(ddl_Rack.SelectedItem.Text);
            if (ddl_shelf.Items.Count > 0)
                shelfno = Convert.ToString(ddl_shelf.SelectedItem.Text);
            string qryposition = "SELECT distinct Pos_No from RowPos_Master WHERE Rack_No ='" + rackno + "' AND Row_No ='" + shelfno + "' AND Lib_Code ='" + libcode + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qryposition, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_posi.DataSource = ds;
                ddl_posi.DataTextField = "Pos_No";
                ddl_posi.DataValueField = "Pos_No";
                ddl_posi.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    #endregion

    #region LoadPosPlace
    public void LoadPosPlace()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (ddl_Rack.Items.Count > 0)
                rackno = Convert.ToString(ddl_Rack.SelectedItem.Text);
            if (ddl_shelf.Items.Count > 0)
                shelfno = Convert.ToString(ddl_shelf.SelectedItem.Text);
            if (ddl_posi.Items.Count > 0)
                posno = Convert.ToString(ddl_posi.SelectedItem.Text);
            string qrypositionplace = "SELECT ISNULL(Max_Capacity,0) Max_Capacity FROM RowPos_Master WHERE Rack_No ='" + rackno + "' AND Row_No ='" + shelfno + "' AND Pos_No='" + posno + "' AND Lib_Code ='" + libcode + "'  ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrypositionplace, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_posplace.DataSource = ds;
                ddl_posplace.DataTextField = "Max_Capacity";
                ddl_posplace.DataValueField = "Max_Capacity";
                ddl_posplace.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }
    #endregion

    #region CurrencyType
    public void GetCurrencyType()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            string qrycurrentype = "SELECT DISTINCT ISNULL(currency_type,'') currency_type FROM currency_convertion";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrycurrentype, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_curren.DataSource = ds;
                ddl_curren.DataTextField = "currency_type";
                ddl_curren.DataValueField = "currency_type";
                ddl_curren.DataBind();

                ddcurrency.DataSource = ds;
                ddcurrency.DataTextField = "currency_type";
                ddcurrency.DataValueField = "currency_type";
                ddcurrency.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }
    #endregion

    #region LoadStatus
    public void LoadStatus()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            string qrycurrentype = "SELECT DISTINCT ISNULL(book_status,'') book_status FROM BookDetails WHERE  Lib_Code ='" + libcode + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrycurrentype, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_status.DataSource = ds;
                ddl_status.DataTextField = "book_status";
                ddl_status.DataValueField = "book_status";
                ddl_status.DataBind();

                dd_sts.DataSource = ds;
                dd_sts.DataTextField = "book_status";
                dd_sts.DataValueField = "book_status";
                dd_sts.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }
    #endregion

    #region Load_Attachement

    public void LoadAttache()
    {
        if (ddllibrary.Items.Count > 0)
            libcode = Convert.ToString(ddllibrary.SelectedValue);
        string qrycurrentype = "SELECT DISTINCT Attachment_name FROM Attachment WHERE  Lib_Code ='" + libcode + "' and Attachment_name<>''";
        ds.Clear();
        ds = d2.select_method_wo_parameter(qrycurrentype, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_atta.DataSource = ds;
            ddl_atta.DataTextField = "Attachment_name";
            ddl_atta.DataValueField = "Attachment_name";
            ddl_atta.DataBind();

            ddl_mat.DataSource = ds;
            ddl_mat.DataTextField = "Attachment_name";
            ddl_mat.DataValueField = "Attachment_name";
            ddl_mat.DataBind();
        }
    }
    #endregion

    #region Load_language
    public void LoadLang()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            string qrycurrentype = "SELECT DISTINCT ISNULL(language,'') language FROM BookDetails WHERE  Lib_Code ='" + libcode + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrycurrentype, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_language.DataSource = ds;
                ddl_language.DataTextField = "language";
                ddl_language.DataValueField = "language";
                ddl_language.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); } { }
    }

    #endregion

    #region LoadPubPlace
    public void LoadPubPlace()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            string qrypubplc = "SELECT DISTINCT  ISNULL(pub_place,'') pub_place FROM BookDetails WHERE  Lib_Code ='" + libcode + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrypubplc, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_publishplace.DataSource = ds;
                ddl_publishplace.DataTextField = "pub_place";
                ddl_publishplace.DataValueField = "pub_place";
                ddl_publishplace.DataBind();
                ddl_publishplace.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }
    #endregion

    #region LoadBookType
    public void LoadBookType()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            string qrybook = "SELECT DISTINCT  ISNULL(typeofbook,'') typeofbook FROM BookDetails WHERE  Lib_Code ='" + libcode + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrybook, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_booktype.DataSource = ds;
                ddl_booktype.DataTextField = "typeofbook";
                ddl_booktype.DataValueField = "typeofbook";
                ddl_booktype.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }
    #endregion

    #region Loaddepatment
    public void loaddepartment()
    {
        try
        {
            ddlsearcbook.Items.Clear();
            ddDepart.Items.Clear();
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            string qrybook = "SELECT DISTINCT  ISNULL(Dept_code,'') Dept_code FROM BookDetails WHERE  Lib_Code ='" + libcode + "' order by Dept_code ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrybook, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddDepart.DataSource = ds;
                ddDepart.DataTextField = "Dept_code";
                ddDepart.DataValueField = "Dept_code";
                ddDepart.DataBind();

                ddlsearcbook.DataSource = ds;
                ddlsearcbook.DataTextField = "Dept_code";
                ddlsearcbook.DataValueField = "Dept_code";
                ddlsearcbook.DataBind();
                ddlsearcbook.Items.Insert(0, "All");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }
    #endregion

    #region loadmonth
    public void loadmonth()
    {
        try
        {
            ddl_monYear.Items.Add("January");
            ddl_monYear.Items.Add("February");
            ddl_monYear.Items.Add("March");
            ddl_monYear.Items.Add("April");
            ddl_monYear.Items.Add("May");
            ddl_monYear.Items.Add("June");
            ddl_monYear.Items.Add("July");
            ddl_monYear.Items.Add("August");
            ddl_monYear.Items.Add("September");
            ddl_monYear.Items.Add("October");
            ddl_monYear.Items.Add("November");
            ddl_monYear.Items.Add("December");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }
    #endregion

    #region LoadRack
    public void LoadRack()
    {
        try
        {
            ddlsts_rackno.Items.Clear();
            string libcode1 = "";
            if (ddllibrary_sts.Items.Count > 0)
                libcode1 = Convert.ToString(ddllibrary_sts.SelectedValue);
            string qrybook = "SELECT DISTINCT  ISNULL(rack_no,'') rack_no FROM rack_master WHERE  Lib_Code ='" + libcode1 + "' order by rack_no ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrybook, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsts_rackno.DataSource = ds;
                ddlsts_rackno.DataTextField = "rack_no";
                ddlsts_rackno.DataValueField = "rack_no";
                ddlsts_rackno.DataBind();
                ddlsts_rackno.Items.Insert(0, "All");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }
    #endregion

    #region SearchTitle
    public void SearchTitle()
    {
        try
        {
            ddlsearch_title.Items.Clear();
            ddlsearch_title.Items.Add("English");
            ddlsearch_title.Items.Add("Tamil");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void ddlsearch_title_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtsearch.Text = "";
            if (ddlsearch_title.Text == "Tamil")
                txtsearch.Font.Name = "Amudham";
            else
                txtsearch.Font.Name = "Arial";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    #endregion

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetTitle(string prefixText)
    {
        WebService ws = new WebService();
        List<string> title = new List<string>();
        string query = string.Empty;
        if (dEntryType == "Books")
        {
            query = "SELECT DISTINCT Title FROM BookDetails WHERE Title LIKE '" + prefixText + "%' and Lib_Code='" + libcodes + "'";
            if (dtitlelang == "Tamil")
                query += " AND ISNULL(AuthorLanguage,0) = 1 ";
            else
                query += " AND ISNULL(AuthorLanguage,0) = 0";
        }
        else if (dEntryType == "Question Bank")
        {
            query = "SELECT DISTINCT Title FROM University_Question WHERE Title LIKE '" + prefixText + "%' AND Lib_Code ='" + libcodes + "'";

        }
        title = ws.Getname(query);
        return title;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getdeptname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        query = "SELECT DISTINCT isnull(Dept_Code,'') Dept_Code  FROM BookDetails  WHERE Dept_Code LIKE '" + prefixText + "%' AND Lib_Code ='" + libcodes + "'";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetAuthor(string prefixText)
    {
        WebService ws = new WebService();
        List<string> Author = new List<string>();

        string query = "";
        query = "SELECT DISTINCT Author FROM BookDetails WHERE Author LIKE  '" + prefixText + "%' and Lib_Code='" + libcodes + "'";
        if (dtitlelang1 == "Tamil")
            query += " AND ISNULL(AuthorLanguage,0) = 1 ";
        else
            query += " AND ISNULL(AuthorLanguage,0) = 0";

        Author = ws.Getname(query);
        return Author;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetSecAuthor(string prefixText)
    {
        WebService ws = new WebService();
        List<string> SecAuthor = new List<string>();
        string query = "";
        query = "SELECT DISTINCT Author FROM BookDetails WHERE Author LIKE  '" + prefixText + "%' and Lib_Code='" + libcodes + "'";
        SecAuthor = ws.Getname(query);
        return SecAuthor;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetPublisher(string prefixText)
    {
        WebService ws = new WebService();
        List<string> Publisher = new List<string>();
        string query = "";
        query = "SELECT DISTINCT Publisher FROM BookDetails WHERE Publisher LIKE  '" + prefixText + "%' and Lib_Code='" + libcodes + "'";
        Publisher = ws.Getname(query);
        return Publisher;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetSubject(string prefixText)
    {
        WebService ws = new WebService();
        List<string> title = new List<string>();
        string query = "";
        query = "SELECT DISTINCT Subject FROM BookDetails WHERE Subject  LIKE  '" + prefixText + "%' and Lib_Code='" + libcodes + "'";
        title = ws.Getname(query);
        return title;
    }

    protected void chkredate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkredate.Checked == true)
            {
                txtfromdate.Enabled = true;
                txttodate.Enabled = true;
            }
            else
            {
                txtfromdate.Enabled = false;
                txttodate.Enabled = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    public void txt_title_Change(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    public void txt_depart_Change(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    #region Add

    protected void btnadd_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            ddlsearcbook.Visible = false;
            string Libraryname = "";
            if (ddllibrary.Items.Count > 0)
                Libraryname = Convert.ToString(ddllibrary.SelectedItem.Text);
            if (ddllibrary.Items.Count > 0)
                libcodes = Convert.ToString(ddllibrary.SelectedValue);
            if (!string.IsNullOrEmpty(Libraryname))
            {
                popview.Visible = true;
                btn_Save.Visible = true;
                btn_Save.ImageUrl = "~/LibImages/save.jpg";
                btn_Delete.Visible = false;
                //ddl_txt_lib.Text = Libraryname;
                Type();
                loadcategory();
                Auto_AccessNo();
                //LoadLanguage();
                Categoryclick();
                LoadBudHead();
                loadinward();
                loagRackno();
                loadshelf();
                loadposition();
                LoadPosPlace();
                GetCurrencyType();
                LoadStatus();
                LoadAttache();
                LoadLang();
                Loadcalldes();
                LoadPubPlace();
                LoadBookType();
                loadlan();
                loadthridAuthor();
                IsNonBook.Checked = false;
                lblnonbook.Visible = false;
                txtnonbook.Visible = false;
                TxtMultiple.Visible = false;
                txt_date_acc.Attributes.Add("readonly", "readonly");
                txt_date_acc.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Txtbilldate.Attributes.Add("readonly", "readonly");
                Txtbilldate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Txtbilldate.Enabled = false;


                txt_Offer.Text = "";
                txt_title.Text = "";
                txt_author.Text = "";
                Txt_pub.Text = "";
                Txt_sub.Text = "";
                Txt_edit.Text = "";
                txvo.Text = "";
                txtpart.Text = "";
                txt_billno.Text = "";
                Txtbilldate.Text = "";
                ddl_CallNo.Text = "";
                txt_depart.Text = "";
                txt_remarks.Text = "";
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select the Library";
                return;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void ddl_txt_lib_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Auto_AccessNo();
        loadAddRackno();
        loadAddshelf();
        loadAddposition();
        LoadAddPosPlace();
        loadcategory();
    }

    public void loadAddRackno()
    {
        try
        {
            if (ddl_txt_lib.Items.Count > 0)
                libcode = Convert.ToString(ddl_txt_lib.SelectedValue);
            string qryrack = "select distinct rack_no from Rack_master  where  lib_code='" + libcode + "'  order by rack_no";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qryrack, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Rack.DataSource = ds;
                ddl_Rack.DataTextField = "rack_no";
                ddl_Rack.DataValueField = "rack_no";

                ddl_Rack.DataBind();
                ddl_Rack.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    public void loadAddshelf()
    {
        try
        {
            if (ddl_txt_lib.Items.Count > 0)
                libcode = Convert.ToString(ddl_txt_lib.SelectedValue);
            if (ddl_Rack.Items.Count > 0)
                rackno = Convert.ToString(ddl_Rack.SelectedItem.Text);
            string qryshelf = "SELECT distinct row_no,len(row_no) from rackrow_master where rack_no='" + rackno + "' and lib_code='" + libcode + "' order by len(row_no),row_no";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qryshelf, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_shelf.DataSource = ds;
                ddl_shelf.DataTextField = "row_no";
                ddl_shelf.DataValueField = "row_no";
                ddl_shelf.DataBind();
                ddl_shelf.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    public void loadAddposition()
    {
        try
        {
            if (ddl_txt_lib.Items.Count > 0)
                libcode = Convert.ToString(ddl_txt_lib.SelectedValue);
            if (ddl_Rack.Items.Count > 0)
                rackno = Convert.ToString(ddl_Rack.SelectedItem.Text);
            if (ddl_shelf.Items.Count > 0)
                shelfno = Convert.ToString(ddl_shelf.SelectedItem.Text);
            string qryposition = "SELECT distinct Pos_No from RowPos_Master WHERE Rack_No ='" + rackno + "' AND Row_No ='" + shelfno + "' AND Lib_Code ='" + libcode + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qryposition, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_posi.DataSource = ds;
                ddl_posi.DataTextField = "Pos_No";
                ddl_posi.DataValueField = "Pos_No";
                ddl_posi.DataBind();
                ddl_posi.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    public void LoadAddPosPlace()
    {
        try
        {
            if (ddl_txt_lib.Items.Count > 0)
                libcode = Convert.ToString(ddl_txt_lib.SelectedValue);
            if (ddl_Rack.Items.Count > 0)
                rackno = Convert.ToString(ddl_Rack.SelectedItem.Text);
            if (ddl_shelf.Items.Count > 0)
                shelfno = Convert.ToString(ddl_shelf.SelectedItem.Text);
            if (ddl_posi.Items.Count > 0)
                posno = Convert.ToString(ddl_posi.SelectedItem.Text);
            string qrypositionplace = "SELECT ISNULL(Max_Capacity,0) Max_Capacity FROM RowPos_Master WHERE Rack_No ='" + rackno + "' AND Row_No ='" + shelfno + "' AND Pos_No='" + posno + "' AND Lib_Code ='" + libcode + "'  ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrypositionplace, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_posplace.DataSource = ds;
                ddl_posplace.DataTextField = "Max_Capacity";
                ddl_posplace.DataValueField = "Max_Capacity";
                ddl_posplace.DataBind();
                ddl_posplace.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    #endregion

    #region Delete

    protected void btndel_Click(object sender, EventArgs e)
    {
        try
        {
            int selectedcount = 0;
            foreach (GridViewRow gvrow in grdInward.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    selectedcount++;
                }
            }
            if (selectedcount == 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select the book to delete";
                return;
            }
            else
            {
                Diveleterecord.Visible = true;
                lbl_Diveleterecord.Text = "Do you want to Delete this Record?";
            }
            // Page.MaintainScrollPositionOnPostBack = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void btn_detele_yes__record_Click(object sender, EventArgs e)
    {
        try
        {
            string getbook = "";
            string getbook1 = "";
            string deletebook = "";
            int deletere = 0;

            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);

            if (grdInward.Rows.Count > 0)
            {
                if (ddltype.Text == "Books")
                {
                    foreach (GridViewRow gvrow in grdInward.Rows)
                    {
                        System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                        if (chk.Checked == true)
                        {
                            int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                            string accnum = Convert.ToString(grdInward.Rows[RowCnt].Cells[2].Text);
                            string bookid = Convert.ToString(grdInward.Rows[RowCnt].Cells[5].Text);
                            getbook = d2.GetFunction("select isnull(count(*),0) from borrow where acc_no ='" + accnum + "' AND Lib_Code ='" + libcode + "' and book_status<>'Available'");
                            if (getbook == "0")
                            {
                                getbook1 = d2.GetFunction("select isnull(count(*),0) from priority_studstaff where access_number ='" + accnum + "' AND Lib_Code ='" + libcode + "'");
                                if (getbook1 == "0")
                                {
                                    deletebook = "DELETE FROM BookDetails where acc_no ='" + accnum + "' AND BookID ='" + bookid + "' AND Lib_Code ='" + libcode + "'";
                                    deletere = d2.update_method_wo_parameter(deletebook, "Text");
                                }
                            }
                        }
                    }
                }
                else if (ddltype.Text == "News Paper")
                {
                }
                else
                {
                }
            }

            if (deletere > 0)
            {
                Diveleterecord.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Books other than issued are deleted sucessfully";
                btngo_Click(sender, e);
            }
            else
            {
                Diveleterecord.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Books are not deleted";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void btn_detele_no__recordClick(object sender, EventArgs e)
    {
        try
        {
            Diveleterecord.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    #endregion

    #region Go

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsgetbook = new DataSet();
            if (ddltype.Text == "Books")
            {
                if (ddlsearch.SelectedIndex == 1)
                    que = "Acc_No";
                else if (ddlsearch.SelectedIndex == 2)
                    que = "call_no";
                else if (ddlsearch.SelectedIndex == 3)
                    que = "Title";
                else if (ddlsearch.SelectedIndex == 4)
                    que = "author";
                else if (ddlsearch.SelectedIndex == 5)
                    que = "book_status";
                else if (ddlsearch.SelectedIndex == 6)
                    que = "dept_code";
                else if (ddlsearch.SelectedIndex == 7)
                    que = "subject";
                else if (ddlsearch.SelectedIndex == 8)
                    que = "bill_no";
                else if (ddlsearch.SelectedIndex == 9)
                    que = "pur_don";
                else if (ddlsearch.SelectedIndex == 10)
                    que = "pur_don";
                else if (ddlsearch.SelectedIndex == 11)
                    que = "typeofbook";
                else if (ddlsearch.SelectedIndex == 12)
                    que = "topics";
                else if (ddlsearch.SelectedIndex == 13)
                    que = "Category";
                else if (ddlsearch.SelectedIndex == 14)
                    que = "Language";
            }
            else if (ddltype.Text == "Question Bank")
            {
                if (ddlsearcbook.Text == "All")
                    Field_Name1 = "";
                else if (ddlsearcbook.Text == "Title")
                    Field_Name1 = "university_question.title";
                else if (ddlsearcbook.Text == "Access Number")
                    Field_Name1 = "university_question.access_code";
                else if (ddlsearcbook.Text == "Course Name")
                    Field_Name1 = "course.course_name";
                else if (ddlsearcbook.Text == "Dept.Name")
                    Field_Name1 = "department.dept_name";
                else if (ddlsearcbook.Text == "Sem.Month")
                    Field_Name1 = "university_question.sem_month";
                else if (ddlsearcbook.Text == "Paper Name")
                    Field_Name1 = "university_question.paper_name";
                else if (ddlsearcbook.Text == "Semester")
                    Field_Name1 = "university_question.semester";
                else
                    Field_Name1 = "university_question.sem_year";
            }
            string searchtext1 = Convert.ToString(ddlsearch.Text);
            dsgetbook = getBookdetails();
            if (dsgetbook.Tables.Count > 0 && dsgetbook.Tables[0].Rows.Count > 0)
            {
                if (ddltype.Text == "Books")
                    loadspreadBook(dsgetbook);
                else if (ddltype.Text == "News paper")
                    loadspreadNews(dsgetbook);
                else
                    loadspreadQues(dsgetbook);
            }
            else
            {
                if (searchaccno)
                {
                    if (chkbetween.Checked == true)
                    {
                        if (Txtbet1.Text == "" || Txtbet2.Text == "")
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Enter the Access No ";
                            return;
                        }
                        if (Txtbet1.Text != "" || Txtbet2.Text != "")
                        {
                            int From = Convert.ToInt32(Txtbet1.Text);
                            int To = Convert.ToInt32(Txtbet2.Text);
                            if (From > To)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "To Access No. should be less then from Access No.";
                                return;
                            }
                        }
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Enter the Search Key For '" + searchtext1 + "'";
                    }
                }
                else if (searchaccno1)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Enter '" + searchtext1 + "'";
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Records Found";
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    private DataSet getBookdetails()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value

            string sqlgetbook = "";
            string search = "";
            string searchbook = "";
            string searchtext = "";
            string firstdate = Convert.ToString(txtfromdate.Text);
            string seconddate = Convert.ToString(txttodate.Text);
            string dt = string.Empty;
            string dt1 = string.Empty;

            string[] split = firstdate.Split('/');
            dt = split[1] + "/" + split[0] + "/" + split[2];

            split = seconddate.Split('/');
            dt1 = split[1] + "/" + split[0] + "/" + split[2];

            string Datewise = "";
            if (chkredate.Checked == true)
            {
                if (ddltype.Text == "Books")
                    Datewise = "and date_accession between '" + dt + "' and '" + dt1 + "'";
                else if (ddltype.Text == "News Paper")
                    Datewise = "and cur_date between '" + dt + "' and '" + dt1 + "'";
                else
                    Datewise = "and access_date between '" + dt + "' and '" + dt1 + "'";
            }

            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (ddlsearch.Items.Count > 0)
                search = Convert.ToString(ddlsearch.SelectedItem.Text);
            if (ddlsearcbook.Items.Count > 0)
                searchbook = Convert.ToString(ddlsearcbook.SelectedItem.Text);

            string que1 = Convert.ToString(que);
            if (libcode != "")
            {
                if (ddltype.Text == "Books")
                {
                    if (ddlsearch.Text == "All")
                    {
                        sqlgetbook = "select  acc_no,title,author,subject,call_no,book_status,price,dept_code,lib_code,attachment,publisher,bill_no,typeofbook,pur_don,ref,book_status,Edition,book_size,Supplier,Remark,BookID,language from bookdetails where book_status<>'request' and lib_code='" + libcode + "' " + Datewise + " order by len(acc_no),acc_no ";
                        //sqlgetbook1 = "select sum(a.tit) from (select count(distinct title) tit from bookdetails where lib_code ='" + libcode + "' group by lib_code,dept_code) a";convert(numeric,acc_no) as 
                    }
                    else
                    {
                        if (ddlsearch.Text == "Access Number")
                        {
                            searchtext = Convert.ToString(ddlsearch.Text);
                            if (chkbetween.Checked == false)
                            {
                                if (txtsearch.Text != "")
                                {
                                    sqlgetbook = "select acc_no,title,author,subject,call_no,book_status,price,dept_code,lib_code,attachment,publisher,bill_no,typeofbook,pur_don,ref,book_status,Edition,book_size,Supplier,Remark,BookID,language from bookdetails where book_status<>'request' and lib_code='" + libcode + "' and " + que + "='" + txtsearch.Text + "' " + Datewise + " order by len(acc_no),acc_no ";
                                }
                            }
                            else
                            {
                                if (chkbetween.Checked == true)
                                {
                                    if (Txtbet1.Text != "" && Txtbet2.Text != "")
                                    {
                                        sqlgetbook = "select    acc_no,title,author,subject,call_no,book_status,price,dept_code,lib_code,attachment,publisher,bill_no,typeofbook,pur_don,ref,book_status,Edition,book_size,Supplier,Remark,BookID,language from bookdetails where book_status<>'request' and lib_code='" + libcode + "' and  cast(acc_no as int)between '" + Txtbet1.Text + "' and '" + Txtbet2.Text + "' " + Datewise + " order by len(acc_no),acc_no ";
                                    }
                                    else
                                    {
                                        searchaccno = true;
                                    }
                                }
                                else
                                {
                                    searchaccno = true;
                                }
                            }
                        }
                        else if (ddlsearch.Text == "Access Number With Acronym")
                        {
                            searchtext = Convert.ToString(ddlsearch.Text);
                            if (chkbetween.Checked == false)
                            {
                                if (txtsearch.Text != "")
                                {
                                    sqlgetbook = "select acc_no,title,author,subject,call_no,book_status,price,dept_code,lib_code,attachment,publisher,bill_no,typeofbook,pur_don,ref,book_status,Edition,book_size,Supplier,Remark,BookID,language from bookdetails where book_status<>'request' and lib_code='" + libcode + "' and " + que + "='" + txtsearch.Text + "' " + Datewise + " order by len(acc_no),acc_no ";
                                }
                            }
                            else
                            {
                                if (chkbetween.Checked == true)
                                {
                                    if (txtsearch.Text != "" && Txtbet1.Text != "" && Txtbet2.Text != "")
                                    {
                                        sqlgetbook = "select   acc_no,title,author,subject,call_no,book_status,price,dept_code,lib_code,attachment,publisher,bill_no,typeofbook,pur_don,ref,book_status,Edition,book_size,Supplier,Remark,BookID,language from bookdetails where book_status<>'request' and lib_code='" + libcode + "' and left(bookdetails.acc_no,  Len('" + txtsearch.Text + "') ) ='" + txtsearch.Text + "' and cast(substring(bookdetails.acc_no,4,len(bookdetails.acc_no)-3) as int) between '" + Txtbet1.Text + "' AND '" + Txtbet2.Text + "' " + Datewise + "  order by len(acc_no),acc_no ";
                                    }
                                    else
                                    {
                                        searchaccno = true;
                                    }
                                }
                                else
                                {
                                    searchaccno = true;
                                }
                            }
                        }
                        else if (ddlsearch.Text == "Call No" || ddlsearch.Text == "Title" || ddlsearch.Text == "Author" || ddlsearch.Text == "Subject Head" || ddlsearch.Text == "Billno" || ddlsearch.Text == "Topics")
                        {
                            searchtext = Convert.ToString(ddlsearch.Text);
                            if (txtsearch.Text != "")
                            {
                                if (ddlsearch.Text == "Title")
                                {
                                    sqlgetbook = "select    acc_no,title,author,subject,call_no,book_status,price,dept_code,lib_code,attachment,publisher,bill_no,typeofbook,pur_don,ref,book_status,Edition,book_size,Supplier,Remark,BookID,language from bookdetails where book_status<>'request' and lib_code='" + libcode + "' and " + que + "='" + txtsearch.Text + "' and  Author='" + searchbook + "' " + Datewise + " order by len(acc_no),acc_no";
                                }
                                else
                                {
                                    sqlgetbook = "select    acc_no,title,author,subject,call_no,book_status,price,dept_code,lib_code,attachment,publisher,bill_no,typeofbook,pur_don,ref,book_status,Edition,book_size,Supplier,Remark,BookID,language from bookdetails where book_status<>'request' and lib_code='" + libcode + "' and " + que + "='" + txtsearch.Text + "' " + Datewise + " order by len(acc_no),acc_no ";
                                }
                            }
                            else
                            {
                                //alertpopwindow.Visible = true;
                                //lblalerterr.Text = "Enter the Search Key For '" + searchtext + "'";
                                searchaccno = true;
                            }
                        }
                        else if (ddlsearch.Text == "Status")
                        {
                            sqlgetbook = "select  acc_no,title,author,subject,call_no,book_status,price,dept_code,lib_code,attachment,publisher,bill_no,typeofbook,pur_don,ref,book_status,Edition,book_size,Supplier,Remark,BookID,language from bookdetails where book_status<>'request' and lib_code='" + libcode + "' and LTRIM(RTRIM(" + que + ")) like '" + searchbook + "' and " + que + " <>'' " + Datewise + " order by len(acc_no),acc_no";
                        }
                        else if (ddlsearch.Text == "Purchased" || ddlsearch.Text == "Donated")
                        {
                            sqlgetbook = "select acc_no,title,author,subject,call_no,book_status,price,dept_code,lib_code,attachment,publisher,bill_no,typeofbook,pur_don,ref,book_status,Edition,book_size,Supplier,Remark,BookID,language from bookdetails where book_status<>'request' and lib_code='" + libcode + "'  and " + que + " like '" + ddlsearch.Text + "' " + Datewise + " order by len(acc_no),acc_no ";
                            //sqlgetbook1 = "select distinct title from bookdetails where lib_code='" + libcode + "'  and " + que + " like '" + searchbook + "' group by title order by title";
                        }
                        else if (ddlsearch.Text == "Type")
                        {
                            sqlgetbook = "select   acc_no,title,author,subject,call_no,book_status,price,dept_code,lib_code,attachment,publisher,bill_no,typeofbook,pur_don,ref,book_status,Edition,book_size,Supplier,Remark,BookID,language from bookdetails where book_status<>'request' and lib_code='" + libcode + "'  and " + que + " = '" + searchbook + "' " + Datewise + " order by len(acc_no),acc_no ";
                        }
                        else if (ddlsearch.Text == "Department")
                        {
                            if (searchbook != "All")
                                sqlgetbook = "select   acc_no,title,author,subject,call_no,book_status,price,dept_code,lib_code,attachment,publisher,bill_no,typeofbook,pur_don,ref,book_status,Edition,book_size,Supplier,Remark,BookID,language from bookdetails where book_status<>'request' and lib_code='" + libcode + "'  and " + que + " = '" + searchbook + "' " + Datewise + " order by len(acc_no),acc_no ";
                            else
                                sqlgetbook = "select   acc_no,title,author,subject,call_no,book_status,price,dept_code,lib_code,attachment,publisher,bill_no,typeofbook,pur_don,ref,book_status,Edition,book_size,Supplier,Remark,BookID,language from bookdetails where lib_code='" + libcode + "' " + Datewise + " order by len(acc_no),acc_no ";
                            //sqlgetbook1 = "select distinct title from bookdetails where lib_code='" + libcode + "' group by title order by title";
                        }
                        else if (ddlsearch.Text == "Category")
                        {
                            sqlgetbook = "select  acc_no,title,author,subject,call_no,book_status,price,dept_code,lib_code,attachment,publisher,bill_no,typeofbook,pur_don,ref,book_status,Edition,book_size,Supplier,Remark,BookID,language from bookdetails where book_status<>'request' and lib_code='" + libcode + "'  and " + que + " = '" + searchbook + "' " + Datewise + " order by len(acc_no),acc_no ";
                        }
                        else if (ddlsearch.Text == "Language")
                        {
                            sqlgetbook = "select  acc_no,title,author,subject,call_no,book_status,price,dept_code,lib_code,attachment,publisher,bill_no,typeofbook,pur_don,ref,book_status,Edition,book_size,Supplier,Remark,BookID,language from bookdetails where book_status<>'request' and lib_code='" + libcode + "'  and " + que + " = '" + searchbook + "' " + Datewise + " order by len(acc_no),acc_no ";
                        }
                    }
                    if (sqlgetbook != "")
                    {
                        dsload.Clear();
                        dsload = d2.select_method_wo_parameter(sqlgetbook, "Text");
                    }
                }
                else if (ddltype.Text == "News Paper")
                {
                    if (ddlsearch.Text == "All")
                        sqlgetbook = "select * from news_paper where lib_code='" + libcode + "' " + Datewise + "";
                    else
                        sqlgetbook = "select * from news_paper where lib_code='" + libcode + "' and title like '" + txtsearch.Text + "%' " + Datewise + "";
                    if (sqlgetbook != "")
                    {
                        dsload.Clear();
                        dsload = d2.select_method_wo_parameter(sqlgetbook, "Text");
                    }
                }
                else
                {
                    if (ddlsearch.Text == "All")
                        sqlgetbook = "select 0,access_code,title,paper_name,semester,sem_month,sem_year,ISNULL(Price,'') Price,issue_flag from university_question where lib_code='" + libcode + "' " + Datewise + " order by len(access_code),access_code  ";
                    else
                    {
                        searchtext = Convert.ToString(ddlsearch.Text);
                        if (txtsearch.Text != "")
                        {
                            sqlgetbook = "select 0,access_code,title,paper_name,semester,sem_month,sem_year,ISNULL(Price,'') Price,issue_flag from university_question where lib_code = '" + libcode + "' and " + Field_Name1 + " like '%" + txtsearch.Text + "%' " + Datewise + " order by len(access_code),access_code";
                        }
                        else
                        {
                            //alertpopwindow.Visible = true;
                            //lblalerterr.Text = "Enter '" + searchtext + "'";
                            searchaccno1 = true;
                        }
                    }
                    if (sqlgetbook != "")
                    {
                        dsload.Clear();
                        dsload = d2.select_method_wo_parameter(sqlgetbook, "Text");
                    }
                }
            }
            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
        return dsload;
    }

    public void loadspreadBook(DataSet ds)
    {
        try
        {
            DataView dv = new DataView();
            DataSet dskit = new DataSet();
            DataTable dtInward = new DataTable();
            DataRow drow;
            dtInward.Columns.Add("SNo", typeof(string));
            dtInward.Columns.Add("Access No", typeof(string));
            dtInward.Columns.Add("Department", typeof(string));
            dtInward.Columns.Add("Title", typeof(string));
            dtInward.Columns.Add("BookId", typeof(string));
            dtInward.Columns.Add("Author", typeof(string));
            dtInward.Columns.Add("Pubisher", typeof(string));
            dtInward.Columns.Add("Price", typeof(string));
            dtInward.Columns.Add("Edition", typeof(string));
            dtInward.Columns.Add("BillNo", typeof(string));
            dtInward.Columns.Add("Attachement", typeof(string));
            dtInward.Columns.Add("Call No", typeof(string));
            dtInward.Columns.Add("BookStatus", typeof(string));
            dtInward.Columns.Add("Subject", typeof(string));
            dtInward.Columns.Add("Purchase", typeof(string));
            dtInward.Columns.Add("BookType", typeof(string));
            dtInward.Columns.Add("Pages", typeof(string));
            dtInward.Columns.Add("Supplier", typeof(string));
            dtInward.Columns.Add("Remarks", typeof(string));
            int sno = 0;
            Hashtable htkit1 = new Hashtable();
            DataSet dsfilter = new DataSet();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    string lang = Convert.ToString(ds.Tables[0].Rows[row]["language"]).Trim();
                    drow = dtInward.NewRow();
                    drow["SNo"] = Convert.ToString(sno);
                    drow["Access No"] = Convert.ToString(ds.Tables[0].Rows[row]["acc_no"]).Trim();
                    drow["Department"] = Convert.ToString(ds.Tables[0].Rows[row]["dept_code"]).Trim();
                    drow["Title"] = Convert.ToString(ds.Tables[0].Rows[row]["title"]).Trim();
                    drow["BookId"] = Convert.ToString(ds.Tables[0].Rows[row]["BookID"]).Trim();
                    drow["Author"] = Convert.ToString(ds.Tables[0].Rows[row]["author"]).Trim();
                    drow["Pubisher"] = Convert.ToString(ds.Tables[0].Rows[row]["publisher"]).Trim();
                    drow["Price"] = Convert.ToString(ds.Tables[0].Rows[row]["price"]).Trim();
                    drow["Edition"] = Convert.ToString(ds.Tables[0].Rows[row]["Edition"]).Trim();
                    drow["BillNo"] = Convert.ToString(ds.Tables[0].Rows[row]["bill_no"]).Trim();
                    drow["Attachement"] = Convert.ToString(ds.Tables[0].Rows[row]["attachment"]).Trim();
                    drow["Call No"] = Convert.ToString(ds.Tables[0].Rows[row]["call_no"]).Trim();
                    drow["BookStatus"] = Convert.ToString(ds.Tables[0].Rows[row]["book_status"]).Trim();
                    drow["Subject"] = Convert.ToString(ds.Tables[0].Rows[row]["subject"]).Trim();
                    drow["Purchase"] = Convert.ToString(ds.Tables[0].Rows[row]["pur_don"]).Trim();
                    drow["BookType"] = Convert.ToString(ds.Tables[0].Rows[row]["typeofbook"]).Trim();
                    drow["Pages"] = Convert.ToString(ds.Tables[0].Rows[row]["book_size"]).Trim();
                    drow["Supplier"] = Convert.ToString(ds.Tables[0].Rows[row]["Supplier"]).Trim();
                    drow["Remarks"] = Convert.ToString(ds.Tables[0].Rows[row]["Remark"]).Trim();
                    dtInward.Rows.Add(drow);
                }
                chkGridSelectAll.Visible = true;
                grdInward.DataSource = dtInward;
                grdInward.DataBind();
                grdInward.Visible = true;
                btndel.Visible = true;
                rptprint1.Visible = true;
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "InwardEntry");
        }
    }

    public void loadspreadNews(DataSet ds)
    {
        try
        {
            DataView dv = new DataView();
            DataSet dskit = new DataSet();
            DataTable dtInwardNews = new DataTable();
            DataRow drow;
            dtInwardNews.Columns.Add("SNo", typeof(string));
            dtInwardNews.Columns.Add("Date", typeof(string));
            dtInwardNews.Columns.Add("Paper Name", typeof(string));
            dtInwardNews.Columns.Add("serialNo", typeof(string));
            dtInwardNews.Columns.Add("Copies", typeof(string));
            dtInwardNews.Columns.Add("Price Per Copy", typeof(string));
            dtInwardNews.Columns.Add("Total in Rs.", typeof(string));
            int totalrs = 0;
            int sno = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    string rDate = Convert.ToString(ds.Tables[0].Rows[row]["cur_date"]).Trim();
                    string rpaname = Convert.ToString(ds.Tables[0].Rows[row]["attachment"]).Trim();
                    string rsno = Convert.ToString(ds.Tables[0].Rows[row]["serial_no"]).Trim();
                    string rcop = Convert.ToString(ds.Tables[0].Rows[row]["noofcopies"]).Trim();
                    string rppcopy = Convert.ToString(ds.Tables[0].Rows[row]["price"]).Trim();

                    if (rcop != "" && rppcopy != "")
                    {
                        int cop = Convert.ToInt32(rcop);
                        int pcopy = Convert.ToInt32(rppcopy);
                        totalrs = cop * pcopy;
                    }
                    drow = dtInwardNews.NewRow();
                    drow["SNo"] = Convert.ToString(sno);
                    drow["Date"] = rDate;
                    drow["Paper Name"] = rpaname;
                    drow["serialNo"] = rsno;
                    drow["Copies"] = rcop;
                    drow["Price Per Copy"] = rppcopy;
                    drow["Total in Rs."] = Convert.ToString(totalrs);
                    dtInwardNews.Rows.Add(drow);
                }
                chkGridSelectAll.Visible = true;
                grdInward.DataSource = dtInwardNews;
                grdInward.DataBind();

                grdInward.Visible = true;
                btndel.Visible = true;
                rptprint1.Visible = true;


            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    public void loadspreadQues(DataSet ds)
    {
        try
        {
            DataView dv = new DataView();
            DataSet dskit = new DataSet();
            DataTable dtInwardQues = new DataTable();
            DataRow drow;
            int sno = 0;
            dtInwardQues.Columns.Add("SNo", typeof(string));
            dtInwardQues.Columns.Add("Date", typeof(string));
            dtInwardQues.Columns.Add("Paper Name", typeof(string));
            dtInwardQues.Columns.Add("Copies", typeof(string));
            dtInwardQues.Columns.Add("Price Per Copy", typeof(string));
            dtInwardQues.Columns.Add("Total in Rs.", typeof(string));


            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    string curdate = Convert.ToString(ds.Tables[0].Rows[row]["cur_date"]).Trim();
                    string rpapname = Convert.ToString(ds.Tables[0].Rows[row]["title"]).Trim();
                    string noofcopu = Convert.ToString(ds.Tables[0].Rows[row]["noofcopies"]).Trim();
                    string price = Convert.ToString(ds.Tables[0].Rows[row]["price"]).Trim();
                    string tota = Convert.ToString(ds.Tables[0].Rows[row]["Total"]).Trim();

                    drow = dtInwardQues.NewRow();
                    drow["SNo"] = Convert.ToString(sno);
                    drow["Date"] = curdate;
                    drow["Paper Name"] = rpapname;
                    drow["Copies"] = noofcopu;
                    drow["Price Per Copy"] = price;
                    drow["Total in Rs."] = Convert.ToString(tota);
                    dtInwardQues.Rows.Add(drow);
                }
                chkGridSelectAll.Visible = true;
                grdInward.DataSource = dtInwardQues;
                grdInward.DataBind();

                grdInward.Visible = true;
                btndel.Visible = true;
                rptprint1.Visible = true;

            }



        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void grdInward_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdInward.PageIndex = e.NewPageIndex;
        btngo_Click(sender, e);
    }

    #endregion

    #region Print

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Inward_Entry";
            string pagename = "Inward_Entry.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(grdInward, pagename, degreedetails, 0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname1.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdInward, reportname);
                lblvalidation2.Visible = false;
            }
            else
            {
                lblvalidation2.Text = "Please Enter Your Report Name";
                lblvalidation2.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    #endregion

    #region Add_PopupWindow

    protected void ddlentrytype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_entrytype.SelectedIndex == 0)
            {
                link_addtion.Visible = true;
                newcopy.Visible = false;
                txt_newcopy.Visible = false;
                link_addtion.Visible = true;
                ddl_Category.Enabled = true;
                ddl_language.Enabled = true;
                ddl_booktype.Enabled = true;
                rblSingle.Enabled = true;
                rblMultiple.Enabled = true;
                txt_author.Enabled = true;
                Txt_pub.Enabled = true;
                Txt_sub.Enabled = true;
                ddl_curren.Enabled = true;
                txt_curval.Enabled = true;
                txt_isbn_No.Enabled = true;
                txt_billno.Enabled = true;
                txt_remarks.Enabled = true;
                Txt_SedAuthor.Enabled = true;
                ddl_atta.Enabled = true;
                ddl_Description.Enabled = true;
                btn_popupDes.Enabled = true;
                ddl_CallNo.Enabled = true;
                ddlsupp.Enabled = true;
                txt_publisyear.Enabled = true;
                txt_remarks.Enabled = true;
            }
            else if (ddl_entrytype.SelectedIndex == 1)
            {
                //link_addtion.Visible = false;
                //ddl_Category.Enabled = false;
                //ddl_language.Enabled = false;
                //ddl_booktype.Enabled = false;
                //newcopy.Visible = true;
                //txt_newcopy.Visible = true;
                //rblSingle.Enabled = false;
                //rblMultiple.Enabled = false;
                //txt_author.Enabled = false;
                //Txt_pub.Enabled = false;
                //Txt_sub.Enabled = false;
                //ddl_curren.Enabled = false;
                //txt_curval.Enabled = false;
                //txt_isbn_No.Enabled = false;
                //txt_billno.Enabled = false;
                //txt_remarks.Enabled = false;
                //txt_supplier.Enabled = true;
                divnews_pop.Visible = true;
                loagRack2();
                loadshelff2();
                loadposition2();
                LoadPosPlace2();
                Library1();
            }
            else
            {
                //link_addtion.Visible = true;
                //ddl_Category.Enabled = false;
                //ddl_atta.Enabled = false;
                //ddl_Description.Enabled = false;
                //btn_popupDes.Enabled = false;
                //ddl_curren.Enabled = false;
                //ddl_booktype.Enabled = false;
                //ddl_CallNo.Enabled = false;
                //newcopy.Visible = false;
                //txt_newcopy.Visible = false;
                //txt_author.Enabled = false;
                //Txt_pub.Enabled = false;
                //Txt_sub.Enabled = false;
                //Txt_SedAuthor.Enabled = false;
                //ddl_language.Enabled = false;
                //txt_publisyear.Enabled = false;
                //txt_isbn_No.Enabled = false;
                //txt_billno.Enabled = false;
                //txt_supplier.Enabled = false;
                //txt_remarks.Enabled = true;
                Div_Question_Bank_popup.Visible = true;
                loagRack();
                loadshelff();
                loadposition1();
                LoadPosPlace1();
            }
            dEntryType = Convert.ToString(ddl_entrytype.SelectedItem);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void ddl_title_lan_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_title.Text = "";
            if (ddl_title_lan.Text == "Tamil")
            {
                txt_title.Font.Name = "Amudham";
                dtitlelang = Convert.ToString(ddl_title_lan.SelectedItem);
            }
            else
            {
                txt_title.Font.Name = "Arial";
                dtitlelang = Convert.ToString(ddl_title_lan.SelectedItem);
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void chk_nonbook_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (IsNonBook.Checked == true)
            {
                lblnonbook.Visible = true;
                txtnonbook.Visible = true;
            }
            else
            {
                lblnonbook.Visible = false;
                txtnonbook.Visible = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void rbl_yes_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rblMultiple.Checked == true)
            {
                lbl_yes.Visible = true;
                text_ref.Visible = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void rbl_no_OnCheckedChanged(object sender, EventArgs e)
    {
        lbl_yes.Visible = false;
        text_ref.Visible = false;
    }

    protected void ddl_Description_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string calldes = "";
            if (ddl_Description.Items.Count > 0)
            {
                calldes = Convert.ToString(ddl_Description.SelectedItem.Text);
            }
            LoadCallNo(calldes);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void ddl_publishplace_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddl_Budget_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddl_booktype_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddl_CallNo_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void chk_date_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chk_date.Checked == true)
                Txtbilldate.Enabled = true;
            else
                Txtbilldate.Enabled = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void ddl_Author_lan_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_author.Text = "";
        if (ddl_Author_lan.Text == "Tamil")
        {
            txt_author.Font.Name = "Amudham";
            dtitlelang1 = Convert.ToString(ddl_Author_lan.SelectedItem);
        }
        else
        {
            txt_author.Font.Name = "Arial";
            dtitlelang1 = Convert.ToString(ddl_Author_lan.SelectedItem);
        }

    }

    protected void ddl_Rack_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadAddshelf();
    }

    protected void ddl_shelf_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadAddposition();
    }

    protected void ddl_posi_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAddPosPlace();
    }

    protected void ddl_posplace_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddl_curren_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddl_status_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddl_atta_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddl_language_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void txt_accno_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string type = "";
            string libname = "";
            string getupdatebookqry = "";
            DataSet dsgetupdatebook = new DataSet();
            if (ddllibrary.Items.Count > 0)
            {
                libname = Convert.ToString(ddllibrary.SelectedItem.Text);
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            }
            if (ddltype.Items.Count > 0)
                type = Convert.ToString(ddltype.SelectedValue);
            string NewInward_Accnumber = "";
            string OldInward_Accnumber = "";
            ddl_CallNo.Items.Clear();
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            //Book Save 
            if (ddl_entrytype.Items[0].Text == "Books")
            {
                NewInward_Accnumber = Convert.ToString(txt_accno.Text);
                if (NewInward_Accnumber != "")
                {
                    OldInward_Accnumber = d2.GetFunction("select acc_No from bookdetails where acc_No='" + NewInward_Accnumber + "' and lib_code='" + libcode + "'");
                    if (OldInward_Accnumber.ToLower() == NewInward_Accnumber)
                    {
                        //alertpopwindow.Visible = true;
                        //lblalerterr.Text = "Access No already exists";
                        //txt_accno.Text = "";
                        //return;
                        popview.Visible = true;
                        btn_Save.Visible = true;
                        btn_Save.ImageUrl = "~/LibImages/update.jpg";
                        btn_Delete.Visible = true;
                        Page.MaintainScrollPositionOnPostBack = true;
                        if (type == "Books")
                        {
                            string getRackNo = d2.GetFunction("select rack_no from rack_allocation where acc_no='" + NewInward_Accnumber + "'");
                            if (getRackNo != "" && getRackNo != "0")
                            {
                                ddl_Rack.SelectedIndex = ddl_Rack.Items.IndexOf(ddl_Rack.Items.FindByValue(Convert.ToString(getRackNo)));
                            }
                            loadshelf();
                            string getShelfNo = d2.GetFunction("select row_no from rack_allocation where acc_no='" + NewInward_Accnumber + "'");
                            if (getShelfNo != "" || getShelfNo != "0")
                            {
                                ddl_shelf.SelectedIndex = ddl_shelf.Items.IndexOf(ddl_shelf.Items.FindByValue(Convert.ToString(getShelfNo)));
                            }
                            getupdatebookqry = "select * from bookdetails where acc_no='" + NewInward_Accnumber + "' and lib_code='" + libcode + "'";
                            dsgetupdatebook.Clear();
                            dsgetupdatebook = d2.select_method_wo_parameter(getupdatebookqry, "Text");
                            if (dsgetupdatebook.Tables[0].Rows.Count > 0)
                            {
                                ddl_txt_lib.Items[0].Text = libname;
                                txt_accno.Text = NewInward_Accnumber;
                                txt_Offer.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["b_discount"]);
                                txt_title.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["title"]);
                                txt_author.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["author"]);
                                Txt_pub.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["publisher"]);
                                Txt_sub.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["supplier"]);
                                Txt_edit.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["edition"]);
                                txvo.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["volume"]);
                                txtpart.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["part"]);
                                txt_publisyear.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["pur_year"]);
                                txt_Price.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["price"]);
                                txt_billno.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["bill_no"]);
                                Txtbilldate.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["bill_date"]);
                                ddl_CallNo.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["call_no"]));
                                ddl_Description.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["call_des"]));
                                string yesorno = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["ref"]);
                                if (yesorno == "Yes")
                                    rbl_yes.Checked = true;
                                else
                                    rbl_no.Checked = true;
                                txt_depart.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["dept_code"]);
                                txt_remarks.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["remark"]);
                                txt_date_acc.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["access_date"]);
                                Txt_SedAuthor.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["sec_author"]);
                                ddl_thridAuthor.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["thi_author"]));
                                txcoll.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["collabrator"]);
                                txt_bookSz.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["book_size"]);
                                txtbose.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["book_series"]);
                                txt_isbn_No.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["isbn"]);

                                txtboselect.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["book_selected_by"]);
                                txtboacc.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["book_accesed_by"]);
                                ddl_inward_type.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["pur_don"]));
                                ddl_status.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["book_status"]));
                                ddl_atta.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["attachment"]));
                                txttopics.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["topics"]);
                                txvolti.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["volumetitle"]);
                                txsubtitle.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["subtitle"]);
                                txt_curval.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["cur_value"]);
                                ddl_curren.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["cur_name"]));
                                txvolpr.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["vol_price"]);
                                Txt_sub.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["subject"]);
                                ddl_booktype.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["typeofbook"]));
                                txkey1.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["key1"]);
                                txkey2.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["key2"]);
                                txkey3.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["key3"]);
                                ddl_Category.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["category"]));
                                ddl_language.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["language"]));
                            }
                        }
                    }
                }

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    #region PlusminusCat
    protected void btn_pls_cat_Click(object sender, EventArgs e)
    {
        try
        {

            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Category";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    protected void btn_min_cat_Click(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    #endregion

    #region PlusminusBudget
    protected void btn_pls_bud_Click(object sender, EventArgs e)
    {
        try
        {

            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Book Type";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    protected void btn_min_bud_Click(object sender, EventArgs e)
    {
        try
        {
            ddl_Budget.Items.Clear();

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    #endregion

    #region Plusminuspubplace
    protected void btn_pls_pubpl_Click(object sender, EventArgs e)
    {
        try
        {

            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Publication Place";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    protected void btn_min_pubpl_Click(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    #endregion

    #region PlusminusBookType
    protected void btn_plu_bo_Click(object sender, EventArgs e)
    {
        try
        {

            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Book Type";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    protected void btn_min_bo_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_status.Items.Count > 0)
                g1 = Convert.ToString(ddl_status.Items[0].Text);
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (g1 != "")
            {
                string get = d2.GetFunction("select count(typeofbook) typeofbook   from bookdetails where typeofbook  ='" + g1 + "' and  Lib_Code ='" + libcode + "'");
                int getcnt = Convert.ToInt32(get);
                if (getcnt > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Alredy Books Available in this Book Type.So it Cannot be deleted.";
                    return;

                }
                else
                {
                    LoadBookType();
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select  Book Type";
                return;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    #endregion

    #region Plusminuscallno
    protected void btn_pls_callno_Click(object sender, EventArgs e)
    {
        try
        {

            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Call No";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    protected void btn_min_callno_Click(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    #endregion

    #region plusminusCurrency
    protected void btcurrencyplus_Click(object sender, EventArgs e)
    {
        try
        {

            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Currency";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    protected void btncurrymin_Click(object sender, EventArgs e)
    {
        try
        {

            if (ddl_curren.Items.Count > 0)
                g1 = Convert.ToString(ddl_curren.Items[0].Text);
            if (g1 != "")
            {
                delete = "if exists(select * from currency_convertion where currency_type='" + g1 + "')delete from currency_convertion  where currency_type='" + g1 + "'";
                demin = d2.update_method_wo_parameter(delete, "Text");
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Currency Type";
                return;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    #endregion

    #region plusminusStatus
    protected void btn_pls_status_Click(object sender, EventArgs e)
    {
        try
        {

            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Status";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    protected void btn_min_status_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_status.Items.Count > 0)
                g1 = Convert.ToString(ddl_status.Items[0].Text);
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (g1 != "")
            {
                get = d2.GetFunction("select count(book_status) book_status from bookdetails where book_status='" + g1 + "' and  Lib_Code ='" + libcode + "'");
                getcnt = Convert.ToInt32(get);
                if (getcnt > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Alredy Books Available in this status.So it Cannot be deleted.";
                    return;

                }
                else
                {
                    LoadStatus();
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select status";
                return;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    #endregion

    #region plusminusAttachement
    protected void btn_pls_att_Click(object sender, EventArgs e)
    {
        try
        {

            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Attachement";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    protected void btn_min_att_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_atta.Items.Count > 0)
                g1 = Convert.ToString(ddl_atta.Items[0].Text);
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (g1 != "")
            {
                string get = d2.GetFunction("select count(Attachment) Attachment from bookdetails where Attachment='" + g1 + "' and  Lib_Code ='" + libcode + "'");
                int getcnt = Convert.ToInt32(get);
                if (getcnt > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Alredy Books Available in this Attachment.So it Cannot be deleted.";
                    return;

                }
                else
                {
                    LoadAttache();
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Attachment";
                return;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    #endregion

    #region plusminusLanguage
    protected void btn_pls_lang_Click(object sender, EventArgs e)
    {
        try
        {

            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Language";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    protected void btn_min_lang_Click(object sender, EventArgs e)
    {
        try
        {

            if (ddl_language.Items.Count > 0)
                g1 = Convert.ToString(ddl_language.Items[0].Text);
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (g1 != "")
            {
                string get = d2.GetFunction("select count(language ) language  from bookdetails where language ='" + g1 + "' and  Lib_Code ='" + libcode + "'");
                int getcnt = Convert.ToInt32(get);
                if (getcnt > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Alredy Books Available in this language.So it Cannot be deleted.";
                    return;

                }
                else
                {
                    LoadLang();
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select language";
                return;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    #endregion

    #region plusminusthridAuthor
    protected void btn_pls_Thrid_Click(object sender, EventArgs e)
    {
        try
        {

            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Thrid Author";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    protected void btn_min_Thrid_Click(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    #endregion

    #region No_Of_Copy_Radio

    protected void rblSingle_Selected(object sender, EventArgs e)
    {
        try
        {
            TxtMultiple.Visible = false;
            rblMultiple.Checked = false;
            if (rbl_yes.Checked == true)
            {
                lbl_yes.Visible = false;
                text_ref.Visible = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    protected void rblMultiple_Selected(object sender, EventArgs e)
    {
        try
        {
            rblSingle.Checked = false;
            TxtMultiple.Visible = true;
            if (rbl_yes.Checked == true)
            {
                lbl_yes.Visible = true;
                text_ref.Visible = true;
            }
            else
            {
                lbl_yes.Visible = false;
                text_ref.Visible = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }


    #endregion

    #region Link_NonBook
    protected void lnknonbook_Click(object sender, EventArgs e)
    {
        try
        {

            DivNonBookpopup.Visible = true;
            ddl_mat.Attributes.Add("onfocus", "frelig5()");
            ddcurrency.Attributes.Add("onfocus", "frelig6()");
            string nonLibraryname = "";
            if (ddllibrary.Items.Count > 0)
                nonLibraryname = Convert.ToString(ddllibrary.SelectedItem.Text);
            if (!string.IsNullOrEmpty(nonLibraryname))
                ddl_Library.Items.Add(nonLibraryname);
            NBMAutoAccno();
            LoadAttache();
            LoadBudHead();
            GetCurrencyType();
            loadmonth();
            loaddepartment();
            txDate_Acc.Attributes.Add("readonly", "readonly");
            txDate_Acc.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }


    #endregion

    #region Link_AdditionalDetails
    protected void link_addtion_Click(object sender, EventArgs e)
    {
        try
        {
            ddl_thridAuthor.Attributes.Add("onfocus", "frelig12()");
            if (ddl_entrytype.SelectedIndex == 0)
            {
                DivAddDetailsBookPopup.Visible = true;
                Div_Question_Bank_popup.Visible = false;
            }
            else if (ddl_entrytype.SelectedIndex == 1)
                DivAddDetailsBookPopup.Visible = false;
            else
            {
                DivAddDetailsBookPopup.Visible = false;
                Div_Question_Bank_popup.Visible = true;
            }

        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    #endregion

    #region Link_Status

    protected void link_status_Click(object sender, EventArgs e)
    {
        DivStatus.Visible = true;
        Bindcollege();
        getLibPrivil();
        LoadRack();
        RackFpSpread.Visible = false;
    }

    #endregion

    #region Save_And_update_Delete

    protected void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_Save.ImageUrl == "~/LibImages/save.jpg")
            {
                int rf = 0;
                int saveacc = 0;
                string MultiCopyACcNo = "";
                string status = "";
                if (ddlCollege.Items.Count > 0)
                    collcode = Convert.ToString(ddllibrary.SelectedValue);
                if (ddl_txt_lib.Items.Count > 0)
                    libcode = Convert.ToString(ddl_txt_lib.SelectedValue);
                if (ddl_Rack.Items.Count > 0)
                    rackno = Convert.ToString(ddl_Rack.SelectedItem.Text);
                if (ddl_shelf.Items.Count > 0)
                    shelfno = Convert.ToString(ddl_shelf.SelectedItem.Text);
                if (ddl_language.Items.Count > 0)
                    language = Convert.ToString(ddl_language.SelectedItem.Text);
                if (ddl_atta.Items.Count > 0)
                    Attache = Convert.ToString(ddl_atta.SelectedItem.Text).ToUpper();
                if (ddl_curren.Items.Count > 0)
                    currntype = Convert.ToString(ddl_curren.SelectedItem.Text).ToUpper();
                if (ddl_Category.Items.Count > 0)
                    cate = Convert.ToString(ddl_Category.SelectedItem.Text);
                if (ddl_CallNo.Items.Count > 0)
                    callno = Convert.ToString(ddl_CallNo.SelectedItem.Text);
                if (ddl_title_lan.Items.Count > 0)
                    titlelang = Convert.ToString(ddl_title_lan.SelectedIndex);
                if (ddl_Author_lan.Items.Count > 0)
                    authorlang = Convert.ToString(ddl_Author_lan.SelectedIndex);
                if (ddl_publishplace.Items.Count > 0)
                    pubplace = Convert.ToString(ddl_publishplace.SelectedItem.Text);
                if (ddl_booktype.Items.Count > 0)
                    booktype = Convert.ToString(ddl_booktype.SelectedItem.Text);
                if (ddl_Description.Items.Count > 0)
                    Calldes = Convert.ToString(ddl_Description.SelectedItem.Text);
                if (ddl_inward_type.Items.Count > 0)
                    inwardtype = Convert.ToString(ddl_inward_type.SelectedItem.Text);
                if (ddl_posi.Items.Count > 0)
                    pos = Convert.ToString(ddl_posi.SelectedItem.Text);
                if (ddl_posplace.Items.Count > 0)
                    posplc = Convert.ToString(ddl_posplace.SelectedItem.Text);
                if (ddl_status.Items.Count > 0)
                    status = Convert.ToString(ddl_status.SelectedItem.Text);
                string Curtdate = DateTime.Now.ToString("MM/dd/yyyy");
                if (txt_Price.Text != "")
                    price = txt_Price.Text;
                else
                    price = "0";
                int supplier = Convert.ToInt32(ddlsupp.Items.Count);
                if (supplier == 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Supplier Details Not Available";
                    return;
                }
                if (rbl_yes.Checked == true)
                {
                    reference = "Yes";
                    Book_Type = "REF";
                }
                else
                {
                    reference = "No";
                    Book_Type = "BOK";
                }

                string Accdate = Convert.ToString(txt_date_acc.Text);
                string[] adate = Accdate.Split('/');
                if (adate.Length == 3)
                {
                    accessdate = adate[1].ToString() + "/" + adate[0].ToString() + "/" + adate[2].ToString();
                    string AccesDt = adate[1].ToString();
                    AccesDt = AccesDt.StartsWith("0") ? AccesDt.Substring(1) : AccesDt;
                    accessdate = AccesDt + "/" + adate[0] + "/" + adate[2];
                }

                string Acctime = DateTime.Now.ToString("hh:mm tt");
                if (ddl_entrytype.Text == "")
                {

                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Select The Entry Type And Then Proceed";
                    return;
                }

                if (ddl_entrytype.Items[0].Text == "Books")
                {
                    if (rblMultiple.Checked == true)
                    {
                        if (TxtMultiple.Text == "")
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Enter No Of Copies";
                            return;
                        }
                        if (TxtMultiple.Text == "0" || Convert.ToInt32(TxtMultiple.Text) <= 1)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Enter No Of Copies greater than 1";
                            return;
                        }
                    }
                    if (rackno == "" && shelfno == "")
                    {
                        string maxcap = d2.GetFunction("select max_capacity from rackrow_master where lib_code='" + libcode + "' and rack_no = '" + rackno + "' and row_no='" + shelfno + "'");
                        int maxcapa = Convert.ToInt32(maxcap);
                        string no_copy = d2.GetFunction("select max_capacity from rackrow_master where lib_code='" + libcode + "' and rack_no = '" + rackno + "' and row_no='" + shelfno + "'");
                        int noc = Convert.ToInt32(no_copy);
                        int capacity = maxcapa - noc;
                    }
                    if (txt_depart.Text != "")
                    {
                        string dept = "if exists(select * from journal_dept where dept_name='" + txt_depart.Text + "' and college_code =" + collcode + " AND Lib_Code ='" + libcode + "')update journal_dept set dept_acr=' ' where dept_name='" + txt_depart.Text + "' AND Lib_code ='" + libcode + "' AND College_Code ='" + collcode + "' else INSERT into journal_dept (dept_name,dept_Acr,lib_code,college_code) values ('" + txt_depart.Text + "',' ','" + libcode + "','" + collcode + "')";
                        save = d2.update_method_wo_parameter(dept, "Text");
                    }
                    if (Attache != "")
                    {
                        string att = "if exists(select * from attachment where attachment_name='" + Attache + "')update attachment set  attachment_name='" + Attache + "' where attachment_name='" + Attache + "' else INSERT into attachment (attachment_name) values ('" + Attache + "')";
                        save = d2.update_method_wo_parameter(att, "Text");
                    }
                    if (currntype != "")
                    {
                        string cutype = "if exists(select * from currency_convertion where currency_type='" + currntype + "')update currency_convertion set currency_type='" + currntype + "' where currency_type='" + currntype + "' else INSERT into currency_convertion (currency_type) values ('" + currntype + "')";
                        save = d2.update_method_wo_parameter(cutype, "Text");
                    }
                    if (rblSingle.Checked == true)
                    {
                        if (rackno == "")
                            rf = 0;
                        else
                            rf = 1;
                        if (AlreadyLoad == "N")
                        {
                            AddtionalDetailPopup.Visible = true;
                            lbr_msg.Text = "Do You Want Enter More Details For Book?";
                            return;
                        }
                        sqlqry = "insert into bookdetails(acc_no,title,author,publisher,supplier,edition,volume,part,pur_year,price,bill_no,bill_date,call_no,call_des,ref,dept_code,remark,access_date,access_time,Lib_code,sec_author,thi_author,collabrator,book_size,book_series,isbn,book_selected_by,book_accesed_by,date_accession,rack_flag,pur_don,book_status,attachment,topics,b_discount,volumetitle,subtitle,cur_value,cur_name,vol_price,subject,typeofbook,pub_place,key1,key2,key3,category,language)values ('" + txt_accno.Text + "','" + txt_title.Text + "','" + txt_author.Text + "','" + Txt_pub.Text + "','" + supplier + "','" + Txt_edit.Text + "','" + txvo.Text + "','" + txtpart.Text + "','" + txt_publisyear.Text + "','" + price + "','" + txt_billno.Text + "','" + billdate + "','" + callno + "','" + Calldes + "','" + reference + "','" + txt_depart.Text + "','" + txt_remarks.Text + "','" + accessdate + "','" + Acctime + "','" + libcode + "','" + Txt_SedAuthor.Text + "','" + ddl_thridAuthor.Text + "','" + txcoll.Text + "','" + txt_bookSz.Text + "','" + txtbose.Text + "','" + txt_isbn_No.Text + "','" + txtboselect.Text + "','" + txtboacc.Text + "','" + accessdate + "'," + rf + ",'" + inwardtype + "','" + status + "','" + Attache + "','" + txttopics.Text + "','" + txt_Offer.Text + "','" + txvolti.Text + "','" + txsubtitle.Text + "','" + txt_curval.Text + "','" + currntype + "','" + txvolpr.Text + "','" + Txt_sub.Text + "','" + Book_Type + "','','" + txkey1.Text + "','" + txkey2.Text + "','" + txkey3.Text + "','" + cate + "','" + language + "')";
                        save = d2.update_method_wo_parameter(sqlqry, "Text");
                        if (rf == 1)
                            ChkAndAddPublisherEntry();
                        string ins = "insert into rack_allocation(lib_code,rack_no,row_no,acc_no,access_date,access_time,book_type) values('" + libcode + "','" + rackno + "','" + shelfno + "','" + txt_accno.Text + "','" + Curtdate + "','" + Acctime + "','BOK')";
                        ins += "update rackrow_master set no_of_copies  = Convert(int ,no_of_copies) + 1 where rack_no = '" + rackno + "' and  row_no = '" + shelfno + "' and lib_code = '" + libcode + "'";

                        ins += "update rack_master set no_of_copies = Convert(int ,no_of_copies) + 1 where lib_code  = '" + libcode + "' and rack_no = '" + rackno + "'";
                        save = d2.update_method_wo_parameter(ins, "Text");
                        if (save > 0)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Book Information Saved Successfully";
                        }
                    }
                    else if (rblMultiple.Checked == true && txt_accno.Text != "")
                    {
                        int mul = 0;
                        int refer = 0;
                        if (rackno == "")
                            rf = 0;
                        else
                            rf = 1;
                        if (TxtMultiple.Text == "")
                        {
                            TxtMultiple.Text = "1";
                            mul = Convert.ToInt32(TxtMultiple.Text);
                        }
                        else
                        {
                            mul = Convert.ToInt32(TxtMultiple.Text);
                        }
                        if (rbl_yes.Checked == true)
                        {
                            if (text_ref.Text == "")
                                text_ref.Text = "1";
                            refer = Convert.ToInt32(text_ref.Text);
                            if (refer > mul)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "The reference book total ('" + text_ref.Text + "') should not be greater than Available books ('" + TxtMultiple.Text + "') ?";
                                return;
                                text_ref.Text = "";
                            }
                        }
                        if (TxtMultiple.Text != "")
                        {

                            int mulcopy = Convert.ToInt32(TxtMultiple.Text);
                            string accnum = txt_accno.Text;
                            for (int i = 0; i < mulcopy; i++)
                            {
                                if (i == 0)
                                {
                                    MultiCopyACcNo = Convert.ToString(accnum);
                                }
                                else
                                {
                                    string str = "";
                                    int index = 0;
                                    string StrVal = "";
                                    for (int k = 0; k < accnum.Length; k++)
                                    {
                                        string a = Convert.ToString(accnum.ElementAt<char>(k));
                                        if (a.All(char.IsNumber))
                                        {
                                            str = str + a;
                                        }
                                        if (a.All(char.IsLetter))
                                        {
                                            StrVal = StrVal + a;
                                        }
                                    }
                                    int jj = Convert.ToInt32(str) + 1;
                                    int BkAccJJ = 0;
                                    string StrBkAcc = Convert.ToString(jj);
                                    if (StrBkAcc.Length == 1)
                                        StrBkAcc = "0" + jj;
                                    else
                                        StrBkAcc = Convert.ToString(jj);
                                    accnum = StrVal + StrBkAcc;
                                    MultiCopyACcNo = MultiCopyACcNo + "," + Convert.ToString(accnum);
                                }
                            }
                        }
                        string[] mulBkArr = MultiCopyACcNo.Split(',');
                        for (int mulBok = 0; mulBok < mulBkArr.Length; mulBok++)
                        {
                            sqlqry = "insert into bookdetails(acc_no,title,author,publisher,supplier,edition,volume,part,pur_year,price,bill_no,bill_date,call_no,call_des,ref,dept_code,remark,access_date,access_time,Lib_code,sec_author,thi_author,collabrator,book_size,book_series,isbn,book_selected_by,book_accesed_by,date_accession,rack_flag,pur_don,book_status,attachment,topics,b_discount,volumetitle,subtitle,cur_value,cur_name,vol_price,subject,typeofbook,pub_place,key1,key2,key3,category,language,MulticopyAccNos)values ('" + mulBkArr[mulBok] + "','" + txt_title.Text + "','" + txt_author.Text + "','" + Txt_pub.Text + "','" + supplier + "','" + Txt_edit.Text + "','" + txvo.Text + "','" + txtpart.Text + "','" + txt_publisyear.Text + "','" + price + "','" + txt_billno.Text + "','" + billdate + "','" + callno + "','" + Calldes + "','" + reference + "','" + txt_depart.Text + "','" + txt_remarks.Text + "','" + accessdate + "','" + Acctime + "','" + libcode + "','" + Txt_SedAuthor.Text + "','" + ddl_thridAuthor.Text + "','" + txcoll.Text + "','" + txt_bookSz.Text + "','" + txtbose.Text + "','" + txt_isbn_No.Text + "','" + txtboselect.Text + "','" + txtboacc.Text + "','" + accessdate + "'," + rf + ",'" + inwardtype + "','" + status + "','" + Attache + "','" + txttopics.Text + "','" + txt_Offer.Text + "','" + txvolti.Text + "','" + txsubtitle.Text + "','" + txt_curval.Text + "','" + currntype + "','" + txvolpr.Text + "','" + Txt_sub.Text + "','" + Book_Type + "','','" + txkey1.Text + "','" + txkey2.Text + "','" + txkey3.Text + "','" + cate + "','" + language + "','" + MultiCopyACcNo + "')";
                            save = d2.update_method_wo_parameter(sqlqry, "Text");

                            if (rf == 1)
                            {
                                string saverack = "insert into rack_allocation(lib_code,rack_no,row_no,acc_no,access_date,access_time,book_type) values('" + libcode + "','" + rackno + "','" + shelfno + "','" + mulBkArr[mulBok] + "','" + Curtdate + "','" + Acctime + "','BOK')";
                                int save1 = d2.update_method_wo_parameter(saverack, "Text");
                                string sqrack = "update rackrow_master set no_of_copies  = no_of_copies +1 where rack_no = '" + rackno + "' and  row_no = '" + shelfno + "' and lib_code = '" + libcode + "'";
                                sqrack += "update rack_master set no_of_copies = no_of_copies +1 where lib_code  = '" + libcode + "' and rack_no = '" + rackno + "'";
                                int save2 = d2.update_method_wo_parameter(saverack, "Text");
                                //string MultiCopyACcNo1 = Convert.ToString(MultiCopyACcNo);
                                //string[] arr = MultiCopyACcNo1.Split(',');
                                //for (int i = 0; i < arr.Length; i++)
                                //{
                                //    string mulaccno = "Update bookdetails set MulticopyAccNos='" + MultiCopyACcNo1 + "' where Acc_No='" + arr[i] + "'";
                                //    saveacc = d2.update_method_wo_parameter(mulaccno, "Text");
                                //}
                                ChkAndAddPublisherEntry();
                                if (saveacc > 0)
                                {
                                    alertpopwindow.Visible = true;
                                    lblalerterr.Text = "Access No '" + txt_accno.Text + "' to '" + txt_accno.Text + "' is Saved Successfully";
                                    return;
                                }
                            }
                        }
                        if (callno != "" && Calldes != "")
                        {
                            string callinsert = "if not exists(select * from callnoentry where callno='" + callno + "' and callnodescription='" + Calldes + "')insert into callnoentry (callno,callnodescription) values ( '" + callno + "','" + Calldes + "')";
                            int sve = d2.update_method_wo_parameter(callinsert, "Text");
                        }
                        if (save > 0)
                        {
                            if (ViewState["bookimage"] != "0" && ViewState["size"] != "")
                            {
                                byte[] photoid = (byte[])(ViewState["bookimage"]);
                                int size = Convert.ToInt32(ViewState["size"]);
                                bookphotosave(txt_accno.Text, libcode, Book_Type, size, photoid);
                            }
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Book Details Saved Successfully";
                            TxtMultiple.Text = "";
                        }
                        loadcategory();
                    }
                }
            }
            else
            {
                if (ddl_entrytype.Items[0].Text == "Books")
                {
                    Book_update();
                }
            }
            clear();
            ddlsearcbook.Visible = false;
            popview.Visible = true;
            btn_Save.Visible = true;
            btn_Save.ImageUrl = "~/LibImages/save.jpg";
            btn_Delete.Visible = false;
            //ddl_txt_lib.Text = Libraryname;
            Type();
            loadcategory();
            Auto_AccessNo();
            //LoadLanguage();
            Categoryclick();
            LoadBudHead();
            loadinward();
            loagRackno();
            loadshelf();
            loadposition();
            LoadPosPlace();
            GetCurrencyType();
            LoadStatus();
            LoadAttache();
            LoadLang();
            Loadcalldes();
            LoadPubPlace();
            LoadBookType();
            loadlan();
            loadthridAuthor();
            IsNonBook.Checked = false;
            lblnonbook.Visible = false;
            txtnonbook.Visible = false;
            TxtMultiple.Visible = false;
            txt_date_acc.Attributes.Add("readonly", "readonly");
            txt_date_acc.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Txtbilldate.Attributes.Add("readonly", "readonly");
            Txtbilldate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Txtbilldate.Enabled = false;
            txt_Offer.Text = "";
            txt_title.Text = "";
            txt_author.Text = "";
            Txt_pub.Text = "";
            Txt_sub.Text = "";
            Txt_edit.Text = "";
            txvo.Text = "";
            txtpart.Text = "";
            txt_billno.Text = "";
            Txtbilldate.Text = "";
            ddl_CallNo.Text = "";
            txt_depart.Text = "";
            txt_remarks.Text = "";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    protected void btn_Delete_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_accno.Text != "")
            {
                if (ddl_status.Text != "Available")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "You can't delete this entry.Since the book is in " + ddl_status.Text + " status";
                    return;
                }
                else
                {

                    DivinwardDelete.Visible = true;
                    lbl_Diveleterecord.Text = "Are You Sure to Delete this Record?";
                }
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void btn_inwardcell_Delete_Click(object sender, EventArgs e)
    {
        try
        {
            int deleterecord = 0;
            string sqldeleterecord = "";
            libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (ddl_entrytype.Text == "Question Bank")
            {

                sqldeleterecord = "delete from  university_question where access_code='" + txt_accno.Text + "' and lib_code='" + libcode + "'";
                deleterecord = d2.update_method_wo_parameter(sqldeleterecord, "Text");

            }
            else if (ddl_entrytype.Text == "Project Book")
            {

                sqldeleterecord = "delete from  project_book where probook_accno='" + txt_accno.Text + "' and lib_code='" + libcode + "'";

                sqldeleterecord += "delete from  Project_BookDetails where probook_accno='" + txt_accno.Text + "' and lib_code='" + libcode + "'";
                deleterecord = d2.update_method_wo_parameter(sqldeleterecord, "Text");

            }
            else
            {

                sqldeleterecord = "delete from  bookdetails where acc_no='" + txt_accno.Text + "' and lib_code='" + libcode + "'";
                deleterecord = d2.update_method_wo_parameter(sqldeleterecord, "Text");

            }
            sqldeleterecord += "delete from rack_allocation where acc_no='" + txt_accno.Text + "' and rack_no='" + ddl_Rack.Text + "' and row_no='" + ddl_shelf.Text + "' and lib_code='" + libcode + "'";
            sqldeleterecord += "update rack_master set no_of_copies=no_of_copies-1 where rack_no='" + ddl_Rack.Text + "' and lib_code='" + libcode + "'";
            sqldeleterecord += "update rackrow_master set no_of_copies=no_of_copies-1 where rack_no='" + ddl_Rack.Text + "' and row_no='" + ddl_shelf.Text + "' and lib_code='" + libcode + "'";
            sqldeleterecord += "update rowpos_master set no_of_copies=no_of_copies-1 where rack_no='" + ddl_Rack.Text + "' and row_no='" + ddl_shelf.Text + "' and pos_no ='" + ddl_posi.Text + "' and Max_Capacity ='" + ddl_posplace.Text + "' and lib_code='" + libcode + "'";
            deleterecord = d2.update_method_wo_parameter(sqldeleterecord, "Text");
            if (deleterecord > 0)
            {
                popview.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Deleted Successfully";
                btngo_Click(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void btn_inwardcell_no_Delete_Click(object sender, EventArgs e)
    {
        DivinwardDelete.Visible = false;

    }

    #endregion

    protected void BtnsaveStud_Click(object sender, EventArgs e)
    {
        try
        {
            bool fileUp = fulstudp.HasFile;

            if (fulstudp.HasFile)
            {
                if (fulstudp.FileName.EndsWith(".jpg") || fulstudp.FileName.EndsWith(".jpeg") || fulstudp.FileName.EndsWith(".JPG") || fulstudp.FileName.EndsWith(".gif") || fulstudp.FileName.EndsWith(".png") || fulstudp.FileName.EndsWith(".gif") || fulstudp.FileName.EndsWith(".bmp"))
                {
                    Session["Image"] = fulstudp.PostedFile;
                    int fileSize = fulstudp.PostedFile.ContentLength;
                    ViewState["size"] = fileSize;
                    byte[] documentBinary = new byte[fileSize];
                    ViewState["bookimage"] = documentBinary;
                    fulstudp.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                    string base64String = Convert.ToBase64String(documentBinary, 0, documentBinary.Length);
                    imgstudp.ImageUrl = "data:image/;base64," + base64String;
                }

            }
        }
        catch (Exception ex)
        {
        }
    }

    protected int bookphotosave(string accnumber, string librcode, string booktype, int FileSize, byte[] DocDocument)
    {
        int Result = 0;
        try
        {
            if (accnumber.Trim() != "" && librcode.Trim() != "" && FileSize != 0 && booktype != "")
            {
                string InsPhoto = "if exists(select Acc_No,photo,lib_code,Book_Type from BookPhoto where Acc_No='" + txt_accno.Text + "' and Lib_Code ='" + libcode + "' and Book_Type ='" + Book_Type + "') update BookPhoto set photo=@photoid here Acc_No='" + txt_accno.Text + "' and Lib_Code ='" + libcode + "' and Book_Type ='" + Book_Type + "' else insert into BookPhoto (Acc_No,photo,lib_code,Book_Type) values(" + accnumber + ",@photoid," + libcode + "," + booktype + ")";
                SqlCommand cmd = new SqlCommand(InsPhoto, ssql);
                SqlParameter uploadedsubject_name = new SqlParameter("@photoid", SqlDbType.Binary, FileSize);
                uploadedsubject_name.Value = DocDocument;
                cmd.Parameters.Add(uploadedsubject_name);
                ssql.Close();
                ssql.Open();
                Result = cmd.ExecuteNonQuery();
                ssql.Close();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

        return Result;
    }

    #region Book_Update

    public void Book_update()
    {
        try
        {
            string bookStatus = "";
            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddllibrary.SelectedValue);
            if (ddl_txt_lib.Items.Count > 0)
                libcode = Convert.ToString(ddl_txt_lib.SelectedValue);
            if (ddl_Rack.Items.Count > 0)
                rackno = Convert.ToString(ddl_Rack.SelectedItem.Text);
            if (ddl_shelf.Items.Count > 0)
                shelfno = Convert.ToString(ddl_shelf.SelectedItem.Text);
            if (ddl_language.Items.Count > 0)
                language = Convert.ToString(ddl_language.SelectedItem.Text);
            if (ddl_atta.Items.Count > 0)
                Attache = Convert.ToString(ddl_atta.SelectedItem.Text).ToUpper();
            if (ddl_curren.Items.Count > 0)
                currntype = Convert.ToString(ddl_curren.SelectedItem.Text).ToUpper();
            if (ddl_Category.Items.Count > 0)
                cate = Convert.ToString(ddl_Category.SelectedItem.Text);
            if (ddl_CallNo.Items.Count > 0)
                callno = Convert.ToString(ddl_CallNo.SelectedItem.Text);
            if (ddl_title_lan.Items.Count > 0)
                titlelang = Convert.ToString(ddl_title_lan.SelectedIndex);
            if (ddl_Author_lan.Items.Count > 0)
                authorlang = Convert.ToString(ddl_Author_lan.SelectedIndex);
            if (ddl_publishplace.Items.Count > 0)
                pubplace = Convert.ToString(ddl_publishplace.SelectedItem.Text);
            if (ddl_booktype.Items.Count > 0)
                booktype = Convert.ToString(ddl_booktype.SelectedItem.Text);
            if (ddl_Description.Items.Count > 0)
                Calldes = Convert.ToString(ddl_Description.SelectedItem.Text);
            if (ddl_inward_type.Items.Count > 0)
                inwardtype = Convert.ToString(ddl_inward_type.SelectedItem.Text);
            if (ddl_posi.Items.Count > 0)
                pos = Convert.ToString(ddl_posi.SelectedItem.Text);
            if (ddl_posplace.Items.Count > 0)
                posplc = Convert.ToString(ddl_posplace.SelectedItem.Text);
            if (ddl_status.Items.Count > 0)
                bookStatus = Convert.ToString(ddl_status.SelectedItem.Text);
            if (txt_Price.Text != "")
                price = txt_Price.Text;
            else
                price = "0";
            if (rbl_yes.Checked == true)
            {
                reference = "Yes";
                Book_Type = "REF";
            }
            else
            {
                reference = "No";
                Book_Type = "BOK";
            }
            string supplier1 = Convert.ToString(ddlsupp.SelectedItem.Text);
            string Accdate = Convert.ToString(txt_date_acc.Text);
            string[] adate = Accdate.Split('/');
            if (adate.Length == 3)
            {
                accessdate = adate[1].ToString() + "/" + adate[0].ToString() + "/" + adate[2].ToString();
                string AccesDt = adate[1].ToString();
                AccesDt = AccesDt.StartsWith("0") ? AccesDt.Substring(1) : AccesDt;
                accessdate = AccesDt + "/" + adate[0] + "/" + adate[2];
            }
            string Acctime = DateTime.Now.ToString("hh:mm tt");

            //Book Save 

            if (shelfno != "" && rackno != "")
            {
                sqlqry = "SELECT rackrow_master.max_capacity,rackrow_master.no_of_copies From rackrow_master, rack_allocation WHERE rackrow_master.lib_code = '" + libcode + "' AND rack_allocation.rack_no = '" + rackno + "' AND rack_allocation.row_no = '" + shelfno + "' AND rack_allocation.acc_no = '" + txt_accno.Text + "' AND rack_allocation.rack_no = rackrow_master.rack_no AND rack_allocation.row_no = rackrow_master.row_no";
                dsbooksave.Clear();
                dsbooksave = d2.select_method_wo_parameter(sqlqry, "Text");
                if (dsbooksave.Tables[0].Rows.Count > 0)
                    fl = 2;
                string copy = d2.GetFunction("select count(*) from rack_allocation where lib_code='" + libcode + "' and rack_no='" + rackno + "' and row_no='" + shelfno + "'");
                if (copy != "" && copy != "0")
                    varCopies = Convert.ToInt32(copy);
                else
                    varCopies = 0;
                string maxcap = d2.GetFunction("select max_capacity from rackrow_master where lib_code='" + libcode + "' and rack_no = '" + rackno + "' and row_no='" + shelfno + "'");
                maxca = Convert.ToInt32(maxcap);
                if (varCopies < maxca)
                {
                    string ssql = d2.GetFunction("select rack_flag from bookdetails where acc_no='" + txt_accno.Text + "' and lib_code='" + libcode + "'");
                    if (ssql == "0")
                        fl = 1;
                    else
                        fl = 2;
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "This Shelf is Fully allocated.Hence Choose Some Other Shelf";
                    return;
                }
            }
            if (shelfno != "" && rackno != "")
                rflag = 1;
            else
                rflag = 0;
            if (txt_depart.Text != "")
            {
                string dept = "if exists(select * from journal_dept where dept_name='" + txt_depart.Text + "' and college_code =" + collcode + " AND Lib_Code ='" + libcode + "')update journal_dept set dept_acr=' ' where dept_name='" + txt_depart.Text + "' AND Lib_code ='" + libcode + "' AND College_Code ='" + collcode + "' else INSERT into journal_dept (dept_name,dept_Acr,lib_code,college_code) values ('" + txt_depart.Text + "',' ','" + libcode + "','" + collcode + "')";
                save = d2.update_method_wo_parameter(dept, "Text");
            }
            if (Attache != "")
            {
                string att = "if exists(select * from attachment where attachment_name='" + Attache + "')update attachment set  attachment_name='" + Attache + "' where attachment_name='" + Attache + "' else INSERT into attachment (attachment_name) values ('" + Attache + "')";
                save = d2.update_method_wo_parameter(att, "Text");
            }
            if (currntype != "")
            {
                string cutype = "if exists(select * from currency_convertion where currency_type='" + currntype + "')update currency_convertion set currency_type='" + currntype + "' where currency_type='" + currntype + "' else INSERT into currency_convertion (currency_type) values ('" + currntype + "')";
                save = d2.update_method_wo_parameter(cutype, "Text");
            }
            if (rblSingle.Checked == true || rblMultiple.Checked == true)
            {
                sqlqry = "select * from bookdetails where acc_no='" + txt_accno.Text + "' and lib_code='" + libcode + "'";// and bookid ='" + txt_bno.Text + "'
                ds.Clear();
                ds = d2.select_method_wo_parameter(sqlqry, "Text");
                string BookId = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    etitle = Convert.ToString(ds.Tables[0].Rows[0]["title"]);
                    eauthor = Convert.ToString(ds.Tables[0].Rows[0]["author"]);
                    BookId = Convert.ToString(ds.Tables[0].Rows[0]["bookid"]);
                    updateqry = "update bookdetails set title='" + txt_title.Text + "',author='" + txt_author.Text + "',sec_author='" + Txt_SedAuthor.Text + "',thi_author='" + ddl_thridAuthor.Text + "',collabrator='" + txcoll.Text + "',call_no='" + callno + "',price ='" + price + "',TitleLanguage='" + titlelang + "',AuthorLanguage='" + authorlang + "',pub_place ='" + pubplace + "',supplier='" + supplier1 + "'  where acc_no='" + txt_accno.Text + "' and lib_code='" + libcode + "' and bookid ='" + BookId + "'";
                    save = d2.update_method_wo_parameter(updateqry, "Text");
                }
                if (rblMultiple.Checked == true)
                {
                    string MultiCopyACcNo1 = Convert.ToString(ds.Tables[0].Rows[0]["MultiCopyAccNos"]);
                    string[] arr = MultiCopyACcNo1.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string updateqry1 = "update bookdetails set title='" + txt_title.Text + "',author='" + txt_author.Text + "',sec_author='" + Txt_SedAuthor.Text + "',thi_author='" + ddl_thridAuthor.Text + "',collabrator='" + txcoll.Text + "',call_no='" + callno + "',price ='" + price + "',TitleLanguage='" + titlelang + "',AuthorLanguage='" + authorlang + "',pub_place ='" + pubplace + "',supplier='" + supplier1 + "'  where title='" + txt_title.Text + "' and author='" + txt_author.Text + "' and  Lib_Code='" + libcode + "'and acc_no='" + arr[i] + "'";
                        save = d2.update_method_wo_parameter(updateqry1, "Text");

                        string sql1 = "update bookdetails set ref='" + reference + "',typeofbook='" + booktype + "',book_size='" + txt_bookSz.Text + "',book_series='" + txtbose.Text + "',isbn='" + txt_isbn_No.Text + "',book_selected_by='" + txtboselect.Text + "',book_accesed_by='" + txtboacc.Text + "',date_accession='" + accessdate + "',attachment='" + Attache + "', dept_code='" + txt_depart.Text + "',category='" + cate + "',rack_flag='" + rflag + "' ,publisher='" + Txt_pub.Text + "',supplier='" + supplier1 + "', edition='" + Txt_edit.Text + "', volume='" + txvo.Text + "',part='" + txtpart.Text + "',pur_year='" + txt_publisyear.Text + "', price='" + price + "', bill_no='" + txt_billno.Text + "', bill_date='" + billdate + "',volumetitle='" + txvolti.Text + "',call_des='" + Calldes + "',remark='" + txt_remarks.Text + "',pur_don='" + inwardtype + "',topics='" + txttopics.Text + "',pub_place='" + pubplace + "',key1='" + txkey1.Text + "',key2='" + txkey1.Text + "',key3='" + txkey3.Text + "',sec_author='" + Txt_SedAuthor.Text + "',Call_No='" + callno + "',TitleLanguage ='" + titlelang + "',AuthorLanguage='" + authorlang + "',subtitle='" + txsubtitle.Text + "',b_discount='" + txt_Offer.Text + "',cur_name='" + currntype + "',cur_value='" + txt_curval.Text + "',vol_price='" + txvolpr.Text + "',subject='" + Txt_sub.Text + "',language='" + language + "' where acc_no='" + arr[i] + "' and  Lib_Code='" + libcode + "' and thi_author='" + ddl_thridAuthor.Text + "' and sec_author='" + Txt_SedAuthor.Text + "'";
                        save = d2.update_method_wo_parameter(sql1, "Text");
                        string sql3 = "update borrow set title='" + txt_title.Text + "',author='" + txt_author.Text + "' where title='" + etitle + "' and author='" + eauthor + "'";
                        save = d2.update_method_wo_parameter(sql3, "Text");
                        if (fl == 2)
                        {
                            //Update Rack Details for multiple book
                            string saveract = "if exists(select * count from rack_allocation  where acc_no='" + arr[i] + "' and book_type='" + Book_Type + "' and lib_code = '" + libcode + "')update rack_allocation set lib_code='" + libcode + "',rack_no='" + rackno + "',row_no='" + shelfno + "',Pos_No ='" + pos + "',Pos_Place ='" + posplc + "',acc_no='" + arr[i] + "',access_date='" + txt_date_acc.Text + "',access_time='" + Acctime + "',book_type='" + Book_Type + "' where acc_no='" + arr[i] + "' and book_type='" + Book_Type + "' and lib_code = '" + libcode + "' else insert into rack_allocation values('" + libcode + "','" + rackno + "','" + shelfno + "','" + arr[i] + "','" + accessdate + "','" + Acctime + "','" + Book_Type + "','" + pos + "','" + posplc + "')";
                        }
                        else if (fl == 1)
                        {
                            string deleteqry = "delete from rack_allocation where acc_no='" + arr[i] + "' and lib_code='" + libcode + "' and book_type='" + Book_Type + "'";
                            Delete = d2.update_method_wo_parameter(deleteqry, "Text");
                            sql1 = "insert into rack_allocation values('" + libcode + "','" + rackno + "','" + shelfno + "','" + arr[i] + "','" + accessdate + "','" + Acctime + "','" + Book_Type + "','" + pos + "','" + posplc + "')";
                            save = d2.update_method_wo_parameter(sql1, "Text");
                            string sql2 = "update rackrow_master set no_of_copies  = no_of_copies +1 where rack_no = '" + rackno + "' and  row_no = '" + shelfno + "' and lib_code = '" + libcode + "'";
                            sql2 += "update rack_master set no_of_copies = no_of_copies +1 where lib_code  ='" + libcode + "' and rack_no = '" + rackno + "'";
                            sql2 += "update rowpos_master set no_of_copies  = no_of_copies +1 where rack_no = '" + rackno + "' and  row_no = '" + shelfno + "' and pos_no ='" + pos + "' and pos_place ='" + posplc + "' and lib_code ='" + libcode + "'";
                            save = d2.update_method_wo_parameter(sql2, "Text");
                        }
                    }
                }
                int save1 = 0;
                if (rblSingle.Checked == true)
                {
                    string sql1 = "update bookdetails set ref='" + reference + "',typeofbook='" + booktype + "',book_size='" + txt_bookSz.Text + "',book_series='" + txtbose.Text + "',isbn='" + txt_isbn_No.Text + "',book_selected_by='" + txtboselect.Text + "',book_accesed_by='" + txtboacc.Text + "',date_accession='" + accessdate + "',attachment='" + Attache + "', dept_code='" + txt_depart.Text + "',category='" + cate + "',rack_flag='" + rflag + "' ,publisher='" + Txt_pub.Text + "',supplier='" + supplier1 + "', edition='" + Txt_edit.Text + "', volume='" + txvo.Text + "',part='" + txtpart.Text + "',pur_year='" + txt_publisyear.Text + "', price='" + price + "', bill_no='" + txt_billno.Text + "', bill_date='" + billdate + "',volumetitle='" + txvolti.Text + "',call_des='" + Calldes + "',remark='" + txt_remarks.Text + "',pur_don='" + inwardtype + "',topics='" + txttopics.Text + "',pub_place='" + pubplace + "',key1='" + txkey1.Text + "',key2='" + txkey1.Text + "',key3='" + txkey3.Text + "',sec_author='" + Txt_SedAuthor.Text + "',Call_No='" + callno + "',TitleLanguage ='" + titlelang + "',AuthorLanguage='" + authorlang + "',subtitle='" + txsubtitle.Text + "',b_discount='" + txt_Offer.Text + "',cur_name='" + currntype + "',cur_value='" + txt_curval.Text + "',vol_price='" + txvolpr.Text + "',subject='" + Txt_sub.Text + "',language='" + language + "',book_status='" + bookStatus + "' where acc_no='" + txt_accno.Text + "' and  Lib_Code='" + libcode + "' and BookId='" + BookId + "'";
                    save = d2.update_method_wo_parameter(sql1, "Text");
                    string sql3 = "update borrow set title='" + txt_title.Text + "',author='" + txt_author.Text + "' where title='" + etitle + "' and author='" + eauthor + "'";
                    save1 = d2.update_method_wo_parameter(sql3, "Text");
                    string inser = "";
                    if (fl == 2)
                    {
                        //Update Rack Details for multiple book
                        string saveract = "if exists(select * count from rack_allocation  where acc_no='" + txt_accno.Text + "' and book_type='" + Book_Type + "' and lib_code = '" + libcode + "')update rack_allocation set lib_code='" + libcode + "',rack_no='" + rackno + "',row_no='" + shelfno + "',Pos_No ='" + pos + "',Pos_Place ='" + posplc + "',acc_no='" + txt_accno.Text + "',access_date='" + txt_date_acc.Text + "',access_time='" + Acctime + "',book_type='" + Book_Type + "' where acc_no='" + txt_accno.Text + "' and book_type='" + Book_Type + "' and lib_code = '" + libcode + "' else insert into rack_allocation values('" + libcode + "','" + rackno + "','" + shelfno + "','" + txt_accno.Text + "','" + accessdate + "','" + Acctime + "','" + Book_Type + "','" + pos + "','" + posplc + "')";
                        save1 = d2.update_method_wo_parameter(saveract, "Text");

                        inser = "update rackrow_master set no_of_copies  = Convert(int ,no_of_copies) + 1 where rack_no = '" + rackno + "' and  row_no = '" + shelfno + "' and lib_code = '" + libcode + "'";

                        inser += "update rack_master set no_of_copies = Convert(int ,no_of_copies) + 1 where lib_code  = '" + libcode + "' and rack_no = '" + rackno + "'";
                        save1 = d2.update_method_wo_parameter(inser, "Text");
                    }
                    else if (fl == 1)
                    {
                        string deleteqry = "delete from rack_allocation where acc_no='" + txt_accno.Text + "' and lib_code='" + libcode + "' and book_type='" + Book_Type + "'";
                        Delete = d2.update_method_wo_parameter(deleteqry, "Text");
                        sql1 = "insert into rack_allocation values('" + libcode + "','" + rackno + "','" + shelfno + "','" + txt_accno.Text + "','" + accessdate + "','" + Acctime + "','" + Book_Type + "','" + pos + "','" + posplc + "')";
                        save1 = d2.update_method_wo_parameter(sql1, "Text");
                        string sql2 = "update rackrow_master set no_of_copies  = no_of_copies +1 where rack_no = '" + rackno + "' and  row_no = '" + shelfno + "' and lib_code = '" + libcode + "'";
                        sql2 += "update rack_master set no_of_copies = no_of_copies +1 where lib_code  ='" + libcode + "' and rack_no = '" + rackno + "'";
                        sql2 += "update rowpos_master set no_of_copies  = no_of_copies +1 where rack_no = '" + rackno + "' and  row_no = '" + shelfno + "' and pos_no ='" + pos + "' and pos_place ='" + posplc + "' and lib_code ='" + libcode + "'";
                        save1 = d2.update_method_wo_parameter(sql2, "Text");
                    }
                }
                if (save > 0)
                {
                    if (ViewState["bookimage"] != "0" && ViewState["size"] != "")
                    {
                        byte[] photoid = (byte[])(ViewState["bookimage"]);
                        int size = Convert.ToInt32(ViewState["size"]);
                        bookphotosave(txt_accno.Text, libcode, Book_Type, size, photoid);
                    }
                    BookSave = true;
                }
                if (Calldes != "" && callno != "")
                {
                    string getcall = d2.GetFunction("select * from callnoentry where callno='" + callno + "' and callnodescription='" + Calldes + "'");
                    if (getcall == "")
                        sqlqry = "insert into callnoentry (callno,callnodescription) values ('" + callno + "','" + Calldes + "')";
                    save1 = d2.update_method_wo_parameter(sqlqry, "Text");
                }
            }
            if (BookSave)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Book Information Updated Successfully";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "InwardEntry");
        }
    }

    #endregion

    #region Newupdate

    public void news_update()
    {
        try
        {
            string crdate = DateTime.Now.ToString("dd/mm/yyyy");
            int papercnt = 0;
            int newsv1 = 0;
            if (ddl_txt_lib.Items.Count > 0)
                libcode = Convert.ToString(ddl_txt_lib.SelectedValue);
            string var_papername = txt_title.Text;
            string newcnt = d2.GetFunction("select Count(*) from newspaper where paper_name='" + var_papername + "'");
            if (newcnt == "" && newcnt == "0")
            {
                string papcode = d2.GetFunction("select count(*),max(paper_code) from newspaper");
                if (papcode != "")
                {
                    papercnt = Convert.ToInt32(papcode);
                    papercnt = papercnt + 1;
                }
                else
                {
                    papercnt = 1;
                }
                string newup = "insert into newspaper values('" + papercnt + "','" + var_papername + "')";
                newsv1 = d2.update_method_wo_parameter(newup, "Text");
            }
            string attcnt1 = "if not exists(select * from attachment where attachment_name='" + Attache + "')insert into attachment(attachment_name,lib_code) values('" + Attache + "','" + libcode + "')";
            newsv1 = d2.update_method_wo_parameter(attcnt1, "Text");
            string upnews = "if exists(select * from news_paper where cur_date ='" + crdate + "' and title = '" + var_papername + "' and Lib_code='" + libcode + "' and  serial_no <> '" + txt_accno.Text + "')update news_paper set cur_date='" + crdate + "',title='" + var_papername + "',price='" + price + "',attachment ='" + Attache + "' ,noofcopies=" + txt_newcopy.Text + "where serial_no='" + txt_accno.Text + "' and Lib_code='" + libcode + "'";
            newsv1 = d2.update_method_wo_parameter(upnews, "Text");
            if (newsv1 > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "News Paper Details Updated Successfully";


            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }
    #endregion

    #region qusupdate

    public void Ques_Update()
    {
        try
        {
            string crndate = DateTime.Now.ToString("dd/mm/yyyy");
            string year = "";
            int upques = 0;
            string sta = "";
            if (ddl_txt_lib.Items.Count > 0)
                libcode = Convert.ToString(ddl_txt_lib.SelectedValue);
            if (ddl_status.Items.Count > 0)
            {
                sta = Convert.ToString(ddl_status.SelectedItem.Text);
            }
            if (Text_year.Text != "")
            {
                year = Text_year.Text;
                if (year.Length < 4)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Year Should Be In Four Digits";
                    return;
                }
                string updateques = "update university_question set access_date = '" + crndate + "',access_time = '" + Acctime + "',title = '" + txt_title.Text + "',paper_name = '" + txpagename.Text + "',degree_code = '',semester = '" + Text_sem.Text + "',sem_month = '" + Text_mon.Text + "',sem_year = '" + year + "',remarks = '" + txt_remarks.Text + "' ,issue_flag ='" + sta + "' where access_code = '" + txt_accno.Text + "' and lib_code = '" + libcode + "'";
                upques = d2.update_method_wo_parameter(updateques, "Text");
            }
            if (upques > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Question Paper details Updated Successfully";
            }


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    #endregion

    #region  ChkAndAddPublisherEntry
    public void ChkAndAddPublisherEntry()
    {
        try
        {
            int pubsave = 0;
            string Currentdate = DateTime.Now.ToString("dd/mm/yyyy");
            if (Txt_pub.Text != "")
            {
                int codeno1 = 0;
                string codeno = d2.GetFunction("select max(cast(publisher_code as integer)) from publisher_details");
                if (codeno != "")
                {
                    codeno1 = Convert.ToInt32(codeno);
                    codeno1 = codeno1 + 1;
                }
                else
                {
                    codeno1 = 1;
                }

                string savepup = "if not exists(Select * from publisher_details where Publisher_name='" + Txt_pub.Text + "') insert into publisher_details          (access_date,access_time,publisher_code,publisher_name,doorst_no,city,pin,phone_no,fax_no,email,created_date)values ('" + accessdate + "','" + Acctime + "','" + codeno1 + "','" + Txt_pub.Text + "','','','','','','','" + Currentdate + "')";
                pubsave = d2.update_method_wo_parameter(savepup, "Text");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    #endregion

    #region Exit
    protected void btn_Exit_Click(object sender, EventArgs e)
    {
        popview.Visible = false;
        ddl_CallNo.Items.Clear();
        chkGridSelectAll.Visible = false;
    }

    #endregion

    #endregion

    #region Non_Book_Popup

    protected void ddl_Library_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loaddepartment();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void ddl_non_budget_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddl_mat_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    #region Journal_AccNo_Popup

    protected void btn_jour_popup_OnClick(object sender, EventArgs e)
    {
        try
        {
            string matname = "";
            if (ddl_mat.Items.Count > 0)
                matname = Convert.ToString(ddl_mat.SelectedItem.Text);
            if (matname != "")
            {
                popwindowjournalaccno.Visible = true;
                GrdBkAccNo.Visible = false;
                btn_pop2exit.Visible = false;
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "please Select The Attachement Material Name";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void ddl_Search_By_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GrdBkAccNo.Visible = false;
            btn_pop2exit.Visible = false;
            if (ddl_Search_By.SelectedIndex == 0)
                txt_bysearch.Visible = false;
            else if (ddl_Search_By.SelectedIndex == 1)
                txt_bysearch.Visible = true;
            else if (ddl_Search_By.SelectedIndex == 2)
                txt_bysearch.Visible = true;
            else if (ddl_Search_By.SelectedIndex == 3)
                txt_bysearch.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void btn_journalaccno_go_Click(object sender, EventArgs e)
    {
        try
        {
            string Field_Name = "";
            string nonlibcode = "";
            string matname = "";
            string search = "";
            string sql = "";
            DataSet dsjouaccno = new DataSet();
            if (ddllibrary.Items.Count > 0)
                nonlibcode = Convert.ToString(ddllibrary.SelectedValue);
            if (ddl_mat.Items.Count > 0)
                matname = Convert.ToString(ddl_mat.SelectedItem.Text);

            if (ddl_Search_By.Items[0].Text == "All")
            {
                txt_bysearch.Text = "";
                txt_bysearch.Visible = false;
                Field_Name = "";
                search = "";
            }
            else
            {
                if (ddl_Search_By.Items[1].Text == "Journal Code")
                {
                    txt_bysearch.Visible = true;
                    Field_Name = "journal_master.journal_code";
                    search = "Journal Code";
                }
                else if (ddl_Search_By.Items[2].Text == "Journal Title")
                {
                    txt_bysearch.Visible = true;
                    Field_Name = "journal_master.journal_name";
                    search = "Journal Title";
                }
                else
                {
                    txt_bysearch.Visible = true;
                    Field_Name = "journal.dept_name";
                    search = "Dept Name";
                }
            }
            string cond = " journal.lib_code = '" + nonlibcode + "' order by journal.journal_code";//and journal.attachement='" + matname + "' and journal.access_code not in (select acc_no from nonbookmat)
            if (txt_acc_coe.Text == "")
            {
                if (search == "Journal Code" || search == "Journal Title" || search == "Dept Name")
                {
                    if (txt_bysearch.Text == "")
                        sql = "SELECT journal.access_code,journal.journal_code,journal_master.journal_name,journal.volume_no,journal.issue_no,journal.dept_name FROM journal INNER JOIN journal_master ON (journal.journal_code = journal_master.journal_code) AND (journal.lib_code = journal_master.lib_code) where " + cond + "";
                    else
                        sql = "SELECT journal.access_code,journal.journal_code,journal_master.journal_name,journal.volume_no,journal.issue_no,journal.dept_name FROM journal INNER JOIN journal_master ON (journal.journal_code = journal_master.journal_code) AND (journal.lib_code = journal_master.lib_code) where " + Field_Name + " like '%" + txt_bysearch.Text + "%' and " + cond + "";
                }
                else
                    sql = "SELECT journal.access_code,journal.journal_code,journal_master.journal_name,journal.volume_no,journal.issue_no,journal.dept_name FROM journal INNER JOIN journal_master ON (journal.journal_code = journal_master.journal_code) AND (journal.lib_code = journal_master.lib_code) where " + cond + "";
            }
            else
            {
                if (search == "Journal Code" || search == "Journal Title" || search == "Dept Name")
                {
                    if (txt_bysearch.Text == "")
                        sql = "SELECT journal.access_code,journal.journal_code,journal_master.journal_name,journal.volume_no,journal.issue_no,journal.dept_name FROM journal INNER JOIN journal_master ON (journal.journal_code = journal_master.journal_code) AND (journal.lib_code = journal_master.lib_code) where journal.access_code like '%" + txt_acc_coe.Text + "%' and " + cond + "";
                    else
                        sql = "SELECT journal.access_code,journal.journal_code,journal_master.journal_name,journal.volume_no,journal.issue_no,journal.dept_name FROM journal INNER JOIN journal_master ON (journal.journal_code = journal_master.journal_code) AND (journal.lib_code = journal_master.lib_code) where " + Field_Name + " like '%" + txt_bysearch.Text + "%' and journal.access_code like '%" + (txt_acc_coe.Text) + "%' and " + cond + "";
                }
                else
                {
                    sql = "SELECT journal.access_code,journal.journal_code,journal_master.journal_name,journal.volume_no,journal.issue_no,journal.dept_name FROM journal INNER JOIN journal_master ON (journal.journal_code = journal_master.journal_code) AND (journal.lib_code = journal_master.lib_code) where journal.access_code like '%" + txt_acc_coe.Text + "%' and " + cond + "";
                }
            }
            dsjouaccno.Clear();
            dsjouaccno = d2.select_method_wo_parameter(sql, "Text");

            if (dsjouaccno.Tables.Count > 0 && dsjouaccno.Tables[0].Rows.Count > 0)
            {
                DataTable dtjouaccno = new DataTable();
                DataRow drow;

                dtjouaccno.Columns.Add("Access Code", typeof(string));
                dtjouaccno.Columns.Add("Journal Code", typeof(string));
                dtjouaccno.Columns.Add("Journal Title", typeof(string));
                dtjouaccno.Columns.Add("Volume No", typeof(string));
                dtjouaccno.Columns.Add("Issue No", typeof(string));
                dtjouaccno.Columns.Add("Dept Name", typeof(string));
                for (int rolcount = 0; rolcount < dsjouaccno.Tables[0].Rows.Count; rolcount++)
                {
                    drow = dtjouaccno.NewRow();
                    drow["Access Code"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["access_code"]);
                    drow["Journal Code"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["journal_code"]);
                    drow["Journal Title"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["journal_name"]);
                    drow["Volume No"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["volume_no"]);
                    drow["Issue No"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["issue_no"]);
                    drow["Dept Name"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["dept_name"]);
                    dtjouaccno.Rows.Add(drow);
                }
                GrdVNonBkAccNo.DataSource = dtjouaccno;
                GrdVNonBkAccNo.DataBind();
                GrdVNonBkAccNo.Visible = true;
                divGrdVNonBkAccNo.Visible = true;
                btn_pop2exit.Visible = true;
                for (int l = 0; l < GrdVNonBkAccNo.Rows.Count; l++)
                {
                    foreach (GridViewRow row in GrdVNonBkAccNo.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            GrdVNonBkAccNo.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            GrdVNonBkAccNo.Rows[l].Cells[4].HorizontalAlign = HorizontalAlign.Right;
                            GrdVNonBkAccNo.Rows[l].Cells[5].HorizontalAlign = HorizontalAlign.Right;
                        }
                    }
                }
            }
            else
            {
                GrdVNonBkAccNo.Visible = false;
                btn_pop2exit.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Records Found";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void GrdVNonBkAccNo_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , HiddenField2.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void GrdVNonBkAccNo_SelectedIndexChanged(Object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.HiddenField2.Value);
        string caldes = Convert.ToString(GrdVNonBkAccNo.Rows[rowIndex].Cells[2].Text);
        //if (rowIndex != "" && rowIndex != "-1")
        //{
        string journalAccno = Convert.ToString(GrdVNonBkAccNo.Rows[rowIndex].Cells[1].Text);
        string journaltitle = Convert.ToString(GrdVNonBkAccNo.Rows[rowIndex].Cells[3].Text);
        string volumeno = Convert.ToString(GrdVNonBkAccNo.Rows[rowIndex].Cells[4].Text);
        txt_jour.Text = journalAccno;
        txtitle.Text = journaltitle;
        txtitle.Enabled = false;
        txtvol.Text = volumeno;
        txtvol.Enabled = false;
        ddl_mat.Enabled = false;
        popwindowjournalaccno.Visible = false;
        //}

    }

    //protected void btn_pop2ok_Click(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        string activerow = "";
    //        string activecol = "";
    //        fpaccno.SaveChanges();
    //        activerow = fpaccno.ActiveSheetView.ActiveRow.ToString();
    //        activecol = fpaccno.ActiveSheetView.ActiveColumn.ToString();
    //        if (activerow != "" && activerow != "-1")
    //        {
    //            string journalAccno = Convert.ToString(fpaccno.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
    //            string journaltitle = Convert.ToString(fpaccno.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
    //            string volumeno = Convert.ToString(fpaccno.Sheets[0].Cells[fpaccno.Sheets[0].RowCount - 1, 4].Tag);
    //            txt_jour.Text = journalAccno;
    //            txtitle.Text = journaltitle;
    //            txtvol.Text = volumeno;
    //            popwindowjournalaccno.Visible = false;
    //        }

    //    }
    //    catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    //}

    protected void btn_pop2exit_Click(object sender, EventArgs e)
    {
        popwindowjournalaccno.Visible = false;
    }

    protected void imagebtnpop2close_Click(object sender, EventArgs e)
    {
        popwindowjournalaccno.Visible = false;
    }

    //protected void GrdVNonBkAccNo_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    //{
    //    GrdVNonBkAccNo.PageIndex = e.NewPageIndex;
    //    btn_journalaccno_go_Click(sender, e);
    //}

    #endregion

    #region Book_AccNo_Popup

    protected void btn_book_accnopopup_OnClick(object sender, EventArgs e)
    {
        try
        {

            string matname = "";
            if (ddl_mat.Items.Count > 0)
                matname = Convert.ToString(ddl_mat.SelectedItem.Text);
            if (matname != "")
            {
                DivBookAccessNo.Visible = true;
                GrdVNonBkAccNo.Visible = false;
                btn_pop2exit.Visible = false;
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "please Select The Attachement Material Name";
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void ddl_search_book_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GrdBkAccNo.Visible = false;
            btn_book_exit.Visible = false;
            if (ddl_search_book.SelectedIndex == 0)
                txt_book_search.Visible = false;
            else if (ddl_search_book.SelectedIndex == 1)
                txt_book_search.Visible = true;
            else if (ddl_search_book.SelectedIndex == 2)
                txt_book_search.Visible = true;
            else if (ddl_search_book.SelectedIndex == 3)
                txt_book_search.Visible = true;
            else if (ddl_search_book.SelectedIndex == 4)
                txt_book_search.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void btn_book_go_Click(object sender, EventArgs e)
    {
        try
        {
            GrdBkAccNo.Visible = true;
            string nonlbcode = "";
            string nonmatname = "";
            string FieldName = "";
            string Searchbook = "";
            DataSet dsbook = new DataSet();
            if (ddl_search_book.Items[0].Text == "All")
            {
                txt_book_search.Text = "";
                txt_book_search.Visible = false;
                FieldName = "";
                Searchbook = "";
            }
            else if (ddl_search_book.Items[1].Text == "Title")
            {
                txt_book_search.Visible = true;
                FieldName = "bookdetails.Title";
                Searchbook = "Title";
            }
            else if (ddl_search_book.Items[2].Text == "Author")
            {
                txt_book_search.Visible = true;
                FieldName = "bookdetails.Author";
                Searchbook = "Author";
            }
            else if (ddl_search_book.Items[3].Text == "Publisher")
            {
                txt_book_search.Visible = true;
                FieldName = "bookdetails.Publisher";
                Searchbook = "Publisher";
            }
            else
            {
                txt_book_search.Visible = true;
                FieldName = "bookdetails.Edition";
                Searchbook = "Edition";
            }
            if (ddllibrary.Items.Count > 0)
                nonlbcode = Convert.ToString(ddllibrary.SelectedValue);
            if (ddl_mat.Items.Count > 0)
                nonmatname = Convert.ToString(ddl_mat.SelectedItem.Text);
            if (txt_boaccno.Text == "")
            {
                if (Searchbook == "" || Searchbook == "All")
                {
                    sqlqry = "select acc_no,title,author,publisher,edition from bookdetails where lib_code='" + nonlbcode + "' order by len(acc_no),acc_no ";//and attachment = '" + nonmatname + "' and acc_no  not in (select acc_no from nonbookmat where lib_code='" + nonlbcode + "')
                }
                else
                {
                    if (txt_book_search.Text == "")

                        sqlqry = "select acc_no,title,author,publisher,edition from bookdetails where lib_code='" + nonlbcode + "' order by len(acc_no),acc_no";//and attachment = '" + nonmatname + "' and acc_no  not in (select acc_no from nonbookmat where lib_code='" + nonlbcode + "')
                    else

                        sqlqry = "select acc_no,title,author,publisher,edition from bookdetails where lib_code='" + nonlbcode + "' and " + FieldName + " like '" + txt_book_search + "%' order by len(acc_no),acc_no";//and attachment = '" + nonmatname + "' and acc_no not in (select acc_no from nonbookmat where lib_code='" + nonlbcode + "')
                }
            }
            else
            {
                if (Searchbook == "" || Searchbook == "All")
                {
                    sqlqry = "select acc_no,title,author,publisher,edition from bookdetails where lib_code='" + nonlbcode + "'  and acc_no like '" + txt_boaccno.Text + "%' order by len(acc_no),acc_no";//and attachment = '" + nonmatname + "' and acc_no  not in (select isnull(acc_no,'') from nonbookmat where  lib_code='" + nonlbcode + "') 
                }
                else
                {
                    if (txt_book_search.Text == "")
                        sqlqry = "select acc_no,title,author,publisher,edition from bookdetails where acc_no like '" + txt_boaccno.Text + "%' and lib_code='" + nonlbcode + "'  order by len(acc_no),acc_no";//and attachment = '" + nonmatname + "' and acc_no not in (select isnull(acc_no,'') from nonbookmat where lib_code='" + nonlbcode + "')
                    else
                        sqlqry = "select acc_no,title,author,publisher,edition from bookdetails where " + FieldName + " like '" + txt_book_search + "%' and acc_no like '" + txt_boaccno.Text + "%' and lib_code='" + nonlbcode + "' and acc_no like '" + txt_boaccno.Text + "%' order by len(acc_no),acc_no";//and attachment = '" + nonmatname + "' and acc_no not in (select isnull(acc_no,'') from nonbookmat where lib_code='" + nonlbcode + "') 
                }
            }
            dsbook.Clear();
            dsbook = d2.select_method_wo_parameter(sqlqry, "Text");
            if (dsbook.Tables.Count > 0 && dsbook.Tables[0].Rows.Count > 0)
            {
                DataTable dtBkaccno = new DataTable();
                DataRow drow;

                dtBkaccno.Columns.Add("Access Code", typeof(string));
                dtBkaccno.Columns.Add("Title", typeof(string));
                dtBkaccno.Columns.Add("Author", typeof(string));
                dtBkaccno.Columns.Add("Publisher", typeof(string));
                dtBkaccno.Columns.Add("Edition", typeof(string));

                for (int row = 0; row < dsbook.Tables[0].Rows.Count; row++)
                {
                    drow = dtBkaccno.NewRow();
                    drow["Access Code"] = Convert.ToString(dsbook.Tables[0].Rows[row]["acc_no"]);
                    drow["Title"] = Convert.ToString(dsbook.Tables[0].Rows[row]["title"]);
                    drow["Author"] = Convert.ToString(dsbook.Tables[0].Rows[row]["author"]);
                    drow["Publisher"] = Convert.ToString(dsbook.Tables[0].Rows[row]["publisher"]);
                    drow["Edition"] = Convert.ToString(dsbook.Tables[0].Rows[row]["edition"]);
                    dtBkaccno.Rows.Add(drow);
                }
                divBkAccNo.Visible = true;
                GrdBkAccNo.DataSource = dtBkaccno;
                GrdBkAccNo.DataBind();
                GrdBkAccNo.Visible = true;
                Cache.Remove("BBcacheKey");
                btn_book_exit.Visible = true;
            }
            else
            {
                divBkAccNo.Visible = false;
                GrdBkAccNo.Visible = false;
                btn_pop2exit.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Records Found";
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    protected void GrdBkAccNo_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , HiddenField3.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void GrdBkAccNo_SelectedIndexChanged(Object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.HiddenField2.Value);
        string title = Convert.ToString(GrdBkAccNo.Rows[rowIndex].Cells[2].Text);
        txtitle.Text = title;
        txtitle.Enabled = false;
        string journalAccno = Convert.ToString(GrdBkAccNo.Rows[rowIndex].Cells[1].Text);
        txtbook_accno.Text = journalAccno;
        string journaltitle = Convert.ToString(GrdBkAccNo.Rows[rowIndex].Cells[3].Text);
        txauthor.Text = journaltitle;
        txauthor.Enabled = false;
        string volumeno = Convert.ToString(GrdBkAccNo.Rows[rowIndex].Cells[4].Text);
        txpublish.Text = volumeno;
        txpublish.Enabled = false;
        DivBookAccessNo.Visible = false;
    }

    protected void btn_book_ok_exit(object sender, EventArgs e)
    {
        DivBookAccessNo.Visible = false;
    }

    protected void image_DivBookAccessNoclose_Click(object sender, EventArgs e)
    {
        DivBookAccessNo.Visible = false;
    }

    protected void GrdBkAccNo_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        GrdBkAccNo.PageIndex = e.NewPageIndex;
        btn_book_go_Click(sender, e);
    }


    #endregion

    protected void ddDepart_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddl_monYear_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddcurrency_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void dd_sts_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    #region plusminusMaterial
    protected void btn_pls_mat_Click(object sender, EventArgs e)
    {
        try
        {

            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Material Name";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    protected void btn_min_mat_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_mat.Items.Count > 0)
                g1 = Convert.ToString(ddl_mat.Items[0].Text);
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (g1 != "")
            {
                string get = d2.GetFunction("select count(Attachment) Attachment from bookdetails where Attachment='" + g1 + "' and  Lib_Code ='" + libcode + "'");
                int getcnt = Convert.ToInt32(get);
                if (getcnt > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Alredy Books Available in this Attachment.So it Cannot be deleted.";
                    return;

                }
                else
                {
                    LoadAttache();
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Attachment";
                return;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    #endregion

    #region plusminusCurrencyType
    protected void btn_pl_currn_Click(object sender, EventArgs e)
    {
        try
        {

            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Currency";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    protected void btn_min_currn_Click(object sender, EventArgs e)
    {
        try
        {

            if (ddcurrency.Items.Count > 0)
                g1 = Convert.ToString(ddcurrency.Items[0].Text);
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (g1 != "")
            {
                string get = d2.GetFunction("select count(cur_name) cur_name from bookdetails where cur_name='" + g1 + "' and  Lib_Code ='" + libcode + "'");
                int getcnt = Convert.ToInt32(get);
                if (getcnt > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Alredy Books Available in this Currency Type.So it Cannot be deleted.";
                    return;

                }
                else
                {
                    GetCurrencyType();
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Currency Type";
                return;
            }


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    #endregion

    #region No_Of_Copy_Radio_Nonbook
    protected void rbl_non_Single_Selected(object sender, EventArgs e)
    {
        try
        {
            txcopy.Visible = false;
            rbl_non_mul.Checked = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    protected void rbl_non_mul_Selected(object sender, EventArgs e)
    {
        try
        {
            rbl_non_Single.Checked = false;
            txcopy.Visible = true;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }


    #endregion


    #region Save_Non_Book
    protected void btn_save_Non_book_Click(object sender, EventArgs e)
    {
        try
        {

            //txt_price.Col = 1
            //txt_price.Row = 1
            //txt_price.CellType = CellTypeNumber
            string lbcode = "";
            string matname = "";
            string sqlnonbooksave = "";
            string currntype = "";
            string jaccno = "";
            string Baccno = "";
            string status = "";
            string Depart = "";
            string sqlsave = "";
            int insert = 0;
            if (ddl_Library.Items.Count > 0)
                lbcode = Convert.ToString(ddl_Library.SelectedValue);
            if (ddl_mat.Items.Count > 0)
                matname = Convert.ToString(ddl_mat.SelectedItem.Text);
            if (ddcurrency.Items.Count > 0)
                currntype = Convert.ToString(ddcurrency.SelectedItem.Text);
            if (dd_sts.Items.Count > 0)
                status = Convert.ToString(dd_sts.SelectedItem.Text);
            if (ddDepart.Items.Count > 0)
                Depart = Convert.ToString(ddDepart.SelectedItem.Text);
            string time = DateTime.Now.ToString("hh:mm tt");
            if (lbcode == "")
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Library";
                return;
            }
            else if (txacc.Text == "")
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Enter the Access No";
                return;
            }
            else if (matname == "")
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Select Material Name";
                return;
            }
            else if (currntype == "")
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please select Currency Type";
                return;
            }
            else if (textarea_contentpart.InnerText == "")
            {

                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Enter Content of Attachement";
                return;
            }

            string Accdate1 = Convert.ToString(txt_date_acc.Text);
            string[] adate1 = Accdate1.Split('/');
            if (adate1.Length == 3)
                accessdate = adate1[2].ToString() + "-" + adate1[1].ToString() + "-" + adate1[0].ToString();
            string monyear = ddl_monYear.Text + '-' + txtyear.Text;
            string Attcount = d2.GetFunction("select  count(attachment_name) as attachment_name from attachment where attachment_name='" + matname + "'");
            int acnt = Convert.ToInt32(Attcount);
            string currncount = d2.GetFunction("select  count(currency_type) as currency_type from currency_convertion where currency_type='" + currntype + "'");
            int cuncnt = Convert.ToInt32(currncount);
            if (acnt <= 0)
                sqlnonbooksave = "insert into attachment(attachment_name,lib_code) values('" + matname + "','" + lbcode + "')";
            else if (cuncnt <= 0)
                sqlnonbooksave = "insert into currency_convertion(currency_type) values( '" + currntype + "')";
            insert = d2.update_method_wo_parameter(sqlnonbooksave, "Text");
            if (txttolprice.Text != "")
            {
                sqlsave = "update nonbookmat set issue_flag = '" + status + "', access_date = '" + accessdate + "',access_time = '" + time + "',title = '" + txtitle.Text + "',author = '" + txauthor.Text + "',publisher = '" + txpublish.Text + "',volume = '" + txtvol.Text + "',isbn = '" + txtisbn.Text + "',runing_time = '" + txt_time.Text + "',contents = '" + textarea_contentpart.InnerText + "',attachment = '" + matname + "',price = " + txttolprice.Text + " ,currency_type = '" + currntype + "' , currency_value = '" + txcurrval.Text + "',department='" + Depart + "',mon_year='" + monyear + "',newaccno='" + txtbook_accno.Text + "', issue_no='" + txtissueno.Text + "' where nonbookmat_no = '" + txacc.Text + "' and lib_code = '" + lbcode + "'";
                insert = d2.update_method_wo_parameter(sqlsave, "Text");

            }
            if (insert > 0)
            {

                alertpopwindow.Visible = true;
                lblalerterr.Text = "Saved SuccessFully!";
                nonbookclear();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }


    public void nonbookclear()
    {
        try
        {
            txacc.Text = "";
            txtitle.Text = "";
            txauthor.Text = "";
            txpublish.Text = "";
            txtvol.Text = "";
            txtisbn.Text = "";
            txttolprice.Text = "";
            txcurrval.Text = "";
            txtbook_accno.Text = "";
            txtyear.Text = "";
            txtissueno.Text = "";
            txacc.Text = "";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    #endregion

    protected void btn_Exit_Non_book_Click(object sender, EventArgs e)
    {
        DivNonBookpopup.Visible = false;

    }
    #endregion

    #region Book_AddDetails

    public void loadthridAuthor()
    {
        try
        {
            ddl_thridAuthor.Items.Clear();
            if (ddl_txt_lib.Items.Count > 0)
                libcode = Convert.ToString(ddl_txt_lib.SelectedValue);
            string qrycurrentype = "Select distinct (ltrim(rtrim(thi_author))) thi_author from bookdetails where ltrim(rtrim(thi_author)) != 'Others' and (ltrim(rtrim(thi_author)) is not null  or ltrim(rtrim(thi_author)) ='' or ltrim(rtrim(thi_author))='-') and lib_code='" + libcode + "' order by ltrim(rtrim(thi_author)) ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrycurrentype, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_thridAuthor.DataSource = ds;
                ddl_thridAuthor.DataTextField = "thi_author";
                ddl_thridAuthor.DataValueField = "thi_author";
                ddl_thridAuthor.DataBind();

            }
        }
        catch
        {

        }
    }

    public void loadinward()
    {
        try
        {

            ddl_inward_type.Items.Clear();
            ddl_inward_type.Items.Add("Purchased");
            ddl_inward_type.Items.Add("Donated");
            ddl_inward_type.Items.Add("Specimen Copy");

        }
        catch
        {

        }
    }

    protected void ddl_thridAuthor_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddl_inward_type_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    #region Ok_And_Exit

    protected void btn_Ok_Add_details_Click(object sender, EventArgs e)
    {
        AlreadyLoad = "Y";
        DivAddDetailsBookPopup.Visible = false;
        btn_Save_Click(sender, e);
    }

    protected void btn_Ex_Add_details_Click(object sender, EventArgs e)
    {
        DivAddDetailsBookPopup.Visible = false;
    }

    #endregion

    #endregion

    #region Question_Bank_Add_Details

    protected void btn_Ok_Add_Qus_details_Click(object sender, EventArgs e)
    {

    }

    protected void btn_Ex_Add_Qus_details_Click(object sender, EventArgs e)
    {
        Div_Question_Bank_popup.Visible = false;
    }

    #endregion

    #region Status_Popup
    protected void ddlcollege_sts_SelectedIndexChanged(object sender, EventArgs e)
    {
        RackFpSpread.Visible = false;
        rptprint.Visible = false;
    }
    protected void ddllibrary_sts_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            RackFpSpread.Visible = false;
            rptprint.Visible = false;
            LoadRack();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    protected void ddlrack_sts_SelectedIndexChanged(object sender, EventArgs e)
    {
        RackFpSpread.Visible = false;
        rptprint.Visible = false;

    }
    protected void btn_sts_Rack_Go_Click(object sender, EventArgs e)
    {
        try
        {
            ds = getrackstatus();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                rackloadspread(ds);
            }
            else
            {

                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Records Found";

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    #region Fspread
    private DataSet getrackstatus()
    {

        DataSet dsload1 = new DataSet();
        try
        {
            #region get Value
            string collegecode = string.Empty;
            string stalibcode = "";
            string statrack = "";


            if (ddlstat_college.Items.Count > 0)
                collegecode = Convert.ToString(ddlstat_college.SelectedValue);
            if (ddllibrary_sts.Items.Count > 0)
                stalibcode = Convert.ToString(ddllibrary_sts.SelectedValue);
            if (ddlsts_rackno.Items.Count > 0)
                statrack = Convert.ToString(ddlsts_rackno.SelectedValue);
            string selQ = string.Empty;
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(stalibcode))
            {
                if (statrack == "" || statrack == "All")
                    selQ = "select distinct rack_no,lib_code  from rack_master where rack_master.lib_code='" + stalibcode + "'";
                else
                    selQ = "select distinct rack_no,lib_code  from rack_master where rack_No='" + statrack + "' and rack_master.lib_code='" + stalibcode + "'";

            }
            dsload1.Clear();
            dsload1 = d2.select_method_wo_parameter(selQ, "Text");
            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


        return dsload1;
    }

    public void rackloadspread(DataSet dsrack)
    {
        try
        {
            DataSet dsrackrow = new DataSet();
            DataSet dscat = new DataSet();
            string categ = "";
            string categ1 = "";
            string acopies = "";
            string maxcap = "";
            RackFpSpread.SaveChanges();
            if (dsrack.Tables.Count > 0 && dsrack.Tables[0].Rows.Count > 0)
            {
                RackFpSpread.Sheets[0].RowCount = 0;
                RackFpSpread.CommandBar.Visible = false;
                RackFpSpread.Sheets[0].AutoPostBack = true;
                RackFpSpread.Sheets[0].ColumnHeader.RowCount = 1;
                RackFpSpread.Sheets[0].ColumnHeader.Columns.Count = 0;
                RackFpSpread.Sheets[0].RowHeader.Visible = false;
                //FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                //darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                //darkstyle.ForeColor = Color.White;

                for (int col1 = 0; col1 < dsrack.Tables[0].Rows.Count; col1++)
                {
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, cncessionColCnt].Text = "Allot";
                    RackFpSpread.Sheets[0].ColumnHeader.Columns.Count++;
                    string racknum = Convert.ToString(dsrack.Tables[0].Rows[col1]["rack_no"]);
                    RackFpSpread.Sheets[0].ColumnHeader.Cells[0, col1].Text = racknum;
                    RackFpSpread.Sheets[0].ColumnHeader.Cells[0, col1].HorizontalAlign = HorizontalAlign.Center;
                    RackFpSpread.Sheets[0].ColumnHeader.Cells[0, col1].BackColor = ColorTranslator.FromHtml("brown");
                }
                int c = 0;
                for (int row = 0; row < dsrack.Tables[0].Rows.Count; row++)
                {
                    string racknum = Convert.ToString(dsrack.Tables[0].Rows[row]["rack_no"]);
                    string licode = Convert.ToString(dsrack.Tables[0].Rows[row]["lib_code"]);
                    int col = row;

                    if (racknum != "")
                    {
                        string getrackqry = "SELECT distinct row_no,lib_code,rack_no from rackrow_master where rack_no='" + racknum + "' and lib_code='" + licode + "' order by row_no ";
                        dsrackrow.Clear();
                        dsrackrow = d2.select_method_wo_parameter(getrackqry, "Text");
                        if (dsrackrow.Tables[0].Rows.Count > 0)
                        {

                            for (int i = 0; i < dsrackrow.Tables[0].Rows.Count; i++)
                            {
                                RackFpSpread.Sheets[0].RowCount++;
                                string rano = Convert.ToString(dsrackrow.Tables[0].Rows[i]["rack_no"]);
                                string rono = Convert.ToString(dsrackrow.Tables[0].Rows[i]["row_no"]);
                                string lcode = Convert.ToString(dsrackrow.Tables[0].Rows[i]["lib_code"]);
                                acopies = d2.GetFunction("select no_of_copies from rackrow_master where lib_code='" + lcode + "' and rack_no='" + rano + "' and row_no ='" + rono + "'");
                                maxcap = d2.GetFunction("select max_capacity from rackrow_master where lib_code='" + lcode + "' and rack_no='" + rano + "' and row_no ='" + rono + "'");
                                int acopies1 = Convert.ToInt32(acopies);
                                int maxcap1 = Convert.ToInt32(maxcap);
                                if (acopies1 == maxcap1)
                                {
                                    RackFpSpread.Sheets[0].Cells[RackFpSpread.Sheets[0].RowCount - 1, c].BackColor = ColorTranslator.FromHtml("Purple");
                                    // fpSpread3.BackColor = &HC0C0FF

                                }
                                else if (acopies1 > 0)
                                {
                                    RackFpSpread.Sheets[0].Cells[RackFpSpread.Sheets[0].RowCount - 1, c].BackColor = ColorTranslator.FromHtml("Green");
                                }
                                else
                                {
                                    RackFpSpread.Sheets[0].Cells[RackFpSpread.Sheets[0].RowCount - 1, c].BackColor = ColorTranslator.FromHtml("Yellow");
                                }
                                string nooftitle1 = d2.GetFunction("select count(acc_no) from rack_allocation where lib_code='" + lcode + "' and rack_no='" + rano + "' and row_no ='" + rono + "'");
                                string cat = "select cat from libcat where lib_code='" + lcode + "' and rno='" + rano + "' and sno ='" + rono + "'";
                                dscat.Clear();
                                dscat = d2.select_method_wo_parameter(cat, "Text");
                                if (dscat.Tables[0].Rows.Count > 0)
                                {
                                    for (int j = 0; j < dscat.Tables[0].Rows.Count; j++)
                                    {
                                        string cat1 = Convert.ToString(dscat.Tables[0].Rows[j]["cat"]);
                                        if (cat1 != "")
                                        {
                                            if (categ == "")
                                                categ = cat1;
                                            else
                                                categ = categ + "," + cat1;
                                        }
                                    }

                                }
                                string status = "SH-" + racknum + "AVAIL-" + acopies + "TOT-" + maxcap + "IM-" + categ + "";

                                string status1 = rano + "," + rono;

                                RackFpSpread.Sheets[0].Cells[RackFpSpread.Sheets[0].RowCount - 1, c].Tag = status1;
                                RackFpSpread.Sheets[0].Cells[RackFpSpread.Sheets[0].RowCount - 1, c].Text = status;
                                RackFpSpread.Sheets[0].Cells[RackFpSpread.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                                //RackFpSpread.Sheets[0].Cells[RackFpSpread.Sheets[0].RowCount, i].BackColor = ColorTranslator.FromHtml("brown"); 
                            }

                        }
                    }
                    c = c + 1;

                }
                RackFpSpread.Sheets[0].PageSize = RackFpSpread.Sheets[0].RowCount;
                RackFpSpread.SaveChanges();
                RackFpSpread.Visible = true;
                rptprint.Visible = true;
            }
        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }



    #endregion

    #region Print

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Rack Status Report";
            string pagename = "Inward_Entry.aspx";
            Printcontrol.loadspreaddetails(RackFpSpread, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(RackFpSpread, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }
    #endregion


    public void RackFpSpread_CellClick(object sender, EventArgs e)
    {
        try
        {

            Cellclick = true;



        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }


    public void RackFpSpread_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == true)
            {

                RackFpSpread.SaveChanges();
                string activerow = "";
                string activecol = "";
                int arow = 0;
                int acol = 0;

                string libname = "";
                DataSet dsgetupdatebook = new DataSet();
                activerow = RackFpSpread.ActiveSheetView.ActiveRow.ToString();
                activecol = RackFpSpread.ActiveSheetView.ActiveColumn.ToString();
                arow = Convert.ToInt32(activerow);
                acol = Convert.ToInt32(activecol);
                if (ddllibrary_sts.Items.Count > 0)
                {
                    libname = Convert.ToString(ddllibrary_sts.SelectedItem.Text);
                    libcode = Convert.ToString(ddllibrary_sts.SelectedValue);
                }
                if (activerow.Trim() != "")
                {
                    string getra = Convert.ToString(RackFpSpread.Sheets[0].Cells[arow, acol].Tag);
                    if (getra != "")
                    {
                        string[] rash = getra.Split(',');
                        string getsql = " select bookdetails.acc_no,title ,author from bookdetails  left join rack_allocation on (bookdetails.acc_no=rack_allocation.acc_no and rack_allocation.lib_code=bookdetails.lib_code) where  bookdetails.lib_code='" + libcode + "'  and rack_no='" + rash[0] + "' and row_no='" + rash[1] + "' and (rack_allocation.book_type='BOK' or  rack_allocation.book_type='REF') ";

                        dsgetupdatebook.Clear();
                        dsgetupdatebook = d2.select_method_wo_parameter(getsql, "Text");

                        FpSpread3.Sheets[0].RowCount = 0;
                        FpSpread3.Sheets[0].ColumnCount = 4;
                        FpSpread3.CommandBar.Visible = false;
                        FpSpread3.Sheets[0].AutoPostBack = true;
                        FpSpread3.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread3.Sheets[0].RowHeader.Visible = false;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.Black;
                        FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].Columns[0].Locked = true;
                        FpSpread3.Columns[0].Width = 50;


                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Access No";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Columns[1].Width = 80;
                        FpSpread3.Columns[1].Visible = true;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Title";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Columns[2].Width = 100;
                        FpSpread3.Columns[2].Visible = true;

                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Author";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread3.Columns[3].Width = 100;
                        FpSpread3.Columns[3].Visible = true;
                        if (dsgetupdatebook.Tables.Count > 0 && dsgetupdatebook.Tables[0].Rows.Count > 0)
                        {
                            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
                            int sno = 0;
                            for (int row = 0; row < dsgetupdatebook.Tables[0].Rows.Count; row++)
                            {
                                FpSpread3.Sheets[0].RowCount++;
                                sno++;

                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].CellType = txtCell;

                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[row]["acc_no"]).Trim();
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[row]["title"]).Trim();
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[row]["author"]).Trim();

                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Locked = true;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Locked = true;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Locked = true;
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Locked = true;

                            }

                        }
                        FpSpread3.SaveChanges();
                        FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
                        FpSpread3.Height = 350;
                        FpSpread3.Width = 800;
                        Divfspreadstatus.Visible = true;
                        Fieldset8.Visible = true;
                        FpSpread3.Visible = true;
                        Buttonexit.Visible = true;
                    }
                }
                else
                {
                    Divfspreadstatus.Visible = false;

                }
            }
        }


        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    protected void btn_popclose5_Click(object sender, EventArgs e)
    {
        Divfspreadstatus.Visible = false;
        Fieldset2.Visible = false;

    }

    protected void Buttonexit_Click(object sender, EventArgs e)
    {
        Divfspreadstatus.Visible = false;
        Fieldset2.Visible = false;

    }

    #endregion

    #region call_num_popup

    protected void btn_popupDes_OnClick(object sender, EventArgs e)
    {
        try
        {
            DivcallDes.Visible = true;
            txt_callno_calldes.Visible = false;
            Search_callNo();
            btn_call_go_Click(sender, e);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    public void Search_callNo()
    {
        try
        {
            ddl_call.Items.Clear();
            ddl_call.Items.Add("All");
            ddl_call.Items.Add("Call Number");
            ddl_call.Items.Add("Call Number Description");

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void ddl_call_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_call.SelectedIndex == 0)
                txt_callno_calldes.Visible = false;
            else if (ddl_call.SelectedIndex == 1)
                txt_callno_calldes.Visible = true;
            else if (ddl_call.SelectedIndex == 2)
                txt_callno_calldes.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void btn_call_go_Click(object sender, EventArgs e)
    {
        try
        {
            string serachcall = "";
            string sql1 = "";
            string txtsearch = "";
            DataSet dscall = new DataSet();
            if (ddl_call.Items.Count > 0)
                serachcall = Convert.ToString(ddl_call.SelectedValue);
            if (txt_callno_calldes.Text != "")
                txtsearch = txt_callno_calldes.Text;

            if (serachcall == "All")
                sql1 = "select distinct CallNo,CallNoDescription,Callnoid from CallNoEntry where CallNo<>'' order by CallNo,CallNoDescription";
            else
            {
                if (serachcall == "Call Number")
                    sql1 = "select distinct CallNo,CallNoDescription,Callnoid from CallNoEntry where CallNo='" + txtsearch + "'";
                else
                    sql1 = "select distinct CallNo,CallNoDescription,Callnoid from CallNoEntry where CallNoDescription='" + txtsearch + "'";
            }
            dscall.Clear();
            dscall = d2.select_method_wo_parameter(sql1, "Text");
            DataTable dtCallNo = new DataTable();
            DataRow drow;
            if (dscall.Tables.Count > 0 && dscall.Tables[0].Rows.Count > 0)
            {
                dtCallNo.Columns.Add("Call No", typeof(string));
                dtCallNo.Columns.Add("Callnoid", typeof(string));
                dtCallNo.Columns.Add("Call Des", typeof(string));

                for (int callrow = 0; callrow < dscall.Tables[0].Rows.Count; callrow++)
                {
                    drow = dtCallNo.NewRow();
                    drow["Call No"] = Convert.ToString(dscall.Tables[0].Rows[callrow]["CallNo"]).Trim();
                    drow["Callnoid"] = Convert.ToString(dscall.Tables[0].Rows[callrow]["Callnoid"]).Trim();
                    drow["Call Des"] = Convert.ToString(dscall.Tables[0].Rows[callrow]["CallNoDescription"]).Trim();
                    dtCallNo.Rows.Add(drow);
                }
                GrdCallNo.DataSource = dtCallNo;
                GrdCallNo.DataBind();
                GrdCallNo.Visible = true;
                btn_add_call.Visible = true;
                //btn_ok_call.Visible = true;
                btn_exit_call.Visible = true;

            }
            else
            {
                GrdCallNo.Visible = false;
                btn_pop2exit.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Records Found";
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    //public void FpSpread2_CellClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Cellclick = true;
    //        DivCallAdd.Visible = true;
    //        btn_call_Update.Visible = true;
    //        btn_call_save.Visible = false; ;
    //    }
    //    catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    //}

    //public void FpSpread2_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Cellclick == true)
    //        {
    //            string activerow = "";
    //            activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
    //            if (activerow.Trim() != "")
    //            {
    //                string CallID = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);

    //                string sql = "select * from CallNoEntry where CallNoid='" + CallID + "' ";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(sql, "text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    callno_txt.Text = Convert.ToString(ds.Tables[0].Rows[0]["CallNo"]);
    //                    calldes_txt.Text = Convert.ToString(ds.Tables[0].Rows[0]["CallNoDescription"]);

    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    //}

    #region Add

    protected void btn_add_call_Click(object sender, EventArgs e)
    {
        callno_txt.Text = "";
        calldes_txt.Text = "";
        DivCallAdd.Visible = true;
    }

    protected void btn_call_save_Click(object sender, EventArgs e)
    {
        try
        {
            string savecallno = "";
            string getcallno = "";
            string getcalldes = "";
            string calnoid = "";
            bool flag = false;
            int save = 0;
            DataSet dscallsve = new DataSet();

            if (callno_txt.Text == "")
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Enter the Call Number";
                return;
            }
            else if (calldes_txt.Text == "")
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Enter the Call Number Description";
                return;
            }
            if (callno_txt.Text != "" && calldes_txt.Text != "")
            {
                savecallno = "select CallNo,callnodescription from CallNoEntry";
                dscallsve.Clear();
                dscallsve = d2.select_method_wo_parameter(savecallno, "Text");
                if (dscallsve.Tables[0].Rows.Count > 0)
                {
                    for (int r = 0; r < dscallsve.Tables[0].Rows.Count; r++)
                    {
                        getcallno = Convert.ToString(dscallsve.Tables[0].Rows[r]["CallNo"]);
                        getcalldes = Convert.ToString(dscallsve.Tables[0].Rows[r]["callnodescription"]);
                        if (getcallno == callno_txt.Text && getcalldes == calldes_txt.Text)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "CallNo & Description already exist";
                            return;
                            flag = true;
                        }
                    }
                }
                if (flag == true)
                {
                    return;
                }
                else
                {
                    string insert = "insert into CallNoEntry(CallNo,CallNoDescription)values('" + callno_txt.Text + "','" + calldes_txt.Text + "')";
                    save = d2.update_method_wo_parameter(insert, "Text");
                }
            }
            if (save > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Saved SuccessFully!";
                callno_txt.Text = "";
                calldes_txt.Text = "";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void btn_call_Update_Click(object sender, EventArgs e)
    {
        try
        {
            //string savecallno = "";
            //string getcallno = "";
            //string getcalldes = "";
            //string calnoid = "";
            //int save = 0;
            //DataSet dscallsve = new DataSet();
            //FpSpread2.SaveChanges();
            //activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
            //activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
            //if (activerow != "" || activerow != "0")
            //    calnoid = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
            //if (callno_txt.Text == "")
            //{
            //    alertpopwindow.Visible = true;
            //    lblalerterr.Text = "Enter the Call Number";
            //    return;
            //}
            //else if (calldes_txt.Text == "")
            //{
            //    alertpopwindow.Visible = true;
            //    lblalerterr.Text = "Enter the Call Number Description";
            //    return;
            //}
            //if (callno_txt.Text != "" && calldes_txt.Text != "")
            //{
            //    savecallno = "select CallNo,callnodescription from CallNoEntry where CallNoId <> '" + calnoid + "'";
            //    dscallsve.Clear();
            //    dscallsve = d2.select_method_wo_parameter(savecallno, "Text");
            //    if (dscallsve.Tables[0].Rows.Count > 0)
            //    {
            //        for (int r = 0; r < dscallsve.Tables[0].Rows.Count; r++)
            //        {
            //            getcallno = Convert.ToString(dscallsve.Tables[0].Rows[r]["CallNo"]);
            //            getcalldes = Convert.ToString(dscallsve.Tables[0].Rows[r]["callnodescription"]);
            //            if (getcallno == callno_txt.Text && getcalldes == calldes_txt.Text)
            //            {
            //                alertpopwindow.Visible = true;
            //                lblalerterr.Text = "CallNo & Description already exist";
            //                return;
            //            }
            //        }
            //    }

            //    string insert = "if exists(select CallNo,callnodescription from CallNoEntry where CallNoId <> '" + calnoid + "' )update CallNoEntry set CallNo='" + callno_txt.Text + "',CallNoDescription='" + calldes_txt.Text + "' where CallnoId='" + calnoid + "' else insert into CallNoEntry(CallNo,CallNoDescription)values('" + callno_txt.Text + "','" + calldes_txt.Text + "')";
            //    save = d2.update_method_wo_parameter(insert, "Text");

            //}
            //if (save > 0)
            //{
            //    alertpopwindow.Visible = true;
            //    lblalerterr.Text = "Updated SuccessFully!";
            //}
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    #region DeleteCall

    protected void btn_call_delete_Click(object sender, EventArgs e)
    {
        try
        {
            string qry = "";
            if (callno_txt.Text == "" && calldes_txt.Text == "")
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record To Delete!";
                return;
            }
            else
            {
                qry = d2.GetFunction("select Callnoid from CallNoEntry where CallNo='" + callno_txt.Text + "' and callnodescription='" + calldes_txt.Text + "'");
                if (qry != "" && qry != "0")
                {
                    surediv.Visible = true;
                    lbl_sure.Text = "Do you want to Delete this Record?";
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        try
        {
            string delqry = "";
            int del = 0;
            if (callno_txt.Text == "" && calldes_txt.Text == "")
                delqry = "delete from CallNoEntry where CallNo='" + callno_txt.Text + "' and callnodescription='" + calldes_txt.Text + "'";
            del = d2.update_method_wo_parameter(delqry, "Text");
            if (del > 0)
                alertpopwindow.Visible = true;
            lblalerterr.Text = "Record successfully Deleted";


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }
    }

    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        try
        {

            surediv.Visible = false;
            alertpopwindow.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    #endregion

    protected void btn_call_exit_Click(object sender, EventArgs e)
    {

        DivCallAdd.Visible = false;
    }

    #endregion

    //protected void btn_ok_call_Click(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        //FpSpread2.SaveChanges();
    //        //activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
    //        //activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
    //        //string caldes = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
    //        //ddl_Description.Items[0].Text = caldes;
    //        //DivcallDes.Visible = false;
    //        //ddl_Description_SelectedIndexChanged(sender, e);

    //    }
    //    catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    //}

    protected void btn_exit_call_Click(object sender, EventArgs e)
    {
        DivcallDes.Visible = false;

    }

    #endregion

    #region close_click

    protected void btn_popclose_Click(object sender, EventArgs e)
    {
        popview.Visible = false;
        ddl_CallNo.Items.Clear();
        chkGridSelectAll.Visible = false;
    }

    protected void btn_DivNonBookpopup_popclose_Click(object sender, EventArgs e)
    {
        DivNonBookpopup.Visible = false;
    }

    protected void btn_DivadddetailsBookpopup_popclose_Click(object sender, EventArgs e)
    {
        DivAddDetailsBookPopup.Visible = false;
    }

    protected void btn_Question_Bank_popup_Click(object sender, EventArgs e)
    {
        DivStatus.Visible = false;

    }

    protected void btn_newspaper_popup_Click(object sender, EventArgs e)
    {
        DivStatus.Visible = false;
        divnews_pop.Visible = false;

    }

    protected void btn_callDes_popup_Click(object sender, EventArgs e)
    {
        DivcallDes.Visible = false;

    }

    protected void btn_call_add_popup_Click(object sender, EventArgs e)
    {
        DivCallAdd.Visible = false;

    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
        Auto_AccessNo();
    }

    #endregion

    #region Add_And_Exit

    protected void btn_addgroup_Click(object sender, EventArgs e)
    {
        try
        {
            string group = Convert.ToString(txt_addgroup.Text);
            string insert = "";
            int addins = 0;
            libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (group != "")
            {

                if (lbl_addgroup.Text.Trim() == "Category")
                {
                    int j = ddl_Category.Items.Count;
                    ddl_Category.Items.Insert(j, group);
                }
                else if (lbl_addgroup.Text.Trim() == "Book Type")
                {
                    int j = ddl_Category.Items.Count;
                    ddl_Budget.Items.Insert(j, group);
                }
                else if (lbl_addgroup.Text.Trim() == "Currency")
                {
                    int j = ddl_Category.Items.Count;
                    ddl_curren.Items.Insert(j, group);
                }
                else if (lbl_addgroup.Text.Trim() == "Status")
                {
                    int j = ddl_Category.Items.Count;
                    ddl_status.Items.Insert(j, group);
                }
                else if (lbl_addgroup.Text.Trim() == "Attachement")
                {
                    int j = ddl_Category.Items.Count;
                    ddl_atta.Items.Insert(j, group);
                }
                else if (lbl_addgroup.Text.Trim() == "Language")
                {
                    int j = ddl_Category.Items.Count;
                    ddl_language.Items.Insert(j, group);
                }
                else if (lbl_addgroup.Text.Trim() == "Publication Place")
                {
                    int j = ddl_Category.Items.Count;
                    ddl_publishplace.Items.Insert(j, group);
                }
                else if (lbl_addgroup.Text.Trim() == "Book Type")
                {
                    int j = ddl_Category.Items.Count;
                    ddl_booktype.Items.Insert(j, group);
                }
                else if (lbl_addgroup.Text.Trim() == "Call No")
                {
                    int j = ddl_Category.Items.Count;
                    ddl_CallNo.Items.Insert(j, group);
                }
                else if (lbl_addgroup.Text.Trim() == "Material Name")
                {
                    int j = ddl_Category.Items.Count;
                    ddl_mat.Items.Insert(j, group);
                }
                else if (lbl_addgroup.Text.Trim() == "Thrid Author")
                {
                    int j = ddl_Category.Items.Count;
                    ddl_thridAuthor.Items.Insert(j, group);
                }
                plusdiv.Visible = false;
            }
            else
            {
                plusdiv.Visible = true;
                lblerror.Visible = true;
                lblerror.Text = "Please Enter the " + lbl_addgroup.Text + "";
            }

        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    protected void btn_exitaddgroup_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addgroup.Visible = false;
        txt_addgroup.Text = "";
    }

    #endregion

    #region Yes_Or_No

    protected void btn_yes1_Click(object sender, EventArgs e)
    {
        if (ddl_entrytype.SelectedIndex == 0)
        {
            DivAddDetailsBookPopup.Visible = true;
            Div_Question_Bank_popup.Visible = false;
            AlreadyLoad = "Y";

            AddtionalDetailPopup.Visible = false;
        }
        else if (ddl_entrytype.SelectedIndex == 1)
            DivAddDetailsBookPopup.Visible = false;
        else
        {
            DivAddDetailsBookPopup.Visible = false;
            Div_Question_Bank_popup.Visible = true;
            AlreadyLoad = "Y";
        }
    }

    protected void btn_no1_Click(object sender, EventArgs e)
    {

        DivAddDetailsBookPopup.Visible = false;
        Div_Question_Bank_popup.Visible = false;
        AddtionalDetailPopup.Visible = false;
        AlreadyLoad = "Y";
        btn_Save_Click(sender, e);
    }

    #endregion

    #region New_yes_No
    protected void bt_yes_Click(object sender, EventArgs e)
    {
        string status = "";
        int papercnt = 0;
        int newsv = 0;
        if (ddl_status.Items.Count > 0)
            status = Convert.ToString(ddl_status.SelectedItem.Text);
        string Curtdate = DateTime.Now.ToString("MM/dd/yyyy");
        if (ddllibrary1.Items.Count > 0)
            libcode = Convert.ToString(ddllibrary1.SelectedValue);
        string var_papername = TextBox2.Text;
        string savenews = "insert into news_paper (serial_no,cur_date,title,noofcopies,price,Total,lib_code) values ('" + TextBox1.Text + "','" + Curtdate + "','" + var_papername + "','" + TextBox3.Text + "','" + TextBox4.Text + "','" + TextBox5.Text + "','" + libcode + "')";
        newsv = d2.update_method_wo_parameter(savenews, "Text");
        if (rackno != "")
        {
            int newcopy = Convert.ToInt32(txt_newcopy.Text);
            string savrack = "insert into rack_allocation(lib_code,rack_no,row_no,acc_no,access_date,access_time,book_type) values('" + libcode + "','" + rackno + "','" + shelfno + "','" + txt_accno.Text + "','" + Curtdate + "','" + Acctime + "','NP')";
            int save1 = d2.update_method_wo_parameter(savrack, "Text");
            string sqrack = "update rackrow_master set no_of_copies  = no_of_copies+'" + newcopy + "' where rack_no = '" + rackno + "' and  row_no = '" + shelfno + "' and lib_code = '" + libcode + "'";
            sqrack += "update rack_master set no_of_copies = no_of_copies +'" + newcopy + "' where lib_code  = '" + libcode + "' and rack_no = '" + rackno + "'";
            newsv = d2.update_method_wo_parameter(savrack, "Text");

        }
        if (newsv > 0)
        {
            if (ViewState["bookimage"] != "0" && ViewState["size"] != "")
            {
                byte[] photoid = (byte[])(ViewState["bookimage"]);
                int size = Convert.ToInt32(ViewState["size"]);
                bookphotosave(txt_accno.Text, libcode, Book_Type, size, photoid);

            }
            alertpopwindow.Visible = true;
            Divnewspopup.Visible = false;
            Div4.Visible = false;
            lblalerterr.Text = "News Paper Details Saved Successfully";


        }
    }

    protected void bt_no_Click(object sender, EventArgs e)
    {
        Div4.Visible = false;
        Divnewspopup.Visible = true;
    }
    #endregion

    #region Clear

    public void clear()
    {
        try
        {
            txt_copy.Text = "";
            txt_accno.Text = "";
            txt_title.Text = "";
            txt_depart.Text = "";
            txt_author.Text = "";
            Txt_SedAuthor.Text = "";
            Txt_pub.Text = "";
            txt_curval.Text = "";
            txt_Offer.Text = "";
            txt_Price.Text = "";
            Txt_edit.Text = "";
            txt_publisyear.Text = "";
            txt_isbn_No.Text = "";
            txt_billno.Text = "";
            txt_bookSz.Text = "";
            txt_remarks.Text = "";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    #endregion

    # region questionpopbind
    #region loagRackno1
    public void loagRack()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            string qryrack = "select distinct rack_no from Rack_master  where  lib_code='" + libcode + "'  order by rack_no";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qryrack, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlrack.DataSource = ds;
                ddlrack.DataTextField = "rack_no";
                ddlrack.DataValueField = "rack_no";
                ddlrack.DataBind();

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    #endregion

    #region Shelf1
    public void loadshelff()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (ddl_Rack.Items.Count > 0)
                rackno = Convert.ToString(ddl_Rack.SelectedItem.Text);
            string qryshelf = "SELECT distinct row_no,len(row_no) from rackrow_master where rack_no='" + rackno + "' and lib_code='" + libcode + "' order by len(row_no),row_no";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qryshelf, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlself.DataSource = ds;
                ddlself.DataTextField = "row_no";
                ddlself.DataValueField = "row_no";
                ddlself.DataBind();

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    #endregion

    #region  loadposition1

    public void loadposition1()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (ddlrack.Items.Count > 0)
                rackno = Convert.ToString(ddlrack.SelectedItem.Text);
            if (ddlself.Items.Count > 0)
                shelfno = Convert.ToString(ddlself.SelectedItem.Text);
            string qryposition = "SELECT distinct Pos_No from RowPos_Master WHERE Rack_No ='" + rackno + "' AND Row_No ='" + shelfno + "' AND Lib_Code ='" + libcode + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qryposition, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlposition.DataSource = ds;
                ddlposition.DataTextField = "Pos_No";
                ddlposition.DataValueField = "Pos_No";
                ddlposition.DataBind();

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    #endregion

    #region LoadPosPlace1
    public void LoadPosPlace1()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (ddlrack.Items.Count > 0)
                rackno = Convert.ToString(ddlrack.SelectedItem.Text);
            if (ddlself.Items.Count > 0)
                shelfno = Convert.ToString(ddlself.SelectedItem.Text);
            if (ddlposition.Items.Count > 0)
                posno = Convert.ToString(ddlposition.SelectedItem.Text);
            string qrypositionplace = "SELECT ISNULL(Max_Capacity,0) Max_Capacity FROM RowPos_Master WHERE Rack_No ='" + rackno + "' AND Row_No ='" + shelfno + "' AND Pos_No='" + posno + "' AND Lib_Code ='" + libcode + "'  ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrypositionplace, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlplacepos.DataSource = ds;
                ddlplacepos.DataTextField = "Max_Capacity";
                ddlplacepos.DataValueField = "Max_Capacity";
                ddlplacepos.DataBind();

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    #endregion

    protected void ddlrack_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void ddlself_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void ddlposition_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void ddlplacepos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    #endregion

    # region newspaperpopbind
    #region loagRackno2
    public void loagRack2()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            string qryrack = "select distinct rack_no from Rack_master  where  lib_code='" + libcode + "'  order by rack_no";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qryrack, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DropDownList1.DataSource = ds;
                DropDownList1.DataTextField = "rack_no";
                DropDownList1.DataValueField = "rack_no";
                DropDownList1.DataBind();

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    #endregion

    #region Shelf2
    public void loadshelff2()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (DropDownList1.Items.Count > 0)
                rackno = Convert.ToString(DropDownList1.SelectedItem.Text);
            string qryshelf = "SELECT distinct row_no,len(row_no) from rackrow_master where rack_no='" + rackno + "' and lib_code='" + libcode + "' order by len(row_no),row_no";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qryshelf, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DropDownList2.DataSource = ds;
                DropDownList2.DataTextField = "row_no";
                DropDownList2.DataValueField = "row_no";
                DropDownList2.DataBind();

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    #endregion

    #region  loadposition2

    public void loadposition2()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (DropDownList1.Items.Count > 0)
                rackno = Convert.ToString(DropDownList1.SelectedItem.Text);
            if (DropDownList2.Items.Count > 0)
                shelfno = Convert.ToString(DropDownList2.SelectedItem.Text);
            string qryposition = "SELECT distinct Pos_No from RowPos_Master WHERE Rack_No ='" + rackno + "' AND Row_No ='" + shelfno + "' AND Lib_Code ='" + libcode + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qryposition, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DropDownList3.DataSource = ds;
                DropDownList3.DataTextField = "Pos_No";
                DropDownList3.DataValueField = "Pos_No";
                DropDownList3.DataBind();

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }



    }
    #endregion

    #region LoadPosPlace2
    public void LoadPosPlace2()
    {
        try
        {
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (DropDownList1.Items.Count > 0)
                rackno = Convert.ToString(DropDownList1.SelectedItem.Text);
            if (DropDownList2.Items.Count > 0)
                shelfno = Convert.ToString(DropDownList2.SelectedItem.Text);
            if (DropDownList3.Items.Count > 0)
                posno = Convert.ToString(DropDownList3.SelectedItem.Text);
            string qrypositionplace = "SELECT ISNULL(Max_Capacity,0) Max_Capacity FROM RowPos_Master WHERE Rack_No ='" + rackno + "' AND Row_No ='" + shelfno + "' AND Pos_No='" + posno + "' AND Lib_Code ='" + libcode + "'  ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrypositionplace, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DropDownList4.DataSource = ds;
                DropDownList4.DataTextField = "Max_Capacity";
                DropDownList4.DataValueField = "Max_Capacity";
                DropDownList4.DataBind();

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    #endregion

    #region Library
    public void Library1()
    {
        try
        {
            ddllibrary1.Items.Clear();
            ds.Clear();
            string College = ddlCollege.SelectedValue.ToString();
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("CollegeCode", Convert.ToString(College));
                ds = storeAcc.selectDataSet("[GetLibrary]", dicQueryParameter);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary1.DataSource = ds;
                    ddllibrary1.DataTextField = "lib_name";
                    ddllibrary1.DataValueField = "lib_code";
                    ddllibrary1.DataBind();




                    // ddllibrary.Items.Insert(0, "All");
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }




    }






    #endregion

    protected void ddlrack2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void ddlself2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }

    protected void ddlposition2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    protected void ddlplacepos2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }


    }
    #endregion

    protected void btnquessave1_Click(object sender, EventArgs e)
    {
        try
        {
            string status = "";
            if (btn_Save.ImageUrl == "~/LibImages/save.jpg")
            {
                if (ddl_status.Items.Count > 0)
                    status = Convert.ToString(ddl_status.SelectedItem.Text);
                string Curtdate = DateTime.Now.ToString("MM/dd/yyyy");
                string qusave = "INSERT INTO university_question(access_code,Title,dept,semester,sem_month,sem_year,regulation,affiliationuniversity,paper_name) values('" + txtQusPaper.Text + "','" + txtQueTitle.Text + "','" + txtQusDept.Text + "','" + Text_sem.Text + "','" + Text_mon.Text + "','" + Text_year.Text + "','" + txtQusRegu.Text + "','" + txtQusAffUni.Text + "','" + txpagename.Text + "')";

                int quesave = d2.update_method_wo_parameter(qusave, "Text");
                if (rackno != "")
                {
                    string saverackno = "insert into rack_allocation(lib_code,rack_no,row_no,acc_no,access_date,access_time,book_type) values('" + libcode + "','" + rackno + "','" + shelfno +
"','" + txt_accno.Text + "', '" + Curtdate + "','" + Acctime + "','QB')";
                    saverackno += "update rackrow_master set no_of_copies  = no_of_copies +1 where  rack_no = '" + rackno + "' and  row_no = '" + shelfno + "' and lib_code = '" + libcode + "'";
                    saverackno += "update rack_master set no_of_copies = no_of_copies +1 where lib_code  = '" + libcode + "' and rack_no = '" + rackno + "'";
                    quesave = d2.update_method_wo_parameter(saverackno, "Text");
                }
                if (quesave > 0)
                {
                    if (ViewState["bookimage"] != "0" && ViewState["size"] != "")
                    {
                        byte[] photoid = (byte[])(ViewState["bookimage"]);
                        int size = Convert.ToInt32(ViewState["size"]);
                        bookphotosave(txt_accno.Text, libcode, Book_Type, size, photoid);
                    }
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Question Paper Saved Successfully";
                }

            }
            else
            {
                Ques_Update();
            }
        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    protected void btnnewssave_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_Save.ImageUrl == "~/LibImages/save.jpg")
            {
                string status = "";
                int papercnt = 0;
                int newsv = 0;
                if (ddl_status.Items.Count > 0)
                    status = Convert.ToString(ddl_status.SelectedItem.Text);
                if (ddllibrary1.Items.Count > 0)
                    libcode = Convert.ToString(ddllibrary1.SelectedValue);
                string Curtdate = DateTime.Now.ToString("MM/dd/yyyy");
                string var_papername = txt_title.Text;
                string newcnt = d2.GetFunction("select Count(*) from newspaper where paper_name='" + var_papername + "'");
                if (newcnt == "" && newcnt == "0")
                {
                    string papcode = d2.GetFunction("select count(*),max(paper_code) from newspaper");
                    if (papcode != "")
                    {
                        papercnt = Convert.ToInt32(papcode);
                        papercnt = papercnt + 1;
                    }
                    else
                    {
                        papercnt = 1;
                    }
                    string newsave = "INSERT INTO newspaper(paper_name,Noofcopies,price,Total,Languages,Suppliername,address,place,Lib_Code) values('" + TextBox2.Text + "','" + TextBox3.Text + "','" + TextBox4.Text + "','" + TextBox5.Text + "','" + TextBox6.Text + "','" + TextBox7.Text + "','" + TextBox8.Text + "','" + TextBox9.Text + "','" + libcode + "')";
                    newsv = d2.update_method_wo_parameter(newsave, "Text");
                }
                string attcnt = "if not exists(select * from attachment where attachment_name='" + Attache + "')insert into attachment(attachment_name,lib_code) values('" + Attache + "','" + libcode + "')";
                newsv = d2.update_method_wo_parameter(attcnt, "Text");
                string getqry = d2.GetFunction("select count(*) from news_paper where cur_date ='" + Curtdate + "' and title = '" + var_papername + "' and Lib_code='" + libcode + "'");
                if (getqry == "" && getqry == "0")
                {

                }
                else
                {
                    Divnewspopup.Visible = true;
                    lbl_news_msg.Text = "This Paper Entry Already Made in this Date? Do You Want to Continue";
                    return;
                }

            }
        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    protected void btnnewsok_Click(object sender, EventArgs e)
    {
        try
        {

        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    protected void btnnewsexit_Click(object sender, EventArgs e)
    {
        try
        {
            divnews_pop.Visible = false;
        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "InwardEntry"); }

    }

    protected void TextBox4_OnTextChanged(object sender, EventArgs e)
    {
        double noofco = 0;
        double pric = 0;
        double tot = 0;
        noofco = Convert.ToDouble(TextBox3.Text);
        pric = Convert.ToDouble(TextBox4.Text);
        tot = noofco * pric;


        TextBox5.Text = Convert.ToString(tot);


    }

    public void getLibPrivil()
    {
        try
        {
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddlCollege.SelectedValue);
            string sql = "";
            string GrpUserVal = "";
            string GrpCode = "";
            string LibCollection = "";
            Hashtable hsLibcode = new Hashtable();
            if (singleUser.ToLower() == "true")
            {
                sql = "SELECT DISTINCT lib_code from lib_privileges where user_code=" + userCode + " and lib_code in (select lib_code from library where college_code=" + coll_Code + ")";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "text");
            }
            else
            {
                string[] groupUser = groupUserCode.Split(';');
                if (groupUser.Length > 0)
                {
                    if (groupUser.Length == 1)
                    {
                        sql = "SELECT DISTINCT lib_code from lib_privileges where group_code=" + groupUser[0] + "";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sql, "text");
                    }
                    if (groupUser.Length > 1)
                    {
                        for (int i = 0; i < groupUser.Length; i++)
                        {
                            GrpUserVal = groupUser[i];
                            if (!GrpCode.Contains(GrpUserVal))
                            {
                                if (GrpCode == "")
                                    GrpCode = GrpUserVal;
                                else
                                    GrpCode = GrpCode + "','" + GrpUserVal;
                            }
                        }
                        sql = "SELECT DISTINCT lib_code from lib_privileges where group_code in ('" + GrpCode + "')";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sql, "text");
                    }
                }

            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                libcodecollection = "WHERE lib_code IN (-1)";
                goto aa;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string codeCollection = Convert.ToString(ds.Tables[0].Rows[i]["lib_code"]);
                    if (!hsLibcode.Contains(codeCollection))
                    {
                        hsLibcode.Add(codeCollection, "LibCode");
                        if (libcodecollection == "")
                            libcodecollection = codeCollection;
                        else
                            libcodecollection = libcodecollection + "','" + codeCollection;
                    }
                }
            }
            //libcodecollection = Left(libcodecollection, Len(libcodecollection) - 1);
            libcodecollection = "WHERE lib_code IN ('" + libcodecollection + "')";
        aa:
            LibCollection = libcodecollection;

            Library(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    public void Auto_AccessNo()
    {
        try
        {
            string codeno = string.Empty;
            string codeno1 = string.Empty;
            string libcodeval = Convert.ToString(ddl_txt_lib.SelectedValue);
            string category = "";
            if (ddl_Category.Items.Count > 0)
            {
                category = Convert.ToString(ddl_Category.SelectedItem.Text);
            }

            DataSet dsAutoAccess = new DataSet();
            DataSet dsBack = new DataSet();

            string sql = "SELECT ISNULL(AutoAccessNo,0) AutoAccessNo,ISNULL(gen_acr,'') gen_acr,ISNULL(gen_stno,1) gen_stno FROM Library Where Lib_Code ='" + libcodeval + "'";
            dsAutoAccess = da.select_method_wo_parameter(sql, "text");
            if (dsAutoAccess.Tables[0].Rows.Count > 0)
            {
                string book = Convert.ToString(dsAutoAccess.Tables[0].Rows[0]["AutoAccessNo"]);
                string bookAcr = Convert.ToString(dsAutoAccess.Tables[0].Rows[0]["gen_acr"]);
                if (book == "1")
                {
                    if (!string.IsNullOrEmpty(bookAcr))
                    {
                        sql = "SELECT * FROM bookdetails WHERE Lib_Code ='" + libcodeval + "' ORDER BY LEN(acc_no),acc_no";
                        dsBack.Clear();
                        dsBack = da.select_method_wo_parameter(sql, "text");
                        if (dsBack.Tables[0].Rows.Count > 0)
                        {
                            codeno = Convert.ToString(dsBack.Tables[0].Rows[dsBack.Tables[0].Rows.Count - 1]["acc_no"]);
                            string str = "";
                            for (int k = 0; k < codeno.Length; k++)
                            {
                                string a = Convert.ToString(codeno.ElementAt<char>(k));
                                if (a.All(char.IsNumber))
                                {
                                    str = str + a;
                                }
                            }
                            int jj = Convert.ToInt32(str) + 1;
                            //int BkAccJJ = (Convert.ToString(jj).Length) == 1 ? (Convert.ToInt32("0" + jj)) : jj;
                            int BkAccJJ = 0;
                            string StrBkAcc = Convert.ToString(jj);
                            if (StrBkAcc.Length == 1)
                                StrBkAcc = "0" + jj;
                            else
                                StrBkAcc = Convert.ToString(jj);
                            codeno1 = Convert.ToString(dsAutoAccess.Tables[0].Rows[0]["gen_acr"]) + StrBkAcc;
                            txt_accno.Text = codeno1;
                        }
                        else
                        {
                            codeno1 = Convert.ToString(dsAutoAccess.Tables[0].Rows[0]["gen_acr"]) + Convert.ToString(dsAutoAccess.Tables[0].Rows[0]["gen_stno"]);
                            txt_accno.Text = codeno1;
                        }
                    }
                    else
                    {
                        sql = "SELECT COUNT(*) as count FROM bookdetails WHERE Lib_Code ='" + libcodeval + "' ";
                        dsBack.Clear();
                        dsBack = da.select_method_wo_parameter(sql, "text");
                        if (dsBack.Tables[0].Rows.Count > 0)
                        {
                            int countVal = Convert.ToInt32(dsBack.Tables[0].Rows[0]["count"]) + 1;
                            txt_accno.Text = Convert.ToString(countVal);
                        }
                    }
                }
                else
                {
                    txt_accno.Text = "";
                }
            }
            else
            {
                txt_accno.Text = "";
            }
        }
        catch (Exception)
        {
        }
    }

    public void NBMAutoAccno()
    {
        try
        {
            string codeno = "";
            string codeno1 = "";

            DataSet rs2 = new DataSet();
            DataSet rs3 = new DataSet();
            string nonlibcode = "";
            string sqlnonqry = "";
            //if (ddl_Library.Items.Count > 0)
            nonlibcode = Convert.ToString(ddl_txt_lib.SelectedValue);
            sqlnonqry = "SELECT ISNULL(NonBookMaterial,0) NonBookMaterial,ISNULL(NM_Acr,'') NM_Acr,ISNULL(NM_StNo,1) NM_StNo FROM Library Where Lib_Code ='" + nonlibcode + "'";
            rs2.Clear();
            rs2 = d2.select_method_wo_parameter(sqlnonqry, "Text");
            if (rs2.Tables[0].Rows.Count > 0)
            {
                string nomat = Convert.ToString(rs2.Tables[0].Rows[0]["NonBookMaterial"]);
                string ordcode = Convert.ToString(rs2.Tables[0].Rows[0]["NM_Acr"]);
                if (nomat.ToUpper() == "TRUE")
                {
                    txacc.Enabled = false;
                    string nonbookaccno = "SELECT * FROM NonBookMat WHERE Lib_Code ='" + nonlibcode + "' ORDER BY LEN(NonBookMat_No),NonBookMat_No";
                    rs3.Clear();
                    rs3 = d2.select_method_wo_parameter(nonbookaccno, "Text");
                    if (rs3.Tables[0].Rows.Count > 0)
                    {
                        codeno = Convert.ToString(rs3.Tables[0].Rows[rs3.Tables[0].Rows.Count - 1]["NonBookMat_No"]);
                        string str = "";
                        for (int k = 0; k < codeno.Length; k++)
                        {
                            string a = Convert.ToString(codeno.ElementAt<char>(k));
                            if (a.All(char.IsNumber))
                            {
                                str = str + a;
                            }
                        }
                        int jj = Convert.ToInt32(str) + 1;
                        codeno1 = Convert.ToString(rs2.Tables[0].Rows[0]["NM_Acr"]) + jj;
                        txacc.Text = codeno1;
                        txacc.Enabled = false;
                    }
                    else
                    {
                        codeno1 = Convert.ToString(rs2.Tables[0].Rows[0]["NM_Acr"]) + Convert.ToString(rs2.Tables[0].Rows[0]["NM_StNo"]);
                        txacc.Text = codeno1;
                        txacc.Enabled = false;
                    }
                }
            }
            else
            {
                txacc.Enabled = true;
            }

        }
        catch
        {

        }

    }

    protected void grdInward_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ddltype.Text == "Books")
            {
                e.Row.Cells[5].Visible = false;
            }
            else if (ddltype.Text == "News Paper")
            {
                e.Row.Cells[4].Visible = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (ddltype.Text == "Books")
            {
                e.Row.Cells[5].Visible = false;
            }
            else if (ddltype.Text == "News Paper")
            {
                e.Row.Cells[4].Visible = false;
            }

        }
        //if (e.Row.RowIndex == 0)
        //{
        //    e.Row.Cells[0].Text = "Select";
        //}

    }

    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void SelectedIndexChanged(Object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

        loagRackno();
        loadposition();
        loadthridAuthor();
        loadinward();
        string activerow = "";
        string type = "";
        string libname = "";
        string getupdatebookqry = "";
        DataSet dsgetupdatebook = new DataSet();
        if (ddllibrary.Items.Count > 0)
        {
            libname = Convert.ToString(ddllibrary.SelectedItem.Text);
            libcode = Convert.ToString(ddllibrary.SelectedValue);
        }
        if (ddltype.Items.Count > 0)
            type = Convert.ToString(ddltype.SelectedValue);

        if (Convert.ToString(rowIndex) != "" && Convert.ToString(selectedCellIndex) != "1")
        {
            popview.Visible = true;
            btn_Save.Visible = true;
            btn_Save.ImageUrl = "~/LibImages/update.jpg";
            btn_Delete.Visible = true;
            Page.MaintainScrollPositionOnPostBack = true;
            if (type == "Books")
            {
                string acnum = Convert.ToString(grdInward.Rows[rowIndex].Cells[2].Text);
                string bid = Convert.ToString(grdInward.Rows[rowIndex].Cells[5].Text);
                string getRackNo = d2.GetFunction("select rack_no from rack_allocation where acc_no='" + acnum + "'");
                if (getRackNo != "" && getRackNo != "0")
                {
                    ddl_Rack.SelectedIndex = ddl_Rack.Items.IndexOf(ddl_Rack.Items.FindByValue(Convert.ToString(getRackNo)));
                }
                loadshelf();
                string getShelfNo = d2.GetFunction("select row_no from rack_allocation where acc_no='" + acnum + "'");
                if (getShelfNo != "" || getShelfNo != "0")
                {
                    ddl_shelf.SelectedIndex = ddl_shelf.Items.IndexOf(ddl_shelf.Items.FindByValue(Convert.ToString(getShelfNo)));
                }
                getupdatebookqry = "select * from bookdetails where acc_no='" + acnum + "' and lib_code='" + libcode + "' and bookid ='" + bid + "'";
                dsgetupdatebook.Clear();
                dsgetupdatebook = d2.select_method_wo_parameter(getupdatebookqry, "Text");
                if (dsgetupdatebook.Tables[0].Rows.Count > 0)
                {
                    ddl_txt_lib.Items[0].Text = libname;
                    txt_accno.Text = acnum;
                    txt_Offer.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["b_discount"]);
                    txt_title.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["title"]);
                    txt_author.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["author"]);
                    Txt_pub.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["publisher"]);
                    Txt_sub.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["supplier"]);
                    Txt_edit.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["edition"]);
                    txvo.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["volume"]);
                    txtpart.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["part"]);
                    txt_publisyear.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["pur_year"]);
                    txt_Price.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["price"]);
                    txt_billno.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["bill_no"]);
                    Txtbilldate.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["bill_date"]);
                    ddl_CallNo.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["call_no"]));
                    ddl_Description.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["call_des"]));
                    string yesorno = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["ref"]);
                    if (yesorno == "Yes")
                        rbl_yes.Checked = true;
                    else
                        rbl_no.Checked = true;
                    txt_depart.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["dept_code"]);
                    txt_remarks.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["remark"]);
                    txt_date_acc.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["access_date"]);
                    Txt_SedAuthor.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["sec_author"]);
                    ddl_thridAuthor.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["thi_author"]));
                    txcoll.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["collabrator"]);
                    txt_bookSz.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["book_size"]);
                    txtbose.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["book_series"]);
                    txt_isbn_No.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["isbn"]);

                    txtboselect.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["book_selected_by"]);
                    txtboacc.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["book_accesed_by"]);
                    ddl_inward_type.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["pur_don"]));
                    ddl_status.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["book_status"]));
                    ddl_atta.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["attachment"]));
                    txttopics.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["topics"]);
                    txvolti.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["volumetitle"]);
                    txsubtitle.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["subtitle"]);
                    txt_curval.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["cur_value"]);
                    ddl_curren.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["cur_name"]));
                    txvolpr.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["vol_price"]);
                    Txt_sub.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["subject"]);
                    ddl_booktype.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["typeofbook"]));
                    txkey1.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["key1"]);
                    txkey2.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["key2"]);
                    txkey3.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["key3"]);
                    ddl_Category.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["category"]));
                    ddl_language.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["language"]));
                }
            }
            else if (type == "News Paper")
            {
                string serialno = Convert.ToString(grdInward.Rows[rowIndex].Cells[4].Text);
                getupdatebookqry = "select * from news_paper where serial_no='" + serialno + "' and Lib_code='" + libcode + "'";
                getupdatebookqry += "select * from rack_allocation where acc_no='" + serialno + "'";
                dsgetupdatebook.Clear();
                dsgetupdatebook = d2.select_method_wo_parameter(getupdatebookqry, "Text");
                if (dsgetupdatebook.Tables.Count > 0 && dsgetupdatebook.Tables[0].Rows.Count > 0)
                {
                    txt_accno.Text = serialno;
                    txt_Price.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["price"]);
                    txt_title.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["title"]);
                    ddl_atta.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["attachment"]));
                    txt_newcopy.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["noofcopies"]);
                    if (dsgetupdatebook.Tables[1].Rows.Count > 0)
                    {
                        ddl_Rack.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[1].Rows[0]["rack_no"]));
                        ddl_shelf.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[1].Rows[0]["row_no"]));
                    }
                }
            }
            else
            {
                string accessno = Convert.ToString(grdInward.Rows[rowIndex].Cells[2].Text);
                getupdatebookqry = " select access_code,title,paper_name,degree_code,semester,sem_month,sem_year,remarks,issue_flag,isnull(dept,'') dept,ISNULL(Price,'') Price from university_question where access_code = '" + accessno + "' and lib_code = '" + libcode + "'";
                getupdatebookqry += "select * from rack_allocation where acc_no='" + accessno + "'";
                dsgetupdatebook.Clear();
                dsgetupdatebook = d2.select_method_wo_parameter(getupdatebookqry, "Text");
                if (dsgetupdatebook.Tables.Count > 0 && dsgetupdatebook.Tables[0].Rows.Count > 0)
                {
                    txt_accno.Text = accessno;
                    txt_Price.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["price"]);
                    txt_depart.Text = Convert.ToString(dsgetupdatebook.Tables[0].Rows[0]["dept"]);
                    if (dsgetupdatebook.Tables[1].Rows.Count > 0)
                    {
                        ddl_Rack.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[1].Rows[0]["rack_no"]));
                        ddl_shelf.Items.Insert(0, Convert.ToString(dsgetupdatebook.Tables[1].Rows[0]["row_no"]));
                    }
                }
            }
        }
    }

    protected void GrdCallNo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[2].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[2].Visible = false;
        }
    }

    protected void GrdCallNo_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void GrdCallNo_SelectedIndexChanged(Object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        string caldes = Convert.ToString(GrdCallNo.Rows[rowIndex].Cells[2].Text);
        ddl_Description.Items[0].Text = caldes;
        DivcallDes.Visible = false;
        ddl_Description_SelectedIndexChanged(sender, e);
    }

    protected void txt_Offer_OnTextChanged(object sender, EventArgs e)
    {
        double CurVal = Convert.ToDouble(txt_curval.Text);
        double discount = Convert.ToDouble(txt_Offer.Text);
        double price = CurVal - (CurVal * (discount / 100));
        txt_Price.Text = Convert.ToString(price);
    }

    protected void BtnClear_OnClick(object sender, EventArgs e)
    {
        clear();
        ddlsearcbook.Visible = false;
        string Libraryname = "";
        if (ddllibrary.Items.Count > 0)
            Libraryname = Convert.ToString(ddllibrary.SelectedItem.Text);
        if (ddllibrary.Items.Count > 0)
            libcodes = Convert.ToString(ddllibrary.SelectedValue);
        if (!string.IsNullOrEmpty(Libraryname))
        {
            popview.Visible = true;
            btn_Save.Visible = true;
            btn_Save.ImageUrl = "~/LibImages/save.jpg";
            btn_Delete.Visible = false;
            //ddl_txt_lib.Text = Libraryname;
            Type();
            loadcategory();
            Auto_AccessNo();
            //LoadLanguage();
            Categoryclick();
            LoadBudHead();
            loadinward();
            loagRackno();
            loadshelf();
            loadposition();
            LoadPosPlace();
            GetCurrencyType();
            LoadStatus();
            LoadAttache();
            LoadLang();
            Loadcalldes();
            LoadPubPlace();
            LoadBookType();
            loadlan();
            loadthridAuthor();
            IsNonBook.Checked = false;
            lblnonbook.Visible = false;
            txtnonbook.Visible = false;
            TxtMultiple.Visible = false;
            txt_date_acc.Attributes.Add("readonly", "readonly");
            txt_date_acc.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Txtbilldate.Attributes.Add("readonly", "readonly");
            Txtbilldate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Txtbilldate.Enabled = false;
            txt_Offer.Text = "";
            txt_title.Text = "";
            txt_author.Text = "";
            Txt_pub.Text = "";
            Txt_sub.Text = "";
            Txt_edit.Text = "";
            txvo.Text = "";
            txtpart.Text = "";
            txt_billno.Text = "";
            Txtbilldate.Text = "";
            ddl_CallNo.Text = "";
            txt_depart.Text = "";
            txt_remarks.Text = "";
        }
    }

}





