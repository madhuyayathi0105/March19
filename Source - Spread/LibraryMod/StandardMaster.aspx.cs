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

public partial class LibraryMod_StandardMaster : System.Web.UI.Page
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
                GrdstandMas.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btn_excel.Visible = false;
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
                query = "SELECT DISTINCT  TOP  100 Acc_No FROM StandardMaster where Acc_No Like '" + prefixText + "%' AND Lib_Code ='" + searchlibcode + "' AND College_Code='" + searchclgcode + "' order by Acc_No";
            else
                query = "SELECT DISTINCT  TOP  100 Acc_No FROM StandardMaster where Acc_No Like '" + prefixText + "%' AND College_Code='" + searchclgcode + "' order by Acc_No";

        }
        else if (searchby == 2)
        {
            if (searchlibcode != "All")
                query = "SELECT DISTINCT  TOP  100 Title FROM StandardMaster where Title Like '" + prefixText + "%' AND Lib_Code ='" + searchlibcode + "' AND College_Code='" + searchclgcode + "' order by Title";
            else
                query = "SELECT DISTINCT  TOP  100 Title FROM StandardMaster where Title Like '" + prefixText + "%'  AND College_Code='" + searchclgcode + "' order by Title";
        }
        values = ws.Getname(query);
        return values;
    }

    #region Binding Methods

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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
        }
    }

    public void loadlibrary(string LibCollection)
    {
        try
        {
            string selectQuery = "Select lib_name,lib_code,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCollection + " AND college_code=" + ddlCollege.SelectedValue + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
        }
    }

    public void loadlist()
    {
        try
        {
            string selectQuery = "SELECT DISTINCT Title FROM StandardMaster WHERE Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
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

            selectQuery = "SELECT Dept_Name FROM Journal_Dept WHERE Dept_Name <> '' AND Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
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

            selectQuery = "SELECT DISTINCT OtherTitle FROM StandardMaster WHERE Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
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
            selectQuery = "SELECT DISTINCT Subject FROM StandardMaster WHERE Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
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

            selectQuery = "SELECT DISTINCT Publisher FROM StandardMaster WHERE Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
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

            selectQuery = "SELECT DISTINCT Country FROM StandardMaster WHERE Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            ddlcounty.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcounty.DataSource = ds;
                ddlcounty.DataTextField = "Country";
                ddlcounty.DataValueField = "Country";
                ddlcounty.DataBind();


            }
            ddlcounty.Items.Insert(0, "");

            selectQuery = "SELECT DISTINCT CurrencyType FROM StandardMaster WHERE Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            ddlcurrency.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcurrency.DataSource = ds;
                ddlcurrency.DataTextField = "CurrencyType";
                ddlcurrency.DataValueField = "CurrencyType";
                ddlcurrency.DataBind();


            }
            ddlcurrency.Items.Insert(0, "");

            selectQuery = "Select TextVal,textcode from textvaltable where college_code=" + ddlCollege.SelectedValue + "and TextCriteria='LBHed' ";
            ds = dacces2.select_method_wo_parameter(selectQuery, "Text");
            ddlcurrency.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcurrency.DataSource = ds;
                ddlcurrency.DataTextField = "TextVal";
                ddlcurrency.DataValueField = "textcode";
                ddlcurrency.DataBind();


            }

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
            ddlcurrency.Items.Insert(0, "");
            ddlstatus.Items.Clear();
            ddlstatus.Items.Add("Available");
            ddlstatus.Items.Add("Lost");
            ddlstatus.Items.Add("Binding");
            ddlstatus.Items.Add("condomn");
            ddlstatus.Items.Insert(0, "");
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
        }
    }


    #endregion

    #region Index Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getLibPrivil();
            GrdstandMas.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btn_excel.Visible = false;
            lbprint.Visible = false;

            searchclgcode = Convert.ToString(ddlCollege.SelectedValue);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
        }
    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            GrdstandMas.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btn_excel.Visible = false;
            lbprint.Visible = false;


            searchlibcode = Convert.ToString(ddllibrary.SelectedValue);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
        }


    }

    protected void ddlsearchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GrdstandMas.Visible = false;
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

                string selectQuery = "SELECT DISTINCT Subject FROM StandardMaster WHERE College_Code =" + ddlCollege.SelectedValue;
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

                string selectQuery = "SELECT DISTINCT Publisher FROM StandardMaster WHERE College_Code =" + ddlCollege.SelectedValue;
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
                SearchField = "m.Supplier_Name";


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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
        }
    }

    protected void btnsubcountry_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlcounty.Items.Count > 0)
            {
                ddlcounty.Items.Remove(ddlcounty.Text);

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
        }
    }

    protected void btnsubcurrency_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlcurrency.Items.Count > 0)
            {
                ddlcurrency.Items.Remove(ddlcurrency.Text);

            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
        }
    }

    protected void btnaddcountry_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_header1.Text = "Country";
            plusdiv.Visible = true;
            panel_addgroup.Visible = true;

            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
        }
    }

    protected void btnaddcurrency_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_header1.Text = "Currency";
            plusdiv.Visible = true;
            panel_addgroup.Visible = true;

            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
                        plusdiv.Visible = false;
                        txt_addgroup.Text = string.Empty;
                        //ddltitle.Items.Insert(0, "");
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
                        plusdiv.Visible = false;
                        txt_addgroup.Text = string.Empty;
                        //ddlothertitle.Items.Insert(0, "");
                    }
                }
                else if (lbl_header1.Text == "Country")
                {
                    if (ddlcounty.Items.Contains(li))
                    {
                        plusdiv.Visible = true;
                        lblerror.Visible = true;
                        lblerror.Text = "Name has been already entered";
                    }
                    else
                    {
                        ddlcounty.Items.Add(textvalue.Trim());
                        ddlcounty.Text = textvalue.Trim();
                        plusdiv.Visible = false;
                        txt_addgroup.Text = string.Empty;
                        //ddlcounty.Items.Insert(0, "");
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
                        plusdiv.Visible = false;
                        txt_addgroup.Text = string.Empty;
                        //ddlstatus.Items.Insert(0, "");
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
                        plusdiv.Visible = false;
                        txt_addgroup.Text = string.Empty;
                        //dlldepartment.Items.Insert(0, "");
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
                        plusdiv.Visible = false;
                        txt_addgroup.Text = string.Empty;
                        //ddlsubject.Items.Insert(0, "");
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
                        plusdiv.Visible = false;
                        txt_addgroup.Text = string.Empty;
                        //ddlpublisher.Items.Insert(0, "");
                    }
                }
                else if (lbl_header1.Text == "Currency")
                {
                    if (ddlcurrency.Items.Contains(li))
                    {
                        plusdiv.Visible = true;
                        lblerror.Visible = true;
                        lblerror.Text = "Name has been already entered";
                    }
                    else
                    {
                        ddlcurrency.Items.Add(textvalue.Trim());
                        ddlcurrency.Text = textvalue.Trim();
                        plusdiv.Visible = false;
                        txt_addgroup.Text = string.Empty;
                        //ddlcurrency.Items.Insert(0, "");
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
            sql = " SELECT Acc_No,Title,Subject,Publisher,m.Supplier_code as Supplier_Name,CallNo,Pages,Pur_Year,Book_Status,Lib_Name,M.Lib_Code ";
            sql = sql + "FROM StandardMaster M ";
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

            if (!string.IsNullOrEmpty(ddlCollege.Text) && !string.IsNullOrEmpty(ddllibrary.Text))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtStandMaster = new DataTable();
                    DataRow drow;
                    dtStandMaster.Columns.Add("SNo", typeof(string));
                    dtStandMaster.Columns.Add("Access No", typeof(string));
                    dtStandMaster.Columns.Add("Title", typeof(string));
                    dtStandMaster.Columns.Add("Subject", typeof(string));
                    dtStandMaster.Columns.Add("Publisher", typeof(string));
                    dtStandMaster.Columns.Add("Supplier", typeof(string));
                    dtStandMaster.Columns.Add("Call No", typeof(string));
                    dtStandMaster.Columns.Add("Pages", typeof(string));
                    dtStandMaster.Columns.Add("Year", typeof(string));
                    dtStandMaster.Columns.Add("Status", typeof(string));
                    dtStandMaster.Columns.Add("Library", typeof(string));
                    dtStandMaster.Columns.Add("Library Code", typeof(string));


                    int sno = 0;
                    for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                    {
                        sno++;
                        drow = dtStandMaster.NewRow();
                        drow["SNo"] = Convert.ToString(sno);
                        drow["Access No"] = ds.Tables[0].Rows[r]["Acc_No"].ToString();
                        drow["Title"] = ds.Tables[0].Rows[r]["Title"].ToString();
                        drow["Subject"] = ds.Tables[0].Rows[r]["Subject"].ToString();
                        drow["Publisher"] = ds.Tables[0].Rows[r]["Publisher"].ToString();
                        drow["Supplier"] = ds.Tables[0].Rows[r]["Supplier_Name"].ToString();
                        drow["Call No"] = ds.Tables[0].Rows[r]["CallNo"].ToString();
                        drow["Pages"] = ds.Tables[0].Rows[r]["Pages"].ToString();
                        drow["Year"] = ds.Tables[0].Rows[r]["Pur_Year"].ToString();
                        drow["Status"] = ds.Tables[0].Rows[r]["Book_Status"].ToString();
                        drow["Library"] = ds.Tables[0].Rows[r]["Lib_Name"].ToString();
                        drow["Library Code"] = ds.Tables[0].Rows[r]["Lib_Code"].ToString();
                        dtStandMaster.Rows.Add(drow);
                    }
                    GrdstandMas.DataSource = dtStandMaster;
                    GrdstandMas.DataBind();

                    GrdstandMas.Visible = true;
                    btnprintmaster.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btn_excel.Visible = true;
                    lbprint.Visible = false;
                    lblerrmainapp.Visible = false;
                }
                else
                {
                    GrdstandMas.Visible = false;
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
                GrdstandMas.Visible = false;
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
        }
    }



    protected void GrdstandMas_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowindex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            loadlist();
            btnnew_Click(sender, e);
            string accno = Convert.ToString(GrdstandMas.Rows[rowindex].Cells[1].Text);
            string libcode = Convert.ToString(GrdstandMas.Rows[rowindex].Cells[11].Text);
            string colcode = ddlCollege.SelectedValue;
            string editQ = "";
            editQ = " SELECT * ";
            editQ = editQ + "FROM StandardMaster M ";
            editQ = editQ + "INNER JOIN Library L ON L.Lib_Code = M.Lib_Code ";
            editQ = editQ + "LEFT JOIN Supplier_Details S ON S.Supplier_Code = M.Supplier_Code ";
            editQ = editQ + "WHERE 1 = 1 ";
            editQ = editQ + "AND Acc_No ='" + accno + "' AND M.Lib_Code ='" + libcode + "' AND M.College_Code =" + colcode;
            DataSet edit = new DataSet();
            edit = da.select_method_wo_parameter(editQ, "Text");

            if (edit.Tables[0].Rows.Count > 0)
            {
                divTarvellerEntryDetails.Visible = true;
                Btnsave.ImageUrl = "~/LibImages/update (2).jpg";
                btndelete.Enabled = true;
                txtaccessno.Text = edit.Tables[0].Rows[0]["Acc_No"].ToString();
                txtedition.Text = edit.Tables[0].Rows[0]["Edition"].ToString();
                txtkeywords.Text = edit.Tables[0].Rows[0]["Keyword"].ToString();
                txtlocation.Text = edit.Tables[0].Rows[0]["Location"].ToString();
                txtremarks.Text = edit.Tables[0].Rows[0]["Remarks"].ToString();
                txtyear.Text = edit.Tables[0].Rows[0]["Pur_Year"].ToString();
                txtcallno.Text = edit.Tables[0].Rows[0]["CallNo"].ToString();
                txtpages.Text = edit.Tables[0].Rows[0]["pages"].ToString();
                txtcost.Text = edit.Tables[0].Rows[0]["Cost"].ToString();
                txtprice.Text = edit.Tables[0].Rows[0]["Price"].ToString();
                txtdiscount.Text = edit.Tables[0].Rows[0]["Discount"].ToString();
                txtnetprice.Text = edit.Tables[0].Rows[0]["NetPrice"].ToString();
                txtreceiveddate.Text = Convert.ToDateTime(edit.Tables[0].Rows[0]["ReceivedDate"]).ToString("dd/MM/yyyy");
                txtinvoice.Text = edit.Tables[0].Rows[0]["Invoice"].ToString();
                txtbudgetyear.Text = edit.Tables[0].Rows[0]["BudgetYear"].ToString();
                txtcodeno.Text = edit.Tables[0].Rows[0]["Book_AccNo"].ToString();
                ddltitle.Text = edit.Tables[0].Rows[0]["Title"].ToString();
                ddlothertitle.Text = edit.Tables[0].Rows[0]["OtherTitle"].ToString();
                ddlcounty.Text = edit.Tables[0].Rows[0]["Country"].ToString();
                dlldepartment.Text = edit.Tables[0].Rows[0]["Department"].ToString();
                ddlsubject.Text = edit.Tables[0].Rows[0]["Subject"].ToString();
                ddlsupplier.Text = edit.Tables[0].Rows[0]["Supplier_Code"].ToString();
                ddlpublisher.Text = edit.Tables[0].Rows[0]["Publisher"].ToString();
                ddlbudget.Text = edit.Tables[0].Rows[0]["BudgetHead"].ToString();
                ddllib.Text = libcode;
                ddlstatus.Text = edit.Tables[0].Rows[0]["Book_Status"].ToString();
                ddlcurrency.Text = edit.Tables[0].Rows[0]["CurrencyType"].ToString();
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
        }
    }

    protected void GrdstandMas_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void GrdstandMas_OnRowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void btnnew_Click(object sender, EventArgs e)
    {
        try
        {
            //loadlist();

            btndelete.Enabled = false;
            Btnsave.ImageUrl = "~/LibImages/save.jpg";

            txtaccessno.Text = txtcodeno.Text = txtreceiveddate.Text = txtpages.Text = txtcallno.Text = txtcost.Text = txtprice.Text = txtdiscount.Text = txtnetprice.Text = txtyear.Text = txtinvoice.Text = txtkeywords.Text = txtlocation.Text = txtremarks.Text = txtedition.Text = txtbudgetyear.Text = "";
            ddltitle.Text = "";
            ddlothertitle.Text = "";
            ddlcurrency.Text = "";
            ddlcounty.Text = "";
            ddlstatus.Text = "";
            dlldepartment.Text = "";
            ddlsubject.Text = "";
            ddlpublisher.Text = "";
            ddlsupplier.Text = "";
            ddlbudget.Text = "";

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
        }

    }

    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {
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
            if (Btnsave.ImageUrl == "~/LibImages/save.jpg")
            {
                if (txtprice.Text != "" && txtdiscount.Text != "" && txtcost.Text != "")
                {
                    sql = "INSERT INTO StandardMaster(Access_Date,Access_Time,Acc_No,Book_AccNo,Title,OtherTitle,Department,Subject,Publisher,Supplier_Code,Edition,Pur_Year,CallNo,Pages,CurrencyType,Cost,Price,Discount,NetPrice,ReceivedDate,Country,Invoice,Keyword,Location,Remarks,BudgetHead,BudgetYear,Book_Status,Lib_Code,College_Code)";
                    sql = sql + "VALUES('" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "'" + txtaccessno.Text + "','" + txtcodeno.Text + "','" + ddltitle.Text + "','" + ddlothertitle.Text + "','" + dlldepartment.Text + "','" + ddlsubject.Text + "','" + ddlpublisher.Text + "','" + ddlsupplier.Text + "','" + txtedition.Text + "','" + txtyear.Text + "',";
                    sql = sql + "'" + txtcallno.Text + "','" + txtpages.Text + "','" + ddlcurrency.Text + "'," + txtcost.Text + "," + txtprice.Text + "," + txtdiscount.Text + "," + txtnetprice.Text + ",'" + receivedate + "','" + ddlcounty.Text + "','" + txtinvoice.Text + "','" + txtkeywords.Text + "','" + txtlocation.Text + "','" + txtremarks.Text + "','" + ddlbudget.Text + "','" + txtbudgetyear.Text + "','" + ddlstatus.Text + "','" + ddllib.SelectedValue + "'," + ddlCollege.SelectedValue + ")";
                }
                else if (txtprice.Text == "" && txtdiscount.Text == "" && txtcost.Text == "")
                {
                    sql = "INSERT INTO StandardMaster(Access_Date,Access_Time,Acc_No,Book_AccNo,Title,OtherTitle,Department,Subject,Publisher,Supplier_Code,Edition,Pur_Year,CallNo,Pages,CurrencyType,ReceivedDate,Country,Invoice,Keyword,Location,Remarks,BudgetHead,BudgetYear,Book_Status,Lib_Code,College_Code)";
                    sql = sql + "VALUES('" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "'" + txtaccessno.Text + "','" + txtcodeno.Text + "','" + ddltitle.Text + "','" + ddlothertitle.Text + "','" + dlldepartment.Text + "','" + ddlsubject.Text + "','" + ddlpublisher.Text + "','" + ddlsupplier.Text + "','" + txtedition.Text + "','" + txtyear.Text + "',";
                    sql = sql + "'" + txtcallno.Text + "','" + txtpages.Text + "','" + ddlcurrency.Text + "','" + receivedate + "','" + ddlcounty.Text + "','" + txtinvoice.Text + "','" + txtkeywords.Text + "','" + txtlocation.Text + "','" + txtremarks.Text + "','" + ddlbudget.Text + "','" + txtbudgetyear.Text + "','" + ddlstatus.Text + "','" + ddllib.SelectedValue + "'," + ddlCollege.SelectedValue + ")";

                }
                else if (txtprice.Text == "" && txtdiscount.Text == "" && txtcost.Text != "")
                {
                    sql = "INSERT INTO StandardMaster(Access_Date,Access_Time,Acc_No,Book_AccNo,Title,OtherTitle,Department,Subject,Publisher,Supplier_Code,Edition,Pur_Year,CallNo,Pages,CurrencyType,Cost,ReceivedDate,Country,Invoice,Keyword,Location,Remarks,BudgetHead,BudgetYear,Book_Status,Lib_Code,College_Code)";
                    sql = sql + "VALUES('" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "'" + txtaccessno.Text + "','" + txtcodeno.Text + "','" + ddltitle.Text + "','" + ddlothertitle.Text + "','" + dlldepartment.Text + "','" + ddlsubject.Text + "','" + ddlpublisher.Text + "','" + ddlsupplier.Text + "','" + txtedition.Text + "','" + txtyear.Text + "',";
                    sql = sql + "'" + txtcallno.Text + "','" + txtpages.Text + "','" + ddlcurrency.Text + "'," + txtcost.Text + ",'" + receivedate + "','" + ddlcounty.Text + "','" + txtinvoice.Text + "','" + txtkeywords.Text + "','" + txtlocation.Text + "','" + txtremarks.Text + "','" + ddlbudget.Text + "','" + txtbudgetyear.Text + "','" + ddlstatus.Text + "','" + ddllib.SelectedValue + "'," + ddlCollege.SelectedValue + ")";

                }
                else if (txtprice.Text != "" && txtdiscount.Text == "" && txtcost.Text != "")
                {
                    sql = "INSERT INTO StandardMaster(Access_Date,Access_Time,Acc_No,Book_AccNo,Title,OtherTitle,Department,Subject,Publisher,Supplier_Code,Edition,Pur_Year,CallNo,Pages,CurrencyType,Cost,Price,NetPrice,ReceivedDate,Country,Invoice,Keyword,Location,Remarks,BudgetHead,BudgetYear,Book_Status,Lib_Code,College_Code)";
                    sql = sql + "VALUES('" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "'" + txtaccessno.Text + "','" + txtcodeno.Text + "','" + ddltitle.Text + "','" + ddlothertitle.Text + "','" + dlldepartment.Text + "','" + ddlsubject.Text + "','" + ddlpublisher.Text + "','" + ddlsupplier.Text + "','" + txtedition.Text + "','" + txtyear.Text + "',";
                    sql = sql + "'" + txtcallno.Text + "','" + txtpages.Text + "','" + ddlcurrency.Text + "'," + txtcost.Text + "," + txtprice.Text + "," + txtnetprice.Text + ",'" + receivedate + "','" + ddlcounty.Text + "','" + txtinvoice.Text + "','" + txtkeywords.Text + "','" + txtlocation.Text + "','" + txtremarks.Text + "','" + ddlbudget.Text + "','" + txtbudgetyear.Text + "','" + ddlstatus.Text + "','" + ddllib.SelectedValue + "'," + ddlCollege.SelectedValue + ")";

                }

                else if (txtprice.Text != "" && txtdiscount.Text == "" && txtcost.Text == "")
                {
                    sql = "INSERT INTO StandardMaster(Access_Date,Access_Time,Acc_No,Book_AccNo,Title,OtherTitle,Department,Subject,Publisher,Supplier_Code,Edition,Pur_Year,CallNo,Pages,CurrencyType,Price,NetPrice,ReceivedDate,Country,Invoice,Keyword,Location,Remarks,BudgetHead,BudgetYear,Book_Status,Lib_Code,College_Code)";
                    sql = sql + "VALUES('" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "'" + txtaccessno.Text + "','" + txtcodeno.Text + "','" + ddltitle.Text + "','" + ddlothertitle.Text + "','" + dlldepartment.Text + "','" + ddlsubject.Text + "','" + ddlpublisher.Text + "','" + ddlsupplier.Text + "','" + txtedition.Text + "','" + txtyear.Text + "',";
                    sql = sql + "'" + txtcallno.Text + "','" + txtpages.Text + "','" + ddlcurrency.Text + "'," + txtprice.Text + "," + txtnetprice.Text + ",'" + receivedate + "','" + ddlcounty.Text + "','" + txtinvoice.Text + "','" + txtkeywords.Text + "','" + txtlocation.Text + "','" + txtremarks.Text + "','" + ddlbudget.Text + "','" + txtbudgetyear.Text + "','" + ddlstatus.Text + "','" + ddllib.SelectedValue + "'," + ddlCollege.SelectedValue + ")";

                }
                else if (txtprice.Text != "" && txtdiscount.Text != "" && txtcost.Text == "")
                {
                    sql = "INSERT INTO StandardMaster(Access_Date,Access_Time,Acc_No,Book_AccNo,Title,OtherTitle,Department,Subject,Publisher,Supplier_Code,Edition,Pur_Year,CallNo,Pages,CurrencyType,Price,Discount,NetPrice,ReceivedDate,Country,Invoice,Keyword,Location,Remarks,BudgetHead,BudgetYear,Book_Status,Lib_Code,College_Code)";
                    sql = sql + "VALUES('" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "'" + txtaccessno.Text + "','" + txtcodeno.Text + "','" + ddltitle.Text + "','" + ddlothertitle.Text + "','" + dlldepartment.Text + "','" + ddlsubject.Text + "','" + ddlpublisher.Text + "','" + ddlsupplier.Text + "','" + txtedition.Text + "','" + txtyear.Text + "',";
                    sql = sql + "'" + txtcallno.Text + "','" + txtpages.Text + "','" + ddlcurrency.Text + "'," + txtprice.Text + "," + txtdiscount.Text + "," + txtnetprice.Text + ",'" + receivedate + "','" + ddlcounty.Text + "','" + txtinvoice.Text + "','" + txtkeywords.Text + "','" + txtlocation.Text + "','" + txtremarks.Text + "','" + ddlbudget.Text + "','" + txtbudgetyear.Text + "','" + ddlstatus.Text + "','" + ddllib.SelectedValue + "'," + ddlCollege.SelectedValue + ")";

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
                if (txtprice.Text != "" && txtdiscount.Text != "" && txtcost.Text != "")
                {

                    sql = "UPDATE StandardMaster SET Access_Date='" + DateTime.Now.ToString("MM-dd-yyyy") + "',Access_Time='" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "Title='" + ddltitle.Text + "',OtherTitle='" + ddlothertitle.Text + "',Subject='" + ddlsubject.Text + "',Publisher='" + ddlpublisher.Text + "',Supplier_Code='" + ddlsupplier.Text + "',Edition='" + txtedition.Text + "',";
                    sql = sql + "Department ='" + dlldepartment.Text + "',Keyword ='" + txtkeywords.Text + "',Location='" + txtlocation.Text + "',Remarks ='" + txtremarks.Text + "',";
                    sql = sql + "Pur_Year='" + txtyear.Text + "',CallNo='" + txtcallno.Text + "',pages='" + txtpages.Text + "',CurrencyType='" + ddlcurrency.Text + "',Cost=" + txtcost.Text + ",Price=" + txtprice.Text + ",Discount=" + txtdiscount.Text + ",NetPrice=" + txtnetprice.Text + ",";
                    sql = sql + "ReceivedDate='" + receivedate + "',Country='" + ddlcounty.Text + "',Invoice='" + txtinvoice.Text + "',BudgetHead ='" + ddlbudget.Text + "',BudgetYear ='" + txtbudgetyear.Text + "',Book_Status='" + ddlstatus.Text + "' ";
                    sql = sql + "WHERE Acc_No ='" + txtaccessno.Text + "' AND Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
                }
                else if (txtprice.Text == "" && txtdiscount.Text == "" && txtcost.Text == "")
                {
                    sql = "UPDATE StandardMaster SET Access_Date='" + DateTime.Now.ToString("MM-dd-yyyy") + "',Access_Time='" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "Title='" + ddltitle.Text + "',OtherTitle='" + ddlothertitle.Text + "',Subject='" + ddlsubject.Text + "',Publisher='" + ddlpublisher.Text + "',Supplier_Code='" + ddlsupplier.Text + "',Edition='" + txtedition.Text + "',";
                    sql = sql + "Department ='" + dlldepartment.Text + "',Keyword ='" + txtkeywords.Text + "',Location='" + txtlocation.Text + "',Remarks ='" + txtremarks.Text + "',";
                    sql = sql + "Pur_Year='" + txtyear.Text + "',CallNo='" + txtcallno.Text + "',pages='" + txtpages.Text + "',CurrencyType='" + ddlcurrency.Text + "',";
                    sql = sql + "ReceivedDate='" + receivedate + "',Country='" + ddlcounty.Text + "',Invoice='" + txtinvoice.Text + "',BudgetHead ='" + ddlbudget.Text + "',BudgetYear ='" + txtbudgetyear.Text + "',Book_Status='" + ddlstatus.Text + "' ";
                    sql = sql + "WHERE Acc_No ='" + txtaccessno.Text + "' AND Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;

                }
                else if (txtprice.Text == "" && txtdiscount.Text == "" && txtcost.Text != "")
                {
                    sql = "UPDATE StandardMaster SET Access_Date='" + DateTime.Now.ToString("MM-dd-yyyy") + "',Access_Time='" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "Title='" + ddltitle.Text + "',OtherTitle='" + ddlothertitle.Text + "',Subject='" + ddlsubject.Text + "',Publisher='" + ddlpublisher.Text + "',Supplier_Code='" + ddlsupplier.Text + "',Edition='" + txtedition.Text + "',";
                    sql = sql + "Department ='" + dlldepartment.Text + "',Keyword ='" + txtkeywords.Text + "',Location='" + txtlocation.Text + "',Remarks ='" + txtremarks.Text + "',";
                    sql = sql + "Pur_Year='" + txtyear.Text + "',CallNo='" + txtcallno.Text + "',pages='" + txtpages.Text + "',CurrencyType='" + ddlcurrency.Text + "',Cost=" + txtcost.Text + ",";
                    sql = sql + "ReceivedDate='" + receivedate + "',Country='" + ddlcounty.Text + "',Invoice='" + txtinvoice.Text + "',BudgetHead ='" + ddlbudget.Text + "',BudgetYear ='" + txtbudgetyear.Text + "',Book_Status='" + ddlstatus.Text + "' ";
                    sql = sql + "WHERE Acc_No ='" + txtaccessno.Text + "' AND Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;

                }
                else if (txtprice.Text != "" && txtdiscount.Text == "" && txtcost.Text != "")
                {
                    sql = "UPDATE StandardMaster SET Access_Date='" + DateTime.Now.ToString("MM-dd-yyyy") + "',Access_Time='" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "Title='" + ddltitle.Text + "',OtherTitle='" + ddlothertitle.Text + "',Subject='" + ddlsubject.Text + "',Publisher='" + ddlpublisher.Text + "',Supplier_Code='" + ddlsupplier.Text + "',Edition='" + txtedition.Text + "',";
                    sql = sql + "Department ='" + dlldepartment.Text + "',Keyword ='" + txtkeywords.Text + "',Location='" + txtlocation.Text + "',Remarks ='" + txtremarks.Text + "',";
                    sql = sql + "Pur_Year='" + txtyear.Text + "',CallNo='" + txtcallno.Text + "',pages='" + txtpages.Text + "',CurrencyType='" + ddlcurrency.Text + "',Cost=" + txtcost.Text + ",Price=" + txtprice.Text + ",NetPrice=" + txtnetprice.Text + ",";
                    sql = sql + "ReceivedDate='" + receivedate + "',Country='" + ddlcounty.Text + "',Invoice='" + txtinvoice.Text + "',BudgetHead ='" + ddlbudget.Text + "',BudgetYear ='" + txtbudgetyear.Text + "',Book_Status='" + ddlstatus.Text + "' ";
                    sql = sql + "WHERE Acc_No ='" + txtaccessno.Text + "' AND Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;

                }

                else if (txtprice.Text != "" && txtdiscount.Text == "" && txtcost.Text == "")
                {
                    sql = "UPDATE StandardMaster SET Access_Date='" + DateTime.Now.ToString("MM-dd-yyyy") + "',Access_Time='" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "Title='" + ddltitle.Text + "',OtherTitle='" + ddlothertitle.Text + "',Subject='" + ddlsubject.Text + "',Publisher='" + ddlpublisher.Text + "',Supplier_Code='" + ddlsupplier.Text + "',Edition='" + txtedition.Text + "',";
                    sql = sql + "Department ='" + dlldepartment.Text + "',Keyword ='" + txtkeywords.Text + "',Location='" + txtlocation.Text + "',Remarks ='" + txtremarks.Text + "',";
                    sql = sql + "Pur_Year='" + txtyear.Text + "',CallNo='" + txtcallno.Text + "',pages='" + txtpages.Text + "',CurrencyType='" + ddlcurrency.Text + "',Price=" + txtprice.Text + ",NetPrice=" + txtnetprice.Text + ",";
                    sql = sql + "ReceivedDate='" + receivedate + "',Country='" + ddlcounty.Text + "',Invoice='" + txtinvoice.Text + "',BudgetHead ='" + ddlbudget.Text + "',BudgetYear ='" + txtbudgetyear.Text + "',Book_Status='" + ddlstatus.Text + "' ";
                    sql = sql + "WHERE Acc_No ='" + txtaccessno.Text + "' AND Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;

                }
                else if (txtprice.Text != "" && txtdiscount.Text != "" && txtcost.Text == "")
                {
                    sql = "UPDATE StandardMaster SET Access_Date='" + DateTime.Now.ToString("MM-dd-yyyy") + "',Access_Time='" + DateTime.Now.ToString("hh:ss tt") + "',";
                    sql = sql + "Title='" + ddltitle.Text + "',OtherTitle='" + ddlothertitle.Text + "',Subject='" + ddlsubject.Text + "',Publisher='" + ddlpublisher.Text + "',Supplier_Code='" + ddlsupplier.Text + "',Edition='" + txtedition.Text + "',";
                    sql = sql + "Department ='" + dlldepartment.Text + "',Keyword ='" + txtkeywords.Text + "',Location='" + txtlocation.Text + "',Remarks ='" + txtremarks.Text + "',";
                    sql = sql + "Pur_Year='" + txtyear.Text + "',CallNo='" + txtcallno.Text + "',pages='" + txtpages.Text + "',CurrencyType='" + ddlcurrency.Text + "',Price=" + txtprice.Text + ",Discount=" + txtdiscount.Text + ",NetPrice=" + txtnetprice.Text + ",";
                    sql = sql + "ReceivedDate='" + receivedate + "',Country='" + ddlcounty.Text + "',Invoice='" + txtinvoice.Text + "',BudgetHead ='" + ddlbudget.Text + "',BudgetYear ='" + txtbudgetyear.Text + "',Book_Status='" + ddlstatus.Text + "' ";
                    sql = sql + "WHERE Acc_No ='" + txtaccessno.Text + "' AND Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;

                }
                save = dacces2.update_method_wo_parameter(sql, "Text");
                if (save == 1)
                {


                    imgAlert.Visible = true;
                    lbl_alert.Text = "Updated Sucessfully";
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
            loadlist();
            btnnew_Click(sender, e);
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
        }
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {

            string deleteQ;
            deleteQ = "Delete from StandardMaster Where Acc_No ='" + txtaccessno.Text + "' AND Lib_Code ='" + ddllib.SelectedValue + "' AND College_Code =" + ddlCollege.SelectedValue;
            int delete = dacces2.update_method_wo_parameter(deleteQ, "Text");
            if (delete == 1)
            {

                imgAlert.Visible = true;
                lbl_alert.Text = "delete Sucessfully";
                btnMainGo_Click(sender, e);

            }
            loadlist();
            btnnew_Click(sender, e);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            loadlist();
            btnnew_Click(sender, e);
            Auto_AccessNo();
            divTarvellerEntryDetails.Visible = true;
            btndelete.Enabled = false;
            Btnsave.ImageUrl = "~/LibImages/save.jpg";
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
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
                dacces2.printexcelreportgrid(GrdstandMas, reportname);
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
            d2.sendErrorMail(ex, userCollegeCode, "StandardMaster");
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        lbprint.Visible = false;
        string degreedetails = "Standart Master";

        string pagename = "StandardMaster.aspx";
        //Session["column_header_row_count"] = Fpload.ColumnHeader.RowCount;

        Printcontrolhed2.loadspreaddetails(GrdstandMas, pagename, degreedetails);
        Printcontrolhed2.Visible = true;

    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

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
            sqlnonqry = "SELECT ISNULL(StdAutoNo,0) StdAutoNo,ISNULL(Std_Acr,'') Std_Acr,ISNULL(Std_StNo,1) Std_StNo FROM Library Where Lib_Code ='" + nonlibcode + "'";
            rs2.Clear();
            rs2 = d2.select_method_wo_parameter(sqlnonqry, "Text");
            if (rs2.Tables[0].Rows.Count > 0)
            {

                string book = Convert.ToString(rs2.Tables[0].Rows[0]["StdAutoNo"]);
                if (book.ToLower() == "true")
                {

                    string sql = "SELECT * FROM StandardMaster WHERE Lib_Code ='" + nonlibcode + "' ORDER BY LEN(acc_no),acc_no";
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
                        codeno1 = Convert.ToString(rs2.Tables[0].Rows[0]["Std_Acr"]) + jj;
                        txtaccessno.Text = codeno1;
                        txtaccessno.Enabled = false;
                    }
                    else
                    {
                        codeno1 = Convert.ToString(rs2.Tables[0].Rows[0]["Std_Acr"]) + Convert.ToString(rs2.Tables[0].Rows[0]["Std_StNo"]);
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
            string libcode = Convert.ToString(ddllib.Text);
            string colcode = ddlCollege.SelectedValue;
            string editQ = "";
            editQ = " SELECT * ";
            editQ = editQ + "FROM StandardMaster M ";
            editQ = editQ + "INNER JOIN Library L ON L.Lib_Code = M.Lib_Code ";
            editQ = editQ + "LEFT JOIN Supplier_Details S ON S.Supplier_Code = M.Supplier_Code ";
            editQ = editQ + "WHERE 1 = 1 ";
            editQ = editQ + "AND Acc_No ='" + accno + "' AND M.Lib_Code ='" + libcode + "' AND M.College_Code =" + colcode;
            DataSet edit = new DataSet();
            edit = da.select_method_wo_parameter(editQ, "Text");

            if (edit.Tables[0].Rows.Count > 0)
            {
                divTarvellerEntryDetails.Visible = true;
                Btnsave.ImageUrl = "~/LibImages/update (2).jpg";
                btndelete.Enabled = true;
                txtaccessno.Text = edit.Tables[0].Rows[0]["Acc_No"].ToString();
                txtedition.Text = edit.Tables[0].Rows[0]["Edition"].ToString();
                txtkeywords.Text = edit.Tables[0].Rows[0]["Keyword"].ToString();
                txtlocation.Text = edit.Tables[0].Rows[0]["Location"].ToString();
                txtremarks.Text = edit.Tables[0].Rows[0]["Remarks"].ToString();
                txtyear.Text = edit.Tables[0].Rows[0]["Pur_Year"].ToString();
                txtcallno.Text = edit.Tables[0].Rows[0]["CallNo"].ToString();
                txtpages.Text = edit.Tables[0].Rows[0]["pages"].ToString();
                txtcost.Text = edit.Tables[0].Rows[0]["Cost"].ToString();
                txtprice.Text = edit.Tables[0].Rows[0]["Price"].ToString();
                txtdiscount.Text = edit.Tables[0].Rows[0]["Discount"].ToString();
                txtnetprice.Text = edit.Tables[0].Rows[0]["NetPrice"].ToString();
                txtreceiveddate.Text = Convert.ToDateTime(edit.Tables[0].Rows[0]["ReceivedDate"]).ToString("dd/MM/yyyy");
                txtinvoice.Text = edit.Tables[0].Rows[0]["Invoice"].ToString();
                txtbudgetyear.Text = edit.Tables[0].Rows[0]["BudgetYear"].ToString();
                txtcodeno.Text = edit.Tables[0].Rows[0]["Book_AccNo"].ToString();
                ddltitle.Text = edit.Tables[0].Rows[0]["Title"].ToString();
                ddlothertitle.Text = edit.Tables[0].Rows[0]["OtherTitle"].ToString();
                ddlcounty.Text = edit.Tables[0].Rows[0]["Country"].ToString();
                dlldepartment.Text = edit.Tables[0].Rows[0]["Department"].ToString();
                ddlsubject.Text = edit.Tables[0].Rows[0]["Subject"].ToString();
                ddlsupplier.Text = edit.Tables[0].Rows[0]["Supplier_Code"].ToString();
                ddlpublisher.Text = edit.Tables[0].Rows[0]["Publisher"].ToString();
                ddlbudget.Text = edit.Tables[0].Rows[0]["BudgetHead"].ToString();
                ddllib.Text = libcode;
                ddlstatus.Text = edit.Tables[0].Rows[0]["Book_Status"].ToString();
                ddlcurrency.Text = edit.Tables[0].Rows[0]["CurrencyType"].ToString();
            }
        }
        catch (Exception ex) { }
    }
}