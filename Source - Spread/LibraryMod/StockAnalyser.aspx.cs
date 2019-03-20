using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using wc = System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BalAccess;
using DalConnection;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using FarPoint.Web.Spread;
using Gios.Pdf;
using System.IO;
using InsproDataAccess;
using System.Text;

public partial class LibraryMod_StockAnalyser : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet dsload = new DataSet();
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string selectQuery = string.Empty;
    static string querystr1 = string.Empty;
    static string booktype = string.Empty;
    static string QueryStr = string.Empty;
    static string querystr2 = string.Empty;
    static string QueryTable = string.Empty;
    string Sql = string.Empty;
    DataTable dtreport = new DataTable();
    DataRow drow = null;
    ArrayList arrColHdrNames = new ArrayList();
    int SNo = 0;
    string AccNo = "";
    string Libcode = "";
    bool sflag = false;
    string tmpvar = "";
    bool BlnAccNo = false;
    static bool BlnAddToSpread = false;
    static bool BlnDelToSpread = false;
    static bool BlnHeaderBind = false;
    int update = 0;
    int insert = 0;
    int delete = 0;

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
        
            if (!Page.IsPostBack)
            {
                chk_Lost.Visible = true;
                FldsetScan.Visible = true;
                Bindcollege();
                getLibPrivil();
                string CurrentDate = d2.ServerDate();
                string[] dat = CurrentDate.Split('/');
                string monthVal = dat[0];
                string Month = "";
                if (monthVal == "1")
                    Month = "January";
                else if (monthVal == "2")
                    Month = "February";
                else if (monthVal == "3")
                    Month = "March";
                else if (monthVal == "4")
                    Month = "April";
                else if (monthVal == "5")
                    Month = "May";
                else if (monthVal == "6")
                    Month = "June";
                else if (monthVal == "7")
                    Month = "July";
                else if (monthVal == "8")
                    Month = "August";
                else if (monthVal == "9")
                    Month = "September";
                else if (monthVal == "10")
                    Month = "October";
                else if (monthVal == "11")
                    Month = "November";
                else if (monthVal == "12")
                    Month = "December";
                lblHeading.Text = "Data Scanning As On Date - " + Month + " - " + dat[2];
            }
        }
        catch (Exception ex) { }
    }

    #region Collge

    public void Bindcollege()
    {
        try
        {
            string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ds.Clear();
            ddlCollege.Items.Clear();
            selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
        }
        catch (Exception ex) { }
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getLibPrivil();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    #endregion

    #region Library

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
            loadlibrary(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    public void loadlibrary(string LibCollection)
    {
        try
        {
            string collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            selectQuery = "select *,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCollection + " and college_code in('" + collegeCode + "') ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            ddlLibrary.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlLibrary.DataSource = ds;
                ddlLibrary.DataTextField = "lib_name";
                ddlLibrary.DataValueField = "lib_code";
                ddlLibrary.DataBind();
                LibraryClick();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    protected void ddlLibrary_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LibraryClick();
    }

    #endregion

    public void LibraryClick()
    {
        try
        {
            string StrLib_Code = Convert.ToString(ddlLibrary.SelectedValue);
            string bok = Convert.ToString(ddlBookType.SelectedItem.Text);
            string CurrentDate = d2.ServerDate();
            if (bok == "Books")
            {
                booktype = "BOK";
                QueryStr = " acc_no,'','" + booktype + "','" + StrLib_Code + "','" + CurrentDate + "' from bookdetails ";
                querystr1 = " book_status from bookdetails ";
                querystr2 = " acc_no ";
                QueryTable = " FROM BookDetails ";
            }
            if (bok == "Project Books")
            {
                booktype = "PRO";
                QueryStr = " probook_accno,'','" + booktype + "','" + StrLib_Code + "','" + CurrentDate + "'  from project_book where ";
                querystr1 = " issue_flag from project_book ";
                querystr2 = " probook_accno ";
                QueryTable = " FROM Project_Book ";
            }
            if (bok == "Non Book Materials")
            {
                booktype = "NBM";
                QueryStr = " nonbookmat_no,'','" + booktype + "','" + StrLib_Code + "','" + CurrentDate + "'  from nonbookmat where issue_flag='Available' ";
                querystr1 = " issue_flag from nonbookmat ";
                querystr2 = " nonbookmat_no ";
                QueryTable = " FROM NonBookMat ";
            }
            if (bok == "Back Volume")
            {
                booktype = "BVO";
                QueryStr = " access_code,'','" + booktype + "','" + StrLib_Code + "','" + CurrentDate + "'  from back_volume where issue_flag='Available' ";
                querystr1 = " issue_flag from back_volume ";
                querystr2 = " access_code ";
                QueryTable = " FROM Back_Volume ";
            }
            //     Label6.Caption = "Scanning for """ + Combo1.Text + """"
            SetStockDetails();
        }
        catch (Exception ex)
        {
        }
    }

    public void SetStockDetails()
    {
        try
        {
            string bokType = Convert.ToString(ddlBookType.SelectedItem.Text);
            Libcode = Convert.ToString(ddlLibrary.SelectedValue);
            int stock = 0;
            int Scan = 0;
            int Binding = 0;
            int Transfered = 0;
            int Issued = 0;
            int Issuedverified = 0;
            int Lost = 0;
            int Withdrawn = 0;
            int Missing = 0;
            int total = 0;

            if (bokType == "Books")
            {
                //Stock
                Sql = "SELECT COUNT(*) as count FROM BookDoList S WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblStockValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    stock = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                }

                //    'To be scan SELECT COUNT(*) FROM BookDoList S,BookDetails B WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND S.Lib_Code ='" + cbo_library.ItemData(cbo_library.ListIndex) + "' AND Acc_No_Phy = '' 
                Sql = "SELECT COUNT(*) as count  FROM BookDoList S,BookDetails B WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND S.Lib_Code ='" + Libcode + "' AND Acc_No_Phy = ''  AND Acc_No_Sys IN (SELECT Acc_No FROM BookDoList WHERE Lib_Code ='" + Libcode + "' AND (Book_Status = 'Available' or Book_Status = 'Issued')) ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblTobeScanValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    Scan = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                }

                //    'Binding
                Sql = "SELECT COUNT(*) as count FROM BookDetails B,BookDoList S WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' AND Acc_No NOT IN (SELECT Acc_No_Sys FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '')";
                if (booktype == "BOK")
                    Sql = Sql + " AND Book_Status ='Binding' ";
                else
                    Sql = Sql + " AND Issue_Flag ='Binding' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblBindingValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    Binding = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                }

                //    'Transfered
                Sql = "SELECT COUNT(*) as count FROM BookDetails B,BookDoList S WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "'AND Acc_No NOT IN (SELECT Acc_No_Sys FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '')";
                if (booktype == "BOK")
                    Sql = Sql + " AND Book_Status ='Transfered' ";
                else
                    Sql = Sql + " AND Issue_Flag ='Transfered' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblTransferedValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    Transfered = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                }

                //    'Issued
                Sql = "SELECT COUNT(*) as count FROM BookDetails B,BookDoList S WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' AND Acc_No NOT IN (SELECT Acc_No_Sys FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '')";
                if (booktype == "BOK")
                    Sql = Sql + " AND Book_Status ='Issued' ";
                else
                    Sql = Sql + " AND Issue_Flag ='Issued' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblIssuedValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    Issued = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                    LblTobeScanValue.Text = Convert.ToString(Scan - Issued);
                    Scan = Convert.ToInt32(Scan - Issued);
                }

                //    'Issued + verified
                Sql = "SELECT COUNT(*) as count FROM BookDetails B,BookDoList S WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' AND Acc_No IN (SELECT Acc_No_Sys FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '')";
                if (booktype == "BOK")
                    Sql = Sql + " AND Book_Status ='Issued' ";
                else
                    Sql = Sql + " AND Issue_Flag ='Issued' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblIssueVerifyValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    Issuedverified = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                    LblStockValue.Text = Convert.ToString(stock - Issuedverified);
                    stock = Convert.ToInt32(stock - Issuedverified);
                }

                //    'Lost
                Sql = "SELECT COUNT(*) as count FROM BookDetails B,BookDoList S WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' AND Acc_No NOT IN (SELECT Acc_No_Sys FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '')";
                if (booktype == "BOK")
                    Sql = Sql + " AND Book_Status ='Lost' ";
                else
                    Sql = Sql + " AND Issue_Flag ='Lost' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblLostValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    Lost = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                }

                //    'Withdrawn
                Sql = "SELECT COUNT(*) as count FROM BookDetails B,BookDoList S WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "'AND Acc_No NOT IN (SELECT Acc_No_Sys FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '')";
                if (booktype == "BOK")
                    Sql = Sql + " AND Book_Status ='Withdrawn' ";
                else
                    Sql = Sql + " AND Issue_Flag ='Withdrawn' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblWithdrawnValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    Withdrawn = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                }

                //    'Missing
                Sql = "SELECT COUNT(*) as count FROM BookDetails B,BookDoList S WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "'AND Acc_No NOT IN (SELECT Acc_No_Sys FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '')";
                if (booktype == "BOK")
                    Sql = Sql + " AND Book_Status ='Missing' ";
                else
                    Sql = Sql + " AND Issue_Flag ='Missing' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblMissingValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    Missing = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                }

                total = stock + Scan + Binding + Transfered + Lost + Withdrawn + Missing + Issued + Issuedverified;
                LblTotalValue.Text = Convert.ToString(total);
            }

            else if (bokType == "Project Books")
            {
                //    'Stock
                Sql = "SELECT COUNT(*) as count FROM BookDoList S WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblStockValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    stock = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                }

                //    'To be scan
                Sql = "SELECT COUNT(*) as count FROM BookDoList S,Project_Book B WHERE B.ProBook_AccNo = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND S.Lib_Code ='" + Libcode + "' AND Acc_No_Phy = '' AND Acc_No_Sys IN (SELECT ProBook_AccNo FROM Project_Book WHERE Lib_Code ='" + Libcode + "' AND (Issue_Flag = 'Available' or Issue_Flag = 'Issued')) ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblTobeScanValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    Scan = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                }
                //    'Binding
                Sql = "SELECT COUNT(*) as count FROM Project_Book B,BookDoList S WHERE B.ProBook_AccNo = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' AND ProBook_AccNo NOT IN (SELECT Acc_No_Sys FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '')";
                if (booktype == "BOK")
                    Sql = Sql + " AND Issue_Flag ='Binding' ";
                else
                    Sql = Sql + " AND Issue_Flag ='Binding' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblBindingValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    Binding = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                }
                //    'Transfered
                Sql = "SELECT COUNT(*) as count FROM Project_Book B,BookDoList S WHERE B.ProBook_AccNo = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' AND ProBook_AccNo NOT IN (SELECT Acc_No_Sys FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '')";
                if (booktype == "BOK")
                    Sql = Sql + " AND Book_Status ='Transfered' ";
                else
                    Sql = Sql + " AND Issue_Flag ='Transfered' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblTransferedValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    Transfered = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                }
                //    'Issued
                Sql = "SELECT COUNT(*) as count FROM Project_Book B,BookDoList S WHERE B.ProBook_AccNo = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' AND ProBook_AccNo NOT IN (SELECT Acc_No_Sys FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '')";
                if (booktype == "BOK")
                    Sql = Sql + " AND Book_Status ='Issued' ";
                else
                    Sql = Sql + " AND Issue_Flag ='Issued' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblIssuedValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    Issued = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                    LblTobeScanValue.Text = Convert.ToString(Scan - Issued);
                    Scan = Convert.ToInt32(Scan - Issued);
                }
                //    'Issued + verified
                Sql = "SELECT COUNT(*) as count FROM Project_Book B,BookDoList S WHERE B.ProBook_AccNo = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' AND ProBook_AccNo IN (SELECT Acc_No_Sys FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '')";
                if (booktype == "BOK")
                    Sql = Sql + " AND Book_Status ='Issued' ";
                else
                    Sql = Sql + " AND Issue_Flag ='Issued' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblIssueVerifyValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    Issuedverified = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                    LblStockValue.Text = Convert.ToString(Scan - Issuedverified);
                    stock = Convert.ToInt32(Scan - Issuedverified);
                }

                //    'Lost
                Sql = "SELECT COUNT(*) as count FROM Project_Book B,BookDoList S WHERE B.ProBook_AccNo = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' AND ProBook_AccNo NOT IN (SELECT Acc_No_Sys FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '')";
                if (booktype == "BOK")
                    Sql = Sql + " AND Book_Status ='Lost' ";
                else
                    Sql = Sql + " AND Issue_Flag ='Lost' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblLostValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    Lost = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                }

                //    'Withdrawn
                Sql = "SELECT COUNT(*) as count FROM Project_Book B,BookDoList S WHERE B.ProBook_AccNo = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' AND ProBook_AccNo NOT IN (SELECT Acc_No_Sys FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '')";
                if (booktype == "BOK")
                    Sql = Sql + " AND Book_Status ='Withdrawn' ";
                else
                    Sql = Sql + " AND Issue_Flag ='Withdrawn' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblWithdrawnValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    Withdrawn = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                }

                //    'Missing
                Sql = "SELECT COUNT(*) as count FROM Project_Book B,BookDoList S WHERE B.ProBook_AccNo = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' AND ProBook_AccNo NOT IN (SELECT Acc_No_Sys FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '')";
                if (booktype == "BOK")
                    Sql = Sql + " AND Book_Status ='Missing' ";
                else
                    Sql = Sql + " AND Issue_Flag ='Missing' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblMissingValue.Text = Convert.ToString(ds.Tables[0].Rows[0]["count"]);
                    Missing = Convert.ToInt32(ds.Tables[0].Rows[0]["count"]);
                }
                total = stock + Scan + Binding + Transfered + Lost + Withdrawn + Missing + Issued + Issuedverified;
                LblTotalValue.Text = Convert.ToString(total);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void lnkbtn_Stock_OnClick(object sender, EventArgs e)
    {
        try
        {
            Libcode = Convert.ToString(ddlLibrary.SelectedValue);
            fldstReport.Visible = true;
            divConPrevScan.Visible = false;
            chk_Lost.Visible = false;
            FldsetScan.Visible = false;
            LedgendName.InnerText = "Stock List";
            arrColHdrNames.Add("S.No");
            dtreport.Columns.Add("S.No");
            arrColHdrNames.Add("Acc No");
            dtreport.Columns.Add("Acc No");
            arrColHdrNames.Add("Title");
            dtreport.Columns.Add("Title");
            arrColHdrNames.Add("Author");
            dtreport.Columns.Add("Author");
            arrColHdrNames.Add("Publisher");
            dtreport.Columns.Add("Publisher");

            DataRow drHdr1 = dtreport.NewRow();
            for (int grCol = 0; grCol < dtreport.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames[grCol];
            }
            dtreport.Rows.Add(drHdr1);
            Sql = "SELECT Acc_No,Title,Author,Publisher FROM BookDoList S,BookDetails B WHERE S.Acc_No_Sys = B.Acc_No AND S.Lib_Code = B.Lib_Code AND BookType ='BOK' AND S.Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '' ORDER BY LEN(Acc_No),Acc_No ";
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SNo++;
                    drow = dtreport.NewRow();
                    drow[0] = SNo;
                    drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                    drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                    drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                    drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["Publisher"]);
                    dtreport.Rows.Add(drow);
                }
            }
            grdReport.DataSource = dtreport;
            grdReport.DataBind();
            grdReport.Visible = true;
            grdReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
            grdReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            grdReport.Rows[0].Font.Bold = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void lnkbtn_Scan_OnClick(object sender, EventArgs e)
    {
        try
        {
            chk_Lost.Visible = false;
            FldsetScan.Visible = false;
            divConPrevScan.Visible = false;
            Libcode = Convert.ToString(ddlLibrary.SelectedValue);
            fldstReport.Visible = true;
            LedgendName.InnerText = "To be scan List";
            arrColHdrNames.Add("S.No");
            dtreport.Columns.Add("S.No");
            arrColHdrNames.Add("Acc No");
            dtreport.Columns.Add("Acc No");
            arrColHdrNames.Add("Title");
            dtreport.Columns.Add("Title");
            arrColHdrNames.Add("Author");
            dtreport.Columns.Add("Author");
            arrColHdrNames.Add("Publisher");
            dtreport.Columns.Add("Publisher");

            DataRow drHdr1 = dtreport.NewRow();
            for (int grCol = 0; grCol < dtreport.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames[grCol];
            }
            dtreport.Rows.Add(drHdr1);

            Sql = "SELECT Acc_No,Title,Author,Publisher FROM BookDoList S,BookDetails B WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND S.Lib_Code ='" + Libcode + "' AND Acc_No_Phy = '' AND BookType ='BOK' AND Acc_No_Sys IN (SELECT Acc_No FROM BookDoList WHERE Lib_Code ='" + Libcode + "' AND Book_Status IN ('Available','Issued')) ORDER BY LEN(Acc_No),Acc_No ";
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    AccNo = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                    Sql = "SELECT * FROM BookDetails WHERE Acc_No ='" + AccNo + "' AND Lib_Code ='" + Libcode + "' AND Book_Status IN ('Available','Issued') ";
                    //dsload = d2.select_method_wo_parameter(Sql, "text");
                    //if (dsload.Tables[0].Rows.Count > 0)
                    //{
                    //    for (int j = 0; j < dsload.Tables[0].Rows.Count; j++)
                    //    {
                    SNo++;
                    drow = dtreport.NewRow();
                    drow[0] = SNo;
                    drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                    drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                    drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                    drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["Publisher"]);
                    dtreport.Rows.Add(drow);
                    //    }
                    //}
                }
            }
            grdReport.DataSource = dtreport;
            grdReport.DataBind();
            grdReport.Visible = true;
            grdReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
            grdReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            grdReport.Rows[0].Font.Bold = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void lnkbtn_Bind_OnClick(object sender, EventArgs e)
    {
        try
        {
            chk_Lost.Visible = false;
            FldsetScan.Visible = false;
            divConPrevScan.Visible = false;
            Libcode = Convert.ToString(ddlLibrary.SelectedValue);
            fldstReport.Visible = true;
            LedgendName.InnerText = "Binding List";
            arrColHdrNames.Add("S.No");
            dtreport.Columns.Add("S.No");
            arrColHdrNames.Add("Acc No");
            dtreport.Columns.Add("Acc No");
            arrColHdrNames.Add("Title");
            dtreport.Columns.Add("Title");
            arrColHdrNames.Add("Author");
            dtreport.Columns.Add("Author");
            arrColHdrNames.Add("Publisher");
            dtreport.Columns.Add("Publisher");

            DataRow drHdr1 = dtreport.NewRow();
            for (int grCol = 0; grCol < dtreport.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames[grCol];
            }
            dtreport.Rows.Add(drHdr1);

            Sql = "SELECT Acc_No,Title,Author,Publisher FROM BookDetails B,BookDoList S WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' ";
            if (booktype == "BOK")
                Sql = Sql + " AND Book_Status ='Binding' ";
            else
                Sql = Sql + " AND Issue_Flag ='Binding' ";

            Sql = Sql + "ORDER BY LEN(Acc_No),Acc_No ";
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SNo++;
                    drow = dtreport.NewRow();
                    drow[0] = SNo;
                    drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                    drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                    drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                    drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["Publisher"]);
                    dtreport.Rows.Add(drow);
                }
            }
            grdReport.DataSource = dtreport;
            grdReport.DataBind();
            grdReport.Visible = true;
            grdReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
            grdReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            grdReport.Rows[0].Font.Bold = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void lnkbtn_Transfer_OnClick(object sender, EventArgs e)
    {
        try
        {
            chk_Lost.Visible = false;
            FldsetScan.Visible = false;
            divConPrevScan.Visible = false;
            Libcode = Convert.ToString(ddlLibrary.SelectedValue);
            fldstReport.Visible = true;
            LedgendName.InnerText = "Transfered List";
            arrColHdrNames.Add("S.No");
            dtreport.Columns.Add("S.No");
            arrColHdrNames.Add("Acc No");
            dtreport.Columns.Add("Acc No");
            arrColHdrNames.Add("Title");
            dtreport.Columns.Add("Title");
            arrColHdrNames.Add("Author");
            dtreport.Columns.Add("Author");
            arrColHdrNames.Add("Publisher");
            dtreport.Columns.Add("Publisher");

            DataRow drHdr1 = dtreport.NewRow();
            for (int grCol = 0; grCol < dtreport.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames[grCol];
            }
            dtreport.Rows.Add(drHdr1);
            Sql = "SELECT Acc_No,Title,Author,Publisher FROM BookDetails B,BookDoList S WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' ";
            if (booktype == "BOK")
                Sql = Sql + " AND Book_Status ='Transfered' ";
            else
                Sql = Sql + " AND Issue_Flag ='Transfered' ";
            Sql = Sql + "ORDER BY LEN(Acc_No),Acc_No ";
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    AccNo = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                    Sql = "SELECT * FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '' AND Acc_No_Sys ='" + AccNo + "'";
                    dsload = d2.select_method_wo_parameter(Sql, "text");
                    if (dsload.Tables[0].Rows.Count == 0)
                    {
                        SNo++;
                        drow = dtreport.NewRow();
                        drow[0] = SNo;
                        drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                        drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                        drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                        drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["Publisher"]);
                        dtreport.Rows.Add(drow);
                    }
                }
            }
            grdReport.DataSource = dtreport;
            grdReport.DataBind();
            grdReport.Visible = true;
            grdReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
            grdReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            grdReport.Rows[0].Font.Bold = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void lnkbtn_Issued_OnClick(object sender, EventArgs e)
    {
        try
        {
            chk_Lost.Visible = false;
            FldsetScan.Visible = false;
            divConPrevScan.Visible = false;
            Libcode = Convert.ToString(ddlLibrary.SelectedValue);
            fldstReport.Visible = true;
            LedgendName.InnerText = "Issued List";
            arrColHdrNames.Add("S.No");
            dtreport.Columns.Add("S.No");
            arrColHdrNames.Add("Acc No");
            dtreport.Columns.Add("Acc No");
            arrColHdrNames.Add("Title");
            dtreport.Columns.Add("Title");
            arrColHdrNames.Add("Author");
            dtreport.Columns.Add("Author");
            arrColHdrNames.Add("Publisher");
            dtreport.Columns.Add("Publisher");

            DataRow drHdr1 = dtreport.NewRow();
            for (int grCol = 0; grCol < dtreport.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames[grCol];
            }
            dtreport.Rows.Add(drHdr1);

            Sql = "SELECT Acc_No,Title,Author,Publisher FROM BookDetails B,BookDoList S WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' ";
            if (booktype == "BOK")
                Sql = Sql + " AND Book_Status ='Issued' ";
            else
                Sql = Sql + " AND Issue_Flag ='Issued' ";
            Sql = Sql + "ORDER BY LEN(Acc_No),Acc_No ";
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    AccNo = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                    Sql = "SELECT * FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '' AND Acc_No_Sys ='" + AccNo + "' ";
                    dsload = d2.select_method_wo_parameter(Sql, "text");
                    if (dsload.Tables[0].Rows.Count == 0)
                    {
                        SNo++;
                        drow = dtreport.NewRow();
                        drow[0] = SNo;
                        drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                        drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                        drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                        drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["Publisher"]);
                        dtreport.Rows.Add(drow);
                    }
                }
            }
            grdReport.DataSource = dtreport;
            grdReport.DataBind();
            grdReport.Visible = true;
            grdReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
            grdReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            grdReport.Rows[0].Font.Bold = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void lnkbtn_IssueVerify_OnClick(object sender, EventArgs e)
    {
        try
        {
            chk_Lost.Visible = false;
            FldsetScan.Visible = false;
            divConPrevScan.Visible = false;
            Libcode = Convert.ToString(ddlLibrary.SelectedValue);
            fldstReport.Visible = true;
            LedgendName.InnerText = "Issued List (Verified)";
            arrColHdrNames.Add("S.No");
            dtreport.Columns.Add("S.No");
            arrColHdrNames.Add("Acc No");
            dtreport.Columns.Add("Acc No");
            arrColHdrNames.Add("Title");
            dtreport.Columns.Add("Title");
            arrColHdrNames.Add("Author");
            dtreport.Columns.Add("Author");
            arrColHdrNames.Add("Publisher");
            dtreport.Columns.Add("Publisher");

            DataRow drHdr1 = dtreport.NewRow();
            for (int grCol = 0; grCol < dtreport.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames[grCol];
            }
            dtreport.Rows.Add(drHdr1);

            Sql = "SELECT Acc_No,Title,Author,Publisher FROM BookDetails B,BookDoList S WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' ";

            if (booktype == "BOK")
                Sql = Sql + " AND Book_Status ='Issued' ";
            else
                Sql = Sql + " AND Issue_Flag ='Issued' ";
            Sql = Sql + "ORDER BY LEN(Acc_No),Acc_No ";
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    AccNo = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                    Sql = "SELECT * FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '' AND Acc_No_Sys ='" + AccNo + "' ";
                    dsload = d2.select_method_wo_parameter(Sql, "text");
                    if (dsload.Tables[0].Rows.Count > 0)
                    {
                        SNo++;
                        drow = dtreport.NewRow();
                        drow[0] = SNo;
                        drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                        drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                        drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                        drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["Publisher"]);
                        dtreport.Rows.Add(drow);
                    }
                }
            }
            grdReport.DataSource = dtreport;
            grdReport.DataBind();
            grdReport.Visible = true;
            grdReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
            grdReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            grdReport.Rows[0].Font.Bold = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void lnkbtn_Lost_OnClick(object sender, EventArgs e)
    {
        try
        {
            chk_Lost.Visible = false;
            FldsetScan.Visible = false;
            divConPrevScan.Visible = false;
            Libcode = Convert.ToString(ddlLibrary.SelectedValue);
            fldstReport.Visible = true;
            LedgendName.InnerText = "Lost List";
            arrColHdrNames.Add("S.No");
            dtreport.Columns.Add("S.No");
            arrColHdrNames.Add("Acc No");
            dtreport.Columns.Add("Acc No");
            arrColHdrNames.Add("Title");
            dtreport.Columns.Add("Title");
            arrColHdrNames.Add("Author");
            dtreport.Columns.Add("Author");
            arrColHdrNames.Add("Publisher");
            dtreport.Columns.Add("Publisher");

            DataRow drHdr1 = dtreport.NewRow();
            for (int grCol = 0; grCol < dtreport.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames[grCol];
            }
            dtreport.Rows.Add(drHdr1);

            Sql = "SELECT Acc_No,Title,Author,Publisher FROM BookDetails B,BookDoList S WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' ";
            if (booktype == "BOK")
                Sql = Sql + " AND Book_Status ='Lost' ";
            else
                Sql = Sql + " AND Issue_Flag ='Lost' ";
            Sql = Sql + "ORDER BY LEN(Acc_No),Acc_No ";
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    AccNo = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);

                    Sql = "SELECT * FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '' AND Acc_No_Sys ='" + AccNo + "' ";
                    dsload = d2.select_method_wo_parameter(Sql, "text");
                    if (dsload.Tables[0].Rows.Count == 0)
                    {
                        SNo++;
                        drow = dtreport.NewRow();
                        drow[0] = SNo;
                        drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                        drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                        drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                        drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["Publisher"]);
                        dtreport.Rows.Add(drow);
                    }
                }
            }
            grdReport.DataSource = dtreport;
            grdReport.DataBind();
            grdReport.Visible = true;
            grdReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
            grdReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            grdReport.Rows[0].Font.Bold = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void lnkbtn_Withdraw_OnClick(object sender, EventArgs e)
    {
        try
        {
            chk_Lost.Visible = false;
            FldsetScan.Visible = false;
            divConPrevScan.Visible = false;
            Libcode = Convert.ToString(ddlLibrary.SelectedValue);
            fldstReport.Visible = true;
            LedgendName.InnerText = "Withdrawn List";
            arrColHdrNames.Add("S.No");
            dtreport.Columns.Add("S.No");
            arrColHdrNames.Add("Acc No");
            dtreport.Columns.Add("Acc No");
            arrColHdrNames.Add("Title");
            dtreport.Columns.Add("Title");
            arrColHdrNames.Add("Author");
            dtreport.Columns.Add("Author");
            arrColHdrNames.Add("Publisher");
            dtreport.Columns.Add("Publisher");

            DataRow drHdr1 = dtreport.NewRow();
            for (int grCol = 0; grCol < dtreport.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames[grCol];
            }
            dtreport.Rows.Add(drHdr1);

            Sql = "SELECT Acc_No,Title,Author,Publisher FROM BookDetails B,BookDoList S WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' ";
            if (booktype == "BOK")
                Sql = Sql + " AND Book_Status ='Withdrawn' ";
            else
                Sql = Sql + " AND Issue_Flag ='Withdrawn' ";
            Sql = Sql + "ORDER BY LEN(Acc_No),Acc_No ";
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    AccNo = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                    Sql = "SELECT * FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '' AND Acc_No_Sys ='" + AccNo + "' ";
                    dsload = d2.select_method_wo_parameter(Sql, "text");
                    if (dsload.Tables[0].Rows.Count == 0)
                    {
                        SNo++;
                        drow = dtreport.NewRow();
                        drow[0] = SNo;
                        drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                        drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                        drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                        drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["Publisher"]);
                        dtreport.Rows.Add(drow);
                    }
                }
            }
            grdReport.DataSource = dtreport;
            grdReport.DataBind();
            grdReport.Visible = true;
            grdReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
            grdReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            grdReport.Rows[0].Font.Bold = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void lnkbtn_Missing_OnClick(object sender, EventArgs e)
    {
        try
        {
            chk_Lost.Visible = false;
            FldsetScan.Visible = false;
            divConPrevScan.Visible = false;
            Libcode = Convert.ToString(ddlLibrary.SelectedValue);
            fldstReport.Visible = true;
            LedgendName.InnerText = "Missing List";
            arrColHdrNames.Add("S.No");
            dtreport.Columns.Add("S.No");
            arrColHdrNames.Add("Acc No");
            dtreport.Columns.Add("Acc No");
            arrColHdrNames.Add("Title");
            dtreport.Columns.Add("Title");
            arrColHdrNames.Add("Author");
            dtreport.Columns.Add("Author");
            arrColHdrNames.Add("Publisher");
            dtreport.Columns.Add("Publisher");

            DataRow drHdr1 = dtreport.NewRow();
            for (int grCol = 0; grCol < dtreport.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames[grCol];
            }
            dtreport.Rows.Add(drHdr1);

            Sql = "SELECT Acc_No,Title,Author,Publisher FROM BookDetails B,BookDoList S WHERE B.Acc_No = S.Acc_No_Sys AND B.Lib_Code = S.Lib_Code AND B.Lib_Code ='" + Libcode + "' ";
            if (booktype == "BOK")
                Sql = Sql + " AND Book_Status ='Missing' ";
            else
                Sql = Sql + " AND Issue_Flag ='Missing' ";
            Sql = Sql + "ORDER BY LEN(Acc_No),Acc_No ";
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    AccNo = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                    Sql = "SELECT * FROM BookDoList WHERE BookType ='" + booktype + "' AND Lib_Code ='" + Libcode + "' AND Acc_No_Phy <> '' AND Acc_No_Sys ='" + AccNo + "' ";
                    dsload = d2.select_method_wo_parameter(Sql, "text");
                    if (dsload.Tables[0].Rows.Count == 0)
                    {
                        SNo++;
                        drow = dtreport.NewRow();
                        drow[0] = SNo;
                        drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                        drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                        drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                        drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["Publisher"]);
                        dtreport.Rows.Add(drow);
                    }
                }
            }
            grdReport.DataSource = dtreport;
            grdReport.DataBind();
            grdReport.Visible = true;
            grdReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
            grdReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            grdReport.Rows[0].Font.Bold = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnprint_Click(object sender, EventArgs e)
    {
        try
        {
            PdfDocument mydoc = new PdfDocument(PdfDocumentFormat.InCentimeters(29, 34.3));
            Gios.Pdf.PdfDocument mypdf = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage deptpdfpage;
            int coltop = 25;
            
            Font Fontbold = new Font("Times New Roman", 20, FontStyle.Bold);
            Font FontHeader = new Font("Times New Roman", 18, FontStyle.Bold);           
            deptpdfpage = mypdf.NewPage();
            Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
            mypdfpage = mydoc.NewPage();
            string collegename = "";
            string address1 = "";
            string address2 = "";
            string address3 = "";
            string pincode = "";
            string CurrentDate = d2.ServerDate();
            string[] dat = CurrentDate.Split('/');
            if (dat.Length == 3)
                CurrentDate = dat[1] + "/" + dat[0] + "/" + dat[2];

            string colquery = "select collname,address1,address2,address3,pincode from collinfo where college_code='" + ddlCollege.SelectedItem.Value + "'";
            DataSet ds1 = d2.select_method_wo_parameter(colquery, "Text");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                collegename = Convert.ToString(ds1.Tables[0].Rows[0]["collname"]);
                address1 = Convert.ToString(ds1.Tables[0].Rows[0]["address1"]);
                address2 = Convert.ToString(ds1.Tables[0].Rows[0]["address2"]);
                address3 = Convert.ToString(ds1.Tables[0].Rows[0]["address3"]);
                pincode = Convert.ToString(ds1.Tables[0].Rows[0]["pincode"]);
            }

            PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                new PdfArea(mydoc, 225, 50, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, collegename);
            mypdfpage.Add(ptc);
            PdfTextArea ptc1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                               new PdfArea(mydoc, 225, 70, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, address1 + "," + address2 + "," + address3 + "-" + pincode);
            mypdfpage.Add(ptc1);
            PdfTextArea ptc2 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                               new PdfArea(mydoc, 225, 100, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "Library Stock Verification Report - " + ddlBookType.SelectedItem.Text);
            mypdfpage.Add(ptc2);
            PdfTextArea ptc3 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                              new PdfArea(mydoc, 600, 130, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date - " + CurrentDate);
            mypdfpage.Add(ptc3);

            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(13).jpeg")))
            {
                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(13).jpeg"));
                mypdfpage.Add(LogoImage, 15, 10, 250);
            }

            PdfArea tete = new PdfArea(mydoc, 20, 170, 780, 510);
            PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
            mypdfpage.Add(pr1);
            coltop = coltop + 170;
            PdfArea tetet = new PdfArea(mydoc, 20, 170, 780, 60);
            PdfRectangle pr1t = new PdfRectangle(mydoc, tetet, Color.Black);
            mypdfpage.Add(pr1t);
            PdfArea tetLast = new PdfArea(mydoc, 20, 680, 780, 60);
            PdfRectangle pr2 = new PdfRectangle(mydoc, tetLast, Color.Black);
            mypdfpage.Add(pr2);
            PdfTextArea ptcsubreg;
            PdfTextArea ptcfn;

            ptcsubreg = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 40, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total No. of Resources as per stock  ");
            mypdfpage.Add(ptcsubreg);
            ptcfn = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, 700, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, LblTotalValue.Text);
            mypdfpage.Add(ptcfn);
            coltop = coltop + 50;

            PdfTextArea ptc08 = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 40, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total No. of Resources as per physical verification");
            mypdfpage.Add(ptc08);
            PdfTextArea ptc08na1 = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 700, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, LblStockValue.Text);
            mypdfpage.Add(ptc08na1);
            coltop = coltop + 50;
            PdfTextArea ptcsem = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 40, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total No. of Resources under Binding");
            mypdfpage.Add(ptcsem);
            PdfTextArea ptcsem1 = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 700, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, LblBindingValue.Text);
            mypdfpage.Add(ptcsem1);
            coltop = coltop + 50;
            PdfTextArea ptctrans = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 40, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total No. of Resources Transfered to other department");
            mypdfpage.Add(ptctrans);
            PdfTextArea ptctrans1 = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 700, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, LblTransferedValue.Text);
            mypdfpage.Add(ptctrans1);
            coltop = coltop + 50;
            PdfTextArea ptcissuever = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 40, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total No. of Resources under Issued (not verified)");
            mypdfpage.Add(ptcissuever);
            PdfTextArea ptcissuever1 = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 700, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, LblIssuedValue.Text);
            mypdfpage.Add(ptcissuever1);
            coltop = coltop + 50;
            PdfTextArea ptcissuever2 = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 40, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total No. of Resources under Issued (verified)");
            mypdfpage.Add(ptcissuever2);
            PdfTextArea ptcissuever3 = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 700, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, LblIssueVerifyValue.Text);
            mypdfpage.Add(ptcissuever3);
            coltop = coltop + 50;
            PdfTextArea ptcLost = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 40, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total No. of Resources alread Lost (Duly Accounted)");
            mypdfpage.Add(ptcLost);
            PdfTextArea ptcLost1 = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 700, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, LblLostValue.Text);
            mypdfpage.Add(ptcLost1);
            coltop = coltop + 50;
            PdfTextArea ptcwith = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 40, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total No. of Resources under Withdrawn");
            mypdfpage.Add(ptcwith);
            PdfTextArea ptcwith1 = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 700, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, LblWithdrawnValue.Text);
            mypdfpage.Add(ptcwith1);
            coltop = coltop + 50;
            PdfTextArea ptcRes = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 40, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total No. of Resources under Missing");
            mypdfpage.Add(ptcRes);
            PdfTextArea ptcRes1 = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 700, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, LblMissingValue.Text);
            mypdfpage.Add(ptcRes1);
            double nettotal = 0;
            nettotal = Convert.ToDouble(LblStockValue.Text) + Convert.ToDouble(LblBindingValue.Text) + Convert.ToDouble(LblTransferedValue.Text) + Convert.ToDouble(LblIssuedValue.Text) + Convert.ToDouble(LblLostValue.Text) + Convert.ToDouble(LblWithdrawnValue.Text) + Convert.ToDouble(LblMissingValue.Text) + Convert.ToDouble(LblIssueVerifyValue.Text);
            coltop = coltop + 50;
            PdfTextArea ptcNet = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 40, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Net Total");
            mypdfpage.Add(ptcNet);
            PdfTextArea ptcNet1 = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 700, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(nettotal));
            mypdfpage.Add(ptcNet1);

            double totalMissing = 0;
            totalMissing = Convert.ToDouble(LblTotalValue.Text) - (Convert.ToDouble(LblStockValue.Text) + Convert.ToDouble(LblBindingValue.Text) + Convert.ToDouble(LblTransferedValue.Text) + Convert.ToDouble(LblIssuedValue.Text) + Convert.ToDouble(LblLostValue.Text) + Convert.ToDouble(LblWithdrawnValue.Text) + Convert.ToDouble(LblMissingValue.Text) + Convert.ToDouble(LblIssueVerifyValue.Text));


            coltop = coltop + 50;
            PdfTextArea ptcMiss = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 40, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total No. of Resources Missing");
            mypdfpage.Add(ptcMiss);
            PdfTextArea ptcMiss1 = new PdfTextArea(FontHeader, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 700, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(totalMissing));
            mypdfpage.Add(ptcMiss1);

            mypdfpage.SaveToDocument();
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "FeeStatus" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmss") + ".pdf";
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void txt_accno_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            Libcode = Convert.ToString(ddlLibrary.SelectedValue);
            fldstReport.Visible = true;
            LedgendName.InnerText = "Missing List";
            arrColHdrNames.Add("S.No");
            dtreport.Columns.Add("S.No");
            arrColHdrNames.Add("Acc No");
            dtreport.Columns.Add("Acc No");
            arrColHdrNames.Add("Title");
            dtreport.Columns.Add("Title");
            arrColHdrNames.Add("Author");
            dtreport.Columns.Add("Author");
            arrColHdrNames.Add("Publisher");
            dtreport.Columns.Add("Publisher");
            arrColHdrNames.Add("Book Status");
            dtreport.Columns.Add("Book Status");

            DataRow drHdr1 = dtreport.NewRow();
            for (int grCol = 0; grCol < dtreport.Columns.Count; grCol++)
            {
                drHdr1[grCol] = arrColHdrNames[grCol];
            }
            dtreport.Rows.Add(drHdr1);


            Sql = "SELECT Acc_No,Title,Author,Publisher,Book_Status FROM BookDetails WHERE Acc_No ='" + Txt_AccNo.Text + "' AND Lib_Code ='" + Libcode + "' ";
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SNo++;
                    drow = dtreport.NewRow();
                    drow[0] = SNo;
                    drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                    drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                    drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                    drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["Publisher"]);
                    drow[5] = Convert.ToString(ds.Tables[0].Rows[i]["Book_Status"]);
                    dtreport.Rows.Add(drow);
                }
            }
            grdReport.DataSource = dtreport;
            grdReport.DataBind();
            grdReport.Visible = true;
            grdReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
            grdReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            grdReport.Rows[0].Font.Bold = true;
        }
        catch (Exception ex)
        {
        }
    }

    #region Scan

    protected void rbNewScan_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbNewScan.Checked)
        {
            rbContinueScan.Checked = false;
        }
    }

    protected void rbContinueScan_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbContinueScan.Checked)
        {
            rbNewScan.Checked = false;
        }
    }

    protected void BtnOk_Click(object sender, EventArgs e)
    {
        if (rbNewScan.Checked == true)
        {
            DivYesOrNo.Visible = true;
        }
        fillgrid(sender, e);
    }

    protected void BtnYes_Click(object sender, EventArgs e)
    {
        Libcode = Convert.ToString(ddlLibrary.SelectedValue);
        string BokTyp = Convert.ToString(ddlBookType.SelectedItem.Text);
        Sql = " delete from bookdolist where booktype = '" + booktype + "' and lib_code = '" + Libcode + "'";
        delete = d2.update_method_wo_parameter(Sql, "Text");
        if (!chk_Lost.Checked)
        {
            if (BokTyp == "Books")
                Sql = "select " + QueryStr + " lib_code ='" + Libcode + "' and book_status<>'Lost'";
            else if (BokTyp == "Project Books")
                Sql = "select " + QueryStr + " lib_code ='" + Libcode + "' and Issue_Flag<>'Lost'";
        }
        else
        {
            Sql = "select " + QueryStr + " lib_code ='" + Libcode + "'";
        }
        Sql = "insert into bookdolist (acc_no_sys,acc_no_phy,booktype,lib_code,scandate) " + Sql;
        insert = d2.update_method_wo_parameter(Sql, "Text");
        fillgrid(sender, e);
    }

    protected void BtnNo_Click(object sender, EventArgs e)
    {
    }

    protected void fillgrid(object sender, EventArgs e)
    {
        tableBooklist.Visible = false;
        divConPrevScan.Visible = true;
        Libcode = Convert.ToString(ddlLibrary.SelectedValue);
        Sql = "select acc_no_sys,acc_no_phy from bookdolist where booktype = '" + booktype + "' and lib_code = '" + Libcode + "' ORDER BY LEN(Acc_No_Sys),Acc_No_Sys ";
        ds = d2.select_method_wo_parameter(Sql, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            lbl_noofrec.Text = "No of Books : " + ds.Tables[0].Rows.Count;
            arrColHdrNames.Add("S.No");
            arrColHdrNames.Add("Access Number Before Scanning");
            arrColHdrNames.Add("Access Number After Scanning");
            dtreport.Columns.Add("Sno");
            dtreport.Columns.Add("Access Number Before Scanning");
            dtreport.Columns.Add("Access Number After Scanning");
            DataRow drHdr1 = dtreport.NewRow();
            for (int grCol = 0; grCol < dtreport.Columns.Count; grCol++)
                drHdr1[grCol] = arrColHdrNames[grCol];
            dtreport.Rows.Add(drHdr1);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                SNo++;
                drow = dtreport.NewRow();
                drow["Sno"] = SNo;
                drow["Access Number Before Scanning"] = Convert.ToString(ds.Tables[0].Rows[i]["acc_no_sys"]);
                drow["Access Number After Scanning"] = Convert.ToString(ds.Tables[0].Rows[i]["acc_no_phy"]);
                dtreport.Rows.Add(drow);
            }
        }
        ViewState["CurrentTable"] = dtreport;
        GrdScanBook.DataSource = dtreport;
        GrdScanBook.DataBind();
        GrdScanBook.Visible = true;
        if (GrdScanBook.Rows.Count > 0)
        {
            GrdScanBook.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
            GrdScanBook.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            GrdScanBook.Rows[0].Font.Bold = true;
        }
        btnStartScan.Enabled = true;
        btnUndoScan.Enabled = true;
        btnComPrint.Enabled = true;
        btnConfirm.Enabled = true;
        btnBack.Enabled = true;
    }

    protected void GrdScanBook_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[2].Visible = false;
        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
        e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                e.Row.Cells[1].Text = "Select";
            }
        }
    }

    #region UndoScan

    protected void btnUndoScan_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow gvrow in GrdScanBook.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                if (chk.Checked == true)
                {
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    string accnum = Convert.ToString(GrdScanBook.Rows[RowCnt].Cells[3].Text);
                    UndoFromGrid(accnum);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void UndoFromGrid(string AccNo)
    {
        string StrLib_Code = Convert.ToString(ddlLibrary.SelectedValue);
        DataTable dt = new DataTable();
        string SerDate = d2.ServerDate();
        for (int i = 1; i < GrdScanBook.Rows.Count; i++)
        {
            string Access = Convert.ToString(GrdScanBook.Rows[i].Cells[3].Text);
            if (Access == AccNo)
            {
                Sql = "update bookdolist set acc_no_phy = '',scandate='" + SerDate + "' where acc_no_sys = '" + Access + "' and lib_code = '" + StrLib_Code + "'";
                int update = d2.update_method_wo_parameter(Sql, "text");
                BlnDelToSpread = true;
            }
            if (BlnHeaderBind == false)
            {
                if (ViewState["CurrentTable"] != null)
                {
                    dt = (DataTable)ViewState["CurrentTable"];
                    arrColHdrNames.Add("S.No");
                    arrColHdrNames.Add("Access Number Before Scanning");
                    arrColHdrNames.Add("Access Number After Scanning");
                    dtreport.Columns.Add("Sno");
                    dtreport.Columns.Add("Access Number Before Scanning");
                    dtreport.Columns.Add("Access Number After Scanning");
                    DataRow drHdr1 = dtreport.NewRow();
                    for (int grCol = 0; grCol < dtreport.Columns.Count; grCol++)
                        drHdr1[grCol] = arrColHdrNames[grCol];
                    dtreport.Rows.Add(drHdr1);
                }
                BlnHeaderBind = true;
            }
            SNo++;
            drow = dtreport.NewRow();
            drow["Sno"] = SNo;
            drow["Access Number Before Scanning"] = Convert.ToString(dt.Rows[i]["Access Number Before Scanning"]);
            string AccBeforeScanning = Convert.ToString(dt.Rows[i]["Access Number Before Scanning"]);
            if (AccBeforeScanning == AccNo)
            {
                drow["Access Number After Scanning"] = "";
            }
            else
            {
                drow["Access Number After Scanning"] = Convert.ToString(dt.Rows[i]["Access Number After Scanning"]);
            }
            dtreport.Rows.Add(drow);
        }
        ViewState["CurrentTable"] = dtreport;
        GrdScanBook.DataSource = dtreport;
        GrdScanBook.DataBind();
        GrdScanBook.Visible = true;
        GrdScanBook.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
        GrdScanBook.Rows[0].HorizontalAlign = HorizontalAlign.Center;
        GrdScanBook.Rows[0].Font.Bold = true;
        BlnHeaderBind = false;
    }

    #endregion

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        DivScanConfirm.Visible = true;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        divConPrevScan.Visible = false;
        tableBooklist.Visible = true;
        btnStartScan.Enabled = false;
        btnUndoScan.Enabled = false;
        btnComPrint.Enabled = false;
        btnConfirm.Enabled = false;
        btnBack.Enabled = false;
    }

    #region StartScan

    protected void btnStartScan_Click(object sender, EventArgs e)
    {
        DivScanYes.Visible = true;
    }

    protected void BtnScanYes_Click(object sender, EventArgs e)
    {
    }

    protected void BtnScanNo_Click(object sender, EventArgs e)
    {
        DivScanYes.Visible = false;
        DivBookScanning.Visible = true;
        Page.Form.DefaultFocus = txt_access.ClientID;
    }

    #region Add

    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string accNo = Convert.ToString(txt_access.Text);
            AddtoGrid(accNo);
            if (BlnAddToSpread == true)// Not IsSelected(txt_accno.Text) Then
            {
                DivErrorMsg.Visible = true;
                LblErrorMsg.Text = "Invalid AccNo.";
                BlnAddToSpread = false;
            }
            txt_access.Text = "";
            Page.Form.DefaultFocus = txt_access.ClientID;
        }
        catch (Exception ex)
        {
        }
    }

    protected void AddtoGrid(string accNo)
    {
        try
        {
            string StrLib_Code = Convert.ToString(ddlLibrary.SelectedValue);
            string bok = Convert.ToString(ddlBookType.SelectedItem.Text);
            string CurrentDate = d2.ServerDate();
            string AccBeforeScanning = "";
            if (bok == "Books")
            {
                booktype = "BOK";
                QueryStr = " acc_no,'','" + booktype + "','" + StrLib_Code + "','" + CurrentDate + "' from bookdetails ";
                querystr1 = " book_status from bookdetails ";
                querystr2 = " acc_no ";
                QueryTable = " FROM BookDetails ";
            }
            if (bok == "Project Books")
            {
                booktype = "PRO";
                QueryStr = " probook_accno,'','" + booktype + "','" + StrLib_Code + "','" + CurrentDate + "'  from project_book where ";
                querystr1 = " issue_flag from project_book ";
                querystr2 = " probook_accno ";
                QueryTable = " FROM Project_Book ";
            }
            if (bok == "Non Book Materials")
            {
                booktype = "NBM";
                QueryStr = " nonbookmat_no,'','" + booktype + "','" + StrLib_Code + "','" + CurrentDate + "'  from nonbookmat where issue_flag='Available' ";
                querystr1 = " issue_flag from nonbookmat ";
                querystr2 = " nonbookmat_no ";
                QueryTable = " FROM NonBookMat ";
            }
            if (bok == "Back Volume")
            {
                booktype = "BVO";
                QueryStr = " access_code,'','" + booktype + "','" + StrLib_Code + "','" + CurrentDate + "'  from back_volume where issue_flag='Available' ";
                querystr1 = " issue_flag from back_volume ";
                querystr2 = " access_code ";
                QueryTable = " FROM Back_Volume ";
            }
            DataTable dt = new DataTable();
            bool blnExist = false;
            string strStatus = "";
            Libcode = Convert.ToString(ddlLibrary.SelectedValue);
            Sql = "select " + querystr1 + " where " + querystr2 + " ='" + accNo + "' and  lib_code='" + Libcode + "'";
            ds = d2.select_method_wo_parameter(Sql, "text");
            string SerDate = d2.ServerDate();
            string FinalAccNo = Convert.ToString(txt_access.Text);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string bookStatus = Convert.ToString(ds.Tables[0].Rows[0]["book_status"]);
                if (bookStatus != "Available" && bookStatus != "Lost")
                {
                    blnExist = true;
                    DivErrorMsg.Visible = true;
                    LblErrorMsg.Text = "Acc No. " + accNo + " is under " + bookStatus;
                    strStatus = bookStatus;
                }

                sflag = false;
                for (int i = 1; i < GrdScanBook.Rows.Count; i++)
                {
                    string Access = Convert.ToString(GrdScanBook.Rows[i].Cells[3].Text);
                    if (Access == accNo)
                    {
                        if (blnExist == false)
                        {
                            tmpvar = Convert.ToString(GrdScanBook.Rows[i].Cells[4].Text);
                            if (tmpvar == accNo)
                            {
                                DivErrorMsg.Visible = true;
                                LblErrorMsg.Text = "Acc No. " + accNo + " Already Scanned ";
                                BlnAddToSpread = true;
                            }
                            else
                            {
                                sflag = false;
                                if (sflag == false)
                                {
                                    Sql = "update bookdolist set acc_no_phy = '" + Access + "',scandate='" + SerDate + "' where acc_no_sys = '" + Access + "' and lib_code = '" + StrLib_Code + "'";
                                    int update = d2.update_method_wo_parameter(Sql, "text");
                                    txt_access.Text = "";
                                }
                                else
                                {
                                    DivErrorMsg.Visible = true;
                                    LblErrorMsg.Text = "Book not found";
                                }
                            }
                        }
                    }

                    if (BlnHeaderBind == false)
                    {
                        if (ViewState["CurrentTable"] != null)
                        {
                            dt = (DataTable)ViewState["CurrentTable"];
                            arrColHdrNames.Add("S.No");
                            arrColHdrNames.Add("Access Number Before Scanning");
                            arrColHdrNames.Add("Access Number After Scanning");
                            dtreport.Columns.Add("Sno");
                            dtreport.Columns.Add("Access Number Before Scanning");
                            dtreport.Columns.Add("Access Number After Scanning");
                            DataRow drHdr1 = dtreport.NewRow();
                            for (int grCol = 0; grCol < dtreport.Columns.Count; grCol++)
                                drHdr1[grCol] = arrColHdrNames[grCol];
                            dtreport.Rows.Add(drHdr1);
                        }
                        BlnHeaderBind = true;
                    }
                    SNo++;
                    drow = dtreport.NewRow();
                    drow["Sno"] = SNo;
                    drow["Access Number Before Scanning"] = Convert.ToString(dt.Rows[i]["Access Number Before Scanning"]);
                    AccBeforeScanning = Convert.ToString(dt.Rows[i]["Access Number Before Scanning"]);
                    if (AccBeforeScanning == FinalAccNo)
                    {
                        drow["Access Number After Scanning"] = AccBeforeScanning;
                    }
                    else
                    {
                        drow["Access Number After Scanning"] = Convert.ToString(dt.Rows[i]["Access Number After Scanning"]);
                    }
                    dtreport.Rows.Add(drow);
                }
                ViewState["CurrentTable"] = dtreport;
                GrdScanBook.DataSource = dtreport;
                GrdScanBook.DataBind();
                GrdScanBook.Visible = true;
                GrdScanBook.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
                GrdScanBook.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                GrdScanBook.Rows[0].Font.Bold = true;
                BlnHeaderBind = false;
            }
            else
            {
                BlnAddToSpread = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        DivErrorMsg.Visible = false;
    }

    #region Delete

    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string accNo = Convert.ToString(txt_access.Text);
            DelFromGrid(accNo);
            if (!BlnDelToSpread == true)// Not IsSelected(txt_accno.Text) Then
            {
                DivErrorMsg.Visible = true;
                LblErrorMsg.Text = "Invalid AccNo.";
                BlnDelToSpread = false;
            }
            txt_access.Text = "";
            Page.Form.DefaultFocus = txt_access.ClientID;
        }
        catch (Exception ex)
        {
        }
    }

    protected void DelFromGrid(string AccNo)
    {
        string StrLib_Code = Convert.ToString(ddlLibrary.SelectedValue);
        DataTable dt = new DataTable();
        string SerDate = d2.ServerDate();
        string FinalAccNo = Convert.ToString(txt_access.Text);
        for (int i = 1; i < GrdScanBook.Rows.Count; i++)
        {
            string Access = Convert.ToString(GrdScanBook.Rows[i].Cells[3].Text);
            if (Access == AccNo)
            {
                Sql = "update bookdolist set acc_no_phy = '',scandate='" + SerDate + "' where acc_no_sys = '" + Access + "' and lib_code = '" + StrLib_Code + "'";
                int update = d2.update_method_wo_parameter(Sql, "text");
                BlnDelToSpread = true;
            }
            if (BlnHeaderBind == false)
            {
                if (ViewState["CurrentTable"] != null)
                {
                    dt = (DataTable)ViewState["CurrentTable"];
                    arrColHdrNames.Add("S.No");
                    arrColHdrNames.Add("Access Number Before Scanning");
                    arrColHdrNames.Add("Access Number After Scanning");
                    dtreport.Columns.Add("Sno");
                    dtreport.Columns.Add("Access Number Before Scanning");
                    dtreport.Columns.Add("Access Number After Scanning");
                    DataRow drHdr1 = dtreport.NewRow();
                    for (int grCol = 0; grCol < dtreport.Columns.Count; grCol++)
                        drHdr1[grCol] = arrColHdrNames[grCol];
                    dtreport.Rows.Add(drHdr1);
                }
                BlnHeaderBind = true;
            }
            SNo++;
            drow = dtreport.NewRow();
            drow["Sno"] = SNo;
            drow["Access Number Before Scanning"] = Convert.ToString(dt.Rows[i]["Access Number Before Scanning"]);
            string AccBeforeScanning = Convert.ToString(dt.Rows[i]["Access Number Before Scanning"]);
            if (AccBeforeScanning == FinalAccNo)
            {
                drow["Access Number After Scanning"] = "";
            }
            else
            {
                drow["Access Number After Scanning"] = Convert.ToString(dt.Rows[i]["Access Number After Scanning"]);
            }
            dtreport.Rows.Add(drow);
        }
        ViewState["CurrentTable"] = dtreport;
        GrdScanBook.DataSource = dtreport;
        GrdScanBook.DataBind();
        GrdScanBook.Visible = true;
        GrdScanBook.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
        GrdScanBook.Rows[0].HorizontalAlign = HorizontalAlign.Center;
        GrdScanBook.Rows[0].Font.Bold = true;
        BlnHeaderBind = false;
    }

    #endregion

    #region Confirmation Click

    protected void BtnConfirmation_Click(object sender, EventArgs e)
    {
        DivScanConfirm.Visible = true;
    }

    protected void Btn_OK_Click(object sender, EventArgs e)
    {
        try
        {
            int cnt = 0;
            string bok = Convert.ToString(ddlBookType.SelectedItem.Text);
            string StrLib_Code = Convert.ToString(ddlLibrary.SelectedValue);
            string AccNo = Convert.ToString(txt_access.Text);
            if (AccNo == "")
            {
                DivErrorMsg.Visible = true;
                LblErrorMsg.Text = "Enter access number";
            }
            string CurrentDate = d2.ServerDate();
            string[] dat = CurrentDate.Split('/');
            if (bok == "Books")
            {
                Sql = "select * from bookdetails where ltrim(rtrim(acc_no)) = '" + AccNo + "' and lib_code='" + StrLib_Code + "'";
                ds = d2.select_method_wo_parameter(Sql, "Text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string BokStatus = Convert.ToString(ds.Tables[0].Rows[0]["book_status"]);
                    if (BokStatus == "Lost")
                    {
                        DivBkStatus.Visible = true;
                        LblBookStatus.Text = "AccNo. " + AccNo + " is under lost.Do you want to change the status as Available?";
                    }
                    else if (BokStatus == "condemn")
                    {
                        Sql = "update bookdetails set book_status = 'condemn' where ltrim(rtrim(acc_no)) = '" + AccNo + "'  and lib_code ='" + StrLib_Code + "'";
                        update = d2.update_method_wo_parameter(Sql, "text");

                        Sql = "delete from bookstatus where ltrim(rtrim(acc_no)) = '" + AccNo + "' and y_lost = '" + dat[2] + "' and book_type = 'BOK' and lib_code='" + StrLib_Code + "'";
                        delete = d2.update_method_wo_parameter(Sql, "text");
                        Sql = "insert into book_condemn values ('" + AccNo + "','" + bok + "','" + dat[2] + "','" + StrLib_Code + "')";
                        insert = d2.update_method_wo_parameter(Sql, "text");
                    }
                    else if (BokStatus == "Available")
                    {
                        Sql = "delete from bookstatus where ltrim(rtrim(acc_no)) = '" + AccNo + "' and y_lost = '" + dat[2] + "' and book_type = 'BOK' and lib_code='" + StrLib_Code + "'";
                    }
                }
            }
            if (bok == "Project Books")
            {
                Sql = "select * from project_book where probook_Accno = '" + AccNo + "' and lib_code='" + StrLib_Code + "' ";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string BokStatus = Convert.ToString(ds.Tables[0].Rows[0]["issue_flag"]);
                    if (BokStatus == "Lost")
                    {
                        DivBkStatus.Visible = true;
                        LblBookStatus.Text = "AccNo. " + AccNo + " is under lost.Do you want to change the status as Available?";

                    }
                    if (BokStatus == "condemn")
                    {
                        Sql = "update project_book set issue_flag = 'condemn' where probook_accno = '" + AccNo + "'  and lib_code ='" + StrLib_Code + "'";
                        update = d2.update_method_wo_parameter(Sql, "text");
                        Sql = "delete from bookstatus where acc_no = '" + AccNo + "' and y_lost = '" + dat[2] + "' and book_type ='PRO'and lib_code='" + StrLib_Code + "'";
                        delete = d2.update_method_wo_parameter(Sql, "text");
                        Sql = "insert into book_condemn values ('" + AccNo + "','" + booktype + "','" + dat[2] + "','" + StrLib_Code + "')";
                        insert = d2.update_method_wo_parameter(Sql, "text");
                    }
                    if (BokStatus == "Available")
                    {
                        Sql = "delete from bookstatus where ltrim(rtrim(acc_no)) = '" + AccNo + "' and y_lost = '" + dat[2] + "' and book_type = 'PRO' and lib_code='" + StrLib_Code + "'";
                        delete = d2.update_method_wo_parameter(Sql, "text");
                    }
                }
            }
            if (bok == "Non Book Materials")
            {
                Sql = "select * from nonbookmat where nonbookmat_no = '" + AccNo + "' and lib_code='" + StrLib_Code + "'";
                ds = d2.select_method_wo_parameter(Sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string BokStatus = Convert.ToString(ds.Tables[0].Rows[0]["issue_flag"]);
                    if (BokStatus == "Lost")
                    {
                        DivBkStatus.Visible = true;
                        LblBookStatus.Text = "AccNo. " + AccNo + " is under lost.Do you want to change the status as Available?";
                    }
                    if (BokStatus == "condemn")
                    {
                        Sql = "update nonbookmat set issue_flag = 'condemn' where nonbookmat_no = '" + AccNo + "'  and lib_code ='" + StrLib_Code + "'";
                        update = d2.update_method_wo_parameter(Sql, "text");
                        Sql = "delete from bookstatus where acc_no = '" + AccNo + "' and y_lost = '" + dat[2] + "' and book_type ='NBM'and lib_code='" + StrLib_Code + "'";
                        delete = d2.update_method_wo_parameter(Sql, "text");
                        Sql = "insert into book_condemn values ('" + AccNo + "','" + booktype + "','" + dat[2] + "','" + StrLib_Code + "')";
                        insert = d2.update_method_wo_parameter(Sql, "text");
                    }
                    if (BokStatus == "Available")
                    {
                        Sql = "delete from bookstatus where ltrim(rtrim(acc_no)) = '" + AccNo + "' and y_lost = '" + dat[2] + "' and book_type = 'NBM' and lib_code='" + StrLib_Code + "'";
                        delete = d2.update_method_wo_parameter(Sql, "text");
                    }
                }
            }
            if (bok == "Back Volume")
            {
                Sql = "select * from back_volume where access_code = '" + AccNo + "' and lib_code='" + StrLib_Code + "'";
                ds = d2.select_method_wo_parameter(Sql, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string BokStatus = Convert.ToString(ds.Tables[0].Rows[0]["issue_flag"]);
                    if (BokStatus == "Lost")
                    {
                        DivBkStatus.Visible = true;
                        LblBookStatus.Text = "AccNo. " + AccNo + " is under lost.Do you want to change the status as Available?";
                    }
                    if (BokStatus == "condemn")
                    {
                        Sql = "update back_volume set issue_flag = 'condemn' where access_code = '" + AccNo + "'  and lib_code ='" + StrLib_Code + "'";
                        update = d2.update_method_wo_parameter(Sql, "text");
                        Sql = "delete from bookstatus where acc_no = '" + AccNo + "' and y_lost = '" + dat[2] + "' and book_type ='BVO'and lib_code='" + StrLib_Code + "'";
                        delete = d2.update_method_wo_parameter(Sql, "text");
                        Sql = "insert into book_condemn values ('" + AccNo + "','" + booktype + "','" + dat[2] + "','" + StrLib_Code + "')";
                        insert = d2.update_method_wo_parameter(Sql, "text");
                    }
                    if (BokStatus == "Available")
                    {
                        Sql = "delete from bookstatus where ltrim(rtrim(acc_no)) = '" + AccNo + "' and y_lost = '" + dat[2] + "' and book_type = 'BVO' and lib_code='" + StrLib_Code + "'";
                        update = d2.update_method_wo_parameter(Sql, "text");
                    }
                }
            }
            if (insert > 0 || update > 0 || delete > 0)
            {
                DivErrorMsg.Visible = true;
                LblErrorMsg.Text = "Scanned Data were Successfully  Updated.....";
                txt_access.Text = "";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        DivScanConfirm.Visible = false;
    }

    protected void BtnBkStatusYes_Click(object sender, EventArgs e)
    {
        try
        {
            string CurrentDate = d2.ServerDate();
            string[] dat = CurrentDate.Split('/');
            string StrLib_Code = Convert.ToString(ddlLibrary.SelectedValue);
            string bok = Convert.ToString(ddlBookType.SelectedItem.Text);
            AccNo = Convert.ToString(txt_access.Text);
            if (bok == "Books")
            {
                Sql = "update bookdetails set book_status = 'Available' where ltrim(rtrim(acc_no)) = '" + AccNo + "'  and lib_code ='" + StrLib_Code + "'";
                update = d2.update_method_wo_parameter(Sql, "text");
                Sql = "delete from bookstatus where ltrim(rtrim(acc_no)) = '" + AccNo + "' and y_lost = '" + dat[2] + "' and book_type = 'BOK' and lib_code='" + StrLib_Code + "'";
                delete = d2.update_method_wo_parameter(Sql, "text");
            }
            if (bok == "Project Books")
            {
                Sql = "update project_book set issue_flag = 'Available' where probook_accno = '" + AccNo + "'  and lib_code ='" + StrLib_Code + "'";
                update = d2.update_method_wo_parameter(Sql, "text");
                Sql = "delete from bookstatus where acc_no = '" + AccNo + "' and y_lost = '" + dat[2] + "' and book_type ='PRO'and lib_code='" + StrLib_Code + "'";
                delete = d2.update_method_wo_parameter(Sql, "text");
            }
            if (bok == "Non Book Materials")
            {
                Sql = "update nonbookmat set issue_flag = 'Available' where nonbookmat_no = '" + AccNo + "'  and lib_code ='" + StrLib_Code + "'";
                update = d2.update_method_wo_parameter(Sql, "text");
                Sql = "delete from bookstatus where acc_no = '" + AccNo + "' and y_lost = '" + dat[2] + "' and book_type ='NBM'and lib_code='" + StrLib_Code + "'";
                delete = d2.update_method_wo_parameter(Sql, "text");
            }
            if (bok == "Back Volume")
            {
                Sql = "update back_volume set issue_flag = 'Available' where access_code = '" + AccNo + "'  and lib_code ='" + StrLib_Code + "'";
                update = d2.update_method_wo_parameter(Sql, "text");
                Sql = "delete from bookstatus where acc_no = '" + AccNo + "' and y_lost = '" + dat[2] + "' and book_type ='BVO'and lib_code='" + StrLib_Code + "'";
                delete = d2.update_method_wo_parameter(Sql, "text");
            }
            DivBkStatus.Visible = false;
            DivScanConfirm.Visible = false;
            if (update > 0 || delete > 0)
            {
                DivErrorMsg.Visible = true;
                LblErrorMsg.Text = "Scanned Data were Successfully  Updated.....";
                txt_access.Text = "";

            }
        }
        catch (Exception ex)
        {

        }

    }

    protected void BtnBkStatusNo_Click(object sender, EventArgs e)
    {
        DivBkStatus.Visible = false;
    }

    #endregion

    protected void btnComPrint_Click(object sender, EventArgs e)
    {
    }

    protected void BtnExit_Click(object sender, EventArgs e)
    {
        DivBookScanning.Visible = false;
    }

    #endregion

    #endregion

}