using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Drawing;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;


public partial class LibraryMod_BookPhoto : System.Web.UI.Page
{
    string usercollegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string groupusercode = string.Empty;
    string college_code = string.Empty;
    string lib_code = string.Empty;
    string accno = string.Empty;
    string title = string.Empty;
    string author = string.Empty;
    string phpath = string.Empty;
    string journalname = string.Empty;
    string typofproject = string.Empty;
    string periodicalname = string.Empty;
    bool flag_true = false;
    string access_no = string.Empty;
    string Book = string.Empty;
    byte[] photoid;
    int size;
    int Result = 0;
    int ACTROW = 0;
    string book_type = string.Empty;
    SqlConnection ssql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    DataTable dtCommon = new DataTable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    Hashtable ht = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    Boolean pageno = false;
    int ivalue = 0;
    int curpage = 0;
    double pageSize1 = 0.0;
    int pagecnt = 0;
    int pgsize = 0;
    int selectedpage = 0;
    static int first = 0;
    static int searchby = 0;
    static int searchbybooktype = 0;
    static string searchclgcode = string.Empty;
    static string searchlibcode = string.Empty;
    DataTable bokpho = new DataTable();
    DataRow drbokp;
    static string BkPhtAccNo = "";

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
            
                usercollegecode = Session["collegecode"] != null ? Convert.ToString(Session["collegecode"]) : "";
                usercode = Session["usercode"] != null ? Convert.ToString(Session["usercode"]) : "";
                singleuser = Session["single_user"] != null ? Convert.ToString(Session["single_user"]) : "";
                groupusercode = Session["group_code"] != null ? Convert.ToString(Session["group_code"]) : "";
           
