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
using System.Configuration;

public partial class LibraryMod_nonbookmaterialreport : System.Web.UI.Page
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


    string status = string.Empty;
    static bool isSaveBtnClick = false;
    DataSet nonbookmaterial = new DataSet();
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
                status1();
                Binddept();
                searchby();
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

    #region Bind Method

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
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

    public void getLibPrivil()
    {
        try
        {
            Hashtable hsLibcode = new Hashtable();
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddlCollege.SelectedValue);
            string sql = "";
            string GrpUserVal = "";
            string GrpCode = "";
            string LibCollection = "";
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

    public void BindLibrary(string libcode)
    {
        try
        {
            ddlLibrary.Items.Clear();
            ds.Clear();
            string College = ddlCollege.SelectedValue.ToString();

            string lib_name = "select lib_code,lib_name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + libcode + " AND college_code=" + College + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
            ds.Clear();
            ds = da.select_method_wo_parameter(lib_name, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlLibrary.DataSource = ds;
                ddlLibrary.DataTextField = "Lib_Name";
                ddlLibrary.DataValueField = "Lib_Code";
                ddlLibrary.DataBind();
                ddlLibrary.Items.Insert(0, "All");
            }
        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

    public void status1()
    {
        try
        {
            ddlstatus.Items.Add("All");
            ddlstatus.Items.Add("Available");
            ddlstatus.Items.Add("Issuable");
            ddlstatus.Items.Add("Refernece");

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

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
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

    public void searchby()
    {
        try
        {
            ddlsearchby.Items.Add("All");
            ddlsearchby.Items.Add("Title");
            ddlsearchby.Items.Add("Author");
            ddlsearchby.Items.Add("Publisher");
            ddlsearchby.Items.Add("Book Access Code");
            ddlsearchby.Items.Add("Material Name");
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

    #endregion

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getLibPrivil();
            //divtable.Visible = false;
            //print2.Visible = false;
            div2.Visible = false;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }

    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //divtable.Visible = false;
            // print2.Visible = false;
            div2.Visible = false;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }

    }

    protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // divtable.Visible = false;
            // print2.Visible = false;
            div2.Visible = false;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

    protected void ddldept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // divtable.Visible = false;
            // print2.Visible = false;
            div2.Visible = false;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

    protected void ddlsearchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlsearchby.SelectedIndex == 0)
            {
                txtsearch.Visible = false;
            }
            if (ddlsearchby.SelectedIndex == 1)
            {
                txtsearch.Visible = true;
            }
            if (ddlsearchby.SelectedIndex == 2)
            {

                txtsearch.Visible = true;
            }
            if (ddlsearchby.SelectedIndex == 3)
            {
                txtsearch.Visible = true;
            }
            if (ddlsearchby.SelectedIndex == 4)
            {
                txtsearch.Visible = true;
            }
            if (ddlsearchby.SelectedIndex == 5)
            {
                txtsearch.Visible = true;
            }
            // divtable.Visible = false;
            // print2.Visible = false;
            div2.Visible = false;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

    protected void txtnoofstud_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //divtable.Visible = false;
            // print2.Visible = false;
            div2.Visible = false;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }

    }

    protected void btngoClick(object sender, EventArgs e)
    {
        DataSet bookmaterial = new DataSet();

        bookmaterial = nonbook();
        if (bookmaterial.Tables.Count > 0 && bookmaterial.Tables[0].Rows.Count > 0)
        {
            loadspreadstud(ds);
        }
        else
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "No Record Found!";
        }
    }

    #region grid

    private DataSet nonbook()
    {
        string lib = string.Empty;
        string dept = string.Empty;
        string status = string.Empty;
        string search = string.Empty;
        string Sql = string.Empty;
        try
        {
            if (ddlCollege.Items.Count > 0)
                collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddlLibrary.Items.Count > 0)
                lib = Convert.ToString(ddlLibrary.SelectedValue);
            if (ddldept.Items.Count > 0)
                dept = Convert.ToString(ddldept.SelectedValue);
            if (ddlstatus.Items.Count > 0)
                status = Convert.ToString(ddlstatus.SelectedValue);
            if (ddlsearchby.Items.Count > 0)
                search = Convert.ToString(ddlsearchby.SelectedValue);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(lib) && !string.IsNullOrEmpty(dept))
            {
                Sql = "select isnull(nonbookmat_no,'') nonbookmat_no,isnull(attachment,'') attachment,isnull(title,'') title,isnull(author,'') author,isnull(publisher,'') publisher,isnull(department,'') department,isnull(contents,'') contents,isnull(issue_flag,'') issue_flag,isnull(invoiceno,'') invoiceno,isnull(supplier,'') supplier,isnull(billdate,'') billdate from nonbookmat where 1=1";
                if (dept != "All")
                {
                    Sql = Sql + " and department ='" + dept + "'";
                }
                if (lib != "All")
                {
                    Sql = Sql + " and lib_code ='" + lib + "'";
                }
                if (status != "All")
                {
                    Sql = Sql + " and issue_flag ='" + status + "'";
                }
                if (ddlsearchby.SelectedIndex == 1)
                {
                    Sql = Sql + " and title like '" + txtsearch.Text + "%'";
                }
                if (ddlsearchby.SelectedIndex == 2)
                {
                    Sql = Sql + " and Author like '" + txtsearch.Text + "%'";
                }
                if (ddlsearchby.SelectedIndex == 3)
                {
                    Sql = Sql + " and Publisher like '" + txtsearch.Text + "%'";
                }
                if (ddlsearchby.SelectedIndex == 4)
                {
                    Sql = Sql + " and nonbookmat_no like '" + txtsearch.Text + "%'";
                }
                if (ddlsearchby.SelectedIndex == 5)
                {
                    Sql = Sql + " and attachment like '" + txtsearch.Text + "%'";
                }
                Sql = Sql + " ORDER BY Len(NonBookMat_No),NonBookMat_No ";
                nonbookmaterial.Clear();
                nonbookmaterial = d2.select_method_wo_parameter(Sql, "Text");
            }

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
        return nonbookmaterial;

    }

    protected void grdManualExit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void grdManualExit_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdManualExit.PageIndex = e.NewPageIndex;
        btngoClick(sender, e);
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

        try
        {

            int nobok = nonbookmaterial.Tables[0].Rows.Count;
            if (nobok > 0)
            {
                grdManualExit.DataSource = nonbookmaterial;
                grdManualExit.DataBind();
                grdManualExit.Visible = true;
                div2.Visible = true;
                print2.Visible = true;
                txtnoofbooks.Text = Convert.ToString(nobok);
            }
            else
            {
                grdManualExit.DataSource = null;
                grdManualExit.DataBind();
                grdManualExit.Visible = false;
                print2.Visible = false;
            }


        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

    #endregion

    #region Print
    protected void btnExcel_Click2(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname2.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdManualExit, reportname);
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
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

    public void btnprintmaster_Click2(object sender, EventArgs e)
    {
        try
        {
            lblvalidation3.Text = "";
            txtexcelname2.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Non Book Material Report";
            //+'@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "nonbookmaterialreport.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrol.loadspreaddetails(grdManualExit, pagename, degreedetails, 0, ss, "1");
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

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
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

    #endregion

    #region alertclose
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;
            // Div4.Visible = false;
            // Div1.Visible = false;
            // Label3.Visible = false;
            // div2.Visible = false;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "nonbookmaterialreport"); }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        // Div1.Visible = false;
        //Div4.Visible = false;
    }
    #endregion
}
