using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Web.Services;
using System.Drawing;
using System.IO;

public partial class Item_master : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable newhash = new Hashtable();
    bool check = false;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ////****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("invM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["Inventory"].Trim();
            //            Response.Redirect(redrURI, false);
            //            return;
            //        }
            //    }
            //}
            //if (Session["collegecode"] == null)
            //{
            //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
            //    Response.Redirect(redrURI, false);
            //    return;
            //}
            ////****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("inventoryindex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/InventoryMod/inventoryindex.aspx");
                    return;
                }
            }
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                rdb_nonacademic.Checked = true;
                loadheadername();
                // bind_subheader();
                loadsubheadername();
                loaditem();

                bind_subheader();
                binditemunit();
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.Visible = false;
                btn_go_Click(sender, e);
                txt_searchby.Visible = true;
            }
            lblvalidation1.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        Newdiv.Visible = false;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct ItemName from IM_ItemMaster WHERE ItemName like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["ItemName"].ToString());
            }
        }
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getitemcode(string prefixText)
    {

        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct ItemCode from IM_ItemMaster WHERE ItemCode like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["ItemCode"].ToString());
            }
        }
        return name;

    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getitemheader(string prefixText)
    {

        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct ItemHeaderName from IM_ItemMaster WHERE ItemHeaderName like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["ItemHeaderName"].ToString());
            }
        }
        return name;

    }

    [WebMethod]
    //public string CheckUserName(string StoreName)
    public static string CheckUserName(string StoreName, string ItemHeadName, string ItemSubHeadName)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string user_name = StoreName;
            string ItemHeaderCode1 = ItemHeadName;
            string subheader_code1 = ItemSubHeadName;
            if (user_name.Trim() != "" && user_name != null)
            {
                string query = dd.GetFunction("select distinct ItemName,StoreFK from IM_ItemMaster  where ItemName ='" + user_name + "' and ItemHeaderCode='" + ItemHeaderCode1 + "' and subheader_code='" + subheader_code1 + "'");
                //string query = dd.GetFunction("select distinct ItemName,StoreFK from IM_ItemMaster  where ItemName ='" + user_name + "' ");
                if (query.Trim() == "" || query == null || query == "0" || query == "-1")
                {
                    returnValue = "0";
                }
            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_type.SelectedValue == "0")
        {
            txt_searchby.Visible = true;
            txt_searchitemcode.Visible = false;
            txt_searchheadername.Visible = false;
            txt_searchheadername.Text = "";
            txt_searchitemcode.Text = "";
        }
        else if (ddl_type.SelectedValue == "1")
        {
            txt_searchby.Visible = false;
            txt_searchitemcode.Visible = true;
            txt_searchheadername.Visible = false;
            txt_searchby.Text = "";
            txt_searchheadername.Text = "";

        }
        else if (ddl_type.SelectedValue == "2")
        {
            txt_searchby.Visible = false;
            txt_searchitemcode.Visible = false;
            txt_searchheadername.Visible = true;
            txt_searchby.Text = "";

            txt_searchitemcode.Text = "";
        }
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            string itemheadercode = "";
            for (int i = 0; i < cbl_headername.Items.Count; i++)
            {
                if (cbl_headername.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                }
            }
            string itemcode = "";
            for (int i = 0; i < cbl_itemname.Items.Count; i++)
            {
                if (cbl_itemname.Items[i].Selected == true)
                {
                    if (itemcode == "")
                    {
                        itemcode = "" + cbl_itemname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemcode = itemcode + "'" + "," + "'" + cbl_itemname.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemcode.Trim() != "" && itemheadercode.Trim() != "")
            {
                string selectquery = "";

                if (txt_searchby.Text.Trim() != "")
                {
                    selectquery = "select ItemHeaderName,ItemHeaderCode,ItemCode,ItemName ,ItemModel,ItemSize ,ItemUnit,ItemSpecification,isnull(Tamil_ItemName,'')Tamil_ItemName from IM_ItemMaster where ItemName='" + txt_searchby.Text + "' order by ItemHeaderCode";
                }
                else if (txt_searchitemcode.Text.Trim() != "")
                {
                    selectquery = "select ItemHeaderName,ItemHeaderCode,ItemCode,ItemName ,ItemModel,ItemSize ,ItemUnit,ItemSpecification,isnull(Tamil_ItemName,'')Tamil_ItemName from IM_ItemMaster where ItemCode='" + txt_searchitemcode.Text + "' order by ItemHeaderCode";
                }
                else if (txt_searchheadername.Text.Trim() != "")
                {
                    selectquery = "select ItemHeaderName,ItemHeaderCode,ItemCode,ItemName ,ItemModel,ItemSize ,ItemUnit,ItemSpecification,isnull(Tamil_ItemName,'')Tamil_ItemName from IM_ItemMaster where ItemHeaderName='" + txt_searchheadername.Text + "' order by ItemHeaderCode";
                }
                else
                {
                    selectquery = "select ItemHeaderName,ItemHeaderCode,ItemCode,ItemName ,ItemModel,ItemSize ,ItemUnit,ItemSpecification,isnull(Tamil_ItemName,'')Tamil_ItemName from IM_ItemMaster where ItemHeaderCode in ('" + itemheadercode + "') and ItemCode in('" + itemcode + "') order by ItemHeaderCode";
                }


                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.Sheets[0].ColumnCount = 0;
                    Fpspread1.CommandBar.Visible = false;
                    Fpspread1.Sheets[0].AutoPostBack = true;
                    Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    Fpspread1.Sheets[0].ColumnCount = 9;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Header";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Code";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Name/Tamil Item Name";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Model";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Size";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Unit";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Specification";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[7].Visible = false;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Item Specification";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspread1.Sheets[0].RowCount++;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["ItemHeaderName"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["ItemHeaderCode"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["ItemCode"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        string name = "";
                        if (Convert.ToString(ds.Tables[0].Rows[row]["Tamil_ItemName"]) != "")
                            name = Convert.ToString(ds.Tables[0].Rows[row]["ItemName"]) + "/" + Convert.ToString(ds.Tables[0].Rows[row]["Tamil_ItemName"]);
                        else
                        name = Convert.ToString(ds.Tables[0].Rows[row]["ItemName"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = name;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "amudham";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["ItemModel"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["ItemSize"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["ItemUnit"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                        //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["description"]);
                        //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                        //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[row]["ItemSpecification"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                    }
                    Fpspread1.Visible = true;
                    rptprint.Visible = true;
                    div1.Visible = true;
                    lbl_error.Visible = false;
                    Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    Fpspread1.Columns[1].VerticalAlign = VerticalAlign.Middle;
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;

                }
                else
                {
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Records Found";
                }
            }
            else
            {
                div1.Visible = false;
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select All Fields";
            }
            txt_searchby.Text = "";
            txt_searchitemcode.Text = "";
            txt_searchheadername.Text = "";
        }
        catch
        {

        }

    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        try
        {
            Newitem();
        }
        catch
        {
        }
    }

    public void Newitem()
    {
        try
        {
            clear();
            string newitemcode = "";
            string selectquery = "select ItemAcr,ItemStNo,ItemSize  from IM_CodeSettings order by startdate desc";//where Latestrec =1"
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["ItemAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["ItemStNo"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["ItemSize"]);
                if (itemacronym.Trim() != "" && itemstarno.Trim() != "")
                {
                    selectquery = " select distinct top (1) ItemCode  from IM_ItemMaster where ItemCode like '" + Convert.ToString(itemacronym) + "%' order by ItemCode desc";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["ItemCode"]);
                        string itemacr = Convert.ToString(itemacronym);
                        int len = itemacr.Length;
                        itemcode = itemcode.Remove(0, len);
                        int len1 = Convert.ToString(itemcode).Length;
                        string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                        len = Convert.ToString(newnumber).Length;
                        len1 = len1 - len;
                        if (len1 == 2)
                        {
                            newitemcode = "00" + newnumber;
                        }
                        else if (len1 == 1)
                        {
                            newitemcode = "0" + newnumber;
                        }
                        else if (len1 == 3)
                        {
                            newitemcode = "000" + newnumber;
                        }
                        else if (len1 == 4)
                        {
                            newitemcode = "0000" + newnumber;
                        }
                        else if (len1 == 5)
                        {
                            newitemcode = "00000" + newnumber;
                        }
                        else if (len1 == 6)
                        {
                            newitemcode = "000000" + newnumber;
                        }
                        else
                        {
                            newitemcode = Convert.ToString(newnumber);
                        }
                        if (newitemcode.Trim() != "")
                        {
                            newitemcode = itemacr + "" + newitemcode;
                        }
                    }
                    else
                    {
                        string itemacr = Convert.ToString(itemstarno);
                        int len = itemacr.Length;
                        string items = Convert.ToString(itemsize);
                        int len1 = Convert.ToInt32(items);
                        int size = len1 - len;
                        if (size == 2)
                        {
                            newitemcode = "00" + itemstarno;
                        }
                        else if (size == 1)
                        {
                            newitemcode = "0" + itemstarno;
                        }
                        else if (size == 3)
                        {
                            newitemcode = "000" + itemstarno;
                        }
                        else if (size == 4)
                        {
                            newitemcode = "0000" + itemstarno;
                        }
                        else if (size == 5)
                        {
                            newitemcode = "00000" + itemstarno;
                        }
                        else if (size == 6)
                        {
                            newitemcode = "000000" + itemstarno;
                        }
                        else
                        {
                            newitemcode = Convert.ToString(itemstarno);
                        }
                        newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(newitemcode);
                    }
                    txt_itemcode1.Text = Convert.ToString(newitemcode);
                    poperrjs.Visible = true;
                    btnsave.Visible = true;
                    SelectdptGrid.Visible = false;
                    btnupdate.Visible = false;
                    btndelete.Visible = false;
                    bindstore();
                    binditemunit();
                    loadheadername();
                    loadsubheadername();
                    loaditem();
                    bind_subheader();
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Update Code Master";
                }
            }
        }
        catch
        {

        }
    }

    protected void cb_headername_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_headername.Checked == true)
            {
                for (int i = 0; i < cbl_headername.Items.Count; i++)
                {
                    cbl_headername.Items[i].Selected = true;
                }
                txt_headername.Text = "Header Name(" + (cbl_headername.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_headername.Items.Count; i++)
                {
                    cbl_headername.Items[i].Selected = false;
                }
                txt_headername.Text = "--Select--";
            }
            loadsubheadername();
            loaditem();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_headername_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            txt_headername.Text = "--Select--";
            cb_headername.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_headername.Items.Count; i++)
            {
                if (cbl_headername.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_headername.Text = "Header Name(" + commcount.ToString() + ")";
                if (commcount == cbl_headername.Items.Count)
                {
                    cb_headername.Checked = true;
                }
            }
            loadsubheadername();
            loaditem();
        }
        catch (Exception ex)
        {

        }
    }

    protected void cb_itemname_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_itemname.Checked == true)
            {
                for (int i = 0; i < cbl_itemname.Items.Count; i++)
                {
                    cbl_itemname.Items[i].Selected = true;
                }
                txt_itemname.Text = "Item Name(" + (cbl_itemname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_itemname.Items.Count; i++)
                {
                    cbl_itemname.Items[i].Selected = false;
                }
                txt_itemname.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void cbl_itemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_itemname.Text = "--Select--";
            cb_itemname.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_itemname.Items.Count; i++)
            {
                if (cbl_itemname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_itemname.Text = "Item Name(" + commcount.ToString() + ")";
                if (commcount == cbl_itemname.Items.Count)
                {
                    cb_itemname.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void loadheadername()
    {
        try
        {
            cbl_headername.Items.Clear();
            ddl_itemheadername1.Items.Clear();

            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and usercode='" + Session["usercode"] + "'";
            }
            string maninvalue = "";
            string selectnewquery = d2.GetFunction("select value  from Master_Settings where settings='ItemHeaderRights' " + columnfield + "");
            if (selectnewquery.Trim() != "" && selectnewquery.Trim() != "0")
            {
                string[] splitnew = selectnewquery.Split(',');
                if (splitnew.Length > 0)
                {
                    for (int row = 0; row <= splitnew.GetUpperBound(0); row++)
                    {
                        if (maninvalue == "")
                        {
                            maninvalue = Convert.ToString(splitnew[row]);
                        }
                        else
                        {
                            maninvalue = maninvalue + "'" + "," + "'" + Convert.ToString(splitnew[row]);
                        }
                    }
                }
            }

            //if (maninvalue.Trim() != "")
            //{
            ds.Clear();
            ds = d2.BindItemHeaderWithOutRights_inv();
            //string query = "";
            //query = "select distinct ItemHeaderCode,ItemHeaderName  from IM_ItemMaster ";
            //ds.Clear();
            //ds = d2.select_method_wo_parameter(query, "Text");
            //}
            //else
            //{
            //    ds.Clear();
            //    ds = d2.BindItemHeaderWithOutRights();
            //}

            if (ds.Tables[0].Rows.Count > 0)//referencess
            {
                cbl_headername.DataSource = ds;
                cbl_headername.DataTextField = "ItemHeaderName";
                cbl_headername.DataValueField = "ItemHeaderCode";
                cbl_headername.DataBind();

                ddl_itemheadername1.DataSource = ds;
                ddl_itemheadername1.DataTextField = "ItemHeaderName";
                ddl_itemheadername1.DataValueField = "ItemHeaderCode";
                ddl_itemheadername1.DataBind();

                ddl_itemheadername1.Items.Insert(0, "Select");
                ddl_itemheadername1.Items.Insert(ddl_itemheadername1.Items.Count, "Others");
                if (cbl_headername.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_headername.Items.Count; i++)
                    {
                        cbl_headername.Items[i].Selected = true;
                    }
                    txt_headername.Text = "Header Name(" + cbl_headername.Items.Count + ")";
                }
            }
            else
            {
                ddl_itemheadername1.Items.Insert(0, "Select");
                ddl_itemheadername1.Items.Insert(ddl_itemheadername1.Items.Count, "Others");
                txt_headername.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void bindsubgroup()
    {
        //ddl_subheader.Items.Insert(0, "Select");
        //ddl_subheader.Items.Insert(ddl_subheader.Items.Count, "Others");

    }

    public void loaditem()
    {
        try
        {
            cbl_itemname.Items.Clear();
            string itemheader = "";
            string subheader = "";
            for (int i = 0; i < cbl_headername.Items.Count; i++)
            {
                if (cbl_headername.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                }
            }

            for (int i = 0; i < cbl_subheadername.Items.Count; i++)
            {
                if (cbl_subheadername.Items[i].Selected == true)
                {
                    if (subheader == "")
                    {
                        subheader = "" + cbl_subheadername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        subheader = subheader + "'" + "," + "" + "'" + cbl_subheadername.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "" && subheader.Trim() != "")
            {
                ds.Clear();
                ds = d2.BindItemCodewithsubheaderMaster_inv(itemheader, subheader);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_itemname.DataSource = ds;
                    cbl_itemname.DataTextField = "ItemName";
                    cbl_itemname.DataValueField = "ItemCode";
                    cbl_itemname.DataBind();
                    if (cbl_itemname.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_itemname.Items.Count; i++)
                        {
                            cbl_itemname.Items[i].Selected = true;
                        }
                        txt_itemname.Text = "Item Name(" + cbl_itemname.Items.Count + ")";
                    }
                    if (cbl_itemname.Items.Count > 5)
                    {
                        Panel1.Width = 300;
                        Panel1.Height = 300;
                    }
                }
                else
                {
                    txt_itemname.Text = "--Select--";
                }
            }
            else
            {
                txt_itemname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void bindstore()
    {
        try
        {
            ddlpopdefaultstore.Items.Clear();
            ds.Clear();

            string storepk = d2.GetFunction("select value from Master_Settings where settings='Store Rights' and usercode='" + usercode + "'");
            ds = d2.BindStorebaseonrights_inv(storepk);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlpopdefaultstore.DataSource = ds;
                ddlpopdefaultstore.DataTextField = "StoreName";
                ddlpopdefaultstore.DataValueField = "StorePK";
                ddlpopdefaultstore.DataBind();
                ddlpopdefaultstore.Items.Insert(0, "Select");
            }
            else
            {
                ddlpopdefaultstore.Items.Insert(0, "Select");
            }
        }
        catch
        {
        }
    }
    public void binditemunit()
    {
        try
        {
            ddl_unit1.Items.Clear();
            string headerquery = "select distinct MasterCode,MasterValue  from CO_MasterValues where MasterCriteria ='itemunit' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(headerquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_unit1.DataSource = ds;
                ddl_unit1.DataTextField = "MasterValue";
                ddl_unit1.DataValueField = "MasterCode";
                ddl_unit1.DataBind();
                ddl_unit1.Items.Insert(0, "Select");
                //ddl_unit1.Items.Insert(ddl_unit1.Items.Count, "Others");
            }
            else
            {
                ddl_unit1.Items.Insert(0, "Select");
                //ddl_unit1.Items.Insert(ddl_unit1.Items.Count, "Others");
            }
        }
        catch
        {
        }
    }
    //public void bindunitddl()
    //{
    //    ddl_unit1.Items.Clear();
    //    ddl_unit1.Items.Insert(ddl_unit1.Items.Count, "Select");
    //    ddl_unit1.Items.Insert(ddl_unit1.Items.Count, "Kg");
    //    newhash.Add("Kg", ddl_unit1.Items.Count - 1);
    //    ddl_unit1.Items.Insert(ddl_unit1.Items.Count, "Ltrs");
    //    newhash.Add("Ltrs", ddl_unit1.Items.Count - 1);
    //    ddl_unit1.Items.Insert(ddl_unit1.Items.Count, "Nos");
    //    newhash.Add("Nos", ddl_unit1.Items.Count - 1);
    //    ddl_unit1.Items.Insert(ddl_unit1.Items.Count, "Box");
    //    newhash.Add("Box", ddl_unit1.Items.Count - 1);
    //    ddl_unit1.Items.Insert(ddl_unit1.Items.Count, "Bag");
    //    newhash.Add("Bag", ddl_unit1.Items.Count - 1);
    //}

    #region PlusMinusUnit
    protected void btnplus_Click(object sender, EventArgs e)
    {
        try
        {
            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Unit";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;
        }
        catch
        {

        }
    }

    protected void btnminus_Click(object sender, EventArgs e)
    {
        try
        {

            if (ddl_unit1.SelectedIndex != 0)
            {
                string sql = "delete from CO_MasterValues where MasterCode='" + ddl_unit1.SelectedItem.Value.ToString() + "' and MasterCriteria='itemunit'";
                int delete = d2.update_method_wo_parameter(sql, "Text");
                if (delete != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Deleted Successfully";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "No Record Selected";
                }
                binditemunit();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Record Selected";
            }
        }
        catch
        {


        }
    }


    protected void btn_addgroup_Click(object sender, EventArgs e)
    {
        try
        {
            string group = Convert.ToString(txt_addgroup.Text);
            int insert = 0;
            if (txt_addgroup.Text != "")
            {
                string sqladd = "if exists ( select * from CO_MasterValues where MasterValue='" + group + "' and MasterCriteria='itemunit') Update CO_MasterValues set MasterValue='" + group + "' where MasterValue='" + group + "' and MasterCriteria='itemunit' else insert into CO_MasterValues (MasterValue,MasterCriteria) values('" + group + "','itemunit')";
                insert = d2.update_method_wo_parameter(sqladd, "Text");
                if (insert != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Added Successfully";
                    binditemunit();
                    txt_addgroup.Text = "";
                    plusdiv.Visible = false;
                    panel_addgroup.Visible = false;
                }

            }

            else
            {
                plusdiv.Visible = true;
                lblerror.Visible = true;
                lblerror.Text = "Enter the Unit";
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

    protected void btn_deptexit_Click(object sender, EventArgs e)
    {
        try
        {
            Newdiv.Visible = false;
        }
        catch
        {

        }
    }
    protected void cbselectAll_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_selectall.Checked == true)
            {
                if (dptgrid.Rows.Count > 0)
                {
                    for (int i = 0; i < dptgrid.Rows.Count; i++)
                    {
                        (dptgrid.Rows[i].FindControl("cb_check") as CheckBox).Checked = true;
                    }
                }
            }
            if (cb_selectall.Checked == false)
            {
                if (dptgrid.Rows.Count > 0)
                {
                    for (int i = 0; i < dptgrid.Rows.Count; i++)
                    {
                        (dptgrid.Rows[i].FindControl("cb_check") as CheckBox).Checked = false;
                    }
                }
            }

        }
        catch
        {

        }
    }

    public void binddepartment()
    {
        try
        {
            string deptquery = "";
            //string deptquery = "select Dept_Code as DeptCode ,Dept_Name as DeptName from Department where college_code ='" + collegecode1 + "' order by Dept_Code ";
            if (rdb_academic.Checked == true)
            {
                deptquery = "select Dept_Code as DeptCode ,Dept_Name as DeptName from Department where college_code ='" + collegecode1 + "' and isacademic ='1' order by Dept_Code";
            }
            else if (rdb_nonacademic.Checked == true)
            {
                deptquery = "select Dept_Code as DeptCode ,Dept_Name as DeptName from Department where college_code ='" + collegecode1 + "' and isacademic ='0' order by Dept_Code";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                dptgrid.DataSource = ds;
                dptgrid.DataBind();
            }
        }
        catch
        {

        }
    }
    protected void rdb_academic_CheckedChanged(object sender, EventArgs e)
    {
        binddepartment();
    }
    protected void rdb_nonacademic_CheckedChanged(object sender, EventArgs e)
    {
        binddepartment();
    }
    protected void btn_deptsave_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DeptCode");
            dt.Columns.Add("DeptName");
            DataRow dr;
            if (dptgrid.Rows.Count > 0)
            {
                for (int ik = 0; ik < dptgrid.Rows.Count; ik++)
                {
                    if ((dptgrid.Rows[ik].FindControl("cb_check") as CheckBox).Checked == true)
                    {
                        dr = dt.NewRow();
                        dr[0] = Convert.ToString((dptgrid.Rows[ik].FindControl("lbl_deptcode") as Label).Text);
                        dr[1] = Convert.ToString((dptgrid.Rows[ik].FindControl("lbl_deptname") as Label).Text);
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    SelectdptGrid.DataSource = dt;
                    SelectdptGrid.DataBind();
                    Newdiv.Visible = false;
                    SelectdptGrid.Visible = true;
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Select All Fields";
                }
            }
        }
        catch
        {

        }
    }

    protected void btn_addpartment_Click(object sender, EventArgs e)
    {
        try
        {
            binddepartment();
            Newdiv.Visible = true;
        }
        catch
        {
        }
    }

    protected void bind_subheader()
    {
        try
        {
            ddl_subheader.Items.Clear();
            string headerquery = "select MasterValue,MasterCode from CO_MasterValues where MasterCriteria='Subheader' and CollegeCode='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(headerquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_subheader.DataSource = ds;
                ddl_subheader.DataTextField = "MasterValue";
                ddl_subheader.DataValueField = "MasterCode";
                ddl_subheader.DataBind();
                ddl_subheader.Items.Insert(0, "Select");
                ddl_subheader.Items.Insert(ddl_subheader.Items.Count, "Others");
            }
            else
            {
                ddl_subheader.Items.Insert(0, "Select");
                ddl_subheader.Items.Insert(ddl_subheader.Items.Count, "Others");
            }
        }
        catch { }

    }

    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {

            if (itemtamil.Checked)
            {
                if (Textitemtamil.Text == "")
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Please Enter Item Name In Tamil ";
                    return;
                }

            }

            string subheadername = "";
            string itemheadername = "";
            string itemheadercode = "";
            string itemcode = "";
            string itemacr = "";
            string itemname = "";
            string model = "";
            string size = "";
            string unit = "";
            string spceialinstruction = "";
            string specification = "";
            string storename = "";
            string storecode = "";
            string consumables = "";
            string forhostel = "";
            string isdpt = "";
            int code = 0;
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            itemheadername = Convert.ToString(ddl_itemheadername1.SelectedItem.Text);
            itemheadername = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(itemheadername);
            if (itemheadername.Trim() != "Others")
            {
                itemheadercode = Convert.ToString(ddl_itemheadername1.SelectedItem.Value);
            }
            else
            {
                itemheadername = Convert.ToString(txt_itemheadername1.Text);
                itemheadercode = Convert.ToString(Session["ItemHeaderCode"]);
            }
            itemcode = Convert.ToString(txt_itemcode1.Text);
            itemname = Convert.ToString(txt_itemname1.Text);
            itemacr = Convert.ToString(txt_itemacronym1.Text);
            model = Convert.ToString(txt_model1.Text);
            size = Convert.ToString(txt_size1.Text);
            unit = Convert.ToString(ddl_unit1.SelectedItem.Text);
            if (unit.Trim() == "Others")
            {
                unit = Convert.ToString(txt_unit1.Text);
            }
            //spceialinstruction = Convert.ToString(txtmulpopspecialinstruction.Text);
            specification = Convert.ToString(txtmulpopspecification.Text);
            if (ddlpopdefaultstore.SelectedItem.Text != "Select")
            {
                storename = Convert.ToString(ddlpopdefaultstore.SelectedItem.Text);
                storecode = Convert.ToString(ddlpopdefaultstore.SelectedItem.Value);
            }
            else
            {
                storecode = "0";
            }
            if (chkpophostel.Checked == true)
            {
                forhostel = "0";
            }
            else
            {
                forhostel = "1";
            }
            if (rdobtnpopconsumaties.Checked == true)
            {
                consumables = "0";
            }
            if (rdobtnpopnonconsumaties.Checked == true)
            {
                consumables = "1";
            }
            if (cb_selectall.Checked == true)
            {
                isdpt = "1";
            }
            else
            {
                isdpt = "0";
            }


            //06.11.15
            string subheader_code = "";
            string isgrp = "Subheader";

            subheadername = Convert.ToString(ddl_subheader.SelectedItem.Text);
            if (subheadername.Trim() != "Others")
            {
                subheader_code = Convert.ToString(ddl_subheader.SelectedItem.Value);
            }
            else
            {

                subheadername = Convert.ToString(txt_subheader.Text);
                subheader_code = subheadercode(isgrp, subheadername);

            }
            //        
            string itempk = d2.getitempk(itemcode);
            //string delete = " delete from IM_ItemMaster where ItemCode ='" + itemcode + "'";
            //delete = delete + " delete from IM_ItemDeptMaster where ItemFK ='" + itempk + "'";
            //int upnow = d2.update_method_wo_parameter(delete, "Text");

            string insertquery = "";
            if (itemtamil.Checked)
            {
                if (Textitemtamil.Text != "")
                {
                    insertquery = "Update IM_ItemMaster set ItemCode='" + itemcode + "' ,ItemAcr='" + itemacr + "',ItemName='" + itemname + "',ItemSpecification='" + specification + "',ItemHeaderCode='" + itemheadercode + "',ItemHeaderName='" + itemheadername + "',ItemUnit='" + unit + "',ForHostelItem='" + forhostel + "',StoreFK='" + storecode + "',ItemType='" + consumables + "',ItemModel='" + model + "',ItemSize='" + size + "',Tamil_ItemName=N'" + Textitemtamil.Text + "' where itempk='" + itempk + "'";

                }

            }
            else
            {
                insertquery = "Update IM_ItemMaster set ItemCode='" + itemcode + "' ,ItemAcr='" + itemacr + "',ItemName='" + itemname + "',ItemSpecification='" + specification + "',ItemHeaderCode='" + itemheadercode + "',ItemHeaderName='" + itemheadername + "',ItemUnit='" + unit + "',ForHostelItem='" + forhostel + "',StoreFK='" + storecode + "',ItemType='" + consumables + "',ItemModel='" + model + "',ItemSize='" + size + "' where itempk='" + itempk + "'";
            }

            //string insertquery = "insert into IM_ItemMaster (ItemCode,ItemAcr,ItemName,ItemSpecification,ItemHeaderCode,ItemHeaderName,ItemUnit,ForHostelItem,StoreFK,ItemType,ItemModel,ItemSize) values ('" + itemcode + "','" + itemacr + "','" + itemname + "','" + specification + "','" + itemheadercode + "','" + itemheadername + "','" + unit + "','" + forhostel + "','" + storecode + "','" + consumables + "','" + model + "','" + size + "')";
            if (itemheadername.Trim() != "")
            {
                insertquery = insertquery + " update IM_ItemMaster set ItemHeaderName ='" + itemheadername + "' where ItemHeaderCode ='" + itemheadercode + "'";
            }
            if (itemheadername.Trim() != "")
            {
                insertquery = insertquery + " update IM_ItemMaster set subheader_code ='" + subheader_code + "' where ItemHeaderName ='" + itemheadername + "' and ItemHeaderCode ='" + itemheadercode + "' and ItemCode ='" + itemcode + "'";
            }
            ds.Clear();
            int upd = d2.update_method_wo_parameter(insertquery, "Text");
            if (upd != 0)
            {
                string itempk1 = d2.GetFunction("select itempk from IM_ItemMaster where itemcode='" + itemcode + "'");
                string delete = "delete IM_ItemDeptMaster where ItemFK='" + itempk1 + "'";
                int up1 = d2.update_method_wo_parameter(delete, "Text");
                //if (up1 != 0)
                //{
                for (int i = 0; i < SelectdptGrid.Rows.Count; i++)
                {
                    string deptcode = "";
                    deptcode = Convert.ToString((SelectdptGrid.Rows[i].FindControl("lbl_deptcode") as Label).Text);
                    string itemdept = "insert into IM_ItemDeptMaster (ItemFK,ItemDeptFK)values ('" + itempk1 + "','" + deptcode + "')";
                    int up = d2.update_method_wo_parameter(itemdept, "Text");
                }
                // }
                loadheadername();
                loadsubheadername();
                loaditem();
                imgdiv2.Visible = true;
                lbl_alert.Text = "Updated Successfully";
                btn_go_Click(sender, e);
                poperrjs.Visible = false;

            }
        }
        catch
        {
        }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btndelete.Text == "Delete")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want to Delete this Record?";
            }
        }
        catch
        {
        }
    }
    protected void delete()
    {
        surediv.Visible = false;
        string itemcode = Convert.ToString(txt_itemcode1.Text);
        string delete = " delete from IM_ItemMaster where ItemCode ='" + itemcode + "'";
        string itempk1 = d2.getitempk(itemcode);
        delete = delete + " delete from IM_ItemDeptMaster where ItemFK ='" + itempk1 + "'";
        int upnow = d2.update_method_wo_parameter(delete, "Text");
        if (upnow != 0)
        {
            loadheadername();
            loadsubheadername();
            loaditem();
            binditemunit();
            imgdiv2.Visible = true;
            lbl_alert.Text = "Deleted Successfully";
            btn_go_Click(sender, e);
            poperrjs.Visible = false;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (itemtamil.Checked)
            {
                if (Textitemtamil.Text == "")
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Please Enter Item Name In Tamil ";
                    return;
                }

            }


            string subheader_code = "";
            string itemheadername = "";
            string itemheadercode = "";
            string itemcode = "";
            string itemacr = "";
            string itemname = "";
            string model = "";
            string size = "";
            string unit = "";
            string spceialinstruction = "";
            string specification = "";
            string storename = "";
            string storecode = "";
            string consumables = "";
            string forhostel = "";
            string isdpt = "";
            int code = 0;
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            itemheadername = Convert.ToString(ddl_itemheadername1.SelectedItem.Text);
            itemheadername = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(itemheadername);
            if (itemheadername.Trim() != "Others")
            {
                itemheadercode = Convert.ToString(ddl_itemheadername1.SelectedItem.Value);
            }
            else
            {
                itemheadername = Convert.ToString(txt_itemheadername1.Text);
                itemheadername = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(itemheadername);
                if (itemheadername.Trim() != "")
                {
                    string selectquery = d2.GetFunction("select ItemHeaderCode from IM_ItemMaster   order by CONVERT(numeric,ItemHeaderCode) desc");
                    if (selectquery.Trim() != "")
                    {
                        if (int.TryParse(selectquery, out code))
                        {
                            code = code + 1;
                            itemheadercode = Convert.ToString(code);
                        }
                        else
                        {
                            string applno = selectquery.Remove(0, 3);
                            int len = applno.Length;
                            int codevalue = Convert.ToInt32(applno);
                            codevalue = codevalue + 1;
                            int len1 = Convert.ToString(codevalue).Length;
                            len = len - len1;
                            if (len == 2)
                            {
                                itemheadercode = "00" + codevalue;
                            }
                            else if (len == 1)
                            {
                                itemheadercode = "0" + codevalue;
                            }
                            else
                            {
                                itemheadercode = Convert.ToString(codevalue);
                            }
                            if (itemheadercode.Trim() != "")
                            {
                                itemheadercode = "ITH" + itemheadercode;
                            }
                        }
                    }
                    else
                    {
                        code = 1;
                        itemheadercode = Convert.ToString(code);
                    }
                }
            }
            itemcode = Convert.ToString(txt_itemcode1.Text);
            itemname = Convert.ToString(txt_itemname1.Text);
            itemname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(itemname);
            itemacr = Convert.ToString(txt_itemacronym1.Text);
            model = Convert.ToString(txt_model1.Text);
            model = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(model);
            size = Convert.ToString(txt_size1.Text);
            unit = Convert.ToString(ddl_unit1.SelectedItem.Text);

            if (unit.Trim() == "Others")
            {
                unit = Convert.ToString(txt_unit1.Text);
            }
            spceialinstruction = Convert.ToString(txtmulpopspecialinstruction.Text);
            specification = Convert.ToString(txtmulpopspecification.Text);
            if (ddlpopdefaultstore.SelectedItem.Text != "Select")
            {
                storename = Convert.ToString(ddlpopdefaultstore.SelectedItem.Text);
                storecode = Convert.ToString(ddlpopdefaultstore.SelectedItem.Value);
            }
            else
            {
                storecode = "0";
            }
            if (chkpophostel.Checked == true)
            {
                forhostel = "0";
            }
            else
            {
                forhostel = "1";
            }
            if (rdobtnpopconsumaties.Checked == true)
            {
                consumables = "0";
            }
            if (rdobtnpopnonconsumaties.Checked == true)
            {
                consumables = "1";
            }
            if (cb_selectall.Checked == true)
            {
                isdpt = "1";
            }
            else
            {
                isdpt = "0";
            }
            string subheadername = "";
            string isgrp = "Subheader";

            subheadername = Convert.ToString(ddl_subheader.SelectedItem.Text);
            if (subheadername.Trim() != "Others")
            {
                subheader_code = Convert.ToString(ddl_subheader.SelectedItem.Value);
            }
            else
            {
                subheadername = Convert.ToString(txt_subheader.Text);
                subheader_code = subheadercode(isgrp, subheadername);
            }
            if (subheader_code.Trim() != "")
            {
                if (SelectdptGrid.Rows.Count > 0)
                {
                    int upd = 0;
                    if (itemtamil.Checked)
                    {
                        if (Textitemtamil.Text != "")
                        {
                            string insertquery = "insert into IM_ItemMaster (ItemCode,ItemName,ItemHeaderCode,ItemHeaderName,ItemUnit,ItemSpecification,ForHostelItem,StoreFK,ItemType,ItemModel,ItemSize,itemAcr,subheader_code,Tamil_ItemName) values ('" + itemcode + "','" + itemname + "','" + itemheadercode + "','" + itemheadername + "','" + unit + "','" + specification + "','" + forhostel + "','" + storecode + "','" + consumables + "','" + model + "','" + size + "','" + itemacr + "','" + subheader_code + "',N'" + Textitemtamil.Text + "')";
                            ds.Clear();
                            upd = d2.update_method_wo_parameter(insertquery, "Text");
                        }

                    }
                    else
                    {
                        string insertquery = "insert into IM_ItemMaster (ItemCode,ItemName,ItemHeaderCode,ItemHeaderName,ItemUnit,ItemSpecification,ForHostelItem,StoreFK,ItemType,ItemModel,ItemSize,itemAcr,subheader_code) values ('" + itemcode + "','" + itemname + "','" + itemheadercode + "','" + itemheadername + "','" + unit + "','" + specification + "','" + forhostel + "','" + storecode + "','" + consumables + "','" + model + "','" + size + "','" + itemacr + "','" + subheader_code + "')";
                        ds.Clear();
                        upd = d2.update_method_wo_parameter(insertquery, "Text");
                    }

                    if (upd != 0)
                    {
                        for (int i = 0; i < SelectdptGrid.Rows.Count; i++)
                        {
                            string deptcode = "";
                            deptcode = Convert.ToString((SelectdptGrid.Rows[i].FindControl("lbl_deptcode") as Label).Text);
                            string itempk = d2.getitempk(itemcode);
                            string insertdeptquery = "insert into IM_ItemDeptMaster(ItemFK,ItemDeptFK) values ('" + itempk + "','" + deptcode + "')";
                            int up = d2.update_method_wo_parameter(insertdeptquery, "Text");
                        }
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Saved Successfully";
                        loadheadername();
                        loadsubheadername();
                        loaditem();
                        Newitem();
                        btn_go_Click(sender, e);
                        poperrjs.Visible = true;
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Select Department Name ";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Select Sub Header Name ";
            }
        }
        catch
        {
        }
    }
    public string subheadercode(string textcri, string subjename)
    {
        string subjec_no = "";
        try
        {
            // subjename = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(subjename);
            string select_subno = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
            }
            else
            {
                string insertquery = "insert into CO_MasterValues(MasterCriteria,MasterValue,CollegeCode) values('" + textcri + "','" + subjename + "','" + collegecode1 + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
                    }
                }
            }
        }
        catch
        {

        }
        return subjec_no;
    }
    //public string subheadercode(string textcri, string subjename)
    //{
    //    int subjec_no = 0;
    //    try
    //    {
    //        string select_subno = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + collegecode1 + " and TextVal='" + subjename + "'";
    //        ds.Clear();
    //        ds = d2.select_method_wo_parameter(select_subno, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            subjec_no = Convert.ToInt32(ds.Tables[0].Rows[0]["TextCode"]);
    //        }
    //        else
    //        {
    //            string insertquery = "insert into textvaltable(TextCriteria,TextVal,college_code) values('" + textcri + "','" + subjename + "','" + collegecode1 + "')";
    //            int result = d2.update_method_wo_parameter(insertquery, "Text");
    //            if (result != 0)
    //            {
    //                string select_subno1 = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + collegecode1 + " and TextVal='" + subjename + "'";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(select_subno1, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    subjec_no = Convert.ToInt32(ds.Tables[0].Rows[0]["TextCode"]);
    //                }
    //            }
    //        }
    //    }
    //    catch
    //    {

    //    }
    //    return Convert.ToString(subjec_no);
    //}


    protected void Cell_Click(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {

        }
    }
    protected void Fpspread1_render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                ddl_itemheadername1.Enabled = true;
                //  ddl_subheader.Enabled = false;
                btnsave.Visible = false;
                btnupdate.Visible = true;
                btndelete.Visible = true;
                btnexit.Visible = true;
                binditemunit();
                loadheadername();
                bind_subheader();
                bindstore();
                poperrjs.Visible = true;
                string activerow = "";
                string activecol = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                collegecode = Session["collegecode"].ToString();
                if (activerow.Trim() != "")
                {
                    string itemheadercode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    string itemcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    string selectquery = "select * from IM_ItemMaster i where  ItemHeaderCode in ('" + itemheadercode + "') and ItemCode in('" + itemcode + "')";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string ihc = Convert.ToString(ds.Tables[0].Rows[0]["ItemHeaderName"]);
                        ddl_itemheadername1.SelectedIndex = ddl_itemheadername1.Items.IndexOf(ddl_itemheadername1.Items.FindByText(ihc));
                        //ddl_itemheadername1.SelectedItem.Text = Convert.ToString(ds.Tables[0].Rows[0]["ItemHeaderName"]);
                        //ddl_itemheadername1.SelectedItem.Value = Convert.ToString(ds.Tables[0].Rows[0]["ItemHeaderCode"]);
                        //Session["itemheaderCode"] = Convert.ToString(ds.Tables[0].Rows[0]["ItemHeaderCode"]);
                        txt_itemcode1.Text = Convert.ToString(ds.Tables[0].Rows[0]["ItemCode"]);
                        txt_itemacronym1.Text = Convert.ToString(ds.Tables[0].Rows[0]["ItemAcr"]);
                        txt_itemname1.Text = Convert.ToString(ds.Tables[0].Rows[0]["ItemName"]);
                        txt_model1.Text = Convert.ToString(ds.Tables[0].Rows[0]["ItemModel"]);
                        txt_size1.Text = Convert.ToString(ds.Tables[0].Rows[0]["ItemSize"]);

                        string tamilitemname = Convert.ToString(ds.Tables[0].Rows[0]["Tamil_ItemName"]);
                        if (tamilitemname != "")
                        {
                            itemtamil.Checked = true;
                            Textitemtamil.Text = tamilitemname;
                        }
                        else
                        {
                            itemtamil.Checked = false;
                            Textitemtamil.Text = "";
                        }

                        string subheader = Convert.ToString(ds.Tables[0].Rows[0]["subheader_code"]);
                        ddl_subheader.SelectedIndex = ddl_subheader.Items.IndexOf(ddl_subheader.Items.FindByValue(subheader));


                        string unit = Convert.ToString(ds.Tables[0].Rows[0]["ItemUnit"]);
                        ddl_unit1.SelectedIndex = ddl_unit1.Items.IndexOf(ddl_unit1.Items.FindByValue(unit));

                        // txtmulpopspecialinstruction.Text = Convert.ToString(ds.Tables[0].Rows[0]["ItemSpecification"]);
                        txtmulpopspecification.Text = Convert.ToString(ds.Tables[0].Rows[0]["ItemSpecification"]);
                        if (Convert.ToString(ds.Tables[0].Rows[0]["StoreFK"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[0]["StoreFK"]).Trim() != "0")
                        {
                            string storename = d2.GetFunction("select StoreName  from IM_StoreMaster where StorePK ='" + Convert.ToString(ds.Tables[0].Rows[0]["StoreFK"]) + "'");

                            ddlpopdefaultstore.SelectedIndex = ddlpopdefaultstore.Items.IndexOf(ddlpopdefaultstore.Items.FindByText(storename));
                        }

                        if (Convert.ToString(ds.Tables[0].Rows[0]["ForHostelItem"]).ToUpper() == "FALSE" || Convert.ToString(ds.Tables[0].Rows[0]["ForHostelItem"]) == "0")
                        {
                            chkpophostel.Checked = true;
                        }
                        else { chkpophostel.Checked = false; }

                        //chkpophostel.Checked = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0]["ForHostelItem"]));

                        if (Convert.ToString(ds.Tables[0].Rows[0]["ItemType"]) == "1" || Convert.ToString(ds.Tables[0].Rows[0]["ItemType"]) == "True")
                        {
                            rdobtnpopnonconsumaties.Checked = true;
                            rdobtnpopconsumaties.Checked = false;
                        }
                        else
                        {
                            rdobtnpopconsumaties.Checked = true;
                            rdobtnpopnonconsumaties.Checked = false;
                        }
                        DataSet ds1 = new DataSet();
                        string deptquery = "select d.Dept_Code,Dept_Name from IM_ItemDeptMaster im,Department d where ItemFK='" + Convert.ToString(ds.Tables[0].Rows[0]["itempk"].ToString()) + "' and d.dept_code=im.itemdeptfk";
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(deptquery, "Text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            dt.Columns.Add("DeptCode");
                            dt.Columns.Add("DeptName");
                            DataRow dr;
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                for (int ik = 0; ik < ds1.Tables[0].Rows.Count; ik++)
                                {
                                    dr = dt.NewRow();
                                    dr[0] = Convert.ToString(ds1.Tables[0].Rows[ik]["Dept_Code"]);
                                    dr[1] = Convert.ToString(ds1.Tables[0].Rows[ik]["Dept_Name"]);
                                    dt.Rows.Add(dr);

                                }
                                if (dt.Rows.Count > 0)
                                {
                                    SelectdptGrid.DataSource = dt;
                                    SelectdptGrid.DataBind();
                                    Newdiv.Visible = false;
                                    SelectdptGrid.Visible = true;
                                }
                            }
                        }

                    }

                }
                btnupdate.Visible = true;
                btndelete.Visible = true;
                btnsave.Visible = false;
            }
        }
        catch
        {

        }
    }
    protected void btnexit_Click(object sender, EventArgs e)
    {
        try
        {
            poperrjs.Visible = false;
        }
        catch
        {

        }
    }

    public void clear()
    {
        try
        {
            ddl_itemheadername1.Enabled = true;
            ddl_subheader.Enabled = true;
            txt_subheader.Text = "";
            txt_itemheadername1.Text = "";
            txt_itemcode1.Text = "";
            txt_itemacronym1.Text = "";
            txt_itemname1.Text = "";
            //  txtpopitemnametamil.Text = "";
            txt_model1.Text = "";
            txt_size1.Text = "";
            txt_unit1.Text = "";
            txtmulpopspecialinstruction.Text = "";
            txtmulpopspecification.Text = "";
            ddl_subheader.SelectedItem.Text = "Select";
            ddl_itemheadername1.SelectedItem.Text = "Select";
            ddlpopdefaultstore.SelectedItem.Text = "Select";
            ddl_unit1.SelectedItem.Text = "Select";
            txt_validity.Text = "";
            chkpophostel.Checked = false;
            chkpopundersaftycondition.Checked = false;
            rdobtnpopconsumaties.Checked = true;
            rdobtnpopnonconsumaties.Checked = false;
        }
        catch
        {

        }


    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Item Master Report";
            string pagename = "Item_master.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {

        }

    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch
        {

        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        //surediv.Visible = false;
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
        poperrjs.Visible = true;
    }

    public object sender { get; set; }
    public EventArgs e { get; set; }

    protected void cb_subheadername_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_subheadername.Checked == true)
            {
                for (int i = 0; i < cbl_subheadername.Items.Count; i++)
                {
                    cbl_subheadername.Items[i].Selected = true;
                }
                txt_subheadername.Text = "Sub Header Name(" + (cbl_subheadername.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_subheadername.Items.Count; i++)
                {
                    cbl_subheadername.Items[i].Selected = false;
                }
                txt_subheadername.Text = "--Select--";
            }
            loaditem();

        }
        catch (Exception ex)
        {

        }
    }
    protected void cbl_subheadername_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_subheadername.Text = "--Select--";
            cb_subheadername.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_subheadername.Items.Count; i++)
            {
                if (cbl_subheadername.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_subheadername.Text = "Sub Header Name(" + commcount.ToString() + ")";
                if (commcount == cbl_subheadername.Items.Count)
                {
                    cb_subheadername.Checked = true;
                }
            }
            loaditem();
        }
        catch (Exception ex)
        {
        }
    }
    public void loadsubheadername()
    {
        try
        {
            cbl_subheadername.Items.Clear();
            string itemheader = "";
            for (int i = 0; i < cbl_headername.Items.Count; i++)
            {
                if (cbl_headername.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_headername.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                string query = "";
                //query = "select distinct t.TextCode,t.TextVal  from TextValTable t,IM_ItemMaster i where t.TextCode=i.subheader_code and ItemHeaderCode in ('" + itemheader + "') and college_code in ('" + collegecode1 + "')";
                query = "select distinct t.MasterCode,t.MasterValue  from CO_MasterValues t,IM_ItemMaster i where t.MasterCode=i.subheader_code and ItemHeaderCode in ('" + itemheader + "') and CollegeCode in ('" + collegecode1 + "') order by MasterValue";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                // ds.Clear();
                // ds = d2.BindItemCodeAll(itemheader);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_subheadername.DataSource = ds;
                    cbl_subheadername.DataTextField = "MasterValue";
                    cbl_subheadername.DataValueField = "MasterCode";
                    cbl_subheadername.DataBind();
                    if (cbl_subheadername.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_subheadername.Items.Count; i++)
                        {
                            cbl_subheadername.Items[i].Selected = true;
                        }
                        txt_subheadername.Text = "Sub Header Name(" + cbl_subheadername.Items.Count + ")";
                    }
                    if (cbl_subheadername.Items.Count > 5)
                    {
                        Panel2.Width = 300;
                        Panel2.Height = 300;
                    }
                }
                else
                {
                    txt_subheadername.Text = "--Select--";
                }
            }
            else
            {
                txt_subheadername.Text = "--Select--";
            }
        }
        catch
        {
        }
    }


    protected void itemtamil_OnCheckedChanged(object sender, EventArgs e)
    {
        if (itemtamil.Checked)
        {
            Textitemtamil.Font.Name = "Amudham";
            Textitemtamil.Visible = true;

        }
        else
            Textitemtamil.Visible = false;

    }
}