            if (!IsPostBack)
            {
                bindclg();
                getLibPrivil();
                booktype();
            }
            fulstudp.Visible = false;
            Browsefile_div.Visible = false;
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
        }
        catch
        {
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearch(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> values = new List<string>();
        if (searchbybooktype == 0)
        {
            if (searchby == 1)
            {


                query = "SELECT DISTINCT  TOP  100 Acc_No FROM BookDetails where Acc_No Like '" + prefixText + "%' AND Lib_Code ='" + searchlibcode + "' order by Acc_No";

            }
            else if (searchby == 2)
            {

                query = "SELECT DISTINCT  TOP  100 Title FROM BookDetails where Title Like '" + prefixText + "%' AND Lib_Code ='" + searchlibcode + "' order by Title";

            }
            else if (searchby == 3)
            {

                query = "SELECT DISTINCT  TOP  100 Author FROM BookDetails where Author Like '" + prefixText + "%' AND Lib_Code ='" + searchlibcode + "' order by Author";

            }
        }
        if (searchbybooktype == 1)
        {
            if (searchby == 1)
            {


                query = "SELECT DISTINCT  TOP  100 Journal_Code FROM Journal where Journal_Code Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by Journal_Code";

            }
            else if (searchby == 2)
            {

                query = "SELECT DISTINCT  TOP  100 Journal_Name FROM Journal_Master where Journal_Name Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by Journal_Name";

            }
            else if (searchby == 3)
            {


                query = "SELECT DISTINCT  TOP  100 Title FROM Journal where Title Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by Title";

            }
        }
        if (searchbybooktype == 2)
        {
            if (searchby == 1)
            {


                query = "SELECT DISTINCT  TOP  100 ProBook_AccNo FROM Project_Book where ProBook_AccNo Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by ProBook_AccNo";

            }
            else if (searchby == 2)
            {

                query = "SELECT DISTINCT  TOP  100 Title FROM Project_Book where Title Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by Title";

            }
            else if (searchby == 3)
            {

                query = "SELECT DISTINCT  TOP  100 Type_of_Project FROM Project_Book where Type_of_Project Like '" + prefixText + "%' AND lib_code ='" + searchlibcode + "' order by Type_of_Project";

            }
        }
        if (searchbybooktype == 3)
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
        if (searchbybooktype == 4)
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
        if (searchbybooktype == 5)
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

    #region BindHeaders

    public void bindclg()
    {
        try
        {

            ddlclg.Items.Clear();
            dtCommon.Clear();

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
                ddlclg.DataSource = dtCommon;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
                ddlclg.SelectedIndex = 0;
                ddlclg.Enabled = true;
            }


            //ddlclg.Items.Clear();
            //string columnfield = string.Empty;
            //string group_user = Session["collegecode"] != null ? Convert.ToString(Session["collegecode"]) : "";
            //if (group_user.Contains(";"))
            //{
            //    string[] groupsemi = group_user.Split(';');
            //    group_user = Convert.ToString(groupsemi[0]);
            //}
            //if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            //{
            //    columnfield = " and group_code='" + group_user + "'";
            //}
            //else if (Session["usercode"] != null)
            //{
            //    columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            //}
            //ht.Clear();
            //ht.Add("column_field", Convert.ToString(columnfield));
            //ds = da.select_method("bind_college", ht, "sp");
            //ddlclg.Items.Clear();
            //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    ddlclg.DataSource = ds;
            //    ddlclg.DataValueField = "college_code";
            //    ddlclg.DataTextField = "collname";
            //    ddlclg.DataBind();
            //    ddlclg.SelectedIndex = 0;
            //}

        }
        catch
        {
        }
    }

    public void bindlibrary(string LibCollection)
    {
        try
        {
            ddllibrary.Items.Clear();
            ds.Clear();
            if (ddlclg.Items.Count > 0)
            {
                foreach (ListItem li in ddlclg.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(college_code))
                        {
                            college_code = "'" + li.Value + "'";
                        }
                        else
                        {
                            college_code = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(college_code))
            {
                string lib = "select lib_name,lib_code,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library  " + LibCollection + " AND college_code=" + college_code + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = da.select_method_wo_parameter(lib, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary.DataSource = ds;
                    ddllibrary.DataValueField = "lib_code";
                    ddllibrary.DataTextField = "lib_name";
                    ddllibrary.DataBind();
                    searchlibcode = Convert.ToString(ddllibrary.SelectedValue);
                }
            }
        }
        catch
        {
        }
    }
   
    public void binddept()
    {
        try
        {
            ddlsearch.Items.Clear();
            ds.Clear();
            if (ddlclg.Items.Count > 0)
            {
                foreach (ListItem li in ddlclg.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(college_code))
                        {
                            college_code = "'" + li.Value + "'";
                        }
                        else
                        {
                            college_code = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(college_code))
            {
                string dept = "Select Distinct Dept_Name from Journal_Dept where dept_name <> '' and college_code =" + college_code + "";
                ds = da.select_method_wo_parameter(dept, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlsearch.DataSource = ds;
                    ddlsearch.DataTextField = "Dept_Name";
                    ddlsearch.DataValueField = "Dept_Name";
                    ddlsearch.DataBind();
                    ddlsearch.Items.Insert(0, "All");

                }
            }
        }
        catch
        {
        }
    }
    
    public void booktype()
    {
        if (ddlbooktype.SelectedIndex == 0 || ddlbooktype.SelectedIndex == 3)
        {
            ddlsearchby.Visible = true;
            ddlsearchby1.Visible = false;
            ddlsearchby2.Visible = false;
            ddlsearchby4.Visible = false;
            ddlsearchby5.Visible = false;
            ddlsearchby.SelectedIndex = 0;
        }

        else if (ddlbooktype.SelectedIndex == 1)
        {
            ddlsearchby.Visible = false;
            ddlsearchby1.Visible = true;
            ddlsearchby2.Visible = false;
            ddlsearchby4.Visible = false;
            ddlsearchby5.Visible = false;
            ddlsearchby1.SelectedIndex = 0;
        }
        else if (ddlbooktype.SelectedIndex == 2)
        {
            ddlsearchby.Visible = false;
            ddlsearchby1.Visible = false;
            ddlsearchby2.Visible = true;
            ddlsearchby4.Visible = false;
            ddlsearchby5.Visible = false;
            ddlsearchby2.SelectedIndex = 0;
        }
        else if (ddlbooktype.SelectedIndex == 4)
        {
            ddlsearchby.Visible = false;
            ddlsearchby1.Visible = false;
            ddlsearchby2.Visible = false;
            ddlsearchby4.Visible = true;
            ddlsearchby5.Visible = false;
            ddlsearchby4.SelectedIndex = 0;
        }
        else if (ddlbooktype.SelectedIndex == 5)
        {
            ddlsearchby.Visible = false;
            ddlsearchby1.Visible = false;
            ddlsearchby2.Visible = false;
            ddlsearchby4.Visible = false;
            ddlsearchby5.Visible = true;
            ddlsearchby5.SelectedIndex = 0;

        }

    }
    
    public void searchty()
    {
        if (ddlsearchby.SelectedIndex == 1 || ddlsearchby.SelectedIndex == 2 || ddlsearchby.SelectedIndex == 3)
        {
            txtserach.Visible = true;
            ddlsearch.Visible = false;
        }
        else if (ddlsearchby.SelectedIndex == 4)
        {
            ddlsearch.Visible = true;
            txtserach.Visible = false;
            binddept();
        }
        else
        {
            txtserach.Visible = false;
            ddlsearch.Visible = false;
        }
    }

    public void getLibPrivil()
    {
        try
        {
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddlclg.SelectedValue);
            string sql = "";
            string GrpUserVal = "";
            string GrpCode = "";
            string LibCollection = "";
            Hashtable hsLibcode = new Hashtable();
            if (singleuser.ToLower() == "true")
            {
                sql = "SELECT DISTINCT lib_code from lib_privileges where user_code=" + usercode + " and lib_code in (select lib_code from library where college_code=" + coll_Code + ")";
                ds.Clear();
                ds = da.select_method_wo_parameter(sql, "text");
            }
            else
            {
                string[] groupUser = groupusercode.Split(';');
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

        bindlibrary(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }
    

    #endregion

    public void libcode()
    {
        string libraryname = Convert.ToString(ddllibrary.SelectedValue).Trim();

        if (ddlclg.Items.Count > 0)
        {
            foreach (ListItem li in ddlclg.Items)
            {
                if (li.Selected)
                {
                    if (string.IsNullOrEmpty(college_code))
                    {
                        college_code = "'" + li.Value + "'";
                    }
                    else
                    {
                        college_code = ",'" + li.Value + "'";
                    }
                }
            }
        }
        if (!string.IsNullOrEmpty(college_code))
        {
            string libcode = "select lib_name,lib_code from library where college_code=" + college_code + " and lib_name='" + libraryname + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(libcode, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                lib_code = Convert.ToString(ds.Tables[0].Rows[0]["lib_code"]);
            }
        }

    }

    protected void ddlclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        getLibPrivil();
    }

    protected void ddllibrary_OnSelectedIndexedChange(object sender, EventArgs e)
    {
        searchlibcode = Convert.ToString(ddllibrary.SelectedValue);
    }

    protected void ddlbooktype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        booktype();
        searchty();
        txtserach.Visible = false;
        ddlsearch.Visible = false;
        searchbybooktype = ddlbooktype.SelectedIndex;
    }

    protected void ddlstatus_selectedindexchange(object sender, EventArgs e)
    {
    }

    protected void ddlsearchby_selectedindexchange(object sender, EventArgs e)
    {
        searchty();
        searchby = ddlsearchby.SelectedIndex;
    }

    protected void ddlsearchby1_selectedindexchange(object sender, EventArgs e)
    {
        if (ddlsearchby1.SelectedIndex == 1 || ddlsearchby1.SelectedIndex == 2 || ddlsearchby1.SelectedIndex == 3)
        {
            txtserach.Visible = true;
            ddlsearch.Visible = false;
        }
        else if (ddlsearchby1.SelectedIndex == 4)
        {
            ddlsearch.Visible = true;
            txtserach.Visible = false;
            binddept();
        }
        else
        {
            txtserach.Visible = false;
            ddlsearch.Visible = false;
        }
        searchby = ddlsearchby1.SelectedIndex;
    }

    protected void ddlsearchby2_selectedindexchange(object sender, EventArgs e)
    {
        if (ddlsearchby2.SelectedIndex == 1 || ddlsearchby2.SelectedIndex == 2 || ddlsearchby2.SelectedIndex == 3)
        {
            txtserach.Visible = true;
            ddlsearch.Visible = false;
        }
        else if (ddlsearchby2.SelectedIndex == 4)
        {
            ddlsearch.Visible = true;
            txtserach.Visible = false;
            binddept();
        }
        else
        {
            txtserach.Visible = false;
            ddlsearch.Visible = false;
        }
        searchby = ddlsearchby2.SelectedIndex;
    }

    protected void ddlsearchby4_selectedindexchange(object sender, EventArgs e)
    {
        if (ddlsearchby4.SelectedIndex == 1 || ddlsearchby4.SelectedIndex == 2 || ddlsearchby4.SelectedIndex == 3)
        {
            txtserach.Visible = true;
            ddlsearch.Visible = false;
        }
        else if (ddlsearchby4.SelectedIndex == 4)
        {
            ddlsearch.Visible = true;
            txtserach.Visible = false;
            binddept();
        }
        else
        {
            txtserach.Visible = false;
            ddlsearch.Visible = false;
        }
        searchby = ddlsearchby4.SelectedIndex;
    }

    protected void ddlsearchby5_selectedindexchange(object sender, EventArgs e)
    {
        if (ddlsearchby5.SelectedIndex == 1 || ddlsearchby5.SelectedIndex == 2 || ddlsearchby5.SelectedIndex == 3)
        {
            txtserach.Visible = true;
            ddlsearch.Visible = false;
        }
        else if (ddlsearchby5.SelectedIndex == 4)
        {
            ddlsearch.Visible = true;
            txtserach.Visible = false;
            binddept();
        }
        else
        {
            txtserach.Visible = false;
            ddlsearch.Visible = false;
        }
        searchby = ddlsearchby5.SelectedIndex;
    }

    protected void ddlsearch_selectedindexchange(object sender, EventArgs e)
    {
    }

    protected void grdBookPhoto_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ddlbooktype.SelectedItem.Text == "Book" || ddlbooktype.SelectedItem.Text == "Non-Book Material")
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = true;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = true;
                e.Row.Cells[9].Visible = true;

            }
            else if (ddlbooktype.SelectedItem.Text == "Periodicals")
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = true;
                e.Row.Cells[9].Visible = true;

            }
            else if (ddlbooktype.SelectedItem.Text == "Project Book")
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = true;
                e.Row.Cells[9].Visible = true;

            }
            else if (ddlbooktype.SelectedItem.Text == "Question Bank")
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = true;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = true;
                e.Row.Cells[9].Visible = true;

            }
            else if (ddlbooktype.SelectedItem.Text == "Back Volume")
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = true;
                e.Row.Cells[8].Visible = true;
                e.Row.Cells[9].Visible = true;

            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (ddlbooktype.SelectedItem.Text == "Book" || ddlbooktype.SelectedItem.Text == "Non-Book Material")
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = true;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = true;
                e.Row.Cells[9].Visible = true;

            }
            else if (ddlbooktype.SelectedItem.Text == "Periodicals")
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = true;
                e.Row.Cells[9].Visible = true;

            }
            else if (ddlbooktype.SelectedItem.Text == "Project Book")
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = true;
                e.Row.Cells[9].Visible = true;

            }
            else if (ddlbooktype.SelectedItem.Text == "Question Bank")
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = true;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = true;
                e.Row.Cells[9].Visible = true;

            }
            else if (ddlbooktype.SelectedItem.Text == "Back Volume")
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[2].Visible = true;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = true;
                e.Row.Cells[8].Visible = true;
                e.Row.Cells[9].Visible = true;

            }
        }
    }

    protected void grdBookPhoto_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        grdBookPhoto.PageIndex = e.NewPageIndex;
        btngo_click(sender, e);
    }

    protected void btngo_click(object sender, EventArgs e)//rrrr
    {
        try
        {
            string qry = string.Empty;
            libcode();
            grdBookPhoto.Visible = true;
            lib_code = Convert.ToString(ddllibrary.SelectedValue);
            if (ddlbooktype.SelectedIndex == 0)
            {
                qry = "Select B.Acc_No ,Title,Author,B.Lib_Code From BookDetails B Left Join BookPhoto P On B.Acc_No = P.Acc_No Where 1=1 And B.Lib_code ='" + lib_code + "'";

                if (ddlstatus.SelectedIndex == 1)
                {
                    qry = qry + " AND Photo is not null";
                }
                else if (ddlstatus.SelectedIndex == 2)
                {
                    qry = qry + " AND Photo is null";
                }
                if (ddlsearchby.SelectedIndex == 1)
                {
                    qry = qry + "  And B.Acc_No Like '" + txtserach.Text + "%'";
                }
                else if (ddlsearchby.SelectedIndex == 2)
                {
                    qry = qry + " And Title Like '" + txtserach.Text + "%'";
                }
                else if (ddlsearchby.SelectedIndex == 3)
                {
                    qry = qry + " And Author Like '" + txtserach.Text + "%'";
                }
                else if (ddlsearchby.SelectedIndex == 4)
                {

                    if (ddlsearch.SelectedIndex != 0)
                    {
                        qry = qry + " And Dept_Code Like '" + Convert.ToString(ddlsearch.SelectedItem.Text) + "'";
                    }
                }
                qry = qry + " Order By Len(B.Acc_No),B.Acc_No";
            }
            else if (ddlbooktype.SelectedIndex == 1)
            {
                qry = "Select j.Journal_Code ,m.Journal_Name,Title,j.Lib_Code From Journal J INNER JOIN Journal_Master M ON J.Journal_Code = M.Journal_Code Left Join BookPhoto P On J.Journal_Code = P.Acc_No Where 1=1 And j.Lib_code ='" + lib_code + "'";


                if (ddlsearchby.SelectedIndex == 1)
                {
                    qry = qry + "  And J.Journal_Code Like '" + txtserach.Text + "%'";
                }
                else if (ddlsearchby.SelectedIndex == 2)
                {
                    qry = qry + " And Title Like '" + txtserach.Text + "%'";
                }
                else if (ddlsearchby.SelectedIndex == 3)
                {
                    qry = qry + " And m.Journal_Name Like '" + txtserach.Text + "%'";
                }
                else if (ddlsearchby.SelectedIndex == 4)
                {
                    if (ddlsearch.SelectedIndex != 0)
                    {
                        qry = qry + " And Dept_Name Like '" + Convert.ToString(ddlsearch.SelectedItem.Text) + "%'";
                    }
                }
                qry = qry + " Order By j.Journal_Code";
            }
            else if (ddlbooktype.SelectedIndex == 2)
            {
                qry = "Select ProBook_AccNo ,Title,type_of_project,j.Lib_Code From Project_Book J Left Join BookPhoto P On J.ProBook_AccNo = P.Acc_No Where 1=1 And j.Lib_code ='" + lib_code + "'";


                if (ddlsearchby.SelectedIndex == 1)
                {
                    qry = qry + "   And ProBook_AccNo Like '" + txtserach.Text + "%'";
                }
                else if (ddlsearchby.SelectedIndex == 2)
                {
                    qry = qry + " And Title Like '" + txtserach.Text + "%'";
                }
                else if (ddlsearchby.SelectedIndex == 3)
                {
                    qry = qry + " And Type_of_Project Like '" + txtserach.Text + "%'";
                }

                qry = qry + " Order By ProBook_AccNo";

            }
            else if (ddlbooktype.SelectedIndex == 3)
            {
                qry = "Select NonBookMat_No ,Title,Author,N.Lib_Code From NonBookMat N Left Join BookPhoto P On N.NonBookMat_No = P.Acc_No Where 1=1 And N.Lib_code ='" + lib_code + "'";
                if (ddlsearchby.SelectedIndex == 1)
                {
                    qry = qry + " And NonBookMat_No Like '" + txtserach.Text + "%'";
                }
                else if (ddlsearchby.SelectedIndex == 2)
                {
                    qry = qry + " And Title Like '" + txtserach.Text + "%'";
                }
                else if (ddlsearchby.SelectedIndex == 3)
                {
                    qry = qry + " And Author Like '" + txtserach.Text + "%'";
                }
                else if (ddlsearchby.SelectedIndex == 4)
                {
                    if (ddlsearch.SelectedIndex != 0)
                    {
                        qry = qry + " And Department Like '" + Convert.ToString(ddlsearch.SelectedItem.Text) + "'";
                    }
                }
                qry = qry + " Order By NonBookMat_No";
            }
            else if (ddlbooktype.SelectedIndex == 4)
            {
                qry = "Select Access_Code  ,Title,Paper_Name,Q.Lib_Code From University_Question Q Left Join BookPhoto P On Q.Access_Code = P.Acc_No Where 1=1 And Q.Lib_code ='" + lib_code + "'";
                if (ddlsearchby.SelectedIndex == 1)
                {
                    qry = qry + " And Access_Code Like '" + txtserach.Text + "%'";
                }
                else if (ddlsearchby.SelectedIndex == 2)
                {
                    qry = qry + " And Title Like '" + txtserach.Text + "%'";
                }
                else if (ddlsearchby.SelectedIndex == 3)
                {
                    qry = qry + " And Paper_Name Like '" + txtserach.Text + "%'";
                }
                else if (ddlsearchby.SelectedIndex == 4)
                {
                    if (ddlsearch.SelectedIndex != 0)
                    {
                        qry = qry + " And Dept Like '" + Convert.ToString(ddlsearch.SelectedItem.Text) + "'";
                    }
                }
                qry = qry + " Order By Access_Code";

            }
            else if (ddlbooktype.SelectedIndex == 5)
            {
                qry = "Select Access_Code,Title,periodicalname,B.Lib_Code From Back_Volume B Left Join BookPhoto P On B.Access_Code = P.Acc_No Where 1=1 And B.Lib_code  ='" + lib_code + "'";
                if (ddlsearchby.SelectedIndex == 1)
                {
                    qry = qry + " And Access_Code Like '" + txtserach.Text + "%'";
                }
                else if (ddlsearchby.SelectedIndex == 2)
                {
                    qry = qry + " And Title Like '" + txtserach.Text + "%'";
                }
                else if (ddlsearchby.SelectedIndex == 3)
                {
                    qry = qry + " And periodicalname Like '" + txtserach.Text + "%'";
                }
                qry = qry + " Order By Access_Code";
            }
            ds.Clear();
            ds = da.select_method_wo_parameter(qry, "text");


            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                bokpho.Columns.Add("Access No");
                bokpho.Columns.Add("Title");
                bokpho.Columns.Add("Author");
                bokpho.Columns.Add("Journal Name");
                bokpho.Columns.Add("Type Of Project");
                bokpho.Columns.Add("Paper Name");
                bokpho.Columns.Add("Periodical Name");
                bokpho.Columns.Add("Photo Path");
                bokpho.Columns.Add("Select Photo");
                int sno = 0;
                int ii = 0;
                for (int row = ii; row < ds.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    drbokp = bokpho.NewRow();

                    if (ddlbooktype.SelectedIndex == 0)
                    {
                        accno = Convert.ToString(ds.Tables[0].Rows[row]["Acc_No"]);
                        title = Convert.ToString(ds.Tables[0].Rows[row]["Title"]);
                        author = Convert.ToString(ds.Tables[0].Rows[row]["Author"]);
                    }
                    else if (ddlbooktype.SelectedIndex == 1)
                    {
                        accno = Convert.ToString(ds.Tables[0].Rows[row]["Journal_Code"]);
                        title = Convert.ToString(ds.Tables[0].Rows[row]["Journal_Name"]);
                        author = Convert.ToString(ds.Tables[0].Rows[row]["Title"]);
                    }
                    else if (ddlbooktype.SelectedIndex == 2)
                    {
                        accno = Convert.ToString(ds.Tables[0].Rows[row]["ProBook_AccNo"]);
                        title = Convert.ToString(ds.Tables[0].Rows[row]["Title"]);
                        author = Convert.ToString(ds.Tables[0].Rows[row]["type_of_project"]);

                    }
                    else if (ddlbooktype.SelectedIndex == 3)
                    {
                        accno = Convert.ToString(ds.Tables[0].Rows[row]["NonBookMat_No"]);
                        title = Convert.ToString(ds.Tables[0].Rows[row]["Title"]);
                        author = Convert.ToString(ds.Tables[0].Rows[row]["Author"]);
                    }
                    else if (ddlbooktype.SelectedIndex == 4)
                    {
                        accno = Convert.ToString(ds.Tables[0].Rows[row]["Access_Code"]);
                        title = Convert.ToString(ds.Tables[0].Rows[row]["Title"]);
                        author = Convert.ToString(ds.Tables[0].Rows[row]["Paper_Name"]);
                    }
                    else if (ddlbooktype.SelectedIndex == 5)
                    {
                        accno = Convert.ToString(ds.Tables[0].Rows[row]["Access_Code"]);
                        title = Convert.ToString(ds.Tables[0].Rows[row]["Title"]);
                        author = Convert.ToString(ds.Tables[0].Rows[row]["periodicalname"]);
                    }
                    DataSet ds1 = new DataSet();
                    string path1 = "select photo from BookPhoto where acc_no='" + accno + "'";
                    ds1.Clear();
                    ds1 = da.select_method_wo_parameter(path1, "text");
                    if (ddlstatus.SelectedIndex == 1)
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            phpath = ds1.Tables[0].Rows[0]["photo"].ToString();
                            phpath = "Selected";
                        }
                    }
                    else if (ddlstatus.SelectedIndex == 2)
                    {
                        phpath = "Not Selected";
                    }
                    if (ddlstatus.SelectedIndex == 0)
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            phpath = ds1.Tables[0].Rows[0]["photo"].ToString();
                            phpath = "Selected";
                        }
                        else
                        {
                            phpath = "Not Selected";
                        }
                    }
                    drbokp["Access No"] = accno;
                    drbokp["Title"] = title;
                    drbokp["Author"] = author;
                    drbokp["Photo Path"] = phpath;
                    bokpho.Rows.Add(drbokp);
                }
                grdBookPhoto.DataSource = bokpho;
                grdBookPhoto.DataBind();
                grdBookPhoto.Visible = true;
                divtable.Visible = true;
                for (int l = 0; l < grdBookPhoto.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdBookPhoto.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdBookPhoto.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            grdBookPhoto.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                            grdBookPhoto.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Left;
                            grdBookPhoto.Rows[l].Cells[3].HorizontalAlign = HorizontalAlign.Left;
                            grdBookPhoto.Rows[l].Cells[4].HorizontalAlign = HorizontalAlign.Left;
                            grdBookPhoto.Rows[l].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
                divPopupAlert.Visible = false;
                divAlertContent.Visible = false;
                lblAlertMsg.Visible = false;
                btnPopAlertClose.Visible = false;
            }
            else
            {
                grdBookPhoto.Visible = false;
                divtable.Visible = false;

                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
                btnPopAlertClose.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        lblAlertMsg.Text = string.Empty;
        lblAlertMsg.Visible = false;
        divPopupAlert.Visible = false;
        lblAlertMsg.Text = string.Empty;
    }  

    protected void btn_upload_click(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {
        }
    }

    protected void BtnsaveStud_Click(object sender, EventArgs e)
    {
        string ffg = fulstudp.FileName;

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
            string path = Server.MapPath("~/Import Files/" + System.IO.Path.GetFileName(fulstudp.FileName));
            // int rowIndex = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;


            string pathname = string.Empty;
            libcode();
            access_no = BkPhtAccNo;
            pathname = path;



            photoid = (byte[])(ViewState["bookimage"]);
            size = Convert.ToInt32(ViewState["size"]);

            Browsefile_div.Visible = false;
            string bokimageurl = string.Empty;
            string BookType = Convert.ToString(ddlbooktype.SelectedItem.Text);

            if (BookType == "Book")
                Book = "BOK";
            if (BookType == "Periodicals")
                Book = "PER";
            if (BookType == "Project Book")
                Book = "PRO";
            if (BookType == "Non-Book Material")
                Book = "NBM";
            if (BookType == "Question Bank")
                Book = "QBA";
            if (BookType == "Back Volume")
                Book = "BVO";
            //if (BookType == "Reference Books")
            //    Book = "REF";
            bookphotosave(access_no, lib_code, Book, size, photoid);


        }
        if (Result > 0)
        {
            divPopupAlert.Visible = true;
            divAlertContent.Visible = true;
            lblAlertMsg.Visible = true;
            lblAlertMsg.Text = "Records Saved Successfully";
            btnPopAlertClose.Visible = true;
            imgstudp.ImageUrl = null;

        }
        else
        {
            divPopupAlert.Visible = true;
            divAlertContent.Visible = true;
            lblAlertMsg.Visible = true;
            lblAlertMsg.Text = "Records Not Saved";
            btnPopAlertClose.Visible = true;
            imgstudp.ImageUrl = null;
            Browsefile_div.Visible = false;

        }

    }

    protected void btn_edit_click(object sender, EventArgs e)
    {
        int rowindex = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;
        string acc = "";
        if (grdBookPhoto.Rows.Count > 0)
        {
            Label access = (Label)grdBookPhoto.Rows[rowindex].FindControl("lbl_accno");
            if (access.Text.Trim() != "")
            {
                acc = access.Text.Trim();
            }
           
            BkPhtAccNo = acc;
            Browsefile_div.Visible = true;
            imagebtn.Visible = true;
            div1.Visible = true;
            fulstudp.Visible = true;


        }
    }

    public void booktype1()
    {
        if (ddlbooktype.SelectedIndex == 0)
        {
            book_type = "BOK";
        }
        else if (ddlbooktype.SelectedIndex == 1)
        {
            book_type = "PER";
        }
        else if (ddlbooktype.SelectedIndex == 2)
        {
            book_type = "PRO";
        }
        else if (ddlbooktype.SelectedIndex == 3)
        {
            book_type = "NBM";
        }
        else if (ddlbooktype.SelectedIndex == 4)
        {
            book_type = "QBA";
        }
        else if (ddlbooktype.SelectedIndex == 5)
        {
            book_type = "BVO";
        }

    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        Browsefile_div.Visible = false;
    }

    protected void bookphotosave(string accnumber, string librarycode, string book_type, int Filesize, byte[] DocDocument)
    {

        try
        {
            lib_code = Convert.ToString(ddllibrary.SelectedValue);
            if (accnumber.Trim() != "" && lib_code.Trim() != "" && book_type.Trim() != "" & Filesize != 0)
            {
                string InsPhoto = "if exists(select Acc_No,photo,lib_code,Book_Type from BookPhoto where Acc_No='" + access_no + "' and Lib_Code ='" + lib_code + "' and Book_Type ='" + book_type + "')update BookPhoto set photo='" + size + "' where Acc_No='" + access_no + "' and Lib_Code ='" + lib_code + "' and Book_Type ='" + book_type + "' else insert into BookPhoto (Acc_No,photo,lib_code,Book_Type) values('" + access_no + "','" + size + "','" + lib_code + "','" + book_type + "')";
                SqlCommand cmd = new SqlCommand(InsPhoto, ssql);
                SqlParameter uploadedsubject_name = new SqlParameter("size", SqlDbType.Binary, Filesize);
                uploadedsubject_name.Value = DocDocument;
                cmd.Parameters.Add(uploadedsubject_name);
                ssql.Close();
                ssql.Open();
                Result = cmd.ExecuteNonQuery();
                ssql.Close();
            }
        }
        catch
        {
        }
    }
}