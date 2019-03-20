using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;

public partial class LibraryMod_BookStatistic : System.Web.UI.Page
{
    #region Field Declaration

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
    string qryCollege = string.Empty;
    string qry = string.Empty;
    string qryj = string.Empty;
    string sqlbv = string.Empty;
    string sqlnb = string.Empty;
    string sqlp = string.Empty;
    string sqlqb = string.Empty;
    string dept = string.Empty;
    string library = string.Empty;
    string selQ = string.Empty;
    string booktype = string.Empty;
    string typ = string.Empty;
    string categor = string.Empty;
    string qrylibraryFilter = string.Empty;
    string qrytxtbooksFilter = string.Empty;
    string qryrefbooksFilter = string.Empty;
    string qrytxtrefbooksFilter = string.Empty;
    string qrydeptfilter = string.Empty;
    string qrycatfilter = string.Empty;
    string qrysub = string.Empty;
    string subject = string.Empty;
    string qrysubfilter = string.Empty;
    string qrybookfilter = string.Empty;
    string infromdate = string.Empty;
    string intodate = string.Empty;
    string accfromdate = string.Empty;
    string acctodate = string.Empty;
    string accessfrom = string.Empty;
    string accessto = string.Empty;
    string pricefrom = string.Empty;
    string priceto = string.Empty;
    string qryinvoicefilter = string.Empty;
    string qryAccessionfilter = string.Empty;
    string qrytitlefilter = string.Empty;
    string Titlewise = string.Empty;
    string Author = string.Empty;
    string qryauthorfilter = string.Empty;
    string qrypricefilter = string.Empty;
    static int selected_dept_sub_wise = 0;
    static int selected_title_author_type = 0;
    string coltext;
    string Department;
    string Subject;
    string NoofTitle;
    string NoofVolume;
    string price;
    string Title;
    string tottitle;
    string totvol;
    string totprice;
    double totaldisp;
    double tot;
    int insdex;
    Hashtable hasprice;
    Hashtable hastitle;
    Hashtable hasvol;
    Boolean pageno = false;
    int ivalue = 0;
    int curpage = 0;
    double pageSize1 = 0.0;
    int pagecnt = 0;
    int pgsize = 0;
    int selectedpage = 0;
    static int first = 0;

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
                Department_Load();
                books();
                type();
                category();
                typewise();
                wise();
                //getPrintSettings();


            }

        }
        catch
        { }
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }
        {
        }
    }

    #endregion

    #region Library

    public void BindLibrary(string LibCollection)
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
                    ddllibrary.Items.Insert(0, "All");
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }
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

            BindLibrary(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region Department
    public void Department_Load()
    {
        try
        {
            Hashtable hat = new Hashtable();
            cbl_department.Items.Clear();
            cb_department.Checked = false;
            txt_department.Text = "---Select---";
            string College = ddlCollege.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(College))
            {
                hat.Add("collegecode", College);
                ds.Clear();
                ds = da.select_method("LoadJournalDepartment", hat, "sp");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_department.DataSource = ds;
                    cbl_department.DataTextField = "Dept_Name";
                    cbl_department.DataValueField = "Dept_Name";
                    cbl_department.DataBind();
                    if (cbl_department.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_department.Items.Count; i++)
                        {
                            cbl_department.Items[i].Selected = true;
                        }
                        txt_department.Text = "Department(" + cbl_department.Items.Count + ")";
                        cb_department.Checked = true;
                    }
                }

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
    }

    protected void cb_department_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_department, cbl_department, txt_department, "Department", "--Select--");
            showreport2.Visible = false;
            //print.Visible = false;


        }
        catch (Exception ex)
        {
        }
    }

    protected void cbl_department_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_department, cbl_department, txt_department, "Department", "--Select--");
            showreport2.Visible = false;
            //print.Visible = false;

        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region books
    public void books()
    {
        try
        {
            //ddlbooks.Items.Add("All");
            ddlbooks.Items.Add("Back Volumes");
            ddlbooks.Items.Add("Books");
            ddlbooks.Items.Add("Journals");
            ddlbooks.Items.Add("NonBook Materials");
            ddlbooks.Items.Add("Project Books");
            ddlbooks.Items.Add("Question Bank");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }
    }
    #endregion

    #region type
    public void type()
    {
        try
        {
            ddltype.Items.Add("All");
            ddltype.Items.Add("References Books");
            ddltype.Items.Add("Text Books");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }
    }
    #endregion

    #region category
    public void category()
    {
        try
        {
            //ddlcategory.Items.Add("All");
            //ddlcategory.Items.Add("Book Bank");
            //ddlcategory.Items.Add("Library Books");

            ds.Clear();
            string library = ddllibrary.SelectedValue.ToString();

            string strqur = " select distinct category from bookdetails";
            if (library != "All")
            {
                strqur += " AND bookdetails.Lib_Code='" + library + "'";
            }

            ds.Clear();
            ds = d2.select_method_wo_parameter(strqur, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcategory.DataSource = ds;
                ddlcategory.DataTextField = "category";
                ddlcategory.DataValueField = "category";
                ddlcategory.DataBind();
                ddlcategory.Items.Insert(0, "All");
            }


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }
    }
    #endregion

    #region wise
    public void wise()
    {
        try
        {
            rblwise.Items.Add("Departmentwise");
            rblwise.Items.Add("Subjectwise");
            rblwise.Items.FindByText("Departmentwise").Selected = true;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }
    }
    #endregion

    #region typewise
    public void typewise()
    {
        try
        {
            rblType.Items.Add("TitleWise");
            rblType.Items.Add("Title & Author");
            rblType.Items.FindByText("TitleWise").Selected = true;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }
    }
    #endregion

    #region Index Changed Events

    # region autosearch
    # region Getrno
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();

        try
        {

            string query = "";

            WebService ws = new WebService();

            {
                string txtval = string.Empty;

                if (selected_dept_sub_wise == 0)
                {

                    query = "select distinct Dept_Code from bookdetails where Dept_Code like '" + prefixText + "%'  order by Dept_Code";
                }
                else if (selected_dept_sub_wise == 1)
                {


                    query = "select distinct subject from bookdetails where subject like '" + prefixText + "%' order by subject";
                }

            }
            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    # endregion

    # region Getrno1
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno1(string prefixText)
    {
        List<string> book = new List<string>();

        try
        {

            string qry = "";

            WebService ws = new WebService();

            if (selected_title_author_type == 0)
            {

                qry = "select distinct Title from bookdetails where Title like '" + prefixText + "%'  order by Title";
            }
            else if (selected_title_author_type == 1)
            {
                qry = "select distinct Title +'--'+ author as TitleAuthor from bookdetails where Title like '%'  order by TitleAuthor";
            }
            book = ws.Getname(qry);
            return book;
        }
        catch { return book; }
    }
    # endregion
    #endregion

    # region dropdown
    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            showreport2.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }


    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            showreport2.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }

    }

    protected void ddldept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            showreport2.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }

    }

    protected void ddlbooks_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cbdate.Checked = false;
            showreport2.Visible = false;
            cbdate2.Checked = false;
            //if (ddlbooks.SelectedIndex == 0)
            //{
            //    lbltype.Visible = false;
            //    ddltype.Visible = false;
            //    lblcategory.Visible = false;
            //    ddlcategory.Visible = false;
            //    cbdate.Enabled = false;
            //    cbdate2.Enabled = false;
            //    cbdate1.Visible = true;
            //    txt_todate.Enabled = false;
            //    txt_fromdate.Enabled = false;
            //    txt_fromdate2.Enabled = false;
            //    txt_todate2.Enabled = false;

            //}
            if (ddlbooks.SelectedIndex == 0)
            {
                lbltype.Enabled = false;
                ddltype.Enabled = false;
                lblcategory.Visible = false;
                ddlcategory.Enabled = false;
                cbdate.Enabled = false;
                cbdate2.Enabled = false;
                cbdate1.Visible = true;
                txt_todate.Enabled = false;
                txt_fromdate.Enabled = false;
                txt_fromdate2.Enabled = false;
                txt_todate2.Enabled = false;
            }
            if (ddlbooks.SelectedIndex == 1)
            {
                // lbltype.Visible = true;
                ddltype.Enabled = true;
                // lblcategory.Visible = true;
                ddlcategory.Enabled = true;
                cbdate.Enabled = true;
                cbdate2.Enabled = true;
                cbdate1.Visible = true;
                txt_todate.Enabled = false;
                txt_fromdate.Enabled = false;
                txt_fromdate2.Enabled = false;
                txt_todate2.Enabled = false;
                txt_fromdate1.Enabled = false;
                txt_todate.Enabled = false;

            }
            if (ddlbooks.SelectedIndex == 2)
            {
                lbltype.Enabled = false;
                ddltype.Enabled = false;


                cbdate.Enabled = false;
                cbdate2.Enabled = false;
                cbdate1.Visible = true;
                txt_todate.Enabled = false;
                txt_fromdate.Enabled = false;
                txt_fromdate2.Enabled = false;
                txt_todate2.Enabled = false;

            }
            if (ddlbooks.SelectedIndex == 3)
            {
                lbltype.Enabled = false;
                ddltype.Enabled = false;
                lblcategory.Visible = false;
                ddlcategory.Enabled = false;
                cbdate.Enabled = false;
                cbdate2.Enabled = false;
                cbdate1.Visible = true;
                txt_todate.Enabled = false;
                txt_fromdate.Enabled = false;
                txt_fromdate2.Enabled = false;
                txt_todate2.Enabled = false;

            }
            if (ddlbooks.SelectedIndex == 4)
            {
                lbltype.Enabled = false;
                ddltype.Enabled = false;
                lblcategory.Visible = false;
                ddlcategory.Enabled = false;
                cbdate.Enabled = false;
                cbdate2.Enabled = false;
                cbdate1.Visible = true;
                txt_todate.Enabled = false;
                txt_fromdate.Enabled = false;
                txt_fromdate2.Enabled = false;
                txt_todate2.Enabled = false;

            }
            if (ddlbooks.SelectedIndex == 5)
            {
                lbltype.Enabled = false;
                ddltype.Enabled = false;
                lblcategory.Visible = false;
                ddlcategory.Enabled = false;
                cbdate.Enabled = false;
                cbdate2.Enabled = false;
                cbdate1.Visible = true;
                txt_todate.Enabled = false;
                txt_fromdate.Enabled = false;
                txt_fromdate2.Enabled = false;
                txt_todate2.Enabled = false;

            }


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }

    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        showreport2.Visible = false;
        try
        {

            if (ddlbooks.SelectedIndex == 1)
            {
                lbltype.Visible = true;
                ddltype.Visible = true;
                lblcategory.Visible = true;
                ddlcategory.Visible = true;
            }


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }

    }

    protected void ddlcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        showreport2.Visible = false;
        try
        {
            if (ddlbooks.SelectedIndex == 1)
            {
                lbltype.Visible = true;
                ddltype.Visible = true;
                lblcategory.Visible = true;
                ddlcategory.Visible = true;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }

    }
    # endregion

    # region radiobutton
    protected void rblwise_Selected(object sender, EventArgs e)
    {
        showreport2.Visible = false;
        try
        {
            if (rblwise.SelectedItem.Text.ToString().ToLower() == "departmentwise")
            {
                selected_dept_sub_wise = 0;
                txtsearch1.Text = "";
                if (rblType.SelectedIndex == 0)
                {
                    //cblcolumnorder.Visible = true;
                    ////cblcolumnorder1.Visible = false;
                    //cblcolumnorder2.Visible = false;
                    //cblcolumnorder3.Visible = false;
                }
                if (rblType.SelectedIndex == 1)
                {
                    //cblcolumnorder.Visible = false;
                    ////cblcolumnorder1.Visible = true;
                    //cblcolumnorder2.Visible = false;
                    //cblcolumnorder3.Visible = false;
                }

            }
            if (rblwise.SelectedItem.Text.ToString().ToLower() == "subjectwise")
            {
                selected_dept_sub_wise = 1;
                txtsearch1.Text = "";
                if (rblType.SelectedIndex == 0)
                {
                    //cblcolumnorder.Visible = false;
                    ////cblcolumnorder1.Visible = false;
                    //cblcolumnorder2.Visible = true;
                    // cblcolumnorder3.Visible = false;

                }
                if (rblType.SelectedIndex == 1)
                {
                    //cblcolumnorder.Visible = false;
                    //// cblcolumnorder1.Visible = false;
                    //cblcolumnorder2.Visible = false;
                    // cblcolumnorder3.Visible = true;


                }
            }


        }
        catch (Exception ex)
        {
        }

    }

    protected void rblType_Selected(object sender, EventArgs e)
    {
        showreport2.Visible = false;

        try
        {

            if (rblType.SelectedItem.Text.ToString().ToLower() == "titlewise")
            {
                selected_title_author_type = 0;
                txtsearch2.Text = "";
                if (rblwise.SelectedIndex == 0)
                {
                    // cblcolumnorder.Visible = true;
                    //cblcolumnorder1.Visible = false;
                    //cblcolumnorder2.Visible = false;
                    //cblcolumnorder3.Visible = false;
                }
                if (rblwise.SelectedIndex == 1)
                {
                    //cblcolumnorder.Visible = false;
                    //cblcolumnorder1.Visible = false;
                    //cblcolumnorder2.Visible = true;
                    //cblcolumnorder3.Visible = false;
                }
            }

            if (rblType.SelectedItem.Text.ToString().ToLower() == "title & author")
            {
                selected_title_author_type = 1;
                txtsearch2.Text = "";
                if (rblwise.SelectedIndex == 0)
                {
                    // cblcolumnorder.Visible = false;
                    //cblcolumnorder1.Visible = true;
                    // cblcolumnorder2.Visible = false;
                    //cblcolumnorder3.Visible = false;
                }
                if (rblwise.SelectedIndex == 1)
                {
                    // cblcolumnorder.Visible = false;
                    // cblcolumnorder1.Visible = false;
                    // cblcolumnorder2.Visible = false;
                    //cblcolumnorder3.Visible = true;
                }
            }
            if (rblType.SelectedIndex == 1)
            {
                lbltype.Visible = true;
                ddltype.Visible = true;
                lblcategory.Visible = true;
                ddlcategory.Visible = true;
            }
            if (rblType.SelectedIndex == 1)
            {
                ddlbooks.Items.Clear();
                //ddlbooks.Items.Add("Books");
                books();
            }
            if (rblType.SelectedIndex == 0)
            {
                ddlbooks.Items.Clear();
                books();
            }
        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }

    }
    # endregion

    # region textbox
    protected void txtsearch1_TextChanged(object sender, EventArgs e)
    {
        showreport2.Visible = false;
        //try
        //{
        //    string txtval = txtsearch1.Text;
        //    if (rblwise.SelectedIndex == 0)
        //    {

        //        Getrno(txtval);
        //    }
        //    if (rblwise.SelectedIndex == 1)
        //    {

        //        Getrnowise(txtval);
        //    }
        //}
        //catch (Exception ex)
        //{
        //}

    }

    protected void txtsearch2_TextChanged(object sender, EventArgs e)
    {
        showreport2.Visible = false;
        try
        {

        }
        catch (Exception ex)
        {
        }

    }
    # endregion

    # region checkbox
    protected void cbdate1_OnCheckedChanged(object sender, EventArgs e)
    {
        showreport2.Visible = false;
        try
        {
            if (cbdate1.Checked)
            {
                txt_fromdate1.Enabled = true;
                txt_todate1.Enabled = true;

            }
            else
            {
                txt_fromdate1.Enabled = false;
                txt_todate1.Enabled = false;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }
        {
        }

    }

    protected void cbdate_OnCheckedChanged(object sender, EventArgs e)
    {
        showreport2.Visible = false;
        try
        {
            if (cbdate.Checked)
            {
                txt_fromdate.Enabled = true;
                txt_todate.Enabled = true;

            }
            else
            {
                txt_fromdate.Enabled = false;
                txt_todate.Enabled = false;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }
        {
        }

    }

    protected void cbdate2_OnCheckedChanged(object sender, EventArgs e)
    {
        showreport2.Visible = false;
        try
        {
            if (cbdate2.Checked)
            {
                txt_fromdate2.Enabled = true;
                txt_todate2.Enabled = true;

            }
            else
            {
                txt_fromdate2.Enabled = false;
                txt_todate2.Enabled = false;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }
        {
        }

    }

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }

    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch (Exception ex) { }
    }

    #endregion
    # endregion

    # region price range

    protected void chkprice_OnCheckedChanged(object sender, EventArgs e)
    {
        showreport2.Visible = false;
        try
        {
            if (cbRange.Checked)
            {
                txtFromRange.Enabled = true;
                txtToRange.Enabled = true;

            }
            else
            {
                txtFromRange.Enabled = false;
                txtToRange.Enabled = false;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }
        {
        }

    }

    # endregion


    #endregion

    #region go
    protected void btn_go_Click(object sender, EventArgs e)
    {

        if (!cbdate1.Checked)
        {
            alertpopwindow.Visible = false;
            if (ddlbooks.SelectedIndex == 1)
            {
                DataTable dsbookdetails = new DataTable();
                dsbookdetails = statistics();
                if (dsbookdetails.Rows.Count > 0)
                {
                    loadspreadCount(dsbookdetails);
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found!";
                }
            }
            //if (ddlbooks.SelectedIndex == 0)
            //{
            //    DataSet dsbackvolumes = new DataSet();
            //    dsbackvolumes = backvolumes();
            //    if (dsbackvolumes.Tables.Count > 0 && dsbackvolumes.Tables[0].Rows.Count > 0)
            //    {
            //        loadspreadCount(dsbackvolumes);
            //    }
            //    else
            //    {
            //        alertpopwindow.Visible = true;
            //        lblalerterr.Text = "No Record Found!";
            //    }
            //}
            //if (ddlbooks.SelectedIndex == 2)
            //{
            //    DataSet dsjournals = new DataSet();
            //    dsjournals = journals();
            //    if (dsjournals.Tables.Count > 0 && dsjournals.Tables[0].Rows.Count > 0)
            //    {
            //        loadspreadCount(dsjournals);
            //    }
            //    else
            //    {
            //        alertpopwindow.Visible = true;
            //        lblalerterr.Text = "No Record Found!";
            //    }
            //}
            //if (ddlbooks.SelectedIndex == 3)
            //{
            //    DataSet dsnonbook = new DataSet();
            //    dsnonbook = nonbookmaterials();
            //    if (dsnonbook.Tables.Count > 0 && dsnonbook.Tables[0].Rows.Count > 0)
            //    {
            //        loadspreadCount(dsnonbook);
            //    }
            //    else
            //    {
            //        alertpopwindow.Visible = true;
            //        lblalerterr.Text = "No Record Found!";
            //    }
            //}
            //if (ddlbooks.SelectedIndex == 4)
            //{
            //    DataSet dsprojectbooks = new DataSet();
            //    dsprojectbooks = projectbooks();
            //    if (dsprojectbooks.Tables.Count > 0 && dsprojectbooks.Tables[0].Rows.Count > 0)
            //    {
            //        loadspreadCount(dsprojectbooks);
            //    }
            //    else
            //    {
            //        alertpopwindow.Visible = true;
            //        lblalerterr.Text = "No Record Found!";
            //    }
            //}
            //if (ddlbooks.SelectedIndex == 5)
            //{
            //    DataSet dsquestionbanks = new DataSet();
            //    dsquestionbanks = questionbanks();
            //    if (dsquestionbanks.Tables.Count > 0 && dsquestionbanks.Tables[0].Rows.Count > 0)
            //    {
            //        loadspreadCount(dsquestionbanks);
            //    }
            //    else
            //    {
            //        alertpopwindow.Visible = true;
            //        lblalerterr.Text = "No Record Found!";
            //    }
            //}
        }
        else
        {
            if (txt_fromdate1.Text == "" || txt_todate1.Text == "" || (txt_fromdate1.Text == "" && txt_todate1.Text == ""))
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Enter the Access No.";
                return;
            }
            else
            {
                alertpopwindow.Visible = false;
                if (ddlbooks.SelectedIndex == 1)
                {
                    DataTable dsbookdetails = new DataTable();
                    dsbookdetails = statistics();
                    if (dsbookdetails.Rows.Count > 0)
                    {
                        loadspreadCount(dsbookdetails);
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Found!";
                    }
                }

                //if (ddlbooks.SelectedIndex == 0)
                //{
                //    DataSet dsbackvolumes = new DataSet();
                //    dsbackvolumes = backvolumes();
                //    if (dsbackvolumes.Tables.Count > 0 && dsbackvolumes.Tables[0].Rows.Count > 0)
                //    {
                //        loadspreadCount(dsbackvolumes);
                //    }
                //    else
                //    {
                //        alertpopwindow.Visible = true;
                //        lblalerterr.Text = "No Record Found!";
                //    }
                //}
                //if (ddlbooks.SelectedIndex == 2)
                //{
                //    DataSet dsjournals = new DataSet();
                //    dsjournals = journals();
                //    if (dsjournals.Tables.Count > 0 && dsjournals.Tables[0].Rows.Count > 0)
                //    {
                //        loadspreadCount(dsjournals);
                //    }
                //    else
                //    {
                //        alertpopwindow.Visible = true;
                //        lblalerterr.Text = "No Record Found!";
                //    }
                //}
                //if (ddlbooks.SelectedIndex == 3)
                //{
                //    DataSet dsnonbook = new DataSet();
                //    dsnonbook = nonbookmaterials();
                //    if (dsnonbook.Tables.Count > 0 && dsnonbook.Tables[0].Rows.Count > 0)
                //    {
                //        loadspreadCount(dsnonbook);
                //    }
                //    else
                //    {
                //        alertpopwindow.Visible = true;
                //        lblalerterr.Text = "No Record Found!";
                //    }
                //}
                //if (ddlbooks.SelectedIndex == 4)
                //{
                //    DataSet dsprojectbooks = new DataSet();
                //    dsprojectbooks = projectbooks();
                //    if (dsprojectbooks.Tables.Count > 0 && dsprojectbooks.Tables[0].Rows.Count > 0)
                //    {
                //        loadspreadCount(dsprojectbooks);
                //    }
                //    else
                //    {
                //        alertpopwindow.Visible = true;
                //        lblalerterr.Text = "No Record Found!";
                //    }
                //}
                //if (ddlbooks.SelectedIndex == 5)
                //{
                //    DataSet dsquestionbanks = new DataSet();
                //    dsquestionbanks = questionbanks();
                //    if (dsquestionbanks.Tables.Count > 0 && dsquestionbanks.Tables[0].Rows.Count > 0)
                //    {
                //        loadspreadCount(dsquestionbanks);
                //    }
                //    else
                //    {
                //        alertpopwindow.Visible = true;
                //        lblalerterr.Text = "No Record Found!";
                //    }
                //}

            }

        }
    }
    #endregion

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void GridView1_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        btn_go_Click(sender, e);
    }

    # region statistics
    public DataTable statistics()
    {

        DataTable dsbookdetails = new DataTable();

        try
        {
            if (ddlCollege.Items.Count > 0)
                collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            if (cbl_department.Items.Count > 0)
                dept = Convert.ToString(getCblSelectedValue(cbl_department));
            if (ddllibrary.Items.Count > 0)
                library = Convert.ToString(ddllibrary.SelectedValue);
            if (ddlbooks.Items.Count > 0)
                booktype = Convert.ToString(ddlbooks.SelectedValue);
            if (ddltype.Items.Count > 0)
                typ = Convert.ToString(ddltype.SelectedValue);
            if (ddlcategory.Items.Count > 0)
                categor = Convert.ToString(ddlcategory.SelectedValue);
            // string subject = rblwise.SelectedItem.Value;

            //&& !string.IsNullOrEmpty(invoicefromdate) && !string.IsNullOrEmpty(invoicetodate) && !string.IsNullOrEmpty(accessionfromdate) && !string.IsNullOrEmpty(accessiontodate) && !string.IsNullOrEmpty(accessfromdate) && !string.IsNullOrEmpty(accesstodate)
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(library))
            {
                if (library != "All" && library != "")
                {
                    qrylibraryFilter = "and l.lib_code in('" + library + "')";
                }
                if (ddltype.SelectedIndex == 2)
                {
                    qrytxtbooksFilter = " and UPPER(ISNULL(Ref,'No')) = 'NO'";
                }
                if (ddltype.SelectedIndex == 1)
                {
                    qryrefbooksFilter = "and UPPER(ISNULL(Ref,'YES')) = 'YES'";
                }
                if (typ != "All" && typ != "")
                {
                    qrytxtrefbooksFilter = " and (UPPER(ISNULL(Ref,'No')) = 'NO' or UPPER(ISNULL(Ref,'YES')) = 'YES')";
                }

                if (dept != "All" && dept != "")
                {

                    if (rblwise.SelectedIndex == 0)
                    {
                        qrydeptfilter = " and Dept_Code in('" + dept + "')";
                        // qrysubfilter = " and subject='" + subject + "'";
                    }
                    else
                    {
                        qrysubfilter = " and subject in('" + dept + "')";
                    }
                }
                if (categor != "All" && categor != "")
                {
                    qrycatfilter = " and category='" + categor + "'";
                }



                if (cbdate.Checked)//Invoice Date
                {
                    string fromDate = txt_fromdate.Text;
                    string toDate = txt_todate.Text;
                    string[] fromdate = fromDate.Split('/');
                    string[] todate = toDate.Split('/');
                    if (fromdate.Length == 3)
                        infromdate = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();

                    if (todate.Length == 3)
                        intodate = todate[2].ToString() + "-" + todate[1].ToString() + "-" + todate[0].ToString();
                    qryinvoicefilter = "and b.bill_date between'" + infromdate + "'and '" + intodate + "'";
                }

                if (cbdate2.Checked)//Accession Date
                {
                    //if (lbl_fromdate2.Length == 3)
                    //    lbl_fromdate2 = lbl_fromdate2[1].ToString() + "/" + lbl_fromdate2[0].ToString() + "/" + lbl_fromdate2[2].ToString();
                    //string[] tdate = lbl_todate2.Split('/');
                    //if (lbl_todate2.Length == 3)
                    //    lbl_fromdate2 = lbl_todate2[1].ToString() + "/" + lbl_todate2[0].ToString() + "/" + lbl_todate2[2].ToString();
                    //qryAccessionfilter = "and b.Access_date between'" + lbl_fromdate2 + "'and '" + lbl_todate2 + "'";
                }

                if (cbdate1.Checked == true)//accessNo
                {
                    accessfrom = txt_fromdate1.Text;
                    accessto = txt_todate1.Text;

                    qrybookfilter = "and CASE WHEN IsNumeric(right(B.acc_no,1)) = 1 then cast(substring(B.acc_no,(PATINDEX('%[0-9]%',B.acc_no)),len(B.acc_no))as int) end  between '" + accessfrom + "' and '" + accessto + "'";
                }
                if (cbRange.Checked == true)//price range
                {
                    pricefrom = txtFromRange.Text;
                    priceto = txtToRange.Text;

                    qrypricefilter = "and (convert(float,b.price)>='" + pricefrom + "' and convert(float,b.price)<='" + priceto + "')";
                }
                if (ddlbooks.SelectedIndex == 1)
                {


                    if (rblwise.SelectedIndex == 0)
                    {

                        if (accessfrom.All(char.IsNumber) && accessto.All(char.IsNumber) && rblType.SelectedIndex == 0)
                        {
                            selQ = " select Lib_Name,Dept_Code,count(title) TotVol,count(distinct title) TotTitle,isnull(sum(cast(price as float)),0) price from bookdetails b inner join library l on l.lib_code = b.lib_code and L.College_Code ='" + collegeCode + "'  " + qrybookfilter + qrylibraryFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + qrydeptfilter + qrycatfilter + qryinvoicefilter + qryAccessionfilter + qrypricefilter + " group by lib_name,dept_code order by lib_name,dept_code";

                            //distinct
                        }

                        dsbookdetails.Clear();
                        dsbookdetails = dirAcc.selectDataTable(selQ);

                    }

                    else if (rblwise.SelectedIndex == 1)
                    {
                        if (accessfrom.All(char.IsNumber) && accessto.All(char.IsNumber) && rblType.SelectedIndex == 0)
                        {
                            qrysub = "select Lib_Name,isnull(subject,'') subject,count(title) TotVol,count(distinct title) TotTitle,isnull(sum(cast(price as float)),0) price from bookdetails b inner join library l on l.lib_code = b.lib_code where 1=1 AND L.College_Code ='" + collegeCode + "'  " + qrybookfilter + qrylibraryFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + qrysubfilter + qrycatfilter + qryinvoicefilter + qryAccessionfilter + qrytitlefilter + qryauthorfilter + qrypricefilter + " group by lib_name,subject order by lib_name,subject";//subject
                        }


                        dsbookdetails.Clear();
                        dsbookdetails = dirAcc.selectDataTable(qrysub);

                    }

                }
            }
        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }
        return dsbookdetails;
    }

    public DataSet backvolumes()
    {
        DataSet dsbackvolumes = new DataSet();
        try
        {
            if (ddlCollege.Items.Count > 0)
                collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            if (cbl_department.Items.Count > 0)
                dept = Convert.ToString(getCblSelectedValue(cbl_department));
            if (ddllibrary.Items.Count > 0)
                library = Convert.ToString(ddllibrary.SelectedValue);
            if (ddlbooks.Items.Count > 0)
                booktype = Convert.ToString(ddlbooks.SelectedValue);
            if (ddltype.Items.Count > 0)
                typ = Convert.ToString(ddltype.SelectedValue);
            if (ddlcategory.Items.Count > 0)
                categor = Convert.ToString(ddlcategory.SelectedValue);


            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(library))
            {
                if (library != "All" && library != "")
                {
                    qrylibraryFilter = "and l.lib_code='" + library + "'";
                }
                if (ddltype.SelectedIndex == 2)
                {
                    qrytxtbooksFilter = " and UPPER(ISNULL(Ref,'No')) = 'NO'";
                }
                if (ddltype.SelectedIndex == 1)
                {
                    qryrefbooksFilter = "and UPPER(ISNULL(Ref,'YES')) = 'YES'";
                }
                if (typ != "All" && typ != "")
                {
                    qrytxtrefbooksFilter = " and (UPPER(ISNULL(Ref,'No')) = 'NO' or UPPER(ISNULL(Ref,'YES')) = 'YES')";
                }
                if (dept != "All" && dept != "")
                {
                    qrydeptfilter = " and dept_name in('" + dept + "')";
                }
                if (subject != "All" && subject != "")
                {
                    qrysubfilter = " and subject='" + subject + "'";
                }
                if (categor != "All" && categor != "")
                {
                    qrycatfilter = " and category='" + categor + "'";
                }
                if (Titlewise != "All" && Titlewise != "")
                {
                    qrytitlefilter = " and Titlewise='" + Titlewise + "'";
                }
                if (Author != "All" && Author != "")
                {
                    qryauthorfilter = " and Author='" + Author + "'";
                }

                if (cbdate.Checked)//Invoice Date
                {
                    string fromDate = txt_fromdate.Text;
                    string toDate = txt_todate.Text;
                    string[] fromdate = fromDate.Split('/');
                    string[] todate = toDate.Split('/');
                    if (fromdate.Length == 3)
                        infromdate = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();

                    if (todate.Length == 3)
                        intodate = todate[2].ToString() + "-" + todate[1].ToString() + "-" + todate[0].ToString();
                    qryinvoicefilter = "and b.bill_date between'" + infromdate + "'and '" + intodate + "'";
                }

                if (cbdate2.Checked)//Accession Date
                {
                    //if (lbl_fromdate2.Length == 3)
                    //    lbl_fromdate2 = lbl_fromdate2[1].ToString() + "/" + lbl_fromdate2[0].ToString() + "/" + lbl_fromdate2[2].ToString();
                    //string[] tdate = lbl_todate2.Split('/');
                    //if (lbl_todate2.Length == 3)
                    //    lbl_fromdate2 = lbl_todate2[1].ToString() + "/" + lbl_todate2[0].ToString() + "/" + lbl_todate2[2].ToString();
                    //qryAccessionfilter = "and b.Access_date between'" + lbl_fromdate2 + "'and '" + lbl_todate2 + "'";
                }

                if (cbdate1.Checked == true)//accessNo
                {
                    accessfrom = txt_fromdate1.Text;
                    accessto = txt_todate1.Text;

                    qrybookfilter = "and CASE WHEN IsNumeric(right(B.acc_no,1)) = 1 then cast(substring(B.acc_no,(PATINDEX('%[0-9]%',B.acc_no)),len(B.acc_no))as int) end  between '" + accessfrom + "' and '" + accessto + "'";
                }

                if (cbRange.Checked == true)//price range
                {
                    pricefrom = txtFromRange.Text;
                    priceto = txtToRange.Text;

                    //qrypricefilter = "and b.Price between '" + pricefrom + "' and '" + priceto + "'";
                    qrypricefilter = "and (convert(float,b.price)>='" + pricefrom + "' and convert(float,b.price)<='" + priceto + "')";
                }
            }
            if (ddlbooks.SelectedIndex == 0)
            {
                if (rblwise.SelectedIndex == 0)
                {

                    if (accessfrom.All(char.IsNumber) && accessto.All(char.IsNumber))
                    {
                        sqlbv = "select  Lib_Name,'' as Dept_Code,Title,count(title) TotVol,count(distinct title) TotTitle,0 as price from Back_Volume b inner join library l on l.lib_code = b.lib_code where 1=1 AND L.College_Code = '" + collegeCode + "' " + qrybookfilter + qrylibraryFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + qrydeptfilter + qrycatfilter + qryinvoicefilter + qryAccessionfilter + qrytitlefilter + qryauthorfilter + qrypricefilter + " group by lib_name,Title order by lib_name,Title";

                    }

                    dsbackvolumes.Clear();
                    dsbackvolumes = d2.select_method_wo_parameter(sqlbv, "Text");
                }

                else if (rblwise.SelectedIndex == 1)
                {
                    if (accessfrom.All(char.IsNumber) && accessto.All(char.IsNumber) && rblType.SelectedIndex == 0)
                    {
                        sqlbv = "select Lib_Name,'' as subject,count(title) TotVol,count(distinct title) TotTitle,0 as price from Back_Volume b inner join library l on l.lib_code = b.lib_code where 1=1 AND L.College_Code ='" + collegeCode + "'  " + qrylibraryFilter + qrypricefilter + " group by lib_name order by lib_name";//subject
                    }


                    dsbackvolumes.Clear();
                    dsbackvolumes = d2.select_method_wo_parameter(sqlbv, "Text");

                }
            }
        }


        catch (Exception ex)
        { }
        return dsbackvolumes;
    }

    public DataSet journals()
    {
        DataSet dsjournals = new DataSet();
        try
        {
            if (ddlCollege.Items.Count > 0)
                collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            if (cbl_department.Items.Count > 0)
                dept = Convert.ToString(getCblSelectedValue(cbl_department));
            if (ddllibrary.Items.Count > 0)
                library = Convert.ToString(ddllibrary.SelectedValue);
            if (ddlbooks.Items.Count > 0)
                booktype = Convert.ToString(ddlbooks.SelectedValue);
            if (ddltype.Items.Count > 0)
                typ = Convert.ToString(ddltype.SelectedValue);
            if (ddlcategory.Items.Count > 0)
                categor = Convert.ToString(ddlcategory.SelectedValue);


            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(library))
            {
                if (library != "All" && library != "")
                {
                    qrylibraryFilter = "and l.lib_code='" + library + "'";
                }
                if (ddltype.SelectedIndex == 2)
                {
                    qrytxtbooksFilter = " and UPPER(ISNULL(Ref,'No')) = 'NO'";
                }
                if (ddltype.SelectedIndex == 1)
                {
                    qryrefbooksFilter = "and UPPER(ISNULL(Ref,'YES')) = 'YES'";
                }
                if (typ != "All" && typ != "")
                {
                    qrytxtrefbooksFilter = " and (UPPER(ISNULL(Ref,'No')) = 'NO' or UPPER(ISNULL(Ref,'YES')) = 'YES')";
                }
                if (dept != "All" && dept != "")
                {

                    if (rblwise.SelectedIndex == 0)
                    {
                        qrydeptfilter = " and Dept_Code in('" + dept + "')";

                    }
                    else
                    {
                        qrysubfilter = " and subject in('" + dept + "')";
                    }
                }
                if (categor != "All" && categor != "")
                {
                    qrycatfilter = " and category='" + categor + "'";
                }
                if (Titlewise != "All" && Titlewise != "")
                {
                    qrytitlefilter = " and Titlewise='" + Titlewise + "'";
                }
                if (Author != "All" && Author != "")
                {
                    qryauthorfilter = " and Author='" + Author + "'";
                }

                if (cbdate.Checked)//Invoice Date
                {
                    string fromDate = txt_fromdate.Text;
                    string toDate = txt_todate.Text;
                    string[] fromdate = fromDate.Split('/');
                    string[] todate = toDate.Split('/');
                    if (fromdate.Length == 3)
                        infromdate = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();

                    if (todate.Length == 3)
                        intodate = todate[2].ToString() + "-" + todate[1].ToString() + "-" + todate[0].ToString();
                    qryinvoicefilter = "and b.bill_date between'" + infromdate + "'and '" + intodate + "'";
                }

                if (cbdate2.Checked)//Accession Date
                {


                    qryAccessionfilter = "and b.Access_date between'" + lbl_fromdate2 + "'and '" + lbl_todate2 + "'";
                }

                if (cbdate1.Checked == true)//accessNo
                {
                    accessfrom = txt_fromdate1.Text;
                    accessto = txt_todate1.Text;

                    qrybookfilter = "and CASE WHEN IsNumeric(right(B.acc_no,1)) = 1 then cast(substring(B.acc_no,(PATINDEX('%[0-9]%',B.acc_no)),len(B.acc_no))as int) end  between '" + accessfrom + "' and '" + accessto + "'";
                }
                if (cbRange.Checked == true)//price range
                {
                    pricefrom = txtFromRange.Text;
                    priceto = txtToRange.Text;

                    //qrypricefilter = "and b.Price between '" + pricefrom + "' and '" + priceto + "'";
                    qrypricefilter = "and (convert(float,b.price)>='" + pricefrom + "' and convert(float,b.price)<='" + priceto + "')";
                }
            }
            if (ddlbooks.SelectedIndex == 2)
            {


                if (rblwise.SelectedIndex == 0)
                {

                    if (accessfrom.All(char.IsNumber) && accessto.All(char.IsNumber))
                    {
                        qryj = "select  Lib_Name,Dept_Name as Dept_Code,Title,count(title) TotTitle,count(distinct title) TotVol,isnull(sum(cast(journal_price as float)),0) price from journal b inner join journal_master m on m.journal_code = b.journal_code inner join library l on l.lib_code = b.lib_code where 1=1 AND L.College_Code ='" + collegeCode + "' " + qrybookfilter + qrylibraryFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + qrydeptfilter + qrycatfilter + qryinvoicefilter + qryAccessionfilter + qrytitlefilter + qryauthorfilter + qrypricefilter + "group by lib_name,dept_name,Title order by lib_name,dept_name,Title";
                    }

                    dsjournals.Clear();
                    dsjournals = d2.select_method_wo_parameter(qryj, "Text");
                }
                else if (rblwise.SelectedIndex == 1)
                {
                    if (accessfrom.All(char.IsNumber) && accessto.All(char.IsNumber) && rblType.SelectedIndex == 0)
                    {
                        qryj = "select Lib_Name,Subject as subject,count(title) TotVol,count(distinct title) TotTitle,isnull(sum(cast(journal_price as float)),0) price from journal b inner join journal_master m on m.journal_code = b.journal_code inner join library l on l.lib_code = b.lib_code where 1=1 AND L.College_Code ='" + collegeCode + "'  " + qrybookfilter + qrylibraryFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + qrycatfilter + qryinvoicefilter + qryAccessionfilter + qrytitlefilter + qryauthorfilter + qrypricefilter + " group by lib_name,subject order by lib_name,subject";//subject
                    }
                    dsjournals.Clear();
                    dsjournals = d2.select_method_wo_parameter(qryj, "Text");

                }
            }
        }

        catch (Exception ex)
        { }
        return dsjournals;
    }

    public DataSet nonbookmaterials()
    {
        DataSet dsnonbook = new DataSet();
        try
        {
            if (ddlCollege.Items.Count > 0)
                collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            if (cbl_department.Items.Count > 0)
                dept = Convert.ToString(getCblSelectedValue(cbl_department));
            if (ddllibrary.Items.Count > 0)
                library = Convert.ToString(ddllibrary.SelectedValue);
            if (ddlbooks.Items.Count > 0)
                booktype = Convert.ToString(ddlbooks.SelectedValue);
            if (ddltype.Items.Count > 0)
                typ = Convert.ToString(ddltype.SelectedValue);
            if (ddlcategory.Items.Count > 0)
                categor = Convert.ToString(ddlcategory.SelectedValue);


            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(library))
            {
                if (library != "All" && library != "")
                {
                    qrylibraryFilter = "and l.lib_code in('" + library + "')";
                }
                if (ddltype.SelectedIndex == 2)
                {
                    qrytxtbooksFilter = " and UPPER(ISNULL(Ref,'No')) = 'NO'";
                }
                if (ddltype.SelectedIndex == 1)
                {
                    qryrefbooksFilter = "and UPPER(ISNULL(Ref,'YES')) = 'YES'";
                }
                if (typ != "All" && typ != "")
                {
                    qrytxtrefbooksFilter = " and (UPPER(ISNULL(Ref,'No')) = 'NO' or UPPER(ISNULL(Ref,'YES')) = 'YES')";
                }
                if (dept != "All" && dept != "")
                {

                    if (rblwise.SelectedIndex == 0)
                    {
                        qrydeptfilter = " and Dept_Code in('" + dept + "')";

                    }
                    else
                    {
                        qrysubfilter = " and Department in('" + dept + "')";
                    }
                }
                if (categor != "All" && categor != "")
                {
                    qrycatfilter = " and category='" + categor + "'";
                }
                if (Titlewise != "All" && Titlewise != "")
                {
                    qrytitlefilter = " and Titlewise='" + Titlewise + "'";
                }
                if (Author != "All" && Author != "")
                {
                    qryauthorfilter = " and Author='" + Author + "'";
                }

                if (cbdate.Checked)//Invoice Date
                {
                    string fromDate = txt_fromdate.Text;
                    string toDate = txt_todate.Text;
                    string[] fromdate = fromDate.Split('/');
                    string[] todate = toDate.Split('/');
                    if (fromdate.Length == 3)
                        infromdate = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();

                    if (todate.Length == 3)
                        intodate = todate[2].ToString() + "-" + todate[1].ToString() + "-" + todate[0].ToString();
                    qryinvoicefilter = "and b.bill_date between'" + infromdate + "'and '" + intodate + "'";
                }

                if (cbdate2.Checked)//Accession Date
                {
                    //if (lbl_fromdate2.Length == 3)
                    //    lbl_fromdate2 = lbl_fromdate2[1].ToString() + "/" + lbl_fromdate2[0].ToString() + "/" + lbl_fromdate2[2].ToString();
                    //string[] tdate = lbl_todate2.Split('/');
                    //if (lbl_todate2.Length == 3)
                    //    lbl_fromdate2 = lbl_todate2[1].ToString() + "/" + lbl_todate2[0].ToString() + "/" + lbl_todate2[2].ToString();
                    //qryAccessionfilter = "and b.Access_date between'" + lbl_fromdate2 + "'and '" + lbl_todate2 + "'";
                }

                if (cbdate1.Checked == true)//accessNo
                {
                    accessfrom = txt_fromdate1.Text;
                    accessto = txt_todate1.Text;

                    qrybookfilter = "and CASE WHEN IsNumeric(right(B.acc_no,1)) = 1 then cast(substring(B.acc_no,(PATINDEX('%[0-9]%',B.acc_no)),len(B.acc_no))as int) end  between '" + accessfrom + "' and '" + accessto + "'";
                }
                if (cbRange.Checked == true)//price range
                {
                    pricefrom = txtFromRange.Text;
                    priceto = txtToRange.Text;

                    //qrypricefilter = "and b.Price between '" + pricefrom + "' and '" + priceto + "'";
                    qrypricefilter = "and (convert(float,b.price)>='" + pricefrom + "' and convert(float,b.price)<='" + priceto + "')";
                }
            }
            if (ddlbooks.SelectedIndex == 3)
            {

                if (accessfrom.All(char.IsNumber) && accessto.All(char.IsNumber))
                {

                    sqlnb = "select distinct Lib_Name,Department as Dept_Code,count(title) TotVol,count(distinct title) TotTitle,isnull(sum(cast(price as float)),0) price from nonbookmat b inner join library l on l.lib_code = b.lib_code where 1=1 AND L.College_Code ='" + collegeCode + "'  " + qrybookfilter + qrylibraryFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + qrydeptfilter + qrycatfilter + qryinvoicefilter + qryAccessionfilter + qrytitlefilter + qryauthorfilter + qrypricefilter + " group by lib_name,Department order by lib_name,Department";



                    dsnonbook.Clear();
                    dsnonbook = d2.select_method_wo_parameter(sqlnb, "Text");
                }
                else if (rblwise.SelectedIndex == 1)
                {
                    if (accessfrom.All(char.IsNumber) && accessto.All(char.IsNumber))
                    {

                        sqlnb = "select Lib_Name,ISNULL(Subject,'')  as subject,count(title) TotVol,count(distinct title) TotTitle,isnull(sum(cast(price as float)),0) price  from  nonbookmat b inner join library l on l.lib_code = b.lib_code  where 1=1 AND L.College_Code ='" + collegeCode + "'  " + qrybookfilter + qrylibraryFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + qrydeptfilter + qrycatfilter + qryinvoicefilter + qryAccessionfilter + qrytitlefilter + qryauthorfilter + qrypricefilter + " group by lib_name,Department order by lib_name,Department";



                        dsnonbook.Clear();
                        dsnonbook = d2.select_method_wo_parameter(sqlnb, "Text");
                    }
                }
            }
        }

        catch (Exception ex)
        { }
        return dsnonbook;
    }

    public DataSet projectbooks()
    {
        DataSet dsprojectbooks = new DataSet();
        try
        {
            if (ddlCollege.Items.Count > 0)
                collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            if (cbl_department.Items.Count > 0)
                dept = Convert.ToString(getCblSelectedValue(cbl_department));
            if (ddllibrary.Items.Count > 0)
                library = Convert.ToString(ddllibrary.SelectedValue);
            if (ddlbooks.Items.Count > 0)
                booktype = Convert.ToString(ddlbooks.SelectedValue);
            if (ddltype.Items.Count > 0)
                typ = Convert.ToString(ddltype.SelectedValue);
            if (ddlcategory.Items.Count > 0)
                categor = Convert.ToString(ddlcategory.SelectedValue);


            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(library))
            {
                if (library != "All" && library != "")
                {
                    qrylibraryFilter = "and l.lib_code='" + library + "'";
                }
                if (ddltype.SelectedIndex == 2)
                {
                    qrytxtbooksFilter = " and UPPER(ISNULL(Ref,'No')) = 'NO'";
                }
                if (ddltype.SelectedIndex == 1)
                {
                    qryrefbooksFilter = "and UPPER(ISNULL(Ref,'YES')) = 'YES'";
                }
                if (typ != "All" && typ != "")
                {
                    qrytxtrefbooksFilter = " and (UPPER(ISNULL(Ref,'No')) = 'NO' or UPPER(ISNULL(Ref,'YES')) = 'YES')";
                }
                if (dept != "All" && dept != "")
                {

                    if (rblwise.SelectedIndex == 0)
                    {
                        qrydeptfilter = " and Dept_Code in('" + dept + "')";

                    }
                    else
                    {
                        qrysubfilter = " and Department in('" + dept + "')";
                    }
                }
                if (categor != "All" && categor != "")
                {
                    qrycatfilter = " and category='" + categor + "'";
                }
                if (Titlewise != "All" && Titlewise != "")
                {
                    qrytitlefilter = " and Titlewise='" + Titlewise + "'";
                }
                if (Author != "All" && Author != "")
                {
                    qryauthorfilter = " and Author='" + Author + "'";
                }

                if (cbdate.Checked)//Invoice Date
                {
                    string fromDate = txt_fromdate.Text;
                    string toDate = txt_todate.Text;
                    string[] fromdate = fromDate.Split('/');
                    string[] todate = toDate.Split('/');
                    if (fromdate.Length == 3)
                        infromdate = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();

                    if (todate.Length == 3)
                        intodate = todate[2].ToString() + "-" + todate[1].ToString() + "-" + todate[0].ToString();
                    qryinvoicefilter = "and b.bill_date between'" + infromdate + "'and '" + intodate + "'";
                }

                if (cbdate2.Checked)//Accession Date
                {
                    //if (lbl_fromdate2.Length == 3)
                    //    lbl_fromdate2 = lbl_fromdate2[1].ToString() + "/" + lbl_fromdate2[0].ToString() + "/" + lbl_fromdate2[2].ToString();
                    //string[] tdate = lbl_todate2.Split('/');
                    //if (lbl_todate2.Length == 3)
                    //    lbl_fromdate2 = lbl_todate2[1].ToString() + "/" + lbl_todate2[0].ToString() + "/" + lbl_todate2[2].ToString();
                    //qryAccessionfilter = "and b.Access_date between'" + lbl_fromdate2 + "'and '" + lbl_todate2 + "'";
                }

                if (cbdate1.Checked == true)//accessNo
                {
                    accessfrom = txt_fromdate1.Text;
                    accessto = txt_todate1.Text;

                    qrybookfilter = "and CASE WHEN IsNumeric(right(B.acc_no,1)) = 1 then cast(substring(B.acc_no,(PATINDEX('%[0-9]%',B.acc_no)),len(B.acc_no))as int) end  between '" + accessfrom + "' and '" + accessto + "'";
                }
                if (cbRange.Checked == true)//price range
                {
                    pricefrom = txtFromRange.Text;
                    priceto = txtToRange.Text;

                    //qrypricefilter = "and b.Price between '" + pricefrom + "' and '" + priceto + "'";
                    qrypricefilter = "and (convert(float,b.price)>='" + pricefrom + "' and convert(float,b.price)<='" + priceto + "')";
                }
            }
            //    if (ddlbooks.SelectedIndex == 4)
            //    {
            //        if (rblwise.SelectedIndex == 0)
            //        {

            //            if (accessfrom.All(char.IsNumber) && accessto.All(char.IsNumber))
            //            {
            //                sqlp = "select  Lib_Name,'' as Dept_Code,Title,count(title) TotVol,count(distinct title) TotTitle,0 as  price from project_book b inner join library l on l.lib_code = b.lib_code where 1=1 AND L.College_Code ='" + collegeCode + "'  " + qrybookfilter + qrylibraryFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + qrydeptfilter + qrycatfilter + qryinvoicefilter + qryAccessionfilter + qrytitlefilter + qryauthorfilter + qrypricefilter + " group by lib_name,Title order by lib_name,Title";

            //                dsprojectbooks.Clear();
            //                dsprojectbooks = d2.select_method_wo_parameter(sqlp, "Text");
            //            }
            //        }

            //        else if (rblwise.SelectedIndex == 1)
            //        {
            //            if (accessfrom.All(char.IsNumber) && accessto.All(char.IsNumber))
            //            {

            //                sqlnb = "select Lib_Name,'' as subject,count(title) TotVol,count(distinct title) TotTitle,0 as  price from project_book b inner join library l on l.lib_code = b.lib_code where 1=1 AND L.College_Code ='" + collegeCode + "'  " + qrybookfilter + qrylibraryFilter + qrypricefilter + " group by lib_name order by lib_name";

            //                dsprojectbooks.Clear();
            //                dsprojectbooks = d2.select_method_wo_parameter(sqlp, "Text");
            //            }
            //        }
            //    }
            //}
            if (ddlbooks.SelectedIndex == 4)
            {


                if (rblwise.SelectedIndex == 0)
                {

                    if (accessfrom.All(char.IsNumber) && accessto.All(char.IsNumber))
                    {
                        sqlp = "select  Lib_Name,'' as Dept_Code,Title,count(title) TotVol,count(distinct title) TotTitle,0 as  price from project_book b inner join library l on l.lib_code = b.lib_code where 1=1 AND L.College_Code ='" + collegeCode + "'  " + qrybookfilter + qrylibraryFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + qrydeptfilter + qrycatfilter + qryinvoicefilter + qryAccessionfilter + qrytitlefilter + qryauthorfilter + qrypricefilter + " group by lib_name,Title order by lib_name,Title";
                    }

                    dsprojectbooks.Clear();
                    dsprojectbooks = d2.select_method_wo_parameter(sqlp, "Text");
                }
                else if (rblwise.SelectedIndex == 1)
                {
                    if (accessfrom.All(char.IsNumber) && accessto.All(char.IsNumber) && rblType.SelectedIndex == 0)
                    {
                        sqlp = "select Lib_Name,'' as subject,count(title) TotVol,count(distinct title) TotTitle,0 as  price from project_book b inner join library l on l.lib_code = b.lib_code where 1=1 AND L.College_Code ='" + collegeCode + "'  " + qrybookfilter + qrylibraryFilter + qrypricefilter + " group by lib_name order by lib_name";
                    }
                    dsprojectbooks.Clear();
                    dsprojectbooks = d2.select_method_wo_parameter(sqlp, "Text");

                }
            }
        }
        catch (Exception ex)
        { }
        return dsprojectbooks;
    }

    public DataSet questionbanks()
    {
        DataSet dsquestionbanks = new DataSet();
        try
        {
            if (ddlCollege.Items.Count > 0)
                collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            if (cbl_department.Items.Count > 0)
                dept = Convert.ToString(getCblSelectedValue(cbl_department));
            if (ddllibrary.Items.Count > 0)
                library = Convert.ToString(ddllibrary.SelectedValue);
            if (ddlbooks.Items.Count > 0)
                booktype = Convert.ToString(ddlbooks.SelectedValue);
            if (ddltype.Items.Count > 0)
                typ = Convert.ToString(ddltype.SelectedValue);
            if (ddlcategory.Items.Count > 0)
                categor = Convert.ToString(ddlcategory.SelectedValue);


            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(library))
            {
                if (library != "All" && library != "")
                {
                    qrylibraryFilter = "and l.lib_code='" + library + "'";
                }
                if (ddltype.SelectedIndex == 2)
                {
                    qrytxtbooksFilter = " and UPPER(ISNULL(Ref,'No')) = 'NO'";
                }
                if (ddltype.SelectedIndex == 1)
                {
                    qryrefbooksFilter = "and UPPER(ISNULL(Ref,'YES')) = 'YES'";
                }
                if (typ != "All" && typ != "")
                {
                    qrytxtrefbooksFilter = " and (UPPER(ISNULL(Ref,'No')) = 'NO' or UPPER(ISNULL(Ref,'YES')) = 'YES')";
                }
                if (dept != "All" && dept != "")
                {
                    qrydeptfilter = " and dept_name in('" + dept + "')";
                }
                if (subject != "All" && subject != "")
                {
                    qrysubfilter = " and subject='" + subject + "'";
                }
                if (categor != "All" && categor != "")
                {
                    qrycatfilter = " and category='" + categor + "'";
                }
                if (Titlewise != "All" && Titlewise != "")
                {
                    qrytitlefilter = " and Titlewise='" + Titlewise + "'";
                }
                if (Author != "All" && Author != "")
                {
                    qryauthorfilter = " and Author='" + Author + "'";
                }

                if (cbdate.Checked)//Invoice Date
                {
                    string fromDate = txt_fromdate.Text;
                    string toDate = txt_todate.Text;
                    string[] fromdate = fromDate.Split('/');
                    string[] todate = toDate.Split('/');
                    if (fromdate.Length == 3)
                        infromdate = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();

                    if (todate.Length == 3)
                        intodate = todate[2].ToString() + "-" + todate[1].ToString() + "-" + todate[0].ToString();
                    qryinvoicefilter = "and b.bill_date between'" + infromdate + "'and '" + intodate + "'";
                }

                if (cbdate2.Checked)//Accession Date
                {
                    //if (lbl_fromdate2.Length == 3)
                    //    lbl_fromdate2 = lbl_fromdate2[1].ToString() + "/" + lbl_fromdate2[0].ToString() + "/" + lbl_fromdate2[2].ToString();
                    //string[] tdate = lbl_todate2.Split('/');
                    //if (lbl_todate2.Length == 3)
                    //    lbl_fromdate2 = lbl_todate2[1].ToString() + "/" + lbl_todate2[0].ToString() + "/" + lbl_todate2[2].ToString();
                    //qryAccessionfilter = "and b.Access_date between'" + lbl_fromdate2 + "'and '" + lbl_todate2 + "'";
                }

                if (cbdate1.Checked == true)//accessNo
                {
                    accessfrom = txt_fromdate1.Text;
                    accessto = txt_todate1.Text;

                    qrybookfilter = "and CASE WHEN IsNumeric(right(B.acc_no,1)) = 1 then cast(substring(B.acc_no,(PATINDEX('%[0-9]%',B.acc_no)),len(B.acc_no))as int) end  between '" + accessfrom + "' and '" + accessto + "'";
                }
                if (cbRange.Checked == true)//price range
                {
                    pricefrom = txtFromRange.Text;
                    priceto = txtToRange.Text;

                    //qrypricefilter = "and b.Price between '" + pricefrom + "' and '" + priceto + "'";
                    qrypricefilter = "and (convert(float,b.price)>='" + pricefrom + "' and convert(float,b.price)<='" + priceto + "')";
                }
            }
            if (ddlbooks.SelectedIndex == 5)
            {
                if (rblwise.SelectedIndex == 0)
                {

                    if (accessfrom.All(char.IsNumber) && accessto.All(char.IsNumber))
                    {
                        sqlqb = "select  Lib_Name,'' as Dept_Code,Title,count(title) TotVol,count(distinct title) TotTitle,0 as price from University_Question b inner join library l on l.lib_code = b.lib_code where 1=1 AND L.College_Code ='" + collegeCode + "'  " + qrybookfilter + qrylibraryFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + qrydeptfilter + qrycatfilter + qryinvoicefilter + qryAccessionfilter + qrytitlefilter + qryauthorfilter + qrypricefilter + "group by lib_name,Title order by lib_name,Title";
                    }
                    dsquestionbanks.Clear();
                    dsquestionbanks = d2.select_method_wo_parameter(sqlqb, "Text");
                }
            }
            else if (rblwise.SelectedIndex == 1)
            {
                if (accessfrom.All(char.IsNumber) && accessto.All(char.IsNumber))
                {

                    sqlnb = "select Lib_Name,'' as subject,count(title) TotVol,count(distinct title) TotTitle,0 as price from University_Question b inner join library l on l.lib_code = b.lib_code where 1=1 AND L.College_Code ='" + collegeCode + "'  " + qrybookfilter + qrylibraryFilter + qrypricefilter + " group by lib_name,Department order by lib_name,Department";

                    dsquestionbanks.Clear();
                    dsquestionbanks = d2.select_method_wo_parameter(sqlqb, "Text");
                }
            }
        }

        catch (Exception ex)
        { }
        return dsquestionbanks;
    }
    # endregion

    #region spread
    private void loadspreadCount(DataTable ds)
    {
        try
        {

            Department = string.Empty;
            Subject = string.Empty;
            NoofTitle = string.Empty;
            NoofVolume = string.Empty;
            price = string.Empty;
            Title = string.Empty;
            Author = string.Empty;
            tottitle = string.Empty;
            totvol = string.Empty;
            totprice = string.Empty;
            totaldisp = 0;
            tot = 0;
            insdex = 0;
            hasprice = new Hashtable();
            hastitle = new Hashtable();
            hasvol = new Hashtable();
            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            int sno = 0;
            DataTable dtnew = new DataTable();
            ItemList.Clear();
            if (rblwise.SelectedIndex == 0)
            {

                if (ItemList.Count == 0)
                {
                    ItemList.Add("Dept_Code");
                    //ItemList.Add("Title");
                    ItemList.Add("TotVol");
                    ItemList.Add("TotTitle");
                    ItemList.Add("price");
                    //ItemList.Add("Subject");
                    //ItemList.Add("Author");
                }

            }
            //
            if (rblwise.SelectedIndex == 1)
            {

                if (ItemList.Count == 0)
                {
                    ItemList.Add("subject");
                    // ItemList.Add("Title");
                    ItemList.Add("TotVol");
                    ItemList.Add("TotTitle");
                    ItemList.Add("price");
                    //ItemList.Add("Subject");
                    //ItemList.Add("Author");
                }

            }
            columnhash.Clear();
            columnhash.Add("Dept_Code", "Departmet");

            columnhash.Add("TotTitle", "nooftitle");
            columnhash.Add("TotVol", "noofvolume");
            columnhash.Add("price", "price");
            columnhash.Add("subject", "Subject");


            int totitle = 0;
            int tovolume = 0;
            int price1 = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("SNo");
            if (rblwise.SelectedIndex == 0)
            {
                dt.Columns.Add("Department");
            }
            else
            {
                dt.Columns.Add("Subject");
            }

            dt.Columns.Add("NoofTitles");
            dt.Columns.Add("NoofVolumes");
            dt.Columns.Add("Price");
            DataRow dr;


            int rowcount = 0;
            for (int i = 0; i < ds.Rows.Count; i++)
            {
                dr = dt.NewRow();

                dr["SNo"] = Convert.ToString(++rowcount);
                if (rblwise.SelectedIndex == 0)
                {
                    dr["Department"] = Convert.ToString(ds.Rows[i]["Dept_Code"]);
                }
                else
                {
                    dr["Subject"] = Convert.ToString(ds.Rows[i]["subject"]);
                }
                dr["NoofTitles"] = Convert.ToString(ds.Rows[i]["TotTitle"]);
                dr["NoofVolumes"] = Convert.ToString(ds.Rows[i]["TotVol"]);
                dr["Price"] = Convert.ToString(ds.Rows[i]["price"]);

                dt.Rows.Add(dr);
                int m = Convert.ToInt32(ds.Rows[i]["TotTitle"]);
                int n = Convert.ToInt32(ds.Rows[i]["TotVol"]);
                int k = Convert.ToInt32(ds.Rows[i]["price"]);
                totitle = totitle + m;
                tovolume = tovolume + n;
                price1 = price1 + k;



            }

            dr = dt.NewRow();

            if (rblwise.SelectedIndex == 0)
            {
                dr["Department"] = "Total";
            }
            else
            {
                dr["Subject"] = "Total";
            }

            dr["NoofTitles"] = Convert.ToString(totitle);
            dr["NoofVolumes"] = Convert.ToString(tovolume);
            dr["Price"] = Convert.ToString(price1);
            dt.Rows.Add(dr);

            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.Visible = true;
            showreport2.Visible = true;
            print.Visible = true;


        }
        catch (Exception ex)

        { }

    }



    # endregion spread

    #region Print

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(GridView1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Book Statistics";
            //+'@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "Book Statistics";
            string ss = Session["usercode"].ToString();
            Printcontrol.loadspreaddetails(GridView1, pagename, degreedetails, 0, ss, "1");
            Printcontrol.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistics"); }{ }
    }

    protected void getPrintSettings()
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
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmasterhed.Visible = true;
                }
            }
            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistics"); }{ }
    }

    #endregion

    public override void VerifyRenderingInServerForm(Control control)
    { }

    #region alertclose
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }
        {
        }
    }


    #endregion


}