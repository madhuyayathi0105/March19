using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InsproDataAccess;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class LibraryMod_Book_Availability : System.Web.UI.Page
{

    #region Field_Declaration
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    Dictionary<string, string> dicStaffList = new Dictionary<string, string>();
    Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    Dictionary<string, string> dictitle = new Dictionary<string, string>();
    DataSet dsprint = new DataSet();
    ArrayList colord = new ArrayList();
    DAccess2 da = new DAccess2();
    DataView dvhead = new DataView();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    DataTable dtCommon = new DataTable();
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string collcode = string.Empty;
    string libcode = string.Empty;
    string libname = string.Empty;
    string activerow = "";
    string activecol = "";
    int selectedcount = 0;
    DataTable bokavail = new DataTable();
    static string searchlibcode = string.Empty;
    static int searchtype = 0;
    static int searchby = 0;
    DataRow drbok;
    Boolean pageno = false;
    DataRow drbokaccess;
    int ivalue = 0;
    int curpage = 0;
    double pageSize1 = 0.0;
    int pagecnt = 0;
    int pgsize = 0;
    int s = 0;
    static bool firstRow = false;
    bool Remove = false;
    DataTable access = new DataTable();
    int selectedCellIndex = 0;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
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
            loadissuetype();
            grdBookAvail.Visible = false;
            //rptprint.Visible = false;
            txt_accno.Text = "";
            txt_Title.Text = "";

        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearchaccno(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> values = new List<string>();



        query = "SELECT DISTINCT  TOP  100 acc_no FROM bookdetails where acc_no Like '" + prefixText + "%' AND Lib_Code ='" + searchlibcode + "' order by acc_no";




        values = ws.Getname(query);
        return values;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearchtitle(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> values = new List<string>();



        query = "SELECT DISTINCT  TOP  100 title FROM bookdetails where title Like '" + prefixText + "%' AND Lib_Code ='" + searchlibcode + "' order by title";




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
        if (searchtype == 0 || searchtype == 6)
        {
            if (searchby == 1)
            {


                query = "SELECT DISTINCT  TOP  100 acc_no FROM bookdetails where acc_no Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by acc_no";

            }
            else if (searchby == 2)
            {

                query = "SELECT DISTINCT  TOP  100 title FROM bookdetails where title Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by title";

            }
            else if (searchby == 3)
            {

                query = "SELECT DISTINCT  TOP  100 Author FROM bookdetails where Author Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by Author";

            }
        }
        if (searchtype == 1)
        {
            if (searchby == 1)
            {


                query = "SELECT DISTINCT  TOP  100 access_code FROM journal where access_code Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by access_code";

            }
            else if (searchby == 2)
            {

                query = "SELECT DISTINCT  TOP  100 title FROM journal where title Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by title";

            }
            else if (searchby == 3)
            {


                query = "SELECT DISTINCT  TOP  100 journal_code FROM journal where journal_code Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by journal_code";

            }
        }
        if (searchtype == 2)
        {
            if (searchby == 1)
            {


                query = "SELECT DISTINCT  TOP  100 probook_accno FROM project_book where probook_accno Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by probook_accno";

            }
            else if (searchby == 2)
            {

                query = "SELECT DISTINCT  TOP  100 title FROM project_book where title Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by title";

            }
            else if (searchby == 3)
            {

                query = "SELECT DISTINCT  TOP  100 name FROM project_book where name Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by name";

            }
        }
        if (searchtype == 3)
        {
            if (searchby == 1)
            {


                query = "SELECT DISTINCT  TOP  100 NonBookMat_No FROM NonBookMat where NonBookMat_No Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by NonBookMat_No";

            }
            else if (searchby == 2)
            {

                query = "SELECT DISTINCT  TOP  100 Title FROM NonBookMat where Title Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by Title";

            }
            else if (searchby == 3)
            {

                query = "SELECT DISTINCT  TOP  100 Author FROM NonBookMat where Author Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by Author";

            }
        }
        if (searchtype == 4)
        {
            if (searchby == 1)
            {


                query = "SELECT DISTINCT  TOP  100 Access_Code FROM University_Question where Access_Code Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by Access_Code";

            }
            else if (searchby == 2)
            {

                query = "SELECT DISTINCT  TOP  100 Title FROM University_Question where Title Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by Title";

            }
            else if (searchby == 3)
            {

                query = "SELECT DISTINCT  TOP  100 Paper_Name FROM University_Question where Paper_Name Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by Paper_Name";

            }
        }
        if (searchtype == 5)
        {
            if (searchby == 1)
            {


                query = "SELECT DISTINCT  TOP  100 Access_Code FROM Back_Volume where Access_Code Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by Access_Code";

            }
            else if (searchby == 2)
            {

                query = "SELECT DISTINCT  TOP  100 Title FROM Back_Volume where Title Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by Title";

            }
            else if (searchby == 3)
            {

                query = "SELECT DISTINCT  TOP  100 periodicalname FROM Back_Volume where periodicalname Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by periodicalname";

            }
        }
        values = ws.Getname(query);
        return values;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearchauthor(string prefixText)
    {
        string query1 = "";
        WebService ws1 = new WebService();
        List<string> valuess = new List<string>();



        query1 = "SELECT DISTINCT  TOP  100 author FROM bookdetails where author Like '" + prefixText + "%' AND Lib_Code ='" + searchlibcode + "' order by author";




        valuess = ws1.Getname(query1);
        return valuess;
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

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Availability"); }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdBookAvail.Visible = false;
        //rptprint.Visible = false;
        txt_accno.Text = "";
        txt_Title.Text = "";
        getLibPrivil();
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
                string lib = "select lib_name,lib_code,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCollection + " AND college_code=" + College + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = da.select_method_wo_parameter(lib, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary.DataSource = ds;
                    ddllibrary.DataTextField = "lib_name";
                    ddllibrary.DataValueField = "lib_code";
                    ddllibrary.DataBind();

                    searchlibcode = Convert.ToString(ddllibrary.SelectedValue);
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Availability"); }



    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdBookAvail.Visible = false;
            //rptprint.Visible = false;
            txt_accno.Text = "";
            txt_Title.Text = "";

            searchlibcode = Convert.ToString(ddllibrary.SelectedValue);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Availability"); }



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
                ds = da.select_method_wo_parameter(sql, "text");
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
                        ds = da.select_method_wo_parameter(sql, "text");
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
                        ds = da.select_method_wo_parameter(sql, "text");
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

    #endregion

    #region IssueType
    public void loadissuetype()
    {

        try
        {
            ddl_issuetype.Items.Clear();
            ddl_issuetype.Items.Add("Book");
            ddl_issuetype.Items.Add("Periodicals");
            ddl_issuetype.Items.Add("Project Books");
            ddl_issuetype.Items.Add("Non Book Materials");
            ddl_issuetype.Items.Add("Question Bank");
            ddl_issuetype.Items.Add(" Back Volume");
            ddl_issuetype.Items.Add("Reference Books");


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Availability"); }



    }

    protected void ddl_issuetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdBookAvail.Visible = false;
        //rptprint.Visible = false;
        txt_accno.Text = "";
        txt_Title.Text = "";
        searchtype = ddl_issuetype.SelectedIndex;
    }
    #endregion

    #region btn_Accno
    protected void btn_accno_Click(object sender, EventArgs e)
    {
        try
        {
            dd_search.Items.Clear();
            popupselectBook.Visible = true;
            if (ddl_issuetype.SelectedIndex == 0 || ddl_issuetype.SelectedIndex == 2 || ddl_issuetype.SelectedIndex == 6)
            {
                dd_search.Items.Add("All");
                dd_search.Items.Add("Access Number");
                dd_search.Items.Add("Title");
                dd_search.Items.Add("Author");

            }
            else if (ddl_issuetype.SelectedIndex == 1)
            {
                dd_search.Items.Add("All");
                dd_search.Items.Add("Access Number");
                dd_search.Items.Add("Title");
                dd_search.Items.Add("Journal Code");
            }
            else if (ddl_issuetype.SelectedIndex == 4)
            {
                dd_search.Items.Add("All");
                dd_search.Items.Add("Access Number");
                dd_search.Items.Add("Title");

            }
            else
            {
                dd_search.Items.Add("All");
                dd_search.Items.Add("Access Number");
                dd_search.Items.Add("Title");
                dd_search.Items.Add("NonBook No");

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Availability"); }

    }
    #endregion

    #region Select_Accno

    protected void dd_search_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdBookAccess.Visible = false;

            btn_Acc_exit1.Visible = false;
            if (dd_search.Text == "All")
                txt_search_book.Visible = false;
            else
                txt_search_book.Visible = true;

            searchby = dd_search.SelectedIndex;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Availability"); }


    }

    protected void btn_go_book_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsgetaccno = new DataSet();
            string search1 = "";
            if (dd_search.Items.Count > 0)
                search1 = Convert.ToString(dd_search.SelectedValue);
            if (search1 != "" && search1 != "All")
            {
                if (txt_search_book.Text == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Enter " + search1 + "";
                    return;
                }
            }
            dsgetaccno = getaccessnodetails();
            if (dsgetaccno.Tables.Count > 0 && dsgetaccno.Tables[0].Rows.Count > 0)
            {
                loadspreadaccnodetails(dsgetaccno);
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Records Found";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Availability"); }

    }

    #endregion

    #region Load_Access_No

    protected void grdBookAccess_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {

                if (ddl_issuetype.SelectedIndex == 0 || ddl_issuetype.SelectedIndex == 2 || ddl_issuetype.SelectedIndex == 4 || ddl_issuetype.SelectedIndex == 5 || ddl_issuetype.SelectedIndex == 6)
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                }
                if (ddl_issuetype.SelectedIndex == 1)
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = true;
                    e.Row.Cells[4].Visible = false;
                }
                if (ddl_issuetype.SelectedIndex == 3)
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = true;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ddl_issuetype.SelectedIndex == 0 || ddl_issuetype.SelectedIndex == 2 || ddl_issuetype.SelectedIndex == 4 || ddl_issuetype.SelectedIndex == 5 || ddl_issuetype.SelectedIndex == 6)
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                }
                if (ddl_issuetype.SelectedIndex == 1)
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = true;
                    e.Row.Cells[4].Visible = false;
                }
                if (ddl_issuetype.SelectedIndex == 3)
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = true;
                }

            }
        }
        catch (Exception ex)
        {
        }

    }

    protected void grdBookAccess_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void grdBookAccess_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            string title1 = "";
            string accno1 = "";
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);



            Label txttitle1 = (Label)grdBookAccess.Rows[rowIndex].FindControl("lbl_title");
            if (txttitle1.Text.Trim() != "")
            {
                title1 = txttitle1.Text.Trim();
            }
            Label txtaccno1 = (Label)grdBookAccess.Rows[rowIndex].FindControl("lbl_accessno");
            if (txtaccno1.Text.Trim() != "")
            {
                accno1 = txtaccno1.Text.Trim();
            }
            txt_accno.Text = accno1;
            txt_Title.Text = title1;
            popupselectBook.Visible = false;
        }

        catch
        {
        }
    }

    protected void grdBookAccess_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        grdBookAccess.PageIndex = e.NewPageIndex;
        getaccessnodetails();
    }

    private DataSet getaccessnodetails()
    {
        DataSet dsload2 = new DataSet();
        try
        {
            #region get Value

            string sqlgetaccno = "";
            string search = "";
            string libcode = "";
            string searchaccno = "";
            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (dd_search.Items.Count > 0)
                search = Convert.ToString(dd_search.SelectedValue);
            if (search != "All")
            {
                if (txt_search_book.Text != "")
                {
                    if (ddl_issuetype.SelectedIndex == 0 || ddl_issuetype.SelectedIndex == 6)
                    {
                        if (search == "Access Number")
                            searchaccno = "and acc_no='" + txt_search_book.Text + "'";
                        else if (search == "Title")
                            searchaccno = "and title='" + txt_search_book.Text + "'";
                        else
                            searchaccno = "and Author='" + txt_search_book.Text + "'";
                    }
                    else if (ddl_issuetype.SelectedIndex == 1)
                    {
                        if (search == "Access Number")
                            searchaccno = "and access_code='" + txt_search_book.Text + "'";
                        else if (search == "Title")
                            searchaccno = "and title='" + txt_search_book.Text + "'";
                        else
                            searchaccno = "and journal_code='" + txt_search_book.Text + "'";
                    }
                    else if (ddl_issuetype.SelectedIndex == 2)
                    {
                        if (search == "Access Number")
                            searchaccno = "and probook_accno='" + txt_search_book.Text + "'";
                        else if (search == "Title")
                            searchaccno = "and title='" + txt_search_book.Text + "'";
                        else
                            searchaccno = "and name='" + txt_search_book.Text + "'";

                    }
                    else if (ddl_issuetype.SelectedIndex == 4 || ddl_issuetype.SelectedIndex == 5)
                    {
                        if (search == "Access Number")
                            searchaccno = "and access_code='" + txt_search_book.Text + "'";
                        else
                            searchaccno = "and title='" + txt_search_book.Text + "'";

                    }
                    else
                    {
                        if (search == "Access Number")
                            searchaccno = "and acc_no='" + txt_search_book.Text + "'";
                        else if (search == "Title")
                            searchaccno = "and title='" + txt_search_book.Text + "'";
                        else
                            searchaccno = "and nonbookmat_no='" + txt_search_book.Text + "'";

                    }

                }
            }
            if (!string.IsNullOrEmpty(collcode) && !string.IsNullOrEmpty(libcode))
            {
                if (ddl_issuetype.SelectedIndex == 0 || ddl_issuetype.SelectedIndex == 6)
                    sqlgetaccno = "select acc_no,title,author,acc_no as dup_acc_no from bookdetails where lib_code='" + libcode + "' and ref = 'No' and book_status = 'Available' " + searchaccno + " order by acc_no";
                else if (ddl_issuetype.SelectedIndex == 1)
                    sqlgetaccno = "select access_code,journal_code,title,access_code as dup_acc_no from journal where lib_code='" + libcode + "'  and issue_flag ='Available' " + searchaccno + " order by journal_code";
                else if (ddl_issuetype.SelectedIndex == 2)
                    sqlgetaccno = "select probook_accno,title,name,probook_accno as dup_acc_no from project_book where lib_code='" + libcode + "'  and issue_flag ='Available' " + searchaccno + " order by probook_accno";
                else if (ddl_issuetype.SelectedIndex == 3)
                    sqlgetaccno = "select nonbookmat_no,acc_no,title,nonbookmat_no as dup_acc_no from nonbookmat where lib_code='" + libcode + "' and  issue_flag='Available' " + searchaccno + " order by acc_no";
                else if (ddl_issuetype.SelectedIndex == 4)
                    sqlgetaccno = "select access_code,title,title,access_code as dup_acc_no from university_question where lib_code='" + libcode + "' and issue_flag ='Available'  " + searchaccno + " order by access_code";
                else
                    sqlgetaccno = "select access_code,title,title,access_code as dup_acc_no from back_volume where lib_code='" + libcode + "' and issue_flag='Available'  " + searchaccno + " order by access_code";
            }
            dsload2.Clear();
            dsload2 = d2.select_method_wo_parameter(sqlgetaccno, "Text");

            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Availability"); }


        return dsload2;


    }

    public void loadspreadaccnodetails(DataSet ds)
    {
        try
        {
            LoadHeader();
            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            int sno = 0;
            string id = "";
            string accno = "";
            string title = "";
            string jcode = "";
            string nonbono = "";
            int i = 0;
            double rowcount = 0.0;
            double pagecn = 0.0;

            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {

                sno++;
                drbokaccess = access.NewRow();
                if (ddl_issuetype.SelectedIndex == 0 || ddl_issuetype.SelectedIndex == 6)
                {
                    accno = Convert.ToString(ds.Tables[0].Rows[row]["acc_no"]).Trim();
                    title = Convert.ToString(ds.Tables[0].Rows[row]["title"]).Trim();
                }
                else if (ddl_issuetype.SelectedIndex == 1)
                {
                    accno = Convert.ToString(ds.Tables[0].Rows[row]["access_code"]).Trim();
                    title = Convert.ToString(ds.Tables[0].Rows[row]["title"]).Trim();
                    jcode = Convert.ToString(ds.Tables[0].Rows[row]["journal_code"]);
                }
                else if (ddl_issuetype.SelectedIndex == 2)
                {
                    accno = Convert.ToString(ds.Tables[0].Rows[row]["probook_accno"]).Trim();
                    title = Convert.ToString(ds.Tables[0].Rows[row]["title"]).Trim();
                }
                else if (ddl_issuetype.SelectedIndex == 3)
                {
                    accno = Convert.ToString(ds.Tables[0].Rows[row]["acc_no"]).Trim();
                    title = Convert.ToString(ds.Tables[0].Rows[row]["title"]).Trim();
                    nonbono = Convert.ToString(ds.Tables[0].Rows[row]["nonbookmat_no"]).Trim();
                }
                else
                {
                    accno = Convert.ToString(ds.Tables[0].Rows[row]["access_code"]).Trim();
                    title = Convert.ToString(ds.Tables[0].Rows[row]["title"]).Trim();
                }

                drbokaccess["Access No"] = accno;
                drbokaccess["Title"] = title;

                if (ddl_issuetype.SelectedIndex == 1)
                {
                    drbokaccess["Journal Code"] = jcode;

                }
                if (ddl_issuetype.SelectedIndex == 3)
                {
                    drbokaccess["Non Book No"] = nonbono;

                }
                access.Rows.Add(drbokaccess);
            }
            grdBookAccess.DataSource = access;
            grdBookAccess.DataBind();
            grdBookAccess.Visible = true;
            for (int l = 0; l < grdBookAccess.Rows.Count; l++)
            {
                foreach (GridViewRow row in grdBookAccess.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        grdBookAccess.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdBookAccess.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                        grdBookAccess.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Left;
                        grdBookAccess.Rows[l].Cells[3].HorizontalAlign = HorizontalAlign.Left;
                        grdBookAccess.Rows[l].Cells[4].HorizontalAlign = HorizontalAlign.Left;
                    }
                }
            }
            btn_Acc_exit1.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Availability"); }
    }

    public void LoadHeader()
    {

        try
        {
            access.Columns.Add("Access No");
            access.Columns.Add("Title");
            access.Columns.Add("Journal Code");
            access.Columns.Add("Non Book No");
        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Availability"); }

    }

    protected void btn_Acc_exit1_Click(object sender, EventArgs e)
    {
        try
        {
            popupselectBook.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Availability"); }


    }

    #endregion

    #region Go

    protected void grdBookAvail_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {

        }

        catch
        {
        }
    }

    protected void grdBookAvail_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        grdBookAvail.PageIndex = e.NewPageIndex;
        btngo_Click(sender, e);
    }

    protected void btngo_Click(object sender, EventArgs e)
    {

        try
        {
            #region get Value

            DataSet dsgo = new DataSet();
            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            string getrecord = "";



            if (libcode != "")
            {
                if (txt_accno.Text == "" && txt_Title.Text == "" && txtauthor.Text == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter Accno Or Title";
                    return;

                }
                if (txt_accno.Text != "" && txt_Title.Text != "")
                    getrecord = "select * from bookdetails where acc_no  ='" + txt_accno.Text + "'  and title  like'" + txt_Title.Text + "%' and lib_code='" + libcode + "'";
                else if (txt_accno.Text == "" && txt_Title.Text != "")
                    getrecord = "select * from bookdetails where title ='" + txt_Title.Text + "' and lib_code='" + libcode + "'";
                else if (txt_accno.Text == "" && txtauthor.Text != "")
                    getrecord = "select * from bookdetails where Author ='" + txtauthor.Text + "' and lib_code='" + libcode + "'";
                else if (txt_accno.Text != "")
                    getrecord = "select * from bookdetails where acc_no  ='" + txt_accno.Text + "'  and title  like'" + txt_Title.Text + "%' and lib_code='" + libcode + "'";

            }
            dsgo.Clear();
            dsgo = d2.select_method_wo_parameter(getrecord, "Text");

            if (dsgo.Tables.Count > 0 && dsgo.Tables[0].Rows.Count > 0)
            {
                bokavail.Columns.Add("SNo", typeof(string));
                bokavail.Columns.Add("Acc No", typeof(string));
                bokavail.Columns.Add("Title", typeof(string));
                bokavail.Columns.Add("Author", typeof(string));
                bokavail.Columns.Add("Status", typeof(string));

               


                int sno = 0;
                string srollno = "";
                string sname = "";
                if (dsgo.Tables.Count > 0 && dsgo.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < dsgo.Tables[0].Rows.Count; row++)
                    {
                        sno++;
                        drbok = bokavail.NewRow();


                        string raccno = Convert.ToString(dsgo.Tables[0].Rows[row]["Acc_No"]).Trim();
                        string rtitle = Convert.ToString(dsgo.Tables[0].Rows[row]["Title"]).Trim();
                        string rauthor = Convert.ToString(dsgo.Tables[0].Rows[row]["Author"]).Trim();
                        string rsta = Convert.ToString(dsgo.Tables[0].Rows[row]["book_status"]);
                        drbok["SNo"] = Convert.ToString(sno);
                        drbok["Acc No"] = raccno;
                        drbok["Title"] = rtitle;
                        drbok["Author"] = rauthor;
                        drbok["Status"] = rsta;
                        bokavail.Rows.Add(drbok);


                    }
                    grdBookAvail.DataSource = bokavail;
                    grdBookAvail.DataBind();
                    grdBookAvail.Visible = true;
                    rptprint.Visible = true;


                    for (int l = 0; l < grdBookAvail.Rows.Count; l++)
                    {
                        foreach (GridViewRow row in grdBookAvail.Rows)
                        {
                            foreach (TableCell cell in row.Cells)
                            {
                                grdBookAvail.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                grdBookAvail.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;

                            }
                        }
                    }

                   
                }
                if (grdBookAvail.Rows.Count > 0)
                {

                    CheckBox selectall = grdBookAvail.Rows[0].FindControl("selectall") as CheckBox;
                    selectall.Visible = true;
                    CheckBox select = grdBookAvail.Rows[0].FindControl("select") as CheckBox;
                    select.Visible = false;

                }


            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";

            }

            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Availability"); }


    }

  
    #endregion

    #region Print

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Book_Availability";
            string pagename = "Book_Availability.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(grdBookAvail, pagename, degreedetails, 0, ss,"1");
            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Availability"); }

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdBookAvail, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book_Availability"); }

    }

    public override void VerifyRenderingInServerForm(Control control)
    { }
    #endregion

    #region Close
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        popupselectBook.Visible = false;
    }

    #endregion


}