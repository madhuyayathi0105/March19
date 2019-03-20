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

public partial class Inv_Dept_stockstatus_Report : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
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
            collegecode1 = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                bindmessname();
                loadheadername();
                loadsubheadername();
                loaditem();
                txtfrom.Attributes.Add("readOnly", "readOnly");
                txtto.Attributes.Add("readOnly", "readOnly");
                txtfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtto.Text = DateTime.Now.ToString("dd/MM/yyyy");

                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Visible = false;
                txtsearchby.Visible = true;
                rdb_deptwise.Checked = true;
                btngoclick(sender, e);

            }
            lblerror.Visible = false;
            lblvalidation1.Visible = false;
        }
        catch (Exception ex) { }
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
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct ItemName from IM_ItemMaster WHERE ItemName like '" + prefixText + "%'";
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
        string query = "select distinct ItemCode from IM_ItemMaster WHERE ItemCode like '" + prefixText + "%'";
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
        string query = "select distinct ItemHeaderName from IM_ItemMaster WHERE ItemHeaderName like '" + prefixText + "%'";
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

    protected void cb_deptname_oncheckedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_deptname.Checked == true)
            {
                for (int i = 0; i < cbl_deptname.Items.Count; i++)
                {
                    cbl_deptname.Items[i].Selected = true;
                }
                txt_deptname.Text = "Department(" + (cbl_deptname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_deptname.Items.Count; i++)
                {
                    cbl_deptname.Items[i].Selected = false;
                }
                txt_deptname.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void cbl_deptname_onselectedindexchange(object sender, EventArgs e)
    {
        try
        {
            txt_deptname.Text = "--Select--";
            cb_deptname.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_deptname.Items.Count; i++)
            {
                if (cbl_deptname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_deptname.Text = "Department(" + commcount.ToString() + ")";
                if (commcount == cbl_deptname.Items.Count)
                {
                    cb_deptname.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddltype_selectchange(object sender, EventArgs e)
    {
        if (ddltype.SelectedValue == "0")
        {
            txtsearchby.Visible = true;
            txtsearchitemcode.Visible = false;
            txtsearchheadername.Visible = false;
            txtsearchheadername.Text = "";
            txtsearchitemcode.Text = "";
        }
        else if (ddltype.SelectedValue == "1")
        {
            txtsearchby.Visible = false;
            txtsearchitemcode.Visible = true;
            txtsearchheadername.Visible = false;
            txtsearchby.Text = "";
            txtsearchheadername.Text = "";

        }
        else if (ddltype.SelectedValue == "2")
        {
            txtsearchby.Visible = false;
            txtsearchitemcode.Visible = false;
            txtsearchheadername.Visible = true;
            txtsearchby.Text = "";

            txtsearchitemcode.Text = "";
        }
    }

    protected void btngoclick(object sender, EventArgs e)
    {
        try
        {
            string itemheadercode = "";
            string item = "";
            bool chk = false;
            Printcontrol.Visible = false;
            for (int i = 0; i < cblheadername.Items.Count; i++)
            {
                if (cblheadername.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cblheadername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cblheadername.Items[i].Value.ToString() + "";
                    }
                }
            }
            string itemcode = "";
            for (int i = 0; i < cblitemname.Items.Count; i++)
            {
                if (cblitemname.Items[i].Selected == true)
                {
                    if (itemcode == "")
                    {
                        itemcode = "" + cblitemname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemcode = itemcode + "'" + "," + "'" + cblitemname.Items[i].Value.ToString() + "";
                    }
                }
            }

            string deptcode = "";
            for (int i = 0; i < cbl_deptname.Items.Count; i++)
            {
                if (cbl_deptname.Items[i].Selected == true)
                {
                    if (deptcode == "")
                    {
                        deptcode = "" + cbl_deptname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        deptcode = deptcode + "'" + "," + "'" + cbl_deptname.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (rdb_deptwise.Checked == true)
            {
                if (itemcode.Trim() != "" && itemheadercode.Trim() != "")
                {
                    string selectquery = "";
                    DataView dv = new DataView();
                    string firstdate = Convert.ToString(txtfrom.Text);
                    string seconddate = Convert.ToString(txtto.Text);
                    DateTime dt = new DateTime();
                    DateTime dt1 = new DateTime();
                    string[] split = firstdate.Split('/');
                    dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                    split = seconddate.Split('/');
                    dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

                    item = d2.getitempk(Convert.ToString(txtsearchitemcode.Text));
                    string headername = "";
                    headername = d2.GetFunction("select ItemHeaderCode from IM_ItemMaster where ItemHeaderName ='" + Convert.ToString(txtsearchheadername.Text) + "'");
                    if (txtsearchby.Text.Trim() != "")
                    {
                        selectquery = "select i.ItemPK,i.ItemCode ,i.ItemName ,sum(issuedQty)issuedQty ,sum(isnull(UsedQty,0))as UsedQty,d.Dept_Code,d.Dept_Name,sum(IssuedQty-ISNULL(UsedQty,0))as Handonqty from IT_StockDeptDetail s,IM_ItemMaster i,Department d where s.ItemFK=i.ItemPK and d.Dept_Code=s.DeptFK  and i.itemname in('" + Convert.ToString(txtsearchby.Text) + "') group by itempk,ItemCode,ItemName,UsedQty,Dept_Code,Dept_Name order by itemcode";

                    }
                    else if (txtsearchitemcode.Text.Trim() != "")
                    {
                        selectquery = "select i.ItemPK,i.ItemCode ,i.ItemName ,sum(issuedQty)issuedQty ,sum(isnull(UsedQty,0))as UsedQty,d.Dept_Code,d.Dept_Name,sum(IssuedQty-ISNULL(UsedQty,0))as Handonqty from IT_StockDeptDetail s,IM_ItemMaster i,Department d where s.ItemFK=i.ItemPK and d.Dept_Code=s.DeptFK  and s.itemfk in('" + item + "') group by itempk,ItemCode,ItemName,UsedQty,Dept_Code,Dept_Name order by itemcode";

                    }
                    else if (txtsearchheadername.Text.Trim() != "")
                    {
                        selectquery = "select i.ItemPK,i.ItemCode ,i.ItemName ,sum(issuedQty)issuedQty ,sum(isnull(UsedQty,0))as UsedQty,d.Dept_Code,d.Dept_Name,sum(IssuedQty-ISNULL(UsedQty,0))as Handonqty from IT_StockDeptDetail s,IM_ItemMaster i,Department d where s.ItemFK=i.ItemPK and d.Dept_Code=s.DeptFK  and i.ItemHeaderCode in('" + headername + "') group by itempk,ItemCode,ItemName,UsedQty,Dept_Code,Dept_Name order by itemcode ";
                    }
                    else
                    {
                        selectquery = "select i.ItemPK,i.ItemCode ,i.ItemName ,sum(issuedQty)issuedQty ,sum(isnull(UsedQty,0))as UsedQty,d.Dept_Code,d.Dept_Name,sum(IssuedQty-ISNULL(UsedQty,0))as Handonqty  from IT_StockDeptDetail s,IM_ItemMaster i,Department d where s.ItemFK=i.ItemPK and d.Dept_Code=s.DeptFK and s.DeptFK in('" + deptcode + "') and s.itemfk in('" + itemcode + "')  group by itempk,ItemCode,ItemName,UsedQty,Dept_Code,Dept_Name order by itemcode";
                    }
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 6;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Columns[0].Width = 50;
                        FpSpread1.Columns[0].Locked = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Columns[1].Width = 150;
                        FpSpread1.Columns[2].Locked = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Columns[2].Width = 150;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total Quantity";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Columns[3].Width = 150;
                        FpSpread1.Columns[3].Locked = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Used Quantity";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Columns[4].Width = 150;
                        FpSpread1.Columns[4].Locked = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Hand on Quantity";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Columns[5].Width = 150;
                        FpSpread1.Columns[5].Locked = true;

                        if (cbl_deptname.Items.Count > 0)//ddl_messname.SelectedItem.Selected
                        {
                            for (int i = 0; i < cbl_deptname.Items.Count; i++)
                            {
                                if (cbl_deptname.Items[i].Selected == true)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = "Dept_Code='" + Convert.ToString(cbl_deptname.Items[i].Value) + "'";
                                    DataView dv1 = ds.Tables[0].DefaultView;
                                    if (dv1.Count > 0)
                                    {
                                        FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv1[0]["Dept_Name"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);

                                        for (int row = 0; row < dv1.Count; row++)
                                        {
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["item_code"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv1[row]["ItemCode"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["item_name"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv1[row]["itemname"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                            double handquery = Math.Round(Convert.ToDouble(dv1[row]["issuedQty"]), 2);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(handquery);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                            double consume = Math.Round(Convert.ToDouble(dv1[row]["UsedQty"]), 2);

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(consume);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv1[row]["Handonqty"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                            chk = false;
                                        }
                                    }
                                    else
                                    {
                                        chk = true;
                                    }
                                }
                            }
                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        lblerror.Visible = false;
                        FpSpread1.Visible = true;
                        spreaddiv1.Visible = true;
                        rptprint.Visible = true;
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "No Record Found";
                        FpSpread1.Visible = false;
                        spreaddiv1.Visible = false;
                        rptprint.Visible = false;
                    }
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select All Field";
                    FpSpread1.Visible = false;
                    spreaddiv1.Visible = false;
                }
                //if (chk == true)
                //{
                //    lblerror.Visible = true;
                //    lblerror.Text = "No Record Found";
                //    FpSpread1.Visible = false;
                //    spreaddiv1.Visible = false;
                //    rptprint.Visible = false;
                //}
            }
            else if (rdb_culmul.Checked == true)
            {
                if (itemcode.Trim() != "" && itemheadercode.Trim() != "")
                {
                    string selectquery = "";
                    DataView dv = new DataView();
                    string firstdate = Convert.ToString(txtfrom.Text);
                    string seconddate = Convert.ToString(txtto.Text);
                    DateTime dt = new DateTime();
                    DateTime dt1 = new DateTime();
                    string[] split = firstdate.Split('/');
                    dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                    split = seconddate.Split('/');
                    dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

                    item = d2.getitempk(Convert.ToString(txtsearchitemcode.Text));
                    string headername = "";
                    headername = d2.GetFunction("select ItemHeaderCode from IM_ItemMaster where ItemHeaderName ='" + Convert.ToString(txtsearchheadername.Text) + "'");
                    if (txtsearchby.Text.Trim() != "")
                    {
                        selectquery = "select i.ItemCode ,i.ItemName ,sum(issuedQty)issuedQty ,sum(isnull(UsedQty,0))as UsedQty,sum(IssuedQty-ISNULL(UsedQty,0))as Handonqty  from IT_StockDeptDetail s,IM_ItemMaster i,Department d where s.ItemFK=i.ItemPK and d.Dept_Code=s.DeptFK   and i.itemname in('" + Convert.ToString(txtsearchby.Text) + "') group by ItemCode,ItemName order by itemcode";
                    }
                    else if (txtsearchitemcode.Text.Trim() != "")
                    {
                        selectquery = "select i.ItemCode ,i.ItemName ,sum(issuedQty)issuedQty ,sum(isnull(UsedQty,0))as UsedQty,sum(IssuedQty-ISNULL(UsedQty,0))as Handonqty  from IT_StockDeptDetail s,IM_ItemMaster i,Department d where s.ItemFK=i.ItemPK and d.Dept_Code=s.DeptFK  and s.itemfk in('" + item + "')  group by ItemCode,ItemName order by itemcode";
                    }
                    else if (txtsearchheadername.Text.Trim() != "")
                    {
                        selectquery = "select i.ItemCode ,i.ItemName ,sum(issuedQty)issuedQty ,sum(isnull(UsedQty,0))as UsedQty,sum(IssuedQty-ISNULL(UsedQty,0))as Handonqty  from IT_StockDeptDetail s,IM_ItemMaster i,Department d where s.ItemFK=i.ItemPK and d.Dept_Code=s.DeptFK  and i.ItemHeaderCode in('" + headername + "')  group by ItemCode,ItemName order by itemcode";

                    }
                    else
                    {
                        selectquery = "select i.ItemCode ,i.ItemName ,sum(issuedQty)issuedQty ,sum(isnull(UsedQty,0))as UsedQty,sum(IssuedQty-ISNULL(UsedQty,0))as Handonqty  from IT_StockDeptDetail s,IM_ItemMaster i,Department d where s.ItemFK=i.ItemPK and d.Dept_Code=s.DeptFK and s.DeptFK in('" + deptcode + "') and s.itemfk in('" + itemcode + "')  group by ItemCode,ItemName order by itemcode";
                    }
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 6;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Columns[0].Width = 50;
                        FpSpread1.Columns[0].Locked = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Columns[1].Width = 150;
                        FpSpread1.Columns[2].Locked = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Columns[2].Width = 150;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total Quantity";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Columns[3].Width = 150;
                        FpSpread1.Columns[3].Locked = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Used Quantity";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Columns[4].Width = 150;
                        FpSpread1.Columns[4].Locked = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Hand on Quantity";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Columns[5].Width = 150;
                        FpSpread1.Columns[5].Locked = true;


                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["itemname"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            double handquery = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["issuedQty"]), 2);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(handquery);


                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            double consume = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["UsedQty"]), 2);

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(consume);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Handonqty"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            chk = false;
                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        lblerror.Visible = false;
                        FpSpread1.Visible = true;
                        spreaddiv1.Visible = true;
                        rptprint.Visible = true;
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "No Record Found";
                        FpSpread1.Visible = false;
                        spreaddiv1.Visible = false;
                        rptprint.Visible = false;
                    }
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select All Field";
                    FpSpread1.Visible = false;
                    spreaddiv1.Visible = false;
                    rptprint.Visible = false;
                }
            }
        }
        catch
        {
        }
    }
    protected void txtfrom_TextChanged(object sender, EventArgs e)
    {
        try
        {

            lblerror.Visible = false;
            string fromdate = txtfrom.Text;
            string todate = txtto.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                if (from > to)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Enter To Date Greater Than From Date";
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void txtto_TextChanged(object sender, EventArgs e)
    {
        try
        {

            lblerror.Visible = false;
            string fromdate = txtfrom.Text;
            string todate = txtto.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                if (from > to)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Enter To Date Grater Than From Date";
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void cbheadername_ChekedChange(object sender, EventArgs e)
    {
        try
        {

            if (cbheadername.Checked == true)
            {
                for (int i = 0; i < cblheadername.Items.Count; i++)
                {
                    cblheadername.Items[i].Selected = true;
                }
                txtheadername.Text = "Header Name(" + (cblheadername.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblheadername.Items.Count; i++)
                {
                    cblheadername.Items[i].Selected = false;
                }
                txtheadername.Text = "--Select--";
            }
            loadsubheadername();
            loaditem();
        }
        catch (Exception ex)
        {

        }
    }
    protected void cblheadername_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            txtheadername.Text = "--Select--";
            cbheadername.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cblheadername.Items.Count; i++)
            {
                if (cblheadername.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtheadername.Text = "Header Name(" + commcount.ToString() + ")";
                if (commcount == cblheadername.Items.Count)
                {
                    cbheadername.Checked = true;
                }
            }
            loadsubheadername();
            loaditem();
        }
        catch (Exception ex)
        {

        }
    }

    protected void cbitemname_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cbitemname.Checked == true)
            {
                for (int i = 0; i < cblitemname.Items.Count; i++)
                {
                    cblitemname.Items[i].Selected = true;
                }
                txtitemname.Text = "Item Name(" + (cblitemname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblitemname.Items.Count; i++)
                {
                    cblitemname.Items[i].Selected = false;
                }
                txtitemname.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void cblitemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtitemname.Text = "--Select--";
            cbitemname.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cblitemname.Items.Count; i++)
            {
                if (cblitemname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtitemname.Text = "Item Name(" + commcount.ToString() + ")";
                if (commcount == cblitemname.Items.Count)
                {
                    cbitemname.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void loaditem()
    {
        try
        {
            cblitemname.Items.Clear();
            string itemheader = "";
            string subheader = "";
            for (int i = 0; i < cblheadername.Items.Count; i++)
            {
                if (cblheadername.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cblheadername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cblheadername.Items[i].Value.ToString() + "";
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
                //ds = d2.BindItemCodewithsubheaderMaster_inv(itemheader, subheader);
                string itemname = "select distinct itempk ,ItemName  from IM_ItemMaster  where ItemHeaderCode in ('" + itemheader + "') and subheader_code in ('" + subheader + "')  order by ItemName";
                ds = d2.select_method_wo_parameter(itemname, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblitemname.DataSource = ds;
                    cblitemname.DataTextField = "itemname";
                    cblitemname.DataValueField = "itempk";
                    cblitemname.DataBind();
                    if (cblitemname.Items.Count > 0)
                    {
                        for (int i = 0; i < cblitemname.Items.Count; i++)
                        {
                            cblitemname.Items[i].Selected = true;
                        }
                        cbitemname.Checked = true;
                        txtitemname.Text = "Item Name(" + cblitemname.Items.Count + ")";
                    }
                    if (cblitemname.Items.Count > 5)
                    {
                        //Panel1.Width = 300;
                        //Panel1.Height = 300;
                    }
                }
                else
                {
                    txtitemname.Text = "--Select--";
                }
            }
            else
            {
                txtitemname.Text = "--Select--";
            }
        }
        catch
        {

        }
    }
    public void loadheadername()
    {
        try
        {
            cblheadername.Items.Clear();
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
            string headerquery = "";
            if (maninvalue.Trim() != "")
            {
                headerquery = "select distinct ItemHeaderCode ,ItemHeaderName  from IM_ItemMaster where ItemHeaderCode in ('" + maninvalue + "')";
            }
            else
            {
                headerquery = "select distinct ItemHeaderCode ,ItemHeaderName  from IM_ItemMaster";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(headerquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblheadername.DataSource = ds;
                cblheadername.DataTextField = "ItemHeaderName";
                cblheadername.DataValueField = "ItemHeaderCode";
                cblheadername.DataBind();

                if (cblheadername.Items.Count > 0)
                {
                    for (int i = 0; i < cblheadername.Items.Count; i++)
                    {
                        cblheadername.Items[i].Selected = true;
                    }
                    cbheadername.Checked = true;
                    txtheadername.Text = "Header Name(" + cblheadername.Items.Count + ")";

                }
            }
            else
            {
                txtheadername.Text = "--Select--";
            }
        }
        catch
        {

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Department Stock Status Report";
            string pagename = "Inv_Dept_stockstatus_Report.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
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
                d2.printexcelreport(FpSpread1, reportname);
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

    protected void ddl_deptname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            spreaddiv1.Visible = false;
            rptprint.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    public void bindmessname()
    {
        try
        {
            ds = d2.loaddepartment(collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_deptname.DataSource = ds;
                ddl_deptname.DataTextField = "Dept_Name";
                ddl_deptname.DataValueField = "Dept_Code";
                ddl_deptname.DataBind();

                cbl_deptname.Items.Clear();
                cbl_deptname.DataSource = ds;
                cbl_deptname.DataTextField = "Dept_Name";
                cbl_deptname.DataValueField = "Dept_Code";
                cbl_deptname.DataBind();

                if (cbl_deptname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_deptname.Items.Count; i++)
                    {
                        cbl_deptname.Items[i].Selected = true;
                    }
                    txt_deptname.Text = "Department Name(" + cbl_deptname.Items.Count + ")";
                }
            }
        }
        catch
        {
        }
    }
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
            for (int i = 0; i < cblheadername.Items.Count; i++)
            {
                if (cblheadername.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cblheadername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cblheadername.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                string query = "";

                query = "select distinct t.MasterCode,t.MasterValue  from CO_MasterValues t,IM_ItemMaster i where t.MasterCode=i.subheader_code and ItemHeaderCode in ('" + itemheader + "') and CollegeCode in ('" + collegecode1 + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
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
                        cb_subheadername.Checked = true;
                    }
                    if (cbl_subheadername.Items.Count > 5)
                    {
                        Panel3.Width = 300;
                        Panel3.Height = 300;
                    }
                }
                else
                {
                    txt_subheadername.Text = "--Select--";
                    cb_subheadername.Checked = false;
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
}