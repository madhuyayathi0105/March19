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
public partial class HM_Stock_Status_Report : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string itemheadercode = "";
    string item = "";
    string itemcode = "";
    string storecode = "";
    string messcode = "";
    string deptcode = "";
    string consume = "";
    string firstdate = "";
    string seconddate = "";
    DateTime dt = new DateTime();
    DateTime dt1 = new DateTime();
    DataView dv = new DataView();
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
            CalendarExtender1.EndDate = DateTime.Now;
            CalendarExtender2.EndDate = DateTime.Now;
            if (!IsPostBack)
            {
                bindmessname();
                bind_deptname(); bindstore_chk();
                txtfrom.Attributes.Add("readOnly", "readOnly");
                txtto.Attributes.Add("readOnly", "readOnly");
                txtfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtto.Text = DateTime.Now.ToString("dd/MM/yyyy");
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Visible = false;
                txtsearchby.Visible = true;
                rb_mess.Checked = true;
                rdbclick();
                rdb_cumlative.Checked = true;
                loadheadername();
                loadsubheadername();
                loaditem();
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

            for (int i = 0; i < cbl_hos.Items.Count; i++)
            {
                if (cbl_hos.Items[i].Selected == true)
                {
                    if (messcode == "")
                    {
                        messcode = "" + cbl_hos.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        messcode = messcode + "'" + "," + "'" + cbl_hos.Items[i].Value.ToString() + "";
                    }
                }
            }

            for (int i = 0; i < cbl_storeb.Items.Count; i++)
            {
                if (cbl_storeb.Items[i].Selected == true)
                {
                    if (storecode == "")
                    {
                        storecode = "" + cbl_storeb.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        storecode = storecode + "'" + "," + "'" + cbl_storeb.Items[i].Value.ToString() + "";
                    }
                }
            }

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
            item = ddl_messname.SelectedItem.Value.ToString();
            if (itemcode.Trim() != "" && itemheadercode.Trim() != "")
            {
                string selectquery = "";
                string Status = string.Empty;//Added By Saranyadevi 13.3.2018
                double balqty = 0;
                double rbu = 0;
                double amount = 0;



                firstdate = Convert.ToString(txtfrom.Text);
                seconddate = Convert.ToString(txtto.Text);

                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                if (rdb_cumlative.Checked == true)
                {
                    #region cumlative
                    FpSpread1.Width = 750;
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].AutoPostBack = true;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 10;
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
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Header";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[1].Width = 180;
                    FpSpread1.Columns[1].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[2].Width = 150;
                    FpSpread1.Columns[2].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[3].Width = 250;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Hands on Quantity";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[4].Width = 150;
                    FpSpread1.Columns[4].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Used Quantity";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[5].Width = 150;
                    FpSpread1.Columns[5].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Rpu";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[6].Width = 150;
                    FpSpread1.Columns[6].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Balance Quantity";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[7].Width = 150;
                    FpSpread1.Columns[6].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Status";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[8].Width = 170;
                    FpSpread1.Columns[6].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Amount";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[9].Width = 150;
                    FpSpread1.Columns[9].Locked = true;
                    if (rb_mess.Checked == true)
                    {
                        #region Mess
                        if (txtsearchby.Text.Trim() != "")
                        {
                            selectquery = "select distinct i.ItemPK ,i.ItemCode,i.ItemName +'('+i.ItemUnit+')' as ItemName ,SUM( BalQty) as BalQty,m.MessMasterPK,m.MessName,i.ItemHeaderName,s.IssuedRPU  from HM_MessMaster m, IT_StockDeptDetail s,IM_ItemMaster i where i.ItemPK=s.ItemFK  and m.MessMasterPK = s.DeptFK and MessMasterPK in('" + messcode + "') and i.ItemName = '" + txtsearchby.Text + "'  and  ISNULL( BalQty,0) <>0 group by i.ItemPK,i.ItemName, m.MessMasterPK,m.MessName,i.ItemCode,i.ItemHeaderName,i.ItemUnit,s.IssuedRPU";
                            selectquery = selectquery + " select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK  and ItemFK=ItemPK and i.ItemName = '" + txtsearchby.Text + "' and dm.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ForMess<>'2' and dm.MessMasterFK in('" + messcode + "') group by ItemFK ";
                        }
                        else if (txtsearchitemcode.Text.Trim() != "")
                        {
                            selectquery = " select i.ItemPK,i.ItemName +'('+i.ItemUnit+')' as ItemName ,i.ItemCode,MessMasterPK,MessName,SUM( BalQty) as BalQty,i.ItemHeaderName,s.IssuedRPU  from IT_StockDeptDetail s,IM_ItemMaster i,HM_MessMaster m where i.ItemPK=s.ItemFK and s.DeptFK = m.MessMasterPK and MessMasterPK in('" + messcode + "') and   ItemCode='" + txtsearchitemcode.Text + "' and  ISNULL( BalQty,0) <>0 group by i.ItemPK,i.ItemName,i.ItemCode,m.MessMasterPK, m.MessName,i.ItemHeaderName,i.ItemUnit,s.IssuedRPU";
                            selectquery = selectquery + " select sum(ConsumptionQty)as ConsumptionQty,ItemFK from HT_DailyConsumptionMaster M, HT_DailyConsumptionDetail D,IM_ItemMaster i where m.DailyConsumptionMasterPK =d.DailyConsumptionMasterFK  and ItemFK=ItemPK and i.ItemCode='" + txtsearchitemcode.Text + "' and M.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and m.MessMasterFK in('" + messcode + "') and ForMess<>'2' group by ItemFK";
                        }
                        else if (txtsearchheadername.Text.Trim() != "")
                        {
                            selectquery = "select distinct i.ItemPK ,i.ItemName +'('+i.ItemUnit+')' as ItemName  ,i.ItemCode,SUM( BalQty) as BalQty  , m.MessMasterPK,m.MessName,i.ItemHeaderName,s.IssuedRPU  from HM_MessMaster m,IT_StockDeptDetail s,IM_ItemMaster i where s.DeptFK = m.MessMasterPK and i.ItemPK=s.ItemFK and i.itemheadername ='" + txtsearchheadername.Text + "' and m.MessMasterPK in('" + messcode + "') group by i.ItemPK ,i.ItemName , m.MessMasterPK,m.MessName,i.ItemCode,i.ItemHeaderName,i.ItemUnit,s.IssuedRPU";
                            selectquery = selectquery + " select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and ItemFK=ItemPK and DailyConsDate  between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.itemheadername = '" + txtsearchheadername.Text + "' and dm.MessMasterFK in('" + messcode + "') and ForMess<>'2' group by ItemFK ";
                        }
                        else
                        {
                            selectquery = "select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName ,sum(isnull(BalQty,0))as BalQty, m.MessMasterPK,m.MessName,i.ItemHeaderName,s.IssuedRPU  from HM_MessMaster m, IT_StockDeptDetail s,IM_ItemMaster i where s.DeptFK = m.MessMasterPK and i.ItemPK=s.ItemFK and m.MessMasterPK in('" + messcode + "') and  i.itempk in ('" + itemcode + "') and itemheadercode in ('" + itemheadercode + "')  and  ISNULL( BalQty,0) <>0  group by ItemPK,ItemCode,ItemName,MessMasterPK,MessName,i.ItemHeaderName,i.ItemUnit,s.IssuedRPU order by i.ItemPK";
                            selectquery = selectquery + "  select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and i.ItemPK =dd.ItemFK and dm.DailyConsDate  between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.itempk  in ('" + itemcode + "')and dm.MessMasterFK in('" + messcode + "')  and ForMess<>'2' group by ItemFK";
                        }
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (cbl_hos.Items.Count > 0)
                            {
                                for (int i = 0; i < cbl_hos.Items.Count; i++)
                                {
                                    if (cbl_hos.Items[i].Selected == true)
                                    {
                                        ds.Tables[0].DefaultView.RowFilter = "MessMasterPK='" + Convert.ToString(cbl_hos.Items[i].Value) + "'";
                                        DataView dv1 = ds.Tables[0].DefaultView;
                                        if (dv1.Count > 0)
                                        {
                                            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv1[0]["MessName"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 10);
                                            for (int row = 0; row < dv1.Count; row++)
                                            {
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv1[row]["ItemHeaderName"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv1[row]["ItemCode"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv1[row]["itemname"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                double handquery = Math.Round(Convert.ToDouble(dv1[row]["BalQty"]), 2);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(handquery);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                string valu1 = Txt_Quantity.Text.ToString();
                                                if (valu1.Trim() != "")
                                                {
                                                    double valu = Convert.ToDouble(valu1.ToString());
                                                    if (valu >= handquery)
                                                    {
                                                        FpSpread1.Rows[FpSpread1.Sheets[0].RowCount - 1].ForeColor = Color.Red;
                                                    }
                                                }

                                                if (ds.Tables[1].Rows.Count > 0)
                                                {
                                                    ds.Tables[1].DefaultView.RowFilter = "ItemFK='" + Convert.ToString(dv1[row]["ItemPK"]) + "'";
                                                    dv = ds.Tables[1].DefaultView;
                                                    if (dv.Count > 0)
                                                    {
                                                        consume = Convert.ToString(dv[0]["ConsumptionQty"]);
                                                    }
                                                    else
                                                    {
                                                        consume = "0";
                                                    }
                                                }
                                                else
                                                {
                                                    consume = "0";
                                                }
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(consume);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                                //Added By Saranyadevi 13.3.2018

                                                rbu = Math.Round(Convert.ToDouble(dv1[row]["IssuedRPU"]), 2);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(rbu);

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                                balqty = Math.Round(Convert.ToDouble(handquery) - Convert.ToDouble(consume), 2);

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(balqty);

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                                if (balqty == 0.00)
                                                {
                                                    Status = "Used";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Status;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                                }
                                                else
                                                {

                                                    Status = "Balance";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Status;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                                }

                                                amount = rbu * balqty;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(amount);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                                                //End By Saranyadevi 13.3.2018
                                            }
                                        }
                                    }
                                }
                                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                lblerror.Visible = false;
                                FpSpread1.Visible = true;
                                //spreaddiv1.Visible = true;
                                rptprint.Visible = true;
                            }
                            else
                            {
                                lblerror.Visible = true;
                                lblerror.Text = "No Record Found";
                                FpSpread1.Visible = false;
                                //spreaddiv1.Visible = false;
                                rptprint.Visible = false;
                            }
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "No Record Found";
                            FpSpread1.Visible = false;
                            //spreaddiv1.Visible = false;
                            rptprint.Visible = false;
                        }
                        #endregion
                    }
                    else if (rb_store.Checked == true)
                    {
                        #region store
                        if (txtsearchby.Text.Trim() != "")
                        {
                            selectquery = " select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName  ,sum(isnull(BalQty,0))as BalQty, sm.StorePK,sm.StoreName,i.ItemHeaderName,s.InwardRPU  from IM_StoreMaster sm, IT_StockDetail s,IM_ItemMaster i where s.StoreFK  = sm.StorePK  and i.ItemPK=s.ItemFK and sm.StorePK  in('" + storecode + "') and i.ItemName ='" + txtsearchby.Text + "'  and  ISNULL( BalQty,0) <>0 group by ItemPK,ItemCode,ItemName,StorePK,StoreName ,i.ItemHeaderName,i.ItemUnit, s.InwardRPU order by i.ItemPK  ";
                            selectquery = selectquery + " select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK  and ItemFK=ItemPK and i.ItemName = '" + txtsearchby.Text + "' and dm.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by ItemFK,ConsumptionQty ";
                        }
                        else if (txtsearchitemcode.Text.Trim() != "")
                        {
                            selectquery = " select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName  ,sum(isnull(BalQty,0))as BalQty, sm.StorePK,sm.StoreName,i.ItemHeaderName, s.InwardRPU from IM_StoreMaster sm, IT_StockDetail s,IM_ItemMaster i where s.StoreFK  = sm.StorePK  and i.ItemPK=s.ItemFK and sm.StorePK  in('" + storecode + "') and i.ItemCode ='" + txtsearchitemcode.Text + "'  and  ISNULL( BalQty,0) <>0 group by ItemPK,ItemCode,ItemName,StorePK,StoreName,i.ItemHeaderName,i.ItemUnit,s.InwardRPU order by i.ItemPK  ";
                            selectquery = selectquery + " select sum(ConsumptionQty)as ConsumptionQty,ItemFK from HT_DailyConsumptionMaster M, HT_DailyConsumptionDetail D,IM_ItemMaster i where m.DailyConsumptionMasterPK =d.DailyConsumptionMasterFK  and ItemFK=ItemPK and i.ItemCode='" + txtsearchitemcode.Text + "' and M.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by ItemFK";
                        }
                        else if (txtsearchheadername.Text.Trim() != "")
                        {
                            selectquery = " select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName  ,sum(isnull(BalQty,0))as BalQty, sm.StorePK,sm.StoreName,i.ItemHeaderName,s.InwardRPU from IM_StoreMaster sm, IT_StockDetail s,IM_ItemMaster i where s.StoreFK  = sm.StorePK  and i.ItemPK=s.ItemFK and sm.StorePK  in('" + storecode + "') and i.itemheadername ='" + txtsearchheadername.Text + "'  and  ISNULL( BalQty,0) <>0 group by ItemPK,ItemCode,ItemName,StorePK,StoreName,i.ItemHeaderName,i.ItemUnit,s.InwardRPU order by i.ItemPK  ";
                            selectquery = selectquery + " select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and ItemFK=ItemPK and DailyConsDate  between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.itemheadername = '" + txtsearchheadername.Text + "' group by ItemFK,ConsumptionQty ";
                        }
                        else
                        {
                            selectquery = " select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName  ,sum(isnull(BalQty,0))as BalQty, sm.StorePK ,sm.StoreName,i.ItemHeaderName,s.InwardRPU from IM_StoreMaster sm, IT_StockDetail s,IM_ItemMaster i where s.StoreFK  = sm.StorePK  and i.ItemPK=s.ItemFK and sm.StorePK  in('" + storecode + "') and  i.itempk in ('" + itemcode + "') and itemheadercode in ('" + itemheadercode + "')  and  ISNULL( BalQty,0) <>0 group by ItemPK,ItemCode,ItemName,StorePK,StoreName,i.ItemHeaderName,i.ItemUnit,s.InwardRPU order by i.ItemPK  ";
                            selectquery = selectquery + "  select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and i.ItemPK =dd.ItemFK and dm.DailyConsDate  between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.itempk  in ('" + itemcode + "')  group by ItemFK";
                        }
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (cbl_storeb.Items.Count > 0)
                            {
                                for (int i = 0; i < cbl_storeb.Items.Count; i++)
                                {
                                    if (cbl_storeb.Items[i].Selected == true)
                                    {
                                        ds.Tables[0].DefaultView.RowFilter = "StorePK='" + Convert.ToString(cbl_storeb.Items[i].Value) + "'";
                                        DataView dv1 = ds.Tables[0].DefaultView;
                                        if (dv1.Count > 0)
                                        {
                                            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv1[0]["StoreName"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 10);
                                            for (int row = 0; row < dv1.Count; row++)
                                            {
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv1[row]["ItemHeaderName"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv1[row]["ItemCode"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv1[row]["itemname"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                double handquery = Math.Round(Convert.ToDouble(dv1[row]["BalQty"]), 2);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(handquery);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                string valu1 = Txt_Quantity.Text.ToString();
                                                if (valu1.Trim() != "")
                                                {
                                                    double valu = Convert.ToDouble(valu1.ToString());
                                                    if (valu >= handquery)
                                                    {
                                                        FpSpread1.Rows[FpSpread1.Sheets[0].RowCount - 1].ForeColor = Color.Red;
                                                    }
                                                }
                                                string consume = "";
                                                if (ds.Tables[1].Rows.Count > 0)
                                                {
                                                    ds.Tables[1].DefaultView.RowFilter = "ItemFK='" + Convert.ToString(dv1[row]["ItemPK"]) + "'";
                                                    dv = ds.Tables[1].DefaultView;
                                                    if (dv.Count > 0)
                                                    {
                                                        consume = Convert.ToString(dv[0]["ConsumptionQty"]);
                                                    }
                                                    else
                                                    {
                                                        consume = "0";
                                                    }
                                                }
                                                else
                                                {
                                                    consume = "0";
                                                }
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(consume);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                                //Added By Saranyadevi 13.3.2018

                                                rbu = Math.Round(Convert.ToDouble(dv1[row]["InwardRPU"]), 2);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(rbu);

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                                balqty = Math.Round(Convert.ToDouble(handquery) - Convert.ToDouble(consume), 2);

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(balqty);

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                                if (balqty == 0.00)
                                                {
                                                    Status = "Used";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Status;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                                }
                                                else
                                                {

                                                    Status = "Balance";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Status;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                                }

                                                amount = rbu * balqty;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(amount);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                                                //End By Saranyadevi 13.3.2018
                                            }
                                        }
                                    }
                                }
                                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                lblerror.Visible = false;
                                FpSpread1.Visible = true;
                                //spreaddiv1.Visible = true;
                                rptprint.Visible = true;
                                txtsearchitemcode.Text = "";
                                txtsearchby.Text = "";
                                txtsearchheadername.Text = "";
                            }
                            else
                            {
                                lblerror.Visible = true;
                                lblerror.Text = "No Record Found";
                                FpSpread1.Visible = false;
                                //spreaddiv1.Visible = false;
                                rptprint.Visible = false;
                            }
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "No Record Found";
                            FpSpread1.Visible = false;
                            //spreaddiv1.Visible = false;
                            rptprint.Visible = false;
                        }
                        #endregion
                    }
                    else if (rb_dept.Checked == true)
                    {
                        #region department
                        if (txtsearchby.Text.Trim() != "")
                        {
                            selectquery = "select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName ,sum(isnull(BalQty,0))as BalQty, d.Dept_Code,d.Dept_Name,i.ItemHeaderName,s.IssuedRPU from Department d, IT_StockDeptDetail s,IM_ItemMaster i where s.DeptFK = d.Dept_Code  and i.ItemPK=s.ItemFK and d.Dept_Code in('" + deptcode + "') and i.ItemName = '" + txtsearchby.Text + "' and  ISNULL( BalQty,0) <>0 group by ItemPK,ItemCode,ItemName,d.Dept_Code ,d.Dept_Name,i.ItemHeaderName,i.ItemUnit,s.IssuedRPU  order by i.ItemPK ";
                            selectquery = selectquery + " select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK  and ItemFK=ItemPK and i.ItemName = '" + txtsearchby.Text + "' and dm.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by ItemFK,ConsumptionQty ";
                        }
                        else if (txtsearchitemcode.Text.Trim() != "")
                        {
                            selectquery = "select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName  ,sum(isnull(BalQty,0))as BalQty, d.Dept_Code,d.Dept_Name,i.ItemHeaderName,s.IssuedRPU from Department d, IT_StockDeptDetail s,IM_ItemMaster i where s.DeptFK = d.Dept_Code  and i.ItemPK=s.ItemFK and d.Dept_Code in('" + deptcode + "') and  ItemCode='" + txtsearchitemcode.Text + "'  and  ISNULL( BalQty,0) <>0 group by ItemPK,ItemCode,ItemName,d.Dept_Code ,d.Dept_Name,i.ItemHeaderName,i.ItemUnit,s.IssuedRPU  order by i.ItemPK ";
                            selectquery = selectquery + " select sum(ConsumptionQty)as ConsumptionQty,ItemFK from HT_DailyConsumptionMaster M, HT_DailyConsumptionDetail D,IM_ItemMaster i where m.DailyConsumptionMasterPK =d.DailyConsumptionMasterFK  and ItemFK=ItemPK and i.ItemCode='" + txtsearchitemcode.Text + "' and M.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by ItemFK";
                        }
                        else if (txtsearchheadername.Text.Trim() != "")
                        {
                            selectquery = "select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName  ,sum(isnull(BalQty,0))as BalQty, d.Dept_Code,d.Dept_Name,i.ItemHeaderName,s.IssuedRPU from Department d, IT_StockDeptDetail s,IM_ItemMaster i where s.DeptFK = d.Dept_Code  and i.ItemPK=s.ItemFK and d.Dept_Code in('" + deptcode + "') and i.itemheadername ='" + txtsearchheadername.Text + "'  and  ISNULL( BalQty,0) <>0 group by ItemPK,ItemCode,ItemName,d.Dept_Code ,d.Dept_Name,i.ItemHeaderName,i.ItemUnit,s.IssuedRPU  order by i.ItemPK ";
                            selectquery = selectquery + " select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and ItemFK=ItemPK and DailyConsDate  between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.itemheadername = '" + txtsearchheadername.Text + "' group by ItemFK,ConsumptionQty ";
                        }
                        else
                        {
                            selectquery = "select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName  ,sum(isnull(BalQty,0))as BalQty, d.Dept_Code,d.Dept_Name,i.ItemHeaderName,s.IssuedRPU from Department d, IT_StockDeptDetail s,IM_ItemMaster i where s.DeptFK = d.Dept_Code  and i.ItemPK=s.ItemFK and d.Dept_Code in('" + deptcode + "') and  i.ItemPK in ('" + itemcode + "') and itemheadercode in ('" + itemheadercode + "')   and  ISNULL( BalQty,0) <>0  group by ItemPK,ItemCode,ItemName,d.Dept_Code ,d.Dept_Name,i.ItemHeaderName,i.ItemUnit,s.IssuedRPU  order by i.ItemPK ";
                            selectquery = selectquery + "  select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and i.ItemPK =dd.ItemFK and dm.DailyConsDate  between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.ItemPK  in ('" + itemcode + "')  group by ItemFK";
                        }
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (cbl_deptname.Items.Count > 0)
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
                                            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 10);
                                            for (int row = 0; row < dv1.Count; row++)
                                            {
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv1[row]["ItemHeaderName"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv1[row]["ItemCode"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv1[row]["itemname"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                double handquery = Math.Round(Convert.ToDouble(dv1[row]["BalQty"]), 2);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(handquery);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                string valu1 = Txt_Quantity.Text.ToString();
                                                if (valu1.Trim() != "")
                                                {
                                                    double valu = Convert.ToDouble(valu1.ToString());
                                                    if (valu >= handquery)
                                                    {
                                                        FpSpread1.Rows[FpSpread1.Sheets[0].RowCount - 1].ForeColor = Color.Red;
                                                    }
                                                }
                                                string consume = "";
                                                if (ds.Tables[1].Rows.Count > 0)
                                                {
                                                    ds.Tables[1].DefaultView.RowFilter = "ItemFK='" + Convert.ToString(dv1[row]["ItemPK"]) + "'";
                                                    dv = ds.Tables[1].DefaultView;
                                                    if (dv.Count > 0)
                                                    {
                                                        consume = Convert.ToString(dv[0]["ConsumptionQty"]);
                                                    }
                                                    else
                                                    {
                                                        consume = "0";
                                                    }
                                                }
                                                else
                                                {
                                                    consume = "0";
                                                }
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(consume);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";


                                                //Added By Saranyadevi 13.3.2018

                                                rbu = Math.Round(Convert.ToDouble(dv1[row]["IssuedRPU"]), 2);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(rbu);

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                                balqty = Math.Round(Convert.ToDouble(handquery) - Convert.ToDouble(consume), 2);

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(balqty);

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                                if (balqty == 0.00)
                                                {
                                                    Status = "Used";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Status;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                                }
                                                else
                                                {

                                                    Status = "Balance";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Status;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                                }

                                                amount = rbu * balqty;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(amount);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                                                //End By Saranyadevi 13.3.2018
                                            }
                                        }
                                    }
                                }
                                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                lblerror.Visible = false;
                                FpSpread1.Visible = true;
                                //spreaddiv1.Visible = true;
                                rptprint.Visible = true;
                            }
                            else
                            {
                                lblerror.Visible = true;
                                lblerror.Text = "No Record Found";
                                FpSpread1.Visible = false;
                                //spreaddiv1.Visible = false;
                                rptprint.Visible = false;
                            }
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "No Record Found";
                            FpSpread1.Visible = false;
                            //spreaddiv1.Visible = false;
                            rptprint.Visible = false;
                        }
                        #endregion
                    }
                    #endregion
                }
                if (rdb_details.Checked == true)
                {
                    #region Details
                    FpSpread1.Width = 800;
                    Fpreadheaderbindmethod("SNo/Item Name/Opening Qty/Used Qty/Received Qty/Closing Qty/Average RPU", FpSpread1, "true");
                    string q1 = "";
                    if (rb_mess.Checked == true)
                    {
                        q1 = " select ItemFK,ItemName,SUM(TransferQty)As openingqty from IT_TransferItem t,IM_ItemMaster i where t.ItemFK=i.ItemPK and TrasnferDate <'" + dt.ToString("MM/dd/yyyy") + "'  and TrasferTo in( '" + messcode + "') group by ItemFK,ItemName ";
                        q1 = q1 + " select ItemFK,avg(RPU)RPU,isnull(SUM(ConsumptionQty),'0')  Used_qty  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd where dm.DailyConsumptionMasterPK=dd.DailyConsumptionMasterFK and DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ForMess='1' and dm.MessMasterFK in('" + messcode + "') group by ItemFK";
                        q1 = q1 + " select ItemFK,SUM(TransferQty)As purchaseqty from IT_TransferItem t,IM_ItemMaster i where t.ItemFK=i.ItemPK and  TrasferTo in('" + messcode + "') and TrasnferDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by ItemFK,ItemName ";
                        q1 = q1 + " select distinct ItemFK,RPU from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd where dm.DailyConsumptionMasterPK= dd.DailyConsumptionMasterFK and DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ForMess='1' and dm.deptfk in('" + messcode + "')";
                    }
                    else if (rb_store.Checked == true)
                    {
                        q1 = " select ItemFK,ItemName,SUM(TransferQty)As openingqty from IT_TransferItem t,IM_ItemMaster i where t.ItemFK=i.ItemPK and TrasnferDate <'" + dt.ToString("MM/dd/yyyy") + "'  and TrasferTo in( '" + storecode + "') group by ItemFK,ItemName ";
                        q1 = q1 + " select ItemFK,avg(RPU)RPU,isnull(SUM(ConsumptionQty),'0')  Used_qty  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd where dm.DailyConsumptionMasterPK=dd.DailyConsumptionMasterFK and DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ForMess='2' and dm.deptfk in('" + storecode + "') group by ItemFK";
                        q1 = q1 + " select ItemFK,SUM(TransferQty)As purchaseqty from IT_TransferItem t,IM_ItemMaster i where t.ItemFK=i.ItemPK and  TrasferTo in('" + storecode + "') and TrasnferDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by ItemFK,ItemName ";
                        q1 = q1 + " select distinct ItemFK,RPU from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd where dm.DailyConsumptionMasterPK= dd.DailyConsumptionMasterFK and DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ForMess='2' and dm.deptfk in('" + storecode + "')";


                    }
                    else 
                    {
                        q1 = " select ItemFK,ItemName,SUM(TransferQty)As openingqty from IT_TransferItem t,IM_ItemMaster i where t.ItemFK=i.ItemPK and TrasnferDate <'" + dt.ToString("MM/dd/yyyy") + "'  and TrasferTo in( '" + deptcode + "') group by ItemFK,ItemName ";
                        q1 = q1 + " select ItemFK,avg(RPU)RPU,isnull(SUM(ConsumptionQty),'0')  Used_qty  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd where dm.DailyConsumptionMasterPK=dd.DailyConsumptionMasterFK and DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ForMess='3' and dm.deptfk in('" + deptcode + "') group by ItemFK";
                        q1 = q1 + " select ItemFK,SUM(TransferQty)As purchaseqty from IT_TransferItem t,IM_ItemMaster i where t.ItemFK=i.ItemPK and  TrasferTo in('" + deptcode + "') and TrasnferDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by ItemFK,ItemName ";
                        q1 = q1 + " select distinct ItemFK,RPU from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd where dm.DailyConsumptionMasterPK= dd.DailyConsumptionMasterFK and DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ForMess='3' and dm.deptfk in('" + deptcode + "')";
                    
                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "Text");
                    DataView dv1 = new DataView();
                    DataView dnew = new DataView();
                    DataView dnew1 = new DataView();
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    FpSpread1.Sheets[0].Columns[6].Visible = false;
                    if (cblitemname.Items.Count > 0)
                    {
                        for (int i = 0; i < cblitemname.Items.Count; i++)
                        {
                            string usededqty = ""; string purchaseqty = ""; string openningqty = "";
                            if (cblitemname.Items[i].Selected == true)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(cblitemname.Items[i].Text);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                ds.Tables[0].DefaultView.RowFilter = "ItemFK='" + Convert.ToString(cblitemname.Items[i].Value) + "'";
                                dnew1 = ds.Tables[0].DefaultView;
                                if (dnew1.Count > 0)
                                {
                                    openningqty = Convert.ToString(dnew1[0]["openingqty"]);
                                    if (openningqty.Trim() != "")
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(openningqty);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "-";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "-";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                }
                                ds.Tables[1].DefaultView.RowFilter = "ItemFK='" + Convert.ToString(cblitemname.Items[i].Value) + "'";
                                dnew = ds.Tables[1].DefaultView;
                                if (dnew.Count > 0)
                                {
                                    usededqty = Convert.ToString(dnew[0]["Used_qty"]);
                                    if (usededqty.Trim() != "")
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(usededqty);
                                        ds.Tables[3].DefaultView.RowFilter = "ItemFK='" + Convert.ToString(cblitemname.Items[i].Value) + "'";
                                        DataView rpuDV = ds.Tables[3].DefaultView;
                                        double AvgRpu = 0;
                                        if (rpuDV.Count > 0)
                                        {
                                            double.TryParse(Convert.ToString(rpuDV.Table.Compute("Sum(rpu)", "ItemFK='" + Convert.ToString(cblitemname.Items[i].Value) + "'")), out AvgRpu);
                                            AvgRpu = AvgRpu / rpuDV.Count;
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(Math.Round(AvgRpu, 2));
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = "-";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "-";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = "-";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "-";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                }
                                ds.Tables[2].DefaultView.RowFilter = "ItemFK='" + Convert.ToString(cblitemname.Items[i].Value) + "'";
                                dv1 = ds.Tables[2].DefaultView;
                                if (dv1.Count > 0)
                                {
                                    purchaseqty = Convert.ToString(dv1[0]["purchaseqty"]);
                                    if (purchaseqty.Trim() != "")
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(purchaseqty);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "-";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "-";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                }
                                string closingqty = "";
                                if (usededqty.Trim() == "")
                                    usededqty = "0";
                                if (openningqty.Trim() == "")
                                    openningqty = "0";
                                if (closingqty.Trim() == "")
                                    closingqty = "0";
                                if (purchaseqty.Trim() == "")
                                    purchaseqty = "0";
                                double closingbal = (Convert.ToDouble(openningqty) + (Convert.ToDouble(purchaseqty))) - Convert.ToDouble(usededqty);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(closingbal);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                            }
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        lblerror.Visible = false;
                        FpSpread1.Visible = true;
                        rptprint.Visible = true;
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "No Records Found";
                        FpSpread1.Visible = false;
                        rptprint.Visible = false;
                    }
                    #endregion
                }
                if (rdb_datewise.Checked == true)
                {
                    #region Datewise
                    Item_wise_Consumption_Report();
                    #endregion
                }
                if (FpSpread1.Sheets[0].ColumnCount > 0)
                {
                    for (int m = 0; m < FpSpread1.Sheets[0].ColumnCount; m++)
                    {
                        FpSpread1.Columns[m].Locked = true;
                    }
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select All Field";
                FpSpread1.Visible = false;
                rptprint.Visible = false;
            }
            txtsearchitemcode.Text = "";
            txtsearchby.Text = "";
            txtsearchheadername.Text = "";
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
            //if (itemheader.Trim() != "")
            //{
            //    string itemselectquery = "select distinct item_code  ,item_name   from item_master  where itemheader_code in ('" + itemheader + "')";
            //    ds.Clear();
            //    ds = d2.select_method_wo_parameter(itemselectquery, "Text");
            //    if (ds.Tables[0].Rows.Count > 0)
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
            string messcode = "";
            for (int i = 0; i < cbl_hos.Items.Count; i++)
            {
                if (cbl_hos.Items[i].Selected == true)
                {
                    if (messcode == "")
                    {
                        messcode = "" + cbl_hos.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        messcode = messcode + "'" + "," + "'" + cbl_hos.Items[i].Value.ToString() + "";
                    }
                }
            }
            string storecode = "";
            for (int i = 0; i < cbl_storeb.Items.Count; i++)
            {
                if (cbl_storeb.Items[i].Selected == true)
                {
                    if (storecode == "")
                    {
                        storecode = "" + cbl_storeb.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        storecode = storecode + "'" + "," + "'" + cbl_storeb.Items[i].Value.ToString() + "";
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
            if (itemheader.Trim() != "" && subheader.Trim() != "")
            {
                ds.Clear();
                //ds = d2.BindItempkwithsubheader_inv(itemheader, subheader);
                // ds = d2.BindItemCodewithsubheaderMaster_inv(itemheader, subheader);
                string headerquery = "";
                if (rb_store.Checked == true)
                {
                    headerquery = " select distinct i.ItemName,i.ItemPK from IM_ItemMaster i,IT_StockDetail s where s.ItemFK=i.ItemPK  AND S.StoreFK IN('" + storecode + "') and i.ItemHeaderCode in('" + itemheader + "') and i.subheader_code in('" + subheader + "') order by ItemName ";
                }
                else if (rb_mess.Checked == true)
                {
                    headerquery = " select distinct i.ItemName,i.ItemPK from IT_StockDeptDetail s,IM_ItemMaster i where i.ItemPK=s.ItemFK and DeptFK in('" + messcode + "')  and i.ItemHeaderCode in('" + itemheader + "') and i.subheader_code in('" + subheader + "')  order by ItemName ";
                }
                else if (rb_dept.Checked == true)
                {
                    headerquery = " select distinct i.ItemName,i.ItemPK from IT_StockDeptDetail s,IM_ItemMaster i,Department d  where i.ItemPK=s.ItemFK and s.DeptFK=d.Dept_Code and DeptFK in('" + deptcode + "') and i.ItemHeaderCode in('" + itemheader + "') and i.subheader_code in('" + subheader + "')  order by ItemName ";
                }
                ds = d2.select_method_wo_parameter(headerquery, "text");
                cblitemname.Items.Clear();
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
            //ddlpopitemheadername.Items.Clear();
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
            string messcode = "";
            for (int i = 0; i < cbl_hos.Items.Count; i++)
            {
                if (cbl_hos.Items[i].Selected == true)
                {
                    if (messcode == "")
                    {
                        messcode = "" + cbl_hos.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        messcode = messcode + "'" + "," + "'" + cbl_hos.Items[i].Value.ToString() + "";
                    }
                }
            }
            string storecode = "";
            for (int i = 0; i < cbl_storeb.Items.Count; i++)
            {
                if (cbl_storeb.Items[i].Selected == true)
                {
                    if (storecode == "")
                    {
                        storecode = "" + cbl_storeb.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        storecode = storecode + "'" + "," + "'" + cbl_storeb.Items[i].Value.ToString() + "";
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
            string headerquery = "";
            if (maninvalue.Trim() != "")
            {
                headerquery = "select distinct ItemHeaderCode ,ItemHeaderName  from IM_ItemMaster where ItemHeaderCode in ('" + maninvalue + "')";
            }
            else
            {
                if (rb_store.Checked == true)
                {
                    headerquery = " select distinct i.ItemHeaderName,i.ItemHeaderCode from IM_ItemMaster i,IT_StockDetail s where s.ItemFK=i.ItemPK  AND S.StoreFK IN('" + storecode + "') ";
                }
                else if (rb_mess.Checked == true)
                {
                    headerquery = " select distinct i.ItemHeaderName,i.ItemHeaderCode from IT_StockDeptDetail s,IM_ItemMaster i where i.ItemPK=s.ItemFK and DeptFK in('" + messcode + "') order by ItemHeaderName ";
                }
                else if (rb_dept.Checked == true)
                {
                    headerquery = " select distinct i.ItemHeaderName,i.ItemHeaderCode from IT_StockDeptDetail s,IM_ItemMaster i,Department d where i.ItemPK=s.ItemFK and s.DeptFK=d.Dept_Code and DeptFK in('" + deptcode + "') order by ItemHeaderName ";
                }
                // headerquery = "select distinct ItemHeaderCode ,ItemHeaderName  from IM_ItemMaster";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(headerquery, "Text");
            cblheadername.Items.Clear();
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
                cbl_subheadername.Items.Clear();
                txt_subheadername.Text = "--Select--";
            }
            loadsubheadername();
            loaditem();
        }
        catch
        {
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Stock Status Report";
            string pagename = "HM_Stock_Status_Report.aspx";
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
    protected void ddl_messname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
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
            ds.Clear();
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            //string deptquery = "select  MessMasterPK,MessName  from HM_MessMaster where CollegeCode ='" + collegecode1 + "' order by MessMasterPK ";
            //ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_messname.DataSource = ds;
                ddl_messname.DataTextField = "MessName";
                ddl_messname.DataValueField = "MessMasterPK";
                ddl_messname.DataBind();
                cbl_hos.Items.Clear();
                cbl_hos.DataSource = ds;
                cbl_hos.DataTextField = "MessName";
                cbl_hos.DataValueField = "MessMasterPK";
                cbl_hos.DataBind();
                if (cbl_hos.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hos.Items.Count; i++)
                    {
                        cbl_hos.Items[i].Selected = true;
                    }
                    txt_hosname.Text = "Mess Name(" + cbl_hos.Items.Count + ")";
                }
            }
            else
            {
                txt_hosname.Text = "--Select--";
            }
            ddl_messname.Items.Insert(0, "Select");
        }
        catch
        {
        }
    }
    protected void cb_hostel_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_hosname.Text = "---Select---";
        if (cb_hos.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_hos.Items.Count; i++)
            {
                cbl_hos.Items[i].Selected = true;
            }
            txt_hosname.Text = "Mess Name(" + (cbl_hos.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hos.Items.Count; i++)
            {
                cbl_hos.Items[i].Selected = false;
            }
        }
        loadheadername();
        loaditem();
    }
    protected void cbl_hostel_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_hos.Checked = false;
        int commcount = 0;
        txt_hosname.Text = "--Select--";
        for (i = 0; i < cbl_hos.Items.Count; i++)
        {
            if (cbl_hos.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_hos.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_hos.Items.Count)
            {
                cb_hos.Checked = true;
            }
            txt_hosname.Text = "Mess Name(" + commcount.ToString() + ")";
        }
        loadheadername();
        loaditem();
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
                //query = "select distinct t.TextCode,t.TextVal  from TextValTable t,item_master i where t.TextCode=i.subheader_code and itemheader_code in ('" + itemheader + "') and college_code in ('" + collegecode1 + "')";
                //ds.Clear();
                //ds = d2.select_method_wo_parameter(query, "Text");
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
                // cb_subheadername.Checked = false;
            }
        }
        catch
        {
        }
    }
    protected void rdbclick()
    {
        if (rb_mess.Checked == true)
        {
            lbl_messname.Text = "Mess Name";
            txt_hosname.Visible = true;
            p1.Visible = true;
            Panel2.Visible = false;
            txt_store.Visible = false;
            txt_deptname.Visible = false;
            p6.Visible = false;
        }
        else if (rb_store.Checked == true)
        {
            lbl_messname.Text = "Store Name";
            Panel2.Visible = true;
            txt_store.Visible = true;
            txt_hosname.Visible = false;
            p1.Visible = false;
            txt_deptname.Visible = false;
            p6.Visible = false;
        }
        else if (rb_dept.Checked == true)
        {
            lbl_messname.Text = "Department";
            txt_deptname.Visible = true;
            p6.Visible = true;
            txt_hosname.Visible = false;
            p1.Visible = false;
            Panel2.Visible = false;
            txt_store.Visible = false;
        }
    }
    protected void rb_store_OnCheckedChanged(object sender, EventArgs e)
    {
        rdbclick();
        //spreaddiv1.Visible = false;
        FpSpread1.Visible = false;
        rptprint.Visible = false;
        loadheadername();
    }
    protected void rb_mess_OnCheckedChanged(object sender, EventArgs e)
    {
        //spreaddiv1.Visible = false;
        FpSpread1.Visible = false;
        rptprint.Visible = false;
        rdbclick();
        loadheadername();
    }
    protected void rb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        rdbclick();
        //spreaddiv1.Visible = false;
        rptprint.Visible = false;
        FpSpread1.Visible = false;
        loadheadername();
    }
    protected void cb_storeb_oncheckedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_storeb.Checked == true)
            {
                for (int i = 0; i < cbl_storeb.Items.Count; i++)
                {
                    cbl_storeb.Items[i].Selected = true;
                }
                txt_store.Text = "Store Name(" + (cbl_storeb.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_storeb.Items.Count; i++)
                {
                    cbl_storeb.Items[i].Selected = false;
                }
                txt_store.Text = "--Select--";
            }
            loadheadername();
            loaditem();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_storeb_onselectedindexchange(object sender, EventArgs e)
    {
        try
        {
            txt_store.Text = "--Select--";
            cb_storeb.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_storeb.Items.Count; i++)
            {
                if (cbl_storeb.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_store.Text = "Store Name(" + commcount.ToString() + ")";
                if (commcount == cbl_storeb.Items.Count)
                {
                    cb_storeb.Checked = true;
                }
            }
            loadheadername();
            loaditem();
        }
        catch (Exception ex)
        {
        }
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
            loadheadername();
            loaditem();
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
            loadheadername();
            loaditem();
        }
        catch (Exception ex)
        {
        }
    }
    protected void bind_deptname()
    {
        try
        {
            string deptquery = "select Dept_Code ,Dept_Name  from Department where college_code ='" + collegecode1 + "' order by Dept_Code";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
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
                    txt_deptname.Text = "Department(" + cbl_deptname.Items.Count + ")";
                }
            }
        }
        catch
        { }
    }
    protected void bindstore_chk()
    {
        try
        {
            ds.Clear();
            // ds = d2.BindStore_inv(collegecode1);
            string storepk = d2.GetFunction("select value from Master_Settings where settings='Store Rights' and usercode='" + usercode + "'");
            ds = d2.BindStorebaseonrights_inv(storepk);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_storeb.DataSource = ds;
                cbl_storeb.DataTextField = "StoreName";
                cbl_storeb.DataValueField = "StorePK";
                cbl_storeb.DataBind();
                for (int i = 0; i < cbl_storeb.Items.Count; i++)
                {
                    cbl_storeb.Items[i].Selected = true;
                    txt_store.Text = "Store Name(" + (cbl_storeb.Items.Count) + ")";
                    cb_storeb.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void rdb_cumlative_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            rptprint.Visible = false;
            rb_mess.Enabled = true;
            rb_store.Enabled = true;
            rb_dept.Enabled = true;
        }
        catch { }
    }
    protected void rdb_details_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            rptprint.Visible = false;
            rb_mess.Enabled = true;
            //rb_store.Enabled = false;
            //rb_dept.Enabled = false;
            rb_store.Enabled = true;
            rb_dept.Enabled = true;
            rb_mess.Checked = true;
            rb_store.Checked = false;
            rb_dept.Checked = false;
            rb_mess_OnCheckedChanged(sender, e);
        }
        catch { }
    }
    public void Fpreadheaderbindmethod(string headername, FarPoint.Web.Spread.FpSpread spreadname, string AutoPostBack)
    {
        try
        {
            int k = 0;
            string[] header = headername.Split('/');
            if (AutoPostBack.Trim().ToUpper() == "TRUE")
            {
                if (header.Length > 0)
                {
                    spreadname.Sheets[0].RowCount = 0;
                    spreadname.Sheets[0].ColumnCount = 0;
                    spreadname.CommandBar.Visible = false;
                    spreadname.Sheets[0].AutoPostBack = true;
                    spreadname.Sheets[0].ColumnHeader.RowCount = 1;
                    spreadname.Sheets[0].RowHeader.Visible = false;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    spreadname.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    foreach (string head in header)
                    {
                        k++;
                        spreadname.Sheets[0].ColumnCount = Convert.ToInt32(header.Length);
                        if (head.Trim().ToUpper() == "S.NO")
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = head;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = 50;
                        }
                        else
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = head;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = 200;
                        }
                    }
                }
            }
            else if (AutoPostBack.Trim().ToUpper() == "FALSE")
            {
                if (header.Length > 0)
                {
                    spreadname.Sheets[0].RowCount = 0;
                    spreadname.Sheets[0].ColumnCount = 0;
                    spreadname.CommandBar.Visible = false;
                    spreadname.Sheets[0].AutoPostBack = false;
                    spreadname.Sheets[0].ColumnHeader.RowCount = 1;
                    spreadname.Sheets[0].RowHeader.Visible = false;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    spreadname.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    foreach (string head in header)
                    {
                        k++;
                        string[] width = head.Split('-');
                        spreadname.Sheets[0].ColumnCount = Convert.ToInt32(header.Length);
                        if (Convert.ToString(width[0]).Trim().ToUpper() == "S.NO")
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = Convert.ToString(width[0]);
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = Convert.ToInt32(width[1]);
                        }
                        else
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = Convert.ToString(width[0]);
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = Convert.ToInt32(width[1]);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Font.Size = FontUnit.Smaller;
            lblerror.Text = ex.ToString();
        }
    }

    //Added By SaranyaDevi 20.3.2018
    protected void rdb_datewise_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            rptprint.Visible = false;
            rb_mess.Enabled = true;
            rb_store.Enabled = true;
            rb_dept.Enabled = true;
        }
        catch
        {
        }

    }


    #region Item_wise_Consumption_Report
    protected void Item_wise_Consumption_Report()
    {
        try
        {
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

            for (int i = 0; i < cbl_hos.Items.Count; i++)
            {
                if (cbl_hos.Items[i].Selected == true)
                {
                    if (messcode == "")
                    {
                        messcode = "" + cbl_hos.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        messcode = messcode + "'" + "," + "'" + cbl_hos.Items[i].Value.ToString() + "";
                    }
                }
            }

            for (int i = 0; i < cbl_storeb.Items.Count; i++)
            {
                if (cbl_storeb.Items[i].Selected == true)
                {
                    if (storecode == "")
                    {
                        storecode = "" + cbl_storeb.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        storecode = storecode + "'" + "," + "'" + cbl_storeb.Items[i].Value.ToString() + "";
                    }
                }
            }

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
            item = ddl_messname.SelectedItem.Value.ToString();
            if (itemcode.Trim() != "" && itemheadercode.Trim() != "" && messcode.Trim() != "")
            {
                string selectquery = "";
                double conrbu = 0;
                int spanrow = 0;
                int spanrow1 = 0;
                int spanrow2 = 0;
                firstdate = Convert.ToString(txtfrom.Text);
                seconddate = Convert.ToString(txtto.Text);
                string date = "";
                string[] split1 = firstdate.Split('/');
                dt = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
                split1 = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
                if (rdb_datewise.Checked == true)
                {
                    FpSpread1.Width = 750;
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].AutoPostBack = true;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 7;
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
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Header";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[1].Width = 180;
                    FpSpread1.Columns[1].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[2].Width = 150;
                    FpSpread1.Columns[2].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[3].Width = 250;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Date";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[4].Width = 150;
                    FpSpread1.Columns[4].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Consumption Quantity";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[5].Width = 150;
                    FpSpread1.Columns[5].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Rpu";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[6].Width = 150;
                    FpSpread1.Columns[6].Locked = true;
                    if (rb_mess.Checked == true)
                    {
                        #region Mess
                        if (txtsearchby.Text.Trim() != "")
                        {
                            selectquery = "select distinct i.ItemPK ,i.ItemCode,i.ItemName +'('+i.ItemUnit+')' as ItemName ,SUM( BalQty) as BalQty,m.MessMasterPK,m.MessName,i.ItemHeaderName,s.IssuedRPU  from HM_MessMaster m, IT_StockDeptDetail s,IM_ItemMaster i where i.ItemPK=s.ItemFK  and m.MessMasterPK = s.DeptFK and MessMasterPK in('" + messcode + "') and i.ItemName = '" + txtsearchby.Text + "'  and  ISNULL( BalQty,0) <>0 group by i.ItemPK,i.ItemName, m.MessMasterPK,m.MessName,i.ItemCode,i.ItemHeaderName,i.ItemUnit,s.IssuedRPU";
                            selectquery = selectquery + " select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK,CONVERT(varchar(20),dm.DailyConsDate,103) DailyConsDate,dd.RPU  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK  and ItemFK=ItemPK and i.ItemName = '" + txtsearchby.Text + "' and dm.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ForMess<>'2' and dm.MessMasterFK in('" + messcode + "') group by ItemFK,dm.DailyConsDate,dd.RPU ";
                        }
                        else if (txtsearchitemcode.Text.Trim() != "")
                        {
                            selectquery = " select i.ItemPK,i.ItemName +'('+i.ItemUnit+')' as ItemName ,i.ItemCode,MessMasterPK,MessName,SUM( BalQty) as BalQty,i.ItemHeaderName,s.IssuedRPU  from IT_StockDeptDetail s,IM_ItemMaster i,HM_MessMaster m where i.ItemPK=s.ItemFK and s.DeptFK = m.MessMasterPK and MessMasterPK in('" + messcode + "') and   ItemCode='" + txtsearchitemcode.Text + "' and  ISNULL( BalQty,0) <>0 group by i.ItemPK,i.ItemName,i.ItemCode,m.MessMasterPK, m.MessName,i.ItemHeaderName,i.ItemUnit,s.IssuedRPU";
                            selectquery = selectquery + " select sum(ConsumptionQty)as ConsumptionQty,ItemFK,CONVERT(varchar(20),dm.DailyConsDate,103) DailyConsDate,dd.RPU from HT_DailyConsumptionMaster M, HT_DailyConsumptionDetail D,IM_ItemMaster i where m.DailyConsumptionMasterPK =d.DailyConsumptionMasterFK  and ItemFK=ItemPK and i.ItemCode='" + txtsearchitemcode.Text + "' and M.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and m.MessMasterFK in('" + messcode + "') and ForMess<>'2' group by ItemFK,dm.DailyConsDate,dd.RPU";
                        }
                        else if (txtsearchheadername.Text.Trim() != "")
                        {
                            selectquery = "select distinct i.ItemPK ,i.ItemName +'('+i.ItemUnit+')' as ItemName  ,i.ItemCode,SUM( BalQty) as BalQty  , m.MessMasterPK,m.MessName,i.ItemHeaderName,s.IssuedRPU  from HM_MessMaster m,IT_StockDeptDetail s,IM_ItemMaster i where s.DeptFK = m.MessMasterPK and i.ItemPK=s.ItemFK and i.itemheadername ='" + txtsearchheadername.Text + "' and m.MessMasterPK in('" + messcode + "') group by i.ItemPK ,i.ItemName , m.MessMasterPK,m.MessName,i.ItemCode,i.ItemHeaderName,i.ItemUnit,s.IssuedRPU";
                            selectquery = selectquery + " select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK,CONVERT(varchar(20),dm.DailyConsDate,103) DailyConsDate,dd.RPU  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and ItemFK=ItemPK and DailyConsDate  between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.itemheadername = '" + txtsearchheadername.Text + "' and dm.MessMasterFK in('" + messcode + "') and ForMess<>'2' group by ItemFK,dm.DailyConsDate,dd.RPU ";
                        }
                        else
                        {
                            selectquery = "select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName ,sum(isnull(BalQty,0))as BalQty, m.MessMasterPK,m.MessName,i.ItemHeaderName,s.IssuedRPU  from HM_MessMaster m, IT_StockDeptDetail s,IM_ItemMaster i where s.DeptFK = m.MessMasterPK and i.ItemPK=s.ItemFK and m.MessMasterPK in('" + messcode + "') and  i.itempk in ('" + itemcode + "') and itemheadercode in ('" + itemheadercode + "')  and  ISNULL( BalQty,0) <>0  group by ItemPK,ItemCode,ItemName,MessMasterPK,MessName,i.ItemHeaderName,i.ItemUnit,s.IssuedRPU order by i.ItemPK";
                            selectquery = selectquery + "  select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK,CONVERT(varchar(20),dm.DailyConsDate,103) DailyConsDate,dd.RPU  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and i.ItemPK =dd.ItemFK and dm.DailyConsDate  between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.itempk  in ('" + itemcode + "')and dm.MessMasterFK in('" + messcode + "')  and ForMess<>'2' group by ItemFK,dm.DailyConsDate,dd.RPU";
                        }
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (cbl_hos.Items.Count > 0)
                            {
                                for (int i = 0; i < cbl_hos.Items.Count; i++)
                                {
                                    if (cbl_hos.Items[i].Selected == true)
                                    {
                                        ds.Tables[0].DefaultView.RowFilter = "MessMasterPK='" + Convert.ToString(cbl_hos.Items[i].Value) + "'";
                                        DataView dv1 = ds.Tables[0].DefaultView;
                                        if (dv1.Count > 0)
                                        {
                                            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv1[0]["MessName"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 10);
                                            for (int row = 0; row < dv1.Count; row++)
                                            {
                                                FpSpread1.Sheets[0].RowCount++;

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv1[row]["ItemHeaderName"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv1[row]["ItemCode"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv1[row]["itemname"]);
                                               FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                if (ds.Tables[1].Rows.Count > 0)
                                                {
                                                    ds.Tables[1].DefaultView.RowFilter = "ItemFK='" + Convert.ToString(dv1[row]["ItemPK"]) + "'";
                                                    dv = ds.Tables[1].DefaultView;

                                                    if (dv.Count > 0)
                                                    {
                                                        spanrow = dv.Count;
                                                        for (int c = 0; c < dv.Count; c++)
                                                        {
                                                            consume = Convert.ToString(dv[c]["ConsumptionQty"]);
                                                            date = Convert.ToString(dv[c]["DailyConsDate"]);
                                                            conrbu = Math.Round(Convert.ToDouble(dv[c]["RPU"]), 2);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = date;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = consume;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";


                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(conrbu);

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                                            spanrow2 = FpSpread1.Sheets[0].RowCount++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        consume = "0";
                                                        date = "-";
                                                        conrbu = 0;

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = date;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = consume;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(conrbu);

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                                    }
                                                    spanrow1 = spanrow2 - spanrow;
                                                    FpSpread1.Sheets[0].SpanModel.Add(spanrow1, 0, spanrow, 1);
                                                    FpSpread1.Sheets[0].SpanModel.Add(spanrow1, 1, spanrow, 1);
                                                    FpSpread1.Sheets[0].SpanModel.Add(spanrow1, 2, spanrow, 1);
                                                    FpSpread1.Sheets[0].SpanModel.Add(spanrow1, 3, spanrow, 1);
                                                    FpSpread1.Sheets[0].RowCount--;
                                                }
                                                else
                                                {
                                                    consume = "0";
                                                    date = "-";
                                                    conrbu = 0;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = date;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = consume;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(conrbu);

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                                }
                                            }
                                        }
                                    }
                                }
                                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                lblerror.Visible = false;
                                FpSpread1.SaveChanges();
                                FpSpread1.Visible = true;
                                rptprint.Visible = true;

                            }
                            else
                            {
                                lblerror.Visible = true;
                                lblerror.Text = "No Record Found";
                                FpSpread1.Visible = false;
                                rptprint.Visible = false;
                            }
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "No Record Found";
                            FpSpread1.Visible = false;
                            rptprint.Visible = false;
                        }
                        #endregion
                    }
                    else if (rb_store.Checked == true)
                    {
                        #region store
                        if (txtsearchby.Text.Trim() != "")
                        {
                            selectquery = " select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName  ,sum(isnull(BalQty,0))as BalQty, sm.StorePK,sm.StoreName,i.ItemHeaderName,s.InwardRPU  from IM_StoreMaster sm, IT_StockDetail s,IM_ItemMaster i where s.StoreFK  = sm.StorePK  and i.ItemPK=s.ItemFK and sm.StorePK  in('" + storecode + "') and i.ItemName ='" + txtsearchby.Text + "'  and  ISNULL( BalQty,0) <>0 group by ItemPK,ItemCode,ItemName,StorePK,StoreName ,i.ItemHeaderName,i.ItemUnit, s.InwardRPU order by i.ItemPK  ";
                            selectquery = selectquery + " select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK,CONVERT(varchar(20),dm.DailyConsDate,103) DailyConsDate,dd.RPU    from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK  and ItemFK=ItemPK and i.ItemName = '" + txtsearchby.Text + "' and dm.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by ItemFK,ConsumptionQty,dm.DailyConsDate,dd.RPU  ";
                        }
                        else if (txtsearchitemcode.Text.Trim() != "")
                        {
                            selectquery = " select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName  ,sum(isnull(BalQty,0))as BalQty, sm.StorePK,sm.StoreName,i.ItemHeaderName, s.InwardRPU from IM_StoreMaster sm, IT_StockDetail s,IM_ItemMaster i where s.StoreFK  = sm.StorePK  and i.ItemPK=s.ItemFK and sm.StorePK  in('" + storecode + "') and i.ItemCode ='" + txtsearchitemcode.Text + "'  and  ISNULL( BalQty,0) <>0 group by ItemPK,ItemCode,ItemName,StorePK,StoreName,i.ItemHeaderName,i.ItemUnit,s.InwardRPU order by i.ItemPK  ";
                            selectquery = selectquery + " select sum(ConsumptionQty)as ConsumptionQty,ItemFK,CONVERT(varchar(20),dm.DailyConsDate,103) DailyConsDate,dd.RPU   from HT_DailyConsumptionMaster M, HT_DailyConsumptionDetail D,IM_ItemMaster i where m.DailyConsumptionMasterPK =d.DailyConsumptionMasterFK  and ItemFK=ItemPK and i.ItemCode='" + txtsearchitemcode.Text + "' and M.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by ItemFK,dm.DailyConsDate,dd.RPU ";
                        }
                        else if (txtsearchheadername.Text.Trim() != "")
                        {
                            selectquery = " select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName  ,sum(isnull(BalQty,0))as BalQty, sm.StorePK,sm.StoreName,i.ItemHeaderName,s.InwardRPU from IM_StoreMaster sm, IT_StockDetail s,IM_ItemMaster i where s.StoreFK  = sm.StorePK  and i.ItemPK=s.ItemFK and sm.StorePK  in('" + storecode + "') and i.itemheadername ='" + txtsearchheadername.Text + "'  and  ISNULL( BalQty,0) <>0 group by ItemPK,ItemCode,ItemName,StorePK,StoreName,i.ItemHeaderName,i.ItemUnit,s.InwardRPU order by i.ItemPK  ";
                            selectquery = selectquery + " select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK,CONVERT(varchar(20),dm.DailyConsDate,103) DailyConsDate,dd.RPU    from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and ItemFK=ItemPK and DailyConsDate  between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.itemheadername = '" + txtsearchheadername.Text + "' group by ItemFK,ConsumptionQty,dm.DailyConsDate,dd.RPU  ";
                        }
                        else
                        {
                            selectquery = " select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName  ,sum(isnull(BalQty,0))as BalQty, sm.StorePK ,sm.StoreName,i.ItemHeaderName,s.InwardRPU from IM_StoreMaster sm, IT_StockDetail s,IM_ItemMaster i where s.StoreFK  = sm.StorePK  and i.ItemPK=s.ItemFK and sm.StorePK  in('" + storecode + "') and  i.itempk in ('" + itemcode + "') and itemheadercode in ('" + itemheadercode + "')  and  ISNULL( BalQty,0) <>0 group by ItemPK,ItemCode,ItemName,StorePK,StoreName,i.ItemHeaderName,i.ItemUnit,s.InwardRPU order by i.ItemPK  ";
                            selectquery = selectquery + "  select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK,CONVERT(varchar(20),dm.DailyConsDate,103) DailyConsDate,dd.RPU    from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and i.ItemPK =dd.ItemFK and dm.DailyConsDate  between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.itempk  in ('" + itemcode + "')  group by ItemFK,dm.DailyConsDate,dd.RPU ";
                        }
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (cbl_storeb.Items.Count > 0)
                            {
                                for (int i = 0; i < cbl_storeb.Items.Count; i++)
                                {
                                    if (cbl_storeb.Items[i].Selected == true)
                                    {
                                        ds.Tables[0].DefaultView.RowFilter = "StorePK='" + Convert.ToString(cbl_storeb.Items[i].Value) + "'";
                                        DataView dv1 = ds.Tables[0].DefaultView;
                                        if (dv1.Count > 0)
                                        {
                                            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv1[0]["StoreName"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 10);
                                            for (int row = 0; row < dv1.Count; row++)
                                            {
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv1[row]["ItemHeaderName"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv1[row]["ItemCode"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv1[row]["itemname"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                                if (ds.Tables[1].Rows.Count > 0)
                                                {
                                                    ds.Tables[1].DefaultView.RowFilter = "ItemFK='" + Convert.ToString(dv1[row]["ItemPK"]) + "'";
                                                    dv = ds.Tables[1].DefaultView;

                                                    if (dv.Count > 0)
                                                    {
                                                        spanrow = dv.Count;
                                                        for (int c = 0; c < dv.Count; c++)
                                                        {

                                                            consume = Convert.ToString(dv[c]["ConsumptionQty"]);
                                                            date = Convert.ToString(dv[c]["DailyConsDate"]);
                                                            conrbu = Math.Round(Convert.ToDouble(dv[c]["RPU"]), 2);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = date;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = consume;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";


                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(conrbu);

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                                            spanrow2 = FpSpread1.Sheets[0].RowCount++;


                                                        }


                                                    }

                                                    else
                                                    {
                                                        consume = "0";
                                                        date = "-";
                                                        conrbu = 0;

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = date;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = consume;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";


                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(conrbu);

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                                    }
                                                    spanrow1 = spanrow2 - spanrow;
                                                    FpSpread1.Sheets[0].SpanModel.Add(spanrow1, 0, spanrow, 1);
                                                    FpSpread1.Sheets[0].SpanModel.Add(spanrow1, 1, spanrow, 1);
                                                    FpSpread1.Sheets[0].SpanModel.Add(spanrow1, 2, spanrow, 1);
                                                    FpSpread1.Sheets[0].SpanModel.Add(spanrow1, 3, spanrow, 1);
                                                    FpSpread1.Sheets[0].RowCount--;
                                                }

                                                else
                                                {
                                                    consume = "0";
                                                    date = "-";
                                                    conrbu = 0;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = date;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = consume;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(conrbu);

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                                }


                                            }
                                        }
                                    }
                                }
                                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                lblerror.Visible = false;
                                FpSpread1.Visible = true;
                                rptprint.Visible = true;
                                FpSpread1.SaveChanges();
                                txtsearchitemcode.Text = "";
                                txtsearchby.Text = "";
                                txtsearchheadername.Text = "";
                            }
                            else
                            {
                                lblerror.Visible = true;
                                lblerror.Text = "No Record Found";
                                FpSpread1.Visible = false;
                                rptprint.Visible = false;
                            }
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "No Record Found";
                            FpSpread1.Visible = false;
                            rptprint.Visible = false;
                        }
                        #endregion
                    }
                    else  
                    {
                        #region department
                        if (txtsearchby.Text.Trim() != "")
                        {
                            selectquery = "select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName ,sum(isnull(BalQty,0))as BalQty, d.Dept_Code,d.Dept_Name,i.ItemHeaderName,s.IssuedRPU from Department d, IT_StockDeptDetail s,IM_ItemMaster i where s.DeptFK = d.Dept_Code  and i.ItemPK=s.ItemFK and d.Dept_Code in('" + deptcode + "') and i.ItemName = '" + txtsearchby.Text + "' and  ISNULL( BalQty,0) <>0 group by ItemPK,ItemCode,ItemName,d.Dept_Code ,d.Dept_Name,i.ItemHeaderName,i.ItemUnit,s.IssuedRPU  order by i.ItemPK ";
                            selectquery = selectquery + " select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK,CONVERT(varchar(20),dm.DailyConsDate,103) DailyConsDate,dd.RPU   from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK  and ItemFK=ItemPK and i.ItemName = '" + txtsearchby.Text + "' and dm.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by ItemFK,ConsumptionQty,dm.DailyConsDate,dd.RPU  ";
                        }
                        else if (txtsearchitemcode.Text.Trim() != "")
                        {
                            selectquery = "select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName  ,sum(isnull(BalQty,0))as BalQty, d.Dept_Code,d.Dept_Name,i.ItemHeaderName,s.IssuedRPU from Department d, IT_StockDeptDetail s,IM_ItemMaster i where s.DeptFK = d.Dept_Code  and i.ItemPK=s.ItemFK and d.Dept_Code in('" + deptcode + "') and  ItemCode='" + txtsearchitemcode.Text + "'  and  ISNULL( BalQty,0) <>0 group by ItemPK,ItemCode,ItemName,d.Dept_Code ,d.Dept_Name,i.ItemHeaderName,i.ItemUnit,s.IssuedRPU  order by i.ItemPK ";
                            selectquery = selectquery + " select sum(ConsumptionQty)as ConsumptionQty,ItemFK,CONVERT(varchar(20),dm.DailyConsDate,103) DailyConsDate,dd.RPU  from HT_DailyConsumptionMaster M, HT_DailyConsumptionDetail D,IM_ItemMaster i where m.DailyConsumptionMasterPK =d.DailyConsumptionMasterFK  and ItemFK=ItemPK and i.ItemCode='" + txtsearchitemcode.Text + "' and M.DailyConsDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by ItemFK,dm.DailyConsDate,dd.RPU ";
                        }
                        else if (txtsearchheadername.Text.Trim() != "")
                        {
                            selectquery = "select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName  ,sum(isnull(BalQty,0))as BalQty, d.Dept_Code,d.Dept_Name,i.ItemHeaderName,s.IssuedRPU from Department d, IT_StockDeptDetail s,IM_ItemMaster i where s.DeptFK = d.Dept_Code  and i.ItemPK=s.ItemFK and d.Dept_Code in('" + deptcode + "') and i.itemheadername ='" + txtsearchheadername.Text + "'  and  ISNULL( BalQty,0) <>0 group by ItemPK,ItemCode,ItemName,d.Dept_Code ,d.Dept_Name,i.ItemHeaderName,i.ItemUnit,s.IssuedRPU  order by i.ItemPK ";
                            selectquery = selectquery + " select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK,CONVERT(varchar(20),dm.DailyConsDate,103) DailyConsDate,dd.RPU   from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and ItemFK=ItemPK and DailyConsDate  between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.itemheadername = '" + txtsearchheadername.Text + "' group by ItemFK,ConsumptionQty,dm.DailyConsDate,dd.RPU  ";
                        }
                        else
                        {
                            selectquery = "select distinct i.ItemPK,i.ItemCode ,i.ItemName +'('+i.ItemUnit+')' as ItemName  ,sum(isnull(BalQty,0))as BalQty, d.Dept_Code,d.Dept_Name,i.ItemHeaderName,s.IssuedRPU from Department d, IT_StockDeptDetail s,IM_ItemMaster i where s.DeptFK = d.Dept_Code  and i.ItemPK=s.ItemFK and d.Dept_Code in('" + deptcode + "') and  i.ItemPK in ('" + itemcode + "') and itemheadercode in ('" + itemheadercode + "')   and  ISNULL( BalQty,0) <>0  group by ItemPK,ItemCode,ItemName,d.Dept_Code ,d.Dept_Name,i.ItemHeaderName,i.ItemUnit,s.IssuedRPU  order by i.ItemPK ";
                            selectquery = selectquery + "  select sum(ConsumptionQty ) as ConsumptionQty ,ItemFK,CONVERT(varchar(20),dm.DailyConsDate,103) DailyConsDate,dd.RPU  from HT_DailyConsumptionMaster dm,HT_DailyConsumptionDetail dd,IM_ItemMaster i where dm.DailyConsumptionMasterPK =dd.DailyConsumptionMasterFK and i.ItemPK =dd.ItemFK and dm.DailyConsDate  between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.ItemPK  in ('" + itemcode + "')  group by ItemFK,dm.DailyConsDate,dd.RPU";
                        }
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (cbl_deptname.Items.Count > 0)
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
                                            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 10);
                                            for (int row = 0; row < dv1.Count; row++)
                                            {
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv1[row]["ItemHeaderName"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv1[row]["ItemCode"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv1[row]["itemname"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                              if (ds.Tables[1].Rows.Count > 0)
                                                {
                                                    ds.Tables[1].DefaultView.RowFilter = "ItemFK='" + Convert.ToString(dv1[row]["ItemPK"]) + "'";
                                                    dv = ds.Tables[1].DefaultView;

                                                    if (dv.Count > 0)
                                                    {
                                                        spanrow = dv.Count;
                                                        for (int c = 0; c < dv.Count; c++)
                                                        {
                                                            consume = Convert.ToString(dv[c]["ConsumptionQty"]);
                                                            date = Convert.ToString(dv[c]["DailyConsDate"]);
                                                            conrbu = Math.Round(Convert.ToDouble(dv[c]["RPU"]), 2);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = date;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = consume;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";


                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(conrbu);

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                                            spanrow2 = FpSpread1.Sheets[0].RowCount++;

                                                          
                                                        }
                                                     

                                                    }

                                                    else
                                                    {
                                                        consume = "0";
                                                        date = "-";
                                                        conrbu = 0;

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = date;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = consume;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(conrbu);

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                                    }
                                                    spanrow1 = spanrow2 - spanrow;
                                                    FpSpread1.Sheets[0].SpanModel.Add(spanrow1, 0, spanrow, 1);
                                                    FpSpread1.Sheets[0].SpanModel.Add(spanrow1, 1, spanrow, 1);
                                                    FpSpread1.Sheets[0].SpanModel.Add(spanrow1, 2, spanrow, 1);
                                                    FpSpread1.Sheets[0].SpanModel.Add(spanrow1, 3, spanrow, 1);
                                                    FpSpread1.Sheets[0].RowCount--;
                                                }
                                                     
                                                else
                                                {
                                                    consume = "0";
                                                    date = "-";
                                                    conrbu = 0;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = date;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = consume;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Right;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(conrbu);

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                                }


                                            }
                                        }
                                    }
                                }
                                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                lblerror.Visible = false;
                                FpSpread1.Visible = true;
                                FpSpread1.SaveChanges();
                                rptprint.Visible = true;
                            }
                            else
                            {
                                lblerror.Visible = true;
                                lblerror.Text = "No Record Found";
                                FpSpread1.Visible = false;
                                rptprint.Visible = false;
                            }
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "No Record Found";
                            FpSpread1.Visible = false;
                            rptprint.Visible = false;
                        }
                        #endregion
                    }

                }

                if (FpSpread1.Sheets[0].ColumnCount > 0)
                {
                    for (int m = 0; m < FpSpread1.Sheets[0].ColumnCount; m++)
                    {
                        FpSpread1.Columns[m].Locked = true;
                    }
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select All Field";
                FpSpread1.Visible = false;
                rptprint.Visible = false;
            }
            txtsearchitemcode.Text = "";
            txtsearchby.Text = "";
            txtsearchheadername.Text = "";
        }
        catch
        {
        }

    }


    #endregion




}