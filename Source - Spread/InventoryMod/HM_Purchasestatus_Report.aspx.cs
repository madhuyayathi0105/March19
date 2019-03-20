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

public partial class HM_Purchasestatus_Report : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();

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
                lblerror.Visible = false;
                bindhostelname();
                vendor();
                item();
                txtfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtto.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtfrom.Attributes.Add("readonly", "readonly");
                txtto.Attributes.Add("readonly", "readonly");

                btngoclick(sender, e);
            }
            lblerror.Visible = false;
            lblvalidation1.Visible = false;
        }
        catch (Exception ex) { }
    }

    public void bindhostelname()
    {
        try
        {
            ddlhos.Items.Clear();
            string selecthostel = "select HostelMasterPK,HostelName from HM_HostelMaster where CollegeCode='" + collegecode1 + "' order by HostelMasterPK";
            ds.Clear();
            ds = da.select_method_wo_parameter(selecthostel, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlhos.DataSource = ds;
                ddlhos.DataTextField = "HostelName";
                ddlhos.DataValueField = "HostelMasterPK";
                ddlhos.DataBind();
            }

        }
        catch
        {

        }
    }

    public void vendor()
    {
        Cblven.Items.Clear();
        string vendor = "select distinct VendorPK,VendorCompName from CO_VendorMaster  where VendorType=1 order by VendorPK ";
        ds = da.select_method_wo_parameter(vendor, "Text");
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cblven.DataSource = ds;
                Cblven.DataTextField = "VendorCompName";
                Cblven.DataValueField = "VendorPK";
                Cblven.DataBind();
                if (Cblven.Items.Count > 0)
                {
                    for (int i = 0; i < Cblven.Items.Count; i++)
                    {

                        Cblven.Items[i].Selected = true;
                    }
                    Chkven.Checked = true;

                    txtvenname.Text = "Vendor(" + Cblven.Items.Count + ")";
                }
            }
        }
        else
        {
            txtvenname.Text = "--Select--";
        }
    }

    public void item()
    {
        Cblitm.Items.Clear();
        string deptquery = "";
        string buildvalue = "";

        for (int i = 0; i < Cblven.Items.Count; i++)
        {
            if (Cblven.Items[i].Selected == true)
            {
                string build = Cblven.Items[i].Value.ToString();
                if (buildvalue == "")
                {
                    buildvalue = build;
                }
                else
                {
                    buildvalue = buildvalue + "'" + "," + "'" + build;
                }
            }
        }
        deptquery = " select distinct i.ItemPK,ItemName from CO_VendorMaster v, IM_VendorItemDept vi, IM_ItemMaster i where v.VendorPK =vi.VenItemFK and vi.ItemFK =i.ItemPK and v.VendorPK in('" + buildvalue + "')";

        ds = da.select_method_wo_parameter(deptquery, "Text");
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cblitm.DataSource = ds;
                Cblitm.DataTextField = "ItemName";
                Cblitm.DataValueField = "ItemPK";
                Cblitm.DataBind();

                if (Cblitm.Items.Count > 0)
                {
                    for (int i = 0; i < Cblitm.Items.Count; i++)
                    {

                        Cblitm.Items[i].Selected = true;
                    }
                    Chkitm.Checked = true;
                    txtitmname.Text = "Items(" + Cblitm.Items.Count + ")";
                }
            }
        }
        else
        {
            txtitmname.Text = "--Select--";
        }
    }

    protected void Chkitmname(object sender, EventArgs e)
    {
        int cout = 0;
        txtitmname.Text = "---Select---";
        if (Chkitm.Checked == true)
        {
            cout++;
            for (int i = 0; i < Cblitm.Items.Count; i++)
            {
                Cblitm.Items[i].Selected = true;
            }
            txtitmname.Text = "Item(" + (Cblitm.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < Cblitm.Items.Count; i++)
            {
                Cblitm.Items[i].Selected = false;
            }
        }
    }

    protected void Cblitmname(object sender, EventArgs e)
    {
        int i = 0;
        Chkitm.Checked = false;
        int commcount = 0;
        txtitmname.Text = "--Select--";
        for (i = 0; i < Cblitm.Items.Count; i++)
        {
            if (Cblitm.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                Chkitm.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == Cblitm.Items.Count)
            {
                Chkitm.Checked = true;
            }
            txtitmname.Text = "Item(" + commcount.ToString() + ")";
        }
    }


    protected void Chksechosname(object sender, EventArgs e)
    {
        try
        {
            if (Chkven.Checked == true)
            {
                for (int i = 0; i < Cblven.Items.Count; i++)
                {
                    Cblven.Items[i].Selected = true;
                }
                txtvenname.Text = "vendor(" + (Cblven.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cblven.Items.Count; i++)
                {
                    Cblven.Items[i].Selected = false;
                }
                txtvenname.Text = "--Select--";
            }
            item();
        }
        catch
        {
        }
    }

    protected void Cblsechosname(object sender, EventArgs e)
    {
        int i = 0;
        Chkven.Checked = false;
        item();
        int commcount = 0;
        txtvenname.Text = "--Select--";
        for (i = 0; i < Cblven.Items.Count; i++)
        {
            if (Cblven.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                Chkven.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == Cblven.Items.Count)
            {
                Chkven.Checked = true;
            }
            txtvenname.Text = "Vendor(" + commcount.ToString() + ")";
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
    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    protected void btngoclick(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            string hostelname = "";
            for (int i = 0; i < ddlhos.Items.Count; i++)
            {
                if (ddlhos.Items.Count > 0)
                {
                    hostelname = ddlhos.SelectedItem.Text;
                }
            }

            string vendor = "";
            for (int i = 0; i < Cblven.Items.Count; i++)
            {
                if (Cblven.Items[i].Selected == true)
                {
                    if (vendor == "")
                    {
                        vendor = "" + Cblven.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        vendor = vendor + "'" + "," + "'" + Cblven.Items[i].Value.ToString() + "";
                    }
                }
            }
            string itemname = "";
            for (int i = 0; i < Cblitm.Items.Count; i++)
            {
                if (Cblitm.Items[i].Selected == true)
                {
                    if (itemname == "")
                    {
                        itemname = "" + Cblitm.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemname = itemname + "'" + "," + "'" + Cblitm.Items[i].Value.ToString() + "";
                    }
                }
            }
            string firstdate = Convert.ToString(txtfrom.Text);
            string seconddate = Convert.ToString(txtto.Text);
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            DataView dv = new DataView();
            if (vendor.Trim() != "" && itemname.Trim() != "")
            {
                // string q = "select distinct  v.vendor_code, item_name,i.item_code,i.item_unit,(app_qty-isnull(waiting_app_qty,0)) as app_qty,(app_qty-isnull(waiting_app_qty,0))- ISNULL(inward_qty,0) as rej_qty,inward_qty,rpu ,(rpu * inward_qty)as value,gi_date from item_master i,vendor_details v,Vendor_ItemDetails vi ,purchase_order p,purchaseorder_items pi,goods_inward g,goodsinward_items gi where v.vendor_code =vi.Vendor_Code and i.item_code =vi.Item_Code and p.order_code =pi.order_code and pi.item_code =i.item_code and pi.item_code =vi.Item_Code and vi.Vendor_Code =pi.vendor_code and pi.vendor_code =v.vendor_code and order_approval ='Approved' and goods_in ='1' and g.gi_code =gi.gi_code and gi.item_code =i.item_code and gi.item_code =vi.Item_Code and gi.item_code =pi.item_code and g.Order_Code =p.order_code and v.vendor_code in ('" + vendor + "') and i.item_code in ('" + itemname + "') and g.gi_date between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' order by v.vendor_code, i.item_code";

                string q = " select p.OrderCode,i.ItemPK,i.ItemName,i.ItemCode,i.ItemUnit,(pi.RPU*g.InwardQty) as value,pi.RPU,convert(varchar(10),g.GoodsInwardDate,103) as GoodsInwardDate,v.VendorPK,(pi.AppQty -ISNULL(pi.RejQty,0))as appQty,g.InwardQty,((pi.AppQty -ISNULL(pi.RejQty,0))-InwardQty) as Pending from IM_ItemMaster i,CO_VendorMaster v,IT_PurchaseOrder p,IT_PurchaseOrderDetail pi,IT_GoodsInward g where v.VendorPK =p.VendorFK and p.PurchaseOrderPK =pi.PurchaseOrderFK and i.ItemPK =pi.ItemFK and g.PurchaseOrderFK =p.PurchaseOrderPK and g.itemfk =i.ItemPK and ApproveStatus ='1' and InwardStatus ='1' and g.GoodsInwardDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and i.ItemPK in ('" + itemname + "') and v.VendorPK in ('" + vendor + "')  order by OrderCode ";
                ds = da.select_method_wo_parameter(q, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    spreaddiv.Visible = true;
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].AutoPostBack = true;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 9;
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

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[1].Width = 100;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[2].Width = 200;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Measure";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[3].Width = 100;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Rate Per Unit";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[4].Width = 100;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Approved Quantity";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[5].Width = 100;
                    FpSpread1.Columns[5].ForeColor = Color.Blue;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Received Quantity";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[6].Width = 100;
                    FpSpread1.Columns[6].ForeColor = Color.Green;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Pending Quantity";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[7].Width = 100;
                    FpSpread1.Columns[7].ForeColor = Color.Red;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Total Value";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[8].Width = 100;
                    int sno = 0;
                    for (int ir = 0; ir < Cblven.Items.Count; ir++)
                    {
                        if (Cblven.Items[ir].Selected == true)
                        {
                            split = firstdate.Split('/');
                            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                            split = seconddate.Split('/');
                            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

                            while (dt <= dt1)
                            {
                                ds.Tables[0].DefaultView.RowFilter = "VendorPK='" + Cblven.Items[ir].Value + "' and GoodsInwardDate='" + dt.ToString("dd/MM/yyyy") + "' ";
                                dv = ds.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(Cblven.Items[ir].Text) + " - " + Convert.ToString(dt.ToString("dd/MM/yyyy"));
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount);
                                    for (int rd = 0; rd < dv.Count; rd++)
                                    {
                                        sno++;
                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[rd]["ItemCode"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[rd]["ItemName"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[rd]["ItemUnit"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[rd]["RPU"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[rd]["appQty"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dv[rd]["InwardQty"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dv[rd]["Pending"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                        if (Convert.ToString(dv[rd]["Pending"]) != "0")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.Red;
                                        }

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dv[rd]["value"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                    }
                                }
                                dt = dt.AddDays(1);
                            }
                        }
                    }

                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    spreaddiv.Visible = true;
                    lblerror.Visible = false;
                    rptprint.Visible = true;
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "No Record Found";
                    spreaddiv.Visible = false;
                    rptprint.Visible = false;
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select All Fields";
                spreaddiv.Visible = false;
                rptprint.Visible = false;
            }
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
                da.printexcelreport(FpSpread1, reportname);
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
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Purchase Status Report";
            string pagename = "HM_Purchasestatus_Report.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
}