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

public partial class LibraryMod_ProceedingsMaster : System.Web.UI.Page
{
    #region initialization
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    DataTable dtCommon = new DataTable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DataSet ds = new DataSet();
    DAccess2 dacces2 = new DAccess2();
    static string SearchField = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds1 = new DataSet();
    DAccess2 da = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string accnoauto = string.Empty;
    static int searchby = 0;
    static string searchclgcode = string.Empty;
    static string searchlibcode = string.Empty;
    public void Connection()
    {
        con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
        con.Open();
    }


    string usercode = "", singleuser = "", group_user = "";
    string collegecode = "";
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

            collegecode = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!Page.IsPostBack)
            {
                Bindcollege();
                getLibPrivil();
                loadsearchby();
                //Fpload.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btn_excel.Visible = false;
                // loadlist();
                Tab1.CssClass = "Clicked";
                MainView.ActiveViewIndex = 0;
            }
        }
        catch (Exception ex) { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearch(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> values = new List<string>();
        if (searchby == 1)
        {

            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 Acc_No FROM ProceedingMaster where Acc_No Like '" + prefixText + "%' AND Lib_Code ='" + searchlibcode + "' AND College_Code='" + searchclgcode + "' order by Acc_No";
            else
                query = "SELECT DISTINCT  TOP  100 Acc_No FROM ProceedingMaster where Acc_No Like '" + prefixText + "%'  AND College_Code='" + searchclgcode + "' order by Acc_No";
        }
        else if (searchby == 2)
        {
            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 Title FROM ProceedingMaster where Title Like '" + prefixText + "%' AND Lib_Code ='" + searchlibcode + "' AND College_Code='" + searchclgcode + "' order by Title";
            else
                query = "SELECT DISTINCT  TOP  100 Title FROM ProceedingMaster where Title Like '" + prefixText + "%'  AND College_Code='" + searchclgcode + "' order by Title";
        }
        values = ws.Getname(query);
        return values;
    }

    #region loaddata

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
                searchclgcode = Convert.ToString(ddlCollege.SelectedValue);
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    public void loadlibrary(string LibCodeCollection)
    {
        try
        {
            string selectQuery = "Select lib_name,lib_code,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCodeCollection + " AND  college_code=" + ddlCollege.SelectedValue + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            ddllibrary.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddllibrary.DataSource = ds;
                ddllibrary.DataTextField = "lib_name";
                ddllibrary.DataValueField = "lib_code";
                ddllibrary.DataBind();
                ddllibrary.Items.Insert(0, "All");


                ddllib.DataSource = ds;
                ddllib.DataTextField = "lib_name";
                ddllib.DataValueField = "lib_code";
                ddllib.DataBind();



                searchlibcode = Convert.ToString(ddllibrary.SelectedValue);

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    public void loadsearchby()
    {
        try
        {
            ddlsearchby.Items.Add("All");
            ddlsearchby.Items.Add("Access No");
            ddlsearchby.Items.Add("Title");
            ddlsearchby.Items.Add("Department");
            ddlsearchby.Items.Add("Subject");
            ddlsearchby.Items.Add("Publisher");
            ddlsearchby.Items.Add("Supplier");
            ddlsearchby.Items.Add("Status");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    public void loadlist()
    {
        try
        {
            string selectQuery = "SELECT DISTINCT Title FROM ProceedingMaster WHERE Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            ddltitle.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltitle.DataSource = ds;
                ddltitle.DataTextField = "Title";
                ddltitle.DataValueField = "Title";
                ddltitle.DataBind();
            }
            ddltitle.Items.Insert(0, "");

            selectQuery = "SELECT Dept_Name FROM Journal_Dept WHERE Dept_Name <> '' AND Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code = " + ddlCollege.SelectedValue;
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            dlldepartment.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                dlldepartment.DataSource = ds;
                dlldepartment.DataTextField = "Dept_Name";
                dlldepartment.DataValueField = "Dept_Name";
                dlldepartment.DataBind();
            }
            dlldepartment.Items.Insert(0, "");

            selectQuery = "SELECT DISTINCT OtherTitle FROM ProceedingMaster WHERE Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            ddlothertitle.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlothertitle.DataSource = ds;
                ddlothertitle.DataTextField = "OtherTitle";
                ddlothertitle.DataValueField = "OtherTitle";
                ddlothertitle.DataBind();
            }
            ddlothertitle.Items.Insert(0, "");

            selectQuery = "SELECT DISTINCT Author FROM ProceedingMaster WHERE Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            ddlauthor.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlauthor.DataSource = ds;
                ddlauthor.DataTextField = "Author";
                ddlauthor.DataValueField = "Author";
                ddlauthor.DataBind();
            }
            ddlauthor.Items.Insert(0, "");

            selectQuery = "SELECT DISTINCT Subject FROM ProceedingMaster WHERE Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            ddlsubject.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsubject.DataSource = ds;
                ddlsubject.DataTextField = "Subject";
                ddlsubject.DataValueField = "Subject";
                ddlsubject.DataBind();
            }
            ddlsubject.Items.Insert(0, "");

            selectQuery = "SELECT DISTINCT Publisher FROM ProceedingMaster WHERE Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            ddlpublisher.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlpublisher.DataSource = ds;
                ddlpublisher.DataTextField = "Publisher";
                ddlpublisher.DataValueField = "Publisher";
                ddlpublisher.DataBind();
            }
            ddlpublisher.Items.Insert(0, "");

            selectQuery = "SELECT DISTINCT ISNULL(VendorCompName,'') as Supplier_Name FROM CO_VendorMaster S WHERE LibraryFlag='1' and ISNULL(VendorCompName,'') <> ''  ORDER BY Supplier_Name ";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            ddlsupplier.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsupplier.DataSource = ds;
                ddlsupplier.DataTextField = "Supplier_Name";
                ddlsupplier.DataValueField = "Supplier_Name";
                ddlsupplier.DataBind();
            }
            ddlsupplier.Items.Insert(0, "");

            selectQuery = "Select TextVal,textcode from textvaltable where college_code=" + ddlCollege.SelectedValue + "and TextCriteria='LBHed' ";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            ddlbudget.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbudget.DataSource = ds;
                ddlbudget.DataTextField = "TextVal";
                ddlbudget.DataValueField = "textcode";
                ddlbudget.DataBind();
            }
            ddlbudget.Items.Insert(0, "");

            selectQuery = "SELECT DISTINCT ConfType FROM ProceedingMaster WHERE Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            ddlconftype.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlconftype.DataSource = ds;
                ddlconftype.DataTextField = "ConfType";
                ddlconftype.DataValueField = "ConfType";
                ddlconftype.DataBind();
            }
            ddlconftype.Items.Insert(0, "");
            ddlstatus.Items.Clear();
            ddlstatus.Items.Add("Available");
            ddlstatus.Items.Add("Lost");
            ddlstatus.Items.Add("Binding");
            ddlstatus.Items.Add("condomn");
            ddlstatus.Items.Insert(0, "");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    #endregion

    #region Index Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getLibPrivil();
            GrdProceding.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btn_excel.Visible = false;
            lbprint.Visible = false;
            searchclgcode = Convert.ToString(ddlCollege.SelectedValue);

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GrdProceding.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btn_excel.Visible = false;
            lbprint.Visible = false;
            searchlibcode = Convert.ToString(ddllibrary.SelectedValue);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void ddlsearchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GrdProceding.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btn_excel.Visible = false;
            lbprint.Visible = false;
            if (ddlsearchby.Text == "All")
            {
                ddlsearchname.Visible = false;
                txtsearchvalue.Visible = false;
                txtsearchvalue.Text = "";
                SearchField = "";
            }
            else if (ddlsearchby.Text == "Access No")
            {
                ddlsearchname.Visible = false;
                txtsearchvalue.Visible = true;
                SearchField = "Acc_No";
                txtsearchvalue.Text = "";
                searchby = 1;
            }
            else if (ddlsearchby.Text == "Title")
            {
                ddlsearchname.Visible = false;
                txtsearchvalue.Visible = true;
                txtsearchvalue.Text = "";
                SearchField = "Title";
                searchby = 2;
            }
            else if (ddlsearchby.Text == "Department")
            {
                ddlsearchname.Visible = true;
                txtsearchvalue.Visible = false;
                txtsearchvalue.Text = "";
                SearchField = "Department";

                string selectQuery = "SELECT Dept_Name FROM Journal_Dept WHERE Dept_Name <> '' AND College_Code =" + ddlCollege.SelectedValue;
                if (ddllibrary.Text != "All")
                    selectQuery = selectQuery + " AND Lib_Code ='" + ddllibrary.SelectedValue + "'";
                ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
                ddlsearchname.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlsearchname.DataSource = ds;
                    ddlsearchname.DataTextField = "Dept_Name";
                    ddlsearchname.DataValueField = "Dept_Name";
                    ddlsearchname.DataBind();
                    ddlsearchname.Items.Insert(0, "");
                }

            }
            else if (ddlsearchby.Text == "Subject")
            {
                ddlsearchname.Visible = true;
                txtsearchvalue.Visible = false;
                txtsearchvalue.Text = "";
                SearchField = "Subject";

                string selectQuery = "SELECT DISTINCT Subject FROM ProceedingMaster WHERE College_Code =" + ddlCollege.SelectedValue;
                if (ddllibrary.Text != "All")
                    selectQuery = selectQuery + " AND Lib_Code ='" + ddllibrary.SelectedValue + "'";
                ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
                ddlsearchname.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlsearchname.DataSource = ds;
                    ddlsearchname.DataTextField = "Subject";
                    ddlsearchname.DataValueField = "Subject";
                    ddlsearchname.DataBind();
                    ddlsearchname.Items.Insert(0, "");
                }


            }
            else if (ddlsearchby.Text == "Publisher")
            {
                ddlsearchname.Visible = true;
                txtsearchvalue.Visible = false;
                txtsearchvalue.Text = "";
                SearchField = "Publisher";

                string selectQuery = "SELECT DISTINCT Publisher FROM ProceedingMaster WHERE College_Code =" + ddlCollege.SelectedValue;
                if (ddllibrary.Text != "All")
                    selectQuery = selectQuery + " AND Lib_Code ='" + ddllibrary.SelectedValue + "'";
                ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
                ddlsearchname.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlsearchname.DataSource = ds;
                    ddlsearchname.DataTextField = "Publisher";
                    ddlsearchname.DataValueField = "Publisher";
                    ddlsearchname.DataBind();
                    ddlsearchname.Items.Insert(0, "");
                }



            }
            else if (ddlsearchby.Text == "Supplier")
            {
                ddlsearchname.Visible = true;
                txtsearchvalue.Visible = false;
                txtsearchvalue.Text = "";
                SearchField = "m.Supplier_code";


                string selectQuery = "SELECT DISTINCT Supplier_Name FROM Supplier_Details WHERE College_Code =" + ddlCollege.SelectedValue;

                ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
                ddlsearchname.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlsearchname.DataSource = ds;
                    ddlsearchname.DataTextField = "Supplier_Name";
                    ddlsearchname.DataValueField = "Supplier_Name";
                    ddlsearchname.DataBind();
                    ddlsearchname.Items.Insert(0, "");
                }
            }
            else if (ddlsearchby.Text == "Status")
            {
                ddlsearchname.Visible = true;
                txtsearchvalue.Visible = false;
                txtsearchvalue.Text = "";
                SearchField = "Book_Status";
                ddlsearchname.Items.Clear();
                ddlsearchname.Items.Add("Available");
                ddlsearchname.Items.Add("Lost");
                ddlsearchname.Items.Add("Binding");
                ddlsearchname.Items.Add("condomn");
                ddlsearchname.Items.Insert(0, "");
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }


    }

    protected void cbfrom_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbfrom.Checked)
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
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void ddltitle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {
        }

    }

    protected void ddllib_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //string selectlibinfo = "SELECT ISNULL(ProcAutoNo,0) ProcAutoNo,ISNULL(Proc_Acr,'') Proc_Acr,ISNULL(Proc_StNo,1) Proc_StNo FROM Library Where Lib_Code ='" + ddllib.SelectedValue;

            //    ds = dacces2.select_method_wo_parameter(selectlibinfo, "Text");

            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        if (ds.Tables[0].Rows[0]["ProcAutoNo"].ToString() == "1")
            //        {
            //            txtaccessno.Enabled = false;
            //            string selectprocinfo = "SELECT * FROM ProceedingMaster WHERE Lib_Code ='" + ddllib.SelectedValue + "'ORDER BY LEN(Acc_No),Acc_No";

            //            ds1 = dacces2.select_method_wo_parameter(selectlibinfo, "Text");
            //            if (ds1.Tables[0].Rows.Count > 0)
            //            {
            //                string AccNo = ds1.Tables[0].Rows[0]["Acc_No"].ToString();
            //                char acc = Convert.ToChar(AccNo);
            //                if (acr != "" && stno != "")
            //                {
            //                    int code = Convert.ToInt32(stno);
            //                    accnoauto = acr + Convert.ToString(code);
            //                }
            //                else
            //                    accnoauto = "";
            //            }
            //            else
            //            {
            //                txtaccessno.Text = ds.Tables[0].Rows[0]["Proc_Acr"].ToString() + ds.Tables[0].Rows[0]["Proc_StNo"].ToString();
            //            }

            //        }
            //        else
            //        {
            //            txtaccessno.Enabled = true;
            //            txtaccessno.Text="";
            //        }
            //    }





            loadlist();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }

    }

    //protected void Fpload_OnButtonCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int actrow = Convert.ToInt32(Fpload.Sheets[0].ActiveRow);
    //        string accno = Fpload.Sheets[0].Cells[actrow, 2].Text;
    //        string libcode = Fpload.Sheets[0].Cells[actrow, 12].Text;
    //        string colcode = ddlCollege.SelectedValue;
    //        string libname = Fpload.Sheets[0].Cells[actrow, 11].Text;
    //        string editQ = "select * from ProceedingMaster where Acc_No='" + accno + "' and Lib_Code='" + libcode + "' and College_Code='" + colcode + "'";
    //        DataSet edit = new DataSet();
    //        edit = da.select_method_wo_parameter(editQ, "Text");
    //        if (edit.Tables[0].Rows.Count > 0)
    //        {
    //            loadlist();
    //            btnnew_Click(sender, e);
    //            Tab1.CssClass = "Clicked";
    //            MainView.ActiveViewIndex = 0;
    //            divTarvellerEntryDetails.Visible = true;
    //            Btnsave.ImageUrl = "~/LibImages/update (2).jpg";
    //            btndelete.Enabled = true;
    //            txtaccessno.Text = edit.Tables[0].Rows[0]["Acc_No"].ToString();
    //            txtbookaccno.Text = edit.Tables[0].Rows[0]["Book_AccNo"].ToString();
    //            txtreceiveddate.Text = Convert.ToDateTime(edit.Tables[0].Rows[0]["ReceivedDate"]).ToString("dd/MM/yyyy");
    //            txtconfname.Text = edit.Tables[0].Rows[0]["ConfName"].ToString();
    //            txteditors.Text = edit.Tables[0].Rows[0]["Editors"].ToString();
    //            txtogranizer.Text = edit.Tables[0].Rows[0]["Organizer"].ToString();
    //            txtcosponser.Text = edit.Tables[0].Rows[0]["CoSponser"].ToString();
    //            txtvenue.Text = edit.Tables[0].Rows[0]["Venue"].ToString();
    //            txtprocdate.Text = Convert.ToDateTime(edit.Tables[0].Rows[0]["ProcDate"]).ToString("dd/MM/yyyy");
    //            txtprice.Text = edit.Tables[0].Rows[0]["Price"].ToString();
    //            txtdiscount.Text = edit.Tables[0].Rows[0]["Discount"].ToString();
    //            txtnetprice.Text = edit.Tables[0].Rows[0]["NetPrice"].ToString();
    //            txtyear.Text = edit.Tables[0].Rows[0]["ProcYear"].ToString();
    //            txtisbn.Text = edit.Tables[0].Rows[0]["ISBN"].ToString();
    //            txtcollation.Text = edit.Tables[0].Rows[0]["Collation"].ToString();
    //            txtvolume.Text = edit.Tables[0].Rows[0]["Volume"].ToString();
    //            txtinvoice.Text = edit.Tables[0].Rows[0]["Invoice"].ToString();
    //            txtkeywords.Text = edit.Tables[0].Rows[0]["Keyword"].ToString();
    //            txtlocation.Text = edit.Tables[0].Rows[0]["Location"].ToString();
    //            txtabstract.Text = edit.Tables[0].Rows[0]["Abstract"].ToString();
    //            txtbudgetyear.Text = edit.Tables[0].Rows[0]["BudgetYear"].ToString();
    //            ddltitle.Text = edit.Tables[0].Rows[0]["Title"].ToString();
    //            ddlothertitle.Text = edit.Tables[0].Rows[0]["OtherTitle"].ToString();
    //            ddlauthor.Text = edit.Tables[0].Rows[0]["Author"].ToString();
    //            ddlconftype.Text = edit.Tables[0].Rows[0]["ConfType"].ToString();
    //            if (Convert.ToInt32(edit.Tables[0].Rows[0]["ProcType"]) == 1)
    //            {
    //                rblnotional.SelectedValue = "0";
    //            }
    //            else
    //            {
    //                rblnotional.SelectedValue = "1";
    //            }
    //            ddllib.Text = libcode;
    //            dlldepartment.Text = edit.Tables[0].Rows[0]["Department"].ToString();
    //            ddlsubject.Text = edit.Tables[0].Rows[0]["Subject"].ToString();
    //            ddlsupplier.Text = edit.Tables[0].Rows[0]["Supplier_Code"].ToString();
    //            ddlpublisher.Text = edit.Tables[0].Rows[0]["Publisher"].ToString();
    //            ddlbudget.Text = edit.Tables[0].Rows[0]["BudgetHead"].ToString();
    //            ddlstatus.Text = edit.Tables[0].Rows[0]["Book_Status"].ToString();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
    //    }
    //}

    protected void rblnotional_Selected(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {
        }
    }

    protected void txtprice_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtprice.Text.Trim() != "" && txtdiscount.Text.Trim() != "")
            {
                double discount = (Convert.ToDouble(txtprice.Text.Trim()) / 100 * (Convert.ToDouble(txtdiscount.Text.Trim())));
                txtnetprice.Text = Convert.ToString((Convert.ToDouble(txtprice.Text.Trim())) - discount);
            }
            else
            {
                txtnetprice.Text = Convert.ToString(txtprice.Text.Trim());
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void txtdiscount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtprice.Text.Trim() != "" && txtdiscount.Text.Trim() != "")
            {
                double discount = (Convert.ToDouble(txtprice.Text.Trim()) / 100 * (Convert.ToDouble(txtdiscount.Text.Trim())));
                txtnetprice.Text = Convert.ToString((Convert.ToDouble(txtprice.Text.Trim())) - discount);
            }
            else
            {
                txtnetprice.Text = Convert.ToString(txtprice.Text.Trim());
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnsubtitle_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddltitle.Items.Count > 0)
            {
                ddltitle.Items.Remove(ddltitle.Text);
                //string deleteqry = "delete TextValTable where TextCriteria='TRREM' and college_code='" + Convert.ToString(Session["collegecode"]) + "' and TextVal='" + ddltitle.SelectedItem.Text.Trim() + "'";
                //int res = dirAcc.deleteData(deleteqry);
                //if (res > 0)
                //{
                //    bindMethod();
                //}
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnsubothertitle_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlothertitle.Items.Count > 0)
            {
                ddlothertitle.Items.Remove(ddlothertitle.Text);

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnsubauthor_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlauthor.Items.Count > 0)
            {
                ddlauthor.Items.Remove(ddlauthor.Text);

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnsubconftype_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlconftype.Items.Count > 0)
            {
                ddlconftype.Items.Remove(ddlconftype.Text);

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnsubstatus_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlstatus.Items.Count > 0)
            {
                ddlstatus.Items.Remove(ddlstatus.Text);

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnsubdept_Click(object sender, EventArgs e)
    {
        try
        {
            if (dlldepartment.Items.Count > 0)
            {
                dlldepartment.Items.Remove(dlldepartment.Text);

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnsubsubject_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlsubject.Items.Count > 0)
            {
                ddlsubject.Items.Remove(ddlsubject.Text);

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnsubpublisher_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlpublisher.Items.Count > 0)
            {
                ddlpublisher.Items.Remove(ddlpublisher.Text);

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnaddtitle_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_header1.Text = "Title";
            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            //lblcriteria.Text = "Remark";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnaddothertitle_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_header1.Text = "Other Title";
            plusdiv.Visible = true;
            panel_addgroup.Visible = true;

            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnaddauthor_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_header1.Text = "Author";
            plusdiv.Visible = true;
            panel_addgroup.Visible = true;

            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnaddconftype_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_header1.Text = "Conf Type";
            plusdiv.Visible = true;
            panel_addgroup.Visible = true;

            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnaddstatus_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_header1.Text = "Status";
            plusdiv.Visible = true;
            panel_addgroup.Visible = true;

            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnadddept_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_header1.Text = "Department";
            plusdiv.Visible = true;
            panel_addgroup.Visible = true;

            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnaddsubject_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_header1.Text = "Subject";
            plusdiv.Visible = true;
            panel_addgroup.Visible = true;

            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnaddpublisher_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_header1.Text = "Publisher";
            plusdiv.Visible = true;
            panel_addgroup.Visible = true;

            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btn_addgroup_Click(object sender, EventArgs e)  //added by raghul dec 26 2017
    {
        try
        {
            string textvalue = txt_addgroup.Text;
            if (!string.IsNullOrEmpty(textvalue))
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                li.Text = textvalue.Trim();


                if (lbl_header1.Text == "Title")
                {
                    if (ddltitle.Items.Contains(li))
                    {
                        plusdiv.Visible = true;
                        lblerror.Visible = true;
                        lblerror.Text = "Name has been already entered";
                    }
                    else
                    {
                        ddltitle.Items.Add(textvalue.Trim());
                        ddltitle.Text = textvalue.Trim();
                        //ddltitle.Items.Insert(0, "");
                        plusdiv.Visible = false;
                        txt_addgroup.Text = string.Empty;
                    }
                }
                else if (lbl_header1.Text == "Other Title")
                {
                    if (ddlothertitle.Items.Contains(li))
                    {
                        plusdiv.Visible = true;
                        lblerror.Visible = true;
                        lblerror.Text = "Name has been already entered";
                    }
                    else
                    {
                        ddlothertitle.Items.Add(textvalue.Trim());
                        ddlothertitle.Text = textvalue.Trim();
                        //ddlothertitle.Items.Insert(0, "");
                        plusdiv.Visible = false;
                        txt_addgroup.Text = string.Empty;
                    }
                }
                else if (lbl_header1.Text == "Author")
                {
                    if (ddlauthor.Items.Contains(li))
                    {
                        plusdiv.Visible = true;
                        lblerror.Visible = true;
                        lblerror.Text = "Name has been already entered";
                    }
                    else
                    {
                        ddlauthor.Items.Add(textvalue.Trim());
                        ddlauthor.Text = textvalue.Trim();
                        //ddlauthor.Items.Insert(0, "");
                        plusdiv.Visible = false;
                        txt_addgroup.Text = string.Empty;
                    }
                }
                else if (lbl_header1.Text == "Conf Type")
                {
                    if (ddlconftype.Items.Contains(li))
                    {
                        plusdiv.Visible = true;
                        lblerror.Visible = true;
                        lblerror.Text = "Name has been already entered";
                    }
                    else
                    {
                        ddlconftype.Items.Add(textvalue.Trim());
                        ddlconftype.Text = textvalue.Trim();
                        //ddlconftype.Items.Insert(0, "");
                        plusdiv.Visible = false;
                        txt_addgroup.Text = string.Empty;
                    }
                }

                else if (lbl_header1.Text == "Status")
                {
                    if (ddlstatus.Items.Contains(li))
                    {
                        plusdiv.Visible = true;
                        lblerror.Visible = true;
                        lblerror.Text = "Name has been already entered";
                    }
                    else
                    {
                        ddlstatus.Items.Add(textvalue.Trim());
                        ddlstatus.Text = textvalue.Trim();
                        //ddlstatus.Items.Insert(0, "");
                        plusdiv.Visible = false;
                        txt_addgroup.Text = string.Empty;
                    }
                }

                else if (lbl_header1.Text == "Department")
                {
                    if (dlldepartment.Items.Contains(li))
                    {
                        plusdiv.Visible = true;
                        lblerror.Visible = true;
                        lblerror.Text = "Name has been already entered";
                    }
                    else
                    {
                        dlldepartment.Items.Add(textvalue.Trim());
                        dlldepartment.Text = textvalue.Trim();
                        //dlldepartment.Items.Insert(0, "");
                        plusdiv.Visible = false;
                        txt_addgroup.Text = string.Empty;
                    }
                }
                else if (lbl_header1.Text == "Subject")
                {
                    if (ddlsubject.Items.Contains(li))
                    {
                        plusdiv.Visible = true;
                        lblerror.Visible = true;
                        lblerror.Text = "Name has been already entered";
                    }
                    else
                    {
                        ddlsubject.Items.Add(textvalue.Trim());
                        ddlsubject.Text = textvalue.Trim();
                        //ddlsubject.Items.Insert(0, "");
                        plusdiv.Visible = false;
                        txt_addgroup.Text = string.Empty;
                    }
                }
                else if (lbl_header1.Text == "Publisher")
                {
                    if (ddlpublisher.Items.Contains(li))
                    {
                        plusdiv.Visible = true;
                        lblerror.Visible = true;
                        lblerror.Text = "Name has been already entered";
                    }
                    else
                    {
                        ddlpublisher.Items.Add(textvalue.Trim());
                        ddlpublisher.Text = textvalue.Trim();
                        //ddlpublisher.Items.Insert(0, "");
                        plusdiv.Visible = false;
                        txt_addgroup.Text = string.Empty;
                    }
                }


                //string inseertqry = "insert into TextValTable (TextVal,TextCriteria,college_code) values('" + textvalue.Trim() + "','TRREM','" + Convert.ToString(Session["collegecode"]) + "')";
                //int i = dirAcc.insertData(inseertqry);
                //if (i > 0)
                //{
                //    bindMethod();
                //}


            }
            else
            {
                lblerror.Text = "please enter you " + lbl_header1.Text;
                lblerror.Visible = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btn_exitaddgroup_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addgroup.Visible = false;
        txt_addgroup.Text = "";
    }

    #endregion

    #region button Go

    protected void btnMainGo_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = string.Empty;
            string fromdate1 = string.Empty;
            string todate1 = string.Empty;
            if (cbfrom.Checked)
            {
                string fromDate = txt_fromdate1.Text;
                string toDate = txt_todate1.Text;
                string[] fromdate = fromDate.Split('/');
                string[] todate = toDate.Split('/');
                if (fromdate.Length == 3)
                    fromdate1 = fromdate[2].ToString() + "-" + fromdate[1].ToString() + "-" + fromdate[0].ToString();
                if (todate.Length == 3)
                    todate1 = todate[2].ToString() + "-" + todate[1].ToString() + "-" + todate[0].ToString();
            }
            sql = " SELECT Acc_No,Title,Subject,Publisher, m.Supplier_code as Supplier_Name,ISBN,Collation,ProcYear,Book_Status,Lib_Name,M.Lib_Code ";
            sql = sql + "FROM ProceedingMaster M ";
            sql = sql + "INNER JOIN Library L ON L.Lib_Code = M.Lib_Code ";
            sql = sql + "LEFT JOIN Supplier_Details S ON S.Supplier_Code = M.Supplier_Code ";
            sql = sql + "WHERE 1 = 1 ";

            if (cbfrom.Checked == true)
                sql = sql + "AND ReceivedDate Between '" + fromdate1 + "' AND '" + todate1 + "' ";
            if (ddllibrary.Text.Trim() != "All")
                sql = sql + "AND M.Lib_Code ='" + ddllibrary.SelectedValue + "' ";
            if (ddlsearchby.Text != "All")
            {
                if (txtsearchvalue.Text.Trim() != "")
                    sql = sql + "AND " + SearchField + " Like '" + txtsearchvalue.Text + "%'";
                if (ddlsearchname.Text != "")
                    sql = sql + "AND " + SearchField + " Like '" + ddlsearchname.Text + "%'";
            }
            sql = sql + "Order By Len(Acc_No),Acc_No,M.Lib_Code";
            ds = da.select_method_wo_parameter(sql, "Text");

            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btn_excel.Visible = false;
            lbprint.Visible = false;

            if (!string.IsNullOrEmpty(ddlCollege.Text) && !string.IsNullOrEmpty(ddllibrary.Text))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtProMaster = new DataTable();
                    DataRow drow;
                    dtProMaster.Columns.Add("SNo", typeof(string));
                    dtProMaster.Columns.Add("Access No", typeof(string));
                    dtProMaster.Columns.Add("Title", typeof(string));
                    dtProMaster.Columns.Add("Subject", typeof(string));
                    dtProMaster.Columns.Add("Publisher", typeof(string));
                    dtProMaster.Columns.Add("Supplier", typeof(string));
                    dtProMaster.Columns.Add("ISBN", typeof(string));
                    dtProMaster.Columns.Add("Collation", typeof(string));
                    dtProMaster.Columns.Add("Year", typeof(string));
                    dtProMaster.Columns.Add("Status", typeof(string));
                    dtProMaster.Columns.Add("Library", typeof(string));
                    dtProMaster.Columns.Add("Library Code", typeof(string));


                    int sno = 0;
                    for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                    {
                        sno++;
                        drow = dtProMaster.NewRow();
                        drow["SNo"] = Convert.ToString(sno);
                        drow["Access No"] = ds.Tables[0].Rows[r]["Acc_No"].ToString();
                        drow["Title"] = ds.Tables[0].Rows[r]["Title"].ToString();
                        drow["Subject"] = ds.Tables[0].Rows[r]["Subject"].ToString();
                        drow["Publisher"] = ds.Tables[0].Rows[r]["Publisher"].ToString();
                        drow["Supplier"] = ds.Tables[0].Rows[r]["Supplier_Name"].ToString();
                        drow["ISBN"] = ds.Tables[0].Rows[r]["ISBN"].ToString();
                        drow["Collation"] = ds.Tables[0].Rows[r]["Collation"].ToString();
                        drow["Year"] = ds.Tables[0].Rows[r]["ProcYear"].ToString();
                        drow["Status"] = ds.Tables[0].Rows[r]["Book_Status"].ToString();
                        drow["Library"] = ds.Tables[0].Rows[r]["Lib_Name"].ToString();
                        drow["Library Code"] = ds.Tables[0].Rows[r]["Lib_Code"].ToString();
                        dtProMaster.Rows.Add(drow);
                    }
                    GrdProceding.DataSource = dtProMaster;
                    GrdProceding.DataBind();

                    GrdProceding.Visible = true;
                    btnprintmaster.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btn_excel.Visible = true;
                    lbprint.Visible = false;
                    lblerrmainapp.Visible = false;
                }
                else
                {
                    GrdProceding.Visible = false;
                    lblerrmainapp.Visible = true;
                    btnprintmaster.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btn_excel.Visible = false;
                    lbprint.Visible = false;
                    lblerrmainapp.Text = "No Record(s) Found";
                }
            }
            else
            {
                lblerrmainapp.Visible = true;
                GrdProceding.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btn_excel.Visible = false;
                lbprint.Visible = false;
                lblerrmainapp.Text = "Select All Field";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }


    }

    protected void GrdProceding_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void GrdProceding_onselectedindexchanged(object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowindex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        string accno = Convert.ToString(GrdProceding.Rows[rowindex].Cells[1].Text);
        string libcode = Convert.ToString(GrdProceding.Rows[rowindex].Cells[11].Text);
        string colcode = ddlCollege.SelectedValue;
        string libname = Convert.ToString(GrdProceding.Rows[rowindex].Cells[10].Text);
        string editQ = "select * from ProceedingMaster where Acc_No='" + accno + "' and Lib_Code='" + libcode + "' and College_Code='" + colcode + "'";
        DataSet edit = new DataSet();
        edit = da.select_method_wo_parameter(editQ, "Text");
        if (edit.Tables[0].Rows.Count > 0)
        {
            loadlist();
            btnnew_Click(sender, e);
            Tab1.CssClass = "Clicked";
            MainView.ActiveViewIndex = 0;
            divTarvellerEntryDetails.Visible = true;
            Btnsave.ImageUrl = "~/LibImages/update (2).jpg";
            btndelete.Enabled = true;
            txtaccessno.Text = edit.Tables[0].Rows[0]["Acc_No"].ToString();
            txtbookaccno.Text = edit.Tables[0].Rows[0]["Book_AccNo"].ToString();
            txtreceiveddate.Text = Convert.ToDateTime(edit.Tables[0].Rows[0]["ReceivedDate"]).ToString("dd/MM/yyyy");
            txtconfname.Text = edit.Tables[0].Rows[0]["ConfName"].ToString();
            txteditors.Text = edit.Tables[0].Rows[0]["Editors"].ToString();
            txtogranizer.Text = edit.Tables[0].Rows[0]["Organizer"].ToString();
            txtcosponser.Text = edit.Tables[0].Rows[0]["CoSponser"].ToString();
            txtvenue.Text = edit.Tables[0].Rows[0]["Venue"].ToString();
            txtprocdate.Text = Convert.ToDateTime(edit.Tables[0].Rows[0]["ProcDate"]).ToString("dd/MM/yyyy");
            txtprice.Text = edit.Tables[0].Rows[0]["Price"].ToString();
            txtdiscount.Text = edit.Tables[0].Rows[0]["Discount"].ToString();
            txtnetprice.Text = edit.Tables[0].Rows[0]["NetPrice"].ToString();
            txtyear.Text = edit.Tables[0].Rows[0]["ProcYear"].ToString();
            txtisbn.Text = edit.Tables[0].Rows[0]["ISBN"].ToString();
            txtcollation.Text = edit.Tables[0].Rows[0]["Collation"].ToString();
            txtvolume.Text = edit.Tables[0].Rows[0]["Volume"].ToString();
            txtinvoice.Text = edit.Tables[0].Rows[0]["Invoice"].ToString();
            txtkeywords.Text = edit.Tables[0].Rows[0]["Keyword"].ToString();
            txtlocation.Text = edit.Tables[0].Rows[0]["Location"].ToString();
            txtabstract.Text = edit.Tables[0].Rows[0]["Abstract"].ToString();
            txtbudgetyear.Text = edit.Tables[0].Rows[0]["BudgetYear"].ToString();
            ddltitle.Text = edit.Tables[0].Rows[0]["Title"].ToString();
            ddlothertitle.Text = edit.Tables[0].Rows[0]["OtherTitle"].ToString();
            ddlauthor.Text = edit.Tables[0].Rows[0]["Author"].ToString();
            ddlconftype.Text = edit.Tables[0].Rows[0]["ConfType"].ToString();
            if (Convert.ToInt32(edit.Tables[0].Rows[0]["ProcType"]) == 1)
            {
                rblnotional.SelectedValue = "0";
            }
            else
            {
                rblnotional.SelectedValue = "1";
            }
            ddllib.Text = libcode;
            dlldepartment.Text = edit.Tables[0].Rows[0]["Department"].ToString();
            ddlsubject.Text = edit.Tables[0].Rows[0]["Subject"].ToString();
            ddlsupplier.Text = edit.Tables[0].Rows[0]["Supplier_Code"].ToString();
            ddlpublisher.Text = edit.Tables[0].Rows[0]["Publisher"].ToString();
            ddlbudget.Text = edit.Tables[0].Rows[0]["BudgetHead"].ToString();
            ddlstatus.Text = edit.Tables[0].Rows[0]["Book_Status"].ToString();
        }

    }

    protected void GrdProceding_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[11].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[11].Visible = false;
        }
    }

    #endregion

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            loadlist();
            Tab1.CssClass = "Clicked";
            MainView.ActiveViewIndex = 0;
            btnnew_Click(sender, e);
            Auto_AccessNo();
            divTarvellerEntryDetails.Visible = true;
            btndelete.Enabled = false;
            Btnsave.ImageUrl = "~/LibImages/save.jpg";
            //ddllib_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }

    }

    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                lbprint.Visible = false;
                dacces2.printexcelreportgrid(GrdProceding, reportname);
            }
            else
            {
                txtexcelname.Focus();
                lbprint.Text = "Please Enter Your Report Name";
                lbprint.Visible = true;
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {

            lbprint.Visible = false;
            string degreedetails = "Proceedings Master";
            string pagename = "ProceedingsMaster.aspx";
            string ss = "";
            Printcontrolhed2.loadspreaddetails(GrdProceding, pagename, degreedetails, 0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btnnew_Click(object sender, EventArgs e)
    {
        try
        {
            Tab1.CssClass = "Clicked";
            MainView.ActiveViewIndex = 0;
            btndelete.Enabled = false;
            Btnsave.ImageUrl = "~/LibImages/save.jpg";
            txtaccessno.Text = txtbookaccno.Text = txtreceiveddate.Text = txtconfname.Text = txteditors.Text = txtogranizer.Text = txtcosponser.Text = txtvenue.Text = txtprocdate.Text = txtprice.Text = txtdiscount.Text = txtnetprice.Text = txtyear.Text = txtisbn.Text = txtcollation.Text = txtvolume.Text = txtinvoice.Text = txtkeywords.Text = txtlocation.Text = txtabstract.Text = txtbudgetyear.Text = "";
            rblnotional.SelectedIndex = 0;


            ddltitle.Text = "";
            ddlothertitle.Text = "";
            ddlauthor.Text = "";
            ddlconftype.Text = "";
            ddlstatus.Text = "";
            dlldepartment.Text = "";
            ddlsubject.Text = "";
            ddlpublisher.Text = "";
            ddlsupplier.Text = "";
            ddlbudget.Text = "";
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (dlldepartment.Text == "")
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please select Department.";
                return;
            }
            int save = 0;
            string sql = "";
            string receivedate = string.Empty;
            if (txtreceiveddate.Text != "")
            {
                string receivedate1 = txtreceiveddate.Text;
                string[] receivedate11 = receivedate1.Split('/');
                if (receivedate11.Length == 3)
                    receivedate = receivedate11[2].ToString() + "-" + receivedate11[1].ToString() + "-" + receivedate11[0].ToString();
            }
            string procdate = string.Empty;
            if (txtprocdate.Text != "")
            {
                string procdate1 = txtprocdate.Text;
                string[] receivedate11 = procdate1.Split('/');
                if (receivedate11.Length == 3)
                    procdate = receivedate11[2].ToString() + "-" + receivedate11[1].ToString() + "-" + receivedate11[0].ToString();
            }
            bool BlnProcType;
            if (rblnotional.SelectedValue == "1")
                BlnProcType = false;
            else
                BlnProcType = true;

            if (Btnsave.ImageUrl == "~/LibImages/save.jpg")
            {
                if (txtprice.Text != "" && txtdiscount.Text != "")
                {
                    sql = "INSERT INTO ProceedingMaster(Access_Date,Access_Time,Acc_No,Book_AccNo,ReceivedDate,Title,OtherTitle,Author,ConfName,Editors,Organizer,CoSponser,Venue,ProcDate,Price,Discount,NetPrice,ConfType,ProcType,Book_Status,ProcYear,ISBN,Collation,Volume,Department,Subject,Supplier_Code,Publisher,Invoice,Keyword,Location,Abstract,BudgetHead,BudgetYear,Lib_Code,College_Code)";
                    sql = sql + "VALUES('" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "'" + txtaccessno.Text + "','" + txtbookaccno.Text + "','" + receivedate + "','" + ddltitle.Text + "','" + ddlothertitle.Text + "','" + ddlauthor.Text + "','" + txtconfname.Text + "','" + txteditors.Text + "','" + txtogranizer.Text + "','" + txtcosponser.Text + "','" + txtvenue.Text + "','" + procdate + "',";
                    sql = sql + txtprice.Text + "," + txtdiscount.Text + "," + txtnetprice.Text + ",'" + ddlconftype.Text + "','" + BlnProcType + "','" + ddlstatus.Text + "','" + txtyear.Text + "','" + txtisbn.Text + "','" + txtcollation.Text + "','" + txtvolume.Text + "',";
                    sql = sql + "'" + dlldepartment.Text + "','" + ddlsubject.Text + "','" + ddlsupplier.Text + "','" + ddlpublisher.Text + "','" + txtinvoice.Text + "','" + txtkeywords.Text + "','" + txtlocation.Text + "','" + txtabstract.Text + "',";
                    sql = sql + "'" + ddlbudget.Text + "','" + txtbudgetyear.Text + "','" + ddllib.SelectedValue + "'," + ddlCollege.SelectedValue + ")";
                }
                else if (txtprice.Text == "" && txtdiscount.Text == "")
                {
                    sql = "INSERT INTO ProceedingMaster(Access_Date,Access_Time,Acc_No,Book_AccNo,ReceivedDate,Title,OtherTitle,Author,ConfName,Editors,Organizer,CoSponser,Venue,ProcDate,ConfType,ProcType,Book_Status,ProcYear,ISBN,Collation,Volume,Department,Subject,Supplier_Code,Publisher,Invoice,Keyword,Location,Abstract,BudgetHead,BudgetYear,Lib_Code,College_Code)";
                    sql = sql + "VALUES('" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "'" + txtaccessno.Text + "','" + txtbookaccno.Text + "','" + receivedate + "','" + ddltitle.Text + "','" + ddlothertitle.Text + "','" + ddlauthor.Text + "','" + txtconfname.Text + "','" + txteditors.Text + "','" + txtogranizer.Text + "','" + txtcosponser.Text + "','" + txtvenue.Text + "','" + procdate + "',";
                    sql = sql + "'" + ddlconftype.Text + "','" + BlnProcType + "','" + ddlstatus.Text + "','" + txtyear.Text + "','" + txtisbn.Text + "','" + txtcollation.Text + "','" + txtvolume.Text + "',";
                    sql = sql + "'" + dlldepartment.Text + "','" + ddlsubject.Text + "','" + ddlsupplier.Text + "','" + ddlpublisher.Text + "','" + txtinvoice.Text + "','" + txtkeywords.Text + "','" + txtlocation.Text + "','" + txtabstract.Text + "',";
                    sql = sql + "'" + ddlbudget.Text + "','" + txtbudgetyear.Text + "','" + ddllib.SelectedValue + "'," + ddlCollege.SelectedValue + ")";
                }
                else if (txtprice.Text != "" && txtdiscount.Text == "")
                {
                    sql = "INSERT INTO ProceedingMaster(Access_Date,Access_Time,Acc_No,Book_AccNo,ReceivedDate,Title,OtherTitle,Author,ConfName,Editors,Organizer,CoSponser,Venue,ProcDate,Price,NetPrice,ConfType,ProcType,Book_Status,ProcYear,ISBN,Collation,Volume,Department,Subject,Supplier_Code,Publisher,Invoice,Keyword,Location,Abstract,BudgetHead,BudgetYear,Lib_Code,College_Code)";
                    sql = sql + "VALUES('" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "'" + txtaccessno.Text + "','" + txtbookaccno.Text + "','" + receivedate + "','" + ddltitle.Text + "','" + ddlothertitle.Text + "','" + ddlauthor.Text + "','" + txtconfname.Text + "','" + txteditors.Text + "','" + txtogranizer.Text + "','" + txtcosponser.Text + "','" + txtvenue.Text + "','" + procdate + "',";
                    sql = sql + txtprice.Text + "," + txtnetprice.Text + ",'" + ddlconftype.Text + "','" + BlnProcType + "','" + ddlstatus.Text + "','" + txtyear.Text + "','" + txtisbn.Text + "','" + txtcollation.Text + "','" + txtvolume.Text + "',";
                    sql = sql + "'" + dlldepartment.Text + "','" + ddlsubject.Text + "','" + ddlsupplier.Text + "','" + ddlpublisher.Text + "','" + txtinvoice.Text + "','" + txtkeywords.Text + "','" + txtlocation.Text + "','" + txtabstract.Text + "',";
                    sql = sql + "'" + ddlbudget.Text + "','" + txtbudgetyear.Text + "','" + ddllib.SelectedValue + "'," + ddlCollege.SelectedValue + ")";
                }
                save = dacces2.update_method_wo_parameter(sql, "Text");
                if (save == 1)
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Saved Sucessfully";
                    btnMainGo_Click(sender, e);
                }
                sql = "SELECT * FROM Journal_Dept WHERE Dept_Name ='" + dlldepartment.Text + "' AND Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
                ds = da.select_method_wo_parameter(sql, "Text");
                if (ds.Tables[0].Rows.Count == 0)
                {
                    sql = "INSERT INTO Journal_Dept(Dept_Name,Dept_Acr,Lib_Code,College_Code) VALUES('" + dlldepartment.Text + "','','" + ddllib.SelectedValue + "'," + ddlCollege.SelectedValue + ")";
                    save = dacces2.update_method_wo_parameter(sql, "Text");
                }
            }
            else if (Btnsave.ImageUrl == "~/LibImages/update (2).jpg")
            {
                if (txtprice.Text != "" && txtdiscount.Text != "")
                {
                    sql = "UPDATE ProceedingMaster SET Access_Date='" + DateTime.Now.ToString("MM-dd-yyyy") + "',Access_Time='" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "ReceivedDate='" + receivedate + "',Title='" + ddltitle.Text + "',OtherTitle='" + ddlothertitle.Text + "',Author='" + ddlauthor.Text + "',ConfName='" + txtconfname.Text + "',Editors='" + txteditors.Text + "',Organizer='" + txtogranizer.Text + "',CoSponser='" + txtcosponser.Text + "',";
                    sql = sql + "Venue='" + txtvenue.Text + "',ProcDate='" + procdate + "',Price=" + txtprice.Text + ",Discount=" + txtdiscount.Text + ",NetPrice=" + txtnetprice.Text + ",ConfType='" + ddlconftype.Text + "',ProcType='" + BlnProcType + "',Book_Status='" + ddlstatus.Text + "',ProcYear='" + txtyear.Text + "',ISBN='" + txtisbn.Text + "',";
                    sql = sql + "Collation='" + txtcollation.Text + "',Volume='" + txtvolume.Text + "',Department='" + dlldepartment.Text + "',Subject='" + ddlsubject.Text + "',Supplier_Code='" + ddlsupplier.Text + "',Publisher='" + ddlpublisher.Text + "',Invoice='" + txtinvoice.Text + "',";
                    sql = sql + "Keyword='" + txtkeywords.Text + "',Location='" + txtlocation.Text + "',Abstract='" + txtabstract.Text + "',BudgetHead='" + ddlbudget.Text + "',BudgetYear='" + txtbudgetyear.Text + "' ";
                    sql = sql + "WHERE Acc_No ='" + txtaccessno.Text + "' AND Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue + "";
                }
                else if (txtprice.Text == "" && txtdiscount.Text == "")
                {
                    sql = "UPDATE ProceedingMaster SET Access_Date='" + DateTime.Now.ToString("MM-dd-yyyy") + "',Access_Time='" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "ReceivedDate='" + receivedate + "',Title='" + ddltitle.Text + "',OtherTitle='" + ddlothertitle.Text + "',Author='" + ddlauthor.Text + "',ConfName='" + txtconfname.Text + "',Editors='" + txteditors.Text + "',Organizer='" + txtogranizer.Text + "',CoSponser='" + txtcosponser.Text + "',";
                    sql = sql + "Venue='" + txtvenue.Text + "',ProcDate='" + procdate + "',ConfType='" + ddlconftype.Text + "',ProcType='" + BlnProcType + "',Book_Status='" + ddlstatus.Text + "',ProcYear='" + txtyear.Text + "',ISBN='" + txtisbn.Text + "',";
                    sql = sql + "Collation='" + txtcollation.Text + "',Volume='" + txtvolume.Text + "',Department='" + dlldepartment.Text + "',Subject='" + ddlsubject.Text + "',Supplier_Code='" + ddlsupplier.Text + "',Publisher='" + ddlpublisher.Text + "',Invoice='" + txtinvoice.Text + "',";
                    sql = sql + "Keyword='" + txtkeywords.Text + "',Location='" + txtlocation.Text + "',Abstract='" + txtabstract.Text + "',BudgetHead='" + ddlbudget.Text + "',BudgetYear='" + txtbudgetyear.Text + "' ";
                    sql = sql + "WHERE Acc_No ='" + txtaccessno.Text + "' AND Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue + "";

                }
                else if (txtprice.Text != "" && txtdiscount.Text == "")
                {
                    sql = "UPDATE ProceedingMaster SET Access_Date='" + DateTime.Now.ToString("MM-dd-yyyy") + "',Access_Time='" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "ReceivedDate='" + receivedate + "',Title='" + ddltitle.Text + "',OtherTitle='" + ddlothertitle.Text + "',Author='" + ddlauthor.Text + "',ConfName='" + txtconfname.Text + "',Editors='" + txteditors.Text + "',Organizer='" + txtogranizer.Text + "',CoSponser='" + txtcosponser.Text + "',";
                    sql = sql + "Venue='" + txtvenue.Text + "',ProcDate='" + procdate + "',Price=" + txtprice.Text + ",NetPrice=" + txtnetprice.Text + ",ConfType='" + ddlconftype.Text + "',ProcType='" + BlnProcType + "',Book_Status='" + ddlstatus.Text + "',ProcYear='" + txtyear.Text + "',ISBN='" + txtisbn.Text + "',";
                    sql = sql + "Collation='" + txtcollation.Text + "',Volume='" + txtvolume.Text + "',Department='" + dlldepartment.Text + "',Subject='" + ddlsubject.Text + "',Supplier_Code='" + ddlsupplier.Text + "',Publisher='" + ddlpublisher.Text + "',Invoice='" + txtinvoice.Text + "',";
                    sql = sql + "Keyword='" + txtkeywords.Text + "',Location='" + txtlocation.Text + "',Abstract='" + txtabstract.Text + "',BudgetHead='" + ddlbudget.Text + "',BudgetYear='" + txtbudgetyear.Text + "' ";
                    sql = sql + "WHERE Acc_No ='" + txtaccessno.Text + "' AND Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue + "";
                }
                save = dacces2.update_method_wo_parameter(sql, "Text");
                if (save == 1)
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Updated Sucessfully";
                    btnMainGo_Click(sender, e);
                }
                sql = "SELECT * FROM Journal_Dept WHERE Dept_Name ='" + dlldepartment.Text + "' AND Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue + "";
                ds = da.select_method_wo_parameter(sql, "Text");
                if (ds.Tables[0].Rows.Count == 0)
                {
                    sql = "INSERT INTO Journal_Dept(Dept_Name,Dept_Acr,Lib_Code,College_Code) VALUES('" + dlldepartment.Text + "','','" + ddllib.SelectedValue + "'," + ddlCollege.SelectedValue + ")";
                    save = dacces2.update_method_wo_parameter(sql, "Text");
                }
            }
            loadlist();
            btnnew_Click(sender, e);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            string deleteQ;
            deleteQ = "Delete from ProceedingMaster Where Acc_No ='" + txtaccessno.Text + "' AND Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
            int delete = dacces2.update_method_wo_parameter(deleteQ, "Text");
            if (delete == 1)
            {

                imgAlert.Visible = true;
                lbl_alert.Text = "delete Sucessfully";
                btnMainGo_Click(sender, e);
                loadlist();
                btnnew_Click(sender, e);
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "ProceedingsMaster");
        }
    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        divTarvellerEntryDetails.Visible = false;
    }

    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
    }

    protected void Tab1_Click(object sender, EventArgs e)
    {
        Tab1.CssClass = "Clicked";
        Tab2.CssClass = "Initial";

        MainView.ActiveViewIndex = 0;
    }

    protected void Tab2_Click(object sender, EventArgs e)
    {
        Tab1.CssClass = "Initial";
        Tab2.CssClass = "Clicked";

        MainView.ActiveViewIndex = 1;
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

            loadlibrary(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    public void Auto_AccessNo()
    {
        try
        {
            string codeno = "";
            string codeno1 = "";
            DataSet rs2 = new DataSet();
            DataSet rs3 = new DataSet();
            string nonlibcode = "";
            string sqlnonqry = "";

            nonlibcode = Convert.ToString(ddllib.SelectedValue);
            sqlnonqry = "SELECT ISNULL(ProcAutoNo,0) ProcAutoNo,ISNULL(Proc_Acr,'') Proc_Acr,ISNULL(Proc_StNo,1) Proc_StNo FROM Library Where Lib_Code ='" + nonlibcode + "'";
            rs2.Clear();
            rs2 = d2.select_method_wo_parameter(sqlnonqry, "Text");
            if (rs2.Tables[0].Rows.Count > 0)
            {

                string book = Convert.ToString(rs2.Tables[0].Rows[0]["ProcAutoNo"]);
                if (book.ToLower() == "true")
                {

                    string sql = "SELECT * FROM ProceedingMaster WHERE Lib_Code ='" + nonlibcode + "' ORDER BY LEN(acc_no),acc_no";
                    rs3.Clear();
                    rs3 = da.select_method_wo_parameter(sql, "text");
                    if (rs3.Tables[0].Rows.Count > 0)
                    {
                        codeno = Convert.ToString(rs3.Tables[0].Rows[rs3.Tables[0].Rows.Count - 1]["acc_no"]);
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
                        codeno1 = Convert.ToString(rs2.Tables[0].Rows[0]["Proc_Acr"]) + jj;
                        txtaccessno.Text = codeno1;
                        txtaccessno.Enabled = false;
                    }
                    else
                    {
                        codeno1 = Convert.ToString(rs2.Tables[0].Rows[0]["Proc_Acr"]) + Convert.ToString(rs2.Tables[0].Rows[0]["Proc_StNo"]);
                        txtaccessno.Text = codeno1;
                        txtaccessno.Enabled = false;
                    }
                }
                else
                {
                    txtaccessno.Text = "";
                    txtaccessno.Enabled = true;
                }
            }
            else
            {
                txtaccessno.Text = "";
                txtaccessno.Enabled = true;
            }

        }
        catch
        {

        }

    }

    protected void txtaccessno_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string accno = Convert.ToString(txtaccessno.Text);
            string libcode = Convert.ToString(ddllib.SelectedValue);
            string colcode = ddlCollege.SelectedValue;
            string editQ = "select * from ProceedingMaster where Acc_No='" + accno + "' and Lib_Code='" + libcode + "' and College_Code='" + colcode + "'";
            DataSet edit = new DataSet();
            edit = da.select_method_wo_parameter(editQ, "Text");
            if (edit.Tables[0].Rows.Count > 0)
            {
                loadlist();
                btnnew_Click(sender, e);
                Tab1.CssClass = "Clicked";
                MainView.ActiveViewIndex = 0;
                divTarvellerEntryDetails.Visible = true;
                Btnsave.ImageUrl = "~/LibImages/update (2).jpg";
                btndelete.Enabled = true;
                txtaccessno.Text = edit.Tables[0].Rows[0]["Acc_No"].ToString();
                txtbookaccno.Text = edit.Tables[0].Rows[0]["Book_AccNo"].ToString();
                txtreceiveddate.Text = Convert.ToDateTime(edit.Tables[0].Rows[0]["ReceivedDate"]).ToString("dd/MM/yyyy");
                txtconfname.Text = edit.Tables[0].Rows[0]["ConfName"].ToString();
                txteditors.Text = edit.Tables[0].Rows[0]["Editors"].ToString();
                txtogranizer.Text = edit.Tables[0].Rows[0]["Organizer"].ToString();
                txtcosponser.Text = edit.Tables[0].Rows[0]["CoSponser"].ToString();
                txtvenue.Text = edit.Tables[0].Rows[0]["Venue"].ToString();
                txtprocdate.Text = Convert.ToDateTime(edit.Tables[0].Rows[0]["ProcDate"]).ToString("dd/MM/yyyy");
                txtprice.Text = edit.Tables[0].Rows[0]["Price"].ToString();
                txtdiscount.Text = edit.Tables[0].Rows[0]["Discount"].ToString();
                txtnetprice.Text = edit.Tables[0].Rows[0]["NetPrice"].ToString();
                txtyear.Text = edit.Tables[0].Rows[0]["ProcYear"].ToString();
                txtisbn.Text = edit.Tables[0].Rows[0]["ISBN"].ToString();
                txtcollation.Text = edit.Tables[0].Rows[0]["Collation"].ToString();
                txtvolume.Text = edit.Tables[0].Rows[0]["Volume"].ToString();
                txtinvoice.Text = edit.Tables[0].Rows[0]["Invoice"].ToString();
                txtkeywords.Text = edit.Tables[0].Rows[0]["Keyword"].ToString();
                txtlocation.Text = edit.Tables[0].Rows[0]["Location"].ToString();
                txtabstract.Text = edit.Tables[0].Rows[0]["Abstract"].ToString();
                txtbudgetyear.Text = edit.Tables[0].Rows[0]["BudgetYear"].ToString();
                ddltitle.Text = edit.Tables[0].Rows[0]["Title"].ToString();
                ddlothertitle.Text = edit.Tables[0].Rows[0]["OtherTitle"].ToString();
                ddlauthor.Text = edit.Tables[0].Rows[0]["Author"].ToString();
                ddlconftype.Text = edit.Tables[0].Rows[0]["ConfType"].ToString();
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["ProcType"]) == 1)
                {
                    rblnotional.SelectedValue = "0";
                }
                else
                {
                    rblnotional.SelectedValue = "1";
                }
                ddllib.Text = libcode;
                dlldepartment.Text = edit.Tables[0].Rows[0]["Department"].ToString();
                ddlsubject.Text = edit.Tables[0].Rows[0]["Subject"].ToString();
                ddlsupplier.Text = edit.Tables[0].Rows[0]["Supplier_Code"].ToString();
                ddlpublisher.Text = edit.Tables[0].Rows[0]["Publisher"].ToString();
                ddlbudget.Text = edit.Tables[0].Rows[0]["BudgetHead"].ToString();
                ddlstatus.Text = edit.Tables[0].Rows[0]["Book_Status"].ToString();
            }

        }
        catch (Exception ex) { }
    }
}