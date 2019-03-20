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
using Gios.Pdf;
using System.IO;
using System.Diagnostics;
using System.Text;

public partial class LibraryMod_BarcodeGeneration : System.Web.UI.Page
{
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string collegecode = string.Empty;
    string[] strDataVl = new string[15];
    //StringBuilder strDataVal = new StringBuilder();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataTable dtCommon = new DataTable();
    DataSet dsprint = new DataSet();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    Dictionary<string, string> dictitle = new Dictionary<string, string>();
    SolidBrush whiteBrush = new SolidBrush(Color.White);
    DataTable barcode = new DataTable();
    DataRow drbar;
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
                chk_datewise_OnCheckedChanged(sender, e);
                getLibPrivil();
                Department();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                divspread.Visible = false;
                loadRackNo();
                loadShelfNo();
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); 
        }
    }

    #region College

    public void Bindcollege()
    {
        try
        {
            ddl_library.Items.Clear();
            dtCommon.Clear();
            ddl_collegename.Enabled = false;
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
                ddl_collegename.DataSource = dtCommon;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
                ddl_collegename.SelectedIndex = 0;
                ddl_collegename.Enabled = true;
            }
        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport");
        }
    }

    #endregion

    #region Library

    public void Library(string LibCollection)
    {
        try
        {
            ddl_library.Items.Clear();
            ds.Clear();
            string College = ddl_collegename.SelectedValue.ToString();
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                string lib = "select lib_name,lib_code,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCollection + " AND college_code=" + College + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = da.select_method_wo_parameter(lib, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_library.DataSource = ds;
                    ddl_library.DataTextField = "lib_name";
                    ddl_library.DataValueField = "lib_code";
                    ddl_library.DataBind();
                    ddl_library.Items.Insert(0, "All");
                }
            }
        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport");
        }


    }

    public void getLibPrivil()
    {
        try
        {
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddl_collegename.SelectedValue);
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
            string College = ddl_collegename.SelectedValue.ToString();
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
        catch (Exception ex)
        { //d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); 
        }
    }

    protected void cb_department_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            d2.CallCheckboxChange(cb_department, cbl_department, txt_department, "Department", "--Select--");
        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); 
        }
    }

    protected void cbl_department_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            d2.CallCheckboxListChange(cb_department, cbl_department, txt_department, "Department", "--Select--");


        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, userCollegeCode, "TitleWiseReport"); 
        }
    }

    #endregion

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
        catch { }
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
        catch { }
    }

    #endregion

    public void loadRackNo()
    {
        try
        {
            ddl_RackNo.Items.Clear();
            ds.Clear();
            string Query = " SELECT DISTINCT rack_no FROM Rack_Allocation order by rack_no";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_RackNo.DataSource = ds;
                ddl_RackNo.DataTextField = "rack_no";
                ddl_RackNo.DataValueField = "rack_no";
                ddl_RackNo.DataBind();
                ddl_RackNo.Items.Insert(0, "All");
            }
        }
        catch
        { }
    }

    public void loadShelfNo()
    {
        try
        {
            ddl_ShelfNo.Items.Clear();
            ds.Clear();
            string Query = " select DISTINCT Row_No  from Rack_Allocation order by row_no";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_ShelfNo.DataSource = ds;
                ddl_ShelfNo.DataTextField = "Row_No";
                ddl_ShelfNo.DataValueField = "Row_No";
                ddl_ShelfNo.DataBind();
                ddl_ShelfNo.Items.Insert(0, "All");
            }
        }
        catch
        { }
    }

    public void Booksearch()
    {
        try
        {
            if (ddl_Search.SelectedItem.Text == "Status")
            {
                ddlStatus.Items.Clear();
                ds.Clear();
                string Query = " select DISTINCT book_status  from bookdetails order by book_status";
                ds = d2.select_method_wo_parameter(Query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlStatus.DataSource = ds;
                    ddlStatus.DataTextField = "book_status";
                    ddlStatus.DataValueField = "book_status";
                    ddlStatus.DataBind();
                }
            }
            if (ddl_Search.SelectedItem.Text == "Category")
            {
                ddlStatus.Items.Clear();
                ds.Clear();
                string Query = " select DISTINCT category  from bookdetails order by category";
                ds = d2.select_method_wo_parameter(Query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlStatus.DataSource = ds;
                    ddlStatus.DataTextField = "category";
                    ddlStatus.DataValueField = "category";
                    ddlStatus.DataBind();
                }
            }
        }
        catch
        { }
    }

    protected void chk_datewise_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chk_datewise.Checked == true)
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

    protected void ddl_Search_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_Search.SelectedItem.Text == "All")
        {
            td_searchby.Visible = false;
            td_Chbtw.Visible = false;
            //td_txtfrom.Visible = false;
            td_ChTo.Visible = false;
            td_txtTo.Visible = false;
            td_status.Visible = false;
        }
        if (ddl_Search.SelectedItem.Text == "Access No")
        {
            td_searchby.Visible = true;
            td_Chbtw.Visible = true;
            //td_txtfrom.Visible = true;
            td_ChTo.Visible = false;
            td_txtTo.Visible = false;
            td_status.Visible = false;

            txt_from.Attributes.Add("placeholder", "Access No");

        }
        if (ddl_Search.SelectedItem.Text == "Call No")
        {
            td_searchby.Visible = true;
            //td_txtfrom.Visible = true;
            td_Chbtw.Visible = false;
            td_ChTo.Visible = false;
            td_txtTo.Visible = false;
            td_status.Visible = false;
            txt_from.Attributes.Add("placeholder", "Call No");
        }
        if (ddl_Search.SelectedItem.Text == "Title")
        {
            td_searchby.Visible = true;
            //td_txtfrom.Visible = true;
            td_Chbtw.Visible = false;
            td_ChTo.Visible = false;
            td_txtTo.Visible = false;
            td_status.Visible = false;
            txt_from.Attributes.Add("placeholder", "Title");

        }
        if (ddl_Search.SelectedItem.Text == "Author")
        {
            td_searchby.Visible = true;
            // td_txtfrom.Visible = true;
            td_Chbtw.Visible = false;
            td_ChTo.Visible = false;
            td_txtTo.Visible = false;
            td_status.Visible = false;
            txt_from.Attributes.Add("placeholder", "Author");

        }
        if (ddl_Search.SelectedItem.Text == "Status")
        {
            td_searchby.Visible = false;
            td_status.Visible = true;
            Booksearch();
            td_Chbtw.Visible = false;
            td_ChTo.Visible = false;
            td_txtTo.Visible = false;
            //td_txtfrom.Visible = false;
        }
        if (ddl_Search.SelectedItem.Text == "Subject")
        {
            td_searchby.Visible = true;
            //td_txtfrom.Visible = true;
            td_Chbtw.Visible = false;
            td_ChTo.Visible = false;
            td_txtTo.Visible = false;
            td_status.Visible = false;
            txt_from.Attributes.Add("placeholder", "Subject");

        }
        if (ddl_Search.SelectedItem.Text == "Bill No")
        {
            td_searchby.Visible = true;
            //td_txtfrom.Visible = true;
            td_Chbtw.Visible = false;
            td_ChTo.Visible = false;
            td_txtTo.Visible = false;
            td_status.Visible = false;
            txt_from.Attributes.Add("placeholder", "Bill No");

        }
        if (ddl_Search.SelectedItem.Text == "Category")
        {
            td_searchby.Visible = false;
            td_status.Visible = true;
            Booksearch();
            td_Chbtw.Visible = false;
            td_ChTo.Visible = false;
            td_txtTo.Visible = false;
            //td_txtfrom.Visible = false;
        }
        if (ddl_Search.SelectedItem.Text == "Purchased")
        {
            td_searchby.Visible = false;
        }
    }

    protected void chk_between_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chk_between.Checked == true)
        {
            td_ChTo.Visible = true;
            td_txtTo.Visible = true;
        }
    }

    protected void btnsearch_Click(object sender, EventArgs e)
    {
        ds.Clear();
        ds = dsvalue();

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            loadspreadvalues();
        }
        else
        {
            divspread.Visible = false;
            //print.Visible = false;
            //lblvalidation1.Text = "";
            imgdiv2.Visible = true;
            lbl_alert.Text = "No Record Found";
        }

    }

    protected DataSet dsvalue()
    {
        DataSet dsload = new DataSet();
        string Library = string.Empty;
        Library = Convert.ToString(ddl_library.SelectedValue);
        string dept = getCblSelectedValue(cbl_department);
        collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
        string rackNo = Convert.ToString(ddl_RackNo.SelectedValue);
        string shelfNo = Convert.ToString(ddl_ShelfNo.SelectedValue);
        string libCode = string.Empty;
        try
        {
            string selectQry = string.Empty;
            selectQry = " select Acc_No,Title,Author,isnull(Edition,'0') as Edition,isnull(Price,'0') as Price,isnull(Attachment,'0') as Attachment,isnull(Call_Des,'0') as Call_Des,isnull(Call_No,'0') as Call_No,Book_Status, Publisher,Dept_Code,Subject,isnull(Pur_Don,'0') as Pur_Don,Bill_No,isnull(typeofbook,'0') as typeofbook,isnull(CopyNo,'0') as CopyNo,Date_Accession,category,Acc_No as barcode from BookDetails bd,library lb where bd.Lib_Code = lb.Lib_Code and lb.College_Code ='" + collegecode + "' ";

            if (rackNo != "All")
            {
                if (Library != "All")
                {
                    libCode = " and Lib_Code ='" + Library + "' ";
                }
                selectQry = selectQry + " and bd.Acc_No in (SELECT Acc_No FROM Rack_Allocation WHERE Rack_No ='" + rackNo + "'  " + libCode + ")";

            }
            if (shelfNo != "All")
            {
                if (Library != "All")
                {
                    libCode = " and Lib_Code ='" + Library + "' ";
                }
                selectQry = selectQry + " and bd.Acc_No in (SELECT Acc_No FROM Rack_Allocation WHERE Row_No ='" + shelfNo + "' " + libCode + ")";
            }
            if (Library != "All" && Library != "")
            {
                selectQry = selectQry + " and lb.lib_code='" + Library + "'";
            }
            if (dept != "")
            {
                selectQry = selectQry + " and bd.Dept_Code in('" + dept + "')";
            }
            if (chk_datewise.Checked == true)
            {
                selectQry = selectQry + "  AND  bd.Date_Accession between '" + txt_fromdate.Text + "' and '" + txt_todate.Text + "'";
            }
            if (ddl_Search.SelectedItem.Text == "Access No" && chk_between.Checked == true)
            {
                selectQry = selectQry + "AND Acc_No between '" + txt_from.Text + "' and '" + txt_To.Text + "' ";
            }
            if (ddl_Search.SelectedItem.Text == "Access No" && chk_between.Checked == false)
            {
                selectQry = selectQry + "AND Acc_No='" + txt_from.Text + "'";
            }
            if (ddl_Search.SelectedItem.Text == "Call No")
            {
                selectQry = selectQry + "and Call_No='" + txt_from.Text + "'";
            }
            if (ddl_Search.SelectedItem.Text == "Title")
            {
                selectQry = selectQry + "and Title='" + txt_from.Text + "'";
            }
            if (ddl_Search.SelectedItem.Text == "Author")
            {
                selectQry = selectQry + "and Author='" + txt_from.Text + "'";
            }
            if (ddl_Search.SelectedItem.Text == "Status")
            {
                selectQry = selectQry + "and Status='" + ddlStatus.SelectedItem.Text + "'";
            }
            if (ddl_Search.SelectedItem.Text == "Subject")
            {
                selectQry = selectQry + "and Subject='" + txt_from.Text + "'";
            }
            if (ddl_Search.SelectedItem.Text == "Bill No")
            {
                selectQry = selectQry + "and Bill No='" + txt_from.Text + "'";
            }
            if (ddl_Search.SelectedItem.Text == "Purchased")
            {
                selectQry = selectQry + "and Pur_Don='Purchased'";
            }
            if (ddl_Search.SelectedItem.Text == "Category")
            {
                selectQry = selectQry + "and Category='" + ddlStatus.SelectedItem.Text + "'";
            }


            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQry, "Text");

        }
        catch (Exception ex)
        {
        }


        return ds;
    }

    protected void loadspreadvalues()
    {
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region Spread Design
               
                barcode.Columns.Add("AccNo", typeof(string));
                barcode.Columns.Add("Title", typeof(string));
                barcode.Columns.Add("Author", typeof(string));
                barcode.Columns.Add("Edition", typeof(string));
                barcode.Columns.Add("Price", typeof(string));
                barcode.Columns.Add("ClassNo", typeof(string));
                barcode.Columns.Add("CallNo", typeof(string));
                barcode.Columns.Add("Status", typeof(string));
                barcode.Columns.Add("Publisher", typeof(string));
                barcode.Columns.Add("Department", typeof(string));
                barcode.Columns.Add("Subject", typeof(string));
                barcode.Columns.Add("Purchase", typeof(string));
                barcode.Columns.Add("BillNo", typeof(string));
                barcode.Columns.Add("Type", typeof(string));
                barcode.Columns.Add("CopyNo", typeof(string));
                barcode.Columns.Add("BarCode", typeof(string));

                
                #endregion

                #region value
                int sno = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    drbar = barcode.NewRow();
                    drbar["AccNo"] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                    drbar["Title"] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                    drbar["Author"] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                    drbar["Edition"] = Convert.ToString(ds.Tables[0].Rows[i]["Edition"]);
                    drbar["Price"] = Convert.ToString(ds.Tables[0].Rows[i]["Price"]);
                    drbar["ClassNo"] = Convert.ToString(ds.Tables[0].Rows[i]["Call_Des"]);
                    drbar["CallNo"] = Convert.ToString(ds.Tables[0].Rows[i]["Call_No"]);
                    drbar["Status"] = Convert.ToString(ds.Tables[0].Rows[i]["Book_Status"]);
                    drbar["Publisher"] = Convert.ToString(ds.Tables[0].Rows[i]["Publisher"]);
                    drbar["Department"] = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Code"]);
                    drbar["Subject"] = Convert.ToString(ds.Tables[0].Rows[i]["Subject"]);
                    drbar["Purchase"] = Convert.ToString(ds.Tables[0].Rows[i]["Pur_Don"]);
                    drbar["BillNo"] = Convert.ToString(ds.Tables[0].Rows[i]["Bill_No"]);
                    drbar["Type"] = Convert.ToString(ds.Tables[0].Rows[i]["typeofbook"]);
                    drbar["CopyNo"] = Convert.ToString(ds.Tables[0].Rows[i]["CopyNo"]);
                   
                    string LibBarCode = Convert.ToString(ds.Tables[0].Rows[i]["barcode"]);


                     drbar["BarCode"] = "*" + LibBarCode + "*";
                     barcode.Rows.Add(drbar);
                }

                #endregion

                grdBarcode.DataSource = barcode;
                grdBarcode.DataBind();
                grdBarcode.Visible = true;
                divspread.Visible = true;
                select_range.Visible = true;

                imgdiv2.Visible = false;
                lbl_alert.Text = "";
            }
            else
            {
                divspread.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Record Found";
            }
        }
        catch (Exception ex)
        {
        }
    }

 

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void btnprint_click(object sender, EventArgs e)
    {
        try
        {
            int RowCheckedCnt = 0;
            if (grdBarcode.Rows.Count > 0)
            {
                
                foreach (GridViewRow row in grdBarcode.Rows)
                {
                    CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                    if (cbsel.Checked == true)
                    {
                        RowCheckedCnt++;
                    }
                }
            }
            PdfDocument mydoc = new PdfDocument(PdfDocumentFormat.InCentimeters(24, 30));
            Gios.Pdf.PdfDocument mypdf = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);

            Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
            Font Fontsmall = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontbold = new Font("Book Antique", 10, FontStyle.Bold);
            Font Fontnormal = new Font("Book Antique", 10, FontStyle.Regular);
            Font fontCoverNo = new Font("IDAutomationHC39M", 10, FontStyle.Bold);

            List<string> Acc_No = new List<string>();
            mypdfpage = mydoc.NewPage();
            string collegename = "";
            string address1 = "";
            string address2 = "";
            string address3 = "";
            string PhNo = "";
            string faxno = "";
            string colquery = "select collname,address1,address2,address3,phoneno,faxno from collinfo where college_code='" + ddl_collegename.SelectedItem.Value + "'";
            DataSet ds1 = d2.select_method_wo_parameter(colquery, "Text");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                collegename = Convert.ToString(ds1.Tables[0].Rows[0]["collname"]);
                address1 = Convert.ToString(ds1.Tables[0].Rows[0]["address1"]);
                address2 = Convert.ToString(ds1.Tables[0].Rows[0]["address2"]);
                address3 = Convert.ToString(ds1.Tables[0].Rows[0]["address3"]);
                PhNo = Convert.ToString(ds1.Tables[0].Rows[0]["phoneno"]);
                faxno = Convert.ToString(ds1.Tables[0].Rows[0]["faxno"]);
            }
            PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                new PdfArea(mydoc, 140, 50, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, collegename);
            mypdfpage.Add(ptc);

            PdfTextArea ptc1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                new PdfArea(mydoc, 125, 65, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, address1 + "," + address2 + "," + address3);
            mypdfpage.Add(ptc1);
            PdfTextArea ptc2 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                new PdfArea(mydoc, 130, 80, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, PhNo + "," + faxno);
            mypdfpage.Add(ptc2);

            int SpreadCheckCount = 0;
            if (RowCheckedCnt > 30)
            {
                SpreadCheckCount = RowCheckedCnt / 2;
            }
            if (RowCheckedCnt < 30)
            {
                SpreadCheckCount = RowCheckedCnt;
            }

            Gios.Pdf.PdfTable table = mydoc.NewTable(Fontsmall, SpreadCheckCount + 1, 5, 1);

            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
            table.VisibleHeaders = false;
            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
            table.Cell(0, 0).SetContent("Acc No");
            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
            table.Cell(0, 0).SetFont(Fontbold);
            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            table.Cell(0, 1).SetContent("Title");
            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            table.Cell(0, 1).SetFont(Fontbold);
            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
            table.Cell(0, 2).SetContent("Author");
            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
            table.Cell(0, 2).SetFont(Fontbold);
            table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
            table.Cell(0, 3).SetContent("Class No");
            table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
            table.Cell(0, 3).SetFont(Fontbold);
            table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
            table.Cell(0, 4).SetContent("Bar code");
            table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
            table.Cell(0, 4).SetFont(Fontbold);
            table.Columns[0].SetWidth(50);
            table.Columns[0].SetCellPadding(9);
            table.Columns[1].SetWidth(200);
            table.Columns[1].SetCellPadding(9);
            table.Columns[2].SetWidth(150);
            table.Columns[2].SetCellPadding(9);
            table.Columns[3].SetWidth(80);
            table.Columns[3].SetCellPadding(9);
            table.Columns[4].SetWidth(100);
            table.Columns[4].SetCellPadding(9);
            int img_pos = 212;
            int TR = 1;
            //for (int dsrow = 0; dsrow < SpreadCheckCount; dsrow++)
            if (SpreadCheckCount > 0)
            {
               
                foreach (GridViewRow row in grdBarcode.Rows)
                {
                    CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                    int RowCnt = Convert.ToInt32(row.RowIndex);
                    if (cbsel.Checked == true)
                    {
                        string accNo = Convert.ToString(grdBarcode.Rows[RowCnt].Cells[2].Text);
                        string title = Convert.ToString(grdBarcode.Rows[RowCnt].Cells[3].Text);
                        string author = Convert.ToString(grdBarcode.Rows[RowCnt].Cells[4].Text);
                        string classNo = Convert.ToString(grdBarcode.Rows[RowCnt].Cells[7].Text);
                        string LibBarCode = Convert.ToString(grdBarcode.Rows[RowCnt].Cells[17].Text);

                        string barCode = LibBarCode;
                        System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();

                        table.Cell(TR, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(TR, 0).SetCellPadding(13);
                        table.Cell(TR, 0).SetContent(Convert.ToString(accNo));
                        table.Cell(TR, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(TR, 1).SetCellPadding(13);
                        table.Cell(TR, 1).SetContent(Convert.ToString(title));
                        table.Cell(TR, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(TR, 2).SetCellPadding(13);
                        table.Cell(TR, 2).SetContent(Convert.ToString(author));
                        table.Cell(TR, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(TR, 3).SetCellPadding(13);
                        table.Cell(TR, 3).SetContent(Convert.ToString(classNo));

                        using (Bitmap bitMap = new Bitmap(accNo.Length * 40, 80))
                        {
                            using (Graphics graphics = Graphics.FromImage(bitMap))
                            {
                                Font oFont = new Font("IDAutomationHC39M", 16);
                                PointF point = new PointF(2f, 2f);
                                SolidBrush blackBrush = new SolidBrush(Color.Black);
                                SolidBrush whiteBrush = new SolidBrush(Color.White);
                                graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                                graphics.DrawString("*" + accNo + "*", oFont, blackBrush, point);
                            }
                            using (MemoryStream ms = new MemoryStream())
                            {
                                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                byte[] byteImage = ms.ToArray();

                                if (File.Exists(HttpContext.Current.Server.MapPath("~/BarCode/" + accNo + ".jpeg")))
                                {
                                    PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/BarCode/" + accNo + ".jpeg"));
                                    mypdfpage.Add(LogoImage1, 580, img_pos, 200);
                                }
                                else
                                {
                                    File.WriteAllBytes(Server.MapPath("~/BarCode/" + accNo + ".jpeg"), byteImage);

                                    DirectoryInfo dir = new DirectoryInfo("~/BarCode/" + accNo + ".jpeg");
                                    dir.Refresh();
                                    ms.Dispose();
                                    ms.Close();
                                    PdfImage LogoImage1 = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/BarCode/" + accNo + ".jpeg"));
                                    mypdfpage.Add(LogoImage1, 580, img_pos, 200);
                                }
                            }
                            img_pos += 36;
                        }
                        TR++;
                    }
                }
                //}
                Gios.Pdf.PdfTablePage newpdftabpage1 = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, 180, 650, 1200));
                mypdfpage.Add(newpdftabpage1);
                mypdfpage.SaveToDocument();

                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "LibraryBarcode" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmss") + ".pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please select the record";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "BarcodeGeneration"); 
        }
    }

    protected void BtnBarCodePrint_click(object sender, EventArgs e)
    {
        try
        {
            string accNo = string.Empty;
            string strData = string.Empty;
            string[] name = new string[15];
            string finalBarcodestr = string.Empty;
            string[] BarCodeText = { "^XA~TA000~JSN^LT0^MNW^MTT^PON^PMN^LH0,0^JMA^PR2,2^MD10^JUS^LRN^CI0^XZ$", "^XA$", "^MMT$", "^LL0160$", "^PW607$", "^LS0$", "^XZ$" };
            foreach (GridViewRow row in grdBarcode.Rows)
            {
                CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                int RowCnt = Convert.ToInt32(row.RowIndex);
                if (cbsel.Checked == true)
                {
                    accNo = Convert.ToString(grdBarcode.Rows[RowCnt].Cells[2].Text);
                    string title = Convert.ToString(grdBarcode.Rows[RowCnt].Cells[3].Text);
                    string Author = Convert.ToString(grdBarcode.Rows[RowCnt].Cells[4].Text);
                    string classNo = Convert.ToString(grdBarcode.Rows[RowCnt].Cells[7].Text);
                    string CallNo = Convert.ToString(grdBarcode.Rows[RowCnt].Cells[8].Text);
                    string deptName = Convert.ToString(grdBarcode.Rows[RowCnt].Cells[11].Text);
                    string copyNo = Convert.ToString(grdBarcode.Rows[RowCnt].Cells[16].Text);

                    string[] strDataVal = { "^XA$", "^BY2,3,49^FT50,88^BCN,,N,N^FD>:" + accNo + "^FS$", @"^FT40,121^A0B,21,24^FH\^FDJMC LIb^FS$", @"^FT28,30^A0N,25,19^FH\^FD" + title + "^FS$", @"^FT46,145^A0N,25,24^FH\^FD" + Author + "^FS$", @"^FT154,143^A0N,24,28^FH\^FD" + CallNo + "^FS$", @"^FT74,117^A0N,28,38^FH\^FD" + accNo + "^FS$", @"^FT386,66^A0N,39,45^FH\^FD" + CallNo + "^FS$", @"^FT394,117^A0N,38,40^FH\^FD" + Author + "^FS$", "^PQ1,0,1,Y^XZ$", "^FX$", "^XA$", "^IDR:ID*.*$", "^XZ$", "^XZ$" };
                    for (int barcode = 0; barcode < strDataVal.Length; barcode++)
                    {
                        finalBarcodestr = finalBarcodestr + Convert.ToString(strDataVal[barcode]);
                    }
                }
            }
            string final = string.Empty;
            for (int i = 0; i < BarCodeText.Length; i++)
            {
                final = final + Convert.ToString(BarCodeText[i]);
            }

            string finalBarcodeString = final + finalBarcodestr;
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szFile = @"C:\barcode\Barcode.txt";
                using (StreamWriter sw = new StreamWriter(szFile))
                {
                    sw.Write(finalBarcodeString.Replace("$", "\r\n"));
                }
                string str_Path = @"C:\barcode\insbar.bat";
                ProcessStartInfo processInfo = new ProcessStartInfo(str_Path);
                processInfo.UseShellExecute = false;
                Process batchProcess = new Process();
                batchProcess.StartInfo = processInfo;
                batchProcess.Start();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "BarcodeGeneration"); 
        }
    }

    protected void grdBarcode_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        grdBarcode.PageIndex = e.NewPageIndex;
        loadspreadvalues();
    }


    protected void Btn_range_Click(object sender, EventArgs e)
    {
        if (txt_frange.Text == "" || txt_trange.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Both From And To Range.')", true);
            return;
        }

        if (Convert.ToInt32(txt_frange.Text) > Convert.ToInt32(txt_trange.Text))
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('To Range Should Be Greater Than Or Equal To From Range.')", true);
            return;
        }

        foreach (GridViewRow row in grdBarcode.Rows)
        {
            Label sno = (Label)row.FindControl("lbl_sno");
            string sl_no = sno.Text;
            if (sl_no != "")
            {
                CheckBox cbsel = (CheckBox)row.FindControl("chkenbl");
                if (Convert.ToInt32(sl_no) >= Convert.ToInt32(txt_frange.Text) && Convert.ToInt32(sl_no) <= Convert.ToInt32(txt_trange.Text))
                {
                    cbsel.Checked = true;
                }
                else
                {
                    cbsel.Checked = false;
                }
            }
        }

        txt_frange.Text = "";
        txt_trange.Text = "";
    }

}