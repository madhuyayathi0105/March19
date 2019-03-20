using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Data;
using System.Collections;
using System.Configuration;

public partial class LibraryMod_nonbookmaterial : System.Web.UI.Page
{
    # region fielddeclaration
    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    DataTable dtCommon = new DataTable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    ReuasableMethods rs = new ReuasableMethods();
    Hashtable columnhash = new Hashtable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string g1 = "";
    int demin = 0;
    bool check = false;
    string delete = "";
    string libcode = string.Empty;
    string status = string.Empty;

    DataSet nonbookmat = new DataSet();
    string bookaccess = string.Empty;
    string jouraccess = string.Empty;

    static string searchlibcode = string.Empty;
    static int searchby1 = 0;
    //Pageno Added by rajasekar 02/07/2018
    Boolean pageno = false;
    int ivalue = 0;
    int curpage = 0;
    double pageSize1 = 0.0;
    int pagecnt = 0;
    int pgsize = 0;
    int selectedpage = 0;
    static int first = 0;
    //***********//
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
                Bindcollege();
                getLibPrivil();
                searchby();
                first = 1;
            }

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearchaccess(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> values = new List<string>();



        if (searchlibcode != "All")
            query = "SELECT DISTINCT  TOP  100 nonbookmat_no FROM nonbookmat where nonbookmat_no Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by nonbookmat_no";
        else
            query = "SELECT DISTINCT  TOP  100 nonbookmat_no FROM nonbookmat where nonbookmat_no Like '" + prefixText + "%' order by nonbookmat_no";





        values = ws.Getname(query);
        return values;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearch(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> values = new List<string>();

        if (searchby1 == 1)
        {

            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 title FROM nonbookmat where title Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by title";
            else
                query = "SELECT DISTINCT  TOP  100 title FROM nonbookmat where title Like '" + prefixText + "%' order by title";
        }
        else if (searchby1 == 2)
        {
            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 author FROM nonbookmat where author Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by author";
            else
                query = "SELECT DISTINCT  TOP  100 author FROM nonbookmat where author Like '" + prefixText + "%' order by author";
        }
        else if (searchby1 == 3)
        {
            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 publisher FROM nonbookmat where publisher Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by publisher";
            else
                query = "SELECT DISTINCT  TOP  100 publisher FROM nonbookmat where publisher Like '" + prefixText + "%' order by publisher";
        }
        else if (searchby1 == 5)
        {
            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 newaccno FROM nonbookmat where newaccno Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by newaccno";
            else
                query = "SELECT DISTINCT  TOP  100 newaccno FROM nonbookmat where newaccno Like '" + prefixText + "%' order by newaccno";
        }
        else if (searchby1 == 7)
        {
            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 attachment FROM nonbookmat where attachment Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by attachment";
            else
                query = "SELECT DISTINCT  TOP  100 attachment FROM nonbookmat where attachment Like '" + prefixText + "%' order by attachment";
        }
        values = ws.Getname(query);
        return values;
    }

    #region college
    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();

            dtCommon.Clear();
            ddlCollege.Enabled = false;
            DataSet dsprint = new DataSet();
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
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }
    #endregion

    #region Library

    public void BindLibrary(string LibCode)
    {
        try
        {
            ddlLibrary.Items.Clear();
            ds.Clear();
            // string College = ddlCollege.SelectedValue.ToString();
            string strquery = "SELECT Lib_Code,Lib_Name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) FROM Library " + LibCode + " and College_Code ='" + userCollegeCode + "' ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlLibrary.DataSource = ds;
                ddlLibrary.DataTextField = "Lib_Name";
                ddlLibrary.DataValueField = "Lib_Code";
                ddlLibrary.DataBind();
                ddlLibrary.Items.Add("All");

                searchlibcode = Convert.ToString(ddlLibrary.SelectedValue);
            }
        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    #endregion

    #region NonLibrary1

    public void BindLibrary1()
    {
        try
        {
            ddl_Library.Items.Clear();
            ds.Clear();
            string libcode = string.Empty;
            // string College = ddlCollege.SelectedValue.ToString();
            if (ddlLibrary.Items.Count > 0)
            {
                for (int i = 0; i < ddlLibrary.Items.Count - 1; i++)
                {
                    if (Convert.ToString(ddlLibrary.SelectedItem) == "All")
                    {
                        if (libcode == "")
                        {
                            libcode = "" + ddlLibrary.Items[i + 1].Value + "";
                        }
                        else
                        {
                            libcode = libcode + "'" + "," + "'" + ddlLibrary.Items[i + 1].Value + "";
                        }
                    }
                    else
                        libcode = ddlLibrary.SelectedValue;
                }
            }
            string strquery1 = "SELECT Lib_Code,Lib_Name FROM Library where College_Code ='" + userCollegeCode + "' and lib_code in('" + libcode + "') ORDER BY Lib_Name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Library.DataSource = ds;
                ddl_Library.DataTextField = "Lib_Name";
                ddl_Library.DataValueField = "Lib_Code";
                ddl_Library.DataBind();
                // ddl_Library.Items.Insert(0, "All");
            }
        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    #endregion

    #region searchby
    public void searchby()
    {
        try
        {
            ddlsearchby.Items.Add("All");
            ddlsearchby.Items.Add("Title");
            ddlsearchby.Items.Add("Author");
            ddlsearchby.Items.Add("Publisher");
            ddlsearchby.Items.Add("Datewise");
            ddlsearchby.Items.Add("Book Access Code");
            ddlsearchby.Items.Add("Department");
            ddlsearchby.Items.Add("Material Name");
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }
    #endregion

    #region dept

    public void Binddept()
    {
        try
        {

            ds.Clear();
            string library = ddlLibrary.SelectedValue.ToString();

            string strqur = "select distinct(dept_name) from journal_dept where dept_name <> '' and college_code ='" + userCollegeCode + "'";
            if (library != "All")
            {
                strqur += " AND Lib_Code='" + library + "'";
            }
            strqur = strqur + " ORDER BY Dept_name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strqur, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldept.DataSource = ds;
                ddldept.DataTextField = "Dept_name";
                ddldept.DataValueField = "Dept_name";
                ddldept.DataBind();

            }
            ddldept.Items.Insert(0, "All");
        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    #endregion

    #region Load_Attachement

    public void LoadAttache()
    {
        if (ddl_Library.Items.Count > 0)
            libcode = Convert.ToString(ddl_Library.SelectedValue);
        string qrycurrentype = "select distinct attachment from  nonbookmat where Lib_Code ='" + libcode + "' ";
        ds.Clear();
        ds = d2.select_method_wo_parameter(qrycurrentype, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_mat.DataSource = ds;
            ddl_mat.DataTextField = "attachment";
            ddl_mat.DataValueField = "attachment";
            ddl_mat.DataBind();
        }
    }

    #endregion

    #region CurrencyType
    public void GetCurrencyType()
    {

        try
        {
            if (ddl_Library.Items.Count > 0)
                libcode = Convert.ToString(ddl_Library.SelectedValue);
            string qrycurrentype = "SELECT DISTINCT ISNULL(currency_type,'') currency_type FROM currency_convertion";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrycurrentype, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddcurrency.DataSource = ds;
                ddcurrency.DataTextField = "currency_type";
                ddcurrency.DataValueField = "currency_type";
                ddcurrency.DataBind();



            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }
    #endregion

    #region LoadBudHead
    public void LoadBudHead()
    {
        try
        {
            if (ddl_Library.Items.Count > 0)
                libcode = Convert.ToString(ddl_Library.SelectedValue);
            string qrycallno = "Select TextVal,textcode from textvaltable where  TextCriteria='LBHed' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrycallno, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddl_non_budget.DataSource = ds;
                ddl_non_budget.DataTextField = "TextVal";
                ddl_non_budget.DataValueField = "textcode";
                ddl_non_budget.DataBind();


            }

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
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
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }

    }
    #endregion

    #region Loaddepatment
    public void loaddepartment()
    {
        try
        {

            ddDepart.Items.Clear();
            if (ddl_Library.Items.Count > 0)
                libcode = Convert.ToString(ddl_Library.SelectedValue);
            string qrybook = "SELECT DISTINCT  ISNULL(Dept_code,'') Dept_code FROM BookDetails WHERE  Lib_Code ='" + libcode + "' order by Dept_code ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qrybook, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddDepart.DataSource = ds;
                ddDepart.DataTextField = "Dept_code";
                ddDepart.DataValueField = "Dept_code";
                ddDepart.DataBind();


            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }

    }
    #endregion

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
        catch (Exception ex) { }
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divtable.Visible = false;
            print2.Visible = false;
            getLibPrivil();
            ddllibrary_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }

    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divtable.Visible = false;
            print2.Visible = false;
            searchlibcode = Convert.ToString(ddlLibrary.SelectedValue);
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    protected void ddlsearchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divtable.Visible = false;
            print2.Visible = false;
            if (ddlsearchby.SelectedIndex == 0)
            {
                txtsearch.Visible = false;
                lbl_fromdate.Visible = false;
                txt_fromdate.Visible = false;
                lbl_todate.Visible = false;
                txt_todate.Visible = false;
                ddldept.Visible = false;
            }
            if (ddlsearchby.SelectedIndex == 1)
            {
                txtsearch.Visible = true;
                lbl_fromdate.Visible = false;
                txt_fromdate.Visible = false;
                lbl_todate.Visible = false;
                txt_todate.Visible = false;
                ddldept.Visible = false;
                searchby1 = 1;
            }
            if (ddlsearchby.SelectedIndex == 2)
            {
                lbl_fromdate.Visible = false;
                txt_fromdate.Visible = false;
                lbl_todate.Visible = false;
                txt_todate.Visible = false;
                txtsearch.Visible = true;
                ddldept.Visible = false;

                searchby1 = 2;
            }
            if (ddlsearchby.SelectedIndex == 3)
            {
                txtsearch.Visible = true;
                lbl_fromdate.Visible = false;
                txt_fromdate.Visible = false;
                lbl_todate.Visible = false;
                txt_todate.Visible = false;
                ddldept.Visible = false;

                searchby1 = 3;
            }
            if (ddlsearchby.SelectedIndex == 4)
            {
                txtsearch.Visible = false;
                lbl_fromdate.Visible = true;
                txt_fromdate.Visible = true;
                lbl_todate.Visible = true;
                txt_todate.Visible = true;
                ddldept.Visible = false;
            }
            if (ddlsearchby.SelectedIndex == 5)
            {
                txtsearch.Visible = true;
                lbl_fromdate.Visible = false;
                txt_fromdate.Visible = false;
                lbl_todate.Visible = false;
                txt_todate.Visible = false;
                ddldept.Visible = false;

                searchby1 = 5;
            }
            if (ddlsearchby.SelectedIndex == 6)
            {
                txtsearch.Visible = false;
                lbl_fromdate.Visible = false;
                txt_fromdate.Visible = false;
                lbl_todate.Visible = false;
                txt_todate.Visible = false;
                Binddept();
                ddldept.Visible = true;
            }
            if (ddlsearchby.SelectedIndex == 7)
            {
                txtsearch.Visible = true;
                lbl_fromdate.Visible = false;
                txt_fromdate.Visible = false;
                lbl_todate.Visible = false;
                txt_todate.Visible = false;
                ddldept.Visible = false;

                searchby1 = 7;
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    protected void ddldept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divtable.Visible = false;
            print2.Visible = false;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }

    }

    protected void txtaccess_TextChanged(object sender, EventArgs e)
    {
        try
        {
            divtable.Visible = false;
            print2.Visible = false;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }

    }

    protected void txtacc_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DivNonBookpopup.Visible = true;


            string accesscode = txacc.Text;
            string sql = "select * from nonbookmat where lib_code = '" + ddl_Library.SelectedValue + "' and nonbookmat_no = '" + accesscode + "'";
            DataSet dsNonbk = da.select_method_wo_parameter(sql, "Text");
            if (dsNonbk.Tables[0].Rows.Count > 0)
            {
                bookaccess = Convert.ToString(dsNonbk.Tables[0].Rows[0]["newaccno"]);
                jouraccess = Convert.ToString(dsNonbk.Tables[0].Rows[0]["title"]);
                string tit = Convert.ToString(dsNonbk.Tables[0].Rows[0]["title"]);
                string auth = Convert.ToString(dsNonbk.Tables[0].Rows[0]["author"]);
                string pub = Convert.ToString(dsNonbk.Tables[0].Rows[0]["publisher"]);
                string vol = Convert.ToString(dsNonbk.Tables[0].Rows[0]["Volume"]);
                string cont = Convert.ToString(dsNonbk.Tables[0].Rows[0]["Contents"]);
                string runn = Convert.ToString(dsNonbk.Tables[0].Rows[0]["runing_time"]);
                string pri = Convert.ToString(dsNonbk.Tables[0].Rows[0]["price"]);
                string stat = Convert.ToString(dsNonbk.Tables[0].Rows[0]["issue_flag"]);
                string acce = Convert.ToString(dsNonbk.Tables[0].Rows[0]["access_date"]);
                string attachment = Convert.ToString(dsNonbk.Tables[0].Rows[0]["attachment"]);
                BindLibrary1();
                txacc.Text = accesscode;
                txtitle.Text = tit;
                txauthor.Text = auth;
                txpublish.Text = pub;
                txDate_Acc.Text = acce;
                txttolprice.Text = pri;
                txtbook_accno.Text = bookaccess;
                textarea_contentpart.InnerText = cont;
                ddl_mat.SelectedItem.Text = attachment;
                txtvol.Text = vol;
                btnupdate.Visible = false;
                btndelete.Visible = false;

            }
            else
            {
                btn_save_Non_book.Visible = true;
                btnupdate.Visible = false;
                btndelete.Visible = false;
            }

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        DivNonBookpopup.Visible = true;
        ddl_mat.Attributes.Add("onfocus", "frelig5()");
        ddcurrency.Attributes.Add("onfocus", "frelig6()");
        AutoAccno();
        BindLibrary1();
        LoadAttache();
        LoadBudHead();
        GetCurrencyType();
        loadmonth();
        loaddepartment();
        btn_save_Non_book.Visible = true;
        txDate_Acc.Attributes.Add("readonly", "readonly");
        txDate_Acc.Text = DateTime.Now.ToString("dd/MM/yyyy");
        ddl_Library_SelectedIndexChanged(sender, e);
        supp();
    }

    #region Non_Book_Popup

    protected void ddl_Library_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Added By Sd
            string non = "";
            string nonlibcode = "";
            if (ddl_Library.Items.Count > 0)
                nonlibcode = Convert.ToString(ddl_Library.SelectedValue);
            if (nonlibcode != "")
            {
                non = d2.GetFunction("select nonbookmaterial from library where lib_code = '" + nonlibcode + "'");
                if (non.ToUpper() == "TRUE")
                {
                    AutoAccno();
                }
                else
                {
                    txacc.Text = "";
                    txacc.Enabled = true;
                }

            }
            loaddepartment();
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }

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
            DataSet dsjouaccno = new DataSet();
            if (ddl_mat.Items.Count > 0)
                matname = Convert.ToString(ddl_mat.SelectedItem.Text);
            if (matname != "")
            {
                popwindowjournalaccno.Visible = true;
                GrdJourAccNo.Visible = false;
                btn_pop2exit.Visible = false;
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "please Select The Attachement Material Name";
            }
            string sql = "SELECT journal.access_code, journal.journal_code,journal_master.journal_name, journal.volume_no,journal.issue_no , journal.dept_name FROM journal_master INNER JOIN journal ON journal_master.journal_code = journal.journal_code and journal_master.lib_code = journal.lib_code WHERE (journal.lib_code = '" + ddl_Library.SelectedValue + "')";
            dsjouaccno.Clear();
            dsjouaccno = d2.select_method_wo_parameter(sql, "Text");
            DataTable dtJourAccNo = new DataTable();
            DataRow drow;
            if (dsjouaccno.Tables.Count > 0 && dsjouaccno.Tables[0].Rows.Count > 0)
            {
                dtJourAccNo.Columns.Add("Access Code", typeof(string));
                dtJourAccNo.Columns.Add("Journal Code", typeof(string));
                dtJourAccNo.Columns.Add("Journal Title", typeof(string));
                dtJourAccNo.Columns.Add("Volume No", typeof(string));
                dtJourAccNo.Columns.Add("Issue No", typeof(string));
                dtJourAccNo.Columns.Add("Dept Name", typeof(string));

                for (int rolcount = 0; rolcount < dsjouaccno.Tables[0].Rows.Count; rolcount++)
                {
                    drow = dtJourAccNo.NewRow();
                    drow["Access Code"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["access_code"]);
                    drow["Journal Code"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["journal_code"]);
                    drow["Journal Title"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["journal_name"]);
                    drow["Volume No"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["volume_no"]);
                    drow["Issue No"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["issue_no"]);
                    drow["Dept Name"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["dept_name"]);
                    dtJourAccNo.Rows.Add(drow);
                }
                GrdJourAccNo.DataSource = dtJourAccNo;
                GrdJourAccNo.DataBind();
                GrdJourAccNo.Visible = true;
                btn_pop2exit.Visible = true;

            }
        }
        catch
        {
        }
    }

    protected void ddl_Search_By_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GrdJourAccNo.Visible = false;
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
        catch
        {

        }
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
            if (ddlLibrary.Items.Count > 0)
                nonlibcode = Convert.ToString(ddlLibrary.SelectedValue);
            if (ddl_mat.Items.Count > 0)
                matname = Convert.ToString(ddl_mat.SelectedItem.Text);

            if (ddl_Search_By.Items[0].Text == "All")
            {
                txt_bysearch.Text = "";
                txt_bysearch.Visible = false;
                Field_Name = "";
                search = "";
            }
            else if (ddl_Search_By.Items[1].Text == "Journal Code")
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
            DataTable dtJourAccNo = new DataTable();
            DataRow drow;

            if (dsjouaccno.Tables.Count > 0 && dsjouaccno.Tables[0].Rows.Count > 0)
            {
                dtJourAccNo.Columns.Add("Access Code", typeof(string));
                dtJourAccNo.Columns.Add("Journal Code", typeof(string));
                dtJourAccNo.Columns.Add("Journal Title", typeof(string));
                dtJourAccNo.Columns.Add("Volume No", typeof(string));
                dtJourAccNo.Columns.Add("Issue No", typeof(string));
                dtJourAccNo.Columns.Add("Dept Name", typeof(string));

                for (int rolcount = 0; rolcount < dsjouaccno.Tables[0].Rows.Count; rolcount++)
                {
                    drow = dtJourAccNo.NewRow();
                    drow["Access Code"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["access_code"]);
                    drow["Journal Code"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["journal_code"]);
                    drow["Journal Title"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["journal_name"]);
                    drow["Volume No"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["volume_no"]);
                    drow["Issue No"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["issue_no"]);
                    drow["Dept Name"] = Convert.ToString(dsjouaccno.Tables[0].Rows[rolcount]["dept_name"]);
                    dtJourAccNo.Rows.Add(drow);
                }
                divTreeView.Visible = true;
                GrdJourAccNo.DataSource = dtJourAccNo;
                GrdJourAccNo.DataBind();
                GrdJourAccNo.Visible = true;
                btn_pop2exit.Visible = true;
            }
            else
            {
                divTreeView.Visible = false;
                GrdJourAccNo.Visible = false;
                btn_pop2exit.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Records Found";
            }
        }
        catch
        {

        }
    }

    protected void btn_pop2exit_Click(object sender, EventArgs e)
    {
        popwindowjournalaccno.Visible = false;
    }

    protected void imagebtnpop2close_Click(object sender, EventArgs e)
    {
        popwindowjournalaccno.Visible = false;
    }

    protected void GrdJourAccNo_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , HiddenField1.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void GrdJourAccNo_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string journalAccno = GrdJourAccNo.Rows[rowIndex].Cells[1].Text;
            string journaltitle = GrdJourAccNo.Rows[rowIndex].Cells[3].Text;
            string volumeno = GrdJourAccNo.Rows[rowIndex].Cells[4].Text;
            txt_jour.Text = journalAccno;
            txtitle.Text = journaltitle;
            txtvol.Text = volumeno;
            popwindowjournalaccno.Visible = false;
            txtitle.Enabled = false;
            ddl_mat.Enabled = false;
        }
        catch (Exception ex)
        {
        }
    }

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
                GrdJourAccNo.Visible = false;
                btn_pop2exit.Visible = false;
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "please Select The Attachement Material Name";
            }

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    protected void ddl_search_book_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdAccessNo.Visible = false;
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
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    protected void btn_book_go_Click(object sender, EventArgs e)
    {
        try
        {
            string nonlbcode = "";
            string nonmatname = "";
            string FieldName = "";
            string Searchbook = "";
            string sqlqry = "";
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
            if (ddl_Library.Items.Count > 0)
                nonlbcode = Convert.ToString(ddl_Library.SelectedValue);
            if (ddl_mat.Items.Count > 0)
                nonmatname = Convert.ToString(ddl_mat.SelectedItem.Text);
            if (txt_boaccno.Text == "")
            {
                if (Searchbook == "" || Searchbook == "All")
                {
                    sqlqry = "select acc_no,title,author,publisher,edition from bookdetails where lib_code='" + nonlbcode + "' order by len(acc_no),acc_no ";// and attachment = '" + nonmatname + "' and acc_no  not in (select acc_no from nonbookmat where lib_code='" + nonlbcode + "')
                }
                else
                {
                    if (txt_book_search.Text == "")

                        sqlqry = "select acc_no,title,author,publisher,edition from bookdetails where lib_code='" + nonlbcode + "' order by len(acc_no),acc_no ";// and acc_no  not in (select acc_no from nonbookmat where lib_code='" + nonlbcode + "')
                    else
                        sqlqry = "select acc_no,title,author,publisher,edition from bookdetails where lib_code='" + nonlbcode + "' and " + FieldName + " like '" + txt_book_search + "%' order by len(acc_no),acc_no  ";//and acc_no not in (select acc_no from nonbookmat where lib_code='" + nonlbcode + "')
                }
            }
            else
            {
                if (Searchbook == "" || Searchbook == "All")
                {
                    sqlqry = "select acc_no,title,author,publisher,edition from bookdetails where lib_code='" + nonlbcode + "' and acc_no like '" + txt_boaccno.Text + "%' order by len(acc_no),acc_no ";//and acc_no  not in (select isnull(acc_no,'') from nonbookmat where  lib_code='" + nonlbcode + "')
                }
                else
                {
                    if (txt_book_search.Text == "")
                        sqlqry = "select acc_no,title,author,publisher,edition from bookdetails where acc_no like '" + txt_boaccno.Text + "%' and lib_code='" + nonlbcode + "'  order by len(acc_no),acc_no ";//and acc_no not in (select isnull(acc_no,'') from nonbookmat where lib_code='" + nonlbcode + "')
                    else
                        sqlqry = "select acc_no,title,author,publisher,edition from bookdetails where " + FieldName + " like '" + txt_book_search + "%' and acc_no like '" + txt_boaccno.Text + "%' and lib_code='" + nonlbcode + "'  and acc_no like '" + txt_boaccno.Text + "%' order by len(acc_no),acc_no ";//and acc_no not in (select isnull(acc_no,'') from nonbookmat where lib_code='" + nonlbcode + "')
                }
            }
            dsbook.Clear();
            dsbook = d2.select_method_wo_parameter(sqlqry, "Text");
            DataTable dtAccessNo = new DataTable();
            DataRow drow;
            if (dsbook.Tables.Count > 0 && dsbook.Tables[0].Rows.Count > 0)
            {
                dtAccessNo.Columns.Add("Access No", typeof(string));
                dtAccessNo.Columns.Add("Title", typeof(string));
                dtAccessNo.Columns.Add("Author", typeof(string));
                dtAccessNo.Columns.Add("Publisher", typeof(string));
                dtAccessNo.Columns.Add("Edition", typeof(string));

                for (int row = 0; row < dsbook.Tables[0].Rows.Count; row++)
                {
                    drow = dtAccessNo.NewRow();
                    drow["Access No"] = Convert.ToString(dsbook.Tables[0].Rows[row]["acc_no"]);
                    drow["Title"] = Convert.ToString(dsbook.Tables[0].Rows[row]["title"]);
                    drow["Author"] = Convert.ToString(dsbook.Tables[0].Rows[row]["author"]);
                    drow["Publisher"] = Convert.ToString(dsbook.Tables[0].Rows[row]["publisher"]);
                    drow["Edition"] = Convert.ToString(dsbook.Tables[0].Rows[row]["edition"]);
                    dtAccessNo.Rows.Add(drow);
                }
                div1.Visible = true;
                grdAccessNo.DataSource = dtAccessNo;
                grdAccessNo.DataBind();
                grdAccessNo.Visible = true;
                Cache.Remove("BBcacheKey");
                btn_book_exit.Visible = true;
            }
            else
            {
                div1.Visible = false;
                grdAccessNo.Visible = false;
                btn_pop2exit.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Records Found";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    protected void btn_book_ok_exit(object sender, EventArgs e)
    {
        DivBookAccessNo.Visible = false;
    }

    protected void image_DivBookAccessNoclose_Click(object sender, EventArgs e)
    {
        DivBookAccessNo.Visible = false;
    }

    protected void grdAccessNo_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void grdAccessNo_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string journalAccno = grdAccessNo.Rows[rowIndex].Cells[1].Text;
            txtbook_accno.Text = journalAccno;
            txauthor.Text = grdAccessNo.Rows[rowIndex].Cells[3].Text;
            txpublish.Text = grdAccessNo.Rows[rowIndex].Cells[4].Text;
            DivBookAccessNo.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void grdAccessNo_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdAccessNo.PageIndex = e.NewPageIndex;
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
        catch
        {
        }

    }
    protected void btn_min_mat_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_mat.Items.Count > 0)
                g1 = Convert.ToString(ddl_mat.Text);
            if (ddl_Library.Items.Count > 0)
                libcode = Convert.ToString(ddl_Library.SelectedValue);
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
        catch
        {
        }

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
        catch
        {
        }

    }
    protected void btn_min_currn_Click(object sender, EventArgs e)
    {
        try
        {

            if (ddcurrency.Items.Count > 0)
                g1 = Convert.ToString(ddcurrency.Text);
            if (ddl_Library.Items.Count > 0)
                libcode = Convert.ToString(ddl_Library.SelectedValue);
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
        catch
        {
        }

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
            libcode = Convert.ToString(ddl_Library.SelectedValue);
            if (group != "")
            {

                if (lbl_addgroup.Text.Trim() == "Currency")
                {

                    int j = ddcurrency.Items.Count;

                    ddcurrency.Items.Insert(j, group);

                }

                    //ddcurrency.Items[0].Text = group;

                else if (lbl_addgroup.Text.Trim() == "Material Name")
                {

                    int j = ddl_mat.Items.Count;

                    ddl_mat.Items.Insert(j, group);

                }

                //ddl_mat.Items[0].Text = group;

                plusdiv.Visible = false;
            }
            else
            {
                plusdiv.Visible = true;
                lblerror.Visible = true;
                lblerror.Text = "Please Enter the " + lbl_addgroup.Text + "";
            }

        }

        catch
        {
        }
    }

    protected void btn_exitaddgroup_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addgroup.Visible = false;
        txt_addgroup.Text = "";
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
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    protected void rbl_non_mul_Selected(object sender, EventArgs e)
    {
        try
        {
            rbl_non_Single.Checked = false;
            txcopy.Visible = true;

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }


    #endregion

    #region Save_Non_Book

    protected void btn_save_Non_book_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet rsBud = new DataSet();
            string lbcode = "";
            string matname = "";
            string sqlnonbooksave = "";
            string currntype = "";
            string jaccno = "";
            string Baccno = "";
            string status = "";
            string Depart = "";
            string sqlsave = "";
            string budjet = "";
            string totprice = "";
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
            if (ddl_non_budget.Items.Count > 0)
                budjet = Convert.ToString(ddl_non_budget.SelectedItem.Value);
            string accessdate = string.Empty;
            string Accdate1 = Convert.ToString(txDate_Acc.Text);
            string[] adate1 = Accdate1.Split('/');
            if (adate1.Length == 3)
                accessdate = adate1[2].ToString() + "-" + adate1[1].ToString() + "-" + adate1[0].ToString();
            string time = DateTime.Now.ToString("hh:mm tt");
            string jour = jouraccess;
            string book = bookaccess;
            string supp = ddlsupp.Text;
            string billda = Txtbilldate.Text;
            string invoice = txt_billno.Text;
            DateTime CurDate = DateTime.Now;
            string CurrentDate = CurDate.ToString("yyyy-MM-dd");
            string datim = accessdate + '-' + time;
            string Attcount = string.Empty;
            string monyear = ddl_monYear.Text + '-' + txtyear.Text;
            string nonbook = txacc.Text;
            Attcount = d2.GetFunction("select  count(attachment_name) as attachment_name from attachment where attachment_name='" + matname + "'");
            int acnt = Convert.ToInt32(Attcount);
            string curcount = d2.GetFunction("select  count(currency_type) as currency_type from currency_convertion where currency_type='" + currntype + "'");
            int ccnt = Convert.ToInt32(curcount);
            if (acnt <= 0)
            {
                sqlsave = "insert into attachment(attachment_name,lib_code) values('" + matname + "','" + lbcode + "')";
                insert = d2.update_method_wo_parameter(sqlsave, "Text");
            }
            else if (ccnt <= 0)
            {

                sqlsave = "insert into currency_convertion(currency_type) values( '" + currntype + "')";
                insert = d2.update_method_wo_parameter(sqlsave, "Text");
            }
            if (txttolprice.Text == "")
                txttolprice.Text = "0.00";
            if (txt_acc_coe.Text != "")
            {
                string accnum = "select count(nonbookmat) as nonbookmat from nonbookmat where nonbookmat_no='" + txt_acc_coe.Text + "' and lib_code='" + lbcode + "'";
                int acnum = Convert.ToInt32(accnum);
                if (acnum > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Access Number Already Exists,Please Enter New Number";
                    return;
                }

            }
            int aa = 0;
            int.TryParse(txcopy.Text, out aa);
            if (txcopy.Visible == true && txcopy.Text == "")
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = ("Enter No Of Copies");
                return;
            }

            if (txcopy.Visible == true && (txcopy.Text == "0" || aa <= 1))
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = ("Enter No Of Copies greater than 1");
                return;
            }

            if (txcopy.Visible == true && txtbook_accno.Text != "")
            {
                if (txcopy.Text == "")
                {
                    txcopy.Text = "1";
                }

                if (aa == 0 || aa <= 1)
                {
                    lblalerterr.Text = ("Enter No Of Copies greater than 1");
                }
                sqlsave = "insert into nonbookmat (access_date,nonbookmat_no,acc_no,title,author,publisher,volume,isbn,runing_time,contents,lib_code,attachment,price,issue_flag,currency_type,currency_value,department,mon_year,newaccno,issue_no,Budget_Head,supplier,billdate,invoiceno) values('" + CurrentDate + "', '" + nonbook + "' ,'" + jour + "','" + txtitle.Text + "','" + txauthor.Text + "','" + txpublish.Text + "','" + txtvol.Text + "','" + txtisbn.Text + "','" + txt_time.Text + "','" + textarea_contentpart.InnerText + "','" + lbcode + "','" + matname + "','" + txttolprice.Text + "','" + status + "','" + currntype + "','" + txcurrval.Text + "','" + Depart + "','" + monyear + "','" + txtbook_accno.Text + "','" + txtissueno.Text + "','" + budjet + "','" + supp + "','" + billda + "','" + invoice + "')";
                insert = d2.update_method_wo_parameter(sqlsave, "Text");

                if (txttolprice.Text != "")
                    totprice = txttolprice.Text;
                if (Convert.ToString(ddl_non_budget.SelectedItem) != "")
                {
                    sqlsave = "select * from LibBudgetMaster WHERE Dept_Name ='" + Depart + "' AND Head_Code ='" + budjet + "' and '" + Convert.ToString(CurrentDate) + "'' between Budget_From And Budget_To  ";
                    rsBud.Clear();
                    rsBud = d2.select_method_wo_parameter(sqlsave, "Text");

                    if (rsBud.Tables[0].Rows.Count > 0)
                    {
                        string nSpendAmt = Convert.ToString(rsBud.Tables[0].Rows[0]["NSpendAmt"]);
                        int bb = Convert.ToInt32(nSpendAmt);
                        int totp = Convert.ToInt32(totprice);
                        string nBudAmt = Convert.ToString(rsBud.Tables[0].Rows[0]["NBudAmt"]);
                        int cc = Convert.ToInt32(nBudAmt);
                        if (bb + totp > cc)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = ("Amount Exceeding Budget Amount");
                            return;
                        }

                        sqlsave = "update LibBudgetMaster set NSpendAmt =NSpendAmt +'" + txttolprice.Text + "',TotSpendAmt =TotSpendAmt+'" + txttolprice.Text + "',NBalAmt = NBalAmt -'" + txttolprice.Text + "',TotBalAmt = TotBalAmt -'" + txttolprice.Text + "' WHERE Dept_Name ='" + Depart + "' AND Head_Code ='" + budjet + "'";
                        insert = d2.update_method_wo_parameter(sqlsave, "Text");
                    }
                }
            }
            else
            {
                if (txcopy.Visible == false)
                {
                    sqlsave = " insert into nonbookmat (access_date,nonbookmat_no,acc_no,title,author,publisher,volume,isbn,runing_time,contents,lib_code,attachment,price,issue_flag,currency_type,currency_value,department,mon_year,newaccno,issue_no,supplier,billdate,invoiceno) values('" + CurrentDate + "', '" + nonbook + "' ,'" + jour + "','" + txtitle.Text + "','" + txauthor.Text + "','" + txpublish.Text + "','" + txtvol.Text + "','" + txtisbn.Text + "','" + txt_time.Text + "','" + textarea_contentpart.InnerText + "','" + lbcode + "','" + matname + "','" + txttolprice.Text + "','" + status + "','" + currntype + "','" + txcurrval.Text + "','" + Depart + "','" + monyear + "','" + txtbook_accno.Text + "','" + txtissueno.Text + "','" + supp + "','" + billda + "','" + invoice + "')";
                    insert = d2.update_method_wo_parameter(sqlsave, "Text");

                }
                if (Convert.ToString(ddl_non_budget.SelectedItem) != "")
                {
                    sqlsave = "select * from LibBudgetMaster WHERE Dept_Name ='" + Depart + "' AND Head_Code ='" + budjet + "' and '" + Convert.ToString(CurrentDate) + "' between Budget_From And Budget_To  ";
                    rsBud.Clear();
                    rsBud = d2.select_method_wo_parameter(sqlsave, "Text");

                    if (rsBud.Tables[0].Rows.Count > 0)
                    {
                        string nSpendAmt = Convert.ToString(rsBud.Tables[0].Rows[0]["NSpendAmt"]);
                        int bb = Convert.ToInt32(nSpendAmt);
                        int totp = Convert.ToInt32(totprice);
                        string nBudAmt = Convert.ToString(rsBud.Tables[0].Rows[0]["NBudAmt"]);
                        int cc = Convert.ToInt32(nBudAmt);
                        if (bb + totp > cc)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = ("Amount Exceeding Budget Amount");
                            return;
                        }
                        sqlsave = "update LibBudgetMaster set NSpendAmt =NSpendAmt +'" + txttolprice.Text + "',TotSpendAmt =TotSpendAmt+'" + txttolprice.Text + "',NBalAmt = NBalAmt -'" + txttolprice.Text + "',TotBalAmt = TotBalAmt -'" + txttolprice.Text + "' WHERE Dept_Name ='" + Depart + "' AND Head_Code ='" + budjet + "'";
                        insert = d2.update_method_wo_parameter(sqlsave, "Text");
                    }
                }
            }

            alertpopwindow.Visible = true;
            lblalerterr.Text = ("Non Book Material Details Saved Successfully");
            ddl_Library.Items.Clear();
            txacc.Text = "";
            ddl_mat.Items.Clear();
            ddl_non_budget.Items.Clear();
            txt_jour.Text = "";
            txtbook_accno.Text = "";
            txtitle.Text = "";
            ddDepart.Items.Clear();
            ddl_monYear.Items.Clear();
            txtyear.Text = "";
            txpublish.Text = "";
            ddcurrency.Items.Clear();
            txcurrval.Text = "";
            txttolprice.Text = "";
            txtissueno.Text = "";
            txtisbn.Text = "";
            textarea_contentpart.InnerText = "";
            txt_time.Text = "";
        }
        catch (Exception ex)
        {
        }
        //{ d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
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
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    #endregion

    protected void btndelete_Click(object sender, EventArgs e)
    {
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


        string access = txacc.Text;
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

        else
        {
            lblalerterr.Text = "Do You Want To Delete this Non Book Material=yes";
        }
        string sql = "delete from nonbookmat where nonbookmat_no = '" + access + "' and lib_code = '" + lbcode + "'";
        insert = d2.update_method_wo_parameter(sql, "Text");
        alertpopwindow.Visible = true;
        lblalerterr.Text = "Nonbookmaterial Details Deleted Successfully";
    }

    protected void btnupdate_Click(object sender, EventArgs e)
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
            btn_pls_mat.Visible = true;
            btn_min_mat.Visible = true;
            btn_pl_currn.Visible = true;
            btn_min_currn.Visible = true;
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


            string accessdate = string.Empty;
            string Accdate1 = Convert.ToString(txDate_Acc.Text);
            string[] adate1 = Accdate1.Split('/');
            if (adate1.Length == 3)
                accessdate = adate1[2].ToString() + "-" + adate1[1].ToString() + "-" + adate1[0].ToString();
            string monyear = ddl_monYear.Text + '-' + txtyear.Text;
            string Attcount = d2.GetFunction("select  count(attachment_name) as attachment_name from attachment where attachment_name='" + matname + "'");
            int acnt = Convert.ToInt32(Attcount);
            string currncount = d2.GetFunction("select  count(currency_type) as currency_type from currency_convertion where currency_type='" + currntype + "'");
            int cuncnt = Convert.ToInt32(currncount);
            if (acnt <= 0)
            {
                sqlnonbooksave = "insert into attachment(attachment_name,lib_code) values('" + matname + "','" + lbcode + "')";
                insert = d2.update_method_wo_parameter(sqlnonbooksave, "Text");
            }
            else if (cuncnt <= 0)
            {
                sqlnonbooksave = "insert into currency_convertion(currency_type) values( '" + currntype + "')";
                insert = d2.update_method_wo_parameter(sqlnonbooksave, "Text");
            }
            if (txttolprice.Text != "")
            {
                sqlsave = "update nonbookmat set issue_flag = '" + status + "', access_date = '" + accessdate + "',access_time = '" + time + "',title = '" + txtitle.Text + "',author = '" + txauthor.Text + "',publisher = '" + txpublish.Text + "',volume = '" + txtvol.Text + "',isbn = '" + txtisbn.Text + "',runing_time = '" + txt_time.Text + "',contents = '" + textarea_contentpart.InnerText + "',attachment = '" + matname + "',price = '" + txttolprice.Text + "' ,currency_type = '" + currntype + "' , currency_value = '" + txcurrval.Text + "',department='" + Depart + "',mon_year='" + monyear + "',newaccno='" + txtbook_accno.Text + "', issue_no='" + txtissueno.Text + "' where nonbookmat_no = '" + txacc.Text + "' and lib_code = '" + lbcode + "'  and supplier = '" + ddlsupp.Text + "'  and billdate = '" + Txtbilldate.Text + "'  and invoiceno = '" + txt_billno.Text + "'";
                insert = d2.update_method_wo_parameter(sqlsave, "Text");

            }
            if (insert > 0)
            {

                alertpopwindow.Visible = true;
                lblalerterr.Text = "Updated SuccessFully!";
                nonbookclear();
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    protected void btn_Exit_Non_book_Click(object sender, EventArgs e)
    {
        DivNonBookpopup.Visible = false;

    }

    public void AutoAccno()
    {
        try
        {
            string codeno = "";
            string codeno1 = "";
            int TotLen = 0;
            int intpos = 0;
            DataSet rs2 = new DataSet();
            DataSet rs3 = new DataSet();
            string nonlibcode = "";
            string sqlnonqry = "";
            if (ddlLibrary.Items.Count > 0)
                nonlibcode = Convert.ToString(ddlLibrary.SelectedValue);
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

    #endregion

    protected void btn_DivNonBookpopup_popclose_Click(object sender, EventArgs e)
    {
        DivNonBookpopup.Visible = false;
    }

    protected void btngoClick(object sender, EventArgs e)
    {
        DataSet material = new DataSet();
        material = book();
        if (material.Tables.Count > 0 && material.Tables[0].Rows.Count > 0)
        {
            loadspreadstud(ds);
        }
        else
        {
            grdNonBook.Visible = false;
            alertpopwindow.Visible = true;
            lblalerterr.Text = "No Record Found!";
        }
    }

    private DataSet book()
    {
        string lib = string.Empty;
        string dept = string.Empty;
        string access = string.Empty;
        string search = string.Empty;
        string sql = string.Empty;
        string title = string.Empty;
        string Author = string.Empty;
        string publish = string.Empty;
        string infromdate = string.Empty;
        string bookcode = string.Empty;
        string intodate = string.Empty;
        string qrylibraryFilter = string.Empty;
        string qrytitlefilter = string.Empty;
        string qryauthorfilter = string.Empty;
        string qrypublishfilter = string.Empty;
        try
        {
            if (ddlCollege.Items.Count > 0)
                collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            //if (ddlLibrary.Items.Count > 0)
            //    lib = Convert.ToString(ddlLibrary.SelectedValue);

            if (ddlLibrary.Items.Count > 0)
            {
                for (int i = 0; i < ddlLibrary.Items.Count - 1; i++)
                {
                    if (Convert.ToString(ddlLibrary.SelectedItem) == "All")
                    {
                        if (lib == "")
                        {
                            lib = "" + ddlLibrary.Items[i + 1].Value + "";
                        }
                        else
                        {
                            lib = lib + "'" + "," + "'" + ddlLibrary.Items[i + 1].Value + "";
                        }
                    }
                    else
                        lib = ddlLibrary.SelectedValue;
                }
            }
            if (ddldept.Items.Count > 0)
                dept = Convert.ToString(ddldept.SelectedValue);
            if (ddlsearchby.Items.Count > 0)
                search = Convert.ToString(ddlsearchby.SelectedValue);
            access = txtaccess.Text;

            string fromDate = txt_fromdate.Text;
            string toDate = txt_todate.Text;
            string[] fromdate = fromDate.Split('/');
            string[] todate = toDate.Split('/');
            if (fromdate.Length == 3)
                infromdate = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();
            if (todate.Length == 3)
                intodate = todate[2].ToString() + "-" + todate[1].ToString() + "-" + todate[0].ToString();
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(lib))
            {
                if (lib != "All" && lib != "")
                {
                    qrylibraryFilter = "and lib_code in('" + lib + "')";
                }
                if (ddlsearchby.SelectedIndex == 0 || txtaccess.Text == "")
                {
                    if (ddlsearchby.SelectedIndex == 0 || ddlsearchby.SelectedItem.Text == "")
                    {
                        sql = "select nonbookmat_no,newaccno,acc_no,title,author,publisher,volume,contents,runing_time,supplier,billdate,invoiceno,issue_flag,CONVERT(varchar(20),access_date,103) access_date,department,price from nonbookmat where  issue_flag<>'Return' " + qrylibraryFilter + "";
                    }
                    else
                        if (ddlsearchby.SelectedIndex == 4)
                        {
                            sql = "select nonbookmat_no,newaccno,acc_no,title,author,publisher,volume,contents,runing_time,supplier,billdate,invoiceno,issue_flag,CONVERT(varchar(20),access_date,103) access_date,department,price,CAST(RIGHT(nonbookmat_no, LEN(nonbookmat_no) - PATINDEX('%[0-9]%', nonbookmat_no)+1) AS float), LEFT(nonbookmat_no, PATINDEX('%[0-9]%', nonbookmat_no)-1) from nonbookmat where  access_date between '" + infromdate + "' and '" + intodate + "'  and issue_flag<>'Return' " + qrylibraryFilter + "";
                        }
                        else if (ddlsearchby.SelectedIndex == 7)
                        {

                            sql = "select nonbookmat_no,newaccno,acc_no,title,author,publisher,volume,contents,runing_time,supplier,billdate,invoiceno,issue_flag,CONVERT(varchar(20),access_date,103) access_date,department,price,CAST(RIGHT(nonbookmat_no, LEN(nonbookmat_no) - PATINDEX('%[0-9]%', nonbookmat_no)+1) AS float), LEFT(nonbookmat_no, PATINDEX('%[0-9]%', nonbookmat_no)-1) from nonbookmat where  attachment like '" + txtsearch.Text + "%' and issue_flag<>'Return' " + qrylibraryFilter + "";
                        }
                        else if (ddlsearchby.SelectedIndex == 5)
                        {

                            sql = "select nonbookmat_no,newaccno,acc_no,title,author,publisher,volume,contents,runing_time,billdate,issue_flag,invoiceno,CONVERT(varchar(20),access_date,103) access_date,department,price,CAST(RIGHT(nonbookmat_no, LEN(nonbookmat_no) - PATINDEX('%[0-9]%', nonbookmat_no)+1) AS float), LEFT(nonbookmat_no, PATINDEX('%[0-9]%', nonbookmat_no)-1) from nonbookmat where  nonbookmat_no like '" + txtsearch.Text + "%' and issue_flag<>'Return' " + qrylibraryFilter + "";
                        }
                        else if (ddlsearchby.SelectedIndex == 6)
                        {
                            if (dept != "All")
                            {
                                sql = "select nonbookmat_no,newaccno,acc_no,title,author,publisher,volume,contents,runing_time,billdate,invoiceno,supplier,issue_flag,CONVERT(varchar(20),access_date,103) access_date,department,price,CAST(RIGHT(nonbookmat_no, LEN(nonbookmat_no) - PATINDEX('%[0-9]%', nonbookmat_no)+1) AS float), LEFT(nonbookmat_no, PATINDEX('%[0-9]%', nonbookmat_no)-1) from nonbookmat where department like '%" + dept + "%'  and issue_flag<>'Return' " + qrylibraryFilter + "";
                            }
                            else
                            {
                                sql = "select nonbookmat_no,newaccno,acc_no,title,author,publisher,volume,contents,runing_time,billdate,supplier,invoiceno,issue_flag,CONVERT(varchar(20),access_date,103) access_date,department,price,CAST(RIGHT(nonbookmat_no, LEN(nonbookmat_no) - PATINDEX('%[0-9]%', nonbookmat_no)+1) AS float), LEFT(nonbookmat_no, PATINDEX('%[0-9]%', nonbookmat_no)-1) from nonbookmat where issue_flag<>'Return' " + qrylibraryFilter + "";
                            }
                        }
                        else

                            if (txtsearch.Text == "")
                            {
                                sql = "select nonbookmat_no,newaccno,acc_no,title,author,publisher,volume,contents,runing_time,supplier,invoiceno,issue_flag,CONVERT(varchar(20),access_date,103) access_date,department,price,CAST(RIGHT(nonbookmat_no, LEN(nonbookmat_no) - PATINDEX('%[0-9]%', nonbookmat_no)+1) AS float), LEFT(nonbookmat_no, PATINDEX('%[0-9]%', nonbookmat_no)-1) from nonbookmat where  issue_flag<>'Return' " + qrylibraryFilter + "";
                            }
                            else
                            {
                                sql = "select nonbookmat_no,newaccno,acc_no,title,author,publisher,volume,contents,runing_time,supplier,invoiceno,issue_flag,CONVERT(varchar(20),access_date,103) access_date,department,price,CAST(RIGHT(nonbookmat_no, LEN(nonbookmat_no) - PATINDEX('%[0-9]%', nonbookmat_no)+1) AS float), LEFT(nonbookmat_no, PATINDEX('%[0-9]%', nonbookmat_no)-1) from nonbookmat where " + ddlsearchby.SelectedItem.Text + " like '%" + txtsearch.Text + "%'  and issue_flag<>'Return'" + qrylibraryFilter + "";
                            }
                }

                if (ddlsearchby.SelectedIndex == 0 || txtaccess.Text != "")
                {
                    sql = sql + " and ltrim(rtrim(nonbookmat_no)) like '" + txtaccess.Text + "%'";
                }
                sql = sql + " ORDER BY LEFT(ltrim(rtrim(nonbookmat_no)), PATINDEX('%[0-9]%', nonbookmat_no)-1)  ,  CAST(RIGHT(ltrim(rtrim(nonbookmat_no)), LEN(ltrim(rtrim(nonbookmat_no))) - PATINDEX('%[0-9]%', ltrim(rtrim(nonbookmat_no)))+1) AS float) ";
                nonbookmat.Clear();
                nonbookmat = d2.select_method_wo_parameter(sql, "Text");
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
        return nonbookmat;
    }

    private void loadspreadstud(DataSet ds)
    {
        DataSet dscostm = new DataSet();
        string access = string.Empty;
        string materialname = string.Empty;
        string title = string.Empty;
        string author = string.Empty;
        string publisher = string.Empty;
        string department = string.Empty;
        string content = string.Empty;
        string statusnon = string.Empty;
        string bookaccess = string.Empty;
        string jouaccess = string.Empty;
        string volume = string.Empty;
        string runningtime = string.Empty;
        string prices = string.Empty;
        string supple = string.Empty;
        string invono = string.Empty;
        string bilda = string.Empty;
        string accessdate = string.Empty;
        try
        {
            if (nonbookmat.Tables.Count > 0 && nonbookmat.Tables[0].Rows.Count > 0)
            {
                DataTable dtNonBook = new DataTable();
                DataRow drow;
                int sno = 0;
                dtNonBook.Columns.Add("SNo", typeof(string));
                dtNonBook.Columns.Add("Access No", typeof(string));
                dtNonBook.Columns.Add("Book Access No", typeof(string));
                dtNonBook.Columns.Add("Journal Access No", typeof(string));
                dtNonBook.Columns.Add("Title", typeof(string));
                dtNonBook.Columns.Add("Author", typeof(string));
                dtNonBook.Columns.Add("Publisher", typeof(string));
                dtNonBook.Columns.Add("Volume", typeof(string));
                dtNonBook.Columns.Add("Contents", typeof(string));
                dtNonBook.Columns.Add("Running Time", typeof(string));
                dtNonBook.Columns.Add("Price", typeof(string));
                dtNonBook.Columns.Add("Status", typeof(string));
                dtNonBook.Columns.Add("Access Date", typeof(string));
                dtNonBook.Columns.Add("Supplier", typeof(string));
                dtNonBook.Columns.Add("Invoice No", typeof(string));
                dtNonBook.Columns.Add("Bill Date", typeof(string));

                for (int row = 0; row < nonbookmat.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    access = Convert.ToString(nonbookmat.Tables[0].Rows[row]["nonbookmat_no"]).Trim();
                    bookaccess = Convert.ToString(nonbookmat.Tables[0].Rows[row]["newaccno"]).Trim();
                    jouaccess = Convert.ToString(nonbookmat.Tables[0].Rows[row]["acc_no"]).Trim();
                    title = Convert.ToString(nonbookmat.Tables[0].Rows[row]["title"]).Trim();
                    author = Convert.ToString(nonbookmat.Tables[0].Rows[row]["author"]).Trim();
                    publisher = Convert.ToString(nonbookmat.Tables[0].Rows[row]["publisher"]).Trim();
                    volume = Convert.ToString(nonbookmat.Tables[0].Rows[row]["volume"]).Trim();
                    content = Convert.ToString(nonbookmat.Tables[0].Rows[row]["contents"]).Trim();
                    runningtime = Convert.ToString(nonbookmat.Tables[0].Rows[row]["runing_time"]).Trim();
                    prices = Convert.ToString(nonbookmat.Tables[0].Rows[row]["price"]).Trim();
                    status = Convert.ToString(nonbookmat.Tables[0].Rows[row]["issue_flag"]).Trim();
                    accessdate = Convert.ToString(nonbookmat.Tables[0].Rows[row]["access_date"]).Trim();
                    supple = Convert.ToString(nonbookmat.Tables[0].Rows[row]["supplier"]).Trim();
                    invono = Convert.ToString(nonbookmat.Tables[0].Rows[row]["invoiceno"]).Trim();
                    bilda = Convert.ToString(nonbookmat.Tables[0].Rows[row]["billdate"]).Trim();

                    drow = dtNonBook.NewRow();
                    drow["SNo"] = Convert.ToString(sno);
                    drow["Access No"] = access;
                    drow["Book Access No"] = bookaccess;
                    drow["Journal Access No"] = jouaccess;
                    drow["Title"] = title;
                    drow["Author"] = author;
                    drow["Publisher"] = publisher;
                    drow["Volume"] = volume;
                    drow["Contents"] = content;
                    drow["Running Time"] = runningtime;
                    drow["Price"] = prices;
                    drow["Status"] = status;
                    drow["Access Date"] = accessdate;
                    drow["Supplier"] = supple;
                    drow["Invoice No"] = invono;
                    drow["Bill Date"] = bilda;
                    dtNonBook.Rows.Add(drow);
                }
                grdNonBook.DataSource = dtNonBook;
                grdNonBook.DataBind();

                grdNonBook.Visible = true;
                divtable.Visible = true;
                print2.Visible = true;
                lbl_totrecord.InnerText = "Total No Of Records:" + nonbookmat.Tables[0].Rows.Count;
            }
        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    protected void grdNonBook_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdNonBook.PageIndex = e.NewPageIndex;
        btngoClick(sender, e);
    }

    protected void grdNonBook_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , HiddenFieldNonBook.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void grdNonBook_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.HiddenFieldNonBook.Value);
            if (Convert.ToString(rowIndex) != "-1" && Convert.ToString(rowIndex) != "")
            {
                LoadBudHead();
                loadmonth();
                loaddepartment();

                if (ddlLibrary.Items.Count > 0)
                {
                    for (int i = 0; i < ddlLibrary.Items.Count - 1; i++)
                    {
                        if (Convert.ToString(ddlLibrary.SelectedItem) == "All")
                        {
                            if (libcode == "")
                            {
                                libcode = "" + ddlLibrary.Items[i + 1].Value + "";
                            }
                            else
                            {
                                libcode = libcode + "'" + "," + "'" + ddlLibrary.Items[i + 1].Value + "";
                            }
                        }
                        else
                            libcode = ddlLibrary.SelectedValue;
                    }
                }
                string qrycurrentype = "select distinct attachment from  nonbookmat where lib_code in('" + libcode + "') ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(qrycurrentype, "TEXT");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_mat.DataSource = ds;
                    ddl_mat.DataTextField = "attachment";
                    ddl_mat.DataValueField = "attachment";
                    ddl_mat.DataBind();
                }
                DivNonBookpopup.Visible = true;
                btn_save_Non_book.Visible = false;
                btnupdate.Visible = true;
                btndelete.Visible = true;
                string accesscode = grdNonBook.Rows[rowIndex].Cells[1].Text;
                string sql = "select * from nonbookmat where lib_code in('" + libcode + "') and nonbookmat_no = '" + accesscode + "'";
                DataSet dsNonbk = da.select_method_wo_parameter(sql, "Text");
                if (dsNonbk.Tables[0].Rows.Count > 0)
                {
                    bookaccess = grdNonBook.Rows[rowIndex].Cells[2].Text;
                    jouraccess = grdNonBook.Rows[rowIndex].Cells[3].Text;
                    if (jouraccess == "&nbsp;")
                    {
                        jouraccess = "";
                    }
                    string tit = Convert.ToString(dsNonbk.Tables[0].Rows[0]["title"]);
                    string auth = Convert.ToString(dsNonbk.Tables[0].Rows[0]["author"]);
                    string pub = Convert.ToString(dsNonbk.Tables[0].Rows[0]["publisher"]);
                    string vol = Convert.ToString(dsNonbk.Tables[0].Rows[0]["Volume"]);
                    string cont = Convert.ToString(dsNonbk.Tables[0].Rows[0]["Contents"]);
                    string runn = Convert.ToString(dsNonbk.Tables[0].Rows[0]["runing_time"]);
                    string pri = Convert.ToString(dsNonbk.Tables[0].Rows[0]["price"]);
                    string stat = Convert.ToString(dsNonbk.Tables[0].Rows[0]["issue_flag"]);
                    string acce = grdNonBook.Rows[rowIndex].Cells[12].Text;
                    string attachment = Convert.ToString(dsNonbk.Tables[0].Rows[0]["attachment"]);
                    string suppli = Convert.ToString(dsNonbk.Tables[0].Rows[0]["supplier"]);
                    string invo = Convert.ToString(dsNonbk.Tables[0].Rows[0]["invoiceno"]);
                    string bildte = Convert.ToString(dsNonbk.Tables[0].Rows[0]["billdate"]);
                    BindLibrary1();
                    txacc.Text = accesscode;
                    txtitle.Text = tit;
                    txauthor.Text = auth;
                    txpublish.Text = pub;
                    txDate_Acc.Text = acce;
                    txttolprice.Text = pri;
                    txtbook_accno.Text = bookaccess;
                    textarea_contentpart.InnerText = cont;
                    ddl_mat.SelectedItem.Text = attachment;
                    txtvol.Text = vol;
                    ddlsupp.SelectedItem.Text = suppli;
                    txt_billno.Text = invo;
                    Txtbilldate.Text = bildte;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    #region Print
    protected void btnExcel_Click2(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname2.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdNonBook, reportname);
                lblvalidation3.Visible = false;
            }
            else
            {
                lblvalidation3.Text = "Please Enter Your  Report Name";
                lblvalidation3.Visible = true;
                txtexcelname2.Focus();
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    public void btnprintmaster_Click2(object sender, EventArgs e)
    {
        try
        {
            lblvalidation3.Text = "";
            txtexcelname2.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Non Book Material";
            //+'@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "nonbookmaterial.aspx";
            string ss = "";
            Printcontrolhed2.loadspreaddetails(grdNonBook, pagename, degreedetails, 0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }
    protected void getPrintSettings2()
    {
        try
        {

            #region Excel print settings
            string usertype = "";
            if (userCode.Trim() != "")
                usertype = " and usercode='" + userCode + "'";
            else if (groupUserCode.Trim() != "")
                usertype = " and group_code='" + groupUserCode + "'";
            string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
            if (printset != "")
            {
                if (printset.Contains("E"))
                {
                    lblrptname2.Visible = true;
                    txtexcelname2.Visible = true;
                    btnExcel2.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed2.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname2.Visible = true;
                    txtexcelname2.Visible = true;
                    btnExcel2.Visible = true;
                    btnprintmasterhed2.Visible = true;

                }
            }
            #endregion
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    #endregion

    #region alertclose
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterial"); }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {


    }
    #endregion

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
            BindLibrary(LibCollection);
        }
        catch (Exception ex)
        {
        }
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

    protected void BtnClear_OnClick(object sender, EventArgs e)
    {
        DivNonBookpopup.Visible = true;
        ddl_mat.Attributes.Add("onfocus", "frelig5()");
        ddcurrency.Attributes.Add("onfocus", "frelig6()");
        AutoAccno();
        BindLibrary1();
        LoadAttache();
        LoadBudHead();
        GetCurrencyType();
        loadmonth();
        loaddepartment();
        btn_save_Non_book.Visible = true;
        txDate_Acc.Attributes.Add("readonly", "readonly");
        txDate_Acc.Text = DateTime.Now.ToString("dd/MM/yyyy");
        ddl_Library_SelectedIndexChanged(sender, e);
        supp();
    }

}