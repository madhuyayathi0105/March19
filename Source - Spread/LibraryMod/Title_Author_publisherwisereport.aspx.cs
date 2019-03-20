using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InsproDataAccess;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Drawing;

public partial class LibraryMod_Title_Author_publisherwisereport : System.Web.UI.Page
{
    #region Field Declaration

    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;

    Dictionary<string, string> dicStaffList = new Dictionary<string, string>();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet dsprint = new DataSet();
    ArrayList colord = new ArrayList();
    DAccess2 da = new DAccess2();
    DataView dvhead = new DataView();
    Hashtable ht = new Hashtable();
    DataTable dtCommon = new DataTable();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    Dictionary<string, string> dictitle = new Dictionary<string, string>();
    DataTable title = new DataTable();
    DataRow drtit;
    string collegecode = string.Empty;
    string department = string.Empty;
    string reporttype = string.Empty;
    string library = string.Empty;
    string qryAuthorFilter = string.Empty;
    string Authorselectqry = string.Empty;
    string Publisherselectqry = string.Empty;
    string Isbnselectqry = string.Empty;
    string Titlenselectqry = string.Empty;
    string qrylibraryFilter = string.Empty;
    string qrytxtbooksFilter = string.Empty;
    string qryrefbooksFilter = string.Empty;
    string qrytxtrefbooksFilter = string.Empty;
    string qrytransferFilter = string.Empty;
    string qrynottransferFilter = string.Empty;
    string qrybothtransferFilter = string.Empty;
    string qrypublisherFilter = string.Empty;
    string qryisbnFilter = string.Empty;
    string qrytitleFilter = string.Empty;
    DataTable dtauthor = new DataTable();
    DataRow drauth;
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
                ReportType();
                getLibPrivil();
                Department();
                TitleType();
                transferred();
                book();
                showreport1.Visible = false;
                showreport2.Visible = false;
                //getPrintSettings();
                //getPrintSettings2();


            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
    }

    #endregion

    #region ReprotType
    public void ReportType()
    {
        try
        {
            ddlreporttype.Items.Add("TitleWise Report");
            ddlreporttype.Items.Add("AuthorWise Report");
            ddlreporttype.Items.Add("PublisherWise Report");
            ddlreporttype.Items.Add("ISBNwise Report");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
    }
    #endregion

    #region TitleType
    public void TitleType()
    {
        try
        {
            ddltitletype.Items.Add("Title");
            ddltitletype.Items.Add("Title With Author");
            ddltitletype.Items.Add("Title With Price");
            ddltitletype.Items.Add("Title,Author And Price");
            ddltitletype.Items.Add("Title With Access No");
            ddltitletype.Items.Add("Title,Author With Access No");
            ddltitletype.Items.Add("Title,Author,AccessNo And Call.No");
            ddltitletype.Items.Add("Title,Author,Department,Edition");
            ddltitletype.Items.Add("Access No,Title,Author,Publisher");
            ddltitletype.Items.Add("SNo,Access No,Title,Author,Publisher,Edition,Price");
            ddltitletype.Items.Add("Access No,Title,Author,Publisher,Edition,Price");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }{ }
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
                    ddllibrary.Items.Insert(0, "All");
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }


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

    #region Department
    public void Department()
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
            d2.CallCheckboxChange(cb_department, cbl_department, txt_department, "Department", "--Select--");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
    }

    protected void cbl_department_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            d2.CallCheckboxListChange(cb_department, cbl_department, txt_department, "Department", "--Select--");


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
    }

    #endregion

    #region Transfer

    public void transferred()
    {
        try
        {
            rbltransType.Items.Add("Transferred");
            rbltransType.Items.Add("Not Transferred");
            rbltransType.Items.Add("Both");
            rbltransType.Items.FindByText("Transferred").Selected = true;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }

    }
    #endregion

    #region Books
    public void book()
    {
        try
        {
            rblbooks.Items.Add("Text Books");
            rblbooks.Items.Add("Reference Books");
            rblbooks.Items.Add("Both");
            rblbooks.Items.FindByText("Text Books").Selected = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
    }
    #endregion

    #region Index Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlCollege.SelectedItem.Value);
                ddlCollege.SelectedIndex = ddlCollege.Items.IndexOf(ddlCollege.Items.FindByValue(collegecode));
                getLibPrivil();
                Department();
                showreport1.Visible = false;
                showreport2.Visible = false;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }

    }


    protected void ddlreporttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            showreport1.Visible = false;
            showreport2.Visible = false;
            if (ddlreporttype.SelectedIndex == 0)
            {
                lbltitletype.Visible = true;
                ddltitletype.Visible = true;
                lbl_Title.Visible = true;
                lbl_Author.Visible = false;
                lbl_Publisher.Visible = false;
                lbl_Isbn.Visible = false;
                txttitle.Visible = true;
                txtauthor.Visible = false;
                txtpublisher.Visible = false;
                txtisbn.Visible = false;
            }
            if (ddlreporttype.SelectedIndex == 1)
            {
                lbltitletype.Visible = false;
                ddltitletype.Visible = false;
                lbl_Title.Visible = false;
                lbl_Author.Visible = true;
                lbl_Publisher.Visible = false;
                lbl_Isbn.Visible = false;
                txttitle.Visible = false;
                txtauthor.Visible = true;
                txtpublisher.Visible = false;
                txtisbn.Visible = false;

            }
            if (ddlreporttype.SelectedIndex == 2)
            {
                lbltitletype.Visible = false;
                ddltitletype.Visible = false;
                lbl_Title.Visible = false;
                lbl_Author.Visible = false;
                lbl_Publisher.Visible = true;
                lbl_Isbn.Visible = false;
                txttitle.Visible = false;
                txtauthor.Visible = false;
                txtpublisher.Visible = true;
                txtisbn.Visible = false;
            }
            if (ddlreporttype.SelectedIndex == 3)
            {
                lbltitletype.Visible = false;
                ddltitletype.Visible = false;
                lbl_Title.Visible = false;
                lbl_Author.Visible = false;
                lbl_Publisher.Visible = false;
                lbl_Isbn.Visible = true;
                txttitle.Visible = false;
                txtauthor.Visible = false;
                txtpublisher.Visible = false;
                txtisbn.Visible = true;
            }


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
    }


    protected void ddltitletype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            showreport1.Visible = false;
            showreport2.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
    }


    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            Department();
            showreport1.Visible = false;
            showreport2.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
    }

    protected void rbltransType_Selected(object sender, EventArgs e)
    {
        try
        {
            showreport1.Visible = false;
            showreport2.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
    }


    protected void rblbooks_Selected(object sender, EventArgs e)
    {
        try
        {
            showreport1.Visible = false;
            showreport2.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
    }

    #endregion Index Changed Events

    #region Go

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsreport = new DataSet();
            if (ddlreporttype.SelectedIndex != 0)
            {
                dsreport = Report();
                if (dsreport.Tables.Count > 0 && dsreport.Tables[0].Rows.Count > 0)
                {
                    loadspreadAuthorDetails(dsreport);

                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found!";
                    showreport1.Visible = false;
                    showreport2.Visible = false;

                }
            }
            else if ((ddlreporttype.SelectedIndex == 0 && ddltitletype.SelectedIndex == 0) || (ddlreporttype.SelectedIndex == 0 && ddltitletype.SelectedIndex == 1) || (ddlreporttype.SelectedIndex == 0 && ddltitletype.SelectedIndex == 2) || (ddlreporttype.SelectedIndex == 0 && ddltitletype.SelectedIndex == 3) || (ddlreporttype.SelectedIndex == 0 && ddltitletype.SelectedIndex == 7) || (ddlreporttype.SelectedIndex == 0 && ddltitletype.SelectedIndex == 8) || (ddlreporttype.SelectedIndex == 0 && ddltitletype.SelectedIndex == 9) || (ddlreporttype.SelectedIndex == 0 && ddltitletype.SelectedIndex == 10))
            {
                dsreport = Report();
                if (dsreport.Tables.Count > 0 && dsreport.Tables[0].Rows.Count > 0)
                {
                    loadspreadTitleDetails(dsreport);

                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found!";
                    showreport1.Visible = false;
                    showreport2.Visible = false;

                }
            }
            else if ((ddlreporttype.SelectedIndex == 0 && ddltitletype.SelectedIndex == 4) || (ddlreporttype.SelectedIndex == 0 && ddltitletype.SelectedIndex == 5) || (ddlreporttype.SelectedIndex == 0 && ddltitletype.SelectedIndex == 6))
            {
                dsreport = TitleReport();
                if (dsreport.Tables.Count > 0 && dsreport.Tables[0].Rows.Count > 0)
                {
                    loadspreadTitleAccDetails(dsreport);

                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found!";
                    showreport1.Visible = false;
                    showreport2.Visible = false;

                }

            }


        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
    }

    #endregion

    protected void grdManualExit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }


    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void GridView1_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        btngo_Click(sender, e);
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ddlreporttype.SelectedIndex == 0)
            {
                if (ddltitletype.SelectedIndex == 0)
                {
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[6].Visible = true;


                }
                if (ddltitletype.SelectedIndex == 1)
                {

                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[6].Visible = true;

                }
                if (ddltitletype.SelectedIndex == 2)
                {
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[3].Visible = true;
                    e.Row.Cells[6].Visible = true;

                }
                if (ddltitletype.SelectedIndex == 3)
                {

                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = true;
                    e.Row.Cells[6].Visible = true;


                }
                if (ddltitletype.SelectedIndex == 4)
                {

                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[4].Visible = true;
                    e.Row.Cells[6].Visible = true;


                }
                if (ddltitletype.SelectedIndex == 5)
                {
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[4].Visible = true;
                    e.Row.Cells[6].Visible = true;


                }
                if (ddltitletype.SelectedIndex == 6)
                {
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[4].Visible = true;
                    e.Row.Cells[5].Visible = true;
                    e.Row.Cells[6].Visible = true;
                }
                if (ddltitletype.SelectedIndex == 10)
                {
                    e.Row.Cells[3].Visible = true;
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[4].Visible = true;
                    e.Row.Cells[5].Visible = true;
                    e.Row.Cells[6].Visible = true;
                }
               
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (ddlreporttype.SelectedIndex == 0)
            {
                if (ddltitletype.SelectedIndex == 0)
                {
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[6].Visible = true;


                }
                if (ddltitletype.SelectedIndex == 1)
                {

                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[6].Visible = true;

                }
                if (ddltitletype.SelectedIndex == 2)
                {
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[3].Visible = true;
                    e.Row.Cells[6].Visible = true;

                }
                if (ddltitletype.SelectedIndex == 3)
                {

                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = true;
                    e.Row.Cells[6].Visible = true;


                }
                if (ddltitletype.SelectedIndex == 4)
                {

                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[4].Visible = true;
                    e.Row.Cells[6].Visible = true;


                }
                if (ddltitletype.SelectedIndex == 5)
                {
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[4].Visible = true;
                    e.Row.Cells[6].Visible = true;


                }
                if (ddltitletype.SelectedIndex == 6)
                {
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[4].Visible = true;
                    e.Row.Cells[5].Visible = true;
                    e.Row.Cells[6].Visible = true;
                }
                if (ddltitletype.SelectedIndex == 10)
                {
                    e.Row.Cells[3].Visible = true;
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[4].Visible = true;
                    e.Row.Cells[5].Visible = true;
                    e.Row.Cells[6].Visible = true;
                }
            }
        }
    }

    #region LoadHeader
    public void loadspread()
    {

        try
        {
            title.Columns.Add("SNo", typeof(string));
            if (ddlreporttype.SelectedIndex == 1)//Author
            {
                title.Columns.Add("Author", typeof(string));

            }
            if (ddlreporttype.SelectedIndex == 2)//Publisher
            {
                title.Columns.Add("Publisher", typeof(string));
            }
            if (ddlreporttype.SelectedIndex == 3)//Isbn
            {
                title.Columns.Add("ISBN", typeof(string));

            }
            title.Columns.Add("No Of Copies", typeof(string));




        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }

    }

    public void Titleloadheader()
    {
        try
        {
            if (ddlreporttype.SelectedIndex == 0 || ddlreporttype.SelectedIndex == 1 || ddlreporttype.SelectedIndex == 2 || ddlreporttype.SelectedIndex == 3)
            {
                if ((ddltitletype.SelectedIndex == 0) || ddlreporttype.SelectedIndex == 1 || ddlreporttype.SelectedIndex == 2 || ddlreporttype.SelectedIndex == 3)
                {
                    dtauthor.Columns.Add("SNo", typeof(string));
                    dtauthor.Columns.Add("Title", typeof(string));
                    dtauthor.Columns.Add("Author", typeof(string));
                    dtauthor.Columns.Add("price", typeof(string));
                    dtauthor.Columns.Add("Access.No", typeof(string));
                    dtauthor.Columns.Add("Call.No", typeof(string));
                    dtauthor.Columns.Add("No Of Copies", typeof(string));

                }
                else if (ddltitletype.SelectedIndex == 7)
                {
                    dtauthor.Columns.Add("SNo", typeof(string));
                    dtauthor.Columns.Add("Title", typeof(string));
                    dtauthor.Columns.Add("Author", typeof(string));
                    dtauthor.Columns.Add("Department", typeof(string));
                    dtauthor.Columns.Add("Edition", typeof(string));
                }
                else if (ddltitletype.SelectedIndex == 8)
                {
                    dtauthor.Columns.Add("SNo", typeof(string));
                    dtauthor.Columns.Add("Access.No", typeof(string));
                    dtauthor.Columns.Add("Title", typeof(string));
                    dtauthor.Columns.Add("Author", typeof(string));
                    dtauthor.Columns.Add("Publisher", typeof(string));
                }
                else if (ddltitletype.SelectedIndex == 9)
                {
                    dtauthor.Columns.Add("SNo", typeof(string));
                    dtauthor.Columns.Add("Access.No", typeof(string));
                    dtauthor.Columns.Add("Title", typeof(string));
                    dtauthor.Columns.Add("Author", typeof(string));
                    dtauthor.Columns.Add("Publisher", typeof(string));
                    dtauthor.Columns.Add("Edition", typeof(string));
                    dtauthor.Columns.Add("price", typeof(string));
                }
                else if (ddltitletype.SelectedIndex == 10)
                {
                    dtauthor.Columns.Add("SNo", typeof(string));
                    dtauthor.Columns.Add("Access.No", typeof(string));
                    dtauthor.Columns.Add("Title", typeof(string));
                    dtauthor.Columns.Add("Author", typeof(string));
                    dtauthor.Columns.Add("Publisher", typeof(string));
                    dtauthor.Columns.Add("Edition", typeof(string));
                    dtauthor.Columns.Add("price", typeof(string));
                }
            }




        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }



    }
    #endregion



    
    public DataSet Report()
    {
        DataSet dsauthor = new DataSet();

        try
        {
            if (ddlCollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlCollege.SelectedValue);
            if (cbl_department.Items.Count > 0)
                department = Convert.ToString(d2.getCblSelectedValue(cbl_department));
            if (ddllibrary.Items.Count > 0)
                //library = ddllibrary.SelectedItem.Text.ToString().ToLower();
                library = Convert.ToString(ddllibrary.SelectedValue);
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(department) && !string.IsNullOrEmpty(library))
            {
                //Author
                if (txtauthor.Text.Trim() != "")
                {
                    qryAuthorFilter = "and Author like '" + txtauthor.Text + "%' ";
                }
                //Publisher
                if (txtpublisher.Text.Trim() != "")
                {
                    qrypublisherFilter = "and Publisher like '" + txtpublisher.Text + "%' ";
                }
                //Isbn
                if (txtisbn.Text.Trim() != "")
                {
                    qryisbnFilter = "and ISBN  like '" + txtisbn.Text + "%' ";

                }
                //Title
                if (txttitle.Text.Trim() != "")
                {
                    qrytitleFilter = "and Title  like '" + txttitle.Text + "%' ";

                }
                //library
                if (library != "All" && library != "")
                {
                    qrylibraryFilter = "and lb.lib_code='" + library + "'";
                }
                //Transferred
                if (rbltransType.SelectedIndex == 0)
                {
                    qrytransferFilter = "AND ISNULL(Transfered,0) = 1";
                }
                if (rbltransType.SelectedIndex == 1)
                {
                    qrynottransferFilter = " AND ISNULL(Transfered,0) = 0";
                }
                if (rbltransType.SelectedIndex == 2)
                {
                    qrybothtransferFilter = " AND (ISNULL(Transfered,0) = 0 or ISNULL(Transfered,0) = 1)";
                }
                //Books
                if (rblbooks.SelectedIndex == 0)
                {
                    qrytxtbooksFilter = " and UPPER(ISNULL(Ref,'No')) = 'NO'";
                }
                if (rblbooks.SelectedIndex == 1)
                {
                    qryrefbooksFilter = "and UPPER(ISNULL(Ref,'YES')) = 'YES'";
                }
                if (rblbooks.SelectedIndex == 2)
                {
                    qrytxtrefbooksFilter = " and (UPPER(ISNULL(Ref,'No')) = 'NO' or UPPER(ISNULL(Ref,'YES')) = 'YES')";
                }

                #region AuthorWiseReport
                //if (rbltransType.SelectedIndex == 0)
                //{
                //    Authorselectqry = "select Count(*) Author,bd.Author as AuthorName   from BookDetails bd,library lb where bd.Lib_Code = lb.Lib_Code and lb.College_Code ='" + collegecode + "' and bd.Dept_Code in('" + department + "')   " + qrylibraryFilter + qryAuthorFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + "  AND ISNULL(Transfered,0) = 1  group by bd.Author,bd.CopyNo";
                //    dsauthor.Clear();
                //    dsauthor = d2.select_method_wo_parameter(Authorselectqry, "Text");
                //}
                //if (rbltransType.SelectedIndex == 1)
                //{
                //    Authorselectqry = "select Count(*) Author,bd.Author as AuthorName   from BookDetails bd,library lb where bd.Lib_Code = lb.Lib_Code and lb.College_Code ='" + collegecode + "' and bd.Dept_Code in('" + department + "') " + qrylibraryFilter + qryAuthorFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + "  AND ISNULL(Transfered,0) = 0  group by bd.Author,bd.CopyNo";
                //    dsauthor.Clear();
                //    dsauthor = d2.select_method_wo_parameter(Authorselectqry, "Text");
                //}
                //if (rbltransType.SelectedIndex == 2)
                //{
                //}
                if (ddlreporttype.SelectedIndex == 1)
                {
                    Authorselectqry = "select distinct Author,Count(*) TotAuthor   from BookDetails bd,library lb where bd.Lib_Code = lb.Lib_Code and lb.College_Code ='" + collegecode + "' and bd.Dept_Code in('" + department + "')" + qrylibraryFilter + qryAuthorFilter + qrytransferFilter + qrynottransferFilter + qrybothtransferFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + " group by Author ORDER BY Author,TotAuthor DESC";
                    dsauthor.Clear();
                    dsauthor = d2.select_method_wo_parameter(Authorselectqry, "Text");

                }
                #endregion

                #region Publisher
                if (ddlreporttype.SelectedIndex == 2)
                {
                    Publisherselectqry = "select distinct Publisher,Count(*) TotPublisher FROM BookDetails bd,Library lb WHERE bd.Lib_Code = lb.Lib_Code AND lb.College_Code ='" + collegecode + "' and bd.Dept_Code in('" + department + "')" + qrylibraryFilter + qrypublisherFilter + qrytransferFilter + qrynottransferFilter + qrybothtransferFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + "  GROUP BY Publisher ORDER BY Publisher,TotPublisher DESC";
                    dsauthor.Clear();
                    dsauthor = d2.select_method_wo_parameter(Publisherselectqry, "Text");
                }
                #endregion


                #region ISBN
                if (ddlreporttype.SelectedIndex == 3)
                {
                    Isbnselectqry = "SELECT ISNULL(ISBN,' ') ISBN,Count(*) TotISBN  FROM BookDetails bd,Library lb WHERE bd.Lib_Code = lb.Lib_Code AND lb.College_Code='" + collegecode + "' and bd.Dept_Code in('" + department + "')" + qrylibraryFilter + qryisbnFilter + qrytransferFilter + qrynottransferFilter + qrybothtransferFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + "   GROUP BY ISBN ORDER BY LEN(ISBN),TotISBN DESC";
                    dsauthor.Clear();
                    dsauthor = d2.select_method_wo_parameter(Isbnselectqry, "Text");
                }
                #endregion

                #region Title
                if (ddlreporttype.SelectedIndex == 0)
                {
                    if (ddltitletype.SelectedIndex == 0)
                    {
                        Titlenselectqry = "SELECT Title,Count(*) TotTitle FROM BookDetails bd,Library lb WHERE bd.Lib_Code = lb.Lib_Code AND lb.College_Code ='" + collegecode + "' and bd.Dept_Code in('" + department + "')" + qrylibraryFilter + qrytitleFilter + qrytransferFilter + qrynottransferFilter + qrybothtransferFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + "GROUP BY Dept_Code,Title ORDER BY TotTitle DESC,Title";
                    }
                    if (ddltitletype.SelectedIndex == 1)
                    {
                        Titlenselectqry = " SELECT Title,Author,Count(*) TotTitle FROM BookDetails bd,Library lb WHERE bd.Lib_Code = lb.Lib_Code AND lb.College_Code ='" + collegecode + "' and bd.Dept_Code in('" + department + "')" + qrylibraryFilter + qrytitleFilter + qrytransferFilter + qrynottransferFilter + qrybothtransferFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + " GROUP BY Dept_Code,Title,Author ORDER BY TotTitle DESC,Dept_Code,Title,Author";

                    }
                    if (ddltitletype.SelectedIndex == 2)
                    {
                        Titlenselectqry = "SELECT Title,Price,Count(*) TotTitle FROM BookDetails bd,Library lb WHERE bd.Lib_Code = lb.Lib_Code AND lb.College_Code ='" + collegecode + "' and bd.Dept_Code in('" + department + "')" + qrylibraryFilter + qrytitleFilter + qrytransferFilter + qrynottransferFilter + qrybothtransferFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + " GROUP BY Dept_Code,Title,Price ORDER BY TotTitle DESC,Dept_Code,Title,Price";
                    }
                    if (ddltitletype.SelectedIndex == 3)
                    {
                        Titlenselectqry = "SELECT Title,Author,Price,Count(*) TotTitle FROM BookDetails bd,Library lb WHERE bd.Lib_Code = lb.Lib_Code AND lb.College_Code ='" + collegecode + "' and bd.Dept_Code in('" + department + "')" + qrylibraryFilter + qrytitleFilter + qrytransferFilter + qrynottransferFilter + qrybothtransferFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + " GROUP BY Dept_Code,Title,Author,Price ORDER BY TotTitle DESC,Dept_Code,Title,Author,Price";
                    }

                    if (ddltitletype.SelectedIndex == 4)
                    {
                        Titlenselectqry = "SELECT Title,isnull(Acc_No ,0) Acc_No,Count(*) TotTitle FROM BookDetails bd,Library lb WHERE bd.Lib_Code = lb.Lib_Code AND lb.College_Code ='" + collegecode + "' and bd.Dept_Code in('" + department + "')" + qrylibraryFilter + qrytitleFilter + qrytransferFilter + qrynottransferFilter + qrybothtransferFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + " GROUP BY Dept_Code,Title,Acc_No ORDER BY TotTitle DESC,Dept_Code,Title,Acc_No";

                    }

                    if (ddltitletype.SelectedIndex == 7)
                    {
                        Titlenselectqry = "SELECT Title,Author,Dept_Code,Edition FROM BookDetails bd,Library lb WHERE bd.Lib_Code = lb.Lib_Code AND lb.College_Code ='" + collegecode + "' and bd.Dept_Code in('" + department + "')" + qrylibraryFilter + qrytitleFilter + qrytransferFilter + qrynottransferFilter + qrybothtransferFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + "GROUP BY Title,Author,Dept_Code,Edition ORDER BY Title,Author,Dept_Code,Edition";
                    }

                    if (ddltitletype.SelectedIndex == 8)
                    {
                        Titlenselectqry = "SELECT Acc_No,Title,Author,Publisher FROM BookDetails bd,Library lb WHERE bd.Lib_Code = lb.Lib_Code AND lb.College_Code ='" + collegecode + "' and bd.Dept_Code in('" + department + "')" + qrylibraryFilter + qrytitleFilter + qrytransferFilter + qrynottransferFilter + qrybothtransferFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + "GROUP BY Acc_No,Title,Author,Publisher ORDER BY Acc_No,Title,Author,Publisher";
                    }
                    if (ddltitletype.SelectedIndex == 9 || ddltitletype.SelectedIndex == 10)
                    {
                        Titlenselectqry = "SELECT Acc_No,Title,Author,Publisher,Edition,Price  FROM BookDetails bd,Library lb WHERE bd.Lib_Code = lb.Lib_Code AND lb.College_Code ='" + collegecode + "' and bd.Dept_Code in('" + department + "')" + qrylibraryFilter + qrytitleFilter + qrytransferFilter + qrynottransferFilter + qrybothtransferFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + "GROUP BY Acc_No,Title,Author,Publisher,Edition,Price  ORDER BY Acc_No,Title,Author,Publisher,Edition,Price ";
                    }
                    dsauthor.Clear();
                    dsauthor = d2.select_method_wo_parameter(Titlenselectqry, "Text");

                }
                #endregion


            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
        return dsauthor;
    }

    public DataSet TitleReport()
    {
        DataSet dsTitle = new DataSet();

        try
        {
            if (ddlCollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlCollege.SelectedValue);
            if (cbl_department.Items.Count > 0)
                department = Convert.ToString(d2.getCblSelectedValue(cbl_department));
            if (ddllibrary.Items.Count > 0)
                //library = ddllibrary.SelectedItem.Text.ToString().ToLower();
                library = Convert.ToString(ddllibrary.SelectedValue);
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(department) && !string.IsNullOrEmpty(library))
            {

                //Title
                if (txttitle.Text.Trim() != "")
                {
                    qrytitleFilter = "and Title  like '" + txttitle.Text + "%' ";

                }
                //library
                if (library != "All" && library != "")
                {
                    qrylibraryFilter = "and lb.lib_code='" + library + "'";
                }
                //Transferred
                if (rbltransType.SelectedIndex == 0)
                {
                    qrytransferFilter = "AND ISNULL(Transfered,0) = 1";
                }
                if (rbltransType.SelectedIndex == 1)
                {
                    qrynottransferFilter = " AND ISNULL(Transfered,0) = 0";
                }
                if (rbltransType.SelectedIndex == 2)
                {
                    qrybothtransferFilter = " AND (ISNULL(Transfered,0) = 0 or ISNULL(Transfered,0) = 1)";
                }
                //Books
                if (rblbooks.SelectedIndex == 0)
                {
                    qrytxtbooksFilter = " and UPPER(ISNULL(Ref,'No')) = 'NO'";
                }
                if (rblbooks.SelectedIndex == 1)
                {
                    qryrefbooksFilter = "and UPPER(ISNULL(Ref,'YES')) = 'YES'";
                }
                if (rblbooks.SelectedIndex == 2)
                {
                    qrytxtrefbooksFilter = " and (UPPER(ISNULL(Ref,'No')) = 'NO' or UPPER(ISNULL(Ref,'YES')) = 'YES')";
                }
                #region Title
                if (ddlreporttype.SelectedIndex == 0)
                {
                    if (ddltitletype.SelectedIndex == 4)
                    {
                        Titlenselectqry = "SELECT Title,isnull(Acc_No ,0) Acc_No,Count(*) TotTitle FROM BookDetails bd,Library lb WHERE bd.Lib_Code = lb.Lib_Code AND lb.College_Code ='" + collegecode + "' and bd.Dept_Code in('" + department + "')" + qrylibraryFilter + qrytitleFilter + qrytransferFilter + qrynottransferFilter + qrybothtransferFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + " GROUP BY Dept_Code,Title,Acc_No ORDER BY TotTitle DESC,Dept_Code,Title,Acc_No";

                    }
                    if (ddltitletype.SelectedIndex == 5)
                    {
                        Titlenselectqry = "SELECT Title,Author,isnull(Acc_No ,0) Acc_No,Count(*) TotTitle FROM BookDetails bd,Library lb WHERE bd.Lib_Code = lb.Lib_Code AND lb.College_Code='" + collegecode + "' and bd.Dept_Code in('" + department + "')" + qrylibraryFilter + qrytitleFilter + qrytransferFilter + qrynottransferFilter + qrybothtransferFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + "  GROUP BY Dept_Code,Title,Author,Acc_No ORDER BY TotTitle DESC,Dept_Code,Title,Author,Acc_No";

                    }
                    if (ddltitletype.SelectedIndex == 6)
                    {
                        Titlenselectqry = "SELECT Title,Author,isnull(Acc_No ,0) Acc_No,isnull(Call_No ,0) Call_No,Count(*) TotTitle FROM BookDetails bd,Library lb WHERE bd.Lib_Code = lb.Lib_Code AND lb.College_Code ='" + collegecode + "' and bd.Dept_Code in('" + department + "')" + qrylibraryFilter + qrytitleFilter + qrytransferFilter + qrynottransferFilter + qrybothtransferFilter + qrytxtbooksFilter + qryrefbooksFilter + qrytxtrefbooksFilter + " GROUP BY Dept_Code,Title,Author,Acc_No,Call_No ORDER BY TotTitle DESC,Dept_Code,Title,Author,Acc_No,Call_No";

                    }
                }
                dsTitle.Clear();
                dsTitle = d2.select_method_wo_parameter(Titlenselectqry, "Text");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
        return dsTitle;
    }
          


    #region LoadDetails
    private void loadspreadAuthorDetails(DataSet ds)
    {
        try
        {

            loadspread();
            string AuthorName = string.Empty;//Author
            string copies = string.Empty;
            int noofauthor = 0;
            int noofcopies = 0;
            string publisherName = string.Empty;//Publisher
            string publishcopies = string.Empty;
            int noofpublisher = 0;
            int noofpublishcopies = 0;
            string ISBN = string.Empty;//ISBN
            string Isbncopies = string.Empty;
            int noofIsbn = 0;
            int noofIsbncopies = 0;
            string Title = string.Empty;//Title
            string Author = string.Empty;
            string price = string.Empty;
            string accno = string.Empty;
            string callno = string.Empty;
            string titlecopies = string.Empty;
            int nooftitle = 0;
            int nooftitlecopies = 0;
            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            int sno = 0;
            int rowCnt = 0;
            int type = 0;
            DataTable dtnew = new DataTable();
            string typetext = txttype.Text;
            int.TryParse(typetext.Trim(), out type);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                if (typetext == "")
                {
                    rowCnt = ds.Tables[0].Rows.Count;
                }
                else
                {
                    rowCnt = type;
                }
                for (int row = 0; row < rowCnt; row++)
                {
                    sno++;
                    drtit = title.NewRow();
                    if (ddlreporttype.SelectedIndex == 1)//Author
                    {
                        AuthorName = Convert.ToString(ds.Tables[0].Rows[row]["Author"]).Trim();
                        copies = Convert.ToString(ds.Tables[0].Rows[row]["TotAuthor"]).Trim();
                        noofauthor = row + 1;
                        //noofcopies = Convert.ToInt32(ds.Tables[0].Compute("Sum(Author)", ""));
                        noofcopies += Convert.ToInt32(copies);
                    }
                    if (ddlreporttype.SelectedIndex == 2)//Publisher
                    {
                        publisherName = Convert.ToString(ds.Tables[0].Rows[row]["Publisher"]).Trim();
                        publishcopies = Convert.ToString(ds.Tables[0].Rows[row]["TotPublisher"]).Trim();
                        noofpublisher = row + 1;
                        //noofcopies = Convert.ToInt32(ds.Tables[0].Compute("Sum(Author)", ""));
                        noofpublishcopies += Convert.ToInt32(publishcopies);

                    }
                    if (ddlreporttype.SelectedIndex == 3)//ISBN
                    {
                        ISBN = Convert.ToString(ds.Tables[0].Rows[row]["ISBN"]).Trim();
                        Isbncopies = Convert.ToString(ds.Tables[0].Rows[row]["TotISBN"]).Trim();
                        noofIsbn = row + 1;
                        //noofcopies = Convert.ToInt32(ds.Tables[0].Compute("Sum(Author)", ""));
                        noofIsbncopies += Convert.ToInt32(Isbncopies);

                    }

                    drtit = title.NewRow();
                    drtit["SNo"] = Convert.ToString(sno);
                    if (ddlreporttype.SelectedIndex == 1)//Author
                    {
                        drtit["Author"] = AuthorName;
                        drtit["No Of Copies"] = copies;
                    }
                    if (ddlreporttype.SelectedIndex == 2)//Publisher
                    {
                        drtit["Publisher"] = publisherName;
                        drtit["No Of Copies"] = publishcopies;
                    }
                    if (ddlreporttype.SelectedIndex == 3)//Isbn
                    {
                        drtit["ISBN"] = ISBN;
                        drtit["No Of Copies"] = Isbncopies;
                    }

                    title.Rows.Add(drtit);
                }
                print.Visible = false;
                print2.Visible = true;
                showreport1.Visible = true;
                grdManualExit.DataSource = title;
                grdManualExit.DataBind();

                grdManualExit.Visible = true;
                div2.Visible = true;
                lblnoofbooksCount.Text = Convert.ToString(sno);
            }
            if (ddlreporttype.SelectedIndex == 1)//Author
            {
                drtit = title.NewRow();
                drtit["Author"] = "No.Of.Author:" + noofauthor + "";
                //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("lightGreen");
                //spreadDet1.Sheets[0].SpanModel.Add(spreadDet1.Sheets[0].RowCount - 1, 0, 1, 3);
                if (typetext == "")
                {
                    drtit = title.NewRow();
                    drtit["No Of Copies"] = "No.Of.Books:" + noofcopies + "";
                    //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("violet");
                    //spreadDet1.Sheets[0].SpanModel.Add(spreadDet1.Sheets[0].RowCount - 1, 0, 1, 3);
                }
            }
            if (ddlreporttype.SelectedIndex == 2)//Publisher
            {
                drtit = title.NewRow();
                drtit["Publisher"] = "No.Of.Publisher:" + noofpublisher + "";
                //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("lightGreen");
                //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                //spreadDet1.Sheets[0].SpanModel.Add(spreadDet1.Sheets[0].RowCount - 1, 0, 1, 3);
                if (typetext == "")
                {
                    drtit["No Of Copies"] = "No.Of.Books:" + noofpublishcopies + "";
                    //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("violet");
                    //spreadDet1.Sheets[0].SpanModel.Add(spreadDet1.Sheets[0].RowCount - 1, 0, 1, 3);
                }
            }
            if (ddlreporttype.SelectedIndex == 3)//ISBN
            {
                drtit = title.NewRow();
                drtit["ISBN"] = "No.Of.ISBN:" + noofIsbn + "";
                //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("lightGreen");
                //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                //spreadDet1.Sheets[0].SpanModel.Add(spreadDet1.Sheets[0].RowCount - 1, 0, 1, 3);
                if (typetext == "")
                {
                    drtit = title.NewRow();
                    drtit["No Of Copies"] = "No.Of.Books:" + noofIsbncopies + "";
                    //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    //spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("violet");
                    //spreadDet1.Sheets[0].SpanModel.Add(spreadDet1.Sheets[0].RowCount - 1, 0, 1, 3);
                }
            }


            showreport1.Visible = true;
            showreport2.Visible = false;
            print.Visible = true;
            print2.Visible = false;
            showreport1.Visible = true;
            grdManualExit.DataSource = title;
            grdManualExit.DataBind();

            grdManualExit.Visible = true;
            div2.Visible = true;
            lblnoofbooksCount.Text = Convert.ToString(sno);

        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }

    }


    private void loadspreadTitleDetails(DataSet ds)
    {

        try
        {
          

            Titleloadheader();
            GridView1.Visible = true;
            string Title = string.Empty;//Title
            string Access = string.Empty;
            string pub = string.Empty;
            string Author = string.Empty;
            string depertm = string.Empty;
            string edition = string.Empty;
            string price = string.Empty;
            string accno = string.Empty;
            string callno = string.Empty;
            string titlecopies = string.Empty;
            int nooftitle = 0;
            int nooftitlecopies = 0;
            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            int sno = 0;
            int rowCnt = 0;
            int type = 0;
            DataTable dtnew = new DataTable();
            string typetext = txttype.Text;
            int.TryParse(typetext.Trim(), out type);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                if (typetext == "")
                {
                    rowCnt = ds.Tables[0].Rows.Count;
                }
                else
                {
                    rowCnt = type;
                }
                for (int row = 0; row < rowCnt; row++)
                {
                    sno++;
                    drauth = dtauthor.NewRow();
                    if (ddltitletype.SelectedIndex == 0)
                    {
                        Title = Convert.ToString(ds.Tables[0].Rows[row]["Title"]).Trim();
                        Title = Title.Replace("'", "''");
                        titlecopies = Convert.ToString(ds.Tables[0].Rows[row]["TotTitle"]).Trim();
                    }
                    if (ddltitletype.SelectedIndex == 1)
                    {
                        Title = Convert.ToString(ds.Tables[0].Rows[row]["Title"]).Trim();
                        Title = Title.Replace("'", "''");
                        Author = Convert.ToString(ds.Tables[0].Rows[row]["Author"]).Trim();
                        titlecopies = Convert.ToString(ds.Tables[0].Rows[row]["TotTitle"]).Trim();
                    }
                    if (ddltitletype.SelectedIndex == 2)
                    {
                        Title = Convert.ToString(ds.Tables[0].Rows[row]["Title"]).Trim();
                        Title = Title.Replace("'", "''");
                        price = Convert.ToString(ds.Tables[0].Rows[row]["Price"]).Trim();
                        titlecopies = Convert.ToString(ds.Tables[0].Rows[row]["TotTitle"]).Trim();
                    }
                    if (ddltitletype.SelectedIndex == 3)
                    {
                        Title = Convert.ToString(ds.Tables[0].Rows[row]["Title"]).Trim();
                        Title = Title.Replace("'", "''");
                        Author = Convert.ToString(ds.Tables[0].Rows[row]["Author"]).Trim();
                        price = Convert.ToString(ds.Tables[0].Rows[row]["Price"]).Trim();
                        titlecopies = Convert.ToString(ds.Tables[0].Rows[row]["TotTitle"]).Trim();
                    }


                    //nooftitle = row + 1;
                    //nooftitlecopies += Convert.ToInt32(titlecopies);

                    if (ddltitletype.SelectedIndex == 7)
                    {
                        Title = Convert.ToString(ds.Tables[0].Rows[row]["Title"]).Trim();
                        Title = Title.Replace("'", "''");
                        Author = Convert.ToString(ds.Tables[0].Rows[row]["Author"]).Trim();
                        depertm = Convert.ToString(ds.Tables[0].Rows[row]["Dept_Code"]).Trim();
                        edition = Convert.ToString(ds.Tables[0].Rows[row]["Edition"]).Trim();
                    }

                    if (ddltitletype.SelectedIndex == 8)
                    {
                        Access = Convert.ToString(ds.Tables[0].Rows[row]["Acc_No"]).Trim();
                        Title = Convert.ToString(ds.Tables[0].Rows[row]["Title"]).Trim();
                        Title = Title.Replace("'", "''");
                        Author = Convert.ToString(ds.Tables[0].Rows[row]["Author"]).Trim();
                        pub = Convert.ToString(ds.Tables[0].Rows[row]["Publisher"]).Trim();
                    }
                    if (ddltitletype.SelectedIndex == 9)
                    {
                        Access = Convert.ToString(ds.Tables[0].Rows[row]["Acc_No"]).Trim();
                        Title = Convert.ToString(ds.Tables[0].Rows[row]["Title"]).Trim();
                        Title = Title.Replace("'", "''");
                        Author = Convert.ToString(ds.Tables[0].Rows[row]["Author"]).Trim();
                        pub = Convert.ToString(ds.Tables[0].Rows[row]["Publisher"]).Trim();
                        edition = Convert.ToString(ds.Tables[0].Rows[row]["Edition"]).Trim();
                        price = Convert.ToString(ds.Tables[0].Rows[row]["Price"]).Trim();
                    }
                    if (ddltitletype.SelectedIndex == 10)
                    {
                        Access = Convert.ToString(ds.Tables[0].Rows[row]["Acc_No"]).Trim();
                        Title = Convert.ToString(ds.Tables[0].Rows[row]["Title"]).Trim();
                        Title = Title.Replace("'", "''");
                        Author = Convert.ToString(ds.Tables[0].Rows[row]["Author"]).Trim();
                        pub = Convert.ToString(ds.Tables[0].Rows[row]["Publisher"]).Trim();
                        edition = Convert.ToString(ds.Tables[0].Rows[row]["Edition"]).Trim();
                        price = Convert.ToString(ds.Tables[0].Rows[row]["Price"]).Trim();
                    }
                  
                    drauth["SNo"] = Convert.ToString(sno);

                    if (ddlreporttype.SelectedIndex == 0)//TitleReport
                    {
                        if (ddltitletype.SelectedIndex == 0)
                        {
                            drauth["Title"] = Title;
                            drauth["No Of Copies"] = titlecopies;
                        }
                        if (ddltitletype.SelectedIndex == 1)
                        {
                            drauth["Title"] = Title;
                            drauth["Author"] = Author;
                            drauth["No Of Copies"] = titlecopies;
                        }
                        if (ddltitletype.SelectedIndex == 2)
                        {
                            drauth["Title"] = Title;
                            drauth["price"] = price;
                            drauth["No Of Copies"] = titlecopies;
                        }
                        if (ddltitletype.SelectedIndex == 3)
                        {
                            drauth["Title"] = Title;
                            drauth["Author"] = Author;
                            drauth["price"] = price;
                            drauth["No Of Copies"] = titlecopies;
                        }
                        if (ddltitletype.SelectedIndex == 7)
                        {
                            drauth["Title"] = Title;
                            drauth["Title"] = Title;
                            drauth["Author"] = Author;
                            drauth["Department"] = depertm;
                            drauth["Edition"] = edition;
                        }
                        if (ddltitletype.SelectedIndex == 8)
                        {
                            drauth["Access.No"] = Access;
                            drauth["Title"] = Title;
                            drauth["Author"] = Author;
                            drauth["Publisher"] = pub;
                        }
                        if (ddltitletype.SelectedIndex == 9)
                        {
                            drauth["Access.No"] = Access;
                            drauth["Title"] = Title;
                            drauth["Author"] = Author;
                            drauth["Publisher"] = pub;
                            drauth["Edition"] = edition;
                            drauth["price"] = price;
                        }
                        if (ddltitletype.SelectedIndex == 10)
                        {
                            drauth["Access.No"] = Access;
                            drauth["Title"] = Title;
                            drauth["Author"] = Author;
                            drauth["Publisher"] = pub;
                            drauth["Edition"] = edition;
                            drauth["price"] = price;
                        }
                    }

                    dtauthor.Rows.Add(drauth);
                }

            }

            drauth = dtauthor.NewRow();
            drauth["Title"] = "No.Of.Title:" + nooftitle + "";
            dtauthor.Rows.Add(drauth);
            if (typetext == "")
            {
                drauth = dtauthor.NewRow();
                drauth["Title"] = "No.Of.Books:" + nooftitlecopies + "";
                dtauthor.Rows.Add(drauth);
            }
            print.Visible = false;
            print2.Visible = true;
            showreport2.Visible = true;
            showreport1.Visible = false;
            GridView1.DataSource = dtauthor;
            GridView1.DataBind();

            GridView1.Visible = true;

        }


        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }



    }


    private void loadspreadTitleAccDetails(DataSet ds)
    {

        try
        {


            Titleloadheader();
            string depertm = string.Empty;
            string edition = string.Empty;
            string Title = string.Empty;//Title
            string Author = string.Empty;
            string price = string.Empty;
            string accno = string.Empty;
            string callno = string.Empty;
            int titlecopies = 0;
            int nooftitle = 0;
            int nooftitlecopies = 0;
            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            int sno = 0;
            int rowCnt = 0;
            int type = 0;
            DataTable dtnew = new DataTable();
            string typetext = txttype.Text;
            int.TryParse(typetext.Trim(), out type);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                if (typetext == "")
                {
                    rowCnt = ds.Tables[0].Rows.Count;
                }
                else
                {
                    rowCnt = type;
                }
                for (int row = 0; row < rowCnt; row++)
                {
                    if (ddltitletype.SelectedIndex == 4)
                    {
                        Title = Convert.ToString(ds.Tables[0].Rows[row]["Title"]).Trim();
                        Title = Title.Replace("'", "''");
                        //Replace(ds.Tables[0].Rows[row]["Title"], "'", "''");
                        accno = Convert.ToString(ds.Tables[0].Rows[row]["Acc_No"]).Trim();
                        titlecopies = Convert.ToInt32(ds.Tables[0].Rows[row]["TotTitle"]);

                        if (!dicStaffList.ContainsKey(Title))
                        {
                            sno++;
                            drauth = dtauthor.NewRow();
                            ds.Tables[0].DefaultView.RowFilter = "Title ='" + Title + "'";
                            dtnew = ds.Tables[0].DefaultView.ToTable();
                            if (dtnew.Rows.Count > 1)
                            {
                                for (int i = 1; i < dtnew.Rows.Count; i++)
                                {
                                    string accno1 = Convert.ToString(dtnew.Rows[i]["Acc_No"]).Trim();
                                    int titlecopie = Convert.ToInt32(dtnew.Rows[i]["TotTitle"]);
                                    accno = accno + "," + accno1;
                                    titlecopies += titlecopie;
                                }
                            }

                            drauth["SNo"] = Convert.ToString(sno);

                            drauth["Title"] = Title;
                            drauth["Access.No"] = accno;
                            drauth["No Of Copies"] = Convert.ToString(titlecopies);
                            dicStaffList.Add(Title, "value");
                        }

                        //nooftitle = sno;
                        //nooftitlecopies += Convert.ToInt32(titlecopies);
                    }
                    if (ddltitletype.SelectedIndex == 5)
                    {
                        Title = Convert.ToString(ds.Tables[0].Rows[row]["Title"]).Trim();
                        Title = Title.Replace("'", "''");
                        Author = Convert.ToString(ds.Tables[0].Rows[row]["Author"]).Trim();
                        accno = Convert.ToString(ds.Tables[0].Rows[row]["Acc_No"]).Trim();
                        titlecopies = Convert.ToInt32(ds.Tables[0].Rows[row]["TotTitle"]);

                        if (!dicStaffList.ContainsKey(Title))
                        {
                            sno++;
                            drauth = dtauthor.NewRow();

                            ds.Tables[0].DefaultView.RowFilter = "Title ='" + Title + "'";
                            dtnew = ds.Tables[0].DefaultView.ToTable();
                            if (dtnew.Rows.Count > 1)
                            {
                                for (int i = 1; i < dtnew.Rows.Count; i++)
                                {
                                    string author1 = Convert.ToString(dtnew.Rows[i]["Author"]).Trim();
                                    string accno1 = Convert.ToString(dtnew.Rows[i]["Acc_No"]).Trim();
                                    int titlecopie = Convert.ToInt32(dtnew.Rows[i]["TotTitle"]);
                                    Author = Author + ";" + author1;
                                    accno = accno + "," + accno1;
                                    titlecopies += titlecopie;
                                }
                            }

                            drauth["SNo"] = Convert.ToString(sno);

                            drauth["Title"] = Title;
                            drauth["Author"] = Author;
                            drauth["Access.No"] = accno;
                            drauth["No Of Copies"] = Convert.ToString(titlecopies);
                            dicStaffList.Add(Title, "value");
                        }


                    }

                    if (ddltitletype.SelectedIndex == 6)
                    {
                        Title = Convert.ToString(ds.Tables[0].Rows[row]["Title"]).Trim();
                        Title = Title.Replace("'", "''");
                        Author = Convert.ToString(ds.Tables[0].Rows[row]["Author"]).Trim();
                        accno = Convert.ToString(ds.Tables[0].Rows[row]["Acc_No"]).Trim();
                        callno = Convert.ToString(ds.Tables[0].Rows[row]["Call_No"]).Trim();
                        titlecopies = Convert.ToInt32(ds.Tables[0].Rows[row]["TotTitle"]);

                        if (!dicStaffList.ContainsKey(Title))
                        {
                            sno++;
                            drauth = dtauthor.NewRow();

                            ds.Tables[0].DefaultView.RowFilter = "Title ='" + Title + "'";
                            dtnew = ds.Tables[0].DefaultView.ToTable();
                            if (dtnew.Rows.Count > 1)
                            {
                                for (int i = 1; i < dtnew.Rows.Count; i++)
                                {
                                    string author1 = Convert.ToString(dtnew.Rows[i]["Author"]).Trim();
                                    string accno1 = Convert.ToString(dtnew.Rows[i]["Acc_No"]).Trim();
                                    string callno1 = Convert.ToString(dtnew.Rows[i]["Call_No"]).Trim();
                                    int titlecopie = Convert.ToInt32(dtnew.Rows[i]["TotTitle"]);
                                    Author = Author + ";" + author1;
                                    accno = accno + "," + accno1;
                                    callno = callno + "," + callno1;
                                    titlecopies += titlecopie;
                                }
                            }

                            drauth["SNo"] = Convert.ToString(sno);

                            drauth["Title"] = Title;
                            drauth["Author"] = Author;
                            drauth["Access.No"] = accno;
                            drauth["Call.No"] = callno;
                            drauth["No Of Copies"] = Convert.ToString(titlecopies);
                            dicStaffList.Add(Title, "value");
                        }


                    }
                    nooftitle = sno;
                    nooftitlecopies += Convert.ToInt32(titlecopies);



                }
                dtauthor.Rows.Add(drauth);


            }

            drauth = dtauthor.NewRow();
            drauth["Title"] = "No.Of.Title:" + nooftitle + "";
            //spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            //spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("lightGreen");
            //spreadDet2.Sheets[0].SpanModel.Add(spreadDet2.Sheets[0].RowCount - 1, 0, 1, 7);
            dtauthor.Rows.Add(drauth);
            if (typetext == "")
            {
                drauth = dtauthor.NewRow();
                drauth["Title"] = "No.Of.Books:" + nooftitlecopies + "";
                //spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                //spreadDet2.Sheets[0].Cells[spreadDet2.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("violet");

                //spreadDet2.Sheets[0].SpanModel.Add(spreadDet2.Sheets[0].RowCount - 1, 0, 1, 7);
                dtauthor.Rows.Add(drauth);
            }


            print.Visible = false;
            print2.Visible = true;
            showreport2.Visible = true;
            GridView1.DataSource = dtauthor;
            GridView1.DataBind();

            GridView1.Visible = true;
        }

        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }



    }
    #endregion




    #region Print
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdManualExit, reportname);
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
            degreedetails = "Title_Author_publisherwisereport";
            string ss = Session["usercode"].ToString();
            //+ '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "Title_Author_publisherwisereport.aspx";
            Printcontrol.loadspreaddetails(grdManualExit, pagename, degreedetails, 0, ss, "1");
            Printcontrol.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }{ }
    }

    protected void getPrintSettings()
    {
        try
        {
            //barath 15.03.17
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }{ }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }


    #endregion




    #region Print
    protected void btnExcel_Click2(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname2.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(GridView1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
                lblvalidation1.Visible = true;
                txtexcelname2.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }

    }

    public void btnprintmaster_Click2(object sender, EventArgs e)
    {
        try
        {
            lblvalidation3.Text = "";
            txtexcelname2.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Title_Author_publisherwisereport";
            string ss = Session["usercode"].ToString();
            //+ '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "Title_Author_publisherwisereport.aspx";
            NEWPrintMater1.loadspreaddetails(GridView1, pagename, degreedetails, 0, ss, "1");
            NEWPrintMater1.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
    }

    #endregion
    #endregion

    #region alertclose

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); }
    }

    #endregion


}